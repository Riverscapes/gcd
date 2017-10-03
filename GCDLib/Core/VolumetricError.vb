
Namespace GISCode.GCD

    ''' <summary>
    ''' Calculates the plus minus error on thresholded volumetric change detection results
    ''' </summary>
    ''' <remarks>Frank's initial implementation of these values was based on the histogram files
    ''' associated with a DoD. This produced weird, inaccurate results because of binning. This
    ''' class attempts to calculate the correct values directly from the DoD raster</remarks>
    Public Class VolumetricError

        Private m_fPMTotalVolumeOfErosion
        Private m_fPMTotalVolumeofDeposition

#Region "Properties"
        Public ReadOnly Property PMTotalVolumeOfErosion As Double
            Get
                Return m_fPMTotalVolumeOfErosion
            End Get
        End Property

        Public ReadOnly Property PMTotalVolumeOfDeposition As Double
            Get
                Return m_fPMTotalVolumeofDeposition
            End Get
        End Property

        Public ReadOnly Property PMTotalVolumeOfDifference As Double
            Get
                Return (m_fPMTotalVolumeOfErosion + m_fPMTotalVolumeofDeposition)
            End Get
        End Property

        Public ReadOnly Property PMTotalNetVolumeDifference As Double
            Get
                Return Math.Sqrt(m_fPMTotalVolumeOfErosion ^ 2 + m_fPMTotalVolumeofDeposition ^ 2)
            End Get
        End Property
#End Region

        Public Sub New(sThresholdedDoD As String, sPropagatedError As String, fCellSize As Double)

            Try
                If Not GISDataStructures.RasterGDAL.Exists(sThresholdedDoD) Then
                    Dim ex As New Exception("The thresholded DoD raster does not exist")
                    Throw ex
                End If

                If Not GISDataStructures.RasterGDAL.Exists(sPropagatedError) Then
                    Dim ex As New Exception("The propagated error raster does not exist")
                    Throw ex
                End If

                Dim sPropErrorErosion As String = WorkspaceManager.GetTempRaster("PropErrErosion")
                GP.SpatialAnalyst.Con_sa(sThresholdedDoD, "1", "0", New IO.FileInfo(sPropErrorErosion), """Value"" < 0")

                Dim sPropErrorDeposition As String = WorkspaceManager.GetTempRaster("PropErrDeposition")
                GP.SpatialAnalyst.Con_sa(sThresholdedDoD, "2", "0", New IO.FileInfo(sPropErrorDeposition), """Value"" > 0")

                Dim sMask1 As String = WorkspaceManager.GetTempRaster("ErosionDepositionMask1")
                GP.SpatialAnalyst.Raster_Calculator("(""" & sPropErrorErosion & """) + (""" & sPropErrorDeposition & """)", sMask1)

                Dim sMask2 As String = WorkspaceManager.GetTempRaster("ErosionDepositionMask2")
                GP.SpatialAnalyst.SetNull(sMask1, sMask1, sMask2, """Value"" = 0")

                Dim gMask2 As New GISDataStructures.Raster(sMask2)

                Dim pWSF As ESRI.ArcGIS.Geodatabase.IWorkspaceFactory = New ESRI.ArcGIS.DataSourcesFile.ShapefileWorkspaceFactory
                Dim pWS As ESRI.ArcGIS.Geodatabase.IWorkspace = pWSF.OpenFromFile(WorkspaceManager.WorkspacePath, Nothing)

                Dim sStatistics As String = GISCode.Table.GetSafeName(pWS, "Stats")
                GP.SpatialAnalyst.ZonalStatisticsAsTable(sMask2, "Value", sPropagatedError, sStatistics, "ALL")
                Dim pTable As ESRI.ArcGIS.Geodatabase.ITable = GISCode.Table.OpenDBFTable(sStatistics)

                Dim dicSum As Dictionary(Of Integer, Double) = GISCode.Table.GetValuesFromTable(pTable, "Value", "SUM")
                Dim dicArea As Dictionary(Of Integer, Double) = GISCode.Table.GetValuesFromTable(pTable, "Value", "AREA")

                If dicSum.ContainsKey(1) AndAlso dicArea.ContainsKey(1) Then
                    m_fPMTotalVolumeOfErosion = dicSum(1) * (fCellSize ^ 2) ' dicArea(1)
                End If

                If dicSum.ContainsKey(2) AndAlso dicArea.ContainsKey(2) Then
                    m_fPMTotalVolumeofDeposition = dicSum(2) * (fCellSize ^ 2)  ' dicArea(2)
                End If

            Catch ex As Exception
                ex.Data.Add("Thresholded DoD", sThresholdedDoD)
                ex.Data.Add("Propagated Error ", sPropagatedError)
                Throw
            End Try

        End Sub

    End Class

End Namespace
