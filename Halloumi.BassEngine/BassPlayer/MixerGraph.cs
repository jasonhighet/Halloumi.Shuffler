using System;
using Halloumi.Shuffler.AudioEngine.Channels;
using Halloumi.Shuffler.AudioEngine.Models;
using Halloumi.Shuffler.AudioEngine.Plugins;

namespace Halloumi.Shuffler.AudioEngine.BassPlayer
{
    /// <summary>
    ///     Builds and owns every mixer channel, and the plug-in slots on them.
    ///     <code>
    ///     CurrentTrack  ──► CurrentTrackMixer  ──┐
    ///     PreviousTrack ──► PreviousTrackMixer ──┴► TrackMixer ──► splitter ──► speaker / monitor
    ///                                                   └► TrackSendMixer (muted until a send) ──► TrackSendFxMixer ──► speaker
    ///     Samples       ──► SamplerMixer ──► splitter ──► speaker / monitor
    ///     Raw loop      ──► RawLoopMixer ──► splitter ──► speaker / monitor
    ///     </code>
    /// </summary>
    internal class MixerGraph
    {
        private readonly OutputSplitter _trackOutputSplitter;
        private readonly OutputSplitter _samplerOutputSplitter;
        private readonly OutputSplitter _rawLoopOutputSplitter;

        public MixerGraph(IBpmProvider bpmProvider)
        {
            SpeakerOutput = new SpeakerOutputChannel();
            MonitorOutput = new MonitorOutputChannel();

            // tracks
            CurrentTrackMixer = new MixerChannel(bpmProvider);
            PreviousTrackMixer = new MixerChannel(bpmProvider);
            TrackMixer = new MixerChannel(bpmProvider, MixerChannelOutputType.MultipleOutputs);
            _trackOutputSplitter = new OutputSplitter(TrackMixer, SpeakerOutput, MonitorOutput);
            TrackMixer.AddInputChannel(CurrentTrackMixer);
            TrackMixer.AddInputChannel(PreviousTrackMixer);

            // track FX send: a muted copy of the track mix, through the send FX, into the speakers
            TrackSendMixer = new MixerChannel(bpmProvider);
            TrackSendMixer.AddInputChannel(TrackMixer);
            TrackSendMixer.SetVolume(0);

            TrackSendFxMixer = new MixerChannel(bpmProvider);
            TrackSendFxMixer.AddInputChannel(TrackSendMixer);
            TrackSendFxMixer.CutBass();
            TrackSendFxMixer.SetVolume(80);
            SpeakerOutput.AddInputChannel(TrackSendFxMixer);

            // sampler (the sample player's output is added by Sampler)
            SamplerMixer = new MixerChannel(bpmProvider);
            SamplerMixer.CutBass();
            _samplerOutputSplitter = new OutputSplitter(SamplerMixer, SpeakerOutput, MonitorOutput);
            SamplerMixer.SetVolume(50);

            // raw loop
            RawLoopMixer = new MixerChannel(bpmProvider, MixerChannelOutputType.MultipleOutputs);
            _rawLoopOutputSplitter = new OutputSplitter(RawLoopMixer, SpeakerOutput, MonitorOutput);
        }

        public SpeakerOutputChannel SpeakerOutput { get; }

        public MonitorOutputChannel MonitorOutput { get; }

        public MixerChannel CurrentTrackMixer { get; }

        public MixerChannel PreviousTrackMixer { get; }

        public MixerChannel TrackMixer { get; }

        public MixerChannel TrackSendMixer { get; }

        public MixerChannel TrackSendFxMixer { get; }

        public MixerChannel SamplerMixer { get; }

        public MixerChannel RawLoopMixer { get; }

        // ---------- output routing ----------

        public SoundOutput TrackOutput
        {
            get { return _trackOutputSplitter.SoundOutput; }
            set { _trackOutputSplitter.SoundOutput = value; }
        }

        public SoundOutput SamplerOutput
        {
            get { return _samplerOutputSplitter.SoundOutput; }
            set { _samplerOutputSplitter.SoundOutput = value; }
        }

        public SoundOutput RawLoopOutput
        {
            get { return _rawLoopOutputSplitter.SoundOutput; }
            set { _rawLoopOutputSplitter.SoundOutput = value; }
        }

        // ---------- levels ----------

        public VolumeLevels GetVolumeLevels()
        {
            return SpeakerOutput.GetVolumeLevels();
        }

        public bool Mono
        {
            get { return SpeakerOutput.Mono; }
            set
            {
                SpeakerOutput.Mono = value;
                MonitorOutput.Mono = value;
            }
        }

        /// <summary>
        ///     Re-times the BPM-synced plug-ins (delays) on the track FX send and the sampler.
        /// </summary>
        public void SetDelayByBpm()
        {
            TrackSendFxMixer.SetPluginBpm();
            SamplerMixer.SetPluginBpm();
        }

        // ---------- plug-ins ----------

        public VstPlugin GetVstPlugin(PluginChannel channel, int index)
        {
            return GetPluginHost(channel).GetVstPlugin(index);
        }

        public VstPlugin LoadVstPlugin(PluginChannel channel, string location, int index)
        {
            return GetPluginHost(channel).LoadVstPlugin(location, index);
        }

        public void ClearVstPlugin(PluginChannel channel, int index)
        {
            GetPluginHost(channel).ClearVstPlugin(index);
        }

        public void UnloadAllVstPlugins()
        {
            foreach (var slot in VstPluginSlot.All)
                ClearVstPlugin(slot.Channel, slot.Index);
        }

        public WaPlugin WaPlugin
        {
            get { return SpeakerOutput.WaPlugin; }
        }

        public WaPlugin LoadWaPlugin(string location)
        {
            return SpeakerOutput.LoadWaPlugin(location);
        }

        public void UnloadAllWaPlugins()
        {
            SpeakerOutput.ClearWaPlugin();
        }

        private Channel GetPluginHost(PluginChannel channel)
        {
            switch (channel)
            {
                case PluginChannel.Main:
                    return SpeakerOutput;
                case PluginChannel.Sampler:
                    return SamplerMixer;
                case PluginChannel.Tracks:
                    return TrackMixer;
                case PluginChannel.TrackSendFx:
                    return TrackSendFxMixer;
                default:
                    throw new ArgumentOutOfRangeException(nameof(channel), channel, null);
            }
        }
    }
}
