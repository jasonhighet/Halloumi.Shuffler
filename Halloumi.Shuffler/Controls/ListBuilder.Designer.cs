namespace Halloumi.Shuffler.Controls
{
    partial class ListBuilder
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lstAvailableItems = new Halloumi.Common.Windows.Controls.ListView();
            this.colSource = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lstSelectedItems = new Halloumi.Common.Windows.Controls.ListView();
            this.colDestination = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAdd = new Halloumi.Common.Windows.Controls.Button();
            this.btnInsert = new Halloumi.Common.Windows.Controls.Button();
            this.btnRemove = new Halloumi.Common.Windows.Controls.Button();
            this.btnClear = new Halloumi.Common.Windows.Controls.Button();
            this.btnProperties = new Halloumi.Common.Windows.Controls.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lstAvailableItems, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lstSelectedItems, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(667, 413);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lstAvailableItems
            // 
            this.lstAvailableItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSource});
            this.lstAvailableItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstAvailableItems.FullRowSelect = true;
            this.lstAvailableItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstAvailableItems.Location = new System.Drawing.Point(3, 3);
            this.lstAvailableItems.Name = "lstAvailableItems";
            this.lstAvailableItems.Size = new System.Drawing.Size(277, 407);
            this.lstAvailableItems.TabIndex = 1;
            this.lstAvailableItems.UseCompatibleStateImageBehavior = false;
            this.lstAvailableItems.View = System.Windows.Forms.View.Details;
            // 
            // colSource
            // 
            this.colSource.Text = "Source";
            // 
            // lstSelectedItems
            // 
            this.lstSelectedItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDestination});
            this.lstSelectedItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSelectedItems.FullRowSelect = true;
            this.lstSelectedItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstSelectedItems.Location = new System.Drawing.Point(386, 3);
            this.lstSelectedItems.MultiSelect = false;
            this.lstSelectedItems.Name = "lstSelectedItems";
            this.lstSelectedItems.Size = new System.Drawing.Size(278, 407);
            this.lstSelectedItems.TabIndex = 0;
            this.lstSelectedItems.UseCompatibleStateImageBehavior = false;
            this.lstSelectedItems.View = System.Windows.Forms.View.Details;
            // 
            // colDestination
            // 
            this.colDestination.Text = "Destination";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnAdd);
            this.flowLayoutPanel1.Controls.Add(this.btnInsert);
            this.flowLayoutPanel1.Controls.Add(this.btnRemove);
            this.flowLayoutPanel1.Controls.Add(this.btnClear);
            this.flowLayoutPanel1.Controls.Add(this.btnProperties);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(286, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(94, 407);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(4, 4);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(85, 31);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.Location = new System.Drawing.Point(4, 43);
            this.btnInsert.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(85, 31);
            this.btnInsert.TabIndex = 5;
            this.btnInsert.Text = "Insert";
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(4, 82);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(85, 31);
            this.btnRemove.TabIndex = 1;
            this.btnRemove.Text = "Remove";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(4, 121);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(85, 31);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnProperties
            // 
            this.btnProperties.Location = new System.Drawing.Point(4, 160);
            this.btnProperties.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(85, 31);
            this.btnProperties.TabIndex = 6;
            this.btnProperties.Text = "Properties";
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
            // 
            // ListBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ListBuilder";
            this.Size = new System.Drawing.Size(667, 413);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Common.Windows.Controls.ListView lstAvailableItems;
        private Common.Windows.Controls.ListView lstSelectedItems;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Common.Windows.Controls.Button btnAdd;
        private Common.Windows.Controls.Button btnRemove;
        private Common.Windows.Controls.Button btnClear;
        private Common.Windows.Controls.Button btnInsert;
        private System.Windows.Forms.ColumnHeader colSource;
        private System.Windows.Forms.ColumnHeader colDestination;
        private Common.Windows.Controls.Button btnProperties;
    }
}
