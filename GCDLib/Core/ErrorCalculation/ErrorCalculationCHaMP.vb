#Region "Code Comments"
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'       Author: Philip Bailey, Nick Ochoski, & Frank Poulsen
'               ESSA Software Ltd.
'               1765 W 8th Avenue
'               Vancouver, BC, Canada V6J 5C6
'     
'     Copyright: (C) 2011 by ESSA technologies Ltd.
'                This software is subject to copyright protection under the       
'                laws of Canada and other countries.
'
'  Date Created: 14 January 2011
'
'   Description: 
'
#End Region

#Region "Imports"

Imports System.IO
'Imports OSGeo.GDAL

Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry

#End Region

Namespace GISCode.GCD.ErrorCalculation

    Public Module ErrorCalculationCHaMP

        ''' <summary>
        ''' Create an error grid using fuzzy inference system.
        ''' </summary>
        ''' <param name="inFISRuleDefinitionFile">FIS rule definition file (*.fis)</param>
        ''' <param name="inInputLayerPaths">Dictionary of FIS rule definitions. Key is FIS input, value is full path to GIS layer.</param>
        ''' <param name="outErrorRaster">Desired name of the resultant error grid</param>
        ''' <remarks>All FIS rules must have a corresponding layer defined in the inInputLayerPaths dictionary or an error is thrown.</remarks>
        Public Sub RBTCreateFISError(ByVal inFISRuleDefinitionFile As String, ByVal inInputLayerPaths As Dictionary(Of String, String), ByVal outErrorRaster As String)
            '
            ' Register the GDAL backend
            '
            'Gdal.AllRegister()
            '
            ' Load the FIS rules from the FIS file
            '
            Dim fisRules As New FISRuleSet()
            fisRules.parseFile(inFISRuleDefinitionFile)

            RBTCreateFISError(fisRules, inInputLayerPaths, outErrorRaster)
            fisRules.Dispose()
            fisRules = Nothing
            GC.Collect()
            'Gdal.GDALDestroyDriverManager()
            'GC.Collect()

        End Sub

        ''' <summary>
        ''' Create an error grid using fuzzy inference system.
        ''' </summary>
        ''' <param name="sFISRulefile">FIS rule definitions</param>
        ''' <param name="inInputLayerPaths">Dictionary of FIS rule definitions. Key is FIS input, value is full path to GIS layer.</param>
        ''' <param name="outErrorRaster">Desired name of the resultant error grid</param>
        ''' <remarks>All FIS rules must have a corresponding layer defined in the inInputLayerPaths dictionary or an error is thrown.</remarks>
        Public Sub RBTCreateFISError(sFISRulefile As String, ByVal inInputLayerPaths As Dictionary(Of String, String), ByVal outErrorRaster As String)
            '
            ' Register the GDAL backend
            '
            'Gdal.AllRegister()

            If Not TypeOf inFISRules Is FISRuleSet Then
                Throw New ArgumentNullException("The FIS rule set is not valid")
            End If
            '
            ' Loop through all the FIS rules defined in the FIS file. Each rule needs an input raster
            ' specified in the input layer dictionary. Any failure to load an input or if an input
            ' is not specified causes an error to be thrown.
            '
            Dim ReferenceLayer As String = ""
            Dim FISDataSourceList As New DataSourceListClass()
            For i As Integer = 0 To inFISRules.numInputs - 1
                If inInputLayerPaths.ContainsKey(inFISRules.getInputName(i)) Then
                    If FISDataSourceList.set(inFISRules.getInputName(i), inInputLayerPaths(inFISRules.getInputName(i))) Then
                        ReferenceLayer = inInputLayerPaths(inFISRules.getInputName(i))
                    Else
                        Dim ex As New Exception("Failed to add FIS input layer")
                        ex.Data.Add("FIS Rule Name", inFISRules.getInputName(i))
                        ex.Data.Add("Input Source Path", inInputLayerPaths(inFISRules.getInputName(i)))
                        Throw ex
                    End If
                Else
                    Dim ex As New Exception("FIS input layer unfedined")
                    ex.Data.Add("FIS Rule Name", inFISRules.getInputName(i))
                    Throw ex
                End If
            Next
            '
            ' GDAL cannot handle file geodatabases. If the final output location is a file geodatabase then create a temporary copy 
            ' of the error grid in the TempWorkspace then copy it to the final location.
            '
            Dim sTempOutputRaster As String = outErrorRaster
            If GISDataStructures.IsFileGeodatabase(outErrorRaster) Then
                sTempOutputRaster = WorkspaceManager.GetTempRaster("error.tif")
            End If

            Try
                '
                ' Call the backend GCD FIS class
                '
                Dim oldFIS As FISClass = New FISClass(sTempOutputRaster, "GTiff", -9999, ReferenceLayer, inFISRules, FISDataSourceList)
                oldFIS.Dispose()
                oldFIS = Nothing

            Catch ex As Exception

                If TypeOf inInputLayerPaths Is Dictionary(Of String, String) Then
                    For Each sKey As String In inInputLayerPaths.Keys
                        ex.Data.Add("FIS input: " & sKey, inInputLayerPaths(sKey))
                    Next
                End If

                ex.Data.Add("outErrorRaster", outErrorRaster)
                Throw ex
            Finally
                FISDataSourceList.Dispose()
                FISDataSourceList = Nothing
            End Try
            '
            ' Copy the error grid to the desired final location.
            '
            If GISCode.GISDataStructures.IsFileGeodatabase(outErrorRaster) Then
                GISCode.GP.DataManagement.CopyRaster(sTempOutputRaster, outErrorRaster)
            End If

            GC.Collect()

        End Sub

    End Module

End Namespace