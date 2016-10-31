using System;
using Halloumi.Common.Helpers;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;
using Halloumi.Shuffler.AudioEngine.Plugins;

namespace Halloumi.Shuffler.AudioEngine
{
    public partial class BassPlayer
    {
        /// <summary>
        ///     Returns a collection of all loaded WinAmp plug-ins
        /// </summary>
        public WaPlugin WaPlugin => _speakerOutput.WaPlugin;

        /// <summary>
        ///     Returns a collection of all loaded VST plug-ins
        /// </summary>
        public VstPlugin MainVstPlugin => _speakerOutput.VstPlugin1;

        /// <summary>
        ///     Returns a collection of all loaded VST plug-ins
        /// </summary>
        public VstPlugin MainVstPlugin2 => _speakerOutput.VstPlugin2;

        /// <summary>
        ///     Returns a collection of all loaded trackFX VST plug-ins
        /// </summary>
        public VstPlugin SamplerVstPlugin => _samplerMixer.VstPlugin1;

        /// <summary>
        ///     Returns a collection of all loaded trackFX VST plug-ins
        /// </summary>
        public VstPlugin SamplerVstPlugin2 => _samplerMixer.VstPlugin2;

        /// <summary>
        ///     Returns a collection of all loaded mixer VST plug-ins
        /// </summary>
        public VstPlugin TrackVstPlugin => _trackMixer.VstPlugin1;

        /// <summary>
        ///     Returns a collection of all loaded mixer VST plug-ins
        /// </summary>
        public VstPlugin TrackSendFxvstPlugin => _trackSendFxMixer.VstPlugin1;

        /// <summary>
        ///     Returns a collection of all loaded mixer VST plug-ins
        /// </summary>
        public VstPlugin TrackSendFxvstPlugin2 => _trackSendFxMixer.VstPlugin2;

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
                    var percentComplete = (decimal) (position/(double) trackPosition.Length);
                    bpm = CurrentTrack.StartBpm + (range*percentComplete);
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
            var playing = (PlayState == PlayState.Playing);
            if (playing) Pause();

            var plugin = _speakerOutput.LoadWaPlugin(location);

            if (playing) Play();

            return plugin;
        }

        /// <summary>
        ///     Loads a VST plug-in and applies it to the mixer
        /// </summary>
        /// <param name="location">The file location of the VST DLL</param>
        public VstPlugin LoadMainVstPlugin(string location)
        {
            return _speakerOutput.LoadVstPlugin(location, 0);
        }

        /// <summary>
        ///     Loads a VST plug-in and applies it to the mixer
        /// </summary>
        /// <param name="location">The file location of the VST DLL</param>
        public VstPlugin LoadMainVstPlugin2(string location)
        {
            return _speakerOutput.LoadVstPlugin(location, 1);
        }

        /// <summary>
        ///     Loads a VST plug-in and applies it to the sample mixer
        /// </summary>
        /// <param name="location">The file location of the VST DLL</param>
        public VstPlugin LoadSamplerVstPlugin(string location)
        {
            return _samplerMixer.LoadVstPlugin(location, 0);
        }

        /// <summary>
        ///     Loads a VST plug-in and applies it to the sample mixer
        /// </summary>
        /// <param name="location">The file location of the VST DLL</param>
        public VstPlugin LoadSamplerVstPlugin2(string location)
        {
            return _samplerMixer.LoadVstPlugin(location, 0);
        }

        /// <summary>
        ///     Loads a VST plug-in and applies it to the sample mixer
        /// </summary>
        /// <param name="location">The file location of the VST DLL</param>
        public VstPlugin LoadTracksVstPlugin(string location)
        {
            return _trackMixer.LoadVstPlugin(location, 0);
        }

        /// <summary>
        ///     Loads a VST plug-in and applies it to the sample mixer
        /// </summary>
        /// <param name="location">The file location of the VST DLL</param>
        public VstPlugin LoadTrackSendFxvstPlugin(string location)
        {
            return _trackSendFxMixer.LoadVstPlugin(location, 0);
        }

        /// <summary>
        ///     Loads a VST plug-in and applies it to the sample mixer
        /// </summary>
        /// <param name="location">The file location of the VST DLL</param>
        public VstPlugin LoadTrackSendFxvstPlugin2(string location)
        {
            return _trackSendFxMixer.LoadVstPlugin(location, 1);
        }

        /// <summary>
        ///     Mutes the track FX.
        /// </summary>
        public void StopTrackFxSend()
        {
            if (CurrentTrack == null || LastTrackFxTrigger == null || LastTrackFxTriggerTrack == null)
                return;

            DebugHelper.WriteLine("Start StopTrackFXSend");

            _trackSendMixer.SetVolume(0);

            DebugHelper.WriteLine("Calculate TrackFXSend Length");

            var position = AudioStreamHelper.GetPosition(LastTrackFxTriggerTrack);
            var positionSeconds = LastTrackFxTriggerTrack.SamplesToSeconds(position);
            var length = positionSeconds - LastTrackFxTrigger.Start;

            if (length <= 0 || position >= LastTrackFxTriggerTrack.FadeOutStart)
            {
                length = LastTrackFxTriggerTrack.SamplesToSeconds(LastTrackFxTriggerTrack.FadeOutStart) -
                         LastTrackFxTrigger.Start;
            }

            LastTrackFxTrigger.Length = length;

            DebugHelper.WriteLine("End StopTrackFXSend");
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
            return (_trackSendMixer.GetVolume() != 0M);
        }

        /// <summary>
        ///     Gets the sample mixer volume.
        /// </summary>
        /// <returns>A value between 0 and 100</returns>
        public decimal GetTrackSendFxVolume()
        {
            return _trackSendFxMixer.GetVolume();
        }

        /// <summary>
        ///     Sets the sample mixer volume.
        /// </summary>
        /// <param name="volume">The volume as a value between 0 and 100.</param>
        public void SetTrackSendFxVolume(decimal volume)
        {
            _trackSendFxMixer.SetVolume(volume);
            OnTrackFxVolumeChanged?.Invoke(CurrentTrack, EventArgs.Empty);
        }

        /// <summary>
        ///     Clears the main VST plug-in.
        /// </summary>
        public void ClearMainVstPlugin()
        {
            _speakerOutput.ClearVstPlugin(0);
        }

        /// <summary>
        ///     Clears the main VST plug-in.
        /// </summary>
        public void ClearMainVstPlugin2()
        {
            _speakerOutput.ClearVstPlugin(1);
        }

        /// <summary>
        ///     Clears the trackFX VST plug-in.
        /// </summary>
        public void ClearSamplerVstPlugin()
        {
            _samplerMixer.ClearVstPlugin(0);
        }

        /// <summary>
        ///     Clears the sampler VST plug-in 2.
        /// </summary>
        public void ClearSamplerVstPlugin2()
        {
            _samplerMixer.ClearVstPlugin(1);
        }

        /// <summary>
        ///     Clears the tracks VST plug-in.
        /// </summary>
        public void ClearTracksVstPlugin()
        {
            _trackMixer.ClearVstPlugin(0);
        }

        /// <summary>
        ///     Clears the tracks VST plug-in.
        /// </summary>
        public void ClearTrackSendFxvstPlugin()
        {
            _trackSendFxMixer.ClearVstPlugin(0);
        }

        /// <summary>
        ///     Clears the trackFX VST plug-in 2.
        /// </summary>
        public void ClearTrackSendFxvstPlugin2()
        {
            _trackSendFxMixer.ClearVstPlugin(1);
        }

        /// <summary>
        ///     Unloads all loaded VST plug-ins.
        /// </summary>
        public void UnloadAllVstPlugins()
        {
            ClearMainVstPlugin();
            ClearMainVstPlugin2();
            ClearSamplerVstPlugin();
            ClearSamplerVstPlugin2();
            ClearTracksVstPlugin();
            ClearTrackSendFxvstPlugin();
            ClearTrackSendFxvstPlugin2();
        }

        /// <summary>
        ///     Unloads all loaded WinAmp plug-ins.
        /// </summary>
        public void UnloadAllWaPlugins()
        {
            _speakerOutput.ClearWaPlugin();
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