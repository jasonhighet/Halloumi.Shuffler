using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Halloumi.BassEngine.Channels;
using Halloumi.Common.Helpers;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Fx;
using Un4seen.Bass.AddOn.Mix;
using Un4seen.Bass.AddOn.Vst;
using Un4seen.Bass.AddOn.WaDsp;

namespace Halloumi.BassEngine
{
    public partial class BassPlayer
    {
        #region Properties

        /// <summary>
        /// A collection of all loaded samples
        /// </summary>
        private List<Sample> _cachedSamples = new List<Sample>();

        /// <summary>
        /// The next available sample Id
        /// </summary>
        private static int _nextSampleId = 0;

        private MixerChannel _samplerMixer;

        private OutputSplitter _samplerOutputSplitter;

        /// <summary>
        /// The path of the temporary sampler folder
        /// </summary>
        private string _tempSamplerFolder = "";

        /// <summary>
        /// The sampler mixer bass EQ effect handle
        /// </summary>
        //private int _samplerMixerBassEQ = int.MinValue;

        #endregion

        #region Public Methods

        /// <summary>
        /// Unloads the samples.
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
        /// Unloads the sample audio data.
        /// </summary>
        /// <param name="sample">The sample.</param>
        public void UnloadSample(Sample sample)
        {
            if (sample == null) return;
            if (sample.Channel == int.MinValue) return;

            DebugHelper.WriteLine("Unloading sample " + sample.Description);

            BassHelper.RemoveSampleFromMixer(sample, _samplerMixer.InternalChannel);
            BassHelper.UnloadSampleAudio(sample);

            _cachedSamples.Remove(sample);
        }

        /// <summary>
        /// Loads the sample.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>The loaded sample</returns>
        public Sample LoadSample(string filename)
        {
            return LoadSample(filename, "", 0);
        }

        /// <summary>
        /// Loads the sample.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="description">The description.</param>
        /// <returns>The loaded sample</returns>
        public Sample LoadSample(string filename, string description, float gain)
        {
            DebugHelper.WriteLine("Loading sample " + filename);

            if (!File.Exists(filename)) throw new Exception("Cannot find file " + filename);

            var sample = new Sample();
            sample.Id = _nextSampleId++;
            sample.Filename = filename;
            sample.Gain = gain;

            if (description == "") description = StringHelper.TitleCase(Path.GetFileNameWithoutExtension(sample.Filename).Replace("_", " ").Replace("-", " "));
            sample.Description = description;

            LoadSampleAudioData(sample);
            AddSampleToSampler(sample);

            _cachedSamples.Add(sample);
            return sample;
        }

        public Sample LoadSample(Track track, string sampleKey)
        {
            if (track == null) return null;

            var trackSample = GetAllTrackSamples(track)
                .Where(ts => ts.Key == sampleKey)
                .FirstOrDefault();

            if (trackSample == null) return null;

            return LoadSample(track, trackSample);
        }

        public Sample LoadSample(Track track, TrackSample trackSample)
        {
            if (trackSample == null) return null;
            if (track == null) return null;

            var filename = GetSampleFilename(trackSample, track.Description, track);

            if (!File.Exists(filename))
            {
                BassHelper.SavePartialAsWave(track, filename, track.SecondsToSamples(trackSample.Start), track.SecondsToSamples(trackSample.Length));
            }

            Sample sample = null;
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
            var filename = String.Format("{0}_{1}_{2}_.wav", trackDescription, trackSample.Key, dateModified);

            filename = FileSystemHelper.StripInvalidFileNameChars(filename).ToLower().Replace(" ", "_");
            filename = Path.Combine(_tempSamplerFolder, filename);

            return filename;
        }

        private DateTime GetTrackLastModified(Track track)
        {
            var shufflerFile = GetExtendedAttributeFile(track);
            var shufflerDate = DateTime.MinValue;

            if (File.Exists(shufflerFile))
            {
                shufflerDate = File.GetLastWriteTime(shufflerFile);
            }

            var dateModified = File.GetLastWriteTime(track.Filename);

            if (shufflerDate > dateModified)
                return shufflerDate;
            else
                return dateModified;
        }

        public List<Sample> LoadSamples(Track track)
        {
            var samples = new List<Sample>();
            if (track == null) return samples;

            DebugHelper.WriteLine("Loading samples for " + track.Description);

            var trackSamples = GetAllTrackSamples(track);
            foreach (var trackSample in trackSamples)
            {
                var sample = LoadSample(track, trackSample);
                if (sample != null) samples.Add(sample);
            }
            return samples;
        }

        private List<TrackSample> GetAllTrackSamples(Track track)
        {
            DebugHelper.WriteLine("GetAllTrackSamples for " + track.Description);

            var trackSamples = GetTrackSamples(track);

            var fadeOutTrackSample = new TrackSample();
            fadeOutTrackSample.IsLooped = track.IsLoopedAtEnd;
            fadeOutTrackSample.Start = track.SamplesToSeconds(track.FadeOutStart);
            fadeOutTrackSample.Length = track.FadeOutLengthSeconds;
            fadeOutTrackSample.Description = "Fade Out";
            fadeOutTrackSample.Key = "FadeOut";
            trackSamples.Insert(0, fadeOutTrackSample);

            var fadeInTrackSample = new TrackSample();
            fadeInTrackSample.IsLooped = track.IsLoopedAtStart;
            fadeInTrackSample.Start = track.SamplesToSeconds(track.FadeInStart);
            fadeInTrackSample.Length = track.FadeInLengthSeconds;
            fadeInTrackSample.Description = "Fade In";
            fadeInTrackSample.Key = "FadeIn";
            trackSamples.Insert(0, fadeInTrackSample);

            if (track.UsePreFadeIn)
            {
                var preFadeInTrackSample = new TrackSample();
                preFadeInTrackSample.IsLooped = false;
                preFadeInTrackSample.Start = track.SamplesToSeconds(track.PreFadeInStart);
                preFadeInTrackSample.Length = track.PreFadeInLengthSeconds;
                preFadeInTrackSample.Description = "Pre Fade In";
                preFadeInTrackSample.Key = "PreFadeIn";
                trackSamples.Insert(0, preFadeInTrackSample);
            }

            return trackSamples;
        }

        public List<TrackSample> GetTrackSamples(Track track)
        {
            return GetAutomationAttributes(track)
                .TrackSamples
                .OrderBy(t => t.Description)
                .ToList();
        }

        /// <summary>
        /// Unloads the samples.
        /// </summary>
        /// <param name="track">The track.</param>
        public void UnloadSamples(Track track)
        {
            if (track == null) return;
            foreach (var trackSample in GetAllTrackSamples(track))
            {
                var filename = GetSampleFilename(trackSample, track.Description, track);
                UnloadSample(filename);
            }
        }

        /// <summary>
        /// Unloads and deletes a sample.
        /// </summary>
        /// <param name="filename">The filename of the sample.</param>
        private void UnloadSample(string filename)
        {
            var sample = Samples.Where(s => s.Filename == filename).FirstOrDefault();
            if (sample != null)
            {
                UnloadSample(sample);
            }
        }

        /// <summary>
        /// Gets a loaded sample by its description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>The loaded sample</returns>
        public Sample GetSampleBySampleId(string sampleId)
        {
            return Samples.Where(s => s.SampleId == sampleId).FirstOrDefault();
        }

        /// <summary>
        /// Plays a sample.
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
                    BassHelper.ResetSampleTempo(sample);
                else if (sample.LinkedTrackDescription == CurrentTrack.Description)
                    BassHelper.ResetSampleTempo(sample);
                else
                    BassHelper.SetSampleTempoToMatchBpm(sample, CurrentTrack.Bpm);

                if (sample.Channel == int.MinValue) return;
                Bass.BASS_ChannelSetPosition(sample.Channel, 1);
                BassHelper.SetSampleVolumeSlide(sample, 0F, 1F, 0.1D);

                if (sample.Channel == int.MinValue) return;
                BassMix.BASS_Mixer_ChannelPlay(sample.Channel);
            }

            _samplerMixer.SetPluginBpm();

            if (CurrentTrack == null) return;

            StartRecordingSampleTrigger(sample);
        }

        private object _samplePlayLock = new object();

        /// <summary>
        /// Stops the sample.
        /// </summary>
        /// <param name="sample">The sample.</param>
        public void StopSample(Sample sample)
        {
            if (sample == null || sample.Channel == int.MinValue) return;

            lock (_samplePlayLock)
            {
                BassMix.BASS_Mixer_ChannelPause(sample.Channel);
            }
        }

        /// <summary>
        /// Stops the samples.
        /// </summary>
        public void StopSamples()
        {
            foreach (var sample in Samples)
            {
                StopSample(sample);
            }
        }

        /// <summary>
        /// Mutes the sample.
        /// </summary>
        /// <param name="sample">The sample.</param>
        public void MuteSample(Sample sample)
        {
            var volume = BassHelper.GetSampleVolume(sample);
            if (volume != 0)
            {
                BassHelper.SetSampleVolumeSlide(sample, 1F, 0F, 0.1D);
            }

            StopRecordingSampleTrigger();
        }

        /// <summary>
        /// Gets the sample mixer volume.
        /// </summary>
        public decimal GetSamplerMixerVolume()
        {
            return _samplerMixer.GetVolume();
        }

        /// <summary>
        /// Sets the sample mixer volume.
        /// </summary>
        /// <param name="volume">The volume as a value between 0 and 100.</param>
        public void SetSamplerMixerVolume(decimal volume)
        {
            _samplerMixer.SetVolume(volume);
        }

        /// <summary>
        /// Gets or sets the sampler output.
        /// </summary>
        public SoundOutput SamplerOutput
        {
            get { return _samplerOutputSplitter.SoundOutput; }
            set { _samplerOutputSplitter.SoundOutput = value; }
        }

        /// <summary>
        /// Gets or sets the track output.
        /// </summary>
        public SoundOutput TrackOutput
        {
            get { return _trackOutputSplitter.SoundOutput; }
            set { _trackOutputSplitter.SoundOutput = value; }
        }

        #endregion

        #region Internal Classes

        /// <summary>
        /// Enum style representing different types of sample sync events.
        /// </summary>
        private enum SampleSyncType
        {
            /// <summary>
            /// sample end sync event type
            /// </summary>
            SampleEnd = 0
        }

        #endregion

        #region Private Methods

        private void InitialiseSampler()
        {
            DebugHelper.WriteLine("InitialiseSampler");

            // create mixer channel
            _samplerMixer = new MixerChannel(this, MixerChannelOutputType.SingleOutput);
            _samplerMixer.SetVolume((decimal)DefaultFadeOutStartVolume);
            _samplerMixer.CutBass();

            _samplerOutputSplitter = new OutputSplitter(_samplerMixer, _speakerOutput, _monitorOutput);

            // delete any temporary sample files
            _tempSamplerFolder = Path.Combine(Path.GetTempPath(), "Sampler");
            if (!Directory.Exists(_tempSamplerFolder))
                Directory.CreateDirectory(_tempSamplerFolder);

            try
            {
                FileSystemHelper.DeleteFiles(_tempSamplerFolder);
            }
            catch { }

            DebugHelper.WriteLine("END InitialiseSampler");
        }

        private Sample LoadSampleAudioData(Sample sample)
        {
            return LoadSampleAudioData(sample, AudioDataMode.LoadIntoMemory);
        }

        /// <summary>
        /// Loads the sample audio data.
        /// </summary>
        /// <param name="sample">The sample to load.</param>
        /// <returns>The loaded sample</returns>
        private Sample LoadSampleAudioData(Sample sample, AudioDataMode mode)
        {
            lock (sample)
            {
                // abort if audio data already loaded
                if (sample.Channel != int.MinValue) return sample;

                DebugHelper.WriteLine("Loading sample Audio Data " + sample.Description);

                if (mode == AudioDataMode.LoadIntoMemory)
                {
                    sample.AudioData = File.ReadAllBytes(sample.Filename);
                    sample.AudioDataHandle = GCHandle.Alloc(sample.AudioData, GCHandleType.Pinned);
                    sample.AddChannel(Bass.BASS_StreamCreateFile(sample.AudioDataPointer, 0, sample.AudioData.Length,
                        BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_STREAM_PRESCAN));
                }
                else
                {
                    sample.AddChannel(Bass.BASS_StreamCreateFile(sample.Filename, 0, 0,
                        BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_STREAM_PRESCAN));
                }

                if (sample.Channel == 0)
                {
                    var errorCode = Bass.BASS_ErrorGetCode();
                    throw new Exception("Cannot load sample " + sample.Filename + ". Error code: " + errorCode.ToString());
                }

                sample.AddChannel(BassFx.BASS_FX_ReverseCreate(sample.Channel, 1, BASSFlag.BASS_STREAM_DECODE));
                if (sample.Channel == 0) throw new Exception("Cannot load sample " + sample.Filename);
                Bass.BASS_ChannelSetAttribute(sample.Channel, BASSAttribute.BASS_ATTRIB_REVERSE_DIR, (float)BASSFXReverse.BASS_FX_RVS_FORWARD);

                sample.AddChannel(BassFx.BASS_FX_TempoCreate(sample.Channel, BASSFlag.BASS_FX_FREESOURCE | BASSFlag.BASS_STREAM_DECODE));
                if (sample.Channel == 0) throw new Exception("Cannot load sample " + sample.Filename);

                sample.Length = Bass.BASS_ChannelGetLength(sample.Channel);
                sample.DefaultSampleRate = BassHelper.GetSampleRate(sample.Channel);

                Bass.BASS_ChannelSetPosition(sample.Channel, 0);
            }

            return sample;
        }

        /// <summary>
        /// Adds to the mixer channel, and sets the sync points.
        /// </summary>
        /// <param name="sample">The sample to sync.</param>
        private void AddSampleToSampler(Sample sample)
        {
            if (sample == null) return;

            DebugHelper.WriteLine("Add sample to sampler " + sample.Description);

            // load audio data if not loaded
            if (sample.Channel == int.MinValue)
            {
                LoadSampleAudioData(sample);
            }

            lock (_mixerLock)
            {
                BassHelper.AddSampleToSampler(sample, _samplerMixer.InternalChannel);
            }

            // set sample sync event
            sample.SampleSync = new SYNCPROC(OnSampleSync);

            SetSampleSyncPositions(sample);
        }

        /// <summary>
        /// Sets the sample sync positions.
        /// </summary>
        /// <param name="sample">The sample.</param>
        private void SetSampleSyncPositions(Sample sample)
        {
            if (sample == null) return;

            ClearSampleSyncPositions(sample);

            DebugHelper.WriteLine("Set sample sync postions " + sample.Description);

            // set end sample sync
            SetSampleSync(sample, sample.Length - 2000, SampleSyncType.SampleEnd);
        }

        /// <summary>
        /// Sets a sample sync.
        /// </summary>
        /// <param name="sample">The sample.</param>
        /// <param name="position">The position.</param>
        /// <param name="syncType">Type of the sync.</param>
        private void SetSampleSync(Sample sample, long position, SampleSyncType syncType)
        {
            if (sample == null || sample.Channel == int.MinValue) return;

            var flags = BASSSync.BASS_SYNC_POS | BASSSync.BASS_SYNC_MIXTIME;

            var syncId = BassMix.BASS_Mixer_ChannelSetSync(sample.Channel,
                flags,
                position,
                sample.SampleSync,
                new IntPtr((int)syncType));

            if (syncType == SampleSyncType.SampleEnd) sample.SampleEndSyncId = syncId;
        }

        /// <summary>
        /// Clears the sample sync positions.
        /// </summary>
        /// <param name="sample">The sample.</param>
        private void ClearSampleSyncPositions(Sample sample)
        {
            DebugHelper.WriteLine("Clear sample sync postions " + sample.Description);

            if (sample == null || sample.Channel == int.MinValue || sample.SampleEndSyncId == int.MinValue) return;

            BassMix.BASS_Mixer_ChannelRemoveSync(sample.Channel, sample.SampleEndSyncId);
            sample.SampleEndSyncId = int.MinValue;
        }

        private void LoopSample(Sample sample)
        {
            if (sample == null || sample.Channel == int.MinValue) return;
            lock (_samplePlayLock)
            {
                if (sample.Channel != int.MinValue) BassMix.BASS_Mixer_ChannelPause(sample.Channel);
                if (sample.Channel != int.MinValue) Bass.BASS_ChannelSetPosition(sample.Channel, 1);
                if (sample.Channel != int.MinValue) BassMix.BASS_Mixer_ChannelPlay(sample.Channel);
            }
        }

        #endregion

        #region Properties

        public List<Sample> Samples
        {
            get { return _cachedSamples.ToList(); }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Called when when the track sync event is fired
        /// </summary>
        private void OnSampleSync(int handle, int channel, int data, IntPtr pointer)
        {
            var syncType = (SampleSyncType)(pointer.ToInt32());
            DebugHelper.WriteLine("Sample Sync Fired: " + syncType.ToString());

            if (syncType == SampleSyncType.SampleEnd)
            {
                var sample = Samples.Where(s => s.Channel == channel).FirstOrDefault();
                if (sample != null && sample.Channel != int.MinValue)
                {
                    if (!sample.IsLooped) BassHelper.SetSampleVolume(sample, 0M);
                    LoopSample(sample);
                }
            }
        }

        #endregion
    }
}