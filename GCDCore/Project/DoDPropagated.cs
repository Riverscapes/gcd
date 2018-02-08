using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using GCDConsoleLib;
using GCDConsoleLib.GCD;

namespace GCDCore.Project
{
    public class DoDPropagated : DoDBase
    {
        public readonly Raster PropagatedError;
        public readonly ErrorSurface NewError;
        public readonly ErrorSurface OldError;

        public override string UncertaintyAnalysisLabel
        {
            get
            {
                return "Propagated Error";
            }
        }

        public DoDPropagated(string name, DirectoryInfo folder, DEMSurvey newDEM, DEMSurvey oldDEM, Raster rawDoD, Raster thrDoD,
         HistogramPair histograms, FileInfo summaryXML, ErrorSurface newError, ErrorSurface oldError, Raster propErr, DoDStats stats)
            : base(name, folder, newDEM, oldDEM, rawDoD, thrDoD, histograms, summaryXML, stats)
        {
            NewError = newError;
            OldError = oldError;
            PropagatedError = propErr;
        }

        public DoDPropagated(XmlNode nodDoD, Dictionary<string, DEMSurvey> dems)
            : base(nodDoD, dems)
        {
            NewError = DeserializeError(nodDoD, NewDEM.ErrorSurfaces.ToList<ErrorSurface>(), "NewError");
            OldError = DeserializeError(nodDoD, OldDEM.ErrorSurfaces.ToList<ErrorSurface>(), "OldError");

            XmlNode nodPropErr = nodDoD.SelectSingleNode("PropagatedError");
            if (nodPropErr != null)
                PropagatedError = new Raster(ProjectManager.Project.GetAbsolutePath(nodPropErr.InnerText));
        }

        public override XmlNode Serialize(XmlDocument xmlDoc, XmlNode nodParent)
        {
            XmlNode nodDoD = base.Serialize(xmlDoc, nodParent);
            XmlNode nodStatistics = nodDoD.SelectSingleNode("Statistics");
            nodDoD.InsertBefore(xmlDoc.CreateElement("PropagatedError"), nodStatistics).InnerText = ProjectManager.Project.GetRelativePath(PropagatedError.GISFileInfo);
            nodDoD.InsertBefore(xmlDoc.CreateElement("NewError"), nodStatistics).InnerText = NewError.Name;
            nodDoD.InsertBefore(xmlDoc.CreateElement("OldError"), nodStatistics).InnerText = OldError.Name;
            return nodDoD;
        }   

        private static ErrorSurface DeserializeError(XmlNode nodDoD, List<ErrorSurface> ErrorSurfaces, string ErrorXMLNodeName)
        {
            string errorName = nodDoD.SelectSingleNode(ErrorXMLNodeName).InnerText;
            return ErrorSurfaces.First<ErrorSurface>(x => string.Compare(errorName, x.Name, true) == 0);
        }

        public override void Delete()
        {
            DeleteRaster(PropagatedError);
            base.Delete();
        }

        /// <summary>
        /// Delete non-ProjectRasterItem rasters (i.e. prop err and other probabilistic rasters)
        /// </summary>
        /// <param name="r"></param>
        /// <remarks>
        /// 1. Ensure the rasters is out of the map
        /// 2. Delete the raster
        /// </remarks>
        protected void DeleteRaster(Raster r)
        {
            ProjectManager.OnGISLayerDelete(new ProjectManager.GISLayerEventArgs(r.GISFileInfo));
            r.Delete();
        }
    }
}
