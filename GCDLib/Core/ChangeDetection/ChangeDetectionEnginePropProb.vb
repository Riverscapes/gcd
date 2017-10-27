Namespace Core.ChangeDetection

    Public Class ChangeDetectionEnginePropProb
        Inherits ChangeDetectionEngineBase

        Private m_gOriginalNewError As GCDConsoleLib.Raster
        Private m_gOriginalOldError As GCDConsoleLib.Raster

        Private m_gAnalysisNewError As GCDConsoleLib.Raster
        Private m_gAnalysisOldError As GCDConsoleLib.Raster

#Region "Properties"

        Public ReadOnly Property OriginalNewError As GCDConsoleLib.Raster
            Get
                Return m_gOriginalNewError
            End Get
        End Property

        Public ReadOnly Property OriginalOldError As GCDConsoleLib.Raster
            Get
                Return m_gOriginalOldError
            End Get
        End Property

        Public ReadOnly Property AnalysisNewError As GCDConsoleLib.Raster
            Get
                Return m_gAnalysisNewError
            End Get
        End Property

        Public ReadOnly Property AnalysisOldError As GCDConsoleLib.Raster
            Get
                Return m_gAnalysisOldError
            End Get
        End Property

#End Region

        Public Sub New(ByVal sName As String, ByVal sFolder As String, ByVal gNewDEM As GCDConsoleLib.Raster, ByVal gOldDEM As GCDConsoleLib.Raster,
                       ByVal gNewError As GCDConsoleLib.Raster, ByVal gOldError As GCDConsoleLib.Raster,
                       ByVal gAOI As GCDConsoleLib.Vector, ByVal fChartHeight As Integer, ByVal fChartWidth As Integer)
            MyBase.New(sName, sFolder, gNewDEM, gOldDEM, gAOI, fChartHeight, fChartWidth)

            m_gOriginalNewError = gNewError
            m_gOriginalOldError = gOldError

        End Sub

        Protected Overrides Function ThresholdRawDoD(rawDoDPath As String, rawHistPath As String) As DoDResult

            Dim propErrorRaster As String = GeneratePropagatedErrorRaster()

            ' Threshold the raster
            Dim thrDoDPath As String = Project.ProjectManagerBase.OutputManager.GetDoDThresholdPath(Name, Folder.FullName)
            External.ThresholdDoDPropErr(rawDoDPath, propErrorRaster, thrDoDPath, Project.ProjectManagerBase.GCDNARCError.ErrorString)

            ' Generate the thresholded histograms
            Dim thrHistPath As String = Project.ProjectManagerBase.OutputManager.GetCsvThresholdPath(Name, Folder.FullName)
            External.CalculateAndWriteDoDHistogram(thrDoDPath, thrHistPath, Project.ProjectManagerBase.GCDNARCError.ErrorString)

            Return New DoDResultPropagated(rawDoDPath, rawHistPath, thrDoDPath, thrHistPath, propErrorRaster, LinearUnits)

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>Calculate the propograted error raster based on the two error surfaces. Then threshold the raw
        ''' DoD removing any cells that have a value less than the propogated error.</remarks>
        Protected Function GeneratePropagatedErrorRaster() As String

            Dim sPropagatedErrorPath As String = Project.ProjectManagerBase.OutputManager.GetPropagatedErrorPath(Name, Folder.FullName)
            Try
                If Not External.RasterManager.RootSumSquares(AnalysisNewError.filepath, AnalysisOldError.filepath, sPropagatedErrorPath, Project.ProjectManagerBase.GCDNARCError.ErrorString) = External.RasterManagerOutputCodes.PROCESS_OK Then
                    Throw New Exception(Project.ProjectManagerBase.GCDNARCError.ErrorString.ToString)
                End If
            Catch ex As Exception
                Dim ex2 As New Exception("Error generating propagated error raster.", ex)
                ex2.Data("DoD Name") = Name
                ex2.Data("DoD Folder") = Folder.FullName
                ex2.Data("New Error") = AnalysisNewError.filepath
                ex2.Data("Old Error") = AnalysisOldError.filepath
                ex2.Data("Propagated Error") = sPropagatedErrorPath
                Throw ex2
            End Try

            Return sPropagatedErrorPath

        End Function

        Protected Shadows Function GenerateAnalysisRasters() As GCDConsoleLib.ExtentRectangle

            Debug.Print("Generating analysis error rasters.")

            ' Clip the DEM rasters to either the AOI or each other as concurrent rasters
            Dim theAnalysisExtent As GCDConsoleLib.ExtentRectangle = MyBase.GenerateAnalysisRasters

            Dim sNewError As String = WorkspaceManager.GetTempRaster("NewError")
            Dim sOldError As String = WorkspaceManager.GetTempRaster("OldError")

            ' Now make the error rasters concurrent to the DEM rasters
            If m_gOriginalNewError.Extent.IsConcurrent(AnalysisNewDEM.Extent) Then
                ' Already concurrent. Use error in situ
                sNewError = m_gOriginalNewError.filepath
            Else
                ' Clip error raster to extent
                If Not External.RasterManager.Copy(m_gOriginalNewError.filepath, sNewError, AnalysisNewDEM.Extent.CellHeight, theAnalysisExtent.Left, theAnalysisExtent.Top,
                                          AnalysisNewDEM.Extent.rows, AnalysisNewDEM.Extent.cols, Project.ProjectManagerBase.GCDNARCError.ErrorString) = External.RasterManagerOutputCodes.PROCESS_OK Then
                    Throw New Exception(Project.ProjectManagerBase.GCDNARCError.ErrorString.ToString)
                End If
                'GP.SpatialAnalyst.Raster_Calculator(Chr(34) & m_gOriginalNewError.FullPath & Chr(34), sNewError, theAnalysisExtent.Rectangle, m_gOriginalNewError.CellSize)
            End If

            If m_gOriginalOldError.Extent.IsConcurrent(AnalysisNewDEM.Extent) Then
                ' Already concurrent. Use error in situ
                sOldError = m_gOriginalOldError.filepath
            Else
                ' Clip error raster to extent
                If Not External.RasterManager.Copy(m_gOriginalOldError.filepath, sOldError, AnalysisOldDEM.Extent.CellHeight, theAnalysisExtent.Left, theAnalysisExtent.Top,
                                          AnalysisOldDEM.Extent.rows, AnalysisOldDEM.Extent.cols, Project.ProjectManagerBase.GCDNARCError.ErrorString) = External.RasterManagerOutputCodes.PROCESS_OK Then
                    Throw New Exception(Project.ProjectManagerBase.GCDNARCError.ErrorString.ToString)
                End If
                ' GP.SpatialAnalyst.Raster_Calculator(Chr(34) & m_gOriginalOldError.FullPath & Chr(34), sOldError, theAnalysisExtent.Rectangle, m_gOriginalOldError.CellSize)
            End If

            m_gAnalysisNewError = New GCDConsoleLib.Raster(sNewError)
            m_gAnalysisOldError = New GCDConsoleLib.Raster(sOldError)

            Return theAnalysisExtent

        End Function

    End Class

End Namespace
