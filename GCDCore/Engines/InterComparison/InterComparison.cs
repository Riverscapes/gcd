using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using GCDCore.Project;
using GCDCore.UserInterface.ChangeDetection;
using System.Text.RegularExpressions;

namespace GCDCore.Engines
{
    public class InterComparison
    {
        /// <summary>
        /// Generate an inter-comparison summary Excel XML spreadsheet
        /// </summary>
        /// <param name="dodStats">Dictionary of DoD statistics.</param>
        /// <param name="output">File path of the output inter-comparison spreadsheet to be generated</param>
        /// <remarks>
        /// This method is used from two places for subtly different purposes:
        /// 
        /// 1. Every budget segregation will generate a single inter-comparison spreadsheet summarizing the DoD statistics for all classes
        /// 2. The user can manually generate an inter-comparison by selecting two or more DoDs in the user interface.
        /// 
        /// In both cases the code that calls this method is responsible for building a dictionary of DoD statistics. In the first
        /// case the dictionary key will be the budget segregation class ("pool", "riffle" etc). In the second case the key will be
        /// the DoD name ("2006 - 2006 Min LoD 0.2m" etc).
        /// 
        /// The processing in this class is identical for both cases.</remarks>
        public static void Generate(Dictionary<string, GCDConsoleLib.GCD.DoDStats> dodStats, FileInfo output)
        {
            //get template and throw error if it doesnt exists
            FileInfo template = new FileInfo(Path.Combine(Project.ProjectManager.ExcelTemplatesFolder.FullName, "InterComparison.xml"));
            if (!template.Exists)
            {
                throw new Exception("The GCD intercomparison spreadsheet template cannot be found at " + template.FullName);
            }

            ExcelXMLDocument xmlExcelDoc = new ExcelXMLDocument(template.FullName);

            //read template into XML document
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(template.FullName);

            //xpaths needs to be specified with namespace, see e.g. https://stackoverflow.com/questions/36504656/how-to-select-xml-nodes-by-position-linq-or-xpath
            var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");

            //clone Areal stats row

            int DoDCount = 0;
            XmlNode ArealRow;

            //get template row
            string NamedCell = "ArealDoDName";
            ArealRow = xmlDoc.SelectSingleNode(".//ss:Row[ss:Cell[ss:NamedCell[@ss:Name='" + NamedCell + "']]]", nsmgr); // gets the cell with the named cell name

            foreach (KeyValuePair<string, GCDConsoleLib.GCD.DoDStats> kvp in dodStats)
            {
                string DoDName = kvp.Key;
                GCDConsoleLib.GCD.DoDStats dodStat = kvp.Value;

                Dictionary<string, string> dicStatValues = GetStatValues(dodStat);
                dicStatValues.Add("TemplateRowName", DoDName);

                DoDCount += 1;

                if (DoDCount > 1)
                {
                    xmlExcelDoc.CloneRow("ArealDoDName", DoDCount - 1, dicStatValues);
                    xmlExcelDoc.CloneRow("VolumeDoDName", DoDCount - 1, dicStatValues);
                    xmlExcelDoc.CloneRow("VerticalDoDName", DoDCount - 1, dicStatValues);
                    xmlExcelDoc.CloneRow("PercentagesDoDName", DoDCount - 1, dicStatValues);
                }
                else
                {
                    xmlExcelDoc.UpdateRow("ArealDoDName", dicStatValues);
                    xmlExcelDoc.UpdateRow("VolumeDoDName", dicStatValues);
                    xmlExcelDoc.UpdateRow("VerticalDoDName", dicStatValues);
                    xmlExcelDoc.UpdateRow("PercentagesDoDName", dicStatValues);
                }

            }

            //save output
            xmlExcelDoc.Save(output.FullName);
        }

        private static void SetSumFormula(XmlNode xmlDoc, XmlNamespaceManager nsmgr, string SumFormularNamedCell, int SumCount)
        {
            XmlNode SumFormulaCell = xmlDoc.SelectSingleNode(".//ss:Cell[ss:NamedCell[@ss:Name='" + SumFormularNamedCell + "']]", nsmgr); // gets the cell with the named cell name

            string SumFormula = "=SUM(R[-" + SumCount + "]C:R[-1]C)";
            SumFormulaCell.Attributes["ss:Formula"].Value = SumFormula;

        }

        private static Dictionary<string, string> GetStatValues(GCDConsoleLib.GCD.DoDStats dodStat)
        {
            Dictionary<string, string> StatValues = new Dictionary<string, string>();

            //get settings and options
            UnitsNet.Area ca = ProjectManager.Project.CellArea;
            DoDSummaryDisplayOptions options = new DoDSummaryDisplayOptions(ProjectManager.Project.Units);
            UnitsNet.Units.LengthUnit vunit = ProjectManager.Project.Units.VertUnit;

            //using same pattern as ucDoDSummary
            StatValues["ArealLoweringRaw"] = dodStat.ErosionRaw.GetArea(ca).As(options.AreaUnits).ToString();
            StatValues["ArealLoweringThresholded"] = dodStat.ErosionThr.GetArea(ca).As(options.AreaUnits).ToString();
            StatValues["ArealRaisingRaw"] = dodStat.DepositionRaw.GetArea(ca).As(options.AreaUnits).ToString();
            StatValues["ArealRaisingThresholded"] = dodStat.DepositionThr.GetArea(ca).As(options.AreaUnits).ToString();

            StatValues["VolumeLoweringRaw"] = dodStat.ErosionRaw.GetVolume(ca, vunit).As(options.VolumeUnits).ToString();
            StatValues["VolumeLoweringThresholded"] = dodStat.ErosionThr.GetVolume(ca, vunit).As(options.VolumeUnits).ToString();
            StatValues["VolumeErrorLowering"] = dodStat.ErosionErr.GetVolume(ca, vunit).As(options.VolumeUnits).ToString();
            StatValues["VolumeRaisingRaw"] = dodStat.DepositionRaw.GetVolume(ca, vunit).As(options.VolumeUnits).ToString();
            StatValues["VolumeRaisingThresholded"] = dodStat.DepositionThr.GetVolume(ca, vunit).As(options.VolumeUnits).ToString();
            StatValues["VolumeErrorRaising"] = dodStat.DepositionErr.GetVolume(ca, vunit).As(options.VolumeUnits).ToString();

            return StatValues;
        }
    }

    #region "support classes"
    class NamedRange
    {
        public int row;
        public int col;
        public string name;
    }      

    #endregion

}
