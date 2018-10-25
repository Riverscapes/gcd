using System;

namespace GCDConsoleLib.ExtentAdjusters
{
    public abstract class ExtentAdjusterBase
    {
        // Read only so that only constructores can alter them
        private readonly ExtentRectangle _SrcExtent;
        protected ExtentRectangle _OutExtent;
        protected ushort _numDecimals;

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

        public ExtentAdjusterBase(ExtentRectangle srcextent)
        {
            _SrcExtent = srcextent;

            // Get the precision of the source raster.
            // Override this if the first DEM in a project has unacceptably high num of decimal places
            _numDecimals = (ushort)Utility.DynamicMath.NumDecimals(srcextent.CellWidth);
            decimal _smallNum = (decimal)Math.Pow(10, -1 * _numDecimals);
            decimal raw = srcextent.CellWidth / _smallNum;
            ushort rawdec = (ushort)Utility.DynamicMath.NumDecimals(raw);

            if (rawdec > 3)
                _numDecimals = (ushort)(2 + rawdec);

            ExtentRectangle temp = new ExtentRectangle(srcextent);
            temp.CellWidth = Math.Round(srcextent.CellWidth, _numDecimals);
            temp.CellHeight = temp.CellWidth * (srcextent.CellHeight < 0 ? -1m : 1m);
            _OutExtent = temp.GetDivisibleExtent();
        }

        public abstract ExtentAdjusterBase AdjustDimensions(decimal top, decimal right, decimal bottom, decimal left);
        public abstract ExtentAdjusterBase AdjustPrecision(ushort precision);
        public abstract ExtentAdjusterBase AdjustCellSize(decimal cellSize);
    }
}
