Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Reflection

Namespace Personify.ApplicationManager.PersonifyDataObjects
    <Serializable, ListBindable(True), Bindable(True), DefaultMember("Item")> _
    Public Class WebOrderMasters
        Inherits CollectionBase
        ' Methods
        Public Overridable Function Add(ByVal Item As WebOrderMaster) As Integer
            Return Me.List.Add(Item)
        End Function

        Public Overridable Function AddNewOrderMaster() As WebOrderMaster
            Dim master2 As New WebOrderMaster
            Dim num As Integer = Me.List.Add(master2)
            Return Me.Item(num)
        End Function


        ' Properties
        Public ReadOnly Default Property Item(ByVal nIndex As Integer) As WebOrderMaster
            Get
                If ((nIndex >= 0) And (nIndex < Me.List.Count)) Then
                    Return DirectCast(Me.List.Item(nIndex), WebOrderMaster)
                End If
                Return Nothing
            End Get
        End Property

    End Class
End Namespace

