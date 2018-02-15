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

        public UnitsNet.Units.DurationUnit DurationDisplayUnits { get; set; }

        public int ZeroFluxCell { get; set; }

        public readonly BindingList<MorphologicalUnit> Units;

        public MorphologicalAnalysis(string name, DirectoryInfo outputFolder, BudgetSegregation bs)
        {
            Name = name;
            OutputFolder = outputFolder;
            BS = bs;
            DurationDisplayUnits = UnitsNet.Units.DurationUnit.Hour;
            _duration = UnitsNet.Duration.From(1, DurationDisplayUnits);
            _porosity = 0.26m;
            _density = 2.5m;
            _competency = 1m;

            Units = new BindingList<MorphologicalUnit>();

            MorphologicalUnit muPos = null;
            foreach (BudgetSegregationClass bsc in bs.Classes.Values)
            {
                MorphologicalUnit mu = new MorphologicalUnit(bsc.Name);
                mu.VolErosion = bsc.Statistics.ErosionThr.GetVolume(ProjectManager.Project.CellArea, ProjectManager.Project.Units.VertUnit);
                mu.VolErsionErr = bsc.Statistics.ErosionErr.GetVolume(ProjectManager.Project.CellArea, ProjectManager.Project.Units.VertUnit);

                mu.VolDeposition = bsc.Statistics.DepositionThr.GetVolume(ProjectManager.Project.CellArea, ProjectManager.Project.Units.VertUnit);
                mu.VolDepositionErr = bsc.Statistics.DepositionErr.GetVolume(ProjectManager.Project.CellArea, ProjectManager.Project.Units.VertUnit);

                // Track the first unit that possesses a positive exit volume
                if (mu.VolOut > new Volume(0))
                    muPos = mu;

                Units.Add(mu);
            }

            // The volume entering the first unit is the volume change of that first unit
            // plus the volume exiting the unit with the first positive volume out
            Units[0].VolIn = Units[0].VolChange + muPos.VolOut;

            // All remaining units should have their volumes in and out adjusted
            for (int i = 1; i < Units.Count; i++)
                Units[i].VolIn = Units[i - 1].VolOut;

            // Calculate the work performed in each cell now that the values are adjusted
            CalculateWork();
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
            nodParent.AppendChild(nodParent.OwnerDocument.CreateElement("Competency")).InnerText = Competency.ToString();

            XmlNode nodDuration = nodParent.OwnerDocument.CreateElement("Duration");
            nodDuration.InnerText = Duration.ToString();
            // nodDuration.Attributes.Append(nodParent.OwnerDocument.CreateAttribute("units")).InnerText = Duration.GetAbbreviation(_duration);
        }
    }
}
