using GCDCore.Project;
using System;

using System.Windows.Forms;

namespace GCDCore.UserInterface.Project
{
    public partial class ucProjectExplorer
    {
        public event ProjectTreeNodeSelectionChangeEventHandler ProjectTreeNodeSelectionChange;
        public delegate void ProjectTreeNodeSelectionChangeEventHandler(object sender, EventArgs e);

        private static SortSurveyBy m_eSortBy = SortSurveyBy.SurveyDateDsc;

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

            if (ProjectManager.Project == null)
                return;

            TreeNodeTypes.TreeNodeGroup nodProj = new TreeNodeTypes.GCDProjectGroup(treProject, components);
        }

        private void treProject_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            TreeNode theNode = treProject.GetNodeAt(e.X, e.Y);
            if (theNode is TreeNode)
                treProject.SelectedNode = theNode;
        }
    }
}