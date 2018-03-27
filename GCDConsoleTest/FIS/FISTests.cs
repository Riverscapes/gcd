using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using GCDConsoleTest.Helpers;
using System.Linq;
using System.Diagnostics;

namespace GCDConsoleLib.FIS.Tests
{
    [TestClass()]
    public class FISTests
    {
        public static double ACCEPTABLE_DELTA = 0.021; // 2.1% seems a little arbitrary...and it is
        /// <summary>
        /// This is our verification testing of values from a spreadsheet that was backed up by scikit-fuzzy and matlab and tests a set of values end-to-end
        /// </summary>
        [TestMethod()]
        [TestCategory("Unit")]
        public void FISAcceptanceTest()
        {
            List<double> nodatas = new List<double> { -1, -1, -1, -1, -1, -1, -1, -1, -1 };

            List<double[]> Feshie = new List<double[]>
            {
                // ROWS (from the spreadsheet: https://docs.google.com/spreadsheets/d/1v6abeYaKZXQAyN25VEuN3NvffOTFth0cl1cJ24zNfLE/edit#gid=1712652478): 
                // [0] Input: SlopeDegrees
                // [1] Input: PQ2006
                // [2] Input: PDensity
                // [3] Output: TSZErrorPDSLPdeg
                // [4] Output: GPSZErrorPDSLPdegPQ
                new double[]{ 1.654609323, 3.535005093, 3.949249744, 9.94642067, 10.52396774, 9.118708611, 9.745541573, 6.231749058, 3.356722593 },
                new double[]{ 0.02074607275, 0.02139801346, 0.02041001059, 0.0207916256, 0.01980362274, 0.01993117295, 0.01788944937, 0.01820923202, 0.02010315657 },
                new double[]{ 0.1599999964, 0.1199999973, 0.07999999821, 0.200000003, 0.1199999973, 0.1199999973, 0.2399999946, 0.1199999973, 0.200000003 },
                // These are the GCD6 numbers
                new double[]{ 0.1333, 0.1417, 0.1479, 0.9125, 0.9797, 0.8423, 0.6588, 0.1410, 0.1182 },
                new double[]{ 0.05000000075, 0.1103881523, 0.1230156422, 0.4148696959, 0.4498254061, 0.3266140521, 0.3981432021, 0.1513022482, 0.1058925837 },
            };

            // TEST 1: TSZErrorPDSLPdeg
            // We need just PointDensity and SlopeDegrees
            Debug.WriteLine("TS_ZError_PD_SLPdeg");
            FisFile TS_ZError_PD_SLPdeg = new FisFile(new FileInfo(DirHelpers.GetTestRootPath(@"FIS\TS_ZError_PD_SLPdeg.fis")));
            for (int idx = 0; idx < Feshie[0].Length; idx++)
            {
                // Input order in the fis file is: SlopeDeg, PointDensity
                double result1 = TS_ZError_PD_SLPdeg.ruleset.calculate(new List<double[]> { Feshie[0], Feshie[2] }, idx, true, nodatas, -2);
                double result2 = TS_ZError_PD_SLPdeg.ruleset.calculate(new List<double> { Feshie[0][idx], Feshie[2][idx]});
                Assert.AreEqual(result1, result2);

                // Now verify them against the spreadsheet
                Debug.WriteLine(result1);
                Assert.AreEqual(result1, Feshie[3][idx], Feshie[3][idx] * ACCEPTABLE_DELTA);
            }

            // TEST 2: GPSZErrorPDSLPdegPQ
            // We need just PointDensity and SlopeDegrees
            Debug.WriteLine("GPS_ZError_PD_SLPdeg_PQ");
            FisFile GPS_ZError_PD_SLPdeg_PQ = new FisFile(new FileInfo(DirHelpers.GetTestRootPath(@"FIS\GPS_ZError_PD_SLPdeg_PQ.fis")));

            for (int idx = 0; idx < Feshie[0].Length; idx++)
            {
                // Input order in the fis file is: 3DPointQuality, SlopeDeg, PointDensity
                double result1 = GPS_ZError_PD_SLPdeg_PQ.ruleset.calculate(new List<double[]> { Feshie[1], Feshie[0], Feshie[2] }, idx, true, nodatas, -2);
                double result2 = GPS_ZError_PD_SLPdeg_PQ.ruleset.calculate(new List<double> { Feshie[1][idx], Feshie[0][idx], Feshie[2][idx] });
                Assert.AreEqual(result1, result2);

                // Now verify them against the spreadsheet
                Debug.WriteLine(result1);
                Assert.AreEqual(result1, Feshie[4][idx], Feshie[4][idx] * ACCEPTABLE_DELTA);
            }


            List<double[]> UGR = new List<double[]>
            {
                // ROWS (from the spreadsheet: https://docs.google.com/spreadsheets/d/1v6abeYaKZXQAyN25VEuN3NvffOTFth0cl1cJ24zNfLE/edit#gid=1712652478): 
                // [0] PointDensity
                // [1] Slope
                // [2] Roughness
                // [3] PointQuality3D
                // [4] InterpolationError
                // [5] GPSZErrorPDSLPdegPQ
                // [6] CHaMPTSZErrorPDSLPdeg3DQIntErr
                // [7] CHaMPTSZErrorPDSLPdegSR3DQIntErr
                new double[] { 0.2928451002, 0.3055774868, 0.3055774868, 0.2801127136, 0.3055774868, 0.3055774868, 0.2801127136, 0.2928451002, 0.2928451002 },
                new double[] { 25.52311134, 25.1586895, 24.2785244, 25.34467316, 24.90020752, 24.01894188, 25.16471291, 24.6658268, 23.80186081 },
                new double[] { 6.678427696, 7.253338814, 7.725869179, 6.481539726, 7.009198189, 7.442351341, 6.268901348, 6.772933483, 7.174583912 },
                new double[] { 0.04005853832, 0.04163705185, 0.04321556166, 0.03906433657, 0.0406428501, 0.04222136363, 0.03807013854, 0.03964864835, 0.04122716188 },
                new double[] { 0.0009646805702, 0.001046532765, 0.001128385076, 0.0009387636674, 0.00102061592, 0.001102468115, 0.0009128467646, 0.0009946989594, 0.001076551271 },
                // These are the GCD6 numbers
                new double[] { 0.05000000075, 0.1103881523, 0.1230156422, 0.4148696959, 0.4498254061, 0.3266140521, 0.3981432021, 0.1513022482, 0.1058925837 },
                new double[] { 0.5009201765, 0.3019917905, 0.1466666609, 0.4215319157, 0.1466666609, 0.1466666609, 0.3067083955, 0.1466666609, 0.1466666609 },
                new double[] { 0.6965144277, 0.4653637111, 0.05000000075, 0.6237809062, 0.05000000075, 0.05000000075, 0.4732590318, 0.05000000075, 0.05000000075 },
            };


            // TEST 3: CHaMP_TS_ZError_PD_SLPdeg_3DQ_IntErr
            // We need just PointDensity and SlopeDegrees
            Debug.WriteLine("CHaMP_TS_ZError_PD_SLPdeg_3DQ_IntErr");
            FisFile CHaMP_TS_ZError_PD_SLPdeg_3DQ_IntErr = new FisFile(new FileInfo(DirHelpers.GetTestRootPath(@"FIS\CHaMP_TS_ZError_PD_SLPdeg_3DQ_IntErr.fis")));
            for (int idx = 0; idx < Feshie[0].Length; idx++)
            {
                // Input order in the fis file is: Slope, PointDensity, 3DPointQuality, InterpolationError
                double result1 = CHaMP_TS_ZError_PD_SLPdeg_3DQ_IntErr.ruleset.calculate(new List<double[]> { UGR[1], UGR[0], UGR[3], UGR[4] }, idx, true, nodatas, -2);
                double result2 = CHaMP_TS_ZError_PD_SLPdeg_3DQ_IntErr.ruleset.calculate(new List<double> { UGR[1][idx], UGR[0][idx], UGR[3][idx], UGR[4][idx] });
                Assert.AreEqual(result1, result2);

                // Now verify them against the spreadsheet
                Debug.WriteLine(result1);
                Assert.AreEqual(result1, Feshie[4][idx], Feshie[4][idx] * ACCEPTABLE_DELTA);
            }


            // TEST 3: CHaMP_TS_ZError_PD_SLPdeg_IntErr
            // We need just PointDensity and SlopeDegrees
            Debug.WriteLine("CHaMP_TS_ZError_PD_SLPdeg_IntErr");
            FisFile CHaMP_TS_ZError_PD_SLPdeg_IntErr = new FisFile(new FileInfo(DirHelpers.GetTestRootPath(@"FIS\CHaMP_TS_ZError_PD_SLPdeg_IntErr.fis")));
            for (int idx = 0; idx < Feshie[0].Length; idx++)
            {
                // Input order in the fis file is: Slope, PointDensity, InterpolationError
                double result1 = CHaMP_TS_ZError_PD_SLPdeg_IntErr.ruleset.calculate(new List<double[]> { UGR[1], UGR[0], UGR[4] }, idx, true, nodatas, -2);
                double result2 = CHaMP_TS_ZError_PD_SLPdeg_IntErr.ruleset.calculate(new List<double> { UGR[1][idx], UGR[0][idx], UGR[4][idx] });
                Assert.AreEqual(result1, result2);

                // Now verify them against the spreadsheet
                Debug.WriteLine(result1);
                Assert.AreEqual(result1, Feshie[4][idx], Feshie[4][idx] * ACCEPTABLE_DELTA);
            }

            // TEST 3: CHaMP_TS_ZError_PD_SLPdeg_SR_3DQ_IntErr
            // We need just PointDensity and SlopeDegrees
            Debug.WriteLine("CHaMP_TS_ZError_PD_SLPdeg_SR_3DQ_IntErr");
            FisFile CHaMP_TS_ZError_PD_SLPdeg_SR_3DQ_IntErr = new FisFile(new FileInfo(DirHelpers.GetTestRootPath(@"FIS\CHaMP_TS_ZError_PD_SLPdeg_SR_3DQ_IntErr.fis")));
            for (int idx = 0; idx < Feshie[0].Length; idx++)
            {
                // Input order in the fis file is: Slope, PointDensity, Roughness, 3DPointQuality, InterpolationError
                double result1 = CHaMP_TS_ZError_PD_SLPdeg_SR_3DQ_IntErr.ruleset.calculate(new List<double[]> { UGR[1], UGR[0], UGR[2], UGR[3], UGR[4] }, idx, true, nodatas, -2);
                double result2 = CHaMP_TS_ZError_PD_SLPdeg_SR_3DQ_IntErr.ruleset.calculate(new List<double> { UGR[1][idx], UGR[0][idx], UGR[2][idx], UGR[3][idx], UGR[4][idx] });
                Assert.AreEqual(result1, result2);

                // Now verify them against the spreadsheet
                Debug.WriteLine(result1);
                Assert.AreEqual(result1, Feshie[4][idx], Feshie[4][idx] * ACCEPTABLE_DELTA);
            }

        }

    }
}