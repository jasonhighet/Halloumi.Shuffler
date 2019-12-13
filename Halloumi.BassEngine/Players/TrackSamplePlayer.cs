using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Halloumi.Shuffler.AudioEngine.Channels;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;

namespace Halloumi.Shuffler.AudioEngine.Players
{
    public class TrackSamplePlayer
    {
        private readonly AudioPlayer _audioPlayer;
        private Track _currentTrack;
        private Track _nextTrack;
        public string LoopFolder { get; set; }

        private readonly IBmpProvider _bpmProvider;

        public TrackSamplePlayer(IBmpProvider bpmProvider)
        {
            _audioPlayer = new AudioPlayer(bpmProvider);
            _bpmProvider = bpmProvider;
        }


        public MixerChannel Output
        {
            get { return _audioPlayer.Output; }
        }

        public void LoadSamples(Track currentTrack, Track nextTrack)
        {
            lock (this)
            {
                if (!HaveTracksChanged(currentTrack, nextTrack))
                    return;

                _audioPlayer.UnloadAll();

                _currentTrack = currentTrack;
                _nextTrack = nextTrack;

                LoadSamples(_currentTrack);
                LoadSamples(_nextTrack);
            }

        }

        private void LoadSamples(Track track)
        {
            if (track == null)
                return;

            // DebugHelper.WriteLine("Loading samples for " + track.Description);

            var trackSamples = GetTrackSamples(track);
            foreach (var trackSample in trackSamples)
            {
                LoadSample(track, trackSample);
            }
        }

        private void LoadSample(Track track, TrackSample trackSample)
        {
            var sampleId = track.Description + " - " + trackSample.Key;

            var filename = trackSample.IsExternalLoop ? Path.Combine(LoopFolder, trackSample.Key) : track.Filename;
            if(!File.Exists(filename))
                return;

            var sample = (Sample) _audioPlayer.Load(sampleId, filename);

            sample.LinkedTrackDescription = track.Description;
            sample.Gain = trackSample.IsExternalLoop ? 0 : track.Gain;
            sample.SampleKey = trackSample.Key;
            sample.IsLooped = trackSample.IsExternalLoop || trackSample.IsLooped;
            sample.Bpm = trackSample.CalculateBpm(track);
            sample.Description = trackSample.Description;

            _audioPlayer.AddSection(sampleId, 
                sampleId, 
                start: trackSample.Start, 
                length: trackSample.Length, 
                bpm: sample.Bpm, 
                loopIndefinitely: trackSample.IsLooped);

            _audioPlayer.QueueSection(sampleId, sampleId);
        }


        private static IEnumerable<TrackSample> GetTrackSamples(Track track)
        {
            // DebugHelper.WriteLine("GetTrackSamples for " + track.Description);

            var trackSamples = new List<TrackSample>();
            if (track.UsePreFadeIn)
            {

                trackSamples.Add(new TrackSample
                {
                    IsLooped = false,
                    Start = track.SamplesToSeconds(track.PreFadeInStart),
                    Length = track.PreFadeInLengthSeconds,
                    Description = "Pre Fade In",
                    Key = "PreFadeIn"
                });
            }

            trackSamples.Add(new TrackSample
            {
                IsLooped = track.IsLoopedAtStart,
                Start = track.SamplesToSeconds(track.FadeInStart),
                Length = track.FadeInLengthSeconds,
                Description = "Fade In",
                Key = "FadeIn"
            });

            trackSamples.Add(new TrackSample
            {
                IsLooped = track.IsLoopedAtEnd,
                Start = track.SamplesToSeconds(track.FadeOutStart),
                Length = track.FadeOutLengthSeconds,
                Description = "Fade Out",
                Key = "FadeOut"
            });

            trackSamples.AddRange(GetAdditionalTrackSamples(track.Description));

            return trackSamples;
        }

        private static IEnumerable<TrackSample> GetAdditionalTrackSamples(string trackDescription)
        {
            var additionalSamples = new List<TrackSample>();

            var attributes = AutomationAttributesHelper.GetAutomationAttributes(trackDescription);

            if (attributes?.TrackSamples != null && attributes.TrackSamples.Count > 0)
            {
                additionalSamples.AddRange(attributes.TrackSamples.OrderBy(t => t.Description).ToList());
            }

            if (attributes?.LoopSamples != null && attributes.LoopSamples.Count > 0)
            {
                additionalSamples.AddRange(attributes.LoopSamples.OrderBy(t => t.Description).ToList());
            }

            return additionalSamples;
        }

        public void PlaySample(string sampleId)
        {
            var section = _audioPlayer.GetAudioSection(sampleId, sampleId);
            if(section == null)
                return;

            _audioPlayer.Pause(sampleId);
            _audioPlayer.SetSectionBpm(sampleId, sampleId, section.Bpm, targetBpm: _bpmProvider.GetCurrentBpm());

            _audioPlayer.QueueSection(sampleId, sampleId);
            _audioPlayer.Unmute(sampleId);
            _audioPlayer.Play(sampleId);
        }

        public void PauseSample(string sampleId)
        {
            var section = _audioPlayer.GetAudioSection(sampleId, sampleId);
            if (section == null)
                return;

            _audioPlayer.SetSectionBpm(sampleId, sampleId, section.Bpm, targetBpm: _bpmProvider.GetCurrentBpm());
            _audioPlayer.Mute(sampleId);
        }
        
        public void PauseAllSamples()
        {
            _audioPlayer.Pause();
        }

        public List<Sample> GetSamples()
        {
            return _audioPlayer.GetAudioStreams().Select(stream => (Sample) stream).ToList();
        }

        public bool HaveTracksChanged(Track currentTrack, Track nextTrack)
        {
            return !(TrackHelper.IsSameTrack(_currentTrack, currentTrack)
                     && TrackHelper.IsSameTrack(_nextTrack, nextTrack));
        }
    }
}