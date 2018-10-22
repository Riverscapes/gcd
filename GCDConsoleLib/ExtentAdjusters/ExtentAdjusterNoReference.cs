using System;

namespace GCDConsoleLib.ExtentAdjusters
{
    public class ExtentAdjusterNoReference : IExtentAdjuster
    {
        public ExtentRectangle SrcExtent { get; private set; }
        public ExtentRectangle OutExtent { get; private set; }
        public readonly ushort Precision;

        public ExtentAdjusterNoReference(ExtentRectangle srcextent)
        {
            SrcExtent = srcextent;

            // Get the precision of the source raster.
            // Override this if the first DEM in a project has unacceptably high num of decimal places
            Precision = (ushort)Utility.DynamicMath.NumDecimals(srcextent.CellWidth);
            if (Precision > 2)
                Precision = 2;

            ExtentRectangle temp = new ExtentRectangle(srcextent);
            temp.CellWidth = Math.Round(srcextent.CellWidth, Precision);
            temp.CellHeight = temp.CellWidth * (srcextent.CellHeight < 0 ? -1m : 1m);
            OutExtent = new ExtentRectangle(temp.GetDivisibleExtent());
        }

        private ExtentAdjusterNoReference(ExtentRectangle srcextent, ExtentRectangle outextent, ushort precision)
        {
            SrcExtent = srcextent;
            Precision = precision;
            OutExtent = new ExtentRectangle(OutExtent);
        }

        public IExtentAdjuster AdjustDimensions(decimal top, decimal right, decimal bottom, decimal left)
        {
            int rows = (int)((top - bottom) / OutExtent.CellWidth);
            int cols = (int)((right - left) / OutExtent.CellWidth);

            ExtentRectangle rawExtent = new ExtentRectangle(top, left, OutExtent.CellHeight, OutExtent.CellWidth, rows, cols);
            ExtentRectangle divExtent = rawExtent.GetDivisibleExtent();
            return new ExtentAdjusterNoReference(SrcExtent, divExtent, Precision);
        }
    }
}
