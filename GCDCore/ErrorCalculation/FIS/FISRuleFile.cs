using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GCDCore.ErrorCalculation.FIS
{
    public class FISRuleFile
    {
        public readonly System.IO.FileInfo RuleFilePath;
        public readonly List<string> FISInputs;

        public override string ToString()
        {
            return string.Format("{0} ({1} Inputs)", System.IO.Path.GetFileName(RuleFilePath.FullName), FISInputs.Count);
        }

        public FISRuleFile(System.IO.FileInfo filePath)
        {
            if (!filePath.Exists)
            {
                Exception ex = new Exception("The FIS rule file cannot be found on the file system.");
                ex.Data["FIS Rule File Path"] = filePath;
                throw ex;
            }
            RuleFilePath = filePath;

            try
            {
                string sRuleFileText = System.IO.File.ReadAllText(RuleFilePath.FullName);
                FISInputs = new List<string>();

                Regex theRegEx = new Regex("dd");
                Match theMatch = theRegEx.Match(sRuleFileText);

                // Match data between single quotes hesitantly.
                MatchCollection col = Regex.Matches(sRuleFileText, "\\[Input[0-9]\\]\\s*Name='([^']*)'");
                foreach (Match m in col)
                {
                    // Access first Group and its value.
                    Group g = m.Groups[1];
                    FISInputs.Add(g.Value);
                }
            }
            catch (Exception ex)
            {
                Exception ex2 = new Exception("Error parsing FIS rule file", ex);
                ex2.Data["FIS Rule File Path"] = RuleFilePath;
                throw ex2;
            }
        }
    }
}