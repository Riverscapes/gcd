using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GCDCore.Project;

namespace GCDCore.UserInterface.Project.TreeNodeTypes
{
    public class ProfileRouteGroup : TreeNodeGroup
    {
        public ProfileRouteGroup(TreeNodeCollection parentNodes, IContainer container)
            : base(parentNodes, "Profile Routes", "Profile Route", "Profile Routes", ProjectManager.Project.ProfileRoutesFolder, container)
        {
            LoadChildNodes();
        }

        public override void LoadChildNodes()
        {
            Nodes.Clear();

            foreach (GCDCore.Project.ProfileRoutes.ProfileRoute route in ProjectManager.Project.ProfileRoutes.Values)
            {
                TreeNodeItem nodMask = new TreeNodeTypes.TreeNodeItem(route, 15, ContextMenuStrip.Container);
                Nodes.Add(nodMask);
            }

            if (Nodes.Count > 0)
                Expand();
        }

        public override void OnAdd(object sender, EventArgs e)
        {
            ProfileRoutes.frmProfileRouteProperties frm = new ProfileRoutes.frmProfileRouteProperties(null);
            EditTreeItem(frm);
        }
    }
}
