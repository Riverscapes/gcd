using System;
using UnitsNet.Units;
using UnitsNet;

namespace GCDConsoleLib.GCD
{
    /// <summary>
    /// NOTE: This partial class was split off from DoDStats because it was really verbose
    /// </summary>
    public partial class DoDStats
    {
        // Areal Properties
        public Area AreaDetectableChange_Thresholded { get { return ErosionThr.GetArea(CellArea) + DepositionThr.GetArea(CellArea); } }
        public Area AreaOfInterest_Raw { get { return ErosionRaw.GetArea(CellArea) + DepositionRaw.GetArea(CellArea); } }
        public decimal AreaPercentAreaInterestWithDetectableChange { get { return 100 * SafeDivision(AreaDetectableChange_Thresholded, AreaOfInterest_Raw); } }

        // Volume Properties
        public Volume VolumeOfDifference_Raw { get { return ErosionRaw.GetVolume(CellArea, StatsUnits.VertUnit) + DepositionRaw.GetVolume(CellArea, StatsUnits.VertUnit); } }
        public Volume VolumeOfDifference_Thresholded { get { return ErosionThr.GetVolume(CellArea, StatsUnits.VertUnit) + DepositionThr.GetVolume(CellArea, StatsUnits.VertUnit); } }
        public Volume VolumeOfDifference_Error { get { return ErosionErr.GetVolume(CellArea, StatsUnits.VertUnit) + DepositionErr.GetVolume(CellArea, StatsUnits.VertUnit); } }

        public Volume NetVolumeOfDifference_Raw { get { return DepositionRaw.GetVolume(CellArea, StatsUnits.VertUnit) - ErosionRaw.GetVolume(CellArea, StatsUnits.VertUnit); } }
        public Volume NetVolumeOfDifference_Thresholded { get { return DepositionThr.GetVolume(CellArea, StatsUnits.VertUnit) - ErosionThr.GetVolume(CellArea, StatsUnits.VertUnit); } }
        public Volume NetVolumeOfDifference_Error
        {
            get
            {
                // In order to do all the math on this we need to convert them to decimals and then back into their preferred unit.
                decimal depErrCubMeter = (decimal)DepositionErr.GetVolume(CellArea, StatsUnits.VertUnit).CubicMeters;
                decimal erosionErrCubMeter = (decimal)ErosionErr.GetVolume(CellArea, StatsUnits.VertUnit).CubicMeters;
                return Volume.FromCubicMeters(Math.Sqrt(Math.Pow((double)depErrCubMeter, 2) + Math.Pow((double)erosionErrCubMeter, 2)));
            }
        }

        public decimal VolumeOfErosion_Percent { get { return SafeDivision(ErosionErr.GetVolume(CellArea, StatsUnits.VertUnit), ErosionThr.GetVolume(CellArea, StatsUnits.VertUnit)) * 100; } }
        public decimal VolumeOfDeposition_Percent { get { return SafeDivision(DepositionErr.GetVolume(CellArea, StatsUnits.VertUnit), DepositionThr.GetVolume(CellArea, StatsUnits.VertUnit)) * 100; } }

        public decimal VolumeOfDifference_Percent { get { return SafeDivision(VolumeOfDifference_Error, VolumeOfDifference_Thresholded) * 100; } }
        public decimal NetVolumeOfDifference_Percent { get { return SafeDivision(NetVolumeOfDifference_Error, NetVolumeOfDifference_Thresholded) * 100; } }

        // Vertical Averages Erosion
        public Length AverageDepthErosion_Raw { get { return SafeDivision(ErosionRaw.GetVolume(CellArea, StatsUnits.VertUnit), ErosionRaw.GetArea(CellArea)); } }
        public Length AverageDepthErosion_Thresholded { get { return SafeDivision(ErosionThr.GetVolume(CellArea, StatsUnits.VertUnit), ErosionThr.GetArea(CellArea)); } }
        public Length AverageDepthErosion_Error { get { return SafeDivision(ErosionErr.GetVolume(CellArea, StatsUnits.VertUnit), ErosionThr.GetArea(CellArea)); } }
        public decimal AverageDepthErosion_Percent { get { return 100 * SafeDivision(AverageDepthErosion_Error, AverageDepthErosion_Thresholded); } }

        // Vertical Averages Deposition
        public Length AverageDepthDeposition_Raw { get { return SafeDivision(DepositionRaw.GetVolume(CellArea, StatsUnits.VertUnit), DepositionRaw.GetArea(CellArea)); } }
        public Length AverageDepthDeposition_Thresholded { get { return SafeDivision(DepositionThr.GetVolume(CellArea, StatsUnits.VertUnit), DepositionThr.GetArea(CellArea)); } }
        public Length AverageDepthDeposition_Error { get { return SafeDivision(DepositionErr.GetVolume(CellArea, StatsUnits.VertUnit), DepositionThr.GetArea(CellArea)); } }
        public decimal AverageDepthDeposition_Percent { get { return 100 * SafeDivision(AverageDepthDeposition_Error, AverageDepthDeposition_Thresholded); } }

        // Vertical Averages Total Thickness of Difference for AOI
        public Length AverageThicknessOfDifferenceAOI_Raw { get { return SafeDivision(VolumeOfDifference_Raw, AreaOfInterest_Raw); } }
        public Length AverageThicknessOfDifferenceAOI_Thresholded { get { return SafeDivision(VolumeOfDifference_Thresholded, AreaOfInterest_Raw); } }
        public Length AverageThicknessOfDifferenceAOI_Error { get { return SafeDivision(VolumeOfDifference_Error, AreaOfInterest_Raw); } }
        public decimal AverageThicknessOfDifferenceAOI_Percent { get { return 100 * SafeDivision(AverageThicknessOfDifferenceAOI_Error, AverageThicknessOfDifferenceAOI_Thresholded); } }

        // Vertical Averages **NET** Thickness of Difference for Area of Interest (AOI)
        public Length AverageNetThicknessofDifferenceAOI_Raw { get { return SafeDivision(NetVolumeOfDifference_Raw, AreaOfInterest_Raw); } }
        public Length AverageNetThicknessOfDifferenceAOI_Thresholded { get { return SafeDivision(NetVolumeOfDifference_Thresholded, AreaOfInterest_Raw); } }
        public Length AverageNetThicknessOfDifferenceAOI_Error { get { return SafeDivision(NetVolumeOfDifference_Error, AreaOfInterest_Raw); } }
        public decimal AverageNetThicknessOfDifferenceAOI_Percent { get { return 100 * SafeDivision(AverageNetThicknessOfDifferenceAOI_Error, AverageNetThicknessOfDifferenceAOI_Thresholded); } }

        // Vertical Averages **Total** Thickness of Difference for Area of Detectable Change (ADC)
        public Length AverageThicknessOfDifferenceADC_Thresholded { get { return SafeDivision(VolumeOfDifference_Thresholded, AreaDetectableChange_Thresholded); } }
        public Length AverageThicknessOfDifferenceADC_Error { get { return SafeDivision(VolumeOfDifference_Error, AreaDetectableChange_Thresholded); } }
        public decimal AverageThicknessOfDifferenceADC_Percent { get { return 100 * SafeDivision(AverageThicknessOfDifferenceADC_Error, AverageThicknessOfDifferenceADC_Thresholded); } }

        // Vertical Averages **NET** Thickness of Difference for Area of Detecktable Change (ADC)
        public Length AverageNetThicknessOfDifferenceADC_Thresholded { get { return SafeDivision(NetVolumeOfDifference_Thresholded, AreaDetectableChange_Thresholded); } }
        public Length AverageNetThicknessOfDifferenceADC_Error { get { return SafeDivision(NetVolumeOfDifference_Error, AreaDetectableChange_Thresholded); } }
        public decimal AverageNetThicknessOfDifferenceADC_Percent { get { return 100 * SafeDivision(AverageNetThicknessOfDifferenceADC_Error, AverageNetThicknessOfDifferenceADC_Thresholded); } }

        // Percentages By Volume
        public decimal PercentErosion_Raw { get { return 100 * SafeDivision(ErosionRaw.GetVolume(CellArea, StatsUnits.VertUnit), VolumeOfDifference_Raw); } }
        public decimal PercentErosion_Thresholded { get { return 100 * SafeDivision(ErosionThr.GetVolume(CellArea, StatsUnits.VertUnit), VolumeOfDifference_Thresholded); } }
        public decimal PercentDeposition_Raw { get { return 100 * SafeDivision(DepositionRaw.GetVolume(CellArea, StatsUnits.VertUnit), VolumeOfDifference_Raw); } }
        public decimal PercentDeposition_Thresholded { get { return 100 * SafeDivision(DepositionThr.GetVolume(CellArea, StatsUnits.VertUnit), VolumeOfDifference_Thresholded); } }
        public decimal PercentImbalance_Raw { get { return 100 * SafeDivision(NetVolumeOfDifference_Raw, (2 * VolumeOfDifference_Raw)); } }
        public decimal PercentImbalance_Thresholded { get { return 100 * SafeDivision(NetVolumeOfDifference_Thresholded, (2 * VolumeOfDifference_Thresholded)); } }
        public decimal NetToTotalVolumeRatio_Raw { get { return 100 * SafeDivision(NetVolumeOfDifference_Raw, VolumeOfDifference_Raw); } }
        public decimal NetToTotalVolumeRatio_Thresholded { get { return 100 * SafeDivision(NetVolumeOfDifference_Thresholded, VolumeOfDifference_Thresholded); } }

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
