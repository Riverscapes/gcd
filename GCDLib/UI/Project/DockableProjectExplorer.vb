''' <summary>
''' Designer class of the dockable window add-in. It contains user interfaces that
''' make up the dockable window.
''' </summary>
Public Class DockableProjectExplorer

  Public Sub New(ByVal hook As Object)

    ' This call is required by the Windows Form Designer.
    InitializeComponent()

    ' Add any initialization after the InitializeComponent() call.
    Me.Hook = hook
  End Sub


  Private m_hook As Object
  ''' <summary>
  ''' Host object of the dockable window
  ''' </summary> 
  Public Property Hook() As Object
    Get
      Return m_hook
    End Get
    Set(ByVal value As Object)
      m_hook = value
    End Set
  End Property

  ''' <summary>
  ''' Implementation class of the dockable window add-in. It is responsible for
  ''' creating and disposing the user interface class for the dockable window.
  ''' </summary>
  Public Class AddinImpl
    Inherits ESRI.ArcGIS.Desktop.AddIns.DockableWindow

        Private m_windowUI As DockableProjectExplorer

        Public ReadOnly Property UI As DockableProjectExplorer
            Get
                Return m_windowUI
            End Get
        End Property

    Protected Overrides Function OnCreateChild() As System.IntPtr
      m_windowUI = New DockableProjectExplorer(Me.Hook)
      Return m_windowUI.Handle
    End Function

    Protected Overrides Sub Dispose(ByVal Param As Boolean)
      If m_windowUI IsNot Nothing Then
        m_windowUI.Dispose(Param)
      End If

      MyBase.Dispose(Param)
    End Sub

  End Class

End Class