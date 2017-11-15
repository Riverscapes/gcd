using System;
using System.Collections.Generic;
using System.IO;

namespace GCDConsoleLib.FIS
{
    public class FisFile
    {
        private FileInfo fn;
        private int _numInputs, _numOutputs, _numRules;

        enum FISFileSection { SYSTEM, INPUT, OUTPUT, RULES, NONE };

        public RuleSet ruleset;

        /// <summary>
        /// Constructor and loader all in one
        /// </summary>
        /// <param name="inputFn"></param>
        public FisFile(FileInfo inputFn)
        {
            fn = inputFn;
            _numInputs = 0;
            _numOutputs = 0;
            _numRules = 0;
            parseFile();
        }

        /// <summary>
        /// 
        /// </summary>
        private void parseInputOutput(List<string> sLines, bool isInput, int inputNum)
        {
            // TODO: do a thing here.
        }

        private void parseRules(List<string> sLines)
        {
            // TODO:  do another thing here.
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
                String lowerLine = line.ToLower();
                string[] sLSplit = lowerLine.Split('=');

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
        public void parseFile()
        {
            bool systemOK = false;
            bool inputOK = false;
            bool outputOK = false;
            bool rulesOK = false;

            if (!fn.Exists)
                throw new FileNotFoundException("Histogram file could not be found", fn.FullName);

            List<string> sLines = new List<string>();
            using (var reader = new StreamReader(fn.FullName))
            {
                while (!reader.EndOfStream)
                    sLines.Add(reader.ReadLine());
            }
            FISFileSection currSection = FISFileSection.NONE;

            // Store each section in its own dictionary list
            Dictionary<FISFileSection, List<string>> sectionLines = new Dictionary<FISFileSection, List<string>>() {
                { FISFileSection.SYSTEM, new List<string>() },
                { FISFileSection.OUTPUT, new List<string>() },
                { FISFileSection.RULES, new List<string>() },
                { FISFileSection.NONE, new List<string>() }
            };

            // Inputs gets its own 
            List<List<string>> Inputs = new List<List<string>>();

            foreach (string line in sLines)
            {
                if (line.Substring(0, 8) == "[System]")
                {
                    systemOK = true;
                    currSection = FISFileSection.SYSTEM;
                }
                else if (line.Substring(0, 6) == "[Input")
                {
                    inputOK = true;
                    currSection = FISFileSection.INPUT;
                }
                else if (line.Substring(0, 7) == "[Output")
                {
                    outputOK = true;
                    currSection = FISFileSection.OUTPUT;
                }
                else if (line.Substring(0, 7) == "[Rules]")
                {
                    rulesOK = true;
                    currSection = FISFileSection.RULES;
                }
                else if (line.Trim().Length > 0)
                {
                    sectionLines[currSection].Add(line);
                }
            }

            if (!systemOK)
                throw new Exception("No [System] section in: " + fn.FullName);
            else if (!inputOK)
                throw new Exception("No [Input] section in: " + fn.FullName);
            else if (!outputOK)
                throw new Exception("No [Output] section in: " + fn.FullName);
            else if (!rulesOK)
                throw new Exception("No [Rules] section inL " + fn.FullName);

            parseSystem(sectionLines[FISFileSection.SYSTEM]);
            parseInputOutput(sectionLines[FISFileSection.INPUT], true, 999);
            parseInputOutput(sectionLines[FISFileSection.OUTPUT], false, 999);
            parseRules(sectionLines[FISFileSection.RULES]);

        }



    }
}
