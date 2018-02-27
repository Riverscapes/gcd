using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GCDCore.UserInterface.Project.TreeNodeTypes
{
    public class GenericNodeGroup : TreeNodeGroup
    {

        public GenericNodeGroup(TreeNodeCollection parentNodes, string name, string nounSingle, string nounPlural, DirectoryInfo folder, IContainer container, bool expand = true, int imageindex = 0)
            : base(parentNodes, name, nounSingle, nounPlural, folder, container, expand, imageindex)
        {

        }

        public override void LoadChildNodes()
        {
            throw new NotImplementedException();
        }

        public override void OnAdd(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
