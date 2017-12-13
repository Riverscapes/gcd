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
        protected int BufferCells;
        protected int BufferLength { get { return 1 + (2 * BufferCells); } }
        protected int BufferCellNum { get { return (int)Math.Pow(BufferLength, 2); } }
        protected int BufferCenterID { get { return BufferLength * BufferCells + BufferCells; } }
        protected decimal OriginalOpBottom { get; private set; }
        public ExtentRectangle WindowExtent;

        private List<List<T[]>> _chunkCache;
        List<T[]> dWindow; // this is our nxn window over which we are doing our masth
        private int _cacheIdx;

        /// <summary>
        /// Output Constructor
        /// </summary>
        /// <param name="rRasters"></param>
        /// <param name="bufferCells"></param>
        /// <param name="rOutputRaster"></param>
        public WindowOverlapOperator(List<Raster> rRasters, int bufferCells, List<Raster> rOutputRasters = null) :
            base(rRasters, rOutputRasters)
        {
            BufferCells = bufferCells;
            _vOffset = -BufferCells;
            _chunkCache = new List<List<T[]>>(BufferLength);

            dWindow = new List<T[]>();
            foreach (Raster rN in _inputRasters)
                dWindow.Add(new T[BufferCellNum]);

            // 1 is the convention for windowed mode for good reason
            ChunkExtent.Rows = 1;
            ChunkExtent.Cols = OpExtent.Cols;

            // We add rows to the end so the operation goes over the end of the file
            OriginalOpBottom = OpExtent.Bottom;
            OpExtent.Rows += BufferCells;
            WindowExtent = new ExtentRectangle(ChunkExtent.Top - OpExtent.CellHeight, 
                ChunkExtent.Left - OpExtent.CellWidth, 
                OpExtent.CellHeight, 
                OpExtent.CellWidth, 
                BufferLength, 
                BufferLength);

        }

        /// <summary>
        /// We need a reliable way to increment our rolling buffer pointer
        /// </summary>
        private void IdxIncrement() { _cacheIdx = (_cacheIdx + 1) % BufferLength; }

        /// <summary>
        /// Convert a window row to a cache id so we can figure out which cache line to draw from 
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private int Row2CacheId(int row) { return (_cacheIdx + (row - 1) - BufferCells + BufferLength) % BufferLength; }

        /// <summary>
        /// WindowOp must be implemented by the operation. It takes an (nxn) double[] and spits back a single number
        /// </summary>
        /// <param name="windowData"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected abstract void WindowOp(List<T[]> windowData, List<T[]> outBuffers, int id);

        private List<T[]> NoDataChunkList()
        {
            List<T[]> inputChunkList = new List<T[]>();
            for (int did = 0; did < _inputRasters.Count; did++)
            {
                T[] inputchunk = new T[ChunkExtent.Rows * ChunkExtent.Cols];
                inputchunk.Fill(inNodataVals[did]);
                inputChunkList.Add(inputchunk);
            }
            return inputChunkList;
        }

        /// <summary>
        /// First time through we need to setup the buffer properly
        /// </summary>
        /// <param name="data"></param>
        private void firstTime(List<T[]> data)
        {
            // Fill the rows above the extent with nodatavals
            // 1 row is the convention for this chunksize
            for (int idx = 0; idx < BufferCells; idx++)
                _chunkCache.Add(NoDataChunkList());

            // starting cache id is always in the middle
            _chunkCache.Add(data.Clone<T[]>());

            // Now try to fill the end of the window
            while (_chunkCache.Count < BufferLength)
            {
                nextChunk();
                GetChunk(data);
                _chunkCache.Add(data.Clone<T[]>());
            }
            // Put our index right in the middle
            _cacheIdx = BufferCells;
        }

        protected override void ChunkOp(List<T[]> data, List<T[]> outBuffers)
        {
            /** Don't forget the numbering scheme:
             *  0  1  2  or   0  1  2  3  4
             *  3  4  5       5  6  7  8  9
             *  6  7  8       10 11 12 13 14
             *                15 16 17 18 19
             *                20 21 22 23 24
             */
            // Initialize the Window properly
            WindowExtent.Left = ChunkExtent.Left - ChunkExtent.CellWidth;
            WindowExtent.Top = ChunkExtent.Top - ChunkExtent.CellHeight;

            // First time around we need to set up the cache properly and force the look-ahead forward
            if (_chunkCache.Count == 0)
                firstTime(data);
            // When we get to the end of the file we need to pad with nodata values at the bottom
            //else if (ChunkExtent.Top > OriginalOpBottom)
            else if ((ChunkExtent.Top * ChunkExtent.CellHeight) >= (OriginalOpBottom * ChunkExtent.CellHeight))
            {
                IdxIncrement();
                _chunkCache[Row2CacheId(BufferLength)] = NoDataChunkList();
            }
            else
            {
                // Put our data into the right place in the rolling cache
                IdxIncrement();
                _chunkCache[Row2CacheId(BufferLength)] = data.Clone<T[]>();
            }

            // Loop over the cells in the Chunk (line) 
            for (int id = 0; id < data[0].GetLength(0); id++)
            {
                // Now loop over cells in the window
                for (int wId = 0; wId < BufferCellNum; wId++)
                {
                    // Get the rowcol translation of this window id
                    int[] winRowCol = WindowExtent.Id2RowCol(wId);
                    // The row of the window tells us which cache to use
                    int cacheNum = Row2CacheId(winRowCol[0]);
                    // This is the column ID of the cache column in window coordinates 
                    int wid2cid = id + (winRowCol[1]-1) - BufferCells;
                    // Now loop over all the data values (number of rasters)
                    for (int dId = 0; dId < data.Count; dId++)
                    {
                        // Edge cases to the left and right of the chunk data become nodata 
                        // (top and bottom are handled by pre-filling the cache)
                        if (wid2cid < 0 || wid2cid >= ChunkExtent.Cols)
                            dWindow[dId][wId] = inNodataVals[dId];
                        else
                            dWindow[dId][wId] = _chunkCache[cacheNum][dId][wid2cid];
                    }
                }
                // DEBUG TEST
                //dWindow[0].Make2DArray(BufferLength, BufferLength).DebugPrintGrid(OpNodataVal);
                WindowOp(dWindow, outBuffers, id);
                // Move the window by one cell to the right
                WindowExtent.Left += ChunkExtent.CellWidth;
            }
        }
    }

}
