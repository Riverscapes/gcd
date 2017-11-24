using System;
using System.Collections.Generic;
using GCDConsoleLib.Utility;

namespace GCDConsoleLib.Internal.Operators
{
    public class RasterMath<T> : CellByCellOperator<T>
    {
        private bool _scalar;
        private RasterOperators.MathOpType _type;
        private double _origOperand;
        private T _operand;

        /// <summary>
        /// Pass-through constructor for Raster Math
        /// </summary>
        /// <param name="otType"></param>
        /// <param name="rInput"></param>
        /// <param name="dOperand"></param>
        /// <param name="sOutputRaster"></param>
        public RasterMath(RasterOperators.MathOpType otType, Raster rInput, decimal dOperand, 
            Raster rOutputRaster) : base(new List<Raster> { rInput }, rOutputRaster)
        {
            _type = otType;
            _scalar = true;
            _origOperand = (double)dOperand;
            _operand = (T)Convert.ChangeType(dOperand, typeof(T));
        }

        public RasterMath(RasterOperators.MathOpType otType, Raster rInputA, 
            Raster rInputB, Raster rOutputRaster) :
            base(new List<Raster> { rInputA, rInputB }, rOutputRaster)
        {
            _type = otType;
            _scalar = false;
        }

        protected override T CellOp(List<T[]> data, int id)
        {
            T val = OpNodataVal;
            if (_scalar)
            {
                if (!data[0][id].Equals(_rasternodatavals[0]))
                {
                    switch (_type)
                    {
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
            else
            {
                if (!data[0][id].Equals(_rasternodatavals[0]) && !data[1][id].Equals(_rasternodatavals[1]))
                {
                    switch (_type)
                    {
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
            return val;
        }
    }
}
