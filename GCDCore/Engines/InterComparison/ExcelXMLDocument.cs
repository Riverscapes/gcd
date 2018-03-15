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

        #region "Members"

            private string _template;
            private XmlDocument xmlDoc;
            private XmlNamespaceManager nsmgr;
            private Dictionary<string, NamedRange> dicNamedRanges;

        #endregion

        #region "Constructor"

        /// <summary>
        /// Constructor. Initializes XML document and load in template, sets up namespace manager and parses named ranges
        /// </summary>
        /// <param name="template"></param>
        public ExcelXMLDocument(string template)
        {
            _template = template;

            //read template into XML document
            xmlDoc = new XmlDocument();
            xmlDoc.Load(template);

            //xpaths needs to be specified with namespace, see e.g. https://stackoverflow.com/questions/36504656/how-to-select-xml-nodes-by-position-linq-or-xpath
            nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");

            //parse named ranges from XML
            ParseNamedRanges();
        }

        #endregion

        #region "Public methods"

        /// <summary>
        /// Updates the named values in row identified by named range using the NamedRangeValues dictionary
        /// </summary>
        /// <param name="NamedRange"></param>
        /// <param name="NamedRangeValues"></param>
        public void UpdateRow(string NamedRange, Dictionary<string, string> NamedRangeValues)
        {
            //Add name to dictionary
            NamedRangeValues[NamedRange] = NamedRangeValues["TemplateRowName"];

            //Get temlate row
            XmlNode TemplateRow = xmlDoc.SelectSingleNode(".//ss:Row[ss:Cell[ss:NamedCell[@ss:Name='" + NamedRange + "']]]", nsmgr); // gets the cell with the named cell name

            //Update all named values that match a key in the dictionary
            SetNamedCellValue(TemplateRow, NamedRangeValues);
        }

        /// <summary>
        /// Clones a row identified by NamedRange, updates the template rows named range using the NamedRangeValues dicitonary and inserts it offset rows down
        /// </summary>
        /// <param name="NamedRange"></param>
        /// <param name="offset"></param>
        /// <param name="NamedRangeValues"></param>
        /// <remarks>
        /// References are maintain in the UpdateReferences method 
        /// </remarks>
        public void CloneRow(string NamedRange, int offset, Dictionary<string, string> NamedRangeValues)
        {
            //Add name to dictionary

            NamedRangeValues[NamedRange] = NamedRangeValues["TemplateRowName"];

            //find template row
            XmlNode TemplateRow = xmlDoc.SelectSingleNode(".//ss:Row[ss:Cell[ss:NamedCell[@ss:Name='" + NamedRange + "']]]", nsmgr); // gets the cell with the named cell name
            XmlNode TemplateRowClone = TemplateRow.Clone();

            //Update all named values that match a key in the dictionary
            SetNamedCellValue(TemplateRowClone, NamedRangeValues);

            //Find reference row
            //Example: Our template row is row 4, offset is one, we will insert at row 5, and use row for as the node before
            NamedRange oNamedRange = dicNamedRanges[NamedRange];
            int NamedRangeRow = oNamedRange.row;
            int ReferenceRowBeforeInsert = NamedRangeRow + offset - 1;
            int InsertRowNumber = NamedRangeRow + offset; //we are inserting the row just below the reference row

            //get reference and before nodes and insert our cloned reference row
            XmlNode ReferenceRowNode = xmlDoc.SelectSingleNode(".//ss:Row[position() >= " + ReferenceRowBeforeInsert + "]", nsmgr);
            XmlNode parent = TemplateRow.ParentNode;
            parent.InsertAfter(TemplateRowClone, ReferenceRowNode);

            //Maintain references
            UpdateReferences(InsertRowNumber);
        }

        /// <summary>
        /// Formats all cells in a row based on CellStyle parameter
        /// </summary>
        /// <param name="NamedRange"></param>
        /// <param name="offset"></param>
        /// <param name="RowFormat"></param>
        public void FormatRow(string NamedRange, int offset, CellStyle RowFormat)
        {
            XmlNode CellNode;

            //get row node
            NamedRange oNamedRange = dicNamedRanges[NamedRange];
            int NamedRangeRow = oNamedRange.row;
            int ReferenceRow = NamedRangeRow + offset;
            XmlNode ReferenceRowNode = xmlDoc.SelectSingleNode(".//ss:Row[position() >= " + ReferenceRow + "]", nsmgr);

            //get cells
            XmlNodeList CellNodes = ReferenceRowNode.SelectNodes(".//ss:Cell", nsmgr);

            //loop through all cells and format using the FormatCell method
            for (int i = 0; i < CellNodes.Count; i++)
            {
                CellNode = CellNodes[i];
                FormatCell(CellNode, RowFormat);
            }

        }

        public void SetFormula(string NamedRange, string formula)
        {
            //get row for named range
            if (!dicNamedRanges.ContainsKey(NamedRange))
            {
                return;
            }

            NamedRange oNamedRange = dicNamedRanges[NamedRange];

            //get reference and before nodes and insert our cloned reference row
            XmlNode ReferenceRowNode = xmlDoc.SelectSingleNode(".//ss:Row[position() >= " + oNamedRange.row + "]", nsmgr);

            //find named cells
            XmlNodeList NamedCells = ReferenceRowNode.SelectNodes(".//ss:Cell[ss:NamedCell[@ss:Name='" + NamedRange + "']]", nsmgr); // gets the cell with the named cell name
            if (NamedCells.Count == 1)
            {
                NamedCells[0].Attributes["ss:Formula"].Value = formula;
            }
        }

        public void SetNamedCellValue(string NamedCell, string value)
        {
            Dictionary<string, string> NamedCellValues = new Dictionary<string, string>();
            NamedCellValues.Add(NamedCell, value);
            SetNamedCellValue(xmlDoc, NamedCellValues);
        }


        /// <summary>
        /// Write Excel XML document to filepath
        /// </summary>
        /// <param name="path"></param>
        public void Save(string path)
        {
            xmlDoc.Save(path);
        }

        #endregion

        #region "Private methods"

        /// <summary>
        /// Creates a dictionary of named ranges from XML
        /// </summary>
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

                //parse row and column
                string sRow = m.Groups[1].Value;
                int iRow = int.Parse(sRow);
                string sCol = m.Groups[2].Value;
                int iCol = int.Parse(sCol);

                //create named range object and add to dictionary
                NamedRange oNamedRange = new Engines.NamedRange();
                oNamedRange.name = name;
                oNamedRange.col = iCol;
                oNamedRange.row = iRow;
                dicNamedRanges.Add(name, oNamedRange);
            }

        }

        /// <summary>
        /// Updates all references, e.g. relative references, sum formulas and named ranges, after inserting row
        /// </summary>
        /// <param name="InsertRowNumber"></param>
        private void UpdateReferences(int InsertRowNumber)
        {
            //update named range
            Dictionary<string, NamedRange> dicUpdatedNamedRanges = new Dictionary<string, NamedRange>();
            foreach (NamedRange oNamedRange in dicNamedRanges.Values)
            {
                if (oNamedRange.row >= InsertRowNumber)
                {
                    oNamedRange.row += 1;
                }
                dicUpdatedNamedRanges.Add(oNamedRange.name, oNamedRange);
            }
            dicNamedRanges = dicUpdatedNamedRanges;
            UpdateNamedRangesXML();

            //Update total number of rows in spreadsheet
            XmlNode TableNode = xmlDoc.SelectSingleNode(".//ss:Table", nsmgr); // gets the cell with the named cell name
            string OrigExpandedRowCount = TableNode.Attributes["ss:ExpandedRowCount"].Value;
            int iOrigExpandedRowCount = int.Parse(OrigExpandedRowCount);
            int iNewExpandedRowCount = iOrigExpandedRowCount + 1; //add new row twice (once for area, once for volume)
            TableNode.Attributes["ss:ExpandedRowCount"].Value = iNewExpandedRowCount.ToString();

            //update formulas
            UpdateFormulaReferences(InsertRowNumber);
            UpdateSumFormulaRange(InsertRowNumber);

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
                    if(CellData != null)
                    {
                        CellData.InnerText = Value;
                    } else {
                        //need to add data node
                        XmlNode NewCellData = xmlDoc.CreateNode("element", "Data", "urn:schemas-microsoft-com:office:spreadsheet");
                        NewCellData.InnerText = Value;

                        //Create a new attribute
                        //ss:Type="Number"
                        XmlAttribute attr = xmlDoc.CreateAttribute("ss", "Type", "urn:schemas-microsoft-com:office:spreadsheet");
                        attr.Value = "Number";

                        //Add the attribute to the node     
                        NewCellData.Attributes.SetNamedItem(attr);


                        //Add the attribute to the node     
                        NamedCells[0].AppendChild(NewCellData);

                    }
                }

            }
        }

        /// <summary>
        /// updates references in the form of R[-1]C
        /// </summary>
        /// <param name="rownumber"></param>
        private void UpdateFormulaReferences(int rownumber)
        {
            //first, find all rows, the loop through rows that are > rownumber
            XmlNodeList AllRows;
            AllRows = xmlDoc.SelectNodes(".//ss:Row", nsmgr); // gets the cell with the named cell name

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

        /// <summary>
        /// Update sum formulas
        /// </summary>
        /// <param name="InsertRowNumber"></param>
        private void UpdateSumFormulaRange(int InsertRowNumber)
        {
            //first, find all rows, the loop through all rows to find formulas
            //we need to do it this way to know what the row number is

            XmlNodeList AllRows;
            AllRows = xmlDoc.SelectNodes(".//ss:Row", nsmgr); // gets the cell with the named cell name

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
                    int FormulaRow = RowIndex + 1; //RowIndex is zero based, Excel Rows are one-based
                    ExcelSumFormula oExcelFormula = ParseSumFormula(formula, FormulaRow);

                    //if formula is a sum formula, updates if necessary
                    if(oExcelFormula != null)
                    {
                        ExcelSumFormula oUpdatedExcelFormula = UpdateSumFormula(oExcelFormula, InsertRowNumber);
                        currentCell.Attributes["ss:Formula"].Value = oUpdatedExcelFormula.GetFormula();
                    }

                }

            }

        }

        /// <summary>
        /// Updates the sum formula range if a new row is added just below the range or inside the range
        /// </summary>
        /// <param name="oExcelSumFormula"></param>
        /// <param name="InsertRow"></param>
        /// <returns></returns>
        private ExcelSumFormula UpdateSumFormula(ExcelSumFormula oExcelSumFormula, int InsertRow)
        {
            Boolean ExpandRange = false;

            //if new row is inserted just below range, expand range
            //Example a. Range is from 5:5, new row inserted at row 5, range exanded to 4:5
            //Example a. Range is from 2:3, new row inserted at row 4, range exanded to 2:4
            if (InsertRow == oExcelSumFormula.ToAbsoluteRow) 
            {
                ExpandRange = true;
            }

            if(ExpandRange)
            {
                oExcelSumFormula.FromRelativeRow = oExcelSumFormula.FromRelativeRow + 1;
            }

            return (oExcelSumFormula);
        }

        /// <summary>
        /// Parse an XML sum formula and returns a SumFormulaObject
        /// </summary>
        /// <remarks>
        /// Only sums in the format =SUM(R[-1]C:R[-1]C) is currently match to lock down the system
        /// </remarks>
        private ExcelSumFormula ParseSumFormula(string formula, int FormulaRow)
        {
            //this patter will match e.g. =SUM(R[-1]C:R[-1]C) and return 2 groups, one for the first row reference and one for the second, without sign
            string pattern = @"=SUM\(R\[-(\d+)\]C:R\[-(\d+)\]C\)"; 

            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection mc = r.Matches(formula);

            //return null if formula doesnt match
            if (mc.Count != 1)
            {
                return (null);
            }

            //parse FromRow and ToRow
            Match m = mc[0]; //We've alread checked that the MatchCollection only contains one match, so this is safe
            string sFromRow = m.Groups[1].Value;
            int iFromRow = int.Parse(sFromRow);
            string sToRow = m.Groups[2].Value;
            int iToRow = int.Parse(sToRow);

            ExcelSumFormula oExcelSumFormula = new ExcelSumFormula();
            oExcelSumFormula.FormulaRow = FormulaRow;
            oExcelSumFormula.FromRelativeRow = iFromRow;
            oExcelSumFormula.ToRelativeRow = iToRow;

            return (oExcelSumFormula);

        }

        /// <summary>
        /// Updates named ranges in XML spreadsheet based on the private dictionary dicNamedRanges
        /// </summary>
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

        /// <summary>
        /// formats a cell based on RowFormat parameter
        /// </summary>
        /// <param name="CellNode"></param>
        /// <param name="RowFormat"></param>
        private void FormatCell(XmlNode CellNode, CellStyle RowFormat)
        {
            //get style id, if available
            if (HasAttribute(CellNode, "ss:StyleID"))
            {
                string styleid = CellNode.Attributes["ss:StyleID"].Value;

                //get style
                string pattern = "//ss:Style[@ss:ID='" + styleid + "']"; //find all nodes of type "Style" anywhere in document with an attribute called ss:ID equivalent to variable styleid
                XmlNode StyleNode = xmlDoc.SelectSingleNode(pattern, nsmgr);

                //modify style
                if (RowFormat.TopBorder.Weight != null)
                {
                    FormatBorder(StyleNode, "Top", "Weight", RowFormat.TopBorder.Weight.ToString());
                }

                if (RowFormat.TopBorder.Color != null)
                {
                    FormatBorder(StyleNode, "Top", "Color", RowFormat.TopBorder.Color);
                }

                if (RowFormat.BottomBorder.Weight != null)
                {
                    FormatBorder(StyleNode, "Bottom", "Weight", RowFormat.TopBorder.Weight.ToString());
                }

                if (RowFormat.BottomBorder.Color != null)
                {
                    FormatBorder(StyleNode, "Bottom", "Color", RowFormat.TopBorder.Color);
                }

            }
        }

        /// <summary>
        /// Formats cellstyle border
        /// </summary>
        /// <param name="StyleNode"></param>
        /// <param name="position"></param>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        private void FormatBorder(XmlNode StyleNode, string position, string attribute, string value)
        {
            //get border node based on position
            string BorderPattern = ".//ss:Border[@ss:Position='" + position + "']"; //find all nodes of type "Style" anywhere in document with an attribute called ss:ID equivalent to variable styleid
            XmlNode BorderNode = StyleNode.SelectSingleNode(BorderPattern, nsmgr);

            //set attribute to value if bordernode exists
            if (BorderNode != null)
            {

                //set attribute if it exists, otherwise create new attribute
                if (HasAttribute(BorderNode, "ss:" + attribute))
                {
                    BorderNode.Attributes["ss:" + attribute].Value = value;
                }
                else
                {
                    //Create a new attribute
                    XmlAttribute attr = xmlDoc.CreateAttribute("ss", attribute, "urn:schemas-microsoft-com:office:spreadsheet");
                    attr.Value = value;

                    //Add the attribute to the node     
                    BorderNode.Attributes.SetNamedItem(attr);
                }
            }
        }

        /// <summary>
        /// Support function that check if CellNode has attribute
        /// </summary>
        /// <param name="CellNode"></param>
        /// <param name="AttributeName"></param>
        /// <returns></returns>
        private Boolean HasAttribute(XmlNode CellNode, string AttributeName)
        {
            foreach (XmlAttribute oAttribute in CellNode.Attributes)
            {
                if (oAttribute.Name == AttributeName)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

    }

    #region "Support Classess"
    class ExcelSumFormula
    {
        public int FormulaRow { get; set; }
        public int FromRelativeRow { get; set; }
        public int ToRelativeRow { get; set; }

        public int FromAbsoluteRow
        {
            get { return (FormulaRow - FromRelativeRow); }
        }

        public int ToAbsoluteRow
        {
            get { return (FormulaRow - ToRelativeRow); }
        }

        public string GetFormula()
        {
            string formula = "=SUM(R[-" + this.FromRelativeRow + "]C:R[-" + this.ToRelativeRow + "]C)";
            return (formula);
        }
    }

    class NamedRange
    {
        public int row;
        public int col;
        public string name;
    }

    class BorderStyle
    {
        public int? Weight;
        public string Color;
    }

    class CellStyle
    {
        public BorderStyle TopBorder;
        public BorderStyle BottomBorder;

        public CellStyle()
        {
            TopBorder = new BorderStyle();
            BottomBorder = new BorderStyle();
        }
    }
    #endregion
}
