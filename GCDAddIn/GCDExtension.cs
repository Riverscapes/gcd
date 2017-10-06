using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ADF.CATIDs;

namespace GCDAddIn
{
    [Guid("9d0ca38b-bffd-4a79-8c76-4fa079138c5b")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("GCDAddIn.GCDExtension")]
    public class GCDExtension : IExtension
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxExtension.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxExtension.Unregister(regKey);

        }

        #endregion
        #endregion
        private IApplication m_application;

        #region IExtension Members

        /// <summary>
        /// Name of extension. Do not exceed 31 characters
        /// </summary>
        public string Name
        {
            get
            {
                //TODO: Modify string to uniquely identify extension
                return "GCDExtension";
            }
        }

        public void Shutdown()
        {
            m_application = null;

            // Destroy  GDAL. This is now done once at the close of the extension.
            try
            {
                GCD.GCDLib.Core.External.RasterManager.DestroyGDAL();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("WARNING: Failed to destroy GDAL.");
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public void Startup(ref object initializationData)
        {
            m_application = initializationData as IApplication;
            if (m_application == null)
                return;

            try
            {
                GCD.GCDLib.Core.External.RasterManager.RegisterGDAL();
            }
            catch (Exception ex)
            {
                naru.error.ExceptionUI.HandleException(ex);
            }
        }

        #endregion
    }
}