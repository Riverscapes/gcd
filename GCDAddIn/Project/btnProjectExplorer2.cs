using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDAddIn.Project
{
    /// <summary>
    /// hooked up to the Project menu item "GCD Project Explorer" whereas btnProjectExplorer is hooked up to the toolbar button
    /// </summary>
    class btnProjectExplorer2 : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            try
            {
                btnProjectExplorer.ShowProjectExplorer(true);
            }
            catch (Exception ex)
            {
                GCDCore.GCDException.HandleException(ex);
            }

            ArcMap.Application.CurrentTool = null;
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}
