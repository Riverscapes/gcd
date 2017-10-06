Imports System.Windows.Forms

Namespace UI.ChangeDetection

    Public Class ucDoDProperties

        Private m_rDoD As ProjectDS.DoDsRow

        Public Property DoDRow As ProjectDS.DoDsRow
            Get
                Return m_rDoD
            End Get
            Set(value As ProjectDS.DoDsRow)
                m_rDoD = value
            End Set
        End Property

        Private Sub DodPropertiesUC_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            ' Make all textboxes have invisible border. Easier to keep border turned on
            ' for working in the designer.
            For Each aControl In Me.Controls
                If TypeOf aControl Is TextBox Then
                    DirectCast(aControl, TextBox).BorderStyle = System.Windows.Forms.BorderStyle.None
                End If
            Next

            If TypeOf m_rDoD Is ProjectDS.DoDsRow Then
                txtNewDEM.Text = m_rDoD.NewSurveyName
                txtNewError.Text = m_rDoD.NewErrorName

                txtOldDEM.Text = m_rDoD.OldSurveyName
                txtOldError.Text = m_rDoD.OldErrorName

                If m_rDoD.TypeMinLOD Then
                    txtType.Text = "Minimum Level of Detection (MinLod)"
                ElseIf m_rDoD.TypePropagated Then
                    txtType.Text = "Propagated Error"
                    grpPropagated.Visible = True

                ElseIf m_rDoD.TypeProbabilistic Then
                    txtType.Text = "Probabilistic"
                    grpProbabilistic.Visible = True
                    grpPropagated.Visible = True
                End If

                If m_rDoD.IsThresholdNull Then
                    lblThreshold.Enabled = False
                    txtThreshold.Enabled = False
                Else
                    If m_rDoD.TypeProbabilistic Then
                        txtThreshold.Text = (100 * m_rDoD.Threshold).ToString("0") & "%"
                    Else
                        txtThreshold.Text = m_rDoD.Threshold.ToString("#,##0.00") & m_rDoD.ProjectRow.DisplayUnits
                    End If
                End If

                If m_rDoD.TypePropagated OrElse m_rDoD.TypeProbabilistic Then
                    If Not m_rDoD.IsPropagatedErrorRasterPathNull Then
                        txtPropErr.Text = Core.GCDProject.ProjectManagerBase.GetAbsolutePath(m_rDoD.PropagatedErrorRasterPath)
                    End If
                End If

                If m_rDoD.TypeProbabilistic Then

                    If Not m_rDoD.IsThresholdNull Then
                        txtConfidence.Text = txtThreshold.Text
                    End If

                    If Not m_rDoD.IsProbabilityRasterNull Then
                        txtProbabilityRaster.Text = Core.GCDProject.ProjectManagerBase.GetAbsolutePath(m_rDoD.ProbabilityRaster)
                    End If

                    If Not m_rDoD.IsSpatialCoErosionRasterNull AndAlso Not String.IsNullOrEmpty(m_rDoD.SpatialCoErosionRaster) Then
                        txtErosionalSpatialCoherenceRaster.Text = Core.GCDProject.ProjectManagerBase.GetAbsolutePath(m_rDoD.SpatialCoErosionRaster)
                    End If

                    If Not m_rDoD.IsSpatialCoDepositionRasterNull AndAlso Not String.IsNullOrEmpty(m_rDoD.SpatialCoDepositionRaster) Then
                        txtDepositionSpatialCoherenceRaster.Text = Core.GCDProject.ProjectManagerBase.GetAbsolutePath(m_rDoD.SpatialCoDepositionRaster)
                    End If

                    If Not m_rDoD.IsConditionalProbRasterNull Then
                        'txtConditionalRaster.Text = GCD.GCDProject.ProjectManager.GetAbsolutePath(m_rDoD.ConditionalProbRaster)
                    End If

                    If Not m_rDoD.IsPosteriorRasterNull AndAlso Not String.IsNullOrEmpty(m_rDoD.PosteriorRaster) Then
                        txtPosteriorRaster.Text = Core.GCDProject.ProjectManagerBase.GetAbsolutePath(m_rDoD.PosteriorRaster)
                    End If

                    If Not m_rDoD.IsBayesianNull AndAlso m_rDoD.Bayesian Then
                        txtBayesian.Text = "Bayesian updating"
                        If Not m_rDoD.IsFilterNull Then
                            txtBayesian.Text &= " with filter size of " & m_rDoD.Filter.ToString & "x" & m_rDoD.Filter.ToString & " cells"
                        End If
                    Else
                        txtBayesian.Text = "None"
                    End If
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
                    sItemName = m_rDoD.NewSurveyName
                Else
                    sItemName = m_rDoD.OldSurveyName
                End If

                For Each aDEMSurvey In m_rDoD.ProjectRow.GetDEMSurveyRows
                    If String.Compare(sItemName, aDEMSurvey.Name, True) = 0 Then
                        ' TODO 
                        Throw New Exception("not implemented")
                        '  Core.GCDProject.ProjectManagerUI.ArcMapManager.AddDEM(aDEMSurvey)
                        Exit Sub
                    End If
                Next

            ElseIf cms.SourceControl.Name.ToLower.Contains("error") Then

                ' Get the new or old error surface name
                If cms.SourceControl.Name.ToLower.Contains("old") Then
                    sItemName = m_rDoD.NewErrorName
                Else
                    sItemName = m_rDoD.OldErrorName
                End If

                For Each aDEMSurvey In m_rDoD.ProjectRow.GetDEMSurveyRows
                    For Each anErrorSurface In aDEMSurvey.GetErrorSurfaceRows
                        If String.Compare(anErrorSurface.Name, sItemName, True) = 0 Then
                            ' TODO 
                            Throw New Exception("not implemented")
                            'Core.GCDProject.ProjectManagerUI.ArcMapManager.AddErrSurface(anErrorSurface)
                            Exit Sub
                        End If
                    Next
                Next
            Else
                Dim sPath As String = cms.SourceControl.Text
                If Not String.IsNullOrEmpty(sPath) Then
                    If Core.GISDataStructures.Raster.Exists(sPath) Then
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

        Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
            MsgBox("Copy DoD not yet implemented.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
        End Sub

        Private Sub PropertiesToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PropertiesToolStripMenuItem.Click
            MsgBox("Properties not yet implemented", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
        End Sub

    End Class

End Namespace