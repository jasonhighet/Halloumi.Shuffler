using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Halloumi.Shuffler.AudioEngine.Channels;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;

namespace Halloumi.Shuffler.AudioEngine.BassPlayer
{
    /// <summary>
    ///     A text report of what the engine is doing, for tracking down glitches such as clicking.
    ///     It answers two questions: is the output buffer running dry (the audio thread is starved,
    ///     by CPU or by a wait), and is anything playing that shouldn't be (a stream left running
    ///     after a transition, a stuck sample, the FX send left open)?
    ///     Read-only: it takes no locks and changes nothing, so it is safe to run while mixing.
    /// </summary>
    internal class AudioDiagnostics
    {
        /// <summary>
        ///     Below this much buffered audio the output is close to an underrun.
        /// </summary>
        private const double LowBufferSeconds = 0.03;

        private const int SampleIntervalMilliseconds = 5;

        private readonly MixingDesk _desk;
        private readonly TrackDeck _deck;
        private readonly TrackLoader _loader;
        private readonly Sampler _sampler;

        public AudioDiagnostics(MixingDesk desk, TrackDeck deck, TrackLoader loader, Sampler sampler)
        {
            _desk = desk;
            _deck = deck;
            _loader = loader;
            _sampler = sampler;
        }

        /// <summary>
        ///     Watches the output for a while, then describes everything that is playing. Blocks for
        ///     the sampling time, so call it off the UI thread.
        /// </summary>
        /// <param name="sampleMilliseconds">How long to watch the output buffer and CPU.</param>
        public string Capture(int sampleMilliseconds)
        {
            var report = new StringBuilder();
            var notes = new List<string>();

            report.AppendLine("Audio diagnostics " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            report.AppendLine();

            AppendOutputSampling(report, notes, sampleMilliseconds);
            AppendStreams(report, notes);
            AppendMixers(report, notes);
            AppendPlugins(report);
            AppendProcess(report);

            report.AppendLine();
            report.AppendLine("Notes:");
            if (notes.Count == 0)
                notes.Add("Nothing unusual found.");
            foreach (var note in notes)
                report.AppendLine("  - " + note);

            return report.ToString();
        }

        // ---------- output buffer and CPU ----------

        private void AppendOutputSampling(StringBuilder report, List<string> notes, int sampleMilliseconds)
        {
            var output = _desk.SpeakerOutput.ChannelId;
            var gcBefore = GcCounts();

            var minBuffer = double.MaxValue;
            var totalBuffer = 0D;
            var maxCpu = 0F;
            var totalCpu = 0D;
            var lowCount = 0;
            var stalledCount = 0;
            var samples = 0;

            var stopwatch = Stopwatch.StartNew();
            while (stopwatch.ElapsedMilliseconds < sampleMilliseconds)
            {
                var bytes = Bass.BASS_ChannelGetData(output, IntPtr.Zero, (int)BASSData.BASS_DATA_AVAILABLE);
                var buffered = bytes < 0 ? 0 : Bass.BASS_ChannelBytes2Seconds(output, bytes);
                var cpu = Bass.BASS_GetCPU();

                minBuffer = Math.Min(minBuffer, buffered);
                totalBuffer += buffered;
                maxCpu = Math.Max(maxCpu, cpu);
                totalCpu += cpu;
                if (buffered < LowBufferSeconds) lowCount++;
                if (Bass.BASS_ChannelIsActive(output) == BASSActive.BASS_ACTIVE_STALLED) stalledCount++;
                samples++;

                Thread.Sleep(SampleIntervalMilliseconds);
            }

            var gcAfter = GcCounts();
            if (samples == 0) samples = 1;

            var bufferSetting = Bass.BASS_GetConfig(BASSConfig.BASS_CONFIG_BUFFER);
            var updatePeriod = Bass.BASS_GetConfig(BASSConfig.BASS_CONFIG_UPDATEPERIOD);

            report.AppendLine("Output buffer (speakers), watched for " + Seconds(sampleMilliseconds / 1000D) + ":");
            report.AppendLine("  buffer setting " + bufferSetting + " ms, update period " + updatePeriod + " ms");
            report.AppendLine("  buffered: min " + Ms(minBuffer) + ", average " + Ms(totalBuffer / samples)
                              + ", below " + Ms(LowBufferSeconds) + " in " + lowCount + " of " + samples + " readings");
            report.AppendLine("  stalled readings: " + stalledCount);
            report.AppendLine("  BASS CPU: average " + Percent(totalCpu / samples) + ", max " + Percent(maxCpu));
            report.AppendLine("  garbage collections meanwhile: gen0 " + (gcAfter[0] - gcBefore[0])
                              + ", gen1 " + (gcAfter[1] - gcBefore[1]) + ", gen2 " + (gcAfter[2] - gcBefore[2]));
            report.AppendLine();

            if (stalledCount > 0 || lowCount > 0)
                notes.Add("The output buffer ran low or dry: the audio thread isn't keeping up (CPU, a plug-in, "
                          + "or a wait on the mixer thread). Clicks are dropouts.");
            else
                notes.Add("The output buffer stayed full: the audio thread kept up, so clicks are in the audio "
                          + "itself (something playing or processing that shouldn't be), not dropouts.");

            if (maxCpu > 60)
                notes.Add("BASS CPU peaked at " + Percent(maxCpu) + " of the update period: the mixing (tracks, "
                          + "tempo, plug-ins) is close to taking longer than real time.");
        }

        private static int[] GcCounts()
        {
            return new[] { GC.CollectionCount(0), GC.CollectionCount(1), GC.CollectionCount(2) };
        }

        // ---------- streams ----------

        private void AppendStreams(StringBuilder report, List<string> notes)
        {
            var slots = new List<KeyValuePair<string, Track>>
            {
                new KeyValuePair<string, Track>("Previous", _deck.Previous),
                new KeyValuePair<string, Track>("Current", _deck.Current),
                new KeyValuePair<string, Track>("Next", _deck.Next),
                new KeyValuePair<string, Track>("Preloaded", _deck.Preloaded)
            };

            report.AppendLine("Track slots:");
            foreach (var slot in slots)
            {
                if (slot.Value == null)
                    report.AppendLine("  " + slot.Key + ": (empty)");
                else
                    report.AppendLine("  " + slot.Key + ": " + DescribeTrack(slot.Value));
            }

            var inSlots = slots.Select(s => s.Value).Where(t => t != null).ToList();
            var others = _loader.GetLoadedTracks().Where(t => !inSlots.Contains(t)).ToList();

            report.AppendLine("Other tracks with audio loaded: " + (others.Count == 0 ? "none" : ""));
            foreach (var track in others)
            {
                report.AppendLine("  " + DescribeTrack(track));
                if (IsPlayingInMixer(track))
                    notes.Add("\"" + track.Description + "\" is playing in a mixer but isn't in any slot: "
                              + "it was left running and nothing will stop it.");
            }

            var playingTracks = inSlots.Concat(others).Distinct().Where(IsPlayingInMixer).ToList();
            if (playingTracks.Count > 1)
                notes.Add(playingTracks.Count + " tracks are playing in the mixer at once. Normal during a fade; "
                          + "otherwise one was left running.");

            var previous = _deck.Previous;
            if (previous != null && IsPlayingInMixer(previous) && previous.IsLoopedAtEnd)
                notes.Add("The previous track is still playing on its end loop (loop " + (previous.CurrentEndLoop + 1)
                          + " of " + previous.EndLoopCount + "). In manual or extended-mix mode it loops until "
                          + "stopped; each loop jumps its position, which can click.");

            var samples = _sampler.GetSamples().Where(s => s.IsAudioLoaded()).ToList();
            var playingSamples = samples.Where(IsPlayingInMixer).ToList();
            report.AppendLine("Samples: " + samples.Count + " loaded, " + playingSamples.Count + " playing");
            foreach (var sample in playingSamples)
                report.AppendLine("  " + DescribeStream(sample));
            report.AppendLine();

            if (playingSamples.Count > 0)
                notes.Add(playingSamples.Count + " sample(s) playing. A short sample looping can sound like clicking.");
        }

        private static string DescribeTrack(Track track)
        {
            var text = DescribeStream(track);
            if (track.IsLoopedAtEnd)
                text += ", end loop " + (track.CurrentEndLoop + 1) + " of " + track.EndLoopCount;
            return text;
        }

        private static string DescribeStream(AudioStream stream)
        {
            var name = "\"" + stream.Description + "\"";
            if (!stream.IsAudioLoaded()) return name + ": audio not loaded";

            var channel = stream.ChannelId;
            var mixer = BassMix.BASS_Mixer_ChannelGetMixer(channel);
            var state = mixer == 0 ? "not in a mixer" : MixerState(channel);

            float tempo = 0;
            Bass.BASS_ChannelGetAttribute(channel, BASSAttribute.BASS_ATTRIB_TEMPO, ref tempo);

            var position = Bass.BASS_ChannelBytes2Seconds(channel, Bass.BASS_ChannelGetPosition(channel));
            var length = Bass.BASS_ChannelBytes2Seconds(channel, Bass.BASS_ChannelGetLength(channel));

            return name + ": " + state
                   + ", volume " + AudioStreamHelper.GetVolume(stream).ToString("0", CultureInfo.InvariantCulture)
                   + ", tempo " + tempo.ToString("+0.0;-0.0;0", CultureInfo.InvariantCulture) + "%"
                   + ", at " + Seconds(position) + " of " + Seconds(length);
        }

        private static string MixerState(int channel)
        {
            switch (BassMix.BASS_Mixer_ChannelIsActive(channel))
            {
                case BASSActive.BASS_ACTIVE_PLAYING:
                    return "PLAYING";
                case BASSActive.BASS_ACTIVE_PAUSED:
                    return "paused";
                case BASSActive.BASS_ACTIVE_STALLED:
                    return "STALLED";
                default:
                    return "stopped";
            }
        }

        private static bool IsPlayingInMixer(AudioStream stream) => AudioStreamHelper.IsPlayingInMixer(stream);

        // ---------- mixers and plug-ins ----------

        private void AppendMixers(StringBuilder report, List<string> notes)
        {
            report.AppendLine("Mixer volumes:");
            report.AppendLine("  Track mixer: " + Volume(_desk.TrackMixer));
            report.AppendLine("  Track FX send: " + Volume(_desk.TrackSendMixer) + " (0 = closed)");
            report.AppendLine("  Track FX return: " + Volume(_desk.TrackSendFxMixer));
            report.AppendLine("  Sampler: " + Volume(_desk.SamplerMixer));
            report.AppendLine("  Raw loop: " + Volume(_desk.RawLoopMixer));
            report.AppendLine("  Speaker output: " + Volume(_desk.SpeakerOutput)
                              + ", " + Bass.BASS_ChannelIsActive(_desk.SpeakerOutput.ChannelId)
                              + (_desk.Mono ? ", mono" : ""));
            report.AppendLine();

            if (_desk.TrackSendMixer.GetVolume() != 0M)
                notes.Add("The track FX send is open: the tracks are also going through the send FX (delay).");
        }

        private static string Volume(Channel channel)
        {
            return channel.GetVolume().ToString("0", CultureInfo.InvariantCulture);
        }

        private void AppendPlugins(StringBuilder report)
        {
            report.AppendLine("Plug-ins:");
            foreach (var slot in VstPluginSlot.All)
            {
                var plugin = _desk.GetVstPlugin(slot.Channel, slot.Index);
                if (plugin != null)
                    report.AppendLine("  VST " + slot + ": " + plugin.Name);
            }

            var waPlugin = _desk.WaPlugin;
            if (waPlugin != null)
                report.AppendLine("  WinAmp DSP: " + waPlugin.Name);
            report.AppendLine();
        }

        // ---------- process ----------

        private static void AppendProcess(StringBuilder report)
        {
            using (var process = Process.GetCurrentProcess())
            {
                report.AppendLine("Process:");
                report.AppendLine("  managed memory " + (GC.GetTotalMemory(false) / (1024 * 1024)) + " MB, "
                                  + "private memory " + (process.PrivateMemorySize64 / (1024 * 1024)) + " MB");
                report.AppendLine("  GDI objects " + GetGuiResources(process.Handle, GdiObjects)
                                  + ", handles " + process.HandleCount + ", threads " + process.Threads.Count);
            }
        }

        private const int GdiObjects = 0;

        [DllImport("user32.dll")]
        private static extern int GetGuiResources(IntPtr process, int flags);

        // ---------- formatting ----------

        private static string Ms(double seconds)
        {
            return (seconds * 1000).ToString("0", CultureInfo.InvariantCulture) + " ms";
        }

        private static string Seconds(double seconds)
        {
            return seconds.ToString("0.0", CultureInfo.InvariantCulture) + " s";
        }

        private static string Percent(double value)
        {
            return value.ToString("0", CultureInfo.InvariantCulture) + "%";
        }
    }
}
