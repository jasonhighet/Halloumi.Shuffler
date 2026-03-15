using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Halloumi.Common.Windows.Forms;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.AudioEngine.BassPlayer;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;
using Un4seen.Bass;
using Un4seen.Bass.Misc;
using AE = Halloumi.Shuffler.AudioEngine;
using Track = Halloumi.Shuffler.AudioLibrary.Models.Track;

namespace Halloumi.Shuffler.Controls
{
    public partial class PlayerDetails : UserControl
    {
        private readonly Visuals _bassVisuals = new Visuals();

        private ShufflerApplication _application;
        private bool _bassPlayerOnTrackChange;
        private bool _bindingVolumeSlider;
        private int _visualWarmupTicks;

        private bool _loaded;

        private PlaylistControl _playlistControl;

        private bool _timerTick;

        private Color _volumeColor1 = Color.Wheat;
        private Color _volumeColor2 = Color.Gold;

        /// <summary>
        ///     Initializes a new instance of the TrackDetails class.
        /// </summary>
        public PlayerDetails()
        {
            InitializeComponent();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;

            Timer = new BASSTimer(100) {Interval = 100};

            Load += TrackDetails_Load;
            KryptonManager.GlobalPaletteChanged += KryptonManager_GlobalPaletteChanged;

            slider.Slid += Slider_Slid;
            btnPlay.Click += btnPlay_Click;
            btnPause.Click += btnPlay_Click;
            Timer.Tick += Timer_Tick;

            btnPrevious.Click += btnPrevious_Click;
            btnSkipToEnd.Click += btnSkipToEnd_Click;
            btnNext.Click += btnNext_Click;
            sldVolume.Scrolled += sldVolume_Slid;

            btnRankExcellent.Click += (s, e) => RankButton_Click(5);
            btnRankVeryGood.Click += (s, e) => RankButton_Click(4);
            btnRankGood.Click += (s, e) => RankButton_Click(3);
            btnRankBearable.Click += (s, e) => RankButton_Click(2);
            btnRankForbidden.Click += (s, e) => RankButton_Click(0);
            VisualsShown = false;
            AlbumArtShown = true;

            slider.Minimum = 0;
            slider.Maximum = 0;

            SetThemeState();

            _bassVisuals.MaxFFT = BASSData.BASS_DATA_FFT1024;
            _bassVisuals.MaxFrequencySpectrum = Utils.FFTFrequency2Index(16000, 1024, 44100);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool VisualsShown { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether album art is shown.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AlbumArtShown
        {
            get { return picCover.Visible; }
            set { picCover.Visible = value; }
        }

        /// <summary>
        ///     Gets or sets the bass player.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private BassPlayer BassPlayer { get; set; }

        /// <summary>
        ///     Gets or sets the timer.
        /// </summary>
        //private System.Windows.Forms.Timer Timer { get; set; }
        private BASSTimer Timer { get; }

        /// <summary>
        ///     Gets or sets the playlist control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private PlaylistControl PlaylistControl
        {
            get { return _playlistControl; }
            set
            {
                //if (_playlistControl != null) _playlistControl.PlaylistChanged -= PlaylistControl_PlaylistChanged;
                _playlistControl = value;
                if (_playlistControl != null) _playlistControl.PlaylistChanged += PlaylistControl_PlaylistChanged;
            }
        }

        /// <summary>
        ///     Gets or sets the tool strip label.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ToolStripLabel ToolStripLabel { get; set; }

        public void Initialize(ShufflerApplication application, PlaylistControl playlistControl)
        {
            _application = application;
            this.BassPlayer = application.BassPlayer;
            this.PlaylistControl = playlistControl;

            var volume = _application.GetVolume();
            BassPlayer.SetMixerVolume(volume);
            SetVolume((int)volume);

            BassPlayer.OnVolumeChanged += BassPlayer_OnVolumeChanged;
        }

        /// <summary>
        ///     Handles the Load event of the TrackDetails control.
        /// </summary>
        private void TrackDetails_Load(object sender, EventArgs e)
        {
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            if (_loaded) return;

            DisplayCurrentTrackDetails();

            slider.Dock = DockStyle.None;
            slider.Width = pnlSlider.Width;
            slider.Dock = DockStyle.Fill;

            if (!Timer.Enabled)
            {
                Timer.Start();
                BassPlayer.OnTrackChange += BassPlayer_OnTrackChange;
            }

            sldVolume.Minimum = 0;
            sldVolume.Maximum = 100;
            var volume = Convert.ToInt32(BassPlayer.GetMixerVolume());
            lblVolume.Text = volume.ToString();
            sldVolume.Value = volume;

            _loaded = true;
        }

        /// <summary>
        ///     Sets the state of the control based on the current theme.
        /// </summary>
        private void SetThemeState()
        {
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;

            var palette = KryptonHelper.GetCurrentPalette();
            var color = KryptonManager.GetPaletteForMode(palette)
                .GetBackColor2(PaletteBackStyle.PanelAlternate, PaletteState.Normal);

            BackColor = color;
            pnlSlider.BackColor = color;
            pnlVolume.BackColor = color;
            slider.BackColor = color;
            sldVolume.BackColor = color;

            _volumeColor1 = KryptonManager.GetPaletteForMode(palette)
                .GetBackColor1(PaletteBackStyle.ButtonStandalone, PaletteState.Pressed);
            _volumeColor2 = KryptonManager.GetPaletteForMode(palette)
                .GetBackColor2(PaletteBackStyle.ButtonStandalone, PaletteState.Pressed);
        }

        /// <summary>
        ///     Displays the current track details.
        /// </summary>
        public void DisplayCurrentTrackDetails()
        {
            Track track = null;
            if (BassPlayer.CurrentTrack != null)
                track = _application.GetTrackByFilename(BassPlayer.CurrentTrack.Filename);

            if (BassPlayer.PlayState == PlayState.Playing)
            {
                if (!btnPause.Visible) btnPause.Visible = true;
                if (btnPlay.Visible) btnPlay.Visible = false;
            }

            if (BassPlayer.PlayState != PlayState.Playing)
            {
                if (btnPause.Visible) btnPause.Visible = false;
                if (!btnPlay.Visible) btnPlay.Visible = true;
            }

            if (track != null)
            {
                lblCurrentTrackDescription.Text = track.Description.Replace("&", "&&");

                var details = $"{track.Album} - {track.Genre} - {track.LengthFormatted}";

                if (track.Bpm != 0)
                    details += $" - {track.Bpm:0.00} BPM";

                if (track.Key != "")
                    details += $" - {KeyHelper.GetDisplayKey(track.Key)}";

                details += $" - {track.Bitrate:00} KPS"; ;

                lblCurrentTrackDetails.Text = details;

                picCover.Image = _application.GetAlbumCover(track.Album);
            }
            else
            {
                lblCurrentTrackDescription.Text = "";
                lblCurrentTrackDetails.Text = "";
                picCover.Image = null;
            }

            var mainForm = (BaseMinimizeToTrayForm) ParentForm;
            if (mainForm == null) return;

            if (track != null)
            {
                var text = track.Description.Replace("&", "&&");
                if (text.Length > 63) text = text.Substring(0, 63);
                mainForm.NotifyIcon.Text = text;
            }
            else
            {
                mainForm.NotifyIcon.Text = "";
            }
        }

        public int GetCurrentMixRank()
        {
            if (BassPlayer.CurrentTrack == null) return 1;

            Track currentTrack = null;
            if (BassPlayer.CurrentTrack != null)
                currentTrack = _application.GetTrackByFilename(BassPlayer.CurrentTrack.Filename);

            var prevTrack = PlaylistControl.GetPreviousTrack();
            if (prevTrack == null) return 1;

            return _application.GetMixLevel(prevTrack, currentTrack);
        }

        public void SetCurrentMixRank(int mixRank)
        {
            if (BassPlayer.CurrentTrack == null) return;

            Track currentTrack = null;
            if (BassPlayer.CurrentTrack != null)
                currentTrack = _application.GetTrackByFilename(BassPlayer.CurrentTrack.Filename);

            var prevTrack = PlaylistControl.GetPreviousTrack();
            if (prevTrack == null) return;

            _application.SetMixLevel(prevTrack, currentTrack, mixRank);
        }


        /// <summary>
        ///     Handles the Slid event of the Slider control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void Slider_Slid(object sender, EventArgs e)
        {
            Timer.Stop();
            var position = slider.Value;
            BassPlayer.SetAdjustedTrackPosition(position);
            Thread.Sleep(200);
            Timer.Start();
        }

        /// <summary>
        ///     Handles the Tick event of the Timer control.
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                BeginInvoke(new MethodInvoker(Timer_Tick));
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     Handles the Tick event of the Timer control.
        /// </summary>
        private void Timer_Tick()
        {
            var form = FindForm();
            if (form != null && form.WindowState == FormWindowState.Minimized) return;

            if (_timerTick) return;
            _timerTick = true;

            var position = BassPlayer.GetTrackPosition();

            if (slider.Maximum != position.Length) slider.Maximum = (int) position.Length;
            if (slider.Value != position.Positition) slider.Value = (int) position.Positition;

            if (lblTimeElapsed.Text != position.ElapsedFormatted) lblTimeElapsed.Text = position.ElapsedFormatted;
            if (lblTimeRemaining.Text != position.RemainingFormatted)
                lblTimeRemaining.Text = position.RemainingFormatted;

            btnPause.Visible = BassPlayer.PlayState == PlayState.Playing;
            btnPlay.Visible = BassPlayer.PlayState != PlayState.Playing;


            if (ToolStripLabel != null)
            {
                if (!Visible && BassPlayer.PlayState == PlayState.Playing)
                {
                    var text = lblCurrentTrackDescription.Text + " - " + position.ElapsedFormatted + " elapsed - " + position.RemainingFormatted + " remaining";
                    ToolStripLabel.Text = text;
                }
                else
                {
                    ToolStripLabel.Text = "";
                }
            }

            ShowVisuals();

            _timerTick = false;
        }

        private void ShowVisuals()
        {
            picVisuals.Visible = VisualsShown && BassPlayer.PlayState == PlayState.Playing;
            if (!VisualsShown) return;
            if (BassPlayer.PlayState == PlayState.Playing)
            {
                _visualWarmupTicks++;
                if (_visualWarmupTicks < 3) return;

                //lock (BassPlayer.ExternalMixerLock)
                {
                    picVisuals.Image = _bassVisuals.CreateSpectrumLine(BassPlayer.SpeakerOutput.ChannelId,
                        picVisuals.Width,
                        picVisuals.Height,
                        _volumeColor1,
                        _volumeColor2,
                        Color.Transparent,
                        10,
                        1,
                        false,
                        true,
                        false);
                }
            }
            else
            {
                _visualWarmupTicks = 0;
            }
        }

        /// <summary>
        ///     Handles the Click event of the btnPlay control.
        /// </summary>
        private void btnPlay_Click(object sender, EventArgs e)
        {
            BeginInvoke(BassPlayer.PlayState == PlayState.Playing
                ? delegate { BassPlayer.Pause(); }
                : new MethodInvoker(delegate { BassPlayer.Play(); }));

            btnPause.Visible = BassPlayer.PlayState == PlayState.Playing;
            btnPlay.Visible = BassPlayer.PlayState != PlayState.Playing;
        }

        /// <summary>
        ///     Handles the Click event of the btnPrevious control.
        /// </summary>
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate { PlaylistControl.PlayPreviousTrack(); }));
        }

        /// <summary>
        ///     Handles the Click event of the btnSkipToEnd control.
        /// </summary>
        private void btnSkipToEnd_Click(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate { BassPlayer.SkipToFadeOut(); }));
        }

        /// <summary>
        ///     Handles the Click event of the btnNext control.
        /// </summary>
        private void btnNext_Click(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate { BassPlayer.SkipToEnd(); }));
        }

        /// <summary>
        ///     Handles the GlobalPaletteChanged event of the KryptonManager control.
        /// </summary>
        private void KryptonManager_GlobalPaletteChanged(object sender, EventArgs e)
        {
            SetThemeState();
            UpdateRankHighlight();
        }

        /// <summary>
        ///     Handles the OnTrackChange event of the BassPlayer control.
        /// </summary>
        private void BassPlayer_OnTrackChange(object sender, EventArgs e)
        {
            if (InvokeRequired)
                BeginInvoke(new MethodInvoker(BassPlayer_OnTrackChange));
            else BassPlayer_OnTrackChange();
        }

        /// <summary>
        ///     Handles the OnTrackChange event of the BassPlayer control.
        /// </summary>
        private void BassPlayer_OnTrackChange()
        {
            if (_bassPlayerOnTrackChange) return;
            _bassPlayerOnTrackChange = true;

            DisplayCurrentTrackDetails();
            UpdateTrackInfoBar();
            UpdateRankHighlight();

            _bassPlayerOnTrackChange = false;
        }


        /// <summary>
        ///     Handles the PlaylistChanged event of the PlaylistControl control.
        /// </summary>
        private void PlaylistControl_PlaylistChanged(object sender, EventArgs e)
        {
            DisplayCurrentTrackDetails();
            UpdateTrackInfoBar();
            UpdateRankHighlight();
        }

        /// <summary>
        ///     Handles the Slid event of the sldVolume control.
        /// </summary>
        private void sldVolume_Slid(object sender, EventArgs e)
        {
            _bindingVolumeSlider = true;
            var volume = Convert.ToDecimal(sldVolume.ScrollValue);
            BassPlayer.SetMixerVolume(volume);
            lblVolume.Text = volume.ToString(CultureInfo.InvariantCulture);
            _bindingVolumeSlider = false;
        }

        private void SetVolume(int volume)
        {
            sldVolume.Value = volume;
            lblVolume.Text = volume.ToString();
        }

        private void BassPlayer_OnVolumeChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
                BeginInvoke(new MethodInvoker(delegate { BindVolume(); }));
            else
                BindVolume();
        }

        private void BindVolume()
        {
            if (_bindingVolumeSlider) return;
            var volume = (int)BassPlayer.GetMixerVolume();
            if (volume != sldVolume.Value)
                SetVolume(volume);
        }

        private void btnReplayMix_Click(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate { PlaylistControl.ReplayMix(); }));
        }

        private void RankButton_Click(int rank)
        {
            SetCurrentMixRank(rank);
            UpdateRankHighlight();
            PlaylistControl?.MixRankAssigned?.Invoke(PlaylistControl, EventArgs.Empty);
        }

        private void UpdateRankHighlight()
        {
            var rank = GetCurrentMixRank();

            var rankButtons = new[]
            {
                (Rank: 5, Button: btnRankExcellent),
                (Rank: 4, Button: btnRankVeryGood),
                (Rank: 3, Button: btnRankGood),
                (Rank: 2, Button: btnRankBearable),
                (Rank: 0, Button: btnRankForbidden),
            };

            foreach (var entry in rankButtons)
            {
                var isActive = entry.Rank == rank;
                entry.Button.StateNormal.Back.Color1 = isActive ? _volumeColor1 : Color.Empty;
                entry.Button.StateNormal.Back.Color2 = isActive ? _volumeColor2 : Color.Empty;
            }
        }

        private void UpdateTrackInfoBar()
        {
            if (PlaylistControl == null) return;

            var prevTrack = PlaylistControl.GetPreviousTrack();
            if (prevTrack != null)
            {
                var info = prevTrack.Description;
                if (prevTrack.LengthFormatted != null) info += $"  {prevTrack.LengthFormatted}";
                if (prevTrack.Bpm != 0) info += $"  {prevTrack.Bpm:0.00} BPM";
                if (!string.IsNullOrEmpty(prevTrack.Key)) info += $"  {KeyHelper.GetDisplayKey(prevTrack.Key)}";
                lblPrevTrack.Text = "Prev: " + info;
            }
            else
            {
                lblPrevTrack.Text = "Previous Track:";
            }

            var nextTrack = PlaylistControl.GetNextTrack();
            if (nextTrack != null)
            {
                var info = nextTrack.Description;
                if (nextTrack.LengthFormatted != null) info += $"  {nextTrack.LengthFormatted}";
                if (nextTrack.Bpm != 0) info += $"  {nextTrack.Bpm:0.00} BPM";
                if (!string.IsNullOrEmpty(nextTrack.Key)) info += $"  {KeyHelper.GetDisplayKey(nextTrack.Key)}";
                lblNextTrack.Text = "Next: " + info;
            }
            else
            {
                lblNextTrack.Text = "Next Track:";
            }
        }

    }
}
