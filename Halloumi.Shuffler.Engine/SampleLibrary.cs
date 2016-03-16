using System.Collections.Generic;
using System.IO;
using System.Linq;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioLibrary.Models;
using AE = Halloumi.Shuffler.AudioEngine;
using Sample = Halloumi.Shuffler.AudioLibrary.Models.Sample;
using Track = Halloumi.Shuffler.AudioLibrary.Models.Track;

namespace Halloumi.Shuffler.AudioLibrary
{
    public class SampleLibrary
    {
        /// <summary>
        ///     Initializes a new instance of the Library class.
        /// </summary>
        public SampleLibrary(AE.BassPlayer bassPlayer, Library trackLibrary)
        {
            Samples = new List<Sample>();
            BassPlayer = bassPlayer;
            TrackLibrary = trackLibrary;

            SampleLibraryFolder = Path.Combine(Path.GetTempPath(), "SampleLibary");
            if (!Directory.Exists(SampleLibraryFolder))
                Directory.CreateDirectory(SampleLibraryFolder);
        }

        /// <summary>
        ///     Gets or sets the bass player.
        /// </summary>
        private AE.BassPlayer BassPlayer { get; }

        /// <summary>
        ///     Gets or sets the track library.
        /// </summary>
        public Library TrackLibrary { get; }

        /// <summary>
        ///     Gets or sets the samples in the library
        /// </summary>
        private List<Sample> Samples { get; }

        private string SampleLibraryFolder { get; }

        /// <summary>
        ///     Gets the name of the file where the sample data is cached.
        /// </summary>
        private string SampleLibraryFilename
            => Path.Combine(TrackLibrary.ShufflerFolder, "Halloumi.Shuffler.SampleLibrary.xml");

        /// <summary>
        ///     Loads the library from the cache.
        /// </summary>
        public void LoadFromCache()
        {
            if (!File.Exists(SampleLibraryFilename)) return;

            var samples = SerializationHelper<List<Sample>>.FromXmlFile(SampleLibraryFilename);

            lock (Samples)
            {
                Samples.Clear();
                Samples.AddRange(samples.ToArray());
            }
        }

        /// <summary>
        ///     Saves the sample details to a cache file
        /// </summary>
        public void SaveCache()
        {
            SerializationHelper<List<Sample>>.ToXmlFile(Samples, SampleLibraryFilename);
        }

        public void UpdateSampleFromTrack(Sample sample, Track track)
        {
            sample.TrackArtist = track.Artist;
            sample.TrackTitle = track.Title;
            sample.TrackLength = track.FullLength;
            sample.Key = track.Key;

            var genre = track.Genre.ToLower();
            if (!sample.Tags.Contains(genre)) sample.Tags.Add(genre);
        }

        public void UpdateTrackSamples(Track track, List<Sample> trackSamples)
        {
            Samples.RemoveAll(
                x => x.TrackTitle == track.Title && x.TrackArtist == track.Artist && x.TrackLength == track.FullLength);

            trackSamples.ForEach(x => UpdateSampleFromTrack(x, track));

            Samples.AddRange(trackSamples);
        }

        public List<Sample> GetSamples(string trackArtist, string trackTitle, string description)
        {
            List<Sample> samples;
            lock (Samples)
            {
                samples = Samples
                    .Where(s => string.IsNullOrEmpty(trackArtist) || s.TrackArtist == trackArtist)
                    .Where(s => string.IsNullOrEmpty(trackTitle) || s.TrackTitle == trackTitle)
                    .Where(s => string.IsNullOrEmpty(description) || s.Description == description)
                    .OrderBy(s => s.Description)
                    .ToList();
            }

            return samples;
        }

        public List<Sample> GetSamples(SampleCriteria criteria)
        {
            if (criteria.MaxBpm == 0) criteria.MaxBpm = 200;

            List<Sample> samples;
            lock (Samples)
            {
                samples = Samples
                    .Where(s => string.IsNullOrEmpty(criteria.TrackArtist) || s.Description == criteria.TrackArtist)
                    .Where(s => string.IsNullOrEmpty(criteria.TrackTitle) || s.TrackTitle == criteria.TrackTitle)
                    .Where(s => string.IsNullOrEmpty(criteria.Description) || s.Description == criteria.Description)
                    .Where(s => string.IsNullOrEmpty(criteria.Key) || s.Key == criteria.Key || criteria.IncludeAtonal && s.IsAtonal)
                    .Where(s => !criteria.AtonalOnly || (criteria.AtonalOnly && s.IsAtonal))
                    .Where(s => !criteria.Primary.HasValue || s.IsPrimaryLoop == criteria.Primary.Value)
                    .Where(s => !criteria.LoopMode.HasValue || s.LoopMode == criteria.LoopMode)
                    .Where(s => s.Bpm >= criteria.MinBpm && s.Bpm <= criteria.MaxBpm)
                    .OrderBy(s => s.Description)
                    .ToList();
            }

            if (string.IsNullOrEmpty(criteria.SearchText))
                return samples;

            criteria.SearchText = criteria.SearchText.ToLower().Trim();
            samples = samples.Where(s => s.Tags.Contains(criteria.SearchText)
                                         || s.Description.ToLower().Contains(criteria.SearchText)
                                         || s.TrackArtist.ToLower().Contains(criteria.SearchText)
                                         || s.TrackTitle.ToLower().Contains(criteria.SearchText)).ToList();

            return samples;
        }

        public List<Sample> GetSamples(Track track)
        {
            var samples = Samples
                .Where(
                    x =>
                        x.TrackTitle == track.Title && x.TrackArtist == track.Artist &&
                        x.TrackLength == track.FullLength)
                .OrderBy(x => x.Description)
                .ToList();

            return samples;
        }

        public Track GetTrackFromSample(Sample sample)
        {
            return TrackLibrary.GetTrack(sample.TrackArtist, sample.TrackTitle, sample.TrackLength);
        }

        public void EnsureSampleExists(Sample sample)
        {
            if (!File.Exists(GetSampleFileName(sample)))
            {
                SaveSampleFiles(GetTrackFromSample(sample));
            }
        }

        public void SaveSampleFiles(Track track)
        {
            var samples = GetSamples(track);

            if (samples.Count == 0) return;

            if (!File.Exists(track.Filename)) return;

            var folder = GetSampleFolder(samples[0]);
            if (!Directory.Exists(folder))
                CreateSampleFolder(samples[0]);

            FileSystemHelper.DeleteFiles(folder);

            var bassTrack = BassPlayer.LoadTrackAndAudio(track.Filename);

            foreach (var sample in samples)
            {
                SaveSampleFile(bassTrack, sample);
            }

            BassPlayer.UnloadTrackAudioData(bassTrack);
        }

        private void SaveSampleFile(AE.Models.Track bassTrack, Sample sample)
        {
            var sampleFolder = GetSampleFolder(sample);

            if (!Directory.Exists(sampleFolder))
                CreateSampleFolder(sample);

            var sampleFile = GetSampleFileName(sample);

            var start = bassTrack.SecondsToSamples(sample.Start);
            var offset = bassTrack.SecondsToSamples(sample.Offset);
            var length = bassTrack.SecondsToSamples(sample.Length);

            AudioExportHelper.SavePartialAsWave(bassTrack, sampleFile, start, length, offset, sample.Gain);
        }

        private void CreateSampleFolder(Sample sample)
        {
            var sampleFolder = Path.Combine(SampleLibraryFolder,
                FileSystemHelper.StripInvalidFileNameChars(sample.TrackArtist));

            if (!Directory.Exists(sampleFolder))
                Directory.CreateDirectory(sampleFolder);

            var titleFolder = $"{sample.TrackTitle} ({TimeFormatHelper.GetFormattedSeconds(sample.TrackLength)})";
            titleFolder = FileSystemHelper.StripInvalidFileNameChars(titleFolder);

            sampleFolder = Path.Combine(sampleFolder, titleFolder);

            if (!Directory.Exists(sampleFolder))
                Directory.CreateDirectory(sampleFolder);
        }

        private string GetSampleFolder(Sample sample)
        {
            var sampleFolder = Path.Combine(SampleLibraryFolder,
                FileSystemHelper.StripInvalidFileNameChars(sample.TrackArtist));

            var titleFolder = $"{sample.TrackTitle} ({TimeFormatHelper.GetFormattedSeconds(sample.TrackLength)})";
            titleFolder = FileSystemHelper.StripInvalidFileNameChars(titleFolder);

            sampleFolder = Path.Combine(sampleFolder, titleFolder);

            return sampleFolder;
        }

        public string GetSampleFileName(Sample sample)
        {
            var filename = FileSystemHelper.StripInvalidFileNameChars(sample.Description + ".wav");
            return Path.Combine(GetSampleFolder(sample), filename);
        }

        public List<Sample> GetMixSectionsAsSamples(Track track)
        {
            var bassTrack = BassPlayer.LoadTrackAndAudio(track.Filename);
            var samples = new List<Sample>();

            var fadeIn = new Sample
            {
                Description = "FadeIn",
                Start = bassTrack.SamplesToSeconds(bassTrack.FadeInStart),
                Length = bassTrack.FadeOutLengthSeconds
            };

            UpdateSampleFromTrack(fadeIn, track);

            samples.Add(fadeIn);

            var fadeOut = new Sample
            {
                Description = "FadeOut",
                Start = bassTrack.SamplesToSeconds(bassTrack.FadeOutStart),
                Length = bassTrack.FadeOutLengthSeconds
            };

            UpdateSampleFromTrack(fadeOut, track);

            samples.Add(fadeOut);

            if (bassTrack.UsePreFadeIn)
            {
                var preFadeIn = new Sample
                {
                    Description = "preFadeIn",
                    Start = bassTrack.SamplesToSeconds(bassTrack.PreFadeInStart),
                    Length = bassTrack.SamplesToSeconds(bassTrack.FadeInStart - bassTrack.PreFadeInStart),
                    LoopMode = LoopMode.PartialLoopAnchorEnd
                };

                UpdateSampleFromTrack(preFadeIn, track);

                samples.Add(preFadeIn);
            }

            BassPlayer.UnloadTrackAudioData(bassTrack);

            return samples;
        }

        public void CalculateSampleKey(Sample sample)
        {
            var track = GetTrackFromSample(sample);
            if (track == null) return;

            KeyHelper.CalculateKey(track.Filename);
            TrackLibrary.ReloadTrackMetaData(track.Filename);

            var samples = GetTrackSamples(track);

            samples.ForEach(x => x.Key = track.Key);
        }

        private List<Sample> GetTrackSamples(Track track)
        {
            return Samples.Where(x => x.TrackArtist == track.Artist && x.TrackTitle == track.Title).ToList();
        }

        public class SampleCriteria
        {
            public string SearchText { get; set; }

            public string Key { get; set; }

            public decimal MinBpm { get; set; }

            public decimal MaxBpm { get; set; }

            public bool AtonalOnly { get; set; }

            public bool IncludeAtonal { get; set; }

            public bool? Primary { get; set; }

            public LoopMode? LoopMode { get; set; }

            public string TrackTitle { get; set; }

            public string TrackArtist { get; set; }

            public string Description { get; set; }
        }
    }
}