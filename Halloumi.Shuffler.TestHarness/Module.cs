using System.Collections.Generic;

namespace Halloumi.Shuffler.TestHarness
{
    public class Module
    {
        public string Title { get; set; }
        public decimal Bpm { get; set; }
        public List<AudioFile> AudioFiles { get; set; }
        public List<Channel> Channels { get; set; }
        public List<Pattern> Patterns { get; set; }
        public List<string> Sequence { get; set; }

        public class Sample
        {
            public string Key { get; set; }
            public double Start { get; set; }
            public double End { get; set; }
            public double? Offset { get; set; }
        }

        public class AudioFile
        {
            public string Key { get; set; }
            public string Path { get; set; }
            public List<Sample> Samples { get; set; }
        }

        public class Channel
        {
            public string Key { get; set; }
        }

        public class Pattern
        {
            public string Key { get; set; }
            public List<List<string>> Sequence { get; set; }
        }
    }
}