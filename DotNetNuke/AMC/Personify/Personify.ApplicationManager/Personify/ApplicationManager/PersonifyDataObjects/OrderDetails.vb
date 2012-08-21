Imports System

Namespace Personify.ApplicationManager.PersonifyDataObjects
    Public Class OrderDetails
        ' Fields
        Public ActualCouponAmount As String
        Public BalanceDue As String
        Public BaseExtendedAmount As Decimal
        Public BaseShipAmount As String
        Public BaseTaxAmount As String
        Public BaseTotalAmount As Decimal
        Public BillMeLaterFlag As Boolean
        Public CanCreateHotelReservationBridge As Boolean
        Public CanManageHotelReservation As Boolean
        Public CouponCode As String
        Public DCDFiles As DCDFileList()
        Public DoesOrderHaveMultipleShipCustomers As Boolean
        Public FirstLineInOrder As Boolean = False
        Public FormattedBaseExtendedAmount As String
        Public FormattedBaseTotalAmount As String
        Public FulfillStatusCode As String
        Public GiftFrequencyCode As String
        Public IsARecurringGift As Boolean
        Public IsBackOrderRequired As String
        Public IsPORequiredForBillMeProduct As Boolean
        Public IsProductAndCustomerQualifyForBillMe As Boolean
        Public IsWaitListingRequired As String
        Public LineStatusCode As String
        Public LineTypeCode As String
        Public NextRecurringDate As DateTime
        Public OrderDate As String
        Public OrderLineNumber As String
        Public OrderNumber As String
        Public ProductCode As String
        Public ProductId As String
        Public ProductTitle As String
        Public ProductTypeCode As String
        Public PromoCode As String
        Public Quantity As String
        Public RateCode As String
        Public RateStructure As String
        Public RelatedLineNumber As String
        Public ShipAddressId As String
        Public ShipCustomerLabelName As String
        Public ShipFormattedAddress As String
        Public ShipMasterCustomerId As String
        Public ShipSubCustomerId As String
        Public ShipViaCode As String
        Public ShowHotelBlockingLink As Boolean
        Public Subsystem As String
        Public TotalPaid As String
        Public TrackingNumber As String
        Public UnitAmount As String
        Public UnitDiscount As String
    End Class
End Namespace

