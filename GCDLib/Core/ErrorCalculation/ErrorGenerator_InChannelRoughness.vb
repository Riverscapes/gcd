
Public Class ErrorGenerator_InChannelRoughness


    Private m_gChannelUnitsFC As GISDataStructures.PolygonDataSource
    Private m_gBankfullPolygonFC As GISDataStructures.PolygonDataSource
    Private m_dChannelUnitWithGrainSize As Dictionary(Of UInteger, GISCode.CHaMP.ChannelUnit)
    Private m_gDEM As GISDataStructures.Raster

    Public Enum GrainSizePercentile
        D16
        D50
        D84
        D90
    End Enum

    ''' <summary>
    ''' RBT Constructor - 2016
    ''' </summary>
    ''' <param name="gChannelUnits">Feature class of channel units</param>
    ''' <param name="dChannelUnitWithGrainSize">dictionary of ChannelUnitWithGrainSize objects, the key to the dictionary is the channel unit id</param>
    ''' <param name="gBankfullPolygonFC">Feature class of bankfull polygon</param>
    ''' <param name="gDEM">DEM</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal gChannelUnits As GISDataStructures.PolygonDataSource,
                   ByVal dChannelUnitWithGrainSize As Dictionary(Of UInteger, GISCode.CHaMP.ChannelUnit),
                   ByVal gBankfullPolygonFC As GISDataStructures.PolygonDataSource,
                   ByVal gDEM As GISDataStructures.Raster)
        m_gChannelUnitsFC = gChannelUnits
        m_dChannelUnitWithGrainSize = dChannelUnitWithGrainSize
        m_gBankfullPolygonFC = gBankfullPolygonFC
        m_gDEM = gDEM
    End Sub


    ''' <summary>
    ''' In channel roughness used for 2016
    ''' A roughness height is now calculated from the D84 value and the extent of raster is extended to the bankfull polygon
    ''' </summary>
    ''' <param name="sOutputRasterPath">Path to create output raster at</param>
    ''' <param name="sUniqueChannelFieldName">Field used to identify unique channel units, typically is "Unit_Numbe"</param>
    ''' <param name="eGrainSizePercentile">Grain size percentile to be used to as the roughness value</param>
    ''' <param name="bCreateCDF">OPTIONAL: Boolean flag to tell function if a cumulative distribution function should be created and saved for each channel unit</param>
    ''' <param name="sOutputCDFFolder">OPTIONAL: folder to save cumulative distribution functions to</param>
    ''' <returns>GISDataStructures.Raster that is concurrent and orthogonal with m_gDEM</returns>
    ''' <remarks></remarks>
    Public Function Execute(ByVal sOutputRasterPath As String,
                            ByVal sUniqueChannelFieldName As String,
                            Optional ByRef lMessages As List(Of String) = Nothing,
                            Optional ByVal eGrainSizePercentile As GrainSizePercentile = GrainSizePercentile.D84,
                            Optional ByVal bCreateCDF As Boolean = False,
                            Optional ByVal sOutputCDFFolder As String = Nothing) As GISDataStructures.Raster

        'Initialize lMessages to avoid issues with using it is nothing
        If lMessages Is Nothing Then
            lMessages = New List(Of String)
        End If

        If m_gChannelUnitsFC.FeatureCount <> m_dChannelUnitWithGrainSize.Count Then
            lMessages.Add(String.Format("Error generating in-channel roughness. Channel Unit feature class count of {0} does not equal channel unit count in csv file of {1}. All channel units not present in csv will be assigned NoData.", m_gChannelUnitsFC.FeatureCount, m_dChannelUnitWithGrainSize.Count))
        End If

        If GISCode.GISDataStructures.VectorDataSource.Exists(m_gChannelUnitsFC.FullPath) Then
            If TypeOf m_gChannelUnitsFC Is GISDataStructures.PolygonDataSource Then

                'Check to make sure there are featues in the channel unit polygon
                If m_gChannelUnitsFC.FeatureCount < 1 Then
                    Debug.WriteLine("The temporary copy of the channel units feature class is empty.")
                    Return Nothing
                End If

                'Add D16, D50, D84, D90 to channel unit if they don't exist
                If m_gChannelUnitsFC.FindField(GrainSizePercentile.D16.ToString()) = -1 Then
                    m_gChannelUnitsFC.AddField(GrainSizePercentile.D16.ToString(), ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble)
                End If
                If m_gChannelUnitsFC.FindField(GrainSizePercentile.D50.ToString()) = -1 Then
                    m_gChannelUnitsFC.AddField(GrainSizePercentile.D50.ToString(), ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble)
                End If
                If m_gChannelUnitsFC.FindField(GrainSizePercentile.D84.ToString()) = -1 Then
                    m_gChannelUnitsFC.AddField(GrainSizePercentile.D84.ToString(), ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble)
                End If
                If m_gChannelUnitsFC.FindField(GrainSizePercentile.D90.ToString()) = -1 Then
                    m_gChannelUnitsFC.AddField(GrainSizePercentile.D90.ToString(), ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble)
                End If
            End If
        Else
            Throw New Exception(String.Format("Error accessing polygon feature class {0}.", m_gChannelUnitsFC.FullPath))
            Exit Function
        End If

        ' ShapeFiles will truncate the field name if it is more than 10 chars (e.g. "Unit_Number" becomes "Unit_Numbe")
        ' if the input is a shapefile recognize this truncation and adjust the field name
        Dim sTempCUField As String = sUniqueChannelFieldName
        If m_gChannelUnitsFC.GISDataStorageType = GISDataStructures.GISDataStorageTypes.ShapeFile Then
            sTempCUField = sTempCUField.Substring(0, 10)
        End If

        Dim pChannelUnitKeys As System.Collections.ICollection = m_dChannelUnitWithGrainSize.Keys
        Dim dResults As Dictionary(Of UInteger, GCD.GrainSizeDistributionCalculator.GrainSizeDistributionResults) = New Dictionary(Of UInteger, GCD.GrainSizeDistributionCalculator.GrainSizeDistributionResults)
        Dim iD16FieldIndex As Integer = m_gChannelUnitsFC.FindField(GrainSizePercentile.D16.ToString())
        Dim iD50FieldIndex As Integer = m_gChannelUnitsFC.FindField(GrainSizePercentile.D50.ToString())
        Dim iD84FieldIndex As Integer = m_gChannelUnitsFC.FindField(GrainSizePercentile.D84.ToString())
        Dim iD90FieldIndex As Integer = m_gChannelUnitsFC.FindField(GrainSizePercentile.D90.ToString())

        For Each iChannelUnitKey In pChannelUnitKeys
            Dim pResults As GCD.GrainSizeDistributionCalculator.GrainSizeDistributionResults = GISCode.GCD.GrainSizeDistributionCalculator.ProcessRBT_ChannelUnits(m_dChannelUnitWithGrainSize(iChannelUnitKey))
            dResults.Add(iChannelUnitKey, pResults)
        Next

        For iChannelUnitKey As Integer = 1 To m_gChannelUnitsFC.FeatureCount
            If dResults.ContainsKey(iChannelUnitKey) Then


                Dim pResults As GCD.GrainSizeDistributionCalculator.GrainSizeDistributionResults = dResults(iChannelUnitKey)

                'Check that the cumulative sum of the numeric substrate columns equals 100, if it does not write information to the reference list of strings lMessages
                Dim sErrorMessage As String = String.Empty
                If pResults.CheckSum <> 100 Then
                    sErrorMessage = String.Format("At channel unit {0} the cumulative total of substrate clases equaled {1}. It should equal 100. Metrics greater than or equal to D{1} cannot be calculated.",
                                                  iChannelUnitKey,
                                                  pResults.CheckSum)
                    lMessages.Add(sErrorMessage)
                End If

                sErrorMessage = String.Empty

                Select Case eGrainSizePercentile
                    Case GrainSizePercentile.D16 : sErrorMessage = If(pResults.D16 Is Nothing, String.Format("Error generating in-channel roughness at channel unit {0}. Could not calculate {1}, NoData has been assigned for channel unit {0}.", iChannelUnitKey, eGrainSizePercentile.ToString()),
                                                                                               String.Empty)
                    Case GrainSizePercentile.D50 : sErrorMessage = If(pResults.D50 Is Nothing, String.Format("Error generating in-channel roughness at channel unit {0}. Could not calculate {1}, NoData has been assigned for channel unit {0}.", iChannelUnitKey, eGrainSizePercentile.ToString()),
                                                                                               String.Empty)
                    Case GrainSizePercentile.D84 : sErrorMessage = If(pResults.D84 Is Nothing, String.Format("Error generating in-channel roughness at channel unit {0}. Could not calculate {1}, NoData has been assigned for channel unit {0}.", iChannelUnitKey, eGrainSizePercentile.ToString()),
                                                                                               String.Empty)
                    Case GrainSizePercentile.D90 : sErrorMessage = If(pResults.D90 Is Nothing, String.Format("Error generating in-channel roughness at channel unit {0}. Could not calculate {1}, NoData has been assigned for channel unit {0}.", iChannelUnitKey, eGrainSizePercentile.ToString()),
                                                                                               String.Empty)
                End Select

                If Not String.IsNullOrEmpty(sErrorMessage) Then
                    lMessages.Add(sErrorMessage)
                End If

                'Update the feature class
                Dim fcBuffer As ESRI.ArcGIS.Geodatabase.IFeatureBuffer = m_gChannelUnitsFC.FeatureClass.CreateFeatureBuffer()
                Dim queryFilter As ESRI.ArcGIS.Geodatabase.IQueryFilter = New ESRI.ArcGIS.Geodatabase.QueryFilterClass()
                Dim iChannelUnitValue As UInteger = iChannelUnitKey
                'Use channel unit number field, "Unit_Numbe" if shapefile and "Unit_Number" if feature class to access the specific channel unit in the feature class
                Dim sSQL_FormattedField As String = m_gChannelUnitsFC.WrapFieldForSQL(sTempCUField)
                queryFilter.WhereClause = String.Format("{0} = {1}", sSQL_FormattedField, iChannelUnitValue)

                Dim updateCursor As ESRI.ArcGIS.Geodatabase.ICursor = m_gChannelUnitsFC.FeatureClass.Update(queryFilter, False)
                Dim pRow As ESRI.ArcGIS.Geodatabase.IRow = updateCursor.NextRow()

                While Not pRow Is Nothing
                    'Null values in numeric fields of feature class in .gdb is represented by Double.MinValue, e.g. -1.7976931348623158e+308 (http://desktop.arcgis.com/en/arcmap/10.3/manage-data/shapefiles/geoprocessing-considerations-for-shapefile-output.htm)
                    'however this does not apply to shapefiles, this is safe as this operation can only be performed with inputs from a gdb
                    If pResults.D16 Is Nothing Then
                        pRow.Value(iD16FieldIndex) = Double.MinValue
                    Else
                        pRow.Value(iD16FieldIndex) = pResults.D16
                    End If
                    If pResults.D50 Is Nothing Then
                        pRow.Value(iD50FieldIndex) = Double.MinValue
                    Else
                        pRow.Value(iD50FieldIndex) = pResults.D50
                    End If
                    If pResults.D84 Is Nothing Then
                        pRow.Value(iD84FieldIndex) = Double.MinValue
                    Else
                        pRow.Value(iD84FieldIndex) = pResults.D84
                    End If
                    If pResults.D90 Is Nothing Then
                        pRow.Value(iD90FieldIndex) = Double.MinValue
                    Else
                        pRow.Value(iD90FieldIndex) = pResults.D90
                    End If
                    updateCursor.UpdateRow(pRow)
                    pRow = updateCursor.NextRow()
                End While

                'Attempt to release the locks placed on shapefile or feature class from update cursor and buffer
                Dim comReferencesLeft As Integer
                Do
                    comReferencesLeft = System.Runtime.InteropServices.Marshal.ReleaseComObject(updateCursor) _
                                        + System.Runtime.InteropServices.Marshal.ReleaseComObject(fcBuffer)
                Loop While (comReferencesLeft > 0)

                Debug.WriteLine(String.Format("D50: {0} ; D84: {1}", pResults.D50, pResults.D84))

                If bCreateCDF Then
                    Dim sCDFFolderName As String = System.IO.Path.GetFileNameWithoutExtension(m_gChannelUnitsFC.FullPath)
                    Dim sCDFFolderPath As String = System.IO.Path.Combine(sOutputRasterPath, sCDFFolderName & "_CDFs")
                    If Not System.IO.Directory.Exists(sCDFFolderPath) Then
                        System.IO.Directory.CreateDirectory(sCDFFolderPath)
                    End If

                    Dim sCDFFilePath As String = System.IO.Path.Combine(sCDFFolderPath, "CU_" & iChannelUnitValue & ".png")
                    GISCode.GCD.GrainSizeDistributionCalculator.CreateCDFPlot(pResults, sCDFFilePath)
                End If

            ElseIf Not dResults.ContainsKey(iChannelUnitKey) Then

                lMessages.Add(String.Format("Error generating in-channel roughness. Channel Unit {0} present in feature class but not in csv. Channel Unit {0} will be assigned NoData.", iChannelUnitKey))

                'Update the feature class
                Dim fcBuffer As ESRI.ArcGIS.Geodatabase.IFeatureBuffer = m_gChannelUnitsFC.FeatureClass.CreateFeatureBuffer()
                Dim queryFilter As ESRI.ArcGIS.Geodatabase.IQueryFilter = New ESRI.ArcGIS.Geodatabase.QueryFilterClass()
                Dim iChannelUnitValue As UInteger = iChannelUnitKey
                'Use channel unit number field, "Unit_Numbe" if shapefile and "Unit_Number" if feature class to access the specific channel unit in the feature class
                Dim sSQL_FormattedField As String = m_gChannelUnitsFC.WrapFieldForSQL(sTempCUField)
                queryFilter.WhereClause = String.Format("{0} = {1}", sSQL_FormattedField, iChannelUnitValue)

                Dim updateCursor As ESRI.ArcGIS.Geodatabase.ICursor = m_gChannelUnitsFC.FeatureClass.Update(queryFilter, False)
                Dim pRow As ESRI.ArcGIS.Geodatabase.IRow = updateCursor.NextRow()

                While Not pRow Is Nothing
                    'Null values in numeric fields of feature class in .gdb is represented by Double.MinValue, e.g. -1.7976931348623158e+308 (http://desktop.arcgis.com/en/arcmap/10.3/manage-data/shapefiles/geoprocessing-considerations-for-shapefile-output.htm)
                    'however this does not apply to shapefiles, this is safe as this operation can only be performed with inputs from a gdb
                    pRow.Value(iD16FieldIndex) = Double.MinValue
                    pRow.Value(iD50FieldIndex) = Double.MinValue
                    pRow.Value(iD84FieldIndex) = Double.MinValue
                    pRow.Value(iD90FieldIndex) = Double.MinValue
                    updateCursor.UpdateRow(pRow)
                    pRow = updateCursor.NextRow()
                End While

                'Attempt to release the locks placed on shapefile or feature class from update cursor and buffer
                Dim comReferencesLeft As Integer
                Do
                    comReferencesLeft = System.Runtime.InteropServices.Marshal.ReleaseComObject(updateCursor) _
                                        + System.Runtime.InteropServices.Marshal.ReleaseComObject(fcBuffer)
                Loop While (comReferencesLeft > 0)
            End If
        Next

        'Run Euclidean Allocation with
        ' - feature source data = channel units 
        ' - source field = Unit_Number
        ' - output allocation raster = temp_allocation.tif
        ' - output cell size = 0.1
        Dim sTempRasterPath As String = GISCode.WorkspaceManager.GetTempRaster("allocation")
        GP.SpatialAnalyst.EuclideanAllocation(m_gChannelUnitsFC.FullPath,
                                              sUniqueChannelFieldName,
                                              sTempRasterPath,
                                              m_gDEM)

        'Convert raster to polygon
        ' - input raster = temp_allocation.tif
        ' - field = Value
        ' - output polygon features = temp_allocation.shp
        Dim sTempAllocationShp As String = GISCode.WorkspaceManager.GetTempShapeFile("allocation")
        GISCode.GP.Conversion.RasterToPolygon_conversion(sTempRasterPath, sTempAllocationShp)

        'Clip 
        ' - input features = temp_allocation.shp
        ' - clip features = Bankfull polygon
        ' - output feature class = bankfull_channel_units.shp
        Dim sBankfullChannelUnits As String = GISCode.WorkspaceManager.GetTempShapeFile("bankfull_channel_unit")
        GISCode.GP.Analysis.Clip_analysis(sTempAllocationShp, m_gBankfullPolygonFC.FullPath, sBankfullChannelUnits)

        'Join temp_allocation.shp to temp channel units in order to get selected grain size percentile values
        GISCode.GP.DataManagement.JoinField(sBankfullChannelUnits, "GRIDCODE", m_gChannelUnitsFC.FullPath, sTempCUField, eGrainSizePercentile.ToString())

        'Convert bankfull channel unit roughness height field values to a raster
        Dim gBankfullChannelUnits As GISDataStructures.PolygonDataSource = New GISDataStructures.PolygonDataSource(sBankfullChannelUnits)
        GISCode.GP.Conversion.PolygonToRaster_conversion(gBankfullChannelUnits, eGrainSizePercentile.ToString(), sOutputRasterPath, m_gDEM)

        Dim gResult As GISDataStructures.Raster = Nothing
        ' Temporary fix. The copy raster routine seems to be messing up the projection on 
        ' the resultant raster. So assume that the output project is identical to the
        ' input project and simply use the geoprocessing routine to define it.

        'Validate raster output was created and that it has a spatial reference
        Dim pSR As ESRI.ArcGIS.Geometry.ISpatialReference = Nothing
        If TypeOf m_gDEM Is GISDataStructures.Raster Then
            pSR = m_gDEM.SpatialReference
            If System.IO.File.Exists(sOutputRasterPath) Then
                gResult = New GISDataStructures.Raster(sOutputRasterPath)
                GP.DataManagement.DefineProjection(gResult.RasterDataset, pSR)
            Else
                Throw New Exception(String.Format("The in channel roughness raster {0} was not created.", sOutputRasterPath))
                Exit Function
            End If
        End If

        Return gResult

    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()

        m_gChannelUnitsFC = Nothing
    End Sub
End Class
