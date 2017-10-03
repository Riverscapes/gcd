Public Class ChangeBarsUC

    Private m_chngStats As GCD.ChangeDetection.ChangeStats
    Private m_Viewer As GCD.ElevationChangeBarViewer
    Private m_eUnits As NumberFormatting.LinearUnits

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        m_eUnits = NumberFormatting.LinearUnits.m

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub ChangeBarsUC_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        m_Viewer = New GCD.ElevationChangeBarViewer(zGraph, NumberFormatting.GetUnitsAsString(m_eUnits))

        cboType.Items.Add(New ListItem(GCD.ElevationChangeBarViewer.BarTypes.Area, "Areal"))
        cboType.Items.Add(New ListItem(GCD.ElevationChangeBarViewer.BarTypes.Volume, "Volumetric"))
        cboType.Items.Add(New ListItem(GCD.ElevationChangeBarViewer.BarTypes.Vertical, "Vertical Averages"))
        cboType.SelectedIndex = 0

    End Sub

    Public Sub Initialize(chngStats As GCD.ChangeDetection.ChangeStats, eUnits As NumberFormatting.LinearUnits)
        m_chngStats = chngStats
        m_eUnits = eUnits

        RefreshBars()
    End Sub

    Private Sub RefreshBars()

        If TypeOf m_chngStats Is GCD.ChangeDetection.ChangeStats Then
            If TypeOf cboType.SelectedItem Is ListItem Then
                Dim eType As GCD.ElevationChangeBarViewer.BarTypes = DirectCast(DirectCast(cboType.SelectedItem, ListItem).ID, GCD.ElevationChangeBarViewer.BarTypes)

                Select Case eType
                    Case GCD.ElevationChangeBarViewer.BarTypes.Area
                        m_Viewer.Refresh(m_chngStats.AreaErosion_Thresholded, _
                                         m_chngStats.AreaDeposition_Thresholded, _
                                         m_eUnits, eType, rdoAbsolute.Checked)

                    Case GCD.ElevationChangeBarViewer.BarTypes.Volume
                        m_Viewer.Refresh(m_chngStats.VolumeErosion_Thresholded, _
                                         m_chngStats.VolumeDeposition_Thresholded, _
                                         m_chngStats.NetVolumeOfDifference_Thresholded, _
                                         m_chngStats.VolumeErosion_Error, _
                                         m_chngStats.VolumeDeposition_Error, _
                                         m_chngStats.NetVolumeOfDifference_Error, _
                                         m_eUnits, eType, rdoAbsolute.Checked)

                    Case GCD.ElevationChangeBarViewer.BarTypes.Vertical
                        m_Viewer.Refresh(m_chngStats.AverageDepthErosion_Thresholded, _
                                         m_chngStats.AverageDepthDeposition_Thresholded, _
                                         m_chngStats.AverageThicknessOfDifferenceADC_Thresholded, _
                                         m_chngStats.AverageThicknessOfDifferenceADC_Error, _
                                         m_chngStats.AverageDepthErosion_Error, _
                                         m_chngStats.AverageNetThicknessOfDifferenceADC_Error, _
                                         m_eUnits, eType, rdoAbsolute.Checked)

                End Select
            End If
        End If

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cboType.SelectedIndexChanged
        RefreshBars()
    End Sub

    Private Sub rdoAbsolute_CheckedChanged(sender As Object, e As System.EventArgs) Handles rdoAbsolute.CheckedChanged
        RefreshBars()
    End Sub
End Class
