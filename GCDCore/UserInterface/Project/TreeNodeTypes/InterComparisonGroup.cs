using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GCDCore.Project;

namespace GCDCore.UserInterface.Project.TreeNodeTypes
{
    public class InterComparisonGroup : TreeNodeGroup
    {
        public InterComparisonGroup(TreeNodeCollection parentNodes, IContainer container)
            : base(parentNodes, "Inter-Comparisons", "Inter-Comparison", "Inter-Comparisons", ProjectManager.Project.InterComparisonsFolder, container)
        {
            // Inter-comparisons are "calculated not added
            ContextMenuStrip.Items.Clear();
            ContextMenuStrip.Items.Add("Create Change Detection Inter-Comparison", Properties.Resources.sigma, OnAdd);
            ContextMenuStrip.Items.Add("-"); // Separator
            ContextMenuStrip.Items.Add(string.Format("Explore {0} Folder", NounPlural), Properties.Resources.BrowseFolder, OnExplore);

            LoadChildNodes();
        }

        public override void LoadChildNodes()
        {
            Nodes.Clear();

            // Add all the project inter-comparisons

            foreach (InterComparison ic in ProjectManager.Project.InterComparisons)
            {
                TreeNodeItem nodIC = new TreeNodeItem(ic, 10, ContextMenuStrip.Container);
                nodIC.ContextMenuStrip.Items.RemoveAt(0);
                nodIC.ContextMenuStrip.Items.Insert(0, new ToolStripMenuItem("View Inter-Comparison folder", Properties.Resources.GCD, OnViewResults));
                Nodes.Add(nodIC);
            }

            if (Nodes.Count > 0)
                Expand();
        }

        public void OnViewResults(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            ContextMenuStrip cms = tsmi.Owner as ContextMenuStrip;
            TreeView projectTree = cms.SourceControl as TreeView;
            TreeNodeItem nodIC = projectTree.SelectedNode as TreeNodeItem;
            InterComparison ic = nodIC.Item as InterComparison;
            if (ic._SummaryXML.Directory.Exists)
            {
                System.Diagnostics.Process.Start(ic._SummaryXML.Directory.FullName);
            }
        }

        public override void OnAdd(object sender, EventArgs e)
        {
            if (ProjectManager.Project.DoDs.Count < 2)
            {
                MessageBox.Show("Your project must contain at least two change detection analyses before you can perform an inter-comparison.", Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            UserInterface.ChangeDetection.Intercomparison.frmInterComparisonProperties frm = new ChangeDetection.Intercomparison.frmInterComparisonProperties();
            EditTreeItem(frm);
        }
    }
}
