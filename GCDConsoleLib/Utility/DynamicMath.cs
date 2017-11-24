using System;
using UnitsNet;

namespace GCDConsoleLib.Utility
{

    public static class DynamicMath
    {
        /// <summary>
        /// You can't just do (T * T) so we wrote a few helper methods to do this for us.
        /// </summary>
        public static dynamic Add(dynamic a, dynamic b) { return a + b; }
        public static dynamic Subtract(dynamic a, dynamic b) { return a - b; }
        public static dynamic Multiply(dynamic a, dynamic b) { return a * b; }
        public static dynamic Divide(dynamic a, dynamic b) { return a / b; }


        /// <summary>
        /// Return zero if we're ever dividing by zero
        /// </summary>
        /// <param name="fNumerator"></param>
        /// <param name="fDenominator"></param>
        /// <returns></returns>
        private static decimal SafeDivision(decimal fNumerator, decimal fDenominator)
        {
            decimal val = 0;
            if (fDenominator != 0)
                val = fNumerator / fDenominator;
            return val;
        }

        /// <summary>
        /// Dividing two lengths
        /// </summary>
        /// <param name="vNum"></param>
        /// <param name="vDenom"></param>
        /// <returns></returns>
        public static decimal SafeDivision(Length vNum, Length vDenom)
        {
            decimal vNummmm = (decimal)vNum.Meters;
            decimal vDenommm = (decimal)vDenom.Meters;

            return SafeDivision(vNummmm, vDenommm);
        }

        /// <summary>
        /// Dividing two volumes
        /// </summary>
        /// <param name="fNumerator"></param>
        /// <param name="fDenominator"></param>
        /// <returns></returns>
        public static decimal SafeDivision(Volume vNum, Volume vDenom)
        {
            decimal vNummmm = (decimal)vNum.CubicMeters;
            decimal vDenommm = (decimal)vDenom.CubicMeters;

            return SafeDivision(vNummmm, vDenommm);
        }

        /// <summary>
        /// Dividing two areas to get a fraction
        /// </summary>
        /// <param name="vNum"></param>
        /// <param name="vDenom"></param>
        /// <returns>decimal fraction</returns>
        public static decimal SafeDivision(Area vNum, Area vDenom)
        {
            decimal vNummmm = (decimal)vNum.SquareMeters;
            decimal vDenommm = (decimal)vDenom.SquareMeters;

            return SafeDivision(vNummmm, vDenommm);
        }

        /// <summary>
        /// Divide a volume by an area to get a length
        /// </summary>
        /// <param name="vNum"></param>
        /// <param name="vDenom"></param>
        /// <returns>UnitsNet Length object</returns>
        public static Length SafeDivision(Volume vNum, Area vDenom)
        {
            decimal vNummmm = (decimal)vNum.CubicMeters;
            decimal vDenommm = (decimal)vDenom.SquareMeters;

            return Length.FromMeters((double)SafeDivision(vNummmm, vDenommm));
        }

        /// <summary>
        /// Divide a Area by a Length to get a length
        /// </summary>
        /// <param name="vNum"></param>
        /// <param name="vDenom"></param>
        /// <returns>UnitsNet Length object</returns>
        public static Length SafeDivision(Area vNum, Length vDenom)
        {
            decimal vNummmm = (decimal)vNum.SquareMeters;
            decimal vDenommm = (decimal)vDenom.Meters;

            return Length.FromMeters((double)SafeDivision(vNummmm, vDenommm));
        }

        /// <summary>
        /// Divide a Volume by a Length to get an area
        /// </summary>
        /// <param name="vNum"></param>
        /// <param name="vDenom"></param>
        /// <returns>UnitsNet Length object</returns>
        public static Area SafeDivision(Volume vNum, Length vDenom)
        {
            decimal vNummmm = (decimal)vNum.CubicMeters;
            decimal vDenommm = (decimal)vDenom.Meters;

            return Area.FromSquareMeters((double)SafeDivision(vNummmm, vDenommm));
        }

    }
}
