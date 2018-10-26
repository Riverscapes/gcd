using System;

namespace GCDConsoleLib.ExtentAdjusters
{
    /// <summary>
    /// Extent adjusters for associated surfaces, error surfaces and reference error surfaces.
    /// </summary>
    /// <remarks>
    /// This type of extent adjuster cannot change ANYTHING. The output extent is locked down
    /// to the reference surface extent</remarks>
    public class ExtentAdjusterFixed : ExtentAdjusterWithReference
    {
        public ExtentAdjusterFixed(ExtentRectangle srcextent, ExtentRectangle refextent) : base(srcextent, refextent)
        {
        }

        public override ExtentAdjusterBase AdjustDimensions(decimal top, decimal right, decimal bottom, decimal left)
        {
            throw new Exception("It should not be possible to adjust the dimensions of a raster with a reference extent.");
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
