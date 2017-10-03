Public Class HistogramConfigForm

    Private m_sRawHist As String
    Private m_sThreshHist As String
    Private m_sRawDoD As String
    Private m_sThreshDoD As String

    Public Sub New(sRawDoD As String, sThreshDoD As String, sRawHist As String, sThreshHist As String)
        InitializeComponent()

        m_sRawDoD = sRawDoD
        m_sThreshDoD = sThreshDoD
        m_sRawHist = sRawHist
        m_sThreshHist = sThreshHist
    End Sub

    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click

        ' Backup the original raw and thresholded histograms
        backUpFile(m_sRawHist)
        backUpFile(m_sThreshHist)

        Try
            GenerateHistogram(m_sRawDoD, m_sRawHist)
            GenerateHistogram(m_sThreshDoD, m_sThreshHist)
        Catch ex As Exception
            ExceptionUI.HandleException(ex)
            Throw
        End Try

    End Sub

    Private Function BackUpFile(sOriginalFilePAth As String) As String
        Dim sResult As String = IO.Path.GetDirectoryName(sOriginalFilePAth)
        sResult = IO.Path.Combine(sResult, IO.Path.GetFileNameWithoutExtension(sOriginalFilePAth))
        sResult &= "_orig"
        sResult = IO.Path.ChangeExtension(sResult, IO.Path.GetExtension(sOriginalFilePAth))
        IO.File.Copy(sOriginalFilePAth, sResult, True)
        Return sResult
    End Function

    Private Sub GenerateHistogram(sDoDPath As String, sHistPath As String)

        Try
            Dim eResult As GCDCoreOutputCodes
            eResult = GCDCore.CalculateAndWriteDoDHistogramWithSpecifiedBins(sDoDPath, sHistPath, valNumBins.Value, valMinBin.Value, valBinSize.Value, valIncrement.Value, GCD.GCDProject.ProjectManagerUI.GCDNARCError.ErrorString)

            If eResult <> GCDCoreOutputCodes.PROCESS_OK Then
                Throw New Exception(GCD.GCDProject.ProjectManagerUI.GCDNARCError.ErrorString.ToString)
            End If

        Catch ex As Exception
            ExceptionUI.HandleException(ex)
        End Try

    End Sub

End Class