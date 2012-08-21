Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Collections
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Data
Imports TIMSS
Imports TIMSS.API.OrderInfo
Imports TIMSS.API.ReportingInfo
Imports TIMSS.Common
Imports TIMSS.Enumerations
Imports TIMSS.ThirdPartyInterfaces

Namespace Personify.ApplicationManager
    Public Class Reporting
        ' Methods
        Public Sub New(ByVal OrgId As String, ByVal OrgUnitId As String)
            Me._OrgId = OrgId
            Me._OrgUnitId = OrgUnitId
            Me.SetUserData(Conversions.ToString(0))
        End Sub

        Public Function GenerateReportURL(ByVal PortalId As Integer, ByVal ReportNameWithPath As String, ByVal ReportInstance As Boolean, ByRef Status As String, ByRef ReportKind As String, ByRef LastRunDateTime As String) As String
            Dim str2 As String = ""
            str2 = BusinessObjectsInterface.GenerateReportURL(Me.OrganizationId, Me.OrganizationUnitId, ReportNameWithPath, ReportInstance, Status, ReportKind, LastRunDateTime)
            If ((str2.Length = 0) AndAlso (Status.Length > 0)) Then
                Throw New Exception(Status)
            End If
            If Not String.IsNullOrEmpty(str2) Then
                str2 = (str2 & "&cmd=EXPORT&EXPORT_FMT=U2FPDF:0&sOutputFormat=P")
            End If
            Return str2
        End Function

        Public Function GenerateWebiReportURL(ByVal PortalId As Integer, ByVal ReportName As String, ByVal ReportId As String, ByRef Status As String) As String
            Dim str2 As String = ""
            str2 = BusinessObjectsInterface.GenerateWebiReportURL(Me.OrganizationId, Me.OrganizationUnitId, ReportName, ReportId, Status)
            If Not String.IsNullOrEmpty(str2) Then
                str2 = (str2 & "&cmd=EXPORT&EXPORT_FMT=U2FPDF:0&sOutputFormat=P")
            End If
            Return str2
        End Function

        Public Function GetOrderAcknowledgementURL(ByVal PortalId As Integer, ByVal OrderNumber As String) As String
            Dim masters As IOrderMasters = DirectCast(TIMSS.Global.GetCollection(Me.OrganizationId, Me.OrganizationUnitId, NamespaceEnum.OrderInfo, "OrderMasters"), IOrderMasters)
            masters.Fill(OrderNumber)
            Dim str2 As String = masters.PrintACKorINV("ACKNOWLEDGEMENT", "ACK", "ORD001", OrderNumber, "", Nothing, 0, DateTime.MinValue, "", "")
            If Not String.IsNullOrEmpty(str2) Then
                str2 = (str2 & "&cmd=EXPORT&EXPORT_FMT=U2FPDF:0&sOutputFormat=P")
            End If
            Return str2
        End Function

        Public Function GetOrderInvoiceURL(ByVal PortalId As Integer, ByVal OrderNumber As String) As String
            Dim masters As IOrderMasters = DirectCast(TIMSS.Global.GetCollection(Me.OrganizationId, Me.OrganizationUnitId, NamespaceEnum.OrderInfo, "OrderMasters"), IOrderMasters)
            masters.Fill(OrderNumber)
            Dim str2 As String = masters.PrintACKorINV("INVOICE", "INV", "ORD001", OrderNumber, "", Nothing, 0, DateTime.MinValue, "", "")
            If Not String.IsNullOrEmpty(str2) Then
                str2 = (str2 & "&cmd=EXPORT&EXPORT_FMT=U2FPDF:0&sOutputFormat=P")
            End If
            Return str2
        End Function

        Public Function GetReleaseURL(ByVal blnProxy As Boolean) As String
            If blnProxy Then
                Return BusinessObjectsInterface.GetReleaseSessionURL(Me.OrganizationId, Me.OrganizationUnitId, "proxy")
            End If
            Return BusinessObjectsInterface.GetReleaseSessionURL(Me.OrganizationId, Me.OrganizationUnitId, "user")
        End Function

        Public Function GetReportParameterList(ByVal PortalId As Integer, ByVal ReportName As String) As NameValueCollection
            Dim enumerator As IEnumerator
            Dim values2 As New NameValueCollection
            Dim tRSParameters As IReportingApplicationParameters = Me.GetTRSParameters(PortalId, ReportName)
            Try 
                enumerator = tRSParameters.GetEnumerator
                Do While enumerator.MoveNext
                    Dim current As IReportingApplicationParameter = DirectCast(enumerator.Current, IReportingApplicationParameter)
                    values2.Add(current.ParameterName, "")
                Loop
            Finally
                If TypeOf enumerator Is IDisposable Then
                    TryCast(enumerator,IDisposable).Dispose
                End If
            End Try
            Return values2
        End Function

        Public Function GetReportURL(ByVal PortalId As Integer, ByVal ReportName As String, ByRef Status As String, ByVal ParamArray ReportParameterValues As String()) As String
            Dim reportParameters As New NameValueCollection
            Dim tRSParameters As IReportingApplicationParameters = Me.GetTRSParameters(PortalId, ReportName)
            If (tRSParameters.Count = ReportParameterValues.Length) Then
                Dim num2 As Integer = (tRSParameters.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num2)
                    reportParameters.Add(tRSParameters.Item(i).ParameterName, ReportParameterValues(i))
                    i += 1
                Loop
                Return Me.GetReportURL(PortalId, ReportName, reportParameters, Status)
            End If
            Status = "The Report Parameters supplied do not match the actual parameters"
            Return ""
        End Function

        Public Function GetReportURL(ByVal PortalId As Integer, ByVal ReportName As String, ByVal ReportParameters As NameValueCollection, ByRef Status As String) As String
            Dim str2 As String = ""
            str2 = BusinessObjectsInterface.OnlineReportGenerateURL(Me.OrganizationId, Me.OrganizationUnitId, ReportName, ReportParameters, Status)
            If Not String.IsNullOrEmpty(str2) Then
                str2 = (str2 & "&cmd=EXPORT&EXPORT_FMT=U2FPDF:0&sOutputFormat=P")
            End If
            Return str2
        End Function

        Public Function GetTRSParameters(ByVal PortalId As Integer, ByVal ReportName As String) As IReportingApplicationParameters
            Dim key As String = (Convert.ToString(PortalId) & ReportName)
            If (PersonifyDataCache.Fetch(key) Is Nothing) Then
                Dim cacheObject As IReportingApplicationParameters = DirectCast(TIMSS.Global.GetCollection(Me.OrganizationId, Me.OrganizationUnitId, NamespaceEnum.ReportingInfo, "ReportingApplicationParameters"), IReportingApplicationParameters)
                cacheObject.Filter.Add("Application", ReportName)
                cacheObject.Sort("DisplayOrder", ListSortDirection.Ascending)
                cacheObject.Fill
                PersonifyDataCache.Store(key, cacheObject)
            End If
            Return DirectCast(PersonifyDataCache.Fetch(key), IReportingApplicationParameters)
        End Function

        Public Function GetwebiReportsList(ByVal PortalId As Integer, ByVal FolderName As String, ByVal FolderId As String, ByVal UserLoggedIn As Boolean, ByRef Status As String) As DataSet
            'Dim folderName As String = "" hung changed
            Dim set1 As DataSet = Nothing

            If (folderName.Length > 0) Then
                Return BusinessObjectsInterface.GetAllFolderandWEBIList(Me.OrganizationId, Me.OrganizationUnitId, folderName, FolderId, Status)
            End If
            If (FolderName.Length > 0) Then
                set1 = BusinessObjectsInterface.GetAllFolderandWEBIList(Me.OrganizationId, Me.OrganizationUnitId, FolderName, FolderId, Status)
            End If
            Return set1
        End Function

        Private Sub SetUserData(ByVal PortalID As String)
            TIMSS.Global.Personas.Item(Me.OrganizationId, Me.OrganizationUnitId).SetUserData(Encryption.Decrypt(PersonifySiteSettings.GetSiteSettings(Conversions.ToInteger(PortalID)).UserData))
        End Sub


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

