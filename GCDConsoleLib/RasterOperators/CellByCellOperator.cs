using System;
using System.Collections.Generic;

namespace GCDConsoleLib.Internal
{
    public abstract class CellByCellOperator<T> : BaseOperator<T>
    {
        // Multi-use dictionary that gets reset after every chunk
        protected List<long> _shapemask;
        protected Vector _polymask;

        // RASTER POLYMASK: 
        protected bool _hasVectorPolymask;
        protected bool _hasRasterizedPolymask;

        /// <summary>
        /// Just a simple pass-through constructor
        /// </summary>
        /// <param name="rRasters"></param>
        /// <param name="rOutputRaster"></param>
        public CellByCellOperator(List<Raster> rRasters, List<Raster> rOutputRasters = null) :
            base(rRasters, rOutputRasters)  {

            _polymask = null;
            _hasVectorPolymask = false;
            _hasRasterizedPolymask = false;
        }

        /// <summary>
        /// Cell by cell op using using a VECTOR for some purpose
        /// </summary>
        /// <param name="rRasters"></param>
        /// <param name="PolygonMask"></param>
        /// <param name="rOutputRasters"></param>
        public CellByCellOperator(List<Raster> rRasters, Vector PolygonMask, List<Raster> rOutputRasters = null) :
            base(rRasters, rOutputRasters)  {

            _polymask = PolygonMask;
            _hasVectorPolymask = true;
            _hasRasterizedPolymask = false;
        }

        /// <summary>
        /// Cell by cell op using using a RASTERIZED VECTOR for some purpose
        /// </summary>
        /// <param name="rRasters"></param>
        /// <param name="rPolygonMask"></param>
        /// <param name="vPolygonMask"></param>
        /// <param name="rOutputRasters"></param>
        public CellByCellOperator(List<Raster> rRasters, VectorRaster rPolygonMask, List<Raster> rOutputRasters = null) :
            base(rRasters, rOutputRasters)
        {
            _hasVectorPolymask = false;
            _hasRasterizedPolymask = true;

            // Make sure we add the rasterized vector as the last item we can look up
            AddInputRaster(rPolygonMask);
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
            if (_hasVectorPolymask)
                _shapemask = _polymask.FIDIntersectExtent(ChunkExtent);

            // We either have an input or an output (but never neither) Figure out the buffer size
            int buffSize;
            if (data.Count > 0)
                buffSize = data[0].Length;
            else buffSize = outChunks[0].Length;

            for (int id = 0; id < buffSize; id++)
            {
#if DEBUG
                num_calcs++;
#endif
                CellOp(data, outChunks, id);
            }
        }

    }

}
