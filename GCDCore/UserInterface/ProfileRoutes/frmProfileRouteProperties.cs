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

namespace GCDCore.UserInterface.ProfileRoutes
{
    public partial class frmProfileRouteProperties : Form
    {
        public GCDCore.Project.ProfileRoutes.ProfileRoute ProfileRoute { get; internal set; }

        public frmProfileRouteProperties(GCDCore.Project.ProfileRoutes.ProfileRoute route)
        {
            InitializeComponent();
            ProfileRoute = route;
        }

        private void frmProfileRouteProperties_Load(object sender, EventArgs e)
        {
            if (ProfileRoute != null)
            {
                txtName.Text = ProfileRoute.Name;

                ucPolyline.Visible = false;
                lblPolylines.Visible = false;

                Height -= cboDistance.Top - ucPolyline.Top;
            }

            // subscribe to the even when the user changes the input ShapeFile
            ucPolyline.PathChanged += InputShapeFileChanged;
            UpdateControls(sender, e);

            // The singleton project manager subscribes to the browse raster event
            // So that the browse can bubble to ArcMap
            if (ProjectManager.IsArcMap)
            {
                ucPolyline.Initialize("Profile Route ShapeFile", GCDConsoleLib.GDalGeometryType.SimpleTypes.LineString);
                ucPolyline.BrowseVector += ProjectManager.OnBrowseVector;
                ucPolyline.SelectVector += ProjectManager.OnSelectVector;
            }
        }

        private void UpdateControls(object sender, EventArgs e)
        {
            cboLabel.Enabled = chkLabel.Checked;
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

                if (ProfileRoute == null)
                {
                    FileInfo fiMask = ProjectManager.Project.GetAbsolutePath(txtPath.Text);
                    fiMask.Directory.Create();

                    ucPolyline.SelectedItem.Copy(fiMask);

                    string lablField = chkLabel.Checked ? cboLabel.Text : string.Empty;
                    
                    ProfileRoute = new GCDCore.Project.ProfileRoutes.ProfileRoute(txtName.Text, fiMask, cboDistance.Text, lablField);
                    ProjectManager.Project.ProfileRoutes[ProfileRoute.Name] = ProfileRoute;
                }
                else
                {
                    ProfileRoute.Name = txtName.Text;
                }

                ProjectManager.Project.Save();

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "Error creating regular mask.");
            }
        }

        private bool ValidateForm()
        {
            // Sanity check to prevent empty names
            txtName.Text = txtName.Text.Trim();

            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("You must provide a name for the profile route.", "Missing Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (!ProjectManager.Project.IsProfileRouteNameUnique(txtName.Text, ProfileRoute))
            {
                MessageBox.Show("This project already contains a mask with this name. Please choose a unique name.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Select();
                return false;
            }

            if (ProfileRoute == null)
            {
                if (!(ucPolyline.SelectedItem is GCDConsoleLib.Vector))
                {
                    MessageBox.Show("You must choose a ShapeFile to continue.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ucPolyline.Select();
                    return false;
                }

                // Should be safe after Validate call above
                GCDConsoleLib.Vector shp = ucPolyline.SelectedItem;

                // Validate that hte user actually chose a POLYGON ShapeFile
                if (shp.GeometryType.SimpleType != GCDConsoleLib.GDalGeometryType.SimpleTypes.LineString)
                {
                    MessageBox.Show(string.Format("The selected ShapeFile appears to be of {0} geometry type. Only polyline ShapeFiles can be used as profile routes.", shp.GeometryType.SimpleType), "Invalid Geometry Type", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ucPolyline.Select();
                    return false;
                }

                if (!UserInterface.SurveyLibrary.GISDatasetValidation.ValidateVector(ucPolyline.SelectedItem))
                    return false;
            }

            if (cboDistance.SelectedIndex < 0)
            {
                MessageBox.Show("You must select a floating point field in the ShapeFile that provides distance values or uncheck the distance checkbox.", "Missing Distance Field", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboDistance.Select();
                return false;
            }

            if (chkLabel.Checked)
            {
                if (cboLabel.SelectedIndex < 0)
                {
                    MessageBox.Show("You must select a text field in the ShapeFile that provides profile route labels or uncheck the label checkbox.", "Missing Label Field", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboLabel.Select();
                    return false;
                }
            }

            if (ucPolyline == null)
            {
                if (ucPolyline.SelectedItem.Features.Values.Any(x => x.IsNull(cboDistance.Text)))
                {
                    MessageBox.Show(string.Format("One or more features in the ShapeFile have null or invalid values in the {0} field. A valid distance field must possess valid floating point values for all features.", cboDistance.Text), "Invalid Distance Values", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            return true;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
                txtPath.Text = string.Empty;
            else
                txtPath.Text = ProjectManager.Project.GetRelativePath(ProjectManager.OutputManager.GetProfilerouteFilePath(txtName.Text));
        }

        private void InputShapeFileChanged(object sender, naru.ui.PathEventArgs e)
        {
            if (ucPolyline.SelectedItem == null)
            {
                return;
            }

            Cursor = Cursors.WaitCursor;
            GCDConsoleLib.Vector shapeFile = ucPolyline.SelectedItem;

            // Use the ShapeFile file name if the user hasn't specified one yet
            if (string.IsNullOrEmpty(txtName.Text))
            {
                txtName.Text = naru.os.File.RemoveDangerousCharacters(System.IO.Path.GetFileNameWithoutExtension(shapeFile.GISFileInfo.FullName));
            }

            cboLabel.DataSource = shapeFile.Fields.Values.Where(x => x.Type.Equals(GCDConsoleLib.GDalFieldType.StringField)).ToList<GCDConsoleLib.VectorField>();
            cboDistance.DataSource = shapeFile.Fields.Values.Where(x => x.Type.Equals(GCDConsoleLib.GDalFieldType.RealField)).ToList<GCDConsoleLib.VectorField>();
            Cursor = Cursors.Default;
        }
    }
}
