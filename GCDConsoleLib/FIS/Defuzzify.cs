namespace GCDConsoleLib.FIS
{
    public static class Defuzzify
    {
        /// <summary>
        /// Defuzzify a membership function using the Centroid method
        /// 
        /// NOTE: This method was found to be problematic and so we modeled it after the
        /// centroid from SciPy fuzzy:
        /// https://github.com/scikit-fuzzy/scikit-fuzzy/blob/master/skfuzzy/defuzzify/defuzz.py
        /// </summary>
        /// <param name="mf"></param>
        /// <returns></returns>
        public static double DefuzzCentroid(MemberFunction mf)
        {
            double sum_moment_area = 0;
            double sum_area = 0;

            for (int i = 1; i < mf.Length; i++)
            {
                double moment = 0;
                double area = 0;
                double x1 = mf.Coords[i - 1].Key;
                double x2 = mf.Coords[i].Key;
                double y1 = mf.Coords[i - 1].Value;
                double y2 = mf.Coords[i].Value;

                // Check that this isn't zero height or zero length
                if (!(y1 == 0 && y2 == 0) && x1 != x2)
                {
                    // Rectangle
                    if (y1 == y2)
                    {
                        moment = 0.5 * (x1 + x2);
                        area = (x2 - x1) * y1;
                    }
                    // Triangle with height y2
                    else if (y1 == 0 && y2 != 0)
                    {
                        moment = (2 * x2 + x1) / 3;
                        area = 0.5 * (x2 - x1) * y2;
                    }
                    // Triangle with height y1
                    else if (y2 == 0 && y1 != 0)
                    {
                        moment = (2 * x1 + x2) / 3;
                        area = 0.5 * (x2 - x1) * y1;
                    }
                    // Other cases
                    else
                    {
                        moment = (2 / 3 * (x2 - x1) * (y2 + 0.5 * y1)) / (y1 + y2) + x1;
                        area = 0.5 * (x2 - x1) * (y1 + y2);
                    }
                }
                sum_moment_area += moment * area;
                sum_area += area;
            }

            return sum_moment_area / sum_area;
        }

        /// <summary>
        /// Defuzzify a membership function using the Bisector method.
        /// 
        /// </summary>
        /// <param name="mf"></param>
        /// <returns></returns>
        public static double FISDefuzzBisect(MemberFunction mf)
        {
            double area = 0;

            for (int i = 0; i < mf.Length - 1; i++)
                area += mf.Coords[i].Key * mf.Coords[i + 1].Value - mf.Coords[i + 1].Key * mf.Coords[i].Value;

            area += mf.Coords[mf.Length - 1].Key * mf.Coords[0].Value - mf.Coords[0].Key * mf.Coords[mf.Length - 1].Value;
            area = area / 2;

            double xMin = 0;
            double xMax = 0;
            double yLast = 0;
            double x = 0;
            double y = 0;
            double m = 0;
            double tmpArea = 0;

            for (int i = 0; i < mf.Length - 1; i++)
            {
                m = mf.Coords[i].Key * mf.Coords[i + 1].Value - mf.Coords[i + 1].Key * mf.Coords[i].Value;
                if (tmpArea + m < area)
                {
                    tmpArea += m;
                }
                else
                {
                    xMin = mf.Coords[i].Key;
                    xMax = mf.Coords[i + 1].Key;
                    yLast = mf.Coords[i].Value;
                    x = xMax;
                    while (area - tmpArea > 0.000001)
                    {
                        x = (xMax - xMin) / 2;
                        y = mf.fuzzify(x);
                        m = xMin * y - x * yLast;
                        if (tmpArea + m > area)
                        {
                            xMax = x;
                        }
                        else if (tmpArea + m <= area)
                        {
                            tmpArea += m;
                            xMin = x;
                        }
                    }
                }
            }
            return x;
        }

        /// <summary>
        /// Defuzzify a membership function using the Middle of Maximum method.
        /// </summary>
        /// <param name="mf"></param>
        /// <returns></returns>
        public static double FISDefuzzMidMax(MemberFunction mf)
        {
            double minX = mf.Coords[0].Key;
            double maxX = mf.Coords[0].Key;
            double y = mf.Coords[0].Value;
            for (int i = 1; i < mf.Length; i++)
            {
                if (mf.Coords[i].Value > y)
                {
                    minX = mf.Coords[i].Key;
                    maxX = minX;
                    y = mf.Coords[i].Value;
                }
                else if (mf.Coords[i].Value == y)
                {
                    maxX = mf.Coords[i].Key;
                }
            }
            return (maxX - minX) / 2;
        }

        /// <summary>
        /// Defuzzify a membership function using the Largest of Maximum method.
        /// </summary>
        /// <param name="mf"></param>
        /// <returns></returns>
        public static double FISDefuzzLargeMax(MemberFunction mf)
        {
            double x = mf.Coords[0].Key;
            double y = mf.Coords[0].Value;
            for (int i = 1; i < mf.Length; i++)
            {
                if (mf.Coords[i].Value >= y)
                {
                    x = mf.Coords[i].Key;
                    y = mf.Coords[i].Value;
                }
            }
            return x;
        }


        /// <summary>
        /// Defuzzify a membership function using the Smallest of Maximum method.
        /// </summary>
        /// <param name="mf"></param>
        /// <returns></returns>
        public static double FISDefuzzSmallMax(MemberFunction mf)
        {
            double x = mf.Coords[0].Key;
            double y = mf.Coords[0].Value;
            for (int i = 1; i < mf.Length; i++)
            {
                if (mf.Coords[i].Value > y)
                {
                    x = mf.Coords[i].Key;
                    y = mf.Coords[i].Value;
                }
            }
            return x;
        }

    }
}
