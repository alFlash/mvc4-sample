Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Reflection

Namespace Personify.ApplicationManager.PersonifyDataObjects
    <Serializable, ListBindable(True), DefaultMember("Item"), Bindable(True)> _
    Public Class ApplicationSubCodes
        Inherits CollectionBase
        ' Methods
        Public Overridable Function Add(ByVal Item As ApplicationCode) As Integer
            Return Me.List.Add(Item)
        End Function

        Public Overridable Function AddNewSubCode() As ApplicationSubCode
            Dim code2 As New ApplicationSubCode
            Dim num As Integer = Me.List.Add(code2)
            Return Me.Item(num)
        End Function


        ' Properties
        Public ReadOnly Default Property Item(ByVal nIndex As Integer) As ApplicationSubCode
            Get
                If ((nIndex >= 0) And (nIndex < Me.List.Count)) Then
                    Return DirectCast(Me.List.Item(nIndex), ApplicationSubCode)
                End If
                Return Nothing
            End Get
        End Property

    End Class
End Namespace

