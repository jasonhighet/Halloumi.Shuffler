using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Halloumi.Common.Helpers;
using Halloumi.Common.Windows.Forms;
using Halloumi.Shuffler.AudioEngine.BassPlayer;
using Halloumi.Shuffler.AudioLibrary.Models;
using Halloumi.Shuffler;

namespace Halloumi.Shuffler.Forms
{
    public partial class FrmUpdateSimilarTracks : BaseForm
    {
        public FrmUpdateSimilarTracks()
        {
            InitializeComponent();
        }

        public ShufflerApplication Application { get; set; }

        public BassPlayer BassPlayer { get; set; }

        public List<Track> SimilarTracks { get; set; }

        /// <summary>
        ///     Updates the details of the selected track
        /// </summary>
        private void UpdateTrackDetails()
        {
            var selectedTrack = GetSelectedTrack();
            if (selectedTrack == null) return;

            var form = new FrmUpdateTrackDetails
            {
                Application = Application,
                Track = selectedTrack
            };
            var result = form.ShowDialog();
            if (result == DialogResult.OK) BindData();
        }

        /// <summary>
        ///     Updates the shuffler details.
        /// </summary>
        private void UpdateShufflerDetails()
        {
            if (GetSelectedTrack() == null) return;

            if (FrmShufflerDetails.OpenForm(GetSelectedTrack().Filename, BassPlayer, Application.Library, Application) == DialogResult.OK)
            {
                BindData();
            }
        }

        /// <summary>
        ///     Gets the selected track.
        /// </summary>
        /// <returns>The selected track</returns>
        public Track GetSelectedTrack()
        {
            if (grdTracks.SelectedRows.Count == 0) return null;
            return grdTracks.SelectedRows[0].DataBoundItem as Track;
        }

        /// <summary>
        ///     Gets the selected tracks.
        /// </summary>
        /// <returns>The selected tracks</returns>
        private List<Track> GetSelectedTracks()
        {
            var tracks = new List<Track>();
            for (var i = 0; i < grdTracks.Rows.Count; i++)
            {
                var row = grdTracks.Rows[i];
                if (row.Selected) tracks.Add(row.DataBoundItem as Track);
            }
            return tracks;
        }

        private void frmSimilarTracks_Load(object sender, EventArgs e)
        {
            SimilarTracks = Application.GetDuplicateButDifferentShufflerTracks();
            BindData();
        }

        private void BindData()
        {
            grdTracks.DataSource = SimilarTracks;
        }

        private void mnuUpdateTrackDetails_Click(object sender, EventArgs e)
        {
            UpdateTrackDetails();
        }

        private void mnuUpdateShufflerDetails_Click(object sender, EventArgs e)
        {
            UpdateShufflerDetails();
        }

        private void mnuUpdateAudioData_Click(object sender, EventArgs e)
        {
            if (GetSelectedTracks().Count == 0) return;

            var destinationTracks = GetSelectedTracks();
            var sourceTracks = GetSimilarTracks(GetSelectedTrack());

            var choices = sourceTracks
                .Where(t => destinationTracks.Count != 1 || t.Filename != destinationTracks[0].Filename)
                .Select(t => new SourceAudioChoice
                {
                    Description = $"{t.Title} ({t.Album}) - {t.FullLengthFormatted}",
                    Filename = t.Filename
                })
                .OrderBy(t => t.Description)
                .ToList();
            choices.Insert(0, new SourceAudioChoice { Description = "", Filename = "" });

            var form = new FrmChooseValue
            {
                Text = "Select Source Audio",
                Items = choices,
                ItemDisplayMember = nameof(SourceAudioChoice.Description),
                ItemValueMember = nameof(SourceAudioChoice.Filename),
                AllowNewValue = false,
                Apply = f =>
                {
                    var sourceFilename = (string)f.SelectedValue;
                    foreach (var destinationTrack in destinationTracks.Where(t => t.Filename != sourceFilename))
                    {
                        try
                        {
                            Application.CopyAudioFromAnotherTrack(destinationTrack, sourceFilename);
                        }
                        catch (Exception exception)
                        {
                            f.ShowError(exception);
                        }
                    }
                }
            };
            form.ShowDialog();
            BindData();
        }

        /// <summary>
        ///     Handles the Click event of the mnuUpdateTrackTitle control.
        /// </summary>
        private void mnuUpdateTrackTitle_Click(object sender, EventArgs e)
        {
            if (GetSelectedTracks().Count == 0) return;

            var tracks = GetSelectedTracks();
            var titles = tracks.Select(t => t.Title).OrderBy(t => t).Distinct().ToList();
            titles.Insert(0, "");

            var form = new FrmChooseValue
            {
                Text = "Select Title",
                Items = titles,
                AllowNewValue = false,
                OptionText = "Rename in shuffler files, playlists etc.",
                OptionChecked = true,
                Apply = f =>
                {
                    foreach (var track in tracks.Where(t => t.Title != f.SelectedText))
                    {
                        try
                        {
                            Application.UpdateTitle(track, f.SelectedText, f.OptionChecked);
                        }
                        catch (Exception exception)
                        {
                            f.ShowError(exception);
                        }
                    }
                }
            };
            form.ShowDialog();
            BindData();
        }

        private class SourceAudioChoice
        {
            public string Description { get; set; }
            public string Filename { get; set; }
        }

        /// <summary>
        ///     Gets similar tracks to a track
        /// </summary>
        /// <param name="track">The track.</param>
        /// <returns>A list of similar tracks</returns>
        private List<Track> GetSimilarTracks(Track track)
        {
            return SimilarTracks.Where(t => t.Artist == track.Artist && StringHelper.FuzzyCompare(track.Title, t.Title))
                .ToList();
        }
    }
}