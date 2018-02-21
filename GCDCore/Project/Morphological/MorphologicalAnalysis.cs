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

        public MorphologicalAnalysis(string name, DirectoryInfo outputFolder, BudgetSegregation bs, UnitsNet.Units.VolumeUnit eVolumeUnits)
        {
            Name = name;
            OutputFolder = outputFolder;
            BS = bs;
            DurationDisplayUnits = UnitsNet.Units.DurationUnit.Hour;
            _duration = UnitsNet.Duration.From(1, DurationDisplayUnits);
            _porosity = 0.26m;
            _density = 2.65m;
            _competency = 1m;
            _DisplayVolumeUnits = eVolumeUnits;

            Units = new BindingList<MorphologicalUnit>();
            InitializeMorphologicalUnits();
        }

        public MorphologicalAnalysis(XmlNode nodAnalysis, BudgetSegregation bs)
        {
            Name = nodAnalysis.SelectSingleNode("Name").InnerText;
            OutputFolder = ProjectManager.Project.GetAbsoluteDir(nodAnalysis.SelectSingleNode("Folder").InnerText);
            BS = bs;

            XmlNode nodDuration = nodAnalysis.SelectSingleNode("Duration");
            DurationDisplayUnits = (UnitsNet.Units.DurationUnit)Enum.Parse(typeof(UnitsNet.Units.DurationUnit), nodDuration.Attributes["units"].InnerText);
            _duration = UnitsNet.Duration.From(double.Parse(nodDuration.InnerText), DurationDisplayUnits);
            _porosity = decimal.Parse(nodAnalysis.SelectSingleNode("Porosity").InnerText);
            _density = decimal.Parse(nodAnalysis.SelectSingleNode("Density").InnerText);
            _competency = decimal.Parse(nodAnalysis.SelectSingleNode("Competency").InnerText);

            double minFluxValue = double.Parse(nodAnalysis.SelectSingleNode("MinimumFluxVolume").InnerText);
            MinimumFlux = UnitsNet.Volume.From(minFluxValue, ProjectManager.Project.Units.VolUnit);

            Units = new BindingList<MorphologicalUnit>();
            InitializeMorphologicalUnits();
        }

        private void InitializeMorphologicalUnits()
        {
            // Loop over the sorted list of mask values IN ASCENDING DIRECTIONAL ORDER
            foreach (KeyValuePair<int, Tuple<string, string>> maskValue in ((GCDCore.Project.Masks.DirectionalMask)BS.Mask).SortedFieldValues)
            {
                // Only able to create a morphological unit if this mask item appears in budget seg
                if (BS.Classes.ContainsKey(maskValue.Value.Item1))
                {
                    BudgetSegregationClass bsc = BS.Classes[maskValue.Value.Item1];

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

            foreach (MorphologicalUnit unit in Units)
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

        private UnitsNet.Units.VolumeUnit _DisplayVolumeUnits;
        public UnitsNet.Units.VolumeUnit DisplayVolumeUnits
        {
            get
            {
                return _DisplayVolumeUnits;
            }

            set
            {
                if (_DisplayVolumeUnits != value)
                {
                    _DisplayVolumeUnits = value;
                    CalculateWork();
                }
            }
        }

        public UnitsNet.Duration CompetentDuration { get { return UnitsNet.Duration.From(this.Duration.As(DurationDisplayUnits) * (double)Competency, DurationDisplayUnits); } }

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

        public void CalculateWork()
        {
            decimal duration = (decimal)CompetentDuration.As(DurationDisplayUnits);

            UnitsNet.Density density = UnitsNet.Density.From((double)Density, UnitsNet.Units.DensityUnit.GramPerCubicCentimeter);
            decimal massPerUnitVolume = (decimal)density.As(UnitsNet.Units.DensityUnit.KilogramPerCubicMeter);

            if (duration > 0)
            {
                foreach (MorphologicalUnit unit in Units)
                {
                    // The volume flux per unit time. THIS IS IN DISPLAY VOLUME UNITS
                    unit.FluxVolume = (1m - Porosity) * (decimal)unit.VolOut.As(DisplayVolumeUnits) / duration;

                    // The volume flux per unit volume and per unit time. THIS IS IN CUBIC METRES
                    decimal volumeFluxPerUnitVolume = (decimal) UnitsNet.Volume.From((double) unit.FluxVolume, DisplayVolumeUnits).As(UnitsNet.Units.VolumeUnit.CubicMeter);

                    // Mass of material per unit time. (Should be independent of volume)
                    unit.FluxMass = volumeFluxPerUnitVolume * massPerUnitVolume;
                }
            }
            else
            {
                Units.ToList<MorphologicalUnit>().ForEach(x => x.FluxVolume = 0m);
            }
            Units.ResetBindings();
        }

        public void Serialize(XmlNode nodParent)
        {
            XmlNode nodMA = nodParent.AppendChild(nodParent.OwnerDocument.CreateElement("MorphologicalAnalysis"));
            nodMA.AppendChild(nodParent.OwnerDocument.CreateElement("Name")).InnerText = Name;
            nodMA.AppendChild(nodParent.OwnerDocument.CreateElement("Folder")).InnerText = ProjectManager.Project.GetRelativePath(OutputFolder.FullName);
            nodMA.AppendChild(nodParent.OwnerDocument.CreateElement("Porosity")).InnerText = Porosity.ToString();
            nodMA.AppendChild(nodParent.OwnerDocument.CreateElement("Density")).InnerText = Density.ToString();
            nodMA.AppendChild(nodParent.OwnerDocument.CreateElement("Competency")).InnerText = Competency.ToString();

            XmlNode nodDuration = nodMA.AppendChild(nodParent.OwnerDocument.CreateElement("Duration"));
            nodDuration.InnerText = Duration.As(DurationDisplayUnits).ToString("R");
            nodDuration.Attributes.Append(nodParent.OwnerDocument.CreateAttribute("units")).InnerText = DurationDisplayUnits.ToString();

            nodMA.AppendChild(nodParent.OwnerDocument.CreateElement("MinimumFluxUnit")).InnerText = MinimumFluxCell.Name;

            XmlNode nodMinFlux = nodMA.AppendChild(nodParent.OwnerDocument.CreateElement("MinimumFluxVolume"));
            nodMinFlux.InnerText = MinimumFlux.As(ProjectManager.Project.Units.VolUnit).ToString("R");
        }

        public void Delete()
        {
            throw new NotImplementedException("deleting morphological analysis is not implemented.");
        }
    }
}
