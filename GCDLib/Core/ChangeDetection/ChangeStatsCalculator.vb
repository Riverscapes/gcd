Imports GCD.GCDLib.Core.Visualization

Namespace Core.ChangeDetection

    ''' <summary>
    ''' Statistics describing a pair of raw and thresholded DoD rasters
    ''' </summary>
    ''' <remarks>Created by Philip Bailey, 18 Feb 2014
    ''' Note that this class is pure virtual and cannot be created as a variable. You must
    ''' instead create variables of type ChangeStatsMinLoD (minimum level of detection) or 
    ''' ChangeStatsPropProb (propagated or probabilistic).</remarks>
    Public Class ChangeStatsCalculator
        Inherits ChangeStats

        Private m_sPrecisionFormatString As String = "#,##0.00"

#Region "External DLL Function Definitions"

        ''' <summary>
        ''' Calculate DoD raw and thresholded statistics from a MinLoD uncertainty analysis
        ''' </summary>
        ''' <param name="sDoDRawPath">Raw DoD Raster Path</param>
        ''' <param name="fThreshold">MinLoD threshold</param>
        ''' <param name="fAreaErosionRaw">(Output) Total Area of Erosion, Raw</param>
        ''' <param name="fAreaDepositonRaw">(Output) Total Area of Deposition, Raw</param>
        ''' <param name="fAreaErosionThr">(Output) Total Area of Erosion, Thresholded</param>
        ''' <param name="fAreaDepositionThr">(Output) Total Area of Depositon, Thresholded</param>
        ''' <param name="fVolErosionRaw">(Output) Total Volume of Erosion, Raw</param>
        ''' <param name="fVolDepositionRaw">(Output) Total Volume of Deposition, Raw</param>
        ''' <param name="fVolErosionThr">(Output) Total Volume of Erosion, Thresholded</param>
        ''' <param name="fVolDepositionThr">(Output) Total Volume of Deposition, Thresholded</param>
        ''' <param name="fVolErosionErr">(Output) Total Volume of Erosion Plus/Minus Error</param>
        ''' <param name="fVolDepositonErr">(Output) Total Volume of Deposition Plus/Minus Error</param>
        ''' <remarks>Calls into C++ DLL that iterates over the raw DoD raster using GDAL. This replaces
        ''' the old code that derived the statistics using ArcGIS geoprocessing. The RasterManager.dll
        ''' file must be in the same folder as the GCD AddIn DLL, as well as the GDAL DLLs.</remarks>
        <Runtime.InteropServices.DllImport(External.ExternalLibs.m_sRasterManagerDLLFileName)>
        Private Shared Sub GetDoDMinLoDStats(ByVal sDoDRawPath As String, fThreshold As Double,
                                        ByRef fAreaErosionRaw As Double,
                                        ByRef fAreaDepositonRaw As Double,
                                        ByRef fAreaErosionThr As Double,
                                        ByRef fAreaDepositionThr As Double,
                                        ByRef fVolErosionRaw As Double,
                                        ByRef fVolDepositionRaw As Double,
                                        ByRef fVolErosionThr As Double,
                                        ByRef fVolDepositionThr As Double,
                                        ByRef fVolErosionErr As Double,
                                        ByRef fVolDepositonErr As Double,
                                        sError As System.Text.StringBuilder)
        End Sub

        ''' <summary>
        ''' Calculate DoD raw and thresholded statistics from a Propagated error uncertainty analysis
        ''' </summary>
        ''' <param name="sDoDRawPath">Raw DoD Raster Path</param>
        ''' <param name="sPropagatedError">Propagated Error Raster Path</param>
        ''' <param name="fAreaErosionRaw">(Output) Total Area of Erosion, Raw</param>
        ''' <param name="fAreaDepositonRaw">(Output) Total Area of Deposition, Raw</param>
        ''' <param name="fAreaErosionThr">(Output) Total Area of Erosion, Thresholded</param>
        ''' <param name="fAreaDepositionThr">(Output) Total Area of Depositon, Thresholded</param>
        ''' <param name="fVolErosionRaw">(Output) Total Volume of Erosion, Raw</param>
        ''' <param name="fVolDepositionRaw">(Output) Total Volume of Deposition, Raw</param>
        ''' <param name="fVolErosionThr">(Output) Total Volume of Erosion, Thresholded</param>
        ''' <param name="fVolDepositionThr">(Output) Total Volume of Deposition, Thresholded</param>
        ''' <param name="fVolErosionErr">(Output) Total Volume of Erosion Plus/Minus Error</param>
        ''' <param name="fVolDepositonErr">(Output) Total Volume of Deposition Plus/Minus Error</param>
        ''' <remarks>Calls into C++ DLL that iterates over the raw DoD raster using GDAL. This replaces
        ''' the old code that derived the statistics using ArcGIS geoprocessing. The RasterManager.dll
        ''' file must be in the same folder as the GCD AddIn DLL, as well as the GDAL DLLs.</remarks>
        <Runtime.InteropServices.DllImport(External.ExternalLibs.m_sRasterManagerDLLFileName)>
        Private Shared Sub GetDoDPropStats(ByVal sDoDRawPath As String, sPropagatedError As String,
                                        ByRef fAreaErosionRaw As Double,
                                        ByRef fAreaDepositonRaw As Double,
                                        ByRef fAreaErosionThr As Double,
                                        ByRef fAreaDepositionThr As Double,
                                        ByRef fVolErosionRaw As Double,
                                        ByRef fVolDepositionRaw As Double,
                                        ByRef fVolErosionThr As Double,
                                        ByRef fVolDepositionThr As Double,
                                        ByRef fVolErosionErr As Double,
                                        ByRef fVolDepositonErr As Double,
                                        sError As System.Text.StringBuilder)
        End Sub

        ''' <summary>
        ''' Calculate DoD raw and thresholded statistics from a Probabilistic uncertainty analysis
        ''' </summary>
        ''' <param name="sDoDRawPath">Raw DoD Raster Path</param>
        ''' <param name="sDoDThrPath">Thresholded DoD Path</param>
        ''' <param name="sPropagatedErrorPath">Propagated Error Raster Path</param>
        ''' <param name="fAreaErosionRaw">(Output) Total Area of Erosion, Raw</param>
        ''' <param name="fAreaDepositonRaw">(Output) Total Area of Deposition, Raw</param>
        ''' <param name="fAreaErosionThr">(Output) Total Area of Erosion, Thresholded</param>
        ''' <param name="fAreaDepositionThr">(Output) Total Area of Depositon, Thresholded</param>
        ''' <param name="fVolErosionRaw">(Output) Total Volume of Erosion, Raw</param>
        ''' <param name="fVolDepositionRaw">(Output) Total Volume of Deposition, Raw</param>
        ''' <param name="fVolErosionThr">(Output) Total Volume of Erosion, Thresholded</param>
        ''' <param name="fVolDepositionThr">(Output) Total Volume of Deposition, Thresholded</param>
        ''' <param name="fVolErosionErr">(Output) Total Volume of Erosion Plus/Minus Error</param>
        ''' <param name="fVolDepositonErr">(Output) Total Volume of Deposition Plus/Minus Error</param>
        ''' <remarks>Calls into C++ DLL that iterates over the raw DoD raster using GDAL. This replaces
        ''' the old code that derived the statistics using ArcGIS geoprocessing. The RasterManager.dll
        ''' file must be in the same folder as the GCD AddIn DLL, as well as the GDAL DLLs.</remarks>
        <Runtime.InteropServices.DllImport(External.ExternalLibs.m_sRasterManagerDLLFileName)>
        Private Shared Sub GetDoDProbStats(ByVal sDoDRawPath As String, sDoDThrPath As String,
                                    ByVal sPropagatedErrorPath As String,
                                        ByRef fAreaErosionRaw As Double,
                                        ByRef fAreaDepositonRaw As Double,
                                        ByRef fAreaErosionThr As Double,
                                        ByRef fAreaDepositionThr As Double,
                                        ByRef fVolErosionRaw As Double,
                                        ByRef fVolDepositionRaw As Double,
                                        ByRef fVolErosionThr As Double,
                                        ByRef fVolDepositionThr As Double,
                                        ByRef fVolErosionErr As Double,
                                        ByRef fVolDepositonErr As Double,
                                        sError As System.Text.StringBuilder)
        End Sub

#End Region

        ' ''' <summary>
        ' ''' Creates a new change statistics class. Note: This is a non-instantiable class
        ' ''' </summary>
        ' '''<param name="dodProps">DoD Properties</param>
        ' ''' <remarks></remarks>
        'Public Sub New(dodProps As ChangeDetectionProperties)

        '    ' Test that the thresholded DoD and the propagated Error have real values.
        '    ' Note that these rasters might be masked for BS and so there might not be coincident
        '    ' areas within the mask and the thresholded DoD.
        '    Dim gThreshDoD As New GCDConsoleLib.Raster(dodProps.ThresholdedDoD)
        '    If Math.Abs(gThreshDoD.Maximum) = Double.MaxValue Then
        '        m_fVolumeErosion_Error = 0
        '        m_fVolumeDeposition_Error = 0
        '        Exit Sub
        '    End If

        '    CellArea = dodProps.CellSize ^ 2

        '    ErrorHandling.UpdateStatus("Calculating area and volume for raw and thresholded DoDs")
        '    CalculateStatistics(dodProps)

        'End Sub

        Public Sub New(ByRef dodProps As ChangeDetection.ChangeDetectionPropertiesMinLoD)

            Dim fAreaErosionRaw As Double
            Dim fAreaDepositonRaw As Double
            Dim fAreaErosionThr As Double
            Dim fAreaDepositionThr As Double
            Dim fVolErosionRaw As Double
            Dim fVolDepositionRaw As Double
            Dim fVolErosionThr As Double
            Dim fVolDepositionThr As Double
            Dim fVolErosionErr As Double
            Dim fVolDepositonErr As Double

            Debug.WriteLine("Getting DoD statistics for MinLoD thresholding:")
            Debug.WriteLine("        Raw: " & dodProps.RawDoD)
            Debug.WriteLine("Thresholded: " & dodProps.ThresholdedDoD)

            GetDoDMinLoDStats(dodProps.RawDoD, dodProps.Threshold,
                            fAreaErosionRaw, fAreaDepositonRaw,
                            fAreaErosionThr, fAreaDepositionThr,
                            fVolErosionRaw, fVolDepositionRaw,
                            fVolErosionThr, fVolDepositionThr,
                            fVolErosionErr, fVolDepositonErr,
                            GCDProject.ProjectManagerBase.GCDNARCError.ErrorString)

            AreaErosion_Raw = fAreaErosionRaw
            AreaDeposition_Raw = fAreaDepositonRaw
            AreaErosion_Thresholded = fAreaErosionThr
            AreaDeposition_Thresholded = fAreaDepositionThr

            VolumeErosion_Raw = fVolErosionRaw
            VolumeDeposition_Raw = fVolDepositionRaw
            VolumeErosion_Thresholded = fVolErosionThr
            VolumeDeposition_Thresholded = fVolDepositionThr

            VolumeErosion_Error = fVolErosionErr
            VolumeDeposition_Error = fVolDepositonErr

            CellArea = dodProps.CellSize ^ 2

        End Sub

        Public Sub New(ByRef dodProps As ChangeDetection.ChangeDetectionPropertiesPropagated)

            Dim fAreaErosionRaw As Double
            Dim fAreaDepositonRaw As Double
            Dim fAreaErosionThr As Double
            Dim fAreaDepositionThr As Double
            Dim fVolErosionRaw As Double
            Dim fVolDepositionRaw As Double
            Dim fVolErosionThr As Double
            Dim fVolDepositionThr As Double
            Dim fVolErosionErr As Double
            Dim fVolDepositonErr As Double

            Debug.WriteLine("Getting DoD statistics for propagated error thresholding:")
            Debug.WriteLine("        Raw: " & dodProps.RawDoD)
            Debug.WriteLine("Thresholded: " & dodProps.ThresholdedDoD)
            Debug.WriteLine(" Propagated: " & dodProps.PropagatedErrorRaster)

            GetDoDPropStats(dodProps.RawDoD, dodProps.PropagatedErrorRaster,
                            fAreaErosionRaw, fAreaDepositonRaw,
                            fAreaErosionThr, fAreaDepositionThr,
                            fVolErosionRaw, fVolDepositionRaw,
                            fVolErosionThr, fVolDepositionThr,
                            fVolErosionErr, fVolDepositonErr,
                            GCDProject.ProjectManagerBase.GCDNARCError.ErrorString)

            AreaErosion_Raw = fAreaErosionRaw
            AreaDeposition_Raw = fAreaDepositonRaw
            AreaErosion_Thresholded = fAreaErosionThr
            AreaDeposition_Thresholded = fAreaDepositionThr

            VolumeErosion_Raw = fVolErosionRaw
            VolumeDeposition_Raw = fVolDepositionRaw
            VolumeErosion_Thresholded = fVolErosionThr
            VolumeDeposition_Thresholded = fVolDepositionThr

            VolumeErosion_Error = fVolErosionErr
            VolumeDeposition_Error = fVolDepositonErr

            CellArea = dodProps.CellSize ^ 2

        End Sub

        Public Sub New(ByRef dodProps As ChangeDetection.ChangeDetectionPropertiesProbabilistic)

            Dim fAreaErosionRaw As Double
            Dim fAreaDepositonRaw As Double
            Dim fAreaErosionThr As Double
            Dim fAreaDepositionThr As Double
            Dim fVolErosionRaw As Double
            Dim fVolDepositionRaw As Double
            Dim fVolErosionThr As Double
            Dim fVolDepositionThr As Double
            Dim fVolErosionErr As Double
            Dim fVolDepositonErr As Double

            Debug.WriteLine("Getting DoD statistics for probabilistic error thresholding:")
            Debug.WriteLine("        Raw: " & dodProps.RawDoD)
            Debug.WriteLine("Thresholded: " & dodProps.ThresholdedDoD)
            Debug.WriteLine(" Propagated: " & dodProps.PropagatedErrorRaster)

            GetDoDProbStats(dodProps.RawDoD, dodProps.ThresholdedDoD,
                            dodProps.PropagatedErrorRaster,
                            fAreaErosionRaw, fAreaDepositonRaw,
                            fAreaErosionThr, fAreaDepositionThr,
                            fVolErosionRaw, fVolDepositionRaw,
                            fVolErosionThr, fVolDepositionThr,
                            fVolErosionErr, fVolDepositonErr,
                            GCDProject.ProjectManagerBase.GCDNARCError.ErrorString)

            AreaErosion_Raw = fAreaErosionRaw
            AreaDeposition_Raw = fAreaDepositonRaw
            AreaErosion_Thresholded = fAreaErosionThr
            AreaDeposition_Thresholded = fAreaDepositionThr

            VolumeErosion_Raw = fVolErosionRaw
            VolumeDeposition_Raw = fVolDepositionRaw
            VolumeErosion_Thresholded = fVolErosionThr
            VolumeDeposition_Thresholded = fVolDepositionThr

            VolumeErosion_Error = fVolErosionErr
            VolumeDeposition_Error = fVolDepositonErr

            CellArea = dodProps.CellSize ^ 2

        End Sub

        'Private Sub CalculateStatistics(ByRef dodProps As ChangeDetection.ChangeDetectionProperties)
        '    '
        '    ' This should not be possible. You can only calculate change statistics for
        '    ' one of the three change detection types.
        '    Throw New Exception("Invalid use of change statistics class. Cannot calculate statistics for base class GCd.ChangeDetection.ChangeDetectionProperties")

        'End Sub
        ' ''' <summary>
        ' ''' Calculates the basic area and volume statistics. Use the Con Statement to select either erosion or deposition cells.
        ' ''' </summary>
        ' ''' <param name="sRasterPath">DoD Raster Path (can be either raw or thresholded)</param>
        ' ''' <param name="sConStatement">""Value"" &lt; 0" for erosion and """Value"" &gt; 0" for deposition</param>
        ' ''' <param name="fCellArea">Area of one cell</param>
        ' ''' <param name="fArea">Output value of the area of cells that meet the condition of the Con Statement</param>
        ' ''' <param name="fVolume">Output value of the volume of cells that meet the condition of the Con Statement</param>
        ' ''' <remarks></remarks>
        'Private Sub CalculateAreaAndVolume(sRasterPath As String, sConStatement As String, fCellArea As Double, ByRef fArea As Double, ByRef fVolume As Double)

        '    fArea = 0
        '    fVolume = 0

        '    Dim sZoneRaster As String = WorkspaceManager.GetTempRaster("TempStats")
        '    GP.SpatialAnalyst.Con_sa(sRasterPath, "1", "0", New IO.FileInfo(sZoneRaster), sConStatement)

        '    Dim pWS As ESRI.ArcGIS.Geodatabase.IWorkspace = GISDataStructures.GetWorkspace(WorkspaceManager.WorkspacePath & IO.Path.DirectorySeparatorChar, GISDataStructures.GISDataStorageTypes.ShapeFile)
        '    Dim sStatistics As String = GISCode.Table.GetSafeName(pWS, "ChangeStats")

        '    GP.SpatialAnalyst.ZonalStatisticsAsTable(sZoneRaster, "Value", sRasterPath, sStatistics, "ALL")
        '    Dim pTable As ESRI.ArcGIS.Geodatabase.ITable = GISCode.Table.OpenDBFTable(sStatistics)

        '    Dim dicSum As Dictionary(Of Integer, Double) = GISCode.Table.GetValuesFromTable(pTable, "Value", "SUM")
        '    Dim dicArea As Dictionary(Of Integer, Double) = GISCode.Table.GetValuesFromTable(pTable, "Value", "AREA")

        '    If dicArea.ContainsKey(1) Then
        '        fArea = dicArea(1)
        '        If dicSum.ContainsKey(1) Then
        '            ' Need to do abs because the sum of values is negative when erosion
        '            fVolume = Math.Abs(dicSum(1)) * CellArea
        '        End If
        '    End If

        '    Runtime.InteropServices.Marshal.ReleaseComObject(pTable)
        '    Runtime.InteropServices.Marshal.ReleaseComObject(pWS)

        'End Sub

        ''' <summary>
        ''' Write the statistics to a new copy of the GCDSummary.xml spreadsheet XML file.
        ''' </summary>
        ''' <param name="eUnits">Units of measurement for the statistics. e.g. "ft"</param>
        ''' <param name="sOutputPath">Full path to the output XML path where the GCD Summary stats XML file should be generated</param>
        ''' <remarks></remarks>
        Public Sub ExportSummary(sExcelTemplateFolder As String, eUnits As UnitsNet.Units.LengthUnit, ByVal sOutputPath As String)

            Dim TemplateFile As String = IO.Path.Combine(sExcelTemplateFolder, "GCDSummary.xml")
            If Not IO.File.Exists(TemplateFile) Then
                Throw New Exception("Could not find required template '" & TemplateFile & "'")
            End If

            ' Read the template file
            Dim objReader As System.IO.StreamReader
            Try
                objReader = New System.IO.StreamReader(TemplateFile)
            Catch ex As System.IO.IOException
                'need to limit the length of the file to display properly
                Dim TrimmedFilename As String = naru.os.File.TrimFilename(TemplateFile, 80)
                ex.Data("UIMessage") = "Could not access the file '" & TrimmedFilename & "' because it is being used by another program."
                Throw
            End Try

            Dim OutputText As Text.StringBuilder = New Text.StringBuilder(objReader.ReadToEnd())
            objReader.Close()
            '
            ' PGB 3 Mar 2012
            ' Need to dynamically put the units in the summary XML.
            '
            OutputText.Replace("[LinearUnits]", eUnits.GetUnitsAsString)

            OutputText.Replace("[TotalAreaOfErosionRaw]", AreaErosion_Raw)
            OutputText.Replace("[TotalAreaOfErosionThresholded]", AreaErosion_Thresholded)

            OutputText.Replace("[TotalAreaOfDepositionRaw]", AreaDeposition_Raw)
            OutputText.Replace("[TotalAreaOfDepositionThresholded]", AreaDeposition_Thresholded)

            OutputText.Replace("[TotalVolumeOfErosionRaw]", VolumeErosion_Raw)
            OutputText.Replace("[TotalVolumeOfErosionThresholded]", VolumeErosion_Thresholded)
            OutputText.Replace("[ErrorVolumeOfErosion]", VolumeErosion_Error)

            OutputText.Replace("[TotalVolumeOfDepositionRaw]", VolumeDeposition_Raw)
            OutputText.Replace("[TotalVolumeOfDepositionThresholded]", VolumeDeposition_Thresholded)
            OutputText.Replace("[ErrorVolumeOfDeposition]", VolumeDeposition_Error)

            ' Write the template file
            Dim objWriter As New System.IO.StreamWriter(sOutputPath)
            objWriter.Write(OutputText)
            objWriter.Close()

        End Sub

        Public Sub GenerateChangeBarGraphicFiles(ByVal sFiguresFolder As String, ByVal eLinearUnit As UnitsNet.Units.LengthUnit, ByVal fChartWidth As Double, ByVal fChartHeight As Double, Optional ByVal sFilePrefix As String = "")

            Dim nDPI As Integer = 300
            Dim chtControl As New System.Windows.Forms.DataVisualization.Charting.Chart
            Dim barViewer As New ElevationChangeBarViewer(chtControl, eLinearUnit)

            If Not String.IsNullOrEmpty(sFilePrefix) Then
                If Not sFilePrefix.EndsWith("_") Then
                    sFilePrefix &= "_"
                End If
            End If

            barViewer.Refresh(AreaErosion_Thresholded, AreaDeposition_Thresholded, eLinearUnit, ElevationChangeBarViewer.BarTypes.Area, True)
            barViewer.Save(IO.Path.Combine(sFiguresFolder, sFilePrefix & "ChangeBars_AreaAbsolute.png"), fChartWidth, fChartHeight, nDPI)

            barViewer.Refresh(AreaErosion_Thresholded, AreaDeposition_Thresholded, eLinearUnit, ElevationChangeBarViewer.BarTypes.Area, False)
            barViewer.Save(IO.Path.Combine(sFiguresFolder, sFilePrefix & "ChangeBars_AreaRelative.png"), fChartWidth, fChartHeight, nDPI)

            barViewer.Refresh(VolumeErosion_Thresholded, VolumeDeposition_Thresholded, NetVolumeOfDifference_Thresholded, VolumeErosion_Error, VolumeDeposition_Error, NetVolumeOfDifference_Error, eLinearUnit, ElevationChangeBarViewer.BarTypes.Volume, True)
            barViewer.Save(IO.Path.Combine(sFiguresFolder, sFilePrefix & "ChangeBars_VolumeAbsolute.png"), fChartWidth, fChartHeight, nDPI)

            barViewer.Refresh(VolumeErosion_Thresholded, VolumeDeposition_Thresholded, NetVolumeOfDifference_Thresholded, VolumeErosion_Error, VolumeDeposition_Error, NetVolumeOfDifference_Error, eLinearUnit, ElevationChangeBarViewer.BarTypes.Volume, False)
            barViewer.Save(IO.Path.Combine(sFiguresFolder, sFilePrefix & "ChangeBars_VolumeRelative.png"), fChartWidth, fChartHeight, nDPI)

            barViewer.Refresh(AverageDepthErosion_Thresholded, AverageDepthDeposition_Thresholded, AverageNetThicknessOfDifferenceADC_Thresholded, AverageDepthErosion_Error, AverageDepthDeposition_Error, AverageThicknessOfDifferenceADC_Error, eLinearUnit, ElevationChangeBarViewer.BarTypes.Vertical, True)
            barViewer.Save(IO.Path.Combine(sFiguresFolder, sFilePrefix & "ChangeBars_DepthAbsolute.png"), fChartWidth, fChartHeight, nDPI)

            barViewer.Refresh(AverageDepthErosion_Thresholded, AverageDepthDeposition_Thresholded, AverageNetThicknessOfDifferenceADC_Thresholded, AverageDepthErosion_Error, AverageDepthDeposition_Error, AverageThicknessOfDifferenceADC_Error, eLinearUnit, ElevationChangeBarViewer.BarTypes.Vertical, False)
            barViewer.Save(IO.Path.Combine(sFiguresFolder, sFilePrefix & "ChangeBars_DepthRelative.png"), fChartWidth, fChartHeight, nDPI)

        End Sub

    End Class

End Namespace