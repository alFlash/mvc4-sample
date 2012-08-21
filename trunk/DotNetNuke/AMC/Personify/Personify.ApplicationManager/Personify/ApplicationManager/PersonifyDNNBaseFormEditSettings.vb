Imports DotNetNuke.Entities.Modules
Imports Microsoft.VisualBasic.CompilerServices
Imports Personify.ApplicationManager.PersonifyDataObjects
Imports System
Imports System.Collections
Imports System.Diagnostics
Imports System.Runtime.CompilerServices
Imports System.Web
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports TIMSS
Imports TIMSS.API
Imports TIMSS.API.ApplicationInfo
Imports TIMSS.API.Core
Imports TIMSS.Enumerations
Imports TIMSS.SqlObjects

Namespace Personify.ApplicationManager
    Public Class PersonifyDNNBaseFormEditSettings
        Inherits PortalModuleBase
        ' Methods
        Shared Sub New()
            PersonifyInitializer.Initialize(HttpContext.Current.Request.PhysicalApplicationPath, PersonifySiteSettings.GetSeatInformation)
        End Sub

        Public Sub New()
            AddHandler MyBase.PreRender, New EventHandler(AddressOf Me.Page_PreRender)
            AddHandler MyBase.Unload, New EventHandler(AddressOf Me.Page_Unload)
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.Page_Load)
            Dim list As ArrayList = PersonifyDNNBaseFormEditSettings.__ENCList
            SyncLock list
                PersonifyDNNBaseFormEditSettings.__ENCList.Add(New WeakReference(Me))
            End SyncLock
            Me._SiteImagesFolder = ""
            Me._clsPersonifyConnect = New PersonifyConnect(Me.OrganizationId, Me.OrganizationUnitId)
        End Sub

        Public Function GetApplicationCodes(ByVal Subsystem As String, ByVal Type As String, ByVal GetWebEnabledCodesOnly As Boolean) As IApplicationCodes
            Return Me._clsPersonifyConnect.GetApplicationCodes(Subsystem, Type, GetWebEnabledCodesOnly)
        End Function

        Public Function GetApplicationSubCodes(ByVal Subsystem As String, ByVal CodeType As String, ByVal Code As String, ByVal GetWebEnabledSubCodesOnly As Boolean) As IApplicationSubcodes
            Return Me._clsPersonifyConnect.GetApplicationSubCodes(Subsystem, CodeType, Code, GetWebEnabledSubCodesOnly)
        End Function

        Public Function GetDefaultCountryCodeForOrganization() As String
            Return CachedApplicationData.ApplicationDataCache.ApplicationOrganizations(Me.OrganizationId).Item(0).DefaultCountryCode.Code
        End Function

        Public Function GetDNNSiteSettings() As SiteSettings
            Dim key As String = ("PersonifySiteSettings" & Me.PortalId.ToString)
            If (PersonifyDataCache.Fetch(key) Is Nothing) Then
                PersonifySiteSettings.GetSiteSettings(Me.PortalId)
            End If
            Return DirectCast(PersonifyDataCache.Fetch(key), SiteSettings)
        End Function

        Private Sub InsertControlForOnDemandDataLoad()
            Dim child As New HtmlGenericControl("Div")
            Dim link As New HyperLink
            link.Text = "This is a system setting, click for more information"
            link.NavigateUrl = "~/DefaultPersonifyImages/OnDemandDataFetch.htm"
            link.Target = "_new"
            Me.chkEnableOnDemand = New CheckBox
            Me.chkEnableOnDemand.ID = "chkOnDemand"
            Me.chkEnableOnDemand.Text = "Enable On Demand Data Fetch. "
            Me.chkEnableOnDemand.Visible = True
            Me.chkEnableOnDemand.Height = &H19
            Me.chkEnableOnDemand.EnableViewState = True
            If (Not Me.Settings.Item("OnDemandDataLoad") Is Nothing) Then
                Me.chkEnableOnDemand.Checked = Me.FetchDataOnDemand
            End If
            child.Style.Item("position") = "relative;"
            child.Style.Item("bottom") = "55px;"
            child.Controls.Add(Me.chkEnableOnDemand)
            child.Controls.Add(link)
            Me.Controls.Add(child)
        End Sub

        Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
            Me.InsertControlForOnDemandDataLoad
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs)
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As EventArgs)
            Me._objModules = Nothing
        End Sub

        Private Sub Personify_SetCurrentContext()
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
            Return Me._clsPersonifyConnect.PersonifyGetCollection(TypeName)
        End Function

        Public Function PersonifyGetCollection(ByVal TypeName As Type) As IBusinessObjectCollection
            Return Me._clsPersonifyConnect.PersonifyGetCollection(TypeName)
        End Function

        Public Function PersonifyGetCollection(ByVal [NameSpace] As NamespaceEnum, ByVal TypeName As String) As IBusinessObjectCollection
            Return Me._clsPersonifyConnect.PersonifyGetCollection([NameSpace], TypeName)
        End Function

        Public Sub UpdateModuleSetting(ByVal SettingName As String, ByVal SettingValue As String)
            Me.ModuleController.UpdateModuleSetting(Me.ModuleId, SettingName, SettingValue)
        End Sub


        ' Properties
        Public Overridable Property chkEnableOnDemand As CheckBox
            <DebuggerNonUserCode> _
            Get
                Return Me._chkEnableOnDemand
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode> _
            Set(ByVal WithEventsValue As CheckBox)
                Me._chkEnableOnDemand = WithEventsValue
            End Set
        End Property

        Public ReadOnly Property FetchDataOnDemand As Boolean
            Get
                Return ((Not Me.Settings.Item("OnDemandDataLoad") Is Nothing) AndAlso Conversions.ToBoolean(Me.Settings.Item("OnDemandDataLoad")))
            End Get
        End Property

        Public ReadOnly Property ModuleController As ModuleController
            Get
                If (Me._objModules Is Nothing) Then
                    Me._objModules = New ModuleController
                End If
                Return Me._objModules
            End Get
        End Property

        Public ReadOnly Property OrganizationId As String
            Get
                If String.IsNullOrEmpty(Me._OrgId) Then
                    Me._OrgId = PersonifySiteSettings.GetSiteSettings(Me.PortalId).OrgId
                End If
                Return Me._OrgId
            End Get
        End Property

        Public ReadOnly Property OrganizationUnitId As String
            Get
                If String.IsNullOrEmpty(Me._OrgUnitId) Then
                    Me._OrgUnitId = PersonifySiteSettings.GetSiteSettings(Me.PortalId).OrgUnitId
                End If
                Return Me._OrgUnitId
            End Get
        End Property

        Public ReadOnly Property SiteImagesFolder As String
            Get
                If (Me._SiteImagesFolder.Length = 0) Then
                    Me._SiteImagesFolder = Me.GetDNNSiteSettings.ImageURL
                End If
                Return Me._SiteImagesFolder
            End Get
        End Property


        ' Fields
        Private Shared __ENCList As ArrayList = New ArrayList
        <AccessedThroughProperty("chkEnableOnDemand")> _
        Private _chkEnableOnDemand As CheckBox
        Private _clsPersonifyConnect As PersonifyConnect
        Private _objModules As ModuleController
        Private _OrgId As String
        Private _OrgUnitId As String
        Private _SiteImagesFolder As String
        Public Const Setting_OnDemandDataLoad As String = "OnDemandDataLoad"
    End Class
End Namespace

