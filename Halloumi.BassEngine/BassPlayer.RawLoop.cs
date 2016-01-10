using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Halloumi.BassEngine.Channels;
using Halloumi.Common.Helpers;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;
using Un4seen.Bass.AddOn.Vst;
using Un4seen.Bass.AddOn.WaDsp;

namespace Halloumi.BassEngine
{
    public partial class BassPlayer
    {
        /// <summary>
        /// Gets the raw loop track.
        /// </summary>
        public Track RawLoopTrack
        {
            get;
            private set;
        }

        private MixerChannel RawLoopMixer = null;
        private OutputSplitter RawLoopOutputSplitter = null;

        /// <summary>
        /// Initialises the raw loop mixer.
        /// </summary>
        private void InitialiseRawLoopMixer()
        {
            DebugHelper.WriteLine("InitialiseRawLoopMixer");

            RawLoopMixer = new MixerChannel(this, MixerChannelOutputType.MultipleOutputs);
            RawLoopOutputSplitter = new OutputSplitter(RawLoopMixer, this.SpeakerOutput, this.MonitorOutput);

            DebugHelper.WriteLine("END InitialiseRawLoopMixer");
        }

        /// <summary>
        /// Loads a track for playing as the raw loop track.
        /// </summary>
        /// <param name="track">The track to queue.</param>
        public Track LoadRawLoopTrack(string filename)
        {
            if (!File.Exists(filename)) throw new Exception("Cannot find file " + filename);

            if (this.RawLoopTrack != null) UnloadRawLoopTrack();

            Track track = new Track();
            track.ID = _nextTrackID++;
            track.Filename = filename;

            SetArtistAndTitle(track, "", "");
            LoadTagData(track);
            LoadExtendedAttributes(track);
            LoadTrackAudioData(track, AudioDataMode.LoadIntoMemory);

            DebugHelper.WriteLine("Loaded raw loop track " + track.Description);

            // set track sync event
            track.TrackSync = new SYNCPROC(OnTrackSync);
            track.CurrentStartLoop = 0;
            track.CurrentEndLoop = 0;
            track.RawLoopStart = 0;
            track.RawLoopEnd = track.Length;

            DebugHelper.WriteLine("Loading raw loop track " + track.Description);

            BassHelper.AddTrackToMixer(track, RawLoopMixer.InternalChannel);
            //BassHelper.SetTrackVolume(track, 100M);
            this.RawLoopTrack = track;

            SetRawLoopPositions(0, track.Length, 0);

            return this.RawLoopTrack;
        }

        /// <summary>
        /// Unloads the raw loop track.
        /// </summary>
        public void UnloadRawLoopTrack()
        {
            if (this.RawLoopTrack == null) return;
            ClearTrackSyncPositions(this.RawLoopTrack);
            UnloadTrack(this.RawLoopTrack);
            this.RawLoopTrack = null;
        }

        /// <summary>
        /// Sets the raw loop positions.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="length">The length.</param>
        public void SetRawLoopPositions(long start, long end, long offset)
        {
            if (this.RawLoopTrack == null) return;
            if (start < 0 || end > this.RawLoopTrack.Length) return;
            if (end <= 0 || end <= start) return;

            var maxEnd = this.RawLoopTrack.Length - 500;
            if (end > maxEnd) end = maxEnd;

            if (offset < start || offset > end)
                offset = start;

            var track = this.RawLoopTrack;

            track.RawLoopStart = start;
            track.RawLoopEnd = end;
            track.RawLoopOffset = offset;

            BassHelper.SetTrackPosition(track, track.RawLoopOffset);

            ClearTrackSyncPositions(track);

            // set track syncs
            SetTrackSync(track, track.Length, SyncType.TrackEnd);
            SetTrackSync(track, track.RawLoopEnd, SyncType.EndRawLoop);
        }

        /// <summary>
        /// Continues playing the raw loop track at the start of the raw-loop section.
        /// </summary>
        private void OnEndRawLoop()
        {
            if (this.RawLoopTrack == null) return;

            DebugHelper.WriteLine("Looping in raw-loop mode");

            BassHelper.TrackPause(this.RawLoopTrack);
            BassHelper.SetTrackPosition(this.RawLoopTrack, this.RawLoopTrack.RawLoopStart);
            BassHelper.TrackPlay(this.RawLoopTrack);
        }

        /// <summary>
        /// Starts playing the raw loop track at the offset of the raw-loop section.
        /// </summary>
        public void PlayRawLoop()
        {
            if (this.RawLoopTrack == null) return;

            DebugHelper.WriteLine("Playing in raw-loop mode");

            BassHelper.TrackPause(this.RawLoopTrack);
            BassHelper.SetTrackPosition(this.RawLoopTrack, this.RawLoopTrack.RawLoopOffset);
            BassHelper.TrackPlay(this.RawLoopTrack);
        }

        /// <summary>
        /// Plays the raw loop track at the start of the raw-loop section.
        /// </summary>
        public void StopRawLoop()
        {
            if (this.RawLoopTrack == null) return;
            DebugHelper.WriteLine("Pausing raw-loop");
            BassHelper.TrackPause(this.RawLoopTrack);
        }

        /// <summary>
        /// Gets or sets the raw loop output.
        /// </summary>
        public SoundOutput RawLoopOutput
        {
            get { return RawLoopOutputSplitter.SoundOutput; }
            set { RawLoopOutputSplitter.SoundOutput = value; }
        }
    }
}