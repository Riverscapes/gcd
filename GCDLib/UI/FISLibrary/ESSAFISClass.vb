Imports System.Collections

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

Public Class ESSAFISClass
    Private fisRules As New FISRuleSet()

    Public Sub New(ByVal fisFile As String)

        fisRules.parseFile(fisFile)

    End Sub

    Public ReadOnly Property inputs As ArrayList
        Get
            inputs = New ArrayList
            For index As Integer = 0 To fisRules.numInputs - 1
                inputs.Add(fisRules.getInputName(index))
            Next
        End Get
    End Property

End Class
