Imports System.Windows.Forms

Namespace UI.Project

    Public Class frmProjectProperties

        Public Enum DisplayModes
            Edit
            Create
        End Enum

        Private m_eDisplayMode As DisplayModes

        Public ReadOnly Property DisplayMode As DisplayModes
            Get
                Return m_eDisplayMode
            End Get
        End Property

        Public Sub New(eMode As DisplayModes)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
            m_eDisplayMode = eMode
        End Sub


#Region "Events"

        Private Sub CreateNewProjectForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


            If Core.WorkspaceManager.WorkspacePath.Contains(" ") Then
                MessageBox.Show(String.Format("The specified temp workspace directory contains spaces ({0}). You must specify a temp workspace that does not contain spaces or punctuation characters in the GCD Options before you create or open a GCD project.", Core.WorkspaceManager.WorkspacePath), My.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Me.Close()
            Else
                If Not System.IO.Directory.Exists(Core.WorkspaceManager.WorkspacePath) Then
                    MessageBox.Show("The temporary workspace directory does not exist. Change the temporary workspace path in GCD Options before creating or opening a GCD project.", My.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.Close()
                End If
            End If

            ttpTooltip.SetToolTip(btnHelp, My.Resources.ttpHelp)
            ttpTooltip.SetToolTip(txtName, "The name for the GCD project. The name will be used in the folder path for the GCD project parent directory.")
            ttpTooltip.SetToolTip(txtDirectory, "The parent folder under which the GCD project folder will be created.")
            ttpTooltip.SetToolTip(btnBrowseOutput, "Browse and select a parent directory for the GCD Project.")
            ttpTooltip.SetToolTip(txtGCDPath, "Read only folder and file name of the GCD Project file.")
            ttpTooltip.SetToolTip(txtDescription, "Information about the GCD project.")
            ttpTooltip.SetToolTip(cboDisplayUnits, "The default units for displaying and outputting change detection results.")
            ttpTooltip.SetToolTip(valPrecision, "The number of decimal places used to round raster cell coordinates when determining divisible raster extents.")

            cboDisplayUnits.Items.Add(New UnitsNet.Units.LengthUnit("millimeters (mm)", UnitsNet.Units.LengthUnit.mm))
            cboDisplayUnits.Items.Add(New UnitsNet.Units.LengthUnit("centimeters (cm)", UnitsNet.Units.LengthUnit.cm))
            Dim i As Integer = cboDisplayUnits.Items.Add(New UnitsNet.Units.LengthUnit("meters (m)", UnitsNet.Units.LengthUnit.m))
            cboDisplayUnits.Items.Add(New UnitsNet.Units.LengthUnit("kilometers (km)", UnitsNet.Units.LengthUnit.km))
            cboDisplayUnits.Items.Add(New UnitsNet.Units.LengthUnit("inches (in)", UnitsNet.Units.LengthUnit.inch))
            cboDisplayUnits.Items.Add(New UnitsNet.Units.LengthUnit("feet (ft)", UnitsNet.Units.LengthUnit.ft))
            cboDisplayUnits.Items.Add(New UnitsNet.Units.LengthUnit("yards (yd)", UnitsNet.Units.LengthUnit.yard))
            cboDisplayUnits.Items.Add(New UnitsNet.Units.LengthUnit("miles (mi)", UnitsNet.Units.LengthUnit.mile))
            cboDisplayUnits.SelectedIndex = i

            If DisplayMode = DisplayModes.Create Then
                Me.Text = "Create New " & Me.Text

                ' Default the directory to the parent folder of the last
                ' project used.
                If Not String.IsNullOrEmpty(My.Settings.LastUsedProjectFolder) Then
                    Dim sDir As String = IO.Path.GetDirectoryName(My.Settings.LastUsedProjectFolder)
                    If IO.Directory.Exists(sDir) Then
                        txtDirectory.Text = sDir
                    End If
                End If
            Else
                Me.Text = Me.Text & " Properties"

                Dim theProjectRow As ProjectDS.ProjectRow = Core.GCDProject.ProjectManager.CurrentProject
                txtName.Text = theProjectRow.Name
                txtDirectory.Text = theProjectRow.OutputDirectory
                txtGCDPath.Text = Core.GCDProject.ProjectManager.FilePath
                txtDescription.Text = theProjectRow.Description
                valPrecision.Value = theProjectRow.Precision

                ' Select the appropriate linear units
                If Not String.IsNullOrEmpty(theProjectRow.DisplayUnits) Then
                    For j As Integer = 0 To cboDisplayUnits.Items.Count - 1
                        If String.Compare(cboDisplayUnits.Items(j).LinearUnit.ToString, theProjectRow.DisplayUnits, True) = 0 Then
                            cboDisplayUnits.SelectedIndex = j
                        End If
                    Next
                End If

                ' Adjust the controls to be read/write only for editing
                txtName.ReadOnly = True
                btnBrowseOutput.Visible = False
                txtDirectory.Width = txtName.Width
                txtDirectory.ReadOnly = True
                valPrecision.Enabled = False
            End If
        End Sub

        Private Sub btnBrowseOutput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseOutput.Click

            If naru.os.Folder.BrowseFolder(txtDirectory, "Select the GCD Project Parent Directory", My.Settings.LastUsedProjectFolder) <> DialogResult.OK Then
                Exit Sub
            End If

            Dim dFolder As New IO.DirectoryInfo(txtDirectory.Text)
            If dFolder.Exists Then
                Dim diar1 As IO.FileInfo() = dFolder.GetFiles()
                Dim dra As IO.FileInfo

                'list the names of all files in the specified directory
                For Each dra In diar1
                    If String.Compare(dra.Extension, ".gcd", True) = 0 Then

                        MsgBox("The selected folder already contains another GCD project." & vbNewLine &
                            "Each GCD project must be created in a separate folder.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                        Exit Sub
                    End If
                Next

                If String.IsNullOrEmpty(txtName.Text) Then
                    txtName.Text = IO.Path.GetFileName(txtDirectory.Text)
                    btnOK.Focus()
                End If
            Else
                MsgBox("The selected folder does not exist. The project folder must exist before the project is created.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
            End If

        End Sub

        Private Sub UpdateGCDPath()

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
                Me.DialogResult = System.Windows.Forms.DialogResult.None
                Exit Sub
            End If

            IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(txtGCDPath.Text))
            My.Settings.LastUsedProjectFolder = txtDirectory.Text
            My.Settings.Save()
            '
            '
            Try
                If DisplayMode = DisplayModes.Create Then
                    Core.GCDProject.ProjectManagerBase.FilePath = txtGCDPath.Text
                    'ProjectManagerUI.ds.Project.AddProjectRow(txtName.Text, txtDescription.Text, txtDirectory.Text, Now, System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString, valPrecision.Value, DirectCast(cboDisplayUnits.SelectedItem, LinearUnitClass).LinearUnit.ToString)
                    Core.GCDProject.ProjectManagerBase.ds.Project.AddProjectRow(txtName.Text, txtDescription.Text, txtDirectory.Text, Now, System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString, valPrecision.Value, Nothing, Nothing, Core.GCDProject.ProjectManager.ProjectTypes.AddIn.ToString())
                    Try
                        ' TODO: was the following extension operation necessary? Surely the name and file can exist on the project manager and aren't need on the extension?
                        Throw New Exception("check code is not needed")
                        'Dim gcd As GCDExtension = GCDExtension.GetGCDExtension(My.ThisApplication)
                        'If TypeOf gcd Is GCDExtension Then
                        '    gcd.SetCurrentProject(txtName.Text, txtGCDPath.Text)
                        'End If

                        ' Remember this folder so that the next time "open project" is used it defaults to the location of this project
                        My.Settings.LastUsedProjectFolder = IO.Path.GetDirectoryName(txtGCDPath.Text)
                        My.Settings.Save()

                    Catch ex As Exception
                        ex.Data.Add("Project Name", txtName.Text)
                        ex.Data.Add("XML File", txtGCDPath.Text)
                        ex.Data.Add("Directory", txtDirectory.Text)
                        Core.ExceptionHelper.HandleException(ex, "An error occured while updating the extension with the latest project")
                    End Try

                Else
                    ' Editing properties of existing project
                    Dim theProjectRow As ProjectDS.ProjectRow = Core.GCDProject.ProjectManager.CurrentProject
                    theProjectRow.Name = txtName.Text
                    theProjectRow.Description = txtDescription.Text
                    'theProjectRow.DisplayUnits = DirectCast(cboDisplayUnits.SelectedItem, LinearUnitClass).LinearUnit.ToString
                End If

                Core.GCDProject.ProjectManager.save()

            Catch ex As Exception
                ex.Data.Add("Project Name", txtName.Text)
                ex.Data.Add("XML File", txtGCDPath.Text)
                ex.Data.Add("Directory", txtDirectory.Text)
                Core.ExceptionHelper.HandleException(ex, "An error occured while trying to save the information")
            End Try

        End Sub

        Private Function ValidateForm() As Boolean

            If String.IsNullOrEmpty(txtName.Text) Then
                MsgBox("Please enter a name for the new project.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                Return False
            End If

            If String.IsNullOrEmpty(txtDirectory.Text.Length) Then
                MsgBox("Please select an output directory.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                Return False
            Else
                If Not IO.Directory.Exists(txtDirectory.Text) Then
                    MsgBox("The parent directory must be valid, existing directory.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                    Return False
                End If
            End If

            ' Only check if the file exists when creating a new one.
            If DisplayMode = DisplayModes.Create Then
                If IO.File.Exists(txtGCDPath.Text) Then
                    MsgBox("There already appears to be a GCD project at the specified path. Change the project name or pick a different parent directory.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                    Return False
                End If
            End If

            Return True

        End Function

        Private Sub btnHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHelp.Click
            If m_eDisplayMode = DisplayModes.Create Then
                Process.Start(My.Resources.HelpBaseURL & "gcd-command-reference/project-menu/new-project")
            ElseIf m_eDisplayMode = DisplayModes.Edit Then
                Process.Start(My.Resources.HelpBaseURL & "gcd-command-reference/gcd-project-explorer/project-context-menu/edit-gcd-project-properties")
            End If
        End Sub

#End Region

        Private Sub txtName_TextChanged(sender As Object, e As System.EventArgs) Handles _
        txtName.TextChanged, txtDirectory.TextChanged

            UpdateGCDPath()
        End Sub

        Private Sub cmdHelpPrecision_Click(sender As System.Object, e As System.EventArgs) Handles cmdHelpPrecision.Click
            Dim frm As New UtilityForms.frmInformation
            frm.InitializeFixedDialog("Horizontal Decimal Precision", My.Resources.PrecisionHelp)
            frm.ShowDialog()
        End Sub
    End Class

End Namespace