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
    public partial class frmUpdateAlbum : BaseForm
    {
        public frmUpdateAlbum()
        {
            InitializeComponent();
            
            this.Tracks = null;
            this.Album = "";
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
            var albums = this.Library.GetAllAlbums();
            cmbAlbum.ValueMember = "Name";
            cmbAlbum.DisplayMember = "Name";
            cmbAlbum.DataSource = albums;

            if (this.Tracks == null)
            {
                this.Text = "Rename Album";
                cmbAlbum.Text = this.Album;
            }
            else
            {
                this.Text = "Update Album";
                cmbAlbum.Text = this.Tracks[0].Album;

            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            if (cmbAlbum.Text.Trim() == "") return;

            this.Cursor = Cursors.Hand;
            Application.DoEvents();

            if (this.Tracks != null)
            {
                this.Library.UpdateAlbum(this.Tracks, cmbAlbum.Text);
            }
            else
            {
                this.Library.RenameAlbum(this.Album, cmbAlbum.Text);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
