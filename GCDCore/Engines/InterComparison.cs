using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using GCDCore.Project;
using GCDCore.UserInterface.ChangeDetection;

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

                /*
                XmlNodeList nodes6 = xmlDoc.SelectNodes("//ss:Cell[ss:NamedCell[@ss:Name='ArealDoDName']]", nsmgr); // gets the cell with the named cell name

                XmlNode CellData = nodes6[0].SelectSingleNode("ss:Data", nsmgr);

                CellData.InnerText = dodStats.Keys.First();
                */

                GCDConsoleLib.GCD.DoDStats dodStat;

                string dodName = dodStats.Keys.First();

                dodStat = dodStats[dodName];

                SetNameCellValue(xmlDoc, nsmgr, "ArealDoDName", dodName);


                //using same pattern as ucDoDSummary

                UnitsNet.Area ca = ProjectManager.Project.CellArea;
                DoDSummaryDisplayOptions options = new DoDSummaryDisplayOptions(ProjectManager.Project.Units);

                string ArealLoweringRaw = dodStat.ErosionRaw.GetArea(ca).As(options.AreaUnits).ToString();
                SetNameCellValue(xmlDoc, nsmgr, "ArealLoweringRaw", ArealLoweringRaw);

                string ArealLoweringThresholded = dodStat.ErosionThr.GetArea(ca).As(options.AreaUnits).ToString();
                SetNameCellValue(xmlDoc, nsmgr, "ArealLoweringThresholded", ArealLoweringThresholded);

                string ArealRaisingRaw = dodStat.DepositionRaw.GetArea(ca).As(options.AreaUnits).ToString();
                SetNameCellValue(xmlDoc, nsmgr, "ArealRaisingRaw", ArealRaisingRaw);

                string ArealRaisingThresholded = dodStat.DepositionThr.GetArea(ca).As(options.AreaUnits).ToString();
                SetNameCellValue(xmlDoc, nsmgr, "ArealRaisingThresholded", ArealRaisingThresholded);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("DONE!");
            xmlDoc.Save(output.FullName);


        }

        private static void SetNameCellValue(XmlDocument xmlDoc, XmlNamespaceManager nsmgr, string NamedCell, string value)
        {
            XmlNodeList NamedCells = xmlDoc.SelectNodes("//ss:Cell[ss:NamedCell[@ss:Name='" + NamedCell + "']]", nsmgr); // gets the cell with the named cell name

            XmlNode CellData = NamedCells[0].SelectSingleNode("ss:Data", nsmgr);

            CellData.InnerText = value;

        }
    }
}
