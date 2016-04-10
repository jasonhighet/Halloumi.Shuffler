using Halloumi.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halloumi.Shuffler.AudioEngine.Midi
{
    public class BassPlayerMidiMapper
    {
        private BassPlayer _bassPlayer;
        private MidiManager _midiManager;
        private List<ControlMapping> _controlMappings;

        public BassPlayerMidiMapper(BassPlayer bassPlayer, MidiManager midiManager)
        {
            _bassPlayer = bassPlayer;
            _midiManager = midiManager;

            _controlMappings = new List<ControlMapping>();
            _controlMappings.Add(new ControlMapping { CommandName = "Play", ControlId = 45 });
            _controlMappings.Add(new ControlMapping { CommandName = "Pause", ControlId = 46 });
            _controlMappings.Add(new ControlMapping { CommandName = "Volume", ControlId = 14 });
            _controlMappings.Add(new ControlMapping { CommandName = "PausePrevious", ControlId = 23 });
            _controlMappings.Add(new ControlMapping { CommandName = "PowerDownPrevious", ControlId = 33 });
            _controlMappings.Add(new ControlMapping { CommandName = "ManualMixVolume", ControlId = 2 });
            _controlMappings.Add(new ControlMapping { CommandName = "FadeNow", ControlId = 48 });
            _controlMappings.Add(new ControlMapping { CommandName = "TrackSendFx", ControlId = 24 });

            _midiManager.OnControlMessageEvent += MidiManager_OnControlMessageEvent;
        }

        private void MidiManager_OnControlMessageEvent(ControlMessageEventArgs e)
        {
            DebugHelper.WriteLine("MidiMessage - ControlId:" + e.ControlId.ToString() + " Value:" + e.Value.ToString());

            var controlMapping = _controlMappings.FirstOrDefault(x => x.ControlId == e.ControlId);
            if (controlMapping == null)
                return;

            DebugHelper.WriteLine("ControlMapped - " + controlMapping.CommandName);

            if (controlMapping.CommandName == "Play")
            {
                if (e.Value != 0)
                    _bassPlayer.Play();
            }
            else if (controlMapping.CommandName == "TrackSendFx")
            {
                if (e.Value != 0)
                    _bassPlayer.StartTrackFxSend();
                else
                    _bassPlayer.StopTrackFxSend();
            }
            else if (controlMapping.CommandName == "FadeNow")
            {
                if (e.Value != 0)
                    _bassPlayer.ForceFadeNow(ForceFadeType.SkipToEnd);
            }
            else if (controlMapping.CommandName == "Volume")
            {
                var volume = (Convert.ToDecimal(e.Value) / 127M) * 100;
                _bassPlayer.SetMixerVolume(volume);
            }
            else if (controlMapping.CommandName == "Pause")
            {
                if (e.Value != 0)
                    _bassPlayer.Pause();
            }
            else if (controlMapping.CommandName == "ManualMixVolume")
            {
                var volume = (Convert.ToDecimal(e.Value) / 127M) * 100;
                _bassPlayer.SetManualMixVolume(volume);
            }
            else if (controlMapping.CommandName == "PowerDownPrevious")
            {
                if (e.Value != 0)
                    _bassPlayer.PowerOffPreviousTrack();
            }
            else if (controlMapping.CommandName == "PausePrevious")
            {
                if (e.Value != 0)
                    _bassPlayer.PausePreviousTrack();
            }

        }

        private class ControlMapping
        {
            public string CommandName { get; set; }
            public int ControlId { get; set; }
        }

    }
}
