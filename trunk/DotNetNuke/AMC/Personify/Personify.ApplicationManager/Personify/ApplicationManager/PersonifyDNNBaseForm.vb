Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Modules.Actions
Imports DotNetNuke.Entities.Profile
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.UI.Skins
Imports Microsoft.ApplicationBlocks.Data
Imports Microsoft.Security.Application
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports Personify.ApplicationManager.PersonifyDataObjects
Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Web
Imports TIMSS
Imports TIMSS.API
Imports TIMSS.API.ApplicationInfo
Imports TIMSS.API.Core
Imports TIMSS.API.Core.Validation
Imports TIMSS.Common
Imports TIMSS.Enumerations
Imports TIMSS.SqlObjects

Namespace Personify.ApplicationManager
    Public Class PersonifyDNNBaseForm
        Inherits PortalModuleBase
        Implements IActionable
        ' Methods
        Shared Sub New()
            Try 
                PersonifyInitializer.Initialize(HttpContext.Current.Request.PhysicalApplicationPath, PersonifySiteSettings.GetSeatInformation)
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                ProjectData.ClearProjectError
            End Try
        End Sub

        Public Sub New()
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.Page_Load)
            Me._MasterCustomerId = ""
            Me._CurrencySymbol = ""
            Me._SiteImagesFolder = ""
            Me._ProductImagesFolder = ""
            If Not PersonifyInitializer.SystemWasInitialized Then
                PersonifyInitializer.Initialize(HttpContext.Current.Request.PhysicalApplicationPath, PersonifySiteSettings.GetSeatInformation)
            End If
            If Me.IsPersonifyConnected Then
                Me._clsPersonifyConnect = New PersonifyConnect(Me.OrganizationId, Me.OrganizationUnitId)
            End If
        End Sub

        Public Overridable Sub AddSessionObject(ByVal Key As PersonifyEnumerations.SessionKeys, ByVal ObjectToAdd As Object, Optional ByVal AppendToKey As String = "")
            SessionManager.AddSessionObject(Me.PortalId, Key, RuntimeHelpers.GetObjectValue(ObjectToAdd), AppendToKey)
        End Sub

        Public Overridable Sub ClearAllSessionObjects()
            SessionManager.ClearAllSessionObjects(Me.PortalId)
        End Sub

        Private Sub ClearAnonymousUserCookie()
            HttpContext.Current.Request.Cookies.Item("AnonumousTimssCMSUser").Expires = DateTime.Now
        End Sub

        Public Overridable Sub ClearSessionObject(ByVal Key As PersonifyEnumerations.SessionKeys, Optional ByVal AppendToKey As String = "")
            SessionManager.ClearSessionObject(Me.PortalId, Key, AppendToKey)
        End Sub

        Public Overridable Sub ClearSessionObject(ByVal PID As Integer, ByVal Key As PersonifyEnumerations.SessionKeys, Optional ByVal AppendToKey As String = "")
            SessionManager.ClearSessionObject(PID, Key, AppendToKey)
        End Sub

        Public Function ConvertPriceFromBaseToPortalCurrency(ByVal Amount As Decimal) As Decimal
            If (Me.BaseCurrency.Code <> Me.PortalCurrency.Code) Then
                Dim rates As IApplicationExchangeRates = CachedApplicationData.ApplicationDataCache.ExchangeRates(Me.BaseCurrency.Code, Me.PortalCurrency.Code, DateTime.Now)
                If (rates.Count > 0) Then
                    Return TIMSS.Common.Currency.Round(Decimal.Multiply(Amount, rates.Item(0).Xrate), Me.PortalCurrency.Code)
                End If
            End If
            Return Amount


        End Function

        Public Overridable Sub DisplayUserAccessMessage(ByVal sRole As String)
            If Me.IsPersonifyConnected Then
                Select Case sRole
                    Case "none"
                        Skin.AddModuleMessage(Me, "", LocalizedText.GetLocalizedText("NoUserLoggedIn.Text", Me.LocalResourceFile), Me.ResolveUrl(("~/" & Me.GetDNNSiteSettings.ImageURL & "/anonymous_48.gif")))
                        Return
                    Case "host"
                        Skin.AddModuleMessage(Me, "", LocalizedText.GetLocalizedText("PersonifyHostLoggedIn.Text", Me.LocalResourceFile), Me.ResolveUrl(("~/" & Me.GetDNNSiteSettings.ImageURL & "/administrator_lock_48.gif")))
                        Exit Select
                    Case "admin"
                        Skin.AddModuleMessage(Me, "", LocalizedText.GetLocalizedText("PersonifyAdminLoggedIn.Text", Me.LocalResourceFile), Me.ResolveUrl(("~/" & Me.GetDNNSiteSettings.ImageURL & "/administrator_lock_48.gif")))
                        Exit Select
                End Select
            End If
        End Sub

        Private Function FindValidationIssueByKey(ByVal VIIssues As IssuesCollection, ByVal KeyToFind As String) As IIssue
            Dim enumerator As IEnumerator
            Try 
                enumerator = VIIssues.GetEnumerator
                Do While enumerator.MoveNext
                    Dim current As IIssue = DirectCast(enumerator.Current, IIssue)
                    If (current.Key = KeyToFind) Then
                        Return current
                    End If
                Loop
            Finally
                If TypeOf enumerator Is IDisposable Then
                    TryCast(enumerator,IDisposable).Dispose
                End If
            End Try
            Return Nothing
        End Function

        Public Function FormatAmount(ByVal Amount As Decimal) As String
            Return String.Format("{0}{1}", Me.PortalCurrency.Symbol, TIMSS.Common.Currency.Round(Amount, Me.PortalCurrency.Symbol))
        End Function

        Public Function get_clsAffiliateManagement() As AffiliateManagement
            Return New AffiliateManagement(Me.OrganizationId, Me.OrganizationUnitId, Me.FetchOnDemand)
        End Function

        Public Function get_clsOrderCheckOutProcessHelper() As OrderCheckOutProcessHelper
            Return New OrderCheckOutProcessHelper(Me.OrganizationId, Me.OrganizationUnitId, Me.BaseCurrency, Me.PortalCurrency, Me.FetchOnDemand)
        End Function

        Public Function get_clsProductHelper() As ProductHelper
            Dim helper As New ProductHelper(Me.OrganizationId, Me.OrganizationUnitId, Me.BaseCurrency, Me.PortalCurrency, Me.FetchOnDemand, Me.PortalId)
            helper.MCID = Me.MasterCustomerId
            helper.SCID = Me.SubCustomerId
            Return helper
        End Function

        Private Function GetAnonymousUserCookie() As HttpCookie
            Dim cookie As HttpCookie = Nothing
            If HttpContext.Current.Request.Browser.Cookies Then
                cookie = HttpContext.Current.Request.Cookies.Item("AnonumousTimssCMSUser")
            End If
            Return cookie
        End Function

        Public Function GetAnonymousUserId(ByVal MCID As String) As String
            Dim anonUserId As String = ""
            If ((MCID Is Nothing) OrElse (MCID = String.Empty)) Then
                If (Me.GetAnonymousUserCookie Is Nothing) Then
                    If (Not HttpContext.Current.Session Is Nothing) Then
                        HttpContext.Current.Session.Item("ShoppingCartSession") = HttpContext.Current.Session.SessionID.ToString
                        anonUserId = HttpContext.Current.Session.Item("ShoppingCartSession").ToString
                    End If
                    Return anonUserId
                End If
                anonUserId = Me.GetAnonymousUserCookie.Value
                Me.SetAnonymousUserCookie(anonUserId)
                Return anonUserId
            End If
            Return MCID
        End Function

        Public Function GetApplicationCodes(ByVal Subsystem As String, ByVal Type As String, ByVal GetWebEnabledCodesOnly As Boolean) As IApplicationCodes
            Return Me._clsPersonifyConnect.GetApplicationCodes(Subsystem, Type, GetWebEnabledCodesOnly)
        End Function

        Public Function GetApplicationSubCodes(ByVal Subsystem As String, ByVal CodeType As String, ByVal Code As String, ByVal GetWebEnabledSubCodesOnly As Boolean) As IApplicationSubcodes
            Return Me._clsPersonifyConnect.GetApplicationSubCodes(Subsystem, CodeType, Code, GetWebEnabledSubCodesOnly)
        End Function

        Protected Overridable Function GetBaseCurrency() As Personify.ApplicationManager.PersonifyDataObjects.Currency
            Dim currencySymbol As String = ""
            Dim organizations As IApplicationOrganizations = CachedApplicationData.ApplicationDataCache.ApplicationOrganizations(Me.OrganizationId)
            If (organizations.Count > 0) Then
                Dim currencies As IApplicationCurrencies = CachedApplicationData.ApplicationDataCache.Currencies(organizations.Item(0).CurrencyCodeString)
                If (currencies.Count > 0) Then
                    currencySymbol = currencies.Item(0).CurrencySymbol
                End If
            End If
            Dim currency2 As New Personify.ApplicationManager.PersonifyDataObjects.Currency
            currency2.Code = organizations.Item(0).CurrencyCodeString
            currency2.Symbol = currencySymbol
            Return currency2
        End Function

        Private Function GetCurrentUser(ByVal UserId As Integer, ByVal PortalId As Integer, ByVal UserInfo As ProfilePropertyDefinitionCollection) As LoginUser
            Dim user2 As New LoginUser
            user2.isLoggedin = False
            If AffiliateManagementSessionHelper.IsManagingAffiliate(PortalId) Then
                Dim currentAffiliateInfo As AffiliateManagementSessionHelper.AffiliateInfo = AffiliateManagementSessionHelper.GetCurrentAffiliateInfo(PortalId)
                If (Not currentAffiliateInfo.AffiliateCustomerId.MasterCustomerId Is Nothing) Then
                    user2.MasterCustomerId = currentAffiliateInfo.AffiliateCustomerId.MasterCustomerId
                    user2.SubCustomerId = currentAffiliateInfo.AffiliateCustomerId.SubCustomerId
                    user2.isLoggedin = True
                End If
            Else
                If (((Not UserInfo.Item("MasterCustomerId") Is Nothing) AndAlso (Not UserInfo.Item("MasterCustomerId") Is Nothing)) AndAlso (Not UserInfo.Item("MasterCustomerId").PropertyValue Is Nothing)) Then
                    user2.MasterCustomerId = Convert.ToString(UserInfo.Item("MasterCustomerId").PropertyValue)
                    user2.isLoggedin = True
                Else
                    user2.MasterCustomerId = Me.GetAnonymousUserId("")
                End If
                If (((Not UserInfo.Item("SubCustomerId") Is Nothing) AndAlso (Not UserInfo.Item("SubCustomerId") Is Nothing)) AndAlso (Not UserInfo.Item("SubCustomerId").PropertyValue Is Nothing)) Then
                    user2.SubCustomerId = Convert.ToInt32(UserInfo.Item("SubCustomerId").PropertyValue)
                Else
                    user2.SubCustomerId = 0
                End If
            End If
            If (Not HttpContext.Current.Session Is Nothing) Then
                Dim rolesByUser As String() = New RoleController().GetRolesByUser(UserId, PortalId)
                If (If(((Not rolesByUser Is Nothing) AndAlso (rolesByUser.Length > 0)), 1, 0) = 0) Then
                    Return user2
                End If
                Dim str As String
                For Each str In rolesByUser
                    If ((str = "PersonifyStaff") AndAlso ((Not Convert.ToString(RuntimeHelpers.GetObjectValue(HttpContext.Current.Session.Item("staffmcid"))) Is Nothing) AndAlso (Convert.ToString(RuntimeHelpers.GetObjectValue(HttpContext.Current.Session.Item("staffmcid"))).Length > 0))) Then
                        user2.MasterCustomerId = Convert.ToString(RuntimeHelpers.GetObjectValue(HttpContext.Current.Session.Item("staffmcid")))
                        user2.SubCustomerId = Convert.ToInt32(RuntimeHelpers.GetObjectValue(HttpContext.Current.Session.Item("staffscid")))
                        user2.LabelName = Convert.ToString(RuntimeHelpers.GetObjectValue(HttpContext.Current.Session.Item("staffln")))
                        user2.isLoggedin = True
                    End If
                Next
            End If
            Return user2
        End Function

        Public Overridable Function GetDefaultCountryCodeForOrganization() As String
            Return CachedApplicationData.ApplicationDataCache.ApplicationOrganizations(Me.OrganizationId).Item(0).DefaultCountryCode.Code
        End Function

        Public Overridable Function GetDNNSiteSettings() As SiteSettings
            Dim key As String = ("PersonifySiteSettings" & Me.PortalId.ToString)
            If (PersonifyDataCache.Fetch(key) Is Nothing) Then
                Dim settings2 As SiteSettings
                Dim reader As SqlDataReader = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings.Item("SiteSqlServer").ConnectionString, "dbo.GetPersonifySiteSettings", New Object() { Me.PortalId })
                If reader.HasRows Then
                    settings2 = New SiteSettings(Me.PortalId)
                    Do While reader.Read
                        If (reader.Item("AffiliatePortalTabId").ToString <> "") Then
                            settings2.AffilatePortalTabId = Conversions.ToInteger(reader.Item("AffiliatePortalTabId").ToString)
                        Else
                            settings2.AffilatePortalTabId = 0
                        End If
                        If (reader.Item("MyPortalTabId").ToString <> "") Then
                            settings2.MyPortalTabId = Conversions.ToInteger(reader.Item("MyPortalTabId").ToString)
                        Else
                            settings2.MyPortalTabId = 0
                        End If
                        If (reader.Item("ProductImageURL").ToString <> "") Then
                            settings2.ProductImageURL = reader.Item("ProductImageURL").ToString
                        Else
                            settings2.ProductImageURL = ""
                        End If
                        If (reader.Item("AdminEmailAddress").ToString <> "") Then
                            settings2.AdminEmailAddress = reader.Item("AdminEmailAddress").ToString
                        Else
                            settings2.AdminEmailAddress = ""
                        End If
                        If (reader.Item("PasswordRegularExpression").ToString <> "") Then
                            settings2.PasswordRegularExpression = reader.Item("PasswordRegularExpression").ToString
                        Else
                            settings2.PasswordRegularExpression = ""
                        End If
                        If (reader.Item("ImageURL").ToString <> "") Then
                            settings2.ImageURL = reader.Item("ImageURL").ToString
                        Else
                            settings2.ImageURL = ""
                        End If
                        settings2.OrgId = reader.Item("OrganizationId").ToString
                        settings2.OrgUnitId = reader.Item("OrganizationUnitId").ToString
                        settings2.PortalCurrency = reader.Item("PortalCurrency").ToString
                    Loop
                    PersonifyDataCache.Store(key, settings2, PersonifyDataCache.CacheExpirationInterval)
                Else
                    settings2 = Nothing
                End If
                If (Not reader Is Nothing) Then
                    reader.Close
                End If
                reader = Nothing
            End If
            Return DirectCast(PersonifyDataCache.Fetch(key), SiteSettings)
        End Function

        Protected Overridable Function GetPortalCurrency() As Personify.ApplicationManager.PersonifyDataObjects.Currency
            Dim currency2 As New Personify.ApplicationManager.PersonifyDataObjects.Currency
            currency2.Code = PersonifySiteSettings.GetSiteSettings(Me.PortalId).PortalCurrency
            Dim currencies As IApplicationCurrencies = CachedApplicationData.ApplicationDataCache.Currencies(currency2.Code)
            If (currencies.Count > 0) Then
                currency2.Symbol = currencies.Item(0).CurrencySymbol
            End If
            Return currency2
        End Function

        Public Sub GetRefreshState()
            Me._refreshState = Conversions.ToBoolean(Me.ViewState.Item("__REFSTATE"))
            Me._isRefresh = (Me._refreshState = Conversions.ToBoolean(Me.Session.Item("__ISREFRESH")))
        End Sub

        Public Overridable Function GetSessionObject(ByVal Key As PersonifyEnumerations.SessionKeys, Optional ByVal AppendToKey As String = "") As Object
            Return SessionManager.GetSessionObject(Me.PortalId, Key, AppendToKey)
        End Function

        Public Overridable Function GetUserRole(ByVal UserInfo As UserInfo) As String
            Dim flag2 As Boolean = False
            If (((Not UserInfo.Profile.GetPropertyValue("MasterCustomerId") Is Nothing) AndAlso (UserInfo.Profile.GetPropertyValue("MasterCustomerId").Length > 0)) AndAlso ((Not UserInfo.Profile.GetPropertyValue("SubCustomerId") Is Nothing) AndAlso (UserInfo.Profile.GetPropertyValue("SubCustomerId").Length >= 0))) Then
                flag2 = True
            End If
            Dim str2 As String = "none"
            Dim flag As Boolean = False
            If UserInfo.IsSuperUser Then
                Return "host"
            End If
            Dim controller As New RoleController
            Dim str3 As String
            For Each str3 In controller.GetRolesByUser(UserInfo.UserID, UserInfo.PortalID)
                If (str3 = "Administrators") Then
                    flag = True
                End If
            Next
            If flag Then
                If flag2 Then
                    Return "personifyadmin"
                End If
                Return "admin"
            End If
            If flag2 Then
                Return "personifyuser"
            End If
            If (UserInfo.UserID > 0) Then
                str2 = "user"
            End If
            Return str2
        End Function

        Public Function isDateWithinRange(ByVal startDate As DateTime, ByVal endDate As DateTime) As Boolean
            If ((DateTime.Compare(startDate, DateAndTime.Today) > 0) Or ((DateTime.Compare(endDate, Conversions.ToDate("#12:00:00 AM#")) <> 0) AndAlso (DateTime.Compare(endDate, DateAndTime.Today) < 0))) Then
                Return False
            End If
            Return True
        End Function

        Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
            If (Not Me.Settings.Item("OnDemandDataLoad") Is Nothing) Then
                Me.FetchOnDemand = Conversions.ToBoolean(Me.Settings.Item("OnDemandDataLoad"))
            Else
                Me.FetchOnDemand = False
            End If
            If Not Me.IsPersonifyConnected Then
                If (Me.GetUserRole(Me.UserInfo) = "host") Then
                    Skin.AddModuleMessage(Me, "Personify Authentication Failed", "Connection Parameters to Personify are incorrect. Check the Personify Site Settings.", Me.ResolveUrl(("~/" & Me.GetDNNSiteSettings.ImageURL & "/close_b_48.gif")))
                Else
                    Skin.AddModuleMessage(Me, "", "Web Site Is Not Available at this time.", Me.ResolveUrl(("~/" & Me.GetDNNSiteSettings.ImageURL & "/close_b_48.gif")))
                End If
            End If
        End Sub

        Public Function PersonifyExecuteQueryRequest(ByVal request As IQueryRequest) As IQueryResult
            Return Me._clsPersonifyConnect.PersonifyExecuteQueryRequest(request)
        End Function

        Public Function PersonifyExecuteQueryRequest(ByVal requestsToExecute As QueryRequestCollection) As QueryResultCollection
            Return Me._clsPersonifyConnect.PersonifyExecuteQueryRequest(requestsToExecute)
        End Function

        Public Function PersonifyExecuteQueryRequest(ByVal request As IBaseRequest, ByVal parameters As IDictionary) As IQueryResult
            Return Me._clsPersonifyConnect.PersonifyExecuteQueryRequest(request, parameters)
        End Function

        Public Function PersonifyExecuteQueryRequest(ByVal request As IBaseRequest, ByVal ParamArray parameters As Object()) As IQueryResult
            Return Me._clsPersonifyConnect.PersonifyExecuteQueryRequest(request, parameters)
        End Function

        Public Function PersonifyGetCollection(ByVal TypeName As String) As IBusinessObjectCollection
            Dim objects As IBusinessObjectCollection = Me._clsPersonifyConnect.PersonifyGetCollection(TypeName)
            objects.EnableIncrementalDataLoad = Me.FetchOnDemand
            Return objects
        End Function

        Public Function PersonifyGetCollection(ByVal TypeName As Type) As IBusinessObjectCollection
            Dim objects As IBusinessObjectCollection = Me._clsPersonifyConnect.PersonifyGetCollection(TypeName)
            objects.EnableIncrementalDataLoad = Me.FetchOnDemand
            Return objects
        End Function

        Public Overridable Function PersonifyGetCollection(ByVal [NameSpace] As NamespaceEnum, ByVal TypeName As String) As IBusinessObjectCollection
            Dim objects As IBusinessObjectCollection = Me._clsPersonifyConnect.PersonifyGetCollection([NameSpace], TypeName)
            objects.EnableIncrementalDataLoad = Me.FetchOnDemand
            Return objects
        End Function

        Public Overridable Sub RespondToValidationIssues(ByRef UnRespondedVIs As IssuesCollection, ByRef RespondedVIs As IssuesCollection)
            Dim enumerator As IEnumerator
            Try 
                enumerator = UnRespondedVIs.GetEnumerator
                Do While enumerator.MoveNext
                    Dim current As IIssue = DirectCast(enumerator.Current, IIssue)
                    If current.ResponseRequired Then
                        Dim issue As IIssue = Nothing
                        issue = Me.FindValidationIssueByKey(RespondedVIs, current.Key)
                        If ((Not issue Is Nothing) AndAlso (Not issue.Responses.SelectedResponse Is Nothing)) Then
                            current.Responses.SelectedResponse = issue.Responses.SelectedResponse
                        End If
                    End If
                Loop
            Finally
                If TypeOf enumerator Is IDisposable Then
                    TryCast(enumerator,IDisposable).Dispose
                End If
            End Try
        End Sub

        Public Sub SaveRefreshState()
            Me.Session.Item("__ISREFRESH") = Me._refreshState
            Me.ViewState.Item("__REFSTATE") = Not Me._refreshState
        End Sub

        Private Sub SetAnonymousUserCookie(ByVal AnonUserId As String)
            If HttpContext.Current.Request.Browser.Cookies Then
                Dim cookie As New HttpCookie("AnonumousTimssCMSUser")
                cookie.Expires = DateTime.Now.AddDays(90)
                cookie.Value = AnonUserId
            End If
        End Sub

        Private Sub SetCustomerIds()
            Dim user As New LoginUser
            user = Me.GetCurrentUser(Me.UserId, Me.PortalId, Me.UserInfo.Profile.ProfileProperties)
            Me._IsUserLoggedIn = user.isLoggedin
            Me._LabelName = Conversions.ToInteger(user.LabelName)
            Me._MasterCustomerId = user.MasterCustomerId
            Me._SubCustomerId = user.SubCustomerId
        End Sub

        Public Function XSS_HTMLEncode(ByVal value As String) As String
            Return AntiXss.HtmlEncode(value)
        End Function


        ' Properties
        Public Overridable ReadOnly Property BaseCurrency As Personify.ApplicationManager.PersonifyDataObjects.Currency
            Get
                If (Me._BaseCurrency Is Nothing) Then
                    Me._BaseCurrency = Me.GetBaseCurrency
                End If
                Return Me._BaseCurrency
            End Get
        End Property

        Public ReadOnly Property DoCreditCardAuthorization As Boolean
            Get
                Dim flag As Boolean
                Dim objectValue As Object = RuntimeHelpers.GetObjectValue(New Object)
                Monitor.Enter(RuntimeHelpers.GetObjectValue(objectValue))
                Try 
                    If (ConfigurationManager.AppSettings.Item("CreditCardAuthorization") Is Nothing) Then
                        Return True
                    End If
                    If (String.Compare(ConfigurationManager.AppSettings.Item("CreditCardAuthorization"), "OFF", True) = 0) Then
                        Return False
                    End If
                    flag = True
                Catch exception1 As Exception
                    ProjectData.SetProjectError(exception1)
                    Dim exception As Exception = exception1
                    flag = True
                    ProjectData.ClearProjectError
                Finally
                    Monitor.Exit(RuntimeHelpers.GetObjectValue(objectValue))
                End Try
                Return flag
            End Get
        End Property

        'Public Overridable ReadOnly Property DotNetNuke.Entities.Modules.IActionable.ModuleActions As ModuleActionCollection
        '    Get
        '        Dim actions As New ModuleActionCollection
        '        actions.Add(Me.GetNextActionID, Localization.GetString("AddContent.Action", Me.LocalResourceFile), "AddContent.Action", "", "", Me.EditUrl, False, SecurityAccessLevel.Edit, True, False)
        '        Return actions
        '    End Get
        'End Property

        Public Overridable ReadOnly Property ModuleActions As ModuleActionCollection Implements IActionable.ModuleActions
            Get
                Dim actions As New ModuleActionCollection
                actions.Add(Me.GetNextActionID, Localization.GetString("AddContent.Action", Me.LocalResourceFile), "AddContent.Action", "", "", Me.EditUrl, False, SecurityAccessLevel.Edit, True, False)
                Return actions
            End Get
        End Property

        'Public Overridable ReadOnly Property IActionable_ModuleActions() As ModuleActionCollection Implements IActionable.ModuleActions
        '    Get
        '        Dim actions As New ModuleActionCollection
        '        actions.Add(Me.GetNextActionID, Localization.GetString("AddContent.Action", Me.LocalResourceFile), "AddContent.Action", "", "", Me.EditUrl, False, SecurityAccessLevel.Edit, True, False)
        '        Return actions
        '    End Get
        'End Property



        Public Overridable Property FetchOnDemand As Boolean
            Get
                Return Me._FetchOnDemand
            End Get
            Set(ByVal value As Boolean)
                Me._FetchOnDemand = value
            End Set
        End Property

        Public ReadOnly Property IsPageRefreshed As Boolean
            Get
                Return Me._isRefresh
            End Get
        End Property

        Public Overridable ReadOnly Property IsPersonifyConnected As Boolean
            Get
                Return PersonifyInitializer.SystemWasInitialized
            End Get
        End Property

        Public Overridable ReadOnly Property IsPersonifyWebUserLoggedIn As Boolean
            Get
                If (Me._MasterCustomerId.Length = 0) Then
                    Me.SetCustomerIds
                End If
                Return Me._IsUserLoggedIn
            End Get
        End Property

        Public Overridable ReadOnly Property LabelName As String
            Get
                If (Me._MasterCustomerId.Length = 0) Then
                    Me.SetCustomerIds
                End If
                Return Conversions.ToString(Me._LabelName)
            End Get
        End Property

        Public Overridable ReadOnly Property MasterCustomerId As String
            Get
                If (Me._MasterCustomerId.Length = 0) Then
                    Me.SetCustomerIds
                End If
                Return Me._MasterCustomerId
            End Get
        End Property

        

        Public Overridable ReadOnly Property OrganizationId As String
            Get
                If String.IsNullOrEmpty(Me._OrgId) Then
                    Me._OrgId = PersonifySiteSettings.GetSiteSettings(Me.PortalId).OrgId
                End If
                Return Me._OrgId
            End Get
        End Property

        Public Overridable ReadOnly Property OrganizationUnitId As String
            Get
                If String.IsNullOrEmpty(Me._OrgUnitId) Then
                    Me._OrgUnitId = PersonifySiteSettings.GetSiteSettings(Me.PortalId).OrgUnitId
                End If
                Return Me._OrgUnitId
            End Get
        End Property

        Public Overridable ReadOnly Property PortalCurrency As Personify.ApplicationManager.PersonifyDataObjects.Currency
            Get
                If (Me._PortalCurrency Is Nothing) Then
                    Me._PortalCurrency = Me.GetPortalCurrency
                End If
                Return Me._PortalCurrency
            End Get
        End Property

        Public Overridable ReadOnly Property PortalCurrencyCode As String
            Get
                If (Me._ProductImagesFolder.Length = 0) Then
                    Me._ProductImagesFolder = Me.GetDNNSiteSettings.ProductImageURL
                End If
                Return Me._ProductImagesFolder
            End Get
        End Property

        Public Overridable ReadOnly Property ProductImagesFolder As String
            Get
                If (Me._ProductImagesFolder.Length = 0) Then
                    Me._ProductImagesFolder = Me.GetDNNSiteSettings.ProductImageURL
                End If
                Return Me._ProductImagesFolder
            End Get
        End Property

        Public Overridable ReadOnly Property SiteImagesFolder As String
            Get
                If (Me._SiteImagesFolder.Length = 0) Then
                    Me._SiteImagesFolder = Me.GetDNNSiteSettings.ImageURL
                End If
                Return Me._SiteImagesFolder
            End Get
        End Property

        Public Overridable ReadOnly Property SubCustomerId As Integer
            Get
                If (Me._MasterCustomerId.Length = 0) Then
                    Me.SetCustomerIds
                End If
                Return Me._SubCustomerId
            End Get
        End Property


        ' Fields
        Private _BaseCurrency As Personify.ApplicationManager.PersonifyDataObjects.Currency
        Private _clsPersonifyConnect As PersonifyConnect
        Private _CurrencySymbol As String
        Private _FetchOnDemand As Boolean
        Private _isRefresh As Boolean
        Private _IsUserLoggedIn As Boolean
        Private _LabelName As Integer
        Private _MasterCustomerId As String
        Private _OrgId As String
        Private _OrgUnitId As String
        Private _PortalCurrency As Personify.ApplicationManager.PersonifyDataObjects.Currency
        Private _ProductImagesFolder As String
        Private _refreshState As Boolean
        Private _SiteImagesFolder As String
        Private _SubCustomerId As Integer
        Public Const C_EMAIL_REGULAR_EXPRESSION As String = "^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"

        ' Nested Types
        Private Class LoginUser
            ' Fields
            Public isLoggedin As Boolean
            Public LabelName As String
            Public MasterCustomerId As String
            Public SubCustomerId As Integer
        End Class

       
    End Class
End Namespace

