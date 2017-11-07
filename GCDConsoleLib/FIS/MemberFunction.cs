using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDConsoleLib.FIS
{
    public class MemberFunction
    {
        private int _n;
        private double _min, _max;
        private List<MemberFunction> _mfs;
        private Dictionary<String, int> _indices;
        private String _msg;


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
