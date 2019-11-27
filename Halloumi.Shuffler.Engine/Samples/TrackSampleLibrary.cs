using System.Collections.Generic;
using System.IO;
using System.Linq;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioEngine.BassPlayer;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioLibrary.Models;

namespace Halloumi.Shuffler.AudioLibrary.Samples
{
    public class TrackSampleLibrary : ISampleLibrary
    {
        /// <summary>
        ///     Initializes a new instance of the Library class.
        /// </summary>
        public TrackSampleLibrary(BassPlayer bassPlayer, Library trackLibrary)
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
        private BassPlayer BassPlayer { get; }

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
        {
            get { return Path.Combine(TrackLibrary.ShufflerFolder, "Halloumi.Shuffler.SampleLibrary.xml"); }
        }

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

            sample.Filename = track.Filename;
        }

        public void UpdateTrackSamples(Track track, List<Sample> trackSamples)
        {
            Samples.RemoveAll(
                x => x.TrackTitle == track.Title && x.TrackArtist == track.Artist && x.TrackLength == track.FullLength);

            trackSamples.ForEach(x => UpdateSampleFromTrack(x, track));

            Samples.AddRange(trackSamples);
        }

        public List<Sample> GetSamples()
        {
            return Samples.ToList(); 
        }

        public List<Sample> GetSamples(SearchCriteria searchCriteria)
        {
            if (searchCriteria.MaxBpm == 0) searchCriteria.MaxBpm = 200;

            List<Sample> samples;
            lock (Samples)
            {
                samples = Samples
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
                return samples;

            searchCriteria.SearchText = searchCriteria.SearchText.ToLower().Trim();
            samples = samples.Where(s => s.Tags.Contains(searchCriteria.SearchText)
                                         || s.Description.ToLower().Contains(searchCriteria.SearchText)
                                         || s.TrackArtist.ToLower().Contains(searchCriteria.SearchText)
                                         || s.TrackTitle.ToLower().Contains(searchCriteria.SearchText))
                .ToList();

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

        public void ExportMixSectionsAsSamples(Track track)
        {
            var existingSamples = GetTrackSamples(track);
            if (existingSamples.Count > 0) return;
            var samples = GetMixSectionsAsSamples(track);
            UpdateTrackSamples(track, samples);
            SaveCache();
        }

        public List<Sample> GetMixSectionsAsSamples(Track track)
        {
            var bassTrack = BassPlayer.LoadTrackAndAudio(track.Filename);
            var samples = new List<Sample>();

            var fadeIn = new Sample
            {
                Description = "FadeIn",
                Start = bassTrack.SamplesToSeconds(bassTrack.FadeInStart),
                Length = bassTrack.FadeInLengthSeconds,
                Bpm = BpmHelper.GetBpmFromLoopLength(bassTrack.FadeInLengthSeconds),
                Gain = bassTrack.Gain
            };

            UpdateSampleFromTrack(fadeIn, track);

            samples.Add(fadeIn);

            var fadeOut = new Sample
            {
                Description = "FadeOut",
                Start = bassTrack.SamplesToSeconds(bassTrack.FadeOutStart),
                Length = bassTrack.FadeOutLengthSeconds,
                Bpm = BpmHelper.GetBpmFromLoopLength(bassTrack.FadeOutLengthSeconds),
                Gain = bassTrack.Gain
            };

            UpdateSampleFromTrack(fadeOut, track);

            samples.Add(fadeOut);

            BassPlayer.UnloadTrackAudioData(bassTrack);

            return samples;
        }

        public void CalculateSampleKey(Sample sample)
        {
            var track = GetTrackFromSample(sample);
            if (track == null) return;

            KeyHelper.CalculateKey(track.Filename);
            TrackLibrary.LoadTrack(track.Filename);

            var samples = GetTrackSamples(track);

            samples.ForEach(x => x.Key = track.Key);
        }

        private List<Sample> GetTrackSamples(Track track)
        {
            return Samples.Where(x => x.TrackArtist == track.Artist && x.TrackTitle == track.Title).ToList();
        }

        public List<Track> GetAllTracks()
        {
            var tracks = new List<Track>();

            foreach (
                var track in
                Samples.Select(GetTrackFromSample)
                    .Where(track => !tracks.Contains(track))
                    .Where(track => track != null))
                tracks.Add(track);

            return tracks;
        }


    }
}