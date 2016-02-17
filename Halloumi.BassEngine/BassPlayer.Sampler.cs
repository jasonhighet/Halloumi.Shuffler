using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Halloumi.Shuffler.AudioEngine.Channels;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;
using Halloumi.Common.Helpers;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;

namespace Halloumi.Shuffler.AudioEngine
{
    public partial class BassPlayer
    {
        /// <summary>
        ///     The next available sample Id
        /// </summary>
        private static int _nextSampleId;

        /// <summary>
        ///     A collection of all loaded samples
        /// </summary>
        private readonly List<Sample> _cachedSamples = new List<Sample>();

        private readonly object _samplePlayLock = new object();

        private MixerChannel _samplerMixer;

        private OutputSplitter _samplerOutputSplitter;

        /// <summary>
        ///     The path of the temporary sampler folder
        /// </summary>
        private string _tempSamplerFolder = "";

        /// <summary>
        ///     Gets or sets the sampler output.
        /// </summary>
        public SoundOutput SamplerOutput
        {
            get { return _samplerOutputSplitter.SoundOutput; }
            set { _samplerOutputSplitter.SoundOutput = value; }
        }

        /// <summary>
        ///     Gets or sets the track output.
        /// </summary>
        public SoundOutput TrackOutput
        {
            get { return _trackOutputSplitter.SoundOutput; }
            set { _trackOutputSplitter.SoundOutput = value; }
        }

        public List<Sample> Samples => _cachedSamples.ToList();

        /// <summary>
        ///     Unloads the samples.
        /// </summary>
        public void UnloadSamples()
        {
            // unload all samples
            foreach (var sample in _cachedSamples.ToList())
            {
                UnloadSample(sample);
            }
            _cachedSamples.Clear();
        }

        /// <summary>
        ///     Unloads the sample audio data.
        /// </summary>
        /// <param name="sample">The sample.</param>
        private void UnloadSample(Sample sample)
        {
            if (sample == null) return;
            if (sample.Channel == int.MinValue) return;

            DebugHelper.WriteLine("Unloading audioStream " + sample.Description);

            AudioStreamHelper.RemoveFromMixer(sample, _samplerMixer.InternalChannel);
            AudioStreamHelper.UnloadAudio(sample);

            _cachedSamples.Remove(sample);
        }


        /// <summary>
        ///     Loads the sample.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="description">The description.</param>
        /// <param name="gain">The gain.</param>
        /// <returns>
        ///     The loaded sample
        /// </returns>
        private Sample LoadSample(string filename, string description, float gain)
        {
            DebugHelper.WriteLine("Loading audioStream " + filename);

            if (!File.Exists(filename)) throw new Exception("Cannot find file " + filename);

            var sample = new Sample
            {
                Id = _nextSampleId++,
                Filename = filename,
                Gain = gain,
                Description = string.IsNullOrWhiteSpace(description)
                    ? GetSampleDescriptionFromFilename(filename)
                    : description
            };


            AudioStreamHelper.LoadAudio(sample);
            AddSampleToSampler(sample);

            _cachedSamples.Add(sample);
            return sample;
        }

        private static string GetSampleDescriptionFromFilename(string filename)
        {
            return
                StringHelper.TitleCase((Path.GetFileNameWithoutExtension(filename) + "").Replace("_", " ")
                    .Replace("-", " "));
        }

        public Sample LoadSample(Track track, string sampleKey)
        {
            if (track == null) return null;

            var trackSample = GetAllTrackSamples(track)
                .FirstOrDefault(ts => ts.Key == sampleKey);

            return trackSample == null ? null : LoadSample(track, trackSample);
        }

        private Sample LoadSample(Track track, TrackSample trackSample)
        {
            if (trackSample == null) return null;
            if (track == null) return null;

            var filename = GetSampleFilename(trackSample, track.Description, track);

            if (!File.Exists(filename))
            {
                AudioExportHelper.SavePartialAsWave(track, filename, track.SecondsToSamples(trackSample.Start),
                    track.SecondsToSamples(trackSample.Length));
            }

            Sample sample;
            try
            {
                sample = LoadSample(filename, trackSample.Description, track.Gain);
                sample.IsLooped = trackSample.IsLooped;
                sample.SampleKey = trackSample.Key;
                sample.LinkedTrackDescription = track.Description;
                sample.Gain = track.Gain;
                sample.Bpm = trackSample.CalculateBpm(track);
            }
            catch
            {
                sample = null;
            }

            return sample;
        }

        private string GetSampleFilename(TrackSample trackSample, string trackDescription, Track track)
        {
            var dateModified = GetTrackLastModified(track).ToString("yyyyMMddhhmmss");
            var filename = $"{trackDescription}_{trackSample.Key}_{dateModified}_.wav";

            filename = FileSystemHelper.StripInvalidFileNameChars(filename).ToLower().Replace(" ", "_");
            filename = Path.Combine(_tempSamplerFolder, filename);

            return filename;
        }

        private static DateTime GetTrackLastModified(AudioStream track)
        {
            var shufflerFile = ExtenedAttributesHelper.GetExtendedAttributeFile(track.Description);
            var shufflerDate = DateTime.MinValue;

            if (File.Exists(shufflerFile))
            {
                shufflerDate = File.GetLastWriteTime(shufflerFile);
            }

            var dateModified = File.GetLastWriteTime(track.Filename);

            return shufflerDate > dateModified ? shufflerDate : dateModified;
        }

        public List<Sample> LoadSamples(Track track)
        {
            if (track == null) return new List<Sample>();

            DebugHelper.WriteLine("Loading samples for " + track.Description);

            return GetAllTrackSamples(track)
                .Select(trackSample => LoadSample(track, trackSample))
                .Where(sample => sample != null)
                .ToList();
        }

        private IEnumerable<TrackSample> GetAllTrackSamples(Track track)
        {
            DebugHelper.WriteLine("GetAllTrackSamples for " + track.Description);

            var trackSamples = GetTrackSamples(track);

            var fadeOutTrackSample = new TrackSample
            {
                IsLooped = track.IsLoopedAtEnd,
                Start = track.SamplesToSeconds(track.FadeOutStart),
                Length = track.FadeOutLengthSeconds,
                Description = "Fade Out",
                Key = "FadeOut"
            };
            trackSamples.Insert(0, fadeOutTrackSample);

            var fadeInTrackSample = new TrackSample
            {
                IsLooped = track.IsLoopedAtStart,
                Start = track.SamplesToSeconds(track.FadeInStart),
                Length = track.FadeInLengthSeconds,
                Description = "Fade In",
                Key = "FadeIn"
            };
            trackSamples.Insert(0, fadeInTrackSample);

            if (!track.UsePreFadeIn) return trackSamples;

            var preFadeInTrackSample = new TrackSample
            {
                IsLooped = false,
                Start = track.SamplesToSeconds(track.PreFadeInStart),
                Length = track.PreFadeInLengthSeconds,
                Description = "Pre Fade In",
                Key = "PreFadeIn"
            };
            trackSamples.Insert(0, preFadeInTrackSample);

            return trackSamples;
        }

        private List<TrackSample> GetTrackSamples(Track track)
        {
            return GetAutomationAttributes(track)
                .TrackSamples
                .OrderBy(t => t.Description)
                .ToList();
        }

        /// <summary>
        ///     Unloads the samples.
        /// </summary>
        /// <param name="track">The track.</param>
        public void UnloadSamples(Track track)
        {
            if (track == null) return;
            var filenames =
                GetAllTrackSamples(track)
                    .Select(trackSample => GetSampleFilename(trackSample, track.Description, track));

            foreach (var filename in filenames)
            {
                UnloadSample(filename);
            }
        }

        /// <summary>
        ///     Unloads and deletes a sample.
        /// </summary>
        /// <param name="filename">The filename of the sample.</param>
        private void UnloadSample(string filename)
        {
            var sample = Samples.FirstOrDefault(s => s.Filename == filename);
            if (sample != null)
            {
                UnloadSample(sample);
            }
        }

        /// <summary>
        ///     Gets a loaded sample by its description.
        /// </summary>
        /// <param name="sampleId">The sample identifier.</param>
        /// <returns>
        ///     The loaded sample
        /// </returns>
        private Sample GetSampleBySampleId(string sampleId)
        {
            return Samples.FirstOrDefault(s => s.SampleId == sampleId);
        }

        /// <summary>
        ///     Plays a sample.
        /// </summary>
        /// <param name="sample">The sample.</param>
        public void PlaySample(Sample sample)
        {
            if (sample == null) return;
            if (sample.Channel == int.MinValue) return;

            lock (_samplePlayLock)
            {
                if (sample.Channel == int.MinValue) return;
                BassMix.BASS_Mixer_ChannelPause(sample.Channel);

                if (CurrentTrack == null)
                    AudioStreamHelper.ResetTempo(sample);
                else if (sample.LinkedTrackDescription == CurrentTrack.Description)
                    AudioStreamHelper.ResetTempo(sample);
                else
                    AudioStreamHelper.SetTempoToMatchBpm(sample, CurrentTrack.Bpm);

                if (sample.Channel == int.MinValue) return;
                Bass.BASS_ChannelSetPosition(sample.Channel, 1);
                AudioStreamHelper.SetVolumeSlide(sample, 0F, 1F, 0.1D);

                if (sample.Channel == int.MinValue) return;
                BassMix.BASS_Mixer_ChannelPlay(sample.Channel);
            }

            _samplerMixer.SetPluginBpm();

            if (CurrentTrack == null) return;

            StartRecordingSampleTrigger(sample);
        }

        /// <summary>
        ///     Stops the sample.
        /// </summary>
        /// <param name="audioStream">The sample.</param>
        private void StopSample(AudioStream audioStream)
        {
            lock (_samplePlayLock)
            {
                AudioStreamHelper.Pause(audioStream);
            }
        }

        /// <summary>
        ///     Stops the samples.
        /// </summary>
        public void StopSamples()
        {
            foreach (var sample in Samples)
            {
                StopSample(sample);
            }
        }

        /// <summary>
        ///     Mutes the sample.
        /// </summary>
        /// <param name="sample">The sample.</param>
        public void MuteSample(Sample sample)
        {
            var volume = AudioStreamHelper.GetVolume(sample);
            if (volume != 0)
            {
                AudioStreamHelper.SetVolumeSlide(sample, 1F, 0F, 0.1D);
            }

            StopRecordingSampleTrigger();
        }

        /// <summary>
        ///     Gets the sample mixer volume.
        /// </summary>
        public decimal GetSamplerMixerVolume()
        {
            return _samplerMixer.GetVolume();
        }

        /// <summary>
        ///     Sets the sample mixer volume.
        /// </summary>
        /// <param name="volume">The volume as a value between 0 and 100.</param>
        public void SetSamplerMixerVolume(decimal volume)
        {
            _samplerMixer.SetVolume(volume);
        }

        private void InitialiseSampler()
        {
            DebugHelper.WriteLine("InitialiseSampler");

            // create mixer channel
            _samplerMixer = new MixerChannel(this, MixerChannelOutputType.SingleOutput);
            _samplerMixer.SetVolume((decimal) DefaultFadeOutStartVolume);
            _samplerMixer.CutBass();

            _samplerOutputSplitter = new OutputSplitter(_samplerMixer, _speakerOutput, _monitorOutput);

            // delete any temporary audioStream files
            _tempSamplerFolder = Path.Combine(Path.GetTempPath(), "Sampler");
            if (!Directory.Exists(_tempSamplerFolder))
                Directory.CreateDirectory(_tempSamplerFolder);

            try
            {
                FileSystemHelper.DeleteFiles(_tempSamplerFolder);
            }
            catch
            {
                // ignored
            }

            DebugHelper.WriteLine("END InitialiseSampler");
        }

        /// <summary>
        ///     Adds to the mixer channel, and sets the sync points.
        /// </summary>
        /// <param name="sample">The sample to sync.</param>
        private void AddSampleToSampler(Sample sample)
        {
            if (sample == null) return;

            DebugHelper.WriteLine("Add audioStream to sampler " + sample.Description);

            // load audio data if not loaded
            if (!sample.IsAudioLoaded())
            {
                lock (sample)
                {
                    AudioStreamHelper.LoadAudio(sample);
                }
            }

            lock (MixerLock)
            {
                AudioStreamHelper.AddToMixer(sample, _samplerMixer.InternalChannel);
            }

            // set audioStream sync event
            sample.SyncProc = OnSampleSync;

            SetSampleSyncPositions(sample);
        }

        /// <summary>
        ///     Sets the sample sync positions.
        /// </summary>
        /// <param name="sample">The sample.</param>
        private static void SetSampleSyncPositions(Sample sample)
        {
            if (sample == null) return;

            ClearSampleSyncPositions(sample);

            DebugHelper.WriteLine("Set audioStream sync positions " + sample.Description);

            // set end audioStream sync
            SetSampleSync(sample, sample.Length - 2000, SampleSyncType.SampleEnd);
        }

        /// <summary>
        ///     Sets a sample sync.
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <param name="position">The position.</param>
        /// <param name="syncType">Type of the sync.</param>
        private static void SetSampleSync(Sample sample, long position, SampleSyncType syncType)
        {
            if (sample == null || sample.Channel == int.MinValue) return;

            var syncId = BassMix.BASS_Mixer_ChannelSetSync(sample.Channel,
                BASSSync.BASS_SYNC_POS | BASSSync.BASS_SYNC_MIXTIME,
                position,
                sample.SyncProc,
                new IntPtr((int) syncType));

            if (syncType == SampleSyncType.SampleEnd)
                sample.SampleEndSyncId = syncId;
        }

        /// <summary>
        ///     Clears the sample sync positions.
        /// </summary>
        /// <param name="sample">The sample.</param>
        private static void ClearSampleSyncPositions(Sample sample)
        {
            DebugHelper.WriteLine("Clear audioStream sync positions " + sample.Description);

            if (sample.Channel == int.MinValue || sample.SampleEndSyncId == int.MinValue) return;

            BassMix.BASS_Mixer_ChannelRemoveSync(sample.Channel, sample.SampleEndSyncId);
            sample.SampleEndSyncId = int.MinValue;
        }

        private void LoopSample(AudioStream sample)
        {
            if (sample == null || sample.Channel == int.MinValue) return;
            lock (_samplePlayLock)
            {
                if (sample.Channel != int.MinValue) BassMix.BASS_Mixer_ChannelPause(sample.Channel);
                if (sample.Channel != int.MinValue) Bass.BASS_ChannelSetPosition(sample.Channel, 1);
                if (sample.Channel != int.MinValue) BassMix.BASS_Mixer_ChannelPlay(sample.Channel);
            }
        }

        /// <summary>
        ///     Called when the track sync event is fired
        /// </summary>
        private void OnSampleSync(int handle, int channel, int data, IntPtr pointer)
        {
            var syncType = (SampleSyncType) (pointer.ToInt32());
            DebugHelper.WriteLine("Sample Sync Fired: " + syncType);

            if (syncType != SampleSyncType.SampleEnd) return;

            var sample = Samples.FirstOrDefault(s => s.Channel == channel);

            if (sample == null || sample.Channel == int.MinValue) return;

            if (!sample.IsLooped) AudioStreamHelper.SetVolume(sample, 0M);
            LoopSample(sample);
        }

        /// <summary>
        ///     Enumeration style representing different types of audioStream sync events.
        /// </summary>
        private enum SampleSyncType
        {
            /// <summary>
            ///     audioStream end sync event type
            /// </summary>
            SampleEnd = 0
        }
    }
}