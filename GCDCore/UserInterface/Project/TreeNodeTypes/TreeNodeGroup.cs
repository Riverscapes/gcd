using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GCDCore.Project;

namespace GCDCore.UserInterface.Project.TreeNodeTypes
{
    public abstract class TreeNodeGroup : TreeNodeBase
    {
        public readonly DirectoryInfo Folder;

        public TreeNodeGroup(TreeNodeCollection parentNodes, string name, string nounSingle, string nounPlural, DirectoryInfo folder, System.ComponentModel.IContainer container, bool expand = true, int imageIndex = 0)
            : base(name, nounSingle, nounPlural, imageIndex)
        {
            Folder = folder;

            ContextMenuStrip = new ContextMenuStrip(container);
            ContextMenuStrip.Items.Add(string.Format("Add Existing {0}", NounSingle), Properties.Resources.Add, OnAdd);

            ContextMenuStrip.Items.Add("-"); // Separator
            ContextMenuStrip.Items.Add(string.Format("Explore {0} Folder", NounPlural), Properties.Resources.BrowseFolder, OnExplore);

            ContextMenuStrip.Items.Add(string.Format("Add all {0} to the Map", NounPlural), Properties.Resources.AddToMap, OnAddToMap);
            ContextMenuStrip.Items.Add("Collapse Child Items", Properties.Resources.collapse, OnCollapseChildren);

            // Hookup the opening event to handle status
            ContextMenuStrip.Opening += cms_Opening;

            parentNodes.Add(this);

            if (expand)
                Expand();
        }



        /// <summary>
        /// Each interited class must implement the OnAdd method that instantiates the 
        /// Appropriate form to add an item of the relevant type 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public abstract void OnAdd(object sender, EventArgs e);

        public void OnAddToMap(object sender, EventArgs e)
        {
            foreach (TreeNode childNode in Nodes)
            {
                if (childNode is TreeNodeItem)
                {
                    TreeNodeItem nodItem = childNode as TreeNodeItem;
                    if (nodItem.Item is GCDProjectRasterItem)
                    {
                        ProjectManager.OnAddRasterToMap(nodItem.Item as GCDProjectRasterItem);
                    }
                    else if (nodItem.Item is GCDCore.Project.Masks.Mask)
                    {
                        ProjectManager.OnAddVectorToMap(nodItem.Item as GCDCore.Project.Masks.Mask);
                    }
                }
            }
        }

        public void OnExplore(object sender, EventArgs e)
        {
            if (Folder.Exists)
            {
                System.Diagnostics.Process.Start(Folder.FullName);
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
