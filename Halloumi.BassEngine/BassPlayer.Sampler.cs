using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioEngine.Channels;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;
using Halloumi.Shuffler.AudioEngine.Players;

using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;

namespace Halloumi.Shuffler.AudioEngine
{
    public partial class BassPlayer
    {
        private MixerChannel _samplerMixer;

        private TrackSamplePlayer _samplePlayer;

        private OutputSplitter _samplerOutputSplitter;

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


        /// <summary>
        ///     Unloads the samples.
        /// </summary>
        public void UnloadSamples()
        {
            _samplePlayer.UnloadAll();
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
            return _samplePlayer.GetSamples().FirstOrDefault(s => s.SampleId == sampleId);
        }

        /// <summary>
        ///     Plays a sample.
        /// </summary>
        /// <param name="sample">The sample.</param>
        public void PlaySample(Sample sample)
        {
            if(sample == null) return;
            
            _samplePlayer.PlaySample(sample.SampleId);
            _samplerMixer.SetPluginBpm();
            if (CurrentTrack == null) return;
            StartRecordingSampleTrigger(sample);
        }

        /// <summary>
        /// Plays a sample.
        /// </summary>
        /// <param name="sampleIndex">Index of the sample.</param>
        public void PlaySample(int sampleIndex)
        {
            PlaySample(GetSample(sampleIndex));
        }

        private Sample GetSample(int sampleIndex)
        {
            var samples = _samplePlayer.GetSamples();
            if (sampleIndex >= samples.Count || sampleIndex < 0) return null;
            return samples[sampleIndex];
        }

        /// <summary>
        /// Plays a sample.
        /// </summary>
        /// <param name="sampleIndex">Index of the sample.</param>
        public void PauseSample(int sampleIndex)
        {
            PauseSample(GetSample(sampleIndex));
        }

        /// <summary>
        ///     Stops the samples.
        /// </summary>
        public void StopSamples()
        {
            _samplePlayer.PauseAllSamples();
        }

        /// <summary>
        ///     Mutes the sample.
        /// </summary>
        /// <param name="sample">The sample.</param>
        public void PauseSample(Sample sample)
        {
            _samplePlayer.PauseSample(sample.SampleId);
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
            OnSamplerMixerVolumeChanged?.Invoke(CurrentTrack, EventArgs.Empty);
        }

        private void InitialiseSampler()
        {
            DebugHelper.WriteLine("InitialiseSampler");

            // create mixer channel
            _samplerMixer = new MixerChannel(this);
            _samplerMixer.SetVolume((decimal)DefaultFadeOutStartVolume);
            _samplerMixer.CutBass();
            _samplerOutputSplitter = new OutputSplitter(_samplerMixer, _speakerOutput, _monitorOutput);

            _samplePlayer = new TrackSamplePlayer(this);
            _samplerMixer.AddInputChannel(_samplePlayer.Output);

            DebugHelper.WriteLine("END InitialiseSampler");
        }

        private void RefreshSamples()
        {
            if (!_samplePlayer.HaveTracksChanged(CurrentTrack, NextTrack)) return;
            Task.Run(() => 
            {
                _samplePlayer.LoadSamples(CurrentTrack, NextTrack);
                OnTrackSamplesChanged?.Invoke(this, EventArgs.Empty);
            });
        }

        public List<Sample> GetSamples()
        {
            return _samplePlayer.GetSamples();
        }
    }
}