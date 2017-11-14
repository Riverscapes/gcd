using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDConsoleLib.FIS
{
    static class FISMath
    {
        /// <summary>
        /// The Maximum OR fuzzy operator
        /// </summary>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        /// <returns></returns>
        static double Max(double n1, double n2)
        {
            return Math.Max(n1, n2);
        }

        /// <summary>
        /// The Minimum AND fuzzy operator
        /// </summary>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        /// <returns></returns>
        static double Min(double n1, double n2)
        {
            return Math.Min(n1, n2);
        }

        /// <summary>
        /// The Probabalistic OR fuzzy operator
        /// </summary>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        /// <returns></returns>
        static double ProbOr(double n1, double n2)
        {
            return n1 + n2 - n1 * n2;
        }

        /// <summary>
        /// The Product AND fuzzy operator
        /// </summary>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        /// <returns></returns>
        static double Product(double n1, double n2)
        {
            return n1 * n2;
        }

        /// <summary>
        /// Figures out where two lines intersect and if the intersection is in the line segments.
        /// </summary>
        /// <param name="ax1"></param>
        /// <param name="ay1"></param>
        /// <param name="ax2"></param>
        /// <param name="ay2"></param>
        /// <param name="bx1"></param>
        /// <param name="by1"></param>
        /// <param name="bx2"></param>
        /// <param name="by2"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        static Tuple<double, double, bool> IntersectLines(double ax1, double ay1, double ax2, double ay2, double bx1, double by1,
                       double bx2, double by2)
        {
            double x = ax1;
            double y = ay1;
            bool bIntersect;
            double aSlope = (ay2 - ay1) / (ax2 - ax1);
            double bSlope = (by2 - by1) / (bx2 - bx1);
            double aIntercept = ay1 - aSlope * ax1;
            double bIntercept = by1 - bSlope * bx1;
            if (aSlope == bSlope)
                bIntersect = false;
            else
            {
                x = (bIntercept - aIntercept) / (aSlope - bSlope);
                y = aSlope * x + aIntercept;
                if ((ax1 < x && x < ax2) && (bx1 < x && x < bx2))
                    bIntersect = true;
                else
                    bIntersect = false;
            }
            return new Tuple<double, double, bool>(x, y, bIntersect);
        }
    }
}
