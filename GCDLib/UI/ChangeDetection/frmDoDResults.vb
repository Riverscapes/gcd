Namespace UI.ChangeDetection
    Public Class frmDoDResults

        Private m_rDoD As ProjectDS.DoDsRow

        Public Sub New(ByRef dodResult As Core.ChangeDetection.DoDResultSet, rDoD As ProjectDS.DoDsRow)

            InitializeComponent()

            m_rDoD = rDoD
            ucHistogram.DoDResultSet = dodResult
            'm_rDoD = rDoD
            ucSummary.DoDResultSet = dodResult
            ucProperties.DoDRow = rDoD

        End Sub

        Private Sub DoDResultsForm_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            txtDoDName.Text = m_rDoD.Name
            ucBars.Initialize(ucHistogram.DoDResultSet.ChangeStats, ucSummary.Options.LinearUnits)



        End Sub

        Private Sub cmdAddToMap_Click(sender As System.Object, e As System.EventArgs) Handles cmdAddToMap.Click

            If TypeOf m_rDoD Is ProjectDS.DoDsRow Then
                ' TODO 
                Throw New Exception("not implemented")
                'Core.GCDProject.ProjectManagerUI.ArcMapManager.AddDoD(m_rDoD)
            Else
                MsgBox("The DoD member row is Null or empty.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
            End If
        End Sub

        Private Sub cmdBrowse_Click(sender As System.Object, e As System.EventArgs) Handles cmdBrowse.Click

            If TypeOf m_rDoD Is ProjectDS.DoDsRow Then
                Dim sFolder As String = Core.GCDProject.ProjectManagerBase.GetAbsolutePath(m_rDoD.OutputFolder) ' GCD.GCDProject.ProjectManagerBase.OutputManager.GetDoDOutputFolder(m_rDoD.Name)
                If IO.Directory.Exists(sFolder) Then
                    Process.Start("explorer.exe", sFolder)
                Else
                    MsgBox("The DoD folder does not exist: " & sFolder.ToString, MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                End If
            Else
                MsgBox("The DoD member row is Null or empty.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
            End If
        End Sub

        'TODO: Create a Event of Linear Units updated
        Private Sub UpdateToUserSelectedUnits(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ucSummary.Click
            'Calls m_Options
            ucHistogram.UserSelectedUnits = ucSummary.DoDResultSet
        End Sub

        Private Sub cmdHelp_Click(sender As System.Object, e As System.EventArgs) Handles cmdHelp.Click
            Process.Start(My.Resources.HelpBaseURL & "gcd-command-reference/gcd-project-explorer/l-individual-change-detection-context-menu/i-view-change-detection-results")
        End Sub

        Private Sub cmdHistogram_Click(sender As System.Object, e As System.EventArgs) Handles cmdHistogram.Click
            'Try
            '    Dim sRawDod As String = GCD.GCDProject.ProjectManagerBase.GetAbsolutePath(m_rDoD.RawDoDPath)
            '    Dim sThreshDoD As String = GCD.GCDProject.ProjectManagerBase.GetAbsolutePath(m_rDoD.ThreshDoDPath)
            '    Dim sRawHist As String = GCD.GCDProject.ProjectManagerBase.GetAbsolutePath(m_rDoD.RawHistPath)
            '    Dim sThrHist As String = GCD.GCDProject.ProjectManagerBase.GetAbsolutePath(m_rDoD.ThreshHistPath)
            '    Dim frm As New HistogramConfigForm(sRawDod, sThreshDoD, sRawHist, sThrHist)
            '    If frm.ShowDialog = Windows.Forms.DialogResult.OK Then
            '        ucHistogram.RefreshHistogram()
            '    End If
            'Catch ex As Exception
            '    ExceptionUI.HandleException(ex)
            'End Try
        End Sub

    End Class
End Namespace