
Namespace Core

    Public Class OutputManager

        Private Const m_sInputsFolder As String = "inputs"
        Private Const m_sAssociatedSurfacesFolder As String = "AssociatedSurfaces"
        Private Const m_sBudgetSegregationFolder As String = "BS"
        Private Const m_sAnalysesFolder As String = "Analyses"
        Private Const m_sChangeDetectionFolder As String = "CD"
        Private Const m_sDoDFolderPrefix As String = "GCD"
        Private Const m_sErrorCalculationsFolder As String = "ErrorSurfaces"
        Private Const m_sAOIFolder As String = "AOIs"
        Private Const m_sDEMSurveyHillshadeSuffix As String = "_HS"
        Private Const m_sDEMSurveyMethodMasks As String = "Masks"
        Private Const m_sErrorSurfaceMethodsFolder As String = "Methods"
        Private Const m_sErrorSurfaceMethodMask As String = "_Mask"
        Private Const m_sFiguresSubfolder As String = "Figs"

        Private m_sOutputDriver As String = "GTiff"
        Private m_nNoData As Integer = -9999

#Region "Properties"

        Public ReadOnly Property OutputDriver As String
            Get
                Return m_sOutputDriver
            End Get
        End Property

        Public ReadOnly Property NoData As Integer
            Get
                Return m_nNoData
            End Get
        End Property
#End Region

        Public Sub New()

        End Sub

#Region "Folder Paths"

        Public Function GCDProjectFolder() As String
            Return IO.Path.GetDirectoryName(GCDProject.ProjectManager.FilePath)
        End Function

        Public Function DEMSurveyFolder(sSurveyName As String) As String
            sSurveyName = naru.os.File.RemoveDangerousCharacters(sSurveyName)
            Return IO.Path.Combine(IO.Path.Combine(GCDProjectFolder, m_sInputsFolder), sSurveyName)
        End Function

        Public Function AssociatedSurfaceFolder(sSurveyName As String, sAssociatedSurfaceName As String) As String
            Dim sRasterPath As String = DEMSurveyFolder(sSurveyName)
            sRasterPath = IO.Path.Combine(sRasterPath, m_sAssociatedSurfacesFolder)
            sAssociatedSurfaceName = naru.os.File.RemoveDangerousCharacters(sAssociatedSurfaceName)
            sRasterPath = IO.Path.Combine(sRasterPath, sAssociatedSurfaceName)
            Return sRasterPath
        End Function

        Public Function ErrorSurfaceFolder(sSurveyName As String, sErrorSurfaceName As String) As String
            Dim sRasterPath As String = DEMSurveyFolder(sSurveyName)
            sRasterPath = IO.Path.Combine(sRasterPath, m_sErrorCalculationsFolder)
            sErrorSurfaceName = naru.os.File.RemoveDangerousCharacters(sErrorSurfaceName)
            sRasterPath = IO.Path.Combine(sRasterPath, sErrorSurfaceName)
            Return sRasterPath
        End Function

        Public Function ErrorSurfaceMethodFolder(sSurveyName As String, sErrorSurfaceName As String, sMethodName As String) As String
            Dim sRasterPath As String = ErrorSurfaceFolder(sSurveyName, sErrorSurfaceName)
            sRasterPath = IO.Path.Combine(sRasterPath, m_sErrorSurfaceMethodsFolder)
            sRasterPath = IO.Path.Combine(sRasterPath, naru.os.File.RemoveDangerousCharacters(sMethodName).Replace(" ", ""))
            Return sRasterPath
        End Function

        Public Function DEMSurveyMethodMaskFolder(sSurveyName As String) As String
            Dim sMaskPath As String = DEMSurveyFolder(sSurveyName)
            sMaskPath = IO.Path.Combine(sMaskPath, m_sDEMSurveyMethodMasks)
            Return sMaskPath
        End Function

        Public Function AOIFolder(sAOIName As String, Optional bCreateIfMissing As Boolean = False) As String
            sAOIName = naru.os.File.RemoveDangerousCharacters(sAOIName)
            Dim sFolder As String = IO.Path.Combine(GCDProjectFolder, m_sAOIFolder)
            If bCreateIfMissing Then
                IO.Directory.CreateDirectory(sFolder)
            End If
            sFolder = IO.Path.Combine(sFolder, sAOIName)
            If bCreateIfMissing Then
                IO.Directory.CreateDirectory(sFolder)
            End If
            Return sFolder

        End Function

        Public Function CreateBudgetSegFolder(ByVal sDoDName As String, Optional ByVal bCreateIfMissing As Boolean = False) As String

            'New code Hensleigh 4/24/2014
            Dim sBSFolder As String = String.Empty
            sBSFolder = GetBudgetSegreationDirectoryPath(sDoDName)

            Dim sAnalysisFolder As String = IO.Path.Combine(GCDProjectFolder, m_sAnalysesFolder)
            If bCreateIfMissing AndAlso Not IO.Directory.Exists(sAnalysisFolder) Then
                IO.Directory.CreateDirectory(sAnalysisFolder)
            End If

            Dim sChangeDetectionFolder As String = IO.Path.Combine(sAnalysisFolder, m_sChangeDetectionFolder)
            If bCreateIfMissing AndAlso Not IO.Directory.Exists(sChangeDetectionFolder) Then
                IO.Directory.CreateDirectory(sChangeDetectionFolder)
            End If

            If Not IO.Directory.Exists(sBSFolder) Then
                IO.Directory.CreateDirectory(sBSFolder)
            End If

            Return sBSFolder

        End Function

        Public Function CreateBudgetSegFolder(ByVal sDoDName As String, ByVal sPolgonMaskFilePath As String, ByVal sField As String, Optional ByVal bCreateIfMissing As Boolean = False) As String


            'New code Hensleigh 4/24/2014
            'Dim sDoDFolder As String = Me.GetDoDOutputFolder(sDoDName, bCreateIfMissing)
            'Dim sBSFolder As String = IO.Path.Combine(sDoDFolder, m_sBudgetSegregationFolder)
            Dim sDoDFolder As String = String.Empty
            Dim sBSFolder As String = String.Empty
            Dim sSafeMaskName As String = naru.os.File.RemoveDangerousCharacters(IO.Path.GetFileNameWithoutExtension(sPolgonMaskFilePath)).Replace(" ", "")
            Dim sSafeMaskField As String = naru.os.File.RemoveDangerousCharacters(sField).Replace(" ", "")

            'Dim sBudgetSegregationDirectorPath As String = IO.Path.Combine(ChangeDetectionDirectoryPath, m_sBudgetSegregationFolder)
            Dim nFolderIndex As Integer = 0

            For Each rDoD As ProjectDS.DoDsRow In Core.GCDProject.ProjectManager.CurrentProject.GetDoDsRows
                If String.Compare(sDoDName, rDoD.Name, True) = 0 Then
                    sDoDFolder = IO.Path.Combine(GCDProjectFolder(), IO.Path.GetDirectoryName(rDoD.RawDoDPath))
                    If Not IO.Directory.Exists(sDoDFolder) Then
                        Dim ex As New Exception("A DoD was found in the project but the associated folder was missing.")
                        ex.Data("DoD Name") = rDoD.Name
                        ex.Data("Folder Path") = sDoDFolder
                        Throw ex
                    End If

                    For Each rBS As ProjectDS.BudgetSegregationsRow In rDoD.GetBudgetSegregationsRows()
                        If nFolderIndex < rBS.BudgetID Then
                            nFolderIndex = rBS.BudgetID
                            Exit For
                        End If
                    Next
                    Exit For
                End If

            Next

            If String.IsNullOrEmpty(sBSFolder) Then
                'The DoD was not found in the project. So determine the DoD folder apth by the next
                'available folder index (greater than the max ID or DoD IDs in the project and existing folders

                Dim sAnalysisFolder As String = IO.Path.Combine(GCDProjectFolder, m_sAnalysesFolder)
                If bCreateIfMissing AndAlso Not IO.Directory.Exists(sAnalysisFolder) Then
                    IO.Directory.CreateDirectory(sAnalysisFolder)
                End If

                Dim sChangeDetectionFolder As String = IO.Path.Combine(sAnalysisFolder, m_sChangeDetectionFolder)
                If bCreateIfMissing AndAlso Not IO.Directory.Exists(sChangeDetectionFolder) Then
                    IO.Directory.CreateDirectory(sChangeDetectionFolder)
                End If

                'Dim sDoD_BS_Folder As String = IO.Path.Combine(sDoDFolder, m_sBudgetSegregationFolder)
                'If Not IO.Directory.Exists(sDoDFolder) Then
                '    IO.Directory.CreateDirectory(sDoD_BS_Folder)
                'End If

                'By now nFolder index should hold an integer that represents either the max BS ID or the
                'max folder integer suffix. Create the new BS folder with the next biggest integer

                Do
                    nFolderIndex += 1
                    sBSFolder = IO.Path.Combine(sDoDFolder, m_sBudgetSegregationFolder & nFolderIndex.ToString("0000"))
                Loop While IO.Directory.Exists(sBSFolder) AndAlso nFolderIndex < 9999


                If Not IO.Directory.Exists(sBSFolder) Then
                    IO.Directory.CreateDirectory(sBSFolder)
                End If


            End If

            Return sBSFolder

            ''''''''''''''''''''''''''
            'Old Code
            '
            'Dim sDoDFolder As String = Me.GetDoDOutputFolder(sDoDName, bCreateIfMissing)
            'Dim sSafeMaskName As String = naru.os.File.RemoveDangerousCharacters(IO.Path.GetFileNameWithoutExtension(sPolgonMaskFilePath)).Replace(" ", "")
            'Dim sSafeMaskField As String = naru.os.File.RemoveDangerousCharacters(sField).Replace(" ", "")

            'Dim sBSFolder As String = IO.Path.Combine(sDoDFolder, m_sBudgetSegregationFolder)
            'If bCreateIfMissing Then
            '    IO.Directory.CreateDirectory(sBSFolder)
            'End If

            'sBSFolder = IO.Path.Combine(sBSFolder, sSafeMaskName)
            'If bCreateIfMissing Then
            '    IO.Directory.CreateDirectory(sBSFolder)
            'End If

            'sBSFolder = IO.Path.Combine(sBSFolder, sSafeMaskField)
            'If bCreateIfMissing Then
            '    IO.Directory.CreateDirectory(sBSFolder)
            'End If

            'Return sBSFolder

        End Function

        Private Function GetBudgetSegregationBaseFolders(ByVal sDoDName As String, ByRef nFolderIndex As Integer)

            Dim sDoDFolder As String = String.Empty

            For Each rDoD As ProjectDS.DoDsRow In GCDProject.ProjectManagerBase.CurrentProject.GetDoDsRows
                If String.Compare(sDoDName, rDoD.Name, True) = 0 Then
                    sDoDFolder = IO.Path.Combine(GCDProjectFolder(), IO.Path.GetDirectoryName(rDoD.RawDoDPath))
                    If Not IO.Directory.Exists(sDoDFolder) Then
                        Dim ex As New Exception("A DoD was found in the project but the associated folder was missing.")
                        ex.Data("DoD Name") = rDoD.Name
                        ex.Data("Folder Path") = sDoDFolder
                        Throw ex
                    End If

                    For Each rBS As ProjectDS.BudgetSegregationsRow In rDoD.GetBudgetSegregationsRows()
                        If nFolderIndex < rBS.BudgetID Then
                            nFolderIndex = rBS.BudgetID
                            Exit For
                        End If
                    Next
                    Exit For
                End If

            Next

            Return sDoDFolder

        End Function


#End Region

#Region "Raster Paths"

        Public Function DEMSurveyRasterPath(ByVal sSurveyName As String) As String
            Return naru.os.File.GetNewSafeName(DEMSurveyFolder(sSurveyName), sSurveyName, GCDProject.ProjectManagerBase.RasterExtension).FullName
        End Function

        Public Function DEMSurveyHillShadeRasterPath(ByVal sSurveyName As String) As String
            Return naru.os.File.GetNewSafeName(DEMSurveyFolder(sSurveyName), sSurveyName & m_sDEMSurveyHillshadeSuffix, GCDProject.ProjectManagerBase.RasterExtension).FullName
        End Function

        Public Function AssociatedSurfaceRasterPath(ByVal sSurveyName As String, ByVal sAssociatedSurface As String) As String
            Return naru.os.File.GetNewSafeName(AssociatedSurfaceFolder(sSurveyName, sAssociatedSurface), sAssociatedSurface, GCDProject.ProjectManagerBase.RasterExtension).FullName
        End Function

        Public Function ErrorSurfaceRasterPath(ByVal sSurveyName As String, ByVal sErrorSurfaceName As String, Optional ByVal bCreateWorkspace As Boolean = True) As String

            Dim sSafeName As String = naru.os.File.RemoveDangerousCharacters(sErrorSurfaceName)
            Dim sWorkspace As String = ErrorSurfaceFolder(sSurveyName, sSafeName)

            If Not IO.Directory.Exists(sWorkspace) AndAlso bCreateWorkspace Then
                IO.Directory.CreateDirectory(sWorkspace)
            End If

            Return naru.os.File.GetNewSafeName(sWorkspace, sSafeName, GCDProject.ProjectManagerBase.RasterExtension).FullName

        End Function

        Public Function DEMSurveyMethodMaskPath(ByVal sSurveyName As String) As String

            Dim sMaskPath As String = DEMSurveyMethodMaskFolder(sSurveyName)
            Dim sFeatureClassName As String = naru.os.File.RemoveDangerousCharacters(sSurveyName) & m_sErrorSurfaceMethodMask

            Return naru.os.File.GetNewSafeName(sMaskPath, sFeatureClassName, "shp").FullName

        End Function

        Public Function AOIFeatureClassPath(ByVal sAOIName As String) As String
            Dim sAOISafe As String = naru.os.File.RemoveDangerousCharacters(sAOIName).Replace(" ", "")
            sAOISafe = IO.Path.Combine(AOIFolder(sAOISafe), sAOISafe)
            sAOISafe = IO.Path.ChangeExtension(sAOISafe, "shp")
            Return sAOISafe
        End Function

#End Region


        'Public Function GetErrorRasterPath(sFolder As String, ByVal sSurveyName As String, ByVal sErrorName As String) As String
        '    'structure: Inputs/Surveyname/ErrorSurfaces/ErrorSurfaceName/RasterName

        '    'input directory
        '    Dim inputsDirectoryPath As String = IO.Path.Combine(sFolder, "Inputs")
        '    If Not IO.Directory.Exists(inputsDirectoryPath) Then
        '        IO.Directory.CreateDirectory(inputsDirectoryPath)
        '    End If

        '    'Survey directory
        '    Dim sSafeSurveyName As String = naru.os.File.RemoveDangerousCharacters(sSurveyName)
        '    Dim surveyDirectoryPath As String = IO.Path.Combine(inputsDirectoryPath, sSafeSurveyName)
        '    If Not IO.Directory.Exists(surveyDirectoryPath) Then
        '        IO.Directory.CreateDirectory(surveyDirectoryPath)
        '    End If

        '    'Error Surfaces directory
        '    Dim ErrorSurfacesDirectoryPath As String = IO.Path.Combine(surveyDirectoryPath, "ErrorSurfaces")
        '    If Not IO.Directory.Exists(ErrorSurfacesDirectoryPath) Then
        '        IO.Directory.CreateDirectory(ErrorSurfacesDirectoryPath)
        '    End If

        '    'ErrorSurfaceName directory
        '    Dim sSafeErrorName As String = naru.os.File.RemoveDangerousCharacters(sErrorName)
        '    Dim ErrorNameDirectoryPath As String = IO.Path.Combine(ErrorSurfacesDirectoryPath, sSafeErrorName)
        '    If Not IO.Directory.Exists(ErrorNameDirectoryPath) Then
        '        IO.Directory.CreateDirectory(ErrorNameDirectoryPath)
        '    End If

        '    ' PGB 14 Sep 2011 - trying to avoid these characters now and use io.path.combine instread
        '    'errorCalcDirectoryPath &= IO.Path.DirectorySeparatorChar
        '    'generate error name
        '    Dim errorCalcPath As String = GISCode.GCDConsoleLib.Raster.GetNewSafeName(ErrorNameDirectoryPath, RasterType, sErrorName, 12)
        '    Return errorCalcPath
        'End Function

        'Public Function GetMethodErrorSurfarcePath(sFolder As String, ByVal sSurveyName As String, ByVal sErrorName As String, ByVal sMethod As String) As String
        '    'structure: Inputs/Surveyname/ErrorSurfaces/ErrorSurfaceName/Method/RasterName

        '    'input directory
        '    Dim inputsDirectoryPath As String = IO.Path.Combine(sFolder, "Inputs")
        '    If Not IO.Directory.Exists(inputsDirectoryPath) Then
        '        IO.Directory.CreateDirectory(inputsDirectoryPath)
        '    End If

        '    'Survey directory
        '    Dim sSafeSurveyName As String = naru.os.File.RemoveDangerousCharacters(sSurveyName)
        '    Dim surveyDirectoryPath As String = IO.Path.Combine(inputsDirectoryPath, sSafeSurveyName)
        '    If Not IO.Directory.Exists(surveyDirectoryPath) Then
        '        IO.Directory.CreateDirectory(surveyDirectoryPath)
        '    End If

        '    'Error Surfaces directory
        '    Dim ErrorSurfacesDirectoryPath As String = IO.Path.Combine(surveyDirectoryPath, "ErrorSurfaces")
        '    If Not IO.Directory.Exists(ErrorSurfacesDirectoryPath) Then
        '        IO.Directory.CreateDirectory(ErrorSurfacesDirectoryPath)
        '    End If

        '    'ErrorSurfaceName directory
        '    Dim sSafeErrorName As String = naru.os.File.RemoveDangerousCharacters(sErrorName)
        '    Dim ErrorNameDirectoryPath As String = IO.Path.Combine(ErrorSurfacesDirectoryPath, sSafeErrorName)
        '    If Not IO.Directory.Exists(ErrorNameDirectoryPath) Then
        '        IO.Directory.CreateDirectory(ErrorNameDirectoryPath)
        '    End If

        '    'MethodName directory
        '    Dim sSafeMethodName As String = naru.os.File.RemoveDangerousCharacters(sMethod)
        '    Dim sMethodNameDirectory As String = IO.Path.Combine(ErrorNameDirectoryPath, sSafeMethodName)
        '    If Not IO.Directory.Exists(sMethodNameDirectory) Then
        '        IO.Directory.CreateDirectory(sMethodNameDirectory)
        '    End If

        '    ' PGB 14 Sep 2011 - trying to avoid these characters now and use io.path.combine instread
        '    'errorCalcDirectoryPath &= IO.Path.DirectorySeparatorChar
        '    'generate error name
        '    Dim sMethodErrorSurfacePath As String = GCDConsoleLib.Raster.GetNewSafeName(sMethodNameDirectory, RasterType, sMethod, 12)
        '    Return sMethodErrorSurfacePath
        'End Function

        'Public Function GetDoDThresholdPath(sFolder As String, ByVal sDoDName As String) As String
        '    Dim safeDoDName As String = naru.os.File.RemoveDangerousCharacters(sDoDName)
        '    'get DoD output path
        '    Dim dodDirectoryPath As String = GetDoDOutputPath(sFolder, sDoDName)
        '    'generate error name
        '    Dim dodThresholdPath As String = GCDConsoleLib.Raster.GetNewSafeName(dodDirectoryPath, RasterType, safeDoDName, 12)
        '    Return dodThresholdPath
        'End Function

        'Public Function GetCsvThresholdPath(sFolder As String, ByVal sDoDName As String) As String
        '    Dim safeDoDName As String = naru.os.File.RemoveDangerousCharacters(sDoDName)
        '    'get DoD output path
        '    Dim dodDirectoryPath As String = GetDoDOutputPath(sFolder, sDoDName)
        '    'generate error name
        '    Dim csvThresholdPath As String = FileSystem.GetNewSafeFileName(dodDirectoryPath, safeDoDName, "csv")
        '    Return csvThresholdPath
        'End Function

        'Public Function GetPropagatedErrorPath(sFolder As String, sDoDName As String) As String
        '    Dim safeDoDName As String = naru.os.File.RemoveDangerousCharacters(sDoDName)
        '    'get DoD output path
        '    Dim dodDirectoryPath As String = GetDoDOutputPath(sFolder, sDoDName)
        '    'generate error name
        '    Dim dodThresholdPath As String = GCDConsoleLib.Raster.GetNewSafeName(dodDirectoryPath, RasterType, safeDoDName, 12)
        '    Return dodThresholdPath
        'End Function

        'Public Function GetCsvRawPath(sFolder As String, ByVal sDoDName As String) As String
        '    Dim safeDoDName As String = naru.os.File.RemoveDangerousCharacters(sDoDName)
        '    'get DoD output path
        '    Dim dodDirectoryPath As String = GetDoDOutputPath(sFolder, sDoDName)
        '    'generate error name
        '    Dim csvThresholdPath As String = FileSystem.GetNewSafeFileName(dodDirectoryPath, "raw", "csv")
        '    Return csvThresholdPath
        'End Function

        'Public Function GetDoDRawPath(sFolder As String, ByVal sDoDName As String) As String
        '    Dim safeDoDName As String = naru.os.File.RemoveDangerousCharacters(sDoDName)
        '    'get DoD output path
        '    Dim dodDirectoryPath As String = GetDoDOutputPath(sFolder, sDoDName)
        '    'generate error name
        '    Dim csvThresholdPath As String = GCDConsoleLib.Raster.GetNewSafeName(dodDirectoryPath, RasterType, "dod", 12)
        '    Return csvThresholdPath
        'End Function

        'Public Function GetDoDOutputPath(sFolder As String, ByVal sDoDName As String) As String

        '    'setup folder for analysis
        '    Dim analysisDirectoryPath As String = IO.Path.Combine(sFolder, m_sAnalysesFolder)
        '    If Not IO.Directory.Exists(analysisDirectoryPath) Then
        '        IO.Directory.CreateDirectory(analysisDirectoryPath)
        '    End If

        '    'setup folder for dods
        '    Dim dodsDirectoryPath As String = IO.Path.Combine(analysisDirectoryPath, m_sChangeDetectionFolder)
        '    If Not IO.Directory.Exists(dodsDirectoryPath) Then
        '        IO.Directory.CreateDirectory(dodsDirectoryPath)
        '    End If

        '    'setup folder for dods
        '    Dim safeDoDName As String = naru.os.File.RemoveDangerousCharacters(sDoDName)
        '    Dim dodDirectoryPath As String = IO.Path.Combine(dodsDirectoryPath, safeDoDName)
        '    If Not IO.Directory.Exists(dodDirectoryPath) Then
        '        IO.Directory.CreateDirectory(dodDirectoryPath)
        '    End If

        '    dodDirectoryPath &= IO.Path.DirectorySeparatorChar
        '    Return dodDirectoryPath
        'End Function




        'Public Function GetChangeDetectionDirectoryPath(ByVal NewSurveyName As String, OldSurveyName As String, ByVal sDoDName As String) As String
        '    'structure: Analyses/ChangeDetection/Dods_NewSurveyName-OldSurveyName/DoDName

        '    Dim safeDoDName As String = naru.os.File.RemoveDangerousCharacters(sDoDName)
        '    Dim sDoDFolder As String = GetDoDOutputFolder(safeDoDName)

        '    'setup folder for dods directory
        '    Dim DoDsDirectoryName As String = "DoDs_" & NewSurveyName & "-" & OldSurveyName
        '    Dim SafeDoDsDirectoryName As String = naru.os.File.RemoveDangerousCharacters(DoDsDirectoryName)
        '    Dim DoDsDirectoryPath As String = IO.Path.Combine(ChangeDetectionDirectoryPath, SafeDoDsDirectoryName)
        '    If Not IO.Directory.Exists(DoDsDirectoryPath) Then
        '        IO.Directory.CreateDirectory(DoDsDirectoryPath)
        '    End If

        '    'setup folder for dod
        '    Dim DoDDirectoryPath As String = IO.Path.Combine(DoDsDirectoryPath, safeDoDName)
        '    If Not IO.Directory.Exists(DoDDirectoryPath) Then
        '        IO.Directory.CreateDirectory(DoDDirectoryPath)
        '    End If

        '    Return DoDDirectoryPath
        'End Function

        'Public Function GetGeomorphOutputPath(sFolder As String, ByVal sDoDName As String) As String

        '    'get DoD folder 
        '    Dim dodDirectoryPath As String = GetDoDOutputPath(sFolder, sDoDName)

        '    'setup folder for Geomorph (based on GCD 4 naming convention
        '    Dim GeomorphDirectoryPath As String = IO.Path.Combine(dodDirectoryPath, "Geomorph")
        '    If Not IO.Directory.Exists(GeomorphDirectoryPath) Then
        '        IO.Directory.CreateDirectory(GeomorphDirectoryPath)
        '    End If

        '    GeomorphDirectoryPath &= IO.Path.DirectorySeparatorChar
        '    Return GeomorphDirectoryPath
        'End Function



        Public Function GetMethodMaskCopyPath(ByVal sFolder As String, ByVal sSurveyName As String, ByVal OrigMethodMaskName As String)
            'structure: Inputs/Surveyname/MethodMasks/MethodMaskName

            'input directory
            Dim inputsDirectoryPath As String = IO.Path.Combine(sFolder, "Inputs")
            If Not IO.Directory.Exists(inputsDirectoryPath) Then
                IO.Directory.CreateDirectory(inputsDirectoryPath)
            End If

            'Survey directory
            Dim sSafeSurveyName As String = naru.os.File.RemoveDangerousCharacters(sSurveyName)
            Dim surveyDirectoryPath As String = IO.Path.Combine(inputsDirectoryPath, sSafeSurveyName)
            If Not IO.Directory.Exists(surveyDirectoryPath) Then
                IO.Directory.CreateDirectory(surveyDirectoryPath)
            End If

            'MethodMask directory
            Dim MethodMaskDirectoryPath As String = IO.Path.Combine(surveyDirectoryPath, "MethodMasks")
            If Not IO.Directory.Exists(MethodMaskDirectoryPath) Then
                IO.Directory.CreateDirectory(MethodMaskDirectoryPath)
            End If

            'Method Mask path
            Dim MethodMaskCopyPath As String = IO.Path.Combine(MethodMaskDirectoryPath, OrigMethodMaskName)
            Return MethodMaskCopyPath

        End Function

        Public Function GetMethodMaskCopyPath(ByVal sSurveyName As String, ByVal OrigMethodMaskName As String)
            'structure: Inputs/Surveyname/MethodMasks/MethodMaskName

            'input directory
            Dim inputsDirectoryPath As String = IO.Path.Combine(GCDProjectFolder, "Inputs")
            If Not IO.Directory.Exists(inputsDirectoryPath) Then
                IO.Directory.CreateDirectory(inputsDirectoryPath)
            End If

            'Survey directory
            Dim sSafeSurveyName As String = naru.os.File.RemoveDangerousCharacters(sSurveyName)
            Dim surveyDirectoryPath As String = IO.Path.Combine(inputsDirectoryPath, sSafeSurveyName)
            If Not IO.Directory.Exists(surveyDirectoryPath) Then
                IO.Directory.CreateDirectory(surveyDirectoryPath)
            End If

            'MethodMask directory
            Dim MethodMaskDirectoryPath As String = IO.Path.Combine(surveyDirectoryPath, "MethodMasks")
            If Not IO.Directory.Exists(MethodMaskDirectoryPath) Then
                IO.Directory.CreateDirectory(MethodMaskDirectoryPath)
            End If

            'Method Mask path
            Dim MethodMaskCopyPath As String = IO.Path.Combine(MethodMaskDirectoryPath, OrigMethodMaskName)
            Return MethodMaskCopyPath

        End Function

        Public Function GetInputDEMCopyPath(ByVal sSurveyName As String, ByVal OrigRasterName As String)
            'structure: Inputs/Surveyname/rastername

            'input directory
            Dim inputsDirectoryPath As String = IO.Path.Combine(GCDProjectFolder, "Inputs")
            If Not IO.Directory.Exists(inputsDirectoryPath) Then
                IO.Directory.CreateDirectory(inputsDirectoryPath)
            End If

            'Survey directory
            Dim sSafeSurveyName As String = naru.os.File.RemoveDangerousCharacters(sSurveyName)
            Dim surveyDirectoryPath As String = IO.Path.Combine(inputsDirectoryPath, sSafeSurveyName)
            If Not IO.Directory.Exists(surveyDirectoryPath) Then
                IO.Directory.CreateDirectory(surveyDirectoryPath)
            End If

            'Raster path
            Dim RasterCopyPath As String = IO.Path.Combine(surveyDirectoryPath, OrigRasterName)
            Return RasterCopyPath

        End Function

        Public Function GetInputDEMCopyPath(ByVal sFolder As String, ByVal sSurveyName As String, ByVal OrigRasterName As String)
            'structure: Inputs/Surveyname/rastername

            'input directory
            Dim inputsDirectoryPath As String = IO.Path.Combine(sFolder, "Inputs")
            If Not IO.Directory.Exists(inputsDirectoryPath) Then
                IO.Directory.CreateDirectory(inputsDirectoryPath)
            End If

            'Survey directory
            Dim sSafeSurveyName As String = naru.os.File.RemoveDangerousCharacters(sSurveyName)
            Dim surveyDirectoryPath As String = IO.Path.Combine(inputsDirectoryPath, sSafeSurveyName)
            If Not IO.Directory.Exists(surveyDirectoryPath) Then
                IO.Directory.CreateDirectory(surveyDirectoryPath)
            End If

            'Raster path
            Dim RasterCopyPath As String = IO.Path.Combine(surveyDirectoryPath, OrigRasterName)
            Return RasterCopyPath

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sSurveyName"></param>
        ''' <param name="sErrorName"></param>
        ''' <param name="sMethod"></param>
        ''' <param name="bCreateWorkspace"></param>
        ''' <returns></returns>
        ''' <remarks>Inputs/Surveyname/ErrorSurfaces/ErrorSurfaceName/Method/RasterName</remarks>
        Public Function ErrorSurfarceMethodRasterPath(ByVal sSurveyName As String, ByVal sErrorName As String, ByVal sMethod As String, Optional ByVal bCreateWorkspace As Boolean = True) As String

            Dim sWorkspace As String = ErrorSurfaceMethodFolder(sSurveyName, sErrorName, sMethod)

            If Not IO.Directory.Exists(sWorkspace) AndAlso bCreateWorkspace Then
                IO.Directory.CreateDirectory(sWorkspace)
            End If

            Return naru.os.File.GetNewSafeName(sWorkspace, sMethod.Replace(" ", ""), GCDProject.ProjectManagerBase.RasterExtension).FullName

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sSurveyName"></param>
        ''' <param name="sErrorName"></param>
        ''' <param name="sMethod"></param>
        ''' <param name="bCreateWorkspace"></param>
        ''' <returns></returns>
        ''' <remarks>Inputs/Surveyname/ErrorSurfaces/ErrorSurfaceName/Method/RasterName</remarks>
        Public Function ErrorSurfarceMethodRasterMaskPath(ByVal sSurveyName As String, ByVal sErrorName As String, ByVal sMethod As String, Optional ByVal bCreateWorkspace As Boolean = True) As String

            Dim sWorkspace As String = ErrorSurfaceMethodFolder(sSurveyName, sErrorName, sMethod)
            If Not IO.Directory.Exists(sWorkspace) AndAlso bCreateWorkspace Then
                IO.Directory.CreateDirectory(sWorkspace)
            End If

            Return naru.os.File.GetNewSafeName(sWorkspace, sMethod.Replace(" ", "") & m_sErrorSurfaceMethodMask, GCDProject.ProjectManagerBase.RasterExtension).FullName

        End Function

        Public Function GetAssociatedSurfaceCopyPath(ByVal sFolder As String, ByVal sSurveyName As String, ByVal AssociatedSurfaceName As String, ByVal OrigRasterName As String) As String
            'structure: Inputs/Surveyname/AssociatedSurfaces/surfacename/rastername

            'input directory
            Dim inputsDirectoryPath As String = IO.Path.Combine(sFolder, "Inputs")
            If Not IO.Directory.Exists(inputsDirectoryPath) Then
                CreateDirectory(inputsDirectoryPath)
            End If

            'Survey directory
            Dim sSafeSurveyName As String = naru.os.File.RemoveDangerousCharacters(sSurveyName)
            Dim surveyDirectoryPath As String = IO.Path.Combine(inputsDirectoryPath, sSafeSurveyName)
            If Not IO.Directory.Exists(surveyDirectoryPath) Then
                CreateDirectory(surveyDirectoryPath)
            End If

            'Associated Surfaces directory
            Dim AssociatedSurfacesDirectoryPath As String = IO.Path.Combine(surveyDirectoryPath, "AssociatedSurfaces")
            If Not IO.Directory.Exists(AssociatedSurfacesDirectoryPath) Then
                CreateDirectory(AssociatedSurfacesDirectoryPath)
            End If

            'Associated Surface directory
            Dim SafeAssociatedSurfaceName As String = naru.os.File.RemoveDangerousCharacters(AssociatedSurfaceName)
            Dim AssociatedSurfaceNameDirectoryPath As String = IO.Path.Combine(AssociatedSurfacesDirectoryPath, SafeAssociatedSurfaceName)
            If Not IO.Directory.Exists(AssociatedSurfaceNameDirectoryPath) Then
                CreateDirectory(AssociatedSurfaceNameDirectoryPath)
            End If

            'Raster path
            Dim AssociatedSurfaceCopyPath As String = IO.Path.Combine(AssociatedSurfaceNameDirectoryPath, OrigRasterName)
            Return AssociatedSurfaceCopyPath


        End Function

        Private Sub CreateDirectory(ByVal DirectoryPath As String)
            Try
                IO.Directory.CreateDirectory(DirectoryPath)
            Catch ex As Exception
                Dim ErrorMessage As String = "Could not create directory '" & DirectoryPath & "'"
                Throw New Exception(ErrorMessage)
            End Try
        End Sub

        Public Function GetBudgetSegreationDirectoryPath(ByVal sDoDName As String) As String

            'New code Hensleigh 4/24/2014
            Dim sBSFolder As String = String.Empty
            Dim nFolderIndex As Integer = 0
            Dim sDoDFolder As String = GetBudgetSegregationBaseFolders(sDoDName, nFolderIndex)
            Dim sAnalysisFolder As String = IO.Path.Combine(GCDProjectFolder, m_sAnalysesFolder)
            Dim sChangeDetectionFolder As String = IO.Path.Combine(sAnalysisFolder, m_sChangeDetectionFolder)
            'Dim sDoD_BS_Folder As String = IO.Path.Combine(sDoDFolder, m_sBudgetSegregationFolder)

            'By now nFolder index should hold an integer that represents either the max BS ID or the
            'max folder integer suffix. Create the new BS folder with the next biggest integer

            Do
                nFolderIndex += 1
                sBSFolder = IO.Path.Combine(sDoDFolder, m_sBudgetSegregationFolder & nFolderIndex.ToString("0000"))
            Loop While IO.Directory.Exists(sBSFolder) AndAlso nFolderIndex < 9999



            Return sBSFolder

        End Function

        Public Function GetBudgetSegreationDirectoryPath(ByVal ChangeDetectionDirectoryPath As String, ByVal MaskFilename As String, ByVal Fieldname As String) As String
            'structure: Analyses/ChangeDetection/Dods_NewSurveyName-OldSurveyName/DoDName/BudgetSegregation/MaskFileName/FieldName


            ''''''''''''''''''''''''''''''''''''''
            'Old Code
            '
            'BudgetSegregation directory
            Dim BudgetSegregationDirectoryPath As String = IO.Path.Combine(ChangeDetectionDirectoryPath, m_sBudgetSegregationFolder)
            If Not IO.Directory.Exists(BudgetSegregationDirectoryPath) Then
                CreateDirectory(BudgetSegregationDirectoryPath)
            End If
            'MaskFileName directory
            Dim SafeMaskFileName As String = naru.os.File.RemoveDangerousCharacters(MaskFilename)
            Dim MaskFileNameDirectoryPath As String = IO.Path.Combine(BudgetSegregationDirectoryPath, SafeMaskFileName)
            If Not IO.Directory.Exists(MaskFileNameDirectoryPath) Then
                CreateDirectory(MaskFileNameDirectoryPath)
            End If

            'MaskFileName directory
            Dim SafeFieldname As String = naru.os.File.RemoveDangerousCharacters(Fieldname)
            Dim FieldnameDirectoryPath As String = IO.Path.Combine(MaskFileNameDirectoryPath, SafeFieldname)
            If Not IO.Directory.Exists(FieldnameDirectoryPath) Then
                CreateDirectory(FieldnameDirectoryPath)
            End If

            Return FieldnameDirectoryPath
        End Function

#Region "Change Detection"

        Public Function GetDoDThresholdPath(ByVal sDoDName As String, Optional ByVal sFolder As String = "") As String
            Dim dodDirectoryPath As String
            If String.IsNullOrEmpty(sFolder) OrElse Not IO.Directory.Exists(sFolder) Then
                dodDirectoryPath = GetDoDOutputFolder(sDoDName, True)
            Else
                dodDirectoryPath = sFolder
            End If

            Return naru.os.File.GetNewSafeName(dodDirectoryPath, "dodThresh", GCDProject.ProjectManagerBase.RasterExtension).FullName

        End Function

        Public Function GetCsvThresholdPath(ByVal sDoDName As String, Optional ByVal sFolder As String = "") As String
            Dim safeDoDName As String = "thresholded" 'naru.os.File.RemoveDangerousCharacters(sDoDName)
            Dim dodDirectoryPath As String
            If String.IsNullOrEmpty(sFolder) OrElse Not IO.Directory.Exists(sFolder) Then
                dodDirectoryPath = GetDoDOutputFolder(sDoDName, True)
            Else
                dodDirectoryPath = sFolder
            End If
            Dim csvThresholdPath As String = naru.os.File.GetNewSafeName(dodDirectoryPath, safeDoDName, "csv").FullName

            Return csvThresholdPath
        End Function

        Public Function GetPropagatedErrorPath(ByVal sDoDName As String, Optional ByVal sFolder As String = "") As String
            Dim dodDirectoryPath As String
            If String.IsNullOrEmpty(sFolder) OrElse Not IO.Directory.Exists(sFolder) Then
                dodDirectoryPath = GetDoDOutputFolder(sDoDName, True)
            Else
                dodDirectoryPath = sFolder
            End If
            Dim sPropErrRaster As String = naru.os.File.GetNewSafeName(dodDirectoryPath, "PropErr", GCDProject.ProjectManagerBase.RasterExtension).FullName
            Return sPropErrRaster
        End Function

        Public Function GetCsvRawPath(ByVal sDoDFolder As String, ByVal sDoDName As String) As String
            'Dim dodDirectoryPath As String = GetDoDOutputFolder(sDoDName)
            Dim csvThresholdPath As String = naru.os.File.GetNewSafeName(sDoDFolder, "raw", "csv").FullName
            Return csvThresholdPath
        End Function

        Public Function GetGCDSummaryXMLPath(ByVal sDoDName As String, Optional ByVal sFolder As String = "") As String

            Dim dodDirectoryPath As String
            If String.IsNullOrEmpty(sFolder) OrElse Not IO.Directory.Exists(sFolder) Then
                dodDirectoryPath = GetDoDOutputFolder(sDoDName, True)
            Else
                dodDirectoryPath = sFolder
            End If
            Dim sOutput As String = naru.os.File.GetNewSafeName(dodDirectoryPath, "Summary", "xml").FullName
            Return sOutput
        End Function

        Public Function GetDoDPNGPlot(ByVal sDoDName As String, Optional ByVal sFolder As String = "") As String
            Dim dodDirectoryPath As String
            If String.IsNullOrEmpty(sFolder) OrElse Not IO.Directory.Exists(sFolder) Then
                dodDirectoryPath = GetDoDOutputFolder(sDoDName, True)
            Else
                dodDirectoryPath = sFolder
            End If
            Dim pngThresholdPath As String = naru.os.File.GetNewSafeName(dodDirectoryPath, "raw", "png").FullName
            Return pngThresholdPath
        End Function

        Public Function GetDoDRawPath(ByVal sDoDName As String, Optional ByVal sFolder As String = "") As String
            Dim dodDirectoryPath As String
            If String.IsNullOrEmpty(sFolder) OrElse Not IO.Directory.Exists(sFolder) Then
                dodDirectoryPath = GetDoDOutputFolder(sDoDName, True)
            Else
                dodDirectoryPath = sFolder
            End If

            Dim sDoDRawPath As String = IO.Path.Combine(dodDirectoryPath, "dodraw")
            sDoDRawPath = IO.Path.ChangeExtension(sDoDRawPath, "tif")
            If IO.Directory.Exists(dodDirectoryPath) Then
                If IO.File.Exists(sDoDRawPath) Then
                    sDoDRawPath = naru.os.File.GetNewSafeName(dodDirectoryPath, IO.Path.GetFileNameWithoutExtension(sDoDRawPath), GCDProject.ProjectManagerBase.RasterExtension).FullName
                End If
            End If
            Return sDoDRawPath

        End Function

        ''' <summary>
        ''' Determine the output folder for a DoD Analysis
        ''' </summary>
        ''' <param name="sDoDName">Raw Name of the DoD. This method will ensure it is safe.</param>
        ''' <param name="bCreateIfMissing">True will create the necessary folder structure. False will just return the path without creating anything.</param>
        ''' <returns></returns>
        ''' <remarks>Note that the user interface should call this with bCreateIfMissing as False.
        ''' That way it can determine the output folder and show it to users without actually
        ''' creating the folder. Then the change detection engine code can call this method with
        ''' the value of True to ensure that the folder exists when it is time to create files in it.</remarks>
        Public Function GetDoDOutputFolder(ByVal sDoDName As String, Optional ByVal bCreateIfMissing As Boolean = False) As String

            Dim sDoDFolder As String = String.Empty
            Dim nFolderIndex As Integer = 0

            ' Check if the DoD already exists and if so then get the established folder path
            For Each rDoD As ProjectDS.DoDsRow In GCDProject.ProjectManager.CurrentProject.GetDoDsRows
                If String.Compare(sDoDName, rDoD.Name, True) = 0 Then
                    sDoDFolder = IO.Path.Combine(GCDProjectFolder, rDoD.OutputFolder)
                    If Not IO.Directory.Exists(sDoDFolder) Then
                        Dim ex As New Exception("A DoD was found in the project but the associated folder was missing.")
                        ex.Data("DoD Name") = rDoD.Name
                        ex.Data("Folder Path") = sDoDFolder
                        Throw ex
                    End If
                End If

                If nFolderIndex < rDoD.DoDID Then
                    nFolderIndex = rDoD.DoDID
                End If
            Next

            If String.IsNullOrEmpty(sDoDFolder) Then
                ' The DoD was not found in the project. So Determine the DoD folder path
                ' by the next available folder index (greater than the max ID of DoD IDs in 
                ' the project and existing folder IDs.

                Dim sAnalysesFolder As String = IO.Path.Combine(GCDProjectFolder, m_sAnalysesFolder)
                If bCreateIfMissing AndAlso Not IO.Directory.Exists(sAnalysesFolder) Then
                    IO.Directory.CreateDirectory(sAnalysesFolder)
                End If

                Dim sChangeDetectionFolder As String = IO.Path.Combine(sAnalysesFolder, m_sChangeDetectionFolder)
                If bCreateIfMissing AndAlso Not IO.Directory.Exists(sChangeDetectionFolder) Then
                    IO.Directory.CreateDirectory(sChangeDetectionFolder)
                End If

                ' By now nFolder index should hold an integer that represents either the max DoD ID or the 
                ' max folder integer suffix. Create the new DoD folder with the next biggest integer
                Do
                    nFolderIndex += 1
                    sDoDFolder = IO.Path.Combine(sChangeDetectionFolder, m_sDoDFolderPrefix & (nFolderIndex).ToString("0000"))
                Loop While IO.Directory.Exists(sDoDFolder) AndAlso nFolderIndex < 9999

                If bCreateIfMissing Then
                    IO.Directory.CreateDirectory(sDoDFolder)
                End If
            End If

            Return sDoDFolder

        End Function

        Private Function GetSafeDoDName(ByVal sOriginalName As String) As String
            Return naru.os.File.RemoveDangerousCharacters(sOriginalName)
        End Function

#End Region

        'Public Shared Function GetErrorRasterPath(ByVal sSurveyName As String, ByVal sErrorName As String) As String
        '    'structure: Inputs/Surveyname/ErrorSurfaces/ErrorSurfaceName/RasterName

        '    'input directory
        '    Dim inputsDirectoryPath As String = IO.Path.Combine(outputdirectory, "Inputs")
        '    If Not IO.Directory.Exists(inputsDirectoryPath) Then
        '        IO.Directory.CreateDirectory(inputsDirectoryPath)
        '    End If

        '    'Survey directory
        '    Dim sSafeSurveyName As String = naru.os.File.RemoveDangerousCharacters(sSurveyName)
        '    Dim surveyDirectoryPath As String = IO.Path.Combine(inputsDirectoryPath, sSafeSurveyName)
        '    If Not IO.Directory.Exists(surveyDirectoryPath) Then
        '        IO.Directory.CreateDirectory(surveyDirectoryPath)
        '    End If

        '    'Error Surfaces directory
        '    Dim ErrorSurfacesDirectoryPath As String = IO.Path.Combine(surveyDirectoryPath, "ErrorSurfaces")
        '    If Not IO.Directory.Exists(ErrorSurfacesDirectoryPath) Then
        '        IO.Directory.CreateDirectory(ErrorSurfacesDirectoryPath)
        '    End If

        '    'ErrorSurfaceName directory
        '    Dim sSafeErrorName As String = naru.os.File.RemoveDangerousCharacters(sErrorName)
        '    Dim ErrorNameDirectoryPath As String = IO.Path.Combine(ErrorSurfacesDirectoryPath, sSafeErrorName)
        '    If Not IO.Directory.Exists(ErrorNameDirectoryPath) Then
        '        IO.Directory.CreateDirectory(ErrorNameDirectoryPath)
        '    End If

        '    ' PGB 14 Sep 2011 - trying to avoid these characters now and use io.path.combine instread
        '    'errorCalcDirectoryPath &= IO.Path.DirectorySeparatorChar
        '    'generate error name
        '    Dim errorCalcPath As String = GetNewSafeNameRaster(ErrorNameDirectoryPath, GISCode.GCDConsoleLib.Raster.GetDefaultRasterType(), sErrorName, 12)
        '    Return errorCalcPath
        'End Function


        'Public Function GetChangeDetectionDirectoryPath(ByVal NewSurveyName As String, OldSurveyName As String, ByVal sDoDName As String) As String
        '    'structure: Analyses/ChangeDetection/Dods_NewSurveyName-OldSurveyName/DoDName

        '    Dim safeDoDName As String = naru.os.File.RemoveDangerousCharacters(sDoDName)

        '    'setup folder for analysis
        '    Dim analysisDirectoryPath As String = IO.Path.Combine(GCDProjectFolder, "Analyses")
        '    If Not IO.Directory.Exists(analysisDirectoryPath) Then
        '        IO.Directory.CreateDirectory(analysisDirectoryPath)
        '    End If

        '    'setup folder for ChangeDetection
        '    Dim ChangeDetectionDirectoryPath As String = IO.Path.Combine(analysisDirectoryPath, "ChangeDetection")
        '    If Not IO.Directory.Exists(ChangeDetectionDirectoryPath) Then
        '        IO.Directory.CreateDirectory(ChangeDetectionDirectoryPath)
        '    End If

        '    'setup folder for dods directory
        '    Dim DoDsDirectoryName As String = "DoDs_" & NewSurveyName & "-" & OldSurveyName
        '    Dim SafeDoDsDirectoryName As String = naru.os.File.RemoveDangerousCharacters(DoDsDirectoryName)
        '    Dim DoDsDirectoryPath As String = IO.Path.Combine(ChangeDetectionDirectoryPath, SafeDoDsDirectoryName)
        '    If Not IO.Directory.Exists(DoDsDirectoryPath) Then
        '        IO.Directory.CreateDirectory(DoDsDirectoryPath)
        '    End If

        '    'setup folder for dod
        '    Dim DoDDirectoryPath As String = IO.Path.Combine(DoDsDirectoryPath, safeDoDName)
        '    If Not IO.Directory.Exists(DoDDirectoryPath) Then
        '        IO.Directory.CreateDirectory(DoDDirectoryPath)
        '    End If

        '    Return DoDDirectoryPath
        'End Function

        Public Function GetErrorRasterPath(ByVal sSurveyName As String, ByVal sErrorName As String) As String
            'structure: Inputs/Surveyname/ErrorSurfaces/ErrorSurfaceName/RasterName

            'input directory
            Dim inputsDirectoryPath As String = IO.Path.Combine(GCDProjectFolder, "Inputs")
            If Not IO.Directory.Exists(inputsDirectoryPath) Then
                IO.Directory.CreateDirectory(inputsDirectoryPath)
            End If

            'Survey directory
            Dim sSafeSurveyName As String = naru.os.File.RemoveDangerousCharacters(sSurveyName)
            Dim surveyDirectoryPath As String = IO.Path.Combine(inputsDirectoryPath, sSafeSurveyName)
            If Not IO.Directory.Exists(surveyDirectoryPath) Then
                IO.Directory.CreateDirectory(surveyDirectoryPath)
            End If

            'Error Surfaces directory
            Dim ErrorSurfacesDirectoryPath As String = IO.Path.Combine(surveyDirectoryPath, "ErrorSurfaces")
            If Not IO.Directory.Exists(ErrorSurfacesDirectoryPath) Then
                IO.Directory.CreateDirectory(ErrorSurfacesDirectoryPath)
            End If

            'ErrorSurfaceName directory
            Dim sSafeErrorName As String = naru.os.File.RemoveDangerousCharacters(sErrorName)
            Dim ErrorNameDirectoryPath As String = IO.Path.Combine(ErrorSurfacesDirectoryPath, sSafeErrorName)
            If Not IO.Directory.Exists(ErrorNameDirectoryPath) Then
                IO.Directory.CreateDirectory(ErrorNameDirectoryPath)
            End If

            ' PGB 14 Sep 2011 - trying to avoid these characters now and use io.path.combine instread
            'errorCalcDirectoryPath &= IO.Path.DirectorySeparatorChar
            'generate error name
            Dim errorCalcPath As String = naru.os.File.GetNewSafeName(ErrorNameDirectoryPath, sErrorName, GCDProject.ProjectManagerBase.RasterExtension).FullName
            Return errorCalcPath
        End Function

        Public Function GetAssociatedSurfaceCopyPath(ByVal sSurveyName As String, ByVal AssociatedSurfaceName As String, ByVal OrigRasterName As String) As String
            'structure: Inputs/Surveyname/AssociatedSurfaces/surfacename/rastername

            'input directory
            Dim inputsDirectoryPath As String = IO.Path.Combine(GCDProjectFolder, "Inputs")
            If Not IO.Directory.Exists(inputsDirectoryPath) Then
                CreateDirectory(inputsDirectoryPath)
            End If

            'Survey directory
            Dim sSafeSurveyName As String = naru.os.File.RemoveDangerousCharacters(sSurveyName)
            Dim surveyDirectoryPath As String = IO.Path.Combine(inputsDirectoryPath, sSafeSurveyName)
            If Not IO.Directory.Exists(surveyDirectoryPath) Then
                CreateDirectory(surveyDirectoryPath)
            End If

            'Associated Surfaces directory
            Dim AssociatedSurfacesDirectoryPath As String = IO.Path.Combine(surveyDirectoryPath, "AssociatedSurfaces")
            If Not IO.Directory.Exists(AssociatedSurfacesDirectoryPath) Then
                CreateDirectory(AssociatedSurfacesDirectoryPath)
            End If

            'Associated Surface directory
            Dim SafeAssociatedSurfaceName As String = naru.os.File.RemoveDangerousCharacters(AssociatedSurfaceName)
            Dim AssociatedSurfaceNameDirectoryPath As String = IO.Path.Combine(AssociatedSurfacesDirectoryPath, SafeAssociatedSurfaceName)
            If Not IO.Directory.Exists(AssociatedSurfaceNameDirectoryPath) Then
                CreateDirectory(AssociatedSurfaceNameDirectoryPath)
            End If

            'Raster path
            Dim AssociatedSurfaceCopyPath As String = IO.Path.Combine(AssociatedSurfaceNameDirectoryPath, OrigRasterName)
            Return AssociatedSurfaceCopyPath


        End Function

        Public Function GetWaterSurfaceRasterPath(ByVal sWaterSurfaceName As String) As String

            'structure: Inputs/WaterSurfaces/rastername

            'input directory
            Dim inputsDirectoryPath As String = IO.Path.Combine(GCDProjectFolder, "Inputs")
            If Not IO.Directory.Exists(inputsDirectoryPath) Then
                CreateDirectory(inputsDirectoryPath)
            End If

            ' Water Surfaces directory
            Dim sSurfacesDirectoryPath As String = IO.Path.Combine(inputsDirectoryPath, "WaterSurfaces")
            If Not IO.Directory.Exists(sSurfacesDirectoryPath) Then
                CreateDirectory(sSurfacesDirectoryPath)
            End If

            'Dim sExtension As String = GISCode.Raster.GetRasterType(My.Settings.DefaultRasterFormat)
            'Dim eType As GCDConsoleLib.Raster.RasterTypes = Me.GetDefaultRasterTyp
            Dim sRasterPath As String = naru.os.File.GetNewSafeName(sSurfacesDirectoryPath, sWaterSurfaceName, GCDProject.ProjectManagerBase.RasterExtension).FullName
            'sRasterPath = IO.Path.Combine(sSurfacesDirectoryPath, sRasterPath)
            'sRasterPath = IO.Path.ChangeExtension(sRasterPath, GISCode.Raster.GetRasterExtension(sExtension))

            Return sRasterPath

        End Function

        Public Function GetReservoirPath(ByVal sDEMName As String, ByVal sWaterSurfaceName As String) As String

            Dim sReservoirPath As String
            Dim sTopfolder As String = GetReservoirParentFolder()

            sReservoirPath = IO.Path.Combine(sTopfolder, naru.os.File.RemoveDangerousCharacters(sDEMName) & "_" & naru.os.File.RemoveDangerousCharacters(sWaterSurfaceName))
            If Not IO.Directory.Exists(sReservoirPath) Then
                CreateDirectory(sReservoirPath)
            End If

            Return sReservoirPath

        End Function

        Public Function GetReservoirParentFolder() As String

            Dim sTopFolder As String = IO.Path.Combine(GCDProjectFolder, "Reservoir")
            If Not IO.Directory.Exists(sTopFolder) Then
                CreateDirectory(sTopFolder)
            End If

            Return sTopFolder

        End Function

        Public Function GetChangeDetectionFiguresFolder(sParentFolder As String, bCreate As Boolean) As String

            Dim sResult As String
            If IO.Directory.Exists(sParentFolder) Then
                sResult = IO.Path.Combine(sParentFolder, m_sFiguresSubfolder)
                If bCreate AndAlso Not IO.Directory.Exists(sResult) Then
                    IO.Directory.CreateDirectory(sResult)
                End If
            Else
                Throw New ArgumentException("The parent folder must already exist.", "sParentFolder")
            End If
            Return sResult
        End Function

    End Class

End Namespace