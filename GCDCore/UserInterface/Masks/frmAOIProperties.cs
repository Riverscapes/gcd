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
    public partial class frmAOIProperties : Form
    {
        public GCDCore.Project.Masks.AOIMask AOIMask { get; internal set; }

        public frmAOIProperties(GCDCore.Project.Masks.AOIMask aoi)
        {
            InitializeComponent();
            AOIMask = aoi;
        }

        private void frmAOIProperties_Load(object sender, EventArgs e)
        {
            if (AOIMask is GCDCore.Project.Masks.AOIMask)
            {
                txtName.Text = AOIMask.Name;
                txtPath.Text = ProjectManager.Project.GetRelativePath(AOIMask._ShapeFile);

                grpShapeFile.Visible = false;
                Height -= (grpShapeFile.Height + grpShapeFile.Top - txtPath.Bottom);

                if (!ProjectManager.IsArcMap)
                {
                    cmdAddToMap.Visible = false;
                    txtPath.Width = cmdAddToMap.Right - txtPath.Left;
                }
            }
            else
            {
                cmdAddToMap.Visible = false;
                txtPath.Width = cmdAddToMap.Right - txtPath.Left;
                ucPolygon.PathChanged += InputShapeFileChanged;
                ucPolygon.BrowseVector += ProjectManager.OnBrowseVector;
                ucPolygon.SelectVector += ProjectManager.OnSelectVector;
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
                naru.error.ExceptionUI.HandleException(ex, "Error saving area of interest mask.");
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
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            // Don't generate new paths for existing AOI
            if (AOIMask != null)
                return;

            if (string.IsNullOrEmpty(txtName.Text))
                txtPath.Text = string.Empty;
            else
                txtPath.Text = ProjectManager.Project.GetRelativePath(ProjectManager.OutputManager.GetMaskShapeFilePath(txtName.Text));
        }

        private void cmdAddToMap_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectManager.OnAddVectorToMap(AOIMask);
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "Error adding area of interest to the map.");
            }
        }
    }
}
