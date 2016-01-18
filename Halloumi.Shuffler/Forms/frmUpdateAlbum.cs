using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Halloumi.Common.Windows.Forms;
using Halloumi.Shuffler.Engine;
using Halloumi.Shuffler.Engine.Models;

namespace Halloumi.Shuffler.Forms
{
    public partial class FrmUpdateAlbum : BaseForm
    {
        public FrmUpdateAlbum()
        {
            InitializeComponent();
            
            Tracks = null;
            Album = "";
        }

        public Library Library { get; set; }
        public string Album { get; set; }
        public List<Track> Tracks { get; set; }


        private void frmUpdateAlbum_Load(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            var albums = Library.GetAllAlbums();
            cmbAlbum.ValueMember = "Name";
            cmbAlbum.DisplayMember = "Name";
            cmbAlbum.DataSource = albums;

            if (Tracks == null)
            {
                Text = "Rename Album";
                cmbAlbum.Text = Album;
            }
            else
            {
                Text = "Update Album";
                cmbAlbum.Text = Tracks[0].Album;

            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            if (cmbAlbum.Text.Trim() == "") return;

            Cursor = Cursors.Hand;
            Application.DoEvents();

            if (Tracks != null)
            {
                Library.UpdateAlbum(Tracks, cmbAlbum.Text);
            }
            else
            {
                Library.RenameAlbum(Album, cmbAlbum.Text);
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
