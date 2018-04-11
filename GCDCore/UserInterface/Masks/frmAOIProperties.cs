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
    public partial class frmAOIProperties : Form, IProjectItemForm
    {
        public GCDCore.Project.Masks.AOIMask AOIMask { get; internal set; }

        public GCDProjectItem GCDProjectItem { get { return AOIMask; } }

        public frmAOIProperties(GCDCore.Project.Masks.AOIMask aoi)
        {
            InitializeComponent();
            AOIMask = aoi;
        }

        private void frmAOIProperties_Load(object sender, EventArgs e)
        {
            ucPolygon.PathChanged += InputShapeFileChanged;

            if (AOIMask is GCDCore.Project.Masks.AOIMask)
            {
                cmdOK.Text = Properties.Resources.UpdateButtonText;

                txtName.Text = AOIMask.Name;
                ucPolygon.InitializeExisting("AOI Mask", AOIMask.Vector);
                ucPolygon.AddToMap += cmdAddToMap_Click;

                lblPath.Visible = false;
                txtPath.Visible = false;
                Height -= (grpShapeFile.Top - txtPath.Top);
            }
            else
            {
                cmdOK.Text = Properties.Resources.CreateButtonText;
                ucPolygon.InitializeBrowseNew("AOI Mask", GCDConsoleLib.GDalGeometryType.SimpleTypes.Polygon);
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

                if (AOIMask == null)
                {
                    FileInfo fiDestination = ProjectManager.Project.GetAbsolutePath(txtPath.Text);
                    fiDestination.Directory.Create();
                    ucPolygon.SelectedItem.Copy(fiDestination);

                    AOIMask = new GCDCore.Project.Masks.AOIMask(txtName.Text, fiDestination);
                    ProjectManager.Project.Masks[AOIMask.Name] = AOIMask;
                    ProjectManager.AddNewProjectItemToMap(AOIMask);
                }
                else
                {
                    AOIMask.Name = txtName.Text;
                }

                ProjectManager.Project.Save();
                Cursor = Cursors.Default;
                MessageBox.Show("Area of interest mask saved successfully.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                DialogResult = DialogResult.None;
                GCDException.HandleException(ex, "Error saving area of interest mask.");
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private bool ValidateForm()
        {
            if (!MaskValidation.ValidateMaskName(txtName, AOIMask))
                return false;

            if (AOIMask == null)
            {
                if (!MaskValidation.ValidateShapeFile(ucPolygon))
                    return false;
            }

            return true;
        }

        private void InputShapeFileChanged(object sender, naru.ui.PathEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            GCDConsoleLib.Vector shapeFile = ucPolygon.SelectedItem;

            // Use the ShapeFile file name if the user hasn't specified one yet
            if (string.IsNullOrEmpty(txtName.Text))
            {
                txtName.Text = Path.GetFileNameWithoutExtension(shapeFile.GISFileInfo.FullName);
            }

            Cursor = Cursors.Default;
            cmdOK.Select();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            // Don't generate new paths for existing AOI
            if (AOIMask != null)
                return;

            if (string.IsNullOrEmpty(txtName.Text))
                txtPath.Text = string.Empty;
            else
                txtPath.Text = ProjectManager.Project.GetRelativePath(ProjectManager.Project.MaskPath(txtName.Text));
        }

        private void cmdAddToMap_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectManager.OnAddVectorToMap(AOIMask);
            }
            catch (Exception ex)
            {
                GCDException.HandleException(ex, "Error adding area of interest to the map.");
            }
        }
    }
}
