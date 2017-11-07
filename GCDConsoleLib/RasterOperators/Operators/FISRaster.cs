using System;
using System.IO;
using System.Collections.Generic;
using GCDConsoleLib.Internal;
using GCDConsoleLib.GCD;

namespace GCDConsoleLib.Internal.Operators
{

    public class FISRaster : CellByCellOperator<float>
    {

        /// <summary>
        /// Pass-through constructure
        /// </summary>
        public FISRaster(List<string> inputNames, List<Raster> rInputs, FileInfo fisFile, Raster rOutput) :
            base(rInputs, rOutput)
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
            FIS.RuleSet theRuleSet = new FIS.RuleSet();
            theRuleSet.parseFile(fisFile);

            //// Confirm that the number of inputs specified matches the number in the rule file
            //if (theRuleSet.numInputs() != (int)dFISVals.size())
            //    return GCD::INCORRECT_NUMBER_FIS_INPUTS;

            //*dResult = theRuleSet.calculate(dFISVals);
        }

        /// <summary>
        /// This is the actual implementation of the cell-by-cell logic
        /// </summary>
        protected override float CellOp(ref List<float[]> data, int id)
        {
            // We need to return something. Doesn't matter what
            return 0;
        }

    }
}
