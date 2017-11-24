using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib.FIS;
using System;
using System.Collections.Generic;

namespace GCDConsoleLib.FIS.Tests
{
    [TestClass()]
    public class MemberFunctionSetTests
    {
        [TestMethod()]
        public void MemberFunctionSetTest()
        {
            MemberFunctionSet mfSet1 = new MemberFunctionSet();
            Assert.IsFalse(mfSet1.Valid);
            Assert.AreEqual(mfSet1.MFunctions.Count, 0);
            Assert.AreEqual(mfSet1.Indices.Count, 0);
            Assert.AreEqual(mfSet1.Count, 0);

            MemberFunctionSet mfSet2 = new MemberFunctionSet(0.1, 0.5);
            Assert.IsFalse(mfSet1.Valid);
            Assert.AreEqual(mfSet1.MFunctions.Count, 0);
            Assert.AreEqual(mfSet1.Indices.Count, 0);
            Assert.AreEqual(mfSet1.Count, 0);
        }

        [TestMethod()]
        public void addMFTest()
        {
            MemberFunctionSet mfSet = new MemberFunctionSet(0, 5);
            MemberFunction mf1 = new MemberFunction(new List<double[]>(){
                new double[2]{ 1,0},
                new double[2]{ 2,1},
                new double[2]{ 3,1},
                new double[2]{ 4,0}
            });
            MemberFunction mf2 = new MemberFunction(new List<double[]>(){
                new double[2]{ 1,0},
                new double[2]{ 2,1},
                new double[2]{ 4,0}
            });

            mfSet.addMF("jerry", mf1);
            Assert.AreEqual(mfSet.Count, 1);
            Assert.AreSame(mfSet.MFunctions[0], mf1);
            Assert.AreEqual(mfSet.Indices["jerry"], 0);

            mfSet.addMF("garry", mf2);
            Assert.AreEqual(mfSet.Count, 2);
            Assert.AreSame(mfSet.MFunctions[1], mf2);
            Assert.AreEqual(mfSet.Indices["garry"], 1);

        }

        [TestMethod()]
        public void addTriangleMFTest()
        {
            MemberFunctionSet mfSet = new MemberFunctionSet(0, 5);

            mfSet.addTriangleMF("jerry", 1, 2, 3, 0.81);
            Assert.AreEqual(mfSet.Count, 1);
            Assert.AreEqual(mfSet.MFunctions[0].Coords.Count, 3);
            Assert.AreEqual(mfSet.MFunctions[0].MaxY, 0.81);
            Assert.AreEqual(mfSet.Indices["jerry"], 0);

            mfSet.addTriangleMF("garry", 1, 2, 3, 1);
            Assert.AreEqual(mfSet.Count, 2);
            Assert.AreEqual(mfSet.MFunctions[1].Coords.Count, 3);
            Assert.AreEqual(mfSet.MFunctions[1].MaxY, 1);
            Assert.AreEqual(mfSet.Indices["garry"], 1);
        }

        [TestMethod()]
        public void addTrapezoidMFTest()
        {
            MemberFunctionSet mfSet = new MemberFunctionSet(0, 5);

            mfSet.addTrapezoidMF("jerry", 1, 2, 3, 4, 0.81);
            Assert.AreEqual(mfSet.Count, 1);
            Assert.AreEqual(mfSet.MFunctions[0].Coords.Count, 4);
            Assert.AreEqual(mfSet.MFunctions[0].MaxY, 0.81);
            Assert.AreEqual(mfSet.Indices["jerry"], 0);

            mfSet.addTrapezoidMF("garry", 1, 2, 3, 4, 1);
            Assert.AreEqual(mfSet.Count, 2);
            Assert.AreEqual(mfSet.MFunctions[1].Coords.Count, 4);
            Assert.AreEqual(mfSet.MFunctions[1].MaxY, 1);
            Assert.AreEqual(mfSet.Indices["garry"], 1);
        }

    }
}