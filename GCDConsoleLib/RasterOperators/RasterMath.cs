using System;
using System.Collections.Generic;

namespace GCDConsoleLib.RasterOperators
{
    public class RasterMath : BaseOpertator
    {
        public enum MathOpType : byte { Addition, Subtraction, Division, Multipication };
        private bool _scalar;
        private MathOpType _type;
        private double _operand;

        /// <summary>
        /// HEre are our public functions
        /// </summary>
        /// <param name="rInput"></param>
        /// <param name="dOperand"></param>
        /// <param name="sOutputRaster"></param>
        /// <returns></returns>
        public static Raster Add(ref Raster rInput, double dOperand, string sOutputRaster)
        {
            RasterMath mathOp = new RasterMath(MathOpType.Addition, ref rInput, dOperand, sOutputRaster);
            return mathOp.RunCellByCellOp();
        }
        public static Raster Add(ref Raster rInputA, ref Raster rInputB, string sOutputRaster)
        {
            RasterMath mathOp = new RasterMath(MathOpType.Addition, ref rInputA, ref rInputB, sOutputRaster);
            return mathOp.RunCellByCellOp();
        }

        /// <summary>
        /// We protect the constructors because we don't really want anyone using them.
        /// </summary>
        /// <param name="otType"></param>
        /// <param name="rInput"></param>
        /// <param name="dOperand"></param>
        /// <param name="sOutputRaster"></param>
        protected RasterMath(MathOpType otType, ref Raster rInput, double dOperand, string sOutputRaster) : base(ref rInput, sOutputRaster)
        {
            _type = otType;
            _scalar = true;
            _operand = dOperand;
        }

        protected RasterMath(MathOpType otType, ref Raster rInputA, ref Raster rInputB, string sOutputRaster) : base(ref rInputA, ref rInputB, sOutputRaster)
        {
            _type = otType;
            _scalar = false;
        }

        protected override double CellOp(ref List<double[]> data, int id)
        {
            double val = 0;
            if (_scalar)
            {
                if (data[0][id] == _nodataval)
                {
                    val = (double)_nodataval;
                }
                else
                {
                    switch (_type)
                    {
                        case MathOpType.Addition:
                            val = data[0][id] + _operand;
                            break;
                        case MathOpType.Subtraction:
                            val = data[0][id] - _operand;
                            break;
                        case MathOpType.Multipication:
                            val = data[0][id] * _operand;
                            break;
                        case MathOpType.Division:
                            val = data[0][id] / _operand;
                            break;
                    }
                }
            }
            else
            {
                if (data[0][id] == _nodataval || data[0][id] == _nodataval)
                {
                    val = (double)_nodataval;
                }
                else
                {
                    switch (_type)
                    {
                        case MathOpType.Addition:
                            val = data[0][id] + data[1][id];
                            break;
                        case MathOpType.Subtraction:
                            val = data[0][id] - data[1][id];
                            break;
                        case MathOpType.Multipication:
                            val = data[0][id] * data[1][id];
                            break;
                        case MathOpType.Division:
                            val = data[0][id] / data[1][id];
                            break;
                    }
                }
            }
            return val;
        }

        protected override double[] ChunkOp(ref List<double[]> data)
        {
            //We don't use this for math. Everything is cell-wise
            throw new NotImplementedException();
        }
    }
}
