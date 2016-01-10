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
    public partial class frmUpdateTrackTitle : BaseForm
    {
        public frmUpdateTrackTitle()
        {
            InitializeComponent();
        }

        public Library Library { get; set; }
        public List<Track> Tracks { get; set; }

        private void frmUpdateTrackTitle_Load(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            var names = this.Tracks.Select(t => t.Title)
            .OrderBy(t => t)
            .Distinct()
            .ToList();
            names.Insert(0, "");
            cmbTitle.DataSource = names;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            if (cmbTitle.Text.Trim() == "") return;

            this.Cursor = Cursors.Hand;
            Application.DoEvents();


            var destinationTracks = this.Tracks.Where(t => t.Title != cmbTitle.Text).ToList();
            foreach (var track in destinationTracks)
            {
                try
                {
                    this.Library.UpdateTitle(track, cmbTitle.Text, chkUpdateAuxillaryFiles.Checked);
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
