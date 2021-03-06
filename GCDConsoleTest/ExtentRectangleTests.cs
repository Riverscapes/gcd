﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using GCDConsoleLib.GCD;
using UnitsNet.Units;
using GCDConsoleTest.Helpers;

namespace GCDConsoleLib.Tests
{
    [TestClass()]
    public class ExtentRectangleTests
    {
        [TestMethod()]
        [TestCategory("Unit")]
        public void ExtentRectangleTest()
        {
            ExtentRectangle rA2 = new ExtentRectangle(new FileInfo(DirHelpers.GetTestRasterPath("Slopey980-950.tif")));
            Assert.AreEqual(rA2.Top, -10019.0m);
            Assert.AreEqual(rA2.Bottom, -10029.0m);
            Assert.AreEqual(rA2.Left, -9979.0m);
            Assert.AreEqual(rA2.Right, -9969.0m);
            Assert.AreEqual(rA2.Rows, 100);
            Assert.AreEqual(rA2.Cols, 100);

            Assert.AreEqual(rA2.CellHeight, -0.1m);
            Assert.AreEqual(rA2.CellWidth, 0.1m);
            Assert.AreEqual(rA2.Height, 10.0m);
            Assert.AreEqual(rA2.Width, 10.0m);
            Assert.AreEqual(rA2.MaxArrID, 9999);

            CollectionAssert.AreEqual(rA2.Transform, new double[6] { -9979, 0.1, 0, -10019, 0, -0.1 });

            // Now let's try a copy constructor
            ExtentRectangle rA1 = new ExtentRectangle(rA2);
            Assert.AreEqual(rA2.Top, rA1.Top);
            Assert.AreEqual(rA2.Bottom, rA1.Bottom);
            Assert.AreEqual(rA2.Left, rA1.Left);
            Assert.AreEqual(rA2.Right, rA1.Right);
            Assert.AreEqual(rA2.Rows, rA1.Rows);
            Assert.AreEqual(rA2.Cols, rA1.Cols);

            Assert.AreEqual(rA2.CellHeight, rA1.CellHeight);
            Assert.AreEqual(rA2.CellWidth, rA1.CellWidth);
            Assert.AreEqual(rA2.Height, rA1.Height);
            Assert.AreEqual(rA2.Width, rA1.Width);
            Assert.AreEqual(rA2.MaxArrID, rA1.MaxArrID);

            // Now try the manual constructor
            ExtentRectangle rA3 = new ExtentRectangle(5, 6, -1, 1, 100, 94);
            Assert.AreEqual(rA3.Top, 5.0m);
            Assert.AreEqual(rA3.Bottom, -95.0m);
            Assert.AreEqual(rA3.Left, 6.0m);
            Assert.AreEqual(rA3.Right, 100.0m);
            Assert.AreEqual(rA3.Rows, 100);
            Assert.AreEqual(rA3.Cols, 94);

            Assert.AreEqual(rA3.CellHeight, -1m);
            Assert.AreEqual(rA3.CellWidth, 1m);
            Assert.AreEqual(rA3.Height, 100.0m);
            Assert.AreEqual(rA3.Width, 94.0m);
            Assert.AreEqual(rA3.MaxArrID, 9399);

            // New tests for constructor that allows overriding cell size
            ExtentRectangle rA4 = new ExtentRectangle(5, 6, -1, 1, 100, 94);
            ExtentRectangle rA5 = new ExtentRectangle(rA4, -2, 2);

            Assert.AreEqual(rA5.Top, rA4.Top);
            Assert.AreEqual(rA5.Left, rA4.Left);
            Assert.AreEqual(rA5.CellHeight, -2);
            Assert.AreEqual(rA5.CellWidth, 2);
            Assert.AreEqual(rA5.Rows, 50);
            Assert.AreEqual(rA5.Cols, 47);

            // Iregular case
            ExtentRectangle rA6 = new ExtentRectangle(5, 6, -0.071123m, 0.835465m, 100, 94);
            ExtentRectangle rA7 = new ExtentRectangle(rA6, -0.0534567m, 0.7898765m);

            Assert.AreEqual(rA7.Top, rA6.Top);
            Assert.AreEqual(rA7.Left, rA6.Left);
            Assert.AreEqual(rA7.CellHeight, -0.0534567m);
            Assert.AreEqual(rA7.CellWidth, 0.7898765m);
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void BufferTest()
        {
            ExtentRectangle rOrig = new ExtentRectangle(20, 20, -1, 1, 20, 20);
            ExtentRectangle rA1 = rOrig.Buffer(10.0m);
            Assert.AreEqual(rA1.Top, 30.0m);
            Assert.AreEqual(rA1.Bottom, -10.0m);
            Assert.AreEqual(rA1.Left, 10.0m);
            Assert.AreEqual(rA1.Right, 50.0m);
            Assert.AreEqual(rA1.Rows, 40);
            Assert.AreEqual(rA1.Cols, 40);

            Assert.AreEqual(rA1.CellHeight, -1m);
            Assert.AreEqual(rA1.CellWidth, 1m);
            Assert.AreEqual(rA1.Height, 40.0m);
            Assert.AreEqual(rA1.Width, 40.0m);
            Assert.AreEqual(rA1.MaxArrID, 1599);

            // we can do it by number of cells too
            ExtentRectangle rA2 = rOrig.Buffer((int)11);
            Assert.AreEqual(rA2.Top, 31.0m);
            Assert.AreEqual(rA2.Bottom, -11.0m);
            Assert.AreEqual(rA2.Left, 9.0m);
            Assert.AreEqual(rA2.Right, 51.0m);
            Assert.AreEqual(rA2.Rows, 42);
            Assert.AreEqual(rA2.Cols, 42);

            Assert.AreEqual(rA2.CellHeight, -1m);
            Assert.AreEqual(rA2.CellWidth, 1m);
            Assert.AreEqual(rA2.Height, 42.0m);
            Assert.AreEqual(rA2.Width, 42.0m);
            Assert.AreEqual(rA2.MaxArrID, 1763);
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void ToStringTest()
        {
            ExtentRectangle rA1 = new ExtentRectangle(5, 6, -1, 1, 100, 100);
            Assert.AreEqual(rA1.ToString(), "6 -95 106 5");
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void UnionTest()
        {
            ExtentRectangle rA1 = new ExtentRectangle(0, 0, -1, 1, 30, 30);
            ExtentRectangle rA2 = new ExtentRectangle(20, 20, -1, 1, 30, 30);

            ExtentRectangle rTest = rA1.Union(rA2);

            Assert.AreEqual(rTest.Top, 20.0m);
            Assert.AreEqual(rTest.Bottom, -30.0m);
            Assert.AreEqual(rTest.Left, 0.0m);
            Assert.AreEqual(rTest.Right, 50.0m);
            Assert.AreEqual(rTest.Rows, 50);
            Assert.AreEqual(rTest.Cols, 50);

            Assert.AreEqual(rTest.CellHeight, -1m);
            Assert.AreEqual(rTest.CellWidth, 1m);
            Assert.AreEqual(rTest.Height, 50.0m);
            Assert.AreEqual(rTest.Width, 50.0m);
            Assert.AreEqual(rTest.MaxArrID, 2499);

        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void IntersectTest()
        {
            ExtentRectangle rA1 = new ExtentRectangle(0, 0, -1, 1, 30, 30);
            ExtentRectangle rA2 = new ExtentRectangle(20, 20, -1, 1, 30, 30);

            ExtentRectangle rTest = rA1.Intersect(rA2);

            Assert.AreEqual(rTest.Top, 0.0m);
            Assert.AreEqual(rTest.Bottom, -10.0m);
            Assert.AreEqual(rTest.Left, 20.0m);
            Assert.AreEqual(rTest.Right, 30.0m);
            Assert.AreEqual(rTest.Rows, 10);
            Assert.AreEqual(rTest.Cols, 10);

            Assert.AreEqual(rTest.CellHeight, -1m);
            Assert.AreEqual(rTest.CellWidth, 1m);
            Assert.AreEqual(rTest.Height, 10.0m);
            Assert.AreEqual(rTest.Width, 10.0m);
            Assert.AreEqual(rTest.MaxArrID, 99);
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void GetTranslationTest()
        {
            ExtentRectangle rA1 = new ExtentRectangle(0, 0, -1, 1, 30, 30);
            ExtentRectangle rA2 = new ExtentRectangle(3, 2, -1, 1, 30, 30);

            // Returns (x,y) ==> (col,row)
            int[] tr1 = rA1.GetTranslation(rA2);
            int[] tr2 = rA2.GetTranslation(rA1);

            Assert.AreEqual(tr1[0], 2);
            Assert.AreEqual(tr1[1], 3);

            Assert.AreEqual(tr2[0], -tr1[0]);
            Assert.AreEqual(tr2[1], -tr1[1]);
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void IsConcurrentTest()
        {
            ExtentRectangle rA1 = new ExtentRectangle(5, 6, -1, 1, 100, 100);
            ExtentRectangle rYES1 = new ExtentRectangle(5, 6, -1, 1, 100, 100);
            ExtentRectangle rNo1 = new ExtentRectangle(4, 6, -1, 1, 100, 100);
            ExtentRectangle rNo2 = new ExtentRectangle(5, 5, -1, 1, 100, 100);
            ExtentRectangle rNo3 = new ExtentRectangle(5, 6, 1, 1, 100, 100);
            ExtentRectangle rNo4 = new ExtentRectangle(5, 6, -1, -1, 100, 100);

            Assert.IsTrue(rA1.IsConcurrent(rYES1));
            Assert.IsFalse(rA1.IsConcurrent(rNo1));
            Assert.IsFalse(rA1.IsConcurrent(rNo2));
            Assert.IsFalse(rA1.IsConcurrent(rNo3));
            Assert.IsFalse(rA1.IsConcurrent(rNo4));
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void IsOrthogonalTest()
        {
            ExtentRectangle rA1 = new ExtentRectangle(5, 6, -1, 1, 100, 100);
            ExtentRectangle rB1 = new ExtentRectangle(1, 3.0m, -1, 1, 100, 100);
            ExtentRectangle rC1 = new ExtentRectangle(2, 7, -1, 1, 100, 100);
            ExtentRectangle rD1 = new ExtentRectangle(4.1m, 6.0m, -1, 1, 100, 100);

            Assert.AreEqual(rA1.MaxArrID, 9999);

            // Positive Tests
            Assert.IsTrue(rA1.IsOrthogonal(rA1));
            Assert.IsTrue(rA1.IsOrthogonal(rB1) && rB1.IsOrthogonal(rA1));
            Assert.IsTrue(rA1.IsOrthogonal(rC1) && rC1.IsOrthogonal(rA1));

            // Negative Tests
            Assert.IsFalse(rA1.IsOrthogonal(rD1) || rD1.IsOrthogonal(rA1));
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void IsDivisibleTest()
        {
            // Positive test
            ExtentRectangle rDivisible1 = new ExtentRectangle(5, 6, -1, 1, 100, 100);
            Assert.IsTrue(rDivisible1.IsDivisible());

            //Negative Test
            ExtentRectangle rNotDivisible1 = new ExtentRectangle(5, 6, -2, 2, 100, 100);
            Assert.IsFalse(rNotDivisible1.IsDivisible());

            // Positive test
            ExtentRectangle rDivisible2 = new ExtentRectangle(5.1m, 6.1m, -0.1m, 0.1m, 100, 100);
            Assert.IsTrue(rDivisible2.IsDivisible());

            //Negative Test
            ExtentRectangle rNotDivisible2 = new ExtentRectangle(5.1m, 6.1m, -0.1m, 0.2m, 100, 100);
            Assert.IsFalse(rNotDivisible2.IsDivisible());

            //Negative Test
            ExtentRectangle rNotDivisible3 = new ExtentRectangle(5.1m, 6.1m, -0.2m, 0.1m, 100, 100);
            Assert.IsFalse(rNotDivisible3.IsDivisible());

            // Positive Test for floating point weirdness
            ExtentRectangle rDivisible4 = new ExtentRectangle(5.0999999999999999999999999999999999999999999m, 6.1m, -0.1m, 0.1m, 100, 100);
            Assert.IsTrue(rDivisible4.IsDivisible());

            // Negative Test for floating point weirdness
            ExtentRectangle rNotDivisible4 = new ExtentRectangle(5.0999999999999999999999999999999999999999999m, 6.1m, -0.2m, 0.1m, 100, 100);
            Assert.IsFalse(rNotDivisible4.IsDivisible());
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void MakeDivisibleTest()
        {
            ///
            /// Here's an already divisible raster. Make sure we don't change it.
            ///
            ExtentRectangle eA1 = new ExtentRectangle(5.1m, 6.4m, 0.1m, 0.2m, 100, 100);

            // This should do nothing
            ExtentRectangle eA1result = eA1.GetDivisibleExtent();
            Assert.AreEqual(eA1result.Top, eA1.Top);
            Assert.AreEqual(eA1result.Bottom, eA1.Bottom);
            Assert.AreEqual(eA1result.Left, eA1.Left);
            Assert.AreEqual(eA1result.Right, eA1.Right);
            Assert.AreEqual(eA1result.Rows, eA1.Rows);
            Assert.AreEqual(eA1result.Cols, eA1.Cols);
            Assert.IsTrue(eA1result.Width >= eA1.Width);
            Assert.IsTrue(eA1result.Height >= eA1.Height);
            Assert.IsTrue(eA1result.IsDivisible());

            ///
            /// NEGStep 1: Negative Cell Height, Positive Cell Width
            ///
            ExtentRectangle eNotDivisible = new ExtentRectangle(5.09m, 6.5m, -0.1m, 0.2m, 100, 100);
            Assert.IsFalse(eNotDivisible.IsDivisible());

            // Now Make it divisible in the right way
            ExtentRectangle eDivisible = eNotDivisible.GetDivisibleExtent();
            Assert.AreEqual(eDivisible.Top, 5.1m);
            Assert.AreEqual(eDivisible.Bottom, -5.0m);
            Assert.AreEqual(eDivisible.Left, 6.4m);
            Assert.AreEqual(eDivisible.Right, 26.6m);
            Assert.AreEqual(eDivisible.Rows, 101);
            Assert.AreEqual(eDivisible.Cols, 101);
            Assert.IsTrue(eDivisible.Width >= eNotDivisible.Width);
            Assert.IsTrue(eDivisible.Height >= eNotDivisible.Height);
            Assert.IsTrue(eDivisible.IsDivisible());

            ///
            /// NEGStep 1: Negative Cell Height, Positive Cell Width
            ///
            ExtentRectangle eNegStep1NonDivisible = new ExtentRectangle(5.09m, 6.39m, -0.1m, 0.2m, 100, 100);
            Assert.IsFalse(eNegStep1NonDivisible.IsDivisible());

            //Now Make it divisible in the right way
            ExtentRectangle eNegStep1 = eNegStep1NonDivisible.GetDivisibleExtent();
            Assert.AreEqual(eNegStep1.Top, 5.1m);
            Assert.AreEqual(eNegStep1.Bottom, -5.0m);
            Assert.AreEqual(eNegStep1.Left, 6.2m);
            Assert.AreEqual(eNegStep1.Right, 26.4m);
            Assert.AreEqual(eNegStep1.Rows, 101);
            Assert.AreEqual(eNegStep1.Cols, 101);
            Assert.IsTrue(eNegStep1.Width >= eNegStep1NonDivisible.Width);
            Assert.IsTrue(eNegStep1.Height >= eNegStep1NonDivisible.Height);
            Assert.IsTrue(eNegStep1.IsDivisible());

            ///
            /// NEGStep 2: Positive Cell Height, Negative Cell Width
            ///
            ExtentRectangle eNegStep2NonDivisible = new ExtentRectangle(5.09m, 6.39m, 0.1m, -0.2m, 100, 100);
            Assert.IsFalse(eNegStep2NonDivisible.IsDivisible());

            //Now Make it divisible in the right way
            ExtentRectangle eNegStep2 = eNegStep2NonDivisible.GetDivisibleExtent();
            Assert.AreEqual(eNegStep2.Top, 5.0m);
            Assert.AreEqual(eNegStep2.Bottom, 15.1m);
            Assert.AreEqual(eNegStep2.Left, 6.4m);
            Assert.AreEqual(eNegStep2.Right, -13.8m);
            Assert.AreEqual(eNegStep2.Rows, 101);
            Assert.AreEqual(eNegStep2.Cols, 101);
            Assert.IsTrue(eNegStep2.Width >= eNegStep2NonDivisible.Width);
            Assert.IsTrue(eNegStep2.Height >= eNegStep2NonDivisible.Height);
            Assert.IsTrue(eNegStep2.IsDivisible());

            ///
            /// NegWorldNEGStep 1: Negative Coordinates, Negative Cell Height, Positive Cell Width
            ///
            ExtentRectangle eNWNegStep1NonDivisible = new ExtentRectangle(-5.09m, -6.39m, -0.1m, 0.2m, 100, 100);
            Assert.IsFalse(eNWNegStep1NonDivisible.IsDivisible());

            //Now Make it divisible in the right way
            ExtentRectangle eNWNegStep1 = eNWNegStep1NonDivisible.GetDivisibleExtent();
            Assert.AreEqual(eNWNegStep1.Top, -5.0m);
            Assert.AreEqual(eNWNegStep1.Bottom, -15.1m);
            Assert.AreEqual(eNWNegStep1.Left, -6.4m);
            Assert.AreEqual(eNWNegStep1.Right, 13.8m);
            Assert.AreEqual(eNWNegStep1.Rows, 101);
            Assert.AreEqual(eNWNegStep1.Cols, 101);
            Assert.IsTrue(eNWNegStep1.Width >= eNWNegStep1NonDivisible.Width);
            Assert.IsTrue(eNWNegStep1.Height >= eNWNegStep1NonDivisible.Height);
            Assert.IsTrue(eNWNegStep1.IsDivisible());

            ///
            /// NegWorldNEGStep 2: Negative Coordinates, Positive Cell Height, Negative Cell Width
            ///
            ExtentRectangle eNWNegStep2NonDivisible = new ExtentRectangle(-5.09m, -6.39m, 0.1m, -0.2m, 100, 100);
            Assert.IsFalse(eNWNegStep2NonDivisible.IsDivisible());

            //Now Make it divisible in the right way
            ExtentRectangle eNWNegStep2 = eNWNegStep2NonDivisible.GetDivisibleExtent();
            Assert.AreEqual(eNWNegStep2.Top, -5.1m);
            Assert.AreEqual(eNWNegStep2.Bottom, 5.0m);
            Assert.AreEqual(eNWNegStep2.Left, -6.2m);
            Assert.AreEqual(eNWNegStep2.Right, -26.4m);
            Assert.AreEqual(eNWNegStep2.Rows, 101);
            Assert.AreEqual(eNWNegStep2.Cols, 101);
            Assert.IsTrue(eNWNegStep2.Width >= eNWNegStep2NonDivisible.Width);
            Assert.IsTrue(eNWNegStep2.Height >= eNWNegStep2NonDivisible.Height);
            Assert.IsTrue(eNWNegStep2.IsDivisible());


            // GCD user reported raster issue 2017/01/15 by P
            ExtentRectangle user1_src = new ExtentRectangle(114.58069247m, 122.938517551m, -0.003m, 0.003m, 4410, 11487);
            Assert.IsFalse(user1_src.IsDivisible());

            ExtentRectangle user1_out = user1_src.GetDivisibleExtent();
            Assert.AreEqual(user1_out.Top, 114.582m);
            Assert.AreEqual(user1_out.Bottom, 101.349m);
            Assert.AreEqual(user1_out.Left, 122.937m);
            Assert.AreEqual(user1_out.Right, 157.401m);
            Assert.AreEqual(user1_out.Rows, 4411);
            Assert.AreEqual(user1_out.Cols, 11488);
            Assert.IsTrue(user1_out.Width >= user1_src.Width);
            Assert.IsTrue(user1_out.Height >= user1_src.Height);
            Assert.IsTrue(user1_out.Rows >= user1_src.Rows);
            Assert.IsTrue(user1_out.Cols >= user1_src.Cols);
            Assert.IsTrue(user1_out.IsDivisible());

            // GCD user reported raster issue 2017/11/15 by AT
            ExtentRectangle user2_src = new ExtentRectangle(5629639.30575m, 558111.051499m, -1m, 1m, 47, 46);
            Assert.IsFalse(user2_src.IsDivisible());

            ExtentRectangle user2_out = user2_src.GetDivisibleExtent();
            Assert.AreEqual(user2_out.Top, 5629640m);
            Assert.AreEqual(user2_out.Bottom, 5629592m);
            Assert.AreEqual(user2_out.Left, 558111m);
            Assert.AreEqual(user2_out.Right, 558158m);
            Assert.AreEqual(user2_out.Rows, 48);
            Assert.AreEqual(user2_out.Cols, 47);
            Assert.IsTrue(user2_out.Width >= user2_src.Width);
            Assert.IsTrue(user2_out.Height >= user2_src.Height);
            Assert.IsTrue(user2_out.Rows >= user2_src.Rows);
            Assert.IsTrue(user2_out.Cols >= user2_src.Cols);
            Assert.IsTrue(user2_out.IsDivisible());

            // GCD user reported raster issue 2018/09/04 by M
            ExtentRectangle user3_src = new ExtentRectangle(7111409.27802m, 445073.27318m, -0.05m, 0.05m, 11800, 10160);
            Assert.IsFalse(user3_src.IsDivisible());

            ExtentRectangle user3_out = user3_src.GetDivisibleExtent();
            Assert.AreEqual(user3_out.Top, 7111409.3m);
            Assert.AreEqual(user3_out.Bottom, 7110819.25m);
            Assert.AreEqual(user3_out.Left, 445073.25m);
            Assert.AreEqual(user3_out.Right, 445581.30m);
            Assert.AreEqual(user3_out.Rows, 11801);
            Assert.AreEqual(user3_out.Cols, 10161);
            Assert.IsTrue(user3_out.Width >= user3_src.Width);
            Assert.IsTrue(user3_out.Height >= user3_src.Height);
            Assert.IsTrue(user3_out.Rows >= user3_src.Rows);
            Assert.IsTrue(user3_out.Cols >= user3_src.Cols);
            Assert.IsTrue(user3_out.IsDivisible());

            // Sulphur Creek Sanity Check
            ExtentRectangle user4_src = new ExtentRectangle(592252.5m, 1958980.5m, -0.5m, 0.5m, 583, 413);

            ExtentRectangle user4_out = user4_src.GetDivisibleExtent();
            Assert.AreEqual(user4_out.Top, 592252.5m);
            Assert.AreEqual(user4_out.Bottom, 591961m);
            Assert.AreEqual(user4_out.Left, 1958980.5m);
            Assert.AreEqual(user4_out.Right, 1959187m);
            Assert.AreEqual(user4_out.Rows, 583);
            Assert.AreEqual(user4_out.Cols, 413);
            Assert.IsTrue(user4_out.Width >= user4_src.Width);
            Assert.IsTrue(user4_out.Height >= user4_src.Height);
            Assert.IsTrue(user4_out.Rows >= user4_src.Rows);
            Assert.IsTrue(user4_out.Cols >= user4_src.Cols);
            Assert.IsTrue(user4_out.IsDivisible());
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void HasOverlapTest()
        {
            // TODO: TEST THIS THOROUGHLY, ESPECIALLY -/+ width heights
            ExtentRectangle eA1 = new ExtentRectangle(5.1m, 6.4m, -0.1m, 0.2m, 100, 100);
            ExtentRectangle eInside = new ExtentRectangle(3, 9, -0.1m, 0.2m, 50, 50);
            ExtentRectangle eOverlap1 = new ExtentRectangle(9.1m, 0.4m, -0.1m, 0.2m, 100, 100);

            ExtentRectangle eEdgesOverlap = new ExtentRectangle(5.1m, 6.4m, 0.1m, 0.2m, 100, 100);

            ExtentRectangle eNoOverlap = new ExtentRectangle(5.1m, 6.4m, 0.1m, 0.2m, 100, 100);

            // Some positive Tests
            Assert.IsTrue(eA1.HasOverlap(eA1));
            Assert.IsTrue(eA1.HasOverlap(eInside));
            Assert.IsTrue(eA1.HasOverlap(eOverlap1));

            // Some border cases
            Assert.IsFalse(eA1.HasOverlap(eEdgesOverlap));

            // Some negative tests
            Assert.IsFalse(eA1.HasOverlap(eNoOverlap));
        }


        [TestMethod()]
        [TestCategory("Unit")]
        public void RowCol2IdTest()
        {
            ExtentRectangle rTest1 = new ExtentRectangle(5, 6, -1, 1, 100, 100);
            Assert.AreEqual(rTest1.RowCol2Id(2, 3), 102);
            Assert.AreEqual(rTest1.RowCol2Id(1, 1), 0);
            Assert.AreEqual(rTest1.RowCol2Id(1, 2), 1);
            Assert.AreEqual(rTest1.RowCol2Id(2, 1), 100);
            Assert.AreEqual(rTest1.RowCol2Id(100, 100), 9999);
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void Id2RowColTest()
        {
            ExtentRectangle rTest1 = new ExtentRectangle(5, 6, -1, 1, 100, 50);
            CollectionAssert.AreEqual(rTest1.Id2RowCol(52), new int[2] { 2, 3 });
            CollectionAssert.AreEqual(rTest1.Id2RowCol(0), new int[2] { 1, 1 });
            CollectionAssert.AreEqual(rTest1.Id2RowCol(1), new int[2] { 1, 2 });
            CollectionAssert.AreEqual(rTest1.Id2RowCol(100), new int[2] { 3, 1 });
            CollectionAssert.AreEqual(rTest1.Id2RowCol(4999), new int[2] { 100, 50 });
        }

        [TestMethod()]
        [TestCategory("Unit")]
        public void RelativeId()
        {
            ExtentRectangle rTest1 = new ExtentRectangle(4, 6, -1, 1, 10, 10);
            ExtentRectangle rTest2 = new ExtentRectangle(5, 3, -1, 1, 8, 8);

            Assert.AreEqual(rTest1.RelativeId(0, rTest1), 0);
            Assert.AreEqual(rTest1.RelativeId(0, rTest2), 11);
            Assert.AreEqual(rTest2.RelativeId(0, rTest1), -1);
            Assert.AreEqual(rTest2.RelativeId(0, rTest2), 0);

            Assert.AreEqual(rTest1.RelativeId(1, rTest1), 1);
            Assert.AreEqual(rTest1.RelativeId(1, rTest2), 12);
            Assert.AreEqual(rTest2.RelativeId(1, rTest1), -1);
            Assert.AreEqual(rTest2.RelativeId(1, rTest2), 1);

            Assert.AreEqual(rTest1.RelativeId(24, rTest1), 24);
            Assert.AreEqual(rTest1.RelativeId(24, rTest2), 31);
            Assert.AreEqual(rTest2.RelativeId(24, rTest1), -1);
            Assert.AreEqual(rTest2.RelativeId(24, rTest2), 24);

            Assert.AreEqual(rTest1.RelativeId(25, rTest1), 25);
            Assert.AreEqual(rTest1.RelativeId(25, rTest2), -1);
            Assert.AreEqual(rTest2.RelativeId(25, rTest1), -1);
            Assert.AreEqual(rTest2.RelativeId(25, rTest2), 25);

            Assert.AreEqual(rTest1.RelativeId(99, rTest1), 99);
            Assert.AreEqual(rTest1.RelativeId(99, rTest2), -1);
            Assert.AreEqual(rTest2.RelativeId(99, rTest1), -1);
            Assert.AreEqual(rTest2.RelativeId(99, rTest2), -1);
        }

        [TestMethod()]
        [TestCategory("Functional")]
        public void CellAreaTest()
        {
            UnitGroup ug = new UnitGroup(VolumeUnit.CubicMeter, AreaUnit.SquareMeter, LengthUnit.Meter, LengthUnit.Meter);
            ExtentRectangle rA2 = new ExtentRectangle(new FileInfo(DirHelpers.GetTestRasterPath("Slopey980-950.tif")));
            Assert.AreEqual(rA2.CellArea(ug).SquareMeters, 0.01, 0.000000000001);
        }
    }
}