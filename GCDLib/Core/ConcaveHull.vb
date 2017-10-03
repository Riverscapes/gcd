Imports System.IO
Imports ESRI.ArcGIS.Geodatabase

Namespace GISCode.GCD

    Public Class ConcaveHull

        Private m_sInputPath As String

        Public Sub New(sInputPath As String)

            If String.IsNullOrEmpty(sInputPath) Then
                Throw New ArgumentNullException("sOutputPath", "Invalid or null argument")
            End If
            m_sInputPath = sInputPath

        End Sub

        ''' <summary>
        ''' Creates concave hull using manually specified distance
        ''' </summary>
        ''' <param name="sOutputPath">Location of the output polygon feature class</param>
        ''' <param name="fDistance">Distance between points at which they are considered part of the same polygon</param>
        ''' <remarks>Use this method when the distance between points is to be specified manually.</remarks>
        Public Sub GenerateConcaveHull(sOutputPath As String, fDistance As Double)

            If String.IsNullOrEmpty(sOutputPath) Then
                Throw New ArgumentNullException("sOutputPath", "Invalid or null argument")
            End If

            If fDistance <= 0 Then
                Throw New ArgumentOutOfRangeException("Distance", fDistance, "Invalid distance")
            End If

            Dim sCopiedFeatures As String = CopyFeatures()
            AggregatePoints(sCopiedFeatures, sOutputPath, fDistance)

        End Sub

        ''' <summary>
        ''' Creates concave hull using automate distance
        ''' </summary>
        ''' <param name="sOutputPath">Location of the output polygon feature class</param>
        ''' <remarks>Use this method when the distance between points is to calculated automatically.
        ''' This "automatic" method was not working in the old GCD. Code copied here but needs work.</remarks>
        Public Sub GenerateConcaveHull(sOutputPath As String)

            If String.IsNullOrEmpty(sOutputPath) Then
                Throw New Exception("Invalid or NULL output location")
            End If

            Throw New Exception("This ""automatic"" method was not working in the old GCD. Code copied here but needs work.")

            'Dim sCopiedFeatures As String = CopyFeatures()
            'Dim fDistance As Double = 0

            ''Create a file geodatabase to work from - this is needed to store the output tables
            ''Dim strWorkspace As String = WorkspaceManager.WorkspacePath()
            ''Dim gdbRandom As String = WorkspaceManager.GetRandomString
            ''Dim pWorkspace As IWorkspace = Nothing
            ''Dim gdbName As String = gdbRandom & ".gdb"
            ''Dim gdbPath As String = strWorkspace
            'Dim sFGDBName As String = GISCode.GISDataStructures.FileGeodatabase.GetSafeName(WorkspaceManager.WorkspacePath, "hull")
            'Dim pWorkspace As IWorkspace = Nothing
            'GISCode.GISDataStructures.FileGeodatabase.CreateFileGdbWorkspace(sFGDBName, pWorkspace)

            ''Dim tableRandom As String = WorkspaceManager.GetRandomString
            ''Dim tableRandom2 As String = WorkspaceManager.GetRandomString
            'Dim strTableRandom As String = GISCode.Table.GetSafeNameFull(pWorkspace, "table1")
            'Dim strTableRandom2 As String = GISCode.Table.GetSafeNameFull(pWorkspace, "table2")

            '' create a table for the output - table needs to be dynamic - path + name (may need to go to geodatabase)
            'GP.Analysis.NearTable(New FileInfo(sCopiedFeatures), New FileInfo(sCopiedFeatures), strTableRandom, "50", False, False, True, 3)

            ''create a table with the maximum distance calculated - table only has 1 row and 3 fields "MAX_NEAR_DIST" is the one we want a value from
            ''this value is the "auto-generated distance"
            ''TODO: get access to this table after it is created
            'Dim strCalcs As String = "NEAR_DIST MAX"
            'GP.Analysis.SummaryStats(strTableRandom, strTableRandom2, strCalcs)

            'AggregatePoints(sCopiedFeatures, sOutputPath, fDistance)

        End Sub

        ''' <summary>
        ''' Create a new, temporary copy of the input point feature class to operate on.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CopyFeatures() As String

            Dim sCopiedFeatures As String = String.Empty

            Try
                sCopiedFeatures = WorkspaceManager.GetTempShapeFile("tempPoints")
                GP.DataManagement.CopyFeatures(m_sInputPath, New FileInfo(sCopiedFeatures))
            Catch ex As Exception
                ex.Data("Input Points") = m_sInputPath
                ex.Data("sCopied Features") = sCopiedFeatures
                Throw ex
            End Try

            Return sCopiedFeatures

        End Function

        Private Sub AggregatePoints(sFeatures As String, sOutputPolygon As String, fDistance As Double)

            'TODO: add field and assign method using calculate field - these will be used for the creation of multi-method polygons (separate tool)
            'Dim strMethod As String = cboMethod.SelectedItem
            'Dim strPolyPath As String = fiAgPoints.FullName
            Try
                GP.DataManagement.AggregatePoints(sFeatures, New FileInfo(sOutputPolygon), fDistance.ToString)
            Catch ex As Exception
                ex.Data("Features") = sFeatures
                ex.Data("OutPolygon") = sOutputPolygon
                ex.Data("Distance") = fDistance
                Throw
            End Try
        End Sub

    End Class

End Namespace