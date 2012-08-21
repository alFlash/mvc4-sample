Imports System
Imports System.ComponentModel

Namespace Personify.ApplicationManager.PersonifyDataObjects
    <Serializable, Bindable(True), ListBindable(True)> _
    Public Class ProductComponent
        ' Properties
        Public Property ComponentQuantity As Integer
            Get
                Return Me._ComponentQuantity
            End Get
            Set(ByVal value As Integer)
                Me._ComponentQuantity = value
            End Set
        End Property

        Public Property CustomerPrice As Decimal
            Get
                Return Me._CustomerPrice
            End Get
            Set(ByVal value As Decimal)
                Me._CustomerPrice = value
            End Set
        End Property

        Public Property IsWebEnabledProduct As Boolean
            Get
                Return Me._IsWebEnabledProduct
            End Get
            Set(ByVal value As Boolean)
                Me._IsWebEnabledProduct = value
            End Set
        End Property

        Public Property ListPrice As Decimal
            Get
                Return Me._ListPrice
            End Get
            Set(ByVal value As Decimal)
                Me._ListPrice = value
            End Set
        End Property

        Public Property LongName As String
            Get
                Return Me._LongName
            End Get
            Set(ByVal value As String)
                Me._LongName = value
            End Set
        End Property

        Public Property MasterProductId As Integer
            Get
                Return Me._MasterProductId
            End Get
            Set(ByVal value As Integer)
                Me._MasterProductId = value
            End Set
        End Property

        Public Property MemberPrice As Decimal
            Get
                Return Me._MemberPrice
            End Get
            Set(ByVal value As Decimal)
                Me._MemberPrice = value
            End Set
        End Property

        Public Property ParentProduct As String
            Get
                Return Me._ParentProduct
            End Get
            Set(ByVal value As String)
                Me._ParentProduct = value
            End Set
        End Property

        Public Property ProductCode As String
            Get
                Return Me._ProductCode
            End Get
            Set(ByVal value As String)
                Me._ProductCode = value
            End Set
        End Property

        Public Property ProductId As Integer
            Get
                Return Me._ProductId
            End Get
            Set(ByVal value As Integer)
                Me._ProductId = value
            End Set
        End Property

        Public Property ProductType As String
            Get
                Return Me._ProductType
            End Get
            Set(ByVal value As String)
                Me._ProductType = value
            End Set
        End Property

        Public Property ShortName As String
            Get
                Return Me._ShortName
            End Get
            Set(ByVal value As String)
                Me._ShortName = value
            End Set
        End Property

        Public Property SmallImageFileName As String
            Get
                Return Me._SmallImageFileName
            End Get
            Set(ByVal value As String)
                Me._SmallImageFileName = value
            End Set
        End Property

        Public Property SubSystem As String
            Get
                Return Me._SubSystem
            End Get
            Set(ByVal value As String)
                Me._SubSystem = value
            End Set
        End Property

        Public Property WebLongDescription As String
            Get
                Return Me._WebLongDescription
            End Get
            Set(ByVal value As String)
                Me._WebLongDescription = value
            End Set
        End Property

        Public Property WebShortDescription As String
            Get
                Return Me._WebShortDescription
            End Get
            Set(ByVal value As String)
                Me._WebShortDescription = value
            End Set
        End Property


        ' Fields
        Private _ComponentQuantity As Integer
        Private _CustomerPrice As Decimal
        Private _IsWebEnabledProduct As Boolean
        Private _ListPrice As Decimal
        Private _LongName As String
        Private _MasterProductId As Integer
        Private _MemberPrice As Decimal
        Private _ParentProduct As String
        Private _ProductCode As String
        Private _ProductId As Integer
        Private _ProductType As String
        Private _ShortName As String
        Private _SmallImageFileName As String
        Private _SubSystem As String
        Private _WebLongDescription As String
        Private _WebShortDescription As String
    End Class
End Namespace

