using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDConsoleLib;

namespace GCDCore.UserInterface.SurveyLibrary
{
    public class ExtentImporter
    {
        public enum Purposes
        {
            Standalone,
            FirstDEM,
            SubsequentDEM,
            AssociatedSurface
        };

        public readonly Purposes Purpose;
        public readonly ExtentRectangle RefExtent;
        public ExtentRectangle Output;

        private ExtentRectangle _InputExtent;
        public ExtentRectangle InputExtent
        {
            get { return _InputExtent; }
            set
            {
                _InputExtent = value;

                decimal cellSize = 0;
                if (Purpose == Purposes.SubsequentDEM || Purpose == Purposes.AssociatedSurface)
                    cellSize = RefExtent.CellWidth;
                else
                {
                    _Precision = CalculatePrecision(value.CellWidth);
                    cellSize = Math.Round(value.CellWidth, _Precision);
                }

                Initialize(cellSize);
            }
        }

        public bool RequiresResampling
        {
            get
            {
                if (InputExtent == null)
                    return false;

                return !InputExtent.IsDivisible() || InputExtent.CellWidth != Output.CellWidth;
            }
        }

        private decimal InputHeightMultiplier { get { return InputExtent.CellHeight > 0 ? 1 : -1; } }
        public bool IsOutputExtentEditable { get { return Purpose != Purposes.AssociatedSurface; } }

        public decimal OutputLeft { get { return Output.Left; } set { Output.Left = value; } }

        public decimal OutputRight
        {
            get { return Output.Right; }
            set
            {
                if (Purpose == Purposes.AssociatedSurface) throw new Exception("Cannot adjust right in associated surface mode.");
                if (value <= OutputLeft) throw new Exception("Cannot adjust right to be less than left.");

                Output.Cols = (int)((value - Output.Left) / Output.CellWidth);
            }
        }

        public decimal OutputTop { get { return Output.Top; } set { Output.Top = value; } }

        public decimal OutputBottom
        {
            get { return Output.Bottom; }
            set
            {
                if (Purpose == Purposes.AssociatedSurface) throw new Exception("Cannot adjust bottom in associated surface mode.");

                Output.Rows = (int)((value - Output.Top) / Output.CellHeight);
            }
        }

        public decimal CellSize
        {
            get { return Output.CellWidth; }
            set
            {
                if (Purpose == Purposes.AssociatedSurface) throw new Exception("Cannot adjust cell size in associated surface mode.");
                if (Purpose == Purposes.SubsequentDEM) throw new Exception("Cannot adjust cell size in subsequent DEM mode.");
                if (value <= 0) throw new ArgumentOutOfRangeException("value", "Invalid cell size");

                Initialize(value);
            }
        }

        private ushort _Precision;
        public ushort Precision
        {
            get { return _Precision; }
            set
            {
                if (Purpose == Purposes.AssociatedSurface) throw new Exception("Cannot adjust precision in associated surface mode.");
                if (Purpose == Purposes.SubsequentDEM) throw new Exception("Cannot adjust precision in subsequent DEM mode.");

                _Precision = value;
                decimal cellSize = Math.Round(Output.CellWidth, _Precision);
                Initialize(cellSize);
            }
        }

        public ExtentImporter(Purposes ePurpose, ExtentRectangle refExtent)
        {
            Purpose = ePurpose;
            RefExtent = refExtent;
            _Precision = CalculatePrecision(RefExtent.CellWidth);

            if (ePurpose == Purposes.AssociatedSurface)
            {
                Output = new ExtentRectangle(refExtent);
            }
            else
            {
                Output = new ExtentRectangle(0, 0, refExtent.CellHeight, refExtent.CellWidth, 0, 0);
            }

            Initialize(RefExtent.CellWidth);
        }

        public ExtentImporter(Purposes ePurpose)
        {
            Purpose = ePurpose;
            Output = new ExtentRectangle(0, 0, 1, 1, 0, 0);
        }

        public void Initialize(decimal cellWidth)
        {
            if (Purpose == Purposes.Standalone && RefExtent != null) throw new Exception("Standalone mode should not have a reference raster.");
            if (Purpose == Purposes.FirstDEM && RefExtent != null) throw new Exception("First DEM mode should not have a reference raster.");
            if (Purpose == Purposes.SubsequentDEM && RefExtent == null) throw new Exception("Subsequent DEM mode requires a reference raster.");
            if (Purpose == Purposes.AssociatedSurface && RefExtent == null) throw new Exception("Associated surface mode requires a reference raster.");
            if (RefExtent is ExtentRectangle && !RefExtent.IsDivisible()) throw new Exception("Reference raster must always be divisble extent.");

            if (cellWidth < 0 && Precision < 1) throw new Exception("The precision must be greater than zero when the cell resolution is less than 1");

            if (_InputExtent != null && Purpose != Purposes.AssociatedSurface)
            {
                ExtentRectangle temp = new ExtentRectangle(InputExtent);
                temp.CellWidth = cellWidth;
                temp.CellHeight = cellWidth * InputHeightMultiplier;
                Output = new ExtentRectangle(temp.GetDivisibleExtent());
            }
        }

        /// <summary>
        /// Try to determine the appropriate precision from the input raster.
        /// Keep increasing the original cell resolution by powers of ten until it
        /// is a whole number. This is the appropriate "initial" precision for the
        /// output until the user overrides it.
        /// </summary>
        /// <param name="cellSize"></param>
        /// <returns></returns>
        public static ushort CalculatePrecision(decimal cellSize)
        {
            ushort precision = 1;
            for (int i = 0; i <= 10; i++)
            {
                decimal fTest = cellSize * (decimal)Math.Pow(10, i);
                fTest = Math.Round(fTest, 4);
                if (fTest % 1 == 0)
                {
                    precision = Convert.ToUInt16(i);
                    break;
                }
            }

            return precision;
        }
    }
}
