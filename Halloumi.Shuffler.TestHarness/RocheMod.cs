using System.Collections.Generic;

namespace Halloumi.Shuffler.TestHarness
{
    public class RocheMod
    {
        public string Title { get; set; }
        public int Bpm { get; set; }
        public List<AudioFile> AudioFiles { get; set; }
        public List<Channel> Channels { get; set; }
        public List<List<string>> Patterns { get; set; }

        public class AudioFile
        {
            public string Key { get; set; }
            public string Path { get; set; }
            public List<Section> Sections { get; set; }
        }

        public class Section
        {
            public string Key { get; set; }
            public double Start { get; set; }
            public double End { get; set; }
            public double? Offset { get; set; }
            public string AudioFileKey { get; set; }
        }

        public class Loop
        {
            public string Key { get; set; }
            public List<string> Sections { get; set; }
        }

        public class Channel
        {
            public string Title { get; set; }
            public List<Loop>Loops { get; set; }
        }
    }
}