Namespace Core.ChangeDetection

    ''' <summary>
    ''' Represents a completed change detection analysis, the raw and thresholded rasters
    ''' </summary>
    ''' <remarks>You cannot create an instance of this class. You have to create one
    ''' of the inherited classes.</remarks>
    Public MustInherit Class DoDResult

        Private m_RawDoD As IO.FileInfo
        Private m_RawHistogram As IO.FileInfo

        Private m_ThrDoD As IO.FileInfo
        Private m_ThrHistogram As IO.FileInfo

        Private m_eLinearUnits As UnitsNet.Units.LengthUnit
        Private m_fCellSize As Double
        Protected m_ChangeStats As GCDConsoleLib.DoDStats

#Region "Properties"

        Public ReadOnly Property RawDoD As IO.FileInfo
            Get
                Return m_RawDoD
            End Get
        End Property

        Public ReadOnly Property RawHistogram As IO.FileInfo
            Get
                Return m_RawHistogram
            End Get
        End Property

        Public ReadOnly Property ThresholdedDoD As IO.FileInfo
            Get
                Return m_ThrDoD
            End Get
        End Property

        Public ReadOnly Property ThresholdedHistogram As IO.FileInfo
            Get
                Return m_ThrHistogram
            End Get
        End Property

        Public ReadOnly Property CellSize As Double
            Get
                Return m_fCellSize
            End Get
        End Property

        Public ReadOnly Property Units As UnitsNet.Units.LengthUnit
            Get
                Return m_eLinearUnits
            End Get
        End Property

        Public ReadOnly Property AreaUnits As UnitsNet.Units.AreaUnit
            Get
                Return GCDConsoleLib.Utility.Conversion.LengthUnit2AreaUnit(Units)
            End Get
        End Property

        Public ReadOnly Property VolumeUnits As UnitsNet.Units.VolumeUnit
            Get
                Return GCDConsoleLib.Utility.Conversion.LengthUnit2VolumeUnit(Units)
            End Get
        End Property

        Public ReadOnly Property ChangeStats As GCDConsoleLib.DoDStats
            Get
                Return m_ChangeStats
            End Get
        End Property

#End Region

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sRawDoD"></param>
        ''' <param name="sThresholdedDoD"></param>
        ''' <param name="fCellSize"></param>
        ''' <param name="eLinearUnits"></param>
        ''' <remarks>PGB 22 Oct 2015 - This constructor used to instantiate a RasterDirect object from the raw
        ''' DoD path and then use this to obtain the cell size and linear units. Unfortunately RasterDirect
        ''' inherits from Raster and GISDataSource, both of which hold ESRI COM pointer member variables that 
        ''' linger when the raster variable is destroyed. (Attempts to make the destructors work properly failed.)
        ''' These lingering variables cause problems when budget segregation attempts to delete temporary mask
        ''' rasters and the COM objects are still connected. Rather than fix the classes with member COM variables (which are
        ''' used throughout all our products) the workaround was to pass these values into the constructor.
        ''' Unfortunately this trickles back up to inherited classes, but at least those classes can obtain the
        ''' cell size and linear units from the original raw DoD which is never deleted during a budget set loop.
        ''' </remarks>
        Public Sub New(sRawDoD As String, sRawHistogram As String, sThresholdedDoD As String, sThreshHistogram As String, fCellSize As Double, eLinearUnits As UnitsNet.Units.LengthUnit)

            m_RawDoD = New IO.FileInfo(sRawDoD)
            m_RawHistogram = New IO.FileInfo(sRawHistogram)

            m_ThrDoD = New IO.FileInfo(sThresholdedDoD)
            m_ThrHistogram = New IO.FileInfo(sThreshHistogram)

            m_fCellSize = fCellSize
            m_eLinearUnits = eLinearUnits

            Throw New NotImplementedException("need to load change stats here")

        End Sub

        ''' <summary>
        ''' Create a new DoD Properties item from a project dataset DoD Row
        ''' </summary>
        ''' <param name="rDoD">The project dataset DoD Row</param>
        ''' <returns>Polymorphic DoD properties object that is initiated using the appropriate
        ''' type (MinLod, Propagated or Probabilistic) depending on the information in the project
        ''' dataset row.</returns>
        ''' <remarks></remarks>
        Public Shared Function CreateFromDoDRow(rDoD As ProjectDS.DoDsRow) As DoDResult

            Dim rawDoDPath As String = GCDProject.ProjectManagerBase.GetAbsolutePath(rDoD.RawDoDPath)
            Dim rawHistoPath As String = GCDProject.ProjectManagerBase.GetAbsolutePath(rDoD.RawHistPath)
            Dim thrDoDPath As String = GCDProject.ProjectManagerBase.GetAbsolutePath(rDoD.ThreshDoDPath)
            Dim thrHistoPath As String = GCDProject.ProjectManagerBase.GetAbsolutePath(rDoD.ThreshHistPath)

            Dim gRawDoDPath As New GCDConsoleLib.Raster(rawDoDPath)
            Dim lUnits As UnitsNet.Units.LengthUnit = gRawDoDPath.Proj.LinearUnit
            Dim cellWidth As Double = Convert.ToDouble(gRawDoDPath.Extent.CellWidth)

            Dim dodProps As DoDResult = Nothing
            If rDoD.TypeMinLOD Then
                dodProps = New DoDResultMinLoD(rawDoDPath, rawHistoPath, thrDoDPath, thrHistoPath, rDoD.Threshold, cellWidth, lUnits)
            Else
                Dim sPropErrPath As String = String.Empty
                If Not rDoD.IsPropagatedErrorRasterPathNull Then
                    sPropErrPath = GCDProject.ProjectManagerBase.GetAbsolutePath(rDoD.PropagatedErrorRasterPath)
                Else
                    Dim ex As New Exception("The DoD project dataset record is missing its propagated error raster.")
                    ex.Data("DoD Name") = rDoD.Name
                    Throw ex
                End If

                If rDoD.TypePropagated Then
                    dodProps = New DoDResultPropagated(rawDoDPath, rawHistoPath, thrDoDPath, thrHistoPath, sPropErrPath, cellWidth, lUnits)

                ElseIf rDoD.TypeProbabilistic Then

                    Dim sProbabilityRaster As String = If(rDoD.IsProbabilityRasterNull, String.Empty, rDoD.ProbabilityRaster)
                    Dim sSpatialCoErosionRaster As String = If(rDoD.IsSpatialCoErosionRasterNull, String.Empty, rDoD.SpatialCoErosionRaster)
                    Dim sSpatialCoDepositionraster As String = If(rDoD.IsSpatialCoDepositionRasterNull, String.Empty, rDoD.SpatialCoDepositionRaster)
                    Dim sConditionalProbRaster As String = If(rDoD.IsConditionalProbRasterNull, String.Empty, rDoD.ConditionalProbRaster)
                    Dim sPosteriorRaster As String = If(rDoD.IsPosteriorRasterNull, String.Empty, rDoD.PosteriorRaster)

                    dodProps = New DoDResultProbabilisitic(rawDoDPath, rawHistoPath, thrDoDPath, thrHistoPath, sPropErrPath, sProbabilityRaster, sSpatialCoErosionRaster, sSpatialCoDepositionraster, sConditionalProbRaster, sPosteriorRaster, rDoD.Threshold, rDoD.Filter, rDoD.Bayesian, gRawDoDPath.Extent.CellWidth, gRawDoDPath.Proj.LinearUnit)
                Else
                    Throw New Exception("Unhandled DoD type.")
                End If
            End If

            Return dodProps

        End Function

    End Class

    Public Class DoDResultMinLoD
        Inherits DoDResult

        Private m_fThreshold As Double

        Public ReadOnly Property Threshold As Double
            Get
                Return m_fThreshold
            End Get
        End Property

        Public Sub New(rawDoD As String, rawHisto As String, thrDoD As String, threshHisto As String, fThreshold As Double, fCellSize As Double, eLinearUnits As UnitsNet.Units.LengthUnit)
            MyBase.New(rawDoD, rawHisto, thrDoD, threshHisto, fCellSize, eLinearUnits)

            m_fThreshold = fThreshold

            ' Build the change statistics
            m_ChangeStats = GCDConsoleLib.RasterOperators.GetStatsMinLoD(rawDoD, thrDoD, fThreshold)
        End Sub

    End Class

    Public Class DoDResultPropagated
        Inherits DoDResult

        Private m_sPropagatedErrorRaster As String

        Public ReadOnly Property PropagatedErrorRaster As String
            Get
                Return m_sPropagatedErrorRaster
            End Get
        End Property

        Public Sub New(rawDoD As String, rawHisto As String, thrDoD As String, threshHisto As String, sPropErrorRaster As String, fCellSize As Double, eLinearUnits As UnitsNet.Units.LengthUnit)
            MyBase.New(rawDoD, rawHisto, thrDoD, threshHisto, fCellSize, eLinearUnits)

            m_sPropagatedErrorRaster = sPropErrorRaster

            ' Build the change statistics
            m_ChangeStats = GCDConsoleLib.RasterOperators.GetStatsPropagated(rawDoD, thrDoD, sPropErrorRaster)
        End Sub

    End Class

    Public Class DoDResultProbabilisitic
        Inherits DoDResultPropagated

        Private m_fConfidenceLevel As Double
        Private m_nSpatialCoherenceFilter As Integer
        Private m_bBayesianUpdating As Boolean

        Private m_sProbabilityRaster As String
        Private m_sSpatialCoErosionRaster As String
        Private m_sSpatialCoDepositionRaster As String
        Private m_sConditionalProbabilityRaster As String
        Private m_PosteriorRaster As String

        Public ReadOnly Property ConfidenceLevel As Double
            Get
                Return m_fConfidenceLevel
            End Get
        End Property

        Public ReadOnly Property SpatialCoherenceFilter As Integer
            Get
                Return m_nSpatialCoherenceFilter
            End Get
        End Property

        Public ReadOnly Property BayesianUpdating As Boolean
            Get
                Return m_bBayesianUpdating
            End Get
        End Property

        Public ReadOnly Property ProbabilityRaster As String
            Get
                Return m_sProbabilityRaster
            End Get
        End Property

        Public ReadOnly Property SpatialCoErosionRaster As String
            Get
                Return m_sSpatialCoErosionRaster
            End Get
        End Property

        Public ReadOnly Property SpatialCoDepositionRaster As String
            Get
                Return m_sSpatialCoDepositionRaster
            End Get
        End Property

        Public ReadOnly Property ConditionalRaster As String
            Get
                Return m_sConditionalProbabilityRaster
            End Get
        End Property

        Public ReadOnly Property PosteriorRaster As String
            Get
                Return m_PosteriorRaster
            End Get
        End Property

        Public Sub New(rawDoD As String, rawHisto As String, thrDoD As String, thrHisto As String, propErrorRaster As String, sProbabilityRaster As String, sSpatialCoErosionRaster As String, sSpatialCoDepositionRaster As String, sConditionalProbabilityRaster As String, sPosteriorRaster As String, fConfidenceLevel As Double, nFilter As Integer, bBayesianUpdating As Boolean,
                       fCellSize As Double, eLinearUnits As UnitsNet.Units.LengthUnit)
            MyBase.New(rawDoD, rawHisto, thrDoD, thrHisto, propErrorRaster, fCellSize, eLinearUnits)

            m_fConfidenceLevel = fConfidenceLevel
            m_nSpatialCoherenceFilter = nFilter
            m_bBayesianUpdating = bBayesianUpdating

            m_sProbabilityRaster = sProbabilityRaster
            m_sSpatialCoErosionRaster = sSpatialCoErosionRaster
            m_sSpatialCoDepositionRaster = sSpatialCoDepositionRaster
            m_sConditionalProbabilityRaster = sConditionalProbabilityRaster
            m_PosteriorRaster = sPosteriorRaster

            ' Build the change statistics
            m_ChangeStats = GCDConsoleLib.RasterOperators.GetStatsProbalistic(rawDoD, thrDoD, propErrorRaster)
        End Sub

    End Class

End Namespace