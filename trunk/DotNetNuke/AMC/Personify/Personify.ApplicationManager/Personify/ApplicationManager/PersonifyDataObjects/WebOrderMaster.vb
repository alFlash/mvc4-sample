Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel

Namespace Personify.ApplicationManager.PersonifyDataObjects
    <Serializable, Bindable(True), ListBindable(True)> _
    Public Class WebOrderMaster
        ' Properties
        Public Property BillMasterCustomerID As String
            Get
                Return Me._BillMasterCustomerID
            End Get
            Set(ByVal value As String)
                Me._BillMasterCustomerID = value
            End Set
        End Property

        Public Property BillSubCustomerID As Integer
            Get
                Return Me._BillSubCustomerID
            End Get
            Set(ByVal value As Integer)
                Me._BillSubCustomerID = value
            End Set
        End Property

        Public Property BillTo As String
            Get
                Return Me._BillTo
            End Get
            Set(ByVal value As String)
                Me._BillTo = value
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

        Public Property Details As WebOrderDetails
            Get
                Return Me._Details
            End Get
            Set(ByVal value As WebOrderDetails)
                Me._Details = value
            End Set
        End Property

        Public Property OrderBalance As Decimal
            Get
                Return Me._OrderBalance
            End Get
            Set(ByVal value As Decimal)
                Me._OrderBalance = value
            End Set
        End Property

        Public Property OrderDate As DateTime
            Get
                Return Me._OrderDate
            End Get
            Set(ByVal value As DateTime)
                Me._OrderDate = value
            End Set
        End Property

        Public Property OrderMaxBadges As Integer
            Get
                Return Me._OrderMaxBadges
            End Get
            Set(ByVal value As Integer)
                Me._OrderMaxBadges = value
            End Set
        End Property

        Public Property OrderNumber As String
            Get
                Return Me._OrderNumber
            End Get
            Set(ByVal value As String)
                Me._OrderNumber = value
            End Set
        End Property

        Public Property OrderStatusCode As String
            Get
                Return Me._OrderStatusCode
            End Get
            Set(ByVal value As String)
                Me._OrderStatusCode = value
            End Set
        End Property

        Public Property OrderStatusCodeDescr As String
            Get
                Return Me._OrderStatusCodeDescr
            End Get
            Set(ByVal value As String)
                Me._OrderStatusCodeDescr = value
            End Set
        End Property

        Public Property OrderTotal As Decimal
            Get
                Return Me._OrderTotal
            End Get
            Set(ByVal value As Decimal)
                Me._OrderTotal = value
            End Set
        End Property

        Public Property ShipMasterCustomerID As String
            Get
                Return Me._ShipMasterCustomerID
            End Get
            Set(ByVal value As String)
                Me._ShipMasterCustomerID = value
            End Set
        End Property

        Public Property ShipSubCustomerID As Integer
            Get
                Return Me._ShipSubCustomerID
            End Get
            Set(ByVal value As Integer)
                Me._ShipSubCustomerID = value
            End Set
        End Property

        Public Property ShipTo As String
            Get
                Return Me._ShipTo
            End Get
            Set(ByVal value As String)
                Me._ShipTo = value
            End Set
        End Property

        Public Property TotalPayments As Decimal
            Get
                Return Me._TotalPayments
            End Get
            Set(ByVal value As Decimal)
                Me._TotalPayments = value
            End Set
        End Property

        Public Property TotalShipping As String
            Get
                Return Conversions.ToString(Me._TotalShipping)
            End Get
            Set(ByVal value As String)
                Me._TotalShipping = Conversions.ToDecimal(value)
            End Set
        End Property

        Public Property TotalTax As String
            Get
                Return Conversions.ToString(Me._TotalTax)
            End Get
            Set(ByVal value As String)
                Me._TotalTax = Conversions.ToDecimal(value)
            End Set
        End Property


        ' Fields
        Private _BillMasterCustomerID As String
        Private _BillSubCustomerID As Integer
        Private _BillTo As String
        Private _Description As String
        Private _Details As WebOrderDetails
        Private _OrderBalance As Decimal
        Private _OrderDate As DateTime
        Private _OrderMaxBadges As Integer
        Private _OrderNumber As String
        Private _OrderStatusCode As String
        Private _OrderStatusCodeDescr As String
        Private _OrderTotal As Decimal
        Private _ShipMasterCustomerID As String
        Private _ShipSubCustomerID As Integer
        Private _ShipTo As String
        Private _TotalPayments As Decimal
        Private _TotalShipping As Decimal
        Private _TotalTax As Decimal
    End Class
End Namespace

