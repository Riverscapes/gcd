using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GCDCore.Project;

namespace GCDCore.UserInterface.Project.TreeNodeTypes
{
    public class TreeNodeGroup : TreeNodeBase
    {
        public TreeNodeGroup(string name, string nounSingle, string nounPlural, System.ComponentModel.IContainer container, bool expand = true, int imageIndex = 0)
            : base(name, nounSingle, nounPlural, imageIndex)
        {
            ContextMenuStrip = new ContextMenuStrip(container);
            ContextMenuStrip.Items.Add(string.Format("Add {0}", NounSingle), Properties.Resources.Add, OnAdd);
            ContextMenuStrip.Items.Add(string.Format("Add all {0} to the Map", NounPlural), Properties.Resources.AddToMap, OnAddToMap);
            ContextMenuStrip.Items.Add("Collapse Child Items", Properties.Resources.collapse, OnCollapseChildren);

            if (expand)
                Expand();
        }

        public void OnAdd(object sender, EventArgs e)
        {

        }

        public void OnAddToMap(object sender, EventArgs e)
        {
            foreach (TreeNode childNode in Nodes)
            {
                if (childNode.Tag is GCDProjectRasterItem)
                {
                    ProjectManager.OnAddRasterToMap(childNode.Tag as GCDProjectRasterItem);
                }
            }
        }

        public void OnCollapseChildren(object sender, EventArgs e)
        {
            foreach (TreeNode childNode in Nodes)
            {
                childNode.Collapse();
            }
        }
    }
}
