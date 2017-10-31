using System;
using System.Diagnostics;
using OSGeo.GDAL;

namespace GCDConsoleLib
{
    public class ExtentRectangle
    {
        public int rows, cols;
        // unused but necessary for transform;
        private double[] _t;

        public ExtentRectangle()
        {
            _Init(0, 0, 0.1, 0.1, 0, 0);
        }

        /// <summary>
        /// Explicit Constructor
        /// </summary>
        /// <param name="fTop"></param>
        /// <param name="fLeft"></param>
        /// <param name="nRows"></param>
        /// <param name="nCols"></param>
        /// <param name="dCellHeight"></param>
        /// <param name="dCellWidth"></param>
        public ExtentRectangle(decimal fTop, decimal fLeft, decimal dCellHeight, decimal dCellWidth, int nRows, int nCols)
        {
            _Init(fTop, fLeft, dCellHeight, dCellWidth, nRows, nCols);
        }


        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="orig"></param>
        public ExtentRectangle(ref ExtentRectangle orig)
        {
            _Init(orig.Top, orig.Left, orig.CellHeight, orig.CellWidth, orig.rows, orig.cols);
        }

        /// <summary>
        /// Load this rectangle from an already-open dataset
        /// </summary>
        /// <param name="dSet"></param>
        public ExtentRectangle(Dataset dSet)
        {
            Band rBand = dSet.GetRasterBand(1);
            double[] tr = new double[6];
            dSet.GetGeoTransform(tr);

            _Init(tr[3], tr[0], tr[5], tr[1], rBand.YSize, rBand.XSize);
        }

        /// <summary>
        /// Load this rectangle from a file on disk
        /// </summary>
        /// <param name="sFilePath"></param>
        public ExtentRectangle(string sFilePath)
        {
            Raster temp = new Raster(sFilePath);
            _Init(temp.Extent.Top, temp.Extent.Left, temp.Extent.CellHeight, temp.Extent.CellWidth, temp.Extent.rows, temp.Extent.cols);
        }

        /// <summary>
        /// Init (Convenience Wrapper) in case we want to use decimals
        /// </summary>
        /// <param name="fTop"></param>
        /// <param name="fLeft"></param>
        /// <param name="dCellHeight"></param>
        /// <param name="dCellWidth"></param>
        /// <param name="nRows"></param>
        /// <param name="nCols"></param>
        private void _Init(decimal fTop, decimal fLeft, decimal dCellHeight, decimal dCellWidth, int nRows, int nCols)
        {
            _Init((double)fTop, (double)fLeft, (double)dCellHeight, (double)dCellWidth, nRows, nCols);
        }

        /// <summary>
        /// Init function to help us deal with loading all the values in a consistent way.
        /// </summary>
        /// <param name="fTop"></param>
        /// <param name="fLeft"></param>
        /// <param name="dCellHeight"></param>
        /// <param name="dCellWidth"></param>
        /// <param name="nRows"></param>
        /// <param name="nCols"></param>
        private void _Init(double fTop, double fLeft, double dCellHeight, double dCellWidth, int nRows, int nCols)
        {
            if (dCellHeight == 0) { throw new ArgumentOutOfRangeException("CellHeight cannot be 0"); }
            else if (dCellWidth == 0) { throw new ArgumentOutOfRangeException("dCellWidth cannot be 0"); }
            //else if (nRows == 0) { throw new ArgumentOutOfRangeException("Raster Rows cannot be 0"); }
            //else if (nCols == 0) { throw new ArgumentOutOfRangeException("Raster Cols cannot be 0"); }

            _t = new double[6] { fLeft, dCellWidth, 0, fTop, 0, dCellHeight };
            cols = nCols;
            rows = nRows;

            if ((Top * CellHeight) > (Bottom * CellHeight))
            {
                throw new ArgumentOutOfRangeException("Top", Top, "The top coordinate cannot be less than or equal to the bottom");
            }
            if ((Left * CellWidth) > (Right * CellWidth))
            {
                throw new ArgumentOutOfRangeException("Left", Left, "The left coordinate cannot be less than or equal to the right");
            }
        }


        /// <summary>
        /// Buffer this rectangle outwards by a decimal amount.
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

                newRows = rows + (int)(2 * vSign * fDistance / CellHeight);
                newCols = cols + (int)(2 * hSign * fDistance / CellWidth);
            }
            return new ExtentRectangle(newTop, newLeft, CellHeight, CellWidth, newRows, newCols);
        }

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
            return String.Format("{0} {1} {2} {3}", Left, Bottom, Right, Top);
        }

        /// <summary>
        /// Get a tuple representing row translation between two Extentrectangles
        /// </summary>
        /// <param name="rExtent1"></param>
        /// <param name="rExtent2"></param>
        /// <returns>(colT, rowT)</returns>
        public Tuple<int, int> GetTopCornerTranslation(ref ExtentRectangle rExtent2)
        {
            int colT = (int)((Left - rExtent2.Left) / CellWidth);
            int rowT = (int)((Top - rExtent2.Top) / CellHeight);
            return new Tuple<int, int>(colT, rowT);
        }

        /// <summary>
        /// Union this rectangle with another
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public ExtentRectangle Union(ref ExtentRectangle rect)
        {
            decimal newright, newleft, newtop, newbottom;
            if (CellHeight > 0)
            {
                newtop = Math.Max(Top, rect.Top);
                newbottom = Math.Max(Bottom, rect.Bottom);
            }
            else
            {
                newtop = Math.Min(Top, rect.Top);
                newbottom = Math.Min(Bottom, rect.Bottom);
            }
            if (CellWidth > 0)
            {
                newleft = Math.Min(Left, rect.Left);
                newright = Math.Min(Right, rect.Right);
            }
            else
            {
                newleft = Math.Max(Left, rect.Left);
                newright = Math.Max(Right, rect.Right);
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
        public ExtentRectangle Intersect(ref ExtentRectangle rect)
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
        /// <returns></returns>
        public Tuple<int, int> GetTranslation(ref ExtentRectangle rect)
        {
            int ydiff = (int)(rect.Top - Top / Math.Abs(CellHeight));
            int xdiff = (int)(rect.Left - Left / Math.Abs(CellWidth));
            return new Tuple<int, int>(xdiff, ydiff);
        }

        /// <summary>
        /// Is this raster concurrent with another?
        /// </summary>
        /// <param name="otherExtent"></param>
        /// <returns></returns>
        public bool IsConcurrent(ref ExtentRectangle otherExtent)
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
            return (row - 1) * rows + (col - 1);
        }

        public int MaxArrID
        {
            get
            {
                return rows * cols - 1;
            }
        }

        /// <summary>
        /// Convert an array index to a row,col tuple
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tuple<int, int> Id2RowCol(int id)
        {
            int row = (int)Math.Floor((decimal)(id / rows));
            int col = id - (row * rows);
            return new Tuple<int, int>(col + 1, row + 1);
        }

        /// <summary>
        /// Translate the 1D array index into another rectangle's 1D array index
        /// </summary>
        /// <param name="origid"></param>
        /// <param name="otherExtent"></param>
        /// <returns>the transformed ID, -1 if it's not valid</returns>
        public int RelativeId(int origid, ref ExtentRectangle otherExtent)
        {
            Tuple<int, int> origColRow = Id2RowCol(origid);
            Tuple<int, int> offsetColRow = GetTopCornerTranslation(ref otherExtent);
            int newInd;
            int newRow = origColRow.Item2 + offsetColRow.Item2;
            int newCol = origColRow.Item1 + offsetColRow.Item1;
            if (newCol <= 0 || newRow <= 0 || newCol > otherExtent.cols || newRow > otherExtent.rows)
                newInd = -1;
            else
                newInd = (newRow - 1) * otherExtent.cols + (newCol) - 1;

            return newInd;
        }

        /// <summary>
        /// Othoganility check for this rectangle
        /// </summary>
        /// <param name="otherExtent"></param>
        /// <returns></returns>
        public bool IsOrthogonal(ref ExtentRectangle otherExtent)
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
        private static decimal DivideModuloOne(decimal numA, decimal numB)
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

        public ExtentRectangle GetDivisibleExtent()
        {
            decimal newTop, newBottom, newLeft, newRight;

            newTop = Math.Floor(Top / CellHeight) * CellHeight;
            newBottom = Math.Ceiling(Bottom / CellHeight) * CellHeight;

            newLeft = Math.Floor(Left / CellWidth) * CellWidth;
            newRight = Math.Ceiling(Right / CellWidth) * CellWidth;

            int newRows = Convert.ToInt32((newBottom - newTop) / CellHeight);
            int newCols = Convert.ToInt32((newRight - newLeft) / CellWidth);

            return new ExtentRectangle(newTop, newLeft, CellHeight, CellWidth, newRows, newCols);
        }


        public bool HasOverlap(ref ExtentRectangle otherExtent)
        {
            // TODO: TEST THIS THOROUGHLY, ESPECIALLY -/+ widht heights
            decimal ulx = Math.Max(this.Left, otherExtent.Left);
            decimal uly = Math.Max(this.Bottom, otherExtent.Bottom);
            decimal lrx = Math.Min(this.Right, otherExtent.Right);
            decimal lry = Math.Min(this.Top, otherExtent.Top);

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
            get { return Left + (CellWidth * cols); }
        }
        public decimal Bottom
        {
            get { return Top + (CellHeight * rows); }
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

        public decimal Height { get { return (rows * Math.Abs(CellHeight)); } }
        public decimal Width { get { return (cols * Math.Abs(CellWidth)); } }

    }
}


