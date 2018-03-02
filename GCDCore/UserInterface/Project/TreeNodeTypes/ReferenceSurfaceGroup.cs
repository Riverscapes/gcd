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
             : base(parentNodes, "Reference Surfaces", "Reference Surface", "Reference Surfaces", ProjectManager.Project.ReferenceSurfacesFolder, container, ProjectManager.Project.ReferenceSurfaces.Count > 0)
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

                TreeNodeGroup nodError = new ErrorSurfaceGroup(nodSurface.Nodes, ContextMenuStrip.Container, surf);
            }

            if (Nodes.Count > 0)
                Expand();
        }

        public override void OnAdd(object sender, EventArgs e)
        {
            DEMSurvey referenceDEM = null;
            if (ProjectManager.Project.DEMSurveys.Count > 0)
            {
                referenceDEM = ProjectManager.Project.DEMSurveys.Values.First();
            }
            else
            {
                MessageBox.Show("You must have at least one DEM survey in your GCD project before you can generate a constant reference surface.", "DEM Surveys Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                SurveyLibrary.frmImportRaster frm = new SurveyLibrary.frmImportRaster(referenceDEM,  SurveyLibrary.ExtentImporter.Purposes.ReferenceSurface, "Reference Surface");
                if (EditTreeItem(frm, false) == DialogResult.OK)
                {
                    GCDConsoleLib.Raster rDEM = frm.ProcessRaster();
                    GCDCore.Project.Surface surf = new Surface(frm.txtName.Text, rDEM.GISFileInfo, Surface.HillShadeRasterPath(rDEM.GISFileInfo));
                    ProjectManager.Project.ReferenceSurfaces[surf.Name] = surf;

                    ProjectManager.Project.Save();
                    LoadChildNodes();

                    // Loop through the child nodes and select the item that was just added
                    foreach (TreeNodeItem childNode in Nodes)
                    {
                        if (childNode.Item.Equals(surf))
                        {
                            TreeView.SelectedNode = childNode;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex, "Error Importing DEM Survey");
            }
        }

        public void OnDeriveFromDEMs(object sender, EventArgs e)
        {
            if (ProjectManager.Project.DEMSurveys.Count < 2)
            {
                MessageBox.Show("You must have at least two DEM surveys in your GCD project before you can generate a reference surface from DEM surveys.",
                    "DEM Surveys Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SurveyLibrary.ReferenceSurfaces.frmReferenceSurfaceFromDEMs frm = new SurveyLibrary.ReferenceSurfaces.frmReferenceSurfaceFromDEMs();
            EditTreeItem(frm);
        }

        public void OnDeriveConstant(object sender, EventArgs e)
        {
            if (ProjectManager.Project.DEMSurveys.Count < 1)
            {
                MessageBox.Show("You must have at least one DEM survey in your GCD project before you can generate a constant reference surface.", "DEM Surveys Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SurveyLibrary.ReferenceSurfaces.frmReferenceSurfaceFromConstant frm = new SurveyLibrary.ReferenceSurfaces.frmReferenceSurfaceFromConstant();
            EditTreeItem(frm);
        }
    }
}
