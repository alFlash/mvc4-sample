Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Reflection

Namespace Personify.ApplicationManager.PersonifyDataObjects
    <Serializable, Bindable(True), ListBindable(True), DefaultMember("Item")> _
    Public Class ApplicationCodes
        Inherits CollectionBase
        ' Methods
        Public Overridable Function Add(ByVal Item As ApplicationCode) As Integer
            Return Me.List.Add(Item)
        End Function

        Public Overridable Function AddNewCode() As ApplicationCode
            Dim code2 As New ApplicationCode
            Dim num As Integer = Me.List.Add(code2)
            Return Me.Item(num)
        End Function


        ' Properties
        Public ReadOnly Default Property Item(ByVal nIndex As Integer) As ApplicationCode
            Get
                If ((nIndex >= 0) And (nIndex < Me.List.Count)) Then
                    Return DirectCast(Me.List.Item(nIndex), ApplicationCode)
                End If
                Return Nothing
            End Get
        End Property

    End Class
End Namespace

