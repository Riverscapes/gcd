using System;
using System.Collections.Generic;
using GCDConsoleLib.Internal;
using GCDConsoleLib.GCD;

namespace GCDConsoleLib.Internal.Operators
{
    class GetChangeStatsSeg : GetChangeStats
    {
        private Dictionary<string, DoDStats> SegStats;
        private Vector _polymask;
        private string _fieldname;

        /// <summary>
        /// Pass-through constructor
        /// </summary>
        public GetChangeStatsSeg(Raster rInput, DoDStats theStats, Vector PolygonMask, string FieldName) :
            base(rInput, theStats)
        {
            SegStats = new Dictionary<string, DoDStats>();
            _polymask = PolygonMask;
            _fieldname = FieldName;
        }

        /// <summary>
        /// This is the actual implementation of the cell-by-cell logic
        /// </summary>
        protected override float CellOp(List<float[]> data, int id)
        {
            List<string> shapes = _polymask.ShapesContainPoint(23, 34, _fieldname);
            if (shapes.Count > 0)
            {
                foreach(string shp in shapes)
                {
                    if (!SegStats.ContainsKey(shp))
                        SegStats[shp] = new DoDStats(Stats);

                    DoDStats myStats = SegStats[shp];

                    CellChangeCalc(data, id, myStats);
                }
               
            }
            return 0;
        }
    }
}
