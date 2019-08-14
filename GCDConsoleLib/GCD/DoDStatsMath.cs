using System;
using UnitsNet;
using GCDConsoleLib.Utility;

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
        public decimal AreaPercentAreaInterestWithDetectableChange { get { return 100 * DynamicMath.SafeDivision(AreaDetectableChange_Thresholded, AreaOfInterest_Raw); } }

        // Volume Properties
        public Volume VolumeOfDifference_Raw { get { return ErosionRaw.GetVolume(CellArea, StatsUnits) + DepositionRaw.GetVolume(CellArea, StatsUnits); } }
        public Volume VolumeOfDifference_Thresholded { get { return ErosionThr.GetVolume(CellArea, StatsUnits) + DepositionThr.GetVolume(CellArea, StatsUnits); } }
        public Volume VolumeOfDifference_Error { get { return ErosionErr.GetVolume(CellArea, StatsUnits) + DepositionErr.GetVolume(CellArea, StatsUnits); } }

        public Volume NetVolumeOfDifference_Raw { get { return DepositionRaw.GetVolume(CellArea, StatsUnits) - ErosionRaw.GetVolume(CellArea, StatsUnits); } }
        public Volume NetVolumeOfDifference_Thresholded { get { return DepositionThr.GetVolume(CellArea, StatsUnits) - ErosionThr.GetVolume(CellArea, StatsUnits); } }
        public Volume NetVolumeOfDifference_Error
        {
            get
            {
                // In order to do all the math on this we need to convert them to decimals and then back into their preferred unit.
                decimal depErrCubMeter = (decimal)DepositionErr.GetVolume(CellArea, StatsUnits).CubicMeters;
                decimal erosionErrCubMeter = (decimal)ErosionErr.GetVolume(CellArea, StatsUnits).CubicMeters;
                return Volume.FromCubicMeters(Math.Sqrt(Math.Pow((double)depErrCubMeter, 2) + Math.Pow((double)erosionErrCubMeter, 2)));
            }
        }

        public decimal VolumeOfErosion_Percent { get { return DynamicMath.SafeDivision(ErosionErr.GetVolume(CellArea, StatsUnits), ErosionThr.GetVolume(CellArea, StatsUnits)) * 100; } }
        public decimal VolumeOfDeposition_Percent { get { return DynamicMath.SafeDivision(DepositionErr.GetVolume(CellArea, StatsUnits), DepositionThr.GetVolume(CellArea, StatsUnits)) * 100; } }

        public decimal VolumeOfDifference_Percent { get { return DynamicMath.SafeDivision(VolumeOfDifference_Error, VolumeOfDifference_Thresholded) * 100; } }
        public decimal NetVolumeOfDifference_Percent { get { return DynamicMath.SafeDivision(NetVolumeOfDifference_Error, NetVolumeOfDifference_Thresholded) * 100; } }

        // Vertical Averages Erosion
        public Length AverageDepthErosion_Raw { get { return DynamicMath.SafeDivision(ErosionRaw.GetVolume(CellArea, StatsUnits), ErosionRaw.GetArea(CellArea)); } }
        public Length AverageDepthErosion_Thresholded { get { return DynamicMath.SafeDivision(ErosionThr.GetVolume(CellArea, StatsUnits), ErosionThr.GetArea(CellArea)); } }
        public Length AverageDepthErosion_Error { get { return DynamicMath.SafeDivision(ErosionErr.GetVolume(CellArea, StatsUnits), ErosionThr.GetArea(CellArea)); } }
        public decimal AverageDepthErosion_Percent { get { return 100 * DynamicMath.SafeDivision(AverageDepthErosion_Error, AverageDepthErosion_Thresholded); } }

        // Vertical Averages Deposition
        public Length AverageDepthDeposition_Raw { get { return DynamicMath.SafeDivision(DepositionRaw.GetVolume(CellArea, StatsUnits), DepositionRaw.GetArea(CellArea)); } }
        public Length AverageDepthDeposition_Thresholded { get { return DynamicMath.SafeDivision(DepositionThr.GetVolume(CellArea, StatsUnits), DepositionThr.GetArea(CellArea)); } }
        public Length AverageDepthDeposition_Error { get { return DynamicMath.SafeDivision(DepositionErr.GetVolume(CellArea, StatsUnits), DepositionThr.GetArea(CellArea)); } }
        public decimal AverageDepthDeposition_Percent { get { return 100 * DynamicMath.SafeDivision(AverageDepthDeposition_Error, AverageDepthDeposition_Thresholded); } }

        // Vertical Averages Total Thickness of Difference for AOI
        public Length AverageThicknessOfDifferenceAOI_Raw { get { return DynamicMath.SafeDivision(VolumeOfDifference_Raw, AreaOfInterest_Raw); } }
        public Length AverageThicknessOfDifferenceAOI_Thresholded { get { return DynamicMath.SafeDivision(VolumeOfDifference_Thresholded, AreaOfInterest_Raw); } }
        public Length AverageThicknessOfDifferenceAOI_Error { get { return DynamicMath.SafeDivision(VolumeOfDifference_Error, AreaOfInterest_Raw); } }
        public decimal AverageThicknessOfDifferenceAOI_Percent { get { return 100 * DynamicMath.SafeDivision(AverageThicknessOfDifferenceAOI_Error, AverageThicknessOfDifferenceAOI_Thresholded); } }

        // Vertical Averages **NET** Thickness of Difference for Area of Interest (AOI)
        public Length AverageNetThicknessofDifferenceAOI_Raw { get { return DynamicMath.SafeDivision(NetVolumeOfDifference_Raw, AreaOfInterest_Raw); } }
        public Length AverageNetThicknessOfDifferenceAOI_Thresholded { get { return DynamicMath.SafeDivision(NetVolumeOfDifference_Thresholded, AreaOfInterest_Raw); } }
        public Length AverageNetThicknessOfDifferenceAOI_Error { get { return DynamicMath.SafeDivision(NetVolumeOfDifference_Error, AreaOfInterest_Raw); } }
        public decimal AverageNetThicknessOfDifferenceAOI_Percent { get { return 100 * DynamicMath.SafeDivision(AverageNetThicknessOfDifferenceAOI_Error, AverageNetThicknessOfDifferenceAOI_Thresholded); } }

        // Vertical Averages **Total** Thickness of Difference for Area of Detectable Change (ADC)
        public Length AverageThicknessOfDifferenceADC_Thresholded { get { return DynamicMath.SafeDivision(VolumeOfDifference_Thresholded, AreaDetectableChange_Thresholded); } }
        public Length AverageThicknessOfDifferenceADC_Error { get { return DynamicMath.SafeDivision(VolumeOfDifference_Error, AreaDetectableChange_Thresholded); } }
        public decimal AverageThicknessOfDifferenceADC_Percent { get { return 100 * DynamicMath.SafeDivision(AverageThicknessOfDifferenceADC_Error, AverageThicknessOfDifferenceADC_Thresholded); } }

        // Vertical Averages **NET** Thickness of Difference for Area of Detecktable Change (ADC)
        public Length AverageNetThicknessOfDifferenceADC_Thresholded { get { return DynamicMath.SafeDivision(NetVolumeOfDifference_Thresholded, AreaDetectableChange_Thresholded); } }
        public Length AverageNetThicknessOfDifferenceADC_Error { get { return DynamicMath.SafeDivision(NetVolumeOfDifference_Error, AreaDetectableChange_Thresholded); } }
        public decimal AverageNetThicknessOfDifferenceADC_Percent { get { return 100 * DynamicMath.SafeDivision(AverageNetThicknessOfDifferenceADC_Error, AverageNetThicknessOfDifferenceADC_Thresholded); } }

        // Percentages By Volume
        public decimal PercentErosion_Raw { get { return 100 * DynamicMath.SafeDivision(ErosionRaw.GetVolume(CellArea, StatsUnits), VolumeOfDifference_Raw); } }
        public decimal PercentErosion_Thresholded { get { return 100 * DynamicMath.SafeDivision(ErosionThr.GetVolume(CellArea, StatsUnits), VolumeOfDifference_Thresholded); } }
        public decimal PercentDeposition_Raw { get { return 100 * DynamicMath.SafeDivision(DepositionRaw.GetVolume(CellArea, StatsUnits), VolumeOfDifference_Raw); } }
        public decimal PercentDeposition_Thresholded { get { return 100 * DynamicMath.SafeDivision(DepositionThr.GetVolume(CellArea, StatsUnits), VolumeOfDifference_Thresholded); } }
        public decimal PercentImbalance_Raw { get { return 100 * DynamicMath.SafeDivision(NetVolumeOfDifference_Raw, (2 * VolumeOfDifference_Raw)); } }
        public decimal PercentImbalance_Thresholded { get { return 100 * DynamicMath.SafeDivision(NetVolumeOfDifference_Thresholded, (2 * VolumeOfDifference_Thresholded)); } }
        public decimal NetToTotalVolumeRatio_Raw { get { return 100 * DynamicMath.SafeDivision(NetVolumeOfDifference_Raw, VolumeOfDifference_Raw); } }
        public decimal NetToTotalVolumeRatio_Thresholded { get { return 100 * DynamicMath.SafeDivision(NetVolumeOfDifference_Thresholded, VolumeOfDifference_Thresholded); } }

    }
}
