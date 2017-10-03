Imports ESRI.ArcGIS.Geodatabase

Namespace GISCode.GCD

    ''' <summary>
    ''' A raster representing a water surface
    ''' </summary>
    ''' <remarks>Typically a constant raster, but could vary such as a CHaMP water surface
    ''' DEM. See bottom of file for an inherited class specifically designed for constant
    ''' water surfaces</remarks>
    Public Class WaterSurface

#Region "Members"
        Private m_sRasterPath As String ' Full, absolute raster path
        Private m_sName As String ' User interface name
#End Region

#Region "Properties"

        ''' <summary>
        ''' Full, absolute raster file path
        ''' </summary>
        ''' <value></value>
        ''' <returns>Full, absolute raster file path</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Source As String
            Get
                Return m_sRasterPath
            End Get
        End Property

        ''' <summary>
        ''' Name of the water surface
        ''' </summary>
        ''' <value></value>
        ''' <returns>Name used in the user interface</returns>
        ''' <remarks>Should be unique within a GCD project</remarks>
        Public ReadOnly Property Name As String
            Get
                Return m_sName
            End Get
        End Property
#End Region

#Region "Constructors"

        ''' <summary>
        ''' Create a new water surface DEM
        ''' </summary>
        ''' <param name="sRasterPath">Full absolute raster path</param>
        ''' <param name="sName">User interface name of the raster</param>
        ''' <remarks></remarks>
        Public Sub New(sRasterPath As String, sName As String)

            If String.IsNullOrEmpty(sRasterPath) Then
                Throw New ArgumentNullException("sRasterPath", "The raster path cannot be null or empty")
            End If

            If Not GISDataStructures.Raster.Exists(sRasterPath) Then
                Dim ex As New Exception("The raster does not exist. The water surface raster must already exist.")
                ex.Data("Raster Path") = sRasterPath
                Throw ex
            End If
            m_sRasterPath = sRasterPath
            m_sName = sName
        End Sub

#End Region

    End Class

    ''' <summary>
    ''' Constant water surface (same value across entire surface)
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ConstantWaterSurface
        Inherits WaterSurface

#Region "Members"
        Private m_fElevation As Double ' The floating point elevation of the surface.
#End Region

#Region "Properties"

        ''' <summary>
        ''' The elevation of the water surface
        ''' </summary>
        ''' <value></value>
        ''' <returns>The elevation of the water surface</returns>
        ''' <remarks>Note that this comes from the GCD project file, not by interogating
        ''' the raster data to obtain the value.</remarks>
        Public ReadOnly Property Elevation As Double
            Get
                Return m_fElevation
            End Get
        End Property
#End Region

#Region "Constructors"
        Public Sub New(sRasterPath As String, sName As String, fElevation As String)
            MyBase.New(sRasterPath, sName)

            If fElevation < 0 Then
                Throw New ArgumentOutOfRangeException("fElevation", "The elevation must be greater than or equal to zero")
            End If
            m_fElevation = fElevation
        End Sub
#End Region

#Region "Methods"

        ''' <summary>
        ''' Creates and returns a new constant water surface raster
        ''' </summary>
        ''' <param name="gExtentRaster">DEM survey that will be used for the extent</param>
        ''' <param name="sOutputRasterPath">Output path where the create the constant raster</param>
        ''' <param name="fElevation">Elevation of the constant raster</param>
        ''' <param name="sname">Name for the user interface</param>
        ''' <returns>Constant Water surface object</returns>
        ''' <remarks></remarks>
        Public Shared Function Calculate(gExtentRaster As GISDataStructures.Raster, sOutputRasterPath As String, fElevation As Double, Optional sname As String = "") As ConstantWaterSurface

            Dim ws As ConstantWaterSurface = Nothing
            Try
                GISDataStructures.Raster.CreateConstantRaster(fElevation, gExtentRaster, sOutputRasterPath)
                ws = New ConstantWaterSurface(sOutputRasterPath, sname, fElevation)
            Catch ex As Exception
                Dim ex2 As New Exception("Error calculating constant water surface raster", ex)
                ex2.Data.Add("Extent raster", gExtentRaster.FullPath)
                ex2.Data.Add("Output raster", sOutputRasterPath)
                ex2.Data.Add("Elevation", fElevation)
                ex2.Data.Add("Name", sname)
                Throw ex2
            End Try

            Return ws

        End Function

#End Region

    End Class

End Namespace