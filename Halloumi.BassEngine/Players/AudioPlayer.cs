using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Halloumi.BassEngine.Channels;
using Halloumi.BassEngine.Helpers;
using Halloumi.BassEngine.Models;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;

namespace Halloumi.BassEngine.Players
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    internal class AudioPlayer
    {
        private readonly List<AudioStreamSection> _audioStreamSections;

        public AudioPlayer()
        {
            Output = new MixerChannel();
            _audioStreamSections = new List<AudioStreamSection>();
        }

        public MixerChannel Output { get; }

        public AudioStream Load(string key, string filename)
        {
            if (!File.Exists(filename))
                throw new Exception("Cannot find file " + filename);
            if (_audioStreamSections.Any(x => x.Key == key))
                throw new Exception("AudioStream already exists with key " + key);

            var audioStream = new Sample
            {
                Filename = filename
            };

            AudioStreamHelper.LoadAudio(audioStream);
            audioStream.SyncProc = OnSync;
            AudioStreamHelper.AddToMixer(audioStream, Output.InternalChannel);
            AudioStreamHelper.SetPosition(audioStream, 0);

            _audioStreamSections.Add(new AudioStreamSection
            {
                Key = key,
                AudioStream = audioStream,
                AudioSections = new List<AudioSection>()
            });

            return audioStream;
        }

        public void Unload(string key)
        {
            var streamSection = _audioStreamSections.FirstOrDefault(x => x.Key == key);
            if (streamSection == null)
                return;

            AudioStreamHelper.Pause(streamSection.AudioStream);

            foreach (var section in streamSection.AudioSections)
            {
                RemoveSyncs(streamSection.AudioStream, section);
            }

            AudioStreamHelper.RemoveFromMixer(streamSection.AudioStream, Output.InternalChannel);
            AudioStreamHelper.UnloadAudio(streamSection.AudioStream);

            streamSection.AudioStream.SyncProc = null;
            _audioStreamSections.Remove(streamSection);
        }

        public void UnloadAll()
        {
            var keys = _audioStreamSections.Select(x => x.Key);
            foreach (var key in keys)
            {
                Unload(key);
            }
        }

        public void Play(string key)
        {
            var audioStream = _audioStreamSections.FirstOrDefault(x => x.Key == key)?.AudioStream;
            AudioStreamHelper.Play(audioStream);
        }

        public void Pause(string key)
        {
            var audioStream = _audioStreamSections.FirstOrDefault(x => x.Key == key)?.AudioStream;
            AudioStreamHelper.Pause(audioStream);
        }

        public AudioSection AddAudioSection(string key, double start, double length,
            double offset = double.MinValue)
        {
            var streamSection = _audioStreamSections.FirstOrDefault(x => x.Key == key);
            if (streamSection == null)
                return null;

            var audioSection = new AudioSection();

            SetSync(streamSection.AudioStream, audioSection, SyncType.Start, start);
            SetSync(streamSection.AudioStream, audioSection, SyncType.End, start + length);
            SetSync(streamSection.AudioStream, audioSection, SyncType.End, offset);

            streamSection.AudioSections.Add(audioSection);

            return audioSection;
        }


        public void UpdateAudioSection(string key, AudioSection audioSection, double start, double length,
            double offset = double.MinValue)
        {
            var audioStream = _audioStreamSections.FirstOrDefault(x => x.Key == key)?.AudioStream;
            RemoveSyncs(audioStream, audioSection);

            SetSync(audioStream, audioSection, SyncType.Start, start);
            SetSync(audioStream, audioSection, SyncType.End, start + length);
            SetSync(audioStream, audioSection, SyncType.End, offset);
        }

        public void Queue(string key, AudioSection audioSection)
        {
            var audioStream = _audioStreamSections.FirstOrDefault(x => x.Key == key)?.AudioStream;
            if (audioStream == null || audioSection == null)
                return;

            var startPosition = audioSection.HasOffset
                ? audioSection.Offset.Position + 500
                : audioSection.Start.Position;

            AudioStreamHelper.SetPosition(audioStream, startPosition);
        }

        private static void SetSync(AudioStream audioStream, AudioSection audioSection, SyncType syncType,
            double position)
        {
            audioSection.AudioSyncs.RemoveAll(x => x.SyncType == syncType);

            if (position == double.MinValue) return;

            if (position < 0) position = 0;
            var samplePosition = audioStream.SecondsToSamples(position);
            var maxSample = audioStream.Length - 500;
            if (samplePosition > maxSample)
                samplePosition = maxSample;

            var audioSync = new AudioSync {SyncType = syncType, Position = samplePosition};

            audioSection.AudioSyncs.Add(audioSync);

            audioSync.Id = BassMix.BASS_Mixer_ChannelSetSync(audioStream.Channel,
                BASSSync.BASS_SYNC_POS | BASSSync.BASS_SYNC_MIXTIME,
                audioSync.Position,
                audioStream.SyncProc,
                new IntPtr((int) audioSync.SyncType));
        }

        private void OnSync(int syncId, int channel, int data, IntPtr pointer)
        {
            var audioStream = GetAudioStreamByChannel(channel);
            var audioSection = GetAudioSectionBySync(channel, syncId);
            var audioSync = GetAudioSyncById(channel, syncId);

            if (audioStream == null || audioSync == null || audioSection == null)
                return;

            switch (audioSync.SyncType)
            {
                case SyncType.End:
                    OnSectionEnd(audioSection, audioStream);
                    break;
                case SyncType.Offset:
                    OnSectionOffset(audioSection, audioStream);
                    break;
                case SyncType.Start:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void OnSectionOffset(AudioSection audioSection, AudioStream audioStream)
        {
            if (!audioSection.LoopIndefinitely)
                AudioStreamHelper.Pause(audioStream);
        }

        private static void OnSectionEnd(AudioSection audioSection, AudioStream audioStream)
        {
            if (audioSection.LoopIndefinitely)
                AudioStreamHelper.SetPosition(audioStream, audioSection.Start.Position);
            else
                AudioStreamHelper.Pause(audioStream);
        }

        private AudioSync GetAudioSyncById(int channel, int syncId)
        {
            return _audioStreamSections
                .FirstOrDefault(x => x.AudioStream.Channel == channel)
                ?.AudioSections
                .SelectMany(a => a.AudioSyncs)
                .FirstOrDefault(x => x.Id == syncId);
        }

        private AudioStream GetAudioStreamByChannel(int channel)
        {
            return _audioStreamSections.Select(x => x.AudioStream).FirstOrDefault(x => x.Channel == channel);
        }

        private AudioSection GetAudioSectionBySync(int channel, int syncId)
        {
            return _audioStreamSections
                .FirstOrDefault(x => x.AudioStream.Channel == channel)
                ?.AudioSections
                .FirstOrDefault(a => a.AudioSyncs.Any(x => x.Id == syncId));
        }

        private static void RemoveSyncs(AudioStream audioStream, AudioSection audioSection)
        {
            foreach (var sync in audioSection.AudioSyncs.Where(x => x.Id != int.MinValue))
            {
                BassMix.BASS_Mixer_ChannelRemoveSync(audioStream.Channel, sync.Id);
                sync.Id = int.MinValue;
            }
        }

        private class AudioStreamSection
        {
            public string Key { get; set; }

            public AudioStream AudioStream { get; set; }

            public List<AudioSection> AudioSections { get; set; }
        }
    }
}