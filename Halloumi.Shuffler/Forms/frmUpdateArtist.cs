using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Halloumi.Common.Windows.Forms;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.AudioLibrary.Models;

namespace Halloumi.Shuffler.Forms
{
    public partial class FrmUpdateArtist : BaseForm
    {
        public FrmUpdateArtist()
        {
            InitializeComponent();
            
            Tracks = null;
            Artist = "";
            Album = "";
        }

        public Library Library { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public List<Track> Tracks { get; set; }

        private void frmUpdateArtist_Load(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            var albums = Library.GetAllArtists();
            cmbArtist.ValueMember = "Name";
            cmbArtist.DisplayMember = "Name";
            cmbArtist.DataSource = albums;
            

            if (Album != "")
            {
                Text = "Update Album Artist";

                var albumTracks = Library.GetAllTracksForAlbum(Album);
                if (albumTracks.Count > 0) cmbArtist.Text = albumTracks[0].AlbumArtist;

            }
            else if (Tracks != null)
            {
                Text = "Rename Artist";
                cmbArtist.Text = Tracks[0].Artist;
            }
            else
            {
                Text = "Update Artist";
                cmbArtist.Text = Artist;
            }

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            if (cmbArtist.Text.Trim() == "") return;

            Cursor = Cursors.Hand;
            Application.DoEvents();

            if (Album != "")
            {
                Library.UpdateAlbumArtist(Album, cmbArtist.Text);
            }
            else if (Tracks != null)
            {
                Library.UpdateArtist(Tracks, cmbArtist.Text);
            }
            else
            {
                Library.RenameArtist(Artist, cmbArtist.Text);
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
