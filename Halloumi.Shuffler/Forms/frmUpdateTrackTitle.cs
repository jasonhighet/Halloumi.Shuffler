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
    public partial class FrmUpdateTrackTitle : BaseForm
    {
        public FrmUpdateTrackTitle()
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
            var names = Tracks.Select(t => t.Title)
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

            Cursor = Cursors.Hand;
            Application.DoEvents();


            var destinationTracks = Tracks.Where(t => t.Title != cmbTitle.Text).ToList();
            foreach (var track in destinationTracks)
            {
                try
                {
                    Library.UpdateTitle(track, cmbTitle.Text, chkUpdateAuxillaryFiles.Checked);
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
