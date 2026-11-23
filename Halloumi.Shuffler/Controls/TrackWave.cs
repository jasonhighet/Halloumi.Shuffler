using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Halloumi.Common.Windows.Controls;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.AudioEngine.BassPlayer;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;
using Un4seen.Bass;
using Un4seen.Bass.Misc;
using Button = Halloumi.Common.Windows.Controls.Button;
using Sample = Halloumi.Shuffler.AudioLibrary.Samples.Sample;

namespace Halloumi.Shuffler.Controls
{
    /// <summary>
    ///     Scrollable/zoomable waveform for setting up a track: fade-in / fade-out / skip sections (Shuffler mode)
    ///     or samples (Sampler mode).
    ///     The view (ZoomStart → ZoomEnd) and the sections are separate things. A section can be made "active"
    ///     (click its band, drag one of its edges, press I / O / K, or zoom to it): Play then loops the active
    ///     section whatever the view shows, and the loop follows the section while it is being edited. With no
    ///     active section, Play loops the view, as it always has.
    /// </summary>
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public partial class TrackWave : BaseUserControl, IMessageFilter
    {
        public enum TrackWaveMode
        {
            Shuffler,
            Sampler
        }

        /// <summary>A part of the track that can be selected, looped and moved as a whole.</summary>
        public enum WaveSection
        {
            None,
            FadeIn,
            FadeOut,
            Skip,
            Sample
        }

        private enum Edge
        {
            None,
            PreFadeIn,
            FadeInStart,
            FadeInEnd,
            FadeOutStart,
            FadeOutEnd,
            SkipStart,
            SkipEnd,
            SampleStart,
            SampleEnd,
            SampleOffset
        }

        private enum DragMode
        {
            None,
            Pending,
            Edge,
            Section,
            Range,
            Pan
        }

        private const decimal ZoomRatio = 0.5M;

        /// <summary>How many suggested positions are offered for each fade section.</summary>
        private const int CandidateCount = 5;

        private const double MinSectionSeconds = 0.02;
        private const double MinViewSeconds = 0.05;

        private static readonly Color RulerColor = Color.FromArgb(32, 32, 38);
        private static readonly Color RulerTextColor = Color.FromArgb(190, 190, 200);
        private static readonly Color FadeInColor = Color.FromArgb(80, 200, 120);
        private static readonly Color FadeOutColor = Color.FromArgb(240, 150, 60);
        private static readonly Color SkipColor = Color.FromArgb(160, 160, 170);
        private static readonly Color SampleColor = Color.FromArgb(170, 130, 255);
        private static readonly Color OtherSampleColor = Color.FromArgb(120, 100, 170);
        private static readonly Color TrackFxColor = Color.LightSkyBlue;
        private static readonly Color DetectedBeatColor = Color.FromArgb(110, 190, 255);

        private readonly BASSTimer _timer;
        private readonly Font _smallFont = new Font("Segoe UI", 8F);

        private bool _showTrackFx;
        private bool _ticking;
        private bool _updatingScroll;

        private Bitmap _waveBitmap;
        private bool _waveDirty = true;
        private double _secondsPerUnit = 0.001;

        private WaveSection _activeSection = WaveSection.None;
        private bool _snap;
        private bool _playing;

        // true when playback is looping the active section (rather than playing from a cursor outside it)
        private bool _loopingSection;
        private long _playPosition = -1;

        private BeatAnalysis _beatAnalysis;
        private CancellationTokenSource _analysisCancellation;
        private FadeProfile _fadeProfile;
        private List<FadeCandidate> _fadeInCandidates;
        private List<FadeCandidate> _fadeOutCandidates;
        private long _fadeOutCandidatesStart = -1;
        private long _fadeOutCandidatesLength = -1;
        private bool _suggestWhenAnalysed;
        private long[] _suggestSnapshot;

        private DragMode _drag = DragMode.None;
        private Edge _dragEdge = Edge.None;
        private WaveSection _dragSection = WaveSection.None;
        private bool _dragMoved;
        private int _mouseDownX;
        private long _mouseDownPosition;
        private long _dragSectionStart;
        private long _dragRangeEnd;
        private long _panStart;

        private Button _btnSnap;
        private Button _btnSuggest;
        private ToolStripMenuItem _mnuSnap;
        private ToolStripMenuItem _mnuSuggest;

        public EventHandler PositionsChanged;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TrackWave" /> class.
        /// </summary>
        public TrackWave()
        {
            InitializeComponent();

            Mode = TrackWaveMode.Shuffler;

            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;

            // create a secure timer
            _timer = new BASSTimer(50);
            _timer.Tick += timer_Tick;

            picWaveForm.BackColor = Color.Black;
            picWaveForm.Paint += picWaveForm_Paint;
            picWaveForm.MouseDown += picWaveForm_MouseDown;
            picWaveForm.MouseMove += picWaveForm_MouseMove;

            BuildExtraControls();

            Disposed += (sender, e) =>
            {
                CancelAnalysis();
                _waveBitmap?.Dispose();
                _smallFont.Dispose();
            };
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TrackWaveMode Mode { get; set; }

        /// <summary>
        ///     Gets or sets the filename.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Filename { get; set; }

        /// <summary>
        ///     Gets or sets the wave form for the track.
        /// </summary>
        private WaveForm Wave { get; set; }

        /// <summary>
        ///     Gets or sets the zoom start.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long ZoomStart { get; private set; }

        /// <summary>
        ///     Gets or sets the zoom end.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long ZoomEnd { get; private set; }

        /// <summary>
        ///     Gets or sets the length of the zoom.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long ZoomLength
        {
            get { return ZoomEnd - ZoomStart; }
        }

        private BassPlayer BassPlayer { get; set; }

        private Track BassTrack { get; set; }

        /// <summary>
        ///     Gets or sets the current position (the red cursor: the last place clicked).
        /// </summary>
        private long CurrentPosition { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ShowTrackFx
        {
            get { return _showTrackFx; }
            set
            {
                _showTrackFx = value;
                RefreshPositions();
            }
        }

        /// <summary>
        ///     The section that Play loops and that the keyboard shortcuts act on (None = the view is looped).
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WaveSection ActiveSection
        {
            get { return _activeSection; }
            set { SetActiveSection(value); }
        }

        /// <summary>
        ///     When on, the cursor and anything dragged or moved lands on the nearest beat (or half-beat) of the
        ///     audio, and section ends land a whole number of beats from their start. Hold Ctrl to do the opposite.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool SnapToBeat
        {
            get { return _snap; }
            set
            {
                _snap = value;
                if (_btnSnap != null) _btnSnap.Text = _snap ? "Snap: On" : "Snap: Off";
                InvalidateWave(false);
            }
        }

        /// <summary>
        ///     Set for a track that has never been set up: as soon as the beat analysis is ready the fade
        ///     sections are replaced with suggested ones, unless they have been changed in the meantime.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool SuggestFadesWhenAnalysed
        {
            get { return _suggestWhenAnalysed; }
            set
            {
                _suggestWhenAnalysed = value;
                _suggestSnapshot = value && BassTrack != null ? FadePositions() : null;
                if (value && _beatAnalysis != null) ApplySuggestionIfUntouched();
            }
        }

        /// <summary>
        ///     Where fade sections usually go for this kind of track (see ShufflerApplication.GetFadeProfile).
        ///     Steers the suggested fade positions; without it the suggestions use library-wide defaults.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FadeProfile FadeProfile
        {
            get { return _fadeProfile; }
            set
            {
                _fadeProfile = value;
                _fadeInCandidates = null;
                _fadeOutCandidates = null;
                InvalidateWave(false);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private List<TrackSample> TrackSamples { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private List<Sample> Samples { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private Sample CurrentSample { get; set; }

        private int RulerHeight
        {
            get { return Scale96(18); }
        }

        private int BandHeight
        {
            get { return Scale96(18); }
        }

        private int GrabPixels
        {
            get { return Scale96(5); }
        }

        private Rectangle WaveArea
        {
            get
            {
                return new Rectangle(0, RulerHeight, picWaveForm.ClientSize.Width,
                    Math.Max(0, picWaveForm.ClientSize.Height - RulerHeight));
            }
        }

        /// <summary>Snapping as it applies right now: Ctrl temporarily reverses the Snap setting.</summary>
        private bool SnapNow
        {
            get
            {
                if (_beatAnalysis == null) return false;
                var reverse = (ModifierKeys & Keys.Control) == Keys.Control;
                return _snap != reverse;
            }
        }

        #region Set-up

        /// <summary>
        ///     Adds the controls that aren't in the designer: Snap, Auto Fades and the shortcut list.
        /// </summary>
        private void BuildExtraControls()
        {
            _btnSnap = NewToolButton("Snap: Off", (sender, e) => SnapToBeat = !SnapToBeat);
            _btnSuggest = NewToolButton("Auto Fades", (sender, e) => SuggestFades());
            var btnKeys = NewToolButton("Keys", (sender, e) => ShowShortcuts());

            flpPlaybackButtons.WrapContents = false;
            flpPlaybackButtons.AutoSize = true;
            flpPlaybackButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpPlaybackButtons.Controls.Add(_btnSnap);
            flpPlaybackButtons.Controls.Add(_btnSuggest);
            flpPlaybackButtons.Controls.Add(btnKeys);

            _mnuSnap = new ToolStripMenuItem("Snap to Beat");
            _mnuSnap.Click += (sender, e) => SnapToBeat = !SnapToBeat;
            _mnuSuggest = new ToolStripMenuItem("Suggest Fade Sections");
            _mnuSuggest.Click += (sender, e) => SuggestFades();
            var mnuKeys = new ToolStripMenuItem("Keyboard Shortcuts...");
            mnuKeys.Click += (sender, e) => ShowShortcuts();

            contextMenuStrip.Items.Add(new ToolStripSeparator());
            contextMenuStrip.Items.Add(_mnuSnap);
            contextMenuStrip.Items.Add(_mnuSuggest);
            contextMenuStrip.Items.Add(mnuKeys);
        }

        private Button NewToolButton(string text, EventHandler click)
        {
            // as tall as the designer's buttons, and wide enough for the text
            var textWidth = TextRenderer.MeasureText(text, btnEdit.Font).Width;
            var button = new Button
            {
                Text = text,
                Size = new Size(Math.Max(btnEdit.Width, textWidth + Scale96(36)), btnEdit.Height),
                Margin = btnEdit.Margin
            };
            button.Click += click;
            return button;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            System.Windows.Forms.Application.AddMessageFilter(this);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            System.Windows.Forms.Application.RemoveMessageFilter(this);
            base.OnHandleDestroyed(e);
        }

        public void SetModeAndBassPlayer(TrackWaveMode sampler, BassPlayer bassPlayer)
        {
            Mode = sampler;
            BassPlayer = bassPlayer;
            if (_btnSuggest != null) _btnSuggest.Visible = Mode == TrackWaveMode.Shuffler;
        }

        public void SetCurrentSample(Sample currentSample)
        {
            CurrentSample = currentSample;
            if (Mode == TrackWaveMode.Sampler)
                _activeSection = currentSample != null ? WaveSection.Sample : WaveSection.None;
        }

        internal void SetSamples(List<Sample> samples)
        {
            Samples = samples;
        }

        internal void SetTrackSamples(List<TrackSample> currentSamples)
        {
            TrackSamples = currentSamples;
        }

        /// <summary>
        ///     Loads a track.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public Track LoadTrack(string fileName)
        {
            Filename = fileName;
            BassTrack = BassPlayer.LoadRawLoopTrack(Filename);

            var isShufflerTrack = ExtendedAttributesHelper.HasExtendedAttributes(BassTrack.Description);
            if (!isShufflerTrack && (BassTrack.TagBpm == 0 || BassTrack.TagBpm == 100))
            {
                var bpm = BpmCalculator.CalculateBpm(BassTrack.Filename);
                if (bpm > 0)
                {
                    BassTrack.TagBpm = BpmHelper.NormaliseBpm(Convert.ToDecimal(bpm));
                    TagHelper.ClearCache(BassTrack.Filename);
                }
            }

            var lengthSeconds = BassTrack.LengthSeconds;
            _secondsPerUnit = BassTrack.Length > 0 && lengthSeconds > 0 ? lengthSeconds / BassTrack.Length : 0.001;

            _activeSection = WaveSection.None;
            _playing = false;
            _playPosition = -1;
            _fadeInCandidates = null;
            _fadeOutCandidates = null;

            LoadTrackWaveData();
            StartBeatAnalysis();

            return BassTrack;
        }

        /// <summary>
        ///     Unloads this instance.
        /// </summary>
        public void Unload()
        {
            CancelAnalysis();
            BassPlayer.UnloadRawLoopTrack();
            _timer.Stop();
            _playing = false;
        }

        /// <summary>
        ///     Loads the wave data for the current track
        /// </summary>
        private void LoadTrackWaveData()
        {
            ZoomStart = 0;
            ZoomEnd = 0;

            if (!File.Exists(Filename)) return;

            // the markers, sections and beat grid are drawn by this control, not by the WaveForm
            Wave = new WaveForm(Filename, WaveForm_Callback, this)
            {
                FrameResolution = 0.001f,
                CallbackFrequency = 30000,
                ColorBackground = Color.Black,
                ColorLeft = Color.Navy,
                ColorLeftEnvelope = Color.LightGray,
                ColorRight = Color.Navy,
                ColorRightEnvelope = Color.LightGray,
                DrawEnvelope = false,
                DrawWaveForm = WaveForm.WAVEFORMDRAWTYPE.Mono,
                DrawMarker = WaveForm.MARKERDRAWTYPE.None
            };

            Wave.RenderStart(true, BASSFlag.BASS_DEFAULT);
        }

        /// <summary>
        ///     Handles the Callback event of a WaveFormObject.
        /// </summary>
        private void WaveForm_Callback(int framesDone, int framesTotal, TimeSpan elapsedTime, bool finished)
        {
            // show the whole track as soon as there is something to draw, unless the view has been set already
            if (ZoomLength == 0)
            {
                ZoomStart = 0;
                ZoomEnd = BassTrack.Length;
            }
            InvalidateWave(true);
        }

        #endregion

        #region Beat analysis

        /// <summary>
        ///     Analyses the beats of the whole track in the background. Until it finishes, snapping does nothing.
        /// </summary>
        private void StartBeatAnalysis()
        {
            CancelAnalysis();
            _beatAnalysis = null;

            if (!File.Exists(Filename)) return;

            var cancellation = new CancellationTokenSource();
            _analysisCancellation = cancellation;
            var file = Filename;

            Task.Run(() => BeatAnalysis.Analyse(file, cancellation.Token))
                .ContinueWith(task =>
                {
                    if (task.IsFaulted || task.Result == null || cancellation.IsCancellationRequested) return;
                    var analysis = task.Result;
                    try
                    {
                        if (IsDisposed || !IsHandleCreated) return;
                        BeginInvoke((MethodInvoker) delegate
                        {
                            if (cancellation.IsCancellationRequested || Filename != file) return;
                            _beatAnalysis = analysis;
                            if (_suggestWhenAnalysed) ApplySuggestionIfUntouched();
                            InvalidateWave(false);
                        });
                    }
                    catch (InvalidOperationException)
                    {
                        // the form closed while the analysis was running
                    }
                });
        }

        private void CancelAnalysis()
        {
            _analysisCancellation?.Cancel();
            _analysisCancellation = null;
        }

        /// <summary>
        ///     Detects the tempo of the audio around a position (10 seconds before it to 30 after).
        /// </summary>
        /// <returns>The BPM as detected (70–180), or 0 if there is no detectable tempo.</returns>
        public double DetectBpmAround(double seconds)
        {
            var start = Math.Max(0, seconds - 10);
            var end = start + 40;

            if (_beatAnalysis != null) return _beatAnalysis.DetectBpm(start, end);
            return BeatAnalysis.DetectBpm(Filename, start, end);
        }

        /// <summary>
        ///     Replaces the fade-in and fade-out sections with the first suggestion for each. The other
        ///     suggestions are numbered on the waveform: Up / Down (or 1-5) steps through them.
        /// </summary>
        public void SuggestFades()
        {
            if (Mode != TrackWaveMode.Shuffler || BassTrack == null) return;

            if (_beatAnalysis == null)
            {
                MessageBox.Show(FindForm(), "The beats of this track are still being analysed. Try again in a moment.",
                    "Auto Fades", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (ExtendedAttributesHelper.HasExtendedAttributes(BassTrack.Description) &&
                !MessageBoxHelper.Confirm("Replace the fade-in and fade-out sections with suggested ones?"))
                return;

            if (ApplySuggestion()) return;

            MessageBox.Show(FindForm(), "Could not find a steady beat in this track to place the fades on.",
                "Auto Fades", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool ApplySuggestion()
        {
            var fadeIn = Candidates(WaveSection.FadeIn);
            if (fadeIn == null || fadeIn.Count == 0) return false;
            SetSectionToCandidate(WaveSection.FadeIn, fadeIn[0]);

            // the fade-out suggestions follow from the fade-in
            var fadeOut = Candidates(WaveSection.FadeOut);
            if (fadeOut != null && fadeOut.Count > 0) SetSectionToCandidate(WaveSection.FadeOut, fadeOut[0]);

            _suggestWhenAnalysed = false;
            UpdatePositions();
            return true;
        }

        /// <summary>
        ///     The suggested positions for a fade section, best first (null if there are none: not a fade
        ///     section, or the beat analysis hasn't finished). The fade-out suggestions are a whole number of
        ///     fade-in lengths after the fade-in, so they are worked out again whenever the fade-in changes.
        /// </summary>
        private List<FadeCandidate> Candidates(WaveSection section)
        {
            if (_beatAnalysis == null || BassTrack == null || Mode != TrackWaveMode.Shuffler) return null;

            if (section == WaveSection.FadeIn)
            {
                return _fadeInCandidates ??
                       (_fadeInCandidates = _beatAnalysis.GetFadeInCandidates(_fadeProfile, CandidateCount));
            }

            if (section != WaveSection.FadeOut) return null;

            var start = BassTrack.FadeInStart;
            var length = BassTrack.FadeInEnd - BassTrack.FadeInStart;
            var current = _fadeOutCandidates != null && _fadeOutCandidatesStart == start &&
                          _fadeOutCandidatesLength == length;

            // not while the fade-in is being dragged: wait until it has been let go
            if (current || (_drag != DragMode.None && _fadeOutCandidates != null)) return _fadeOutCandidates;

            _fadeOutCandidates = _beatAnalysis.GetFadeOutCandidates(ToSeconds(start), ToSeconds(length), _fadeProfile,
                CandidateCount);
            _fadeOutCandidatesStart = start;
            _fadeOutCandidatesLength = length;
            return _fadeOutCandidates;
        }

        private void SetSectionToCandidate(WaveSection section, FadeCandidate candidate)
        {
            var start = Clamp(ToUnits(candidate.Start), 0, BassTrack.Length);
            var end = Clamp(ToUnits(candidate.Start + candidate.Length), start + 1, BassTrack.Length);
            SetSection(section, start, end);
        }

        /// <summary>
        ///     Which suggestion a fade section is sitting on (-1 if it isn't on one).
        /// </summary>
        private int CurrentCandidate(WaveSection section)
        {
            var candidates = Candidates(section);
            long start, end;
            if (candidates == null || !GetSection(section, out start, out end)) return -1;

            var tolerance = Math.Max(1, ToUnits(0.005));
            for (var i = 0; i < candidates.Count; i++)
            {
                if (Math.Abs(ToUnits(candidates[i].Start) - start) <= tolerance) return i;
            }
            return -1;
        }

        /// <summary>
        ///     Moves a fade section to one of its suggestions, selects it and shows it. A playing loop carries
        ///     straight on at the new position, so suggestions can be auditioned one after another.
        /// </summary>
        private void ApplyCandidate(WaveSection section, int index)
        {
            var candidates = Candidates(section);
            if (candidates == null || index < 0 || index >= candidates.Count) return;

            SetSectionToCandidate(section, candidates[index]);
            _activeSection = section;

            long start, end;
            GetSection(section, out start, out end);
            CurrentPosition = start;

            if (start < ZoomStart || end > ZoomEnd)
            {
                // keep the zoom if the section fits on screen with room to spare, otherwise frame the section
                if ((end - start) * 1.3 <= ZoomLength && ZoomLength < BassTrack.Length)
                    SetView(start - (ZoomLength - (end - start)) / 2, ZoomLength);
                else
                    ZoomToSection(section);
            }

            UpdatePositions();
        }

        /// <summary>
        ///     Steps the selected fade section (the fade-in, if neither is selected) to its next or previous
        ///     suggestion.
        /// </summary>
        private void CycleCandidate(int direction)
        {
            var section = _activeSection == WaveSection.FadeOut ? WaveSection.FadeOut : WaveSection.FadeIn;
            var candidates = Candidates(section);
            if (candidates == null || candidates.Count == 0) return;

            var current = CurrentCandidate(section);
            var next = current < 0
                ? (direction > 0 ? 0 : candidates.Count - 1)
                : (current + direction + candidates.Count) % candidates.Count;
            ApplyCandidate(section, next);
        }

        private Rectangle CandidateBox(FadeCandidate candidate)
        {
            var size = BandHeight - Scale96(2);
            return new Rectangle(XAt(ToUnits(candidate.Start)), RulerHeight + BandHeight + Scale96(1), size, size);
        }

        /// <summary>
        ///     The numbered boxes under the section bands: the suggested positions for the fade-in (green) and
        ///     fade-out (orange), 1 being the best guess. Click one to move the section there.
        /// </summary>
        private void DrawCandidates(Graphics g)
        {
            foreach (var section in new[] { WaveSection.FadeIn, WaveSection.FadeOut })
            {
                var candidates = Candidates(section);
                if (candidates == null) continue;

                var current = CurrentCandidate(section);
                var color = SectionColor(section);

                for (var i = 0; i < candidates.Count; i++)
                {
                    var position = ToUnits(candidates[i].Start);
                    if (position < ZoomStart || position > ZoomEnd) continue;

                    var box = CandidateBox(candidates[i]);
                    using (var brush = new SolidBrush(i == current ? color : Color.FromArgb(215, 20, 20, 24)))
                        g.FillRectangle(brush, box);
                    using (var pen = new Pen(color))
                    {
                        g.DrawRectangle(pen, box);
                        g.DrawLine(pen, box.Left, box.Top, box.Left, box.Bottom + Scale96(6));
                    }

                    TextRenderer.DrawText(g, (i + 1).ToString(), _smallFont, box, i == current ? Color.Black : color,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
                }
            }
        }

        /// <summary>
        ///     Finds the suggestion box at a point.
        /// </summary>
        private bool HitTestCandidate(int x, int y, out WaveSection section, out int index)
        {
            section = WaveSection.None;
            index = -1;

            foreach (var candidateSection in new[] { WaveSection.FadeIn, WaveSection.FadeOut })
            {
                var candidates = Candidates(candidateSection);
                if (candidates == null) continue;

                for (var i = 0; i < candidates.Count; i++)
                {
                    if (!CandidateBox(candidates[i]).Contains(x, y)) continue;
                    section = candidateSection;
                    index = i;
                    return true;
                }
            }
            return false;
        }

        private void ApplySuggestionIfUntouched()
        {
            var snapshot = _suggestSnapshot;
            _suggestWhenAnalysed = false;
            _suggestSnapshot = null;
            if (snapshot == null || BassTrack == null || _drag != DragMode.None) return;

            var current = FadePositions();
            for (var i = 0; i < current.Length; i++)
            {
                if (current[i] != snapshot[i]) return;
            }

            ApplySuggestion();
        }

        private long[] FadePositions()
        {
            return new[] { BassTrack.FadeInStart, BassTrack.FadeInEnd, BassTrack.FadeOutStart, BassTrack.FadeOutEnd };
        }

        private long SnapPosition(long position)
        {
            if (!SnapNow) return position;
            return Clamp(ToUnits(_beatAnalysis.SnapToBeat(ToSeconds(position))), 0, BassTrack.Length);
        }

        private long SnapEnd(long start, long end)
        {
            if (!SnapNow) return end;
            return Clamp(ToUnits(_beatAnalysis.SnapEndToBeat(ToSeconds(start), ToSeconds(end))), 0, BassTrack.Length);
        }

        #endregion

        #region Sections and edges

        private double ToSeconds(long units)
        {
            return units * _secondsPerUnit;
        }

        private long ToUnits(double seconds)
        {
            return (long) Math.Round(seconds / _secondsPerUnit);
        }

        private static long Clamp(long value, long min, long max)
        {
            if (max < min) max = min;
            return value < min ? min : value > max ? max : value;
        }

        private IEnumerable<WaveSection> Sections()
        {
            if (Mode == TrackWaveMode.Shuffler)
                return new[] { WaveSection.FadeIn, WaveSection.FadeOut, WaveSection.Skip };
            return new[] { WaveSection.Sample };
        }

        private IEnumerable<Edge> Edges()
        {
            if (Mode == TrackWaveMode.Shuffler)
            {
                return new[]
                {
                    Edge.FadeInStart, Edge.FadeInEnd, Edge.FadeOutStart, Edge.FadeOutEnd,
                    Edge.SkipStart, Edge.SkipEnd, Edge.PreFadeIn
                };
            }
            return new[] { Edge.SampleStart, Edge.SampleEnd, Edge.SampleOffset };
        }

        /// <summary>
        ///     Gets where a section is, in track positions.
        /// </summary>
        /// <returns>False if the section doesn't exist (in this mode, or because it hasn't been set).</returns>
        private bool GetSection(WaveSection section, out long start, out long end)
        {
            start = end = 0;
            if (BassTrack == null) return false;

            switch (section)
            {
                case WaveSection.FadeIn:
                    if (Mode != TrackWaveMode.Shuffler) return false;
                    start = BassTrack.FadeInStart;
                    end = BassTrack.FadeInEnd;
                    break;
                case WaveSection.FadeOut:
                    if (Mode != TrackWaveMode.Shuffler) return false;
                    start = BassTrack.FadeOutStart;
                    end = BassTrack.FadeOutEnd;
                    break;
                case WaveSection.Skip:
                    if (Mode != TrackWaveMode.Shuffler || !BassTrack.HasSkipSection) return false;
                    start = BassTrack.SkipStart;
                    end = BassTrack.SkipEnd;
                    break;
                case WaveSection.Sample:
                    if (Mode != TrackWaveMode.Sampler || CurrentSample == null) return false;
                    start = ToUnits(CurrentSample.Start);
                    end = ToUnits(CurrentSample.Start + CurrentSample.Length);
                    break;
                default:
                    return false;
            }

            return end > start;
        }

        private void SetSection(WaveSection section, long start, long end)
        {
            switch (section)
            {
                case WaveSection.FadeIn:
                    BassTrack.FadeInStart = start;
                    BassTrack.FadeInEnd = end;
                    break;
                case WaveSection.FadeOut:
                    BassTrack.FadeOutStart = start;
                    BassTrack.FadeOutEnd = end;
                    break;
                case WaveSection.Skip:
                    BassTrack.SkipStart = Math.Max(1, start); // a skip section starting at 0 means "none"
                    BassTrack.SkipEnd = end;
                    break;
                case WaveSection.Sample:
                    if (CurrentSample == null) return;
                    var newStart = ToSeconds(start);
                    if (CurrentSample.Offset != 0D) CurrentSample.Offset += newStart - CurrentSample.Start;
                    CurrentSample.Start = newStart;
                    CurrentSample.Length = ToSeconds(end - start);
                    if (CurrentSample.Offset <= CurrentSample.Start ||
                        CurrentSample.Offset >= CurrentSample.Start + CurrentSample.Length)
                        CurrentSample.Offset = 0D;
                    break;
            }
        }

        private static WaveSection SectionOf(Edge edge)
        {
            switch (edge)
            {
                case Edge.FadeInStart:
                case Edge.FadeInEnd:
                    return WaveSection.FadeIn;
                case Edge.FadeOutStart:
                case Edge.FadeOutEnd:
                    return WaveSection.FadeOut;
                case Edge.SkipStart:
                case Edge.SkipEnd:
                    return WaveSection.Skip;
                case Edge.SampleStart:
                case Edge.SampleEnd:
                case Edge.SampleOffset:
                    return WaveSection.Sample;
                default:
                    return WaveSection.None;
            }
        }

        private static bool IsEndEdge(Edge edge)
        {
            return edge == Edge.FadeInEnd || edge == Edge.FadeOutEnd || edge == Edge.SkipEnd || edge == Edge.SampleEnd;
        }

        /// <summary>
        ///     Gets the position of an edge, or -1 if it isn't there to be shown or dragged.
        /// </summary>
        private long GetEdge(Edge edge)
        {
            if (BassTrack == null) return -1;

            long start, end;
            switch (edge)
            {
                case Edge.PreFadeIn:
                    if (Mode != TrackWaveMode.Shuffler) return -1;
                    return BassTrack.UsePreFadeIn || BassTrack.PreFadeInStart > 0 ? BassTrack.PreFadeInStart : -1;
                case Edge.SampleOffset:
                    if (!GetSection(WaveSection.Sample, out start, out end) || CurrentSample.Offset == 0D) return -1;
                    return ToUnits(CurrentSample.Offset);
                case Edge.None:
                    return -1;
            }

            if (!GetSection(SectionOf(edge), out start, out end)) return -1;
            return IsEndEdge(edge) ? end : start;
        }

        /// <summary>
        ///     Moves one edge, leaving the other end of its section where it is.
        /// </summary>
        private void SetEdge(Edge edge, long position)
        {
            var min = Math.Max(1, ToUnits(MinSectionSeconds));
            position = Clamp(position, 0, BassTrack.Length);

            if (edge == Edge.PreFadeIn)
            {
                BassTrack.PreFadeInStart = Clamp(position, 0, BassTrack.FadeInStart - min);
                return;
            }

            long start, end;
            if (!GetSection(SectionOf(edge), out start, out end)) return;

            if (edge == Edge.SampleOffset)
            {
                if (position <= start || position >= end) return;
                CurrentSample.Offset = ToSeconds(position);
                return;
            }

            if (IsEndEdge(edge))
                end = Clamp(position, start + min, BassTrack.Length);
            else
                start = Clamp(position, 0, end - min);

            if (SectionOf(edge) == WaveSection.Sample)
            {
                // resizing a sample leaves its offset where it is in the track
                var offset = CurrentSample.Offset;
                CurrentSample.Start = ToSeconds(start);
                CurrentSample.Length = ToSeconds(end - start);
                CurrentSample.Offset = offset > CurrentSample.Start && offset < CurrentSample.Start + CurrentSample.Length
                    ? offset
                    : 0D;
                return;
            }

            SetSection(SectionOf(edge), start, end);
        }

        /// <summary>
        ///     Moves a whole section so that it starts at the position, keeping its length.
        /// </summary>
        private void MoveSection(WaveSection section, long newStart)
        {
            long start, end;
            if (!GetSection(section, out start, out end)) return;

            var length = end - start;
            newStart = Clamp(newStart, 0, BassTrack.Length - length);
            SetSection(section, newStart, newStart + length);
        }

        private void SetActiveSection(WaveSection section)
        {
            long start, end;
            if (section != WaveSection.None && !GetSection(section, out start, out end)) section = WaveSection.None;
            if (_activeSection == section) return;

            _activeSection = section;
            InvalidateWave(false);
            UpdateViewText();
        }

        /// <summary>
        ///     Makes the section that the view exactly frames the active one (so the Zoom buttons on the forms
        ///     select the section they zoom to); otherwise there is no active section and Play loops the view.
        /// </summary>
        private void ActivateSectionInView()
        {
            var tolerance = Math.Max(1, ToUnits(0.002));
            var match = WaveSection.None;

            foreach (var section in Sections())
            {
                long start, end;
                if (!GetSection(section, out start, out end)) continue;
                if (Math.Abs(start - ZoomStart) > tolerance || Math.Abs(end - ZoomEnd) > tolerance) continue;
                match = section;
                break;
            }

            _activeSection = match;
        }

        private static Color SectionColor(WaveSection section)
        {
            switch (section)
            {
                case WaveSection.FadeIn:
                    return FadeInColor;
                case WaveSection.FadeOut:
                    return FadeOutColor;
                case WaveSection.Skip:
                    return SkipColor;
                default:
                    return SampleColor;
            }
        }

        private static string SectionName(WaveSection section)
        {
            switch (section)
            {
                case WaveSection.FadeIn:
                    return "Fade In";
                case WaveSection.FadeOut:
                    return "Fade Out";
                case WaveSection.Skip:
                    return "Skip";
                case WaveSection.Sample:
                    return "Sample";
                default:
                    return "View";
            }
        }

        /// <summary>
        ///     The length of one beat of a section, in track positions: a section is a whole number of beats
        ///     long (16, or 16 doubled or halved until its tempo is in Shuffler's 68–136 range).
        /// </summary>
        private double SectionBeat(long start, long end, out double bpm)
        {
            bpm = (double) BpmHelper.GetBpmFromLoopLength(ToSeconds(end - start));
            return bpm <= 0 ? 0 : 60.0 / bpm / _secondsPerUnit;
        }

        private string SectionLabel(WaveSection section, long start, long end)
        {
            var seconds = ToSeconds(end - start);
            var name = section == WaveSection.Sample && CurrentSample != null
                ? CurrentSample.Description
                : SectionName(section);

            if (section == WaveSection.Skip) return name + "  ·  " + seconds.ToString("0.0") + "s";

            double bpm;
            var beat = SectionBeat(start, end, out bpm);
            if (beat <= 0) return name;

            var beats = (int) Math.Round((end - start) / beat);
            return name + "  ·  " + beats + " beats  ·  " + bpm.ToString("0.00") + " BPM";
        }

        #endregion

        #region View

        /// <summary>
        ///     Refreshes the track positions.
        /// </summary>
        public void RefreshPositions()
        {
            if (Wave == null) return;
            if (_activeSection == WaveSection.None && Mode == TrackWaveMode.Shuffler) ActivateSectionInView();
            SyncPlayingLoop();
            InvalidateWave(false);
            UpdateViewText();
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
        ///     Zooms the view to the specified position. If that is exactly a section, the section becomes the
        ///     active one; otherwise the view itself is what Play loops.
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
            ActivateSectionInView();
            InvalidateWave(true);
        }

        /// <summary>
        ///     Changes what is shown without changing the cursor or the active section.
        /// </summary>
        private void SetView(long start, long length)
        {
            if (BassTrack == null || BassTrack.Length <= 0) return;

            length = Clamp(length, Math.Max(1, ToUnits(MinViewSeconds)), BassTrack.Length);
            start = Clamp(start, 0, BassTrack.Length - length);

            ZoomStart = start;
            ZoomEnd = start + length;
            InvalidateWave(true);
        }

        /// <summary>
        ///     Zooms in or out around a position, which stays where it is on screen.
        /// </summary>
        private void ZoomAround(double factor, long anchor)
        {
            if (ZoomLength <= 0) return;

            var length = (long) Math.Round(ZoomLength * factor);
            length = Clamp(length, Math.Max(1, ToUnits(MinViewSeconds)), BassTrack.Length);
            var fraction = (anchor - ZoomStart) / (double) ZoomLength;
            SetView(anchor - (long) Math.Round(fraction * length), length);
        }

        /// <summary>The cursor if it is on screen (or the playhead while playing), else the middle of the view.</summary>
        private long ZoomAnchor()
        {
            if (_playing && _playPosition >= ZoomStart && _playPosition <= ZoomEnd) return _playPosition;
            if (CurrentPosition >= ZoomStart && CurrentPosition <= ZoomEnd) return CurrentPosition;
            return ZoomStart + ZoomLength / 2;
        }

        /// <summary>
        ///     Shows a section with a little room either side, so that both of its edges can be dragged.
        /// </summary>
        public void ZoomToSection(WaveSection section)
        {
            long start, end;
            if (!GetSection(section, out start, out end)) return;

            SetActiveSection(section);
            var padding = (long) ((end - start) * 0.15);
            SetView(start - padding, end - start + padding * 2);
            if (CurrentPosition < start || CurrentPosition > end) CurrentPosition = start;
            UpdateViewText();
        }

        /// <summary>Scrolls the view, keeping its zoom, until the range is on screen.</summary>
        private void EnsureVisible(long start, long end)
        {
            if (start >= ZoomStart && end <= ZoomEnd) return;

            var length = ZoomLength;
            if (end - start >= length)
                SetView(start, length);
            else if (start < ZoomStart)
                SetView(start - length / 20, length);
            else
                SetView(end + length / 20 - length, length);
        }

        /// <summary>
        ///     Zooms in: the view (which is the loop, when no section is active) is halved, keeping its start.
        /// </summary>
        private void ZoomIn()
        {
            var zoomLength = ZoomLength;
            zoomLength = (int) (zoomLength * ZoomRatio);
            Zoom(zoomLength);
        }

        /// <summary>
        ///     Zooms to the specified zoom length.
        /// </summary>
        /// <param name="zoomLength">Length of the zoom.</param>
        private void Zoom(long zoomLength)
        {
            if (zoomLength <= 0) return;

            ZoomEnd = ZoomStart + zoomLength;
            if (ZoomEnd > BassTrack.Length)
            {
                ZoomEnd = BassTrack.Length;
                ZoomStart = BassTrack.Length - zoomLength;
            }
            if (ZoomEnd < 0) ZoomEnd = BassTrack.Length;
            if (ZoomStart < 0) ZoomStart = 0;
            InvalidateWave(true);
        }

        /// <summary>
        ///     Zooms out.
        /// </summary>
        private void ZoomOut()
        {
            var zoomLength = ZoomLength;
            zoomLength = (int) (zoomLength / ZoomRatio);
            Zoom(zoomLength);
        }

        /// <summary>
        ///     Zooms to full.
        /// </summary>
        private void ZoomFull()
        {
            ZoomStart = 0;
            ZoomEnd = BassTrack.Length;
            InvalidateWave(true);
        }

        /// <summary>
        ///     Redraws the waveform (safe to call from any thread).
        /// </summary>
        /// <param name="viewChanged">True if the waveform image itself needs rebuilding (zoom, scroll, render).</param>
        private void InvalidateWave(bool viewChanged)
        {
            if (viewChanged) _waveDirty = true;
            if (IsDisposed || !IsHandleCreated) return;

            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke((MethodInvoker) (() => InvalidateWave(viewChanged)));
                }
                catch (InvalidOperationException)
                {
                    // closing
                }
                return;
            }

            picWaveForm.Invalidate();
            if (!viewChanged) return;

            UpdateScrollBar();
            UpdateViewText();
        }

        private void UpdateScrollBar()
        {
            if (BassTrack == null) return;

            _updatingScroll = true;
            try
            {
                var durationMs = (int) Math.Ceiling(ToSeconds(BassTrack.Length) * 1000);
                var viewMs = (int) Math.Max(1, ToSeconds(ZoomLength) * 1000);
                scrollBar.Minimum = 0;
                scrollBar.Maximum = Math.Max(1, durationMs);
                scrollBar.LargeChange = Math.Max(1, Math.Min(viewMs, scrollBar.Maximum));
                scrollBar.SmallChange = Math.Max(1, viewMs / 10);
                scrollBar.Value = Math.Max(0,
                    Math.Min((int) (ToSeconds(ZoomStart) * 1000), scrollBar.Maximum - scrollBar.LargeChange + 1));
            }
            finally
            {
                _updatingScroll = false;
            }
        }

        private void UpdateViewText()
        {
            if (BassTrack == null) return;

            var start = TimeFormatHelper.GetFormattedSeconds(ToSeconds(ZoomStart));
            var end = TimeFormatHelper.GetFormattedSeconds(ToSeconds(ZoomEnd));
            var length = TimeFormatHelper.GetFormattedSeconds(ToSeconds(ZoomLength));
            var position = TimeFormatHelper.GetFormattedSeconds(ToSeconds(CurrentPosition));

            var loop = SectionName(_activeSection);
            var candidates = Candidates(_activeSection);
            var candidate = CurrentCandidate(_activeSection);
            if (candidates != null && candidate >= 0) loop += $" (suggestion {candidate + 1} of {candidates.Count})";

            lblViewDetails.Text = $"View: {start} to {end} ({length})   Cursor: {position}   Loop: {loop}";
        }

        #endregion

        #region Painting

        private int Scale96(int pixels)
        {
            return (int) Math.Round(pixels * DeviceDpi / 96.0);
        }

        private int XAt(long position)
        {
            if (ZoomLength <= 0) return 0;
            var x = (position - ZoomStart) / (double) ZoomLength * picWaveForm.ClientSize.Width;
            return (int) Math.Round(Math.Max(-20000, Math.Min(20000, x)));
        }

        private long PositionAt(int x)
        {
            var width = Math.Max(1, picWaveForm.ClientSize.Width);
            var position = ZoomStart + (long) Math.Round(x / (double) width * ZoomLength);
            return Clamp(position, 0, BassTrack.Length);
        }

        private void picWaveForm_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.Black);

            if (Wave == null || BassTrack == null || ZoomLength <= 0) return;

            var area = WaveArea;
            if (area.Width <= 0 || area.Height <= 0) return;

            DrawWaveBitmap(g, area);
            DrawSectionFills(g, area);
            DrawBeatGrids(g, area);
            DrawRangeDrag(g, area);
            DrawMarkers(g, area);
            DrawSectionBands(g, area);
            DrawCandidates(g);
            DrawRuler(g, area);
            DrawCursors(g, area);
        }

        private void DrawWaveBitmap(Graphics g, Rectangle area)
        {
            if (_waveDirty || _waveBitmap == null || _waveBitmap.Width != area.Width || _waveBitmap.Height != area.Height)
            {
                Bitmap bitmap = null;
                try
                {
                    bitmap = Wave.CreateBitmap(area.Width, area.Height,
                        Wave.Position2Frames(ToSeconds(ZoomStart)),
                        Wave.Position2Frames(ToSeconds(ZoomEnd)),
                        true);
                }
                catch
                {
                    // the wave data isn't ready yet
                }

                if (bitmap != null)
                {
                    _waveBitmap?.Dispose();
                    _waveBitmap = bitmap;
                    _waveDirty = false;
                }
            }

            if (_waveBitmap != null) g.DrawImageUnscaled(_waveBitmap, area.Left, area.Top);
        }

        private void DrawSectionFills(Graphics g, Rectangle area)
        {
            foreach (var section in Sections())
            {
                long start, end;
                if (!GetSection(section, out start, out end)) continue;
                if (end < ZoomStart || start > ZoomEnd) continue;

                var x1 = XAt(start);
                var x2 = XAt(end);
                var alpha = section == _activeSection ? 55 : 22;
                using (var brush = new SolidBrush(Color.FromArgb(alpha, SectionColor(section))))
                    g.FillRectangle(brush, x1, area.Top, Math.Max(1, x2 - x1), area.Height);
            }
        }

        /// <summary>
        ///     Each fade section (or the sample) gets a beat grid anchored on its start, at its own tempo, so its
        ///     last line lands exactly on its end: if the drum hits sit on the lines, the section is on the beat.
        ///     The grid of the active section carries on faintly across the rest of the track.
        /// </summary>
        private void DrawBeatGrids(Graphics g, Rectangle area)
        {
            var gridSections = new List<WaveSection>();
            foreach (var section in Sections())
            {
                long start, end;
                if (section != WaveSection.Skip && GetSection(section, out start, out end)) gridSections.Add(section);
            }
            if (gridSections.Count == 0) return;

            var reference = gridSections.Contains(_activeSection) ? _activeSection : gridSections[0];

            using (var beatPen = new Pen(Color.FromArgb(70, 255, 255, 255)))
            using (var barPen = new Pen(Color.FromArgb(150, 255, 255, 255)))
            using (var outsideBarPen = new Pen(Color.FromArgb(38, 255, 255, 255)))
            using (var outsidePhrasePen = new Pen(Color.FromArgb(80, 255, 255, 255)))
            {
                foreach (var section in gridSections)
                {
                    long start, end;
                    GetSection(section, out start, out end);

                    double bpm;
                    var beat = SectionBeat(start, end, out bpm);
                    if (beat <= 0) continue;

                    var pixelsPerBeat = beat / ZoomLength * area.Width;

                    if (pixelsPerBeat * 4 >= 4)
                    {
                        var first = (long) Math.Ceiling((Math.Max(start, ZoomStart) - start) / beat - 0.001);
                        var last = (long) Math.Floor((Math.Min(end, ZoomEnd) - start) / beat + 0.001);
                        for (var i = first; i <= last && last - first < 4000; i++)
                        {
                            var pen = i % 4 == 0 ? barPen : pixelsPerBeat >= 6 ? beatPen : null;
                            if (pen == null) continue;
                            var x = XAt(start + (long) Math.Round(i * beat));
                            g.DrawLine(pen, x, area.Top, x, area.Bottom);
                        }
                    }

                    if (section != reference || pixelsPerBeat * 4 < Scale96(24)) continue;

                    // bar lines outside the sections, from the reference section's grid
                    var firstOutside = (long) Math.Floor((ZoomStart - start) / beat);
                    var lastOutside = (long) Math.Ceiling((ZoomEnd - start) / beat);
                    for (var i = firstOutside; i <= lastOutside && lastOutside - firstOutside < 4000; i++)
                    {
                        if (i % 4 != 0) continue;
                        var position = start + (long) Math.Round(i * beat);
                        if (IsInsideGridSection(position, gridSections)) continue;

                        var x = XAt(position);
                        g.DrawLine(i % 16 == 0 ? outsidePhrasePen : outsideBarPen, x, area.Top, x, area.Bottom);
                    }
                }
            }
        }

        private bool IsInsideGridSection(long position, IEnumerable<WaveSection> gridSections)
        {
            foreach (var section in gridSections)
            {
                long start, end;
                if (GetSection(section, out start, out end) && position >= start && position <= end) return true;
            }
            return false;
        }

        /// <summary>The range being dragged out on an empty part of the waveform (it becomes the view).</summary>
        private void DrawRangeDrag(Graphics g, Rectangle area)
        {
            if (_drag != DragMode.Range) return;

            var x1 = XAt(Math.Min(_mouseDownPosition, _dragRangeEnd));
            var x2 = XAt(Math.Max(_mouseDownPosition, _dragRangeEnd));
            using (var brush = new SolidBrush(Color.FromArgb(50, 255, 255, 255)))
                g.FillRectangle(brush, x1, area.Top, Math.Max(1, x2 - x1), area.Height);
        }

        private void DrawMarkers(Graphics g, Rectangle area)
        {
            // the edges that can be dragged
            foreach (var edge in Edges())
            {
                var position = GetEdge(edge);
                if (position < ZoomStart || position > ZoomEnd) continue;

                var section = SectionOf(edge);
                var color = edge == Edge.PreFadeIn ? Color.Turquoise
                    : edge == Edge.SampleOffset ? Color.LimeGreen
                    : SectionColor(section);
                var width = section != WaveSection.None && section == _activeSection ? 2 : 1;
                var x = XAt(position);
                using (var pen = new Pen(color, width))
                    g.DrawLine(pen, x, area.Top, x, area.Bottom);

                if (edge == Edge.PreFadeIn) DrawTag(g, area, x, "Pre-Fade", color);
                if (edge == Edge.SampleOffset) DrawTag(g, area, x, "Offset", color);
            }

            // the track's samples (Shuffler mode) / the other samples (Sampler mode)
            if (Mode == TrackWaveMode.Shuffler && TrackSamples != null)
            {
                foreach (var sample in TrackSamples)
                {
                    if (sample.Start == 0 && sample.Length == 0) continue;
                    DrawSpan(g, area, sample.Start, sample.Length, sample.Description, OtherSampleColor);
                }
            }

            if (Mode == TrackWaveMode.Sampler && Samples != null)
            {
                foreach (var sample in Samples)
                {
                    if (sample == CurrentSample) continue;
                    if (sample.Start == 0 && sample.Length == 0) continue;
                    DrawSpan(g, area, sample.Start, sample.Length, sample.Description, OtherSampleColor);
                }
            }

            if (Mode != TrackWaveMode.Shuffler || !ShowTrackFx) return;

            var attributes = AutomationAttributesHelper.GetAutomationAttributes(BassTrack.Description);
            foreach (var trackFx in attributes.TrackFXTriggers)
            {
                DrawSpan(g, area, trackFx.Start, trackFx.Length, "FX", TrackFxColor);
            }
        }

        /// <summary>A start and end line that can't be dragged, with a name at the foot of the start line.</summary>
        private void DrawSpan(Graphics g, Rectangle area, double startSeconds, double lengthSeconds, string name, Color color)
        {
            var start = ToUnits(startSeconds);
            var end = ToUnits(startSeconds + lengthSeconds);
            if (end < ZoomStart || start > ZoomEnd) return;

            using (var pen = new Pen(color))
            {
                var x1 = XAt(start);
                var x2 = XAt(end);
                g.DrawLine(pen, x1, area.Top, x1, area.Bottom);
                g.DrawLine(pen, x2, area.Top, x2, area.Bottom);
                DrawTag(g, area, x1, name, color);
            }
        }

        private void DrawTag(Graphics g, Rectangle area, int x, string text, Color color)
        {
            if (string.IsNullOrEmpty(text)) return;

            var size = TextRenderer.MeasureText(g, text, _smallFont);
            var box = new Rectangle(x + 1, area.Bottom - size.Height - 1, size.Width, size.Height);
            using (var brush = new SolidBrush(Color.FromArgb(200, color)))
                g.FillRectangle(brush, box);
            TextRenderer.DrawText(g, text, _smallFont, box, Color.Black,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        }

        /// <summary>
        ///     The band along the top of each section: its name, length in beats and tempo. Click it to make the
        ///     section the active one, drag it to move the whole section.
        /// </summary>
        private void DrawSectionBands(Graphics g, Rectangle area)
        {
            foreach (var section in Sections())
            {
                long start, end;
                if (!GetSection(section, out start, out end)) continue;
                if (end < ZoomStart || start > ZoomEnd) continue;

                var x1 = XAt(start);
                var x2 = XAt(end);
                var active = section == _activeSection;
                var band = new Rectangle(x1, area.Top, Math.Max(2, x2 - x1), BandHeight);

                using (var brush = new SolidBrush(Color.FromArgb(active ? 235 : 150, SectionColor(section))))
                    g.FillRectangle(brush, band);

                var textLeft = Math.Max(band.Left, 0) + Scale96(3);
                var text = new Rectangle(textLeft, band.Top, Math.Min(band.Right, area.Right) - textLeft, band.Height);
                if (text.Width < Scale96(24)) continue;

                TextRenderer.DrawText(g, SectionLabel(section, start, end), _smallFont, text, Color.Black,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis |
                    TextFormatFlags.NoPadding);
            }
        }

        private void DrawRuler(Graphics g, Rectangle area)
        {
            var height = RulerHeight;
            using (var brush = new SolidBrush(RulerColor))
                g.FillRectangle(brush, 0, 0, area.Width, height);

            var viewSeconds = ToSeconds(ZoomLength);
            var viewStart = ToSeconds(ZoomStart);
            var viewEnd = ToSeconds(ZoomEnd);

            // time ticks
            double[] steps = { 0.05, 0.1, 0.25, 0.5, 1, 2, 5, 10, 15, 30, 60, 120, 300 };
            var step = 600.0;
            foreach (var candidate in steps)
            {
                if (candidate / viewSeconds * area.Width < Scale96(80)) continue;
                step = candidate;
                break;
            }

            using (var pen = new Pen(Color.FromArgb(120, 120, 130)))
            {
                for (var t = Math.Ceiling(viewStart / step) * step; t <= viewEnd; t += step)
                {
                    var x = XAt(ToUnits(t));
                    g.DrawLine(pen, x, height - Scale96(5), x, height);
                    TextRenderer.DrawText(g, FormatRulerTime(t, step), _smallFont, new Point(x + Scale96(2), 0),
                        RulerTextColor, TextFormatFlags.NoPadding);
                }
            }

            // the beats found in the audio (what Snap snaps to), when zoomed in far enough to tell them apart
            if (_beatAnalysis == null || viewSeconds > 45) return;

            var grid = _beatAnalysis.GetGridAt((viewStart + viewEnd) / 2);
            if (grid == null || grid.Confidence < BeatAnalysis.MinSnapConfidence) return;
            if (grid.BeatLength / viewSeconds * area.Width < Scale96(10)) return;

            using (var pen = new Pen(_snap ? DetectedBeatColor : Color.FromArgb(110, DetectedBeatColor), Scale96(2)))
            {
                for (var t = grid.Snap(viewStart) - grid.BeatLength; t <= viewEnd; t += grid.BeatLength)
                {
                    if (t < viewStart) continue;
                    var x = XAt(ToUnits(t));
                    g.DrawLine(pen, x, height - Scale96(6), x, height);
                }
            }
        }

        private static string FormatRulerTime(double seconds, double step)
        {
            var minutes = (int) (seconds / 60);
            var remainder = seconds - minutes * 60;
            var format = step < 0.1 ? "00.00" : step < 1 ? "00.0" : "00";
            return minutes + ":" + remainder.ToString(format);
        }

        private void DrawCursors(Graphics g, Rectangle area)
        {
            if (CurrentPosition >= ZoomStart && CurrentPosition <= ZoomEnd)
            {
                var x = XAt(CurrentPosition);
                g.DrawLine(Pens.Red, x, RulerHeight, x, area.Bottom);
            }

            if (!_playing || _playPosition < ZoomStart || _playPosition > ZoomEnd) return;

            // the playhead: thicker than the grid lines, with a flag in the ruler
            var playX = XAt(_playPosition);
            using (var pen = new Pen(Color.White, Scale96(2)))
                g.DrawLine(pen, playX, 0, playX, area.Bottom);

            var flag = Scale96(6);
            g.FillPolygon(Brushes.White,
                new[] { new Point(playX - flag, 0), new Point(playX + flag, 0), new Point(playX, flag * 2) });
        }

        #endregion

        #region Mouse

        private Edge HitTestEdge(int x, int y)
        {
            var best = Edge.None;
            var bestDistance = int.MaxValue;

            foreach (var edge in Edges())
            {
                var position = GetEdge(edge);
                if (position < 0) continue;

                var distance = Math.Abs(x - XAt(position));
                if (distance > GrabPixels) continue;

                // where two edges meet, the active section's edge wins
                if (SectionOf(edge) == _activeSection && _activeSection != WaveSection.None) distance -= GrabPixels;
                if (distance >= bestDistance) continue;

                bestDistance = distance;
                best = edge;
            }

            return best;
        }

        private WaveSection HitTestBand(int x, int y)
        {
            if (y < RulerHeight || y > RulerHeight + BandHeight) return WaveSection.None;

            foreach (var section in Sections())
            {
                long start, end;
                if (!GetSection(section, out start, out end)) continue;
                if (x >= XAt(start) && x <= XAt(end)) return section;
            }
            return WaveSection.None;
        }

        private void picWaveForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (BassTrack == null || ZoomLength <= 0) return;

            // take the focus from any text box, so that the keyboard shortcuts work
            Select();

            _mouseDownX = e.X;
            _mouseDownPosition = PositionAt(e.X);
            _dragMoved = false;

            if (e.Button == MouseButtons.Left)
            {
                var edge = HitTestEdge(e.X, e.Y);
                var band = edge == Edge.None ? HitTestBand(e.X, e.Y) : WaveSection.None;

                WaveSection candidateSection;
                int candidate;
                if (edge == Edge.None && HitTestCandidate(e.X, e.Y, out candidateSection, out candidate))
                {
                    // a suggestion box: move the section there
                    _drag = DragMode.None;
                    ApplyCandidate(candidateSection, candidate);
                    return;
                }

                if (edge != Edge.None)
                {
                    _drag = DragMode.Edge;
                    _dragEdge = edge;
                    if (SectionOf(edge) != WaveSection.None) SetActiveSection(SectionOf(edge));
                }
                else if (band != WaveSection.None)
                {
                    long end;
                    GetSection(band, out _dragSectionStart, out end);
                    _drag = DragMode.Section;
                    _dragSection = band;
                    SetActiveSection(band);
                }
                else
                {
                    _drag = DragMode.Pending;
                }
            }
            else if (e.Button == MouseButtons.Middle)
            {
                _drag = DragMode.Pan;
                _panStart = ZoomStart;
                picWaveForm.Cursor = Cursors.SizeAll;
            }
        }

        private void picWaveForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (BassTrack == null || ZoomLength <= 0) return;

            var position = PositionAt(e.X);
            var moved = Math.Abs(e.X - _mouseDownX) > 3;

            switch (_drag)
            {
                case DragMode.None:
                    WaveSection hoverSection;
                    int hoverCandidate;
                    if (HitTestEdge(e.X, e.Y) != Edge.None) picWaveForm.Cursor = Cursors.SizeWE;
                    else if (HitTestCandidate(e.X, e.Y, out hoverSection, out hoverCandidate)) picWaveForm.Cursor = Cursors.Hand;
                    else if (HitTestBand(e.X, e.Y) != WaveSection.None) picWaveForm.Cursor = Cursors.Hand;
                    else picWaveForm.Cursor = Cursors.Default;
                    break;

                case DragMode.Pending:
                    if (!moved) break;
                    _drag = DragMode.Range;
                    _dragRangeEnd = position;
                    InvalidateWave(false);
                    break;

                case DragMode.Range:
                    _dragRangeEnd = position;
                    InvalidateWave(false);
                    break;

                case DragMode.Edge:
                    if (!_dragMoved && !moved) break;
                    _dragMoved = true;
                    DragEdgeTo(_dragEdge, position);
                    OnSectionEdited(true);
                    break;

                case DragMode.Section:
                    if (!_dragMoved && !moved) break;
                    _dragMoved = true;
                    MoveSection(_dragSection, SnapPosition(_dragSectionStart + position - _mouseDownPosition));
                    OnSectionEdited(true);
                    break;

                case DragMode.Pan:
                    var delta = (long) Math.Round((e.X - _mouseDownX) / (double) Math.Max(1, picWaveForm.ClientSize.Width) * ZoomLength);
                    SetView(_panStart - delta, ZoomLength);
                    break;
            }
        }

        private void DragEdgeTo(Edge edge, long position)
        {
            if (IsEndEdge(edge))
            {
                long start, end;
                if (GetSection(SectionOf(edge), out start, out end)) position = SnapEnd(start, position);
            }
            else
            {
                position = SnapPosition(position);
            }

            SetEdge(edge, position);
        }

        /// <summary>
        ///     Handles the MouseUp event of the picWaveForm control.
        /// </summary>
        private void picWaveForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (BassTrack == null || ZoomLength <= 0) return;

            var drag = _drag;
            _drag = DragMode.None;
            picWaveForm.Cursor = Cursors.Default;

            switch (drag)
            {
                case DragMode.Edge:
                case DragMode.Section:
                    if (_dragMoved) OnSectionEdited(false);
                    else InvalidateWave(false);
                    return;

                case DragMode.Range:
                    // dragging out a range is the same as click then shift-click: it becomes the view
                    var start = Math.Min(_mouseDownPosition, _dragRangeEnd);
                    var end = Math.Max(_mouseDownPosition, _dragRangeEnd);
                    if (end - start > ToUnits(MinViewSeconds)) Zoom(start, end);
                    else InvalidateWave(false);
                    return;

                case DragMode.Pan:
                    return;
            }

            if (e.Button != MouseButtons.Left && e.Button != MouseButtons.Right) return;

            // letting go after clicking a suggestion box (the box already did its job)
            if (drag == DragMode.None && e.Button == MouseButtons.Left) return;

            var position = PositionAt(e.X);

            if (e.Button == MouseButtons.Left && ModifierKeys == Keys.Shift)
            {
                Zoom(CurrentPosition, position);
            }
            else
            {
                CurrentPosition = SnapPosition(position);
                InvalidateWave(false);
                UpdateViewText();
            }
        }

        private void picWaveForm_DoubleClick(object sender, EventArgs e)
        {
            Play();
        }

        /// <summary>
        ///     Handles the Resize event of the picWaveForm control.
        /// </summary>
        private void picWaveForm_Resize(object sender, EventArgs e)
        {
            InvalidateWave(true);
        }

        /// <summary>
        ///     Mouse wheel over the waveform: zoom around the pointer, or scroll with Shift. (The waveform can't
        ///     take the focus, so the wheel is picked up here, wherever the focus is.)
        /// </summary>
        public bool PreFilterMessage(ref Message m)
        {
            const int wmMouseWheel = 0x020A;
            if (m.Msg != wmMouseWheel) return false;
            if (BassTrack == null || ZoomLength <= 0 || !Visible || !picWaveForm.IsHandleCreated) return false;

            var form = FindForm();
            if (form == null || Form.ActiveForm != form) return false;

            var point = picWaveForm.PointToClient(MousePosition);
            if (!picWaveForm.ClientRectangle.Contains(point)) return false;

            var delta = (short) ((m.WParam.ToInt64() >> 16) & 0xFFFF);
            var notches = delta / 120.0;

            if ((ModifierKeys & Keys.Shift) == Keys.Shift)
                SetView(ZoomStart - (long) (notches * 0.15 * ZoomLength), ZoomLength);
            else
                ZoomAround(Math.Pow(0.8, notches), PositionAt(point.X));

            return true;
        }

        /// <summary>
        ///     Handles the ValueChanged event of the scrollBar control.
        /// </summary>
        private void scrollBar_ValueChanged(object sender, EventArgs e)
        {
            if (_updatingScroll || BassTrack == null) return;
            SetView(ToUnits(scrollBar.Value / 1000.0), ZoomLength);
        }

        #endregion

        #region Editing

        /// <summary>
        ///     Called whenever a section changes. While a drag is still going the forms are only told in Shuffler
        ///     mode (where it's cheap), so that the fade lengths and BPMs follow the mouse.
        /// </summary>
        private void OnSectionEdited(bool dragging)
        {
            SyncPlayingLoop();
            InvalidateWave(false);

            if (dragging && Mode != TrackWaveMode.Shuffler) return;
            PositionsChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Updates the fade positions.
        /// </summary>
        private void UpdatePositions()
        {
            SyncPlayingLoop();
            InvalidateWave(false);
            UpdateViewText();

            // raise track changed event
            PositionsChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>Halves or doubles the active section, keeping its start (so its tempo stays the same).</summary>
        private void ScaleActiveSection(double factor)
        {
            long start, end;
            if (!GetSection(_activeSection, out start, out end)) return;

            var length = (long) Math.Round((end - start) * factor);
            if (length < ToUnits(MinSectionSeconds) || start + length > BassTrack.Length) return;

            SetSection(_activeSection, start, start + length);
            UpdatePositions();
        }

        /// <summary>
        ///     Moves the active section back or forward by its own length: the previous / next phrase. With Snap
        ///     on it lands on the beat there, which keeps it on the beat even if the tempo drifts.
        /// </summary>
        private void ShiftActiveSection(int direction)
        {
            long start, end;
            if (!GetSection(_activeSection, out start, out end)) return;

            var length = end - start;
            var newStart = start + direction * length;
            if (newStart < 0 || newStart + length > BassTrack.Length) return;

            MoveSection(_activeSection, SnapPosition(newStart));

            GetSection(_activeSection, out start, out end);
            EnsureVisible(start, end);
            UpdatePositions();
        }

        /// <summary>The playhead while playing, otherwise the cursor.</summary>
        private long PlayheadOrCursor()
        {
            return _playing && _playPosition >= 0 ? _playPosition : CurrentPosition;
        }

        private void SetActiveEdge(bool end, long position)
        {
            Edge edge;
            switch (_activeSection)
            {
                case WaveSection.FadeIn:
                    edge = end ? Edge.FadeInEnd : Edge.FadeInStart;
                    break;
                case WaveSection.FadeOut:
                    edge = end ? Edge.FadeOutEnd : Edge.FadeOutStart;
                    break;
                case WaveSection.Skip:
                    edge = end ? Edge.SkipEnd : Edge.SkipStart;
                    break;
                case WaveSection.Sample:
                    edge = end ? Edge.SampleEnd : Edge.SampleStart;
                    break;
                default:
                    return;
            }

            DragEdgeTo(edge, position);
            UpdatePositions();
        }

        /// <summary>
        ///     Handles a keyboard shortcut. The forms call this from ProcessCmdKey when the focus isn't somewhere
        ///     that needs the key for typing.
        /// </summary>
        /// <returns>True if the key was a shortcut.</returns>
        public bool HandleKey(Keys keyData)
        {
            if (BassTrack == null || ZoomLength <= 0) return false;

            switch (keyData)
            {
                case Keys.Space:
                    if (_playing) Stop();
                    else Play();
                    return true;
                case Keys.Oemplus:
                case Keys.Add:
                    ZoomAround(0.5, ZoomAnchor());
                    return true;
                case Keys.OemMinus:
                case Keys.Subtract:
                    ZoomAround(2, ZoomAnchor());
                    return true;
                case Keys.Control | Keys.D0:
                    SetView(0, BassTrack.Length);
                    return true;
                case Keys.Z:
                    ZoomToSection(_activeSection);
                    return true;
                case Keys.OemOpenBrackets:
                    ScaleActiveSection(0.5);
                    return true;
                case Keys.OemCloseBrackets:
                    ScaleActiveSection(2);
                    return true;
                case Keys.Oemcomma:
                    ShiftActiveSection(-1);
                    return true;
                case Keys.OemPeriod:
                    ShiftActiveSection(1);
                    return true;
                case Keys.S:
                    SetActiveEdge(false, PlayheadOrCursor());
                    return true;
                case Keys.E:
                    SetActiveEdge(true, PlayheadOrCursor());
                    return true;
                case Keys.N:
                    SnapToBeat = !SnapToBeat;
                    return true;
                case Keys.Down:
                    if (Mode != TrackWaveMode.Shuffler) return false;
                    CycleCandidate(1);
                    return true;
                case Keys.Up:
                    if (Mode != TrackWaveMode.Shuffler) return false;
                    CycleCandidate(-1);
                    return true;
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                    if (Mode != TrackWaveMode.Shuffler) return false;
                    ApplyCandidate(_activeSection == WaveSection.FadeOut ? WaveSection.FadeOut : WaveSection.FadeIn,
                        keyData - Keys.D1);
                    return true;
                case Keys.I:
                    if (Mode != TrackWaveMode.Shuffler) return false;
                    ZoomToSection(WaveSection.FadeIn);
                    return true;
                case Keys.O:
                    if (Mode != TrackWaveMode.Shuffler) return false;
                    ZoomToSection(WaveSection.FadeOut);
                    return true;
                case Keys.K:
                    if (Mode != TrackWaveMode.Shuffler) return false;
                    ZoomToSection(WaveSection.Skip);
                    return true;
            }

            return false;
        }

        private void ShowShortcuts()
        {
            var sections = Mode == TrackWaveMode.Shuffler
                ? "  I / O / K        Select and show the fade-in / fade-out / skip section\n" +
                  "  Down / Up        Move the fade-in (or fade-out, if selected) to its next / previous suggestion\n" +
                  "  1 - 5            Move it to that suggestion (the numbered boxes; 1 is the best guess)\n"
                : "";

            MessageBox.Show(FindForm(),
                "Click the waveform first if the keys are going to a text box.\n\n" +
                "Playback\n" +
                "  Space            Play / stop from the red cursor. Loops the selected section if the cursor is in it;\n" +
                "                   otherwise loops the view (or plays on to the end if the cursor is off screen)\n\n" +
                "Sections (the selected one is brighter, and named after 'Loop:')\n" +
                sections +
                "  S / E            Set the section's start / end at the cursor (the playhead while playing)\n" +
                "  [  /  ]          Halve / double the section's length\n" +
                "  ,  /  .          Move the section back / forward by its own length\n" +
                "  Z                Show the section with room to drag its edges\n" +
                "  N                Snap to beat on / off (hold Ctrl while dragging for the opposite)\n\n" +
                "Mouse\n" +
                "  Click            Put the red cursor there\n" +
                "  Drag an edge     Move that edge;  drag a section's coloured band to move all of it\n" +
                "  Click a band     Select that section\n" +
                "  Drag elsewhere   Zoom to the range dragged (as click then Shift+click)\n" +
                "  Right-click      Set / Move commands at the cursor\n" +
                "  Wheel            Zoom around the pointer;  Shift+Wheel scrolls;  middle-drag pans\n" +
                "  +  /  -          Zoom in / out;  Ctrl+0 shows the whole track\n\n" +
                "The Zoom In / Zoom Out / < / > buttons change the view as a loop, as before: they deselect the section.",
                "Waveform Shortcuts", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Playback

        /// <summary>
        ///     Plays from the red cursor. If the cursor is inside the selected section, loops that section;
        ///     otherwise loops the view, or plays to the end of the track if the cursor is outside the view.
        ///     The section stays selected either way, so it can still be edited.
        /// </summary>
        private void Play()
        {
            if (BassTrack == null || ZoomLength <= 0) return;

            long start, end;
            if (GetSection(_activeSection, out start, out end) && CurrentPosition >= start && CurrentPosition <= end)
            {
                // wraps back to the section's start (a cursor on the end, e.g. after setting the end, starts there)
                var from = CurrentPosition < end ? CurrentPosition : start;
                BassPlayer.SetRawLoopPositions(start, end, from);
                BassPlayer.PlayRawLoop();
                if (from != start) BassPlayer.UpdateRawLoopPositions(start, end);
                _loopingSection = true;
            }
            else
            {
                if (CurrentPosition < 0 || CurrentPosition >= BassTrack.Length) CurrentPosition = ZoomStart;
                var inView = CurrentPosition >= ZoomStart && CurrentPosition < ZoomEnd;
                var loopStart = inView ? ZoomStart : 0;
                var loopEnd = inView ? ZoomEnd : BassTrack.Length;
                BassPlayer.SetRawLoopPositions(loopStart, loopEnd, CurrentPosition);
                BassPlayer.PlayRawLoop();
                if (CurrentPosition != loopStart) BassPlayer.UpdateRawLoopPositions(loopStart, loopEnd);
                _loopingSection = false;
            }

            _playing = true;
            _timer.Enabled = true;
            InvalidateWave(false);
        }

        public void Stop()
        {
            _timer.Enabled = false;
            _playing = false;
            _playPosition = -1;
            BassPlayer.StopRawLoop();
            InvalidateWave(false);
        }

        /// <summary>
        ///     Keeps a playing loop on the active section while the section is being edited, without restarting it.
        /// </summary>
        private void SyncPlayingLoop()
        {
            // playing from the cursor outside the section: editing the section mustn't pull playback into it
            if (!_playing || !_loopingSection || BassPlayer == null) return;

            long start, end;
            if (!GetSection(_activeSection, out start, out end)) return;

            var track = BassPlayer.RawLoopTrack;
            if (track == null || (track.RawLoopStart == start && track.RawLoopEnd == end)) return;

            BassPlayer.UpdateRawLoopPositions(start, end);
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
        ///     Handles the Tick event of the timer control.
        /// </summary>
        private void timer_Tick(object sender, EventArgs e)
        {
            if (_ticking) return;
            _ticking = true;
            try
            {
                _playPosition = AudioStreamHelper.GetPosition(BassPlayer.RawLoopTrack);
                InvalidateWave(false);
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

        #endregion

        #region Buttons

        /// <summary>
        ///     The Zoom In / Zoom Out / Zoom Full / &lt; / &gt; buttons treat the view as the loop, as they always
        ///     have, so they deselect the active section. (The wheel, + and - only change what is shown.)
        /// </summary>
        private void UseViewAsLoop()
        {
            _activeSection = WaveSection.None;
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            UseViewAsLoop();
            ZoomIn();
        }

        private void btnZoomFull_Click(object sender, EventArgs e)
        {
            UseViewAsLoop();
            ZoomFull();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            UseViewAsLoop();
            ZoomOut();
        }

        private void btnlLeft_Click(object sender, EventArgs e)
        {
            UseViewAsLoop();
            var zoomLength = ZoomLength;
            ZoomStart -= zoomLength;
            Zoom(zoomLength);
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            UseViewAsLoop();
            var zoomLength = ZoomLength;
            ZoomStart += zoomLength;
            Zoom(zoomLength);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            Play();
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

        #endregion

        #region Context menu

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            var shuffler = Mode == TrackWaveMode.Shuffler;

            mnuSetFadeInEnd.Visible = shuffler;
            mnuSetFadeInStart.Visible = shuffler;
            mnuSetFadeOutEnd.Visible = shuffler;
            mnuSetFadeOutStart.Visible = shuffler;
            mnuMoveFadeInStart.Visible = shuffler;
            mnuMoveFadeInEnd.Visible = shuffler;
            mnuMoveFadeOutStart.Visible = shuffler;
            mnuMoveFadeOutEnd.Visible = shuffler;
            mnuSetSkipStart.Visible = shuffler;
            mnuSetSkipEnd.Visible = shuffler;
            mnuMoveSkipStart.Visible = shuffler;
            mnuSetPreFadeInStart.Visible = shuffler;
            toolStripMenuItem1.Visible = shuffler;
            toolStripMenuItem2.Visible = shuffler;
            toolStripMenuItem3.Visible = shuffler;
            toolStripMenuItem4.Visible = shuffler;

            mnuSetSampleEnd.Visible = !shuffler;
            mnuSetSampleStart.Visible = !shuffler;
            mnuSetSampleOffset.Visible = !shuffler;

            mnuSetSampleEnd.Enabled = CurrentSample != null;
            mnuSetSampleStart.Enabled = CurrentSample != null;
            mnuSetSampleOffset.Enabled = CurrentSample != null;

            if (_mnuSnap != null) _mnuSnap.Checked = _snap;
            if (_mnuSuggest != null) _mnuSuggest.Visible = shuffler;
        }

        private void mnuSetSampleStart_Click(object sender, EventArgs e)
        {
            if (CurrentSample == null) return;

            CurrentSample.Start = ToSeconds(CurrentPosition);
            _activeSection = WaveSection.Sample;
            UpdatePositions();
        }

        private void mnuSetSampleEnd_Click(object sender, EventArgs e)
        {
            if (CurrentSample == null) return;

            var end = ToSeconds(CurrentPosition);
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
            _activeSection = WaveSection.Sample;

            UpdatePositions();
        }

        private void mnuSetSampleOffset_Click(object sender, EventArgs e)
        {
            if (CurrentSample == null) return;

            CurrentSample.Offset = ToSeconds(CurrentPosition);
            UpdatePositions();
        }

        private void mnuSetPreFadeInStart_Click(object sender, EventArgs e)
        {
            BassTrack.PreFadeInStart = CurrentPosition;
            UpdatePositions();
        }

        private void mnuSetFadeInStart_Click(object sender, EventArgs e)
        {
            BassTrack.FadeInStart = CurrentPosition;
            _activeSection = WaveSection.FadeIn;
            UpdatePositions();
        }

        private void mnuSetFadeInEnd_Click(object sender, EventArgs e)
        {
            BassTrack.FadeInEnd = CurrentPosition;
            _activeSection = WaveSection.FadeIn;
            UpdatePositions();
        }

        private void mnuSetFadeOutStart_Click(object sender, EventArgs e)
        {
            BassTrack.FadeOutStart = CurrentPosition;
            _activeSection = WaveSection.FadeOut;
            UpdatePositions();
        }

        private void mnuSetFadeOutEnd_Click(object sender, EventArgs e)
        {
            BassTrack.FadeOutEnd = CurrentPosition;
            _activeSection = WaveSection.FadeOut;
            UpdatePositions();
        }

        private void mnuSetSkipStart_Click(object sender, EventArgs e)
        {
            BassTrack.SkipStart = CurrentPosition;
            if (BassTrack.HasSkipSection) _activeSection = WaveSection.Skip;
            UpdatePositions();
        }

        private void mnuSetSkipEnd_Click(object sender, EventArgs e)
        {
            BassTrack.SkipEnd = CurrentPosition;
            if (BassTrack.HasSkipSection) _activeSection = WaveSection.Skip;
            UpdatePositions();
        }

        private void mnuMoveFadeInStart_Click(object sender, EventArgs e)
        {
            var length = BassTrack.FadeInEnd - BassTrack.FadeInStart;

            BassTrack.FadeInStart = CurrentPosition;
            BassTrack.FadeInEnd = CurrentPosition + length;
            _activeSection = WaveSection.FadeIn;

            UpdatePositions();
        }

        private void mnuMoveFadeOutStart_Click(object sender, EventArgs e)
        {
            var length = BassTrack.FadeOutEnd - BassTrack.FadeOutStart;

            BassTrack.FadeOutStart = CurrentPosition;
            BassTrack.FadeOutEnd = CurrentPosition + length;
            _activeSection = WaveSection.FadeOut;

            UpdatePositions();
        }

        private void mnuMoveSkipStart_Click(object sender, EventArgs e)
        {
            var length = BassTrack.SkipEnd - BassTrack.SkipStart;

            BassTrack.SkipStart = CurrentPosition;
            BassTrack.SkipEnd = CurrentPosition + length;
            if (BassTrack.HasSkipSection) _activeSection = WaveSection.Skip;

            UpdatePositions();
        }

        private void mnuMoveFadeInEnd_Click(object sender, EventArgs e)
        {
            var length = BassTrack.FadeInEnd - BassTrack.FadeInStart;
            if (CurrentPosition - length < 0) return;

            BassTrack.FadeInStart = CurrentPosition - length;
            BassTrack.FadeInEnd = CurrentPosition;
            _activeSection = WaveSection.FadeIn;

            UpdatePositions();
        }

        private void mnuMoveFadeOutEnd_Click(object sender, EventArgs e)
        {
            var length = BassTrack.FadeOutEnd - BassTrack.FadeOutStart;
            if (CurrentPosition - length < 0) return;

            BassTrack.FadeOutStart = CurrentPosition - length;
            BassTrack.FadeOutEnd = CurrentPosition;
            _activeSection = WaveSection.FadeOut;

            UpdatePositions();
        }

        #endregion
    }
}
