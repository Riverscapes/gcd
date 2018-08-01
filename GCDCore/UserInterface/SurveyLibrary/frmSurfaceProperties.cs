using System;
using System.Windows.Forms;
using GCDCore.Project;
using System.Collections.Generic;
using System.Drawing;

namespace GCDCore.UserInterface.SurveyLibrary
{
    public partial class frmSurfaceProperties : Form, IProjectItemForm
    {
        public readonly GCDProjectRasterItem Raster;
        private SurveyDateTime SurveyDate { get; set; }
        private readonly naru.ui.SortableBindingList<GridViewPropertyValueItem> ItemProperties;

        public GCDProjectItem GCDProjectItem { get { return Raster; } }

        public frmSurfaceProperties(GCDProjectRasterItem surface, bool editable)
        {
            InitializeComponent();
            ItemProperties = new naru.ui.SortableBindingList<GridViewPropertyValueItem>();
            Raster = surface;
            if (surface is DEMSurvey)
            {
                SurveyDate = ((DEMSurvey)surface).SurveyDate;
            }

            txtName.ReadOnly = !editable;
            cmdSurveyDate.Enabled = editable;
            cmdOK.Visible = editable;

            if (!editable)
                cmdCancel.Text = "Close";
        }

        private void frmDEMProperties_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Text = string.Format("{0} Properties", Raster.Noun);
            grpProperties.Text = string.Format("{0} Raster Properties", Raster.Noun);
            cmdOK.Text = Properties.Resources.UpdateButtonText;

            txtName.Text = Raster.Name;
            txtPath.Text = ProjectManager.Project.GetRelativePath(Raster.Raster.GISFileInfo);

            ItemProperties.Add(new GridViewPropertyValueItem("Raster Properties"));
            ItemProperties.Add(new GridViewPropertyValueItem("Cell size", UnitsNet.Length.From((double)Raster.Raster.Extent.CellWidth, Raster.Raster.Proj.HorizontalUnit).ToString()));
            ItemProperties.Add(new GridViewPropertyValueItem("Rows", Raster.Raster.Extent.Rows.ToString("N0")));
            ItemProperties.Add(new GridViewPropertyValueItem("Columns", Raster.Raster.Extent.Cols.ToString("N0")));
            ItemProperties.Add(new GridViewPropertyValueItem("Height", UnitsNet.Length.From((double)Raster.Raster.Extent.Height, Raster.Raster.Proj.HorizontalUnit).ToString()));
            ItemProperties.Add(new GridViewPropertyValueItem("Width", UnitsNet.Length.From((double)Raster.Raster.Extent.Width, Raster.Raster.Proj.HorizontalUnit).ToString()));
            ItemProperties.Add(new GridViewPropertyValueItem("Spatial Reference", Raster.Raster.Proj.Wkt));

            // Values from raster stats are optional
            try
            {
                Raster.Raster.ComputeStatistics();
                Dictionary<string, decimal> stats = Raster.Raster.GetStatistics();
                ItemProperties.Add(new GridViewPropertyValueItem("Raster Statistics"));
                ItemProperties.Add(new GridViewPropertyValueItem("Maximum raster value", stats["max"].ToString("n2")));
                ItemProperties.Add(new GridViewPropertyValueItem("Minimum raster value", stats["min"].ToString("n2")));
                ItemProperties.Add(new GridViewPropertyValueItem("Mean raster value", stats["mean"].ToString("n2")));
                ItemProperties.Add(new GridViewPropertyValueItem("Standard deviation of raster values", stats["stddev"].ToString("n2")));
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error calculating statistics from {0}, {1}", Raster.Raster.GISFileInfo.FullName, ex.Message));
            }

            if (Raster is DEMSurvey)
            {
                txtSurveyDate.Text = SurveyDate is SurveyDateTime ? SurveyDate.ToString() : SurveyDateTime.NotSetString;
            }
            else
            {
                lblSurveyDate.Visible = false;
                txtSurveyDate.Visible = false;
                cmdSurveyDate.Visible = false;
                grpProperties.Height += grpProperties.Top - txtSurveyDate.Top;
                grpProperties.Top = txtSurveyDate.Top;
            }

            if (!ProjectManager.IsArcMap)
            {
                cmdAddTopMap.Visible = false;
                txtPath.Width = cmdAddTopMap.Right - txtPath.Left;
            }

            grdData.AutoGenerateColumns = false;
            grdData.ColumnHeadersVisible = false;
            grdData.DataSource = ItemProperties;
            grdData.Select();

            tTip.SetToolTip(txtName, "The name used to refer to this item within the GCD project. It cannot be empty and it must be unique among all other items of this type.");
            tTip.SetToolTip(txtPath, "The relative file path of this raster within the GCD project.");
            tTip.SetToolTip(cmdAddTopMap, "Add the raster to the current map document.");
            tTip.SetToolTip(txtSurveyDate, "The date on which the DEM survey was collected.");
            tTip.SetToolTip(cmdSurveyDate, "Configure the date on which the DEM survey was collected.");
            tTip.SetToolTip(grdData, "Properties of the raster associated with this item.");

            Cursor = Cursors.Default;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                DialogResult = DialogResult.None;
                return;
            }

            // Detect if name has been changed - need to update owner dictionaries
            if (string.Compare(Raster.Name, txtName.Text, false) != 0)
            {
                Raster.Name = txtName.Text;                
            }

            if (Raster is DEMSurvey)
                ((DEMSurvey)Raster).SurveyDate = SurveyDate;

            ProjectManager.Project.Save();
        }

        private bool ValidateForm()
        {
            // Sanity check to prevent empty names
            txtName.Text = txtName.Text.Trim();

            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show(string.Format("You must provide a name for the {0}. The name cannot be blank.", Raster.Noun), "Missing Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Select();
                return false;
            }

            bool bUnique = true;
            if (Raster is DEMSurvey)
            {
                bUnique = ProjectManager.Project.IsDEMNameUnique(txtName.Text, (DEMSurvey)Raster);
            }
            else if (Raster is Surface)
            {
                bUnique = ProjectManager.Project.IsReferenceSurfaceNameUnique(txtName.Text, (Surface)Raster);
            }
            else if (Raster is ErrorSurface)
            {
                bUnique = ((ErrorSurface)Raster).Surf.IsErrorNameUnique(txtName.Text, (ErrorSurface)Raster);
            }
            else
            {
                throw new Exception("Unhandled surface type");
            }

            if (!bUnique)
            {
                MessageBox.Show(string.Format("A {0} with this name already exists within this project. Please enter a unique name for the {0}", Raster.Noun.ToLower()), "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Select();
                return false;
            }

            return true;
        }

        private void cmdAddTopMap_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectManager.OnAddRasterToMap(Raster);
            }
            catch (Exception ex)
            {
                GCDException.HandleException(ex);
            }
        }

        private void cmdSurveyDate_Click(object sender, EventArgs e)
        {
            frmSurveyDateTime frm = new frmSurveyDateTime(SurveyDate);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                SurveyDate = frm.SurveyDateTime;
                txtSurveyDate.Text = SurveyDate is SurveyDateTime ? SurveyDate.ToString() : SurveyDateTime.NotSetString;
            }
        }

        private void cmdHelp_Click(object sender, EventArgs e)
        {
            OnlineHelp.Show(Name);
        }

        private void grdData_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex > -1)
            {
                GridViewPropertyValueItem prop = (GridViewPropertyValueItem)grdData.Rows[e.RowIndex].DataBoundItem;

                if (prop.Header)
                {
                    grdData.Rows[e.RowIndex].Cells[0].Style.Font = new Font(grdData.Font, FontStyle.Bold);
                }
                else
                {
                    grdData.Rows[e.RowIndex].Cells[0].Style.Padding = new Padding(prop.LeftPadding, 0, 0, 0);
                }                
            }
        }
    }
}
