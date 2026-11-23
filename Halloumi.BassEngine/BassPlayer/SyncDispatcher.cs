using System;
using System.Collections.Generic;
using Halloumi.Shuffler.AudioEngine.Models;
using Un4seen.Bass;

namespace Halloumi.Shuffler.AudioEngine.BassPlayer
{
    /// <summary>
    ///     Routes BASS sync callbacks to the component that handles each sync type. Components
    ///     register their handlers when they are created, so no single class has to know about
    ///     every feature.
    ///     Handlers run on a BASS mixer thread, in the middle of mixing: keep them short.
    /// </summary>
    internal class SyncDispatcher
    {
        private readonly Dictionary<TrackSyncType, Action<Track>> _handlers = new Dictionary<TrackSyncType, Action<Track>>();
        private readonly Func<int, Track> _findTrack;

        /// <param name="findTrack">Finds the track playing on a BASS channel (null if none).</param>
        public SyncDispatcher(Func<int, Track> findTrack)
        {
            _findTrack = findTrack;
            Callback = OnSync;
        }

        /// <summary>
        ///     The callback to give BASS (set it as a track's SyncProc). One delegate is kept for the
        ///     life of the player so it cannot be garbage collected while BASS holds it.
        /// </summary>
        public SYNCPROC Callback { get; }

        /// <summary>
        ///     Registers the handler for a sync type. The handler gets the in-use track the sync fired
        ///     on, or null (for example for the raw-loop track). Sync types with no handler are ignored.
        /// </summary>
        public void On(TrackSyncType syncType, Action<Track> handler)
        {
            if (_handlers.ContainsKey(syncType))
                throw new InvalidOperationException("A handler for " + syncType + " is already registered");
            _handlers[syncType] = handler;
        }

        private void OnSync(int handle, int channel, int data, IntPtr user)
        {
            Action<Track> handler;
            if (!_handlers.TryGetValue((TrackSyncType) user.ToInt32(), out handler)) return;
            handler(_findTrack(channel));
        }
    }
}
