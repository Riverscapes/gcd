Imports naru.math.NumberFormatting

Namespace Core.ChangeDetection

    ''' <summary>
    ''' Represents a completed change detection analysis, the raw and thresholded rasters
    ''' </summary>
    ''' <remarks>You cannot create and instance of this class. You have to create one
    ''' of the inherited classes.</remarks>
    Public MustInherit Class ChangeDetectionProperties

        Private m_sRawDoD As String
        Private m_sThresholdedDoD As String
        Private m_eLinearUnits As naru.math.LinearUnitClass
        Private m_fCellSize As Double

#Region "Properties"

        Public ReadOnly Property RawDoD As String
            Get
                Return m_sRawDoD
            End Get
        End Property

        Public ReadOnly Property ThresholdedDoD As String
            Get
                Return m_sThresholdedDoD
            End Get
        End Property

        Public ReadOnly Property CellSize As Double
            Get
                Return m_fCellSize
            End Get
        End Property

        Public ReadOnly Property Units As naru.math.LinearUnitClass
            Get
                Return m_eLinearUnits
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
        Public Sub New(sRawDoD As String, sThresholdedDoD As String, fCellSize As Double, eLinearUnits As naru.math.LinearUnitClass)

            m_sRawDoD = sRawDoD
            m_sThresholdedDoD = sThresholdedDoD
            m_fCellSize = fCellSize
            m_eLinearUnits = eLinearUnits

        End Sub

        ''' <summary>
        ''' Create a new DoD Properties item from a project dataset DoD Row
        ''' </summary>
        ''' <param name="rDoD">The project dataset DoD Row</param>
        ''' <returns>Polymorphic DoD properties object that is initiated using the appropriate
        ''' type (MinLod, Propagated or Probabilistic) depending on the information in the project
        ''' dataset row.</returns>
        ''' <remarks></remarks>
        Public Shared Function CreateFromDoDRow(rDoD As ProjectDS.DoDsRow) As ChangeDetectionProperties

            Dim sRawDoDPath As String = GCDProject.ProjectManager.GetAbsolutePath(rDoD.RawDoDPath)
            Dim sThrDoDPath As String = GCDProject.ProjectManager.GetAbsolutePath(rDoD.ThreshDoDPath)
            Dim gRawDoDPath As New GISDataStructures.Raster(sRawDoDPath)

            Dim dodProps As ChangeDetectionProperties = Nothing
            If rDoD.TypeMinLOD Then
                dodProps = New ChangeDetectionPropertiesMinLoD(sRawDoDPath, sThrDoDPath, rDoD.Threshold, gRawDoDPath.CellSize, gRawDoDPath.LinearUnits)
            Else
                Dim sPropErrPath As String = String.Empty
                If Not rDoD.IsPropagatedErrorRasterPathNull Then
                    sPropErrPath = GCDProject.ProjectManager.GetAbsolutePath(rDoD.PropagatedErrorRasterPath)
                Else
                    Dim ex As New Exception("The DoD project dataset record is missing its propagated error raster.")
                    ex.Data("DoD Name") = rDoD.Name
                    Throw ex
                End If

                If rDoD.TypePropagated Then
                    dodProps = New ChangeDetection.ChangeDetectionPropertiesPropagated(sRawDoDPath, sThrDoDPath, sPropErrPath, gRawDoDPath.CellSize, gRawDoDPath.LinearUnits)
                ElseIf rDoD.TypeProbabilistic Then


                    Dim sProbabilityRaster As String = String.Empty
                    If Not rDoD.IsProbabilityRasterNull Then
                        sProbabilityRaster = rDoD.ProbabilityRaster
                    End If

                    Dim sSpatialCoErosionRaster As String = String.Empty
                    If Not rDoD.IsSpatialCoErosionRasterNull Then
                        sSpatialCoErosionRaster = rDoD.SpatialCoErosionRaster
                    End If

                    Dim sSpatialCoDepositionraster As String = String.Empty
                    If Not rDoD.IsSpatialCoDepositionRasterNull Then
                        sSpatialCoDepositionraster = rDoD.SpatialCoDepositionRaster
                    End If

                    Dim sConditionalProbRaster As String = String.Empty
                    If Not rDoD.IsConditionalProbRasterNull Then
                        sConditionalProbRaster = rDoD.ConditionalProbRaster
                    End If

                    Dim sPosteriorRaster As String = String.Empty
                    If Not rDoD.IsPosteriorRasterNull Then
                        sPosteriorRaster = rDoD.PosteriorRaster
                    End If

                    dodProps = New ChangeDetectionPropertiesProbabilistic(sRawDoDPath, sThrDoDPath, sPropErrPath, sProbabilityRaster, sSpatialCoErosionRaster, sSpatialCoDepositionraster, sConditionalProbRaster, sPosteriorRaster, rDoD.Threshold, rDoD.Filter, rDoD.Bayesian, gRawDoDPath.CellSize, gRawDoDPath.LinearUnits)
                Else
                    Throw New Exception("Unhandled DoD type.")
                End If
            End If

            Return dodProps

        End Function

    End Class


    Public Class ChangeDetectionPropertiesMinLoD
        Inherits ChangeDetectionProperties

        Private m_fThreshold As Double

        Public ReadOnly Property Threshold As Double
            Get
                Return m_fThreshold
            End Get
        End Property

        Public Sub New(sRawDoD As String, sThresholdedDoD As String, fThreshold As Double, fCellSize As Double, eLinearUnits As naru.math.LinearUnitClass)
            MyBase.New(sRawDoD, sThresholdedDoD, fCellSize, eLinearUnits)

            m_fThreshold = fThreshold
        End Sub

    End Class

    Public Class ChangeDetectionPropertiesPropagated
        Inherits ChangeDetectionProperties

        Private m_sPropagatedErrorRaster As String

        Public ReadOnly Property PropagatedErrorRaster As String
            Get
                Return m_sPropagatedErrorRaster
            End Get
        End Property

        Public Sub New(sRawDoD As String, sThresholdedDoD As String, sPropagatedErrorRaster As String, fCellSize As Double, eLinearUnits As naru.math.LinearUnitClass)
            MyBase.New(sRawDoD, sThresholdedDoD, fCellSize, eLinearUnits)

            m_sPropagatedErrorRaster = sPropagatedErrorRaster
        End Sub

    End Class

    Public Class ChangeDetectionPropertiesProbabilistic
        Inherits ChangeDetectionPropertiesPropagated

        Private m_fConfidenceLevel As Double
        'Private m_SpatialCoherenceProperties As CoherenceProperties
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

        'Public ReadOnly Property SpatialCoherenceProperties As CoherenceProperties
        '    Get
        '        Return m_SpatialCoherenceProperties
        '    End Get
        'End Property

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


        Public Sub New(sRawDoD As String, sThresholdedDod As String, sPropagatedErrorRaster As String, sProbabilityRaster As String, sSpatialCoErosionRaster As String, sSpatialCoDepositionRaster As String, sConditionalProbabilityRaster As String, sPosteriorRaster As String, fConfidenceLevel As Double, nFilter As Integer, bBayesianUpdating As Boolean,
                       fCellSize As Double, eLinearUnits As naru.math.LinearUnitClass)
            MyBase.New(sRawDoD, sThresholdedDod, sPropagatedErrorRaster, fCellSize, eLinearUnits)

            m_fConfidenceLevel = fConfidenceLevel
            'm_SpatialCoherenceProperties = SpatialCoherenceProps
            m_nSpatialCoherenceFilter = nFilter
            m_bBayesianUpdating = bBayesianUpdating

            m_sProbabilityRaster = sProbabilityRaster
            m_sSpatialCoErosionRaster = sSpatialCoErosionRaster
            m_sSpatialCoDepositionRaster = sSpatialCoDepositionRaster
            m_sConditionalProbabilityRaster = sConditionalProbabilityRaster
            m_PosteriorRaster = sPosteriorRaster
        End Sub

    End Class

End Namespace