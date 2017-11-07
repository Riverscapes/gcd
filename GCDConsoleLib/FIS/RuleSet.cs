using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDConsoleLib.FIS
{
    class RuleSet
    {
        private int _nInputs_, _nRules;
        List<MemberFunctionSet> _inputs;
        Dictionary<String, int> _indices;
        MemberFunctionSet _output;
        String _outputName;
        String _msg;

        List<Rule> rules_;
        List<List<double>> fuzzyInputs_;

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
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public String getInputName(int i)
        {
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String getMsg()
        {
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int numInputs()
        {
            return 0;
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
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="name"></param>
        /// <param name="mfs"></param>
        /// <returns></returns>
        public bool setInputMFSet(int index, String name, MemberFunctionSet mfs)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool valid()
        {
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
