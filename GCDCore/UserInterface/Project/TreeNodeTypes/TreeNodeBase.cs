using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            DialogResult eResult = DialogResult.OK;
            try
            {
                eResult = frm.ShowDialog();
                if (eResult == DialogResult.OK && treeReload)
                {
                    LoadChildNodes();
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "Error Editing GCD Project Item");
            }

            return eResult;
        }
    }
}
