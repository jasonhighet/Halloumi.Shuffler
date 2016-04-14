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

            var tags = TagHelper.LoadTags(filename);
            if (tags != null)
            {
                if (tags.Gain.HasValue)
                    audioStream.Gain = tags.Gain.Value;
                if (tags.Bpm.HasValue)
                    audioStream.Bpm = tags.Bpm.Value;
            }

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

            lock (streamSection.AudioStream)
            {
                foreach (var sync in streamSection.AudioStream.AudioSyncs)
                {
                    RemoveSyncFromStream(streamSection.AudioStream, sync);
                }
                streamSection.AudioStream.AudioSyncs.Clear();
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
            AudioStreamHelper.Play(audioStream);
        }

        public void Play(IEnumerable<string> streamKeys)
        {
            Parallel.ForEach(streamKeys, streamKey =>
            {
                var audioStream = GetAudioStream(streamKey);
                AudioStreamHelper.Play(audioStream);
            });
        }

        public List<AudioStream> GetAudioStreams()
        {
            return GetStreamKeys().Select(GetAudioStream).ToList();
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

        public void Pause()
        {
            var streamKeys = GetStreamKeys();

            Parallel.ForEach(streamKeys, streamKey =>
            {
                var audioStream = GetAudioStream(streamKey);
                AudioStreamHelper.Pause(audioStream);
            });
        }

        public AudioSection AddSection(string streamKey, string sectionKey, bool loopIndefinitely = false)
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
                LoopIndefinitely = loopIndefinitely
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

        public void SetSectionPositions(string streamKey, string sectionKey, double start = 0, double length = 0, double offset = 0)
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
        }

        public void SetSectionBpm(string streamKey, string sectionKey, decimal bpm = 0, bool calculateBpmFromLength = false, decimal targetBpm = 0)
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



        public void AddCustomSync(string streamKey, double position)
        {
            var audioStream = GetAudioStream(streamKey);
            if (audioStream == null)
                return;

            if (position == double.MinValue) return;

            if (position < 0) position = 0;
            var samplePosition = audioStream.SecondsToSamples(position);
            var maxSample = audioStream.Length - 500;
            if (samplePosition > maxSample)
                samplePosition = maxSample;

            var audioSync = new AudioSync {SyncType = SyncType.Custom, Position = samplePosition};

            lock (audioStream)
            {
                audioStream.AudioSyncs.Add(audioSync);
            }

            AddSyncToStream(audioStream, audioSync);
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
            if (syncType == SyncType.Custom)
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

            if (position == double.MinValue) return;

            if (position < 0) position = 0;
            var samplePosition = audioStream.SecondsToSamples(position);
            var maxSample = audioStream.Length - 500;
            if (samplePosition > maxSample)
                samplePosition = maxSample;

            audioSync = new AudioSync {SyncType = syncType, Position = samplePosition};

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
                case SyncType.Custom:
                    var onRaiseCustomSync = new OnRaiseCustomSync(RaiseCustomSync);
                    onRaiseCustomSync.BeginInvoke(streamSection, audioSync, null, null);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     Called when a custom sync is fired.
        /// </summary>
        /// <param name="streamSection">The stream section.</param>
        /// <param name="audioSync">The audio synchronize.</param>
        private void RaiseCustomSync(AudioStreamSection streamSection, AudioSync audioSync)
        {
            var eventArgs = new CustomSyncEventArgs
            {
                SyncId = audioSync.Id,
                StreamKey = streamSection.Key
            };
            OnCustomSync?.Invoke(this, eventArgs);
            DebugHelper.WriteLine("custom sync");
        }

        private void OnSectionOffset(AudioSection audioSection, AudioStreamSection streamSection)
        {
            if (!audioSection.LoopIndefinitely)
                PlayNextSection(audioSection, streamSection);
        }

        private void OnSectionEnd(AudioSection audioSection, AudioStreamSection streamSection)
        {
            if (audioSection.LoopIndefinitely)
            {
                AudioStreamHelper.SetPosition(streamSection.AudioStream, audioSection.Start.Position);
                var customStartSync =
                    streamSection.AudioStream.AudioSyncs.FirstOrDefault(
                        x => x.SyncType == SyncType.Custom && x.Position == 0);

                if (customStartSync == null) return;
                var onRaiseCustomSync = new OnRaiseCustomSync(RaiseCustomSync);
                onRaiseCustomSync.BeginInvoke(streamSection, customStartSync, null, null);
            }

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
                return (audioSync.SyncType == SyncType.Custom)
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
            audioSync.Id = BassMix.BASS_Mixer_ChannelSetSync(audioStream.Channel,
                BASSSync.BASS_SYNC_POS | BASSSync.BASS_SYNC_MIXTIME,
                audioSync.Position,
                audioStream.SyncProc,
                new IntPtr((int) audioSync.SyncType));
        }

        public event CustomSyncEventHandler OnCustomSync;

        private delegate void OnRaiseCustomSync(AudioStreamSection streamSection, AudioSync audioSync);

        private class AudioStreamSection
        {
            public string Key { get; set; }

            public AudioStream AudioStream { get; set; }

            public List<AudioSection> AudioSections { get; set; }
        }
    }

    public class CustomSyncEventArgs : EventArgs
    {
        public string StreamKey { get; set; }

        public int SyncId { get; set; }
    }

    public delegate void CustomSyncEventHandler(object sender, CustomSyncEventArgs e);
}