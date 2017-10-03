Namespace GISCode.GCD

    Public Enum GCDAnalysisType
        SIMPLE
        PROPAGATED
        PROBABILITY
    End Enum

    Public Class ChangeDetectionSettings

#Region "Members"

        Private _AnalysisType As GCDAnalysisType
        Private _Threshold As Double
        Private _Probability As Double

        Private _Baysian As Boolean
        Private _filterSize As Integer
        Private _percentLess As Integer
        Private _percentGreater As Integer
#End Region

#Region "Properties"

        Public ReadOnly Property AnalysisType As GCDAnalysisType
            Get
                Return _AnalysisType
            End Get
        End Property

        Public ReadOnly Property Threshold As Double
            Get
                Return _Threshold
            End Get
        End Property

        Public ReadOnly Property Probability As Double
            Get
                Return _Probability
            End Get
        End Property

        Public ReadOnly Property Baysian As Boolean
            Get
                Return _Baysian
            End Get
        End Property

        Public ReadOnly Property filterSize As Integer
            Get
                Return _filterSize
            End Get
        End Property

        Public ReadOnly Property percentLess As Integer
            Get
                Return _percentLess
            End Get
        End Property

        Public ReadOnly Property percentGreater As Integer
            Get
                Return _percentGreater
            End Get
        End Property
#End Region

        Public Sub New(AnalysisType As GCDAnalysisType, Threshold As Double, Probability As Double, _
                       Baysian As Boolean, filterSize As Integer, percentLess As Integer, percentGreater As Integer)
            _AnalysisType = AnalysisType
            _Threshold = Threshold
            _Probability = Probability
            _Baysian = Baysian
            _filterSize = filterSize
            _percentLess = percentLess
            _percentGreater = percentGreater

        End Sub
    End Class

End Namespace