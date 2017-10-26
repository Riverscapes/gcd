using System;

namespace GCDConsoleLib
{
    /// <summary>
    /// Represents the summary statistics of a DoD analysis.
    /// There are 10 core values that are retrieved from the raw and
    /// thresholded DoD rasters. The method by which these core values
    /// is retrieved depends on the thresholding method used to produce
    /// the threshold raster.
    /// 
    /// The remaining values are all derived from these 10 core values.
    /// </summary>
    public class DoDStats
    {
        public readonly double CellArea;

        public readonly double AreaErosion_Raw;
        public readonly double AreaDeposition_Raw;
        public readonly double VolumeErosion_Raw;
        public readonly double VolumeDeposition_Raw;

        // The following members need to be writeable so that budget segregation
        // can accumulate the total stats across all classes
        public double AreaErosion_Thresholded { get; set; }
        public double AreaDeposition_Thresholded { get; set; }
        public double VolumeErosion_Thresholded { get; set; }
        public double VolumeDeposition_Thresholded { get; set; }

        public readonly double VolumeErosion_Error;
        public readonly double VolumeDeposition_Error;

        #region Derived Property Values

        // Areal Properties
        public double AreaDetectableChange_Thresholded { get { return (AreaErosion_Thresholded + AreaDeposition_Thresholded); } }
        public double AreaOfInterest_Raw { get { return (AreaErosion_Raw + AreaDeposition_Raw); } }
        public double AreaPercentAreaInterestWithDetectableChange { get { return 100 * (AreaDetectableChange_Thresholded / AreaOfInterest_Raw); } }

        // Volume Properties
        public double VolumeOfDifference_Raw { get { return VolumeErosion_Raw + VolumeDeposition_Raw; } }
        public double VolumeOfDifference_Thresholded { get { return VolumeErosion_Thresholded + VolumeDeposition_Thresholded; } }
        public double VolumeOfDifference_Error { get { return (VolumeDeposition_Error + VolumeErosion_Error); } }

        public double NetVolumeOfDifference_Raw { get { return (VolumeDeposition_Raw - VolumeErosion_Raw); } }
        public double NetVolumeOfDifference_Thresholded { get { return (VolumeDeposition_Thresholded - VolumeErosion_Thresholded); } }
        public double NetVolumeOfDifference_Error { get { return Math.Sqrt(Math.Pow(VolumeDeposition_Error, 2) + Math.Pow(VolumeErosion_Error, 2)); } }

        public double VolumeOfErosion_Percent
        {
            get
            {
                double fResult = 0;
                if (VolumeErosion_Error != 0 && VolumeErosion_Thresholded != 0)
                {
                    fResult = 100 * (VolumeErosion_Error / VolumeErosion_Thresholded);
                }
                return fResult;
            }
        }

        public double VolumeOfDeposition_Percent
        {
            get
            {
                double fResult = 0;
                if (VolumeDeposition_Error != 0 && VolumeDeposition_Thresholded != 0)
                {
                    fResult = 100 * (VolumeDeposition_Error / VolumeDeposition_Thresholded);
                }
                return fResult;
            }
        }

        public double VolumeOfDifference_Percent
        {
            get
            {
                double fResult = 0;
                if (VolumeOfDifference_Error != 0 && VolumeOfDifference_Thresholded != 0)
                {
                    fResult = 100 * (VolumeOfDifference_Error / VolumeOfDifference_Thresholded);
                }
                return fResult;
            }
        }

        public double NetVolumeOfDifference_Percent
        {
            get
            {
                double fResult = 0;
                if (NetVolumeOfDifference_Error != 0 && NetVolumeOfDifference_Thresholded != 0)
                {
                    fResult = 100 * (NetVolumeOfDifference_Error / NetVolumeOfDifference_Thresholded);
                }
                return fResult;
            }
        }

        // Vertical Averages Erosion
        public double AverageDepthErosion_Raw { get { return SafeDivision(VolumeErosion_Raw, AreaErosion_Raw); } }
        public double AverageDepthErosion_Thresholded { get { return SafeDivision(VolumeErosion_Thresholded, AreaErosion_Thresholded); } }
        public double AverageDepthErosion_Error { get { return SafeDivision(VolumeErosion_Error, AreaErosion_Thresholded); } }
        public double AverageDepthErosion_Percent { get { return 100 * SafeDivision(AverageDepthErosion_Error, AverageDepthErosion_Thresholded); } }

        // Vertical Averages Deposition
        public double AverageDepthDeposition_Raw { get { return SafeDivision(VolumeDeposition_Raw, AreaDeposition_Raw); } }
        public double AverageDepthDeposition_Thresholded { get { return SafeDivision(VolumeDeposition_Thresholded, AreaDeposition_Thresholded); } }
        public double AverageDepthDeposition_Error { get { return SafeDivision(VolumeDeposition_Error, AreaDeposition_Thresholded); } }
        public double AverageDepthDeposition_Percent { get { return 100 * SafeDivision(AverageDepthDeposition_Error, AverageDepthDeposition_Thresholded); } }

        // Vertical Averages Total Thickness of Difference for AOI
        public double AverageThicknessOfDifferenceAOI_Raw { get { return SafeDivision(VolumeOfDifference_Raw, AreaOfInterest_Raw); } }
        public double AverageThicknessOfDifferenceAOI_Thresholded { get { return SafeDivision(VolumeOfDifference_Thresholded, AreaOfInterest_Raw); } }
        public double AverageThicknessOfDifferenceAOI_Error { get { return SafeDivision(VolumeOfDifference_Error, AreaOfInterest_Raw); } }
        public double AverageThicknessOfDifferenceAOI_Percent { get { return 100 * SafeDivision(AverageThicknessOfDifferenceAOI_Error, AverageThicknessOfDifferenceAOI_Thresholded); } }

        // Vertical Averages **NET** Thickness of Difference for Area of Interest (AOI)
        public double AverageNetThicknessofDifferenceAOI_Raw { get { return SafeDivision(NetVolumeOfDifference_Raw, AreaOfInterest_Raw); } }
        public double AverageNetThicknessOfDifferenceAOI_Thresholded { get { return SafeDivision(NetVolumeOfDifference_Thresholded, AreaOfInterest_Raw); } }
        public double AverageNetThicknessOfDifferenceAOI_Error { get { return SafeDivision(NetVolumeOfDifference_Error, AreaOfInterest_Raw); } }
        public double AverageNetThicknessOfDifferenceAOI_Percent { get { return 100 * SafeDivision(AverageNetThicknessOfDifferenceAOI_Error, AverageNetThicknessOfDifferenceAOI_Thresholded); } }

        // Vertical Averages **Total** Thickness of Difference for Area of Detectable Change (ADC)
        public double AverageThicknessOfDifferenceADC_Thresholded { get { return SafeDivision(VolumeOfDifference_Thresholded, AreaDetectableChange_Thresholded); } }
        public double AverageThicknessOfDifferenceADC_Error { get { return SafeDivision(VolumeOfDifference_Error, AreaDetectableChange_Thresholded); } }
        public double AverageThicknessOfDifferenceADC_Percent { get { return 100 * SafeDivision(AverageThicknessOfDifferenceADC_Error, AverageThicknessOfDifferenceADC_Thresholded); } }

        // Vertical Averages **NET** Thickness of Difference for Area of Detecktable Change (ADC)
        public double AverageNetThicknessOfDifferenceADC_Thresholded { get { return SafeDivision(NetVolumeOfDifference_Thresholded, AreaDetectableChange_Thresholded); } }
        public double AverageNetThicknessOfDifferenceADC_Error { get { return SafeDivision(NetVolumeOfDifference_Error, AreaDetectableChange_Thresholded); } }
        public double AverageNetThicknessOfDifferenceADC_Percent { get { return 100 * SafeDivision(AverageNetThicknessOfDifferenceADC_Error, AverageNetThicknessOfDifferenceADC_Thresholded); } }

        // Percentages By Volume
        public double PercentErosion_Raw { get { return 100 * SafeDivision(VolumeErosion_Raw, VolumeOfDifference_Raw); } }
        public double PercentErosion_Thresholded { get { return 100 * SafeDivision(VolumeErosion_Thresholded, VolumeOfDifference_Thresholded); } }
        public double PercentDeposition_Raw { get { return 100 * SafeDivision(VolumeDeposition_Raw, VolumeOfDifference_Raw); } }
        public double PercentDeposition_Thresholded { get { return 100 * SafeDivision(VolumeDeposition_Thresholded, VolumeOfDifference_Thresholded); } }
        public double PercentImbalance_Raw { get { return 100 * SafeDivision(NetVolumeOfDifference_Raw, (2 * VolumeOfDifference_Raw)); } }
        public double PercentImbalance_Thresholded { get { return 100 * SafeDivision(NetVolumeOfDifference_Thresholded, (2 * VolumeOfDifference_Thresholded)); } }
        public double NetToTotalVolumeRatio_Raw { get { return 100 * SafeDivision(NetVolumeOfDifference_Raw, VolumeOfDifference_Raw); } }
        public double NetToTotalVolumeRatio_Thresholded { get { return 100 * SafeDivision(NetVolumeOfDifference_Thresholded, VolumeOfDifference_Thresholded); } }

        private double SafeDivision(double fNumerator, double fDenominator)
        {
            double fResult = 0;
            if (fNumerator != 0 && fDenominator != 0)
            {
                fResult = fNumerator / fDenominator;
            }
            return fResult;
        }

        #endregion

        public DoDStats(double fCellArea, double fAreaErosion_Raw, double fAreaDeposition_Raw, double fAreaErosion_Thresholded, double fAreaDeposition_Thresholded,
            double fVolumeErosion_Raw, double fVolumeDeposition_Raw, double fVolumeErosion_Thresholded, double fVolumeDeposition_Thresholded,
            double fVolumeErosion_Error, double fVolumeDeposition_Error)
        {
            CellArea = fCellArea;

            AreaErosion_Raw = fAreaErosion_Raw;
            AreaDeposition_Raw = fAreaDeposition_Raw;
            AreaErosion_Thresholded = fAreaErosion_Thresholded;
            AreaDeposition_Thresholded = fAreaDeposition_Thresholded;

            VolumeErosion_Raw = fVolumeErosion_Raw;
            VolumeDeposition_Raw = fVolumeDeposition_Raw;
            VolumeErosion_Thresholded = fVolumeErosion_Thresholded;
            VolumeDeposition_Thresholded = fVolumeDeposition_Thresholded;

            VolumeErosion_Error = fVolumeErosion_Error;
            VolumeDeposition_Error = fVolumeDeposition_Error;
        }
    }
}
