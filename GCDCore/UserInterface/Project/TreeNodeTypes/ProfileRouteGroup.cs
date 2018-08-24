using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GCDCore.Project;
using GCDCore.Project.ProfileRoutes;

namespace GCDCore.UserInterface.Project.TreeNodeTypes
{
    public class ProfileRouteGroup : TreeNodeGroup
    {
        public ProfileRouteGroup(TreeNodeCollection parentNodes, IContainer container)
            : base(parentNodes, "Profile Routes", "Profile Route", "Profile Routes", ProjectManager.Project.ProfileRoutesFolder, container)
        {

            ContextMenuStrip.Items[0].Visible = false;
            ContextMenuStrip.Items.Insert(0, new ToolStripMenuItem("Add Existing Longitudinal Profile Route", Properties.Resources.Add, OnAddLongitudinal));
            ContextMenuStrip.Items.Insert(0, new ToolStripMenuItem("Add Existing Transect Profile Route", Properties.Resources.Add, OnAddTransect));

            LoadChildNodes();
        }

        public override void LoadChildNodes()
        {
            Nodes.Clear();

            foreach (ProfileRoute route in ProjectManager.Project.ProfileRoutes)
            {
                TreeNodeItem nodMask = new TreeNodeItem(route, route.ProfileRouteType == ProfileRoute.ProfileRouteTypes.Transect ? 15 : 18, ContextMenuStrip.Container);
                Nodes.Add(nodMask);
            }

            if (Nodes.Count > 0)
                Expand();
        }

        public override void OnAdd(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnAddTransect(object sender, EventArgs e)
        {
            ProfileRoutes.frmProfileRouteProperties frm = new ProfileRoutes.frmProfileRouteProperties(ProfileRoute.ProfileRouteTypes.Transect, null);
            EditTreeItem(frm);
        }

        public void OnAddLongitudinal(object sender, EventArgs e)
        {
            ProfileRoutes.frmProfileRouteProperties frm = new ProfileRoutes.frmProfileRouteProperties(ProfileRoute.ProfileRouteTypes.Longitudinal, null);
            EditTreeItem(frm);
        }
    }
}
