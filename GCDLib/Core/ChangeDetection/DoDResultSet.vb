Namespace Core.ChangeDetection

    ''' <summary>
    ''' Container class for collecting together the results of a GCD change detection analysis
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DoDResultSet

        Private m_dodProps As ChangeDetectionProperties
        Private m_ChangeStats As ChangeDetection.ChangeStats
        Private m_RawHistogramPath As String
        Private m_ThreshHistogramPath As String

#Region "Properties"

        Public ReadOnly Property ChangeStats As ChangeStats
            Get
                Return m_ChangeStats
            End Get
        End Property

        Public ReadOnly Property RawHistogramPath As String
            Get
                Return m_RawHistogramPath
            End Get
        End Property

        Public ReadOnly Property ThreshHistogramPath As String
            Get
                Return m_ThreshHistogramPath
            End Get
        End Property

        Public ReadOnly Property DoDProperties As ChangeDetectionProperties
            Get
                Return m_dodProps
            End Get
        End Property

#End Region

        Public Sub New(ByRef theChangeStats As ChangeStats, dodProps As ChangeDetectionProperties, rawHistogramPath As String, threshHistogramPath As String)
            m_ChangeStats = theChangeStats
            m_dodProps = dodProps
            m_RawHistogramPath = rawHistogramPath
            m_ThreshHistogramPath = threshHistogramPath
        End Sub

    End Class

End Namespace