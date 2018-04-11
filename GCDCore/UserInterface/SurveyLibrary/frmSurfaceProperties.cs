using System;
using System.Windows.Forms;
using GCDCore.Project;
using System.Collections.Generic;

namespace GCDCore.UserInterface.SurveyLibrary
{
    public partial class frmSurfaceProperties : Form, IProjectItemForm
    {
        public readonly GCDProjectRasterItem Surface;
        private SurveyDateTime SurveyDate { get; set; }
        private readonly naru.ui.SortableBindingList<ItemProperty> ItemProperties;

        public GCDProjectItem GCDProjectItem { get { return Surface; } }

        private string Noun
        {
            get
            {
                if (Surface is DEMSurvey)
                    return "DEM Survey";
                else if (Surface is ErrorSurface)
                    return "Reference Error Surface";
                else if (Surface is Surface)
                    return "Reference Surface";
                else
                    return string.Empty;
            }
        }

        public frmSurfaceProperties(GCDProjectRasterItem surface)
        {
            InitializeComponent();
            ItemProperties = new naru.ui.SortableBindingList<ItemProperty>();
            Surface = surface;
            if (surface is DEMSurvey)
            {
                SurveyDate = ((DEMSurvey)surface).SurveyDate;
            }
        }

        private void frmDEMProperties_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Text = string.Format("{0} Properties", Noun);
            grpProperties.Text = string.Format("{0} Raster Properties", Noun);
            cmdOK.Text = Properties.Resources.UpdateButtonText;

            txtName.Text = Surface.Name;
            txtPath.Text = ProjectManager.Project.GetRelativePath(Surface.Raster.GISFileInfo);

            ItemProperties.Add(new ItemProperty("Cell size", UnitsNet.Length.From((double)Surface.Raster.Extent.CellWidth, Surface.Raster.Proj.HorizontalUnit).ToString()));
            ItemProperties.Add(new ItemProperty("Rows", Surface.Raster.Extent.Rows.ToString("N0")));
            ItemProperties.Add(new ItemProperty("Columns", Surface.Raster.Extent.Cols.ToString("N0")));
            ItemProperties.Add(new ItemProperty("Height", UnitsNet.Length.From((double)Surface.Raster.Extent.Height, Surface.Raster.Proj.HorizontalUnit).ToString()));
            ItemProperties.Add(new ItemProperty("Width", UnitsNet.Length.From((double)Surface.Raster.Extent.Width, Surface.Raster.Proj.HorizontalUnit).ToString()));
            ItemProperties.Add(new ItemProperty("Spatial Reference", Surface.Raster.Proj.Wkt));

            // Values from raster stats are optional
            try
            {
                Surface.Raster.ComputeStatistics();
                Dictionary<string, decimal> stats = Surface.Raster.GetStatistics();
                ItemProperties.Add(new ItemProperty("Maximum raster value", stats["max"].ToString("n2")));
                ItemProperties.Add(new ItemProperty("Minimum raster value", stats["min"].ToString("n2")));
                ItemProperties.Add(new ItemProperty("Mean raster value", stats["mean"].ToString("n2")));
                ItemProperties.Add(new ItemProperty("Standard deviation of raster values", stats["stddev"].ToString("n2")));
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error calculating statistics from {0}, {1}", Surface.Raster.GISFileInfo.FullName, ex.Message));
            }

            if (Surface is DEMSurvey)
            {
                txtSurveyDate.Text = SurveyDate is SurveyDateTime ? SurveyDate.ToString() : SurveyDateTime.NotSetString;
            }
            else
            {
                lblSurveyDate.Visible = false;
                txtSurveyDate.Visible = false;
                cmdSurveyDate.Visible = false;
                grpProperties.Top = txtSurveyDate.Top;
            }

            if (!ProjectManager.IsArcMap)
            {
                cmdAddTopMap.Visible = false;
                txtPath.Width = cmdAddTopMap.Right - txtPath.Left;
            }

            grdData.DataSource = ItemProperties;
            grdData.Select();

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
            if (string.Compare(Surface.Name, txtName.Text, false) != 0)
            {
                if (Surface is DEMSurvey)
                {
                    ProjectManager.Project.DEMSurveys.Remove(Surface.Name);
                    Surface.Name = txtName.Text;
                    ProjectManager.Project.DEMSurveys[Surface.Name] = Surface as DEMSurvey;
                }
                else if (Surface is Surface)
                {
                    ProjectManager.Project.ReferenceSurfaces.Remove(Surface.Name);
                    Surface.Name = txtName.Text;
                    ProjectManager.Project.ReferenceSurfaces[Surface.Name] = Surface as Surface;
                }
                else if (Surface is ErrorSurface)
                {
                    ErrorSurface err = Surface as ErrorSurface;
                    err.Surf.ErrorSurfaces.Remove(err);
                    err.Name = txtName.Text;
                    err.Surf.ErrorSurfaces.Add(err);
                }
            }

            if (Surface is DEMSurvey)
                ((DEMSurvey)Surface).SurveyDate = SurveyDate;

            ProjectManager.Project.Save();
        }

        private bool ValidateForm()
        {
            // Sanity check to prevent empty names
            txtName.Text = txtName.Text.Trim();

            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show(string.Format("You must provide a name for the {0}. The name cannot be blank.", Noun), "Missing Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Select();
                return false;
            }

            bool bUnique = true;
            if (Surface is DEMSurvey)
            {
                bUnique = ProjectManager.Project.IsDEMNameUnique(txtName.Text, (DEMSurvey)Surface);
            }
            else if (Surface is Surface)
            {
                bUnique = ProjectManager.Project.IsReferenceSurfaceNameUnique(txtName.Text, (Surface)Surface);
            }
            else if (Surface is ErrorSurface)
            {
                bUnique = ((ErrorSurface)Surface).Surf.IsErrorNameUnique(txtName.Text, (ErrorSurface)Surface);
            }
            else
            {
                throw new Exception("Unhandled surface type");
            }

            if (!bUnique)
            {
                MessageBox.Show(string.Format("A {0} with this name already exists within this project. Please enter a unique name for the {0}", Noun.ToLower()), "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Select();
                return false;
            }

            return true;
        }

        private void cmdAddTopMap_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectManager.OnAddRasterToMap(Surface);
            }
            catch (Exception ex)
            {
                GCDException.HandleException(ex);
            }
        }

        private class ItemProperty
        {
            public string Property { get; internal set; }
            public string Value { get; internal set; }

            public ItemProperty(string property, string value)
            {
                Property = property;
                Value = value;
            }

            /// <summary>
            /// Default constructor for binding
            /// </summary>
            public ItemProperty()
            {

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
    }
}
