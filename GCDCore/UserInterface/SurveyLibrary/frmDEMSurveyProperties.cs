using GCDCore.Project;
using System.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace GCDCore.UserInterface.SurveyLibrary
{
    public partial class frmDEMSurveyProperties
    {
        private DEMSurvey DEM;

        #region "Survey Property Routines"

        public frmDEMSurveyProperties(DEMSurvey editDEM)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();
            DEM = editDEM;

            ucDEMMask.PathChanged += OnMaskChanged;
        }

        private void SurveyPropertiesForm_Load(System.Object sender, System.EventArgs e)
        {
            InitControls();
            cboSingle.Items.AddRange(ProjectManager.SurveyTypes.Values.ToArray());

            txtName.Text = DEM.Name;
            txtRasterPath.Text = ProjectManager.Project.GetRelativePath(DEM.Raster.GISFileInfo);

            ucDEMMask.Initialize("DEM Survey Mask", GCDConsoleLib.GDalGeometryType.SimpleTypes.Polygon);
            txtFolder.Text = DEM.Raster.GISFileInfo.DirectoryName;
            rdoSingle.Checked = DEM.IsSingleSurveyMethod;

            if (DEM.IsSingleSurveyMethod)
            {
                // Select the single survey method
                if (string.IsNullOrEmpty(DEM.SurveyMethod))
                {
                    cboSingle.SelectedIndex = 0;
                }
                else
                {
                    cboSingle.Text = DEM.SurveyMethod;
                }
            }
            else
            {
                // Load the fields from the Polygon mask
                if (DEM.MethodMask is FileInfo && DEM.MethodMask.Exists)
                {
                    // This should fire the reloading of the mask field dropdown
                    ucDEMMask.Initialize(DEM.MethodMask.FullName,  GCDConsoleLib.GDalGeometryType.SimpleTypes.Polygon);
                }
            }

            // Turn on handling survey method event handling (after setting control from project)
            rdoSingle.CheckedChanged += rdoSingle_CheckedChanged;

            lblDatetime.Text = DEM.SurveyDate is SurveyDateTime ? DEM.SurveyDate.ToString() : SurveyDateTime.NotSetString;

            grdErrorSurfaces.DataSource = DEM.ErrorSurfaces;
            grdAssocSurface.DataSource = DEM.AssocSurfaces;

            if (ProjectManager.IsArcMap)
            {
                ucDEMMask.BrowseVector += ProjectManager.OnBrowseVector;
                ucDEMMask.SelectVector += ProjectManager.OnSelectVector;
            }

            UpdateControls(sender, e);
            LoadRasterProperties();
        }

        private void InitControls()
        {
            cmdAddDEMToMap.Visible = ProjectManager.IsArcMap;
            cmdAssocAddToMap.Visible = ProjectManager.IsArcMap;
            cmdErrorAddToMap.Visible = ProjectManager.IsArcMap;

            grdAssocSurface.AutoGenerateColumns = false;
            grdAssocSurface.AllowUserToResizeRows = false;
            grdAssocSurface.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            grdAssocSurface.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            grdErrorSurfaces.AllowUserToResizeRows = false;
            grdErrorSurfaces.AutoGenerateColumns = false;

            //General Tooltips
            //ttpTooltip.SetToolTip(btnCancel, My.Resources.ttpCancel)
            //ttpTooltip.SetToolTip(btnHelp, My.Resources.ttpHelp)

            //'DEM Survey Tooltips
            //ttpTooltip.SetToolTip(txtName, My.Resources.ttpBudgetFormBtnCalculate)

            //ttpTooltip.SetToolTip(txtName, My.Resources.ttpSurveyPropertiesDEMSurveyName)
            //ttpTooltip.SetToolTip(txtDate, My.Resources.ttpSurveyPropertiesDEMSurveyDate)
            //'ttpTooltip.SetToolTip(cboLayer, My.Resources.ttpDEMLayer)
            //ttpTooltip.SetToolTip(cboSingle, My.Resources.ttpSurveyPropertiesSingleCombo)
            //'ttpTooltip.SetToolTip(cboPolyMask, My.Resources.ttpPolyMask)
            //ttpTooltip.SetToolTip(cboIdentify, My.Resources.ttpSurveyPropertiesIdentifierField)
            //ttpTooltip.SetToolTip(cmdAddToMap, My.Resources.ttpSurveyPropertiesBrowseFile)
            //ttpTooltip.SetToolTip(btnBrowseMask, My.Resources.ttpSurveyPropertiesBrowseMask)
            //ttpTooltip.SetToolTip(txtRasterPath, My.Resources.ttpSurveyPropertiesTxtFolder)
            //ttpTooltip.SetToolTip(txtMask, My.Resources.ttpSurveyPropertiesTxtMask)
            //ttpTooltip.SetToolTip(rdoSingle, My.Resources.ttpSurveyPropertiesSingleMethod)
            //ttpTooltip.SetToolTip(rdoMulti, My.Resources.ttpSurveyPropertiesMultiMethod)

            //'Associated Surfaces Tooltips
            //ttpTooltip.SetToolTip(btnAddAssociatedSurface, My.Resources.ttpAddAS)
            //ttpTooltip.SetToolTip(btnSettingsAssociatedSurface, My.Resources.ttpSettingsAS)
            //ttpTooltip.SetToolTip(btnDeleteAssociatedSurface, My.Resources.ttpDeleteAS)
            //ttpTooltip.SetToolTip(btnAddToMap, My.Resources.ttpAddAStoMap)

            //'Error Calculation Tooltips
            //ttpTooltip.SetToolTip(btnAddError, "Associate an existing raster as an error surface")
            //ttpTooltip.SetToolTip(btnCalculateError, "Calculate a new error raster surface")
            //ttpTooltip.SetToolTip(btnErrorSettings, My.Resources.ttpSettingsError)
            //ttpTooltip.SetToolTip(btnErrorDelete, My.Resources.ttpDeleteError)

        }

        private bool ValidateForm()
        {
            txtName.Text = txtName.Text.Trim();

            if (!ProjectManager.Project.IsDEMNameUnique(txtName.Text, DEM))
            {
                MessageBox.Show(string.Format("There is already another DEM survey in this project with the name '{0}'. Each DEM Survey must have a unique name.", txtName.Text));
                txtName.Select();
                return false;
            }

            if (rdoSingle.Checked)
            {
                if (string.IsNullOrEmpty(cboSingle.Text))
                {
                    MessageBox.Show("You must select a survey method type.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboSingle.Select();
                    return false;
                }
            }
            else
            {
                if (ucDEMMask.SelectedItem == null)
                {
                    MessageBox.Show("You must select a survey method mask polygon feature class, or designate the DEM as a single method survey.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ucDEMMask.Select();
                    return false;
                }

                if (string.IsNullOrEmpty(cboIdentify.Text))
                {
                    MessageBox.Show("You must select a survey method identifier field.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboIdentify.Select();
                    return false;
                }
            }

            return true;
        }

        private void OnMaskChanged(object sender, EventArgs e)
        {
            cboIdentify.Items.Clear();
            if (ucDEMMask.SelectedItem == null)
            {
                return;
            }

            // Copy the newly selected polygon mask into project
            FileInfo maskPath = ProjectManager.OutputManager.DEMSurveyMethodMaskPath(DEM.Name);

            // Delete any existing mask
            if (maskPath.Exists)
            {
                try
                {
                    GCDConsoleLib.Vector.Delete(maskPath);
                }
                catch (Exception ex)
                {
                    naru.error.ExceptionUI.HandleException(ex, "Error attempting to delete DEM mask at " + maskPath.FullName);
                    return;
                }
            }

            try
            {
                maskPath.Directory.Create();
                ucDEMMask.SelectedItem.Copy(maskPath);
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "Error attempting to copy DEM mask into project at " + maskPath.FullName);
                return;
            }

            // Disable event handling to avoid circular function calls
            ucDEMMask.PathChanged -= OnMaskChanged;
            ucDEMMask.Initialize("DEM Survey Mask", maskPath, true);
            ucDEMMask.PathChanged += OnMaskChanged;

            List<GCDConsoleLib.VectorField> stringFields = ucDEMMask.SelectedItem.Fields.Values.Where(x => x.Type.Equals(GCDConsoleLib.GDalFieldType.StringField)).ToList<GCDConsoleLib.VectorField>();
            cboIdentify.Items.AddRange(stringFields.ToArray());
            if (!string.IsNullOrEmpty(DEM.MethodMaskField) && cboIdentify.Items.Contains(DEM.MethodMaskField))
            {
                cboIdentify.Text = DEM.MethodMaskField;
            }
            else if (cboIdentify.Items.Count == 1)
                cboIdentify.SelectedIndex = 0;
        }

        private void txtFolder_DoubleClick(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFolder.Text))
            {
                if (System.IO.Directory.Exists(txtFolder.Text))
                {
                    Process.Start("explorer.exe", txtFolder.Text);
                }
            }
        }

        private void UpdateControls(object sender, EventArgs e)
        {
            cboSingle.Enabled = rdoSingle.Checked;
            ucDEMMask.Enabled = rdoMulti.Checked;
            cboIdentify.Enabled = rdoMulti.Checked;

            cmdErrorProperties.Enabled = grdErrorSurfaces.SelectedRows.Count == 1;
            cmdErrorDelete.Enabled = grdErrorSurfaces.SelectedRows.Count == 1;
            cmdErrorAddToMap.Enabled = grdErrorSurfaces.SelectedRows.Count == 1;

            cmdAssocDelete.Enabled = grdAssocSurface.SelectedRows.Count == 1;
            cmdAssocProperties.Enabled = grdAssocSurface.SelectedRows.Count == 1;
            cmdAssocAddToMap.Enabled = grdAssocSurface.SelectedRows.Count == 1;
        }

        private void btnOK_Click(System.Object sender, System.EventArgs e)
        {
            if (ValidateForm())
            {
                SaveDEMSurvey();
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.None;
            }
        }

        private void SaveDEMSurvey()
        {
            DEM.Name = txtName.Text;

            if (rdoSingle.Checked)
            {
                if (cboSingle.Text.ToLower().Contains("undefined"))
                    DEM.SurveyMethod = string.Empty;
                else
                    DEM.SurveyMethod = cboSingle.Text;

                DEM.MethodMask = null;
                DEM.MethodMaskField = string.Empty;
            }
            else
            {
                DEM.SurveyMethod = string.Empty;
                DEM.MethodMask = ucDEMMask.SelectedItem.GISFileInfo;
                DEM.MethodMaskField = cboIdentify.Text;
            }

            ProjectManager.Project.Save();
        }

        private void rdoSingle_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            if (rdoSingle.Checked)
            {
                if (ucDEMMask.SelectedItem is GCDConsoleLib.Vector)
                {
                    try
                    {
                        GCDConsoleLib.Vector.Delete(ucDEMMask.SelectedItem.GISFileInfo);
                    }
                    catch (Exception ex)
                    {
                        naru.error.ExceptionUI.HandleException(ex, "Error attempting to delete DEM method mask at " + ucDEMMask.SelectedItem.GISFileInfo.FullName);
                    }

                    // proceed and unhook the DEM from the mask anyway.
                    // Creating a new mask should use a new safe name anyway.
                    ucDEMMask.PathChanged -= OnMaskChanged;
                    ucDEMMask.ClearSelectedItem();
                    ucDEMMask.PathChanged += OnMaskChanged;
                    cboIdentify.Items.Clear();
                }
            }
            UpdateControls(sender, e);
        }

        private void btnHlp_Click(System.Object sender, System.EventArgs e)
        {
            Process.Start(Properties.Resources.HelpBaseURL + "gcd-command-reference/gcd-project-explorer/d-dem-context-menu/i-edit-dem-survey-properties");
        }

        private void LoadRasterProperties()
        {
            string sRasterProperties = "-- GCD Raster Properties --";

            var _with1 = DEM.Raster.Extent;
            sRasterProperties += Environment.NewLine + "Left: " + _with1.Left.ToString("#,##0.#");
            sRasterProperties += Environment.NewLine + "Top: " + _with1.Top.ToString("#,##0.#");
            sRasterProperties += Environment.NewLine + "Right: " + _with1.Right.ToString("#,##0.#");
            sRasterProperties += Environment.NewLine + "Bottom: " + _with1.Bottom.ToString("#,##0.#");
            sRasterProperties += Environment.NewLine;
            sRasterProperties += Environment.NewLine + "Cell size: " + _with1.CellWidth;
            sRasterProperties += Environment.NewLine;
            sRasterProperties += Environment.NewLine + "Width: " + (_with1.Right - _with1.Left).ToString("#,##0.#");
            sRasterProperties += Environment.NewLine + "Height: " + (_with1.Top - _with1.Bottom).ToString("#,##0.#");
            sRasterProperties += Environment.NewLine;
            sRasterProperties += Environment.NewLine + "Rows: " + _with1.Rows.ToString("#,##0");
            sRasterProperties += Environment.NewLine + "Columns: " + _with1.Cols.ToString("#,##0");

            sRasterProperties += Environment.NewLine + Environment.NewLine;
            //sRasterProperties &= "-- Original Raster Properties --"
            //sRasterProperties &= vbNewLine & "Left: " & demRow.OriginalExtentLeft.ToString
            //sRasterProperties &= vbNewLine & "Top: " & demRow.OriginalExtentTop.ToString
            //sRasterProperties &= vbNewLine & "Right: " & demRow.OriginalExtentRight.ToString
            //sRasterProperties &= vbNewLine & "Bottom: " & demRow.OriginalExtentBottom.ToString
            //sRasterProperties &= vbNewLine
            //sRasterProperties &= vbNewLine & "Cell size: " & demRow.OriginalCellSize.ToString
            //sRasterProperties &= vbNewLine
            //sRasterProperties &= vbNewLine & "Path: " & demRow.OriginalSource
            //sRasterProperties &= vbNewLine & "Computer: " & demRow.OriginalComputer
            //sRasterProperties &= vbNewLine

            try
            {
                // File Size
                sRasterProperties += Environment.NewLine + "Raster file size: " + naru.os.File.GetFormattedFileSize(DEM.Raster.GISFileInfo);
            }
            catch (Exception ex)
            {
                // Do nothing. 
            }

            txtProperties.Text = sRasterProperties;
        }

        #endregion

        #region "Associated Surface Events"

        private void cmdEditAssocSurfaceProperties_Click(System.Object sender, EventArgs e)
        {
            if (ViewAssociatedSurface() == DialogResult.OK)
                DEM.AssocSurfaces.ResetBindings();
        }

        private DialogResult ViewAssociatedSurface()
        {
            // Save DEM so that latest survey method is stored ready for associated use by surface 
            SaveDEMSurvey();

            AssocSurface assoc = (AssocSurface)grdAssocSurface.SelectedRows[0].DataBoundItem;
            frmAssocSurfaceProperties frm = new frmAssocSurfaceProperties(DEM, assoc);
            return frm.ShowDialog();
        }

        private void btnAddToMap_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                AssocSurface assoc = (AssocSurface)grdAssocSurface.SelectedRows[0].DataBoundItem;
                ProjectManager.OnAddToMap((GCDProjectRasterItem)assoc);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding associated surface to the map.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAddAssociatedSurface_Click(System.Object sender, System.EventArgs e)
        {

            if (ValidateForm())
            {
                // Save DEM so that latest survey method is stored ready for associated use by surface 
                SaveDEMSurvey();

                frmAssocSurfaceProperties SurfaceForm = new frmAssocSurfaceProperties(DEM, null);
                if (SurfaceForm.ShowDialog() == DialogResult.OK)
                {
                    grdAssocSurface.Rows[grdAssocSurface.Rows.Count - 1].Selected = true;
                    ProjectManager.OnAddToMap((AssocSurface) grdAssocSurface.Rows[grdAssocSurface.Rows.Count - 1].DataBoundItem);
                }
            }
        }

        private void btnDeleteAssociatedSurface_Click(System.Object sender, System.EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove the selected associated surface from the GCD Project?" + " This will also delete the raster associated with this surface.", "Deleted associated surface?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                try
                {
                    DEM.DeleteAssociatedSurface((AssocSurface)grdAssocSurface.SelectedRows[0].DataBoundItem);
                }
                catch (Exception ex)
                {
                    naru.error.ExceptionUI.HandleException(ex, "Error deleting associated surface.");
                }
            }
        }

        #endregion

        #region "Error Calculation Events"

        private void cmdSpecifyErrorSurface_Click(System.Object sender, System.EventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                // save the DEM survey information
                SaveDEMSurvey();
                if (SpecifyErrorSurface(DEM) is ErrorSurface)
                {
                    grdErrorSurfaces.Rows[grdErrorSurfaces.Rows.Count - 1].Selected = true;
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        public static ErrorSurface SpecifyErrorSurface(DEMSurvey dem)
        {
            ErrorSurface errSurf = null;
            frmImportRaster frm = new frmImportRaster(dem, ExtentImporter.Purposes.ErrorSurface, "Error Surface");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                GCDConsoleLib.Raster errRaster = frm.ProcessRaster();

                // Create the associated surface
                AssocSurface assoc = new AssocSurface(frm.txtName.Text, errRaster.GISFileInfo, dem, AssocSurface.AssociatedSurfaceTypes.ElevationUncertainty);
                dem.AssocSurfaces.Add(assoc);

                // Create the error surface that points to the associated surface
                Dictionary<string, ErrorSurfaceProperty> dProps = new Dictionary<string, ErrorSurfaceProperty>();
                dProps[frmErrorSurfaceProperties.m_sEntireDEMExtent] = new ErrorSurfaceProperty(frmErrorSurfaceProperties.m_sEntireDEMExtent);
                dProps[frmErrorSurfaceProperties.m_sEntireDEMExtent].AssociatedSurface = assoc;
                errSurf = new ErrorSurface(assoc.Name, assoc.Raster.GISFileInfo, dem, dem.ErrorSurfaces.Count == 0, dProps);
                dem.ErrorSurfaces.Add(errSurf);

                ProjectManager.Project.Save();
            }
            return errSurf;
        }

        public static ErrorSurface CalculateErrorSurface(DEMSurvey dem)
        {
            ErrorSurface errSurf = null;
            frmErrorSurfaceProperties frm = new frmErrorSurfaceProperties(dem, null);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                errSurf = frm.ErrorSurf;
            }

            return errSurf;
        }

        private void cmdCalculateErrorSurface_Click(System.Object sender, System.EventArgs e)
        {
            if (ValidateForm())
            {
                try
                {
                    // Only open the Error calculation form if the survey properties save successfully.
                    SaveDEMSurvey();
                    if (CalculateErrorSurface(DEM) is ErrorSurface)
                    {
                        grdErrorSurfaces.Rows[grdErrorSurfaces.Rows.Count - 1].Selected = true;
                    }
                }
                catch (Exception ex)
                {
                    naru.error.ExceptionUI.HandleException(ex, "Error calculating error surface");
                }
            }
        }

        private void Error_CellContentEnter(System.Object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            cmdErrorAddToMap.Enabled = true;
            cmdErrorDelete.Enabled = true;
            cmdErrorProperties.Enabled = true;
        }

        private void Error_CellContentLeave(System.Object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            cmdErrorAddToMap.Enabled = false;
            cmdErrorDelete.Enabled = false;
            cmdErrorProperties.Enabled = false;
        }

        private void Error_DoubleClick(System.Object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            try
            {
                if (ViewErrorSurfaceProperties((ErrorSurface)grdErrorSurfaces.SelectedRows[0].DataBoundItem) == DialogResult.OK)
                {
                    DEM.ErrorSurfaces.ResetBindings();
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "Error viewing error surface properties.");
            }
        }

        private void btnErrorSurfaceSettings_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                if (ViewErrorSurfaceProperties((ErrorSurface)grdErrorSurfaces.SelectedRows[0].DataBoundItem) == DialogResult.OK)
                {
                    DEM.ErrorSurfaces.ResetBindings();
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "Error viewing error surface properties.");
            }
        }

        private void btnDeleteErrorSurface_Click(System.Object sender, System.EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the selected error surface from the GCD project?" + " This will also delete the raster associated with this error surface.", "Delete Error Surface", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                DEM.DeleteErrorSurface((ErrorSurface)grdErrorSurfaces.SelectedRows[0].DataBoundItem);
            }
        }

        private void btnAddErrorToMap_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                ProjectManager.OnAddToMap((GCDProjectRasterItem)grdErrorSurfaces.SelectedRows[0].DataBoundItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding error surface to the map.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public static DialogResult ViewErrorSurfaceProperties(ErrorSurface errSurf)
        {
            frmErrorSurfaceProperties frm = new frmErrorSurfaceProperties(errSurf.DEM, errSurf);
            return frm.ShowDialog();
        }

        #endregion

        private void cmdDateTime_Click(System.Object sender, System.EventArgs e)
        {
            frmSurveyDateTime frm = new frmSurveyDateTime(DEM.SurveyDate);
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DEM.SurveyDate = frm.SurveyDateTime;
                if (DEM.SurveyDate is SurveyDateTime)
                    lblDatetime.Text = DEM.SurveyDate.ToString();
                else
                    lblDatetime.Text = SurveyDateTime.NotSetString;
            }
        }

        private void grdAssocSurface_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ViewAssociatedSurface() == DialogResult.OK)
                DEM.AssocSurfaces.ResetBindings();
        }

        private void cmdAddDEMToMap_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectManager.OnAddToMap(DEM);
            }
            catch(Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "Error adding DEM to map.");
            }
        }
    }
}
