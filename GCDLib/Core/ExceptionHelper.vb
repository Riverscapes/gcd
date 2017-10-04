Namespace Core
    Public Class ExceptionHelper
        Inherits naru.error.ExceptionUI

        ''' <summary>
        ''' Prints an error message to the relevant output locations.
        ''' </summary>
        ''' <param name="ex">The exception object.</param>
        ''' <param name="xmlLogFile">Optional log file to print the error message</param>
        ''' <remarks>Use this message to report errors in console applications. It will iterate through
        ''' the data dictionary in the exception object.</remarks>
        Public Overloads Shared Sub HandleException(ByVal ex As Exception, ByVal xmlLogFile As Xml.XmlTextWriter, Optional sMessage As String = "")

            Dim sMainMessage As String = sMessage
            If Not TypeOf ex Is Exception Then
                sMainMessage = "Unknown error recorded. Null exception caught."
            Else
                sMainMessage = GetExceptionInformation(ex)
            End If
            AppendToolInformation(sMainMessage)

            System.Console.WriteLine(sMainMessage)
            Debug.Print(sMainMessage)

            If TypeOf xmlLogFile Is Xml.XmlTextWriter Then
                xmlLogFile.WriteStartElement("error")
                xmlLogFile.WriteAttributeString("time", Now.ToString)
                xmlLogFile.WriteElementString("error_message", sMainMessage)
            End If

            If TypeOf xmlLogFile Is Xml.XmlTextWriter Then
                xmlLogFile.WriteEndElement()
            End If
        End Sub

        Public Shared Sub UpdateStatus(sMessage As String)
            Debug.WriteLine(sMessage)
        End Sub

        ''' <summary>
        ''' Returns a string that contains the name and version of the software tool in which the error has occurred
        ''' </summary>
        ''' <remarks></remarks>
        Protected Shared Shadows Sub AppendToolInformation(ByRef sMessage As String)

            ' Call the base class
            'naru.error.ExceptionUI.AppendToolInformation(sMessage)

            Try
                Dim sToolSpecificMessage As String = String.Format("RasterMan version {0}{1}", External.RasterManager.GetFileVersion.FileVersion, vbNewLine)
                sToolSpecificMessage &= String.Format("RasterMan version {0}{1}", External.GCDCore.GetFileVersion.FileVersion, vbNewLine)
                sMessage &= sToolSpecificMessage
            Catch ex As Exception
                '
                ' Do Nothing. This information is a nice-to-have, and not essential
                '
            End Try

        End Sub

    End Class
End Namespace