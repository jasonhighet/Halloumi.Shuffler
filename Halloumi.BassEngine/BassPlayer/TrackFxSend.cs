using Halloumi.Shuffler.AudioEngine.Helpers;

namespace Halloumi.Shuffler.AudioEngine.BassPlayer
{
    /// <summary>
    ///     The track FX send: opens the send so the tracks also go through the send FX (usually a
    ///     BPM-synced delay), and records each send as a track FX trigger.
    /// </summary>
    internal class TrackFxSend
    {
        private readonly MixerGraph _graph;
        private readonly TrackDeck _deck;
        private readonly TriggerRecorder _recorder;

        public TrackFxSend(MixerGraph graph, TrackDeck deck, TriggerRecorder recorder)
        {
            _graph = graph;
            _deck = deck;
            _recorder = recorder;
        }

        /// <summary>
        ///     The delay length in notes (0.25 = a quarter note).
        /// </summary>
        public decimal DelayNotes
        {
            get { return _graph.TrackSendFxMixer.SyncNotes; }
            set { _graph.TrackSendFxMixer.SyncNotes = value; }
        }

        /// <summary>
        ///     True while audio is being sent to the track FX.
        /// </summary>
        public bool IsSending
        {
            get { return _graph.TrackSendMixer.GetVolume() != 0M; }
        }

        /// <summary>
        ///     Sets the delay length, then opens the send.
        /// </summary>
        public void Start(decimal delayNotes)
        {
            DelayNotes = delayNotes;
            Start();
        }

        /// <summary>
        ///     Opens the send and starts recording a track FX trigger on the current track.
        /// </summary>
        public void Start()
        {
            _graph.TrackSendMixer.SetVolume(50M);

            var track = _deck.Current;
            if (track == null) return;

            _recorder.StartTrackFx(track, AudioStreamHelper.GetPosition(track), DelayNotes);

            _graph.TrackSendMixer.SetPluginBpm();
        }

        /// <summary>
        ///     Closes the send and finishes recording the track FX trigger. Does nothing unless a
        ///     trigger is being recorded.
        /// </summary>
        public void Stop()
        {
            if (_deck.Current == null || _recorder.TrackFxTrigger == null || _recorder.TrackFxTrack == null)
                return;

            _graph.TrackSendMixer.SetVolume(0);
            _recorder.EndTrackFx(AudioStreamHelper.GetPosition(_recorder.TrackFxTrack));
        }
    }
}
