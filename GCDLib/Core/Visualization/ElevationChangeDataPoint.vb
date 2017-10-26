Namespace Core.Visualization

    Public Class ElevationChangeDataPoint

        Private m_fElevation As Double
        Private m_fAreaChange As Double
        Private m_fVolumeChange As Double

        Public ReadOnly Property Elevation As Double
            Get
                Return m_fElevation
            End Get
        End Property

        Public Sub New(fElevation As Double, fArea As Double, fVolume As Double)
            m_fElevation = fElevation
            m_fAreaChange = fArea
            m_fVolumeChange = fVolume
        End Sub

        Public Function AreaChange(dataUnits As UnitsNet.Units.AreaUnit, displayUnits As UnitsNet.Units.AreaUnit) As Double
            Return UnitsNet.Area.From(m_fAreaChange, dataUnits).As(displayUnits)
        End Function

        Public Function VolumeChange(dataUnits As UnitsNet.Units.VolumeUnit, displayUnits As UnitsNet.Units.VolumeUnit) As Double
            Return UnitsNet.Volume.From(m_fVolumeChange, dataUnits).As(displayUnits)
        End Function

        Public Function GetElevation(dataUnits As UnitsNet.Units.LengthUnit, displayUnits As UnitsNet.Units.LengthUnit) As Double
            Return UnitsNet.Length.From(m_fElevation, dataUnits).As(displayUnits)
        End Function

        Public Function AreaDeposition(dataUnits As UnitsNet.Units.AreaUnit, displayUnits As UnitsNet.Units.AreaUnit) As Double

            If m_fElevation > 0 Then
                Return UnitsNet.Area.From(m_fAreaChange, dataUnits).As(displayUnits)
            Else
                Return 0
            End If

        End Function

        Public Function AreaErosion(dataUnits As UnitsNet.Units.AreaUnit, displayUnits As UnitsNet.Units.AreaUnit) As Double
            If m_fElevation < 0 Then
                Return UnitsNet.Area.From(m_fAreaChange, dataUnits).As(displayUnits)
            Else
                Return 0
            End If
        End Function

        Public Function VolumeErosion(dataUnits As UnitsNet.Units.VolumeUnit, displayUnits As UnitsNet.Units.VolumeUnit) As Double
            If m_fElevation < 0 Then
                Return UnitsNet.Volume.From(m_fVolumeChange, dataUnits).As(displayUnits)
            Else
                Return 0
            End If
        End Function

        Public Function VolumeDeposition(dataUnits As UnitsNet.Units.VolumeUnit, displayUnits As UnitsNet.Units.VolumeUnit) As Double
            If m_fElevation > 0 Then
                Return UnitsNet.Volume.From(m_fVolumeChange, dataUnits).As(displayUnits)
            Else
                Return 0
            End If
        End Function

    End Class

End Namespace