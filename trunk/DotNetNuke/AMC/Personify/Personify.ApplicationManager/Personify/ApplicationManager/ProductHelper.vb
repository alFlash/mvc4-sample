Imports System
Imports System.Data
Imports System.Collections.Generic
Imports System.Collections
Imports System.Text
Imports System.Runtime.CompilerServices
Imports System.Collections.Specialized
Imports TIMSS.API.Base.OrderInfo
Imports TIMSS.Interfaces
Imports TIMSS.DataAccess
Imports TIMSS
Imports TIMSS.Enumerations
Imports TIMSS.API
Imports TIMSS.API.ApplicationInfo
Imports Microsoft.VisualBasic.CompilerServices
Imports TIMSS.API.Core
Imports TIMSS.API.DataCache
Imports TIMSS.SqlObjects
Imports TIMSS.API.MembershipInfo
Imports TIMSS.API.MeetingInfo
Imports TIMSS.API.FundRaisingInfo
Imports TIMSS.API.DigitalContentDeliveryInfo
Imports Personify.ApplicationManager.PersonifyDataObjects
Imports TIMSS.API.ProductInfo
Imports TIMSS.API.WebInfo
Imports Microsoft.VisualBasic
Imports TIMSS.Common

Namespace Personify.ApplicationManager
    Public Class ProductHelper
        Inherits BaseHelperClass
        ' Methods
        Public Sub New (ByVal OrgId As String, ByVal OrgUnitId As String, ByVal PortalId As Integer)
            MyBase.New (OrgId, OrgUnitId)
            Me._DefaultListRateStructure = String.Empty
            Me._DefaultMemberRateStructure = String.Empty
        End Sub

        Public Sub New (ByVal OrgId As String, ByVal OrgUnitId As String, ByVal EnableOnDemandDataLoad As Boolean,
                        ByVal PortalId As Integer)
            MyBase.New (OrgId, OrgUnitId, EnableOnDemandDataLoad)
            Me._DefaultListRateStructure = String.Empty
            Me._DefaultMemberRateStructure = String.Empty
        End Sub

        Public Sub New(ByVal OrgId As String, ByVal OrgUnitId As String, ByVal BaseCurrency As Personify.ApplicationManager.PersonifyDataObjects.Currency,
                        ByVal PortalCurrency As Personify.ApplicationManager.PersonifyDataObjects.Currency, ByVal EnableOnDemandDataLoad As Boolean,
                        ByVal PortalId As Integer)
            MyBase.New(OrgId, OrgUnitId, EnableOnDemandDataLoad)
            Me._DefaultListRateStructure = String.Empty
            Me._DefaultMemberRateStructure = String.Empty
            Me._BaseCurrency = BaseCurrency
            Me._PortalCurrency = PortalCurrency
            Me._PortalId = PortalId
        End Sub

        Private Function ConstructComponentWebPrice (ByVal Ratecode As IProductRateCode) As WebPrices
            Dim price As WebPrice
            Dim enumerator As IEnumerator
            Dim baseCurrencyWebPrices As New WebPrices
            Dim prices3 As New WebPrices
            Dim pricingList As IProductPricingList = Ratecode.PricingList
            Try
                enumerator = pricingList.GetEnumerator
                Do While enumerator.MoveNext
                    Dim current As IProductPricing = DirectCast (enumerator.Current, IProductPricing)
                    If ((current.CurrencyCode.Code = Me._BaseCurrency.Code) AndAlso current.IsActive) Then
                        price = baseCurrencyWebPrices.AddNewWebPrice
                        price.RateCode = Ratecode.RateCode.Code
                        price.RateCodeDescr = Ratecode.RateCode.Description
                        price.RateStructure = Ratecode.RateStructure.Code
                        price.RateStructureDescr = Ratecode.RateStructure.Description
                        price.IsDefault = Ratecode.DefaultRateWebFlag
                        price.MaxBadges = Ratecode.MaxBadges
                        price.Price = current.Price
                    End If
                    If ((current.CurrencyCode.Code = Me._PortalCurrency.Code) AndAlso current.IsActive) Then
                        Dim price2 As WebPrice = prices3.AddNewWebPrice
                        price2.RateCode = Ratecode.RateCode.Code
                        price2.RateCodeDescr = Ratecode.RateCode.Description
                        price2.RateStructure = Ratecode.RateStructure.Code
                        price2.RateStructureDescr = Ratecode.RateStructure.Description
                        price2.IsDefault = Ratecode.DefaultRateWebFlag
                        price2.MaxBadges = Ratecode.MaxBadges
                        price2.Price = current.Price
                    End If
                Loop
            Finally
                If TypeOf enumerator Is IDisposable Then
                    TryCast (enumerator, IDisposable).Dispose
                End If
            End Try
            If ((Not prices3 Is Nothing) AndAlso (prices3.Count > 0)) Then
                Return prices3
            End If
            If ((Not baseCurrencyWebPrices Is Nothing) AndAlso (baseCurrencyWebPrices.Count > 0)) Then
                Return Me.DoCurrencyPriceConversion (baseCurrencyWebPrices)
            End If
            price = baseCurrencyWebPrices.AddNewWebPrice
            price.RateCode = Ratecode.RateCode.Code
            price.RateCodeDescr = Ratecode.RateCode.Description
            price.RateStructure = Ratecode.RateStructure.Code
            price.RateStructureDescr = Ratecode.RateStructure.Description
            price.IsDefault = Ratecode.DefaultRateWebFlag
            price.MaxBadges = Ratecode.MaxBadges
            price.Price = Decimal.Zero
            Return baseCurrencyWebPrices
        End Function

        Private Function ConstructInClause (ByVal CommaSeperatedList As String) As String
            Dim builder As New StringBuilder
            Dim str2 As String
            For Each str2 In CommaSeperatedList.Split (New Char() {","c})
                builder.Append (("'" & str2 & "'"))
                builder.Append (",")
            Next
            builder.Remove ((builder.Length - 1), 1)
            builder.Insert (0, "(")
            builder.Insert (builder.Length, ")")
            Return builder.ToString
        End Function

        Private Function ConvertToProductIdArray (ByVal productDataTable As DataTable) As String()
            Dim list As New List(Of String)
            Dim num As Integer = (productDataTable.Rows.Count - 1)
            Dim num3 As Integer = num
            Dim i As Integer = 0
            Do While (i <= num3)
                list.Add (Conversions.ToString (productDataTable.Rows.Item (i).Item ("component_product_id")))
                i += 1
            Loop
            Return list.ToArray
        End Function

        Private Function DoCurrencyPriceConversion (ByVal BaseCurrencyWebPrices As WebPrices) As WebPrices
            Dim rates As IApplicationExchangeRates =
                    CachedApplicationData.ApplicationDataCache.ExchangeRates(Me._BaseCurrency.Code,
                                                                                  Me._PortalCurrency.Code, DateTime.Now)
            If (rates.Count > 0) Then
                Dim num2 As Integer = (BaseCurrencyWebPrices.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num2)
                    BaseCurrencyWebPrices.Item(i).Price =
                        TIMSS.Common.Currency.Round(Decimal.Multiply(BaseCurrencyWebPrices.Item(i).Price, rates.Item(0).Xrate),
                                        Me._PortalCurrency.Code)
                    If BaseCurrencyWebPrices.Item (i).HasValidSchedulePrice Then
                        BaseCurrencyWebPrices.Item(i).MinScheduledPrice =
                            TIMSS.Common.Currency.Round(
                                Decimal.Multiply(BaseCurrencyWebPrices.Item(i).MinScheduledPrice, rates.Item(0).Xrate),
                                Me._PortalCurrency.Code)
                        BaseCurrencyWebPrices.Item(i).MaxScheduledPrice =
                            TIMSS.Common.Currency.Round(
                                Decimal.Multiply(BaseCurrencyWebPrices.Item(i).MaxScheduledPrice, rates.Item(0).Xrate),
                                Me._PortalCurrency.Code)
                        BaseCurrencyWebPrices.Item (i).ScheduledPriceRange =
                            (BaseCurrencyWebPrices.Item (i).MinScheduledPrice.ToString & "," &
                             BaseCurrencyWebPrices.Item (i).MaxScheduledPrice.ToString)
                    End If
                    i += 1
                Loop
            End If
            Return BaseCurrencyWebPrices
        End Function

        Public Overridable Function GetAllDetailsForAProduct(ByVal ProductId As Integer,
                                                                    Optional ByVal FetchCrossSell As Boolean = True,
                                                                    Optional ByVal FetchUpSell As Boolean = True,
                                                                    Optional ByVal FetchListPrice As Boolean = True,
                                                                    Optional ByVal FetchMemberPrice As Boolean = True,
                                                                    Optional ByVal FetchAllPrices As Boolean = False,
                                                                    Optional ByVal SubSystemPricingOption As SubSystem_PricingOption() = Nothing,
                                                                    Optional ByVal FetchCustomerPrice As Boolean = False,
                                                                    Optional ByVal MasterCustomerId As String = "",
                                                                    Optional ByVal SubCustomerId As Integer = 0,
                                                                    Optional ByVal FetchComponents As Boolean = True,
                                                                    Optional ByVal FetchECDFiles As Boolean = False,
                                                                    Optional ByVal FetchSubProducts As Boolean = False,
                                                                    Optional ByVal FetchMeetingInfo As Boolean = False,
                                                                    Optional ByVal FetchRelatedCustomers As Boolean = False,
                                                                    Optional ByVal SubProductTypeFilter As String() = Nothing,
                                                                    Optional ByVal SubsystemListForCrossSell As String = "",
                                                                    Optional ByVal SubsystemListForUpSell As String = "") _
            As ProductDetails

            If SubSystemPricingOption Is Nothing Then
                SubSystemPricingOption = New SubSystem_PricingOption() {}
            End If


            If SubProductTypeFilter Is Nothing Then
                SubProductTypeFilter = New String() {}
            End If


            Dim oProductDetailHashTable As New Hashtable
            Dim productIds As String() = New String() {Conversions.ToString(ProductId)}
            Dim oWebProductInfo As ITmarWebProductViewList =
                    CachedApplicationData.ApplicationDataCache.GetWebEnabledProductsWithPriceFromCache(productIds,
                                                                                                            Me.
                                                                                                               GetBestQualifiedRateStructureForLoginUserArray,
                                                                                                            Me.
                                                                                                               _PortalCurrency _
                                                                                                               .Code,
                                                                                                            Me.
                                                                                                               _BaseCurrency _
                                                                                                               .Code,
                                                                                                            Me.
                                                                                                               DefaultListRateStructure,
                                                                                                            Me.
                                                                                                               DefaultMemberRateStructure)
            If _
                (If(((oWebProductInfo.Count > 0) AndAlso Me.IsProductsExistInSameOrgOrgUnit(oWebProductInfo)), 1, 0) =
                 0) Then
                Return Nothing
            End If

            Dim defaultPricing As PersonifyEnumerations.PricingOptions = PersonifyEnumerations.PricingOptions.DefaultPricing

            If ((Not SubSystemPricingOption Is Nothing) AndAlso (SubSystemPricingOption.Length > 0)) Then
                Dim num2 As Integer = (SubSystemPricingOption.Length - 1)
                Dim i As Integer = 0
                Do While (i <= num2)
                    If _
                        ((Not SubSystemPricingOption(i) Is Nothing) AndAlso
                         (oWebProductInfo.Item(0).Subsystem.ToLower = SubSystemPricingOption(i).Subsystem.ToLower)) _
                        Then
                        defaultPricing = SubSystemPricingOption(i).PricingOption
                        Exit Do
                    End If
                    i += 1
                Loop
            End If
            oProductDetailHashTable.Add("ProductDetails_WebProductInfo", oWebProductInfo)
            If (FetchCrossSell AndAlso SubsystemListForCrossSell.Contains(oWebProductInfo.Item(0).Subsystem)) Then
                oProductDetailHashTable.Add("ProductDetails_CrossSell", Me.GetRelatedProducts_CrossSell(ProductId))
            End If
            If (FetchUpSell AndAlso SubsystemListForUpSell.Contains(oWebProductInfo.Item(0).Subsystem)) Then
                oProductDetailHashTable.Add("ProductDetails_UpSell", Me.GetRelatedProducts_UpSell(ProductId))
            End If
            Dim productComponentsWithPrice As ProductComponentList = Nothing
            Dim listPrices As WebPrices = Nothing
            Dim memberPrices As WebPrices = Nothing
            Dim yourPrices As WebPrices = Nothing
            If ((oWebProductInfo.Item(0).ProductTypeCode.Code = "P") OrElse FetchComponents) Then
                productComponentsWithPrice = Me.GetProductComponentsWithPrice(oWebProductInfo.Item(0))
                oProductDetailHashTable.Add("ProductDetails_Components", productComponentsWithPrice)
                Me.SetProductWithAllDefaultPrices(oWebProductInfo.Item(0), listPrices, memberPrices, yourPrices,
                                                   defaultPricing, productComponentsWithPrice, True, True, True)
            Else
                Me.SetProductWithAllDefaultPrices(oWebProductInfo.Item(0), listPrices, memberPrices, yourPrices,
                                                   defaultPricing, Nothing, True, True, True)
            End If
            If FetchListPrice Then
                oProductDetailHashTable.Add("ProductDetails_ListPrices", listPrices)
            End If
            If FetchMemberPrice Then
                oProductDetailHashTable.Add("ProductDetails_MemberPrices", memberPrices)
            End If
            If FetchCustomerPrice Then
                Dim flag As Boolean = (defaultPricing = PersonifyEnumerations.PricingOptions.MultiplePricing)
                oProductDetailHashTable.Add("ProductDetails_CustomerPrice", yourPrices)
            End If
            If (FetchECDFiles AndAlso (oWebProductInfo.Item(0).Subsystem = "ECD")) Then
                oProductDetailHashTable.Add("ProductDetails_ECDFileInfo", Me.GetDCDWebProductInfo(ProductId))
            End If
            Dim details2 As New ProductDetails(oProductDetailHashTable)
            If FetchSubProducts Then
                oProductDetailHashTable.Add("ProductDetails_SubProducts",
                                             Me.GetSubProductsDetails(ProductId, False, False, FetchListPrice,
                                                                       FetchMemberPrice, FetchAllPrices,
                                                                       SubSystemPricingOption, FetchCustomerPrice,
                                                                       MasterCustomerId, SubCustomerId, False, False,
                                                                       False, FetchMeetingInfo, FetchRelatedCustomers,
                                                                       SubProductTypeFilter))
            End If
            If (FetchMeetingInfo AndAlso (oWebProductInfo.Item(0).Subsystem = "MTG")) Then
                oProductDetailHashTable.Add("ProductDetails_MeetingInfo", Me.GetMeetingInfo(ProductId))
            End If
            If FetchRelatedCustomers Then
                oProductDetailHashTable.Add("ProductDetails_RelatedCustomers",
                                             Me.GetRelatedCustomers(ProductId, MasterCustomerId, SubCustomerId))
            End If
            Return New ProductDetails(oProductDetailHashTable)
        End Function

        Public Function GetBestQualifiedRateStructureForLoginUserArray() As String()
            If (Me._LoadBestQualifiedRateStructureForCustomerArray Is Nothing) Then
                Me._LoadBestQualifiedRateStructureForCustomerArray =
                    Me.LoadBestQualifiedRateStructureForCustomer (Me.MCID, Me.SCID)
            End If
            Return Me._LoadBestQualifiedRateStructureForCustomerArray.ToArray
        End Function

        Private Function GetCalculatedComponentProductRateCodes (ByVal PID As Long) As IProductRateCodes
            Dim codes2 As IProductRateCodes = DirectCast ([Global].GetCollection (Me.OrganizationId,
                                                                                  Me.OrganizationUnitId,
                                                                                  NamespaceEnum.ProductInfo,
                                                                                  "ProductRateCodes"),
                                                          IProductRateCodes)
            codes2.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
            codes2.Filter.Add ("ProductId", Conversions.ToString (PID))
            codes2.Filter.Add ("DefaultRateWebFlag", "Y")
            codes2.Fill
            Return codes2
        End Function

        Public Overridable Function GetDCDWebProductInfo (ByVal ProductId As Integer) _
            As IDigitalContentDeliverySetupList
            Dim builder As New StringBuilder
            Dim key As String = (ProductId.ToString & "GetDCDWebProductInfo")
            Dim cacheObject As IDigitalContentDeliverySetupList = DirectCast (PersonifyDataCache.Fetch (key),
                                                                              IDigitalContentDeliverySetupList)
            If (cacheObject Is Nothing) Then
                cacheObject = DirectCast(TIMSS.Global.GetCollection(Me.OrganizationId, Me.OrganizationUnitId,
                                                                NamespaceEnum.DigitalContentDeliveryInfo,
                                                                "DigitalContentDeliverySetupList"), 
                                          IDigitalContentDeliverySetupList)
                cacheObject.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
                cacheObject.Filter.Add ("ProductId", ProductId.ToString)
                cacheObject.Fill
                PersonifyDataCache.Store (key, cacheObject, PersonifyDataCache.CacheExpirationInterval)
            End If
            Return cacheObject
        End Function

        Public Overridable Function GetFacilityInfo (ByVal facMCID As String, ByVal facSCID As Integer) _
            As FacilityInformation
            Dim key As String = ("facilityinfo" & facMCID & facMCID)
            Dim builder As New StringBuilder
            If (facMCID.Length = 0) Then
                Return Nothing
            End If
            Dim cacheObject As FacilityInformation = DirectCast (PersonifyDataCache.Fetch (key), FacilityInformation)
            If (cacheObject Is Nothing) Then
                cacheObject = New FacilityInformation
                Dim obj2 As New SearchObject (Me.OrganizationId, Me.OrganizationUnitId)
                obj2.Target = [Global].GetCollection (Me.OrganizationId, Me.OrganizationUnitId,
                                                      NamespaceEnum.CustomerInfo, "CustomerAddressViewList")
                obj2.Parameters.Add (Me.SearchProperty ("MasterCustomerId", True, False, facMCID))
                obj2.Parameters.Add (Me.SearchProperty ("SubCustomerId", True, False, Conversions.ToString (facSCID)))
                obj2.Parameters.Add (Me.SearchProperty ("PrioritySeq", True, False, Conversions.ToString (0)))
                obj2.Parameters.Add (Me.SearchProperty ("LabelName", False, True, ""))
                obj2.Parameters.Add (Me.SearchProperty ("City", False, True, ""))
                obj2.Parameters.Add (Me.SearchProperty ("State", False, True, ""))
                obj2.Search
                If (obj2.Results.Table.Rows.Count > 0) Then
                    cacheObject.FacilityMasterCustomerId = facMCID
                    cacheObject.FacilitySubCustomerId = facSCID
                    cacheObject.FacilityName =
                        Conversions.ToString (
                            IIf (
                                IsDBNull (
                                    RuntimeHelpers.GetObjectValue (obj2.Results.Table.Rows.Item (0).Item ("LabelName"))),
                                String.Empty,
                                RuntimeHelpers.GetObjectValue (obj2.Results.Table.Rows.Item (0).Item ("LabelName"))))
                    cacheObject.City =
                        Conversions.ToString (
                            IIf (
                                IsDBNull (RuntimeHelpers.GetObjectValue (obj2.Results.Table.Rows.Item (0).Item ("City"))),
                                String.Empty,
                                RuntimeHelpers.GetObjectValue (obj2.Results.Table.Rows.Item (0).Item ("City"))))
                    cacheObject.State =
                        Conversions.ToString (
                            IIf (
                                IsDBNull (RuntimeHelpers.GetObjectValue (obj2.Results.Table.Rows.Item (0).Item ("State"))),
                                String.Empty,
                                RuntimeHelpers.GetObjectValue (obj2.Results.Table.Rows.Item (0).Item ("State"))))
                End If
                PersonifyDataCache.Store (key, cacheObject, PersonifyDataCache.CacheExpirationInterval)
            End If
            Return cacheObject
        End Function

        Public Overridable Function GetFundRaisingInfo (ByVal ProductId As Integer) As IFundRaisingProduct
            Dim products As IFundRaisingProducts = Nothing
            Dim builder As New StringBuilder
            If (products Is Nothing) Then
                products = DirectCast ([Global].GetCollection (Me.OrganizationId, Me.OrganizationUnitId,
                                                               NamespaceEnum.FundRaisingInfo, "FundRaisingProducts"),
                                       IFundRaisingProducts)
                products.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
                products.Filter.Add ("ProductId", ProductId.ToString)
                products.Fill
                If (products.Count > 0) Then
                    Return products.Item (0)
                End If
            End If
            Return DirectCast (products, IFundRaisingProduct)
        End Function

        Public Sub GetListAndMemberRateStructures (ByRef ListRateStructure As String,
                                                   ByRef MemberRateStructure As String)
            Dim key As String = "LIST_RATE_STRUCTURE"
            Dim str2 As String = "MEMBER_RATE_STRUCTURE"
            If (Not PersonifyDataCache.Fetch (key) Is Nothing) Then
                ListRateStructure = DirectCast (PersonifyDataCache.Fetch (key), IRateStructure).RateStructure
            End If
            If (Not PersonifyDataCache.Fetch (str2) Is Nothing) Then
                MemberRateStructure = DirectCast (PersonifyDataCache.Fetch (str2), IRateStructure).RateStructure
            End If
            If ((ListRateStructure = "") OrElse (MemberRateStructure = "")) Then
                Dim structures As IRateStructures = DirectCast ([Global].GetCollection (Me.OrganizationId,
                                                                                        Me.OrganizationUnitId,
                                                                                        NamespaceEnum.ProductInfo,
                                                                                        "RateStructures"),
                                                                IRateStructures)
                structures.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
                structures.Fill
                Dim num2 As Integer = (structures.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num2)
                    If structures.Item (i).DefaultStructureFlag Then
                        PersonifyDataCache.Store (key, structures.Item (i), PersonifyDataCache.CacheExpirationInterval)
                        ListRateStructure = structures.Item (i).RateStructure
                    End If
                    If structures.Item (i).MemberPriceFlag Then
                        PersonifyDataCache.Store (str2, structures.Item (i), PersonifyDataCache.CacheExpirationInterval)
                        MemberRateStructure = structures.Item (i).RateStructure
                    End If
                    i += 1
                Loop
            End If
        End Sub

        Public Overridable Function GetMeetingInfo (ByVal ProductId As Integer) As IMeetingProducts
            Dim builder As New StringBuilder
            Dim key As String = (ProductId.ToString & "GetMeetingInfo")
            Dim cacheObject As IMeetingProducts = DirectCast (PersonifyDataCache.Fetch (key), IMeetingProducts)
            If (cacheObject Is Nothing) Then
                cacheObject = DirectCast ([Global].GetCollection (Me.OrganizationId, Me.OrganizationUnitId,
                                                                  NamespaceEnum.MeetingInfo, "MeetingProducts"),
                                          IMeetingProducts)
                cacheObject.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
                cacheObject.Filter.Add ("ProductId", ProductId.ToString)
                cacheObject.Fill
                PersonifyDataCache.Store (key, cacheObject, PersonifyDataCache.CacheExpirationInterval)
            End If
            Return cacheObject
        End Function

        Public Overridable Function GetMembershipInfo (ByVal ProductId As Integer) As IMembershipProduct
            Dim products As IMembershipProducts = Nothing
            Dim builder As New StringBuilder
            If (products Is Nothing) Then
                products = DirectCast ([Global].GetCollection (Me.OrganizationId, Me.OrganizationUnitId,
                                                               NamespaceEnum.MembershipInfo, "MembershipProducts"),
                                       IMembershipProducts)
                products.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
                products.Filter.Add ("ProductId", ProductId.ToString)
                products.Fill
                If (products.Count > 0) Then
                    Return products.Item (0)
                End If
            End If
            Return DirectCast (products, IMembershipProduct)
        End Function

        Private Function GetPricingRange (ByVal ScheduleId As String, ByRef oCurrencyWebPrice As WebPrice) As String
            Dim request As SimpleRequest = Nothing
            Dim builder As New StringBuilder
            Dim set2 As RequestSet = Nothing
            Dim set3 As IResultSet = Nothing
            Dim set1 As DataSet = Nothing
            Dim str2 As String = String.Empty
            Try
                Dim builder2 As StringBuilder = builder
                builder2.Append (
                    " SELECT MIN(MDSD.DUES_AMOUNT) as MINDUESAMOUNT, MAX(MDSD.DUES_AMOUNT)as MAXDUESAMOUNT ")
                builder2.Append (" FROM MBR_DUES_SCHEDULE MDS, MBR_DUES_SCHEDULE_DETAIL MDSD WHERE ")
                builder2.Append (" MDS.SCHEDULE_ID = MDSD.SCHEDULE_ID AND ")
                builder2.Append(("MDS.ACTIVE_FLAG = " & TIMSS.Common.Functions.QuoteString("Y") & " AND "))
                builder2.Append(("MDS.SCHEDULE_ID = " & TIMSS.Common.Functions.QuoteString(ScheduleId)))
                builder2 = Nothing
                request = New SimpleRequest ("AddressCount", builder.ToString, Nothing)
                set2 = New RequestSet
                set1 = TIMSS.Global.App.GetData(request).Unpack
                If _
                    (((Not set1 Is Nothing) AndAlso (set1.Tables.Count > 0)) AndAlso
                     (set1.Tables.Item(0).Rows.Count > 0)) Then
                    oCurrencyWebPrice.MinScheduledPrice =
                        Conversions.ToDecimal(set1.Tables.Item(0).Rows.Item(0).Item("MINDUESAMOUNT").ToString)
                    oCurrencyWebPrice.MaxScheduledPrice =
                        Conversions.ToDecimal(set1.Tables.Item(0).Rows.Item(0).Item("MAXDUESAMOUNT").ToString)
                    str2 =
                        (oCurrencyWebPrice.MinScheduledPrice.ToString & "," &
                         oCurrencyWebPrice.MaxScheduledPrice.ToString)
                    oCurrencyWebPrice.HasValidSchedulePrice = True
                    Return str2
                End If
                oCurrencyWebPrice.HasValidSchedulePrice = False
            Finally
                If (Not set1 Is Nothing) Then
                    set1.Clear()
                    set1.Dispose()
                    set1 = Nothing
                    set2 = Nothing
                    builder = Nothing
                    set3 = Nothing
                    request = Nothing
                End If
            End Try
            Return str2
        End Function

        Private Function GetProductComponentsWithPrice (ByVal oProd As ITmarWebProductView) As ProductComponentList
            Dim table As DataTable
            Dim enumerator As IEnumerator
            Dim parameters As New Hashtable
            Dim num2 As New Decimal
            Dim num3 As New Decimal
            Dim num4 As New Decimal
            Dim num As New Decimal
            Dim key As String = (oProd.ProductId.ToString & "Components")
            If (PersonifyDataCache.Fetch (key) Is Nothing) Then
                parameters.Add ("P_ProductId", oProd.ProductId)
                table =
                    [Global].App.Execute (Me.GetSelectRequest_GetAllProductComponents (CInt (oProd.ProductId)),
                                          parameters).DataSet.Tables.Item (0)
                PersonifyDataCache.Store (key, table, PersonifyDataCache.CacheExpirationInterval)
            Else
                table = DirectCast (PersonifyDataCache.Fetch (key), DataTable)
            End If
            If (table Is Nothing) Then
                Return Nothing
            End If
            Dim list2 As New ProductComponentList
            Try
                enumerator = table.Rows.GetEnumerator
                Do While enumerator.MoveNext
                    Dim current As DataRow = DirectCast (enumerator.Current, DataRow)
                    Dim component2 As ProductComponent = list2.AddNewComponent
                    component2.MasterProductId = CInt (oProd.ProductId)
                    component2.ProductId = Conversions.ToInteger (current.Item ("component_product_id"))
                    component2.ProductCode = current.Item ("product_code").ToString
                    component2.ParentProduct = current.Item ("parent_product").ToString
                    component2.SubSystem = current.Item ("subsystem").ToString
                    component2.ShortName = current.Item ("short_name").ToString
                    component2.LongName = current.Item ("long_name").ToString
                    component2.ProductType = current.Item ("product_type_code").ToString
                    component2.ComponentQuantity = Conversions.ToInteger (current.Item ("component_qty"))
                    If (current.Item ("web_product_id") Is DBNull.Value) Then
                        component2.IsWebEnabledProduct = False
                    Else
                        component2.IsWebEnabledProduct = True
                    End If
                    component2.SmallImageFileName = current.Item ("small_image_file_name").ToString
                    component2.WebShortDescription = current.Item ("web_short_description").ToString
                    component2.WebLongDescription = current.Item ("web_long_description").ToString
                    If (oProd.ProductTypeCode.Code = "P") Then
                        Dim str2 As String = Conversions.ToString (current.Item ("RATE_METHOD_CODE"))
                        Dim left As Object = current.Item ("RATE_METHOD_CODE")
                        If Operators.ConditionalCompareObjectEqual (left, "CALCULATED", False) Then
                            Dim listPrices As WebPrices = Nothing
                            Dim memberPrices As WebPrices = Nothing
                            Dim yourPrices As WebPrices = Nothing
                            Me.SetComponentProductWithAllDefaultPrices (CLng (component2.ProductId), listPrices,
                                                                        memberPrices, yourPrices)
                            component2.ListPrice = listPrices.Item (0).Price
                            component2.MemberPrice = memberPrices.Item (0).Price
                            component2.CustomerPrice = yourPrices.Item (0).Price
                        ElseIf Operators.ConditionalCompareObjectEqual (left, "SPECIFIC", False) Then
                            Dim price As Decimal
                            key = String.Concat (New Object() _
                                                    {oProd.ProductId.ToString, "_",
                                                     RuntimeHelpers.GetObjectValue (current.Item ("RATE_STRUCTURE")),
                                                     "_", RuntimeHelpers.GetObjectValue (current.Item ("RATE_CODE"))})
                            If (PersonifyDataCache.Fetch (key) Is Nothing) Then
                                Dim codes As IProductRateCodes =
                                        Me.GetSpecificComponentRateCodes (CLng (component2.ProductId),
                                                                          Conversions.ToString (
                                                                              current.Item ("RATE_STRUCTURE")),
                                                                          Conversions.ToString (
                                                                              current.Item ("RATE_CODE")))
                                If ((Not codes Is Nothing) AndAlso (codes.Count > 0)) Then
                                    Dim ratecode As IProductRateCode = codes.Item (0)
                                    price = Me.ConstructComponentWebPrice (ratecode).Item (0).Price
                                    PersonifyDataCache.Store (key, price, PersonifyDataCache.CacheExpirationInterval)
                                End If
                            Else
                                price = Conversions.ToDecimal (PersonifyDataCache.Fetch (key))
                            End If
                            component2.ListPrice = price
                            component2.MemberPrice = price
                            component2.CustomerPrice = price
                        End If
                        num2 = Decimal.Add (num2, component2.ListPrice)
                        num3 = Decimal.Add (num3, component2.MemberPrice)
                        num4 = Decimal.Add (num4, component2.CustomerPrice)
                    Else
                        component2.ListPrice = Decimal.Zero
                        component2.MemberPrice = Decimal.Zero
                        component2.CustomerPrice = Decimal.Zero
                    End If
                    component2 = Nothing
                Loop
            Finally
                If TypeOf enumerator Is IDisposable Then
                    TryCast (enumerator, IDisposable).Dispose
                End If
            End Try
            If (oProd.ProductTypeCode.Code = "P") Then
                If (Decimal.Compare (oProd.PackageDiscountPercent, Decimal.Zero) > 0) Then
                    num2 = Decimal.Multiply (num2,
                                             Decimal.Subtract (Decimal.One,
                                                               Decimal.Divide (oProd.PackageDiscountPercent, 100)))
                    num3 = Decimal.Multiply (num3,
                                             Decimal.Subtract (Decimal.One,
                                                               Decimal.Divide (oProd.PackageDiscountPercent, 100)))
                    num4 = Decimal.Multiply (num4,
                                             Decimal.Subtract (Decimal.One,
                                                               Decimal.Divide (oProd.PackageDiscountPercent, 100)))
                End If
                list2.TotalPackageListPrice = num2
                list2.TotalPackageMemberPrice = num3
                list2.TotalPackageYourPrice = num4
            End If
            Return list2
        End Function

        Private Function GetProductInfo (ByVal PID As Integer) As IProducts
            Dim products As IProducts = DirectCast ([Global].GetCollection (Me.OrganizationId, Me.OrganizationUnitId,
                                                                            NamespaceEnum.ProductInfo, "Products"),
                                                    IProducts)
            products.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
            products.Filter.Add ("ProductId", Conversions.ToString (PID))
            products.Fill
            Return products
        End Function

        <
            Obsolete _
                (
                    "The function will be depracated. Use Individual Functions GetProductPricesForListRateStructure and GetProductPricesForMemberRateStructure Instead")
            >
        Public Overridable Function GetProductListAndMemberPrices (ByVal Mode As String, ByVal ProductId As Integer,
                                                                   ByVal Pricingoptions As _
                                                                      PersonifyEnumerations.PricingOptions) As WebPrices
            Dim str3 As String = "USD"
            Dim listRateStructure As String = ""
            Dim memberRateStructure As String = ""
            Dim cacheObject As WebPrices = Nothing
            Dim prices3 As WebPrices = Nothing
            Dim key As String = (ProductId & Pricingoptions.ToString & "ListPrices")
            Dim str2 As String = (ProductId & Pricingoptions.ToString & "MemberPrices")
            If (Not PersonifyDataCache.Fetch (key) Is Nothing) Then
                cacheObject = DirectCast (PersonifyDataCache.Fetch (key), WebPrices)
            End If
            If (Not PersonifyDataCache.Fetch (str2) Is Nothing) Then
                prices3 = DirectCast (PersonifyDataCache.Fetch (str2), WebPrices)
            End If
            If ((cacheObject Is Nothing) OrElse (prices3 Is Nothing)) Then
                Dim price As WebPrice
                Me.GetListAndMemberRateStructures (listRateStructure, memberRateStructure)
                Dim productInfo As IProducts = Me.GetProductInfo (ProductId)
                If productInfo.Item (0).IsPackageProduct Then
                    Dim defaultPrice As Decimal = productInfo.Item (0).GetDefaultPrice ("USD", True)
                    cacheObject = New WebPrices
                    price = cacheObject.AddNewWebPrice
                    price.ProductId = ProductId.ToString
                    price.RateStructure = listRateStructure
                    price.Price = defaultPrice
                    defaultPrice = productInfo.Item (0).GetMemberPrice ("USD", True)
                    prices3 = New WebPrices
                    price = prices3.AddNewWebPrice
                    price.ProductId = ProductId.ToString
                    price.RateStructure = memberRateStructure
                    price.Price = defaultPrice
                    price.IsDefault = True
                    price.MaxBadges = 0
                Else
                    Dim expression As String = ""
                    expression = String.Concat (New String() _
                                                   {"Rate_Structure in ( '", listRateStructure, "','",
                                                    memberRateStructure, "')"})
                    Dim codes As IProductRateCodes = DirectCast ([Global].GetCollection (Me.OrganizationId,
                                                                                         Me.OrganizationUnitId,
                                                                                         NamespaceEnum.ProductInfo,
                                                                                         "ProductRateCodes"),
                                                                 IProductRateCodes)
                    codes.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
                    codes.Filter.Add ("ProductId", ProductId.ToString)
                    codes.Filter.Add ("EcommerceFlag", "Y")
                    If ((listRateStructure.Length > 0) AndAlso (memberRateStructure.Length > 0)) Then
                        codes.Filter.Add (New FilterItem (expression))
                    End If
                    codes.Fill()
                    cacheObject = New WebPrices
                    prices3 = New WebPrices
                    Dim num4 As Integer = (codes.Count - 1)
                    Dim i As Integer = 0
                    Do While (i <= num4)
                        Dim num5 As Integer = (codes.Item (i).PricingList.Count - 1)
                        Dim j As Integer = 0
                        Do While (j <= num5)
                            If _
                                ((codes.Item (i).PricingList.Item (j).CurrencyCode.Code = str3) AndAlso
                                 codes.Item (i).PricingList.Item (j).IsActive) Then
                                If _
                                    ((codes.Item (i).RateStructure.Code = listRateStructure) AndAlso
                                     ((Pricingoptions = Pricingoptions.MultiplePricing) OrElse
                                      ((Pricingoptions = Pricingoptions.DefaultPricing) And
                                       codes.Item (i).DefaultRateWebFlag))) Then
                                    price = cacheObject.AddNewWebPrice
                                    price.ProductId = ProductId.ToString
                                    price.RateCode = codes.Item (i).RateCode.Code
                                    price.RateCodeDescr = codes.Item (i).RateCode.Description
                                    price.RateStructure = codes.Item (i).RateStructure.Code
                                    price.RateStructureDescr = codes.Item (i).RateStructure.Description
                                    price.Price = codes.Item (i).PricingList.Item (j).Price
                                    price.IsDefault = codes.Item (i).DefaultRateWebFlag
                                    price.MaxBadges = codes.Item (i).MaxBadges
                                End If
                                If _
                                    ((codes.Item (i).RateStructure.Code = memberRateStructure) AndAlso
                                     ((Pricingoptions = Pricingoptions.MultiplePricing) OrElse
                                      ((Pricingoptions = Pricingoptions.DefaultPricing) And
                                       codes.Item (i).DefaultRateWebFlag))) Then
                                    price = prices3.AddNewWebPrice
                                    price.ProductId = ProductId.ToString
                                    price.RateCode = codes.Item (i).RateCode.Code
                                    price.RateCodeDescr = codes.Item (i).RateCode.Description
                                    price.RateStructure = codes.Item (i).RateStructure.Code
                                    price.RateStructureDescr = codes.Item (i).RateStructure.Description
                                    price.Price = codes.Item (i).PricingList.Item (j).Price
                                    price.IsDefault = codes.Item (i).DefaultRateWebFlag
                                    price.MaxBadges = codes.Item (i).MaxBadges
                                End If
                            End If
                            j += 1
                        Loop
                        i += 1
                    Loop
                End If
                PersonifyDataCache.Store (key, cacheObject, PersonifyDataCache.CacheExpirationInterval)
                PersonifyDataCache.Store (str2, prices3, PersonifyDataCache.CacheExpirationInterval)
            End If
            If ((Mode <> "LIST") AndAlso (Mode = "MEMBER")) Then
                Return prices3
            End If
            Return cacheObject
        End Function

        Public Function GetProductListDefaultPrices (ByVal PID As Long,
                                                     ByVal PricingOption As PersonifyEnumerations.PricingOptions) _
            As WebPrices
            Dim prices2 As WebPrices
            Dim prices3 As WebPrices
            Dim prices4 As WebPrices
            Dim productIds As String() = New String() {Conversions.ToString (PID)}
            Dim list As ITmarWebProductViewList =
                    CachedApplicationData.ApplicationDataCache.GetWebEnabledProductsWithPriceFromCache(productIds,
                                                                                                            Me.
                                                                                                               GetBestQualifiedRateStructureForLoginUserArray,
                                                                                                            Me.
                                                                                                               _PortalCurrency _
                                                                                                               .Code,
                                                                                                            Me.
                                                                                                               _BaseCurrency _
                                                                                                               .Code,
                                                                                                            Me.
                                                                                                               DefaultListRateStructure,
                                                                                                            Me.
                                                                                                               DefaultMemberRateStructure)
            Me.SetProductWithAllDefaultPrices (list.Item (0), prices2, prices3, prices4, PricingOption, Nothing, True, False, False)
            Return prices2
        End Function

        Public Overridable Function GetProductListing_KeysOnly (ByVal ProductClass As String) As String()
            Return _
                Me.GetSelectRequest_GetProductIdsGeneric (ProductClass, "", False, False,
                                                          PersonifyEnumerations.MembersOnlyFilter.Both, 0)
        End Function

        Public Overridable Function GetProductListing_KeysOnly (ByVal ProductCategory As String,
                                                                ByVal ProductSubCategory As String) As String()
            Dim parameters As New Hashtable
            parameters.Add ("P_Category", ProductCategory)
            parameters.Add ("P_SubCategory", ProductSubCategory)
            Dim dataSet As DataSet =
                    [Global].App.Execute (
                        Me.GetSelectRequest_GetProductIdsForCategory (ProductCategory, ProductSubCategory), parameters).
                    DataSet
            Dim builder As New StringBuilder
            If (dataSet.Tables.Count > 0) Then
                Dim num2 As Integer = (dataSet.Tables.Item (0).Rows.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num2)
                    If (i > 0) Then
                        builder.Append (",")
                    End If
                    builder.Append (
                        RuntimeHelpers.GetObjectValue (dataSet.Tables.Item (0).Rows.Item (i).Item ("Product_id")))
                    i += 1
                Loop
            End If
            Return builder.ToString.Split (New Char() {","c})
        End Function

        Public Overridable Function GetProductListing_KeysOnly (ByVal SubSystem As String, ByVal Featured As Boolean,
                                                                ByVal Promotional As Boolean,
                                                                ByVal MembersOnlyFiltering As _
                                                                   PersonifyEnumerations.MembersOnlyFilter,
                                                                ByVal MaxRows As Integer) As String()
            Return _
                Me.GetSelectRequest_GetProductIdsGeneric ("", SubSystem, Promotional, Featured, MembersOnlyFiltering,
                                                          MaxRows)
        End Function

        Public Function GetProductMemberDefaultPrices (ByVal PID As Long,
                                                       ByVal PricingOption As PersonifyEnumerations.PricingOptions) _
            As WebPrices
            Dim prices2 As WebPrices
            Dim prices3 As WebPrices
            Dim prices4 As WebPrices
            Dim productIds As String() = New String() {Conversions.ToString (PID)}
            Dim list As ITmarWebProductViewList =
                    CachedApplicationData.ApplicationDataCache.GetWebEnabledProductsWithPriceFromCache(productIds,
                                                                                                            Me.
                                                                                                               GetBestQualifiedRateStructureForLoginUserArray,
                                                                                                            Me.
                                                                                                               _PortalCurrency _
                                                                                                               .Code,
                                                                                                            Me.
                                                                                                               _BaseCurrency _
                                                                                                               .Code,
                                                                                                            Me.
                                                                                                               DefaultListRateStructure,
                                                                                                            Me.
                                                                                                               DefaultMemberRateStructure)
            Me.SetProductWithAllDefaultPrices (list.Item (0), prices2, prices3, prices4, PricingOption, Nothing, False,
                                               True, False)
            Return prices3
        End Function

        Public Function GetProductSpecificRatePrices (ByVal PID As Long, ByVal RateStructure As String,
                                                      ByVal RateCode As String) As WebPrices
            Dim list As List(Of YourPriceItem) =
                    CachedApplicationData.ApplicationDataCache.LoadProductPricesFromCache(
                        Conversions.ToString(PID), RateStructure)
            Dim num2 As Integer = (list.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num2)
                If (list.Item (i).RateCode = RateCode) Then
                    Dim prices2 As New WebPrices
                    Dim price As WebPrice = prices2.AddNewWebPrice
                    price.ProductId = Conversions.ToString (PID)
                    price.IsDefault = list.Item (i).IsDefault
                    price.MaxBadges = list.Item (i).MaxBadges
                    price.Price = list.Item (i).Price
                    Return prices2
                End If
                i += 1
            Loop
            Return Nothing
        End Function

        Public Function GetProductYourDefaultPrices(ByVal PID As Long, ByVal PricingOption As PersonifyEnumerations.PricingOptions) _
            As WebPrices
            Dim prices2 As WebPrices
            Dim prices3 As WebPrices
            Dim prices4 As WebPrices
            Dim productIds As String() = New String() {Conversions.ToString(PID)}
            Dim list As ITmarWebProductViewList =
                    CachedApplicationData.ApplicationDataCache.GetWebEnabledProductsWithPriceFromCache(productIds,
                                                                                                            Me.
                                                                                                               GetBestQualifiedRateStructureForLoginUserArray,
                                                                                                            Me.
                                                                                                               _PortalCurrency _
                                                                                                               .Code,
                                                                                                            Me.
                                                                                                               _BaseCurrency _
                                                                                                               .Code,
                                                                                                            Me.
                                                                                                               DefaultListRateStructure,
                                                                                                            Me.
                                                                                                               DefaultMemberRateStructure)
            Me.SetProductWithAllDefaultPrices(list.Item(0), prices2, prices3, prices4, PricingOption, Nothing, False,
                                               False, True)
            Return prices4
        End Function

        Public Overridable Function GetRelatedCustomers (ByVal ProductId As Integer, ByVal MasterCustomerID As String,
                                                         ByVal SubcustomerID As Integer) As IProductRelatedCustomers
            Dim key As String = (ProductId.ToString & "RelatedCustomers")
            Dim cacheObject As IProductRelatedCustomers = DirectCast (PersonifyDataCache.Fetch (key),
                                                                      IProductRelatedCustomers)
            If (cacheObject Is Nothing) Then
                Dim webProductInfo As ITmarWebProductViewList = Me.GetWebProductInfo (ProductId)
                If ((Not webProductInfo Is Nothing) AndAlso (webProductInfo.Count > 0)) Then
                    cacheObject = webProductInfo.Item (0).RelatedCustomers
                End If
                PersonifyDataCache.Store (key, cacheObject, PersonifyDataCache.CacheExpirationInterval)
            End If
            Return cacheObject
        End Function

        Public Overridable Function GetRelatedProducts_CrossSell (ByVal ProductId As Integer) As ITmarWebProductViewList
            Dim list3 As ITmarWebProductViewList = Nothing
            Dim key As String = (ProductId.ToString & "CrossSell")
            Dim cacheObject As List(Of String) = DirectCast (PersonifyDataCache.Fetch (key), List(Of String))
            If ((cacheObject Is Nothing) OrElse (cacheObject.Count = 0)) Then
                cacheObject = New List(Of String)
                Dim relations As IProductRelations = DirectCast ([Global].GetCollection (Me.OrganizationId,
                                                                                         Me.OrganizationUnitId,
                                                                                         NamespaceEnum.ProductInfo,
                                                                                         "ProductRelations"),
                                                                 IProductRelations)
                relations.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
                relations.Filter.Add ("RelationTypeCode", "CROSS")
                relations.Filter.Add ("ProductId", ProductId.ToString)
                relations.Fill
                Dim num2 As Integer = (relations.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num2)
                    If relations.Item (i).IsActive Then
                        cacheObject.Add (Conversions.ToString (relations.Item (i).RelatedProductId))
                    End If
                    i += 1
                Loop
                PersonifyDataCache.Store (key, cacheObject, PersonifyDataCache.CacheExpirationInterval)
            End If
            If ((Not cacheObject Is Nothing) AndAlso (cacheObject.Count > 0)) Then
                list3 =
                    CachedApplicationData.ApplicationDataCache.GetWebEnabledProductsWithPriceFromCache(
                        cacheObject.ToArray, Me.GetBestQualifiedRateStructureForLoginUserArray, Me._PortalCurrency.Code,
                        Me._BaseCurrency.Code, Me.DefaultListRateStructure, Me.DefaultMemberRateStructure)
            End If
            Return list3
        End Function

        Public Overridable Function GetRelatedProducts_UpSell (ByVal ProductId As Integer) As ITmarWebProductViewList
            Dim list3 As ITmarWebProductViewList = Nothing
            Dim key As String = (ProductId.ToString & "UpSell")
            Dim cacheObject As List(Of String) = DirectCast (PersonifyDataCache.Fetch (key), List(Of String))
            If ((cacheObject Is Nothing) OrElse (cacheObject.Count = 0)) Then
                cacheObject = New List(Of String)
                Dim relations As IProductRelations = DirectCast ([Global].GetCollection (Me.OrganizationId,
                                                                                         Me.OrganizationUnitId,
                                                                                         NamespaceEnum.ProductInfo,
                                                                                         "ProductRelations"),
                                                                 IProductRelations)
                relations.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
                relations.Filter.Add ("RelationTypeCode", "UPSELL")
                relations.Filter.Add ("ProductId", ProductId.ToString)
                relations.Fill
                Dim num2 As Integer = (relations.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num2)
                    If relations.Item (i).IsActive Then
                        cacheObject.Add (Conversions.ToString (relations.Item (i).RelatedProductId))
                    End If
                    i += 1
                Loop
                PersonifyDataCache.Store (key, cacheObject, PersonifyDataCache.CacheExpirationInterval)
            End If
            If ((Not cacheObject Is Nothing) AndAlso (cacheObject.Count > 0)) Then
                list3 =
                    CachedApplicationData.ApplicationDataCache.GetWebEnabledProductsWithPriceFromCache(
                        cacheObject.ToArray, Me.GetBestQualifiedRateStructureForLoginUserArray, Me._PortalCurrency.Code,
                        Me._BaseCurrency.Code, Me.DefaultListRateStructure, Me.DefaultMemberRateStructure)
            End If
            Return list3
        End Function

        Private Function GetScheduledPriceRange (ByVal ScheduleId As String, ByRef oCurrencyWebPrice As WebPrice) _
            As String
            Dim pricingRange As String = Me.GetPricingRange (ScheduleId, oCurrencyWebPrice)
            If Not String.IsNullOrEmpty (pricingRange) Then
                Dim strArray As String() = pricingRange.Split (New Char() {","c})
                If String.Equals (strArray (0).Trim, strArray (1).Trim) Then
                    pricingRange = ("0," & strArray (0).Trim)
                End If
            End If
            Return pricingRange
        End Function

        Public Overridable Function GetSelectRequest_GetAllProductComponents (ByVal ProductId As Integer) _
            As IBaseRequest
            Dim request2 As ISelectRequest = New SqlObjects.SelectRequest ("GetAllProductComponents")
            Dim item As New SelectTable ("PRODUCT_COMPONENT", "PC")
            item.ResultColumns.Add ("component_product_id")
            item.ResultColumns.Add ("component_qty")
            item.ResultColumns.Add ("RATE_METHOD_CODE")
            item.ResultColumns.Add ("RATE_STRUCTURE")
            item.ResultColumns.Add ("RATE_CODE")
            Dim table As ISelectTable = New SelectTable ("PRODUCT", "p")
            table.ResultColumns.Add ("product_id")
            table.ResultColumns.Add ("short_name")
            table.ResultColumns.Add ("long_name")
            table.ResultColumns.Add ("product_id")
            table.ResultColumns.Add ("product_code")
            table.ResultColumns.Add ("parent_product")
            table.ResultColumns.Add ("subsystem")
            table.ResultColumns.Add ("product_type_code")
            table.ResultColumns.Add ("product_class_code")
            Dim table3 As ISelectTable = New SelectTable ("TMAR_WEB_PRODUCT_VW", "t")
            table3.ResultColumns.Add ("product_id", "web_product_id")
            table3.ResultColumns.Add ("small_image_file_name")
            table3.ResultColumns.Add ("web_long_description")
            table3.ResultColumns.Add ("web_short_description")
            request2.Tables.Add (item)
            request2.Tables.Add (table)
            request2.Tables.Add (table3)
            Dim join As New TableJoin ("Inner join", item, table, JoinType.InnerJoin)
            join.ColumnJoins.Add (New ColumnJoin ("component_product_id", "product_id"))
            request2.TableJoins.Add (join)
            Dim join2 As New TableJoin ("Left outer join", item, table3, JoinType.LeftOuterJoin)
            join2.ColumnJoins.Add (New ColumnJoin ("component_product_id", "product_id"))
            request2.TableJoins.Add (join2)
            request2.Parameters.Add ("PC", "product_id", "P_ProductId", ProductId)
            request2.Parameters.Add ("PC", "active_flag", "P_ActiveFlag", "Y")
            Return request2
        End Function

        Public Overridable Function GetSelectRequest_GetProductIdsForCategory (ByVal Optional pCategory As String = "",
                                                                               ByVal Optional pSubCategory As String =
                                                                                  "") As IBaseRequest
            Dim request2 As ISelectRequest = New SqlObjects.SelectRequest ("Select1")
            Dim item As ISelectTable = New SelectTable ("tmar_web_product_vw", "web")
            Dim table As ISelectTable = New SelectTable ("Product_Category", "pc")
            request2.Distinct = True
            item.ResultColumns.Add ("product_id", "product_id")
            request2.Tables.Add (item)
            request2.Tables.Add (table)
            Dim join As New TableJoin ("Normal join", item, table, JoinType.Normal)
            join.ColumnJoins.Add (New ColumnJoin ("product_id", "product_id"))
            request2.TableJoins.Add (join)
            If ((Not pCategory Is Nothing) AndAlso (pCategory.Length > 0)) Then
                request2.Parameters.Add ("pc", "Category", "P_Category", pCategory, ParameterDirection.Input,
                                         QueryOperatorType.Equals)
                If (pSubCategory.Length > 0) Then
                    Dim table2 As ISelectTable = New SelectTable ("Product_SUB_Category", "psc")
                    request2.Tables.Add (table2)
                    Dim join2 As New TableJoin ("Normal join", table, table2, JoinType.Normal)
                    join2.ColumnJoins.Add (New ColumnJoin ("product_category_id", "product_category_id"))
                    request2.TableJoins.Add (join2)
                    request2.Parameters.Add ("psc", "sub_category", "P_SubCategory", pSubCategory,
                                             ParameterDirection.Input, QueryOperatorType.Equals)
                End If
            End If
            Return request2
        End Function

        Public Overridable Function GetSelectRequest_GetProductIdsForClass (ByVal Optional pClass As String = "") _
            As IBaseRequest
            Dim request2 As ISelectRequest = New SqlObjects.SelectRequest ("Select1")
            Dim item As ISelectTable = New SelectTable ("tmar_web_product_vw", "web")
            request2.Distinct = True
            item.ResultColumns.Add ("product_id", "product_id")
            request2.Tables.Add (item)
            If (pClass.Length > 0) Then
                request2.Parameters.Add ("web", "Product_Class_code", "P_Class", pClass, ParameterDirection.Input,
                                         QueryOperatorType.Equals)
            End If
            Return request2
        End Function

        Public Overridable Function GetSelectRequest_GetProductIdsGeneric (
                                                                           Optional ByVal ProductClassCodes As String =
                                                                              "",
                                                                           Optional ByVal Subsystem As String = "",
                                                                           Optional ByVal FetchPromotionalProducts As _
                                                                              Boolean = False,
                                                                           Optional ByVal FetchFeaturedProducts As _
                                                                              Boolean = False,
                                                                           Optional ByVal MembersOnlyFiltering As _
                                                                              PersonifyEnumerations.MembersOnlyFilter =
                                                                              2,
                                                                           Optional ByVal MaximumRows As Integer = 0) _
            As String()
            Dim request As SimpleRequest = Nothing
            Dim builder2 As New StringBuilder
            Dim set1 As DataSet = Nothing
            Dim builder As New StringBuilder
            Dim serverDateTime As DateTime = [Global].App.ServerDateTime
            Dim list As New List(Of String)
            Dim builder3 As StringBuilder = builder
            If (MaximumRows > 0) Then
                builder3.Append ("s"c, MaximumRows)
            Else
                builder3.Append ("select product_id from tmar_web_product_vw ")
            End If
            builder3.Append (" where master_product_flag = 'Y' ")
            builder3.AppendFormat (" and ( currency_lock_code is null or currency_lock_code = '{0}' ) ",
                                   Me._PortalCurrency.Code)
            builder3.AppendFormat (" and (language_long = '{0}' or language_long is null )",
                                   [Global].App.Context.Language)
            builder3.AppendFormat (" and (language_short = '{0}' or language_short is null )",
                                   [Global].App.Context.Language)
            Select Case CInt (MembersOnlyFiltering)
                Case 0
                    builder3.Append (" and members_only_flag = 'N' ")
                    Exit Select
                Case 1
                    builder3.Append (" and members_only_flag = 'Y' ")
                    Exit Select
            End Select
            If (Subsystem.Length > 0) Then
                builder3.AppendFormat (" and subsystem in {0} ", Me.ConstructInClause (Subsystem))
                If Subsystem.Contains ("MTG") Then
                    builder3.AppendFormat (" and (LAST_REGISTRATION_DATE >= '{0}' or LAST_REGISTRATION_DATE is null ) ",
                                           serverDateTime)
                End If
            End If
            If (ProductClassCodes.Length > 0) Then
                builder3.AppendFormat (" and product_class_code in {0} ", Me.ConstructInClause (ProductClassCodes))
            End If
            If FetchFeaturedProducts Then
                builder3.AppendFormat (
                    " and SPECIAL_FEATURE_FLAG = 'Y' and SPECIAL_FEATURE_BEGIN_DATE <= '{0}'  and (SPECIAL_FEATURE_END_DATE >= '{0}'  or SPECIAL_FEATURE_END_DATE is null)",
                    serverDateTime)
            End If
            If FetchPromotionalProducts Then
                builder3.AppendFormat (
                    " and SPECIAL_PROMO_FLAG = 'Y' and SPECIAL_PROMO_BEGIN_DATE <= '{0}'  and (SPECIAL_PROMO_END_DATE >= '{0}'  or SPECIAL_PROMO_END_DATE is null)",
                    serverDateTime)
            End If
            If ((Me.OrganizationId.Length > 0) AndAlso (Me.OrganizationUnitId.Length > 0)) Then
                builder3.AppendFormat (" and ORG_ID = '{0}' and ORG_UNIT_ID = '{1}'", Me.OrganizationId.ToUpper,
                                       Me.OrganizationUnitId.ToUpper)
            End If
            builder3 = Nothing
            request = New SimpleRequest ("PIDS", builder.ToString, Nothing)
            set1 = TIMSS.Global.App.GetData(request).Unpack
            If ((set1.Tables.Count > 0) AndAlso (set1.Tables.Item(0).Rows.Count > 0)) Then
                Dim num2 As Integer = (set1.Tables.Item(0).Rows.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num2)
                    list.Add(set1.Tables.Item(0).Rows.Item(i).Item("Product_id").ToString)
                    i += 1
                Loop
            End If
            Return list.ToArray
        End Function

        Public Overridable Function GetSelectRequest_GetSubProducts (ByVal ProductCode As String) As String()
            Dim request As SimpleRequest = Nothing
            Dim builder2 As New StringBuilder
            Dim set1 As DataSet = Nothing
            Dim builder As New StringBuilder
            Dim serverDateTime As DateTime = TIMSS.Global.App.ServerDateTime
            Dim builder3 As StringBuilder = builder
            builder3.Append ("select Product_id from tmar_web_product_vw ")
            builder3.AppendFormat (" where parent_product = '{0}'", ProductCode)
            builder3.Append (" and parent_product <> product_code ")
            builder3.Append (" order by START_DATE ASC ")
            builder3 = Nothing
            request = New SimpleRequest ("PIDS", builder.ToString, Nothing)
            set1 = TIMSS.Global.App.GetData(request).Unpack
            If (set1.Tables.Count > 0) Then
                Dim num2 As Integer = (set1.Tables.Item(0).Rows.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num2)
                    If (i > 0) Then
                        builder2.Append(",")
                    End If
                    builder2.Append(
                        RuntimeHelpers.GetObjectValue(set1.Tables.Item(0).Rows.Item(i).Item("Product_id")))
                    i += 1
                Loop
            End If
            Return builder2.ToString.Split (New Char() {","c})
        End Function

        Private Function GetSpecificComponentRateCodes (ByVal PID As Long, ByVal Ratestructure As String,
                                                        ByVal RateCode As String) As IProductRateCodes
            Dim codes2 As IProductRateCodes = DirectCast(TIMSS.Global.GetCollection(Me.OrganizationId, Me.OrganizationUnitId,
                                                                                NamespaceEnum.ProductInfo,
                                                                                "ProductRateCodes"), 
                                                          IProductRateCodes)
            codes2.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
            codes2.Filter.Add ("ProductId", Conversions.ToString (PID))
            codes2.Filter.Add ("RateStructure", Ratestructure)
            codes2.Filter.Add ("RateCode", RateCode)
            codes2.Fill
            Return codes2
        End Function

        Public Overridable Function GetSubProducts (ByVal ProductId As Integer, ByVal SubProductClassFilter As String()) _
            As ITmarWebProductViewList
            Dim list2 As New ArrayList
            Dim key As String = (ProductId.ToString & "SubProductsKey")
            Dim cacheObject As String() = DirectCast (PersonifyDataCache.Fetch (key), String())
            If ((cacheObject Is Nothing) OrElse (cacheObject.Length = 0)) Then
                Dim webProductInfo As ITmarWebProductViewList = Me.GetWebProductInfo (ProductId)
                cacheObject = Me.GetSelectRequest_GetSubProducts (webProductInfo.Item (0).ParentProduct)
                PersonifyDataCache.Store (key, cacheObject, PersonifyDataCache.CacheExpirationInterval)
            End If
            If _
                (((cacheObject Is Nothing) OrElse (cacheObject.Length = 0)) OrElse
                 String.IsNullOrEmpty (cacheObject (0))) Then
                Return Nothing
            End If
            Dim list3 As ITmarWebProductViewList =
                    CachedApplicationData.ApplicationDataCache.GetWebEnabledProductsWithPriceFromCache(cacheObject,
                                                                                                            Me.
                                                                                                               GetBestQualifiedRateStructureForLoginUserArray,
                                                                                                            Me.
                                                                                                               _PortalCurrency _
                                                                                                               .Code,
                                                                                                            Me.
                                                                                                               _BaseCurrency _
                                                                                                               .Code,
                                                                                                            Me.
                                                                                                               DefaultListRateStructure,
                                                                                                            Me.
                                                                                                               DefaultMemberRateStructure)
            If ((SubProductClassFilter Is Nothing) OrElse (SubProductClassFilter.Length = 0)) Then
                Return list3
            End If
            Dim str2 As String
            For Each str2 In SubProductClassFilter
                Dim num4 As Integer = (list3.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num4)
                    If (list3.Item (i).ProductTypeCodeString.ToString = str2) Then
                        list2.Add (list3.Item (i))
                    End If
                    i += 1
                Loop
            Next
            Dim list4 As ITmarWebProductViewList = DirectCast ([Global].GetCollection (Me.OrganizationId,
                                                                                       Me.OrganizationUnitId,
                                                                                       NamespaceEnum.WebInfo,
                                                                                       "TmarWebProductViewList"),
                                                               ITmarWebProductViewList)
            If (list2.Count > 0) Then
                Dim num5 As Integer = (list2.Count - 1)
                Dim j As Integer = 0
                Do While (j <= num5)
                    list4.Add (DirectCast (list2.Item (j), ITmarWebProductView))
                    j += 1
                Loop
            End If
            Return list4
        End Function

        Public Overridable Function GetSubProductsDetails(ByVal ProductId As Integer,
                                                                 Optional ByVal FetchCrossSell As Boolean = True,
                                                                 Optional ByVal FetchUpSell As Boolean = True,
                                                                 Optional ByVal FetchListPrice As Boolean = True,
                                                                 Optional ByVal FetchMemberPrice As Boolean = True,
                                                                 Optional ByVal FetchAllPrices As Boolean = False,
                                                                 Optional ByVal SubSystemPricingOption As SubSystem_PricingOption() = Nothing,
                                                                 Optional ByVal FetchCustomerPrice As Boolean = False,
                                                                 Optional ByVal MasterCustomerId As String = "",
                                                                 Optional ByVal SubCustomerId As Integer = 0,
                                                                 Optional ByVal FetchComponents As Boolean = True,
                                                                 Optional ByVal FetchECDFiles As Boolean = False,
                                                                 Optional ByVal FetchSubProducts As Boolean = False,
                                                                 Optional ByVal FetchMeetingInfo As Boolean = False,
                                                                 Optional ByVal FetchRelatedCustomers As Boolean = False,
                                                                 Optional ByVal SubProductTypeFilter As String() = Nothing) As ProductDetails()

            If SubSystemPricingOption Is Nothing Then
                SubSystemPricingOption = New SubSystem_PricingOption() {}
            End If

            If SubProductTypeFilter Is Nothing Then
                SubProductTypeFilter = New String() {}
            End If

            Dim subProducts As ITmarWebProductViewList = Me.GetSubProducts(ProductId, SubProductTypeFilter)
            If (Not subProducts Is Nothing) Then
                If (subProducts.Count > 0) Then
                    Dim detailsArray2 As ProductDetails() = New ProductDetails(((subProducts.Count - 1) + 1) - 1) {}
                    Dim num2 As Integer = (subProducts.Count - 1)
                    Dim i As Integer = 0
                    Do While (i <= num2)
                        detailsArray2(i) = Me.GetAllDetailsForAProduct(
                            Convert.ToInt32(subProducts.Item(i).ProductId), FetchCrossSell, FetchUpSell,
                            FetchListPrice, FetchMemberPrice, FetchAllPrices, SubSystemPricingOption, FetchCustomerPrice,
                            MasterCustomerId, SubCustomerId, FetchComponents, FetchECDFiles, FetchSubProducts,
                            FetchMeetingInfo, FetchRelatedCustomers, Nothing, "", "")
                        i += 1
                    Loop
                    Return detailsArray2
                End If
                Return Nothing
            End If
            Return Nothing
        End Function

        Private Function GetWebPriceLists(ByVal oPriceLists As List(Of YourPriceItem), ByVal PID As Long, ByVal Subsystem As String) As WebPrices
            Dim prices2 As New WebPrices
            Dim num2 As Integer = (oPriceLists.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num2)
                Dim price As WebPrice = prices2.AddNewWebPrice
                price.RateStructure = oPriceLists.Item(i).RateStructure
                price.ProductId = Conversions.ToString(PID)
                price.MaxBadges = oPriceLists.Item(i).MaxBadges
                price.RateCode = oPriceLists.Item(i).RateCode
                price.RateCodeDescr =
                    CachedApplicationData.ApplicationDataCache.ApplicationCode(Subsystem, "RATE",
                                                                                    oPriceLists.Item(i).RateCode).
                        Description
                price.IsDefault = oPriceLists.Item(i).IsDefault
                price.Price = oPriceLists.Item(i).Price
                i += 1
            Loop
            Return prices2
        End Function

        Public Overridable Function GetWebProductInfo(ByVal ProductId As Integer) As ITmarWebProductViewList
            Dim productIds As String() = New String() {Conversions.ToString(ProductId)}
            Return _
                CachedApplicationData.ApplicationDataCache.GetWebEnabledProductsWithPriceFromCache(productIds,
                                                                                                        Me.
                                                                                                           GetBestQualifiedRateStructureForLoginUserArray,
                                                                                                        Me.
                                                                                                           _PortalCurrency _
                                                                                                           .Code,
                                                                                                        Me._BaseCurrency _
                                                                                                           .Code,
                                                                                                        Me.
                                                                                                           DefaultListRateStructure,
                                                                                                        Me.
                                                                                                           DefaultMemberRateStructure)
        End Function

        Private Function IsProductsExistInSameOrgOrgUnit(ByVal oWebProductInfo As ITmarWebProductViewList) As Boolean
            Dim predicates As New ListDictionary
            Dim dictionary2 As ListDictionary = predicates
            dictionary2.Add("OrganizationId", Me.OrganizationId)
            dictionary2.Add("OrganizationUnitId", Me.OrganizationUnitId)
            dictionary2 = Nothing
            Return (Not oWebProductInfo.FindObject(predicates) Is Nothing)
        End Function

        Public Function IsValidFundProduct(ByVal FundEventProductId As Integer) As Boolean
            Dim flag As Boolean
            Dim request As ISelectRequest
            Dim result As IQueryResult
            Try
                request = New SqlObjects.SelectRequest("FundEventProduct")
                Dim request2 As ISelectRequest = request
                request2.Tables.Add("FND_EVENT_PRODUCT")
                request2.Tables.Item("FND_EVENT_PRODUCT").ResultColumns.Add("FND_EVENT_PRODUCT_ID")
                request2.Parameters.Add(New ParameterItem("FND_EVENT_PRODUCT", "FND_EVENT_PRODUCT_ID",
                                                            "FND_EVENT_PRODUCT_ID", FundEventProductId.ToString))
                request2 = Nothing
                result = [Global].App.Execute(request, New Object(0 - 1) {})
                If _
                    ((result.Success AndAlso (Not result.DataSet Is Nothing)) AndAlso
                     ((result.DataSet.Tables.Count > 0) AndAlso (result.DataSet.Tables.Item(0).Rows.Count > 0))) Then
                    flag = True
                End If
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                ProjectData.ClearProjectError()
            Finally
                result = Nothing
                request = Nothing
            End Try
            Return flag
        End Function

        Private Function LoadBestQualifiedRateStructureForCustomer(ByVal MCID As String, ByVal SCID As Integer) _
            As List(Of String)

            Dim structure1 As New OrderDefaultStructure

            If _
                (SessionManager.GetSessionObject(Me._PortalId,
                                                  PersonifyEnumerations.SessionKeys.
                                                     PersonifyCustomerQualifiedRateStructures, MCID) Is Nothing) Then
                Dim objectToAdd As List(Of String) = Me.sp_LoadBestQualifiedRateStructureForCustomer(MCID, SCID)
                SessionManager.AddSessionObject(Me._PortalId,
                                                 PersonifyEnumerations.SessionKeys.
                                                    PersonifyCustomerQualifiedRateStructures, objectToAdd, MCID)
                Return objectToAdd
            End If
            Return _
                DirectCast(SessionManager.GetSessionObject(Me._PortalId,
                                                             PersonifyEnumerations.SessionKeys.
                                                                PersonifyCustomerQualifiedRateStructures, MCID), 
                            List(Of String))
        End Function

        Private Function SearchProperty(ByVal PropertyName As String, ByVal UseInQuery As Boolean,
                                         ByVal ShowInResults As Boolean, Optional ByVal PropertyValue As String = "") _
            As SearchProperty

            Dim property1 As New SearchProperty(PropertyName)

            property1.UseInQuery = UseInQuery
            property1.ShowInResults = ShowInResults
            property1.Value = PropertyValue
            Return property1
        End Function

        Private Sub SetComponentProductWithAllDefaultPrices(ByVal PID As Long, ByRef ListPrices As WebPrices,
                                                             ByRef MemberPrices As WebPrices,
                                                             ByRef YourPrices As WebPrices)
            Dim code As IProductRateCode
            Dim calculatedComponentProductRateCodes As IProductRateCodes = Nothing
            Dim price3 As WebPrice = Nothing
            calculatedComponentProductRateCodes = Me.GetCalculatedComponentProductRateCodes(PID)
            Dim key As String =
                    ("ProductComponentPrice" & PID.ToString & Me._PortalCurrency.Code & Me.DefaultListRateStructure)
            If (PersonifyDataCache.Fetch(key) Is Nothing) Then
                code = DirectCast(calculatedComponentProductRateCodes.FindObject("RateStructure",
                                                                                   Me.DefaultListRateStructure), 
                                   IProductRateCode)
                ListPrices = Me.ConstructComponentWebPrice(code)
                PersonifyDataCache.Store(key, ListPrices, PersonifyDataCache.CacheExpirationInterval)
            Else
                ListPrices = DirectCast(PersonifyDataCache.Fetch(key), WebPrices)
            End If
            key = ("ProductComponentPrice" & PID.ToString & Me._PortalCurrency.Code & Me.DefaultMemberRateStructure)
            If (PersonifyDataCache.Fetch(key) Is Nothing) Then
                If (calculatedComponentProductRateCodes Is Nothing) Then
                    calculatedComponentProductRateCodes = Me.GetCalculatedComponentProductRateCodes(PID)
                End If
                code = DirectCast(calculatedComponentProductRateCodes.FindObject("RateStructure",
                                                                                   Me.DefaultMemberRateStructure), 
                                   IProductRateCode)
                If (Not code Is Nothing) Then
                    MemberPrices = Me.ConstructComponentWebPrice(code)
                    PersonifyDataCache.Store(key, MemberPrices, PersonifyDataCache.CacheExpirationInterval)
                Else
                    PersonifyDataCache.Store(key, ListPrices, PersonifyDataCache.CacheExpirationInterval)
                End If
            Else
                MemberPrices = DirectCast(PersonifyDataCache.Fetch(key), WebPrices)
            End If
            Dim bestQualifiedRateStructureForLoginUserArray As String() =
                    Me.GetBestQualifiedRateStructureForLoginUserArray
            Dim num2 As Integer = (bestQualifiedRateStructureForLoginUserArray.Length - 1)
            Dim i As Integer = 0
            Do While (i <= num2)
                Dim str2 As String = bestQualifiedRateStructureForLoginUserArray(i)
                If (str2 = Me.DefaultListRateStructure) Then
                    YourPrices = New WebPrices
                    price3 = YourPrices.AddNewWebPrice
                    price3.Price = ListPrices.Item(0).Price
                    price3.RateStructure = ListPrices.Item(0).RateStructure
                    price3.RateCode = ListPrices.Item(0).RateCode
                    Exit Do
                End If
                If (str2 = Me.DefaultMemberRateStructure) Then
                    YourPrices = New WebPrices
                    price3 = YourPrices.AddNewWebPrice
                    price3.Price = MemberPrices.Item(0).Price
                    price3.RateStructure = MemberPrices.Item(0).RateStructure
                    price3.RateCode = MemberPrices.Item(0).RateCode
                    Exit Do
                End If
                key =
                    ("ProductComponentPrice" & PID.ToString & Me._PortalCurrency.Code &
                     bestQualifiedRateStructureForLoginUserArray(i))
                If (PersonifyDataCache.Fetch(key) Is Nothing) Then
                    If (calculatedComponentProductRateCodes Is Nothing) Then
                        calculatedComponentProductRateCodes = Me.GetCalculatedComponentProductRateCodes(PID)
                    End If
                    code = DirectCast(calculatedComponentProductRateCodes.FindObject("RateStructure",
                                                                                       bestQualifiedRateStructureForLoginUserArray(
                                                                                           i)), 
                                       IProductRateCode)
                    If (Not code Is Nothing) Then
                        YourPrices = Me.ConstructComponentWebPrice(code)
                        If (Not YourPrices Is Nothing) Then
                            PersonifyDataCache.Store(key, YourPrices, PersonifyDataCache.CacheExpirationInterval)
                            Exit Do
                        End If
                    End If
                Else
                    YourPrices = DirectCast(PersonifyDataCache.Fetch(key), WebPrices)
                    Exit Do
                End If
                i += 1
            Loop
        End Sub

        Private Sub SetListAndMemberRateStructures()
            Dim key As String = "LIST_RATE_STRUCTURE"
            Dim str2 As String = "MEMBER_RATE_STRUCTURE"
            If (Not PersonifyDataCache.Fetch(key) Is Nothing) Then
                Me._DefaultListRateStructure = DirectCast(PersonifyDataCache.Fetch(key), IRateStructure).RateStructure
            End If
            If (Not PersonifyDataCache.Fetch(str2) Is Nothing) Then
                Me._DefaultMemberRateStructure =
                    DirectCast(PersonifyDataCache.Fetch(str2), IRateStructure).RateStructure
            End If
            If ((Me._DefaultListRateStructure = "") OrElse (Me._DefaultMemberRateStructure = "")) Then
                Dim structures As IRateStructures = DirectCast(TIMSS.Global.GetCollection(Me.OrganizationId,
                                                                                  Me.OrganizationUnitId,
                                                                                  NamespaceEnum.ProductInfo,
                                                                                  "RateStructures"), 
                                                            IRateStructures)
                structures.Fill()
                Dim num2 As Integer = (structures.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num2)
                    If structures.Item(i).DefaultStructureFlag Then
                        PersonifyDataCache.Store(key, structures.Item(i), PersonifyDataCache.CacheExpirationInterval)
                        Me._DefaultListRateStructure = structures.Item(i).RateStructure
                    End If
                    If structures.Item(i).MemberPriceFlag Then
                        PersonifyDataCache.Store(str2, structures.Item(i), PersonifyDataCache.CacheExpirationInterval)
                        Me._DefaultMemberRateStructure = structures.Item(i).RateStructure
                    End If
                    i += 1
                Loop
            End If
        End Sub

        Public Sub SetProductWithAllDefaultPrices(ByVal PID As Long, ByRef ListPrices As WebPrices,
                                                   ByRef MemberPrices As WebPrices, ByRef YourPrices As WebPrices,
                                                   Optional ByVal PricingOption As PersonifyEnumerations.PricingOptions _
                                                      = 0)
            Dim productIds As String() = New String() {Conversions.ToString(PID)}
            Dim oProd As ITmarWebProductView =
                    DirectCast(
                        CachedApplicationData.ApplicationDataCache.GetWebEnabledProductsWithPriceFromCache(
                            productIds, Me.GetBestQualifiedRateStructureForLoginUserArray, Me._PortalCurrency.Code,
                            Me._BaseCurrency.Code, Me.DefaultListRateStructure, Me.DefaultMemberRateStructure), 
                        ITmarWebProductView)
            Me.SetProductWithAllDefaultPrices(oProd, ListPrices, MemberPrices, YourPrices, PricingOption, Nothing, True,True, True)
        End Sub

        Public Sub SetProductWithAllDefaultPrices(ByVal oProd As ITmarWebProductView, ByRef ListPrices As WebPrices,
                                                   ByRef MemberPrices As WebPrices, ByRef YourPrices As WebPrices,
                                                         Optional ByVal PricingOption As PersonifyEnumerations.PricingOptions = 0,
                                                         Optional ByVal ComponentProducts As ProductComponentList = Nothing,
                                                         Optional ByVal GetListPrice As Boolean = True,
                                                         Optional ByVal GetMemberPrice As Boolean = True,
                                                         Optional ByVal GetYourPrice As Boolean = True)
            If ComponentProducts Is Nothing Then
                ComponentProducts = New ProductComponentList()
            End If
            
            Dim price As WebPrice
            Dim price2 As WebPrice
            Dim price3 As WebPrice
            Dim flag As Boolean = (oProd.ProductTypeCode.Code = "P")
            If (flag AndAlso (ComponentProducts Is Nothing)) Then
                ComponentProducts = Me.GetProductComponentsWithPrice(oProd)
            End If
            Select Case CInt(PricingOption)
                Case 0
                    If Not GetListPrice Then
                        GoTo Label_00C4
                    End If
                    ListPrices = New WebPrices
                    price = ListPrices.AddNewWebPrice
                    price.ProductId = Conversions.ToString(oProd.ProductId)
                    price.IsDefault = True
                    price.MaxBadges = oProd.YourPriceMaxBadges
                    If flag Then
                        price.Price = ComponentProducts.TotalPackageListPrice
                        Exit Select
                    End If
                    price.Price = oProd.ListPrice
                    Exit Select
                Case 1
                    Dim list As List(Of YourPriceItem)
                    If GetListPrice Then
                        list =
                            CachedApplicationData.ApplicationDataCache.LoadProductPricesFromCache(
                                Conversions.ToString(oProd.ProductId), Me.DefaultListRateStructure)
                        ListPrices = Me.GetWebPriceLists(list, oProd.ProductId, oProd.Subsystem)
                    End If
                    If GetMemberPrice Then
                        list =
                            CachedApplicationData.ApplicationDataCache.LoadProductPricesFromCache(
                                Conversions.ToString(oProd.ProductId), Me.DefaultMemberRateStructure)
                        MemberPrices = Me.GetWebPriceLists(list, oProd.ProductId, oProd.Subsystem)
                    End If
                    If GetYourPrice Then
                        list =
                            CachedApplicationData.ApplicationDataCache.LoadProductPricesFromCache(
                                Conversions.ToString(oProd.ProductId), oProd.YourPriceRateStructure)
                        YourPrices = Me.GetWebPriceLists(list, oProd.ProductId, oProd.Subsystem)
                    End If
                    GoTo Label_026C
                Case Else
                    GoTo Label_026C
            End Select
            price.RateStructure = Me.DefaultListRateStructure
Label_00C4:
            If GetMemberPrice Then
                MemberPrices = New WebPrices
                price2 = MemberPrices.AddNewWebPrice
                price2.ProductId = Conversions.ToString(oProd.ProductId)
                price2.IsDefault = True
                price2.MaxBadges = oProd.YourPriceMaxBadges
                If Not flag Then
                    price2.Price = oProd.MemberPrice
                Else
                    price2.Price = ComponentProducts.TotalPackageMemberPrice
                End If
                price2.RateStructure = Me.DefaultMemberRateStructure
            End If
            If GetYourPrice Then
                YourPrices = New WebPrices
                price3 = YourPrices.AddNewWebPrice
                price3.ProductId = Conversions.ToString(oProd.ProductId)
                price3.IsDefault = True
                price3.MaxBadges = oProd.YourPriceMaxBadges
                If Not flag Then
                    price3.Price = oProd.YourPrice
                Else
                    price3.Price = ComponentProducts.TotalPackageYourPrice
                End If
                price3.RateStructure = oProd.YourPriceRateStructure
            End If
Label_026C:
            If Not String.IsNullOrEmpty(oProd.YourPriceScheduleId) Then
                Dim yourPriceRateStructure As String = oProd.YourPriceRateStructure
                If (yourPriceRateStructure = Me.DefaultListRateStructure) Then
                    If GetListPrice Then
                        price.ScheduledPriceRange = Me.GetScheduledPriceRange(oProd.YourPriceScheduleId, price)
                    End If
                ElseIf ((yourPriceRateStructure = Me.DefaultMemberRateStructure) AndAlso GetMemberPrice) Then
                    price2.ScheduledPriceRange = Me.GetScheduledPriceRange(oProd.YourPriceScheduleId, price2)
                End If
                If GetYourPrice Then
                    price3.ScheduledPriceRange = Me.GetScheduledPriceRange(oProd.YourPriceScheduleId, price3)
                End If
            End If
        End Sub

        Private Function sp_LoadBestQualifiedRateStructureForCustomer(ByVal MCID As String, ByVal SCID As Integer) _
            As List(Of String)
            Dim list2 As New List(Of String)
            Dim list As ArrayList = New OrderDefaultStructure().GetRateStructuresForCustomer(Me.OrganizationId,
                                                                                              Me.OrganizationUnitId,
                                                                                              MCID, SCID, MCID, SCID)
            Dim num2 As Integer = (list.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num2)
                list2.Add(Conversions.ToString(list.Item(i)))
                i += 1
            Loop
            Return list2
        End Function


        ' Properties
        Public ReadOnly Property DefaultListRateStructure As String
            Get
                If String.IsNullOrEmpty(Me._DefaultListRateStructure) Then
                    Me.SetListAndMemberRateStructures()
                End If
                Return Me._DefaultListRateStructure
            End Get
        End Property

        Public ReadOnly Property DefaultMemberRateStructure As String
            Get
                If String.IsNullOrEmpty(Me._DefaultMemberRateStructure) Then
                    Me.SetListAndMemberRateStructures()
                End If
                Return Me._DefaultMemberRateStructure
            End Get
        End Property

        Public Property MCID As String
            Get
                Return Me._MCID
            End Get
            Set(ByVal value As String)
                Me._MCID = value
            End Set
        End Property

        Public Property SCID As Integer
            Get
                Return Me._SCID
            End Get
            Set(ByVal value As Integer)
                Me._SCID = value
            End Set
        End Property


        ' Fields
        Private _BaseCurrency As Personify.ApplicationManager.PersonifyDataObjects.Currency
        Private _DefaultListRateStructure As String
        Private _DefaultMemberRateStructure As String
        Private _LoadBestQualifiedRateStructureForCustomerArray As List(Of String)
        Private _MCID As String
        Private _PortalCurrency As Personify.ApplicationManager.PersonifyDataObjects.Currency
        Private _PortalId As Integer
        Private _SCID As Integer
    End Class
End Namespace

