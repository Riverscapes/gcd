using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using GCDCore.Project;
using System.Windows.Forms;
using System.ComponentModel;

namespace GCDCore.UserInterface.Project.TreeNodeTypes
{
    public class LinearExtractionGrp : TreeNodeGroup
    {
        public GCDProjectItem Surface { get; internal set; } // could also be DoD

        public LinearExtractionGrp(TreeNodeCollection nodParents, GCDProjectItem parentSurface, DirectoryInfo dir, IContainer container)
            : base(nodParents, "Linear Extractions", "Linear Extraction", "Linear Extractions", dir, container)
        {
            Surface = parentSurface;

            ContextMenuStrip.Items.RemoveAt(0);
            ContextMenuStrip.Items.Insert(0, new ToolStripMenuItem("Calculate Linear Extraction From Profile Route", Properties.Resources.Add, OnAdd));

            LoadChildNodes();
        }

        public override void LoadChildNodes()
        {
            Nodes.Clear();

            List<GCDCore.Project.LinearExtraction.LinearExtraction> LEs = null;
            if (Surface is Surface)
            {
                LEs = ((Surface)Surface).LinearExtractions;
            }
            else if (Surface is DoDBase)
            {
                LEs = ((DoDBase)Surface).LinearExtractions;
            }

            foreach (GCDCore.Project.LinearExtraction.LinearExtraction le in LEs)
            {
                TreeNodeItem nod = new TreeNodeItem(le, 16, ContextMenuStrip.Container);
                nod.ContextMenuStrip.Items.Insert(0, new ToolStripMenuItem("View Linear Extraction Folder", Properties.Resources.GCD, OnViewResults));
                Nodes.Add(nod);
            }
        }

        public override void OnAdd(object sender, EventArgs e)
        {
            if (ProjectManager.Project.ProfileRoutes.Count < 1)
            {
                MessageBox.Show("You must create at least one profile route before you can perform a linear extraction.", "Insufficient Profile Routes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            LinearExtraction.frmLinearExtractionProperties frm = new LinearExtraction.frmLinearExtractionProperties(Surface);
            EditTreeItem(frm);
        }


        public void OnViewResults(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            ContextMenuStrip cms = tsmi.Owner as ContextMenuStrip;
            TreeView projectTree = cms.SourceControl as TreeView;
            TreeNodeItem nodIC = projectTree.SelectedNode as TreeNodeItem;
            GCDCore.Project.LinearExtraction.LinearExtraction le = nodIC.Item as GCDCore.Project.LinearExtraction.LinearExtraction;
            if (le.Database.Directory.Exists)
            {
                System.Diagnostics.Process.Start(le.Database.Directory.FullName);
            }
        }
    }
}
