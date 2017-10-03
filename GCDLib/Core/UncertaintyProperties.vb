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

Namespace GISCode.GCD

    Public Class UncertaintyProperties
        Private _UncertaintyID As Integer = -1
        Private m_projectDS As ProjectDS

        Public Sub New(projectDS As ProjectDS, ByVal UncertaintyID As Integer)
            _UncertaintyID = UncertaintyID
            m_projectDS = projectDS
        End Sub

        Public ReadOnly Property NewSurveySource As String
            Get
                Dim UncertaintyProperties As ProjectDS.UncertaintyPropertiesRow = m_projectDS.UncertaintyProperties.FindByUncertaintyID(_UncertaintyID)
                Dim DoDID As Integer = UncertaintyProperties.DoDID
                Dim DoDProps As ProjectDS.DoDTableRow = m_projectDS.DoDTable.FindByDoDID(DoDID)
                Dim newSurveyName As String = DoDProps.NewSurveyName

                Dim NewSurveys As ProjectDS.DEMSurveyRow() = m_projectDS.DEMSurvey.Select(m_projectDS.DEMSurvey.NameColumn.ColumnName & "='" & newSurveyName & "'")
                Dim NewSurvey As ProjectDS.DEMSurveyRow = NewSurveys(0)
                NewSurveySource = NewSurvey.Source

            End Get
        End Property

        Public ReadOnly Property OldSurveySource As String
            Get
                Dim UncertaintyProperties As ProjectDS.UncertaintyPropertiesRow = m_projectDS.UncertaintyProperties.FindByUncertaintyID(_UncertaintyID)
                Dim DoDID As Integer = UncertaintyProperties.DoDID
                Dim DoDProps As ProjectDS.DoDTableRow = m_projectDS.DoDTable.FindByDoDID(DoDID)
                Dim oldSurveyName As String = DoDProps.OldSurveyName

                Dim OldSurveys As ProjectDS.DEMSurveyRow() = m_projectDS.DEMSurvey.Select(m_projectDS.DEMSurvey.NameColumn.ColumnName & "='" & oldSurveyName & "'")
                Dim OldSurvey As ProjectDS.DEMSurveyRow = OldSurveys(0)
                OldSurveySource = OldSurvey.Source

            End Get
        End Property
    End Class

End Namespace