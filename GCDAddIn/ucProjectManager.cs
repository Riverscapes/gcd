using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project;

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


                GCDCore.Project.ProjectManager.GISLayerDeletingEventHandler += OnGISLayerDeleting;
                GCDCore.Project.ProjectManager.GISLayerBrowsingEventHandler += OnGISLayerBrowsing;
                GCDCore.Project.ProjectManager.GISAddToMapEventHandler += OnAddRasterToMap;

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

            public void OnGISLayerBrowsing(System.Windows.Forms.TextBox txt, naru.ui.PathEventArgs e)
            {
                System.IO.DirectoryInfo dir = null;
                if (e.Path is System.IO.FileInfo)
                    dir = e.Path.Directory;

                GCDConsoleLib.Raster result = ArcMapBrowse.BrowseOpenRaster(e.FormTitle, dir);
                if (result is GCDConsoleLib.Raster)
                    txt.Text = result.GISFileInfo.FullName;
            }

            public void OnAddRasterToMap(GCDProjectRasterItem raster)
            {
                if (raster is DEMSurvey)
                    ArcMapManager.AddDEM((DEMSurvey)raster);
                else if (raster is AssocSurface)
                    ArcMapManager.AddAssociatedSurface((AssocSurface)raster);
                else if (raster is ErrorSurface)
                    ArcMapManager.AddErrSurface((ErrorSurface)raster);
                else if (raster is DoDBase)
                    ArcMapManager.AddDoD(raster);



                ArcMap.Document.ActivatedView.Refresh();
                ArcMap.Document.UpdateContents();
            }
        }
    }
}
