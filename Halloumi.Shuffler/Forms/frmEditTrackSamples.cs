using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Forms;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.AudioEngine.BassPlayer;
using Halloumi.Shuffler.AudioEngine.Channels;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.AudioLibrary.Samples;
using Halloumi.Shuffler.Controls;
using AE = Halloumi.Shuffler.AudioEngine;
using Track = Halloumi.Shuffler.AudioEngine.Models.Track;


namespace Halloumi.Shuffler.Forms
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public partial class FrmEditTrackSamples : BaseForm
    {
        private bool _bindingData;

        private bool _updateSampleLibrary;

        public FrmEditTrackSamples()
        {
            InitializeComponent();

            trackWave.PositionsChanged += trackWave_PositionsChanged;
            cmbSampleLength.TextChanged += cmbSampleLength_TextChanged;
        }


        public BassPlayer BassPlayer { get; set; }

        public string Filename { get; set; }

        public TrackSampleLibrary TrackSampleLibrary { get; set; }

        public Library Library { get; set; }

        private Sample CurrentSample { get; set; }

        public List<Sample> Samples { get; set; }

        public Track Track { get; private set; }

        public AudioLibrary.Models.Track LibraryTrack { get; private set; }

        public string InitialSample { get; set; }

        /// <summary>
        ///     Handles the SelectedIndexChanged event of the lstSamples control.
        /// </summary>
        private void lstSamples_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_bindingData) return;

            trackWave.Stop();

            UpdateCurrentSample();
            if (lstSamples.SelectedItems.Count == 0)
            {
                CurrentSample = null;
                trackWave.SetCurrentSample(null);
            }
            else
            {
                var description = lstSamples.SelectedItems[0].Text;
                CurrentSample = Samples.FirstOrDefault(s => s.Description == description);
                trackWave.SetCurrentSample(CurrentSample);
            }
            trackWave.RefreshPositions();
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

        //private void btnCalculateKey_Click(object sender, EventArgs e)
        //{
        //    KeyHelper.CalculateKey(trackWave.Filename);
        //}

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
            var mixSamples = TrackSampleLibrary.GetMixSectionsAsSamples(LibraryTrack);
            Samples.AddRange(mixSamples);

            UpdateCurrentSample();
            BindSamples();
        }

        private void Initialise()
        {
            trackWave.SetModeAndBassPlayer(TrackWave.TrackWaveMode.Sampler, BassPlayer);

            Track = trackWave.LoadTrack(Filename);
            LibraryTrack = Library.GetTrackByFilename(Filename) ?? Library.LoadNonLibraryTrack(Filename);

            cmbOutput.SelectedIndex = 0;

            if ((Samples == null || (Samples != null && Samples.Count == 0)) && LibraryTrack != null)
            {
                _updateSampleLibrary = true;
                Samples = TrackSampleLibrary
                    .GetSamples(LibraryTrack)
                    .Select(sample => sample.Clone())
                    .ToList();
            }
            else
            {
                if (LibraryTrack != null && Samples != null)
                    foreach (var sample in Samples)
                    {
                        TrackSampleLibrary.UpdateSampleFromTrack(sample, LibraryTrack);
                    }
            }


            trackWave.SetSamples(Samples);

            btnImportSamplesFromMix.Visible = LibraryTrack != null && LibraryTrack.IsShufflerTrack;

            BindData();

            for (var i = 0; i < lstSamples.Items.Count; i++)
            {
                lstSamples.Items[i].Selected = lstSamples.Items[i].Text == InitialSample;
            }

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

            BindTrack();
            BindSamples();
            BindSample();

            _bindingData = false;
        }

        private void BindTrack()
        {
            lblTrackBPM.Text = Track.Bpm.ToString("0.00");
            Text = @"Halloumi : Shuffler : Sample Details : " + Track.Description;

            lblTrackArtist.Text = LibraryTrack.Artist;
            lblTrackTitle.Text = LibraryTrack.Title;
            lblTrackGenre.Text = LibraryTrack.Genre;
            lblTrackKey.Text = KeyHelper.GetDisplayKey(LibraryTrack.Key);
        }

        /// <summary>
        ///     Saves the track.
        /// </summary>
        private void SaveSamples()
        {
            trackWave.Stop();

            RefreshTrackWavePositions();
            UpdateCurrentSample();

            foreach (var sample in Samples)
            {
                sample.Gain = trackWave.GetNormalizationGain(sample.Start, sample.Length);
            }

            if (_updateSampleLibrary)
            {
                TrackSampleLibrary.UpdateTrackSamples(LibraryTrack, Samples);
                TrackSampleLibrary.SaveToCache();
            }

            Close();
        }

        /// <summary>
        ///     Updates the track from the controls
        /// </summary>
        private void RefreshTrackWavePositions()
        {
            if (_bindingData) return;

            trackWave.RefreshPositions();
            CalculateSampleBpm();
        }

        private void BindSamples()
        {
            lstSamples.SuspendLayout();
            lstSamples.Items.Clear();
            foreach (var sample in Samples)
            {
                var item = new ListViewItem(sample.Description);
                lstSamples.Items.Add(item);
                item.Selected = sample == CurrentSample;
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
                txtSampleOffsetPosition.Seconds = 0;
                loopLengths = BpmHelper.GetLoopLengths(Track.Bpm);
                cmbSampleLength.Seconds = 0;
                SetLoopModeOnDropdown(LoopMode.FullLoop);
                chkAtonal.Checked = false;
                chkPrimaryLoop.Checked = false;
                lblSampleBPM.Text = @"100.00";
                txtTags.Text = "";
            }
            else
            {
                loopLengths = BpmHelper.GetLoopLengths(CurrentSample.Bpm);

                txtSampleStartPosition.Seconds = CurrentSample.Start;
                txtSampleOffsetPosition.Seconds = CurrentSample.Offset;

                lblSampleBPM.Text = CurrentSample.Bpm.ToString("0.00");

                cmbSampleLength.Seconds = CurrentSample.Length;
                SetLoopModeOnDropdown(CurrentSample.LoopMode);
                chkAtonal.Checked = CurrentSample.IsAtonal;
                chkPrimaryLoop.Checked = CurrentSample.IsPrimaryLoop;

                txtTags.Text = string.Join(", ", CurrentSample.Tags.ToArray());
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
            return
                (LoopMode)
                    Enum.Parse(typeof(LoopMode),
                        StringHelper.GetAlphabeticCharactersOnly(cmbLoopMode.GetTextThreadSafe()));
        }

        private decimal CalculateSampleBpm()
        {
            var length = cmbSampleLength.Seconds;
            var start = txtSampleStartPosition.Seconds;

            if (length == 0D) return 0M;

            if (GetLoopModeFromDropdown() == LoopMode.FullLoop)
                return BpmHelper.GetBpmFromLoopLength(length);

            var samples = Samples.Where(x => x.LoopMode == LoopMode.FullLoop).ToList();
            samples.Remove(CurrentSample);

            if (!samples.Any()) return Track.Bpm;

            var firstSample = samples
                .OrderByDescending(x => Math.Abs(start - x.Start))
                .FirstOrDefault();

            return firstSample?.Bpm ?? 100M;
        }

        private void AddSample()
        {
            var sampleName = UserInputHelper.GetUserInput("Sample Name", "", this);
            if (sampleName == "") return;

            var start = Track.SamplesToSeconds(trackWave.ZoomStart);
            var length = Track.SamplesToSeconds(trackWave.ZoomEnd - trackWave.ZoomStart);

            AddSample(sampleName, start, length);
        }

        private void AddSample(string sampleName, double start, double length)
        {
            var sample = new Sample
            {
                Description = sampleName
            };

            if (LibraryTrack != null)
                TrackSampleLibrary.UpdateSampleFromTrack(sample, LibraryTrack);

            Samples.Add(sample);
            CurrentSample = sample;
            trackWave.SetCurrentSample(sample);
            trackWave.SetSamples(Samples);

            sample.Start = start;
            sample.Length = length;
            sample.Bpm = BpmHelper.GetBpmFromLoopLength(sample.Length);

            RefreshTrackWavePositions();

            BindData();
        }

        private void RemoveSample()
        {
            if (CurrentSample == null) return;

            var message = "Are you sure you wish to delete sample '" + CurrentSample.Description + "'?";
            if (!MessageBoxHelper.Confirm(message)) return;

            Samples.Remove(CurrentSample);
            CurrentSample = null;
            trackWave.SetCurrentSample(null);
            trackWave.RefreshPositions();

            BindData();
        }

        private void RenameSample()
        {
            if (CurrentSample == null) return;
            var sampleName = UserInputHelper.GetUserInput("Rename Sample", CurrentSample.Description, this);
            if (sampleName == CurrentSample.Description) return;

            CurrentSample.Description = sampleName;
            BindData();
        }

        private void UpdateCurrentSample()
        {
            if (CurrentSample == null) return;

            CurrentSample.LoopMode = GetLoopModeFromDropdown();
            CurrentSample.IsAtonal = chkAtonal.Checked;
            CurrentSample.IsPrimaryLoop = chkPrimaryLoop.Checked;

            if (txtSampleStartPosition.Seconds != 0) CurrentSample.Start = txtSampleStartPosition.Seconds;
            if (txtSampleOffsetPosition.Seconds != 0) CurrentSample.Offset = txtSampleOffsetPosition.Seconds;
            if (cmbSampleLength.Seconds != 0) CurrentSample.Length = cmbSampleLength.Seconds;

            CurrentSample.Bpm = CalculateSampleBpm();

            CurrentSample.Tags = txtTags.Text
                .Split(',')
                .Select(x => x.ToLower().Trim())
                .OrderBy(x => x)
                .ToList();
        }

        /// <summary>
        ///     Handles the Click event of the btnOK control.
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveSamples();
        }

        /// <summary>
        ///     Handles the Click event of the btnCancel control.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        ///     Handles the FormClosed event of the frmTrack control.
        /// </summary>
        private void frmSamples_FormClosed(object sender, FormClosedEventArgs e)
        {
            trackWave.Unload();
        }

        /// <summary>
        ///     Handles the TextChanged event of the trackWave control.
        /// </summary>
        private void trackWave_PositionsChanged(object sender, EventArgs e)
        {
            BindData();
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
            trackWave.Zoom(CurrentSample.Start, CurrentSample.Length, CurrentSample.Offset);
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

        /// <summary>
        ///     Handles the Load event of the frmSamples control.
        /// </summary>
        private void frmSamples_Load(object sender, EventArgs e)
        {
            Initialise();
        }

        /// <summary>
        ///     Handles the SelectedIndexChanged event of the cmbOutput control.
        /// </summary>
        private void cmbOutput_SelectedIndexChanged(object sender, EventArgs e)
        {
            var outputType = cmbOutput.ParseEnum<SoundOutput>();
            BassPlayer.RawLoopOutput = outputType;
        }

        private void btnCloneSample_Click(object sender, EventArgs e)
        {
            if (CurrentSample == null) return;

            var description = CurrentSample.Description + " Cloned";

            AddSample(description, CurrentSample.Start, CurrentSample.Length);
        }
    }
}