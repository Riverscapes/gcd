using System;
using System.Windows.Forms;
using GCDCore.Project;
using System.Collections.Generic;
using System.Drawing;

namespace GCDCore.UserInterface.SurveyLibrary
{
    public partial class frmSurfaceProperties : Form, IProjectItemForm
    {
        public readonly GCDProjectRasterItem Raster;
        private SurveyDateTime SurveyDate { get; set; }
 
        public GCDProjectItem GCDProjectItem { get { return Raster; } }

        public frmSurfaceProperties(GCDProjectRasterItem surface, bool editable)
        {
            InitializeComponent();
           Raster = surface;
            if (surface is DEMSurvey)
            {
                SurveyDate = ((DEMSurvey)surface).SurveyDate;
            }

            txtName.ReadOnly = !editable;
            cmdSurveyDate.Enabled = editable;
            cmdOK.Visible = editable;

            if (!editable)
                cmdCancel.Text = "Close";
        }

        private void frmDEMProperties_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Text = string.Format("{0} Properties", Raster.Noun);
            cmdOK.Text = Properties.Resources.UpdateButtonText;

            txtName.Text = Raster.Name;
            txtPath.Text = ProjectManager.Project.GetRelativePath(Raster.Raster.GISFileInfo);

            ucRasterProperties1.Initialize(Raster.Noun, Raster.Raster);
         
            if (Raster is DEMSurvey)
            {
                txtSurveyDate.Text = SurveyDate is SurveyDateTime ? SurveyDate.ToString() : SurveyDateTime.NotSetString;
            }
            else
            {
                lblSurveyDate.Visible = false;
                txtSurveyDate.Visible = false;
                cmdSurveyDate.Visible = false;
                ucRasterProperties1.Height += ucRasterProperties1.Top - txtSurveyDate.Top;
                ucRasterProperties1.Top = txtSurveyDate.Top;
            }

            if (!ProjectManager.IsArcMap)
            {
                cmdAddTopMap.Visible = false;
                txtPath.Width = cmdAddTopMap.Right - txtPath.Left;
            }
  
            tTip.SetToolTip(txtName, "The name used to refer to this item within the GCD project. It cannot be empty and it must be unique among all other items of this type.");
            tTip.SetToolTip(txtPath, "The relative file path of this raster within the GCD project.");
            tTip.SetToolTip(cmdAddTopMap, "Add the raster to the current map document.");
            tTip.SetToolTip(txtSurveyDate, "The date on which the DEM survey was collected.");
            tTip.SetToolTip(cmdSurveyDate, "Configure the date on which the DEM survey was collected.");
         
            Cursor = Cursors.Default;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                DialogResult = DialogResult.None;
                return;
            }

            // Detect if name has been changed - need to update owner dictionaries
            if (string.Compare(Raster.Name, txtName.Text, false) != 0)
            {
                Raster.Name = txtName.Text;                
            }

            if (Raster is DEMSurvey)
                ((DEMSurvey)Raster).SurveyDate = SurveyDate;

            ProjectManager.Project.Save();
        }

        private bool ValidateForm()
        {
            // Sanity check to prevent empty names
            txtName.Text = txtName.Text.Trim();

            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show(string.Format("You must provide a name for the {0}. The name cannot be blank.", Raster.Noun), "Missing Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Select();
                return false;
            }

            bool bUnique = true;
            if (Raster is DEMSurvey)
            {
                bUnique = ProjectManager.Project.IsDEMNameUnique(txtName.Text, (DEMSurvey)Raster);
            }
            else if (Raster is Surface)
            {
                bUnique = ProjectManager.Project.IsReferenceSurfaceNameUnique(txtName.Text, (Surface)Raster);
            }
            else if (Raster is ErrorSurface)
            {
                bUnique = ((ErrorSurface)Raster).Surf.IsErrorNameUnique(txtName.Text, (ErrorSurface)Raster);
            }
            else
            {
                throw new Exception("Unhandled surface type");
            }

            if (!bUnique)
            {
                MessageBox.Show(string.Format("A {0} with this name already exists within this project. Please enter a unique name for the {0}", Raster.Noun.ToLower()), "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Select();
                return false;
            }

            return true;
        }

        private void cmdAddTopMap_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectManager.OnAddRasterToMap(Raster);
            }
            catch (Exception ex)
            {
                GCDException.HandleException(ex);
            }
        }

        private void cmdSurveyDate_Click(object sender, EventArgs e)
        {
            frmSurveyDateTime frm = new frmSurveyDateTime(SurveyDate);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                SurveyDate = frm.SurveyDateTime;
                txtSurveyDate.Text = SurveyDate is SurveyDateTime ? SurveyDate.ToString() : SurveyDateTime.NotSetString;
            }
        }

        private void cmdHelp_Click(object sender, EventArgs e)
        {
            OnlineHelp.Show(Name);
        }
    }
}
