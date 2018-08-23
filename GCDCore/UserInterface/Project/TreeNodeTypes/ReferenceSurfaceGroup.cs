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
            Nodes.Clear();

            foreach (Surface surf in ProjectManager.Project.ReferenceSurfaces)
            {
                TreeNodeItem nodSurface = new TreeNodeItem(surf, 5, ContextMenuStrip.Container);
                Nodes.Add(nodSurface);
                nodSurface.ContextMenuStrip.Items.Insert(1, new ToolStripMenuItem("Calculate Linear Extraction From Profile Route", Properties.Resources.Add, OnLinear));


                TreeNodeGroup nodError = new ErrorSurfaceGroup(nodSurface.Nodes, ContextMenuStrip.Container, surf);

                if (surf.LinearExtractions.Count > 0)
                {
                    TreeNodeGroup nodLinea = new LinearExtractionGrp(nodSurface.Nodes, surf, surf.Raster.GISFileInfo.Directory, ContextMenuStrip.Container);
                    nodLinea.Expand();
                }
            }

            if (Nodes.Count > 0)
                Expand();
        }

        public override void OnAdd(object sender, EventArgs e)
        {
            DEMSurvey referenceDEM = null;
            if (ProjectManager.Project.DEMSurveys.Count > 0)
            {
                referenceDEM = ProjectManager.Project.DEMSurveys.First();
            }
            else
            {
                MessageBox.Show("You must have at least one DEM survey in your GCD project before you can generate a constant reference surface.", "DEM Surveys Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                SurveyLibrary.frmImportRaster frm = new SurveyLibrary.frmImportRaster(referenceDEM, SurveyLibrary.ExtentImporter.Purposes.ReferenceSurface, "Reference Surface");
                if (EditTreeItem(frm, false) == DialogResult.OK)
                {
                    GCDConsoleLib.Raster rDEM = frm.ProcessRaster();
                    GCDCore.Project.Surface surf = new Surface(frm.txtName.Text, rDEM.GISFileInfo, Surface.HillShadeRasterPath(rDEM.GISFileInfo));
                    ProjectManager.Project.ReferenceSurfaces.Add(surf);

                    ProjectManager.Project.Save();
                    LoadChildNodes();

                    ProjectManager.AddNewProjectItemToMap(surf);

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
                GCDException.HandleException(ex, "Error Importing DEM Survey");
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

        private void OnLinear(object sender, EventArgs e)
        {
            ToolStripDropDownItem ctrl = sender as ToolStripDropDownItem;
            ContextMenuStrip cms = ctrl.Owner as ContextMenuStrip;
            TreeView tre = cms.SourceControl as TreeView;
            TreeNodeItem nodSurf = tre.SelectedNode as TreeNodeItem;

            LinearExtraction.frmLinearExtractionProperties frm = new LinearExtraction.frmLinearExtractionProperties(nodSurf.Item as GCDProjectItem);
            EditTreeItem(frm);
        }
    }
}
