
Namespace TopCAT
    Public Class GIS

        Public Shared Function LoadProjectedCoordinateSystem(ByVal prjFile As String) As ESRI.ArcGIS.Geometry.IProjectedCoordinateSystem

            Dim factoryType As Type = Type.GetTypeFromProgID("esriGeometry.SpatialReferenceEnvironment")
            Dim spatialRefFactory As ESRI.ArcGIS.Geometry.ISpatialReferenceFactory3 = CType(Activator.CreateInstance(factoryType), ESRI.ArcGIS.Geometry.ISpatialReferenceFactory3)
            Dim projectedCoordinateSystem As ESRI.ArcGIS.Geometry.IProjectedCoordinateSystem = CType(spatialRefFactory.CreateESRISpatialReferenceFromPRJFile(prjFile), ESRI.ArcGIS.Geometry.IProjectedCoordinateSystem)

            Return projectedCoordinateSystem

        End Function

        Public Shared Function OpenRasterWorkspace(ByVal filePath As String) As ESRI.ArcGIS.Geodatabase.IRasterWorkspace2

            Dim workspaceFactory As ESRI.ArcGIS.DataSourcesRaster.RasterWorkspaceFactory = New ESRI.ArcGIS.DataSourcesRaster.RasterWorkspaceFactory()
            Dim rasterWs As ESRI.ArcGIS.Geodatabase.IRasterWorkspace2 = CType(workspaceFactory.OpenFromFile(filePath, 0), ESRI.ArcGIS.Geodatabase.IRasterWorkspace2)
            Return rasterWs

        End Function

        Public Shared Function OpenRasterDataset(ByVal rasterWs As ESRI.ArcGIS.Geodatabase.IRasterWorkspace2, ByVal pRaster As String) As ESRI.ArcGIS.Geodatabase.IRasterDataset

            If String.IsNullOrEmpty(pRaster) Then
                Return Nothing
            End If

            Dim rasterDataset As ESRI.ArcGIS.Geodatabase.IRasterDataset = rasterWs.OpenRasterDataset(pRaster)
            Return rasterDataset

        End Function

        Public Shared Function ReportRasterStats(ByVal rasterDataset As ESRI.ArcGIS.Geodatabase.IRasterDataset) As ESRI.ArcGIS.DataSourcesRaster.IRasterStatistics

            Dim bandCollection As ESRI.ArcGIS.DataSourcesRaster.IRasterBandCollection = CType(rasterDataset, ESRI.ArcGIS.DataSourcesRaster.IRasterBandCollection)
            Dim rasterBand As ESRI.ArcGIS.DataSourcesRaster.IRasterBand = bandCollection.Item(0)
            Dim rasterStats As ESRI.ArcGIS.DataSourcesRaster.IRasterStatistics = rasterBand.Statistics

            Dim builder As New System.Text.StringBuilder
            builder.Append("Statistics for: " & System.IO.Path.GetFileName(rasterDataset.CompleteName)).AppendLine().AppendLine()
            builder.Append("Mean: " & rasterStats.Mean).AppendLine()
            builder.Append("Median: " & rasterStats.Median).AppendLine()
            builder.Append("Mode: " & rasterStats.Mode).AppendLine()
            builder.Append("Min: " & rasterStats.Minimum).AppendLine()
            builder.Append("Max: " & rasterStats.Maximum).AppendLine()
            builder.Append("Standard Deviation: " & rasterStats.StandardDeviation).AppendLine()
            MsgBox(builder.ToString, MsgBoxStyle.OkOnly, "Raster Stats")
            'builder.Clear()
            builder = Nothing

            Return rasterStats

        End Function

        Public Shared Function ExtractRasterValuesAtXY_ToTextFile(ByVal raster As ESRI.ArcGIS.Geodatabase.IRasterDataset, ByVal xCoord As Double, ByVal yCoord As Double)


            Dim inputRaster As ESRI.ArcGIS.DataSourcesRaster.IRaster2 = CType(raster.CreateDefaultRaster(), ESRI.ArcGIS.DataSourcesRaster.IRaster2)
            Dim col As Integer = inputRaster.ToPixelColumn(xCoord)
            Dim row As Integer = inputRaster.ToPixelRow(yCoord)
            Dim rasValue As Double = CDbl(inputRaster.GetPixelValue(0, col, row))
            Return rasValue

        End Function

        Public Shared Function OpenFeatureClassWorkspace(ByVal filePath As String) As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace

            Dim workspaceFactory As ESRI.ArcGIS.Geodatabase.IWorkspaceFactory = New ESRI.ArcGIS.DataSourcesFile.ShapefileWorkspaceFactoryClass()
            Dim workspace As ESRI.ArcGIS.Geodatabase.IWorkspace = workspaceFactory.OpenFromFile(IO.Path.GetDirectoryName(filePath), 0)
            Dim featureWorkspace As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace = CType(workspace, ESRI.ArcGIS.Geodatabase.IFeatureWorkspace)
            Return featureWorkspace

        End Function

        Public Shared Function testCreateFeatureClass(ByVal featureWorkspace As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace,
                                                 ByVal featureClassName As String,
                                                 ByVal classExtensionUID As ESRI.ArcGIS.esriSystem.UID)

            ' Create a fields collection for the feature class.
            Dim fields As ESRI.ArcGIS.Geodatabase.IFields = New ESRI.ArcGIS.Geodatabase.FieldsClass()
            Dim fieldsEdit As ESRI.ArcGIS.Geodatabase.IFieldsEdit = DirectCast(fields, ESRI.ArcGIS.Geodatabase.IFieldsEdit)

            ' Add an object ID field to the fields collection. This is mandatory for feature classes.
            Dim oidField As ESRI.ArcGIS.Geodatabase.IField = New ESRI.ArcGIS.Geodatabase.FieldClass()
            Dim oidFieldEdit As ESRI.ArcGIS.Geodatabase.IFieldEdit = DirectCast(oidField, ESRI.ArcGIS.Geodatabase.IFieldEdit)
            oidFieldEdit.Name_2 = "OID"
            oidFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeOID
            fieldsEdit.AddField(oidField)

            '' Create a geometry definition (and spatial reference) for the feature class.
            Dim geometryDef As ESRI.ArcGIS.Geodatabase.IGeometryDef = New ESRI.ArcGIS.Geodatabase.GeometryDefClass()
            Dim geometryDefEdit As ESRI.ArcGIS.Geodatabase.IGeometryDefEdit = DirectCast(geometryDef, ESRI.ArcGIS.Geodatabase.IGeometryDefEdit)
            geometryDefEdit.GeometryType_2 = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint
            'Dim spatialReferenceFactory As ESRI.ArcGIS.Geometry.ISpatialReferenceFactory = New ESRI.ArcGIS.Geometry.SpatialReferenceEnvironmentClass()
            'Dim spatialReference As ESRI.ArcGIS.Geometry.ISpatialReference = spatialReferenceFactory.CreateProjectedCoordinateSystem(CInt(ESRI.ArcGIS.Geometry.esriSRProjCSType.esriSRProjCS_NAD1983UTM_20N))
            'Dim spatialReferenceResolution As ESRI.ArcGIS.Geometry.ISpatialReferenceResolution = DirectCast(spatialReference, ESRI.ArcGIS.Geometry.ISpatialReferenceResolution)
            'spatialReferenceResolution.ConstructFromHorizon()
            'Dim spatialReferenceTolerance As ESRI.ArcGIS.Geometry.ISpatialReferenceTolerance = DirectCast(spatialReference, ESRI.ArcGIS.Geometry.ISpatialReferenceTolerance)
            'spatialReferenceTolerance.SetDefaultXYTolerance()
            Dim spatialReference = GIS.LoadProjectedCoordinateSystem("C:\Program Files (x86)\ET_AL\MBES_Tools\projections\Idaho_Power_Transverse_Mercator_(IPTM).prj")
            geometryDefEdit.SpatialReference_2 = spatialReference

            ' Add a geometry field to the fields collection. This is where the geometry definition is applied.
            Dim geometryField As ESRI.ArcGIS.Geodatabase.IField = New ESRI.ArcGIS.Geodatabase.FieldClass()
            Dim geometryFieldEdit As ESRI.ArcGIS.Geodatabase.IFieldEdit = DirectCast(geometryField, ESRI.ArcGIS.Geodatabase.IFieldEdit)
            geometryFieldEdit.Name_2 = "Shape"
            geometryFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeGeometry
            geometryFieldEdit.GeometryDef_2 = geometryDef
            fieldsEdit.AddField(geometryField)

            ' Create a text field called "Name" for the fields collection.
            Dim zField As ESRI.ArcGIS.Geodatabase.IField = New ESRI.ArcGIS.Geodatabase.FieldClass()
            Dim zFieldEdit As ESRI.ArcGIS.Geodatabase.IFieldEdit = DirectCast(zField, ESRI.ArcGIS.Geodatabase.IFieldEdit)
            zFieldEdit.Name_2 = "Z"
            zFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
            fieldsEdit.AddField(zField)

            ' Use IFieldChecker to create a validated fields collection.
            Dim fieldChecker As ESRI.ArcGIS.Geodatabase.IFieldChecker = New ESRI.ArcGIS.Geodatabase.FieldCheckerClass()
            Dim enumFieldError As ESRI.ArcGIS.Geodatabase.IEnumFieldError = Nothing
            Dim validatedFields As ESRI.ArcGIS.Geodatabase.IFields = Nothing
            fieldChecker.ValidateWorkspace = DirectCast(featureWorkspace, ESRI.ArcGIS.Geodatabase.IWorkspace)
            fieldChecker.Validate(fields, enumFieldError, validatedFields)

            ' The enumFieldError enumerator can be inspected at this point to determine 
            ' which fields were modified during validation.

            ' Create the feature class. Note that the CLSID parameter is null - this indicates to use the
            ' default CLSID, esriGeodatabase.Feature (acceptable in most cases for feature classes).

            Dim featureClass As ESRI.ArcGIS.Geodatabase.IFeatureClass = featureWorkspace.CreateFeatureClass(featureClassName, validatedFields,
                                                                                                            Nothing, classExtensionUID,
                                                                                                            ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTSimple,
                                                                                                            "Shape", "")

            Return featureClass

        End Function

        Public Shared Function testGetPointGeometryList(ByVal filePath As String) As List(Of ESRI.ArcGIS.Geometry.IGeometry)


            Dim geometryList As New List(Of ESRI.ArcGIS.Geometry.IGeometry)
            Dim pointGeometry As ESRI.ArcGIS.Geometry.IGeometry

            Dim streamReader = New System.IO.StreamReader(filePath)
            streamReader.ReadLine()

            Do Until streamReader.EndOfStream
                Dim point As ESRI.ArcGIS.Geometry.IPoint = New ESRI.ArcGIS.Geometry.Point
                Dim newLine As String = streamReader.ReadLine()
                point.X = Double.Parse(newLine.Split(",")(0))
                point.Y = Double.Parse(newLine.Split(",")(1))
                pointGeometry = point
                geometryList.Add(pointGeometry)

            Loop
            streamReader.Close()
            streamReader.Dispose()
            Return geometryList

        End Function


        Public Shared Sub testInsertFeatures(ByVal featureClass As ESRI.ArcGIS.Geodatabase.IFeatureClass, ByVal geometryListFilePath As String)

            'Create a feature buffer and insert cursor
            Dim featureBuffer As ESRI.ArcGIS.Geodatabase.IFeatureBuffer = featureClass.CreateFeatureBuffer()
            Dim insertCursor As ESRI.ArcGIS.Geodatabase.IFeatureCursor = featureClass.Insert(True)
            Dim pointGeometry As ESRI.ArcGIS.Geometry.IGeometry
            Dim streamReader = New System.IO.StreamReader(geometryListFilePath)
            streamReader.ReadLine()

            Do Until streamReader.EndOfStream
                'Get the points
                Dim point As ESRI.ArcGIS.Geometry.IPoint = New ESRI.ArcGIS.Geometry.Point
                Dim newLine As String = streamReader.ReadLine()
                point.X = Double.Parse(newLine.Split(",")(0))
                point.Y = Double.Parse(newLine.Split(",")(1))
                pointGeometry = point
                'Set the feature buffer's shape and insert it
                featureBuffer.Shape = pointGeometry
                insertCursor.InsertFeature(featureBuffer)
            Loop

            streamReader.Close()
            streamReader.Dispose()
            insertCursor.Flush()
            Dim comReferencesLeft As Integer
            Do
                comReferencesLeft = System.Runtime.InteropServices.Marshal.ReleaseComObject(insertCursor) _
                    + System.Runtime.InteropServices.Marshal.ReleaseComObject(featureClass) _
                    + System.Runtime.InteropServices.Marshal.ReleaseComObject(featureBuffer)
            Loop While (comReferencesLeft > 0)

        End Sub

        Public Function CreateFeatureClass(ByVal featureClassName As String,
                                       ByVal classExtensionUID As ESRI.ArcGIS.esriSystem.UID,
                                       ByVal featureWorkspace As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace) As ESRI.ArcGIS.Geodatabase.IFeatureClass

            ' Create a fields collection for the feature class.
            Dim fields As ESRI.ArcGIS.Geodatabase.IFields = New ESRI.ArcGIS.Geodatabase.FieldsClass()
            Dim fieldsEdit As ESRI.ArcGIS.Geodatabase.IFieldsEdit = DirectCast(fields, ESRI.ArcGIS.Geodatabase.IFieldsEdit)

            ' Add an object ID field to the fields collection. This is mandatory for feature classes.
            Dim oidField As ESRI.ArcGIS.Geodatabase.IField = New ESRI.ArcGIS.Geodatabase.FieldClass()
            Dim oidFieldEdit As ESRI.ArcGIS.Geodatabase.IFieldEdit = DirectCast(oidField, ESRI.ArcGIS.Geodatabase.IFieldEdit)
            oidFieldEdit.Name_2 = "OID"
            oidFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeOID
            fieldsEdit.AddField(oidField)

            ' Create a geometry definition (and spatial reference) for the feature class.
            Dim geometryDef As ESRI.ArcGIS.Geodatabase.IGeometryDef = New ESRI.ArcGIS.Geodatabase.GeometryDefClass()
            Dim geometryDefEdit As ESRI.ArcGIS.Geodatabase.IGeometryDefEdit = DirectCast(geometryDef, ESRI.ArcGIS.Geodatabase.IGeometryDefEdit)
            geometryDefEdit.GeometryType_2 = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint
            Dim spatialReferenceFactory As ESRI.ArcGIS.Geometry.ISpatialReferenceFactory = New ESRI.ArcGIS.Geometry.SpatialReferenceEnvironmentClass()
            Dim spatialReference As ESRI.ArcGIS.Geometry.ISpatialReference = spatialReferenceFactory.CreateProjectedCoordinateSystem(CInt(ESRI.ArcGIS.Geometry.esriSRProjCSType.esriSRProjCS_NAD1983UTM_20N))
            Dim spatialReferenceResolution As ESRI.ArcGIS.Geometry.ISpatialReferenceResolution = DirectCast(spatialReference, ESRI.ArcGIS.Geometry.ISpatialReferenceResolution)
            spatialReferenceResolution.ConstructFromHorizon()
            Dim spatialReferenceTolerance As ESRI.ArcGIS.Geometry.ISpatialReferenceTolerance = DirectCast(spatialReference, ESRI.ArcGIS.Geometry.ISpatialReferenceTolerance)
            spatialReferenceTolerance.SetDefaultXYTolerance()
            geometryDefEdit.SpatialReference_2 = spatialReference

            ' Add a geometry field to the fields collection. This is where the geometry definition is applied.
            Dim geometryField As ESRI.ArcGIS.Geodatabase.IField = New ESRI.ArcGIS.Geodatabase.FieldClass()
            Dim geometryFieldEdit As ESRI.ArcGIS.Geodatabase.IFieldEdit = DirectCast(geometryField, ESRI.ArcGIS.Geodatabase.IFieldEdit)
            geometryFieldEdit.Name_2 = "Shape"
            geometryFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeGeometry
            geometryFieldEdit.GeometryDef_2 = geometryDef
            fieldsEdit.AddField(geometryField)

            ' Create a text field called "Name" for the fields collection.
            Dim nameField As ESRI.ArcGIS.Geodatabase.IField = New ESRI.ArcGIS.Geodatabase.FieldClass()
            Dim nameFieldEdit As ESRI.ArcGIS.Geodatabase.IFieldEdit = DirectCast(nameField, ESRI.ArcGIS.Geodatabase.IFieldEdit)
            nameFieldEdit.Name_2 = "Name"
            nameFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
            nameFieldEdit.Length_2 = 20
            fieldsEdit.AddField(nameField)

            ' Use IFieldChecker to create a validated fields collection.
            Dim fieldChecker As ESRI.ArcGIS.Geodatabase.IFieldChecker = New ESRI.ArcGIS.Geodatabase.FieldCheckerClass()
            Dim enumFieldError As ESRI.ArcGIS.Geodatabase.IEnumFieldError = Nothing
            Dim validatedFields As ESRI.ArcGIS.Geodatabase.IFields = Nothing
            fieldChecker.ValidateWorkspace = DirectCast(featureWorkspace, ESRI.ArcGIS.Geodatabase.IWorkspace)
            fieldChecker.Validate(fields, enumFieldError, validatedFields)

            ' The enumFieldError enumerator can be inspected at this point to determine 
            ' which fields were modified during validation.

            ' Create the feature class. Note that the CLSID parameter is null - this indicates to use the
            ' default CLSID, esriGeodatabase.Feature (acceptable in most cases for feature classes).

            Dim featureClass As ESRI.ArcGIS.Geodatabase.IFeatureClass = featureWorkspace.CreateFeatureClass(featureClassName, validatedFields,
                                                                                                            Nothing, classExtensionUID,
                                                                                                            ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTSimple,
                                                                                                            "Shape", "")

            Return featureClass

        End Function


        Public Function CreatePointFeatureClass(ByVal workspace As ESRI.ArcGIS.Geodatabase.IWorkspace2,
                                                       ByVal featureDataset As ESRI.ArcGIS.Geodatabase.IFeatureDataset,
                                                       ByVal featureClassName As System.String,
                                                       ByVal CLSID As ESRI.ArcGIS.esriSystem.UID,
                                                       ByVal fields As ESRI.ArcGIS.Geodatabase.IFields,
                                                       ByVal CLSEXT As ESRI.ArcGIS.esriSystem.UID,
                                                       ByVal strConfigKeyword As System.String)

            Dim featureClass As ESRI.ArcGIS.Geodatabase.IFeatureClass
            Dim featureWorkspace As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace = CType(workspace, ESRI.ArcGIS.Geodatabase.IFeatureWorkspace)

            'Check if feature class already exists
            If workspace.NameExists(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureClass, featureClassName) Then
                featureClass = featureWorkspace.OpenFeatureClass(featureClassName)
                Return featureClass
            End If

            'assign the class id value if not assigned
            If CLSID Is Nothing Then
                CLSID = New ESRI.ArcGIS.esriSystem.UIDClass
                CLSID.Value = "esriGeoDatabase.Feature"
            End If

            Dim objectClassDescription As ESRI.ArcGIS.Geodatabase.IObjectClassDescription = New ESRI.ArcGIS.Geodatabase.FeatureClassDescriptionClass

            If fields Is Nothing Then

                'create the fields using the required fields method
                fields = objectClassDescription.RequiredFields
                Dim fieldsEdit As ESRI.ArcGIS.Geodatabase.IFieldsEdit = CType(fields, ESRI.ArcGIS.Geodatabase.IFieldsEdit)
                Dim field As ESRI.ArcGIS.Geodatabase.IField = New ESRI.ArcGIS.Geodatabase.FieldClass

                'create a user defined text field
                Dim fieldEdit As ESRI.ArcGIS.Geodatabase.IFieldEdit = CType(field, ESRI.ArcGIS.Geodatabase.IFieldEdit)

                'setup field properties
                fieldEdit.Name_2 = "SampleField"
                fieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
                fieldEdit.IsNullable_2 = True
                fieldEdit.AliasName_2 = "Sample Field Column"
                fieldEdit.DefaultValue_2 = "test"
                fieldEdit.Editable_2 = True
                fieldEdit.Length_2 = 100

                'add field to field collection
                fieldsEdit.AddField(field)
                fields = CType(fieldsEdit, ESRI.ArcGIS.Geodatabase.IFields)

            End If

            Dim strShapeField As System.String = ""

            'locate the shape field
            Dim j As System.Int32
            For j = 0 To fields.FieldCount
                If fields.Field(j).Type = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeGeometry Then
                    strShapeField = fields.Field(j).Name
                End If
            Next j

            'Use IFieldChecker to create a validated fields collection
            Dim fieldChecker As ESRI.ArcGIS.Geodatabase.IFieldChecker = New ESRI.ArcGIS.Geodatabase.FieldCheckerClass()
            Dim enumFieldError As ESRI.ArcGIS.Geodatabase.IEnumFieldError = Nothing
            Dim validatedFields As ESRI.ArcGIS.Geodatabase.IFields = Nothing
            fieldChecker.ValidateWorkspace = CType(workspace, ESRI.ArcGIS.Geodatabase.IWorkspace)
            fieldChecker.Validate(fields, enumFieldError, validatedFields)

            'The enumFieldError enumerator can be inspected at this point to determine which fields were modified during validation

            'finally create and return the feauture class
            If featureDataset Is Nothing Then
                featureClass = featureWorkspace.CreateFeatureClass(featureClassName, validatedFields, CLSID,
                                                                   CLSEXT, ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTSimple,
                                                                   strShapeField, strConfigKeyword)
            Else
                featureClass = featureDataset.CreateFeatureClass(featureClassName, validatedFields, CLSID,
                                                                 CLSEXT, ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTSimple,
                                                                 strShapeField, strConfigKeyword)
            End If

            Return featureClass

        End Function


        Public Function CreateStandaloneFeatureClass(ByVal workspace As ESRI.ArcGIS.Geodatabase.IWorkspace,
                                                     ByVal featureClassName As String,
                                                     ByVal fieldsCollection As ESRI.ArcGIS.Geodatabase.IFields,
                                                     ByVal shapeFieldName As String) As ESRI.ArcGIS.Geodatabase.IFeatureClass

            Dim featureWorkspace As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace = CType(workspace, ESRI.ArcGIS.Geodatabase.IFeatureWorkspace)
            Dim fcDesc As ESRI.ArcGIS.Geodatabase.IFeatureClassDescription = New ESRI.ArcGIS.Geodatabase.FeatureClassDescriptionClass()
            Dim ocDesc As ESRI.ArcGIS.Geodatabase.IObjectClassDescription = CType(fcDesc, ESRI.ArcGIS.Geodatabase.IObjectClassDescription)

            ' Use IFieldChecker to create a validated fields collection.
            Dim fieldChecker As ESRI.ArcGIS.Geodatabase.IFieldChecker = New ESRI.ArcGIS.Geodatabase.FieldCheckerClass()
            Dim enumFieldError As ESRI.ArcGIS.Geodatabase.IEnumFieldError = Nothing
            Dim validatedFields As ESRI.ArcGIS.Geodatabase.IFields = Nothing
            fieldChecker.ValidateWorkspace = workspace
            fieldChecker.Validate(fieldsCollection, enumFieldError, validatedFields)

            ' The enumFieldError enumerator can be inspected at this point to determine
            ' which fields were modified during validation.
            Dim featureClass As ESRI.ArcGIS.Geodatabase.IFeatureClass = featureWorkspace.CreateFeatureClass(featureClassName, validatedFields,
                                                                                                            ocDesc.InstanceCLSID, ocDesc.ClassExtensionCLSID,
                                                                                                            ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTSimple,
                                                                                                            shapeFieldName, "")

            Return featureClass
        End Function

        Public Shared Function CreateSpatialOperator(ByVal pFeatureClassPath As String)

            Dim isPointInPolygon As ESRI.ArcGIS.Geometry.IRelationalOperator2 = Nothing

            If String.IsNullOrEmpty(pFeatureClassPath) Then
                Return Nothing
            End If

            If Not IsNothing(pFeatureClassPath) Then

                Dim featureClassWorkspace = GIS.OpenFeatureClassWorkspace(pFeatureClassPath)
                Dim extentFeatureClass As ESRI.ArcGIS.Geodatabase.IFeatureClass = featureClassWorkspace.OpenFeatureClass(System.IO.Path.GetFileName(pFeatureClassPath))
                Dim searchCursor As ESRI.ArcGIS.Geodatabase.IFeatureCursor = extentFeatureClass.Search(Nothing, True)
                Dim extentFeature As ESRI.ArcGIS.Geodatabase.IFeature = searchCursor.NextFeature
                Dim polygonGeometry As ESRI.ArcGIS.Geometry.IPointCollection4 = extentFeature.Value(extentFeatureClass.FindField(extentFeatureClass.ShapeFieldName))
                searchCursor.Flush()
                isPointInPolygon = CType(polygonGeometry, ESRI.ArcGIS.Geometry.IRelationalOperator2)

            End If
            Return isPointInPolygon

        End Function

        Public Shared Sub AddShapefile(ByVal pArcMap As ESRI.ArcGIS.Framework.IApplication, ByVal pShpFilePath As String)
            Dim pMXDoc As ESRI.ArcGIS.ArcMapUI.IMxDocument = pArcMap.Document
            ' Create a ShapefileWorkspaceFactory object and
            ' open a shapefile folder. The path works with a standard 9.3 installation.
            Dim workspaceFactory As ESRI.ArcGIS.Geodatabase.IWorkspaceFactory = New ESRI.ArcGIS.DataSourcesFile.ShapefileWorkspaceFactoryClass()
            Dim featureWorkspace As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace = TryCast(workspaceFactory.OpenFromFile(IO.Path.GetDirectoryName(pShpFilePath), 0), ESRI.ArcGIS.Geodatabase.IFeatureWorkspace)
            ' Create a FeatureLayer and assign a shapefile to it.
            Dim featureLayer As ESRI.ArcGIS.Carto.IFeatureLayer = New ESRI.ArcGIS.Carto.FeatureLayerClass
            featureLayer.FeatureClass = featureWorkspace.OpenFeatureClass(IO.Path.GetFileName(pShpFilePath))
            Dim layer As ESRI.ArcGIS.Carto.ILayer = featureLayer
            layer.Name = featureLayer.FeatureClass.AliasName
            ' Add the layer to the focus map.
            Dim pMap As ESRI.ArcGIS.Carto.IMap = pMXDoc.ActiveView.FocusMap
            pMap.AddLayer(featureLayer)
        End Sub


        'Private Sub CreateSR_FeatureClass(ByVal pSource As String)

        '    Dim workspaceFactory As ESRI.ArcGIS.Geodatabase.IWorkspaceFactory = New ESRI.ArcGIS.DataSourcesFile.ShapefileWorkspaceFactoryClass()
        '    Dim featureWorkspace As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace = TryCast(workspaceFactory.OpenFromFile(IO.Path.GetDirectoryName(m_RawPointCloud_FileDialog.FileName), 1), ESRI.ArcGIS.Geodatabase.IFeatureWorkspace)
        '    'Dim workspace As ESRI.ArcGIS.Geodatabase.IWorkspace = TryCast(featureWorkspace, ESRI.ArcGIS.Geodatabase.IWorkspace)

        '    'Assign the class id value 
        '    Dim CLSID As New ESRI.ArcGIS.esriSystem.UIDClass
        '    CLSID.Value = ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureDataset
        '    Dim CLSEXT As New ESRI.ArcGIS.esriSystem.UID
        '    CLSEXT.Value = "{40A9E885-5533-11D0-98BE-00805F7CED21}"

        '    Dim strConfigKeyword As System.String = "TempSR"

        '    'Set up fields
        '    Dim fields As ESRI.ArcGIS.Geodatabase.IFields
        '    Dim fieldsEdit As ESRI.ArcGIS.Geodatabase.IFieldsEdit = CType(fields, ESRI.ArcGIS.Geodatabase.IFieldsEdit)
        '    Dim field As ESRI.ArcGIS.Geodatabase.IField = New ESRI.ArcGIS.Geodatabase.FieldClass

        '    'Create Surface Roughness field
        '    Dim fieldEdit As ESRI.ArcGIS.Geodatabase.IFieldEdit = CType(field, ESRI.ArcGIS.Geodatabase.IFieldEdit)
        '    fieldEdit.Name_2 = "Roughness"
        '    fieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
        '    fieldEdit.AliasName_2 = "Detrended Standard Deviation"
        '    fieldEdit.DefaultValue_2 = -9999
        '    fieldEdit.Editable_2 = True

        '    'Add field to collection
        '    fieldsEdit.AddField(field)
        '    fields = CType(fieldsEdit, ESRI.ArcGIS.Geodatabase.IFields)

        '    'Locate the shapefield
        '    Dim strShapeField As System.String = ""
        '    Dim j As System.Int32
        '    For j = 0 To fields.FieldCount
        '        If fields.Field(j).Type = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeGeometry Then
        '            strShapeField = fields.Field(j).Name
        '        End If
        '    Next

        '    'Use IFieldChecker to create a validated fields collection
        '    Dim fieldChecker As ESRI.ArcGIS.Geodatabase.IFieldChecker = New ESRI.ArcGIS.Geodatabase.FieldCheckerClass()
        '    Dim enumFieldError As ESRI.ArcGIS.Geodatabase.IEnumFieldError = Nothing
        '    Dim validatedFields As ESRI.ArcGIS.Geodatabase.IFields = Nothing
        '    fieldChecker.ValidateWorkspace = CType(featureWorkspace, ESRI.ArcGIS.Geodatabase.IWorkspace)
        '    fieldChecker.Validate(fields, enumFieldError, validatedFields)

        '    'Can now use enumFieldError to inspect which fields were modified during validation

        '    Dim featureClass As ESRI.ArcGIS.Geodatabase.IFeatureClass = featureWorkspace.CreateFeatureClass("Temp",
        '                                                                                                    validatedFields,
        '                                                                                                    CLSID,
        '                                                                                                    CLSEXT,
        '                                                                                                    ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTSimple,
        '                                                                                                    strShapeField,
        '                                                                                                    strConfigKeyword)

        'End Sub


        'Private Sub PopulateShapeFile(ByVal pShpSource As String, ByVal pToPCAT_Source As String)

        '    Dim workspaceFactory As ESRI.ArcGIS.Geodatabase.IWorkspaceFactory = New ESRI.ArcGIS.DataSourcesFile.ShapefileWorkspaceFactoryClass()
        '    Dim featureWorkspace As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace = TryCast(workspaceFactory.OpenFromFile(IO.Path.GetDirectoryName(pShpSource), 1), ESRI.ArcGIS.Geodatabase.IFeatureWorkspace)
        '    Dim featureClass As ESRI.ArcGIS.Geodatabase.IFeatureClass = featureWorkspace.OpenFeatureClass("Temp")
        '    'Create feature buffer
        '    Dim featureBuffer As ESRI.ArcGIS.Geodatabase.IFeatureBuffer = featureClass.CreateFeatureBuffer()
        '    Dim featureCursor As ESRI.ArcGIS.Geodatabase.IFeatureCursor = featureClass.Insert(True)

        '    Dim fileReader As New System.IO.StreamReader(pToPCAT_Source, False)
        '    Dim header As String = fileReader.ReadLine()

        '    Dim lineOfText As String = fileReader.ReadLine()
        '    While lineOfText IsNot Nothing
        '        Dim X As Double = System.Double.Parse(header.Split(",")(0))
        '        Dim Y As Double = System.Double.Parse(header.Split(",")(1))
        '        Dim SR As Double = System.Double.Parse(header.Split(",")(7))

        '        featureBuffer.Shape = (X,Y)



        '    End While



        'End Sub

    End Class
End Namespace
