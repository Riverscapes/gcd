Imports ESRI.ArcGIS.Carto

Namespace GISCode.GCD

    Module RasterSymbolization

        Public Function CreateESRIDefinedContinuousRenderer(ByVal gRaster As GISDataStructures.Raster,
                                                            ByVal iClassCount As Integer,
                                                            ByVal sColorRampName As String,
                                                            Optional ByVal bInvert As Boolean = False) As IRasterRenderer

            'This method is in transition mode, it needs to be reworked to symbolize all rasters in GCD
            'To do this alogrithmic color ramps, MultiPart ColorRamps from ESRI, and pre-programed color ramps need to be supported 

            Try
                'Create continous render aka ColorMapRenderer
                Dim stretchRenderer As RasterStretchColorRampRenderer = New RasterStretchColorRampRenderer()
                Dim rasterRenderer As IRasterRenderer = CType(stretchRenderer, IRasterRenderer)

                'Set up the renderer properties.
                Dim rasterDataset As ESRI.ArcGIS.Geodatabase.IRasterDataset = gRaster.RasterDataset
                Dim raster As ESRI.ArcGIS.Geodatabase.IRaster = rasterDataset.CreateDefaultRaster()
                rasterRenderer.Raster = raster

                'Create Color Ramp
                Dim pColorRamp As ESRI.ArcGIS.Display.IColorRamp = Nothing '= New RasterStretchColorRampRenderer()
                Dim pStyleItem As ESRI.ArcGIS.Display.IStyleGalleryItem = GetESRIStyleColorRamp(pColorRamp, sColorRampName)

                Dim pRenderColorRamp As IRasterRendererColorRamp = CType(rasterRenderer, IRasterRendererColorRamp)
                pRenderColorRamp.ColorScheme = pStyleItem.Name

                'Set Legend Items
                Dim pStretchInfo As IRasterStretchMinMax = CType(stretchRenderer, IRasterStretchMinMax)
                pStretchInfo.CustomStretchMin = 0
                Dim iRound As Integer = GetMagnitude(gRaster.Maximum)
                pStretchInfo.CustomStretchMax = Math.Round(gRaster.Maximum, Math.Abs(iRound))
                pStretchInfo.UseCustomStretchMinMax = True

                ''Apply advanced labeling
                'Dim pAdvancedRasterLabel As IRasterStretchAdvancedLabels = CType(pStretchInfo, IRasterStretchAdvancedLabels)
                'pAdvancedRasterLabel.LabelText(0) = Math.Round(gRaster.Maximum, Math.Abs(iRound)).ToString()
                'pAdvancedRasterLabel.LabelValue(0) = Math.Round(gRaster.Maximum, Math.Abs(iRound))
                'pAdvancedRasterLabel.LabelText(1) = "0.0"
                'pAdvancedRasterLabel.LabelValue(0) = 0.0
                'pAdvancedRasterLabel.UseAdvancedLabeling = True

                stretchRenderer.LabelHigh = Math.Round(gRaster.Maximum, Math.Abs(iRound)).ToString()
                stretchRenderer.LabelLow = "0.0"

                'Invert color ramp if flagged to do so
                If bInvert Then
                    Dim pStretch As IRasterStretch2 = CType(stretchRenderer, IRasterStretch2)
                    pStretch.Invert = True
                End If

                rasterRenderer.Update()
                Return rasterRenderer

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine(ex.Message)
                Return Nothing
            End Try

        End Function

        Public Function CreateESRIDefinedContinuousRenderer(ByVal gRaster As GISDataStructures.Raster,
                                                            ByVal sColorRampName As String,
                                                            Optional ByVal bInvert As Boolean = False) As IRasterRenderer

            'This method is in transition mode, it needs to be reworked to symbolize all rasters in GCD
            'To do this alogrithmic color ramps, MultiPart ColorRamps from ESRI, and pre-programed color ramps need to be supported 

            Try
                'Create continous render aka ColorMapRenderer
                Dim stretchRenderer As RasterStretchColorRampRenderer = New RasterStretchColorRampRenderer()
                Dim rasterRenderer As IRasterRenderer = CType(stretchRenderer, IRasterRenderer)

                'Set up the renderer properties.
                Dim rasterDataset As ESRI.ArcGIS.Geodatabase.IRasterDataset = gRaster.RasterDataset
                Dim raster As ESRI.ArcGIS.Geodatabase.IRaster = rasterDataset.CreateDefaultRaster()
                rasterRenderer.Raster = raster

                'Create Color Ramp
                Dim pColorRamp As ESRI.ArcGIS.Display.IColorRamp = Nothing '= New RasterStretchColorRampRenderer()
                Dim pStyleItem As ESRI.ArcGIS.Display.IStyleGalleryItem = GetESRIStyleColorRamp(pColorRamp, sColorRampName)

                Dim pRenderColorRamp As IRasterRendererColorRamp = CType(rasterRenderer, IRasterRendererColorRamp)
                pRenderColorRamp.ColorScheme = pStyleItem.Name

                'Set Legend Items
                Dim iRound As Integer = GetMagnitude(gRaster.Maximum)
                stretchRenderer.LabelHigh = Math.Round(gRaster.Maximum, Math.Abs(iRound)).ToString()
                stretchRenderer.LabelLow = Math.Round(gRaster.Minimum, Math.Abs(iRound)).ToString()

                'Invert color ramp if flagged to do so
                If bInvert Then
                    Dim pStretch As IRasterStretch2 = CType(stretchRenderer, IRasterStretch2)
                    pStretch.Invert = True
                End If

                rasterRenderer.Update()
                Return rasterRenderer

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine(ex.Message)
                Return Nothing
            End Try

        End Function

        Public Function CreateESRIDefinedContinuousRenderer(ByVal gRaster As GISDataStructures.Raster,
                                                     ByVal iClassCount As Integer,
                                                     ByVal sColorRampName As String,
                                                     ByVal rasterCollectionMin As Double,
                                                     ByVal rasterCollectionMax As Double,
                                                     ByVal rasterCollectionMean As Double,
                                                     ByVal rasterCollectionStDev As Double,
                                                     Optional ByVal bInvert As Boolean = False) As IRasterRenderer

            'This method is in transition mode, it needs to be reworked to symbolize all rasters in GCD
            'To do this alogrithmic color ramps, MultiPart ColorRamps from ESRI, and pre-programed color ramps need to be supported 

            Try
                'Create continous render aka ColorMapRenderer
                Dim colorMapRenderer As RasterColormapRenderer = New RasterStretchColorRampRenderer()
                Dim rasterRenderer As IRasterRenderer = CType(colorMapRenderer, IRasterRenderer)

                'Set up the renderer properties.
                Dim rasterDataset As ESRI.ArcGIS.Geodatabase.IRasterDataset = gRaster.RasterDataset
                Dim raster As ESRI.ArcGIS.Geodatabase.IRaster = rasterDataset.CreateDefaultRaster()
                rasterRenderer.Raster = raster

                'Create Color Ramp
                Dim pColorRamp As ESRI.ArcGIS.Display.IColorRamp = Nothing '= New RasterStretchColorRampRenderer()
                Dim pStyleItem As ESRI.ArcGIS.Display.IStyleGalleryItem = GetESRIStyleColorRamp(pColorRamp, sColorRampName)

                Dim pRenderColorRamp As IRasterRendererColorRamp = CType(rasterRenderer, IRasterRendererColorRamp)
                pRenderColorRamp.ColorScheme = pStyleItem.Name

                'Set Legend Items
                Dim pStretchInfo As IRasterStretchMinMax = CType(colorMapRenderer, IRasterStretchMinMax)
                pStretchInfo.CustomStretchMin = 0
                Dim iRound As Integer = GetMagnitude(gRaster.Maximum)
                pStretchInfo.CustomStretchMax = Math.Round(rasterCollectionMax, 2).ToString("#,###.00")
                pStretchInfo.UseCustomStretchMinMax = True

                Dim pStretch As IRasterStretch2 = CType(colorMapRenderer, IRasterStretch2)
                pStretch.StretchType = esriRasterStretchTypesEnum.esriRasterStretch_MinimumMaximum
                Debug.Print("Stretch type: {0}", pStretch.StretchType.ToString())
                ''pStretch.StandardDeviationsParam = rasterCollectionStDev
                'Dim stretchIVariant As ESRI.ArcGIS.esriSystem.IArray = New ESRI.ArcGIS.esriSystem.Array
                'pStretch.StretchStatsType = esriRasterStretchStatsTypeEnum.esriRasterStretchStats_GlobalStats
                'pStretch.StretchStats.Add(rasterCollectionMin)
                'pStretch.StretchStats.Add(rasterCollectionMax)
                'pStretch.StretchStats.Add(rasterCollectionMean)
                'pStretch.StretchStats.Add(rasterCollectionStDev)
                'pStretch.StretchStats = stretchIVariant
                'pStretch.StretchType.GetNames()

                Dim pLegend As IRasterStretchAdvancedLabels = CType(colorMapRenderer, IRasterStretchAdvancedLabels)
                pLegend.NumLabels = 3
                'pLegend.LabelText(0) = "This test"
                'pLegend.LabelValue(0) = 0
                'pLegend.LabelText(1) = "Max Text"
                'pLegend.LabelValue(1) = rasterCollectionMax


                'Invert color ramp if flagged to do so
                If bInvert Then
                    'Dim pStretch As IRasterStretch2 = CType(colorMapRenderer, IRasterStretch2)
                    pStretch.Invert = True
                End If
                rasterRenderer.Update()
                Return rasterRenderer

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine(ex.Message)
                Return Nothing
            End Try

        End Function

        Public Function CreateContinuousRenderer(ByVal gRaster As GISDataStructures.Raster,
                                                 ByVal pColorRamp As ESRI.ArcGIS.Display.IColorRamp,
                                                 Optional ByVal bInvert As Boolean = False) As IRasterRenderer

            'This method is in transition mode, it needs to be reworked to symbolize all rasters in GCD
            'To do this alogrithmic color ramps, MultiPart ColorRamps from ESRI, and pre-programed color ramps need to be supported 

            Try
                'Create continous render aka ColorMapRenderer
                Dim stretchRenderer As RasterStretchColorRampRenderer = New RasterStretchColorRampRenderer()
                Dim rasterRenderer As IRasterRenderer = CType(stretchRenderer, IRasterRenderer)

                'Set up the renderer properties.
                Dim rasterDataset As ESRI.ArcGIS.Geodatabase.IRasterDataset = gRaster.RasterDataset
                Dim raster As ESRI.ArcGIS.Geodatabase.IRaster = rasterDataset.CreateDefaultRaster()
                rasterRenderer.Raster = raster

                Dim pRenderColorRamp As IRasterRendererColorRamp = CType(rasterRenderer, IRasterRendererColorRamp)
                pRenderColorRamp.ColorRamp = pColorRamp

                'Set Legend Items
                Dim iRound As Integer = GetMagnitude(gRaster.Maximum)
                stretchRenderer.LabelHigh = Math.Round(gRaster.Maximum, Math.Abs(iRound)).ToString()
                stretchRenderer.LabelLow = Math.Round(gRaster.Minimum, Math.Abs(iRound)).ToString()

                'Invert color ramp if flagged to do so
                If bInvert Then
                    Dim pStretch As IRasterStretch2 = CType(stretchRenderer, IRasterStretch2)
                    pStretch.Invert = True
                End If

                rasterRenderer.Update()
                Return rasterRenderer

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine(ex.Message)
                Return Nothing
            End Try

        End Function

        Public Function CreateClassifyRenderer(ByVal gRaster As GISDataStructures.Raster, ByVal iClassCount As Integer, ByVal sColorRampName As String, Optional ByVal bInvert As Boolean = False) As IRasterRenderer

            Try
                'Create the classify renderer.
                Dim classifyRenderer As IRasterClassifyColorRampRenderer = New RasterClassifyColorRampRendererClass()
                Dim rasterRenderer As IRasterRenderer = CType(classifyRenderer, IRasterRenderer)
                Dim fillSymbol As ESRI.ArcGIS.Display.IFillSymbol = New ESRI.ArcGIS.Display.SimpleFillSymbolClass()

                'Set up the renderer properties.
                Dim rasterDataset As ESRI.ArcGIS.Geodatabase.IRasterDataset = gRaster.RasterDataset
                Dim raster As ESRI.ArcGIS.Geodatabase.IRaster = rasterDataset.CreateDefaultRaster()
                rasterRenderer.Raster = raster

                'Create Color Ramp
                Dim pColorRamp As ESRI.ArcGIS.Display.IColorRamp = Nothing '= New RasterStretchColorRampRenderer()
                Dim pStyleItem As ESRI.ArcGIS.Display.IStyleGalleryItem = GetESRIStyleColorRamp(pColorRamp, sColorRampName)
                classifyRenderer.ClassCount = iClassCount
                rasterRenderer.Update()
                CreateClassBreaks(gRaster.Maximum, iClassCount, classifyRenderer)

                pColorRamp.Size = iClassCount
                pColorRamp.CreateRamp(True)

                ''Create the symbol for the classes. Load a list with colors to be reversed later for proper red to blue representation
                Dim lColors As List(Of ESRI.ArcGIS.Display.IColor) = New List(Of ESRI.ArcGIS.Display.IColor)
                For i As Integer = 0 To classifyRenderer.ClassCount - 1 Step i + 1
                    lColors.Add(pColorRamp.Color(i))
                Next

                'Load colors into renderer
                If bInvert Then
                    lColors.Reverse()
                End If

                For i As Integer = 0 To classifyRenderer.ClassCount - 1 Step i + 1
                    fillSymbol.Color = lColors.Item(i)
                    classifyRenderer.Symbol(i) = fillSymbol
                Next

                Return rasterRenderer

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine(ex.Message)
                Return Nothing
            End Try

        End Function

        Public Function CreateClassifyRenderer(ByVal gRaster As GISDataStructures.Raster, ByVal iClassCount As Integer, ByVal sColorRampName As String, ByVal dMax As Double, Optional ByVal bInvert As Boolean = False) As IRasterRenderer

            Try
                'Create the classify renderer.
                Dim classifyRenderer As IRasterClassifyColorRampRenderer = New RasterClassifyColorRampRendererClass()
                Dim rasterRenderer As IRasterRenderer = CType(classifyRenderer, IRasterRenderer)
                Dim fillSymbol As ESRI.ArcGIS.Display.IFillSymbol = New ESRI.ArcGIS.Display.SimpleFillSymbolClass()

                'Set up the renderer properties.
                Dim rasterDataset As ESRI.ArcGIS.Geodatabase.IRasterDataset = gRaster.RasterDataset
                Dim raster As ESRI.ArcGIS.Geodatabase.IRaster = rasterDataset.CreateDefaultRaster()
                rasterRenderer.Raster = raster

                'Create Color Ramp
                Dim pColorRamp As ESRI.ArcGIS.Display.IColorRamp = Nothing '= New RasterStretchColorRampRenderer()
                Dim pStyleItem As ESRI.ArcGIS.Display.IStyleGalleryItem = GetESRIStyleColorRamp(pColorRamp, sColorRampName)
                classifyRenderer.ClassCount = iClassCount
                rasterRenderer.Update()
                CreateClassBreaks(dMax, iClassCount, classifyRenderer)

                pColorRamp.Size = iClassCount
                pColorRamp.CreateRamp(True)

                ''Create the symbol for the classes. Load a list with colors to be reversed later for proper red to blue representation
                Dim lColors As List(Of ESRI.ArcGIS.Display.IColor) = New List(Of ESRI.ArcGIS.Display.IColor)
                For i As Integer = 0 To classifyRenderer.ClassCount - 1 Step i + 1
                    lColors.Add(pColorRamp.Color(i))
                Next

                'Load colors into renderer
                If bInvert Then
                    lColors.Reverse()
                End If

                For i As Integer = 0 To classifyRenderer.ClassCount - 1 Step i + 1
                    fillSymbol.Color = lColors.Item(i)
                    classifyRenderer.Symbol(i) = fillSymbol
                Next

                Return rasterRenderer

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine(ex.Message)
                Return Nothing
            End Try

        End Function

        Public Function CreateDoDClassifyRenderer(ByVal gRaster As GISDataStructures.Raster, ByVal iClassCount As Integer) As IRasterRenderer

            'This method is in transition mode, it needs to be reworked to symbolize all rasters in GCD
            'To do this alogrithmic color ramps, MultiPart ColorRamps from ESRI, and pre-programed color ramps need to be supported 

            Try
                'Create the classify renderer.
                Dim classifyRenderer As IRasterClassifyColorRampRenderer = New RasterClassifyColorRampRendererClass()
                Dim rasterRenderer As IRasterRenderer = CType(classifyRenderer, IRasterRenderer)
                Dim fillSymbol As ESRI.ArcGIS.Display.IFillSymbol = New ESRI.ArcGIS.Display.SimpleFillSymbolClass()

                'Set up the renderer properties.
                Dim rasterDataset As ESRI.ArcGIS.Geodatabase.IRasterDataset = gRaster.RasterDataset
                Dim raster As ESRI.ArcGIS.Geodatabase.IRaster = rasterDataset.CreateDefaultRaster()
                rasterRenderer.Raster = raster

                If (gRaster.Minimum = Double.MinValue And gRaster.Maximum = Double.MaxValue) Or (gRaster.Minimum = Double.MaxValue And gRaster.Maximum = Double.MinValue) _
                    Or (gRaster.Minimum = Single.MinValue And gRaster.Maximum = Single.MaxValue) Or (gRaster.Minimum = Single.MaxValue And gRaster.Maximum = Single.MinValue) Then
                    classifyRenderer.ClassCount = 1
                    Dim rgbColor As ESRI.ArcGIS.Display.IRgbColor = CreateRGBColor(255, 255, 255)
                    rgbColor.Transparency = 0
                    fillSymbol.Color = rgbColor
                    classifyRenderer.Symbol(0) = fillSymbol
                    classifyRenderer.Label(0) = "No Data (no change detected)"
                    Return rasterRenderer
                End If

                classifyRenderer.ClassCount = iClassCount
                rasterRenderer.Update()

                'pDoDColorRamp.Size = iClassCount
                'pDoDColorRamp.CreateRamp(True)

                'Define the class breaks here
                CreateDoDClassBreaks(gRaster.Maximum, gRaster.Minimum, iClassCount, classifyRenderer)

                ''Define color enumeration
                'Dim eColors As ESRI.ArcGIS.Display.IEnumColors = pDoDColorRamp.Colors
                ''Dim pColorRampProperties As IColorRampSymbol = CType(pDoDColorRamp, IColorRampSymbol)
                ''If pDoDColorRamp.Size > 0 Then

                'Dim bCreateColorRamp As Boolean = True
                'pDoDColorRamp.CreateRamp(bCreateColorRamp)

                ''Create the symbol for the classes. Load a list with colors to be reversed later for proper red to blue representation
                'Dim lColors As List(Of ESRI.ArcGIS.Display.IColor) = New List(Of ESRI.ArcGIS.Display.IColor)
                'For i As Integer = 0 To classifyRenderer.ClassCount - 1 Step i + 1
                '    lColors.Add(pDoDColorRamp.Color(i))
                'Next

                'Load colors into renderer
                'lColors.Reverse()
                Dim lColors As List(Of ESRI.ArcGIS.Display.IColor) = CreateDoDColorRamp()
                For i As Integer = 0 To classifyRenderer.ClassCount - 1 Step i + 1
                    fillSymbol.Color = lColors(i)
                    classifyRenderer.Symbol(i) = fillSymbol
                Next

                Return rasterRenderer

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine(ex.Message)
                Return Nothing
            End Try
        End Function

        Public Function CreateProbabilityClassifyRenderer(ByVal gRaster As GISDataStructures.Raster, ByVal iClassCount As Integer) As IRasterRenderer

            Try
                'Create the classify renderer.
                Dim classifyRenderer As IRasterClassifyColorRampRenderer = New RasterClassifyColorRampRendererClass()
                Dim rasterRenderer As IRasterRenderer = CType(classifyRenderer, IRasterRenderer)
                Dim fillSymbol As ESRI.ArcGIS.Display.IFillSymbol = New ESRI.ArcGIS.Display.SimpleFillSymbolClass()

                'Set up the renderer properties.
                Dim rasterDataset As ESRI.ArcGIS.Geodatabase.IRasterDataset = gRaster.RasterDataset
                Dim raster As ESRI.ArcGIS.Geodatabase.IRaster = rasterDataset.CreateDefaultRaster()
                rasterRenderer.Raster = raster

                'Create Color Ramp

                classifyRenderer.ClassCount = iClassCount
                rasterRenderer.Update()
                CreateDoDClassBreaks(gRaster.Maximum, gRaster.Minimum, iClassCount, classifyRenderer)

                ''Create the symbol for the classes. Load a list with colors to be reversed later for proper red to blue representation
                Dim lColors As List(Of ESRI.ArcGIS.Display.IColor) = CreateDoDColorRamp()
                For i As Integer = 0 To classifyRenderer.ClassCount - 1 Step i + 1
                    fillSymbol.Color = lColors(i)
                    classifyRenderer.Symbol(i) = fillSymbol
                Next

                Return rasterRenderer

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine(ex.Message)
                Return Nothing
            End Try

        End Function

        Private Sub CreateDoDClassBreaks(ByVal dMax As Double, ByVal dMin As Double, ByVal iClassCount As Integer, ByRef pRasterClassify As IRasterClassifyColorRampRenderer)

            Dim fMax As Double = Math.Max(Math.Abs(dMax), Math.Abs(dMin))
            Dim fRange As Double = 2 * fMax
            Dim interval As Double = fRange / iClassCount

            Dim iRound As Integer = GetMagnitude(fRange)
            Debug.Print("Interval before Rounding: {0}", interval)
            interval = Math.Round(interval * Math.Abs(iRound), Math.Abs(iRound) + 2) / Math.Abs(iRound)

            Dim sFormat As String = "#,#0"
            If iRound < 0 Then
                iRound = Math.Abs(iRound) + 2
                For i As Integer = 0 To iRound
                    If i = 0 Then
                        sFormat &= "."
                    End If
                    sFormat &= "0"
                Next

            Else
                sFormat = "#,#0.00"
            End If

            Debug.Print("Class count: " & pRasterClassify.ClassCount)
            Debug.Print("Interval: {0}", interval)
            Debug.Print("Max: {0}", fMax)

            For i As Integer = 0 To iClassCount

                Dim fBreak As Double = Math.Round(-1 * fMax + (interval * i), iRound)
                Debug.Print("Break " & i.ToString & " = " & fBreak)
                pRasterClassify.Break(i) = fBreak

                If i = 0 Then

                    pRasterClassify.Break(i) = fBreak
                    fBreak = Math.Round(-1 * fMax + (interval * 1), iRound)
                    pRasterClassify.Label(i) = "< " & fBreak.ToString(sFormat)

                ElseIf i < iClassCount - 1 Then

                    If i = 10 Then
                        pRasterClassify.Break(i) = 0
                        pRasterClassify.Label(i) = (0.0).ToString(sFormat) & " to " & Math.Round(fBreak + interval, iRound).ToString(sFormat)
                        Continue For
                    End If

                    pRasterClassify.Label(i) = (fBreak).ToString(sFormat) & " to " & Math.Round(fBreak + interval, iRound).ToString(sFormat)

                ElseIf i = iClassCount - 1 Then
                    pRasterClassify.Label(i) = "> " & fBreak.ToString(sFormat)

                End If
            Next

        End Sub

        Private Function CreateDoDColorRamp() As List(Of ESRI.ArcGIS.Display.IColor)

            Dim lColors As List(Of ESRI.ArcGIS.Display.IColor) = New List(Of ESRI.ArcGIS.Display.IColor)
            Dim rgbColor As ESRI.ArcGIS.Display.IRgbColor = CreateRGBColor(230, 0, 0) 'reds
            lColors.Add(rgbColor)
            rgbColor = CreateRGBColor(235, 45, 23)
            lColors.Add(rgbColor)
            rgbColor = CreateRGBColor(240, 67, 41)
            lColors.Add(rgbColor)
            rgbColor = CreateRGBColor(242, 88, 61)
            lColors.Add(rgbColor)
            rgbColor = CreateRGBColor(245, 108, 81)
            lColors.Add(rgbColor)
            rgbColor = CreateRGBColor(245, 131, 105)
            lColors.Add(rgbColor)
            rgbColor = CreateRGBColor(245, 151, 130)
            lColors.Add(rgbColor)
            rgbColor = CreateRGBColor(242, 171, 155)
            lColors.Add(rgbColor)
            rgbColor = CreateRGBColor(237, 190, 180)
            lColors.Add(rgbColor)
            rgbColor = CreateRGBColor(230, 208, 207)
            lColors.Add(rgbColor)
            rgbColor = CreateRGBColor(218, 218, 224) 'blues
            lColors.Add(rgbColor)
            rgbColor = CreateRGBColor(197, 201, 219)
            lColors.Add(rgbColor)
            rgbColor = CreateRGBColor(176, 183, 214)
            lColors.Add(rgbColor)
            rgbColor = CreateRGBColor(155, 166, 207)
            lColors.Add(rgbColor)
            rgbColor = CreateRGBColor(135, 150, 201)
            lColors.Add(rgbColor)
            rgbColor = CreateRGBColor(110, 131, 194)
            lColors.Add(rgbColor)
            rgbColor = CreateRGBColor(92, 118, 189)
            lColors.Add(rgbColor)
            rgbColor = CreateRGBColor(72, 105, 184)
            lColors.Add(rgbColor)
            rgbColor = CreateRGBColor(49, 91, 176)
            lColors.Add(rgbColor)
            rgbColor = CreateRGBColor(2, 77, 168)
            lColors.Add(rgbColor)
            Return lColors

        End Function

        Private Function GetFormattedRange(ByVal fRange As Double)

            Dim iRound As Integer = GetMagnitude(fRange)
            fRange = Math.Round(fRange * 2, Math.Abs(iRound) + 1, MidpointRounding.AwayFromZero) / 2
            Return fRange

        End Function

        Private Sub CreateClassBreaks(ByVal fMax As Double, ByVal iClassCount As Integer, ByRef pRasterClassify As IRasterClassifyColorRampRenderer)

            fMax = GetFormattedRange(fMax)
            Dim interval = fMax / iClassCount
            interval = GetFormattedRange(interval)

            Dim iRound As Integer = GetMagnitude(fMax)
            Debug.Print("Interval before Rounding: {0}", interval)
            interval = Math.Round(interval * Math.Abs(iRound), Math.Abs(iRound) + 1) / Math.Abs(iRound)

            Dim sFormat As String = "#,#0"
            If iRound < 0 Then
                iRound = Math.Abs(iRound) + 1
                For i As Integer = 0 To iRound
                    If i = 0 Then
                        sFormat &= "."
                    End If
                    sFormat &= "0"
                Next

            Else
                sFormat = "#,#0.00"
            End If

            Debug.Print("Class count: " & pRasterClassify.ClassCount)
            Debug.Print("Interval: {0}", interval)
            Debug.Print("Max: {0}", fMax)

            If pRasterClassify.ClassCount <> iClassCount Then
                pRasterClassify.ClassCount = iClassCount
            End If

            For i As Integer = 0 To iClassCount

                Dim fBreak As Double = Math.Round(interval * i, iRound)
                Debug.Print("Break " & i.ToString & " = " & fBreak)
                pRasterClassify.Break(i) = fBreak

                If i < iClassCount - 1 Then

                    pRasterClassify.Label(i) = (fBreak).ToString(sFormat) & " to " & Math.Round(fBreak + interval, iRound).ToString(sFormat)
                    Debug.Print(pRasterClassify.Label(i))
                ElseIf i = iClassCount - 1 Then
                    pRasterClassify.Label(i) = "> " & fBreak.ToString(sFormat)
                    Debug.Print(pRasterClassify.Label(i))
                End If
            Next

        End Sub

        Public Function CreateSlopeDegreesColorRamp(ByVal gRaster As GISDataStructures.Raster) As IRasterRenderer


            Try
                'Create the classify renderer.
                Dim classifyRenderer As IRasterClassifyColorRampRenderer = New RasterClassifyColorRampRendererClass()
                Dim rasterRenderer As IRasterRenderer = CType(classifyRenderer, IRasterRenderer)

                'Set up the renderer properties.
                Dim rasterDataset As ESRI.ArcGIS.Geodatabase.IRasterDataset = gRaster.RasterDataset
                Dim raster As ESRI.ArcGIS.Geodatabase.IRaster = rasterDataset.CreateDefaultRaster()
                rasterRenderer.Raster = raster
                classifyRenderer.ClassCount = 10
                rasterRenderer.Update()

                'Define the class breaks here
                classifyRenderer.Break(0) = gRaster.Minimum
                classifyRenderer.Label(0) = "0 to 2"
                classifyRenderer.Break(1) = 2
                classifyRenderer.Label(1) = "2 to 5"
                classifyRenderer.Break(2) = 5
                classifyRenderer.Label(2) = "5 to 10"
                classifyRenderer.Break(3) = 10
                classifyRenderer.Label(3) = "10 to 15"
                classifyRenderer.Break(4) = 15
                classifyRenderer.Label(4) = "15 to 25"
                classifyRenderer.Break(5) = 25
                classifyRenderer.Label(5) = "25 to 35"
                classifyRenderer.Break(6) = 35
                classifyRenderer.Label(6) = "35 to 45"
                classifyRenderer.Break(7) = 45
                classifyRenderer.Label(7) = "45 to 60"
                classifyRenderer.Break(8) = 60
                classifyRenderer.Label(8) = "60 to 80"
                classifyRenderer.Break(9) = 80
                classifyRenderer.Label(9) = "80 to 90"
                classifyRenderer.Break(10) = 90

                Dim lColors As List(Of ESRI.ArcGIS.Display.RgbColor) = CreateSlopeColorRamp()

                Dim fillSymbol As ESRI.ArcGIS.Display.IFillSymbol = New ESRI.ArcGIS.Display.SimpleFillSymbolClass()
                For i As Integer = 0 To classifyRenderer.ClassCount - 1 Step i + 1
                    Debug.WriteLine(String.Format("Red: {0}, Green: {1}, Blue: {2}", lColors(i).Red, lColors(i).Green, lColors(i).Blue))
                    fillSymbol.Color = lColors(i)
                    classifyRenderer.Symbol(i) = fillSymbol
                Next

                Return rasterRenderer
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine(ex.Message)
                Return Nothing
            End Try

        End Function

        Public Function CreateSlopePrecentRiseColorRamp(ByVal gRaster As GISDataStructures.Raster) As IRasterRenderer

            Try
                'Create the classify renderer.
                Dim classifyRenderer As IRasterClassifyColorRampRenderer = New RasterClassifyColorRampRendererClass()
                Dim rasterRenderer As IRasterRenderer = CType(classifyRenderer, IRasterRenderer)

                'Set up the renderer properties.
                Dim rasterDataset As ESRI.ArcGIS.Geodatabase.IRasterDataset = gRaster.RasterDataset
                Dim raster As ESRI.ArcGIS.Geodatabase.IRaster = rasterDataset.CreateDefaultRaster()
                rasterRenderer.Raster = raster
                classifyRenderer.ClassCount = 10
                rasterRenderer.Update()

                'Define the class breaks here
                classifyRenderer.Break(0) = 0
                classifyRenderer.Label(0) = "0 to 3.5%"
                classifyRenderer.Break(1) = 3.5
                classifyRenderer.Label(1) = "3.5% to 8.75%"
                classifyRenderer.Break(2) = 8.75
                classifyRenderer.Label(2) = "8.75% to 15%"
                classifyRenderer.Break(3) = 15
                classifyRenderer.Label(3) = "15% to 25%"
                classifyRenderer.Break(4) = 25
                classifyRenderer.Label(4) = "25% to 45%"
                classifyRenderer.Break(5) = 45
                classifyRenderer.Label(5) = "45% to 70%"
                classifyRenderer.Break(6) = 70
                classifyRenderer.Label(6) = "70% to 100%"
                classifyRenderer.Break(7) = 100
                classifyRenderer.Label(7) = "100% to 175%"
                classifyRenderer.Break(8) = 175
                classifyRenderer.Label(8) = "175% to 565%"
                classifyRenderer.Break(9) = 565
                classifyRenderer.Label(9) = "> 565%"

                Dim lColors As List(Of ESRI.ArcGIS.Display.RgbColor) = CreateSlopeColorRamp()

                Dim fillSymbol As ESRI.ArcGIS.Display.IFillSymbol = New ESRI.ArcGIS.Display.SimpleFillSymbolClass()
                For i As Integer = 0 To classifyRenderer.ClassCount - 1 Step i + 1
                    Debug.WriteLine(String.Format("Red: {0}, Green: {1}, Blue: {2}", lColors(i).Red, lColors(i).Green, lColors(i).Blue))
                    fillSymbol.Color = lColors(i)
                    classifyRenderer.Symbol(i) = fillSymbol
                Next

                Return rasterRenderer
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine(ex.Message)
                Return Nothing
            End Try

        End Function

        Private Function CreateSlopeColorRamp() As List(Of ESRI.ArcGIS.Display.RgbColor)

            Dim lColors As List(Of ESRI.ArcGIS.Display.RgbColor) = New List(Of ESRI.ArcGIS.Display.RgbColor)
            lColors.Add(CreateRGBColor(255, 235, 176))
            lColors.Add(CreateRGBColor(255, 219, 135))
            lColors.Add(CreateRGBColor(255, 202, 97))
            lColors.Add(CreateRGBColor(255, 186, 59))
            lColors.Add(CreateRGBColor(255, 170, 0))
            lColors.Add(CreateRGBColor(255, 128, 0))
            lColors.Add(CreateRGBColor(255, 85, 0))
            lColors.Add(CreateRGBColor(255, 42, 0))
            lColors.Add(CreateRGBColor(161, 120, 120))
            lColors.Add(CreateRGBColor(130, 130, 130))
            Return lColors

        End Function

        Public Function CreateRoughnessColorRamp(ByVal gRaster As GISDataStructures.Raster) As IRasterRenderer


            Try
                'Create the classify renderer.
                Dim classifyRenderer As IRasterClassifyColorRampRenderer = New RasterClassifyColorRampRendererClass()
                Dim rasterRenderer As IRasterRenderer = CType(classifyRenderer, IRasterRenderer)

                'Set up the renderer properties.
                Dim rasterDataset As ESRI.ArcGIS.Geodatabase.IRasterDataset = gRaster.RasterDataset
                Dim raster As ESRI.ArcGIS.Geodatabase.IRaster = rasterDataset.CreateDefaultRaster()
                rasterRenderer.Raster = raster
                classifyRenderer.ClassCount = 10
                rasterRenderer.Update()

                'Dim pColorRamp As ESRI.ArcGIS.Display.IColorRamp = New ESRI.ArcGIS.Display.MultiPartColorRamp()
                'pColorRamp.Size = iClassCount
                'pColorRamp.CreateRamp(True)

                'Define the class breaks here
                classifyRenderer.Break(0) = 0
                classifyRenderer.Label(0) = "0 to 0.1"
                classifyRenderer.Break(1) = 0.1
                classifyRenderer.Label(1) = "0.1 to 0.25"
                classifyRenderer.Break(2) = 0.25
                classifyRenderer.Label(2) = "0.25 to 0.5"
                classifyRenderer.Break(3) = 0.5
                classifyRenderer.Label(3) = "0.5 to 0.75"
                classifyRenderer.Break(4) = 0.75
                classifyRenderer.Label(4) = "0.75 to 1.0"
                classifyRenderer.Break(5) = 1.0
                classifyRenderer.Label(5) = "1.0 to 1.5"
                classifyRenderer.Break(6) = 1.5
                classifyRenderer.Label(6) = "1.5 to 2.0"
                classifyRenderer.Break(7) = 2.0
                classifyRenderer.Label(7) = "2.0 to 3.0"
                classifyRenderer.Break(8) = 3.0
                classifyRenderer.Label(8) = "3.0 to 5.0"
                classifyRenderer.Break(9) = 5.0
                classifyRenderer.Label(9) = "> 5.0"

                Dim lColors As List(Of ESRI.ArcGIS.Display.RgbColor) = New List(Of ESRI.ArcGIS.Display.RgbColor)
                lColors.Add(CreateRGBColor(255, 255, 179))
                lColors.Add(CreateRGBColor(252, 241, 167))
                lColors.Add(CreateRGBColor(252, 230, 157))
                lColors.Add(CreateRGBColor(250, 218, 145))
                lColors.Add(CreateRGBColor(250, 208, 135))
                lColors.Add(CreateRGBColor(237, 191, 126))
                lColors.Add(CreateRGBColor(219, 167, 118))
                lColors.Add(CreateRGBColor(201, 147, 111))
                lColors.Add(CreateRGBColor(184, 127, 106))
                lColors.Add(CreateRGBColor(166, 101, 101))

                Dim fillSymbol As ESRI.ArcGIS.Display.IFillSymbol = New ESRI.ArcGIS.Display.SimpleFillSymbolClass()
                For i As Integer = 0 To classifyRenderer.ClassCount - 1 Step i + 1
                    Debug.WriteLine(String.Format("Red: {0}, Green: {1}, Blue: {2}", lColors(i).Red, lColors(i).Green, lColors(i).Blue))
                    fillSymbol.Color = lColors(i)
                    classifyRenderer.Symbol(i) = fillSymbol
                Next

                Return rasterRenderer
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine(ex.Message)
                Return Nothing
            End Try

        End Function

        Public Function CreateGrainSizeStatisticColorRamp(ByVal gRaster As GISDataStructures.Raster, ByVal eUnits As NumberFormatting.LinearUnits) As IRasterRenderer


            Try
                'Create the classify renderer.
                Dim classifyRenderer As IRasterClassifyColorRampRenderer = New RasterClassifyColorRampRendererClass()
                Dim rasterRenderer As IRasterRenderer = CType(classifyRenderer, IRasterRenderer)

                'Set up the renderer properties.
                Dim rasterDataset As ESRI.ArcGIS.Geodatabase.IRasterDataset = gRaster.RasterDataset
                Dim raster As ESRI.ArcGIS.Geodatabase.IRaster = rasterDataset.CreateDefaultRaster()
                rasterRenderer.Raster = raster
                classifyRenderer.ClassCount = 5
                rasterRenderer.Update()
                'Define the class breaks here
                classifyRenderer.Break(0) = 0
                classifyRenderer.Label(0) = "Fines, Sand (0 to 2 mm)"
                classifyRenderer.Break(1) = NumberFormatting.Convert(NumberFormatting.LinearUnits.mm, eUnits, 2)
                classifyRenderer.Label(1) = "Fine Gravel (2 mm to 16 mm)"
                classifyRenderer.Break(2) = NumberFormatting.Convert(NumberFormatting.LinearUnits.mm, eUnits, 16)
                classifyRenderer.Label(2) = "Coarse Gravel (16 mm to 64 mm)"
                classifyRenderer.Break(3) = NumberFormatting.Convert(NumberFormatting.LinearUnits.mm, eUnits, 64)
                classifyRenderer.Label(3) = "Cobbles (64 mm to 256 mm)"
                classifyRenderer.Break(4) = NumberFormatting.Convert(NumberFormatting.LinearUnits.mm, eUnits, 256)
                classifyRenderer.Label(4) = "Boulders (> 256 mm)"

                Dim lColors As List(Of ESRI.ArcGIS.Display.RgbColor) = New List(Of ESRI.ArcGIS.Display.RgbColor)
                lColors.Add(CreateRGBColor(194, 82, 60))
                lColors.Add(CreateRGBColor(240, 180, 17))
                lColors.Add(CreateRGBColor(123, 237, 0))
                lColors.Add(CreateRGBColor(27, 168, 124))
                lColors.Add(CreateRGBColor(11, 44, 122))

                Dim fillSymbol As ESRI.ArcGIS.Display.IFillSymbol = New ESRI.ArcGIS.Display.SimpleFillSymbolClass()
                For i As Integer = 0 To classifyRenderer.ClassCount - 1 Step i + 1
                    Debug.WriteLine(String.Format("Red: {0}, Green: {1}, Blue: {2}", lColors(i).Red, lColors(i).Green, lColors(i).Blue))
                    fillSymbol.Color = lColors(i)
                    classifyRenderer.Symbol(i) = fillSymbol
                Next

                Return rasterRenderer
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine(ex.Message)
                Return Nothing
            End Try

        End Function

        Private Function CreateRGBColor(ByVal iRed As UInt16, ByVal iGreen As UInt16, ByVal iBlue As UInt16) As ESRI.ArcGIS.Display.RgbColor
            Dim RGBColor As ESRI.ArcGIS.Display.RgbColor = New ESRI.ArcGIS.Display.RgbColor()
            RGBColor.Red = iRed
            RGBColor.Green = iGreen
            RGBColor.Blue = iBlue
            Return RGBColor
        End Function

        Private Function GetESRIStyleColorRamp(ByRef pColorRamp As ESRI.ArcGIS.Display.IColorRamp, ByVal sColorRampName As String) As ESRI.ArcGIS.Display.IStyleGalleryItem

            ' Getting color ramp from ESRI Sytle Gallery, get gallery from esri install folder, find color ramp file, load into enumeration
            Dim pStyleGallery As ESRI.ArcGIS.Display.IStyleGallery = New ESRI.ArcGIS.Framework.StyleGallery()
            Dim pStyleStorage As ESRI.ArcGIS.Display.IStyleGalleryStorage
            pStyleStorage = TryCast(pStyleGallery, ESRI.ArcGIS.Display.IStyleGalleryStorage)
            Dim pStylePath As String = pStyleStorage.DefaultStylePath + "ESRI.style"
            pStyleStorage.AddFile(pStylePath)
            Dim eESRIRampCategories As ESRI.ArcGIS.esriSystem.IEnumBSTR = pStyleGallery.Categories("Color Ramps")

            Dim sESRIRampCategoryName As String = eESRIRampCategories.Next()
            Dim pStyleItem As ESRI.ArcGIS.Display.IStyleGalleryItem

            Dim bFound As Boolean = False
            Do Until sESRIRampCategoryName Is Nothing
                'Debug.Print("Color Ramp Category: {0}", sESRIRampCategoryName)
                Dim eESRIColorRamps As ESRI.ArcGIS.Display.IEnumStyleGalleryItem = pStyleGallery.Items("Color Ramps", pStylePath, sESRIRampCategoryName)
                pStyleItem = eESRIColorRamps.Next()

                'Constant variables/settings need to be created to represent color ramps we use for pStyleItem.Name

                Do Until pStyleItem Is Nothing
                    'Debug.Print(String.Format("Style Name: {0} StyleID: {1}", pStyleItem.Name, pStyleItem.ID))
                    If String.Compare(pStyleItem.Name, sColorRampName) = 0 Then
                        pColorRamp = pStyleItem.Item
                        Return pStyleItem
                    End If
                    pStyleItem = eESRIColorRamps.Next
                Loop
                sESRIRampCategoryName = eESRIRampCategories.Next()
            Loop

            Throw New Exception("The name of the color ramp provided does not exist.")

        End Function

        Private Function GetMagnitude(ByVal range As Double, Optional ByVal iMagnitudeMinimum As Integer = -4, Optional ByVal iMagnitudeMaximum As Integer = 4) As Integer

            For i As Integer = iMagnitudeMinimum To iMagnitudeMaximum Step 1
                Dim tempVal As Double = Nothing
                tempVal = range / Math.Pow(10, i)
                'Debug.Print("Range: {0} TempVal: {1}", range, tempVal)
                If Math.Floor(Math.Abs(tempVal)) >= 1 And Math.Floor(Math.Abs(tempVal)) < 10 Then
                    If i = 0 Then
                        Return 1
                    Else
                        Return i
                    End If
                End If
            Next
            Return 1
        End Function

    End Module

End Namespace
