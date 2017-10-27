Imports GCDLib.Core.Visualization
Imports GCDConsoleLib

Namespace Core.ChangeDetection

    Public Enum UncertaintyTypes
        MinLoD
        Propagated
        Probabilistic
    End Enum

    Public MustInherit Class ChangeDetectionEngineBase

        Private m_sAnalysisName As String
        Private m_dAnalysisFolder As IO.DirectoryInfo

        ' These are the original survey DEMs. May be non-concurrent
        Private m_gOriginalNewDEM As GCDConsoleLib.Raster
        Private m_gOriginalOldDEM As GCDConsoleLib.Raster

        ' This is a polymorphic class. Call calculate and it will calculate
        ' the statistics different ways depending on the type instantiated.
        'Protected m_ChangeStats As ChangeStats

        Protected m_nNumBins As Integer
        Protected m_nMinimumBin As Integer
        Protected m_fBinSize As Double
        Protected m_fBinIncrement As Double

        Private m_fChartHeight As Integer
        Private m_fChartWidth As Integer

#Region "Properties"

        Public ReadOnly Property Name As String
            Get
                Return m_sAnalysisName
            End Get
        End Property

        Public ReadOnly Property Folder As IO.DirectoryInfo
            Get
                Return m_dAnalysisFolder
            End Get
        End Property

        Public ReadOnly Property OriginalNewDEM As GCDConsoleLib.Raster
            Get
                Return m_gOriginalNewDEM
            End Get
        End Property

        Public ReadOnly Property OriginalOldDEM As GCDConsoleLib.Raster
            Get
                Return m_gOriginalOldDEM
            End Get
        End Property

        Protected ReadOnly Property ChartHeight As Integer
            Get
                Return m_fChartHeight
            End Get
        End Property

        Protected ReadOnly Property ChartWidth As Integer
            Get
                Return m_fChartWidth
            End Get
        End Property

        Protected ReadOnly Property LinearUnits As UnitsNet.Units.LengthUnit
            Get
                Return OriginalNewDEM.Proj.LinearUnit
            End Get
        End Property

#End Region

        Public Sub New(ByVal sName As String, ByVal sFolder As String, ByVal gNewDEM As Raster, ByVal gOldDEM As Raster,
                       ByVal fChartHeight As Integer, ByVal fChartWidth As Integer)

            If String.IsNullOrEmpty(sName) Then
                Throw New ArgumentNullException("sName", "The change detection analysis name cannot be empty.")
            End If
            m_sAnalysisName = sName

            If String.IsNullOrEmpty(sFolder) Then
                Throw New ArgumentNullException("sFolder", "The change detection analysis folder cannot be empty.")
            Else
                If IO.Directory.Exists(sFolder) Then
                    Dim ex As New Exception("The change detection analysis folder already exists.")
                    ex.Data("Folder") = sFolder
                    Throw ex
                End If
            End If
            m_dAnalysisFolder = New IO.DirectoryInfo(sFolder)

            If Not gNewDEM.Extent.HasOverlap(gOldDEM.Extent) Then
                Dim ex As New Exception("The two rasters do not overlap.")
                ex.Data("New DEM Path") = gNewDEM.FilePath
                ex.Data("New DEM Extent") = gNewDEM.Extent.ToString()
                ex.Data("Old DEM Path") = gOldDEM.FilePath
                ex.Data("Old DEM Extent") = gOldDEM.Extent.ToString()
            End If

            m_gOriginalNewDEM = gNewDEM
            m_gOriginalOldDEM = gOldDEM

            m_fChartHeight = fChartHeight
            m_fChartWidth = fChartWidth

        End Sub

        Public Function Calculate(rawDoDPath As IO.FileInfo, ByRef thrDodPath As IO.FileInfo, ByRef rawHistPath As IO.FileInfo, ByRef thrHistPath As IO.FileInfo,
                                  ByRef summaryXMLPath As IO.FileInfo, numberHistogramBins As Integer) As DoDResult

            ' Substrat the new and old rasters to produce the raw DoD
            Dim rawDoD As Raster = RasterOperators.Subtract(m_gOriginalNewDEM, m_gOriginalOldDEM, rawDoDPath)

            ' Calculate the raw histogram
            Dim rawHisto As Dictionary(Of Double, HistogramBin) = RasterOperators.BinRaster(rawDoD, numberHistogramBins)

            ' Write the raw histogram
            HistogramBin.WriteHistogram(rawHisto, rawHistPath)

            ' Call the polymorphic method to threshold the DoD depending on the thresholding method
            Dim thrDoD As Raster = ThresholdRawDoD(rawDoD, thrDodPath)

            ' Calculate the thresholded histogram
            Dim thrHisto As Dictionary(Of Double, HistogramBin) = RasterOperators.BinRaster(thrDoD, numberHistogramBins)

            ' Write the thresholded histogram
            HistogramBin.WriteHistogram(thrHisto, thrHistPath)

            ' Calculate the change statistics and write the output files
            Dim changeStats = CalculateChangeStats(rawDoD, thrDoD)
            GenerateSummaryXML(changeStats, summaryXMLPath, LinearUnits)
            GenerateChangeBarGraphicFiles(changeStats, m_fChartWidth, m_fChartHeight)
            GenerateHistogramGraphicFiles(rawHistPath, thrHistPath, m_fChartWidth, m_fChartHeight)


            Return New DoDResultMinLoD(rawDoDPath, rawHistPath, thrDodPath, thrHistPath, m_fThreshold, LinearUnits)






            Return dodResults

        End Function

        Protected MustOverride Function ThresholdRawDoD(ByRef rawDoD As Raster, thrDoDPath As IO.FileInfo) As Raster

        Protected MustOverride Function CalculateChangeStats(ByRef rawDoD As Raster, ByRef thrDoD As Raster) As DoDStats

        Protected MustOverride Function GetDoDResult(rawDoDPath As IO.FileInfo, thrDoDPath As IO.FileInfo, rawDoDHist As IO.FileInfo, thrDoDHist As IO.FileInfo, eUnits As UnitsNet.Units.LengthUnit) As DoDResult

        ''' <summary>
        ''' Makes new copies of the original DEM rasters that are clipped either to the AOI or concurrent to each other.
        ''' </summary>
        ''' <remarks>Should be overridden for Propagated and Probalistic so that the error
        ''' rasters can also be made concurrent with the DEMs.</remarks>
        Protected Function GenerateAnalysisRasters() As GCDConsoleLib.ExtentRectangle

            Debug.Print("Generating analysis DEM rasters")

            m_gAnalysisNewDEM = Nothing
            m_gAnalysisOldDEM = Nothing

            If TypeOf m_gAOI Is GCDConsoleLib.Vector Then

                ' Mask the rasters using the specified AOI
                Dim sAOIRaster As String = WorkspaceManager.GetTempRaster("AOI")
                Throw New NotImplementedException
                'GP.Conversion.PolygonToRaster_conversion(m_gAOI, m_gAOI.OIDFieldName, sAOIRaster, m_gOriginalNewDEM)
                Debug.Print("AOI raster produced at:" & sAOIRaster)

                Throw New NotImplementedException
                ' GP.SpatialAnalyst.SetNull(sAOIRaster, m_gOriginalNewDEM.FullPath, sNewDEM, """VALUE"" IS NULL")
                Throw New NotImplementedException
                '  GP.SpatialAnalyst.SetNull(sAOIRaster, m_gOriginalOldDEM.FullPath, sOldDEM, """VALUE"" IS NULL")
            Else

                If m_gOriginalNewDEM.Extent.IsConcurrent(m_gOriginalOldDEM.Extent) Then
                    ' Rasters already concurrent. Simply use in situ
                    m_gAnalysisNewDEM = New GCDConsoleLib.Raster(m_gOriginalNewDEM.FilePath)
                    m_gAnalysisOldDEM = New GCDConsoleLib.Raster(m_gOriginalOldDEM.FilePath)
                Else
                    Dim sNewDEM As String = WorkspaceManager.GetTempRaster("NewDEM")
                    Dim sOldDEM As String = WorkspaceManager.GetTempRaster("OldDEM")

                    ' Make copies of the original rasters that are concurrent
                    Dim theUnionExtent As GCDConsoleLib.ExtentRectangle = m_gOriginalNewDEM.Extent.Union(m_gOriginalOldDEM.Extent)

                    Throw New NotImplementedException("commented out while Matt fixes raster operators")
                    'm_gAnalysisNewDEM = GCDConsoleLib.Internal..RasterOperators.RasterCopy.ExtendedCopy(m_gOriginalNewDEM, sNewDEM, theUnionExtent)
                    'm_gAnalysisOldDEM = GCDConsoleLib.RasterOperators.RasterCopy.ExtendedCopy(m_gOriginalOldDEM, sOldDEM, theUnionExtent)
                End If
            End If

            Debug.Print("Analysis New DEM produced at: " & m_gAnalysisNewDEM.FilePath)
            Debug.Print("Analysis Old DEM produced at: " & m_gAnalysisOldDEM.FilePath)
            Debug.Print("Analysis extent: " & m_gAnalysisNewDEM.Extent.ToString)

            ' Final check
            If Not m_gAnalysisNewDEM.Extent.IsConcurrent(m_gAnalysisOldDEM.Extent) Then
                Dim ex As New Exception("Failed to make analysis rasters concurrent.")
                ex.Data("Original New DEM Path") = m_gOriginalNewDEM.FilePath
                ex.Data("Original New DEM Extent") = m_gOriginalNewDEM.Extent.ToString
                ex.Data("Original Old DEM Path") = m_gOriginalOldDEM.FilePath
                ex.Data("Original Old DEM Extent") = m_gOriginalOldDEM.Extent.ToString
                ex.Data("Analysis New DEM Path") = m_gAnalysisNewDEM.FilePath
                ex.Data("AnalysisNew DEM Extent") = m_gAnalysisNewDEM.Extent.ToString
                ex.Data("AnalysisOld DEM Path") = m_gAnalysisOldDEM.FilePath
                ex.Data("AnalysisOld DEM Extent") = m_gAnalysisOldDEM.Extent.ToString
                Throw ex
            End If

            Return m_gAnalysisNewDEM.Extent

        End Function

        Protected Function CalculateRawDoD(ByRef sRawDoDPath As String, ByRef sRawHistogram As String) As String

            Try
                sRawDoDPath = Project.ProjectManagerBase.OutputManager.GetDoDRawPath(Name, m_dAnalysisFolder.FullName)


                sRawHistogram = Project.ProjectManagerBase.OutputManager.GetCsvRawPath(IO.Path.GetDirectoryName(sRawDoDPath), Name)
                eResult = External.CalculateAndWriteDoDHistogramWithBins(sRawDoDPath, sRawHistogram, m_nNumBins, m_nMinimumBin, m_fBinSize, m_fBinIncrement, Project.ProjectManagerBase.GCDNARCError.ErrorString)
                If eResult <> External.GCDCoreOutputCodes.PROCESS_OK Then
                    Dim ex As New Exception(Project.ProjectManagerBase.GCDNARCError.ErrorString.ToString)
                    Throw New Exception("Error calculating and writing the raw DEM histogram.", ex)
                End If

            Catch ex As Exception
                ex.Data("Raw DoD Raster Path") = sRawDoDPath
                ex.Data("Raw DoD Histogram Path") = sRawHistogram
                Throw
            End Try

            Return sRawDoDPath

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="changeStats"></param>
        ''' <returns></returns>
        ''' <remarks>This method is needed by budget segregation as well</remarks>
        Public Shared Function GenerateSummaryXML(ByRef changeStats As DoDStats, outputPath As IO.FileInfo, linearUnits As UnitsNet.Units.LengthUnit) As String

            Dim templatePath As String = IO.Path.Combine(Project.ProjectManagerBase.ExcelTemplatesFolder.FullName, "GCDSummary.xml")
            Dim outputText As Text.StringBuilder

            Try
                Using objReader As New System.IO.StreamReader(templatePath)
                    outputText = New Text.StringBuilder(objReader.ReadToEnd())
                End Using
            Catch ex As Exception
                Dim ex2 As New Exception("Error reading the GCD summary XML template file", ex)
                ex.Data("Excel Template Path") = templatePath
                Throw ex2
            End Try

            outputText.Replace("[LinearUnits]", UnitsNet.Length.GetAbbreviation(linearUnits))

            outputText.Replace("[TotalAreaOfErosionRaw]", changeStats.AreaErosion_Raw)
            outputText.Replace("[TotalAreaOfErosionThresholded]", changeStats.AreaErosion_Thresholded)

            outputText.Replace("[TotalAreaOfDepositionRaw]", changeStats.AreaDeposition_Raw)
            outputText.Replace("[TotalAreaOfDepositionThresholded]", changeStats.AreaDeposition_Thresholded)

            outputText.Replace("[TotalVolumeOfErosionRaw]", changeStats.VolumeErosion_Raw)
            outputText.Replace("[TotalVolumeOfErosionThresholded]", changeStats.VolumeErosion_Thresholded)
            outputText.Replace("[ErrorVolumeOfErosion]", changeStats.VolumeErosion_Error)

            outputText.Replace("[TotalVolumeOfDepositionRaw]", changeStats.VolumeDeposition_Raw)
            outputText.Replace("[TotalVolumeOfDepositionThresholded]", changeStats.VolumeDeposition_Thresholded)
            outputText.Replace("[ErrorVolumeOfDeposition]", changeStats.VolumeDeposition_Error)

            Try
                Using objWriter As New IO.StreamWriter(outputPath)
                    objWriter.Write(outputText)
                End Using
            Catch ex As Exception
                Dim ex2 As New Exception("Error writing the GCD summary XML template file", ex)
                ex.Data("Excel Template Path") = templatePath
                Throw ex2
            End Try

            Return outputPath

        End Function

        Private Function GetFiguresFolder(bCreateIfMissing As Boolean) As String
            Return Project.ProjectManagerBase.OutputManager.GetChangeDetectionFiguresFolder(m_dAnalysisFolder.FullName, bCreateIfMissing)
        End Function

        Protected Sub GenerateHistogramGraphicFiles(rawHistogramPath As IO.FileInfo, threshHistogramPath As IO.FileInfo, ByVal fChartWidth As Double, ByVal fChartHeight As Double)

            Dim sFiguresFolder As String = GetFiguresFolder(True)
            Dim areaHistPath As String = IO.Path.Combine(sFiguresFolder, "Histogram_Area.png")
            Dim volhistPath As String = IO.Path.Combine(sFiguresFolder, "Histogram_Volume.png")

            Dim ExportHistogramViewer As New DoDHistogramViewerClass(rawHistogramPath, threshHistogramPath, LinearUnits)
            ExportHistogramViewer.ExportCharts(areaHistPath, volhistPath, fChartWidth, fChartHeight)
        End Sub

        Protected Sub GenerateChangeBarGraphicFiles(ByRef changeStats As GCDConsoleLib.DoDStats, ByVal fChartWidth As Double, ByVal fChartHeight As Double, Optional ByVal sFilePrefix As String = "")

            Dim barViewer As New Visualization.ElevationChangeBarViewer(LinearUnits)
            Dim sFiguresFolder As String = GetFiguresFolder(True)

            If Not String.IsNullOrEmpty(sFilePrefix) Then
                If Not sFilePrefix.EndsWith("_") Then
                    sFilePrefix &= "_"
                End If
            End If

            barViewer.Refresh(changeStats.AreaErosion_Thresholded, changeStats.AreaDeposition_Thresholded, LinearUnits, Visualization.ElevationChangeBarViewer.BarTypes.Area, True)
            barViewer.Save(IO.Path.Combine(sFiguresFolder, sFilePrefix & "ChangeBars_AreaAbsolute.png"), fChartWidth, fChartHeight)

            barViewer.Refresh(changeStats.AreaErosion_Thresholded, changeStats.AreaDeposition_Thresholded, LinearUnits, Visualization.ElevationChangeBarViewer.BarTypes.Area, False)
            barViewer.Save(IO.Path.Combine(sFiguresFolder, sFilePrefix & "ChangeBars_AreaRelative.png"), fChartWidth, fChartHeight)

            barViewer.Refresh(changeStats.VolumeErosion_Thresholded, changeStats.VolumeDeposition_Thresholded, changeStats.NetVolumeOfDifference_Thresholded, changeStats.VolumeErosion_Error, changeStats.VolumeDeposition_Error, changeStats.NetVolumeOfDifference_Error, LinearUnits, ElevationChangeBarViewer.BarTypes.Volume, True)
            barViewer.Save(IO.Path.Combine(sFiguresFolder, sFilePrefix & "ChangeBars_VolumeAbsolute.png"), fChartWidth, fChartHeight)

            barViewer.Refresh(changeStats.VolumeErosion_Thresholded, changeStats.VolumeDeposition_Thresholded, changeStats.NetVolumeOfDifference_Thresholded, changeStats.VolumeErosion_Error, changeStats.VolumeDeposition_Error, changeStats.NetVolumeOfDifference_Error, LinearUnits, ElevationChangeBarViewer.BarTypes.Volume, False)
            barViewer.Save(IO.Path.Combine(sFiguresFolder, sFilePrefix & "ChangeBars_VolumeRelative.png"), fChartWidth, fChartHeight)

            barViewer.Refresh(changeStats.AverageDepthErosion_Thresholded, changeStats.AverageDepthDeposition_Thresholded, changeStats.AverageNetThicknessOfDifferenceADC_Thresholded, changeStats.AverageDepthErosion_Error, changeStats.AverageDepthDeposition_Error, changeStats.AverageThicknessOfDifferenceADC_Error, LinearUnits, ElevationChangeBarViewer.BarTypes.Vertical, True)
            barViewer.Save(IO.Path.Combine(sFiguresFolder, sFilePrefix & "ChangeBars_DepthAbsolute.png"), fChartWidth, fChartHeight)

            barViewer.Refresh(changeStats.AverageDepthErosion_Thresholded, changeStats.AverageDepthDeposition_Thresholded, changeStats.AverageNetThicknessOfDifferenceADC_Thresholded, changeStats.AverageDepthErosion_Error, changeStats.AverageDepthDeposition_Error, changeStats.AverageThicknessOfDifferenceADC_Error, LinearUnits, ElevationChangeBarViewer.BarTypes.Vertical, False)
            barViewer.Save(IO.Path.Combine(sFiguresFolder, sFilePrefix & "ChangeBars_DepthRelative.png"), fChartWidth, fChartHeight)

        End Sub

    End Class

End Namespace