using System;
using System.Collections.Generic;

namespace GCDConsoleLib.Internal
{
    public abstract class CellByCellOperator<T> : BaseOperator<T>
    {
        // Multi-use dictionary that gets reset after every chunk
        protected List<long> _shapemask;
        protected Vector _polymask;
        protected bool isBudgSeg;

        /// <summary>
        /// Just a simple pass-through constructor
        /// </summary>
        /// <param name="rRasters"></param>
        /// <param name="rOutputRaster"></param>
        public CellByCellOperator(List<Raster> rRasters, List<Raster> rOutputRasters = null) :
            base(rRasters, rOutputRasters)  {
            isBudgSeg = false;

        }

        /// <summary>
        /// Budget Seggregation case
        /// </summary>
        /// <param name="rRasters"></param>
        /// <param name="PolygonMask"></param>
        /// <param name="rOutputRasters"></param>
        public CellByCellOperator(List<Raster> rRasters, Vector PolygonMask, List<Raster> rOutputRasters = null) :
            base(rRasters, rOutputRasters)  {

            _polymask = PolygonMask;
            isBudgSeg = true;

        }

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
            // First check if this chunk intersects with any of the shapes and filter the list
            if (isBudgSeg)
                _shapemask = _polymask.FIDIntersectExtent(ChunkExtent);

            for (int id = 0; id < data[0].Length; id++)
                CellOp(data, outChunks, id);
        }

    }

}
