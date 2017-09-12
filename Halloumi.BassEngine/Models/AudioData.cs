using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Halloumi.Shuffler.AudioEngine.Models
{
    public class AudioData
    {
        public string Filename { get; set; }

        public GCHandle DataHandle { get; set; }

        public byte[] Data { get; set; }

        public IntPtr DataPointer
        {
            get { return DataHandle.AddrOfPinnedObject(); }
        }

        public List<AudioStream> AudioStreams { get; set; }

        public AudioData()
        {
            AudioStreams = new List<AudioStream>();
        }
    }
}