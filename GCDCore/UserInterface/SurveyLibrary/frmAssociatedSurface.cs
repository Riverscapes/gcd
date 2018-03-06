using System;
using System.Windows.Forms;
using GCDCore.Project;
using System.IO;

namespace GCDCore.UserInterface.SurveyLibrary
{
    public partial class frmAssociatedSurface : Form, IProjectItemForm
    {
        public readonly DEMSurvey DEM;
        public AssocSurface Assoc { get; internal set; }
        public GCDProjectItem GCDProjectItem { get { return Assoc as GCDProjectItem; } }
        public AssocSurface.AssociatedSurfaceTypes AssocType { get; internal set; }

        /// <summary>
        /// Get and set the combo box that displays the associated surface type
        /// </summary>
        private AssocSurface.AssociatedSurfaceTypes SelectedAssociatedSurfaceType
        {
            get
            {
                AssocSurface.AssociatedSurfaceTypes eType = AssocSurface.AssociatedSurfaceTypes.Other;
                if (cboType.SelectedItem is naru.db.NamedObject)
                {
                    eType = (AssocSurface.AssociatedSurfaceTypes)(((naru.db.NamedObject)cboType.SelectedItem).ID);
                }
                return eType;
            }
            set
            {
                foreach (naru.db.NamedObject item in cboType.Items)
                {
                    if (((long)item.ID) == ((long)value))
                    {
                        cboType.SelectedItem = item;
                        return;
                    }
                }
            }
        }

        public frmAssociatedSurface(DEMSurvey dem, AssocSurface assoc)
        {
            InitializeComponent();
            DEM = dem;
            Assoc = assoc;
            AssocType = assoc.AssocSurfaceType;
        }

        public frmAssociatedSurface(DEMSurvey dem, AssocSurface.AssociatedSurfaceTypes eType)
        {
            InitializeComponent();
            DEM = dem;
            AssocType = eType;
        }

        private void frmAssociatedSurface_Load(object sender, EventArgs e)
        {
            cboType.DataSource = AssocSurface.GetAssocatedSurfaceTypes();
            SelectedAssociatedSurfaceType = AssocType;

            if (Assoc == null)
            {
                cmdOK.Text = Properties.Resources.CreateButtonText;
                ucRaster.Visible = false;
                txtPath.Width = ucRaster.Right - txtPath.Left;
                cboType.Enabled = false;
                this.txtName.TextChanged += txtName_TextChanged;
            }
            else
            {
                txtName.Text = Assoc.Name;
                txtPath.Text = ProjectManager.Project.GetRelativePath(Assoc.Raster.GISFileInfo);
                cmdOK.Text = Properties.Resources.UpdateButtonText;
                txtPath.Visible = false;
                ucRaster.Width = ucRaster.Right - txtPath.Left;
                ucRaster.Left = txtPath.Left;
                ucRaster.InitializeExisting("Associated Surface", Assoc.Raster);
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

                if (Assoc == null)
                {
                    FileInfo fiOutput = ProjectManager.Project.GetAbsolutePath(txtPath.Text);
                    fiOutput.Directory.Create();

                    switch (SelectedAssociatedSurfaceType)
                    {
                        case AssocSurface.AssociatedSurfaceTypes.SlopeDegree:
                            GCDConsoleLib.RasterOperators.SlopeDegrees(DEM.Raster, fiOutput);
                            break;

                        case AssocSurface.AssociatedSurfaceTypes.SlopePercent:
                            GCDConsoleLib.RasterOperators.SlopePercent(DEM.Raster, fiOutput);

                            break;

                        default:
                            // This form is not capable of creating other associated surface types
                            throw new NotImplementedException(string.Format("The associated surface form is not capable of creating {0} rasters", SelectedAssociatedSurfaceType));
                    }

                    Assoc = new AssocSurface(txtName.Text, fiOutput, DEM, SelectedAssociatedSurfaceType);
                    DEM.AssocSurfaces.Add(Assoc);
                }
                else
                {
                    Assoc.Name = txtName.Text;
                    Assoc.AssocSurfaceType = SelectedAssociatedSurfaceType;
                }

                ProjectManager.Project.Save();
                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "Error generating associated surface raster.");
                DialogResult = DialogResult.None;
            }
        }

        private bool ValidateForm()
        {
            // Safety check against names with only blank spaces
            txtName.Text = txtName.Text.Trim();

            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Please provide a name for the associated surface.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Focus();
                return false;
            }
            else
            {
                if (!DEM.IsAssocNameUnique(txtName.Text, Assoc))
                {
                    MessageBox.Show("The name '" + txtName.Text + "' is already in use by another associated surface within this survey. Please choose a unique name.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtName.Select();
                    return false;
                }
            }

            return true;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            txtPath.Text = ProjectManager.Project.GetRelativePath(DEM.AssocSurfacePath(txtName.Text));
        }
    }
}
