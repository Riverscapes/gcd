using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib.FIS;
using System;
using System.Collections.Generic;
using GCDConsoleLib.GCD;
using System.Diagnostics;

namespace GCDConsoleLib.FIS.Tests
{
    [TestClass()]
    public class FISOperatorsTests
    {
        [TestMethod()]
        public void MaxTest()
        {
            Assert.AreEqual(FISOperators.Max(1, 1), 1);
            Assert.AreEqual(FISOperators.Max(0, 1), 1);
            Assert.AreEqual(FISOperators.Max(0, -1), 0);
            Assert.AreEqual(FISOperators.Max(0.01, -1), 0.01);
            Assert.AreEqual(FISOperators.Max(23, 24), 24);
        }

        [TestMethod()]
        public void MinTest()
        {
            Assert.AreEqual(FISOperators.Min(1, 1), 1);
            Assert.AreEqual(FISOperators.Min(0, 1), 0);
            Assert.AreEqual(FISOperators.Min(0, -1), -1);
            Assert.AreEqual(FISOperators.Min(0.01, -1), -1);
            Assert.AreEqual(FISOperators.Min(23, 24), 23);
        }

        [TestMethod()]
        public void ProbOrTest()
        {
            // Note: I go this from https://www.mathworks.com/help/fuzzy/probor.html?requestedDomain=www.mathworks.com
            Assert.AreEqual(FISOperators.ProbOr(1, 1), 1);
            Assert.AreEqual(FISOperators.ProbOr(0, 1), 1);
            Assert.AreEqual(FISOperators.ProbOr(0, -1), -1);
            Assert.AreEqual(FISOperators.ProbOr(0.01, -1), -0.98);
            Assert.AreEqual(FISOperators.ProbOr(23, 24), -505);
        }

        [TestMethod()]
        public void ProductTest()
        {
            Assert.AreEqual(FISOperators.Product(1, 1), 1);
            Assert.AreEqual(FISOperators.Product(0, 2), 0);
            Assert.AreEqual(FISOperators.Product(3, -1), -3);
            Assert.AreEqual(FISOperators.Product(0.01, -1), -0.01);
            Assert.AreEqual(FISOperators.Product(23, 24), 552);
        }

        [TestMethod()]
        public void IntersectLinesTest()
        {
            // Lines do intersects with infinities first (vertical lines)
            Tuple<double, double, bool> inf1 = FISOperators.IntersectLines(0, 0, 0, 4, -2, 2, 2, 2);
            Assert.AreEqual(inf1.Item1, 0);
            Assert.AreEqual(inf1.Item2, 2);
            Assert.AreEqual(inf1.Item3, true);

            // Lines do intersects with infinities first (vertical lines)
            Tuple<double, double, bool> inf2 = FISOperators.IntersectLines(-2, 2, 2, 2, 0, 0, 0, 4);
            Assert.AreEqual(inf2.Item1, 0);
            Assert.AreEqual(inf2.Item2, 2);
            Assert.AreEqual(inf2.Item3, true);

            // Draw a nice X away from zero
            Tuple<double, double, bool> x1 = FISOperators.IntersectLines(1, 1, 4, 4, 1, 4, 4, 1);
            Assert.AreEqual(x1.Item1, 2.5);
            Assert.AreEqual(x1.Item2, 2.5);
            Assert.AreEqual(x1.Item3, true);

            // Lines don't intersect
            Tuple<double, double, bool> res2 = FISOperators.IntersectLines(0, 0, 0, 4, 2, 0, 4, 0);
            Assert.AreEqual(res2.Item1, 0);
            Assert.AreEqual(res2.Item2, 0);
            Assert.AreEqual(res2.Item3, false);

        }

        [TestMethod()]
        public void ImpMinTest()
        {
            // Set our input
            MemberFunction inMF = new MemberFunction(new List<double[]>
            {
                new double[] { 0,0},
                new double[] { 1,1},
                new double[] { 2,1},
                new double[] { 3,0},
            });

            // Run the test
            MemberFunction outMF = FISOperators.ImpMin(inMF, 0.5, 3.0);

            // Here's what we expect to get
            List<double[]> expected = new List<double[]>
            {
                new double[] { 0, 0 },
                new double[] { 0.5, 1.5 },
                new double[] { 2.5, 1.5 },
                new double[] { 3, 0 },
            };

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(outMF.Coords[i][0], expected[i][0]);
                Assert.AreEqual(outMF.Coords[i][1], expected[i][1]);
            }
        }

        [TestMethod()]
        public void ImpProductTest()
        {
            // Set our input
            MemberFunction inMF = new MemberFunction(new List<double[]>
            {
                new double[] { 0,0},
                new double[] { 1,1},
                new double[] { 2,1},
                new double[] { 3,0},
            });

            // Run the test
            MemberFunction outMF = FISOperators.ImpProduct(inMF, 0.5, 3.0);

            // Here's what we expect to get
            List<double[]> expected = new List<double[]>
            {
                new double[] { 0, 0 },
                new double[] { 1, 1.5 },
                new double[] { 2, 1.5 },
                new double[] { 3, 0 },
            };

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(outMF.Coords[i][0], expected[i][0]);
                Assert.AreEqual(outMF.Coords[i][1], expected[i][1]);
            }
        }

        [TestMethod()]
        public void AggMaxTest()
        {
            // Set our inputs
            MemberFunction inMF = new MemberFunction(new List<double[]>
            {
                new double[] { 0,0},
                new double[] { 1,1},
                new double[] { 2,1},
                new double[] { 3,0},
            });
            MemberFunction outMf = new MemberFunction(new List<double[]>
            {
                new double[] { 1.4, 0 },
                new double[] { 1.5, 2 },
                new double[] { 3, 2 },
                new double[] { 4, 0 },
            });

            // Run the test
            FISOperators.AggMax(inMF, outMf);

            // Here's what we expect to get
            List<double[]> expected = new List<double[]>
            {
                new double[] { 0, 0 },
                new double[] { 1, 1 },
                new double[] { 1.4, 1 },
                new double[] { 1.5, 2 },
                new double[] { 2, 2 },
                new double[] { 3, 2 },
                new double[] { 4, 0 },
            };

            // Visual output for sanity
            for (int i = 0; i < expected.Count; i++)
                Debug.WriteLine(String.Format("({0}, {1}) ==> ({2}, {3})", outMf.Coords[i][0], outMf.Coords[i][1], expected[i][0], expected[i][1]));

            // Test the values
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(outMf.Coords[i][0], expected[i][0]);
                Assert.AreEqual(outMf.Coords[i][1], expected[i][1]);
            }
        }

    }
}