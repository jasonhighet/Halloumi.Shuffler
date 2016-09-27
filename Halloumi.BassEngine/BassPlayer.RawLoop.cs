using System;
using System.IO;
using Halloumi.Shuffler.AudioEngine.Channels;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;
using Halloumi.Common.Helpers;

namespace Halloumi.Shuffler.AudioEngine
{
    public partial class BassPlayer
    {
        private MixerChannel _rawLoopMixer;
        private OutputSplitter _rawLoopOutputSplitter;

        /// <summary>
        ///     Gets the raw loop track.
        /// </summary>
        public Track RawLoopTrack { get; private set; }

        /// <summary>
        ///     Gets or sets the raw loop output.
        /// </summary>
        public SoundOutput RawLoopOutput
        {
            get { return _rawLoopOutputSplitter.SoundOutput; }
            set { _rawLoopOutputSplitter.SoundOutput = value; }
        }

        /// <summary>
        ///     Initialises the raw loop mixer.
        /// </summary>
        private void InitialiseRawLoopMixer()
        {
            DebugHelper.WriteLine("InitialiseRawLoopMixer");

            _rawLoopMixer = new MixerChannel(this, MixerChannelOutputType.MultipleOutputs);
            _rawLoopOutputSplitter = new OutputSplitter(_rawLoopMixer, _speakerOutput, _monitorOutput);

            DebugHelper.WriteLine("END InitialiseRawLoopMixer");
        }

        /// <summary>
        ///     Loads a track for playing as the raw loop track.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Cannot find file  + filename</exception>
        public Track LoadRawLoopTrack(string filename)
        {
            if (!File.Exists(filename)) throw new Exception("Cannot find file " + filename);

            if (RawLoopTrack != null) UnloadRawLoopTrack();

            var track = new Track
            {
                Id = _nextTrackId++,
                Filename = filename
            };

            SetArtistAndTitle(track, "", "");
            LoadTagData(track);
            ExtenedAttributesHelper.LoadExtendedAttributes(track);
            LoadTrackAudioData(track);

            DebugHelper.WriteLine("Loaded raw loop track " + track.Description);

            // set track sync event
            track.SyncProc = OnTrackSync;
            track.CurrentStartLoop = 0;
            track.CurrentEndLoop = 0;
            track.RawLoopStart = 0;
            track.RawLoopEnd = track.Length;

            DebugHelper.WriteLine("Loading raw loop track " + track.Description);

            AudioStreamHelper.AddToMixer(track, _rawLoopMixer.ChannelId);
            RawLoopTrack = track;

            SetRawLoopPositions(0, track.Length, 0);

            return RawLoopTrack;
        }

        /// <summary>
        ///     Unloads the raw loop track.
        /// </summary>
        public void UnloadRawLoopTrack()
        {
            if (RawLoopTrack == null) return;
            ClearTrackSyncPositions(RawLoopTrack);
            UnloadTrack(RawLoopTrack);
            RawLoopTrack = null;
        }

        /// <summary>
        ///     Sets the raw loop positions.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="offset">The offset.</param>
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

            AudioStreamHelper.SetPosition(track, track.RawLoopOffset);

            ClearTrackSyncPositions(track);

            // set track syncs
            SetTrackSync(track, track.Length, SyncType.TrackEnd);
            SetTrackSync(track, track.RawLoopEnd, SyncType.EndRawLoop);
        }

        /// <summary>
        ///     Starts playing the raw loop track at the offset of the raw-loop section.
        /// </summary>
        public void PlayRawLoop()
        {
            if (RawLoopTrack == null) return;

            DebugHelper.WriteLine("Playing in raw-loop mode");

            AudioStreamHelper.Pause(RawLoopTrack);
            AudioStreamHelper.SetPosition(RawLoopTrack, RawLoopTrack.RawLoopOffset);
            AudioStreamHelper.Play(RawLoopTrack);
        }

        /// <summary>
        ///     Plays the raw loop track at the start of the raw-loop section.
        /// </summary>
        public void StopRawLoop()
        {
            if (RawLoopTrack == null) return;
            DebugHelper.WriteLine("Pausing raw-loop");
            AudioStreamHelper.Pause(RawLoopTrack);
        }
    }
}