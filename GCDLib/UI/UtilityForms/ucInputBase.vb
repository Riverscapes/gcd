Namespace UI.UtilityForms

    Public MustInherit Class ucInputBase

        Private m_sNoun As String

#Region "Properties"

        Public Property Noun As String
            Get
                Return m_sNoun
            End Get
            Set(ByVal value As String)
                If String.IsNullOrEmpty(value) Then
                    m_sNoun = String.Empty
                Else
                    m_sNoun = value.Trim
                    tTip.SetToolTip(cmdBrowse, naru.ui.UIHelpers.WrapMessageWithNoun("Browse and select a", Noun, " feature class"))
                End If
            End Set
        End Property

        Public ReadOnly Property SelectedItem As Core.GISDataStructures.GISDataSource
            Get
                Dim gResult As Core.GISDataStructures.GISDataSource = Nothing
                If Core.GISDataStructures.GISDataSource.Exists(txtPath.Text) Then
                    gResult = New Core.GISDataStructures.GISDataSource(txtPath.Text)
                End If
                Return gResult
            End Get
        End Property

#End Region

#Region "Methods"

        Public Sub Initialize(ByVal sNoun As String)
            Noun = sNoun
        End Sub

        Public MustOverride Shadows Function Validate() As Boolean

        Public Sub txtPath_TextChanged(sender As Object, e As EventArgs) Handles txtPath.TextChanged
            If Not Validate() Then
                txtPath.Text = String.Empty
            End If
        End Sub


        ''' <summary>
        ''' Specify the item that will be added to the dropdown and selected in the dropdown
        ''' </summary>
        ''' <param name="gGISDataSource"></param>
        ''' <remarks>Use this method if the form needs to be be re-loaded. The specified item
        ''' will be added to the dropdown and then selected when the form is shown.</remarks>
        Public Sub PreSelect(ByVal gGISDataSource As Core.GISDataStructures.GISDataSource)
            'm_gPreSelect = gGISDataSource
        End Sub

#End Region

#Region "Events"

        Public Event WorkspaceChanged(ByVal Sender As System.Object, ByVal e As InputUCWorkspaceChangedEventArgs)

        Public Event SelectedItemChanged(ByVal Sender As System.Object, ByVal e As InputUCSelectedItemChangedEventArgs)

#End Region

    End Class

    ''' <summary>
    ''' Event arguments for the input user control when the workspace changes
    ''' </summary>
    ''' <remarks>http://msdn.microsoft.com/en-us/library/aa302342.aspx</remarks>
    Public Class InputUCWorkspaceChangedEventArgs
        Inherits System.EventArgs

        Private m_sWorkspacePath As String

        ''' <summary>
        ''' The workspace path for the selected data source
        ''' </summary>
        ''' <value></value>
        ''' <returns>The workspace path for the selected data source</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property WorkspacePath As String
            Get
                Return m_sWorkspacePath
            End Get
        End Property

        ''' <summary>
        ''' Creates a new input user control event args
        ''' </summary>
        ''' <param name="sWorkspacePath">The workspace path for the selected data source</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal sWorkspacePath As String)
            m_sWorkspacePath = sWorkspacePath
        End Sub
    End Class

    ''' <summary>
    ''' Event arguments for the input user control when the selected item changes
    ''' </summary>
    ''' <remarks>http://msdn.microsoft.com/en-us/library/aa302342.aspx</remarks>
    Public Class InputUCSelectedItemChangedEventArgs
        Inherits System.EventArgs

        Private m_sFullPath As String

        ''' <summary>
        ''' The workspace path for the selected data source
        ''' </summary>
        ''' <value></value>
        ''' <returns>The workspace path for the selected data source</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property SelectedItemFullPath As String
            Get
                Return m_sFullPath
            End Get
        End Property

        ''' <summary>
        ''' Creates a new input user control event args
        ''' </summary>
        ''' <param name="sFullPath">The full path for the selected data source</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal sFullPath As String)
            m_sFullPath = sFullPath
        End Sub

        Public Class BrowseLayerEventArgs
            Inherits System.EventArgs

            Private m_sFormTitle As String
            Private m_eBrowseType As Core.GISDataStructures.BrowseGISTypes
            Private m_sExistingPath As String

            Public ReadOnly Property FormTitle As String
                Get
                    Return m_sFormTitle
                End Get
            End Property

            Public ReadOnly Property BrowseType As Core.GISDataStructures.BrowseGISTypes
                Get
                    Return m_eBrowseType
                End Get
            End Property

            Public ReadOnly Property ExistingPath As String
                Get
                    Return m_sExistingPath
                End Get
            End Property

            Public Sub New(sFormTitle As String, eBrowseType As Core.GISDataStructures.BrowseGISTypes, Optional sExistingPath As String = "")
                m_sFormTitle = sFormTitle
                m_eBrowseType = eBrowseType
                m_sExistingPath = sExistingPath
            End Sub

        End Class

    End Class

End Namespace