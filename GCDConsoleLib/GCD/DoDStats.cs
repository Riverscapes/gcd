using System;
using UnitsNet.Units;
using UnitsNet;

namespace GCDConsoleLib.GCD
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
    public partial class DoDStats
    {
        public GCDAreaVolume ErosionRaw, DepositionRaw, ErosionThr, DepositionThr, ErosionErr, DepositionErr;

        public readonly UnitGroup StatsUnits;
        public readonly Area CellArea;

        /// <summary>
        /// Initialize a fresh, new obejct with zeros
        /// </summary>
        /// <param name="cellArea"></param>
        /// <param name="vUnit"></param>
        public DoDStats(Area cellArea, UnitGroup sUnits)
        {
            StatsUnits = sUnits;
            CellArea = cellArea;
            ErosionRaw = new GCDAreaVolume();
            DepositionRaw = new GCDAreaVolume();
            ErosionThr = new GCDAreaVolume();
            DepositionThr = new GCDAreaVolume();
            ErosionErr = new GCDAreaVolume();
            DepositionErr = new GCDAreaVolume();
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="oldStats"></param>
        public DoDStats(DoDStats oldStats)
        {
            StatsUnits = oldStats.StatsUnits;
            CellArea = oldStats.CellArea;
            ErosionRaw = new GCDAreaVolume();
            DepositionRaw = new GCDAreaVolume();
            ErosionThr = new GCDAreaVolume();
            DepositionThr = new GCDAreaVolume();
            ErosionErr = new GCDAreaVolume();
            DepositionErr = new GCDAreaVolume();
        }

        /// <summary>
        /// Create this object from nothing more than numbers and units. There are a lot of each.
        /// </summary>
        /// <param name="AreaErosion_Raw"></param>
        /// <param name="AreaDeposition_Raw"></param>
        /// <param name="AreaErosion_Thresholded"></param>
        /// <param name="AreaDeposition_Thresholded"></param>
        /// <param name="VolumeErosion_Raw"></param>
        /// <param name="VolumeDeposition_Raw"></param>
        /// <param name="VolumeErosion_Thresholded"></param>
        /// <param name="VolumeDeposition_Thresholded"></param>
        /// <param name="VolumeErosion_Error"></param>
        /// <param name="VolumeDeposition_Error"></param>
        /// <param name="cellArea"></param>
        /// <param name="sUnits"></param>
        public DoDStats(Area AreaErosion_Raw, Area AreaDeposition_Raw, Area AreaErosion_Thresholded, Area AreaDeposition_Thresholded,
            Volume VolumeErosion_Raw, Volume VolumeDeposition_Raw, Volume VolumeErosion_Thresholded, Volume VolumeDeposition_Thresholded,
            Volume VolumeErosion_Error, Volume VolumeDeposition_Error, 
            Area cellArea, UnitGroup sUnits)
        {
            StatsUnits = sUnits;
            CellArea = cellArea;

            ErosionRaw = new GCDAreaVolume(AreaErosion_Raw, VolumeErosion_Raw, cellArea);
            DepositionRaw = new GCDAreaVolume(AreaDeposition_Raw, VolumeDeposition_Raw, cellArea);

            ErosionThr = new GCDAreaVolume(AreaErosion_Thresholded, VolumeErosion_Thresholded, cellArea);
            DepositionThr = new GCDAreaVolume(AreaDeposition_Thresholded, VolumeDeposition_Thresholded, cellArea);

            // Note that we don't store Area for the error so let's just set it to 0
            ErosionErr = new GCDAreaVolume(Area.FromSquareMeters(0), VolumeErosion_Error, cellArea);
            DepositionErr = new GCDAreaVolume(Area.FromSquareMeters(0), VolumeDeposition_Error, cellArea);
        }

    }
}
