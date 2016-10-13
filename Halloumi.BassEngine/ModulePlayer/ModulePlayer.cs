using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioEngine.Channels;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Players;
using Newtonsoft.Json;

namespace Halloumi.Shuffler.AudioEngine.ModulePlayer
{
    public class ModulePlayer : IBmpProvider
    {
        private const string SongKey = "Song";
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

        public void PlayModule()
        {
            var section = _mainPlayer.GetAudioSection(SongKey, SongKey);
            section.LoopIndefinitely = false;
            _mainPlayer.QueueSection(SongKey, SongKey);
            _mainPlayer.Play(SongKey);
        }

        public void PlayModuleLooped()
        {
            var section = _mainPlayer.GetAudioSection(SongKey, SongKey);
            section.LoopIndefinitely = true;
            _mainPlayer.QueueSection(SongKey, SongKey);
            _mainPlayer.Play(SongKey);
        }


        public void Pause()
        {
            _mainPlayer.Pause();
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

        public void LoadModule(Module module)
        {
            Pause();
            _mainPlayer.UnloadAll();

            _targetBpm = module.Bpm;
            _loopLength = BpmHelper.GetDefaultLoopLength(_targetBpm);

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

        public void SetBPM(decimal bpm)
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
                    _mainPlayer.AddEvent(SongKey, position, channelSequenceKey, channelSequenceKey,
                        EventType.Play, player);
                }
                patternIndex++;
            }
        }

        private void LoadPatternSequencePlayer(Module module)
        {
            _mainPlayer.Load(SongKey, SilenceHelper.GetSilenceAudioFile());

            var songLength = _loopLength*module.Sequence.Count;

            _mainPlayer.AddSection(SongKey, SongKey, 0, songLength, bpm: _targetBpm);
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
                player.AddSection(channelSequenceKey, channelSequenceKey, 0, _loopLength, bpm: _targetBpm);

                var rowIndex = 0;
                foreach (var row in channelSequence)
                {
                    var loopPosition = 0.0;
                    if (rowIndex > 0)
                        loopPosition = rowIndex / (double)channelSequence.Count;

                    var position = GetLoopPosition(loopPosition, _targetBpm);

                    player.AddPlayEvent(channelSequenceKey, position, row, row);

                    rowIndex++;
                }

                channelIndex++;
            }
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
                channelPlayer.AddSection(fullSampleKey,
                    fullSampleKey,
                    sample.Start,
                    sample.Length,
                    sample.Offset,
                    calculateBpmFromLength:true,
                    targetBpm: module.Bpm);
            }
        }

        private static double GetLoopPosition(double loopCount, decimal bpm)
        {
            return BpmHelper.GetDefaultLoopLength(bpm)*loopCount;
        }

        public void PlayPattern(string patternKey)
        {
            var section = _mainPlayer.GetAudioSection(SongKey, SongKey);
            section.LoopIndefinitely = true;
            _mainPlayer.QueueSection(SongKey, SongKey);
            _mainPlayer.Play(SongKey);
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