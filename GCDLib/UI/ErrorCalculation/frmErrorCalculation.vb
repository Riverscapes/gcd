Imports GCD.GCDLib.Core.ErrorCalculation
Imports System.Windows.Forms

Namespace UI.ErrorCalculation

    Public Class frmErrorCalculation

        Private m_rDEMSurvey As ProjectDS.DEMSurveyRow
        Private m_rErrorSurface As ProjectDS.ErrorSurfaceRow

        ' This text is used when the DEM survey is single method.
        Private Const m_sEntireDEMExtent As String = "Entire DEM Extent"

        ' This dictionary stores the definitions of the error surface properties for each survey method polygon
        Private m_dErrorCalculationProperties As Dictionary(Of String, ErrorCalcPropertiesBase)

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
            For Each rFIS As GCDLib.FISLibrary.FISTableRow In Core.GCDProject.ProjectManagerUI.fisds.FISTable
                cboFIS.Items.Add(New naru.db.NamedObject(rFIS.FISID, rFIS.Name))
            Next
            cboFIS.ValueMember = "ID"
            cboFIS.DisplayMember = "Name"


            If TypeOf m_rErrorSurface Is ProjectDS.ErrorSurfaceRow Then
                ' Existing error surface. Disable editing.
                txtName.Text = m_rErrorSurface.Name
                txtRasterPath.Text = m_rErrorSurface.Source
                btnOK.Text = "Save"
            End If

            Try
                ' Load the survey methods on the left and then populate the right side of the window.
                LoadSurveyMethods()
                LoadErrorCalculationMethods()

                ' Need to force the right side of the form to update to reflect initial properties.

                ' Need to force the error properties to update and reflect the contents of the left side of the form
                UpdateGridWithErrorProperties(0)

                ' Update which controls are enabled.
                UpdateControls()
            Catch ex As Exception
                Core.ExceptionHelper.HandleException(ex)
            End Try


            ' Load all the associated surfaces in the survey library to the grid combo box
            ' Also load any error surfaces into the associated error surface combo box.
            Dim colCombo As DataGridViewComboBoxColumn = grdFISInputs.Columns(1)
            For Each aSurface As ProjectDS.AssociatedSurfaceRow In m_rDEMSurvey.GetAssociatedSurfaceRows
                colCombo.Items.Add(New naru.db.NamedObject(aSurface.AssociatedSurfaceID, aSurface.Name))

                ' Also load any error surfaces into the associated error surface combo box.
                'If String.Compare(aSurface.Type, "Error Surface", True) = 0 Then
                cboAssociated.Items.Add(New naru.db.NamedObject(aSurface.AssociatedSurfaceID, aSurface.Name))
                'End If
            Next
            colCombo.ValueMember = "ID"
            colCombo.DisplayMember = "Name"

            ' Disable the associated error surface option if in readonly mode or else
            ' there are no associated error surfaces for the DEM survey
            rdoAssociated.Enabled = m_rErrorSurface Is Nothing AndAlso cboAssociated.Items.Count > 0

            Try
                ' Safely retrieve the spatial units of the DEM
                Dim sDEMPath As String = Core.GCDProject.ProjectManagerBase.GetAbsolutePath(m_rDEMSurvey.Source)
                Dim gDEM As New Core.RasterWranglerLib.Raster(sDEMPath)
                rdoUniform.Text = rdoUniform.Text & " " & gDEM.LinearUnits.GetUnitsAsString(True)
            Catch ex As Exception
                ' Don't show an error in release mode
                Debug.Assert(False, "Error retrieving linear units from DEM")
            End Try

        End Sub

        ''' <summary>
        ''' Use this constructor to create a new error surface.
        ''' </summary>
        ''' <param name="rDEMSurvey">The DEM survey for which the error surface is being created</param>
        ''' <remarks></remarks>
        Public Sub New(rDEMSurvey As ProjectDS.DEMSurveyRow)

            ' This call is required by the designer.
            InitializeComponent()

            m_rDEMSurvey = rDEMSurvey
            m_rErrorSurface = Nothing
        End Sub

        ''' <summary>
        ''' Use this constructor to view the properties of an existing error surface
        ''' </summary>
        ''' <param name="rErrorSurface">The error surface to be viewed</param>
        ''' <remarks></remarks>
        Public Sub New(rErrorSurface As ProjectDS.ErrorSurfaceRow)

            ' This call is required by the designer.
            InitializeComponent()

            m_rDEMSurvey = rErrorSurface.DEMSurveyRow
            m_rErrorSurface = rErrorSurface
        End Sub

        ''' <summary>
        ''' Loads the survey methods into the member dictionary.
        ''' </summary>
        ''' <remarks>Note that another method is responsible for displaying the dictionary contents in the UI</remarks>
        Private Sub LoadSurveyMethods()

            ' Always create a new dictionary, which will clear any existing entries
            m_dErrorCalculationProperties = New Dictionary(Of String, Core.ErrorCalculation.ErrorCalcPropertiesBase)

            ' Attempt to load the survey methods from an existing error surface if it exists
            If TypeOf m_rErrorSurface Is ProjectDS.ErrorSurfaceRow Then

                ' Loop over all the error inputs in the project and load the dictionary from there
                For Each rProperties As ProjectDS.MultiErrorPropertiesRow In m_rErrorSurface.GetMultiErrorPropertiesRows
                    Select Case rProperties.ErrorType

                        Case ErrorSurfaceEngine.UniformErrorString
                            m_dErrorCalculationProperties.Add(rProperties.Method, New Core.ErrorCalculation.ErrorCalcPropertiesUniform(rProperties.Method, rProperties._Error))

                        Case ErrorSurfaceEngine.AssociatedsurfaceErrorType
                            m_dErrorCalculationProperties.Add(rProperties.Method, New ErrorCalcPropertiesAssoc(rProperties.Method, rProperties.AssociatedSurfaceID))

                        Case Else
                            ' The type appears to be the name of the FIS file (weird)

                            Dim dFISInputs As New Dictionary(Of String, Integer)
                            Dim sFISName As String = String.Empty
                            For Each rInput As ProjectDS.FISInputsRow In m_rErrorSurface.GetFISInputsRows

                                ' Loop over all associated surface rows and get the one with the right name
                                Dim nAssocID As Integer = 0
                                For Each rAssoc As ProjectDS.AssociatedSurfaceRow In m_rDEMSurvey.GetAssociatedSurfaceRows
                                    If String.Compare(rAssoc.Name, rInput.AssociatedSurface, True) = 0 Then
                                        nAssocID = rAssoc.AssociatedSurfaceID
                                        Exit For
                                    End If
                                Next

                                dFISInputs.Add(rInput.FISInput, nAssocID)
                                sFISName = rInput.FIS
                            Next

                            ' TODO Get the FIS input rule file ID from the name
                            Dim nFISID As Integer = -1

                            ' Find the local path of the FIS rule file based on the library on this machine. Note
                            ' could be imported project from another machine.
                            Dim sFISFuleFilePath As String = String.Empty
                            For Each rFISRuleFile As GCDLib.FISLibrary.FISTableRow In Core.GCDProject.ProjectManager.fisds.FISTable.Rows
                                If String.Compare(rFISRuleFile.Name, sFISName, True) = 0 Then
                                    sFISFuleFilePath = rFISRuleFile.Path
                                    Exit For
                                End If
                            Next

                            m_dErrorCalculationProperties.Add(rProperties.Method, New ErrorCalcPropertiesFIS(rProperties.Method, nFISID, sFISFuleFilePath, dFISInputs))

                    End Select
                Next
            Else
                '
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' Now check that the dictionary contains all the correct elements. 
                ' this will primarily be used for new error surfaces, but could also
                ' be used if a method mask was not correctly initialized.
                If IsSingleMethodError Then
                    ' This is a single method survey

                    ' This is a new error surface. Default to uniform error. Try and get the error
                    ' value from the DEM survey type or default to zero.
                    Dim sSurveyType As String = m_sEntireDEMExtent
                    Dim fErrorValue As Double = 0
                    If Not m_rDEMSurvey.IsSingleMethodTypeNull Then
                        sSurveyType = m_rDEMSurvey.SingleMethodType
                        fErrorValue = GetDefaultErrorValue(m_rDEMSurvey.SingleMethodType)
                    End If

                    ' Add a single survey method, unfiorm error value as default
                    m_dErrorCalculationProperties(sSurveyType) = New ErrorCalcPropertiesUniform(sSurveyType, fErrorValue)
                Else
                    If m_rDEMSurvey.IsMethodMaskFieldNull Then
                        Throw New Exception("Multi method DEM with no method mask field defined.")
                    Else
                        Dim sMethodMask As String = Core.GCDProject.ProjectManagerBase.GetAbsolutePath(m_rDEMSurvey.MethodMask)
                        Dim gMethodMask As New Core.RasterWranglerLib.Vector(sMethodMask)
                        Dim nIdentifierFld As Integer = gMethodMask.FindField(m_rDEMSurvey.MethodMaskField)
                        If nIdentifierFld < 0 Then
                            Dim ex As New Exception("Unable to find method mask field in mask polygon feature class")
                            ex.Data("Feature Class") = gMethodMask.FullPath
                            ex.Data("Field") = m_rDEMSurvey.MethodMaskField
                            Throw ex
                        End If

                        ' TODO: commented out loop over featurees below
                        Throw New Exception("not implemented")

                        'Dim pCursor As IFeatureCursor = gMethodMask.FeatureClass.Search(Nothing, True)
                        'Dim pFeature As IFeature = pCursor.NextFeature
                        'Dim sValue As String

                        'While TypeOf pFeature Is IFeature
                        '    If Not IsDBNull(pFeature.Value(nIdentifierFld)) Then
                        '        sValue = pFeature.Value(nIdentifierFld)

                        '        ' Attempt to get the error value from the survey types data grid
                        '        Dim fErrorValue As Double = 0
                        '        If Not m_rDEMSurvey.IsSingleMethodTypeNull Then
                        '            fErrorValue = GetDefaultErrorValue(sValue)
                        '        End If

                        '        ' Add a single survey method, unfiorm error value as default
                        '        m_dErrorCalculationProperties(sValue) = New ErrorCalcPropertiesUniform(sValue, fErrorValue)
                        '    End If
                        '    pFeature = pCursor.NextFeature
                        'End While
                        'Runtime.InteropServices.Marshal.ReleaseComObject(pCursor)
                        'pCursor = Nothing
                    End If
                End If
            End If

        End Sub

        ''' <summary>
        ''' Loads the contents of the member dictionary into the user interface
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub LoadErrorCalculationMethods()

            For Each errProps As ErrorCalcPropertiesBase In m_dErrorCalculationProperties.Values
                Dim nMethodRow As Integer = grdErrorProperties.Rows.Add
                grdErrorProperties.Rows(nMethodRow).Cells(0).Value = errProps.SurveyMethod
                grdErrorProperties.Rows(nMethodRow).Cells(1).Value = errProps.ErrorType
            Next

            ' Now select the first row in the grid. this will automatically update
            ' the right hand panel
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

            Dim fValue As Double = 0
            Dim dRows As SurveyTypes.SurveyTypesRow() = Core.GCDProject.ProjectManager.surveyds._SurveyTypes.Select("Name = '" & sMethod & "'")
            If dRows.Count > 0 Then
                Dim r As SurveyTypes.SurveyTypesRow = dRows(0)
                Double.TryParse(r._Error, fValue)
            End If

            Return fValue

        End Function

        ''' <summary>
        ''' The FIS is single when the DEM Survey is defined as single
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private ReadOnly Property IsSingleMethodError As Boolean
            Get
                Dim bSingleMethod As Boolean = True
                If Not m_rDEMSurvey.IsSingleMethodTypeNull Then
                    Boolean.TryParse(m_rDEMSurvey.SingleMethod, bSingleMethod)
                End If
                Return bSingleMethod
            End Get
        End Property

        ''' <summary>
        ''' When the user clicks on any cell in the left grid, the right grid should be updated.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>Remember to save the current settings before updating</remarks>
        Private Sub grdErrorProperties_CellClick(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdErrorProperties.CellClick

            ' SaveErrorProperties()
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
            'If nNewRow >= 0 Then
            Dim sMethod As String = grdErrorProperties.Rows(nNewRow).Cells(0).Value
            If Not String.IsNullOrEmpty(sMethod) Then
                If m_dErrorCalculationProperties.ContainsKey(sMethod) Then

                    ' Only proceed and load anything into the FIS inputs grid if the error surface for this survey method is a FIS
                    Dim eErrorProperties As ErrorCalcPropertiesBase = m_dErrorCalculationProperties(sMethod)
                    If TypeOf eErrorProperties Is ErrorCalcPropertiesUniform Then
                        rdoUniform.Checked = True
                        valUniform.Value = DirectCast(eErrorProperties, ErrorCalcPropertiesUniform).UniformErrorValue

                        cboFIS.SelectedIndex = -1
                        cboAssociated.SelectedIndex = -1

                    ElseIf TypeOf eErrorProperties Is ErrorCalcPropertiesAssoc Then
                        rdoAssociated.Checked = True
                        cboFIS.SelectedIndex = -1

                        For i As Integer = 0 To cboAssociated.Items.Count - 1
                            If DirectCast(cboAssociated.Items(i), naru.db.NamedObject).ID = DirectCast(eErrorProperties, ErrorCalcPropertiesAssoc).AssociatedSurfaceID Then
                                cboAssociated.SelectedIndex = i
                                Exit For
                            End If
                        Next
                    Else
                        Dim eFISProperties As ErrorCalcPropertiesFIS = eErrorProperties
                        rdoFIS.Checked = True

                        'cboFIS.SelectedValue = eFISProperties.FISID
                        For i As Integer = 0 To cboFIS.Items.Count - 1
                            If String.Compare(DirectCast(cboFIS.Items(i), naru.db.NamedObject).Name, eFISProperties.FISRuleName, True) = 0 Then
                                cboFIS.SelectedIndex = i
                                Exit For
                            End If
                        Next

                        cboAssociated.SelectedIndex = -1

                        ' Force the FIS grid to update (including retrieval of the FIS properties)
                        UpdateFISGrid()

                    End If
                End If
            End If
            'End If

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
                If m_rDEMSurvey.GetAssociatedSurfaceRows.Count < 1 Then
                    MessageBox.Show("You cannot create a FIS error surface until you define at least 2 associated surfaces for this DEM survey.", My.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    rdoUniform.Checked = True
                End If
            End If

            If TypeOf m_rErrorSurface Is ProjectDS.ErrorSurfaceRow Then
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

            If rdoFIS.Checked Then
                If TypeOf cboFIS.SelectedItem Is naru.db.NamedObject Then
                    Dim l As naru.db.NamedObject = cboFIS.SelectedItem
                    For Each rFIS As GCDLib.FISLibrary.FISTableRow In Core.GCDProject.ProjectManagerUI.fisds.FISTable
                        If l.ID = rFIS.FISID Then
                            ' This is the FIS selected in the combo box
                            Dim theFISRuleFile As New FIS.FISRuleFile(rFIS.Path)

                            ' Loop over all the inputs defined for the FIS
                            For Each sInput As String In theFISRuleFile.FISInputs
                                Dim nRow As Integer = grdFISInputs.Rows.Add
                                grdFISInputs.Rows(nRow).Cells(0).Value = sInput

                                ' Get the selected error properties row
                                Dim lErr As DataGridViewSelectedRowCollection = grdErrorProperties.SelectedRows
                                If lErr.Count = 1 Then
                                    Dim errProps As ErrorCalcPropertiesBase = m_dErrorCalculationProperties(lErr(0).Cells(0).Value)

                                    ' Only proceed if the error surface definition is a FIS
                                    If TypeOf errProps Is ErrorCalcPropertiesFIS Then
                                        Dim fisProps As ErrorCalcPropertiesFIS = errProps

                                        ' loop over all the defined FIS inputs for the error surface
                                        For Each sDefinedInput As String In fisProps.FISInputs.Keys
                                            If String.Compare(sInput, sDefinedInput, True) = 0 Then
                                                ' this is a FIS input that has a definition already
                                                grdFISInputs.Rows(nRow).Cells(1).Value = fisProps.FISInputs(sDefinedInput)
                                            End If
                                        Next
                                    End If

                                End If
                            Next

                            ' Do not continue looking for FIS files
                            Exit For
                        End If
                    Next
                End If
            End If

        End Sub

        ''' <summary>
        ''' Save the error surface properties for the selected survey method
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function SaveErrorProperties() As Boolean

            Dim r As DataGridViewSelectedRowCollection = grdErrorProperties.SelectedRows
            If r.Count = 1 Then
                ' Save just the survey method that is selected in the left grid
                Dim sSurveyMethod As String = r.Item(0).Cells(0).Value
                If (m_dErrorCalculationProperties.ContainsKey(sSurveyMethod)) Then
                    'Remove any existing properties for this survey method
                    m_dErrorCalculationProperties.Remove(sSurveyMethod)
                End If

                If rdoUniform.Checked Then
                    ' Create a new Uniform error properties
                    m_dErrorCalculationProperties(sSurveyMethod) = New ErrorCalcPropertiesUniform(sSurveyMethod, valUniform.Value)
                ElseIf rdoAssociated.Checked Then

                    If TypeOf cboAssociated.SelectedItem Is naru.db.NamedObject Then
                        ' Create a new associated surface error properties
                        m_dErrorCalculationProperties(sSurveyMethod) = New ErrorCalcPropertiesAssoc(sSurveyMethod, DirectCast(cboAssociated.SelectedItem, naru.db.NamedObject).ID)
                    Else
                        MessageBox.Show("You must select an associated surface that contains the error values for this error surface.", My.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return False
                    End If
                Else
                    ' Make sure the user has selected a GIS input
                    If cboFIS.SelectedIndex < 0 Then
                        MessageBox.Show("You must select a FIS rule file for this survey method or define it as uniform error.", My.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return False
                    Else
                        ' FIS. Loop through all the FIS inputs

                        Dim dInputs As New Dictionary(Of String, Integer)
                        For i = 0 To grdFISInputs.Rows.Count - 1
                            Dim cboAssoc As DataGridViewComboBoxCell = grdFISInputs.Rows(i).Cells(1)
                            If cboAssoc.Value > 0 Then
                                dInputs.Add(grdFISInputs.Rows(i).Cells(0).Value, cboAssoc.Value)
                            Else
                                MessageBox.Show("You must choose an associated surface for each FIS input.", My.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Return False
                            End If
                        Next

                        ' Find the matching fis library file
                        For Each rFIS As GCDLib.FISLibrary.FISTableRow In Core.GCDProject.ProjectManager.fisds.FISTable
                            If String.Compare(rFIS.Name, cboFIS.Text, True) = 0 Then
                                m_dErrorCalculationProperties(sSurveyMethod) = New ErrorCalcPropertiesFIS(sSurveyMethod, rFIS.FISID, rFIS.Path, dInputs)
                                Exit For
                            End If
                        Next
                    End If
                End If

                r.Item(0).Cells(1).Value = m_dErrorCalculationProperties(sSurveyMethod).ErrorType

            End If

            Return True

        End Function

        ''' <summary>
        ''' The error surface name has changed. Update the output raster path.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub txtName_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtName.TextChanged

            If TypeOf m_rErrorSurface Is ProjectDS.ErrorSurfaceRow Then
                txtRasterPath.Text = m_rErrorSurface.Source
            Else
                Dim sRasterPath As String = String.Empty
                If Not String.IsNullOrEmpty(txtName.Text) Then
                    sRasterPath = Core.GCDProject.ProjectManagerBase.OutputManager.ErrorSurfaceRasterPath(m_rDEMSurvey.Name, txtName.Text, False)
                    sRasterPath = Core.GCDProject.ProjectManagerBase.GetRelativePath(sRasterPath)
                End If
                txtRasterPath.Text = sRasterPath
            End If
        End Sub

        Private Sub btnHelp_Click(sender As System.Object, e As System.EventArgs) Handles btnHelp.Click
            Process.Start(My.Resources.HelpBaseURL & "gcd-command-reference/gcd-project-explorer/g-error-surfaces-context-menu/ii-derive-error-surface")
        End Sub

        Private Function ValidateForm() As Boolean

            If (String.IsNullOrEmpty(txtName.Text)) Then
                MessageBox.Show("You must provide a unique name for the error surface.", My.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            Else
                Dim bUnique As Boolean = True
                Dim nErrorSurfaceID As Integer = 0
                If TypeOf m_rErrorSurface Is ProjectDS.ErrorSurfaceRow Then
                    nErrorSurfaceID = m_rErrorSurface.ErrorSurfaceID
                End If

                ' The error surface name must be unique for the current DEM Survey
                For Each rError As ProjectDS.ErrorSurfaceRow In m_rDEMSurvey.GetErrorSurfaceRows
                    If rError.ErrorSurfaceID <> nErrorSurfaceID Then
                        If String.Compare(rError.Name, txtName.Text, True) = 0 Then
                            MessageBox.Show("There is another error surface for this DEM Survey that already possesses this name. You must provide a unique name for the error surface.", My.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Return False
                        End If
                    End If
                Next
            End If

            Return True
        End Function

        ''' <summary>
        ''' Determine the error surface type (Uniform, FIS, Multiple)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>If there's just one of a particular error surface type, then use that type.
        ''' IF there are instances of both types, or more than one of either then it must be multiple</remarks>
        Private ReadOnly Property ErrorSurfaceType As String
            Get
                Dim sType As String = ErrorSurfaceEngine.UniformErrorString

                Dim nFISCount As Integer = 0
                Dim nUniCount As Integer = 0
                Dim nAssoc As Integer = 0

                For Each errProp As ErrorCalcPropertiesBase In m_dErrorCalculationProperties.Values
                    If TypeOf errProp Is ErrorCalcPropertiesUniform Then
                        nUniCount += 1
                    ElseIf TypeOf errProp Is ErrorCalcPropertiesAssoc Then
                        nAssoc += 1
                    Else
                        nFISCount += 1
                    End If
                Next

                If nUniCount = 1 AndAlso nFISCount = 0 AndAlso nAssoc = 0 Then
                    sType = ErrorSurfaceEngine.UniformErrorString
                ElseIf nFISCount = 1 AndAlso nUniCount = 0 AndAlso nAssoc = 0 Then
                    sType = ErrorSurfaceEngine.FISErrorType
                ElseIf nAssoc = 1 AndAlso nUniCount = 0 AndAlso nFISCount = 0 Then
                    sType = ErrorSurfaceEngine.AssociatedsurfaceErrorType
                Else
                    sType = ErrorSurfaceEngine.MultipleErrorType
                End If

                Return sType
            End Get

        End Property

        Private Sub btnOK_Click(sender As Object, e As System.EventArgs) Handles btnOK.Click

            Cursor.Current = Cursors.WaitCursor

            If Not ValidateForm() Then
                Me.DialogResult = DialogResult.None
                Exit Sub
            End If

            ' Need to save the current error properties first.
            If Not SaveErrorProperties() Then
                Me.DialogResult = DialogResult.None
                Exit Sub
            End If

            If TypeOf m_rErrorSurface Is ProjectDS.ErrorSurfaceRow Then
                m_rErrorSurface.Name = txtName.Text
            Else
                ' New error surface. Save it to the database
                m_rErrorSurface = Core.GCDProject.ProjectManagerBase.ds.ErrorSurface.NewErrorSurfaceRow
                m_rErrorSurface.Name = txtName.Text
                m_rErrorSurface.DEMSurveyRow = m_rDEMSurvey
                m_rErrorSurface.Source = txtRasterPath.Text
                m_rErrorSurface.Type = ErrorSurfaceType
                Core.GCDProject.ProjectManagerBase.ds.ErrorSurface.AddErrorSurfaceRow(m_rErrorSurface)

                ' Add all the survey methods to the database
                For Each sSurveyMethod As String In m_dErrorCalculationProperties.Keys
                    Dim fError As Double = 0
                    Dim nAssociatedSurfaceID As Integer = 0

                    If TypeOf m_dErrorCalculationProperties(sSurveyMethod) Is ErrorCalcPropertiesUniform Then
                        fError = DirectCast(m_dErrorCalculationProperties(sSurveyMethod), ErrorCalcPropertiesUniform).UniformErrorValue
                    End If

                    If TypeOf m_dErrorCalculationProperties(sSurveyMethod) Is ErrorCalcPropertiesAssoc Then
                        nAssociatedSurfaceID = DirectCast(m_dErrorCalculationProperties(sSurveyMethod), ErrorCalcPropertiesAssoc).AssociatedSurfaceID
                    End If

                    ' This error type appears to be not used when uniform, but when it's FIS it should be the name of the FIS rule
                    ' file
                    Dim sErrorType As String = m_dErrorCalculationProperties(sSurveyMethod).ErrorType
                    If TypeOf m_dErrorCalculationProperties(sSurveyMethod) Is ErrorCalcPropertiesFIS Then
                        sErrorType = DirectCast(m_dErrorCalculationProperties(sSurveyMethod), ErrorCalcPropertiesFIS).FISRuleName
                    End If

                    Core.GCDProject.ProjectManagerBase.ds.MultiErrorProperties.AddMultiErrorPropertiesRow(sSurveyMethod, fError, m_rErrorSurface, sErrorType, nAssociatedSurfaceID) ' m_dErrorCalculationProperties(sSurveyMethod).ErrorType)

                    ' Now add all the FIS inputs to the FIS table
                    If TypeOf m_dErrorCalculationProperties(sSurveyMethod) Is ErrorCalcPropertiesFIS Then
                        Dim errProps As ErrorCalcPropertiesFIS = m_dErrorCalculationProperties(sSurveyMethod)
                        For Each sInput As String In errProps.FISInputs.Keys
                            ' Now find the associated surface for this input
                            For Each rAssoc As ProjectDS.AssociatedSurfaceRow In m_rDEMSurvey.GetAssociatedSurfaceRows
                                If rAssoc.AssociatedSurfaceID = errProps.FISInputs(sInput) Then
                                    Core.GCDProject.ProjectManagerBase.ds.FISInputs.AddFISInputsRow(m_rErrorSurface, sInput, rAssoc.Name, errProps.FISRuleName)
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

                    If My.Settings.AddOutputLayersToMap Then
                        ' TODO 
                        Throw New Exception("not implemented")
                        'Core.GCDProject.ProjectManagerUI.ArcMapManager.AddErrSurface(m_rErrorSurface)
                    End If
                Catch ex As Exception
                    DialogResult = DialogResult.None
                    Cursor.Current = Cursors.Default
                    Core.GCDProject.ProjectManagerBase.ds.ErrorSurface.RemoveErrorSurfaceRow(m_rErrorSurface)
                    Dim ex2 As New Exception("Error generating error surface raster. No changes were made to the GCD project.", ex)
                    naru.error.ExceptionUI.HandleException(ex2)
                    Exit Sub
                End Try
            End If

            ' Now save the GCD project
            Try
                Core.GCDProject.ProjectManagerBase.save()
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
                        sInputs &= sFISInput & ";" & Core.GCDProject.ProjectManager.GetAbsolutePath(rAssoc.Source) & ";"
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