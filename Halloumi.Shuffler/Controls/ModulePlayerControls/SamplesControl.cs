using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Helpers;
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

        public void AddSamples(List<AudioLibrary.Models.Sample> samples)
        {

            foreach (var sample in samples)
            {
                var track = SampleLibrary.GetTrackFromSample(sample);
                var audioFilePath = track.Filename;
                var audioFile = ModulePlayer.Module.AudioFiles.FirstOrDefault(x => x.Path == audioFilePath);
                if (audioFile == null)
                {
                    audioFile = new Module.AudioFile
                    {
                        Samples = new List<Module.Sample>(),
                        Path = audioFilePath,
                        Key = StringHelper.GetAlphaNumericCharactersOnly(track.Artist)
                            + "." 
                            + StringHelper.GetAlphaNumericCharactersOnly(track.Description)
                    };
                    ModulePlayer.Module.AudioFiles.Add(audioFile);
                }
               // ModulePlayer.UpdateSamples(aud);
            }

            // for each audio file
            //   get list of samples that don't exist already
            //   update audio file, samples

            // bind data
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

          //  _binding = false;
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

                var loopLength = BpmHelper.GetDefaultLoopLength(ModulePlayer.Module.Bpm);
                _player.Load("Loop", SilenceHelper.GetSilenceAudioFile());
                _player.AddSection("Loop", "Loop", 0, loopLength, bpm: ModulePlayer.Module.Bpm);
                _player.QueueSection("Loop", "Loop");

                foreach (var sampleModel in sampleModels)
                {
                    var sampleLoopLength = BpmHelper.GetDefaultLoopLength(sampleModel.Bpm);
                    var samplesPerLoop = Math.Round(sampleLoopLength / sampleModel.Sample.Length, 0); 

                    _player.Load(sampleModel.Description, sampleModel.AudioFile.Path);
                    _player.AddSection(sampleModel.Description,
                        sampleModel.Description,
                        sampleModel.Sample.Start,
                        sampleModel.Sample.Length,
                        sampleModel.Sample.Offset ?? 0,
                        calculateBpmFromLength:true,
                        targetBpm: ModulePlayer.Module.Bpm,
                        loopIndefinitely:true);

                    var sampleStep = BpmHelper.GetDefaultLoopLength(ModulePlayer.Module.Bpm) / samplesPerLoop;
                    var position = 0D;
                    for (var i = 0; i < samplesPerLoop; i++)
                    {
                        _player.AddEvent("Loop", position, sampleModel.Description, sampleModel.Description, EventType.Play);
                        position += sampleStep;
                    }

                    
                }
                _player.Pause();
                _player.Play("Loop");
            }
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
            PlaySamples();
        }


        private void EditSample()
        {
            var sampleModel = GetSelectedSampleModel();
            if (sampleModel == null) return;

            PauseSamples();

            var audioFile = sampleModel.AudioFile;

            var samples = audioFile
                .Samples
                .Select(x => new AudioLibrary.Models.Sample
                {
                    Start = x.Start,
                    Description = x.Key,
                    Length = x.Length,
                    Offset = x.Offset ?? 0
                }).ToList();


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

            var newSamples = form.Samples.Select(x => new Module.Sample
            {
                Start = x.Start,
                Length = x.Length,
                Offset = x.Offset,
                Key = x.Description
            }).ToList();

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

        private void PauseSamples()
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
                    sampleModel.Sample.Offset ?? 0,
                    gain: 0,
                    bpm: sampleModel.Bpm,
                    targetBpm: ModulePlayer.Module.Bpm);

            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {

        }
    }
}
