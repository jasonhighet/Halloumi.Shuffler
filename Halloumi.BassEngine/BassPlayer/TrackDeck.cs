using System.Collections.Generic;
using System.Linq;
using Halloumi.Shuffler.AudioEngine.Helpers;
using Halloumi.Shuffler.AudioEngine.Models;

namespace Halloumi.Shuffler.AudioEngine.BassPlayer
{
    /// <summary>
    ///     The four track slots BassPlayer plays from, and the only place they change.
    ///     Previous: fading out (or finished). Current: playing. Next: queued to take over at the
    ///     current track's fade-out. Preloaded: audio loaded ahead of time so queueing it is quick.
    ///     Slot changes are made under a lock, so a sync callback never sees a half-made change.
    /// </summary>
    internal class TrackDeck
    {
        private readonly object _lock = new object();

        public Track Previous { get; private set; }

        public Track Current { get; private set; }

        public Track Next { get; private set; }

        public Track Preloaded { get; private set; }

        /// <summary>
        ///     The current track, or the previous one if the current track has finished and nothing was queued.
        /// </summary>
        public Track Active
        {
            get { return Current ?? Previous; }
        }

        public void SetPreloaded(Track track)
        {
            lock (_lock)
            {
                Preloaded = track;
            }
        }

        /// <summary>
        ///     Queues a track: it becomes the current track if there is none, otherwise the next track.
        /// </summary>
        /// <param name="track">The track to queue.</param>
        /// <param name="replacedNext">The next track it replaced, if any.</param>
        /// <returns>True if the track became the current track.</returns>
        public bool Queue(Track track, out Track replacedNext)
        {
            lock (_lock)
            {
                replacedNext = null;
                if (Current == null)
                {
                    Current = track;
                    return true;
                }

                replacedNext = Next;
                Next = track;
                return false;
            }
        }

        /// <summary>
        ///     Clears the previous, next and current tracks (in that order).
        /// </summary>
        /// <returns>The tracks that were cleared, in that order; null entries for empty slots.</returns>
        public Track[] ClearPlaying()
        {
            lock (_lock)
            {
                var cleared = new[] { Previous, Next, Current };
                Previous = null;
                Next = null;
                Current = null;
                return cleared;
            }
        }

        /// <summary>
        ///     Moves every track along one slot at the start of a transition: current becomes previous,
        ///     next becomes current, and next is empty.
        /// </summary>
        /// <returns>The track that was previous before the move (it is no longer in a slot unless it is also preloaded).</returns>
        public Track Advance()
        {
            lock (_lock)
            {
                var oldPrevious = Previous;
                Previous = Current;
                Current = Next;
                Next = null;
                return oldPrevious;
            }
        }

        /// <summary>
        ///     The tracks in a slot: current, next, previous, preloaded (empty slots left out).
        /// </summary>
        public List<Track> InUse()
        {
            lock (_lock)
            {
                return new[] { Current, Next, Previous, Preloaded }.Where(t => t != null).ToList();
            }
        }

        /// <summary>
        ///     Determines whether a track is in any slot.
        /// </summary>
        public bool IsInUse(Track track)
        {
            return track != null && InUse().Any(t => TrackHelper.IsSameTrack(track, t));
        }

        /// <summary>
        ///     Finds the in-use track playing on a BASS channel.
        /// </summary>
        public Track FindByChannel(int channelId)
        {
            return InUse().FirstOrDefault(t => t.ChannelId == channelId);
        }
    }
}
