using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Halloumi.Common.Windows.Forms;
using Halloumi.Shuffler.Engine;
using Halloumi.Shuffler.Engine.Models;

namespace Halloumi.Shuffler.Forms
{
    public partial class FrmUpdateTrackAudio : BaseForm
    {
        public FrmUpdateTrackAudio()
        {
            InitializeComponent();
        }

        public Library Library { get; set; }
        public List<Track> DestinationTracks { get; set; }
        public List<Track> SourceTracks { get; set; }

        private class TrackModel
        {
            public string Description { get; set; }
            public string Filename { get; set; }
        }

        private void frmUpdateAlbum_Load(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            var sourceTracks = SourceTracks.Select(t => new TrackModel()
            {
                Description = string.Format("{0} ({1}) - {2}", t.Title, t.Album, t.FullLengthFormatted),
                Filename = t.Filename
            })
            .OrderBy(t => t.Description)
            .ToList();

            if (DestinationTracks.Count == 1)
            {
                var sourceTrack = sourceTracks.Where(t => t.Filename == SourceTracks[0].Filename).FirstOrDefault();
                sourceTracks.Remove(sourceTrack);
            }

            sourceTracks.Insert(0, new TrackModel() { Description = "", Filename = "" });

            cmbSourceTrack.ValueMember = "Filename";
            cmbSourceTrack.DisplayMember = "Description";
            cmbSourceTrack.DataSource = sourceTracks;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            if (cmbSourceTrack.Text.Trim() == "") return;

            Cursor = Cursors.Hand;
            Application.DoEvents();

            var sourceTrackFile = cmbSourceTrack.SelectedValue.ToString();
            var sourceTrack = SourceTracks.Where(t => t.Filename == sourceTrackFile).FirstOrDefault();

            foreach (var destinationTrack in DestinationTracks)
            {
                if (destinationTrack.Filename == sourceTrack.Filename) continue;

                try
                {
                    Library.CopyAudioFromAnotherTrack(destinationTrack, sourceTrack);
                }
                catch (Exception e)
                {
                    HandleException(e);
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
