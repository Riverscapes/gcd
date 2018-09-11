using GCDCore.Project;
using System;

using System.Windows.Forms;

namespace GCDCore.UserInterface.Project
{
    public partial class ucProjectExplorer
    {
        //public event ProjectTreeNodeSelectionChangeEventHandler ProjectTreeNodeSelectionChange;
        public delegate void ProjectTreeNodeSelectionChangeEventHandler(object sender, EventArgs e);

        //private static SortSurveyBy m_eSortBy = SortSurveyBy.SurveyDateDsc;

        public enum SortSurveyBy
        {
            NameAsc,
            NameDsc,
            SurveyDateAsc,
            SurveyDateDsc
        }

        public ucProjectExplorer()
        {
            InitializeComponent();
        }

        private void ProjectExplorerUC_Load(object sender, System.EventArgs e)
        {

        }

        public void LoadTree()
        {
            treProject.Nodes.Clear();

            System.Windows.Forms.MessageBox.Show("Before Load Project Tree Object Check", "Diagnostic Message");

            System.Windows.Forms.MessageBox.Show(ProjectManager.Project.ToString(), "Project Item Status");

            if (ProjectManager.Project == null)
                return;

            System.Windows.Forms.MessageBox.Show("After Load Project Tree Object Check", "Diagnostic Message");

            TreeNodeTypes.TreeNodeGroup nodProj = new TreeNodeTypes.GCDProjectGroup(treProject, components);

            System.Windows.Forms.MessageBox.Show("After Project Tree Node Loading", "Diagnostic Message");
        }

        private void treProject_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }

        private void treProject_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            TreeNode theNode = treProject.GetNodeAt(e.X, e.Y);
            if (theNode is TreeNode)
                treProject.SelectedNode = theNode;

        }
    }
}