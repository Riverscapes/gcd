Imports GCDAddIn.TopCAT
Imports System.Windows.Forms

Namespace UI.SurveyLibrary

    Public Class frmSurfaceRoughness

        Private m_dReferenceResolution As Double

        Public Sub New(ByVal gReferenceResolution As Double)

            InitializeComponent()
            m_dReferenceResolution = gReferenceResolution

        End Sub

        Public Function CalculateRoughness(ByVal sOutputPath As String, ByRef gDEMRaster As GCDConsoleLib.Raster) As Boolean

            'Create dedicated folder in temp workspace
            Dim bResult As Boolean
            'Dim sTemp As String = Core.WorkspaceManager.WorkspacePath

            'Cursor.Current = Cursors.WaitCursor
            'Dim tempDir = WindowsManagement.CreateTemporaryDirectory("ToPCAT_Raster")

            'Try

            '    ToPCAT_Assistant.RunToPCat(ucToPCAT_Inputs.txtBox_RawPointCloudFile.Text,
            '                           ucToPCAT_Inputs.numXresolution.Value,
            '                           ucToPCAT_Inputs.numYresolution.Value,
            '                           ucToPCAT_Inputs.numNtoCalculateStats.Value.ToString)


            '    Dim sMessages As String = Nothing
            '    If Not TopCAT.ToPCAT_Assistant.MoveToPCAT_TextFiles(ucToPCAT_Inputs.txtBox_RawPointCloudFile.Text, tempDir, sMessages) Then
            '        MsgBox(sMessages, MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
            '        Return False
            '    End If
            '    Dim rawPointCloudPaths As New RawPointCloudPaths(ucToPCAT_Inputs.txtBox_RawPointCloudFile.Text, tempDir)

            '    'Initialize the geoprocessor engine
            '    Dim geoProcessorEngine As ESRI.ArcGIS.Geoprocessing.GeoProcessor = New ESRI.ArcGIS.Geoprocessing.GeoProcessor()


            '    'Create an IVariant to hold the parameter values & populate the variant array with parameter values
            '    '
            '    Dim tempPointLayerParameters As ESRI.ArcGIS.esriSystem.IVariantArray = New ESRI.ArcGIS.esriSystem.VarArray
            '    tempPointLayerParameters.Add(rawPointCloudPaths.zStatText)
            '    tempPointLayerParameters.Add("x")
            '    tempPointLayerParameters.Add("y")
            '    tempPointLayerParameters.Add("tempLayer")

            '    'Dim spatialRef As ESRI.ArcGIS.Geometry.ISpatialReference = TryCast(gDEMRaster.SpatialReference, ESRI.ArcGIS.Geometry.ISpatialReference)
            '    tempPointLayerParameters.Add(gDEMRaster.SpatialReference)
            '    tempPointLayerParameters.Add("stdev_detrended")
            '    MyGeoprocessing.RunToolGeoprocessingTool("MakeXYEventLayer_management", geoProcessorEngine, tempPointLayerParameters, True)

            '    'Set the geoprocessing extent environment
            '    geoProcessorEngine.SetEnvironmentValue("extent", gDEMRaster.ExtentAsString)

            '    Dim pointToRasterParameters As ESRI.ArcGIS.esriSystem.IVariantArray = New ESRI.ArcGIS.esriSystem.VarArray
            '    pointToRasterParameters.Add(tempPointLayerParameters.Element(3))
            '    pointToRasterParameters.Add("stdev_detrended")
            '    pointToRasterParameters.Add(sOutputPath)
            '    pointToRasterParameters.Add("")
            '    pointToRasterParameters.Add("")
            '    pointToRasterParameters.Add(ucToPCAT_Inputs.numXresolution.Value.ToString())
            '    MyGeoprocessing.RunToolGeoprocessingTool("PointToRaster_conversion", geoProcessorEngine, pointToRasterParameters, True)

            '    'Define projection for output raster
            '    '
            '    ' Temporary fix. The above routine seems does not project
            '    ' the resultant raster. So assume that the output project is identical to the
            '    ' input project and simply use the geoprocessing routine to define it.
            '    Try

            '        Dim pSR As ESRI.ArcGIS.Geometry.ISpatialReference = Nothing
            '        If TypeOf gDEMRaster Is GCDConsoleLib.RasterDirect Then
            '            pSR = gDEMRaster.SpatialReference
            '        End If

            '        If IO.File.Exists(sOutputPath) Then
            '            GP.DataManagement.DefineProjection(sOutputPath, pSR)
            '        End If

            '    Catch
            '        'Do nothing
            '    End Try

            '    bResult = True
            'Catch ex As Exception
            '    bResult = False
            'Finally
            '    System.IO.Directory.Delete(tempDir, True)
            '    Cursor.Current = Cursors.Default
            'End Try

            Return bResult

        End Function

        Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

            'TODO Validate the user control 
            If Not ucToPCAT_Inputs.ValidateUserControl() Then
                Me.DialogResult = DialogResult.None
                Exit Sub
            End If

            'Validate x and y resolution with project cell resolution
            Dim iXResolution As Double = ucToPCAT_Inputs.numXresolution.Value
            Dim iYResolution As Double = ucToPCAT_Inputs.numYresolution.Value

            If Not ValidateResolutions(m_dReferenceResolution, iXResolution, iYResolution) Then
                Me.DialogResult = DialogResult.None
                Exit Sub
            End If

        End Sub

        Private Function ValidateResolutions(ByVal dReferenceResolution As Double, ByVal dXResolution As Double, ByVal dYResolution As Double) As Boolean

            If dXResolution <> dYResolution Then
                MsgBox("X and Y parameters for sample window size must match.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                Return False
            End If

            If dXResolution <> dReferenceResolution Or dYResolution <> dReferenceResolution Then
                MsgBox(String.Format("X and Y parameters for sample window both must match the cell resolution, {0}, of the project.", dReferenceResolution),
                                 MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                Return False
            End If

            Return True

        End Function

        Private Sub SurfaceRoughnessForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            ' hide advanced tab
            Me.ucToPCAT_Inputs.tbcDecimationDetrending.TabPages.Remove(ucToPCAT_Inputs.tbpDetrending)

        End Sub

        Private Sub btnHelp_Click(sender As System.Object, e As System.EventArgs) Handles btnHelp.Click
            Process.Start(My.Resources.HelpBaseURL & "gcd-command-reference/gcd-project-explorer/d-dem-context-menu/iv-add-associated-surface/4-deriving-roughness")
        End Sub
    End Class

End Namespace