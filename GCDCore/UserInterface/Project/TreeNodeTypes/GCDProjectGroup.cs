using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project;
using System.ComponentModel;

namespace GCDCore.UserInterface.Project.TreeNodeTypes
{
    public class GCDProjectGroup : TreeNodeGroup
    {
        public GCDProjectGroup(IContainer container)
            : base(ProjectManager.Project.Name, string.Empty, string.Empty, container)
        {
            ContextMenuStrip.Items.Clear();



        }
    }
}
