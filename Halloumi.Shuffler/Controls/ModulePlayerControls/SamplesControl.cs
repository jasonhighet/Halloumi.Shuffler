﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Halloumi.Shuffler.AudioEngine.ModulePlayer;
using Halloumi.Shuffler.AudioEngine.Players;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Channels;
using Halloumi.Shuffler.Forms;

namespace Halloumi.Shuffler.Controls.ModulePlayerControls
{
    public partial class SamplesControl : ModulePlayerControl
    {
        private readonly Font _font = new Font("Segoe UI", 9, GraphicsUnit.Point);

       // private bool _binding;
        private List<SampleModel> SampleModels { get; set; }
        private  AudioPlayer _player;

        public SamplesControl()
        {
            InitializeComponent();

            grdSamples.SelectionChanged += grdSamples_SelectionChanged;
            grdSamples.SortOrderChanged += grdSamples_SortOrderChanged;
            grdSamples.CellValueNeeded += grdSamples_CellValueNeeded;
            grdSamples.SortColumnIndex = -1;

            grdSamples.DefaultCellStyle.Font = _font;



        }

        public void Initialize()
        {
            var speakers = new SpeakerOutputChannel();
            _player = new AudioPlayer();
            speakers.AddInputChannel(_player.Output);

            BindData();
        }

        public void Close()
        {
            StopCurrentSample();
        }

        private void StopCurrentSample()
        {
            lock (_player)
            {
                _player.Pause();
                _player.UnloadAll();
            }
        }

        public void BindData()
        {
         //   _binding = true;

            grdSamples.SaveSelectedRows();

            var sampleModels = GetDisplayedSampleModels();

            if (grdSamples.SortedColumn != null)
            {
                var sortField = grdSamples.SortedColumn.DataPropertyName;
                if (sortField == "Description") sampleModels = sampleModels.OrderBy(t => t.Description).ToList();
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

          //  _binding = false;
        }

        private List<SampleModel> GetDisplayedSampleModels()
        {
            var sampleModels = new List<SampleModel>();
            foreach (var audioFile in ModulePlayer.Module.AudioFiles)
            {
                foreach (var sample in audioFile.Samples)
                {
                    sampleModels.Add(new SampleModel(sample, audioFile));
                }
            }

            return sampleModels;
        }

        private void PlayCurrentSamples()
        {
            var sampleModels = GetSelectedSampleModels();
            if (sampleModels == null || sampleModels.Count == 0) return;

            lock (_player)
            {
                _player.Pause();
                _player.UnloadAll();

                var loopLength = BpmHelper.GetDefaultLoopLength(ModulePlayer.Module.Bpm);
                _player.Load("Silence", SilenceHelper.GetSilenceAudioFile());
                _player.AddSection("Silence", "Silence", 0, loopLength, bpm: ModulePlayer.Module.Bpm);
                _player.QueueSection("Silence", "Silence");

                var filenames = new List<string>();
                foreach (var sampleModel in sampleModels)
                {
                    _player.Load(sampleModel.Description, sampleModel.AudioFile.Path);
                    _player.AddSection(sampleModel.Description,
                        sampleModel.Description,
                        sampleModel.Sample.Start,
                        sampleModel.Sample.Length,
                        sampleModel.Sample.Offset.HasValue ? sampleModel.Sample.Offset.Value : 0,
                        calculateBpmFromLength:true,
                        targetBpm: ModulePlayer.Module.Bpm,
                        loopIndefinitely:true);

                    _player.AddEvent("Silence", 0, sampleModel.Description, sampleModel.Description, EventType.Play);
                }
                _player.Pause();
                _player.Play("Silence");
            }
        }

        private void grdSamples_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex == -1) return;

            var sampleModel = GetSampleModelByIndex(e.RowIndex);

            if (sampleModel == null) e.Value = "";
            else if (e.ColumnIndex == 0) e.Value = sampleModel.Description;
        }

        private void grdSamples_SortOrderChanged(object sender, EventArgs e)
        {
            var bindData = new BindDataHandler(BindData);
            BeginInvoke(bindData);
        }

        private void grdSamples_SelectionChanged(object sender, EventArgs e)
        {
            PlayCurrentSamples();
        }


        private void EditSample()
        {
            //var sample = GetSelectedSample();
            //if (sample == null) return;

            //var track = SampleLibrary.GetTrackFromSample(sample);
            //if (track == null) return;

            //StopCurrentSample();

            //var form = new FrmEditTrackSamples
            //{
            //    BassPlayer = BassPlayer,
            //    Filename = track.Filename,
            //    SampleLibrary = SampleLibrary,
            //    Library = SampleLibrary.TrackLibrary
            //};

            //var result = form.ShowDialog();
            //if (result == DialogResult.OK)
            //{
            //    BindData();
            //}
        }


        /// <summary>
        ///     Gets the selected sample.
        /// </summary>
        /// <returns>The selected sample</returns>
        private SampleModel GetSelectedSampleModel()
        {
            var samples = GetSelectedSampleModels();
            return samples.Count == 0 ? null : samples[0];
        }

        /// <summary>
        ///     Gets the selected samples.
        /// </summary>
        /// <returns>The selected samples</returns>
        private List<SampleModel> GetSelectedSampleModels()
        {
            var samples = new List<SampleModel>();

            for (var i = 0; i < grdSamples.SelectedRows.Count; i++)
            {
                var sample = GetSampleModelByIndex(grdSamples.SelectedRows[i].Index);
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
        private Module.Sample GetSampleByIndex(int index)
        {
            var sampleModel = GetSampleModelByIndex(index);
            return sampleModel?.Sample;
        }

        private delegate void BindDataHandler();

        private class SampleModel
        {
            public SampleModel(Module.Sample sample, Module.AudioFile audioFile)
            {
                Description = audioFile.Key + "." + sample.Key;
                Sample = sample;
                AudioFile = audioFile;
            }

            public string Description { get; }
            public Module.Sample Sample { get; }
            public Module.AudioFile AudioFile { get; }
        }
    }
}
