using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using GCDCore.Project;

namespace GCDCore.UserInterface.SurveyLibrary
{
    public partial class frmErrorSurfaceProperties : Form
    {
        public readonly DEMSurvey DEM;
        public ErrorSurface ErrorSurf { get; internal set; }

        private const string m_sEntireDEMExtent = "Entire DEM Extent";
    
        // This dictionary stores the definitions of the error surface properties for each survey method polygon

        private Dictionary<string, ErrorCalculation.ErrorCalcPropertiesBase> ErrorCalcProps;
   

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

            grdErrorProperties.AllowUserToResizeRows = false;
            grdFISInputs.AllowUserToResizeRows = false;

            // Load all the FIS rule files in the library to the combobox
            // (Need to do this before the try/catch below that loads the error surface data
            foreach (FIS.FISLibraryItem fis in ProjectManager.FISLibrary)
            {
                cboFIS.Items.Add(fis);
            }

            if (ErrorSurf is ErrorSurface)
            {
                // Existing error surface. Disable editing.
                txtName.Text = ErrorSurf.Name;
                txtRasterPath.Text = ProjectManager.Project.GetRelativePath(ErrorSurf.Raster.GISFileInfo);
                btnOK.Text = "Save";
            }

            try
            {
                // Load the survey methods on the left and then populate the right side of the window.
                LoadSurveyMethods();
                LoadErrorCalculationMethods();

                // Need to force the error properties to update and reflect the contents of the left side of the form
                UpdateGridWithErrorProperties(0);

                // Update which controls are enabled.
                UpdateControls();
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }

            // Load all the associated surfaces in the survey library to the grid combo box
            DataGridViewComboBoxColumn colCombo = (DataGridViewComboBoxColumn)grdFISInputs.Columns[1];
            colCombo.DataSource = DEM.AssocSurfaces.Values.AsEnumerable();
            colCombo.DisplayMember = "Name";

            // Also load any error surfaces into the associated error surface combo box.
            cboAssociated.DataSource = DEM.AssocSurfaces.Values;

            // Disable the associated error surface option if in readonly mode or else
            // there are no associated error surfaces for the DEM survey
            rdoAssociated.Enabled = ErrorSurf == null && cboAssociated.Items.Count > 0;
            rdoFIS.Enabled = ErrorSurf == null && DEM.AssocSurfaces.Count < 1;

            try
            {
                // Safely retrieve the spatial units of the DEM
                rdoUniform.Text = string.Format("{0} ({1})", rdoUniform.Text, UnitsNet.Length.GetAbbreviation(ProjectManager.Project.Units.VertUnit));
            }
            catch (Exception ex)
            {
                // Don't show an error in release mode
                System.Diagnostics.Debug.Assert(false, "Error retrieving linear units from DEM");
            }
        }

        /// <summary>
        /// Loads the survey methods into the member dictionary.
        /// </summary>
        /// <remarks>Note that another method is responsible for displaying the dictionary contents in the UI</remarks>

        private void LoadSurveyMethods()
        {
            // Always create a new dictionary, which will clear any existing entries
            ErrorCalcProps = new Dictionary<string, ErrorSurfaceProperty>();

            // Attempt to load the survey methods from an existing error surface if it exists

            if (ErrorSurf == null)
            {
                if (DEM.IsSingleSurveyMethod)
                {
                    // Single method, see if the survey method has a default error value in the GCD software SurveyTypes XML file
                    double uniformValue = 0;
                    if (!string.IsNullOrEmpty(DEM.SurveyMethod) && ProjectManager.SurveyTypes.ContainsKey(DEM.SurveyMethod))
                    {
                        uniformValue = ProjectManager.SurveyTypes[DEM.SurveyMethod].ErrorValue;
                    }
                    ErrorCalcProps[m_sEntireDEMExtent] = new ErrorSurfaceProperty(m_sEntireDEMExtent, uniformValue);
                }
                else
                {
                    // Multi-method - load distinct survey types from method mask field in ShapeFile
                    GCDConsoleLib.Vector polygonMask = new GCDConsoleLib.Vector(DEM.MethodMask);
                    foreach (GCDConsoleLib.VectorFeature feature in polygonMask.Features.Values)
                    {
                        string maskValue = feature.GetFieldAsString(DEM.MethodMaskField);
                        if (!string.IsNullOrEmpty(maskValue) && !ErrorCalcProps.ContainsKey(maskValue))
                        {
                            double uniformValue = 0;
                            if (ProjectManager.SurveyTypes.ContainsKey(maskValue))
                            {
                                uniformValue = ProjectManager.SurveyTypes[maskValue].ErrorValue;
                            }
                            ErrorCalcProps[maskValue] = new ErrorSurfaceProperty(maskValue, uniformValue);
                        }
                    }
                }
            }
            else
            {
                ErrorCalcProps = m_ErrorSurface.ErrorProperties;
            }
        }

        /// <summary>
        /// Loads the contents of the member dictionary into the user interface
        /// </summary>
        /// <remarks></remarks>
        private void LoadErrorCalculationMethods()
        {
            foreach (ErrorSurfaceProperty errProps in ErrorCalcProps.Values)
            {
                int nMethodRow = grdErrorProperties.Rows.Add;
                grdErrorProperties.Rows[nMethodRow].Cells[0].Value = errProps.Name;

                if (errProps.UniformValue.HasValue)
                {
                    grdErrorProperties.Rows[nMethodRow].Cells[1].Value = "Uniform Error";
                }
                else if (object.ReferenceEquals(errProps.AssociatedSurface, AssociatedSurface))
                {
                    grdErrorProperties.Rows[nMethodRow].Cells[1].Value = "Associated Surface";
                }
                else
                {
                    grdErrorProperties.Rows[nMethodRow].Cells[1].Value = "FIS Error";
                }
            }

            // Now select the first row in the grid. this will automatically update the right hand panel
            if (grdErrorProperties.Rows.Count > 0)
            {
                grdErrorProperties.Rows[0].Selected = true;
            }
        }

        /// <summary>
        /// Retrieve the default error value for a particular survey type from the survey types library
        /// </summary>
        /// <param name="sMethod"></param>
        /// <returns>Default error value or zero if the method is not found</returns>
        /// <remarks></remarks>
        private double GetDefaultErrorValue(string sMethod)
        {
            if (ProjectManager.SurveyTypes.ContainsKey(sMethod))
            {
                return ProjectManager.SurveyTypes[sMethod].ErrorValue;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// When the user clicks on any cell in the left grid, the right grid should be updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Remember to save the current settings before updating</remarks>
        private void grdErrorProperties_CellClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            UpdateGridWithErrorProperties(e.RowIndex);
        }

        private void grdErrorProperties_CellLeave(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            SaveErrorProperties();
        }

        /// <summary>
        /// Update the FIS inputs grid based on the currently selected survey method
        /// </summary>
        /// <param name="nNewRow"></param>
        /// <remarks></remarks>
        private void UpdateGridWithErrorProperties(int nNewRow)
        {
            grdFISInputs.Rows.Clear();
            string sMethod = grdErrorProperties.Rows[nNewRow].Cells[0].Value.ToString();
            if (!string.IsNullOrEmpty(sMethod))
            {
                if (ErrorCalcProps.ContainsKey(sMethod))
                {
                    // Only proceed and load anything into the FIS inputs grid if the error surface for this survey method is a FIS
                    ErrorSurfaceProperty prop = ErrorCalcProps[sMethod];
                    if (prop.UniformValue.HasValue)
                    {
                        rdoUniform.Checked = true;
                        valUniform.Value = prop.UniformValue.Value;

                        cboFIS.SelectedIndex = -1;
                        cboAssociated.SelectedIndex = -1;
                    }
                    else if (object.ReferenceEquals(prop.AssociatedSurface, AssociatedSurface))
                    {
                        rdoAssociated.Checked = true;
                        cboFIS.SelectedIndex = -1;
                        cboAssociated.SelectedItem = prop.AssociatedSurface;
                    }
                    else
                    {
                        rdoFIS.Checked = true;
                        cboAssociated.SelectedIndex = -1;

                        for (int i = 0; i <= cboFIS.Items.Count - 1; i++)
                        {
                            if (string.Compare(((naru.db.NamedObject)cboFIS.Items[i]).Name, prop.FISRuleFile.FullName, true) == 0)
                            {
                                cboFIS.SelectedIndex = i;
                                break; // TODO: might not be correct. Was : Exit For
                            }
                        }

                        // Force the FIS grid to update (including retrieval of the FIS properties)
                        UpdateFISGrid();
                    }
                }
            }

            cboAssociated.Enabled = rdoAssociated.Enabled && rdoAssociated.Checked;
        }

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
            if (rdoFIS.Checked)
            {
                if (DEM.AssocSurfaces.Count < 1)
                {
                    MessageBox.Show("You cannot create a FIS error surface until you define at least 2 associated surfaces for this DEM survey.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rdoUniform.Checked = true;
                }
            }

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

            // Need to change the left, survey methods grid
            DataGridViewSelectedRowCollection r = grdErrorProperties.SelectedRows;
            if (r.Count == 1)
            {
                string sType = "Uniform Error";
                if (rdoAssociated.Checked)
                {
                    sType = string.Format("Associated Surface");
                }
                else if (rdoFIS.Checked)
                {
                    sType = "FIS Error";
                }
                r[0].Cells[1].Value = sType;
            }
        }

        private void cboFIS_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            UpdateFISGrid();
        }

        /// <summary>
        /// Change which FIS inputs are listed on the right side of the form
        /// </summary>
        /// <remarks></remarks>
        private void UpdateFISGrid()
        {
            grdFISInputs.Rows.Clear();

            if (!rdoFIS.Checked)
            {
                return;
            }

            FIS.FISRuleFile theFISRuleFile = new FIS.FISRuleFile(((FIS.FISLibraryItem)cboFIS.SelectedItem).FilePath);

            // Loop over all the inputs defined for the FIS
            foreach (string sInput in theFISRuleFile.FISInputs)
            {
                int nRow = grdFISInputs.Rows.Add;
                grdFISInputs.Rows[nRow].Cells[0].Value = sInput;

                // Get the selected error properties row
                DataGridViewSelectedRowCollection lErr = grdErrorProperties.SelectedRows;
                if (lErr.Count == 1)
                {
                    ErrorSurfaceProperty errProps = ErrorCalcProps(lErr[0].Cells[0].Value);

                    // Only proceed if the error surface definition is a FIS
                    if ((errProps.FISInputs != null))
                    {
                        // loop over all the defined FIS inputs for the error surface
                        foreach (string sDefinedInput in errProps.FISInputs.Keys)
                        {
                            if (string.Compare(sInput, sDefinedInput, true) == 0)
                            {
                                // this is a FIS input that has a definition already
                                grdFISInputs.Rows[nRow].Cells[1].Value = errProps.FISInputs[DefinedInput];
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Save the error surface properties for the selected survey method
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool SaveErrorProperties()
        {
            DataGridViewSelectedRowCollection r = grdErrorProperties.SelectedRows;
            if (r.Count != 1)
            {
                return true;
            }

            // Save just the survey method that is selected in the left grid
            string sSurveyMethod = r[0].Cells[0].Value.ToString();
            string sErrorType = r[0].Cells[1].Value.ToString();

            if (rdoUniform.Checked)
            {
                // Create a new Uniform error properties
                ErrorCalcProps[sSurveyMethod] = new ErrorSurfaceProperty(sSurveyMethod, valUniform.Value);
                sErrorType = "Uniform Error";
            }
            else if (rdoAssociated.Checked)
            {
                if (cboAssociated.SelectedIndex >= 0)
                {
                    // Create a new associated surface error properties
                    ErrorCalcProps[sSurveyMethod] = new ErrorSurfaceProperty(sSurveyMethod, (AssocSurface)cboAssociated.SelectedItem);
                    sErrorType = "Associated Surface";
                }
                else
                {
                    MessageBox.Show("You must select an associated surface that contains the error values for this error surface.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            else
            {
                // Make sure the user has selected a GIS input
                if (cboFIS.SelectedIndex < 0)
                {
                    MessageBox.Show("You must select a FIS rule file for this survey method or define it as uniform error.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                else
                {
                    // FIS. Loop through all the FIS inputs
                    Dictionary<string, AssocSurface> dInputs = new Dictionary<string, AssocSurface>();
                    for (int i = 0; i <= grdFISInputs.Rows.Count - 1; i++)
                    {
                        DataGridViewComboBoxCell cboAssoc = (DataGridViewComboBoxCell)grdFISInputs.Rows[i].Cells[1];
                        if (cboAssoc.Value > 0)
                        {
                            dInputs.Add(grdFISInputs.Rows[i].Cells[0].Value, cboAssoc.Value);
                        }
                        else
                        {
                            MessageBox.Show("You must choose an associated surface for each FIS input.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }
                    }

                    // Find the matching fis library file
                    ErrorCalcProps[sSurveyMethod] = new ErrorSurfaceProperty(sSurveyMethod, ((FIS.FISLibraryItem)cboFIS.SelectedItem).FilePath, dInputs);
                    sErrorType = "FIS Error";
                }
            }

            r[0].Cells[1].Value = sErrorType;

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
            if (ErrorSurf is ErrorSurface)
            {
                txtRasterPath.Text = ProjectManager.Project.GetRelativePath(m_ErrorSurface.Raster.RasterPath);
            }
            else
            {
                string sRasterPath = string.Empty;
                if (!string.IsNullOrEmpty(txtName.Text))
                {
                    sRasterPath = ProjectManager.OutputManager.ErrorSurfaceRasterPath(DEM.Name, txtName.Text, false).FullName;
                    sRasterPath = ProjectManager.Project.GetRelativePath(sRasterPath);
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

            if ((string.IsNullOrEmpty(txtName.Text.Trim())))
            {
                MessageBox.Show("You must provide a unique name for the error surface.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else
            {
                if (!DEM.IsErrorNameUnique(txtName.Text, ErrorSurf))
                {
                    MessageBox.Show("There is another error surface for this DEM Survey that already possesses this name. You must provide a unique name for the error surface.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            return true;
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            if (!ValidateForm())
            {
                this.DialogResult = DialogResult.None;
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            // Need to save the current error properties first.
            if (!SaveErrorProperties())
            {
                this.DialogResult = DialogResult.None;
                return;
            }

            if (ErrorSurf is ErrorSurface)
            {
                ErrorSurf.Name = txtName.Text;
            }
            else
            {
                // Create the new error surface object

                ErrorSurf = new ErrorSurface(;
                ErrorSurface.Name = txtName.Text;
                ErrorSurf.DEMSurvey = DEM;
                ErrorSurf.Source = txtRasterPath.Text;
                ErrorSurf.Type = ErrorSurfaceType;
                ProjectManager.ds.ErrorSurface.AddErrorSurfaceRow(ErrorSurf);

                // Add all the survey methods to the database
                foreach (string sSurveyMethod in ErrorCalcProps.Keys)
                {
                    double fError = 0;
                    int nAssociatedSurfaceID = 0;

                    if (ErrorCalcProps(sSurveyMethod) is ErrorCalcPropertiesUniform)
                    {
                        fError = ((ErrorCalcPropertiesUniform)ErrorCalcProps(sSurveyMethod)).UniformErrorValue;
                    }

                    if (ErrorCalcProps(sSurveyMethod) is ErrorCalcPropertiesAssoc)
                    {
                        nAssociatedSurfaceID = ((ErrorCalcPropertiesAssoc)ErrorCalcProps(sSurveyMethod)).AssociatedSurfaceID;
                    }

                    // This error type appears to be not used when uniform, but when it's FIS it should be the name of the FIS rule
                    // file
                    string sErrorType = ErrorCalcProps(sSurveyMethod).ErrorType;
                    if (ErrorCalcProps(sSurveyMethod) is ErrorCalcPropertiesFIS)
                    {
                        sErrorType = ((ErrorCalcPropertiesFIS)ErrorCalcProps(sSurveyMethod)).FISRuleFilePath.FullName;
                    }

                    ProjectManager.ds.MultiErrorProperties.AddMultiErrorPropertiesRow(sSurveyMethod, fError, ErrorSurf, sErrorType, nAssociatedSurfaceID);
                    // m_dErrorCalculationProperties(sSurveyMethod).ErrorType)

                    // Now add all the FIS inputs to the FIS table
                    if (ErrorCalcProps(sSurveyMethod) is ErrorCalcPropertiesFIS)
                    {
                        ErrorCalcPropertiesFIS errProps = ErrorCalcProps(sSurveyMethod);
                        foreach (string sInput in errProps.FISInputs.Keys)
                        {
                            // Now find the associated surface for this input
                            foreach (ProjectDS.AssociatedSurfaceRow rAssoc in m_rDEMSurvey.GetAssociatedSurfaceRows)
                            {
                                if (rAssoc.AssociatedSurfaceID == errProps.FISInputs(sInput))
                                {
                                    ProjectManager.ds.FISInputs.AddFISInputsRow(ErrorSurf, sInput, rAssoc.Name, errProps.FISRuleFilePath.FullName);
                                    break; // TODO: might not be correct. Was : Exit For
                                }
                            }
                        }
                        //fError = DirectCast(m_dErrorCalculationProperties(sSurveyMethod), ErrorCalculation.ErrorCalcPropertiesUniform).UniformErrorValue
                    }
                }
                // For new error surfaces, we now want to create the actual error raster
                try
                {
                    ErrorSurfaceEngine errEngine = new ErrorSurfaceEngine(ErrorSurf);
                    errEngine.CreateErrorSurfaceRaster();

                    //If My.Settings.AddOutputLayersToMap Then
                    // TODO 
                    throw new Exception("not implemented");
                    //Core.GCDProject.ProjectManagerUI.ArcMapManager.AddErrSurface(ErrorSurf)
                    //End If
                }
                catch (Exception ex)
                {
                    DialogResult = DialogResult.None;
                    Cursor.Current = Cursors.Default;
                    ProjectManager.ds.ErrorSurface.RemoveErrorSurfaceRow(ErrorSurf);
                    Exception ex2 = new Exception("Error generating error surface raster. No changes were made to the GCD project.", ex);
                    naru.error.ExceptionUI.HandleException(ex2);
                    return;
                }
            }

            // Now save the GCD project
            try
            {
                ProjectManager.save();
            }
            catch (Exception ex)
            {
                DialogResult = DialogResult.None;
                Cursor.Current = Cursors.Default;
                Exception ex2 = new Exception("Error saving error surface properties to the GCD project.", ex);
                naru.error.ExceptionUI.HandleException(ex2);
                return;
            }

            Cursor.Current = Cursors.Default;
        }
    }
}