Namespace ErrorCalculation

    Public Class ErrorCalcPropertiesBase

        Private m_sSurveyMethod As String
        Private m_sErrorType As String

        Public ReadOnly Property SurveyMethod As String
            Get
                Return m_sSurveyMethod
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return m_sSurveyMethod
        End Function

        Public ReadOnly Property ErrorType As String
            Get
                Return m_sErrorType
            End Get
        End Property

        Public Sub New(sSurveyMethod As String, sErrorType As String)
            m_sSurveyMethod = sSurveyMethod
            m_sErrorType = sErrorType
        End Sub
    End Class

    ''' <summary>
    ''' '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ErrorCalcPropertiesUniform
        Inherits ErrorCalcPropertiesBase

        Private m_fUniformErrorValue As Double

        Public ReadOnly Property UniformErrorValue As Double
            Get
                Return m_fUniformErrorValue
            End Get
        End Property

        Public Sub New(sSurveyMethod As String, fUniformErrorValue As Double)
            MyBase.New(sSurveyMethod, "Uniform Error")

            If fUniformErrorValue < 0 Then
                Throw New Exception("Invalid uniform error value")
            End If

            m_fUniformErrorValue = fUniformErrorValue
        End Sub
    End Class

    ''' <summary>
    ''' '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ErrorCalcPropertiesFIS
        Inherits ErrorCalcPropertiesBase

        Private m_dFISInputs As Dictionary(Of String, Integer)
        Private m_sFISRuleFilePath As String
        Private m_nFISID As Integer

        Public ReadOnly Property FISRuleName As String
            Get
                Return IO.Path.GetFileNameWithoutExtension(m_sFISRuleFilePath)
            End Get
        End Property

        Public ReadOnly Property FISRulePath As String
            Get
                Return m_sFISRuleFilePath
            End Get
        End Property

        Public ReadOnly Property FISID As Integer
            Get
                Return m_nFISID
            End Get
        End Property

        Public ReadOnly Property FISInputs As Dictionary(Of String, Integer)
            Get
                Return m_dFISInputs
            End Get
        End Property

        Public Sub New(sSurveyMethod As String, nFISID As Integer, sFISRuleFilePath As String, dFISInputs As Dictionary(Of String, Integer))
            MyBase.New(sSurveyMethod, "FIS Error")

            m_nFISID = nFISID
            m_sFISRuleFilePath = sFISRuleFilePath
   
            If dFISInputs Is Nothing Then
                m_dFISInputs = New Dictionary(Of String, Integer)
            Else
                m_dFISInputs = dFISInputs
            End If
        End Sub

    End Class

    Public Class ErrorCalcPropertiesAssoc
        Inherits ErrorCalcPropertiesBase

        Private m_nAssociatedSurfaceID As Integer

        Public ReadOnly Property AssociatedSurfaceID As Integer
            Get
                Return m_nAssociatedSurfaceID
            End Get
        End Property

        Public Sub New(sSurveyMethod As String, nAssociatedSurfaceID As Integer)
            MyBase.New(sSurveyMethod, "Associated Surface")

            If nAssociatedSurfaceID < 1 Then
                Throw New Exception(String.Format("Invalid associated surface ID ({0}) for {1} survey method", nAssociatedSurfaceID, sSurveyMethod))
            End If

            m_nAssociatedSurfaceID = nAssociatedSurfaceID
        End Sub
    End Class

End Namespace