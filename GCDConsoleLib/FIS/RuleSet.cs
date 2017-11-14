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
        private String _msg;

        private List<Rule> rules_;
        private List<List<double>> fuzzyInputs_;

        public RuleSet()
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="andOperator"></param>
        /// <param name="orOperator"></param>
        /// <param name="implicator"></param>
        /// <param name="aggregator"></param>
        /// <param name="defuzzifier"></param>
        public RuleSet(FISOperatorAnd andOperator, FISOperatorOr orOperator, FISImplicator implicator,
                              FISAggregator aggregator, FISDefuzzifier defuzzifier)
        {
        }

        bool addMFToRule(Rule rule, int i, String name)
        {
            if (!_inputs[i].Indices.ContainsKey(name))
                throw new ArgumentException("There is no membership function named '" + name + "'.");
            else
            {
                if (name != null && "NULL" != name)
                {
                    rule.Inputs.Add(i);
                    rule.MFSInd.Add(_inputs[i].Indices[name]);
                }
                return true;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="mfs"></param>
        /// <returns></returns>
        public bool addInputMFSet(String sName, MemberFunctionSet mfs)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="mfs"></param>
        public void addOutputMFSet(String sName, MemberFunctionSet mfs)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sInputs"></param>
        /// <param name="output"></param>
        /// <param name="op"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public bool addRule(String sInputs, String output, FISOperator op, double weight = 1)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public double calculate(List<double> inputs)
        {
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public void initFuzzy()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataArrays"></param>
        /// <param name="n"></param>
        /// <param name="checkNoData"></param>
        /// <param name="noDataValues"></param>
        /// <param name="noData"></param>
        /// <returns></returns>
        public double calculate(List<float> dataArrays, int n, bool checkNoData,
                           List<float> noDataValues, float noData)
        {
            return 0;
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
        /// 
        /// </summary>
        /// <param name="fn"></param>
        /// <returns></returns>
        public bool parseFile(FileInfo fn)
        {
            return false;
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
        public bool setInputMFSet(int index, String name, MemberFunctionSet mfs)
        {
    //        if (0 > index || nInputs_ <= index)
    //            return setError("Invalid index for input: " + stringify(index));
    //        else if ((indices_.end() != indices_.find(name)) && (indices_[name] != index))
    //            return setError("The name '" + std::string(name) + "' is already in use.");
    //        else if (std::string::npos != std::string(name).find(' '))
    //    return setError("Invalid name '" + std::string(name) + "'. Spaces are not allowed.");
    //else {
    //            inputs_[index] = mfs;
    //            indices_[name] = index;
    //            return true;
    //        }

            return true;
        }

        /// <summary>
        /// 
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
        public double getFuzzyVal(Rule rule, int ruleItemind)
        {
            return 0;
        }
    }
}
