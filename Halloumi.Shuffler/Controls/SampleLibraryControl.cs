using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Halloumi.Shuffler.Engine;
using Halloumi.Shuffler.Forms;
using BE = Halloumi.BassEngine;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Common.Helpers;
using System.IO;
using Halloumi.BassEngine.Helpers;
using Halloumi.Shuffler.Engine.Models;

namespace Halloumi.Shuffler.Controls
{
    public partial class SampleLibraryControl : UserControl
    {
        private bool _binding = false;

        private string SearchFilter { get; set; }

        private int MinBpm { get; set; }

        private int MaxBpm { get; set; }

        private string KeyFilter { get; set; }

        private string LoopFilter { get; set; }

        private class SampleModel
        {
            public string Description { get; set; }

            public string Artist { get; set; }

            public string Tags { get; set; }

            public string LengthFormatted { get; set; }

            public decimal Length { get; set; }

            public decimal Bpm { get; set; }

            public string Key { get; set; }

            public Sample Sample { get; set; }

            public SampleModel(Sample sample)
            {
                Description = sample.TrackArtist + " - " + sample.TrackTitle + " - " + sample.Description;
                Tags = string.Join(", ", sample.Tags.ToArray());
                LengthFormatted = BassHelper.GetFormattedLength(Convert.ToDecimal(sample.Length));
                Length = Convert.ToDecimal(sample.Length);
                Bpm = sample.Bpm;
                Sample = sample;
                Key = sample.IsAtonal
                    ? "Atonal"
                    : KeyHelper.GetDisplayKey(sample.Key);
            }
        }

        /// <summary>
        /// Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SampleLibrary SampleLibrary { get; set; }

        /// <summary>
        /// Gets or sets the bass player.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BE.BassPlayer BassPlayer { get; set; }

        public SampleLibraryControl()
        {
            InitializeComponent();

            MinBpm = 0;
            MaxBpm = 1000;
            grdSamples.VirtualMode = true;

            txtMinBPM.TextChanged += new EventHandler(txtMinBPM_TextChanged);
            txtMaxBPM.TextChanged += new EventHandler(txtMaxBPM_TextChanged);

            txtSearch.KeyPress += new KeyPressEventHandler(txtSearch_KeyPress);
            //txtSearch.TextChanged += new EventHandler(txtSearch_TextChanged);

            grdSamples.SelectionChanged += new EventHandler(grdSamples_SelectionChanged);
            grdSamples.SortOrderChanged += new EventHandler(grdSamples_SortOrderChanged);
            grdSamples.CellValueNeeded += grdSamples_CellValueNeeded;
            grdSamples.SortColumnIndex = -1;

            SearchFilter = "";

            grdSamples.DefaultCellStyle.Font = _font;

            cmbKey.Items.Clear();
            cmbKey.Items.Add("");
            cmbKey.Items.Add("Atonal");
            foreach (var key in KeyHelper.GetDisplayKeys())
            {
                cmbKey.Items.Add(key);
            }

            cmbLoopType.Items.Clear();
            cmbLoopType.Items.Add("");
            cmbLoopType.Items.Add("Primary Loop");
            cmbLoopType.Items.Add("Loop");
            cmbLoopType.Items.Add("Partial Loop (End)");
            cmbLoopType.Items.Add("Partial Loop (Start)");

            LoopFilter = "";
            KeyFilter = "";
        }

        public void Initialize()
        {
            BindData();
        }

        public void Close()
        {
            StopCurrentSample();
        }

        private void StopCurrentSample()
        {
            BassPlayer.StopRawLoop();
            BassPlayer.UnloadRawLoopTrack();
        }

        public List<Sample> GetDisplayedSamples()
        {
            var criteria = new SampleLibrary.SampleCriteria();

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
                criteria.Atonal = true;
                criteria.Key = "";
            }
            else
            {
                criteria.Atonal = false;
                criteria.Key = KeyHelper.GetKeyFromDisplayKey(KeyFilter);
            }

            criteria.MaxBpm = MaxBpm;
            criteria.MinBpm = MinBpm;

            criteria.SearchText = SearchFilter;
            var samples = SampleLibrary.GetSamples(criteria);

            return samples;
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        private delegate void BindDataHandler();

        /// <summary>
        /// Binds the data.
        /// </summary>
        /// <param name="bindSamples">If set to true, binds the samples.</param>
        private void BindData()
        {
            if (_binding) return;

            var selectedSamples = GetSelectedSamples();

            BindSamples(selectedSamples);
        }

        /// <summary>
        /// Binds the samples.
        /// </summary>
        /// <param name="selectedSamples">The selected samples.</param>
        private void BindSamples(List<Sample> selectedSamples)
        {
            _binding = true;

            var descriptions = new List<string>();

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

        private List<SampleModel> SampleModels { get; set; }

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

        private Font _font = new Font("Segoe UI", 9, GraphicsUnit.Point);

        /// <summary>
        /// Gets the selected sample.
        /// </summary>
        /// <returns>The selected sample</returns>
        public Sample GetSelectedSample()
        {
            var samples = GetSelectedSamples();
            if (samples.Count == 0) return null;
            return samples[0];
        }

        /// <summary>
        /// Gets the selected samples.
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
        /// Gets the index of the sample model by.
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
        /// Gets a sample by its index
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The sample at the index</returns>
        private Sample GetSampleByIndex(int index)
        {
            var sampleModel = GetSampleModelByIndex(index);
            if (sampleModel == null) return null;
            return sampleModel.Sample;
        }

        /// <summary>
        /// Sets the search filter and performs a search
        /// </summary>
        private void Search()
        {
            if (SearchFilter == txtSearch.Text.Trim()) return;
            //if (txtSearch.Text.Trim().Length > 2 || txtSearch.Text.Trim().Length == 0)
            {
                SearchFilter = txtSearch.Text.Trim();
                var bindData = new BindDataHandler(BindData);
                txtSearch.BeginInvoke(bindData);
            }
        }

        /// <summary>
        /// Sets the loop filter.
        /// </summary>
        private void SetLoopFilter()
        {
            if (LoopFilter != cmbLoopType.GetTextThreadSafe())
            {
                LoopFilter = cmbLoopType.GetTextThreadSafe();
                var bindData = new BindDataHandler(BindData);
                BeginInvoke(bindData);
            }
        }

        /// <summary>
        /// Sets the loop filter.
        /// </summary>
        private void SetKeyFilter()
        {
            if (KeyFilter != cmbKey.GetTextThreadSafe())
            {
                KeyFilter = cmbKey.GetTextThreadSafe();
                var bindData = new BindDataHandler(BindData);
                BeginInvoke(bindData);
            }
        }

        private void SetBpmFilter()
        {
            var min = 0;
            if (txtMinBPM.Text != "") min = Convert.ToInt32(txtMinBPM.Text);

            var max = 1000;
            if (txtMaxBPM.Text != "") max = Convert.ToInt32(txtMaxBPM.Text);

            if (min != MinBpm || max != MaxBpm)
            {
                MinBpm = min;
                MaxBpm = max;
                var bindData = new BindDataHandler(BindData);
                BeginInvoke(bindData);
            }
        }

        private void PlayCurrentSample()
        {
            BassPlayer.StopRawLoop();
            BassPlayer.UnloadRawLoopTrack();

            var sample = GetSelectedSample();
            if (sample == null) return;

            var filename = SampleLibrary.GetSampleFileName(sample);
            
            BassPlayer.LoadRawLoopTrack(filename);
            BassPlayer.PlayRawLoop();
        }

        /// <summary>
        /// Handles the TextChanged event of the txtSearch control.
        /// </summary>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            Search();
        }

        /// <summary>
        /// Handles the KeyPress event of the txtSearch control.
        /// </summary>
        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            Search();
        }

        /// <summary>
        /// Handles the SortOrderChanged event of the grdSamples control.
        /// </summary>
        private void grdSamples_SortOrderChanged(object sender, EventArgs e)
        {
            var bindData = new BindDataHandler(BindData);
            BeginInvoke(bindData);
        }

        /// <summary>
        /// Handles the SelectionChanged event of the grdSamples control.
        /// </summary>
        private void grdSamples_SelectionChanged(object sender, EventArgs e)
        {
            PlayCurrentSample();
        }

        /// <summary>
        /// Handles the TextChanged event of the txtMaxBPM control.
        /// </summary>
        private void txtMaxBPM_TextChanged(object sender, EventArgs e)
        {
            SetBpmFilter();
        }

        /// <summary>
        /// Handles the TextChanged event of the txtMinBPM control.
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
            var sample = GetSelectedSample();
            if (sample == null) return;

            var track = SampleLibrary.GetTrackFromSample(sample);
            if (track == null) return;

            StopCurrentSample();

            var form = new FrmEditTrackSamples();
            form.BassPlayer = BassPlayer;
            form.Filename = track.Filename;
            form.SampleLibrary = SampleLibrary;
            form.Library = SampleLibrary.TrackLibrary;

            var result = form.ShowDialog();
            if (result == DialogResult.OK)
            {
                BindData();
            }
        }

        private void mnuEditTags_Click(object sender, EventArgs e)
        {
        }

        private void mnuCalculateKey_Click(object sender, EventArgs e)
        {
            //var sample = GetSelectedSample();
            //if (sample == null) return;

            StopCurrentSample();

            foreach (var sample in GetSelectedSamples())
            {
                if(sample.Key == "")
                    SampleLibrary.CalculateSampleKey(sample);            
            }

            SampleLibrary.SaveCache();

            BindData();

        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            var multiSamplesSelected = (GetSelectedSamples().Count > 1);

            mnEditSample.Visible = !multiSamplesSelected;
            mnuCalculateKey.Visible = !multiSamplesSelected;
        }

        private void mnuCopySample_Click(object sender, EventArgs e)
        {
            var samples = GetSelectedSamples();
            if (samples == null) return;

            var folder = FileDialogHelper.OpenFolder();
            if (folder != "")
            {
                foreach (var sample in samples)
                {
                    var source = SampleLibrary.GetSampleFileName(sample);
                    var destination = Path.Combine(folder, Path.GetFileName(source));
                    FileSystemHelper.Copy(source, destination);
                }
            }
            
        }
    }
}