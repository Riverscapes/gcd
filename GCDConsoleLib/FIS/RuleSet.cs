using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GCDConsoleLib.Extensions;

namespace GCDConsoleLib.FIS
{
    public enum FISOperatorAnd { FISOpAnd_Min, FISOpAnd_Product, FISOpAnd_None };
    public enum FISOperatorOr { FISOpOr_Max, FISOpOr_Probor, FISOpOr_None };
    public enum FISOperator { FISOp_And, FISOp_Or, FISOp_None };
    public enum FISImplicator { FISImp_Min, FISImp_Product, FISImp_None };
    public enum FISAggregator { FISAgg_Max, FISAgg_Probor, FISAgg_Sum, FISAgg_None };
    public enum FISDefuzzifier
    {
        FISDefuzz_Centroid, FISDefuzz_Bisect, FISDefuzz_MidMax, FISDefuzz_LargeMax,
        FISDefuzz_SmallMax, FISDefuzz_None
    };

    public class RuleSet
    {
        public Dictionary<string, MemberFunctionSet> Inputs;
        public Map<int, string> InputLookupMap;

        // In GCD there is only ever one but FIS technically allows for more
        public MemberFunctionSet Outputs;
        public List<Rule> Rules;

        public String OutputName;

        private FISOperatorAnd _andOp;
        private FISOperatorOr _orOp;
        private FISImplicator _implicator;
        private FISAggregator _aggregator;
        private FISDefuzzifier _defuzzifier;

        public RuleSet()
        {
            _init();
        }

        public RuleSet(FISOperatorAnd andOperator, FISOperatorOr orOperator, FISImplicator implicator,
                      FISAggregator aggregator, FISDefuzzifier defuzzifier)
        {
            _andOp = andOperator;
            _orOp = orOperator;
            _implicator = implicator;
            _aggregator = aggregator;
            _defuzzifier = defuzzifier;

            _init();
        }

        private void _init()
        {
            Inputs = new Dictionary<string, MemberFunctionSet>();
            InputLookupMap = new Map<int, string>();
            Rules = new List<Rule>();
            Outputs = new MemberFunctionSet();
            OutputName = "";
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
            string inputName = InputLookupMap.Forward[i];
            if (!Inputs[inputName].Indices.ContainsKey(sName))
                throw new ArgumentException("There is no membership function named '" + sName + "'.");
            else
            {
                if (sName != null && "NULL" != sName)
                {
                    rule.InputInd.Add(i);
                    rule.MFSInd.Add(Inputs[inputName].Indices[sName]);
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
            if (Inputs.ContainsKey(sName))
                throw new ArgumentException(string.Format("The name '{0}' is already in use.", sName));
            else if (sName.Contains(" "))
                throw new ArgumentException(string.Format("Invalid name '{0}'. Spaces are not allowed.", sName));
            else
            {
                Inputs[sName] = mfs;
                InputLookupMap.Add(Inputs.Count - 1, sName);
            }
        }

        /// <summary>
        /// Add a set of membership functions that make up the output variable.
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="mfs"></param>
        public void addOutputMFSet(String sName, MemberFunctionSet mfs)
        {
            Outputs = mfs;
            OutputName = sName;
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
            else if (!Outputs.Indices.ContainsKey(sOut))
                throw new ArgumentException(string.Format("There is no output membership function named '{0}'.", sOut));
            else
            {
                String name = sInputs;
                String inputStr = sInputs;
                Rule rule = new Rule();

                rule.Weight = weight;
                rule.Output = Outputs.MFunctions[Outputs.Indices[sOut]];

                rule.Operator = op;

                string[] InputsSubStr = sInputs.Split(); // no arg == space

                for (int i = 0; i < Inputs.Count; i++)
                {
                    if (i >= InputsSubStr.Length)
                        throw new ArgumentException("There are not enough membership functions provided for the rule.");
                    else
                    {
                        name = InputsSubStr[i];
                        addMFToRule(rule, i, name);
                    }
                }
                Rules.Add(rule);
            }
        }

        /// <summary>
        /// Calculate a crisp output based on a set of inputs to this rule set.
        /// </summary>
        /// <param name="lInputs">Inputs The list of input values. These MUST be in the same order that the input variables</param>
        /// <returns></returns>
        public double calculate(List<double> lInputs, bool debug = false)
        {
            List<List<double>> _fuzzyInputs = new List<List<double>>();
            if (lInputs.Count != Inputs.Count)
                return -1;

            int idx, jdx;

            for (idx = 0; idx < Inputs.Count; idx++)
            {
                string inputName = InputLookupMap.Forward[idx];
                List<double> _fin = new List<double>();
                for (jdx = 0; jdx < Inputs[inputName].Count; jdx++)
                    _fin.Add(Inputs[inputName].MFunctions[jdx].fuzzify(lInputs[idx]));
                _fuzzyInputs.Add(_fin);
            }

            if (debug == true)
            {
                Debug.WriteLine("Debug=================");
                _fuzzyInputs.ForEach(x => Debug.WriteLine(String.Format("INPUT: Fuzzy: ({0})", String.Join(", ", x))));
            }

            MemberFunction impMf = new MemberFunction();
            MemberFunction aggMf = new MemberFunction();

            // Loop over the rules and run the aggregator and implicators for each.
            for (int r = 0; r < Rules.Count; r++)
            {
                Rule rule = Rules[r];
                double impValue = getFuzzyVal(rule, 0, _fuzzyInputs);
                // Loop over rule items (probably same as number of inputs but not
                // if some are 0 value
                for (int i = 1; i < rule.InputInd.Count; i++)
                {
                    // For now this is "and" or "or"
                    double fuzzyInput = getFuzzyVal(rule, i, _fuzzyInputs);
                    impValue = RuleOperator(rule, impValue, fuzzyInput);
                }
                impMf = ImplicatorOp(rule.Output, impValue, rule.Weight);
                AggregatorOp(impMf, aggMf);

                if (debug == true)
                {
                    List<string> coords = new List<string>();
                    impMf.Coords.ForEach(x => coords.Add(String.Format("({0})", String.Join(", ", x))));
                    Debug.WriteLine(String.Format("IMP Coords Rule{1}: {0}", String.Join(" ", coords), r));
                }

            }

            double defuzzed = DefuzzifierOp(aggMf);

            if (debug == true)
            {
                List<string> coords = new List<string>();
                aggMf.Coords.ForEach(x => coords.Add(String.Format("({0})", String.Join(", ", x))));
                Debug.WriteLine(String.Format("Aggregated Coords: {0}", String.Join(" ", coords)));

                Debug.WriteLine(String.Format("Defuzzified: {0}", DefuzzifierOp(aggMf)));
                Debug.WriteLine("ENDDebug=================");
            }

            // The defuzzifier is where composite shape gets reduced
            // to a single number
            return defuzzed;
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
        public double calculate(List<double[]> dataArrays, int n, bool checkNoData,
                           List<double> noDataValues, double noData)
        {
            List<List<double>> _fuzzyInputs = new List<List<double>>();
            MemberFunction impMf = new MemberFunction();
            MemberFunction aggMf = new MemberFunction();
            Rule rule;
            double impValue;
            double v;
            if (checkNoData)
            {
                bool ok = true;
                for (int i = 0; i < Inputs.Count; i++)
                {
                    v = dataArrays[i][n];
                    if (v == noDataValues[i])
                    {
                        ok = false;
                        break;
                    }
                    string inputName = InputLookupMap.Forward[i];
                    List<double> fuzzymflst = new List<double>();
                    for (int j = 0; j < Inputs[inputName].MFunctions.Count; j++)
                        fuzzymflst.Add(Inputs[inputName].MFunctions[j].fuzzify(v));
                    _fuzzyInputs.Add(fuzzymflst);
                }
                if (ok)
                {
                    for (int r = 0; r < Rules.Count; r++)
                    {
                        rule = Rules[r];
                        // This is where the NOT calculation happens. First pick the first fuzzified value
                        impValue = _fuzzyInputs[rule.InputInd[0]][rule.MFSInd[0]];
                        // Now operate 
                        for (int m = 1; m < rule.InputInd.Count; m++)
                            impValue = RuleOperator(rule, impValue, _fuzzyInputs[rule.InputInd[m]][rule.MFSInd[m]]);
                        impMf = ImplicatorOp(rule.Output, impValue, rule.Weight);
                        AggregatorOp(impMf, aggMf);
                    }
                    return DefuzzifierOp(aggMf);
                }
                else
                    return noData;
            }
            else
            {
                for (int i = 0; i < Inputs.Count; i++)
                {
                    v = dataArrays[i][n];
                    string inputName = InputLookupMap.Forward[i];
                    for (int j = 0; j < Inputs[inputName].MFunctions.Count; j++)
                        _fuzzyInputs[i][j] = Inputs[inputName].MFunctions[j].fuzzify(v);
                }
                for (int r = 0; r < Rules.Count; r++)
                {
                    rule = Rules[r];
                    // This is where the NOT calculation happens.
                    impValue = _fuzzyInputs[rule.InputInd[0]][rule.MFSInd[0]];
                    for (int m = 1; m < rule.InputInd.Count; m++)
                        impValue = RuleOperator(rule, impValue, _fuzzyInputs[rule.InputInd[m]][rule.MFSInd[m]]);
                    impMf = ImplicatorOp(rule.Output, impValue, rule.Weight);
                    AggregatorOp(impMf, aggMf);
                }
                return DefuzzifierOp(aggMf);
            }
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
            if (Inputs.ContainsKey(sName))
                throw new ArgumentException(string.Format("The name '{0}' is already in use.", sName));
            else if (sName.Contains(" "))
                throw new ArgumentException(string.Format("Invalid name '{0}'. Spaces are not allowed.", sName));
            else
            {
                Inputs[sName] = mfs;
                InputLookupMap.Forward[index] = sName;
            }
        }

        /// <summary>
        /// Check whether this ruleset follows the rules.
        /// </summary>
        /// <returns></returns>
        public bool Valid
        {
            get
            {
                if (Inputs.Count == 0)
                    //"No FIS inputs."
                    return false;
                else if (Rules.Count == 0)
                    //"No FIS rules."
                    return false;
                for (int i = 0; i < Inputs.Count; i++)
                {
                    string inputName = InputLookupMap.Forward[i];
                    if (!Inputs[inputName].Valid)
                        //"Invalid input number " + stringify(i) + "."
                        return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="ruleItemind"></param>
        /// <returns></returns>
        public double getFuzzyVal(Rule rule, int ruleItemInd, List<List<double>> fuzzyInputs)
        {
            int inputInd = rule.InputInd[ruleItemInd];
            int mfsInd = rule.MFSInd[ruleItemInd];
            double fuzzyInput = fuzzyInputs[inputInd][mfsInd];

            // Apply the NOT operator if we need to
            if (rule.MFSNot[ruleItemInd] == true)
            {
                string inputName = InputLookupMap.Forward[inputInd];
                double yMax = Inputs[inputName].MFunctions[mfsInd].MaxY;
                fuzzyInput = yMax - fuzzyInput;
            }
            return fuzzyInput;
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
        /// Choose between and and or operations
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        private double RuleOperator(Rule theRule, double d1, double d2)
        {
            switch (theRule.Operator)
            {
                case FISOperator.FISOp_And:
                    return AndOp(d1, d2);
                case FISOperator.FISOp_Or:
                    return OrOp(d1, d2);
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
        public MemberFunction ImplicatorOp(MemberFunction inMf, double n, double weight)
        {
            switch (_implicator)
            {
                case FISImplicator.FISImp_Min:
                    return FISOperators.ImpMin(inMf, n, weight);
                case FISImplicator.FISImp_Product:
                    return FISOperators.ImpProduct(inMf, n, weight);
                default:
                    throw new ArgumentException("Invalid operator");
            }
        }

        /// <summary>
        /// Generalized Aggregator
        /// </summary>
        /// <param name="inMf"></param>
        /// <param name="outMf"></param>
        private void AggregatorOp(MemberFunction inMf, MemberFunction outMf)
        {
            switch (_aggregator)
            {
                case FISAggregator.FISAgg_Max:
                    FISOperators.AggMax(inMf, outMf);
                    break;
                //case FISAggregator.FISAgg_Probor:
                //    FISOperators.AggProbor(inMf, outMf);
                //    break;
                case FISAggregator.FISAgg_Sum:
                default:
                    throw new ArgumentException("Invalid operator");
            }
        }

        /// <summary>
        /// Generalized Defizzifier
        /// </summary>
        /// <param name="mf"></param>
        private double DefuzzifierOp(MemberFunction mf)
        {
            switch (_defuzzifier)
            {
                case FISDefuzzifier.FISDefuzz_Bisect:
                    return Defuzzify.DefuzzBisect(mf);
                case FISDefuzzifier.FISDefuzz_Centroid:
                    return Defuzzify.DefuzzCentroid(mf);
                case FISDefuzzifier.FISDefuzz_LargeMax:
                    return Defuzzify.FISDefuzzLargeMax(mf);
                case FISDefuzzifier.FISDefuzz_MidMax:
                    return Defuzzify.FISDefuzzMidMax(mf);
                case FISDefuzzifier.FISDefuzz_SmallMax:
                    return Defuzzify.FISDefuzzSmallMax(mf);
                default:
                    throw new ArgumentException("Invalid operator");
            }
        }
    }
}
