using System;
using System.IO;
using Halloumi.BassEngine.Channels;
using Halloumi.Common.Helpers;
using Un4seen.Bass;

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

        private MixerChannel _rawLoopMixer = null;
        private OutputSplitter _rawLoopOutputSplitter = null;

        /// <summary>
        /// Initialises the raw loop mixer.
        /// </summary>
        private void InitialiseRawLoopMixer()
        {
            DebugHelper.WriteLine("InitialiseRawLoopMixer");

            _rawLoopMixer = new MixerChannel(this, MixerChannelOutputType.MultipleOutputs);
            _rawLoopOutputSplitter = new OutputSplitter(_rawLoopMixer, _speakerOutput, _monitorOutput);

            DebugHelper.WriteLine("END InitialiseRawLoopMixer");
        }

        /// <summary>
        /// Loads a track for playing as the raw loop track.
        /// </summary>
        /// <param name="track">The track to queue.</param>
        public Track LoadRawLoopTrack(string filename)
        {
            if (!File.Exists(filename)) throw new Exception("Cannot find file " + filename);

            if (RawLoopTrack != null) UnloadRawLoopTrack();

            var track = new Track();
            track.Id = _nextTrackId++;
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

            BassHelper.AddTrackToMixer(track, _rawLoopMixer.InternalChannel);
            //BassHelper.SetTrackVolume(track, 100M);
            RawLoopTrack = track;

            SetRawLoopPositions(0, track.Length, 0);

            return RawLoopTrack;
        }

        /// <summary>
        /// Unloads the raw loop track.
        /// </summary>
        public void UnloadRawLoopTrack()
        {
            if (RawLoopTrack == null) return;
            ClearTrackSyncPositions(RawLoopTrack);
            UnloadTrack(RawLoopTrack);
            RawLoopTrack = null;
        }

        /// <summary>
        /// Sets the raw loop positions.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="length">The length.</param>
        public void SetRawLoopPositions(long start, long end, long offset)
        {
            if (RawLoopTrack == null) return;
            if (start < 0 || end > RawLoopTrack.Length) return;
            if (end <= 0 || end <= start) return;

            var maxEnd = RawLoopTrack.Length - 500;
            if (end > maxEnd) end = maxEnd;

            if (offset < start || offset > end)
                offset = start;

            var track = RawLoopTrack;

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
            if (RawLoopTrack == null) return;

            DebugHelper.WriteLine("Looping in raw-loop mode");

            BassHelper.TrackPause(RawLoopTrack);
            BassHelper.SetTrackPosition(RawLoopTrack, RawLoopTrack.RawLoopStart);
            BassHelper.TrackPlay(RawLoopTrack);
        }

        /// <summary>
        /// Starts playing the raw loop track at the offset of the raw-loop section.
        /// </summary>
        public void PlayRawLoop()
        {
            if (RawLoopTrack == null) return;

            DebugHelper.WriteLine("Playing in raw-loop mode");

            BassHelper.TrackPause(RawLoopTrack);
            BassHelper.SetTrackPosition(RawLoopTrack, RawLoopTrack.RawLoopOffset);
            BassHelper.TrackPlay(RawLoopTrack);
        }

        /// <summary>
        /// Plays the raw loop track at the start of the raw-loop section.
        /// </summary>
        public void StopRawLoop()
        {
            if (RawLoopTrack == null) return;
            DebugHelper.WriteLine("Pausing raw-loop");
            BassHelper.TrackPause(RawLoopTrack);
        }

        /// <summary>
        /// Gets or sets the raw loop output.
        /// </summary>
        public SoundOutput RawLoopOutput
        {
            get { return _rawLoopOutputSplitter.SoundOutput; }
            set { _rawLoopOutputSplitter.SoundOutput = value; }
        }
    }
}