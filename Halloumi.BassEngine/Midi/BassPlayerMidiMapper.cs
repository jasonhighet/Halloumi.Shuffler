using Halloumi.Shuffler.AudioEngine.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using Halloumi.Shuffler.AudioEngine.BassPlayer;
using Un4seen.Bass.AddOn.Vst;
using System.IO;
using Newtonsoft.Json;

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

            var mappingFile = "MidiMapping.json";
            if (File.Exists(mappingFile))
            {
                var json = File.ReadAllText(mappingFile);
                var midiMapping = JsonConvert.DeserializeObject<MidiMapping>(json);

                _controlMappings = midiMapping.Commands.Select(command => new ControlMapping()
                {
                    CommandName = command.Key,
                    MidiControlId = midiMapping.Controls.FirstOrDefault(control => control.Key == command.Value).Value
                }).ToList();

                _vstMappings = new List<VstMapping>();
                foreach (var vstCommand in midiMapping.VstCommands)
                {
                    foreach (var parameter in vstCommand.Value)
                    {
                        _vstMappings.Add(new VstMapping
                        {
                            VstPlugin = GetPluginByName(vstCommand.Key),
                            MidiControlId = midiMapping.Controls.FirstOrDefault(control => control.Key == parameter.Value).Value,
                            ParameterIndex = parameter.Key,
                        });
                    }
                }
            }

            midiManager.OnControlMessageEvent += MidiManager_OnControlMessageEvent;
        }

        private VstPlugin GetPluginByName(string name)
        {
            switch (name)
            {
                case "SamplerVstPlugin":
                    return _bassPlayer.SamplerVstPlugin;
                case "SamplerVstPlugin2":
                    return _bassPlayer.SamplerVstPlugin2;
                case "TrackVstPlugin":
                    return _bassPlayer.TrackVstPlugin;
                case "TrackSendFxVstPlugin":
                    return _bassPlayer.TrackSendFxVstPlugin;
                case "TrackSendFxVstPlugin2":
                    return _bassPlayer.TrackSendFxVstPlugin2;
                case "MainVstPlugin":
                    return _bassPlayer.MainVstPlugin;
                case "MainVstPlugin2":
                    return _bassPlayer.MainVstPlugin2;
                default:
                    return _bassPlayer.MainVstPlugin;
            }

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
            else if (controlMapping.CommandName == "TrackSendFxHalf")
            {
                if (IsControlOn(midiValue, controlMapping))
                    _bassPlayer.StartTrackFxSendHalf();
                else
                    _bassPlayer.StopTrackFxSend();
            }
            else if (controlMapping.CommandName == "TrackSendFxQuarter")
            {
                if (IsControlOn(midiValue, controlMapping))
                    _bassPlayer.StartTrackFxSendQuarter();
                else
                    _bassPlayer.StopTrackFxSend();
            }
            else if (controlMapping.CommandName == "TrackSendFxEighth")
            {
                if (IsControlOn(midiValue, controlMapping))
                    _bassPlayer.StartTrackFxSendEighth();
                else
                    _bassPlayer.StopTrackFxSend();
            }
            else if (controlMapping.CommandName == "TrackSendFxDottedQuarter")
            {
                if (IsControlOn(midiValue, controlMapping))
                    _bassPlayer.StartTrackFxSendDottedQuarter();
                else
                    _bassPlayer.StopTrackFxSend();
            }
            else if (controlMapping.CommandName == "TrackSendFxDottedEighth")
            {
                if (IsControlOn(midiValue, controlMapping))
                    _bassPlayer.StartTrackFxSendDottedEighth();
                else
                    _bassPlayer.StopTrackFxSend();
            }
            else if (controlMapping.CommandName == "TrackSendFxSixteenth")
            {
                if (IsControlOn(midiValue, controlMapping))
                    _bassPlayer.StartTrackFxSendSixteenth();
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
            else if (controlMapping.CommandName.StartsWith("1Sample"))
            {
                var index = int.Parse(controlMapping.CommandName.Replace("1Sample", ""));
                if (IsControlOn(midiValue, controlMapping))
                    _bassPlayer.PlaySample(1, index);
                else
                    _bassPlayer.PauseSample(1, index);
            }
            else if (controlMapping.CommandName.StartsWith("2Sample"))
            {
                var index = int.Parse(controlMapping.CommandName.Replace("2Sample", ""));
                if (IsControlOn(midiValue, controlMapping))
                    _bassPlayer.PlaySample(2, index);
                else
                    _bassPlayer.PauseSample(2, index);
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
            public int MinVstValue { get; }
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