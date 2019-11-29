using System.Collections.Generic;
using System.IO;
using System.Linq;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioEngine.BassPlayer;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioLibrary.Models;

namespace Halloumi.Shuffler.AudioLibrary.Samples
{
    public class LoopLibrary : ISampleLibrary
    {
        private readonly BassPlayer _bassPlayer;
        private readonly string _folder;
        private readonly List<Sample> _samples;

        public LoopLibrary(BassPlayer bassPlayer, string folder)
        {
            _folder = folder;
            _samples = new List<Sample>();
            _bassPlayer = bassPlayer;

            LoadSamples();
        }

        public void SaveCache()
        {
        }

        public List<Sample> GetSamples()
        {
            return _samples.ToList();
        }

        public List<Sample> GetSamples(SearchCriteria searchCriteria)
        {
            return _samples.ToList();
        }

        public string GetSampleFileName(Sample sample)
        {
            return sample.Filename;
        }

        public void CalculateSampleKey(Sample sample)
        {
        }

        public Track GetTrackFromSample(Sample sample)
        {
            return null;
        }

        private void LoadSamples()
        {
            var files = FileSystemHelper.SearchFiles(_folder, "*.wav", true);

            ParallelHelper.ForEach(files, LoadSample);
            //foreach (var file in files) LoadSample(file);
        }

        private void LoadSample(string file)
        {
            var lengthInSeconds = AudioStreamHelper.GetLength(file);
            var bpm = BpmHelper.GetBpmFromLoopLength(lengthInSeconds);

            var sample = new Sample
            {
                Filename = file,
                Description = (Path.GetFileNameWithoutExtension(file) + "").Replace(_folder, "").Replace("\\", ""),
                IsAtonal = true,
                IsPrimaryLoop = true,
                Gain = 0,
                LoopMode = LoopMode.FullLoop,
                Length = lengthInSeconds,
                TrackLength = (decimal) lengthInSeconds,
                Offset = 0,
                Start = 0,
                Key = "",
                Bpm = bpm,
                Tags = new List<string>(),
                TrackArtist = (Path.GetDirectoryName(file) + "").Replace(_folder, "").Replace("\\", ""),
                TrackTitle = (Path.GetFileNameWithoutExtension(file) + "").Replace(_folder, "").Replace("\\", ""),
            };

            lock (_samples)
            {
                _samples.Add(sample);
            }
        }
    }
}