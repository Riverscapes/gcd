Namespace Core

    Public Class RasterPyramidManager

        Private Const m_sListDelimeter As String = ","
        Private Const m_sKeyValueDelimeter As String = "="

        Public Enum PyramidRasterTypes
            DEM
            Hillshade
            AssociatedSurfaces
            ErrorSurfaces
            DoDRaw
            DoDThresholded
            PropagatedError
            ProbabilityRasters
        End Enum

        Private Shared m_dAutomaticallyBuildPyramids As Dictionary(Of PyramidRasterTypes, Boolean)

        ''' <summary>
        ''' Use this constructor for non-user interface applications
        ''' All rasters will default to NO pyramids
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

            ' There's no need to actually add each raster type here because the default return
            ' value from AutomaticallyBuildPyramids below should be False.
            m_dAutomaticallyBuildPyramids = New Dictionary(Of PyramidRasterTypes, Boolean)

        End Sub

        ''' <summary>
        ''' Use this constructor for user interface applications
        ''' </summary>
        ''' <param name="sPyramidString">Pass in the concatenated pyramid settings string, typically from the software products My.Settings</param>
        ''' <remarks></remarks>
        Public Sub New(sPyramidString As String)

            m_dAutomaticallyBuildPyramids = New Dictionary(Of PyramidRasterTypes, Boolean)
            ' If there is no pyramid string, or something when wrong parsing it.
            ' If bUseDefaults Then
            m_dAutomaticallyBuildPyramids.Add(PyramidRasterTypes.DEM, True)
            m_dAutomaticallyBuildPyramids.Add(PyramidRasterTypes.Hillshade, True)
            m_dAutomaticallyBuildPyramids.Add(PyramidRasterTypes.AssociatedSurfaces, True)
            m_dAutomaticallyBuildPyramids.Add(PyramidRasterTypes.ErrorSurfaces, True)
            m_dAutomaticallyBuildPyramids.Add(PyramidRasterTypes.DoDRaw, False)
            m_dAutomaticallyBuildPyramids.Add(PyramidRasterTypes.DoDThresholded, True)
            m_dAutomaticallyBuildPyramids.Add(PyramidRasterTypes.PropagatedError, False)

            If Not String.IsNullOrEmpty(sPyramidString) Then

                ' Loop over all the known raster types and try to retrieve they value from the argument string
                For Each sKeyValuePair As String In sPyramidString.Split(m_sListDelimeter)

                    ' Split the key and value using equal sign
                    Dim sKeyAndValue() As String = sKeyValuePair.Split(m_sKeyValueDelimeter)
                    If sKeyAndValue.Count = 2 Then

                        ' Retrieve the raster type and check that its not empty and represents one of the raster types
                        Dim sKey As String = sKeyAndValue(0)
                        If Not String.IsNullOrEmpty(sKey) Then
                            If [Enum].IsDefined(GetType(PyramidRasterTypes), sKey) Then
                                Dim eRasterType As PyramidRasterTypes = [Enum].Parse(GetType(PyramidRasterTypes), sKey)

                                ' check that the argument string contains a value and that is not null and also a boolean
                                Dim sValue As String = sKeyAndValue(1)
                                If Not String.IsNullOrEmpty(sValue) Then
                                    Dim bValue As Boolean = False
                                    If Boolean.TryParse(sValue, bValue) Then

                                        ' Update the pyramid raster setting based on the retrieved value
                                        m_dAutomaticallyBuildPyramids(eRasterType) = bValue
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next
            End If

        End Sub

        Public Shared Function AutomaticallyBuildPyramids(eRasterType As PyramidRasterTypes) As Boolean

            If m_dAutomaticallyBuildPyramids.ContainsKey(eRasterType) Then
                Return m_dAutomaticallyBuildPyramids(eRasterType)
            Else
                ' The default is to not build pyramids
                Return False
            End If

        End Function

        Public Shared Sub SetAutomaticPyramidsForRasterType(eRasterType As PyramidRasterTypes, bBuildPyramids As Boolean)

            If m_dAutomaticallyBuildPyramids.ContainsKey(eRasterType) Then
                m_dAutomaticallyBuildPyramids(eRasterType) = bBuildPyramids
            Else
                m_dAutomaticallyBuildPyramids.Add(eRasterType, bBuildPyramids)
            End If

        End Sub

        Public Shared Function GetPyramidSettingString()

            Dim sResult As String = String.Empty
            For Each eRasterType As PyramidRasterTypes In m_dAutomaticallyBuildPyramids.Keys
                sResult &= String.Format("{0}{1}{2}{3}", eRasterType, m_sKeyValueDelimeter, m_dAutomaticallyBuildPyramids(eRasterType), m_sListDelimeter)
            Next

            If Not String.IsNullOrEmpty(sResult) Then
                sResult = sResult.TrimEnd(m_sListDelimeter)
            End If

            Return sResult

        End Function

        Public Shared Function GetRasterTypeName(eRasterType As PyramidRasterTypes) As String

            Select Case eRasterType
                Case PyramidRasterTypes.DEM : Return "DEM"
                Case PyramidRasterTypes.Hillshade : Return "Hillshade"
                Case PyramidRasterTypes.AssociatedSurfaces : Return "Associated Surfaces"
                Case PyramidRasterTypes.ErrorSurfaces : Return "Error Surfaces"
                Case PyramidRasterTypes.DoDRaw : Return "Raw DEM of Difference"
                Case PyramidRasterTypes.DoDThresholded : Return "Thresholded DEM of Difference"
                Case PyramidRasterTypes.PropagatedError : Return "Propagated Error Rasters"
                Case PyramidRasterTypes.ProbabilityRasters : Return "Probability Rasters"
                Case Else
                    Dim ex As New Exception("Unhandled raster type within pyramid manager.")
                    ex.Data("Raster Type") = eRasterType
                    Throw ex
            End Select

        End Function

        Public Shared Sub PerformRasterPyramids(ePyramidRasterType As RasterPyramidManager.PyramidRasterTypes, sRasterPath As String)

            If RasterPyramidManager.AutomaticallyBuildPyramids(ePyramidRasterType) Then
                If GISDataStructures.Raster.Exists(sRasterPath) Then
                    Dim eResult As UInteger = External.GCDCore.BuildPyramids(sRasterPath, GCDProject.ProjectManagerUI.GCDNARCError.ErrorString)
                    If eResult <> External.RasterManager.RasterManagerOutputCodes.PROCESS_OK Then
                        Dim ex As New Exception("Error building raster pyramids. This is a non-essential process and the GCD project is unaffected.")
                        ex.Data("GCDCore Error") = GCDProject.ProjectManagerUI.GCDNARCError.ErrorString
                        ex.Data("Raster") = sRasterPath
                        ex.Data("Return code") = eResult.ToString
                        Throw ex
                    End If
                End If
            End If

        End Sub

    End Class

    ''' <summary>
    ''' Use this class to store raster pyramid types in a checked listbox, like in the GCD options form
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PyramidRasterTypeDisplay

        Private m_eRasterType As RasterPyramidManager.PyramidRasterTypes
        Private m_sName As String

        Public ReadOnly Property RasterType As RasterPyramidManager.PyramidRasterTypes
            Get
                Return m_eRasterType
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return m_sName
        End Function

        Public Sub New(eRasterType As RasterPyramidManager.PyramidRasterTypes)
            m_eRasterType = eRasterType
            m_sName = RasterPyramidManager.GetRasterTypeName(eRasterType)
        End Sub
    End Class

End Namespace