using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnitsNet;
using System.ComponentModel;
using System.Xml;
using GCDCore.Engines;

namespace GCDCore.Project.Morphological
{
    public class MorphologicalAnalysis : GCDProjectItem
    {
        public readonly BudgetSegregation BS;
        public readonly DirectoryInfo OutputFolder;
        public readonly FileInfo Spreadsheet;

        public UnitsNet.Units.DurationUnit DurationDisplayUnits { get; set; }

        public MorphologicalUnit MinimumFluxCell { get; set; }
        public UnitsNet.Volume MinimumFlux { get; set; }

        public readonly BindingList<MorphologicalUnit> Units;

        public override bool IsItemInUse { get { return false; } }

        public override string Noun { get { return "Morphological Analysis"; } }


        public MorphologicalAnalysis(string name, DirectoryInfo outputFolder, BudgetSegregation bs, UnitsNet.Units.VolumeUnit eVolumeUnits)
            : base(name)
        {
            OutputFolder = outputFolder;
            Spreadsheet = new FileInfo(Path.Combine(OutputFolder.FullName, "Morphological.xml"));
            BS = bs;
            DurationDisplayUnits = UnitsNet.Units.DurationUnit.Hour;
            _DisplayVolumeUnits = ProjectManager.Project.Units.VolUnit;
            _duration = UnitsNet.Duration.From(1, DurationDisplayUnits);
            _porosity = 0.26m;
            _density = 2.65m;
            _competency = 1m;
            _DisplayVolumeUnits = eVolumeUnits;

            Units = new BindingList<MorphologicalUnit>();
            InitializeMorphologicalUnits();
        }

        public MorphologicalAnalysis(XmlNode nodAnalysis, BudgetSegregation bs)
            : base(nodAnalysis)
        {
            OutputFolder = ProjectManager.Project.GetAbsoluteDir(nodAnalysis.SelectSingleNode("Folder").InnerText);
            Spreadsheet = ProjectManager.Project.GetAbsolutePath(nodAnalysis.SelectSingleNode("Spreadsheet").InnerText);

            BS = bs;

            XmlNode nodDuration = nodAnalysis.SelectSingleNode("Duration");
            DurationDisplayUnits = (UnitsNet.Units.DurationUnit)Enum.Parse(typeof(UnitsNet.Units.DurationUnit), nodDuration.Attributes["units"].InnerText);
            _DisplayVolumeUnits = ProjectManager.Project.Units.VolUnit;

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

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            // Uncomment next line to clear the morph unit data and load the debug data
            //LoadFeshieData();
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            Units[0].VolIn = Units[0].VolChange;
            Units[0].VolOut = (-1 * Units[0].VolChange) + Units[0].VolIn;
            Units[0].CumulativeVolume = Units[0].VolChange;

            for (int i = 1; i < Units.Count; i++)
            {
                Units[i].CumulativeVolume = Units[i].VolChange + Units[i - 1].CumulativeVolume;

                Units[i].VolIn = Units[i - 1].VolOut;
                Units[i].VolOut = Units[i].VolIn - Units[i].VolChange;

                // Track the first unit that possesses a positive volume change
                if (MinimumFluxCell == null && Units[i].VolChange > new Volume(0))
                {
                    MinimumFluxCell = Units[i];
                    MinimumFlux = UnitsNet.Volume.From(Math.Abs(Units[i].VolOut.As(UnitsNet.Units.VolumeUnit.CubicMeter)), UnitsNet.Units.VolumeUnit.CubicMeter);
                }
            }

            // Add back in the VolOut for the minimum flux cell (or the whole reach if there was no min flux cell)
            if (MinimumFluxCell == null)
                MinimumFlux = Units.Last().VolOut;

            Units[0].VolIn = -1 * Units[0].VolChange + MinimumFlux;

            Units[0].VolOut = Units[0].VolIn + Units[0].VolChange;

            // Recalculate the VolOut for each unit now we know the reach input flux
            for (int i = 1; i < Units.Count; i++)
            {
                Units[i].VolIn = Units[i - 1].VolOut;
                Units[i].VolOut = Units[i].VolIn - Units[i].VolChange;
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
                    decimal volumeFluxPerUnitVolume = (decimal)UnitsNet.Volume.From((double)unit.FluxVolume, DisplayVolumeUnits).As(UnitsNet.Units.VolumeUnit.CubicMeter);

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
            nodMA.AppendChild(nodParent.OwnerDocument.CreateElement("Spreadsheet")).InnerText = ProjectManager.Project.GetRelativePath(Spreadsheet);

            XmlNode nodDuration = nodMA.AppendChild(nodParent.OwnerDocument.CreateElement("Duration"));
            nodDuration.InnerText = Duration.As(DurationDisplayUnits).ToString("R");
            nodDuration.Attributes.Append(nodParent.OwnerDocument.CreateAttribute("units")).InnerText = DurationDisplayUnits.ToString();

            XmlNode nodMinFluxCell = nodMA.AppendChild(nodParent.OwnerDocument.CreateElement("MinimumFluxUnit"));
            if (MinimumFluxCell != null)
                nodMinFluxCell.InnerText = MinimumFluxCell.Name;

            XmlNode nodMinFlux = nodMA.AppendChild(nodParent.OwnerDocument.CreateElement("MinimumFluxVolume"));
            nodMinFlux.InnerText = MinimumFlux.As(ProjectManager.Project.Units.VolUnit).ToString("R");
        }

        public override void Delete()
        {
            throw new NotImplementedException("deleting morphological analysis is not implemented.");
        }

        public void SaveExcelSpreadsheet()
        {
            //get template and throw error if it doesnt exists
            FileInfo template = new FileInfo(Path.Combine(ProjectManager.ExcelTemplatesFolder.FullName, "Morphological.xml"));
            if (!template.Exists)
            {
                throw new Exception("The GCD morphological approach spreadsheet template cannot be found at " + template.FullName);
            }

            // Verify if file already exists and if so, can it be deleted and replaced
            if (Spreadsheet.Exists)
            {
                // Will throw exception if locked
                IsFileLocked(Spreadsheet);

                Spreadsheet.Delete();
            }

            //setup ExcelXMLDocument which does the heavy lifting of updating the XML
            ExcelXMLDocument xmlExcelDoc = new ExcelXMLDocument(template.FullName);

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // TODO: write morphological spreadsheet here

            foreach (MorphologicalUnit unit in Units)
            {
                // TODO: write values to spreadsheet
            }

            //save output
            xmlExcelDoc.Save(Spreadsheet.FullName);

        }

        private bool IsFileLocked(FileInfo fiPath)
        {
            if (!fiPath.Exists)
                return false;

            List<System.Diagnostics.Process> processes = naru.os.FileUtil.WhoIsLocking(fiPath.FullName);
            if (processes.Count > 0)
            {
                IOException ex = new IOException("The file is in use by another process");
                ex.Data["Processes"] = string.Join(",", processes.Select(x => x.ProcessName).ToArray());
                throw ex;
            }

            return false;
        }

        /// <summary>
        /// Debug information for verifying the morphological results
        /// </summary>
        private void LoadFeshieData()
        {
            Units.Clear();

            for (int i = 0; i < 9; i++)
                Units.Add(new MorphologicalUnit(i.ToString()));

            Units[0].VolErosion = new UnitsNet.Volume(8495.756690979);
            Units[0].VolErosionErr = new UnitsNet.Volume(2277.76617275923);
            Units[0].VolDeposition = new UnitsNet.Volume(8462.97969818115);
            Units[0].VolDepositionErr = new UnitsNet.Volume(2040.09397353604);

            Units[1].VolErosion = new UnitsNet.Volume(13145.7649154663);
            Units[1].VolErosionErr = new UnitsNet.Volume(3363.22726191208);
            Units[1].VolDeposition = new UnitsNet.Volume(14018.8771057129);
            Units[1].VolDepositionErr = new UnitsNet.Volume(3323.17051564902);

            Units[2].VolErosion = new UnitsNet.Volume(10800.1524581909);
            Units[2].VolErosionErr = new UnitsNet.Volume(3358.31976071373);
            Units[2].VolDeposition = new UnitsNet.Volume(9090.47595977783);
            Units[2].VolDepositionErr = new UnitsNet.Volume(2271.5536907278);

            Units[3].VolErosion = new UnitsNet.Volume(17021.1651306152);
            Units[3].VolErosionErr = new UnitsNet.Volume(4407.54704015329);
            Units[3].VolDeposition = new UnitsNet.Volume(9055.04878997803);
            Units[3].VolDepositionErr = new UnitsNet.Volume(2638.86314678937);

            Units[4].VolErosion = new UnitsNet.Volume(16371.3544235229);
            Units[4].VolErosionErr = new UnitsNet.Volume(4382.48134100437);
            Units[4].VolDeposition = new UnitsNet.Volume(9054.64354705811);
            Units[4].VolDepositionErr = new UnitsNet.Volume(2556.30508193001);

            Units[5].VolErosion = new UnitsNet.Volume(15129.2975006104);
            Units[5].VolErosionErr = new UnitsNet.Volume(3913.95101860911);
            Units[5].VolDeposition = new UnitsNet.Volume(5790.98097229004);
            Units[5].VolDepositionErr = new UnitsNet.Volume(1933.64249853417);

            Units[6].VolErosion = new UnitsNet.Volume(11400.3183135986);
            Units[6].VolErosionErr = new UnitsNet.Volume(2502.36957178637);
            Units[6].VolDeposition = new UnitsNet.Volume(4151.42811584473);
            Units[6].VolDepositionErr = new UnitsNet.Volume(1316.84695769101);

            Units[7].VolErosion = new UnitsNet.Volume(7477.41519927979);
            Units[7].VolErosionErr = new UnitsNet.Volume(2253.93063607067);
            Units[7].VolDeposition = new UnitsNet.Volume(3493.41770172119);
            Units[7].VolDepositionErr = new UnitsNet.Volume(1302.82937397808);

            Units[8].VolErosion = new UnitsNet.Volume(8546.26876068115);
            Units[8].VolErosionErr = new UnitsNet.Volume(2326.89401529357);
            Units[8].VolDeposition = new UnitsNet.Volume(8882.98578643799);
            Units[8].VolDepositionErr = new UnitsNet.Volume(2608.83717241883);
        }
    }
}
