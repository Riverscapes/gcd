using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;
using System.ComponentModel;
using System.Xml;

namespace GCDCore.Project.Morphological
{
    public class MorphologicalAnalysis
    {
        public string Name { get; set; }
        public readonly BudgetSegregation BS;
        public readonly DirectoryInfo OutputFolder;

        public int ZeroFluxCell { get; set; }

        public readonly BindingList<MorphologicalUnit> Units;

        public MorphologicalAnalysis(string name, DirectoryInfo outputFolder, BudgetSegregation bs)
        {
            Name = name;
            OutputFolder = outputFolder;
            BS = bs;

            Units = new BindingList<MorphologicalUnit>();
        }

        private decimal _porosity;
        public decimal Porosity
        {
            get { return _porosity; }
            set
            {
                if (_porosity == value)
                    return;

                _porosity = value;
                CalculateWork();
            }
        }

        private decimal _density;
        public decimal Density
        {
            get
            {
                return _density;
            }

            set
            {
                if (_density == value)
                    return;

                _density = value;
                CalculateWork();
            }
        }

        private UnitsNet.Duration _duration;
        public UnitsNet.Duration Duration
        {
            get { return _duration; }
            set
            {
                if (_duration == value)
                    return;

                _duration = value;
                CalculateWork();
            }

        }

        private decimal _competency;
        public decimal Competency
        {
            get { return _competency; }
            set
            {
                if (_competency == value)
                    return;

                _competency = value;
                CalculateWork();
            }
        }

        private void CalculateWork()
        {

            MorphologicalUnit MaxDepsotionUnit = null;
            UnitsNet.Volume MaxDeposition = new Volume(0);

            UnitsNet.Volume cumulativeVol = new Volume(0);
            foreach (MorphologicalUnit unit in Units)
            {
                cumulativeVol += (unit.VolIn - unit.VolOut);

                if (unit.VolOut > new Volume(0) && unit.VolOut > MaxDeposition)
                {
                    MaxDepsotionUnit = unit;
                    MaxDeposition = unit.VolOut;
                }
            }

            Units.ResetBindings();
        }

        public void Serialize(XmlNode nodParent)
        {
            XmlNode nodDEM = nodParent.AppendChild(nodParent.OwnerDocument.CreateElement("MorphologicalAnalysis"));
            nodDEM.AppendChild(nodParent.OwnerDocument.CreateElement("Name")).InnerText = Name;
            nodDEM.AppendChild(nodParent.OwnerDocument.CreateElement("Folder")).InnerText = ProjectManager.Project.GetRelativePath(OutputFolder.FullName);
            nodDEM.AppendChild(nodParent.OwnerDocument.CreateElement("DoD")).InnerText = BS.DoD.Name;
            nodDEM.AppendChild(nodParent.OwnerDocument.CreateElement("BudgetSegregation")).InnerText = BS.Name;
            nodDEM.AppendChild(nodParent.OwnerDocument.CreateElement("Porosity")).InnerText = Porosity.ToString();
            nodDEM.AppendChild(nodParent.OwnerDocument.CreateElement("Density")).InnerText = Density.ToString();

            XmlNode nodDuration = nodParent.OwnerDocument.CreateElement("Duration");
            nodDuration.InnerText = Duration.ToString();
            nodDuration.Attributes.Append(nodParent.OwnerDocument.CreateAttribute("units")).InnerText = Duration.un


        }
    }
}
