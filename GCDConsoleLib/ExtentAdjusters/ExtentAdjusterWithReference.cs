using System;

namespace GCDConsoleLib.ExtentAdjusters
{
    public class ExtentAdjusterWithReference : IExtentAdjuster
    {
        // Read only so that only constructores can alter them
        private readonly ExtentRectangle _SrcExtent;
        private readonly ExtentRectangle _RefExtent;
        private readonly ExtentRectangle _OutExtent;
        private readonly ushort _Precision;

        // Interface properties that only allow setting
        public ExtentRectangle SrcExtent { get { return _SrcExtent; } }
        public ExtentRectangle RefExtent { get { return _RefExtent; } }
        public ExtentRectangle OutExtent { get { return _OutExtent; } }
        public ushort Precision { get { return _Precision; } }

        public ExtentAdjusterWithReference(ExtentRectangle srcextent, ExtentRectangle refextent)
        {
            _SrcExtent = srcextent;
            _RefExtent = refextent;
            _Precision = (ushort)Utility.DynamicMath.NumDecimals(refextent.CellWidth);

            // The output extent is fixed as that of the reference extent
            _OutExtent = new ExtentRectangle(refextent);
        }

        public IExtentAdjuster AdjustDimensions(decimal top, decimal right, decimal bottom, decimal left)
        {
            throw new Exception("It should not be possible to adjust the dimensions of a raster with a reference extent.");
        }

        public IExtentAdjuster AdjustPrecision(decimal precision)
        {
            throw new Exception("It should not be possible to adjust the precision of a raster with a reference extent.");
        }
        
        public IExtentAdjuster AdjustCellSize(decimal cellSize)
        {
            throw new Exception("It should not be possible to adjust the cell size of a raster with a reference extent.");
        }
    }
}
