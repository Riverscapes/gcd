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
        public enum FluxDirection
        {
            Input,
            Output
        };

        public readonly BudgetSegregation BS;
        public readonly DirectoryInfo OutputFolder;
        public readonly FileInfo Spreadsheet;

        public Volume ReachInputFlux { get; internal set; }
        public Volume BoundaryFlux { get; internal set; }
        public MorphologicalUnit BoundaryFluxUnit { get; internal set; }
        public FluxDirection BoundaryFluxDirection { get; internal set; }

        public readonly BindingList<MorphologicalUnit> Units;

        public override bool IsItemInUse { get { return false; } }

        public override string Noun { get { return "Morphological Analysis"; } }

        public MorphologicalAnalysis(string name, DirectoryInfo outputFolder, BudgetSegregation bs)
            : base(name)
        {
            OutputFolder = outputFolder;
            Spreadsheet = new FileInfo(Path.Combine(OutputFolder.FullName, "Morphological.xml"));
            BS = bs;

            DisplayUnits_Duration = UnitsNet.Units.DurationUnit.Hour;
            DisplayUnits_Volume = ProjectManager.Project.Units.VolUnit;
            DisplayUnits_Mass = UnitsNet.Units.MassUnit.Kilogram;

            _duration = Duration.From(1, DisplayUnits_Duration);
            _porosity = 0.26m;
            _density = 2.65m;
            _competency = 1m;
            //_DataVolumeUnits = ProjectManager.Project.Units.VolUnit;

            Units = new BindingList<MorphologicalUnit>();
            LoadMorphologicalUnitData();
            ImposeMinimumFlux();
        }

        public MorphologicalAnalysis(XmlNode nodAnalysis, BudgetSegregation bs)
            : base(nodAnalysis)
        {
            OutputFolder = ProjectManager.Project.GetAbsoluteDir(nodAnalysis.SelectSingleNode("Folder").InnerText);
            Spreadsheet = ProjectManager.Project.GetAbsolutePath(nodAnalysis.SelectSingleNode("Spreadsheet").InnerText);
            BS = bs;

            XmlNode nodDuration = nodAnalysis.SelectSingleNode("Duration");
            _DisplayUnits_Duration = (UnitsNet.Units.DurationUnit)Enum.Parse(typeof(UnitsNet.Units.DurationUnit), nodDuration.Attributes["units"].InnerText);
            _DisplayUnits_Volume = ProjectManager.Project.Units.VolUnit;
            _DisplayUnits_Mass = UnitsNet.Units.MassUnit.Kilogram;

            _duration = Duration.From(double.Parse(nodDuration.InnerText), DisplayUnits_Duration);
            _porosity = decimal.Parse(nodAnalysis.SelectSingleNode("Porosity").InnerText);
            _density = decimal.Parse(nodAnalysis.SelectSingleNode("Density").InnerText);
            _competency = decimal.Parse(nodAnalysis.SelectSingleNode("Competency").InnerText);
            //_DataVolumeUnits = ProjectManager.Project.Units.VolUnit;

            double minFluxValue = double.Parse(nodAnalysis.SelectSingleNode("MinimumFluxVolume").InnerText);
            BoundaryFlux = Volume.From(minFluxValue, ProjectManager.Project.Units.VolUnit);

            Units = new BindingList<MorphologicalUnit>();
            LoadMorphologicalUnitData();
        }

        public void LoadMorphologicalUnitData()
        {
            Units.Clear();

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
//#if DEBUG
//            LoadFeshieData();
//#endif
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            // Add the total row
            Units.Add(new MorphologicalUnit("Reach Total", true));
        }

        public void ImposeMinimumFlux()
        {
            // Make sure that the reach is reset with zero boundary condition flux
            ResetFluxes();

            MorphologicalUnit minFluxUnit = null;

            // Find the first unit with positive volume change
            if (Units.Where(x => !x.IsTotal).Any(x => x.VolChange >= new Volume(0)))
            {
                minFluxUnit = Units.Where(x => !x.IsTotal).First(x => x.VolChange >= new Volume(0));
            }
            else
            {
                // No positive change, so find the least negative change
                minFluxUnit = Units.Where(x => !x.IsTotal).OrderByDescending(x => x.VolChange).First();
            }

            // Update the reach fluxes with the min flux volume
            UpdateFluxes(FluxDirection.Output, minFluxUnit, new Volume(0));
        }

        public void ImposeBoundaryCondition(FluxDirection eDir, MorphologicalUnit unit, Volume boundaryVol)
        {
            // Make sure to remove any existing boundary condition flux
            ResetFluxes();

            // Update all units with the new flux as the reach input
            UpdateFluxes(eDir, unit, boundaryVol);
        }

        /// <summary>
        /// Reset the reach input flux to zero and calculate all the downstream unit vol in and out
        /// </summary>
        private void ResetFluxes()
        {
            Units[0].VolIn = Units[0].VolChange;
            Units[0].VolOut = (-1 * Units[0].VolChange) + Units[0].VolIn;

            for (int i = 1; i < Units.Count; i++)
            {
                Units[i].VolIn = Units[i - 1].VolOut;
                Units[i].VolOut = Units[i].VolIn - Units[i].VolChange;
            }
        }

        private void UpdateFluxes(FluxDirection eDir, MorphologicalUnit boundaryUnit, Volume boundaryFlux)
        {
            BoundaryFluxDirection = eDir;
            BoundaryFluxUnit = boundaryUnit;
            BoundaryFlux = boundaryFlux;
            ReachInputFlux = boundaryFlux - (eDir == FluxDirection.Input ? boundaryUnit.VolIn : boundaryUnit.VolOut);

            // Add back in the VolOut for the minimum flux cell (or the whole reach if there was no min flux cell)
            Units[0].VolIn = -1 * Units[0].VolChange + ReachInputFlux;
            Units[0].VolOut = Units[0].VolIn + Units[0].VolChange;
            Units[0].CumulativeVolume = Units[0].VolChange;

            // Recalculate the VolOut for each downstream unit now we know the reach input flux
            for (int i = 1; i < Units.Count; i++)
            {
                Units[i].VolIn = Units[i - 1].VolOut;
                Units[i].VolOut = Units[i].VolIn - Units[i].VolChange;
                Units[i].CumulativeVolume = Units[i - 1].CumulativeVolume + Units[i].VolChange;
            }

            // Loop over all units and recalc the erosion and deposition.
            MorphologicalUnit muTotal = Units.Last();
            muTotal.VolErosion = new Volume(0);
            muTotal.VolErosionErr = new Volume(0);
            muTotal.VolDeposition = new Volume(0);
            muTotal.VolDepositionErr = new Volume(0);
            foreach (MorphologicalUnit unit in Units)
            {
                muTotal.VolErosion += unit.VolErosion;
                muTotal.VolErosionErr += unit.VolErosionErr;
                muTotal.VolDeposition += unit.VolDeposition;
                muTotal.VolDepositionErr += unit.VolDepositionErr;
            }

            muTotal.VolIn = Units[0].VolIn;
            muTotal.VolOut = Units[Units.Count - 1].VolOut;
            muTotal.CumulativeVolume = Units[Units.Count - 1].CumulativeVolume;

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

        private Duration _duration;
        public Duration Duration
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

        public Duration CompetentDuration { get { return Duration.From(this.Duration.As(DisplayUnits_Duration) * (double)Competency, DisplayUnits_Duration); } }

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

        private UnitsNet.Units.VolumeUnit _DisplayUnits_Volume;
        public UnitsNet.Units.VolumeUnit DisplayUnits_Volume
        {
            get { return _DisplayUnits_Volume; }
            set
            {
                if (_DisplayUnits_Volume == value)
                    return;

                _DisplayUnits_Volume = value;
            }
        }

        private UnitsNet.Units.DurationUnit _DisplayUnits_Duration;
        public UnitsNet.Units.DurationUnit DisplayUnits_Duration
        {
            get { return _DisplayUnits_Duration; }
            set
            {
                if (_DisplayUnits_Duration == value)
                    return;

                _DisplayUnits_Duration = value;
                CalculateWork();
            }
        }

        private UnitsNet.Units.MassUnit _DisplayUnits_Mass;
        public UnitsNet.Units.MassUnit DisplayUnits_Mass
        {
            get { return _DisplayUnits_Mass; }
            set
            {
                if (_DisplayUnits_Mass == value)
                    return;

                _DisplayUnits_Mass = value;
                CalculateWork();
            }
        }

        public void CalculateWork()
        {
            decimal duration = (decimal)CompetentDuration.As(DisplayUnits_Duration);

            decimal gramConvert = (decimal)Mass.FromGrams(1).As(DisplayUnits_Mass);
            decimal volConvert = (decimal)Volume.FromCubicCentimeters(1).As(DisplayUnits_Volume);
            decimal densityConvert = this.Density * (gramConvert / volConvert);

            if (duration > 0)
            {
                foreach (MorphologicalUnit unit in Units)
                {
                    // The volume flux per unit time. THIS IS IN DISPLAY VOLUME UNITS
                    unit.FluxVolume = (1m - Porosity) * (decimal)unit.VolOut.As(DisplayUnits_Volume) / duration;

                    // The volume flux per unit volume and per unit time. THIS IS IN CUBIC METRES
                    //decimal volumeFluxPerUnitVolume = (decimal)Volume.From((double)unit.FluxVolume, DisplayUnits_Volume).As(UnitsNet.Units.VolumeUnit.CubicMeter);

                    // Mass of material per unit time. (Should be independent of volume)
                    unit.FluxMass = unit.FluxVolume * densityConvert;
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
            nodDuration.InnerText = Duration.As(DisplayUnits_Duration).ToString("R");
            nodDuration.Attributes.Append(nodParent.OwnerDocument.CreateAttribute("units")).InnerText = DisplayUnits_Duration.ToString();

            XmlNode nodMinFluxCell = nodMA.AppendChild(nodParent.OwnerDocument.CreateElement("MinimumFluxUnit"));
            if (BoundaryFluxUnit != null)
                nodMinFluxCell.InnerText = BoundaryFluxUnit.Name;

            XmlNode nodMinFlux = nodMA.AppendChild(nodParent.OwnerDocument.CreateElement("MinimumFluxVolume"));
            nodMinFlux.InnerText = BoundaryFlux.As(ProjectManager.Project.Units.VolUnit).ToString("R");
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

            //loop through all the units and update spreadsheet
            for (int UnitIndex = 0; UnitIndex < (Units.Count - 1); UnitIndex++)
            {
                MorphologicalUnit unit = Units[UnitIndex];

                //get values and write to dictionary to update named ranges in spreadsheet
                Dictionary<string, string> dicStatValues = new Dictionary<string, string>();
                dicStatValues.Add("TemplateRowName", unit.Name);
                dicStatValues.Add("VolumeErosion", unit.VolErosion.CubicMeters.ToString());
                dicStatValues.Add("VolumeErosionError", unit.VolErosionErr.CubicMeters.ToString());
                dicStatValues.Add("VolumeDeposition", unit.VolDeposition.CubicMeters.ToString());
                dicStatValues.Add("VolumeDepositionError", unit.VolDepositionErr.CubicMeters.ToString());

                //clone or update template row
                if (UnitIndex > 0)
                {
                    xmlExcelDoc.CloneRow("ReachName", UnitIndex, dicStatValues);
                }
                else
                {
                    xmlExcelDoc.UpdateRow("ReachName", dicStatValues);
                }

            }

            //The formulas in the first row is different from the remaining rows, so they need to be update
            //Results match the UI
            string InitialVInformula = "=" + BoundaryFlux.CubicMeters + "-RC[-3]";
            xmlExcelDoc.SetFormula("InitialVIn", InitialVInformula);

            string InitialVOutformula = "=RC[-4]+RC[-1]";
            xmlExcelDoc.SetFormula("InitialVOut", InitialVOutformula);

            string InitialVCumulativeformula = "=RC[-6]";
            xmlExcelDoc.SetFormula("InitialVCumulative", InitialVCumulativeformula);

            //Set duration adn porosity values in spreadsheet
            xmlExcelDoc.SetNamedCellValue("FlowDuration", CompetentDuration.Hours.ToString());
            xmlExcelDoc.SetNamedCellValue("Porosity", Porosity.ToString());

            //cells should be formatted with black, single weight top and bottom border
            CellStyle oCellStyle = new CellStyle();
            oCellStyle.TopBorder.Weight = 1;
            oCellStyle.TopBorder.Color = "#000000";
            oCellStyle.BottomBorder.Weight = 1;
            oCellStyle.BottomBorder.Color = "#000000";

            //loop through all cells and format
            for (int i = 0; i < (Units.Count - 2); i++)
            {
                xmlExcelDoc.FormatRow("ReachName", i, oCellStyle);
            }

            //save output
            //xml cant be saved using the easier readable format with line breaks between nodes
            //because it mangles the header with subscript. Excel interprets the line breaks as new lines in the cell
            string xml = xmlExcelDoc.GetXML();
            File.WriteAllText(Spreadsheet.FullName, xml);

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
