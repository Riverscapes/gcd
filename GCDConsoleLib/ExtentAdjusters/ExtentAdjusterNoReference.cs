using System;

namespace GCDConsoleLib.ExtentAdjusters
{
    public class ExtentAdjusterNoReference : ExtentAdjusterBase
    {
        public ExtentAdjusterNoReference(ExtentRectangle srcextent) : base(srcextent)
        {

        }

        private ExtentAdjusterNoReference(ExtentRectangle srcextent, ExtentRectangle outextent, ushort precision) : base(srcextent)
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
            return new ExtentAdjusterNoReference(SrcExtent, divExtent, Precision);
        }

        public override ExtentAdjusterBase AdjustCellSize(decimal cellSize)
        {
            ExtentRectangle rawExtent = new ExtentRectangle(OutExtent);
            rawExtent.CellWidth = cellSize;
            rawExtent.CellHeight = OutExtent.CellHeight < 0 ? cellSize * -1m : cellSize;
            ExtentRectangle divExtent = rawExtent.GetDivisibleExtent();

            return new ExtentAdjusterNoReference(SrcExtent, divExtent, Precision);
        }

        public override ExtentAdjusterBase AdjustPrecision(ushort precision)
        {
            decimal rawCellWidth = (decimal)Math.Round((double)OutExtent.CellWidth, precision);
            decimal rawCellHeight = (decimal)Math.Round((double)OutExtent.CellHeight, precision);

            // Abort if f the new precision causes the cell width or height to be zero.
            // e.g. cell width is 0.5 and the user passes precision 0
            if (rawCellWidth == 0m || rawCellHeight == 0m)
                return this;

            ExtentRectangle rawExtent = new ExtentRectangle(OutExtent.Top, OutExtent.Left, rawCellHeight, rawCellWidth, OutExtent.Rows, OutExtent.Cols);
            ExtentRectangle divExtent = rawExtent.GetDivisibleExtent();

            return new ExtentAdjusterNoReference(SrcExtent, divExtent, precision);
        }
    }
}
