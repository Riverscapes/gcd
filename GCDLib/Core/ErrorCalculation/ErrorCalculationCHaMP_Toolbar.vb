Namespace GISCode.GCD.ErrorCalculation

    Public Class ErrorCalculationCHaMP_Toolbar

        Private m_ErrorGenerator As ErrorCalculationCHaMP2
        Private m_fiChannelUnitCSV As System.IO.FileInfo

        Public Sub RunSlope(ByRef pArcMap As ESRI.ArcGIS.Framework.IApplication, ByRef pTopoData As GISCode.CHaMP.Toolbar.TopoDataToolbar)
            Run(pArcMap, pTopoData, True, False, False, False, False, False, "", Nothing)
        End Sub

        Public Sub RunPointDensity(ByRef pArcMap As ESRI.ArcGIS.Framework.IApplication, ByRef pTopoData As GISCode.CHaMP.Toolbar.TopoDataToolbar)
            Run(pArcMap, pTopoData, False, True, False, False, False, False, "", Nothing)
        End Sub

        Public Sub RunInterpError(ByRef pArcMap As ESRI.ArcGIS.Framework.IApplication, ByRef pTopoData As GISCode.CHaMP.Toolbar.TopoDataToolbar, diTINPath As System.IO.DirectoryInfo)
            Dim gTopoTIN As New GISDataStructures.TINDataSource(diTINPath)
            Run(pArcMap, pTopoData, False, False, True, False, False, False, "", gTopoTIN)
        End Sub

        Public Sub RunRoughness(ByRef pArcMap As ESRI.ArcGIS.Framework.IApplication, ByRef pTopoData As GISCode.CHaMP.Toolbar.TopoDataToolbar, sChannelUnitCSV As String)
            Run(pArcMap, pTopoData, False, False, False, True, False, False, sChannelUnitCSV, Nothing)
        End Sub

        Public Sub Run3DPointQuality(ByRef pArcMap As ESRI.ArcGIS.Framework.IApplication, ByRef pTopoData As GISCode.CHaMP.Toolbar.TopoDataToolbar)
            Run(pArcMap, pTopoData, False, False, False, False, True, False, "", Nothing)
        End Sub

        Public Sub RunErrorSurface(ByRef pArcMap As ESRI.ArcGIS.Framework.IApplication, ByRef pTopoData As GISCode.CHaMP.Toolbar.TopoDataToolbar)
            Run(pArcMap, pTopoData, False, False, False, False, False, True, "", Nothing)
        End Sub

        Private Sub Run(pArcMap As ESRI.ArcGIS.Framework.IApplication, ByRef pTopoData As GISCode.CHaMP.Toolbar.TopoDataToolbar, bSlope As Boolean, bPointDensity As Boolean, bInterpError As Boolean, bRoughness As Boolean, b3DPointquality As Boolean, bErrorRaster As Boolean, sChannelUnitCSV As String, ByRef gTopoTIN As GISDataStructures.TINDataSource)

            If Not ValidateTopoData(pTopoData) Then
                Exit Sub
            End If

            If bRoughness Then
                If Not String.IsNullOrEmpty(sChannelUnitCSV) AndAlso System.IO.File.Exists(sChannelUnitCSV) Then
                    m_fiChannelUnitCSV = New System.IO.FileInfo(sChannelUnitCSV)
                Else
                    System.Windows.Forms.MessageBox.Show("The specified channel unit CSV file does not exist.", My.Resources.ApplicationNameLong, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Information)
                    Exit Sub
                End If
            End If

            GISCode.ArcMap.SetStatus(pArcMap, "Preparing to generate associated and error surfaces...")
            Dim sToolName As String = "Derive Associated and Error Surfaces"

            ' Build the Path to the FIS Rule Files
            Dim sFISRuleFolder As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)
            sFISRuleFolder = System.IO.Path.Combine(sFISRuleFolder, "FISRuleFiles")

            ' Tool bar never reuses rasters. Always regenerated.
            Windows.Forms.Cursor.Current = Windows.Forms.Cursors.WaitCursor
            m_ErrorGenerator = New ErrorCalculationCHaMP2(pTopoData.DEM, False, New System.IO.DirectoryInfo(sFISRuleFolder))

            If bSlope Then
                Windows.Forms.Cursor.Current = Windows.Forms.Cursors.WaitCursor
                GISCode.ArcMap.SetStatus(pArcMap, "Generating slope raster")
                m_ErrorGenerator.GenerateSlope(System.IO.Path.Combine(pTopoData.FileGDB.FullName, GISCode.CHaMP.TopoData.m_sLayerSlopeRaster), True)
            End If

            If bPointDensity Then
                Windows.Forms.Cursor.Current = Windows.Forms.Cursors.WaitCursor
                GISCode.ArcMap.SetStatus(pArcMap, "Generating point density raster")
                m_ErrorGenerator.GeneratePointDensity(System.IO.Path.Combine(pTopoData.FileGDB.FullName, GISCode.CHaMP.TopoData.m_sLayerPointDensity), pTopoData.TopoPoints, pTopoData.EdgeOfWaterPoints)
            End If

            If bInterpError Then
                Windows.Forms.Cursor.Current = Windows.Forms.Cursors.WaitCursor
                GISCode.ArcMap.SetStatus(pArcMap, "Generating interpolation error raster")
                m_ErrorGenerator.GenerateInterpolationError(System.IO.Path.Combine(pTopoData.FileGDB.FullName, GISCode.CHaMP.TopoData.m_sLayerInterpError), gTopoTIN, pTopoData.SurveyExtent, True)
            End If

            If b3DPointquality Then
                If TypeOf pTopoData.TopoPoints Is GISDataStructures.PointDataSource AndAlso pTopoData.TopoPoints.HasPointQualityField Then

                    ' 3D Point Quality information available in Topo Points.
                    Windows.Forms.Cursor.Current = Windows.Forms.Cursors.WaitCursor
                    GISCode.ArcMap.SetStatus(pArcMap, "Generating 3D point quality raster")
                    m_ErrorGenerator.Generate3DPointQuality(System.IO.Path.Combine(pTopoData.FileGDB.FullName, GISCode.CHaMP.TopoData.m_sLayer3DPointQuality), pTopoData.TopoPoints, pTopoData.SurveyExtent)
                End If
            End If

            If bRoughness Then
                If TypeOf pTopoData.ChannelUnits Is GISDataStructures.ChannelUnits Then
                    Windows.Forms.Cursor.Current = Windows.Forms.Cursors.WaitCursor

                    ' Channel unit information needed for in-channel roughness
                    Dim dGrainSize As Dictionary(Of UInteger, GISCode.CHaMP.ChannelUnit) = Nothing
                    Dim lMissingFields As List(Of String) = GISCode.CHaMP.ChannelUnitCSVFile.ReadCSVFile(m_fiChannelUnitCSV, dGrainSize, True)
                    If lMissingFields.Count <= 0 Then



                        Windows.Forms.Cursor.Current = Windows.Forms.Cursors.WaitCursor
                        GISCode.ArcMap.SetStatus(pArcMap, "Generating in and out of channel rasters.")

                        'Create a list of string to pass as reference to roughness generation function, this list will hold all error messages and write them to the SurveyGDB Log
                        Dim lMessages As List(Of String) = New List(Of String)
                        m_ErrorGenerator.GenerateRoughness(System.IO.Path.Combine(pTopoData.FileGDB.FullName, GISCode.CHaMP.TopoData.m_sLayerRoughness),
                                                           pTopoData.ChannelUnits,
                                                           dGrainSize,
                                                           pTopoData.Bankfull.WaterExtent,
                                                           pTopoData.TopoTIN,
                                                           pTopoData.SurveyExtent,
                                                           pTopoData.Wetted.WaterExtent,
                                                           lMessages)
                        'Write error messages to survey gdb log
                        For Each sErrorMessage As String In lMessages
                            Toolbox.StoreNote(CTTExtension.Site.Visit.TopoData.FileGDB.FullName, "In Channel Roughness", "Warning", sErrorMessage)
                        Next
                    ElseIf lMissingFields.Count > 0 Then
                        MsgBox(String.Format("Mandatory columns missing from file: {0}.{1}{1}Obtain a valid channelunits.csv file either from the CHaMP broker software, champmonitoring.org or the CHaMP Workbench",
                                             String.Join(", ", lMissingFields.ToArray()), vbCrLf), MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                    End If
                End If
            End If

            Windows.Forms.Cursor.Current = Windows.Forms.Cursors.WaitCursor
            Dim sErrorRasterPath As String = System.IO.Path.Combine(pTopoData.FileGDB.FullName, GISCode.CHaMP.TopoData.m_sLayerErrorRaster)
            If GISDataStructures.Raster.Exists(sErrorRasterPath) Then
                GP.DataManagement.Delete(sErrorRasterPath)
            End If

            If bErrorRaster Then
                Windows.Forms.Cursor.Current = Windows.Forms.Cursors.WaitCursor
                Dim sProcessing As String = String.Empty
                GISCode.ArcMap.SetStatus(pArcMap, "Generating FIS error surface")
                m_ErrorGenerator.LoadExistingAssociatedSurfaceRasters(pTopoData.FileGDB)
                m_ErrorGenerator.Run(sErrorRasterPath, sProcessing)
            End If

            GISCode.ArcMap.SetStatus(pArcMap, "")

        End Sub

        Private Function ValidateTopoData(ByRef pTopoData As GISCode.CHaMP.Toolbar.TopoDataToolbar) As Boolean

            If pTopoData.DEM Is Nothing Then
                System.Windows.Forms.MessageBox.Show("You must generate a DEM raster before you can generated associated or error surfaces.", My.Resources.ApplicationNameLong, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Information)
                Return False
            End If

            If pTopoData.TopoPoints Is Nothing Then
                System.Windows.Forms.MessageBox.Show("The survey geodatabase must contain a Topo Points feature class before you can generated associated or error surfaces.", My.Resources.ApplicationNameLong, Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Information)
                Return False
            End If

            Return True

        End Function

    End Class

End Namespace