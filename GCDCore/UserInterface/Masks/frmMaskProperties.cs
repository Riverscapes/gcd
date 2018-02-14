using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GCDCore.Project;

namespace GCDCore.UserInterface.Masks
{
    public partial class frmMaskProperties : Form
    {
        public readonly naru.ui.SortableBindingList<GCDCore.Project.Masks.MaskItem> MaskItems;
        public GCDCore.Project.Masks.RegularMask Mask { get; internal set; }

        public frmMaskProperties(GCDCore.Project.Masks.RegularMask mask = null)
        {
            InitializeComponent();
            MaskItems = new naru.ui.SortableBindingList<GCDCore.Project.Masks.MaskItem>();
            Mask = mask;
        }

        private void frmMaskProperties_Load(object sender, EventArgs e)
        {
            FileInfo fiShapeFile = null;

            if (Mask is GCDCore.Project.Masks.RegularMask)
            {
                txtName.Text = Mask.Name;
                txtPath.Text = ProjectManager.Project.GetRelativePath(Mask._ShapeFile);
                fiShapeFile = Mask._ShapeFile;

                ucPolygon.Initialize("Mask Polygon ShapeFile", fiShapeFile, true);
                ucPolygon.ReadOnly = true;

                cboField.DataSource = new BindingList<string>() { Mask._Field };
                cboField.SelectedIndex = 0;
                cboField.Enabled = false;

                Mask._Items.ForEach(x => MaskItems.Add(x));
                cmdOK.Text = "Update";

                grdData.Select();
            }


            // subscribe to the even when the user changes the input ShapeFile
            ucPolygon.PathChanged += InputShapeFileChanged;

            // Subscribe to name changing so that path can update
            // do this after any setting of name for existing mask
            txtName.TextChanged += txtName_TextChanged;

            // Subscribe to the field changing.
            // do this after any setting of shapefile for existing mask
            cboField.SelectedIndexChanged += cboField_SelectedIndexChanged;

            grdData.DataSource = MaskItems;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
                txtPath.Text = string.Empty;
            else
                txtPath.Text = GCDCore.Project.ProjectManager.Project.GetRelativePath(GCDCore.Project.ProjectManager.OutputManager.GetMaskShapeFilePath(txtName.Text));
        }

        private void InputShapeFileChanged(object sender, naru.ui.PathEventArgs e)
        {
            cboField.DataSource = null;

            if (ucPolygon.SelectedItem == null)
            {
                return;
            }

            Cursor = Cursors.WaitCursor;
            GCDConsoleLib.Vector shapeFile = ucPolygon.SelectedItem;

            // Use the ShapeFile file name if the user hasn't specified one yet
            if (string.IsNullOrEmpty(txtName.Text))
            {
                txtName.Text = naru.os.File.RemoveDangerousCharacters(System.IO.Path.GetFileNameWithoutExtension(shapeFile.GISFileInfo.FullName));
            }

            cboField.DataSource = shapeFile.Fields.Values.Where(x => x.Type.Equals(GCDConsoleLib.GDalFieldType.StringField)).ToList<GCDConsoleLib.VectorField>();
            Cursor = Cursors.Default;
        }

        private void cboField_SelectedIndexChanged(object sender, EventArgs e)
        {
            MaskItems.Clear();
            if (string.IsNullOrEmpty(cboField.Text))
                return;

            Cursor = Cursors.WaitCursor;

            foreach (KeyValuePair<long, GCDConsoleLib.VectorFeature> vFeature in ucPolygon.SelectedItem.Features)
            {
                string fieldValue = vFeature.Value.GetFieldAsString(cboField.Text);
                if (!MaskItems.Any<GCDCore.Project.Masks.MaskItem>(x => string.Compare(x.FieldValue, fieldValue, true) == 0))
                {
                    MaskItems.Add(new GCDCore.Project.Masks.MaskItem(true, fieldValue, fieldValue));
                }
            }
        }

        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                bool bIncude = ((System.Windows.Forms.ToolStripMenuItem)sender).Name.ToLower().Contains("all");
                MaskItems.ToList<GCDCore.Project.Masks.MaskItem>().ForEach(x => x.Include = bIncude);
                MaskItems.ResetBindings();
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        private bool ValidateForm()
        {
            // Sanity check to prevent empty names
            txtName.Text = txtName.Text.Trim();

            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("You must provide a name for the mask.", "Missing Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (!GCDCore.Project.ProjectManager.Project.IsMaskNameUnique(txtName.Text, Mask))
            {
                MessageBox.Show("This project already contains a mask with this name. Please choose a unique name.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Select();
                return false;
            }

            if (!(ucPolygon.SelectedItem is GCDConsoleLib.Vector))
            {
                MessageBox.Show("You must choose a mask ShapeFile to continue.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                ucPolygon.Select();
                return false;
            }

            // Should be safe after Validate call above
            GCDConsoleLib.Vector shp = ucPolygon.SelectedItem;

            if (shp.Features.Count < 1)
            {
                MessageBox.Show("The ShapeFile does not contain any features. You must choose a polygon ShapeFile with one or more feature.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                ucPolygon.Select();
                return false;
            }

            // Validate that hte user actually chose a POLYGON ShapeFile
            if (shp.GeometryType.SimpleType != GCDConsoleLib.GDalGeometryType.SimpleTypes.Polygon)
            {
                MessageBox.Show(string.Format("The selected ShapeFile appears to be of {0} geometry type. Only polygon ShapeFiles can be used as masks.", shp.GeometryType.SimpleType), "Invalid Geometry Type", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ucPolygon.Select();
                return false;
            }

            if (shp.Proj.PrettyWkt.ToLower().Contains("unknown"))
            {
                MessageBox.Show("The selected ShapeFile appears to be missing a spatial reference." +
                    " All GCD ShapeFiles must possess a spatial reference and it must be the same spatial reference for all rasters in a GCD project.", "Missing Spatial Reference", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ucPolygon.Select();
                return false;
            }

            if (ProjectManager.Project.DEMSurveys.Count > 0)
            {
                if (!ProjectManager.Project.DEMSurveys.Values.First<DEMSurvey>().Raster.Proj.IsSame(shp.Proj))
                {
                    string wkt = ProjectManager.Project.DEMSurveys.Values.First<DEMSurvey>().Raster.Proj.Wkt;

                    MessageBox.Show("The coordinate system of the selected ShapeFile:" + Environment.NewLine + Environment.NewLine + shp.Proj.PrettyWkt + Environment.NewLine + Environment.NewLine +
                       "does not match that of the GCD project:" + Environment.NewLine + Environment.NewLine + wkt + Environment.NewLine + Environment.NewLine +
                       "All ShapeFiles and rasters within a GCD project must have the identical coordinate system. However, small discrepencies in coordinate system names might cause the two coordinate systems to appear different. " +
                       "If you believe that the selected ShapeFile does in fact possess the same coordinate system as the GCD project then use the ArcToolbox 'Define Projection' geoprocessing tool in the " +
                       "'Data Management -> Projection & Transformations' Toolbox to correct the problem with the selected raster by defining the coordinate system as:"
                       + Environment.NewLine + Environment.NewLine + wkt + Environment.NewLine + Environment.NewLine + "Then try importing it into the GCD again.",
                       Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            if (string.IsNullOrEmpty(cboField.Text))
            {
                MessageBox.Show("You must select the field in the ShapeFile that identifies the mask values.", "Missing Mask Field", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboField.Select();
                return false;
            }

            if (MaskItems.Count < 1)
            {
                MessageBox.Show(string.Format("The ShapeFile selected does not contain any valid string values in the {0} field. Please choose a different field or ShapeFile.", cboField.Text), "No Field Values", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboField.Select();
                return false;
            }

            if (MaskItems.Count(x => !x.Include) < 1)
            {
                MessageBox.Show("You must check the box beside at least one field value.", "No Field Values Included", MessageBoxButtons.OK, MessageBoxIcon.Information);
                grdData.Select();
                return false;
            }

            return true;
        }

        private void grdData_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
            {
                MessageBox.Show("You must provide a non-empty label for every field value.", "Empty Label", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                DialogResult = DialogResult.None;
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;

                if (Mask == null)
                {
                    FileInfo fiMask = ProjectManager.Project.GetAbsolutePath(txtPath.Text);
                    fiMask.Directory.Create();

                    ucPolygon.SelectedItem.Copy(fiMask);

                    Mask = new GCDCore.Project.Masks.RegularMask(txtName.Text, fiMask, cboField.Text, MaskItems.ToList<GCDCore.Project.Masks.MaskItem>());
                    ProjectManager.Project.Masks[Mask.Name] = Mask;
                }
                else
                {
                    Mask.Name = txtName.Text;
                }

                ProjectManager.Project.Save();

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "Error creating regular mask.");
            }
        }
    }
}
