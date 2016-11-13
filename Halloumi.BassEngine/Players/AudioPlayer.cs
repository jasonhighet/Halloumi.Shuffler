using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioEngine.Channels;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;

namespace Halloumi.Shuffler.AudioEngine.Players
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public class AudioPlayer
    {
        private readonly List<Event> _audioStreamEvents = new List<Event>();
        private readonly List<AudioStreamSection> _streamSections;

        private DateTime _lastEvent = DateTime.Now;

        public AudioPlayer(IBmpProvider bpmProvider = null)
        {
            Output = new MixerChannel(bpmProvider);
            _streamSections = new List<AudioStreamSection>();
        }

        public MixerChannel Output { get; }

        public AudioStream Load(string streamKey, string filename)
        {
            if (!File.Exists(filename))
                throw new Exception("Cannot find file " + filename);
            if (GetStreamSection(streamKey) != null)
                throw new Exception("AudioStream already exists with streamKey " + streamKey);

            var audioStream = new Sample
            {
                Filename = filename
            };

            var tags = TagHelper.LoadTags(filename);
            if (tags != null)
            {
                if (tags.Gain.HasValue)
                    audioStream.Gain = tags.Gain.Value;
                if (tags.Bpm.HasValue)
                    audioStream.Bpm = tags.Bpm.Value;

                audioStream.Description = TrackHelper.GuessTrackDescription(filename, tags.Artist, tags.Title);
            }

            AudioStreamHelper.LoadAudio(audioStream);

            audioStream.SyncProc = OnSync;
            AudioStreamHelper.AddToMixer(audioStream, Output.ChannelId);
            AudioStreamHelper.SetPosition(audioStream, 0);

            lock (_streamSections)
            {
                _streamSections.Add(new AudioStreamSection
                {
                    Key = streamKey,
                    AudioStream = audioStream,
                    AudioSections = new List<AudioSection>()
                });
            }

            return audioStream;
        }

        public void Unload(string streamKey)
        {
            var streamSection = GetStreamSection(streamKey);
            if (streamSection == null)
                return;

            AudioStreamHelper.Pause(streamSection.AudioStream);

            lock (_audioStreamEvents)
            {
                _audioStreamEvents.RemoveAll(x => x.StreamKey == streamKey);
            }

            lock (streamSection)
            {
                foreach (var sync in streamSection.AudioSections.SelectMany(section => section.AudioSyncs))
                {
                    RemoveSyncFromStream(streamSection.AudioStream, sync);
                }
            }

            lock (streamSection.AudioStream)
            {
                foreach (var sync in streamSection.AudioStream.AudioSyncs)
                {
                    RemoveSyncFromStream(streamSection.AudioStream, sync);
                }
                streamSection.AudioStream.AudioSyncs.Clear();
            }

            AudioStreamHelper.RemoveFromMixer(streamSection.AudioStream, Output.ChannelId);
            AudioStreamHelper.UnloadAudio(streamSection.AudioStream);

            streamSection.AudioStream.SyncProc = null;

            lock (_streamSections)
            {
                _streamSections.Remove(streamSection);
            }
        }


        private AudioStreamSection GetStreamSection(string streamKey)
        {
            lock (_streamSections)
            {
                return _streamSections.FirstOrDefault(x => x.Key == streamKey);
            }
        }

        public void UnloadAll()
        {
            Pause();
            var keys = GetStreamKeys();
            foreach (var streamKey in keys)
            {
                Unload(streamKey);
            }
        }

        // ReSharper disable once ReturnTypeCanBeEnumerable.Local
        /// <summary>
        ///     Gets the keys of all loaded streams
        /// </summary>
        /// <returns>the keys of all loaded streams</returns>
        public IList<string> GetStreamKeys()
        {
            lock (_streamSections)
            {
                return _streamSections.Select(x => x.Key).ToList();
            }
        }

        public void Play(string streamKey)
        {
            var audioStream = GetAudioStream(streamKey);

            ApplySyncIfOneExistsForCurrentPosition(audioStream);

            AudioStreamHelper.Play(audioStream);
        }

        private void ApplySyncIfOneExistsForCurrentPosition(AudioStream audioStream)
        {
            var position = AudioStreamHelper.GetPosition(audioStream);
            var sync = GetAudioSync(audioStream, position);
            if (sync != null && sync.SyncType != SyncType.AudioStreamEvent)
            {
                OnSync(sync.Id, audioStream.Channel, int.MinValue, IntPtr.Zero);
            }
        }

        public void Play(IEnumerable<string> streamKeys)
        {
            foreach (var streamKey in streamKeys)
            {
                Play(streamKey);
            }
        }

        public List<AudioStream> GetAudioStreams()
        {
            return GetStreamKeys().Select(GetAudioStream).ToList();
        }


        public AudioStream GetAudioStream(string streamKey)
        {
            lock (_streamSections)
            {
                return _streamSections.FirstOrDefault(x => x.Key == streamKey)?.AudioStream;
            }
        }

        public void Unmute(string streamKey)
        {
            var audioStream = GetAudioStream(streamKey);
            AudioStreamHelper.SetVolume(audioStream, 100M);
        }

        public void Mute(string streamKey)
        {
            var audioStream = GetAudioStream(streamKey);

            var volume = AudioStreamHelper.GetVolume(audioStream);
            if (volume != 0)
            {
                AudioStreamHelper.SetVolumeSlide(audioStream, 1F, 0F, 0.1D);
            }
        }

        public void Pause()
        {
            var streamKeys = GetStreamKeys();
            foreach (var streamKey in streamKeys)
            {
                var audioStream = GetAudioStream(streamKey);
                AudioStreamHelper.Pause(audioStream);
            }
        }

        public void Pause(string streamKey)
        {
            var audioStream = GetAudioStream(streamKey);
            AudioStreamHelper.Pause(audioStream);
        }

        public AudioSection AddSection(string streamKey,
            string sectionKey,
            double start = 0,
            double length = 0,
            double offset = 0,
            decimal bpm = 0,
            bool calculateBpmFromLength = false,
            decimal targetBpm = 0,
            bool loopIndefinitely = false,
            bool autoPlayNextSection = false)
        {
            var section = CreateSection(streamKey, sectionKey, loopIndefinitely, autoPlayNextSection);
            SetSectionPositions(streamKey, sectionKey, start, length, offset);

            if (bpm == 0 && targetBpm > 0)
                calculateBpmFromLength = true;

            SetSectionBpm(streamKey, sectionKey, bpm, calculateBpmFromLength, targetBpm);
            return section;
        }

        private AudioSection CreateSection(string streamKey, string sectionKey, bool loopIndefinitely,
            bool autoPlayNextSection)
        {
            var streamSection = GetStreamSection(streamKey);
            if (streamSection == null)
                return null;

            var audioSection = GetAudioSection(streamKey, sectionKey);
            if (audioSection != null)
                return audioSection;

            audioSection = new AudioSection
            {
                Key = sectionKey,
                LoopIndefinitely = loopIndefinitely,
                AutoPlayNextSection = autoPlayNextSection
            };

            lock (streamSection)
            {
                streamSection.AudioSections.Add(audioSection);
            }

            return audioSection;
        }

        public AudioSection GetAudioSection(string streamKey, string sectionKey)
        {
            var streamSection = GetStreamSection(streamKey);
            if (streamSection == null) return null;

            lock (streamSection)
            {
                return streamSection.AudioSections.FirstOrDefault(x => x.Key == sectionKey);
            }
        }

        public void SetSectionPositions(string streamKey, string sectionKey, double start = 0, double length = 0,
            double offset = 0)
        {
            var audioStream = GetAudioStream(streamKey);
            if (audioStream == null)
                return;

            var audioSection = GetAudioSection(streamKey, sectionKey);
            if (audioSection == null)
                return;

            if (length == 0)
                length = audioStream.LengthSeconds - start;

            SetSync(audioStream, audioSection, SyncType.Start, start);
            SetSync(audioStream, audioSection, SyncType.End, start + length);

            if (offset == 0) offset = double.MinValue;
            SetSync(audioStream, audioSection, SyncType.Offset, offset);

            audioSection.Length = length;
        }

        public void SetSectionBpm(string streamKey, string sectionKey, decimal bpm = 0,
            bool calculateBpmFromLength = false, decimal targetBpm = 0)
        {
            var audioStream = GetAudioStream(streamKey);
            if (audioStream == null)
                return;

            var audioSection = GetAudioSection(streamKey, sectionKey);
            if (audioSection == null)
                return;

            if (calculateBpmFromLength)
            {
                var length = audioStream.SamplesToSeconds(audioSection.End.Position - audioSection.Start.Position);
                audioSection.Bpm = BpmHelper.GetBpmFromLoopLength(length);
            }
            else if (bpm != 0)
                audioSection.Bpm = bpm;
            else if (audioSection.Bpm != 0)
                audioSection.Bpm = 100;

            audioSection.TargetBpm = targetBpm;
        }

        public void AddEvent(string streamKey, double position, string targetStreamKey, string targetSectionKey,
            EventType eventType, AudioPlayer player = null, double length = 0)
        {
            var audioStream = GetAudioStream(streamKey);
            if (audioStream == null)
                return;
            if (position == double.MinValue) return;

            if (player == null)
                player = this;

            var audioSync = GetAudioSync(audioStream, position)
                            ?? AddSync(audioStream, SyncType.AudioStreamEvent, position);

            var audioStreamEvent = new Event
            {
                SyncId = audioSync.Id,
                StreamKey = streamKey,
                TargetStreamKey = targetStreamKey,
                TargetSectionKey = targetSectionKey,
                StreamEventType = eventType,
                Player = player,
                Length = length
            };

            lock (_audioStreamEvents)
            {
                _audioStreamEvents.Add(audioStreamEvent);
            }

            //var message = $"Added {eventType} on {streamKey} to {targetStreamKey} at {position}";
            //DebugHelper.WriteLine(message);
        }


        private static AudioSync GetAudioSync(AudioStream audioStream, double position)
        {
            var samplePosition = GetSamplePosition(audioStream, position);
            return GetAudioSync(audioStream, samplePosition);
        }

        private static AudioSync GetAudioSync(AudioStream audioStream, long samplePosition)
        {
            return audioStream
                .AudioSyncs
                .FirstOrDefault(x => x.Position == samplePosition);
        }

        private List<Event> GetAudioStreamEvents(int syncId)
        {
            lock (_audioStreamEvents)
            {
                return _audioStreamEvents.Where(x => x.SyncId == syncId).ToList();
            }
        }


        public void QueueSection(string streamKey, string sectionKey)
        {
            var audioStream = GetAudioStream(streamKey);
            if (audioStream == null)
                throw new Exception("Cannot find stream " + streamKey);

            var audioSection = GetAudioSection(streamKey, sectionKey);
            if (audioSection == null)
                throw new Exception("Cannot find section " + sectionKey + " for stream " + streamKey);

            if (!audioSection.HasStartAndEnd)
                throw new Exception("Cannot find start and end for section " + sectionKey + " for stream " + streamKey);

            var startPosition = audioSection.HasOffset
                ? audioSection.Offset.Position + 500
                : audioSection.Start.Position;

            AudioStreamHelper.SetPosition(audioStream, startPosition);
            SetSectionTempo(audioStream, audioSection);
        }

        private static void SetSectionTempo(AudioStream audioStream, AudioSection audioSection)
        {
            if (audioSection.TargetBpm != 0)
                AudioStreamHelper.SetTempoToMatchBpm(audioStream, audioSection.Bpm, audioSection.TargetBpm);
        }

        private static void SetSync(AudioStream audioStream, AudioSection audioSection, SyncType syncType,
            double position)
        {
            if (syncType == SyncType.AudioStreamEvent)
                return;

            var audioSync = GetAudioSync(audioSection, syncType);
            if (audioSync != null)
            {
                RemoveSyncFromStream(audioStream, audioSync);
                lock (audioSection)
                {
                    audioSection.AudioSyncs.Remove(audioSync);
                }
            }

            audioSync = AddSync(audioStream, syncType, position);
            lock (audioSection)
            {
                if (audioSync != null)
                    audioSection.AudioSyncs.Add(audioSync);
            }
        }

        private static AudioSync AddSync(AudioStream audioStream, SyncType syncType, double position)
        {
            if (position == double.MinValue) return null;

            var samplePosition = GetSamplePosition(audioStream, position);
            var audioSync = new AudioSync {SyncType = syncType, Position = samplePosition};
            AddSyncToStream(audioStream, audioSync);

            return audioSync;
        }

        private static long GetSamplePosition(AudioStream audioStream, double position)
        {
            if (position < 0) position = 0;
            var samplePosition = audioStream.SecondsToSamples(position);
            var maxSample = audioStream.Length - 500;
            if (samplePosition > maxSample)
                samplePosition = maxSample;
            return samplePosition;
        }

        private static AudioSync GetAudioSync(AudioSection audioSection, SyncType syncType)
        {
            lock (audioSection)
            {
                return audioSection.AudioSyncs.FirstOrDefault(x => x.SyncType == syncType);
            }
        }

        private void OnSync(int syncId, int channel, int data, IntPtr pointer)
        {
            Task.Run(() => OnSync(syncId, channel));
        }

        private void OnSync(int syncId, int channel)
        {
            //var currentEvent = DateTime.Now;
            //var timeSpan = currentEvent - _lastEvent;
            //var secondsSinceLastEvent = Math.Round(timeSpan.TotalSeconds, 4);
            //_lastEvent = currentEvent;

            ////DebugHelper.WriteLine("start onnsyc " + streamSection.Key + " [ " + audioSync.SyncType + "]");
            //DebugHelper.WriteLine(secondsSinceLastEvent);

            var streamSection = GetStreamSectionByChannel(channel);
            var audioSync = GetAudioSyncById(channel, syncId);
            if (audioSync == null) return;

            var audioSection = GetAudioSectionBySync(channel, audioSync);
            switch (audioSync.SyncType)
            {
                case SyncType.End:
                    OnSectionEnd(audioSection, streamSection);
                    break;
                case SyncType.Offset:
                    OnSectionOffset(audioSection, streamSection);
                    break;
                case SyncType.Start:
                    break;
                case SyncType.AudioStreamEvent:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // DebugHelper.WriteLine("end onnsyc for " + streamSection.Key + " " + audioSync.SyncType);

            OnEventSync(syncId);
        }

        private void OnEventSync(int syncId)
        {
            var audioStreamEvents = GetAudioStreamEvents(syncId);
            if (audioStreamEvents == null || audioStreamEvents.Count == 0)
                return;

            var players = audioStreamEvents.Select(x => x.Player).Distinct();
            ParallelHelper.ForEach(players, player =>
            {
                var playerEvents = audioStreamEvents.Where(x => x.Player == player);
                foreach (var audioEvent in playerEvents)
                {
                    //DebugHelper.WriteLine("start event sycn:" + audioEvent.StreamEventType + " " + syncId);
                    AudioStream stream;
                    switch (audioEvent.StreamEventType)
                    {
                        case EventType.PlaySolo:
                            player.Pause();
                            player.QueueSection(audioEvent.TargetStreamKey, audioEvent.TargetSectionKey);
                            player.Play(audioEvent.TargetStreamKey);
                            break;
                        case EventType.Play:
                            player.Pause(audioEvent.TargetStreamKey);
                            player.QueueSection(audioEvent.TargetStreamKey, audioEvent.TargetSectionKey);
                            player.Play(audioEvent.TargetStreamKey);
                            break;
                        case EventType.PauseAll:
                            player.Pause();
                            break;
                        case EventType.Pause:
                            player.Pause(audioEvent.TargetStreamKey);
                            break;
                        case EventType.FadeIn:
                            stream = player.GetAudioStream(audioEvent.TargetStreamKey);
                            AudioStreamHelper.SetVolumeSlide(stream, 0f, 1f, audioEvent.Length);
                            break;
                        case EventType.FadeOut:
                            stream = player.GetAudioStream(audioEvent.TargetStreamKey);
                            AudioStreamHelper.SetVolumeSlide(stream, 1f, 0f, audioEvent.Length);
                            break;
                        default:
                            throw new Exception("Invalid Event Type");
                    }
                    //DebugHelper.WriteLine("end event sycn:" + audioEvent.StreamEventType + " " + syncId);
                }
            });
        }

        private void OnSectionOffset(AudioSection audioSection, AudioStreamSection streamSection)
        {
            if (!audioSection.LoopIndefinitely)
                PlayNextSection(audioSection, streamSection);
        }

        private void OnSectionEnd(AudioSection audioSection, AudioStreamSection streamSection)
        {
            if (audioSection.LoopIndefinitely || audioSection.HasOffset)
            {
                AudioStreamHelper.SetPosition(streamSection.AudioStream, audioSection.Start.Position);
                ApplySyncIfOneExistsForCurrentPosition(streamSection.AudioStream);
            }
            else
                PlayNextSection(audioSection, streamSection);
        }

        private void PlayNextSection(AudioSection audioSection, AudioStreamSection streamSection)
        {
            if (!audioSection.AutoPlayNextSection)
            {
                // DebugHelper.WriteLine("Pause " + streamSection.AudioStream.Description);
                AudioStreamHelper.Pause(streamSection.AudioStream);
            }
            else
            {
                var nextSection = GetNextSection(audioSection, streamSection);
                if (nextSection != null)
                {
                    QueueSection(streamSection.Key, nextSection.Key);
                }
                else
                {
                    // DebugHelper.WriteLine("Pause " + streamSection.AudioStream.Description);
                    AudioStreamHelper.Pause(streamSection.AudioStream);
                }
            }
        }

        private static AudioSection GetNextSection(AudioSection audioSection, AudioStreamSection streamSection)
        {
            lock (streamSection)
            {
                var index = streamSection.AudioSections.IndexOf(audioSection);

                index++;
                if (index >= streamSection.AudioSections.Count) return null;

                Console.WriteLine(index);
                return streamSection.AudioSections[index];
            }
        }

        private AudioSync GetAudioSyncById(int channel, int syncId)
        {
            var streamSection = GetStreamSectionByChannel(channel);
            lock (streamSection)
            {
                return streamSection.AudioSections
                    .SelectMany(a => a.AudioSyncs)
                    .Union(streamSection.AudioStream.AudioSyncs)
                    .FirstOrDefault(x => x.Id == syncId);
            }
        }

        private AudioSection GetAudioSectionBySync(int channel, AudioSync audioSync)
        {
            var streamSection = GetStreamSectionByChannel(channel);
            lock (streamSection)
            {
                return audioSync.SyncType == SyncType.AudioStreamEvent
                    ? null
                    : streamSection.AudioSections.FirstOrDefault(a => a.AudioSyncs.Any(x => x.Id == audioSync.Id));
            }
        }

        private AudioStreamSection GetStreamSectionByChannel(int channel)
        {
            lock (_streamSections)
            {
                return _streamSections.FirstOrDefault(x => x.AudioStream.Channel == channel);
            }
        }

        private static void RemoveSyncFromStream(AudioStream audioStream, AudioSync sync)
        {
            if (sync.Id == int.MinValue) return;
            BassMix.BASS_Mixer_ChannelRemoveSync(audioStream.Channel, sync.Id);
            sync.Id = int.MinValue;
        }

        private static void AddSyncToStream(AudioStream audioStream, AudioSync audioSync)
        {
            lock (audioStream)
            {
                audioStream.AudioSyncs.Add(audioSync);
            }

            if (audioSync.SyncType == SyncType.Start || audioStream.DisableSyncs)
                return;

            audioSync.Id = BassMix.BASS_Mixer_ChannelSetSync(audioStream.Channel,
                BASSSync.BASS_SYNC_POS | BASSSync.BASS_SYNC_MIXTIME,
                audioSync.Position,
                audioStream.SyncProc,
                new IntPtr((int) audioSync.SyncType));
        }

        private class AudioStreamSection
        {
            public string Key { get; set; }

            public AudioStream AudioStream { get; set; }

            public List<AudioSection> AudioSections { get; set; }
        }
    }
}