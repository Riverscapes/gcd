Namespace GISCode.GCD.ChangeDetection

    Public Class CoherenceProperties

        Private m_nMovingWindowDimensions As Integer
        Private m_nInflectionA As Integer
        Private m_nInflectionB As Integer

#Region "Properties"
        Public Property MovingWindowDimensions As Integer
            Get
                Return m_nMovingWindowDimensions
            End Get
            Set(value As Integer)
                If value > 0 Then
                    m_nMovingWindowDimensions = value
                End If
            End Set
        End Property

        Public ReadOnly Property InflectionA As Integer
            Get
                Return m_nInflectionA
            End Get
        End Property

        Public ReadOnly Property InflectionB As Integer
            Get
                Return m_nInflectionB
            End Get
        End Property

        Public ReadOnly Property MovingWindowWidth As Integer
            Get
                Return m_nMovingWindowDimensions
            End Get
        End Property

        Public ReadOnly Property MovingWindowHeight As Integer
            Get
                Return m_nMovingWindowDimensions
            End Get
        End Property

        Public ReadOnly Property XMin As Integer
            Get
                Dim fResult As Double = Math.Floor((MovingWindowWidth * MovingWindowHeight) * (m_nInflectionA / 100))
                Return fResult
            End Get
        End Property

        Public ReadOnly Property XMax As Integer
            Get
                Dim fResult As Double = Math.Floor((MovingWindowWidth * MovingWindowHeight) * (m_nInflectionB / 100))
                Return fResult
            End Get
        End Property

#End Region

        Public Sub New(nMovingWindowDimensions As Integer, nInflectionA As Integer, nInflectionB As Integer)

            If nMovingWindowDimensions < 1 Then
                Throw New ArgumentOutOfRangeException("MovingwindowDimensions", nMovingWindowDimensions, "The moving window dimension must be greater than zero.")
            End If
            m_nMovingWindowDimensions = nMovingWindowDimensions

            If nInflectionA < 0 OrElse nInflectionA > 100 Then
                Throw New ArgumentOutOfRangeException("InflectionA", nInflectionA, "The inflection A point must be greater than or equal to zero and less than or equal to 100.")
            End If
            m_nInflectionA = nInflectionA

            If nInflectionB < 0 OrElse nInflectionB > 100 Then
                Throw New ArgumentOutOfRangeException("InflectionB", nInflectionB, "The inflection B point must be greater than or equal to zero and less than or equal to 100.")
            End If
            m_nInflectionB = nInflectionB

        End Sub


        Public Sub New()
            m_nMovingWindowDimensions = 5
            m_nInflectionA = 60
            m_nInflectionB = 100
        End Sub

    End Class

End Namespace