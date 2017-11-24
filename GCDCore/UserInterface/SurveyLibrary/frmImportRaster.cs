using GCDCore.Project;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace GCDCore.UserInterface.SurveyLibrary
{

    public partial class frmImportRaster
    {

        public enum ImportRasterPurposes
        {
            DEMSurvey,
            AssociatedSurface,
            ErrorCalculation,
            StandaloneTool
        }

        private enum RoundingDirection
        {
            Up,
            Down
        }

        private enum ResamplingMethods
        {
            Bilinear,
            Cubic,
            NaturalNeighbours,
            NearestNeighbour,
            None
        }

        private GCDConsoleLib.Raster m_gReferenceRaster;
        private ImportRasterPurposes m_ePurpose;
        private DEMSurvey m_DEM;
        private GCDConsoleLib.ExtentRectangle m_OriginalExtent;
        // not populated until the action of importing.
        private string m_sRasterMetaData;


        private int m_nNoInterpolationIndex;
        /// <summary>
        /// Dictionary of non-GDAL compliant rasters to their GDAL compliant pairs.
        /// </summary>
        /// <remarks>This form requires the use of GDAL compliant rasters. i.e. the
        /// original raster cannot be in a file geodatabase. Each time the user browses
        /// to or picks a new raster it needs to be copied to a GDAL compliant format
        /// if it is not already. This can be time consuming. So this dictionary
        /// keeps track of any non-GDAL compliant raster paths and the GDAL compliant
        /// copy. That way the user can change raster selection quickly without a lag
        /// as rasters are copied repeatedly. It also keeps the number of temp rasters
        /// down.
        /// </remarks>

        private Dictionary<string, string> m_RasterDirects;
        public GCDConsoleLib.ExtentRectangle OriginalExtent
        {
            get { return m_OriginalExtent; }
        }

        public string RasterMetaData
        {
            get { return m_sRasterMetaData; }
        }

        public string StringFormat
        {
            get
            {
                string sResult = "#,##0";
                if (valPrecision is System.Windows.Forms.NumericUpDown)
                {
                    for (int i = 1; i <= Convert.ToInt32(valPrecision.Value); i++)
                    {
                        if (i == 1)
                        {
                            sResult += ".";
                        }
                        sResult += "0";
                    }
                }
                return sResult;
            }
        }


        public frmImportRaster(GCDConsoleLib.Raster gReferenceRaster, DEMSurvey referenceDEM, ImportRasterPurposes ePurpose, string sNoun)
        {
            Load += ImportRasterForm_Load;
            // This call is required by the designer.
            InitializeComponent();
            m_gReferenceRaster = gReferenceRaster;
            m_ePurpose = ePurpose;
            m_DEM = referenceDEM;
            m_RasterDirects = new Dictionary<string, string>();
            ucRaster.Noun = sNoun;
        }

        /// <summary>
        /// Launch the raster import in standalone mode for cleaning rasters
        /// </summary>

        public frmImportRaster()
        {
            Load += ImportRasterForm_Load;
            // This call is required by the designer.
            InitializeComponent();
            m_gReferenceRaster = null;
            m_ePurpose = ImportRasterPurposes.StandaloneTool;
            m_RasterDirects = new Dictionary<string, string>();

        }


        private void ImportRasterForm_Load(object sender, System.EventArgs e)
        {
            SetupToolTips();

            cboMethod.Items.Add(new naru.db.NamedObject((long)ResamplingMethods.Bilinear, "Bilinear Interpolation"));
            cboMethod.Items.Add(new naru.db.NamedObject((long)ResamplingMethods.Cubic, "Cubic Convolution"));
            cboMethod.Items.Add(new naru.db.NamedObject((long)ResamplingMethods.NaturalNeighbours, "Natural Neighbours"));
            cboMethod.Items.Add(new naru.db.NamedObject((long)ResamplingMethods.NearestNeighbour, "Nearest Neighbour"));
            m_nNoInterpolationIndex = cboMethod.Items.Add(new naru.db.NamedObject((long)ResamplingMethods.None, "None (straight cell-wise copy)"));
            cboMethod.SelectedIndex = 0;

            valCellSize.Minimum = 0.01m;
            valCellSize.Maximum = 1000;
            // This needs to be changed to a larger value or else rasters with cell sizes greater than 1 will cause an error to be thrown. Perhaps 1000 is more appropriate?
            valCellSize.Value = 1;

            if (m_ePurpose != ImportRasterPurposes.StandaloneTool && ProjectManager.Project is GCDCore.Project.GCDProject)
            {
                valPrecision.Value = 2;
                // ProjectManager.Project.Precision
            }

            valTop.ReadOnly = !(m_ePurpose == ImportRasterPurposes.DEMSurvey || m_ePurpose == ImportRasterPurposes.StandaloneTool);
            valLeft.ReadOnly = valTop.ReadOnly;
            valBottom.ReadOnly = valTop.ReadOnly;
            valRight.ReadOnly = valTop.ReadOnly;
            valCellSize.ReadOnly = valTop.ReadOnly;

            valTop.Enabled = m_ePurpose == ImportRasterPurposes.DEMSurvey || m_ePurpose == ImportRasterPurposes.StandaloneTool;
            valLeft.Enabled = valTop.Enabled;
            valBottom.Enabled = valTop.Enabled;
            valRight.Enabled = valTop.Enabled;
            valCellSize.Enabled = valTop.Enabled;

            valTop.Minimum = decimal.MinValue;
            valLeft.Minimum = decimal.MinValue;
            valBottom.Minimum = decimal.MinValue;
            valRight.Minimum = decimal.MinValue;

            valTop.Maximum = decimal.MaxValue;
            valLeft.Maximum = decimal.MaxValue;
            valBottom.Maximum = decimal.MaxValue;
            valRight.Maximum = decimal.MaxValue;

            valTop.ThousandsSeparator = true;
            valLeft.ThousandsSeparator = true;
            valBottom.ThousandsSeparator = true;
            valRight.ThousandsSeparator = true;

            txtLeft.BackColor = this.BackColor;
            txtTop.BackColor = txtLeft.BackColor;
            txtBottom.BackColor = txtTop.BackColor;
            txtRight.BackColor = txtTop.BackColor;

            // need to clear the original raster text box. User may have canceled the form
            // with a selected raster (e.g. because spatial resolution doesn't match. This
            // form persists on the parent form and then is shown again. 
            txtLeft.Text = string.Empty;
            txtTop.Text = string.Empty;
            txtBottom.Text = string.Empty;
            txtRight.Text = string.Empty;
            txtOrigCellSize.Text = string.Empty;
            txtOrigRows.Text = string.Empty;
            txtOrigCols.Text = string.Empty;
            txtOrigHeight.Text = string.Empty;
            txtOrigWidth.Text = string.Empty;

            valPrecision.Enabled = m_ePurpose == ImportRasterPurposes.StandaloneTool;
            if (m_ePurpose == ImportRasterPurposes.StandaloneTool)
            {
                //lblPrecision.Text = "Precision:"
                this.Text = "Clean Raster";
                grpProjectRaaster.Text = "Clean Raster";
                cmdOK.Text = "Create Clean Raster";
                lblRasterPath.Text = "Output path";
                lblName.Visible = false;
                txtName.Visible = false;

                // Shortern the form when in standalone mode because there in no raster name needed.
                var fFormHeightOffset = (grpOriginalRaster.Top - lblName.Top);
                grpProjectRaaster.Top = grpProjectRaaster.Top - fFormHeightOffset;
                this.Height = this.Height - fFormHeightOffset;
                grpOriginalRaster.Top = txtName.Top;
            }
            else
            {
                this.Text = "Specify GCD " + ucRaster.Noun;
                cmdOK.Text = "Import Raster";
                grpProjectRaaster.Text = "GCD " + ucRaster.Noun;
                txtRasterPath.Width = txtName.Width;
                cmdSave.Visible = false;

                if (m_ePurpose == ImportRasterPurposes.DEMSurvey)
                {
                    if (m_gReferenceRaster is GCDConsoleLib.Raster)
                    {
                        // there is already at least one DEM in the project. Disable cell size.
                        valCellSize.Enabled = false;
                    }
                    else
                    {
                        // This is the first DEM survey. Let the user adjust the precision.
                        valPrecision.Enabled = true;
                    }
                }
            }
            OriginalRasterChanged();
            ucRaster.PathChanged += OnRasterChanged;
        }


        private void SetupToolTips()
        {
        }


        private void cmdOK_Click(System.Object sender, System.EventArgs e)
        {
            if (!ValidateForm())
            {
                this.DialogResult = DialogResult.None;
                return;
            }

        }

        private bool ValidateForm()
        {

            // Sanity check to avoid names with only spaces
            txtName.Text = txtName.Text.Trim();

            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("The raster name cannot be empty.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (!(ucRaster.SelectedItem is GCDConsoleLib.Raster))
            {
                MessageBox.Show("You must select a raster to import. Use the browse button if the raster you want is not already in the map and dropdown list.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            GCDConsoleLib.Raster r = ucRaster.SelectedItem;

            // Safely get the projection of the reference raster
            string RefRasterSpatRef = string.Empty;
            if (m_gReferenceRaster is GCDConsoleLib.Raster)
            {
                RefRasterSpatRef = m_gReferenceRaster.Proj.Wkt;
            }

            // Verify that the raster has a spatial reference
            if (ucRaster.SelectedItem.Proj.PrettyWkt.ToLower().Contains("unknown"))
            {
                MessageBox.Show("The selected raster appears to be missing a spatial reference. All GCD rasters must possess a spatial reference and it must be the same spatial reference for all rasters in a GCD project." + " If the selected raster exists in the same coordinate system " + RefRasterSpatRef + ", but the coordinate system has not yet been defined for the raster" + " use the ArcToolbox 'Define Projection' geoprocessing tool in the 'Data Management -> Projection & Transformations' Toolbox to correct the problem with the selected raster by defining the coordinate system as:" + Environment.NewLine + Environment.NewLine + RefRasterSpatRef + Environment.NewLine + Environment.NewLine + "Then try importing it into the GCD again.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else
            {
                if (m_gReferenceRaster is GCDConsoleLib.Raster)
                {
                    if (!m_gReferenceRaster.Proj.IsSame(ucRaster.SelectedItem.Proj))
                    {
                        MessageBox.Show("The coordinate system of the selected raster:" + Environment.NewLine + Environment.NewLine + ucRaster.SelectedItem.Proj.PrettyWkt + Environment.NewLine + Environment.NewLine + "does not match that of the GCD project:" + Environment.NewLine + Environment.NewLine + RefRasterSpatRef + Environment.NewLine + Environment.NewLine + "All rasters within a GCD project must have the identical coordinate system. However, small discrepencies in coordinate system names might cause the two coordinate systems to appear different. " + "If you believe that the selected raster does in fact possess the same coordinate system as the GCD project then use the ArcToolbox 'Define Projection' geoprocessing tool in the " + "'Data Management -> Projection & Transformations' Toolbox to correct the problem with the selected raster by defining the coordinate system as:" + Environment.NewLine + Environment.NewLine + RefRasterSpatRef + Environment.NewLine + Environment.NewLine + "Then try importing it into the GCD again.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
            }


            if (m_ePurpose != ImportRasterPurposes.StandaloneTool)
            {
                // Verify that the horizontal units match those of the project.
                if (ProjectManager.Project.Units.HorizUnit != r.Proj.HorizontalUnit)
                {
                    string msg = string.Format("The horizontal units of the raster ({0}) do not match those of the GCD project ({1}).", r.Proj.HorizontalUnit.ToString(), ProjectManager.Project.Units.HorizUnit.ToString());
                    if (ProjectManager.Project.DEMSurveys.Count < 1)
                    {
                        msg += " You can change the GCD project horizontal units by canceling this form and opening the GCD project properties form.";
                    }
                    MessageBox.Show(msg, "HorizontalUnits Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                // Verify the optional vertical units for rasters that should share the project vertical units
                if (m_ePurpose == ImportRasterPurposes.DEMSurvey || m_ePurpose == ImportRasterPurposes.ErrorCalculation)
                {
                    if (!(r.VerticalUnits == null) && r.VerticalUnits != UnitsNet.Units.LengthUnit.Undefined)
                    {
                        if (r.VerticalUnits != ProjectManager.Project.Units.VertUnit)
                        {
                            MessageBox.Show(string.Format("The raster has different vertical units ({0}) than the GCD project ({1})." + " You must change the vertical units of the raster before it can be used within the GCD.", r.VerticalUnits.ToString(), ProjectManager.Project.Units.VertUnit.ToString()), "Vertical Units Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(txtRasterPath.Text))
            {
                if (m_ePurpose == ImportRasterPurposes.StandaloneTool)
                {
                    MessageBox.Show("The output raster path cannot be empty. Click the Save button to specify an output raster path.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("The " + ucRaster.Noun + " path cannot be empty. Try using a different name.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return false;
            }
            else
            {
                if (System.IO.File.Exists(txtRasterPath.Text))
                {
                    MessageBox.Show("The project raster path already exists. Try using a different name for the raster.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                else
                {
                    string sExtension = System.IO.Path.GetExtension(txtRasterPath.Text);
                    if (string.Compare(sExtension, ".tif", true) != 0)
                    {
                        MessageBox.Show("This tool can only currently produce GeoTIFF rasters. Please provide an output raster path ending with '.tif'", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
            }

            if (valCellSize.Value <= 0)
            {
                MessageBox.Show("The cell size must be greater than or equal to zero.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if ((valRight.Value - valLeft.Value) < valCellSize.Value)
            {
                MessageBox.Show("The right edge of the extent must be at least one cell width more than the left edge of the extent.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (valTop.Value - valBottom.Value < valCellSize.Value)
            {
                MessageBox.Show("The top edge of the extent must be at least one cell width more than the bottom edge of the extent.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            ResamplingMethods eType = (ResamplingMethods)((naru.db.NamedObject)cboMethod.SelectedItem).ID;
            if (RequiresResampling())
            {
                if (eType != ResamplingMethods.Bilinear)
                {
                    if (eType == ResamplingMethods.None)
                    {
                        MessageBox.Show("The input raster requires resampling. Please select the desired resampling method.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Only bilinear interpolation is currently functional within the GCD.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return false;
                }
            }
            else
            {
                if (cboMethod.SelectedIndex != m_nNoInterpolationIndex)
                {
                    MessageBox.Show("The raster is orthogonal and divisible with the specified output. No interpolation is required. Select \"None\" in the interpolation method drop down.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            return true;

        }


        private void OnRasterChanged(object sender, naru.ui.PathEventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                OriginalRasterChanged();
                cmdOK.Select();
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }

        }

        private void OriginalRasterChanged()
        {
            //
            // There is no reference raster, or we are in DEM survey mode. So determine the
            // orthogonal extent of the selected raster. Convert it to a GDAL raster first
            // (if its not already) then "orthogonalize" it's extent.
            //
            if (ucRaster.SelectedItem is GCDConsoleLib.Raster)
            {
                GCDConsoleLib.Raster gOriginalRaster = null;

                if (GetSafeOriginalRaster(ref gOriginalRaster))
                {
                    if (valPrecision.Enabled)
                    {
                        //
                        // Try to determine the appropriate precision from the input raster.
                        // Keep increasing the original cell resolution by powers of ten until it
                        // is a whole number. This is the appropriate "initial" precision for the
                        // output until the user overrides it.
                        //
                        int nPrecision = 1;
                        decimal fCellSize = gOriginalRaster.Extent.CellWidth;
                        for (int i = 0; i <= 10; i++)
                        {
                            decimal fTest = fCellSize * (decimal)Math.Pow(10, i);
                            fTest = Math.Round(fTest, 4);
                            if (fTest % 1 == 0)
                            {
                                valPrecision.Value = Convert.ToDecimal(i);
                                break; // TODO: might not be correct. Was : Exit For
                            }
                        }
                    }

                    m_OriginalExtent = gOriginalRaster.Extent;

                    txtTop.Text = gOriginalRaster.Extent.Top.ToString();
                    txtLeft.Text = gOriginalRaster.Extent.Left.ToString();
                    txtBottom.Text = gOriginalRaster.Extent.Bottom.ToString();
                    txtRight.Text = gOriginalRaster.Extent.Right.ToString();

                    txtOrigRows.Text = gOriginalRaster.Extent.rows.ToString("#,##0");
                    txtOrigCols.Text = gOriginalRaster.Extent.cols.ToString("#,##0");
                    txtOrigWidth.Text = (gOriginalRaster.Extent.Right - gOriginalRaster.Extent.Left).ToString();
                    txtOrigHeight.Text = (gOriginalRaster.Extent.Top - gOriginalRaster.Extent.Bottom).ToString();
                    txtOrigCellSize.Text = gOriginalRaster.Extent.CellWidth.ToString();
                    UpdateOriginalRasterExtentFormatting();

                    if (!(m_gReferenceRaster is GCDConsoleLib.Raster && m_ePurpose != ImportRasterPurposes.DEMSurvey))
                    {
                        valCellSize.Value = Math.Max(Math.Round(gOriginalRaster.Extent.CellWidth, valCellSize.DecimalPlaces), valCellSize.Minimum);
                        if (valPrecision.Value < 1)
                        {
                            valCellSize.Value = Math.Max(valCellSize.Value, 1);
                        }
                    }

                    if (string.IsNullOrEmpty(txtName.Text))
                    {
                        txtName.Text = System.IO.Path.GetFileNameWithoutExtension(ucRaster.SelectedItem.GISFileInfo.FullName);
                    }
                    else
                    {
                        UpdateRasterPath();
                    }
                }

            }

            //
            // Only use the reference raster for the orthogonal extent when in associated
            // surface or error surface mode. When in DEM Survey mode, the reference raster
            // is just for matching spatial reference.

            if (m_gReferenceRaster is GCDConsoleLib.Raster && (m_ePurpose != ImportRasterPurposes.DEMSurvey || m_ePurpose == ImportRasterPurposes.StandaloneTool))
            {
                decimal fCellSize = m_gReferenceRaster.Extent.CellWidth;
                valCellSize.Maximum = fCellSize;
                valCellSize.Value = fCellSize;

                valTop.Maximum = m_gReferenceRaster.Extent.Top;
                valTop.Value = m_gReferenceRaster.Extent.Top;

                valLeft.Maximum = m_gReferenceRaster.Extent.Left;
                valLeft.Value = m_gReferenceRaster.Extent.Left;

                valBottom.Maximum = m_gReferenceRaster.Extent.Bottom;
                valBottom.Value = m_gReferenceRaster.Extent.Bottom;

                valRight.Maximum = m_gReferenceRaster.Extent.Right;
                valRight.Value = m_gReferenceRaster.Extent.Right;

                // PGB - 24 Apr 2015 - When in associated surface mode we need to update the method
                // dropdown to reflect where the raster being imported can be copied or is resampled.
                RequiresResampling();

                //This case deals with when using the Standalone tool and switching between rasters in combobox need mechanism to update to current raster
                //Case also deals with when a GCD project is started and no raster has been added yet or in map, i.e. no reference raster and raster is added through browsing
            }
            else if (m_gReferenceRaster == null && (m_ePurpose == ImportRasterPurposes.StandaloneTool || m_ePurpose == ImportRasterPurposes.DEMSurvey))
            {
                UpdateOutputExtent();

                //This case deals with when importing a raster and switching between rasters in combobox need mechanism to update to current raster
            }
            else if (m_gReferenceRaster is GCDConsoleLib.Raster && (m_ePurpose == ImportRasterPurposes.DEMSurvey || m_ePurpose == ImportRasterPurposes.StandaloneTool))
            {
                UpdateOutputExtent();
            }
            else
            {
                RequiresResampling();
            }

            string sFormat = "#,##0";
            if (valCellSize.DecimalPlaces > 0)
            {
                sFormat += ".";
                for (int i = 0; i <= valCellSize.DecimalPlaces - 1; i++)
                {
                    sFormat += "0";
                }
            }

        }

        /// <summary>
        /// Get a GDAL raster for the selected raster in the dropdown list.
        /// </summary>
        /// <param name="gRasterDirect">Output GDAL raster for the selected item in the raster combo box.</param>
        /// <remarks>>This form requires the use of GDAL compliant rasters. i.e. the
        /// original raster cannot be in a file geodatabase. Each time the user browses
        /// to or picks a new raster it needs to be copied to a GDAL compliant format
        /// if it is not already. This can be time consuming. So this dictionary
        /// keeps track of any non-GDAL compliant raster paths and the GDAL compliant
        /// copy. That way the user can change raster selection quickly without a lag
        /// as rasters are copied repeatedly. It also keeps the number of temp rasters
        /// down.</remarks>
        private bool GetSafeOriginalRaster(ref GCDConsoleLib.Raster gRasterDirect)
        {

            gRasterDirect = null;
            if (ucRaster.SelectedItem is GCDConsoleLib.Raster)
            {
                gRasterDirect = new GCDConsoleLib.Raster(ucRaster.SelectedItem.GISFileInfo);
            }

            return gRasterDirect is GCDConsoleLib.Raster;

        }


        private void UpdateRasterPath()
        {
            try
            {
                // Standalone tool browses to the output, and does not derive it from original raster.
                if (m_ePurpose == ImportRasterPurposes.StandaloneTool)
                {
                    return;
                }

                System.IO.FileInfo sRasterPath = null;
                if (!string.IsNullOrEmpty(txtName.Text))
                {
                    if (ucRaster.SelectedItem is GCDConsoleLib.Raster)
                    {
                        // Get the appropriate raster path depending on the purpose of this window (DEM, associated surface, error surface)

                        switch (m_ePurpose)
                        {
                            case ImportRasterPurposes.DEMSurvey:
                                sRasterPath = ProjectManager.OutputManager.DEMSurveyRasterPath(txtName.Text);

                                break;
                            case ImportRasterPurposes.AssociatedSurface:
                                sRasterPath = ProjectManager.OutputManager.AssociatedSurfaceRasterPath(m_DEM.Name, txtName.Text);

                                break;
                            case ImportRasterPurposes.ErrorCalculation:
                                sRasterPath = ProjectManager.OutputManager.ErrorSurfaceRasterPath(m_DEM.Name, txtName.Text);

                                break;
                            default:
                                MessageBox.Show("Unhandled import raster purpose: " + m_ePurpose.ToString(), Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                break;
                        }
                    }
                }

                if (sRasterPath == null)
                {
                    txtRasterPath.Text = string.Empty;
                }
                else
                {
                    txtRasterPath.Text = sRasterPath.FullName;
                }

            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        public GCDConsoleLib.Raster ProcessRaster()
        {

            GCDConsoleLib.Raster gResult = null;
            if (!string.IsNullOrEmpty(txtRasterPath.Text))
            {
                if (System.IO.File.Exists(txtRasterPath.Text))
                {
                    Exception ex = new Exception("The raster path already exists.");
                    ex.Data.Add("Raster path", txtRasterPath.Text);
                    throw ex;
                }
                else
                {
                    GCDConsoleLib.Raster gRaster = null;

                    if (GetSafeOriginalRaster(ref gRaster))
                    {
                        string sWorkspace = System.IO.Path.GetDirectoryName(txtRasterPath.Text);
                        System.IO.DirectoryInfo theDir = System.IO.Directory.CreateDirectory(sWorkspace);

                        if (theDir.Exists)
                        {
                            int nCols = Convert.ToInt32(txtProjCols.Text.Replace(",", ""));
                            int nRows = Convert.ToInt32(txtProjRows.Text.Replace(",", ""));

                            // Match the cell height of the final raster with that of the original raster
                            decimal cellHeight = valCellSize.Value;
                            GCDConsoleLib.Raster gOrigRaster = new GCDConsoleLib.Raster(ucRaster.SelectedItem.GISFileInfo);
                            if (gOrigRaster.Extent.CellHeight < 0)
                            {
                                cellHeight = -cellHeight;
                            }

                            GCDConsoleLib.ExtentRectangle outputExtent = new GCDConsoleLib.ExtentRectangle(valTop.Value, valLeft.Value, cellHeight, valCellSize.Value, nRows, nCols);

                            if (RequiresResampling())
                            {
                                gResult = GCDConsoleLib.RasterOperators.BilinearResample(gRaster, new System.IO.FileInfo(txtRasterPath.Text), outputExtent);
                                Debug.WriteLine("Bilinear resample:" + outputExtent.ToString());
                            }
                            else
                            {
                                gResult = GCDConsoleLib.RasterOperators.ExtendedCopy(gRaster, new System.IO.FileInfo(txtRasterPath.Text), outputExtent);
                                Debug.WriteLine("Copying raster:" + outputExtent.ToString());
                            }

                            // This method will check to see if pyrmaids are need and then build if necessary.
                            PerformRasterPyramids(m_ePurpose, new System.IO.FileInfo(txtRasterPath.Text));

                            if (m_ePurpose == ImportRasterPurposes.DEMSurvey)
                            {
                                // Now try the hillshade for DEM Surveys
                                System.IO.FileInfo sHillshadePath = ProjectManager.OutputManager.DEMSurveyHillShadeRasterPath(txtName.Text);
                                GCDConsoleLib.RasterOperators.Hillshade(gResult, sHillshadePath);
                                ProjectManager.PyramidManager.PerformRasterPyramids(GCDCore.RasterPyramidManager.PyramidRasterTypes.Hillshade, sHillshadePath);
                            }
                        }
                        else
                        {
                            Exception ex = new Exception("Failed to create raster workspace folder");
                            ex.Data.Add("Raster Path", txtRasterPath.Text);
                            ex.Data.Add("Workspace", sWorkspace);
                            throw ex;
                        }
                    }
                }
            }

            return gResult;

        }

        private void txtName_TextChanged(object sender, System.EventArgs e)
        {
            UpdateRasterPath();
        }

        private void valBuffeer_ValueChanged(object sender, System.EventArgs e)
        {
            OriginalRasterChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Note that this can be triggered either by the user changing the value or by 
        /// the code setting the value.</remarks>

        private void valLeft_ValueChanged(object sender, System.EventArgs e)
        {
            UpdateOutputRowsColsHeightWidth();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Note that changing the cell size requires that the extent be changed. This 
        /// in turn will trigger the updating of the rows/cols and width/height.</remarks>

        private void valCellSize_ValueChanged(object sender, System.EventArgs e)
        {
            //valCellSize.Value = Math.Round(valCellSize.Value, CInt(valPrecision.Value))

            UpdateOutputExtent();
            UpdateOriginalRasterExtentFormatting();

            valLeft.Increment = valCellSize.Value;
            valTop.Increment = valCellSize.Value;
            valRight.Increment = valCellSize.Value;
            valBottom.Increment = valCellSize.Value;
        }


        private void UpdateOutputExtent()
        {
            try
            {
                if (m_OriginalExtent is GCDConsoleLib.ExtentRectangle)
                {
                    valTop.Minimum = decimal.MinValue;
                    valLeft.Minimum = decimal.MinValue;
                    valBottom.Minimum = decimal.MinValue;
                    valRight.Minimum = decimal.MinValue;

                    valTop.Maximum = decimal.MaxValue;
                    valLeft.Maximum = decimal.MaxValue;
                    valBottom.Maximum = decimal.MaxValue;
                    valRight.Maximum = decimal.MaxValue;

                    valLeft.Value = MakeDivisible(m_OriginalExtent.Left, valCellSize.Value, Convert.ToInt32(valPrecision.Value), RoundingDirection.Down);
                    valBottom.Value = MakeDivisible(m_OriginalExtent.Bottom, valCellSize.Value, Convert.ToInt32(valPrecision.Value), RoundingDirection.Down);
                    valRight.Value = MakeDivisible(m_OriginalExtent.Right, valCellSize.Value, Convert.ToInt32(valPrecision.Value), RoundingDirection.Up);
                    valTop.Value = MakeDivisible(m_OriginalExtent.Top, valCellSize.Value, Convert.ToInt32(valPrecision.Value), RoundingDirection.Up);
                    UpdateOriginalRasterExtentFormatting();
                }

                UpdateOutputRowsColsHeightWidth();
                RequiresResampling();

            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }


        private void UpdateOutputRowsColsHeightWidth()
        {
            Debug.Assert(valCellSize.Value > 0, "The cell size should never be zero.");

            decimal fProjHeight = (valTop.Value - valBottom.Value);
            decimal fProjWidth = (valRight.Value - valLeft.Value);
            txtProjRows.Text = (fProjHeight / valCellSize.Value).ToString("#,##0");
            txtProjCols.Text = (fProjWidth / valCellSize.Value).ToString("#,##0");
            txtProjWidth.Text = fProjWidth.ToString();
            txtProjHeight.Text = fProjHeight.ToString();

            valTop.ForeColor = System.Drawing.Color.Black;
            valLeft.ForeColor = System.Drawing.Color.Black;
            valBottom.ForeColor = System.Drawing.Color.Black;
            valRight.ForeColor = System.Drawing.Color.Black;
            if (ucRaster.SelectedItem is GCDConsoleLib.Raster)
            {
                GCDConsoleLib.Raster gRaster = null;

                if (GetSafeOriginalRaster(ref gRaster))
                {
                    if (valTop.Value == MakeDivisible(gRaster.Extent.Top, valCellSize.Value, Convert.ToInt32(valPrecision.Value), RoundingDirection.Up))
                        valTop.ForeColor = System.Drawing.Color.DarkGreen;
                    if (valLeft.Value == MakeDivisible(gRaster.Extent.Left, valCellSize.Value, Convert.ToInt32(valPrecision.Value), RoundingDirection.Down))
                        valLeft.ForeColor = System.Drawing.Color.DarkGreen;
                    if (valBottom.Value == MakeDivisible(gRaster.Extent.Bottom, valCellSize.Value, Convert.ToInt32(valPrecision.Value), RoundingDirection.Down))
                        valBottom.ForeColor = System.Drawing.Color.DarkGreen;
                    if (valRight.Value == MakeDivisible(gRaster.Extent.Right, valCellSize.Value, Convert.ToInt32(valPrecision.Value), RoundingDirection.Up))
                        valRight.ForeColor = System.Drawing.Color.DarkGreen;

                }
            }

        }

        private decimal MakeDivisible(decimal fOriginalValue, decimal fCellSize, int nPrecision, RoundingDirection eRoundingDirection)
        {

            decimal fResult = 0;
            if (fOriginalValue != 0 && fCellSize != 0)
            {
                fResult = fOriginalValue / fCellSize;
                // (10 ^ nPrecision)
                if (eRoundingDirection == RoundingDirection.Up)
                {
                    fResult = Math.Ceiling(fResult);
                }
                else
                {
                    fResult = Math.Floor(fResult);
                }
                fResult = fResult * fCellSize;
            }
            return fResult;

        }


        private void UpdateOriginalRasterExtentFormatting()
        {
            throw new NotImplementedException("commented");

            //// Set the extent to red text if it is not divisible.
            //if (m_OriginalExtent is GCDConsoleLib.ExtentRectangle && valCellSize.Value > 0)
            //{
            //    decimal fValue = Math.IEEERemainder(m_OriginalExtent.Left, valCellSize.Value);
            //    fValue = Math.Round(Math.IEEERemainder(m_OriginalExtent.Left, valCellSize.Value), Convert.ToInt32(valPrecision.Value) + 1);

            //    decimal fCellSize = Math.Max(Math.Round(valCellSize.Value, Convert.ToInt32(valPrecision.Value)), valCellSize.Minimum);
            //    Debug.Assert(fCellSize > 0, "The cell size should not be zero.");

            //    if (Math.Round(Math.IEEERemainder(m_OriginalExtent.Left, fCellSize), Convert.ToInt32(valPrecision.Value) + 1) != 0)
            //    {
            //        txtLeft.ForeColor = System.Drawing.Color.Red;
            //    }
            //    else
            //    {
            //        txtLeft.ForeColor = Control.DefaultForeColor;
            //    }

            //    if (Math.Round(Math.IEEERemainder(m_OriginalExtent.Top, fCellSize), Convert.ToInt32(valPrecision.Value) + 1) != 0)
            //    {
            //        txtTop.ForeColor = System.Drawing.Color.Red;
            //    }
            //    else
            //    {
            //        txtTop.ForeColor = Control.DefaultForeColor;
            //    }

            //    if (Math.Round(Math.IEEERemainder(m_OriginalExtent.Bottom, fCellSize), Convert.ToInt32(valPrecision.Value) + 1) != 0)
            //    {
            //        txtBottom.ForeColor = System.Drawing.Color.Red;
            //    }
            //    else
            //    {
            //        txtBottom.ForeColor = Control.DefaultForeColor;
            //    }

            //    if (Math.Round(Math.IEEERemainder(m_OriginalExtent.Right, fCellSize), Convert.ToInt32(valPrecision.Value) + 1) != 0)
            //    {
            //        txtRight.ForeColor = System.Drawing.Color.Red;
            //    }
            //    else
            //    {
            //        txtRight.ForeColor = Control.DefaultForeColor;
            //    }
            //}
        }

        private bool RequiresResampling()
        {

            bool bResult = true;
            GCDConsoleLib.Raster gOriginalRaster = null;
            if (GetSafeOriginalRaster(ref gOriginalRaster))
            {
                bResult = !gOriginalRaster.IsDivisible();
            }

            if (cboMethod.Items.Count > m_nNoInterpolationIndex)
            {
                if (bResult)
                {
                    cboMethod.SelectedIndex = 0;
                }
                else
                {
                    cboMethod.SelectedIndex = m_nNoInterpolationIndex;
                }
            }
            cboMethod.Enabled = bResult;

            return bResult;

        }

        /// <summary>
        /// Disable typing in the original raster extent text boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Cannot change forecolor of textboxes when they are readonly. So make them
        /// ReadOnly = False but skip any key pressing.</remarks>

        private void txtLeft_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cmdSave_Click(System.Object sender, System.EventArgs e)
        {
            naru.ui.Textbox.BrowseSaveRaster(txtRasterPath, "Output Raster", naru.os.File.RemoveDangerousCharacters(txtName.Text));
        }

        private void valPrecision_ValueChanged(object sender, System.EventArgs e)
        {
            valCellSize.DecimalPlaces = (int)valPrecision.Value;
            valCellSize.Increment = (decimal)Math.Pow(10, Convert.ToDouble(decimal.Negate(valPrecision.Value)));
            valCellSize.Minimum = (decimal)Math.Pow(10, Convert.ToDouble(decimal.Negate(valPrecision.Value)));
            valCellSize.Value = Math.Round(valCellSize.Value, Convert.ToInt32(valPrecision.Value));
            valTop.DecimalPlaces = valCellSize.DecimalPlaces;
            valLeft.DecimalPlaces = valCellSize.DecimalPlaces;
            valBottom.DecimalPlaces = valCellSize.DecimalPlaces;
            valRight.DecimalPlaces = valCellSize.DecimalPlaces;
            //UpdateOriginalRasterExtentFormatting()
        }

        private void cmdHelpPrecision_Click(System.Object sender, System.EventArgs e)
        {
            GCDCore.UserInterface.UtilityForms.frmInformation frm = new GCDCore.UserInterface.UtilityForms.frmInformation();
            frm.InitializeFixedDialog("Horizontal Decimal Precision", GCDCore.Properties.Resources.PrecisionHelp);
            frm.ShowDialog();
        }


        private void cmdHelp_Click(System.Object sender, System.EventArgs e)
        {
            switch (m_ePurpose)
            {
                case ImportRasterPurposes.StandaloneTool:
                    Process.Start(GCDCore.Properties.Resources.HelpBaseURL + "gcd-command-reference/data-prep-menu/a-clean-raster-tool");

                    break;
                case ImportRasterPurposes.DEMSurvey:
                    Process.Start(GCDCore.Properties.Resources.HelpBaseURL + "gcd-command-reference/data-prep-menu/d-add-dem-survey");

                    break;
                case ImportRasterPurposes.AssociatedSurface:
                    Process.Start(GCDCore.Properties.Resources.HelpBaseURL + "gcd-command-reference/gcd-project-explorer/d-dem-context-menu/iv-add-associated-surface/1-loading-user-defined-associated-surface");

                    break;
                case ImportRasterPurposes.ErrorCalculation:
                    Process.Start(GCDCore.Properties.Resources.HelpBaseURL + "gcd-command-reference/gcd-project-explorer/g-error-surfaces-context-menu/i-specify-error-surface");

                    break;
            }
        }


        private void PerformRasterPyramids(ImportRasterPurposes ePurpose, System.IO.FileInfo sRasterPath)
        {
            GCDCore.RasterPyramidManager.PyramidRasterTypes ePyramidRasterType = default(GCDCore.RasterPyramidManager.PyramidRasterTypes);
            switch (m_ePurpose)
            {
                case ImportRasterPurposes.DEMSurvey:
                    ePyramidRasterType = GCDCore.RasterPyramidManager.PyramidRasterTypes.DEM;
                    break;
                case ImportRasterPurposes.AssociatedSurface:
                    ePyramidRasterType = GCDCore.RasterPyramidManager.PyramidRasterTypes.AssociatedSurfaces;
                    break;
                case ImportRasterPurposes.ErrorCalculation:
                    ePyramidRasterType = GCDCore.RasterPyramidManager.PyramidRasterTypes.ErrorSurfaces;
                    break;
                case ImportRasterPurposes.StandaloneTool:
                    return;

                    break;
                default:
                    Debug.Assert(false, string.Format("The import raster purpose '{0}' does not have a corresponding raster pyramid build type.", m_ePurpose));
                    break;
            }

            ProjectManager.PyramidManager.PerformRasterPyramids(ePyramidRasterType, sRasterPath);

        }

    }

}
