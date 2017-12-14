using System;
using System.Collections.Generic;

namespace GCDConsoleLib.Internal
{
    public abstract class CellByCellOperator<T> : BaseOperator<T>
    {
        /// <summary>
        /// Just a simple pass-through constructor
        /// </summary>
        /// <param name="rRasters"></param>
        /// <param name="rOutputRaster"></param>
        public CellByCellOperator(List<Raster> rRasters, List<Raster> rOutputRasters = null) :
            base(rRasters, rOutputRasters)  { }

        /// <summary>
        /// The individual operators must implement this.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected abstract void CellOp(List<T[]> data, List<T[]> outputs, int id);

        /// <summary>
        /// This is how we loop over a chunk
        /// </summary>
        /// <param name="data"></param>
        /// <param name="outChunk"></param>
        protected override void ChunkOp(List<T[]> data, List<T[]> outChunks)
        {
            for (int id = 0; id < data[0].Length; id++)
                CellOp(data, outChunks, id);
        }
    }

}
