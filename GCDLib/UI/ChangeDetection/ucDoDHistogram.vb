﻿Imports GCD.GCDLib.Core.ChangeDetection
Imports GCD.GCDLib.Core.Visualization
Imports System.Windows.Forms

Namespace UI.ChangeDetection

    Public Class ucDoDHistogram

        Private m_DoDResultSet As DoDResultSet
        Private m_HistogramViewer As DoDHistogramViewerClass
        Private m_UserSelectedUnits As DoDResultSet

        Public Property DoDResultSet As DoDResultSet
            Get
                Return m_DoDResultSet
            End Get
            Set(value As DoDResultSet)
                m_DoDResultSet = value
            End Set
        End Property

        ''' <summary>
        ''' Variable to allow the user to toggle between units on the axis
        ''' </summary>
        ''' <remarks></remarks>
        Public Property UserSelectedUnits As DoDResultSet
            Get
                Return m_UserSelectedUnits
            End Get
            Set(ByVal value As DoDResultSet)
                m_UserSelectedUnits = value
            End Set

        End Property

        Private Sub CDResultsUC_Load(sender As Object, e As System.EventArgs) Handles Me.Load

#If DEBUG Then
            cmdRefresh.Visible = True
#End If

            ' Needed for Visual Studio Designer to work when the object is not created yet.
            If TypeOf m_DoDResultSet Is DoDResultSet Then
                If m_UserSelectedUnits Is Nothing Then
                    m_HistogramViewer = New DoDHistogramViewerClass(zGraph, DoDResultSet.DoDProperties.Units)
                    RefreshHistogram()
                Else
                    m_HistogramViewer = New DoDHistogramViewerClass(zGraph, m_UserSelectedUnits.DoDProperties.Units)
                    RefreshHistogram()
                End If
            End If

        End Sub

        Private Sub SetUpHeaderCell(aCell As DataGridViewCell, sText As String, eAlignment As DataGridViewContentAlignment)
            aCell.Value = sText
            ' Dim txt As DataGridViewTextBoxCell = aCell
            'aCell.Style.Font = New Drawing.Font(grdData.Font, Drawing.FontStyle.Bold)
            ' aCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

        End Sub

        Private Sub cmdRefresh_Click(sender As System.Object, e As System.EventArgs) Handles cmdRefresh.Click

            RefreshHistogram()
        End Sub

        Private Sub rdoVolume_CheckedChanged(sender As Object, e As System.EventArgs) Handles rdoVolume.CheckedChanged
            RefreshHistogram()
        End Sub

        Public Sub RefreshHistogram()

            ' Need for Visual Studio design mode to work before the object is created.
            If TypeOf DoDResultSet Is DoDResultSet Then
                If TypeOf m_HistogramViewer Is DoDHistogramViewerClass Then

                    If rdoArea.Checked Then
                        If m_UserSelectedUnits Is Nothing Then
                            m_HistogramViewer.refresh(DoDResultSet.HistogramStats.m_RawAreaHist, DoDResultSet.HistogramStats.m_ThresholdedAreaHist, True, DoDResultSet.DoDProperties.Units)
                        Else
                            m_HistogramViewer.refresh(DoDResultSet.HistogramStats.m_RawAreaHist, DoDResultSet.HistogramStats.m_ThresholdedAreaHist, True, m_UserSelectedUnits.DoDProperties.Units)
                        End If
                    Else
                        If m_UserSelectedUnits Is Nothing Then
                            m_HistogramViewer.refresh(DoDResultSet.HistogramStats.m_RawVolumeHist, DoDResultSet.HistogramStats.m_ThresholdedVolumeHist, False, DoDResultSet.DoDProperties.Units)
                        Else
                            m_HistogramViewer.refresh(DoDResultSet.HistogramStats.m_RawVolumeHist, DoDResultSet.HistogramStats.m_ThresholdedVolumeHist, False, m_UserSelectedUnits.DoDProperties.Units)
                        End If
                    End If
                End If
            End If

        End Sub

    End Class

End Namespace