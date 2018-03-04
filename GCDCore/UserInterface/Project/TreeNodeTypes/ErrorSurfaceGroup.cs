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
                tsmi = new ToolStripMenuItem("Calculate New Error Surface", Properties.Resources.sigma, OnCalculateDEMErrorSurface);
                tsmi = new ToolStripMenuItem("Calculate New Error Surface For Entire DEM Extent", Properties.Resources.sigma, OnCalculateDEMErrorSurface_SingleMethod);
            }
            else
            {
                tsmi = new ToolStripMenuItem("Calculate New Reference Error Surface", Properties.Resources.sigma, OnCalculateReferenceErrorSurface);
            }
            ContextMenuStrip.Items.Insert(1, tsmi);

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
            EditTreeItem(frm);
        }

        public void OnCalculateDEMErrorSurface(object sender, EventArgs e)
        {
            SurveyLibrary.frmErrorSurfaceProperties frm = new SurveyLibrary.frmErrorSurfaceProperties(Surface as DEMSurvey, null);
            EditTreeItem(frm);
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
    }
}
