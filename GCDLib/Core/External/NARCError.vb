Imports System.Text

Namespace Core.External

    ''' <summary>
    ''' Error message class for use with the NAR C API methods.
    ''' </summary>
    ''' <remarks>Use this class to send a string variable to C API calls and 
    ''' retrieve back any error information when something goes wrong.</remarks>
    Public Class NARCError

        Private Const m_nDefaultErrorMessageSize As Integer = 1024

        ''' <summary>
        ''' The placeholder for storing the error message string returned from 
        ''' C API calls.
        ''' </summary>
        ''' <remarks>This string builder should be instantiated in the constructor
        ''' of this class using the default size member constant.</remarks>
        Private m_sErrorString As StringBuilder

        ''' <summary>
        ''' Gets or sets the error message from a C API call
        ''' </summary>
        ''' <value></value>
        ''' <returns>The error message from the last C API call</returns>
        ''' <remarks>Use this property to send the error message member variable
        ''' into a C API call. Because the string builder variable is already
        ''' allocated to 1024 characters, the C routine should be able to write
        ''' this many characters into the variable. Then the .Net code can use
        ''' this property to retrieve the error string and show it to the user.</remarks>
        Public Property ErrorString As StringBuilder
            Get
                Return m_sErrorString
            End Get
            Set(value As StringBuilder)
                m_sErrorString = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return m_sErrorString.ToString
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks>Must instantiate the member string builder so that it is
        ''' already allocated ready for sending to C API calls</remarks>
        Public Sub New()
            m_sErrorString = New StringBuilder(1024)
        End Sub

    End Class

End Namespace