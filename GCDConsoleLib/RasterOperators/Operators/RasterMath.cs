using System;
using GCDConsoleLib;
using System.Collections.Generic;
using GCDConsoleLib.Internal;
using GCDConsoleLib.Utility;
namespace GCDConsoleLib.Internal.Operators
{
    public class RasterMath<T> : CellByCellOperator<T>
    {
        private bool _scalar;
        private RasterOperators.MathOpType _type;
        private T _operand;

        /// <summary>
        /// We protect the constructors because we don't really want anyone using them.
        /// </summary>
        /// <param name="otType"></param>
        /// <param name="rInput"></param>
        /// <param name="dOperand"></param>
        /// <param name="sOutputRaster"></param>
        internal RasterMath(RasterOperators.MathOpType otType, ref Raster rInput, T dOperand, ref Raster rOutputRaster) :
            base(new List<Raster> { rInput }, ref rOutputRaster)
        {
            _type = otType;
            _scalar = true;
            _operand = dOperand;
        }

        internal RasterMath(RasterOperators.MathOpType otType, ref Raster rInputA, ref Raster rInputB, ref Raster rOutputRaster) :
            base(new List<Raster> { rInputA, rInputB }, ref rOutputRaster)
        {
            _type = otType;
            _scalar = false;
        }

        protected override T CellOp(ref List<T[]> data, int id)
        {
            T val = OpNodataVal;
            if (_scalar)
            {
                if (!data[0][id].Equals(OpNodataVal))
                {
                    switch (_type)
                    {
                        case RasterOperators.MathOpType.Addition:
                            val = Dynamics.Add(data[0][id],_operand);
                            break;
                        case RasterOperators.MathOpType.Subtraction:
                            val = Dynamics.Subtract(data[0][id], _operand);
                            break;
                        case RasterOperators.MathOpType.Multipication:
                            val = Dynamics.Multiply(data[0][id], _operand);
                            break;
                        case RasterOperators.MathOpType.Division:
                            val = Dynamics.Divide(data[0][id], _operand);
                            break;
                    }
                }
            }
            else
            {
                if (!data[0][id].Equals(OpNodataVal) && !data[1][id].Equals(OpNodataVal))
                {
                    switch (_type)
                    {
                        case RasterOperators.MathOpType.Addition:
                            val = Dynamics.Add(data[0][id], data[1][id]);
                            break;
                        case RasterOperators.MathOpType.Subtraction:
                            val = Dynamics.Subtract(data[0][id], data[1][id]);
                            break;
                        case RasterOperators.MathOpType.Multipication:
                            val = Dynamics.Multiply(data[0][id], data[1][id]);
                            break;
                        case RasterOperators.MathOpType.Division:
                            val = Dynamics.Divide(data[0][id], data[1][id]);
                            break;
                    }
                }
            }
            return val;
        }
    }
}
