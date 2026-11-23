using System;
using System.Threading.Tasks;

namespace Halloumi.Shuffler.AudioEngine.BassPlayer
{
    /// <summary>
    ///     Runs work in order on a background thread. Sync handlers use it for anything that does
    ///     not have to happen at the sync's exact moment (raising events, freeing audio), so the
    ///     BASS mixer thread is not held up.
    /// </summary>
    internal class BackgroundQueue
    {
        private readonly object _lock = new object();
        private Task _tail = Task.CompletedTask;

        /// <summary>
        ///     Queues work to run after everything queued before it. An exception in one item is
        ///     logged and does not stop the queue.
        /// </summary>
        public void Post(Action action)
        {
            lock (_lock)
            {
                _tail = _tail.ContinueWith(previous => Run(action), TaskScheduler.Default);
            }
        }

        /// <summary>
        ///     Waits until everything queued so far has run (or the timeout passes).
        /// </summary>
        /// <returns>False if the timeout passed first.</returns>
        public bool WaitUntilIdle(int timeoutMilliseconds)
        {
            Task tail;
            lock (_lock)
            {
                tail = _tail;
            }
            return tail.Wait(timeoutMilliseconds);
        }

        private static void Run(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("BassPlayer background work failed: " + e);
            }
        }
    }
}
