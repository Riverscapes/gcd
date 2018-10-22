using System;

namespace GCDConsoleLib.ExtentAdjusters
{
    public class ExtentAdjusterWithReference : IExtentAdjuster
    {
        public ExtentRectangle SrcExtent { get; private set; }
        public ExtentRectangle RefExtent { get; private set; }
        public ExtentRectangle OutExtent { get; private set; }
        public readonly ushort Precision;

        public ExtentAdjusterWithReference(ExtentRectangle srcextent, ExtentRectangle refextent)
        {
            SrcExtent = srcextent;
            RefExtent = refextent;
            Precision = (ushort)Utility.DynamicMath.NumDecimals(refextent.CellWidth);

            // The output extent is fixed as that of the reference extent
            OutExtent = new ExtentRectangle(refextent);
        }

        public IExtentAdjuster AdjustDimensions(decimal top, decimal right, decimal bottom, decimal left)
        {
            throw new Exception("It should not be possible to adjust the dimensions of a raster with a reference extent.");
        }
    }
}
