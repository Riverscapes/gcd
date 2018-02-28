using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using GCDCore.Project;
using GCDConsoleLib;
using GCDCore.ErrorCalculation.FIS;

namespace GCDCore.UserInterface.SurveyLibrary
{
    public partial class frmErrorSurfaceProperties : Form, IProjectItemForm
    {
        public readonly DEMSurvey DEM;
        public ErrorSurface ErrorSurf { get; internal set; }

        public const string m_sEntireDEMExtent = "Entire DEM Extent";

        public GCDProjectItem GCDProjectItem {  get { return ErrorSurf; } }

        // This dictionary stores the definitions of the error surface properties for each survey method polygon
        private naru.ui.SortableBindingList<ErrorSurfaceProperty> ErrorCalcProps;

        // The item bound to the selected row on the left grid
        private ErrorSurfaceProperty SelectedErrProp { get { return (ErrorSurfaceProperty)grdErrorProperties.SelectedRows[0].DataBoundItem; } }

        public frmErrorSurfaceProperties(DEMSurvey dem, ErrorSurface errorSurf)
        {
            InitializeComponent();
            DEM = dem;
            ErrorSurf = errorSurf;
        }

        private void frmErrorSurfaceProperties_Load(System.Object sender, System.EventArgs e)
        {
            tTip.SetToolTip(txtName, "The name for the error surface. The name must be unique for the DEM survey.");
            tTip.SetToolTip(txtRasterPath, "The path of the error surface raster within the GCD project.");
            tTip.SetToolTip(grdErrorProperties, "List of the survey methods obtained from the DEM Survey polygon feature class. The error surface properties for each survey method are show on the right.");
            tTip.SetToolTip(rdoUniform, "Choose this option to specify a constant, uniform error surface value for the selected survey method.");
            tTip.SetToolTip(valUniform, "The constant, uniform error surface value for the selected survey method. Must be a positive value");
            tTip.SetToolTip(rdoFIS, "Choose this option to specify an FIS error surface for the selected survey method.");
            tTip.SetToolTip(cboFIS, "Species the FIS rule file from the GCD FIS Library to use for the selected survey method.");
            tTip.SetToolTip(grdFISInputs, "Specify an associated surface for each FIS input for the selected FIS rule file and for the selected survey method.");

            grdErrorProperties.AutoGenerateColumns = false;
            grdErrorProperties.AllowUserToResizeRows = false;
            grdFISInputs.AutoGenerateColumns = false;
            grdFISInputs.AllowUserToResizeRows = false;

            // Load the survey methods on the left and then populate the right side of the window.
            LoadSurveyMethods();
            grdErrorProperties.DataSource = ErrorCalcProps;

            // Load all the FIS rule files in the library to the combobox
            // (Need to do this before the try/catch below that loads the error surface data
            // Also turn of event handling or this sets the error props to FIS and clears props
            cboFIS.SelectedIndexChanged -= cboFIS_SelectedIndexChanged;
            cboFIS.DataSource = ProjectManager.FISLibrary;
            cboFIS.SelectedIndexChanged += cboFIS_SelectedIndexChanged;

            // Prepare the associated surface dropdown with all surfaces from the dEM
            cboAssociated.DataSource = new BindingList<AssocSurface>(DEM.AssocSurfaces);

            // Load all the associated surfaces in the survey library to the grid combo box
            DataGridViewComboBoxColumn colCombo = (DataGridViewComboBoxColumn)grdFISInputs.Columns[1];
            colCombo.DataSource = new BindingList<AssocSurface>(DEM.AssocSurfaces);
            colCombo.DisplayMember = "Name";
            colCombo.ValueMember = "This"; // needed to support binding column to complex object

            if (ErrorSurf is ErrorSurface)
            {
                // Existing error surface. Disable editing.
                txtName.Text = ErrorSurf.Name;
                txtRasterPath.Text = ProjectManager.Project.GetRelativePath(ErrorSurf.Raster.GISFileInfo);
                chkIsDefault.Checked = ErrorSurf.IsDefault;
                chkIsDefault.Enabled = !ErrorSurf.IsDefault; // cannot uncheck default... only set another surface as default
                btnOK.Text = "Save";
            }
            else
            {
                // The first error surface is always the default.
                chkIsDefault.Checked = DEM.ErrorSurfaces.Count == 0;
                chkIsDefault.Enabled = !chkIsDefault.Checked;
            }

            try
            {

                //LoadErrorCalculationMethods();

                // Need to force the error properties to update and reflect the contents of the left side of the form
                //UpdateGridWithErrorProperties(0);

                // Update which controls are enabled.
                UpdateControls();

            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }

            // Disable the associated error surface option if in readonly mode or else
            // there are no associated error surfaces for the DEM survey
            rdoAssociated.Enabled = ErrorSurf == null && cboAssociated.Items.Count > 0;
            rdoFIS.Enabled = !(ErrorSurf != null || DEM.AssocSurfaces.Count < 2);

            try
            {
                // Safely retrieve the spatial units of the DEM
                rdoUniform.Text = string.Format("{0} ({1})", rdoUniform.Text, UnitsNet.Length.GetAbbreviation(ProjectManager.Project.Units.VertUnit));
            }
            catch (Exception ex)
            {
                // Don't show an error in release mode
                System.Diagnostics.Debug.Assert(false, "Error retrieving linear units from DEM: " + ex.Message);
            }
        }

        /// <summary>
        /// Loads the survey methods into the member dictionary.
        /// </summary>
        /// <remarks>Note that another method is responsible for displaying the dictionary contents in the UI</remarks>
        private void LoadSurveyMethods()
        {
            // Always create a new dictionary, which will clear any existing entries

            // Attempt to load the survey methods from an existing error surface if it exists
            if (ErrorSurf == null)
            {
                ErrorCalcProps = new naru.ui.SortableBindingList<ErrorSurfaceProperty>();

                if (DEM.IsSingleSurveyMethod)
                {
                    // Single method, see if the survey method has a default error value in the GCD software SurveyTypes XML file
                    decimal uniformValue = 0;
                    if (!string.IsNullOrEmpty(DEM.SurveyMethod) && ProjectManager.SurveyTypes.ContainsKey(DEM.SurveyMethod))
                    {
                        uniformValue = ProjectManager.SurveyTypes[DEM.SurveyMethod].ErrorValue;
                    }

                    ErrorSurfaceProperty errProp = new ErrorSurfaceProperty(m_sEntireDEMExtent);
                    errProp.UniformValue = uniformValue;
                    ErrorCalcProps.Add(errProp);
                }
                else
                {
                    // Multi-method - load distinct survey types from method mask field in ShapeFile
                    Vector polygonMask = new Vector(DEM.MethodMask);
                    foreach (VectorFeature feature in polygonMask.Features.Values)
                    {
                        string maskValue = feature.GetFieldAsString(DEM.MethodMaskField);
                        if (!string.IsNullOrEmpty(maskValue) && ErrorCalcProps.Count<ErrorSurfaceProperty>(x => string.Compare(x.Name, maskValue, true) == 0) == 0)
                        {
                            decimal uniformValue = 0;
                            if (ProjectManager.SurveyTypes.ContainsKey(maskValue))
                            {
                                uniformValue = ProjectManager.SurveyTypes[maskValue].ErrorValue;
                            }
                            ErrorSurfaceProperty errProp = new ErrorSurfaceProperty(maskValue);
                            errProp.UniformValue = uniformValue;
                            ErrorCalcProps.Add(errProp);
                        }
                    }
                }
            }
            else
            {
                ErrorCalcProps = new naru.ui.SortableBindingList<ErrorSurfaceProperty>(ErrorSurf.ErrorProperties.Values.ToList<ErrorSurfaceProperty>());
            }
        }

        /// <summary>
        /// Loads the contents of the member dictionary into the user interface
        /// </summary>
        /// <remarks></remarks>
        //private void LoadErrorCalculationMethods()
        //{
        //    foreach (ErrorSurfaceProperty errProps in ErrorCalcProps.Values)
        //    {
        //        int nMethodRow = grdErrorProperties.Rows.Add();
        //        grdErrorProperties.Rows[nMethodRow].Cells[0].Value = errProps.Name;

        //        if (errProps.UniformValue.HasValue)
        //        {
        //            grdErrorProperties.Rows[nMethodRow].Cells[1].Value = "Uniform Error";
        //        }
        //        else if (object.ReferenceEquals(errProps.AssociatedSurface, AssociatedSurface))
        //        {
        //            grdErrorProperties.Rows[nMethodRow].Cells[1].Value = "Associated Surface";
        //        }
        //        else
        //        {
        //            grdErrorProperties.Rows[nMethodRow].Cells[1].Value = "FIS Error";
        //        }
        //    }

        //    // Now select the first row in the grid. this will automatically update the right hand panel
        //    if (grdErrorProperties.Rows.Count > 0)
        //    {
        //        grdErrorProperties.Rows[0].Selected = true;
        //    }
        //}

        /// <summary>
        /// Retrieve the default error value for a particular survey type from the survey types library
        /// </summary>
        /// <param name="sMethod"></param>
        /// <returns>Default error value or zero if the method is not found</returns>
        /// <remarks></remarks>
        //private double GetDefaultErrorValue(string sMethod)
        //{
        //    if (ProjectManager.SurveyTypes.ContainsKey(sMethod))
        //    {
        //        return ProjectManager.SurveyTypes[sMethod].ErrorValue;
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}

        /// <summary>
        /// When the user clicks on any cell in the left grid, the right grid should be updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Remember to save the current settings before updating</remarks>
        private void grdErrorProperties_CellClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            //    UpdateGridWithErrorProperties(e.RowIndex);
        }

        private void grdErrorProperties_CellLeave(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            //    SaveErrorProperties();
        }

        /// <summary>
        /// Update the FIS inputs grid based on the currently selected survey method
        /// </summary>
        /// <param name="nNewRow"></param>
        /// <remarks></remarks>
        //private void UpdateGridWithErrorProperties(int nNewRow)
        //{
        //    grdFISInputs.Rows.Clear();
        //    string sMethod = grdErrorProperties.Rows[nNewRow].Cells[0].Value.ToString();
        //    if (!string.IsNullOrEmpty(sMethod))
        //    {
        //        if (ErrorCalcProps.ContainsKey(sMethod))
        //        {
        //            // Only proceed and load anything into the FIS inputs grid if the error surface for this survey method is a FIS
        //            ErrorSurfaceProperty prop = ErrorCalcProps[sMethod];
        //            if (prop.UniformValue.HasValue)
        //            {
        //                rdoUniform.Checked = true;
        //                valUniform.Value = (decimal)prop.UniformValue.Value;

        //                cboFIS.SelectedIndex = -1;
        //                cboAssociated.SelectedIndex = -1;
        //            }
        //            else if (prop.AssociatedSurface is AssocSurface)
        //            {
        //                rdoAssociated.Checked = true;
        //                cboFIS.SelectedIndex = -1;
        //                cboAssociated.SelectedItem = prop.AssociatedSurface;
        //            }
        //            else
        //            {
        //                rdoFIS.Checked = true;
        //                cboAssociated.SelectedIndex = -1;

        //                for (int i = 0; i <= cboFIS.Items.Count - 1; i++)
        //                {
        //                    if (string.Compare(((naru.db.NamedObject)cboFIS.Items[i]).Name, prop.FISRuleFile.FullName, true) == 0)
        //                    {
        //                        cboFIS.SelectedIndex = i;
        //                        break;
        //                    }
        //                }

        //                // Force the FIS grid to update (including retrieval of the FIS properties)
        //                UpdateFISGrid();
        //            }
        //        }
        //    }

        //    cboAssociated.Enabled = rdoAssociated.Enabled && rdoAssociated.Checked;
        //}

        /// <summary>
        /// Update which error surface controls are available based on the currently selected survey method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        private void rdoUniform_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            UpdateControls();
        }

        /// <summary>
        /// Update which error surface controls are available based on the currently selected survey method
        /// </summary>
        /// <remarks></remarks>
        private void UpdateControls()
        {
            if (rdoUniform.Checked)
                ((ErrorSurfaceProperty)grdErrorProperties.SelectedRows[0].DataBoundItem).UniformValue = valUniform.Value;
            else if (rdoAssociated.Checked)
            {
                if (cboAssociated.SelectedIndex < 0)
                    cboAssociated.SelectedIndex = 0;
            }
            else if (rdoFIS.Checked)
            {
                if (cboFIS.SelectedIndex < 0)
                    cboFIS.SelectedIndex = 0;

                if (DEM.AssocSurfaces.Count < 1)
                {
                    MessageBox.Show("You cannot create a FIS error surface until you define at least 2 associated surfaces for this DEM survey.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rdoUniform.Checked = true;
                }
            }

            // Force a refresh of the binding list so that the correct error calc method name is displayed beside the survey method on the left
            ErrorCalcProps.ResetBindings();

            if (ErrorSurf is ErrorSurface)
            {
                // Existing error surface. Disable editing.
                rdoUniform.Enabled = false;
                valUniform.Enabled = false;

                rdoFIS.Enabled = false;
                cboFIS.Enabled = false;
                grdFISInputs.Enabled = false;

                rdoAssociated.Enabled = false;
                cboAssociated.Enabled = false;
            }
            else
            {
                valUniform.Enabled = rdoUniform.Checked;
                cboFIS.Enabled = rdoFIS.Checked;
                grdFISInputs.Enabled = rdoFIS.Checked;
                cboAssociated.Enabled = rdoAssociated.Checked;
            }

            //// Need to change the left, survey methods grid
            //DataGridViewSelectedRowCollection r = grdErrorProperties.SelectedRows;
            //if (r.Count == 1)
            //{
            //    string sType = "Uniform Error";
            //    if (rdoAssociated.Checked)
            //    {
            //        sType = string.Format("Associated Surface");
            //    }
            //    else if (rdoFIS.Checked)
            //    {
            //        sType = "FIS Error";
            //    }
            //    r[0].Cells[1].Value = sType;
            //}
        }

        private void cboFIS_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            if (!rdoFIS.Checked)
            {
                grdFISInputs.DataSource = null;
                return;
            }

            ErrorSurfaceProperty selectedErrorProp = (ErrorSurfaceProperty)grdErrorProperties.SelectedRows[0].DataBoundItem;

            if (!rdoFIS.Checked)
            {
                selectedErrorProp.FISRuleFile = null;
                selectedErrorProp.FISInputs.Clear();
                return;
            }

            if (cboFIS.SelectedItem == null)
                return;

            FISRuleFile selectedFIS = new FISRuleFile(((FISLibraryItem)cboFIS.SelectedItem).FilePath);

            // Detect if this is already the identified FIS
            if (selectedErrorProp.FISRuleFile is System.IO.FileInfo && string.Compare(selectedErrorProp.FISRuleFile.FullName, selectedFIS.RuleFilePath.FullName, true) == 0)
                return;

            // Load the inputs for the newly selected FIS rule file into the error properties
            selectedErrorProp.FISRuleFile = selectedFIS.RuleFilePath;
            selectedErrorProp.FISInputs.Clear();
            foreach (string input in selectedFIS.FISInputs)
                selectedErrorProp.FISInputs.Add(new FISInput(input));

            grdFISInputs.DataSource = selectedErrorProp.FISInputs;
        }

        /// <summary>
        /// Change which FIS inputs are listed on the right side of the form
        /// </summary>
        /// <remarks></remarks>
        //private void UpdateFISGrid()
        //{
        //    grdFISInputs.DataSource = null;

        //    // Only load FIS inputs if this region of DEM is FIS and there's a FIS rule file selected
        //    if (!(rdoFIS.Checked && cboFIS.SelectedItem is FISRuleFile))
        //    {
        //        return;
        //    }





        //    ErrorCalculation.FIS.FISRuleFile theFISRuleFile = new ErrorCalculation.FIS.FISRuleFile(((ErrorCalculation.FIS.FISLibraryItem)cboFIS.SelectedItem).FilePath);

        //    // Loop over all the inputs defined for the FIS
        //    foreach (string sInput in theFISRuleFile.FISInputs)
        //    {
        //        int nRow = grdFISInputs.Rows.Add();
        //        grdFISInputs.Rows[nRow].Cells[0].Value = sInput;

        //        // Get the selected error properties row
        //        DataGridViewSelectedRowCollection lErr = grdErrorProperties.SelectedRows;
        //        if (lErr.Count == 1)
        //        {
        //            ErrorSurfaceProperty errProps = ErrorCalcProps[lErr[0].Cells[0].Value.ToString()];

        //            // Only proceed if the error surface definition is a FIS
        //            if ((errProps.FISInputs != null))
        //            {
        //                // loop over all the defined FIS inputs for the error surface
        //                foreach (string sDefinedInput in errProps.FISInputs.Keys)
        //                {
        //                    if (string.Compare(sInput, sDefinedInput, true) == 0)
        //                    {
        //                        // this is a FIS input that has a definition already
        //                        grdFISInputs.Rows[nRow].Cells[1].Value = errProps.FISInputs[sDefinedInput];
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Save the error surface properties for the selected survey method
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool SaveErrorProperties()
        {
            //DataGridViewSelectedRowCollection r = grdErrorProperties.SelectedRows;
            //if (r.Count != 1)
            //{
            //    return true;
            //}

            //// Save just the survey method that is selected in the left grid
            //string sSurveyMethod = r[0].Cells[0].Value.ToString();
            //string sErrorType = r[0].Cells[1].Value.ToString();

            //if (rdoUniform.Checked)
            //{
            //    Create a new Uniform error properties
            //    ErrorCalcProps[sSurveyMethod] = new ErrorSurfaceProperty(sSurveyMethod, (double)valUniform.Value);
            //    sErrorType = "Uniform Error";
            //}
            //else if (rdoAssociated.Checked)
            //{
            //    if (cboAssociated.SelectedIndex >= 0)
            //    {
            //        Create a new associated surface error properties
            //       ErrorCalcProps[sSurveyMethod] = new ErrorSurfaceProperty(sSurveyMethod, (AssocSurface)cboAssociated.SelectedItem);
            //        sErrorType = "Associated Surface";
            //    }
            //    else
            //    {
            //        MessageBox.Show("You must select an associated surface that contains the error values for this error surface.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return false;
            //    }
            //}
            //else
            //{
            //    Make sure the user has selected a GIS input
            //    if (cboFIS.SelectedIndex < 0)
            //    {
            //        MessageBox.Show("You must select a FIS rule file for this survey method or define it as uniform error.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return false;
            //    }
            //    else
            //    {
            //        FIS.Loop through all the FIS inputs
            //        Dictionary<string, AssocSurface> dInputs = new Dictionary<string, AssocSurface>();
            //        for (int i = 0; i <= grdFISInputs.Rows.Count - 1; i++)
            //        {
            //            DataGridViewComboBoxCell cboAssoc = (DataGridViewComboBoxCell)grdFISInputs.Rows[i].Cells[1];
            //            if (cboAssoc.Value != null)
            //            {
            //                dInputs[grdFISInputs.Rows[i].Cells[0].Value.ToString()] = (AssocSurface)cboAssoc.Value;
            //            }
            //            else
            //            {
            //                MessageBox.Show("You must choose an associated surface for each FIS input.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                return false;
            //            }
            //        }

            //        Find the matching fis library file
            //        ErrorCalcProps[sSurveyMethod] = new ErrorSurfaceProperty(sSurveyMethod, ((ErrorCalculation.FIS.FISLibraryItem)cboFIS.SelectedItem).FilePath, dInputs);
            //        sErrorType = "FIS Error";
            //    }
            //}

            //r[0].Cells[1].Value = sErrorType;

            return true;
        }

        /// <summary>
        /// The error surface name has changed. Update the output raster path.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        private void txtName_TextChanged(System.Object sender, System.EventArgs e)
        {
            if (ErrorSurf == null)
            {
                string sRasterPath = string.Empty;
                if (!string.IsNullOrEmpty(txtName.Text))
                {
                    sRasterPath = ProjectManager.Project.GetRelativePath(DEM.ErrorSurfacePath(txtName.Text));
                }
                txtRasterPath.Text = sRasterPath;
            }
        }

        private void btnHelp_Click(System.Object sender, System.EventArgs e)
        {
            System.Diagnostics.Process.Start(GCDCore.Properties.Resources.HelpBaseURL + "gcd-command-reference/gcd-project-explorer/g-error-surfaces-context-menu/ii-derive-error-surface");
        }

        private bool ValidateForm()
        {
            // Sanity check to avoid empty string names
            txtName.Text = txtName.Text.Trim();

            if ((string.IsNullOrEmpty(txtName.Text.Trim())))
            {
                MessageBox.Show("You must provide a unique name for the error surface.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Select();
                return false;
            }
            else
            {
                if (!DEM.IsErrorNameUnique(txtName.Text, ErrorSurf))
                {
                    MessageBox.Show("There is another error surface for this DEM Survey that already possesses this name. You must provide a unique name for the error surface.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtName.Select(txtName.Text.Length, 0);
                    return false;
                }
            }

            foreach (ErrorSurfaceProperty prop in ErrorCalcProps)
            {
                if (prop.UniformValue.HasValue)
                {
                    if (prop.UniformValue.Value <= 0)
                    {
                        string sMessage;
                        if (ErrorCalcProps.Count == 1)
                            sMessage = "The uniform error value must be greater than zero.";
                        else
                            sMessage = "The uniform error value for one or more regions are zero. All uniform error values must be greater than zero.";
                        MessageBox.Show(sMessage, "Zero Uniform Error Value", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }

                if (prop.FISInputs != null)
                {
                    if (prop.FISInputs.Count<FISInput>(x => x.AssociatedSurface == null) > 0)
                    {
                        MessageBox.Show("One or more FIS inputs have not been assigned to an associated surface.", "Unassigned FIS Inputs", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        grdFISInputs.Select();
                        return false;
                    }
                }
            }

            return true;
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            // Need to save the current error properties first.
            if (!SaveErrorProperties())
            {
                this.DialogResult = DialogResult.None;
                return;
            }

            if (!ValidateForm())
            {
                this.DialogResult = DialogResult.None;
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            if (chkIsDefault.Enabled && chkIsDefault.Checked)
            {
                // If this error surface is set to default then ensure that all other error surfaces for this DEM Are not set to default.
                // Note that UI should disable this checkbox if this is the only error surface and the checkbox should already be checked.
                DEM.ErrorSurfaces.Where(x => x.IsDefault && !x.Equals(ErrorSurf)).ToList<ErrorSurface>().ForEach(x => x.IsDefault = false);
            }

            if (ErrorSurf is ErrorSurface)
            {
                ErrorSurf.Name = txtName.Text;
                ErrorSurf.IsDefault = chkIsDefault.Checked;
            }
            else
            {
                try
                {
                    // Create the new error surface object and the folder that will store the raster
                    Dictionary<string, ErrorSurfaceProperty> dProps = new Dictionary<string, ErrorSurfaceProperty>();
                    foreach (ErrorSurfaceProperty prop in ErrorCalcProps)
                        dProps[prop.Name] = prop;

                    ErrorSurf = new ErrorSurface(txtName.Text, ProjectManager.Project.GetAbsolutePath(txtRasterPath.Text), DEM, chkIsDefault.Checked, dProps);
                    ErrorSurf.Raster.GISFileInfo.Directory.Create();

                    if (ErrorCalcProps.Count == 1)
                    {
                        // Single method survey
                        if (ErrorCalcProps[0].AssociatedSurface is AssocSurface)
                        {
                            // Do nothing. Single method survey and the error surfce is simply pointing to an associated surface.
                        }
                        else
                        {
                            // Perform the single method error raster generation.
                            RasterOperators.CreateErrorRaster(DEM.Raster, ErrorCalcProps[0].SingleErrorRasterProperty, ErrorSurf.Raster.GISFileInfo);
                        }
                    }
                    else
                    {
                        // Transform all the GCD project ErrorSurfaceProperty into the ErrorRasterProperty needed for raster processing
                        Dictionary<string, GCDConsoleLib.GCD.ErrorRasterProperties> multiProps = new Dictionary<string, GCDConsoleLib.GCD.ErrorRasterProperties>();
                        foreach (ErrorSurfaceProperty errProp in ErrorCalcProps)
                            multiProps[errProp.Name] = errProp.SingleErrorRasterProperty;

                        // Multi-method error surface.
                        Vector polygonMask = new Vector(DEM.MethodMask);
                        RasterOperators.CreateErrorRaster(DEM.Raster, polygonMask, DEM.MethodMaskField, multiProps, ErrorSurf.Raster.GISFileInfo);
                    }

                    // Add the error surface to the project now that processing is complete
                    DEM.ErrorSurfaces.Add(ErrorSurf);
                }
                catch (Exception ex)
                {
                    naru.error.ExceptionUI.HandleException(ex);
                }
            }

            ProjectManager.Project.Save();
            Cursor.Current = Cursors.Default;
        }

        private void grdFISInputs_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            System.Diagnostics.Debug.Print(e.Exception.Message);
        }

        private void grdErrorProperties_SelectionChanged(object sender, EventArgs e)
        {
            if (grdErrorProperties.SelectedRows.Count < 1)
                return;

            rdoUniform.Checked = SelectedErrProp.UniformValue.HasValue;
            valUniform.ValueChanged -= valUniform_ValueChanged;
            if (SelectedErrProp.UniformValue.HasValue)
                valUniform.Value = (decimal)SelectedErrProp.UniformValue.Value;
            else
                valUniform.Value = 0;
            valUniform.ValueChanged += valUniform_ValueChanged;

            cboAssociated.SelectedItem = SelectedErrProp.AssociatedSurface;

            if (SelectedErrProp.FISRuleFile is System.IO.FileInfo)
            {
                if (ProjectManager.FISLibrary.Count<FISLibraryItem>(x => string.Compare(x.FilePath.FullName, SelectedErrProp.FISRuleFile.FullName, true) == 0) > 0)
                {
                    cboFIS.SelectedItem = ProjectManager.FISLibrary.First<FISLibraryItem>(x => string.Compare(x.FilePath.FullName, SelectedErrProp.FISRuleFile.FullName, true) == 0);
                }
            }
            else
            {
                cboFIS.SelectedItem = null;
            }
        }

        private void valUniform_ValueChanged(object sender, EventArgs e)
        {
            ((ErrorSurfaceProperty)grdErrorProperties.SelectedRows[0].DataBoundItem).UniformValue = valUniform.Value;
        }

        private void cboAssociated_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdoAssociated.Checked)
                ((ErrorSurfaceProperty)grdErrorProperties.SelectedRows[0].DataBoundItem).AssociatedSurface = (AssocSurface)cboAssociated.SelectedItem;
        }
    }
}