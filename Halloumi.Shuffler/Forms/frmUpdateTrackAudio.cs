using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Halloumi.Common.Windows.Forms;
using Halloumi.Shuffler.Engine;

namespace Halloumi.Shuffler.Forms
{
    public partial class frmUpdateTrackAudio : BaseForm
    {
        public frmUpdateTrackAudio()
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
            var sourceTracks = this.SourceTracks.Select(t => new TrackModel()
            {
                Description = string.Format("{0} ({1}) - {2}", t.Title, t.Album, t.FullLengthFormatted),
                Filename = t.Filename
            })
            .OrderBy(t => t.Description)
            .ToList();

            if (this.DestinationTracks.Count == 1)
            {
                var sourceTrack = sourceTracks.Where(t => t.Filename == this.SourceTracks[0].Filename).FirstOrDefault();
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

            this.Cursor = Cursors.Hand;
            Application.DoEvents();

            var sourceTrackFile = cmbSourceTrack.SelectedValue.ToString();
            var sourceTrack = this.SourceTracks.Where(t => t.Filename == sourceTrackFile).FirstOrDefault();

            foreach (var destinationTrack in this.DestinationTracks)
            {
                if (destinationTrack.Filename == sourceTrack.Filename) continue;

                try
                {
                    this.Library.CopyAudioFromAnotherTrack(destinationTrack, sourceTrack);
                }
                catch (Exception e)
                {
                    this.HandleException(e);
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
