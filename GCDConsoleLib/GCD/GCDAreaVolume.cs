using System;
using UnitsNet;
using UnitsNet.Units;

namespace GCDConsoleLib.GCD
{
    public class GCDAreaVolume
    {
        public int Count { get; private set; }

        // _sum has an implied unit so we really don't want to expose it.
        // it is a double because the rasters get read as a double
        private double _sum { get; set; }

        /// <summary>
        /// Initialize the Count and Sum to 0
        /// </summary>
        /// <param name="cellArea"></param>
        /// <param name="vUnit"></param>
        public GCDAreaVolume()
        {
            Count = 0;
            _sum = 0;
        }

        /// <summary>
        /// Initialize the Count and Sum to Values
        /// </summary>
        /// <param name="count"></param>
        /// <param name="sum"></param>
        /// <param name="cellArea"></param>
        /// <param name="vUnit"></param>
        public GCDAreaVolume(int count, double sum)
        {
            Count = count;
            _sum = sum;
        }

        /// <summary>
        /// Initialize using area / vol and cell area
        /// </summary>
        /// <param name="ar"></param>
        /// <param name="vol"></param>
        /// <param name="cellArea"></param>
        public GCDAreaVolume(Area ar, Volume vol, Area cellArea)
        {
            SetArea(ar, cellArea);
            SetVolume(vol, cellArea);
        }

        /// <summary>
        /// Mainly Used for Testing
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(GCDAreaVolume other)
        {
            return Count.Equals(other.Count) && _sum.Equals(other._sum);
        }

        /// <summary>
        /// Add to the Sum and increment the counter
        /// </summary>
        /// <param name="val"></param>
        public void AddToSumAndIncrementCounter(double val) { _sum += val; Count++; }

        /// <summary>
        /// Increment the Counter
        /// </summary>
        /// <param name="num"></param>
        public void IncrementCount(int num) { Count++; }

        /// <summary>
        /// Add to the sum 
        /// </summary>
        /// <param name="val"></param>
        public void AddToSum(double val) { _sum += val; }

        /// <summary>
        /// Get the actual area value
        /// </summary>
        /// <param name="cellArea"></param>
        /// <returns></returns>
        public Area GetArea(Area cellArea) { return Count * cellArea; }

        /// <summary>
        /// Set the Area directly
        /// </summary>
        /// <param name="theArea"></param>
        /// <param name="cellArea"></param>
        public void SetArea(Area theArea, Area cellArea)  {  Count =(int)( theArea.SquareMeters / cellArea.SquareMeters); }

        /// <summary>
        /// Get the Area in whatever unit you want
        /// </summary>
        /// <param name="cellArea"></param>
        /// <param name="outputUnits"></param>
        /// <returns></returns>
        public double GetAreaValue(Area cellArea, AreaUnit outputUnits) { return GetArea(cellArea).As(outputUnits); }

        /// <summary>
        /// Get the actual volume value in any unit you choose
        /// </summary>
        /// <param name="cellArea"></param>
        /// <param name="vUnit"></param>
        /// <returns></returns>
        public Volume GetVolume(Area cellArea, LengthUnit vUnit) { return Volume.FromCubicMeters(Length.From(_sum, vUnit).Meters * cellArea.SquareMeters); }

        /// <summary>
        /// Set the volume directly
        /// </summary>
        /// <param name="vol"></param>
        /// <param name="cellArea"></param>
        public void SetVolume(Volume vol, Area cellArea) {  _sum = vol.CubicMeters / cellArea.SquareMeters;  }
    }
}
