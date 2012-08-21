Imports Microsoft.VisualBasic.CompilerServices
Imports Personify.ApplicationManager
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Text
Imports TIMSS
Imports TIMSS.API.AccountingInfo
Imports TIMSS.API.ApplicationInfo
Imports TIMSS.API.Core
Imports TIMSS.API.OrderInfo
Imports TIMSS.Common
Imports TIMSS.Enumerations

Namespace Personify.ApplicationManager.PersonifyDataObjects
    Public Class EntireOrderSummary
        Inherits BaseHelperClass
        Implements IDisposable
        ' Methods
        Public Sub New(ByVal OrgId As String, ByVal OrgUnitId As String, ByVal BaseCurrency As Currency, ByVal PortalCurrency As Currency, ByVal EnableOnDemandDataLoad As Boolean)
            MyBase.New(OrgId, OrgUnitId, EnableOnDemandDataLoad)
            Me.disposedValue = False
            Me._BaseCurrency = BaseCurrency
            Me._PortalCurrency = PortalCurrency
        End Sub

        Public Sub New(ByVal OrgId As String, ByVal OrgUnitId As String, ByVal ReceiptNumber As Integer, ByVal BaseCurrency As Currency, ByVal PortalCurrency As Currency, ByVal EnableOnDemandDataLoad As Boolean)
            MyBase.New(OrgId, OrgUnitId, EnableOnDemandDataLoad)
            Me.disposedValue = False
            Me._BaseCurrency = BaseCurrency
            Me._PortalCurrency = PortalCurrency
            Dim oARTransactions As IARTransactions = DirectCast(TIMSS.Global.GetCollection(Me.OrganizationId, Me.OrganizationUnitId, NamespaceEnum.AccountingInfo, "ARTransactions"), IARTransactions)
            oARTransactions.EnableIncrementalDataLoad = EnableOnDemandDataLoad
            oARTransactions.Filter.Add("receipt_no", QueryOperatorEnum.Equals, Conversions.ToString(ReceiptNumber))
            oARTransactions.Fill
            If (oARTransactions.Count > 0) Then
                Dim oOrderDetails As IOrderDetails = DirectCast(TIMSS.Global.GetCollection(Me.OrganizationId, Me.OrganizationUnitId, NamespaceEnum.OrderInfo, "OrderDetails"), IOrderDetails)
                Dim builder As New StringBuilder
                Dim num2 As Integer = (oARTransactions.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num2)
                    If (i > 0) Then
                        builder.Append(" or ")
                    End If
                    builder.Append(String.Concat(New String() { " ( order_no = '", oARTransactions.Item(i).OrderNumber, "' and order_line_no = ", oARTransactions.Item(i).OrderLineNumber.ToString, " ) " }))
                    i += 1
                Loop
                If (builder.Length > 0) Then
                    builder.Remove(0, 1)
                End If
                oOrderDetails.Filter.Add(New FilterItem(builder.ToString))
                Dim propertyNameList As String() = New String() { "order_no,ASC", "order_line_no,ASC" }
                oOrderDetails.Sort(propertyNameList)
                oOrderDetails.EnableIncrementalDataLoad = EnableOnDemandDataLoad
                oOrderDetails.Fill
                If (oOrderDetails.Count > 0) Then
                    Me._OrderDetails = Me.LoadClassOrderDetails(oOrderDetails, Me._OrderTotals)
                    Me._BillToShipToAddress = Me.LoadClassBillToShipToAddress(oARTransactions)
                    If True Then
                        Me._OrderReceiptDetails = Me.LoadClassForReceipts(oARTransactions)
                    End If
                End If
            End If
        End Sub

        Public Sub New(ByVal OrgId As String, ByVal OrgUnitId As String, ByVal oOrderMasters As IOrderMasters, ByVal BaseCurrency As Currency, ByVal PortalCurrency As Currency, ByVal EnableOnDemandDataLoad As Boolean)
            MyBase.New(OrgId, OrgUnitId, EnableOnDemandDataLoad)
            Me.disposedValue = False
            Me._oOrderMasters = oOrderMasters
            Me._BaseCurrency = BaseCurrency
            Me._PortalCurrency = PortalCurrency
            Me.FillEntireOrderSummary
        End Sub

        Public Sub New(ByVal OrgId As String, ByVal OrgUnitId As String, ByVal OrderNumber As String, ByVal FetchReceipts As Boolean, ByVal BaseCurrency As Currency, ByVal PortalCurrency As Currency, ByVal EnableOnDemandDataLoad As Boolean)
            MyBase.New(OrgId, OrgUnitId, EnableOnDemandDataLoad)
            Me.disposedValue = False
            Me._BaseCurrency = BaseCurrency
            Me._PortalCurrency = PortalCurrency
            Me._oOrderMasters = DirectCast(TIMSS.Global.GetCollection(Me.OrganizationId, Me.OrganizationUnitId, NamespaceEnum.OrderInfo, "OrderMasters"), IOrderMasters)
            Me._oOrderMasters.EnableIncrementalDataLoad = EnableOnDemandDataLoad
            Me._oOrderMasters.Fill(OrderNumber)
            Me.FillEntireOrderSummary
            If FetchReceipts Then
                Me._OrderReceiptDetails = Me.LoadClassForReceipts(Me._oOrderMasters)
            End If
        End Sub

        Private Function CheckIfBillCustomerIsEmployer(ByVal oOrderMasters As IOrderMasters) As Boolean
            If (oOrderMasters.Item(0).ShipToCustomer.PrimaryEmployer Is Nothing) Then
                Return False
            End If
            Return ((oOrderMasters.Item(0).ShipToCustomer.PrimaryEmployer.MasterCustomerId = oOrderMasters.Item(0).BillMasterCustomerId) And (oOrderMasters.Item(0).ShipToCustomer.PrimaryEmployer.SubCustomerId = oOrderMasters.Item(0).BillSubCustomerId))
        End Function

        Private Function CheckReceiptNumberExists(ByVal ReceiptDetails As OrderReceiptDetails(), ByVal ReceiptNo As String) As Boolean
            Dim flag As Boolean = False
            Dim details As OrderReceiptDetails
            For Each details In ReceiptDetails
                If (details Is Nothing) Then
                    Return flag
                End If
                If (details.ReceiptNo = Conversions.ToDouble(ReceiptNo)) Then
                    Return True
                End If
            Next
            Return flag
        End Function

        Public Sub Dispose() Implements IDisposable.Dispose
            Me.Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If (Not Me.disposedValue AndAlso disposing) Then
            End If
            Me.disposedValue = True
        End Sub

        Public Function DoesOrderHaveMultipleShippingMethods(ByVal OrderMasters As IOrderMasters) As Boolean
            Dim enumerator As IEnumerator
            Dim shipViaCodeString As String = ""
            Try 
                enumerator = OrderMasters.Item(0).Details.GetEnumerator
                Do While enumerator.MoveNext
                    Dim current As IOrderDetail = DirectCast(enumerator.Current, IOrderDetail)
                    If (shipViaCodeString = "") Then
                        shipViaCodeString = current.ShipViaCodeString
                    End If
                    If ((current.ShipViaCodeString.Length > 0) AndAlso (shipViaCodeString <> current.ShipViaCodeString)) Then
                        Return True
                    End If
                    If (current.ShipViaCodeString.Length > 0) Then
                        shipViaCodeString = current.ShipViaCodeString
                    End If
                Loop
            Finally
                If TypeOf enumerator Is IDisposable Then
                    TryCast(enumerator,IDisposable).Dispose
                End If
            End Try
            Return False
        End Function

        Private Sub FillEntireOrderSummary()
            Dim enumerator As IEnumerator
            Me._OrderDetails = Me.LoadClassOrderDetails(Me._oOrderMasters)
            Me._BillToShipToAddress = Me.LoadClassBillToShipToAddress(Me._oOrderMasters)
            Me._OrderTotals = Me.LoadClassOrderTotals(Me._oOrderMasters)
            Me._OrderHasMultipleShipMethods = Me.DoesOrderHaveMultipleShippingMethods(Me._oOrderMasters)
            If Me.BillToShipToAddress(0).BillToCustomerIsPrimaryEmployer Then
                Me._TotalWebBillMeNowOrderAmount = New Decimal
            Else
                Me._TotalWebBillMeNowOrderAmount = Decimal.Add(Me._oOrderMasters.Item(0).OrderFinancialAnalysis.InvClearedReceipts, Me._oOrderMasters.Item(0).TotalWebBillMeNowOrderAmount)
            End If
            Me._DoesOrderHaveBillMeLines = False
            Try 
                enumerator = Me._oOrderMasters.Item(0).Details.GetEnumerator
                Do While enumerator.MoveNext
                    Dim current As IOrderDetail = DirectCast(enumerator.Current, IOrderDetail)
                    If current.IsBillMe Then
                        Me._DoesOrderHaveBillMeLines = True
                        Return
                    End If
                Loop
            Finally
                If TypeOf enumerator Is IDisposable Then
                    TryCast(enumerator,IDisposable).Dispose
                End If
            End Try
        End Sub

        Public Function FormatAmount(ByVal Amount As Decimal) As String
            Return String.Format("{0}{1}", Me._PortalCurrency.Symbol, String.Format("{0:0.00}", TIMSS.Common.Currency.Round(Amount, Me._PortalCurrency.Symbol)))
        End Function

        Public Function LoadClassBillToShipToAddress(ByVal oARTransactions As IARTransactions) As BillToShipToAddress()
            Dim addressArray As BillToShipToAddress() = New BillToShipToAddress() { New BillToShipToAddress }
            Dim str As String = ""
            str = (oARTransactions.Item(0).OrderLineInfo.OrderMaster.BillAddressDetailInfo.FormattedDetail & oARTransactions.Item(0).OrderLineInfo.OrderMaster.BillAddressInfo.FormattedAddress).Replace(Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10), "<br/>")
            addressArray(0).BillToAddress = str
            str = (oARTransactions.Item(0).OrderLineInfo.OrderMaster.ShipAddressDetailInfo.FormattedDetail & oARTransactions.Item(0).OrderLineInfo.OrderMaster.ShipAddressInfo.FormattedAddress).Replace(Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10), "<br/>")
            addressArray(0).ShipToAddress = str
            addressArray(0).BillMasterCustomerId = oARTransactions.Item(0).BillCustomerInfo.MasterCustomerId
            addressArray(0).BillSubCustomerId = Conversions.ToString(oARTransactions.Item(0).BillCustomerInfo.SubCustomerId)
            Dim str2 As String = ""
            str2 = String.Concat(New String() { oARTransactions.Item(0).BillCustomerInfo.FirstName, " ", oARTransactions.Item(0).BillCustomerInfo.MiddleName, " ", oARTransactions.Item(0).BillCustomerInfo.LastName })
            addressArray(0).BillToName = str2
            Return addressArray
        End Function

        Public Function LoadClassBillToShipToAddress(ByVal oOrderMasters As IOrderMasters) As BillToShipToAddress()
            Dim details As IOrderDetails = oOrderMasters.Item(0).Details
            Dim addressArray As BillToShipToAddress() = New BillToShipToAddress() { New BillToShipToAddress }
            Dim str As String = ""



            str = (oOrderMasters.Item(0).BillAddressDetailInfo.FormattedDetail & oOrderMasters.Item(0).BillAddressInfo.FormattedAddress).Replace(Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10), "<br/>")
            addressArray(0).BillToAddress = str
            addressArray(0).BillToAddressStatus = oOrderMasters.Item(0).BillAddressInfo.AddressStatusCodeString
            str = (oOrderMasters.Item(0).ShipAddressDetailInfo.FormattedDetail & oOrderMasters.Item(0).ShipAddressInfo.FormattedAddress).Replace(Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10), "<br/>")
            addressArray(0).ShipToAddress = str
            addressArray(0).ShipToAddressStatus = oOrderMasters.Item(0).ShipAddressDetailInfo.AddressStatusCodeString
            addressArray(0).BillMasterCustomerId = oOrderMasters.Item(0).BillMasterCustomerId
            addressArray(0).BillSubCustomerId = Conversions.ToString(oOrderMasters.Item(0).BillSubCustomerId)
            Dim firstName As String = ""
            firstName = oOrderMasters.Item(0).BillToCustomer.FirstName
            If (oOrderMasters.Item(0).BillToCustomer.MiddleName = "") Then
                firstName = (firstName & " ")
            Else
                firstName = (firstName & " " & oOrderMasters.Item(0).BillToCustomer.MiddleName & " ")
            End If
            firstName = (firstName & oOrderMasters.Item(0).BillToCustomer.LastName)
            addressArray(0).BillToName = firstName
            If (Not oOrderMasters.Item(0).ShipToCustomer.PrimaryEmployer Is Nothing) Then
                If (oOrderMasters.Item(0).ShipToCustomer.PrimaryEmployer.MasterCustomerId.Length > 0) Then
                    addressArray(0).PrimaryEmployerExists = True
                Else
                    addressArray(0).PrimaryEmployerExists = False
                End If
            End If
            If Me.CheckIfBillCustomerIsEmployer(oOrderMasters) Then
                addressArray(0).BillToCustomerIsPrimaryEmployer = True
                Return addressArray
            End If
            addressArray(0).BillToCustomerIsPrimaryEmployer = False
            Return addressArray
        End Function

        Public Function LoadClassForReceipts(ByVal oTransactions As IARTransactions) As OrderReceiptDetails()
            Dim index As Integer = 0
            Dim receiptDetails As OrderReceiptDetails() = New OrderReceiptDetails(1  - 1) {}
            Dim num3 As Integer = (oTransactions.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num3)
                If (((oTransactions.Item(i).TransactionTypeCode.Code = "1") Or (oTransactions.Item(i).TransactionTypeCode.Code = "9")) AndAlso Not Me.CheckReceiptNumberExists(receiptDetails, Conversions.ToString(oTransactions.Item(i).ReceiptInfo.ReceiptNumber))) Then
                    If (index > 0) Then
                        receiptDetails = DirectCast(Utils.CopyArray(DirectCast(receiptDetails, Array), New OrderReceiptDetails((index + 1)  - 1) {}), OrderReceiptDetails())
                    End If
                    receiptDetails(index) = New OrderReceiptDetails
                    receiptDetails(index).OrderNumber = oTransactions.Item(0).OrderNumber
                    receiptDetails(index).ReceiptNo = oTransactions.Item(i).ReceiptInfo.ReceiptNumber
                    receiptDetails(index).ReceiptTypeCode = oTransactions.Item(i).ReceiptInfo.ReceiptTypeCode.Description
                    receiptDetails(index).PartialCCNumber = oTransactions.Item(i).ReceiptInfo.PartialCCAccountNumber
                    receiptDetails(index).TotalAmount = Me.FormatAmount(oTransactions.Item(i).ReceiptInfo.ActualAmount)
                    index += 1
                End If
                i += 1
            Loop
            Return receiptDetails
        End Function

        Public Function LoadClassForReceipts(ByVal OrderMasters As IOrderMasters) As OrderReceiptDetails()
            Dim index As Integer = 0
            Dim receiptDetails As OrderReceiptDetails() = New OrderReceiptDetails(1  - 1) {}
            Dim num3 As Integer = (OrderMasters.Item(0).Transactions.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num3)
                If (((OrderMasters.Item(0).Transactions.Item(i).TransactionTypeCode.Code = "1") Or (OrderMasters.Item(0).Transactions.Item(i).TransactionTypeCode.Code = "9")) AndAlso Not Me.CheckReceiptNumberExists(receiptDetails, Conversions.ToString(OrderMasters.Item(0).Transactions.Item(i).ReceiptInfo.ReceiptNumber))) Then
                    If (index > 0) Then
                        receiptDetails = DirectCast(Utils.CopyArray(DirectCast(receiptDetails, Array), New OrderReceiptDetails((index + 1)  - 1) {}), OrderReceiptDetails())
                    End If
                    receiptDetails(index) = New OrderReceiptDetails
                    receiptDetails(index).OrderNumber = OrderMasters.Item(0).OrderNumber
                    receiptDetails(index).ReceiptNo = OrderMasters.Item(0).Transactions.Item(i).ReceiptInfo.ReceiptNumber
                    receiptDetails(index).ReceiptTypeCode = OrderMasters.Item(0).Transactions.Item(i).ReceiptInfo.ReceiptTypeCode.Description
                    receiptDetails(index).PartialCCNumber = OrderMasters.Item(0).Transactions.Item(i).ReceiptInfo.PartialCCAccountNumber
                    receiptDetails(index).TotalAmount = Me.FormatAmount(OrderMasters.Item(0).Transactions.Item(i).ReceiptInfo.ActualAmount)
                    index += 1
                End If
                i += 1
            Loop
            Return receiptDetails
        End Function

        Public Function LoadClassOrderDetails(ByVal oOrderMasters As IOrderMasters) As OrderDetails()
            Dim num As Integer = 0
            Dim details As IOrderDetails = oOrderMasters.Item(0).Details
            Dim detailsArray2 As OrderDetails() = New OrderDetails(((details.Count - 1) + 1)  - 1) {}
            details.Sort("OrderLineNumber", ListSortDirection.Ascending)
            Dim orderNumber As String = ""
            Dim num4 As Integer = (details.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num4)
                detailsArray2(i) = New OrderDetails
                Dim details2 As OrderDetails = detailsArray2(i)
                details2.OrderNumber = details.Item(i).OrderNumber
                details2.OrderLineNumber = details.Item(i).OrderLineNumber.ToString
                If (orderNumber = details.Item(i).OrderNumber) Then
                    details2.FirstLineInOrder = False
                Else
                    details2.FirstLineInOrder = True
                End If
                orderNumber = details.Item(i).OrderNumber
                details2.RelatedLineNumber = details.Item(i).RelatedLineNumber.ToString
                details2.Subsystem = details.Item(i).Subsystem
                details2.OrderDate = details.Item(i).OrderDate.ToShortDateString
                details2.ProductCode = details.Item(i).ProductCode
                details2.ProductTitle = details.Item(i).Description
                details2.ProductTypeCode = details.Item(i).ProductTypeCode.Code
                details2.Quantity = details.Item(i).OrderQuantity.ToString
                details2.RateCode = details.Item(i).RateCode.Code
                details2.RateStructure = details.Item(i).RateStructure.Code
                details2.UnitAmount = Me.FormatAmount(details.Item(i).ActualUnitPrice)
                details2.UnitDiscount = Me.FormatAmount(details.Item(i).ActualUnitDiscount)
                details2.BaseTotalAmount = details.Item(i).ActualTotalAmount
                details2.FormattedBaseTotalAmount = Me.FormatAmount(details.Item(i).ActualTotalAmount)
                details2.BaseExtendedAmount = details.Item(i).ActualExtendedAmount
                details2.FormattedBaseExtendedAmount = Me.FormatAmount(details.Item(i).ActualExtendedAmount)
                details2.LineTypeCode = details.Item(i).LineTypeCode.Code
                details2.ShipMasterCustomerId = details.Item(i).ShipMasterCustomerId
                details2.ShipSubCustomerId = details.Item(i).ShipSubCustomerId.ToString
                details2.ShipCustomerLabelName = details.Item(i).ShipCustomer.LabelName
                details2.ShipAddressId = details.Item(i).ShipAddressId.ToString
                details2.ShipViaCode = details.Item(i).ShipViaCode.Code
                details2.TrackingNumber = details.Item(i).TrackingNumber
                details2.PromoCode = details.Item(i).MarketCode
                details2.CouponCode = details.Item(i).CouponCode
                details2.ActualCouponAmount = Me.FormatAmount(details.Item(i).ActualCouponAmount)
                details2.IsProductAndCustomerQualifyForBillMe = details.Item(i).IsProductAndCustomerQualifyForBillMe
                details2.IsPORequiredForBillMeProduct = details.Item(i).IsPORequiredForBillMeProduct
                details2.ShipFormattedAddress = details.Item(i).ShipAddress.FormattedAddress
                details2.BaseShipAmount = Me.FormatAmount(details.Item(i).ActualShipAmount)
                details2.BaseTaxAmount = Me.FormatAmount(details.Item(i).ActualTaxAmount)
                details2.LineStatusCode = details.Item(i).LineStatusCode.Description
                details2.FulfillStatusCode = details.Item(i).FulfillStatusCode.Description
                details2.ProductId = Conversions.ToString(details.Item(i).ProductId)
                details2.TotalPaid = Me.FormatAmount(Decimal.Negate(Decimal.Add(details.Item(i).OrderFinancialAnalysis.InvClearedReceipts, details.Item(i).OrderFinancialAnalysis.InvDeferredCreditCards)))
                details2.BalanceDue = Me.FormatAmount(Decimal.Add(Decimal.Add(Decimal.Add(details.Item(i).ActualTotalAmount, details.Item(i).OrderFinancialAnalysis.InvTransfers), details.Item(i).OrderFinancialAnalysis.InvClearedReceipts), details.Item(i).OrderFinancialAnalysis.InvDeferredCreditCards))
                If (details.Item(i).Subsystem = "ECD") Then
                    Dim listArray As DCDFileList() = New DCDFileList(((details.Item(i).DCDInfoList.Count - 1) + 1)  - 1) {}
                    Dim num7 As Integer = (details.Item(i).DCDInfoList.Count - 1)
                    Dim j As Integer = 0
                    Do While (j <= num7)
                        If (Not details.Item(i).DCDInfoList.Item(j).DCDFileDetails.Item(0) Is Nothing) Then
                            listArray(j) = New DCDFileList
                            listArray(j).OrderNumber = details.Item(i).DCDInfoList.Item(j).OrderNumber
                            listArray(j).OrderLineNumber = Convert.ToInt32(details.Item(i).DCDInfoList.Item(j).OrderLineNumber)
                            listArray(j).FileId = Convert.ToInt32(details.Item(i).DCDInfoList.Item(j).FileId)
                            listArray(j).DocumentTitle = details.Item(i).DCDInfoList.Item(j).DCDFileDetails.Item(0).DocumentTitle
                            listArray(j).DisplayCopyright = details.Item(i).DCDInfoList.Item(j).DCDFileDetails.Item(0).DisplayCopyrightFlag
                            listArray(j).CopyrightText = details.Item(i).DCDInfoList.Item(j).DCDFileDetails.Item(0).CopyrightText
                        End If
                        j += 1
                    Loop
                    details2.DCDFiles = listArray
                End If
                details2.ShowHotelBlockingLink = (details.Item(i).CanCreateHotelReservationBridge Or details.Item(i).CanManageHotelReservation)
                details2.CanCreateHotelReservationBridge = details.Item(i).CanCreateHotelReservationBridge
                details2.CanManageHotelReservation = details.Item(i).CanManageHotelReservation
                If (((String.Compare(details.Item(i).Subsystem, "FND", True) = 0) AndAlso (String.Compare(details.Item(i).ProductTypeCodeString, "CASH", True) = 0)) AndAlso (details.Item(i).OrderFundRaisingDetail.RecurringGiftFlag AndAlso (String.Compare(details.Item(i).OrderFundRaisingDetail.RecurringGiftStatusCodeString, "ACTIVE", True) = 0))) Then
                    details2.IsARecurringGift = True
                    details2.NextRecurringDate = details.Item(i).OrderFundRaisingDetail.NextRecurringGiftDate
                    details2.GiftFrequencyCode = details.Item(i).OrderFundRaisingDetail.GiftFrequencyCode.Description
                End If
                details2 = Nothing
                num += 1
                i += 1
            Loop
            Return detailsArray2
        End Function

        Public Function LoadClassOrderDetails(ByVal oOrderDetails As IOrderDetails, ByRef _orderTotals As OrderTotals()) As OrderDetails()
            Dim num2 As Decimal
            Dim num3 As Decimal
            Dim num4 As Decimal
            Dim num5 As Decimal
            Dim num6 As Decimal
            Dim num As Integer = 0
            Dim detailsArray2 As OrderDetails() = New OrderDetails(((oOrderDetails.Count - 1) + 1)  - 1) {}
            oOrderDetails.Sort("OrderLineNumber", ListSortDirection.Ascending)
            Dim orderNumber As String = ""
            Dim num8 As Integer = (oOrderDetails.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num8)
                detailsArray2(i) = New OrderDetails
                Dim details As OrderDetails = detailsArray2(i)
                details.OrderNumber = oOrderDetails.Item(i).OrderNumber
                details.OrderLineNumber = oOrderDetails.Item(i).OrderLineNumber.ToString
                If (orderNumber = oOrderDetails.Item(i).OrderNumber) Then
                    details.FirstLineInOrder = False
                Else
                    details.FirstLineInOrder = True
                End If
                orderNumber = oOrderDetails.Item(i).OrderNumber
                details.RelatedLineNumber = oOrderDetails.Item(i).RelatedLineNumber.ToString
                details.Subsystem = oOrderDetails.Item(i).Subsystem
                details.OrderDate = oOrderDetails.Item(i).OrderDate.ToShortDateString
                details.ProductCode = oOrderDetails.Item(i).ProductCode
                details.ProductTitle = oOrderDetails.Item(i).Description
                details.ProductTypeCode = oOrderDetails.Item(i).ProductTypeCode.Code
                details.Quantity = oOrderDetails.Item(i).OrderQuantity.ToString
                details.RateCode = oOrderDetails.Item(i).RateCode.Code
                details.RateStructure = oOrderDetails.Item(i).RateStructure.Code
                details.UnitAmount = Me.FormatAmount(oOrderDetails.Item(i).ActualUnitPrice)
                details.UnitDiscount = Me.FormatAmount(oOrderDetails.Item(i).ActualUnitDiscount)
                details.BaseTotalAmount = oOrderDetails.Item(i).ActualTotalAmount
                details.FormattedBaseTotalAmount = Me.FormatAmount(oOrderDetails.Item(i).ActualTotalAmount)
                details.BaseExtendedAmount = oOrderDetails.Item(i).ActualExtendedAmount
                details.FormattedBaseExtendedAmount = Me.FormatAmount(oOrderDetails.Item(i).ActualExtendedAmount)
                details.LineTypeCode = oOrderDetails.Item(i).LineTypeCode.Code
                details.ShipMasterCustomerId = oOrderDetails.Item(i).ShipMasterCustomerId
                details.ShipSubCustomerId = oOrderDetails.Item(i).ShipSubCustomerId.ToString
                details.ShipCustomerLabelName = oOrderDetails.Item(i).ShipCustomer.LabelName
                details.ShipAddressId = oOrderDetails.Item(i).ShipAddressId.ToString
                details.ShipViaCode = oOrderDetails.Item(i).ShipViaCode.Code
                details.TrackingNumber = oOrderDetails.Item(i).TrackingNumber
                details.PromoCode = oOrderDetails.Item(i).MarketCode
                details.CouponCode = oOrderDetails.Item(i).CouponCode
                details.ActualCouponAmount = Me.FormatAmount(oOrderDetails.Item(i).ActualCouponAmount)
                details.IsProductAndCustomerQualifyForBillMe = oOrderDetails.Item(i).IsProductAndCustomerQualifyForBillMe
                details.IsPORequiredForBillMeProduct = oOrderDetails.Item(i).IsPORequiredForBillMeProduct
                details.ShipFormattedAddress = oOrderDetails.Item(i).ShipAddress.FormattedAddress
                details.BaseShipAmount = Me.FormatAmount(oOrderDetails.Item(i).ActualShipAmount)
                details.BaseTaxAmount = Me.FormatAmount(oOrderDetails.Item(i).ActualTaxAmount)
                details.IsBackOrderRequired = Conversions.ToString(oOrderDetails.Item(i).IsBackOrderRequired)
                details.IsWaitListingRequired = Conversions.ToString(oOrderDetails.Item(i).IsWaitListingRequired)
                details.LineStatusCode = oOrderDetails.Item(i).LineStatusCodeString
                details.FulfillStatusCode = oOrderDetails.Item(i).FulfillStatusCodeString
                details.ProductId = Conversions.ToString(oOrderDetails.Item(i).ProductId)
                num4 = Decimal.Add(num4, oOrderDetails.Item(i).ActualUnitDiscount)
                num5 = Decimal.Add(num5, oOrderDetails.Item(i).ActualShipAmount)
                num6 = Decimal.Add(num6, oOrderDetails.Item(i).ActualTaxAmount)
                num3 = Decimal.Add(num3, oOrderDetails.Item(i).ActualTotalAmount)
                num2 = Decimal.Add(num2, oOrderDetails.Item(i).ActualUnitPrice)
                details.TotalPaid = Me.FormatAmount(Decimal.Negate(Decimal.Add(oOrderDetails.Item(i).OrderFinancialAnalysis.InvClearedReceipts, oOrderDetails.Item(i).OrderFinancialAnalysis.InvDeferredCreditCards)))
                details.BalanceDue = Me.FormatAmount(Decimal.Add(Decimal.Add(Decimal.Add(oOrderDetails.Item(i).ActualTotalAmount, oOrderDetails.Item(i).OrderFinancialAnalysis.InvTransfers), oOrderDetails.Item(i).OrderFinancialAnalysis.InvClearedReceipts), oOrderDetails.Item(i).OrderFinancialAnalysis.InvDeferredCreditCards))
                details = Nothing
                num += 1
                i += 1
            Loop
            Dim totalsArray As OrderTotals() = New OrderTotals() { New OrderTotals }
            Dim totals As OrderTotals = totalsArray(0)
            totals.TotalDiscountAmount = Me.FormatAmount(num4)
            totals.TotalShippingAmount = Me.FormatAmount(num5)
            totals.TotalTaxAmount = Me.FormatAmount(num6)
            totals.TotalAmount = Me.FormatAmount(num3)
            totals.SubTotal = Me.FormatAmount(num2)
            totals.TotalTotalPaid = Me.FormatAmount(Decimal.Negate(Decimal.Add(oOrderDetails.Item(0).OrderMaster.OrderFinancialAnalysis.InvClearedReceipts, oOrderDetails.Item(0).OrderMaster.OrderFinancialAnalysis.InvDeferredCreditCards)))
            totals.TotalBalanceDue = Me.FormatAmount(Decimal.Add(Decimal.Add(Decimal.Add(num3, oOrderDetails.Item(0).OrderMaster.OrderFinancialAnalysis.InvTransfers), oOrderDetails.Item(0).OrderMaster.OrderFinancialAnalysis.InvClearedReceipts), oOrderDetails.Item(0).OrderMaster.OrderFinancialAnalysis.InvDeferredCreditCards))
            totals = Nothing
            _orderTotals = totalsArray
            Return detailsArray2
        End Function

        Public Function LoadClassOrderTotals(ByVal OrderMasters As IOrderMasters) As OrderTotals()
            Dim totalsArray2 As OrderTotals() = New OrderTotals() { New OrderTotals }
            Dim totals As OrderTotals = totalsArray2(0)
            totals.OrderNumber = OrderMasters.Item(0).OrderNumber
            totals.TotalDiscountAmount = Me.FormatAmount(OrderMasters.Item(0).TotalDiscountAmount)
            totals.TotalShippingAmount = Me.FormatAmount(OrderMasters.Item(0).TotalShippingAmount)
            totals.TotalTaxAmount = Me.FormatAmount(OrderMasters.Item(0).TotalTaxAmount)
            totals.TotalAmount = Me.FormatAmount(OrderMasters.Item(0).TotalOrderAmount)
            totals.SubTotal = Me.FormatAmount(OrderMasters.Item(0).TotalProductAmount)
            totals.TotalTotalPaid = Me.FormatAmount(Decimal.Negate(Decimal.Add(OrderMasters.Item(0).OrderFinancialAnalysis.InvClearedReceipts, OrderMasters.Item(0).OrderFinancialAnalysis.InvDeferredCreditCards)))
            totals.TotalBalanceDue = Me.FormatAmount(Decimal.Add(Decimal.Add(Decimal.Add(OrderMasters.Item(0).TotalOrderAmount, OrderMasters.Item(0).OrderFinancialAnalysis.InvTransfers), OrderMasters.Item(0).OrderFinancialAnalysis.InvClearedReceipts), OrderMasters.Item(0).OrderFinancialAnalysis.InvDeferredCreditCards))
            totals.TotalUnitPrice = Me.FormatAmount(OrderMasters.Item(0).TotalUnitPrice)
            totals = Nothing
            Return totalsArray2
        End Function

        Public Function LoadClassShippingDetails(ByVal oOrderMasters As IOrderMasters) As ShippingDetails()
            Dim enumerator As IEnumerator
            If (oOrderMasters Is Nothing) Then
                oOrderMasters = Me._oOrderMasters
            End If
            If (oOrderMasters Is Nothing) Then
                Return Nothing
            End If
            Dim hashtable As New Hashtable
            Dim index As Integer = 0
            Dim num5 As Integer = (oOrderMasters.Item(0).Details.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num5)
                Dim detail As IOrderDetail = oOrderMasters.Item(0).Details.Item(i)
                If (detail.ShipViaCodeString.Length > 0) Then
                    hashtable.Add(detail.OrderLineNumber, detail.ShipViaCode.Code)
                End If
                detail = Nothing
                i += 1
            Loop
            Dim codes As IApplicationCodes = New PersonifyConnect(Me.OrganizationId, Me.OrganizationUnitId).GetApplicationCodes("ORD", "SHIP_VIA", True)
            Dim detailsArray2 As ShippingDetails() = New ShippingDetails(((codes.Count - 1) + 1)  - 1) {}
            Try 
                enumerator = codes.GetEnumerator
                Do While enumerator.MoveNext
                    Dim current As IApplicationCode = DirectCast(enumerator.Current, IApplicationCode)
                    Dim num6 As Integer = (oOrderMasters.Item(0).Details.Count - 1)
                    Dim k As Integer = 0
                    Do While (k <= num6)
                        Dim detail2 As IOrderDetail = oOrderMasters.Item(0).Details.Item(k)
                        If (detail2.ShipViaCodeString.Length > 0) Then
                            detail2.ShipViaCode.Code = current.Code
                        End If
                        detail2 = Nothing
                        k += 1
                    Loop
                    detailsArray2(index) = New ShippingDetails
                    detailsArray2(index).ShipViaCode = current.Code
                    detailsArray2(index).ShipViaCodeDescription = current.Description
                    detailsArray2(index).ShippingAmount = oOrderMasters.Item(0).TotalShippingAmount
                    detailsArray2(index).FormattedShippingAmount = Me.FormatAmount(oOrderMasters.Item(0).TotalShippingAmount)
                    index += 1
                Loop
            Finally
                If TypeOf enumerator Is IDisposable Then
                    TryCast(enumerator,IDisposable).Dispose
                End If
            End Try
            Dim num7 As Integer = (oOrderMasters.Item(0).Details.Count - 1)
            Dim j As Integer = 0
            Do While (j <= num7)
                Dim detail3 As IOrderDetail = oOrderMasters.Item(0).Details.Item(j)
                If ((detail3.ShipViaCodeString.Length > 0) AndAlso (Not hashtable.Item(detail3.OrderLineNumber) Is Nothing)) Then
                    detail3.ShipViaCode = DirectCast(NewLateBinding.LateGet(NewLateBinding.LateGet(detail3.ShipViaCode.List, Nothing, "Item", New Object() { RuntimeHelpers.GetObjectValue(hashtable.Item(detail3.OrderLineNumber)) }, Nothing, Nothing, Nothing), Nothing, "ToCodeObject", New Object(0  - 1) {}, Nothing, Nothing, Nothing), ICode)
                End If
                detail3 = Nothing
                j += 1
            Loop
            Return detailsArray2
        End Function


        ' Properties
        Public ReadOnly Property BillToShipToAddress As BillToShipToAddress()
            Get
                Return Me._BillToShipToAddress
            End Get
        End Property

        Public ReadOnly Property DoesOrderHaveBillMeLines As Boolean
            Get
                Return Me._DoesOrderHaveBillMeLines
            End Get
        End Property

        Public ReadOnly Property FormattedTotalWebBillMeNowOrderAmount As String
            Get
                Return Me.FormatAmount(Me._TotalWebBillMeNowOrderAmount)
            End Get
        End Property

        Public ReadOnly Property OrderDetails As OrderDetails()
            Get
                Return Me._OrderDetails
            End Get
        End Property

        Public ReadOnly Property OrderHasMultipleShipMethods As Boolean
            Get
                Return Me._OrderHasMultipleShipMethods
            End Get
        End Property

        Public ReadOnly Property OrderReceiptDetails As OrderReceiptDetails()
            Get
                Return Me._OrderReceiptDetails
            End Get
        End Property

        Public ReadOnly Property OrderTotals As OrderTotals()
            Get
                Return Me._OrderTotals
            End Get
        End Property

        Public ReadOnly Property ShippingDetails As ShippingDetails()
            Get
                Return Me._ShippingDetails
            End Get
        End Property

        Public ReadOnly Property TotalWebBillMeNowOrderAmount As Decimal
            Get
                Return Me._TotalWebBillMeNowOrderAmount
            End Get
        End Property


        ' Fields
        Private _BaseCurrency As Currency
        Private _BillToShipToAddress As BillToShipToAddress()
        Private _DoesOrderHaveBillMeLines As Boolean
        Private _oOrderMasters As IOrderMasters
        Private _OrderDetails As OrderDetails()
        Private _OrderHasMultipleShipMethods As Boolean
        Private _OrderReceiptDetails As OrderReceiptDetails()
        Private _OrderTotals As OrderTotals()
        Private _PortalCurrency As Currency
        Private _ShippingDetails As ShippingDetails()
        Private _TotalWebBillMeNowOrderAmount As Decimal
        Private disposedValue As Boolean
    End Class
End Namespace

