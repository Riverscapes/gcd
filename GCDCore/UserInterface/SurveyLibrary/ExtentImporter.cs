using System;
using GCDConsoleLib;

namespace GCDCore.UserInterface.SurveyLibrary
{
    public class ExtentImporter
    {
        public enum Purposes
        {
            FirstDEM,
            SubsequentDEM,
            AssociatedSurface,
            ErrorSurface,
            ReferenceSurface,
            ReferenceErrorSurface
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
                if (Purpose == Purposes.SubsequentDEM || Purpose == Purposes.AssociatedSurface || Purpose == Purposes.ErrorSurface ||
                    Purpose == Purposes.ReferenceSurface || Purpose == Purposes.ReferenceErrorSurface)
                {
                    cellSize = RefExtent.CellWidth;
                    _Precision = (ushort)GCDConsoleLib.Utility.DynamicMath.NumDecimals(RefExtent.CellWidth);
                }
                else
                {
                    // Get the raw precision of the new extent. Override this raw precision if this
                    // is the first DEM in a project and the raw precision is unacceptably high num of decimal places
                    ushort rawPrecision = (ushort)GCDConsoleLib.Utility.DynamicMath.NumDecimals(value.CellWidth);
                    if (Purpose == Purposes.FirstDEM && rawPrecision > 2)
                        _Precision = 2;
                    else
                        _Precision = rawPrecision;

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
        public bool IsOutputExtentEditable { get { return !(Purpose == Purposes.AssociatedSurface || Purpose == Purposes.ErrorSurface || Purpose == Purposes.ReferenceErrorSurface); } }

        public decimal OutputLeft { get { return Output.Left; } set { Output.Left = value; } }

        public decimal OutputRight
        {
            get { return Output.Right; }
            set
            {
                if (Purpose == Purposes.AssociatedSurface) throw new Exception("Cannot adjust right in associated surface mode.");
                if (Purpose == Purposes.ErrorSurface) throw new Exception("Cannot adjust right in error surface mode.");
                if (Purpose == Purposes.ReferenceErrorSurface) throw new Exception("Cannot adjust right in reference error surface mode.");
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
                if (Purpose == Purposes.ErrorSurface) throw new Exception("Cannot adjust bottom in error surface mode.");
                if (Purpose == Purposes.ReferenceErrorSurface) throw new Exception("Cannot adjust bottom in reference error surface mode.");

                Output.Rows = (int)((value - Output.Top) / Output.CellHeight);
            }
        }

        public decimal CellSize
        {
            get { return Output.CellWidth; }
            set
            {
                if (Purpose == Purposes.AssociatedSurface) throw new Exception("Cannot adjust cell size in associated surface mode.");
                if (Purpose == Purposes.ErrorSurface) throw new Exception("Cannot adjust cell size in error surface mode.");
                if (Purpose == Purposes.SubsequentDEM) throw new Exception("Cannot adjust cell size in subsequent DEM mode.");
                if (Purpose == Purposes.ReferenceSurface) throw new Exception("Cannot adjust cell size in reference surface mode.");
                if (Purpose == Purposes.ReferenceErrorSurface) throw new Exception("Cannot adjust cell size in reference error surface mode.");
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
                if (Purpose == Purposes.ErrorSurface) throw new Exception("Cannot adjust precision in error surface mode.");
                if (Purpose == Purposes.SubsequentDEM) throw new Exception("Cannot adjust precision in subsequent DEM mode.");
                if (Purpose == Purposes.ReferenceSurface) throw new Exception("Cannot adjust precision in reference surface DEM mode.");
                if (Purpose == Purposes.ReferenceErrorSurface) throw new Exception("Cannot adjust precision in reference error surface mode.");

                _Precision = value;
                decimal cellSize = Math.Round(Output.CellWidth, _Precision);
                Initialize(cellSize);
            }
        }

        public ExtentImporter(Purposes ePurpose, ExtentRectangle refExtent)
        {
            Purpose = ePurpose;
            RefExtent = refExtent;
            _Precision = (ushort)GCDConsoleLib.Utility.DynamicMath.NumDecimals(RefExtent.CellWidth);

            if (ePurpose == Purposes.AssociatedSurface || ePurpose == Purposes.ErrorSurface || ePurpose == Purposes.ReferenceErrorSurface)
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
            if (Purpose == Purposes.FirstDEM && RefExtent != null) throw new Exception("First DEM mode should not have a reference raster.");
            if (Purpose == Purposes.SubsequentDEM && RefExtent == null) throw new Exception("Subsequent DEM mode requires a reference raster.");
            if (Purpose == Purposes.AssociatedSurface && RefExtent == null) throw new Exception("Associated surface mode requires a reference raster.");
            if (Purpose == Purposes.ErrorSurface && RefExtent == null) throw new Exception("Error surface mode requires a reference raster.");
            if (Purpose == Purposes.ReferenceSurface && RefExtent == null) throw new Exception("Reference surface mode requires a reference raster.");
            if (Purpose == Purposes.ReferenceErrorSurface && RefExtent == null) throw new Exception("Reference error surface mode requires a reference raster.");
            if (RefExtent is ExtentRectangle && !RefExtent.IsDivisible()) throw new Exception("Reference raster must always be divisble extent.");

            if (cellWidth < 0 && Precision < 1) throw new Exception("The precision must be greater than zero when the cell resolution is less than 1");

            if (_InputExtent != null && Purpose != Purposes.AssociatedSurface && Purpose != Purposes.ErrorSurface && Purpose != Purposes.ReferenceErrorSurface)
            {
                ExtentRectangle temp = new ExtentRectangle(InputExtent);
                temp.CellWidth = cellWidth;
                temp.CellHeight = cellWidth * InputHeightMultiplier;
                Output = new ExtentRectangle(temp.GetDivisibleExtent());
            }
        }
    }
}
