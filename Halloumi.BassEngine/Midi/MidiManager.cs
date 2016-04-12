using System;
using Sanford.Multimedia.Midi;

namespace Halloumi.Shuffler.AudioEngine.Midi
{
    public class MidiManager
    {
        private InputDevice _inDevice;

        public MidiManager()
        {
            if (InputDevice.DeviceCount == 0)
                return;

            _inDevice = new InputDevice(0);
            _inDevice.ChannelMessageReceived += InDevice_ChannelMessageReceived;
            _inDevice.StartRecording();
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