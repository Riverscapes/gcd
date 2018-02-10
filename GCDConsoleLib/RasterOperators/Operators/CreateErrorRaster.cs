using System;
using System.Collections.Generic;
using GCDConsoleLib.GCD;
using System.Linq;

namespace GCDConsoleLib.Internal.Operators
{
    class CreateErrorRaster : CellByCellOperator<double>
    {
        private Dictionary<string, ErrorRasterProperties> _props;
        private Dictionary<string, FISRasterOp> _fisops;
        private Dictionary<string, int> _assocRasters;
        private Dictionary<string, List<int>> _fisinputs;

        private string _fieldname;

        // When we use rasterized polygons we use this as the field vals
        private Dictionary<int, string> _rasterVectorFieldVals;

        /// <summary>
        /// Single method Constructor
        /// </summary>
        /// <param name="rawDEM"></param>
        /// <param name="props"></param>
        /// <param name="outputRaster"></param>
        public CreateErrorRaster(Raster rawDEM, ErrorRasterProperties prop, Raster rOutputRaster) :
            base(new List<Raster> { rawDEM }, new List<Raster> { rOutputRaster })
        {
            _fisinputs = new Dictionary<string, List<int>>();
            _props = new Dictionary<string, ErrorRasterProperties>() { { "", prop } };

            if (prop.TheType == ErrorRasterProperties.ERPType.ASSOC)
            {
                AddInputRaster(prop.AssociatedSurface);
                // Now keep track so we can find it later
                _assocRasters = new Dictionary<string, int>() { { "", _inputRasters.Count - 1 } };
            }
            else if (prop.TheType == ErrorRasterProperties.ERPType.FIS)
            {
                _fisinputs[""] = new List<int>();
                _fisops = new Dictionary<string, FISRasterOp>();
                _fisops[""] = new FISRasterOp(prop.FISInputs, prop.FISRuleFile);
                // Add the FIS rasters to these inputs so we can use them and keep track of their indices
                // so we can slice this data out and feed it to the FIS operator
                foreach (Raster fisinput in prop.FISInputs.Values)
                {
                    _fisinputs[""].Add(_inputRasters.Count);
                    AddInputRaster(fisinput);
                }
            }
        }

        /// <summary>
        /// Multi-method Constructor (Pure Vector)
        /// </summary>
        /// <param name="rawDEM"></param>
        /// <param name="PolygonMask"></param>
        /// <param name="MaskFieldName"></param>
        /// <param name="props"></param>
        /// <param name="outputRaster"></param>
        public CreateErrorRaster(Raster rawDEM, Vector PolygonMask, string MaskFieldName,
            Dictionary<string, ErrorRasterProperties> props, Raster rOutputRaster) :
            base(new List<Raster> { rawDEM }, PolygonMask, new List <Raster> { rOutputRaster })
        {
            _initMultiMethod(MaskFieldName, props);
        }

        /// <summary>
        /// Multi-method Constructor (Rasterized Vector Method)
        /// </summary>
        /// <param name="rawDEM"></param>
        /// <param name="PolygonMask"></param>
        /// <param name="MaskFieldName"></param>
        /// <param name="props"></param>
        /// <param name="rOutputRaster"></param>
        public CreateErrorRaster(Raster rawDEM, VectorRaster rPolygonMask, string MaskFieldName,
            Dictionary<string, ErrorRasterProperties> props, Raster rOutputRaster) :
            base(new List<Raster> { rawDEM }, rPolygonMask, new List<Raster> { rOutputRaster })
        {
            _initMultiMethod(MaskFieldName, props);

            _rasterVectorFieldVals = rPolygonMask.FieldValues;
        }

        /// <summary>
        /// Code we were using in two constructors made sense to combine
        /// </summary>
        /// <param name="MaskFieldName"></param>
        /// <param name="props"></param>
        private void _initMultiMethod(string MaskFieldName, Dictionary<string, ErrorRasterProperties> props)
        {
            _fieldname = MaskFieldName;
            _fisinputs = new Dictionary<string, List<int>>();

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
                    _assocRasters[kvp.Key] = _inputRasters.Count - 1;
                }
                else if (kvp.Value.TheType == ErrorRasterProperties.ERPType.FIS)
                {
                    _fisinputs[kvp.Key] = new List<int>();
                    _fisops[kvp.Key] = new FISRasterOp(kvp.Value.FISInputs, kvp.Value.FISRuleFile);
                    // Add the FIS rasters to these inputs so we can use them and keep track of their indices
                    // so we can slice this data out and feed it to the FIS operator
                    foreach (Raster fisinput in kvp.Value.FISInputs.Values)
                    {
                        _fisinputs[kvp.Key].Add(_inputRasters.Count);
                        AddInputRaster(fisinput);
                    }
                }
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
            if (!_props.ContainsKey(propkey))
                return outNodataVals[0];

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
                    // We use Linq to slice the data and only send the appropriate 
                    // inputs to the FIS Function
                    return _fisops[propkey].FISCellOp(data.Where((arr, ind) => _fisinputs[propkey].Contains(ind)).ToList(), id);

                default:
                    throw new ArgumentException("Type not found");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override void CellOp(List<double[]> data, List<double[]> outputs, int id)
        {
            outputs[0][id] = outNodataVals[0];

            // Speed things up by ignoring nodatas
            if (data[0][id] == inNodataVals[0])                
                return;

            // With multimethod errors we need to do some fancy footwork
            if (_hasVectorPolymask)
            {
                if (_shapemask.Count > 0)
                {
                    decimal[] ptcoords = ChunkExtent.Id2XY(id);
                    // Is this point in one (or more) of the shapes?
                    List<string> shapes = _polymask.ShapesContainPoint((double)ptcoords[0], (double)ptcoords[1], _fieldname);

                    // Now we need to decide what to do based on how many intersections we found.
                    if (shapes.Count == 1)
                        outputs[0][id] = CellChangeCalc(shapes[0], data, id);
                    else if (shapes.Count > 1)
                        throw new NotImplementedException("Overlapping shapes is not yet supported");
                }
            }
            else if (_hasRasterizedPolymask)
            {
                double rPolymaskVal = data[_inputRasters.Count - 1][id];
                if (rPolymaskVal != inNodataVals[_inputRasters.Count - 1])
                {
                    string fldVal = _rasterVectorFieldVals[(int)rPolymaskVal];
                    outputs[0][id] = CellChangeCalc(fldVal, data, id);
                }
            }
            // Single method is easier
            else
                outputs[0][id] = CellChangeCalc("", data, id);
        }

    }

}
