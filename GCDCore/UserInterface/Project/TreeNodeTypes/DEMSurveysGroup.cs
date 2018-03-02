using GCDCore.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GCDCore.UserInterface.Project.TreeNodeTypes
{
    public class DEMSurveysGroup : TreeNodeGroup
    {
        public DEMSurveysGroup(TreeNodeCollection parentNodes, IContainer container)
            : base(parentNodes, "DEM Surveys", "DEM Survey", "DEM Surveys", ProjectManager.Project.SurveysFolder, container, ProjectManager.Project.DEMSurveys.Count > 0)
        {
            LoadChildNodes();
        }

        public override void LoadChildNodes()
        {
            Nodes.Clear();

            foreach (DEMSurvey dem in ProjectManager.Project.DEMSurveys.Values)
            {
                TreeNodeItem nodDEM = new TreeNodeTypes.TreeNodeItem(dem, 2, ContextMenuStrip.Container);
                Nodes.Add(nodDEM);

                TreeNodeGroup nodAssoc = new AssocGroup(nodDEM.Nodes, ContextMenuStrip.Container, dem);
                TreeNodeGroup nodError = new ErrorSurfaceGroup(nodDEM.Nodes, ContextMenuStrip.Container, dem);
                TreeNodeGroup nodLinea = new LinearExtractionGrp(nodDEM.Nodes, dem, dem.Raster.GISFileInfo.Directory, ContextMenuStrip.Container);
            }

            if (Nodes.Count > 0)
                Expand();
        }

        public override void OnAdd(object sender, EventArgs e)
        {
            // Determine if this is the first DEM in the project or use the first existing DEM as a reference surface
            DEMSurvey referenceDEM = null;
            SurveyLibrary.ExtentImporter.Purposes ePurpose = SurveyLibrary.ExtentImporter.Purposes.FirstDEM;
            if (ProjectManager.Project.DEMSurveys.Count > 0)
            {
                referenceDEM = ProjectManager.Project.DEMSurveys.Values.First();
                ePurpose = SurveyLibrary.ExtentImporter.Purposes.SubsequentDEM;
            }

            try
            {
                SurveyLibrary.frmImportRaster frm = new SurveyLibrary.frmImportRaster(referenceDEM, ePurpose, "DEM Survey");
                if (EditTreeItem(frm, false) == DialogResult.OK)
                {
                    GCDConsoleLib.Raster rDEM = frm.ProcessRaster();
                    DEMSurvey dem = new DEMSurvey(frm.txtName.Text, null, rDEM.GISFileInfo, Surface.HillShadeRasterPath(rDEM.GISFileInfo));
                    ProjectManager.Project.DEMSurveys[dem.Name] = dem;

                    // If this was the first raster then we need to store the cell resolution on the project
                    if (ePurpose == SurveyLibrary.ExtentImporter.Purposes.FirstDEM)
                        ProjectManager.Project.CellArea = dem.Raster.Extent.CellArea(ProjectManager.Project.Units);

                    ProjectManager.Project.Save();
                    LoadChildNodes();

                    // Loop through the child nodes and select the item that was just added
                    foreach (TreeNodeItem childNode in Nodes)
                    {
                        if (childNode.Item.Equals(dem))
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
    }
}
