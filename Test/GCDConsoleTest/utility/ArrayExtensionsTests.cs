using Microsoft.VisualStudio.TestTools.UnitTesting;
using GCDConsoleLib.Common.Extensons;

namespace GCDConsoleLib.Common.Extensons.Tests
{
    [TestClass()]
    public class ArrayExtensionsTests
    {
        [TestMethod()]
        public void SliceTest1D()
        {
            int[] intArr = new int[] { -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 };

            int[] slice1 = intArr.Slice(0, 19);
            CollectionAssert.AreEqual(slice1, intArr);

            int[] slice2 = intArr.Slice(2, 4);
            CollectionAssert.AreEqual(slice2, new int[] { 1, 2, 3 });

            int[] slice3 = intArr.Slice(0, 5);
            CollectionAssert.AreEqual(slice3, new int[] { -1, 0, 1, 2, 3, 4 });

        }

        [TestMethod()]
        public void SliceTest2D()
        {
            int[,] intArr = new int[,] {
                { -1, 0, 1, 2 },
                { 3, 4, 5, 6},
                { 7, 8, 9, 10},
                { 11, 12, 13, 14},
                { 15, 16, 17, 18} };

            int[,] slice1 = intArr.Slice(0, 4, 0, 3);
            CollectionAssert.AreEqual(slice1, intArr);

            int[,] slice2 = intArr.Slice(1, 3, 1, 2);
            CollectionAssert.AreEqual(slice2, new int[,] { { 4, 5 }, { 8, 9 }, { 12, 13 } });
        }

        [TestMethod()]
        public void FillTest1D()
        {
            int[] intArr = new int[] { 0, 0, 0, 0 };
            intArr.Fill(6);
            CollectionAssert.AreEqual(intArr, new int[] { 6, 6, 6, 6 });
        }

        [TestMethod()]
        public void FillTest2D()
        {
            int[,] intArr = new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
            intArr.Fill(6);
            CollectionAssert.AreEqual(intArr, new int[,] { { 6, 6, 6, 6 }, { 6, 6, 6, 6 }, { 6, 6, 6, 6 }, { 6, 6, 6, 6 }, { 6, 6, 6, 6 } });

        }

        [TestMethod()]
        public void PlunkTest1D()
        {
            int[] intArr = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] plunkArr = new int[] { 1, 2, 3 };
            int[] expected = new int[] { 0, 0, 0, 1, 2, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

            intArr.Plunk(ref plunkArr, 3);
            CollectionAssert.AreEqual(intArr, expected);
        }

        [TestMethod()]
        public void PlunkTest2D()
        {
            int[,] intArr = new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
            int[,] plunkArr = new int[,] { { 1, 1 }, { 2, 2 } };
            int[,] expected = new int[,] { { 0, 0, 0, 0 }, { 0, 0, 1, 1 }, { 0, 0, 2, 2 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };

            intArr.Plunk(ref plunkArr, 1, 2);
            CollectionAssert.AreEqual(intArr, expected);
        }

        [TestMethod()]
        public void PlunkTestFlat()
        {
            int[,] intArr = new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
            int[,] plunkArr = new int[,] { { 1, 1 }, { 2, 2 } };
            int[,] expected = new int[,] { { 0, 0, 0, 0 }, { 0, 0, 1, 1 }, { 0, 0, 2, 2 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };

            int[] intArrFlat = intArr.Make1DArray();
            int[] plunkArrFlat = intArr.Make1DArray();
            int[] exectedFlat = intArr.Make1DArray();
            intArrFlat.Plunk(ref plunkArrFlat, intArr.GetLength(0), intArr.GetLength(1), plunkArr.GetLength(0), plunkArr.GetLength(1), 1, 2);

            CollectionAssert.AreEqual(intArrFlat, exectedFlat);
        }

        [TestMethod()]
        public void Make2DArrayTest()
        {
            int[] intArr = new int[] { 0, 0, 0, 0, 1, 2, 1, 2, 2, 3, 3, 4, 5, 5, 5, 6, 7, 7, 7, 9 };
            int[,] expected = new int[,] { { 0, 0, 0, 0 }, { 1, 2, 1, 2 }, { 2, 3, 3, 4 }, { 5, 5, 5, 6 }, { 7, 7, 7, 9 } };

            int[,] testResult = intArr.Make2DArray(5, 4);
            CollectionAssert.AreEqual(testResult, expected);

        }

        [TestMethod()]
        public void Make1DArrayTest()
        {
            int[,] intArr = new int[,] { { 0, 0, 0, 0 }, { 1, 2, 1, 2 }, { 2, 3, 3, 4 }, { 5, 5, 5, 6 }, { 7, 7, 7, 9 } };
            int[] expected = new int[] { 0, 0, 0, 0, 1, 2, 1, 2, 2, 3, 3, 4, 5, 5, 5, 6, 7, 7, 7, 9 };

            int[] testResult = intArr.Make1DArray();
            CollectionAssert.AreEqual(testResult, expected);

        }
    }
}