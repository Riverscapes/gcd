Imports System.Text

Namespace Core.BudgetSegregation

    Public Class MaskStatsClass
        Private _MaskStats As New Dictionary(Of String, ChangeDetection.StatsDataClass)
        Private _TotalStats As New ChangeDetection.StatsDataClass

        Public ReadOnly Property TotalStats As ChangeDetection.StatsDataClass
            Get
                Return _TotalStats
            End Get
        End Property

        Public ReadOnly Property MaskStats As Dictionary(Of String, ChangeDetection.StatsDataClass)
            Get
                Return _MaskStats
            End Get
        End Property

        Public Sub ExportSummaries(sExcelTemplatefolder As String, ByVal MaskOutputs As Dictionary(Of String, BudgetSegregationOutputsClass.MaskOutputClass), sDisplayUnits As String)

            For Each kvp As KeyValuePair(Of String, BudgetSegregationOutputsClass.MaskOutputClass) In MaskOutputs
                Dim MaskName As String = kvp.Key
                Dim MaskOutput As BudgetSegregationOutputsClass.MaskOutputClass = kvp.Value
                Dim SummaryPath As String = MaskOutput.SummaryPath
                Dim MaskStat As ChangeDetection.StatsDataClass = _MaskStats(MaskName)
                ExportMaskStat(sExcelTemplatefolder, MaskStat, SummaryPath, sDisplayUnits)
            Next

        End Sub

        ''' <summary>
        ''' Summarize all the mask classes together in one file.
        ''' </summary>
        ''' <param name="MaskOutputs">Mask output class</param>
        ''' <param name="sDisplayUnits">String representing the linear spatial units of the GIS data</param>
        ''' <remarks>PGB 26 Apr 2012. New XML output file that combines all the mask classes together
        ''' into one XML file. The format is based on the individual budget summary template, but the
        ''' value and percent columns are repeated for each mask class. This is done by having [REPEAT]
        ''' tags in the XML template that are copied by the code for each mask class. The values are 
        ''' then inserted the same way as the budget summary except that just the value is inserted into
        ''' just the first occurance of each tag (because the copying the repeat tags means there are
        ''' multiple copies of each tag.)</remarks>
        Public Sub ExportClassSummary(sTemplateFolder As String, sClassSummaryPath As String, ByVal MaskOutputs As Dictionary(Of String, BudgetSegregationOutputsClass.MaskOutputClass), sDisplayUnits As String)

            If Not IO.Directory.Exists(sTemplateFolder) Then
                Dim ex As New Exception("The template folder does not exist")
                ex.Data("Template folder") = sTemplateFolder
                Throw ex
            End If
            '
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Create the class summary XML file from the template
            '
            Dim TemplateFile As String = IO.Path.Combine(sTemplateFolder, "ClassSummary.xml")
            If Not IO.File.Exists(TemplateFile) Then
                Throw New Exception("Could not find required template '" & TemplateFile & "'")
            End If
            '
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Read the template file into a memory stream
            '
            Dim objReader As System.IO.StreamReader
            Try
                objReader = New System.IO.StreamReader(TemplateFile)
            Catch ex As System.IO.IOException
                'need to limit the length of the file to display properly
                Dim TrimmedFilename As String = GISCode.FileSystem.TrimFilename(TemplateFile, 80)
                ex.Data("UIMessage") = "Could not access the file '" & TrimmedFilename & "' because it is being used by another program."
                Throw ex
            End Try
            Dim TemplateText As String = objReader.ReadToEnd()
            objReader.Close()
            Dim OutputText As StringBuilder = New StringBuilder(TemplateText)
            OutputText.Replace("[LinearUnits]", sDisplayUnits)
            '
            ' Duplicate the repeat statements
            '
            OutputText = DuplicateRepeatStatements(OutputText, MaskOutputs.Count)
            OutputText.Replace("[CLASS_COUNT]", (MaskOutputs.Count * 2) + 1)

            For Each kvp As KeyValuePair(Of String, BudgetSegregationOutputsClass.MaskOutputClass) In MaskOutputs
                Dim MaskName As String = kvp.Key
                Dim MaskOutput As BudgetSegregationOutputsClass.MaskOutputClass = kvp.Value
                Dim MaskStat As ChangeDetection.StatsDataClass = _MaskStats(MaskName)

                Dim nIndex As Integer = OutputText.ToString.IndexOf("[CLASS_NAME]")
                OutputText.Replace("[CLASS_NAME]", MaskName, nIndex, "[CLASS_NAME]".Length)

                ReplaceTag(OutputText, "[TotalAreaOfErosionThresholded]", MaskStat.AreaErosion_Thresholded)

                ReplaceTag(OutputText, "[TotalAreaOfDepositionThresholded]", MaskStat.AreaDeposition_Thresholded)

                ReplaceTag(OutputText, "[TotalVolumeOfErosionThresholded]", MaskStat.VolumeErosion_Thresholded)

                ReplaceTag(OutputText, "[TotalVolumeOfDepositionThresholded]", MaskStat.VolumeDeposition_Thresholded)

                ReplaceTag(OutputText, "[TotalThresholdedVolumeOfDifference]", MaskStat.VolumeOfDifference_Thresholded)

                ReplaceTag(OutputText, "[TotalThresholdedNetVolumeDifference]", MaskStat.NetVolumeOfDifference_Thresholded)

                'percent of total
                ReplaceTag(OutputText, "[PercentAreaOfErosionThresholded]", Math.Round(MaskStat.AreaErosion_Thresholded / _TotalStats.AreaErosion_Thresholded, 2))

                ReplaceTag(OutputText, "[PercentAreaOfDepositionThresholded]", Math.Round(MaskStat.AreaDeposition_Thresholded / _TotalStats.AreaDeposition_Thresholded, 2))

                ReplaceTag(OutputText, "[PercentVolumeOfErosionThresholded]", Math.Round(MaskStat.VolumeErosion_Thresholded / _TotalStats.VolumeErosion_Thresholded, 2))

                ReplaceTag(OutputText, "[PercentVolumeOfDepositionThresholded]", Math.Round(MaskStat.VolumeDeposition_Thresholded / _TotalStats.VolumeDeposition_Thresholded, 2))

                ReplaceTag(OutputText, "[PercentThresholdedVolumeOfDifference]", Math.Round(MaskStat.VolumeOfDifference_Thresholded / _TotalStats.VolumeOfDifference_Thresholded, 2))

                ReplaceTag(OutputText, "[PercentThresholdedNetVolumeDifference]", Math.Round(MaskStat.NetVolumeOfDifference_Thresholded / _TotalStats.NetVolumeOfDifference_Thresholded, 2))

                'percentages
                ReplaceTag(OutputText, "[ThresholdedPercentErosion]", Math.Round(MaskStat.PercentErosion_Thresholded, 2))

                ReplaceTag(OutputText, "[ThresholdedPercentDeposition]", Math.Round(MaskStat.PercentDeposition_Thresholded, 2))

                ReplaceTag(OutputText, "[ThresholdedPercentImbalance]", Math.Round(MaskStat.PercentImbalance_Thresholded, 2))

            Next
            '
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Remove the repeat tags
            '
            OutputText.Replace("[Repeat]", "")
            OutputText.Replace("[/Repeat]", "")
            '
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Create the acutal file, then stream the content at the file and close it out.
            '
            If Not String.IsNullOrEmpty(sClassSummaryPath) Then
                'Dim sClassSummaryFile As String = GISCode.FileSystem.RemoveDangerousCharacters("ClassSummary")
                'sClassSummaryFile = IO.Path.Combine(IO.Path.GetDirectoryName(sClassSummaryPath), sClassSummaryFile)
                'sClassSummaryFile = IO.Path.ChangeExtension(sClassSummaryFile, "xml")

                Dim objWriter As New System.IO.StreamWriter(sClassSummaryPath)
                objWriter.Write(OutputText)
                objWriter.Close()
            End If
        End Sub

        ''' <summary>
        ''' Replaces the text between "repeat" tags in the class summary XML and duplicates the contents of the tag for the number of mask classes.
        ''' </summary>
        ''' <param name="sText">The template XML that will be replaced</param>
        ''' <param name="nTimes">The number of mask classes</param>
        ''' <returns></returns>
        ''' <remarks>Used for the class summary XML file.</remarks>
        Private Function DuplicateRepeatStatements(sText As StringBuilder, nTimes As Integer) As StringBuilder

            Dim sStartTag As String = "[Repeat]"
            Dim sEndTag As String = "[/Repeat]"
            Dim sRepeatText As String
            Dim nRepeatLength As Integer ' including repeat tags
            Dim nStartIndex As Integer = 0
            Dim nEndIndex As Integer = nStartIndex + 1 + sStartTag.Length

            nStartIndex = sText.ToString.IndexOf(sStartTag)
            Do While nStartIndex > 0
                nEndIndex = sText.ToString.IndexOf(sEndTag, nStartIndex)
                nRepeatLength = (nEndIndex + sEndTag.Length) - nStartIndex ' the full length of the string being replaced
                sRepeatText = sText.ToString.Substring(nStartIndex + sStartTag.Length, nEndIndex - (nStartIndex + sStartTag.Length))
                For i As Integer = 0 To nTimes - 2 ' there is already one copy of the text in the file
                    sText = sText.Insert(nStartIndex + sStartTag.Length + 1, sRepeatText)
                Next

                nStartIndex = sText.ToString.IndexOf(sStartTag, nStartIndex + 1)
            Loop

            Return sText
        End Function

        Private Sub ExportMaskStat(sExcelTemplateFolder As String, ByVal StatsData As ChangeDetection.StatsDataClass, ByVal SummaryPath As String, sDisplayUnits As String)

            ' PGB 15 Jan 2014. Each class now gets its own copy of the new GCD SummaryXML file.
            StatsData.ExportSummary(sDisplayUnits, sDisplayUnits, SummaryPath)
        End Sub

        Private Sub WriteCell(ByRef Template As StringBuilder, ByVal tag As String, ByVal value As Double, Optional ByVal format As String = "")
            Dim CellText As String

            'dont write cell if value is NaN, e.g. from divide by zero
            If Double.IsNaN(value) Then
                CellText = "<Data ss:Type=""String""></Data>"
            Else
                'get cell text
                CellText = "<Data ss:Type=""Number"">" & value & "</Data>"
            End If

            'replace tag with cell text
            Template.Replace(tag, CellText)
        End Sub

        ''' <summary>
        ''' Replaces the first occurance of the argument tag with the argument value
        ''' </summary>
        ''' <param name="Template">Template XML file for output as string</param>
        ''' <param name="tag">Name of the tag to be replaced</param>
        ''' <param name="value">Value to be inserted in the place of the tag</param>
        ''' <param name="format"></param>
        ''' <remarks>PGB 26 Apr 2012. Instead of replacing all occurances of a tag -as done by WriteCell() - 
        ''' this method replaces just the first occurrance. This is used in the class summary XML where each
        ''' tag is duplicated for each mask class.</remarks>
        Private Sub ReplaceTag(ByRef Template As StringBuilder, tag As String, value As Double, Optional format As String = "")

            Dim CellText As String

            'dont write cell if value is NaN, e.g. from divide by zero
            If Double.IsNaN(value) Then
                CellText = "<Data ss:Type=""String""></Data>"
            Else
                'get cell text
                CellText = "<Data ss:Type=""Number"">" & value & "</Data>"
            End If

            Dim nIndex As Integer = Template.ToString.IndexOf(tag)
            If nIndex >= 0 Then
                Template.Replace(tag, CellText, nIndex, tag.Length)
            End If
        End Sub
    End Class
End Namespace