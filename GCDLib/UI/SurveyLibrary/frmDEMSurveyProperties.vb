﻿Namespace UI

    Public Class frmDEMSurveyProperties

        Private m_DEMSurveyID As Integer
        Private m_ImportRasterform As frmImportRaster
        Private m_SurveyDateTime As SurveyDateTime

#Region "Survey Property Routines"

        Public Sub New(ByRef pApp As IApplication, ByVal nSurveyID As Integer)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
            m_pArcMap = pApp
            m_DEMSurveyID = nSurveyID
            m_ImportRasterform = Nothing
            m_SurveyDateTime = New SurveyDateTime
        End Sub

        Private Sub SurveyPropertiesForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            SetToolTips()
            SurveyTypesBindingSource.DataSource = ProjectManager.surveyds
            DEMSurveyBindingSource.DataSource = ProjectManager.ds
            DEMSurfaceBindingSource.DataSource = ProjectManager.ds
            ErrorTableBindingSource.DataSource = ProjectManager.ds

            If m_DEMSurveyID < 1 Then
                'DEMSurveyBindingSource.AddNew()
                Throw New Exception("This form does not allow new DEM Surveys.")
            Else
                DEMSurveyBindingSource.Filter = "DEMSurveyID = " & m_DEMSurveyID
                Dim demRow As ProjectDS.DEMSurveyRow = DirectCast(DEMSurveyBindingSource.Current, DataRowView).Row
                txtName.Text = demRow.Name
                txtRasterPath.Text = demRow.Source
                txtMask.Text = demRow.MethodMask
                txtFolder.Text = GCD.GCDProject.ProjectManager.OutputManager.DEMSurveyFolder(demRow.Name)
                rdoSingle.Checked = demRow.SingleMethod
                rdoMulti.Checked = demRow.MultiMethod

                ' Select the method mask field
                If demRow.MultiMethod Then
                    Dim sMaskPath As String = GCD.GCDProject.ProjectManager.GetAbsolutePath(txtMask.Text)
                    If GISDataStructures.VectorDataSource.Exists(sMaskPath, esriGeometryPolygon) Then
                        Dim gMask As New GISDataStructures.PolygonDataSource(sMaskPath)
                        gMask.FillComboWithFields(cboIdentify, "Method", esriFieldType.esriFieldTypeString, True)
                        For i As Integer = 0 To cboIdentify.Items.Count - 1
                            If String.Compare(cboIdentify.Items(i).ToString, demRow.MethodMaskField, True) = 0 Then
                                cboIdentify.SelectedIndex = i
                                Exit For
                            End If
                        Next
                    End If
                Else
                    For i As Integer = 0 To cboSingle.Items.Count - 1
                        If String.Compare(DirectCast(cboSingle.Items(i), DataRowView).Item("Name"), demRow.SingleMethodType, True) = 0 Then
                            cboSingle.SelectedIndex = i
                            Exit For
                        End If
                    Next
                End If

                m_SurveyDateTime = New SurveyDateTime(demRow)
            End If

            lblDatetime.Text = m_SurveyDateTime.ToString

            DEMSurfaceBindingSource.Filter = "DEMSurveyID = " & m_DEMSurveyID
            ErrorTableBindingSource.Filter = "DEMSurveyID = " & m_DEMSurveyID

            UpdateControls()
            LoadRasterProperties()
        End Sub

        Private Sub SetToolTips()
            'General Tooltips
            'ttpTooltip.SetToolTip(btnCancel, My.Resources.ttpCancel)
            'ttpTooltip.SetToolTip(btnHelp, My.Resources.ttpHelp)

            ''DEM Survey Tooltips
            'ttpTooltip.SetToolTip(txtName, My.Resources.ttpBudgetFormBtnCalculate)

            'ttpTooltip.SetToolTip(txtName, My.Resources.ttpSurveyPropertiesDEMSurveyName)
            'ttpTooltip.SetToolTip(txtDate, My.Resources.ttpSurveyPropertiesDEMSurveyDate)
            ''ttpTooltip.SetToolTip(cboLayer, My.Resources.ttpDEMLayer)
            'ttpTooltip.SetToolTip(cboSingle, My.Resources.ttpSurveyPropertiesSingleCombo)
            ''ttpTooltip.SetToolTip(cboPolyMask, My.Resources.ttpPolyMask)
            'ttpTooltip.SetToolTip(cboIdentify, My.Resources.ttpSurveyPropertiesIdentifierField)
            'ttpTooltip.SetToolTip(cmdAddToMap, My.Resources.ttpSurveyPropertiesBrowseFile)
            'ttpTooltip.SetToolTip(btnBrowseMask, My.Resources.ttpSurveyPropertiesBrowseMask)
            'ttpTooltip.SetToolTip(txtRasterPath, My.Resources.ttpSurveyPropertiesTxtFolder)
            'ttpTooltip.SetToolTip(txtMask, My.Resources.ttpSurveyPropertiesTxtMask)
            'ttpTooltip.SetToolTip(rdoSingle, My.Resources.ttpSurveyPropertiesSingleMethod)
            'ttpTooltip.SetToolTip(rdoMulti, My.Resources.ttpSurveyPropertiesMultiMethod)

            ''Associated Surfaces Tooltips
            'ttpTooltip.SetToolTip(btnAddAssociatedSurface, My.Resources.ttpAddAS)
            'ttpTooltip.SetToolTip(btnSettingsAssociatedSurface, My.Resources.ttpSettingsAS)
            'ttpTooltip.SetToolTip(btnDeleteAssociatedSurface, My.Resources.ttpDeleteAS)
            'ttpTooltip.SetToolTip(btnAddToMap, My.Resources.ttpAddAStoMap)

            ''Error Calculation Tooltips
            'ttpTooltip.SetToolTip(btnAddError, "Associate an existing raster as an error surface")
            'ttpTooltip.SetToolTip(btnCalculateError, "Calculate a new error raster surface")
            'ttpTooltip.SetToolTip(btnErrorSettings, My.Resources.ttpSettingsError)
            'ttpTooltip.SetToolTip(btnErrorDelete, My.Resources.ttpDeleteError)

        End Sub

        Private Function ValidateForm() As Boolean

            For Each aDEM As ProjectDS.DEMSurveyRow In GCD.GCDProject.ProjectManagerUI.ds.DEMSurvey
                If aDEM.DEMSurveyID <> m_DEMSurveyID Then
                    If String.Compare(aDEM.Name, txtName.Text, True) = 0 Then
                        MessageBox.Show(String.Format("Ther is already another DEM survey in this project with the name '{0}'. Each DEM Survey must have a unique name.", txtName.Text))
                        Return False
                    End If
                End If
            Next

            If rdoSingle.Checked Then
                If String.IsNullOrEmpty(cboSingle.Text) Then
                    MsgBox("You must select a survey method type.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                    Return False
                End If
            Else
                If String.IsNullOrEmpty(txtMask.Text) Then
                    MsgBox("You must select a survey mask feature class.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                    Return False
                End If

                If String.IsNullOrEmpty(cboIdentify.Text) Then
                    MsgBox("You must select a survey method identifier field.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                    Return False
                End If
            End If

            Return True

        End Function

        'Private Sub txtMask_TextChanged(sender As Object, e As System.EventArgs) Handles txtMask.TextChanged

        '    If Not String.IsNullOrEmpty(txtMask.Text) Then

        '        cboIdentify.Items.Clear()

        '        Dim sFolder As String = IO.Path.GetDirectoryName(txtMask.Text)
        '        Dim sName As String = IO.Path.GetFileNameWithoutExtension(txtMask.Text)

        '        Dim pWorkFact As IWorkspaceFactory2
        '        If txtMask.Text.ToLower.Contains(".gdb") Then
        '            pWorkFact = New FileGDBWorkspaceFactory
        '        Else
        '            pWorkFact = New ShapefileWorkspaceFactoryClass
        '        End If

        '        Dim pWork As IFeatureWorkspace = pWorkFact.OpenFromFile(sFolder, 0)
        '        Dim pFClass As IFeatureClass = pWork.OpenFeatureClass(sName)

        '        'Get the Fields collection from the feature class
        '        Dim pFields As IFields = pFClass.Fields
        '        Dim Field As IField = Nothing

        '        'Interate through the fields in the collection
        '        For i As Integer = 0 To pFields.FieldCount - 1
        '            Field = pFields.Field(i)

        '            'Add field to combobox
        '            If Field.Type = esriFieldType.esriFieldTypeString Then
        '                Dim TextField As String
        '                TextField = Field.AliasName
        '                cboIdentify.Items.Add(TextField)
        '            End If
        '        Next
        '        '
        '        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '        ' PGB - 8 July 2011 - Help the user by selecting probable fields that might contain elevation data.
        '        ' Build a list of probably elevation field names. These will then be checked against the fields in
        '        ' the point cloud shapefile (case insensitive) and the last match used as the default field.
        '        '
        '        Dim lMaskfields As New List(Of String)
        '        lMaskfields.Add("type")
        '        lMaskfields.Add("method")

        '        For Each sField In lMaskfields
        '            For i = 0 To cboIdentify.Items.Count - 1
        '                If String.Compare(sField, cboIdentify.Items(i), True) = 0 Then
        '                    cboIdentify.SelectedIndex = i
        '                    Exit For
        '                End If
        '            Next
        '        Next

        '    End If

        'End Sub

        Private Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click

            If Not String.IsNullOrEmpty(txtRasterPath.Text) Then ' layer has something!

                Dim message As String = "You have loaded DEM files into your project, are you sure you want to exit without saving this survey to your project's survey library in the project GCD file?"
                Dim caption As String = "Are you sure?"
                Dim response = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If response = DialogResult.Yes Then
                    DEMSurveyBindingSource.CancelEdit()
                Else
                    DialogResult = Nothing
                End If
            Else
                DEMSurveyBindingSource.CancelEdit()
            End If
        End Sub

        Private Sub txtName_TextChanged(sender As Object, e As System.EventArgs) Handles txtName.TextChanged

            If DirectCast(DEMSurveyBindingSource.Current, DataRowView).IsNew Then
                Dim sRasterPath As String = String.Empty
                If Not String.IsNullOrEmpty(txtName.Text) Then
                    If TypeOf m_ImportRasterform Is frmImportRaster Then
                        If TypeOf m_ImportRasterform.ucRaster.SelectedItem Is GISDataStructures.Raster Then
                            sRasterPath = GCD.GCDProject.ProjectManager.OutputManager.DEMSurveyRasterPath(txtName.Text)
                        End If
                    End If
                End If
                txtRasterPath.Text = sRasterPath
            End If
        End Sub

        Private Sub txtFolder_DoubleClick(sender As Object, e As System.EventArgs) Handles txtFolder.DoubleClick

            If Not String.IsNullOrEmpty(txtFolder.Text) Then
                If IO.Directory.Exists(txtFolder.Text) Then
                    Process.Start("explorer.exe", txtFolder.Text)
                End If
            End If

        End Sub

        Private Sub UpdateControls()

            cboSingle.Enabled = rdoSingle.Checked
            txtMask.Enabled = rdoMulti.Checked
            cboIdentify.Enabled = rdoMulti.Checked
            btnBrowseMask.Enabled = rdoMulti.Checked

        End Sub

        Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

            If ValidateForm() Then
                SaveDEMSurvey()
                Me.DialogResult = Windows.Forms.DialogResult.OK
            Else
                Me.DialogResult = Windows.Forms.DialogResult.None
            End If

        End Sub

        Private Sub SaveDEMSurvey()

            Dim theRow As DataRowView = DirectCast(DEMSurveyBindingSource.Current, DataRowView)
            Dim demRow As ProjectDS.DEMSurveyRow = theRow.Row
            demRow.SingleMethod = rdoSingle.Checked
            demRow.MultiMethod = rdoMulti.Checked
            demRow.MethodMask = txtMask.Text
            demRow.MethodMaskField = cboIdentify.Text
            demRow.SingleMethodType = cboSingle.Text
            demRow.Name = txtName.Text

            If m_SurveyDateTime.Year = 0 Then
                demRow.SetSurveyYearNull()
                demRow.SetSurveyMonthNull()
                demRow.SetSurveyDayNull()
            Else
                demRow.SurveyYear = m_SurveyDateTime.Year

                If m_SurveyDateTime.Month = 0 Then
                    demRow.SetSurveyMonthNull()
                    demRow.SetSurveyDayNull()
                Else
                    demRow.SurveyMonth = m_SurveyDateTime.Month

                    If m_SurveyDateTime.Day = 0 Then
                        demRow.SurveyDay = m_SurveyDateTime.Day
                    End If
                End If
            End If

            If m_SurveyDateTime.Hour = -1 Then
                demRow.SetSurveyHourNull()
                demRow.SetSurveyMinNull()
            Else
                demRow.SurveyHour = m_SurveyDateTime.Hour
                If m_SurveyDateTime.Minute = -1 Then
                    demRow.SetSurveyMinNull()
                Else
                    demRow.SurveyMin = m_SurveyDateTime.Minute
                End If
            End If

            DEMSurfaceBindingSource.EndEdit()
            ProjectManager.save()

            'TODO: call here to update the ProjectExplorer Tree
            'ProjectExplorerUC.LoadTree(ProjectExplorerUC.GCDNodeTypes.DEMSurvey.ToString)
        End Sub

        Private Sub btnBrowseFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddToMap.Click

            Dim CurrentRow As DataRowView = DEMSurfaceBindingSource.Current
            GCD.GCDProject.ProjectManagerUI.ArcMapManager.AddDEM(DEMSurveyBindingSource.Current.Row)

            'If GISDataStructures.Raster.Exists(txtRasterPath.Text) Then
            '    Dim sHillShade As String = GCD.GCDProject.ProjectManager.OutputManager.DEMSurveyHillShadeRasterPath(txtName.Text)
            '    If GISDataStructures.Raster.Exists(sHillShade) Then
            '        Dim gHillShade As New GISDataStructures.Raster(sHillShade)
            '        gHillShade.AddToMap(m_pArcMap, txtName.Text & " HS", txtName.Text)
            '    End If

            '    Dim gRasterGDAL As New GISDataStructures.RasterGDAL(txtRasterPath.Text)
            '    gRasterGDAL.AddToMap(m_pArcMap, txtDate.Name, txtName.Text)
            'End If

        End Sub

        Private Sub btnBrowseMask_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseMask.Click

            rdoMulti.Checked = True

            Try
                ' Do not initiate the browse window with the existing mask path because 
                ' it is inside the GCD project path.
                Dim sName As String = ""
                Dim sFolder As String = String.Empty

                ' browse and see if the user selects a new polygon feature class.
                Dim gNewMask As GISDataStructures.PolygonDataSource = GISDataStructures.VectorDataSource.BrowseOpen("DEM Survey Method Polygon Mask", sFolder, sName, GISDataStructures.BrowseGISTypes.Polygon, Me.Handle)
                If Not TypeOf gNewMask Is GISDataStructures.PolygonDataSource Then
                    Exit Sub
                End If

                ' Check if the user has browsed to the same mask. In which case do nothing except reload the fields
                ' (just in case they are doing this for a reason.
                Dim sTempPath As String = GCD.GCDProject.ProjectManager.GetRelativePath(gNewMask.FullPath)
                If String.Compare(sTempPath, txtMask.Text, True) = 0 Then
                    gNewMask.FillComboWithFields(cboIdentify, "Method", esriFieldType.esriFieldTypeString, True)
                    Exit Sub
                End If

                If TypeOf gNewMask Is GISDataStructures.PolygonDataSource Then

                    ' Check that the new mask has the same spatial reference as the DEM survey.
                    Dim dr As DataRowView = DEMSurveyBindingSource.Current
                    Dim demRow As ProjectDS.DEMSurveyRow = dr.Row
                    Dim gDEM As New GISDataStructures.RasterDirect(GCD.GCDProject.ProjectManager.GetAbsolutePath(demRow.Source))
                    If Not gDEM.CheckSpatialReferenceMatches(gNewMask.SpatialReference) Then
                        MsgBox("The spatial reference of the selected polygon feature class does not match that of the DEM survey raster (" & gDEM.SpatialReference.Name.ToString & ").", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                        Exit Sub
                    End If

                    ' Check that the new mask has at least one feature
                    If gNewMask.FeatureCount < 1 Then
                        MsgBox("The polygon feature class is empty and contains no features.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                        Exit Sub
                    End If

                    ' Check that the new mask has at least one string field
                    Dim bContainsAtLeastOneStringField As Boolean = False
                    Dim theFields As IFields = gNewMask.FeatureClass.Fields
                    For i As Integer = 0 To theFields.FieldCount - 1
                        If theFields.Field(i).Type = esriFieldType.esriFieldTypeString Then
                            bContainsAtLeastOneStringField = True
                            Exit For
                        End If
                    Next

                    If Not bContainsAtLeastOneStringField Then
                        MsgBox("The polygon feature class does not contain at least one field of type string, needed for capturing the survey method.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                        Exit Sub
                    End If

                    ' Attempt to delete any existing mask feature class
                    DeleteExistingMaskFeatureClass()

                    Dim sMethodMask As String = GCD.GCDProject.ProjectManager.OutputManager.DEMSurveyMethodMaskPath(txtName.Text)
                    Try
                        IO.Directory.CreateDirectory(GISDataStructures.VectorDataSource.GetWorkspacePath(sMethodMask))
                    Catch ex As Exception
                        Dim ex2 As New Exception("Error attempting to create the directory for the DEM Survey method mask.")
                        ex2.Data.Add("Method Mask Folder", sMethodMask)
                        ExceptionUI.HandleException(ex2)
                        Exit Sub
                    End Try

                    If GISDataStructures.PolygonDataSource.Exists(sMethodMask, esriGeometryPolygon) Then
                        Dim ex As New Exception("The polygon mask feature class already exists.")
                        ex.Data.Add("Original Mask", gNewMask.FullPath)
                        ex.Data.Add("GCD Project Mask", sMethodMask)
                        ExceptionUI.HandleException(ex)
                    Else
                        Try
                            ' GP.Copy doesn't work for file geodatabases.
                            Dim sSourcePath As String
                            If GISDataStructures.IsFileGeodatabase(gNewMask.FullPath) Then
                                sSourcePath = GP.Conversion.FeatureClassToShapeFile(gNewMask.FullPath, WorkspaceManager.WorkspacePath)
                            Else
                                sSourcePath = gNewMask.FullPath
                            End If
                            GP.DataManagement.Copy(sSourcePath, sMethodMask)

                            gNewMask = New GISDataStructures.PolygonDataSource(sMethodMask)

                            ' Poplulate the method mask dropdown with the string fields from the feature class
                            gNewMask.FillComboWithFields(cboIdentify, "Method", esriFieldType.esriFieldTypeString)
                            txtMask.Text = GCD.GCDProject.ProjectManager.GetRelativePath(sMethodMask)

                        Catch ex As Exception
                            Dim ex2 As New Exception("Error attempting to copy the method mask feature class into the GCD project folder.", ex)
                            ex2.Data.Add("Original Path", gNewMask.FullPath)
                            ex2.Data.Add("GCD Project Path", sMethodMask)
                            ExceptionUI.HandleException(ex2)
                            Exit Sub
                        End Try
                    End If

                End If

            Catch ex As Exception
                ExceptionUI.HandleException(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Delete any existing mask feature class if one exists.
        ''' </summary>
        ''' <remarks>Called from choosing a new feature class or from setting the survey type to single.</remarks>
        Private Sub DeleteExistingMaskFeatureClass()

            Dim sMaskPath As String = GCD.GCDProject.ProjectManager.GetAbsolutePath(txtMask.Text)
            If GISDataStructures.PolygonDataSource.Exists(sMaskPath) Then
                Dim sWorkspace As String = GISDataStructures.VectorDataSource.GetWorkspacePath(sMaskPath)
                Try
                    GP.DataManagement.Delete(sMaskPath)
                    txtMask.Text = String.Empty
                    cboIdentify.Items.Clear()
                Catch ex As Exception
                    ExceptionUI.HandleException(ex, "Error deleting the existing DEM survey method mask polygon feature class. You can attempt to delete it using ArcCatalog and then try again to browse to a new method mask.")
                    Exit Sub
                End Try

                If IO.Directory.Exists(sWorkspace) Then
                    Try
                        IO.Directory.Delete(sWorkspace)
                    Catch ex As Exception
                        ' do nothing
                    End Try
                End If

            End If

        End Sub

        Private Sub rdoSingle_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoSingle.CheckedChanged

            If rdoSingle.Checked Then
                DeleteExistingMaskFeatureClass()
            End If
            UpdateControls()
        End Sub

        Private Sub btnHlp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHlp.Click
            Process.Start(My.Resources.HelpBaseURL & "gcd-command-reference/gcd-project-explorer/d-dem-context-menu/i-edit-dem-survey-properties")
        End Sub

        Private Sub LoadRasterProperties()

            Dim sAbsolutePath As String = GCD.GCDProject.ProjectManager.GetAbsolutePath(txtRasterPath.Text)
            If Not GISDataStructures.Raster.Exists(sAbsolutePath) Then
                Exit Sub
            End If

            Dim gRaster As New GISDataStructures.RasterDirect(sAbsolutePath)
            Dim demRow As ProjectDS.DEMSurveyRow = DirectCast(DEMSurveyBindingSource.Current, DataRowView).Row

            Dim sRasterProperties As String = "-- GCD Raster Properties --"
            sRasterProperties &= vbNewLine & "Left: " & gRaster.Extent.Left.ToString("#,##0.#")
            sRasterProperties &= vbNewLine & "Top: " & gRaster.Extent.Top.ToString("#,##0.#")
            sRasterProperties &= vbNewLine & "Right: " & gRaster.Extent.Right.ToString("#,##0.#")
            sRasterProperties &= vbNewLine & "Bottom: " & gRaster.Extent.Bottom.ToString("#,##0.#")
            sRasterProperties &= vbNewLine
            sRasterProperties &= vbNewLine & "Cell size: " & Math.Round(gRaster.CellSize, CInt(GCD.GCDProject.ProjectManager.CurrentProject.Precision)).ToString
            sRasterProperties &= vbNewLine
            sRasterProperties &= vbNewLine & "Width: " & (gRaster.Extent.Right - gRaster.Extent.Left).ToString("#,##0.#")
            sRasterProperties &= vbNewLine & "Height: " & (gRaster.Extent.Top - gRaster.Extent.Bottom).ToString("#,##0.#")
            sRasterProperties &= vbNewLine
            sRasterProperties &= vbNewLine & "Rows: " & gRaster.Rows.ToString("#,##0")
            sRasterProperties &= vbNewLine & "Columns: " & gRaster.Columns.ToString("#,##0")
            sRasterProperties &= vbNewLine & vbNewLine
            sRasterProperties &= "-- Original Raster Properties --"
            sRasterProperties &= vbNewLine & "Left: " & demRow.OriginalExtentLeft.ToString '("#,##0.#")
            sRasterProperties &= vbNewLine & "Top: " & demRow.OriginalExtentTop.ToString '("#,##0.#")
            sRasterProperties &= vbNewLine & "Right: " & demRow.OriginalExtentRight.ToString '("#,##0.#")
            sRasterProperties &= vbNewLine & "Bottom: " & demRow.OriginalExtentBottom.ToString '("#,##0.#")
            sRasterProperties &= vbNewLine
            sRasterProperties &= vbNewLine & "Cell size: " & demRow.OriginalCellSize.ToString
            sRasterProperties &= vbNewLine
            sRasterProperties &= vbNewLine & "Path: " & demRow.OriginalSource
            sRasterProperties &= vbNewLine & "Computer: " & demRow.OriginalComputer
            sRasterProperties &= vbNewLine

            Try
                ' File Size
                If IO.File.Exists(sAbsolutePath) Then
                    Dim info2 As New IO.FileInfo(sAbsolutePath)
                    sRasterProperties &= vbNewLine & "Raster file size: " & (info2.Length / 1048576).ToString("#,##0.00") & "Mb"
                End If
            Catch ex As Exception
                ' Do nothing. 
            End Try

            txtProperties.Text = sRasterProperties

        End Sub

#End Region

#Region "Associated Surface Events"

        Private Sub ViewSurveySettings()

            Dim CurrentRow As DataRowView
            CurrentRow = DEMSurfaceBindingSource.Current

            If Not CurrentRow Is Nothing Then

                DEMSurveyBindingSource.EndEdit()
                ProjectManager.save()

                Dim CurrentSurfaceRow As DataRowView = DEMSurfaceBindingSource.Current
                If TypeOf CurrentSurfaceRow Is DataRowView AndAlso TypeOf CurrentSurfaceRow.Row Is ProjectDS.AssociatedSurfaceRow Then
                    Dim DEMSurfaceID As Integer = DirectCast(CurrentSurfaceRow.Row, ProjectDS.AssociatedSurfaceRow).AssociatedSurfaceID
                    Dim SurfaceForm As New frmAssocSurfaceProperties(m_pArcMap, m_DEMSurveyID, DEMSurfaceID)
                    SurfaceForm.ShowDialog()
                End If
            End If

        End Sub

        Private Sub btnAddToMap_Click(sender As System.Object, e As System.EventArgs) Handles btnAddToMap.Click

            Dim CurrentRow As DataRowView = DEMSurfaceBindingSource.Current

            If Not CurrentRow Is Nothing AndAlso TypeOf CurrentRow.Row Is ProjectDS.AssociatedSurfaceRow Then

                GCD.GCDProject.ProjectManagerUI.ArcMapManager.AddAssociatedSurface(CurrentRow.Row)

                'Dim row As ProjectDS.AssociatedSurfaceRow = CurrentRow.Row
                '
                ' Attempt to determine to retrieve the layer file for the 
                ' corresponding associated surface type.
                '
                'Dim sLayerFile As String = String.Empty
                'Dim eType As RasterLayerTypes
                'Dim bknowntype As Boolean = False
                'Dim sType As String = row.Type
                'If sType.ToLower.Contains("slope") Then
                '    eType = RasterLayerTypes.Slope
                '    bknowntype = True
                'ElseIf sType.ToLower.Contains("density") Then
                '    eType = RasterLayerTypes.PointDensity
                '    bknowntype = True
                'ElseIf sType.ToLower.Contains("error") Then
                '    eType = RasterLayerTypes.ErrorSurfaces
                '    bknowntype = True
                'ElseIf sType.ToLower.Contains("3d point quality") Then
                '    eType = RasterLayerTypes.PointQuality
                '    bknowntype = True
                'ElseIf sType.ToLower.Contains("roughness") Then
                '    eType = RasterLayerTypes.Roughness
                '    bknowntype = True
                'End If
                'If bknowntype Then
                '    sLayerFile = GetSymbologyLayerFile(eType)
                'End If
                'ArcMap.AddToMap(m_pArcMap, GetCurrentProjectName(m_pArcMap), ArcMap.GCDRasterType.Surface, row.Source, row.Name, txtName.Text, My.Settings.AddMapLayersIfAlreadyPresent, sLayerFile)
            Else
                Dim warnNothing = MessageBox.Show("You have not selected an associated surface to add to your Map's Table of Contents. An associated surface must exist and be selected to add it to your map.", "No Associated Surface Selected!", MessageBoxButtons.OK)
                btnSettingsAssociatedSurface.Enabled = False
                btnDeleteAssociatedSurface.Enabled = False
                btnAddToMap.Enabled = False
            End If
        End Sub

        Private Sub btnAddAssociatedSurface_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddAssociatedSurface.Click

            If ValidateForm() Then

                DEMSurveyBindingSource.EndEdit()
                ProjectManager.save()

                Dim SurfaceForm As New frmAssocSurfaceProperties(m_pArcMap, m_DEMSurveyID)
                SurfaceForm.ShowDialog()
            End If

        End Sub

        Private Sub Associated_DoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DEMSurfaceDataGridView.CellContentDoubleClick

            ViewSurveySettings()

        End Sub

        Private Sub Associated_CellContentEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DEMSurfaceDataGridView.CellEnter
            btnAddToMap.Enabled = True
            btnDeleteAssociatedSurface.Enabled = True
            btnSettingsAssociatedSurface.Enabled = True
        End Sub

        Private Sub Associated_CellContentLeave(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DEMSurfaceDataGridView.CellLeave
            btnAddToMap.Enabled = False
            btnDeleteAssociatedSurface.Enabled = False
            btnSettingsAssociatedSurface.Enabled = False
        End Sub

        Private Sub btnSettingsAssociatedSurface_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSettingsAssociatedSurface.Click

            Dim CurrentRow As DataRowView
            CurrentRow = DEMSurfaceBindingSource.Current

            If Not CurrentRow Is Nothing Then
                ViewSurveySettings()
            Else
                Dim warnNothing = MessageBox.Show("You have not selected an associated surface to view its settings. An associated surface must exist and be selected to view its properties.", "No Associated Surface Selected!", MessageBoxButtons.OK)
                btnSettingsAssociatedSurface.Enabled = False
                btnDeleteAssociatedSurface.Enabled = False
                btnAddToMap.Enabled = False
            End If

        End Sub

        Private Sub btnDeleteAssociatedSurface_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteAssociatedSurface.Click

            Dim CurrentRow As DataRowView = DEMSurfaceBindingSource.Current
            DeleteAssociatedSurface(DirectCast(CurrentRow.Row, ProjectDS.AssociatedSurfaceRow))

        End Sub

        Public Shared Sub DeleteAssociatedSurface(rAssoc As ProjectDS.AssociatedSurfaceRow)

            If Not TypeOf rAssoc Is ProjectDS.AssociatedSurfaceRow Then
                Exit Sub
            End If

            Dim response As MsgBoxResult = MsgBox("Are you sure you want to remove the selected associated surface from the GCD Project? This will also delete the raster associated with this surface.", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, My.Resources.ApplicationNameLong)
            If response = MsgBoxResult.Yes Then
                Dim sPath As String = GCD.GCDProject.ProjectManager.GetAbsolutePath(rAssoc.Source)
                Dim bContinue As Boolean = True

                Try
                    RemoveAuxillaryLayersFromMap(sPath)
                Catch ex As Exception
                    MsgBox("Error removing the associated surface from the ArcMap table of contents. Remove it manually and then try deleting the associated surface again.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                    bContinue = False
                End Try

                If bContinue Then
                    If GISDataStructures.Raster.Exists(GCD.GCDProject.ProjectManager.GetAbsolutePath(rAssoc.Source)) Then
                        Try
                            GP.DataManagement.Delete(sPath)
                        Catch ex As Exception
                            Dim ex2 As New Exception("Error deleting the associated surface raster file and directory.", ex)
                            ex2.Data.Add("Raster Path", sPath)
                            ExceptionUI.HandleException(ex2)
                            bContinue = False
                        End Try
                    End If
                End If

                Try
                    IO.Directory.Delete(GISDataStructures.Raster.GetWorkspacePath(GCD.GCDProject.ProjectManager.GetAbsolutePath(rAssoc.Source)))
                Catch ex As Exception
                    ' do nothing
                End Try

                If bContinue Then
                    Try
                        rAssoc.Delete()
                        ProjectManager.save()
                    Catch ex As Exception
                        ExceptionUI.HandleException(ex, "The raster file was deleted, but an error occurred removing the surface from the GCD project file. This will be fixed automatically by closing and opening ArcMap if Validate Project is selected from the options menu")
                    End Try
                End If

            End If
        End Sub

#End Region

#Region "Error Calculation Events"

        Private Sub btn_AddErrorSurface_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddError.Click

            ' save the DEM survey information
            If Not ValidateForm() Then
                Exit Sub
            End If

            Try
                Dim demRow As ProjectDS.DEMSurveyRow = GCD.GCDProject.ProjectManager.ds.DEMSurvey.FindByDEMSurveyID(m_DEMSurveyID)
                DEMSurveyBindingSource.EndEdit()
                SpecifyErrorSurface(m_pArcMap, demRow)
            Catch ex As Exception
                ExceptionUI.HandleException(ex)
            End Try

        End Sub

        Public Shared Function SpecifyErrorSurface(pArcMap As IApplication, rDEM As ProjectDS.DEMSurveyRow) As ProjectDS.ErrorSurfaceRow

            Dim rError As ProjectDS.ErrorSurfaceRow = Nothing
            Dim gDEM As New GISDataStructures.RasterDirect(GCD.GCDProject.ProjectManager.GetAbsolutePath(rDEM.Source))
            Dim frm As New frmImportRaster(pArcMap, gDEM, rDEM, frmImportRaster.ImportRasterPurposes.ErrorCalculation, "Error Surface")
            If frm.ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim gRaster As GISDataStructures.RasterDirect = Nothing
                Try
                    gRaster = frm.ProcessRaster
                Catch ex As Exception
                    Try
                        IO.Directory.Delete(GISDataStructures.Raster.GetWorkspacePath(frm.txtRasterPath.Text))
                    Catch ex2 As Exception
                        ' do nothing
                    End Try

                    ExceptionUI.HandleException(ex, "An error occurred attempting to import the error surface into the GCD project. No information has been saved to the GCD project file but you should check the GCD project folder to determine if any remains of the raster remain.")
                End Try

                If TypeOf gRaster Is GISDataStructures.RasterDirect Then
                    Try
                        Dim sRasterPath As String = GCD.GCDProject.ProjectManager.GetRelativePath(frm.txtRasterPath.Text)
                        rError = GCD.GCDProject.ProjectManager.ds.ErrorSurface.AddErrorSurfaceRow(frm.txtName.Text, "Imported Raster", sRasterPath, rDEM)
                        ProjectManager.save()

                        If My.Settings.AddOutputLayersToMap Then
                            GCD.GCDProject.ProjectManagerUI.ArcMapManager.AddErrSurface(rError)
                        End If
                    Catch ex As Exception
                        Dim bRasterExists As Boolean = True
                        Try
                            GP.DataManagement.Delete(frm.txtRasterPath.Text)
                            bRasterExists = False
                        Catch ex2 As Exception
                            bRasterExists = True
                        End Try

                        Dim sMsg As String = "Failed to save the error surface information to the GCD project file."
                        If bRasterExists Then
                            sMsg &= " The GCD project error surface raster still exists and should be deleted by hand."
                        Else
                            sMsg &= "The GCD project error surface raster was deleted."
                        End If

                        ExceptionUI.HandleException(ex, sMsg)
                    End Try
                End If
            End If

            Return rError
        End Function

        Private Sub btnCalculateError_Click(sender As System.Object, e As System.EventArgs) Handles btnCalculateError.Click
            '
            ' Only open the Error calculation form if the survey properties save successfully.
            '
            If ValidateForm() Then
                SaveDEMSurvey()
                Dim dr As DataRowView = DEMSurveyBindingSource.Current
                Dim frm As New frmErrorCalculation2(m_pArcMap, DirectCast(dr.Row, ProjectDS.DEMSurveyRow))
                frm.ShowDialog()
            End If

        End Sub

        Private Sub Error_CellContentEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ErrorTableDataGridView.CellEnter
            btnAddErrorToMap.Enabled = True
            btnErrorDelete.Enabled = True
            btnErrorSettings.Enabled = True
        End Sub

        Private Sub Error_CellContentLeave(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ErrorTableDataGridView.CellLeave
            btnAddErrorToMap.Enabled = False
            btnErrorDelete.Enabled = False
            btnErrorSettings.Enabled = False
        End Sub

        Private Sub Error_DoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles ErrorTableDataGridView.CellContentDoubleClick
            ViewErrorSettings()
        End Sub

        Private Sub btnErrorSurfaceSettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnErrorSettings.Click
            ViewErrorSettings()
        End Sub

        Private Sub btnDeleteErrorSurface_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnErrorDelete.Click

            Dim CurrentRow As DataRowView = ErrorTableBindingSource.Current
            DeleteErrorSurface(CurrentRow.Row)

        End Sub

        Public Shared Sub DeleteErrorSurface(rError As ProjectDS.ErrorSurfaceRow)

            If Not TypeOf rError Is ProjectDS.ErrorSurfaceRow Then
                Exit Sub
            End If

            Dim response As MsgBoxResult = MsgBox("Are you sure you want to delete the selected error surface from the GCD project? This will also delete the raster associated with this error surface.", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, My.Resources.ApplicationNameLong)
            If response = MsgBoxResult.Yes Then
                Dim sPath As String = GCD.GCDProject.ProjectManager.GetAbsolutePath(rError.Source.ToString)
                Dim bContinue As Boolean = True

                Try
                    RemoveAuxillaryLayersFromMap(sPath)
                Catch ex As Exception
                    MsgBox("Error removing the error surface from the ArcMap table of contents. Remove it manually and then try deleting the error surface again.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                    bContinue = False
                End Try

                If bContinue Then
                    If Not rError.IsSourceNull Then
                        If GISDataStructures.Raster.Exists(GCD.GCDProject.ProjectManager.GetAbsolutePath(rError.Source)) Then
                            Try
                                GP.DataManagement.Delete(sPath)
                            Catch ex As Exception
                                ExceptionUI.HandleException(ex, "Failed to delete the error surface raster.")
                                bContinue = False
                            End Try
                        End If
                    End If
                End If

                If bContinue Then
                    Try
                        IO.Directory.Delete(GISDataStructures.Raster.GetWorkspacePath(GCD.GCDProject.ProjectManager.GetAbsolutePath(rError.Source)))
                    Catch ex As Exception
                        ' do nothing
                    End Try
                End If

                If bContinue Then
                    Try
                        rError.Delete()
                        ProjectManager.save()
                    Catch ex As Exception
                        ExceptionUI.HandleException(ex, "The raster file was deleted, but an error occurred removing the surface from the GCD project file. This will be fixed automatically by closing and opening ArcMap if Validate Project is selected from the options menu")
                    End Try
                End If

            End If

        End Sub

        Private Sub btnAddErrorToMap_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddErrorToMap.Click

            Dim CurrentRow As DataRowView = Me.ErrorTableBindingSource.Current
            If Not CurrentRow Is Nothing AndAlso TypeOf CurrentRow.Row Is ProjectDS.ErrorSurfaceRow Then
                GCD.GCDProject.ProjectManagerUI.ArcMapManager.AddErrSurface(CurrentRow.Row)
            Else
                Dim warnNothing = MessageBox.Show("You have not selected an error surface to add to your map. An error surface must exist and be selected to be added to the map.", "No Error Surface Selected!", MessageBoxButtons.OK)
                btnErrorSettings.Enabled = False
                btnErrorDelete.Enabled = False
                btnAddErrorToMap.Enabled = False
            End If

        End Sub

        Private Sub ViewErrorSettings()

            Dim CurrentRow As DataRowView = Me.ErrorTableBindingSource.Current
            If Not CurrentRow Is Nothing AndAlso TypeOf CurrentRow.Row Is ProjectDS.ErrorSurfaceRow Then
                Dim frm As New frmErrorCalculation2(m_pArcMap, DirectCast(CurrentRow.Row, ProjectDS.ErrorSurfaceRow))
                frm.ShowDialog()
            End If

        End Sub

#End Region

        Private Sub btnHelp_Click(sender As Object, e As System.EventArgs)

        End Sub

        Private Sub cmdDateTime_Click(sender As System.Object, e As System.EventArgs) Handles cmdDateTime.Click

            Dim frm As New frmSurveyDateTime()
            frm.SurveyDateTime = m_SurveyDateTime
            If frm.ShowDialog = System.Windows.Forms.DialogResult.OK Then
                m_SurveyDateTime = frm.SurveyDateTime
                lblDatetime.Text = m_SurveyDateTime.ToString
            End If
        End Sub
    End Class

End Namespace