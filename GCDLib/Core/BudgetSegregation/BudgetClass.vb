Imports System.IO
Imports System.Xml
Imports System.Windows.Forms.DataVisualization.Charting

Namespace Core.BudgetSegregation

    Public Class BudgetClass

#Region "Member Variables"
        Private Const Precision As Integer = 4
        Private _Mask As String
        Private _MaskCopy As FileInfo
        Private _ClassFieldName As String
        Private _Field As String
        Private _DoDSource As String
        Private _SegregationRaster As String
        Private _RawHistSource As String
        Private _DoDName As String
        Private _NewSurveyName As String
        Private _OldSurveyName As String
        Private _MaskFilename As String
        Private _output As BudgetSegregationOutputsClass

#End Region

#Region "Properties"

        Public Property Mask As String
            Get
                Return _Mask
            End Get
            Set(ByVal value As String)
                _Mask = value
            End Set
        End Property

        Public Property Field As String
            Get
                Return _Field
            End Get
            Set(ByVal value As String)
                _Field = value
            End Set
        End Property

        Public Property DODSource As String
            Get
                Return _DoDSource
            End Get
            Set(ByVal value As String)
                _DoDSource = value
            End Set
        End Property

        Public ReadOnly Property SegregationRaster As String
            Get
                Return _SegregationRaster
            End Get
        End Property

        Public Property RawHistSource As String
            Get
                Return _RawHistSource
            End Get
            Set(ByVal value As String)
                _RawHistSource = value
            End Set
        End Property

        Public Property DoDName
            Get
                Return _DoDName
            End Get
            Set(ByVal value)
                _DoDName = value
            End Set
        End Property

        Public Property NewSurveyName
            Get
                Return _NewSurveyName
            End Get
            Set(ByVal value)
                _NewSurveyName = value
            End Set
        End Property

        Public Property OldSurveyName
            Get
                Return _OldSurveyName
            End Get
            Set(ByVal value)
                _OldSurveyName = value
            End Set
        End Property

        Public Property MaskFilename
            Get
                Return _MaskFilename
            End Get
            Set(ByVal value)
                _MaskFilename = value
            End Set
        End Property

        Public ReadOnly Property output As BudgetSegregationOutputsClass
            Get
                Return _output
            End Get
        End Property
#End Region

        Public Sub CalculateBudget(sTemplateFolder As String, sRootFolder As String, nChartWidth As Integer, nChartHeight As Integer, xmlResults As XmlTextWriter)

            If Not IO.Directory.Exists(sRootFolder) Then
                Dim ex As New Exception("The root folder for the budget segregation does not exist")
                ex.Data("Root Folder") = sRootFolder
                Throw ex
            End If

            'validate input
            If String.IsNullOrEmpty(_Field) Then
                Throw New Exception("CalculateBudget failed. Field is null or empty")
            End If

            If String.IsNullOrEmpty(_Mask) Then
                Throw New Exception("CalculateBudget failed. Mask is null or empty")
            End If

            If String.IsNullOrEmpty(_DoDSource) Then
                Throw New Exception("CalculateBudget failed. DodSource is null or empty")
            End If

            _SegregationRaster = WorkspaceManager.GetTempRaster("Seg").ToString

            Dim gRaster As New GCDConsoleLib.Raster(_DoDSource)

            Dim ExtentRectangle As String = gRaster.Extent.Rectangle
            '
            ' Must call this before the conversion of the mask to raster, because it makes a copy of the mask feature class.
            '
            Dim MaskLabels As Dictionary(Of String, Integer) = GetLabels()
            '
            ' Coonvert the polygon mask layer to raster (using the orthogonality information from the DoD)
            '
            Dim gMask As New GCDConsoleLib.Vector(_MaskCopy.FullName)
            Throw New NotImplementedException
            'GP.Conversion.PolygonToRaster_conversion(gMask, _ClassFieldName, _SegregationRaster, gRaster)
            '
            ' PGB 24 Jul 2013 - In ArcGIS 10.1 the conversion does not seem to preserve the map projection. Apply the projection of the
            ' polygons to the segregation raster to be sure.
            '
            Throw New NotImplementedException
            'GP.DataManagement.DefineProjection(_SegregationRaster, gRaster.SpatialReference)
            '
            ' Now ensure it is concurrent
            '
            'Dim expression As String = """" & _SegregationRaster & """"
            'Dim SegCCOrthog As String = WorkspaceManager.GetTempRaster("SegCCOrth").ToString
            'GP.SpatialAnalyst.Raster_Calculator(expression, SegCCOrthog, ExtentRectangle, CellResolution)
            '
            ' The budget seg uses the member variable, so now replace the original name with the concurrent & orthogonal name
            '
            '_SegregationRaster = SegCCOrthog

            SegregateBudget(sTemplateFolder, sRootFolder, MaskLabels, nChartWidth, nChartHeight, xmlResults)
        End Sub

        Public Function GetBudgetSegregationDirectoryPath() As String
            'Get GetChangeDetectionDirectoryPath
            'Dim ChangeDetectionDirectoryPath As String = GCDProject.ProjectManager.OutputManager.GetChangeDetectionDirectoryPath(_NewSurveyName, _OldSurveyName, _DoDName)
            Dim BudgetSegregationDirectoryPath As String = GCDProject.ProjectManagerBase.OutputManager.GetBudgetSegreationDirectoryPath(GCDProject.ProjectManagerBase.OutputManager.GetDoDOutputFolder(_DoDName), _MaskFilename, _Field)
            Return BudgetSegregationDirectoryPath
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sExcelTemplateFolder"></param>
        ''' <param name="sRootFolder"></param>
        ''' <param name="MaskLabels"></param>
        ''' <param name="nChartWidth"></param>
        ''' <param name="nChartHeight"></param>
        ''' <param name="xmlresults"></param>
        ''' <remarks>The Excel template folder has to be abstracted because this is now used by GCD and RBT console</remarks>
        Private Sub SegregateBudget(sExcelTemplateFolder As String, sRootFolder As String, ByVal MaskLabels As Dictionary(Of String, Integer), nChartWidth As Integer, nChartHeight As Integer, xmlresults As XmlTextWriter)
            'Dim csvFilename As String
            Dim maskValues As String = ""
            Dim MaskLabel As String

            'init GDAL
            'Gdal.AllRegister()

            'Get GetChangeDetectionDirectoryPath
            Dim BudgetSegregationDirectoryPath As String = sRootFolder ' GetBudgetSegregationDirectoryPath(sRootFolder)


            _output = New BudgetSegregationOutputsClass(BudgetSegregationDirectoryPath, MaskLabels, "")
            '_output.DeleteOutputs()

            'setup mask values
            For Each maskoutput As BudgetSegregationOutputsClass.MaskOutputClass In _output.MaskOutputs.Values
                maskValues &= maskoutput.MaskValue & ","
            Next
            maskValues = maskValues.TrimEnd(",")

            'Save csvs
            Dim gDoDSource As New GCDConsoleLib.Raster(_DoDSource)
            Dim gSegregatedRaster As New GCDConsoleLib.Raster(_SegregationRaster)

            If Not gDoDSource.Extent.IsConcurrent(gSegregatedRaster.Extent) Then
                Throw New Exception("Input layers are not concurrent")
            End If

            'Dim maskHistograms As New MaskHistogramsClass(_DoDSource, _SegregationRaster, maskValues)

            Dim sMaskIndicesAndCSVFilePath As String = ""
            For Each kvp As KeyValuePair(Of String, BudgetSegregationOutputsClass.MaskOutputClass) In _output.MaskOutputs
                Dim maskoutout As BudgetSegregationOutputsClass.MaskOutputClass = kvp.Value
                'maskHistograms.writeCSV(maskoutout.MaskValue, maskoutout.csvFilename)
                sMaskIndicesAndCSVFilePath &= maskoutout.MaskValue.ToString & ";" & maskoutout.csvFilename & ";"
            Next

            If String.IsNullOrEmpty(sMaskIndicesAndCSVFilePath) Then
                Throw New Exception("The list of mask CSV file paths is empty.")
            Else
                sMaskIndicesAndCSVFilePath = sMaskIndicesAndCSVFilePath.Substring(0, sMaskIndicesAndCSVFilePath.Length - 1)
            End If

            ' New method for calculating and writing mask values.
            If Not External.GCDCore.CalculateAndWriteMaskHistograms(_DoDSource, _SegregationRaster, maskValues, sMaskIndicesAndCSVFilePath, GCDProject.ProjectManagerBase.GCDNARCError.ErrorString) = External.GCDCoreOutputCodes.PROCESS_OK Then
                Throw New Exception(GCDProject.ProjectManagerBase.GCDNARCError.ErrorString.ToString)
            End If

            'add to ClassLegend csv string
            Dim sbClassLegend As New System.Text.StringBuilder
            sbClassLegend.AppendLine("Class Value,Class Description")

            For Each kvp As KeyValuePair(Of String, BudgetSegregationOutputsClass.MaskOutputClass) In _output.MaskOutputs
                MaskLabel = kvp.Key
                Dim MaskOutput As BudgetSegregationOutputsClass.MaskOutputClass = kvp.Value

                sbClassLegend.AppendLine(MaskLabel & "," & MaskOutput.MaskValue)
            Next

            'Save ClassLegend csv
            Dim ClassLegendFile As New StreamWriter(output.ClassLegendPath)
            ClassLegendFile.Write(sbClassLegend.ToString())
            ClassLegendFile.Close()

            'export histograms
            'For Each MaskOutput As BudgetSegregationOutputsClass.MaskOutputClass In output.MaskOutputs.Values
            'Dim gDODSource As New GCDConsoleLib.Raster(_DoDSource)

            For Each kvp As KeyValuePair(Of String, BudgetSegregationOutputsClass.MaskOutputClass) In _output.MaskOutputs
                MaskLabel = kvp.Key
                'Dim ExportStatsData As New ChangeDetection.StatsDataClass(_RawHistSource, _output.MaskOutputs(MaskLabel).csvFilename)
                Dim ExportStatsData As New ChangeDetection.DoDResultHistograms(_RawHistSource, _output.MaskOutputs(MaskLabel).csvFilename)

                If TypeOf xmlresults Is Xml.XmlTextWriter Then
                    xmlresults.WriteStartElement(MaskLabel)
                    '    ExportStatsData.OutputCHaMPResults(xmlresults)
                    xmlresults.WriteEndElement()
                End If

                Dim c As New Chart
                'Dim c As New Windows.Forms.DataVisualization.Charting.Chart
                Dim ExportHistogramViewer As New Core.Visualization.DoDHistogramViewerClass(c, gDoDSource.LinearUnits)
                ExportHistogramViewer.ExportCharts(ExportStatsData, gDoDSource.LinearUnits, _output.MaskOutputs(MaskLabel).AreaChartPath, _output.MaskOutputs(MaskLabel).VolumeChartPath, nChartWidth, nChartHeight)
            Next

            'export pie charts
            'Dim ExportZedGraph As New ZedGraph.ZedGraphControl
            'Dim pieChart As New Windows.Forms.DataVisualization.Charting.Chart
            Dim ExportMaskStats As MaskStatsClass = GetMaskStats()
            'Dim ExportPieChartViewer As New GISCode.GCD.PieChartViewerClass(pieChart, ExportMaskStats.MaskStats)
            'ExportPieChartViewer.Legend = True

            'ExportPieChartViewer.Statistics = "Total Deposition Volume"
            'ExportPieChartViewer.refresh()
            'ExportPieChartViewer.ExportCharts(output.PieCharts.PercentageTotalDepositionVolumePiePath, nChartWidth, nChartHeight)

            'ExportPieChartViewer.Statistics = "Total Erosion Volume"
            'ExportPieChartViewer.refresh()
            'ExportPieChartViewer.ExportCharts(output.PieCharts.PercentageTotalErosionVolumePiePath, nChartWidth, nChartHeight)

            'ExportPieChartViewer.Statistics = "Total Volume Change"
            'ExportPieChartViewer.refresh()
            'ExportPieChartViewer.ExportCharts(output.PieCharts.PercentageTotalVolumePiePath, nChartWidth, nChartHeight)

            'export summaries
            ExportMaskStats.ExportSummaries(sExcelTemplateFolder, output.MaskOutputs, gDoDSource.LinearUnits)
            '
            ' PGB 24 Apr 2012. 
            ' Export one, combined summary for all classes
            '
            ExportMaskStats.ExportClassSummary(sExcelTemplateFolder, output.ClassSummaryPath, output.MaskOutputs, gDoDSource.LinearUnits)

            GC.Collect()
            'Gdal.GDALDestroyDriverManager()
            'GC.Collect()

        End Sub

        Private Function GetLabels() As Dictionary(Of String, Integer)

            Dim pValue As String
            Dim nClass As Integer = 1
            Dim MaskLabels As New Dictionary(Of String, Integer)
            '
            ' Copy shapefile
            '
            _MaskCopy = New FileInfo(WorkspaceManager.GetTempShapeFile("Mask"))
            GCDConsoleLib.Vector.CopyShapeFile(_Mask, _MaskCopy.FullName)
            '
            ' Add class field
            '
            Dim RootFieldName = "Class"
            _ClassFieldName = "Class"
            Dim nCount As Integer = 0

            Dim gMaskCopy As New GCDConsoleLib.Vector(_MaskCopy.FullName)

            Do
                If nCount = 0 Then
                    _ClassFieldName = RootFieldName
                Else
                    _ClassFieldName = RootFieldName & nCount
                End If
                nCount = nCount + 1
            Loop While gMaskCopy.FindField(_ClassFieldName) > -1 And nCount < 9999

            gMaskCopy.AddField(_ClassFieldName, GISDataStructures.FieldTypes.IntField)

            'open shapefile
            'm_pMaskFeature = ShapeFile.OpenShapefile(_MaskCopy.Name, _MaskCopy.DirectoryName)



            'TODO
            Throw New Exception("following loop commented out")
            'If TypeOf gMaskCopy.FeatureClass Is IFeatureClass Then

            '    'get field index
            '    Dim nIdentifierFld As Integer = gMaskCopy.FindField(_Field)
            '    If nIdentifierFld < 0 Then
            '        Throw New Exception("Budget segregation: Survey method identifier field does not exist in shapefile.")
            '    End If

            '    'get class index
            '    Dim nClassFld As Integer = gMaskCopy.FindField(_ClassFieldName)
            '    If nClassFld < 0 Then
            '        Throw New Exception("Budget segregation: Class field does not exist in shapefile.")
            '    End If

            '    'loop over all features
            '    Dim pCursor As IFeatureCursor = gMaskCopy.FeatureClass.Search(Nothing, True)
            '    pFeature = pCursor.NextFeature
            '    While pFeature IsNot Nothing
            '        pValue = pFeature.Value(nIdentifierFld)

            '        'if new value, add to existing methods
            '        If Not MaskLabels.ContainsKey(pValue) Then
            '            MaskLabels.Add(pValue, nClass)
            '            nClass = nClass + 1
            '        End If

            '        pFeature.Value(nClassFld) = MaskLabels(pValue)
            '        pFeature.Store()
            '        pFeature = pCursor.NextFeature
            '    End While
            'End If
            Return MaskLabels
        End Function

        ''' <summary>
        ''' Calculate statistics for each mask
        ''' </summary>
        ''' <remarks></remarks>
        Public Function GetMaskStats() As MaskStatsClass
            Dim MasksStats As New MaskStatsClass

            For Each kvp As KeyValuePair(Of String, BudgetSegregationOutputsClass.MaskOutputClass) In _output.MaskOutputs
                Dim MaskOutput As BudgetSegregationOutputsClass.MaskOutputClass = kvp.Value
                Dim MaskName As String = kvp.Key
                Dim MaskCsvPath As String = MaskOutput.csvFilename

                Throw New NotImplementedException
                Dim StatsData As ChangeDetection.ChangeStats '(ChangeDetection.StatsDataClass(MaskCsvPath)

                MasksStats.TotalStats.AreaErosion_Thresholded += StatsData.AreaErosion_Thresholded
                MasksStats.TotalStats.AreaDeposition_Thresholded += StatsData.AreaDeposition_Thresholded
                MasksStats.TotalStats.VolumeErosion_Thresholded += StatsData.VolumeErosion_Thresholded
                MasksStats.TotalStats.VolumeDeposition_Thresholded += StatsData.VolumeDeposition_Thresholded

                ' PGB 14 Jan 2014. I think that these will reflect the mask classes by virtue of the erosion
                ' and deposition accumulated above. i.e. the Total and Net Volume of Difference is derived from
                ' the erosion and deposition values.
                'MasksStats.TotalStats.VolumeOfDifference_Thresholded += StatsData.VolumeOfDifference_Thresholded
                'MasksStats.TotalStats.NetVolumeOfDifference_Thresholded += StatsData.NetVolumeOfDifference_Thresholded

                MasksStats.MaskStats.Add(MaskName, StatsData)
            Next
            Return MasksStats
        End Function

    End Class

End Namespace