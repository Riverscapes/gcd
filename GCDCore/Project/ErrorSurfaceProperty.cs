using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

namespace GCDCore.Project
{
    public class ErrorSurfaceProperty
    {
        public string Name { get; set; }

        private decimal? _UniformValue;
        public decimal? UniformValue
        {
            get { return _UniformValue; }
            set
            {
                _UniformValue = value;
                _AssociatedSurface = null;
                _FISRuleFile = null;
                FISInputs = null;
            }
        }

        private AssocSurface _AssociatedSurface;
        public AssocSurface AssociatedSurface
        {
            get { return _AssociatedSurface; }
            set
            {
                _AssociatedSurface = value;
                _UniformValue = new decimal?();
                _FISRuleFile = null;
                FISInputs = null;
            }
        }

        private FileInfo _FISRuleFile;
        public FileInfo FISRuleFile
        {
            get { return _FISRuleFile; }
            set
            {
                _FISRuleFile = value;
                FISInputs = new naru.ui.SortableBindingList<FISInput>();
                if (_FISRuleFile is FileInfo)
                {
                    _UniformValue = new decimal?();
                    _AssociatedSurface = null;
                }
            }
        }

        public naru.ui.SortableBindingList<FISInput> FISInputs { get; internal set; }

        public string ErrorType
        {
            get
            {
                if (UniformValue.HasValue)
                    return "Uniform Error";
                else if (AssociatedSurface is AssocSurface)
                    return "Associated Surface";
                else
                    return "FIS Error";
            }
        }

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
                    foreach (FISInput input in FISInputs)
                    {
                        fisinputs[input.FISInputName] = input.AssociatedSurface.Raster;
                    }
                    return new GCDConsoleLib.GCD.ErrorRasterProperties(FISRuleFile, fisinputs);
                }
            }
        }

        /// <summary>
        /// Default constructor needed for binding lists of this class to DataGridView
        /// </summary>
        public ErrorSurfaceProperty()
        {
            _UniformValue = new decimal?(0);
        }

        public ErrorSurfaceProperty(string name)
        {
            Name = name;
            _UniformValue = new decimal?(0);
        }

        public ErrorSurfaceProperty(XmlNode nodProperty, Surface surf)
        {
            ErrorSurfaceProperty errProp = new ErrorSurfaceProperty(nodProperty.SelectSingleNode("Name").InnerText);

            XmlNode nodUni = nodProperty.SelectSingleNode("UniformValue");
            XmlNode nodAss = nodProperty.SelectSingleNode("AssociatedSurface");
            XmlNode nodFIS = nodProperty.SelectSingleNode("FISRuleFile");

            if (nodUni is XmlNode)
            {
                errProp.UniformValue = decimal.Parse(nodUni.InnerText);
            }
            else if (surf is DEMSurvey)
            {
                DEMSurvey dem = ((DEMSurvey)surf);

                if (nodAss is XmlNode)
                {

                    errProp.AssociatedSurface = dem.AssocSurfaces.First<AssocSurface>(x => string.Compare(nodAss.InnerText, x.Name, true) == 0);
                }
                else if (nodFIS is XmlNode)
                {
                    errProp.FISRuleFile = ProjectManager.Project.GetAbsolutePath(nodFIS.InnerText);
                    foreach (XmlNode nodInput in nodProperty.SelectNodes("FISInputs/Input"))
                    {
                        AssocSurface assoc = dem.AssocSurfaces.First<AssocSurface>(x => string.Compare(nodInput.SelectSingleNode("AssociatedSurface").InnerText, x.Name, true) == 0);
                        errProp.FISInputs.Add(new FISInput(nodInput.SelectSingleNode("Name").InnerText, assoc));
                    }
                }
            }
        }

        public void Serialize(XmlNode nodParent)
        {
            nodParent.AppendChild(nodParent.OwnerDocument.CreateElement("Name")).InnerText = Name;

            if (UniformValue.HasValue)
                nodParent.AppendChild(nodParent.OwnerDocument.CreateElement("UniformValue")).InnerText = UniformValue.Value.ToString();

            if (AssociatedSurface != null)
                nodParent.AppendChild(nodParent.OwnerDocument.CreateElement("AssociatedSurface")).InnerText = AssociatedSurface.Name;

            if (FISRuleFile is FileInfo)
                nodParent.AppendChild(nodParent.OwnerDocument.CreateElement("FISRuleFile")).InnerText = ProjectManager.Project.GetRelativePath(FISRuleFile);

            if (FISInputs != null)
            {
                XmlNode nodInputs = nodParent.AppendChild(nodParent.OwnerDocument.CreateElement("FISInputs"));
                foreach (FISInput input in FISInputs)
                {
                    XmlNode nodInput = nodInputs.AppendChild(nodParent.OwnerDocument.CreateElement("Input"));
                    nodInput.AppendChild(nodParent.OwnerDocument.CreateElement("Name")).InnerText = input.FISInputName;
                    nodInput.AppendChild(nodParent.OwnerDocument.CreateElement("AssociatedSurface")).InnerText = input.AssociatedSurface.Name;
                }
            }
        }
    }
}
