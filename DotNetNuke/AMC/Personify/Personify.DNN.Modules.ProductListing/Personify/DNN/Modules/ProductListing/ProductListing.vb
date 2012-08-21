Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.UI.Skins
Imports DotNetNuke.UI.Skins.Controls
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports Personify.ApplicationManager
Imports Personify.ApplicationManager.PersonifyDataObjects
Imports Personify.ShoppingCartManager.Business
Imports Personify.WebControls
Imports System
Imports System.Collections
Imports System.Diagnostics
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Web.UI.WebControls
Imports TIMSS.API
Imports TIMSS.API.Core
Imports TIMSS.API.WebInfo
Imports TIMSS.Enumerations

Namespace Personify.DNN.Modules.ProductListing
    Public MustInherit Class ProductListing
        Inherits PersonifyDNNBaseForm
        ' Methods
        Public Sub New()
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.Page_Load)
            AddHandler MyBase.Init, New EventHandler(AddressOf Me.Page_Init)
            Me._strListRateStructure = String.Empty
            Me._strMemberRateStructure = String.Empty
            Me.MeetingFull = False
        End Sub

        Private Sub AddComponentsToCart(ByVal MasterCustomerId As String, ByVal SubCustomerId As Integer, ByVal CartItemId As Integer, ByVal ProductID As Integer, ByVal ProductComponentDetails As ProductComponent, ByVal isWishList As Boolean)
            Try 
                Dim controller As New ShoppingCartController
                Dim shoppingCartComp As New ShoppingCartComponentInfo
                Dim info2 As ShoppingCartComponentInfo = shoppingCartComp
                info2.MasterCustomerId = MasterCustomerId
                info2.SubCustomerId = SubCustomerId
                info2.CartItemId = CartItemId
                info2.LongName = ProductComponentDetails.LongName
                info2.ShortName = ProductComponentDetails.ShortName
                info2.ProductId = ProductID
                info2.ComponentProductId = ProductComponentDetails.ProductId
                info2.AddDate = DateTime.Now
                info2.ModDate = DateTime.Now
                info2.IsWishList = isWishList
                info2.PortalId = Me.PortalId
                info2 = Nothing
                controller.AddShoppingCartComponents(shoppingCartComp)
                If (Not controller Is Nothing) Then
                    controller.Dispose
                End If
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exc As Exception = exception1
                Exceptions.ProcessModuleLoadException(DirectCast(Me, PortalModuleBase), exc)
                ProjectData.ClearProjectError
            End Try
        End Sub

        Private Function AddToCart(ByVal MasterCustomerId As String, ByVal SubCustomerId As Integer, ByVal ProductID As Integer, ByVal SubProductId As Integer, ByVal ProductDetails As ProductDetails, ByVal Quantity As Integer, ByVal RateCode As String, ByVal RateStructure As String, ByVal Price As Decimal, ByVal isWishList As Boolean, ByVal HasValidScheduledPrice As Boolean) As Integer
            Dim num As Integer
            Try 
                Dim controller As New ShoppingCartController
                Dim shoppingCart As New ShoppingCartInfo
                Dim info3 As ShoppingCartInfo = shoppingCart
                info3.Subsystem = ProductDetails.ProductInfo.Item(0).Subsystem
                info3.ProductType = ProductDetails.ProductInfo.Item(0).ProductTypeCodeString
                info3.RateStructure = RateStructure
                info3.RateCode = RateCode
                info3.MasterCustomerId = MasterCustomerId
                info3.SubCustomerId = SubCustomerId
                info3.LongName = ProductDetails.ProductInfo.Item(0).LongName
                info3.ShortName = ProductDetails.ProductInfo.Item(0).ShortName
                info3.Price = Price
                info3.HasValidScheduledPrice = HasValidScheduledPrice
                info3.Quantity = Quantity
                info3.ProductId = ProductID
                info3.SubProductId = SubProductId
                info3.AddDate = DateTime.Now
                info3.ModDate = DateTime.Now
                info3.MaxBadges = 0
                info3.MaximumTickets = ProductDetails.ProductInfo.Item(0).MaximumTickets
                info3.IsWishList = isWishList
                info3.ShipMasterCustomerId = Nothing
                info3.ShipSubCustomerId = 0
                If (ProductDetails.Components.Count > 0) Then
                    info3.ComponentExists = True
                Else
                    info3.ComponentExists = False
                End If
                info3.MaxBadges = Me.GetMaxBadges(ProductDetails, info3.RelatedCartItemId, info3.RateStructure, info3.RateCode)
                info3 = Nothing

                'Dim cartItemId As Integer = controller.AddToCart(shoppingCart, Nothing)
                Dim cartItemId As Integer = controller.AddToCart(shoppingCart)

                If ((shoppingCart.MaxBadges > 0) AndAlso Me.IsPersonifyWebUserLoggedIn) Then
                    Dim defaultBadgeInformation As DefaultBadgeInfo = Me.GetDefaultBadgeInformation
                    If (Not defaultBadgeInformation Is Nothing) Then
                        controller.AddUpdateBadge(cartItemId, 1, "SELF", defaultBadgeInformation.FirstName, defaultBadgeInformation.LabelName, defaultBadgeInformation.CompanyName, defaultBadgeInformation.City, defaultBadgeInformation.State, defaultBadgeInformation.PostalCode)
                    End If
                End If
                If (Not controller Is Nothing) Then
                    controller.Dispose
                End If
                num = cartItemId
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exc As Exception = exception1
                Exceptions.ProcessModuleLoadException(DirectCast(Me, PortalModuleBase), exc)
                ProjectData.ClearProjectError
            End Try
            Return num
        End Function

        Private Sub AddToCartControl_ButtonClick(ByVal tempAddToCartControl As AddToCartControl, ByVal e As EventArgs)
            Me.AddToCommonControl_ButtonClick(Me.MasterCustomerId, Me.SubCustomerId, tempAddToCartControl, e, False, False)
        End Sub

        Private Sub AddToCommonControl_ButtonClick(ByVal MasterCustomerId As String, ByVal SubCustomerId As Integer, ByVal tempAddToCartControl As AddToCartControl, ByVal e As EventArgs, ByVal isWishList As Boolean, ByVal Optional IsGUID As Boolean = False)
            Try 
                Dim isPersonifyWebUserLoggedIn As Boolean
                If IsGUID Then
                    isPersonifyWebUserLoggedIn = False
                Else
                    isPersonifyWebUserLoggedIn = Me.IsPersonifyWebUserLoggedIn
                End If
                Dim control As PricingControl = DirectCast(Me.FindControl(("PricingControl" & Me.ModuleId.ToString & "_" & tempAddToCartControl.ProductId.ToString)), PricingControl)
                Dim productDetails As ProductDetails = Me.get_clsProductHelper.GetAllDetailsForAProduct(Convert.ToInt32(tempAddToCartControl.ProductId), False, False, True, True, False, Nothing, isPersonifyWebUserLoggedIn, MasterCustomerId, SubCustomerId, True, True, False, False, False, Nothing, "", "")
                Dim price As Decimal = Convert.ToDecimal(Me.Request.Item(String.Concat(New String() { "PricingControl", Me.ModuleId.ToString, "_", tempAddToCartControl.ProductId.ToString, "_price" })))
                Dim rateCode As String = Convert.ToString(Me.Request.Item(String.Concat(New String() { "PricingControl", Me.ModuleId.ToString, "_", tempAddToCartControl.ProductId.ToString, "_rc" })))
                Dim rateStructure As String = Convert.ToString(Me.Request.Item(String.Concat(New String() { "PricingControl", Me.ModuleId.ToString, "_", tempAddToCartControl.ProductId.ToString, "_rs" })))
                Dim hasValidScheduledPrice As Boolean = Convert.ToBoolean(Me.Request.Item(String.Concat(New String() { "PricingControl", Me.ModuleId.ToString, "_", tempAddToCartControl.ProductId.ToString, "_hasValidScheduledPrice" })))
                Dim quantity As Integer = Convert.ToInt32(Me.Request.Item(tempAddToCartControl.txtQuantity.UniqueID))
                Dim cartItemId As Integer = Me.AddToCart(MasterCustomerId, SubCustomerId, CInt(tempAddToCartControl.ProductId), 0, productDetails, quantity, rateCode, rateStructure, price, isWishList, hasValidScheduledPrice)
                If (productDetails.Components.Count > 0) Then
                    Dim num7 As Integer = (productDetails.Components.Count - 1)
                    Dim i As Integer = 0
                    Do While (i <= num7)
                        If (Not productDetails.Components.Item(i) Is Nothing) Then
                            Me.AddComponentsToCart(MasterCustomerId, SubCustomerId, cartItemId, CInt(tempAddToCartControl.ProductId), productDetails.Components.Item(i), isWishList)
                        End If
                        i += 1
                    Loop
                End If
                If Not isWishList Then
                    Skin.AddModuleMessage(Me, Localization.GetString("MessageControl.Text", Me.LocalResourceFile).Replace("$quantity$", quantity.ToString), ModuleMessage.ModuleMessageType.GreenSuccess)
                    Dim script As String = "<script language='javascript'>setTimeout('window.scrollTo(0, 0)',1);</script>"
                    If Not Me.Page.ClientScript.IsStartupScriptRegistered("goScroll") Then
                        Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "goScroll", script)
                    End If
                Else
                    Skin.AddModuleMessage(Me, Localization.GetString("WishListMessageControl.Text", Me.LocalResourceFile).Replace("$quantity$", quantity.ToString), ModuleMessage.ModuleMessageType.GreenSuccess)
                    Dim str4 As String = "<script language='javascript'>setTimeout('window.scrollTo(0, 0)',1);</script>"
                    If Not Me.Page.ClientScript.IsStartupScriptRegistered("goScroll") Then
                        Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "goScroll", str4)
                    End If
                End If
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exc As Exception = exception1
                Exceptions.ProcessModuleLoadException(DirectCast(Me, PortalModuleBase), exc)
                ProjectData.ClearProjectError
            End Try
        End Sub

        Private Sub AddToWishListControl_ButtonClick(ByVal tempAddToCartControl As AddToCartControl, ByVal e As EventArgs)
            Me.AddToCommonControl_ButtonClick(Me.MasterCustomerId, Me.SubCustomerId, tempAddToCartControl, e, True, False)
        End Sub

        Private Sub BuyforGroupControl_ButtonClick(ByVal tempAddToCartControl As AddToCartControl, ByVal e As EventArgs)
            Dim cartGUID As String = Convert.ToString(AffiliateManagementSessionHelper.GetGroupPurchaseCartGUID(Me.PortalId))
            If (cartGUID Is Nothing) Then
                cartGUID = Guid.NewGuid.ToString("N").ToUpper
                If (cartGUID.Length > 20) Then
                    cartGUID = cartGUID.Substring(1, 20)
                End If
                AffiliateManagementSessionHelper.SetGroupPurchaseInfo(Me.PortalId, cartGUID)
            End If
            Me.AddToCommonControl_ButtonClick(cartGUID, 0, tempAddToCartControl, e, False, True)
            Dim currentSegmentGroupPurchaseActionURL As Integer = AffiliateManagementSessionHelper.GetCurrentSegmentGroupPurchaseActionURL(Me.PortalId)
            If (currentSegmentGroupPurchaseActionURL = 0) Then
                Me.Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(Conversions.ToInteger(Me.Settings.Item("BuyForGroupUrl"))), True)
            Else
                Me.Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(currentSegmentGroupPurchaseActionURL), True)
            End If
        End Sub

        Private Function CheckIfPostBackByButton(ByVal strControlName As String, ByVal strWitch As String) As String
            Dim enumerator As IEnumerator
            Try 
                enumerator = Me.Request.Form.Keys.GetEnumerator
                Do While enumerator.MoveNext
                    Dim str3 As String = Conversions.ToString(enumerator.Current)
                    If ((str3.IndexOf(strControlName) > 0) AndAlso (str3.IndexOf(strWitch) > 0)) Then
                        Return str3
                    End If
                Loop
            Finally
                If TypeOf enumerator Is IDisposable Then
                    TryCast(enumerator,IDisposable).Dispose
                End If
            End Try
            Return ""
        End Function

        Private Function GetAddToCartImageURL() As String
            Return Me.ResolveUrl(("~/" & Me.SiteImagesFolder & "/btn_addtocart.gif"))
        End Function

        Private Function GetBuyForGroupImageURL() As String
            Return Me.ResolveUrl(("~/" & Me.SiteImagesFolder & "/btn_buyforgroup.gif"))
        End Function

        Private Function GetDefaultBadgeInformation() As DefaultBadgeInfo
            Dim obj2 As New SearchObject(Me.OrganizationId, Me.OrganizationUnitId)
            obj2.Target = Me.PersonifyGetCollection(NamespaceEnum.CustomerInfo, "CustomerPrimaryInfoViewList")
            obj2.EnforceLimits = False
            Dim item As New SearchProperty("MasterCustomerId")
            item.UseInQuery = True
            item.ShowInResults = False
            item.Value = Me.MasterCustomerId
            obj2.Parameters.Add(item)
            item = New SearchProperty("SubCustomerId")
            item.UseInQuery = True
            item.ShowInResults = False
            item.Value = Conversions.ToString(Me.SubCustomerId)
            obj2.Parameters.Add(item)
            item = New SearchProperty("FirstName")
            item.UseInQuery = False
            item.ShowInResults = True
            obj2.Parameters.Add(item)
            item = New SearchProperty("LastName")
            item.UseInQuery = False
            item.ShowInResults = True
            obj2.Parameters.Add(item)
            item = New SearchProperty("LabelName")
            item.UseInQuery = False
            item.ShowInResults = True
            obj2.Parameters.Add(item)
            item = New SearchProperty("CompanyName")
            item.UseInQuery = False
            item.ShowInResults = True
            obj2.Parameters.Add(item)
            item = New SearchProperty("City")
            item.UseInQuery = False
            item.ShowInResults = True
            obj2.Parameters.Add(item)
            item = New SearchProperty("State")
            item.UseInQuery = False
            item.ShowInResults = True
            obj2.Parameters.Add(item)
            item = New SearchProperty("PostalCode")
            item.UseInQuery = False
            item.ShowInResults = True
            obj2.Parameters.Add(item)
            obj2.Search
            If ((Not obj2.Results.Table Is Nothing) AndAlso (obj2.Results.Table.Rows.Count > 0)) Then
                Dim info2 As New DefaultBadgeInfo
                Dim info3 As DefaultBadgeInfo = info2
                info3.FirstName = Conversions.ToString(Interaction.IIf((obj2.Results.Table.Rows.Item(0).Item("FirstName") Is DBNull.Value), "", RuntimeHelpers.GetObjectValue(obj2.Results.Table.Rows.Item(0).Item("FirstName"))))
                info3.LastName = Conversions.ToString(Interaction.IIf((obj2.Results.Table.Rows.Item(0).Item("LastName") Is DBNull.Value), "", RuntimeHelpers.GetObjectValue(obj2.Results.Table.Rows.Item(0).Item("LastName"))))
                info3.LabelName = Conversions.ToString(Interaction.IIf((obj2.Results.Table.Rows.Item(0).Item("LabelName") Is DBNull.Value), "", RuntimeHelpers.GetObjectValue(obj2.Results.Table.Rows.Item(0).Item("LabelName"))))
                info3.CompanyName = Conversions.ToString(Interaction.IIf((obj2.Results.Table.Rows.Item(0).Item("CompanyName") Is DBNull.Value), "", RuntimeHelpers.GetObjectValue(obj2.Results.Table.Rows.Item(0).Item("CompanyName"))))
                info3.City = Conversions.ToString(Interaction.IIf((obj2.Results.Table.Rows.Item(0).Item("City") Is DBNull.Value), "", RuntimeHelpers.GetObjectValue(obj2.Results.Table.Rows.Item(0).Item("City"))))
                info3.State = Conversions.ToString(Interaction.IIf((obj2.Results.Table.Rows.Item(0).Item("State") Is DBNull.Value), "", RuntimeHelpers.GetObjectValue(obj2.Results.Table.Rows.Item(0).Item("State"))))
                info3.PostalCode = Conversions.ToString(Interaction.IIf((obj2.Results.Table.Rows.Item(0).Item("PostalCode") Is DBNull.Value), "", RuntimeHelpers.GetObjectValue(obj2.Results.Table.Rows.Item(0).Item("PostalCode"))))
                info3 = Nothing
                Return info2
            End If
            Return Nothing
        End Function

        Private Function GetMaxBadges(ByVal pProductDetails As ProductDetails, ByVal pRelatedCartItemId As Integer, ByVal pRateStructure As String, ByVal pRateCode As String) As Integer
            Dim maxBadges As Integer = 0
            Dim flag As Boolean = False
            If (pRelatedCartItemId <> 0) Then
                Return 0
            End If
            If ((Not pProductDetails.CustomerPrice Is Nothing) AndAlso (pProductDetails.CustomerPrice.Count > 0)) Then
                Dim num6 As Integer = (pProductDetails.CustomerPrice.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num6)
                    If ((pProductDetails.CustomerPrice.Item(i).RateStructure = pRateStructure) And (pProductDetails.CustomerPrice.Item(i).RateCode = pRateCode)) Then
                        maxBadges = pProductDetails.CustomerPrice.Item(i).MaxBadges
                        flag = True
                    End If
                    i += 1
                Loop
            End If
            If ((Not flag AndAlso (Not pProductDetails.ListPrices Is Nothing)) AndAlso (pProductDetails.ListPrices.Count > 0)) Then
                Dim num7 As Integer = (pProductDetails.ListPrices.Count - 1)
                Dim j As Integer = 0
                Do While (j <= num7)
                    If ((pProductDetails.ListPrices.Item(j).RateStructure = pRateStructure) And (pProductDetails.ListPrices.Item(j).RateCode = pRateCode)) Then
                        maxBadges = pProductDetails.ListPrices.Item(j).MaxBadges
                        flag = True
                    End If
                    j += 1
                Loop
            End If
            If ((Not flag AndAlso (Not pProductDetails.MemberPrices Is Nothing)) AndAlso (pProductDetails.MemberPrices.Count > 0)) Then
                Dim num8 As Integer = (pProductDetails.MemberPrices.Count - 1)
                Dim k As Integer = 0
                Do While (k <= num8)
                    If ((pProductDetails.MemberPrices.Item(k).RateStructure = pRateStructure) And (pProductDetails.MemberPrices.Item(k).RateCode = pRateCode)) Then
                        maxBadges = pProductDetails.MemberPrices.Item(k).MaxBadges
                        flag = True
                    End If
                    k += 1
                Loop
            End If
            Return maxBadges
        End Function

        Private Function GetWishListImageURL() As String
            Return Me.ResolveUrl(("~/" & Me.SiteImagesFolder & "/btn_wishlist.gif"))
        End Function

        <DebuggerStepThrough> _
        Private Sub InitializeComponent()
        End Sub

        Private Function IsUserPErsonifyMember() As Boolean
            Dim flag As Boolean = False
            Dim userRole As String = Me.GetUserRole(Me.UserInfo)
            If ((userRole = "personifyuser") Or (userRole = "personifyadmin")) Then
                Dim rolesByUser As String() = New RoleController().GetRolesByUser(Me.UserId, Me.PortalId)
                If (If(((Not rolesByUser Is Nothing) AndAlso (rolesByUser.Length > 0)), 1, 0) = 0) Then
                    Return flag
                End If
                Dim str2 As String
                For Each str2 In rolesByUser
                    If (str2 = "PersonifyMember") Then
                        flag = True
                    End If
                Next
            End If
            Return flag
        End Function

        Protected Sub ItemsPerPageDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
            Me.PLDataPager.PageSize = Convert.ToInt32(Me.ItemsPerPageDropDownList.SelectedValue)
            Me.PLDataPagerBottom.PageSize = Convert.ToInt32(Me.ItemsPerPageDropDownList.SelectedValue)
            Me.LoadDataToXML
        End Sub

        Private Sub LoadDataToXML()
            Try 
                Dim length As Integer = Convert.ToInt32(RuntimeHelpers.GetObjectValue(Me.Settings.Item("Truncate")))
                If ((Me.PLDataPager.DataSourcePaged.Count > 0) AndAlso ((Not Me.PLDataPager.DataSourcePaged Is Nothing) AndAlso (Me.PLDataPager.DataSourcePaged.Count > 0))) Then
                    Dim enumerator As IEnumerator = Me.PLDataPager.DataSourcePaged.GetEnumerator
                    Dim productArray As aProduct() = New aProduct(((Me.PLDataPager.DataSourcePaged.Count - 1) + 1)  - 1) {}
                    Dim o As Boolean = False
                    Dim i As Integer = 0
                    Do While enumerator.MoveNext
                        Dim current As ITmarWebProductView = DirectCast(enumerator.Current, ITmarWebProductView)
                        productArray(i) = New aProduct
                        productArray(i).ProductId = CInt(current.ProductId)
                        productArray(i).Subsystem = current.Subsystem
                        productArray(i).AddToCartFlag = current.AddToCartFlag
                        Me.get_clsProductHelper.SetProductWithAllDefaultPrices(current, productArray(i).ListPrices, productArray(i).MemberPrices, productArray(i).YourPrices, PersonifyEnumerations.PricingOptions.DefaultPricing, Nothing, True, True, True)
                        If (Not Me.IsPersonifyWebUserLoggedIn AndAlso (Me.UserInfo.UserID <> -1)) Then
                            productArray(i).Code = current.ProductCode
                            productArray(i).ParentCode = current.ParentProduct
                            o = True
                        End If
                        If (current.Subsystem = "MTG") Then
                            productArray(i).MaxBadges = current.YourPriceMaxBadges
                            productArray(i).StartDate = current.MeetingStartDate.ToString("MMMM d, yyyy hh:mmtt")
                            productArray(i).EndDate = current.MeetingEndDate.ToString("MMMM d, yyyy hh:mmtt")
                            Dim facilityInfo As FacilityInformation = Nothing
                            If ((Not current.FacilityMasterCustomerId Is Nothing) AndAlso (current.FacilityMasterCustomerId.Length > 0)) Then
                                facilityInfo = Me.get_clsProductHelper.GetFacilityInfo(current.FacilityMasterCustomerId, current.FacilitySubCustomerId)
                            End If
                            If (Not facilityInfo Is Nothing) Then
                                productArray(i).FacilityLabelName = facilityInfo.FacilityName
                                productArray(i).FacilityCity = facilityInfo.City
                                productArray(i).FacilityState = facilityInfo.State
                            End If
                            If (current.MeetingCapacity > current.MeetingRegistrations) Then
                                productArray(i).Status = Localization.GetString("AvailableMessage", Me.LocalResourceFile)
                            End If
                            If ((current.MeetingCapacity = current.MeetingRegistrations) And (current.MeetingWaitListCapacity > current.MeetingWaitListRegistrations)) Then
                                productArray(i).Status = Localization.GetString("WaitlistedMessage", Me.LocalResourceFile)
                            End If
                            If (((current.MeetingCapacity = current.MeetingRegistrations) And (current.MeetingWaitListCapacity = current.MeetingWaitListRegistrations)) And ((current.ProductTypeCodeString = "M") Or (current.ProductTypeCodeString = "S"))) Then
                                productArray(i).Status = Localization.GetString("FullMessage", Me.LocalResourceFile)
                            End If
                        End If
                        If (current.Subsystem = "INV") Then
                            If Not current.InventoryFlag Then
                                productArray(i).Status = Localization.GetString("InStockMessage", Me.LocalResourceFile)
                            ElseIf (current.TotalAvailableInventory > 0) Then
                                productArray(i).Status = Localization.GetString("InStockMessage", Me.LocalResourceFile)
                            Else
                                productArray(i).Status = Localization.GetString("OutOfStockMessage", Me.LocalResourceFile)
                                If (current.NextInvReceiptDate.ToString("MMMM d, yyyy") = "01/01/0001") Then
                                    productArray(i).NextInvReceiptDate = current.NextInvReceiptDate.ToString("MMMM d, yyyy")
                                Else
                                    productArray(i).NextInvReceiptDate = Nothing
                                End If
                            End If
                        End If
                        Dim webShortDescription As String = current.WebShortDescription
                        If (webShortDescription Is Nothing) Then
                            webShortDescription = ""
                        End If
                        If ((length > 0) AndAlso (webShortDescription.Length > 0)) Then
                            webShortDescription = HtmlUtils.Shorten(Me.StripHTML(webShortDescription), length, "...")
                        End If
                        productArray(i).WebShortDescription = webShortDescription
                        Dim webLongDescription As String = current.WebLongDescription
                        If (webLongDescription Is Nothing) Then
                            webLongDescription = ""
                        End If
                        If ((length > 0) AndAlso (webLongDescription.Length > 0)) Then
                            webLongDescription = HtmlUtils.Shorten(Me.StripHTML(webLongDescription), length, "...")
                        End If
                        If (length < 0) Then
                            webLongDescription = ""
                        End If
                        productArray(i).WebLongDescription = webShortDescription
                        If (Convert.ToInt32(RuntimeHelpers.GetObjectValue(Me.Settings.Item("DisplayImage"))) = 1) Then
                            productArray(i).SmallImageFileName = current.SmallImageFileName
                        End If
                        productArray(i).LongName = current.LongName
                        productArray(i).ShortName = current.ShortName
                        If (((Not Me.Settings.Item("DetailUrl") Is Nothing) AndAlso (Convert.ToString(RuntimeHelpers.GetObjectValue(Me.Settings.Item("DetailUrl"))).Length > 0)) AndAlso (Convert.ToInt32(RuntimeHelpers.GetObjectValue(Me.Settings.Item("DetailUrl"))) > 0)) Then
                            productArray(i).URLToGo = (DotNetNuke.Common.Globals.NavigateURL(Conversions.ToInteger(Me.Settings.Item("DetailUrl"))) & "?ProductId=" & current.ProductId.ToString)
                        End If
                        i += 1
                    Loop
                    Dim horizontal As RepeatDirection = RepeatDirection.Horizontal
                    Dim num3 As Integer = Convert.ToInt32(RuntimeHelpers.GetObjectValue(Me.Settings.Item("Columns")))
                    If (num3 <= 0) Then
                        num3 = 2
                    End If
                    Dim flag2 As Boolean = False
                    If (Convert.ToInt32(RuntimeHelpers.GetObjectValue(Me.Settings.Item("MainProduct"))) = 1) Then
                        flag2 = True
                    End If
                    Dim product As New aProduct
                    Dim num5 As Integer = 0
                    If flag2 Then
                        num5 = 1
                        product = productArray(0)
                    End If
                    Dim num4 As Integer = Convert.ToInt32(CDbl((CDbl((productArray.Length - num5)) / CDbl(num3))))
                    If (((CDbl((productArray.Length - num5)) / CDbl(num3)) - num4) > 0) Then
                        num4 += 1
                    End If
                    Dim rowArray As aRow() = New aRow(((num4 - 1) + 1)  - 1) {}
                    Dim num11 As Integer = (rowArray.Length - 1)
                    Dim j As Integer = 0
                    Do While (j <= num11)
                        rowArray(j) = New aRow
                        Dim productArray2 As aProduct() = New aProduct(((num3 - 1) + 1)  - 1) {}
                        Dim num12 As Integer = (num3 - 1)
                        Dim k As Integer = 0
                        Do While (k <= num12)
                            Dim num7 As Integer
                            productArray2(k) = New aProduct
                            If (horizontal = RepeatDirection.Horizontal) Then
                                num7 = (((j * num3) + k) + num5)
                            Else
                                num7 = (((k * num3) + j) + num5)
                            End If
                            If (num7 < productArray.Length) Then
                                productArray2(k) = productArray(num7)
                            End If
                            k += 1
                        Loop
                        rowArray(j).row = productArray2
                        j += 1
                    Loop
                    Dim path As String = ""
                    Try 
                        path = (Me.ModulePath & "Templates\" & Me.Settings.Item("Layout").ToString)
                    Catch exception1 As Exception
                        ProjectData.SetProjectError(exception1)
                        Dim exception As Exception = exception1
                        Skin.AddModuleMessage(Me, Localization.GetString("WrongTemplate.Text", Me.LocalResourceFile), ModuleMessage.ModuleMessageType.YellowWarning)
                        ProjectData.ClearProjectError
                    End Try
                    Me.PLXslTemplate.XSLfile = Me.Server.MapPath(path)
                    If (Not product Is Nothing) Then
                        Me.PLXslTemplate.AddObject("MainProduct", product)
                    End If
                    If ((Not rowArray Is Nothing) AndAlso (rowArray.Length > 0)) Then
                        Me.PLXslTemplate.AddObject("", rowArray)
                    End If
                    Me.PLXslTemplate.AddObject("IsAdmin", o)
                    Me.PLXslTemplate.AddObject("ModuleId", Me.ModuleId)
                    Me.PLXslTemplate.AddObject("RepeatColumns", num3)
                    Dim str As String = Convert.ToString(Me.GetDNNSiteSettings.ProductImageURL)
                    Me.PLXslTemplate.AddObject("ProductImageURL", str)
                    Me.PLXslTemplate.Display
                    Dim flag As Boolean = Me.IsUserPErsonifyMember
                    If ((Not productArray Is Nothing) AndAlso (productArray.Length > 0)) Then
                        Dim num13 As Integer = (productArray.Length - 1)
                        Dim m As Integer = 0
                        Do While (m <= num13)
                            If ((Not productArray(m) Is Nothing) AndAlso (productArray(m).ProductId > 0)) Then
                                Dim child As New PricingControl
                                child.ID = ("PricingControl" & Me.ModuleId.ToString & "_" & productArray(m).ProductId.ToString)
                                child.PortalID = Me.PortalId
                                child.IsMember = flag
                                child.PortalCurrency = Me.PortalCurrency
                                child.AllPrices = productArray(m).AllPrices
                                child.ListPrices = productArray(m).ListPrices
                                child.MemberPrices = productArray(m).MemberPrices
                                child.YourPrices = productArray(m).YourPrices
                                child.MemberPriceLabel = Localization.GetString("MemberPriceLabel.Text", Me.LocalResourceFile)
                                child.ListPriceLabel = Localization.GetString("ListPriceLabel.Text", Me.LocalResourceFile)
                                child.YourPriceLabel = Localization.GetString("YourPriceLabel.Text", Me.LocalResourceFile)
                                child.HideSchedulePriceLabelMessage = Localization.GetString("HideSchedulePriceLabelMessage.Text", Me.LocalResourceFile)
                                If (Not Me.Settings.Item(("DisplayRateCodeSettingsKey_" & productArray(m).Subsystem.ToUpper)) Is Nothing) Then
                                    child.ShowRateCode = True
                                End If
                                If Me.IsPersonifyWebUserLoggedIn Then
                                    child.IsMember = True
                                Else
                                    child.IsMember = False
                                End If
                                Dim holder As New PlaceHolder
                                holder = DirectCast(Me.FindControl(("PricingPlaceHolder" & Me.ModuleId.ToString & "_" & productArray(m).ProductId.ToString)), PlaceHolder)
                                If (Not holder Is Nothing) Then
                                    holder.Controls.Add(child)
                                End If
                                Dim control As New AddToCartControl
                                control.ID = ("AddToCartControl" & Me.ModuleId.ToString & "_" & productArray(m).ProductId.ToString)
                                control.Visible = False
                                Me.MeetingFull = False
                                If (productArray(m).Status = Localization.GetString("FullMessage", Me.LocalResourceFile)) Then
                                    Me.MeetingFull = True
                                End If
                                If (Not Me.MeetingFull AndAlso (((Convert.ToInt32(RuntimeHelpers.GetObjectValue(Me.Settings.Item("AddToCart"))) = 1) Or (Convert.ToInt32(RuntimeHelpers.GetObjectValue(Me.Settings.Item("AddToWishList"))) = 1)) Or (Convert.ToInt32(RuntimeHelpers.GetObjectValue(Me.Settings.Item("BuyForGroup"))) = 1))) Then
                                    control.Visible = True
                                    control.VisibleAddToCart = ((Convert.ToInt32(RuntimeHelpers.GetObjectValue(Me.Settings.Item("AddToCart"))) = 1) AndAlso productArray(m).AddToCartFlag)
                                    If (Convert.ToInt32(RuntimeHelpers.GetObjectValue(Me.Settings.Item("AddToWishList"))) = 1) Then
                                        control.VisibleWishList = True
                                    Else
                                        control.VisibleWishList = False
                                    End If
                                    control.VisibleBuyForGroup = False
                                    If Not Me.MeetingFull Then
                                        If ((Convert.ToInt32(RuntimeHelpers.GetObjectValue(Me.Settings.Item("BuyForGroup"))) = 1) AndAlso Me.get_clsAffiliateManagement.IsGroupPurchaseEnabledForSegmentController(Me.PortalId, Me.MasterCustomerId, Me.SubCustomerId)) Then
                                            control.VisibleBuyForGroup = True
                                        End If
                                        If (Not Me.Settings.Item("DefaultQuantity") Is Nothing) Then
                                            control.Quantity = Conversions.ToInteger(Me.Settings.Item("DefaultQuantity"))
                                        Else
                                            control.Quantity = 1
                                        End If
                                        Select Case productArray(m).Subsystem
                                            Case "INV", "MISC", "SUB"
                                                control.VisibleQuantity = True
                                                Exit Select
                                            Case "MTG"
                                                If (productArray(m).MaxBadges >= 1) Then
                                                    control.VisibleQuantity = True
                                                Else
                                                    control.VisibleQuantity = False
                                                End If
                                                Exit Select
                                            Case Else
                                                control.VisibleQuantity = False
                                                Exit Select
                                        End Select
                                        control.ProductId = productArray(m).ProductId
                                        control.Text = Localization.GetString("AddToCart.Text", Me.LocalResourceFile)
                                        control.WishListText = Localization.GetString("AddToWishList.Text", Me.LocalResourceFile)
                                        control.BuyForGroupText = Localization.GetString("BuyForGroup.Text", Me.LocalResourceFile)
                                        control.ButtonCss = "btna"
                                        control.ButtonMode = AddToCartControl.EnumButtonMode.Button
                                        control.ImageURL = Me.GetAddToCartImageURL
                                        control.WishListImageURL = Me.GetWishListImageURL
                                        control.BuyForGroupImageURL = Me.GetBuyForGroupImageURL
                                        If (productArray(m).Subsystem <> "ECD") Then
                                            holder = DirectCast(Me.FindControl(("AddToCartPlaceHolder" & Me.ModuleId.ToString & "_" & productArray(m).ProductId.ToString)), PlaceHolder)
                                            If (Not holder Is Nothing) Then
                                                holder.Controls.Add(control)
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                            m += 1
                        Loop
                    End If
                End If
            Catch exception3 As Exception
                ProjectData.SetProjectError(exception3)
                Dim exc As Exception = exception3
                Exceptions.ProcessModuleLoadException(DirectCast(Me, PortalModuleBase), exc)
                ProjectData.ClearProjectError
            End Try
        End Sub

        Private Sub LoadDropDownListSetting(ByVal aDropDownList As DropDownList, ByVal aSettingString As String, ByVal aSettingList As String)
            If (aDropDownList.Items.Count <= 0) Then
                Dim strArray As String() = Localization.GetString(aSettingList, (Me.LocalResourceFile & "Edit")).Split(New Char() { "|"c })
                Dim str As String = Convert.ToString(RuntimeHelpers.GetObjectValue(Me.Settings.Item(aSettingString)))
                Dim num2 As Integer = (Convert.ToInt32(CDbl((CDbl(strArray.Length) / 2))) - 1)
                Dim i As Integer = 0
                Do While (i <= num2)
                    aDropDownList.Items.Add(New ListItem(strArray(((2 * i) + 1)), strArray((2 * i))))
                    If (aDropDownList.Items.Item((aDropDownList.Items.Count - 1)).Value = str) Then
                        aDropDownList.SelectedValue = str
                    End If
                    i += 1
                Loop
            End If
        End Sub

        Private Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
            Me.InitializeComponent
        End Sub

        Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
            Try 
                If Me.IsPostBack Then
                    Me.GetRefreshState
                End If
                Me.get_clsProductHelper.GetListAndMemberRateStructures(Me._strListRateStructure, Me._strMemberRateStructure)
                If (Not Me.Settings.Item("Layout") Is Nothing) Then
                    Dim no As PersonifyEnumerations.MembersOnlyFilter
                    Me.ItemsPerPageDropDownList.AutoPostBack = True
                    If (Me.ItemsPerPageDropDownList.Items.Count = 0) Then
                        Me.LoadDropDownListSetting(Me.ItemsPerPageDropDownList, "ItemsPerPage", "ItemsPerPage.List")
                    End If
                    AddHandler Me.ItemsPerPageDropDownList.SelectedIndexChanged, New EventHandler(AddressOf Me.ItemsPerPageDropDownList_SelectedIndexChanged)
                    Me.SortByDropDownList.AutoPostBack = True
                    If (Me.SortByDropDownList.Items.Count = 0) Then
                        Me.LoadDropDownListSetting(Me.SortByDropDownList, "Sorting", "Sorting.List")
                    End If
                    AddHandler Me.SortByDropDownList.SelectedIndexChanged, New EventHandler(AddressOf Me.SortByDropDownList_SelectedIndexChanged)
                    If Not Me.IsPostBack Then
                        Dim str6 As String = Me.Request.Item("CLASS")
                        If ((Not str6 Is Nothing) AndAlso (str6.Length > 0)) Then
                            Me.HiddenFieldClass.Value = str6
                        End If
                        Dim str5 As String = Me.Request.Item("CATEGORY")
                        If ((Not str5 Is Nothing) AndAlso (str5.Length > 0)) Then
                            Me.HiddenFieldCategory.Value = str5
                        End If
                        Dim str7 As String = Me.Request.Item("SUBCATEGORY")
                        If ((Not str7 Is Nothing) AndAlso (str7.Length > 0)) Then
                            Me.HiddenFieldSubcategory.Value = str7
                        End If
                        Dim str8 As String = Me.Request.Item("SORTBY")
                        If ((Not str8 Is Nothing) AndAlso (str8.Length > 0)) Then
                            Me.SortByDropDownList.SelectedValue = str8
                        End If
                        Dim str4 As String = Me.Request.Item("PAGESIZE")
                        If ((Not str4 Is Nothing) AndAlso (str4.Length > 0)) Then
                            Me.ItemsPerPageDropDownList.SelectedValue = str4
                        End If
                    End If
                    Dim flag2 As Boolean = True
                    If (Not Me.Request.Params.Item("__EVENTTARGET") Is Nothing) Then
                        If (Me.Request.Params.Item("__EVENTTARGET").IndexOf("PLDataPager") >= 0) Then
                            flag2 = False
                        End If
                        If (Me.Request.Params.Item("__EVENTTARGET").IndexOf("ItemsPerPageDropDownList") >= 0) Then
                            flag2 = False
                            Me.RedirectOnChange
                        End If
                        If (Me.Request.Params.Item("__EVENTTARGET").IndexOf("SortByDropDownList") >= 0) Then
                            flag2 = False
                            Me.RedirectOnChange
                        End If
                    End If
                    Dim selectedValue As String = Nothing
                    If (Me.SortByDropDownList.SelectedIndex >= 0) Then
                        selectedValue = Me.SortByDropDownList.SelectedValue
                    End If
                    Dim list As ITmarWebProductViewList = Nothing
                    Dim num As Integer = Convert.ToInt32(RuntimeHelpers.GetObjectValue(Me.Settings.Item("Attributes")))
                    Dim featured As Boolean = False
                    Dim promotional As Boolean = False
                    If ((num = 1) Or (num = 3)) Then
                        featured = True
                    End If
                    If ((num = 2) Or (num = 3)) Then
                        promotional = True
                    End If
                    Dim str3 As String = Convert.ToString(RuntimeHelpers.GetObjectValue(Me.Settings.Item("ProductIDs")))
                    Dim productIds As String() = Nothing
                    If ((Not str3 Is Nothing) AndAlso (str3.Length > 0)) Then
                        productIds = str3.Split(New Char() { ","c })
                        If ((Not Me.Settings.Item("Randomize") Is Nothing) AndAlso Convert.ToBoolean(RuntimeHelpers.GetObjectValue(Me.Settings.Item("Randomize")))) Then
                            If Not Me.IsPostBack Then
                                VBMath.Randomize
                                Dim index As Integer = Convert.ToInt32(CSng(((productIds.Length - 1) * VBMath.Rnd)))
                                productIds = New String() { productIds(index) }
                                Me.Session.Item("ProductIDs") = productIds
                            Else
                                productIds = DirectCast(Me.Session.Item("ProductIDs"), String())
                            End If
                            Me.ItemsPerPageDropDownList.Visible = False
                            Me.SortByDropDownList.Visible = False
                        End If
                    End If
                    Dim maxRows As Integer = Convert.ToInt32(RuntimeHelpers.GetObjectValue(Me.Settings.Item("MaxProduct")))
                    Select Case Convert.ToInt32(RuntimeHelpers.GetObjectValue(Me.Settings.Item("MembersOnly")))
                        Case 0
                            no = PersonifyEnumerations.MembersOnlyFilter.No
                            Exit Select
                        Case 1
                            no = PersonifyEnumerations.MembersOnlyFilter.Yes
                            Exit Select
                        Case 2
                            no = PersonifyEnumerations.MembersOnlyFilter.Both
                            Exit Select
                    End Select
                    Dim strArray2 As String() = Nothing
                    If ((Not Me.HiddenFieldClass.Value Is Nothing) And (Me.HiddenFieldClass.Value.Length > 0)) Then
                        strArray2 = New String() { Me.HiddenFieldClass.Value }
                    ElseIf ((Not Me.Settings.Item("Subsystens") Is Nothing) AndAlso (Convert.ToString(RuntimeHelpers.GetObjectValue(Me.Settings.Item("Subsystens"))).Length > 0)) Then
                        strArray2 = Convert.ToString(RuntimeHelpers.GetObjectValue(Me.Settings.Item("Subsystens"))).Split(New Char() { "|"c })
                    End If
                    If ((Not Me.HiddenFieldCategory.Value Is Nothing) And (Me.HiddenFieldCategory.Value.Length > 0)) Then
                        If ((Not Me.HiddenFieldSubcategory.Value Is Nothing) And (Me.HiddenFieldSubcategory.Value.Length < 1)) Then
                            Me.HiddenFieldSubcategory.Value = Nothing
                        End If
                        productIds = Me.get_clsProductHelper.GetProductListing_KeysOnly(Me.HiddenFieldCategory.Value, Me.HiddenFieldSubcategory.Value)
                    ElseIf ((Not Me.HiddenFieldClass.Value Is Nothing) And (Me.HiddenFieldClass.Value.Length > 0)) Then
                        productIds = Me.get_clsProductHelper.GetProductListing_KeysOnly(Me.HiddenFieldClass.Value)
                    ElseIf (str3.Length > 0) Then
                        productIds = str3.Split(New Char() { ","c })
                    Else
                        Dim subSystem As String = String.Empty
                        If ((Not strArray2 Is Nothing) AndAlso (strArray2.Length > 0)) Then
                            subSystem = String.Join(",", strArray2)
                        End If
                        productIds = Me.get_clsProductHelper.GetProductListing_KeysOnly(subSystem, featured, promotional, no, maxRows)
                    End If
                    If ((Not productIds Is Nothing) AndAlso (productIds.Length > 0)) Then
                        list = CachedApplicationData.ApplicationDataCache.GetWebEnabledProductsWithPriceFromCache(productIds, Me.get_clsProductHelper.GetBestQualifiedRateStructureForLoginUserArray, Me.PortalCurrencyCode, Me.BaseCurrency.Code, Me.get_clsProductHelper.DefaultListRateStructure, Me.get_clsProductHelper.DefaultMemberRateStructure)
                    End If
                    If ((Not list Is Nothing) AndAlso (list.Count > 0)) Then
                        Me.PLDataPager.Visible = True
                        Me.PLDataPager.ShowFirstLast = True
                        Me.PLDataPager.PagingMode = PagingModeType.PostBack
                        Me.PLDataPager.PageSize = 10
                        If (Convert.ToInt32(RuntimeHelpers.GetObjectValue(Me.Settings.Item("ItemsPerPage"))) > 0) Then
                            Me.PLDataPager.PageSize = Convert.ToInt32(RuntimeHelpers.GetObjectValue(Me.Settings.Item("ItemsPerPage")))
                        End If
                        If (Me.ItemsPerPageDropDownList.SelectedIndex >= 0) Then
                            Me.PLDataPager.PageSize = Convert.ToInt32(Me.ItemsPerPageDropDownList.SelectedValue)
                        End If
                        Me.PLDataPager.DataSourcePaged.CurrentPageIndex = (Me.PLDataPager.CurrentPage - 1)
                        Me.PLDataPager.DataSource = list
                        Me.PLDataPager.DataBind
                        Me.PLDataPagerBottom.Visible = True
                        Me.PLDataPagerBottom.ShowFirstLast = True
                        Me.PLDataPagerBottom.PagingMode = PagingModeType.PostBack
                        Me.PLDataPagerBottom.PageSize = 10
                        If (Convert.ToInt32(RuntimeHelpers.GetObjectValue(Me.Settings.Item("ItemsPerPage"))) > 0) Then
                            Me.PLDataPagerBottom.PageSize = Convert.ToInt32(RuntimeHelpers.GetObjectValue(Me.Settings.Item("ItemsPerPage")))
                        End If
                        If (Me.ItemsPerPageDropDownList.SelectedIndex >= 0) Then
                            Me.PLDataPagerBottom.PageSize = Convert.ToInt32(Me.ItemsPerPageDropDownList.SelectedValue)
                        End If
                        Me.PLDataPagerBottom.DataSourcePaged.CurrentPageIndex = (Me.PLDataPagerBottom.CurrentPage - 1)
                        Me.PLDataPagerBottom.DataSource = list
                        Me.PLDataPagerBottom.DataBind
                        If (list.Count > Me.PLDataPager.PageSize) Then
                            Me.ItemsPerPageDropDownList.Visible = True
                            Me.SortByDropDownList.Visible = True
                        Else
                            Me.ItemsPerPageDropDownList.Visible = False
                            Me.SortByDropDownList.Visible = False
                        End If
                        If flag2 Then
                            Me.LoadDataToXML
                        End If
                    End If
                Else
                    Skin.AddModuleMessage(Me, LocalizedText.GetLocalizedText("PersonifyMissingSettings.Text", Me.LocalResourceFile), ModuleMessage.ModuleMessageType.RedError)
                End If
                Dim id As String = Me.CheckIfPostBackByButton(("AddToCartControl" & Me.ModuleId.ToString), "Add_To_Cart")
                If Not Me.IsPageRefreshed Then
                    If (id.Length > 0) Then
                        id = id.Replace("$", ":")
                        id = id.Split(New Char() { ":"c })((id.Split(New Char() { ":"c }).Length - 2))
                        Dim tempAddToCartControl As New AddToCartControl
                        tempAddToCartControl = DirectCast(Me.FindControl(id), AddToCartControl)
                        Me.AddToCartControl_ButtonClick(tempAddToCartControl, Nothing)
                    Else
                        id = Me.CheckIfPostBackByButton(("AddToCartControl" & Me.ModuleId.ToString), "Add_To_Wish_List")
                        If (id.Length > 0) Then
                            id = id.Replace("$", ":")
                            id = id.Split(New Char() { ":"c })((id.Split(New Char() { ":"c }).Length - 2))
                            Dim control2 As New AddToCartControl
                            control2 = DirectCast(Me.FindControl(id), AddToCartControl)
                            Me.AddToWishListControl_ButtonClick(control2, Nothing)
                        Else
                            id = Me.CheckIfPostBackByButton(("AddToCartControl" & Me.ModuleId.ToString), "Buy_For_Group")
                            If (id.Length > 0) Then
                                id = id.Replace("$", ":")
                                id = id.Split(New Char() { ":"c })((id.Split(New Char() { ":"c }).Length - 2))
                                Dim control3 As New AddToCartControl
                                control3 = DirectCast(Me.FindControl(id), AddToCartControl)
                                Me.BuyforGroupControl_ButtonClick(control3, Nothing)
                            End If
                        End If
                    End If
                End If
                Me.SaveRefreshState
            Catch exception1 As ThreadAbortException
                ProjectData.SetProjectError(exception1)
                Dim exception As ThreadAbortException = exception1
                ProjectData.ClearProjectError
            Catch exception3 As Exception
                ProjectData.SetProjectError(exception3)
                Dim exc As Exception = exception3
                Exceptions.ProcessModuleLoadException(DirectCast(Me, PortalModuleBase), exc)
                ProjectData.ClearProjectError
            End Try
        End Sub

        Private Sub PLDataPager_Change()
            Me.PLDataPager.DataSourcePaged.CurrentPageIndex = (Me.PLDataPager.CurrentPage - 1)
            Me.PLDataPagerBottom.CurrentPage = Me.PLDataPager.CurrentPage
            Me.PLDataPagerBottom.DataSourcePaged.CurrentPageIndex = (Me.PLDataPager.CurrentPage - 1)
            Me.LoadDataToXML
        End Sub

        Private Sub PLDataPagerBottom_Change()
            Me.PLDataPagerBottom.DataSourcePaged.CurrentPageIndex = (Me.PLDataPagerBottom.CurrentPage - 1)
            Me.PLDataPager.CurrentPage = Me.PLDataPagerBottom.CurrentPage
            Me.PLDataPager.DataSourcePaged.CurrentPageIndex = (Me.PLDataPagerBottom.CurrentPage - 1)
            Me.LoadDataToXML
        End Sub

        Private Sub RedirectOnChange()
            Dim url As String = DotNetNuke.Common.Globals.NavigateURL
            If (url.IndexOf("?") > 0) Then
                url = (url & "&")
            Else
                url = (url & "?")
            End If
            If ((Not Me.HiddenFieldClass.Value Is Nothing) AndAlso (Me.HiddenFieldClass.Value.Length > 0)) Then
                url = (url & "CLASS" & Me.HiddenFieldClass.Value & "&")
            End If
            If ((Not Me.HiddenFieldCategory.Value Is Nothing) AndAlso (Me.HiddenFieldCategory.Value.Length > 0)) Then
                url = (url & "CATEGORY" & Me.HiddenFieldCategory.Value & "&")
            End If
            If ((Not Me.HiddenFieldSubcategory.Value Is Nothing) AndAlso (Me.HiddenFieldSubcategory.Value.Length > 0)) Then
                url = (url & "SUBCATEGORY" & Me.HiddenFieldSubcategory.Value & "&")
            End If
            url = ((url & "SORTBY=" & Me.SortByDropDownList.SelectedValue & "&") & "PAGESIZE=" & Me.ItemsPerPageDropDownList.SelectedValue)
            Me.Response.Redirect(url, True)
        End Sub

        Protected Sub SortByDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
            Me.PLDataPager.PageSize = Convert.ToInt32(Me.ItemsPerPageDropDownList.SelectedValue)
            Me.PLDataPagerBottom.PageSize = Convert.ToInt32(Me.ItemsPerPageDropDownList.SelectedValue)
            Me.LoadDataToXML
        End Sub

        Private Function StripHTML(ByVal objHTML As String) As String
            Dim str As String
            Try 
                If Not Information.IsDBNull(objHTML) Then
                    Dim input As String = objHTML
                    input = New Regex("<(.|\n)+?>", RegexOptions.IgnoreCase).Replace(input, "")
                    Dim regex2 As New Regex("&lt;(.|\n)+?&gt;", RegexOptions.IgnoreCase)
                    Return Strings.Replace(Strings.Replace(regex2.Replace(input, ""), ChrW(10), "", 1, -1, CompareMethod.Binary), ChrW(13), "", 1, -1, CompareMethod.Binary)
                End If
                str = ""
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                Throw exception
            End Try
            Return str
        End Function


        ' Properties
        Protected Overridable Property ItemsPerPageDropDownList As DropDownList
            <DebuggerNonUserCode> _
            Get
                Return Me._ItemsPerPageDropDownList
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode> _
            Set(ByVal WithEventsValue As DropDownList)
                Me._ItemsPerPageDropDownList = WithEventsValue
            End Set
        End Property

        Protected Overridable Property PLDataPager As DataPager
            <DebuggerNonUserCode> _
            Get
                Return Me._PLDataPager
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode> _
            Set(ByVal WithEventsValue As DataPager)
                If (Not Me._PLDataPager Is Nothing) Then
                    RemoveHandler Me._PLDataPager.Change, New DataPager.ChangeEventHandler(AddressOf Me.PLDataPager_Change)
                End If
                Me._PLDataPager = WithEventsValue
                If (Not Me._PLDataPager Is Nothing) Then
                    AddHandler Me._PLDataPager.Change, New DataPager.ChangeEventHandler(AddressOf Me.PLDataPager_Change)
                End If
            End Set
        End Property

        Protected Overridable Property PLDataPagerBottom As DataPager
            <DebuggerNonUserCode> _
            Get
                Return Me._PLDataPagerBottom
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode> _
            Set(ByVal WithEventsValue As DataPager)
                If (Not Me._PLDataPagerBottom Is Nothing) Then
                    RemoveHandler Me._PLDataPagerBottom.Change, New DataPager.ChangeEventHandler(AddressOf Me.PLDataPagerBottom_Change)
                End If
                Me._PLDataPagerBottom = WithEventsValue
                If (Not Me._PLDataPagerBottom Is Nothing) Then
                    AddHandler Me._PLDataPagerBottom.Change, New DataPager.ChangeEventHandler(AddressOf Me.PLDataPagerBottom_Change)
                End If
            End Set
        End Property

        Protected Overridable Property SortByDropDownList As DropDownList
            <DebuggerNonUserCode> _
            Get
                Return Me._SortByDropDownList
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode> _
            Set(ByVal WithEventsValue As DropDownList)
                Me._SortByDropDownList = WithEventsValue
            End Set
        End Property


        ' Fields
        <AccessedThroughProperty("ItemsPerPageDropDownList")> _
        Private _ItemsPerPageDropDownList As DropDownList
        <AccessedThroughProperty("PLDataPager")> _
        Private _PLDataPager As DataPager
        <AccessedThroughProperty("PLDataPagerBottom")> _
        Private _PLDataPagerBottom As DataPager
        <AccessedThroughProperty("SortByDropDownList")> _
        Private _SortByDropDownList As DropDownList
        Private _strListRateStructure As String
        Private _strMemberRateStructure As String
        Private designerPlaceholderDeclaration As Object
        Protected HiddenFieldCategory As HiddenField
        Protected HiddenFieldClass As HiddenField
        Protected HiddenFieldSubcategory As HiddenField
        Private MeetingFull As Boolean
        Protected PLXslTemplate As XslTemplate
        Public Products As ITmarWebProductViewList

        ' Nested Types
        Public Class aProduct
            ' Fields
            Public AddToCartFlag As Boolean
            Public AllPrices As WebPrices
            Public Code As String
            Public EndDate As String
            Public FacilityCity As String
            Public FacilityLabelName As String
            Public FacilityState As String
            Public ListPrices As WebPrices
            Public LongName As String
            Public MaxBadges As Integer
            Public MemberPrices As WebPrices
            Public NextInvReceiptDate As String
            Public ParentCode As String
            Public ProductId As Integer
            Public ShortName As String
            Public SmallImageFileName As String
            Public StartDate As String
            Public Status As String
            Public Subsystem As String
            Public URLToGo As String
            Public WebLongDescription As String
            Public WebShortDescription As String
            Public YourPrices As WebPrices
        End Class

        Public Class aRow
            ' Fields
            Public row As aProduct()
        End Class

        Private Class DefaultBadgeInfo
            ' Fields
            Public City As String
            Public CompanyName As String
            Public FirstName As String
            Public LabelName As String
            Public LastName As String
            Public PostalCode As String
            Public State As String
        End Class
    End Class
End Namespace

