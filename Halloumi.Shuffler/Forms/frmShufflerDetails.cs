using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Halloumi.BassEngine.Channels;
using Halloumi.BassEngine.Helpers;
using Halloumi.BassEngine.Models;
using Halloumi.Common.Windows.Forms;
using Halloumi.Common.Windows.Helpers;
using BE = Halloumi.BassEngine;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Halloumi.Shuffler.Forms
{
    public partial class FrmShufflerDetails : BaseForm
    {
        private bool _bindingData;

        private bool _saved;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FrmShufflerDetails" /> class.
        /// </summary>
        public FrmShufflerDetails()
        {
            InitializeComponent();

            cmbCustomFadeInLength.TextChanged += cmbCustomFadeInLength_TextChanged;
            cmbCustomFadeOutLength.TextChanged += cmbCustomFadeOutLength_TextChanged;
            txtFadeInPosition.TextChanged += txtFadeInPosition_TextChanged;
            txtFadeOutStartPosition.TextChanged += txtFadeOutStartPosition_TextChanged;
            trackWave.PositionsChanged += trackWave_PositionsChanged;


            cmbSampleLength.SelectedIndexChanged += cmbSampleLength_SelectedIndexChanged;
            txtSampleStartPosition.TextChanged += txtSampleStartPosition_TextChanged;
            cmbSampleLength.TextChanged += cmbSampleLength_TextChanged;
            chkLoopSample.CheckedChanged += chkLoopSample_CheckedChanged;

        }

        public Track Track { get; private set; }


        public BE.BassPlayer BassPlayer { get; set; }

        public string Filename { get; set; }

        private AutomationAttributes AutomationAttributes => BassPlayer.GetAutomationAttributes(Track);

        private TrackSample CurrentSample { get; set; }

        private List<TrackSample> CurrentSamples { get; set; }

        private void btnDeleteTrackFX_Click(object sender, EventArgs e)
        {
            var trackFx = GetSelectedTrackFx();
            if (trackFx == null) return;

            AutomationAttributes.TrackFxTriggers.Remove(trackFx);

            PopulateTrackFxComboBox();
            trackWave.RefreshPositions();
        }

        private void btnUpdateTrackFX_Click(object sender, EventArgs e)
        {
            var trackFx = GetSelectedTrackFx();
            if (trackFx == null) return;

            if (rdbDelay1.Checked) trackFx.DelayNotes = 0.5M;
            if (rdbDelay2.Checked) trackFx.DelayNotes = 0.25M;
            if (rdbDelay3.Checked) trackFx.DelayNotes = 0.125M;
            if (rdbDelay4.Checked) trackFx.DelayNotes = 0.0625M;

            trackFx.Start = Track.SamplesToSeconds(trackWave.ZoomStart);
            trackFx.Length = Track.SamplesToSeconds(trackWave.ZoomLength);

            PopulateTrackFxComboBox();
            trackWave.RefreshPositions();
        }

        private void btnAddTrackFX_Click(object sender, EventArgs e)
        {
            var trackFx = new TrackFxTrigger();

            if (rdbDelay1.Checked) trackFx.DelayNotes = 0.5M;
            if (rdbDelay2.Checked) trackFx.DelayNotes = 0.25M;
            if (rdbDelay3.Checked) trackFx.DelayNotes = 0.125M;
            if (rdbDelay4.Checked) trackFx.DelayNotes = 0.0625M;

            trackFx.Start = Track.SamplesToSeconds(trackWave.ZoomStart);
            trackFx.Length = Track.SamplesToSeconds(trackWave.ZoomLength);

            AutomationAttributes.TrackFxTriggers.Add(trackFx);

            PopulateTrackFxComboBox();
            trackWave.RefreshPositions();
        }

        private void btnClearTrackFX_Click(object sender, EventArgs e)
        {
            var message = "Are you sure you wish to clear all Track FX triggers for " + Track.Description + "?";
            if (!MessageBoxHelper.Confirm(message)) return;

            AutomationAttributes.TrackFxTriggers.Clear();

            PopulateTrackFxComboBox();
            trackWave.RefreshPositions();
        }

        /// <summary>
        ///     Handles the Click event of the btnTrackFXZoom control.
        /// </summary>
        private void btnTrackFXZoom_Click(object sender, EventArgs e)
        {
            var trackFx = GetSelectedTrackFx();
            if (trackFx == null) return;

            var start = Track.SecondsToSamples(trackFx.Start);
            var end = Track.SecondsToSamples(trackFx.Start + trackFx.Length);

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
            txtSkipStart.Seconds = Track.SamplesToSeconds(trackWave.ZoomStart);
            cmbSkipLength.Seconds = Track.SamplesToSeconds(trackWave.ZoomLength);
            UpdateData();
        }

        private void btnSkipZoom_Click(object sender, EventArgs e)
        {
            trackWave.Zoom(Track.SkipStart, Track.SkipEnd);
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
        ///     Handles the SelectedIndexChanged event of the lstSamples control.
        /// </summary>
        private void lstSamples_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCurrentSample();
            if (lstSamples.SelectedItems.Count == 0)
            {
                CurrentSample = null;
            }
            else
            {
                var description = lstSamples.SelectedItems[0].Text;
                CurrentSample = CurrentSamples.FirstOrDefault(s => s.Description == description);
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

        private void Initialise()
        {
            trackWave.BassPlayer = BassPlayer;
            Track = trackWave.LoadTrack(Filename);
            cmbOutput.SelectedIndex = 0;

            CurrentSamples = new List<TrackSample>();
            foreach (var trackSample in AutomationAttributes.TrackSamples)
            {
                CurrentSamples.Add(trackSample.Clone());
            }
            trackWave.TrackSamples = CurrentSamples;

            SetControlStates();
            BindData();
            SetBpmValues();
            PopulateTrackFxComboBox();
        }


        /// <summary>
        ///     Loads the settings.
        /// </summary>
        public void LoadSettings()
        {
            try
            {
                var settings = Settings.Default;
                BassPlayer.RawLoopOutput = settings.RawLoopOutput;
                if (settings.RawLoopOutput == SoundOutput.Speakers) cmbOutput.SelectedIndex = 0;
                if (settings.RawLoopOutput == SoundOutput.Monitor) cmbOutput.SelectedIndex = 1;
                if (settings.RawLoopOutput == SoundOutput.Both) cmbOutput.SelectedIndex = 2;
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     Binds the data.
        /// </summary>
        private void BindData()
        {
            _bindingData = true;

            var loopLengths = BassHelper.GetLoopLengths(Track.StartBpm);
            cmbCustomFadeInLength.PopulateItemsFromSecondsList(loopLengths);

            loopLengths = BassHelper.GetLoopLengths(Track.EndBpm);
            cmbCustomFadeOutLength.PopulateItemsFromSecondsList(loopLengths);

            lblStartBPM.Text = Track.StartBpm.ToString("0.00");
            lblEndBPM.Text = Track.EndBpm.ToString("0.00");

            Text = @"Halloumi : Shuffler : Shuffler Details : " + Track.Description;

            chkUsePreFadeIn.Checked = Track.UsePreFadeIn;

            chkUseSkipSection.Checked = Track.HasSkipSection;
            txtSkipStart.Seconds = Track.SamplesToSeconds(Track.SkipStart);
            cmbSkipLength.Seconds = Track.SkipLengthSeconds;
            cmbSkipLength.PopulateItemsFromSecondsList(loopLengths);

            txtFadeInPosition.Seconds = Track.SamplesToSeconds(Track.FadeInStart);
            txtFadeOutStartPosition.Seconds = Track.SamplesToSeconds(Track.FadeOutStart);
            txtPreFadeInStartPosition.Seconds = Track.SamplesToSeconds(Track.PreFadeInStart);

            if (Track.FadeInEnd != 0)
                cmbCustomFadeInLength.Seconds = Track.SamplesToSeconds(Track.FadeInEnd - Track.FadeInStart);
            if (Track.FadeOutEnd != 0)
                cmbCustomFadeOutLength.Seconds = Track.SamplesToSeconds(Track.FadeOutEnd - Track.FadeOutStart);

            cmbPreFadeInStartVolume.Text = (Track.PreFadeInStartVolume*100).ToString(CultureInfo.InvariantCulture);

            cmbFadeInLoopCount.Text = !Track.IsLoopedAtStart ? "0" : Track.StartLoopCount.ToString();

            cmbFadeOutLoopCount.Text = !Track.IsLoopedAtEnd ? "0" : Track.EndLoopCount.ToString();

            chkPowerDown.Checked = Track.PowerDownOnEnd;

            BindSamples();
            BindSample();

            _bindingData = false;
        }

        /// <summary>
        ///     Populates the volume drop down.
        /// </summary>
        /// <param name="comboBox">The combo box.</param>
        private static void PopulateVolumeDropDown(KryptonComboBox comboBox)
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
        ///     Sets the control states.
        /// </summary>
        private void SetControlStates()
        {
            txtPreFadeInStartPosition.Enabled = chkUsePreFadeIn.Checked;
            cmbPreFadeInStartVolume.Enabled = chkUsePreFadeIn.Checked;

            txtSkipStart.Enabled = chkUseSkipSection.Checked;
            cmbSkipLength.Enabled = chkUseSkipSection.Checked;
        }

        /// <summary>
        ///     Sets the BPM values.
        /// </summary>
        private void SetBpmValues()
        {
            if (_bindingData) return;

            var fadeInLength = BassHelper.GetDefaultLoopLength(Track.TagBpm);
            if (cmbCustomFadeInLength.Seconds != 0D)
            {
                fadeInLength = cmbCustomFadeInLength.Seconds;
            }
            var startBpm = BassHelper.GetBpmFromLoopLength(fadeInLength);
            lblStartBPM.Text = startBpm.ToString("0.00");

            var fadeOutLength = BassHelper.GetDefaultLoopLength(Track.TagBpm);
            if (cmbCustomFadeOutLength.Seconds != 0D)
            {
                fadeOutLength = cmbCustomFadeOutLength.Seconds;
            }
            var endBpm = BassHelper.GetBpmFromLoopLength(fadeOutLength);
            lblEndBPM.Text = endBpm.ToString("0.00");

            PopulateVolumeDropDown(cmbPreFadeInStartVolume);
        }

        /// <summary>
        ///     Saves the track.
        /// </summary>
        private void SaveTrack()
        {
            UpdateData();
            UpdateCurrentSample();

            AutomationAttributes.TrackSamples.Clear();
            foreach (var sample in CurrentSamples.Where(sample => sample.Length != 0 || sample.Start != 0))
            {
                AutomationAttributes.TrackSamples.Add(sample);
            }

            BassPlayer.SaveExtendedAttributes(Track);
            BassPlayer.ReloadTrack(Track.Filename);
            BassPlayer.SaveAutomationAttributes(Track);
            BassPlayer.ReloadTrack(Track.Filename);

            _saved = true;

            Close();
        }

        /// <summary>
        ///     Updates the track from the controls
        /// </summary>
        private void UpdateData()
        {
            if (_bindingData) return;

            Track.ChangeTempoOnFadeOut = true;

            Track.FadeInStart = Track.SecondsToSamples(txtFadeInPosition.Seconds);
            Track.FadeInEnd = Track.FadeInStart + Track.SecondsToSamples(cmbCustomFadeInLength.Seconds);

            Track.FadeOutStart = Track.SecondsToSamples(txtFadeOutStartPosition.Seconds);
            Track.FadeOutEnd = Track.FadeOutStart + Track.SecondsToSamples(cmbCustomFadeOutLength.Seconds);

            Track.UsePreFadeIn = chkUsePreFadeIn.Checked;
            if (chkUsePreFadeIn.Checked)
            {
                Track.PreFadeInStart = Track.SecondsToSamples(txtPreFadeInStartPosition.Seconds);
                Track.PreFadeInStartVolume = float.Parse(cmbPreFadeInStartVolume.Text)/100F;
            }
            else
            {
                Track.PreFadeInStart = 0;
                Track.PreFadeInStartVolume = 0F;
            }

            Track.PowerDownOnEnd = chkPowerDown.Checked;

            if (cmbFadeInLoopCount.Text == "") cmbFadeInLoopCount.Text = @"0";
            if (cmbFadeOutLoopCount.Text == "") cmbFadeOutLoopCount.Text = @"0";

            Track.StartLoopCount = int.Parse(cmbFadeInLoopCount.Text);
            Track.EndLoopCount = int.Parse(cmbFadeOutLoopCount.Text);

            Track.SkipStart = Track.SecondsToSamples(txtSkipStart.Seconds);
            if (Track.SkipStart != 0)
                Track.SkipEnd = Track.SkipStart + Track.SecondsToSamples(cmbSkipLength.Seconds);
            else
                Track.SkipEnd = 0;

            trackWave.RefreshPositions();

            SetBpmValues();
            SetControlStates();
        }

        /// <summary>
        ///     Populates the track FX combo box.
        /// </summary>
        private void PopulateTrackFxComboBox()
        {
            cmbTrackFX.Items.Clear();

            foreach (var trigger in AutomationAttributes.TrackFxTriggers.OrderBy(t => t.Start).ToList())
            {
                cmbTrackFX.Items.Add(BassHelper.GetFormattedSecondsNoHours(trigger.Start));
            }

            if (cmbTrackFX.Items.Count > 0) cmbTrackFX.SelectedIndex = 0;
        }

        /// <summary>
        ///     Gets the selected track FX.
        /// </summary>
        /// <returns>The selected track FX.</returns>
        private TrackFxTrigger GetSelectedTrackFx()
        {
            if (cmbTrackFX.Items.Count == 0 || cmbTrackFX.SelectedIndex < 0) return null;

            var selectedText = cmbTrackFX.SelectedItem.ToString();

            return AutomationAttributes
                .TrackFxTriggers
                .OrderBy(t => t.Start)
                .FirstOrDefault(trackFx => selectedText == BassHelper.GetFormattedSecondsNoHours(trackFx.Start));
        }

        private void BindSamples()
        {
            lstSamples.SuspendLayout();
            lstSamples.Items.Clear();
            foreach (var trackSample in CurrentSamples)
            {
                var item = new ListViewItem(trackSample.Description);
                lstSamples.Items.Add(item);
                item.Selected = (trackSample == CurrentSample);
            }
            lstSamples.ResumeLayout();
            trackWave.RefreshPositions();
        }

        private void BindSample()
        {
            List<double> loopLengths;
            if (CurrentSample == null)
            {
                txtSampleStartPosition.Seconds = 0;
                loopLengths = BassHelper.GetLoopLengths(Track.Bpm);
                cmbSampleLength.Seconds = 0;
                chkLoopSample.Checked = false;
            }
            else
            {
                txtSampleStartPosition.Seconds = CurrentSample.Start;
                loopLengths = BassHelper.GetLoopLengths(CurrentSample.CalculateBpm(Track));
                cmbSampleLength.Seconds = CurrentSample.Length;
                chkLoopSample.Checked = CurrentSample.IsLooped;
            }
            cmbSampleLength.PopulateItemsFromSecondsList(loopLengths);
        }

        private void AddSample()
        {
            var sampleName = UserInputHelper.GetUserInput("Sample Name", "", this);
            if (sampleName == "") return;

            var sampleKey = "";
            for (var i = 0; i < 2000; i++)
            {
                sampleKey = "Sample" + (i + 1);
                if (!CurrentSamples.Exists(s => s.Key == sampleKey))
                {
                    break;
                }
            }

            var trackSample = new TrackSample
            {
                Description = sampleName,
                Key = sampleKey
            };
            CurrentSamples.Add(trackSample);
            CurrentSample = trackSample;

            BindSamples();
            BindSample();
        }

        private void RemoveSample()
        {
            if (CurrentSample == null) return;

            var message = "Are you sure you wish to delete sample '" + CurrentSample.Description + "'?";
            if (!MessageBoxHelper.Confirm(message)) return;

            CurrentSamples.Remove(CurrentSample);
            CurrentSample = null;
            BindSamples();
            BindSample();
        }

        private void RenameSample()
        {
            if (CurrentSample == null) return;
            var sampleName = UserInputHelper.GetUserInput("Rename Sample", CurrentSample.Description, this);
            if (sampleName == CurrentSample.Description) return;

            CurrentSample.Description = sampleName;
            BindSamples();
            BindSample();
        }

        private void UpdateCurrentSample()
        {
            if (CurrentSample == null) return;

            CurrentSample.IsLooped = chkLoopSample.Checked;
            if (txtSampleStartPosition.Seconds != 0D) CurrentSample.Start = txtSampleStartPosition.Seconds;
            if (cmbSampleLength.Seconds != 0D) CurrentSample.Length = cmbSampleLength.Seconds;
        }


        /// <summary>
        ///     Handles the Click event of the btnOK control.
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveTrack();
        }

        /// <summary>
        ///     Handles the CheckedChanged event of the chkUsePreFadeIn control.
        /// </summary>
        private void chkUsePreFadeIn_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUsePreFadeIn.Checked)
            {
                if (cmbPreFadeInStartVolume.Text == @"0")
                    cmbPreFadeInStartVolume.Text = @"75";
            }
            else
            {
                cmbPreFadeInStartVolume.Text = @"0";
            }

            SetControlStates();
            UpdateData();
        }

        /// <summary>
        ///     Handles the Click event of the btnCancel control.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        ///     Handles the SelectedIndexChanged event of the cmbFadeInLoopCount control.
        /// </summary>
        private void cmbFadeInLoopCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        /// <summary>
        ///     Handles the SelectedIndexChanged event of the cmbFadeOutLoopCount control.
        /// </summary>
        private void cmbFadeOutLoopCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        /// <summary>
        ///     Handles the SelectedIndexChanged event of the cmbCustomFadeInLength control.
        /// </summary>
        private void cmbCustomFadeInLength_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        /// <summary>
        ///     Handles the SelectedIndexChanged event of the cmbCustomFadeOutLength control.
        /// </summary>
        private void cmbCustomFadeOutLength_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        /// <summary>
        ///     Handles the FormClosed event of the frmTrack control.
        /// </summary>
        private void frmTrack_FormClosed(object sender, FormClosedEventArgs e)
        {
            trackWave.Unload();
            if (!_saved) return;

            BassPlayer.ReloadAutomationAttributes(Track);
            BassPlayer.ReloadTrack(Track.Filename);
        }

        /// <summary>
        ///     Handles the TextChanged event of the txtFadeOutStartPosition control.
        /// </summary>
        private void txtFadeOutStartPosition_TextChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        /// <summary>
        ///     Handles the TextChanged event of the txtFadeInPosition control.
        /// </summary>
        private void txtFadeInPosition_TextChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        /// <summary>
        ///     Handles the TextChanged event of the cmbCustomFadeOutLength control.
        /// </summary>
        private void cmbCustomFadeOutLength_TextChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        /// <summary>
        ///     Handles the TextChanged event of the cmbCustomFadeInLength control.
        /// </summary>
        private void cmbCustomFadeInLength_TextChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        /// <summary>
        ///     Handles the TextChanged event of the trackWave control.
        /// </summary>
        private void trackWave_PositionsChanged(object sender, EventArgs e)
        {
            _bindingData = true;

            chkUsePreFadeIn.Checked = Track.UsePreFadeIn;
            txtFadeInPosition.Seconds = Track.SamplesToSeconds(Track.FadeInStart);
            txtFadeOutStartPosition.Seconds = Track.SamplesToSeconds(Track.FadeOutStart);
            txtPreFadeInStartPosition.Seconds = Track.SamplesToSeconds(Track.PreFadeInStart);
            if (Track.FadeInEnd != 0)
                cmbCustomFadeInLength.Seconds = Track.SamplesToSeconds(Track.FadeInEnd - Track.FadeInStart);
            if (Track.FadeOutEnd != 0)
                cmbCustomFadeOutLength.Seconds = Track.SamplesToSeconds(Track.FadeOutEnd - Track.FadeOutStart);

            chkUseSkipSection.Checked = Track.HasSkipSection;
            txtSkipStart.Seconds = Track.SamplesToSeconds(Track.SkipStart);
            if (Track.SkipEnd != 0) cmbSkipLength.Seconds = Track.SamplesToSeconds(Track.SkipEnd - Track.SkipStart);

            _bindingData = false;
        }

        /// <summary>
        ///     Handles the Click event of the btnFadeInUpdate control.
        /// </summary>
        private void btnFadeInUpdate_Click(object sender, EventArgs e)
        {
            txtFadeInPosition.Seconds = Track.SamplesToSeconds(trackWave.ZoomStart);
            cmbCustomFadeInLength.Seconds = Track.SamplesToSeconds(trackWave.ZoomLength);
            UpdateData();
        }

        /// <summary>
        ///     Handles the Click event of the btnFadeOutUpdate control.
        /// </summary>
        private void btnFadeOutUpdate_Click(object sender, EventArgs e)
        {
            txtFadeOutStartPosition.Seconds = Track.SamplesToSeconds(trackWave.ZoomStart);
            cmbCustomFadeOutLength.Seconds = Track.SamplesToSeconds(trackWave.ZoomLength);
            UpdateData();
        }

        private void btnSampleUpdate_Click(object sender, EventArgs e)
        {
            txtSampleStartPosition.Seconds = Track.SamplesToSeconds(trackWave.ZoomStart);
            cmbSampleLength.Seconds = Track.SamplesToSeconds(trackWave.ZoomLength);
            UpdateCurrentSample();
        }

        private void btnZoomSample_Click(object sender, EventArgs e)
        {
            ZoomToSample();
        }

        private void ZoomToSample()
        {
            if (CurrentSample == null) return;
            trackWave.Zoom(CurrentSample.Start, CurrentSample.Length);
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
        ///     Handles the Load event of the frmShufflerDetails control.
        /// </summary>
        private void frmShufflerDetails_Load(object sender, EventArgs e)
        {
            Initialise();
        }

        /// <summary>
        ///     Handles the Click event of the btnZoomFadeIn control.
        /// </summary>
        private void btnZoomFadeIn_Click(object sender, EventArgs e)
        {
            trackWave.Zoom(Track.FadeInStart, Track.FadeInEnd);
        }

        /// <summary>
        ///     Handles the Click event of the btnZoomPreFade control.
        /// </summary>
        private void btnZoomPreFade_Click(object sender, EventArgs e)
        {
            trackWave.Zoom(Track.PreFadeInStart, Track.FadeInStart);
        }

        /// <summary>
        ///     Handles the Click event of the btnZoomFadeOut control.
        /// </summary>
        private void btnZoomFadeOut_Click(object sender, EventArgs e)
        {
            trackWave.Zoom(Track.FadeOutStart, Track.FadeOutEnd);
        }

        /// <summary>
        ///     Handles the SelectedIndexChanged event of the cmbOutput control.
        /// </summary>
        private void cmbOutput_SelectedIndexChanged(object sender, EventArgs e)
        {
            var outputType = cmbOutput.ParseEnum<SoundOutput>();
            BassPlayer.RawLoopOutput = outputType;
        }

        /// <summary>
        ///     Handles the Click event of the btnPreFadeInUpdate control.
        /// </summary>
        private void btnPreFadeInUpdate_Click(object sender, EventArgs e)
        {
            if (trackWave.ZoomStart >= Track.FadeInStart) return;
            txtPreFadeInStartPosition.Seconds = Track.SamplesToSeconds(trackWave.ZoomStart);
            UpdateData();
        }

        /// <summary>
        ///     Handles the CheckedChanged event of the chkShowTrackFX control.
        /// </summary>
        private void chkShowTrackFX_CheckedChanged(object sender, EventArgs e)
        {
            trackWave.ShowTrackFx = chkShowTrackFX.Checked;
        }

        /// <summary>
        ///     Handles the SelectedIndexChanged event of the cmbTrackFX control.
        /// </summary>
        private void cmbTrackFX_SelectedIndexChanged(object sender, EventArgs e)
        {
            var trackFx = GetSelectedTrackFx();
            if (trackFx == null) rdbDelay2.Checked = true;
            else if (trackFx.DelayNotes == 0.5M) rdbDelay1.Checked = true;
            else if (trackFx.DelayNotes == 0.25M) rdbDelay2.Checked = true;
            else if (trackFx.DelayNotes == 0.125M) rdbDelay3.Checked = true;
            else if (trackFx.DelayNotes == 0.0625M) rdbDelay4.Checked = true;
            else rdbDelay2.Checked = true;
        }
    }
}