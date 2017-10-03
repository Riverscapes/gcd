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

#Region "Imports"
Imports System.IO
#End Region

Namespace GISCode.GCD

    Public Class XMLHandling

        Public _Dataset As Data.DataSet

        Public Shared Sub XMLWriteFIS(ByRef WriteDataset As Data.DataSet)

            Dim sw As StringWriter = New StringWriter()
            WriteDataset.WriteXml(sw)
            My.Settings.FISLibraryXML = sw.ToString()
            My.Settings.Save()

        End Sub

        Public Shared Sub XMLReadFIS(ByRef ReadDataset As Data.DataSet)

            Dim sr As StringReader = New StringReader(My.Settings.FISLibraryXML)
            ReadDataset.Clear()
            ReadDataset.ReadXml(sr)
            'Dim resultRead As String = sr.ToString()

        End Sub

    End Class

End Namespace
