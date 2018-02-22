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
        public UnitsNet.Area CellArea { get; set; }
        public GCDConsoleLib.GCD.UnitGroup Units { get; set; }

        public readonly Dictionary<string, DEMSurvey> DEMSurveys;
        public readonly Dictionary<string, Surface> ReferenceSurfaces;
        public readonly Dictionary<string, DoDBase> DoDs;
        public readonly Dictionary<string, string> MetaData;
        public readonly Dictionary<string, InterComparison> InterComparisons;
        public readonly Dictionary<string, Masks.Mask> Masks;
        public readonly Dictionary<string, ProfileRoutes.ProfileRoute> ProfileRoutes;

        public override string Noun { get { return "GCD Project"; } }

        /// <summary>
        /// Projects are never considered in use
        /// </summary>
        public override bool IsItemInUse
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Get the spatial reference of the first GIS dataset in the project.
        /// </summary>
        public GCDConsoleLib.Projection ReferenceProjection
        {
            get
            {
                if (DEMSurveys.Count > 0)
                    return DEMSurveys.Values.First().Raster.Proj;

                if (ReferenceSurfaces.Values.Count > 0)
                    return ReferenceSurfaces.Values.First().Raster.Proj;

                if (Masks.Count > 0)
                    return new GCDConsoleLib.Vector(Masks.Values.First()._ShapeFile).Proj;

                return null;
            }
        }

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
            ReferenceSurfaces = new Dictionary<string, Surface>();
            Masks = new Dictionary<string, Project.Masks.Mask>();
            DoDs = new Dictionary<string, DoDBase>();
            InterComparisons = new Dictionary<string, InterComparison>();
            MetaData = new Dictionary<string, string>();
            ProfileRoutes = new Dictionary<string, Project.ProfileRoutes.ProfileRoute>();
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
                return FullPath;
            }
            else
            {
                string relativePath = FullPath.Substring(ProjectFile.DirectoryName.Length, FullPath.Length - ProjectFile.DirectoryName.Length);
                relativePath = relativePath.TrimStart(Path.DirectorySeparatorChar);
                return relativePath;
            }
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

        public FileInfo GetAbsolutePath(string sRelativePath)
        {
            if (sRelativePath.Contains(":") || sRelativePath.StartsWith("\\\\"))
                return new FileInfo(sRelativePath);
            else
                return new FileInfo(Path.Combine(ProjectFile.DirectoryName, sRelativePath));
        }

        public DirectoryInfo GetAbsoluteDir(string sRelativeDir)
        {
            return new DirectoryInfo(Path.Combine(ProjectFile.DirectoryName, sRelativeDir));
        }

        public bool IsDEMNameUnique(string name, DEMSurvey ignore)
        {
            return DEMSurveys.ContainsKey(name) ? DEMSurveys[name] == ignore : true;
        }

        public bool IsReferenceSurfaceNameUnique(string name, Surface ignore)
        {
            return ReferenceSurfaces.ContainsKey(name) ? ReferenceSurfaces[name] == ignore : true;
        }

        public bool IsDoDNameUnique(string name, DoDBase ignore)
        {
            return DoDs.ContainsKey(name) ? DoDs[name] == ignore : true;
        }

        public bool IsMaskNameUnique(string name, Masks.Mask ignore)
        {
            return Masks.ContainsKey(name) ? Masks[name] == ignore : true;
        }

        public bool IsProfileRouteNameUnique(string name, GCDCore.Project.ProfileRoutes.ProfileRoute ignore)
        {
            return ProfileRoutes.ContainsKey(name) ? ProfileRoutes[name] == ignore : true;
        }

        public bool IsInterComparisonNameUnique(string name, InterComparison ignore)
        {
            return InterComparisons.ContainsKey(name) ? InterComparisons[name] == ignore : true;
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
                XmlNode nodDEMs = nodProject.AppendChild(xmlDoc.CreateElement("DEMSurveys"));
                foreach (DEMSurvey dem in DEMSurveys.Values)
                {
                    XmlNode nodItem = nodDEMs.AppendChild(xmlDoc.CreateElement("DEM"));
                    dem.Serialize(nodItem);
                }
            }

            if (ReferenceSurfaces.Count > 0)
            {
                XmlNode nodSurfaces = nodProject.AppendChild(xmlDoc.CreateElement("ReferenceSurfaces"));
                foreach (Surface surf in ReferenceSurfaces.Values)
                {
                    XmlNode nodItem = nodSurfaces.AppendChild(xmlDoc.CreateElement("ReferenceSurface"));
                    surf.Serialize(nodItem);
                }
            }

            if (Masks.Count > 0)
            {
                XmlNode nodMasks = nodProject.AppendChild(xmlDoc.CreateElement("Masks"));
                Masks.Values.ToList().ForEach(x => x.Serialize(nodMasks));
            }

            if (DoDs.Count > 0)
            {
                XmlNode nodDoDs = nodProject.AppendChild(xmlDoc.CreateElement("DoDs"));
                foreach (DoDBase dod in DoDs.Values)
                    dod.Serialize(nodDoDs);
            }

            if (InterComparisons.Count > 0)
            {
                XmlNode nodInter = nodProject.AppendChild(xmlDoc.CreateElement("InterComparisons"));
                InterComparisons.Values.ToList<InterComparison>().ForEach(x => x.Serialize(nodInter));
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

            ProjectManager.Project = new GCDProject(name, desc, projectFile, dtCreated, gcdv, cellArea, units);

            foreach (XmlNode nodDEM in nodProject.SelectNodes("DEMSurveys/DEM"))
            {
                DEMSurvey dem = new DEMSurvey(nodDEM);
                ProjectManager.Project.DEMSurveys[dem.Name] = dem;
            }

            foreach (XmlNode nodRefSurf in nodProject.SelectNodes("ReferenceSurfaces/ReferenceSurface"))
            {
                Surface surf = new Surface(nodRefSurf);
                ProjectManager.Project.ReferenceSurfaces[surf.Name] = surf;
            }

            foreach (XmlNode nodMask in nodProject.SelectNodes("Masks/Mask"))
            {
                if (nodMask.SelectSingleNode("Field") is XmlNode)
                {
                    // Regular or directional mask
                    if (nodMask.SelectSingleNode("Items") is XmlNode)
                    {
                        GCDCore.Project.Masks.RegularMask regMask = new Project.Masks.RegularMask(nodMask);
                        ProjectManager.Project.Masks[regMask.Name] = regMask;
                    }
                    else
                    {
                        GCDCore.Project.Masks.DirectionalMask dirMask = new Project.Masks.DirectionalMask(nodMask);
                        ProjectManager.Project.Masks[dirMask.Name] = dirMask;
                    }
                }
                else
                {
                    // Area of interest
                    GCDCore.Project.Masks.AOIMask aoiMask = new Project.Masks.AOIMask(nodMask);
                    ProjectManager.Project.Masks[aoiMask.Name] = aoiMask;
                }
            }

            foreach (XmlNode nodDoD in nodProject.SelectNodes("DoDs/DoD"))
            {
                DoDBase dod = null;
                if (nodDoD.SelectSingleNode("Threshold") is XmlNode)
                    dod = new DoDMinLoD(nodDoD);
                else if (nodDoD.SelectSingleNode("ConfidenceLevel") is XmlNode)
                    dod = new DoDProbabilistic(nodDoD);
                else
                    dod = new DoDPropagated(nodDoD);

                ProjectManager.Project.DoDs[dod.Name] = dod;
            }

            foreach (XmlNode nodInter in nodProject.SelectNodes("InterComparisons/InterComparison"))
            {
                InterComparison inter = new InterComparison(nodInter, ProjectManager.Project.DoDs);
                ProjectManager.Project.InterComparisons[inter.Name] = inter;
            }

            foreach (XmlNode nodItem in nodProject.SelectNodes("MetaData/Item"))
            {
                ProjectManager.Project.MetaData[nodItem.SelectSingleNode("Key").InnerText] = nodItem.SelectSingleNode("Value").InnerText;
            }

            return ProjectManager.Project;
        }

        public override void Delete()
        {
            throw new NotImplementedException("Deleting entire projects is not implemented");
        }
    }
}
