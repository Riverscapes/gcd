using System;
using System.Collections.Generic;
using GCDConsoleLib.Common.Extensons;

namespace GCDConsoleLib.Operators.Base
{
    /// <summary>
    /// The overlap operator works by operating over a moving window
    /// </summary>
    public abstract class WindowOverlapOperator : BaseOperator
    {
        private int _bufferCells;
        private int BufferLength { get { return 1 + (2 * _bufferCells); } }
        private int BufferCellNum { get { return (int)Math.Pow(BufferLength, 2); } }

        private List<List<double[]>> _chunkCache;
        List<double[]> dWindow; // this is our nxn window over which we are doing our masth
        private int _cacheIdx;

        public WindowOverlapOperator(List<Raster> rRasters, ref Raster rOutputRaster, int bufferCells) :
            base(rRasters, ref rOutputRaster)
        {
            dWindow = new List<double[]>();
            _bufferCells = bufferCells;
            _chunkCache = new List<List<double[]>>(BufferLength);
        }

        /// <summary>
        /// We need a reliable way to increment our rolling buffer pointer
        /// </summary>
        private void IdxIncrement() { _cacheIdx = (_cacheIdx + 1) % (BufferLength); }


        /// <summary>
        /// WindowOp must be implemented by the operation. It takes an (nxn) double[] and spits back a single number
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected abstract double WindowOp(ref List<double[]> data, int id);

        /// <summary>
        /// First time through we need to setup the buffer properly
        /// </summary>
        /// <param name="data"></param>
        private void firstTime(ref List<double[]> data)
        {
            // Fill the rows above the extent with nodatavals
            double[] inputchunk = new double[_chunkWindow.cols * _chunkWindow.rows];
            inputchunk.Fill(OpNoDataVal);
            for (int idx = 0; idx < _bufferCells; idx++)
                _chunkCache.Add(null);

            // starting cache id is always in the middle
            _cacheIdx = _bufferCells;
            _chunkCache.Add(data);
            // Now try to fill the end of the window
            while (_chunkCache.Count < BufferLength)
            {
                GetChunk(ref data);
                _chunkCache.Add(data);
            }
            // Put our index right in the middle
            _cacheIdx = _bufferCells;
        }


        protected override void ChunkOp(ref List<double[]> data, ref double[] outChunk)
        {
            /** Don't forget the numbering scheme:
             *  0  1  2  or   0  1  2  3  4
             *  3  4  5       5  6  7  8  9
             *  6  7  8       10 11 12 13 14
             *                15 16 17 18 19
             *                20 21 22 23 24
             */

            // First time around we need to set up the cache properly and force the look-ahead forward
            if (_chunkCache.Count == 0)
                firstTime(ref data);
            else
            {
                // Put our data into the right place in the rolling cache
                IdxIncrement();
                _chunkCache[_cacheIdx] = data;
            }

            // Loop over the Chunk (line) 
            for (int id = 0; id < outChunk.GetLength(0); id++)
            {
                // Loop over the cells in the window (see numbering scheme in the comments above)
                for (int wId = 0; wId < BufferCellNum; wId++)
                {
                    int col = wId % BufferLength;
                    int row = (wId - (wId % BufferLength)) / BufferLength;
                    if (id < col || (id) > _chunkWindow.cols)
                        // Set Nodata For each layer we care about:
                        for (int dId = 0; dId < data.Count; dId++)
                            dWindow[dId][wId] = OpNoDataVal;
                    else
                        // Set Nodata for each layer we care about:
                        for (int dId = 0; dId < _chunkCache.Count; dId++)
                            dWindow[dId][wId] = data[dId][id];
                }
                // Now call the cell op on this chunk
                outChunk[id] = WindowOp(ref data, id);
            }
        }
    }

}
