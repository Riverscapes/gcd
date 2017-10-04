Imports ESRI.ArcGIS.Framework

Namespace UI.UtilityForms

    Public MustInherit Class ucInputBase

        Private m_sNoun As String
        Protected m_gPreSelect As GISDataStructures.GISDataSource

#Region "Properties"

        Public Property ArcMap As ESRI.ArcGIS.Framework.IApplication
            Get
                Return m_pArcMap
            End Get
            Set(ByVal value As ESRI.ArcGIS.Framework.IApplication)
                m_pArcMap = value
            End Set
        End Property

        Public Property Noun As String
            Get
                Return m_sNoun
            End Get
            Set(ByVal value As String)
                If String.IsNullOrEmpty(value) Then
                    m_sNoun = String.Empty
                Else
                    m_sNoun = value.Trim
                    tTip.SetToolTip(cmdBrowse, GISCode.UserInterface.WrapMessageWithNoun("Browse and select a", Noun, " feature class"))
                End If
            End Set
        End Property

        Public ReadOnly Property SelectedItem As GISDataStructures.GISDataSource
            Get
                Dim gResult As GISDataStructures.GISDataSource = Nothing
                If TypeOf cboInput.SelectedItem Is GISDataStructures.GISDataSource Then
                    gResult = DirectCast(cboInput.SelectedItem, GISDataStructures.GISDataSource)
                End If
                Return gResult
            End Get
        End Property

        Public Property ToolTip As String
            Get
                Return tTip.GetToolTip(cboInput)
            End Get
            Set(ByVal value As String)
                tTip.SetToolTip(cboInput, value)
            End Set
        End Property

#End Region

#Region "Methods"

        Private Sub cmdBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBrowse.Click

            If String.IsNullOrEmpty(Noun) Then
                Throw New Exception("You must provide the ""noun"" for this vector input user control before using the browse event")
            End If

            Browse()

        End Sub

        Public Sub Initialize(ByVal sNoun As String)
            Noun = sNoun
        End Sub

        Protected MustOverride Sub Browse()

        Public MustOverride Shadows Function Validate() As Boolean

        Public Function AddSelectedItemToArcMap() As ESRI.ArcGIS.Carto.IFeatureLayer

            If Not TypeOf m_pArcMap Is ESRI.ArcGIS.Framework.IApplication Then
                Throw New Exception("You must provide the pointer to the ArcMAp application before this control loads")
            End If

            Dim pFLayer As ESRI.ArcGIS.Carto.IFeatureLayer = Nothing
            Dim gVector As GISDataStructures.VectorDataSource = SelectedItem
            If TypeOf gVector Is GISDataStructures.VectorDataSource Then
                pFLayer = gVector.AddToMap(ArcMap)
            End If

            Return pFLayer

        End Function

        Public Function AddToMap() As ESRI.ArcGIS.Carto.ILayer

            Dim pLayer As ESRI.ArcGIS.Carto.ILayer = Nothing
            If TypeOf ArcMap Is IApplication Then
                If TypeOf SelectedItem Is GISDataStructures.GISDataSource Then
                    pLayer = SelectedItem.AddToMap(ArcMap)
                End If
            End If

            Return pLayer

        End Function

        ''' <summary>
        ''' Specify the item that will be added to the dropdown and selected in the dropdown
        ''' </summary>
        ''' <param name="gGISDataSource"></param>
        ''' <remarks>Use this method if the form needs to be be re-loaded. The specified item
        ''' will be added to the dropdown and then selected when the form is shown.</remarks>
        Public Sub PreSelect(ByVal gGISDataSource As GISDataStructures.GISDataSource)
            m_gPreSelect = gGISDataSource
        End Sub

#End Region

#Region "Events"

        Private Sub cboInput_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboInput.SelectedIndexChanged

            Dim sFullPath As String = String.Empty
            Dim gSource As GISDataStructures.GISDataSource = cboInput.SelectedItem
            If TypeOf gSource Is GISDataStructures.GISDataSource Then
                tTip.SetToolTip(cboInput, gSource.FullPath)
                '
                ' Raise the event that the workspace has changed. This will bubble to the parent form
                ' which will then send the workspace to any output user controls on the form so that
                ' the output goes to the correct workspace.
                '
                RaiseEvent WorkspaceChanged(Me, New InputUCWorkspaceChangedEventArgs(gSource.WorkspacePath))

                sFullPath = gSource.FullPath
            Else
                tTip.SetToolTip(cboInput, String.Empty)
            End If
            '
            ' Raise the event that selected item has changed.
            '
            RaiseEvent SelectedItemChanged(Me, New InputUCSelectedItemChangedEventArgs(sFullPath))
        End Sub

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

    End Class

End Namespace