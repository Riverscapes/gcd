Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry

Public Class ErrorGenerator_InterpolationError

    Private Const m_sInterpolationErrorField As String = "InterErr"
    Private Const m_TempZField As String = "Z"
    Private Const m_RasterValueField As String = "RASTERVALU"

    Private m_gPoints As GISDataStructures.PointDataSource3D
    Private m_gPointsUI As GISDataStructures.PointDataSource
    Private m_gSurveyExtent As GISDataStructures.PolygonDataSource
    Private m_gDEM As GISDataStructures.Raster
    Private m_gTIN As GISDataStructures.TINDataSource

    Public Sub New(ByVal sTINPath As String, ByVal gSurveyExtent As GISDataStructures.PolygonDataSource, ByVal gRaster As GISDataStructures.Raster)

        Dim sTempPointFCPath = WorkspaceManager.GetTempShapeFile("in_memoryTinNodes")
        GP.Analyst3D.TINNodesToPoints_3D(sTINPath, sTempPointFCPath, m_TempZField)
        m_gPoints = New GISDataStructures.PointDataSource(sTempPointFCPath)
        m_gSurveyExtent = gSurveyExtent
        m_gDEM = gRaster
        m_gTIN = New GISDataStructures.TINDataSource(sTINPath)

    End Sub

    ''' <summary>
    ''' RBT Contructor
    ''' </summary>
    ''' <param name="gTIN">Final TIN used to create DEM</param>
    ''' <param name="gSurveyExtent">Survey Extent</param>
    ''' <param name="bOtainExtension">Console applications cannot use the ExtensionManager that the ObtainExtension method uses. Instead they use the
    ''' AOInit.CheckoutExtension method to obtain spatial analyst. Therefore this method should only do the extension check in UI products.</param>
    ''' <param name="gRaster">DEM</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal gTIN As GISDataStructures.TINDataSource, ByVal gSurveyExtent As GISDataStructures.PolygonDataSource, ByVal gRaster As GISDataStructures.Raster, bOtainExtension As Boolean)

        Dim sTempPointFCPath = WorkspaceManager.GetTempShapeFile("in_memoryTinNodes")
        GP.Analyst3D.TINNodesToPoints_3D(gTIN, sTempPointFCPath, m_TempZField, bOtainExtension)
        m_gPoints = New GISDataStructures.PointDataSource3D(sTempPointFCPath)
        m_gSurveyExtent = gSurveyExtent
        m_gDEM = gRaster
        m_gTIN = gTIN

    End Sub


    ''' <summary>
    ''' UI Constructor
    ''' </summary>
    ''' <param name="gPoints"></param>
    ''' <param name="gSurveyExtent"></param>
    ''' <param name="gRaster"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal gPoints As GISDataStructures.PointDataSource, ByVal gSurveyExtent As GISDataStructures.PolygonDataSource, ByVal gRaster As GISDataStructures.Raster)

        m_gPointsUI = gPoints
        m_gSurveyExtent = gSurveyExtent
        m_gDEM = gRaster

    End Sub

    ''' <summary>
    ''' Creates a surface from the difference between TIN nodes/survyed points and the DEM that was created from them
    ''' </summary>
    ''' <param name="sOutputRaster">path to create output raster</param>
    ''' <returns>GISDataStructures.Raster that is concurrent and orthogonal with m_gDEM</returns>
    ''' <remarks></remarks>
    Public Function Execute(ByVal sOutputRaster As String) As GISDataStructures.Raster

        Dim sTempPointFCPath = WorkspaceManager.GetTempShapeFile("in_memory")

        Dim pTempPointFC As GISDataStructures.PointDataSource = GISCode.GP.SpatialAnalyst.ExtractValuesToPoints(m_gPoints, m_gDEM, sTempPointFCPath)

        Dim geoProcessorEngine As ESRI.ArcGIS.Geoprocessing.IGeoProcessor2 = New ESRI.ArcGIS.Geoprocessing.GeoProcessor()
        geoProcessorEngine.AddOutputsToMap = False
        pTempPointFC.AddField(m_sInterpolationErrorField, ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeSingle)

        Dim pInterErrFieldIndex = pTempPointFC.FindField(m_sInterpolationErrorField)
        Dim pRasterValueField = pTempPointFC.FindField(m_RasterValueField)
        Dim pZFieldIndex As Integer = pTempPointFC.FindField(m_TempZField)

        Dim pFeatureBuffer = pTempPointFC.FeatureClass.CreateFeatureBuffer()
        Dim pUpdateCursor As ESRI.ArcGIS.Geodatabase.IFeatureCursor = pTempPointFC.FeatureClass.Update(Nothing, False)
        Dim pRow As ESRI.ArcGIS.Geodatabase.IRow = pUpdateCursor.NextFeature()
        While Not pRow Is Nothing

            Dim zValue As Double = pRow.Value(pZFieldIndex)

            If pRow.Value(pRasterValueField) < 0 Or Double.IsNaN(zValue) Then
                Debug.WriteLine("No data found.")
                pRow = pUpdateCursor.NextFeature()
                Continue While
            End If

            pRow.Value(pInterErrFieldIndex) = Math.Abs(zValue - pRow.Value(pRasterValueField))

            pUpdateCursor.UpdateFeature(pRow)
            pRow = pUpdateCursor.NextFeature()
        End While

        System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureBuffer)
        System.Runtime.InteropServices.Marshal.ReleaseComObject(pUpdateCursor)

        Dim pSurfaceInterpolator As SurfaceInterpolator = New SurfaceInterpolator(pTempPointFC, m_sInterpolationErrorField, m_gSurveyExtent, m_gDEM)
        Dim sTempTIN As String = WorkspaceManager.GetSafeDirectoryPath("TempTIN")
        Debug.WriteLine("TIN: " & sTempTIN)
        Debug.WriteLine("Raster: " & sOutputRaster)
        Dim gResult = pSurfaceInterpolator.ExecuteRBT(False, sTempTIN, sOutputRaster, GP.Analyst3D.DelaunayTriangulationTypes.ConstrainedDelauncy)

        'Validate raster output was created
        If GISDataStructures.Raster.Exists(sOutputRaster) Then
            gResult = New GISDataStructures.Raster(sOutputRaster)
        Else
            Throw New Exception(String.Format("The interpolation raster {0} was not created.", sOutputRaster))
        End If

        Return gResult
    End Function

    Public Function ExecuteUI(ByVal sOutputRaster As String, ByVal sZFieldName As String) As GISDataStructures.Raster

        Dim sTempPointFCPath = WorkspaceManager.GetTempShapeFile("in_memory")

        'z field stuff
        Dim iProvidedZFieldIndex As Integer = m_gPointsUI.FindField(sZFieldName)
        Dim sActualShapeFieldName As String = m_gPointsUI.FeatureClass.ShapeFieldName
        Dim iActualShapeFieldIndex As Integer = m_gPointsUI.FindField(sActualShapeFieldName)

        'temp vector stuff
        Dim gTempPointFC = GISDataStructures.PointDataSource.CreateFeatureClass(sTempPointFCPath, GISDataStructures.BasicGISTypes.Point, True, m_gPointsUI.SpatialReference)
        Dim pTempPointBuffer = gTempPointFC.FeatureClass.CreateFeatureBuffer()
        Dim pInsertCursor = gTempPointFC.FeatureClass.Insert(True)

        Dim pZValueRaster As ESRI.ArcGIS.DataSourcesRaster.IRaster2 = CType(m_gDEM.Raster, ESRI.ArcGIS.DataSourcesRaster.IRaster2)

        'permanent vector
        Dim iRows As Integer = 0
        Dim pSearchCursor As ESRI.ArcGIS.Geodatabase.IFeatureCursor = m_gPointsUI.FeatureClass.Search(Nothing, False)
        Dim pRow As ESRI.ArcGIS.Geodatabase.IRow = pSearchCursor.NextFeature()

        Dim pPoint As ESRI.ArcGIS.Geometry.IPoint
        Dim dZValue As Double

        While Not pRow Is Nothing

            'create a point with the shapefield, regardless of where the z value is coming from (e.g. if point feature class is not z enabled it could be another field)
            pPoint = CType(pRow.Value(iActualShapeFieldIndex), ESRI.ArcGIS.Geometry.IPoint)

            'get correct z value, depending on if the user provided a z field different from shape
            If iProvidedZFieldIndex = iActualShapeFieldIndex Then
                dZValue = pPoint.Z
            Else
                dZValue = pRow.Value(iProvidedZFieldIndex)
            End If

            'get raster value and differnce from z value
            Dim dRasterValue As Double = ExtractRasterValuesAtXY(pZValueRaster, pPoint.X, pPoint.Y)
            Dim dDifference As Double = Math.Abs(dRasterValue - dZValue)

            'assign z value to point, this should save on memory when writin to the temporary feature class because the shape field is binary
            pPoint.Z = dDifference
            pTempPointBuffer.Shape = pPoint

            'Debug.Print(String.Format("Value: {0}", pValue))

            'place value in buffer, this helps with performance, only flush buffer every 2000 points
            pInsertCursor.InsertFeature(pTempPointBuffer)
            iRows += 1
            If (iRows Mod 2000) = 0 Then
                pInsertCursor.Flush()
            End If

            pRow = pSearchCursor.NextFeature()
        End While

        Dim comReferencesLeft As Integer
        Do
            comReferencesLeft = System.Runtime.InteropServices.Marshal.ReleaseComObject(pInsertCursor) _
                + System.Runtime.InteropServices.Marshal.ReleaseComObject(pTempPointBuffer)
        Loop While (comReferencesLeft > 0)

        'Get correct name of Shape.Z
        Dim sShapeZFieldName As String = String.Format("{0}.Z", gTempPointFC.FeatureClass.ShapeFieldName)


        Dim pSurfaceInterpolator As SurfaceInterpolator = New SurfaceInterpolator(gTempPointFC, sShapeZFieldName, m_gSurveyExtent, m_gDEM)
        Dim sTempTIN As String = WorkspaceManager.GetSafeDirectoryPath("TempTIN")
        Debug.WriteLine("TIN: " & sTempTIN)
        Debug.WriteLine("Raster: " & sOutputRaster)
        Dim gResult = pSurfaceInterpolator.ExecuteRBT(False, sTempTIN, sOutputRaster, GP.Analyst3D.DelaunayTriangulationTypes.ConstrainedDelauncy)

        'Validate raster output was created
        If GISDataStructures.Raster.Exists(sOutputRaster) Then
            gResult = New GISDataStructures.Raster(sOutputRaster)
        Else
            Throw New Exception(String.Format("The interpolation raster {0} was not created.", sOutputRaster))
        End If

        Return gResult


    End Function

    Private Function GetESRIFieldType(sTypeName As String) As ESRI.ArcGIS.Geodatabase.esriFieldType

        Dim d As Double
        Dim s As Single
        Dim i16 As Int16
        Dim i32 As Int32

        Select Case sTypeName
            Case d.GetType().Name
                Return ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble

            Case s.GetType().Name
                Return ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeSingle

            Case i16.GetType().Name
                Return ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeSmallInteger

            Case i32.GetType().Name
                Return ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeInteger

        End Select

        Return Nothing

    End Function

    Private Function ExtractRasterValuesAtXY(ByVal pRaster As ESRI.ArcGIS.DataSourcesRaster.IRaster2, ByVal xCoord As Double, ByVal yCoord As Double) As Double

        Dim col As Integer = pRaster.ToPixelColumn(xCoord)
        Dim row As Integer = pRaster.ToPixelRow(yCoord)
        Dim rasValue As Double = CDbl(pRaster.GetPixelValue(0, col, row))
        Return rasValue

    End Function

    Private Function ExtractRasterValuesAtXY(ByVal pRaster As ESRI.ArcGIS.Geodatabase.IRasterDataset,
                                             ByVal pPointFeatureClass As ESRI.ArcGIS.Geodatabase.IFeatureClass,
                                             ByVal iZFieldIndex As Integer) As GISDataStructures.PointDataSource

        Dim sTempPointFCPath = WorkspaceManager.GetTempShapeFile("in_memory")
        Dim gTempPointFC = GISDataStructures.PointDataSource.CreateFeatureClassUnprojected(sTempPointFCPath, GISDataStructures.BasicGISTypes.Point, True)
        Dim pTempPointBuffer = gTempPointFC.FeatureClass.CreateFeatureBuffer()
        Dim pInsertCursor = gTempPointFC.FeatureClass.Insert(True)

        Dim pInputRaster As ESRI.ArcGIS.DataSourcesRaster.IRaster2 = CType(pRaster.CreateDefaultRaster(), ESRI.ArcGIS.DataSourcesRaster.IRaster2)

        Dim pFeatureBuffer = pPointFeatureClass.CreateFeatureBuffer()

        Dim iRows As Integer = 0
        Dim pSearchCursor As ESRI.ArcGIS.Geodatabase.IFeatureCursor = pPointFeatureClass.Search(Nothing, False)
        Dim pRow As ESRI.ArcGIS.Geodatabase.IRow = pSearchCursor.NextFeature()
        While Not pRow Is Nothing
            Dim pPoint As ESRI.ArcGIS.Geometry.IPoint = CType(pRow.Value(iZFieldIndex), ESRI.ArcGIS.Geometry.IPoint)
            Dim iCol As Integer = pInputRaster.ToPixelColumn(pPoint.X)
            Dim iRow As Integer = pInputRaster.ToPixelRow(pPoint.Y)
            Dim dRasterValue As Double = CDbl(pInputRaster.GetPixelValue(0, iCol, iRow))
            Dim dDifference As Double = Math.Abs(dRasterValue - pPoint.Z)
            pPoint.Z = dDifference

            pTempPointBuffer.Shape = pPoint

            'Debug.Print(String.Format("Value: {0}", pValue))

            pInsertCursor.InsertFeature(pTempPointBuffer)
            iRows += 1
            If (iRows Mod 2000) = 0 Then
                pInsertCursor.Flush()
            End If


            pRow = pSearchCursor.NextFeature()
        End While

        Return gTempPointFC

    End Function

End Class
