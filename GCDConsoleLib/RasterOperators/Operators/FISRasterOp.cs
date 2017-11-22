using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using GCDConsoleLib.Internal;
using GCDConsoleLib.GCD;

namespace GCDConsoleLib.Internal.Operators
{

    public class FISRasterOp : CellByCellOperator<double>
    {

        /// <summary>
        /// Pass-through constructure
        /// </summary>
        public FISRasterOp(Dictionary<string, Raster> rInputs, FileInfo fisFile, Raster rOutput) :
            base(rInputs.Values.ToList(), rOutput)
        {

            for (int i = 0; i < rInputs.Count / 2; i++)
            {
                bool ParamIsNumeric;
                //double dFISVal = sInputList.at(i * 2 + 1).toDouble(&ParamIsNumeric);
                //if (!ParamIsNumeric)
                //    return GCD::ERROR_LOADING_FIS_INPUTS;
                //dFISVals.at(i) = dFISVal;
            }

            // Load the FIS rule file

            FIS.FisFile theFisFile = new FIS.FisFile(fisFile);

            FIS.RuleSet theRuleSet = theFisFile.ruleset;
            //// Confirm that the number of inputs specified matches the number in the rule file
            //if (theRuleSet.numInputs() != (int)dFISVals.size())
            //    return GCD::INCORRECT_NUMBER_FIS_INPUTS;

            //*dResult = theRuleSet.calculate(dFISVals);
        }

        /// <summary>
        /// Sometimes we just want the op value
        /// </summary>
        /// <param name="rInputs"></param>
        /// <param name="fisFile"></param>
        public FISRasterOp(Dictionary<string, Raster> rInputs, FileInfo fisFile) :
            base(rInputs.Values.ToList())
        {

        }

        /// <summary>
        /// This is the actual implementation of the cell-by-cell logic
        /// </summary>
        protected override double CellOp(List<double[]> data, int id)
        {
            // We need to return something. Doesn't matter what
            return 0;
        }

        /// <summary>
        /// This is the actual fis logic. We store it here in case it's needed elsewhere
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public float FISOp(List<double[]> data, int id)
        {
            return 0;
        }
    }
}
