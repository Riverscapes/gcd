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
        private enum SortOrders
        {
            AlphaAsc,
            AlphaDsc,
            ChronAsc,
            ChronDsc
        };

        private SortOrders SortOrder;

        public DEMSurveysGroup(TreeNodeCollection parentNodes, IContainer container)
            : base(parentNodes, "DEM Surveys", "DEM Survey", "DEM Surveys", ProjectManager.Project.SurveysFolder, container, ProjectManager.Project.DEMSurveys.Count > 0)
        {
            ToolStripMenuItem tsmiSort = new ToolStripMenuItem("Sort DEM Surveys");
            tsmiSort.DropDownItems.Add(new ToolStripMenuItem("Sort Alphabetical Ascending", Properties.Resources.alphabetical, OnSort));
            tsmiSort.DropDownItems.Add(new ToolStripMenuItem("Sort Alphabetical Descending", Properties.Resources.alpha_descending, OnSort));
            tsmiSort.DropDownItems.Add(new ToolStripMenuItem("Sort Chronological Ascending", Properties.Resources.chrono_ascending, OnSort));
            tsmiSort.DropDownItems.Add(new ToolStripMenuItem("Sort Chronological Descending", Properties.Resources.chrono_descending, OnSort));
            ContextMenuStrip.Items.Insert(ContextMenuStrip.Items.Count - 2, tsmiSort);

            // Default is ascending alphabetical
            SortOrder = SortOrders.AlphaAsc;

            LoadChildNodes();
        }

        public override void LoadChildNodes()
        {
            Nodes.Clear();

            List<DEMSurvey> dems = ProjectManager.Project.DEMSurveys.Values.ToList();

            switch (SortOrder)
            {
                case SortOrders.AlphaAsc:
                    dems.Sort((a, b) => a.CompareTo(b as GCDProjectItem));
                    break;

                case SortOrders.AlphaDsc:
                    dems.Sort((a, b) => -1 * a.CompareTo(b as GCDProjectItem));
                    break;

                case SortOrders.ChronAsc:
                    dems.Sort((a, b) => a.CompareTo(b));
                    break;

                case SortOrders.ChronDsc:
                    dems.Sort((a, b) => -1 * a.CompareTo(b));
                    break;
            }

            foreach (DEMSurvey dem in dems)
            {
                TreeNodeItem nodDEM = new TreeNodeTypes.TreeNodeItem(dem, 2, ContextMenuStrip.Container);
                Nodes.Add(nodDEM);
                nodDEM.ContextMenuStrip.Items.Insert(1, new ToolStripMenuItem("Calculate Linear Extraction From Profile Route", Properties.Resources.Add, OnLinear));

                TreeNodeGroup nodAssoc = new AssocGroup(nodDEM.Nodes, ContextMenuStrip.Container, dem);
                TreeNodeGroup nodError = new ErrorSurfaceGroup(nodDEM.Nodes, ContextMenuStrip.Container, dem);

                if (dem.LinearExtractions.Count > 0)
                {
                    TreeNodeGroup nodLinea = new LinearExtractionGrp(nodDEM.Nodes, dem, dem.LinearExtractions.Values.First().Database.Directory.Parent, ContextMenuStrip.Container);
                }
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
                    ProjectManager.AddNewProjectItemToMap(dem);
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

        private void OnSort(object sender, EventArgs e)
        {
            ToolStripDropDownItem ctrl = sender as ToolStripDropDownItem;
            if (ctrl.Text.ToLower().Contains("alpha"))
            {
                SortOrder = ctrl.Text.ToLower().Contains("asc") ? SortOrders.AlphaAsc : SortOrders.AlphaDsc;
            }
            else
            {
                SortOrder = ctrl.Text.ToLower().Contains("asc") ? SortOrders.ChronAsc : SortOrders.ChronDsc;
            }

            LoadChildNodes();
        }

        private void OnLinear(object sender, EventArgs e)
        {
            ToolStripDropDownItem ctrl = sender as ToolStripDropDownItem;
            ContextMenuStrip cms = ctrl.Owner as ContextMenuStrip;
            TreeView tre = cms.SourceControl as TreeView;
            TreeNodeItem nodDEM = tre.SelectedNode as TreeNodeItem; 

            LinearExtraction.frmLinearExtractionProperties frm = new LinearExtraction.frmLinearExtractionProperties(nodDEM.Item as GCDProjectItem);
            EditTreeItem(frm);
        }
    }
}
