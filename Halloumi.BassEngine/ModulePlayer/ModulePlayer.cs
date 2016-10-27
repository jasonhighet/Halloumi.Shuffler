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
        private List<AudioPlayer> _channelPlayers = new List<AudioPlayer>();
        private decimal _loopLength;
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

        public void PlayModule()
        {
            var section = _mainPlayer.GetAudioSection(SongPlayer, SongPlayer);
            section.LoopIndefinitely = false;
            _mainPlayer.QueueSection(SongPlayer, SongPlayer);
            _mainPlayer.Play(SongPlayer);
        }

        public void PlayModuleLooped()
        {
            var section = _mainPlayer.GetAudioSection(SongPlayer, SongPlayer);
            section.LoopIndefinitely = true;
            _mainPlayer.QueueSection(SongPlayer, SongPlayer);
            _mainPlayer.Play(SongPlayer);
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

            LoadPattern(pattern);
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
            _loopLength = (decimal)BpmHelper.GetDefaultLoopLength(_targetBpm);

            LoadChannelPlayers(module);
            LoadAudioFiles(module);
            LoadPatterns(module);
            LoadPatternSequencePlayer(module);
            LoadPatternSequence(module);

            Module = module;
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

        private void LoadPatternSequence(Module module)
        {
            var patternIndex = 0;
            foreach (var patternKey in module.Sequence)
            {
                var position = GetLoopPosition(patternIndex, _targetBpm);
                var pattern = module.Patterns.FirstOrDefault(x => x.Key == patternKey);
                if (pattern == null)
                    continue;

                for (var columnIndex = 0; columnIndex < pattern.Sequence.Count; columnIndex++)
                {
                    var player = _channelPlayers[columnIndex];
                    var channelSequenceKey = patternKey + columnIndex;
                    _mainPlayer.AddEvent(SongPlayer, (double)position, channelSequenceKey, channelSequenceKey,
                        EventType.Play, player);
                }
                patternIndex++;
            }
        }

        private void LoadPatternSequencePlayer(Module module)
        {
            _mainPlayer.Load(SongPlayer, SilenceHelper.GetSilenceAudioFile());

            var songLength = _loopLength*module.Sequence.Count;

            _mainPlayer.AddSection(SongPlayer, SongPlayer, 0, (double)songLength, bpm: _targetBpm);
        }

        private void LoadPatterns(Module module)
        {
            foreach (var pattern in module.Patterns)
                LoadPattern(pattern);
        }

        private void LoadPattern(Module.Pattern pattern)
        {
            var channelIndex = 0;
            foreach (var channelSequence in pattern.Sequence)
            {
                var player = _channelPlayers.FirstOrDefault(x => _channelPlayers.IndexOf(x) == channelIndex);
                if (player == null)
                    continue;

                var channelSequenceKey = pattern.Key + channelIndex;
                player.Load(channelSequenceKey, SilenceHelper.GetSilenceAudioFile());
                player.AddSection(channelSequenceKey, channelSequenceKey, 0, (double)_loopLength, bpm: _targetBpm);

                var positions = GetPositions(channelSequence);
                foreach (var position in positions)
                {
                    player.AddPlayEvent(channelSequenceKey, (double)position.Item2, position.Item1, position.Item1);
                }

                channelIndex++;
            }
        }

        private IEnumerable<Tuple<string, decimal>> GetPositions(IReadOnlyCollection<string> sequence, decimal offset = 0)
        {
            var positions = new List<Tuple<string, decimal>>();

            var rowIndex = 0;
            foreach (var sampleKey in sequence)
            {
                var loopPosition = 0.0M;
                if (rowIndex > 0)
                    loopPosition = rowIndex / (decimal)sequence.Count;

                var position = GetLoopPosition(loopPosition, _targetBpm) + offset;

                positions.Add(new Tuple<string, decimal>(sampleKey, position));

                rowIndex++;
            }

            return positions;
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
            const double fadeLength = 0.01;
            foreach (var sample in audioFile.Samples)
            {
                var fullSampleKey = audioFile.Key + "." + sample.Key;
                channelPlayer.Load(fullSampleKey, audioFile.Path);
                channelPlayer.AddSection(fullSampleKey,
                    fullSampleKey,
                    sample.Start,
                    sample.Length,
                    sample.Offset,
                    calculateBpmFromLength: true,
                    targetBpm: module.Bpm);

                if (sample.Offset == 0)
                {
                    channelPlayer.AddEvent(fullSampleKey, sample.Start, fullSampleKey, fullSampleKey, EventType.FadeIn,
                        length: fadeLength/2);
                    var startFadeOut = sample.Start + sample.Length - fadeLength;
                    channelPlayer.AddEvent(fullSampleKey, startFadeOut, fullSampleKey, fullSampleKey, EventType.FadeOut,
                        length: fadeLength);
                }
                //   }
                    //else
                    //{
                    //    channelPlayer.AddEvent(fullSampleKey, sample.Offset, fullSampleKey, fullSampleKey, EventType.FadeIn, length: fadeLength);
                    //    var startFadeOut = sample.Offset - fadeLength;
                    //    channelPlayer.AddEvent(fullSampleKey, startFadeOut, fullSampleKey, fullSampleKey, EventType.FadeOut, length: fadeLength);
                    //}
                }
            }

        private static decimal GetLoopPosition(decimal loopCount, decimal bpm)
        {
            return (decimal)BpmHelper.GetDefaultLoopLength(bpm)*loopCount;
        }

        public void PlayPattern(string patternKey)
        {
            const int patternLoopCount = 32;

            _mainPlayer.Pause(PatternPlayer);
            _mainPlayer.Unload(PatternPlayer);
            _mainPlayer.Load(PatternPlayer, SilenceHelper.GetSilenceAudioFile());
            _mainPlayer.AddSection(PatternPlayer, PatternPlayer, 0, (double)_loopLength * patternLoopCount, bpm: _targetBpm);

            var pattern = Module.Patterns.FirstOrDefault(x => x.Key == patternKey);
            if(pattern == null)
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
                    for (var i = 0; i < patternLoopCount; i++)
                    {
                        var currentPosition = position.Item2 + (i * _loopLength);
                        _mainPlayer.AddEvent(PatternPlayer, (double)currentPosition, position.Item1, position.Item1, EventType.Play, player);
                    }
                }
            }

            _mainPlayer.QueueSection(PatternPlayer, PatternPlayer);
            _mainPlayer.Play(PatternPlayer);
        }

        public void PlayPatternChannel(string patternKey, string channelKey)
        {
            const int patternLoopCount = 32;

            _mainPlayer.Pause(PatternPlayer);
            _mainPlayer.Unload(PatternPlayer);
            _mainPlayer.Load(PatternPlayer, SilenceHelper.GetSilenceAudioFile());
            _mainPlayer.AddSection(PatternPlayer, PatternPlayer, 0, (double) _loopLength * patternLoopCount, bpm: _targetBpm);

            var pattern = Module.Patterns.FirstOrDefault(x => x.Key == patternKey);
            if (pattern == null)
                return;

            var channel = Module.Channels.FirstOrDefault(x => x.Key == channelKey);
            if (channel == null)
                return;

            var section = _mainPlayer.GetAudioSection(PatternPlayer, PatternPlayer);
            section.LoopIndefinitely = true;

            var channelIndex = Module.Channels.IndexOf(channel);

            var channelSequence = pattern.Sequence[channelIndex];
            var positions = GetPositions(channelSequence);
            var player = _channelPlayers[channelIndex];

            foreach (var position in positions)
            {
                for (var i = 0; i < patternLoopCount; i++)
                {
                    var currentPosition = position.Item2 + (i * _loopLength);
                    _mainPlayer.AddEvent(PatternPlayer, (double)currentPosition, position.Item1, position.Item1, EventType.Play, player);
                }
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