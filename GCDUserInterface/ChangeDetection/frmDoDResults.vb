Imports GCDCore.ChangeDetection

Namespace ChangeDetection
    Public Class frmDoDResults

        ''' <summary>
        ''' Stores the status of what columns, rows and units to use in the child user controls
        ''' </summary>
        ''' <remarks>This is passed to the pop-up form </remarks>
        Private m_Options As DoDSummaryDisplayOptions
        Private DoD As DoDBase

        Public Sub New(dodItem As GCDCore.Project.DoDBase)

            InitializeComponent()

            DoD = dodItem
            m_Options = New DoDSummaryDisplayOptions(ProjectManager.Project.Units)
            txtDoDName.Text = DoD.Name
            ucProperties.Initialize(DoD)

            ' Select the tab control to make it easy for user to quickly pan results
            tabProperties.Select()

        End Sub

        Private Sub DoDResultsForm_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            If Not ProjectManager.IsArcMap Then
                cmdAddToMap.Visible = False
                txtDoDName.Width = cmdAddToMap.Right - txtDoDName.Left
            End If

            ucBars.ChangeStats = DoD.Statistics
            ucHistogram.LoadHistograms(DoD.Histograms.Raw.Data, DoD.Histograms.Thr.Data)
            ucSummary.RefreshDisplay(DoD.Statistics, m_Options)

        End Sub

        Private Sub cmdAddToMap_Click(sender As System.Object, e As System.EventArgs) Handles cmdAddToMap.Click

            Throw New Exception("Add to map not implemented")

        End Sub

        Private Sub cmdBrowse_Click(sender As System.Object, e As System.EventArgs) Handles cmdBrowse.Click
            Process.Start("explorer.exe", DoD.RawDoD.RasterPath.Directory.FullName)
        End Sub

        Private Sub cmdSettings_Click(sender As Object, e As EventArgs) Handles cmdSettings.Click
            Try
                Dim frm As New frmDoDSummaryProperties(m_Options)
                If frm.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                    ucSummary.RefreshDisplay(DoD.Statistics, m_Options)
                    ucHistogram.SetHistogramUnits(m_Options.Units)
                    ucBars.Refresh()
                End If

            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try
        End Sub

        Private Sub cmdHelp_Click(sender As System.Object, e As System.EventArgs) Handles cmdHelp.Click
            Process.Start(GCDCore.Properties.Resources.HelpBaseURL & "gcd-command-reference/gcd-project-explorer/l-individual-change-detection-context-menu/i-view-change-detection-results")
        End Sub

    End Class

End Namespace