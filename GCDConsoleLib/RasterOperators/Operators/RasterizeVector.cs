using System;
using System.Collections.Generic;

namespace GCDConsoleLib.Internal.Operators
{
    class RasterizeVector : CellByCellOperator<int>
    {
        /// <summary>
        /// Rasterize a shp file
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="vInput"></param>
        /// <param name="output"></param>
        public RasterizeVector(Vector vInput, Raster output) :
            base(new List<Raster> {}, vInput, new List<Raster> { output })  { }

        /// <summary>
        /// Find the intersection of each cell 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="outputs"></param>
        /// <param name="id"></param>
        protected override void CellOp(List<int[]> data, List<int[]> outputs, int id)
        {
            outputs[0][id] = outNodataVals[0];
            if (_shapemask.Count > 0)
            {
                decimal[] ptcoords = ChunkExtent.Id2XY(id);
                long? fid = _polymask.ShapeContainPoint((double)ptcoords[0], (double)ptcoords[1], _shapemask);
                if (fid != null) outputs[0][id] = (int)fid;
            }               
        }
    }
}
