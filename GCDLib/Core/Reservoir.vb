' PGB March 2013
' New reservoir class.
'
Imports ESRI.ArcGIS.Geodatabase

Namespace GISCode.GCD.Reservoir

    ''' <summary>
    ''' Represents a single reservoir defined by a bathymetry DEM and water surface.
    ''' </summary>
    ''' <remarks>Note that lower in this file is the "reservoir analysis class". It's that class
    ''' that combines three reservoirs together to form an analysis. The "Raw" reservoir is the 
    ''' original DEM interected with the water surface. The "Min" reservoir is original DEM
    ''' minus the error surface. And the Max reservoir is the raw DEM plus the error surface/</remarks>
    Public Class Reservoir

#Region "Members"

        Private m_WaterSurface As GISCode.GCD.ConstantWaterSurface
        Private m_gDEM As GISDataStructures.Raster ' Note that this is the DEM on which the reservoir is based. Not necessarily the DEM survey reservoir

        Private m_fMinElevation As Double
        Private m_fMeanElevation As Double
        Private m_fStdElevation As Double
        Private m_fArea As Double
        Private m_fVolume As Double

        Private m_sReservoirBathymetryRaster As String
        Private m_sReservoirDepthRaster As String
        Private m_sVolumeRaster As String
        Private m_sMaskRaster As String
        Private m_sStatsTable As String

#End Region

#Region "Properties"

        ''' <summary>
        ''' Lowest elevation in this reservoir
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MinElevation As Double
            Get
                Return m_fMinElevation
            End Get
        End Property

        ''' <summary>
        ''' Highest elevation in this reservoir
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MaxElevation As Double
            Get
                Return m_WaterSurface.Elevation
            End Get
        End Property

        ''' <summary>
        ''' Mean elevation of the reservoir
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MeanElevation As Double
            Get
                Return m_fMeanElevation
            End Get
        End Property

        ''' <summary>
        ''' Standard deviation of elevations in the reservoir
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property StdDevElevation As Double
            Get
                Return m_fStdElevation
            End Get
        End Property

        ''' <summary>
        ''' Area of the wetted part of the reservoir
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Area As Double
            Get
                Return m_fArea
            End Get
        End Property

        ''' <summary>
        ''' Volume of the wetted part of the reservoir
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Volume As Double
            Get
                Return m_fVolume
            End Get
        End Property

        ''' <summary>
        ''' Path to the DEM used for this reservoir.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Note, this may not be the survey DEM raster. If the reservoir is one of the 
        ''' min or max then it could be the DEM plus or minus the error.</remarks>
        Public ReadOnly Property DEMPath As GISDataStructures.Raster
            Get
                Return m_gDEM
            End Get
        End Property

        ''' <summary>
        '''  The water surface object on which this reservoir is based
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property WaterSurface As ConstantWaterSurface
            Get
                Return m_WaterSurface
            End Get
        End Property

        ''' <summary>
        ''' Minimum depth of the reservoir
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Should always be zero or close to zero</remarks>
        Public ReadOnly Property MinDepth As Double
            Get
                Return m_WaterSurface.Elevation - MaxElevation
            End Get
        End Property

        ''' <summary>
        ''' Max depth of the reservoir at the deepest point
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MaxDepth As Double
            Get
                Return m_WaterSurface.Elevation - MinElevation
            End Get
        End Property

        ''' <summary>
        '''  Mean depth of the reservoir
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MeanDepth As Double
            Get
                Return m_WaterSurface.Elevation - MeanElevation
            End Get
        End Property

        ''' <summary>
        ''' The raster of the wetted reservoir elevations
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property BathymetryRaster As String
            Get
                Return m_sReservoirBathymetryRaster
            End Get
        End Property

        ''' <summary>
        ''' Raster of the wetted reservoir depths 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Water surface minus the bathymetry</remarks>
        Public ReadOnly Property DepthRaster As String
            Get
                Return m_sReservoirDepthRaster
            End Get
        End Property

        ''' <summary>
        ''' Each cell contains the volume of that cell
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Volume is the cell depth multiplied by the cell area</remarks>
        Public ReadOnly Property VolumeRaster As String
            Get
                Return m_sVolumeRaster
            End Get
        End Property

        ''' <summary>
        ''' The mask used to generate the raster.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Values of 1 are wetted and 0 are dry parts of the original DEM</remarks>
        Public ReadOnly Property MaskRaster As String
            Get
                Return m_sMaskRaster
            End Get
        End Property

        ''' <summary>
        ''' The DBF zonal stats table used to calculate the reservoir properties
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property StatsTable As String
            Get
                Return m_sStatsTable
            End Get
        End Property

#End Region

        ''' <summary>
        ''' Create a new reservoir from a DEM and a water surface
        ''' </summary>
        ''' <param name="waterSurface">Constant water surface object</param>
        ''' <param name="gDEM">DEM raster</param>
        ''' <remarks>The DEM does not need to be a raw raster. It can be a DEM plus or minus an error surface</remarks>
        Public Sub New(waterSurface As GISCode.GCD.ConstantWaterSurface, gDEM As GISDataStructures.Raster)
            m_WaterSurface = waterSurface
            m_gDEM = gDEM
        End Sub

        ''' <summary>
        ''' Create a reservoir object from the GCD project file
        ''' </summary>
        ''' <param name="aRow">GCD project file representing a reservoir row</param>
        ''' <param name="gDEM">DEM path</param>
        ''' <param name="aWS">Water surface</param>
        ''' <remarks></remarks>
        Public Sub New(aRow As ProjectDS.ReservoirRow, gDEM As GISDataStructures.Raster, aWS As GCD.WaterSurface)

            m_WaterSurface = aWS
            m_gDEM = gDEM
            m_sReservoirBathymetryRaster = aRow.BathRaster
            m_sReservoirDepthRaster = aRow.DepthRaster
            m_sVolumeRaster = aRow.VolumeRaster
            m_sMaskRaster = aRow.MaskRaster
            m_sStatsTable = aRow.StatsTable
            ReadStats()
        End Sub

        ''' <summary>
        ''' Calculate a reservoir
        ''' </summary>
        ''' <param name="diOutputFolder">Folder where all permanent artifacts will be placed</param>
        ''' <remarks>Intersects the water surface with the DEM and then measures the depth, area and volume
        ''' between these two surfaces where the difference is positive.</remarks>
        Public Sub Calculate(diOutputFolder As IO.DirectoryInfo)

            ''
            '' Positive depth
            'Dim sposdepth As String = WorkspaceManager.GetTempRaster("PosDepth")
            'GISCode.GP.SpatialAnalyst.Raster_Calculator("""" & sDepthMask & """ * """ & sDepth & """", sposdepth)
            ''GISCode.ArcMap.AddToMap(m_pArcMap, String.Empty, ArcMap.GCDRasterType.DEM, sposdepth, IO.Path.GetFileNameWithoutExtension(sposdepth), String.Empty)
            '
            ' Positive depth using SetNull()
            'GISCode.GP.SpatialAnalyst.SetNull(m_sDEMPath, m_sDEMPath, m_sReservoirBathymetryRaster, """VALUE"" >= " & sRawDepth)
            '
            ' Reservoir Depth
            'm_sDepthRaster = GISCode.Raster.GetNewSafeNameRaster(diOutputFolder.FullName, eRasterType, "Depth")
            'GISCode.GP.SpatialAnalyst.Raster_Calculator(WaterSurface.Elevation & " - """ & m_sReservoirBathymetryRaster & """", m_sDepthRaster)

            Dim eRasterType As GISDataStructures.Raster.RasterTypes = GISCode.GISDataStructures.Raster.GetDefaultRasterType ' GISCode.Raster.GetRasterType(My.Settings.DefaultRasterFormat)

            ' Raw Depth
            Dim sRawDepth As String = WorkspaceManager.GetTempRaster("RawDepth")
            GISCode.GP.SpatialAnalyst.Raster_Calculator("""" & m_WaterSurface.Source & """ - """ & m_gDEM.FullPath & """", sRawDepth)

            ' Depth for just the reservoir area 
            m_sReservoirDepthRaster = GISDataStructures.Raster.GetNewSafeName(diOutputFolder.FullName, eRasterType, "Depth")
            GISCode.GP.SpatialAnalyst.SetNull(sRawDepth, sRawDepth, m_sReservoirDepthRaster)

            ' bathymetry for just the reservoir area
            m_sReservoirBathymetryRaster = GISDataStructures.Raster.GetNewSafeName(diOutputFolder.FullName, eRasterType, "Bathymetry")
            GISCode.GP.SpatialAnalyst.SetNull(sRawDepth, m_gDEM.FullPath, m_sReservoirBathymetryRaster)

            ' Mask where depth is positive (i.e. below the surface)
            m_sMaskRaster = GISDataStructures.Raster.GetNewSafeName(diOutputFolder.FullName, eRasterType, "Mask")
            GISCode.GP.SpatialAnalyst.Raster_Calculator("""" & sRawDepth & """ > 0", m_sMaskRaster)

            ' Volume of the reservoir area
            Dim fCellSize As Double = m_gDEM.CellSize 'GISDataStructures.Raster.GetNewSafeName.GetCellSize(m_gDEM.FullPath)
            m_sVolumeRaster = GISDataStructures.Raster.GetNewSafeName(diOutputFolder.FullName, eRasterType, "Volume")
            GISCode.GP.SpatialAnalyst.Raster_Calculator("""" & m_sReservoirDepthRaster & """ * " & Math.Pow(fCellSize, 2), m_sVolumeRaster)

            m_sStatsTable = GISCode.FileSystem.GetNewSafeFileName(diOutputFolder.FullName, "stats", "dbf")
            GISCode.GP.SpatialAnalyst.ZonalStatisticsAsTable(m_sMaskRaster, "Value", m_sReservoirBathymetryRaster, m_sStatsTable, "ALL", False)

            ReadStats()

        End Sub

        ''' <summary>
        ''' Read the statistics from a zonal statistics DBF table and load them into member variables
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ReadStats()

            Dim pTable As ESRI.ArcGIS.Geodatabase.ITable = GISCode.Table.OpenDBFTable(m_sStatsTable)
            Dim pQry As IQueryFilter = New QueryFilter
            pQry.WhereClause = """Value"" = 1"

            Dim pCursor As ICursor = pTable.Search(pQry, True)
            Dim prow As IRow = pCursor.NextRow

            If TypeOf prow Is IRow Then
                m_fMeanElevation = RetrieveValue(pTable, prow, GP.SpatialAnalyst.sZonalStatsMeanFld)
                m_fMinElevation = RetrieveValue(pTable, prow, GP.SpatialAnalyst.sZonalStatsMinFld)
                m_fStdElevation = RetrieveValue(pTable, prow, GP.SpatialAnalyst.sZonalStatsStdFld)
                m_fArea = RetrieveValue(pTable, prow, GP.SpatialAnalyst.sZonalStatsAreaFld)
                Dim fCount As Double = RetrieveValue(pTable, prow, GP.SpatialAnalyst.sZonalStatsCountFld)

                m_fVolume = 0
                Dim nSumFld As Integer = pTable.FindField(GP.SpatialAnalyst.sZonalStatsSumFld)
                If nSumFld >= 0 Then
                    If Not IsDBNull(prow.Value(nSumFld)) Then
                        Dim fSum As Double = prow.Value(nSumFld)
                        m_fVolume = ((m_WaterSurface.Elevation * fCount) - fSum) * (m_fArea / fCount)
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Load a value from a DBF table and attempt to convert it to a double value.
        ''' </summary>
        ''' <param name="pTable"></param>
        ''' <param name="pRow"></param>
        ''' <param name="sFieldName"></param>
        ''' <returns></returns>
        ''' <remarks>This method is used because the process is called many times and it
        ''' needs to be done carefully, avoiding NULL values etc.</remarks>
        Private Function RetrieveValue(pTable As ESRI.ArcGIS.Geodatabase.ITable, pRow As ESRI.ArcGIS.Geodatabase.IRow, sFieldName As String) As Double

            Dim fValue As Double = 0
            Dim nFld As Integer = pTable.FindField(sFieldName)
            If nFld < 0 Then
                Dim ex As New Exception("Unable to find field in table")
                ex.Data("Field") = sFieldName
                Throw ex
            End If

            If TypeOf pRow Is IRow Then
                If Not IsDBNull(pRow.Value(nFld)) Then
                    fValue = pRow.Value(nFld)
                End If
            End If

            Return fValue

        End Function

        ''' <summary>
        ''' Delete any rasters and GIS tables created by this reservoir
        ''' </summary>
        ''' <param name="bDeleteDEM">True will cause hte DEM to get deleted. False, the DEM remains.</param>
        ''' <remarks>Note that the raw DEM uses the original survey DEM raster and this should not get deleted</remarks>
        Public Function DeleteRastersAndTables(bDeleteDEM As Boolean) As Boolean

            Dim bOK As Boolean = True
            Try
                GISCode.GP.DataManagement.Delete(m_sReservoirBathymetryRaster)
            Catch ex As Exception
                bOK = False
            End Try

            Try
                GISCode.GP.DataManagement.Delete(m_sReservoirDepthRaster)
            Catch ex As Exception
                bOK = False
            End Try

            Try
                GISCode.GP.DataManagement.Delete(m_sVolumeRaster)
            Catch ex As Exception
                bOK = False
            End Try

            Try
                GISCode.GP.DataManagement.Delete(m_sStatsTable)
            Catch ex As Exception
                bOK = False
            End Try

            Try
                GISCode.GP.DataManagement.Delete(m_sMaskRaster)
            Catch ex As Exception
                bOK = False
            End Try

            If bDeleteDEM Then
                Try
                    GISCode.GP.DataManagement.Delete(m_gDEM.FullPath)
                    m_gDEM = Nothing
                Catch ex As Exception
                    bOK = False
                End Try
            End If

            Return bOK

        End Function

    End Class

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''' <summary>
    ''' Represents three reservoirs: The raw bathymetry together with the min and max bathymetry, together with a water surface
    ''' </summary>
    ''' <remarks>Note that lower in this file is the "reservoir analysis class". It's that class
    ''' that combines three reservoirs together to form an analysis. The "Raw" reservoir is the 
    ''' original DEM interected with the water surface. The "Min" reservoir is original DEM
    ''' minus the error surface. And the Max reservoir is the raw DEM plus the error surface/</remarks>
    Public Class ReservoirAnalysis

#Region "Members"
        Private m_WaterSurface As GCD.ConstantWaterSurface

        Private m_sRawDEMName As String
        Private m_sRawDEMPath As String

        Private m_sErrorName As String
        Private m_sErrorPath As String

        Private m_RawReservoir As Reservoir
        Private m_MinReservoir As Reservoir
        Private m_MaxReservoir As Reservoir

        ''' <summary>
        ''' These are the names of the folders that are used to represent the three realizations of the reservoir.
        ''' Min refers to the DEM minus the error surface and max is the DEM plus the error surface.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const m_sMinimumFolder As String = "Min"
        Public Const m_sMaximumFolder As String = "Max"
        Public Const m_sRawFolder As String = "Raw"

        Private m_bOriginal As Boolean

#End Region

#Region "Properties"

        ''' <summary>
        ''' The lower reservoir
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MinReservoir As Reservoir
            Get
                Return m_MinReservoir
            End Get
        End Property

        ''' <summary>
        ''' The upper reservoir
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MaxReservoir As Reservoir
            Get
                Return m_MaxReservoir
            End Get
        End Property

        ''' <summary>
        '''  The raw or original reservoir
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property RawReservoir As Reservoir
            Get
                Return m_RawReservoir
            End Get
        End Property

        ''' <summary>
        ''' display name for the reservoir
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>The suffix "(Original)" is added for original reservoirs</remarks>
        Public ReadOnly Property DEMName As String
            Get
                If m_bOriginal Then
                    Return m_sRawDEMName & " (Original)"
                Else
                    Return m_sRawDEMName
                End If
            End Get
        End Property

        Public ReadOnly Property DEMPath As String
            Get
                Return m_sRawDEMPath
            End Get
        End Property

        Public ReadOnly Property ErrorName As String
            Get
                Return m_sErrorName
            End Get
        End Property

        Public ReadOnly Property ErrorPath As String
            Get
                Return m_sErrorPath
            End Get
        End Property

        Public ReadOnly Property WaterSurface As ConstantWaterSurface
            Get
                Return m_WaterSurface
            End Get
        End Property

        Public ReadOnly Property MaxDepth As Double
            Get
                Return m_RawReservoir.MaxDepth
            End Get
        End Property

        Public ReadOnly Property MaxDepthPM As Double
            Get
                Return m_RawReservoir.MaxDepth - m_MaxReservoir.MaxDepth
            End Get
        End Property

        Public ReadOnly Property MeanDepth As Double
            Get
                Return m_RawReservoir.MeanDepth
            End Get
        End Property

        Public ReadOnly Property RawArea As Double
            Get
                Return m_RawReservoir.Area
            End Get
        End Property

        Public ReadOnly Property MinArea As Double
            Get
                ' Note that the smallest area is actually the higher reservoir bathymetry
                Return m_MaxReservoir.Area
            End Get
        End Property

        Public ReadOnly Property MaxArea As Double
            Get
                ' Note that the largest area is actually the lower reservoir bathymetry
                Return m_MinReservoir.Area
            End Get
        End Property

        Public ReadOnly Property RawVolume As Double
            Get
                Return m_RawReservoir.Volume
            End Get
        End Property

        Public ReadOnly Property MinVolume As Double
            Get
                ' note that the smallest volume is actually the higher bathymetry
                Return m_MaxReservoir.Volume
            End Get
        End Property

        Public ReadOnly Property MaxVolume As Double
            Get
                ' note that the largest volume is actually the lower bathymetry
                Return m_MinReservoir.Volume
            End Get
        End Property

        Public ReadOnly Property OutputFolder As String
            Get
                Return GCDProject.ProjectManager.OutputManager.GetReservoirPath(DEMName, WaterSurface.Name)
            End Get
        End Property

#End Region

        ''' <summary>
        ''' Create a new reservoir analysis
        ''' </summary>
        ''' <param name="WaterSurface">The constant water surface representing the top of the reservoir water level</param>
        ''' <param name="sDEMName">The raw DEM survey name</param>
        ''' <param name="sRawDEMPath">The raw DEM survey raster</param>
        ''' <param name="sErrorName">The name of the error surface used to represent uncertainty in the reservoir bathymetry</param>
        ''' <param name="sErrorRasterPath">The error raster path</param>
        ''' <param name="bOriginal">True if this DEM represents the original survey of a reservoir</param>
        ''' <remarks></remarks>
        Public Sub New(WaterSurface As ConstantWaterSurface, sDEMName As String, sRawDEMPath As String, sErrorName As String, sErrorRasterPath As String, Optional bOriginal As Boolean = False)

            m_WaterSurface = WaterSurface
            m_sRawDEMName = sDEMName
            m_sRawDEMPath = sRawDEMPath
            m_sErrorName = sErrorName
            m_sErrorPath = sErrorRasterPath
            m_bOriginal = bOriginal

        End Sub

        ''' <summary>
        ''' Create a reservoir analysis from the GCD project file
        ''' </summary>
        ''' <param name="ResAnalysisRow">GCD project file reservoir analysis row</param>
        ''' <remarks></remarks>
        Public Sub New(gcdDS As ProjectDS, ResAnalysisRow As ProjectDS.ReservoirAnalysisRow)

            m_WaterSurface = New ConstantWaterSurface(ResAnalysisRow.WaterSurfacesRow.Source, ResAnalysisRow.WaterSurfacesRow.Name, ResAnalysisRow.WaterSurfacesRow.Elevation)
            For Each aRow As ProjectDS.ReservoirRow In gcdDS.Reservoir.Select("ReservoirAnalysisID = " & ResAnalysisRow.ReservoirAnalysisID)
                Dim gRaster As New GISDataStructures.Raster(ResAnalysisRow.DEMSurveyRow.Source)
                Dim aRes As New Reservoir(aRow, gRaster, m_WaterSurface)
                Select Case aRow.Type
                    Case m_sMinimumFolder : m_MinReservoir = aRes
                    Case m_sRawFolder : m_RawReservoir = aRes
                    Case m_sMaximumFolder : m_MaxReservoir = aRes
                End Select
            Next

            m_sRawDEMName = ResAnalysisRow.DEMSurveyRow.Name
            m_sRawDEMPath = ResAnalysisRow.DEMSurveyRow.Source
            m_sErrorName = ResAnalysisRow.ErrorTableRow.Name
            m_sErrorPath = ResAnalysisRow.ErrorTableRow.Source

            m_bOriginal = ResAnalysisRow.IsOriginal

        End Sub

        ''' <summary>
        ''' Delete all the files associated with a reservoir analysis. Including the underlying reservoirs
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>Calls delete on each of the three underlying reservoirs</remarks>
        Public Function DeleteRastersTablesAndfiles() As Boolean

            Dim bOK As Boolean = True

            If Not m_MinReservoir.DeleteRastersAndTables(False) Then
                bOK = False
            End If

            If Not m_MaxReservoir.DeleteRastersAndTables(False) Then
                bOK = False
            End If

            If Not m_RawReservoir.DeleteRastersAndTables(False) Then
                bOK = False
            End If

            Try
                IO.Directory.Delete(OutputFolder, True)
            Catch ex As Exception
                bOK = False
            End Try

            Return bOK

        End Function

        ''' <summary>
        ''' Calculate a reservoir analysis
        ''' </summary>
        ''' <remarks>The analysis consists of three reservoirs. The Min, Max and Raw.</remarks>
        Public Sub Calculate()

            Dim eRasterType As GISCode.GISDataStructures.Raster.RasterTypes = GISCode.GISDataStructures.Raster.GetDefaultRasterType ' GISCode.Raster.GetRasterType(My.Settings.DefaultRasterFormat)
            '
            ' Minimum DEM Reservoir
            '
            Dim diMin As IO.DirectoryInfo = IO.Directory.CreateDirectory(IO.Path.Combine(OutputFolder, m_sMinimumFolder))
            Dim sMinDEM As String = GISDataStructures.Raster.GetNewSafeName(diMin.FullName, eRasterType, "MinDEM")
            Dim gRaster As GISDataStructures.Raster = GISCode.GP.SpatialAnalyst.Raster_Calculator("""" & m_sRawDEMPath & """ - """ & m_sErrorPath & """", sMinDEM)
            m_MinReservoir = New Reservoir(m_WaterSurface, gRaster)
            m_MinReservoir.Calculate(diMin)
            '
            ' Raw DEM Reservoir
            '
            Dim diRaw As IO.DirectoryInfo = IO.Directory.CreateDirectory(IO.Path.Combine(OutputFolder, m_sRawFolder))
            Dim gRawDEM As New GISDataStructures.Raster(m_sRawDEMPath)
            m_RawReservoir = New Reservoir(m_WaterSurface, gRawDEM)
            m_RawReservoir.Calculate(diRaw)
            '
            ' Max DEM Reservoir
            '
            Dim diMax As IO.DirectoryInfo = IO.Directory.CreateDirectory(IO.Path.Combine(OutputFolder, m_sMaximumFolder))
            Dim sMaxDEM As String = GISDataStructures.Raster.GetNewSafeName(diMax.FullName, eRasterType, "MaxDEM")
            Dim gMaxRaster As GISDataStructures.Raster = GISCode.GP.SpatialAnalyst.Raster_Calculator("""" & m_sRawDEMPath & """ + """ & m_sErrorPath & """", sMaxDEM)
            m_MaxReservoir = New Reservoir(m_WaterSurface, gMaxRaster)
            m_MaxReservoir.Calculate(diMax)

        End Sub

    End Class

End Namespace