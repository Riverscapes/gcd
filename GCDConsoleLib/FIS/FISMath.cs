using System;
using System.Collections.Generic;
using GCDConsoleLib.Extensions;
using System.Linq;

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
        /// Apply the Minimum implication method to a membership function.
        /// </summary>
        /// <param name="inMf">membership function to implicate</param>
        /// <param name="outMf">membership function to hold the output</param>
        /// <param name="n">The antecedent to reshape the membership function with.</param>
        /// <param name="weight">The weight of the rule</param>
        public static MemberFunction ImpMin(MemberFunction inMf, double n, double weight)
        {
            MemberFunction outMf = new MemberFunction();
            outMf.Coords.Add(new KeyValuePair<double, double>(inMf.Coords[0].Key, inMf.Coords[0].Value * weight));

            int i;
            for (i = 1; i < inMf.Coords.Count - 1; i++)
            {
                if (inMf.Coords[i].Value > n)
                {
                    outMf.Coords.Add(new KeyValuePair<double, double>(inMf.getX(i - 1, i, n), n * weight));
                    break;
                }
                else
                    outMf.Coords.Add(new KeyValuePair<double, double>(inMf.Coords[i].Key, inMf.Coords[i].Value * weight));
            }
            // Now let's continue and fill in the rest (thus the i=i)
            for (i = i; i < inMf.Coords.Count; i++)
            {
                if (inMf.Coords[i].Value < n)
                {
                    outMf.Coords.Add(new KeyValuePair<double, double>(inMf.getX(i - 1, i, n), n * weight));
                    outMf.Coords.Add(new KeyValuePair<double, double>(inMf.Coords[i].Key, inMf.Coords[i].Value * weight));
                }
            }
            return outMf;
        }

        /// <summary>
        /// The product Implicator
        /// Apply the Product implication method to a membership function.
        /// </summary>
        /// <param name="inMf">membership function to implicate</param>
        /// <param name="outMf">membership function to hold the output</param>
        /// <param name="n">The antecedent to reshape the membership function with.</param>
        /// <param name="weight">The weight of the rule</param>
        public static MemberFunction ImpProduct(MemberFunction inMf, double n, double weight)
        {
            MemberFunction outMf = new MemberFunction();
            for (int i = 0; i < inMf.Coords.Count; i++)
                outMf.Coords.Add(new KeyValuePair<double, double>(inMf.Coords[i].Key, inMf.Coords[i].Value * n * weight));
            return outMf;
        }

        /// <summary>
        /// Aggregate two membership functions using the Maximum method.
        /// </summary>
        /// <param name="inMf">One of the membership functions to aggregate.This one will not be modified.</param>
        /// <param name="outMf"> The other membership function to aggregate. This one will be modified to hold the</param>
        public static void AggMax(MemberFunction inMf, MemberFunction outMf)
        {

            if (0 == outMf.Coords.Count)
                outMf.Copy(inMf);
            else
            {
                // Key is X and Value is Y
                Dictionary<double, double> coords = new Dictionary<double, double>();

                foreach (KeyValuePair<double, double> inMfXY in inMf.Coords)
                    if (coords.ContainsKey(inMfXY.Key))
                        coords[inMfXY.Key] = Math.Max(coords[inMfXY.Key], Math.Max(inMfXY.Value, outMf.fuzzify(inMfXY.Key)));
                    else
                        coords[inMfXY.Key] = Math.Max(inMfXY.Value, outMf.fuzzify(inMfXY.Key));

                foreach (KeyValuePair<double, double> outMfXY in outMf.Coords)
                    if (coords.ContainsKey(outMfXY.Key))
                        coords[outMfXY.Key] = Math.Max(coords[outMfXY.Key], Math.Max(inMf.fuzzify(outMfXY.Key), outMfXY.Value));
                    else
                        coords[outMfXY.Key] = Math.Max(inMf.fuzzify(outMfXY.Key), outMfXY.Value);

                // Let's empty out the MF and build it up again
                outMf.clear();

                // Now loop over every line segment in inMf and see if it intersects with any segment in outMf
                for (int i = 1; i < inMf.Coords.Count; i++)
                {
                    for (int j = 1; j < outMf.Coords.Count; j++)
                    {
                        Tuple<double, double, bool> intersect = IntersectLines(inMf.Coords[i - 1].Key, inMf.Coords[i - 1].Value,
                            inMf.Coords[i].Key, inMf.Coords[i].Value, outMf.Coords[j - 1].Key,
                            outMf.Coords[j - 1].Value, outMf.Coords[j].Key, outMf.Coords[j].Value);
                        // Is there an intersection?
                        if (intersect.Item3 == true)
                            if (coords.ContainsKey(intersect.Item1))
                                coords[intersect.Item1] = Math.Max(coords[intersect.Item1], intersect.Item2);
                            else
                                coords[intersect.Item1] = intersect.Item2;
                    }
                }

                // Now make a list of the X values in order
                List<double> xs = coords.Keys.ToList();
                xs.Sort();

                // Now loop over all the keys by index (int)
                for (int i = 0; i < xs.Count; i++)
                {
                    double x = xs[i];
                    // If the first value isn't zero then push a zero value onto the stack
                    if (i == 0 && coords[x] != 0.0)
                        outMf.Coords.Add(new KeyValuePair<double, double>(x, 0));

                    outMf.Coords.Add(new KeyValuePair<double, double>(x, coords[x]));

                    // Push a zero onto the stack if the last value isn't zero
                    if (i == coords.Count - 1 && coords[x] != 0.0)
                        outMf.Coords.Add(new KeyValuePair<double, double>(x, 0));
                }

            }
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
                if (double.IsNaN(aIntercept))
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
                    x = (bIntercept - aIntercept) / (aSlope - bSlope);
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
