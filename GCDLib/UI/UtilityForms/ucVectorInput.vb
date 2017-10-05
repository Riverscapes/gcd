Imports GCD.GCDLib.Core

Namespace UI.UtilityForms

    Public Class ucVectorInput
        Inherits ucInputBase

        Private m_GeometryType As GISDataStructures.BrowseVectorTypes

#Region "Properties"

        Public Property BrowseType As GISDataStructures.BrowseVectorTypes
            Get
                Return m_GeometryType
            End Get
            Set(ByVal value As GISDataStructures.BrowseVectorTypes)
                m_GeometryType = value
            End Set
        End Property

        Private ReadOnly Property GeometryType As GISDataStructures.GeometryTypes
            Get
                Dim eResult As GISDataStructures.GeometryTypes
                Select Case BrowseType
                    Case GISDataStructures.BrowseVectorTypes.Point
                        eResult = GISDataStructures.GeometryTypes.Point
                    Case GISDataStructures.BrowseVectorTypes.Line, GISDataStructures.BrowseVectorTypes.CrossSections
                        eResult = GISDataStructures.GeometryTypes.Line
                    Case GISDataStructures.BrowseVectorTypes.Polygon
                        eResult = GISDataStructures.GeometryTypes.Polygon
                    Case Else
                        Dim ex As New Exception("Invalid type. This class is only intended for vector types")
                        ex.Data.Add("Type", BrowseType.ToString)
                        Throw ex
                End Select

                Return eResult

            End Get
        End Property

        Private ReadOnly Property BasicGISType As GISDataStructures.BasicGISTypes
            Get
                Dim eResult As GISDataStructures.BrowseGISTypes
                Select Case BrowseType
                    Case GISDataStructures.BrowseVectorTypes.Point
                        eResult = GISDataStructures.BrowseGISTypes.Point
                    Case GISDataStructures.BrowseVectorTypes.Line, GISDataStructures.BrowseVectorTypes.CrossSections
                        eResult = GISDataStructures.BrowseGISTypes.Line
                    Case GISDataStructures.BrowseVectorTypes.Polygon
                        eResult = GISDataStructures.BrowseGISTypes.Polygon
                    Case Else
                        Dim ex As New Exception("Invalid type. This class is only intended for vector types")
                        ex.Data.Add("Type", BrowseType.ToString)
                        Throw ex
                End Select

                Return eResult

            End Get
        End Property

        'Public ReadOnly Property GeometryTypeESRI As ESRI.ArcGIS.Geometry.esriGeometryType
        '    Get
        '        Select Case GeometryType
        '            Case GISDataStructures.GeometryTypes.Line
        '                Return ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline

        '            Case GISDataStructures.GeometryTypes.Point
        '                Return ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint

        '            Case GISDataStructures.GeometryTypes.Polygon
        '                Return ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon
        '        End Select
        '    End Get
        'End Property

#End Region

#Region "Methods"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sNoun"></param>
        ''' <param name="eBrowseType"></param>
        ''' <remarks></remarks>
        Public Shadows Sub Initialize(ByVal sNoun As String, ByVal eBrowseType As GISDataStructures.BrowseVectorTypes)
            Noun = sNoun
            BrowseType = eBrowseType
        End Sub

        Private Sub VectorInputUC_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        End Sub

        Protected Overrides Sub Browse()

            Try
                naru.ui.Textbox.BrowseOpenVector(txtPath, naru.ui.UIHelpers.WrapMessageWithNoun("Select a", Noun, " Feature Class"))
            Catch ex As Exception
                ExceptionHelper.HandleException(ex, "Error browsing for vector input dataset.")
            End Try
        End Sub

        Public Overrides Function Validate() As Boolean

            If (String.IsNullOrEmpty(txtPath.Text) OrElse Not System.IO.File.Exists(txtPath.Text)) Then
                System.Windows.Forms.MessageBox.Show(naru.ui.UIHelpers.WrapMessageWithNoun("Please select a", Noun, " feature class to continue."), My.Resources.ApplicationNameLong, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information)
                Return False
            End If

            Return True

        End Function

        Public Shadows Function SelectedItem() As GISDataStructures.Vector
            Return DirectCast(MyBase.SelectedItem, GISDataStructures.Vector)
        End Function

#End Region

    End Class

End Namespace