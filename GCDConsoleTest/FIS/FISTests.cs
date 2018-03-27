using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Collections.Generic;
using GCDConsoleTest.Helpers;
using System.Linq;

namespace GCDConsoleLib.FIS.Tests
{
    [TestClass()]
    public class FISTests
    {
        /// <summary>
        /// This is our verification testing of values from a spreadsheet that was backed up by scikit-fuzzy and matlab and tests a set of values end-to-end
        /// </summary>
        [TestMethod()]
        [TestCategory("Unit")]
        public void FISAcceptanceTest()
        {
            FisFile TS_ZError_PD_SLPdeg = new FisFile(new FileInfo(DirHelpers.GetTestRootPath(@"FIS\TS_ZError_PD_SLPdeg.fis")));

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
                new double[]{ 0.1333, 0.1417, 0.1479, 0.9125, 0.9797, 0.8423, 0.6588, 0.1410, 0.1182 },
                new double[]{ 0.05000000075, 0.1103881523, 0.1230156422, 0.4148696959, 0.4498254061, 0.3266140521, 0.3981432021, 0.1513022482, 0.1058925837 },
            };

            // TEST 1: TSZErrorPDSLPdeg
            // We need just PointDensity and SlopeDegrees
            for (int idx = 0; idx < Feshie.Count; idx++)
            {
                double result1 = TS_ZError_PD_SLPdeg.ruleset.calculate(new List<double[]> { Feshie[0], Feshie[2] }, idx, true, nodatas, -2);
                //double result2 = TS_ZError_PD_SLPdeg.ruleset.calculate(Test1Inputs[idx].ToList());
                Assert.Fail();
            }


            FisFile CHaMP_TS_ZError_PD_SLPdeg_3DQ_IntErr = new FisFile(new FileInfo(DirHelpers.GetTestRootPath(@"FIS\CHaMP_TS_ZError_PD_SLPdeg_3DQ_IntErr.fis")));
            FisFile CHaMP_TS_ZError_PD_SLPdeg_IntErr = new FisFile(new FileInfo(DirHelpers.GetTestRootPath(@"FIS\CHaMP_TS_ZError_PD_SLPdeg_IntErr.fis")));
            FisFile CHaMP_TS_ZError_PD_SLPdeg_SR_3DQ_IntErr = new FisFile(new FileInfo(DirHelpers.GetTestRootPath(@"FIS\CHaMP_TS_ZError_PD_SLPdeg_SR_3DQ_IntErr.fis")));
            FisFile GPS_ZError_PD_SLPdeg_PQ = new FisFile(new FileInfo(DirHelpers.GetTestRootPath(@"FIS\GPS_ZError_PD_SLPdeg_PQ.fis")));

        }

    }
}