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
        private string _folder;
        private readonly List<Sample> _samples;

        public LoopLibrary(BassPlayer bassPlayer)
        {
            _samples = new List<Sample>();
        }

        public void Initialize(string folder)
        {
            _folder = folder;
        }

        public List<Sample> GetSamples()
        {
            lock (_samples)
            {
                return _samples.OrderByDescending(x => x.Bpm).ToList();
            }
        }

        public List<Sample> GetSamples(SearchCriteria searchCriteria)
        {
            if (searchCriteria.MaxBpm == 0) searchCriteria.MaxBpm = 200;

            List<Sample> samples;
            lock (_samples)
            {
                samples = _samples
                    .Where(s => string.IsNullOrEmpty(searchCriteria.TrackArtist) || s.Description == searchCriteria.TrackArtist)
                    .Where(s => string.IsNullOrEmpty(searchCriteria.TrackTitle) || s.TrackTitle == searchCriteria.TrackTitle)
                    .Where(s => string.IsNullOrEmpty(searchCriteria.Description) || s.Description == searchCriteria.Description)
                    .Where(
                        s =>
                            string.IsNullOrEmpty(searchCriteria.Key) || s.Key == searchCriteria.Key && !s.IsAtonal ||
                            searchCriteria.IncludeAtonal && s.IsAtonal)
                    .Where(s => !searchCriteria.AtonalOnly || searchCriteria.AtonalOnly && s.IsAtonal)
                    .Where(s => !searchCriteria.Primary.HasValue || s.IsPrimaryLoop == searchCriteria.Primary.Value)
                    .Where(s => !searchCriteria.LoopMode.HasValue || s.LoopMode == searchCriteria.LoopMode)
                    .Where(s => s.Bpm >= searchCriteria.MinBpm && s.Bpm <= searchCriteria.MaxBpm)
                    .OrderBy(s => s.Description)
                    .ToList();
            }

            if (string.IsNullOrEmpty(searchCriteria.SearchText))
                return samples.OrderBy(x => x.Bpm).ToList();

            searchCriteria.SearchText = searchCriteria.SearchText.ToLower().Trim();
            samples = samples.Where(s => s.Tags.Contains(searchCriteria.SearchText)
                                         || s.Description.ToLower().Contains(searchCriteria.SearchText)
                                         || s.TrackArtist.ToLower().Contains(searchCriteria.SearchText)
                                         || s.TrackTitle.ToLower().Contains(searchCriteria.SearchText))
                .ToList();

            return samples.OrderBy(x => x.Bpm).ToList();
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

        public void LoadFromFiles()
        {
            lock (_samples)
            {
                _samples.Clear();
            }
            var files = FileSystemHelper.SearchFiles(_folder, "*.wav", true);
            ParallelHelper.ForEach(files, LoadSample);
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

        public void LoadFromCache()
        {
            if (!File.Exists(LibraryCacheFilename)) return;
            try
            {
                var samples = SerializationHelper<List<Sample>>.FromXmlFile(LibraryCacheFilename);
                lock (_samples)
                {
                    _samples.Clear();
                    _samples.AddRange(samples.ToArray());
                }
            }
            catch
            {
                // ignored
            }
        }


        /// <summary>
        ///     Saves the track details to a cache file
        /// </summary>
        public void SaveToCache()
        {
            lock (_samples)
            {
                SerializationHelper<List<Sample>>.ToXmlFile(_samples, LibraryCacheFilename);
            }
        }


        /// <summary>
        ///     Gets the name of the file where the track data is cached.
        /// </summary>
        private static string LibraryCacheFilename
        {
            get { return Path.Combine(ApplicationHelper.GetUserDataPath(), "Halloumi.Shuffler.LoopLibrary.xml"); }
        }
    }
}