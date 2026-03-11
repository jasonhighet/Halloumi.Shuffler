using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Halloumi.Shuffler.AudioEngine.BassPlayer;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.AudioLibrary.Models;

namespace Halloumi.Shuffler.Controls
{
    public partial class ShufflerController : Component
    {
        private BackgroundWorker _generateWorker;

        /// <summary>
        ///     Initializes a new instance of the ShufflerController class.
        /// </summary>
        public ShufflerController()
        {
            InitializeComponent();
        }

        /// <summary>
        ///// Initializes a new instance of the ShufflerController class.
        /// </summary>
        /// <param name="container">The container.</param>
        public ShufflerController(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        ///     Gets or sets the shuffler application.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ShufflerApplication Application { get; set; }

        /// <summary>
        ///     Gets or sets the library.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TrackLibraryControl LibraryControl { get; set; }


        /// <summary>
        ///     Gets the playlist control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PlaylistControl PlaylistControl { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether playlist automatic generating is on.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AutoGenerateEnabled
        {
            get => Application != null && Application.AutoGenerateEnabled;
            set
            {
                if (Application != null) Application.AutoGenerateEnabled = value;
            }
        }

        /// <summary>
        ///     Initalizes this instance.
        /// </summary>
        public void Initalize()
        {
            _generateWorker = new BackgroundWorker();
            _generateWorker.DoWork += GenerateWorker_DoWork;
            _generateWorker.RunWorkerCompleted += GenerateWorker_RunWorkerCompleted;

            // Wire up the playlist count provider so ShufflerApplication can evaluate
            // the threshold without holding any WinForms reference.
            Application.PlaylistTrackCountProvider = () => PlaylistControl.GetNumberOfTracksRemaining();

            Application.OnAutoGenerateRequired += Application_OnAutoGenerateRequired;
        }

        /// <summary>
        ///     Handles the Application OnAutoGenerateRequired event.
        ///     ShufflerApplication has already verified that the threshold is met;
        ///     we just need to marshal onto the UI thread and trigger generation.
        /// </summary>
        private void Application_OnAutoGenerateRequired(object sender, EventArgs e)
        {
            if (PlaylistControl.InvokeRequired)
                PlaylistControl.BeginInvoke(new MethodInvoker(AutoGeneratePlaylist));
            else
                AutoGeneratePlaylist();
        }

        private void AutoGeneratePlaylist()
        {
            if (_generateWorker.IsBusy) return;

            // Capture UI state on the UI thread before handing off to the worker
            var request = Application.GetPlaylistGenerationRequest();
            request.ApproximateLengthMinutes = int.MaxValue;

            List<Track> availableTracks;
            if (request.DisplayedTracksOnly && LibraryControl != null)
                availableTracks = LibraryControl.DisplayedTracks.Where(t => t.IsShufflerTrack).ToList();
            else
                availableTracks = LibraryControl.AvailableTracks.Where(t => t.IsShufflerTrack).ToList();

            var currentPlaylist = PlaylistControl.GetTracks();

            _generateWorker.RunWorkerAsync(
                Tuple.Create(request, availableTracks, currentPlaylist));
        }

        private void GenerateWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var args = (Tuple<PlaylistGenerationRequest, List<Track>, List<Track>>)e.Argument;
            var tracks = Application.GeneratePlaylist(args.Item1, args.Item2, args.Item3);
            e.Result = Tuple.Create(tracks, args.Item1.Direction);
        }

        private void GenerateWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result == null) return;

            var result = (Tuple<List<Track>, TrackSelector.GenerateDirection>)e.Result;
            var tracks = result.Item1;

            if (tracks.Count == 0) return;

            var currentCount = PlaylistControl.GetTracks().Count;
            var additionalCount = tracks.Count - currentCount;
            if (additionalCount <= 0) return;

            tracks.Reverse();
            var toAdd = tracks.Take(additionalCount).ToList();
            toAdd.Reverse();
            PlaylistControl.QueueTracks(toAdd);
        }

        /// <summary>
        ///     Automatically generates the playlist now.
        /// </summary>
        public void AutoGenerateNow()
        {
            AutoGeneratePlaylist();
        }
    }
}
