using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project;
using System.Windows.Forms;

namespace GCDCore.UserInterface.Project.TreeNodeTypes
{
    public class ReferenceSurfaceGroup : TreeNodeGroup
    {
        public ReferenceSurfaceGroup(TreeNodeCollection parentNodes, IContainer container)
             : base(parentNodes, "Reference Surfaces", "Reference Surface", "Reference Surfaces", container, ProjectManager.Project.ReferenceSurfaces.Count > 0)
        {
            ContextMenuStrip.Items.Insert(1, new ToolStripMenuItem("Calculate New Reference Surface From DEM Surveys", Properties.Resources.sigma, OnDeriveFromDEMs));
            ContextMenuStrip.Items.Insert(2, new ToolStripMenuItem("Calculate New Constant Reference Surface(s)", Properties.Resources.sigma, OnDeriveConstant));

            LoadChildNodes();
        }

        public override void LoadChildNodes()
        {
            foreach (Surface surf in ProjectManager.Project.ReferenceSurfaces.Values)
            {
                TreeNodeItem nodSurface = new TreeNodeItem(surf, 5, ContextMenuStrip.Container);
                Nodes.Add(nodSurface);

                TreeNodeGroup nodError = new GenericNodeGroup(nodSurface.Nodes, "Error Surfaces", "Error Surface", "Error Surfaces", ContextMenuStrip.Container, surf.ErrorSurfaces.Count > 0);
                surf.ErrorSurfaces.ToList().ForEach(x => nodError.Nodes.Add(new TreeNodeItem(x, 4, ContextMenuStrip.Container)));
            }

            if (Nodes.Count > 0)
                Expand();
        }

        public override void OnAdd(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnDeriveFromDEMs(object sender, EventArgs e)
        {
            SurveyLibrary.ReferenceSurfaces.frmReferenceSurfaceFromDEMs frm = new SurveyLibrary.ReferenceSurfaces.frmReferenceSurfaceFromDEMs();
            EditTreeItem(frm);
        }

        public void OnDeriveConstant(object sender, EventArgs e)
        {
            SurveyLibrary.ReferenceSurfaces.frmReferenceSurfaceFromConstant frm = new SurveyLibrary.ReferenceSurfaces.frmReferenceSurfaceFromConstant();
            EditTreeItem(frm);
        }
    }
}
