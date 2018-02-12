using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
            XmlNodeList nodes = xmlDoc.SelectNodes("//ss:Row", nsmgr);

            XmlNodeList nodes2 = xmlDoc.SelectNodes("//ss:NamedCell", nsmgr);

            XmlNodeList nodes3 = xmlDoc.SelectNodes("//ss:Cell/ss:NamedCell", nsmgr);

            XmlNodeList nodes4 = xmlDoc.SelectNodes("//ss:Cell[ss:NamedCell]", nsmgr);

            //XmlNodeList nodes5 = xmlDoc.SelectNodes("//x:Cell[x:NamedCell@Name='ArealDoDName']", nsmgr);
            XmlNodeList nodes5 = xmlDoc.SelectNodes("//ss:NamedCell[@ss:Name='ArealDoDName']", nsmgr); // gets named cell

            XmlNodeList nodes6 = xmlDoc.SelectNodes("//ss:Cell[ss:NamedCell[@ss:Name='ArealDoDName']]", nsmgr); // gets the cell with the named cell name

                XmlNode CellData = nodes6[0].SelectSingleNode("ss:Data");

                CellData.InnerText = dodStats.Keys.First();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("DONE!");
            xmlDoc.Save(output.FullName);


        }
    }
}
