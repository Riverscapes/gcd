Namespace UI.ChangeDetection
    Public Class frmDoDResults

        ''' <summary>
        ''' Stores the status of what columns, rows and units to use in the child user controls
        ''' </summary>
        ''' <remarks>This is passed to the pop-up form </remarks>
        Private m_Options As DoDSummaryDisplayOptions
        Private m_DoDResult As Core.ChangeDetection.DoDResult

        Public Sub New(rDoD As ProjectDS.DoDsRow, ByRef dodResult As Core.ChangeDetection.DoDResult)

            InitializeComponent()

            m_DoDResult = dodResult
            m_Options = New DoDSummaryDisplayOptions(dodResult.Units)
            txtDoDName.Text = rDoD.Name
            ucProperties.DoDRow = rDoD

        End Sub

        Private Sub DoDResultsForm_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            If Not Core.GCDProject.ProjectManagerUI.IsArcMap Then
                cmdAddToMap.Visible = False
                txtDoDName.Width = cmdAddToMap.Right - txtDoDName.Left
            End If

            ucBars.Initialize(m_DoDResult.ChangeStats, m_DoDResult.Units)
            ucHistogram.LoadHistograms(m_DoDResult.RawHistogram, m_DoDResult.ThresholdedHistogram, m_DoDResult.Units)
            ucSummary.RefreshDisplay(m_DoDResult, m_Options)

        End Sub

        Private Sub cmdAddToMap_Click(sender As System.Object, e As System.EventArgs) Handles cmdAddToMap.Click

            Throw New Exception("Add to map not implemented")

        End Sub

        Private Sub cmdBrowse_Click(sender As System.Object, e As System.EventArgs) Handles cmdBrowse.Click

            Dim sFolder As String = IO.Path.GetDirectoryName(Core.GCDProject.ProjectManagerBase.GetAbsolutePath(m_DoDResult.RawDoD.FullName))
            If IO.Directory.Exists(sFolder) Then
                Process.Start("explorer.exe", sFolder)
            End If
        End Sub

        Private Sub cmdSettings_Click(sender As Object, e As EventArgs) Handles cmdSettings.Click
            Try
                Dim frm As New frmDoDSummaryProperties(m_DoDResult.Units, m_Options)
                If frm.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                    ucSummary.RefreshDisplay(m_DoDResult, m_Options)
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