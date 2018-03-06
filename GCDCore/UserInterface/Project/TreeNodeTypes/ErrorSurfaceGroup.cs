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
    public class ErrorSurfaceGroup : TreeNodeGroup
    {
        Surface Surface;

        public ErrorSurfaceGroup(TreeNodeCollection parentNodes, IContainer container, Surface surf)
            : base(parentNodes, "Error Surfaces", "Error Surface", "Error Surfaces", surf.ErrorSurfacesFolder, container)
        {
            Surface = surf;

            ToolStripMenuItem tsmi = null;
            if (Surface is DEMSurvey)
            {
                ContextMenuStrip.Items.Insert(1, new ToolStripMenuItem("Calculate New Error Surface For Entire DEM Extent", Properties.Resources.sigma, OnCalculateDEMErrorSurface_SingleMethod));
                ContextMenuStrip.Items.Insert(1, new ToolStripMenuItem("Calculate New Error Surface With Mask", Properties.Resources.sigma, OnCalculateDEMErrorSurface_MaskMethod));
            }
            else
            {
                ContextMenuStrip.Items.Insert(1, new ToolStripMenuItem("Calculate New Reference Error Surface", Properties.Resources.sigma, OnCalculateReferenceErrorSurface));
            }

            LoadChildNodes();
        }

        public override void LoadChildNodes()
        {
            Nodes.Clear();

            Surface.ErrorSurfaces.ToList().ForEach(x => Nodes.Add(new TreeNodeItem(x.NameWithDefault, x, 4, ContextMenuStrip.Container)));

            if (Nodes.Count > 0)
                Expand();
        }

        public override void OnAdd(object sender, EventArgs e)
        {
            string noun = "Error Surface";
            SurveyLibrary.ExtentImporter.Purposes ePurpose = SurveyLibrary.ExtentImporter.Purposes.ReferenceErrorSurface;
            if (Surface is DEMSurvey)
            {
                ePurpose = SurveyLibrary.ExtentImporter.Purposes.ErrorSurface;
                noun = "Reference Error Surface";
            }

            SurveyLibrary.frmImportRaster frm = new SurveyLibrary.frmImportRaster(Surface, ePurpose, noun);
            if (EditTreeItem(frm) == DialogResult.OK)
            {
                GCDConsoleLib.Raster raster = frm.ProcessRaster();
                ErrorSurface err = new ErrorSurface(frm.txtName.Text, raster.GISFileInfo, Surface, Surface.ErrorSurfaces.Count == 0, null);
                Surface.ErrorSurfaces.Add(err);
                ProjectManager.Project.Save();
                ProjectManager.AddNewProjectItemToMap(err);
                LoadChildNodes();

                // Loop through the child nodes and select the item that was just added
                foreach (TreeNode childNode in Nodes)
                {
                    if (childNode is TreeNodeItem)
                    {
                        if (((TreeNodeItem)childNode).Item.Equals(err))
                        {
                            TreeView.SelectedNode = childNode;
                            break;
                        }
                    }
                }
            }
        }

        public void OnCalculateReferenceErrorSurface(object sender, EventArgs e)
        {
            SurveyLibrary.ReferenceSurfaces.frmRefErrorSurface frm = new SurveyLibrary.ReferenceSurfaces.frmRefErrorSurface(Surface);
            EditTreeItem(frm);
        }

        public void OnCalculateDEMErrorSurface_SingleMethod(object sender, EventArgs e)
        {
            SurveyLibrary.ErrorSurfaces.frmSingleMethodError frm = new SurveyLibrary.ErrorSurfaces.frmSingleMethodError(Surface as DEMSurvey);
            EditTreeItem(frm);
        }

        public void OnCalculateDEMErrorSurface_MaskMethod(object sender, EventArgs e)
        {
            if (!ProjectManager.Project.Masks.Values.Any(x => x is GCDCore.Project.Masks.RegularMask))
            {
                MessageBox.Show("You must add at least one regular mask to this GCD project before you can create an error surface using a mask.", "Insufficient Regular Masks", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SurveyLibrary.ErrorSurfaces.frmMultiMethodError frm = new SurveyLibrary.ErrorSurfaces.frmMultiMethodError(Surface as DEMSurvey);
            EditTreeItem(frm);
        }
    }
}
