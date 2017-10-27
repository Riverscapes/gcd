using System;
using System.Collections.Generic;
using GCDConsoleLib.Common.Extensons;

namespace GCDConsoleLib.Internal
{
    /// <summary>
    /// The overlap operator works by operating over a moving window
    /// </summary>
    public abstract class WindowOverlapOperator<T> : BaseOperator<T>
    {
        protected int _bufferCells;
        protected int BufferLength { get { return 1 + (2 * _bufferCells); } }
        protected int BufferCellNum { get { return (int)Math.Pow(BufferLength, 2); } }
        protected int BufferCenterID { get { return BufferLength * _bufferCells + _bufferCells;  } }

        private List<List<T[]>> _chunkCache;
        List<T[]> dWindow; // this is our nxn window over which we are doing our masth
        private int _cacheIdx;

        public WindowOverlapOperator(List<Raster> rRasters, ref Raster rOutputRaster, int bufferCells) :
            base(rRasters, rOutputRaster)
        {
            dWindow = new List<T[]>();
            _bufferCells = bufferCells;
            _chunkCache = new List<List<T[]>>(BufferLength);
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
        protected abstract T WindowOp(ref List<T[]> data);



        /// <summary>
        /// First time through we need to setup the buffer properly
        /// </summary>
        /// <param name="data"></param>
        private void firstTime(ref List<T[]> data)
        {
            // Fill the rows above the extent with nodatavals
            T[] inputchunk = new T[ChunkWindow.cols * ChunkWindow.rows];
            inputchunk.Fill(OpNodataVal);
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


        protected override void ChunkOp(ref List<T[]> data, ref T[] outChunk)
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
                    if (id < col || (id) > ChunkWindow.cols)
                        // Set Nodata For each layer we care about:
                        for (int dId = 0; dId < data.Count; dId++)
                            dWindow[dId][wId] = OpNodataVal;
                    else
                        // Set Nodata for each layer we care about:
                        for (int dId = 0; dId < _chunkCache.Count; dId++)
                            dWindow[dId][wId] = data[dId][id];
                }
                // Now call the cell op on this chunk
                outChunk[id] = WindowOp(ref data);
            }
        }
    }

}
