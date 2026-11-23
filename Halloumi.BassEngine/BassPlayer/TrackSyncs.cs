using System;
using System.Collections.Generic;
using Halloumi.Shuffler.AudioEngine.Models;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;

namespace Halloumi.Shuffler.AudioEngine.BassPlayer
{
    /// <summary>
    ///     The BASS position callbacks (syncs) set on one track. Every sync BassPlayer sets goes
    ///     through here, so they can all be removed together.
    ///     <para>
    ///         Setting a sync that is already set at the same position leaves it alone. Removing and
    ///         re-adding a sync while the mixer is working through that part of the track can make
    ///         BASS miss it, so only syncs that actually change are touched.
    ///     </para>
    /// </summary>
    internal class TrackSyncs
    {
        private readonly object _lock = new object();
        private readonly AudioStream _owner;
        private readonly List<Entry> _syncs = new List<Entry>();

        public TrackSyncs(AudioStream owner)
        {
            _owner = owner;
        }

        /// <summary>
        ///     True for sync types a track can have several of at once (automation triggers).
        ///     Setting any other type replaces the one already set.
        /// </summary>
        private static bool AllowsMany(TrackSyncType syncType)
        {
            return syncType == TrackSyncType.StartTrackFxTrigger
                   || syncType == TrackSyncType.EndTrackFxTrigger
                   || syncType == TrackSyncType.StartSampleTrigger
                   || syncType == TrackSyncType.EndSampleTrigger;
        }

        /// <summary>
        ///     Sets a sync at a sample position. TrackEnd ignores the position and fires at the end of the file.
        ///     For a type a track has only one of, this replaces the one set (unless it is already at this
        ///     position, when nothing changes); the new sync is set before the old one is removed.
        /// </summary>
        public void Set(TrackSyncType syncType, long position)
        {
            if (!_owner.IsAudioLoaded()) throw new Exception("Track audio not loaded");
            position = Normalise(syncType, position);

            lock (_lock)
            {
                if (AllowsMany(syncType))
                {
                    Add(syncType, position);
                    return;
                }

                var existing = _syncs.FindAll(s => s.Type == syncType);
                if (existing.Count == 1 && existing[0].Position == position) return;

                Add(syncType, position);
                foreach (var sync in existing)
                    Remove(sync);
            }
        }

        /// <summary>
        ///     Makes the track's syncs exactly these. Syncs already set at the same position are kept;
        ///     new ones are set before old ones are removed, so a sync about to be reached is never
        ///     missing.
        /// </summary>
        public void ReplaceAll(IEnumerable<KeyValuePair<TrackSyncType, long>> syncs)
        {
            if (!_owner.IsAudioLoaded()) throw new Exception("Track audio not loaded");

            lock (_lock)
            {
                var unmatched = new List<Entry>(_syncs);
                var toAdd = new List<KeyValuePair<TrackSyncType, long>>();

                foreach (var wanted in syncs)
                {
                    var position = Normalise(wanted.Key, wanted.Value);
                    var match = unmatched.Find(s => s.Type == wanted.Key && s.Position == position);
                    if (match != null)
                        unmatched.Remove(match);
                    else
                        toAdd.Add(new KeyValuePair<TrackSyncType, long>(wanted.Key, position));
                }

                foreach (var sync in toAdd)
                    Add(sync.Key, sync.Value);

                foreach (var sync in unmatched)
                    Remove(sync);
            }
        }

        /// <summary>
        ///     Removes all syncs. If the audio is already unloaded the handles died with the channel,
        ///     so they are only forgotten.
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                if (_owner.IsAudioLoaded())
                    foreach (var sync in _syncs)
                        BassMix.BASS_Mixer_ChannelRemoveSync(_owner.ChannelId, sync.Handle);

                _syncs.Clear();
            }
        }

        private static long Normalise(TrackSyncType syncType, long position)
        {
            return syncType == TrackSyncType.TrackEnd ? 0 : position;
        }

        private void Add(TrackSyncType syncType, long position)
        {
            var flags = syncType == TrackSyncType.TrackEnd
                ? BASSSync.BASS_SYNC_END
                : BASSSync.BASS_SYNC_POS | BASSSync.BASS_SYNC_MIXTIME;

            var handle = BassMix.BASS_Mixer_ChannelSetSync(_owner.ChannelId, flags, position, _owner.SyncProc,
                new IntPtr((int) syncType));

            _syncs.Add(new Entry { Type = syncType, Position = position, Handle = handle });
        }

        private void Remove(Entry sync)
        {
            BassMix.BASS_Mixer_ChannelRemoveSync(_owner.ChannelId, sync.Handle);
            _syncs.Remove(sync);
        }

        private class Entry
        {
            public TrackSyncType Type;
            public long Position;
            public int Handle;
        }
    }
}
