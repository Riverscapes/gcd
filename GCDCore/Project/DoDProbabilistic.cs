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
        public readonly decimal ConfidenceLevel;
        public readonly CoherenceProperties SpatialCoherence;

        public readonly Raster PriorProbability;
        public readonly Raster PosteriorProbability;
        public readonly Raster ConditionalRaster;
        public readonly Raster SpatialCoherenceErosion;
        public readonly Raster SpatialCoherenceDeposition;

        public override string UncertaintyAnalysisLabel
        {
            get
            {
                return string.Format("Probabilistic Thresholding at the {0}% confidence level", (100 * ConfidenceLevel).ToString("0") + "%");
            }
        }

        public DoDProbabilistic(string name, DirectoryInfo folder, Surface newSurface, Surface oldSurface, HistogramPair histograms, FileInfo summaryXML,
            Raster rawDoD, Raster thrDoD,
            ErrorSurface newError, ErrorSurface oldError, Raster propErr, FileInfo priorProb,
            FileInfo postProb, FileInfo cond, FileInfo spatCoEr, FileInfo spatCoDep, CoherenceProperties spatCoProps,
            decimal confidenceLevel, DoDStats stats)
            : base(name, folder, newSurface, oldSurface, rawDoD, thrDoD, histograms, summaryXML, newError, oldError, propErr, stats)
        {
            ConfidenceLevel = confidenceLevel;
            PriorProbability = new Raster(priorProb);

            if (spatCoProps != null)
            {
                PosteriorProbability = new Raster(postProb);
                ConditionalRaster = new Raster(cond);
                SpatialCoherenceErosion = new Raster(spatCoEr);
                SpatialCoherenceDeposition = new Raster(spatCoDep);
                SpatialCoherence = spatCoProps;
            }
        }

        public DoDProbabilistic(XmlNode nodDoD)
            : base(nodDoD)
        {
            ConfidenceLevel = decimal.Parse(nodDoD.SelectSingleNode("ConfidenceLevel").InnerText);
            PriorProbability = DeserializeRaster(nodDoD, "PriorProbability");

            XmlNode nodSpatCo = nodDoD.SelectSingleNode("SpatialCoherence");
            if (nodSpatCo != null)
            {
                int windowSize = int.Parse(nodSpatCo.SelectSingleNode("WindowSize").InnerText);
                int inflectinA = int.Parse(nodSpatCo.SelectSingleNode("InflectionA").InnerText);
                int inflectinB = int.Parse(nodSpatCo.SelectSingleNode("InflectionB").InnerText);
                SpatialCoherence = new CoherenceProperties(windowSize, inflectinA, inflectinB);

                PosteriorProbability = DeserializeRaster(nodDoD, "PosteriorProbability");
                ConditionalRaster = DeserializeRaster(nodDoD, "ConditionalRaster");
                SpatialCoherenceErosion = DeserializeRaster(nodDoD, "SpatialCoherenceErosion");
                SpatialCoherenceDeposition = DeserializeRaster(nodDoD, "SpatialCoherenceDeposition");
            }
        }

        public override XmlNode Serialize(XmlNode nodParent)
        {
            XmlNode nodDod = base.Serialize(nodParent);
            nodDod.InsertBefore(nodParent.OwnerDocument.CreateElement("ConfidenceLevel"), nodDod.SelectSingleNode("Statistics")).InnerText = ConfidenceLevel.ToString();

            // Prior probability always exists, regardless of whether spatial coherence was used.
            nodDod.AppendChild(nodParent.OwnerDocument.CreateElement("PriorProbability")).InnerText = ProjectManager.Project.GetRelativePath(PriorProbability.GISFileInfo);

            // Remaining rasters only exist if spatial coherence was used.
            if (PosteriorProbability != null)
                nodDod.AppendChild(nodParent.OwnerDocument.CreateElement("PosteriorProbability")).InnerText = ProjectManager.Project.GetRelativePath(PosteriorProbability.GISFileInfo);

            if (ConditionalRaster != null)
                nodDod.AppendChild(nodParent.OwnerDocument.CreateElement("ConditionalRaster")).InnerText = ProjectManager.Project.GetRelativePath(ConditionalRaster.GISFileInfo);

            if (SpatialCoherenceErosion != null)
                nodDod.AppendChild(nodParent.OwnerDocument.CreateElement("SpatialCoherenceErosion")).InnerText = ProjectManager.Project.GetRelativePath(SpatialCoherenceErosion.GISFileInfo);

            if (SpatialCoherenceDeposition != null)
                nodDod.AppendChild(nodParent.OwnerDocument.CreateElement("SpatialCoherenceDeposition")).InnerText = ProjectManager.Project.GetRelativePath(SpatialCoherenceDeposition.GISFileInfo);

            if (SpatialCoherence != null)
            {
                XmlNode nodSpatCo = nodDod.AppendChild(nodParent.OwnerDocument.CreateElement("SpatialCoherence"));
                nodSpatCo.AppendChild(nodParent.OwnerDocument.CreateElement("WindowSize")).InnerText = SpatialCoherence.BufferSize.ToString();
                nodSpatCo.AppendChild(nodParent.OwnerDocument.CreateElement("InflectionA")).InnerText = SpatialCoherence.InflectionA.ToString();
                nodSpatCo.AppendChild(nodParent.OwnerDocument.CreateElement("InflectionB")).InnerText = SpatialCoherence.InflectionB.ToString();
            }

            return nodDod;
        }

        private Raster DeserializeRaster(XmlNode nodParent, string nodeName)
        {
            Raster result = null;
            XmlNode nodRaster = nodParent.SelectSingleNode(nodeName);
            if (nodRaster is XmlNode)
                result = new Raster(ProjectManager.Project.GetAbsolutePath(nodRaster.InnerText));

            return result;
        }

        public override void Delete()
        {
            DeleteRaster(PriorProbability);
            DeleteRaster(PosteriorProbability);
            DeleteRaster(ConditionalRaster);
            DeleteRaster(SpatialCoherenceErosion);
            DeleteRaster(SpatialCoherenceDeposition);
            base.Delete();
        }
    }
}
