Imports ESRI.ArcGIS.Geometry

'TODO: Enable the python script to handle non z-enabled shapefiles

Public Class ErrorGenerator_BootstrapRoughness

    Private m_gPoints As GISDataStructures.PointDataSource
    Private m_gSurveyExtent As GISDataStructures.PolygonDataSource
    Private m_gInChannel As GISDataStructures.PolygonDataSource
    Private m_gDEM As GISDataStructures.Raster

    ''' <summary>
    ''' RBT Constructor
    ''' </summary>
    ''' <param name="gPoints">Feature class of all points in survey</param>
    ''' <param name="gSurveyExtent">Survey Extent polgyon</param>
    ''' <param name="gInChannel">Water Extent Polygon</param>
    ''' <param name="gDEM">DEM created from feature class points</param>
    ''' <remarks></remarks>
    Public Sub New(gPoints As GISDataStructures.PointDataSource, ByVal gSurveyExtent As GISDataStructures.PolygonDataSource, gInChannel As GISDataStructures.PolygonDataSource, gDEM As GISDataStructures.Raster)
        m_gPoints = gPoints
        m_gSurveyExtent = gSurveyExtent
        m_gInChannel = gInChannel
        m_gDEM = gDEM
    End Sub


    ''' <summary>
    ''' Constructor for pure bootstrap no in channel/out channel
    ''' </summary>
    ''' <param name="gPoints">Feature class of all points in survey</param>
    ''' <param name="gSurveyExtent">Survey Extent polgyon</param>
    ''' <param name="gDEM">DEM created from feature class points</param>
    ''' <remarks></remarks>
    Public Sub New(gPoints As GISDataStructures.PointDataSource, ByVal gSurveyExtent As GISDataStructures.PolygonDataSource, gDEM As GISDataStructures.Raster)
        m_gPoints = gPoints
        m_gSurveyExtent = gSurveyExtent
        m_gDEM = gDEM
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nIterations">Number of iterations to run tool</param>
    ''' <param name="nPointSamplePercent">Numer of points to sample. Use 85 as the default</param>
    ''' <param name="sTempWorkspace">The GCD Temporary workspace, i.e. appdata/GCD/TempWorkspace</param>
    ''' <param name="sOutputRaster">path to create output raster</param>
    ''' <param name="eDEMUnits">NumberFormatting.LinearUnits that the DEM is in</param>
    ''' <returns>GISDataStructures.Raster that is concurrent and orthogonal with m_gDEM and is surface roughness in millimeters to be used with Wentworth Pebble Scale</returns>
    ''' <remarks></remarks>
    Public Function Execute(nIterations As UInteger, nPointSamplePercent As Single, sTempWorkspace As String, sOutputRaster As String, Optional ByVal eDEMUnits As Object = NumberFormatting.LinearUnits.m) As GISDataStructures.Raster

        'Validate Execute inputs
        ValidateExcecuteInputs(nIterations, nPointSamplePercent, sTempWorkspace, sOutputRaster, eDEMUnits)

        Dim lParams As New List(Of String)
        lParams.Add(m_gPoints.FullPath) 'Survey Points Path
        lParams.Add(m_gSurveyExtent.FullPath) 'Survey Extent Path
        If m_gInChannel Is Nothing Then
            lParams.Add("")
        Else
            lParams.Add(m_gInChannel.FullPath) 'In Channel Extent Path (Optional but still provide empty string)
        End If
        lParams.Add(m_gDEM.FullPath) 'DEM Path
        Dim sIterations As String = nIterations.ToString()
        lParams.Add(sIterations) 'Number of iterations
        Dim sPercentData As String = nPointSamplePercent.ToString()
        lParams.Add(sPercentData) 'Percent data to withhold each iteration

        Dim sCellSize As String = m_gDEM.CellSize.ToString()
        lParams.Add(sCellSize) ' Cell size
        lParams.Add(sTempWorkspace) ' GCD Temp Workspace
        lParams.Add(sOutputRaster) 'Output raster path

        If eDEMUnits Is Nothing Then
            lParams.Add("")
        Else
            Dim sUnits As String = eDEMUnits.ToString()
            lParams.Add(sUnits)
        End If

        Dim sToolboxPath As String = IO.Path.Combine(IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "GISCode\GCD\Python\GCD_Python.pyt")
        Debug.Assert(System.IO.File.Exists(sToolboxPath), String.Format("The {0} toolbox path does not exist.", sToolboxPath))

        GISCode.Toolbox.ToolBox.RunTool(sToolboxPath, "BootstrapRoughness", lParams)

        'Validate raster output was created
        Dim gResult As GISDataStructures.Raster
        If System.IO.File.Exists(sOutputRaster) Then
            gResult = New GISDataStructures.Raster(sOutputRaster)
        Else
            Throw New Exception(String.Format("The in bootstrap roughness raster {0} was not created.", sOutputRaster))
            Exit Function
        End If

        Return gResult

    End Function

    Private Sub ValidateExcecuteInputs(ByVal nIterations As UInteger, ByVal nPointSamplePercent As Single, ByVal sTempWorkspace As String, ByVal sOutputRaster As String, ByVal eUnits As NumberFormatting.LinearUnits)

        If nIterations <= 0 Then
            Throw New Exception("Number of iterations must be greater than 0")
        End If

        If nPointSamplePercent < 0.01 Or nPointSamplePercent > 0.99 Then
            Throw New Exception("Percent of sampled points is not within accepted range")
        End If

        If String.IsNullOrEmpty(sOutputRaster) Then
            Throw New Exception("Path for output raster cannot be empty.")
        End If

        If Not String.IsNullOrEmpty(sTempWorkspace) Then
            If Not System.IO.Directory.Exists(sTempWorkspace) Then
                Throw New Exception("The temporary workspace provided does not exist.")
            End If
        End If

        If eUnits <> NumberFormatting.LinearUnits.m And _
            eUnits <> NumberFormatting.LinearUnits.cm And _
            eUnits <> NumberFormatting.LinearUnits.mm And _
            eUnits <> NumberFormatting.LinearUnits.ft And _
            eUnits <> NumberFormatting.LinearUnits.inch And _
            eUnits <> NumberFormatting.LinearUnits.km And _
            eUnits <> NumberFormatting.LinearUnits.yard And _
            eUnits <> NumberFormatting.LinearUnits.mile Then
            Throw New Exception("Unsupported linear unit used.")
        End If

    End Sub

End Class
