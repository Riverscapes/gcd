using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project;
using System.ComponentModel;
using System.Windows.Forms;

namespace GCDCore.UserInterface.Project.TreeNodeTypes
{
    public class GCDProjectGroup : TreeNodeGroup
    {
        public GCDProjectGroup(TreeNodeCollection parentNodes, IContainer container)
            : base(parentNodes, ProjectManager.Project.Name, string.Empty, string.Empty, container)
        {
            ContextMenuStrip.Items.Clear();
        }
    }
}
