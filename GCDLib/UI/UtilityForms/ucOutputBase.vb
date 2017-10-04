Namespace UI.UtilityForms
    Public MustInherit Class ucOutputBase

        Private m_sNoun As String
        Protected m_sWorkspace As String
        Private m_bUserBrowsedWorkspace As Boolean
        Protected m_sInitialDatasetName As String

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

        Public Sub SetWorkspacePath(ByVal sWorkspacePath As String)

            m_sWorkspace = sWorkspacePath
            txtOutput.Text = FullPath()

        End Sub

        Public ReadOnly Property UserBrowsedWorkspace As Boolean
            Get
                Return m_bUserBrowsedWorkspace
            End Get
        End Property

        Public MustOverride Function FullPath() As String

        Public MustOverride Overloads Function Validate() As Boolean

        Public MustOverride ReadOnly Property IsValidPath() As Boolean

        Protected MustOverride Function Browse() As String

        Protected Sub Initialize(ByVal sNoun As String, ByVal sInitialDatasetName As String)
            Noun = sNoun
            m_sInitialDatasetName = sInitialDatasetName
        End Sub

#End Region

        Private Sub cmdBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBrowse.Click

            Dim sFullPath As String = Browse()
            If Not String.IsNullOrEmpty(sFullPath) Then
                txtOutput.Text = sFullPath
                m_bUserBrowsedWorkspace = True
            End If

        End Sub

        Public Sub New()
            ' This call is required by the designer.
            InitializeComponent()
            m_bUserBrowsedWorkspace = False
        End Sub

        Private Sub OutputUC_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        End Sub
    End Class
End Namespace