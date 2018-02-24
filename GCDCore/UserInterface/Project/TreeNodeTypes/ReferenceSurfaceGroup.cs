using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project;

namespace GCDCore.UserInterface.Project.TreeNodeTypes
{
    public class ReferenceSurfaceGroup : TreeNodeGroup
    {
        public ReferenceSurfaceGroup(IContainer container)
             : base("Reference Surfaces", "Reference Surface", "Reference Surfaces", container, ProjectManager.Project.ReferenceSurfaces.Count > 0)
        {
            ContextMenuStrip.Items.Add("Derive Reference Surface From DEM Surveys", Properties.Resources.sigma, OnDeriveFromDEMs);
            ContextMenuStrip.Items.Add("Derive Constant Reference Surface", Properties.Resources.sigma, OnDeriveConstant);

            foreach (Surface surf in ProjectManager.Project.ReferenceSurfaces.Values)
            {
                TreeNodeItem nodSurface = new TreeNodeItem(surf, 5, container);
                Nodes.Add(nodSurface);

                TreeNodeGroup nodError = new TreeNodeGroup("Error Surfaces", "Error Surface", "Error Surfaces", container, surf.ErrorSurfaces.Count > 0);
                nodSurface.Nodes.Add(nodError);
                surf.ErrorSurfaces.ToList().ForEach(x => nodError.Nodes.Add(new TreeNodeItem(x, 4, container)));
            }
        }

        public void OnDeriveFromDEMs(object sender, EventArgs e)
        {

        }

        public void OnDeriveConstant(object sender, EventArgs e)
        {

        }
    }
}
