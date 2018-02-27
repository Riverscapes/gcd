using System;
using System.Collections.Generic;
using System.Linq;
using GCDCore.Project;
using System.ComponentModel;
using System.Windows.Forms;

namespace GCDCore.UserInterface.Project.TreeNodeTypes
{
    public class MasksGroup : TreeNodeGroup
    {
        public MasksGroup(TreeNodeCollection parentNodes, IContainer container)
            : base(parentNodes, "Masks", "Mask", "Masks", ProjectManager.Project.MasksFolder, container, ProjectManager.Project.Masks.Count > 0)
        {
            ContextMenuStrip.Items[0].Text = "Add Existing Regular Mask";
            ContextMenuStrip.Items.Insert(1, new ToolStripMenuItem("Add Existing Directional Mask", Properties.Resources.Add, OnAddDirectionalMask));
            ContextMenuStrip.Items.Insert(2, new ToolStripMenuItem("Add Existing Area Of Interest Mask", Properties.Resources.Add, OnAddAOIMask));

            LoadChildNodes();
        }

        public override void LoadChildNodes()
        {
            Nodes.Clear();

            foreach (GCDCore.Project.Masks.Mask mask in ProjectManager.Project.Masks.Values)
            {
                int imageIndex = 6;
                if (mask is GCDCore.Project.Masks.DirectionalMask)
                    imageIndex = 13;
                else if (mask is GCDCore.Project.Masks.AOIMask)
                    imageIndex = 14;

                TreeNodeItem nodMask = new TreeNodeTypes.TreeNodeItem(mask, imageIndex, ContextMenuStrip.Container);
                Nodes.Add(nodMask);
            }

            if (Nodes.Count > 0)
                Expand();
        }

        public override void OnAdd(object sender, EventArgs e)
        {
            Masks.frmMaskProperties frm = new Masks.frmMaskProperties();
            EditTreeItem(frm);
        }

        public void OnAddDirectionalMask(object sender, EventArgs e)
        {
            Masks.frmDirectionalMaskProps frm = new Masks.frmDirectionalMaskProps();
            EditTreeItem(frm);
        }

        public void OnAddAOIMask(object sender, EventArgs e)
        {
            Masks.frmAOIProperties frm = new Masks.frmAOIProperties(null);
            EditTreeItem(frm);
        }
    }
}
