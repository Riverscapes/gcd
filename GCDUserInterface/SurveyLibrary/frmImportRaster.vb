Imports System.Windows.Forms
Imports System.ComponentModel
Imports GCDCore.Project

Namespace SurveyLibrary

    Public Class frmImportRaster

        Public Enum ImportRasterPurposes
            DEMSurvey
            AssociatedSurface
            ErrorCalculation
            StandaloneTool
        End Enum

        Private Enum RoundingDirection
            Up
            Down
        End Enum

        Private Enum ResamplingMethods
            Bilinear
            Cubic
            NaturalNeighbours
            NearestNeighbour
            None
        End Enum

        Private m_gReferenceRaster As GCDConsoleLib.Raster
        Private m_ePurpose As ImportRasterPurposes
        Private m_DEM As DEMSurvey
        Private m_OriginalExtent As GCDConsoleLib.ExtentRectangle
        Private m_sRasterMetaData As String ' not populated until the action of importing.

        Private m_nNoInterpolationIndex As Integer

        ''' <summary>
        ''' Dictionary of non-GDAL compliant rasters to their GDAL compliant pairs.
        ''' </summary>
        ''' <remarks>This form requires the use of GDAL compliant rasters. i.e. the
        ''' original raster cannot be in a file geodatabase. Each time the user browses
        ''' to or picks a new raster it needs to be copied to a GDAL compliant format
        ''' if it is not already. This can be time consuming. So this dictionary
        ''' keeps track of any non-GDAL compliant raster paths and the GDAL compliant
        ''' copy. That way the user can change raster selection quickly without a lag
        ''' as rasters are copied repeatedly. It also keeps the number of temp rasters
        ''' down.
        ''' </remarks>
        Private m_RasterDirects As Dictionary(Of String, String)

        Public ReadOnly Property OriginalExtent As GCDConsoleLib.ExtentRectangle
            Get
                Return m_OriginalExtent
            End Get
        End Property

        Public ReadOnly Property RasterMetaData As String
            Get
                Return m_sRasterMetaData
            End Get
        End Property

        Public ReadOnly Property StringFormat As String
            Get
                Dim sResult As String = "#,##0"
                If TypeOf valPrecision Is System.Windows.Forms.NumericUpDown Then
                    For i As Integer = 1 To CInt(valPrecision.Value)
                        If i = 1 Then
                            sResult &= "."
                        End If
                        sResult &= "0"
                    Next
                End If
                Return sResult
            End Get
        End Property

        Public Sub New(gReferenceRaster As GCDConsoleLib.Raster, referenceDEM As DEMSurvey, ePurpose As ImportRasterPurposes, sNoun As String)

            ' This call is required by the designer.
            InitializeComponent()
            m_gReferenceRaster = gReferenceRaster
            m_ePurpose = ePurpose
            m_DEM = referenceDEM
            m_RasterDirects = New Dictionary(Of String, String)
            ucRaster.Noun = sNoun
        End Sub

        ''' <summary>
        ''' Launch the raster import in standalone mode for cleaning rasters
        ''' </summary>
        Public Sub New()

            ' This call is required by the designer.
            InitializeComponent()
            m_gReferenceRaster = Nothing
            m_ePurpose = ImportRasterPurposes.StandaloneTool
            m_RasterDirects = New Dictionary(Of String, String)

        End Sub

        Private Sub ImportRasterForm_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            SetupToolTips()

            cboMethod.Items.Add(New naru.db.NamedObject(ResamplingMethods.Bilinear, "Bilinear Interpolation"))
            cboMethod.Items.Add(New naru.db.NamedObject(ResamplingMethods.Cubic, "Cubic Convolution"))
            cboMethod.Items.Add(New naru.db.NamedObject(ResamplingMethods.NaturalNeighbours, "Natural Neighbours"))
            cboMethod.Items.Add(New naru.db.NamedObject(ResamplingMethods.NearestNeighbour, "Nearest Neighbour"))
            m_nNoInterpolationIndex = cboMethod.Items.Add(New naru.db.NamedObject(ResamplingMethods.None, "None (straight cell-wise copy)"))
            cboMethod.SelectedIndex = 0

            valCellSize.Minimum = 0.01
            valCellSize.Maximum = 1000 ' This needs to be changed to a larger value or else rasters with cell sizes greater than 1 will cause an error to be thrown. Perhaps 1000 is more appropriate?
            valCellSize.Value = 1

            If m_ePurpose <> ImportRasterPurposes.StandaloneTool AndAlso TypeOf ProjectManager.Project Is GCDCore.Project.GCDProject Then
                valPrecision.Value = 0 ' ProjectManager.Project.Precision
            End If

            valTop.ReadOnly = Not (m_ePurpose = ImportRasterPurposes.DEMSurvey OrElse m_ePurpose = ImportRasterPurposes.StandaloneTool)
            valLeft.ReadOnly = valTop.ReadOnly
            valBottom.ReadOnly = valTop.ReadOnly
            valRight.ReadOnly = valTop.ReadOnly
            valCellSize.ReadOnly = valTop.ReadOnly

            valTop.Enabled = m_ePurpose = ImportRasterPurposes.DEMSurvey OrElse m_ePurpose = ImportRasterPurposes.StandaloneTool
            valLeft.Enabled = valTop.Enabled
            valBottom.Enabled = valTop.Enabled
            valRight.Enabled = valTop.Enabled
            valCellSize.Enabled = valTop.Enabled

            valTop.Minimum = Decimal.MinValue
            valLeft.Minimum = Decimal.MinValue
            valBottom.Minimum = Decimal.MinValue
            valRight.Minimum = Decimal.MinValue

            valTop.Maximum = Decimal.MaxValue
            valLeft.Maximum = Decimal.MaxValue
            valBottom.Maximum = Decimal.MaxValue
            valRight.Maximum = Decimal.MaxValue

            valTop.ThousandsSeparator = True
            valLeft.ThousandsSeparator = True
            valBottom.ThousandsSeparator = True
            valRight.ThousandsSeparator = True

            txtLeft.BackColor = Me.BackColor
            txtTop.BackColor = txtLeft.BackColor
            txtBottom.BackColor = txtTop.BackColor
            txtRight.BackColor = txtTop.BackColor

            ' need to clear the original raster text box. User may have canceled the form
            ' with a selected raster (e.g. because spatial resolution doesn't match. This
            ' form persists on the parent form and then is shown again. 
            txtLeft.Text = String.Empty
            txtTop.Text = String.Empty
            txtBottom.Text = String.Empty
            txtRight.Text = String.Empty
            txtOrigCellSize.Text = String.Empty
            txtOrigRows.Text = String.Empty
            txtOrigCols.Text = String.Empty
            txtOrigHeight.Text = String.Empty
            txtOrigWidth.Text = String.Empty

            valPrecision.Enabled = m_ePurpose = ImportRasterPurposes.StandaloneTool
            If m_ePurpose = ImportRasterPurposes.StandaloneTool Then
                'lblPrecision.Text = "Precision:"
                Me.Text = "Clean Raster"
                grpProjectRaaster.Text = "Clean Raster"
                cmdOK.Text = "Create Clean Raster"
                lblRasterPath.Text = "Output path"
                lblName.Visible = False
                txtName.Visible = False

                ' Shortern the form when in standalone mode because there in no raster name needed.
                Dim fFormHeightOffset = (grpOriginalRaster.Top - lblName.Top)
                grpProjectRaaster.Top = grpProjectRaaster.Top - fFormHeightOffset
                Me.Height = Me.Height - fFormHeightOffset
                grpOriginalRaster.Top = txtName.Top
            Else
                Me.Text = "Specify GCD " & ucRaster.Noun
                cmdOK.Text = "Import Raster"
                grpProjectRaaster.Text = "GCD " & ucRaster.Noun
                txtRasterPath.Width = txtName.Width
                cmdSave.Visible = False

                If m_ePurpose = ImportRasterPurposes.DEMSurvey Then
                    If TypeOf m_gReferenceRaster Is GCDConsoleLib.Raster Then
                        ' there is already at least one DEM in the project. Disable cell size.
                        valCellSize.Enabled = False
                    Else
                        ' This is the first DEM survey. Let the user adjust the precision.
                        valPrecision.Enabled = True
                    End If
                End If
            End If
            OriginalRasterChanged()
            AddHandler ucRaster.PathChanged, AddressOf OnRasterChanged
        End Sub

        Private Sub SetupToolTips()

        End Sub

        Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click

            If Not ValidateForm() Then
                Me.DialogResult = DialogResult.None
                Exit Sub
            End If

            'ucRaster.PreSelect(ucRaster.SelectedItem)

            'Add Rasters if standalone tool
            'If m_ePurpose = ImportRasterPurposes.StandaloneTool And My.Settings.AddOutputLayersToMap = True Then
            ' TODO 
            Throw New NotImplementedException("not implemented")
            'ucRaster.AddToMap()
            'End If

        End Sub

        Private Function ValidateForm() As Boolean
            'this is a demo comment

            If String.IsNullOrEmpty(txtName.Text) Then
                MsgBox("The raster name cannot be empty.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                Return False
            End If

            If TypeOf ucRaster.SelectedItem Is GCDConsoleLib.Raster Then
                Dim r As GCDConsoleLib.Raster = ucRaster.SelectedItem

                Throw New NotImplementedException("Decide how to handle rasters that don't have a no data value")
                'If Not r.HasNoDataValue Then
                '    MsgBox("The raster is missing a NoDataValue. You  must set the NoDataValue before you can use this raster with GCD.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                '    Return False
                'End If

                Dim bMissingSpatialReference As Boolean = True
                Throw New NotImplementedException
                'If TypeOf ucRaster.SelectedItem.SpatialReference Is ESRI.ArcGIS.Geometry.ISpatialReference Then
                bMissingSpatialReference = ucRaster.SelectedItem.Proj.PrettyWkt.ToLower.Contains("unknown")
                ' End If

                If bMissingSpatialReference Then
                    MsgBox("The selected raster appears to be missing a spatial reference. All GCD rasters must possess a spatial reference and it must be the same spatial reference for all rasters in a GCD project." &
                            " If the selected raster exists in the same coordinate system, " & m_gReferenceRaster.Proj.PrettyWkt & ", but the coordinate system has not yet been defined for the raster." &
                            " Use the ArcToolbox 'Define Projection' geoprocessing tool in the 'Data Management -> Projection & Transformations' Toolbox to correct the problem with the selected raster by defining the coordinate system as:" & vbCrLf & vbCrLf & m_gReferenceRaster.Proj.PrettyWkt & vbCrLf & vbCrLf & "Then try importing it into the GCD again.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                    Return False
                Else
                    If TypeOf m_gReferenceRaster Is GCDConsoleLib.Raster Then
                        If Not m_gReferenceRaster.Proj.IsSame(ucRaster.SelectedItem.Proj) Then
                            MsgBox("The coordinate system of the selected raster:" & vbCrLf & vbCrLf & ucRaster.SelectedItem.Proj.PrettyWkt & vbCrLf & vbCrLf & "does not match that of the GCD project:" & vbCrLf & vbCrLf & m_gReferenceRaster.Proj.PrettyWkt & "." & vbCrLf & vbCrLf &
                               "All rasters within a GCD project must have the identical coordinate system. However, small discrepencies in coordinate system names might cause the two coordinate systems to appear different. " &
                               "If you believe that the selected raster does in fact possess the same coordinate system as the GCD project then use the ArcToolbox 'Define Projection' geoprocessing tool in the " &
                               "'Data Management -> Projection & Transformations' Toolbox to correct the problem with the selected raster by defining the coordinate system as:" & vbCrLf & vbCrLf & m_gReferenceRaster.Proj.PrettyWkt & vbCrLf & vbCrLf & "Then try importing it into the GCD again.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                            Return False
                        End If
                    End If
                End If
            Else
                MsgBox("You must select a raster to import. Use the browse button if the raster you want is not already in the map and dropdown list.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                Return False
            End If

            If String.IsNullOrEmpty(txtRasterPath.Text) Then
                If m_ePurpose = ImportRasterPurposes.StandaloneTool Then
                    MsgBox("The output raster path cannot be empty. Click the Save button to specify an output raster path.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                Else
                    MsgBox("The " & ucRaster.Noun & " path cannot be empty. Try using a different name.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                End If
                Return False
            Else
                If System.IO.File.Exists(txtRasterPath.Text) Then
                    MsgBox("The project raster path already exists. Try using a different name for the raster.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                    Return False
                Else
                    Dim sExtension As String = IO.Path.GetExtension(txtRasterPath.Text)
                    If String.Compare(sExtension, ".tif", True) <> 0 Then
                        MsgBox("This tool can only currently produce GeoTIFF rasters. Please provide an output raster path ending with '.tif'", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                        Return False
                    End If
                End If
            End If

            If valCellSize.Value <= 0 Then
                MsgBox("The cell size must be greater than or equal to zero.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                Return False
            End If

            If (valRight.Value - valLeft.Value) < valCellSize.Value Then
                MsgBox("The right edge of the extent must be at least one cell width more than the left edge of the extent.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                Return False
            End If

            If valTop.Value - valBottom.Value < valCellSize.Value Then
                MsgBox("The top edge of the extent must be at least one cell width more than the bottom edge of the extent.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                Return False
            End If

            If TypeOf cboMethod.SelectedItem Is naru.db.NamedObject Then
                Dim lItem As naru.db.NamedObject = DirectCast(cboMethod.SelectedItem(), naru.db.NamedObject)
                Dim eType As ResamplingMethods = lItem.ID
                If RequiresResampling() Then
                    If eType <> ResamplingMethods.Bilinear Then
                        If eType = ResamplingMethods.None Then
                            MsgBox("The input raster requires resampling. Please select the desired resampling method.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                        Else
                            MsgBox("Only bilinear interpolation is currently functional within the GCD.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                        End If
                        Return False
                    End If
                Else
                    If cboMethod.SelectedIndex <> m_nNoInterpolationIndex Then
                        MsgBox("The raster is orthogonal and divisible with the specified output. No interpolation is required. Select ""None"" in the interpolation method drop down.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                        Return False
                    End If
                End If
            End If

            If (m_ePurpose = ImportRasterPurposes.DEMSurvey AndAlso valPrecision.Enabled = False) Or (m_ePurpose = ImportRasterPurposes.AssociatedSurface) Or (m_ePurpose = ImportRasterPurposes.ErrorCalculation) Then
                ' If the project units have not yet been written to 
                Throw New NotImplementedException("Are we still doing this, or are we requiring the units to be set on the project already?")
                'If Not ProjectManager.CurrentProject.DisplayUnits Is Nothing Then
                '    If TypeOf ucRaster.SelectedItem Is GCDConsoleLib.Raster Then
                '        If ucRaster.SelectedItem.VerticalUnits <> ProjectManager.CurrentProject.DisplayUnits Then
                '            MsgBox(String.Format("The linear units of the selected raster {0} does not match the linear units {1} of the GCD Project." & vbCrLf & vbCrLf & "Please select a raster that has the same linear units as the GCD Project.",
                '                             ucRaster.SelectedItem.VerticalUnits, ProjectManager.CurrentProject.DisplayUnits), MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                '            Return False
                '        End If
                '    End If
                'End If
            End If

            Return True

        End Function

        Private Sub OnRasterChanged(ByVal sender As Object, ByVal e As naru.ui.PathEventArgs) Handles ucRaster.PathChanged

            Try
                OriginalRasterChanged()
            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try

        End Sub

        Private Sub OriginalRasterChanged()
            '
            ' There is no reference raster, or we are in DEM survey mode. So determine the
            ' orthogonal extent of the selected raster. Convert it to a GDAL raster first
            ' (if its not already) then "orthogonalize" it's extent.
            '
            If TypeOf ucRaster.SelectedItem Is GCDConsoleLib.Raster Then
                Dim gOriginalRaster As GCDConsoleLib.Raster = Nothing
                If GetSafeOriginalRaster(gOriginalRaster) Then

                    If valPrecision.Enabled Then
                        '
                        ' Try to determine the appropriate precision from the input raster.
                        ' Keep increasing the original cell resolution by powers of ten until it
                        ' is a whole number. This is the appropriate "initial" precision for the
                        ' output until the user overrides it.
                        '
                        Dim nPrecision As Integer = 1
                        Dim fCellSize As Double = gOriginalRaster.Extent.CellWidth
                        For i = 0 To 10
                            Dim fTest As Double = fCellSize * Math.Pow(10, i)
                            fTest = Math.Round(fTest, 4)
                            If fTest Mod 1 = 0 Then
                                valPrecision.Value = i
                                Exit For
                            End If
                        Next
                    End If

                    m_OriginalExtent = gOriginalRaster.Extent

                    txtTop.Text = gOriginalRaster.Extent.Top.ToString
                    txtLeft.Text = gOriginalRaster.Extent.Left.ToString
                    txtBottom.Text = gOriginalRaster.Extent.Bottom.ToString
                    txtRight.Text = gOriginalRaster.Extent.Right.ToString

                    txtOrigRows.Text = gOriginalRaster.Extent.rows.ToString("#,##0")
                    txtOrigCols.Text = gOriginalRaster.Extent.cols.ToString("#,##0")
                    txtOrigWidth.Text = (gOriginalRaster.Extent.Right - gOriginalRaster.Extent.Left).ToString
                    txtOrigHeight.Text = (gOriginalRaster.Extent.Top - gOriginalRaster.Extent.Bottom).ToString
                    txtOrigCellSize.Text = gOriginalRaster.Extent.CellWidth.ToString
                    UpdateOriginalRasterExtentFormatting()

                    If Not (TypeOf m_gReferenceRaster Is GCDConsoleLib.Raster AndAlso m_ePurpose <> ImportRasterPurposes.DEMSurvey) Then
                        valCellSize.Value = Math.Max(Math.Round(gOriginalRaster.Extent.CellWidth, valCellSize.DecimalPlaces), valCellSize.Minimum)
                        If valPrecision.Value < 1 Then
                            valCellSize.Value = Math.Max(valCellSize.Value, 1)
                        End If
                    End If

                    If String.IsNullOrEmpty(txtName.Text) Then
                        txtName.Text = IO.Path.GetFileNameWithoutExtension(ucRaster.SelectedItem.GISFileInfo.FullName)
                    Else
                        UpdateRasterPath()
                    End If
                End If

            End If

            '
            ' Only use the reference raster for the orthogonal extent when in associated
            ' surface or error surface mode. When in DEM Survey mode, the reference raster
            ' is just for matching spatial reference.

            If TypeOf m_gReferenceRaster Is GCDConsoleLib.Raster AndAlso (m_ePurpose <> ImportRasterPurposes.DEMSurvey OrElse m_ePurpose = ImportRasterPurposes.StandaloneTool) Then
                Dim fCellSize As Decimal = Math.Round(m_gReferenceRaster.Extent.CellWidth, valCellSize.DecimalPlaces)
                valCellSize.Maximum = fCellSize
                valCellSize.Value = fCellSize

                valTop.Maximum = m_gReferenceRaster.Extent.Top
                valTop.Value = m_gReferenceRaster.Extent.Top

                valLeft.Maximum = m_gReferenceRaster.Extent.Left
                valLeft.Value = m_gReferenceRaster.Extent.Left

                valBottom.Maximum = m_gReferenceRaster.Extent.Bottom
                valBottom.Value = m_gReferenceRaster.Extent.Bottom

                valRight.Maximum = m_gReferenceRaster.Extent.Right
                valRight.Value = m_gReferenceRaster.Extent.Right

                ' PGB - 24 Apr 2015 - When in associated surface mode we need to update the method
                ' dropdown to reflect where the raster being imported can be copied or is resampled.
                RequiresResampling()

                'This case deals with when using the Standalone tool and switching between rasters in combobox need mechanism to update to current raster
                'Case also deals with when a GCD project is started and no raster has been added yet or in map, i.e. no reference raster and raster is added through browsing
            ElseIf m_gReferenceRaster Is Nothing AndAlso (m_ePurpose = ImportRasterPurposes.StandaloneTool OrElse m_ePurpose = ImportRasterPurposes.DEMSurvey) Then
                UpdateOutputExtent()

                'This case deals with when importing a raster and switching between rasters in combobox need mechanism to update to current raster
            ElseIf TypeOf m_gReferenceRaster Is GCDConsoleLib.Raster AndAlso (m_ePurpose = ImportRasterPurposes.DEMSurvey OrElse m_ePurpose = ImportRasterPurposes.StandaloneTool) Then
                UpdateOutputExtent()
            Else
                RequiresResampling()
            End If

            Dim sFormat As String = "#,##0"
            If valCellSize.DecimalPlaces > 0 Then
                sFormat &= "."
                For i As Integer = 0 To valCellSize.DecimalPlaces - 1
                    sFormat &= "0"
                Next
            End If

        End Sub

        ''' <summary>
        ''' Get a GDAL raster for the selected raster in the dropdown list.
        ''' </summary>
        ''' <param name="gRasterDirect">Output GDAL raster for the selected item in the raster combo box.</param>
        ''' <remarks>>This form requires the use of GDAL compliant rasters. i.e. the
        ''' original raster cannot be in a file geodatabase. Each time the user browses
        ''' to or picks a new raster it needs to be copied to a GDAL compliant format
        ''' if it is not already. This can be time consuming. So this dictionary
        ''' keeps track of any non-GDAL compliant raster paths and the GDAL compliant
        ''' copy. That way the user can change raster selection quickly without a lag
        ''' as rasters are copied repeatedly. It also keeps the number of temp rasters
        ''' down.</remarks>
        Private Function GetSafeOriginalRaster(ByRef gRasterDirect As GCDConsoleLib.Raster) As Boolean

            gRasterDirect = Nothing
            If TypeOf ucRaster.SelectedItem Is GCDConsoleLib.Raster Then
                gRasterDirect = New GCDConsoleLib.Raster(ucRaster.SelectedItem.GISFileInfo)
            End If

            Return TypeOf gRasterDirect Is GCDConsoleLib.Raster

        End Function

        Private Sub UpdateRasterPath()

            Try
                ' Standalone tool browses to the output, and does not derive it from original raster.
                If m_ePurpose = ImportRasterPurposes.StandaloneTool Then
                    Exit Sub
                End If

                Dim sRasterPath As IO.FileInfo
                If Not String.IsNullOrEmpty(txtName.Text) Then
                    If TypeOf ucRaster.SelectedItem Is GCDConsoleLib.Raster Then
                        ' Get the appropriate raster path depending on the purpose of this window (DEM, associated surface, error surface)

                        Select Case m_ePurpose
                            Case ImportRasterPurposes.DEMSurvey
                                sRasterPath = ProjectManager.OutputManager.DEMSurveyRasterPath(txtName.Text)

                            Case ImportRasterPurposes.AssociatedSurface
                                sRasterPath = ProjectManager.OutputManager.AssociatedSurfaceRasterPath(m_DEM.Name, txtName.Text)

                            Case ImportRasterPurposes.ErrorCalculation
                                sRasterPath = ProjectManager.OutputManager.ErrorSurfaceRasterPath(m_DEM.Name, txtName.Text)

                            Case Else
                                MsgBox("Unhandled import raster purpose: " & m_ePurpose.ToString, MsgBoxStyle.Exclamation, GCDCore.Properties.Resources.ApplicationNameLong)
                        End Select
                    End If
                End If
                txtRasterPath.Text = sRasterPath.FullName
            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try
        End Sub

        Public Function ProcessRaster() As GCDConsoleLib.Raster

            Dim gResult As GCDConsoleLib.Raster = Nothing
            If Not String.IsNullOrEmpty(txtRasterPath.Text) Then
                If IO.File.Exists(txtRasterPath.Text) Then
                    Dim ex As New Exception("The raster path already exists.")
                    ex.Data.Add("Raster path", txtRasterPath.Text)
                    Throw ex
                Else
                    Dim gRaster As GCDConsoleLib.Raster = Nothing
                    If GetSafeOriginalRaster(gRaster) Then

                        Dim sWorkspace As String = System.IO.Path.GetDirectoryName(txtRasterPath.Text)
                        Dim theDir As IO.DirectoryInfo = IO.Directory.CreateDirectory(sWorkspace)
                        If theDir.Exists Then

                            Dim nCols As Integer = CInt(txtProjCols.Text.Replace(",", ""))
                            Dim nRows As Integer = CInt(txtProjRows.Text.Replace(",", ""))

                            Dim outputExtent As New GCDConsoleLib.ExtentRectangle(valTop.Value, valLeft.Value, valCellSize.Value, valCellSize.Value, nRows, nCols)

                            If RequiresResampling() Then
                                GCDConsoleLib.RasterOperators.BilinearResample(gRaster, New IO.FileInfo(txtRasterPath.Text), outputExtent)
                                Debug.WriteLine("Bilinear resample:" & outputExtent.ToString)
                            Else
                                GCDConsoleLib.RasterOperators.ExtendedCopy(gRaster, New IO.FileInfo(txtRasterPath.Text), outputExtent)
                                Debug.WriteLine("Copying raster:" & outputExtent.ToString)
                            End If


                            ' This method will check to see if pyrmaids are need and then build if necessary.
                            PerformRasterPyramids(m_ePurpose, New IO.FileInfo(txtRasterPath.Text))

                            ' Save the precision and the linear unit of the raster back to the GCD project
                            If m_ePurpose = ImportRasterPurposes.DEMSurvey AndAlso valPrecision.Enabled Then

                                Throw New NotImplementedException("Are we still doing this or relying on project to have these things already?")
                                'Try
                                '    'If the project units have not yet been written to 
                                '    If ProjectManager.CurrentProject.DisplayUnits Is Nothing Then
                                '        ProjectManager.CurrentProject.DisplayUnits = gRaster.VerticalUnits
                                '    End If

                                '    'If the coordinate system has not yet been written to 
                                '    If ProjectManager.CurrentProject.CoordinateSystem Is Nothing Then
                                '        Dim sCoordinateSystem As String = gRaster.Proj.Wkt
                                '        ProjectManager.CurrentProject.CoordinateSystem = sCoordinateSystem
                                '    End If

                                '    ProjectManager.CurrentProject.Precision = CInt(valPrecision.Value)
                                '    ProjectManager.save()
                                'Catch ex As Exception
                                '    MsgBox("Failed to save the new precision to the GCD project.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                                'End Try


                                If m_ePurpose = ImportRasterPurposes.DEMSurvey Then
                                    ' Now try the hillshade for DEM Surveys
                                    Dim sHillshadePath As IO.FileInfo = ProjectManager.OutputManager.DEMSurveyHillShadeRasterPath(txtName.Text)
                                    GCDConsoleLib.RasterOperators.Hillshade(gResult, sHillshadePath)
                                    ProjectManager.PyramidManager.PerformRasterPyramids(GCDCore.RasterPyramidManager.PyramidRasterTypes.Hillshade, sHillshadePath)
                                End If
                            End If
                        Else
                            Dim ex As New Exception("Failed to create raster workspace folder")
                            ex.Data.Add("Raster Path", txtRasterPath.Text)
                            ex.Data.Add("Workspace", sWorkspace)
                            Throw ex
                        End If
                    End If
                End If
            End If

            Return gResult

        End Function

        Private Sub txtName_TextChanged(sender As Object, e As System.EventArgs) Handles txtName.TextChanged
            UpdateRasterPath()
        End Sub

        Private Sub valBuffeer_ValueChanged(sender As Object, e As System.EventArgs)
            OriginalRasterChanged()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>Note that this can be triggered either by the user changing the value or by 
        ''' the code setting the value.</remarks>
        Private Sub valLeft_ValueChanged(sender As Object, e As System.EventArgs) Handles _
        valLeft.ValueChanged, valTop.ValueChanged,
        valRight.ValueChanged, valBottom.ValueChanged

            UpdateOutputRowsColsHeightWidth()

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>Note that changing the cell size requires that the extent be changed. This 
        ''' in turn will trigger the updating of the rows/cols and width/height.</remarks>
        Private Sub valCellSize_ValueChanged(sender As Object, e As System.EventArgs) Handles valCellSize.ValueChanged

            'valCellSize.Value = Math.Round(valCellSize.Value, CInt(valPrecision.Value))

            UpdateOutputExtent()
            UpdateOriginalRasterExtentFormatting()

            valLeft.Increment = valCellSize.Value
            valTop.Increment = valCellSize.Value
            valRight.Increment = valCellSize.Value
            valBottom.Increment = valCellSize.Value
        End Sub

        Private Sub UpdateOutputExtent()

            Try
                If TypeOf m_OriginalExtent Is GCDConsoleLib.ExtentRectangle Then
                    valTop.Minimum = Decimal.MinValue
                    valLeft.Minimum = Decimal.MinValue
                    valBottom.Minimum = Decimal.MinValue
                    valRight.Minimum = Decimal.MinValue

                    valTop.Maximum = Decimal.MaxValue
                    valLeft.Maximum = Decimal.MaxValue
                    valBottom.Maximum = Decimal.MaxValue
                    valRight.Maximum = Decimal.MaxValue

                    valLeft.Value = MakeDivisible(m_OriginalExtent.Left, valCellSize.Value, CInt(valPrecision.Value), RoundingDirection.Down)
                    valBottom.Value = MakeDivisible(m_OriginalExtent.Bottom, valCellSize.Value, CInt(valPrecision.Value), RoundingDirection.Down)
                    valRight.Value = MakeDivisible(m_OriginalExtent.Right, valCellSize.Value, CInt(valPrecision.Value), RoundingDirection.Up)
                    valTop.Value = MakeDivisible(m_OriginalExtent.Top, valCellSize.Value, CInt(valPrecision.Value), RoundingDirection.Up)
                    UpdateOriginalRasterExtentFormatting()
                End If

                UpdateOutputRowsColsHeightWidth()
                RequiresResampling()

            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try
        End Sub

        Private Sub UpdateOutputRowsColsHeightWidth()

            Debug.Assert(valCellSize.Value > 0, "The cell size should never be zero.")

            Dim fProjHeight As Double = (valTop.Value - valBottom.Value)
            Dim fProjWidth As Double = (valRight.Value - valLeft.Value)
            txtProjRows.Text = (fProjHeight / valCellSize.Value).ToString("#,##0")
            txtProjCols.Text = (fProjWidth / valCellSize.Value).ToString("#,##0")
            txtProjWidth.Text = fProjWidth.ToString
            txtProjHeight.Text = fProjHeight.ToString

            valTop.ForeColor = Drawing.Color.Black
            valLeft.ForeColor = Drawing.Color.Black
            valBottom.ForeColor = Drawing.Color.Black
            valRight.ForeColor = Drawing.Color.Black
            If TypeOf ucRaster.SelectedItem Is GCDConsoleLib.Raster Then
                Dim gRaster As GCDConsoleLib.Raster = Nothing
                If GetSafeOriginalRaster(gRaster) Then

                    If valTop.Value = MakeDivisible(gRaster.Extent.Top, valCellSize.Value, CInt(valPrecision.Value), RoundingDirection.Up) Then valTop.ForeColor = Drawing.Color.DarkGreen
                    If valLeft.Value = MakeDivisible(gRaster.Extent.Left, valCellSize.Value, CInt(valPrecision.Value), RoundingDirection.Down) Then valLeft.ForeColor = Drawing.Color.DarkGreen
                    If valBottom.Value = MakeDivisible(gRaster.Extent.Bottom, valCellSize.Value, CInt(valPrecision.Value), RoundingDirection.Down) Then valBottom.ForeColor = Drawing.Color.DarkGreen
                    If valRight.Value = MakeDivisible(gRaster.Extent.Right, valCellSize.Value, CInt(valPrecision.Value), RoundingDirection.Up) Then valRight.ForeColor = Drawing.Color.DarkGreen

                End If
            End If

        End Sub

        Private Function MakeDivisible(fOriginalValue As Decimal, fCellSize As Decimal, nPrecision As Integer, eRoundingDirection As RoundingDirection) As Decimal

            Dim fResult As Decimal = 0
            If fOriginalValue <> 0 AndAlso fCellSize <> 0 Then
                fResult = fOriginalValue / fCellSize ' (10 ^ nPrecision)
                If eRoundingDirection = RoundingDirection.Up Then
                    fResult = Math.Ceiling(fResult)
                Else
                    fResult = Math.Floor(fResult)
                End If
                fResult = fResult * fCellSize
            End If
            Return fResult

        End Function

        Private Sub UpdateOriginalRasterExtentFormatting()

            ' Set the extent to red text if it is not divisible.
            If TypeOf m_OriginalExtent Is GCDConsoleLib.ExtentRectangle AndAlso valCellSize.Value > 0 Then
                Dim fValue As Double = Math.IEEERemainder(m_OriginalExtent.Left, valCellSize.Value)
                fValue = Math.Round(Math.IEEERemainder(m_OriginalExtent.Left, valCellSize.Value), CInt(valPrecision.Value) + 1)

                Dim fCellSize As Decimal = Math.Max(Math.Round(valCellSize.Value, CInt(valPrecision.Value)), valCellSize.Minimum)
                Debug.Assert(fCellSize > 0, "The cell size should not be zero.")

                If Math.Round(Math.IEEERemainder(m_OriginalExtent.Left, fCellSize), CInt(valPrecision.Value) + 1) <> 0 Then
                    txtLeft.ForeColor = Drawing.Color.Red
                Else
                    txtLeft.ForeColor = Control.DefaultForeColor
                End If

                If Math.Round(Math.IEEERemainder(m_OriginalExtent.Top, fCellSize), CInt(valPrecision.Value) + 1) <> 0 Then
                    txtTop.ForeColor = Drawing.Color.Red
                Else
                    txtTop.ForeColor = Control.DefaultForeColor
                End If

                If Math.Round(Math.IEEERemainder(m_OriginalExtent.Bottom, fCellSize), CInt(valPrecision.Value) + 1) <> 0 Then
                    txtBottom.ForeColor = Drawing.Color.Red
                Else
                    txtBottom.ForeColor = Control.DefaultForeColor
                End If

                If Math.Round(Math.IEEERemainder(m_OriginalExtent.Right, fCellSize), CInt(valPrecision.Value) + 1) <> 0 Then
                    txtRight.ForeColor = Drawing.Color.Red
                Else
                    txtRight.ForeColor = Control.DefaultForeColor
                End If
            End If
        End Sub

        Private Function RequiresResampling() As Boolean

            Dim bResult As Boolean = True
            Dim gOriginalRaster As GCDConsoleLib.Raster = Nothing
            If GetSafeOriginalRaster(gOriginalRaster) Then
                bResult = gOriginalRaster.IsDivisible
            End If

            If cboMethod.Items.Count > m_nNoInterpolationIndex Then
                If bResult Then
                    cboMethod.SelectedIndex = 0
                Else
                    cboMethod.SelectedIndex = m_nNoInterpolationIndex
                End If
            End If
            cboMethod.Enabled = bResult

            Return bResult

        End Function

        ''' <summary>
        ''' Disable typing in the original raster extent text boxes
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>Cannot change forecolor of textboxes when they are readonly. So make them
        ''' ReadOnly = False but skip any key pressing.</remarks>
        Private Sub txtLeft_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles _
        txtLeft.KeyPress, txtTop.KeyPress, txtBottom.KeyPress, txtRight.KeyPress

            e.Handled = True
        End Sub

        Private Sub cmdSave_Click(sender As System.Object, e As System.EventArgs) Handles cmdSave.Click
            naru.ui.Textbox.BrowseSaveRaster(txtRasterPath, "Output Raster", naru.os.File.RemoveDangerousCharacters(txtName.Text))
        End Sub

        Private Sub valPrecision_ValueChanged(sender As Object, e As System.EventArgs) Handles valPrecision.ValueChanged
            valCellSize.DecimalPlaces = valPrecision.Value
            valCellSize.Increment = 10 ^ (Decimal.Negate(valPrecision.Value))
            valCellSize.Minimum = 10 ^ Decimal.Negate(valPrecision.Value)
            valCellSize.Value = Math.Round(valCellSize.Value, CInt(valPrecision.Value))
            valTop.DecimalPlaces = valCellSize.DecimalPlaces
            valLeft.DecimalPlaces = valCellSize.DecimalPlaces
            valBottom.DecimalPlaces = valCellSize.DecimalPlaces
            valRight.DecimalPlaces = valCellSize.DecimalPlaces
            'UpdateOriginalRasterExtentFormatting()
        End Sub

        Private Sub cmdHelpPrecision_Click(sender As System.Object, e As System.EventArgs) Handles cmdHelpPrecision.Click
            Dim frm As New UtilityForms.frmInformation
            frm.InitializeFixedDialog("Horizontal Decimal Precision", GCDCore.Properties.Resources.PrecisionHelp)
            frm.ShowDialog()
        End Sub

        Private Sub cmdHelp_Click(sender As System.Object, e As System.EventArgs) Handles cmdHelp.Click

            Select Case m_ePurpose
                Case ImportRasterPurposes.StandaloneTool
                    Process.Start(GCDCore.Properties.Resources.HelpBaseURL & "gcd-command-reference/data-prep-menu/a-clean-raster-tool")

                Case ImportRasterPurposes.DEMSurvey
                    Process.Start(GCDCore.Properties.Resources.HelpBaseURL & "gcd-command-reference/data-prep-menu/d-add-dem-survey")

                Case ImportRasterPurposes.AssociatedSurface
                    Process.Start(GCDCore.Properties.Resources.HelpBaseURL & "gcd-command-reference/gcd-project-explorer/d-dem-context-menu/iv-add-associated-surface/1-loading-user-defined-associated-surface")

                Case ImportRasterPurposes.ErrorCalculation
                    Process.Start(GCDCore.Properties.Resources.HelpBaseURL & "gcd-command-reference/gcd-project-explorer/g-error-surfaces-context-menu/i-specify-error-surface")

            End Select
        End Sub

        Private Sub PerformRasterPyramids(ePurpose As ImportRasterPurposes, sRasterPath As IO.FileInfo)

            Dim ePyramidRasterType As GCDCore.RasterPyramidManager.PyramidRasterTypes
            Select Case m_ePurpose
                Case ImportRasterPurposes.DEMSurvey : ePyramidRasterType = GCDCore.RasterPyramidManager.PyramidRasterTypes.DEM
                Case ImportRasterPurposes.AssociatedSurface : ePyramidRasterType = GCDCore.RasterPyramidManager.PyramidRasterTypes.AssociatedSurfaces
                Case ImportRasterPurposes.ErrorCalculation : ePyramidRasterType = GCDCore.RasterPyramidManager.PyramidRasterTypes.ErrorSurfaces
                Case ImportRasterPurposes.StandaloneTool : Exit Sub
                Case Else
                    Debug.Assert(False, String.Format("The import raster purpose '{0}' does not have a corresponding raster pyramid build type.", m_ePurpose))
            End Select

            ProjectManager.PyramidManager.PerformRasterPyramids(ePyramidRasterType, sRasterPath)

        End Sub

    End Class

End Namespace