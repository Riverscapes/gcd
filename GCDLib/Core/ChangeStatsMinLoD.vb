''' <summary>
''' Change statistics for a minimum level of detection change detection
''' </summary>
''' <remarks>See base class for the basic statistics. This class is responsible for
''' calculate the plus minus error using the method specific for MinLoD change detections.</remarks>
Public Class ChangeStatsMinLoD
    Inherits ChangeStats

    Protected Enum ChangeType
        Erosion
        Deposition
    End Enum

    ''' <summary>
    ''' Creates a new change statistics class for minimum level of detection change detections.
    ''' </summary>
    ''' <param name="sDodRaw">Raw DoD Raster Path</param>
    ''' <param name="sDodThresh">Thresholded DoD Raster Path</param>
    ''' <param name="fCellSize">Cell Resolution / Size</param>
    ''' <param name="fThreshold">Minimum Level of Detection Thresholded</param>
    ''' <remarks>See accompanying class for probabilistic and propagated error change detections</remarks>
    Public Sub New(sDodRaw As String, sDodThresh As String, fCellSize As Double, fThreshold As Double)
        MyBase.New(sDodRaw, sDodThresh, fCellSize)

        CalculatePlusMinusError(sDodRaw, fThreshold, ChangeType.Erosion, m_fVolumeErosion_Error)
        CalculatePlusMinusError(sDodRaw, fThreshold, ChangeType.Deposition, m_fVolumeErosion_Error)

    End Sub

    ''' <summary>
    ''' Calculate the plus minus error for MinLoD change detections
    ''' </summary>
    ''' <param name="sDodRaw">Raw DoD Raster Path</param>
    ''' <param name="fThreshold">Minimum Level of Detection Threshold</param>
    ''' <param name="eChangeType">Erosion or Deposition</param>
    ''' <param name="fError">Output error value</param>
    ''' <remarks></remarks>
    Protected Shadows Sub CalculatePlusMinusError(sDodRaw As String, fThreshold As Double, eChangeType As ChangeType, ByRef fError As Double)

        fError = 0

        Try
            '
            ' Calculate the erosion 
            Dim sMask As String = WorkspaceManager.GetTempRaster("Mask")
            Dim sDirection As String
            Dim nSign As Integer = 1

            If eChangeType = ChangeType.Erosion Then
                sDirection = "<"
                nSign = -1
            Else
                sDirection = ">"
            End If

            GP.SpatialAnalyst.Con_sa(sDodRaw, "1", New IO.FileInfo(sMask), "Value " & sDirection & " " & (fThreshold * nSign))
            ' deposition threshold: Dod VALUE > Threshold
            ' erosion threshold: Dod VALUE < Threshold * -1

            '
            ' Run zonal statistics on this mask to get the sum of all pixel values in the masked area.
            '
            Dim pWFS As ESRI.ArcGIS.Geodatabase.IWorkspaceFactory = GISCode.ArcMap.GetWorkspaceFactory(GISDataStructures.GISDataStorageTypes.ShapeFile)
            Dim pWorkspace As ESRI.ArcGIS.Geodatabase.IWorkspace = pWFS.OpenFromFile(WorkspaceManager.WorkspacePath & IO.Path.DirectorySeparatorChar, Nothing)
            Dim sErosionTable As String = GISCode.Table.GetSafeName(pWorkspace, "MaskVals")
            GISCode.GP.SpatialAnalyst.ZonalStatisticsAsTable(sMask, "Value", sDodRaw, sErosionTable, "SUM")

            Dim pTable As ESRI.ArcGIS.Geodatabase.ITable = GISCode.Table.Table.OpenDBFTable(sErosionTable)
            Dim nCountField As Integer = pTable.FindField("COUNT")

            Dim nCount As Integer
            Dim pCursor As ESRI.ArcGIS.Geodatabase.ICursor = pTable.Search(Nothing, True)
            Dim aRow As ESRI.ArcGIS.Geodatabase.IRow = pCursor.NextRow
            If TypeOf aRow Is ESRI.ArcGIS.Geodatabase.IRow Then
                If Not IsDBNull(aRow.Value(nCountField)) Then
                    nCount = aRow.Value(nCountField)
                    fError = CellArea * CType(nCount, Double) * Math.Abs(fThreshold)
                End If
            End If
            Runtime.InteropServices.Marshal.ReleaseComObject(pCursor)
            Runtime.InteropServices.Marshal.ReleaseComObject(pTable)
            Runtime.InteropServices.Marshal.ReleaseComObject(pWFS)
            Runtime.InteropServices.Marshal.ReleaseComObject(pWorkspace)

        Catch ex As Exception
            ex.Data("Raw DoD") = sDodRaw
            ex.Data("Threshold") = fThreshold.ToString
            ex.Data("Cell Resolution") = CellArea.ToString
            ex.Data("Erosion or Deposition") = eChangeType.ToString
            Throw
        End Try

    End Sub

End Class
