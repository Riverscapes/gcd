using System.Collections.Generic;

namespace GCDConsoleLib.Internal.Operators
{

    public class ExtendedCopy<T> : CellByCellOperator<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="rOutputRaster"></param>
        /// <param name="newRect"></param>
        public ExtendedCopy(Raster rInput, Raster rOutputRaster, ExtentRectangle newRect) :
            base(new List<Raster> { rInput }, rOutputRaster)
        {
            SetOpExtent(newRect);
        }

        /// <summary>
        /// The actual cell operation
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override T CellOp(List<T[]> data, int id)
        {
            if (data[0][id].Equals(_rasternodatavals[0]))
                return OpNodataVal;
            else
                return data[0][id];
        }

    }
}
