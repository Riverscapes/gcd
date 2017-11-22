using System;
using System.IO;
using System.Xml;

namespace GCDCore.Project
{
    public class DoDProbabilityRasters
    {
        public readonly CoherenceProperties SpatialCoherence;

        public readonly ProjectRaster PriorProbability;
        public readonly ProjectRaster PosteriorProbability;
        public readonly ProjectRaster ConditionalRaster;
        public readonly ProjectRaster SpatialCoherenceErosion;
        public readonly ProjectRaster SpatialCoherenceDeposition;

        /// <summary>
        /// Probabilistic thresholding rasters when not using spatial coherence
        /// </summary>
        /// <param name="priorProb"></param>
        public DoDProbabilityRasters(FileInfo priorProb)
        {
            PriorProbability = new ProjectRaster(priorProb);
        }

        /// <summary>
        /// Probabilistic thresholding rasters with spatial coherence
        /// </summary>
        /// <param name="priorProb"></param>
        /// <param name="postProb"></param>
        /// <param name="cond"></param>
        /// <param name="spatCoEr"></param>
        /// <param name="spatCoDep"></param>
        /// <param name="SpatCo"></param>
        public DoDProbabilityRasters(FileInfo priorProb, FileInfo postProb, FileInfo cond, FileInfo spatCoEr, FileInfo spatCoDep, CoherenceProperties SpatCo)
        {
            PriorProbability = new ProjectRaster(priorProb);
            PosteriorProbability = new ProjectRaster(postProb);
            ConditionalRaster = new ProjectRaster(cond);
            SpatialCoherenceErosion = new ProjectRaster(spatCoEr);
            SpatialCoherenceDeposition = new ProjectRaster(spatCoDep);

            SpatialCoherence = SpatCo;
        }

        public void Serialize(XmlDocument xmlDoc, XmlNode nodParent)
        {
            // Prior probability always exists, regardless of whether spatial coherence was used.
            nodParent.AppendChild(xmlDoc.CreateElement("PriorProbability")).InnerText = ProjectManager.Project.GetRelativePath(PriorProbability.RasterPath);

            // Remaining rasters only exist if spatial coherence was used.
            if (PosteriorProbability != null)
                nodParent.AppendChild(xmlDoc.CreateElement("PosteriorProbability")).InnerText = ProjectManager.Project.GetRelativePath(PosteriorProbability.RasterPath);

            if (ConditionalRaster != null)
                nodParent.AppendChild(xmlDoc.CreateElement("ConditionalRaster")).InnerText = ProjectManager.Project.GetRelativePath(ConditionalRaster.RasterPath);

            if (SpatialCoherenceErosion != null)
                nodParent.AppendChild(xmlDoc.CreateElement("SpatialCoherenceErosion")).InnerText = ProjectManager.Project.GetRelativePath(SpatialCoherenceErosion.RasterPath);

            if (SpatialCoherenceDeposition != null)
                nodParent.AppendChild(xmlDoc.CreateElement("SpatialCoherenceDeposition")).InnerText = ProjectManager.Project.GetRelativePath(SpatialCoherenceDeposition.RasterPath);

            if (SpatialCoherence != null)
            {
                XmlNode nodSpatCo = nodParent.AppendChild(xmlDoc.CreateElement("SpatialCoherence"));
                nodSpatCo.AppendChild(xmlDoc.CreateElement("WindowSize")).InnerText = SpatialCoherence.MovingWindowDimensions.ToString();
                nodSpatCo.AppendChild(xmlDoc.CreateElement("InflectionA")).InnerText = SpatialCoherence.InflectionA.ToString();
                nodSpatCo.AppendChild(xmlDoc.CreateElement("InflectionB")).InnerText = SpatialCoherence.InflectionB.ToString();
            }
        }

        public static DoDProbabilityRasters Deserialize(XmlNode nodParent)
        {
            FileInfo priorProb = ProjectManager.Project.GetAbsolutePath(nodParent.SelectSingleNode("PriorProbability").InnerText);

            FileInfo postProb = DeserializeRaster(nodParent, "PosteriorProbability");
            FileInfo CondRast = DeserializeRaster(nodParent, "ConditionalRaster");
            FileInfo SpatCoEr = DeserializeRaster(nodParent, "SpatialCoherenceErosion");
            FileInfo SpatCoDe = DeserializeRaster(nodParent, "SpatialCoherenceDeposition");

            CoherenceProperties props = null;
            XmlNode nodSpatCo = nodParent.SelectSingleNode("SpatialCoherence");
            if (nodSpatCo is XmlNode)
            {
                int windowSize = int.Parse(nodSpatCo.SelectSingleNode("WindowSize").InnerText);
                int inflectinA = int.Parse(nodSpatCo.SelectSingleNode("InflectionA").InnerText);
                int inflectinB = int.Parse(nodSpatCo.SelectSingleNode("InflectionB").InnerText);
                props = new CoherenceProperties(windowSize, inflectinA, inflectinB);
            }

            DoDProbabilityRasters result = null;
            if (postProb != null && CondRast != null && SpatCoEr != null && SpatCoDe != null && props != null)
            {
                result = new DoDProbabilityRasters(priorProb, postProb, CondRast, SpatCoEr, SpatCoDe, props);
            }
            else
            {
                result = new DoDProbabilityRasters(priorProb);
            }

            return result;
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
