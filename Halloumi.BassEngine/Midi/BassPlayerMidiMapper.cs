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

            _midiManager.OnControlMessageEvent += MidiManager_OnControlMessageEvent;
        }

        private void MidiManager_OnControlMessageEvent(ControlMessageEventArgs e)
        {            DebugHelper.WriteLine("MidiMessage - ControlId:" + e.ControlId.ToString() + " Value:" + e.Value.ToString());

            var controlMapping = _controlMappings.FirstOrDefault(x => x.ControlId == e.ControlId);
            if (controlMapping == null)
                return;

            DebugHelper.WriteLine("ControlMapped - " + controlMapping.CommandName);

            if (controlMapping.CommandName == "Play")
            {
                if (e.Value != 0)
                    _bassPlayer.Play();
            }
            if (controlMapping.CommandName == "Pause")
            {
                if (e.Value != 0)
                    _bassPlayer.Pause();
            }
            if (controlMapping.CommandName == "Volume")
            {
                var volume = (Convert.ToDecimal(e.Value) / 127M) * 100;
                    _bassPlayer.SetMixerVolume(volume);
            }

        }

        private class ControlMapping
        {
            public string CommandName { get; set; }
            public int ControlId { get; set; }
        }

    }
}
