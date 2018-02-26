using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project;
using System.ComponentModel;
using System.Windows.Forms;

namespace GCDCore.UserInterface.Project.TreeNodeTypes
{
    public class GCDProjectGroup : TreeNodeGroup
    {
        public GCDProjectGroup(TreeView treeView, IContainer container)
            : base(treeView.Nodes, ProjectManager.Project.Name, string.Empty, string.Empty, container, true, 1)
        {
            ContextMenuStrip.Items.Clear();
            ContextMenuStrip.Items.Add("Edit Project Properties", Properties.Resources.Options, OnEditProperties);
            ContextMenuStrip.Items.Add("Explore Project Folder", Properties.Resources.BrowseFolder, OnExplore);
            ContextMenuStrip.Items.Add("Refresh Project Tree", Properties.Resources.refresh, OnRefresh);
        }

        public void OnEditProperties(object sender, EventArgs e)
        {
            frmProjectProperties frm = new frmProjectProperties();
            EditTreeItem(frm);
        }

        public void OnExplore(object sender, EventArgs e)
        {
            if (ProjectManager.Project.ProjectFile.Directory.Exists)
            {
                System.Diagnostics.Process.Start(ProjectManager.Project.ProjectFile.Directory.FullName);
            }
        }

        public void OnRefresh(object sender, EventArgs e)
        {
            try
            {
                LoadTree();
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "Error Editing GCD Project Item");
            }
        }

        public override void OnAdd(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
