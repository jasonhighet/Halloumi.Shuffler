using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;

namespace Halloumi.Shuffler.Controls
{
    /// <summary>
    ///     The Mixer view's waveform strip. The top row is the current track; the bottom row holds the end of the
    ///     previous track (under the current track's fade-in, while it plays out) and the next track (under the
    ///     fade-out). Each is shown as heard (loops repeated, skip section cut out) with the fade sections shaded.
    ///     In manual mix mode the coming mix's fades can be dragged, a whole section at a time. The moves are kept
    ///     if the manual mix is saved as an extended mix. See docs/mixer-waveform.md.
    /// </summary>
    public class MixOverviewControl : Control
    {
        private const int LabelHeight = 18;
        private const int RowGap = 4;

        private const TextFormatFlags LabelFlags = TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis |
                                                   TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;

        private static readonly Color BackgroundColor = Color.FromArgb(24, 24, 24);
        private static readonly Color LabelColor = Color.FromArgb(220, 220, 220);
        private static readonly Color DimLabelColor = Color.FromArgb(140, 140, 140);
        private static readonly Color WarningLabelColor = Color.FromArgb(255, 200, 120);
        private static readonly Color BodyWaveColor = Color.FromArgb(170, 170, 170);
        private static readonly Color FadeInWaveColor = Color.FromArgb(120, 180, 255);
        private static readonly Color FadeOutWaveColor = Color.FromArgb(255, 180, 90);
        private static readonly Color UnheardWaveColor = Color.FromArgb(85, 85, 85);
        private static readonly Color FadeInShade = Color.FromArgb(60, 60, 140, 230);
        private static readonly Color PreFadeInShade = Color.FromArgb(30, 60, 140, 230);
        private static readonly Color FadeOutShade = Color.FromArgb(60, 240, 140, 40);
        private static readonly Color PlayedShade = Color.FromArgb(130, 0, 0, 0);
        private static readonly Color RepeatLineColor = Color.FromArgb(150, 255, 255, 255);
        private static readonly Color GhostFill = Color.FromArgb(50, 255, 255, 255);

        private readonly Timer _timer = new Timer { Interval = 66 };
        private readonly Dictionary<string, WaveformPeaks> _peaks = new Dictionary<string, WaveformPeaks>();
        private readonly HashSet<string> _loadingPeaks = new HashSet<string>();
        private readonly RowBitmap _topRow = new RowBitmap();
        private readonly RowBitmap _bottomRow = new RowBitmap();
        private readonly ContextMenuStrip _menu = new ContextMenuStrip();
        private readonly ToolStripMenuItem _mnuRevert = new ToolStripMenuItem("Back to the extended mix's fades");
        private readonly ToolStripMenuItem _mnuReset = new ToolStripMenuItem("Reset to the tracks' own fades");

        private MixOverview _overview;

        // a fade being dragged: which one, where the mouse went down, and the section count it would land on
        private DragTarget _drag = DragTarget.None;
        private int _dragStartX;
        private int _dragSections;

        public MixOverviewControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            BackColor = BackgroundColor;
            _timer.Tick += Timer_Tick;

            _menu.Items.AddRange(new ToolStripItem[] { _mnuRevert, _mnuReset });
            _mnuRevert.Click += (s, e) => RevertToSaved();
            _mnuReset.Click += (s, e) => ResetMoves();
        }

        private enum DragTarget
        {
            None,
            FadeOut,
            FadeIn
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ShufflerApplication Application { get; set; }

        public void Initialize()
        {
            _timer.Start();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer.Stop();
                _timer.Dispose();
                _menu.Dispose();
                _topRow.Dispose();
                _bottomRow.Dispose();
            }

            base.Dispose(disposing);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (Application == null || !Visible) return;
            RefreshOverview();
        }

        private void RefreshOverview()
        {
            try
            {
                _overview = Application.GetMixOverview();
            }
            catch (Exception)
            {
                // the slots are read without a lock and can change mid-read; try again next tick
                return;
            }

            // the fade being dragged has started, or the tracks changed: drop the drag
            if (_drag != DragTarget.None && GetMove(_drag)?.CanMove != true)
                CancelDrag();

            EnsurePeaks(_overview.Current?.Filename);
            EnsurePeaks(_overview.Previous?.Filename);
            EnsurePeaks(_overview.Next?.Filename);
            Invalidate();
        }

        /// <summary>
        ///     Starts decoding a track's peaks in the background if they are not loaded or loading already.
        /// </summary>
        private void EnsurePeaks(string filename)
        {
            if (string.IsNullOrEmpty(filename)) return;
            if (_peaks.ContainsKey(filename) || _loadingPeaks.Contains(filename)) return;

            _loadingPeaks.Add(filename);
            var application = Application;
            Task.Run(() => application.GetWaveformPeaks(filename)).ContinueWith(task =>
            {
                var peaks = task.IsFaulted ? null : task.Result;
                if (IsDisposed || !IsHandleCreated) return;
                BeginInvoke(new MethodInvoker(() => PeaksLoaded(filename, peaks)));
            });
        }

        private void PeaksLoaded(string filename, WaveformPeaks peaks)
        {
            _loadingPeaks.Remove(filename);

            // keep only the tracks still on show (a null entry means the file could not be decoded)
            var shown = new[] { _overview?.Previous?.Filename, _overview?.Current?.Filename, _overview?.Next?.Filename };
            foreach (var old in _peaks.Keys.Where(x => !shown.Contains(x)).ToList())
                _peaks.Remove(old);

            _peaks[filename] = peaks;
            Invalidate();
        }

        // ---------- moving fades ----------

        private MixOverviewFadeMove GetMove(DragTarget target)
        {
            switch (target)
            {
                case DragTarget.FadeOut: return _overview?.FadeOutMove;
                case DragTarget.FadeIn: return _overview?.FadeInMove;
                default: return null;
            }
        }

        /// <summary>
        ///     The fade under the mouse, if it can be moved: the current track's fade-out on the top row, the next
        ///     track's fade-in on the bottom row.
        /// </summary>
        private DragTarget HitTest(Point point)
        {
            var layout = RowLayout.For(this, _overview);
            if (layout == null) return DragTarget.None;

            if (layout.TopWave.Contains(point) && _overview.FadeOutMove?.CanMove == true
                && IsInSegments(layout, layout.TopWave, _overview.Current, MixOverviewSegmentKind.FadeOut, point.X))
                return DragTarget.FadeOut;

            if (layout.BottomWave.Contains(point) && _overview.FadeInMove?.CanMove == true
                && IsInSegments(layout, layout.BottomWave, _overview.Next, MixOverviewSegmentKind.FadeIn, point.X))
                return DragTarget.FadeIn;

            return DragTarget.None;
        }

        private static bool IsInSegments(RowLayout layout, Rectangle wave, MixOverviewTrack track,
            MixOverviewSegmentKind kind, int x)
        {
            if (track == null) return false;

            var heard = track.HeardStart;
            foreach (var segment in track.Segments)
            {
                var x0 = wave.Left + layout.Axis.ToX(track.Offset + heard, wave.Width);
                var x1 = wave.Left + layout.Axis.ToX(track.Offset + heard + segment.Length, wave.Width);
                heard += segment.Length;
                if (segment.Kind == kind && x >= x0 && x <= x1) return true;
            }

            return false;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Right)
            {
                if (_drag != DragTarget.None)
                {
                    CancelDrag();
                    return;
                }

                ShowMenu(e.Location);
                return;
            }

            if (e.Button != MouseButtons.Left) return;

            var target = HitTest(e.Location);
            if (target == DragTarget.None) return;

            _drag = target;
            _dragStartX = e.X;
            _dragSections = GetMove(target).Shift;
            Capture = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_drag == DragTarget.None)
            {
                Cursor = HitTest(e.Location) != DragTarget.None ? Cursors.SizeWE : Cursors.Default;
                return;
            }

            var move = GetMove(_drag);
            var layout = RowLayout.For(this, _overview);
            if (move == null || layout == null || move.SectionLength <= 0) return;

            // snap to whole sections, within the range the engine allows
            var seconds = (e.X - _dragStartX) / (double)Math.Max(1, layout.TopWave.Width) * layout.Axis.Length;
            var sections = move.Shift + (int)Math.Round(seconds / move.SectionLength);
            sections = Math.Max(move.Min, Math.Min(move.Max, sections));

            if (sections == _dragSections) return;
            _dragSections = sections;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button != MouseButtons.Left || _drag == DragTarget.None) return;

            var target = _drag;
            var sections = _dragSections;
            CancelDrag();

            var move = GetMove(target);
            if (move == null || sections == move.Shift) return;

            if (target == DragTarget.FadeOut) Application.MoveFadeOut(sections);
            else Application.MoveFadeIn(sections);

            RefreshOverview();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_drag == DragTarget.None) Cursor = Cursors.Default;
        }

        private void CancelDrag()
        {
            _drag = DragTarget.None;
            Capture = false;
            Cursor = Cursors.Default;
            Invalidate();
        }

        private void ShowMenu(Point location)
        {
            var overview = _overview;
            var fadeOut = overview?.FadeOutMove;
            var fadeIn = overview?.FadeInMove;
            if (fadeOut == null || fadeIn == null || !overview.IsManualMixMode) return;

            var isMoved = fadeOut.Shift != 0 || fadeIn.Shift != 0;
            _mnuRevert.Enabled = overview.HasUnsavedMoves && overview.HasSavedMoves && CanMoveTo(fadeOut, fadeOut.SavedShift)
                                 && CanMoveTo(fadeIn, fadeIn.SavedShift);
            _mnuReset.Enabled = isMoved && CanMoveTo(fadeOut, 0) && CanMoveTo(fadeIn, 0);
            _menu.Show(this, location);
        }

        /// <summary>
        ///     True if a fade is already at a count, or can move there.
        /// </summary>
        private static bool CanMoveTo(MixOverviewFadeMove move, int sections)
        {
            return move.Shift == sections || move.CanMove && sections >= move.Min && sections <= move.Max;
        }

        private void RevertToSaved()
        {
            var overview = _overview;
            if (overview?.FadeOutMove == null || overview.FadeInMove == null) return;

            if (overview.FadeOutMove.Shift != overview.FadeOutMove.SavedShift)
                Application.MoveFadeOut(overview.FadeOutMove.SavedShift);
            if (overview.FadeInMove.Shift != overview.FadeInMove.SavedShift)
                Application.MoveFadeIn(overview.FadeInMove.SavedShift);
            RefreshOverview();
        }

        private void ResetMoves()
        {
            var overview = _overview;
            if (overview?.FadeOutMove == null || overview.FadeInMove == null) return;

            if (overview.FadeOutMove.Shift != 0) Application.MoveFadeOut(0);
            if (overview.FadeInMove.Shift != 0) Application.MoveFadeIn(0);
            RefreshOverview();
        }

        // ---------- painting ----------

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(BackgroundColor);

            var overview = _overview;
            var layout = RowLayout.For(this, overview);
            if (layout == null)
            {
                if (ClientSize.Height > LabelHeight)
                    DrawLabel(g, new Rectangle(0, 0, ClientSize.Width, LabelHeight), 0, "Nothing playing",
                        DimLabelColor);
                return;
            }

            var axis = layout.Axis;
            var current = overview.Current;
            var previous = overview.Previous;
            var next = overview.Next;

            // top row: the current track
            DrawLabel(g, layout.TopRow, 0, "Now: " + current.Description, LabelColor);
            DrawStatus(g, layout.TopRow, overview);
            DrawRow(g, layout.TopWave, axis, _topRow, current);

            // bottom row: the end of the previous track under the fade-in, the next track under the fade-out
            DrawRow(g, layout.BottomWave, axis, _bottomRow, previous, next);

            if (previous != null)
                DrawLabel(g, layout.BottomRow, axis.ToX(previous.Offset + previous.AudibleStart, layout.BottomRow.Width),
                    "Previous: " + previous.Description, LabelColor);

            if (next != null)
                DrawLabel(g, layout.BottomRow, axis.ToX(next.Offset + next.AudibleStart, layout.BottomRow.Width),
                    "Next: " + next.Description, LabelColor);
            else
                DrawLabel(g, layout.BottomRow, axis.ToX(current.FadeOutHeardStart, layout.BottomRow.Width),
                    "Next: nothing queued", DimLabelColor);

            DrawGhost(g, layout, overview);
        }

        /// <summary>
        ///     While a fade is dragged, outlines where it would land, over the audio it would play.
        /// </summary>
        private void DrawGhost(Graphics g, RowLayout layout, MixOverview overview)
        {
            if (_drag == DragTarget.None) return;

            var move = GetMove(_drag);
            var track = _drag == DragTarget.FadeOut ? overview.Current : overview.Next;
            var wave = _drag == DragTarget.FadeOut ? layout.TopWave : layout.BottomWave;
            if (move == null || track == null || _dragSections == move.Shift) return;

            var fileStart = move.FileStart + (_dragSections - move.Shift) * move.SectionLength;
            var start = track.HeardTimeOf(fileStart);
            var end = track.HeardTimeOf(fileStart + move.SectionLength);
            if (start == null || end == null) return;

            var x0 = wave.Left + layout.Axis.ToX(track.Offset + start.Value, wave.Width);
            var x1 = wave.Left + layout.Axis.ToX(track.Offset + end.Value, wave.Width);
            var rect = new Rectangle(x0, wave.Top, Math.Max(2, x1 - x0), wave.Height - 1);

            using (var fill = new SolidBrush(GhostFill))
                g.FillRectangle(fill, rect);
            using (var pen = new Pen(Color.White, 2) { DashStyle = DashStyle.Dash })
                g.DrawRectangle(pen, rect);

            var steps = _dragSections - move.Shift;
            var text = (steps > 0 ? "+" : "") + steps;
            TextRenderer.DrawText(g, text, Font, new Rectangle(rect.Left + 3, rect.Top + 2, 60, LabelHeight),
                Color.White, LabelFlags | TextFormatFlags.Left);
        }

        /// <summary>
        ///     The top-right status: time to the mix, and whether the mix's fades are moved, saved or locked.
        /// </summary>
        private void DrawStatus(Graphics g, Rectangle rowRect, MixOverview overview)
        {
            var parts = new List<string>();
            var color = LabelColor;

            var mixIn = GetMixInText(overview);
            if (mixIn != null) parts.Add(mixIn);

            if (overview.IsManualMixMode && overview.HasUnsavedMoves)
            {
                parts.Add("fades moved: kept if you save the mix");
                color = WarningLabelColor;
            }
            else if (overview.IsRecordedMix)
            {
                parts.Add(overview.HasSavedMoves ? "extended mix, fades moved" : "extended mix");
            }

            if (parts.Count == 0) return;
            var labelRect = new Rectangle(rowRect.Left + 4, rowRect.Top, rowRect.Width - 8, LabelHeight);
            TextRenderer.DrawText(g, string.Join("  ·  ", parts), Font, labelRect, color,
                LabelFlags | TextFormatFlags.Right);
        }

        private static string GetMixInText(MixOverview overview)
        {
            var current = overview.Current;
            if (overview.Next == null || !current.HasPosition) return null;

            var remaining = current.FadeOutHeardStart - current.Position;
            if (remaining < 0) return null;
            var time = TimeSpan.FromSeconds(Math.Floor(remaining));
            return "mix in " + (time.TotalHours >= 1 ? time.ToString(@"h\:mm\:ss") : time.ToString(@"m\:ss"));
        }

        /// <summary>
        ///     Draws a label starting at x (where its track starts), moved left if needed so it fits on the row.
        /// </summary>
        private void DrawLabel(Graphics g, Rectangle rowRect, int x, string text, Color color)
        {
            var width = TextRenderer.MeasureText(g, text, Font).Width + 8;
            var left = rowRect.Left + Math.Max(4, Math.Min(x + 4, rowRect.Width - width));
            var labelRect = new Rectangle(left, rowRect.Top, Math.Max(0, rowRect.Right - left), LabelHeight);
            TextRenderer.DrawText(g, text, Font, labelRect, color, LabelFlags | TextFormatFlags.Left);
        }

        /// <summary>
        ///     Draws a row's waveforms (cached until a track, its fades, the axis or the size change), then dims the
        ///     part of each track already played and draws its playhead.
        /// </summary>
        private void DrawRow(Graphics g, Rectangle waveRect, Axis axis, RowBitmap cache,
            params MixOverviewTrack[] tracks)
        {
            var key = string.Join("#", tracks.Select(GetTrackKey)) + "|" + axis.Start + "|" + axis.End + "|" +
                      waveRect.Width + "x" + waveRect.Height;
            if (cache.Key != key)
                cache.Set(key, RenderRow(tracks, axis, waveRect.Size));

            g.DrawImageUnscaled(cache.Bitmap, waveRect.Location);

            foreach (var track in tracks)
            {
                if (track == null || !track.HasPosition) continue;

                var start = waveRect.Left + axis.ToX(track.Offset + track.AudibleStart, waveRect.Width);
                var end = waveRect.Left + axis.ToX(track.Offset + track.HeardEnd, waveRect.Width);
                var x = waveRect.Left + axis.ToX(track.Offset + track.Position, waveRect.Width);
                x = Math.Max(start, Math.Min(end, x));

                using (var played = new SolidBrush(PlayedShade))
                    g.FillRectangle(played, start, waveRect.Top, x - start, waveRect.Height);
                g.DrawLine(Pens.White, x, waveRect.Top, x, waveRect.Bottom - 1);
            }
        }

        private string GetTrackKey(MixOverviewTrack track)
        {
            if (track == null) return "";
            _peaks.TryGetValue(track.Filename ?? "", out var peaks);
            var segments = string.Join(";", track.Segments.Select(s => s.Kind + ":" + s.FileStart + "-" + s.FileEnd));
            return string.Join("|", track.Filename, track.Offset, track.HeardStart, segments, peaks != null);
        }

        private Bitmap RenderRow(MixOverviewTrack[] tracks, Axis axis, Size size)
        {
            var bitmap = new Bitmap(Math.Max(1, size.Width), Math.Max(1, size.Height));
            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(BackgroundColor);

                var left = 0;
                for (var i = 0; i < tracks.Length; i++)
                {
                    var track = tracks[i];
                    if (track == null) continue;

                    // a track stops where the next one on the row is first heard (the previous track's tail can be
                    // long), and the next one's unheard intro only fills what is left
                    var right = size.Width;
                    var following = tracks.Skip(i + 1).FirstOrDefault(t => t != null);
                    if (following != null)
                        right = Math.Min(right, axis.ToX(following.Offset + following.AudibleStart, size.Width));

                    _peaks.TryGetValue(track.Filename ?? "", out var peaks);
                    var drawnTo = RenderTrack(g, track, axis, size, left, right, peaks);
                    left = Math.Max(left, drawnTo);
                }
            }

            return bitmap;
        }

        /// <summary>
        ///     Draws one track's segments between two x positions; returns the x it drew up to.
        /// </summary>
        private static int RenderTrack(Graphics g, MixOverviewTrack track, Axis axis, Size size, int left, int right,
            WaveformPeaks peaks)
        {
            var middle = size.Height / 2;
            var halfHeight = Math.Max(1, middle - 2);
            var heard = track.HeardStart;
            var drawnTo = left;

            foreach (var segment in track.Segments)
            {
                var x0 = axis.ToX(track.Offset + heard, size.Width);
                var x1 = axis.ToX(track.Offset + heard + segment.Length, size.Width);
                heard += segment.Length;
                if (x1 <= x0) continue;

                var secondsPerPixel = segment.Length / (x1 - x0);
                var from = Math.Max(x0, left);
                var to = Math.Min(x1, right);
                if (to <= from) continue;
                drawnTo = Math.Max(drawnTo, to);

                var shade = GetShade(segment.Kind);
                if (shade != Color.Empty)
                    using (var brush = new SolidBrush(shade))
                        g.FillRectangle(brush, from, 0, to - from, size.Height);

                using (var pen = new Pen(GetWaveColor(segment.Kind)))
                {
                    if (peaks == null)
                    {
                        g.DrawLine(pen, from, middle, to, middle);
                    }
                    else
                    {
                        for (var x = from; x < to; x++)
                        {
                            var start = segment.FileStart + (x - x0) * secondsPerPixel;
                            var height = (int)(peaks.GetPeak(start, start + secondsPerPixel) * halfHeight);
                            g.DrawLine(pen, x, middle - height, x, middle + height);
                        }
                    }
                }

                // a dashed line where a looped section starts again
                if (segment.Repeat > 0 && x0 >= left)
                    using (var pen = new Pen(RepeatLineColor) { DashStyle = DashStyle.Dash })
                        g.DrawLine(pen, x0, 0, x0, size.Height - 1);
            }

            return drawnTo;
        }

        private static Color GetShade(MixOverviewSegmentKind kind)
        {
            switch (kind)
            {
                case MixOverviewSegmentKind.PreFadeIn: return PreFadeInShade;
                case MixOverviewSegmentKind.FadeIn: return FadeInShade;
                case MixOverviewSegmentKind.FadeOut: return FadeOutShade;
                default: return Color.Empty;
            }
        }

        private static Color GetWaveColor(MixOverviewSegmentKind kind)
        {
            switch (kind)
            {
                case MixOverviewSegmentKind.PreFadeIn:
                case MixOverviewSegmentKind.FadeIn:
                    return FadeInWaveColor;
                case MixOverviewSegmentKind.FadeOut:
                    return FadeOutWaveColor;
                case MixOverviewSegmentKind.Intro:
                case MixOverviewSegmentKind.Tail:
                    return UnheardWaveColor;
                default:
                    return BodyWaveColor;
            }
        }

        /// <summary>
        ///     Where the rows are, and the time axis they share.
        /// </summary>
        private class RowLayout
        {
            public Rectangle TopRow;
            public Rectangle TopWave;
            public Rectangle BottomRow;
            public Rectangle BottomWave;
            public Axis Axis;

            /// <summary>
            ///     The layout for an overview, or null when there is nothing to draw or no room.
            /// </summary>
            public static RowLayout For(Control control, MixOverview overview)
            {
                if (overview?.Current == null) return null;

                var size = control.ClientSize;
                var rowHeight = (size.Height - RowGap) / 2;
                if (rowHeight <= LabelHeight + 4 || size.Width < 10) return null;

                var layout = new RowLayout
                {
                    TopRow = new Rectangle(0, 0, size.Width, rowHeight),
                    BottomRow = new Rectangle(0, rowHeight + RowGap, size.Width, rowHeight),
                    Axis = Axis.For(overview)
                };
                layout.TopWave = WaveOf(layout.TopRow);
                layout.BottomWave = WaveOf(layout.BottomRow);
                return layout;
            }

            private static Rectangle WaveOf(Rectangle row)
            {
                return new Rectangle(row.Left, row.Top + LabelHeight, row.Width, row.Height - LabelHeight);
            }
        }

        /// <summary>
        ///     The shared time axis: from the earliest time any track is heard to the latest time shown. The next
        ///     track's unheard intro does not stretch it.
        /// </summary>
        private struct Axis
        {
            public double Start;
            public double End;

            public double Length => End - Start;

            public static Axis For(MixOverview overview)
            {
                var axis = new Axis { Start = double.MaxValue, End = double.MinValue };
                axis.Include(overview.Previous);
                axis.Include(overview.Current);
                axis.Include(overview.Next);
                if (axis.End <= axis.Start) axis.End = axis.Start + 1;
                return axis;
            }

            private void Include(MixOverviewTrack track)
            {
                if (track == null) return;
                Start = Math.Min(Start, track.Offset + track.AudibleStart);
                End = Math.Max(End, track.Offset + track.HeardEnd);
            }

            public int ToX(double time, int width)
            {
                return (int)Math.Round((time - Start) / (End - Start) * width);
            }
        }

        private class RowBitmap : IDisposable
        {
            public string Key { get; private set; }

            public Bitmap Bitmap { get; private set; }

            public void Set(string key, Bitmap bitmap)
            {
                Bitmap?.Dispose();
                Key = key;
                Bitmap = bitmap;
            }

            public void Dispose()
            {
                Bitmap?.Dispose();
                Bitmap = null;
                Key = null;
            }
        }
    }
}
