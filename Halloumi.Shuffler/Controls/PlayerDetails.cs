using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Halloumi.BassEngine.Helpers;
using Halloumi.BassEngine.Models;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.Engine;
using Halloumi.Shuffler.Engine.Models;
using Halloumi.Shuffler.Forms;
using Un4seen.Bass;
using Un4seen.Bass.Misc;
using BE = Halloumi.BassEngine;
using Track = Halloumi.Shuffler.Engine.Models.Track;

namespace Halloumi.Shuffler.Controls
{
    public partial class PlayerDetails : UserControl
    {
        private Visuals _bassVisuals = new Visuals();

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TrackDetails class.
        /// </summary>
        public PlayerDetails()
        {
            InitializeComponent();

            Timer = new BASSTimer(100);
            //this.Timer = new System.Windows.Forms.Timer();
            Timer.Interval = 100;

            Load += new EventHandler(TrackDetails_Load);
            KryptonManager.GlobalPaletteChanged += new EventHandler(KryptonManager_GlobalPaletteChanged);

            slider.Slid += new EventHandler(Slider_Slid);
            btnPlay.Click += new EventHandler(btnPlay_Click);
            btnPause.Click += new EventHandler(btnPlay_Click); Timer.Tick += new EventHandler(Timer_Tick);

            btnPrevious.Click += new EventHandler(btnPrevious_Click);
            btnSkipToEnd.Click += new EventHandler(btnSkipToEnd_Click);
            btnNext.Click += new EventHandler(btnNext_Click);
            sldVolume.Scrolled += new MediaSlider.MediaSlider.ScrollDelegate(sldVolume_Slid);
            VisualsShown = false;
            AlbumArtShown = true;

            slider.Minimum = 0;
            slider.Maximum = 0;

            SetThemeState();

            _bassVisuals.MaxFFT = BASSData.BASS_DATA_FFT1024;
            _bassVisuals.MaxFrequencySpectrum = Utils.FFTFrequency2Index(16000, 1024, 44100);
        }

        public void Initialize()
        {
            var settings = Settings.Default;
            BassPlayer.SetMixerVolume(settings.Volume);
            sldVolume.Value = (int)settings.Volume;
            lblVolume.Text = settings.Volume.ToString();
        }

        private Track GetCurrentTrack()
        {
            if (BassPlayer.CurrentTrack != null)
            {
                return Library.GetTrackByFilename(BassPlayer.CurrentTrack.Filename);
            }
            return null;
        }

        private Track GetPreviousTrack()
        {
            return PlaylistControl.GetPreviousTrack();
        }

        /// <summary>
        /// Handles the Load event of the TrackDetails control.
        /// </summary>
        private void TrackDetails_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            if (_loaded) return;

            DisplayCurrentTrackDetails();

            slider.Dock = DockStyle.None;
            slider.Width = pnlSlider.Width;
            slider.Dock = DockStyle.Fill;

            if (!Timer.Enabled)
            {
                Timer.Start();
                BassPlayer.OnTrackChange += new EventHandler(BassPlayer_OnTrackChange);
            }

            sldVolume.Minimum = 0;
            sldVolume.Maximum = 100;
            var volume = Convert.ToInt32(BassPlayer.GetMixerVolume());
            lblVolume.Text = volume.ToString();
            sldVolume.Value = volume;

            _loaded = true;
        }

        private bool _loaded = false;

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets the state of the control based on the current theme.
        /// </summary>
        private void SetThemeState()
        {
            if (DesignMode) return;

            var palette = KryptonHelper.GetCurrentPalette();
            var color = KryptonManager.GetPaletteForMode(palette).GetBackColor2(PaletteBackStyle.PanelAlternate, PaletteState.Normal);

            BackColor = color;
            pnlSlider.BackColor = color;
            pnlVolume.BackColor = color;
            slider.BackColor = color;
            sldVolume.BackColor = color;

            _volumeColor1 = KryptonManager.GetPaletteForMode(palette).GetBackColor1(PaletteBackStyle.ButtonStandalone, PaletteState.Pressed);
            _volumeColor2 = KryptonManager.GetPaletteForMode(palette).GetBackColor2(PaletteBackStyle.ButtonStandalone, PaletteState.Pressed);
        }

        private Color _volumeColor1 = Color.Wheat;
        private Color _volumeColor2 = Color.Gold;

        /// <summary>
        /// Displays the current track details.
        /// </summary>
        public void DisplayCurrentTrackDetails()
        {
            Track track = null;
            if (BassPlayer.CurrentTrack != null)
            {
                track = Library.GetTrackByFilename(BassPlayer.CurrentTrack.Filename);
            }

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

                var details = track.Album + " - " + track.Genre + " ";
                details += " - " + track.LengthFormatted;
                if (track.Bpm != 0) details += " - " + track.Bpm.ToString("0.00") + " BPM";
                if (track.Key != "") details += " - " + KeyHelper.GetDisplayKey(track.Key);

                lblCurrentTrackDetails.Text = details;

                picCover.Image = Library.GetAlbumCover(new Album(track.Album));
                CurrentTrackDescription = track.Description;
            }
            else
            {
                lblCurrentTrackDescription.Text = "";
                lblCurrentTrackDetails.Text = "";
                picCover.Image = null;
                CurrentTrackDescription = "";
            }
        }

        public int GetCurrentMixRank()
        {
            if (BassPlayer.CurrentTrack == null) return 1;

            Track currentTrack = null;
            if (BassPlayer.CurrentTrack != null)
            {
                currentTrack = Library.GetTrackByFilename(BassPlayer.CurrentTrack.Filename);
            }

            var prevTrack = PlaylistControl.GetPreviousTrack();
            if (prevTrack == null) return 1;

            return MixLibrary.GetMixLevel(prevTrack, currentTrack);
        }

        public void SetCurrentMixRank(int mixRank)
        {
            if (BassPlayer.CurrentTrack == null) return;

            Track currentTrack = null;
            if (BassPlayer.CurrentTrack != null)
            {
                currentTrack = Library.GetTrackByFilename(BassPlayer.CurrentTrack.Filename);
            }

            var prevTrack = PlaylistControl.GetPreviousTrack();
            if (prevTrack == null) return;

            MixLibrary.SetMixLevel(prevTrack, currentTrack, mixRank);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the Slid event of the Slider control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Slider_Slid(object sender, EventArgs e)
        {
            Timer.Stop();
            var position = slider.Value;
            BassPlayer.SetAdjustedTrackPosition(position);
            Thread.Sleep(200);
            Timer.Start();
        }

        /// <summary>
        /// Handles the Tick event of the Timer control.
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                BeginInvoke(new MethodInvoker(delegate() { Timer_Tick(); }));
            }
            catch
            { }
        }

        public bool VisualsShown { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether album art is shown.
        /// </summary>
        public bool AlbumArtShown
        {
            get { return picCover.Visible; }
            set { picCover.Visible = value; }
        }

        /// <summary>
        /// Handles the Tick event of the Timer control.
        /// </summary>
        private void Timer_Tick()
        {
            if (FindForm() != null && FindForm().WindowState == FormWindowState.Minimized) return;

            if (_timerTick) return;
            _timerTick = true;

            var position = BassPlayer.GetTrackPosition();

            if (slider.Maximum != position.Length) slider.Maximum = (int)position.Length;
            if (slider.Value != position.Positition) slider.Value = (int)position.Positition;

            if (lblTimeElapsed.Text != position.ElapsedFormatted) lblTimeElapsed.Text = position.ElapsedFormatted;
            if (lblTimeRemaining.Text != position.RemainingFormatted) lblTimeRemaining.Text = position.RemainingFormatted;

            btnPause.Visible = (BassPlayer.PlayState == PlayState.Playing);
            btnPlay.Visible = (BassPlayer.PlayState != PlayState.Playing);

            ShowVisuals();

            _timerTick = false;
        }

        private bool _timerTick = false;
        private bool _firstVisualShown = false;

        private void ShowVisuals()
        {
            picVisuals.Visible = (VisualsShown && BassPlayer.PlayState == PlayState.Playing);
            if (!VisualsShown) return;
            if (BassPlayer.PlayState == PlayState.Playing)
            {
                if (!_firstVisualShown)
                {
                    for (var i = 0; i < 5; i++)
                    {
                        Thread.Sleep(50);
                        Application.DoEvents();
                    }
                }

                lock (BassPlayer.MixerLock)
                {
                    picVisuals.Image = _bassVisuals.CreateSpectrumLine(BassPlayer.MixerChanel,
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
                    _firstVisualShown = true;
                }
            }
            else
            {
                _firstVisualShown = false;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnPlay control.
        /// </summary>
        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (BassPlayer.PlayState == PlayState.Playing)
                BeginInvoke(new MethodInvoker(delegate() { this.BassPlayer.Pause(); }));
            else
                BeginInvoke(new MethodInvoker(delegate() { this.BassPlayer.Play(); }));

            btnPause.Visible = (BassPlayer.PlayState == PlayState.Playing);
            btnPlay.Visible = (BassPlayer.PlayState != PlayState.Playing);
        }

        /// <summary>
        /// Handles the Click event of the btnPrevious control.
        /// </summary>
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate() { this.PlaylistControl.PlayPreviousTrack(); }));
        }

        /// <summary>
        /// Handles the Click event of the btnSkipToEnd control.
        /// </summary>
        private void btnSkipToEnd_Click(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate() { this.BassPlayer.SkipToEnd(); }));
        }

        /// <summary>
        /// Handles the Click event of the btnNext control.
        /// </summary>
        private void btnNext_Click(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate() { this.PlaylistControl.PlayNextTrack(); }));
        }

        /// <summary>
        /// Handles the GlobalPaletteChanged event of the KryptonManager control.
        /// </summary>
        private void KryptonManager_GlobalPaletteChanged(object sender, EventArgs e)
        {
            SetThemeState();
        }

        /// <summary>
        /// Handles the OnTrackChange event of the BassPlayer control.
        /// </summary>
        private void BassPlayer_OnTrackChange(object sender, EventArgs e)
        {
            //if (!this.IsHandleCreated) return;
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate()
                {
                    BassPlayer_OnTrackChange();
                }));
            }
            else BassPlayer_OnTrackChange();
        }

        /// <summary>
        /// Handles the OnTrackChange event of the BassPlayer control.
        /// </summary>
        private void BassPlayer_OnTrackChange()
        {
            if (_bassPlayerOnTrackChange) return;
            _bassPlayerOnTrackChange = true;

            DisplayCurrentTrackDetails();

            _bassPlayerOnTrackChange = false;
        }

        private bool _bassPlayerOnTrackChange = false;

        /// <summary>
        /// Handles the Shown event of the TrackDetails control.
        /// </summary>
        private void TrackDetails_Shown(object sender, EventArgs e)
        {
            //slider.Invalidate();
            // sldVolume.Invalidate();
        }

        /// <summary>
        /// Handles the PlaylistChanged event of the PlaylistControl control.
        /// </summary>
        private void PlaylistControl_PlaylistChanged(object sender, EventArgs e)
        {
            DisplayCurrentTrackDetails();
        }

        /// <summary>
        /// Handles the Slid event of the sldVolume control.
        /// </summary>
        private void sldVolume_Slid(object sender, EventArgs e)
        {
            var volume = Convert.ToDecimal(sldVolume.ScrollValue);
            BassPlayer.SetMixerVolume(volume);
            lblVolume.Text = volume.ToString();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current track description.
        /// </summary>
        private string CurrentTrackDescription { get; set; }

        /// <summary>
        /// Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Library Library { get; set; }

        /// <summary>
        /// Gets or sets the bass player.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BE.BassPlayer BassPlayer { get; set; }

        /// <summary>
        /// Gets or sets the mix library
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MixLibrary MixLibrary { get; set; }

        /// <summary>
        /// Gets or sets the timer.
        /// </summary>
        //private System.Windows.Forms.Timer Timer { get; set; }

        private BASSTimer Timer { get; set; }

        /// <summary>
        /// Gets or sets the playlist control.
        /// </summary>
        public PlaylistControl PlaylistControl
        {
            get
            {
                return _playlistControl;
            }
            set
            {
                if (_playlistControl != null) _playlistControl.PlaylistChanged -= new EventHandler(PlaylistControl_PlaylistChanged);
                _playlistControl = value;
                if (_playlistControl != null) _playlistControl.PlaylistChanged += new EventHandler(PlaylistControl_PlaylistChanged);
            }
        }

        private PlaylistControl _playlistControl = null;

        #endregion

        private void btnReplayMix_Click(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate() { this.PlaylistControl.ReplayMix(); }));
        }

        public event EventHandler SelectedViewChanged;

        private void tabButtons_CheckedButtonChanged(object sender, EventArgs e)
        {
            if (SelectedViewChanged != null)
                SelectedViewChanged(this, e);
        }

        public SelectedView GetSelectedView()
        {
            return (SelectedView)tabButtons.CheckedIndex;
        }

        public void SetSelectedView(SelectedView selectedView)
        {
            var index = (int)selectedView;
            if (index != tabButtons.CheckedIndex)
                tabButtons.CheckedIndex = (int)selectedView;
        }

        public enum SelectedView
        {
            Playlist = 0,
            Library = 1,
            Mixer = 2
        }
    }
}