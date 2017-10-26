Namespace UI.ChangeDetection
    Public Class frmDoDResults

        ''' <summary>
        ''' Stores the status of what columns, rows and units to use in the child user controls
        ''' </summary>
        ''' <remarks>This is passed to the pop-up form </remarks>
        Private m_Options As DoDSummaryDisplayOptions
        Private m_DoDResultSet As Core.ChangeDetection.DoDResultSet

        Public Sub New(sDoDName As String, ByRef dodResult As Core.ChangeDetection.DoDResultSet)

            InitializeComponent()

            m_DoDResultSet = dodResult
            m_Options = New DoDSummaryDisplayOptions(dodResult.DoDProperties.Units)
            txtDoDName.Text = sDoDName
        End Sub

        Private Sub DoDResultsForm_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            If Not Core.GCDProject.ProjectManagerUI.IsArcMap Then
                cmdAddToMap.Visible = False
                txtDoDName.Width = cmdAddToMap.Right - txtDoDName.Left
            End If

            ucBars.Initialize(m_DoDResultSet.ChangeStats, m_DoDResultSet.DoDProperties.Units)
            ucHistogram.LoadHistograms(m_DoDResultSet.RawHistogramPath, m_DoDResultSet.ThreshHistogramPath, m_DoDResultSet.DoDProperties.Units)

        End Sub

        Private Sub cmdAddToMap_Click(sender As System.Object, e As System.EventArgs) Handles cmdAddToMap.Click

            Throw New Exception("Add to map not implemented")

        End Sub

        Private Sub cmdBrowse_Click(sender As System.Object, e As System.EventArgs) Handles cmdBrowse.Click

            Dim sFolder As String = Core.GCDProject.ProjectManagerBase.GetAbsolutePath(m_DoDResultSet.DoDProperties.RawDoD)
            If IO.Directory.Exists(sFolder) Then
                Process.Start("explorer.exe", sFolder)
            End If
        End Sub

        Private Sub cmdSettings_Click(sender As Object, e As EventArgs) Handles cmdSettings.Click
            Try
                Dim frm As New frmDoDSummaryProperties(m_DoDResultSet.DoDProperties.Units, m_Options)
                If frm.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                    ucSummary.Refresh(m_DoDResultSet, m_Options)
                    ucHistogram.SetHistogramUnits(m_Options)
                    ucBars.Refresh()
                End If

            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try
        End Sub

        Private Sub cmdHelp_Click(sender As System.Object, e As System.EventArgs) Handles cmdHelp.Click
            Process.Start(My.Resources.HelpBaseURL & "gcd-command-reference/gcd-project-explorer/l-individual-change-detection-context-menu/i-view-change-detection-results")
        End Sub

    End Class

End Namespace