Imports System
Imports System.Collections
Imports Microsoft.VisualBasic.CompilerServices
Imports TIMSS
Imports TIMSS.Enumerations
Imports TIMSS.API.Core
Imports TIMSS.SqlObjects
Imports TIMSS.API.ApplicationInfo
Imports System.Web

Namespace Personify.ApplicationManager
    Public Class PersonifyConnect
        ' Methods
        Public Sub New (ByVal DefaultPortalID As Integer)
            If Not PersonifyInitializer.SystemWasInitialized Then
                PersonifyInitializer.Initialize (HttpContext.Current.Request.PhysicalApplicationPath,
                                                 PersonifySiteSettings.GetSeatInformation)
            End If
            Me._OrgId = PersonifySiteSettings.GetSiteSettings (DefaultPortalID).OrgId
            Me._OrgUnitId = PersonifySiteSettings.GetSiteSettings (DefaultPortalID).OrgId
        End Sub

        Public Sub New (ByVal OrgId As String, ByVal OrgUnitId As String)
            Me._OrgId = OrgId
            Me._OrgUnitId = OrgUnitId
            If Not PersonifyInitializer.SystemWasInitialized Then
                PersonifyInitializer.Initialize (HttpContext.Current.Request.PhysicalApplicationPath,
                                                 PersonifySiteSettings.GetSeatInformation)
            End If
        End Sub

        Public Function GetApplicationCodes (ByVal Subsystem As String, ByVal Type As String,
                                             ByVal GetWebEnabledCodesOnly As Boolean) As IApplicationCodes
            Dim params As CacheFilterItem() = New CacheFilterItem() _
                    {New CacheFilterItem ("Subsystem", Subsystem), New CacheFilterItem ("Type", Type),
                     New CacheFilterItem ("ActiveFlag", "Y")}
            If GetWebEnabledCodesOnly Then
                params = DirectCast (Utils.CopyArray (DirectCast (params, Array), New CacheFilterItem(4 - 1) {}),
                                     CacheFilterItem())
                params (3) = New CacheFilterItem ("AvailableToWebFlag", "Y")
            End If
            Return DirectCast (CacheLoader.LoadBOCFromCache (GetType (IApplicationCodes), params), IApplicationCodes)
        End Function

        Public Function GetApplicationSubCodes (ByVal Subsystem As String, ByVal CodeType As String,
                                                ByVal Code As String, ByVal GetWebEnabledSubCodesOnly As Boolean) _
            As IApplicationSubcodes
            Dim params As CacheFilterItem() = New CacheFilterItem() _
                    {New CacheFilterItem ("Subsystem", Subsystem), New CacheFilterItem ("Type", CodeType),
                     New CacheFilterItem ("Code", Code), New CacheFilterItem ("ActiveFlag", "Y")}
            If GetWebEnabledSubCodesOnly Then
                params = DirectCast (Utils.CopyArray (DirectCast (params, Array), New CacheFilterItem(5 - 1) {}),
                                     CacheFilterItem())
                params (4) = New CacheFilterItem ("AvailableToWebFlag", "Y")
            End If
            Return _
                DirectCast (CacheLoader.LoadBOCFromCache (GetType (IApplicationSubcodes), params), IApplicationSubcodes)
        End Function

        Public Function PersonifyExecuteQueryRequest (ByVal request As IQueryRequest) As IQueryResult
            Return [Global].App.Execute (request)
        End Function

        Public Function PersonifyExecuteQueryRequest (ByVal requestsToExecute As QueryRequestCollection) _
            As QueryResultCollection
            Return [Global].App.Execute (requestsToExecute)
        End Function

        Public Function PersonifyExecuteQueryRequest (ByVal request As IBaseRequest, ByVal parameters As IDictionary) _
            As IQueryResult
            Return [Global].App.Execute (request, parameters)
        End Function

        Public Function PersonifyExecuteQueryRequest (ByVal request As IBaseRequest,
                                                      ByVal ParamArray parameters As Object()) As IQueryResult
            Return [Global].App.Execute (request, parameters)
        End Function

        Public Function PersonifyGetCollection (ByVal TypeName As String) As IBusinessObjectCollection
            Return [Global].GetCollection (Me.OrganizationId, Me.OrganizationUnitId, TypeName)
        End Function

        Public Function PersonifyGetCollection (ByVal TypeName As Type) As IBusinessObjectCollection
            Return [Global].GetCollection (Me.OrganizationId, Me.OrganizationUnitId, TypeName)
        End Function

        Public Function PersonifyGetCollection (ByVal [NameSpace] As NamespaceEnum, ByVal TypeName As String) _
            As IBusinessObjectCollection
            Return [Global].GetCollection (Me.OrganizationId, Me.OrganizationUnitId, [NameSpace], TypeName)
        End Function


        ' Properties
        Public ReadOnly Property OrganizationId As String
            Get
                Return Me._OrgId
            End Get
        End Property

        Public ReadOnly Property OrganizationUnitId As String
            Get
                Return Me._OrgUnitId
            End Get
        End Property


        ' Fields
        Private _OrgId As String
        Private _OrgUnitId As String
    End Class
End Namespace

