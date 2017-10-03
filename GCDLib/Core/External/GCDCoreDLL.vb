Namespace Core.External

    Public Module GCDCore

        Public Enum GCDCoreOutputCodes
            PROCESS_OK = 0
        End Enum

        ''' <summary>
        ''' Retrieve the file version information for the required DLL
        ''' </summary>
        ''' <param name="bFileIsRequired">If true and the DLL file is missing then an exception is thrown</param>
        ''' <returns></returns>
        ''' <remarks>The reason for the bFileIsRequired argument is that this method is called in
        ''' a couple of different contexts. When used in the About form of applications the file
        ''' must always exist, so no argument is needed. i.e. applications that don't use this DLL
        ''' won't have a label that needs the version number. But this method is also called from
        ''' within the exception handling classes (HandleExceptionUI and HandleExceptionConsole)
        ''' in which case these methods might call this method for a DLL that is not part </remarks>
        Public Function GetFileVersion(Optional bFileIsRequired As Boolean = True) As System.Diagnostics.FileVersionInfo

            Dim sPath As String = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), ExternalLibs.m_sGCDDLLFileName)
            Dim theFileVersionInfo As System.Diagnostics.FileVersionInfo = Nothing
            Try
                If System.IO.File.Exists(sPath) Then
                    theFileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(sPath)
                ElseIf bFileIsRequired Then
                    Throw New Exception("The required DLL does not exist.")
                End If
            Catch ex As Exception
                Dim ex2 As New Exception("Error retrieving the file version for supporting DLL.", ex)
                ex.Data("DLL File") = sPath
                Throw ex
            End Try

            Return theFileVersionInfo

        End Function

        ''' <summary>
        ''' Calculate DoD raw and thresholded statistics from a MinLoD uncertainty analysis
        ''' </summary>
        ''' <param name="sNewDEM">New DEM Raster Path</param>
        ''' <param name="sOldDEM">Old DEM Raster Path</param>
        ''' <param name="sOutputRawDoDPath">Raw DoD Raster Path</param>
        ''' <param name="fNoData">Output Raster NoData value</param>
        ''' <param name="sGDALDriver">Output raster GDAL Driver (TIFF or IMG)</param>
        ''' <returns>The return code of the operation</returns>
        ''' <remarks>Calls into GCD Core C++ DLL.</remarks>
        <Runtime.InteropServices.DllImport(ExternalLibs.m_sGCDDLLFileName)>
        Public Function DoDRaw(sNewDEM As String, sOldDEM As String, sOutputRawDoDPath As String, sGDALDriver As String, fNoData As Double, sError As System.Text.StringBuilder) As Integer
        End Function

        ''' <summary>
        ''' Calculate the histogram for a DoD and write them to a CSV file path
        ''' </summary>
        ''' <param name="sDoDPath">DoD Raster Path</param>
        ''' <param name="nNumBins">Output: Total Number of Bins</param>
        ''' <param name="nMinimumBin">Output: Minimum Bin</param>
        ''' <param name="fBinSize">Output: Bin size</param>
        ''' <param name="fBinIncrement">Output: Bin Increment</param>
        ''' <returns>The return code of the operation</returns>
        ''' <remarks>Use this version of the method when you want to retrieve the attributes of the histogram (bin size etc)</remarks>
        <Runtime.InteropServices.DllImport(ExternalLibs.m_sGCDDLLFileName)>
        Public Function CalculateAndWriteDoDHistogramWithBins(sDoDPath As String, sHistogramPath As String, ByRef nNumBins As Integer,
                                                              ByRef nMinimumBin As Integer, ByRef fBinSize As Double, ByRef fBinIncrement As Double, sError As System.Text.StringBuilder) As Integer
        End Function

        ''' <summary>
        ''' Calculate the histogram for a DoD using specified bin attributes and then write it to a CSV file path
        ''' </summary>
        ''' <param name="sDoDPath">DoD Raster Path</param>
        ''' <param name="nNumBins">Total Number of Bins</param>
        ''' <param name="nMinimumBin">Minimum Bin</param>
        ''' <param name="fBinSize">Bin size</param>
        ''' <param name="fBinIncrement">Bin Increment</param>
        ''' <returns>The return code of the operation</returns>
        ''' <remarks>Use this method when you want to provide the bin attributes (size, increment etc) and force the
        ''' histogram to use these specified attributes. This version is intended for probabilistic thresholding.</remarks>
        <Runtime.InteropServices.DllImport(ExternalLibs.m_sGCDDLLFileName)>
        Public Function CalculateAndWriteDoDHistogramWithSpecifiedBins(sDoDPath As String, sHistogramPath As String,
                                                                       nNumBins As Integer, nMinimumBin As Integer, fBinSize As Double, fBinIncrement As Double, sError As System.Text.StringBuilder) As Integer
        End Function

        ''' <summary>
        ''' Calculate the histogram for a DoD and write them to a CSV file path
        ''' </summary>
        ''' <param name="sDoDPath">DoD Raster Path</param>
        ''' <returns>The return code of the operation</returns>
        ''' <remarks>Use this version of the method when you don't need to retrieve the attributes of the histogram (bin size etc)</remarks>
        <Runtime.InteropServices.DllImport(ExternalLibs.m_sGCDDLLFileName)>
        Public Function CalculateAndWriteDoDHistogram(sDoDPath As String, sHistogramPath As String, sError As System.Text.StringBuilder) As Integer
        End Function

        <Runtime.InteropServices.DllImport(ExternalLibs.m_sGCDDLLFileName)>
        Public Function ThresholdDoDMinLoD(sDoDPath As String, sOutputThresholdDoDPath As String, fThreshold As Double, sGDALDriver As String, fNoData As Double, sError As System.Text.StringBuilder) As Integer
        End Function

        ''' <summary>
        ''' Threshold a raw DoD raster using a propagated error raster
        ''' </summary>
        ''' <param name="sDoDPath">Full absolute path to an existing raw DoD raster</param>
        ''' <param name="sPropagatedErrorRaster">Full, absolute path to an existing propagated error raster</param>
        ''' <param name="sOutputThresholdDoDPath">Full absolute path to the thresholded DoD raster that will get created by this method</param>
        ''' <returns>The return code of the operation</returns>
        ''' <remarks></remarks>
        <Runtime.InteropServices.DllImport(ExternalLibs.m_sGCDDLLFileName)>
        Public Function ThresholdDoDPropErr(sDoDPath As String, sPropagatedErrorRaster As String, sOutputThresholdDoDPath As String, sError As System.Text.StringBuilder) As Integer
        End Function

        ''' <summary>
        ''' Create a prior probability raster for an existing raw DoD
        ''' </summary>
        ''' <param name="sDoDPath">Full, absolute path to an existing raw DoD raster</param>
        ''' <param name="sNewError">Full, absolute path to an existing error surface raster for the new survey</param>
        ''' <param name="sOldError">Full, absolute path to an existing error surface raster for the new survey</param>
        ''' <param name="sPriorProbRaster">Full, absolute path to the prior probabsiliy raster that will get created by this method</param>
        ''' <param name="fNoData">Output Raster NoData value</param>
        ''' <param name="sGDALDriver">Output raster GDAL Driver (TIFF or IMG)</param>
        ''' <returns>The return code of the operation</returns>
        ''' <remarks>Calls into GCD Core C++ DLL.</remarks>
        <Runtime.InteropServices.DllImport(ExternalLibs.m_sGCDDLLFileName)>
        Public Function CreatePriorProbabilityRaster(sDoDPath As String, sNewError As String, sOldError As String, sPriorProbRaster As String,
                                                     sGDALDriver As String, fNoData As Double, sError As System.Text.StringBuilder) As Integer
        End Function

        ''' <summary>
        ''' Threshold a raw DoD using probabilistic thresholding and using spatial coherence
        ''' </summary>
        ''' <param name="sRawDoDPath">Full, absolute path to an existing raw DoD raster</param>
        ''' <param name="sThresholdDoDPath">Full, absolute path to the thresholded raster that will be created by this method</param>
        ''' <param name="sNewError">Full, absolute path to the existing error surface raster for the new survey</param>
        ''' <param name="sOldError">Full, absolute path to the existing error surface raster for the old survey</param>
        ''' <param name="sPriorProbRaster">Full, absolute raster path to the prior proability raster that will be created by this method</param>
        ''' <param name="sPostProb">Full, absolute raster path to the postior probability raster that will be created by this method</param>
        ''' <param name="sConditional">Full, absolute raster path to the conditional raster that will be created by this method</param>
        ''' <param name="sSpatCoErosion">Full, absolute raster path to the EROSION spatial coherence raster that will be created by this method</param>
        ''' <param name="sSpatCoDeposition">Full, absolute raster path to the DEPOSITON spatial coherence raster that will be created by this method</param>
        ''' <param name="fNoData">Output Raster NoData value</param>
        ''' <param name="sGDALDriver">Output raster GDAL Driver (TIFF or IMG)</param>
        ''' <param name="nWinWidth">Moving window width for spatial coherence</param>
        ''' <param name="nWinHeight">Moving window height for spatial coherence</param>
        ''' <param name="fThreshold">Confidence interval threshold (percent)</param>
        ''' <returns>The return code of the operation</returns>
        ''' <remarks>Call this method to perform probabilistic thresholding on an existing raw DoD, using spatial coherence.</remarks>
        <Runtime.InteropServices.DllImport(ExternalLibs.m_sGCDDLLFileName)>
        Public Function ThresholdDoDProbWithSpatialCoherence(sRawDoDPath As String, sThresholdDoDPath As String, sNewError As String, sOldError As String,
                                                                sPriorProbRaster As String, sPostProb As String, sConditional As String, sSpatCoErosion As String, sSpatCoDeposition As String,
                                                                sGDALDriver As String, fNoData As Double, nWinWidth As Integer, nWinHeight As Integer, fThreshold As Double, sError As System.Text.StringBuilder) As Integer
        End Function

        ''' <summary>
        ''' Threshold a raw DoD using probabilistic thresholding (NOT using spatial coherence)
        ''' </summary>
        ''' <param name="sRawDoDPath">Full, absolute path to an existing raw DoD raster</param>
        ''' <param name="sThresholdDoDPath">Full, absolute path to the thresholded raster that will be created by this method</param>
        ''' <param name="sNewError">Full, absolute path to the existing error surface raster for the new survey</param>
        ''' <param name="sOldError">Full, absolute path to the existing error surface raster for the old survey</param>
        ''' <param name="sPriorProbRaster">Full, absolute raster path to the prior proability raster that will be created by this method</param>
        ''' <param name="fNoData">Output Raster NoData value</param>
        ''' <param name="sGDALDriver">Output raster GDAL Driver (TIFF or IMG)</param>
        ''' <param name="fThreshold">Confidence interval threshold (percent)</param>
        ''' <returns>The return code of the operation</returns>
        ''' <remarks>Call this method to perform probabilistic thresholding on an existing raw DoD. No spatial coherence is used.</remarks>
        <Runtime.InteropServices.DllImport(ExternalLibs.m_sGCDDLLFileName)>
        Public Function ThresholdDoDProbability(sRawDoDPath As String, sThresholdDoDPath As String, sNewError As String, sOldError As String,
                                                                sPriorProbRaster As String, sGDALDriver As String, fNoData As Double, fThreshold As Double, sError As System.Text.StringBuilder) As Integer
        End Function

        ''' <summary>
        ''' Create a FIS error surface
        ''' </summary>
        ''' <param name="sDEMPath">Absolute, full path to existing DEM error surface</param>
        ''' <param name="sFISRuleFilePath">Absolute, full path to existing FIS rule file (*.fis)</param>
        ''' <param name="sInputs">FIS inputs concatenated as a string with semicolon delimeter (slope;C:\MySlope.tif;PointDensity;C:\MyDensity.tif)</param>
        ''' <param name="sOutputErrorSurface">Output error surface to be created by this method</param>
        ''' <param name="sGDALDriver">Output raster GDAL Driver (TIFF or IMG)</param>
        ''' <returns>The return code of the operation</returns>
        ''' <remarks></remarks>
        <Runtime.InteropServices.DllImport(ExternalLibs.m_sGCDDLLFileName)>
        Public Function CreateFISError(sDEMPath As String, sFISRuleFilePath As String, sInputs As String, sOutputErrorSurface As String, sGDALDriver As String, fNoData As Double, sError As System.Text.StringBuilder) As Integer
        End Function

        ''' <summary>
        ''' Loop over the classes in a budget segregation and write their histogram CSV files.
        ''' </summary>
        ''' <param name="sDoDPath"></param>
        ''' <param name="sSegregationRaster"></param>
        ''' <param name="sMaskValues"></param>
        ''' <param name="sMaskCSVPathList"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Runtime.InteropServices.DllImport(ExternalLibs.m_sGCDDLLFileName)>
        Public Function CalculateAndWriteMaskHistograms(sDoDPath As String, sSegregationRaster As String, sMaskValues As String, sMaskCSVPathList As String, sError As System.Text.StringBuilder) As Integer
        End Function

        ''' <summary>
        ''' Generate a uniform error raster 
        ''' </summary>
        ''' <param name="sDEM">Full, absolute path to existing DEM raster.</param>
        ''' <param name="sOutput">Output: The full, absolute path of the uniform error raster that this method will produce.</param>
        ''' <param name="fErrorValue">The error value to use in the output raster.</param>
        ''' <returns>GCD Core return code.</returns>
        ''' <remarks>Note that the error raster has the same extent and spatial reference as the DEM but possesses the constant
        ''' uniform argument value in all Non-NoData values of the DEM.</remarks>
        <Runtime.InteropServices.DllImport(ExternalLibs.m_sGCDDLLFileName)>
        Public Function UniformError(sDEM As String, sOutput As String, fErrorValue As Double, sError As System.Text.StringBuilder) As Integer
        End Function

        ''' <summary>
        ''' Build raster pyramids for the specified raster.
        ''' </summary>
        ''' <param name="sRasterPath">Full, absolute raster path</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Runtime.InteropServices.DllImport(ExternalLibs.m_sGCDDLLFileName)>
        Public Function BuildPyramids(sRasterPath As String, sError As System.Text.StringBuilder) As Integer
        End Function

    End Module

End Namespace