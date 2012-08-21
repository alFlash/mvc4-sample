Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Reflection

Namespace Personify.ApplicationManager.PersonifyDataObjects
    <Serializable, Bindable(True), DefaultMember("Item"), ListBindable(True)> _
    Public Class WebOrderDetails
        Inherits CollectionBase
        ' Methods
        Public Overridable Function Add(ByVal Item As WebOrderDetail) As Integer
            Return Me.List.Add(Item)
        End Function

        Public Overridable Function AddNewOrderDetail() As WebOrderDetail
            Dim detail2 As New WebOrderDetail
            Dim num As Integer = Me.List.Add(detail2)
            Return Me.Item(num)
        End Function


        ' Properties
        Public ReadOnly Default Property Item(ByVal nIndex As Integer) As WebOrderDetail
            Get
                If ((nIndex >= 0) And (nIndex < Me.List.Count)) Then
                    Return DirectCast(Me.List.Item(nIndex), WebOrderDetail)
                End If
                Return Nothing
            End Get
        End Property

    End Class
End Namespace

