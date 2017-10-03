Namespace GISCode.GCD

    Public Class GCDOptions
        Public Shared ReadOnly Property outputDriver As String
            Get
                Return "GTiff"
            End Get
        End Property

        Public Shared ReadOnly Property RasterExtension As String
            Get
                Return ".tif"
            End Get
        End Property

        Public Shared ReadOnly Property NoData As Integer
            Get
                Return -9999
            End Get
        End Property

    End Class

End Namespace