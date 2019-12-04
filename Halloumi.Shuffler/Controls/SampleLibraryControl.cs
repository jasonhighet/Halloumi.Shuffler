using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.AudioEngine.BassPlayer;
using Halloumi.Shuffler.AudioEngine.Channels;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Players;
using Halloumi.Shuffler.AudioLibrary.Samples;
using Halloumi.Shuffler.Forms;

namespace Halloumi.Shuffler.Controls
{
    public partial class SampleLibraryControl : UserControl
    {
        private readonly Font _font = new Font("Segoe UI", 9, GraphicsUnit.Point);
        private bool _binding;
        private bool _playing;
        private SyncedSamplePlayer _player;

        private string _playingSamples;
        private bool _bindingVolumeSlider;

        public SampleLibraryControl()
        {
            InitializeComponent();

            MinBpm = 0;
            MaxBpm = 1000;
            grdSamples.VirtualMode = true;

            txtMinBPM.TextChanged += txtMinBPM_TextChanged;
            txtMaxBPM.TextChanged += txtMaxBPM_TextChanged;

            txtSearch.KeyPress += txtSearch_KeyPress;

            grdSamples.SelectionChanged += grdSamples_SelectionChanged;
            grdSamples.SortOrderChanged += grdSamples_SortOrderChanged;
            grdSamples.CellValueNeeded += grdSamples_CellValueNeeded;
            grdSamples.SortColumnIndex = -1;

            SearchFilter = "";

            grdSamples.DefaultCellStyle.Font = _font;

            cmbKey.Items.Clear();
            cmbKey.Items.Add("");
            cmbKey.Items.Add("Atonal");
            foreach (var key in KeyHelper.GetDisplayKeys())
                cmbKey.Items.Add(key);

            cmbLoopType.Items.Clear();
            cmbLoopType.Items.Add("");
            cmbLoopType.Items.Add("Primary Loop");
            cmbLoopType.Items.Add("Loop");
            cmbLoopType.Items.Add("Partial Loop (End)");
            cmbLoopType.Items.Add("Partial Loop (Start)");

            LoopFilter = "";
            KeyFilter = "";
        }

        private string SearchFilter { get; set; }

        private int MinBpm { get; set; }

        private int MaxBpm { get; set; }

        private string KeyFilter { get; set; }

        private bool IncludeAntonalFilter { get; set; }

        private string LoopFilter { get; set; }

        /// <summary>
        ///     Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISampleLibrary SampleLibrary { get; set; }

        /// <summary>
        ///     Gets or sets the bass player.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BassPlayer BassPlayer { get; set; }

        private List<SampleModel> SampleModels { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISampleRecipient SampleRecipient { get; set; }
        public bool AutoPlaySamples { get; private set; }

        public void Initialize()
        {
            _player = new SyncedSamplePlayer(BassPlayer.SpeakerOutput, BassPlayer.MonitorOutput)
            {
                SoundOutput = SoundOutput.Speakers
            };

            cmbOutput.SelectedIndex = 0;

            var settings = Settings.Default;
            _player.SetVolume(settings.LoopVolume);
            SetVolume((int)settings.LoopVolume);

            sldVolume.Minimum = 0;
            sldVolume.Maximum = 100;
            var volume = Convert.ToInt32(_player.GetVolume());
            lblVolume.Text = volume.ToString();
            sldVolume.Value = volume;
            sldVolume.Scrolled += SldVolume_Slid;

            this.AutoPlaySamples = false;

            BindData();
        }

        private void SldVolume_Slid(object sender, EventArgs e)
        {
            _bindingVolumeSlider = true;
            var volume = Convert.ToDecimal(sldVolume.ScrollValue);
            _player.SetVolume(volume);
            lblVolume.Text = volume.ToString(CultureInfo.InvariantCulture);
            _bindingVolumeSlider = false;
        }

        private void SetVolume(int volume)
        {
            sldVolume.Value = volume;
            lblVolume.Text = volume.ToString();
            var settings = Settings.Default;
            settings.LoopVolume = volume;
        }

        public void Close()
        {
            StopSamples();
        }

        private void StopSamples()
        {
            lock (_player)
            {
                _playing = false;
                //_player.Pause();
                _player.Mute();
            }
        }

        public List<Sample> GetDisplayedSamples()
        {
            var criteria = new SearchCriteria();

            var loopTypeText = LoopFilter;
            if (loopTypeText.Contains("Primary"))
            {
                criteria.Primary = true;
                criteria.LoopMode = LoopMode.FullLoop;
            }
            else if (loopTypeText != "")
            {
                criteria.Primary = false;
                if (loopTypeText.Contains("Start"))
                    criteria.LoopMode = LoopMode.PartialLoopAnchorStart;
                else if (loopTypeText.Contains("End"))
                    criteria.LoopMode = LoopMode.PartialLoopAnchorEnd;
                else if (loopTypeText == "Loop")
                    criteria.LoopMode = LoopMode.FullLoop;
            }

            var keyText = KeyFilter;
            if (keyText == "Atonal")
            {
                criteria.AtonalOnly = true;
                criteria.Key = "";
            }
            else
            {
                criteria.Key = KeyHelper.GetKeyFromDisplayKey(KeyFilter);
                criteria.AtonalOnly = false;
            }

            criteria.IncludeAtonal = IncludeAntonalFilter;

            criteria.MaxBpm = MaxBpm;
            criteria.MinBpm = MinBpm;

            criteria.SearchText = SearchFilter;
            var samples = SampleLibrary.GetSamples(criteria);

            return samples;
        }

        /// <summary>
        ///     Binds the data.
        /// </summary>
        private void BindData()
        {
            if (_binding) return;
            BindSamples();
        }

        /// <summary>
        ///     Binds the samples.
        /// </summary>
        private void BindSamples()
        {
            _binding = true;

            lock (_player)
            {
                _player.Pause();
            }


            grdSamples.SaveSelectedRows();

            var sampleModels = GetDisplayedSamples()
                .Take(2000)
                .Select(t => new SampleModel(t))
                .ToList();

            if (grdSamples.SortedColumn != null)
            {
                var sortField = grdSamples.SortedColumn.DataPropertyName;
                if (sortField == "Description") sampleModels = sampleModels.OrderBy(t => t.Description).ToList();
                if (sortField == "LengthFormatted") sampleModels = sampleModels.OrderBy(t => t.Length).ToList();
                if (sortField == "Tags") sampleModels = sampleModels.OrderBy(t => t.Tags).ToList();
                if (sortField == "BPM") sampleModels = sampleModels.OrderBy(t => t.Bpm).ToList();
                if (sortField == "Key") sampleModels = sampleModels.OrderByDescending(t => t.Key).ToList();
                if (grdSamples.SortOrder == SortOrder.Descending) sampleModels.Reverse();
            }
            SampleModels = sampleModels;

            if (sampleModels.Count != grdSamples.RowCount)
            {
                grdSamples.RowCount = 0;
                grdSamples.RowCount = sampleModels.Count;
            }

            grdSamples.RestoreSelectedRows();
            grdSamples.InvalidateDisplayedRows();

            _binding = false;
        }

        private void grdSamples_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex == -1) return;

            var sampleModel = GetSampleModelByIndex(e.RowIndex);

            if (sampleModel == null) e.Value = "";
            else if (e.ColumnIndex == 0) e.Value = sampleModel.Description;
            else if (e.ColumnIndex == 1) e.Value = sampleModel.Bpm;
            else if (e.ColumnIndex == 2) e.Value = sampleModel.Key;
            else if (e.ColumnIndex == 3) e.Value = sampleModel.LengthFormatted;
            else if (e.ColumnIndex == 4) e.Value = sampleModel.Tags;
        }

        /// <summary>
        ///     Gets the selected sample.
        /// </summary>
        /// <returns>The selected sample</returns>
        public Sample GetSelectedSample()
        {
            var samples = GetSelectedSamples();
            return samples.Count == 0 ? null : samples[0];
        }

        /// <summary>
        ///     Gets the selected samples.
        /// </summary>
        /// <returns>The selected samples</returns>
        private List<Sample> GetSelectedSamples()
        {
            var samples = new List<Sample>();

            for (var i = 0; i < grdSamples.SelectedRows.Count; i++)
            {
                var sample = GetSampleByIndex(grdSamples.SelectedRows[i].Index);
                if (sample != null) samples.Add(sample);
            }
            return samples;
        }

        /// <summary>
        ///     Gets the index of the sample model by.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private SampleModel GetSampleModelByIndex(int index)
        {
            if (SampleModels == null) return null;
            if (index < 0 || index >= SampleModels.Count) return null;
            return SampleModels[index];
        }

        /// <summary>
        ///     Gets a sample by its index
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The sample at the index</returns>
        private Sample GetSampleByIndex(int index)
        {
            var sampleModel = GetSampleModelByIndex(index);
            return sampleModel?.Sample;
        }

        /// <summary>
        ///     Sets the search filter and performs a search
        /// </summary>
        private void Search()
        {
            if (SearchFilter == txtSearch.Text.Trim()) return;

            SearchFilter = txtSearch.Text.Trim();
            BindData();
        }

        /// <summary>
        ///     Sets the loop filter.
        /// </summary>
        private void SetLoopFilter()
        {
            if (LoopFilter == cmbLoopType.GetTextThreadSafe())
                return;

            LoopFilter = cmbLoopType.GetTextThreadSafe();
            BindData();
        }

        /// <summary>
        ///     Sets the loop filter.
        /// </summary>
        private void SetKeyFilter()
        {
            if (KeyFilter == cmbKey.GetTextThreadSafe())
                return;

            KeyFilter = cmbKey.GetTextThreadSafe();
            BindData();
        }

        /// <summary>
        ///     Sets the loop filter.
        /// </summary>
        private void SetIncludeAtonalFilter()
        {
            if (IncludeAntonalFilter == chkIncludeAntonal.Checked)
                return;

            IncludeAntonalFilter = chkIncludeAntonal.Checked;
            BindData();
        }

        private void SetBpmFilter()
        {
            var min = 0;
            if (txtMinBPM.Text != "") min = Convert.ToInt32(txtMinBPM.Text);

            var max = 1000;
            if (txtMaxBPM.Text != "") max = Convert.ToInt32(txtMaxBPM.Text);

            if (min == MinBpm && max == MaxBpm) return;

            MinBpm = min;
            MaxBpm = max;
            BindData();
        }

        private void PlayCurrentSamples()
        {
            if (_playing) return;
            lock (_player)
            {
                _playing = true;
                _player.SetBpm(BassPlayer.GetCurrentBpm());
                _player.Unmute();
                _player.Play();
            }
        }

        private void LoadCurrentSamples()
        {
            lock (_player)
            {
                _player.Pause();
                var samples = GetSelectedSamples();
                if (samples == null || samples.Count == 0) return;

                _player.UnloadAll();
                foreach (var sample in samples)
                {
                    var track = SampleLibrary.GetTrackFromSample(sample);

                    var filename = (track == null) ? sample.Filename : track.Filename;
                    if (!File.Exists(filename))
                        continue;

                    _player.AddSample(GetSampleKey(sample),
                        filename,
                        sample.Start,
                        sample.Length,
                        sample.Offset);
                }
            }
        }

        private string GetSampleKey(Sample sample)
        {
            var track = SampleLibrary.GetTrackFromSample(sample);
            return (track == null) ? sample.Filename : track.Description + " - " + sample.Description;
        }

        /// <summary>
        ///     Handles the KeyPress event of the txtSearch control.
        /// </summary>
        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            Search();
        }

        /// <summary>
        ///     Handles the SortOrderChanged event of the grdSamples control.
        /// </summary>
        private void grdSamples_SortOrderChanged(object sender, EventArgs e)
        {
            BindData();
        }

        /// <summary>
        ///     Handles the SelectionChanged event of the grdSamples control.
        /// </summary>
        private void grdSamples_SelectionChanged(object sender, EventArgs e)
        {
            var playingSamples = string.Join(",",
                GetSelectedSamples().Select(x => x.TrackArtist + x.TrackTitle + x.Description));

            if (playingSamples != _playingSamples)
            {
                LoadCurrentSamples();
                if(this.AutoPlaySamples)
                    PlayCurrentSamples();
            }

            _playingSamples = playingSamples;
        }

        /// <summary>
        ///     Handles the TextChanged event of the txtMaxBPM control.
        /// </summary>
        private void txtMaxBPM_TextChanged(object sender, EventArgs e)
        {
            SetBpmFilter();
        }

        /// <summary>
        ///     Handles the TextChanged event of the txtMinBPM control.
        /// </summary>
        private void txtMinBPM_TextChanged(object sender, EventArgs e)
        {
            SetBpmFilter();
        }

        private void cmbKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetKeyFilter();
        }

        private void cmbLoopType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetLoopFilter();
        }

        private void mnEditSample_Click(object sender, EventArgs e)
        {
            EditSample();
        }

        private void EditSample()
        {
            var trackSampleLibrary = (TrackSampleLibrary)SampleLibrary;
            if (trackSampleLibrary == null)
                return;

            var sample = GetSelectedSample();
            if (sample == null) return;

            var track = SampleLibrary.GetTrackFromSample(sample);
            if (track == null) return;

            StopSamples();

            var initialSample = sample.Description;


            var form = new FrmEditTrackSamples
            {
                BassPlayer = BassPlayer,
                Filename = track.Filename,
                TrackSampleLibrary = trackSampleLibrary,
                Library = trackSampleLibrary.TrackLibrary,
                InitialSample = initialSample
            };

            var result = form.ShowDialog();
            if (result == DialogResult.OK)
                BindData();

            form.Dispose();
        }

        private void mnuEditTags_Click(object sender, EventArgs e)
        {
        }

        private void mnuCalculateKey_Click(object sender, EventArgs e)
        {
            StopSamples();

            foreach (var sample in GetSelectedSamples().Where(sample => sample.Key == ""))
                SampleLibrary.CalculateSampleKey(sample);

            SampleLibrary.SaveCache();

            BindData();
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            var multiSamplesSelected = GetSelectedSamples().Count > 1;

            mnEditSample.Visible = !multiSamplesSelected;
            mnuCalculateKey.Visible = !multiSamplesSelected;

            mnuImportSamples.Visible = SampleRecipient != null;
        }

        private void mnuCopySample_Click(object sender, EventArgs e)
        {
            var samples = GetSelectedSamples();
            if (samples == null) return;

            var folder = FileDialogHelper.OpenFolder();
            if (folder == "") return;

            foreach (var sample in samples)
            {
                var source = SampleLibrary.GetSampleFileName(sample);
                var filename = $"{sample.TrackArtist} - {sample.TrackTitle} - {sample.Description}";
                filename = FileSystemHelper.StripInvalidFileNameChars(filename) + Path.GetExtension(source);
                var destination = Path.Combine(folder, filename);
                FileSystemHelper.Copy(source, destination);
            }
        }

        private void chkIncludeAntonal_CheckedChanged(object sender, EventArgs e)
        {
            SetIncludeAtonalFilter();
        }

        private void mnuImportSamples_Click(object sender, EventArgs e)
        {
            SampleRecipient?.ImportLibrarySamples(GetSelectedSamples());
        }



        private void mnuExportSamples_Click(object sender, EventArgs e)
        {
            var samples = GetSelectedSamples();
            if (samples == null) return;
            ExportSamples(samples);
        }

        private void ExportSamples(List<Sample> samples)
        {
            StopSamples();

            Cursor = Cursors.WaitCursor;


            var folder = FileDialogHelper.OpenFolder();
            if (folder == "") return;

            ParallelHelper.ForEach(samples, sample =>
            {
                var track = SampleLibrary.GetTrackFromSample(sample);
                if (track == null)
                    return;

                var outPath = Path.Combine(folder, track.Genre);

                if (!Directory.Exists(outPath))
                    Directory.CreateDirectory(outPath);

                var outFile = FileSystemHelper.StripInvalidFileNameChars($"{sample.Bpm:000.00} - {sample.TrackArtist} - {sample.TrackTitle} - {sample.Description}.wav");

                outPath = Path.Combine(outPath, outFile);

                if (File.Exists(outPath))
                    File.Delete(outPath);

                AudioExportHelper.SavePartialAsWave(track.Filename, outPath, sample.Start, sample.Length, sample.Offset,
                    sample.Gain);
            });

            Cursor = Cursors.Default;
        }

        private class SampleModel
        {
            public SampleModel(Sample sample)
            {
                Description = (sample.TrackTitle == sample.Description)
                    ? sample.TrackArtist + " - " + sample.Description
                    : sample.TrackArtist + " - " + sample.TrackTitle + " - " + sample.Description;
                Tags = string.Join(", ", sample.Tags.ToArray());
                LengthFormatted = TimeFormatHelper.GetFormattedHours(Convert.ToDecimal(sample.Length));
                Length = Convert.ToDecimal(sample.Length);
                Bpm = sample.Bpm;
                Sample = sample;
                Key = sample.IsAtonal
                    ? "Atonal"
                    : KeyHelper.GetDisplayKey(sample.Key);
            }

            public string Description { get; }

            public string Tags { get; }

            public string LengthFormatted { get; }

            public decimal Length { get; }

            public decimal Bpm { get; }

            public string Key { get; }

            public Sample Sample { get; }
        }

        private void mnuExportAllSamples_Click(object sender, EventArgs e)
        {
            ExportSamples(SampleLibrary.GetSamples());
        }

        private void btnPlay_MouseDown(object sender, MouseEventArgs e)
        {
            PlayCurrentSamples();
        }

        private void btnPlay_MouseUp(object sender, MouseEventArgs e)
        {
            StopSamples();
        }

        private void grdSamples_KeyDown(object sender, KeyEventArgs e)
        {
            if (AutoPlaySamples)
                return;

            if(e.KeyCode != Keys.Space)
                return;

            PlayCurrentSamples();
        }

        private void grdSamples_KeyUp(object sender, KeyEventArgs e)
        {
            if (AutoPlaySamples)
                return;

            if (e.KeyCode != Keys.Space)
                return;

            StopSamples();

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtMinBPM.Text = (BassPlayer.GetCurrentBpm() * 0.9M).ToString("0");
            txtMaxBPM.Text = (BassPlayer.GetCurrentBpm() * 1.1M).ToString("0");
            BindData();
        }

        private void CmbOutput_SelectedIndexChanged(object sender, EventArgs e)
        {
            var outputType = cmbOutput.ParseEnum<SoundOutput>();
            _player.SoundOutput = outputType;
        }
    }

    public interface ISampleRecipient
    {
        void ImportLibrarySamples(List<Sample> librarySamples);
    }
}