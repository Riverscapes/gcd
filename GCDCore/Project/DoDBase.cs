﻿using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using GCDConsoleLib;
using GCDConsoleLib.GCD;
using System.Xml;

namespace GCDCore.Project
{
    public abstract class DoDBase : GCDProjectItem
    {
        public readonly DirectoryInfo Folder;
        public readonly Surface NewSurface;
        public readonly Surface OldSurface;
        public readonly Masks.AOIMask AOIMask;

        public DoDRaster RawDoD { get; internal set; }
        public DoDRaster ThrDoD { get; internal set; }

        public HistogramPair Histograms { get; set; }
        public FileInfo SummaryXML { get; set; }
        public readonly DoDStats Statistics;

        public Dictionary<string, BudgetSegregation> BudgetSegregations { get; internal set; }
        public readonly Dictionary<string, LinearExtraction.LinearExtraction> LinearExtractions;

        public override string Noun { get { return "Change Detection"; } }

        public abstract string UncertaintyAnalysisLabel
        {
            get;
        }

        /// <summary>
        /// A DoD is in use if any inter-comparisons refer to it
        /// </summary>
        public override bool IsItemInUse
        {
            get
            {
                return ProjectManager.Project.InterComparisons.Values.Any(x => x._DoDs.Contains(this));
            }
        }

        public string AOILabel { get { return AOIMask == null ? Masks.AOIMask.SurfaceDataExtentIntersection : AOIMask.Name; } }

        public DirectoryInfo BudgetSegFolder { get { return new DirectoryInfo(Path.Combine(Folder.FullName, "BS")); } }

        public DirectoryInfo BudgetSegPath()
        {
            return ProjectManager.GetIndexedSubDirectory(BudgetSegFolder, "BS");
        }

        public static DirectoryInfo FiguresFolderPath(DirectoryInfo dodDir)
        {
            return new DirectoryInfo(Path.Combine(dodDir.FullName, "Figs"));
        }

        protected DoDBase(string name, DirectoryInfo folder, Surface newSurface, Surface oldSurface, Masks.AOIMask aoi, Raster rawDoD, Raster thrDoD, HistogramPair histograms, FileInfo summaryXML, DoDStats stats)
            : base(name)
        {
            Folder = folder;
            NewSurface = newSurface;
            OldSurface = oldSurface;
            AOIMask = aoi;
            RawDoD = new DoDRaster(string.Format(Name + " - Raw"), rawDoD);
            ThrDoD = new DoDRaster(string.Format(Name + " - Thresholded"), thrDoD);
            Histograms = histograms;
            SummaryXML = summaryXML;
            Statistics = stats;
            BudgetSegregations = new Dictionary<string, BudgetSegregation>();
            LinearExtractions = new Dictionary<string, LinearExtraction.LinearExtraction>();
        }

        protected DoDBase(XmlNode nodDoD)
            : base(nodDoD.SelectSingleNode("Name").InnerText)
        {
            Folder = ProjectManager.Project.GetAbsoluteDir(nodDoD.SelectSingleNode("Folder").InnerText);
            NewSurface = DeserializeSurface(nodDoD, "NewSurface");
            OldSurface = DeserializeSurface(nodDoD, "OldSurface");

            XmlNode nodAOI = nodDoD.SelectSingleNode("AOI");
            if (nodAOI is XmlNode)
            {
                AOIMask = ProjectManager.Project.Masks[nodAOI.InnerText] as Masks.AOIMask;
            }

            RawDoD = new DoDRaster(string.Format(Name + " - Raw"), ProjectManager.Project.GetAbsolutePath(nodDoD.SelectSingleNode("RawDoD").InnerText));
            ThrDoD = new DoDRaster(string.Format(Name + " - Thresholded"), ProjectManager.Project.GetAbsolutePath(nodDoD.SelectSingleNode("ThrDoD").InnerText));
            Histograms = new HistogramPair(ProjectManager.Project.GetAbsolutePath(nodDoD.SelectSingleNode("RawHistogram").InnerText),
                ProjectManager.Project.GetAbsolutePath(nodDoD.SelectSingleNode("ThrHistogram").InnerText));
            SummaryXML = ProjectManager.Project.GetAbsolutePath(nodDoD.SelectSingleNode("SummaryXML").InnerText);
            Statistics = DeserializeStatistics(nodDoD.SelectSingleNode("Statistics"), ProjectManager.Project.CellArea, ProjectManager.Project.Units);

            BudgetSegregations = new Dictionary<string, BudgetSegregation>();
            XmlNode nodBSes = nodDoD.SelectSingleNode("BudgetSegregations");
            if (nodBSes is XmlNode)
            {
                foreach (XmlNode nodBS in nodBSes.SelectNodes("BudgetSegregation"))
                {
                    BudgetSegregation bs = new BudgetSegregation(nodBS, this);
                    BudgetSegregations[bs.Name] = bs;
                }
            }

            LinearExtractions = new Dictionary<string, LinearExtraction.LinearExtraction>();
            foreach (XmlNode nodLE in nodDoD.SelectNodes("LinearExtractions/LinearExtraction"))
            {
                LinearExtraction.LinearExtraction le = new LinearExtraction.LinearExtractionFromDoD(nodLE);
                LinearExtractions[le.Name] = le;
            }
        }

        public bool IsBudgetSegNameUnique(string name, BudgetSegregation ignore)
        {
            return BudgetSegregations.ContainsKey(name) ? BudgetSegregations[name] == ignore : true;
        }

        public virtual XmlNode Serialize(XmlNode nodParent)
        {
            XmlNode nodDoD = nodParent.AppendChild(nodParent.OwnerDocument.CreateElement("DoD"));
            nodDoD.AppendChild(nodParent.OwnerDocument.CreateElement("Name")).InnerText = Name;
            nodDoD.AppendChild(nodParent.OwnerDocument.CreateElement("Folder")).InnerText = ProjectManager.Project.GetRelativePath(Folder.FullName);
            SerializeSurface(nodDoD, NewSurface, "NewSurface");
            SerializeSurface(nodDoD, OldSurface, "OldSurface");

            if (AOIMask != null)
            {
                nodDoD.AppendChild(nodParent.OwnerDocument.CreateElement("AOI")).InnerText = AOIMask.Name;
            }

            nodDoD.AppendChild(nodParent.OwnerDocument.CreateElement("RawDoD")).InnerText = ProjectManager.Project.GetRelativePath(RawDoD.Raster.GISFileInfo);
            nodDoD.AppendChild(nodParent.OwnerDocument.CreateElement("ThrDoD")).InnerText = ProjectManager.Project.GetRelativePath(ThrDoD.Raster.GISFileInfo);
            nodDoD.AppendChild(nodParent.OwnerDocument.CreateElement("RawHistogram")).InnerText = ProjectManager.Project.GetRelativePath(Histograms.Raw.Path);
            nodDoD.AppendChild(nodParent.OwnerDocument.CreateElement("ThrHistogram")).InnerText = ProjectManager.Project.GetRelativePath(Histograms.Thr.Path);
            nodDoD.AppendChild(nodParent.OwnerDocument.CreateElement("SummaryXML")).InnerText = ProjectManager.Project.GetRelativePath(SummaryXML);

            SerializeDoDStatistics(nodParent.OwnerDocument, nodDoD.AppendChild(nodParent.OwnerDocument.CreateElement("Statistics")), Statistics);

            if (BudgetSegregations.Count > 0)
            {
                XmlNode nodBS = nodDoD.AppendChild(nodParent.OwnerDocument.CreateElement("BudgetSegregations"));
                foreach (BudgetSegregation bs in BudgetSegregations.Values)
                    bs.Serialize(nodBS);
            }

            if (LinearExtractions.Count > 0)
            {
                XmlNode nodLE = nodDoD.AppendChild(nodDoD.OwnerDocument.CreateElement("LinearExtractions"));
                LinearExtractions.Values.ToList().ForEach(x => x.Serialize(nodLE));
            }

            // Return this so inherited classes can append to it.
            return nodDoD;
        }

        public static void SerializeDoDStatistics(XmlDocument xmlDoc, XmlNode nodParent, DoDStats stats)
        {
            XmlNode nodErosion = nodParent.AppendChild(xmlDoc.CreateElement("Erosion"));
            SerializeAreaVolume(xmlDoc, nodErosion.AppendChild(xmlDoc.CreateElement("Raw")), stats.ErosionRaw, stats.StatsUnits, stats.CellArea);
            SerializeAreaVolume(xmlDoc, nodErosion.AppendChild(xmlDoc.CreateElement("Thresholded")), stats.ErosionThr, stats.StatsUnits, stats.CellArea);
            nodErosion.AppendChild(xmlDoc.CreateElement("Error")).AppendChild(xmlDoc.CreateElement("Volume")).InnerText =
                stats.ErosionErr.GetVolume(stats.CellArea, stats.StatsUnits.VertUnit).As(stats.StatsUnits.VolUnit).ToString("R");

            XmlNode nodDeposition = nodParent.AppendChild(xmlDoc.CreateElement("Deposition"));
            SerializeAreaVolume(xmlDoc, nodDeposition.AppendChild(xmlDoc.CreateElement("Raw")), stats.DepositionRaw, stats.StatsUnits, stats.CellArea);
            SerializeAreaVolume(xmlDoc, nodDeposition.AppendChild(xmlDoc.CreateElement("Thresholded")), stats.DepositionThr, stats.StatsUnits, stats.CellArea);
            nodDeposition.AppendChild(xmlDoc.CreateElement("Error")).AppendChild(xmlDoc.CreateElement("Volume")).InnerText =
                stats.DepositionErr.GetVolume(stats.CellArea, stats.StatsUnits.VertUnit).As(stats.StatsUnits.VolUnit).ToString("R");
        }

        private static void SerializeAreaVolume(XmlDocument xmlDoc, XmlNode nodParent, GCDAreaVolume areaVol, UnitGroup units, UnitsNet.Area cellArea)
        {
            nodParent.AppendChild(xmlDoc.CreateElement("Area")).InnerText = areaVol.GetArea(cellArea).As(units.ArUnit).ToString("R");
            nodParent.AppendChild(xmlDoc.CreateElement("Volume")).InnerText = areaVol.GetVolume(cellArea, units.VertUnit).As(units.VolUnit).ToString("R");
        }

        private void SerializeSurface(XmlNode nodDoD, Surface surface, string nodName)
        {
            XmlNode nodSurface = nodDoD.AppendChild(nodDoD.OwnerDocument.CreateElement(nodName));
            nodSurface.InnerText = surface.Name;

            // Track whether the surface is a DEM or reference surface as an attribute.
            // This is needed for derserialization to look in the correct dictionary.
            nodSurface.Attributes.Append(nodDoD.OwnerDocument.CreateAttribute("type")).InnerText = surface is DEMSurvey ? "dem" : "surface";
        }

        private Surface DeserializeSurface(XmlNode nodDoD, string nodDEMName)
        {
            string surfaceName = nodDoD.SelectSingleNode(nodDEMName).InnerText;
            string surfaceType = nodDoD.SelectSingleNode(nodDEMName).Attributes["type"].InnerText;

            if (string.Compare(surfaceType, "dem", true) == 0)
            {
                return ProjectManager.Project.DEMSurveys[surfaceName];
            }
            else
            {
                return ProjectManager.Project.ReferenceSurfaces[surfaceName];
            }
        }

        /// <summary>
        /// Deserialize change statistics
        /// </summary>
        /// <param name="nodStatistics"></param>
        /// <param name="cellArea"></param>
        /// <param name="units"></param>
        /// <returns></returns>
        /// <remarks>This is used by budget segregations and so needs to be static and public</remarks>
        public static DoDStats DeserializeStatistics(XmlNode nodStatistics, UnitsNet.Area cellArea, UnitGroup units)
        {
            UnitsNet.Area AreaErosion_Raw = UnitsNet.Area.From(double.Parse(nodStatistics.SelectSingleNode("Erosion/Raw/Area").InnerText), units.ArUnit);
            UnitsNet.Area AreaDeposit_Raw = UnitsNet.Area.From(double.Parse(nodStatistics.SelectSingleNode("Deposition/Raw/Area").InnerText), units.ArUnit);
            UnitsNet.Area AreaErosion_Thr = UnitsNet.Area.From(double.Parse(nodStatistics.SelectSingleNode("Erosion/Thresholded/Area").InnerText), units.ArUnit);
            UnitsNet.Area AreaDeposit_Thr = UnitsNet.Area.From(double.Parse(nodStatistics.SelectSingleNode("Deposition/Thresholded/Area").InnerText), units.ArUnit);

            UnitsNet.Volume VolErosion_Raw = UnitsNet.Volume.From(double.Parse(nodStatistics.SelectSingleNode("Erosion/Raw/Volume").InnerText), units.VolUnit);
            UnitsNet.Volume VolDeposit_Raw = UnitsNet.Volume.From(double.Parse(nodStatistics.SelectSingleNode("Deposition/Raw/Volume").InnerText), units.VolUnit);
            UnitsNet.Volume VolErosion_Thr = UnitsNet.Volume.From(double.Parse(nodStatistics.SelectSingleNode("Erosion/Thresholded/Volume").InnerText), units.VolUnit);
            UnitsNet.Volume VolDeposit_Thr = UnitsNet.Volume.From(double.Parse(nodStatistics.SelectSingleNode("Deposition/Thresholded/Volume").InnerText), units.VolUnit);

            UnitsNet.Volume VolErosion_Err = UnitsNet.Volume.From(double.Parse(nodStatistics.SelectSingleNode("Erosion/Error/Volume").InnerText), units.VolUnit);
            UnitsNet.Volume VolDeposit_Err = UnitsNet.Volume.From(double.Parse(nodStatistics.SelectSingleNode("Deposition/Error/Volume").InnerText), units.VolUnit);

            return new DoDStats(
                AreaErosion_Raw, AreaDeposit_Raw, AreaErosion_Thr, AreaDeposit_Thr,
                VolErosion_Raw, VolDeposit_Raw, VolErosion_Thr, VolDeposit_Thr,
                VolErosion_Err, VolDeposit_Err,
                cellArea, units);
        }

        private static UnitsNet.Area DeserializeArea(XmlNode nodParent, string nodName, UnitsNet.Units.AreaUnit unit)
        {
            double value = double.Parse(nodParent.SelectSingleNode(nodName).InnerText);
            return UnitsNet.Area.From(value, unit);
        }

        public override void Delete()
        {
            try
            {
                BudgetSegregations.Values.ToList().ForEach(x => x.Delete());
            }
            finally
            {
                BudgetSegregations.Clear();
            }

            // Delete the raw and thresholded rasters
            RawDoD.Delete();
            ThrDoD.Delete();

            // Now delete all the meta files associated with the DoD
            foreach (FileInfo file in RawDoD.Raster.GISFileInfo.Directory.GetFiles("*.*", SearchOption.AllDirectories))
            {
                try
                {
                    file.Delete();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to delete file " + file.FullName);
                    Console.WriteLine(ex.Message);
                }
            }

            // Delete the figures folder
            DirectoryInfo dirFigs = FiguresFolderPath(Folder);
            try
            {
                System.IO.Directory.Delete(dirFigs.FullName, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to delete DoD figures folder " + dirFigs.FullName);
                Console.WriteLine(ex.Message);
            }

            // Try to delete the DoD folder 
            try
            {
                Folder.Delete();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to delete DoD folder  " + Folder.FullName);
                Console.WriteLine(ex.Message);
            }

            // If there are no more DoDs try to delete the parent change detection folder "CD"
            if (Folder.Parent.GetDirectories().Length < 1)
            {
                Folder.Parent.Delete();
            }

            // If there are no more analyses then delete this folder
            if (Folder.Parent.Parent.GetDirectories().Length < 1)
            {
                Folder.Parent.Parent.Delete();
            }

            // Remove the DoD from the project
            ProjectManager.Project.DoDs.Remove(Name);
            ProjectManager.Project.Save();
        }
    }
}
