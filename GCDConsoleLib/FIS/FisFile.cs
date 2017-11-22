using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace GCDConsoleLib.FIS
{
    public class FisFile
    {
        private FileInfo _fisFileInfo;
        private int _numInputs, _numOutputs, _numRules;

        enum FISFileSection { SYSTEM, INPUT, OUTPUT, RULES, NONE };

        public RuleSet ruleset;

        private Dictionary<FISFileSection, List<List<string>>> SectionLines;

        static private Dictionary<FISFileSection, Regex> regexes = new Dictionary<FISFileSection, Regex>() {
                { FISFileSection.SYSTEM, new Regex(@"\[system\]", RegexOptions.IgnoreCase) },
                { FISFileSection.OUTPUT, new Regex(@"\[output[0-9]+\]", RegexOptions.IgnoreCase) },
                { FISFileSection.INPUT, new Regex(@"\[input[0-9]+\]", RegexOptions.IgnoreCase) },
                { FISFileSection.RULES, new Regex(@"\[rules\]", RegexOptions.IgnoreCase) },
                { FISFileSection.NONE, new Regex(@"\[system\]", RegexOptions.IgnoreCase) }
            };

        /// <summary>
        /// Constructor and loader all in one
        /// </summary>
        /// <param name="inputFn"></param>
        public FisFile(FileInfo inputFn)
        {
            _numInputs = 0;
            _numOutputs = 0;
            _numRules = 0;
            _fisFileInfo = inputFn;

            // Store each section in its own dictionary list
            SectionLines = new Dictionary<FISFileSection, List<List<string>>>() {
                { FISFileSection.SYSTEM, new List<List<string>>() },
                { FISFileSection.OUTPUT, new List<List<string>>() },
                { FISFileSection.RULES, new List<List<string>>() },
                { FISFileSection.NONE, new List<List<string>>() },
            };

            parseFile(_fisFileInfo);
            parseSystem(SectionLines[FISFileSection.SYSTEM][0]);

            // Inputs and outputs can have multiple
            foreach (List<string> input in SectionLines[FISFileSection.INPUT])
                parseInputOutput(input, true);

            foreach (List<string> output in SectionLines[FISFileSection.OUTPUT])
                parseInputOutput(output, false);

            parseRules(SectionLines[FISFileSection.RULES][0]);
        }

        /// <summary>
        /// We often have the pattern: "[0 0 0.09 0.17]" that needs parsing
        /// </summary>
        public static List<double> RangeSquareBrackets(string input)
        {
            List<double> output = new List<double>();
            string[] sRanges = input.Trim().Substring(1, input.Length - 2).Split();
            for (int idx = 0; idx < sRanges.Length; idx++)
            {
                double tryval;
                if (!double.TryParse(sRanges[idx], out tryval))
                    throw new Exception("Error parsing double");
                output.Add(tryval);
            }
            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        private void parseInputOutput(List<string> sLines, bool isInput)
        {
            string name = "";
            double low = 0;
            double high = 0;
            int numMfs = 0;
            int inputNum = 0; //starts at 1

            Dictionary<string, MemberFunction> mfs = new Dictionary<string, MemberFunction>();

            foreach (string line in sLines)
            {
                // First let's see if we can figure out what input index we have
                if (regexes[FISFileSection.INPUT].IsMatch(line))
                {
                    //[Input1] needs to be a number
                    string lineTrimmed = line.Trim();

                    if (!int.TryParse(lineTrimmed.Substring(6, lineTrimmed.Length - 7), out inputNum))
                        throw new Exception("Could not extract input/output index: " + line);
                }
                else
                {
                    string[] sLSplit = line.Split('=');
                    sLSplit[0] = sLSplit[0].ToLower();

                    // Example: Name='Depth'
                    if (sLSplit[0] == "name")
                        name = sLSplit[1].Replace("'", "");

                    // Example: Range=[0 4]
                    else if (sLSplit[0] == "range")
                    {
                        List<double> sRanges = RangeSquareBrackets(sLSplit[1]);

                        if (sRanges.Count != 2)
                            throw new Exception("Wrong number of ranges: " + line);

                        low = sRanges[0];
                        high = sRanges[1];
                    }

                    // Example: NumMFs=4
                    else if (sLSplit[0] == "nummfs")
                    {
                        if (!int.TryParse(sLSplit[1], out numMfs))
                            throw new Exception("Could not parse number of member functions: " + line);
                    }

                    // Example: MF2='Moderate':'trapmf',[0.09 0.17 0.32 0.46]
                    else if (sLSplit[0].StartsWith("mf"))
                    {
                        int mfnum;
                        int.TryParse(sLSplit[0].Substring(2), out mfnum);
                        if (mfnum != mfs.Count + 1)
                            throw new Exception("Wrong number of memberfunctions: " + line);

                        string[] mflinesplit = sLSplit[1].Split(',');
                        string[] mflinesplitsplit = mflinesplit[0].Split(':');
                        string mfname = mflinesplitsplit[0].Replace("'", "");
                        string mftype = mflinesplitsplit[1].Replace("'", "");

                        if (mfs.ContainsKey(mfname))
                            throw new Exception("Duplicate MF name detected: " + line);

                        List<double> vertices = RangeSquareBrackets(mflinesplit[1]);

                        switch (mftype)
                        {
                            case "trapmf":
                                if (vertices.Count != 4)
                                    throw new Exception("Wrong number of vertices: " + line);
                                mfs[mfname] = new MemberFunction(vertices[0], vertices[1], vertices[2], vertices[3], 1);
                                break;
                            case "trimf":
                                if (vertices.Count != 3)
                                    throw new Exception("Wrong number of vertices: " + line);
                                mfs[mfname] = new MemberFunction(vertices[0], vertices[1], vertices[2], 1);
                                break;
                            default:
                                throw new Exception("Unsupported MF type: " + line);
                        }
                    }
                }
            }

            // Now try to create a memberfunction set from all our parsed functions
            MemberFunctionSet mfSet = new MemberFunctionSet(low, high);
            foreach (KeyValuePair<string, MemberFunction> mfkvp in mfs)
                mfSet.addMF(mfkvp.Key, mfkvp.Value);

            if (isInput)
                ruleset.setInputMFSet(inputNum -1, name, mfSet);
            else
                ruleset.addOutputMFSet(name, mfSet);
        }


        /// <summary>
        /// Parse the rules
        /// </summary>
        /// <param name="sLines"></param>
        private void parseRules(List<string> sLines)
        {
            foreach (string line in sLines)
                // Match everything except the first line
                if (!regexes[FISFileSection.RULES].IsMatch(line))
                    ParseAddRuleString(line);
        }

        /// <summary>
        /// Here's what a rule looks like: 2 1 4, 1 (1) : 1
        /// INPUT INPUT INPUT, (OUTOUT) : OPERATOR
        /// 
        /// 
        /// This is the version that the machine deals with. The first column in this structure 
        /// corresponds to the input variable, the second column corresponds to the output variable,
        /// the third column displays the weight applied to each rule, and the fourth column is 
        /// shorthand that indicates whether this is an OR (2) rule or an AND (1) rule. 
        /// The numbers in the first two columns refer to the index number of the membership function.
        /// A literal interpretation of rule 1 is: "If input 1 is MF1 (the first membership function 
        /// associated with input 1) then output 1 should be MF1 (the first membership function 
        /// associated with output 1) with the weight 1." Since there is only one input for this 
        /// system, the AND connective implied by the 1 in the last column is of no consequence.
        /// </summary>
        /// <param name="ruleString"></param>
        /// <returns></returns>
        private void ParseAddRuleString(string ruleString)
        {
            Rule theRule = new Rule();
            // 2 1 4, 1 (1) <==> 1
            string[] rsSplit1 = ruleString.Split(':');


            // Now let's figure out the operator for this rule
            int op;
            if (!int.TryParse(rsSplit1[1].Trim(), out op))
                throw new Exception("could not parse the operator out of the rule: " + ruleString);

            switch (op)
            {
                case 1:
                    theRule.Operator = FISOperator.FISOp_And;
                    break;
                case 2:
                    theRule.Operator = FISOperator.FISOp_Or;
                    break;
                default:
                    throw new Exception("could not parse the operator out of the rule: " + ruleString);
            }

            // 2 1 4 <==> 1 (1)
            string[] rsSplit2 = rsSplit1[0].Split(',');
            string[] mfIndStr = rsSplit2[0].Trim().Split();
            // 1 <==> (1)
            string[] rsSplit3 = rsSplit2[1].Trim().Split();

            // There is only ever one output for this case
            theRule.Output = ruleset.Output;

            // Now figure out the weight of this Rule
            string trimmedWeight = rsSplit3[1].Trim();
            if (!double.TryParse(trimmedWeight.Substring(1, trimmedWeight.Length - 2), out theRule.Weight))
                throw new Exception("Could not parse the weight: " + ruleString);

            // Now, finally we're ready to look at inputs.
            List<int> mfIndeces = new List<int>();
            for (int idx = 0; idx < mfIndStr.Length; idx++)
            {
                int mfInd;
                if (!int.TryParse(mfIndStr[idx].Trim(), out mfInd))
                    throw new Exception("Error parsing rule: " + ruleString);
                theRule.addMf(idx, mfInd);
            }

            // Last step: push the rule onto the stack
            ruleset.Rules.Add(theRule);
        }

        /// <summary>
        /// Parse the system section of the FIS File
        /// </summary>
        /// <param name="sLines"></param>
        private void parseSystem(List<string> sLines)
        {
            FISOperatorAnd andOp = FISOperatorAnd.FISOpAnd_None;
            FISOperatorOr orOp = FISOperatorOr.FISOpOr_None;
            FISImplicator imp = FISImplicator.FISImp_None;
            FISAggregator agg = FISAggregator.FISAgg_None;
            FISDefuzzifier defuzz = FISDefuzzifier.FISDefuzz_None;

            foreach (string line in sLines)
            {
                string[] sLSplit = line.Split('=');
                sLSplit[0] = sLSplit[0].ToLower();

                switch (sLSplit[0])
                {
                    case "numinputs":
                        if (!Int32.TryParse(sLSplit[1], out _numInputs))
                            throw new Exception("Invalid number of inputs: " + line);
                        break;
                    case "numoutputs":
                        if (!Int32.TryParse(sLSplit[1], out _numOutputs))
                            throw new Exception("Invalid number of outputs: " + line);
                        break;
                    case "numrules":
                        if (!Int32.TryParse(sLSplit[1], out _numRules))
                            throw new Exception("Invalid number of rules: " + line);
                        break;
                    case "andmethod":
                        if ("'min'" == sLSplit[1])
                            andOp = FISOperatorAnd.FISOpAnd_Min;
                        else if ("'product'" == sLSplit[1])
                            andOp = FISOperatorAnd.FISOpAnd_Product;
                        else
                            throw new Exception("Unsupported AndMethod: " + line);
                        break;
                    case "ormethod":
                        if ("'max'" == sLSplit[1])
                            orOp = FISOperatorOr.FISOpOr_Max;
                        else if ("'probor'" == sLSplit[1])
                            orOp = FISOperatorOr.FISOpOr_Probor;
                        else
                            throw new Exception("Unsupported OrMethod: " + line);
                        break;
                    case "impmethod":
                        if ("'min'" == sLSplit[1])
                            imp = FISImplicator.FISImp_Min;
                        else if ("'product'" == sLSplit[1])
                            imp = FISImplicator.FISImp_Product;
                        else
                            throw new Exception("Unsupported ImplicatorMethod: " + line);
                        break;
                    case "aggmethod":
                        if ("'max'" == sLSplit[1])
                            agg = FISAggregator.FISAgg_Max;
                        else if ("'probor'" == sLSplit[1])
                            agg = FISAggregator.FISAgg_Probor;
                        else if ("'sum'" == sLSplit[1])
                            agg = FISAggregator.FISAgg_Sum;
                        else
                            throw new Exception("Unsupported Aggregator Method: " + line);
                        break;
                    case "defuzzmethod":
                        if ("'centroid'" == sLSplit[1])
                            defuzz = FISDefuzzifier.FISDefuzz_Centroid;
                        else if ("'bisect'" == sLSplit[1])
                            defuzz = FISDefuzzifier.FISDefuzz_Bisect;
                        else if ("'midmax'" == sLSplit[1])
                            defuzz = FISDefuzzifier.FISDefuzz_MidMax;
                        else if ("'largemax'" == sLSplit[1])
                            defuzz = FISDefuzzifier.FISDefuzz_LargeMax;
                        else if ("'smallmax'" == sLSplit[1])
                            defuzz = FISDefuzzifier.FISDefuzz_SmallMax;
                        else
                            throw new Exception("Unsupported DefuzzMethod: " + line);
                        break;
                }
            }

            if (andOp == FISOperatorAnd.FISOpAnd_None)
                throw new Exception("No AndMethod set.");
            else if (orOp == FISOperatorOr.FISOpOr_None)
                throw new Exception("No OrMethod set.");
            else if (imp == FISImplicator.FISImp_None)
                throw new Exception("No ImpMethod set.");
            else if (agg == FISAggregator.FISAgg_None)
                throw new Exception("No AggMethod set.");
            else if (defuzz == FISDefuzzifier.FISDefuzz_None)
                throw new Exception("No DefuzzMethod set.");

            // Initialize our new ruleset
            ruleset = new RuleSet(andOp, orOp, imp, agg, defuzz);
        }



        /// <summary>
        /// Parse a Matlab Fuzzy Toolbox .fis file for a rule set.
        /// </summary>
        /// <param name="fn">The filename of the file to parse.</param>
        /// <returns></returns>
        private void parseFile(FileInfo inputFn)
        {
            bool systemOK = false;
            bool inputOK = false;
            bool outputOK = false;
            bool rulesOK = false;

            if (!inputFn.Exists)
                throw new FileNotFoundException("Histogram file could not be found", inputFn.FullName);

            List<string> sLines = new List<string>();
            using (var reader = new StreamReader(inputFn.FullName))
            {
                while (!reader.EndOfStream)
                    sLines.Add(reader.ReadLine());
            }
            FISFileSection currSection = FISFileSection.NONE;
            FISFileSection newSection = FISFileSection.NONE;

            // Inputs gets its own 
            List<string> currSectionList = new List<string>();

            foreach (string line in sLines)
            {
                bool bNewSection = false;
                if (regexes[FISFileSection.SYSTEM].IsMatch(line))
                {
                    systemOK = true;
                    bNewSection = true;
                    newSection = FISFileSection.SYSTEM;
                }
                else if (regexes[FISFileSection.INPUT].IsMatch(line))
                {
                    inputOK = true;
                    bNewSection = true;
                    newSection = FISFileSection.INPUT;
                }
                else if (regexes[FISFileSection.OUTPUT].IsMatch(line))
                {
                    outputOK = true;
                    bNewSection = true;
                    newSection = FISFileSection.OUTPUT;
                }
                else if (regexes[FISFileSection.RULES].IsMatch(line))
                {
                    rulesOK = true;
                    bNewSection = true;
                    newSection = FISFileSection.RULES;
                }

                // If a section change has been requested then store the current section
                // and move on to the new one
                if (bNewSection)
                {
                    if (!SectionLines.ContainsKey(newSection))
                        SectionLines[newSection] = new List<List<string>>();

                    SectionLines[currSection].Add(currSectionList);
                    currSectionList = new List<string>();
                    currSection = newSection;
                }

                // We ignore blank lines
                if (line.Trim().Length > 0)
                    currSectionList.Add(line);
            }
            // Add the last thing in
            SectionLines[currSection].Add(currSectionList);

            if (!systemOK || SectionLines[FISFileSection.SYSTEM].Count == 0)
                throw new Exception("No [System] section in: " + _fisFileInfo.FullName);

            else if (!inputOK || SectionLines[FISFileSection.INPUT].Count == 0)
                throw new Exception("No [Input] section in: " + _fisFileInfo.FullName);

            else if (!outputOK || SectionLines[FISFileSection.OUTPUT].Count == 0)
                throw new Exception("No [Output] section in: " + _fisFileInfo.FullName);

            else if (!rulesOK || SectionLines[FISFileSection.RULES].Count == 0)
                throw new Exception("No [Rules] section inL " + _fisFileInfo.FullName);
        }

    }
}
