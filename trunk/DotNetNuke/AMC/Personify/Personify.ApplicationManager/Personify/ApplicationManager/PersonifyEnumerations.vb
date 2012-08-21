Imports System

Namespace Personify.ApplicationManager
    Public Class PersonifyEnumerations
        ' Nested Types
        Public Enum CustomerMode
            ' Fields
            BILL = 1
            BOTH = 2
            SHIP = 0
        End Enum

        Public Enum ExportType
            ' Fields
            CSV = 2
            Excel = 1
        End Enum

        Public Enum MembersOnlyFilter
            ' Fields
            Both = 2
            No = 0
            Yes = 1
        End Enum

        Public Enum PersonifyCheckOut_ModuleCommunicationKeys
            ' Fields
            CheckForSamePageCheckout = 4
            CreateOrderFromProductIds = 3
            RequestProductIdsFromProductDetailWebPart = 0
            RespondToValidationIssues = 2
            SamePageCheckOutEnabled = 5
            SelectedProductIds = 1
        End Enum

        Public Enum PersonifyUserLoginStatus
            ' Fields
            INVALIDPASSWORD = 2
            INVALIDUSER = 1
            LOGINDISABLED = 3
            SUCCESS = 4
        End Enum

        Public Enum PricingOptions
            ' Fields
            DefaultPricing = 0
            MultiplePricing = 1
        End Enum

        Public Enum SessionKeys
            ' Fields
            PersonifyAbstractCallAdmin = &H19
            PersonifyAbstractCallSubTypeStaffPerCustomer = &H18
            PersonifyAbstractMenuXML = &H16
            PersonifyAddCustomerCommunicationGUIDKeys = &H11
            PersonifyAddCustomerGUIDKeys = 15
            PersonifyAddSegmentGUIDKeys = 14
            PersonifyCartItemMap = &H1A
            PersonifyCurrentAffiliateInfo = 7
            PersonifyCurrentSegmentInfo = 6
            PersonifyCustomerPrice = &H17
            PersonifyCustomerQualifiedRateStructures = &H15
            PersonifyGroupActionInfo = 9
            PersonifyGroupPurchaseInfo = 10
            PersonifyMembershipRenewalsViewList = 11
            PersonifyOrder = 1
            PersonifyOrderMaster = 30
            PersonifyOrdersViewList = 13
            PersonifyPayOrderIds = 12
            PersonifySaveAddressGUIDKeys = 2
            PersonifySaveCommitteeMemberGUIDKeys = 4
            PersonifySaveCommunicationGUIDKeys = 3
            PersonifySaveEmployeeRelationshipGUIDKeys = 5
            PersonifyWebOrderMaster = &H10
        End Enum

        Public Enum TransactionMode
            ' Fields
            ADD = 1
            DELETE = 3
            EDIT = 2
        End Enum

        Public Enum WebProductAttributeEnum
            ' Fields
            Both = 2
            Featured = 0
            None = 3
            Promotional = 1
        End Enum
    End Class
End Namespace

