using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib.FIS;
using System;
using System.Collections.Generic;

namespace GCDConsoleLib.FIS.Tests
{
    [TestClass()]
    public class MemberFunctionTests
    {
        public void testCoord(List<double[]> listA, List<double[]> listB)
        {
            Assert.AreEqual(listA.Count, listB.Count);

            for (int i = 0; i < listA.Count; i++)
            {
                Assert.AreEqual(listA[i][0], listB[i][0]);
                Assert.AreEqual(listA[i][1], listB[i][1]);
            }
        }

        [TestMethod()]
        public void MemberFunctionTest()
        {
            // Empty Constructor
            MemberFunction construct1 = new MemberFunction();
            Assert.AreEqual(construct1.MaxY, 0);
            testCoord(construct1.Coords, new List<double[]>());
                
            // List Constructor
            MemberFunction construct2 = new MemberFunction(new List<double[]>(){
                new double[2]{ 1,0},
                new double[2]{ 2,1},
                new double[2]{ 3,1},
                new double[2]{ 4,0}
            });
            testCoord(construct2.Coords, new List<double[]>(){
                new double[2]{ 1,0},
                new double[2]{ 2,1},
                new double[2]{ 3,1},
                new double[2]{ 4,0}
            });
            Assert.AreEqual(construct2.MaxY, 1);

            // Triangle
            MemberFunction construct3 = new MemberFunction(1, 2.5, 3, 0.9);
            testCoord(construct3.Coords, new List<double[]>(){
                new double[2]{ 1,0},
                new double[2]{ 2.5, 0.9},
                new double[2]{ 3,0}
            });
            Assert.AreEqual(construct3.MaxY, 0.9);

            // Trapezoid
            MemberFunction construct4 = new MemberFunction(1, 2.5, 3, 4, 0.9);
            testCoord(construct4.Coords, new List<double[]>(){
                new double[2]{ 1, 0 },
                new double[2]{ 2.5, 0.9 },
                new double[2]{ 3, 0.9 },
                new double[2]{ 4, 0 }
            });
            Assert.AreEqual(construct4.MaxY, 0.9);

        }

        [TestMethod()]
        public void clearTest()
        {
            MemberFunction mf1 = new MemberFunction(new List<double[]>(){
                new double[2]{ 1,0},
                new double[2]{ 2,1},
                new double[2]{ 3,1},
                new double[2]{ 4,0}
            });

            mf1.clear();

            Assert.AreEqual(mf1.MaxY, 0);
            testCoord(mf1.Coords, new List<double[]>());

        }

        [TestMethod()]
        public void getXTest()
        {
            MemberFunction mf1 = new MemberFunction(new List<double[]>(){
                new double[2]{ 1, 0 }, //0
                new double[2]{ 2, 1 }, //1 <-- horiz
                new double[2]{ 3, 1 }, //2 <-- Vertical
                new double[2]{ 3, 2 }, //3 <-- horiz
                new double[2]{ 4, 2 }, //4
                new double[2]{ 5, 0 }  //5
            });

            double x1 = mf1.getX(0, 1, 0.5); // Slopey test
            Assert.AreEqual(x1, 1.5);

            double x2 = mf1.getX(0, 1, 0.9); // Slopey test
            Assert.AreEqual(x2, 1.9);

            double x3 = mf1.getX(1, 2, 1); // horiz test
            Assert.AreEqual(x3, 2);

            double x4 = mf1.getX(2, 3, 1); // Vert test
            Assert.AreEqual(x4, 3);

            double x5 = mf1.getX(4, 5, 0.5);
            Assert.AreEqual(x5, 4.75);

            double x6 = mf1.getX(5, 4, 0.5); // BAckwards indeces just for fun
            Assert.AreEqual(x6, 4.75);

        }

        [TestMethod()]
        public void fuzzifyTest()
        {
            MemberFunction mf1 = new MemberFunction(new List<double[]>(){
                new double[2]{ 1, 0 }, //0
                new double[2]{ 2, 1 }, //1 <-- horiz
                new double[2]{ 3, 1 }, //2 <-- Vertical
                new double[2]{ 3, 2 }, //3 <-- horiz
                new double[2]{ 4, 2 }, //4
                new double[2]{ 5, 0 }  //5
            });

            Assert.AreEqual(mf1.fuzzify(0), 0);
            Assert.AreEqual(mf1.fuzzify(1), 0);
            Assert.AreEqual(mf1.fuzzify(2), 1);
            Assert.AreEqual(mf1.fuzzify(3), 1);
            Assert.AreEqual(mf1.fuzzify(4), 2);
            Assert.AreEqual(mf1.fuzzify(5), 0);

            Assert.AreEqual(mf1.fuzzify(0.5), 0);
            Assert.AreEqual(mf1.fuzzify(1.5), 0.5);
            Assert.AreEqual(mf1.fuzzify(2.5), 1);
            Assert.AreEqual(mf1.fuzzify(3.5), 2);
            Assert.AreEqual(mf1.fuzzify(4.5), 1);
            Assert.AreEqual(mf1.fuzzify(5.5), 0);

            Assert.AreEqual(mf1.fuzzify(-100), 0);
            Assert.AreEqual(mf1.fuzzify(100), 0);

            Assert.AreEqual(mf1.fuzzify(double.PositiveInfinity), 0);
            Assert.AreEqual(mf1.fuzzify(double.NegativeInfinity), 0);

        }
    }
}