Imports naru.db
Imports System.Windows.Forms

Namespace UI.SurveyLibrary

    Public Class frmSurveyDateTime

        Private m_dateTime As Core.GCDProject.SurveyDateTime

        Public Sub New()
            ' This call is required by the designer.
            InitializeComponent()
            m_dateTime = New Core.GCDProject.SurveyDateTime
        End Sub

        Public Property SurveyDateTime As Core.GCDProject.SurveyDateTime
            Get
                Return m_dateTime
            End Get
            Set(value As Core.GCDProject.SurveyDateTime)
                m_dateTime = value
            End Set
        End Property

        Private Sub frmSurveyDateTime_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

            Dim nIndex As Integer = 0

            cboYear.Items.Add(New NamedObject(0, "YYYY"))
            cboYear.SelectedIndex = 0
            For nYear As Integer = 1970 To Now.Year + 5
                nIndex = cboYear.Items.Add(New NamedObject(nYear, nYear))
                If nYear = m_dateTime.Year Then
                    cboYear.SelectedIndex = nIndex
                End If
            Next

            cboMonth.Items.Add(New NamedObject(0, "MM"))
            cboMonth.SelectedIndex = 0
            For nMonth As Integer = 1 To 12
                Dim dt As New DateTime(1970, nMonth, 1)
                nIndex = cboMonth.Items.Add(New NamedObject(nMonth, dt.ToString("MMM")))
                If nMonth = m_dateTime.Month Then
                    cboMonth.SelectedIndex = nMonth
                End If
            Next

            ' Note that the days of the month are loaded repeatedly
            ' when the year or month dropdowns change their selection
            ' and that this is triggered by the lines of code above
            ReLoadDaysOfMonth(m_dateTime.Day)

            cboHour.Items.Add(New NamedObject(-1, "HH"))
            cboHour.SelectedIndex = 0
            For nHour As Integer = 0 To 24
                nIndex = cboHour.Items.Add(New NamedObject(nHour, nHour.ToString("00")))
                If nHour = m_dateTime.Hour Then
                    cboHour.SelectedIndex = nIndex
                End If
            Next

            cboMinute.Items.Add(New naru.db.NamedObject(-1, "MM"))
            cboMinute.SelectedIndex = 0
            For nMin As Integer = 0 To 59
                nIndex = cboMinute.Items.Add(New NamedObject(nMin, nMin.ToString("00")))
                If nMin = m_dateTime.Minute Then
                    cboMinute.SelectedIndex = nIndex
                End If
            Next

        End Sub

        Private Function ValidateForm() As Boolean

            If DirectCast(cboYear.SelectedItem, NamedObject).ID = 0 AndAlso DirectCast(cboMonth.SelectedItem, NamedObject).ID <> 0 Then
                MessageBox.Show("You must select a year if you want to specify a month.", My.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            If DirectCast(cboYear.SelectedItem, NamedObject).ID = 0 AndAlso DirectCast(cboMonth.SelectedItem, NamedObject).ID = 0 AndAlso DirectCast(cboDay.SelectedItem, NamedObject).ID <> 0 Then
                MessageBox.Show("You must select a year and month if you want to specify a day.", My.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            If DirectCast(cboHour.SelectedItem, NamedObject).ID = -1 AndAlso DirectCast(cboMinute.SelectedItem, NamedObject).ID > -1 Then
                MessageBox.Show("You must also select the hour if you want to specify the minute of the hour.", My.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            Return True

        End Function

        Private Sub cmdSave_Click(sender As Object, e As System.EventArgs) Handles cmdSave.Click

            If Not ValidateForm() Then
                Me.DialogResult = System.Windows.Forms.DialogResult.None
                Exit Sub
            End If

            m_dateTime = New Core.GCDProject.SurveyDateTime
            m_dateTime.Year = DirectCast(cboYear.SelectedItem, NamedObject).ID
            m_dateTime.Month = DirectCast(cboMonth.SelectedItem, NamedObject).ID
            m_dateTime.Day = DirectCast(cboDay.SelectedItem, NamedObject).ID

            m_dateTime.Hour = DirectCast(cboHour.SelectedItem, NamedObject).ID
            m_dateTime.Minute = DirectCast(cboMinute.SelectedItem, NamedObject).ID

        End Sub

        ''' <summary>
        ''' Reload the days of the month when the year or month changes
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub cboYear_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles _
        cboYear.SelectedIndexChanged,
        cboMonth.SelectedIndexChanged

            Dim nCurrentDay As Integer = 0
            If TypeOf cboDay.SelectedItem Is NamedObject Then
                nCurrentDay = DirectCast(cboDay.SelectedItem, NamedObject).ID
            End If

            ReLoadDaysOfMonth(nCurrentDay)

        End Sub

        Private Sub ReLoadDaysOfMonth(nSelectDay As Integer)

            Dim nMaxDays As Integer = 31
            If TypeOf cboYear.SelectedItem Is NamedObject Then
                Dim nYear As UShort = DirectCast(cboYear.SelectedItem, NamedObject).ID
                If nYear > 0 Then
                    If TypeOf cboMonth.SelectedItem Is NamedObject Then
                        Dim nMonth As Byte = DirectCast(cboMonth.SelectedItem, NamedObject).ID
                        If nMonth > 0 Then
                            nMaxDays = DateTime.DaysInMonth(nYear, nMonth)
                        End If
                    End If
                End If
            End If

            cboDay.Items.Clear()
            cboDay.Items.Add(New NamedObject(0, "DD"))
            cboDay.SelectedIndex = 0
            For nDay As Integer = 1 To nMaxDays
                Dim nIndex As Integer = cboDay.Items.Add(New NamedObject(nDay, nDay))
                If nDay = nSelectDay Then
                    cboDay.SelectedIndex = nIndex
                End If
            Next
        End Sub
    End Class

End Namespace