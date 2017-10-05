Namespace Core.External

    Public Module RasterManager

        Public Enum RasterManagerOutputCodes
            PROCESS_OK = 0
        End Enum

        Public Enum SlopeTypes
            Percent
            Degrees
        End Enum

        ''' <summary>
        ''' Retrieve the file version information for the required DLL
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetFileVersion() As System.Diagnostics.FileVersionInfo

            Dim sPath As String = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), ExternalLibs.m_sRasterManagerDLLFileName)
            Dim theFileVersionInfo As System.Diagnostics.FileVersionInfo = Nothing
            Try
                If System.IO.File.Exists(sPath) Then
                    theFileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(sPath)
                Else
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
        ''' Initialize the GDAL drivers. This must be called before any GDAL operations are called.
        ''' </summary>
        ''' <remarks>Call this once that the start of the application. See the corresponding destroy
        ''' command that should be called at the end of the application.</remarks>
        <Runtime.InteropServices.DllImport(ExternalLibs.m_sRasterManagerDLLFileName)>
        Public Sub RegisterGDAL()
        End Sub

        ''' <summary>
        ''' Destroy and close the GDAL drivers. This should be called at the end of the application.
        ''' </summary>
        ''' <remarks>See the corresponding call to initialize the GDAL drivers.</remarks>
        <Runtime.InteropServices.DllImport(ExternalLibs.m_sRasterManagerDLLFileName)>
        Public Sub DestroyGDAL()
        End Sub

        ''' <summary>
        ''' Calculate DoD raw and thresholded statistics from a MinLoD uncertainty analysis
        ''' </summary>
        ''' <remarks>Calls into C++ DLL.</remarks>
        <Runtime.InteropServices.DllImport(ExternalLibs.m_sRasterManagerDLLFileName)>
        Public Function BiLinearResample(ByVal sOriginalPath As String, sDestinationPath As String,
                                    fNewCellSize As Double,
                                    fLeft As Double, fTop As Double,
                                    nRows As Integer, nCols As Integer, sError As System.Text.StringBuilder) As Integer
        End Function

        ''' <summary>
        ''' Calculate DoD raw and thresholded statistics from a MinLoD uncertainty analysis
        ''' </summary>
        ''' <remarks>Calls into C++ DLL.</remarks>
        <Runtime.InteropServices.DllImport(ExternalLibs.m_sRasterManagerDLLFileName)>
        Public Function Copy(ByVal sOriginalPath As String, sDestinationPath As String,
                             fNewCellSize As Double,
                            fLeft As Double, fTop As Double,
                            nRows As Integer, nCols As Integer, sError As System.Text.StringBuilder) As Integer
        End Function


        ''' <summary>
        ''' Copy a raster enforcing the spatial reference and units of the output.
        ''' </summary>
        ''' <param name="sOriginalPath"></param>
        ''' <param name="sDestinationPath"></param>
        ''' <param name="fNewCellSize"></param>
        ''' <param name="fLeft"></param>
        ''' <param name="fTop"></param>
        ''' <param name="nRows"></param>
        ''' <param name="nCols"></param>
        ''' <param name="sSpatialReference"></param>
        ''' <param name="sUnits"></param>
        ''' <param name="sError"></param>
        ''' <returns></returns>
        ''' <remarks>This is very similar to the basic copy function but it will enforce that the
        ''' output raster has the argument spatial reference and units. This should really
        ''' only be used after confirming with the user that the input raster does indeed possess
        ''' the argument spatial reference. It's intended to overcome problems when the input
        ''' raster has a slightly different spatial reference than other (project) rasters but
        ''' does in fact share the same projection.</remarks>
        <Runtime.InteropServices.DllImport(ExternalLibs.m_sRasterManagerDLLFileName)>
        Public Function ExtendedCopy(ByVal sOriginalPath As String, sDestinationPath As String,
                             fNewCellSize As Double,
                            fLeft As Double, fTop As Double,
                            nRows As Integer, nCols As Integer,
                            sSpatialReference As String,
                            sUnits As String,
                            sError As System.Text.StringBuilder) As Integer
        End Function

        ''' <summary>
        ''' Retrieve the primary properties of a file-based raster
        ''' </summary>
        ''' <remarks>Calls into C++ DLL.</remarks>
        <Runtime.InteropServices.DllImport(ExternalLibs.m_sRasterManagerDLLFileName)>
        Public Function GetRasterProperties(ByVal sFullPath As String,
                                    ByRef fCellHeight As Double, ByRef fCellWidth As Double,
                                    ByRef fLeft As Double, ByRef fTop As Double,
                                    ByRef nRows As Integer, ByRef nCols As Integer,
                                    ByRef fNoData As Double, ByRef nHasNoData As Integer,
                                    ByRef nDataType As Integer,
                                    sUnits As System.Text.StringBuilder,
                                    sSpatialReference As System.Text.StringBuilder,
                                    sError As System.Text.StringBuilder) As Integer
        End Function

        ''' <summary>
        ''' Retrieve the error code as a string from an integer
        ''' </summary>
        ''' <remarks>Calls into the C++ DLL.</remarks>
        <Runtime.InteropServices.DllImport(ExternalLibs.m_sRasterManagerDLLFileName)>
        Public Function GetReturnCodeAsString(iErrCode As UInteger, sErrorMessage As System.Text.StringBuilder) As Integer
        End Function


        ''' <summary>
        ''' Calculate the root sum squares of two rasters
        ''' </summary>
        ''' <remarks>Calls into C++ DLL.</remarks>
        <Runtime.InteropServices.DllImport(ExternalLibs.m_sRasterManagerDLLFileName)>
        Public Function RootSumSquares(ByVal psRaster1 As String, ByVal psRaster2 As String, ByVal psOutputRaster As String, sError As System.Text.StringBuilder) As Integer
        End Function

        ''' <summary>
        ''' Mosaic a series of raster together into one
        ''' </summary>
        ''' <param name="sRasters">Semicolon delimited series of input rasters</param>
        ''' <param name="sOutputRaster">Output: The full, absolute path of the raster to be generated by this method.</param>
        ''' <returns>Raster Manager return code</returns>
        ''' <remarks>Note that rasters have presedence in the order in which they are passed. Values from the the first raster
        ''' will be output except where the first raster is NoData, then the second raster will be looked at.</remarks>
        <Runtime.InteropServices.DllImport(ExternalLibs.m_sRasterManagerDLLFileName)>
        Public Function Mosaic(sRasters As String, sOutputRaster As String, sError As System.Text.StringBuilder) As Integer
        End Function

        ''' <summary>
        ''' Set the values in the input raster to NoData where the mask raster has NoData
        ''' </summary>
        ''' <param name="sInputRaster">Input raster that will be masked</param>
        ''' <param name="sMaskRaster">The mask. The values are not used. Only the NoData extent is used.</param>
        ''' <param name="sOutputRaster">Output: the new raster path that will get created</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Runtime.InteropServices.DllImport(ExternalLibs.m_sRasterManagerDLLFileName)>
        Public Function Mask(sInputRaster As String, sMaskRaster As String, sOutputRaster As String, sError As System.Text.StringBuilder) As Integer
        End Function


        ''' <summary>
        ''' Set the value in the input raster to NoData whereever the MaskValue exists
        ''' </summary>
        ''' <param name="sInputRaster">Input raster that will be masked</param>
        ''' <param name="sOutputRaster">Output: the new raster path that will get created</param>
        ''' <param name="fMaskValue">The value to be masked and converted to NoData.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Runtime.InteropServices.DllImport(ExternalLibs.m_sRasterManagerDLLFileName)>
        Public Function MaskValue(sInputRaster As String, sOutputRaster As String, fMaskValue As Double, sError As System.Text.StringBuilder) As Integer
        End Function

        ''' <summary>
        ''' Set all existing data to NoData and replace all NoData values with the argument value
        ''' </summary>
        ''' <param name="sInputRaster">Input raster</param>
        ''' <param name="sOutputRaster">Output: the new raster path that will be created</param>
        ''' <param name="fValue">The value that will be placed in used in the output raster for all NoData cells in the input raster</param>
        ''' <param name="sError"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Runtime.InteropServices.DllImport(ExternalLibs.m_sRasterManagerDLLFileName)>
        Public Function RasterInvert(sInputRaster As String, sOutputRaster As String, fValue As Double, sError As System.Text.StringBuilder) As Integer
        End Function

        ''' <summary>
        ''' Set the values in the input raster to NoData where the mask raster has NoData
        ''' </summary>
        ''' <param name="sRasterSourcePath">Input raster that will be written to CSV</param>
        ''' <param name="sOutputCSVPath">The output CSV file that will be created.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Runtime.InteropServices.DllImport(ExternalLibs.m_sRasterManagerDLLFileName)>
        Public Function RasterToCSV(sRasterSourcePath As String, sOutputCSVPath As String, sError As System.Text.StringBuilder) As Integer
        End Function

        ''' <summary>
        ''' Uses GDAL to determine if two datasets possess matching spatial references
        ''' </summary>
        ''' <param name="sRaster1">First dataset</param>
        ''' <param name="sRaster2">Second dataset</param>
        ''' <param name="nResult">0 = false, does not false. Anything else is true, match.</param>
        ''' <param name="sError"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Runtime.InteropServices.DllImport(ExternalLibs.m_sRasterManagerDLLFileName)>
        Public Function SpatialReferenceMatches(sRaster1 As String, sRaster2 As String, ByRef nResult As Integer, sError As System.Text.StringBuilder) As Integer
        End Function

        Public Function CreateSlope(sInputRaster As String, sOutputSlopeRaster As String, eSlopeType As SlopeTypes, sError As System.Text.StringBuilder) As Integer
            Return CreateSlope(sInputRaster, sOutputSlopeRaster, IIf(eSlopeType = SlopeTypes.Degrees, "degrees", "percent"), sError)
        End Function

        <Runtime.InteropServices.DllImport(ExternalLibs.m_sRasterManagerDLLFileName)>
        Private Function CreateSlope(sInputRaster As String, sOutputSlopeRaster As String, sSlopeType As String, sError As System.Text.StringBuilder) As Integer
        End Function

        <Runtime.InteropServices.DllImport(ExternalLibs.m_sRasterManagerDLLFileName)>
        Public Function CreateHillshade(sInputRaster As String, sOutputHillshadeRaster As String, sError As System.Text.StringBuilder) As Integer
        End Function

        <Runtime.InteropServices.DllImport(ExternalLibs.m_sRasterManagerDLLFileName)>
        Public Function Delete(sDatasetPath As String, sError As System.Text.StringBuilder) As Integer
        End Function

    End Module

End Namespace