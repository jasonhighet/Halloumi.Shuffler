﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Halloumi.BassEngine.Models
{
    public class AudioData
    {
        public string Filename { get; set; }

        public GCHandle DataHandle { get; set; }

        public byte[] Data { get; set; }

        public IntPtr DataPointer => DataHandle.AddrOfPinnedObject();

        public List<AudioStream> AudioStreams { get; set; }

        public AudioData()
        {
            AudioStreams = new List<AudioStream>();
        }
    }
}