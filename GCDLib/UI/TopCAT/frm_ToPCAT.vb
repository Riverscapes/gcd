Imports System.Windows.Forms
Imports Microsoft.Win32
Imports System.IO

Namespace TopCAT

    Public Class frm_ToPCAT

        Public Sub New(ByRef pApp As ESRI.ArcGIS.Framework.IApplication)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
            m_pArcMap = pApp

        End Sub

        Private m_pArcMap As ESRI.ArcGIS.Framework.IApplication

        Dim m_OutputPath_FolderDialog As New FolderBrowserDialog
        Dim m_frmResults As New TopCAT.frm_MessageManager

        '
        'Private Sub selectFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        '    Error_NotSpaceDelimited.Clear()
        '    WindowsFormAssistant.OpenFileDialog("Raw Point Cloud", txtBox_RawPointCloudFile)
        '    ToPCAT_Assistant.CheckIfToPCAT_Ready(ucInputsWindow.txtBox_RawPointCloudFile.Text, btn_Run, ucInputsWindow.btn_RawPointCloud)

        'End Sub

        Private Sub btn_SpatialReference_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SpatialReference.Click


            WindowsFormAssistant.OpenFileDialog("Spatial Reference", txtBox_SpatialReference)
            'Dim finalLocation = WindowsFormAssistant.OpenGxFileDialog("SpatialRef", "Select a spatial reference", 0)

            If Not String.IsNullOrEmpty(txtBox_SpatialReference.Text) Then
                Dim sPRJPath As String = String.Empty

                If String.Compare(IO.Path.GetExtension(txtBox_SpatialReference.Text), ".prj", True) = 0 Then
                    ' Got PRJ File
                    sPRJPath = txtBox_SpatialReference.Text
                ElseIf String.Compare(txtBox_SpatialReference.Text, ".shp", True) Then
                    ' Got shapefile. Get the associated projection
                    sPRJPath = IO.Path.ChangeExtension(txtBox_SpatialReference.Text, "prj")
                    If Not IO.File.Exists(sPRJPath) Then
                        MsgBox(IO.Path.GetFileName(txtBox_SpatialReference.Text) & " does not have a spatial reference and cannot be used to provide a spatial reference to the output shapefile." & vbCrLf & vbCrLf &
                                      "Please select another .shp file or .prj file.")
                        sPRJPath = String.Empty
                    ElseIf IO.File.Exists(sPRJPath) Then
                        txtBox_SpatialReference.Text = sPRJPath
                    End If
                Else
                    txtBox_SpatialReference.Text = String.Empty
                End If

                If IO.File.Exists(sPRJPath) Then
                    '
                    ' TODO: Load projection from PRJ file and send the units to 
                    ' the user control label.
                    '
                    Dim thePrj As ESRI.ArcGIS.Geometry.IProjectedCoordinateSystem = LoadProjectedCoordinateSystem(sPRJPath)
                    If Not thePrj Is Nothing Then
                        If Not thePrj.CoordinateUnit.Name Is Nothing Then
                            ucInputsWindow.Units = thePrj.CoordinateUnit.Name
                        Else
                            ucInputsWindow.Units = Nothing
                        End If
                    End If
                End If
            End If

        End Sub

        Private Sub btn_SelectOutputPath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SelectOutputPath.Click

            m_OutputPath_FolderDialog.Description = "Select a path to contain the output from ToPCAT"
            m_OutputPath_FolderDialog.ShowNewFolderButton = True
            If m_OutputPath_FolderDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
                txtBox_SelectOutputPath.Text = m_OutputPath_FolderDialog.SelectedPath
            End If

        End Sub

        Private Sub runAnalysis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Run.Click



            If Not ValidateForm() Then
                Me.DialogResult = Windows.Forms.DialogResult.None
                Exit Sub
            End If

            ' Get values for cell resolution, n to calculate statistics, and detrending options
            Dim xRes As Decimal = ucInputsWindow.numXresolution.Value
            Dim yRes As Decimal = ucInputsWindow.numYresolution.Value
            Dim nMin As Decimal = ucInputsWindow.numNtoCalculateStats.Value

            'Configure Detrending options
            Dim zMeanOption As Boolean = ucInputsWindow.rdbZmean.Checked
            Dim zMinOption As Boolean = Not ucInputsWindow.rdbZmean.Checked


            Try
                Windows.Forms.Cursor.Current = Cursors.WaitCursor


                Dim resultsString = ToPCAT_Assistant.RunToPCat(ucInputsWindow.txtBox_RawPointCloudFile.Text,
                                                               xRes,
                                                               yRes,
                                                               nMin,
                                                               zMinOption,
                                                               zMeanOption)

                Dim rawPointCloud As New RawPointCloudPaths(ucInputsWindow.txtBox_RawPointCloudFile.Text, m_OutputPath_FolderDialog.SelectedPath)

                If chkCreateZstatShp.Checked OrElse chkCreateZmaxShp.Checked OrElse chkCreateZminShp.Checked OrElse chkCreateZunderPopulatedShp.Checked Then
                    Dim spatialRef As ESRI.ArcGIS.Geometry.ISpatialReference = CType(LoadProjectedCoordinateSystem(txtBox_SpatialReference.Text), ESRI.ArcGIS.Geometry.ISpatialReference)

                    If chkCreateZstatShp.Checked Then
                        ToPCAT_Assistant.MoveToPCAT_File(rawPointCloud, ToPCAT_Assistant.ToPCAT_FileType.zStat, resultsString)
                        Try

                            If ToPCAT_Assistant.CreateToPCAT_Shp(rawPointCloud,
                                                              GISDataStructures.BasicGISTypes.Point,
                                                              False,
                                                              spatialRef,
                                                              ToPCAT_Assistant.ToPCAT_FileType.zStat,
                                                              resultsString) Then

                                If My.Settings.AddOutputLayersToMap Then
                                    Try
                                        TopCAT.GIS.AddShapefile(m_pArcMap, rawPointCloud.zStatShapefile)
                                    Catch ex As Exception
                                        'Do Nothing
                                    End Try
                                End If

                            End If

                            If chkIncludeTextVersions.Checked = False Then
                                IO.File.Delete(rawPointCloud.zStatText)
                            End If

                        Catch ex As Exception

                        End Try
                    ElseIf chkCreateZstatShp.Checked = False Then
                        ToPCAT_Assistant.MoveToPCAT_File(rawPointCloud, ToPCAT_Assistant.ToPCAT_FileType.zStat, resultsString)
                        If chkIncludeTextVersions.Checked = False Then
                            IO.File.Delete(rawPointCloud.zStatText)
                        End If
                    End If

                    If chkCreateZmaxShp.Checked Then

                        ToPCAT_Assistant.MoveToPCAT_File(rawPointCloud, ToPCAT_Assistant.ToPCAT_FileType.zMax, resultsString)
                        Try
                            If ToPCAT_Assistant.CreateToPCAT_Shp(rawPointCloud,
                                                              GISDataStructures.BasicGISTypes.Point,
                                                              False,
                                                              spatialRef,
                                                              ToPCAT_Assistant.ToPCAT_FileType.zMax,
                                                              resultsString) Then

                                If My.Settings.AddOutputLayersToMap Then
                                    Try
                                        TopCAT.GIS.AddShapefile(m_pArcMap, rawPointCloud.zMaxShapefile)
                                    Catch ex As Exception
                                        'Do Nothing
                                    End Try
                                End If

                            End If

                            If chkIncludeTextVersions.Checked = False Then
                                IO.File.Delete(rawPointCloud.zMaxText)
                            End If

                        Catch ex As Exception

                        End Try
                    ElseIf chkCreateZmaxShp.Checked = False Then
                        ToPCAT_Assistant.MoveToPCAT_File(rawPointCloud, ToPCAT_Assistant.ToPCAT_FileType.zMax, resultsString)
                        If chkIncludeTextVersions.Checked = False Then
                            IO.File.Delete(rawPointCloud.zMaxText)
                        End If
                    End If

                    If chkCreateZminShp.Checked Then
                        ToPCAT_Assistant.MoveToPCAT_File(rawPointCloud, ToPCAT_Assistant.ToPCAT_FileType.zMin, resultsString)
                        Try

                            If ToPCAT_Assistant.CreateToPCAT_Shp(rawPointCloud,
                                                                  GISDataStructures.BasicGISTypes.Point,
                                                                  False,
                                                                  spatialRef,
                                                                  ToPCAT_Assistant.ToPCAT_FileType.zMin,
                                                                  resultsString) Then

                                If My.Settings.AddOutputLayersToMap Then
                                    Try
                                        TopCAT.GIS.AddShapefile(m_pArcMap, rawPointCloud.zMinShapefile)
                                    Catch ex As Exception
                                        'Do Nothing
                                    End Try
                                End If
                            End If

                            If chkIncludeTextVersions.Checked = False Then
                                IO.File.Delete(rawPointCloud.zMinText)
                            End If

                        Catch ex As Exception

                        End Try
                    ElseIf chkCreateZminShp.Checked = False Then
                        ToPCAT_Assistant.MoveToPCAT_File(rawPointCloud, ToPCAT_Assistant.ToPCAT_FileType.zMin, resultsString)
                        If chkIncludeTextVersions.Checked = False Then
                            IO.File.Delete(rawPointCloud.zMinText)
                        End If
                    End If

                    If chkCreateZunderPopulatedShp.Checked Then
                        ToPCAT_Assistant.MoveToPCAT_File(rawPointCloud, ToPCAT_Assistant.ToPCAT_FileType.zStatUnderpopulated, resultsString)
                        Try

                            If ToPCAT_Assistant.CreateToPCAT_Shp(rawPointCloud,
                                                                 GISDataStructures.BasicGISTypes.Point,
                                                                 False,
                                                                 spatialRef,
                                                                 ToPCAT_Assistant.ToPCAT_FileType.zStatUnderpopulated,
                                                                 resultsString) Then

                                If My.Settings.AddOutputLayersToMap Then
                                    Try
                                        TopCAT.GIS.AddShapefile(m_pArcMap, rawPointCloud.zStatUnderPopulatedShapefile)
                                    Catch ex As Exception
                                        'Do Nothing
                                    End Try
                                End If
                            End If

                            If chkIncludeTextVersions.Checked = False Then
                                IO.File.Delete(rawPointCloud.zStatUnderPopulatedText)
                            End If

                        Catch ex As Exception

                        End Try
                    ElseIf chkCreateZunderPopulatedShp.Checked = False Then
                        ToPCAT_Assistant.MoveToPCAT_File(rawPointCloud, ToPCAT_Assistant.ToPCAT_FileType.zStatUnderpopulated, resultsString)
                        If chkIncludeTextVersions.Checked = False Then
                            IO.File.Delete(rawPointCloud.zStatUnderPopulatedText)
                        End If
                    End If

                End If

                ToPCAT_Assistant.CollectToPCAT_FormOutputOptions(rawPointCloud,
                                                                 m_OutputPath_FolderDialog.SelectedPath,
                                                                 chkIncludeTextVersions.Checked,
                                                                 chkIncludeBinaryZstats.Checked,
                                                                 chkIncludeBinarySorted.Checked)


                Dim detrendedOption As String
                If ucInputsWindow.rdbZmean.Checked Then
                    detrendedOption = "zmean"
                Else
                    detrendedOption = "zmin"
                End If

                Dim finalMessage As String = "Your file: " & Path.GetFileName(ucInputsWindow.txtBox_RawPointCloudFile.Text) & " was successfully decimated." & vbCrLf &
                      "Decimation resolution x: " & xRes & vbCrLf &
                      "Decimation resolution y: " & yRes & vbCrLf &
                      "Minimum points used to calculate statistics: " & nMin & vbCrLf & vbCrLf &
                      "The detrended standard deviation for each sample window was calculated as " & ucInputsWindow.numStdevDetrendedOption.Value.ToString &
                      " standard deviations from the " & detrendedOption & "." & vbCrLf & vbCrLf

            Catch ex As Exception
                ExceptionUI.HandleException(ex)

            Finally
                Windows.Forms.Cursor.Current = Cursors.Default
            End Try

        End Sub

        ''' <summary>
        ''' Validate all the inputs are good.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ValidateForm() As Boolean

            If Not ucInputsWindow.ValidateUserControl Then
                Return False
            End If

            If String.IsNullOrEmpty(txtBox_SelectOutputPath.Text) Then
                MsgBox("Please select a folder to save the output to.", MsgBoxStyle.Information, "No Folder Selected to Save Output")
                Return False
            Else
                If Not Directory.Exists(txtBox_SelectOutputPath.Text) Then
                    MsgBox("The output folder you provided does not exist. Please provide an existing folder.", MsgBoxStyle.Information, "No Folder Selected to Save Output")
                    Return False
                End If
            End If

            'Check if any shapefiles will be created, if so then a spatial reference should be provided
            If String.IsNullOrEmpty(txtBox_SpatialReference.Text) Then
                If String.IsNullOrEmpty(txtBox_SpatialReference.Text) AndAlso (chkCreateZmaxShp.Checked OrElse chkCreateZminShp.Checked OrElse chkCreateZstatShp.Checked OrElse chkCreateZunderPopulatedShp.Checked) Then
                    MsgBox("Please select a spatial reference to for the resulting shapefile(s)", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                    Return False
                End If
            Else
                If Not File.Exists(txtBox_SpatialReference.Text) Then
                    MsgBox("The spatial reference file you provided does not exist. Please provide an existing spatial reference file.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                    Return False
                End If
            End If

            Return True

        End Function

        Private Sub detrendedOptionsInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

            System.Diagnostics.Process.Start(My.Resources.HelpBaseURL & "gcd-command-reference/data-prep-menu/e-topcat-menu/ii-topcat-point-cloud-decimation-tool")

        End Sub

        Private Sub outputFileDescription_btn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OutputFileDescription.Click

            System.Diagnostics.Process.Start(My.Resources.HelpBaseURL & "gcd-command-reference/data-prep-menu/e-topcat-menu/ii-topcat-point-cloud-decimation-tool")

        End Sub

        Private Sub btnHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Help.Click

            System.Diagnostics.Process.Start(My.Resources.HelpBaseURL & "gcd-command-reference/data-prep-menu/e-topcat-menu/ii-topcat-point-cloud-decimation-tool")

        End Sub

        Private Sub frm_ToPCAT_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            'Tooltips
            tTip.SetToolTip(chkCreateZmaxShp, "Check box to create a point shapefile of the maximum elevation in each sample window.")
            tTip.SetToolTip(chkCreateZminShp, "Check box to create a point shapefile of the minimum elevation in each sample window.")
            tTip.SetToolTip(chkCreateZstatShp, "Check box to create a point shapefile of the statistics calculated for each sample window.")
            tTip.SetToolTip(chkCreateZunderPopulatedShp, "Check box to create a point shapefile of the sample windows where not enough points existed within to calculate statisitics.")
            tTip.SetToolTip(chkIncludeBinarySorted, "Check box to include a binary version of the sorted sample windows.")
            tTip.SetToolTip(chkIncludeBinaryZstats, "Check box to include a binary version of the statistics calculated for each sample window.")
            tTip.SetToolTip(chkIncludeTextVersions, "Check box to include text versions of the zmin, zmax, zstats, and underpopulated files.")
            tTip.SetToolTip(btn_OutputFileDescription, "Click to go the documentation about the outputs from ToPCAT.")
            tTip.SetToolTip(btn_SpatialReference, "Press this button to open a file dialog and select a file (.prj or .shp) containing spatial reference information.")
            tTip.SetToolTip(txtBox_SpatialReference, "Displays the file name of the spatial reference. Use the selection button to the right to populate this field.")
            tTip.SetToolTip(btn_SelectOutputPath, "Press button to open a folder dialog to select the folder where all selected output will be saved to.")
            tTip.SetToolTip(txtBox_SelectOutputPath, "Displays the folder selected to save the output to. Use the selection button to the right to populate this field.")
            tTip.SetToolTip(btn_Run, "Click to run the analysis")
            tTip.SetToolTip(btn_Help, "Click to go to the tool documentation.")
            tTip.SetToolTip(btn_Cancel, "Cancel analysis and exit the tool window.")

            'Status
            UpdateControls()

            ' Start up the BackgroundWorker1.


        End Sub


        Public Shared Function LoadProjectedCoordinateSystem(ByVal prjFile As String) As ESRI.ArcGIS.Geometry.IProjectedCoordinateSystem

            Dim factoryType As Type = Type.GetTypeFromProgID("esriGeometry.SpatialReferenceEnvironment")
            Dim spatialRefFactory As ESRI.ArcGIS.Geometry.ISpatialReferenceFactory3 = CType(Activator.CreateInstance(factoryType), ESRI.ArcGIS.Geometry.ISpatialReferenceFactory3)
            Dim projectedCoordinateSystem As ESRI.ArcGIS.Geometry.IProjectedCoordinateSystem = CType(spatialRefFactory.CreateESRISpatialReferenceFromPRJFile(prjFile), ESRI.ArcGIS.Geometry.IProjectedCoordinateSystem)

            Return projectedCoordinateSystem

        End Function

        Public Shared Function OpenFeatureClassWorkspace(ByVal filePath As String) As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace

            Dim workspaceFactory As ESRI.ArcGIS.Geodatabase.IWorkspaceFactory = New ESRI.ArcGIS.DataSourcesFile.ShapefileWorkspaceFactoryClass()
            Dim workspace As ESRI.ArcGIS.Geodatabase.IWorkspace = workspaceFactory.OpenFromFile(IO.Path.GetDirectoryName(filePath), 0)
            Dim featureWorkspace As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace = CType(workspace, ESRI.ArcGIS.Geodatabase.IFeatureWorkspace)
            Return featureWorkspace

        End Function

        Private Sub UpdateControls()

            Dim bNeedsSpatialRef As Boolean = chkCreateZmaxShp.Checked OrElse chkCreateZminShp.Checked OrElse chkCreateZstatShp.Checked OrElse chkCreateZunderPopulatedShp.Checked
            txtBox_SpatialReference.Enabled = bNeedsSpatialRef
            btn_SpatialReference.Enabled = bNeedsSpatialRef
            lblSpatialRef.Enabled = bNeedsSpatialRef

        End Sub

        Private Sub chkCreateZmaxShp_CheckedChanged(ByVal sender As Object,
                                                    ByVal e As System.EventArgs) _
                                                    Handles chkCreateZmaxShp.CheckedChanged,
                                                     chkCreateZminShp.CheckedChanged,
                                                     chkCreateZstatShp.CheckedChanged,
                                                     chkCreateZunderPopulatedShp.CheckedChanged
            UpdateControls()
        End Sub


        Public Shared Function CreateTemporaryFeatureClass(ByVal outFeatureClass As String, ByVal eType As GISDataStructures.BasicGISTypes, ByVal b3D As Boolean, ByVal pSpatRef As ESRI.ArcGIS.Geometry.ISpatialReference, Optional ByVal pInputFields As ESRI.ArcGIS.Geodatabase.IFields2 = Nothing) As ESRI.ArcGIS.Geodatabase.IFeatureClass

            If String.IsNullOrEmpty(outFeatureClass) Then
                Throw New Exception("Null or empty feature class name")
            End If

            If Not TypeOf pSpatRef Is ESRI.ArcGIS.Geometry.ISpatialReference Then
                Throw New Exception("Invalid or null spatial reference")
            End If

            Dim pFCResult As ESRI.ArcGIS.Geodatabase.IFeatureClass = Nothing
            Dim gResult As GISDataStructures.VectorDataSource = Nothing

            'Dim sWorkspacePath As String = GetWorkspacePath(outFeatureClass) ' IO.Path.GetDirectoryName(outFeatureClass) ' FeatureClass.GetWorkspacePath(outFeatureClass)
            Dim pWorkFact As ESRI.ArcGIS.Geodatabase.IWorkspaceFactory = New ESRI.ArcGIS.DataSourcesGDB.InMemoryWorkspaceFactoryClass()

            ' Create a new in-memory workspace. This returns a name object.
            Dim pWorkspaceName As ESRI.ArcGIS.Geodatabase.IWorkspaceName = pWorkFact.Create(Nothing, "InMemoryWorkspace", Nothing, 0)
            Dim pName As ESRI.ArcGIS.esriSystem.IName = DirectCast(pWorkspaceName, ESRI.ArcGIS.esriSystem.IName)

            ' Open the workspace through the name object.
            Dim pWorkspace As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace = DirectCast(pName.Open(), ESRI.ArcGIS.Geodatabase.IFeatureWorkspace)

            Dim sFeatureClassName As String
            Dim pFDS As ESRI.ArcGIS.Geodatabase.IFeatureDataset = Nothing

            Try
                'Create shape field
                Dim pGDef As ESRI.ArcGIS.Geodatabase.IGeometryDef = New ESRI.ArcGIS.Geodatabase.GeometryDef
                Dim pGDefEdit As ESRI.ArcGIS.Geodatabase.IGeometryDefEdit = pGDef

                Select Case eType
                    Case GISDataStructures.BasicGISTypes.Point : pGDefEdit.GeometryType_2 = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint
                    Case GISDataStructures.BasicGISTypes.Line : pGDefEdit.GeometryType_2 = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline
                    Case GISDataStructures.BasicGISTypes.Polygon : pGDefEdit.GeometryType_2 = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon
                    Case Else
                        Dim ex As New Exception("Invalid GIS data type")
                        ex.Data.Add("Type", eType.ToString)
                        Throw ex
                End Select
                pGDefEdit.HasZ_2 = b3D

                'Set projection
                pGDefEdit.SpatialReference_2 = pSpatRef

                Dim pField As ESRI.ArcGIS.Geodatabase.IField = New ESRI.ArcGIS.Geodatabase.Field
                Dim pFieldEdit As ESRI.ArcGIS.Geodatabase.IFieldEdit = pField
                pFieldEdit.Name_2 = "Shape"
                pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeGeometry
                pFieldEdit.GeometryDef_2 = pGDef

                Dim pFields As ESRI.ArcGIS.Geodatabase.IFields2 = New ESRI.ArcGIS.Geodatabase.Fields
                Dim pFieldsEdit As ESRI.ArcGIS.Geodatabase.IFieldsEdit = pFields
                pFieldsEdit.AddField(pField)

                If Not pInputFields Is Nothing Then
                    For index As Integer = 0 To pInputFields.FieldCount - 1
                        Dim pInputField As ESRI.ArcGIS.Geodatabase.IField
                        pInputField = pInputFields.Field(index)
                        pFieldsEdit.AddField(pInputField)
                    Next
                End If

                'Dim sFullPath As String = pWorkspaceName.PathName
                sFeatureClassName = Path.GetFileNameWithoutExtension(outFeatureClass)
                pFCResult = pWorkspace.CreateFeatureClass(sFeatureClassName, pFields, Nothing, Nothing, ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTSimple, "Shape", "")
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFields)
                'sFullPath = IO.Path.Combine(sFullPath, sFeatureClassName)

                'Dim pWS As ESRI.ArcGIS.Geodatabase.IWorkspace = pWorkspace
                'If pWS.Type = ESRI.ArcGIS.Geodatabase.esriWorkspaceType.esriFileSystemWorkspace Then
                '    sFullPath = IO.Path.ChangeExtension(sFullPath, "shp")
                'End If

                'Select Case eType
                '    Case GISDataStructures.BasicGISTypes.Point
                '        If b3D Then
                '            gResult = New GISDataStructures.PointDataSource3D(sFullPath)
                '        Else
                '            gResult = New GISDataStructures.PointDataSource(sFullPath)
                '        End If

                '    Case GISDataStructures.BasicGISTypes.Line
                '        gResult = New GISDataStructures.PolylineDataSource(sFullPath)

                '    Case GISDataStructures.BasicGISTypes.Polygon
                '        gResult = New GISDataStructures.PolygonDataSource(sFullPath)
                'End Select

            Catch ex As Exception
                Dim e As New Exception("Error creating new feature class", ex)
                e.Data.Add("outFeatureClass", outFeatureClass)
                e.Data.Add("eType", eType.ToString)
                e.Data.Add("b3D", b3D.ToString)
                Throw e
            End Try

            Return pFCResult

        End Function

        Public Shared Function CreateField(ByVal pFieldName As String,
                                           ByVal pGeometryType As ESRI.ArcGIS.Geometry.esriGeometryType,
                                           ByVal pSpatialRef As ESRI.ArcGIS.Geometry.ISpatialReference,
                                           ByVal pFieldType As ESRI.ArcGIS.Geodatabase.esriFieldType,
                                           ByVal b3D As Boolean) As ESRI.ArcGIS.Geodatabase.IField

            Dim pGDef As ESRI.ArcGIS.Geodatabase.IGeometryDef = New ESRI.ArcGIS.Geodatabase.GeometryDef
            Dim pGDefEdit As ESRI.ArcGIS.Geodatabase.IGeometryDefEdit = pGDef
            pGDefEdit.GeometryType_2 = pGeometryType
            'Dim ex As New Exception("Invalid GIS data type")
            'ex.Data.Add("Type", pGeometryType.ToString)
            'Throw ex

            pGDefEdit.SpatialReference_2 = pSpatialRef
            pGDefEdit.HasZ_2 = b3D

            Dim pField As ESRI.ArcGIS.Geodatabase.IField = New ESRI.ArcGIS.Geodatabase.Field
            Dim pFieldEdit As ESRI.ArcGIS.Geodatabase.IFieldEdit = pField
            pFieldEdit.Name_2 = pFieldName
            pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeGeometry
            pFieldEdit.GeometryDef_2 = pGDef

            'Dim pFields As ESRI.ArcGIS.Geodatabase.IFields2 = New ESRI.ArcGIS.Geodatabase.Fields
            'Dim pFieldsEdit As ESRI.ArcGIS.Geodatabase.IFieldsEdit = pFields
            'pFieldsEdit.AddField(pField)

            Return pField
        End Function

        'Dim field As ESRI.ArcGIS.Geodatabase.IField = New ESRI.ArcGIS.Geodatabase.FieldClass
        'Dim fieldEdit As ESRI.ArcGIS.Geodatabase.IFieldEdit = CType(field, ESRI.ArcGIS.Geodatabase.IFieldEdit)

        ''setup field properties
        'fieldEdit.Name_2 = "zmin"
        'fieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
        'fieldEdit.IsNullable_2 = True
        'fieldEdit.AliasName_2 = "Test"
        'fieldEdit.DefaultValue_2 = 0.0
        'fieldEdit.Editable_2 = True
        'testShp.AddField(field)


        Private Sub BackgroundWorker1_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

            m_frmResults.Show()

        End Sub
    End Class


End Namespace