using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Halloumi.Shuffler.Controls
{
    public class DataGridView : Halloumi.Common.Windows.Controls.DataGridView
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DataGridView class.
        /// </summary>
        public DataGridView()
            : base()
        {
            this.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(DataGridView_ColumnHeaderMouseClick);
            this.ColumnHeaderMouseDoubleClick += new DataGridViewCellMouseEventHandler(DataGridView_ColumnHeaderMouseDoubleClick);
            this.SortColumnIndex = -1;
            base.ColumnHeadersHeight = 26;
            base.DoubleBuffered = true;

            //this.MouseDown += new MouseEventHandler(DataGridView_MouseDown);
            //this.DragDrop += new DragEventHandler(DataGridView_DragDrop);
            //this.DragOver += new DragEventHandler(DataGridView_DragOver);
            //this.MouseUp +=new MouseEventHandler(DataGridView_MouseUp);
        }

        #endregion

        public void SetSortColumn(int columnIndex, SortOrder sortOrder)
        {
            if (columnIndex < 0 || columnIndex >= this.ColumnCount) return;

            this.SortColumnIndex = columnIndex;
            this.SortOrder = sortOrder;
            SetSortGlyph();
        }

        #region Private Methods

        /// <summary>
        /// Sets the sort glyph.
        /// </summary>
        private void SetSortGlyph()
        {
            for (var i = 0; i < this.Columns.Count; i++)
            {
                if (this.Columns[i].SortMode != DataGridViewColumnSortMode.Programmatic) continue;
                var sortOrder = SortOrder.None;
                if (i == this.SortColumnIndex) sortOrder = this.SortOrder;
                this.Columns[i].HeaderCell.SortGlyphDirection = sortOrder;
            }
        }

        /// <summary>
        /// Sets the new sort order.
        /// </summary>
        /// <param name="columnIndex">Index of the sort column that was clicked.</param>
        private void SetNewSortOrder(int columnIndex)
        {
            var oldSortColumnIndex = this.SortColumnIndex;
            this.SortColumnIndex = columnIndex;

            if (this.SortColumnIndex == oldSortColumnIndex)
            {
                if (this.SortOrder == SortOrder.Ascending) this.SortOrder = SortOrder.Descending;
                else this.SortOrder = SortOrder.None;
            }
            else
            {
                this.SortOrder = SortOrder.Ascending;
            }

            if (this.SortOrder == SortOrder.None) this.SortColumnIndex = -1;

            SetSortGlyph();

            if (this.SortOrderChanged != null) SortOrderChanged(this, EventArgs.Empty);
        }

        ///// <summary>
        ///// Gets the index of the destination row of a drag/drop event
        ///// </summary>
        ///// <param name="e">The DragEventArgs containing the drag/drop.</param>
        ///// <returns>The index of the destination row</returns>
        //private int GetDestinationRowIndex(DragEventArgs e)
        //{
        //    if (!IsDropSourceThisGrid(e)) return -1;
        //    var clientPoint = this.PointToClient(new Point(e.X, e.Y));
        //    return this.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
        //}

        ///// <summary>
        ///// Gets the destination row of a drag/drop event
        ///// </summary>
        ///// <param name="e">The DragEventArgs containing the drag/drop.</param>
        ///// <returns>The destiantion row </returns>
        //private DataGridViewRow GetDestinationRow(DragEventArgs e)
        //{
        //    var rowIndex = this.GetDestinationRowIndex(e);
        //    if (rowIndex >= 0 && rowIndex < this.Rows.Count) return this.Rows[rowIndex];
        //    return null;
        //}

        ///// <summary>
        ///// Determines whether the is drop source this grid
        ///// </summary>
        ///// <param name="e">The object containing the event data.</param>
        ///// <returns>True if the source of the drop is this grid.
        ///// </returns>
        //private bool IsDropSourceThisGrid(DragEventArgs e)
        //{
        //    var gridView = e.Data.GetData(typeof(DataGridView)) as DataGridView;
        //    return (gridView == this);
        //}

        ///// <summary>
        ///// Raises the row drag drop event
        ///// </summary>
        ///// <param name="destinationRow">The destination row.</param>
        //private void RaiseRowDragDrop(DataGridViewRow destinationRow)
        //{
        //    if (this.RowDragDrop != null)
        //    {
        //        var eventArgs = new RowDragDropEventArgs();
        //        eventArgs.DestinationRowIndex = destinationRow.Index;
        //        this.RowDragDrop(this, eventArgs);
        //    }
        //}

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
                if (this.SortColumnIndex < 0 || this.SortColumnIndex >= this.ColumnCount) return null;
                return this.Columns[this.SortColumnIndex];
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
            if (this.Columns[e.ColumnIndex].SortMode == DataGridViewColumnSortMode.Programmatic) SetNewSortOrder(e.ColumnIndex);
        }

        /// <summary>
        /// Handles the ColumnHeaderMouseClick event of the DataGridView control.
        /// </summary>
        private void DataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.Columns[e.ColumnIndex].SortMode == DataGridViewColumnSortMode.Programmatic) SetNewSortOrder(e.ColumnIndex);
        }

        ///// <summary>
        ///// Handles the DragDrop event of the DataGridView control.
        ///// </summary>
        //private void DataGridView_DragDrop(object sender, DragEventArgs e)
        //{
        //    if (!this.AllowDrop) return;

        //    var row = GetDestinationRow(e);
        //    if (row != null && !row.Selected) RaiseRowDragDrop(row);
        //}

        ///// <summary>
        ///// Handles the DragOver event of the DataGridView control.
        ///// </summary>
        //private void DataGridView_DragOver(object sender, DragEventArgs e)
        //{
        //    if (!this.AllowDrop) return;

        //    var row = GetDestinationRow(e);
        //    if (row != null && !row.Selected) e.Effect = DragDropEffects.Move;
        //}

        ///// <summary>
        ///// Handles the MouseClick event of the DataGridView control.
        ///// </summary>
        //private void DataGridView_MouseDown(object sender, MouseEventArgs e)
        //{
        //    var currentRow = this.HitTest(e.X, e.Y).RowIndex;

        //    if (!this.AllowDrop) return;

        //    if (e.Button == MouseButtons.Right && this.SelectedRows.Count > 0)
        //    {
        //        if (!(this.SelectedRows.Count == 1 && this.SelectedRows[0].Index == currentRow))
        //        {
        //            // invoke drag-drop on right click
        //            this.DoDragDrop(this, DragDropEffects.Move);
        //        }
        //    }
        //}

        ///// <summary>
        ///// Handles the MouseClick event of the DataGridView control.
        ///// </summary>
        //private void DataGridView_MouseUp(object sender, MouseEventArgs e)
        //{
        //    if (this.ContextMenuStrip == null) return;

        //    if (e.Button == MouseButtons.Right && this.SelectedRows.Count == 1)
        //    {
        //        this.ContextMenuStrip.Show(this, new Point(e.X, e.Y));
        //    }
        //}

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the sort order is changed.
        /// </summary>
        public event EventHandler SortOrderChanged;

        ///// <summary>
        ///// Row Drag Drop event handler delegate
        ///// </summary>
        //public delegate void RowDragDropHandler(object sender, RowDragDropEventArgs e);

        ///// <summary>
        ///// Row Drag Drop event arguments
        ///// </summary>
        //public class RowDragDropEventArgs : EventArgs
        //{
        //    public int DestinationRowIndex { get; internal set; }
        //}

        ///// <summary>
        ///// Occurs when rows are drag/dropped
        ///// </summary>
        //public event RowDragDropHandler RowDragDrop;

        #endregion
    }
}