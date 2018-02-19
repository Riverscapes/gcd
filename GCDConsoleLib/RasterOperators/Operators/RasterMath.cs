using System;
using System.Collections.Generic;
using GCDConsoleLib.Utility;

namespace GCDConsoleLib.Internal.Operators
{
    public class RasterMath<T> : CellByCellOperator<T>
    {
        private bool _scalar;
        private bool _masked;
        private RasterOperators.MathOpType _type;
        private double _origOperand;
        private T _operand;

        /// <summary>
        /// Pass-through constructor for Raster Math with a scalar operand
        /// </summary>
        /// <param name="otType"></param>
        /// <param name="rInput"></param>
        /// <param name="dOperand"></param>
        /// <param name="sOutputRaster"></param>
        public RasterMath(RasterOperators.MathOpType otType, Raster rInput, decimal dOperand, 
            Raster rOutputRaster) : base(new List<Raster> { rInput }, new List<Raster> { rOutputRaster })
        {
            _type = otType;
            _scalar = true;
            _masked = false;
            _origOperand = (double)dOperand;
            _operand = (T)Convert.ChangeType(dOperand, typeof(T));
        }

        /// <summary>
        /// This second constructor is for when we use two rasters
        /// </summary>
        /// <param name="otType"></param>
        /// <param name="rInputA"></param>
        /// <param name="rInputB"></param>
        /// <param name="rOutputRaster"></param>
        public RasterMath(RasterOperators.MathOpType otType, Raster rInputA, 
            Raster rInputB, Raster rOutputRaster) :
            base(new List<Raster> { rInputA, rInputB }, new List<Raster> { rOutputRaster })
        {
            _type = otType;
            _scalar = false;
            _masked = false;

        }


        /// <summary>
        /// Pass in a regular vector mask (slow)
        /// </summary>
        /// <param name="otType"></param>
        /// <param name="rInputA"></param>
        /// <param name="rInputB"></param>
        /// <param name="rPolymask"></param>
        /// <param name="rOutputRaster"></param>
        public RasterMath(RasterOperators.MathOpType otType, Raster rInputA,
            Raster rInputB, Vector vPolymask, Raster rOutputRaster) :
           base(new List<Raster> { rInputA, rInputB }, vPolymask, new List<Raster> { rOutputRaster })
        {
            _type = otType;
            _scalar = false;
            _masked = true;
        }

        /// <summary>
        /// Pass in a rasterized vector mask
        /// </summary>
        /// <param name="otType"></param>
        /// <param name="rInputA"></param>
        /// <param name="rInputB"></param>
        /// <param name="rPolymask"></param>
        /// <param name="rOutputRaster"></param>
        public RasterMath(RasterOperators.MathOpType otType, Raster rInputA,
            Raster rInputB, VectorRaster rPolymask, Raster rOutputRaster) :
           base(new List<Raster> { rInputA, rInputB }, rPolymask, new List<Raster> { rOutputRaster })
        {
            _type = otType;
            _scalar = false;
            _masked = true;
        }

        /// <summary>
        /// The actual cell-by-cell operations
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override void CellOp(List<T[]> data, List<T[]> outputs, int id)
        {
            T val = outNodataVals[0];
            // This is the raster and scalar case
            if (_scalar)
            {
                if (!data[0][id].Equals(inNodataVals[0]))
                {
                    switch (_type)
                    {
                        // Choose your math operation
                        case RasterOperators.MathOpType.Addition:
                            val = DynamicMath.Add(data[0][id],_operand);
                            break;
                        case RasterOperators.MathOpType.Subtraction:
                            val = DynamicMath.Subtract(data[0][id], _operand);
                            break;
                        case RasterOperators.MathOpType.Multipication:
                            val = DynamicMath.Multiply(data[0][id], _operand);
                            break;
                        case RasterOperators.MathOpType.Division:
                            val = DynamicMath.Divide(data[0][id], _operand);
                            break;
                    }
                }
            }
            // This is the two raster case
            else
            {
                bool masked = false;

                // Pure vector method. (This is the slow way)
                if (_hasVectorPolymask)
                {
                    decimal[] ptcoords = ChunkExtent.Id2XY(id);
                    List<long> shapes = _polymask.ShapesContainPoint((double)ptcoords[0], (double)ptcoords[1], _shapemask);
                    if (shapes.Count == 0) masked = true;
                }

                // Rasterized vector method
                else if (_hasRasterizedPolymask && data.Count == 3 && data[2][id].Equals(inNodataVals[2]))
                    masked = true;


                if (!data[0][id].Equals(inNodataVals[0]) && !data[1][id].Equals(inNodataVals[1]) && !masked)
                {
                    switch (_type)
                    {
                        // Choose your math operation
                        case RasterOperators.MathOpType.Addition:
                            val = DynamicMath.Add(data[0][id], data[1][id]);
                            break;
                        case RasterOperators.MathOpType.Subtraction:
                            val = DynamicMath.Subtract(data[0][id], data[1][id]);
                            break;
                        case RasterOperators.MathOpType.Multipication:
                            val = DynamicMath.Multiply(data[0][id], data[1][id]);
                            break;
                        case RasterOperators.MathOpType.Division:
                            val = DynamicMath.Divide(data[0][id], data[1][id]);
                            break;
                    }
                }
            }

            outputs[0][id] = val;
        }
    }
}
