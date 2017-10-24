
Namespace Core

    Public Class GCDArcMapManager

        Private m_fDefaultDEMTransparency As Double

        ' These constants are the names for the group layers that should be greated
        ' and the appropriate rasters created inside them
        Private Const m_sInputsGroupLayer As String = "Inputs"
        Private Const m_sAssociatedSurfacesGroupLayer As String = "Associated Surfaces"
        Private Const m_sErrorSurfacesGroupLayer As String = "Error Surfaces"
        Private Const m_sAOIsGroupLayer As String = "AOIs"
        Private Const m_sAnalysesGroupLayer As String = "Analyses"

        Public Sub New(Optional ByVal fDefaultDEMTransparency As Double = 40)
            m_fDefaultDEMTransparency = fDefaultDEMTransparency
        End Sub


        Public Sub AddSurvey(demRow As GCDLib.ProjectDS.DEMSurveyRow)

        End Sub

















        '    ''' <summary>
        '    ''' Add a group layer node to the TOC top level for the GCD project
        '    ''' </summary>
        '    ''' <param name="projRow"></param>
        '    ''' <returns></returns>
        '    ''' <remarks>This method is private because nowhere outside this class
        '    ''' should need to add an empty node to the TOC for a project. The only
        '    ''' purpose for adding the project node to the TOC is to then add other
        '    ''' GCD layers as children via other methods in this class.</remarks>
        '    Private Function AddProjectGroupLayer(projRow As ProjectDS.ProjectRow) As IGroupLayer

        '        Dim pProjectGrpLyr As IGroupLayer = GISCode.ArcMap.GetGroupLayer(m_pArcMap, projRow.Name)
        '        Return pProjectGrpLyr

        '    End Function

        '    ''' <summary>
        '    ''' Adds the entire contents of a GCD project to ArcMap (all DEMs, associated and error surfaces and DoDs)
        '    ''' </summary>
        '    ''' <returns></returns>
        '    ''' <remarks></remarks>
        '    Public Function AddProject() As IGroupLayer

        '        ' should only be one project in the dataset, but loop just to be sure
        '        Dim pProjectGrpLyr As IGroupLayer = Nothing
        '        For Each projRow In GCD.GCDProject.ProjectManagerBase.ds.Project
        '            pProjectGrpLyr = AddProjectGroupLayer(projRow)

        '            For Each demRow As ProjectDS.DEMSurveyRow In projRow.GetDEMSurveyRows

        '                ' add the survey and all of it's associated and error surfaces
        '                AddSurvey(demRow)
        '            Next

        '            For Each dodRow As ProjectDS.DoDsRow In GCD.GCDProject.ProjectManagerBase.ds.DoDs
        '                AddDoD(dodRow)
        '            Next
        '        Next

        '        Return pProjectGrpLyr

        '    End Function

        '    Private Function AddInputsGroupLayer(projRow As ProjectDS.ProjectRow) As IGroupLayer

        '        ' Ensure the group layer for the project exists
        '        Dim pProjectGrpLyr As IGroupLayer = AddProjectGroupLayer(projRow)
        '        Dim pInputsGrpLyr As IGroupLayer = GISCode.ArcMap.GetGroupLayer(m_pArcMap, m_sInputsGroupLayer, pProjectGrpLyr)
        '        Return pInputsGrpLyr

        '    End Function

        '    Private Function AddSurveyGroupLayer(demRow As ProjectDS.DEMSurveyRow) As IGroupLayer

        '        ' Ensure the group layer for the inputs exists
        '        Dim pInputsGrpLyr As IGroupLayer = AddInputsGroupLayer(demRow.ProjectRow)
        '        Dim pSurveyLyr As IGroupLayer = GISCode.ArcMap.GetGroupLayer(m_pArcMap, demRow.Name, pInputsGrpLyr)
        '        Return pSurveyLyr

        '    End Function

        '    ''' <summary>
        '    ''' Add a whole survey (DEM and its associated and error surfaces)
        '    ''' </summary>
        '    ''' <param name="demRow"></param>
        '    ''' <returns></returns>
        '    ''' <remarks></remarks>
        '    Public Function AddSurvey(demRow As ProjectDS.DEMSurveyRow) As IGroupLayer

        '        Dim pSurveyLyr As IGroupLayer = AddSurveyGroupLayer(demRow)

        '        ' Finally add the DEM and it's hillshade
        '        AddDEM(demRow)

        '        For Each assocRow As ProjectDS.AssociatedSurfaceRow In demRow.GetAssociatedSurfaceRows
        '            AddAssociatedSurface(assocRow)
        '        Next

        '        For Each errRow As ProjectDS.ErrorSurfaceRow In demRow.GetErrorSurfaceRows
        '            AddErrSurface(errRow)
        '        Next

        '        Return pSurveyLyr

        '    End Function

        '    ''' <summary>
        '    ''' 
        '    ''' </summary>
        '    ''' <param name="demRow"></param>
        '    ''' <returns></returns>
        '    ''' <remarks>Note: Add the hillshade first so that it appear UNDER the DEM in the TOC</remarks>
        '    Public Function AddDEM(demRow As ProjectDS.DEMSurveyRow) As IRasterLayer

        '        Dim fDEMTransparency As Double = -1
        '        Dim pSurveyLyr As IGroupLayer = AddSurveyGroupLayer(demRow)
        '        Dim sHillShadePath As String = GCD.GCDProject.ProjectManagerBase.OutputManager.DEMSurveyHillShadeRasterPath(demRow.Name)
        '        If GCDConsoleLib.Raster.Exists(sHillShadePath) Then
        '            AddToMapRaster(sHillShadePath, demRow.Name & " HillShade", pSurveyLyr)
        '            fDEMTransparency = m_fDefaultDEMTransparency
        '            'pSurveyLyr.Expanded = False
        '        End If

        '        Dim sSymbology As String = GCDConsoleLib.RasterGCD.GetSymbologyLayerFile(ArcMap.RasterLayerTypes.DEM)
        '        Dim sRasterPath As String = GCD.GCDProject.ProjectManagerBase.GetAbsolutePath(demRow.Source)
        '        Dim pDEMLyr As IRasterLayer = AddToMapRaster(sRasterPath, demRow.Name, pSurveyLyr, sSymbology, fDEMTransparency)

        '        'Collapse the Hillshade legend in the TOC
        '        Dim pCompositeLyr As ICompositeLayer = DirectCast(pSurveyLyr, ICompositeLayer)
        '        For i As Integer = 0 To pCompositeLyr.Count - 1
        '            If pCompositeLyr.Layer(i).Name.Contains("HillShade") Then
        '                Dim hsLyr As IRasterLayer = DirectCast(pCompositeLyr.Layer(i), IRasterLayer)
        '                Dim legendInfo As ILegendInfo = DirectCast(hsLyr, ILegendInfo)
        '                Dim legendGroup As ILegendGroup = DirectCast(legendInfo.LegendGroup(0), ILegendGroup)
        '                legendGroup.Visible = False

        '                'Commented out methods below are not valid on IMapDocuments, due to lack of control of TOC
        '                'Refresh updates "TOC" of IMapDocument, Me.MXDoc refers to old methods on IMxDocument
        '                Dim pMXDoc As ESRI.ArcGIS.ArcMapUI.IMxDocument = m_pArcMap.Document
        '                pMXDoc.UpdateContents()
        '                pMXDoc.ActiveView.Refresh()
        '                pMXDoc.CurrentContentsView.Refresh(Nothing)

        '                'Me.MapDocument.ActiveView.Refresh()
        '            End If
        '        Next

        '        Return pDEMLyr

        '    End Function

        '    Private Function AddAssociatedSurfaceGroupLayer(demRow As ProjectDS.DEMSurveyRow) As IGroupLayer

        '        Dim pSurveyGrpLyr As IGroupLayer = AddSurveyGroupLayer(demRow)
        '        Dim pAssocGrpLyr As IGroupLayer = GISCode.ArcMap.GetGroupLayer(m_pArcMap, m_sAssociatedSurfacesGroupLayer, pSurveyGrpLyr)
        '        Return pAssocGrpLyr

        '    End Function

        '    Public Function AddAssociatedSurface(assocRow As ProjectDS.AssociatedSurfaceRow) As IRasterLayer

        '        Dim pAssocGrpLyr As IGroupLayer = AddAssociatedSurfaceGroupLayer(assocRow.DEMSurveyRow)
        '        Dim eType As ArcMap.RasterLayerTypes = ProjectDS.GetAssociatedSurfaceType(assocRow)
        '        Dim sRasterPath As String = GCD.GCDProject.ProjectManagerBase.GetAbsolutePath(assocRow.Source)
        '        Dim gAssociatedRaster As GCDConsoleLib.Raster = New GCDConsoleLib.Raster(sRasterPath)
        '        Dim sHeader As String = GetLayerHeader(assocRow)
        '        Dim dTransparency As Double = -1
        '        If My.Settings.TransparencyAssociatedLayers Then
        '            dTransparency = My.Settings.AutoTransparencyValue
        '        End If

        '        If eType = ArcMap.RasterLayerTypes.InterpolationError Then

        '            If My.Settings.ApplyComparativeSymbology = False Then
        '                'Dim rasterRenderer As IRasterRenderer = GCD.RasterSymbolization.CreateESRIDefinedContinuousRenderer(gAssociatedRaster, 0, "Slope")
        '                Dim rasterRenderer As IRasterRenderer = GCD.RasterSymbolization.CreateClassifyRenderer(gAssociatedRaster, 11, "Slope")
        '                Dim pAssocLyr As ILayer = AddRasterLayer(m_pArcMap.Document, gAssociatedRaster, rasterRenderer, assocRow.Name, pAssocGrpLyr, sHeader, dTransparency)
        '                Return pAssocLyr

        '            ElseIf My.Settings.ApplyComparativeSymbology And My.Settings.ComparativeSymbologyInterpolationError Then

        '            End If

        '        ElseIf eType = ArcMap.RasterLayerTypes.PointQuality Then

        '            If My.Settings.ApplyComparativeSymbology = False Then

        '                'Dim rasterRenderer As IRasterRenderer = GCD.RasterSymbolization.CreateESRIDefinedContinuousRenderer(gAssociatedRaster, 0, "Precipitation")
        '                Dim rasterRenderer As IRasterRenderer = GCD.RasterSymbolization.CreateClassifyRenderer(gAssociatedRaster, 11, "Precipitation", True)
        '                Dim pAssocLyr As ILayer = AddRasterLayer(m_pArcMap.Document, gAssociatedRaster, rasterRenderer, assocRow.Name, pAssocGrpLyr, sHeader, dTransparency)
        '                Return pAssocLyr

        '            ElseIf My.Settings.ApplyComparativeSymbology And My.Settings.ComparativeSymbology3dPointQuality Then


        '            End If

        '        ElseIf eType = ArcMap.RasterLayerTypes.PointDensity Then

        '            If My.Settings.ApplyComparativeSymbology = False Then

        '                'Check raster statistics to see if it is appropriate to apply our scale
        '                If gAssociatedRaster.Maximum <= 2 And gAssociatedRaster.Maximum > 0.25 Then
        '                    Dim rasterRenderer As IRasterRenderer = GCD.RasterSymbolization.CreateClassifyRenderer(gAssociatedRaster, 11, "Green to Blue", 1.1, True)
        '                    Dim pAssocLyr As ILayer = AddRasterLayer(m_pArcMap.Document, gAssociatedRaster, rasterRenderer, assocRow.Name, pAssocGrpLyr, sHeader, dTransparency)
        '                    Return pAssocLyr
        '                Else
        '                    'Dim rasterRenderer As IRasterRenderer = GCD.RasterSymbolization.CreateESRIDefinedContinuousRenderer(gAssociatedRaster, 11, "Green to Blue", True)
        '                    Dim rasterRenderer As IRasterRenderer = GCD.RasterSymbolization.CreateClassifyRenderer(gAssociatedRaster, 11, "Green to Blue", True)
        '                    Dim pAssocLyr As ILayer = AddRasterLayer(m_pArcMap.Document, gAssociatedRaster, rasterRenderer, assocRow.Name, pAssocGrpLyr, sHeader, dTransparency)
        '                    Return pAssocLyr
        '                End If

        '            ElseIf My.Settings.ApplyComparativeSymbology And My.Settings.ComparativeSymbologyPointDensity Then


        '            End If

        '        ElseIf eType = ArcMap.RasterLayerTypes.GrainSizeStatistic Then

        '            'TODO: Currently this only works from a CHaMP perspective as these rasters are created with z as mm, symbology needs to be implemented for other units
        '            Dim eUnits As NumberFormatting.LinearUnits = NumberFormatting.GetLinearUnitsFromString(GCD.GCDProject.ProjectManagerBase.DisplayUnits.ToString())
        '            Dim rasterRenderer As IRasterRenderer = GCD.RasterSymbolization.CreateGrainSizeStatisticColorRamp(gAssociatedRaster, eUnits)
        '            Dim pAssocLyr As ILayer = AddRasterLayer(m_pArcMap.Document, gAssociatedRaster, rasterRenderer, assocRow.Name, pAssocGrpLyr, sHeader, dTransparency)
        '            Return pAssocLyr

        '        ElseIf eType = ArcMap.RasterLayerTypes.Roughness Then

        '            'Dim sSymbology As String = GCDConsoleLib.RasterGCD.GetSymbologyLayerFile(eType)
        '            'Dim pAssocLyr As IRasterLayer = AddToMapRaster(sRasterPath, assocRow.Name, pAssocGrpLyr, sSymbology, dTransparency, sHeader)
        '            Dim rasterRenderer As IRasterRenderer = GCD.RasterSymbolization.CreateRoughnessColorRamp(gAssociatedRaster)
        '            Dim pAssocLyr As ILayer = AddRasterLayer(m_pArcMap.Document, gAssociatedRaster, rasterRenderer, assocRow.Name, pAssocGrpLyr, sHeader, dTransparency)
        '            Return pAssocLyr

        '        ElseIf eType = ArcMap.RasterLayerTypes.SlopeDegrees Then

        '            Dim rasterRenderer As IRasterRenderer = GCD.RasterSymbolization.CreateSlopeDegreesColorRamp(gAssociatedRaster)
        '            Dim pAssocLyr As ILayer = AddRasterLayer(m_pArcMap.Document, gAssociatedRaster, rasterRenderer, assocRow.Name, pAssocGrpLyr, sHeader, dTransparency)
        '            Return pAssocLyr

        '        ElseIf eType = ArcMap.RasterLayerTypes.SlopePercentRise Then

        '            'Dim sSymbology As String = GCDConsoleLib.RasterGCD.GetSymbologyLayerFile(eType)
        '            'Dim pAssocLyr As IRasterLayer = AddToMapRaster(sRasterPath, assocRow.Name, pAssocGrpLyr, sSymbology, dTransparency, sHeader)
        '            Dim rasterRenderer As IRasterRenderer = GCD.RasterSymbolization.CreateSlopePrecentRiseColorRamp(gAssociatedRaster)
        '            Dim pAssocLyr As ILayer = AddRasterLayer(m_pArcMap.Document, gAssociatedRaster, rasterRenderer, assocRow.Name, pAssocGrpLyr, sHeader, dTransparency)
        '            Return pAssocLyr

        '        ElseIf eType <> GISCode.ArcMap.RasterLayerTypes.Undefined Then

        '            Dim sSymbology As String = GCDConsoleLib.RasterGCD.GetSymbologyLayerFile(eType)
        '            Dim pAssocLyr As IRasterLayer = AddToMapRaster(sRasterPath, assocRow.Name, pAssocGrpLyr, sSymbology, dTransparency, sHeader)
        '            Return pAssocLyr

        '        Else

        '            Dim pAssocLyr As IRasterLayer = AddToMapRaster(sRasterPath, assocRow.Name, pAssocGrpLyr)
        '            Return pAssocLyr

        '        End If

        '        Throw New Exception("An unrecognized Raster Layer Type was used.")

        '    End Function

        '    Private Function AddErrorSurfacesGroupLayer(demRow As ProjectDS.DEMSurveyRow) As IGroupLayer

        '        Dim pSurveyGrpLyr As IGroupLayer = AddSurveyGroupLayer(demRow)
        '        Dim pErrGrpLyr As IGroupLayer = GISCode.ArcMap.GetGroupLayer(m_pArcMap, m_sErrorSurfacesGroupLayer, pSurveyGrpLyr)
        '        Return pErrGrpLyr

        '    End Function

        '    Public Function AddErrSurface(errRow As ProjectDS.ErrorSurfaceRow) As IRasterLayer

        '        Dim pErrGrpLyr As IGroupLayer = AddErrorSurfacesGroupLayer(errRow.DEMSurveyRow)
        '        Dim sRasterPath As String = GCD.GCDProject.ProjectManagerBase.GetAbsolutePath(errRow.Source)
        '        Dim gErrRaster As GCDConsoleLib.Raster = New GCDConsoleLib.Raster(sRasterPath)
        '        Dim sHeader As String = "Error (" & errRow.DEMSurveyRow.ProjectRow.DisplayUnits & ")"

        '        Dim dTransparency As Double = -1
        '        If My.Settings.TransparencyErrorLayers Then
        '            dTransparency = My.Settings.AutoTransparencyValue
        '        End If

        '        'Check raster statistics to see if it is appropriate to apply our scale
        '        If gErrRaster.Minimum = gErrRaster.Maximum Then
        '            Dim rasterRenderer As IRasterRenderer = GCD.RasterSymbolization.CreateESRIDefinedContinuousRenderer(gErrRaster, 1, "Partial Spectrum")
        '            Dim pAssocLyr As ILayer = AddRasterLayer(m_pArcMap.Document, gErrRaster, rasterRenderer, errRow.Name, pErrGrpLyr, sHeader, dTransparency)
        '            Return pAssocLyr
        '        ElseIf gErrRaster.Maximum <= 1 And gErrRaster.Maximum > 0.25 Then
        '            Dim rasterRenderer As IRasterRenderer = GCD.RasterSymbolization.CreateClassifyRenderer(gErrRaster, 11, "Partial Spectrum", 1.1)
        '            Dim pAssocLyr As ILayer = AddRasterLayer(m_pArcMap.Document, gErrRaster, rasterRenderer, errRow.Name, pErrGrpLyr, sHeader, dTransparency)
        '            Return pAssocLyr
        '        Else
        '            'Dim rasterRenderer As IRasterRenderer = GCD.RasterSymbolization.CreateESRIDefinedContinuousRenderer(gErrRaster, 11, "Partial Spectrum")
        '            Dim rasterRenderer As IRasterRenderer = GCD.RasterSymbolization.CreateClassifyRenderer(gErrRaster, 11, "Partial Spectrum")
        '            Dim pAssocLyr As ILayer = AddRasterLayer(m_pArcMap.Document, gErrRaster, rasterRenderer, errRow.Name, pErrGrpLyr, sHeader, dTransparency)
        '            Return pAssocLyr
        '        End If

        '    End Function

        '    Private Function AddAOIGroupLayer(rProjectRow As ProjectDS.ProjectRow)
        '        Dim pProjectGrpLyr As IGroupLayer = AddProjectGroupLayer(rProjectRow)
        '        Dim pAOIGrpLyr As IGroupLayer = GISCode.ArcMap.GetGroupLayer(m_pArcMap, m_sAOIsGroupLayer, pProjectGrpLyr)
        '        Return pAOIGrpLyr
        '    End Function

        '    Public Function AddAOI(rAOI As ProjectDS.AOIsRow) As IFeatureLayer

        '        Dim pAOIGrpLyr As IGroupLayer = AddAOIGroupLayer(rAOI.ProjectRow)
        '        Dim sFullPath As String = GCD.GCDProject.ProjectManagerBase.GetAbsolutePath(rAOI.Source)
        '        Dim pAOILyr As IFeatureLayer = AddToMapVector(sFullPath, rAOI.Name, pAOIGrpLyr)
        '        Return pAOILyr

        '    End Function

        '    Private Function AddAnalysesGroupLayer(projRow As ProjectDS.ProjectRow) As IGroupLayer

        '        Dim pProjLyr As IGroupLayer = AddProjectGroupLayer(projRow)
        '        Dim pAnalGrpLyr As IGroupLayer = GISCode.ArcMap.GetGroupLayer(m_pArcMap, m_sAnalysesGroupLayer, pProjLyr)
        '        Return pAnalGrpLyr

        '    End Function

        '    Public Function AddDoD(dodRow As ProjectDS.DoDsRow, Optional bThresholded As Boolean = True) As IRasterLayer

        '        Dim sRasterPath As String
        '        Dim sLayerName As String = dodRow.Name
        '        If bThresholded Then
        '            sRasterPath = GCD.GCDProject.ProjectManagerBase.GetAbsolutePath(dodRow.ThreshDoDPath)
        '            sLayerName &= " (Thresholded)"
        '        Else
        '            sRasterPath = GCD.GCDProject.ProjectManagerBase.GetAbsolutePath(dodRow.RawDoDPath)
        '            sLayerName &= " (Raw)"
        '        End If

        '        Dim pAnalGrpLayer As IGroupLayer = AddAnalysesGroupLayer(dodRow.ProjectRow)
        '        Dim sSymbology As String = GCDConsoleLib.RasterGCD.GetSymbologyLayerFile(ArcMap.RasterLayerTypes.DoD)
        '        Dim dTransparency As Double = -1
        '        If My.Settings.TransparencyAnalysesLayers Then
        '            dTransparency = My.Settings.AutoTransparencyValue
        '        End If

        '        Dim gDoDRaster As GCDConsoleLib.Raster = New GCDConsoleLib.Raster(sRasterPath)
        '        Dim rasterRenderer As IRasterRenderer = GISCode.GCD.RasterSymbolization.CreateDoDClassifyRenderer(gDoDRaster, 20)
        '        Dim sHeader As String = "Elevation Difference (" & dodRow.ProjectRow.DisplayUnits & ")"
        '        Dim pDoDLyr As ILayer = AddRasterLayer(m_pArcMap.Document, gDoDRaster, rasterRenderer, sLayerName, pAnalGrpLayer, sHeader, dTransparency)
        '        Return pDoDLyr

        '    End Function

        '    ''' <summary>
        '    ''' Add a temporary DoD raster to the map
        '    ''' </summary>
        '    ''' <param name="gRaster"></param>
        '    ''' <param name="sName"></param>
        '    ''' <returns></returns>
        '    ''' <remarks>Added by Philip on 11 Dec 2013 for adding temporary DoDs that have not been
        '    ''' added to the GCD project yet</remarks>
        '    Public Function AddDoDTemp(theProjectRow As ProjectDS.ProjectRow, gRaster As GCDConsoleLib.RasterDirect, sName As String) As IRasterLayer

        '        RemoveDoDPreviewFromTOC(theProjectRow)

        '        Dim pAnalGrpLayer As IGroupLayer = AddAnalysesGroupLayer(theProjectRow)
        '        Dim sSymbology As String = GCDConsoleLib.RasterGCD.GetSymbologyLayerFile(ArcMap.RasterLayerTypes.DoD)
        '        Dim pDoDLyr As IRasterLayer = AddToMapRaster(gRaster.FullPath, sName, pAnalGrpLayer, sSymbology)
        '        Return pDoDLyr

        '    End Function

        '    Public Function AddPropErr(dodRow As ProjectDS.DoDsRow) As IRasterLayer

        '        Dim pAnalGrpLayer As IGroupLayer = AddAnalysesGroupLayer(dodRow.ProjectRow)
        '        Dim sRasterPath As String = GCD.GCDProject.ProjectManagerBase.GetAbsolutePath(dodRow.PropagatedErrorRasterPath)
        '        Dim gPropErrRaster As GCDConsoleLib.Raster = New GCDConsoleLib.Raster(sRasterPath)
        '        Dim sLyrName As String = dodRow.Name & "_" & IO.Path.GetFileNameWithoutExtension(dodRow.PropagatedErrorRasterPath)
        '        Dim sHeader As String = "Error (" & dodRow.ProjectRow.DisplayUnits & ")"

        '        Dim dTransparency As Double = -1
        '        If My.Settings.TransparencyErrorLayers Then
        '            dTransparency = My.Settings.AutoTransparencyValue
        '        End If

        '        If gPropErrRaster.Maximum <= 2 And gPropErrRaster.Maximum > 0.25 Then
        '            Dim rasterRenderer As IRasterRenderer = GISCode.GCD.RasterSymbolization.CreateClassifyRenderer(gPropErrRaster, 11, "Partial Spectrum", 2.1)
        '            Dim pAssocLyr As ILayer = AddRasterLayer(m_pArcMap.Document, gPropErrRaster, rasterRenderer, sLyrName, pAnalGrpLayer, sHeader, dTransparency)
        '            Return pAssocLyr
        '        Else
        '            'Dim rasterRenderer As IRasterRenderer = GISCode.GCD.RasterSymbolization.CreateESRIDefinedContinuousRenderer(gPropErrRaster, 0, "Partial Spectrum")
        '            Dim rasterRenderer As IRasterRenderer = GISCode.GCD.RasterSymbolization.CreateClassifyRenderer(gPropErrRaster, 11, "Partial Spectrum")
        '            Dim pAssocLyr As ILayer = AddRasterLayer(m_pArcMap.Document, gPropErrRaster, rasterRenderer, sLyrName, pAnalGrpLayer, sHeader, dTransparency)
        '            Return pAssocLyr
        '        End If

        '    End Function

        '    Public Function AddProbabilityRaster(dodRow As ProjectDS.DoDsRow, sProbabilityRasterPath As String) As IRasterLayer

        '        Dim pAnalGrpLayer As IGroupLayer = AddAnalysesGroupLayer(dodRow.ProjectRow)
        '        Dim sRasterPath As String = GCD.GCDProject.ProjectManagerBase.GetAbsolutePath(sProbabilityRasterPath)
        '        Dim gProbabilityRaster As GCDConsoleLib.Raster = New GCDConsoleLib.Raster(sRasterPath)
        '        Dim sFileName As String = IO.Path.GetFileNameWithoutExtension(sProbabilityRasterPath)
        '        Dim sLyrName As String = dodRow.Name & "_" & sFileName

        '        Dim dTransparency As Double = -1
        '        If My.Settings.TransparencyAnalysesLayers Then
        '            dTransparency = My.Settings.AutoTransparencyValue
        '        End If

        '        Select Case sFileName
        '            Case "priorProb"
        '                Dim rasterRenderer As IRasterRenderer = GISCode.GCD.RasterSymbolization.CreateProbabilityClassifyRenderer(gProbabilityRaster, 20)
        '                Dim pProbLyr As ILayer = AddRasterLayer(m_pArcMap.Document, gProbabilityRaster, rasterRenderer, sLyrName, pAnalGrpLayer, "Probability (-eros. ; + dep.)", dTransparency)
        '                Return pProbLyr

        '            Case "nbrErosion"
        '                Dim rasterRenderer As IRasterRenderer = GISCode.GCD.RasterSymbolization.CreateESRIDefinedContinuousRenderer(gProbabilityRaster, 10, "Red Light to Dark")
        '                Dim pProbLyr As ILayer = AddRasterLayer(m_pArcMap.Document, gProbabilityRaster, rasterRenderer, sLyrName, pAnalGrpLayer, "Cell Count", dTransparency)
        '                Return pProbLyr

        '            Case "nbrDeposition"
        '                Dim rasterRenderer As IRasterRenderer = GISCode.GCD.RasterSymbolization.CreateESRIDefinedContinuousRenderer(gProbabilityRaster, 10, "Blue Light to Dark")
        '                Dim pProbLyr As ILayer = AddRasterLayer(m_pArcMap.Document, gProbabilityRaster, rasterRenderer, sLyrName, pAnalGrpLayer, "Cell Count", dTransparency)
        '                Return pProbLyr

        '            Case Else
        '                Throw New Exception("Unhandled filename when creating probability raster layer.")

        '        End Select


        '    End Function


        '    ''' <summary>
        '    ''' Add a raster or feature layer to its associated group layer in the current ArcMap document.
        '    ''' </summary>
        '    ''' <param name="sSource">Full path of the GIS data source</param>
        '    ''' <param name="sDisplayName">Name to be used into the Table of Contents</param>
        '    ''' <param name="pGrpLyr">Parent Group Layer under which to create this raster</param>
        '    ''' <param name="sSymbologyLayerFile">Optional symbology layer file</param>
        '    ''' <returns>pointer to the layer added to the map - or the layer that was already in the map</returns>
        '    ''' <remarks>PGB 16 May 2012. New consolidated AddToMap method for both raster and vector.</remarks>
        '    Private Function AddToMapRaster(sSource As String,
        '                                    sDisplayName As String,
        '                                    pGrpLyr As IGroupLayer,
        '                                    Optional sSymbologyLayerFile As String = "",
        '                                    Optional fTransparency As Double = -1,
        '                                    Optional sHeader As String = Nothing) As ILayer

        '        If String.IsNullOrEmpty(sDisplayName) Then
        '            Throw New ArgumentNullException("Display Name", "Null or empty display Name")
        '        End If

        '        ' PGB - 14 Oct 2014 - Need to gracefully handle when the raster doesn't exist on the
        '        ' file system. Pretty fundamental that this is handled here and also by calling methods
        '        If Not GCDConsoleLib.Raster.Exists(sSource) Then
        '            Return Nothing
        '        End If
        '        '
        '        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '        ' Only add if it doesn't exist already while respecting GCD Options
        '        '

        '        Dim pResultLayer As ILayer = IsRasterLayerInGroupLayer(sSource, pGrpLyr)

        '        'The Add layers if already present option has been removed, but code is safe to remain
        '        If My.Settings.AddMapLayersIfAlreadyPresent = False Then
        '            If TypeOf pResultLayer Is ILayer Then
        '                Return pResultLayer
        '            End If
        '        End If

        '        Dim pMXDoc As ESRI.ArcGIS.ArcMapUI.IMxDocument = m_pArcMap.Document
        '        Dim pMap As ESRI.ArcGIS.Carto.IMap = pMXDoc.FocusMap
        '        'Dim pMap As ESRI.ArcGIS.Carto.IMap = Me.MapDocument.ActiveView.FocusMap
        '        Dim gxLayer = New ESRI.ArcGIS.Catalog.GxLayerClass()
        '        Dim gxFile As ESRI.ArcGIS.Catalog.IGxFile = gxLayer
        '        gxFile.Path = sSymbologyLayerFile
        '        pResultLayer = TryCast(gxLayer.Layer, IRasterLayer)

        '        '
        '        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '        ' RASTER
        '        '
        '        'TODO: Potentially will have to change this if problems calculating statistics
        '        Dim gRaster As New GCDConsoleLib.Raster(sSource)
        '        gRaster.CalculateStatistics()
        '        If String.IsNullOrEmpty(sSymbologyLayerFile) Then
        '            pResultLayer = gRaster.RasterLayer
        '        Else
        '            If IO.File.Exists(sSymbologyLayerFile) Then
        '                pResultLayer = gRaster.ApplySymbology(sSymbologyLayerFile)
        '            Else
        '                Throw New ArgumentOutOfRangeException("Symbology Layer File", sSymbologyLayerFile, "Symbology layer file does not exist")
        '            End If
        '        End If
        '        '
        '        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '        ' Apply transparency
        '        If fTransparency >= 0 Then
        '            Dim pLayerEffects As ILayerEffects = pResultLayer
        '            pLayerEffects.Transparency = fTransparency
        '        End If
        '        '

        '        'Handle an optional arguement here to give the raster values their headers , i.e. units above the symbology
        '        If Not String.IsNullOrEmpty(sHeader) Then
        '            Dim pLegend As ILegendInfo = CType(pResultLayer, ILegendInfo)
        '            pLegend.LegendGroup(0).Heading = sHeader
        '        End If

        '        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '        ' Finally, add the layer to the map
        '        '
        '        Dim pMapLayers As IMapLayers = pMap



        '        If Not String.IsNullOrEmpty(sDisplayName) Then
        '            pResultLayer.Name = sDisplayName
        '        End If

        '        If pGrpLyr Is Nothing Then
        '            pMapLayers.InsertLayer(pResultLayer, True, 0)
        '        Else
        '            pMapLayers.InsertLayerInGroup(pGrpLyr, pResultLayer, True, 0)
        '        End If


        '        'Release lock on raster layer - FP Jan 10 2012
        '        If pMapLayers IsNot Nothing Then
        '            System.Runtime.InteropServices.Marshal.ReleaseComObject(pMapLayers)
        '            pMapLayers = Nothing
        '            pResultLayer = Nothing
        '        End If


        '        'Commented out methods below are not valid on IMapDocuments, due to lack of control of TOC
        '        'Refresh updates "TOC" of IMapDocument, Me.MXDoc refers to old methods on IMxDocument
        '        'Me.MapDocument.ActiveView.Refresh()

        '        pMXDoc.UpdateContents()
        '        pMXDoc.ActiveView.Refresh()
        '        pMXDoc.CurrentContentsView.Refresh(Nothing)

        '        Return pResultLayer

        '    End Function

        '    ''' <summary>
        '    ''' Add a feature class to its associated group layer in the current ArcMap document.
        '    ''' </summary>
        '    ''' <param name="sSource">Full path of the GIS data source</param>
        '    ''' <param name="sDisplayName">Name to be used into the Table of Contents</param>
        '    ''' <param name="pGrpLyr">Parent Group Layer under which to create this raster</param>
        '    ''' <returns>pointer to the layer added to the map - or the layer that was already in the map</returns>
        '    ''' <remarks></remarks>
        '    Private Function AddToMapVector(sSource As String, sDisplayName As String, pGrpLyr As IGroupLayer, Optional fTransparency As Double = -1) As ILayer

        '        If String.IsNullOrEmpty(sDisplayName) Then
        '            Throw New ArgumentNullException("Display Name", "Null or empty display Name")
        '        End If
        '        '
        '        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '        ' Only add if it doesn't exist already while respecting GCD Options
        '        '
        '        Dim pResultLayer As IFeatureLayer = IsFeatureLayerInGroupLayer(sSource, pGrpLyr)

        '        'The Add layers if already present option has been removed, but code is safe to remain
        '        If My.Settings.AddMapLayersIfAlreadyPresent = False Then
        '            If TypeOf pResultLayer Is ILayer Then
        '                Return pResultLayer
        '            End If
        '        End If

        '        Dim pMXDoc As ESRI.ArcGIS.ArcMapUI.IMxDocument = m_pArcMap.Document
        '        Dim pMap As ESRI.ArcGIS.Carto.IMap = pMXDoc.FocusMap
        '        'Dim pMap As ESRI.ArcGIS.Carto.IMap = Me.MapDocument.ActiveView.FocusMap
        '        'Dim gxLayer = New ESRI.ArcGIS.Catalog.GxLayerClass()
        '        'Dim gxFile As ESRI.ArcGIS.Catalog.IGxFile = gxLayer
        '        'gxFile.Path = sSymbologyLayerFile

        '        Dim pWSF As ESRI.ArcGIS.Geodatabase.IWorkspaceFactory = ArcMap.GetWorkspaceFactory(GISDataStructures.GISDataStorageTypes.ShapeFile)
        '        Dim pWS As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace = pWSF.OpenFromFile(GCDConsoleLib.VectorDataSource.GetWorkspacePath(sSource), 0)
        '        Dim pFC As ESRI.ArcGIS.Geodatabase.IFeatureClass = pWS.OpenFeatureClass(IO.Path.GetFileNameWithoutExtension(sSource))
        '        pResultLayer = New FeatureLayer
        '        pResultLayer.FeatureClass = pFC

        '        'pResultLayer = TryCast(gxLayer.Layer, IFeatureLayer)
        '        '
        '        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '        ' Apply transparency
        '        If fTransparency >= 0 Then
        '            Dim pLayerEffects As ILayerEffects = pResultLayer
        '            pLayerEffects.Transparency = fTransparency
        '        End If
        '        '
        '        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '        ' Finally, add the layer to the map
        '        '
        '        Dim pMapLayers As IMapLayers = pMap

        '        If Not String.IsNullOrEmpty(sDisplayName) Then
        '            pResultLayer.Name = sDisplayName
        '        End If

        '        If pGrpLyr Is Nothing Then
        '            pMapLayers.InsertLayer(pResultLayer, True, 0)
        '        Else
        '            pMapLayers.InsertLayerInGroup(pGrpLyr, pResultLayer, True, 0)
        '        End If

        '        'Release lock on raster layer - FP Jan 10 2012
        '        If pMapLayers IsNot Nothing Then
        '            System.Runtime.InteropServices.Marshal.ReleaseComObject(pMapLayers)
        '            pMapLayers = Nothing
        '            pResultLayer = Nothing
        '        End If

        '        'Commented out methods below are not valid on IMapDocuments, due to lack of control of TOC
        '        'Refresh updates "TOC" of IMapDocument, Me.MXDoc refers to old methods on IMxDocument
        '        ' Me.MapDocument.ActiveView.Refresh()

        '        pMXDoc.UpdateContents()
        '        pMXDoc.ActiveView.Refresh()
        '        pMXDoc.CurrentContentsView.Refresh(Nothing)


        '        Return pResultLayer

        '    End Function

        '    ''' <summary>
        '    ''' Checks if a layer is in a group layer, if it is then it returns a reference to that layer, else it returns nothing
        '    ''' </summary>
        '    ''' <param name="sSource">Raster Path</param>
        '    ''' <param name="pGrpLyr">Group Layer Interface</param>
        '    ''' <returns>raster layer or nothing</returns>
        '    ''' <remarks></remarks>
        '    Private Function IsRasterLayerInGroupLayer(ByVal sSource As String, ByVal pGrpLyr As IGroupLayer) As IRasterLayer

        '        Dim pMXDoc As ESRI.ArcGIS.ArcMapUI.IMxDocument = m_pArcMap.Document
        '        Dim pMap As ESRI.ArcGIS.Carto.IMap = pMXDoc.FocusMap
        '        'Dim pMap As ESRI.ArcGIS.Carto.IMap = Me.MapDocument.ActiveView.FocusMap
        '        'Create composite layer so that sub layers can be looped over
        '        Dim compositeLayer As ICompositeLayer = TryCast(pGrpLyr, ICompositeLayer)
        '        If compositeLayer IsNot Nothing And compositeLayer.Count > 0 Then
        '            For i As Integer = 0 To compositeLayer.Count - 1
        '                If TypeOf compositeLayer.Layer(i) Is IRasterLayer Then
        '                    Dim pLayer As IRasterLayer = compositeLayer.Layer(i)
        '                    If String.Compare(pLayer.FilePath, sSource, True) = 0 Then
        '                        Return pLayer
        '                    End If
        '                End If
        '            Next
        '        End If

        '        Return Nothing

        '    End Function


        '    ''' <summary>
        '    ''' Checks if a layer is in a group layer, if it is then it returns a reference to that layer, else it returns nothing
        '    ''' </summary>
        '    ''' <param name="sSource">Feature Class Path</param>
        '    ''' <param name="pGrpLyr">Group Layer Interface</param>
        '    ''' <returns>feature layer or nothing</returns>
        '    ''' <remarks></remarks>
        '    Private Function IsFeatureLayerInGroupLayer(ByVal sSource As String, ByVal pGrpLyr As IGroupLayer) As IFeatureLayer

        '        Dim pMXDoc As ESRI.ArcGIS.ArcMapUI.IMxDocument = m_pArcMap.Document
        '        Dim pMap As ESRI.ArcGIS.Carto.IMap = pMXDoc.FocusMap
        '        'Dim pMap As ESRI.ArcGIS.Carto.IMap = Me.MapDocument.ActiveView.FocusMap
        '        'Create composite layer so that sub layers can be looped over
        '        Dim compositeLayer As ICompositeLayer = TryCast(pGrpLyr, ICompositeLayer)
        '        If compositeLayer IsNot Nothing And compositeLayer.Count > 0 Then
        '            For i As Integer = 0 To compositeLayer.Count - 1
        '                If TypeOf compositeLayer.Layer(i) Is IFeatureLayer Then

        '                    'IFeatureLayer does not have a filepath property must check source by casting to IDataset and combining the properties of IDataset to create source path
        '                    Dim pLayer As ESRI.ArcGIS.Geodatabase.IDataset = compositeLayer.Layer(i)
        '                    Dim pLayerPath As String = IO.Path.Combine(pLayer.Workspace.PathName, pLayer.BrowseName & ".shp")
        '                    If String.Compare(pLayerPath, sSource, True) = 0 Then
        '                        'MsgBox(System.IO.Path.GetFileName(sSource) & " is already in the map view and loaded in the table of contents.", vbOKOnly, "Raster Layer Already Loaded.")
        '                        Return pLayer
        '                    End If

        '                End If
        '            Next
        '        End If

        '        Return Nothing

        '    End Function

        '    ''' <summary>
        '    ''' Checks if a layer is in a group layer, if it is then it returns a reference to that layer, else it returns nothing
        '    ''' </summary>
        '    ''' <param name="sSource">Layer Path</param>
        '    ''' <param name="pGrpLyr">Group Layer Interface</param>
        '    ''' <returns>ILayer or nothing</returns>
        '    ''' <remarks></remarks>
        '    Public Function IsLayerInGroupLayer(ByVal sSource As String, ByVal pGrpLyr As IGroupLayer) As ILayer

        '        Dim pLayer As ILayer = Nothing

        '        'Create composite layer so that sub layers can be looped over
        '        Dim compositeLayer As ICompositeLayer = TryCast(pGrpLyr, ICompositeLayer)
        '        If compositeLayer IsNot Nothing And compositeLayer.Count > 0 Then
        '            For i As Integer = 0 To compositeLayer.Count - 1
        '                If TypeOf compositeLayer.Layer(i) Is IRasterLayer Then
        '                    pLayer = IsRasterLayerInGroupLayer(sSource, pGrpLyr)
        '                ElseIf TypeOf compositeLayer.Layer(i) Is FeatureLayer Then
        '                    pLayer = IsFeatureLayerInGroupLayer(sSource, pGrpLyr)
        '                End If
        '            Next
        '        End If

        '        If pLayer IsNot Nothing Then
        '            Return CType(pLayer, ESRI.ArcGIS.Carto.ILayer)
        '        End If
        '        Return Nothing
        '    End Function

        '    Private Sub RemoveDoDPreviewFromTOC(ByVal pProjectRow As ProjectDS.ProjectRow)

        '        Dim pMXDoc As ESRI.ArcGIS.ArcMapUI.IMxDocument = m_pArcMap.Document
        '        Dim pMap As ESRI.ArcGIS.Carto.IMap = pMXDoc.FocusMap
        '        'Dim pMap As ESRI.ArcGIS.Carto.IMap = Me.MapDocument.ActiveView.FocusMap

        '        If pMap.LayerCount < 1 Then
        '            Exit Sub
        '        End If


        '        Dim pSurveyLayer As ICompositeLayer = ArcMap.GetLayerByName(pProjectRow.Name, m_pArcMap, ArcMap.eEsriLayerType.Esri_GroupLayer)

        '        If pSurveyLayer IsNot Nothing And pSurveyLayer.Count > 0 Then
        '            For i As Integer = 0 To pSurveyLayer.Count - 1
        '                If String.Compare(pSurveyLayer.Layer(i).Name, "Analyses", True) = 0 Then
        '                    Dim pAnalysesLayer As ICompositeLayer = TryCast(pSurveyLayer.Layer(i), ICompositeLayer)
        '                    If pAnalysesLayer IsNot Nothing And pAnalysesLayer.Count > 0 Then
        '                        For j As Integer = 0 To pAnalysesLayer.Count - 1
        '                            If TypeOf pAnalysesLayer.Layer(j) Is IRasterLayer Then
        '                                Dim pLayer As IRasterLayer = pAnalysesLayer.Layer(j)
        '                                If pLayer.Name.Contains("preview") Then
        '                                    pMap.DeleteLayer(pLayer)
        '                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pLayer)
        '                                    Exit Sub
        '                                End If
        '                            End If
        '                        Next
        '                    End If
        '                End If
        '            Next
        '        End If

        '    End Sub

        '    ''' <summary>
        '    ''' Removes an entire project from the TOC
        '    ''' </summary>
        '    ''' <remarks>If user changes the name of the project layer this will not work</remarks>
        '    Public Shared Sub RemoveProjectFromTOC()

        '        Dim projectName As String = GCD.GCDProject.ProjectManagerBase.CurrentProject.Name
        '        Dim pMXDoc As ESRI.ArcGIS.ArcMapUI.IMxDocument = DirectCast(My.ThisApplication, ESRI.ArcGIS.Framework.IApplication).Document
        '        Dim pMap As ESRI.ArcGIS.Carto.IMap = pMXDoc.FocusMap

        '        Dim enumLayer As ESRI.ArcGIS.Carto.IEnumLayer = pMap.Layers
        '        Dim pLayer As ESRI.ArcGIS.Carto.ILayer = enumLayer.Next()

        '        While pLayer IsNot Nothing
        '            If String.Compare(projectName, pLayer.Name, True) = 0 Then
        '                If TypeOf pLayer Is IRasterLayer Or TypeOf pLayer Is IFeatureLayer Then
        '                    pLayer = enumLayer.Next()
        '                    Continue While
        '                End If
        '                pMap.DeleteLayer(pLayer)
        '                System.Runtime.InteropServices.Marshal.ReleaseComObject(pLayer)
        '                pLayer = Nothing
        '                Exit While
        '            End If
        '            pLayer = enumLayer.Next()
        '        End While
        '    End Sub

        '    Private Function AddRasterLayer(ByVal pMXDoc As ESRI.ArcGIS.ArcMapUI.IMxDocument,
        '                                    ByVal gRaster As GCDConsoleLib.Raster,
        '                                    ByVal rasterRenderer As ESRI.ArcGIS.Carto.IRasterRenderer,
        '                                    sRasterName As String, pGrpLyr As IGroupLayer,
        '                                    Optional ByVal sHeader As String = Nothing,
        '                                    Optional fTransparency As Double = -1) As ILayer

        '        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '        ' Only add if it doesn't exist already while respecting GCD Options
        '        '
        '        'Check if layer is already in map, if it is don't add it
        '        Dim pResultLayer As ILayer = IsRasterLayerInGroupLayer(gRaster.FullPath, pGrpLyr)
        '        If TypeOf pResultLayer Is ILayer Then
        '            Return pResultLayer
        '        End If

        '        Dim pMap As ESRI.ArcGIS.Carto.IMap = pMXDoc.FocusMap
        '        'Dim gxLayer = New ESRI.ArcGIS.Catalog.GxLayerClass()
        '        'Dim gxFile As ESRI.ArcGIS.Catalog.IGxFile = gxLayer

        '        'Create a raster layer from a raster dataset. You can also create a raster layer from a raster.
        '        Dim rasterLayer As ESRI.ArcGIS.Carto.IRasterLayer = New RasterLayerClass()
        '        rasterLayer.CreateFromDataset(gRaster.RasterDataset)

        '        'Set the raster renderer. The default renderer will be used if passing a null value.
        '        If Not rasterRenderer Is Nothing Then
        '            rasterLayer.Renderer = rasterRenderer
        '        End If
        '        'Add it to a map if the layer is valid.
        '        If Not rasterLayer Is Nothing Then

        '            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '            ' Finally, add the layer to the map
        '            '
        '            Dim pMapLayers As IMapLayers = pMap

        '            If Not String.IsNullOrEmpty(sRasterName) Then
        '                rasterLayer.Name = sRasterName
        '            End If

        '            'Handle an optional arguement here to give the raster values their headers , i.e. units above the symbology
        '            If Not String.IsNullOrEmpty(sHeader) Then
        '                Dim pLegend As ILegendInfo = CType(rasterLayer, ILegendInfo)
        '                pLegend.LegendGroup(0).Heading = sHeader
        '            End If

        '            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '            ' Apply transparency
        '            If fTransparency >= 0 Then
        '                Dim pLayerEffects As ILayerEffects = rasterLayer
        '                pLayerEffects.Transparency = fTransparency
        '            End If

        '            If pGrpLyr Is Nothing Then
        '                pMapLayers.InsertLayer(rasterLayer, True, 0)
        '            Else
        '                'TODO: check to see if raster layer is already in group layer
        '                pMapLayers.InsertLayerInGroup(pGrpLyr, rasterLayer, True, 0)
        '            End If

        '            'pMap.AddLayer(CType(rasterLayer, ILayer))
        '        End If

        '        pMXDoc.UpdateContents()
        '        pMXDoc.ActiveView.Refresh()
        '        pMXDoc.CurrentContentsView.Refresh(Nothing)

        '        ''Release lock on raster layer - FP Jan 10 2012
        '        'If pMapLayers IsNot Nothing Then
        '        '    System.Runtime.InteropServices.Marshal.ReleaseComObject(pMapLayers)
        '        '    pMapLayers = Nothing
        '        '    pResultLayer = Nothing
        '        'End If

        '        Return CType(rasterLayer, ILayer)

        '    End Function

        '    Public Shared Sub ApplyClassifiedColorSchema(ByVal pGeoFeatureLayer As ESRI.ArcGIS.Carto.IGeoFeatureLayer)

        '        If pGeoFeatureLayer Is Nothing Then
        '            Exit Sub
        '        End If

        '        Dim pMarkerSymbol As ESRI.ArcGIS.Display.ISimpleMarkerSymbol = New ESRI.ArcGIS.Display.SimpleMarkerSymbolClass
        '        Dim pClassBreakRenderer As ESRI.ArcGIS.Carto.IClassBreaksRenderer = New ESRI.ArcGIS.Carto.ClassBreaksRenderer
        '        Dim pMAE As Single = 0.03
        '        Dim pClonedSymbol As ESRI.ArcGIS.esriSystem.IClone = Nothing
        '        'Dim pRandomColorRamp As ESRI.ArcGIS.Display.IRandomColorRamp = New ESRI.ArcGIS.Display.RandomColorRampClass
        '        Dim pColorRamp As ESRI.ArcGIS.Display.IColorRamp = New ESRI.ArcGIS.Display.MultiPartColorRamp
        '        Dim pColor As ESRI.ArcGIS.Display.IColor = New ESRI.ArcGIS.Display.RgbColor
        '        Dim pHsvColor As ESRI.ArcGIS.Display.IHsvColor = Nothing
        '        Dim pNewSymbol As ESRI.ArcGIS.Display.ISymbol = Nothing

        '        'Define properties of symbol
        '        pMarkerSymbol.Style = ESRI.ArcGIS.Display.esriSimpleMarkerStyle.esriSMSCircle
        '        pMarkerSymbol.Size = 6
        '        pMarkerSymbol.Outline = True

        '        'Define properties of pClassBreakRenderer
        '        pClassBreakRenderer.MinimumBreak = -100
        '        pClassBreakRenderer.BreakCount = 3
        '        pClassBreakRenderer.Break(0) = pMAE
        '        pClassBreakRenderer.Break(1) = pMAE * 3
        '        'pClassBreakRenderer.Break(2) = 100
        '        pClassBreakRenderer.Field = "VDE"
        '        pColor.RGB = 52480
        '        pMarkerSymbol.Color = pColor
        '        pClassBreakRenderer.Symbol(0) = pMarkerSymbol
        '        pClassBreakRenderer.Label(0) = "Within acceptable error (" & pMAE.ToString & " cm)"
        '        pColor.RGB = 25
        '        pMarkerSymbol.Color = pColor
        '        pClassBreakRenderer.Symbol(1) = pMarkerSymbol
        '        pClassBreakRenderer.Label(1) = "Above acceptable error - tier 1 (" & pMAE.ToString & " to " & (pMAE * 3).ToString & "cm)"
        '        pColor.RGB = 100
        '        pMarkerSymbol.Color = pColor
        '        pClassBreakRenderer.Symbol(2) = pMarkerSymbol
        '        pClassBreakRenderer.Label(2) = "Above acceptabe error - tier 2 ( > " & (pMAE * 3).ToString & "cm)"



        '        'pColor = DirectCast(pColor, ESRI.ArcGIS.Display.IColor)

        '        'Set color ramp
        '        'pRandomColorRamp.Size = 3
        '        'pColorRamp.Size = 3
        '        'Dim bOK As Boolean = True
        '        'pRandomColorRamp.CreateRamp(bOK)
        '        ''pColorRamp.CreateRamp(bOK)
        '        'pColorRamp.Color(0).RGB = 52480
        '        'pColorRamp.Color(1).RGB = 16776960
        '        'pColorRamp.Color(2).RGB = 16711680
        '        pGeoFeatureLayer.Renderer = pClassBreakRenderer

        '        'set pGeoFeatureLayer renderer to the created Class Break Renderer
        '        pGeoFeatureLayer.Renderer = pClassBreakRenderer
        '        'Dim pEnumColors As ESRI.ArcGIS.Display.IEnumColors = pColorRamp.Colors
        '        'pEnumColors.Reset()

        '        'For lngCount = 0 To pClassBreakRenderer.BreakCount - 1

        '        '    'For each value in pClassBreakRenderer, clone the existing symbol (so all properties are preserved and set its color to new color)
        '        '    pClonedSymbol = CloneMe(pClassBreakRenderer.Symbol(lngCount))

        '        '    'Now pClonedSymbol holds a copy of existing symbol change the assigned color. Set the new symbol onto the symbol array of the renderer
        '        '    pHsvColor = pEnumColors.Next()
        '        '    pNewSymbol = SetColorRamp(pClonedSymbol, pHsvColor)
        '        '    If Not pNewSymbol Is Nothing Then
        '        '        pClassBreakRenderer.Symbol(lngCount) = pNewSymbol
        '        '    End If

        '        'Next

        '        'Refresh the table of contents and the changed layer
        '        Dim activeView As ESRI.ArcGIS.Carto.IActiveView = My.ArcMap.Document.FocusMap
        '        activeView.ContentsChanged()
        '        My.ArcMap.Document.UpdateContents()
        '        activeView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeography, pGeoFeatureLayer, Nothing)



        '    End Sub

        '    'Private Sub ApplyComparitiveSymbology()
        '    '    gAssociatedRaster.CalculateStatistics()
        '    '    Dim rasterCollectionMax As Double = gAssociatedRaster.Maximum
        '    '    Dim rasterCollectionMin As Double = gAssociatedRaster.Minimum

        '    '    Dim rasterCollectionMean As Double = 0
        '    '    Dim rasterCollectionStDev As Double = 0
        '    '    Dim rasterCollectionCount As UInteger = 0

        '    '    For Each rDEM As ProjectDS.DEMSurveyRow In ProjectManager.ds.DEMSurvey.Rows

        '    '        For Each rAssoc As ProjectDS.AssociatedSurfaceRow In rDEM.GetAssociatedSurfaceRows()

        '    '            Dim tempEType As ArcMap.RasterLayerTypes = ProjectDS.GetAssociatedSurfaceType(rAssoc)

        '    '            If tempEType = eType Then

        '    '                Dim sTempRasterPath As String = GCD.GCDProject.ProjectManagerBase.GetAbsolutePath(rAssoc.Source)
        '    '                Dim gTempRaster As GCDConsoleLib.Raster = New GCDConsoleLib.Raster(sTempRasterPath)

        '    '                If gTempRaster.Maximum > rasterCollectionMax Then
        '    '                    rasterCollectionMax = gTempRaster.Maximum
        '    '                End If

        '    '                If gTempRaster.Minimum < rasterCollectionMin Then
        '    '                    rasterCollectionMin = gTempRaster.Minimum
        '    '                End If

        '    '                rasterCollectionMean += gTempRaster.Mean
        '    '                rasterCollectionStDev += gTempRaster.StandardDeviation
        '    '                rasterCollectionCount += 1

        '    '                Debug.Print("Min : {0}", rasterCollectionMin)
        '    '                Debug.Print("Max : {0}", rasterCollectionMax)
        '    '                Debug.Print("Mean : {0}", rasterCollectionMean)
        '    '                Debug.Print("Stdev : {0}", rasterCollectionStDev)
        '    '                Debug.Print("Count : {0}", rasterCollectionCount)
        '    '            End If

        '    '        Next

        '    '    Next

        '    '    rasterCollectionMean = rasterCollectionMean / rasterCollectionCount
        '    '    rasterCollectionStDev = rasterCollectionStDev / rasterCollectionCount

        '    '    Debug.Print("Min : {0}", rasterCollectionMin)
        '    '    Debug.Print("Max : {0}", rasterCollectionMax)
        '    '    Debug.Print("Mean : {0}", rasterCollectionMean)
        '    '    Debug.Print("Stdev : {0}", rasterCollectionStDev)
        '    '    Debug.Print("Count : {0}", rasterCollectionCount)

        '    '    If rasterCollectionCount > 1 Then
        '    '        Dim rasterRenderer As IRasterRenderer = CreateESRIDefinedContinuousRenderer(gAssociatedRaster, 0, "Slope", rasterCollectionMin, rasterCollectionMax, rasterCollectionMean, rasterCollectionStDev, False)
        '    '        Dim pAssocLyr As ILayer = AddRasterLayer(m_pArcMap.Document, gAssociatedRaster, rasterRenderer, assocRow.Name, pAssocGrpLyr)
        '    '        Return pAssocLyr
        '    '    Else
        '    '        Dim rasterRenderer As IRasterRenderer = CreateESRIDefinedContinuousRenderer(gAssociatedRaster, 0, "Slope")
        '    '        Dim pAssocLyr As ILayer = AddRasterLayer(m_pArcMap.Document, gAssociatedRaster, rasterRenderer, assocRow.Name, pAssocGrpLyr)
        '    '        Return pAssocLyr
        '    '    End If
        '    'End Sub


        '    Private Function GetLayerHeader(assocRow As ProjectDS.AssociatedSurfaceRow) As String
        '        Dim sHeader = Nothing
        '        Dim eType As ArcMap.RasterLayerTypes = ProjectDS.GetAssociatedSurfaceType(assocRow)

        '        Select Case eType

        '            Case ArcMap.RasterLayerTypes.GrainSizeStatistic
        '                sHeader = "D50 Size Category (mm)"

        '            Case ArcMap.RasterLayerTypes.InterpolationError
        '                sHeader = "Uncertainty (" & assocRow.DEMSurveyRow.ProjectRow.DisplayUnits & ")"

        '            Case ArcMap.RasterLayerTypes.PointDensity
        '                sHeader = "pts/" & assocRow.DEMSurveyRow.ProjectRow.DisplayUnits & "^2"

        '            Case ArcMap.RasterLayerTypes.PointQuality
        '                sHeader = "Uncertainty (" & assocRow.DEMSurveyRow.ProjectRow.DisplayUnits & ")"

        '            Case ArcMap.RasterLayerTypes.Roughness
        '                sHeader = assocRow.DEMSurveyRow.ProjectRow.DisplayUnits

        '            Case ArcMap.RasterLayerTypes.SlopeDegrees
        '                sHeader = "Slope (degrees)"
        '            Case ArcMap.RasterLayerTypes.SlopePercentRise
        '                sHeader = "Slope (pecent rise)"

        '            Case ArcMap.RasterLayerTypes.Undefined

        '            Case Else
        '                Throw New Exception("An unidentified case was used.")

        '        End Select

        '        Return sHeader

        '    End Function

        '    Private Function GetLayerHeader(dodRow As ProjectDS.DoDsRow) As String
        '        Dim sHeader = Nothing
        '        'Dim eType As ArcMap.RasterLayerTypes = ProjectDS.GetAssociatedSurfaceType(dodRow)

        '        'Select Case eType


        '        '    Case Else
        '        '        Throw New Exception("An unidentified case was used.")

        '        'End Select

        '        Return sHeader
        '    End Function

        '    Public Function AddBSMaskVector(ByVal gPolygon As GISDataStructures.PolygonDataSource, rBS As ProjectDS.BudgetSegregationsRow) As ILayer

        '        Dim pAnalGrpLayer As IGroupLayer = AddAnalysesGroupLayer(rBS.DoDsRow.ProjectRow)
        '        Dim pFL As ILayer = AddToMapVector(gPolygon.FullPath, rBS.Name & " (Mask)", pAnalGrpLayer)
        '        Return pFL

        '    End Function

        'End Class

    End Class

End Namespace