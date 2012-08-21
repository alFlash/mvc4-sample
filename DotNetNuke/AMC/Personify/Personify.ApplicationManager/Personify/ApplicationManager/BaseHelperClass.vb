Imports System
Imports System.Web
Imports TIMSS

Namespace Personify.ApplicationManager
    Public Class BaseHelperClass
        ' Methods
        Public Sub New(ByVal OrgId As String, ByVal OrgUnitId As String)
            Me._OrgId = OrgId
            Me._OrgUnitId = OrgUnitId
            If Not PersonifyInitializer.SystemWasInitialized Then
                PersonifyInitializer.Initialize(HttpContext.Current.Request.PhysicalApplicationPath, PersonifySiteSettings.GetSeatInformation)
            End If
        End Sub

        Public Sub New(ByVal OrgId As String, ByVal OrgUnitId As String, ByVal EnableOnDemandDataLoad As Boolean)
            Me._OrgId = OrgId
            Me._OrgUnitId = OrgUnitId
            If Not PersonifyInitializer.SystemWasInitialized Then
                PersonifyInitializer.Initialize(HttpContext.Current.Request.PhysicalApplicationPath, PersonifySiteSettings.GetSeatInformation)
            End If
            Me._EnableOnDemandDataLoad = EnableOnDemandDataLoad
        End Sub


        ' Properties
        Public ReadOnly Property EnableOnDemandDataLoad As Boolean
            Get
                Return Me._EnableOnDemandDataLoad
            End Get
        End Property

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
        Private _EnableOnDemandDataLoad As Boolean
        Private _OrgId As String
        Private _OrgUnitId As String
    End Class
End Namespace

