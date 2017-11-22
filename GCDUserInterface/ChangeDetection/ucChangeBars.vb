Imports GCDCore.ChangeDetection
Imports GCDCore.Visualization

Namespace ChangeDetection
    Public Class ucChangeBars

        Private m_Viewer As ElevationChangeBarViewer

        Private m_chngStats As GCDConsoleLib.GCD.DoDStats
        Public Property ChangeStats As GCDConsoleLib.GCD.DoDStats
            Get
                Return m_chngStats
            End Get
            Set(value As GCDConsoleLib.GCD.DoDStats)

                ' If this is the first time specifying the change stats then need to also set the units
                ' But set the units by setting the internal variable to avoid double call of RefreshBars()
                Dim bUseChangeStatsUnits As Boolean = m_chngStats Is Nothing
                m_chngStats = value
                m_DisplayUnits = value.StatsUnits

                RefreshBars(Nothing, Nothing)

            End Set
        End Property

        Private m_DisplayUnits As GCDConsoleLib.GCD.UnitGroup
        Public Property DisplayUnits
            Get
                Return m_DisplayUnits
            End Get
            Set(value)
                m_DisplayUnits = value
                RefreshBars(Nothing, Nothing)
            End Set
        End Property

        Public Sub New()

            ' This call is required by the designer.
            InitializeComponent()
            ' Add any initialization after the InitializeComponent() call.

        End Sub

        Private Sub ChangeBarsUC_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            m_Viewer = New ElevationChangeBarViewer(chtControl)

            cboType.Items.Add(New naru.db.NamedObject(ElevationChangeBarViewer.BarTypes.Area, "Areal"))
            cboType.Items.Add(New naru.db.NamedObject(ElevationChangeBarViewer.BarTypes.Volume, "Volumetric"))
            cboType.Items.Add(New naru.db.NamedObject(ElevationChangeBarViewer.BarTypes.Vertical, "Vertical Averages"))

            ' Add these handlers here so that everything is initialized properly before they fire
            AddHandler rdoAbsolute.CheckedChanged, AddressOf RefreshBars
            AddHandler cboType.SelectedIndexChanged, AddressOf RefreshBars
            cboType.SelectedIndex = 0

        End Sub

        Private Sub RefreshBars(sender As Object, e As EventArgs)

            If cboType.SelectedItem Is Nothing Then
                Return
            End If

            Dim eType As ElevationChangeBarViewer.BarTypes = DirectCast(Convert.ToInt32(DirectCast(cboType.SelectedItem, naru.db.NamedObject).ID), ElevationChangeBarViewer.BarTypes)

            Dim ca As UnitsNet.Area = ProjectManager.Project.CellArea

            Select Case eType
                Case ElevationChangeBarViewer.BarTypes.Area
                    m_Viewer.Refresh(m_chngStats.ErosionThr.GetArea(ca).As(DisplayUnits.ArUnit),
                                     m_chngStats.DepositionThr.GetArea(ca).As(DisplayUnits.ArUnit),
                                   UnitsNet.Area.GetAbbreviation(DisplayUnits.ArUnit), eType, rdoAbsolute.Checked)

                Case ElevationChangeBarViewer.BarTypes.Volume
                    m_Viewer.Refresh(m_chngStats.ErosionThr.GetVolume(ca, m_chngStats.StatsUnits.VertUnit).As(DisplayUnits.VolUnit),
                                     m_chngStats.DepositionThr.GetVolume(ca, m_chngStats.StatsUnits.VertUnit).As(DisplayUnits.VolUnit),
                                     m_chngStats.NetVolumeOfDifference_Thresholded.As(DisplayUnits.VolUnit),
                                     m_chngStats.ErosionErr.GetVolume(ca, m_chngStats.StatsUnits.VertUnit).As(DisplayUnits.VolUnit),
                                     m_chngStats.DepositionErr.GetVolume(ca, m_chngStats.StatsUnits.VertUnit).As(DisplayUnits.VolUnit),
                                     m_chngStats.NetVolumeOfDifference_Error.As(m_chngStats.StatsUnits.VertUnit),
                                     UnitsNet.Volume.GetAbbreviation(m_chngStats.StatsUnits.VertUnit), eType, rdoAbsolute.Checked)

                Case ElevationChangeBarViewer.BarTypes.Vertical
                    m_Viewer.Refresh(m_chngStats.AverageDepthErosion_Thresholded.As(DisplayUnits.VertUnit),
                                     m_chngStats.AverageDepthDeposition_Thresholded.As(DisplayUnits.VertUnit),
                                     m_chngStats.AverageThicknessOfDifferenceADC_Thresholded.As(DisplayUnits.VertUnit),
                                     m_chngStats.AverageThicknessOfDifferenceADC_Error.As(DisplayUnits.VertUnit),
                                     m_chngStats.AverageDepthErosion_Error.As(DisplayUnits.VertUnit),
                                     m_chngStats.AverageNetThicknessOfDifferenceADC_Error.As(DisplayUnits.VertUnit),
                                     UnitsNet.Length.GetAbbreviation(DisplayUnits.VertUnit), eType, rdoAbsolute.Checked)
            End Select

        End Sub

    End Class

End Namespace