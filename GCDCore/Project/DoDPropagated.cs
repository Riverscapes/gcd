using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDConsoleLib.GCD;

namespace GCDCore.Project
{
    public class DoDPropagated : DoDBase
    {
        public readonly ProjectRaster PropagatedError;
        public readonly ErrorSurface NewError;
        public readonly ErrorSurface OldError;

        public DoDPropagated(string name, DirectoryInfo folder, DEMSurvey newDEM, DEMSurvey oldDEM,
            ErrorSurface newError, ErrorSurface oldError, FileInfo propErr, DoDStats stats)
            : base(name, folder, newDEM, oldDEM, stats)
        {
            NewError = newError;
            OldError = oldError;
            PropagatedError = new ProjectRaster(propErr);
        }

        public DoDPropagated(DoDBase dod, FileInfo propError, ErrorSurface newError, ErrorSurface oldError)
            : base(dod.Name, dod.Folder, dod.NewDEM, dod.OldDEM,dod.Statistics)
        {
            NewError = newError;
            OldError = oldError;
            PropagatedError = new ProjectRaster(propError);
        }

        new public XmlNode Serialize(XmlDocument xmlDoc, XmlNode nodParent)
        {
            XmlNode nodDoD = base.Serialize(xmlDoc, nodParent);
            XmlNode nodStatistics = nodDoD.SelectSingleNode("Statistics");
            nodDoD.InsertBefore(xmlDoc.CreateElement("PrograpatedError"), nodStatistics).InnerText = ProjectManagerBase.GetRelativePath(PropagatedError.RasterPath);
            nodDoD.InsertBefore(xmlDoc.CreateElement("NewError"), nodStatistics).InnerText = NewError.Name;
            nodDoD.InsertBefore(xmlDoc.CreateElement("OldError"), nodStatistics).InnerText = OldError.Name;
            return nodDoD;
        }

        new public static DoDPropagated Deserialize(XmlNode nodDoD, Dictionary<string, DEMSurvey> dems)
        {
            DoDBase partialDoD = DoDBase.Deserialize(nodDoD, dems);

            ErrorSurface newError = DeserializeError(nodDoD, partialDoD.NewDEM.ErrorSurfaces, "NewError");
            ErrorSurface oldError = DeserializeError(nodDoD, partialDoD.OldDEM.ErrorSurfaces, "OldError");

            FileInfo propErr = null;
            XmlNode nodPropErr = nodDoD.SelectSingleNode("PropagatedError");
            if (nodPropErr != null)
                propErr = ProjectManagerBase.GetAbsolutePath(nodPropErr.InnerText);

            return new DoDPropagated(partialDoD, propErr, newError, oldError);
        }

        private static ErrorSurface DeserializeError(XmlNode nodDoD, Dictionary<string, ErrorSurface> ErrorSurfaces, string ErrorXMLNodeName)
        {
            string errorName = nodDoD.SelectSingleNode(ErrorXMLNodeName).InnerText;
            if (ErrorSurfaces.ContainsKey(errorName))
            {
                return ErrorSurfaces[errorName];
            }
            else
                return null;
        }
    }
}
