using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Halloumi.Shuffler.Controls
{
    public partial class ListBuilder : UserControl
    {
        private readonly Font _font = new Font("Segoe UI", 9, GraphicsUnit.Point);


        public ListBuilder()
        {
            InitializeComponent();

            AvailableItems = new List<string>();
            SelectedItems = new List<string>();

            lstSelectedItems.Font = _font;
            lstAvailableItems.Font = _font;

            lstAvailableItems.HideSelection = false;
            lstSelectedItems.HideSelection = false;

            PropertiesButtonVisible = false;
        }

        private List<string> AvailableItems { get; set; }

        private List<string> SelectedItems { get; set; }

        public bool AllowMultipleAvailableItems { get; set; }

        public bool PropertiesButtonVisible { get; set; }

        public void SetAvailableItems(List<string> sourceList)
        {
            AvailableItems = sourceList.ToList();
            BindData();
        }

        public void SetSelectedItems(List<string> destinationList)
        {
            SelectedItems = destinationList.ToList();
            BindData();
        }

        private void BindData()
        {
            BindAvailableItems();
            BindSelectedItems();

            btnProperties.Visible = PropertiesButtonVisible;
        }

        private void BindSelectedItems()
        {
            lstSelectedItems.BeginUpdate();
            lstSelectedItems.Items.Clear();

            var items = SelectedItems
                .Select(value => new ListViewItem(value) {Tag = value})
                .ToArray();

            lstSelectedItems.Items.AddRange(items);
            lstSelectedItems.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lstSelectedItems.EndUpdate();
        }

        private void BindAvailableItems()
        {
            lstAvailableItems.BeginUpdate();
            lstAvailableItems.Items.Clear();

            var items = AvailableItems
                .Select(value => new ListViewItem(value) {Tag = value})
                .ToArray();

            lstAvailableItems.Items.AddRange(items);
            lstAvailableItems.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lstAvailableItems.EndUpdate();
        }

        public List<string> GetSelectedItems()
        {
            return SelectedItems.ToList();
        }

        public List<string> GetSelectedSelectedItems()
        {
            return GetSelectedItems(lstSelectedItems);
        }

        public event EventHandler SelectedItemsChanged;

        public event EventHandler PropertiesClicked;

        private void btnClear_Click(object sender, EventArgs e)
        {
            SelectedItems.Clear();
            BindData();
            RaiseSelectedItemsChanged();
        }

        private void RaiseSelectedItemsChanged()
        {
            SelectedItemsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void RaisePropertiesClicked()
        {
            PropertiesClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var selectedItems = GetSelectedItems(lstAvailableItems);
            if (!selectedItems.Any()) return;

            SelectedItems.AddRange(selectedItems);
            BindData();
            RaiseSelectedItemsChanged();
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
            var selectedItems = GetSelectedItems(lstAvailableItems);
            if (!selectedItems.Any()) return;

            var destinationIndex = GetSelectedIndex(lstSelectedItems);
            SelectedItems.InsertRange(destinationIndex, selectedItems);

            BindData();
            RaiseSelectedItemsChanged();
        }

        private static int GetSelectedIndex(ListView listView)
        {
            return listView.SelectedIndices.Count == 0 ? 0 : listView.SelectedIndices[0];
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var selectedItems = GetSelectedItems(lstSelectedItems);
            if (!selectedItems.Any()) return;

            foreach (ListViewItem item in lstSelectedItems.SelectedItems)
            {
                item.Remove();
            }
            SelectedItems = lstSelectedItems
                .Items
                .Cast<object>()
                .Select((t, i) => lstSelectedItems.Items[i].Tag.ToString())
                .ToList();

            RaiseSelectedItemsChanged();
        }

        private void btnProperties_Click(object sender, EventArgs e)
        {
            RaisePropertiesClicked();
        }
    }
}