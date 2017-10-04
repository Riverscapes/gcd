Imports GCD.GCDLib.Core

Namespace UI.UtilityForms

    Public Class VectorOutputUC
        Inherits ucOutputBase

        Private m_GeometryType As GISDataStructures.BrowseVectorTypes

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

        'Private ReadOnly Property GeometryTypeESRI As ESRI.ArcGIS.Geometry.esriGeometryType
        '    Get
        '        Select Case GeometryType
        '            Case GISDataStructures.GeometryTypes.Point
        '                Return ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint

        '            Case GISDataStructures.GeometryTypes.Line
        '                Return ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline

        '            Case GISDataStructures.GeometryTypes.Polygon
        '                Return ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon
        '        End Select
        '    End Get
        'End Property

        Public Overrides Function FullPath() As String

            Dim sFullPath As String = txtOutput.Text
            If String.IsNullOrEmpty(sFullPath) Then
                If String.IsNullOrEmpty(m_sInitialDatasetName) Then
                    Return String.Empty
                Else
                    sFullPath = IO.Path.Combine(m_sWorkspace, m_sInitialDatasetName)
                End If
            End If
            GISDataStructures.Vector.ConfirmExtension(sFullPath)
            Return sFullPath

        End Function

        Public Shadows Sub Initialize(ByVal sNoun As String, ByVal sInitialDatasetName As String, ByVal eBrowseType As GISDataStructures.BrowseVectorTypes)
            MyBase.Initialize(sNoun, sInitialDatasetName)
            BrowseType = eBrowseType
        End Sub

        ''' <summary>
        ''' Open browse dialog and choose a new vector output location
        ''' </summary>
        ''' <returns>The full path of the selected data source, or empty string if cancelled</returns>
        ''' <remarks>This is polymorphic method overriden from base class</remarks>
        Protected Overrides Function Browse() As String

            Dim sTitle As String = naru.ui.UIHelpers.WrapMessageWithNoun("Select", Noun, "Output Location")
            naru.ui.Textbox.BrowseSaveVector(txtOutput, sTitle, txtOutput.Text)
            Return txtOutput.Text

        End Function

        Public Overrides Function Validate() As Boolean

            If String.IsNullOrEmpty(txtOutput.Text) Then
                MsgBox("You must specify an output " & Noun & " feature class", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                Return False
            End If

            If GISDataStructures.GISDataSource.Exists(txtOutput.Text) Then
                If MsgBox("The " & Noun & " feature class already exists. Do you wish to overwrite it?", MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Question, My.Resources.ApplicationNameLong) = MsgBoxResult.Yes Then
                    Try
                        External.RasterManager.Delete(txtOutput.Text, GCDProject.ProjectManager.GCDNARCError.ErrorString)
                    Catch ex As Exception
                        ExceptionHelper.HandleException(ex, "Error attempting to delete the existing feature class")
                        Return False
                    End Try
                Else
                    Return False
                End If
            End If

            If Not IsValidPath Then
                MsgBox("The output path is not a valid GIS path name.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                Return False
            End If

            Return True

        End Function

        Public Overrides ReadOnly Property IsValidPath() As Boolean
            Get
                Dim bValid As Boolean = False
                Dim sFullPath As String = FullPath()
                Dim sWorkspace As String = GISDataStructures.GISDataSource.GetWorkspacePath(sFullPath)
                If String.IsNullOrEmpty(sWorkspace) Then
                    bValid = False
                Else
                    'If GISDataStructures.IsFileGeodatabase(sFullPath) Then
                    '    If GISCode.GISDataStructures.FileGeodatabase.Exists(sWorkspace) Then
                    '        If String.IsNullOrEmpty(IO.Path.GetFileNameWithoutExtension(sFullPath)) Then
                    '            bValid = False
                    '        Else
                    '            bValid = String.IsNullOrEmpty(IO.Path.GetExtension(sFullPath))
                    '        End If
                    '    Else
                    '        bValid = False
                    '    End If
                    'Else
                    If IO.Directory.Exists(sWorkspace) Then
                        If String.IsNullOrEmpty(IO.Path.GetFileNameWithoutExtension(sFullPath)) Then
                            bValid = False
                        Else
                            Dim sExt As String = IO.Path.GetExtension(sFullPath)
                            If String.IsNullOrEmpty(sExt) Then
                                bValid = False
                            Else
                                bValid = String.Compare(sExt, ".shp", True) = 0
                            End If
                        End If
                    Else
                        bValid = False
                    End If
                End If
                'End If
                Return bValid
            End Get
        End Property

    End Class
End Namespace