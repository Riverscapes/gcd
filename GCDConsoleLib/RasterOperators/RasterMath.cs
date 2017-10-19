using System;
using GCDConsoleLib;
using System.Collections.Generic;

namespace GCDConsoleLib.RasterOperators
{
    public class RasterMath : BaseOperator
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
            Raster rOutput = new Raster(sOutputRaster);
            RasterMath mathOp = new RasterMath(MathOpType.Addition, ref rInput, dOperand, ref rOutput);
            return mathOp.RunCellByCellOp();
        }
        public static Raster Add(ref Raster rInputA, ref Raster rInputB, string sOutputRaster)
        {
            Raster rOutput = new Raster(sOutputRaster);
            RasterMath mathOp = new RasterMath(MathOpType.Addition, ref rInputA, ref rInputB, ref rOutput);
            return mathOp.RunCellByCellOp();
        }


        // These are mainly for testing
        public static Raster Add(ref Raster rInput, double dOperand, ref Raster rOutputRaster)
        {
            RasterMath mathOp = new RasterMath(MathOpType.Addition, ref rInput, dOperand, ref rOutputRaster);
            return mathOp.RunCellByCellOp();
        }
        public static Raster Add(ref Raster rInputA, ref Raster rInputB, ref Raster rOutputRaster)
        {
            RasterMath mathOp = new RasterMath(MathOpType.Addition, ref rInputA, ref rInputB, ref rOutputRaster);
            return mathOp.RunCellByCellOp();
        }

        /// <summary>
        /// We protect the constructors because we don't really want anyone using them.
        /// </summary>
        /// <param name="otType"></param>
        /// <param name="rInput"></param>
        /// <param name="dOperand"></param>
        /// <param name="sOutputRaster"></param>
        protected RasterMath(MathOpType otType, ref Raster rInput, double dOperand, ref Raster rOutputRaster) : base(ref rInput, ref rOutputRaster)
        {
            _type = otType;
            _scalar = true;
            _operand = dOperand;
        }

        protected RasterMath(MathOpType otType, ref Raster rInputA, ref Raster rInputB, ref Raster rOutputRaster) : base(ref rInputA, ref rInputB, ref rOutputRaster)
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
