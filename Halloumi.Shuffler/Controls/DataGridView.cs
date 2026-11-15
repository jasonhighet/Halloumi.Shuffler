using System;
using System.Windows.Forms;

namespace Halloumi.Shuffler.Controls
{
    public class DataGridView : Common.Windows.Controls.DataGridView
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DataGridView class.
        /// </summary>
        public DataGridView()
            : base()
        {
            ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(DataGridView_ColumnHeaderMouseClick);
            ColumnHeaderMouseDoubleClick += new DataGridViewCellMouseEventHandler(DataGridView_ColumnHeaderMouseDoubleClick);
            SortColumnIndex = -1;
            base.ColumnHeadersHeight = 44;
            DoubleBuffered = true;

            this.RowTemplate.Height = 44;
        }

        #endregion

        public void SetSortColumn(int columnIndex, SortOrder sortOrder)
        {
            if (columnIndex < 0 || columnIndex >= ColumnCount) return;

            SortColumnIndex = columnIndex;
            SortOrder = sortOrder;
            SetSortGlyph();
        }

        #region Private Methods

        /// <summary>
        /// Sets the sort glyph.
        /// </summary>
        private void SetSortGlyph()
        {
            for (var i = 0; i < Columns.Count; i++)
            {
                if (Columns[i].SortMode != DataGridViewColumnSortMode.Programmatic) continue;
                var sortOrder = SortOrder.None;
                if (i == SortColumnIndex) sortOrder = SortOrder;
                Columns[i].HeaderCell.SortGlyphDirection = sortOrder;
            }
        }

        /// <summary>
        /// Sets the new sort order.
        /// </summary>
        /// <param name="columnIndex">Index of the sort column that was clicked.</param>
        private void SetNewSortOrder(int columnIndex)
        {
            var oldSortColumnIndex = SortColumnIndex;
            SortColumnIndex = columnIndex;

            if (SortColumnIndex == oldSortColumnIndex)
            {
                if (SortOrder == SortOrder.Ascending) SortOrder = SortOrder.Descending;
                else SortOrder = SortOrder.None;
            }
            else
            {
                SortOrder = SortOrder.Ascending;
            }

            if (SortOrder == SortOrder.None) SortColumnIndex = -1;

            SetSortGlyph();

            if (SortOrderChanged != null) SortOrderChanged(this, EventArgs.Empty);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the index of the sort column.
        /// </summary>
        public int SortColumnIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether the items in the DataGridView control are sorted in ascending or descending order, or are not sorted.
        /// </summary>
        /// <returns>One of the SortOrder values.</returns>
        public new SortOrder SortOrder { get; set; }

        /// <summary>
        /// Gets the sort colum.
        /// </summary>
        public new DataGridViewColumn SortedColumn
        {
            get
            {
                if (SortColumnIndex < 0 || SortColumnIndex >= ColumnCount) return null;
                return Columns[SortColumnIndex];
            }
        }

        /// <summary>
        /// Gets or sets the data source that the DataGridView is displaying data for.
        /// </summary>
        public new object DataSource
        {
            get { return base.DataSource; }
            set
            {
                base.DataSource = value;
                SetSortGlyph();
            }
        }

        /// <summary>
        /// Gets or sets the height, in pixels, of the column headers row
        /// </summary>
        /// <returns>
        /// The height, in pixels, of the row that contains the column headers. The default is 27.
        /// </returns>
        public new int ColumnHeadersHeight
        {
            get { return base.ColumnHeadersHeight; }
            set { base.ColumnHeadersHeight = base.ColumnHeadersHeight; }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the ColumnHeaderMouseDoubleClick event of the DataGridView control.
        /// </summary>
        private void DataGridView_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (Columns[e.ColumnIndex].SortMode == DataGridViewColumnSortMode.Programmatic) SetNewSortOrder(e.ColumnIndex);
        }

        /// <summary>
        /// Handles the ColumnHeaderMouseClick event of the DataGridView control.
        /// </summary>
        private void DataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (Columns[e.ColumnIndex].SortMode == DataGridViewColumnSortMode.Programmatic) SetNewSortOrder(e.ColumnIndex);
        }

        //            // invoke drag-drop on right click

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the sort order is changed.
        /// </summary>
        public event EventHandler SortOrderChanged;

        #endregion
    }
}