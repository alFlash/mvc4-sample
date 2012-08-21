Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Reflection

Namespace Personify.ApplicationManager.PersonifyDataObjects
    <Serializable, Bindable(True), DefaultMember("Item"), ListBindable(True)> _
    Public Class ProductComponentList
        Inherits CollectionBase
        ' Methods
        Public Overridable Function Add(ByVal Item As ProductComponent) As Integer
            Return Me.List.Add(Item)
        End Function

        Public Overridable Function AddNewComponent() As ProductComponent
            Dim component2 As New ProductComponent
            Dim num As Integer = Me.List.Add(component2)
            Return Me.Item(num)
        End Function


        ' Properties
        Public ReadOnly Default Property Item(ByVal nIndex As Integer) As ProductComponent
            Get
                If ((nIndex >= 0) And (nIndex < Me.List.Count)) Then
                    Return DirectCast(Me.List.Item(nIndex), ProductComponent)
                End If
                Return Nothing
            End Get
        End Property

        Public Property TotalPackageListPrice As Decimal
            Get
                Return Me._TotalPackageListPrice
            End Get
            Set(ByVal value As Decimal)
                Me._TotalPackageListPrice = value
            End Set
        End Property

        Public Property TotalPackageMemberPrice As Decimal
            Get
                Return Me._TotalPackageMemberPrice
            End Get
            Set(ByVal value As Decimal)
                Me._TotalPackageMemberPrice = value
            End Set
        End Property

        Public Property TotalPackageYourPrice As Decimal
            Get
                Return Me._TotalPackageYourPrice
            End Get
            Set(ByVal value As Decimal)
                Me._TotalPackageYourPrice = value
            End Set
        End Property


        ' Fields
        Private _TotalPackageListPrice As Decimal
        Private _TotalPackageMemberPrice As Decimal
        Private _TotalPackageYourPrice As Decimal
    End Class
End Namespace

