using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;
using Halloumi.Common.Windows.Controls;
using Un4seen.Bass;
using Un4seen.Bass.Misc;
using AE = Halloumi.Shuffler.AudioEngine;
using Sample = Halloumi.Shuffler.AudioLibrary.Models.Sample;

namespace Halloumi.Shuffler.Controls
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public partial class TrackWave : BaseUserControl
    {
        public enum TrackWaveMode
        {
            Shuffler,
            Sampler
        }

        private const decimal ZoomRatio = 0.5M;

        private readonly BASSTimer _timer;

        private bool _scrolling;

        private bool _showTrackFx;

        private bool _ticking;

        private bool _waveRendered = true;


        public EventHandler PositionsChanged;


        /// <summary>
        ///     Initializes a new instance of the <see cref="TrackWave" /> class.
        /// </summary>
        public TrackWave()
        {
            InitializeComponent();

            // create a secure timer
            _timer = new BASSTimer(50);
            _timer.Tick += timer_Tick;

            Mode = TrackWaveMode.Shuffler;
        }


        public TrackWaveMode Mode { get; set; }

        /// <summary>
        ///     Gets or sets the filename.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        ///     Gets or sets the wave form for the track.
        /// </summary>
        private WaveForm Wave { get; set; }

        /// <summary>
        ///     Gets or sets the zoom start.
        /// </summary>
        public long ZoomStart { get; private set; }

        /// <summary>
        ///     Gets or sets the zoom end.
        /// </summary>
        public long ZoomEnd { get; private set; }

        /// <summary>
        ///     Gets or sets the length of the zoom.
        /// </summary>
        public long ZoomLength => ZoomEnd - ZoomStart;

        /// <summary>
        ///     Gets or sets the bass player.
        /// </summary>
        public AE.BassPlayer BassPlayer { get; set; }

        /// <summary>
        ///     Gets or sets the bass track.
        /// </summary>
        public Track BassTrack { get; private set; }

        /// <summary>
        ///     Gets or sets the current position.
        /// </summary>
        private long CurrentPosition { get; set; }

        public bool ShowTrackFx
        {
            get { return _showTrackFx; }
            set
            {
                _showTrackFx = value;
                RefreshPositions();
            }
        }

        public List<TrackSample> TrackSamples { get; set; }

        public List<Sample> Samples { get; set; }

        public Sample CurrentSample { get; set; }


        private void picWaveForm_DoubleClick(object sender, EventArgs e)
        {
            Play();
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            mnuSetFadeInEnd.Visible = (Mode == TrackWaveMode.Shuffler);
            mnuSetFadeInStart.Visible = (Mode == TrackWaveMode.Shuffler);
            mnuSetFadeOutEnd.Visible = (Mode == TrackWaveMode.Shuffler);
            mnuSetFadeOutStart.Visible = (Mode == TrackWaveMode.Shuffler);
            mnuSetSkipStart.Visible = (Mode == TrackWaveMode.Shuffler);
            mnuSetSkipEnd.Visible = (Mode == TrackWaveMode.Shuffler);
            mnuSetPreFadeInStart.Visible = (Mode == TrackWaveMode.Shuffler);

            mnuSetSampleEnd.Visible = (Mode == TrackWaveMode.Sampler);
            mnuSetSampleStart.Visible = (Mode == TrackWaveMode.Sampler);
            mnuSetSampleOffset.Visible = (Mode == TrackWaveMode.Sampler);

            mnuSetSampleEnd.Enabled = (CurrentSample != null);
            mnuSetSampleStart.Enabled = (CurrentSample != null);
            mnuSetSampleOffset.Enabled = (CurrentSample != null);
        }

        private void mnuSetSampleStart_Click(object sender, EventArgs e)
        {
            if (CurrentSample == null) return;

            CurrentSample.Start = BassTrack.SamplesToSeconds(CurrentPosition);
            UpdatePositions();
        }

        private void mnuSetSampleEnd_Click(object sender, EventArgs e)
        {
            if (CurrentSample == null) return;

            var end = BassTrack.SamplesToSeconds(CurrentPosition);
            var start = CurrentSample.Start;

            if (start > end)
            {
                var temp = start;
                start = end;
                end = temp;
            }
            var length = end - start;

            CurrentSample.Start = start;
            CurrentSample.Length = length;

            UpdatePositions();
        }

        private void mnuSetSampleOffset_Click(object sender, EventArgs e)
        {
            if (CurrentSample == null) return;

            CurrentSample.Offset = BassTrack.SamplesToSeconds(CurrentPosition);
            UpdatePositions();
        }


        /// <summary>
        ///     Loads a track.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public Track LoadTrack(string fileName)
        {
            Filename = fileName;
            BassTrack = BassPlayer.LoadRawLoopTrack(Filename);

            LoadTrackWaveData();
            DrawWave();

            return BassTrack;
        }

        /// <summary>
        ///     Unloads this instance.
        /// </summary>
        public void Unload()
        {
            BassPlayer.UnloadRawLoopTrack();
            _timer.Stop();
        }

        /// <summary>
        ///     Refreshes the track positions.
        /// </summary>
        public void RefreshPositions()
        {
            if (Wave == null) return;
            SetMarkers();
            DrawWave();
        }

        /// <summary>
        ///     Zooms the view to the specified position
        /// </summary>
        /// <param name="startSeconds">The start seconds.</param>
        /// <param name="lengthSeconds">The length seconds.</param>
        /// <param name="currentPositionSeconds">The current position seconds.</param>
        public void Zoom(double startSeconds, double lengthSeconds, double currentPositionSeconds)
        {
            var start = BassTrack.SecondsToSamples(startSeconds);
            var end = BassTrack.SecondsToSamples(startSeconds + lengthSeconds);
            var currentPosition = start;
            if (currentPositionSeconds != 0D)
                currentPosition = BassTrack.SecondsToSamples(currentPositionSeconds);

            Zoom(start, end, currentPosition);
        }

        /// <summary>
        ///     Zooms the view to the specified position
        /// </summary>
        /// <param name="startSeconds">The start seconds.</param>
        /// <param name="lengthSeconds">The length seconds.</param>
        public void Zoom(double startSeconds, double lengthSeconds)
        {
            Zoom(startSeconds, lengthSeconds, startSeconds);
        }

        /// <summary>
        ///     Zooms the view to the specified position
        /// </summary>
        /// <param name="start">The start sample.</param>
        /// <param name="end">The end sample.</param>
        public void Zoom(long start, long end)
        {
            Zoom(start, end, start);
        }

        /// <summary>
        ///     Zooms the view to the specified position
        /// </summary>
        /// <param name="start">The start sample.</param>
        /// <param name="end">The end sample.</param>
        /// <param name="currentPosition">The current position.</param>
        public void Zoom(long start, long end, long currentPosition)
        {
            if (start < 0) return;
            if (end > BassTrack.Length) return;

            if (start > end)
            {
                var oldStart = start;
                var oldEnd = end;
                end = oldStart;
                start = oldEnd;
            }

            if (end - start <= 0) return;

            if (currentPosition < start || currentPosition > end)
                currentPosition = start;

            ZoomStart = start;
            ZoomEnd = end;
            CurrentPosition = currentPosition;
            DrawWave();
        }


        /// <summary>
        ///     Draws the wave.
        /// </summary>
        private void DrawWave()
        {
            if (Wave == null) return;
            if (ZoomLength == 0) return;

            BeginInvoke((MethodInvoker) delegate
            {
                var oldBitmap = picWaveForm.BackgroundImage;
                var bitmap = Wave.CreateBitmap(picWaveForm.Width,
                    picWaveForm.Height,
                    Wave.Position2Frames(BassTrack.SamplesToSeconds(ZoomStart)),
                    Wave.Position2Frames(BassTrack.SamplesToSeconds(ZoomEnd)),
                    true);

                if (CurrentPosition >= ZoomStart && CurrentPosition <= ZoomEnd)
                {
                    var positionPercent = ((CurrentPosition - ZoomStart) /(double)ZoomLength);
                    var x = Convert.ToInt32(Math.Round(positionPercent*picWaveForm.Width, 0));

                    using (var pen = new Pen(Color.Red))
                    using (var graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.DrawLine(pen, x, 0, x, picWaveForm.Height - 1);
                    }
                }

                picWaveForm.BackgroundImage = bitmap;
                oldBitmap?.Dispose();

                scrollBar.Maximum = (int) (BassTrack.Length - ZoomLength);
                scrollBar.SmallChange = scrollBar.Maximum/400;
                scrollBar.LargeChange = scrollBar.Maximum/50;

                if (!_scrolling) scrollBar.Value = (int)ZoomStart;

                UpdateViewText();
            });
        }

        /// <summary>
        ///     Loads the wave data for the current track
        /// </summary>
        private void LoadTrackWaveData()
        {
            _waveRendered = false;

            if (!File.Exists(Filename)) return;

            Wave = new WaveForm(Filename, WaveForm_Callback, this)
            {
                FrameResolution = 0.001f,
                CallbackFrequency = 30000,
                ColorBackground = Color.Black,
                ColorLeft = Color.Navy,
                ColorLeftEnvelope = Color.LightGray,
                ColorRight = Color.Navy,
                ColorRightEnvelope = Color.LightGray,
                ColorMarker = Color.Gold,
                ColorBeat = Color.LightSkyBlue,
                ColorVolume = Color.White,
                DrawEnvelope = false,
                DrawWaveForm = WaveForm.WAVEFORMDRAWTYPE.Mono,
                DrawMarker =
                    WaveForm.MARKERDRAWTYPE.Line | WaveForm.MARKERDRAWTYPE.Name | WaveForm.MARKERDRAWTYPE.NameBoxFilled,
                MarkerLength = 0.9f
            };


            Wave.RenderStart(true, BASSFlag.BASS_DEFAULT);

            SetMarkers();

            while (!_waveRendered)
            {
                Application.DoEvents();
                Thread.Sleep(100);
            }
        }

        private void UpdateViewText()
        {
            var start = TimeFormatHelper.GetFormattedSeconds(BassTrack.SamplesToSeconds(ZoomStart));
            var end = TimeFormatHelper.GetFormattedSeconds(BassTrack.SamplesToSeconds(ZoomEnd));
            var length = TimeFormatHelper.GetFormattedSeconds(BassTrack.SamplesToSeconds(ZoomLength));
            var position = TimeFormatHelper.GetFormattedSeconds(BassTrack.SamplesToSeconds(CurrentPosition));

            lblViewDetails.Text = $"View: {start} to {end} ({length}) Cursor: {position}";
        }

        /// <summary>
        ///     Sets the markers.
        /// </summary>
        private void SetMarkers()
        {
            if (Mode == TrackWaveMode.Shuffler)
                SetShufflerMarkers();
            else
                SetSamplerMarkers();
        }

        private void SetSamplerMarkers()
        {
            if (Wave == null) return;

            for (var i = 1; i <= 2000; i++)
            {
                Wave.RemoveMarker("S" + i + "S");
                Wave.RemoveMarker("S" + i + "E");
            }

            if (Samples != null)
            {
                for (var i = 1; i <= Samples.Count; i++)
                {
                    var sample = Samples[i - 1];

                    if (sample == CurrentSample)
                        continue;

                    if (sample.Start == 0 && sample.Length == 0) continue;

                    Wave.AddMarker("S" + i + "S", sample.Start);
                    Wave.AddMarker("S" + i + "E", sample.Start + sample.Length);
                }
            }

            Wave.RemoveMarker("CSS");
            Wave.RemoveMarker("CSE");
            Wave.RemoveMarker("CSO");

            if (CurrentSample == null) return;

            Wave.AddMarker("CSS", CurrentSample.Start);

            if (CurrentSample.Offset != 0D)
                Wave.AddMarker("CSO", CurrentSample.Offset);

            Wave.AddMarker("CSE", CurrentSample.Start + CurrentSample.Length);
        }

        private void SetShufflerMarkers()
        {
            if (Wave == null) return;

            Wave.RemoveMarker("PFI");
            Wave.RemoveMarker("FIS");
            Wave.RemoveMarker("FIE");
            Wave.RemoveMarker("FOS");
            Wave.RemoveMarker("FOE");
            Wave.RemoveMarker("SKS");
            Wave.RemoveMarker("SKE");

            var attributes = BassPlayer.GetAutomationAttributes(BassTrack);
            foreach (var trigger in attributes.TrackFXTriggers)
            {
                Wave.RemoveMarker("TS" + attributes.TrackFXTriggers.IndexOf(trigger));
                Wave.RemoveMarker("TE" + attributes.TrackFXTriggers.IndexOf(trigger));
            }

            for (var i = 1; i <= 2000; i++)
            {
                Wave.RemoveMarker("S" + i + "S");
                Wave.RemoveMarker("S" + i + "E");
            }

            Wave.AddMarker("PFI", BassTrack.SamplesToSeconds(BassTrack.PreFadeInStart));
            Wave.AddMarker("FIS", BassTrack.SamplesToSeconds(BassTrack.FadeInStart));
            Wave.AddMarker("FIE", BassTrack.SamplesToSeconds(BassTrack.FadeInEnd));
            Wave.AddMarker("FOS", BassTrack.SamplesToSeconds(BassTrack.FadeOutStart));
            Wave.AddMarker("FOE", BassTrack.SamplesToSeconds(BassTrack.FadeOutEnd));

            if (BassTrack.HasSkipSection)
            {
                Wave.AddMarker("SKS", BassTrack.SamplesToSeconds(BassTrack.SkipStart));
                Wave.AddMarker("SKE", BassTrack.SamplesToSeconds(BassTrack.SkipEnd));
            }

            if (TrackSamples != null)
            {
                for (var i = 1; i <= TrackSamples.Count; i++)
                {
                    var trackSample = TrackSamples[i - 1];

                    if (trackSample.Start == 0 && trackSample.Length == 0) continue;

                    Wave.AddMarker("S" + i + "S", trackSample.Start);
                    Wave.AddMarker("S" + i + "E", trackSample.Start + trackSample.Length);
                }
            }

            if (ShowTrackFx)
            {
                foreach (var trackFx in attributes.TrackFXTriggers)
                {
                    Wave.AddMarker("TS" + attributes.TrackFXTriggers.IndexOf(trackFx), trackFx.Start);
                    Wave.AddMarker("TE" + attributes.TrackFXTriggers.IndexOf(trackFx), trackFx.Start + trackFx.Length);
                }
            }
        }

        /// <summary>
        ///     Zooms in.
        /// </summary>
        private void ZoomIn()
        {
            var zoomLength = ZoomLength;
            zoomLength = (int) (zoomLength*ZoomRatio);
            Zoom(zoomLength);
        }

        /// <summary>
        ///     Zooms to the specified zoom length.
        /// </summary>
        /// <param name="zoomLength">Length of the zoom.</param>
        private void Zoom(long zoomLength)
        {
            ZoomEnd = ZoomStart + zoomLength;
            if (ZoomEnd > BassTrack.Length)
            {
                ZoomEnd = BassTrack.Length;
                ZoomStart = BassTrack.Length - zoomLength;
            }
            if (ZoomEnd < 0) ZoomEnd = BassTrack.Length;
            if (ZoomStart < 0) ZoomStart = 0;
            DrawWave();
        }

        /// <summary>
        ///     Zooms out.
        /// </summary>
        private void ZoomOut()
        {
            var zoomLength = ZoomLength;
            zoomLength = (int) (zoomLength/ZoomRatio);
            Zoom(zoomLength);
        }

        /// <summary>
        ///     Zooms to full.
        /// </summary>
        private void ZoomFull()
        {
            ZoomStart = 0;
            ZoomEnd = BassTrack.Length;
            DrawWave();
        }

        /// <summary>
        ///     Draws the current selected position.
        /// </summary>
        private void DrawCurrentPosition()
        {
            var position = AudioStreamHelper.GetPosition(BassPlayer.RawLoopTrack);

            BeginInvoke((MethodInvoker) delegate
            {
                var oldBitmap = picWaveForm.Image;
                var bitmap = new Bitmap(picWaveForm.Width, picWaveForm.Height);

                if (position >= ZoomStart && position <= ZoomEnd)
                {
                    var positionPercent = ((position - ZoomStart) /(double)ZoomLength);
                    var x = Convert.ToInt32(Math.Round(positionPercent*picWaveForm.Width, 0));

                    using (var pen = new Pen(Color.Gray))
                    using (var graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.Clear(Color.Black);
                        graphics.DrawLine(pen, x, 0, x, picWaveForm.Height - 1);
                    }
                }
                bitmap.MakeTransparent(Color.Black);
                picWaveForm.Image = bitmap;

                oldBitmap?.Dispose();
            });
        }

        /// <summary>
        ///     Updates the fade positions.
        /// </summary>
        private void UpdatePositions()
        {
            SetMarkers();
            DrawWave();

            // raise track changed event
            PositionsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void Play()
        {
            BeginInvoke((MethodInvoker) delegate
            {
                BassPlayer.SetRawLoopPositions(ZoomStart, ZoomEnd, CurrentPosition);
                BassPlayer.PlayRawLoop();
                _timer.Enabled = true;
            });
        }

        public float GetNormalizationGain(double start, double length)
        {
            var end = start + length;

            var startFrame = Wave.Position2Frames(start);
            var endFrame = Wave.Position2Frames(end);

            var peak = 0F;
            var gain = Wave.GetNormalizationGain(startFrame, endFrame, ref peak);

            return gain;
        }


        /// <summary>
        ///     Handles the Callback event of a WaveFormObject.
        /// </summary>
        /// <param name="framesDone">The frames done.</param>
        /// <param name="framesTotal">The frames total.</param>
        /// <param name="elapsedTime">The elapsed time.</param>
        /// <param name="finished">If true, rendering has finished.</param>
        private void WaveForm_Callback(int framesDone, int framesTotal, TimeSpan elapsedTime, bool finished)
        {
            if (finished)
            {
                ZoomStart = 0;
                ZoomEnd = BassTrack.Length;
                _waveRendered = true;
            }
            DrawWave();
        }

        /// <summary>
        ///     Handles the Resize event of the picWaveForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void picWaveForm_Resize(object sender, EventArgs e)
        {
            DrawWave();
        }

        /// <summary>
        ///     Handles the Click event of the btnZoomIn control.
        /// </summary>
        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            ZoomIn();
        }

        /// <summary>
        ///     Handles the Click event of the btnZoomFull control.
        /// </summary>
        private void btnZoomFull_Click(object sender, EventArgs e)
        {
            ZoomFull();
        }

        /// <summary>
        ///     Handles the Click event of the btnZoomOut control.
        /// </summary>
        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            ZoomOut();
        }

        /// <summary>
        ///     Handles the ValueChanged event of the scrollBar control.
        /// </summary>
        private void scrollBar_ValueChanged(object sender, EventArgs e)
        {
            if (_scrolling) return;
            _scrolling = true;

            var zoomLength = ZoomLength;
            ZoomStart = scrollBar.Value;
            ZoomEnd = ZoomStart + zoomLength;
            if (ZoomEnd < 0) ZoomEnd = BassTrack.Length;
            if (ZoomStart < 0) ZoomStart = 0;
            DrawWave();

            _scrolling = false;
        }

        /// <summary>
        ///     Handles the MouseUp event of the picWaveForm control.
        /// </summary>
        private void picWaveForm_MouseUp(object sender, MouseEventArgs e)
        {
            var percent = e.X/(double) picWaveForm.Width;
            var viewPercent = percent*ZoomLength;
            var position = ZoomStart + Convert.ToInt32(Math.Round(viewPercent, 0));

            if (ModifierKeys == Keys.Shift)
            {
                Zoom(CurrentPosition, position);
            }
            else
            {
                CurrentPosition = position;
                DrawWave();
            }
        }

        /// <summary>
        ///     Handles the Click event of the mnuSetPreFadeInStart control.
        /// </summary>
        private void mnuSetPreFadeInStart_Click(object sender, EventArgs e)
        {
            BassTrack.PreFadeInStart = CurrentPosition;
            UpdatePositions();
        }

        /// <summary>
        ///     Handles the Click event of the mnuSetFadeInStart control.
        /// </summary>
        private void mnuSetFadeInStart_Click(object sender, EventArgs e)
        {
            BassTrack.FadeInStart = CurrentPosition;
            UpdatePositions();
        }

        /// <summary>
        ///     Handles the Click event of the mnuSetFadeInEnd control.
        /// </summary>
        private void mnuSetFadeInEnd_Click(object sender, EventArgs e)
        {
            BassTrack.FadeInEnd = CurrentPosition;
            UpdatePositions();
        }

        /// <summary>
        ///     Handles the Click event of the mnuSetFadeOutStart control.
        /// </summary>
        private void mnuSetFadeOutStart_Click(object sender, EventArgs e)
        {
            BassTrack.FadeOutStart = CurrentPosition;
            UpdatePositions();
        }

        /// <summary>
        ///     Handles the Click event of the mnuSetFadeOutEnd control.
        /// </summary>
        private void mnuSetFadeOutEnd_Click(object sender, EventArgs e)
        {
            BassTrack.FadeOutEnd = CurrentPosition;
            UpdatePositions();
        }

        /// <summary>
        ///     Handles the Click event of the mnuSetSkipStart control.
        /// </summary>
        private void mnuSetSkipStart_Click(object sender, EventArgs e)
        {
            BassTrack.SkipStart = CurrentPosition;
            UpdatePositions();
        }

        /// <summary>
        ///     Handles the Click event of the mnuSetSkipEnd control.
        /// </summary>
        private void mnuSetSkipEnd_Click(object sender, EventArgs e)
        {
            BassTrack.SkipEnd = CurrentPosition;
            UpdatePositions();
        }

        /// <summary>
        ///     Handles the Click event of the btnStop control.
        /// </summary>
        private void btnStop_Click(object sender, EventArgs e)
        {
            Stop();
        }

        public void Stop()
        {
            picWaveForm.Image = null;
            _timer.Enabled = false;
            BassPlayer.StopRawLoop();
        }

        /// <summary>
        ///     Handles the Click event of the btnPlay control.
        /// </summary>
        private void btnPlay_Click(object sender, EventArgs e)
        {
            Play();
        }

        /// <summary>
        ///     Handles the Tick event of the timer control.
        /// </summary>
        private void timer_Tick(object sender, EventArgs e)
        {
            if (_ticking) return;
            try
            {
                DrawCurrentPosition();
            }
            catch
            {
                // ignored
            }
            finally
            {
                _ticking = false;
            }
        }

        /// <summary>
        ///     Handles the Click event of the btnEdit control.
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            const string editPath = @"C:\Program Files (x86)\coolpro2\coolpro2.exe";
            var output = Path.Combine(Path.GetTempPath(), @"Shuffler.wav");

            if (!File.Exists(editPath)) return;

            AudioExportHelper.SaveAsWave(BassTrack.Filename, output);

            if (!File.Exists(output)) return;
            output = $"\"{output}\"";

            Process.Start(editPath, output);
        }

        private void btnlLeft_Click(object sender, EventArgs e)
        {
            var zoomLength = ZoomLength;
            ZoomStart -= zoomLength;
            Zoom(zoomLength);
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            var zoomLength = ZoomLength;
            ZoomStart += zoomLength;
            Zoom(zoomLength);
        }
    }
}