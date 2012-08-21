Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel

Namespace Personify.ApplicationManager.PersonifyDataObjects
    <Serializable, ListBindable(True), Bindable(True)> _
    Public Class WebOrderDetail
        ' Properties
        Public Property Amount As Decimal
            Get
                Return Me._Amount
            End Get
            Set(ByVal value As Decimal)
                Me._Amount = value
            End Set
        End Property

        Public Property CycleBeginDate As DateTime
            Get
                Return Me._CycleBeginDate
            End Get
            Set(ByVal value As DateTime)
                Me._CycleBeginDate = value
            End Set
        End Property

        Public Property CycleEndDate As DateTime
            Get
                Return Me._CycleEndDate
            End Get
            Set(ByVal value As DateTime)
                Me._CycleEndDate = value
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

        Public Property Discount As Decimal
            Get
                Return Me._Discount
            End Get
            Set(ByVal value As Decimal)
                Me._Discount = value
            End Set
        End Property

        Public Property LineDescription As String
            Get
                Return Me._LineDescription
            End Get
            Set(ByVal value As String)
                Me._LineDescription = value
            End Set
        End Property

        Public Property MaxBadges As Integer
            Get
                Return Me._MaxBadges
            End Get
            Set(ByVal value As Integer)
                Me._MaxBadges = value
            End Set
        End Property

        Public Property MeetingLocation As String
            Get
                Return Me._MeetingLocation
            End Get
            Set(ByVal value As String)
                Me._MeetingLocation = value
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

        Public Property OrderLineBalance As Decimal
            Get
                Return Me._OrderLineBalance
            End Get
            Set(ByVal value As Decimal)
                Me._OrderLineBalance = value
            End Set
        End Property

        Public Property OrderLineNumber As Integer
            Get
                Return Me._OrderLineNumber
            End Get
            Set(ByVal value As Integer)
                Me._OrderLineNumber = value
            End Set
        End Property

        Public Property OrderLineStatusCode As String
            Get
                Return Me._OrderLineStatusCode
            End Get
            Set(ByVal value As String)
                Me._OrderLineStatusCode = value
            End Set
        End Property

        Public Property OrderLineStatusCodeDescr As String
            Get
                Return Me._OrderLineStatusCodeDescr
            End Get
            Set(ByVal value As String)
                Me._OrderLineStatusCodeDescr = value
            End Set
        End Property

        Public Property OrderLineTotal As Decimal
            Get
                Return Me._OrderLineTotal
            End Get
            Set(ByVal value As Decimal)
                Me._OrderLineTotal = value
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

        Public Property OrderQuantity As Long
            Get
                Return Me._OrderQuantity
            End Get
            Set(ByVal value As Long)
                Me._OrderQuantity = value
            End Set
        End Property

        Public Property Paid As Decimal
            Get
                Return Me._Paid
            End Get
            Set(ByVal value As Decimal)
                Me._Paid = value
            End Set
        End Property

        Public Property Product As String
            Get
                Return Me._Product
            End Get
            Set(ByVal value As String)
                Me._Product = value
            End Set
        End Property

        Public Property ProductId As Integer
            Get
                Return Conversions.ToInteger(Me._ProductId)
            End Get
            Set(ByVal value As Integer)
                Me._ProductId = Conversions.ToString(value)
            End Set
        End Property

        Public Property ShipAddress As String
            Get
                Return Me._ShipAddress
            End Get
            Set(ByVal value As String)
                Me._ShipAddress = value
            End Set
        End Property

        Public Property ShipCustomer As String
            Get
                Return Me._ShipCustomer
            End Get
            Set(ByVal value As String)
                Me._ShipCustomer = value
            End Set
        End Property

        Public Property Shipping As Decimal
            Get
                Return Me._Shipping
            End Get
            Set(ByVal value As Decimal)
                Me._Shipping = value
            End Set
        End Property

        Public Property ShippingStatus As String
            Get
                Return Me._ShippingStatus
            End Get
            Set(ByVal value As String)
                Me._ShippingStatus = value
            End Set
        End Property

        Public Property SubSystem As String
            Get
                Return Me._Subsystem
            End Get
            Set(ByVal value As String)
                Me._Subsystem = value
            End Set
        End Property

        Public Property Tax As Decimal
            Get
                Return Me._Tax
            End Get
            Set(ByVal value As Decimal)
                Me._Tax = value
            End Set
        End Property

        Public Property Total As Decimal
            Get
                Return Me._Total
            End Get
            Set(ByVal value As Decimal)
                Me._Total = value
            End Set
        End Property

        Public Property TotalLinePayments As Decimal
            Get
                Return Me._TotalLinePayments
            End Get
            Set(ByVal value As Decimal)
                Me._TotalLinePayments = value
            End Set
        End Property

        Public Property TrackingNumber As String
            Get
                Return Me._TrackingNumber
            End Get
            Set(ByVal value As String)
                Me._TrackingNumber = value
            End Set
        End Property


        ' Fields
        Private _Amount As Decimal
        Private _CycleBeginDate As DateTime
        Private _CycleEndDate As DateTime
        Private _Description As String
        Private _Discount As Decimal
        Private _LineDescription As String
        Private _MaxBadges As Integer
        Private _MeetingLocation As String
        Private _OrderDate As DateTime
        Private _OrderLineBalance As Decimal
        Private _OrderLineNumber As Integer
        Private _OrderLineStatusCode As String
        Private _OrderLineStatusCodeDescr As String
        Private _OrderLineTotal As Decimal
        Private _OrderNumber As String
        Private _OrderQuantity As Long
        Private _Paid As Decimal
        Private _Product As String
        Private _ProductId As String
        Private _ShipAddress As String
        Private _ShipCustomer As String
        Private _Shipping As Decimal
        Private _ShippingStatus As String
        Private _Subsystem As String
        Private _Tax As Decimal
        Private _Total As Decimal
        Private _TotalLinePayments As Decimal
        Private _TrackingNumber As String
    End Class
End Namespace

