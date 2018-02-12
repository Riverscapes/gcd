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


                //need to set after adding rows
                //pattern:
                //<Table ss:ExpandedColumnCount="52" ss:ExpandedRowCount="29" x:FullColumns="1" x:FullRows="1" ss:DefaultRowHeight="15">
                XmlNode TableNode= xmlDoc.SelectSingleNode("//ss:Table", nsmgr); // gets the cell with the named cell name

                string OrigExpandedRowCount = TableNode.Attributes["ss:ExpandedRowCount"].Value;
                int iOrigExpandedRowCount = int.Parse(OrigExpandedRowCount);
                int iNewExpandedRowCount = iOrigExpandedRowCount + DoDCount - 1;
                TableNode.Attributes["ss:ExpandedRowCount"].Value = iNewExpandedRowCount.ToString();

                //Update sum formulas
                SetSumFormula(xmlDoc, nsmgr, "TotalRaw", DoDCount);  //update formula for total raw
                SetSumFormula(xmlDoc, nsmgr, "TotalThresholded", DoDCount);  //update formula for total thresholded
                SetSumFormula(xmlDoc, nsmgr, "SumPctTotalArealLowering", DoDCount); //update formula for SumPctTotalArealLowering

                //update names range
                //      <Names>/< NamedRange ss:Name="TotalThresholded" ss:RefersTo="=Intercomparison!R5C3"/>
                XmlNode TotalThresholdedNamedRange = xmlDoc.SelectSingleNode("//ss:Names/ss:NamedRange[@ss:Name='TotalThresholded']", nsmgr); // gets the cell with the named cell name
                string refersto = TotalThresholdedNamedRange.Attributes["ss:RefersTo"].Value;

                //match R*C*
                Regex r = new Regex(@".*!R(.)C.", RegexOptions.IgnoreCase);
                Match m = r.Match(refersto);
                string sRow = m.Groups[1].Value;
                int iRow = int.Parse(sRow);

                iRow = iRow + DoDCount - 1;

                var pattern = @"(.*!)(R)(.)(C.)";
                var replaced = Regex.Replace(refersto, pattern, "$1R" + iRow + "$4");

                TotalThresholdedNamedRange.Attributes["ss:RefersTo"].Value = replaced;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("DONE!");
            xmlDoc.Save(output.FullName);


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
    }
}
