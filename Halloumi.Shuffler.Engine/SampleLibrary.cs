using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Halloumi.Common.Helpers;

using IdSharp.Tagging.ID3v2;
using BE = Halloumi.BassEngine;

namespace Halloumi.Shuffler.Engine
{
    public class SampleLibrary
    {
        /// <summary>
        /// Initializes a new instance of the Library class.
        /// </summary>
        public SampleLibrary(BE.BassPlayer bassPlayer, Library trackLibrary)
        {
            this.Samples = new List<Sample>();
            this.BassPlayer = bassPlayer;
            this.TrackLibrary = trackLibrary;

            this.SampleLibraryFolder = Path.Combine(Path.GetTempPath(), "SampleLibary");
            if (!Directory.Exists(this.SampleLibraryFolder))
                Directory.CreateDirectory(this.SampleLibraryFolder);
        }

        /// <summary>
        /// Loads the library from the cache.
        /// </summary>
        public void LoadFromCache()
        {
            if (File.Exists(this.SampleLibraryFilename))
            {
                var samples = SerializationHelper<List<Sample>>.FromXmlFile(this.SampleLibraryFilename);

                lock (this.Samples)
                {
                    this.Samples.Clear();
                    this.Samples.AddRange(samples.ToArray());
                }
            }
        }

        /// <summary>
        /// Saves the sample details to a cache file
        /// </summary>
        public void SaveCache()
        {
            SerializationHelper<List<Sample>>.ToXmlFile(this.Samples, this.SampleLibraryFilename);
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
            this.Samples.RemoveAll(x => x.TrackTitle == track.Title && x.TrackArtist == track.Artist && x.TrackLength == track.FullLength);

            trackSamples.ForEach(x => UpdateSampleFromTrack(x, track));

            this.Samples.AddRange(trackSamples);
        }

        public List<Sample> GetSamples(string trackArtist, string trackTitle, string description)
        {
            var samples = new List<Sample>();
            lock (this.Samples)
            {
                samples = this.Samples
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
            if (criteria.MaxBPM == 0) criteria.MaxBPM = 200;

            var samples = new List<Sample>();
            lock (this.Samples)
            {
                samples = this.Samples
                    .Where(s => string.IsNullOrEmpty(criteria.TrackArtist) || s.Description == criteria.TrackArtist)
                    .Where(s => string.IsNullOrEmpty(criteria.TrackTitle) || s.TrackTitle == criteria.TrackTitle)
                    .Where(s => string.IsNullOrEmpty(criteria.Description) || s.Description == criteria.Description)
                    .Where(s => criteria.Atonal.HasValue && s.IsAtonal == criteria.Atonal.Value)
                    .Where(s => string.IsNullOrEmpty(criteria.Key) || s.Key == criteria.Key)
                    .Where(s => !criteria.Primary.HasValue || s.IsPrimaryLoop == criteria.Primary.Value)
                    .Where(s => !criteria.LoopMode.HasValue || s.LoopMode == criteria.LoopMode)
                    .Where(s => s.BPM >= criteria.MinBPM && s.BPM <= criteria.MaxBPM)
                    .OrderBy(s => s.Description)
                    .ToList();
            }

            if (!string.IsNullOrEmpty(criteria.SearchText))
            {
                criteria.SearchText = criteria.SearchText.ToLower().Trim();
                samples = samples.Where(s => s.Tags.Contains(criteria.SearchText)
                    || s.Description.ToLower().Contains(criteria.SearchText)
                    || s.TrackArtist.ToLower().Contains(criteria.SearchText)
                    || s.TrackTitle.ToLower().Contains(criteria.SearchText)).ToList();
            }

            return samples;
        }

        public List<Sample> GetSamples(Track track)
        {
            var samples = this.Samples
                .Where(x => x.TrackTitle == track.Title && x.TrackArtist == track.Artist && x.TrackLength == track.FullLength)
                .OrderBy(x => x.Description)
                .ToList();

            return samples;
        }

        public Track GetTrackFromSample(Sample sample)
        {
            return this.TrackLibrary.GetTrack(sample.TrackArtist, sample.TrackTitle, sample.TrackLength);
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

            var bassTrack = this.BassPlayer.LoadTrackAndAudio(track.Filename);

            foreach (var sample in samples)
            {
                SaveSampleFile(bassTrack, sample);
            }

            this.BassPlayer.UnloadTrackAudioData(bassTrack);
        }

        private void SaveSampleFile(BE.Track bassTrack, Sample sample)
        {
            var sampleFolder = GetSampleFolder(sample);

            if (!Directory.Exists(sampleFolder))
                CreateSampleFolder(sample);

            var sampleFile = GetSampleFileName(sample);

            var start = bassTrack.SecondsToSamples(sample.Start);
            var offset = bassTrack.SecondsToSamples(sample.Offset);
            var length = bassTrack.SecondsToSamples(sample.Length);

            BE.BassHelper.SavePartialAsWave(bassTrack, sampleFile, start, length, offset, sample.Gain);
        }

        //public void EnsureSampleFileExists(Sample sample)
        //{
        //    var sampleFilename = GetSampleFilename(sample);

        //    if (!File.Exists(sampleFilename))
        //        SaveSampleFile(sample);
        //}

        private void CreateSampleFolder(Sample sample)
        {
            var sampleFolder = Path.Combine(this.SampleLibraryFolder, FileSystemHelper.StripInvalidFileNameChars(sample.TrackArtist));

            if (!Directory.Exists(sampleFolder))
                Directory.CreateDirectory(sampleFolder);

            string titleFolder = String.Format("{0} ({1})", sample.TrackTitle, BE.BassHelper.GetShortFormattedSeconds(sample.TrackLength));
            titleFolder = FileSystemHelper.StripInvalidFileNameChars(titleFolder);

            sampleFolder = Path.Combine(sampleFolder, titleFolder);

            if (!Directory.Exists(sampleFolder))
                Directory.CreateDirectory(sampleFolder);
        }

        private string GetSampleFolder(Sample sample)
        {
            var sampleFolder = Path.Combine(this.SampleLibraryFolder, FileSystemHelper.StripInvalidFileNameChars(sample.TrackArtist));

            string titleFolder = String.Format("{0} ({1})", sample.TrackTitle, BE.BassHelper.GetShortFormattedSeconds(sample.TrackLength));
            titleFolder = FileSystemHelper.StripInvalidFileNameChars(titleFolder);

            sampleFolder = Path.Combine(sampleFolder, titleFolder);

            return sampleFolder;
        }

        public string GetSampleFileName(Sample sample)
        {
            var filename = FileSystemHelper.StripInvalidFileNameChars(sample.Description + ".wav");
            return Path.Combine(GetSampleFolder(sample), filename);
        }

        #region Properties

        /// <summary>
        /// Gets or sets the bass player.
        /// </summary>
        private BE.BassPlayer BassPlayer { get; set; }

        /// <summary>
        /// Gets or sets the track library.
        /// </summary>
        public Library TrackLibrary { get; private set; }

        /// <summary>
        /// Gets or sets the samples in the library
        /// </summary>
        private List<Sample> Samples { get; set; }

        private string SampleLibraryFolder { get; set; }

        /// <summary>
        /// Gets the name of the file where the sample data is cached.
        /// </summary>
        private string SampleLibraryFilename
        {
            get { return Path.Combine(this.TrackLibrary.ShufflerFolder, "Halloumi.Shuffler.SampleLibrary.xml"); }
        }

        #endregion

        public List<Sample> GetMixSectionsAsSamples(Track track)
        {
            var bassTrack = this.BassPlayer.LoadTrackAndAudio(track.Filename);
            var samples = new List<Sample>();

            var fadeIn = new Sample();
            fadeIn.Description = "FadeIn";
            fadeIn.Start = bassTrack.SamplesToSeconds(bassTrack.FadeInStart);
            fadeIn.Length = bassTrack.FadeOutLengthSeconds;

            this.UpdateSampleFromTrack(fadeIn, track);

            samples.Add(fadeIn);

            var fadeOut = new Sample();
            fadeOut.Description = "FadeOut";
            fadeOut.Start = bassTrack.SamplesToSeconds(bassTrack.FadeOutStart);
            fadeOut.Length = bassTrack.FadeOutLengthSeconds;

            this.UpdateSampleFromTrack(fadeOut, track);

            samples.Add(fadeOut);

            if (bassTrack.UsePreFadeIn)
            {
                var preFadeIn = new Sample();
                preFadeIn.Description = "preFadeIn";
                preFadeIn.Start = bassTrack.SamplesToSeconds(bassTrack.PreFadeInStart);
                preFadeIn.Length = bassTrack.SamplesToSeconds(bassTrack.FadeInStart - bassTrack.PreFadeInStart);
                preFadeIn.LoopMode = LoopMode.PartialLoopAnchorEnd;

                this.UpdateSampleFromTrack(preFadeIn, track);

                samples.Add(preFadeIn);
            }

            this.BassPlayer.UnloadTrackAudioData(bassTrack);

            return samples;
        }

        public class SampleCriteria
        {
            public string SearchText { get; set; }

            public string Key { get; set; }

            public decimal MinBPM { get; set; }

            public decimal MaxBPM { get; set; }

            public Nullable<bool> Atonal { get; set; }

            public Nullable<bool> Primary { get; set; }

            public Nullable<LoopMode> LoopMode { get; set; }

            public string TrackTitle { get; set; }

            public string TrackArtist { get; set; }

            public string Description { get; set; }
        }

        public void CalculateSampleKey(Sample sample)
        {
            var track = this.GetTrackFromSample(sample);
            if (track == null) return;

            BE.KeyHelper.CalculateKey(track.Filename);
            this.TrackLibrary.ReloadTrackMetaData(track.Filename);
            
            var samples = this.GetTrackSamples(track);

            samples.ForEach(x => x.Key = track.Key);
            
        }

        private List<Sample> GetTrackSamples(Track track)
        {
            return this.Samples.Where(x => x.TrackArtist == track.Artist && x.TrackTitle == track.Title).ToList();
        }
    }
}