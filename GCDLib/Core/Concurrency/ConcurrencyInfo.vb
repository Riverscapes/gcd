Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesRaster

Namespace GISCode.GCD.Concurrency

    Public Class ConcurrencyInfo

        Public Const LeftFld As String = "Left"
        Public Const RightFld As String = "Right"
        Public Const TopFld As String = "Top"
        Public Const BottomFld As String = "Bottom"
        Public Const RowsFld As String = "Rows"
        Public Const ColumnsFld As String = "Cols"
        Public Const CellResolutionFld As String = "CellRes"
        Public Const DateCreatedFld As String = GISDataStructures.CrossSectionFields.CreateDate

        Public Shared Function CreateOrthogonalityInfoTable(sWorkspace As String, eWorkspaceType As GISDataStructures.GISDataStorageTypes, sTableName As String) As ITable

            Dim pRasterWorkspace As IWorkspace = GISDataStructures.GetWorkspace(sWorkspace, eWorkspaceType)
            Dim pWS2 As IWorkspace2 = pRasterWorkspace
            Dim pFileGDBWorkspace As IFeatureWorkspace = pWS2

            Dim pTable As ESRI.ArcGIS.Geodatabase.ITable

            If pWS2.NameExists(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTTable, sTableName) Then
                pTable = pFileGDBWorkspace.OpenTable(sTableName)
                Return pTable
            End If

            Dim pField As ESRI.ArcGIS.Geodatabase.IField
            Dim pFieldEdit As ESRI.ArcGIS.Geodatabase.IFieldEdit

            Dim pFields As ESRI.ArcGIS.Geodatabase.IFields = New FieldsClass()
            Dim pFieldsEdit As ESRI.ArcGIS.Geodatabase.IFieldsEdit = pFields

            pField = New ESRI.ArcGIS.Geodatabase.FieldClass
            pFieldEdit = pField
            pFieldEdit.Name_2 = LeftFld
            pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
            pFieldEdit.IsNullable_2 = False
            pFieldEdit.Editable_2 = True
            pFieldsEdit.AddField(pField)

            pField = New ESRI.ArcGIS.Geodatabase.FieldClass
            pFieldEdit = pField
            pFieldEdit.Name_2 = RightFld
            pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
            pFieldEdit.IsNullable_2 = False
            pFieldEdit.Editable_2 = True
            pFieldsEdit.AddField(pField)

            pField = New ESRI.ArcGIS.Geodatabase.FieldClass
            pFieldEdit = pField
            pFieldEdit.Name_2 = TopFld
            pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
            pFieldEdit.IsNullable_2 = False
            pFieldEdit.Editable_2 = True
            pFieldsEdit.AddField(pField)

            pField = New ESRI.ArcGIS.Geodatabase.FieldClass
            pFieldEdit = pField
            pFieldEdit.Name_2 = BottomFld
            pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
            pFieldEdit.IsNullable_2 = False
            pFieldEdit.Editable_2 = True
            pFieldsEdit.AddField(pField)

            pField = New ESRI.ArcGIS.Geodatabase.FieldClass
            pFieldEdit = pField
            pFieldEdit.Name_2 = CellResolutionFld
            pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
            pFieldEdit.IsNullable_2 = False
            pFieldEdit.Editable_2 = True
            pFieldsEdit.AddField(pField)

            pField = New ESRI.ArcGIS.Geodatabase.FieldClass
            pFieldEdit = pField
            pFieldEdit.Name_2 = RowsFld
            pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
            pFieldEdit.IsNullable_2 = False
            pFieldEdit.Editable_2 = True
            pFieldsEdit.AddField(pField)

            pField = New ESRI.ArcGIS.Geodatabase.FieldClass
            pFieldEdit = pField
            pFieldEdit.Name_2 = ColumnsFld
            pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
            pFieldEdit.IsNullable_2 = False
            pFieldEdit.Editable_2 = True
            pFieldsEdit.AddField(pField)

            pField = New ESRI.ArcGIS.Geodatabase.FieldClass
            pFieldEdit = pField
            pFieldEdit.Name_2 = DateCreatedFld
            pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDate
            pFieldEdit.IsNullable_2 = False
            pFieldEdit.Editable_2 = True
            pFieldsEdit.AddField(pField)

            pFields = pFieldsEdit

            ' Use IFieldChecker to create a validated fields collection.
            Dim pFieldChecker As ESRI.ArcGIS.Geodatabase.IFieldChecker = New ESRI.ArcGIS.Geodatabase.FieldCheckerClass()
            Dim enumFieldError As ESRI.ArcGIS.Geodatabase.IEnumFieldError = Nothing
            Dim pValidatedFields As ESRI.ArcGIS.Geodatabase.IFields = Nothing
            pFieldChecker.ValidateWorkspace = CType(pWS2, ESRI.ArcGIS.Geodatabase.IWorkspace)
            pFieldChecker.Validate(pFields, enumFieldError, pValidatedFields)

            ' The enumFieldError enumerator can be inspected at this point to determine 
            ' which fields were modified during validation.
            '
            ' http://edndoc.esri.com/arcobjects/9.2/NET/E1E93AF5-D224-491A-BDEA-C0EEF2251A7A.htm
            '
            Dim ocDesc As IObjectClassDescription = New ObjectClassDescription
            pTable = pFileGDBWorkspace.CreateTable(sTableName, pFields, ocDesc.InstanceCLSID, ocDesc.ClassExtensionCLSID, Nothing)

            Return pTable

        End Function

        Public Shared Function SaveRasterProperties(pTable As ITable, gRaster As GISDataStructures.RasterDirect, Optional nPrecision As Integer = -1) As Boolean

            'Dim theRaster As New DEMInfoClass(sRasterPath, nPrecision)

            Dim bResult As Boolean = False

            Try
                Dim nLeftField As Integer = pTable.FindField(LeftFld)
                Dim nRightField As Integer = pTable.FindField(RightFld)
                Dim nTopField As Integer = pTable.FindField(TopFld)
                Dim nBottomField As Integer = pTable.FindField(BottomFld)
                Dim nCellResolutionField As Integer = pTable.FindField(CellResolutionFld)
                Dim nRowsField As Integer = pTable.FindField(RowsFld)
                Dim nColsField As Integer = pTable.FindField(ColumnsFld)
                Dim nDateCreatedField As Integer = pTable.FindField(DateCreatedFld)

                Dim pCursor As ICursor = pTable.Insert(True)
                Dim pRowBuffer As IRowBuffer = pTable.CreateRowBuffer

                pRowBuffer.Value(nLeftField) = gRaster.Extent.Left
                pRowBuffer.Value(nRightField) = gRaster.Extent.Right
                pRowBuffer.Value(nTopField) = gRaster.Extent.Top
                pRowBuffer.Value(nBottomField) = gRaster.Extent.Bottom
                pRowBuffer.Value(nCellResolutionField) = gRaster.CellWidth
                pRowBuffer.Value(nRowsField) = gRaster.Rows
                pRowBuffer.Value(nColsField) = gRaster.Columns
                pRowBuffer.Value(nDateCreatedField) = Now()

                pCursor.InsertRow(pRowBuffer)
                pCursor.Flush()

                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pRowBuffer)
                pCursor = Nothing
                pRowBuffer = Nothing

                bResult = True

            Catch ex As Exception
                ex.Data("Raster") = gRaster.FullPath
                Throw
            End Try

            Return bResult

        End Function
    End Class

End Namespace