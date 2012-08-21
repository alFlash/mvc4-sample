Imports System

Namespace Personify.ApplicationManager.PersonifyDataObjects
    Public Class OrderEntryParameters
        ' Fields
        Public Badges As OrderEntryParametersForBadges()
        Public CartItemId As Integer
        Public DCDFiles As OrderEntryParametersForDCDFiles()
        Public IsDirectPriceUpdate As Boolean
        Public MarketCode As String
        Public MasterCustomerId As String
        Public ProductId As Integer
        Public ProductTypeCode As String
        Public Quantity As Integer
        Public RateCode As String
        Public RateStructure As String
        Public RelatedCartItemId As Integer
        Public ShipMasterCustomerId As String
        Public ShipSubCustomerId As Integer
        Public SubCustomerId As Integer
        Public SubProductId As Integer
        Public Subsystem As String
        Public UnitPrice As String
    End Class
End Namespace

