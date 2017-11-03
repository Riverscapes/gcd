using System;
using UnitsNet.Units;
using UnitsNet;

namespace GCDConsoleLib.GCD
{
    public partial class DoDStats
    {
        // Areal Properties
        public Area AreaDetectableChange_Thresholded { get { return ErosionThr.GetArea(CellArea) + DepositionThr.GetArea(CellArea); } }
        public Area AreaOfInterest_Raw { get { return ErosionRaw.GetArea(CellArea) + DepositionRaw.GetArea(CellArea); } }
        public double AreaPercentAreaInterestWithDetectableChange { get { return 100 * (AreaDetectableChange_Thresholded / AreaOfInterest_Raw); } }

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
                // In order to do all the math on this we need to convert them to doubles and then back into their preferred unit.
                double depErrCubMeter = DepositionErr.GetVolume(CellArea, StatsUnits.VertUnit).CubicMeters;
                double erosionErrCubMeter = ErosionErr.GetVolume(CellArea, StatsUnits.VertUnit).CubicMeters;
                return Volume.FromCubicMeters(Math.Sqrt(Math.Pow(depErrCubMeter, 2) + Math.Pow(erosionErrCubMeter, 2)));
            }
        }

        public double VolumeOfErosion_Percent { get { return SafeDivision(ErosionErr.GetVolume(CellArea, StatsUnits.VertUnit), ErosionThr.GetVolume(CellArea, StatsUnits.VertUnit)) * 100; } }
        public double VolumeOfDeposition_Percent { get { return SafeDivision(DepositionErr.GetVolume(CellArea, StatsUnits.VertUnit), DepositionThr.GetVolume(CellArea, StatsUnits.VertUnit)) * 100; } }

        public double VolumeOfDifference_Percent { get { return SafeDivision(VolumeOfDifference_Error, VolumeOfDifference_Thresholded) * 100; } }
        public double NetVolumeOfDifference_Percent { get { return SafeDivision(NetVolumeOfDifference_Error, NetVolumeOfDifference_Thresholded) * 100; } }

        // Vertical Averages Erosion
        public Length AverageDepthErosion_Raw { get { return SafeDivision(ErosionRaw.GetVolume(CellArea, StatsUnits.VertUnit), ErosionRaw.GetArea(CellArea), StatsUnits.VertUnit); } }
        public Length AverageDepthErosion_Thresholded { get { return SafeDivision(ErosionThr.GetVolume(CellArea, StatsUnits.VertUnit), ErosionThr.GetArea(CellArea), StatsUnits.VertUnit); } }
        public Length AverageDepthErosion_Error { get { return SafeDivision(ErosionErr.GetVolume(CellArea, StatsUnits.VertUnit), ErosionThr.GetArea(CellArea), StatsUnits.VertUnit); } }
        public double AverageDepthErosion_Percent { get { return 100 * SafeDivision(AverageDepthErosion_Error, AverageDepthErosion_Thresholded); } }

        // Vertical Averages Deposition
        public Length AverageDepthDeposition_Raw { get { return SafeDivision(DepositionRaw.GetVolume(CellArea, StatsUnits.VertUnit), DepositionRaw.GetArea(CellArea), StatsUnits.VertUnit); } }
        public Length AverageDepthDeposition_Thresholded { get { return SafeDivision(DepositionThr.GetVolume(CellArea, StatsUnits.VertUnit), DepositionThr.GetArea(CellArea), StatsUnits.VertUnit); } }
        public Length AverageDepthDeposition_Error { get { return SafeDivision(DepositionErr.GetVolume(CellArea, StatsUnits.VertUnit), DepositionThr.GetArea(CellArea), StatsUnits.VertUnit); } }
        public double AverageDepthDeposition_Percent { get { return 100 * SafeDivision(AverageDepthDeposition_Error, AverageDepthDeposition_Thresholded); } }

        // Vertical Averages Total Thickness of Difference for AOI
        public Length AverageThicknessOfDifferenceAOI_Raw { get { return SafeDivision(VolumeOfDifference_Raw, AreaOfInterest_Raw, StatsUnits.VertUnit); } }
        public Length AverageThicknessOfDifferenceAOI_Thresholded { get { return SafeDivision(VolumeOfDifference_Thresholded, AreaOfInterest_Raw, StatsUnits.VertUnit); } }
        public Length AverageThicknessOfDifferenceAOI_Error { get { return SafeDivision(VolumeOfDifference_Error, AreaOfInterest_Raw, StatsUnits.VertUnit); } }
        public double AverageThicknessOfDifferenceAOI_Percent { get { return 100 * SafeDivision(AverageThicknessOfDifferenceAOI_Error, AverageThicknessOfDifferenceAOI_Thresholded); } }

        // Vertical Averages **NET** Thickness of Difference for Area of Interest (AOI)
        public Length AverageNetThicknessofDifferenceAOI_Raw { get { return SafeDivision(NetVolumeOfDifference_Raw, AreaOfInterest_Raw, StatsUnits.VertUnit); } }
        public Length AverageNetThicknessOfDifferenceAOI_Thresholded { get { return SafeDivision(NetVolumeOfDifference_Thresholded, AreaOfInterest_Raw, StatsUnits.VertUnit); } }
        public Length AverageNetThicknessOfDifferenceAOI_Error { get { return SafeDivision(NetVolumeOfDifference_Error, AreaOfInterest_Raw, StatsUnits.VertUnit); } }
        public double AverageNetThicknessOfDifferenceAOI_Percent { get { return 100 * SafeDivision(AverageNetThicknessOfDifferenceAOI_Error, AverageNetThicknessOfDifferenceAOI_Thresholded); } }

        // Vertical Averages **Total** Thickness of Difference for Area of Detectable Change (ADC)
        public Length AverageThicknessOfDifferenceADC_Thresholded { get { return SafeDivision(VolumeOfDifference_Thresholded, AreaDetectableChange_Thresholded, StatsUnits.VertUnit); } }
        public Length AverageThicknessOfDifferenceADC_Error { get { return SafeDivision(VolumeOfDifference_Error, AreaDetectableChange_Thresholded, StatsUnits.VertUnit); } }
        public double AverageThicknessOfDifferenceADC_Percent { get { return 100 * SafeDivision(AverageThicknessOfDifferenceADC_Error, AverageThicknessOfDifferenceADC_Thresholded); } }

        // Vertical Averages **NET** Thickness of Difference for Area of Detecktable Change (ADC)
        public Length AverageNetThicknessOfDifferenceADC_Thresholded { get { return SafeDivision(NetVolumeOfDifference_Thresholded, AreaDetectableChange_Thresholded, StatsUnits.VertUnit); } }
        public Length AverageNetThicknessOfDifferenceADC_Error { get { return SafeDivision(NetVolumeOfDifference_Error, AreaDetectableChange_Thresholded, StatsUnits.VertUnit); } }
        public double AverageNetThicknessOfDifferenceADC_Percent { get { return 100 * SafeDivision(AverageNetThicknessOfDifferenceADC_Error, AverageNetThicknessOfDifferenceADC_Thresholded); } }

        // Percentages By Volume
        public double PercentErosion_Raw { get { return 100 * SafeDivision(ErosionRaw.GetVolume(CellArea, StatsUnits.VertUnit), VolumeOfDifference_Raw); } }
        public double PercentErosion_Thresholded { get { return 100 * SafeDivision(ErosionThr.GetVolume(CellArea, StatsUnits.VertUnit), VolumeOfDifference_Thresholded); } }
        public double PercentDeposition_Raw { get { return 100 * SafeDivision(DepositionRaw.GetVolume(CellArea, StatsUnits.VertUnit), VolumeOfDifference_Raw); } }
        public double PercentDeposition_Thresholded { get { return 100 * SafeDivision(DepositionThr.GetVolume(CellArea, StatsUnits.VertUnit), VolumeOfDifference_Thresholded); } }
        public double PercentImbalance_Raw { get { return 100 * SafeDivision(NetVolumeOfDifference_Raw, (2 * VolumeOfDifference_Raw)); } }
        public double PercentImbalance_Thresholded { get { return 100 * SafeDivision(NetVolumeOfDifference_Thresholded, (2 * VolumeOfDifference_Thresholded)); } }
        public double NetToTotalVolumeRatio_Raw { get { return 100 * SafeDivision(NetVolumeOfDifference_Raw, VolumeOfDifference_Raw); } }
        public double NetToTotalVolumeRatio_Thresholded { get { return 100 * SafeDivision(NetVolumeOfDifference_Thresholded, VolumeOfDifference_Thresholded); } }

        /// <summary>
        /// Return zero if we're ever dividing by zero
        /// </summary>
        /// <param name="fNumerator"></param>
        /// <param name="fDenominator"></param>
        /// <returns></returns>
        private double SafeDivision(double fNumerator, double fDenominator)
        {
            double val = 0;
            if (fDenominator != 0)
                val = fNumerator / fDenominator;
            return val;
        }

        /// <summary>
        /// Dividing two volumes
        /// </summary>
        /// <param name="fNumerator"></param>
        /// <param name="fDenominator"></param>
        /// <returns></returns>
        private double SafeDivision(Volume vNum, Volume vDenom)
        {
            double val = 0;
            double vNummmm = vNum.CubicMeters;
            double vDenommm = vDenom.CubicMeters;
            if (vNummmm != 0)
                val = vNummmm / vDenommm;
            return val;
        }

        /// <summary>
        /// Dividing two lengths
        /// </summary>
        /// <param name="vNum"></param>
        /// <param name="vDenom"></param>
        /// <returns></returns>
        private double SafeDivision(Length vNum, Length vDenom)
        {
            double val = 0;
            double vNummmm = vNum.Meters;
            double vDenommm = vDenom.Meters;
            if (vNummmm != 0)
                val = vNummmm / vDenommm;
            return val;
        }

        /// <summary>
        /// Divide a volume by an area to get a length
        /// </summary>
        /// <param name="vNum"></param>
        /// <param name="vDenom"></param>
        /// <returns></returns>
        private Length SafeDivision(Volume vNum, Area vDenom, LengthUnit lUnit)
        {
            Length val = Length.From(0, lUnit);
            double vNummmm = vNum.CubicMeters;
            double vDenommm = vDenom.SquareMeters;
            if (vNummmm != 0)
                val = Length.From(vNummmm / vDenommm, lUnit);
            return val;
        }

        /// <summary>
        /// Dividing two areas
        /// </summary>
        /// <param name="vNum"></param>
        /// <param name="vDenom"></param>
        /// <returns></returns>
        private double SafeDivision(Area vNum, Area vDenom)
        {
            double val = 0;
            double vNummmm = vNum.SquareMeters;
            double vDenommm = vDenom.SquareMeters;
            if (vNummmm != 0)
                val = vNummmm / vDenommm;
            return val;
        }

    }
}
