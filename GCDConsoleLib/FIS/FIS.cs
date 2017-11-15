using System;
using System.Collections.Generic;
using System.IO;

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

    public class FISBase : IDisposable
    {
        List<double> FISCoords;
        List<double> FISValues;

        public FISBase(FileInfo sFISFile)
        {
            _init(sFISFile);
        }
        private void _init(FileInfo sFISFile)
        {

        }
        public void Dispose()
        {

        }


        /// <summary>
        ///  Does the actual processing(in this case, creating a FIS). The i class member variable is the
        ///  index of the current pixel and is set in the processByBlock() method on BaseGCDClass.There
        ///  are two loops -- one for the case of having to check for NODATA in the inputs and one for
        ///  ignoring that possibility.
        /// </summary>
        public void ProcessData()
        {
            //fisData[i] = (float)rules.calculate(inputData, i, checkNoData, inputNoDataValues, noDataValue);
        }

        /// <summary>
        /// Reads the input data. The x and y origin stuff is for the future when we allow the processing
        /// to happen on just part of an image.The offsets and valid variables are class members and are
        /// set in the processByBlock() method on BaseGCDClass.
        /// </summary>
        public void ReadData()
        {

        }

        /// <summary>
        /// Writes the output data.The x and y origin stuff is for the future when we allow the
        /// processing to happen on just part of an image.The offsets and valid variables are class
        /// members and are set in the processByBlock() method on BaseGCDClass.
        /// </summary>
        public void WriteData()
        {

        }
    }
}
