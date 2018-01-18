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

        public DoDPropagated(string name, DirectoryInfo folder, DEMSurvey newDEM, DEMSurvey oldDEM, Raster rawDoD, Raster thrDoD,
         HistogramPair histograms, FileInfo summaryXML, ErrorSurface newError, ErrorSurface oldError, Raster propErr, DoDStats stats)
            : base(name, folder, newDEM, oldDEM, rawDoD, thrDoD, histograms, summaryXML, stats)
        {
            NewError = newError;
            OldError = oldError;
            PropagatedError = propErr;
        }

        public DoDPropagated(DoDBase dod, FileInfo propError, ErrorSurface newError, ErrorSurface oldError)
            : base(dod)
        {
            NewError = newError;
            OldError = oldError;
            PropagatedError = new Raster(propError);
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

        new public static DoDPropagated Deserialize(XmlNode nodDoD, Dictionary<string, DEMSurvey> dems)
        {
            DoDBase partialDoD = DoDBase.Deserialize(nodDoD, dems);

            ErrorSurface newError = DeserializeError(nodDoD, partialDoD.NewDEM.ErrorSurfaces.ToList<ErrorSurface>(), "NewError");
            ErrorSurface oldError = DeserializeError(nodDoD, partialDoD.OldDEM.ErrorSurfaces.ToList<ErrorSurface>(), "OldError");

            FileInfo propErr = null;
            XmlNode nodPropErr = nodDoD.SelectSingleNode("PropagatedError");
            if (nodPropErr != null)
                propErr = ProjectManager.Project.GetAbsolutePath(nodPropErr.InnerText);

            return new DoDPropagated(partialDoD, propErr, newError, oldError);
        }

        private static ErrorSurface DeserializeError(XmlNode nodDoD, List<ErrorSurface> ErrorSurfaces, string ErrorXMLNodeName)
        {
            string errorName = nodDoD.SelectSingleNode(ErrorXMLNodeName).InnerText;
            return ErrorSurfaces.First<ErrorSurface>(x => string.Compare(errorName, x.Name, true) == 0);
        }

        public override void Delete()
        {
            // Delete the propagated error raster
            PropagatedError.Delete();

            base.Delete();
        }
    }
}
