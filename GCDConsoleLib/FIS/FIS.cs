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
}
