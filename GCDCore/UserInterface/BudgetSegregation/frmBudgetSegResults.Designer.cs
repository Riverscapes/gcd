using GCDCore.Project;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;

namespace GCDCore.UserInterface.BudgetSegregation
{
    partial class frmBudgetSegResults : System.Windows.Forms.Form
    {

        //Form overrides dispose to clean up the component list.
        [System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components != null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        //Required by the Windows Form Designer

        private System.ComponentModel.IContainer components;
        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.  
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBudgetSegResults));
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.TabPage1 = new System.Windows.Forms.TabPage();
            this.TabPage2 = new System.Windows.Forms.TabPage();
            this.TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.TabPage3 = new System.Windows.Forms.TabPage();
            this.grpBudgetSeg = new System.Windows.Forms.GroupBox();
            this.txtField = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.txtPolygonMask = new System.Windows.Forms.TextBox();
            this.cmsBasicRaster = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddToMapToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.Label3 = new System.Windows.Forms.Label();
            this.TabPage4 = new System.Windows.Forms.TabPage();
            this.Label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.cmdBrowse = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.cboBudgetClass = new System.Windows.Forms.ComboBox();
            this.cboRaw = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ucSummary = new GCDCore.UserInterface.ChangeDetection.ucDoDSummary();
            this.ucBars = new GCDCore.UserInterface.ChangeDetection.ucChangeBars();
            this.ucHistogram = new GCDCore.UserInterface.ChangeDetection.ucDoDHistogram();
            this.ucProperties = new GCDCore.UserInterface.ChangeDetection.ucDoDProperties();
            this.tabMain.SuspendLayout();
            this.TabPage1.SuspendLayout();
            this.TabPage2.SuspendLayout();
            this.TableLayoutPanel1.SuspendLayout();
            this.TabPage3.SuspendLayout();
            this.grpBudgetSeg.SuspendLayout();
            this.cmsBasicRaster.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(591, 475);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 8;
            this.cmdCancel.Text = "Close";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdHelp.Location = new System.Drawing.Point(12, 475);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 9;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            // 
            // tabMain
            // 
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabMain.Controls.Add(this.TabPage1);
            this.tabMain.Controls.Add(this.TabPage2);
            this.tabMain.Controls.Add(this.TabPage3);
            this.tabMain.Controls.Add(this.TabPage4);
            this.tabMain.Location = new System.Drawing.Point(4, 109);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(662, 360);
            this.tabMain.TabIndex = 7;
            // 
            // TabPage1
            // 
            this.TabPage1.Controls.Add(this.ucSummary);
            this.TabPage1.Location = new System.Drawing.Point(4, 22);
            this.TabPage1.Name = "TabPage1";
            this.TabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage1.Size = new System.Drawing.Size(654, 334);
            this.TabPage1.TabIndex = 0;
            this.TabPage1.Text = "Tabular Results By Category";
            this.TabPage1.UseVisualStyleBackColor = true;
            // 
            // TabPage2
            // 
            this.TabPage2.Controls.Add(this.TableLayoutPanel1);
            this.TabPage2.Location = new System.Drawing.Point(4, 22);
            this.TabPage2.Name = "TabPage2";
            this.TabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage2.Size = new System.Drawing.Size(654, 334);
            this.TabPage2.TabIndex = 1;
            this.TabPage2.Text = "Graphical Results By Category";
            this.TabPage2.UseVisualStyleBackColor = true;
            // 
            // TableLayoutPanel1
            // 
            this.TableLayoutPanel1.ColumnCount = 3;
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.TableLayoutPanel1.Controls.Add(this.ucBars, 2, 0);
            this.TableLayoutPanel1.Controls.Add(this.ucHistogram, 0, 0);
            this.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.TableLayoutPanel1.Name = "TableLayoutPanel1";
            this.TableLayoutPanel1.RowCount = 1;
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 376F));
            this.TableLayoutPanel1.Size = new System.Drawing.Size(648, 328);
            this.TableLayoutPanel1.TabIndex = 6;
            // 
            // TabPage3
            // 
            this.TabPage3.Controls.Add(this.grpBudgetSeg);
            this.TabPage3.Controls.Add(this.ucProperties);
            this.TabPage3.Location = new System.Drawing.Point(4, 22);
            this.TabPage3.Name = "TabPage3";
            this.TabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage3.Size = new System.Drawing.Size(654, 334);
            this.TabPage3.TabIndex = 2;
            this.TabPage3.Text = "Analysis Inputs";
            this.TabPage3.UseVisualStyleBackColor = true;
            // 
            // grpBudgetSeg
            // 
            this.grpBudgetSeg.Controls.Add(this.txtField);
            this.grpBudgetSeg.Controls.Add(this.Label4);
            this.grpBudgetSeg.Controls.Add(this.txtPolygonMask);
            this.grpBudgetSeg.Controls.Add(this.Label3);
            this.grpBudgetSeg.Location = new System.Drawing.Point(8, 200);
            this.grpBudgetSeg.Name = "grpBudgetSeg";
            this.grpBudgetSeg.Size = new System.Drawing.Size(640, 100);
            this.grpBudgetSeg.TabIndex = 1;
            this.grpBudgetSeg.TabStop = false;
            this.grpBudgetSeg.Text = "Budget Segregation";
            // 
            // txtField
            // 
            this.txtField.Location = new System.Drawing.Point(99, 59);
            this.txtField.Name = "txtField";
            this.txtField.ReadOnly = true;
            this.txtField.Size = new System.Drawing.Size(204, 20);
            this.txtField.TabIndex = 3;
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(10, 59);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(32, 13);
            this.Label4.TabIndex = 2;
            this.Label4.Text = "Field:";
            // 
            // txtPolygonMask
            // 
            this.txtPolygonMask.ContextMenuStrip = this.cmsBasicRaster;
            this.txtPolygonMask.Location = new System.Drawing.Point(99, 24);
            this.txtPolygonMask.Name = "txtPolygonMask";
            this.txtPolygonMask.ReadOnly = true;
            this.txtPolygonMask.Size = new System.Drawing.Size(535, 20);
            this.txtPolygonMask.TabIndex = 1;
            // 
            // cmsBasicRaster
            // 
            this.cmsBasicRaster.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddToMapToolStripMenuItem1});
            this.cmsBasicRaster.Name = "cmsBasicRaster";
            this.cmsBasicRaster.Size = new System.Drawing.Size(138, 26);
            // 
            // AddToMapToolStripMenuItem1
            // 
            this.AddToMapToolStripMenuItem1.Image = global::GCDCore.Properties.Resources.AddToMap;
            this.AddToMapToolStripMenuItem1.Name = "AddToMapToolStripMenuItem1";
            this.AddToMapToolStripMenuItem1.Size = new System.Drawing.Size(137, 22);
            this.AddToMapToolStripMenuItem1.Text = "Add to Map";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(10, 24);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(76, 13);
            this.Label3.TabIndex = 0;
            this.Label3.Text = "Polygon mask:";
            // 
            // TabPage4
            // 
            this.TabPage4.Location = new System.Drawing.Point(4, 22);
            this.TabPage4.Name = "TabPage4";
            this.TabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage4.Size = new System.Drawing.Size(654, 334);
            this.TabPage4.TabIndex = 3;
            this.TabPage4.Text = "Report";
            this.TabPage4.UseVisualStyleBackColor = true;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(50, 14);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(35, 13);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "Name";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(91, 10);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(546, 20);
            this.txtName.TabIndex = 1;
            // 
            // cmdBrowse
            // 
            this.cmdBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowse.Image = global::GCDCore.Properties.Resources.BrowseFolder;
            this.cmdBrowse.Location = new System.Drawing.Point(643, 9);
            this.cmdBrowse.Name = "cmdBrowse";
            this.cmdBrowse.Size = new System.Drawing.Size(23, 23);
            this.cmdBrowse.TabIndex = 2;
            this.cmdBrowse.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Budget class";
            // 
            // cboBudgetClass
            // 
            this.cboBudgetClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboBudgetClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBudgetClass.FormattingEnabled = true;
            this.cboBudgetClass.Location = new System.Drawing.Point(91, 39);
            this.cboBudgetClass.Name = "cboBudgetClass";
            this.cboBudgetClass.Size = new System.Drawing.Size(546, 21);
            this.cboBudgetClass.TabIndex = 4;
            this.cboBudgetClass.SelectedIndexChanged += new System.EventHandler(this.cboBudgetClass_SelectedIndexChanged);
            // 
            // cboRaw
            // 
            this.cboRaw.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboRaw.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRaw.FormattingEnabled = true;
            this.cboRaw.Location = new System.Drawing.Point(91, 69);
            this.cboRaw.Name = "cboRaw";
            this.cboRaw.Size = new System.Drawing.Size(546, 21);
            this.cboRaw.TabIndex = 6;
            this.cboRaw.SelectedIndexChanged += new System.EventHandler(this.cboBudgetClass_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Raw represents";
            // 
            // ucSummary
            // 
            this.ucSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSummary.Location = new System.Drawing.Point(3, 3);
            this.ucSummary.Name = "ucSummary";
            this.ucSummary.Size = new System.Drawing.Size(648, 328);
            this.ucSummary.TabIndex = 2;
            // 
            // ucBars
            // 
            this.ucBars.ChangeStats = null;
            this.ucBars.DisplayUnits = null;
            this.ucBars.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucBars.Location = new System.Drawing.Point(504, 3);
            this.ucBars.Name = "ucBars";
            this.ucBars.Size = new System.Drawing.Size(141, 370);
            this.ucBars.TabIndex = 5;
            // 
            // ucHistogram
            // 
            this.ucHistogram.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.TableLayoutPanel1.SetColumnSpan(this.ucHistogram, 2);
            this.ucHistogram.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucHistogram.Location = new System.Drawing.Point(3, 3);
            this.ucHistogram.Name = "ucHistogram";
            this.ucHistogram.Size = new System.Drawing.Size(495, 370);
            this.ucHistogram.TabIndex = 4;
            // 
            // ucProperties
            // 
            this.ucProperties.Location = new System.Drawing.Point(6, 6);
            this.ucProperties.Name = "ucProperties";
            this.ucProperties.Size = new System.Drawing.Size(642, 206);
            this.ucProperties.TabIndex = 0;
            // 
            // frmBudgetSegResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 510);
            this.Controls.Add(this.cboRaw);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboBudgetClass);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmdBrowse);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.cmdCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmBudgetSegResults";
            this.Text = "Budget Segregation Results";
            this.Load += new System.EventHandler(this.BudgetSegResultsForm_Load);
            this.tabMain.ResumeLayout(false);
            this.TabPage1.ResumeLayout(false);
            this.TabPage2.ResumeLayout(false);
            this.TableLayoutPanel1.ResumeLayout(false);
            this.TabPage3.ResumeLayout(false);
            this.grpBudgetSeg.ResumeLayout(false);
            this.grpBudgetSeg.PerformLayout();
            this.cmsBasicRaster.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        internal System.Windows.Forms.Button cmdCancel;
        internal System.Windows.Forms.Button cmdHelp;
        internal System.Windows.Forms.TabControl tabMain;
        internal System.Windows.Forms.TabPage TabPage1;
        internal System.Windows.Forms.TabPage TabPage2;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox txtName;
        internal ChangeDetection.ucDoDSummary ucSummary;
        internal System.Windows.Forms.TabPage TabPage3;
        internal System.Windows.Forms.TabPage TabPage4;
        internal System.Windows.Forms.GroupBox grpBudgetSeg;
        internal System.Windows.Forms.TextBox txtField;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.TextBox txtPolygonMask;
        internal System.Windows.Forms.Label Label3;
        internal ChangeDetection.ucDoDProperties ucProperties;
        internal ChangeDetection.ucDoDHistogram ucHistogram;
        internal ChangeDetection.ucChangeBars ucBars;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel1;
        internal System.Windows.Forms.Button cmdBrowse;
        internal System.Windows.Forms.ContextMenuStrip cmsBasicRaster;
        internal System.Windows.Forms.ToolStripMenuItem AddToMapToolStripMenuItem1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboBudgetClass;
        private System.Windows.Forms.ComboBox cboRaw;
        private System.Windows.Forms.Label label2;
    }
}
