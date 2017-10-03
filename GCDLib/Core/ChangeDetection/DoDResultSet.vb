Namespace Core.ChangeDetection

    ''' <summary>
    ''' Container class for collecting together the results of a GCD change detection analysis
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DoDResultSet

        Private m_dodProps As ChangeDetectionProperties
        Private m_ChangeStats As ChangeDetection.ChangeStats
        Private m_HistogramStats As DoDResultHistograms

#Region "Properties"

        Public ReadOnly Property ChangeStats As ChangeStats
            Get
                Return m_ChangeStats
            End Get
        End Property

        Public ReadOnly Property HistogramStats As DoDResultHistograms
            Get
                Return m_HistogramStats
            End Get
        End Property

        Public ReadOnly Property DoDProperties As ChangeDetectionProperties
            Get
                Return m_dodProps
            End Get
        End Property

#End Region

        Public Sub New(ByRef theChangeStats As ChangeStats, ByRef histoStats As DoDResultHistograms, dodProps As ChangeDetectionProperties)
            m_ChangeStats = theChangeStats
            m_HistogramStats = histoStats
            m_dodProps = dodProps
        End Sub

    End Class

End Namespace