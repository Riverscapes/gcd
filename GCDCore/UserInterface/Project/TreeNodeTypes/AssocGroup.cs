using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project;
using System.Windows.Forms;
using System.ComponentModel;

namespace GCDCore.UserInterface.Project.TreeNodeTypes
{
    public class AssocGroup : TreeNodeGroup
    {
        public readonly DEMSurvey DEM;

        public AssocGroup(TreeNodeCollection parentNodes, IContainer container, DEMSurvey dem)
            : base(parentNodes, "Associated Surfaces", "Associated Surface", "Associated Surfaces", dem.AssocSurfacesFolder, container)
        {
            DEM = dem;

            // Associated surface uses the same form for browsing to existing as well as calculating new
            // ContextMenuStrip.Items[0].Text = "Add Associated Surface";

            ContextMenuStrip.Items.Insert(1, new ToolStripMenuItem("Calculate New Percent Slope Associated Surface", Properties.Resources.sigma, OnCalculateSlopePercent));
            ContextMenuStrip.Items.Insert(1, new ToolStripMenuItem("Calculate New Slope Degrees Associated Surface", Properties.Resources.sigma, OnCalculateSlopeDegrees));
            ContextMenuStrip.Items.Insert(1, new ToolStripMenuItem("Calculate New Point Density Associated Surface", Properties.Resources.sigma, OnCalculatePointDensity));

            LoadChildNodes();
        }

        public override void LoadChildNodes()
        {
            Nodes.Clear();
            DEM.AssocSurfaces.ToList().ForEach(x => Nodes.Add(new TreeNodeItem(x, 3, ContextMenuStrip.Container)));

            if (Nodes.Count > 0)
                Expand();
        }

        public override void OnAdd(object sender, EventArgs e)
        {
            SurveyLibrary.frmImportRaster frm = new SurveyLibrary.frmImportRaster(DEM, SurveyLibrary.ExtentImporter.Purposes.AssociatedSurface, "Associated Surface");
            if (EditTreeItem(frm) == DialogResult.OK)
            {
                GCDConsoleLib.Raster rAssoc = frm.ProcessRaster();
                AssocSurface assoc = new AssocSurface(frm.txtName.Text, rAssoc.GISFileInfo, DEM, AssocSurface.AssociatedSurfaceTypes.Other);
                DEM.AssocSurfaces.Add(assoc);
                ProjectManager.Project.Save();
                LoadChildNodes();

                // Loop through the child nodes and select the item that was just added
                foreach (TreeNode childNode in Nodes)
                {
                    if (childNode is TreeNodeItem)
                    {
                        if (((TreeNodeItem)childNode).Item.Equals(assoc))
                        {
                            TreeView.SelectedNode = childNode;
                            break;
                        }
                    }
                }
            }
        }

        private void OnCalculateSlopePercent(object sender, EventArgs e)
        {
            SurveyLibrary.frmAssociatedSurface frm = new SurveyLibrary.frmAssociatedSurface(DEM, AssocSurface.AssociatedSurfaceTypes.SlopePercent);
            EditTreeItem(frm);
        }

        private void OnCalculateSlopeDegrees(object sender, EventArgs e)
        {
            SurveyLibrary.frmAssociatedSurface frm = new SurveyLibrary.frmAssociatedSurface(DEM, AssocSurface.AssociatedSurfaceTypes.SlopeDegree);
            EditTreeItem(frm);
        }

        private void OnCalculatePointDensity(object sender, EventArgs e)
        {
            SurveyLibrary.frmPointDensity frm = new SurveyLibrary.frmPointDensity(DEM);
            EditTreeItem(frm);
        }
    }
}
