using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

namespace GCDCore.Project
{
    public class ErrorSurfaceProperty : GCDProjectItem
    {
        public readonly double? UniformValue;
        public readonly AssocSurface AssociatedSurface;

        public readonly FileInfo FISRuleFile;
        public readonly Dictionary<string, AssocSurface> FISInputs;

        /// <summary>
        /// Return the error raster properties in the format needed by the raster processor
        /// </summary>
        public GCDConsoleLib.GCD.ErrorRasterProperties SingleErrorRasterProperty
        {
            get
            {
                if (UniformValue.HasValue)
                    return new GCDConsoleLib.GCD.ErrorRasterProperties(UniformValue.Value);
                else if (AssociatedSurface is AssocSurface)
                    return new GCDConsoleLib.GCD.ErrorRasterProperties(AssociatedSurface.Raster);
                else
                {
                    Dictionary<string, GCDConsoleLib.Raster> fisinputs = new Dictionary<string, GCDConsoleLib.Raster>();
                    foreach (KeyValuePair<string, AssocSurface> kvp in FISInputs)
                    {
                        fisinputs[kvp.Key] = kvp.Value.Raster;
                    }
                    return new GCDConsoleLib.GCD.ErrorRasterProperties(FISRuleFile, fisinputs);
                }
            }
        }

        public ErrorSurfaceProperty(string name, double uniformValue) : base(name)
        {
            UniformValue = new double?(uniformValue);
        }

        public ErrorSurfaceProperty(string name, AssocSurface assoc) : base(name)
        {
            AssociatedSurface = assoc;
        }

        public ErrorSurfaceProperty(string name, FileInfo fisRuleFile, Dictionary<string, AssocSurface> fisInputs) : base(name)
        {
            FISRuleFile = fisRuleFile;
            FISInputs = fisInputs;
        }

        public void Serialize(XmlDocument xmlDoc, XmlNode nodParent)
        {
            nodParent.AppendChild(xmlDoc.CreateElement("Name")).InnerText = Name;

            if (UniformValue.HasValue)
                nodParent.AppendChild(xmlDoc.CreateElement("UniformValue")).InnerText = UniformValue.ToString();

            if (AssociatedSurface != null)
                nodParent.AppendChild(xmlDoc.CreateElement("AssociatedSurface")).InnerText = AssociatedSurface.Name;

            if (FISRuleFile is FileInfo)
                nodParent.AppendChild(xmlDoc.CreateElement("FISRuleFile")).InnerText = ProjectManager.Project.GetRelativePath(FISRuleFile);

            if (FISInputs != null)
            {
                XmlNode nodInputs = nodParent.AppendChild(xmlDoc.CreateElement("FISInputs"));
                foreach (KeyValuePair<string, AssocSurface> input in FISInputs)
                {
                    XmlNode nodInput = nodInputs.AppendChild(xmlDoc.CreateElement("Input"));
                    nodInput.AppendChild(xmlDoc.CreateElement("Name")).InnerText = input.Key;
                    nodInput.AppendChild(xmlDoc.CreateElement("AssociatedSurface")).InnerText = input.Value.Name;
                }
            }
        }

        public static ErrorSurfaceProperty Deserialize(XmlNode nodProperty, DEMSurvey dem)
        {
            ErrorSurfaceProperty result = null;

            string name = nodProperty.SelectSingleNode("Name").InnerText;

            XmlNode nodUni = nodProperty.SelectSingleNode("UniformValue");
            XmlNode nodAss = nodProperty.SelectSingleNode("AssociatedSurface");
            XmlNode nodFIS = nodProperty.SelectSingleNode("FISRuleFile");

            if (nodUni is XmlNode)
            {
                double value = double.Parse(nodUni.InnerText);
                result = new ErrorSurfaceProperty(name, value);
            }
            else if (nodAss is XmlNode)
            {
                AssocSurface assoc = dem.AssocSurfaces.First<AssocSurface>(x => string.Compare(nodAss.InnerText, x.Name, true) ==0);
                result = new ErrorSurfaceProperty(name, assoc);
            }
            else if (nodFIS is XmlNode)
            {
                FileInfo fisRuleFile = ProjectManager.Project.GetAbsolutePath(nodFIS.SelectSingleNode("FISRuleFile").InnerText);
                Dictionary<string, AssocSurface> inputs = new Dictionary<string, AssocSurface>();

                foreach (XmlNode nodInput in nodFIS.SelectNodes("Inputs/Input"))
                    inputs[nodInput.SelectSingleNode("Name").InnerText] = dem.AssocSurfaces.First<AssocSurface>(x => string.Compare(nodInput.SelectSingleNode("AssociatedSurface").InnerText, x.Name, true) == 0);

                result = new ErrorSurfaceProperty(name, fisRuleFile, inputs);
            }

            return result;
        }
    }
}
