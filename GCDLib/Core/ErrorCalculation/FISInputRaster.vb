Namespace GISCode.GCD.ErrorCalculation

    Public Enum AssociatedSurfaceTypes
        PointDensity
        Slope
        PointQuality3D
        InerpolationError
        Roughness
    End Enum

    Public Class FISInputRaster

        Private m_sRasterPath As String
        Private m_eAssociatedSurfaceType As AssociatedSurfaceTypes
        Private m_sAssociatedSurfaceType As String
        Private m_sFISInputName As String

#Region "Properties"

        Public ReadOnly Property RasterPath As String
            Get
                Return m_sRasterPath
            End Get
        End Property

        Public ReadOnly Property AssociatedSurfaceType As AssociatedSurfaceTypes
            Get
                Return m_eAssociatedSurfaceType
            End Get
        End Property

        Public ReadOnly Property AssociatedSurfaceTypeAsString As String
            Get
                Return m_sAssociatedSurfaceType
            End Get
        End Property

        Public ReadOnly Property FISInputName As String
            Get
                Return m_sFISInputName
            End Get
        End Property

#End Region

        Public Sub New(sRasterPath As String, eAssociatedSurfaceType As AssociatedSurfaceTypes)

            m_sRasterPath = sRasterPath
            m_eAssociatedSurfaceType = eAssociatedSurfaceType
         
            Select Case m_eAssociatedSurfaceType
                Case AssociatedSurfaceTypes.InerpolationError
                    m_sFISInputName = "InterpolationError"
                    m_sAssociatedSurfaceType = "Interpolation Error"

                Case AssociatedSurfaceTypes.PointDensity
                    m_sFISInputName = "PointDensity"
                    m_sAssociatedSurfaceType = "Point Density"

                Case AssociatedSurfaceTypes.PointQuality3D
                    m_sFISInputName = "3DPointQuality"
                    m_sAssociatedSurfaceType = "3D Point Quality"

                Case AssociatedSurfaceTypes.Roughness
                    m_sFISInputName = "Roughness"
                    m_sAssociatedSurfaceType = "Roughness"

                Case AssociatedSurfaceTypes.Slope
                    m_sFISInputName = "Slope"
                    m_sAssociatedSurfaceType = "Slope Degrees"

                Case Else
                    Throw New Exception(String.Format("Unhandled associated surface type {0}", m_eAssociatedSurfaceType.ToString))
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Return String.Format("{0} FIS Input = {1}", m_sFISInputName, m_sRasterPath)
        End Function

    End Class

End Namespace