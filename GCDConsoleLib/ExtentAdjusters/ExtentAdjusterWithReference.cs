using System;

namespace GCDConsoleLib.ExtentAdjusters
{
    public class ExtentAdjusterWithReference : ExtentAdjusterBase
    {
        // Read only so that only constructores can alter them
        private readonly ExtentRectangle _RefExtent;

        // Interface properties that only allow setting
        public ExtentRectangle RefExtent { get { return _RefExtent; } }

        public ExtentAdjusterWithReference(ExtentRectangle srcextent, ExtentRectangle refextent) : base(srcextent)
        {
            _RefExtent = refextent;
        }

        private ExtentAdjusterWithReference(ExtentRectangle srcextent, ExtentRectangle outextent, ushort precision) : base(srcextent)
        {
            _numDecimals = precision;
            _OutExtent = new ExtentRectangle(outextent);
        }

        public override ExtentAdjusterBase AdjustDimensions(decimal top, decimal right, decimal bottom, decimal left)
        {
            int rows = (int)((top - bottom) / OutExtent.CellWidth);
            int cols = (int)((right - left) / OutExtent.CellWidth);

            ExtentRectangle rawExtent = new ExtentRectangle(top, left, OutExtent.CellHeight, OutExtent.CellWidth, rows, cols);
            ExtentRectangle divExtent = rawExtent.GetDivisibleExtent();
            return new ExtentAdjusterWithReference(SrcExtent, divExtent, Precision);
        }

        public override ExtentAdjusterBase AdjustPrecision(ushort precision)
        {
            throw new Exception("It should not be possible to adjust the precision of a raster with a reference extent.");
        }

        public override ExtentAdjusterBase AdjustCellSize(decimal cellSize)
        {
            throw new Exception("It should not be possible to adjust the cell size of a raster with a reference extent.");
        }
    }
}
