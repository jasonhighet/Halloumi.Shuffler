using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Halloumi.Shuffler.Controls.ModulePlayerControls
{
    public partial class ListBuilder : UserControl
    {
        private readonly Font _font = new Font("Segoe UI", 9, GraphicsUnit.Point);


        public ListBuilder()
        {
            InitializeComponent();

            SourceList = new List<string>();
            DestinationList = new List<string>();

            lstDestination.Font = _font;
            lstSource.Font = _font;

            lstSource.HideSelection = false;
            lstDestination.HideSelection = false;
        }

        private List<string> SourceList { get; set; }

        private List<string> DestinationList { get; set; }

        public bool AllowMultipleSourceItemsInDestination { get; set; }

        public void SetSourceList(List<string> sourceList)
        {
            SourceList = sourceList.ToList();
            BindData();
        }

        public void SetDestinationList(List<string> destinationList)
        {
            DestinationList = destinationList.ToList();
            BindData();
        }

        private void BindData()
        {
            BindSourceList();
            BindDestinationList();
        }

        private void BindDestinationList()
        {
            lstDestination.BeginUpdate();
            lstDestination.Items.Clear();

            var items = DestinationList
                .Select(value => new ListViewItem(value) {Tag = value})
                .ToArray();

            lstDestination.Items.AddRange(items);
            lstDestination.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lstDestination.EndUpdate();
        }

        private void BindSourceList()
        {
            lstSource.BeginUpdate();
            lstSource.Items.Clear();

            var items = SourceList
                .Select(value => new ListViewItem(value) {Tag = value})
                .ToArray();

            lstSource.Items.AddRange(items);
            lstSource.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lstSource.EndUpdate();
        }

        public List<string> GetDestinationList()
        {
            return DestinationList.ToList();
        }

        public event EventHandler OnDestinationListChanged;

        private void btnClear_Click(object sender, EventArgs e)
        {
            DestinationList.Clear();
            BindData();
            RaiseOnChanged();
        }

        private void RaiseOnChanged()
        {
            OnDestinationListChanged?.Invoke(this, EventArgs.Empty);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var selectedItems = GetSelectedItems(lstSource);
            if (!selectedItems.Any()) return;

            DestinationList.AddRange(selectedItems);
            BindData();
            RaiseOnChanged();
        }

        private static List<string> GetSelectedItems(ListView listView)
        {
            return listView
                .SelectedItems
                .Cast<object>()
                .Select((t, i) => listView.SelectedItems[i].Tag.ToString())
                .ToList();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            var selectedItems = GetSelectedItems(lstSource);
            if (!selectedItems.Any()) return;

            var destinationIndex = GetSelectedIndex(lstDestination);
            DestinationList.InsertRange(destinationIndex, selectedItems);

            BindData();
            RaiseOnChanged();
        }

        private static int GetSelectedIndex(ListView listView)
        {
            return listView.SelectedIndices.Count == 0 ? 0 : listView.SelectedIndices[0];
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var selectedItems = GetSelectedItems(lstDestination);
            if (!selectedItems.Any()) return;

            foreach (ListViewItem item in lstDestination.SelectedItems)
            {
                item.Remove();
            }
            DestinationList = lstDestination
                .Items
                .Cast<object>()
                .Select((t, i) => lstDestination.Items[i].Tag.ToString())
                .ToList();

            RaiseOnChanged();
        }
    }
}