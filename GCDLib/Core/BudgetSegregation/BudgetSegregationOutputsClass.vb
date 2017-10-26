
Namespace Core.BudgetSegregation

    Public Class BudgetSegregationOutputsClass

        Private m_pieCharts As PieChartOutputs
        Private m_dMaskOutputs As New Dictionary(Of String, MaskOutputClass)
        Private m_sClassLegendPath As String
        Private m_sClassSummaryPath As String ' Class summary XML file that combines all budget summaries into one file.

        Private m_sPolygonMask As String

#Region "Properties"

        Public ReadOnly Property PieCharts As PieChartOutputs
            Get
                Return m_pieCharts
            End Get
        End Property

        Public ReadOnly Property MaskOutputs As Dictionary(Of String, MaskOutputClass)
            Get
                Return m_dMaskOutputs
            End Get
        End Property


        Public ReadOnly Property ClassLegendPath As String
            Get
                Return m_sClassLegendPath
            End Get
        End Property

        Public ReadOnly Property ClassSummaryPath As String
            Get
                Return m_sClassSummaryPath
            End Get
        End Property

        Public ReadOnly Property PolygonMask As String
            Get
                Return m_sPolygonMask
            End Get
        End Property

#End Region

        Public Sub New(ByVal BSOutputFolder As String, ByVal MaskLabels As Dictionary(Of String, Integer), sPolygonMask As String)

            'New code to move the .pngs to a subfolder called Figs - Hensleigh 4/24/2014
            Dim BS_FiguresOutputFolder As String = IO.Path.Combine(BSOutputFolder, "Figs")

            Dim PercentageTotalDepositionVolumePiePath As String = IO.Path.Combine(BSOutputFolder, "PercentageTotalDepositionVolumePie.png")
            Dim PercentageTotalErosionVolumePiePath As String = IO.Path.Combine(BSOutputFolder, "PercentageTotalErosionVolumePie.png")
            Dim PercentageTotalVolumePiePath As String = IO.Path.Combine(BSOutputFolder, "PercentageTotalVolumePie.png")

            m_pieCharts = New PieChartOutputs(PercentageTotalDepositionVolumePiePath, PercentageTotalErosionVolumePiePath, PercentageTotalVolumePiePath)

            For Each sMaskName As String In MaskLabels.Keys
                Dim nMaskValue As Integer = MaskLabels(sMaskName)

                Dim SafeMaskName As String = naru.os.File.RemoveDangerousCharacters(sMaskName)
                Dim AreaChartPath As String = IO.Path.Combine(BS_FiguresOutputFolder, String.Format("c{0}_{1}_Area.png", nMaskValue.ToString("000"), SafeMaskName))
                Dim VolumeChartPath As String = IO.Path.Combine(BS_FiguresOutputFolder, String.Format("c{0}_{1}_Volume.png", nMaskValue.ToString("000"), SafeMaskName))
                Dim csvFilename As String = IO.Path.Combine(BSOutputFolder, String.Format("c{0}_{1}_ElevDist.csv", nMaskValue.ToString("000"), SafeMaskName))
                Dim SummaryPath As String = IO.Path.Combine(BSOutputFolder, String.Format("c{0}_{1}.xml", nMaskValue.ToString("000"), SafeMaskName))

                Dim MaskOutput As New MaskOutputClass(nMaskValue, AreaChartPath, VolumeChartPath, csvFilename, SummaryPath)
                m_dMaskOutputs.Add(sMaskName, MaskOutput)
            Next

            m_sClassLegendPath = IO.Path.Combine(BSOutputFolder, "ClassLegend.csv")
            m_sClassSummaryPath = IO.Path.Combine(BSOutputFolder, "ClassSummary.xml")
            m_sPolygonMask = sPolygonMask

        End Sub

        Public Sub New(rBS As ProjectDS.BudgetSegregationsRow)

            Dim sPCDepositionVolumePiePath As String = GCDProject.ProjectManagerBase.GetAbsolutePath(rBS.PCDepositoonVolPie)
            Dim sPCErosionVolumePiePath As String = GCDProject.ProjectManagerBase.GetAbsolutePath(rBS.PCErosionVolPie)
            Dim sPCVolumePiePath As String = GCDProject.ProjectManagerBase.GetAbsolutePath(rBS.PCTotalVolPie)

            m_pieCharts = New PieChartOutputs(sPCDepositionVolumePiePath, sPCErosionVolumePiePath, sPCVolumePiePath)

            m_dMaskOutputs = New Dictionary(Of String, MaskOutputClass)
            For Each rMask As ProjectDS.BSMasksRow In rBS.GetBSMasksRows

                Dim AreaChartPath As String = GCDProject.ProjectManagerBase.GetAbsolutePath(rMask.AreaChartPath)
                Dim VolumeChartPath As String = GCDProject.ProjectManagerBase.GetAbsolutePath(rMask.VolumeChartPath)
                Dim csvFilename As String = GCDProject.ProjectManagerBase.GetAbsolutePath(rMask.CSVFileName)
                Dim SummaryPath As String = GCDProject.ProjectManagerBase.GetAbsolutePath(rMask.SummaryXMLPath)

                Dim MaskOutput As New MaskOutputClass(rMask.MaskValue, AreaChartPath, VolumeChartPath, csvFilename, SummaryPath)

                MaskOutput.ChangeStats = New GCDConsoleLib.DoDStats(rBS.DoDsRow.CellArea, rMask.AreaErosionRaw, rMask.AreaDepositionRaw,
                                                                    rMask.AreaErosionThresholded, rMask.AreaDepositionThresholded,
                                                                    rMask.VolumeErosionRaw, rMask.VolumeErosionThresholded,
                                                                    rMask.VolumeErosionThresholded, rMask.VolumeDepositionThresholded,
                                                                    rMask.VolumeErosionError, rMask.VolumeDepositionError)

                m_dMaskOutputs.Add(rMask.MaskName, MaskOutput)
            Next

            m_sClassLegendPath = GCDProject.ProjectManagerBase.GetAbsolutePath(rBS.ClassLegendPath)
            m_sClassSummaryPath = GCDProject.ProjectManagerBase.GetAbsolutePath(rBS.ClassSummaryPath)

        End Sub

        Public Class PieChartOutputs
            Private m_sPercentageTotalDepositionVolumePiePath As String
            Private m_sPercentageTotalErosionVolumePiePath As String
            Private m_sPercentageTotalVolumePiePath As String

            Public ReadOnly Property PercentageTotalDepositionVolumePiePath As String
                Get
                    Return m_sPercentageTotalDepositionVolumePiePath
                End Get
            End Property

            Public ReadOnly Property PercentageTotalErosionVolumePiePath As String
                Get
                    Return m_sPercentageTotalErosionVolumePiePath
                End Get
            End Property

            Public ReadOnly Property PercentageTotalVolumePiePath As String
                Get
                    Return m_sPercentageTotalVolumePiePath
                End Get
            End Property

            Public Sub New(ByVal PercentageTotalDepositionVolumePiePath As String,
                           ByVal PercentageTotalErosionVolumePiePath As String,
                           ByVal PercentageTotalVolumePiePath As String)
                m_sPercentageTotalDepositionVolumePiePath = PercentageTotalDepositionVolumePiePath
                m_sPercentageTotalErosionVolumePiePath = PercentageTotalErosionVolumePiePath
                m_sPercentageTotalVolumePiePath = PercentageTotalVolumePiePath
            End Sub
        End Class

        Public Class MaskOutputClass

            Private m_sAreaChartPath As String
            Private m_sVolumeChartPath As String
            Private m_sCSVFileName As String
            Private m_sClassValue As Integer
            Private m_sSummaryXMLPath As String

            Private m_changeStats As GCDConsoleLib.DoDStats
            Private m_DoDProps As ChangeDetection.DoDResult

#Region "Properties"

            Public ReadOnly Property AreaChartPath As String
                Get
                    Return m_sAreaChartPath
                End Get
            End Property

            Public ReadOnly Property VolumeChartPath As String
                Get
                    Return m_sVolumeChartPath
                End Get
            End Property

            Public ReadOnly Property csvFilename As String
                Get
                    Return m_sCSVFileName
                End Get
            End Property

            Public ReadOnly Property SummaryPath As String
                Get
                    Return m_sSummaryXMLPath
                End Get
            End Property

            Public ReadOnly Property MaskValue As Integer
                Get
                    Return m_sClassValue
                End Get
            End Property

            Public Property ChangeStats As GCDConsoleLib.DoDStats
                Get
                    Return m_changeStats
                End Get
                Set(value As GCDConsoleLib.DoDStats)
                    m_changeStats = value
                End Set
            End Property

            Public Property DoDProps As ChangeDetection.DoDResult
                Get
                    Return m_DoDProps
                End Get
                Set(value As ChangeDetection.DoDResult)
                    m_DoDProps = value
                End Set
            End Property
#End Region

            Public Sub New(nClassValue As Integer, sAreaChartPath As String, sVolumeChartPath As String, sCSVFileName As String, sSummaryXMLPath As String)
                m_sClassValue = nClassValue
                m_sAreaChartPath = sAreaChartPath
                m_sVolumeChartPath = sVolumeChartPath
                m_sCSVFileName = sCSVFileName
                m_sSummaryXMLPath = sSummaryXMLPath
            End Sub

        End Class
    End Class

End Namespace