Namespace Core.Visualization

    Public Class HistogramDisplayDataPoint

        Private m_elevation As Double
        Private m_erosion As Double
        Private m_deposition As Double
        Private m_raw As Double

        Public ReadOnly Property Elevation As Double
            Get
                Return m_elevation
            End Get
        End Property

        Public Property Deposition As Double
            Get
                Return m_deposition
            End Get
            Set(value As Double)
                m_deposition = value
            End Set
        End Property

        Public Property Erosion As Double
            Get
                Return m_erosion
            End Get
            Set(value As Double)
                m_erosion = value
            End Set
        End Property

        Public Property Raw As Double
            Get
                Return m_raw
            End Get
            Set(value As Double)
                m_raw = value
            End Set
        End Property

        Public Sub New(fElevation As Double)
            m_elevation = fElevation
            m_erosion = 0
            m_deposition = 0
        End Sub

    End Class

End Namespace