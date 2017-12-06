using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            public AddinImpl()
            {
            }

            public ucProjectManager UI { get { return m_windowUI; } }

            protected override IntPtr OnCreateChild()
            {
                m_windowUI = new ucProjectManager(this.Hook);

                GCDCore.Project.ProjectManager.GISLayerDeletingEventHandler += OnGISLayerDeleting;
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
                System.Windows.Forms.MessageBox.Show(e.RasterPath.FullName, "ArcGIS Notified of Layer Deletion", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Stop);
            }
        }
    }
}
