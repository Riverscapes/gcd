using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using GCDConsoleLib.GCD;
using GCDCore.Project;

namespace GCDCore.UserInterface.Project
{
    public partial class frmProjectProperties : Form
    {
        private readonly bool CreateMode;
        private readonly BindingList<ProjectMetaData> MetaData;

        public frmProjectProperties()
        {
            InitializeComponent();
            // New empty list for metadata
            MetaData = new BindingList<ProjectMetaData>();
        }

        public frmProjectProperties(bool bCreateMode)
        {
            KeyDown += frmProjectProperties_KeyDown;

            // This call is required by the Windows Form Designer.
            InitializeComponent();
            CreateMode = bCreateMode;

            // New empty list for metadata
            MetaData = new BindingList<ProjectMetaData>();
        }

        private void frmProjectProperties_Load(System.Object sender, System.EventArgs e)
        {
            grdMetaData.AllowUserToResizeRows = false;
            grdMetaData.AllowUserToOrderColumns = false;

            SetToolTips();

            cboHorizontalUnits.DataSource = GCDUnits.GCDLinearUnits();
            cboVerticalUnits.DataSource = GCDUnits.GCDLinearUnits();
            cboAreaUnits.DataSource = GCDUnits.GCDAreaUnits();
            cboVolumeUnits.DataSource = GCDUnits.GCDVolumeUnits();

            if (CreateMode)
            {
                this.Text = "Create New GCD Project";
                btnOK.Text = "   Create";
                btnOK.Text = Properties.Resources.CreateButtonText;

                // Default the directory to the parent folder of the last project used.
                if (!string.IsNullOrEmpty(GCDCore.Properties.Settings.Default.LastUsedProjectFolder))
                {
                    string sDir = Path.GetDirectoryName(GCDCore.Properties.Settings.Default.LastUsedProjectFolder);
                    if (Directory.Exists(sDir))
                    {
                        txtDirectory.Text = sDir;
                    }
                }

                GCDUnits.SelectUnit(cboHorizontalUnits, UnitsNet.Units.LengthUnit.Meter);
                GCDUnits.SelectUnit(cboVerticalUnits, UnitsNet.Units.LengthUnit.Meter);
                GCDUnits.SelectUnit(cboAreaUnits, UnitsNet.Units.AreaUnit.SquareMeter);
                GCDUnits.SelectUnit(cboVolumeUnits, UnitsNet.Units.VolumeUnit.CubicMeter);
            }
            else
            {
                this.Text = "GCD Project Properties";
                btnOK.Text = Properties.Resources.UpdateButtonText;

                txtName.Text = ProjectManager.Project.Name;
                txtDirectory.Text = ProjectManager.Project.ProjectFile.DirectoryName;
                txtGCDPath.Text = ProjectManager.Project.ProjectFile.FullName;
                txtDescription.Text = ProjectManager.Project.Description;

                GCDUnits.SelectUnit(cboHorizontalUnits, ProjectManager.Project.Units.HorizUnit);
                GCDUnits.SelectUnit(cboVerticalUnits, ProjectManager.Project.Units.VertUnit);
                GCDUnits.SelectUnit(cboAreaUnits, ProjectManager.Project.Units.ArUnit);
                GCDUnits.SelectUnit(cboVolumeUnits, ProjectManager.Project.Units.VolUnit);
                
                // Only allow the units to be changed if no DEMs have been defined
                int demCount = ProjectManager.Project.DEMSurveys.Count;
                cboHorizontalUnits.Enabled = demCount == 0;
                cboVerticalUnits.Enabled = demCount == 0;

                // Copy the project meta into the binding list
                foreach (KeyValuePair<string, string> kvp in ProjectManager.Project.MetaData)
                {
                    MetaData.Add(new ProjectMetaData(kvp.Key, kvp.Value));
                }

                // Adjust the controls to be read/write only for editing
                txtName.ReadOnly = true;
                txtDirectory.ReadOnly = true;

                // Default focus to the OK button in edit mode
                btnOK.Select();
            }

            // Bind the data grid to the binding list
            grdMetaData.DataSource = MetaData;
        }

        private void SetToolTips()
        {
            tTip.SetToolTip(btnHelp, Properties.Resources.ttpHelp);
            tTip.SetToolTip(txtName, "The name for the GCD project. The name will be used in the folder path for the GCD project parent directory.");
            tTip.SetToolTip(txtDirectory, "The parent folder under which the GCD project folder will be created.");
            tTip.SetToolTip(btnBrowseOutput, "Browse and select a parent directory for the GCD Project.");
            tTip.SetToolTip(txtGCDPath, "Read only folder and file name of the GCD Project file.");
            tTip.SetToolTip(txtDescription, "Information about the GCD project.");
            tTip.SetToolTip(cboHorizontalUnits, "The default units for displaying and outputting change detection results.");

            tTip.SetToolTip(cboHorizontalUnits, "The horizontal, linear units of the raster datasets used in this project. i.e. this should be the same as the map coordinate units.");
            tTip.SetToolTip(cboVerticalUnits, "The vertical units of the raster datasets in this project. i.e. this should be the same the raster cell value units.");
            tTip.SetToolTip(cboAreaUnits, "The areal units for the project. This is typically the square equivalent of the horizontal units.");
            tTip.SetToolTip(cboVolumeUnits, "The volume units for the project. This is typically related to the vertical and areal units.");
        }


        private void btnBrowseOutput_Click(System.Object sender, System.EventArgs e)
        {
            if (CreateMode)
            {
                if (naru.os.Folder.BrowseFolder(txtDirectory, "Select the GCD Project Parent Directory", GCDCore.Properties.Settings.Default.LastUsedProjectFolder) != DialogResult.OK)
                {
                    return;
                }

                DirectoryInfo dFolder = new DirectoryInfo(txtDirectory.Text);
                if (dFolder.Exists)
                {
                    // Folder with GCD projects already in them are not allowed
                    if (dFolder.GetFiles("*.gcd", SearchOption.TopDirectoryOnly).Count() > 0)
                    {
                        MessageBox.Show("The selected folder already contains another GCD project.\nEach GCD project must be created in a separate folder.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (string.IsNullOrEmpty(txtName.Text))
                    {
                        txtName.Text = Path.GetFileName(txtDirectory.Text);
                        btnOK.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("The selected folder does not exist. The project folder must exist before the project is created.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(txtDirectory.Text) && System.IO.Directory.Exists(txtDirectory.Text))
                    System.Diagnostics.Process.Start(txtDirectory.Text);
            }
        }

        private void UpdateGCDPath(object sender, System.EventArgs e)
        {
            string sGCDPath = string.Empty;
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                if (!string.IsNullOrEmpty(txtDirectory.Text))
                {
                    if (Directory.Exists(txtDirectory.Text))
                    {
                        sGCDPath = Path.Combine(txtDirectory.Text, naru.os.File.RemoveDangerousCharacters(txtName.Text));
                        sGCDPath = Path.Combine(sGCDPath, naru.os.File.RemoveDangerousCharacters(txtName.Text));
                        sGCDPath = Path.ChangeExtension(sGCDPath, "gcd");
                    }
                }
            }
            txtGCDPath.Text = sGCDPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>PGB - 5 May 2011
        /// Change this so that the XML file is created when the file is created. This is needed because 
        /// the XML file stores the name of the project, which is required now for the toolbar.</remarks>
        private void btnOK_Click(System.Object sender, System.EventArgs e)
        {
            if (!ValidateForm())
            {
                this.DialogResult = DialogResult.None;
                return;
            }

            try
            {
                Properties.Settings.Default.LastUsedProjectFolder = Path.GetDirectoryName(txtGCDPath.Text);
                Properties.Settings.Default.Save();

                if (CreateMode)
                {
                    // Creating a new project
                    Directory.CreateDirectory(Path.GetDirectoryName(txtGCDPath.Text));
                    System.IO.FileInfo projectFile = new System.IO.FileInfo(txtGCDPath.Text);
                    GCDConsoleLib.GCD.UnitGroup units = GetSelectedUnits();

                    GCDProject project = new GCDProject(txtName.Text, txtDescription.Text, projectFile, DateTime.Now, System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(), UnitsNet.Area.From(0, units.ArUnit), units);

                    UpdateProjectMetdata(project);

                    ProjectManager.CreateProject(project);
                }
                else
                {
                    // Editing properties of existing project
                    ProjectManager.Project.Name = txtName.Text;
                    ProjectManager.Project.Description = txtDescription.Text;

                    // only allowed to change the units if there are no DEMs
                    if (ProjectManager.Project.DEMSurveys.Count < 1)
                    {
                        ProjectManager.Project.Units = GetSelectedUnits();
                    }

                    UpdateProjectMetdata(ProjectManager.Project);
                    ProjectManager.Project.Save();
                }

            }
            catch (Exception ex)
            {
                DialogResult = DialogResult.None;
                ex.Data["Project Name"] = txtName.Text;
                ex.Data["XML File"] = txtGCDPath.Text;
                ex.Data["Directory"] = txtDirectory.Text;
                GCDException.HandleException(ex, "An error occured while trying to save the information");
            }
        }

        /// <summary>
        /// Call this when saving the form data to update the project with the contents of binding list from the data grid
        /// </summary>
        /// <param name="project"></param>
        private void UpdateProjectMetdata(GCDProject project)
        {
            project.MetaData.Clear();
            foreach (ProjectMetaData item in MetaData)
            {
                project.MetaData[item.Key] = item.Value;
            }
        }

        private UnitGroup GetSelectedUnits()
        {
            return new UnitGroup(
                ((GCDUnits.FormattedUnit<UnitsNet.Units.VolumeUnit>)cboVolumeUnits.SelectedItem).Unit,
                ((GCDUnits.FormattedUnit<UnitsNet.Units.AreaUnit>)cboAreaUnits.SelectedItem).Unit,
                ((GCDUnits.FormattedUnit<UnitsNet.Units.LengthUnit>)cboVerticalUnits.SelectedItem).Unit,
                ((GCDUnits.FormattedUnit<UnitsNet.Units.LengthUnit>)cboHorizontalUnits.SelectedItem).Unit
            );
        }

        private bool ValidateForm()
        {

            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Please enter a name for the new project.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Select();
                return false;
            }

            if (txtDirectory.Text.Length < 1)
            {
                MessageBox.Show("Please select an output directory.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnBrowseOutput.Select();
                return false;
            }
            else
            {
                if (!Directory.Exists(txtDirectory.Text))
                {
                    MessageBox.Show("The parent directory must be valid, existing directory.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnBrowseOutput.Select();
                    return false;
                }
            }

            // Only check if the file exists when creating a new one.
            if (CreateMode)
            {
                if (File.Exists(txtGCDPath.Text))
                {
                    MessageBox.Show("There already appears to be a GCD project at the specified path. Change the project name or pick a different parent directory.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            return true;
        }

        private void btnHelp_Click(System.Object sender, System.EventArgs e)
        {
            OnlineHelp.Show(CreateMode ? "NewProject" : "ProjectProperties");
        }

        private void cmdHelpPrecision_Click(System.Object sender, System.EventArgs e)
        {
            UtilityForms.frmInformation frm = new UtilityForms.frmInformation();
            frm.InitializeFixedDialog("Horizontal Decimal Precision", GCDCore.Properties.Resources.PrecisionHelp);
            frm.ShowDialog();
        }

        private class ProjectMetaData
        {
            private string m_Key;

            private string m_Value;
            public string Key
            {
                get { return m_Key; }
                set { m_Key = value; }
            }

            public string Value
            {
                get { return m_Value; }
                set { m_Value = value; }
            }


            public ProjectMetaData()
            {
            }

            public ProjectMetaData(string sKey, string sValue)
            {
                Key = sKey;
                Value = sValue;
            }

        }

        private void frmProjectProperties_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                btnHelp.PerformClick();
            }
        }
    }
}