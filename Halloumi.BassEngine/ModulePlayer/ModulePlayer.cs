using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioEngine.Channels;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Players;
using Newtonsoft.Json;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Halloumi.Shuffler.AudioEngine.ModulePlayer
{
    public class ModulePlayer : IBmpProvider
    {
        private const string PatternPlayer = "Pattern";
        private const string SongPlayer = "Song";
        private const string SongPlayer2 = "Song2";
        private List<AudioPlayer> _channelPlayers = new List<AudioPlayer>();
        private double _loopLength;
        private readonly AudioPlayer _mainPlayer;
        private decimal _targetBpm = 100;
        public Module Module { get; internal set; }

        public ModulePlayer()
        {
            Output = new MixerChannel(this);
            _mainPlayer = new AudioPlayer();

            Output.AddInputChannel(_mainPlayer.Output);
        }

        public MixerChannel Output { get; }

        public decimal GetCurrentBpm()
        {
            return _targetBpm;
        }

        public void PlayModule(bool loopIndefinitely = false)
        {
            _mainPlayer.Pause(SongPlayer2);
            _mainPlayer.Unload(SongPlayer2);
            _mainPlayer.Load(SongPlayer2, SilenceHelper.GetSilenceAudioFile());
            var songLength = _loopLength * Module.Sequence.Count;
            _mainPlayer.AddSection(SongPlayer2, SongPlayer2, 0, songLength, bpm: _targetBpm);

            var section = _mainPlayer.GetAudioSection(SongPlayer2, SongPlayer2);
            section.LoopIndefinitely = loopIndefinitely;

            var patternOffset = 0D;
            foreach (var sequence in Module.Sequence)
            {
                var pattern = Module.Patterns.FirstOrDefault(x => x.Key == sequence);
                if (pattern == null)
                    continue;

                for (var channelIndex = 0; channelIndex < Module.Channels.Count; channelIndex++)
                {
                    var channelSequence = pattern.Sequence[channelIndex];
                    var positions = GetPositions(channelSequence);
                    var player = _channelPlayers[channelIndex];

                    foreach (var position in positions)
                    {
                        var currentPosition = position.Item2 + patternOffset; 
                        _mainPlayer.AddEvent(SongPlayer2, currentPosition, position.Item1, position.Item1, EventType.PlaySolo, player);
                    }
                }

                patternOffset += _loopLength;
            }

            _mainPlayer.QueueSection(SongPlayer2, SongPlayer2);
            _mainPlayer.Play(SongPlayer2);
        }

        public void PlayModuleLooped()
        {
            PlayModule(true);
        }

        public void Pause()
        {
            _mainPlayer.Pause();
            foreach (var player in _channelPlayers)
            {
                player.Pause();
            }
        }

        public void AddPattern(string patternKey)
        {
            Pause();
            var pattern = new Module.Pattern()
            {
                Key = patternKey,
                Sequence = new List<List<string>>()
            };

            Module.Patterns.Add(pattern);

            foreach (var channel in Module.Channels)
            {
                pattern.Sequence.Add(new List<string>());
            }
        }

        public void SaveModule(string modulePath)
        {
            File.WriteAllText(modulePath, JsonConvert.SerializeObject(Module));
        }

        public void AddChannel(string channelKey)
        {
            Pause();
            var channel = new Module.Channel()
            {
                Key = channelKey
            };

            Module.Channels.Add(channel);
            foreach (var patttern in Module.Patterns)
            {
                patttern.Sequence.Add(new List<string>());
            }

            LoadModule(Module);
        }

        public void LoadModule(string modulePath)
        {
            var module = JsonConvert.DeserializeObject<Module>(File.ReadAllText(modulePath));
            LoadModule(module);
        }

        public void DeletePattern(string patternKey)
        {
            Pause();

            var pattern = Module.Patterns.FirstOrDefault(x => x.Key == patternKey);
            if (pattern == null) return;

            Module.Patterns.RemoveAll(x => x.Key == patternKey);
            LoadModule(Module);
        }

        public void ReloadModule()
        {
            LoadModule(Module);
        }

        public void LoadModule(Module module)
        {
            Pause();
            _mainPlayer.UnloadAll();
            foreach (var player in _channelPlayers)
            {
                player.UnloadAll();
            }

            _targetBpm = module.Bpm;
            _loopLength = BpmHelper.GetDefaultLoopLength(_targetBpm);

            Module = module;

            LoadChannelPlayers(module);
            LoadAudioFiles(module);
        }

        public void DeleteChannel(string channelKey)
        {
            Pause();

            var channel = Module.Channels.FirstOrDefault(x => x.Key == channelKey);
            if (channel == null) return;

            var channelIndex = Module.Channels.FindIndex(x => x.Key == channelKey);

            Module.Channels.RemoveAll(x => x.Key == channelKey);

            foreach (var patttern in Module.Patterns)
            {
                patttern.Sequence.RemoveAt(channelIndex);
            }

            LoadModule(Module);
        }

        public void SetBpm(decimal bpm)
        {
            Module.Bpm = bpm;
            LoadModule(Module);
        }


        private IEnumerable<Tuple<string, double, double>> GetPositions(IEnumerable<string> sequence)
        {
            var positions = new List<Tuple<string, double, double>>();

            var currentPosition = 0D;
            foreach (var sampleKey in sequence)
            {
                var sampleKeys = sampleKey.Split('.');
                var adjustedLength = GetAdjustedSampleLenth(sampleKeys[0], sampleKeys[1]);

                positions.Add(new Tuple<string, double, double>(sampleKey, currentPosition, currentPosition + (adjustedLength - 0.005D)));

                currentPosition += adjustedLength;

                if (currentPosition > _loopLength)
                    break;
            }

            return positions;
        }

        private double GetAdjustedSampleLenth(string streamKey, string sampleKey)
        {
            var audioFile = Module.AudioFiles.FirstOrDefault(x => x.Key == streamKey);
            var sample = audioFile?.Samples.FirstOrDefault(x => x.Key == sampleKey);
            if (sample == null)
                return 0;

            var bpm = BpmHelper.GetBpmFromLoopLength(sample.Length);

            return BpmHelper.GetAdjustedAudioLength(sample.Length, bpm, Module.Bpm);
        }

        private void LoadChannelPlayers(Module module)
        {
            _channelPlayers = new List<AudioPlayer>();
            for (var channelIndex = 0; channelIndex < module.Channels.Count; channelIndex++)
            {
                var channelPlayer = new AudioPlayer();
                Output.AddInputChannel(channelPlayer.Output);
                _channelPlayers.Add(channelPlayer);
            }
        }

        public void MergeSamples(Module.AudioFile audioFile, List<Module.Sample> samples)
        {
            var newSampleKeys = samples.Select(x => x.Key).ToList();
            audioFile.Samples.RemoveAll(x => newSampleKeys.Contains(x.Key));
            audioFile.Samples.AddRange(samples);
            UpdateAudioFile(audioFile);
        }


        public void UpdateAudioFile(Module.AudioFile audioFile)
        {
            Pause();
            Module.AudioFiles.RemoveAll(x => x.Key == audioFile.Key);
            Module.AudioFiles.Add(audioFile);

            foreach (var channelPlayer in _channelPlayers)
            {
                var existingSampleKeys = channelPlayer.GetStreamKeys().Where(x => x.StartsWith(audioFile.Key + ".")).ToList();
                foreach (var sampleKey in existingSampleKeys)
                {
                    channelPlayer.Unload(sampleKey);
                }

                LoadSamples(Module, channelPlayer, audioFile);
            }
        }


        private void LoadAudioFiles(Module module)
        {
            foreach (var channelPlayer in _channelPlayers)
            {
                foreach (var audioFile in module.AudioFiles)
                {
                    LoadSamples(module, channelPlayer, audioFile);
                }
            }
        }

        private static void LoadSamples(Module module, AudioPlayer channelPlayer, Module.AudioFile audioFile)
        {
            foreach (var sample in audioFile.Samples)
            {
                var fullSampleKey = audioFile.Key + "." + sample.Key;
                channelPlayer.Load(fullSampleKey, audioFile.Path);
                var section = channelPlayer.AddSection(fullSampleKey,
                    fullSampleKey,
                    sample.Start,
                    sample.Length,
                    sample.Offset,
                    calculateBpmFromLength: true,
                    targetBpm: module.Bpm);

                section.LoopIndefinitely = true;
            }
        }

        public void PlayPattern(string patternKey)
        {
            const int patternLoopCount = 1;

            _mainPlayer.Pause(PatternPlayer);
            _mainPlayer.Unload(PatternPlayer);
            _mainPlayer.Load(PatternPlayer, SilenceHelper.GetSilenceAudioFile());
            _mainPlayer.AddSection(PatternPlayer, PatternPlayer, 0, _loopLength * patternLoopCount, bpm: _targetBpm);

            var pattern = Module.Patterns.FirstOrDefault(x => x.Key == patternKey);
            if (pattern == null)
                return;

            var section = _mainPlayer.GetAudioSection(PatternPlayer, PatternPlayer);
            section.LoopIndefinitely = true;

            for (var channelIndex = 0; channelIndex < Module.Channels.Count; channelIndex++)
            {
                var channelSequence = pattern.Sequence[channelIndex];
                var positions = GetPositions(channelSequence);
                var player = _channelPlayers[channelIndex];

                foreach (var position in positions)
                {
                    var currentPosition = position.Item2;
                    _mainPlayer.AddEvent(PatternPlayer, currentPosition, position.Item1, position.Item1, EventType.PlaySolo, player);
                }
            }

            _mainPlayer.QueueSection(PatternPlayer, PatternPlayer);
            _mainPlayer.Play(PatternPlayer);
        }

        public void PlayPatternChannel(string patternKey, string channelKey)
        {
            var pattern = Module.Patterns.FirstOrDefault(x => x.Key == patternKey);
            if (pattern == null)
                return;

            var channel = Module.Channels.FirstOrDefault(x => x.Key == channelKey);
            if (channel == null)
                return;

            var channelIndex = Module.Channels.IndexOf(channel);
            var channelSequence = pattern.Sequence[channelIndex];
            var positions = GetPositions(channelSequence);
            var player = _channelPlayers[channelIndex];

            _mainPlayer.Pause(PatternPlayer);
            _mainPlayer.Unload(PatternPlayer);
            _mainPlayer.Load(PatternPlayer, SilenceHelper.GetSilenceAudioFile());
            _mainPlayer.AddSection(PatternPlayer, PatternPlayer, 0, _loopLength, bpm: _targetBpm);
            var section = _mainPlayer.GetAudioSection(PatternPlayer, PatternPlayer);
            section.LoopIndefinitely = true;

            foreach (var position in positions)
            {
                var currentPosition = position.Item2;
                _mainPlayer.AddEvent(PatternPlayer, currentPosition, position.Item1, position.Item1, EventType.PlaySolo, player);
            }

            _mainPlayer.QueueSection(PatternPlayer, PatternPlayer);
            _mainPlayer.Play(PatternPlayer);
        }


        public void CreateModule()
        {
            _mainPlayer.UnloadAll();

            Module = new Module()
            {
                Bpm = 100,
                AudioFiles = new List<Module.AudioFile>(),
                Channels = new List<Module.Channel>(),
                Patterns = new List<Module.Pattern>(),
                Sequence = new List<string>(),
                Title = ""
            };
        }
    }
}