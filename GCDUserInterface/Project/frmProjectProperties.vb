Imports System.Windows.Forms

Namespace Project

    Public Class frmProjectProperties

        Private m_bCreateMode As Boolean

        Public Sub New(bCreateMode As Boolean)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
            m_bCreateMode = bCreateMode
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

            ttpTooltip.SetToolTip(btnHelp, My.Resources.ttpHelp)
            ttpTooltip.SetToolTip(txtName, "The name for the GCD project. The name will be used in the folder path for the GCD project parent directory.")
            ttpTooltip.SetToolTip(txtDirectory, "The parent folder under which the GCD project folder will be created.")
            ttpTooltip.SetToolTip(btnBrowseOutput, "Browse and select a parent directory for the GCD Project.")
            ttpTooltip.SetToolTip(txtGCDPath, "Read only folder and file name of the GCD Project file.")
            ttpTooltip.SetToolTip(txtDescription, "Information about the GCD project.")
            ttpTooltip.SetToolTip(cboDisplayUnits, "The default units for displaying and outputting change detection results.")
            ttpTooltip.SetToolTip(valPrecision, "The number of decimal places used to round raster cell coordinates when determining divisible raster extents.")

            cboDisplayUnits.Items.Add(New naru.db.NamedObject(UnitsNet.Units.LengthUnit.Millimeter, UnitsNet.Length.GetAbbreviation(UnitsNet.Units.LengthUnit.Millimeter)))
            cboDisplayUnits.Items.Add(New naru.db.NamedObject(UnitsNet.Units.LengthUnit.Centimeter, UnitsNet.Length.GetAbbreviation(UnitsNet.Units.LengthUnit.Centimeter)))
            cboDisplayUnits.SelectedIndex = cboDisplayUnits.Items.Add(New naru.db.NamedObject(UnitsNet.Units.LengthUnit.Meter, UnitsNet.Length.GetAbbreviation(UnitsNet.Units.LengthUnit.Meter)))
            cboDisplayUnits.Items.Add(New naru.db.NamedObject(UnitsNet.Units.LengthUnit.Kilometer, UnitsNet.Length.GetAbbreviation(UnitsNet.Units.LengthUnit.Kilometer)))
            cboDisplayUnits.Items.Add(New naru.db.NamedObject(UnitsNet.Units.LengthUnit.Inch, UnitsNet.Length.GetAbbreviation(UnitsNet.Units.LengthUnit.Inch)))
            cboDisplayUnits.Items.Add(New naru.db.NamedObject(UnitsNet.Units.LengthUnit.Foot, UnitsNet.Length.GetAbbreviation(UnitsNet.Units.LengthUnit.Foot)))
            cboDisplayUnits.Items.Add(New naru.db.NamedObject(UnitsNet.Units.LengthUnit.Yard, UnitsNet.Length.GetAbbreviation(UnitsNet.Units.LengthUnit.Yard)))
            cboDisplayUnits.Items.Add(New naru.db.NamedObject(UnitsNet.Units.LengthUnit.Mile, UnitsNet.Length.GetAbbreviation(UnitsNet.Units.LengthUnit.Mile)))

            If m_bCreateMode Then
                Me.Text = "Create New " & Me.Text

                ' Default the directory to the parent folder of the last project used.
                If Not String.IsNullOrEmpty(GCDCore.Properties.Settings.Default.LastUsedProjectFolder) Then
                    Dim sDir As String = IO.Path.GetDirectoryName(GCDCore.Properties.Settings.Default.LastUsedProjectFolder)
                    If IO.Directory.Exists(sDir) Then
                        txtDirectory.Text = sDir
                    End If
                End If
            Else
                Me.Text = Me.Text & " Properties"

                With ProjectManager.Project
                    txtName.Text = .Name
                    txtDirectory.Text = .ProjectFile.DirectoryName
                    txtGCDPath.Text = .ProjectFile.FullName
                    txtDescription.Text = .Description
                    valPrecision.Value = .Precision
                End With

                '' Select the appropriate linear units
                'If Not String.IsNullOrEmpty(theProjectRow.DisplayUnits) Then
                '    For j As Integer = 0 To cboDisplayUnits.Items.Count - 1
                '        If String.Compare(cboDisplayUnits.Items(j).ToString, theProjectRow.DisplayUnits, True) = 0 Then
                '            cboDisplayUnits.SelectedIndex = j
                '        End If
                '    Next
                'End If

                ' Adjust the controls to be read/write only for editing
                txtName.ReadOnly = True
                btnBrowseOutput.Visible = False
                txtDirectory.Width = txtName.Width
                txtDirectory.ReadOnly = True
                valPrecision.Enabled = False
            End If
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

                    Dim project As New GCDProject(txtName.Text, txtDescription.Text, projectFile, DateTime.Now,
                                                  Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString, )


                Else
                    ' Editing properties of existing project
                    ProjectManager.Project.Name = txtName.Text
                    ProjectManager.Project.Description = txtDescription.Text
                End If

                ProjectManager.save()

            Catch ex As Exception
                ex.Data("Project Name") = txtName.Text
                ex.Data("XML File") = txtGCDPath.Text
                ex.Data("Directory") = txtDirectory.Text
                naru.error.ExceptionUI.HandleException(ex, "An error occured while trying to save the information")
            End Try

        End Sub

        Private Function ValidateForm() As Boolean

            If String.IsNullOrEmpty(txtName.Text) Then
                MessageBox.Show("Please enter a name for the new project.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            If String.IsNullOrEmpty(txtDirectory.Text.Length) Then
                MessageBox.Show("Please select an output directory.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            Else
                If Not IO.Directory.Exists(txtDirectory.Text) Then
                    MessageBox.Show("The parent directory must be valid, existing directory.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
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

        Private Sub cmdHelpPrecision_Click(sender As System.Object, e As System.EventArgs) Handles cmdHelpPrecision.Click
            Dim frm As New UtilityForms.frmInformation
            frm.InitializeFixedDialog("Horizontal Decimal Precision", GCDCore.Properties.Resources.PrecisionHelp)
            frm.ShowDialog()
        End Sub
    End Class

End Namespace