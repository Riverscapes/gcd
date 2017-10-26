Namespace Core.ChangeDetection

    Public Class ChangeDetectionEngineMinLOD
        Inherits ChangeDetectionEngineBase

        Private m_fThreshold As Double

        Public ReadOnly Property Threshold As Double
            Get
                Return m_fThreshold
            End Get
        End Property

        Public Sub New(ByVal sName As String, ByVal sFolder As String, ByVal gNewDEM As GCDConsoleLib.Raster, ByVal gOldDEM As GCDConsoleLib.Raster,
                       ByVal gAOI As GCDConsoleLib.Vector, ByVal fThreshold As Double, ByVal fChartWidth As Integer, ByVal fChartHeight As Integer)
            MyBase.New(sName, sFolder, gNewDEM, gOldDEM, gAOI, fChartHeight, fChartWidth)

            m_fThreshold = fThreshold
        End Sub

        Protected Overrides Function ThresholdRawDoD(rawDoDPath As String, rawHistPath As String) As DoDResult

            ' Threshold the raster
            Dim thrDoDPath As String = GCDProject.ProjectManagerBase.OutputManager.GetDoDThresholdPath(Name, Folder.FullName)
            External.ThresholdDoDMinLoD(rawDoDPath, thrDoDPath, m_fThreshold, GCDProject.ProjectManagerBase.OutputManager.OutputDriver,
                GCDProject.ProjectManagerBase.OutputManager.NoData, GCDProject.ProjectManagerBase.GCDNARCError.ErrorString)

            ' Generate the thresholded histograms
            Dim thrHistPath As String = GCDProject.ProjectManagerBase.OutputManager.GetCsvThresholdPath(Name, Folder.FullName)
            External.CalculateAndWriteDoDHistogram(thrDoDPath, thrHistPath, GCDProject.ProjectManagerBase.GCDNARCError.ErrorString)

            Return New DoDResultMinLoD(rawDoDPath, rawHistPath, thrDoDPath, thrHistPath, m_fThreshold, CellSize, LinearUnits)

        End Function

    End Class


End Namespace