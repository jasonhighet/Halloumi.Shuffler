using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Halloumi.BassEngine;
using Halloumi.Common.Windows.Forms;
using Halloumi.Common.Windows.Helpers;

using BE = Halloumi.BassEngine;

namespace Halloumi.Shuffler.Forms
{
    public partial class frmShufflerDetails : BaseForm
    {
        #region Constructors

        public BE.BassPlayer BassPlayer { get; set; }

        public string Filename { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="frmTrack"/> class.
        /// </summary>
        public frmShufflerDetails()
        {
            InitializeComponent();

            cmbCustomFadeInLength.TextChanged += new EventHandler(cmbCustomFadeInLength_TextChanged);
            cmbCustomFadeOutLength.TextChanged += new EventHandler(cmbCustomFadeOutLength_TextChanged);
            txtFadeInPosition.TextChanged += new EventHandler(txtFadeInPosition_TextChanged);
            txtFadeOutStartPosition.TextChanged += new EventHandler(txtFadeOutStartPosition_TextChanged);
            trackWave.PositionsChanged += new EventHandler(trackWave_PositionsChanged);
        }

        private void Initialise()
        {
            trackWave.BassPlayer = this.BassPlayer;
            this.Track = trackWave.LoadTrack(this.Filename);
            cmbOutput.SelectedIndex = 0;

            this.CurrentSamples = new List<TrackSample>();
            foreach (var trackSample in this.AutomationAttributes.TrackSamples)
            {
                this.CurrentSamples.Add(trackSample.Clone());
            }
            this.trackWave.TrackSamples = this.CurrentSamples;

            SetControlStates();
            BindData();
            SetBPMValues();
            PopulateTrackFXComboBox();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public void LoadSettings()
        {
            try
            {
                var settings = Halloumi.Shuffler.Forms.Settings.Default;
                this.BassPlayer.RawLoopOutput = settings.RawLoopOutput;
                if (settings.RawLoopOutput == BE.Channels.SoundOutput.Speakers) cmbOutput.SelectedIndex = 0;
                if (settings.RawLoopOutput == BE.Channels.SoundOutput.Monitor) cmbOutput.SelectedIndex = 1;
                if (settings.RawLoopOutput == BE.Channels.SoundOutput.Both) cmbOutput.SelectedIndex = 2;
            }
            catch
            { }
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        {
            _bindingData = true;

            List<double> loopLengths = BE.BassHelper.GetLoopLengths(this.Track.StartBPM);
            cmbCustomFadeInLength.PopulateItemsFromSecondsList(loopLengths);

            loopLengths = BE.BassHelper.GetLoopLengths(this.Track.EndBPM);
            cmbCustomFadeOutLength.PopulateItemsFromSecondsList(loopLengths);

            lblStartBPM.Text = this.Track.StartBPM.ToString("0.00");
            lblEndBPM.Text = this.Track.EndBPM.ToString("0.00");

            this.Text = "Halloumi : Shuffler : Shuffer Details : " + this.Track.Description;

            chkUsePreFadeIn.Checked = this.Track.UsePreFadeIn;

            chkUseSkipSection.Checked = this.Track.HasSkipSection;
            txtSkipStart.Seconds = this.Track.SamplesToSeconds(this.Track.SkipStart);
            cmbSkipLength.Seconds = this.Track.SkipLengthSeconds;
            cmbSkipLength.PopulateItemsFromSecondsList(loopLengths);

            txtFadeInPosition.Seconds = this.Track.SamplesToSeconds(this.Track.FadeInStart);
            txtFadeOutStartPosition.Seconds = this.Track.SamplesToSeconds(this.Track.FadeOutStart);
            txtPreFadeInStartPosition.Seconds = this.Track.SamplesToSeconds(this.Track.PreFadeInStart);

            if (this.Track.FadeInEnd != 0D) cmbCustomFadeInLength.Seconds = this.Track.SamplesToSeconds(this.Track.FadeInEnd - this.Track.FadeInStart);
            if (this.Track.FadeOutEnd != 0D) cmbCustomFadeOutLength.Seconds = this.Track.SamplesToSeconds(this.Track.FadeOutEnd - this.Track.FadeOutStart);

            cmbPreFadeInStartVolume.Text = (this.Track.PreFadeInStartVolume * 100).ToString();

            if (!this.Track.IsLoopedAtStart)
            {
                cmbFadeInLoopCount.Text = "0";
            }
            else
            {
                cmbFadeInLoopCount.Text = this.Track.StartLoopCount.ToString();
            }

            if (!this.Track.IsLoopedAtEnd)
            {
                cmbFadeOutLoopCount.Text = "0";
            }
            else
            {
                cmbFadeOutLoopCount.Text = this.Track.EndLoopCount.ToString();
            }

            chkPowerDown.Checked = this.Track.PowerDownOnEnd;

            BindSamples();
            BindSample();

            _bindingData = false;
        }

        private bool _bindingData = false;

        /// <summary>
        /// Populates the volume drop down.
        /// </summary>
        /// <param name="comboBox">The combo box.</param>
        private void PopulateVolumeDropDown(Halloumi.Common.Windows.Controls.ComboBox comboBox)
        {
            comboBox.Items.Clear();
            comboBox.Items.Add("0");
            comboBox.Items.Add("5");
            comboBox.Items.Add("10");
            comboBox.Items.Add("20");
            comboBox.Items.Add("25");
            comboBox.Items.Add("30");
            comboBox.Items.Add("40");
            comboBox.Items.Add("50");
            comboBox.Items.Add("60");
            comboBox.Items.Add("70");
            comboBox.Items.Add("75");
            comboBox.Items.Add("80");
            comboBox.Items.Add("90");
            comboBox.Items.Add("95");
            comboBox.Items.Add("100");
        }

        /// <summary>
        /// Populates the volume drop down.
        /// </summary>
        /// <param name="comboBox">The combo box.</param>
        private void PopulateVolumeDropDownReverse(Halloumi.Common.Windows.Controls.ComboBox comboBox)
        {
            comboBox.Items.Clear();
            comboBox.Items.Add("100");
            comboBox.Items.Add("95");
            comboBox.Items.Add("90");
            comboBox.Items.Add("80");
            comboBox.Items.Add("75");
            comboBox.Items.Add("70");
            comboBox.Items.Add("60");
            comboBox.Items.Add("50");
            comboBox.Items.Add("40");
            comboBox.Items.Add("30");
            comboBox.Items.Add("25");
            comboBox.Items.Add("20");
            comboBox.Items.Add("10");
            comboBox.Items.Add("5");
            comboBox.Items.Add("0");
        }

        /// <summary>
        /// Sets the control states.
        /// </summary>
        private void SetControlStates()
        {
            txtPreFadeInStartPosition.Enabled = chkUsePreFadeIn.Checked;
            cmbPreFadeInStartVolume.Enabled = chkUsePreFadeIn.Checked;

            txtSkipStart.Enabled = chkUseSkipSection.Checked;
            cmbSkipLength.Enabled = chkUseSkipSection.Checked;
        }

        /// <summary>
        /// Sets the BPM values.
        /// </summary>
        private void SetBPMValues()
        {
            if (_bindingData) return;

            decimal startBPM = this.Track.StartBPM;
            decimal endBPM = this.Track.EndBPM;

            var fadeInLength = BE.BassHelper.GetDefaultLoopLength(this.Track.TagBPM);
            if (cmbCustomFadeInLength.Seconds != 0)
            {
                fadeInLength = cmbCustomFadeInLength.Seconds;
            }
            startBPM = BE.BassHelper.GetBPMFromLoopLength(fadeInLength);
            lblStartBPM.Text = startBPM.ToString("0.00");

            var fadeOutLength = BE.BassHelper.GetDefaultLoopLength(this.Track.TagBPM);
            if (cmbCustomFadeOutLength.Seconds != 0)
            {
                fadeOutLength = cmbCustomFadeOutLength.Seconds;
            }
            endBPM = BE.BassHelper.GetBPMFromLoopLength(fadeOutLength);
            lblEndBPM.Text = endBPM.ToString("0.00");

            PopulateVolumeDropDownReverse(cmbPreFadeInStartVolume);
        }

        /// <summary>
        /// Saves the track.
        /// </summary>
        private void SaveTrack()
        {
            UpdateData();
            UpdateCurrentSample();

            this.AutomationAttributes.TrackSamples.Clear();
            foreach (var sample in this.CurrentSamples)
            {
                if (sample.Length == 0 && sample.Start == 0) continue;
                this.AutomationAttributes.TrackSamples.Add(sample);
            }

            this.BassPlayer.SaveExtendedAttributes(this.Track);
            this.BassPlayer.ReloadTrack(this.Track.Filename);
            this.BassPlayer.SaveAutomationAttributes(this.Track);
            this.BassPlayer.ReloadTrack(this.Track.Filename);

            _saved = true;

            this.Close();
        }

        private bool _saved = false;

        /// <summary>
        /// Updates the track from the controls
        /// </summary>
        private void UpdateData()
        {
            if (_bindingData) return;

            this.Track.ChangeTempoOnFadeOut = true;

            this.Track.FadeInStart = this.Track.SecondsToSamples(txtFadeInPosition.Seconds);
            this.Track.FadeInEnd = this.Track.FadeInStart + this.Track.SecondsToSamples(cmbCustomFadeInLength.Seconds);

            this.Track.FadeOutStart = this.Track.SecondsToSamples(txtFadeOutStartPosition.Seconds);
            this.Track.FadeOutEnd = this.Track.FadeOutStart + this.Track.SecondsToSamples(cmbCustomFadeOutLength.Seconds);

            this.Track.UsePreFadeIn = chkUsePreFadeIn.Checked;
            if (chkUsePreFadeIn.Checked)
            {
                this.Track.PreFadeInStart = this.Track.SecondsToSamples(txtPreFadeInStartPosition.Seconds);
                this.Track.PreFadeInStartVolume = float.Parse(cmbPreFadeInStartVolume.Text) / 100F;
            }
            else
            {
                this.Track.PreFadeInStart = 0;
                this.Track.PreFadeInStartVolume = 0F;
            }

            this.Track.PowerDownOnEnd = chkPowerDown.Checked;

            if (cmbFadeInLoopCount.Text == "") cmbFadeInLoopCount.Text = "0";
            if (cmbFadeOutLoopCount.Text == "") cmbFadeOutLoopCount.Text = "0";

            this.Track.StartLoopCount = int.Parse(cmbFadeInLoopCount.Text);
            this.Track.EndLoopCount = int.Parse(cmbFadeOutLoopCount.Text);

            this.Track.SkipStart = this.Track.SecondsToSamples(txtSkipStart.Seconds);
            if (this.Track.SkipStart != 0)
                this.Track.SkipEnd = this.Track.SkipStart + this.Track.SecondsToSamples(cmbSkipLength.Seconds);
            else
                this.Track.SkipEnd = 0;

            trackWave.RefreshPositions();

            SetBPMValues();
            SetControlStates();
        }

        /// <summary>
        /// Populates the track FX combo box.
        /// </summary>
        private void PopulateTrackFXComboBox()
        {
            cmbTrackFX.Items.Clear();

            foreach (var trigger in this.AutomationAttributes.TrackFXTriggers.OrderBy(t => t.Start).ToList())
            {
                cmbTrackFX.Items.Add(BassHelper.GetFormattedSecondsNoHours(trigger.Start));
            }

            if (cmbTrackFX.Items.Count > 0) cmbTrackFX.SelectedIndex = 0;
        }

        private AutomationAttributes AutomationAttributes
        {
            get { return this.BassPlayer.GetAutomationAttributes(this.Track); }
        }

        /// <summary>
        /// Gets the selected track FX.
        /// </summary>
        /// <returns>The selected track FX.</returns>
        private TrackFXTrigger GetSelectedTrackFX()
        {
            if (cmbTrackFX.Items.Count == 0 || cmbTrackFX.SelectedIndex < 0) return null;

            var selectedText = cmbTrackFX.SelectedItem.ToString();

            foreach (var trackFX in this.AutomationAttributes.TrackFXTriggers.OrderBy(t => t.Start).ToList())
            {
                if (selectedText == BassHelper.GetFormattedSecondsNoHours(trackFX.Start))
                    return trackFX;
            }

            return null;
        }

        private TrackSample CurrentSample
        {
            get;
            set;
        }

        private List<TrackSample> CurrentSamples
        {
            get;
            set;
        }

        private void BindSamples()
        {
            lstSamples.SuspendLayout();
            lstSamples.Items.Clear();
            foreach (var trackSample in this.CurrentSamples)
            {
                var item = new ListViewItem(trackSample.Description);
                lstSamples.Items.Add(item);
                item.Selected = (trackSample == this.CurrentSample);
            }
            lstSamples.ResumeLayout();
            trackWave.RefreshPositions();
        }

        private void BindSample()
        {
            List<double> loopLengths = null;
            if (this.CurrentSample == null)
            {
                txtSampleStartPosition.Seconds = 0;
                loopLengths = BE.BassHelper.GetLoopLengths(this.Track.BPM);
                cmbSampleLength.Seconds = 0;
                chkLoopSample.Checked = false;
            }
            else
            {
                txtSampleStartPosition.Seconds = this.CurrentSample.Start;
                loopLengths = BE.BassHelper.GetLoopLengths(this.CurrentSample.CalculateBPM(this.Track));
                cmbSampleLength.Seconds = this.CurrentSample.Length;
                chkLoopSample.Checked = this.CurrentSample.IsLooped;
            }
            cmbSampleLength.PopulateItemsFromSecondsList(loopLengths);
        }

        private void AddSample()
        {
            var sampleName = UserInputHelper.GetUserInput("Sample Name", "", this);
            if (sampleName != "")
            {
                var sampleKey = "";
                for (int i = 0; i < 2000; i++)
                {
                    sampleKey = "Sample" + (i + 1).ToString();
                    if (!this.CurrentSamples.Exists(s => s.Key == sampleKey))
                    {
                        break;
                    }
                }

                var trackSample = new TrackSample()
                {
                    Description = sampleName,
                    Key = sampleKey
                };
                this.CurrentSamples.Add(trackSample);
                this.CurrentSample = trackSample;

                BindSamples();
                BindSample();
            }
        }

        private void RemoveSample()
        {
            if (this.CurrentSample == null) return;

            var message = "Are you sure you wish to delete sample '" + this.CurrentSample.Description + "'?";
            if (MessageBoxHelper.Confirm(message))
            {
                this.CurrentSamples.Remove(this.CurrentSample);
                this.CurrentSample = null;
                BindSamples();
                BindSample();
            }
        }

        private void RenameSample()
        {
            if (this.CurrentSample == null) return;
            var sampleName = UserInputHelper.GetUserInput("Rename Sample", this.CurrentSample.Description, this);
            if (sampleName != this.CurrentSample.Description)
            {
                this.CurrentSample.Description = sampleName;
                BindSamples();
                BindSample();
            }
        }

        private void UpdateCurrentSample()
        {
            if (this.CurrentSample == null) return;

            this.CurrentSample.IsLooped = chkLoopSample.Checked;
            if (txtSampleStartPosition.Seconds != 0) this.CurrentSample.Start = txtSampleStartPosition.Seconds;
            if (cmbSampleLength.Seconds != 0) this.CurrentSample.Length = cmbSampleLength.Seconds;
        }

        #endregion

        #region Properties

        public BE.Track Track { get; private set; }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the Click event of the btnOK control.
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveTrack();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkUsePreFadeIn control.
        /// </summary>
        private void chkUsePreFadeIn_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUsePreFadeIn.Checked)
            {
                if (cmbPreFadeInStartVolume.Text == "0")
                    cmbPreFadeInStartVolume.Text = "75";
            }
            else
            {
                cmbPreFadeInStartVolume.Text = "0";
            }

            SetControlStates();
            UpdateData();
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cmbFadeInLoopCount control.
        /// </summary>
        private void cmbFadeInLoopCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cmbFadeOutLoopCount control.
        /// </summary>
        private void cmbFadeOutLoopCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cmbCustomFadeInLength control.
        /// </summary>
        private void cmbCustomFadeInLength_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cmbCustomFadeOutLength control.
        /// </summary>
        private void cmbCustomFadeOutLength_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        /// <summary>
        /// Handles the FormClosed event of the frmTrack control.
        /// </summary>
        private void frmTrack_FormClosed(object sender, FormClosedEventArgs e)
        {
            trackWave.Unload();
            if (_saved)
            {
                this.BassPlayer.ReloadAutomationAttributes(this.Track);
                this.BassPlayer.ReloadTrack(this.Track.Filename);
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the txtFadeOutStartPosition control.
        /// </summary>
        private void txtFadeOutStartPosition_TextChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        /// <summary>
        /// Handles the TextChanged event of the txtFadeInPosition control.
        /// </summary>
        private void txtFadeInPosition_TextChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        /// <summary>
        /// Handles the TextChanged event of the cmbCustomFadeOutLength control.
        /// </summary>
        private void cmbCustomFadeOutLength_TextChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        /// <summary>
        /// Handles the TextChanged event of the cmbCustomFadeInLength control.
        /// </summary>
        private void cmbCustomFadeInLength_TextChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        /// <summary>
        /// Handles the TextChanged event of the trackWave control.
        /// </summary>
        private void trackWave_PositionsChanged(object sender, EventArgs e)
        {
            _bindingData = true;

            chkUsePreFadeIn.Checked = this.Track.UsePreFadeIn;
            txtFadeInPosition.Seconds = this.Track.SamplesToSeconds(this.Track.FadeInStart);
            txtFadeOutStartPosition.Seconds = this.Track.SamplesToSeconds(this.Track.FadeOutStart);
            txtPreFadeInStartPosition.Seconds = this.Track.SamplesToSeconds(this.Track.PreFadeInStart);
            if (this.Track.FadeInEnd != 0D) cmbCustomFadeInLength.Seconds = this.Track.SamplesToSeconds(this.Track.FadeInEnd - this.Track.FadeInStart);
            if (this.Track.FadeOutEnd != 0D) cmbCustomFadeOutLength.Seconds = this.Track.SamplesToSeconds(this.Track.FadeOutEnd - this.Track.FadeOutStart);

            chkUseSkipSection.Checked = this.Track.HasSkipSection;
            txtSkipStart.Seconds = this.Track.SamplesToSeconds(this.Track.SkipStart);
            if (this.Track.SkipEnd != 0D) cmbSkipLength.Seconds = this.Track.SamplesToSeconds(this.Track.SkipEnd - this.Track.SkipStart);

            _bindingData = false;
        }

        /// <summary>
        /// Handles the Click event of the btnFadeInUpdate control.
        /// </summary>
        private void btnFadeInUpdate_Click(object sender, EventArgs e)
        {
            txtFadeInPosition.Seconds = this.Track.SamplesToSeconds(this.trackWave.ZoomStart);
            cmbCustomFadeInLength.Seconds = this.Track.SamplesToSeconds(this.trackWave.ZoomLength);
            UpdateData();
        }

        /// <summary>
        /// Handles the Click event of the btnFadeOutUpdate control.
        /// </summary>
        private void btnFadeOutUpdate_Click(object sender, EventArgs e)
        {
            txtFadeOutStartPosition.Seconds = this.Track.SamplesToSeconds(this.trackWave.ZoomStart);
            cmbCustomFadeOutLength.Seconds = this.Track.SamplesToSeconds(this.trackWave.ZoomLength);
            UpdateData();
        }

        private void btnSampleUpdate_Click(object sender, EventArgs e)
        {
            txtSampleStartPosition.Seconds = this.Track.SamplesToSeconds(this.trackWave.ZoomStart);
            cmbSampleLength.Seconds = this.Track.SamplesToSeconds(this.trackWave.ZoomLength);
            UpdateCurrentSample();
        }

        private void btnZoomSample_Click(object sender, EventArgs e)
        {
            ZoomToSample();
        }

        private void ZoomToSample()
        {
            if (this.CurrentSample == null) return;
            trackWave.Zoom(this.CurrentSample.Start, this.CurrentSample.Length);
        }

        private void cmbSampleLength_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void txtSampleStartPosition_TextChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void cmbSampleLength_TextChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void chkLoopSample_CheckedChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        /// <summary>
        /// Handles the Load event of the frmShufflerDetails control.
        /// </summary>
        private void frmShufflerDetails_Load(object sender, EventArgs e)
        {
            Initialise();
        }

        /// <summary>
        /// Handles the Click event of the btnZoomFadeIn control.
        /// </summary>
        private void btnZoomFadeIn_Click(object sender, EventArgs e)
        {
            trackWave.Zoom(this.Track.FadeInStart, this.Track.FadeInEnd);
        }

        /// <summary>
        /// Handles the Click event of the btnZoomPreFade control.
        /// </summary>
        private void btnZoomPreFade_Click(object sender, EventArgs e)
        {
            trackWave.Zoom(this.Track.PreFadeInStart, this.Track.FadeInStart);
        }

        /// <summary>
        /// Handles the Click event of the btnZoomFadeOut control.
        /// </summary>
        private void btnZoomFadeOut_Click(object sender, EventArgs e)
        {
            trackWave.Zoom(this.Track.FadeOutStart, this.Track.FadeOutEnd);
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cmbOutput control.
        /// </summary>
        private void cmbOutput_SelectedIndexChanged(object sender, EventArgs e)
        {
            var outputType = cmbOutput.ParseEnum<BE.Channels.SoundOutput>();
            this.BassPlayer.RawLoopOutput = outputType;
        }

        /// <summary>
        /// Handles the Click event of the btnPreFadeInUpdate control.
        /// </summary>
        private void btnPreFadeInUpdate_Click(object sender, EventArgs e)
        {
            if (this.trackWave.ZoomStart < this.Track.FadeInStart)
            {
                txtPreFadeInStartPosition.Seconds = this.Track.SamplesToSeconds(this.trackWave.ZoomStart);
                UpdateData();
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkShowTrackFX control.
        /// </summary>
        private void chkShowTrackFX_CheckedChanged(object sender, EventArgs e)
        {
            trackWave.ShowTrackFX = chkShowTrackFX.Checked;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cmbTrackFX control.
        /// </summary>
        private void cmbTrackFX_SelectedIndexChanged(object sender, EventArgs e)
        {
            var trackFX = GetSelectedTrackFX();
            if (trackFX == null) rdbDelay2.Checked = true;
            else if (trackFX.DelayNotes == 0.5M) rdbDelay1.Checked = true;
            else if (trackFX.DelayNotes == 0.25M) rdbDelay2.Checked = true;
            else if (trackFX.DelayNotes == 0.125M) rdbDelay3.Checked = true;
            else if (trackFX.DelayNotes == 0.0625M) rdbDelay4.Checked = true;
            else rdbDelay2.Checked = true;
        }

        #endregion

        private void btnDeleteTrackFX_Click(object sender, EventArgs e)
        {
            var trackFX = this.GetSelectedTrackFX();
            if (trackFX == null) return;

            this.AutomationAttributes.TrackFXTriggers.Remove(trackFX);

            PopulateTrackFXComboBox();
            trackWave.RefreshPositions();
        }

        private void btnUpdateTrackFX_Click(object sender, EventArgs e)
        {
            var trackFX = this.GetSelectedTrackFX();
            if (trackFX == null) return;

            if (rdbDelay1.Checked) trackFX.DelayNotes = 0.5M;
            if (rdbDelay2.Checked) trackFX.DelayNotes = 0.25M;
            if (rdbDelay3.Checked) trackFX.DelayNotes = 0.125M;
            if (rdbDelay4.Checked) trackFX.DelayNotes = 0.0625M;

            trackFX.Start = this.Track.SamplesToSeconds(trackWave.ZoomStart);
            trackFX.Length = this.Track.SamplesToSeconds(trackWave.ZoomLength);

            PopulateTrackFXComboBox();
            trackWave.RefreshPositions();
        }

        private void btnAddTrackFX_Click(object sender, EventArgs e)
        {
            var trackFX = new TrackFXTrigger();

            if (rdbDelay1.Checked) trackFX.DelayNotes = 0.5M;
            if (rdbDelay2.Checked) trackFX.DelayNotes = 0.25M;
            if (rdbDelay3.Checked) trackFX.DelayNotes = 0.125M;
            if (rdbDelay4.Checked) trackFX.DelayNotes = 0.0625M;

            trackFX.Start = this.Track.SamplesToSeconds(trackWave.ZoomStart);
            trackFX.Length = this.Track.SamplesToSeconds(trackWave.ZoomLength);

            this.AutomationAttributes.TrackFXTriggers.Add(trackFX);

            PopulateTrackFXComboBox();
            trackWave.RefreshPositions();
        }

        private void btnClearTrackFX_Click(object sender, EventArgs e)
        {
            if (!MessageBoxHelper.Confirm("Are you sure you wish to clear all Track FX triggers for " + this.Track.Description + "?")) return;

            this.AutomationAttributes.TrackFXTriggers.Clear();

            PopulateTrackFXComboBox();
            trackWave.RefreshPositions();
        }

        /// <summary>
        /// Handles the Click event of the btnTrackFXZoom control.
        /// </summary>
        private void btnTrackFXZoom_Click(object sender, EventArgs e)
        {
            var trackFX = this.GetSelectedTrackFX();
            if (trackFX == null) return;

            var start = this.Track.SecondsToSamples(trackFX.Start);
            var end = this.Track.SecondsToSamples(trackFX.Start + trackFX.Length);

            trackWave.Zoom(start, end);
        }

        private void chkUseSkipSection_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkUseSkipSection.Checked)
            {
                cmbSkipLength.Seconds = 0;
                txtSkipStart.Seconds = 0;
            }

            SetControlStates();
            UpdateData();
        }

        private void btnSkipUpdate_Click(object sender, EventArgs e)
        {
            txtSkipStart.Seconds = this.Track.SamplesToSeconds(this.trackWave.ZoomStart);
            cmbSkipLength.Seconds = this.Track.SamplesToSeconds(this.trackWave.ZoomLength);
            UpdateData();
        }

        private void btnSkipZoom_Click(object sender, EventArgs e)
        {
            trackWave.Zoom(this.Track.SkipStart, this.Track.SkipEnd);
        }

        private void cmbSkipLength_TextChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void cmbSkipLength_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void txtSkipStart_TextChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the lstSamples control.
        /// </summary>
        private void lstSamples_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCurrentSample();
            if (lstSamples.SelectedItems.Count == 0)
            {
                this.CurrentSample = null;
            }
            else
            {
                var description = lstSamples.SelectedItems[0].Text;
                this.CurrentSample = this.CurrentSamples.Where(s => s.Description == description).FirstOrDefault();
            }
            BindSample();
            ZoomToSample();
        }

        private void btnAddSample_Click(object sender, EventArgs e)
        {
            AddSample();
        }

        private void btnRemoveSample_Click(object sender, EventArgs e)
        {
            RemoveSample();
        }

        private void btnRenameSample_Click(object sender, EventArgs e)
        {
            RenameSample();
        }

        private void btnCalculateKey_Click(object sender, EventArgs e)
        {
            BE.KeyHelper.CalculateKey(this.trackWave.Filename);
        }
    }
}