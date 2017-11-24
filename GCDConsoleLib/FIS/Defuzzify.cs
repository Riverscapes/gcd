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
        /// https://www.mathworks.com/help/fuzzy/examples/defuzzification-methods.html
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
                double x1 = mf.Coords[i - 1][0];
                double x2 = mf.Coords[i][0];
                double y1 = mf.Coords[i - 1][1];
                double y2 = mf.Coords[i][1];

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
        /// Find the area underneath a line segment
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static double Area(double[] ptA, double[] ptB)
        {
            double x1 = ptA[0];
            double x2 = ptB[0];
            double y1 = ptA[1];
            double y2 = ptB[1];
            double area = 0;
            // Check that this isn't zero height or zero length
            if (!(y1 == 0 && y2 == 0) && x1 != x2)
            {
                // Rectangle
                if (y1 == y2)
                    area = (x2 - x1) * y1;
                // Triangle with height y2
                else if (y1 == 0 && y2 != 0)
                    area = 0.5 * (x2 - x1) * y2;
                // Triangle with height y1
                else if (y2 == 0 && y1 != 0)
                    area = 0.5 * (x2 - x1) * y1;
                // Other cases
                else
                    area = 0.5 * (x2 - x1) * (y1 + y2);
            }
            return area;
        }

        /// <summary>
        /// Find the moment of inertia along the X axis
        /// </summary>
        /// <param name="ptA"></param>
        /// <param name="ptB"></param>
        /// <returns></returns>
        public static double Moment(double[] ptA, double[] ptB)
        {
            double x1 = ptA[0];
            double x2 = ptB[0];
            double y1 = ptA[1];
            double y2 = ptB[1];
            double moment = 0;
            // Check that this isn't zero height or zero length
            if (!(y1 == 0 && y2 == 0) && x1 != x2)
            {
                // Rectangle
                if (y1 == y2)
                    moment = 0.5 * (x1 + x2);
                // Triangle with height y2
                else if (y1 == 0 && y2 != 0)
                    moment = (2 * x2 + x1) / 3;
                // Triangle with height y1
                else if (y2 == 0 && y1 != 0)
                    moment = (2 * x1 + x2) / 3;
                // Other cases
                else
                    moment = (2 / 3 * (x2 - x1) * (y2 + 0.5 * y1)) / (y1 + y2) + x1;
            }
            return moment;
        }

        /// <summary>
        /// Defuzzify a membership function using the Bisector method.
        /// https://www.mathworks.com/help/fuzzy/examples/defuzzification-methods.html
        /// </summary>
        /// <param name="mf"></param>
        /// <returns></returns>
        public static double DefuzzBisect(MemberFunction mf)
        {
            double areaTotal = 0;
            double[] areas = new double[mf.Length];

            // Find the area of each segment
            for (int i = 0; i < mf.Length - 1; i++)
            {
                double x1 = mf.Coords[i][0];
                double x2 = mf.Coords[i + 1][0];
                double y1 = mf.Coords[i][1];
                double y2 = mf.Coords[i + 1][1];
                areas[i] = 0.5 * (x2 - x1) * (y1 + y2);
                areaTotal += areas[i];
            }
            double halfArea = areaTotal / 2;

            double xMin = 0;
            double xMax = 0;
            double yLast = 0;
            double x = 0;
            double y = 0;
            double m = 0;
            // This is our accumulating area
            double tmpArea = 0;

            for (int i = 0; i < mf.Length - 1; i++)
            {
                // Get the area of this segment
                m = areas[i];
                x = xMax = mf.Coords[i][0];
                // Add points in until we cross over to the haldway point
                if (tmpArea + m <= halfArea)
                    tmpArea += m;
                // Cut the distance in hald until we are reasonable close to the point we want
                else
                {
                    xMin = mf.Coords[i][0];
                    xMax = mf.Coords[i + 1][0];
                    yLast = mf.Coords[i][1];
                    while (halfArea - tmpArea > 0.000001)
                    {
                        x = xMin + ((xMax - xMin) / 2);
                        y = mf.fuzzify(x);

                        m = Area(new double[2] { xMin, yLast }, new double[2] { x, y });

                        if (tmpArea + m > halfArea)
                            xMax = x;

                        else if (tmpArea + m <= halfArea)
                        {
                            tmpArea += m;
                            xMin = x;
                        }
                    }
                    break;
                }
            }
            return x;
        }

        /// <summary>
        /// Defuzzify a membership function using the Middle of Maximum method.
        /// https://www.mathworks.com/help/fuzzy/examples/defuzzification-methods.html
        /// </summary>
        /// <param name="mf"></param>
        /// <returns></returns>
        public static double FISDefuzzMidMax(MemberFunction mf)
        {
            double minX = mf.Coords[0][0];
            double maxX = mf.Coords[0][0];
            double y = mf.Coords[0][1];
            for (int i = 1; i < mf.Length; i++)
            {
                if (mf.Coords[i][1] > y)
                {
                    minX = mf.Coords[i][0];
                    maxX = minX;
                    y = mf.Coords[i][1];
                }
                else if (mf.Coords[i][1] == y)
                {
                    maxX = mf.Coords[i][0];
                }
            }
            return minX + (maxX - minX) / 2;
        }

        /// <summary>
        /// Defuzzify a membership function using the Largest of Maximum method.
        /// </summary>
        /// <param name="mf"></param>
        /// <returns></returns>
        public static double FISDefuzzLargeMax(MemberFunction mf)
        {
            double x = mf.Coords[0][0];
            double y = mf.Coords[0][1];
            for (int i = 1; i < mf.Length; i++)
            {
                if (mf.Coords[i][1] >= y)
                {
                    x = mf.Coords[i][0];
                    y = mf.Coords[i][1];
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
            double x = mf.Coords[0][0];
            double y = mf.Coords[0][1];
            for (int i = 1; i < mf.Length; i++)
            {
                if (mf.Coords[i][1] > y)
                {
                    x = mf.Coords[i][0];
                    y = mf.Coords[i][1];
                }
            }
            return x;
        }

    }
}
