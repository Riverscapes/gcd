Imports System.Windows.Forms

Namespace ChangeDetection

    Public Class ucDoDProperties

        Private Sub DodPropertiesUC_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            ' Make all textboxes have invisible border. Easier to keep border turned on
            ' for working in the designer.
            For Each aControl In Me.Controls
                If TypeOf aControl Is TextBox Then
                    DirectCast(aControl, TextBox).BorderStyle = System.Windows.Forms.BorderStyle.None
                End If
            Next

        End Sub

        Public Sub Initialize(dod As DoDBase)

            txtNewDEM.Text = dod.NewDEM.Name
            txtOldDEM.Text = dod.OldDEM.Name

            If TypeOf dod Is DoDMinLoD Then
                txtType.Text = "Minimum Level of Detection (MinLod)"
                With DirectCast(dod, DoDMinLoD)
                    txtThreshold.Text = .Threshold
                    txtThreshold.Text = String.Format("{0:0.00}{1}", ProjectManager.Project.Units.VertUnit, UnitsNet.Length.GetAbbreviation(ProjectManager.Project.Units.VertUnit))
                End With
            Else
                txtType.Text = "Propagated Error"
                grpPropagated.Visible = True

                With DirectCast(dod, DoDPropagated)
                    txtNewError.Text = .NewError.Name
                    txtOldError.Text = .OldError.Name
                    txtPropErr.Text = .PropagatedError.RelativePath
                End With

                If TypeOf dod Is DoDProbabilistic Then
                    txtType.Text = "Probabilistic"
                    grpProbabilistic.Visible = True
                    grpPropagated.Visible = True

                    With DirectCast(dod, DoDProbabilistic)
                        txtConfidence.Text = (100 * .ConfidenceLevel).ToString("0") & "%"
                        txtProbabilityRaster.Text = .PriorProbability.RelativePath
                        txtBayesian.Text = "None"

                        If TypeOf .SpatialCoherence Is GCDCore.Project.CoherenceProperties Then
                            txtPosteriorRaster.Text = .PosteriorProbability.RelativePath
                            txtConditionalRaster.Text = .ConditionalRaster.RelativePath
                            txtErosionalSpatialCoherenceRaster.Text = .SpatialCoherenceErosion.RelativePath
                            txtDepositionSpatialCoherenceRaster.Text = .SpatialCoherenceDeposition.RelativePath
                            txtBayesian.Text = String.Format("Bayesian updating with filter size of {0} X {0} cells", .SpatialCoherence.MovingWindowDimensions)
                        End If
                    End With
                End If
            End If
        End Sub

        Private Sub AddToMapToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles _
            AddToMapToolStripMenuItem.Click, AddToMapToolStripMenuItem1.Click

            Dim myItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
            Dim cms As ContextMenuStrip = CType(myItem.Owner, ContextMenuStrip)

            Dim sItemName As String = String.Empty
            If cms.SourceControl.Name.ToLower.Contains("dem") Then

                ' Get the new or old survey name
                If cms.SourceControl.Name.ToLower.Contains("new") Then
                    sItemName = txtNewDEM.Text
                Else
                    sItemName = txtOldDEM.Text
                End If

                Throw New NotImplementedException("add to map functionality commented out")
                'For Each aDEMSurvey In m_rDoD.ProjectRow.GetDEMSurveyRows
                '    If String.Compare(sItemName, aDEMSurvey.Name, True) = 0 Then
                '        '  Core.GCDProject.ProjectManagerUI.ArcMapManager.AddDEM(aDEMSurvey)
                '        Exit Sub
                '    End If
                'Next

            ElseIf cms.SourceControl.Name.ToLower.Contains("error") Then

                ' Get the new or old error surface name
                If cms.SourceControl.Name.ToLower.Contains("old") Then
                    sItemName = txtNewError.Text
                Else
                    sItemName = txtOldError.Text
                End If

                Throw New NotImplementedException("Add to map functionaility commented out")
                'For Each aDEMSurvey In m_rDoD.ProjectRow.GetDEMSurveyRows
                '    For Each anErrorSurface In aDEMSurvey.GetErrorSurfaceRows
                '        If String.Compare(anErrorSurface.Name, sItemName, True) = 0 Then
                '            'Core.GCDProject.ProjectManagerUI.ArcMapManager.AddErrSurface(anErrorSurface)
                '            Exit Sub
                '        End If
                '    Next
                'Next
            Else
                Dim sPath As String = cms.SourceControl.Text
                If Not String.IsNullOrEmpty(sPath) Then
                    If IO.File.Exists(sPath) Then
                        'GISCode.ArcMap.AddToMap(My.ThisApplication, sPath, IO.Path.GetFileNameWithoutExtension(sPath), GISDataStructures.BasicGISTypes.Raster)
                        Dim sFileName As String = IO.Path.GetFileNameWithoutExtension(sPath)

                        Select Case sFileName

                            Case "PropErr"
                                ' TODO 
                                Throw New Exception("not implemented")
                                ' Core.GCDProject.ProjectManagerUI.ArcMapManager.AddPropErr(m_rDoD)

                            Case "priorProb"
                                ' TODO 
                                Throw New Exception("not implemented")
                                '  Core.GCDProject.ProjectManagerUI.ArcMapManager.AddProbabilityRaster(m_rDoD, m_rDoD.ProbabilityRaster)

                            Case "nbrErosion"
                                ' TODO 
                                Throw New Exception("not implemented")
                                ' Core.GCDProject.ProjectManagerUI.ArcMapManager.AddProbabilityRaster(m_rDoD, m_rDoD.SpatialCoErosionRaster)

                            Case "nbrDeposition"
                                ' TODO 
                                Throw New Exception("not implemented")
                                '  Core.GCDProject.ProjectManagerUI.ArcMapManager.AddProbabilityRaster(m_rDoD, m_rDoD.SpatialCoDepositionRaster)

                        End Select
                    End If
                End If
            End If
        End Sub

        Private Sub PropertiesToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PropertiesToolStripMenuItem.Click
            MsgBox("Properties not yet implemented", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
        End Sub

    End Class

End Namespace