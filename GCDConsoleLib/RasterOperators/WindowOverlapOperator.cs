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
        protected int BufferCenterID { get { return BufferLength * _bufferCells + _bufferCells; } }

        public ExtentRectangle WinExtent;

        private List<List<T[]>> _chunkCache;
        List<T[]> dWindow; // this is our nxn window over which we are doing our masth
        private int _cacheIdx;

        public WindowOverlapOperator(List<Raster> rRasters, int bufferCells, Raster rOutputRaster = null) :
            base(rRasters, rOutputRaster)
        {
            _bufferCells = bufferCells;
            _chunkCache = new List<List<T[]>>(BufferLength);

            dWindow = new List<T[]>();
            foreach (Raster rN in _rasters)
                dWindow.Add(new T[BufferCellNum]);

            WinExtent = new ExtentRectangle(ChunkWindow.Top - OpExtent.CellHeight, ChunkWindow.Left - OpExtent.CellWidth, OpExtent.CellHeight, OpExtent.CellWidth, BufferLength, BufferLength);

        }

        /// <summary>
        /// We need a reliable way to increment our rolling buffer pointer
        /// </summary>
        private void IdxIncrement() { _cacheIdx = (_cacheIdx + 1) % (BufferLength); }

        private int IdxTranslate(int num) { return (_cacheIdx + num) % (BufferLength); }

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
            {
                List<T[]> inputChunkList = new List<T[]>();
                for (int did = 0; did < _rasters.Count; did++)
                {
                    inputChunkList.Add(inputchunk);
                }
                _chunkCache.Add(inputChunkList);
            }

            // starting cache id is always in the middle
            _chunkCache.Add(data);
            // Now try to fill the end of the window
            while (_chunkCache.Count < BufferLength)
            {
                nextChunk();
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
            // Initialize the Window properly
            WinExtent.Left = ChunkWindow.Left - ChunkWindow.CellWidth;
            WinExtent.Top = ChunkWindow.Top - ChunkWindow.CellHeight;

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
                // Move the window by one cell
                WinExtent.Left += ChunkWindow.CellWidth;
                // Now loop over cells in the window
                for (int wId = 0; wId < BufferCellNum; wId++)
                {
                    // Get the rowcol translation of this window id
                    Tuple<int, int> rowcol = WinExtent.Id2RowCol(wId);
                    // The row of the window tells us which cache to use
                    int cacheNum = IdxTranslate(rowcol.Item1 - _bufferCells);

                    for (int dId = 0; dId < data.Count; dId++)
                    {
                        // Edge cases to the left and right of the chunk data become nodata 
                        // (top and bottom are handled by pre-filling the cache)
                        if ((id == 0 && rowcol.Item2 == 0) || (id == ChunkWindow.rows - 1 && rowcol.Item2 == WinExtent.rows - 1))
                            dWindow[dId][wId] = OpNodataVal;
                        else
                            dWindow[dId][wId] = _chunkCache[cacheNum][dId][id];
                    }
                }
                outChunk[id] = WindowOp(ref dWindow);
            }
        }
    }

}
