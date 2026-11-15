using System;
using System.Windows.Forms;
using Halloumi.Common.Windows.Forms;

namespace Halloumi.Shuffler.Forms
{
    /// <summary>
    ///     Asks for one value from a list (rename artist, album or genre, pick a title or source audio),
    ///     then runs the caller's update when OK is clicked.
    /// </summary>
    public partial class FrmChooseValue : BaseForm
    {
        public FrmChooseValue()
        {
            InitializeComponent();
        }

        /// <summary>The list to choose from.</summary>
        public object Items { get; set; }

        /// <summary>Property of each item to show; empty when the items are strings.</summary>
        public string ItemDisplayMember { get; set; } = "";

        /// <summary>Property of each item returned by SelectedValue; empty when the items are strings.</summary>
        public string ItemValueMember { get; set; } = "";

        /// <summary>The value shown when the dialog opens.</summary>
        public string InitialText { get; set; } = "";

        /// <summary>True to allow typing a value that is not in the list.</summary>
        public bool AllowNewValue { get; set; } = true;

        /// <summary>Text of an optional check box; null hides it.</summary>
        public string OptionText { get; set; }

        public bool OptionChecked
        {
            get => chkOption.Checked;
            set => chkOption.Checked = value;
        }

        /// <summary>Runs when OK is clicked with a non-blank value.</summary>
        public Action<FrmChooseValue> Apply { get; set; }

        public string SelectedText => cmbValue.Text;

        public object SelectedValue => cmbValue.SelectedValue;

        /// <summary>Reports an error from Apply without stopping it, e.g. when updating several tracks.</summary>
        public void ShowError(Exception exception)
        {
            HandleException(exception);
        }

        private void FrmChooseValue_Load(object sender, EventArgs e)
        {
            if (OptionText == null)
            {
                chkOption.Visible = false;
                var unused = pnlMain.Height - cmbValue.Height - pnlMain.Padding.Vertical;
                ClientSize = new System.Drawing.Size(ClientSize.Width, ClientSize.Height - unused);
            }
            else
            {
                chkOption.Values.Text = OptionText;
            }

            cmbValue.DropDownStyle = AllowNewValue ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList;
            cmbValue.DisplayMember = ItemDisplayMember;
            cmbValue.ValueMember = ItemValueMember;
            cmbValue.DataSource = Items;
            cmbValue.Text = InitialText;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cmbValue.Text.Trim() == "") return;

            Cursor = Cursors.WaitCursor;
            System.Windows.Forms.Application.DoEvents();

            try
            {
                Apply?.Invoke(this);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
