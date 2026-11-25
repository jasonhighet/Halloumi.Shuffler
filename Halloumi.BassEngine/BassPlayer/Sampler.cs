using System;
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
    /// <summary>
    ///     Plays the samples linked to the current and next tracks on the sampler mixer, and records
    ///     each sample played over a track as a sample trigger.
    /// </summary>
    internal class Sampler
    {
        private readonly MixingDesk _desk;
        private readonly TrackDeck _deck;
        private readonly TriggerRecorder _recorder;
        private readonly TrackSamplePlayer _samplePlayer;

        public Sampler(IBpmProvider bpmProvider, MixingDesk desk, TrackDeck deck, TriggerRecorder recorder)
        {
            _desk = desk;
            _deck = deck;
            _recorder = recorder;

            _samplePlayer = new TrackSamplePlayer(bpmProvider);
            _desk.SamplerMixer.AddInputChannel(_samplePlayer.Output);
        }

        /// <summary>
        ///     Raised (on a worker thread) when the loaded samples change.
        /// </summary>
        public event EventHandler SamplesChanged;

        public string LoopFolder
        {
            get { return _samplePlayer.LoopFolder; }
            set { _samplePlayer.LoopFolder = value; }
        }

        /// <summary>
        ///     The delay length in notes for BPM-synced plug-ins on the sampler.
        /// </summary>
        public decimal DelayNotes
        {
            get { return _desk.SamplerMixer.SyncNotes; }
            set { _desk.SamplerMixer.SyncNotes = value; }
        }

        public List<Sample> GetSamples()
        {
            return _samplePlayer.GetSamples();
        }

        public Sample GetSampleById(string sampleId)
        {
            return _samplePlayer.GetSamples().FirstOrDefault(s => s.SampleId == sampleId);
        }

        /// <summary>
        ///     Gets a sample by its position in the list of loaded samples.
        /// </summary>
        public Sample GetSample(int sampleIndex)
        {
            var samples = _samplePlayer.GetSamples();
            if (sampleIndex >= samples.Count || sampleIndex < 0) return null;
            return samples[sampleIndex];
        }

        /// <summary>
        ///     Gets a sample by track (1 = the current track's samples, 2 = the next track's) and its
        ///     position within that track's samples.
        /// </summary>
        public Sample GetSample(int trackIndex, int sampleIndex)
        {
            if (trackIndex == 0)
                return null;

            var samples = _samplePlayer.GetSamples();
            var currentTrack = samples[0].LinkedTrackDescription;
            var nextTrackIndex = samples.IndexOf(samples.FirstOrDefault(x => x.LinkedTrackDescription != currentTrack));

            if (trackIndex == 1 && sampleIndex < nextTrackIndex)
                return GetSample(sampleIndex);
            if (trackIndex == 2)
                return GetSample(sampleIndex + nextTrackIndex - 1);
            return null;
        }

        /// <summary>
        ///     Plays a sample and starts recording it as a sample trigger on the current track.
        /// </summary>
        public void Play(Sample sample)
        {
            if (sample == null) return;

            _samplePlayer.PlaySample(sample.SampleId);
            _desk.SamplerMixer.SetPluginBpm();

            var track = _deck.Current;
            if (track == null) return;

            _recorder.StartSample(track, _deck.Previous, _deck.Next, AudioStreamHelper.GetPosition(track),
                DelayNotes, sample.SampleId);
        }

        /// <summary>
        ///     Stops a sample and finishes recording its sample trigger.
        /// </summary>
        public void Pause(Sample sample)
        {
            if (sample == null) return;

            _samplePlayer.PauseSample(sample.SampleId);

            if (_deck.Current == null || _recorder.SampleTrack == null) return;
            _recorder.EndSample(AudioStreamHelper.GetPosition(_recorder.SampleTrack));
        }

        public void StopAll()
        {
            _samplePlayer.PauseAllSamples();
        }

        /// <summary>
        ///     Reloads the samples (in the background) if the current or next track has changed.
        /// </summary>
        public void Refresh()
        {
            if (!_samplePlayer.HaveTracksChanged(_deck.Current, _deck.Next)) return;
            ReloadInBackground();
        }

        /// <summary>
        ///     Links a loop file in the loop folder to a track as one of its samples.
        /// </summary>
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

            if (track == _deck.Current || track == _deck.Next)
                ReloadInBackground();
        }

        private void ReloadInBackground()
        {
            var current = _deck.Current;
            var next = _deck.Next;
            Task.Run(() =>
            {
                _samplePlayer.LoadSamples(current, next);
                SamplesChanged?.Invoke(this, EventArgs.Empty);
            });
        }
    }
}
