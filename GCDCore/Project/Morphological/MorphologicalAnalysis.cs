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

        public MorphologicalUnit MinimumFluxCell { get; set; }
        public UnitsNet.Volume MinimumFlux { get; set; }

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

            foreach (KeyValuePair<int, Tuple<string, string>> maskValue in ((GCDCore.Project.Masks.DirectionalMask)bs.Mask).SortedFieldValues)
            {
                if (bs.Classes.ContainsKey(maskValue.Value.Item1))
                {
                    BudgetSegregationClass bsc = bs.Classes[maskValue.Value.Item1];

                    MorphologicalUnit mu = new MorphologicalUnit(bsc.Name);
                    mu.VolErosion = bsc.Statistics.ErosionThr.GetVolume(ProjectManager.Project.CellArea, ProjectManager.Project.Units.VertUnit);
                    mu.VolErosionErr = bsc.Statistics.ErosionErr.GetVolume(ProjectManager.Project.CellArea, ProjectManager.Project.Units.VertUnit);

                    mu.VolDeposition = bsc.Statistics.DepositionThr.GetVolume(ProjectManager.Project.CellArea, ProjectManager.Project.Units.VertUnit);
                    mu.VolDepositionErr = bsc.Statistics.DepositionErr.GetVolume(ProjectManager.Project.CellArea, ProjectManager.Project.Units.VertUnit);

                    Units.Add(mu);
                }
            }



            Units[0].VolIn = Units[0].VolChange;
            Units[0].VolOut = (-1 * Units[0].VolChange) + Units[0].VolIn;
            Units[0].CumulativeVolume = Units[0].VolChange;

            for (int i = 1; i < Units.Count; i++)
            {
                Units[i].CumulativeVolume = Units[i].VolChange + Units[i - 1].CumulativeVolume;

                Units[i].VolIn = Units[i - 1].VolOut;
                Units[i].VolOut = Units[i].VolIn - Units[i].VolChange;

                // Track the first unit that possesses a positive exit volume
                if (Units[i].VolOut > new Volume(0))
                {
                    MinimumFluxCell = Units[i];
                    MinimumFlux = Units[i].VolIn;
                }
            }

            // Total row
            MorphologicalUnit muTotal = new MorphologicalUnit("Reach Total", true);
            muTotal.VolIn = Units[0].VolIn;
            muTotal.VolOut = Units[Units.Count - 1].VolOut;
            muTotal.CumulativeVolume = Units[Units.Count - 1].CumulativeVolume;

            foreach(MorphologicalUnit unit in Units)
            {
                muTotal.VolErosion += unit.VolErosion;
                muTotal.VolErosionErr += unit.VolErosionErr;
                muTotal.VolDeposition += unit.VolDeposition;
                muTotal.VolDepositionErr += unit.VolDepositionErr;
            }
            Units.Add(muTotal);

            // Add back in the VolIn to the start of the reach to make the output of first positve
            //Units[0].VolIn = muPos.VolIn + Units[0].VolIn;
            //Units[0].VolOut = (-1 * Units[0].VolChange) + Units[0].VolIn;

            //for (int i = 1; i < Units.Count; i++)
            //{
            //    Units[i].VolIn = Units[i - 1].VolOut;
            //    Units[i].VolOut = Units[i].VolIn - Units[i].VolChange;
            //}






            //// The volume entering the first unit is the volume change of that first unit
            //// plus the volume exiting the unit with the first positive volume out
            //Units[0].VolIn = Units[0].VolChange + muPos.VolOut;

            //// All remaining units should have their volumes in and out adjusted
            //for (int i = 1; i < Units.Count; i++)
            //    Units[i].VolIn = Units[i - 1].VolOut;

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
