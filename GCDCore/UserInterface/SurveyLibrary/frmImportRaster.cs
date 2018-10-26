using System;
using System.Windows.Forms;
using GCDCore.Project;
using GCDConsoleLib;
using GCDConsoleLib.ExtentAdjusters;

namespace GCDCore.UserInterface.SurveyLibrary
{
    public partial class frmImportRaster
    {
        public enum Purposes
        {
            FirstDEM,
            SubsequentDEM,
            AssociatedSurface,
            ErrorSurface,
            ReferenceSurface,
            ReferenceErrorSurface
        };

        public static frmImportRaster PrepareToImportRaster(Surface refSurface, Purposes purpose, string Noun, IntPtr parentWindow)
        {
            frmImportRaster frm = null;
            Raster source = ProjectManager.BrowseRaster(naru.ui.UIHelpers.WrapMessageWithNoun("Browse and Select a", Noun, "Raster"), parentWindow);
            if (source is Raster)
            {

                if (GISDatasetValidation.DSHasSpatialRef(source, "raster", "rasters"))
                {
                    frm = new frmImportRaster(source, refSurface, purpose, Noun);
                }
            }

            return frm;
        }

        private bool NeedsForcedProjection; // true if the source raster doesn't quite match project SRS but user wants to force it anyway
        public readonly Raster SourceRaster;
        public readonly Surface ReferenceSurface;
        public readonly Purposes Purpose;

        public ExtentAdjusterBase ExtImporter;

        public frmImportRaster(Raster sourceRaster, Surface refSurface, Purposes ePurpose, string sNoun)
        {

            // This call is required by the designer.
            InitializeComponent();

            Cursor = Cursors.WaitCursor;

            NeedsForcedProjection = false;
            Text = "Add Existing " + sNoun;
            grpProjectRaaster.Text = "GCD " + sNoun;
            Purpose = ePurpose;
            SourceRaster = sourceRaster;
            txtSourceRaster.Text = sourceRaster.GISFileInfo.FullName;

            if (ePurpose == Purposes.FirstDEM)
            {
                ExtImporter = new ExtentAdjusterNoReference(sourceRaster.Extent);
            }
            else
            {
                ReferenceSurface = refSurface;

                if (Purpose == Purposes.SubsequentDEM || Purpose == Purposes.ReferenceSurface)
                {
                    ExtImporter = new ExtentAdjusterWithReference(sourceRaster.Extent, refSurface.Raster.Extent);
                }
                else
                {
                    ExtImporter = new ExtentAdjusterFixed(sourceRaster.Extent, refSurface.Raster.Extent);
                }
            }

            Cursor = Cursors.Default;
        }

        private void ImportRasterForm_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            #region Tool Tips
            tTip.SetToolTip(txtName, "The name used to refer to this GCD project item. It cannot be empty and must be unique among all items of the same type.");
            tTip.SetToolTip(txtTop, "The top, northern most extent of the original raster. Non-divislbe values apear in red.");
            tTip.SetToolTip(txtLeft, "The left, western most extent of the original raster. Non-divislbe values apear in red.");
            tTip.SetToolTip(txtRight, "The right, eastern most extent of the original raster. Non-divislbe values apear in red.");
            tTip.SetToolTip(txtBottom, "The bottom, southern most extent of the original raster. Non-divislbe values apear in red.");
            tTip.SetToolTip(txtOrigRows, "The number of rows in the original raster.");
            tTip.SetToolTip(txtOrigCols, "The number of columns in the original raster.");
            tTip.SetToolTip(txtOrigWidth, "The width of the original raster shown in the linear units of the raster.");
            tTip.SetToolTip(txtOrigHeight, "The height of the original raster shown in the linear units of the raster.");
            tTip.SetToolTip(txtOrigCellSize, "The width of each cell in the original raster shown in the linear units of the raster");
            tTip.SetToolTip(txtRasterPath, "The raster file path where the output raster will get generated.");
            tTip.SetToolTip(valTop, "The top, northern most extent of the output raster. It must be wholely divisible by the output cell resolution.");
            tTip.SetToolTip(valLeft, "The left, western most extent of the output raster. It must be wholely divisible by the output cell resolution.");
            tTip.SetToolTip(valRight, "The right, eastern most extent of the output raster. It must be wholely divisible by the output cell resolution.");
            tTip.SetToolTip(valBottom, "The bottom, southern most extent of the output raster. It must be wholely divisible by the output cell resolution.");
            tTip.SetToolTip(txtProjRows, "The number of rows in the output raster.");
            tTip.SetToolTip(txtProjCols, "The number of columns in the output raster.");
            tTip.SetToolTip(txtProjWidth, "The width of the output raster shown in the linear units of the raster.");
            tTip.SetToolTip(txtProjHeight, "The height of the output raster shown in the linear units of the raster.");
            tTip.SetToolTip(valCellSize, "The size of each cell in the output raster specified in the linear units of the raster.");
            tTip.SetToolTip(valPrecision, "The number of decimal places to consider when rounding the cell size of the original raster.");
            tTip.SetToolTip(txtInterpolationMethod, "Method used to generate the output raster. If the original raster extent is not evenly divisible by the cell resolution then bilinear sampling must be used. If the original raster is divisible by the cell resolution then the raster can simply be copied to the output path.");

            #endregion

            // This needs to be changed to a larger value or else rasters with cell sizes greater than 1 will cause an error to be thrown. Perhaps 1000 is more appropriate?
            valCellSize.Minimum = 0m;
            valCellSize.Maximum = 1000;
            valCellSize.Value = ExtImporter.OutExtent.CellWidth;
            valPrecision.Value = ExtImporter.Precision;
            valCellSize.Enabled = Purpose == Purposes.FirstDEM;
            valPrecision.Enabled = valCellSize.Enabled;

            // Changing dimensions only possible for first DEM
            valTop.Enabled = !(ExtImporter is ExtentAdjusterFixed);
            valLeft.Enabled = valTop.Enabled;
            valBottom.Enabled = valTop.Enabled;
            valRight.Enabled = valTop.Enabled;

            valTop.Minimum = decimal.MinValue;
            valLeft.Minimum = decimal.MinValue;
            valBottom.Minimum = decimal.MinValue;
            valRight.Minimum = decimal.MinValue;

            valTop.Maximum = decimal.MaxValue;
            valLeft.Maximum = decimal.MaxValue;
            valBottom.Maximum = decimal.MaxValue;
            valRight.Maximum = decimal.MaxValue;

            txtLeft.BackColor = this.BackColor;
            txtTop.BackColor = txtLeft.BackColor;
            txtBottom.BackColor = txtTop.BackColor;
            txtRight.BackColor = txtTop.BackColor;

            // Source raster properties
            txtLeft.Text = SourceRaster.Extent.Left.ToString();
            txtTop.Text = SourceRaster.Extent.Top.ToString();
            txtBottom.Text = SourceRaster.Extent.Bottom.ToString();
            txtRight.Text = SourceRaster.Extent.Right.ToString();
            txtOrigCellSize.Text = SourceRaster.Extent.CellWidth.ToString();
            txtOrigRows.Text = SourceRaster.Extent.Rows.ToString("#,##0");
            txtOrigCols.Text = SourceRaster.Extent.Cols.ToString();
            txtOrigHeight.Text = SourceRaster.Extent.Height.ToString();
            txtOrigWidth.Text = SourceRaster.Extent.Width.ToString();

            // Input extent text is red when not divisible
            txtTop.ForeColor = ExtentRectangle.DivideModuloOne(SourceRaster.Extent.Top, valCellSize.Value) == 0 ? Control.DefaultForeColor : System.Drawing.Color.Red;
            txtLeft.ForeColor = ExtentRectangle.DivideModuloOne(SourceRaster.Extent.Left, valCellSize.Value) == 0 ? Control.DefaultForeColor : System.Drawing.Color.Red;
            txtRight.ForeColor = ExtentRectangle.DivideModuloOne(SourceRaster.Extent.Right, valCellSize.Value) == 0 ? Control.DefaultForeColor : System.Drawing.Color.Red;
            txtBottom.ForeColor = ExtentRectangle.DivideModuloOne(SourceRaster.Extent.Bottom, valCellSize.Value) == 0 ? Control.DefaultForeColor : System.Drawing.Color.Red;

            // Initialize the output raster name and path
            txtName.Text = System.IO.Path.GetFileNameWithoutExtension(SourceRaster.GISFileInfo.FullName);
            UpdateRasterPath(sender, e);

            cmdOK.Select();

            // Trigger the updating of the output raster properties
            UpdateOutputExtent(sender, e);

            Cursor = Cursors.Default;
        }

        private void UpdateOutputExtent(object sender, EventArgs e)
        {
            ExtentAdjusterBase temp = ExtImporter;

            if (sender == valTop || sender == valRight || sender == valBottom || sender == valLeft)
            {
                temp = ExtImporter.AdjustDimensions(valTop.Value, valRight.Value, valBottom.Value, valLeft.Value);
            }
            else if (sender == valCellSize)
            {
                if (valCellSize.Value <= 0m)
                {
                    valCellSize.Value = ExtImporter.OutExtent.CellWidth;
                    return;
                }

                temp = ExtImporter.AdjustCellSize(valCellSize.Value);
            }
            else if (sender == valPrecision)
            {
                temp = ExtImporter.AdjustPrecision((ushort)valPrecision.Value);
            }

            // Turn off event firing
            valTop.ValueChanged -= UpdateOutputExtent;
            valBottom.ValueChanged -= UpdateOutputExtent;
            valRight.ValueChanged -= UpdateOutputExtent;
            valLeft.ValueChanged -= UpdateOutputExtent;
            valCellSize.ValueChanged -= UpdateOutputExtent;
            valPrecision.ValueChanged -= UpdateOutputExtent;

            // Update the extent adjustor
            ExtImporter = temp;

            // Update output control values
            valCellSize.DecimalPlaces = ExtImporter.Precision;
            valTop.DecimalPlaces = ExtImporter.Precision;
            valLeft.DecimalPlaces = ExtImporter.Precision;
            valRight.DecimalPlaces = ExtImporter.Precision;
            valBottom.DecimalPlaces = ExtImporter.Precision;

            valTop.Value = ExtImporter.OutExtent.Top;
            valLeft.Value = ExtImporter.OutExtent.Left;
            valRight.Value = ExtImporter.OutExtent.Right;
            valBottom.Value = ExtImporter.OutExtent.Bottom;
            valCellSize.Value = ExtImporter.OutExtent.CellWidth;
            valPrecision.Value = ExtImporter.Precision;

            // Colour the numeric up down boxes based on whether they match the original extent
            valTop.ForeColor = SourceRaster.Extent.Top == ExtImporter.OutExtent.Top ? System.Drawing.Color.DarkGreen : System.Drawing.Color.Black;
            valLeft.ForeColor = SourceRaster.Extent.Left == ExtImporter.OutExtent.Left ? System.Drawing.Color.DarkGreen : System.Drawing.Color.Black;
            valRight.ForeColor = SourceRaster.Extent.Right == ExtImporter.OutExtent.Right ? System.Drawing.Color.DarkGreen : System.Drawing.Color.Black;
            valBottom.ForeColor = SourceRaster.Extent.Bottom == ExtImporter.OutExtent.Bottom ? System.Drawing.Color.DarkGreen : System.Drawing.Color.Black;

            txtProjCols.Text = ExtImporter.OutExtent.Cols.ToString("#,##0");
            txtProjRows.Text = ExtImporter.OutExtent.Rows.ToString("#,##0");
            UnitsNet.Units.LengthUnit hUnits = SourceRaster.Proj.HorizontalUnit;
            txtProjWidth.Text = string.Format("{0}{1}", ExtImporter.OutExtent.Width, UnitsNet.Length.GetAbbreviation(hUnits));
            txtProjHeight.Text = string.Format("{0}{1}", ExtImporter.OutExtent.Height, UnitsNet.Length.GetAbbreviation(hUnits));

            txtInterpolationMethod.Text = ExtImporter.RequiresResampling ? "Bilinear Interpolation" : "None (straight cell-wise copy)";

            // Turn on event firing
            valTop.ValueChanged += UpdateOutputExtent;
            valBottom.ValueChanged += UpdateOutputExtent;
            valRight.ValueChanged += UpdateOutputExtent;
            valLeft.ValueChanged += UpdateOutputExtent;
            valCellSize.ValueChanged += UpdateOutputExtent;
            valPrecision.ValueChanged += UpdateOutputExtent;
        }

        private void UpdateRasterPath(object sender, EventArgs e)
        {
            txtRasterPath.Text = string.Empty;

            if (string.IsNullOrEmpty(txtName.Text))
                return;

            // Get the appropriate raster path depending on the purpose of this window (DEM, associated surface, error surface)
            switch (Purpose)
            {
                case Purposes.FirstDEM:
                case Purposes.SubsequentDEM:
                    txtRasterPath.Text = ProjectManager.Project.GetRelativePath(ProjectManager.Project.DEMSurveyPath(txtName.Text));
                    break;

                case Purposes.AssociatedSurface:
                    txtRasterPath.Text = ProjectManager.Project.GetRelativePath(((DEMSurvey)ReferenceSurface).AssocSurfacePath(txtName.Text));
                    break;

                case Purposes.ErrorSurface:
                case Purposes.ReferenceErrorSurface:
                    txtRasterPath.Text = ProjectManager.Project.GetRelativePath(ReferenceSurface.ErrorSurfacePath(txtName.Text));
                    break;

                case Purposes.ReferenceSurface:
                    txtRasterPath.Text = ProjectManager.Project.GetRelativePath(ProjectManager.Project.ReferenceSurfacePath(txtName.Text));
                    break;
            }
        }

        private void cmdOK_Click(Object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                DialogResult = DialogResult.None;
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
                txtName.Select();
                return false;
            }

            switch (Purpose)
            {
                case Purposes.SubsequentDEM:
                    if (!ProjectManager.Project.IsDEMNameUnique(txtName.Text, null))
                    {
                        MessageBox.Show(string.Format("There is already another DEM survey in this project with the name '{0}'. Each DEM Survey must have a unique name.", txtName.Text), Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtName.Select();
                        return false;
                    }
                    break;

                case Purposes.AssociatedSurface:
                    if (!((DEMSurvey)ReferenceSurface).IsAssocNameUnique(txtName.Text, null))
                    {
                        MessageBox.Show(string.Format("There is already another associated surface for this DEM with the name '{0}'. The associated surfaces for each DEM must have a unique name.", txtName.Text), Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtName.Select();
                        return false;
                    }
                    break;

                case Purposes.ErrorSurface:
                case Purposes.ReferenceErrorSurface:
                    if (!ReferenceSurface.IsErrorNameUnique(txtName.Text, null))
                    {
                        string parentType = "Reference Surface";
                        if (ReferenceSurface is DEMSurvey)
                            parentType = "DEM Survey";

                        MessageBox.Show(string.Format("There is already another error surface for this {0} with the name '{1}'. The error surfaces for each {0} must have a unique name.", parentType, txtName.Text), Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtName.Select();
                        return false;
                    }
                    break;

                case Purposes.ReferenceSurface:
                    if (!ProjectManager.Project.IsReferenceSurfaceNameUnique(txtName.Text, null))
                    {
                        MessageBox.Show(string.Format("There is already another reference surface with the name '{0}'. Each reference surface must have a unique name.", txtName.Text), Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtName.Select();
                        return false;
                    }
                    break;
            }


            NeedsForcedProjection = false;

            // Importing rasters into GCD projects requires unit checks
            if (!GISDatasetValidation.DSSpatialRefMatchesProject(SourceRaster))
            {
                string msg = string.Format(
                    "{0}{1}{0}If you believe that these projections are the same (or equivalent) choose \"Yes\" to continue anyway. Otherwise choose \"No\" to abort.",
                    Environment.NewLine, GISDatasetValidation.SpatialRefNoMatchString(SourceRaster, "raster", "rasters"));

                DialogResult result = MessageBox.Show(msg, Properties.Resources.ApplicationNameLong, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                if (result == DialogResult.No)
                {
                    NeedsForcedProjection = false;
                    return false;
                }
                else
                {
                    NeedsForcedProjection = true;
                }
            }

            // TODO: This check appears to be VERY similar to the next block of code. Research and simplify if possible.
            if (!GISDatasetValidation.DSHorizUnitsMatchProject(SourceRaster, "raster", "rasters"))
            {
                return false;
            }

            // Verify that the horizontal units match those of the project.
            if (ProjectManager.Project.Units.HorizUnit != SourceRaster.Proj.HorizontalUnit)
            {
                string msg = string.Format("The horizontal units of the raster ({0}) do not match those of the GCD project ({1}).", SourceRaster.Proj.HorizontalUnit.ToString(), ProjectManager.Project.Units.HorizUnit.ToString());
                if (ProjectManager.Project.DEMSurveys.Count < 1)
                {
                    msg += " You can change the GCD project horizontal units by canceling this form and opening the GCD project properties form.";
                }
                MessageBox.Show(msg, "HorizontalUnits Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            // Verify the optional vertical units (if they are specified) for rasters that should share the project vertical units
            if (Purpose == Purposes.FirstDEM || Purpose == Purposes.SubsequentDEM)
            {
                if (SourceRaster.VerticalUnits != UnitsNet.Units.LengthUnit.Undefined)
                {
                    if (SourceRaster.VerticalUnits != ProjectManager.Project.Units.VertUnit)
                    {
                        MessageBox.Show(string.Format("The raster has different vertical units ({0}) than the GCD project ({1})." + " You must change the vertical units of the raster before it can be used within the GCD.", SourceRaster.VerticalUnits.ToString(), ProjectManager.Project.Units.VertUnit.ToString()), "Vertical Units Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
            }


            if (string.IsNullOrEmpty(txtRasterPath.Text))
            {
                //if (ExtImporter.Purpose == ExtentImporter.Purposes.Standalone)
                //{
                //    MessageBox.Show("The output raster path cannot be empty. Click the Save button to specify an output raster path.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                //else
                //{
                MessageBox.Show("The output raster path cannot be empty. Try using a different name.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                return false;
            }
            else
            {
                System.IO.FileInfo outputPath = new System.IO.FileInfo(txtRasterPath.Text);
                if (ProjectManager.Project != null)
                    outputPath = ProjectManager.Project.GetAbsolutePath(txtRasterPath.Text);

                if (outputPath.Exists)
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

            return true;
        }

        public Raster ProcessRaster()
        {
            Cursor = Cursors.WaitCursor;

            Raster gResult = null;
            System.IO.FileInfo fiOutput = ProjectManager.Project.GetAbsolutePath(txtRasterPath.Text);

            if (fiOutput.Exists)
            {
                Exception ex = new Exception("The raster path already exists.");
                ex.Data.Add("Raster path", txtRasterPath.Text);
                throw ex;
            }

            fiOutput.Directory.Create();

            if (ExtImporter.RequiresResampling)
            {
                gResult = RasterOperators.BilinearResample(SourceRaster, fiOutput, ExtImporter.OutExtent, ProjectManager.OnProgressChange);
            }
            else
            {
                if (SourceRaster.Extent.Equals(ExtImporter.OutExtent))
                {
                    // Output extent is same as original raster. Simple dataset copy
                    if (SourceRaster.driver == Raster.RasterDriver.GTiff)
                    {
                        SourceRaster.Copy(fiOutput);
                        gResult = new Raster(fiOutput);
                    }
                    else
                    {
                        gResult = RasterOperators.ExtendedCopy(SourceRaster, fiOutput, ProjectManager.OnProgressChange);
                    }
                }
                else
                {
                    // Output extent differs from original raster. Use extended copy
                    gResult = RasterOperators.ExtendedCopy(SourceRaster, fiOutput, ExtImporter.OutExtent, ProjectManager.OnProgressChange);
                }
            }

            // This method will check to see if pyrmaids are need and then build if necessary.
            PerformRasterPyramids(new System.IO.FileInfo(txtRasterPath.Text));

            if (Purpose == Purposes.FirstDEM || Purpose == Purposes.SubsequentDEM || Purpose == Purposes.ReferenceSurface)
            {
                // Now try the hillshade for DEM Surveys and reference surfaces
                System.IO.FileInfo sHillshadePath = Surface.HillShadeRasterPath(fiOutput);
                RasterOperators.Hillshade(gResult, sHillshadePath, ProjectManager.OnProgressChange);
                ProjectManager.PyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.Hillshade, sHillshadePath);
            }


            Cursor = Cursors.Default;

            Projection projRef = GISDatasetValidation.GetProjectProjection();
            if (projRef != null && NeedsForcedProjection)
                gResult.SetProjection(projRef);

            return gResult;
        }

        /// <summary>
        /// Disable typing in the original raster extent text boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Cannot change forecolor of textboxes when they are readonly. So make them
        /// ReadOnly = False but skip any key pressing.</remarks>
        private void OriginalExtentTextBoxes_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cmdHelpPrecision_Click(Object sender, EventArgs e)
        {
            UtilityForms.frmInformation frm = new UtilityForms.frmInformation();
            frm.InitializeFixedDialog("Horizontal Decimal Precision", Properties.Resources.PrecisionHelp);
            frm.ShowDialog();
        }

        private void PerformRasterPyramids(System.IO.FileInfo sRasterPath)
        {
            RasterPyramidManager.PyramidRasterTypes ePyramidRasterType = default(RasterPyramidManager.PyramidRasterTypes);
            switch (Purpose)
            {
                case Purposes.FirstDEM:
                case Purposes.SubsequentDEM:
                    ePyramidRasterType = RasterPyramidManager.PyramidRasterTypes.DEM;
                    break;

                case Purposes.AssociatedSurface:
                    ePyramidRasterType = RasterPyramidManager.PyramidRasterTypes.AssociatedSurfaces;
                    break;
            }

            ProjectManager.PyramidManager.PerformRasterPyramids(ePyramidRasterType, sRasterPath);
        }

        private void cmdHelp_Click(object sender, EventArgs e)
        {
            string helpKey;
            switch (Purpose)
            {
                case Purposes.AssociatedSurface:
                    helpKey = "AssocSurface";
                    break;

                case Purposes.ErrorSurface:
                    helpKey = "ErrorSurface";
                    break;

                case Purposes.ReferenceErrorSurface:
                    helpKey = "ReferenceSurface";
                    break;

                case Purposes.FirstDEM:
                case Purposes.SubsequentDEM:
                default:
                    helpKey = "NewDEM";
                    break;
            }

            OnlineHelp.Show(helpKey);
        }
    }
}
