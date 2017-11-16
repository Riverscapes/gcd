using System;
using System.Collections.Generic;

namespace GCDConsoleLib.FIS
{
    public class MemberFunction
    {
        public List<KeyValuePair<double, double>> Coords;
        public Dictionary<String, int> Indices;

        public double yMax;

        public MemberFunction()
        {
            Coords = new List<KeyValuePair<double, double>>();
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
            Coords = new List<KeyValuePair<double, double>>();
            yMax = dyMax;
            if ((yMax <= 0) || (yMax > 1))
                throw new ArgumentException(string.Format("Invalid yMax of {0}. It must be between 0 and 1.", yMax));
            else if ((x1 > x2) || (x2 > x3))
                throw new ArgumentException(string.Format("Membership function vertices ({0} {1} {2}) must be in ascending order.", x1, x2, x3));
            else
            {
                Coords.Add(new KeyValuePair<double, double>(x1, 0));
                Coords.Add(new KeyValuePair<double, double>(x2, yMax));
                Coords.Add(new KeyValuePair<double, double>(x3, 0));
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
            Coords = new List<KeyValuePair<double, double>>();
            yMax = dyMax;
            if ((yMax <= 0) || (yMax > 1))
                throw new ArgumentException(string.Format("Invalid yMax of {0}. It must be between 0 and 1.", yMax));
            else if ((x1 > x2) || (x2 >= x3) || (x3 > x4))
                throw new ArgumentException(string.Format("Membership function vertices ({0} {1} {2} {3}) must be in ascending order.", x1, x2, x3, x4));
            else
            {
                Coords.Add(new KeyValuePair<double, double>(x1, 0));
                Coords.Add(new KeyValuePair<double, double>(x2, yMax));
                Coords.Add(new KeyValuePair<double, double>(x3, yMax));
                Coords.Add(new KeyValuePair<double, double>(x4, 0));
            }
        }

        /// <summary>
        /// Constructor, using two vectors of coordinates.
        /// The x coordinates must be in order from smallest to largest.The x and y coordinates must be in the
        /// same order.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public MemberFunction(List<KeyValuePair<double, double>> kvp)
        {
            if (0 == kvp.Count)
                throw new ArgumentException("No coordinates provided.");
            Coords = kvp;
        }

        /// <summary>
        /// Clear all vertices out of this function
        /// </summary>
        public void clear()
        {
            Coords.Clear();
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
                if ((x >= Coords[i - 1].Key) && (x <= Coords[i].Key))
                {
                    // Asymptote special case
                    // NOTE: THIS HAS Been tested for the MAX workflow only.
                    // TODO: Need to test this with other FIS cases.
                    if (Coords[i].Key - Coords[i - 1].Key == 0)
                        result = Math.Max(Coords[i].Value, Coords[i - 1].Value);
                    else
                        result = Coords[i - 1].Value + (x - Coords[i - 1].Key)
                            * ((Coords[i].Value - Coords[i - 1].Value)
                            / (Coords[i].Key - Coords[i - 1].Key));
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Computes the x coordinate that corresponds to a given y coordinate in between two vertices.
        /// </summary>
        /// <param name="v1">The index of the first vertex</param>
        /// <param name="v2">The index of the second vertex</param>
        /// <param name="y">The y coordinate</param>
        /// <returns>The x coordinate</returns>
        public double getX(int v1, int v2, double y)
        {
            return (Coords[v2].Key - Coords[v1].Key)
                / (Coords[v2].Value - Coords[v1].Value)
                * (y - Coords[v1].Value) + Coords[v1].Key;
        }

        /// <summary>
        /// Check if a membership function is valid.
        /// </summary>
        public bool Valid { get { return Coords.Count > 0; } }

        /// <summary>
        /// Simple Property
        /// </summary>
        public int Length { get { return Coords.Count; } }

        /// <summary>
        /// Apply the Minimum implication method to a membership function.
        /// This is a static function that does not apply to a specific object.
        /// </summary>
        /// <param name="inMf">The membership function to implicate.</param>
        /// <param name="outMf">The membership function to hold the output.</param>
        /// <param name="n">The antecedent to reshape the membership function with.</param>
        /// <param name="weight">The weight of the rule.</param>
        void ImpMin(MemberFunction inMf, MemberFunction outMf, double n, double weight)
        {
            outMf.clear();
            outMf.Coords.Add(new KeyValuePair<double, double>(inMf.Coords[0].Key, inMf.Coords[0].Value * weight));

            int i;
            for (i = 1; i < inMf.Length - 1; i++)
            {
                if (inMf.Coords[i].Value > n)
                {
                    outMf.Coords.Add(new KeyValuePair<double, double>(inMf.getX(i - 1, i, n), n * weight));
                    break;
                }
                else
                {
                    outMf.Coords.Add(new KeyValuePair<double, double>(inMf.Coords[i].Key, inMf.Coords[i].Value * weight));
                }
            }
            // NOTE: this was  (i = i; i < inMf.Length; i++) in the original code
            for (i = 1; i < inMf.Length; i++)
            {
                if (inMf.Coords[i].Value < n)
                {
                    outMf.Coords.Add(new KeyValuePair<double, double>(inMf.getX(i - 1, i, n), n * weight));
                    outMf.Coords.Add(new KeyValuePair<double, double>(inMf.Coords[i].Key, inMf.Coords[i].Value * weight));
                }
            }
        }


        /// <summary>
        /// Apply the Product implication method to a membership function.
        /// This is a static function that does not apply to a specific object.
        /// </summary>
        /// <param name="inMf">The membership function to implicate.</param>
        /// <param name="outMf">The membership function to hold the output.</param>
        /// <param name="n">The antecedent to reshape the membership function with.</param>
        /// <param name="weight">The weight of the rule.</param>
        void ImpProduct(MemberFunction inMf, MemberFunction outMf, double n, double weight)
        {
            outMf.clear();
            for (int i = 0; i < inMf.Length; i++)
            {
                outMf.Coords.Add(new KeyValuePair<double, double>(inMf.Coords[i].Key, inMf.Coords[i].Value * n * weight));
            }
        }

        /// <summary>
        /// Aggregate two membership functions using the Maximum method.
        /// </summary>
        /// <param name="inMf">One of the membership functions to aggregate.</param>
        /// <param name="outMf">The other membership function to aggregate.</param>
        static MemberFunction AggMax(MemberFunction mfA, MemberFunction MfB)
        {
            MemberFunction retBVal;
            if (0 == MfB.Length)
            {
                retBVal = MfB;
            }
            else
            {
                List<KeyValuePair<double, double>> coords = new List<KeyValuePair<double, double>>();
                foreach(KeyValuePair<double,double> kvpA in mfA.Coords)
                {
                    double val = kvpA.Value;
                    foreach (KeyValuePair<double, double> kvpB in MfB.Coords)
                    {
                        if (kvpA.Key == kvpB.Key)
                        {
                            val = Math.Max(kvpA.Value, kvpB.Value);
                        }
                    }
                    coords.Add(new KeyValuePair<double, double>(kvpA.Key, val));
                }

                retBVal = new MemberFunction(coords);

                //TODO: THIS IS A MESS: HERE's THE ORIGINA:
                //std::map<double, double> coords;
                //double x, y;
                //int i, j;
                //for (i = 0; i < inMf->n_; i++)
                //    coords[inMf->x_[i]] = std::max(coords[inMf->x_[i]], std::max(inMf->y_[i], outMf->fuzzify(inMf->x_[i])));
                //for (i = 0; i < outMf->n_; i++)
                //    coords[outMf->x_[i]] = std::max(coords[outMf->x_[i]], std::max(inMf->fuzzify(outMf->x_[i]), outMf->y_[i]));
                //for (i = 1; i < inMf->n_; i++)
                //{
                //    for (j = 1; j < outMf->n_; j++)
                //    {
                //        if (FISIntersectLines(inMf->x_[i - 1], inMf->y_[i - 1], inMf->x_[i], inMf->y_[i], outMf->x_[j - 1],
                //                              outMf->y_[j - 1], outMf->x_[j], outMf->y_[j], &x, &y))
                //            coords[x] = std::max(coords[x], y);
                //    }
                //}
                //outMf->clear();
                //std::map<double, double>::iterator it = coords.begin();
                //// If the first value isn't zero then push a zero value onto the stack
                //if (0 != (*it).second)
                //{
                //    outMf->x_.push_back((*it).first);
                //    outMf->y_.push_back(0);
                //}
                //for (it = it; it != coords.end(); it++)
                //{
                //    outMf->x_.push_back((*it).first);
                //    outMf->y_.push_back((*it).second);
                //}
                //it--;
                //// Push a zero onto the stack if the last value isn't zero
                //if (0 != (*it).second)
                //{
                //    outMf->x_.push_back((*it).first);
                //    outMf->y_.push_back(0);
                //}

            }
            return retBVal;
        }

        /// <summary>
        /// 
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
        public bool IntersectLines(double ax1, double ay1, double ax2, double ay2, double bx1, double by1,
                                                 double bx2, double by2, double x, double y)
        {
            return true;
        }

        /// <summary>
        /// The Maximum OR fuzzy operator.
        /// </summary>
        /// <param name="n1">The first value to apply the operator to.</param>
        /// <param name="n2">The second value to apply the operator to.</param>
        /// <returns>The output from the fuzzy operator.</returns>
        public double OpMax(double n1, double n2)
        {
            return Math.Max(n1, n2);
        }

        /// <summary>
        /// The Minimum AND fuzzy operator.
        /// </summary>
        /// <param name="n1">The first value to apply the operator to.</param>
        /// <param name="n2">The second value to apply the operator to.</param>
        /// <returns>The output from the fuzzy operator.</returns>
        public double OpMin(double n1, double n2)
        {
            return Math.Min(n1, n2);
        }

        /// <summary>
        /// The Probabalistic OR fuzzy operator.
        /// </summary>
        /// <param name="n1">The first value to apply the operator to.</param>
        /// <param name="n2">The second value to apply the operator to.</param>
        /// <returns>The output from the fuzzy operator.</returns>
        public double OpProbor(double n1, double n2)
        {
            return n1 + n2 - n1 * n2;
        }

        /// <summary>
        /// The Product AND fuzzy operator.
        /// </summary>
        /// <param name="n1">The first value to apply the operator to.</param>
        /// <param name="n2">The second value to apply the operator to.</param>
        /// <returns>The output from the fuzzy operator.</returns>
        public double OpProduct(double n1, double n2)
        {
            return n1 * n2;
        }

    }
}
