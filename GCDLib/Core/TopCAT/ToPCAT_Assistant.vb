Imports System.IO
Imports System.Windows.Forms

Namespace Core.TopCAT

    Public Class ToPCAT_Assistant

        Public Shared Function MoveToPCAT_TextFiles(ByVal pointCloudFullPath As String, ByVal newDirectory As String, Optional ByRef sMessages As String = "") As Boolean

            sMessages = String.Empty
            Dim bResult As Boolean = True

            Dim m_txtFileNames As String() = {"_zstat.txt", "_zmin.txt", "_zmax.txt", "_underpopulated_zstat.txt", "_sorted", ""}
            Dim fileNameNoExtension As String = Path.GetFileNameWithoutExtension(pointCloudFullPath)
            Dim inFilePathNoExtension As String = Path.Combine(Path.GetDirectoryName(pointCloudFullPath), fileNameNoExtension)
            Dim messageBuilder As New Text.StringBuilder

            For Each txtFileEnding As String In m_txtFileNames
                Dim sOriginalFile As String = inFilePathNoExtension & txtFileEnding
                Dim sDestinationFile As String = Path.Combine(newDirectory, fileNameNoExtension & txtFileEnding)
                Debug.WriteLine("Moving TopCAT output from: " & sOriginalFile & ", to: " & sDestinationFile)

                Try
                    My.Computer.FileSystem.MoveFile(sOriginalFile, sDestinationFile)
                Catch ex As Exception
                    messageBuilder.Append(sOriginalFile & " could not be moved to the output folder: " & sDestinationFile).AppendLine()
                    bResult = False
                End Try
            Next

            sMessages = messageBuilder.ToString
            Return bResult

        End Function

        Public Shared Function MoveToPCAT_File(ByVal rawPointCloud As TopCAT.RawPointCloudPaths,
                                               ByVal tpFileType As ToPCAT_FileType,
                                               ByRef sMessages As String) As Boolean

            sMessages = String.Empty
            Dim bResult As Boolean = True

            Dim sOriginalFile As String = Path.Combine(rawPointCloud.BaseDirectory, rawPointCloud.Name)
            Dim messageBuilder As New Text.StringBuilder
            Dim sDestinationFile As String = Path.Combine(rawPointCloud.OutputDirectory, rawPointCloud.Name)

            Try

                Select Case tpFileType

                    Case ToPCAT_FileType.zStat
                        sOriginalFile &= "_zstat.txt"
                        sDestinationFile &= "_zstat.txt"
                        My.Computer.FileSystem.MoveFile(sOriginalFile, sDestinationFile)

                    Case ToPCAT_FileType.zMin
                        sOriginalFile &= "_zmin.txt"
                        sDestinationFile &= "_zmin.txt"
                        My.Computer.FileSystem.MoveFile(sOriginalFile, sDestinationFile)

                    Case ToPCAT_FileType.zMax
                        sOriginalFile &= "_zmax.txt"
                        sDestinationFile &= "_zmax.txt"
                        My.Computer.FileSystem.MoveFile(sOriginalFile, sDestinationFile)

                    Case ToPCAT_FileType.zStatUnderpopulated
                        sOriginalFile &= "_underpopulated_zstat.txt"
                        sDestinationFile &= "_underpopulated_zstat.txt"
                        My.Computer.FileSystem.MoveFile(sOriginalFile, sDestinationFile)

                    Case ToPCAT_FileType.zBinarySorted
                        sOriginalFile &= "_sorted"
                        sDestinationFile &= "_sorted"
                        My.Computer.FileSystem.MoveFile(sOriginalFile, sDestinationFile)

                    Case ToPCAT_FileType.zBinaryStat
                        sOriginalFile &= ""
                        sDestinationFile &= ""
                        My.Computer.FileSystem.MoveFile(sOriginalFile, sDestinationFile)

                End Select
            Catch ex As Exception
                messageBuilder.Append(ex.Message).AppendLine()
                messageBuilder.Append(sOriginalFile & " could not be moved to the output folder: " & sDestinationFile).AppendLine()
                sMessages = messageBuilder.ToString
                Return False
            End Try

            Return True

        End Function

        Public Shared Function ToPCATPrep(ByVal pFilePath As String, ByVal seperator As String, ByVal pOutputFilePath As String) As String

            Dim ctLinesRead As UInteger = 0
            Dim ctLinesWritten As UInteger = 0

            If Not String.IsNullOrEmpty(pFilePath) Then
                If File.Exists(pFilePath) Then
                    Using streamWriter = New System.IO.StreamWriter(pOutputFilePath)
                        Using streamReader = New System.IO.StreamReader(pFilePath)
                            Do Until streamReader.EndOfStream
                                Dim currentLine As String = streamReader.ReadLine()
                                ctLinesRead += 1
                                Try
                                    Dim correctedLine As String = currentLine.Replace(seperator, " ")
                                    streamWriter.WriteLine(correctedLine)
                                    ctLinesWritten += 1
                                Catch ex As Exception
                                    Continue Do
                                End Try
                            Loop
                        End Using
                    End Using
                End If
            End If
            Dim msgBuilder As New Text.StringBuilder
            Dim numberOfLinesNotWritten = ctLinesRead - ctLinesWritten
            If numberOfLinesNotWritten = 0 Then
                msgBuilder.Append("File processing complete.").AppendLine()
            Else
                msgBuilder.Append("File processed with: " & numberOfLinesNotWritten.ToString("#,##0") & " lines omitted due to errors.").AppendLine()
            End If
            msgBuilder.Append("Total lines in file: " & ctLinesRead.ToString("#,##0")).AppendLine()
            msgBuilder.Append("Total lines written to ToPCAT ready file: " & ctLinesWritten.ToString("#,##0")).AppendLine()
            Dim msgText As String = msgBuilder.ToString
            Return msgText

        End Function

        Public Shared Function CheckIfToPCAT_Ready(ByVal rawPointCloudPath As String)

            If Not File.Exists(rawPointCloudPath) Then
                Return False
            End If

            Dim bResult As Boolean
            Using checkDelimiterReader As New System.IO.StreamReader(rawPointCloudPath, False)
                checkDelimiterReader.ReadLine()

                Dim checkLine As String = checkDelimiterReader.ReadLine()
                If Not String.IsNullOrEmpty(checkLine) Then

                    checkDelimiterReader.Close()
                    bResult = CheckLineReady(checkLine)

                Else
                    checkLine = checkDelimiterReader.ReadLine()
                    bResult = CheckLineReady(checkLine)

                End If
            End Using
            Return bResult
        End Function

        Private Shared Function CheckLineReady(ByVal line As String)

            Try
                Dim lineArray = line.Split(" ")
                For i As Integer = 0 To 2
                    Dim doubleCheck As Double = CType(lineArray(i), Double)
                Next
            Catch ex As Exception
                Return False
            End Try

            Return True

        End Function

        Public Shared Function ValidateMultipleResolutionInputs(ByVal xRes1 As System.Windows.Forms.NumericUpDown, ByVal xRes2 As System.Windows.Forms.NumericUpDown,
                                                                ByVal yRes1 As System.Windows.Forms.NumericUpDown, ByVal yRes2 As System.Windows.Forms.NumericUpDown) As Boolean

            Dim ct As Integer = 0
            If xRes2.Value Mod xRes1.Value <> 0 Then
                MessageBox.Show("The value for x resolution 2 is invalid!" & vbCrLf & vbCrLf & "Value of x resolution 2 must be divisible by x resolution 1 to create a concurrent and orthagonal final raster. Change your input.", "Error with Input X Resolution Value")
                xRes2.Value = xRes1.Value * 2
                ct += 1
            End If

            If yRes2.Value Mod yRes1.Value <> 0 Then
                MessageBox.Show("The value for y resolution 2 is invalid!" & vbCrLf & vbCrLf & "Value of y resolution 2 must be divisible by y resolution 1 to create a concurrent and orthagonal final raster. Change your input.", "Error with Input Y Resolution Value")
                yRes2.Value = yRes1.Value * 2
                ct += 1
            End If
            If xRes2.Value <= xRes1.Value Then
                MessageBox.Show("The value for x resolution 2 is invalid!" & vbCrLf & vbCrLf & "Value of x resolution 2 must be greater than x resolution 1 to create a concurrent and orthagonal final raster. Change your input.", "Error with Input X Resolution Value")
                xRes2.Value = xRes1.Value * 2
                ct += 1
            End If
            If yRes2.Value <= yRes1.Value Then
                MessageBox.Show("The value for y resolution 2 is invalid!" & vbCrLf & vbCrLf & "Value of y resolution 2 must be greater than y resolution 1 to create a concurrent and orthagonal final raster. Change your input.", "Error with Input Y Resolution Value")
                yRes2.Value = yRes1.Value * 2
                ct += 1
            End If

            If ct = 0 Then
                Return True
            ElseIf ct > 0 Then
                Return False
            End If

        End Function

        Public Shared Sub PreviewFirstLine(ByVal pFilePath As String)

            If Not String.Compare(pFilePath, "") = 0 Then
                Dim filePreview As New System.IO.StreamReader(pFilePath, False)
                Dim header As String = filePreview.ReadLine()
                Dim firstLine As String = filePreview.ReadLine()

                MsgBox("First line of file: " & header & vbCrLf & vbCrLf & "Second line of file: " & firstLine & vbCrLf, MsgBoxStyle.OkOnly, "Line Preview")
                filePreview.Close()
            Else
                MsgBox("Please select a file to preview.", MsgBoxStyle.OkOnly, "Select a File")
            End If

        End Sub

        Public Shared Sub ResolutionWarning()

            MessageBox.Show("This value is invalid! Value must be greater than 0. Change your input." & vbCrLf & vbCrLf &
                             "If you need to enter a value below 0.50, then manually type in the value in the resolution selection box.", "Error with Input Value")

        End Sub

        Public Shared Function RunToPCat(ByVal pPointCloudPath As String,
                                                    ByVal xRes As Decimal,
                                                    ByVal yRes As Decimal,
                                                    ByVal nMin As String,
                                                    Optional ByVal bZMin As Boolean = False,
                                                    Optional ByVal bZMean As Boolean = True,
                                                    Optional ByVal stdev As Integer = 0) As String

            'Code to check for and correct if user is using machine with european decimal place
            Dim sXRes As String = xRes.ToString(Globalization.CultureInfo.InvariantCulture)
            Dim sYRes As String = yRes.ToString(Globalization.CultureInfo.InvariantCulture)

            'GET THE PATH OF THE TOOLBAR TO GAIN THE ABILITY TO POINT TO TOPCAT.EXE
            Dim sPath As String = System.Reflection.Assembly.GetExecutingAssembly.Location
            sPath = IO.Path.GetDirectoryName(sPath)
            sPath = IO.Path.Combine(sPath, "TopCAT")

            Dim zmin As Integer = If(bZMin, 1, 0)
            Dim zmean As Integer = If(bZMean, 1, 0)

            ' Check the architecture of the host computer and run the appropriate
            ' version of TopCAT. Note: ArcGIS Desktop is always 32 bit, but
            ' we can call 64 bit because TopCAT is called independently on command
            ' line.
            Dim topcatVersion As String = "ToPCATx86.exe"
            Try
                If GISCode.OperatingSystem.Is64BitOperatingSystem Then
                    topcatVersion = "ToPCATx64.exe"
                End If
            Catch ex As Exception
                topcatVersion = "ToPCATx86.exe"
            End Try

            sPath = IO.Path.Combine(sPath, topcatVersion)
            If sPath.Contains(" ") Then
                sPath = """" & sPath & """"
            End If

            'If Not IO.File.Exists(sPath) Then
            '    Dim ex As New Exception("The TopCAT executable is not in the expected location")
            '    ex.Data("TopCAT Executable Path") = sPath
            '    Throw ex
            'End If

            Dim resultsLog As String = IO.Path.Combine(IO.Path.GetDirectoryName(pPointCloudPath), "ResultsLog.txt")
            Dim topCAT_BatFile As String = sPath & " " & Chr(34) & pPointCloudPath & Chr(34) &
                                           " --xres " & sXRes &
                                           " --yres " & sYRes &
                                           " --nmin " & nMin &
                                           " --*zmin " & zmin.ToString &
                                           " --*zmean " & zmean.ToString &
                                           " --*stdev " & stdev.ToString &
                                           " > " & Chr(34) & resultsLog & Chr(34) & "2>&1"

            '#If DEBUG Then
            '            topCAT_BatFile &= vbCrLf & "@pause"
            '#End If

            'CREATE THE BATCH FILE IN THE %TEMP% FOLDER OF THE USER
            Dim topCatBatFilePath As String = Environ("TEMP")
            topCatBatFilePath = IO.Path.Combine(topCatBatFilePath, "runToPCAT_" & Now.ToString("yyMMdd_HHmmss") & ".bat")

            Try
                Dim batFileWriter As New System.IO.StreamWriter(topCatBatFilePath, False)
                batFileWriter.WriteLine(topCAT_BatFile)
                batFileWriter.Close()
            Catch ex As Exception
                Dim ex2 As New Exception("Error writing TopCAT batch file.", ex)
                ex2.Data("Batch File Path") = topCatBatFilePath
                ex2.Data("TopCAT Parameters") = topCAT_BatFile
                Throw ex2
            End Try

            Try
                Shell(topCatBatFilePath, AppWinStyle.Hide, True)
            Catch ex As Exception
                Dim ex2 As New Exception("Error executing TopCat command line using batch file", ex)
                ex2.Data("Batch File Path") = topCatBatFilePath
                ex2.Data("TopCAT Parameters") = topCAT_BatFile
                Throw ex2
            End Try

            Dim messageBuilder As New Text.StringBuilder
            Using resultsReader As New StreamReader(resultsLog)
                Do Until resultsReader.EndOfStream
                    messageBuilder.Append(resultsReader.ReadLine).AppendLine()
                Loop

            End Using

            Return messageBuilder.ToString

        End Function

        Public Shared Sub CollectToPCAT_FormOutputOptions(ByVal pPointCloudPaths As RawPointCloudPaths,
                                                          ByVal pOutPath As String,
                                                          ByVal IncludeText As Boolean,
                                                          ByVal IncludeBinaryZstat As Boolean,
                                                          ByVal IncludeBinarySorted As Boolean)

            'Dim outFilePathNoExtension As String = Path.Combine(m_OutputPath_FolderDialog.SelectedPath,
            '                                                    Path.GetFileNameWithoutExtension(txtBox_RawPointCloudFile.Text))

            If IncludeBinarySorted = False Then
                If File.Exists(pPointCloudPaths.binarySortedBase) Then
                    IO.File.Delete(pPointCloudPaths.binarySortedBase)
                End If
            ElseIf IncludeBinarySorted = True Then
                If File.Exists(pPointCloudPaths.binarySortedBase) Then
                    IO.File.Move(pPointCloudPaths.binarySortedBase, pPointCloudPaths.binarySortedMoved)
                End If
            End If

            If IncludeBinaryZstat = False Then
                If File.Exists(pPointCloudPaths.binaryBase) Then
                    IO.File.Delete(pPointCloudPaths.binaryBase)
                End If
            ElseIf IncludeBinaryZstat = True Then
                If File.Exists(pPointCloudPaths.binaryBase) Then
                    IO.File.Move(pPointCloudPaths.binaryBase, pPointCloudPaths.binaryMoved)
                End If
            End If

            Dim orginalFileNoExtension As String = IO.Path.Combine(pPointCloudPaths.BaseDirectory, pPointCloudPaths.Name)

            If IncludeText = False Then
                If IO.File.Exists(orginalFileNoExtension & "_zstat.txt") Then
                    IO.File.Delete(orginalFileNoExtension & "_zstat.txt")
                End If
                If IO.File.Exists(orginalFileNoExtension & "_zmin.txt") Then
                    IO.File.Delete(orginalFileNoExtension & "_zmin.txt")
                End If
                If IO.File.Exists(orginalFileNoExtension & "_zmax.txt") Then
                    IO.File.Delete(orginalFileNoExtension & "_zmax.txt")
                End If
                If IO.File.Exists(orginalFileNoExtension & "_underpopulated_zstat.txt") Then
                    IO.File.Delete(orginalFileNoExtension & "_underpopulated_zstat.txt")
                End If
            End If

        End Sub

        Public Shared Sub CreateRasters(ByVal pPointCloudPath As String,
                                                    ByVal cellRes As String,
                                                    ByVal rasterField As String,
                                                    ByVal spatialReference As String,
                                                    ByVal pOutputRasterPath As String)
            ''''''''''''''''''''''''''''''''''
            'PYTHON
            '
            'CREATE BATCH FILE TO OUTPUT THE SURFACE ROUGHNESS RASTER
            Dim joinPath = {IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location), "pythonScripts\simpleRoughnessModel.py"}
            'joinPath(0) = [IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)]["pythonScripts\simpleRoughnessModel.py"]

            Dim pythonFile As String = [String].Join("\", joinPath)
            'Dim pythonFile As String = [String].Join("\", IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location),
            '                                 "pythonScripts\simpleRoughnessModel.py")

            Dim esriPythonPath As String = WindowsManagement.getESRI_PythonPath()
            Dim ESRI_PythonBasePath As String = Path.GetDirectoryName(esriPythonPath)

            Dim python_BatFile As String
            python_BatFile = "cd " & ESRI_PythonBasePath & vbCrLf &
                             "python " & Chr(34) & pythonFile & Chr(34) & " " &
                             Chr(34) & pPointCloudPath & Chr(34) & " " &
                             cellRes & " " &
                             Chr(34) & pOutputRasterPath & Chr(34) & " " &
                             Chr(34) & rasterField & Chr(34) & " " &
                             "--spatialReference " & Chr(34) & spatialReference & Chr(34) & vbCrLf &
                             "@pause"


            'WRITE THE BATCH FILE   
            Dim python_BatFilePath As String = Environ("TEMP")
            python_BatFilePath = IO.Path.Combine(python_BatFilePath, "runToPCATOutShp.bat")
            Dim batFileWriter As New System.IO.StreamWriter(python_BatFilePath, False)
            batFileWriter.WriteLine(python_BatFile)
            batFileWriter.Close()

            'RUN THE BATCH FILE
            Shell(python_BatFilePath, AppWinStyle.NormalFocus, True)

        End Sub

        Public Shared Sub RunToPCatAndCreateRasters(ByVal pPointCloudPath As String,
                                                    ByVal xRes As String,
                                                    ByVal yRes As String,
                                                    ByVal nMin As String,
                                                    ByVal rasterField As String,
                                                    ByVal spatialReference As String,
                                                    ByVal pOutputRasterPath As String)

            ''''''''''''''''''''''''''''
            'TOPCAT
            '
            'GET THE PATH OF THE TOOLBAR TO GAIN THE ABILITY TO POINT TO TOPCAT.EXE
            Dim sPath As String = System.Reflection.Assembly.GetExecutingAssembly.Location
            sPath = IO.Path.GetDirectoryName(sPath)

            Dim topCAT_BatFile As String = "cd " & sPath & vbCrLf &
                                           "ToPCAT.exe" & " " & Chr(34) & pPointCloudPath & Chr(34) &
                                           " --xres " & xRes &
                                           " --yres " & yRes &
                                           " --nmin " & nMin &
                                           " --*zmin 0" &
                                           " --*zmean 1" &
                                           " --*stdev 0" & vbCrLf

            'CREATE THE BATCH FILE IN THE %TEMP% FOLDER OF THE USER
            Dim topCatBatFilePath As String = Environ("TEMP")
            topCatBatFilePath = IO.Path.Combine(topCatBatFilePath, "runToPCAT.bat")

            Dim batFileWriter As New System.IO.StreamWriter(topCatBatFilePath, False)
            batFileWriter.WriteLine(topCAT_BatFile)
            batFileWriter.Close()
            Shell(topCatBatFilePath, AppWinStyle.NormalFocus, True)

            ''''''''''''''''''''''''''''''''''
            'PYTHON
            '
            'CREATE BATCH FILE TO OUTPUT THE SURFACE ROUGHNESS RASTER

            Dim joinPath = {IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location), "pythonScripts\simpleRoughnessModel.py"}
            'joinPath(0) = [IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)]["pythonScripts\simpleRoughnessModel.py"]

            Dim pythonFile As String = [String].Join("\", joinPath)

            'Dim pythonFile As String = [String].Join("\", IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location),
            '                                 "pythonScripts\simpleRoughnessModel.py")

            Dim esriPythonPath As String = WindowsManagement.getESRI_PythonPath()
            Dim ESRI_PythonBasePath As String = Path.GetDirectoryName(esriPythonPath)

            Dim python_BatFile As String
            python_BatFile = "cd " & ESRI_PythonBasePath & vbCrLf &
                             "python " & Chr(34) & pythonFile & Chr(34) & " " &
                             Chr(34) & pPointCloudPath & Chr(34) & " " &
                             xRes & " " &
                             Chr(34) & pOutputRasterPath & Chr(34) & " " &
                             Chr(34) & rasterField & Chr(34) & " " &
                             "--spatialReference " & Chr(34) & spatialReference & Chr(34) & vbCrLf

            'WRITE THE BATCH FILE   
            Dim python_BatFilePath As String = Environ("TEMP")
            python_BatFilePath = IO.Path.Combine(python_BatFilePath, "runToPCATOutShp.bat")
            Dim batFileWriter2 As New System.IO.StreamWriter(python_BatFilePath, False)
            batFileWriter2.WriteLine(python_BatFile)
            batFileWriter2.Close()

            'RUN THE BATCH FILE
            Shell(python_BatFilePath, AppWinStyle.NormalFocus, True)

        End Sub

        Public Shared Function CreateSurfaceRoughnessModel(ByVal pRasterResolution1 As String, ByVal pRasterResolution2 As String, ByVal pOutputRasterPath As String, ByVal xRes As String)

            ''''''''''''''''''''''''''''''''''
            'PYTHON
            '
            'CREATE BATCH FILE TO OUTPUT THE SURFACE ROUGHNESS RASTER

            Dim joinPath = {IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location), "pythonScripts\toPCAT_MultiResolutionSR.py"}
            'joinPath(0) = [IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)]["pythonScripts\simpleRoughnessModel.py"]

            Dim pythonFile As String = [String].Join("\", joinPath)
            'Dim pythonFile As String = [String].Join("\", IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location),
            '                                 "pythonScripts\toPCAT_MultiResolutionSR.py")

            Dim esriPythonPath As String = WindowsManagement.getESRI_PythonPath()
            Dim ESRI_PythonBasePath As String = Path.GetDirectoryName(esriPythonPath)

            Dim python_BatFile As String
            python_BatFile = "cd " & ESRI_PythonBasePath & vbCrLf &
                             "python " & Chr(34) & pythonFile & Chr(34) & " " &
                             Chr(34) & pRasterResolution1 & Chr(34) & " " &
                             Chr(34) & pRasterResolution2 & Chr(34) & " " &
                             Chr(34) & pOutputRasterPath & Chr(34) & " " &
                             xRes & vbCrLf

            'WRITE THE BATCH FILE   
            Dim python_BatFilePath As String = Environ("TEMP")
            python_BatFilePath = IO.Path.Combine(python_BatFilePath, "runtoPCAT_MultiResolutionSR.bat")
            Dim batFileWriter As New System.IO.StreamWriter(python_BatFilePath, False)
            batFileWriter.WriteLine(python_BatFile)
            batFileWriter.Close()

            'RUN THE BATCH FILE
            Shell(python_BatFilePath, AppWinStyle.NormalFocus, True)

            If System.IO.File.Exists(pOutputRasterPath) Then
                Return True
            ElseIf Not System.IO.File.Exists(pOutputRasterPath) Then
                Return False
            Else
                Return Nothing
            End If

        End Function

        Public Shared Function getZstatColumnIndex(ByVal fieldName As String)

            Select Case fieldName

                Case "zmean"
                    Return "zmean"
                Case "zmean detrended"
                    Return "zmean_detrended"
                Case "zmax"
                    Return "zmax"
                Case "zmin"
                    Return "zmin"
                Case "range"
                    Return "range"
                Case "standard deviation"
                    Return "stdev"
                Case "standard deviation detrended"
                    Return "stdev_detrended"
                Case "skew"
                    Return "sk"
                Case "skew detrended"
                    Return "sk-detr"
                Case "kurtosis"
                    Return "ku"
                Case "kurtosis detrended"
                    Return "ku-detr"
                Case "n"
                    Return "n"
                Case Else
                    Return Nothing
            End Select

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks></remarks>

        Enum ToPCAT_FileType
            zStat
            zMin
            zMax
            zStatUnderpopulated
            zBinaryStat
            zBinarySorted
        End Enum

        Public Shared Function CreateToPCATFields(ByVal fields As ESRI.ArcGIS.Geodatabase.IFields, ByVal topCatShpType As ToPCAT_Assistant.ToPCAT_FileType)

            Dim ZStatsFieldOrder() As String = {"zmean", "zmax", "zmin", "range", "stdev", "stdevDetr", "skew",
                                               "skewDetr", "kurt", "kurtDetr", "zmeanDetr", "n"}
            If fields Is Nothing Then
                'Dim objectClassDescription As ESRI.ArcGIS.Geodatabase.IObjectClassDescription = New ESRI.ArcGIS.Geodatabase.FeatureClassDescriptionClass
                'fields = objectClassDescription.RequiredFields
                'fields = New fields
            End If

            Dim fieldsEdit As ESRI.ArcGIS.Geodatabase.IFieldsEdit = CType(fields, ESRI.ArcGIS.Geodatabase.IFieldsEdit)

            Select Case topCatShpType
                Case ToPCAT_FileType.zStat
                    For Each stat In ZStatsFieldOrder
                        Dim field As ESRI.ArcGIS.Geodatabase.IField = New ESRI.ArcGIS.Geodatabase.FieldClass
                        Dim fieldEdit As ESRI.ArcGIS.Geodatabase.IFieldEdit = CType(field, ESRI.ArcGIS.Geodatabase.IFieldEdit)
                        fieldEdit.Name_2 = stat
                        fieldEdit.AliasName_2 = stat
                        fieldEdit.Editable_2 = True

                        Select Case stat

                            Case "n"
                                fieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeInteger

                            Case Else
                                fieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble

                        End Select
                        fieldsEdit.AddField(field)
                    Next

                Case ToPCAT_FileType.zStatUnderpopulated
                    For Each stat In ZStatsFieldOrder
                        Dim field As ESRI.ArcGIS.Geodatabase.IField = New ESRI.ArcGIS.Geodatabase.FieldClass
                        Dim fieldEdit As ESRI.ArcGIS.Geodatabase.IFieldEdit = CType(field, ESRI.ArcGIS.Geodatabase.IFieldEdit)
                        fieldEdit.Name_2 = stat
                        fieldEdit.AliasName_2 = stat
                        fieldEdit.Editable_2 = True

                        Select Case stat

                            Case "n"
                                fieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeInteger

                            Case Else
                                fieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble

                        End Select
                        fieldsEdit.AddField(field)
                    Next

                Case ToPCAT_FileType.zMin
                    Dim field As ESRI.ArcGIS.Geodatabase.IField = New ESRI.ArcGIS.Geodatabase.FieldClass
                    Dim fieldEdit As ESRI.ArcGIS.Geodatabase.IFieldEdit = CType(field, ESRI.ArcGIS.Geodatabase.IFieldEdit)
                    fieldEdit.Name_2 = "zmin"
                    fieldEdit.AliasName_2 = "zmin"
                    fieldEdit.Editable_2 = True
                    fieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
                    fieldsEdit.AddField(field)

                Case ToPCAT_FileType.zMax
                    Dim field As ESRI.ArcGIS.Geodatabase.IField = New ESRI.ArcGIS.Geodatabase.FieldClass
                    Dim fieldEdit As ESRI.ArcGIS.Geodatabase.IFieldEdit = CType(field, ESRI.ArcGIS.Geodatabase.IFieldEdit)
                    fieldEdit.Name_2 = "zmax"
                    fieldEdit.AliasName_2 = "zmax"
                    fieldEdit.Editable_2 = True
                    fieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
                    fieldsEdit.AddField(field)

            End Select

            ''Use IFieldChecker to create a validated fields collection
            'Dim fieldChecker As ESRI.ArcGIS.Geodatabase.IFieldChecker = New ESRI.ArcGIS.Geodatabase.FieldCheckerClass()
            'Dim enumFieldError As ESRI.ArcGIS.Geodatabase.IEnumFieldError = Nothing
            'Dim validatedFields As ESRI.ArcGIS.Geodatabase.IFields = Nothing
            'fieldChecker.ValidateWorkspace = CType(workspace, ESRI.ArcGIS.Geodatabase.IWorkspace)
            'fieldChecker.Validate(fields, enumFieldError, validatedFields)

            fields = CType(fieldsEdit, ESRI.ArcGIS.Geodatabase.IFields)
            Return fields

        End Function


        Public Shared Function CreateZStatFields(ByVal fields As ESRI.ArcGIS.Geodatabase.IFields)

            Dim ZStatsFieldOrder() As String = {"zmean", "zmax", "zmin", "range", "stdev", "stdevDetr", "skew",
                                               "skewDetr", "kurt", "kurtDetr", "zmeanDetr", "n"}
            If fields Is Nothing Then
                'Dim objectClassDescription As ESRI.ArcGIS.Geodatabase.IObjectClassDescription = New ESRI.ArcGIS.Geodatabase.FeatureClassDescriptionClass
                'fields = objectClassDescription.RequiredFields
                'fields = New fields
            End If

            Dim fieldsEdit As ESRI.ArcGIS.Geodatabase.IFieldsEdit = CType(fields, ESRI.ArcGIS.Geodatabase.IFieldsEdit)

            For Each stat In ZStatsFieldOrder
                Dim field As ESRI.ArcGIS.Geodatabase.IField = New ESRI.ArcGIS.Geodatabase.FieldClass
                Dim fieldEdit As ESRI.ArcGIS.Geodatabase.IFieldEdit = CType(field, ESRI.ArcGIS.Geodatabase.IFieldEdit)
                fieldEdit.Name_2 = stat
                fieldEdit.AliasName_2 = stat
                fieldEdit.Editable_2 = True

                Select Case stat

                    Case "n"
                        fieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeInteger

                    Case Else
                        fieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble

                End Select
                fieldsEdit.AddField(field)


            Next

            ''Use IFieldChecker to create a validated fields collection
            'Dim fieldChecker As ESRI.ArcGIS.Geodatabase.IFieldChecker = New ESRI.ArcGIS.Geodatabase.FieldCheckerClass()
            'Dim enumFieldError As ESRI.ArcGIS.Geodatabase.IEnumFieldError = Nothing
            'Dim validatedFields As ESRI.ArcGIS.Geodatabase.IFields = Nothing
            'fieldChecker.ValidateWorkspace = CType(workspace, ESRI.ArcGIS.Geodatabase.IWorkspace)
            'fieldChecker.Validate(fields, enumFieldError, validatedFields)

            fields = CType(fieldsEdit, ESRI.ArcGIS.Geodatabase.IFields)
            Return fields

        End Function

        Sub ExecuteCommandSync(ByVal Command As Object)

            Try

                ' Create the ProcessStartInfo using "cmd" as the program to be run,
                ' and "/c " as the parameters.
                ' "/c" tells cmd that you want it to execute the command that follows,
                ' then exit.
                Dim procStartInfo As System.Diagnostics.ProcessStartInfo = New System.Diagnostics.ProcessStartInfo("cmd", "/c " + Command)

                ' The following commands are needed to redirect the standard output.
                ' This means that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = True
                procStartInfo.UseShellExecute = False
                procStartInfo.CreateNoWindow = False

                ' Now you create a process, assign its ProcessStartInfo, and start it.
                Dim proc As System.Diagnostics.Process = New System.Diagnostics.Process()
                proc.StartInfo = procStartInfo
                proc.Start()

                ' Get the output into a string.
                Dim result As String = proc.StandardOutput.ReadToEnd()

                ' Display the command output.
                MsgBox(result, MsgBoxStyle.Information, My.Resources.ApplicationNameLong)

            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                ' Log the exception and errors.

            End Try

        End Sub

        Public Shared Sub InsertToPCAT_Stats(ByRef pFC As ESRI.ArcGIS.Geodatabase.IFeatureClass, ByVal geometryListFilePath As String)

            Dim dFields As New List(Of FieldManager)

            'Get current culture info inorder to convert value from ToPCAT output into proper double format (*ToPCAT output is invariant culture)
            Dim pCultureInfo As Globalization.CultureInfo = New Globalization.CultureInfo(Globalization.CultureInfo.CurrentCulture.Name)
            Dim pInvariantCulture As Globalization.CultureInfo = New Globalization.CultureInfo("")

            'Create a feature buffer and insert cursor
            Dim featureBuffer As ESRI.ArcGIS.Geodatabase.IFeatureBuffer = pFC.CreateFeatureBuffer()
            Dim insertCursor As ESRI.ArcGIS.Geodatabase.IFeatureCursor = pFC.Insert(True)
            Dim pointGeometry As ESRI.ArcGIS.Geometry.IGeometry
            Using streamReader As New System.IO.StreamReader(geometryListFilePath)

                Dim sFirstLine As String() = streamReader.ReadLine().Split(",")
                Dim fieldCount As Integer = 0
                For Each aFieldName In sFirstLine
                    If aFieldName = "n" Then
                        dFields.Add(New FieldManager(aFieldName, fieldCount, pFC, ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeInteger))
                    Else
                        dFields.Add(New FieldManager(aFieldName, fieldCount, pFC, ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble))
                    End If
                    fieldCount += 1
                Next

                Dim rowCount As Integer = 0
                Do Until streamReader.EndOfStream

                    'Get the points
                    Try
                        Dim point As ESRI.ArcGIS.Geometry.IPoint = New ESRI.ArcGIS.Geometry.Point
                        Dim newLine As String() = streamReader.ReadLine().Split(",")
                        point.X = Double.Parse(newLine(0))
                        point.Y = Double.Parse(newLine(1))
                        pointGeometry = point
                        'Set the feature buffer's shape and insert it
                        featureBuffer.Shape = pointGeometry

                        For Each field In dFields
                            If String.Compare(field.Name, "x") = 0 Or String.Compare(field.Name, "y") = 0 Then
                                Continue For
                            End If
                            If String.Compare(field.Name, "n") = 0 Then
                                featureBuffer.Value(field.FeatureClassFieldIndex) = Integer.Parse(newLine(field.CSV_FieldIndex))
                            Else
                                If Not String.Compare(pCultureInfo.NumberFormat.NumberDecimalSeparator, pInvariantCulture.NumberFormat.NumberDecimalSeparator) = 0 Then
                                    Dim dCurrentCulture As Double = ConvertToCurrentCulture(newLine(field.CSV_FieldIndex), pCultureInfo)
                                    featureBuffer.Value(field.FeatureClassFieldIndex) = dCurrentCulture
                                Else
                                    featureBuffer.Value(field.FeatureClassFieldIndex) = Double.Parse(newLine(field.CSV_FieldIndex))
                                End If
                            End If
                        Next

                        'Fill in miscellaneaous fields not in csv
                        For i As Integer = 0 To featureBuffer.Fields.FieldCount - 1

                            If featureBuffer.Value(i) Is Nothing Then
                                featureBuffer.Value(i) = featureBuffer.Fields.Field(i).DefaultValue
                            End If

                        Next

                    Catch ex As Exception
                        Continue Do
                    End Try

                    insertCursor.InsertFeature(featureBuffer)
                    rowCount += 1
                    If (rowCount Mod 2000) = 0 Then
                        insertCursor.Flush()
                        rowCount = 0
                    End If
                Loop

                insertCursor.Flush()
            End Using

            Dim comReferencesLeft As Integer
            Do
                comReferencesLeft = System.Runtime.InteropServices.Marshal.ReleaseComObject(insertCursor) _
                    + System.Runtime.InteropServices.Marshal.ReleaseComObject(pFC) _
                    + System.Runtime.InteropServices.Marshal.ReleaseComObject(featureBuffer)
            Loop While (comReferencesLeft > 0)

        End Sub

        Public Shared Function CreateToPCAT_Shp(ByVal pointCloudPaths As RawPointCloudPaths,
                                           ByVal shpFileType As GISDataStructures.BasicGISTypes,
                                           ByVal b3D As Boolean,
                                           ByRef spatialRef As ESRI.ArcGIS.Geometry.ISpatialReference,
                                           ByVal topCatShpType As ToPCAT_Assistant.ToPCAT_FileType,
                                           ByRef resultsReporter As String) As Boolean


            Dim outPath As String = Nothing
            Dim statPath As String = Nothing

            Try

                Select Case topCatShpType
                    Case ToPCAT_FileType.zStat
                        outPath = pointCloudPaths.zStatShapefile
                        statPath = pointCloudPaths.zStatText
                    Case ToPCAT_FileType.zMin
                        outPath = pointCloudPaths.zMinShapefile
                        statPath = pointCloudPaths.zMinText
                    Case ToPCAT_FileType.zMax
                        outPath = pointCloudPaths.zMaxShapefile
                        statPath = pointCloudPaths.zMaxText
                    Case ToPCAT_FileType.zStatUnderpopulated
                        outPath = pointCloudPaths.zStatUnderPopulatedShapefile
                        statPath = pointCloudPaths.zStatUnderPopulatedText

                End Select

                Dim fields As ESRI.ArcGIS.Geodatabase.IFields = New ESRI.ArcGIS.Geodatabase.FieldsClass()
                Dim zFields = ToPCAT_Assistant.CreateToPCATFields(fields, topCatShpType)
                Dim testShp As GISDataStructures.VectorDataSource = GISDataStructures.VectorDataSource.CreateFeatureClass(outPath,
                                                                      GISDataStructures.BasicGISTypes.Point,
                                                                      False,
                                                                      spatialRef,
                                                                      zFields)
                ToPCAT_Assistant.InsertToPCAT_Stats(testShp.FeatureClass, statPath)
            Catch ex As Exception
                resultsReporter = ex.Message
                Return resultsReporter
            End Try

            resultsReporter = outPath & " successfully created." & vbCrLf & vbCrLf
            Return True

        End Function


        ''' <summary>
        ''' Takes a string that is presumed to be a double, parses it to a double of number format dot for decimal seperator and converts it to a double of the current culture of the users machine
        ''' </summary>
        ''' <param name="sValue">string that can be converted to a double</param>
        ''' <param name="pCurrentCulture">Globalization.CultureInfo.CurrentCulture</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function ConvertToCurrentCulture(ByVal sValue As String, ByVal pCurrentCulture As Globalization.CultureInfo) As Double

            If String.IsNullOrEmpty(sValue) Then
                Return Double.MinValue
            End If

            Dim dInvariantCulture As Double = Double.Parse(sValue, New Globalization.CultureInfo(""))
            Dim sDoubleInvariantCulture As String = dInvariantCulture.ToString()
            Dim dCurrentCulture As Double = Convert.ToDouble(sDoubleInvariantCulture, pCurrentCulture)
            Return dCurrentCulture

        End Function

    End Class

    Public Class FieldManager
        Private m_sName As String
        Private m_nFeatureClassFldIdx As Integer
        Private m_nCSVFldIdx As Integer
        Private m_eFieldType As ESRI.ArcGIS.Geodatabase.esriFieldType

        Public ReadOnly Property FieldOrder As String()
            Get
                Return {"zmean", "zmax", "zmin", "range", "stdev", "stdevDetr", "skew",
                        "skewDetr", "kurt", "kurtDetr", "zmeanDetr", "n"}
            End Get
        End Property

        Public ReadOnly Property Name As String
            Get
                Return m_sName
            End Get
        End Property

        Public ReadOnly Property FeatureClassFieldIndex As Integer
            Get
                Return m_nFeatureClassFldIdx
            End Get
        End Property

        Public ReadOnly Property CSV_FieldIndex As Integer
            Get
                Return m_nCSVFldIdx
            End Get
        End Property

        Public ReadOnly Property ESRI_FieldType As ESRI.ArcGIS.Geodatabase.esriFieldType
            Get
                Return m_eFieldType
            End Get
        End Property

        Public Sub New(ByVal sName As String, ByVal nCSVIdx As Integer, ByVal pFC As ESRI.ArcGIS.Geodatabase.IFeatureClass, ByVal eFieldType As ESRI.ArcGIS.Geodatabase.esriFieldType)
            m_nCSVFldIdx = nCSVIdx
            m_sName = GetFC_SafeColumnName(sName)
            m_nFeatureClassFldIdx = pFC.FindField(m_sName)
            m_eFieldType = eFieldType
        End Sub

        Private Function GetFC_SafeColumnName(ByVal m_sName As String)
            Select Case m_sName
                Case "stdev_detrended"
                    Return "stdevDetr"
                Case "sk"
                    Return "skew"
                Case "sk-detr"
                    Return "skewDetr"
                Case "ku"
                    Return "kurt"
                Case "ku-detr"
                    Return "kurtDetr"
                Case "zmean_detrended"
                    Return "zmeanDetr"
                Case Else
                    Return m_sName
            End Select
        End Function


    End Class

    Public Class RawPointCloudPaths
        Private m_OriginalFile As String
        Private m_BaseName As String
        Private m_BaseDir As String
        Private m_OutputDir As String

        Public ReadOnly Property OriginalFile As String
            Get
                Return m_OriginalFile
            End Get
        End Property

        Public ReadOnly Property Name As String
            Get
                Return m_BaseName
            End Get
        End Property

        Public ReadOnly Property BaseDirectory As String
            Get
                Return m_BaseDir
            End Get
        End Property

        Public ReadOnly Property OutputDirectory As String
            Get
                Return m_OutputDir
            End Get
        End Property

        Public ReadOnly Property zStatText As String
            Get
                Return Path.Combine(m_OutputDir, m_BaseName & "_zstat.txt")
            End Get
        End Property

        Public ReadOnly Property zMinText As String
            Get
                Return Path.Combine(m_OutputDir, m_BaseName & "_zmin.txt")
            End Get
        End Property

        Public ReadOnly Property zMaxText As String
            Get
                Return Path.Combine(m_OutputDir, m_BaseName & "_zmax.txt")
            End Get
        End Property

        Public ReadOnly Property zStatUnderPopulatedText As String
            Get
                Return Path.Combine(m_OutputDir, m_BaseName & "_underpopulated_zstat.txt")
            End Get
        End Property

        Public ReadOnly Property zStatShapefile As String
            Get
                Return Path.Combine(m_OutputDir, m_BaseName & "_zstat.shp")
            End Get
        End Property

        Public ReadOnly Property zMinShapefile As String
            Get
                Return Path.Combine(m_OutputDir, m_BaseName & "_zmin.shp")
            End Get
        End Property

        Public ReadOnly Property zMaxShapefile As String
            Get
                Return Path.Combine(m_OutputDir, m_BaseName & "_zmax.shp")
            End Get
        End Property

        Public ReadOnly Property zStatUnderPopulatedShapefile As String
            Get
                Return Path.Combine(m_OutputDir, m_BaseName & "_underpopulated_zstat.shp")
            End Get
        End Property

        Public ReadOnly Property binaryBase
            Get
                Return IO.Path.Combine(m_BaseDir, m_BaseName)
            End Get
        End Property

        Public ReadOnly Property binaryMoved
            Get
                Return IO.Path.Combine(m_OutputDir, m_BaseName)
            End Get
        End Property

        Public ReadOnly Property binarySortedBase
            Get
                Return IO.Path.Combine(m_BaseDir, m_BaseName & "_sorted")
            End Get
        End Property

        Public ReadOnly Property binarySortedMoved
            Get
                Return IO.Path.Combine(m_OutputDir, m_BaseName & "_sorted")
            End Get
        End Property

        Public Sub New(ByVal sPath As String, ByVal sOutPath As String)
            m_OriginalFile = sPath
            m_BaseName = Path.GetFileNameWithoutExtension(sPath)
            m_BaseDir = Path.GetDirectoryName(sPath)
            m_OutputDir = sOutPath
        End Sub

    End Class

End Namespace