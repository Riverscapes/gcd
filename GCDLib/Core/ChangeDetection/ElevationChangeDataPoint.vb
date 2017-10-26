Namespace Core.ChangeDetection

    Public Class ElevationChangeDataPoint

    Private m_fElevation As UnitsNet.Length
    Private m_fAreaChange As UnitsNet.Area
    Private m_fVolumeChange As UnitsNet.Volume

        Public Property AreaChange As UnitsNet.Area
            Get
                Return m_fAreaChange
            End Get
            Set(value As UnitsNet.Area)
                m_fAreaChange = value
            End Set
        End Property

        Public Property VolumeChange As UnitsNet.Volume
            Get
                Return m_fVolumeChange
            End Get
            Set(value As UnitsNet.Volume)
                m_fVolumeChange = value
            End Set
        End Property

        Public Sub New(fElevation As Double, eLinearUnits As UnitsNet.Units.LengthUnit)
            m_fElevation = UnitsNet.Length.From(fElevation, eLinearUnits)
            m_fAreaChange = New UnitsNet.Area()
            m_fVolumeChange = New UnitsNet.Volume()
        End Sub

#Region "Methods to support elevation change histograms"

        Public Function Elevation(eUnits As UnitsNet.Units.LengthUnit) As Double
        Return m_fElevation.As(eUnits)
    End Function

    Public Function AreaDeposition(eUnits As UnitsNet.Units.AreaUnit) As Double

        If m_fAreaChange.SquareMeters > 0 Then
            Return m_fAreaChange.As(eUnits)
        Else
            Return 0
        End If

    End Function

    Public Function VolumeDepsition(eUnits As UnitsNet.Units.VolumeUnit) As Double

        If m_fVolumeChange.CubicMeters > 0 Then
            Return m_fVolumeChange.As(eUnits)
        Else
            Return 0
        End If

    End Function

    Public Function AreaErosion(eUnits As UnitsNet.Units.AreaUnit) As Double

        If m_fAreaChange.SquareMeters < 0 Then
            Return m_fAreaChange.As(eUnits)
        Else
            Return 0
        End If

    End Function

    Public Function VolumeErosion(eUnits As UnitsNet.Units.VolumeUnit) As Double

        If m_fVolumeChange.CubicMeters < 0 Then
            Return m_fVolumeChange.As(eUnits)
        Else
            Return 0
        End If

    End Function

#End Region

End Class

End Namespace