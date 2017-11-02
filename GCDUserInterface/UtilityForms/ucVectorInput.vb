Imports GCDLib.Core

Namespace UtilityForms

    Public Class ucVectorInput
        Inherits naru.ui.ucInput

        Private m_GeometryType As GCDConsoleLib.GDalGeometryType.SimpleTypes
        Private m_sNoun As String

#Region "Properties"

        Public ReadOnly Property Noun As String
            Get
                Return m_sNoun
            End Get
        End Property

        Public ReadOnly Property SelectedItem As GCDConsoleLib.Vector
            Get
                If TypeOf Path Is System.IO.FileInfo Then
                    Return New GCDConsoleLib.Vector(Path.FullName)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Private Property GeometryType As GCDConsoleLib.GDalGeometryType.SimpleTypes
            Get
                Return m_GeometryType
            End Get
            Set(value As GCDConsoleLib.GDalGeometryType.SimpleTypes)
                m_GeometryType = value
            End Set
        End Property

#End Region

#Region "Methods"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sNoun"></param>
        ''' <param name="eBrowseType"></param>
        ''' <remarks></remarks>
        Public Shadows Sub Initialize(ByVal sNoun As String, ByVal eBrowseType As GCDConsoleLib.GDalGeometryType.SimpleTypes)
            m_sNoun = sNoun
            GeometryType = eBrowseType
        End Sub

        Private Sub VectorInputUC_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        End Sub

        'Protected Overrides Sub Browse()

        '    Try
        '        naru.ui.Textbox.BrowseOpenVector(txtPath, naru.ui.UIHelpers.WrapMessageWithNoun("Select a", Noun, " Feature Class"))
        '    Catch ex As Exception
        '        ExceptionHelper.HandleException(ex, "Error browsing for vector input dataset.")
        '    End Try
        'End Sub

        'Public Overrides Function Validate() As Boolean

        '    If (String.IsNullOrEmpty(txtPath.Text) OrElse Not System.IO.File.Exists(txtPath.Text)) Then
        '        System.Windows.Forms.MessageBox.Show(naru.ui.UIHelpers.WrapMessageWithNoun("Please select a", Noun, " feature class to continue."), GCDCore.Properties.Resources.ApplicationNameLong, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information)
        '        Return False
        '    End If

        '    Return True

        'End Function

        'Public Shadows Function SelectedItem() As GCDConsoleLib.Vector
        '    Return DirectCast(MyBase.SelectedItem, GCDConsoleLib.Vector)
        'End Function

#End Region

    End Class

End Namespace