Imports System
Imports System.ComponentModel

Namespace Personify.ApplicationManager.PersonifyDataObjects
    <Serializable, Bindable(True), ListBindable(True)> _
    Public Class WebPrice
        ' Properties
        Public Property FormattedPrice As String
            Get
                Return Me._FormattedPrice
            End Get
            Set(ByVal value As String)
                Me._FormattedPrice = value
            End Set
        End Property

        Public Property HasValidSchedulePrice As Boolean
            Get
                Return Me._HasValidSchedulePrice
            End Get
            Set(ByVal value As Boolean)
                Me._HasValidSchedulePrice = value
            End Set
        End Property

        Public Property IsDefault As Boolean
            Get
                Return Me._IsDefault
            End Get
            Set(ByVal value As Boolean)
                Me._IsDefault = value
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

        Public Property MaxScheduledPrice As Decimal
            Get
                Return Me._maxScheduledPrice
            End Get
            Set(ByVal value As Decimal)
                Me._maxScheduledPrice = value
            End Set
        End Property

        Public Property MinScheduledPrice As Decimal
            Get
                Return Me._minScheduledPrice
            End Get
            Set(ByVal value As Decimal)
                Me._minScheduledPrice = value
            End Set
        End Property

        Public Property Price As Decimal
            Get
                Return Me._Price
            End Get
            Set(ByVal value As Decimal)
                Me._Price = value
            End Set
        End Property

        Public Property ProductId As String
            Get
                Return Me._ProductId
            End Get
            Set(ByVal value As String)
                Me._ProductId = value
            End Set
        End Property

        Public Property RateCode As String
            Get
                Return Me._RateCode
            End Get
            Set(ByVal value As String)
                Me._RateCode = value
            End Set
        End Property

        Public Property RateCodeDescr As String
            Get
                Return Me._RateCodeDescr
            End Get
            Set(ByVal value As String)
                Me._RateCodeDescr = value
            End Set
        End Property

        Public Property RateStructure As String
            Get
                Return Me._RateStructure
            End Get
            Set(ByVal value As String)
                Me._RateStructure = value
            End Set
        End Property

        Public Property RateStructureDescr As String
            Get
                Return Me._RateStructureDescr
            End Get
            Set(ByVal value As String)
                Me._RateStructureDescr = value
            End Set
        End Property

        Public Property ScheduledPriceRange As String
            Get
                Return Me._ScheduledPriceRange
            End Get
            Set(ByVal value As String)
                Me._ScheduledPriceRange = value
            End Set
        End Property


        ' Fields
        Private _FormattedPrice As String
        Private _HasValidSchedulePrice As Boolean
        Private _IsDefault As Boolean
        Private _MaxBadges As Integer
        Private _maxScheduledPrice As Decimal
        Private _minScheduledPrice As Decimal
        Private _Price As Decimal
        Private _ProductId As String
        Private _RateCode As String
        Private _RateCodeDescr As String
        Private _RateStructure As String
        Private _RateStructureDescr As String
        Private _ScheduledPriceRange As String
    End Class
End Namespace

