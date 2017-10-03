Imports System.Web.UI

Namespace GISCode.GCD.Report

    Public MustInherit Class Report

        Private m_fCSSFilePath As IO.FileInfo
        Private m_dReportResourceFolder As IO.DirectoryInfo
        Private m_MapEngine As GISCode.MapImages.MapImageEngine
        'Private m_ArcMapManager As GCDArcMapManager

        Protected ReadOnly Property CSSFilePath As IO.FileInfo
            Get
                Return m_fCSSFilePath
            End Get
        End Property

        'Protected ReadOnly Property ArcMapManager As GCD.GCDArcMapManager
        '    Get
        '        Return m_ArcMapManager
        '    End Get

        'End Property

        Protected ReadOnly Property MapEngine As GISCode.MapImages.MapImageEngine
            Get
                Return m_MapEngine
            End Get

        End Property
        Public Sub New(ByVal pArcMap As ESRI.ArcGIS.Framework.IApplication, ByVal fCSSFilePath As IO.FileInfo, ByVal dReportResourceFolder As IO.DirectoryInfo)

            'If Not fCSSFilePath.Exists Then
            '    Dim ex As New Exception("The cascading style sheet file is missing.")
            '    ex.Data("CSS File Path") = fCSSFilePath
            '    Throw ex
            'End If
            m_fCSSFilePath = fCSSFilePath
            m_dReportResourceFolder = dReportResourceFolder

            'Create mxd to work with
            Dim timeNow As DateTime = DateTime.Now
            Dim format As String = "mmssfff"
            Dim sMXDPath As String = IO.Path.Combine(Environ("TEMP"), "mxd_" & timeNow.ToString(format) & ".mxd")

            m_MapEngine = New GISCode.MapImages.MapImageEngine(sMXDPath)

            'm_ArcMapManager = New GCDArcMapManager(pArcMap, 45.0, m_MapEngine.MapDocument)

        End Sub

        Public MustOverride Function GenerateReport(rProject As ProjectDS.ProjectRow) As IO.StringWriter

        ''' <summary>
        ''' Inject the contents of a CSS file into the report stream
        ''' </summary>
        ''' <param name="ht"></param>
        ''' <remarks></remarks>
        Protected Sub InjectCSS(ByRef ht As HtmlTextWriter)

            ht.AddAttribute(HtmlTextWriterAttribute.Type, "text/css")
            ht.RenderBeginTag(HtmlTextWriterTag.Style)
            InjectFile(ht, m_fCSSFilePath.FullName)
            ht.RenderEndTag() ' style

        End Sub

        Protected Sub InjectFile(ByRef ht As HtmlTextWriter, sFilePath As String)

            If IO.File.Exists(sFilePath) Then
                Dim tr As IO.TextReader = New IO.StreamReader(sFilePath)
                Dim s As String = tr.ReadToEnd
                If Not String.IsNullOrEmpty(s) Then
                    ' ht.RenderBeginTag(HtmlTextWriterTag.Style)
                    ht.Write(s)
                    'ht.RenderEndTag() ' style
                End If
                tr.Close()
            Else
                ' Throw error?
            End If

        End Sub

        Protected Sub InjectImage(ByRef ht As HtmlTextWriter, ByVal sFileName As String, ByVal sCSSClass As String, Optional ByVal sHRef As String = "", Optional ByVal nWidth As Integer = 0, Optional ByVal nHeight As Integer = 0, Optional ByVal alt As String = Nothing)

            Dim sFilePath As String = sFileName
            If Not sFilePath.ToLower.StartsWith("http") Then
                If Not IO.File.Exists(sFilePath) Then
                    sFilePath = IO.Path.Combine(m_dReportResourceFolder.FullName, "Images")
                    sFilePath = IO.Path.Combine(sFilePath, sFileName)
                End If

                If Not IO.File.Exists(sFilePath) Then
                    Exit Sub
                End If

                sFilePath = "file://" & sFilePath
            End If

            Dim sImageTag As String = String.Empty

            If Not String.IsNullOrEmpty(sHRef) Then
                sImageTag = "<a href=""" & sHRef & """>"
            End If

            sImageTag &= "<img"

            If Not String.IsNullOrEmpty(sCSSClass) Then
                sImageTag &= " class=""" & sCSSClass & """"
            End If

            If nWidth > 0 Then
                sImageTag &= " width=""" & nWidth.ToString & "px"""
            End If

            If nHeight > 0 Then
                sImageTag &= " height=""" & nHeight.ToString & "px"""
            End If

            sImageTag &= " src=""" & sFilePath & """"

            'Addition
            sImageTag &= " alt=""" & alt & """"

            'According to W3 HTML 4.01 Transitional remove "/"
            'sImageTag &= " />"
            sImageTag &= " >"

            If Not String.IsNullOrEmpty(sHRef) Then
                sImageTag &= "</a>"
            End If

            ht.Write(sImageTag)
        End Sub

        Protected Sub InjectRasterImage(ByVal ht As HtmlTextWriter, ByVal sFilePath As String) ', ByVal dLayers As Dictionary(Of Integer, GISCode.MapImages.MapLayer))

            m_MapEngine.CreateHighResJPEGFromActiveView(m_MapEngine.MapDocument.ActiveView, sFilePath)
            InjectImage(ht, sFilePath, "RasterImage", sFilePath)
            MapEngine.Map.ClearLayers()

        End Sub

        Protected Sub InjectRasterImage(ByVal ht As HtmlTextWriter, ByVal sFilePath As String, ByVal dLayers As Dictionary(Of Integer, MapImages.MapLayer)) ', ByVal dLayers As Dictionary(Of Integer, GISCode.MapImages.MapLayer))

            m_MapEngine.GenerateMap(dLayers, sFilePath, True, True, True)
            InjectImage(ht, sFilePath, "RasterImage", sFilePath)
            'm_MapEngine.Map.ClearLayers()

        End Sub

    End Class

End Namespace