using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace GCDCore.Engines
{
    class ExcelXMLDocument
    {
        private string _template;
        private XmlDocument xmlDoc;
        private XmlNamespaceManager nsmgr;
        private Dictionary<string, NamedRange> dicNamedRanges;

        public ExcelXMLDocument(string template)
       {
            _template = template;

            //read template into XML document
            xmlDoc = new XmlDocument();
            xmlDoc.Load(template);

            //xpaths needs to be specified with namespace, see e.g. https://stackoverflow.com/questions/36504656/how-to-select-xml-nodes-by-position-linq-or-xpath
            nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");

            //get named ranges
            ParseNamedRanges();
        }

        private void ParseNamedRanges()
        {
            dicNamedRanges = new Dictionary<string, NamedRange>();

            XmlNodeList NamedRangeNodes = xmlDoc.SelectNodes(".//ss:Names/ss:NamedRange", nsmgr); // gets the cell with the named cell name

            foreach (XmlNode NamedRange in NamedRangeNodes)
            {
                string name = NamedRange.Attributes["ss:Name"].Value;

                string refersto = NamedRange.Attributes["ss:RefersTo"].Value;
                //match R*C*
                Regex r = new Regex(@".*!R(.+)C(.+)", RegexOptions.IgnoreCase);
                Match m = r.Match(refersto);
                string sRow = m.Groups[1].Value;
                int iRow = int.Parse(sRow);

                string sCol = m.Groups[2].Value;
                int iCol = int.Parse(sCol);

                NamedRange oNamedRange = new Engines.NamedRange();
                oNamedRange.name = name;
                oNamedRange.col = iCol;
                oNamedRange.row = iRow;

                dicNamedRanges.Add(name, oNamedRange);
            }

        }

        public void UpdateRow(string NamedRange, Dictionary<string, string> NamedRangeValues)
        {
            //get row
            NamedRangeValues[NamedRange] = NamedRangeValues["TemplateRowName"];

            XmlNode TemplateRow = xmlDoc.SelectSingleNode(".//ss:Row[ss:Cell[ss:NamedCell[@ss:Name='" + NamedRange + "']]]", nsmgr); // gets the cell with the named cell name
            SetNamedCellValue(TemplateRow, NamedRangeValues);
        }

        public void CloneRow(string NamedRange, int offset, Dictionary<string, string> NamedRangeValues)
        {
            NamedRangeValues[NamedRange] = NamedRangeValues["TemplateRowName"];

            NamedRange oNamedRange = dicNamedRanges[NamedRange];
            int row = oNamedRange.row;
            InsertRow(row);

            //find areal row
            XmlNode TemplateRow = xmlDoc.SelectSingleNode(".//ss:Row[ss:Cell[ss:NamedCell[@ss:Name='" + NamedRange + "']]]", nsmgr); // gets the cell with the named cell name
            XmlNode TemplateRowClone = TemplateRow.Clone();

            SetNamedCellValue(TemplateRowClone, NamedRangeValues);

            XmlNode parent = TemplateRow.ParentNode;
            parent.InsertAfter(TemplateRowClone, TemplateRow);
        }

        /// <summary>
        /// Updates named cells in the node with the values in StatValues
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="nsmgr"></param>
        /// <param name="StatValues"></param>
        private void SetNamedCellValue(XmlNode xmlNode, Dictionary<string, string> StatValues)
        {
            foreach (KeyValuePair<string, string> kvp in StatValues)
            {
                String NamedCell = kvp.Key;
                String Value = kvp.Value;

                //find named cells
                XmlNodeList NamedCells = xmlNode.SelectNodes(".//ss:Cell[ss:NamedCell[@ss:Name='" + NamedCell + "']]", nsmgr); // gets the cell with the named cell name
                if (NamedCells.Count == 1)
                {
                    //if there is exactly one match, update data
                    XmlNode CellData = NamedCells[0].SelectSingleNode("ss:Data", nsmgr);
                    CellData.InnerText = Value;
                }

            }
        }

        public void Save(string path)
        {
            xmlDoc.Save(path);
        }

        private void InsertRow(int rownumber)
        {
            Dictionary<string, NamedRange> dicUpdatedNamedRanges = new Dictionary<string, NamedRange>();
            foreach (NamedRange oNamedRange in dicNamedRanges.Values)
            {
                if (oNamedRange.row > rownumber)
                {
                    oNamedRange.row += 1;
                }
                dicUpdatedNamedRanges.Add(oNamedRange.name, oNamedRange);
            }

            XmlNode TableNode = xmlDoc.SelectSingleNode(".//ss:Table", nsmgr); // gets the cell with the named cell name
            string OrigExpandedRowCount = TableNode.Attributes["ss:ExpandedRowCount"].Value;
            int iOrigExpandedRowCount = int.Parse(OrigExpandedRowCount);
            int iNewExpandedRowCount = iOrigExpandedRowCount + 1; //add new row twice (once for area, once for volume)
            TableNode.Attributes["ss:ExpandedRowCount"].Value = iNewExpandedRowCount.ToString();

            //update formulas

            //first, find all rows, the loop through rows that are > rownumber
            XmlNodeList AllRows;
            AllRows = xmlDoc.SelectNodes(".//ss:Row", nsmgr); // gets the cell with the named cell name

            try
            {

                for (int RowIndex = 0; RowIndex < AllRows.Count; RowIndex++)
                {
                    //get row
                    XmlNode CurrentRow = AllRows[RowIndex];

                    //select cells with formulas

                    XmlNodeList CellsWithFormulas = CurrentRow.SelectNodes(".//ss:Cell[@ss:Formula]", nsmgr);

                    //for each formula, check if it contains a relative reference, pattern "=R[-5]C[-4]/R[-10]C10", e.g. R[-5]C[-4]
                    foreach (XmlNode currentCell in CellsWithFormulas)
                    {
                        //get formula
                        string formula = currentCell.Attributes["ss:Formula"].Value;

                        //match for R
                        //var pattern = @"R\[-(\d+)\]C"; //matches only one in =SUM(R[-1]C:R[-1]C)
                        var pattern = @"(R\[-)(\d+)(\]C)";

                        Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
                        MatchCollection mc = r.Matches(formula);

                        if (mc.Count > 0)
                        {

                            string NewFormula = "";
                            int textindex = 0;
                            for (int matchindex = 0; matchindex < mc.Count; matchindex++)
                            {
                                Match m = mc[matchindex];
                                NewFormula = NewFormula + formula.Substring(textindex, m.Index - textindex);

                                string sRow = m.Groups[2].Value;
                                int iRow = int.Parse(sRow);
                                int correctionrow = 0;
                                int referencerow = RowIndex - iRow;
                                if (referencerow <= rownumber && RowIndex > rownumber)
                                {
                                    correctionrow = 1;
                                }

                                string replace = m.Groups[1].Value + (iRow + correctionrow) + m.Groups[3].Value;

                                NewFormula = NewFormula + replace;

                                textindex = m.Index + m.Length;

                            }

                            NewFormula = NewFormula + formula.Substring(textindex);

                            currentCell.Attributes["ss:Formula"].Value = NewFormula;
                        }


                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            dicNamedRanges =  dicUpdatedNamedRanges;

            UpdateNamedRangesXML();

        }

        private void UpdateNamedRangesXML()
        {
            foreach (NamedRange oNamedRange in dicNamedRanges.Values)
            {

                XmlNode NamedRangeNode = xmlDoc.SelectSingleNode(".//ss:Names/ss:NamedRange[@ss:Name='" + oNamedRange.name + "']", nsmgr); // gets the cell with the named cell name
                string refersto = NamedRangeNode.Attributes["ss:RefersTo"].Value;

                var pattern = @"(.*!)R(.+)C(.+)";
                var replaced = Regex.Replace(refersto, pattern, "$1R" + oNamedRange.row + "C" + oNamedRange.col);

                NamedRangeNode.Attributes["ss:RefersTo"].Value = replaced;
            }
        }
    }


}
