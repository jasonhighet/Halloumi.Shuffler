using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using Un4seen.Bass.AddOn.WaDsp;
using Un4seen.Bass.AddOn.Vst;
using Halloumi.Common.Helpers;
using System.Threading;
using Un4seen.Bass.AddOn.Mix;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Fx;
using System.Runtime.InteropServices;

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
        /// The next available sample ID
        /// </summary>
        private static int _nextSampleID = 0;

        /// <summary>
        /// Channel ID for the sample mixer channel
        /// </summary>
        private int _sampleMixerChannel = int.MinValue;

        #endregion

        #region Public Methods
                

        public void UnloadSamples()
        {
            // unload all samples
            foreach (var sample in _cachedSamples)
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
            if (sample.Channel == int.MinValue) return;

            Debug.Print("Unloading sample " + sample.Description);

            BassMix.BASS_Mixer_ChannelRemove(sample.Channel);
            Bass.BASS_StreamFree(sample.Channel);
            sample.Channel = int.MinValue;

            sample.AudioDataHandle.Free();
            sample.AudioData = null;
        }

        public Sample LoadSample(string filename)
        {
            return LoadSample(filename, "");
        }

        public Sample LoadSample(string filename, string description)
        {
            if (_cachedSamples.Exists(t => t.Filename.ToLower() == filename.ToLower()))
            {
                return _cachedSamples.Where(t => t.Filename.ToLower() == filename.ToLower()).FirstOrDefault();
            }

            Debug.Print("Loading sample " + filename);

            if (!File.Exists(filename)) throw new Exception("Cannot find file " + filename);

            var sample = new Sample();
            sample.ID = _nextSampleID++;
            sample.Filename = filename;

            if (description == "") description = StringHelper.TitleCase(Path.GetFileNameWithoutExtension(sample.Filename).Replace("_", " ").Replace("-", " "));
            sample.Description = description;

            LoadSampleAudioData(sample);
            AddSampleToMixer(sample);

            _cachedSamples.Add(sample);
            return sample;
        }

        public void PlaySample(Sample sample)
        {
            BassMix.BASS_Mixer_ChannelPause(sample.Channel);
            
            Bass.BASS_ChannelSetPosition(sample.Channel, 1);

            BassMix.BASS_Mixer_ChannelPlay(sample.Channel);
        }

        public void StopSample(Sample sample)
        {
            BassMix.BASS_Mixer_ChannelPause(sample.Channel);
        }

        #endregion

        #region Private Methods

        private void InitialiseSampleMixer()
        {
            // create mixer channel
            _sampleMixerChannel = BassHelper.IntialiseMixerChannel();

            // add to list of active mixers
            _activeMixers.Add(_sampleMixerChannel, _sampleMixerChannel);

            Bass.BASS_ChannelSetSync(_sampleMixerChannel, BASSSync.BASS_SYNC_STALL, 0L, _mixerStall, IntPtr.Zero);
            Bass.BASS_ChannelPlay(_sampleMixerChannel, false);
        }

        /// <summary>
        /// Loads the sample audio data.
        /// </summary>
        /// <param name="sample">The sample to load.</param>
        /// <returns>The loaded sample</returns>
        private Sample LoadSampleAudioData(Sample sample)
        {
            // abort if audio data already loaded
            if (sample.Channel != int.MinValue) return sample;

            Debug.Print("Loading sample Audio Data " + sample.Description);

            sample.AudioData = File.ReadAllBytes(sample.Filename);
            sample.AudioDataHandle = GCHandle.Alloc(sample.AudioData, GCHandleType.Pinned);
            sample.Channel = Bass.BASS_StreamCreateFile(sample.AudioDataPointer, 0, sample.AudioData.Length, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_STREAM_PRESCAN);

            if (sample.Channel == 0)
            {
                var errorCode = Bass.BASS_ErrorGetCode();
                throw new Exception("Cannot load sample " + sample.Filename + ". Error code: " + errorCode.ToString());
            }

            sample.Channel = BassFx.BASS_FX_ReverseCreate(sample.Channel, 1, BASSFlag.BASS_STREAM_DECODE);
            if (sample.Channel == 0) throw new Exception("Cannot load sample " + sample.Filename);
            Bass.BASS_ChannelSetAttribute(sample.Channel, BASSAttribute.BASS_ATTRIB_REVERSE_DIR, (float)BASSFXReverse.BASS_FX_RVS_FORWARD);

            sample.Channel = BassFx.BASS_FX_TempoCreate(sample.Channel, BASSFlag.BASS_FX_FREESOURCE | BASSFlag.BASS_STREAM_DECODE);
            if (sample.Channel == 0) throw new Exception("Cannot load sample " + sample.Filename);

            sample.Length = Bass.BASS_ChannelGetLength(sample.Channel);

            sample.DefaultSampleRate = BassHelper.GetSampleRate(sample.Channel);

            return sample;
        }

        /// <summary>
        /// Adds to the mixer channel, and sets the sync points.
        /// </summary>
        /// <param name="sample">The sample to sync.</param>
        private void AddSampleToMixer(Sample sample)
        {
            if (sample == null) return;

            Debug.Print("Add sample to mixer " + sample.Description);

            // load audio data if not loaded
            if (sample.Channel == int.MinValue)
            {
                LoadSampleAudioData(sample);
            }

            //if (sample != this.PreviousSample && sample != this.CurrentSample)
            //{
                // add the new sample to the mixer (in paused mode)
                //BassMix.BASS_Mixer_StreamAddChannel(_mixerChannel, sample.Channel, BASSFlag.BASS_MIXER_PAUSE | BASSFlag.BASS_MIXER_DOWNMIX | BASSFlag.BASS_STREAM_AUTOFREE);
            BassMix.BASS_Mixer_StreamAddChannel(_sampleMixerChannel, sample.Channel, BASSFlag.BASS_MIXER_PAUSE | BASSFlag.BASS_MIXER_DOWNMIX);

                //// set sample sync event
                //sample.SampleSync = new SYNCPROC(OnSampleSync);

                //// set replay gain
                //BassHelper.SetSampleReplayGain(sample.Channel, sample.Gain);
            //}
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
            SyncType syncType = (SyncType)(pointer.ToInt32());

            Debug.WriteLine("Event Fired: " + syncType.ToString());
        }

        #endregion

    }
}
