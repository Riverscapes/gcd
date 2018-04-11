using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project;
using ESRI.ArcGIS.DataManagementTools;

namespace GCDAddIn
{
    class ucProjectManager : GCDCore.UserInterface.Project.ucProjectExplorer
    {

        public ucProjectManager(object hook) : base()
        {
            this.Hook = hook;
        }

        /// <summary>
        /// Host object of the dockable window
        /// </summary>
        private object Hook
        {
            get;
            set;
        }

        /// <summary>
        /// Implementation class of the dockable window add-in. It is responsible for 
        /// creating and disposing the user interface class of the dockable window.
        /// </summary>
        public class AddinImpl : ESRI.ArcGIS.Desktop.AddIns.DockableWindow
        {
            private ucProjectManager m_windowUI;
            private GCDArcMapManager ArcMapManager;

            public AddinImpl()
            {
            }

            public ucProjectManager UI { get { return m_windowUI; } }

            protected override IntPtr OnCreateChild()
            {
                m_windowUI = new ucProjectManager(this.Hook);


                ProjectManager.GISLayerDeletingEventHandler += OnGISLayerDeleting;
                ProjectManager.GISLayerBrowsingEventHandler += OnGISBrowseRaster;
                ProjectManager.GISBrowseVectorEventHandler += OnGISBrowseVector;
                ProjectManager.GISSelectingVectorEventHandler += OnGISSelectingVector;
                ProjectManager.GISAddRasterToMapEventHandler += OnAddRasterToMap;
                ProjectManager.GISAddVectorToMapEventHandler += OnAddVectorToMap;

                ArcMapManager = new GCDArcMapManager();

                return m_windowUI.Handle;
            }

            protected override void Dispose(bool disposing)
            {
                if (m_windowUI != null)
                    m_windowUI.Dispose(disposing);

                base.Dispose(disposing);
            }

            public void OnGISLayerDeleting(GCDCore.Project.ProjectManager.GISLayerEventArgs e)
            {
                ArcMapUtilities.RemoveLayer(e.RasterPath);
            }

            public void OnGISBrowseRaster(System.Windows.Forms.TextBox txt, naru.ui.PathEventArgs e)
            {
                System.IO.DirectoryInfo dir = null;
                string RasterName = string.Empty;
                if (e.Path is System.IO.FileInfo)
                {
                    dir = e.Path.Directory;
                    RasterName = System.IO.Path.GetFileName(e.Path.FullName);
                }

                GCDConsoleLib.Raster result = ArcMapBrowse.BrowseOpenRaster(e.FormTitle, dir, RasterName, e.hWndParent);
                if (result is GCDConsoleLib.Raster)
                    txt.Text = result.GISFileInfo.FullName;
            }

            public void OnGISBrowseVector(System.Windows.Forms.TextBox txt, naru.ui.PathEventArgs e, GCDConsoleLib.GDalGeometryType.SimpleTypes geometryType)
            {
                System.IO.DirectoryInfo dir = null;
                string FCName = string.Empty;
                if (e.Path is System.IO.FileInfo)
                {
                    dir = e.Path.Directory;
                    FCName = System.IO.Path.GetFileNameWithoutExtension(e.Path.FullName);
                }

                ArcMapBrowse.BrowseGISTypes eType = ArcMapBrowse.BrowseGISTypes.Any;
                switch (geometryType)
                {
                    case GCDConsoleLib.GDalGeometryType.SimpleTypes.Point: eType = ArcMapBrowse.BrowseGISTypes.Point; break;
                    case GCDConsoleLib.GDalGeometryType.SimpleTypes.LineString: eType = ArcMapBrowse.BrowseGISTypes.Line; break;
                    case GCDConsoleLib.GDalGeometryType.SimpleTypes.Polygon: eType = ArcMapBrowse.BrowseGISTypes.Polygon; break;
                }

                GCDConsoleLib.Vector result = ArcMapBrowse.BrowseOpenVector(e.FormTitle, dir, FCName, eType, e.hWndParent);
                if (result is GCDConsoleLib.Vector)
                    txt.Text = result.GISFileInfo.FullName;
            }

            public void OnGISSelectingVector(System.Windows.Forms.TextBox txt, naru.ui.PathEventArgs e, GCDConsoleLib.GDalGeometryType.SimpleTypes geometryType)
            {
                try
                {
                    ArcMapBrowse.BrowseGISTypes eType = ArcMapBrowse.BrowseGISTypes.Polygon;
                    switch (geometryType)
                    {
                        case GCDConsoleLib.GDalGeometryType.SimpleTypes.Point:
                            eType = ArcMapBrowse.BrowseGISTypes.Point;
                            break;

                        case GCDConsoleLib.GDalGeometryType.SimpleTypes.LineString:
                            eType = ArcMapBrowse.BrowseGISTypes.Line;
                            break;

                        case GCDConsoleLib.GDalGeometryType.SimpleTypes.Polygon:
                            eType = ArcMapBrowse.BrowseGISTypes.Polygon;
                            break;
                    }

                    frmLayerSelector frm = new frmLayerSelector(eType);
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        txt.Text = frm.SelectedLayer.FullPath.FullName;
                    }
                }
                catch (Exception ex)
                {
                    GCDCore.GCDException.HandleException(ex);
                }
            }

            public void OnAddRasterToMap(GCDProjectRasterItem raster)
            {
                if (raster is DEMSurvey)
                    ArcMapManager.AddDEM((DEMSurvey)raster);
                else if (raster is AssocSurface)
                    ArcMapManager.AddAssociatedSurface((AssocSurface)raster);
                else if (raster is ErrorSurface)
                    ArcMapManager.AddErrSurface((ErrorSurface)raster);
                else if (raster is Surface)
                    ArcMapManager.AddReferenceSurface(raster as Surface);
                else if (raster is GCDProjectRasterItem)
                    ArcMapManager.AddDoD(raster);

                ArcMap.Document.ActivatedView.Refresh();
                ArcMap.Document.UpdateContents();
            }

            public void OnAddVectorToMap(GCDCore.Project.GCDProjectVectorItem vector)
            {
                if (vector is GCDCore.Project.Masks.AttributeFieldMask)
                    ArcMapManager.AddMask(vector as GCDCore.Project.Masks.AttributeFieldMask);
                else if (vector is GCDCore.Project.Masks.AOIMask)
                    ArcMapManager.AddAOI(vector as GCDCore.Project.Masks.AOIMask);
                else if (vector is GCDCore.Project.ProfileRoutes.ProfileRoute)
                    ArcMapManager.AddProfileRoute(vector as GCDCore.Project.ProfileRoutes.ProfileRoute);
            }
        }
    }
}
