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
    }
}
