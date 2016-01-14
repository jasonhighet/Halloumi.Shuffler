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
    public partial class FrmUpdateArtist : BaseForm
    {
        public FrmUpdateArtist()
        {
            InitializeComponent();
            
            this.Tracks = null;
            this.Artist = "";
            this.Album = "";
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
            var albums = this.Library.GetAllArtists();
            cmbArtist.ValueMember = "Name";
            cmbArtist.DisplayMember = "Name";
            cmbArtist.DataSource = albums;
            

            if (this.Album != "")
            {
                this.Text = "Update Album Artist";

                var albumTracks = this.Library.GetAllTracksForAlbum(this.Album);
                if (albumTracks.Count > 0) cmbArtist.Text = albumTracks[0].AlbumArtist;

            }
            else if (this.Tracks != null)
            {
                this.Text = "Rename Artist";
                cmbArtist.Text = this.Tracks[0].Artist;
            }
            else
            {
                this.Text = "Update Artist";
                cmbArtist.Text = this.Artist;
            }

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            if (cmbArtist.Text.Trim() == "") return;

            this.Cursor = Cursors.Hand;
            Application.DoEvents();

            if (this.Album != "")
            {
                this.Library.UpdateAlbumArtist(this.Album, cmbArtist.Text);
            }
            else if (this.Tracks != null)
            {
                this.Library.UpdateArtist(this.Tracks, cmbArtist.Text);
            }
            else
            {
                this.Library.RenameArtist(this.Artist, cmbArtist.Text);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
