using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using GCDCore.Project;

namespace GCDCore.UserInterface
{
    public partial class ucProjectItemName : UserControl
    {
        public string Noun { get; internal set; }
        public List<string> ExistingNames { get; internal set; }

        // Properties needed to determine new folder path
        private DirectoryInfo ParentFolder { get; set; }
        private string DirectoryPrefix;
        private string FileExtension;

        public string ItemName {  get { return txtName.Text; } }

        /// <summary>
        /// This is the height that parent forms need to reduce their height by when
        /// this user control is in existing format and the path control is hidden
        /// </summary>
        public int VariableHeight { get { return txtPath.Bottom - txtName.Bottom; } }

        private FileInfo _FileInfo;
        public FileInfo AbsolutePath
        {
            get { return _FileInfo; }

            internal set
            {
                if (value == null)
                {
                    _FileInfo = null;
                    txtPath.Text = string.Empty;
                }
                else
                {
                    _FileInfo = value;
                    txtPath.Text = ProjectManager.Project.GetRelativePath(value);
                }
            }
        }

        public string RelativePath { get { return txtPath.Text; } }

        public ucProjectItemName()
        {
            InitializeComponent();
        }

        public void InitializeExisting(string noun, List<string> existingNames, string name, FileInfo fiPath)
        {
            Noun = noun;
            ExistingNames = existingNames;
            txtName.Text = name;
            AbsolutePath = fiPath;
            lblPath.Visible = false;
            txtPath.Visible = false;
        }

        public void InitializeNewRaster(string noun, List<string> existingNames, DirectoryInfo parentDir, string directoryPrefix)
        {
            InitializeNew(noun, existingNames, parentDir, directoryPrefix, ProjectManager.RasterExtension);
        }

        public void InitializeNewVector(string noun, List<string> existingNames, DirectoryInfo parentDir, string directoryPrefix)
        {
            InitializeNew(noun, existingNames, parentDir, directoryPrefix, "shp");
        }

        private void InitializeNew(string noun, List<string> existingNames, DirectoryInfo parentDir, string directoryPrefix, string fileExtension)
        {
            Noun = noun;
            ExistingNames = existingNames;
            ParentFolder = parentDir;
            DirectoryPrefix = directoryPrefix;
            FileExtension = fileExtension;

            // Only hook the name changed event when creating a new item
            txtName.TextChanged += txtNamed_Changed;
        }

        private void txtNamed_Changed(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                AbsolutePath = ProjectManager.GetProjectItemPath(ParentFolder, DirectoryPrefix, txtName.Text, FileExtension);
            }
        }

        public bool ValidateForm()
        {
            // Sanity check to avoid empty string names
            txtName.Text = txtName.Text.Trim();

            if ((string.IsNullOrEmpty(txtName.Text.Trim())))
            {
                MessageBox.Show(string.Format("You must provide a unique name for the {0}.", Noun), GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Select();
                return false;
            }

            if (ExistingNames.Contains(txtName.Text))
            {
                MessageBox.Show(string.Format("There is another {0} in this GCD Project that possesses this name. You must provide a unique name for the {0}.", Noun), GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Select(txtName.Text.Length, 0);
                return false;
            }

            return true;
        }
    }
}
