using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib;

namespace GCDConsoleTest.Helpers
{
    public struct RasterTests
    {
        /// <summary>
        /// A Quick way to make sure a raster file is cell-by-cell the same as a second one.
        /// </summary>
        /// <param name="rTest"></param>
        /// <param name="rTruth"></param>
        public static void RasterCompare(Raster rTest, Raster rTruth)
        {
            List<string> errs = new List<string> { };

            if (!rTest.IsDivisible()) errs.Add("Raster is not divisible");
            else if (!rTest.IsOrthogonal(rTruth)) errs.Add("Raster is not orthogonal to truth raster");
            else if (!rTest.IsConcurrent(rTruth)) errs.Add("Raster is not concurrent with truth raster");

            if (!rTest.Proj.IsSame(rTruth.Proj)) errs.Add("Raster has unexpected Projection");

            if (rTest.HasNodata != rTruth.HasNodata) errs.Add("Raster has mismatched nodata values");
            else if (rTest.HasNodata && !rTest.origNodataVal.Equals(rTest.origNodataVal)) errs.Add("Raster has incorrect NodataValue");

            int diff = 0;
            double sum = 0;

            for (int idx = 0; idx < rTest.Extent.Rows; idx++)
            {
                try
                {
                    double[] test_buff = new double[rTest.Extent.Cols];
                    double[] truth_buff = new double[rTest.Extent.Cols];
                    rTest.Read(0, idx, rTest.Extent.Cols, 1, test_buff);
                    rTruth.Read(0, idx, rTest.Extent.Cols, 1, truth_buff);

                    for(int idy = 0; idy < rTest.Extent.Cols; idy++)
                    {
                        if (test_buff[idy] != truth_buff[idy])
                        {
                            diff++;
                            sum += test_buff[idy] - truth_buff[idy];
                        }
                    }
                }
                catch (Exception e) {
                    errs.Add("Raster encountered critical read error while verifying cells.");
                    break;
                }
            }

            if (diff > 0) errs.Add(String.Format("Raster had {0} cells that were different than expected summing to {1:N1} total difference", diff, sum));

            if (errs.Count > 0)
            {
                string errMsg = String.Join(System.Environment.NewLine, errs);
                Assert.Fail(errMsg);
            }
        }
    }
}
