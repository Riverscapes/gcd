using GCDCore.Project;
using System.Windows.Forms;
using System;
using System.Diagnostics;
using System.IO;

namespace GCDCore.UserInterface.SurveyLibrary
{
    public partial class frmAssocSurfaceProperties : Form, IProjectItemForm
    {
        public AssocSurface m_Assoc { get; internal set; }
        private DEMSurvey DEM;
        private frmImportRaster m_ImportForm;

        // Point density properties
        private GCDConsoleLib.Vector PointCloud;
        private GCDConsoleLib.RasterOperators.KernelShapes PointDensityShape;
        private decimal PointDensitySize;

        public GCDProjectItem GCDProjectItem { get { return m_Assoc; } }

        public frmAssocSurfaceProperties(DEMSurvey parentDEM, AssocSurface assoc)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            DEM = parentDEM;
            m_Assoc = assoc;

            // Initialize the point density values to sensible defaults
            PointDensityShape = GCDConsoleLib.RasterOperators.KernelShapes.Square;
            PointDensitySize = 4;
        }

        private void SurfacePropertiesForm_Load(Object sender, EventArgs e)
        {
            ttpTooltip.SetToolTip(btnCancel, "Cancel and close this form.");
            ttpTooltip.SetToolTip(btnHelp, string.Empty);
            ttpTooltip.SetToolTip(cboType, "Associated surface type. Use this to define what physical phenomenon this associated surface represents.");
            ttpTooltip.SetToolTip(btnBrowse, "Browse and choose an existing raster that represents the associated surface.");
            ttpTooltip.SetToolTip(btnSlopePercent, "Calculate a slope raster - in percent - from the DEM survey.");
            ttpTooltip.SetToolTip(btnSlopeDegree, "Calculate a slope raster - in degrees - from the DEM survey.");
            ttpTooltip.SetToolTip(btnDensity, "Calculate a point density raster from the DEM Survey. You will be presented with options for the point density calculation.");
            ttpTooltip.SetToolTip(btnRoughness, "Calculate a surface roughness raster from space delimited point cloud file.");

            cboType.DataSource = AssocSurface.GetAssocatedSurfaceTypes();
            cboType.Text = "Unknown";

            btnRoughness.Enabled = ProjectManager.IsArcMap;

            if (m_Assoc == null)
            {
                btnOK.Text = Properties.Resources.CreateButtonText;
            }
            else
            {
                btnOK.Text = Properties.Resources.UpdateButtonText;

                txtName.Text = m_Assoc.Name;
                txtProjectRaster.Text = ProjectManager.Project.GetRelativePath(m_Assoc.Raster.GISFileInfo);
                txtProjectRaster.ReadOnly = true;
                btnSlopePercent.Visible = false;
                btnDensity.Visible = false;
                btnBrowse.Visible = false;
                btnSlopeDegree.Visible = false;
                btnRoughness.Visible = false;
                txtOriginalRaster.Width = txtName.Width;

                SelectedAssociatedSurfaceType = m_Assoc.AssocSurfaceType;

                // Select the type combo to help the user quickly change the most likely thing they want to change
                cboType.Select();
            }
        }

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

        private void btnOK_Click(Object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                DialogResult = DialogResult.None;
                return;
            }

            Cursor = Cursors.WaitCursor;

            try
            {
                if (m_Assoc == null)
                {
                    if (!ImportRaster())
                    {
                        DialogResult = System.Windows.Forms.DialogResult.None;
                        return;
                    }

                    m_Assoc = new AssocSurface(txtName.Text.Trim(), ProjectManager.Project.GetAbsolutePath(txtProjectRaster.Text), DEM, SelectedAssociatedSurfaceType);
                    DEM.AssocSurfaces.Add(m_Assoc);
                }
                else
                {
                    m_Assoc.Name = txtName.Text.Trim();
                    m_Assoc.AssocSurfaceType = SelectedAssociatedSurfaceType;
                }

                ProjectManager.Project.Save();

            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "The associated surface failed to save to the GCD project file. The associated surface raster still exists.");
                DialogResult = System.Windows.Forms.DialogResult.None;
            }
            finally
            {
                Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private bool ImportRaster()
        {
            var bRasterImportSuccessful = false;
            FileInfo fiOutput = ProjectManager.Project.GetAbsolutePath(txtProjectRaster.Text);

            try
            {
                fiOutput.Directory.Create();

                switch (SelectedAssociatedSurfaceType)
                {
                    case AssocSurface.AssociatedSurfaceTypes.SlopeDegree:
                        GCDConsoleLib.RasterOperators.SlopeDegrees(DEM.Raster, fiOutput);
                        break;

                    case AssocSurface.AssociatedSurfaceTypes.SlopePercent:
                        GCDConsoleLib.RasterOperators.SlopePercent(DEM.Raster, fiOutput);
                        break;

                    case AssocSurface.AssociatedSurfaceTypes.PointDensity:
                        GCDConsoleLib.RasterOperators.PointDensity(DEM.Raster, PointCloud, new FileInfo(txtProjectRaster.Text), PointDensityShape, PointDensitySize);
                        break;

                    case AssocSurface.AssociatedSurfaceTypes.Roughness:
                        throw new NotImplementedException("Roughness raster is not implemented");
                        //m_SurfaceRoughnessForm.CalculateRoughness(txtProjectRaster.Text, gDEMRaster)
                        break;

                    default:
                        throw new NotImplementedException("Unhandled associated surface type");
                }

                // Build raster pyramids if they are needed
                ProjectManager.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.AssociatedSurfaces, fiOutput);

                bRasterImportSuccessful = true;
            }
            catch (Exception ex)
            {
                // Something went wrong. Check if the raster exists and safely attempt to clean it up if it does.
                if (fiOutput.Exists)
                {
                    try
                    {
                        GCDConsoleLib.Raster.Delete(fiOutput);
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine(string.Format("ERROR attempting to delete associated surface raster at {0} after an error during its creation\n\n{1}\n\n", fiOutput.FullName, ex.Message, ex2.Message));
                    }
                }
            }

            return bRasterImportSuccessful;
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
                if (!DEM.IsAssocNameUnique(txtName.Text, m_Assoc))
                {
                    MessageBox.Show("The name '" + txtName.Text + "' is already in use by another associated surface within this survey. Please choose a unique name.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtName.Select();
                    return false;
                }

            }

            if (string.IsNullOrEmpty(txtProjectRaster.Text))
            {
                MessageBox.Show("You must either browse and select an existing raster for this associated surface, or choose to generate a slope or point density raster from the DEM Survey raster.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else
            {
                if (m_Assoc == null && ProjectManager.Project.GetAbsolutePath(txtProjectRaster.Text).Exists)
                {
                    MessageBox.Show("The associated surface project raster path already exists. Changing the name of the associated surface will change the raster path.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            if (cboType.SelectedItem == null)
            {
                MessageBox.Show("Please select an associated surface type to continue.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboType.Focus();
                return false;
            }

            return true;
        }

        private void btnBrowse_Click(System.Object sender, System.EventArgs e)
        {
            if (m_ImportForm == null)
            {
                m_ImportForm = new frmImportRaster(DEM, ExtentImporter.Purposes.AssociatedSurface, "Associated Surface");
            }

            m_ImportForm.txtName.Text = txtName.Text;
            if (m_ImportForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtName.Text = m_ImportForm.txtName.Text;
                txtOriginalRaster.Text = m_ImportForm.ucRaster.SelectedItem.GISFileInfo.FullName;
            }

        }

        private void btnSlope_Click(System.Object sender, System.EventArgs e)
        {
            SlopeButtonClicked(AssocSurface.AssociatedSurfaceTypes.SlopePercent);
        }

        private void btnSlopeDegree_Click(System.Object sender, System.EventArgs e)
        {
            SlopeButtonClicked(AssocSurface.AssociatedSurfaceTypes.SlopeDegree);
        }

        private void SlopeButtonClicked(AssocSurface.AssociatedSurfaceTypes eType)
        {
            // Assign a name if the user hasn't already
            if (string.IsNullOrEmpty(txtName.Text))
            {
                txtName.Text = string.Format("Slope {0}", (eType == AssocSurface.AssociatedSurfaceTypes.SlopeDegree ? "Degrees" : "Percent"));
            }

            SelectedAssociatedSurfaceType = eType;

            txtOriginalRaster.Text = DEM.Raster.GISFileInfo.FullName;

            MessageBox.Show("The slope raster will be generated after you click OK.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnOK.Select();
        }

        private void btnDensity_Click(System.Object sender, System.EventArgs e)
        {
            frmPointDensity frm = new frmPointDensity(ProjectManager.Project.Units.VertUnit, PointDensityShape, PointDensitySize);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                SelectedAssociatedSurfaceType = AssocSurface.AssociatedSurfaceTypes.PointDensity;
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    txtName.Text = "PDensity";
                }
                PointDensityShape = frm.KernelShape;
                PointDensitySize = frm.KernerlSize;
                PointCloud = frm.PointCloud;

                txtOriginalRaster.Text = frm.ucPointCloud.SelectedItem.GISFileInfo.FullName;
            }
        }

        private void txtName_TextChanged(object sender, System.EventArgs e)
        {
            if (m_Assoc == null)
            {
                txtProjectRaster.Text = ProjectManager.Project.GetRelativePath(DEM.AssocSurfacePath(txtName.Text));
            }
        }

        private void btnRoughness_Click(System.Object sender, System.EventArgs e)
        {
            //If m_SurfaceRoughnessForm Is Nothing Then
            //    Dim dReferenceResolution As Double = Math.Abs(DEMSurveyRaster.Extent.CellWidth)
            //    m_SurfaceRoughnessForm = New frmSurfaceRoughness(dReferenceResolution)
            //End If

            //If m_SurfaceRoughnessForm.ShowDialog = System.Windows.Forms.DialogResult.OK Then
            //    If String.IsNullOrEmpty(txtName.Text) Then
            //        txtName.Text = "Roughness"
            //    End If
            //    m_eMethod = AssociatedSurfaceMethods.Roughness

            //    ' Select the appropriate type in the dropdown box
            //    '
            //    For i As Integer = 0 To cboType.Items.Count - 1
            //        If String.Compare(cboType.Items(i).ToString, "Roughness", True) = 0 Then
            //            cboType.SelectedIndex = i
            //            Exit For
            //        End If
            //    Next
            //    Try
            //        txtOriginalRaster.Text = m_SurfaceRoughnessForm.ucToPCAT_Inputs.txtBox_RawPointCloudFile.Text
            //    Catch ex As Exception
            //        naru.error.ExceptionUI.HandleException(ex)
            //    End Try
            //End If
        }

        private void btnHelp_Click(System.Object sender, System.EventArgs e)
        {
            Process.Start(GCDCore.Properties.Resources.HelpBaseURL + "gcd-command-reference/gcd-project-explorer/d-dem-context-menu/iv-add-associated-surface");
        }
    }
}
