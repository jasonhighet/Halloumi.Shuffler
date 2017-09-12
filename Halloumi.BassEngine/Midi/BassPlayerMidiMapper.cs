using Halloumi.Shuffler.AudioEngine.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using Halloumi.Shuffler.AudioEngine.BassPlayer;
using Un4seen.Bass.AddOn.Vst;

namespace Halloumi.Shuffler.AudioEngine.Midi
{
    public class BassPlayerMidiMapper
    {
        private readonly BassPlayer.BassPlayer _bassPlayer;
        private readonly List<ControlMapping> _controlMappings;
        private readonly List<VstMapping> _vstMappings;

        public BassPlayerMidiMapper(BassPlayer.BassPlayer bassPlayer, MidiManager midiManager)
        {
            _bassPlayer = bassPlayer;

            _controlMappings = new List<ControlMapping>
            {
                new ControlMapping {CommandName = "Play", MidiControlId = 45},
                new ControlMapping {CommandName = "PowerDownCurrent", MidiControlId = 46},
                new ControlMapping {CommandName = "Volume", MidiControlId = 2},
                new ControlMapping {CommandName = "PausePrevious", MidiControlId = 23},
                new ControlMapping {CommandName = "PowerDownPrevious", MidiControlId = 33},
                new ControlMapping {CommandName = "ManualMixVolume", MidiControlId = 14},
                new ControlMapping {CommandName = "FadeNow", MidiControlId = 48},
                new ControlMapping {CommandName = "TrackSendFx", MidiControlId = 24},
                new ControlMapping {CommandName = "TrackSendFxVolume", MidiControlId = 3},
                new ControlMapping {CommandName = "SamplerVolume", MidiControlId = 4},
                new ControlMapping {CommandName = "ToggleManualMixMode", MidiControlId = 44},
                new ControlMapping {CommandName = "LoopFadeInForever", MidiControlId = 49},
                new ControlMapping {CommandName = "JumpBack", MidiControlId = 47}
            };

            for (var i = 0; i < 12; i++)
            {
                _controlMappings.Add(new ControlMapping
                {
                    CommandName = "Sample" + (i + 1),
                    MidiControlId = (i < 6) ? i + 26 : i + 36
                });
            }

            _vstMappings = new List<VstMapping>
            {
                new VstMapping
                {
                    VstPlugin = _bassPlayer.MainVstPlugin,
                    MidiControlId = 17,
                    ParameterIndex = 4,
                }
            };


            midiManager.OnControlMessageEvent += MidiManager_OnControlMessageEvent;
        }

        private void MidiManager_OnControlMessageEvent(ControlMessageEventArgs e)
        {
            var controlMapping = _controlMappings.FirstOrDefault(x => x.MidiControlId == e.ControlId);
            if (controlMapping != null)
            {
                ProcessControlMapping(e.Value, controlMapping);
            }
            else
            {
                var vstMapping = _vstMappings.FirstOrDefault(x => x.MidiControlId == e.ControlId);
                if (vstMapping != null)
                    ProcessVstMapping(e.Value, vstMapping);
            }
        }

        private void ProcessVstMapping(int value, VstMapping vstMapping)
        {
            if (vstMapping.VstPlugin == null)
                return;
            if (vstMapping.ParameterIndex >= vstMapping.VstPlugin.Parameters.Count)
                return;
            
            var plugin = vstMapping.VstPlugin;
            var parameter = vstMapping.VstPlugin.Parameters[vstMapping.ParameterIndex];

            var vstValue = GetVstValue(value, vstMapping);
            BassVst.BASS_VST_SetParam(plugin.Id, parameter.Id, vstValue);
        }

        private void ProcessControlMapping(int midiValue, ControlMapping controlMapping)
        {
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (controlMapping.CommandName == "Play")
            {
                if (IsControlOn(midiValue, controlMapping))
                    _bassPlayer.Play();
            }
            else if (controlMapping.CommandName == "TrackSendFx")
            {
                if (IsControlOn(midiValue, controlMapping))
                    _bassPlayer.StartTrackFxSend();
                else
                    _bassPlayer.StopTrackFxSend();
            }
            else if (controlMapping.CommandName == "FadeNow")
            {
                if (IsControlOn(midiValue, controlMapping))
                    _bassPlayer.ForceFadeNow(ForceFadeType.SkipToEnd);
            }
            else if (controlMapping.CommandName == "Volume")
            {
                var volume = GetPercentage(midiValue, controlMapping);
                _bassPlayer.SetMixerVolume(volume);
            }
            else if (controlMapping.CommandName == "PowerDownCurrent")
            {
                if (IsControlOn(midiValue, controlMapping))
                    _bassPlayer.PowerOffCurrentTrack();
            }
            else if (controlMapping.CommandName == "ManualMixVolume")
            {
                var volume = GetPercentage(midiValue, controlMapping);
                _bassPlayer.SetManualMixVolume(volume);
            }
            else if (controlMapping.CommandName == "PowerDownPrevious")
            {
                if (IsControlOn(midiValue, controlMapping))
                    _bassPlayer.PowerOffPreviousTrack();
            }
            else if (controlMapping.CommandName == "PausePrevious")
            {
                if (IsControlOn(midiValue, controlMapping))
                    _bassPlayer.PausePreviousTrack();
            }
            else if (controlMapping.CommandName == "TrackSendFxVolume")
            {
                var volume = GetPercentage(midiValue, controlMapping);
                _bassPlayer.SetTrackSendFxVolume(volume);
            }
            else if (controlMapping.CommandName == "SamplerVolume")
            {
                var volume = GetPercentage(midiValue, controlMapping);
                _bassPlayer.SetSamplerMixerVolume(volume);
            }
            else if (controlMapping.CommandName == "ToggleManualMixMode")
            {
                if (IsControlOn(midiValue, controlMapping))
                    _bassPlayer.ToggleManualMixMode();
            }
            else if (controlMapping.CommandName == "LoopFadeInForever")
            {
                if (IsControlOn(midiValue, controlMapping))
                    _bassPlayer.LoopFadeInForever = !_bassPlayer.LoopFadeInForever;
            }
            else if (controlMapping.CommandName == "JumpBack")
            {
                if (IsControlOn(midiValue, controlMapping))
                    _bassPlayer.JumpBack();
            }
            else if (controlMapping.CommandName.StartsWith("Sample"))
            {
                var index = int.Parse(controlMapping.CommandName.Replace("Sample", ""));
                if (IsControlOn(midiValue, controlMapping))
                    _bassPlayer.PlaySample(index);
                else
                    _bassPlayer.PauseSample(index);
            }
        }

        private static bool IsControlOn(int value, ControlMapping controlMapping)
        {
            return value != controlMapping.MinMidiValue;
        }

        private static decimal GetPercentage(int value, ControlMapping controlMapping)
        {
            var adjustedValue = Convert.ToDecimal(value - controlMapping.MinMidiValue);
            var adjustedMax = Convert.ToDecimal(controlMapping.MaxMidiValue - controlMapping.MinMidiValue);
            return adjustedValue / adjustedMax * 100;
        }

        private static float GetVstValue(int value, VstMapping vstMapping)
        {
            var adjustedMidiValue = Convert.ToSingle(value - vstMapping.MinMidiValue);
            var adjustedMidiMax = Convert.ToSingle(vstMapping.MaxMidiValue - vstMapping.MinMidiValue);
            return adjustedMidiValue / adjustedMidiMax;
        }


        private class VstMapping
        {
            public VstMapping()
            {
                MinMidiValue = 0;
                MaxMidiValue = 127;

                MinVstValue = 0;
                MaxVstValue = 1;
            }

            public VstPlugin VstPlugin { get; set; }
            public int ParameterIndex { get; set; }
            public int MinVstValue { get;  }
            public int MaxVstValue { get; }

            public int MidiControlId { get; set; }
            public int MinMidiValue { get; }
            public int MaxMidiValue { get; }
        }

        private class ControlMapping
        {
            public ControlMapping()
            {
                MinMidiValue = 0;
                MaxMidiValue = 127;
            }

            public string CommandName { get; set; }
            public int MidiControlId { get; set; }
            public int MinMidiValue { get; }
            public int MaxMidiValue { get; }
        }
    }
}