''' <summary>
''' Statistics describing a pair of raw and thresholded DoD rasters
''' </summary>
''' <remarks>Created by Philip Bailey, 18 Feb 2014
''' Note that this class is pure virtual and cannot be created as a variable. You must
''' instead create variables of type ChangeStatsMinLoD (minimum level of detection) or 
''' ChangeStatsPropProb (propagated or probabilistic).</remarks>
Public MustInherit Class ChangeStats

    Private m_fCellArea As Double

    Private m_fAreaErosion_Raw As Double
    Private m_fAreaDeposition_Raw As Double
    Private m_fAreaErosion_Thresholded As Double
    Private m_fAreaDeposition_Thresholded As Double

    Private m_fVolumeErosion_Raw As Double
    Private m_fVolumeDeposition_Raw As Double
    Private m_fVolumeErosion_Thresholded As Double
    Private m_fVolumeDeposition_Thresholded As Double

    Protected m_fVolumeErosion_Error As Double
    Protected m_fVolumeDeposition_Error As Double

#Region "Properties"

    Public ReadOnly Property CellArea As Double
        Get
            Return m_fCellArea
        End Get
    End Property


#Region "Areal Properties"
    Public ReadOnly Property AreaErosion_Raw As Double
        Get
            Return m_fAreaErosion_Raw
        End Get
    End Property

    Public ReadOnly Property AreaDeposition_Raw As Double
        Get
            Return m_fAreaDeposition_Raw
        End Get
    End Property

    Public Property AreaErosion_Thresholded As Double
        Get
            Return m_fAreaErosion_Thresholded
        End Get
        Set(value As Double)
            m_fAreaErosion_Thresholded = value
        End Set
    End Property

    Public Property AreaDeposition_Thresholded As Double
        Get
            Return m_fAreaDeposition_Thresholded
        End Get
        Set(value As Double)
            m_fAreaDeposition_Thresholded = value
        End Set
    End Property

    Public ReadOnly Property AreaDetectableChange_Thresholded As Double
        Get
            Return (AreaErosion_Thresholded + AreaDeposition_Thresholded)
        End Get
    End Property

    Public ReadOnly Property AreaOfInterest_Raw As Double
        Get
            Return (AreaErosion_Raw + AreaDeposition_Raw)
        End Get
    End Property

    Public ReadOnly Property AreaPercentAreaInterestWithDetectableChange As Double
        Get
            Return 100 * (AreaDetectableChange_Thresholded / AreaOfInterest_Raw)
        End Get
    End Property
#End Region

#Region "Volumetric Properties"
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public ReadOnly Property VolumeErosion_Raw As Double
        Get
            Return m_fVolumeErosion_Raw
        End Get
    End Property

    Public ReadOnly Property VolumeDeposition_Raw As Double
        Get
            Return m_fVolumeDeposition_Raw
        End Get
    End Property

    Public Property VolumeErosion_Thresholded As Double
        Get
            Return m_fVolumeErosion_Thresholded
        End Get
        Set(value As Double)
            m_fVolumeErosion_Thresholded = value
        End Set
    End Property

    Public Property VolumeDeposition_Thresholded As Double
        Get
            Return m_fVolumeDeposition_Thresholded
        End Get
        Set(value As Double)
            m_fVolumeDeposition_Thresholded = value
        End Set
    End Property

    Public Property VolumeErosion_Error As Double
        Get
            Return m_fVolumeErosion_Error
        End Get
        Set(value As Double)
            m_fVolumeErosion_Error = value
        End Set
    End Property

    Public Property VolumeDeposition_Error As Double
        Get
            Return m_fVolumeDeposition_Error
        End Get
        Set(value As Double)
            m_fVolumeDeposition_Error = value
        End Set
    End Property

    Public ReadOnly Property VolumeOfDifference_Raw As Double
        Get
            Return VolumeErosion_Raw + VolumeDeposition_Raw
        End Get
    End Property

    Public ReadOnly Property VolumeOfDifference_Thresholded As Double
        Get
            Return VolumeErosion_Thresholded + VolumeDeposition_Thresholded
        End Get
    End Property

    Public ReadOnly Property VolumeOfDifference_Error As Double
        Get
            Return (VolumeDeposition_Error + VolumeErosion_Error)
        End Get
    End Property

    Public ReadOnly Property NetVolumeOfDifference_Raw As Double
        Get
            Return (VolumeDeposition_Raw - VolumeErosion_Raw)
        End Get
    End Property

    Public ReadOnly Property NetVolumeOfDifference_Thresholded As Double
        Get
            Return (VolumeDeposition_Thresholded - VolumeErosion_Thresholded)
        End Get
    End Property

    Public ReadOnly Property NetVolumeOfDifference_Error As Double
        Get
            Return Math.Sqrt(Math.Pow(VolumeDeposition_Error, 2) + Math.Pow(VolumeErosion_Error, 2))
        End Get
    End Property

    Public ReadOnly Property VolumeOfErosion_Percent As Double
        Get
            Dim fResult As Double = 0
            If VolumeErosion_Error <> 0 AndAlso VolumeErosion_Thresholded <> 0 Then
                fResult = 100 * (VolumeErosion_Error / VolumeErosion_Thresholded)
            End If
            Return fResult
        End Get
    End Property

    Public ReadOnly Property VolumeOfDeposition_Percent As Double
        Get
            Dim fResult As Double = 0
            If VolumeDeposition_Error <> 0 AndAlso VolumeDeposition_Thresholded <> 0 Then
                fResult = 100 * (VolumeDeposition_Error / VolumeDeposition_Thresholded)
            End If
            Return fResult
        End Get
    End Property

    Public ReadOnly Property VolumeOfDifference_Percent As Double
        Get
            Dim fResult As Double = 0
            If VolumeOfDifference_Error <> 0 AndAlso VolumeOfDifference_Thresholded <> 0 Then
                fResult = 100 * (VolumeOfDifference_Error / VolumeOfDifference_Thresholded)
            End If
            Return fResult
        End Get
    End Property

    Public ReadOnly Property NetVolumeOfDifference_Percent As Double
        Get
            Dim fResult As Double = 0
            If NetVolumeOfDifference_Error <> 0 AndAlso NetVolumeOfDifference_Thresholded <> 0 Then
                fResult = 100 * (NetVolumeOfDifference_Error / NetVolumeOfDifference_Thresholded)
            End If
            Return fResult
        End Get
    End Property

#End Region

#Region "Vertical Averages Erosion"

    Public ReadOnly Property AverageDepthErosion_Raw As Double
        Get
            Return SafeDivision(AreaErosion_Raw, VolumeErosion_Raw)
        End Get
    End Property

    Public ReadOnly Property AverageDepthErosion_Thresholded As Double
        Get
            Return SafeDivision(VolumeErosion_Thresholded, AreaErosion_Thresholded)
        End Get
    End Property

    Public ReadOnly Property AverageDepthErosion_Error As Double
        Get
            Return SafeDivision(VolumeErosion_Error, AreaErosion_Thresholded)
        End Get
    End Property

    Public ReadOnly Property AverageDepthErosion_Percent As Double
        Get
            Return 100 * SafeDivision(AverageDepthErosion_Error, AverageDepthErosion_Thresholded)
        End Get
    End Property

#End Region

#Region "Vertical Averages Deposition"

    Public ReadOnly Property AverageDepthDeposition_Raw As Double
        Get
            Return SafeDivision(AreaDeposition_Raw, VolumeDeposition_Raw)
        End Get
    End Property

    Public ReadOnly Property AverageDepthDeposition_Thresholded As Double
        Get
            Return SafeDivision(VolumeDeposition_Thresholded, AreaDeposition_Thresholded)
        End Get
    End Property

    Public ReadOnly Property AverageDepthDeposition_Error As Double
        Get
            Return SafeDivision(VolumeDeposition_Error, AreaDeposition_Thresholded)
        End Get
    End Property

    Public ReadOnly Property AverageDepthDeposition_Percent As Double
        Get
            Return 100 * SafeDivision(AverageDepthDeposition_Error, AverageDepthDeposition_Thresholded)
        End Get
    End Property

#End Region


#Region "Vertical Averages Total Thickness of Difference for AOI"

    Public ReadOnly Property AverageThicknessOfDifferenceAOI_Raw As Double
        Get
            Return SafeDivision(VolumeOfDifference_Raw, AreaOfInterest_Raw)
        End Get
    End Property

    Public ReadOnly Property AverageThicknessOfDifferenceAOI_Thresholded As Double
        Get
            Return SafeDivision(VolumeOfDifference_Thresholded, AreaOfInterest_Raw)
        End Get
    End Property

    Public ReadOnly Property AverageThicknessOfDifferenceAOI_Error As Double
        Get
            Return SafeDivision(VolumeOfDifference_Error, AreaOfInterest_Raw)
        End Get
    End Property

    Public ReadOnly Property AverageThicknessOfDifferenceAOI_Percent As Double
        Get
            Return 100 * SafeDivision(AverageThicknessOfDifferenceAOI_Error, AverageThicknessOfDifferenceAOI_Thresholded)
        End Get
    End Property

#End Region

#Region "Vertical Averages **NET** Thickness of Difference for Area of Interest (AOI)"

    Public ReadOnly Property AverageNetThicknessofDifferenceAOI_Raw As Double
        Get
            Return SafeDivision(NetVolumeOfDifference_Raw, AreaOfInterest_Raw)
        End Get
    End Property

    Public ReadOnly Property AverageNetThicknessOfDifferenceAOI_Thresholded As Double
        Get
            Return SafeDivision(NetVolumeOfDifference_Thresholded, AreaOfInterest_Raw)
        End Get
    End Property

    Public ReadOnly Property AverageNetThicknessOfDifferenceAOI_Error As Double
        Get
            Return SafeDivision(NetVolumeOfDifference_Error, AreaOfInterest_Raw)
        End Get
    End Property

    Public ReadOnly Property AverageNetThicknessOfDifferenceAOI_Percent As Double
        Get
            Return 100 * SafeDivision(AverageNetThicknessOfDifferenceAOI_Error, AverageNetThicknessOfDifferenceAOI_Thresholded)
        End Get
    End Property

#End Region

#Region "Vertical Averages **Total** Thickness of Difference for Area of Detectable Change (ADC)"

    Public ReadOnly Property AverageThicknessOfDifferenceADC_Thresholded As Double
        Get
            Return SafeDivision(VolumeOfDifference_Thresholded, AreaDetectableChange_Thresholded)
        End Get
    End Property

    Public ReadOnly Property AverageThicknessOfDifferenceADC_Error As Double
        Get
            Return SafeDivision(VolumeOfDifference_Error, AreaDetectableChange_Thresholded)
        End Get
    End Property

    Public ReadOnly Property AverageThicknessOfDifferenceADC_Percent As Double
        Get
            Return 100 * SafeDivision(AverageThicknessOfDifferenceADC_Error, AverageThicknessOfDifferenceADC_Thresholded)
        End Get
    End Property

#End Region

#Region "Vertical Averages **NET** Thickness of Difference for Area of Detecktable Change (ADC)"

    Public ReadOnly Property AverageNetThicknessOfDifferenceADC_Thresholded As Double
        Get
            Return SafeDivision(NetVolumeOfDifference_Thresholded, AreaDetectableChange_Thresholded)
        End Get
    End Property

    Public ReadOnly Property AverageNetThicknessOfDifferenceADC_Error As Double
        Get
            Return SafeDivision(NetVolumeOfDifference_Error, AreaDetectableChange_Thresholded)
        End Get
    End Property

    Public ReadOnly Property AverageNetThicknessOfDifferenceADC_Percent As Double
        Get
            Return 100 * SafeDivision(AverageNetThicknessOfDifferenceADC_Error, AverageNetThicknessOfDifferenceADC_Thresholded)
        End Get
    End Property

#End Region

#Region "Percentages By Volume"

    Public ReadOnly Property PercentErosion_Raw As Double
        Get
            Return 100 * SafeDivision(VolumeErosion_Raw, VolumeOfDifference_Raw)
        End Get
    End Property

    Public ReadOnly Property PercentErosion_Thresholded As Double
        Get
            Return 100 * SafeDivision(VolumeErosion_Thresholded, VolumeOfDifference_Thresholded)
        End Get
    End Property

    Public ReadOnly Property PercentDeposition_Raw As Double
        Get
            Return 100 * SafeDivision(VolumeDeposition_Raw, VolumeOfDifference_Raw)
        End Get
    End Property

    Public ReadOnly Property PercentDeposition_Thresholded As Double
        Get
            Return 100 * SafeDivision(VolumeDeposition_Thresholded, VolumeOfDifference_Thresholded)
        End Get
    End Property

    Public ReadOnly Property PercentImbalance_Raw As Double
        Get
            Return 100 * SafeDivision(NetVolumeOfDifference_Raw, (2 * VolumeOfDifference_Raw))
        End Get
    End Property

    Public ReadOnly Property PercentImbalance_Thresholded As Double
        Get
            Return 100 * SafeDivision(NetVolumeOfDifference_Thresholded, (2 * VolumeOfDifference_Thresholded))
        End Get
    End Property

    Public ReadOnly Property NetToTotalVolumeRatio_Raw As Double
        Get
            Return 100 * SafeDivision(NetVolumeOfDifference_Raw, VolumeOfDifference_Raw)
        End Get
    End Property

    Public ReadOnly Property NetToTotalVolumeRatio_Thresholded As Double
        Get
            Return 100 * SafeDivision(NetVolumeOfDifference_Thresholded, VolumeOfDifference_Thresholded)
        End Get
    End Property

#End Region

#End Region

    Private Function SafeDivision(fNumerator As Double, fDenominator As Double) As Double
        Dim fResult As Double = 0
        If fNumerator <> 0 AndAlso fDenominator <> 0 Then
            fResult = fNumerator / fDenominator
        End If
        Return fResult
    End Function

    ''' <summary>
    ''' Creates a new change statistics class. Note: This is a non-instantiable class
    ''' </summary>
    ''' <param name="sDodRaw">Raw DoD Raster Path</param>
    ''' <param name="sDodThresh">Thresholded DoD Raster Path</param>
    ''' <param name="fCellSize">Cell Resolution / Size</param>
    ''' <remarks></remarks>
    Public Sub New(sDodRaw As String, sDodThresh As String, fCellSize As Double)

        If fCellSize <= 0 Then
            Throw New ArgumentOutOfRangeException("Cell Size", fCellSize, "The cell size must be greater than zero.")
        End If
        m_fCellArea = fCellSize ^ 2

        'Erosiom Raw
        CalculateAreaAndVolume(sDodRaw, """Value"" < 0", m_fCellArea, m_fAreaErosion_Raw, m_fVolumeErosion_Raw)
        CalculateAreaAndVolume(sDodThresh, """Value"" < 0", m_fCellArea, m_fAreaErosion_Thresholded, m_fVolumeErosion_Thresholded)

        ' Deposition
        CalculateAreaAndVolume(sDodRaw, """Value"" > 0", m_fCellArea, m_fAreaDeposition_Raw, m_fVolumeDeposition_Raw)
        CalculateAreaAndVolume(sDodThresh, """Value"" > 0", m_fCellArea, m_fAreaDeposition_Thresholded, m_fVolumeDeposition_Thresholded)

    End Sub

    ''' <summary>
    ''' Calculates the basic area and volume statistics. Use the Con Statement to select either erosion or deposition cells.
    ''' </summary>
    ''' <param name="sRasterPath">DoD Raster Path (can be either raw or thresholded)</param>
    ''' <param name="sConStatement">""Value"" &lt; 0" for erosion and """Value"" &gt; 0" for deposition</param>
    ''' <param name="fCellArea">Area of one cell</param>
    ''' <param name="fArea">Output value of the area of cells that meet the condition of the Con Statement</param>
    ''' <param name="fVolume">Output value of the volume of cells that meet the condition of the Con Statement</param>
    ''' <remarks></remarks>
    Private Sub CalculateAreaAndVolume(sRasterPath As String, sConStatement As String, fCellArea As Double, ByRef fArea As Double, ByRef fVolume As Double)

        fArea = 0
        fVolume = 0

        Dim sZoneRaster As String = WorkspaceManager.GetTempRaster("TempStats")
        GP.SpatialAnalyst.Con_sa(sRasterPath, "1", "0", New IO.FileInfo(sZoneRaster), sConStatement)

        Dim pWS As ESRI.ArcGIS.Geodatabase.IWorkspace = GISDataStructures.GetWorkspace(WorkspaceManager.WorkspacePath & IO.Path.DirectorySeparatorChar, GISDataStructures.GISDataStorageTypes.ShapeFile)
        Dim sStatistics As String = GISCode.Table.GetSafeName(pWS, "ChangeStats")

        GP.SpatialAnalyst.ZonalStatisticsAsTable(sZoneRaster, "Value", sRasterPath, sStatistics, "ALL")
        Dim pTable As ESRI.ArcGIS.Geodatabase.ITable = GISCode.Table.OpenDBFTable(sStatistics)

        Dim dicSum As Dictionary(Of Integer, Double) = GISCode.Table.GetValuesFromTable(pTable, "Value", "SUM")
        Dim dicArea As Dictionary(Of Integer, Double) = GISCode.Table.GetValuesFromTable(pTable, "Value", "AREA")

        If dicArea.ContainsKey(1) Then
            fArea = dicArea(1)
            If dicSum.ContainsKey(1) Then
                ' Need to do abs because the sum of values is negative when erosion
                fVolume = Math.Abs(dicSum(1)) * CellArea
            End If
        End If

        Runtime.InteropServices.Marshal.ReleaseComObject(pTable)
        Runtime.InteropServices.Marshal.ReleaseComObject(pWS)

    End Sub

    ''' <summary>
    ''' Output the statistics to an XML text writer.
    ''' </summary>
    ''' <param name="xmlResults">XML text writer</param>
    ''' <remarks>This method will only output the actual value XML elements. It's assumed that other code
    ''' is responsible for surrounding these methods with the appropriate tags.</remarks>
    Public Sub OutputCHaMPResults(xmlResults As Xml.XmlTextWriter)

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' AREAS
        xmlResults.WriteElementString("raw_area_erosion", AreaErosion_Raw.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("raw_area_deposition", AreaDeposition_Raw.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("thresholded_area_erosion", AreaErosion_Thresholded.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("thresholded_area_deposition", AreaDeposition_Thresholded.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))

        xmlResults.WriteElementString("area_detectable_change", AreaDetectableChange_Thresholded.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("area_of_interest_raw", AreaOfInterest_Raw.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("percent_area_of_interest_detectable_change", AreaPercentAreaInterestWithDetectableChange.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' VOLUMES
        xmlResults.WriteElementString("raw_volume_erosion", VolumeErosion_Raw.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("thresholded_volume_erosion", VolumeErosion_Thresholded.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("error_volume_erosion", VolumeErosion_Error.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("thresholded_percent_erosion", VolumeOfErosion_Percent.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))

        xmlResults.WriteElementString("raw_volume_deposition", VolumeDeposition_Raw.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("thresholded_volume_deposition", VolumeDeposition_Thresholded.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("error_volume_deposition", VolumeDeposition_Error.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("thresholded_percent_deposition", VolumeOfDeposition_Percent.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))

        xmlResults.WriteElementString("raw_volume_difference", VolumeOfDifference_Raw.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("thresholded_volume_difference", VolumeOfDifference_Thresholded.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("error_volume_difference", VolumeOfDifference_Error.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("volume_difference_percent", VolumeOfDifference_Percent.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' VERTICAL AVERAGES
        xmlResults.WriteElementString("average_depth_erosion_raw", AverageDepthErosion_Raw.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("average_depth_erosion_thresholded", AverageDepthErosion_Thresholded.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("average_depth_erosion_error", AverageDepthErosion_Error.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("average_depth_erosion_percent", AverageDepthErosion_Percent.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))

        xmlResults.WriteElementString("average_depth_deposition_raw", AverageDepthDeposition_Raw.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("average_depth_deposition_thresholded", AverageDepthDeposition_Thresholded.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("average_depth_deposition_error", AverageDepthDeposition_Error.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("average_depth_deposition_percent", AverageDepthDeposition_Percent.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))

        xmlResults.WriteElementString("average_thickness_difference_aoi_raw", AverageThicknessOfDifferenceAOI_Raw.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("average_thickness_difference_aoi_thresholded", AverageThicknessOfDifferenceAOI_Thresholded.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("average_thickness_difference_aoi_error", AverageThicknessOfDifferenceAOI_Error.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("average_thickness_difference_aoi_percent", AverageThicknessOfDifferenceAOI_Percent.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))

        xmlResults.WriteElementString("average_net_thickness_difference_aoi_raw", AverageNetThicknessofDifferenceAOI_Raw.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("average_net_thickness_difference_aoi_thresholded", AverageNetThicknessOfDifferenceAOI_Thresholded.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("average_net_thickness_difference_aoi_error", AverageNetThicknessOfDifferenceAOI_Error.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("average_net_thickness_difference_aoi_percent", AverageNetThicknessOfDifferenceAOI_Percent.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))

        xmlResults.WriteElementString("average_thickness_difference_adc_thresholded", AverageThicknessOfDifferenceADC_Thresholded.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("average_thickness_difference_adc_error", AverageThicknessOfDifferenceADC_Error.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("average_thickness_difference_adc_percent", AverageThicknessOfDifferenceADC_Percent.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))

        xmlResults.WriteElementString("average_net_thickness_difference_adc_thresholded", AverageNetThicknessOfDifferenceADC_Thresholded.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("average_net_thickness_difference_adc_error", AverageNetThicknessOfDifferenceADC_Error.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("average_net_thickness_difference_adc_percent", AverageNetThicknessOfDifferenceADC_Percent.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' PERCENTAGES
        xmlResults.WriteElementString("percent_erosion_raw", PercentErosion_Raw.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("percent_erosion_thresholded", PercentErosion_Thresholded.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))

        xmlResults.WriteElementString("percent_deposition_raw", PercentDeposition_Raw.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("percent_deposition_thresholded", PercentDeposition_Thresholded.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))

        xmlResults.WriteElementString("percent_imbalance_raw", PercentImbalance_Raw.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("percent_imbalance_thresholded", PercentImbalance_Thresholded.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))

        xmlResults.WriteElementString("percent_net_volume_ratio_raw", NetToTotalVolumeRatio_Raw.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))
        xmlResults.WriteElementString("percent_net_volume_ratio_thresholded", NetToTotalVolumeRatio_Thresholded.ToString(RBTConsole.RBTEngine.Config.PrecisionFormatString))

    End Sub

    ''' <summary>
    ''' Write the statistics to a new copy of the GCDSummary.xml spreadsheet XML file.
    ''' </summary>
    ''' <param name="sDisplayUnits">Units of measurement for the statistics. e.g. "ft"</param>
    ''' <param name="sOutputPath">Full path to the output XML path where the GCD Summary stats XML file should be generated</param>
    ''' <remarks></remarks>
    Public Sub ExportSummary(sExcelTemplateFolder As String, ByVal sDisplayUnits As String, ByVal sOutputPath As String)

        Dim TemplateFile As String = IO.Path.Combine(sExcelTemplateFolder, "GCDSummary.xml")
        If Not IO.File.Exists(TemplateFile) Then
            Throw New Exception("Could not find required template '" & TemplateFile & "'")
        End If

        ' Read the template file
        Dim objReader As System.IO.StreamReader
        Try
            objReader = New System.IO.StreamReader(TemplateFile)
        Catch ex As System.IO.IOException
            'need to limit the length of the file to display properly
            Dim TrimmedFilename As String = GISCode.FileSystem.TrimFilename(TemplateFile, 80)
            ex.Data("UIMessage") = "Could not access the file '" & TrimmedFilename & "' because it is being used by another program."
            Throw
        End Try

        Dim OutputText As Text.StringBuilder = New Text.StringBuilder(objReader.ReadToEnd())
        objReader.Close()
        '
        ' PGB 3 Mar 2012
        ' Need to dynamically put the units in the summary XML.
        '
        OutputText.Replace("[LinearUnits]", sDisplayUnits)

        OutputText.Replace("[TotalAreaOfErosionRaw]", AreaErosion_Raw)
        OutputText.Replace("[TotalAreaOfErosionThresholded]", AreaErosion_Thresholded)

        OutputText.Replace("[TotalAreaOfDepositionRaw]", AreaDeposition_Raw)
        OutputText.Replace("[TotalAreaOfDepositionThresholded]", AreaDeposition_Thresholded)

        OutputText.Replace("[TotalVolumeOfErosionRaw]", VolumeErosion_Raw)
        OutputText.Replace("[TotalVolumeOfErosionThresholded]", m_fVolumeErosion_Thresholded)
        OutputText.Replace("[ErrorVolumeOfErosion]", VolumeErosion_Error)

        OutputText.Replace("[TotalVolumeOfDepositionRaw]", VolumeDeposition_Raw)
        OutputText.Replace("[TotalVolumeOfDepositionThresholded]", m_fVolumeDeposition_Thresholded)
        OutputText.Replace("[ErrorVolumeOfDeposition]", VolumeDeposition_Error)

        ' Write the template file
        Dim objWriter As New System.IO.StreamWriter(sOutputPath)
        objWriter.Write(OutputText)
        objWriter.Close()

    End Sub

End Class
