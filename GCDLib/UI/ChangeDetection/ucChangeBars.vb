Imports GCDLib.Core.ChangeDetection
Imports GCDLib.Core.Visualization

Namespace UI.ChangeDetection
    Public Class ucChangeBars

        Private m_chngStats As ChangeStats
        Private m_Viewer As ElevationChangeBarViewer
        Private m_eUnits As UnitsNet.Units.LengthUnit

        Public Sub New()

            ' This call is required by the designer.
            InitializeComponent()
            m_eUnits = UnitsNet.Units.LengthUnit.Meter

            ' Add any initialization after the InitializeComponent() call.

        End Sub

        Private Sub ChangeBarsUC_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            m_Viewer = New ElevationChangeBarViewer(chtControl, m_eUnits)

            cboType.Items.Add(New naru.db.NamedObject(ElevationChangeBarViewer.BarTypes.Area, "Areal"))
            cboType.Items.Add(New naru.db.NamedObject(ElevationChangeBarViewer.BarTypes.Volume, "Volumetric"))
            cboType.Items.Add(New naru.db.NamedObject(ElevationChangeBarViewer.BarTypes.Vertical, "Vertical Averages"))
            cboType.SelectedIndex = 0

        End Sub

        Public Sub Initialize(chngStats As ChangeStats, eUnits As UnitsNet.Units.LengthUnit)
            m_chngStats = chngStats
            m_eUnits = eUnits

            RefreshBars(eUnits, GCDConsoleLib.Utility.Conversion.LengthUnit2AreaUnit(eUnits), GCDConsoleLib.Utility.Conversion.LengthUnit2VolumeUnit(eUnits))
        End Sub

        Private Sub RefreshBars(linearDisplayUnits As UnitsNet.Units.LengthUnit, areaDisplayUnits As UnitsNet.Units.AreaUnit, volumeDisplayUnits As UnitsNet.Units.VolumeUnit)

            If TypeOf m_chngStats Is ChangeStats Then
                If TypeOf cboType.SelectedItem Is naru.db.NamedObject Then
                    Dim eType As ElevationChangeBarViewer.BarTypes = DirectCast(Convert.ToInt32(DirectCast(cboType.SelectedItem, naru.db.NamedObject).ID), ElevationChangeBarViewer.BarTypes)

                    Select Case eType
                        Case ElevationChangeBarViewer.BarTypes.Area
                            m_Viewer.Refresh(m_chngStats.AreaErosion_Thresholded,
                                             m_chngStats.AreaDeposition_Thresholded,
                                             m_eUnits, eType, rdoAbsolute.Checked)

                        Case ElevationChangeBarViewer.BarTypes.Volume
                            m_Viewer.Refresh(m_chngStats.VolumeErosion_Thresholded,
                                             m_chngStats.VolumeDeposition_Thresholded,
                                             m_chngStats.NetVolumeOfDifference_Thresholded,
                                             m_chngStats.VolumeErosion_Error,
                                             m_chngStats.VolumeDeposition_Error,
                                             m_chngStats.NetVolumeOfDifference_Error,
                                             m_eUnits, eType, rdoAbsolute.Checked)

                        Case ElevationChangeBarViewer.BarTypes.Vertical
                            m_Viewer.Refresh(m_chngStats.AverageDepthErosion_Thresholded,
                                             m_chngStats.AverageDepthDeposition_Thresholded,
                                             m_chngStats.AverageThicknessOfDifferenceADC_Thresholded,
                                             m_chngStats.AverageThicknessOfDifferenceADC_Error,
                                             m_chngStats.AverageDepthErosion_Error,
                                             m_chngStats.AverageNetThicknessOfDifferenceADC_Error,
                                             m_eUnits, eType, rdoAbsolute.Checked)

                    End Select
                End If
            End If

        End Sub

        Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cboType.SelectedIndexChanged
            RefreshBars(m_eUnits, GCDConsoleLib.Utility.Conversion.LengthUnit2AreaUnit(m_eUnits), GCDConsoleLib.Utility.Conversion.LengthUnit2VolumeUnit(m_eUnits))
        End Sub

        Private Sub rdoAbsolute_CheckedChanged(sender As Object, e As System.EventArgs) Handles rdoAbsolute.CheckedChanged
            RefreshBars(m_eUnits, GCDConsoleLib.Utility.Conversion.LengthUnit2AreaUnit(m_eUnits), GCDConsoleLib.Utility.Conversion.LengthUnit2VolumeUnit(m_eUnits))
        End Sub
    End Class
End Namespace