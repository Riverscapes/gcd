using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            FileInfo template = new FileInfo(Path.Combine(Project.ProjectManager.ExcelTemplatesFolder.FullName, "InterComparison.xml"));
            if (!template.Exists)
            {
                throw new Exception("The GCD intercomparison spreadsheet template cannot be found at " + template.FullName);
            }

            // TODO: implement inter-comparison template here.

            //read template
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(template.FullName);

            //find node to replace name
            //https://msdn.microsoft.com/library/1431789e-c545-4765-8c09-3057e07d3041
            //XmlNode root = xmlDoc.DocumentElement;

            //XmlNodeList titleNodes = root.SelectNodes("//*"); //works
            //XmlNodeList titleNodes = xmlDoc.ChildNodes;
            //int count = titleNodes.Count;
            //save document

            //XmlNode titleNode = root.SelectSingleNode("Worksheet");


            /*
            foreach (XmlNode nodDEM in root.SelectNodes("Worksheet"))
            {
                Console.WriteLine(nodDEM.InnerText);
            }
            */
            try
            {


                //xpaths needs to be specified with namespace, see e.g. https://stackoverflow.com/questions/36504656/how-to-select-xml-nodes-by-position-linq-or-xpath
                var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                nsmgr.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");

                Dictionary<string, NamedRange> dicNamedRanges = ParseNamedRanges(xmlDoc, nsmgr);

                int DoDCount = 0;
                XmlNode ArealRow;

                string NamedCell = "ArealDoDName";
                ArealRow = xmlDoc.SelectSingleNode("//ss:Row[ss:Cell[ss:NamedCell[@ss:Name='" + NamedCell + "']]]", nsmgr); // gets the cell with the named cell name

                UnitsNet.Area ca = ProjectManager.Project.CellArea;
                DoDSummaryDisplayOptions options = new DoDSummaryDisplayOptions(ProjectManager.Project.Units);

                foreach (KeyValuePair<string, GCDConsoleLib.GCD.DoDStats> kvp in dodStats)
                {
                    string DoDName = kvp.Key;
                    GCDConsoleLib.GCD.DoDStats dodStat = kvp.Value;

                    DoDCount += 1;


                    if (DoDCount > 1)
                    {
                        NamedRange oNamedRange = dicNamedRanges[NamedCell];
                        int row = oNamedRange.row;
                        dicNamedRanges = InsertRow(dicNamedRanges, row);

                        //find areal row
                        XmlNode ArealRowClone = ArealRow.Clone();
                        XmlNode parent = ArealRow.ParentNode;
                        parent.InsertAfter(ArealRowClone, ArealRow);
                        ArealRow = ArealRowClone;
                    }

                    SetNameCellValue(ArealRow, nsmgr, "ArealDoDName", DoDName);

                    //using same pattern as ucDoDSummary
                    string ArealLoweringRaw = dodStat.ErosionRaw.GetArea(ca).As(options.AreaUnits).ToString();
                    SetNameCellValue(ArealRow, nsmgr, "ArealLoweringRaw", ArealLoweringRaw);

                    string ArealLoweringThresholded = dodStat.ErosionThr.GetArea(ca).As(options.AreaUnits).ToString();
                    SetNameCellValue(ArealRow, nsmgr, "ArealLoweringThresholded", ArealLoweringThresholded);

                    string ArealRaisingRaw = dodStat.DepositionRaw.GetArea(ca).As(options.AreaUnits).ToString();
                    SetNameCellValue(ArealRow, nsmgr, "ArealRaisingRaw", ArealRaisingRaw);

                    string ArealRaisingThresholded = dodStat.DepositionThr.GetArea(ca).As(options.AreaUnits).ToString();
                    SetNameCellValue(ArealRow, nsmgr, "ArealRaisingThresholded", ArealRaisingThresholded);

                }

                //Find VolumeDoDName
                string VolumeDoDNamedCell = "VolumeDoDName";
                XmlNode VolumeRow;
                VolumeRow = xmlDoc.SelectSingleNode("//ss:Row[ss:Cell[ss:NamedCell[@ss:Name='" + VolumeDoDNamedCell + "']]]", nsmgr); // gets the cell with the named cell name
                UnitsNet.Units.LengthUnit vunit = ProjectManager.Project.Units.VertUnit;
                DoDCount = 0;

                foreach (KeyValuePair<string, GCDConsoleLib.GCD.DoDStats> kvp in dodStats)
                {
                    string DoDName = kvp.Key;
                    GCDConsoleLib.GCD.DoDStats dodStat = kvp.Value;

                    DoDCount += 1;


                    if (DoDCount > 1)
                    {
                        //find areal row
                        XmlNode VolumeRowClone = VolumeRow.Clone();
                        XmlNode parent = VolumeRow.ParentNode;
                        parent.InsertAfter(VolumeRowClone, VolumeRow);
                        VolumeRow = VolumeRowClone;
                    }
                    //using same pattern as ucDoDSummary

                    SetNameCellValue(xmlDoc, nsmgr, "VolumeDoDName", DoDName);

                    string VolumeLoweringRaw = dodStat.ErosionRaw.GetVolume(ca, vunit).As(options.VolumeUnits).ToString();
                    SetNameCellValue(xmlDoc, nsmgr, "VolumeLoweringRaw", VolumeLoweringRaw);

                    string VolumeLoweringThresholded = dodStat.ErosionThr.GetVolume(ca, vunit).As(options.VolumeUnits).ToString();
                    SetNameCellValue(xmlDoc, nsmgr, "VolumeLoweringThresholded", VolumeLoweringThresholded);

                    string VolumeErrorLowering = dodStat.ErosionErr.GetVolume(ca, vunit).As(options.VolumeUnits).ToString();
                    SetNameCellValue(xmlDoc, nsmgr, "VolumeErrorLowering", VolumeErrorLowering);

                    string VolumeRaisingRaw = dodStat.DepositionRaw.GetVolume(ca, vunit).As(options.VolumeUnits).ToString();
                    SetNameCellValue(xmlDoc, nsmgr, "VolumeRaisingRaw", VolumeRaisingRaw);

                    string VolumeRaisingThresholded = dodStat.DepositionThr.GetVolume(ca, vunit).As(options.VolumeUnits).ToString();
                    SetNameCellValue(xmlDoc, nsmgr, "VolumeRaisingThresholded", VolumeRaisingThresholded);

                    string VolumeErrorRaising = dodStat.DepositionErr.GetVolume(ca, vunit).As(options.VolumeUnits).ToString();
                    SetNameCellValue(xmlDoc, nsmgr, "VolumeErrorRaising", VolumeErrorRaising);
                }

                //need to set after adding rows
                //pattern:
                //<Table ss:ExpandedColumnCount="52" ss:ExpandedRowCount="29" x:FullColumns="1" x:FullRows="1" ss:DefaultRowHeight="15">
                XmlNode TableNode = xmlDoc.SelectSingleNode("//ss:Table", nsmgr); // gets the cell with the named cell name

                string OrigExpandedRowCount = TableNode.Attributes["ss:ExpandedRowCount"].Value;
                int iOrigExpandedRowCount = int.Parse(OrigExpandedRowCount);
                int iNewExpandedRowCount = iOrigExpandedRowCount + 2 * (DoDCount - 1); //add new row twice (once for area, once for volume)
                TableNode.Attributes["ss:ExpandedRowCount"].Value = iNewExpandedRowCount.ToString();

                //Update areal formulas
                SetSumFormula(xmlDoc, nsmgr, "SumRawArealSurfaceLowering", DoDCount);
                SetSumFormula(xmlDoc, nsmgr, "SumThresholdedArealSurfaceLowering", DoDCount);
                SetSumFormula(xmlDoc, nsmgr, "SumPctTotalArealLowering", DoDCount);
                SetSumFormula(xmlDoc, nsmgr, "SumRawArealSurfaceRaising", DoDCount);
                SetSumFormula(xmlDoc, nsmgr, "SumThresholdedArealSurfaceRaising", DoDCount);
                SetSumFormula(xmlDoc, nsmgr, "SumPctTotalArealRaising", DoDCount);
                SetSumFormula(xmlDoc, nsmgr, "SumThresholdedArealDetectableChange", DoDCount);
                SetSumFormula(xmlDoc, nsmgr, "SumPctTotalDetecableChange", DoDCount);
                SetSumFormula(xmlDoc, nsmgr, "SumTotalAreaOfInterest", DoDCount);

                //Update volume formulas
                SetSumFormula(xmlDoc, nsmgr, "SumRawVolumeSurfaceLowering", DoDCount);
                SetSumFormula(xmlDoc, nsmgr, "SumThresholdedVolumeSurfaceLowering", DoDCount);
                SetSumFormula(xmlDoc, nsmgr, "SumErrorVolumeLowering", DoDCount);
                SetSumFormula(xmlDoc, nsmgr, "SumPctTotalVolumeLowering", DoDCount);
                SetSumFormula(xmlDoc, nsmgr, "SumRawVolumeSurfaceRaising", DoDCount);
                SetSumFormula(xmlDoc, nsmgr, "SumThresholdedVolumeSurfaceRaising", DoDCount);
                SetSumFormula(xmlDoc, nsmgr, "SumErrorVolumeRaising", DoDCount);
                SetSumFormula(xmlDoc, nsmgr, "SumPctTotalVolumeRaising", DoDCount);
                SetSumFormula(xmlDoc, nsmgr, "SumRawVolumeOfDifference", DoDCount);
                SetSumFormula(xmlDoc, nsmgr, "SumThresholdedVolumeOfDifference", DoDCount);
                SetSumFormula(xmlDoc, nsmgr, "SumErrorVolumeOfDifference", DoDCount);
                SetSumFormula(xmlDoc, nsmgr, "SumPctVolumeOfDifference", DoDCount);
                SetSumFormula(xmlDoc, nsmgr, "SumRawNetVolumeDifference", DoDCount);
                SetSumFormula(xmlDoc, nsmgr, "SumThresholdedNetVolumeDifference", DoDCount);


                //update areal names range
                UpdateNamedRangeRefersTo(xmlDoc, nsmgr, "SumThresholdedArealSurfaceLowering", DoDCount - 1);
                UpdateNamedRangeRefersTo(xmlDoc, nsmgr, "SumThresholdedArealSurfaceRaising", DoDCount - 1);
                UpdateNamedRangeRefersTo(xmlDoc, nsmgr, "SumThresholdedArealDetectableChange", DoDCount - 1);

                //update volume names range
                UpdateNamedRangeRefersTo(xmlDoc, nsmgr, "SumThresholdedVolumeSurfaceLowering", 2 * (DoDCount - 1));
                UpdateNamedRangeRefersTo(xmlDoc, nsmgr, "SumThresholdedVolumeSurfaceRaising", 2 * (DoDCount - 1));
                UpdateNamedRangeRefersTo(xmlDoc, nsmgr, "SumThresholdedVolumeOfDifference", 2 * (DoDCount - 1));

                UpdateNamedRangesXML(xmlDoc, nsmgr,  dicNamedRanges);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("DONE!");
            xmlDoc.Save(output.FullName);


        }

        private static void UpdateNamedRangesXML(XmlNode xmlDoc, XmlNamespaceManager nsmgr, Dictionary<string, NamedRange> dicNamedRanges)
        {
            foreach(NamedRange oNamedRange in dicNamedRanges.Values)
            {

                XmlNode NamedRangeNode = xmlDoc.SelectSingleNode("//ss:Names/ss:NamedRange[@ss:Name='" + oNamedRange.name + "']", nsmgr); // gets the cell with the named cell name
                string refersto = NamedRangeNode.Attributes["ss:RefersTo"].Value;

                //match R*C*
                /*
                Regex r = new Regex(@".*!R(.+)C.", RegexOptions.IgnoreCase);
                Match m = r.Match(refersto);
                string sRow = m.Groups[1].Value;
                int iRow = int.Parse(sRow);

                iRow = iRow + AddedRows;
                */
                var pattern = @"(.*!)R(.+)C(.+)";
                var replaced = Regex.Replace(refersto, pattern, "$1R" + oNamedRange.row + "C" + oNamedRange.col);

                NamedRangeNode.Attributes["ss:RefersTo"].Value = replaced;
            }
        }


        private static Dictionary<string, NamedRange> InsertRow(Dictionary<string, NamedRange> dicNamedRanges, int rownumber)
        {
            Dictionary<string, NamedRange> dicUpdatedNamedRanges = new Dictionary<string, NamedRange>();
            foreach (NamedRange oNamedRange in dicNamedRanges.Values)
            {
                if(oNamedRange.row > rownumber)
                {
                    oNamedRange.row += 1;
                }
                dicUpdatedNamedRanges.Add(oNamedRange.name, oNamedRange);
            }
            return dicUpdatedNamedRanges;
        }



        private static Dictionary<string, NamedRange> ParseNamedRanges(XmlNode xmlDoc, XmlNamespaceManager nsmgr)
        {
            Dictionary<string, NamedRange> NamedRanges = new Dictionary<string, NamedRange>();

            XmlNodeList NamedRangeNodes = xmlDoc.SelectNodes("//ss:Names/ss:NamedRange", nsmgr); // gets the cell with the named cell name

            foreach(XmlNode NamedRange in NamedRangeNodes)
            {
                string name = NamedRange.Attributes["ss:Name"].Value;

                string refersto = NamedRange.Attributes["ss:RefersTo"].Value;
                //match R*C*
                Regex r = new Regex(@".*!R(.+)C(.+)", RegexOptions.IgnoreCase);
                Match m = r.Match(refersto);
                string sRow = m.Groups[1].Value;
                int iRow = int.Parse(sRow);

                string sCol= m.Groups[2].Value;
                int iCol= int.Parse(sCol);

                NamedRange oNamedRange = new Engines.NamedRange();
                oNamedRange.name = name;
                oNamedRange.col = iCol;
                oNamedRange.row = iRow;

                NamedRanges.Add(name, oNamedRange);
            }

            return (NamedRanges);
        }


        private static void SetNameCellValue(XmlNode xmlDoc, XmlNamespaceManager nsmgr, string NamedCell, string value)
        {
            XmlNodeList NamedCells = xmlDoc.SelectNodes("//ss:Cell[ss:NamedCell[@ss:Name='" + NamedCell + "']]", nsmgr); // gets the cell with the named cell name

            XmlNode CellData = NamedCells[0].SelectSingleNode("ss:Data", nsmgr);

            CellData.InnerText = value;

        }

        private static void SetSumFormula(XmlNode xmlDoc, XmlNamespaceManager nsmgr, string SumFormularNamedCell, int SumCount)
        {
            XmlNode SumFormulaCell = xmlDoc.SelectSingleNode("//ss:Cell[ss:NamedCell[@ss:Name='" + SumFormularNamedCell + "']]", nsmgr); // gets the cell with the named cell name

            string SumFormula = "=SUM(R[-" + SumCount + "]C:R[-1]C)";
            SumFormulaCell.Attributes["ss:Formula"].Value = SumFormula;

        }

        private static void UpdateNamedRangeRefersTo(XmlNode xmlDoc, XmlNamespaceManager nsmgr, string NamedRange, int AddedRows)
        {
            XmlNode NamedRangeNode = xmlDoc.SelectSingleNode("//ss:Names/ss:NamedRange[@ss:Name='" + NamedRange + "']", nsmgr); // gets the cell with the named cell name
            string refersto = NamedRangeNode.Attributes["ss:RefersTo"].Value;

            //match R*C*
            Regex r = new Regex(@".*!R(.+)C.", RegexOptions.IgnoreCase);
            Match m = r.Match(refersto);
            string sRow = m.Groups[1].Value;
            int iRow = int.Parse(sRow);

            iRow = iRow + AddedRows;

            var pattern = @"(.*!)(R)(.+)(C.)";
            var replaced = Regex.Replace(refersto, pattern, "$1R" + iRow + "$4");

            NamedRangeNode.Attributes["ss:RefersTo"].Value = replaced;

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
