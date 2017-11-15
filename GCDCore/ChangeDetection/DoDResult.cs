using System;
using System.IO;
using GCDConsoleLib.GCD;

namespace GCDCore.ChangeDetection
{
    /// <summary>
    /// Represents a completed change detection analysis, the raw and thresholded rasters
    /// </summary>
    /// <remarks>You cannot create an instance of this class. You have to create one
    /// of the inherited classes.</remarks>
    public abstract class DoDResult
    {
        public FileInfo RawDoD { get; internal set; }
        public FileInfo RawHistogram { get; internal set; }
        public FileInfo ThrDoD { get; internal set; }
        public FileInfo ThrHistogram { get; internal set; }
        public UnitsNet.Units.LengthUnit LinearUnits { get; internal set; }
        public DoDStats ChangeStats { get; internal set; }

        public UnitsNet.Units.AreaUnit AreaUnits
        {
            get { return GCDConsoleLib.Utility.Conversion.LengthUnit2AreaUnit(LinearUnits); }
        }

        public UnitsNet.Units.VolumeUnit VolumeUnits
        {
            get { return GCDConsoleLib.Utility.Conversion.LengthUnit2VolumeUnit(LinearUnits); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sRawDoD"></param>
        /// <param name="sThresholdedDoD"></param>
        /// <param name="fCellSize"></param>
        /// <param name="eLinearUnits"></param>
        /// <remarks>PGB 22 Oct 2015 - This constructor used to instantiate a RasterDirect object from the raw
        /// DoD path and then use this to obtain the cell size and linear units. Unfortunately RasterDirect
        /// inherits from Raster and GISDataSource, both of which hold ESRI COM pointer member variables that 
        /// linger when the raster variable is destroyed. (Attempts to make the destructors work properly failed.)
        /// These lingering variables cause problems when budget segregation attempts to delete temporary mask
        /// rasters and the COM objects are still connected. Rather than fix the classes with member COM variables (which are
        /// used throughout all our products) the workaround was to pass these values into the constructor.
        /// Unfortunately this trickles back up to inherited classes, but at least those classes can obtain the
        /// cell size and linear units from the original raw DoD which is never deleted during a budget set loop.
        /// </remarks>
        public DoDResult(DoDStats changeStats, FileInfo rawDoD, FileInfo thrDoD, FileInfo rawHist, FileInfo thrHist, UnitsNet.Units.LengthUnit eLinearUnits)
        {
            ChangeStats = changeStats;
            RawDoD = rawDoD;
            ThrDoD = thrDoD;
            RawHistogram = rawHist;
            ThrHistogram = thrHist;
            LinearUnits = eLinearUnits;
        }

        /// <summary>
        /// Create a new DoD Properties item from a project dataset DoD Row
        /// </summary>
        /// <param name="rDoD">The project dataset DoD Row</param>
        /// <returns>Polymorphic DoD properties object that is initiated using the appropriate
        /// type (MinLod, Propagated or Probabilistic) depending on the information in the project
        /// dataset row.</returns>
        /// <remarks></remarks>
        public static DoDResult CreateFromDoDRow(Project.ProjectDS.DoDsRow rDoD)
        {
            FileInfo rawDoDPath = Project.ProjectManagerBase.GetAbsolutePath(rDoD.RawDoDPath);
            FileInfo rawHistoPath = Project.ProjectManagerBase.GetAbsolutePath(rDoD.RawHistPath);
            FileInfo thrDoDPath = Project.ProjectManagerBase.GetAbsolutePath(rDoD.ThreshDoDPath);
            FileInfo thrHistoPath = Project.ProjectManagerBase.GetAbsolutePath(rDoD.ThreshHistPath);

            GCDConsoleLib.Raster gRawDoDPath = new GCDConsoleLib.Raster(rawDoDPath);
            UnitsNet.Units.LengthUnit lUnits = gRawDoDPath.Proj.HorizontalUnit;
            double cellWidth = Convert.ToDouble(gRawDoDPath.Extent.CellWidth);
            DoDStats changeStats = CreateStatsFromRow(rDoD);

            DoDResult dodResult = null;
            if (rDoD.TypeMinLOD)
            {
                dodResult = new DoDResultMinLoD(ref changeStats, rawDoDPath, rawHistoPath, thrDoDPath, thrHistoPath, rDoD.Threshold, lUnits);
            }
            else
            {
                FileInfo sPropErrPath;
                if (!rDoD.IsPropagatedErrorRasterPathNull())
                {
                    sPropErrPath = Project.ProjectManagerBase.GetAbsolutePath(rDoD.PropagatedErrorRasterPath);
                }
                else
                {
                    Exception ex = new Exception("The DoD project dataset record is missing its propagated error raster.");
                    ex.Data["DoD Name"] = rDoD.Name;
                    throw ex;
                }

                if (rDoD.TypePropagated)
                {
                    dodResult = new DoDResultPropagated(ref changeStats, rawDoDPath, rawHistoPath, thrDoDPath, thrHistoPath, sPropErrPath, lUnits);
                }
                else if (rDoD.TypeProbabilistic)
                {
                    FileInfo sProbabilityRaster = rDoD.IsProbabilityRasterNull() ? null : Project.ProjectManagerBase.GetAbsolutePath(rDoD.ProbabilityRaster);
                    FileInfo sSpatialCoErosionRaster = rDoD.IsSpatialCoErosionRasterNull() ? null : Project.ProjectManagerBase.GetAbsolutePath(rDoD.SpatialCoErosionRaster);
                    FileInfo sSpatialCoDepositionraster = rDoD.IsSpatialCoDepositionRasterNull() ? null : Project.ProjectManagerBase.GetAbsolutePath(rDoD.SpatialCoDepositionRaster);
                    FileInfo sConditionalProbRaster = rDoD.IsConditionalProbRasterNull() ? null : Project.ProjectManagerBase.GetAbsolutePath(rDoD.ConditionalProbRaster);
                    FileInfo sPosteriorRaster = rDoD.IsPosteriorRasterNull() ? null : Project.ProjectManagerBase.GetAbsolutePath(rDoD.PosteriorRaster);

                    dodResult = new DoDResultProbabilisitic(ref changeStats, rawDoDPath, rawHistoPath, thrDoDPath, thrHistoPath, sPropErrPath, sProbabilityRaster, sSpatialCoErosionRaster, sSpatialCoDepositionraster, sConditionalProbRaster,
                    sPosteriorRaster, rDoD.Threshold, rDoD.Filter, rDoD.Bayesian, gRawDoDPath.Proj.HorizontalUnit);
                }
            }

            return dodResult;
        }

        public static DoDStats CreateStatsFromRow(Project.ProjectDS.DoDsRow dodRow)
        {
            UnitsNet.Area areaErosionRaw = UnitsNet.Area.From(dodRow.AreaErosionRaw, Project.ProjectManagerBase.Units.ArUnit);
            UnitsNet.Area areaDepositRaw = UnitsNet.Area.From(dodRow.AreaDepositonRaw, Project.ProjectManagerBase.Units.ArUnit);
            UnitsNet.Area areaErosionThr = UnitsNet.Area.From(dodRow.AreaErosionThresholded, Project.ProjectManagerBase.Units.ArUnit);
            UnitsNet.Area areaDepositThr = UnitsNet.Area.From(dodRow.AreaDepositionThresholded, Project.ProjectManagerBase.Units.ArUnit);

            UnitsNet.Volume volErosionRaw = UnitsNet.Volume.From(dodRow.VolumeErosionRaw, Project.ProjectManagerBase.Units.VolUnit);
            UnitsNet.Volume volDepositRaw = UnitsNet.Volume.From(dodRow.VolumeDepositionRaw, Project.ProjectManagerBase.Units.VolUnit);
            UnitsNet.Volume volErosionThr = UnitsNet.Volume.From(dodRow.VolumeErosionThresholded, Project.ProjectManagerBase.Units.VolUnit);
            UnitsNet.Volume volDepositThr = UnitsNet.Volume.From(dodRow.VolumeDepositionThresholded, Project.ProjectManagerBase.Units.VolUnit);

            UnitsNet.Volume volErosionErr = UnitsNet.Volume.From(dodRow.VolumeErosionError, Project.ProjectManagerBase.Units.VolUnit);
            UnitsNet.Volume volDepositErr = UnitsNet.Volume.From(dodRow.VolumeDepositionError, Project.ProjectManagerBase.Units.VolUnit);

            return new DoDStats(areaErosionRaw, areaDepositRaw,
                areaErosionThr, areaDepositThr,
                volErosionRaw, volDepositRaw,
                volErosionThr, volDepositThr,
                volErosionErr, volDepositErr,
                Project.ProjectManagerBase.CellArea,
                Project.ProjectManagerBase.Units);
        }

        public static DoDStats CreateStatsFromRow(Project.ProjectDS.BSMasksRow dodRow)
        {
            UnitsNet.Area areaErosionRaw = UnitsNet.Area.From(dodRow.AreaErosionRaw, Project.ProjectManagerBase.Units.ArUnit);
            UnitsNet.Area areaDepositRaw = UnitsNet.Area.From(dodRow.AreaDepositionRaw, Project.ProjectManagerBase.Units.ArUnit);
            UnitsNet.Area areaErosionThr = UnitsNet.Area.From(dodRow.AreaErosionThresholded, Project.ProjectManagerBase.Units.ArUnit);
            UnitsNet.Area areaDepositThr = UnitsNet.Area.From(dodRow.AreaDepositionThresholded, Project.ProjectManagerBase.Units.ArUnit);

            UnitsNet.Volume volErosionRaw = UnitsNet.Volume.From(dodRow.VolumeErosionRaw, Project.ProjectManagerBase.Units.VolUnit);
            UnitsNet.Volume volDepositRaw = UnitsNet.Volume.From(dodRow.VolumeDepositionRaw, Project.ProjectManagerBase.Units.VolUnit);
            UnitsNet.Volume volErosionThr = UnitsNet.Volume.From(dodRow.VolumeErosionThresholded, Project.ProjectManagerBase.Units.VolUnit);
            UnitsNet.Volume volDepositThr = UnitsNet.Volume.From(dodRow.VolumeDepositionThresholded, Project.ProjectManagerBase.Units.VolUnit);

            UnitsNet.Volume volErosionErr = UnitsNet.Volume.From(dodRow.VolumeErosionError, Project.ProjectManagerBase.Units.VolUnit);
            UnitsNet.Volume volDepositErr = UnitsNet.Volume.From(dodRow.VolumeDepositionError, Project.ProjectManagerBase.Units.VolUnit);

            return new DoDStats(areaErosionRaw, areaDepositRaw,
                areaErosionThr, areaDepositThr,
                volErosionRaw, volDepositRaw,
                volErosionThr, volDepositThr,
                volErosionErr, volDepositErr,
                Project.ProjectManagerBase.CellArea,
                Project.ProjectManagerBase.Units);
        }
    }

    public class DoDResultMinLoD : DoDResult
    {
        public readonly double Threshold;

        public DoDResultMinLoD(ref DoDStats changeStats, FileInfo rawDoD, FileInfo thrDoD, FileInfo rawHist, FileInfo thrHist, double fThreshold, UnitsNet.Units.LengthUnit eLinearUnits) : base(changeStats, rawDoD, thrDoD, rawHist, thrHist, eLinearUnits)
        {
            Threshold = fThreshold;
        }
    }

    public class DoDResultPropagated : DoDResult
    {
        public readonly FileInfo PropErrRaster;

        public DoDResultPropagated(ref DoDStats changeStats, FileInfo rawDoD, FileInfo rawHisto, FileInfo thrDoD, FileInfo threshHisto, FileInfo propErrorRaster, UnitsNet.Units.LengthUnit eLinearUnits)
            : base(changeStats, rawDoD, rawHisto, thrDoD, threshHisto, eLinearUnits)
        {
            PropErrRaster = propErrorRaster;
        }
    }

    public class DoDResultProbabilisitic : DoDResultPropagated
    {
        public readonly double ConfidenceLevel;
        public readonly int SpatialCoherenceFilter;
        public readonly bool BayesianUpdating;
        public readonly FileInfo ProbabilityRaster;
        public readonly FileInfo SpatialCoErosionRaster;
        public readonly FileInfo SpatialCoDepositionRaster;
        public readonly FileInfo ConditionalProbabilityRaster;
        public readonly FileInfo PosteriorRaster;

        public DoDResultProbabilisitic(ref DoDStats changeStats, FileInfo rawDoD, FileInfo rawHisto, FileInfo thrDoD, FileInfo thrHisto, FileInfo propErrorRaster, FileInfo sProbabilityRaster, FileInfo sSpatialCoErosionRaster, FileInfo sSpatialCoDepositionRaster, FileInfo sConditionalProbabilityRaster,

        FileInfo sPosteriorRaster, double fConfidenceLevel, int nFilter, bool bBayesianUpdating, UnitsNet.Units.LengthUnit eLinearUnits)
            : base(ref changeStats, rawDoD, rawHisto, thrDoD, thrHisto, propErrorRaster, eLinearUnits)
        {
            ConfidenceLevel = fConfidenceLevel;
            SpatialCoherenceFilter = nFilter;
            BayesianUpdating = bBayesianUpdating;

            ProbabilityRaster = sProbabilityRaster;
            SpatialCoErosionRaster = sSpatialCoErosionRaster;
            SpatialCoDepositionRaster = sSpatialCoDepositionRaster;
            ConditionalProbabilityRaster = sConditionalProbabilityRaster;
            PosteriorRaster = sPosteriorRaster;
        }
    }
}