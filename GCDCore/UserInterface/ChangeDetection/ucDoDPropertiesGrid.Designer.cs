namespace GCDCore.UserInterface.ChangeDetection
{
    partial class ucDoDPropertiesGrid
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
            this.components = new System.ComponentModel.Container();
            this.grdData = new System.Windows.Forms.DataGridView();
            this.colProperty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rasterPropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdData
            // 
            this.grdData.AllowUserToAddRows = false;
            this.grdData.AllowUserToDeleteRows = false;
            this.grdData.AllowUserToResizeRows = false;
            this.grdData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colProperty,
            this.colValue});
            this.grdData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdData.Location = new System.Drawing.Point(0, 0);
            this.grdData.MultiSelect = false;
            this.grdData.Name = "grdData";
            this.grdData.ReadOnly = true;
            this.grdData.RowHeadersVisible = false;
            this.grdData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdData.Size = new System.Drawing.Size(522, 453);
            this.grdData.TabIndex = 0;
            this.grdData.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grdData_CellFormatting);
            this.grdData.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdData_CellMouseDown);
            // 
            // colProperty
            // 
            this.colProperty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colProperty.DataPropertyName = "Property";
            this.colProperty.HeaderText = "Property";
            this.colProperty.Name = "colProperty";
            this.colProperty.ReadOnly = true;
            // 
            // colValue
            // 
            this.colValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colValue.DataPropertyName = "Value";
            this.colValue.HeaderText = "Value";
            this.colValue.Name = "colValue";
            this.colValue.ReadOnly = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToMapToolStripMenuItem,
            this.rasterPropertiesToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(163, 70);
            // 
            // addToMapToolStripMenuItem
            // 
            this.addToMapToolStripMenuItem.Image = global::GCDCore.Properties.Resources.AddToMap;
            this.addToMapToolStripMenuItem.Name = "addToMapToolStripMenuItem";
            this.addToMapToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.addToMapToolStripMenuItem.Text = "Add To Map";
            this.addToMapToolStripMenuItem.Click += new System.EventHandler(this.addToMapToolStripMenuItem_Click);
            // 
            // rasterPropertiesToolStripMenuItem
            // 
            this.rasterPropertiesToolStripMenuItem.Image = global::GCDCore.Properties.Resources.Settings;
            this.rasterPropertiesToolStripMenuItem.Name = "rasterPropertiesToolStripMenuItem";
            this.rasterPropertiesToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.rasterPropertiesToolStripMenuItem.Text = "Raster Properties";
            this.rasterPropertiesToolStripMenuItem.Click += new System.EventHandler(this.rasterPropertiesToolStripMenuItem_Click);
            // 
            // ucDoDPropertiesGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grdData);
            this.Name = "ucDoDPropertiesGrid";
            this.Size = new System.Drawing.Size(522, 453);
            this.Load += new System.EventHandler(this.ucDoDPropertiesGrid_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdData;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProperty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addToMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rasterPropertiesToolStripMenuItem;
    }
}
