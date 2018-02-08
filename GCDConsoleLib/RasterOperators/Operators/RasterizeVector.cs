using System;
using System.Collections.Generic;
using GCDConsoleLib.Common.Extensons;
using GCDConsoleLib.Utility;

namespace GCDConsoleLib.Internal.Operators
{
    class RasterizeVector : CellByCellOperator<int>
    {

        public RasterizeVector(Raster rInput, Vector vInput, Raster output) :
            base(new List<Raster> { rInput }, vInput, new List<Raster> { output })
        { }

        protected override void CellOp(List<int[]> data, List<int[]> outputs, int id)
        {
            if (_shapemask.Count > 0)
            {
                decimal[] ptcoords = ChunkExtent.Id2XY(id);
                List<long> fids = _polymask.ShapesContainPoint((double)ptcoords[0], (double)ptcoords[1], _shapemask);
                if (fids.Count > 0)
                    outputs[0][id] = (int)fids[0];
            }
            else
                outputs[0][id] = outNodataVals[0];
        }
    }
}
