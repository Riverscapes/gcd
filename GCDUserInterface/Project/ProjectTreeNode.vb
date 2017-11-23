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

        Public Shared Operator =(node1 As ProjectTreeNode, node2 As ProjectTreeNode) As Boolean

            If node1 Is Nothing AndAlso node2 Is Nothing Then
                Return True
            ElseIf node1 Is Nothing Then
                Return False
            ElseIf node2 Is Nothing Then
                Return False
            Else
                If (node1.NodeType = node2.NodeType) Then
                    Return node1.Item Is node2.Item
                Else
                    Return False
                End If
            End If
        End Operator

        Public Shared Operator <>(node1 As ProjectTreeNode, node2 As ProjectTreeNode) As Boolean
            Return Not node1 = node2
        End Operator

    End Class

End Namespace
