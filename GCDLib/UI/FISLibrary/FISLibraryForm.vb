#Region "Code Comments"
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'       Author: Philip Bailey, Nick Ochoski, & Frank Poulsen
'               ESSA Software Ltd.
'               1765 W 8th Avenue
'               Vancouver, BC, Canada V6J 5C6
'     
'     Copyright: (C) 2011 by ESSA technologies Ltd.
'                This software is subject to copyright protection under the       
'                laws of Canada and other countries.
'
'  Date Created: 14 January 2011
'
'   Description: 
'
#End Region

#Region " Imports "
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Geodatabase
Imports System.Windows.Forms
Imports ESRI.ArcGIS.Geometry.esriGeometryType
Imports ESRI.ArcGIS.DataSourcesFile
Imports System.IO

#End Region

Public Class FISLibraryForm

#Region "Members"
    Private m_pApp As IApplication
    Private pMap As IMap
#End Region

#Region "Methods"
    Public Sub New(ByRef pApp As IApplication)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        m_pApp = pApp
        Dim pMXDoc As IMxDocument = m_pApp.Document
        pMap = pMXDoc.FocusMap
    End Sub
#End Region

    Private Sub btnAddFIS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddFIS.Click

        Dim AddFISForm As New AddFISForm
        AddFISForm.ShowDialog()

    End Sub

    Private Sub btnDeleteFIS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteFIS.Click


        Dim CurrentRow As DataRowView
        CurrentRow = FISTableBindingSource.Current

        If Not CurrentRow Is Nothing Then

            Dim response As MsgBoxResult
            response = MsgBox("Are you sure you want to remove the selected FIS file from the GCD Software? Note that this will not delete the associated *.fis file.", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, My.Resources.ApplicationNameLong)
            If response = MsgBoxResult.Yes Then
                If Not CurrentRow Is Nothing Then
                    'Delete the selected item from the dataset and write this new information to the XML file at the specified location
                    FISTableBindingSource.RemoveCurrent()
                    ProjectManager.saveFIS()
                End If
            End If
        End If

    End Sub

    Private Sub FISLibraryForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ttpTooltip.SetToolTip(btnAddFIS, "Add a FIS file to the GCD FIS Library.")
        ttpTooltip.SetToolTip(btnEditFIS, "Edit the selected FIS file.")
        ttpTooltip.SetToolTip(btnDeleteFIS, "Delete the selected FIS file.")

        FISTableBindingSource.DataSource = ProjectManager.fisds

        If DataGridView1.Columns.Count = 2 Then
            DataGridView1.Columns(1).Width = DataGridView1.Width - DataGridView1.Columns(0).Width - 5
        End If

        'XMLHandling.XMLReadFIS(Me.FISLibrary)

    End Sub

    Private Sub btnEditFIS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditFIS.Click

        Dim CurrentRow As DataRowView = FISTableBindingSource.Current
        If Not CurrentRow Is Nothing Then
            If TypeOf CurrentRow.Row Is FISLibrary.FISTableRow Then
                Dim fisRow As FISLibrary.FISTableRow = CurrentRow.Row
                If IO.File.Exists(fisRow.Path) Then
                    Try
                        Dim frm As New EditFISForm(fisRow.Path)
                        frm.ShowDialog()
                    Catch ex As Exception
                        Dim ex2 As New Exception("Error showing FIS form.", ex)
                        ex2.Data.Add("FIS Path", fisRow.Path)
                        Throw ex2
                    End Try
                Else
                    MsgBox("The specified FIS file does not exist.", MsgBoxStyle.Exclamation, My.Resources.ApplicationNameLong)
                End If
            End If
        End If
    End Sub

    Private Sub btnHelp_Click(sender As System.Object, e As System.EventArgs) Handles btnHelp.Click
        Process.Start(My.Resources.HelpBaseURL & "gcd-command-reference/customize-menu/fis-library")
    End Sub

    Private Sub btnFISRepo_Click(sender As System.Object, e As System.EventArgs) Handles btnFISRepo.Click

        Process.Start(My.Settings.FISRepositoryWebsite)

    End Sub
End Class