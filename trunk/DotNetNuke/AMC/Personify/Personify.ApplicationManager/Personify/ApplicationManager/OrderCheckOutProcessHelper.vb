Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports System.Configuration
Imports TIMSS.API
Imports TIMSS.API.Core
Imports Microsoft.VisualBasic.CompilerServices
Imports System.Text
Imports TIMSS
Imports TIMSS.Enumerations
Imports TIMSS.API.AccountingInfo
Imports TIMSS.API.CustomerInfo
Imports TIMSS.API.ApplicationInfo
Imports TIMSS.API.OrderInfo
Imports Personify.ApplicationManager.PersonifyDataObjects
Imports TIMSS.API.Core.Validation

Namespace Personify.ApplicationManager
    Public Class OrderCheckOutProcessHelper
        Inherits BaseHelperClass
        ' Methods
        Public Sub New (ByVal OrgId As String, ByVal OrgUnitId As String)
            MyBase.New (OrgId, OrgUnitId)
        End Sub

        Public Sub New (ByVal OrgId As String, ByVal OrgUnitId As String, ByVal EnableOnDemandDataLoad As Boolean)
            MyBase.New (OrgId, OrgUnitId, EnableOnDemandDataLoad)
        End Sub

        Public Sub New (ByVal OrgId As String, ByVal OrgUnitId As String, ByVal BaseCurrency As Currency,
                        ByVal PortalCurrency As Currency, ByVal EnableOnDemandDataLoad As Boolean)
            MyBase.New (OrgId, OrgUnitId, EnableOnDemandDataLoad)
            Me._BaseCurrency = BaseCurrency
            Me._PortalCurrency = PortalCurrency
        End Sub

        Public Overridable Function AddSubProductToExistingOrder (ByVal OrderNumber As String,
                                                                  ByVal OrderLineNumber As Integer,
                                                                  ByVal ParamArray ProductId As String()) _
            As IOrderMasters
            Dim masters As IOrderMasters
            Try
                Dim masters2 As IOrderMasters = DirectCast ([Global].GetCollection (Me.OrganizationId,
                                                                                    Me.OrganizationUnitId,
                                                                                    NamespaceEnum.OrderInfo,
                                                                                    "OrderMasters"),
                                                            IOrderMasters)
                masters2.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
                masters2.Fill (OrderNumber)
                If (masters2.Count > 0) Then
                    Dim detail As IOrderDetail = DirectCast (masters2.Item (0).Details.FindObject ("OrderLineNumber",
                                                                                                   OrderLineNumber),
                                                             IOrderDetail)
                    If (Not detail Is Nothing) Then
                        Dim str As String
                        For Each str In ProductId
                            Dim num As Long = Convert.ToInt64 (str.Split (New Char() {"q"c}) (0))
                            Dim num2 As Integer = CInt (Convert.ToInt64 (str.Split (New Char() {"q"c}) (1)))
                            masters2.Item (0).AddSubProduct (CLng (Convert.ToInt32 (num)), OrderLineNumber)
                            Dim num5 As Integer = (masters2.Item (0).Details.Count - 1)
                            Dim i As Integer = 0
                            Do While (i <= num5)
                                If _
                                    ((masters2.Item (0).Details.Item (i).ObjectState = BusinessObjectState.Added) AndAlso
                                     (masters2.Item (0).Details.Item (i).ProductId = num)) Then
                                    masters2.Item (0).Details.Item (i).OrderQuantity = num2
                                End If
                                i += 1
                            Loop
                        Next
                    End If
                    masters2.Validate
                End If
                masters = masters2
            Catch exception1 As Exception
                ProjectData.SetProjectError (exception1)
                Dim exception As Exception = exception1
                Throw exception
            End Try
            Return masters
        End Function

        Public Overridable Function CheckIfThePayorHAsCreditCardOfRecord (ByVal MasterCustomerId As String,
                                                                          ByVal SubCustomerID As Integer) As Boolean
            Dim flag As Boolean = False
            Dim receipts As IARReceipts = DirectCast ([Global].GetCollection (Me.OrganizationId, Me.OrganizationUnitId,
                                                                              NamespaceEnum.AccountingInfo, "ARReceipts"),
                                                      IARReceipts)
            receipts.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
            Dim receipt2 As IARReceipt = receipts.AddNew
            receipt2.BillMasterCustomerId = MasterCustomerId
            receipt2.BillSubCustomerId = SubCustomerID
            receipt2.MasterCustomerId = MasterCustomerId
            receipt2.SubCustomerId = SubCustomerID
            If receipt2.PayorHasCreditCardOfRecord Then
                flag = True
            End If
            receipt2 = Nothing
            Return flag
        End Function

        Public Overridable Function CreateOrderMaster (ByVal MasterCustomerId As String, ByVal SubCustomerId As Integer) _
            As IOrderMasters
            Dim objectToAdd As IOrderMasters = DirectCast (SessionManager.GetSessionObject (0,
                                                                                            PersonifyEnumerations.
                                                                                               SessionKeys.
                                                                                               PersonifyOrderMaster,
                                                                                            MasterCustomerId),
                                                           IOrderMasters)
            If (objectToAdd Is Nothing) Then
                objectToAdd = DirectCast ([Global].GetCollection (Me.OrganizationId, Me.OrganizationUnitId,
                                                                  NamespaceEnum.OrderInfo, "OrderMasters"),
                                          IOrderMasters)
                objectToAdd.CreateNewOrder (MasterCustomerId, SubCustomerId, 0, Nothing, Nothing, Nothing, 1, Nothing, 0)
                objectToAdd.Item (0).OrderMethodCode =
                    objectToAdd.Item (0).OrderMethodCode.List.Item ("WEB").ToCodeObject
                objectToAdd.Item (0).CurrencyCode.Code = Me._PortalCurrency.Code
                SessionManager.AddSessionObject (0, PersonifyEnumerations.SessionKeys.PersonifyOrderMaster, objectToAdd,
                                                 MasterCustomerId)
                Return objectToAdd
            End If
            Dim index As Integer = 0
            Dim numArray As Integer() = New Integer((index + 1) - 1) {}
            Dim num4 As Integer = (objectToAdd.Item (0).Details.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num4)
                If _
                    (objectToAdd.Item (0).Details.Item (i).OrderLineNumber =
                     objectToAdd.Item (0).Details.Item (i).RelatedLineNumber) Then
                    If (index > 0) Then
                        numArray = DirectCast (Utils.CopyArray (DirectCast (numArray, Array),
                                                                New Integer((index + 1) - 1) {}),
                                               Integer())
                    End If
                    numArray (index) = objectToAdd.Item (0).Details.Item (i).OrderLineNumber
                    objectToAdd.Item(0).Details.Item(i).ReadOnly("InvoiceNumber") = False '("InvoiceNumber", False)
                    objectToAdd.Item (0).Details.Item (i).SetNull ("InvoiceNumber")
                    index += 1
                End If
                i += 1
            Loop
            Dim num3 As Integer
            For Each num3 In numArray
                If (num3 > 0) Then
                    objectToAdd.Item (0).Delete (num3)
                End If
            Next
            Return objectToAdd
        End Function

        Public Overridable Function CreateOrders(ByVal MasterCustomerId As String, ByVal SubCustomerId As Integer,
                                                  ByVal OrderParameters As OrderEntryParameters(),
                                                        Optional ByRef CartItemMap As Hashtable = Nothing) _
            As IOrderMasters

            If CartItemMap Is Nothing Then
                CartItemMap = New Hashtable()
            End If


            Dim detail As IOrderDetail = Nothing
            If (CartItemMap Is Nothing) Then
                CartItemMap = New Hashtable
            End If
            Dim count As Integer = -1
            Dim masters2 As IOrderMasters = Me.CreateOrderMaster(MasterCustomerId, SubCustomerId)
            Dim flag As Boolean = False
            Dim parameters As OrderEntryParameters
            For Each parameters In OrderParameters
                If (parameters.RelatedCartItemId = 0) Then
                    detail = masters2.Item(0).Details.AddNew
                    detail.ProductId = parameters.ProductId
                    Dim orderLineNumber As Integer = detail.OrderLineNumber
                    CartItemMap.Add(parameters.CartItemId, orderLineNumber)
                Else
                    count = masters2.Item(0).Details.Count
                    If _
                        masters2.Item(0).AddSubProduct(CLng(parameters.SubProductId),
                                                         Conversions.ToInteger(
                                                             CartItemMap.Item(parameters.RelatedCartItemId))) Then
                        Dim enumerator As IEnumerator
                        Try
                            enumerator = masters2.Item(0).Details.GetEnumerator
                            Do While enumerator.MoveNext
                                Dim current As IOrderDetail = DirectCast(enumerator.Current, IOrderDetail)
                                If (current.OrderLineNumber = (count + 1)) Then
                                    detail = current
                                    CartItemMap.Add(parameters.CartItemId, current.OrderLineNumber)
                                    GoTo Label_0181
                                End If
                            Loop
                        Finally
                            If TypeOf enumerator Is IDisposable Then
                                TryCast(enumerator, IDisposable).Dispose()
                            End If
                        End Try
                    End If
                End If
Label_0181:
                If (detail.Subsystem = "MBR") Then
                    flag = True
                End If
                If (Not detail Is Nothing) Then
                    Dim detail3 As IOrderDetail = detail
                    If ((parameters.Subsystem = "ECD") AndAlso (Not parameters.DCDFiles Is Nothing)) Then
                        Dim fileIDs As Integer() = New Integer(((parameters.DCDFiles.Length - 1) + 1) - 1) {}
                        Dim num7 As Integer = (parameters.DCDFiles.Length - 1)
                        Dim i As Integer = 0
                        Do While (i <= num7)
                            fileIDs(i) = parameters.DCDFiles(i).FileId
                            i += 1
                        Loop
                        detail3.AddDCDFiles(fileIDs)
                    End If
                    If Not detail3.IsLineDirty Then
                        If _
                            ((Not parameters.ShipMasterCustomerId Is Nothing) AndAlso
                             (parameters.ShipMasterCustomerId.Length > 0)) Then
                            detail3.ShipMasterCustomerId = parameters.ShipMasterCustomerId
                            detail3.ShipSubCustomerId = parameters.ShipSubCustomerId
                        End If
                        If ((parameters.Subsystem <> "ECD") AndAlso (parameters.Quantity > 0)) Then
                            detail3.OrderQuantity = parameters.Quantity
                        End If
                        If parameters.IsDirectPriceUpdate Then
                            If (Not parameters.UnitPrice Is Nothing) Then
                                detail3.BaseUnitPrice = Conversions.ToDecimal(parameters.UnitPrice)
                            End If
                            If ((Not parameters.RateStructure Is Nothing) AndAlso (parameters.RateStructure.Length > 0)) _
                                Then
                                detail3.RateStructure =
                                    detail3.RateStructure.List.Item(parameters.RateStructure).ToCodeObject
                            End If
                        End If
                        If _
                            (((Not parameters.RateCode Is Nothing) AndAlso
                              (parameters.RateCode <> detail3.RateCode.Code)) AndAlso (parameters.RateCode.Length > 0)) _
                            Then
                            detail3.RateCode = detail3.RateCode.List.Item(parameters.RateCode).ToCodeObject
                        End If
                        If (Not parameters.MarketCode Is Nothing) Then
                            detail3.MarketCode = parameters.MarketCode
                        End If
                        If (Not parameters.Badges Is Nothing) Then
                            detail3.Badges.RemoveAll()
                            Dim badges As OrderEntryParametersForBadges
                            For Each badges In parameters.Badges
                                Dim badge As IOrderDetailBadge = detail3.ParentDetail.Badges.AddNew
                                badge.OrderLineNumber = detail3.RelatedLineNumber
                                If (badges.BadgeTypeCode Is Nothing) Then
                                    badges.BadgeTypeCode = badge.BadgeTypeCode.List.Item(0).Code.ToString
                                End If
                                If (badges.BadgeTypeCode.Length <= 0) Then
                                    badges.BadgeTypeCode = badge.BadgeTypeCode.List.Item(0).Code.ToString
                                End If
                                badge.BadgeTypeCode = badge.BadgeTypeCode.List.Item(badges.BadgeTypeCode).ToCodeObject
                                badge.FirstName = badges.FirstName
                                badge.FullName = badges.FullName
                                badge.CompanyName = badges.Company
                                badge.City = badges.City
                                badge.State = badges.State
                                badge.PostalCode = badges.PostalCode
                                If ((detail.Subsystem = "MTG") AndAlso (detail.ProductTypeCodeString = "BADGE")) Then
                                    badge.PaidFlag = True
                                End If
                            Next
                        End If
                    End If
                    detail3 = Nothing
                End If
            Next
            masters2.Validate()
            Return masters2
        End Function

        Private Function FindValidationIssueByPartialKey (ByVal VIIssues As IssuesCollection, ByVal KeyToFind As String) _
            As IIssue
            Dim enumerator As IEnumerator
            Dim validationIssueType As String = Me.GetValidationIssueType (KeyToFind)
            Try
                enumerator = VIIssues.GetEnumerator
                Do While enumerator.MoveNext
                    Dim current As IIssue = DirectCast (enumerator.Current, IIssue)
                    If current.Key.Contains (validationIssueType) Then
                        Return current
                    End If
                Loop
            Finally
                If TypeOf enumerator Is IDisposable Then
                    TryCast (enumerator, IDisposable).Dispose
                End If
            End Try
            Return Nothing
        End Function

        Public Overridable Function GetApplicationStates (ByVal CountryCode As String) As IApplicationStates
            If (PersonifyDataCache.Fetch (("GetApplicationStates" & CountryCode)) Is Nothing) Then
                Dim cacheObject As IApplicationStates = DirectCast ([Global].GetCollection (Me.OrganizationId,
                                                                                            Me.OrganizationUnitId,
                                                                                            NamespaceEnum.
                                                                                               ApplicationInfo,
                                                                                            "ApplicationStates"),
                                                                    IApplicationStates)
                cacheObject.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
                Dim states3 As IApplicationStates = cacheObject
                states3.Filter.Add ("ActiveFlag", "Y")
                states3.Filter.Add ("CountryCode", CountryCode)
                states3.Sort (New String() {"StateDescription"})
                states3.Fill
                states3 = Nothing
                PersonifyDataCache.Store (("GetApplicationStates" & CountryCode), cacheObject,
                                          PersonifyDataCache.CacheExpirationInterval)
            End If
            Return _
                DirectCast (PersonifyDataCache.Fetch (("GetApplicationStates" & CountryCode.ToString)),
                            IApplicationStates)
        End Function

        Public Overridable Function GetCheckType() As CheckType()
            Dim codes As IApplicationCodes =
                    New PersonifyConnect (Me.OrganizationId, Me.OrganizationUnitId).GetApplicationCodes ("FAR",
                                                                                                         "CHECK_TYPE",
                                                                                                         True)
            Dim typeArray2 As CheckType() = New CheckType(((codes.Count - 1) + 1) - 1) {}
            Dim index As Integer = 0
            Try
                Dim enumerator As IEnumerator
                Try
                    enumerator = codes.GetEnumerator
                    Do While enumerator.MoveNext
                        Dim current As IApplicationCode = DirectCast (enumerator.Current, IApplicationCode)
                        Dim type As New CheckType
                        type.CheckTypeValue = current.Code
                        type.CheckTypeDescrip = current.Description
                        typeArray2 (index) = type
                        index += 1
                    Loop
                Finally
                    If TypeOf enumerator Is IDisposable Then
                        TryCast (enumerator, IDisposable).Dispose
                    End If
                End Try
            Catch exception1 As Exception
                ProjectData.SetProjectError (exception1)
                Dim exception As Exception = exception1
                Throw exception
            End Try
            Return typeArray2
        End Function

        Public Overridable Function GetCustomerCreditCardOnRecord (ByVal MasterCustomerId As String,
                                                                   ByVal SubCustomerId As Integer) _
            As ICustomerCreditCards
            Dim cards2 As ICustomerCreditCards = DirectCast ([Global].GetCollection (Me.OrganizationId,
                                                                                     Me.OrganizationUnitId,
                                                                                     NamespaceEnum.CustomerInfo,
                                                                                     "CustomerCreditCards"),
                                                             ICustomerCreditCards)
            cards2.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
            cards2.Fill (MasterCustomerId, SubCustomerId, Me.OrganizationId, Me.OrganizationUnitId)
            Return cards2
        End Function

        Public Overridable Function GetCustomerCreditCardOnRecord (ByVal MasterCustomerId As String,
                                                                   ByVal SubCustomerId As Integer,
                                                                   ByVal ValidReceipts As ValidReceiptCodes()) _
            As ICustomerCreditCards
            Dim builder As New StringBuilder
            Dim num2 As Integer = (ValidReceipts.Length - 1)
            Dim i As Integer = 0
            Do While (i <= num2)
                If (i = 0) Then
                    builder.Append ("receipt_Type_Code in ( '")
                    builder.Append (ValidReceipts (i).ReceiptCode)
                    builder.Append ("'")
                Else
                    builder.Append (", '")
                    builder.Append (ValidReceipts (i).ReceiptCode)
                    builder.Append ("'")
                End If
                i += 1
            Loop
            If Not String.IsNullOrEmpty (builder.ToString) Then
                builder.Append (" )")
            End If
            Dim cards2 As ICustomerCreditCards = DirectCast ([Global].GetCollection (Me.OrganizationId,
                                                                                     Me.OrganizationUnitId,
                                                                                     NamespaceEnum.CustomerInfo,
                                                                                     "CustomerCreditCards"),
                                                             ICustomerCreditCards)
            cards2.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
            If (builder.Length > 0) Then
                cards2.Filter.Add (New FilterItem (builder.ToString))
            End If
            cards2.Filter.Add ("MasterCustomerId", MasterCustomerId)
            cards2.Filter.Add ("SubCustomerId", Conversions.ToString (SubCustomerId))
            cards2.Filter.Add ("OrganizationId", Me.OrganizationId)
            cards2.Filter.Add ("OrganizationUnitId", Me.OrganizationUnitId)
            cards2.Fill
            Return cards2
        End Function

        Private Function GetDefaultShipViaCodeForWeb() As String
            Return _
                CachedApplicationData.ApplicationDataCache.ApplicationOrganizationUnits(Me.OrganizationId,
                                                                                             Me.OrganizationUnitId).Item(
                                                                                                 0).WebDomesticShipVia.
                    Code
        End Function

        Public Overridable Function GetECheckCodeList() As List(Of String)
            Dim list As New List(Of String)
            Dim errorMessage As String = String.Empty
            Dim validReceiptsForECommerceBatch As ValidReceiptCodes() =
                    Me.GetValidReceiptsForECommerceBatch (errorMessage)
            If (Not validReceiptsForECommerceBatch Is Nothing) Then
                Dim codes As ValidReceiptCodes
                For Each codes In validReceiptsForECommerceBatch
                    If (codes.ReceiptType = "ECHK") Then
                        list.Add (codes.ReceiptCode)
                    End If
                Next
            End If
            Return list
        End Function

        Public Overridable Function GetECommerceBatch (ByRef ErrorMessage As String) As IARBatches
            Dim flag As Boolean
            Dim batches2 As IARBatches
            Dim builder As New StringBuilder
            Dim units As IApplicationOrganizationUnits =
                    CachedApplicationData.ApplicationDataCache.ApplicationOrganizationUnits(Me.OrganizationId,
                                                                                                 Me.OrganizationUnitId)
            If (units.Count > 0) Then
                batches2 = units.Item (0).SetCurrentECommerceBatch
                If (batches2.ValidationIssues.Count > 0) Then
                    flag = True
                Else
                    flag = False
                End If
                If flag Then
                    Dim num2 As Integer = (batches2.ValidationIssues.Count - 1)
                    Dim i As Integer = 0
                    Do While (i <= num2)
                        builder.Append (batches2.ValidationIssues.Item (i).Message)
                        builder.Append ("<br/>")
                        i += 1
                    Loop
                    ErrorMessage = builder.ToString
                End If
            Else
                batches2 = Nothing
            End If
            If flag Then
                Return Nothing
            End If
            Return batches2
        End Function

        Public Overridable Function GetOneClickCustomer (ByVal ocps As OneClickDonationParameters,
                                                         ByVal Optional CreateNewCustomer As Boolean = True) _
            As ICustomers
            Dim oCustomers As ICustomers = DirectCast ([Global].GetCollection (Me.OrganizationId, Me.OrganizationUnitId,
                                                                               NamespaceEnum.CustomerInfo, "Customers"),
                                                       ICustomers)
            oCustomers.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
            If (ocps.MCID.Length > 0) Then
                oCustomers.Fill (ocps.MCID, ocps.SCID)
                Return oCustomers
            End If
            If CreateNewCustomer Then
                Dim customer As ICustomer = oCustomers.AddNew
                customer.FirstName = ocps.FirstName
                customer.LastName = ocps.LastName
                customer.RecordType = "I"
                customer.CustomerClassCode = customer.CustomerClassCode.List.Item ("INDIV").ToCodeObject
                Dim detail As ICustomerAddressDetail = customer.CreateDefaultAddress
                detail.AddressTypeCode = detail.AddressTypeCode.List.Item (ocps.AddressTypeCode).ToCodeObject
                detail.LabelName = customer.LabelName
                detail.ShipToFlag = True
                detail.BillToFlag = True
                detail.DirectoryFlag = False
                detail.AddressStatusCode = detail.AddressStatusCode.List.Item ("GOOD").ToCodeObject
                Dim address As ICustomerAddress = detail.Address
                address.CountryCode = address.CountryCode.List.Item (ocps.AddrCountryCode).ToCodeObject
                address.Address1 = ocps.AddrLine1
                address.Address2 = ocps.AddrLine2
                address.Address3 = ocps.AddrLine3
                address.Address4 = ocps.AddrLine4
                address.AddressStatusCode = address.AddressStatusCode.List.Item ("GOOD").ToCodeObject
                address.City = ocps.City
                address.IsAddressValidationAutoRespond = True
                If (String.Compare (ocps.State, "[ALL]") <> 0) Then
                    If (ocps.State = "-1") Then
                        address.State = ""
                    Else
                        address.State = ocps.State
                    End If
                End If
                address.PostalCode = ocps.ZipCode
                Dim communication As ICustomerCommunication = customer.Communications.AddNew
                communication.CommTypeCode = communication.CommTypeCode.List.Item ("EMAIL").ToCodeObject
                communication.CommLocationCode = communication.CommLocationCode.List.Item ("HOME").ToCodeObject
                communication.FormattedPhoneAddress = ocps.EMail
                communication.CountryCode = ocps.AddrCountryCode
                communication.PrimaryFlag = True
                customer = Nothing
                Dim customers3 As spReturnGetPotentialMatchCustomers = Me.GetPotentialMatchCustomers (oCustomers,
                                                                                                      ocps.FirstName,
                                                                                                      ocps.LastName,
                                                                                                      ocps.EMail,
                                                                                                      ocps.State,
                                                                                                      ocps.City,
                                                                                                      address.
                                                                                                         FormattedAddress)
                If (customers3.DuplicateMatchProbability = "100") Then
                    oCustomers = Nothing
                    oCustomers = DirectCast ([Global].GetCollection (Me.OrganizationId, Me.OrganizationUnitId,
                                                                     NamespaceEnum.CustomerInfo, "Customers"),
                                             ICustomers)
                    oCustomers.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
                    oCustomers.Fill (customers3.DuplicateCustomers (0).MCID, customers3.DuplicateCustomers (0).SCID)
                    Return oCustomers
                End If
                If (customers3.DuplicateMatchProbability <> "0") Then
                    oCustomers.Item (0).PotentialDuplicateReviewFlag = True
                    oCustomers.Item (0).DuplicateMatchProbability =
                        Conversions.ToDecimal (customers3.DuplicateMatchProbability)
                    oCustomers.Item (0).DuplicateReason = customers3.DuplicateReason
                    Dim customers4 As spDuplicateCustomers
                    For Each customers4 In customers3.DuplicateCustomers
                        Dim duplicate As ICustomerPotentialOneclickDuplicate =
                                oCustomers.Item (0).CustomerPotentialOneClickDuplicates.AddNew
                        duplicate.DuplicateMasterCustomerId = customers4.MCID
                        duplicate.DuplicateSubCustomerId = customers4.SCID
                        duplicate = Nothing
                    Next
                End If
                oCustomers.Item (0).OverrideDuplicateCheck = True
                Me.OverRideOneclickCustomerValidationIssues (oCustomers)
                oCustomers.Save
            End If
            Return oCustomers
        End Function

        Private Function GetPotentialMatchCustomers (ByVal oCustomers As ICustomers, ByVal FirstName As String,
                                                     ByVal LastName As String, ByVal Email As String,
                                                     ByVal City As String, ByVal State As String,
                                                     ByVal FormattedAddress As String) _
            As spReturnGetPotentialMatchCustomers
            Dim customers2 As New spReturnGetPotentialMatchCustomers
            Dim strArray As String() = oCustomers.GetPotentialMatchCustomers (LastName, FirstName, Email, State, City,
                                                                              FormattedAddress)
            customers2.DuplicateMatchProbability = strArray (0)
            customers2.DuplicateReason = strArray (1)
            Dim customersArray As spDuplicateCustomers() =
                    New spDuplicateCustomers(((strArray (2).Split (New Char() {","c}).Length - 1) + 1) - 1) {}
            Dim index As Integer = 0
            If (strArray (2).Length > 0) Then
                Dim str As String
                For Each str In strArray (2).Split (New Char() {","c})
                    customersArray (index) = New spDuplicateCustomers
                    customersArray (index).MCID = str.Split (New Char() {"-"c}) (0)
                    customersArray (index).SCID = Convert.ToInt16 (str.Split (New Char() {"-"c}) (1))
                    index += 1
                Next
            End If
            customers2.DuplicateCustomers = customersArray
            Return customers2
        End Function

        Private Function GetValidationIssueType (ByVal strKey As String) As String
            Return strKey.Split (New Char() {":"c}) (1)
        End Function

        Public Overridable Function GetValidReceiptsForECommerceBatch (ByRef ErrorMessage As String) _
            As ValidReceiptCodes()
            Dim cacheObject As ValidReceiptCodes() = Nothing
            Dim index As Integer = 0
            Dim eCommerceBatch As IARBatches = Me.GetECommerceBatch (ErrorMessage)
            If (eCommerceBatch Is Nothing) Then
                Return Nothing
            End If
            If (eCommerceBatch.Count <= 0) Then
                Return cacheObject
            End If
            Dim key As String =
                    ([Global].FinancialBatches.Item (Me.OrganizationId, Me.OrganizationUnitId).BatchId &
                     Me._PortalCurrency.Code & "ValidReceiptTypes")
            If (PersonifyDataCache.Fetch (key) Is Nothing) Then
                If (eCommerceBatch.Item (0).Details.Count > 0) Then
                    Dim enumerator As IEnumerator
                    cacheObject =
                        New ValidReceiptCodes _
                            (
                                ((eCommerceBatch.Item (0).Details.FindAll ("CurrencyCode", Me._PortalCurrency.Code).
                                      Length - 1) + 1) - 1) {}
                    Try
                        enumerator = eCommerceBatch.Item (0).Details.GetEnumerator
                        Do While enumerator.MoveNext
                            Dim current As IARBatchDetail = DirectCast (enumerator.Current, IARBatchDetail)
                            If (current.CurrencyCodeString = Me._PortalCurrency.Code) Then
                                cacheObject (index) = New ValidReceiptCodes
                                cacheObject (index).ReceiptCode = current.ReceiptTypeCode.Code
                                cacheObject (index).ReceiptDescription = current.ReceiptTypeInfo.WebDescription
                                If current.ReceiptTypeInfo.CreditCardReceiptFlag Then
                                    cacheObject (index).ReceiptType = "CC"
                                End If
                                If current.ReceiptTypeInfo.eCheckReceiptFlag Then
                                    cacheObject (index).ReceiptType = "ECHK"
                                End If
                                index += 1
                            End If
                        Loop
                    Finally
                        If TypeOf enumerator Is IDisposable Then
                            TryCast (enumerator, IDisposable).Dispose
                        End If
                    End Try
                    PersonifyDataCache.Store (key, cacheObject, PersonifyDataCache.CacheExpirationInterval)
                    Return cacheObject
                End If
                Return Nothing
            End If
            Return DirectCast (PersonifyDataCache.Fetch (key), ValidReceiptCodes())
        End Function

        Public Overridable Function OneClickDonation(ByVal OneClickParams As OneClickDonationParameters,
                                                            Optional ByVal CustomerValidationIssues As IssuesCollection = Nothing,
                                                            Optional ByVal OrderValidationIssues As IssuesCollection = Nothing,
                                                            Optional ByVal FarValidationIssues As IssuesCollection = Nothing) As ReturnStatusOneClickDonations
            If CustomerValidationIssues Is Nothing Then
                CustomerValidationIssues = New IssuesCollection()
            End If

            If OrderValidationIssues Is Nothing Then
                OrderValidationIssues = New IssuesCollection()
            End If
            
            If FarValidationIssues Is Nothing Then
                FarValidationIssues = New IssuesCollection()
            End If
            

            Dim receiptValidationIssues As IssuesCollection = Nothing
            Dim donations2 As New ReturnStatusOneClickDonations
            Dim oneClickCustomer As ICustomers = Me.GetOneClickCustomer(OneClickParams, True)
            If (oneClickCustomer.ValidationIssues.ErrorCount > 0) Then
                donations2.DonationSuccessFull = False
                donations2.oCustomers = oneClickCustomer
                Return donations2
            End If
            donations2.oCustomers = oneClickCustomer
            Dim orderMasters As IOrderMasters = DirectCast([Global].GetCollection(Me.OrganizationId,
                                                                                    Me.OrganizationUnitId,
                                                                                    NamespaceEnum.OrderInfo,
                                                                                    "OrderMasters"), 
                                                            IOrderMasters)
            orderMasters.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
            orderMasters.CreateNewOrder(oneClickCustomer.Item(0).MasterCustomerId,
                                         oneClickCustomer.Item(0).SubCustomerId, CInt(OneClickParams.PID),
                                         OneClickParams.RateStructure, OneClickParams.RateCode, Nothing, 1, Nothing, 0)
            If ((orderMasters.Count > 0) AndAlso (orderMasters.Item(0).Details.Count > 0)) Then
                orderMasters.Item(0).OrderMethodCode =
                    orderMasters.Item(0).OrderMethodCode.List.Item("WEB").ToCodeObject
                orderMasters.Item(0).CurrencyCode.Code = Me._PortalCurrency.Code
                If (Decimal.Compare(OneClickParams.AmountToDonate, Decimal.Zero) > 0) Then
                    orderMasters.Item(0).Details.Item(0).DisplayUnitPrice = OneClickParams.AmountToDonate
                Else
                    orderMasters.Item(0).Details.Item(0).RateStructure.Code = OneClickParams.RateStructure
                    orderMasters.Item(0).Details.Item(0).RateCode.Code = OneClickParams.RateCode
                End If
                If OneClickParams.IsRecurringDonation Then
                    orderMasters.Item(0).Details.Item(0).OrderFundRaisingDetail.RecurringGiftFlag = True
                    orderMasters.Item(0).Details.Item(0).OrderFundRaisingDetail.GiftFrequencyCode.Code =
                        OneClickParams.RecurringDonationFrequency
                End If
            End If
            If (Not OrderValidationIssues Is Nothing) Then
                orderMasters.Validate()
                If (orderMasters.ValidationIssues.ErrorCount > 0) Then
                    Dim validationIssues As IssuesCollection = DirectCast(orderMasters.ValidationIssues, 
                                                                           IssuesCollection)
                    Me.RespondToValidationIssuesUsingPartialKey(validationIssues, OrderValidationIssues)
                End If
            End If
            Dim flag As Boolean = Me.SaveOrderHelper(orderMasters, OneClickParams.CCNo, OneClickParams.CCType,
                                                      OneClickParams.ExpMonth, OneClickParams.ExpYear,
                                                      OneClickParams.CVV2, OneClickParams.NameOnCard, False,
                                                      receiptValidationIssues, OneClickParams.RoutingNumber,
                                                      OneClickParams.AccountNumber, OneClickParams.CheckNumber,
                                                      OneClickParams.CheckType, OneClickParams.DriversLicense,
                                                      OneClickParams.DriversLicenseState, OneClickParams.FederalTaxId,
                                                      OneClickParams.DateOfBirth, OneClickParams.IDType, False, "",
                                                      OneClickParams)
            donations2.DonationSuccessFull = flag
            donations2.oOrderMasters = orderMasters
            donations2.oFARIssues = receiptValidationIssues
            Return donations2
        End Function

        Private Sub OverRideOneclickCustomerValidationIssues (ByVal oCustomers As ICustomers)
            oCustomers.Validate
            If (oCustomers.ValidationIssues.ErrorCount > 0) Then
                Dim num2 As Integer = (oCustomers.ValidationIssues.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num2)
                    If _
                        (oCustomers.ValidationIssues.Item (i).ResponseRequired AndAlso
                         oCustomers.ValidationIssues.Item (i).Key.Contains ("EmailExistAsPrimaryForAnotherCustomerIssue")) _
                        Then
                        oCustomers.ValidationIssues.Item (i).Responses.SelectedResponse =
                            oCustomers.ValidationIssues.Item (i).Responses.Item ("Yes")
                    End If
                    i += 1
                Loop
            End If
        End Sub

        Private Sub RespondToValidationIssuesUsingPartialKey (ByRef UnRespondedVIs As IssuesCollection,
                                                              ByRef RespondedVIs As IssuesCollection)
            Dim enumerator As IEnumerator
            Try
                enumerator = UnRespondedVIs.GetEnumerator
                Do While enumerator.MoveNext
                    Dim current As IIssue = DirectCast (enumerator.Current, IIssue)
                    If current.ResponseRequired Then
                        Dim issue As IIssue = Nothing
                        issue = Me.FindValidationIssueByPartialKey (RespondedVIs, current.Key)
                        If ((Not issue Is Nothing) AndAlso (Not issue.Responses.SelectedResponse Is Nothing)) Then
                            current.Responses.SelectedResponse = issue.Responses.SelectedResponse
                        End If
                    End If
                Loop
            Finally
                If TypeOf enumerator Is IDisposable Then
                    TryCast (enumerator, IDisposable).Dispose
                End If
            End Try
        End Sub

        Private Function ReturnReceiptCollectionForOrder(ByRef OrderMasters As IOrderMasters,
                                                          ByVal CreditCardNo As String, ByVal PaymentType As String,
                                                          ByVal ExpirationMonth As Integer,
                                                          ByVal ExpirationYear As Integer, ByVal CVV2Number As String,
                                                          ByVal Name As String, ByVal MakeCCOfRecord As Boolean,
                                                          ByVal UsePreferredCreditCard As Boolean,
                                                          ByVal BankRoutingNumber As String,
                                                          ByVal BankAccountNumber As String, ByVal CheckNumber As String,
                                                          ByVal CheckType As String,
                                                          ByVal DriversLicenseNumber As String,
                                                          ByVal DriversLicenseState As String,
                                                          ByVal FederalTaxId As String, ByVal DateOfBirth As String,
                                                          ByVal IDType As String, ByRef Approved As Boolean,
                                                                Optional ByVal ocps As OneClickDonationParameters = Nothing,
                                                                Optional ByVal NewOrderFlag As Boolean = True) As IARReceipts
            If ocps Is Nothing Then
                ocps = New OneClickDonationParameters()
            End If


            Dim enumerator As IEnumerator
            Dim time As New DateTime
            Dim receipts As IARReceipts = DirectCast([Global].GetCollection(Me.OrganizationId, Me.OrganizationUnitId,
                                                                              NamespaceEnum.AccountingInfo, "ARReceipts"), 
                                                      IARReceipts)
            receipts.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
            If UsePreferredCreditCard Then
                Dim customerCreditCardOnRecord As ICustomerCreditCards =
                        Me.GetCustomerCreditCardOnRecord(OrderMasters.Item(0).BillMasterCustomerId,
                                                          OrderMasters.Item(0).BillSubCustomerId)
                If (customerCreditCardOnRecord.Count > 0) Then
                    If Not NewOrderFlag Then
                        receipts.AddNew()
                    Else
                        receipts.CreateReceiptForUnsavedOrder(OrderMasters,
                                                               customerCreditCardOnRecord.Item(0).ReceiptTypeCode.Code)
                    End If
                End If
            ElseIf Not NewOrderFlag Then
                receipts.AddNew()
            Else
                receipts.CreateReceiptForUnsavedOrder(OrderMasters, PaymentType)
            End If
            Try
                enumerator = OrderMasters.Item(0).Details.GetEnumerator
                Do While enumerator.MoveNext
                    Dim current As IOrderDetail = DirectCast(enumerator.Current, IOrderDetail)
                    If current.IsBillMe Then
                        Dim num2 As Integer = (receipts.Item(0).Transactions.Count - 1)
                        Dim i As Integer = 0
                        Do While (i <= num2)
                            If _
                                ((receipts.Item(0).Transactions.Item(i).OrderNumber = current.OrderNumber) AndAlso
                                 (receipts.Item(0).Transactions.Item(i).OrderLineNumber = current.OrderLineNumber)) _
                                Then
                                receipts.Item(0).Transactions.RemoveAt(i)
                                Exit Do
                            End If
                            i += 1
                        Loop
                    End If
                Loop
            Finally
                If TypeOf enumerator Is IDisposable Then
                    TryCast(enumerator, IDisposable).Dispose()
                End If
            End Try
            Dim receipt As IARReceipt = receipts.Item(0)
            Dim receipt2 As IARReceipt = receipt
            receipt2.BillMasterCustomerId = OrderMasters.Item(0).BillMasterCustomerId
            receipt2.BillSubCustomerId = OrderMasters.Item(0).BillSubCustomerId
            receipt2.MasterCustomerId = OrderMasters.Item(0).BillMasterCustomerId
            receipt2.SubCustomerId = OrderMasters.Item(0).BillSubCustomerId
            receipt2.ActualAmount = OrderMasters.Item(0).TotalWebBillMeNowOrderAmount
            If UsePreferredCreditCard Then
                If receipt2.PayorHasCreditCardOfRecord Then
                    receipt2.RetrieveCreditCardOfRecord()
                End If
                If (CVV2Number.Length > 0) Then
                    receipt2.Cvv2Number = CVV2Number
                End If
                time =
                    DateTime.Parse((ExpirationMonth.ToString & "/01/" & ExpirationYear.ToString)).AddMonths(1).AddDays(
                        -1)
                receipt2.CCName = Name
                receipt2.CCExpirationDate = time
            Else
                If ((ExpirationMonth <> 0) AndAlso (-((ExpirationYear <> 0) > False) > False)) Then
                    time =
                        DateTime.Parse((ExpirationMonth.ToString & "/01/" & ExpirationYear.ToString)).AddMonths(1).
                            AddDays(-1)
                End If
                receipt2.ReceiptTypeCode = receipt2.ReceiptTypeCode.List.Item(PaymentType).ToCodeObject
                receipt2.CCReferenceDecrypted = CreditCardNo
                receipt2.CCExpirationDate = time
                If (CVV2Number.Length > 0) Then
                    receipt2.Cvv2Number = CVV2Number
                End If
                If (ocps Is Nothing) Then
                    receipt2.CCName = Name
                    receipt2.CCAddress1 = OrderMasters.Item(0).BillAddressInfo.Address1
                    receipt2.CCCity = OrderMasters.Item(0).BillAddressInfo.City
                    receipt2.CCState = OrderMasters.Item(0).BillAddressInfo.State
                    receipt2.CCPostalCode = OrderMasters.Item(0).BillAddressInfo.PostalCode
                    receipt2.CCCountryCode = OrderMasters.Item(0).BillAddressInfo.CountryCode.Code
                Else
                    receipt2.CCName = ocps.NameOnCard
                    receipt2.CCAddress1 = ocps.AddrLine1
                    receipt2.CCCity = ocps.City
                    receipt2.CCState = ocps.State
                    receipt2.CCPostalCode = ocps.ZipCode
                    receipt2.CCCountryCode = ocps.AddrCountryCode
                End If
            End If
            receipt2.BankRoutingNumber = BankRoutingNumber
            receipt2.BankAccountNumber = BankAccountNumber
            receipt2.PersonalIdentifier = IDType
            receipt2.DriversLicenseStateCode = DriversLicenseState
            receipt2.DriversLicenseNumber = DriversLicenseNumber
            receipt2.FederalTaxId = FederalTaxId
            If Not String.IsNullOrEmpty(DateOfBirth) Then
                receipt2.BirthDate = Conversions.ToDate(DateOfBirth)
            End If
            If Not (CheckType Is "") Then
                receipt2.CheckTypeCode = receipt2.CheckTypeCode.List.Item(CheckType).ToCodeObject
            End If
            receipt2.CheckReference = CheckNumber
            If Me.DoCreditCardAuthorization Then
                If (receipt2.CCAuthorization.Length = 0) Then
                    Approved = receipt2.AuthorizeCreditCard
                    If _
                        ((Not NewOrderFlag AndAlso (receipt2.ValidationIssues.Count = 1)) AndAlso
                         receipt2.ValidationIssues.Item(0).Key.Contains(
                             "TIMSS.API.Base.AccountingInfo.CreateUnappliedReceiptTransactionIssue")) Then
                        receipt2.ValidationIssues.Item(0).Responses.SelectedResponse =
                            receipt2.ValidationIssues.Item(0).Responses.Item(0)
                        Approved = receipt2.AuthorizeCreditCard
                    End If
                End If
            Else
                receipt2.ReceiptStatusCode = receipt2.ReceiptStatusCode.List.Item("A").ToCodeObject
            End If
            receipt2 = Nothing
            Return receipts
        End Function

        Public Overridable Function SaveOrder (ByRef OrderMasters As IOrderMasters,
                                               ByVal Optional PurchaseOrderNumber As String = "") As Boolean
            If _
                ((PurchaseOrderNumber.Length > 0) AndAlso
                 ((Not OrderMasters Is Nothing) AndAlso (OrderMasters.Count > 0))) Then
                OrderMasters.Item (0).PONumber = PurchaseOrderNumber
            End If
            OrderMasters.Save
            If (OrderMasters.ValidationIssues.Count > 0) Then
                Return False
            End If
            Return True
        End Function

        Public Overridable Function SaveOrder(ByRef OrderMasters As IOrderMasters, ByVal CreditCardNo As String,
                                               ByVal CreditCardType As String, ByVal ExpirationMonth As Integer,
                                               ByVal ExpirationYear As Integer, ByVal CVV2Number As String,
                                               ByVal NameOnCard As String, ByVal MakeCCOfRecord As Boolean,
                                               ByRef ReceiptValidationIssues As IssuesCollection,
                                                     Optional ByVal UsePreferredCreditCard As Boolean = False,
                                                     Optional ByVal PurchaseOrderNumber As String = "",
                                                     Optional ByVal ocps As OneClickDonationParameters = Nothing) As Boolean
            If ocps Is Nothing Then
                ocps = New OneClickDonationParameters()
            End If


            Return _
                Me.SaveOrderHelper(OrderMasters, CreditCardNo, CreditCardType, ExpirationMonth, ExpirationYear,
                                    CVV2Number, NameOnCard, MakeCCOfRecord, ReceiptValidationIssues, "", "", "", "", "",
                                    "", "", "", "", UsePreferredCreditCard, PurchaseOrderNumber, ocps)
        End Function

        Public Overridable Function SaveOrder(ByVal PaymentType As String, ByVal AccountName As String,
                                               ByVal BankRoutingNumber As String, ByVal BankAccountNumber As String,
                                               ByVal CheckNumber As String, ByVal CheckType As String,
                                               ByVal DriversLicenseNumber As String, ByVal DriversLicenseState As String,
                                               ByVal FederalTaxId As String, ByVal DateOfBirth As String,
                                               ByVal IDType As String, ByRef oOrderMasters As IOrderMasters,
                                               ByRef ReceiptValidationIssues As IssuesCollection,
                                                     Optional ByVal PurchaseOrderNumber As String = "",
                                                     Optional ByVal ocps As OneClickDonationParameters =Nothing) As Boolean

            If ocps Is Nothing Then
                ocps = New OneClickDonationParameters()
            End If

            Return _
                Me.SaveOrderHelper(oOrderMasters, "", PaymentType, 0, 0, "", AccountName, False,
                                    ReceiptValidationIssues, BankRoutingNumber, BankAccountNumber, CheckNumber,
                                    CheckType, DriversLicenseNumber, DriversLicenseState, FederalTaxId, DateOfBirth,
                                    IDType, False, PurchaseOrderNumber, ocps)
        End Function

        Public Overridable Function SaveOrderHelper(ByRef OrderMasters As IOrderMasters, ByVal CreditCardNo As String,
                                                     ByVal PaymentType As String, ByVal ExpirationMonth As Integer,
                                                     ByVal ExpirationYear As Integer, ByVal CVV2Number As String,
                                                     ByVal Name As String, ByVal MakeCCOfRecord As Boolean,
                                                     ByRef ReceiptValidationIssues As IssuesCollection,
                                                     ByVal BankRoutingNumber As String,
                                                     ByVal BankAccountNumber As String, ByVal CheckNumber As String,
                                                     ByVal CheckType As String, ByVal DriversLicenseNumber As String,
                                                     ByVal DriversLicenseState As String, ByVal FederalTaxId As String,
                                                     ByVal DateOfBirth As String, ByVal IDType As String,
                                                           Optional ByVal UsePreferredCreditCard As Boolean = False,
                                                           Optional ByVal PurchaseOrderNumber As String = "",
                                                           Optional ByVal ocps As OneClickDonationParameters = Nothing) As Boolean
            If ocps Is Nothing Then
                ocps = New OneClickDonationParameters()
            End If

            Dim flag2 As Boolean
            Dim receipts As IARReceipts = Nothing
            Dim enumerator As IEnumerator
            Dim errorMessage As String = ""
            Dim flag3 As Boolean = False
            Dim approved As Boolean = True
            Dim flag4 As Boolean = False
            If _
                ((PurchaseOrderNumber.Length > 0) AndAlso
                 ((Not OrderMasters Is Nothing) AndAlso (OrderMasters.Count > 0))) Then
                OrderMasters.Item(0).PONumber = PurchaseOrderNumber
            End If
            Dim newOrderFlag As Boolean = True
            Try
                enumerator = OrderMasters.Item(0).Details.GetEnumerator
                Do While enumerator.MoveNext
                    Dim current As IOrderDetail = DirectCast(enumerator.Current, IOrderDetail)
                    If (Decimal.Compare(current.PaidAmount, Decimal.Zero) > 0) Then
                        newOrderFlag = False
                        OrderMasters.Save()
                    End If
                Loop
            Finally
                If TypeOf enumerator Is IDisposable Then
                    TryCast(enumerator, IDisposable).Dispose()
                End If
            End Try
            Dim eCommerceBatch As IARBatches = Me.GetECommerceBatch(errorMessage)
            If newOrderFlag Then
                flag3 = (Decimal.Compare(OrderMasters.Item(0).TotalWebBillMeNowOrderAmount, Decimal.Zero) = 0)
            Else
                OrderMasters.Item(0).Transactions.Refresh(False)
                flag3 = (Decimal.Compare(OrderMasters.Item(0).OrderBalance, Decimal.Zero) = 0)
            End If
            If Not flag3 Then
                receipts = Me.ReturnReceiptCollectionForOrder(OrderMasters, CreditCardNo, PaymentType, ExpirationMonth,
                                                               ExpirationYear, CVV2Number, Name, MakeCCOfRecord,
                                                               UsePreferredCreditCard, BankRoutingNumber,
                                                               BankAccountNumber, CheckNumber, CheckType,
                                                               DriversLicenseNumber, DriversLicenseState, FederalTaxId,
                                                               DateOfBirth, IDType, approved, ocps, newOrderFlag)
                If (receipts.ValidationIssues.Count > 0) Then
                    ReceiptValidationIssues = DirectCast(receipts.ValidationIssues, IssuesCollection)
                    Return flag4
                End If
                If ((receipts.Item(0).CCAuthorization.Length > 0) OrElse approved) Then
                    flag2 = True
                ElseIf Me.DoCreditCardAuthorization Then
                    ReceiptValidationIssues = DirectCast(receipts.ValidationIssues, IssuesCollection)
                    Return flag4
                End If
            Else
                flag2 = True
            End If
            If Not Me.DoCreditCardAuthorization Then
                flag2 = True
            End If
            If Not (flag2 Or flag3) Then
                Return flag4
            End If
            Dim flag5 As Boolean = OrderMasters.Save
            If (OrderMasters.ValidationIssues.Count > 0) Then
                Return flag4
            End If
            If Not flag3 Then
                Dim enumerator2 As IEnumerator
                Dim enumerator3 As IEnumerator
                Try
                    enumerator2 = OrderMasters.Item(0).Details.GetEnumerator
                    Do While enumerator2.MoveNext
                        Dim oDetail As IOrderDetail = DirectCast(enumerator2.Current, IOrderDetail)
                        If _
                            (Not oDetail.IsBillMe AndAlso
                             ((Decimal.Compare(oDetail.BaseTotalAmount, Decimal.Zero) > 0) And
                              (Decimal.Compare(oDetail.PaidAmount, Decimal.Zero) = 0))) Then
                            If Not newOrderFlag Then
                                receipts.Item(0).AddOrderLineToPayWithReceipt(oDetail)
                            End If
                            receipts.Item(0).ApplyCash(oDetail.BaseTotalAmount, oDetail.OrderNumber,
                                                         oDetail.OrderLineNumber, False)
                        End If
                    Loop
                Finally
                    If TypeOf enumerator2 Is IDisposable Then
                        TryCast(enumerator2, IDisposable).Dispose()
                    End If
                End Try
                Try
                    enumerator3 = receipts.Item(0).Transactions.GetEnumerator
                    Do While enumerator3.MoveNext
                        Dim transaction As IARTransaction = DirectCast(enumerator3.Current, IARTransaction)
                        If _
                            ((Not transaction.Children Is Nothing) AndAlso
                             (Not _
                              DirectCast(transaction.Children.Item("OrderLineInfo"), IBusinessObjectCollection) Is
                              Nothing)) Then
                            DirectCast(transaction.Children.Item("OrderLineInfo"), IBusinessObjectCollection).IsFilled _
                                = False
                        End If
                    Loop
                Finally
                    If TypeOf enumerator3 Is IDisposable Then
                        TryCast(enumerator3, IDisposable).Dispose()
                    End If
                End Try
                receipts.Save()
                If (receipts.ValidationIssues.Count = 0) Then
                    If (MakeCCOfRecord AndAlso receipts.Item(0).PayorCanMakeThisCreditCardOfRecord) Then
                        receipts.Item(0).SaveCreditCardOfRecord()
                    End If
                    PersonifyDataCache.Remove(
                        ("GetCustomerCreditCardOnRecord" & OrderMasters.Item(0).BillMasterCustomerId.ToString &
                         OrderMasters.Item(0).BillSubCustomerId.ToString))
                    Return True
                End If
                ReceiptValidationIssues = DirectCast(receipts.ValidationIssues, IssuesCollection)
                Return flag4
            End If
            Return True
        End Function

        Public Overridable Function SetBillMeLaterFlagAtOrderLineLevel (ByVal OrderNumber As String,
                                                                        ByVal OrderLineNumber As Integer,
                                                                        ByVal BillMeLaterFlag As Boolean,
                                                                        ByVal OrderMasters As IOrderMasters) _
            As IOrderMasters
            If (OrderMasters.Count > 0) Then
                Dim enumerator As IEnumerator
                Try
                    enumerator = OrderMasters.Item (0).Details.GetEnumerator
                    Do While enumerator.MoveNext
                        Dim current As IOrderDetail = DirectCast (enumerator.Current, IOrderDetail)
                        If (current.OrderLineNumber = OrderLineNumber) Then
                            current.IsBillMe = BillMeLaterFlag
                            Return OrderMasters
                        End If
                    Loop
                Finally
                    If TypeOf enumerator Is IDisposable Then
                        TryCast (enumerator, IDisposable).Dispose
                    End If
                End Try
            End If
            Return OrderMasters
        End Function

        Public Overridable Function SetCouponCodeAtOrderLineLevel (ByVal OrderNumber As String,
                                                                   ByVal OrderLineNumber As Integer,
                                                                   ByVal CouponCode As String,
                                                                   ByVal OrderMasters As IOrderMasters) As IOrderMasters
            If (OrderMasters.Count > 0) Then
                Dim enumerator As IEnumerator
                Try
                    enumerator = OrderMasters.Item (0).Details.GetEnumerator
                    Do While enumerator.MoveNext
                        Dim current As IOrderDetail = DirectCast (enumerator.Current, IOrderDetail)
                        If (current.OrderLineNumber = OrderLineNumber) Then
                            current.CouponCode = CouponCode
                            Return OrderMasters
                        End If
                    Loop
                Finally
                    If TypeOf enumerator Is IDisposable Then
                        TryCast (enumerator, IDisposable).Dispose
                    End If
                End Try
            End If
            Return OrderMasters
        End Function

        Public Overridable Function SetDefaultShipViaForEntireOrder (ByVal OrderMasters As IOrderMasters) _
            As IOrderMasters
            Return Me.SetShipViaCodeForOrder (Me.GetDefaultShipViaCodeForWeb, OrderMasters, - 1)
        End Function

        Public Overridable Function SetMarketCodeAtOrderLevel (ByVal MarketCode As String,
                                                               ByVal OrderMasters As IOrderMasters) As IOrderMasters
            If (OrderMasters.Count > 0) Then
                OrderMasters.Item (0).MarketCode = MarketCode
            End If
            Return OrderMasters
        End Function

        Public Overridable Function SetMarketCodeAtOrderLineLevel (ByVal OrderNumber As String,
                                                                   ByVal OrderLineNumber As Integer,
                                                                   ByVal MarketCode As String,
                                                                   ByVal OrderMasters As IOrderMasters) As IOrderMasters
            If (OrderMasters.Count > 0) Then
                Dim enumerator As IEnumerator
                Try
                    enumerator = OrderMasters.Item (0).Details.GetEnumerator
                    Do While enumerator.MoveNext
                        Dim current As IOrderDetail = DirectCast (enumerator.Current, IOrderDetail)
                        If (current.OrderLineNumber = OrderLineNumber) Then
                            current.MarketCode = MarketCode
                            Return OrderMasters
                        End If
                    Loop
                Finally
                    If TypeOf enumerator Is IDisposable Then
                        TryCast (enumerator, IDisposable).Dispose
                    End If
                End Try
            End If
            Return OrderMasters
        End Function

        Public Overridable Function SetResetBillPrimaryEmployer (ByVal BillPrimaryEmployer As Boolean,
                                                                 ByVal OrderMasters As IOrderMasters) As IOrderMasters
            If Not BillPrimaryEmployer Then
                OrderMasters.Item (0).BillMasterCustomerId = OrderMasters.Item (0).ShipMasterCustomerId
                OrderMasters.Item (0).BillSubCustomerId = OrderMasters.Item (0).ShipSubCustomerId
                Return OrderMasters
            End If
            OrderMasters.Item (0).BillMasterCustomerId =
                OrderMasters.Item (0).ShipToCustomer.PrimaryEmployer.MasterCustomerId
            OrderMasters.Item (0).BillSubCustomerId = OrderMasters.Item (0).ShipToCustomer.PrimaryEmployer.SubCustomerId
            Return OrderMasters
        End Function

        Public Overridable Function SetShipAddressIDForOrder (ByVal ShipAddressID As Integer,
                                                              ByVal OrderMasters As IOrderMasters,
                                                              ByVal Optional OrderLineNumber As Integer = - 1) _
            As IOrderMasters
            If (OrderMasters.Count > 0) Then
                Dim num2 As Integer = (OrderMasters.Item (0).Details.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num2)
                    Dim detail As IOrderDetail = OrderMasters.Item (0).Details.Item (i)
                    If (OrderLineNumber = - 1) Then
                        OrderMasters.Item (0).ShipAddressId = ShipAddressID
                    ElseIf (detail.OrderLineNumber = OrderLineNumber) Then
                        OrderMasters.Item (0).Details.Item (i).ShipAddressId = ShipAddressID
                        Return OrderMasters
                    End If
                    detail = Nothing
                    i += 1
                Loop
            End If
            Return OrderMasters
        End Function

        Public Overridable Function SetShipViaCodeForOrder (ByVal ShipViaCode As String,
                                                            ByVal OrderMasters As IOrderMasters,
                                                            ByVal Optional OrderLineNumber As Integer = - 1) _
            As IOrderMasters
            If (OrderMasters.Count > 0) Then
                Dim num2 As Integer = (OrderMasters.Item (0).Details.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num2)
                    Dim detail As IOrderDetail = OrderMasters.Item (0).Details.Item (i)
                    If (OrderLineNumber = - 1) Then
                        If (detail.ShipViaCode.Code.Length > 0) Then
                            detail.ShipViaCode = detail.ShipViaCode.List.Item (ShipViaCode).ToCodeObject
                        End If
                    ElseIf (detail.OrderLineNumber = OrderLineNumber) Then
                        detail.ShipViaCode = detail.ShipViaCode.List.Item (ShipViaCode).ToCodeObject
                        Return OrderMasters
                    End If
                    detail = Nothing
                    i += 1
                Loop
            End If
            Return OrderMasters
        End Function


        ' Properties
        Private ReadOnly Property DoCreditCardAuthorization As Boolean
            Get
                Dim flag As Boolean
                Dim objectValue As Object = RuntimeHelpers.GetObjectValue (New Object)
                Monitor.Enter (RuntimeHelpers.GetObjectValue (objectValue))
                Try
                    If (ConfigurationManager.AppSettings.Item ("CreditCardAuthorization") Is Nothing) Then
                        Return True
                    End If
                    If _
                        (String.Compare (ConfigurationManager.AppSettings.Item ("CreditCardAuthorization"), "OFF", True) =
                         0) Then
                        Return False
                    End If
                    flag = True
                Catch exception1 As Exception
                    ProjectData.SetProjectError (exception1)
                    Dim exception As Exception = exception1
                    flag = True
                    ProjectData.ClearProjectError
                Finally
                    Monitor.Exit (RuntimeHelpers.GetObjectValue (objectValue))
                End Try
                Return flag
            End Get
        End Property


        ' Fields
        Private _BaseCurrency As Currency
        Private _PortalCurrency As Currency
    End Class
End Namespace

