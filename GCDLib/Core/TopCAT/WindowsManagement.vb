Imports Microsoft.Win32
Imports System.IO

Namespace Core.TopCAT
    Public Class WindowsManagement

        Public Shared Function getESRI_PythonPath() As String

            Dim pythonPath As String = Nothing
            Dim localMachineKey = Registry.LocalMachine
            Dim softwareKey = localMachineKey.OpenSubKey("SOFTWARE\Wow6432Node")

            If softwareKey Is Nothing Then
                softwareKey = localMachineKey.OpenSubKey("SOFTWARE")
            End If

            Dim esriKey = softwareKey.OpenSubKey("ESRI")
            Dim realVersion = DirectCast(esriKey.OpenSubKey("ArcGIS").GetValue("RealVersion"), String)
            Dim shortVersion As String = [String].Join(".", realVersion.Split("."c).Take(2).ToArray())
            Dim pythonKey = esriKey.OpenSubKey("Python" + shortVersion)

            If pythonKey Is Nothing Then
                Throw New InvalidOperationException("Python not installed with ArcGIS!")
            End If

            Dim pythonDirectory = DirectCast(pythonKey.GetValue("PythonDir"), String)
            If Directory.Exists(pythonDirectory) Then
                Dim pythonPathFromRegistry As String = Path.Combine(Path.Combine(pythonDirectory, "ArcGIS" & Convert.ToString(shortVersion)), "python.exe")
                If File.Exists(pythonPathFromRegistry) Then
                    pythonPath = pythonPathFromRegistry
                End If

            End If

            Return pythonPath

        End Function

        Public Shared Function getR_Path() As String

            Dim rPath As String = Nothing
            Dim rCore As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\R-core")

            If rCore Is Nothing Then
                Throw New System.ApplicationException("R Core registry key was not found.")
            End If

            'Dim is64Bit As Boolean = System.Environment.Is64BitProcess
            Dim is64Bit As Boolean
            If IntPtr.Size = 8 Then
                is64Bit = True
            ElseIf IntPtr.Size = 4 Then
                is64Bit = False
            End If

            Dim R As Microsoft.Win32.RegistryKey = rCore.OpenSubKey(If(is64Bit, "R64", "R"))

            If R Is Nothing Then
                Throw New System.ApplicationException("R registry key was not found.")
            End If

            Dim currentVersion As System.Version = New System.Version(DirectCast(R.GetValue("Current Version"), String))
            Dim installPath As String = DirectCast(R.GetValue("InstallPath"), String)
            Dim bin As String = System.IO.Path.Combine(installPath, "bin")

            Return If(currentVersion < New System.Version(2, 12), bin, System.IO.Path.Combine(bin, If(is64Bit, "x64", "i386")))

        End Function

        Public Shared Function CreateTemporaryDirectory(ByVal folderName As String)

            'Create a temporary folder to house the intermediate files
            Dim tempDir As String = IO.Path.Combine(Environ("TEMP"), folderName)
            System.IO.Directory.CreateDirectory(tempDir)
            Return tempDir

        End Function

    End Class

End Namespace