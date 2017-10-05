Imports System.Text.RegularExpressions

Namespace Core.ErrorCalculation.FIS

    ''' <summary>
    ''' Class for loading a FIS rule file and then making the inputs available via a list.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class FISRuleFile

        Private m_sRuleFilePath As String
        Private m_lFISInputs As List(Of String)

        ''' <summary>
        ''' Get the list of FIS inputs contained in the FIS rule file.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property FISInputs As List(Of String)
            Get
                Return m_lFISInputs
            End Get
        End Property

        ''' <summary>
        ''' Get the full absolute file path of the *.fis FIS rule file
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property RuleFilePath As String
            Get
                Return m_sRuleFilePath
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sRuleFilePath">Full, absolute path to a *.fis rule file.</param>
        ''' <remarks></remarks>
        Public Sub New(sRuleFilePath As String)

            If String.IsNullOrEmpty(sRuleFilePath) Then
                Throw New Exception("The FIS rule file path cannot be null or empty.")
            Else
                If Not System.IO.File.Exists(sRuleFilePath) Then
                    Dim ex As New Exception("The FIS rule file cannot be found on the file system.")
                    ex.Data("FIS Rule File Path") = sRuleFilePath
                    Throw ex
                End If
            End If

            m_sRuleFilePath = sRuleFilePath

            Try
                Dim sRuleFileText As String = IO.File.ReadAllText(sRuleFilePath)
                m_lFISInputs = New List(Of String)

                Dim theRegEx As New Regex("dd")
                Dim theMatch As Match = theRegEx.Match(sRuleFileText)
                Dim nIndex As Integer = 0

                ' Match data between single quotes hesitantly.
                Dim col As MatchCollection = Regex.Matches(sRuleFileText, "\[Input[0-9]\]\s*Name='([^']*)'")
                For Each m As Match In col
                    ' Access first Group and its value.
                    Dim g As Group = m.Groups(1)
                    m_lFISInputs.Add(g.Value)
                Next

            Catch ex As Exception
                Dim ex2 As New Exception("Error parsing FIS rule file", ex)
                ex2.Data("FIS Rule File Path") = sRuleFilePath
                Throw ex2
            End Try
        End Sub

    End Class

End Namespace