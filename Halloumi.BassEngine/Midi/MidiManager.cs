using System;
using System.IO;
using Newtonsoft.Json;
using Sanford.Multimedia.Midi;

namespace Halloumi.Shuffler.AudioEngine.Midi
{
    public class MidiManager
    {
        private InputDevice _inDevice;

        public MidiManager()
        {
            var mappingFile = "MidiMapping.json";
            if (!File.Exists(mappingFile)) return;

            var json = File.ReadAllText(mappingFile);
            var midiMapping = JsonConvert.DeserializeObject<MidiMapping>(json);

            int deviceCount = InputDevice.DeviceCount;
            for (int i = 0; i < deviceCount; i++)
            {
                var details = InputDevice.GetDeviceCapabilities(i);

                if (details.name.ToLower() != midiMapping.DeviceName.ToLower())
                    continue;

                _inDevice = new InputDevice(i);
                _inDevice.ChannelMessageReceived += InDevice_ChannelMessageReceived;
                _inDevice.StartRecording();

                break;
            }
        }

        private void InDevice_ChannelMessageReceived(object sender, ChannelMessageEventArgs e)
        {
            // raise event
            if (OnControlMessageEvent == null) return;
            if (e.Message.Command != ChannelCommand.Controller) return;

            OnControlMessageEvent(new ControlMessageEventArgs
            {
                ControlId = e.Message.Data1,
                Value = e.Message.Data2
            });
        }

        public void Dispose()
        {
            if (_inDevice == null) return;
            _inDevice.StopRecording();
            _inDevice.Dispose();
            _inDevice = null;
        }

        public event ControlMessageEventDelegate OnControlMessageEvent;
    }

    public delegate void ControlMessageEventDelegate(ControlMessageEventArgs e);

    public class ControlMessageEventArgs : EventArgs
    {
        public int ControlId { get; set; }
        public int Value { get; set; }
    }
}