Imports Microsoft.ApplicationBlocks.Data
Imports System
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Runtime.InteropServices

Namespace Personify.WebUtility
    <Serializable> _
    Public Class ContextManager
        ' Methods
        Private Shared Function GetConnectionString() As String
            Return ConfigurationManager.ConnectionStrings.Item("SiteSqlServer").ConnectionString
        End Function

        Public Shared Function GetContextValues(ByRef OrgId As String, ByRef OrgUnitId As String, ByRef Language As String, ByRef BaseCurrency As String, ByRef Currency As String, ByRef Country As String, ByVal Optional PortalId As Integer = -1) As Boolean
            Dim flag As Boolean
            If (ConfigurationManager.AppSettings.Item("RunMode") = "WEB") Then
                Dim reader As SqlDataReader = SqlHelper.ExecuteReader(ContextManager.GetConnectionString, "dbo.GetTIMSSSiteSettings", New Object() { PortalId })
                Do While reader.Read
                    OrgId = reader.Item("OrganizationId").ToString
                    OrgUnitId = reader.Item("OrganizationUnitId").ToString
                    Language = reader.Item("Language").ToString
                    BaseCurrency = reader.Item("BaseCurrency").ToString
                    Currency = reader.Item("Currency").ToString
                    Country = reader.Item("Country").ToString
                Loop
                reader.Close
                reader = Nothing
                Return flag
            End If
            OrgId = ConfigurationManager.AppSettings.Item("OrganizationId")
            OrgUnitId = ConfigurationManager.AppSettings.Item("OrganizationUnitId")
            Language = ConfigurationManager.AppSettings.Item("Language")
            BaseCurrency = ConfigurationManager.AppSettings.Item("BaseCurrency")
            Currency = ConfigurationManager.AppSettings.Item("Currency")
            Country = ConfigurationManager.AppSettings.Item("Country")
            Return flag
        End Function

        Public Shared Sub GetCurrentOrganizationInfo(ByRef OrgId As String, ByRef OrgUnitId As String)
            Dim baseCurrency As String = Nothing
            Dim country As String = Nothing
            Dim currency As String = Nothing
            Dim language As String = Nothing
            ContextManager.GetContextValues(OrgId, OrgUnitId, language, baseCurrency, currency, country, -1)
        End Sub

    End Class
End Namespace

