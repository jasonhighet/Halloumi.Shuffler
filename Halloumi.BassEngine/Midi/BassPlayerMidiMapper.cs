using System;
using System.Collections.Generic;
using System.Linq;

namespace Halloumi.Shuffler.AudioEngine.Midi
{
    public class BassPlayerMidiMapper
    {
        private readonly BassPlayer _bassPlayer;
        private readonly List<ControlMapping> _controlMappings;

        public BassPlayerMidiMapper(BassPlayer bassPlayer, MidiManager midiManager)
        {
            _bassPlayer = bassPlayer;

            _controlMappings = new List<ControlMapping>
            {
                new ControlMapping {CommandName = "Play", ControlId = 45},
                new ControlMapping {CommandName = "PowerDownCurrent", ControlId = 46},
                new ControlMapping {CommandName = "Volume", ControlId = 2},
                new ControlMapping {CommandName = "PausePrevious", ControlId = 23},
                new ControlMapping {CommandName = "PowerDownPrevious", ControlId = 33},
                new ControlMapping {CommandName = "ManualMixVolume", ControlId = 14},
                new ControlMapping {CommandName = "FadeNow", ControlId = 48},
                new ControlMapping {CommandName = "TrackSendFx", ControlId = 24},
                new ControlMapping {CommandName = "TrackSendFxVolume", ControlId = 3},
                new ControlMapping {CommandName = "SamplerVolume", ControlId = 4},
                new ControlMapping {CommandName = "ToggleManualMixMode", ControlId = 44},
                new ControlMapping {CommandName = "LoopFadeInForever", ControlId = 49},
                new ControlMapping {CommandName = "JumpBack", ControlId = 47}
            };

            for (var i = 0; i < 12; i++)
            {
                _controlMappings.Add(new ControlMapping
                {
                    CommandName = "Sample" + (i + 1),
                    ControlId =  (i < 6) ?  i + 26 : i + 36
                });
            }

            midiManager.OnControlMessageEvent += MidiManager_OnControlMessageEvent;
        }

        private void MidiManager_OnControlMessageEvent(ControlMessageEventArgs e)
        {
            var controlMapping = _controlMappings.FirstOrDefault(x => x.ControlId == e.ControlId);
            if (controlMapping == null)
                return;

            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (controlMapping.CommandName == "Play")
            {
                if (IsControlOn(e.Value, controlMapping))
                    _bassPlayer.Play();
            }
            else if (controlMapping.CommandName == "TrackSendFx")
            {
                if (IsControlOn(e.Value, controlMapping))
                    _bassPlayer.StartTrackFxSend();
                else
                    _bassPlayer.StopTrackFxSend();
            }
            else if (controlMapping.CommandName == "FadeNow")
            {
                if (IsControlOn(e.Value, controlMapping))
                    _bassPlayer.ForceFadeNow(ForceFadeType.SkipToEnd);
            }
            else if (controlMapping.CommandName == "Volume")
            {
                var volume = GetPercentage(e.Value, controlMapping);
                _bassPlayer.SetMixerVolume(volume);
            }
            else if (controlMapping.CommandName == "PowerDownCurrent")
            {
                if (IsControlOn(e.Value, controlMapping))
                    _bassPlayer.PowerOffCurrentTrack();
            }
            else if (controlMapping.CommandName == "ManualMixVolume")
            {
                var volume = GetPercentage(e.Value, controlMapping);
                _bassPlayer.SetManualMixVolume(volume);
            }
            else if (controlMapping.CommandName == "PowerDownPrevious")
            {
                if (IsControlOn(e.Value, controlMapping))
                    _bassPlayer.PowerOffPreviousTrack();
            }
            else if (controlMapping.CommandName == "PausePrevious")
            {
                if (IsControlOn(e.Value, controlMapping))
                    _bassPlayer.PausePreviousTrack();
            }
            else if (controlMapping.CommandName == "TrackSendFxVolume")
            {
                var volume = GetPercentage(e.Value, controlMapping);
                _bassPlayer.SetTrackSendFxVolume(volume);
            }
            else if (controlMapping.CommandName == "SamplerVolume")
            {
                var volume = GetPercentage(e.Value, controlMapping);
                _bassPlayer.SetSamplerMixerVolume(volume);
            }
            else if (controlMapping.CommandName == "ToggleManualMixMode")
            {
                if (IsControlOn(e.Value, controlMapping))
                    _bassPlayer.ToggleManualMixMode();
            }
            else if (controlMapping.CommandName == "LoopFadeInForever")
            {
                if (IsControlOn(e.Value, controlMapping))
                    _bassPlayer.LoopFadeInForever = !_bassPlayer.LoopFadeInForever;
            }
            else if (controlMapping.CommandName == "JumpBack")
            {
                if (IsControlOn(e.Value, controlMapping))
                    _bassPlayer.JumpBack();
            }
            else if (controlMapping.CommandName.StartsWith("Sample"))
            {
                var index = int.Parse(controlMapping.CommandName.Replace("Sample", ""));
                if (IsControlOn(e.Value, controlMapping))
                    _bassPlayer.PlaySample(index);
                else
                    _bassPlayer.PauseSample(index);
            }

        }

        private static bool IsControlOn(int value, ControlMapping controlMapping)
        {
            return value != controlMapping.MinValue;
        }

        private static decimal GetPercentage(int value, ControlMapping controlMapping)
        {
            var adjustedValue = Convert.ToDecimal(value - controlMapping.MinValue);
            var adjustedMax = Convert.ToDecimal(controlMapping.MaxValue - controlMapping.MinValue);
            return adjustedValue/adjustedMax*100;
        }

        private class ControlMapping
        {
            public ControlMapping()
            {
                MinValue = 0;
                MaxValue = 127;
            }

            public string CommandName { get; set; }
            public int ControlId { get; set; }

            public int MinValue { get; }
            public int MaxValue { get; }
        }
    }
}