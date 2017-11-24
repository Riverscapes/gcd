using System;
using System.Collections.Generic;

namespace GCDConsoleLib.FIS
{
    public class MemberFunction
    {
        public List<double[]> Coords;
        public double MaxY;

        /// <summary>
        /// Blank Constructor
        /// </summary>
        public MemberFunction()
        {
            Coords = new List<double[]>();
            MaxY = 0;
        }

        /// <summary>
        /// Constructor, using a triangle membership function.
        /// The x coordinates must be in order from smallest to largest.The y values corresponding to x1 and
        /// x3 are assumed to be 0.
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="x3"></param>
        /// <param name="yMax"></param>
        public MemberFunction(double x1, double x2, double x3, double dyMax)
        {
            Coords = new List<double[]>();
            MaxY = dyMax;
            if ((MaxY <= 0) || (MaxY > 1))
                throw new ArgumentException(string.Format("Invalid yMax of {0}. It must be between 0 and 1.", MaxY));
            else if ((x1 > x2) || (x2 > x3))
                throw new ArgumentException(string.Format("Membership function vertices ({0} {1} {2}) must be in ascending order.", x1, x2, x3));
            else
            {
                Coords.Add(new double[2] { x1, 0 });
                Coords.Add(new double[2] { x2, MaxY });
                Coords.Add(new double[2] { x3, 0 });
            }
        }

        /// <summary>
        /// Constructor, using a trapezoidal membership function.
        /// The x coordinates must be in order from smallest to largest.The y values corresponding to x1 and
        /// x4 are assumed to be 0.
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="x3"></param>
        /// <param name="x4"></param>
        /// <param name="dyMax"></param>
        public MemberFunction(double x1, double x2, double x3, double x4, double dyMax)
        {
            Coords = new List<double[]>();
            MaxY = dyMax;
            if ((MaxY <= 0) || (MaxY > 1))
                throw new ArgumentException(string.Format("Invalid yMax of {0}. It must be between 0 and 1.", MaxY));
            else if ((x1 > x2) || (x2 >= x3) || (x3 > x4))
                throw new ArgumentException(string.Format("Membership function vertices ({0} {1} {2} {3}) must be in ascending order.", x1, x2, x3, x4));
            else
            {
                Coords.Add(new double[] { x1, 0 });
                Coords.Add(new double[2] { x2, MaxY });
                Coords.Add(new double[2] { x3, MaxY });
                Coords.Add(new double[2] { x4, 0 });
            }
        }

        /// <summary>
        /// Constructor, using two vectors of coordinates.
        /// The x coordinates must be in order from smallest to largest.The x and y coordinates must be in the
        /// same order.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public MemberFunction(List<double[]> coords)
        {
            if (0 == coords.Count)
                throw new ArgumentException("No coordinates provided.");

            Coords = coords;

            if (coords.Count == 0)
                MaxY = 0;
            else
            {
                double max = 0;
                foreach (double[] coord in coords)
                    if (coord[0] > max)
                        max = coord[0];
                MaxY = max;
            }
        }

        /// <summary>
        /// Copy one member function into another
        /// </summary>
        /// <param name="mFunc"></param>
        public void Copy(MemberFunction mFunc)
        {
            Coords.Clear();
            foreach (double[] coord in mFunc.Coords)
                Coords.Add(coord);
            MaxY = mFunc.MaxY;
        }

        /// <summary>
        /// Clear all vertices out of this function
        /// </summary>
        public void clear()
        {
            Coords.Clear();
            MaxY = 0;
        }

        /// <summary>
        /// Gets the y value from a membership function, given an x value.
        /// </summary>
        /// <param name="x">The x coordinate to get the corresponding y value for</param>
        /// <returns>The y coordinate</returns>
        public double fuzzify(double x)
        {
            double result = 0;
            for (int i = 1; i < Coords.Count; i++)
            {
                if ((x >= Coords[i - 1][0]) && (x <= Coords[i][0]))
                {
                    // Asymptote special case
                    // NOTE: THIS HAS Been tested for the MAX workflow only.
                    // TODO: Need to test this with other FIS cases.
                    if (Coords[i][0] - Coords[i - 1][0] == 0)
                        result = Math.Max(Coords[i][1], Coords[i - 1][1]);
                    else
                        result = Coords[i - 1][1] + (x - Coords[i - 1][0])
                            * ((Coords[i][1] - Coords[i - 1][1])
                            / (Coords[i][0] - Coords[i - 1][0]));
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Computes the x coordinate that corresponds to a given y coordinate in between two vertices.
        /// http://www.datadigitization.com/dagra-in-action/linear-interpolation-with-excel/
        /// </summary>
        /// <param name="v1">The index of the first vertex</param>
        /// <param name="v2">The index of the second vertex</param>
        /// <param name="y">The y coordinate</param>
        /// <returns>The x coordinate. Returns Coordss[v1][0] for vertical or horizontal</returns>
        public double getX(int v1, int v2, double y)
        {
            double slope = (Coords[v2][1] - Coords[v1][1]) / (Coords[v2][0] - Coords[v1][0]);
            if (double.IsInfinity(slope) || slope == 0)
                return Coords[v1][0];
            else
                return  ((y - Coords[v1][1]) / slope) + Coords[v1][0];  /// y = y1 + (x - x1)*slope
        }

        /// <summary>
        /// Check if a membership function is valid.
        /// </summary>
        public bool Valid { get { return Coords.Count > 0; } }

        /// <summary>
        /// Simple Property
        /// </summary>
        public int Length { get { return Coords.Count; } }

    }
}
