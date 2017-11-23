Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Linq

Namespace Project

    Public Class frmProjectProperties

        Private m_bCreateMode As Boolean

        Private m_MetaData As BindingList(Of ProjectMetaData)

        Public Sub New(bCreateMode As Boolean)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
            m_bCreateMode = bCreateMode

            ' New empty list for metadata
            m_MetaData = New BindingList(Of ProjectMetaData)()

        End Sub

#Region "Events"

        Private Sub CreateNewProjectForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            If GCDCore.WorkspaceManager.WorkspacePath.Contains(" ") Then
                MessageBox.Show(String.Format("The specified temp workspace directory contains spaces ({0}). You must specify a temp workspace that does not contain spaces or punctuation characters in the GCD Options before you create or open a GCD project.", GCDCore.WorkspaceManager.WorkspacePath), GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                DialogResult = DialogResult.Abort
                Return
            Else
                If Not IO.Directory.Exists(GCDCore.WorkspaceManager.WorkspacePath) Then
                    MessageBox.Show("The temporary workspace directory does not exist. Change the temporary workspace path in GCD Options before creating or opening a GCD project.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    DialogResult = DialogResult.Abort
                    Return
                End If
            End If

            grdMetaData.AllowUserToResizeRows = False
            grdMetaData.AllowUserToOrderColumns = False

            SetToolTips()

            cboHorizontalUnits.DataSource = GCDUnits.GCDLinearUnitsAsString()
            cboVerticalUnits.DataSource = GCDUnits.GCDLinearUnitsAsString()
            cboAreaUnits.DataSource = GCDUnits.GCDAreaUnitsAsString()
            cboVolumeUnits.DataSource = GCDUnits.GCDVolumeUnitsAsString()

            If m_bCreateMode Then
                Me.Text = "Create New " & Me.Text

                ' Default the directory to the parent folder of the last project used.
                If Not String.IsNullOrEmpty(GCDCore.Properties.Settings.Default.LastUsedProjectFolder) Then
                    Dim sDir As String = IO.Path.GetDirectoryName(GCDCore.Properties.Settings.Default.LastUsedProjectFolder)
                    If IO.Directory.Exists(sDir) Then
                        txtDirectory.Text = sDir
                    End If
                End If

                cboHorizontalUnits.SelectedItem = "Meter"
                cboVerticalUnits.SelectedItem = "Meter"
                cboAreaUnits.SelectedItem = "SquareMeter"
                cboVolumeUnits.SelectedItem = "CubicMeter"
            Else
                Me.Text = Me.Text & " Properties"

                With ProjectManager.Project
                    txtName.Text = .Name
                    txtDirectory.Text = .ProjectFile.DirectoryName
                    txtGCDPath.Text = .ProjectFile.FullName
                    txtDescription.Text = .Description

                    cboHorizontalUnits.Text = .Units.HorizUnit.ToString()
                    cboVerticalUnits.Text = .Units.VertUnit.ToString()
                    cboAreaUnits.Text = .Units.ArUnit.ToString()
                    cboVolumeUnits.Text = .Units.VolUnit.ToString()

                    ' Only allow the units to be changed if no DEMs have been defined
                    Dim demCount As Integer = .DEMSurveys.Count
                    cboHorizontalUnits.Enabled = demCount = 0
                    cboVerticalUnits.Enabled = demCount = 0

                    ' Copy the project meta into the binding list
                    For Each kvp As KeyValuePair(Of String, String) In .MetaData
                        m_MetaData.Add(New ProjectMetaData(kvp.Key, kvp.Value))
                    Next
                End With

                ' Adjust the controls to be read/write only for editing
                txtName.ReadOnly = True
                btnBrowseOutput.Visible = False
                txtDirectory.Width = txtName.Width
                txtDirectory.ReadOnly = True

                ' Default focus to the OK button in edit mode
                btnOK.Select()
            End If

            ' Bind the data grid to the binding list
            grdMetaData.DataSource = m_MetaData

        End Sub

        Private Sub SetToolTips()
            ttpTooltip.SetToolTip(btnHelp, My.Resources.ttpHelp)
            ttpTooltip.SetToolTip(txtName, "The name for the GCD project. The name will be used in the folder path for the GCD project parent directory.")
            ttpTooltip.SetToolTip(txtDirectory, "The parent folder under which the GCD project folder will be created.")
            ttpTooltip.SetToolTip(btnBrowseOutput, "Browse and select a parent directory for the GCD Project.")
            ttpTooltip.SetToolTip(txtGCDPath, "Read only folder and file name of the GCD Project file.")
            ttpTooltip.SetToolTip(txtDescription, "Information about the GCD project.")
            ttpTooltip.SetToolTip(cboHorizontalUnits, "The default units for displaying and outputting change detection results.")

            ttpTooltip.SetToolTip(cboHorizontalUnits, "The horizontal, linear units of the raster datasets used in this project. i.e. this should be the same as the map coordinate units.")
            ttpTooltip.SetToolTip(cboVerticalUnits, "The vertical units of the raster datasets in this project. i.e. this should be the same the raster cell value units.")
            ttpTooltip.SetToolTip(cboAreaUnits, "The areal units for the project. This is typically the square equivalent of the horizontal units.")
            ttpTooltip.SetToolTip(cboVolumeUnits, "The volume units for the project. This is typically related to the vertical and areal units.")
        End Sub

        Private Sub btnBrowseOutput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseOutput.Click

            If naru.os.Folder.BrowseFolder(txtDirectory, "Select the GCD Project Parent Directory", GCDCore.Properties.Settings.Default.LastUsedProjectFolder) <> DialogResult.OK Then
                Exit Sub
            End If

            Dim dFolder As New IO.DirectoryInfo(txtDirectory.Text)
            If dFolder.Exists Then
                ' Folder with GCD projects already in them are not allowed
                If dFolder.GetFiles("*.gcd", IO.SearchOption.TopDirectoryOnly).Count > 0 Then
                    MessageBox.Show("The selected folder already contains another GCD project." & vbNewLine &
                        "Each GCD project must be created in a separate folder.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If

                If String.IsNullOrEmpty(txtName.Text) Then
                    txtName.Text = IO.Path.GetFileName(txtDirectory.Text)
                    btnOK.Focus()
                End If
            Else
                MessageBox.Show("The selected folder does not exist. The project folder must exist before the project is created.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        End Sub

        Private Sub UpdateGCDPath(sender As Object, e As System.EventArgs) Handles txtName.TextChanged, txtDirectory.TextChanged

            Dim sGCDPath As String = String.Empty
            If Not String.IsNullOrEmpty(txtName.Text) Then
                If Not String.IsNullOrEmpty(txtDirectory.Text) Then
                    If IO.Directory.Exists(txtDirectory.Text) Then
                        sGCDPath = IO.Path.Combine(txtDirectory.Text, naru.os.File.RemoveDangerousCharacters(txtName.Text))
                        sGCDPath = IO.Path.Combine(sGCDPath, naru.os.File.RemoveDangerousCharacters(txtName.Text))
                        sGCDPath = IO.Path.ChangeExtension(sGCDPath, "gcd")
                    End If
                End If
            End If
            txtGCDPath.Text = sGCDPath
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>PGB - 5 May 2011
        ''' Change this so that the XML file is created when the file is created. This is needed because 
        ''' the XML file stores the name of the project, which is required now for the toolbar.</remarks>
        Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

            If Not ValidateForm() Then
                Me.DialogResult = DialogResult.None
                Exit Sub
            End If

            Try
                GCDCore.Properties.Settings.Default.LastUsedProjectFolder = IO.Path.GetDirectoryName(txtGCDPath.Text)
                My.Settings.Save()

                If m_bCreateMode Then
                    ' Creating a new project
                    IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(txtGCDPath.Text))
                    Dim projectFile As New System.IO.FileInfo(txtGCDPath.Text)
                    Dim units As GCDConsoleLib.GCD.UnitGroup = GetSelectedUnits()

                    Dim project As New GCDProject(txtName.Text, txtDescription.Text, projectFile, DateTime.Now,
                                                  Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString, UnitsNet.Area.From(0, units.ArUnit), units)

                    UpdateProjectMetdata(project)

                    ProjectManager.CreateProject(project)
                Else
                    ' Editing properties of existing project
                    ProjectManager.Project.Name = txtName.Text
                    ProjectManager.Project.Description = txtDescription.Text

                    ' only allowed to change the units if there are no DEMs
                    If ProjectManager.Project.DEMSurveys.Count < 1 Then
                        ProjectManager.Project.Units = GetSelectedUnits()
                    End If

                    UpdateProjectMetdata(ProjectManager.Project)

                    ProjectManager.Project.Save()
                End If

            Catch ex As Exception
                DialogResult = DialogResult.None
                ex.Data("Project Name") = txtName.Text
                ex.Data("XML File") = txtGCDPath.Text
                ex.Data("Directory") = txtDirectory.Text
                naru.error.ExceptionUI.HandleException(ex, "An error occured while trying to save the information")
            End Try

        End Sub

        ''' <summary>
        ''' Call this when saving the form data to update the project with the contents of binding list from the data grid
        ''' </summary>
        ''' <param name="project"></param>
        Private Sub UpdateProjectMetdata(project As GCDProject)
            project.MetaData.Clear()
            For Each item As ProjectMetaData In m_MetaData
                project.MetaData(item.Key) = item.Value
            Next
        End Sub

        Private Function GetSelectedUnits() As GCDConsoleLib.GCD.UnitGroup
            Return New GCDConsoleLib.GCD.UnitGroup(
                        DirectCast([Enum].Parse(GetType(UnitsNet.Units.VolumeUnit), cboVolumeUnits.SelectedItem), UnitsNet.Units.VolumeUnit),
                        DirectCast([Enum].Parse(GetType(UnitsNet.Units.AreaUnit), cboAreaUnits.SelectedItem), UnitsNet.Units.AreaUnit),
                        DirectCast([Enum].Parse(GetType(UnitsNet.Units.LengthUnit), cboVerticalUnits.SelectedItem), UnitsNet.Units.LengthUnit),
                        DirectCast([Enum].Parse(GetType(UnitsNet.Units.LengthUnit), cboHorizontalUnits.SelectedItem), UnitsNet.Units.LengthUnit))
        End Function

        Private Function ValidateForm() As Boolean

            If String.IsNullOrEmpty(txtName.Text) Then
                MessageBox.Show("Please enter a name for the new project.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtName.Select()
                Return False
            End If

            If String.IsNullOrEmpty(txtDirectory.Text.Length) Then
                MessageBox.Show("Please select an output directory.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                btnBrowseOutput.Select()
                Return False
            Else
                If Not IO.Directory.Exists(txtDirectory.Text) Then
                    MessageBox.Show("The parent directory must be valid, existing directory.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    btnBrowseOutput.Select()
                    Return False
                End If
            End If

            ' Only check if the file exists when creating a new one.
            If m_bCreateMode Then
                If IO.File.Exists(txtGCDPath.Text) Then
                    MessageBox.Show("There already appears to be a GCD project at the specified path. Change the project name or pick a different parent directory.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            End If

            Return True

        End Function

        Private Sub btnHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHelp.Click
            If m_bCreateMode Then
                Process.Start(GCDCore.Properties.Resources.HelpBaseURL & "gcd-command-reference/project-menu/new-project")
            Else
                Process.Start(GCDCore.Properties.Resources.HelpBaseURL & "gcd-command-reference/gcd-project-explorer/project-context-menu/edit-gcd-project-properties")
            End If
        End Sub

#End Region

        Private Sub cmdHelpPrecision_Click(sender As System.Object, e As System.EventArgs)
            Dim frm As New UtilityForms.frmInformation
            frm.InitializeFixedDialog("Horizontal Decimal Precision", GCDCore.Properties.Resources.PrecisionHelp)
            frm.ShowDialog()
        End Sub

        Private Class ProjectMetaData

            Private m_Key As String
            Private m_Value As String

            Public Property Key As String
                Get
                    Return m_Key
                End Get
                Set(value As String)
                    m_Key = value
                End Set
            End Property

            Public Property Value As String
                Get
                    Return m_Value
                End Get
                Set(value As String)
                    m_Value = value
                End Set
            End Property

            Public Sub New()

            End Sub

            Public Sub New(sKey As String, sValue As String)
                Key = sKey
                Value = sValue
            End Sub

        End Class

        Private Sub frmProjectProperties_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
            If e.KeyCode = Keys.F1 Then
                btnHelp.PerformClick()
            End If
        End Sub
    End Class

End Namespace