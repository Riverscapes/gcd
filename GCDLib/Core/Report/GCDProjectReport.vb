Imports System.Web.UI

Namespace GISCode.GCD.Report

    Public Class GCDProjectReport
        Inherits Report

        Public Sub New(ByVal pArcMap As ESRI.ArcGIS.Framework.IApplication, ByVal fCSSFilePath As IO.FileInfo, ByVal dReportResourcesFolder As IO.DirectoryInfo)
            MyBase.New(pArcMap, fCSSFilePath, dReportResourcesFolder)

        End Sub

        Public Function GenerateReportXSL(ByVal sXSLPath As String, ByVal sGCDReportPath As String) As String

            'http://msdn.microsoft.com/en-us/library/system.xml.xsl.xslcompiledtransform(v=vs.110).aspx
            'http://forums.asp.net/t/1476290.aspx?Problem+in+using+XslCompiledTransform+Load+ - answer from Martin

            Dim sReportString As String = Nothing

            'Load the xml data file
            Dim gcdXML As System.Xml.XPath.XPathDocument = New System.Xml.XPath.XPathDocument(GCD.GCDProject.ProjectManager.FilePath)

            'Set xml settings
            Dim xmlSettings As System.Xml.XmlReaderSettings = New System.Xml.XmlReaderSettings
            xmlSettings.ProhibitDtd = False

            'Set xsl settings
            Dim xslSettings As System.Xml.Xsl.XsltSettings = New System.Xml.Xsl.XsltSettings
            xslSettings.EnableDocumentFunction = True
            xslSettings.EnableScript = True

            'Create an XssltArgumentList and Extension objects to it  here
            Dim xslArgs As System.Xml.Xsl.XsltArgumentList = New System.Xml.Xsl.XsltArgumentList()
            'xslArgs.AddExtensionObject("http://exslt.org/math", 
            Dim obj As New Absolute()
            xslArgs.AddExtensionObject("my:abs", obj)

            Dim sReportFileResourcesPath As String = IO.Path.Combine(GCD.GCDProject.ProjectManager.ResourcesFolder.FullName, "ReportFiles")

            ''Add Footer file to the XsltArgumentList - As code evolves more parameters should be added here
            ''http://msdn.microsoft.com/en-us/library/system.web.ui.webcontrols.xmldatasource.transformargumentlist(v=vs.110).aspx
            Dim sReportFooterPath As String = System.IO.Path.Combine(sReportFileResourcesPath, "GCDReportFooter.htm")
            xslArgs.AddParam("sFooterPath", "", "GCDReportFooter.htm")

            Dim reportXSLTransform As New System.Xml.Xsl.XslCompiledTransform()



            'Create folder to house javascript
            Dim sJavaScriptFolder As String = System.IO.Path.Combine(sGCDReportPath, "js")
            If Not System.IO.Directory.Exists(sJavaScriptFolder) Then
                System.IO.Directory.CreateDirectory(sJavaScriptFolder)
                Dim sReportResourcesJSDistPath As String = System.IO.Path.Combine(sReportFileResourcesPath, "dist\js")
                CopyDirectory(sReportResourcesJSDistPath, sJavaScriptFolder)
            End If

            'Create folder to house raster images of report
            Dim sImagesFolder As String = System.IO.Path.Combine(sGCDReportPath, "Images")
            If Not System.IO.Directory.Exists(sImagesFolder) Then
                System.IO.Directory.CreateDirectory(sImagesFolder)
                Dim sImageDistPath As String = System.IO.Path.Combine(sReportFileResourcesPath, "dist\Images")
                If System.IO.Directory.Exists(sImagesFolder) Then
                    CopyDirectory(sImageDistPath, sImagesFolder)
                End If
            End If

            'Copy style sheet
            Dim sStyleDistPath As String = System.IO.Path.Combine(sReportFileResourcesPath, "dist\style.css")
            System.IO.File.Copy(sStyleDistPath, System.IO.Path.Combine(sGCDReportPath, "style.css"))

            Dim rProject As ProjectDS.ProjectRow = GCD.GCDProject.ProjectManager.CurrentProject

            For Each rDEM As ProjectDS.DEMSurveyRow In rProject.GetDEMSurveyRows

                'Get hillshade and add it to display if it exists, add it before DEM so it is below in map
                Dim sHillShadePath As String = GCD.GCDProject.ProjectManager.OutputManager.DEMSurveyHillShadeRasterPath(rDEM.Name)
                Dim hsLayer As MapImages.MapLayer = Nothing
                If IO.File.Exists(sHillShadePath) Then
                    Dim hsRaster As New GISDataStructures.Raster(sHillShadePath)
                    hsLayer = New GISCode.MapImages.MapLayer(hsRaster, "Hillshade")
                End If

                Dim dLayers As Dictionary(Of Integer, MapImages.MapLayer) = GenerateRasterLayerDictionary(rDEM.Source, ArcMap.RasterLayerTypes.DEM, 45, True, hsLayer)

                'Add DEM to dictionary second so it is above hillshade
                Dim sRasterFigurePath As String = System.IO.Path.Combine(sImagesFolder, rDEM.Name & "_" & rDEM.DEMSurveyID.ToString & ".png")
                Me.MapEngine.GenerateMap(dLayers, sRasterFigurePath, True, True, True, 150)

                For Each rAssoc As ProjectDS.AssociatedSurfaceRow In rDEM.GetAssociatedSurfaceRows
                    Try

                        'Get path to associated surface raster, get/apply symbology, tranparency
                        Debug.WriteLine("Processing raster: " & rAssoc.Name)
                        Dim assocSymbologyType As ArcMap.RasterLayerTypes = ProjectDS.GetAssociatedSurfaceType(rAssoc)

                        'Add Associated Surface to Map
                        sRasterFigurePath = System.IO.Path.Combine(sImagesFolder, rAssoc.Name & "_" & rAssoc.AssociatedSurfaceID & ".png")
                        dLayers = GenerateRasterLayerDictionary(rAssoc.Source, assocSymbologyType, 45, True, hsLayer)
                        Me.MapEngine.GenerateMap(dLayers, sRasterFigurePath, True, True, True, 150)

                    Catch ex As Exception
                        Continue For
                    End Try
                Next

                For Each rError As ProjectDS.ErrorSurfaceRow In rDEM.GetErrorSurfaceRows
                    Try

                        'Get path to error surface raster, get/apply symbology, tranparency
                        Debug.WriteLine("Processing raster: " & rError.Name)
                        
                        'Add Error Surface to Map
                        sRasterFigurePath = System.IO.Path.Combine(sImagesFolder, rError.Name & "_" & rError.ErrorSurfaceID & ".png")
                        dLayers = GenerateRasterLayerDictionary(rError.Source, ArcMap.RasterLayerTypes.ErrorSurfaces, 45, True, hsLayer)
                        Me.MapEngine.GenerateMap(dLayers, sRasterFigurePath, True, True, True, 150)

                    Catch ex As Exception
                        Continue For
                    End Try
                Next


            Next

            For Each rDoD As ProjectDS.DoDsRow In rProject.GetDoDsRows

                'Get DoD Threshold Source
                Dim sDoD_RasterPath As String = GCD.GCDProject.ProjectManager.GetAbsolutePath(rDoD.ThreshDoDPath)

                'Add DoD Surface to Map
                Dim sRasterFigurePath As String = System.IO.Path.Combine(sImagesFolder, rDoD.Name & "_" & rDoD.DoDID & ".png")
                Dim dLayers As Dictionary(Of Integer, MapImages.MapLayer) = GenerateRasterLayerDictionary(sDoD_RasterPath, ArcMap.RasterLayerTypes.DoD, 45, True)
                Me.MapEngine.GenerateMap(dLayers, sRasterFigurePath, True, True, True, 150)

                'Set up histogram class to create temporary histogram files

                'Create Linear Unit class
                Dim pLinearUnits As GISCode.LinearUnitClass = New GISCode.LinearUnitClass(GISCode.NumberFormatting.LinearUnits.ft)

                Dim pZedGraphControl As ZedGraph.ZedGraphControl = New ZedGraph.ZedGraphControl()
                Dim pDoDHistogramViewer As DoDHistogramViewerClass = New DoDHistogramViewerClass(pZedGraphControl, GCD.GCDProject.ProjectManager.DisplayUnits.ToString())
                Dim sRawHist As String = GCD.GCDProject.ProjectManager.GetAbsolutePath(rDoD.RawHistPath)
                Dim sThresHist As String = GCD.GCDProject.ProjectManager.GetAbsolutePath(rDoD.ThreshHistPath)

                'Creat DoDResultHistogram
                Dim pDoDResultHistogram As GISCode.GCD.ChangeDetection.DoDResultHistograms = New GISCode.GCD.ChangeDetection.DoDResultHistograms(sRawHist, sThresHist)

                'Create paths in temp folder to save histograms to 
                'Dim jpgAreaHistFilePath As String = GISCode.FileSystem.GetNewSafeFileName(sImagesFolder, rDoD.Name & "AreaHist", "png")
                'Dim jpgVolumeHistFilePath As String = GISCode.FileSystem.GetNewSafeFileName(sImagesFolder, rDoD.Name & "VolumeHist", "png")
                Dim jpgAreaHistFilePath As String = System.IO.Path.Combine(sImagesFolder, rDoD.Name & "AreaHist.png")
                Dim jpgVolumeHistFilePath As String = System.IO.Path.Combine(sImagesFolder, rDoD.Name & "VolumeHist.png")
                Debug.Print(jpgAreaHistFilePath)
                Debug.Print(jpgVolumeHistFilePath)
                pDoDHistogramViewer.ExportCharts(pDoDResultHistogram, pLinearUnits, jpgAreaHistFilePath, jpgVolumeHistFilePath, 300, 300)


            Next

            Dim sReportName As String = GCD.GCDProject.ProjectManager.CurrentProject.Name & "_Report.html"
            Dim sReportPath As String = System.IO.Path.Combine(sGCDReportPath, sReportName)

            Using xReader As System.Xml.XmlReader = System.Xml.XmlReader.Create(sXSLPath, xmlSettings)

                reportXSLTransform.Load(xReader, xslSettings, Nothing)
                'Solution to handling namespace of xml output from data sets http://stackoverflow.com/questions/1756652/how-to-display-xsd-validated-xml-using-xslt
                'reportXSLTransform.Transform(GCD.GCDProject.ProjectManager.FilePath, sReportPath)
                Using fOutput As System.IO.TextWriter = System.IO.File.CreateText(sReportPath)

                    reportXSLTransform.Transform(GCD.GCDProject.ProjectManager.FilePath, xslArgs, fOutput)

                End Using
            End Using

            Using fReader As System.IO.StreamReader = New System.IO.StreamReader(sReportPath)

                sReportString = fReader.ReadToEnd()

            End Using

            Return sReportString

        End Function

        Public Class Absolute

            Public Function MyAbs(ByVal dVal As Double)
                Return Math.Abs(dVal)
            End Function

        End Class

        Public Function GenerateRasterLayerDictionary(ByVal sRasterSourcePath As String,
                                                      ByVal eRasterLayerType As ArcMap.RasterLayerTypes,
                                                      ByVal iTransparency As Integer,
                                                      Optional ByVal bIncludeHillshade As Boolean = False,
                                                      Optional ByVal hsLayer As MapImages.MapLayer = Nothing) As Dictionary(Of Integer, MapImages.MapLayer)

            Dim pMapLayer As MapImages.MapLayer
            Dim dLayers As Dictionary(Of Integer, MapImages.MapLayer) = New Dictionary(Of Integer, MapImages.MapLayer)

            'Get DEM, get/apply symbology, tranparency
            Dim sRasterName As String = System.IO.Path.GetFileName(sRasterSourcePath)
            Dim sRasterPath As String = GCD.GCDProject.ProjectManager.GetAbsolutePath(sRasterSourcePath)
            Dim gRaster As New GISDataStructures.Raster(sRasterPath)
            Dim sSymbology As String = Nothing
            Dim rasterRenderer As ESRI.ArcGIS.Carto.IRasterRenderer = Nothing
            Select Case eRasterLayerType

                Case ArcMap.RasterLayerTypes.InterpolationError
                    'rasterRenderer = GCD.RasterSymbolization.CreateESRIDefinedContinuousRenderer(gRaster, 0, "Slope")
                    rasterRenderer = GCD.RasterSymbolization.CreateClassifyRenderer(gRaster, 11, "Slope")

                Case ArcMap.RasterLayerTypes.PointQuality
                    'rasterRenderer = GCD.RasterSymbolization.CreateESRIDefinedContinuousRenderer(gRaster, 0, "Precipitation")
                    rasterRenderer = GCD.RasterSymbolization.CreateClassifyRenderer(gRaster, 11, "Precipitation", True)

                Case ArcMap.RasterLayerTypes.PointDensity
                    rasterRenderer = GCD.RasterSymbolization.CreateClassifyRenderer(gRaster, 11, "Green to Blue", True)

                    'Check raster statistics to see if it is appropriate to apply our scale
                    If gRaster.Maximum <= 2 And gRaster.Maximum > 0.25 Then
                        rasterRenderer = GCD.RasterSymbolization.CreateClassifyRenderer(gRaster, 11, "Green to Blue", 1.1, True)
                    Else
                        rasterRenderer = GCD.RasterSymbolization.CreateClassifyRenderer(gRaster, 11, "Green to Blue", True)
                    End If

                Case ArcMap.RasterLayerTypes.GrainSizeStatistic
                    Dim eUnits As NumberFormatting.LinearUnits = NumberFormatting.GetLinearUnitsFromString(GCD.GCDProject.ProjectManager.DisplayUnits)
                    rasterRenderer = GCD.RasterSymbolization.CreateGrainSizeStatisticColorRamp(gRaster, eUnits)

                Case ArcMap.RasterLayerTypes.Roughness
                    'sSymbology = GISDataStructures.RasterGCD.GetSymbologyLayerFile(eRasterLayerType)
                    rasterRenderer = GCD.RasterSymbolization.CreateRoughnessColorRamp(gRaster)

                Case ArcMap.RasterLayerTypes.SlopeDegrees
                    rasterRenderer = GCD.RasterSymbolization.CreateSlopeDegreesColorRamp(gRaster)

                Case ArcMap.RasterLayerTypes.SlopePercentRise
                    'sSymbology = GISDataStructures.RasterGCD.GetSymbologyLayerFile(eRasterLayerType)
                    rasterRenderer = GCD.RasterSymbolization.CreateSlopePrecentRiseColorRamp(gRaster)

                Case ArcMap.RasterLayerTypes.DEM
                    sSymbology = GISDataStructures.RasterGCD.GetSymbologyLayerFile(eRasterLayerType)

                Case ArcMap.RasterLayerTypes.DoD
                    rasterRenderer = GISCode.GCD.RasterSymbolization.CreateDoDClassifyRenderer(gRaster, 20)

                Case ArcMap.RasterLayerTypes.ErrorSurfaces
                    'Check raster statistics to see if it is appropriate to apply our scale
                    If gRaster.Minimum = gRaster.Maximum Then
                        rasterRenderer = GCD.RasterSymbolization.CreateESRIDefinedContinuousRenderer(gRaster, 1, "Partial Spectrum")
                    ElseIf gRaster.Maximum <= 1 And gRaster.Maximum > 0.25 Then
                        rasterRenderer = GCD.RasterSymbolization.CreateClassifyRenderer(gRaster, 11, "Partial Spectrum", 1.1)
                    Else
                        rasterRenderer = GCD.RasterSymbolization.CreateClassifyRenderer(gRaster, 11, "Partial Spectrum")
                    End If

                Case ArcMap.RasterLayerTypes.Undefined

                Case Else
                    Throw New Exception("An unrecognized Raster Layer Type was used.")

            End Select

            'Get hillshade and add it to display if it exists, add it before DEM so it is below in map
            If bIncludeHillshade Then
                If TypeOf hsLayer Is MapImages.MapLayer Then
                    dLayers.Add(0, hsLayer)
                End If
            End If

            'Add Raster to dictionary second so it is above hillshade
            If Not String.IsNullOrEmpty(sSymbology) Then
                pMapLayer = New GISCode.MapImages.MapLayer(gRaster, sRasterName, sSymbology, iTransparency)
            ElseIf Not rasterRenderer Is Nothing Then
                pMapLayer = New GISCode.MapImages.MapLayer(gRaster, sRasterName, rasterRenderer, iTransparency)
            Else
                pMapLayer = New GISCode.MapImages.MapLayer(gRaster, sRasterName, iTransparency)
            End If

            'Apply symbology
            'Set the raster renderer. The default renderer will be used if passing a null value.
            dLayers.Add(1, pMapLayer)

            Return dLayers

        End Function

        Public Overrides Function GenerateReport(rProject As ProjectDS.ProjectRow) As IO.StringWriter

            Dim sw As New IO.StringWriter
            Dim ht As New Web.UI.HtmlTextWriter(sw)

            ht.Write("<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">")

            ht.RenderBeginTag(HtmlTextWriterTag.Html)
            ht.RenderBeginTag(HtmlTextWriterTag.Head)

            If CSSFilePath.Exists Then
                InjectCSS(ht)
            End If

            Dim sHeaderFilePath As String = IO.Path.Combine(GCD.GCDProject.ProjectManager.ResourcesFolder.FullName, "ReportFiles")
            sHeaderFilePath = IO.Path.Combine(sHeaderFilePath, "GCDReportHeader.htm")
            If IO.File.Exists(sHeaderFilePath) Then
                InjectFile(ht, sHeaderFilePath)
            End If

            ht.RenderBeginTag(HtmlTextWriterTag.Title)
            ht.RenderEndTag() ' title
            ht.RenderEndTag() 'head
            ht.RenderBeginTag(HtmlTextWriterTag.Body)

            ReportTitle(ht)
            TableOfContents(ht, rProject)

            ht.RenderBeginTag(HtmlTextWriterTag.H1)
            ht.AddAttribute(HtmlTextWriterAttribute.Name, "#Inputs")
            ht.RenderBeginTag(HtmlTextWriterTag.A)
            ht.Write("Inputs")
            ht.RenderEndTag() ' anchor
            ht.RenderEndTag() ' h1

            For Each rDEM As ProjectDS.DEMSurveyRow In rProject.GetDEMSurveyRows
                WriteDEMSurveyHTML(ht, rDEM)
            Next

            ht.AddAttribute(HtmlTextWriterAttribute.Class, "Analyses")
            ht.RenderBeginTag(HtmlTextWriterTag.Div)
            ht.AddAttribute("name", "Analyses")
            ht.RenderBeginTag(HtmlTextWriterTag.A)
            ht.Write("<h1>Analyses</h1>")
            ht.RenderEndTag() 'Anchor
            ht.RenderEndTag() 'Div

            For Each rDoD As ProjectDS.DoDsRow In rProject.GetDoDsRows
                WriteDoDAnalysisHTML(ht, rDoD, "sUnits")
            Next

            Dim sFooterFilePath As String = IO.Path.Combine(GCD.GCDProject.ProjectManager.ResourcesFolder.FullName, "ReportFiles")
            sFooterFilePath = IO.Path.Combine(sFooterFilePath, "GCDReportFooter.htm")
            If IO.File.Exists(sFooterFilePath) Then
                InjectFile(ht, sFooterFilePath)
            End If

            ht.RenderEndTag() ' body
            ht.RenderEndTag()  'html

            Return sw

        End Function

        Public Sub GenerateReportJSON(rProject As ProjectDS.ProjectRow)

            'Create file path in temp drive
            Dim sGCDReportPath As String = WorkspaceManager.GetSafeDirectoryPath(System.IO.Path.Combine(GCDProject.ProjectManager.OutputManager.GCDProjectFolder, "GCD_Report"))
            If Not System.IO.Directory.Exists(sGCDReportPath) Then
                System.IO.Directory.CreateDirectory(sGCDReportPath)
            End If

            Dim sJSONFolder As String = System.IO.Path.Combine(sGCDReportPath, "js")
            If Not System.IO.Directory.Exists(sJSONFolder) Then
                System.IO.Directory.CreateDirectory(sJSONFolder)
            End If

            Dim sJSONPath = GISCode.FileSystem.GetNewSafeFileName(sJSONFolder, "GCD_Report", "json")
            Debug.Print("JSON Path: {0}", sJSONPath)

            Dim fWriter As System.IO.StreamWriter = New System.IO.StreamWriter(sJSONPath)
            fWriter.WriteLine("""Sections""" & ":")

            For Each rDEM As ProjectDS.DEMSurveyRow In rProject.GetDEMSurveyRows
                WriteDEMSurveyJSON(rDEM, sGCDReportPath, fWriter)
            Next


            For Each rDoD As ProjectDS.DoDsRow In rProject.GetDoDsRows
                Dim sDODJSON As String = WriteDoDAnalysisJSON(rDoD, "sUnits", sGCDReportPath)
                fWriter.WriteLine(sDODJSON)
            Next

            fWriter.WriteLine("}")
            fWriter.Close()

        End Sub

        Public Sub ReportTitle(ByRef ht As HtmlTextWriter)

            'ht.AddAttribute(HtmlTextWriterAttribute.Id, "TitleSection")
            'ht.RenderBeginTag(HtmlTextWriterTag.Div)

            ht.AddAttribute(HtmlTextWriterAttribute.Id, "ReportLogo")
            ht.RenderBeginTag(HtmlTextWriterTag.Div)
            InjectImage(ht, "gcd_logo.png", "ReportLogo")
            ht.RenderEndTag() ' div around image

            ht.AddAttribute(HtmlTextWriterAttribute.Id, "Title")
            ht.RenderBeginTag(HtmlTextWriterTag.Div)
            ht.Write("GCD " & System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString & " Analysis Report")
            ht.RenderEndTag() ' div

            'ht.RenderEndTag() ' DIV tag wrapped around header


            ht.AddAttribute("class", "AboutThisReport")
            ht.RenderBeginTag(HtmlTextWriterTag.Div)

            ht.AddAttribute("href", "#AboutThisReport")
            ht.RenderBeginTag(HtmlTextWriterTag.A)
            ht.Write("About This Report")
            ht.RenderEndTag() ' anchor
            ht.RenderEndTag() ' div

        End Sub

        Public Sub TableOfContents(ByRef ht As HtmlTextWriter, rProject As ProjectDS.ProjectRow)

            'Addtion
            ht.RenderBeginTag(HtmlTextWriterTag.Div)
            ht.RenderBeginTag(HtmlTextWriterTag.A)
            ht.AddAttribute(HtmlTextWriterAttribute.Id, "TOC")

            'commented out to remove errors
            'ht.AddAttribute(HtmlTextWriterAttribute.Name, "TOC")
            'ht.RenderBeginTag(HtmlTextWriterTag.H1)
            ht.Write("Table of Contents")
            'ht.RenderEndTag() ' h1
            ht.RenderEndTag() 'Anchor

            ' Inputs / DEM Surveys
            'OLD
            'ht.Write("<div class=""toc1""><a href=""#Inputs"">Inputs</a></div>")

            'NEW - WELL FORMATED HTML
            ht.AddAttribute(HtmlTextWriterAttribute.Class, "toc1")
            ht.AddAttribute("name", "Inputs")
            ht.RenderBeginTag(HtmlTextWriterTag.Div)
            ht.AddAttribute("href", "#Inputs")
            ht.RenderBeginTag(HtmlTextWriterTag.A)
            ht.Write("Inputs")
            ht.RenderEndTag()
            ht.RenderEndTag()
            For Each rDEM As ProjectDS.DEMSurveyRow In rProject.GetDEMSurveyRows

                'OLD
                'ht.Write("<div class=""toc2""><a href=""#DEM_" & rDEM.DEMSurveyID.ToString & """>" & rDEM.Name & "</a></div>")

                'NEW - WELL FORMATED HTML
                ht.AddAttribute(HtmlTextWriterAttribute.Class, "toc2")
                ht.AddAttribute("name", rDEM.Name)
                ht.RenderBeginTag(HtmlTextWriterTag.Div)
                ht.AddAttribute("href", "#DEM_" & rDEM.DEMSurveyID)
                ht.RenderBeginTag(HtmlTextWriterTag.A)
                ht.Write(rDEM.Name)
                ht.RenderEndTag()
                ht.RenderEndTag()


                'OLD Associated Surfaces
                'ht.Write("<div class=""toc3""><A href=""#DEM_" & rDEM.DEMSurveyID.ToString & "_Assoc"">Associated Surfaces</a></div>")

                'NEW - WELL FORMATED HTML
                ht.AddAttribute(HtmlTextWriterAttribute.Class, "toc3")
                ht.RenderBeginTag(HtmlTextWriterTag.Div)
                ht.AddAttribute("href", "#DEM_" & rDEM.DEMSurveyID & "_Assoc")
                ht.RenderBeginTag(HtmlTextWriterTag.A)
                ht.Write("Associated Surfaces")
                ht.RenderEndTag()
                ht.RenderEndTag()

                For Each rAssoc As ProjectDS.AssociatedSurfaceRow In rDEM.GetAssociatedSurfaceRows

                    'OLD
                    'ht.Write("<div class=""toc4""><A href=""#Assoc_" & rAssoc.AssociatedSurfaceID.ToString & """>" & rAssoc.Name & "</a></div>")

                    'NEW - WELL FORMATED HTML
                    ht.AddAttribute(HtmlTextWriterAttribute.Class, "toc4")
                    ht.RenderBeginTag(HtmlTextWriterTag.Div)
                    ht.AddAttribute("href", "#Assoc_" & rAssoc.AssociatedSurfaceID.ToString)
                    ht.RenderBeginTag(HtmlTextWriterTag.A)
                    ht.Write(rAssoc.Name)
                    ht.RenderEndTag()
                    ht.RenderEndTag()

                Next

                ' OLD Error Surfaces
                'ht.Write("<div class=""toc3""><A href=""#DEM_" & rDEM.DEMSurveyID.ToString & "_Assoc"">Error Surfaces</a></div>")

                'NEW - WELL FORMATED HTML
                ht.AddAttribute(HtmlTextWriterAttribute.Class, "toc3")
                ht.RenderBeginTag(HtmlTextWriterTag.Div)
                ht.AddAttribute("href", "#DEM_" & rDEM.DEMSurveyID & "_Error")
                ht.RenderBeginTag(HtmlTextWriterTag.A)
                ht.Write("Error Surfaces")
                ht.RenderEndTag()
                ht.RenderEndTag()

                For Each rError As ProjectDS.ErrorSurfaceRow In rDEM.GetErrorSurfaceRows

                    'OLD
                    'ht.Write("<div class=""toc4""><a href=""#Error_" & rError.ErrorSurfaceID.ToString & """>" & rError.Name & "</a></div>")

                    'NEW - WELL FORMATED HTML
                    ht.AddAttribute(HtmlTextWriterAttribute.Class, "toc4")
                    ht.RenderBeginTag(HtmlTextWriterTag.Div)
                    ht.AddAttribute("href", "#Error_" & rError.ErrorSurfaceID.ToString)
                    ht.RenderBeginTag(HtmlTextWriterTag.A)
                    ht.Write(rError.Name)
                    ht.RenderEndTag()
                    ht.RenderEndTag()
                Next
            Next

            'OLD - Analyses
            'ht.Write("<div class=""toc1""><a href=""#Analyses"">Analyses</a></div>")

            'NEW - WELL FORMATED HTML
            ht.AddAttribute(HtmlTextWriterAttribute.Class, "toc1")
            ht.RenderBeginTag(HtmlTextWriterTag.Div)
            ht.AddAttribute("href", "#Analyses")
            ht.RenderBeginTag(HtmlTextWriterTag.A)
            ht.Write("Analyses")
            ht.RenderEndTag()
            ht.RenderEndTag()

            For Each rDoD As ProjectDS.DoDsRow In rProject.GetDoDsRows
                'OLD 
                'ht.Write("<div class=""toc2""><a href=""#DoD_" & rDoD.DoDID.ToString & """>" & rDoD.Name & "</a></div>")

                'NEW - WELL FORMATED HTML
                ht.AddAttribute(HtmlTextWriterAttribute.Class, "toc2")
                ht.RenderBeginTag(HtmlTextWriterTag.Div)
                ht.AddAttribute("href", "#DoD_" & rDoD.DoDID.ToString)
                ht.RenderBeginTag(HtmlTextWriterTag.A)
                ht.Write(rDoD.Name)
                ht.RenderEndTag()
                ht.RenderEndTag()

                ' Budget Segregations
                'ht.Write("<div class=""toc3""><A href=""#DoD_" & rDoD.DoDID.ToString & "_BS"">Budget Segregations</a></div>")

                'NEW - WELL FORMATED HTML
                ht.AddAttribute(HtmlTextWriterAttribute.Class, "toc3")
                ht.RenderBeginTag(HtmlTextWriterTag.Div)
                ht.AddAttribute("href", "#DoD_" & rDoD.DoDID.ToString & "_BS")
                ht.RenderBeginTag(HtmlTextWriterTag.A)
                ht.Write("Budget Segregations")
                ht.RenderEndTag()
                ht.RenderEndTag()

                For Each rBS As ProjectDS.BudgetSegregationsRow In rDoD.GetBudgetSegregationsRows
                    'OLD
                    'ht.Write("<div class=""toc4""><A href=""#BS_" & rBS.BudgetID.ToString & ">" & rBS.Name & "</a></div>")

                    'NEW - WELL FORMATED HTML
                    ht.AddAttribute(HtmlTextWriterAttribute.Class, "toc4")
                    ht.RenderBeginTag(HtmlTextWriterTag.Div)
                    ht.AddAttribute("href", "#BS_" & rBS.BudgetID.ToString)
                    ht.RenderBeginTag(HtmlTextWriterTag.A)
                    ht.Write(rBS.Name)
                    ht.RenderEndTag()
                    ht.RenderEndTag()
                Next
            Next

            'OLD
            'ht.Write("<div class=""Acknowledgements""><a href=""#Acknowledgements"">Acknowledgements</a></div>")
            'NEW - WELL FORMATED HTML
            ht.AddAttribute(HtmlTextWriterAttribute.Class, "Acknowledgements")
            ht.RenderBeginTag(HtmlTextWriterTag.Div)
            ht.AddAttribute("href", "#Acknowledgements")
            ht.RenderBeginTag(HtmlTextWriterTag.A)
            ht.Write("Acknowledgements")
            ht.RenderEndTag()
            ht.RenderEndTag()

            'OLD
            'ht.Write("<div class=""References""><a href=""#References"">References</a></div>")

            'NEW - WELL FORMATED HTML
            ht.AddAttribute(HtmlTextWriterAttribute.Class, "References")
            ht.RenderBeginTag(HtmlTextWriterTag.Div)
            ht.AddAttribute("href", "#References")
            ht.RenderBeginTag(HtmlTextWriterTag.A)
            ht.Write("References")
            ht.RenderEndTag()
            ht.RenderEndTag()

            'OLD
            'ht.Write("<div class=""Appendices""><a href=""#Appendices"">Appendices</a></div>")

            'NEW - WELL FORMATED HTML
            ht.AddAttribute(HtmlTextWriterAttribute.Class, "Appendices")
            ht.RenderBeginTag(HtmlTextWriterTag.Div)
            ht.AddAttribute("href", "#Appendices")
            ht.RenderBeginTag(HtmlTextWriterTag.A)
            ht.Write("Appendices")
            ht.RenderEndTag()
            ht.RenderEndTag()
            'ht.Write("<div class=""toc1"">Acknowledgements</div>")
            'ht.Write("<div class=""toc1"">Acknowledgements</div>")

            'ht.Write("<div class=""""><a href=#""TopographicDataCollection"">Topographic Data Collection</a></div>")
            'ht.Write("<div class=""""><a href=#""SurveyLibrary"">Survey Library</a></div>")
            'ht.Write("<div class=""""><a href=#""SurveyMethod"">Survey Method</a></div>")
            'ht.Write("<div class=""""><a href=#""TotalStationSurvey"">Total Station Survey</a></div>")
            'ht.Write("<div class=""""><a href=#""rtkGPSSurvey"">rtkGPS Survey</a></div>")
            'ht.Write("<div class=""""><a href=#""AerialPhotogrammetry"">Aerial Photogrammetry</a></div>")
            'ht.Write("<div class=""""><a href=#""Aerial LiDAR"">Aerial LiDAR</a></div>")
            'ht.Write("<div class=""""><a href=#""TerrestrialLaserScanning"">Terrestrial Laser Scanning</a></div>")
            'ht.Write("<div class=""""><a href=#""SingleBeamSONAR"">Single Beam SONAR</a></div>")
            'ht.Write("<div class=""""><a href=#""MultiBeamSONAR"">Multi Beam SONAR</a></div>")
            'ht.Write("<div class=""""><a href=#""BathymetricLiDAR"">Bathymetric LiDAR</a></div>")
            'ht.Write("<div class=""""><a href=#""HybridApproach"">Hybrid Approach</a></div>")
            'ht.Write("<div class=""""><a href=#""Undefined"">Undefined</a></div>")
            'ht.Write("<div class=""""><a href=#""TopographicDataProcessing"">Topographic Data Processing</a></div>")
            'ht.Write("<div class=""""><a href=#""TINGeneration"">Triangular Irregular Network (TIN) Generation</a></div>")
            'ht.Write("<div class=""""><a href=#""TINEditing"">TIN Editing</a></div>")
            'ht.Write("<div class=""""><a href=#""DEMGeneration"">DEM Generation</a></div>")
            'ht.Write("<div class=""""><a href=#""SurveyExtentPolygons"">Survey Extent Polygons</a></div>")

            ''Test to correct errors
            'ht.Write("<div class=""""><a href=""#TopographicDataCollection"">Topographic Data Collection</a></div>")
            'ht.Write("<div class=""""><a href=""#SurveyLibrary"">Survey Library</a></div>")
            'ht.Write("<div class=""""><a href=""#SurveyMethod"">Survey Method</a></div>")
            'ht.Write("<div class=""""><a href=""#TotalStationSurvey"">Total Station Survey</a></div>")
            'ht.Write("<div class=""""><a href=""#rtkGPSSurvey"">rtkGPS Survey</a></div>")
            'ht.Write("<div class=""""><a href=""#AerialPhotogrammetry"">Aerial Photogrammetry</a></div>")
            'ht.Write("<div class=""""><a href=""#Aerial LiDAR"">Aerial LiDAR</a></div>")
            'ht.Write("<div class=""""><a href=""#TerrestrialLaserScanning"">Terrestrial Laser Scanning</a></div>")
            'ht.Write("<div class=""""><a href=""#SingleBeamSONAR"">Single Beam SONAR</a></div>")
            'ht.Write("<div class=""""><a href=""#MultiBeamSONAR"">Multi Beam SONAR</a></div>")
            'ht.Write("<div class=""""><a href=""#BathymetricLiDAR"">Bathymetric LiDAR</a></div>")
            'ht.Write("<div class=""""><a href=""#HybridApproach"">Hybrid Approach</a></div>")
            'ht.Write("<div class=""""><a href=""#Undefined"">Undefined</a></div>")
            'ht.Write("<div class=""""><a href=""#TopographicDataProcessing"">Topographic Data Processing</a></div>")
            'ht.Write("<div class=""""><a href=""#TINGeneration"">Triangular Irregular Network (TIN) Generation</a></div>")
            'ht.Write("<div class=""""><a href=""#TINEditing"">TIN Editing</a></div>")
            'ht.Write("<div class=""""><a href=""#DEMGeneration"">DEM Generation</a></div>")
            'ht.Write("<div class=""""><a href=""#SurveyExtentPolygons"">Survey Extent Polygons</a></div>")

            ht.RenderEndTag() ' TOC DIV
        End Sub

        Private Sub WriteToTableOfContents(ByVal ht As HtmlTextWriter, ByVal tocTag As String, ByVal hRefName As String, ByVal displayName As String)
            'NEW - WELL FORMATED HTML
            ht.AddAttribute(HtmlTextWriterAttribute.Class, tocTag)
            ht.RenderBeginTag(HtmlTextWriterTag.Div)
            ht.AddAttribute("href", hRefName)
            ht.RenderBeginTag(HtmlTextWriterTag.A)
            ht.Write(displayName)
            ht.RenderEndTag()
            ht.RenderEndTag()
        End Sub

        Private Sub WriteDEMSurveyHTML(ByRef ht As HtmlTextWriter, ByVal rDEM As ProjectDS.DEMSurveyRow)

            Dim pMapLayer As MapImages.MapLayer
            Dim dLayers As Dictionary(Of Integer, MapImages.MapLayer) = New Dictionary(Of Integer, MapImages.MapLayer)

            'Get DEM, get/apply symbology, tranparency
            Dim sRasterPath As String = GCD.GCDProject.ProjectManager.GetAbsolutePath(rDEM.Source)
            Dim gDEMRaster As New GISDataStructures.Raster(sRasterPath)
            Dim demSymbology As String = GISDataStructures.RasterGCD.GetSymbologyLayerFile(ArcMap.RasterLayerTypes.DEM)

            'Get hillshade and add it to display if it exists, add it before DEM so it is below in map
            Dim sHillShadePath As String = GCD.GCDProject.ProjectManager.OutputManager.DEMSurveyHillShadeRasterPath(rDEM.Name)
            Dim hsLayer As MapImages.MapLayer = Nothing
            If IO.File.Exists(sHillShadePath) Then
                Dim hsRaster As New GISDataStructures.Raster(sHillShadePath)
                hsLayer = New GISCode.MapImages.MapLayer(hsRaster, "Hillshade")
                dLayers.Add(0, hsLayer)
            End If

            'Add DEM to dictionary second so it is above hillshade
            'Dim aLayer As New GISCode.MapImages.MapLayer(gDEMRaster, rDEM.Name, 45.0)
            pMapLayer = New GISCode.MapImages.MapLayer(gDEMRaster, rDEM.Name, demSymbology, 45)
            dLayers.Add(1, pMapLayer)

            'Create file path in temp drive
            Dim sJpgFilePath As String = GISCode.FileSystem.GetNewSafeFileName(Environ("TEMP"), rDEM.Name & Now.ToString("mmssfff"), "jpg")
            Debug.Print(sJpgFilePath)
            ''Write the DEM Survey tag 
            ''ht.Write("<div class=""DEMSurvey""><a name=""DEM_" & rDEM.DEMSurveyID & """>" & rDEM.Name & "</a></div>")
            ht.AddAttribute(HtmlTextWriterAttribute.Class, "DEMSurvey")
            ht.RenderBeginTag(HtmlTextWriterTag.Div)
            ht.AddAttribute("name", "DEM_" & rDEM.DEMSurveyID)
            ht.RenderBeginTag(HtmlTextWriterTag.A)
            ht.Write("<h1>" & rDEM.Name & "</h1>")
            ht.RenderEndTag() 'Anchor
            ht.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.P)
            ht.Write("The following are the DEM and hillshade generated from survey one used in the change detection analysis.")
            ht.RenderEndTag() 'Paragraph
            InjectRasterImage(ht, sJpgFilePath, dLayers)
            ht.RenderEndTag() 'Div


            'Begin writing Associated surface section
            'ht.AddAttribute(HtmlTextWriterAttribute.Class, "AssociatedSurfaces")
            ht.AddAttribute("name", "DEM_" & rDEM.DEMSurveyID & "_Assoc")
            ht.RenderBeginTag(HtmlTextWriterTag.A)
            ht.Write("<h2>Associated Surfaces</h2>")
            ht.RenderEndTag() 'Anchor
            'ht.Write("<h2><a name=""DEM_" & rDEM.DEMSurveyID & "_Assoc"">Associated Surfaces</a></h2>")

            For Each rAssoc As ProjectDS.AssociatedSurfaceRow In rDEM.GetAssociatedSurfaceRows
                Try
                    WriteAssocSurfaceTag(ht, rAssoc)
                    'ArcMapManager.AddAssociatedSurface(rAssoc)

                    'Get path to associated surface raster, get/apply symbology, tranparency
                    sRasterPath = GCD.GCDProject.ProjectManager.GetAbsolutePath(rAssoc.Source)
                    Debug.WriteLine("Processing raster: " & sRasterPath)
                    Dim assocRaster As New GISDataStructures.Raster(sRasterPath)
                    Dim assocSymbologyType As ArcMap.RasterLayerTypes = ProjectDS.GetAssociatedSurfaceType(rAssoc)
                    Dim assocSymbology = GISCode.GISDataStructures.RasterGCD.GetSymbologyLayerFile(assocSymbologyType)

                    'Add hillshade raster 
                    'MapEngine.Map.AddLayer(hsESRI_Lyr)

                    'Add Associated Surface to Map
                    sJpgFilePath = GISCode.FileSystem.GetNewSafeFileName(Environ("TEMP"), "Assoc_" & rAssoc.AssociatedSurfaceID & Now.ToString("mmssfff"), "jpg")
                    Debug.Print(sJpgFilePath)
                    'MapEngine.Map.AddLayer(symbolizedAssocRaster)
                    'MapEngine.MapDocument.ActiveView.ContentsChanged()
                    'MapEngine.MapDocument.ActiveView.Refresh()


                    pMapLayer = New MapImages.MapLayer(assocRaster, rAssoc.Type, assocSymbology, 45)
                    dLayers = New Dictionary(Of Integer, MapImages.MapLayer)
                    dLayers.Add(0, hsLayer)
                    dLayers.Add(1, pMapLayer)
                    InjectRasterImage(ht, sJpgFilePath, dLayers)

                Catch ex As Exception
                    Continue For
                Finally
                    'dLayers.Remove(1)
                End Try
            Next

            'Close the associated surfaces class
            'ht.RenderEndTag()


            'ht.Write("<h2><a name=""DEM_" & rDEM.DEMSurveyID & "_Error"">Error Surfaces</a></h2>")
            ht.AddAttribute("name", "DEM_" & rDEM.DEMSurveyID & "_Error")
            ht.RenderBeginTag(HtmlTextWriterTag.A)
            ht.Write("<h2>Error Surfaces</h2>")
            ht.RenderEndTag() 'Anchor

            For Each rError As ProjectDS.ErrorSurfaceRow In rDEM.GetErrorSurfaceRows
                Try

                    WriteErrorSurfaceTag(ht, rError)
                    'ArcMapManager.AddErrorSurface(rAssoc)

                    'Get path to error surface raster, get/apply symbology, tranparency
                    sRasterPath = GCD.GCDProject.ProjectManager.GetAbsolutePath(rError.Source)
                    Debug.WriteLine("Processing raster: " & sRasterPath)
                    Dim errorRaster As New GISDataStructures.Raster(sRasterPath)
                    Dim errorSymbology = GISDataStructures.RasterGCD.GetSymbologyLayerFile(ArcMap.RasterLayerTypes.ErrorSurfaces)
                    Dim symbolizedErrorRaster = errorRaster.ApplySymbology(errorSymbology)
                    Dim pErrorLayerEffects As ESRI.ArcGIS.Carto.ILayerEffects = symbolizedErrorRaster
                    pErrorLayerEffects.Transparency = 45

                    'Add Error Surface to Map
                    sJpgFilePath = GISCode.FileSystem.GetNewSafeFileName(Environ("TEMP"), "Assoc_" & rError.ErrorSurfaceID & Now.ToString("mmssfff"), "jpg")
                    Debug.Print(sJpgFilePath)
                    pMapLayer = New MapImages.MapLayer(errorRaster, "Error", errorSymbology, 45)
                    dLayers = New Dictionary(Of Integer, MapImages.MapLayer)
                    dLayers.Add(0, hsLayer)
                    dLayers.Add(1, pMapLayer)
                    InjectRasterImage(ht, sJpgFilePath, dLayers)

                Catch ex As Exception
                    Continue For
                End Try
            Next

        End Sub

        Private Sub WriteDEMSurveyJSON(ByVal rDEM As ProjectDS.DEMSurveyRow, ByVal sGCDReportPath As String, ByRef fWriter As System.IO.StreamWriter)

            Dim pMapLayer As MapImages.MapLayer
            Dim dLayers As Dictionary(Of Integer, MapImages.MapLayer) = New Dictionary(Of Integer, MapImages.MapLayer)

            'Get DEM, get/apply symbology, tranparency
            Dim sRasterPath As String = GCD.GCDProject.ProjectManager.GetAbsolutePath(rDEM.Source)
            Dim gDEMRaster As New GISDataStructures.Raster(sRasterPath)
            Dim demSymbology As String = GISDataStructures.RasterGCD.GetSymbologyLayerFile(ArcMap.RasterLayerTypes.DEM)

            'Get hillshade and add it to display if it exists, add it before DEM so it is below in map
            Dim sHillShadePath As String = GCD.GCDProject.ProjectManager.OutputManager.DEMSurveyHillShadeRasterPath(rDEM.Name)
            Dim hsLayer As MapImages.MapLayer = Nothing
            If IO.File.Exists(sHillShadePath) Then
                Dim hsRaster As New GISDataStructures.Raster(sHillShadePath)
                hsLayer = New GISCode.MapImages.MapLayer(hsRaster, "Hillshade")
                dLayers.Add(0, hsLayer)
            End If

            'Add DEM to dictionary second so it is above hillshade
            'Dim aLayer As New GISCode.MapImages.MapLayer(gDEMRaster, rDEM.Name, 45.0)
            pMapLayer = New GISCode.MapImages.MapLayer(gDEMRaster, rDEM.Name, demSymbology, 45)
            dLayers.Add(1, pMapLayer)

            'Create file path in temp drive
            Dim sCDFImageFolderPath As String = System.IO.Path.Combine(sGCDReportPath, "Images")
            'Create Folders
            If Not System.IO.Directory.Exists(sCDFImageFolderPath) Then
                System.IO.Directory.CreateDirectory(sCDFImageFolderPath)
            End If

            Dim sRasterFigurePath As String = GISCode.FileSystem.GetNewSafeFileName(sCDFImageFolderPath, rDEM.Name & rDEM.DEMSurveyID.ToString, ".jpg")
            Dim sRasterFigureFileName As String = System.IO.Path.GetFileName(sRasterFigurePath)
            Dim sRelativeRasterFigurePath = "Images/" & sRasterFigureFileName
            Debug.Print(sRasterFigurePath)

            MapEngine.GenerateMap(dLayers, sRasterFigurePath, True, True, True)

            'JSON Creation
            Dim sHeaderArray As String() = New String() {}
            Dim sRowArray()() As String = New String()() {}

            Dim sAuxillaryArray As Dictionary(Of String, Object) = New Dictionary(Of String, Object) From {{"Name:", rDEM.Name},
                                                                                                           {"demID", rDEM.DEMSurveyID},
                                                                                                           {"Mean", gDEMRaster.Mean},
                                                                                                           {"Max", gDEMRaster.Maximum},
                                                                                                           {"Minimum", gDEMRaster.Minimum},
                                                                                                           {"StDev", gDEMRaster.StandardDeviation},
                                                                                                           {"Survey Date", rDEM.SurveyDate}}

            Dim sImagePathArray As String() = New String(0) {sRelativeRasterFigurePath}
            Dim sImageNameArray As String() = New String(0) {rDEM.Name}
            Dim sJSON As String = CreateJSONSection(rDEM.Name & rDEM.DEMSurveyID, "This is filler text. A description of the Raster", sHeaderArray, sRowArray, sImagePathArray, sImageNameArray, sAuxillaryArray, sGCDReportPath)
            fWriter.WriteLine(sJSON)

            For Each rAssoc As ProjectDS.AssociatedSurfaceRow In rDEM.GetAssociatedSurfaceRows
                Try

                    'Get path to associated surface raster, get/apply symbology, tranparency
                    sRasterPath = GCD.GCDProject.ProjectManager.GetAbsolutePath(rAssoc.Source)
                    Debug.WriteLine("Processing raster: " & sRasterPath)
                    Dim gAssocRaster As New GISDataStructures.Raster(sRasterPath)
                    Dim assocSymbologyType As ArcMap.RasterLayerTypes = ProjectDS.GetAssociatedSurfaceType(rAssoc)
                    Dim assocSymbology = GISCode.GISDataStructures.RasterGCD.GetSymbologyLayerFile(assocSymbologyType)

                    'Add Associated Surface to Map
                    sRasterFigurePath = GISCode.FileSystem.GetNewSafeFileName(sCDFImageFolderPath, rAssoc.Name & rAssoc.AssociatedSurfaceID, ".jpg")
                    sRasterFigureFileName = System.IO.Path.GetFileName(sRasterFigurePath)
                    sRelativeRasterFigurePath = "Images/" & sRasterFigureFileName
                    Debug.Print(sRasterFigurePath)

                    pMapLayer = New MapImages.MapLayer(gAssocRaster, rAssoc.Type, assocSymbology, 45)
                    dLayers = New Dictionary(Of Integer, MapImages.MapLayer)
                    dLayers.Add(0, hsLayer)
                    dLayers.Add(1, pMapLayer)
                    MapEngine.GenerateMap(dLayers, sRasterFigurePath, True, True, True)

                    'JSON Creation
                    sHeaderArray = New String() {}
                    sRowArray = New String()() {}

                    sAuxillaryArray = New Dictionary(Of String, Object) From {{"Name", rAssoc.Name},
                                                                              {"Surface Type:", rAssoc.Type},
                                                                              {"Mean", gAssocRaster.Mean},
                                                                              {"Max", gAssocRaster.Maximum},
                                                                              {"Minimum", gAssocRaster.Minimum},
                                                                              {"StDev", gAssocRaster.StandardDeviation},
                                                                              {"demID", rAssoc.DEMSurveyID}}

                    sImagePathArray = New String(0) {sRelativeRasterFigurePath}
                    sImageNameArray = New String(0) {rAssoc.Name}
                    sJSON = CreateJSONSection(rAssoc.Name & rAssoc.AssociatedSurfaceID, "This is filler text. A description of the Raster", sHeaderArray, sRowArray, sImagePathArray, sImageNameArray, sAuxillaryArray, sGCDReportPath)
                    fWriter.WriteLine(sJSON)

                Catch ex As Exception
                    Continue For
                Finally
                    'dLayers.Remove(1)
                End Try
            Next

            For Each rError As ProjectDS.ErrorSurfaceRow In rDEM.GetErrorSurfaceRows
                Try

                    'Get path to error surface raster, get/apply symbology, tranparency
                    sRasterPath = GCD.GCDProject.ProjectManager.GetAbsolutePath(rError.Source)
                    Debug.WriteLine("Processing raster: " & sRasterPath)
                    Dim gErrorRaster As New GISDataStructures.Raster(sRasterPath)
                    Dim errorSymbology = GISDataStructures.RasterGCD.GetSymbologyLayerFile(ArcMap.RasterLayerTypes.ErrorSurfaces)
                    Dim symbolizedErrorRaster = gErrorRaster.ApplySymbology(errorSymbology)
                    Dim pErrorLayerEffects As ESRI.ArcGIS.Carto.ILayerEffects = symbolizedErrorRaster
                    pErrorLayerEffects.Transparency = 45

                    'Add Error Surface to Map
                    sRasterFigurePath = GISCode.FileSystem.GetNewSafeFileName(sCDFImageFolderPath, rError.Name & rError.ErrorSurfaceID, ".jpg")
                    sRasterFigureFileName = System.IO.Path.GetFileName(sRasterFigurePath)
                    sRelativeRasterFigurePath = "Images/" & sRasterFigureFileName
                    Debug.Print(sRasterFigurePath)

                    pMapLayer = New MapImages.MapLayer(gErrorRaster, "Error", errorSymbology, 45)
                    dLayers = New Dictionary(Of Integer, MapImages.MapLayer)
                    dLayers.Add(0, hsLayer)
                    dLayers.Add(1, pMapLayer)
                    MapEngine.GenerateMap(dLayers, sRasterFigurePath, True, True, True)

                    'JSON Creation
                    sHeaderArray = New String() {}
                    sRowArray = New String()() {}

                    sAuxillaryArray = New Dictionary(Of String, Object) From {{"Name", rError.Name},
                                                                              {"Error Surface Type:", rError.Type},
                                                                              {"Mean", gErrorRaster.Mean},
                                                                              {"Max", gErrorRaster.Maximum},
                                                                              {"Minimum", gErrorRaster.Minimum},
                                                                              {"StDev", gErrorRaster.StandardDeviation},
                                                                              {"demID", rError.DEMSurveyID}}

                    sImagePathArray = New String(0) {sRelativeRasterFigurePath}
                    sImageNameArray = New String(0) {rError.Name}
                    sJSON = CreateJSONSection(rError.Name & rError.ErrorSurfaceID, "This is filler text. A description of the Raster", sHeaderArray, sRowArray, sImagePathArray, sImageNameArray, sAuxillaryArray, sGCDReportPath)
                    fWriter.WriteLine(sJSON)

                Catch ex As Exception
                    Continue For
                End Try
            Next

        End Sub


        Private Sub WriteAssocSurfaceTag(ByRef ht As HtmlTextWriter, ByVal rAssoc As ProjectDS.AssociatedSurfaceRow)

            'ht.Write("<div class=""AssociatedSurfaceRaster"">")
            'ht.Write("<h1><a name=""Assoc_" & rAssoc.AssociatedSurfaceID & """>" & rAssoc.Name & "</a></h1>")
            'ht.Write("</div>")

            ht.AddAttribute(HtmlTextWriterAttribute.Class, "AssociatedSurfaces")
            ht.RenderBeginTag(HtmlTextWriterTag.Div)
            ht.AddAttribute("name", "Assoc_" & rAssoc.AssociatedSurfaceID)
            ht.RenderBeginTag(HtmlTextWriterTag.A)
            ht.Write("<h1>" & rAssoc.Name & "</h1>")
            ht.RenderEndTag() 'Anchor
            ht.RenderEndTag() 'Div

        End Sub


        Private Sub WriteErrorSurfaceTag(ByRef ht As HtmlTextWriter, ByVal rError As ProjectDS.ErrorSurfaceRow)

            'ht.Write("<div class=""ErrorSurfaceRaster"">")
            'ht.Write("<h1><a name=""Error_" & rError.ErrorSurfaceID & """>" & rError.Name & "</a></h1>")
            'ht.Write("</div>")

            ht.AddAttribute(HtmlTextWriterAttribute.Class, "ErrorSurfaceRaster")
            ht.RenderBeginTag(HtmlTextWriterTag.Div)
            ht.AddAttribute("name", "Error_" & rError.ErrorSurfaceID)
            ht.RenderBeginTag(HtmlTextWriterTag.A)
            ht.Write("<h1>" & rError.Name & "</h1>")
            ht.RenderEndTag() 'Anchor
            ht.RenderEndTag() 'Div

        End Sub

        Private Sub WriteDoDAnalysisHTML(ByRef ht As HtmlTextWriter, ByVal rDoD As ProjectDS.DoDsRow, ByVal sUnits As String)

            'OLD
            'ht.Write("<div class=""DoD_Analysis""><a name=""DoD_" & rDoD.DoDID & """>" & rDoD.Name & "</a></div>")

            ht.RenderBeginTag(HtmlTextWriterTag.Div)
            ht.AddAttribute("name", "DoD_" & rDoD.DoDID)
            ht.RenderBeginTag(HtmlTextWriterTag.A)
            ht.Write("<h1>" & rDoD.Name & "</h1>")
            ht.RenderEndTag() 'Anchor
            ht.RenderEndTag() 'Div

            'OLD
            'ht.Write("<p>The following are the DoD results for change detection analysis.</p>")

            ht.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.P)
            ht.Write("The following are the DoD results for change detection analysis.")
            ht.RenderEndTag()

            'OLD
            'ht.Write("<h2><a name=""DoD_" & rDoD.DoDID & "_Budget"">Budget Segregation</a></h2>")

            ht.AddAttribute("name", "DoD_" & rDoD.DoDID & "_Budget")
            ht.RenderBeginTag(HtmlTextWriterTag.A)
            ht.Write("<h1>Budget Segregation</h1>")
            ht.RenderEndTag() 'Anchor

            WriteSummaryDoDTable(ht, rDoD)

            'Get DoD, get/apply symbology, tranparency
            Dim sDoD_RasterPath As String = GCD.GCDProject.ProjectManager.GetAbsolutePath(rDoD.ThreshDoDPath)
            Dim dodRaster As New GISDataStructures.Raster(sDoD_RasterPath)
            Dim dodSymbology As String = GISDataStructures.RasterGCD.GetSymbologyLayerFile(ArcMap.RasterLayerTypes.DoD)

            Dim sJpgFilePath As String = GISCode.FileSystem.GetNewSafeFileName(Environ("TEMP"), rDoD.Name & Now.ToString("mmssfff"), "jpg")
            Debug.Print(sJpgFilePath)
            Dim pMapLayer As MapImages.MapLayer = New MapImages.MapLayer(dodRaster, "Elevation Difference", dodSymbology, 45)
            Dim dLayers As Dictionary(Of Integer, MapImages.MapLayer) = New Dictionary(Of Integer, MapImages.MapLayer)
            dLayers.Add(0, pMapLayer)
            InjectRasterImage(ht, sJpgFilePath, dLayers)

            'Set up histogram class to create temporary histogram files
            Dim pZedGraphControl As ZedGraph.ZedGraphControl = New ZedGraph.ZedGraphControl()
            Dim pDoDHistogramViewer As DoDHistogramViewerClass = New DoDHistogramViewerClass(pZedGraphControl, sUnits)
            Dim sRawHist As String = GCD.GCDProject.ProjectManager.GetAbsolutePath(rDoD.RawHistPath)
            Dim sThresHist As String = GCD.GCDProject.ProjectManager.GetAbsolutePath(rDoD.ThreshHistPath)

            'Creat DoDResultHistogram
            Dim pDoDResultHistogram As GISCode.GCD.ChangeDetection.DoDResultHistograms = New GISCode.GCD.ChangeDetection.DoDResultHistograms(sRawHist, sThresHist)

            'Create paths in temp folder to save histograms to 
            Dim jpgAreaHistFilePath As String = GISCode.FileSystem.GetNewSafeFileName(Environ("TEMP"), rDoD.Name & "AreaHist" & Now.ToString("mmssfff"), "jpg")
            Dim jpgVolumeHistFilePath As String = GISCode.FileSystem.GetNewSafeFileName(Environ("TEMP"), rDoD.Name & "VolumeHist" & Now.ToString("mmssfff"), "jpg")
            Debug.Print(jpgAreaHistFilePath)
            Debug.Print(jpgVolumeHistFilePath)

            'Create Linear Unit class
            Dim pLinearUnits As GISCode.LinearUnitClass = New GISCode.LinearUnitClass(GISCode.NumberFormatting.LinearUnits.ft)
            pDoDHistogramViewer.ExportCharts(pDoDResultHistogram, pLinearUnits, jpgAreaHistFilePath, jpgVolumeHistFilePath, 300, 300)

            'Insert histograms
            InjectImage(ht, jpgAreaHistFilePath, "RasterImage", jpgAreaHistFilePath)
            InjectImage(ht, jpgVolumeHistFilePath, "RasterImage", jpgVolumeHistFilePath)

            For Each rBS As ProjectDS.BudgetSegregationsRow In rDoD.GetBudgetSegregationsRows
                Try

                    WriteBudgetSegregation(ht, rBS)

                Catch ex As Exception
                    Continue For
                End Try

            Next

        End Sub

        Private Function WriteDoDAnalysisJSON(ByVal rDoD As ProjectDS.DoDsRow, ByVal sUnits As String, ByVal sGCDReportPath As String) As String

            'Get DoD, get/apply symbology, tranparency
            Dim sDoD_RasterPath As String = GCD.GCDProject.ProjectManager.GetAbsolutePath(rDoD.ThreshDoDPath)
            Dim dodRaster As New GISDataStructures.Raster(sDoD_RasterPath)
            Dim dodSymbology As String = GISDataStructures.RasterGCD.GetSymbologyLayerFile(ArcMap.RasterLayerTypes.DoD)

            'Get GCD Report Folder & GCD Report Images Folder
            Dim sCDFImageFolderPath As String = System.IO.Path.Combine(sGCDReportPath, "Images")


            Dim sRasterFigurePath As String = GISCode.FileSystem.GetNewSafeFileName(sCDFImageFolderPath, rDoD.Name & rDoD.DoDID.ToString(), ".jpg")
            Dim sRasterFigureFileName As String = System.IO.Path.GetFileName(sRasterFigurePath)
            Dim sRelativeRasterFigurePath = "Images/" & sRasterFigureFileName

            Debug.Print(sRasterFigurePath)
            Dim pMapLayer As MapImages.MapLayer = New MapImages.MapLayer(dodRaster, "Elevation Difference", dodSymbology, 45)
            Dim dLayers As Dictionary(Of Integer, MapImages.MapLayer) = New Dictionary(Of Integer, MapImages.MapLayer)
            dLayers.Add(0, pMapLayer)
            MapEngine.GenerateMap(dLayers, sRasterFigurePath, True, True, True)

            'Set up histogram class to create temporary histogram files
            Dim pZedGraphControl As ZedGraph.ZedGraphControl = New ZedGraph.ZedGraphControl()
            Dim pDoDHistogramViewer As DoDHistogramViewerClass = New DoDHistogramViewerClass(pZedGraphControl, sUnits)
            Dim sRawHist As String = GCD.GCDProject.ProjectManager.GetAbsolutePath(rDoD.RawHistPath)
            Dim sThresHist As String = GCD.GCDProject.ProjectManager.GetAbsolutePath(rDoD.ThreshHistPath)

            'Creat DoDResultHistogram
            Dim pDoDResultHistogram As GISCode.GCD.ChangeDetection.DoDResultHistograms = New GISCode.GCD.ChangeDetection.DoDResultHistograms(sRawHist, sThresHist)

            'Create paths in temp folder to save histograms to 
            Dim sAreaHistFilePath As String = GISCode.FileSystem.GetNewSafeFileName(sCDFImageFolderPath, "AreaHistogram" & rDoD.Name & rDoD.DoDID.ToString(), ".jpg")
            Dim sAreaHistFileName As String = System.IO.Path.GetFileName(sAreaHistFilePath)
            Dim sAreaHistRelativePath = "Images/" & sAreaHistFileName

            Dim sVolumeHistFilePath As String = GISCode.FileSystem.GetNewSafeFileName(sCDFImageFolderPath, "VolumeHistogram" & rDoD.Name & rDoD.DoDID.ToString(), ".jpg")
            Dim sVolumeHistFileName As String = System.IO.Path.GetFileName(sVolumeHistFilePath)
            Dim sVolumeHistRelativePath = "Images/" & sVolumeHistFileName

            Debug.Print(sAreaHistFilePath)
            Debug.Print(sVolumeHistFilePath)

            'Create Linear Unit class
            Dim pLinearUnits As GISCode.LinearUnitClass = New GISCode.LinearUnitClass(GISCode.NumberFormatting.LinearUnits.ft)
            pDoDHistogramViewer.ExportCharts(pDoDResultHistogram, pLinearUnits, sAreaHistFilePath, sVolumeHistFilePath, 300, 300)

            Dim sHeaderArray As String() = {"Attribute", "Raw", "Thresholded DoD Estimate", "", "±Error Volume", "%Error"}
            Dim sFormat As String = "#,##0.00"
            Dim sRowArray()() As String = WriteJSONDoDTable(rDoD)

            Dim sAuxillaryArray As Dictionary(Of String, Object) = New Dictionary(Of String, Object) From {{"Name:", rDoD.Name},
                                                                                                           {"Mean", dodRaster.Mean},
                                                                                                           {"Max", dodRaster.Maximum},
                                                                                                           {"Minimum", dodRaster.Minimum},
                                                                                                           {"StDev", dodRaster.StandardDeviation}}

            Dim sImagePathArray As String() = New String(2) {sRelativeRasterFigurePath, sAreaHistRelativePath, sVolumeHistRelativePath}
            Dim sImageNameArray As String() = New String(2) {"DoD", "Raw DoD Histogram", "Thresholded DoD Histogram"}
            Dim sJSON = CreateJSONSection(rDoD.Name, "This is filler text. A description of the DoD", sHeaderArray, sRowArray, sImagePathArray, sImageNameArray, sAuxillaryArray, sGCDReportPath)
            Return sJSON

        End Function

        Private Sub WriteBudgetSegregation(ByRef ht As HtmlTextWriter, ByVal rBS As ProjectDS.BudgetSegregationsRow)

            'ht.Write("<div class=""BudgetSegregation"">")
            'ht.Write("<h1><a name=""Budget_" & rBS.BudgetID & """>" & rBS.Name & "</a></h1>")
            'ht.Write("</div>")

            ht.AddAttribute(HtmlTextWriterAttribute.Class, "BudgetSegregation")
            ht.RenderBeginTag(HtmlTextWriterTag.Div)
            ht.AddAttribute("name", "Budget_" & rBS.BudgetID)
            ht.RenderBeginTag(HtmlTextWriterTag.A)
            ht.Write("<h1>" & rBS.Name & "</h1>")
            ht.RenderEndTag() 'Anchor
            ht.RenderEndTag() 'Div
        End Sub


        Private Sub WriteSummaryDoDTable(ByRef ht As HtmlTextWriter, ByVal rDoD As ProjectDS.DoDsRow)

            Dim chngStats As New GCD.ChangeDetection.ChangeStatsFromDoDRow(rDoD)

            'TODO: provide this class the ability to access the units of the project
            Dim finishedHTML_Table As String = chngStats.GetSummaryStatsAsHTML("#,##0.00", GISCode.NumberFormatting.LinearUnits.ft)
            ht.Write(finishedHTML_Table)
            'ht.Write("<caption>This is a test caption for the DoD table for " & rDoD.Name & ". It contains summary info on the amount of geomorphic change</caption>")

        End Sub

        Private Function WriteJSONDoDTable(ByVal rDoD As ProjectDS.DoDsRow) As String()()

            Dim chngStats As New GCD.ChangeDetection.ChangeStatsFromDoDRow(rDoD)
            Dim jsonDoDTable As String()() = chngStats.GetSummaryStatsAsJSON("#,##0.00", GISCode.NumberFormatting.LinearUnits.m)
            Return jsonDoDTable

        End Function

        Public Shared Function SaveHTML(ByVal sb As String, ByVal sHTMLpath As String) As Boolean


            Using pStreamWriter As New IO.StreamWriter(sHTMLpath)
                pStreamWriter.Write(sb.ToString)
            End Using

            If IO.File.Exists(sHTMLpath) Then
                Return True
            Else
                Return False
            End If

        End Function

        Private Function CreateJSONSection(ByVal sSectionTitle As String,
                                           ByVal sDescription As String,
                                           ByVal sHeadersArray As String(),
                                           ByVal sRowArray()() As String,
                                           ByVal sImagePathArray As String(),
                                           ByVal sImageNameArray As String(),
                                           ByVal sAuxiallryArray As Dictionary(Of String, Object),
                                           ByVal sGCDReportPath As String) As String

            'Dim jsonDictionary As Dictionary(Of String, Dictionary(Of String, Object)) = New Dictionary(Of String, Dictionary(Of String, Object))

            Dim sectionDictionary As Dictionary(Of String, Object) = New Dictionary(Of String, Object)
            Dim dTable As Dictionary(Of String, Object) = New Dictionary(Of String, Object)
            dTable.Add("header", sHeadersArray)
            dTable.Add("rows", sRowArray)


            Dim dataDictionary As Dictionary(Of String, Object) = New Dictionary(Of String, Object)
            dataDictionary.Add("title", sSectionTitle)
            dataDictionary.Add("description", sDescription)
            dataDictionary.Add("Table", dTable)
            Dim sImageArray As String()() = New String(sImagePathArray.Length - 1)() {}
            For i As UInteger = 0 To sImagePathArray.Length - 1
                dataDictionary.Add("image" & i, New String(1) {sImagePathArray(i), sImageNameArray(i)})
            Next

            dataDictionary.Add("results", sAuxiallryArray)
            sectionDictionary.Add(sSectionTitle, dataDictionary)
            'jsonDictionary.Add("Sections", sectionDictionary)

            Dim tParse As System.Web.Script.Serialization.JavaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer()
            Dim sJSON As String = tParse.Serialize(sectionDictionary)
            Return sJSON
            'tJSON = "Sections:" & tJSON

            'Dim sJSONFolder As String = System.IO.Path.Combine(sGCDReportPath, "js")
            'If Not System.IO.Directory.Exists(sJSONFolder) Then
            '    System.IO.Directory.CreateDirectory(sJSONFolder)
            'End If

            'Dim sJSONPath = GISCode.FileSystem.GetNewSafeFileName(sJSONFolder, sSectionTitle, "json")
            'Debug.Print("JSON Path: {0}", sJSONPath)
            'Dim tWriter As System.IO.StreamWriter = New System.IO.StreamWriter(sJSONPath)
            'tWriter.Write(tJSON)
            'tWriter.Close()

            'Return sectionDictionary

        End Function

        Private Sub CopyDirectory(ByVal sSourcePath As String, ByVal sDestinationPath As String)

            If Not System.IO.Directory.Exists(sDestinationPath) Then
                System.IO.Directory.CreateDirectory(sDestinationPath)
            End If

            For Each sFile As String In System.IO.Directory.GetFiles(sSourcePath)
                Dim sDestination As String = System.IO.Path.Combine(sDestinationPath, System.IO.Path.GetFileName(sFile))
                System.IO.File.Copy(sFile, sDestination)
            Next

            For Each sFolder As String In System.IO.Directory.GetDirectories(sSourcePath)
                Dim sDestination As String = System.IO.Path.Combine(sDestinationPath, System.IO.Path.GetFileName(sFolder))
                CopyDirectory(sFolder, sDestination)
            Next

        End Sub


    End Class

End Namespace