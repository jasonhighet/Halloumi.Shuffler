using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Helpers;
using Halloumi.Shuffler.AudioEngine.Channels;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.ModulePlayer;
using Halloumi.Shuffler.AudioEngine.Players;
using Halloumi.Shuffler.AudioLibrary.Models;
using Halloumi.Shuffler.Forms;
using Halloumi.Common.Windows.Controls;
using System.ComponentModel;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.AudioEngine;

namespace Halloumi.Shuffler.Controls.ModulePlayerControls
{
    public partial class SamplesControl : BaseUserControl, ISampleRecipient
    {
        /// <summary>
        ///     Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ModulePlayer ModulePlayer { get; set; }

        /// <summary>
        ///     Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SampleLibrary SampleLibrary { get; set; }

        /// <summary>
        ///     Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BassPlayer BassPlayer { get; set; }

        /// <summary>
        ///     Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Library Library { get; set; }

        private readonly Font _font = new Font("Segoe UI", 9, GraphicsUnit.Point);
        private AudioPlayer _player;
        private FrmSampleLibrary _frmSampleLibrary;
        private bool _binding;

        public SamplesControl()
        {
            InitializeComponent();

            grdSamples.SelectionChanged += grdSamples_SelectionChanged;
            grdSamples.SortOrderChanged += grdSamples_SortOrderChanged;
            grdSamples.CellValueNeeded += grdSamples_CellValueNeeded;
            grdSamples.SortColumnIndex = -1;

            grdSamples.DefaultCellStyle.Font = _font;
        }
        
        private List<SampleModel> SampleModels { get; set; }

        public void Initialize()
        {
            _player = new AudioPlayer();
            BassPlayer.SpeakerOutput.AddInputChannel(_player.Output);

            BindData();
        }

        public void Close()
        {
            StopCurrentSample();
        }

        public void ImportLibrarySamples(List<Sample> librarySamples)
        {
            var audioFiles = new List<Module.AudioFile>();

            foreach (var librarySample in librarySamples)
            {
                var track = SampleLibrary.GetTrackFromSample(librarySample);
                if (track == null) continue;

                var audioFile = audioFiles.FirstOrDefault(x => x.Path == track.Filename);
                if (audioFile == null)
                {
                    audioFile = GetAudioFileFromModule(track.Filename) ?? GetNewAudioFile(track);
                    audioFiles.Add(audioFile);
                }

                var sample = GetNewModuleSample(librarySample);
                audioFile.Samples.RemoveAll(x => x.Key == sample.Key);
                audioFile.Samples.Add(sample);
            }

            foreach (var audioFile in audioFiles)
            {
                ModulePlayer.UpdateAudioFile(audioFile);
            }

            BindData();
        }

        private static Module.Sample GetNewModuleSample(Sample librarySample)
        {
            return new Module.Sample
            {
                Start = librarySample.Start,
                Length = librarySample.Length,
                Offset = librarySample.Offset,
                Key = librarySample.Description
            };
        }

        private static Sample GetNewLibarySample(Module.Sample librarySample)
        {
            return new Sample
            {
                Start = librarySample.Start,
                Length = librarySample.Length,
                Offset = librarySample.Offset,
                Description = librarySample.Key
            };
        }


        private Module.AudioFile GetAudioFileFromModule(string path)
        {
            return ModulePlayer.Module.AudioFiles.FirstOrDefault(x => x.Path == path);
        }

        private static Module.AudioFile GetNewAudioFile(Track track)
        {
            return new Module.AudioFile
            {
                Samples = new List<Module.Sample>(),
                Path = track.Filename,
                Key = StringHelper.GetAlphaNumericCharactersOnly(track.Title)
            };
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
            _binding = true;

            grdSamples.SaveSelectedRows();

            var sampleModels = GetSampleModelsFromModule();

            if (grdSamples.SortedColumn != null)
            {
                var sortField = grdSamples.SortedColumn.DataPropertyName;
                if (sortField == "Description") sampleModels = sampleModels.OrderBy(t => t.Description).ToList();
                if (sortField == "BPM") sampleModels = sampleModels.OrderBy(t => t.Bpm).ToList();
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

        private List<SampleModel> GetSampleModelsFromModule()
        {
            var sampleModels = new List<SampleModel>();
            foreach (var audioFile in ModulePlayer.Module.AudioFiles)
            {
                sampleModels.AddRange(audioFile.Samples.Select(sample => new SampleModel(sample, audioFile)));
            }

            return sampleModels;
        }

        private void PlaySamples()
        {
            var sampleModels = GetSelectedSampleModels();
            if (sampleModels == null || sampleModels.Count == 0) return;

            lock (_player)
            {
                _player.Pause();
                _player.UnloadAll();

                const int loopCount = 1;

                var loopLength = BpmHelper.GetDefaultLoopLength(ModulePlayer.Module.Bpm) * loopCount;
                _player.Load("Loop", SilenceHelper.GetSilenceAudioFile());
                var section = _player.AddSection("Loop", "Loop", 0, loopLength, bpm: ModulePlayer.Module.Bpm);
                section.LoopIndefinitely = true;
                _player.QueueSection("Loop", "Loop");

                foreach (var sampleModel in sampleModels)
                {
                    _player.Load(sampleModel.Description, sampleModel.AudioFile.Path);
                    _player.AddSection(sampleModel.Description,
                        sampleModel.Description,
                        sampleModel.Sample.Start,
                        sampleModel.Sample.Length,
                        sampleModel.Sample.Offset,
                        calculateBpmFromLength: true,
                        targetBpm: ModulePlayer.Module.Bpm,
                        loopIndefinitely: true);

                    var adjustedLength = BpmHelper.GetAdjustedAudioLength(sampleModel.Sample.Length, sampleModel.Bpm, ModulePlayer.Module.Bpm);
                    var position = 0D;
                    while(position < loopLength)
                    {
                        _player.AddEvent("Loop", position, sampleModel.Description, sampleModel.Description, EventType.PlaySolo);
                        _player.AddEvent("Loop", position + adjustedLength, sampleModel.Description, sampleModel.Description, EventType.Pause);
                        position += adjustedLength;
                    }
                }
                _player.Pause();
                _player.Play("Loop");
            }
        }

        private double GetAdjustedAudioLength(double length, decimal sourceBpm, decimal targetBpm)
        {
            var sampleLoopLength = BpmHelper.GetDefaultLoopLength(sourceBpm);
            var samplesPerLoop = Math.Round(sampleLoopLength / length, 0);
            return BpmHelper.GetDefaultLoopLength(targetBpm) / samplesPerLoop;
        }

        private void grdSamples_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex == -1) return;

            var sampleModel = GetSampleModelByIndex(e.RowIndex);

            if (sampleModel == null) e.Value = "";
            else if (e.ColumnIndex == 0)
            {
                e.Value = sampleModel.Description;
            }
            else if (e.ColumnIndex == 1)
            {
                e.Value = sampleModel.Bpm.ToString("0.00");
            }
        }

        private void grdSamples_SortOrderChanged(object sender, EventArgs e)
        {
            var bindData = new BindDataHandler(BindData);
            BeginInvoke(bindData);
        }

        private void grdSamples_SelectionChanged(object sender, EventArgs e)
        {
            if(_binding)
                return;
            PlaySamples();
        }


        private void EditSample()
        {
            var sampleModel = GetSelectedSampleModel();
            if (sampleModel == null) return;

            PauseSamples();

            var audioFile = sampleModel.AudioFile;

            var samples = audioFile.Samples.Select(GetNewLibarySample).ToList();

            var initialSample = sampleModel.Sample.Key;

            var form = new FrmEditTrackSamples
            {
                BassPlayer = BassPlayer,
                Filename = audioFile.Path,
                SampleLibrary = SampleLibrary,
                Library = Library,
                Samples = samples,
                InitialSample = initialSample
            };

            if (form.ShowDialog() != DialogResult.OK) return;

            var newSamples = form.Samples.Select(GetNewModuleSample).ToList();

            audioFile.Samples = newSamples;

            ModulePlayer.UpdateAudioFile(audioFile);

            BindData();
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditSample();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            PlaySamples();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            PauseSamples();
        }

        public void PauseSamples()
        {
            lock (_player)
            {
                _player.Pause();
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var destinationFolder = FileDialogHelper.OpenFolder();
            foreach (var sampleModel in GetSelectedSampleModels())
            {
                var exportFileName = Path.Combine(destinationFolder, sampleModel.Description + ".wav");
                AudioExportHelper.SavePartialAsWave(sampleModel.AudioFile.Path,
                    exportFileName,
                    sampleModel.Sample.Start,
                    sampleModel.Sample.Length,
                    sampleModel.Sample.Offset,
                    0,
                    sampleModel.Bpm,
                    ModulePlayer.Module.Bpm);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (_frmSampleLibrary == null || _frmSampleLibrary.IsDisposed)
            {
                _frmSampleLibrary = new FrmSampleLibrary();
                _frmSampleLibrary.Initialize(BassPlayer, SampleLibrary, this);
            }

            if (!_frmSampleLibrary.Visible)
            {
                WindowHelper.ShowDialog(ParentForm, _frmSampleLibrary);
            }
        }

        private delegate void BindDataHandler();

        private class SampleModel
        {
            public SampleModel(Module.Sample sample, Module.AudioFile audioFile)
            {
                Description = audioFile.Key + "." + sample.Key;
                Sample = sample;
                AudioFile = audioFile;
                Bpm = BpmHelper.GetBpmFromLoopLength(sample.Length);
            }

            public decimal Bpm { get; }
            public string Description { get; }
            public Module.Sample Sample { get; }
            public Module.AudioFile AudioFile { get; }
        }
    }
}