using System;
using System.Collections.Generic;
using Halloumi.Shuffler.AudioEngine.Channels;
using Halloumi.Shuffler.AudioEngine.Helpers;

namespace Halloumi.Shuffler.AudioEngine.Players
{
    public class SyncedSamplePlayer : IBmpProvider
    {
        private const string PatternPlayer = "Pattern";
        private readonly AudioPlayer _mainPlayer;
        private readonly List<AudioPlayer> _channelPlayers = new List<AudioPlayer>();
        private readonly List<string> _sampleKeys =  new List<string>();
        private double _loopLength;
        private decimal _targetBpm = int.MinValue;

        public SyncedSamplePlayer()
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

        public void SetBpm(decimal bpm)
        {
            _targetBpm = bpm;
            _loopLength = BpmHelper.GetDefaultLoopLength(_targetBpm);
        }

        public void AddSample(string sampleKey, string path, double start, double length, double offset, bool loopIndefinitely = true)
        {
            _sampleKeys.Add(sampleKey);

            if (_targetBpm == int.MinValue)
            {
                _targetBpm = BpmHelper.GetBpmFromLoopLength(length);
            }

            AudioPlayer channelPlayer;
            if (_sampleKeys.Count > _channelPlayers.Count)
            {
                channelPlayer = new AudioPlayer();
                Output.AddInputChannel(channelPlayer.Output);
                _channelPlayers.Add(channelPlayer);
            }
            else
            {
                channelPlayer = _channelPlayers[_sampleKeys.Count - 1];
            }

            channelPlayer.UnloadAll();
            channelPlayer.Load(sampleKey, path);
            var section = channelPlayer.AddSection(sampleKey,
                sampleKey, 
                start, 
                length, 
                offset, 
                calculateBpmFromLength: true,
                targetBpm: _targetBpm);

            section.LoopIndefinitely = loopIndefinitely;
        }

        public List<string> GetSampleKeys()
        {
            return _sampleKeys;
        }

        public void Play()
        {

            _mainPlayer.Unload(PatternPlayer);
            _mainPlayer.Load(PatternPlayer, SilenceHelper.GetSilenceAudioFile());
            _mainPlayer.AddSection(PatternPlayer, PatternPlayer, 0, _loopLength, bpm: _targetBpm);

            var section = _mainPlayer.GetAudioSection(PatternPlayer, PatternPlayer);
            section.LoopIndefinitely = true;

            for (var channelIndex = 0; channelIndex < _sampleKeys.Count; channelIndex++)
            {
                var positions = GetPositions(channelIndex);
                var player = _channelPlayers[channelIndex];

                foreach (var position in positions)
                {
                    var currentPosition = position.Item2;
                        _mainPlayer.AddEvent(PatternPlayer, currentPosition, position.Item1, position.Item1,
                            EventType.PlaySolo, player);
                }
            }

            _mainPlayer.QueueSection(PatternPlayer, PatternPlayer);
            _mainPlayer.Play(PatternPlayer);
        }

        private IEnumerable<Tuple<string, double>> GetPositions(int channelIndex)
        {
            var positions = new List<Tuple<string, double>>();

            var currentPosition = 0D;
            for(var i = 0; i <32; i++)
            {
                var sampleKey = _sampleKeys[channelIndex];
                var audioSection = _channelPlayers[channelIndex].GetAudioSection(sampleKey, sampleKey);

                var sampleLength = audioSection.Length;
                var bpm = audioSection.Bpm;
                var adjustedLength = BpmHelper.GetAdjustedAudioLength(sampleLength, bpm, _targetBpm);
                
                positions.Add(new Tuple<string, double>(sampleKey, currentPosition));

                currentPosition += adjustedLength;

                if (currentPosition > _loopLength)
                    break;
            }

            return positions;
        }


        public void Pause()
        {
            _mainPlayer.Pause();
            foreach (var player in _channelPlayers)
            {
                player.Pause();
            }
        }

        public void UnloadAll()
        {
            Pause();
            _mainPlayer.UnloadAll();
            foreach (var channelPlayer in _channelPlayers)
            {
                channelPlayer.UnloadAll();
            }

            _sampleKeys.Clear();
            _targetBpm = int.MinValue;

        }

        public void Mute()
        {
            foreach (var player in _channelPlayers)
            {
                try
                {
                    var channelIndex = _channelPlayers.IndexOf(player);
                    if (channelIndex >= _sampleKeys.Count)
                        continue;

                    var key = _sampleKeys[channelIndex];
                    player.Mute(key);
                }
                catch
                {
                    // ignored
                }
            }
        }

        public void Unmute()
        {
            foreach (var player in _channelPlayers)
            {
                try
                {
                    var channelIndex = _channelPlayers.IndexOf(player);
                    if (channelIndex >= _sampleKeys.Count)
                        continue;
                       
                    var key = _sampleKeys[channelIndex];
                    player.Unmute(key);
                }
                catch { }
                
            }
        }
    }
}