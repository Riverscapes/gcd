using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using System.Globalization;
using Rsxml.ProjectXml;

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
            xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
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

            try
            {
                SaveRiverscapesProject(ProjectFile);
            }
            catch (Exception ex)
            {
                // Fail silently
                Console.WriteLine("Error saving riverscapes project file.");
                Console.WriteLine(ex.Message);
            }
        }

        private void SaveRiverscapesProject(FileInfo gcdProjectFile)
        {
            HashSet<string> usedIds = new HashSet<string>();

            // Global metadata
            List<Meta> metaVals = new List<Meta>
            {
                new Meta("GCDVersion", GCDVersion, null, null, false),
                new Meta("DateCreated", DateTimeCreated.ToString("o"), null, null, false)
            };
            if (!string.IsNullOrEmpty(Description) && !MetaData.ContainsKey("Description"))
                metaVals.Add(new Meta("Description", Description, null, null, false));
            foreach (KeyValuePair<string, string> item in MetaData)
                metaVals.Add(new Meta(item.Key, item.Value, null, null, false));

            List<Realization> realizations = new List<Realization>();

            // 1. One Realization per DEM Survey
            foreach (DEMSurvey dem in DEMSurveys)
            {
                string demId = UniqueId(dem.Name, usedIds);
                List<Dataset> demInputs = new List<Dataset>();

                demInputs.Add(new Dataset(demId, dem.Name,
                    SanitizePath(GetRelativePath(dem.Raster.GISFileInfo)),
                    "DEM", "DEM", null, null, null, null,
                    new MetaData(), null));

                if (dem.Hillshade != null)
                    demInputs.Add(new Dataset(UniqueId(dem.Name + "_hillshade", usedIds), dem.Hillshade.Name,
                        SanitizePath(GetRelativePath(dem.Hillshade.Raster.GISFileInfo)),
                        "Raster", "Hillshade", null, null, null, null,
                        new MetaData(new List<Meta> { new Meta("ParentID", demId, null, null, false) }), null));

                foreach (AssocSurface assoc in dem.AssocSurfaces)
                    demInputs.Add(new Dataset(UniqueId(dem.Name + "_" + assoc.Name, usedIds), assoc.Name,
                        SanitizePath(GetRelativePath(assoc.Raster.GISFileInfo)),
                        "Raster", "AssociatedSurface", null, null, null, null,
                        new MetaData(new List<Meta>
                        {
                            new Meta("ParentID", demId, null, null, false),
                            new Meta("AssocType", assoc.AssocSurfaceType.ToString(), null, null, false)
                        }), null));

                foreach (ErrorSurface err in dem.ErrorSurfaces)
                    demInputs.Add(new Dataset(UniqueId(dem.Name + "_" + err.Name, usedIds), err.Name,
                        SanitizePath(GetRelativePath(err.Raster.GISFileInfo)),
                        "Raster", "ErrorSurface", null, null, null, null,
                        new MetaData(new List<Meta>
                        {
                            new Meta("ParentID", demId, null, null, false),
                            new Meta("IsDefault", err.IsDefault.ToString(), null, null, false)
                        }), null));

                foreach (LinearExtraction.LinearExtraction le in dem.LinearExtractions)
                {
                    if (le.Database != null)
                    {
                        List<Meta> leMeta = new List<Meta>
                        {
                            new Meta("ParentID", demId, null, null, false),
                            new Meta("ProfileRoute", le.ProfileRoute.Name, null, null, false)
                        };
                        LinearExtraction.LinearExtractionFromSurface leFromSurf = le as LinearExtraction.LinearExtractionFromSurface;
                        if (leFromSurf != null && leFromSurf.ErrorSurface != null)
                            leMeta.Add(new Meta("ErrorSurface", leFromSurf.ErrorSurface.Name, null, null, false));
                        demInputs.Add(new Dataset(UniqueId(dem.Name + "_" + le.Name, usedIds), le.Name,
                            SanitizePath(GetRelativePath(le.Database)),
                            "Vector", "LinearExtraction", null, null, null, null,
                            new MetaData(leMeta), null));
                    }
                }

                realizations.Add(new Realization(dem.Name, UniqueId("realization_" + dem.Name, usedIds),
                    DateTimeCreated, SanitizeVersion(GCDVersion),
                    null, null, null,
                    new MetaData(),
                    null, null, null, demInputs, null));
            }

            // 2. One Realization per DoD surface pair; each DoD becomes an Analysis
            Dictionary<Tuple<string, string>, List<DoDBase>> pairGroups = new Dictionary<Tuple<string, string>, List<DoDBase>>();
            foreach (DoDBase dod in DoDs)
            {
                Tuple<string, string> key = Tuple.Create(dod.NewSurface.Name, dod.OldSurface.Name);
                if (!pairGroups.ContainsKey(key))
                    pairGroups[key] = new List<DoDBase>();
                pairGroups[key].Add(dod);
            }

            foreach (KeyValuePair<Tuple<string, string>, List<DoDBase>> pair in pairGroups)
            {
                string pairId = UniqueId("dod_pair_" + pair.Key.Item1 + "_" + pair.Key.Item2, usedIds);
                List<Analysis> pairAnalyses = new List<Analysis>();

                foreach (DoDBase dod in pair.Value)
                {
                    string dodId = UniqueId(dod.Name, usedIds);
                    List<Dataset> products = new List<Dataset>();

                    products.Add(new Dataset(UniqueId(dod.Name + "_raw", usedIds), "Raw DoD",
                        SanitizePath(GetRelativePath(dod.RawDoD.Raster.GISFileInfo)),
                        "Raster", "RawDoD", null, null, null, null,
                        new MetaData(), null));

                    products.Add(new Dataset(UniqueId(dod.Name + "_thresh", usedIds), "Thresholded DoD",
                        SanitizePath(GetRelativePath(dod.ThrDoD.Raster.GISFileInfo)),
                        "Raster", "ThresholdedDoD", null, null, null, null,
                        new MetaData(), null));

                    products.Add(new Dataset(UniqueId(dod.Name + "_err", usedIds), "Thresholded Error",
                        SanitizePath(GetRelativePath(dod.ThrErr.Raster.GISFileInfo)),
                        "Raster", "DoDError", null, null, null, null,
                        new MetaData(), null));

                    if (dod.SummaryXML != null)
                        products.Add(new Dataset(UniqueId(dod.Name + "_summary", usedIds), "DoD Summary",
                            SanitizePath(GetRelativePath(dod.SummaryXML)),
                            "DataTable", "DoDSummary", null, null, null, null,
                            new MetaData(), null));

                    DoDPropagated dodProp = dod as DoDPropagated;
                    if (dodProp != null && dodProp.PropagatedError != null)
                        products.Add(new Dataset(UniqueId(dod.Name + "_prop_err", usedIds), "Propagated Error",
                            SanitizePath(GetRelativePath(dodProp.PropagatedError.GISFileInfo)),
                            "Raster", "PropagatedError", null, null, null, null,
                            new MetaData(), null));

                    DoDProbabilistic dodProb = dod as DoDProbabilistic;
                    if (dodProb != null && dodProb.PriorProbability != null)
                        products.Add(new Dataset(UniqueId(dod.Name + "_prior_prob", usedIds), "Prior Probability",
                            SanitizePath(GetRelativePath(dodProb.PriorProbability.GISFileInfo)),
                            "Raster", "PriorProbability", null, null, null, null,
                            new MetaData(), null));

                    if (dod.Histograms != null)
                    {
                        if (dod.Histograms.Raw.Path != null)
                            products.Add(new Dataset(UniqueId(dod.Name + "_raw_hist", usedIds), "Raw Histogram",
                                SanitizePath(GetRelativePath(dod.Histograms.Raw.Path)),
                                "DataTable", "RawHistogram", null, null, null, null,
                                new MetaData(), null));
                        if (dod.Histograms.Thr.Path != null)
                            products.Add(new Dataset(UniqueId(dod.Name + "_thr_hist", usedIds), "Thresholded Histogram",
                                SanitizePath(GetRelativePath(dod.Histograms.Thr.Path)),
                                "DataTable", "ThresholdHistogram", null, null, null, null,
                                new MetaData(), null));
                    }

                    foreach (BudgetSegregation bs in dod.BudgetSegregations)
                        if (bs.SummaryXML != null)
                            products.Add(new Dataset(UniqueId(dod.Name + "_" + bs.Name + "_bs_summary", usedIds), bs.Name,
                                SanitizePath(GetRelativePath(bs.SummaryXML)),
                                "DataTable", "BudgetSegSummary", null, null, null, null,
                                new MetaData(), null));

                    foreach (LinearExtraction.LinearExtraction le in dod.LinearExtractions)
                    {
                        if (le.Database != null)
                        {
                            List<Meta> leMeta = new List<Meta>
                            {
                                new Meta("ProfileRoute", le.ProfileRoute.Name, null, null, false)
                            };
                            LinearExtraction.LinearExtractionFromSurface leFromSurf = le as LinearExtraction.LinearExtractionFromSurface;
                            if (leFromSurf != null && leFromSurf.ErrorSurface != null)
                                leMeta.Add(new Meta("ErrorSurface", leFromSurf.ErrorSurface.Name, null, null, false));
                            products.Add(new Dataset(UniqueId(dod.Name + "_" + le.Name + "_le", usedIds), le.Name,
                                SanitizePath(GetRelativePath(le.Database)),
                                "Vector", "DoDLinearExtraction", null, null, null, null,
                                new MetaData(leMeta), null));
                        }
                    }

                    List<Meta> dodMeta = new List<Meta>
                    {
                        new Meta("GCD_TYPE", "DoD", null, null, false),
                        new Meta("NewSurface", dod.NewSurface.Name, null, null, false),
                        new Meta("OldSurface", dod.OldSurface.Name, null, null, false)
                    };
                    if (dod.AOIMask != null)
                        dodMeta.Add(new Meta("AOI", dod.AOIMask.Name, null, null, false));
                    dodMeta.Add(new Meta("ErosionRawVolume",
                        dod.Statistics.ErosionRaw.GetVolume(dod.Statistics.CellArea, dod.Statistics.StatsUnits).ToString(), null, null, false));
                    dodMeta.Add(new Meta("DepositionRawVolume",
                        dod.Statistics.DepositionRaw.GetVolume(dod.Statistics.CellArea, dod.Statistics.StatsUnits).ToString(), null, null, false));

                    pairAnalyses.Add(new Analysis(dodId, dod.Name,
                        new MetaData(),
                        new List<Dataset>(), products,
                        null, null, null,
                        new MetaData(dodMeta)));
                }

                realizations.Add(new Realization(pair.Key.Item1 + " / " + pair.Key.Item2, pairId,
                    DateTimeCreated, SanitizeVersion(GCDVersion),
                    null, null, null,
                    new MetaData(),
                    null, null, null, new List<Dataset>(), null, null, pairAnalyses));
            }

            // 3. Summary Realization: reference surfaces, masks, config file, inter-comparisons
            List<Dataset> summaryInputs = new List<Dataset>();

            foreach (Surface surf in ReferenceSurfaces)
            {
                string surfId = UniqueId(surf.Name, usedIds);
                summaryInputs.Add(new Dataset(surfId, surf.Name,
                    SanitizePath(GetRelativePath(surf.Raster.GISFileInfo)),
                    "Raster", "ReferenceSurface", null, null, null, null,
                    new MetaData(), null));

                if (surf.Hillshade != null)
                    summaryInputs.Add(new Dataset(UniqueId(surf.Name + "_hillshade", usedIds), surf.Hillshade.Name,
                        SanitizePath(GetRelativePath(surf.Hillshade.Raster.GISFileInfo)),
                        "Raster", "Hillshade", null, null, null, null,
                        new MetaData(new List<Meta> { new Meta("ParentID", surfId, null, null, false) }), null));

                foreach (ErrorSurface err in surf.ErrorSurfaces)
                    summaryInputs.Add(new Dataset(UniqueId(surf.Name + "_" + err.Name, usedIds), err.Name,
                        SanitizePath(GetRelativePath(err.Raster.GISFileInfo)),
                        "Raster", "ErrorSurface", null, null, null, null,
                        new MetaData(new List<Meta>
                        {
                            new Meta("ParentID", surfId, null, null, false),
                            new Meta("IsDefault", err.IsDefault.ToString(), null, null, false)
                        }), null));

                foreach (LinearExtraction.LinearExtraction le in surf.LinearExtractions)
                {
                    if (le.Database != null)
                    {
                        List<Meta> leMeta = new List<Meta>
                        {
                            new Meta("ParentID", surfId, null, null, false),
                            new Meta("ProfileRoute", le.ProfileRoute.Name, null, null, false)
                        };
                        LinearExtraction.LinearExtractionFromSurface leFromSurf = le as LinearExtraction.LinearExtractionFromSurface;
                        if (leFromSurf != null && leFromSurf.ErrorSurface != null)
                            leMeta.Add(new Meta("ErrorSurface", leFromSurf.ErrorSurface.Name, null, null, false));
                        summaryInputs.Add(new Dataset(UniqueId(surf.Name + "_" + le.Name, usedIds), le.Name,
                            SanitizePath(GetRelativePath(le.Database)),
                            "Vector", "LinearExtraction", null, null, null, null,
                            new MetaData(leMeta), null));
                    }
                }
            }

            foreach (Masks.Mask mask in Masks)
                summaryInputs.Add(new Dataset(UniqueId(mask.Name, usedIds), mask.Name,
                    SanitizePath(GetRelativePath(mask.Vector.GISFileInfo)),
                    "Vector", "Mask", null, null, null, null,
                    new MetaData(), null));

            foreach (ProfileRoutes.ProfileRoute route in ProfileRoutes)
            {
                List<Meta> routeMeta = new List<Meta>
                {
                    new Meta("RouteType", route.ProfileRouteType.ToString(), null, null, false),
                    new Meta("DistanceField", route.DistanceField, null, null, false)
                };
                if (!string.IsNullOrEmpty(route.LabelField))
                    routeMeta.Add(new Meta("LabelField", route.LabelField, null, null, false));
                summaryInputs.Add(new Dataset(UniqueId(route.Name, usedIds), route.Name,
                    SanitizePath(GetRelativePath(route.Vector.GISFileInfo)),
                    "Vector", "ProfileRoute", null, null, null, null,
                    new MetaData(routeMeta), null));
            }

            summaryInputs.Add(new Dataset("gcd_project_file", Name,
                SanitizePath(gcdProjectFile.Name),
                "ConfigFile", "ProjectConfig", null, null, null, null,
                new MetaData(), null));

            List<Analysis> summaryAnalyses = new List<Analysis>();
            foreach (InterComparison ic in InterComparisons)
            {
                List<Dataset> icProducts = new List<Dataset>();
                if (ic._SummaryXML != null)
                    icProducts.Add(new Dataset(UniqueId(ic.Name + "_summary", usedIds), "Inter-Comparison Summary",
                        SanitizePath(GetRelativePath(ic._SummaryXML)),
                        "DataTable", "InterComparisonSummary", null, null, null, null,
                        new MetaData(), null));

                summaryAnalyses.Add(new Analysis(UniqueId(ic.Name, usedIds), ic.Name,
                    new MetaData(),
                    new List<Dataset>(), icProducts,
                    null, null, null,
                    new MetaData(new List<Meta>
                    {
                        new Meta("GCD_TYPE", "InterComparison", null, null, false),
                        new Meta("DoDs", string.Join(", ", ic._DoDs.Select(d => d.Name)), null, null, false)
                    })));
            }

            realizations.Add(new Realization("Project Summary and Analyses", "GCD_ANALYSES",
                DateTimeCreated, SanitizeVersion(GCDVersion),
                null, null, null,
                new MetaData(),
                null, null, null, summaryInputs, null, null, summaryAnalyses));

            FileInfo riverscapesFile = new FileInfo(Path.Combine(gcdProjectFile.DirectoryName, "project.rs.xml"));

            Warehouse existingWarehouse = null;
            if (riverscapesFile.Exists)
            {
                try
                {
                    Rsxml.ProjectXml.Project existing = Rsxml.ProjectXml.Project.LoadProject(riverscapesFile.FullName);
                    existingWarehouse = existing.Warehouse;
                }
                catch (Exception)
                {
                    // If the existing file can't be parsed, proceed without a warehouse
                }
            }

            Rsxml.ProjectXml.Project project = new Rsxml.ProjectXml.Project(
                Name, "GCD", null,
                SanitizePath(GetRelativePath(gcdProjectFile)),
                null, Description, null,
                new MetaData(metaVals),
                existingWarehouse, null, realizations, null);

            project.Write(riverscapesFile.FullName);
        }

        private static string SanitizePath(string path)
        {
            return path.Replace('\\', '/').Replace(' ', '_');
        }

        private static string SanitizeVersion(string version)
        {
            if (string.IsNullOrEmpty(version))
                return "1.0.0";
            List<string> parts = new List<string>(version.Split('.'));
            while (parts.Count < 3)
                parts.Add("0");
            return string.Join(".", parts.Take(3));
        }

        private static string MakeSafeId(string name)
        {
            string safe = Regex.Replace(name.ToLower(), @"[^a-z0-9_]", "_");
            safe = Regex.Replace(safe, @"_+", "_").Trim('_');
            if (safe.Length > 64)
                return safe.Substring(0, 30) + "_" + safe.Substring(safe.Length - 33);
            return string.IsNullOrEmpty(safe) ? "id" : safe;
        }

        private static string UniqueId(string name, HashSet<string> usedIds)
        {
            string baseId = MakeSafeId(name);
            string candidate = baseId;
            int counter = 2;
            while (usedIds.Contains(candidate))
            {
                string suffix = "_" + counter;
                candidate = baseId.Substring(0, Math.Min(baseId.Length, 64 - suffix.Length)) + suffix;
                counter++;
            }
            usedIds.Add(candidate);
            return candidate;
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
