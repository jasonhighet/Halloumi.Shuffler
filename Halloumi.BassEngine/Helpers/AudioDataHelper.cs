using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Halloumi.BassEngine.Models;

namespace Halloumi.BassEngine.Helpers
{
    public static class AudioDataHelper
    {
        static AudioDataHelper()
        {
            LoadedData = new List<AudioData>();
        }

        private static List<AudioData> LoadedData { get; }

        public static AudioData LoadAudioData(AudioStream audioStream)
        {
            lock (LoadedData)
            {
                var audioData = LoadedData.FirstOrDefault(x => x.Filename == audioStream.Filename);
                if (audioData == null)
                {
                    audioData = new AudioData
                    {
                        Filename = audioStream.Filename,
                        Data = File.ReadAllBytes(audioStream.Filename)
                    };

                    audioData.DataHandle = GCHandle.Alloc(audioData.Data, GCHandleType.Pinned);
                    LoadedData.Add(audioData);
                }

                audioStream.AudioData = audioData;
                audioData.AudioStreams.Add(audioStream);

                return audioData;
            }
        }

        public static void UnloadAudioData(AudioStream audioStream)
        {
            lock (LoadedData)
            {
                var audioData = LoadedData.FirstOrDefault(x => x.Filename == audioStream.Filename);

                if (audioData == null)
                    return;

                audioData.AudioStreams.Remove(audioStream);
                audioStream.AudioData = null;

                if (audioData.AudioStreams.Count > 0)
                    return;

                LoadedData.Remove(audioData);
                audioData.DataHandle.Free();
                audioData.Data = null;
            }
        }
    }
}