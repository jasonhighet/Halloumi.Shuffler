using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;
using Halloumi.Shuffler.AudioEngine.Plugins;
using System;

namespace Halloumi.Shuffler.AudioEngine.BassPlayer
{
    public partial class BassPlayer
    {
        /// <summary>
        ///     Returns a collection of all loaded WinAmp plug-ins
        /// </summary>
        public WaPlugin WaPlugin
        {
            get { return SpeakerOutput.WaPlugin; }
        }

        /// <summary>
        ///     Returns a collection of all loaded VST plug-ins
        /// </summary>
        public VstPlugin MainVstPlugin
        {
            get { return SpeakerOutput.GetVstPlugin(0); }
        }

        /// <summary>
        ///     Returns a collection of all loaded VST plug-ins
        /// </summary>
        public VstPlugin MainVstPlugin2
        {
            get { return SpeakerOutput.GetVstPlugin(1); }
        }

        /// <summary>
        ///     Returns a collection of all loaded trackFX VST plug-ins
        /// </summary>
        public VstPlugin SamplerVstPlugin
        {
            get { return _samplerMixer.GetVstPlugin(0); }
        }

        /// <summary>
        ///     Returns a collection of all loaded trackFX VST plug-ins
        /// </summary>
        public VstPlugin SamplerVstPlugin2
        {
            get { return _samplerMixer.GetVstPlugin(1); }
        }

        /// <summary>
        ///     Returns a collection of all loaded mixer VST plug-ins
        /// </summary>
        public VstPlugin TrackVstPlugin
        {
            get { return _trackMixer.GetVstPlugin(0); }
        }

        /// <summary>
        ///     Returns a collection of all loaded mixer VST plug-ins
        /// </summary>
        public VstPlugin TrackSendFxVstPlugin
        {
            get { return _trackSendFxMixer.GetVstPlugin(0); }
        }

        /// <summary>
        ///     Returns a collection of all loaded mixer VST plug-ins
        /// </summary>
        public VstPlugin TrackSendFxVstPlugin2
        {
            get { return _trackSendFxMixer.GetVstPlugin(1); }
        }

        public decimal TrackSendFxDelayNotes
        {
            get { return _trackSendFxMixer.SyncNotes; }
            set { _trackSendFxMixer.SyncNotes = value; }
        }

        public decimal SamplerDelayNotes
        {
            get { return _samplerMixer.SyncNotes; }
            set { _samplerMixer.SyncNotes = value; }
        }

        public Channels.MixerChannel SamplerMixer
        {
            get { return _samplerMixer; }
        }

        /// <summary>
        ///     Gets the current BPM.
        /// </summary>
        /// <returns>The current BPM</returns>
        public decimal GetCurrentBpm()
        {
            var bpm = 100M;
            if (CurrentTrack != null)
            {
                var trackPosition = GetPositionNoLock();
                var position = trackPosition.Positition;
                if (position < 0) position = 0;

                if (position < CurrentTrack.FullStartLoopLength)
                {
                    bpm = CurrentTrack.StartBpm;
                }
                else
                {
                    var range = CurrentTrack.EndBpm - CurrentTrack.StartBpm;
                    var percentComplete = (decimal)(position / (double)trackPosition.Length);
                    bpm = CurrentTrack.StartBpm + range * percentComplete;
                }
            }

            bpm = BpmHelper.NormaliseBpm(bpm);
            return bpm;
        }

        /// <summary>
        ///     Loads a WinAmp DSP plug-in and applies it to the mixer
        /// </summary>
        /// <param name="location">The file location of the WinAmp DSP DLL</param>
        public WaPlugin LoadWaPlugin(string location)
        {
            var playing = PlayState == PlayState.Playing;
            if (playing) Pause();

            var plugin = SpeakerOutput.LoadWaPlugin(location);

            if (playing) Play();

            return plugin;
        }

        /// <summary>
        ///     Loads a VST plug-in and applies it to the mixer
        /// </summary>
        /// <param name="location">The file location of the VST DLL</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public VstPlugin LoadMainVstPlugin(string location, int index)
        {
            return SpeakerOutput.LoadVstPlugin(location, index);
        }


        /// <summary>
        ///     Loads a VST plug-in and applies it to the sample mixer
        /// </summary>
        /// <param name="location">The file location of the VST DLL</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public VstPlugin LoadSamplerVstPlugin(string location, int index)
        {
            return _samplerMixer.LoadVstPlugin(location, index);
        }

        /// <summary>
        ///     Loads a VST plug-in and applies it to the sample mixer
        /// </summary>
        /// <param name="location">The file location of the VST DLL</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public VstPlugin LoadTracksVstPlugin(string location, int index)
        {
            return _trackMixer.LoadVstPlugin(location, index);
        }

        /// <summary>
        ///     Loads a VST plug-in and applies it to the sample mixer
        /// </summary>
        /// <param name="location">The file location of the VST DLL</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public VstPlugin LoadTrackSendFxvstPlugin(string location, int index)
        {
            return _trackSendFxMixer.LoadVstPlugin(location, index);
        }

        /// <summary>
        ///     Mutes the track FX.
        /// </summary>
        public void StopTrackFxSend()
        {
            if (CurrentTrack == null || LastTrackFxTrigger == null || LastTrackFxTriggerTrack == null)
                return;

            // DebugHelper.WriteLine("Start StopTrackFXSend");

            _trackSendMixer.SetVolume(0);

            // DebugHelper.WriteLine("Calculate TrackFXSend Length");

            var position = AudioStreamHelper.GetPosition(LastTrackFxTriggerTrack);
            var positionSeconds = LastTrackFxTriggerTrack.SamplesToSeconds(position);
            var length = positionSeconds - LastTrackFxTrigger.Start;

            if (length <= 0 || position >= LastTrackFxTriggerTrack.FadeOutStart)
                length = LastTrackFxTriggerTrack.SamplesToSeconds(LastTrackFxTriggerTrack.FadeOutStart) -
                         LastTrackFxTrigger.Start;

            LastTrackFxTrigger.Length = length;

            // DebugHelper.WriteLine("End StopTrackFXSend");
        }

        public void StartTrackFxSendHalf()
        {
            TrackSendFxDelayNotes = 0.5M;
            StartTrackFxSend();
        }

        public void StartTrackFxSendQuarter()
        {
            TrackSendFxDelayNotes = 0.25M;
            StartTrackFxSend();
       }

        public void StartTrackFxSendEighth()
        {
            TrackSendFxDelayNotes = 0.125M;
            StartTrackFxSend();
        }

        public void StartTrackFxSendSixteenth()
        {
            TrackSendFxDelayNotes = 0.0625M;
            StartTrackFxSend();
        }

        /// <summary>
        ///     Silences the track FX.
        /// </summary>
        public void StartTrackFxSend()
        {
            _trackSendMixer.SetVolume(50M);
            if (CurrentTrack == null) return;

            LastTrackFxTriggerTrack = CurrentTrack;

            var position = AudioStreamHelper.GetPosition(LastTrackFxTriggerTrack);
            LastTrackFxTrigger = new TrackFXTrigger
            {
                Start = LastTrackFxTriggerTrack.SamplesToSeconds(position),
                DelayNotes = TrackSendFxDelayNotes
            };

            _trackSendMixer.SetPluginBpm();
        }

        /// <summary>
        ///     Determines whether audio is being sent to the track FX.
        /// </summary>
        /// <returns>True if audio is being sent to the track FX; otherwise, false.</returns>
        public bool IsTrackFxSending()
        {
            return _trackSendMixer.GetVolume() != 0M;
        }

        /// <summary>
        ///     Clears the main VST plug-in.
        /// </summary>
        public void ClearMainVstPlugin(int index)
        {
            SpeakerOutput.ClearVstPlugin(index);
        }


        /// <summary>
        ///     Clears the trackFX VST plug-in.
        /// </summary>
        public void ClearSamplerVstPlugin(int index)
        {
            _samplerMixer.ClearVstPlugin(index);
        }

        /// <summary>
        ///     Clears the tracks VST plug-in.
        /// </summary>
        public void ClearTracksVstPlugin(int index)
        {
            _trackMixer.ClearVstPlugin(index);
        }

        /// <summary>
        ///     Clears the tracks VST plug-in.
        /// </summary>
        public void ClearTrackSendFxvstPlugin(int index)
        {
            _trackSendFxMixer.ClearVstPlugin(index);
        }

        /// <summary>
        ///     Unloads all loaded VST plug-ins.
        /// </summary>
        public void UnloadAllVstPlugins()
        {
            ClearMainVstPlugin(0);
            ClearMainVstPlugin(1);
            ClearSamplerVstPlugin(0);
            ClearSamplerVstPlugin(1);
            ClearTracksVstPlugin(0);
            ClearTrackSendFxvstPlugin(0);
            ClearTrackSendFxvstPlugin(1);
        }

        /// <summary>
        ///     Unloads all loaded WinAmp plug-ins.
        /// </summary>
        public void UnloadAllWaPlugins()
        {
            SpeakerOutput.ClearWaPlugin();
        }


        /// <summary>
        ///     Sets the delay by BPM.
        /// </summary>
        private void SetDelayByBpm()
        {
            _trackSendFxMixer.SetPluginBpm();
            _samplerMixer.SetPluginBpm();
        }
    }
}