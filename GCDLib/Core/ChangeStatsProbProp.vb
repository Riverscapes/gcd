''' <summary>
''' Represents the change statistics for a probabilistic or propagated error 
''' change detection
''' </summary>
''' <remarks>Philip Bailey - 18 Feb 2014.
''' Replaces Frank's old StatsDataClass that calculates values from histogram CSVs and 
''' produces erroneous values.</remarks>
Public Class ChangeStatsProbProp
    Inherits ChangeStats

    ''' <summary>
    ''' Creates a new change statistics class for probabilistic and propagated error 
    ''' change detections.
    ''' </summary>
    ''' <param name="sDodRaw">Raw DoD raster path</param>
    ''' <param name="sDodThresh">Thresholded DoD raster path</param>
    ''' <param name="sPropErrorRasterPath">Propoagated Error raster path</param>
    ''' <param name="fCellSize">Cell Resolution / Size</param>
    ''' <remarks>See related class for minimum level of detection change detections.</remarks>
    Public Sub New(sDodRaw As String, sDodThresh As String, sPropErrorRasterPath As String, fCellSize As Double)
        MyBase.New(sDodRaw, sDodThresh, fCellSize)

        CalculatePlusMinusError(sDodThresh, sPropErrorRasterPath)

    End Sub

    ''' <summary>
    ''' Calculates the volumetric plus / minus error using the correct method for propabilistic
    ''' and propagated error change detections
    ''' </summary>
    ''' <param name="sDodThresholded">Thresholded DoD raster</param>
    ''' <param name="sPropErrorRasterPath">Propoagated Error Raster</param>
    ''' <remarks></remarks>
    Protected Shadows Sub CalculatePlusMinusError(sDodThresholded As String, sPropErrorRasterPath As String)

        Try
            Dim sPropErrorErosion As String = WorkspaceManager.GetTempRaster("PropErrErosion")
            GP.SpatialAnalyst.Con_sa(sDodThresholded, "1", "0", New IO.FileInfo(sPropErrorErosion), """Value"" < 0")

            Dim sPropErrorDeposition As String = WorkspaceManager.GetTempRaster("PropErrDeposition")
            GP.SpatialAnalyst.Con_sa(sDodThresholded, "2", "0", New IO.FileInfo(sPropErrorDeposition), """Value"" > 0")

            Dim sMask1 As String = WorkspaceManager.GetTempRaster("ErosionDepositionMask1")
            GP.SpatialAnalyst.Raster_Calculator("(""" & sPropErrorErosion & """) + (""" & sPropErrorDeposition & """)", sMask1)

            Dim sMask2 As String = WorkspaceManager.GetTempRaster("ErosionDepositionMask2")
            GP.SpatialAnalyst.SetNull(sMask1, sMask1, sMask2, """Value"" = 0")

            Dim gMask2 As New GISDataStructures.Raster(sMask2)
            Dim pWS As ESRI.ArcGIS.Geodatabase.IWorkspace = GISDataStructures.GetWorkspace(WorkspaceManager.WorkspacePath & IO.Path.DirectorySeparatorChar, GISDataStructures.GISDataStorageTypes.ShapeFile)

            Dim sStatistics As String = GISCode.Table.GetSafeName(pWS, "Stats")
            GP.SpatialAnalyst.ZonalStatisticsAsTable(sMask2, "Value", sPropErrorRasterPath, sStatistics, "ALL")
            Dim pTable As ESRI.ArcGIS.Geodatabase.ITable = GISCode.Table.OpenDBFTable(sStatistics)

            Dim dicSum As Dictionary(Of Integer, Double) = GISCode.Table.GetValuesFromTable(pTable, "Value", "SUM")
            Dim dicArea As Dictionary(Of Integer, Double) = GISCode.Table.GetValuesFromTable(pTable, "Value", "AREA")

            Runtime.InteropServices.Marshal.ReleaseComObject(pTable)
            Runtime.InteropServices.Marshal.ReleaseComObject(pWS)

            If dicSum.ContainsKey(1) AndAlso dicArea.ContainsKey(1) Then
                m_fVolumeErosion_Error = dicSum(1) * CellArea
            End If

            If dicSum.ContainsKey(2) AndAlso dicArea.ContainsKey(2) Then
                m_fVolumeDeposition_Error = dicSum(2) * CellArea
            End If

        Catch ex As Exception
            ex.Data("Thresholded DoD") = sDodThresholded
            ex.Data("Propagated Error") = sPropErrorRasterPath
            Throw
        Finally
        End Try
    End Sub

End Class
