Public Class ErrorGenerator_OutChannelRoughness

    Private m_gSurveyExtent As GISDataStructures.PolygonDataSource
    Private m_gWaterExtent As GISDataStructures.WaterExtentPolygon
    Private m_gDEM As GISDataStructures.Raster

    ''' <summary>
    ''' RBT Constructor - 2016
    ''' </summary>
    ''' <param name="gSurveyExtent"></param>
    ''' <param name="gWaterExtent"></param>
    ''' <param name="gDEM"></param>
    ''' <remarks></remarks>
    Public Sub New(gSurveyExtent As GISDataStructures.PolygonDataSource,
                   gWaterExtent As GISDataStructures.WaterExtentPolygon,
                   gDEM As GISDataStructures.Raster)
        m_gSurveyExtent = gSurveyExtent
        m_gWaterExtent = gWaterExtent
        m_gDEM = gDEM
    End Sub


    ''' <summary>
    ''' Out of channel roughness used for 2016
    ''' Calculated by solving for d50 in the Manning's coefficient equation 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>out of channel roughness value is uniform and calculated from by solving for d50 in Manning's coefficient n equation (n = (d50^(1/6))/21.1) this equates to
    ''' d50 = (21.1*n)^6. In this equation the value of n is taken from a lookup table for Manning's coefficient n, the value is for Floodplains with trees (0.15) 
    ''' http://www.engineeringtoolbox.com/mannings-roughness-d_799.html''' </remarks>
    Public Function Execute() As GISDataStructures.Raster

        Dim sOutChannelRaster As String = WorkspaceManager.GetTempRaster("OCRoughness")
        Dim sTempOutChannelShp As String = WorkspaceManager.GetTempShapeFile("out_channel")
        GP.Analysis.Erase_analysis(m_gSurveyExtent.FullPath, m_gWaterExtent.FullPath, sTempOutChannelShp)

        Dim gTempOutChannelShp As GISDataStructures.PolygonDataSource = New GISDataStructures.PolygonDataSource(sTempOutChannelShp)
        gTempOutChannelShp.AddField("Roughness", ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble)
        Dim iRoughnessFieldIndex As Integer = gTempOutChannelShp.FindField("Roughness")

        'Update the feature class
        Dim fcBuffer As ESRI.ArcGIS.Geodatabase.IFeatureBuffer = gTempOutChannelShp.FeatureClass.CreateFeatureBuffer()
        Dim queryFilter As ESRI.ArcGIS.Geodatabase.IQueryFilter = New ESRI.ArcGIS.Geodatabase.QueryFilterClass()

        Dim updateCursor As ESRI.ArcGIS.Geodatabase.ICursor = gTempOutChannelShp.FeatureClass.Update(queryFilter, False)
        Dim pRow As ESRI.ArcGIS.Geodatabase.IRow = updateCursor.NextRow()

        Dim dManningsN_FloodplainsTree As Double = 0.15

        While Not pRow Is Nothing
            pRow.Value(iRoughnessFieldIndex) = Math.Pow((21.1 * dManningsN_FloodplainsTree), 6)
            updateCursor.UpdateRow(pRow)
            pRow = updateCursor.NextRow()
        End While

        'Attempt to release the locks placed on shapefile or feature class from update cursor and buffer
        Dim comReferencesLeft As Integer
        Do
            comReferencesLeft = System.Runtime.InteropServices.Marshal.ReleaseComObject(updateCursor) _
                                + System.Runtime.InteropServices.Marshal.ReleaseComObject(fcBuffer)
        Loop While (comReferencesLeft > 0)

        GP.Conversion.PolygonToRaster_conversion(gTempOutChannelShp, "Roughness", sOutChannelRaster, m_gDEM)

        Dim gResult As GISDataStructures.Raster = Nothing

        If GISDataStructures.Raster.Exists(sOutChannelRaster) Then
            gResult = New GISDataStructures.Raster(sOutChannelRaster)
            Return gResult
        Else
            Throw New Exception(String.Format("The out of channel roughness raster {0} was not created.", sOutChannelRaster))
            Exit Function
        End If

        Return gResult

    End Function



End Class
