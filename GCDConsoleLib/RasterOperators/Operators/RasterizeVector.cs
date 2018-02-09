using System;
using System.Collections.Generic;

namespace GCDConsoleLib.Internal.Operators
{
    class RasterizeVector : CellByCellOperator<int>
    {
        private List<string> _fieldvals;
        private string _fieldname;

        /// <summary>
        /// Rasterize a shp file. This is actually not doing the rasterization at all, just playing archaeology
        /// for what GDAL has done.
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="vInput"></param>
        /// <param name="output"></param>
        public RasterizeVector(Raster Template, Vector vInput, Raster output, string fieldname, 
            List<string> fieldvals) :
            base(new List<Raster> { Template }, vInput, new List<Raster> { output })
        {
            _fieldvals = fieldvals;
            _fieldname = fieldname;
        }

        /// <summary>
        /// Find the intersection of each cell 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="outputs"></param>
        /// <param name="id"></param>
        protected override void CellOp(List<int[]> data, List<int[]> outputs, int id)
        {
            outputs[0][id] = outNodataVals[0];

            if (_shapemask.Count > 0 && data[0][id] != inNodataVals[0])
            {
                decimal[] ptcoords = ChunkExtent.Id2XY(id);
                List<string> shapes = _polymask.ShapesContainPoint((double)ptcoords[0], (double)ptcoords[1], _fieldname, _shapemask);

                if (shapes.Count > 0) {
                    // Add a dictionary entry if we need to
                    if (!_fieldvals.Contains(shapes[0])) _fieldvals.Add(shapes[0]);

                    // 0 is our nodataval here so we pad the list by 1. 
                    // Now we just need to remember that the raster is perpertually off by one
                    outputs[0][id] = _fieldvals.IndexOf(shapes[0]) + 1;
                }
            }
        }
    }
}
