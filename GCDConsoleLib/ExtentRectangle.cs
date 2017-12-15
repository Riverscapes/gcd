using System;
using System.Diagnostics;
using OSGeo.GDAL;
using System.IO;
using UnitsNet;
using GCDConsoleLib.GCD;

namespace GCDConsoleLib
{
    public class ExtentRectangle
    {
        public int Rows, Cols;
        // unused but necessary for transform;
        private double[] _t;

        /// <summary>
        /// Explicit Constructor
        /// </summary>
        /// <param name="mTop"></param>
        /// <param name="mLeft"></param>
        /// <param name="nRows"></param>
        /// <param name="nCols"></param>
        /// <param name="mCellHeight"></param>
        /// <param name="dCellWidth"></param>
        public ExtentRectangle(decimal mTop, decimal mLeft, decimal mCellHeight, decimal mCellWidth, int nRows, int nCols)
        {
            _Init(mTop, mLeft, mCellHeight, mCellWidth, nRows, nCols);
        }


        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="orig"></param>
        public ExtentRectangle(ExtentRectangle orig)
        {
            _Init(orig.Top, orig.Left, orig.CellHeight, orig.CellWidth, orig.Rows, orig.Cols);
        }

        /// <summary>
        /// Load this rectangle from an already-open dataset
        /// </summary>
        /// <param name="dSet"></param>
        internal ExtentRectangle(Dataset dSet)
        {
            Band rBand = dSet.GetRasterBand(1);
            double[] tr = new double[6];
            dSet.GetGeoTransform(tr);

            _Init((decimal)tr[3], (decimal)tr[0], (decimal)tr[5], (decimal)tr[1], rBand.YSize, rBand.XSize);
        }

        /// <summary>
        /// Load this rectangle from a file on disk
        /// </summary>
        /// <param name="sFilePath"></param>
        public ExtentRectangle(FileInfo sFilePath)
        {
            Raster temp = new Raster(sFilePath);
            _Init(temp.Extent.Top, temp.Extent.Left, temp.Extent.CellHeight, temp.Extent.CellWidth, temp.Extent.Rows, temp.Extent.Cols);
            temp.Dispose();
        }

        /// <summary>
        /// Init function to help us deal with loading all the values in a consistent way.
        /// </summary>
        /// <param name="mTop"></param>
        /// <param name="mLeft"></param>
        /// <param name="mCellHeight"></param>
        /// <param name="mCellWidth"></param>
        /// <param name="nRows"></param>
        /// <param name="nCols"></param>
        private void _Init(decimal mTop, decimal mLeft, decimal mCellHeight, decimal mCellWidth, int nRows, int nCols)
        {
            if (mCellHeight == 0) { throw new ArgumentOutOfRangeException("CellHeight cannot be 0"); }
            else if (mCellWidth == 0) { throw new ArgumentOutOfRangeException("dCellWidth cannot be 0"); }
            //else if (nRows == 0) { throw new ArgumentOutOfRangeException("Raster Rows cannot be 0"); }
            //else if (nCols == 0) { throw new ArgumentOutOfRangeException("Raster Cols cannot be 0"); }

            _t = new double[6] { (double)mLeft, (double)mCellWidth, 0, (double)mTop, 0, (double)mCellHeight };
            Cols = nCols;
            Rows = nRows;

            if ((Top * this.CellHeight) > (Bottom * this.CellHeight))
            {
                throw new ArgumentOutOfRangeException("Top", Top, "The top coordinate cannot be less than or equal to the bottom");
            }
            if ((Left * CellWidth) > (Right * CellWidth))
            {
                throw new ArgumentOutOfRangeException("Left", Left, "The left coordinate cannot be less than or equal to the right");
            }
        }


        /// <summary>
        /// Buffer this rectangle outwards by a double amount.
        /// </summary>
        /// <param name="fDistance"></param>
        /// <returns></returns>
        public ExtentRectangle Buffer(decimal fDistance)
        {
            decimal vSign = Math.Abs(CellHeight) / CellHeight;
            decimal hSign = Math.Abs(CellWidth) / CellWidth;
            decimal newTop, newLeft;
            int newRows, newCols;

            if (fDistance <= 0)
            {
                throw new ArgumentOutOfRangeException("Distance", fDistance, "The buffer distance must be greater than zero");
            }
            else
            {
                newTop = Top - (vSign * fDistance);
                newLeft = Left - (hSign * fDistance);

                newRows = Rows + (int)(2 * vSign * fDistance / CellHeight);
                newCols = Cols + (int)(2 * hSign * fDistance / CellWidth);
            }
            return new ExtentRectangle(newTop, newLeft, CellHeight, CellWidth, newRows, newCols);
        }

        /// <summary>
        /// Buffer the raster by a certain number of cells
        /// </summary>
        /// <param name="fCells"></param>
        /// <returns></returns>
        public ExtentRectangle Buffer(int fCells)
        {
            return Buffer(Math.Abs(fCells * CellHeight));
        }

        /// <summary>
        /// Simple ToString method to output "Left Bottom Right Top"
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", Left, Bottom, Right, Top);
        }

        /// <summary>
        /// Get a tuple representing row translation between two Extentrectangles
        /// </summary>
        /// <param name="rExtent1"></param>
        /// <param name="rExtent2"></param>
        /// <returns>(colT, rowT)</returns>
        public int[] GetTopCornerTranslationRowCol(ExtentRectangle rExtent2)
        {
            int colT = (int)((Left - rExtent2.Left) / CellWidth);
            int rowT = (int)((Top - rExtent2.Top) / CellHeight);
            return new int[2] { rowT, colT };
        }

        /// <summary>
        /// Union this rectangle with another
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public ExtentRectangle Union(ExtentRectangle rect)
        {
            decimal newright, newleft, newtop, newbottom;
            if (CellHeight > 0)
            {
                newtop = Math.Min(Top, rect.Top);
                newbottom = Math.Max(Bottom, rect.Bottom);
            }
            else
            {
                newtop = Math.Max(Top, rect.Top);
                newbottom = Math.Min(Bottom, rect.Bottom);
            }
            if (CellWidth > 0)
            {
                newleft = Math.Min(Left, rect.Left);
                newright = Math.Max(Right, rect.Right);
            }
            else
            {
                newleft = Math.Max(Left, rect.Left);
                newright = Math.Min(Right, rect.Right);
            }

            int newcols = (int)((newright - newleft) / Math.Abs(CellWidth));
            int newrows = (int)((newtop - newbottom) / Math.Abs(CellHeight));

            return new ExtentRectangle(newtop, newleft, CellHeight, CellWidth, newrows, newcols);
        }

        /// <summary>
        /// Intersect another rectangle with this one and return the resulting rectangle
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public ExtentRectangle Intersect(ExtentRectangle rect)
        {
            decimal newright, newleft, newtop, newbottom;
            if (CellHeight > 0)
            {
                newtop = Math.Max(Top, rect.Top);
                newbottom = Math.Min(Bottom, rect.Bottom);
            }
            else
            {
                newtop = Math.Min(Top, rect.Top);
                newbottom = Math.Max(Bottom, rect.Bottom);
            }
            if (CellWidth > 0)
            {
                newleft = Math.Max(Left, rect.Left);
                newright = Math.Min(Right, rect.Right);
            }
            else
            {
                newleft = Math.Min(Left, rect.Left);
                newright = Math.Max(Right, rect.Right);
            }

            int newcols = (int)((newright - newleft) / Math.Abs(CellWidth));
            int newrows = (int)((newtop - newbottom) / Math.Abs(CellHeight));

            if (newrows <= 0 || newcols <= 0)
            {
                newrows = 0;
                newcols = 0;
            }

            return new ExtentRectangle(newtop, newleft, CellHeight, CellWidth, newrows, newcols);
        }

        /// <summary>
        /// Return (x,y) integer typle representing how shifted one rectangle is relative to this one
        /// </summary>
        /// <param name="rect"></param>
        /// <returns>Tuple (x,y)</returns>
        public int[] GetTranslation(ExtentRectangle rect)
        {
            int ydiff = (int)(rect.Top - Top / Math.Abs(CellHeight));
            int xdiff = (int)(rect.Left - Left / Math.Abs(CellWidth));
            return new int[2] { xdiff, ydiff };
        }

        /// <summary>
        /// Is this raster concurrent with another?
        /// </summary>
        /// <param name="otherExtent"></param>
        /// <returns></returns>
        public bool IsConcurrent(ExtentRectangle otherExtent)
        {
            return CellWidth == otherExtent.CellWidth &&
                CellHeight == otherExtent.CellHeight &&
                Left == otherExtent.Left &&
                Right == otherExtent.Right &&
                Top == otherExtent.Top &&
                Bottom == otherExtent.Bottom;
        }

        /// <summary>
        /// Convert a row and col value to an array index
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public int RowCol2Id(int row, int col)
        {
            return (row - 1) * Rows + (col - 1);
        }

        public int MaxArrID
        {
            get
            {
                return Rows * Cols - 1;
            }
        }

        /// <summary>
        /// Convert an array index to a row,col tuple
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int[] Id2RowCol(int id)
        {
            int rowId = (int)Math.Floor((double)(id / Cols));
            int colId = id - (rowId * Cols);
            return new int[2] { rowId + 1, colId + 1 };
        }

        public int[] Pt2RowCol(double[] pt)
        {
            int row = (int)Math.Floor((pt[1] - (double)Top) / (double)CellHeight);
            int col = (int)Math.Floor((pt[0] - (double)Left) / (double)CellWidth);
            return new int[2] { row, col };
        }

        /// <summary>
        /// NOTE: THIS RETURNS CELL CENTERS
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public decimal[] Id2XY(int id)
        {
            int[] rowcol = Id2RowCol(id);
            decimal rowY = Top + (CellHeight * rowcol[0] - CellHeight / 2);
            decimal rowX = Left + (CellWidth * rowcol[1] - CellWidth / 2);
            return new decimal[2] { rowX, rowY };
        }

        /// <summary>
        /// Translate the 1D array index into another rectangle's 1D array index
        /// </summary>
        /// <param name="origid"></param>
        /// <param name="otherExtent"></param>
        /// <returns>the transformed ID, -1 if it's not valid</returns>
        public int RelativeId(int origid, ExtentRectangle otherExtent)
        {
            int[] origRowCol = Id2RowCol(origid);
            int[] offsetRowCol = GetTopCornerTranslationRowCol(otherExtent);
            int newInd;
            int newRow = origRowCol[0] + offsetRowCol[0];
            int newCol = origRowCol[1] + offsetRowCol[1];
            if (newCol <= 0 || newRow <= 0 || newCol > otherExtent.Cols || newRow > otherExtent.Rows)
                newInd = -1;
            else
                newInd = (newRow - 1) * otherExtent.Cols + (newCol) - 1;

            return newInd;
        }

        /// <summary>
        /// Othoganility check for this rectangle
        /// </summary>
        /// <param name="otherExtent"></param>
        /// <returns></returns>
        public bool IsOrthogonal(ExtentRectangle otherExtent)
        {
            return IsDivisible() && otherExtent.IsDivisible();
        }

        /// <summary>
        /// Module operation is notoriously unreliable for doubles. This 
        /// function is specific and equivalent to: 
        ///                                      return num % 1 == 0;
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static decimal DivideModuloOne(decimal numA, decimal numB)
        {
            return (numA / numB) - Math.Floor(numA / numB);
        }

        /// <summary>
        /// Is this raster divisible?
        /// </summary>
        /// <returns></returns>
        public bool IsDivisible()
        {
            return (DivideModuloOne(Top, CellHeight) == 0 &&
                    DivideModuloOne(Left, CellWidth) == 0 &&
                    DivideModuloOne(Bottom, CellHeight) == 0 &&
                    DivideModuloOne(Right, CellWidth) == 0);
        }

        /// <summary>
        /// Return a new extent that is divisible
        /// </summary>
        /// <returns></returns>
        public ExtentRectangle GetDivisibleExtent()
        {
            decimal newTop, newBottom, newLeft, newRight;

            newTop = Math.Floor(Top / CellHeight) *CellHeight;
            newBottom = Math.Ceiling(Bottom / CellHeight) * CellHeight;

            newLeft = Math.Floor(Left / CellWidth) * CellWidth;
            newRight = Math.Ceiling(Right / CellWidth) * CellWidth;

            int newRows = Convert.ToInt32((newBottom - newTop) / CellHeight);
            int newCols = Convert.ToInt32((newRight - newLeft) / CellWidth);

            return new ExtentRectangle(newTop, newLeft, CellHeight, CellWidth, newRows, newCols);
        }


        /// <summary>
        /// Determine if two extents overlap
        /// </summary>
        /// <param name="otherExtent"></param>
        /// <returns></returns>
        public bool HasOverlap(ExtentRectangle otherExtent)
        {
            // TODO: TEST THIS THOROUGHLY, ESPECIALLY -/+ widht heights
            decimal ulx = Math.Max(Left, otherExtent.Left);
            decimal uly = Math.Max(Bottom, otherExtent.Bottom);
            decimal lrx = Math.Min(Right, otherExtent.Right);
            decimal lry = Math.Min(Top, otherExtent.Top);

            return ulx <= lrx && uly <= lry;
        }
        //Remember:
        // The affine transform consists of six coefficients returned by GDALDataset::GetGeoTransform()
        // which map pixel/line coordinates into georeferenced space using the following relationship:

        //    Xgeo = GT(0) + Xpixel*GT(1) + Yline*GT(2)
        //    Ygeo = GT(3) + Xpixel*GT(4) + Yline*GT(5)

        // In case of north up images, the GT(2) and GT(4) coefficients are zero, and the GT(1) is
        // pixel width, and GT(5) is pixel height. The (GT(0),GT(3)) position is the top left corner
        // of the top left pixel of the raster.

        //[0]/* top left x 
        //[1]/* w-e pixel resolution 
        //[2]/* rotation, 0 if image is "north up" 
        //[3]/* top left y 
        //[4]/* rotation, 0 if image is "north up" 
        //[5]/* n-s pixel resolution 
        public double[] Transform
        {
            get { return _t; }
            set { _t = value; }
        }
        public decimal Top
        {
            get { return (decimal)_t[3]; }
            set { _t[3] = (double)value; }
        }
        public decimal Left
        {
            get { return (decimal)_t[0]; }
            set { _t[0] = (double)value; }
        }
        public decimal Right
        {
            get { return Left + (CellWidth * Cols); }
        }
        public decimal Bottom
        {
            get { return Top + (CellHeight * Rows); }
        }
        public decimal CellWidth
        {
            get { return (decimal)_t[1]; }
            set { _t[1] = (double)value; }
        }
        public decimal CellHeight
        {
            get { return (decimal)_t[5]; }
            set { _t[5] = (double)value; }
        }

        public decimal Height { get { return (Rows * Math.Abs(CellHeight)); } }
        public decimal Width { get { return (Cols * Math.Abs(CellWidth)); } }

        /// <summary>
        /// Get the bin Area in Area units
        /// </summary>
        public Area CellArea(UnitGroup units)
        {
            // We have to be sure to read the Cell height and width as the horizontal units
            double Lengthm = Length.From(Math.Abs((double)CellWidth), units.HorizUnit).Meters;
            double Heightm = Length.From(Math.Abs((double)CellHeight), units.HorizUnit).Meters;

            // Return a unitless Area from SquareMeters
            return Area.From(Lengthm * Heightm, UnitsNet.Units.AreaUnit.SquareMeter);
        }

    }
}


