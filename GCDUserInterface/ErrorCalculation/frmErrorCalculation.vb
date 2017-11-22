Imports GCDCore.ErrorCalculation
Imports System.Windows.Forms
Imports GCDCore.Project

Namespace ErrorCalculation

    Public Class frmErrorCalculation

        Private DEM As DEMSurvey
        Private m_ErrorSurface As ErrorSurface

        ' This text is used when the DEM survey is single method.
        Private Const m_sEntireDEMExtent As String = "Entire DEM Extent"

        ' This dictionary stores the definitions of the error surface properties for each survey method polygon
        Private ErrorCalcProps As Dictionary(Of String, ErrorSurfaceProperty)

        ''' <summary>
        ''' Constructor to create a new error surface.
        ''' </summary>
        ''' <param name="dem">The DEM survey for which the error surface is being created</param>
        ''' <remarks></remarks>
        Public Sub New(dem As DEMSurvey)

            ' This call is required by the designer.
            InitializeComponent()

            Me.DEM = dem
        End Sub

        ''' <summary>
        ''' Constructor to view the properties of an existing error surface
        ''' </summary>
        ''' <param name="errSurface">The error surface to be viewed</param>
        ''' <remarks></remarks>
        Public Sub New(errSurface As ErrorSurface)

            ' This call is required by the designer.
            InitializeComponent()

            DEM = errSurface.DEM
            m_ErrorSurface = errSurface
        End Sub

        Private Sub frmErrorCalculation2_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

            tTip.SetToolTip(txtName, "The name for the error surface. The name must be unique for the DEM survey.")
            tTip.SetToolTip(txtRasterPath, "The path of the error surface raster within the GCD project.")
            tTip.SetToolTip(grdErrorProperties, "List of the survey methods obtained from the DEM Survey polygon feature class. The error surface properties for each survey method are show on the right.")
            tTip.SetToolTip(rdoUniform, "Choose this option to specify a constant, uniform error surface value for the selected survey method.")
            tTip.SetToolTip(valUniform, "The constant, uniform error surface value for the selected survey method. Must be a positive value")
            tTip.SetToolTip(rdoFIS, "Choose this option to specify an FIS error surface for the selected survey method.")
            tTip.SetToolTip(cboFIS, "Species the FIS rule file from the GCD FIS Library to use for the selected survey method.")
            tTip.SetToolTip(grdFISInputs, "Specify an associated surface for each FIS input for the selected FIS rule file and for the selected survey method.")

            grdErrorProperties.AllowUserToResizeRows = False
            grdFISInputs.AllowUserToResizeRows = False


            ' Load all the FIS rule files in the library to the combobox
            ' (Need to do this before the try/catch below that loads the error surface data
            For Each fis As FIS.FISLibraryItem In ProjectManager.FISLibrary
                cboFIS.Items.Add(fis)
            Next

            If TypeOf m_ErrorSurface Is ErrorSurface Then
                ' Existing error surface. Disable editing.
                txtName.Text = m_ErrorSurface.Name
                txtRasterPath.Text = ProjectManager.Project.GetRelativePath(m_ErrorSurface.Raster.RasterPath)
                btnOK.Text = "Save"
            End If

            Try
                ' Load the survey methods on the left and then populate the right side of the window.
                LoadSurveyMethods()
                LoadErrorCalculationMethods()

                ' Need to force the error properties to update and reflect the contents of the left side of the form
                UpdateGridWithErrorProperties(0)

                ' Update which controls are enabled.
                UpdateControls()
            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try

            ' Load all the associated surfaces in the survey library to the grid combo box
            Dim colCombo As DataGridViewComboBoxColumn = grdFISInputs.Columns(1)
            colCombo.DataSource = DEM.AssocSurfaces.Values.AsEnumerable()
            colCombo.DisplayMember = "Name"

            ' Also load any error surfaces into the associated error surface combo box.
            cboAssociated.DataSource = DEM.AssocSurfaces.Values

            ' Disable the associated error surface option if in readonly mode or else
            ' there are no associated error surfaces for the DEM survey
            rdoAssociated.Enabled = m_ErrorSurface Is Nothing AndAlso cboAssociated.Items.Count > 0
            rdoFIS.Enabled = m_ErrorSurface Is Nothing AndAlso DEM.AssocSurfaces.Count < 1

            Try
                ' Safely retrieve the spatial units of the DEM
                rdoUniform.Text = String.Format("{0} ({1})", rdoUniform.Text, UnitsNet.Length.GetAbbreviation(ProjectManager.Project.Units.VertUnit))
            Catch ex As Exception
                ' Don't show an error in release mode
                Debug.Assert(False, "Error retrieving linear units from DEM")
            End Try

        End Sub

        ''' <summary>
        ''' Loads the survey methods into the member dictionary.
        ''' </summary>
        ''' <remarks>Note that another method is responsible for displaying the dictionary contents in the UI</remarks>
        Private Sub LoadSurveyMethods()

            ' Always create a new dictionary, which will clear any existing entries
            ErrorCalcProps = New Dictionary(Of String, ErrorSurfaceProperty)

            ' Attempt to load the survey methods from an existing error surface if it exists
            If m_ErrorSurface Is Nothing Then

                If DEM.IsSingleSurveyMethod Then
                    ' Single method, see if the survey method has a default error value in the GCD software SurveyTypes XML file
                    Dim uniformValue As Double = 0
                    If Not String.IsNullOrEmpty(DEM.SurveyMethod) AndAlso ProjectManager.SurveyTypes.ContainsKey(DEM.SurveyMethod) Then
                        uniformValue = ProjectManager.SurveyTypes(DEM.SurveyMethod).ErrorValue
                    End If
                    ErrorCalcProps(m_sEntireDEMExtent) = New ErrorSurfaceProperty(m_sEntireDEMExtent, uniformValue)
                Else
                    ' Multi-method - load distinct survey types from method mask field in ShapeFile
                    Dim polygonMask As New GCDConsoleLib.Vector(DEM.MethodMask)
                    For Each feature As GCDConsoleLib.VectorFeature In polygonMask.Features.Values
                        Dim maskValue As String = feature.GetFieldAsString(DEM.MethodMaskField)
                        If Not String.IsNullOrEmpty(maskValue) AndAlso Not ErrorCalcProps.ContainsKey(maskValue) Then
                            Dim uniformValue As Double = 0
                            If ProjectManager.SurveyTypes.ContainsKey(maskValue) Then
                                uniformValue = ProjectManager.SurveyTypes(maskValue).ErrorValue
                            End If
                            ErrorCalcProps(maskValue) = New ErrorSurfaceProperty(maskValue, uniformValue)
                        End If
                    Next
                End If
            Else
                ErrorCalcProps = m_ErrorSurface.ErrorProperties
            End If

        End Sub

        ''' <summary>
        ''' Loads the contents of the member dictionary into the user interface
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub LoadErrorCalculationMethods()

            For Each errProps As ErrorSurfaceProperty In ErrorCalcProps.Values
                Dim nMethodRow As Integer = grdErrorProperties.Rows.Add
                grdErrorProperties.Rows(nMethodRow).Cells(0).Value = errProps.Name

                If errProps.UniformValue.HasValue Then
                    grdErrorProperties.Rows(nMethodRow).Cells(1).Value = "Uniform Error"
                ElseIf errProps.AssociatedSurface Is AssociatedSurface Then
                    grdErrorProperties.Rows(nMethodRow).Cells(1).Value = "Associated Surface"
                Else
                    grdErrorProperties.Rows(nMethodRow).Cells(1).Value = "FIS Error"
                End If
            Next

            ' Now select the first row in the grid. this will automatically update the right hand panel
            If grdErrorProperties.Rows.Count > 0 Then
                grdErrorProperties.Rows(0).Selected = True
            End If
        End Sub

        ''' <summary>
        ''' Retrieve the default error value for a particular survey type from the survey types library
        ''' </summary>
        ''' <param name="sMethod"></param>
        ''' <returns>Default error value or zero if the method is not found</returns>
        ''' <remarks></remarks>
        Private Function GetDefaultErrorValue(sMethod As String) As Double

            If ProjectManager.SurveyTypes.ContainsKey(sMethod) Then
                Return ProjectManager.SurveyTypes(sMethod).ErrorValue
            Else
                Return 0
            End If

        End Function

        ''' <summary>
        ''' When the user clicks on any cell in the left grid, the right grid should be updated.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>Remember to save the current settings before updating</remarks>
        Private Sub grdErrorProperties_CellClick(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdErrorProperties.CellClick

            UpdateGridWithErrorProperties(e.RowIndex)

        End Sub

        Private Sub grdErrorProperties_CellLeave(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdErrorProperties.CellLeave

            SaveErrorProperties()

        End Sub

        ''' <summary>
        ''' Update the FIS inputs grid based on the currently selected survey method
        ''' </summary>
        ''' <param name="nNewRow"></param>
        ''' <remarks></remarks>
        Private Sub UpdateGridWithErrorProperties(nNewRow)

            grdFISInputs.Rows.Clear()
            Dim sMethod As String = grdErrorProperties.Rows(nNewRow).Cells(0).Value
            If Not String.IsNullOrEmpty(sMethod) Then
                If ErrorCalcProps.ContainsKey(sMethod) Then

                    ' Only proceed and load anything into the FIS inputs grid if the error surface for this survey method is a FIS
                    Dim prop As ErrorSurfaceProperty = ErrorCalcProps(sMethod)
                    If prop.UniformValue.HasValue Then
                        rdoUniform.Checked = True
                        valUniform.Value = prop.UniformValue.Value

                        cboFIS.SelectedIndex = -1
                        cboAssociated.SelectedIndex = -1
                    ElseIf prop.AssociatedSurface Is AssociatedSurface Then
                        rdoAssociated.Checked = True
                        cboFIS.SelectedIndex = -1
                        cboAssociated.SelectedItem = prop.AssociatedSurface
                    Else
                        rdoFIS.Checked = True
                        cboAssociated.SelectedIndex = -1

                        For i As Integer = 0 To cboFIS.Items.Count - 1
                            If String.Compare(DirectCast(cboFIS.Items(i), naru.db.NamedObject).Name, prop.FISRuleFile.FullName, True) = 0 Then
                                cboFIS.SelectedIndex = i
                                Exit For
                            End If
                        Next

                        ' Force the FIS grid to update (including retrieval of the FIS properties)
                        UpdateFISGrid()
                    End If
                End If
            End If

            cboAssociated.Enabled = rdoAssociated.Enabled AndAlso rdoAssociated.Checked

        End Sub

        ''' <summary>
        ''' Update which error surface controls are available based on the currently selected survey method
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub rdoUniform_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdoUniform.CheckedChanged, rdoFIS.CheckedChanged, rdoAssociated.CheckedChanged

            UpdateControls()
        End Sub

        ''' <summary>
        ''' Update which error surface controls are available based on the currently selected survey method
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub UpdateControls()

            If rdoFIS.Checked Then
                If DEM.AssocSurfaces.Count < 1 Then
                    MessageBox.Show("You cannot create a FIS error surface until you define at least 2 associated surfaces for this DEM survey.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    rdoUniform.Checked = True
                End If
            End If

            If TypeOf m_ErrorSurface Is ErrorSurface Then
                ' Existing error surface. Disable editing.

                rdoUniform.Enabled = False
                valUniform.Enabled = False

                rdoFIS.Enabled = False
                cboFIS.Enabled = False
                grdFISInputs.Enabled = False

                rdoAssociated.Enabled = False
                cboAssociated.Enabled = False
            Else
                valUniform.Enabled = rdoUniform.Checked
                cboFIS.Enabled = rdoFIS.Checked
                grdFISInputs.Enabled = rdoFIS.Checked
                cboAssociated.Enabled = rdoAssociated.Checked
            End If

            ' Need to change the left, survey methods grid
            Dim r As DataGridViewSelectedRowCollection = grdErrorProperties.SelectedRows
            If r.Count = 1 Then
                Dim sType As String = "Uniform Error"
                If rdoAssociated.Checked Then
                    sType = String.Format("Associated Surface")
                ElseIf rdoFIS.Checked Then
                    sType = "FIS Error"
                End If
                r(0).Cells(1).Value = sType
            End If

        End Sub

        Private Sub cboFIS_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboFIS.SelectedIndexChanged
            UpdateFISGrid()
        End Sub

        ''' <summary>
        ''' Change which FIS inputs are listed on the right side of the form
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub UpdateFISGrid()

            grdFISInputs.Rows.Clear()

            If Not rdoFIS.Checked Then
                Return
            End If

            Dim theFISRuleFile As New FIS.FISRuleFile(DirectCast(cboFIS.SelectedItem, FIS.FISLibraryItem).FilePath)

            ' Loop over all the inputs defined for the FIS
            For Each sInput As String In theFISRuleFile.FISInputs
                Dim nRow As Integer = grdFISInputs.Rows.Add
                grdFISInputs.Rows(nRow).Cells(0).Value = sInput

                ' Get the selected error properties row
                Dim lErr As DataGridViewSelectedRowCollection = grdErrorProperties.SelectedRows
                If lErr.Count = 1 Then
                    Dim errProps As ErrorSurfaceProperty = ErrorCalcProps(lErr(0).Cells(0).Value)

                    ' Only proceed if the error surface definition is a FIS
                    If Not errProps.FISInputs Is Nothing Then
                        ' loop over all the defined FIS inputs for the error surface
                        For Each sDefinedInput As String In errProps.FISInputs.Keys
                            If String.Compare(sInput, sDefinedInput, True) = 0 Then
                                ' this is a FIS input that has a definition already
                                grdFISInputs.Rows(nRow).Cells(1).Value = errProps.FISInputs(sDefinedInput)
                            End If
                        Next
                    End If
                End If
            Next

        End Sub

        ''' <summary>
        ''' Save the error surface properties for the selected survey method
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function SaveErrorProperties() As Boolean

            Dim r As DataGridViewSelectedRowCollection = grdErrorProperties.SelectedRows
            If r.Count <> 1 Then
                Return True
            End If

            ' Save just the survey method that is selected in the left grid
            Dim sSurveyMethod As String = r.Item(0).Cells(0).Value
            Dim sErrorType As String = r.Item(0).Cells(1).Value

            If rdoUniform.Checked Then
                ' Create a new Uniform error properties
                ErrorCalcProps(sSurveyMethod) = New ErrorSurfaceProperty(sSurveyMethod, valUniform.Value)
                sErrorType = "Uniform Error"
            ElseIf rdoAssociated.Checked Then
                If cboAssociated.SelectedIndex >= 0 Then
                    ' Create a new associated surface error properties
                    ErrorCalcProps(sSurveyMethod) = New ErrorSurfaceProperty(sSurveyMethod, DirectCast(cboAssociated.SelectedItem, AssocSurface))
                    sErrorType = "Associated Surface"
                Else
                    MessageBox.Show("You must select an associated surface that contains the error values for this error surface.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            Else
                ' Make sure the user has selected a GIS input
                If cboFIS.SelectedIndex < 0 Then
                    MessageBox.Show("You must select a FIS rule file for this survey method or define it as uniform error.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                Else
                    ' FIS. Loop through all the FIS inputs
                    Dim dInputs As New Dictionary(Of String, AssocSurface)
                    For i = 0 To grdFISInputs.Rows.Count - 1
                        Dim cboAssoc As DataGridViewComboBoxCell = grdFISInputs.Rows(i).Cells(1)
                        If cboAssoc.Value > 0 Then
                            dInputs.Add(grdFISInputs.Rows(i).Cells(0).Value, cboAssoc.Value)
                        Else
                            MessageBox.Show("You must choose an associated surface for each FIS input.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Return False
                        End If
                    Next

                    ' Find the matching fis library file
                    ErrorCalcProps(sSurveyMethod) = New ErrorSurfaceProperty(sSurveyMethod, DirectCast(cboFIS.SelectedItem, FIS.FISLibraryItem).FilePath, dInputs)
                    sErrorType = "FIS Error"
                End If
            End If

            r.Item(0).Cells(1).Value = sErrorType

            Return True

        End Function

        ''' <summary>
        ''' The error surface name has changed. Update the output raster path.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub txtName_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtName.TextChanged

            If TypeOf m_ErrorSurface Is ErrorSurface Then
                txtRasterPath.Text = ProjectManager.Project.GetRelativePath(m_ErrorSurface.Raster.RasterPath)
            Else
                Dim sRasterPath As String = String.Empty
                If Not String.IsNullOrEmpty(txtName.Text) Then
                    sRasterPath = ProjectManager.OutputManager.ErrorSurfaceRasterPath(DEM.Name, txtName.Text, False).FullName
                    sRasterPath = ProjectManager.Project.GetRelativePath(sRasterPath)
                End If
                txtRasterPath.Text = sRasterPath
            End If
        End Sub

        Private Sub btnHelp_Click(sender As System.Object, e As System.EventArgs) Handles btnHelp.Click
            Process.Start(GCDCore.Properties.Resources.HelpBaseURL & "gcd-command-reference/gcd-project-explorer/g-error-surfaces-context-menu/ii-derive-error-surface")
        End Sub

        Private Function ValidateForm() As Boolean

            If (String.IsNullOrEmpty(txtName.Text.Trim())) Then
                MessageBox.Show("You must provide a unique name for the error surface.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            Else
                If Not DEM.IsErrorSurfaceNameUnique(txtName.Text, m_ErrorSurface) Then
                    MessageBox.Show("There is another error surface for this DEM Survey that already possesses this name. You must provide a unique name for the error surface.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            End If

            Return True

        End Function

        Private Sub btnOK_Click(sender As Object, e As System.EventArgs) Handles btnOK.Click

            If Not ValidateForm() Then
                Me.DialogResult = DialogResult.None
                Exit Sub
            End If

            Cursor.Current = Cursors.WaitCursor

            ' Need to save the current error properties first.
            If Not SaveErrorProperties() Then
                Me.DialogResult = DialogResult.None
                Exit Sub
            End If

            If TypeOf m_ErrorSurface Is ErrorSurface Then
                m_ErrorSurface.Name = txtName.Text
            Else
                ' Create the new error surface object
                m_ErrorSurface = GCDCore.Engines.

                m_rErrorSurface = ProjectManager.ds.ErrorSurface.NewErrorSurfaceRow
                m_rErrorSurface.Name = txtName.Text
                m_rErrorSurface.DEMSurveyRow = m_rDEMSurvey
                m_rErrorSurface.Source = txtRasterPath.Text
                m_rErrorSurface.Type = ErrorSurfaceType
                ProjectManager.ds.ErrorSurface.AddErrorSurfaceRow(m_rErrorSurface)

                ' Add all the survey methods to the database
                For Each sSurveyMethod As String In ErrorCalcProps.Keys
                    Dim fError As Double = 0
                    Dim nAssociatedSurfaceID As Integer = 0

                    If TypeOf ErrorCalcProps(sSurveyMethod) Is ErrorCalcPropertiesUniform Then
                        fError = DirectCast(ErrorCalcProps(sSurveyMethod), ErrorCalcPropertiesUniform).UniformErrorValue
                    End If

                    If TypeOf ErrorCalcProps(sSurveyMethod) Is ErrorCalcPropertiesAssoc Then
                        nAssociatedSurfaceID = DirectCast(ErrorCalcProps(sSurveyMethod), ErrorCalcPropertiesAssoc).AssociatedSurfaceID
                    End If

                    ' This error type appears to be not used when uniform, but when it's FIS it should be the name of the FIS rule
                    ' file
                    Dim sErrorType As String = ErrorCalcProps(sSurveyMethod).ErrorType
                    If TypeOf ErrorCalcProps(sSurveyMethod) Is ErrorCalcPropertiesFIS Then
                        sErrorType = DirectCast(ErrorCalcProps(sSurveyMethod), ErrorCalcPropertiesFIS).FISRuleFilePath.FullName
                    End If

                    ProjectManager.ds.MultiErrorProperties.AddMultiErrorPropertiesRow(sSurveyMethod, fError, m_rErrorSurface, sErrorType, nAssociatedSurfaceID) ' m_dErrorCalculationProperties(sSurveyMethod).ErrorType)

                    ' Now add all the FIS inputs to the FIS table
                    If TypeOf ErrorCalcProps(sSurveyMethod) Is ErrorCalcPropertiesFIS Then
                        Dim errProps As ErrorCalcPropertiesFIS = ErrorCalcProps(sSurveyMethod)
                        For Each sInput As String In errProps.FISInputs.Keys
                            ' Now find the associated surface for this input
                            For Each rAssoc As ProjectDS.AssociatedSurfaceRow In m_rDEMSurvey.GetAssociatedSurfaceRows
                                If rAssoc.AssociatedSurfaceID = errProps.FISInputs(sInput) Then
                                    ProjectManager.ds.FISInputs.AddFISInputsRow(m_rErrorSurface, sInput, rAssoc.Name, errProps.FISRuleFilePath.FullName)
                                    Exit For
                                End If
                            Next
                        Next
                        'fError = DirectCast(m_dErrorCalculationProperties(sSurveyMethod), ErrorCalculation.ErrorCalcPropertiesUniform).UniformErrorValue
                    End If
                Next
                ' For new error surfaces, we now want to create the actual error raster
                Try
                    Dim errEngine As New ErrorSurfaceEngine(m_rErrorSurface)
                    errEngine.CreateErrorSurfaceRaster()

                    'If My.Settings.AddOutputLayersToMap Then
                    ' TODO 
                    Throw New Exception("not implemented")
                    'Core.GCDProject.ProjectManagerUI.ArcMapManager.AddErrSurface(m_rErrorSurface)
                    'End If
                Catch ex As Exception
                    DialogResult = DialogResult.None
                    Cursor.Current = Cursors.Default
                    ProjectManager.ds.ErrorSurface.RemoveErrorSurfaceRow(m_rErrorSurface)
                    Dim ex2 As New Exception("Error generating error surface raster. No changes were made to the GCD project.", ex)
                    naru.error.ExceptionUI.HandleException(ex2)
                    Exit Sub
                End Try
            End If

            ' Now save the GCD project
            Try
                ProjectManager.save()
            Catch ex As Exception
                DialogResult = DialogResult.None
                Cursor.Current = Cursors.Default
                Dim ex2 As New Exception("Error saving error surface properties to the GCD project.", ex)
                naru.error.ExceptionUI.HandleException(ex2)
                Exit Sub
            End Try

            Cursor.Current = Cursors.Default

        End Sub

        ''' <summary>
        ''' Build the semi-colon delimeted string pair list of FIS input names and associated source raster paths
        ''' </summary>
        ''' <param name="errProps"></param>
        ''' <returns></returns>
        ''' <remarks>.e.g. Slope;C:\MySlope.tif;PointDensity;C:\MyPointDensity.tif</remarks>
        Private Function GetFISInputString(ByRef errProps As ErrorCalcPropertiesFIS) As String

            Dim sInputs As String = ""
            For Each sFISInput As String In errProps.FISInputs.Keys
                For Each rAssoc As ProjectDS.AssociatedSurfaceRow In m_rDEMSurvey.GetAssociatedSurfaceRows
                    If rAssoc.AssociatedSurfaceID = errProps.FISInputs(sFISInput) Then
                        sInputs &= String.Format("{0};{1}", sFISInput, ProjectManager.GetAbsolutePath(rAssoc.Source))
                        Exit For
                    End If
                Next
            Next

            If sInputs.Length > 0 Then
                sInputs = sInputs.Substring(0, sInputs.Length - 1)
            Else
                Throw New Exception("No FIS inputs to process.")
            End If

            Return sInputs

        End Function

    End Class

End Namespace