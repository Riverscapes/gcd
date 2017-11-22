Namespace Project

    Public Class ProjectTreeNode

        Private m_Type As ucProjectExplorer.GCDNodeTypes
        Private m_Item As Object

        Public ReadOnly Property Item As Object
            Get
                Return m_Item
            End Get
        End Property

        Public ReadOnly Property NodeType As ucProjectExplorer.GCDNodeTypes
            Get
                Return m_Type
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return Item.ToString()
        End Function

        Public Sub New(type As ucProjectExplorer.GCDNodeTypes, theItem As Object)
            m_Type = type
            m_Item = theItem
        End Sub

    End Class

End Namespace
