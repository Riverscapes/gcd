Imports GCDConsoleLib

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

        Protected Overrides Function ThresholdRawDoD(ByRef rawDoD As Raster, thrDoDPath As IO.FileInfo) As Raster

            Return RasterOperators.SetNullLessThan(rawDoD, Threshold, thrDoDPath)

        End Function

        Protected Overrides Function CalculateChangeStats(ByRef rawDoD As Raster, ByRef thrDoD As Raster) As DoDStats

            Return RasterOperators.GetStatsMinLoD(rawDoD, thrDoD, Threshold)

        End Function

        Protected Overrides Function GetDoDResult(ByRef changeStats As DoDStats, rawDoDPath As IO.FileInfo, thrDoDPath As IO.FileInfo, rawDoDHist As IO.FileInfo, thrDoDHist As IO.FileInfo, eUnits As UnitsNet.Units.LengthUnit) As DoDResult
            Return New DoDResultMinLoD(changeStats, rawDoDPath, rawDoDHist, thrDoDPath, thrDoDHist, Threshold)
        End Function



    End Class


End Namespace