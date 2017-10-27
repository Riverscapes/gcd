Namespace Core.Project
    Public Class SurveyDateTime
        Implements IComparable(Of SurveyDateTime)

        Private m_nYear As UShort
        Private m_nMonth As Byte
        Private m_nDay As Byte

        Private m_nHour As Short
        Private m_nMin As Short

#Region "Properties"

        Public Property Year As UShort
            Get
                Return m_nYear
            End Get
            Set(value As UShort)
                m_nYear = value
            End Set
        End Property

        Public Property Month As Byte
            Get
                Return m_nMonth
            End Get
            Set(value As Byte)
                If value < 0 OrElse value > 12 Then
                    Throw New ArgumentOutOfRangeException("value", value, "Invalid month value.")
                Else
                    m_nMonth = value
                End If
            End Set
        End Property

        Public Property Day As Byte
            Get
                Return m_nDay
            End Get
            Set(value As Byte)
                If value < -1 OrElse value > 31 Then
                    Throw New ArgumentOutOfRangeException("value", value, "Invalid day value.")
                Else
                    m_nDay = value
                End If
            End Set
        End Property

        Public Property Hour As Short
            Get
                Return m_nHour
            End Get
            Set(value As Short)
                If value < -1 OrElse value > 23 Then
                    Throw New ArgumentOutOfRangeException("value", value, "Invalid hour value.")
                Else
                    m_nHour = value
                End If
            End Set
        End Property

        ''' <summary>
        ''' Minutes of the hour
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Note that zero minutes is valid. Therefore a null minute value is -1</remarks>
        Public Property Minute As Short
            Get
                Return m_nMin
            End Get
            Set(value As Short)
                If value < -1 OrElse value > 59 Then
                    Throw New ArgumentOutOfRangeException("value", value, "Invalid minute value.")
                Else
                    m_nMin = value
                End If
            End Set
        End Property

#End Region

        Public Sub New()

            ' The default for all values is Null, stored as zero
            m_nYear = 0
            m_nMonth = 0
            m_nDay = 0

            m_nHour = -1
            m_nMin = -1
        End Sub

        Public Sub New(ByRef rDEMSurvey As ProjectDS.DEMSurveyRow)
            Me.New()

            ' New survey data time fields
            If Not rDEMSurvey.IsSurveyYearNull Then
                Me.Year = rDEMSurvey.SurveyYear
                If Not rDEMSurvey.IsSurveyMonthNull Then
                    Me.Month = rDEMSurvey.SurveyMonth
                    If Not rDEMSurvey.IsSurveyDayNull Then
                        Me.Day = rDEMSurvey.SurveyDay
                    End If
                End If
            End If

            If Not rDEMSurvey.IsSurveyHourNull Then
                Me.Hour = rDEMSurvey.SurveyHour
                If Not rDEMSurvey.IsSurveyMinNull Then
                    Me.Minute = rDEMSurvey.SurveyMin
                End If
            End If
        End Sub

        Public Overrides Function ToString() As String

            Dim sDate As String = String.Empty
            If m_nYear > 0 Then
                If m_nMonth > 0 Then
                    If m_nDay > 0 Then
                        Dim dt As New DateTime(m_nYear, m_nMonth, m_nDay)
                        sDate = dt.ToString("yyyy MMM dd")
                    Else
                        Dim dt As New DateTime(m_nYear, m_nMonth, 1)
                        sDate = dt.ToString("yyyy MMM")
                    End If
                Else
                    sDate = m_nYear.ToString
                End If
            End If

            Dim sTime As String = String.Empty
            If m_nHour >= 0 Then
                ' Note that minutes can be zero
                If m_nMin >= 0 Then
                    sTime = String.Format("{0:00}:{1:00}", m_nHour, m_nMin)
                Else
                    sTime = m_nHour.ToString("00")
                End If
            End If

            Dim sResult As String = String.Empty
            If String.IsNullOrEmpty(sDate) Then
                If String.IsNullOrEmpty(sTime) Then
                    sResult = "Not Set"
                Else
                    sResult = sTime
                End If
            Else
                If String.IsNullOrEmpty(sTime) Then
                    sResult = sDate
                Else
                    sResult = String.Format("{0} {1}", sDate, sTime)
                End If
            End If

            Return sResult

        End Function

        Public Function CompareTo(other As SurveyDateTime) As Integer _
            Implements IComparable(Of SurveyDateTime).CompareTo

            Dim nResult As Integer = m_nYear - other.Year
            If nResult = 0 Then
                nResult = m_nMonth - other.Month
                If nResult = 0 Then
                    nResult = m_nDay - other.Day
                    If nResult = 0 Then
                        nResult = m_nHour - other.Hour
                        If nResult = 0 Then
                            nResult = m_nMin - other.Minute
                        End If
                    End If
                End If
            End If

            Return nResult

        End Function

    End Class
End Namespace