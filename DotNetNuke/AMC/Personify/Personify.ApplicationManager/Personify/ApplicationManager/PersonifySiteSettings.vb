Imports System.Runtime.CompilerServices
Imports Microsoft.ApplicationBlocks.Data
Imports Microsoft.VisualBasic.CompilerServices
Imports Personify.ApplicationManager.PersonifyDataObjects
Imports System
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Web
Imports TIMSS.Client.Implementation.Security.Authentication
Imports TIMSS.Common
Imports TIMSS.Security.Network

Namespace Personify.ApplicationManager
    Public Class PersonifySiteSettings
        ' Methods
        Private Shared Function GetSeatFromDB() As SeatInfo
            Dim str2 As String = ""
            Dim num As Integer = 0
            Dim information As New SeatInformation
            Dim reader As SqlDataReader = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings.Item("SiteSqlServer").ConnectionString, "dbo.GetPersonifySiteSettings", New Object() { num })
            If Not reader.HasRows Then
                Throw New ApplicationException("Unable to read Seat Information. Contact system administrator.")
            End If
            Do While reader.Read
                information.AssociationName = reader.Item("AssociationName").ToString
                information.EnvironmentName = reader.Item("EnvironmentName").ToString
                information.AppTypeName = reader.Item("AppTypeName").ToString
                information.VersionName = reader.Item("VersionName").ToString
                information.DatabaseName = reader.Item("DatabaseName").ToString
                information.Login = reader.Item("Login").ToString
                Try 
                    str2 = Encryption.Decrypt(reader.Item("Password").ToString)
                Catch exception1 As Exception
                    ProjectData.SetProjectError(exception1)
                    Dim exception As Exception = exception1
                    str2 = reader.Item("Password").ToString
                    ProjectData.ClearProjectError
                End Try
            Loop
            reader.Close
            reader = Nothing
            Dim info2 As New SeatInfo
            info2.SeatInformation = information
            info2.Password = str2
            Return info2
        End Function

        Public Shared Function GetSeatInformation() As ISeatInformationProvider
            Dim seatFromDB As SeatInfo
            Dim str As String = "PersonifySeatInformation"
            If (HttpContext.Current.Application.Item(str) Is Nothing) Then
                Dim objLock As Object = PersonifySiteSettings.objLock
                ObjectFlowControl.CheckForSyncLockOnValueType(objLock)
                SyncLock objLock
                    If (HttpContext.Current.Application.Item(str) Is Nothing) Then
                        seatFromDB = PersonifySiteSettings.GetSeatFromDB
                        HttpContext.Current.Application.Item(str) = seatFromDB
                    End If
                End SyncLock
            End If
            seatFromDB = DirectCast(HttpContext.Current.Application.Item(str), SeatInfo)
            Return New StaticSeatInformationProvider(seatFromDB.SeatInformation, seatFromDB.Password)
        End Function

        Public Shared Function GetSiteSettings(ByVal MyPortalId As Integer) As SiteSettings
            Dim key As String = ("PersonifySiteSettings" & MyPortalId.ToString)
            If (PersonifyDataCache.Fetch(key) Is Nothing) Then
                Dim settings2 As SiteSettings
                Dim reader As SqlDataReader = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings.Item("SiteSqlServer").ConnectionString, "dbo.GetPersonifySiteSettings", New Object() { MyPortalId })
                If reader.HasRows Then
                    settings2 = New SiteSettings(MyPortalId)
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
                        settings2.UserData = reader.Item("Password").ToString
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


        ' Fields
        Public Shared objLock As Object = RuntimeHelpers.GetObjectValue(New Object)

        ' Nested Types
        Private Class SeatInfo
            ' Fields
            Public Password As String
            Public SeatInformation As SeatInformation
        End Class
    End Class
End Namespace

