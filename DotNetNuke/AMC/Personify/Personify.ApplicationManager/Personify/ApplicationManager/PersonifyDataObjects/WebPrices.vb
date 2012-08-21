Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Reflection

Namespace Personify.ApplicationManager.PersonifyDataObjects
    <Serializable, Bindable(True), DefaultMember("Item"), ListBindable(True)> _
    Public Class WebPrices
        Inherits CollectionBase
        ' Methods
        Public Overridable Function Add(ByVal Item As WebPrice) As Integer
            Return Me.List.Add(Item)
        End Function

        Public Overridable Function AddNewWebPrice() As WebPrice
            Dim price2 As New WebPrice
            Dim num As Integer = Me.List.Add(price2)
            Return Me.Item(num)
        End Function


        ' Properties
        Public ReadOnly Default Property Item(ByVal nIndex As Integer) As WebPrice
            Get
                If ((nIndex >= 0) And (nIndex < Me.List.Count)) Then
                    Return DirectCast(Me.List.Item(nIndex), WebPrice)
                End If
                Return Nothing
            End Get
        End Property

    End Class
End Namespace

