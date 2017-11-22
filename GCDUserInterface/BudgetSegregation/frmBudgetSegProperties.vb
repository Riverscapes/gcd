Imports System.Windows.Forms
Imports GCDCore.Project

Namespace UI.BudgetSegregation

    Public Class frmBudgetSegProperties

        Private m_BudgetSeg As GCDCore.Project.BudgetSegregation
        Private InitialDoD As DoDBase

        Public ReadOnly Property BudgetSeg As GCDCore.Project.BudgetSegregation
            Get
                Return m_BudgetSeg
            End Get
        End Property

        Public Sub New(parentDoD As DoDBase)

            ' This call is required by the designer.
            InitializeComponent()
            InitialDoD = parentDoD

            ucPolygon.Initialize("Budget Segregation Polygon Mask", GCDConsoleLib.GDalGeometryType.SimpleTypes.Polygon)
        End Sub

        Private Sub BudgetSegPropertiesForm_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            cboDoD.DataSource = ProjectManager.Project.DoDs.Values
            cboDoD.SelectedItem = InitialDoD

        End Sub

        Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click

            If Not ValidateForm() Then
                Me.DialogResult = DialogResult.None
                Exit Sub
            End If

            Try
                Cursor.Current = Cursors.WaitCursor

                Dim dod As DoDBase = DirectCast(cboDoD.SelectedItem, DoDBase)
                Dim bsFolder As IO.DirectoryInfo = ProjectManager.OutputManager.GetBudgetSegreationDirectoryPath(dod.Folder, True)

                Dim bsEngine As New GCDCore.Engines.BudgetSegregationEngine(txtName.Text, bsFolder)
                m_BudgetSeg = bsEngine.Calculate(dod, ucPolygon.SelectedItem, cboField.Text)

                ProjectManager.Project.Save()

            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            Finally
                Cursor.Current = Cursors.Default
            End Try

        End Sub

        Private Function ValidateForm() As Boolean

            ' Sanity check to avoid names with only empty spaces
            txtName.Text = txtName.Text.Trim()

            If Not TypeOf cboDoD.SelectedItem Is DoDBase Then
                MessageBox.Show("Please choose a change detection analysis on which you want to base this budget segregation.", "Missing Change Detection", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If


            If String.IsNullOrEmpty(txtName.Text) Then
                MsgBox("Please enter a name for the budget segregation analysis.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                Return Nothing
            Else
                If Not DirectCast(cboDoD.SelectedItem, DoDBase).IsBudgetSegNameUnique(txtName.Text, Nothing) Then
                    MsgBox("Another budget segregation already uses the name '" & txtName.Text & "'. Please choose a unique name.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                    Return False
                End If
            End If

            If TypeOf ucPolygon.SelectedItem Is GCDConsoleLib.Vector Then

                If ucPolygon.SelectedItem.Features.Count < 1 Then
                    MsgBox("The polygon mask feature class is empty and contains no features. You must choose a polygon feature class with at least one feature.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                    Return False
                End If

                Dim dod As DoDBase = DirectCast(cboDoD.SelectedItem, DoDBase)

                If ucPolygon.SelectedItem.Proj.PrettyWkt.ToLower.Contains("unknown") Then
                    MsgBox("The selected feature class appears to be missing a spatial reference. All GCD inputs must possess a spatial reference and it must be the same spatial reference for all datasets in a GCD project." &
                      " If the selected feature class exists in the same coordinate system, " & dod.RawDoD.Raster.Proj.PrettyWkt & ", but the coordinate system has not yet been defined for the feature class." &
                      " Use the ArcToolbox 'Define Projection' geoprocessing tool in the 'Data Management -> Projection & Transformations' Toolbox to correct the problem with the selected datasets by defining the coordinate system as:" & vbCrLf & vbCrLf & dod.RawDoD.Raster.Proj.PrettyWkt & vbCrLf & vbCrLf & "Then try using it with the GCD again.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                    Return False
                Else
                    If Not dod.RawDoD.Raster.Proj.IsSame(ucPolygon.SelectedItem.Proj) Then
                        MsgBox("The coordinate system of the selected feature class:" & vbCrLf & vbCrLf & ucPolygon.SelectedItem.Proj.PrettyWkt & vbCrLf & vbCrLf & "does not match that of the GCD project:" & vbCrLf & vbCrLf & dod.RawDoD.Raster.Proj.PrettyWkt & vbCrLf & vbCrLf &
                         "All datasets within a GCD project must have the identical coordinate system. However, small discrepencies in coordinate system names might cause the two coordinate systems to appear different. " &
                         "If you believe that the selected dataset does in fact possess the same coordinate system as the GCD project then use the ArcToolbox 'Define Projection' geoprocessing tool in the " &
                         "'Data Management -> Projection & Transformations' Toolbox to correct the problem with the selected dataset by defining the coordinate system as:" & vbCrLf & vbCrLf & dod.RawDoD.Raster.Proj.PrettyWkt & vbCrLf & vbCrLf & "Then try importing it into the GCD again.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                        Return False
                    End If
                End If
            Else
                MsgBox("Please choose a polygon mask on which you wish to base this budget segregation.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                Return False
            End If

            If String.IsNullOrEmpty(cboField.Text) Then
                MsgBox("Please choose a polygon mask field. Or add a ""string"" field to the feature class if one does not exist.", MsgBoxStyle.Information, GCDCore.Properties.Resources.ApplicationNameLong)
                Return False
            End If

            Return True

        End Function

        Private Sub cboDoD_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cboDoD.SelectedIndexChanged

            If Not TypeOf cboDoD.SelectedItem Is DoDBase Then
                Return
            End If

            Dim dod As DoDBase = DirectCast(cboDoD.SelectedItem, DoDBase)

            txtNewDEM.Text = dod.NewDEM.Name
            txtOldDEM.Text = dod.OldDEM.Name

            If TypeOf dod Is DoDMinLoD Then
                txtUncertaintyAnalysis.Text = String.Format("Minimum Level of Detection at {0:#.00}{1}", DirectCast(dod, DoDMinLoD).Threshold, ProjectManager.Project.Units.VertUnit)
            ElseIf TypeOf dod Is DoDPropagated Then
                txtUncertaintyAnalysis.Text = "Propagated Error"
            Else
                txtUncertaintyAnalysis.Text = String.Format("Probabilistic at {0}% confidence level", DirectCast(dod, DoDProbabilistic).ConfidenceLevel * 100)
            End If

            txtOutputFolder.Text = ProjectManager.Project.GetRelativePath(ProjectManager.OutputManager.GetBudgetSegreationDirectoryPath(dod.Folder, False).FullName)

        End Sub

        Private Sub PolygonChanged(ByVal sender As Object, ByVal e As naru.ui.PathEventArgs) Handles ucPolygon.PathChanged

            cboField.Items.Clear()

            If Not TypeOf ucPolygon.SelectedItem Is GCDConsoleLib.Vector Then
                Return
            End If

            cboField.DataSource = ucPolygon.SelectedItem.Fields.Values.Where(Function(p) p.Type.Equals(GCDConsoleLib.GDalFieldType.StringField))

            If cboField.Items.Count > 0 Then
                cboField.SelectedIndex = 0
            End If

        End Sub

        Private Sub cmdHelp_Click(sender As System.Object, e As System.EventArgs) Handles cmdHelp.Click
            Process.Start(GCDCore.Properties.Resources.HelpBaseURL & "gcd-command-reference/gcd-project-explorer/l-individual-change-detection-context-menu/v-add-budget-segregation")
        End Sub
    End Class

End Namespace