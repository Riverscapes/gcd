using System;
using System.Collections.Generic;
using System.IO;

namespace GCDConsoleLib.FIS
{
    class RuleSet
    {
        private List<MemberFunctionSet> _inputs;
        private Dictionary<String, int> _indices;
        private MemberFunctionSet _output;
        private String _outputName;

        private List<Rule> rules_;
        private List<List<double>> fuzzyInputs_;

        enum FISFileSection { SYSTEM, INPUT, OUTPUT, RULES, NONE };

        private FISOperator _operator;
        private FISOperatorAnd _andOp;
        private FISOperatorOr _orOp;
        private FISImplicator _implicator;
        private FISAggregator _aggregator;
        private FISDefuzzifier _defuzzifier;

        public RuleSet()
        {
        }

        public RuleSet(FISOperatorAnd andOperator, FISOperatorOr orOperator, FISImplicator implicator,
                      FISAggregator aggregator, FISDefuzzifier defuzzifier)
        {
            _andOp = andOperator;
            _orOp = orOperator;
            _implicator = implicator;
            _aggregator = aggregator;
            _defuzzifier = defuzzifier;
        }




        /// <summary>
        /// Add a membership function to a rule.
        /// NOTE: THiS DOESN'T GET USED
        /// </summary>
        /// <param name="rule">The rule to add the membership function to.</param>
        /// <param name="i">The index of the input variable that the MF corresponds to.</param>
        /// <param name="sName">The name of the membership function</param>
        private void addMFToRule(Rule rule, int i, String sName)
        {
            if (!_inputs[i].Indices.ContainsKey(sName))
                throw new ArgumentException("There is no membership function named '" + sName + "'.");
            else
            {
                if (sName != null && "NULL" != sName)
                {
                    rule.Inputs.Add(i);
                    rule.MFSInd.Add(_inputs[i].Indices[sName]);
                }
            }
        }


        /// <summary>
        /// Add a set of membership functions that make up an input variable.
        /// </summary>
        /// <param name="sName">The name of the input variable. Cannot contain spaces.</param>
        /// <param name="mfs">The membership function set.</param>
        /// <returns></returns>
        public void addInputMFSet(String sName, MemberFunctionSet mfs)
        {
            if (_indices.ContainsKey(sName))
                throw new ArgumentException(String.Format("The name '{0}' is already in use.", sName));
            else if (sName.Contains(" "))
                throw new ArgumentException(String.Format("Invalid name '{0}'. Spaces are not allowed.", sName));
            else
            {
                _inputs.Add(mfs);
                _indices[sName] = _inputs.Count;
            }
        }

        /// <summary>
        /// Add a set of membership functions that make up the output variable.
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="mfs"></param>
        public void addOutputMFSet(String sName, MemberFunctionSet mfs)
        {
            _output = mfs;
            _outputName = sName;
        }

        /// <summary>
        /// Add a rule.
        /// NOTE::: THIS DOESN't EVER SEEM To BE USED
        /// The inputs MUST be in the same order as they were added to the Rule Set. Use "NULL" to denote that
        /// a specific input isn't need for this rule.
        /// </summary>
        /// <param name="sInputs">The relevant member function names for each input, separated by spaces.</param>
        /// <param name="output">The member function name for the output.</param>
        /// <param name="op">The operator function to use.</param>
        /// <param name="weight">The rule weight (optional, defaults to 1).</param>
        public void addRule(String sInputs, String sOut, FISOperator op, double weight = 1)
        {
            if (weight < 0 || weight > 1)
                throw new ArgumentException("The weight must be between 0 and 1.");
            else if (!_output.Indices.ContainsKey(sOut))
                throw new ArgumentException(String.Format("There is no output membership function named '{0}'.", sOut));
            else
            {
                String name = sInputs;
                String inputStr = sInputs;
                Rule rule = new Rule();

                rule.Weight = weight;
                rule.Output = _output._mfs[_output.Indices[sOut]];

                _operator = op;

                string[] InputsSubStr = sInputs.Split(); // no arg == space

                for (int i = 0; i < _inputs.Count; i++)
                {
                    if (i >= InputsSubStr.Length)
                        throw new ArgumentException("There are not enough membership functions provided for the rule.");
                    else
                    {
                        name = InputsSubStr[i];
                        addMFToRule(rule, i, name);
                    }
                }
                rules_.Add(rule);
            }
        }

        /// <summary>
        /// Calculate a crisp output based on a set of inputs to this rule set.
        /// </summary>
        /// <param name="lInputs">Inputs The list of input values. These MUST be in the same order that the input variables</param>
        /// <returns></returns>
        public double calculate(List<double> lInputs)
        {
            if (lInputs.Count != _inputs.Count)
                return -1;

            int idx, jdx;

            for (idx = 0; idx < _inputs.Count; idx++)
                for (jdx = 0; jdx < _inputs[idx].Length; jdx++)
                    fuzzyInputs_[idx][jdx] = _inputs[idx]._mfs[jdx].fuzzify(lInputs[idx]);

            MemberFunction impMf = new MemberFunction();
            MemberFunction aggMf = new MemberFunction();

            // Loop over the rules and run the aggregator and implicators for each.
            for (int r = 0; r < rules_.Count; r++)
            {
                Rule rule = rules_[r];
                double impValue = getFuzzyVal(rule, 0);
                // Loop over rule items (probably same as number of inputs but not
                // if some are 0 value
                for (int i = 1; i < rule.Inputs.Count; i++)
                {
                    // For now this is "and" or "or"
                    double fuzzyInput = getFuzzyVal(rule, i);
                    impValue = RuleOperator(impValue, fuzzyInput);
                }
                ImplicatorOp(ref rule.Output, ref impMf, impValue, rule.Weight);
                AggregatorOp(ref impMf, ref aggMf);
            }

            // The defuzzifier is where composite shape gets reduced
            // to a single number
            return DefuzzifierOp(ref aggMf);
        }

        /// <summary>
        /// Apply a FIS to a set of arrays.
        /// All data sets are assumed to be doubleing point. The data sets are referenced in the DoDData object
        /// with the keywords "old", "new", and "dod". ****** UPDATE *******
        /// </summary>
        /// <param name="dataArrays"></param>
        /// <param name="n"></param>
        /// <param name="checkNoData"></param>
        /// <param name="noDataValues"></param>
        /// <param name="noData"></param>
        /// <returns></returns>
        public double calculate(List<float[]> dataArrays, int n, bool checkNoData,
                           List<float> noDataValues, float noData)
        {
            MemberFunction impMf = new MemberFunction();
            MemberFunction aggMf = new MemberFunction();
            Rule rule;
            double impValue;
            float v;
            if (checkNoData)
            {
                bool ok = true;
                for (int i = 0; i < _inputs.Count; i++)
                {
                    v = dataArrays[i][n];
                    if (v == noDataValues[i])
                    {
                        ok = false;
                        break;
                    }
                    for (int j = 0; j < _inputs[i].Length; j++)
                        fuzzyInputs_[i][j] = _inputs[i]._mfs[j].fuzzify(v);
                }
                if (ok)
                {
                    for (int r = 0; r < rules_.Count; r++)
                    {
                        rule = rules_[r];
                        // This is where the NOT calculation happens.
                        impValue = fuzzyInputs_[rule.Inputs[0]][rule.MFSInd[0]];
                        for (int m = 1; m < rule.Inputs.Count; m++)
                            impValue = RuleOperator(impValue, fuzzyInputs_[rule.Inputs[m]][rule.MFSInd[m]]);
                        ImplicatorOp(ref rule.Output, ref impMf, impValue, rule.Weight);
                        AggregatorOp(ref impMf, ref aggMf);
                    }
                    return DefuzzifierOp(ref aggMf);
                }
                else
                    return noData;
            }
            else
            {
                for (int i = 0; i < _inputs.Count; i++)
                {
                    v = dataArrays[i][n];
                    for (int j = 0; j < _inputs[i]._mfs.Count; j++)
                        fuzzyInputs_[i][j] = _inputs[i]._mfs[j].fuzzify(v);
                }
                for (int r = 0; r < rules_.Count; r++)
                {
                    rule = rules_[r];
                    // This is where the NOT calculation happens.
                    impValue = fuzzyInputs_[rule.Inputs[0]][rule.MFSInd[0]];
                    for (int m = 1; m < rule.Inputs.Count; m++)
                        impValue = RuleOperator(impValue, fuzzyInputs_[rule.Inputs[m]][rule.MFSInd[m]]);
                    ImplicatorOp(ref rule.Output, ref impMf, impValue, rule.Weight);
                    AggregatorOp(ref impMf, ref aggMf);
                }
                return DefuzzifierOp(ref aggMf);
            }
        }

        /// <summary>
        /// Get the name of the input variable at the specified index.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public String getInputName(int i)
        {
            foreach (KeyValuePair<String, int> it in _indices)
            {
                if (it.Value == i)
                    return it.Key;
            }
            return "";
        }

        /// <summary>
        /// Parse a Matlab Fuzzy Toolbox .fis file for a rule set.
        /// </summary>
        /// <param name="fn">The filename of the file to parse.</param>
        /// <returns></returns>
        public void parseFile(FileInfo fn)
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
                { FISFileSection.INPUT, new List<string>() },
                { FISFileSection.OUTPUT, new List<string>() },
                { FISFileSection.RULES, new List<string>() },
                { FISFileSection.NONE, new List<string>() }
            };

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
                else if(line.Trim().Length > 0)
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
                        int numInputs;
                        if (!Int32.TryParse(sLSplit[1], out numInputs))
                            throw new Exception("Invalid number of inputs: " + line);
                        break;
                    case "numoutputs":
                        int numOutputs;
                        if (!Int32.TryParse(sLSplit[1], out numOutputs))
                            throw new Exception("Invalid number of outputs: " + line);
                        break;
                    case "numrules":
                        int numRules;
                        if (!Int32.TryParse(sLSplit[1], out numRules))
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

            //init(andOp, orOp, imp, agg, defuzz);

        }

        /// <summary>
        /// Add a set of membership functions that make up an input variable.
        /// Unlike addInputMFSet, this assumes allows the user to choose the index of the input set, but that
        /// index must already exist.
        /// </summary>
        /// <param name="index">The index of the input variable. The FISRuleSet must already know that there are at least that many variables (it does not automatically increment).</param>
        /// <param name="name">The name of the input variable. Cannot contain spaces.</param>
        /// <param name="mfs">The membership function set.</param>
        /// <returns>True of the set could be added, false otherwise</returns>
        public void setInputMFSet(int index, String sName, MemberFunctionSet mfs)
        {
            if (0 > index || _inputs.Count <= index)
                throw new ArgumentException(String.Format("Invalid index for input: {0}", index));
            else if (_indices.ContainsKey(sName))
                throw new ArgumentException(String.Format("The name '{0}' is already in use.", sName));
            else if (sName.Contains(" "))
                throw new ArgumentException(String.Format("Invalid name '{0}'. Spaces are not allowed.", sName));
            else
            {
                _inputs[index] = mfs;
                _indices[sName] = index;
            }
        }

        /// <summary>
        /// Check whether this ruleset follows the rules.
        /// </summary>
        /// <returns></returns>
        public bool valid()
        {
            if (_inputs.Count == 0)
                //"No FIS inputs."
                return false;
            else if (rules_.Count == 0)
                //"No FIS rules."
                return false;
            for (int i = 0; i < _inputs.Count; i++)
                if (!_inputs[i].valid())
                    //"Invalid input number " + stringify(i) + "."
                    return false;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="ruleItemind"></param>
        /// <returns></returns>
        public double getFuzzyVal(Rule rule, int ruleItemInd)
        {
            int inputInd = rule.Inputs[ruleItemInd];
            int mfsInd = rule.MFSInd[ruleItemInd];
            double fuzzyInput = fuzzyInputs_[inputInd][mfsInd];

            if (rule.MFSNot[ruleItemInd] == 1)
            {
                double yMax = _inputs[inputInd]._mfs[mfsInd].yMax;
                fuzzyInput = yMax - fuzzyInput;
            }
            return fuzzyInput;
        }


        /// <summary>
        /// Choose between and and or operations
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        private double RuleOperator(double d1, double d2)
        {
            switch (_operator)
            {
                case FISOperator.FISOp_And:
                    return OrOp(d1, d2);
                case FISOperator.FISOp_Or:
                    return AndOp(d1, d2);
                default:
                    throw new ArgumentException("Invalid operator");
            }
        }

        /// <summary>
        /// Generalized Or Operator
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        private double OrOp(double d1, double d2)
        {
            switch (_orOp)
            {
                case FISOperatorOr.FISOpOr_Max:
                    return FISOperators.Max(d1, d2);
                case FISOperatorOr.FISOpOr_Probor:
                    return FISOperators.ProbOr(d1, d2);
                default:
                    throw new ArgumentException("Invalid operator");
            }
        }

        /// <summary>
        /// Generalized And Operator
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        private double AndOp(double d1, double d2)
        {
            switch (_andOp)
            {
                case FISOperatorAnd.FISOpAnd_Min:
                    return FISOperators.Min(d1, d2);
                case FISOperatorAnd.FISOpAnd_Product:
                    return FISOperators.Product(d1, d2);
                default:
                    throw new ArgumentException("Invalid operator");
            }
        }

        /// <summary>
        /// Generalized Implicator
        /// </summary>
        /// <param name="inMf"></param>
        /// <param name="outMf"></param>
        /// <param name="n"></param>
        /// <param name="weight"></param>
        private void ImplicatorOp(ref MemberFunction inMf,
            ref MemberFunction outMf,
            double n, double weight)
        {
            switch (_implicator)
            {
                case FISImplicator.FISImp_Min:
                //return FISOperators.Min(d1, d2);
                case FISImplicator.FISImp_Product:
                //return FISOperators.Product(d1, d2);
                default:
                    throw new ArgumentException("Invalid operator");
            }
        }

        /// <summary>
        /// Generalized Aggregator
        /// </summary>
        /// <param name="inMf"></param>
        /// <param name="outMf"></param>
        private void AggregatorOp(ref MemberFunction inMf, ref MemberFunction outMf)
        {
            switch (_aggregator)
            {
                case FISAggregator.FISAgg_Max:
                //return FISOperators.Min(d1, d2);
                case FISAggregator.FISAgg_Probor:
                //return FISOperators.Product(d1, d2);
                case FISAggregator.FISAgg_Sum:
                default:
                    throw new ArgumentException("Invalid operator");
            }
        }

        /// <summary>
        /// Generalized Defizzifier
        /// </summary>
        /// <param name="mf"></param>
        private double DefuzzifierOp(ref MemberFunction mf)
        {
            switch (_defuzzifier)
            {
                case FISDefuzzifier.FISDefuzz_Bisect:
                    return Defuzzify.FISDefuzzBisect(ref mf);
                case FISDefuzzifier.FISDefuzz_Centroid:
                    return Defuzzify.DefuzzCentroid(ref mf);
                case FISDefuzzifier.FISDefuzz_LargeMax:
                    return Defuzzify.FISDefuzzLargeMax(ref mf);
                case FISDefuzzifier.FISDefuzz_MidMax:
                    return Defuzzify.FISDefuzzMidMax(ref mf);
                case FISDefuzzifier.FISDefuzz_SmallMax:
                    return Defuzzify.FISDefuzzSmallMax(ref mf);
                default:
                    throw new ArgumentException("Invalid operator");
            }
        }
    }
}
