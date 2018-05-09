using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDConsoleLib.Utility
{
    public class IntervalMath  
    {
        /// <summary>
        /// Choose a clean division that is a muliple of 5 or 10
        /// </summary>
        /// <param name="startWidth"></param>
        /// <returns></returns>
        public static decimal GetNearestFiveOrderWidth(decimal startWidth)
        {
            // Special case. Constant rasters will generate this.
            if (startWidth == 0) return 0;

            int order = (int)Math.Round(Math.Log10((double)startWidth));
            decimal tener = (decimal)Math.Pow(10, order);

            Dictionary<decimal, decimal> compares = new Dictionary<decimal, decimal>()
            {
                {tener, Math.Abs(tener - startWidth) },
                {(tener/2), Math.Abs((tener/2) - startWidth) },
                {(tener * 5),  Math.Abs((tener * 5) - startWidth) },
            };
            return compares.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
        }

        /// <summary>
        /// Returns a range rounded up or down to the nearest log10 with an optional buffer
        /// This is mostly used in charting
        /// </summary>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="buffer">between 0 and 1</param>
        /// <returns>Note: this method fails silently and returns the inputs if bad behaviour is detected</returns>
        public static Tuple<decimal, decimal> GetRegularizedMaxMin(decimal max, decimal min, decimal buffer = 0)
        {
            try
            {
                if (buffer < 0 || buffer > 1)
                    throw new Exception("bad behaviour");

                decimal buffmax = max + max * buffer;
                decimal buffmin = min + min * buffer;
                decimal range = buffmax - buffmin;

                // Get the order of the range
                int order = (int)Math.Floor(Math.Log10((double)(range)));

                // Two options for the order of things
                //decimal optSm = (decimal)Math.Pow(10, order - 1);
                decimal opt = (decimal)Math.Pow(10, order) / 2;

                decimal newMax = Math.Ceiling(buffmax / opt) * opt;
                decimal newMin = Math.Floor(buffmin / opt) * opt;

                return new Tuple<decimal, decimal>(newMax, newMin);
            }
            catch (Exception)
            {}
            return new Tuple<decimal, decimal>(max, min);
        }

        /// <summary>
        /// Choose an interval that guarantees a zero point
        /// </summary>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <returns></returns>
        public static decimal GetSensibleChartInterval(decimal max, decimal min, int aimFor)
        {
            // Get the order of the range
            int order = (int)Math.Round(Math.Log10((double)(max - min)));
            decimal range = max - min;
            decimal zeropoint = Math.Abs(min);

            // Now make a list of possibilities
            SortedDictionary<decimal, decimal> possibles = new SortedDictionary<decimal, decimal>();
            for (int o = order; o >= 0; o--)
            {
                decimal tener = (decimal)Math.Pow(10, o);
                List<decimal> combos = new List<decimal> { tener, tener / 2, tener * 2, tener * 5, tener / 5 };

                foreach(decimal theVal in combos)
                {
                    if (range % theVal == 0 && zeropoint % theVal == 0)
                    {
                        decimal tenerscore = Math.Abs((range / theVal) - aimFor);
                        possibles[tenerscore] = theVal;
                    }
                }
                
            }

            if (possibles.Count == 0)
                return 0;

            return possibles[possibles.Keys.First()];

        }

    }
}
