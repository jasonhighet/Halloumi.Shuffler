using System;
using System.Collections.Generic;
using System.IO;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;

namespace Halloumi.Shuffler.AudioEngine.BassPlayer
{
    /// <summary>
    ///     Loops a section of a track on its own mixer, separate from the track slots. Used for
    ///     auditioning loops and samples.
    /// </summary>
    internal class RawLooper
    {
        private readonly MixerGraph _graph;
        private readonly TrackLoader _loader;
        private readonly SyncDispatcher _syncs;

        public RawLooper(MixerGraph graph, TrackLoader loader, SyncDispatcher syncs)
        {
            _graph = graph;
            _loader = loader;
            _syncs = syncs;

            syncs.On(TrackSyncType.EndRawLoop, track => Play());
        }

        /// <summary>
        ///     The track being looped, or null.
        /// </summary>
        public Track Track { get; private set; }

        /// <summary>
        ///     Loads a track to loop, replacing the one loaded. The whole track is the loop until
        ///     SetPositions is called.
        /// </summary>
        public Track Load(string filename)
        {
            if (!File.Exists(filename)) throw new Exception("Cannot find file " + filename);

            if (Track != null) Unload();

            var track = _loader.CreateUncachedTrack(filename);
            _loader.LoadTagData(track);
            ExtendedAttributesHelper.LoadExtendedAttributes(track);
            _loader.LoadTrackAudioData(track);

            track.SyncProc = _syncs.Callback;
            track.CurrentStartLoop = 0;
            track.CurrentEndLoop = 0;
            track.RawLoopStart = 0;
            track.RawLoopEnd = track.Length;

            AudioStreamHelper.AddToMixer(track, _graph.RawLoopMixer);
            Track = track;

            SetPositions(0, track.Length, 0);

            return Track;
        }

        public void Unload()
        {
            if (Track == null) return;
            _loader.Unload(Track);
            Track = null;
        }

        /// <summary>
        ///     Sets the loop section and jumps to the offset.
        /// </summary>
        /// <param name="start">The loop start (samples).</param>
        /// <param name="end">The loop end (samples); capped just short of the end of the track.</param>
        /// <param name="offset">Where to start playing the first time round; the start if outside the loop.</param>
        public void SetPositions(long start, long end, long offset)
        {
            var track = Track;
            if (track == null) return;
            if (start < 0 || end > track.Length) return;
            if (end <= 0 || end <= start) return;

            var maxEnd = track.Length - 500;
            if (end > maxEnd) end = maxEnd;

            if (offset < start || offset > end)
                offset = start;

            track.RawLoopStart = start;
            track.RawLoopEnd = end;
            track.RawLoopOffset = offset;

            AudioStreamHelper.SetPosition(track, track.RawLoopOffset);

            SetSyncs(track);
        }

        /// <summary>
        ///     Moves the loop section without restarting it, so that a loop can be adjusted while it plays.
        ///     Playback carries on from where it is (or from the new start, if it is now outside the section),
        ///     and the loop wraps back to the new start.
        /// </summary>
        public void UpdatePositions(long start, long end)
        {
            var track = Track;
            if (track == null) return;
            if (start < 0 || end > track.Length) return;
            if (end <= 0 || end <= start) return;

            var maxEnd = track.Length - 500;
            if (end > maxEnd) end = maxEnd;
            if (end <= start) return;

            track.RawLoopStart = start;
            track.RawLoopEnd = end;
            track.RawLoopOffset = start;

            var position = AudioStreamHelper.GetPosition(track);
            if (position < start || position >= end)
                AudioStreamHelper.SetPosition(track, start);

            SetSyncs(track);
        }

        /// <summary>
        ///     Plays from the loop offset. Also called each time the loop end is reached.
        /// </summary>
        public void Play()
        {
            var track = Track;
            if (track == null) return;

            AudioStreamHelper.Pause(track);
            AudioStreamHelper.SetPosition(track, track.RawLoopOffset);
            AudioStreamHelper.Play(track);
        }

        public void Stop()
        {
            if (Track == null) return;
            AudioStreamHelper.Pause(Track);
        }

        private static void SetSyncs(Track track)
        {
            track.Syncs.ReplaceAll(new[]
            {
                new KeyValuePair<TrackSyncType, long>(TrackSyncType.TrackEnd, track.Length),
                new KeyValuePair<TrackSyncType, long>(TrackSyncType.EndRawLoop, track.RawLoopEnd)
            });
        }
    }
}
