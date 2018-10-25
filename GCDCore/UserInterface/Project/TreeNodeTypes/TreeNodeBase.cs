using System;
using System.Windows.Forms;
using GCDCore.Project;

namespace GCDCore.UserInterface.Project.TreeNodeTypes
{
    public abstract class TreeNodeBase : TreeNode
    {
        public readonly string NounSingle;
        public readonly string NounPlural;

        public TreeNodeBase(string name, string nounSingle, string noundPlural, int imageindex)
            : base(name, imageindex, imageindex)
        {
            NounSingle = nounSingle;
            NounPlural = noundPlural;
        }

        /// <summary>
        /// Each inherited class must implement the LoadChildNodes method that knows
        /// how to load the child nodes of the relevant type. This is called during
        /// construction and also after OnAdd 
        /// </summary>
        public abstract void LoadChildNodes();

        public void cms_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (ToolStripItem tsmi in ContextMenuStrip.Items)
            {
                // ignore separator
                if (!(tsmi is ToolStripMenuItem))
                    continue;

                if (tsmi.Text.ToLower().Contains("map"))
                {
                    tsmi.Visible = ProjectManager.IsArcMap;
                }
                else if (tsmi.Text.ToLower().Contains("explore"))
                {
                    ContextMenuStrip cms = sender as ContextMenuStrip;
                    TreeView treProject = cms.SourceControl as TreeView;
                    TreeNodeGroup nodGroup = treProject.SelectedNode as TreeNodeGroup;
                    if (nodGroup.Folder is System.IO.DirectoryInfo)
                    {
                        nodGroup.Folder.Refresh();
                        tsmi.Enabled = nodGroup.Folder.Exists;
                    }
                    else
                    {
                        tsmi.Visible = false;
                    }
                }
            }
        }

        protected DialogResult EditTreeItem(Form frm, bool treeReload = true)
        {
            if (frm == null)
                return DialogResult.Abort;

            DialogResult eResult = DialogResult.OK;
            try
            {
                eResult = frm.ShowDialog();
                if (eResult == DialogResult.OK && treeReload)
                {
                    IProjectItemForm iForm = frm as IProjectItemForm;
                    if (iForm != null)
                    {
                        // Get the GCD project item that was just added or edited
                        GCDProjectItem changedItem = iForm.GCDProjectItem;

                        // Selected this node if it is an item node and was just edited
                        if (this is TreeNodeItem && ((TreeNodeItem)this).Item.Equals(changedItem))
                        {
                            Text = changedItem.Name;
                        }
                        else
                        {
                            // Polymorphic loading of relevant child nodes
                            LoadChildNodes();

                            // Loop through the child nodes and select the item that was just added
                            foreach (TreeNode childNode in Nodes)
                            {
                                if (childNode is TreeNodeItem)
                                {
                                    if (((TreeNodeItem)childNode).Item.Equals(changedItem))
                                    {
                                        TreeView.SelectedNode = childNode;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GCDException.HandleException(ex, "Error Editing GCD Project Item");
            }

            return eResult;
        }
    }
}
