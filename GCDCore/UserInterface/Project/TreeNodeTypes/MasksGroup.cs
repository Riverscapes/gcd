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
            : base(parentNodes, "Masks", "Mask", "Masks", container, ProjectManager.Project.Masks.Count > 0)
        {
            ContextMenuStrip.Items[0].Visible = false; // Hide the generic "add"
            ContextMenuStrip.Items.Insert(1, new ToolStripMenuItem("Add Existing Regular Mask", Properties.Resources.Add, OnAddRegularMask));
            ContextMenuStrip.Items.Insert(2, new ToolStripMenuItem("Add Existing Directional Mask", Properties.Resources.Add, OnAddDirectionalMask));
            ContextMenuStrip.Items.Insert(2, new ToolStripMenuItem("Add Existing Area Of Interest Mask", Properties.Resources.Add, OnAddAOIMask));

            foreach (GCDCore.Project.Masks.Mask mask in ProjectManager.Project.Masks.Values)
            {
                int imageIndex = 6;
                if (mask is GCDCore.Project.Masks.DirectionalMask)
                    imageIndex = 13;
                else if (mask is GCDCore.Project.Masks.AOIMask)
                    imageIndex = 14;

                TreeNodeItem nodMask = new TreeNodeTypes.TreeNodeItem(mask, imageIndex, container);
                Nodes.Add(nodMask);                
            }
        }

        public override void OnAdd(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnAddRegularMask(object sender, EventArgs e)
        {

        }

        public void OnAddDirectionalMask(object sender, EventArgs e)
        {

        }

        public void OnAddAOIMask(object sender, EventArgs e)
        {

        }
    }
}
