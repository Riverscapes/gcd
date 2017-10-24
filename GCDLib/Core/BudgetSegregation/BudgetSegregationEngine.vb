Namespace Core.BudgetSegregation

    Public Class BudgetSegregationEngine

        Private m_DoDInfo As ChangeDetection.ChangeDetectionProperties

        Private m_gPolygonMask As GCDConsoleLib.Vector
        Private m_sMaskField As String ' This is the string field containing the BS class
        Private m_sClassField As String ' This is an integer representing the BS class
        Private m_dOutputFolder As IO.DirectoryInfo

        Private m_fChartHeight As Integer
        Private m_fChartWidth As Integer

        Public ReadOnly Property DoD As ChangeDetection.ChangeDetectionProperties
            Get
                Return m_DoDInfo
            End Get
        End Property

        Public Sub New(ByVal dodInfo As ChangeDetection.ChangeDetectionProperties, ByVal dOutputFolder As IO.DirectoryInfo, ByVal fChartHeight As Integer, ByVal fChartWidth As Integer)

            m_DoDInfo = dodInfo
            m_dOutputFolder = dOutputFolder

            m_fChartHeight = fChartHeight
            m_fChartWidth = fChartWidth
            'If dOutputFolder.Parent.Exists Then
            'Else
            '    Dim ex As New Exception("The parent folder for the budget segregation does not exist.")
            '    ex.Data("Budget Seg Output Folder") = m_dOutputFolder.FullName
            '    ex.Data("Parent folder") = m_dOutputFolder.Parent.FullName
            '    Throw ex
            'End If

        End Sub

        Public Function Calculate(gInputPolygonMask As GCDConsoleLib.Vector, sMaskField As String, nChartWidth As Integer, nChartHeight As Integer, bDeleteIntermediateRasters As Boolean) As BudgetSegregationOutputsClass

            ' 1. Build Dictionary of Mask Labels
            ' 2. Loop through each mask
            ' 3. Mask the raw and thresholded DoDs
            ' 4. Build stats for the DoDs
            ' 5. Produce histograms for the DoDs
            ' 6. Produce graphics for the DoDs


            Dim gDoDRaw As New GCDConsoleLib.Raster(DoD.RawDoD)
            If gInputPolygonMask.Proj.IsSame(gDoDRaw.Proj) Then
                If String.IsNullOrEmpty(sMaskField) Then
                    Dim ex As New Exception("The mask field cannot be null or empty.")
                    ex.Data("Polygon Mask") = gInputPolygonMask.FilePath
                    Throw ex
                Else
                    If Not gInputPolygonMask.Fields.ContainsKey(sMaskField) Then
                        Dim ex As New Exception("The field '" & sMaskField & "' does not exist in the polygon mask feature class.")
                        ex.Data("Polygon Mask") = gInputPolygonMask.FilePath
                        ex.Data("Mask field") = sMaskField
                        Throw ex
                    End If
                End If
            Else
                Dim ex As New Exception("The spatial reference of the polygon mask does not match that of the DoD raster. All data within a GCD project must share identical spatial references.")
                ex.Data("Polygon spatial reference") = gInputPolygonMask.Proj.PrettyWkt
                ex.Data("DoD spatial reference") = gDoDRaw.Proj.PrettyWkt
                Throw ex
            End If

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Perform the budget segregation
            '
            ' Determine the path where the BS will be stored.
            'Dim sOutputFolder As String = GCD.GCDProject.ProjectManagerBase.OutputManager.GetBudgetSegreationDirectoryPath(GISDataStructures.GetWorkspacePath(DoD.RawDoD), IO.Path.GetFileNameWithoutExtension(gInputPolygonMask.FullPath), sMaskField)

            'Dim sOutputFolder As String = GCD.GCDProject.ProjectManagerBase.OutputManager.CreateBudgetSegFolder(DoD.RawDoD, IO.Path.GetFileNameWithoutExtension(gInputPolygonMask.FullPath), sMaskField)
            Dim sOutputFolder = m_dOutputFolder.FullName
            ' Copy Polygon features to the output folder
            'Dim sPolygonMask As String = GCDConsoleLib.VectorDataSource.GetNewSafeName(sOutputFolder, "Mask")

            'Changed to provide a more descriptive name than mask - Hensleigh 4/24/2014
            Dim sPolygonMask As String = naru.os.File.GetNewSafeName(sOutputFolder, IO.Path.GetFileNameWithoutExtension(gInputPolygonMask.FilePath) & "_" & sMaskField, "shp").FullName
            gInputPolygonMask.Copy(sPolygonMask)
            m_gPolygonMask = New GCDConsoleLib.Vector(sPolygonMask)

            ' Build the dictionary of budget classes. This method also writes the 
            ' class indexes to the shapefile in an integer field.
            Dim dMaskClasses As Dictionary(Of String, Integer) = BuildMaskLabels(sMaskField)

            ' Build a comma separated string of the mask class IDs
            Dim sMaskIDList As String = String.Empty
            For Each nClassID As Integer In dMaskClasses.Values
                sMaskIDList &= nClassID.ToString & ","
            Next
            sMaskIDList = sMaskIDList.TrimEnd(",")

            ' Convert the polygon mask layer to raster (using the orthogonality information from the DoD)
            ' PGB 24 Jul 2013 - In ArcGIS 10.1 the conversion does not seem to preserve the map projection.
            ' Apply the projection of the polygons to the segregation raster to be sure.
            '
            ' PGB Nov 2014. Experienced a situation where the mask has different rows and cols to DoD
            ' despite passing the DoD raster as reference. This occured when the DoD extent possessed
            ' decimal coordinates but was divisible. 
            '
            ' Solution... Convert it using ESRI (for now) to temp location but then use GDAL to copy it
            ' With precise size.
            Dim sTempMask As String = WorkspaceManager.GetTempRaster("TempMask")

            Throw New NotImplementedException
            '  GP.Conversion.PolygonToRaster_conversion(m_gPolygonMask, m_sClassField, sTempMask, gDoDRaw)
            Throw New NotImplementedException
            '  GP.DataManagement.DefineProjection(sTempMask, gDoDRaw.SpatialReference)

            ' Now copy to the desired location
            Dim sMaskRaster As String = naru.os.File.GetNewSafeName(sOutputFolder, "Mask", GCDProject.ProjectManagerBase.RasterExtension).FullName
            'If Not External.RasterManager.Copy(sTempMask, sMaskRaster, gDoDRaw.HorizontalPrecision, gDoDRaw.Extent.Left, gDoDRaw.Extent.Top, gDoDRaw.Rows, gDoDRaw.Columns, GCDProject.ProjectManagerBase.GCDNARCError.ErrorString) = External.RasterManager.RasterManagerOutputCodes.PROCESS_OK Then
            '    Throw New Exception(GCDProject.ProjectManagerBase.GCDNARCError.ErrorString.ToString)
            'End If
            Dim gMaskRaster As New GCDConsoleLib.Raster(sMaskRaster)

            Dim bsOutputs As New BudgetSegregationOutputsClass(sOutputFolder, dMaskClasses, m_gPolygonMask.FilePath)

            ' Open the mask legend CSV file ready to store a line for each mask class
            Dim sbClassLegend As New System.Text.StringBuilder
            sbClassLegend.AppendLine("Class Value,Class Description")

            ' Loop through each mask class and generate the histograms
            'Dim maskHistograms As New MaskHistogramsClass(DoD.ThresholdedDoD, sMaskRaster, sMaskIDList)
            Dim sMaskIndicesAndCSVPaths As String = ""
            For Each sMaskName As String In bsOutputs.MaskOutputs.Keys
                sMaskIndicesAndCSVPaths &= bsOutputs.MaskOutputs(sMaskName).MaskValue & ";" & bsOutputs.MaskOutputs(sMaskName).csvFilename & ";"
            Next
            sMaskIndicesAndCSVPaths = sMaskIndicesAndCSVPaths.Substring(0, sMaskIndicesAndCSVPaths.Length - 1)
            If Not External.GCDCore.CalculateAndWriteMaskHistograms(DoD.ThresholdedDoD, sMaskRaster, sMaskIDList, sMaskIndicesAndCSVPaths, GCDProject.ProjectManagerBase.GCDNARCError.ErrorString) = External.GCDCoreOutputCodes.PROCESS_OK Then
                Throw New Exception(GCDProject.ProjectManagerBase.GCDNARCError.ErrorString.ToString)
            End If

            For Each kvMask As KeyValuePair(Of String, BudgetSegregation.BudgetSegregationOutputsClass.MaskOutputClass) In bsOutputs.MaskOutputs
                Dim sSafeMaskName As String = naru.os.File.RemoveDangerousCharacters(kvMask.Value.MaskValue)
                Debug.WriteLine(String.Format("Budget Segregation Class with value {0} and safe name '{1}'.", kvMask.Value.MaskValue, sSafeMaskName))
                Dim maskOutputClass As BudgetSegregationOutputsClass.MaskOutputClass = bsOutputs.MaskOutputs(kvMask.Key)
                '
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' This version of RasterMan MaskValue **retains** cells in the mask that possess the argument value.
                '
                Dim sPositiveMask As String = WorkspaceManager.GetTempRaster("PMask_" & sSafeMaskName)
                External.RasterManager.MaskValue(sMaskRaster, sPositiveMask, Convert.ToDouble(kvMask.Value.MaskValue), GCDProject.ProjectManagerBase.GCDNARCError.ErrorString)
                '
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' Mask the raw and thresholded DoDs to create a pair of rasters that just have values
                ' for the valid areas for the current Mask
                Dim sMaskRaw As String = WorkspaceManager.GetTempRaster("MR_" & sSafeMaskName)
                External.RasterManager.Mask(DoD.RawDoD, sPositiveMask, sMaskRaw, GCDProject.ProjectManagerBase.GCDNARCError.ErrorString)

                Dim sMaskThr As String = WorkspaceManager.GetTempRaster("MT_" & sSafeMaskName)
                External.RasterManager.Mask(DoD.ThresholdedDoD, sPositiveMask, sMaskThr, GCDProject.ProjectManagerBase.GCDNARCError.ErrorString)

                ' Need to create a new ChangeStats for the new masked out DoD rasters.
                ' First build a new DoD Properties depending on the type of the full DoD
                ' for the whole raster. Then use the attributes of this full DoD combined
                ' with the newly created masked DoDs for the new BS change stats for this mask.
                If TypeOf DoD Is ChangeDetection.ChangeDetectionPropertiesMinLoD Then
                    Dim BSDoD As New ChangeDetection.ChangeDetectionPropertiesMinLoD(sMaskRaw, sMaskThr, DirectCast(DoD, ChangeDetection.ChangeDetectionPropertiesMinLoD).Threshold, gDoDRaw.Extent.CellWidth, gDoDRaw.VerticalUnits)
                    maskOutputClass.ChangeStats = New ChangeDetection.ChangeStatsCalculator(BSDoD)
                Else
                    ' PGB - 8 Apr 2015 - Need to check type against probabilistic first because the 
                    ' probabilistic class is **inherited** from the propagated. Hence the type will
                    ' always be propagated, even when it is probabilistic. i.e. perform the more
                    ' restrictive check first.
                    ' 
                    If TypeOf DoD Is ChangeDetection.ChangeDetectionPropertiesProbabilistic Then
                        Dim FullDoD As ChangeDetection.ChangeDetectionPropertiesProbabilistic = DirectCast(DoD, ChangeDetection.ChangeDetectionPropertiesProbabilistic)
                        Dim BSDoD As New ChangeDetection.ChangeDetectionPropertiesProbabilistic(sMaskRaw, sMaskThr, FullDoD.PropagatedErrorRaster, FullDoD.ProbabilityRaster, FullDoD.SpatialCoErosionRaster, FullDoD.SpatialCoDepositionRaster, FullDoD.ConditionalRaster, FullDoD.PosteriorRaster, FullDoD.ConfidenceLevel, FullDoD.SpatialCoherenceFilter, FullDoD.BayesianUpdating, gDoDRaw.Extent.CellWidth, gDoDRaw.VerticalUnits)
                        maskOutputClass.ChangeStats = New ChangeDetection.ChangeStatsCalculator(BSDoD)
                    ElseIf TypeOf DoD Is ChangeDetection.ChangeDetectionPropertiesPropagated Then
                        Dim BSDoD As New ChangeDetection.ChangeDetectionPropertiesPropagated(sMaskRaw, sMaskThr, DirectCast(DoD, ChangeDetection.ChangeDetectionPropertiesPropagated).PropagatedErrorRaster, gDoDRaw.Extent.CellWidth, gDoDRaw.VerticalUnits)
                        maskOutputClass.ChangeStats = New ChangeDetection.ChangeStatsCalculator(BSDoD)
                    Else
                        Dim ex As New Exception("Unhandled change detection type.")
                        Throw ex
                    End If
                End If

                ' Export the change statistics XML file.
                Dim sSummaryXMLPath As String = IO.Path.Combine(m_dOutputFolder.FullName, "c" & maskOutputClass.MaskValue.ToString("000") & "_summary.xml")
                DirectCast(maskOutputClass.ChangeStats, ChangeDetection.ChangeStatsCalculator).ExportSummary(GCDProject.ProjectManagerBase.ExcelTemplatesFolder.FullName,
                                                                                                               DoD.Units,
                                                                                                                 sSummaryXMLPath)

                ' Write the thresholded histogram CSV file. Then load it as a DoD result histogram
                'maskHistograms.writeCSV(maskOutputClass.MaskValue, maskOutputClass.csvFilename)
                Dim ExportStatsData As New ChangeDetection.DoDResultHistograms(maskOutputClass.csvFilename)

                ' Export the histogram graphics
                'Dim c As New Windows.Forms.DataVisualization.Charting.Chart
                Dim c As New System.Windows.Forms.DataVisualization.Charting.Chart
                Dim ExportHistogramViewer As New Core.Visualization.DoDHistogramViewerClass(c, DoD.Units)

                IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(maskOutputClass.AreaChartPath))
                ExportHistogramViewer.ExportCharts(ExportStatsData, gDoDRaw.VerticalUnits, maskOutputClass.AreaChartPath, maskOutputClass.VolumeChartPath, nChartWidth, nChartHeight)

                Dim cbViewer As New Core.Visualization.ElevationChangeBarViewer(c, UnitsNet.Length.GetAbbreviation(gDoDRaw.VerticalUnits))
                DirectCast(maskOutputClass.ChangeStats, ChangeDetection.ChangeStatsCalculator).GenerateChangeBarGraphicFiles(GCDProject.ProjectManagerBase.OutputManager.GetChangeDetectionFiguresFolder(sOutputFolder, True), DoD.Units, m_fChartWidth, m_fChartHeight, "c" & maskOutputClass.MaskValue.ToString("000"))

                ' Append this class to the legend file
                sbClassLegend.AppendLine(maskOutputClass.MaskValue & "," & kvMask.Key)
                '
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' Deletes the mask rasters so that they don't accumulate (and fill the hard drive if there are lots of classes)
                '
                If bDeleteIntermediateRasters Then
                    GC.Collect()

                    GCDConsoleLib.Raster.Delete(sPositiveMask)
                    GCDConsoleLib.Raster.Delete(sMaskRaw)
                    GCDConsoleLib.Raster.Delete(sMaskThr)
                End If
            Next
            'maskHistograms.Dispose()

            ' Save and close the class legend CSV file
            Dim ClassLegendFile As New IO.StreamWriter(bsOutputs.ClassLegendPath)
            ClassLegendFile.Write(sbClassLegend.ToString())
            ClassLegendFile.Close()

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Now process summary outputs.

            'Dim pieChart As New Windows.Forms.DataVisualization.Charting.Chart
            'Dim ExportMaskStats As MaskStatsClass '= GetMaskStats()
            'Dim ExportPieChartViewer As New PieChartViewerClass(pieChart, ExportMaskStats.MaskStats)
            'ExportPieChartViewer.Legend = True

            'ExportPieChartViewer.Statistics = "Total Deposition Volume"
            'ExportPieChartViewer.refresh()
            'ExportPieChartViewer.ExportCharts(bsOutputs.PieCharts.PercentageTotalDepositionVolumePiePath, nChartWidth, nChartHeight)

            'ExportPieChartViewer.Statistics = "Total Erosion Volume"
            'ExportPieChartViewer.refresh()
            'ExportPieChartViewer.ExportCharts(bsOutputs.PieCharts.PercentageTotalErosionVolumePiePath, nChartWidth, nChartHeight)

            'ExportPieChartViewer.Statistics = "Total Volume Change"
            'ExportPieChartViewer.refresh()
            'ExportPieChartViewer.ExportCharts(bsOutputs.PieCharts.PercentageTotalVolumePiePath, nChartWidth, nChartHeight)

            '' Export summaries and the GCD summary XML
            'ExportMaskStats.ExportSummaries(GCD.GCDProject.ProjectManagerBase.ExcelTemplatesFolder, bsOutputs.MaskOutputs, GISDataStructures.GetLinearUnitsAsString(gDODSource.LinearUnits))
            'ExportMaskStats.ExportClassSummary(GCD.GCDProject.ProjectManagerBase.ExcelTemplatesFolder.FullName, bsOutputs.ClassSummaryPath, bsOutputs.MaskOutputs, Units)

            Return bsOutputs

        End Function

        Private Function BuildMaskLabels(sMaskField As String) As Dictionary(Of String, Integer)
            '
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Add a class field to the polygon feature class that will store the
            ' integer representing each mask class. Make sure that 
            ' the field name is unique and doesn't exceed the size allowed.
            Dim nCount As Integer = 0
            Do
                m_sClassField = "Class"
                If nCount > 0 Then
                    m_sClassField &= nCount.ToString
                End If

                If sMaskField.Length > 10 Then
                    Dim ex As New Exception("The class field length has exceeded 10 characters.")
                    ex.Data("Class field") = m_sClassField
                    ex.Data("Mask field") = m_sMaskField
                    ex.Data("Polygon feature class") = m_gPolygonMask.FilePath
                    Throw ex
                End If

                nCount += 1
            Loop While m_gPolygonMask.Fields.ContainsKey(m_sClassField)

            Dim nClasskFieldIndex As Integer = m_gPolygonMask.AddField(m_sClassField, GCDConsoleLib.GDalFieldType.IntField, Nothing)
            If nClasskFieldIndex < 0 Then
                Dim ex As New Exception("Budget segregation class field does not exist in feature class.")
                ex.Data("Class field") = m_sClassField
                ex.Data("Mask Field") = m_sMaskField
                ex.Data("Polygon feature class") = m_gPolygonMask.FilePath
                Throw ex
            End If

            Dim nMaskFieldIndex As Integer = m_gPolygonMask.Fields.ContainsKey(sMaskField)
            If nMaskFieldIndex < 0 Then
                Dim ex As New Exception("Budget segregation mask field does not exist in feature class.")
                ex.Data("Class field") = m_sClassField
                ex.Data("Mask Field") = m_sMaskField
                ex.Data("Polygon feature class") = m_gPolygonMask.FilePath
                Throw ex
            End If

            'TODO
            Throw New NotImplementedException()
            '
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Loop over all features in the polygon feature class. Read their **mask**
            ' value. If it doesn't already appear in the mask dictionary then add it
            ' with the next class ID. Then write the class ID to each feature.
            Dim nClass As Integer = 1
            Dim dMaskClassValues As New Dictionary(Of String, Integer)(StringComparer.InvariantCultureIgnoreCase)
            'Dim pCursor As ESRI.ArcGIS.Geodatabase.IFeatureCursor = m_gPolygonMask.FeatureClass.Search(Nothing, True)
            'Dim pFeature As ESRI.ArcGIS.Geodatabase.IFeature = pCursor.NextFeature
            'While TypeOf pFeature Is ESRI.ArcGIS.Geodatabase.IFeature
            '    If Not IsDBNull(pFeature.Value(nMaskFieldIndex)) Then
            '        Dim sMaskValue As String = pFeature.Value(nMaskFieldIndex)

            '        'if new value, add to existing mask values
            '        If Not dMaskClassValues.ContainsKey(sMaskValue) Then
            '            dMaskClassValues.Add(sMaskValue, nClass)
            '            nClass = nClass + 1
            '        End If
            '        pFeature.Value(nClasskFieldIndex) = dMaskClassValues(sMaskValue)
            '        pFeature.Store()
            '        pFeature = pCursor.NextFeature
            '    End If
            'End While
            'Runtime.InteropServices.Marshal.ReleaseComObject(pCursor)
            'pCursor = Nothing

            Return dMaskClassValues

        End Function

    End Class

End Namespace