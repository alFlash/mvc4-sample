Imports System
Imports System.ComponentModel

Namespace Personify.ApplicationManager.PersonifyDataObjects
    <Serializable, Bindable(True), ListBindable(True)> _
    Public Class ApplicationSubCode
        ' Properties
        Public Property Code As String
            Get
                Return Me._Code
            End Get
            Set(ByVal value As String)
                Me._Code = value
            End Set
        End Property

        Public Property Description As String
            Get
                Return Me._Description
            End Get
            Set(ByVal value As String)
                Me._Description = value
            End Set
        End Property

        Public Property SubCode As String
            Get
                Return Me._SubCode
            End Get
            Set(ByVal value As String)
                Me._SubCode = value
            End Set
        End Property


        ' Fields
        Private _Code As String
        Private _Description As String
        Private _SubCode As String
    End Class
End Namespace

