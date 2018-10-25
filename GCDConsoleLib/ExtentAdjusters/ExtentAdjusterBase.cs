using System;

namespace GCDConsoleLib.ExtentAdjusters
{
    public class ExtentAdjusterNoReference : IExtentAdjuster
    {
        // Read only so that only constructores can alter them
        private readonly ExtentRectangle _SrcExtent;
        private readonly ExtentRectangle _OutExtent;
        private readonly ushort _numDecimals;

        // Interface properties that only allow setting
        public ExtentRectangle SrcExtent { get { return _SrcExtent; } }
        public ExtentRectangle OutExtent { get { return _OutExtent; } }
        public ushort Precision { get { return _numDecimals; } }

        public bool RequiresResampling
        {
            get
            {
                return !SrcExtent.IsDivisible() || SrcExtent.CellWidth != OutExtent.CellWidth;
            }
        }

        public ExtentAdjusterNoReference(ExtentRectangle srcextent)
        {
            _SrcExtent = srcextent;

            // Get the precision of the source raster.
            // Override this if the first DEM in a project has unacceptably high num of decimal places
            _numDecimals = (ushort)Utility.DynamicMath.NumDecimals(srcextent.CellWidth);
            decimal _smallNum = (decimal)Math.Pow(10, _numDecimals);
            if (Utility.DynamicMath.SafeDivision(_smallNum, srcextent.CellWidth) > 100)
                _numDecimals = 2;

            ExtentRectangle temp = new ExtentRectangle(srcextent);
            temp.CellWidth = Math.Round(srcextent.CellWidth, _numDecimals);
            temp.CellHeight = temp.CellWidth * (srcextent.CellHeight < 0 ? -1m : 1m);
            _OutExtent = temp.GetDivisibleExtent();
        }

        private ExtentAdjusterNoReference(ExtentRectangle srcextent, ExtentRectangle outextent, ushort precision)
        {
            _SrcExtent = srcextent;
            _numDecimals = precision;
            _OutExtent = new ExtentRectangle(outextent);
        }

        public IExtentAdjuster AdjustDimensions(decimal top, decimal right, decimal bottom, decimal left)
        {
            int rows = (int)((top - bottom) / OutExtent.CellWidth);
            int cols = (int)((right - left) / OutExtent.CellWidth);

            ExtentRectangle rawExtent = new ExtentRectangle(top, left, OutExtent.CellHeight, OutExtent.CellWidth, rows, cols);
            ExtentRectangle divExtent = rawExtent.GetDivisibleExtent();
            return new ExtentAdjusterNoReference(SrcExtent, divExtent, Precision);
        }

        public IExtentAdjuster AdjustCellSize(decimal cellSize)
        {
            ExtentRectangle rawExtent = new ExtentRectangle(OutExtent);
            rawExtent.CellWidth = cellSize;
            rawExtent.CellHeight = OutExtent.CellHeight < 0 ? cellSize * -1m : cellSize;
            ExtentRectangle divExtent = rawExtent.GetDivisibleExtent();

            return new ExtentAdjusterNoReference(SrcExtent, divExtent, Precision);
        }

        public IExtentAdjuster AdjustPrecision(ushort precision)
        {
            decimal rawCellWidth = (decimal)Math.Round((double)OutExtent.CellWidth, precision);
            decimal rawCellHeight = (decimal)Math.Round((double)OutExtent.CellHeight, precision);

            ExtentRectangle rawExtent = new ExtentRectangle(OutExtent.Top, OutExtent.Left, rawCellHeight, rawCellWidth, OutExtent.Rows, OutExtent.Cols);
            ExtentRectangle divExtent = rawExtent.GetDivisibleExtent();

            return new ExtentAdjusterNoReference(SrcExtent, divExtent, precision);
        }
    }
}
