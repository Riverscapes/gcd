using System;

namespace GCDConsoleLib.ExtentAdjusters
{
    /// <summary>
    /// Base extent adjuster.
    /// </summary>
    /// <remarks>
    /// Abstract! Do not use this class. Use one of the more specific inherited
    /// classes instead.</remarks>
    public abstract class ExtentAdjusterBase
    {
        // Read only so that only constructores can alter them
        private readonly ExtentRectangle _SrcExtent;
        protected ExtentRectangle _OutExtent;
        protected ushort _numDecimals;

        // Interface properties that only allow getting values
        public ExtentRectangle SrcExtent { get { return _SrcExtent; } }
        public ExtentRectangle OutExtent { get { return _OutExtent; } }
        public ushort Precision { get { return _numDecimals; } }

        /// <summary>
        /// True when the source raster was divisible and the input and output cell sizes match
        /// </summary>
        public bool RequiresResampling
        {
            get
            {
                return !SrcExtent.IsDivisible() || SrcExtent.CellWidth != OutExtent.CellWidth;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcextent">Extent of the source raster that is being adjusted</param>
        public ExtentAdjusterBase(ExtentRectangle srcextent)
        {
            _SrcExtent = srcextent;
            _numDecimals = GetInitialNumDecimals(srcextent.CellWidth);

            ExtentRectangle temp = new ExtentRectangle(srcextent);
            temp.CellWidth = Math.Round(srcextent.CellWidth, _numDecimals);
            temp.CellHeight = temp.CellWidth * (srcextent.CellHeight < 0 ? -1m : 1m);
            _OutExtent = temp.GetDivisibleExtent();
        }

        // Inherited classes must implement all three methods.
        // If one of these methods is inappropriate then throw an exception inside the method.
        public abstract ExtentAdjusterBase AdjustDimensions(decimal top, decimal right, decimal bottom, decimal left);
        public abstract ExtentAdjusterBase AdjustPrecision(ushort precision);
        public abstract ExtentAdjusterBase AdjustCellSize(decimal cellSize);

        /// <summary>
        /// Get the precision of the source raster
        /// </summary>
        /// <param name="cellWidth">Cell width for which you want to know the number of decimals</param>
        /// <returns>Appropriate number of decimals for the argument cell width</returns>
        /// <remarks>See the unit tests. There are some non-obvious return values</remarks>
        public static ushort GetInitialNumDecimals(decimal cellWidth)
        {
            bool tryAgain;
            decimal ratio, smallNum, testCellWidth;

            // Get the number of decimals from the original cell resolution
            ushort numDecimals = (ushort)Utility.DynamicMath.NumDecimals(cellWidth);

            do
            {
                // Find the smallest value with the current number of decimals (i.e. 3 decimals is 0.001)
                smallNum = (decimal)Math.Pow(10, -1 * numDecimals);

                // How many decimal places between the first and last significant digit.
                // Big numbers are cause for concern (2.000000000004 is bad while 2.5 is OK)
                ratio = cellWidth / smallNum;
                tryAgain = ratio > 10000;
                
                if (tryAgain)
                {
                    // Remove one decimal place and try the test again. 
                    numDecimals--;
                    testCellWidth = Math.Round(cellWidth, numDecimals);
                    numDecimals = (ushort)Utility.DynamicMath.NumDecimals(testCellWidth);
                }

            } while (tryAgain);

            return numDecimals;
        }
    }
}
