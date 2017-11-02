using System;
using System.IO;

namespace GCDCore
{
    public class WorkspaceManager
    {
        private static DirectoryInfo m_diWorkspacePath;
        public static string WorkspacePath
        {
            get
            {
                if (m_diWorkspacePath is DirectoryInfo)
                {
                    return m_diWorkspacePath.FullName;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string GetTempRaster(string sRootName)
        {
            return naru.os.File.GetNewSafeName(m_diWorkspacePath.FullName, sRootName, "tif").FullName;
        }

        public static string GetTempShapeFile(string sRootName)
        {
            if (string.IsNullOrEmpty(sRootName))
            {
                throw new ArgumentNullException("sRootName");
            }

            return naru.os.File.GetNewSafeName(m_diWorkspacePath.FullName, Path.ChangeExtension(sRootName, ""), "shp").FullName;
        }

        public static void Initialize()
        {
            string sPath = string.Empty;
            if (string.IsNullOrEmpty(Properties.Settings.Default.TempWorkspace) || !Directory.Exists(Properties.Settings.Default.TempWorkspace))
            {
                sPath = GetDefaultWorkspace(Properties.Resources.ApplicationNameShort);
            }
            else
            {
                // During AddIn startup, must set the workspace path on the workspace manager
                // object. This bypasses validation. The workspace path will be validated
                // (with UI warnings) during new/open project. For now, just set the workspace.
                sPath = Properties.Settings.Default.TempWorkspace;
            }

            SetWorkspacePath(sPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks></remarks>
        public static void ClearWorkspace()
        {
            //' Only proceed if the workspace exists.
            //If Not TypeOf m_diWorkspacePath Is System.DirectoryInfo OrElse Not m_diWorkspacePath.Exists Then
            //    Exit Sub
            //End If

            //'use datasetname to avoid fail on dataset enumeration
            //'TODO: Develop method to delete .mxd files

            //Dim datasetnames As New ArrayList()

            //Dim pWkSp As IWorkspace
            //'Dim pWkSpFactory As IWorkspaceFactory

            //Dim pEDName As IEnumDatasetName
            //Dim pDSName As IDatasetName
            //Dim pName As ESRI.ArcGIS.esriSystem.IName
            //Dim pDataset As IDataset

            //'get shapefile dataserts in folder
            //Dim pWkSpFactory As IWorkspaceFactory = GISCode.ArcMap.GetWorkspaceFactory(GISDataStructures.GISDataStorageTypes.ShapeFile)
            //pWkSp = pWkSpFactory.OpenFromFile(WorkspacePath, 0)
            //pEDName = pWkSp.DatasetNames(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureClass)
            //pDSName = pEDName.Next
            //Do Until pDSName Is Nothing
            //    datasetnames.Add(pDSName)
            //    pDSName = pEDName.Next
            //Loop
            //Runtime.InteropServices.Marshal.ReleaseComObject(pEDName)

            //' PGB - 2 June 2016 - TINs!
            //'get shapefile dataserts in folder
            //Dim pWkSpFactoryTIN As IWorkspaceFactory = GISCode.ArcMap.GetWorkspaceFactory(GISDataStructures.GISDataStorageTypes.TIN)
            //pWkSp = pWkSpFactoryTIN.OpenFromFile(WorkspacePath, 0)
            //pEDName = pWkSp.DatasetNames(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTTin)
            //pDSName = pEDName.Next
            //Do Until pDSName Is Nothing
            //    datasetnames.Add(pDSName)
            //    pDSName = pEDName.Next
            //Loop
            //Runtime.InteropServices.Marshal.ReleaseComObject(pEDName)

            //'Get raster datasets in folder
            //pWkSpFactory = GISCode.ArcMap.GetWorkspaceFactory(GISDataStructures.GISDataStorageTypes.RasterFile)
            //pWkSp = pWkSpFactory.OpenFromFile(WorkspacePath, 0)
            //pEDName = pWkSp.DatasetNames(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTRasterDataset)
            //pDSName = pEDName.Next
            //Do Until pDSName Is Nothing
            //    datasetnames.Add(pDSName)
            //    pDSName = pEDName.Next
            //Loop

            //'delete datasets
            //If datasetnames.Count > 0 Then
            //    For Each datasetname As IDatasetName In datasetnames
            //        If TypeOf datasetname Is ESRI.ArcGIS.esriSystem.IName Then
            //            pName = datasetname
            //            Try
            //                pDataset = pName.Open
            //                pDataset.Delete()
            //            Catch ex As Exception
            //                Debug.WriteLine("Could not delete dataset in temporary workspace: " & datasetname.Name)
            //            End Try
            //        End If
            //    Next
            //End If
            //'
            //' PGB 14 Mar 2012
            //' Starting to use more and more file geodatabases in the temporary workspace.
            //' delete these too.
            //'
            //Dim GP As New ESRI.ArcGIS.Geoprocessor.Geoprocessor
            //GP.SetEnvironmentValue("workspace", WorkspacePath)
            //Dim gplWorkspaces As IGpEnumList = GP.ListWorkspaces("", "")
            //Dim sWorkspace As String = gplWorkspaces.Next()

            //Dim DeleteTool As New ESRI.ArcGIS.DataManagementTools.Delete

            //Do While Not String.IsNullOrEmpty(sWorkspace)
            //    Try
            //        DeleteTool.in_data = sWorkspace
            //        GP.Execute(DeleteTool, Nothing)
            //    Catch ex As Exception
            //        Debug.WriteLine("Could not delete workspace" & sWorkspace)
            //    End Try
            //    sWorkspace = gplWorkspaces.Next()
            //Loop
        }

        /// <summary>
        /// Get the default workspace
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetDefaultWorkspace(string sApplicationShort)
        {

            if ((string.IsNullOrEmpty(sApplicationShort)))
            {
                throw new ArgumentNullException("sApplicationShort", "Invalid default workspace parameter");
            }

            string sDefault = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            sDefault = Path.Combine(sDefault, sApplicationShort);
            if (!Directory.Exists(sDefault))
            {
                Directory.CreateDirectory(sDefault);
            }

            sDefault = Path.Combine(sDefault, "TempWorkspace");
            if (!Directory.Exists(sDefault))
            {
                Directory.CreateDirectory(sDefault);
            }

            return sDefault;
        }

        public static bool ValidateWorkspace(string sWorkspacePath)
        {
            // This string will remain empty unless there is a problem, in which
            // case it will contain the relevant message to show the user.
            string sWarningMessage = string.Empty;
            string sFixMessage = string.Format("Open the {0} Options to assign a valid temporary workspace path.", Properties.Resources.ApplicationNameShort);

            if (string.IsNullOrEmpty(sWorkspacePath))
            {
                sWarningMessage = string.Format("The {0} temporary workspace path cannot be empty.{1}", Properties.Resources.ApplicationNameShort, sFixMessage);
            }
            else
            {
                if (System.IO.Directory.Exists(sWorkspacePath))
                {
                    if (Properties.Settings.Default.StartUpWorkspaceWarning)
                    {
                        if (System.Text.RegularExpressions.Regex.IsMatch(sWorkspacePath, "[ .]"))
                        {
                            // Show the message box that asks the user whether they want to proceed.
                            // This message box also controls whether they are warned again.
                            string sWarning = string.Format("The proposed temporary workspace path ({0}) contains characters that are not recommended." +
                                " This is typically because your user name contains spaces or periods." +
                                " It is strongly recommended that you choose an alternative path that is near the root of your C drive and that does not contain spaces or periods." +
                                " Do you want to proceed and use the selected path anyway?", sWorkspacePath);

                            naru.ui.frmMsgBoxWithCheckbox frm = new naru.ui.frmMsgBoxWithCheckbox(sWorkspacePath);
                            System.Windows.Forms.DialogResult eResult = frm.ShowDialog();

                            if (eResult == System.Windows.Forms.DialogResult.OK)
                            {
                                Properties.Settings.Default.StartUpWorkspaceWarning = frm.chkReminder.Checked;
                                Properties.Settings.Default.Save();
                            }
                            return eResult == System.Windows.Forms.DialogResult.OK;
                        }
                    }
                }
                else
                {
                    sWarningMessage = string.Format("The {0} temporary workspace path does not exist.{1}", Properties.Resources.ApplicationNameLong, sFixMessage);
                }
            }

            if (!string.IsNullOrEmpty(sWarningMessage))
            {
                System.Windows.Forms.MessageBox.Show(sWarningMessage, string.Format("Invalid {0} Temporary Workspace", Properties.Resources.ApplicationNameShort), System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }

            return string.IsNullOrEmpty(sWarningMessage);
        }

        /// <summary>
        /// Set the temp workspace to a directory
        /// </summary>
        /// <param name="sPath"></param>
        /// <returns></returns>
        /// <remarks>PGB Apr 2015 - Throw exception rather than debug assert when the temp workspace does not exist.
        /// Discovered on Steve Fortneys laptop.</remarks>
        public static string SetWorkspacePath(string sPath)
        {
            if (string.IsNullOrEmpty(sPath) || !Directory.Exists(sPath))
            {
                Exception ex = new Exception("The specified temp workspace directory is null or does not exist. Go to GCD Options and set the temp workspace to a valid folder.");
                ex.Data["Path"] = sPath;
                throw ex;
            }

            m_diWorkspacePath = new DirectoryInfo(sPath);
            Properties.Settings.Default.TempWorkspace = sPath;
            Properties.Settings.Default.Save();

            return m_diWorkspacePath.FullName;
        }

        public static string SetWorkspacePathDefault()
        {
            string sPath = GetDefaultWorkspace(Properties.Resources.ApplicationNameShort);
            return SetWorkspacePath(sPath);
        }
    }
}