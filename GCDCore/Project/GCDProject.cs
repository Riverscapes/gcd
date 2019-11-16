using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Globalization;

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

        // Variables we need access to from the online stuff
        public Dictionary<string, string> OnlineParams;

        public readonly List<DEMSurvey> DEMSurveys;
        public readonly List<Surface> ReferenceSurfaces;
        public readonly List<DoDBase> DoDs;
        public readonly List<InterComparison> InterComparisons;
        public readonly List<Masks.Mask> Masks;
        public readonly List<ProfileRoutes.ProfileRoute> ProfileRoutes;
        public readonly Dictionary<string, string> MetaData;

        public override string Noun { get { return "GCD Project"; } }

        // Project Inputs Folder Paths
        public DirectoryInfo Folder { get { return ProjectFile.Directory; } }
        public DirectoryInfo InputsFolder { get { return ProjectManager.CombinePaths(Folder, "Inputs"); } }
        public DirectoryInfo SurveysFolder { get { return ProjectManager.CombinePaths(InputsFolder, "Surveys"); } }
        public DirectoryInfo ReferenceSurfacesFolder { get { return ProjectManager.CombinePaths(InputsFolder, "RefSurf"); } }
        public DirectoryInfo MasksFolder { get { return ProjectManager.CombinePaths(InputsFolder, "Masks"); } }
        public DirectoryInfo ProfileRoutesFolder { get { return ProjectManager.CombinePaths(InputsFolder, "Routes"); } }

        // Project Analyses Folder Paths
        public DirectoryInfo AnalysesFolder { get { return ProjectManager.CombinePaths(Folder, "Analyses"); } }
        public DirectoryInfo ChangeDetectionFolder { get { return ProjectManager.CombinePaths(AnalysesFolder, "CD"); } }
        public DirectoryInfo InterComparisonsFolder { get { return ProjectManager.CombinePaths(AnalysesFolder, "IC"); } }

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

        public FileInfo DEMSurveyPath(string name)
        {
            return ProjectManager.GetProjectItemPath(SurveysFolder, "DEM", name, ProjectManager.RasterExtension);
        }

        public FileInfo ReferenceSurfacePath(string name)
        {
            return ProjectManager.GetProjectItemPath(ReferenceSurfacesFolder, "Ref", name, ProjectManager.RasterExtension);
        }

        public FileInfo MaskPath(string name)
        {
            return ProjectManager.GetProjectItemPath(MasksFolder, "Mask", name, "shp");
        }

        public FileInfo InterComparisonPath(string name)
        {
            return ProjectManager.GetProjectItemPath(InterComparisonsFolder, "IC", name, "xml");
        }

        public FileInfo ProfileRoutePath(string name)
        {
            return ProjectManager.GetProjectItemPath(ProfileRoutesFolder, "Mask", name, ProjectManager.RasterExtension);
        }

        /// <summary>
        /// Get the spatial reference of the first GIS dataset in the project.
        /// </summary>
        public GCDConsoleLib.Projection ReferenceProjection
        {
            get
            {
                if (DEMSurveys.Count > 0)
                    return DEMSurveys.First().Raster.Proj;

                if (ReferenceSurfaces.Count > 0)
                    return ReferenceSurfaces.First().Raster.Proj;

                if (Masks.Count > 0)
                    return Masks.First().Vector.Proj;

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

            DEMSurveys = new List<DEMSurvey>();
            ReferenceSurfaces = new List<Surface>();
            Masks = new List<Masks.Mask>();
            DoDs = new List<DoDBase>();
            InterComparisons = new List<InterComparison>();
            ProfileRoutes = new List<ProfileRoutes.ProfileRoute>();
            MetaData = new Dictionary<string, string>();

            OnlineParams = new Dictionary<string, string>();
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
            List<DEMSurvey> dems = DEMSurveys.OrderBy(x => x.Name).ToList<DEMSurvey>();

            if (!bAscending)
                dems.Reverse();

            return dems;
        }

        public IEnumerable<DEMSurvey> DEMsSortByDate(bool bAscending)
        {
            List<DEMSurvey> dems = DEMSurveys.OrderBy(x => x.SurveyDate).ToList<DEMSurvey>();

            if (!bAscending)
                dems.Reverse();

            return dems;
        }

        public FileInfo GetAbsolutePath(string sRelativePath)
        {
            if (string.IsNullOrEmpty(sRelativePath))
                return null;
            else if (sRelativePath.Contains(":") || sRelativePath.StartsWith("\\\\"))
                return new FileInfo(sRelativePath);
            else
                return new FileInfo(Path.Combine(ProjectFile.DirectoryName, sRelativePath));
        }

        public DirectoryInfo GetAbsoluteDir(string sRelativeDir)
        {
            return new DirectoryInfo(Path.Combine(ProjectFile.DirectoryName, sRelativeDir));
        }

        public bool IsNameUnique(string name, List<GCDProjectItem> items, GCDProjectItem ignore)
        {
            return !items.Any(x => x != ignore && string.Compare(name, x.Name, true) == 0);
        }

        public bool IsDEMNameUnique(string name, DEMSurvey ignore)
        {
            return IsNameUnique(name, DEMSurveys.Select(x => x as GCDProjectItem).ToList<GCDProjectItem>(), ignore);
        }

        public bool IsReferenceSurfaceNameUnique(string name, Surface ignore)
        {
            return IsNameUnique(name, ReferenceSurfaces.Select(x => x as GCDProjectItem).ToList<GCDProjectItem>(), ignore);
        }

        public bool IsDoDNameUnique(string name, DoDBase ignore)
        {
            return IsNameUnique(name, DoDs.Select(x => x as GCDProjectItem).ToList<GCDProjectItem>(), ignore);
        }

        public bool IsMaskNameUnique(string name, Masks.Mask ignore)
        {
            return IsNameUnique(name, Masks.Select(x => x as GCDProjectItem).ToList<GCDProjectItem>(), ignore);
        }

        public bool IsProfileRouteNameUnique(string name, GCDCore.Project.ProfileRoutes.ProfileRoute ignore)
        {
            return IsNameUnique(name, ProfileRoutes.Select(x => x as GCDProjectItem).ToList<GCDProjectItem>(), ignore);
        }

        public bool IsInterComparisonNameUnique(string name, InterComparison ignore)
        {
            return IsNameUnique(name, InterComparisons.Select(x => x as GCDProjectItem).ToList<GCDProjectItem>(), ignore);
        }

        public DirectoryInfo GetDoDFolder()
        {
            return ProjectManager.GetIndexedSubDirectory(ChangeDetectionFolder, "DoD");
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

            if (OnlineParams.Count > 0)
            {
                XmlNode nodUpload = nodProject.AppendChild(xmlDoc.CreateElement("Online"));
                foreach (KeyValuePair<string, string> kvp in OnlineParams)
                {
                    XmlNode kvpItem = nodUpload.AppendChild(xmlDoc.CreateElement(kvp.Key));
                    kvpItem.InnerText = kvp.Value;
                }
            }

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
                nodArea.InnerText = CellArea.As(Units.ArUnit).ToString("R", CultureInfo.InvariantCulture);

            if (DEMSurveys.Count > 0)
            {
                XmlNode nodDEMs = nodProject.AppendChild(xmlDoc.CreateElement("DEMSurveys"));
                foreach (DEMSurvey dem in DEMSurveys)
                {
                    XmlNode nodItem = nodDEMs.AppendChild(xmlDoc.CreateElement("DEM"));
                    dem.Serialize(nodItem);
                }
            }

            if (ReferenceSurfaces.Count > 0)
            {
                XmlNode nodSurfaces = nodProject.AppendChild(xmlDoc.CreateElement("ReferenceSurfaces"));
                foreach (Surface surf in ReferenceSurfaces)
                {
                    XmlNode nodItem = nodSurfaces.AppendChild(xmlDoc.CreateElement("ReferenceSurface"));
                    surf.Serialize(nodItem);
                }
            }

            if (Masks.Count > 0)
            {
                XmlNode nodMasks = nodProject.AppendChild(xmlDoc.CreateElement("Masks"));
                Masks.ForEach(x => x.Serialize(nodMasks));
            }

            if (ProfileRoutes.Count > 0)
            {
                XmlNode nodRoutes = nodProject.AppendChild(xmlDoc.CreateElement("ProfileRoutes"));
                ProfileRoutes.ForEach(x => x.Serialize(nodRoutes));
            }

            if (DoDs.Count > 0)
            {
                XmlNode nodDoDs = nodProject.AppendChild(xmlDoc.CreateElement("DoDs"));
                foreach (DoDBase dod in DoDs)
                    dod.Serialize(nodDoDs);
            }

            if (InterComparisons.Count > 0)
            {
                XmlNode nodInter = nodProject.AppendChild(xmlDoc.CreateElement("InterComparisons"));
                InterComparisons.ForEach(x => x.Serialize(nodInter));
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
                cellArea = UnitsNet.Area.From(double.Parse(nodCellArea.InnerText, CultureInfo.InvariantCulture), area);

            ProjectManager.Project = new GCDProject(name, desc, projectFile, dtCreated, gcdv, cellArea, units);

            foreach (XmlNode nodOnline in nodProject.SelectNodes("Online/*"))
                ProjectManager.Project.OnlineParams[nodOnline.Name] = nodOnline.InnerText;

            // Load masks before DEMs. DEMs will load error surfaces that refer to masks
            foreach (XmlNode nodMask in nodProject.SelectNodes("Masks/Mask"))
            {
                if (nodMask.SelectSingleNode("Field") is XmlNode)
                {
                    // Regular or directional mask
                    if (nodMask.SelectSingleNode("Items") is XmlNode)
                    {
                        ProjectManager.Project.Masks.Add(new Masks.RegularMask(nodMask));
                    }
                    else
                    {
                        ProjectManager.Project.Masks.Add(new Project.Masks.DirectionalMask(nodMask));
                    }
                }
                else
                {
                    // Area of interest
                    ProjectManager.Project.Masks.Add(new Project.Masks.AOIMask(nodMask));
                }
            }

            // Load profile routes before DEMs, Ref Surfs and DoDs that might refer to
            // routes in their linear extractions
            foreach (XmlNode nodRoute in nodProject.SelectNodes("ProfileRoutes/ProfileRoute"))
            {
                ProjectManager.Project.ProfileRoutes.Add(new Project.ProfileRoutes.ProfileRoute(nodRoute));
            }

            foreach (XmlNode nodDEM in nodProject.SelectNodes("DEMSurveys/DEM"))
            {
                DEMSurvey dem = new DEMSurvey(nodDEM);
                ProjectManager.Project.DEMSurveys.Add(dem);
            }

            foreach (XmlNode nodRefSurf in nodProject.SelectNodes("ReferenceSurfaces/ReferenceSurface"))
            {
                Surface surf = new Surface(nodRefSurf, true, true);
                ProjectManager.Project.ReferenceSurfaces.Add(surf);
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

                ProjectManager.Project.DoDs.Add(dod);
            }

            foreach (XmlNode nodInter in nodProject.SelectNodes("InterComparisons/InterComparison"))
            {
                InterComparison inter = new InterComparison(nodInter, ProjectManager.Project.DoDs);
                ProjectManager.Project.InterComparisons.Add(inter);
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
