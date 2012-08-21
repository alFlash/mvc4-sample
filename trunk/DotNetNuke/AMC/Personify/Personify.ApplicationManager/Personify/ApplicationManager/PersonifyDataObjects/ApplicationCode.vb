Imports System
Imports System.ComponentModel

Namespace Personify.ApplicationManager.PersonifyDataObjects
    <Serializable, ListBindable(True), Bindable(True)> _
    Public Class ApplicationCode
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

        Public Property SubCodes As ApplicationSubCodes
            Get
                Return Me._SubCodes
            End Get
            Set(ByVal value As ApplicationSubCodes)
                Me._SubCodes = value
            End Set
        End Property


        ' Fields
        Private _Code As String
        Private _Description As String
        Private _SubCodes As ApplicationSubCodes
    End Class
End Namespace

