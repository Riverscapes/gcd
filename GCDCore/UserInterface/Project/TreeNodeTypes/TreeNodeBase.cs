using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GCDCore.UserInterface.Project.TreeNodeTypes
{
    public class TreeNodeBase : TreeNode
    {
        public readonly string NounSingle;
        public readonly string NounPlural;

        public TreeNodeBase(string name, string nounSingle, string noundPlural, int imageindex)
            : base(name, imageindex, imageindex)
        {
            NounSingle = nounSingle;
            NounPlural = noundPlural;
        }

        protected void LoadTree()
        {
            ucProjectExplorer pe = TreeView.Parent as ucProjectExplorer;
            pe.LoadTree(null, null);
        }

        protected DialogResult EditTreeItem(Form frm, bool treeReload = true)
        {
            DialogResult eResult = DialogResult.OK;
            try
            {
                eResult = frm.ShowDialog();
                if (eResult == DialogResult.OK && treeReload)
                {
                    LoadTree();
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
