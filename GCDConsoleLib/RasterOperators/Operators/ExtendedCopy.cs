using System.Collections.Generic;

namespace GCDConsoleLib.Internal.Operators
{

    public class ExtendedCopy<T> : CellByCellOperator<T>
    {
        /// <summary>
        /// Pass-through constructor for Extended Copy
        /// </summary>
        public ExtendedCopy(Raster rInput, Raster rOutputRaster, ExtentRectangle newRect) :
            base(new List<Raster> { rInput }, rOutputRaster)
        {
            SetOpExtent(newRect);
        }

        /// <summary>
        /// This is the actual implementation of the cell-by-cell logic
        /// </summary>
        protected override T CellOp(List<T[]> data, int id)
        {
            if (data[0][id].Equals(_rasternodatavals[0]))
                return OpNodataVal;
            else
                return data[0][id];
        }

    }
}
