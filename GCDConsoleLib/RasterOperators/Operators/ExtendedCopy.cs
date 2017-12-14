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
            base(new List<Raster> { rInput }, new List<Raster> { rOutputRaster })
        {
            SetOpExtent(newRect);
        }

        /// <summary>
        /// The actual cell operation
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override void CellOp(List<T[]> data, List<T[]> outputs, int id)
        {
            if (data[0][id].Equals(inNodataVals[0]))
                outputs[0][id] = outNodataVals[0];
            else
                outputs[0][id] = data[0][id];
        }

    }
}
