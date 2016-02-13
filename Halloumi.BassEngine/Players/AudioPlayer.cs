using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
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
        private readonly List<AudioStreamSection> _streamSections;

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

            AudioStreamHelper.LoadAudio(audioStream);
            audioStream.SyncProc = OnSync;
            AudioStreamHelper.AddToMixer(audioStream, Output.InternalChannel);
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
            lock (streamSection)
            {
                foreach (var sync in streamSection.AudioSections.SelectMany(section => section.AudioSyncs))
                {
                    RemoveSyncFromStream(streamSection.AudioStream, sync);
                }
            }

            AudioStreamHelper.RemoveFromMixer(streamSection.AudioStream, Output.InternalChannel);
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
            var keys = GetStreamSectionsKeys();
            foreach (var streamKey in keys)
            {
                Unload(streamKey);
            }
        }

        private IEnumerable<string> GetStreamSectionsKeys()
        {
            lock (_streamSections)
            {
                return _streamSections.Select(x => x.Key);
            }
        }

        public void Play(string streamKey)
        {
            var audioStream = GetAudioStream(streamKey);
            AudioStreamHelper.Play(audioStream);
        }

        private AudioStream GetAudioStream(string streamKey)
        {
            lock (_streamSections)
            {
                return _streamSections.FirstOrDefault(x => x.Key == streamKey)?.AudioStream;
            }
        }

        public void Pause(string streamKey)
        {
            var audioStream = GetAudioStream(streamKey);
            AudioStreamHelper.Pause(audioStream);
        }

        public AudioSection AddSection(string streamKey, string sectionKey)
        {
            var streamSection = GetStreamSection(streamKey);
            if (streamSection == null)
                return null;

            var audioSection = GetAudioSection(streamKey, sectionKey);
            if (audioSection != null)
                return audioSection;

            audioSection = new AudioSection
            {
                Key = sectionKey
            };

            lock (streamSection)
            {
                streamSection.AudioSections.Add(audioSection);
            }

            return audioSection;
        }

        private AudioSection GetAudioSection(string streamKey, string sectionKey)
        {
            var streamSection = GetStreamSection(streamKey);
            lock (streamSection)
            {
                return streamSection?.AudioSections.FirstOrDefault(x => x.Key == sectionKey);
            }

        }

        public void SetSectionPositions(string streamKey, string sectionKey, double start, double length,
            double offset = 0)
        {
            var audioStream = GetAudioStream(streamKey);
            if (audioStream == null)
                return;

            var audioSection = GetAudioSection(streamKey, sectionKey);
            if (audioSection == null)
                return;

            SetSync(audioStream, audioSection, SyncType.Start, start);
            SetSync(audioStream, audioSection, SyncType.End, start + length);

            if (offset == 0) offset = double.MinValue;
            SetSync(audioStream, audioSection, SyncType.Offset, offset);
        }

        public void QueueSection(string streamKey, string sectionKey)
        {
            var audioStream = GetAudioStream(streamKey);
            if (audioStream == null)
                return;

            var audioSection = GetAudioSection(streamKey, sectionKey);
            if (audioSection == null)
                return;

            if (!audioSection.HasStartAndEnd)
                return;

            var startPosition = audioSection.HasOffset
                ? audioSection.Offset.Position + 500
                : audioSection.Start.Position;

            AudioStreamHelper.SetPosition(audioStream, startPosition);
        }

        private static void SetSync(AudioStream audioStream, AudioSection audioSection, SyncType syncType,
            double position)
        {
            var audioSync = GetAudioSync(audioSection, syncType);
            if (audioSync != null)
            {
                RemoveSyncFromStream(audioStream, audioSync);

                lock (audioSection)
                {
                    audioSection.AudioSyncs.Remove(audioSync);
                }

            }

            if (position == double.MinValue) return;

            if (position < 0) position = 0;
            var samplePosition = audioStream.SecondsToSamples(position);
            var maxSample = audioStream.Length - 500;
            if (samplePosition > maxSample)
                samplePosition = maxSample;

            audioSync = new AudioSync { SyncType = syncType, Position = samplePosition };

            lock (audioSection)
            {
                audioSection.AudioSyncs.Add(audioSync);
            }

            AddSyncToStream(audioStream, audioSync);
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
            var streamSection = GetStreamSectionByChannel(channel);
            var audioSection = GetAudioSectionBySync(channel, syncId);
            var audioSync = GetAudioSyncById(channel, syncId);
            

            if (streamSection == null || audioSync == null || audioSection == null)
                return;

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
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnSectionOffset(AudioSection audioSection, AudioStreamSection streamSection)
        {
            if (!audioSection.LoopIndefinitely)
                PlayNextSection(audioSection, streamSection);
        }

        private void OnSectionEnd(AudioSection audioSection, AudioStreamSection streamSection)
        {
            if (audioSection.LoopIndefinitely)
                AudioStreamHelper.SetPosition(streamSection.AudioStream, audioSection.Start.Position);
            else
                PlayNextSection(audioSection, streamSection);
        }

        private void PlayNextSection(AudioSection audioSection, AudioStreamSection streamSection)
        {
            var nextSection = GetNextSection(audioSection, streamSection);

            if (nextSection != null)
            {
                QueueSection(streamSection.Key, nextSection.Key);               
            }
            else
            {
                AudioStreamHelper.Pause(streamSection.AudioStream);
            }
        }

        private static AudioSection GetNextSection(AudioSection audioSection, AudioStreamSection streamSection)
        {
            lock(streamSection)
            {
                var index = streamSection.AudioSections.IndexOf(audioSection);

                index++;
                if (index < streamSection.AudioSections.Count)
                {
                    Console.WriteLine(index);
                    return streamSection.AudioSections[index];
                }
                return null;
            }
        }

        private AudioSync GetAudioSyncById(int channel, int syncId)
        {
            var streamSection = GetStreamSectionByChannel(channel);
            lock (streamSection)
            {
                return streamSection?.AudioSections
                    .SelectMany(a => a.AudioSyncs)
                    .FirstOrDefault(x => x.Id == syncId);
            }

        }

        private AudioStream GetAudioStreamByChannel(int channel)
        {
            lock (_streamSections)
            {
                return _streamSections.Select(x => x.AudioStream).FirstOrDefault(x => x.Channel == channel);
            }
        }

        private AudioSection GetAudioSectionBySync(int channel, int syncId)
        {
            var streamSection = GetStreamSectionByChannel(channel);
            lock (streamSection)
            {
                return streamSection?.AudioSections
                .FirstOrDefault(a => a.AudioSyncs.Any(x => x.Id == syncId));
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
            audioSync.Id = BassMix.BASS_Mixer_ChannelSetSync(audioStream.Channel,
                BASSSync.BASS_SYNC_POS | BASSSync.BASS_SYNC_MIXTIME,
                audioSync.Position,
                audioStream.SyncProc,
                new IntPtr((int)audioSync.SyncType));
        }

        private class AudioStreamSection
        {
            public string Key { get; set; }

            public AudioStream AudioStream { get; set; }

            public List<AudioSection> AudioSections { get; set; }
        }
    }
}