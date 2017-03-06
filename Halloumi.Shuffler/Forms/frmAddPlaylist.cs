using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Halloumi.Common.Windows.Forms;
using Halloumi.Shuffler.AudioLibrary;
using Halloumi.Shuffler.AudioLibrary.Helpers;
using Halloumi.Shuffler.AudioLibrary.Models;

namespace Halloumi.Shuffler.Forms
{
    public partial class FrmAddPlaylist : BaseForm
    {
        public FrmAddPlaylist()
        {
            InitializeComponent();

            txtPlaylistName.IsRequired = true;
            txtPlaylistName.ErrorMessage = "Please enter a playlist name";

            Tracks = null;
        }

        public Library Library { get; set; }
        public List<Track> Tracks { get; set; }


        private void frmAddPlaylist_Load(object sender, EventArgs e)
        {
            BindData();
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        { }

        private void btnOK_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            var playlistName = txtPlaylistName.Text.Trim();
            if (playlistName == "") return;

            Cursor = Cursors.Hand;
            Application.DoEvents();

            PlaylistHelper.AddTracksToPlaylist(playlistName, Tracks);

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
