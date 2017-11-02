Imports System.Windows.Forms

Namespace TopCAT

    Public Class WindowsFormAssistant

        Public Shared Sub PopulateDropDownsWithHeader(ByVal pPath As String, ByVal cmbBox As ComboBox)

            If String.IsNullOrEmpty(pPath) Then
                Exit Sub
            End If

            Dim streamReader As New System.IO.StreamReader(pPath)
            Dim header As String = streamReader.ReadLine()

            If Not String.Compare(header, "") = 0 Then
                For i As Integer = 0 To header.Split(",").Length - 1
                    If Not String.Compare(header.Split(",")(i), "") = 0 Then
                        cmbBox.Items.Add(header.Split(",")(i))
                    End If
                Next i
            End If


            cmbBox.SelectedItem = cmbBox.Items.Item(0)
        End Sub

        'Public Enum FileOpenTypes
        '    RawPointCloud
        '    ShapeFile
        '    Other
        'End Enum

        Public Shared Sub OpenFileDialog(ByVal sFileType As String, ByRef txtTextBox As TextBox)

            Dim frm As New OpenFileDialog
            If Not String.IsNullOrEmpty(txtTextBox.Text) Then
                If IO.Directory.Exists(IO.Path.GetDirectoryName(txtTextBox.Text)) Then
                    frm.InitialDirectory = IO.Path.GetDirectoryName(txtTextBox.Text)
                    frm.FileName = IO.Path.GetFileName(txtTextBox.Text)
                End If
            End If

            Select Case sFileType

                Case "Raw Point Cloud"
                    frm.Filter = "Point Cloud Files|*.pts;*.pt;*.xyz;*.txt;*.llh*|All Files|*.*"
                    frm.Title = "Select a Raw Point Cloud File"
                    frm.Multiselect = True
                    frm.SupportMultiDottedExtensions = True
                Case "Shapefile"
                    frm.Filter = "Shapefiles|*.shp"
                    frm.Title = "Select a point shapefile"

                Case "Raster"
                    frm.Filter = "Raster Files|*.img;*.tif"
                    frm.Title = "Select a raster file"

                Case "Coincident Points File"
                    'frm.Filter = "Coincident Point File|*.shp;*.txt"
                    frm.Filter = "Coincident Point File|*.txt"
                    frm.Title = "Select a coincident points shapefile or text based file"

                Case "Spatial Reference"
                    frm.Filter = "Projection files|*.prj;*.shp"
                    frm.Title = "Select a file containing projection information"
                    'If frm_CustomizeOptions.Chkbox_SetProjectSpatialRef.Checked = True Then

                    'End If
                Case Else

            End Select

            Dim sResult As String = String.Empty
            If frm.ShowDialog = DialogResult.OK Then
                txtTextBox.Text = frm.FileName
            End If

        End Sub

        Public Shared Function OpenGxFileDialog(ByVal fileType As String,
                                           ByVal sTitle As String,
                                           ByVal hParentWindowHandle As System.IntPtr) As String

            Dim gResult As ESRI.ArcGIS.Catalog.IGxObject = Nothing
            Dim pGxDialog As ESRI.ArcGIS.CatalogUI.IGxDialog = New ESRI.ArcGIS.CatalogUI.GxDialog
            Dim pFilterCol As ESRI.ArcGIS.Catalog.IGxObjectFilterCollection = pGxDialog

            pGxDialog.RememberLocation = True
            Dim pEnumGx As ESRI.ArcGIS.Catalog.IEnumGxObject = Nothing
            Dim pGxObject As ESRI.ArcGIS.Catalog.IGxObject = Nothing

            pGxDialog.Title = sTitle
            Dim sReturn As String = String.Empty

            Select Case fileType

                Case "Raster"
                    'gxDialog.AllowMultiSelect = True
                    pGxDialog.ButtonCaption = "Select raster"
                    'gxDialog.Title = "Select a raster file"
                    Dim gxObjFilter As ESRI.ArcGIS.Catalog.IGxObjectFilter = New ESRI.ArcGIS.Catalog.GxFilterRasterDatasets
                    pGxDialog.ObjectFilter = gxObjFilter

                Case "Shapefile"
                    pGxDialog.ButtonCaption = "Select .shp"
                    'gxDialog.Title = "Select a shapefile"
                    Dim gxObjFilter As ESRI.ArcGIS.Catalog.IGxObjectFilter = New ESRI.ArcGIS.Catalog.GxFilterShapefiles
                    pGxDialog.ObjectFilter = gxObjFilter

                Case "SpatialRef"
                    pGxDialog.ButtonCaption = "Select file"
                    Dim gxObjFilterCollection As ESRI.ArcGIS.Catalog.IGxObjectFilterCollection = CreateObject("esriCatalogUI.GxObjectFiltercollection")
                    Dim gxObjFilterShp As ESRI.ArcGIS.Catalog.IGxObjectFilter = New ESRI.ArcGIS.Catalog.GxFilterShapefiles
                    Dim gxObjFilterRas As ESRI.ArcGIS.Catalog.IGxObjectFilter = New ESRI.ArcGIS.Catalog.GxFilterRasterDatasets
                    Dim gxObjFilterTin As ESRI.ArcGIS.Catalog.IGxObjectFilter = New ESRI.ArcGIS.Catalog.GxFilterTINDatasets
                    gxObjFilterCollection.AddFilter(gxObjFilterShp, False)
                    gxObjFilterCollection.AddFilter(gxObjFilterRas, False)
                    gxObjFilterCollection.AddFilter(gxObjFilterTin, False)
                    pGxDialog.ObjectFilter = gxObjFilterCollection

                Case Else
                    Throw New Exception("Unhandled file type string when selecting gx dialog.")
            End Select

            Try

                If pGxDialog.DoModalOpen(hParentWindowHandle.ToInt32, pEnumGx) Then
                    pGxObject = pEnumGx.Next

                End If

                'Return pGxDialog.FinalLocation
                sReturn = pGxDialog.InternalCatalog.SelectedObject.FullName
            Catch ex As Exception
                'Dim ex2 As New Exception("Error browsing to feature class", ex)
                'ex2.Data.Add("Title", sTitle)
                'ex2.Data.Add("Folder", sFolder)
                'ex2.Data.Add("File", sFCName)
                'ex2.Data.Add("Geometry", eType.ToString)
                'Throw ex2
            End Try

            Return sReturn

        End Function

        Public Shared Sub SaveFileDialog(ByVal sFileType As String, ByRef txtTextBox As TextBox)

            Dim frm As New SaveFileDialog
            If Not String.IsNullOrEmpty(txtTextBox.Text) Then
                If IO.Directory.Exists(IO.Path.GetDirectoryName(txtTextBox.Text)) Then
                    frm.InitialDirectory = IO.Path.GetDirectoryName(txtTextBox.Text)
                    frm.FileName = IO.Path.GetFileName(txtTextBox.Text)
                End If
            End If

            Select Case sFileType

                Case "Raster"
                    frm.Filter = "Raster Files|*.img;*.tif"
                    frm.Title = "Select a location and name for the raster"
                Case "Shapefile"
                    frm.Filter = "Shapefiles|*.shp"
                    frm.Title = "Select a location and name for the point shapefile"
                Case "ToPCAT Ready Point Cloud File"
                    frm.Filter = "Point Cloud Files|*.pts;*.pt;*.xyz;*.txt;*.llh*|All Files|*.*"
                    frm.Title = "Select a location and name for the ToPCAT ready point cloud file"
                Case Else

            End Select

            Dim sResult As String = String.Empty
            If frm.ShowDialog = DialogResult.OK Then
                txtTextBox.Text = frm.FileName
            End If

        End Sub

        Public Shared Function GetSeperator(ByVal seperator As String) As String

            Select Case seperator.ToLower()

                Case "comma"
                    Return ","
                Case "space"
                    Return " "
                Case "semi-colon"
                    Return ";"
                Case "colon"
                    Return ":"
                Case "period"
                    Return "."
                Case "tab"
                    Return vbTab
                Case Else
                    Return Nothing
            End Select

        End Function

        Public Shared Function CheckIfShapefileHasPrjFile(ByVal ShapefileName As String)

            Dim prjFile As String = IO.Path.Combine(IO.Path.GetDirectoryName(ShapefileName), IO.Path.GetFileNameWithoutExtension(ShapefileName) & ".prj")

            If System.String.IsNullOrEmpty(ShapefileName) Then
                Return False
            Else
                If String.Compare(IO.Path.GetExtension(ShapefileName), ".shp") = 0 Then
                        If Not IO.File.Exists(prjFile) Then
                            'MsgBox(IO.Path.GetFileName(ShapefileName) & " does not have a spatial reference and cannot be used to provide a spatial reference to the output shapefile." & vbCrLf & vbCrLf &
                            '           "Please select another .shp file or .prj file.")
                            Return False
                        ElseIf IO.File.Exists(prjFile) Then
                            Return True
                        End If
                ElseIf String.Compare(IO.Path.GetExtension(ShapefileName), ".prj") = 0 Then
                    Return True
                End If
                Return False
            End If


        End Function


    End Class

End Namespace