﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Halloumi.Shuffler.AudioEngine.Channels;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;
using Halloumi.Shuffler.AudioEngine.Players;

namespace Halloumi.Shuffler.AudioEngine.BassPlayer
{
    public partial class BassPlayer
    {
        private TrackSamplePlayer _samplePlayer;
        private MixerChannel _samplerMixer;

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

        public string LoopFolder
        {
            get { return _samplePlayer.LoopFolder; }
            set { _samplePlayer.LoopFolder = value; }
        }

        public event EventHandler OnTrackSamplesChanged;

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
            if (sample == null) return;

            _samplePlayer.PlaySample(sample.SampleId);
            _samplerMixer.SetPluginBpm();
            if (CurrentTrack == null) return;
            StartRecordingSampleTrigger(sample);
        }

        /// <summary>
        ///     Plays a sample.
        /// </summary>
        /// <param name="sampleIndex">Index of the sample.</param>
        public void PlaySample(int sampleIndex)
        {
            PlaySample(GetSample(sampleIndex));
        }

        public void PlaySample(int trackSample, int sampleIndex)
        {
            PlaySample(GetSample(trackSample, sampleIndex));
        }

        private Sample GetSample(int sampleIndex)
        {
            var samples = _samplePlayer.GetSamples();
            if (sampleIndex >= samples.Count || sampleIndex < 0) return null;
            return samples[sampleIndex];
        }

        private Sample GetSample(int trackIndex, int sampleIndex)
        {
            if (trackIndex == 0)
                return null;

            var samples = _samplePlayer.GetSamples();
            var currentTrack = samples[0].LinkedTrackDescription;
            var nextTrackIndex = samples.IndexOf(samples.FirstOrDefault(x => x.LinkedTrackDescription != currentTrack));

            if (trackIndex == 1 && sampleIndex < nextTrackIndex)
                return GetSample(sampleIndex);
            else if (trackIndex == 2)
                return GetSample(sampleIndex + nextTrackIndex - 1);
            else
                return null;

        }

        /// <summary>
        ///     Plays a sample.
        /// </summary>
        /// <param name="sampleIndex">Index of the sample.</param>
        public void PauseSample(int sampleIndex)
        {
            PauseSample(GetSample(sampleIndex));
        }

        public void PauseSample(int trackIndex, int sampleIndex)
        {
            PauseSample(GetSample(trackIndex, sampleIndex));
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
            if (sample == null)
                return;

            _samplePlayer.PauseSample(sample.SampleId);
            StopRecordingSampleTrigger();
        }


        private void InitialiseSampler()
        {
            // DebugHelper.WriteLine("InitialiseSampler");

            // create mixer channel
            _samplerMixer = new MixerChannel(this);
            _samplerMixer.SetVolume((decimal) DefaultFadeOutStartVolume);
            _samplerMixer.CutBass();
            _samplerOutputSplitter = new OutputSplitter(_samplerMixer, SpeakerOutput, MonitorOutput);

            _samplePlayer = new TrackSamplePlayer(this);
            _samplerMixer.AddInputChannel(_samplePlayer.Output);

            _samplerMixer.SetVolume(50);

            // DebugHelper.WriteLine("END InitialiseSampler");
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

        public void LinkLoopSampleToTrack(string loopKey, Track track)
        {
            var filename = Path.Combine(LoopFolder, loopKey);
            if (!File.Exists(filename) || track == null)
                return;

            var length = AudioStreamHelper.GetLength(filename);

            var attributes = AutomationAttributesHelper.GetAutomationAttributes(track.Description);

            var description = Path.GetFileNameWithoutExtension(filename)
                .Replace("_", " ")
                .Replace("-", " ")
                .Replace(".", " ");


            attributes.LoopSamples.Add(new TrackSample
            {
                Description = description,
                IsExternalLoop = true,
                Key = loopKey,
                Length = length,
                IsLooped = true,
                Start = 0
            });

            AutomationAttributesHelper.SaveAutomationAttributes(track.Description, attributes);

            if (track == CurrentTrack || track == NextTrack)
                Task.Run(() =>
                {
                    _samplePlayer.LoadSamples(CurrentTrack, NextTrack);
                    OnTrackSamplesChanged?.Invoke(this, EventArgs.Empty);
                });
        }
    }
}