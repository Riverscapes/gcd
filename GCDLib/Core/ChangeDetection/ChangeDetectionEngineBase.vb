Namespace Core.ChangeDetection

    Public Enum UncertaintyTypes
        MinLoD
        Propagated
        Probabilistic
    End Enum

    Public MustInherit Class ChangeDetectionEngineBase

        Private m_sAnalysisName As String
        Private m_dAnalysisFolder As IO.DirectoryInfo

        ' These are the original survey DEMs. May be non-concurrent and 
        ' not masked to the AOI
        Private m_gOriginalNewDEM As GCDConsoleLib.Raster
        Private m_gOriginalOldDEM As GCDConsoleLib.Raster

        ' These are concurrent and masked to the AOI
        Private m_gAnalysisNewDEM As GCDConsoleLib.Raster
        Private m_gAnalysisOldDEM As GCDConsoleLib.Raster

        Private m_gAOI As GCDConsoleLib.Vector

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

        Public ReadOnly Property AOI As GCDConsoleLib.Vector
            Get
                Return m_gAOI
            End Get
        End Property

        Public ReadOnly Property AnalysisNewDEM As GCDConsoleLib.Raster
            Get
                Return m_gAnalysisNewDEM
            End Get
        End Property

        Public ReadOnly Property AnalysisOldDEM As GCDConsoleLib.Raster
            Get
                Return m_gAnalysisOldDEM
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

#End Region

        Public Sub New(ByVal sName As String, ByVal sFolder As String, ByVal gNewDEM As GCDConsoleLib.Raster, ByVal gOldDEM As GCDConsoleLib.Raster,
                       ByVal gAOI As GCDConsoleLib.Vector, ByVal fChartHeight As Integer, ByVal fChartWidth As Integer)

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

            m_gAOI = gAOI

            m_fChartHeight = fChartHeight
            m_fChartWidth = fChartWidth

        End Sub

        Public MustOverride Function Calculate(ByRef sRawDoDPath As String, ByRef sThreshDodPath As String, ByRef sRawHistPath As String, ByRef sThreshHistPath As String, ByRef sSummaryXMLPath As String) As DoDResultSet

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

                    m_gAnalysisNewDEM = GCDConsoleLib.RasterOperators.RasterCopy.ExtendedCopy(m_gOriginalNewDEM, sNewDEM, theUnionExtent)
                    m_gAnalysisOldDEM = GCDConsoleLib.RasterOperators.RasterCopy.ExtendedCopy(m_gOriginalOldDEM, sOldDEM, theUnionExtent)
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
                sRawDoDPath = Core.GCDProject.ProjectManager.OutputManager.GetDoDRawPath(Name, m_dAnalysisFolder.FullName)

                Dim eResult As External.GCDCoreOutputCodes = External.DoDRaw(AnalysisNewDEM.FilePath, AnalysisOldDEM.FilePath, sRawDoDPath,
                                                           GCDProject.ProjectManagerBase.OutputManager.OutputDriver, GCDProject.ProjectManagerBase.OutputManager.NoData,
                                                          GCDProject.ProjectManagerBase.GCDNARCError.ErrorString)

                If eResult <> External.GCDCoreOutputCodes.PROCESS_OK Then

                    Dim ex As New Exception(GCDProject.ProjectManagerBase.GCDNARCError.ErrorString.ToString)
                    Throw New Exception("Error calculating the raw DEM of difference raster", ex)

                End If

                ' Check that the raster exists
                If Not System.IO.File.Exists(sRawDoDPath) Then
                    Throw New Exception("The raw DoD raster file does noth exist.")
                End If

                ''Checks to make sure the histogram is not zeros which causes a non-descript "writing to corrupt memory" exception
                'Dim rawDoDRaster As New GCDConsoleLib.Raster(sRawDoDPath)
                'If rawDoDRaster.Minimum = rawDoDRaster.Maximum Then
                '    Throw New Exception("There was an error calculating the raw DoD. All values are zero in raw DoD.")
                'End If

                sRawHistogram = GCDProject.ProjectManagerBase.OutputManager.GetCsvRawPath(IO.Path.GetDirectoryName(sRawDoDPath), Name)
                eResult = External.CalculateAndWriteDoDHistogramWithBins(sRawDoDPath, sRawHistogram, m_nNumBins, m_nMinimumBin, m_fBinSize, m_fBinIncrement, GCDProject.ProjectManagerBase.GCDNARCError.ErrorString)
                If eResult <> External.GCDCoreOutputCodes.PROCESS_OK Then
                    Dim ex As New Exception(GCDProject.ProjectManagerBase.GCDNARCError.ErrorString.ToString)
                    Throw New Exception("Error calculating and writing the raw DEM histogram.", ex)
                End If

            Catch ex As Exception
                ex.Data("Raw DoD Raster Path") = sRawDoDPath
                ex.Data("Raw DoD Histogram Path") = sRawHistogram
                Throw
            End Try

            Return sRawDoDPath

        End Function

        Protected Function GenerateSummaryXML(theChangeStats As ChangeDetection.ChangeStatsCalculator) As String

            Dim sSummaryXMLPath As String = GCDProject.ProjectManagerBase.OutputManager.GetGCDSummaryXMLPath(Name, m_dAnalysisFolder.FullName)
            theChangeStats.ExportSummary(GCDProject.ProjectManagerBase.ExcelTemplatesFolder.FullName, AnalysisNewDEM.VerticalUnits, sSummaryXMLPath)
            Return sSummaryXMLPath

        End Function

        Protected Sub GenerateHistogramGraphicFiles(histStats As DoDResultHistograms, ByVal eLinearUnit As UnitsNet.Units.LengthUnit)

            'Save Histograms & Create Figs subfolder - Hensleigh 4/24/2014
            Dim sFiguresFolder As String = GCDProject.ProjectManagerBase.OutputManager.GetChangeDetectionFiguresFolder(m_dAnalysisFolder.FullName, True)
            Dim zedGraph As New System.Windows.Forms.DataVisualization.Charting.Chart
            Dim areaHistPath As String = IO.Path.Combine(sFiguresFolder, "Histogram_Area" & ".png")
            Dim volhistPath As String = IO.Path.Combine(sFiguresFolder, "Histogram_Volume" & ".png")
            Dim ExportHistogramViewer As New Visualization.DoDHistogramViewerClass(zedGraph, eLinearUnit)
            ExportHistogramViewer.ExportCharts(histStats, eLinearUnit, areaHistPath, volhistPath, 600, 600)

        End Sub

    End Class

End Namespace