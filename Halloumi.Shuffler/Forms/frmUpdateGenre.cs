using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Halloumi.Common.Windows.Forms;
using Halloumi.Shuffler.Engine;

namespace Halloumi.Shuffler.Forms
{
    public partial class FrmUpdateGenre : BaseForm
    {
        /// <summary>
        /// Initializes a new instance of the frmUpdateGenre class.
        /// </summary>
        public FrmUpdateGenre()
        {
            InitializeComponent();
            
            Tracks = null;
            Genre = "";
        }

        /// <summary>
        /// Gets or sets the library.
        /// </summary>
        public Library Library { get; set; }
        
        /// <summary>
        /// Gets or sets the genre.
        /// </summary>
        public string Genre { get; set; }
        
        /// <summary>
        /// Gets or sets the tracks.
        /// </summary>
        public List<Track> Tracks { get; set; }

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        {
            var genres = Library.GetAllGenres();
            cmbGenre.ValueMember = "Name";
            cmbGenre.DisplayMember = "Name";
            cmbGenre.DataSource = genres;

            if (Tracks == null)
            {
                Text = "Rename Genre";
                cmbGenre.Text = Genre;
            }
            else
            {
                Text = "Update Genre";
                cmbGenre.Text = Tracks[0].Genre;
            }
        }

        /// <summary>
        /// Updates the data.
        /// </summary>
        private void UpdateData()
        {
            if (cmbGenre.Text.Trim() == "") return;

            Cursor = Cursors.Hand;
            Application.DoEvents();

            if (Tracks != null)
            {
                Library.UpdateGenre(Tracks, cmbGenre.Text);
            }
            else
            {
                Library.RenameGenre(Genre, cmbGenre.Text);
            }

            DialogResult = DialogResult.OK;
            Close();
        }


        /// <summary>
        /// Handles the Load event of the frmUpdateGenre control.
        /// </summary>
        private void frmUpdateGenre_Load(object sender, EventArgs e)
        {
            BindData();
        }

        /// <summary>
        /// Handles the Click event of the btnOK control.
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            UpdateData();
        }
    }
}
