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
    }
}
