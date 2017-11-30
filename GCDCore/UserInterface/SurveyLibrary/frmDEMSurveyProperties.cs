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
        private frmImportRaster m_ImportRasterform;

        private System.ComponentModel.BindingList<AssocSurface> AssocSurfaceBindingList;
        #region "Survey Property Routines"


        public frmDEMSurveyProperties(DEMSurvey editDEM)
        {
            Load += SurveyPropertiesForm_Load;
            // This call is required by the Windows Form Designer.
            InitializeComponent();
            DEM = editDEM;
        }


        private void SurveyPropertiesForm_Load(System.Object sender, System.EventArgs e)
        {
            InitControls();
            cboSingle.Items.AddRange(ProjectManager.SurveyTypes.Values.ToArray());

            txtName.Text = DEM.Name;
            txtRasterPath.Text = ProjectManager.Project.GetRelativePath(DEM.Raster.GISFileInfo);
            txtMask.Text = DEM.MethodMaskField;
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
                    txtMask.Text = DEM.MethodMask.FullName;
                }
            }

            // Turn on handling survey method event handling (after setting control from project)
            rdoSingle.CheckedChanged += rdoSingle_CheckedChanged;

            if (DEM.SurveyDate == null)
            {
                lblDatetime.Text = SurveyDateTime.NotSetString;
            }
            else
            {
                lblDatetime.Text = DEM.SurveyDate.ToString();
            }

            AssocSurfaceBindingList = new System.ComponentModel.BindingList<AssocSurface>(DEM.AssocSurfaces.Values.ToList());

            grdAssocSurface.DataSource = AssocSurfaceBindingList;

            UpdateControls();
            LoadRasterProperties();

        }


        private void InitControls()
        {
            cmdAddDEMToMap.Visible = ProjectManager.IsArcMap;
            cmdAddAssocToMap.Visible = ProjectManager.IsArcMap;
            cmdAddErrorToMap.Visible = ProjectManager.IsArcMap;

            grdAssocSurface.AllowUserToResizeRows = false;
            grdAssocSurface.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            grdAssocSurface.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

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
                if (string.IsNullOrEmpty(txtMask.Text))
                {
                    MessageBox.Show("You must select a survey mask feature class.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnBrowseMask.Select();
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


        private void txtMask_TextChanged(object sender, System.EventArgs e)
        {
            cboIdentify.Items.Clear();
            if (string.IsNullOrEmpty(txtMask.Text))
            {
                return;
            }

            GCDConsoleLib.Vector gMask = new GCDConsoleLib.Vector(DEM.MethodMask);
            List<GCDConsoleLib.VectorField> stringFields = gMask.Fields.Values.Where(x => x.Type == GCDConsoleLib.GDalFieldType.StringField).ToList<GCDConsoleLib.VectorField>();
            cboIdentify.Items.AddRange(stringFields.ToArray());
            if (!string.IsNullOrEmpty(DEM.MethodMaskField) && cboIdentify.Items.Contains(DEM.MethodMaskField))
            {
                cboIdentify.Text = DEM.MethodMaskField;
            }

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


        private void UpdateControls()
        {
            cboSingle.Enabled = rdoSingle.Checked;
            txtMask.Enabled = rdoMulti.Checked;
            cboIdentify.Enabled = rdoMulti.Checked;
            btnBrowseMask.Enabled = rdoMulti.Checked;

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
                DEM.MethodMask = new System.IO.FileInfo(txtMask.Text);
                DEM.MethodMaskField = cboIdentify.Text;
            }

            ProjectManager.Project.Save();
        }

        private void btnBrowseFile_Click(System.Object sender, System.EventArgs e)
        {
            //TODO ArcMap manager in addin
            throw new Exception("not implemented");

        }


        private void btnBrowseMask_Click(System.Object sender, System.EventArgs e)
        {
            rdoMulti.Checked = true;

            try
            {
                throw new NotImplementedException("browse for polygon mask. then set mask text. This should trigger field combo refresh.");

            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }


        private void rdoSingle_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            if (rdoSingle.Checked)
            {
                if (DEM.MethodMask is FileInfo && DEM.MethodMask.Exists)
                {
                    throw new NotImplementedException("Need to delete any existing polygon mask");
                }
            }
            UpdateControls();
        }

        private void btnHlp_Click(System.Object sender, System.EventArgs e)
        {
            Process.Start(GCDCore.Properties.Resources.HelpBaseURL + "gcd-command-reference/gcd-project-explorer/d-dem-context-menu/i-edit-dem-survey-properties");
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


        private void ViewAssociatedSurface(System.Object sender, EventArgs e)
        {
            // Save DEM so that latest survey method is stored ready for associated use by surface 
            SaveDEMSurvey();

            AssocSurface assoc = (AssocSurface)grdAssocSurface.SelectedRows[0].DataBoundItem;
            frmAssocSurfaceProperties frm = new frmAssocSurfaceProperties(DEM, assoc);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                throw new NotImplementedException("Need to refresh data grid view");
            }

        }


        private void btnAddToMap_Click(System.Object sender, System.EventArgs e)
        {
            throw new NotImplementedException("add selected associated surface to map");

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
                    AssocSurfaceBindingList.Add(SurfaceForm.AssociatedSurface);
                    AssocSurfaceBindingList.ResetBindings();
                }
            }

        }

        private void Associated_CellContentEnter(System.Object sender, DataGridViewCellEventArgs e)
        {
            cmdAddAssocToMap.Enabled = true;
            btnDeleteAssociatedSurface.Enabled = true;
            btnSettingsAssociatedSurface.Enabled = true;
        }

        private void Associated_CellContentLeave(System.Object sender, DataGridViewCellEventArgs e)
        {
            cmdAddAssocToMap.Enabled = false;
            btnDeleteAssociatedSurface.Enabled = false;
            btnSettingsAssociatedSurface.Enabled = false;
        }


        private void btnDeleteAssociatedSurface_Click(System.Object sender, System.EventArgs e)
        {

            if (MessageBox.Show("Are you sure you want to remove the selected associated surface from the GCD Project?" + " This will also delete the raster associated with this surface.", "Deleted associated surface?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                DEM.DeleteAssociatedSurface((AssocSurface)grdAssocSurface.SelectedRows[0].DataBoundItem);
            }

        }

        #endregion

        #region "Error Calculation Events"


        private void btn_AddErrorSurface_Click(System.Object sender, System.EventArgs e)
        {
            if (!ValidateForm())
            {
                return;
            }

            try
            {
                // save the DEM survey information
                SaveDEMSurvey();
                SpecifyErrorSurface(DEM);
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }

        }

        public static ErrorSurface SpecifyErrorSurface(DEMSurvey dem)
        {

            throw new NotImplementedException("specify error surface");

            ErrorSurface errSurface = null;
            //Dim gDEM As New GCDConsoleLib.Raster(ProjectManager.GetAbsolutePath(rDEM.Source))

            //Dim frm As New frmImportRaster(dem.Raster.Raster, rDEM, frmImportRaster.ImportRasterPurposes.ErrorCalculation, "Error Surface")

            //If frm.ShowDialog = System.Windows.Forms.DialogResult.OK Then
            //    Dim gRaster As GCDConsoleLib.Raster = Nothing
            //    Try
            //        gRaster = frm.ProcessRaster
            //    Catch ex As Exception
            //        Try
            //            IO.Directory.Delete(IO.Path.GetDirectoryName(frm.txtRasterPath.Text))
            //        Catch ex2 As Exception
            //            ' do nothing
            //        End Try

            //        naru.error.ExceptionUI.HandleException(ex, "An error occurred attempting to import the error surface into the GCD project. No information has been saved to the GCD project file but you should check the GCD project folder to determine if any remains of the raster remain.")
            //    End Try

            //    If TypeOf gRaster Is GCDConsoleLib.Raster Then
            //        Try
            //            Dim errSurface As ErrorSurface = New ErrorSurface(frm.txtName.Text, gRaster.GISFileInfo, dem)
            //            dem.ErrorSurfaces.Add(errSurface.Name, errSurface)
            //            ProjectManager.Project.Save()

            //            'If My.Settings.AddOutputLayersToMap Then
            //            'TODO not implemented
            //            Throw New Exception("Not implemented")
            //            'GCDProject.ProjectManagerUI.ArcMapManager.AddErrSurface(rError)
            //            'End If

            //        Catch ex As Exception
            //            Dim bRasterExists As Boolean = True
            //            Try
            //                GCDConsoleLib.Raster.Delete(New IO.FileInfo(frm.txtRasterPath.Text))
            //                bRasterExists = False
            //            Catch ex2 As Exception
            //                bRasterExists = True
            //            End Try

            //            Dim sMsg As String = "Failed to save the error surface information to the GCD project file."
            //            If bRasterExists Then
            //                sMsg &= " The GCD project error surface raster still exists And should be deleted by hand."
            //            Else
            //                sMsg &= "The GCD project error surface raster was deleted."
            //            End If

            //            naru.error.ExceptionUI.HandleException(ex, sMsg)
            //        End Try
            //    End If
            //End If

            return errSurface;

        }

        private void btnCalculateError_Click(System.Object sender, System.EventArgs e)
        {
            //
            // Only open the Error calculation form if the survey properties save successfully.
            //
            throw new NotImplementedException("todo");
            //If ValidateForm() Then
            //    SaveDEMSurvey()
            //    Dim dr As DataRowView = DEMSurveyBindingSource.Current
            //    Dim frm As New ErrorCalculation.frmErrorCalculation(DirectCast(dr.Row, ProjectDS.DEMSurveyRow))
            //    frm.ShowDialog()
            //End If

        }

        private void Error_CellContentEnter(System.Object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            cmdAddErrorToMap.Enabled = true;
            btnErrorDelete.Enabled = true;
            btnErrorSettings.Enabled = true;
        }

        private void Error_CellContentLeave(System.Object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            cmdAddErrorToMap.Enabled = false;
            btnErrorDelete.Enabled = false;
            btnErrorSettings.Enabled = false;
        }

        private void Error_DoubleClick(System.Object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            ViewErrorSettings();
        }

        private void btnErrorSurfaceSettings_Click(System.Object sender, System.EventArgs e)
        {
            ViewErrorSettings();
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
            throw new NotImplementedException("add error surface to map");

        }


        private void ViewErrorSettings()
        {
            throw new NotImplementedException("edit error surface");

            //Dim CurrentRow As DataRowView = Me.ErrorTableBindingSource.Current
            //If Not CurrentRow Is Nothing AndAlso TypeOf CurrentRow.Row Is ProjectDS.ErrorSurfaceRow Then
            //    Dim frm As New ErrorCalculation.frmErrorCalculation(DirectCast(CurrentRow.Row, ProjectDS.ErrorSurfaceRow))
            //    frm.ShowDialog()
            //End If

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
    }

}
