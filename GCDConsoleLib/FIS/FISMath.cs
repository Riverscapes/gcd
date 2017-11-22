using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDConsoleLib.FIS
{
    public static class FISOperators
    {

        /// <summary>
        /// The Maximum OR fuzzy operator
        /// </summary>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        /// <returns></returns>
        public static double Max(double n1, double n2)
        {
            return Math.Max(n1, n2);
        }

        /// <summary>
        /// The Minimum AND fuzzy operator
        /// </summary>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        /// <returns></returns>
        public static double Min(double n1, double n2)
        {
            return Math.Min(n1, n2);
        }

        /// <summary>
        /// The Probabalistic OR fuzzy operator
        /// </summary>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        /// <returns></returns>
        public static double ProbOr(double n1, double n2)
        {
            return n1 + n2 - (n1 * n2);
        }

        /// <summary>
        /// The Product AND fuzzy operator
        /// </summary>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        /// <returns></returns>
        public static double Product(double n1, double n2)
        {
            return n1 * n2;
        }

        /// <summary>
        /// The minimum implicator
        /// </summary>
        /// <param name="inMf"></param>
        /// <param name="outMf"></param>
        /// <param name="n"></param>
        /// <param name="weight"></param>
        public static void ImpMin(MemberFunction inMf, MemberFunction outMf, 
            double n, double weight)
        {
            outMf.clear();
            outMf.Coords.Add(new KeyValuePair<double, double>(inMf.Coords[0].Key, inMf.Coords[0].Value * weight));

            for (int i = 1; i < inMf.Coords.Count -1; i++)
            {
                if (inMf.Coords[i].Value > n)
                {
                    outMf.Coords.Add(new KeyValuePair<double, double>(inMf.getX(i - 1, i, n), n * weight));
                    break;
                }
                else
                    outMf.Coords.Add(new KeyValuePair<double, double>(inMf.Coords[i].Key, inMf.Coords[i].Value * weight));
            }
            for (int i = 0; i < inMf.Coords.Count; i++)
            {
                if (inMf.Coords[i].Value < n)
                {
                    outMf.Coords.Add(new KeyValuePair<double, double>(inMf.getX(i - 1, i, n), n * weight));
                    outMf.Coords.Add(new KeyValuePair<double, double>(inMf.Coords[i].Key, inMf.Coords[i].Value * weight));
                }
            }
        }

        /// <summary>
        /// The product Implicator
        /// </summary>
        /// <param name="inMf"></param>
        /// <param name="outMf"></param>
        /// <param name="n"></param>
        /// <param name="weight"></param>
        public static void ImpProduct(MemberFunction inMf, MemberFunction outMf,
            double n, double weight)
        {
            outMf.clear();
            for (int i = 0; i < inMf.Coords.Count; i++)
                outMf.Coords.Add(new KeyValuePair<double, double>(inMf.Coords[i].Key, inMf.Coords[i].Value * n* weight));
        }

        /// <summary>
        /// Figures out where two lines intersect and if the intersection is in the line segments.
        /// NOTE: This doesn't seem to be in use anywhere
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
        public static Tuple<double, double, bool> IntersectLines(
            double ax1, double ay1, double ax2, double ay2, 
            double bx1, double by1, double bx2, double by2)
        {
            double x = ax1;
            double y = ay1;
            double s, k;
            bool bIntersect;
            double aSlope = (ay2 - ay1) / (ax2 - ax1);
            double bSlope = (by2 - by1) / (bx2 - bx1);

            double aIntercept = ay1 - aSlope * ax1;
            double bIntercept = by1 - bSlope * bx1;

            if (aSlope == bSlope)
                bIntersect = false;
            else
            {
                // Deal with Inifinities
                if  (double.IsNaN(aIntercept))
                {
                    s = bSlope;
                    k = bIntercept;
                    x = ax1;
                }
                else if (double.IsNaN(bIntercept))
                {
                    s = aSlope;
                    k = aIntercept;
                    x = bx1;
                }
                else
                {
                    s = aSlope;
                    k = aIntercept;
                    x = (bIntercept - aIntercept) / (aSlope-bSlope);
                }
                 
                y = s * x + k;
                if ((ax1 <= x && x <= ax2) && (bx1 <= x && x <= bx2))
                    bIntersect = true;
                else
                    bIntersect = false;
            }
            return new Tuple<double, double, bool>(x, y, bIntersect);
        }
    }
}
