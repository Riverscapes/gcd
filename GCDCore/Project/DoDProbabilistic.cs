using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using GCDConsoleLib;
using GCDConsoleLib.GCD;

namespace GCDCore.Project
{
    public class DoDProbabilistic : DoDPropagated
    {
        public readonly double ConfidenceLevel;
        public readonly CoherenceProperties SpatialCoherence;

        public readonly ProjectRaster PriorProbability;
        public readonly ProjectRaster PosteriorProbability;
        public readonly ProjectRaster ConditionalRaster;
        public readonly ProjectRaster SpatialCoherenceErosion;
        public readonly ProjectRaster SpatialCoherenceDeposition;

        //public DoDProbabilistic(string name, DirectoryInfo folder, DEMSurvey newDEM, DEMSurvey oldDEM, HistogramPair histograms,
        //    Raster rawDod, Raster thrDod,
        //    ErrorSurface newError, ErrorSurface oldError, FileInfo propErr, FileInfo priorProb, double confidenceLevel, DoDStats stats)
        //    : base(name, folder, newDEM, oldDEM, rawDod, thrDod, histograms, newError, oldError, propErr, stats)
        //{
        //    ConfidenceLevel = confidenceLevel;
        //    PriorProbability = new ProjectRaster(priorProb);
        //}

        public DoDProbabilistic(string name, DirectoryInfo folder, DEMSurvey newDEM, DEMSurvey oldDEM, HistogramPair histograms,
            Raster rawDoD, Raster thrDoD,
            ErrorSurface newError, ErrorSurface oldError, FileInfo propErr, FileInfo priorProb,
            FileInfo postProb, FileInfo cond, FileInfo spatCoEr, FileInfo spatCoDep, CoherenceProperties spatCoProps,
            double confidenceLevel, DoDStats stats)
            : base(name, folder, newDEM, oldDEM, rawDoD, thrDoD, histograms, newError, oldError, propErr, stats)
        {
            ConfidenceLevel = confidenceLevel;
            PriorProbability = new ProjectRaster(priorProb);
            PosteriorProbability = new ProjectRaster(postProb);
            ConditionalRaster = new ProjectRaster(cond);
            SpatialCoherenceErosion = new ProjectRaster(spatCoEr);
            SpatialCoherenceDeposition = new ProjectRaster(spatCoDep);
            SpatialCoherence = spatCoProps;
        }

        public DoDProbabilistic(DoDPropagated dod, FileInfo priorProb, double confidenceLevel)
            : base(dod, dod.PropagatedError.RasterPath, dod.NewError, dod.OldError)
        {
            PriorProbability = new ProjectRaster(priorProb);
            ConfidenceLevel = confidenceLevel;
        }

        public DoDProbabilistic(DoDPropagated dod, FileInfo priorProb, double confidenceLevel,
            FileInfo postProb, FileInfo cond, FileInfo spatCoEr, FileInfo spatCoDep, CoherenceProperties spatCoProps)
       : base(dod, dod.PropagatedError.RasterPath, dod.NewError, dod.OldError)
        {
            PriorProbability = new ProjectRaster(priorProb);
            ConfidenceLevel = confidenceLevel;

            PosteriorProbability = new ProjectRaster(postProb);
            ConditionalRaster = new ProjectRaster(cond);
            SpatialCoherenceErosion = new ProjectRaster(spatCoEr);
            SpatialCoherenceDeposition = new ProjectRaster(spatCoDep);
            SpatialCoherence = spatCoProps;
        }

        new public void Serialize(XmlDocument xmlDoc, XmlNode nodParent)
        {
            XmlNode nodDod = base.Serialize(xmlDoc, nodParent);
            nodDod.InsertBefore(xmlDoc.CreateElement("ConfidenceLevel"), nodDod.SelectSingleNode("Statistics")).InnerText = ConfidenceLevel.ToString("R");

            // Prior probability always exists, regardless of whether spatial coherence was used.
            nodDod.AppendChild(xmlDoc.CreateElement("PriorProbability")).InnerText = ProjectManager.Project.GetRelativePath(PriorProbability.RasterPath);

            // Remaining rasters only exist if spatial coherence was used.
            if (PosteriorProbability != null)
                nodDod.AppendChild(xmlDoc.CreateElement("PosteriorProbability")).InnerText = ProjectManager.Project.GetRelativePath(PosteriorProbability.RasterPath);

            if (ConditionalRaster != null)
                nodDod.AppendChild(xmlDoc.CreateElement("ConditionalRaster")).InnerText = ProjectManager.Project.GetRelativePath(ConditionalRaster.RasterPath);

            if (SpatialCoherenceErosion != null)
                nodDod.AppendChild(xmlDoc.CreateElement("SpatialCoherenceErosion")).InnerText = ProjectManager.Project.GetRelativePath(SpatialCoherenceErosion.RasterPath);

            if (SpatialCoherenceDeposition != null)
                nodDod.AppendChild(xmlDoc.CreateElement("SpatialCoherenceDeposition")).InnerText = ProjectManager.Project.GetRelativePath(SpatialCoherenceDeposition.RasterPath);

            if (SpatialCoherence != null)
            {
                XmlNode nodSpatCo = nodDod.AppendChild(xmlDoc.CreateElement("SpatialCoherence"));
                nodSpatCo.AppendChild(xmlDoc.CreateElement("WindowSize")).InnerText = SpatialCoherence.MovingWindowDimensions.ToString();
                nodSpatCo.AppendChild(xmlDoc.CreateElement("InflectionA")).InnerText = SpatialCoherence.InflectionA.ToString();
                nodSpatCo.AppendChild(xmlDoc.CreateElement("InflectionB")).InnerText = SpatialCoherence.InflectionB.ToString();
            }
        }

        new public static DoDProbabilistic Deserialize(XmlNode nodDoD, Dictionary<string, DEMSurvey> dems)
        {
            DoDPropagated partialDoD = DoDPropagated.Deserialize(nodDoD, dems);

            FileInfo priorProb = ProjectManager.Project.GetAbsolutePath(nodDoD.SelectSingleNode("PriorProbability").InnerText);
            double confidenceLevel = double.Parse(nodDoD.SelectSingleNode("ConfidenceLevel").InnerText);

            DoDProbabilistic dod;
            CoherenceProperties props = null;
            XmlNode nodSpatCo = nodDoD.SelectSingleNode("SpatialCoherence");
            if (nodSpatCo == null)
            {
                dod = new DoDProbabilistic(partialDoD, priorProb, confidenceLevel);
            }
            else
            {
                int windowSize = int.Parse(nodSpatCo.SelectSingleNode("WindowSize").InnerText);
                int inflectinA = int.Parse(nodSpatCo.SelectSingleNode("InflectionA").InnerText);
                int inflectinB = int.Parse(nodSpatCo.SelectSingleNode("InflectionB").InnerText);
                props = new CoherenceProperties(windowSize, inflectinA, inflectinB);

                FileInfo postProb = DeserializeRaster(nodDoD, "PosteriorProbability");
                FileInfo CondRast = DeserializeRaster(nodDoD, "ConditionalRaster");
                FileInfo SpatCoEr = DeserializeRaster(nodDoD, "SpatialCoherenceErosion");
                FileInfo SpatCoDe = DeserializeRaster(nodDoD, "SpatialCoherenceDeposition");

                dod = new DoDProbabilistic(partialDoD, priorProb, confidenceLevel, postProb, CondRast, SpatCoEr, SpatCoDe, props);
            }

            return dod;
        }

        private static FileInfo DeserializeRaster(XmlNode nodParent, string nodeName)
        {
            FileInfo result = null;
            XmlNode nodRaster = nodParent.SelectSingleNode(nodeName);
            if (nodRaster is XmlNode)
                result = ProjectManager.Project.GetAbsolutePath(nodRaster.InnerText);

            return result;
        }
    }
}
