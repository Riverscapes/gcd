using System;
using System.Collections.Generic;
using GCDConsoleLib.GCD;

namespace GCDConsoleLib.Internal.Operators
{
    class CreateErrorRaster : CellByCellOperator<double>
    {
        private bool isMultiMethod;

        private Dictionary<string, ErrorRasterProperties> _props;
        private Dictionary<string, FISRasterOp> _fisops;
        private Dictionary<string, int> _assocRasters;

        private Vector _polymask;
        private string _fieldname;

        /// <summary>
        /// Single method Constructor
        /// </summary>
        /// <param name="rawDEM"></param>
        /// <param name="props"></param>
        /// <param name="outputRaster"></param>
        public CreateErrorRaster(Raster rawDEM, ErrorRasterProperties prop, Raster outputRaster) :
            base(new List<Raster> { rawDEM }, outputRaster)
        {
            isMultiMethod = false;
            _props = new Dictionary<string, ErrorRasterProperties>() { { "", prop } };

            if (prop.TheType == ErrorRasterProperties.ERPType.ASSOC)
            {
                AddInputRaster(prop.AssociatedSurface);
                // Now keep track so we can find it later
                _assocRasters = new Dictionary<string, int>() { { "", _rasters.Count - 1 } };
            }
            else if (prop.TheType == ErrorRasterProperties.ERPType.FIS)
                _fisops = new Dictionary<string, FISRasterOp>() { { "", new FISRasterOp(prop.FISInputs, prop.FISRuleFile) } };
        }

        /// <summary>
        /// Multi-method Constructor
        /// </summary>
        /// <param name="rawDEM"></param>
        /// <param name="PolygonMask"></param>
        /// <param name="MaskFieldName"></param>
        /// <param name="props"></param>
        /// <param name="outputRaster"></param>
        public CreateErrorRaster(Raster rawDEM, Vector PolygonMask, string MaskFieldName,
            Dictionary<string, ErrorRasterProperties> props, Raster outputRaster) :
            base(new List<Raster> { rawDEM }, outputRaster)
        {
            isMultiMethod = true;
            _polymask = PolygonMask;
            _fieldname = MaskFieldName;

            _props = new Dictionary<string, ErrorRasterProperties>();
            _assocRasters = new Dictionary<string, int>();
            _fisops = new Dictionary<string, FISRasterOp>();

            foreach (KeyValuePair<string, ErrorRasterProperties> kvp in props)
            {
                _props[kvp.Key] = kvp.Value;
                if (kvp.Value.TheType == ErrorRasterProperties.ERPType.ASSOC)
                {
                    AddInputRaster(kvp.Value.AssociatedSurface);
                    // Now keep track so we can find it later
                    _assocRasters[kvp.Key] =  _rasters.Count - 1;
                }
                else if (kvp.Value.TheType == ErrorRasterProperties.ERPType.FIS)
                    _fisops[kvp.Key] = new FISRasterOp(kvp.Value.FISInputs, kvp.Value.FISRuleFile);
            }
        }

        /// <summary>
        /// Based on what kind of error we have, operate on the cell
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <param name="stats"></param>
        public double CellChangeCalc(string propkey, List<double[]> data, int id)
        {
            switch (_props[propkey].TheType)
            {
                // This is easy. Just return a value from the correct raster
                case ErrorRasterProperties.ERPType.ASSOC:
                    return data[_assocRasters[propkey]][id];

                // This is easy. Just return a single value
                case ErrorRasterProperties.ERPType.UNIFORM:
                    return (double)_props[propkey].UniformValue;

                // For FIS we have to do a whole thing...
                case ErrorRasterProperties.ERPType.FIS:
                    _fisops[propkey].FISCellOp(data, id);
                    break;

                default:
                    throw new ArgumentException("Type not found");
            }
            // we should never get this far
            return OpNodataVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override double CellOp(List<double[]> data, int id)
        {
            // Speed things up by ignoring nodatas
            if (data[0][id] == _rasternodatavals[0])
                return OpNodataVal;

            // With multimethod errors we need to do some fancy footwork
            if (isMultiMethod)
            {
               decimal[] ptcoords = ChunkExtent.Id2XY(id);
                // Is this point in one (or more) of the shapes?
                List<string> shapes = _polymask.ShapesContainPoint((double)ptcoords[0], (double)ptcoords[1], _fieldname);

                // Now we need to decide what to do based on how many intersections we found.
                if (shapes.Count == 1)
                    CellChangeCalc(shapes[0], data, id);
                else if (shapes.Count > 1)
                    throw new NotImplementedException("Overlapping shapes is not yet supported");
                else
                    return OpNodataVal;
            }
            // Single method is easier
            else
                return CellChangeCalc("", data, id);

            // We should never get this far
            return OpNodataVal;
        }

    }

}
