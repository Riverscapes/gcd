using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Xml;

namespace GCDCore.Project
{
    public class GCDProject : GCDProjectItem
    {
        public string Description { get; set; }
        public readonly FileInfo ProjectFile;
        public readonly DateTime DateTimeCreated;
        public readonly string GCDVersion;
        public readonly UnitsNet.Area CellArea;
        public GCDConsoleLib.GCD.UnitGroup Units { get; set; }

        public readonly Dictionary<string, DEMSurvey> DEMSurveys;
        public readonly Dictionary<string, DoDBase> DoDs;
        public readonly Dictionary<string, string> MetaData;

        public GCDProject(string name, string description, FileInfo projectFile,
            DateTime dtCreated, string gcdVersion, UnitsNet.Area cellArea, GCDConsoleLib.GCD.UnitGroup units)
            : base(name)
        {
            Description = description;
            ProjectFile = projectFile;
            DateTimeCreated = dtCreated;
            GCDVersion = gcdVersion;
            Units = units;
            CellArea = cellArea;

            DEMSurveys = new Dictionary<string, DEMSurvey>();
            DoDs = new Dictionary<string, DoDBase>();
            MetaData = new Dictionary<string, string>();
        }

        public string GetRelativePath(FileInfo FullPath)
        {
            return GetRelativePath(FullPath.FullName);
        }

        public string GetRelativePath(string FullPath)
        {
            int nIndex = FullPath.ToLower().IndexOf(ProjectFile.DirectoryName.ToLower());
            if (nIndex < 0)
            {
                Exception ex = new Exception("Unable to determine relative path.");
                ex.Data["Project Directory"] = ProjectFile.DirectoryName;
                ex.Data["File Path"] = FullPath;
                throw ex;
            }

            string relativePath = FullPath.Substring(ProjectFile.FullName.Length, FullPath.Length - ProjectFile.DirectoryName.Length);
            relativePath = relativePath.TrimStart(Path.DirectorySeparatorChar);
            return relativePath;
        }

        public IEnumerable<DEMSurvey> DEMsSortByName(bool bAscending)
        {
            List<DEMSurvey> dems = DEMSurveys.Values.ToList<DEMSurvey>().OrderBy(x => x.Name).ToList<DEMSurvey>();

            if (!bAscending)
                dems.Reverse();

            return dems;
        }

        public IEnumerable<DEMSurvey> DEMsSortByDate(bool bAscending)
        {
            List<DEMSurvey> dems = DEMSurveys.Values.ToList<DEMSurvey>().OrderBy(x => x.SurveyDate).ToList<DEMSurvey>();

            if (!bAscending)
                dems.Reverse();

            return dems;
        }

        public void DeleteDEMSurvey(DEMSurvey dem)
        {
            throw new NotImplementedException("delete DEM not implemented");
        }

        public void DeleteDoD(DoDBase dod)
        {
            //    Private Sub DeleteDoD(ByVal rDoD As ProjectDS.DoDsRow)

            //    'TODO entire contents commented out
            //    Throw New Exception("not implemented")

            //    'If Not TypeOf rDoD Is ProjectDS.DoDsRow Then
            //    '    Exit Sub
            //    'End If


            //    'Dim response As MsgBoxResult = MsgBox("Are you sure you want to remove the selected change detection?  Note: this will remove the change detection from the GCD Project (*.gcd) file and also delete the copy of the raster in the GCD project folder.", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, GCDCore.Properties.Resources.ApplicationNameLong)
            //    'If response = MsgBoxResult.Yes Then

            //    '    Dim pMXDoc As ESRI.ArcGIS.ArcMapUI.IMxDocument = DirectCast(My.ThisApplication, ESRI.ArcGIS.Framework.IApplication).Document
            //    '    Dim pMap As ESRI.ArcGIS.Carto.IMap = pMXDoc.FocusMap
            //    '    Dim projectName As String = rDoD.Name

            //    '    ' Both raw and thresholded DoD rasters can be in the map. Make a list and remove both.
            //    '    Dim lDoDSource As New List(Of String)
            //    '    lDoDSource.Add(GCDProject.ProjectManagerBase.GetAbsolutePath(rDoD.RawDoDPath))
            //    '    lDoDSource.Add(GCDProject.ProjectManagerBase.GetAbsolutePath(rDoD.ThreshDoDPath))

            //    '    For Each sDoDSource As String In lDoDSource
            //    '        'New code to remove project layer from map if in map
            //    '        Dim enumLayer As ESRI.ArcGIS.Carto.IEnumLayer = pMap.Layers
            //    '        Dim pLayer As ESRI.ArcGIS.Carto.ILayer = enumLayer.Next()

            //    '        While pLayer IsNot Nothing
            //    '            If TypeOf pLayer Is ESRI.ArcGIS.Carto.RasterLayer Then
            //    '                If String.Compare(sDoDSource, DirectCast(pLayer, ESRI.ArcGIS.Carto.RasterLayer).FilePath, True) = 0 Then
            //    '                    pMap.DeleteLayer(pLayer)
            //    '                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pLayer)
            //    '                    pLayer = Nothing
            //    '                    Exit While
            //    '                End If
            //    '            End If
            //    '            pLayer = enumLayer.Next()
            //    '        End While
            //    '    Next

            //    '    Try
            //    '        'DeleteSurveyFiles(IO.Path.GetDirectoryName(CurrentRow.Item("Source")))
            //    '        'IO.Directory.Delete(IO.Path.GetDirectoryName(CurrentRow.Item("Source")), True)
            //    '        DeleteSurveyFiles(IO.Path.GetDirectoryName(lDoDSource(0)))
            //    '        IO.Directory.Delete(IO.Path.GetDirectoryName(lDoDSource(0)), True)
            //    '    Catch ex As Exception
            //    '        naru.error.ExceptionUI.HandleException(ex, "The GCD survey files failed to delete. Some of the files associated with this survey still exist.")
            //    '        'Dim ex2 As New Exception("The GCD survey files failed to delete. Some of the files associated with this survey still exist.", ex)
            //    '        'ex2.Data.Add("Project Name: ", projectName)
            //    '        'Throw ex2
            //    '    Finally
            //    '        rDoD.Delete()
            //    '        GCDProject.ProjectManagerBase.save()
            //    '    End Try

            //    'End If
            //End Sub
        }

        public FileInfo GetAbsolutePath(string sRelativePath)
        {
            return new FileInfo(Path.Combine(ProjectFile.DirectoryName, sRelativePath));
        }

        public DirectoryInfo GetAbsoluteDir(string sRelativeDir)
        {
            return new DirectoryInfo(Path.Combine(ProjectFile.DirectoryName, sRelativeDir));
        }

        public bool IsDEMNameUnique(string name, DEMSurvey ignore)
        {
            bool bUnique = true;
            if (DEMSurveys.ContainsKey(name))
            {
                bUnique = DEMSurveys[name] != ignore;
            }
            return bUnique;
        }

        public bool IsDoDNameUnique(string name, DoDBase ignore)
        {
            bool bUnique = true;
            if (DoDs.ContainsKey(name))
            {
                bUnique = DoDs[name] != ignore;
            }
            return bUnique;
        }

        public void Save()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode nodProject = xmlDoc.CreateElement("Project");
            xmlDoc.AppendChild(nodProject);

            // Create an XML declaration. 
            XmlDeclaration xmldecl;
            xmldecl = xmlDoc.CreateXmlDeclaration("1.0", null, null);
            xmlDoc.InsertBefore(xmldecl, xmlDoc.DocumentElement);

            nodProject.AppendChild(xmlDoc.CreateElement("Name")).InnerText = Name;
            nodProject.AppendChild(xmlDoc.CreateElement("DateTimeCreated")).InnerText = DateTimeCreated.ToString("o");
            nodProject.AppendChild(xmlDoc.CreateElement("GCDVersion")).InnerText = GCDVersion;

            XmlNode nodDescription = nodProject.AppendChild(xmlDoc.CreateElement("Description"));
            if (!string.IsNullOrEmpty(Description))
                nodDescription.InnerText = Description;

            XmlNode nodUnits = nodProject.AppendChild(xmlDoc.CreateElement("Units"));
            nodUnits.AppendChild(xmlDoc.CreateElement("Horizontal")).InnerText = Units.HorizUnit.ToString();
            nodUnits.AppendChild(xmlDoc.CreateElement("Vertical")).InnerText = Units.VertUnit.ToString();
            nodUnits.AppendChild(xmlDoc.CreateElement("Area")).InnerText = Units.ArUnit.ToString();
            nodUnits.AppendChild(xmlDoc.CreateElement("Volume")).InnerText = Units.VolUnit.ToString();

            XmlNode nodArea = nodProject.AppendChild(xmlDoc.CreateElement("CellArea"));
            if (CellArea.As(Units.ArUnit) > 0)
                nodArea.InnerText = CellArea.As(Units.ArUnit).ToString("R");

            if (DEMSurveys.Count > 0)
            {
                XmlNode nodDEMs = nodProject.AppendChild(xmlDoc.CreateElement("DEMSurveyrs"));
                foreach (DEMSurvey dem in DEMSurveys.Values)
                    dem.Serialize(xmlDoc, nodDEMs);
            }

            if (DoDs.Count > 0)
            {
                XmlNode nodDoDs = nodProject.AppendChild(xmlDoc.CreateElement("DoDs"));
                foreach (DoDBase dod in DoDs.Values)
                    dod.Serialize(xmlDoc, nodDoDs);
            }

            if (MetaData.Count > 0)
            {
                XmlNode nodMetaData = nodProject.AppendChild(xmlDoc.CreateElement("MetaData"));
                foreach (KeyValuePair<string, string> item in MetaData)
                {
                    XmlNode nodItem = nodMetaData.AppendChild(xmlDoc.CreateElement("Item"));
                    nodItem.AppendChild(xmlDoc.CreateElement("Key")).InnerText = item.Key;
                    nodItem.AppendChild(xmlDoc.CreateElement("Value")).InnerText = item.Value;
                }
            }

            xmlDoc.Save(ProjectFile.FullName);
        }

        public static GCDProject Load(FileInfo projectFile)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(projectFile.FullName);

            XmlNode nodProject = xmlDoc.SelectSingleNode("Project");
            string name = nodProject.SelectSingleNode("Name").InnerText;
            string desc = nodProject.SelectSingleNode("Description").InnerText;
            string gcdv = nodProject.SelectSingleNode("GCDVersion").InnerText;
            DateTime dtCreated = DateTime.Parse(nodProject.SelectSingleNode("DateTimeCreated").InnerText);

            UnitsNet.Units.LengthUnit horiz = (UnitsNet.Units.LengthUnit)Enum.Parse(typeof(UnitsNet.Units.LengthUnit), nodProject.SelectSingleNode("Units/Horizontal").InnerText);
            UnitsNet.Units.LengthUnit vert = (UnitsNet.Units.LengthUnit)Enum.Parse(typeof(UnitsNet.Units.LengthUnit), nodProject.SelectSingleNode("Units/Vertical").InnerText);
            UnitsNet.Units.AreaUnit area = (UnitsNet.Units.AreaUnit)Enum.Parse(typeof(UnitsNet.Units.AreaUnit), nodProject.SelectSingleNode("Units/Area").InnerText);
            UnitsNet.Units.VolumeUnit vol = (UnitsNet.Units.VolumeUnit)Enum.Parse(typeof(UnitsNet.Units.VolumeUnit), nodProject.SelectSingleNode("Units/Volume").InnerText);
            GCDConsoleLib.GCD.UnitGroup units = new GCDConsoleLib.GCD.UnitGroup(vol, area, vert, horiz);

            UnitsNet.Area cellArea = UnitsNet.Area.From(0, area);
            XmlNode nodCellArea = nodProject.SelectSingleNode("CellArea");
            if (!string.IsNullOrEmpty(nodCellArea.InnerText))
                cellArea = UnitsNet.Area.From(double.Parse(nodCellArea.InnerText), area);

            GCDProject project = new GCDProject(name, desc, projectFile, dtCreated, gcdv, cellArea, units);

            foreach (XmlNode nodDEM in nodProject.SelectNodes("DEMSurveys/DEM"))
            {
                DEMSurvey dem = DEMSurvey.Deserialize(nodDEM);
                project.DEMSurveys[dem.Name] = dem;
            }

            foreach (XmlNode nodDoD in nodProject.SelectNodes("DoDs/DoD"))
            {
                DoDBase dod = DoDBase.Deserialize(nodDoD, project.DEMSurveys);
                project.DoDs[dod.Name] = dod;
            }

            foreach (XmlNode nodItem in nodProject.SelectNodes("MetaData/Item"))
            {
                project.MetaData[nodItem.SelectSingleNode("Key").InnerText] = nodItem.SelectSingleNode("Value").InnerText;
            }

            return project;
        }
    }
}
