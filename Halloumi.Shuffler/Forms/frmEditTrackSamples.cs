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
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Forms;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.Controls;
using Halloumi.Shuffler.Engine;
using BE = Halloumi.BassEngine;

namespace Halloumi.Shuffler.Forms
{
    public partial class FrmEditTrackSamples : BaseForm
    {
        #region Constructors

        public BE.BassPlayer BassPlayer { get; set; }

        public string Filename { get; set; }

        public SampleLibrary SampleLibrary { get; set; }

        public Library Library { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="frmTrack"/> class.
        /// </summary>
        public FrmEditTrackSamples()
        {
            InitializeComponent();

            trackWave.PositionsChanged += new EventHandler(trackWave_PositionsChanged);
        }

        private void Initialise()
        {
            trackWave.Mode = TrackWave.TrackWaveMode.Sampler;
            trackWave.BassPlayer = this.BassPlayer;
            this.Track = trackWave.LoadTrack(this.Filename);
            this.LibraryTrack = this.Library.GetTrackByFilename(this.Filename);

            cmbOutput.SelectedIndex = 0;

            this.Samples = new List<Sample>();
            var samples = this.SampleLibrary.GetSamples(this.LibraryTrack);

            foreach (var sample in samples)
            {
                this.Samples.Add(sample.Clone());
            }
            this.trackWave.Samples = this.Samples;

            BindData();
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

            BindTrack();
            BindSamples();
            BindSample();

            _bindingData = false;
        }

        private void BindTrack()
        {
            lblTrackBPM.Text = this.Track.Bpm.ToString("0.00");
            this.Text = "Halloumi : Shuffler : Sample Details : " + this.Track.Description;

            lblTrackArtist.Text = this.LibraryTrack.Artist;
            lblTrackTitle.Text = this.LibraryTrack.Title;
            lblTrackGenre.Text = this.LibraryTrack.Genre;
            lblTrackKey.Text = BE.KeyHelper.GetDisplayKey(this.LibraryTrack.Key);
        }

        private bool _bindingData = false;

        /// <summary>
        /// Saves the track.
        /// </summary>
        private void SaveSamples()
        {
            trackWave.Stop();

            RefreshTrackWavePositions();
            UpdateCurrentSample();

            foreach (var sample in this.Samples)
            {
                sample.Gain = this.trackWave.GetNormalizationGain(sample.Start, sample.Length);
            }

            this.SampleLibrary.UpdateTrackSamples(this.LibraryTrack, this.Samples);
            this.SampleLibrary.SaveCache();

            this.SampleLibrary.SaveSampleFiles(this.LibraryTrack);

            //_saved = true;

            this.Close();
        }

        //private bool _saved = false;

        /// <summary>
        /// Updates the track from the controls
        /// </summary>
        private void RefreshTrackWavePositions()
        {
            if (_bindingData) return;

            trackWave.RefreshPositions();
            CalculateSampleBpm();
        }

        private Sample CurrentSample
        {
            get;
            set;
        }

        private List<Sample> Samples
        {
            get;
            set;
        }

        private void BindSamples()
        {
            lstSamples.SuspendLayout();
            lstSamples.Items.Clear();
            foreach (var sample in this.Samples)
            {
                var item = new ListViewItem(sample.Description);
                lstSamples.Items.Add(item);
                item.Selected = (sample == this.CurrentSample);
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
                txtSampleOffsetPosition.Seconds = 0;
                loopLengths = BE.BassHelper.GetLoopLengths(this.Track.Bpm);
                cmbSampleLength.Seconds = 0;
                SetLoopModeOnDropdown(LoopMode.FullLoop);
                chkAtonal.Checked = false;
                chkPrimaryLoop.Checked = false;
                lblSampleBPM.Text = "100.00";
                txtTags.Text = "";
            }
            else
            {
                loopLengths = BE.BassHelper.GetLoopLengths(this.CurrentSample.Bpm);

                txtSampleStartPosition.Seconds = this.CurrentSample.Start;
                txtSampleOffsetPosition.Seconds = this.CurrentSample.Offset;

                lblSampleBPM.Text = string.Format("0.00", this.CurrentSample.Bpm);

                cmbSampleLength.Seconds = this.CurrentSample.Length;
                SetLoopModeOnDropdown(this.CurrentSample.LoopMode);
                chkAtonal.Checked = this.CurrentSample.IsAtonal;
                chkPrimaryLoop.Checked = this.CurrentSample.IsPrimaryLoop;

                txtTags.Text = string.Join(", ", this.CurrentSample.Tags.ToArray());
            }

            // SetSampleCheckBoxes();
            SetSampleBpmLabel();
            cmbSampleLength.PopulateItemsFromSecondsList(loopLengths);
        }

        private void SetSampleBpmLabel()
        {
            var bmp = CalculateSampleBpm();
            lblSampleBPM.Text = bmp.ToString("0.00");
        }

        private void SetLoopModeOnDropdown(LoopMode loopMode)
        {
            if (loopMode == LoopMode.FullLoop)
                cmbLoopMode.SelectedIndex = 0;

            if (loopMode == LoopMode.PartialLoopAnchorStart)
                cmbLoopMode.SelectedIndex = 1;

            if (loopMode == LoopMode.PartialLoopAnchorEnd)
                cmbLoopMode.SelectedIndex = 2;
        }

        private LoopMode GetLoopModeFromDropdown()
        {
            return (LoopMode)Enum.Parse(typeof(LoopMode), StringHelper.GetAlphabeticCharactersOnly(cmbLoopMode.GetTextThreadSafe()));
        }

        private decimal CalculateSampleBpm()
        {
            var length = cmbSampleLength.Seconds;
            var start = txtSampleStartPosition.Seconds;

            if (length == 0D) return 0M;

            if (GetLoopModeFromDropdown() == LoopMode.FullLoop)
                return BE.BassHelper.GetBpmFromLoopLength(length);

            var samples = this.Samples.Where(x => x.LoopMode == LoopMode.FullLoop).ToList();
            samples.Remove(this.CurrentSample);

            if (samples.Count() == 0) return this.Track.Bpm;

            return samples
                .OrderByDescending(x => Math.Abs(start - x.Start))
                .FirstOrDefault()
                .Bpm;
        }

        private void AddSample()
        {
            var sampleName = UserInputHelper.GetUserInput("Sample Name", "", this);
            if (sampleName != "")
            {
                var sample = new Sample()
                {
                    Description = sampleName,
                };

                this.SampleLibrary.UpdateSampleFromTrack(sample, this.LibraryTrack);

                this.Samples.Add(sample);
                this.CurrentSample = sample;
                this.trackWave.CurrentSample = sample;
                this.trackWave.Samples = this.Samples;

                sample.Start = this.Track.SamplesToSeconds(trackWave.ZoomStart);
                sample.Length = this.Track.SamplesToSeconds(trackWave.ZoomEnd - trackWave.ZoomStart);
                sample.Bpm = BE.BassHelper.GetBpmFromLoopLength(sample.Length);

                RefreshTrackWavePositions();

                BindData();
            }
        }

        private void RemoveSample()
        {
            if (this.CurrentSample == null) return;

            var message = "Are you sure you wish to delete sample '" + this.CurrentSample.Description + "'?";
            if (MessageBoxHelper.Confirm(message))
            {
                this.Samples.Remove(this.CurrentSample);
                this.CurrentSample = null;
                this.trackWave.CurrentSample = null;
                this.trackWave.RefreshPositions();

                BindData();
            }
        }

        private void RenameSample()
        {
            if (this.CurrentSample == null) return;
            var sampleName = UserInputHelper.GetUserInput("Rename Sample", this.CurrentSample.Description, this);
            if (sampleName != this.CurrentSample.Description)
            {
                this.CurrentSample.Description = sampleName;
                BindData();
            }
        }

        private void UpdateCurrentSample()
        {
            if (this.CurrentSample == null) return;

            this.CurrentSample.LoopMode = GetLoopModeFromDropdown();
            this.CurrentSample.IsAtonal = chkAtonal.Checked;
            this.CurrentSample.IsPrimaryLoop = chkPrimaryLoop.Checked;

            if (txtSampleStartPosition.Seconds != 0) this.CurrentSample.Start = txtSampleStartPosition.Seconds;
            if (txtSampleOffsetPosition.Seconds != 0) this.CurrentSample.Offset = txtSampleOffsetPosition.Seconds;
            if (cmbSampleLength.Seconds != 0) this.CurrentSample.Length = cmbSampleLength.Seconds;

            this.CurrentSample.Bpm = CalculateSampleBpm();

            this.CurrentSample.Tags = txtTags.Text
                .Split(',')
                .Select(x => x.ToLower().Trim())
                .OrderBy(x => x)
                .ToList();
        }

        #endregion

        #region Properties

        public BE.Track Track { get; private set; }

        public Track LibraryTrack { get; private set; }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the Click event of the btnOK control.
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveSamples();
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles the FormClosed event of the frmTrack control.
        /// </summary>
        private void frmSamples_FormClosed(object sender, FormClosedEventArgs e)
        {
            trackWave.Unload();
        }

        /// <summary>
        /// Handles the TextChanged event of the txtFadeOutStartPosition control.
        /// </summary>
        private void txtFadeOutStartPosition_TextChanged(object sender, EventArgs e)
        {
            RefreshTrackWavePositions();
        }

        /// <summary>
        /// Handles the TextChanged event of the trackWave control.
        /// </summary>
        private void trackWave_PositionsChanged(object sender, EventArgs e)
        {
            BindData();
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
            trackWave.Zoom(this.CurrentSample.Start, this.CurrentSample.Length, this.CurrentSample.Offset);
        }

        private void cmbSampleLength_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshTrackWavePositions();
        }

        private void txtSampleStartPosition_TextChanged(object sender, EventArgs e)
        {
            RefreshTrackWavePositions();
        }

        private void cmbSampleLength_TextChanged(object sender, EventArgs e)
        {
            RefreshTrackWavePositions();
        }

        private void chkLoopSample_CheckedChanged(object sender, EventArgs e)
        {
            RefreshTrackWavePositions();
        }

        /// <summary>
        /// Handles the Load event of the frmSamples control.
        /// </summary>
        private void frmSamples_Load(object sender, EventArgs e)
        {
            Initialise();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cmbOutput control.
        /// </summary>
        private void cmbOutput_SelectedIndexChanged(object sender, EventArgs e)
        {
            var outputType = cmbOutput.ParseEnum<BE.Channels.SoundOutput>();
            this.BassPlayer.RawLoopOutput = outputType;
        }

        #endregion

        /// <summary>
        /// Handles the SelectedIndexChanged event of the lstSamples control.
        /// </summary>
        private void lstSamples_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_bindingData) return;

            this.trackWave.Stop();

            UpdateCurrentSample();
            if (lstSamples.SelectedItems.Count == 0)
            {
                this.CurrentSample = null;
                this.trackWave.CurrentSample = null;
            }
            else
            {
                var description = lstSamples.SelectedItems[0].Text;
                this.CurrentSample = this.Samples.Where(s => s.Description == description).FirstOrDefault();
                this.trackWave.CurrentSample = this.CurrentSample;
            }
            this.trackWave.RefreshPositions();
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

        private void txtSampleOffsetPosition_TextChanged(object sender, EventArgs e)
        {
            RefreshTrackWavePositions();
        }

        private void cmbSampleLength_TextUpdate(object sender, EventArgs e)
        {
            RefreshTrackWavePositions();
        }

        private void cmbLoopMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSampleBpmLabel();
        }

        private void btnImportSamplesFromMix_Click(object sender, EventArgs e)
        {
            var mixSamples = this.SampleLibrary.GetMixSectionsAsSamples(this.LibraryTrack);
            this.Samples.AddRange(mixSamples);

            UpdateCurrentSample();
            BindSamples();
        }
    }
}