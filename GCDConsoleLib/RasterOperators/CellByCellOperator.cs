using System;
using System.Collections.Generic;


namespace GCDConsoleLib.Operators.Base
{
    public abstract class CellByCellOperator : BaseOperator
    {
        public CellByCellOperator(List<Raster> rRasters, ref Raster rOutputRaster) :
            base(rRasters, ref rOutputRaster)
        {
        }

        /// <summary>
        /// The individual operators must implement this.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected abstract double CellOp(ref List<double[]> data, int id);

        protected override void ChunkOp(ref List<double[]> data, ref double[] outChunk)
        {
            for (int id = 0; id < data[0].Length; id++)
            {
                outChunk[id] = CellOp(ref data, id);
            }
        }
    }

}
