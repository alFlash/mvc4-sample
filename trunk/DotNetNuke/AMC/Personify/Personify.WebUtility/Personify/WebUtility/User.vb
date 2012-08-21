Imports DotNetNuke.Entities.Profile
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Roles
Imports System
Imports System.Runtime.CompilerServices
Imports System.Web

Namespace Personify.WebUtility
    <Serializable> _
    Public Class User
        ' Methods
        Public Shared Sub ClearAnonymousUserCookie()
            HttpContext.Current.Request.Cookies.Item("AnonumousTimssCMSUser").Expires = DateTime.Now
        End Sub

        Private Shared Function GetAnonymousUserCookie() As HttpCookie
            Dim cookie As HttpCookie = Nothing
            If HttpContext.Current.Request.Browser.Cookies Then
                cookie = HttpContext.Current.Request.Cookies.Item("AnonumousTimssCMSUser")
            End If
            Return cookie
        End Function

        Public Shared Function GetAnonymousUserId(ByVal MasterCustomerId As String) As String
            Dim anonUserId As String = ""
            If ((MasterCustomerId Is Nothing) OrElse (MasterCustomerId = String.Empty)) Then
                If (User.GetAnonymousUserCookie Is Nothing) Then
                    If (Not HttpContext.Current.Session Is Nothing) Then
                        HttpContext.Current.Session.Item("ShoppingCartSession") = HttpContext.Current.Session.SessionID.ToString
                        anonUserId = HttpContext.Current.Session.Item("ShoppingCartSession").ToString
                    End If
                    Return anonUserId
                End If
                anonUserId = User.GetAnonymousUserCookie.Value
                User.SetAnonymousUserCookie(anonUserId)
                Return anonUserId
            End If
            Return MasterCustomerId
        End Function

        Public Shared Function GetCurrentCustomer(ByVal UserId As Integer, ByVal PortalId As Integer, ByVal UserInfo As ProfilePropertyDefinitionCollection) As LoginCustomer
            Dim customer2 As New LoginCustomer
            customer2.isLoggedin = False
            If AffiliateManagement.IsManagingAffiliate(PortalId) Then
                Dim currentAffiliateInfo As AffiliateManagement.AffiliateInfo = AffiliateManagement.GetCurrentAffiliateInfo(PortalId)
                If (Not currentAffiliateInfo.AffiliateCustomerId.MasterCustomerId Is Nothing) Then
                    customer2.MasterCustomerId = currentAffiliateInfo.AffiliateCustomerId.MasterCustomerId
                    customer2.SubCustomerId = currentAffiliateInfo.AffiliateCustomerId.SubCustomerId
                    customer2.isLoggedin = True
                End If
            Else
                If (((Not UserInfo.Item("MasterCustomerId") Is Nothing) AndAlso (Not UserInfo.Item("MasterCustomerId") Is Nothing)) AndAlso (Not UserInfo.Item("MasterCustomerId").PropertyValue Is Nothing)) Then
                    customer2.MasterCustomerId = Convert.ToString(UserInfo.Item("MasterCustomerId").PropertyValue)
                    customer2.isLoggedin = True
                Else
                    customer2.MasterCustomerId = User.GetAnonymousUserId("")
                End If
                If (((Not UserInfo.Item("SubCustomerId") Is Nothing) AndAlso (Not UserInfo.Item("SubCustomerId") Is Nothing)) AndAlso (Not UserInfo.Item("SubCustomerId").PropertyValue Is Nothing)) Then
                    customer2.SubCustomerId = Convert.ToInt32(UserInfo.Item("SubCustomerId").PropertyValue)
                Else
                    customer2.SubCustomerId = 0
                End If
            End If
            If (Not HttpContext.Current.Session Is Nothing) Then
                Dim rolesByUser As String() = New RoleController().GetRolesByUser(UserId, PortalId)
                If (If(((Not rolesByUser Is Nothing) AndAlso (rolesByUser.Length > 0)), 1, 0) = 0) Then
                    Return customer2
                End If
                Dim str As String
                For Each str In rolesByUser
                    If ((str = "PersonifyStaff") AndAlso ((Not Convert.ToString(RuntimeHelpers.GetObjectValue(HttpContext.Current.Session.Item("staffmcid"))) Is Nothing) AndAlso (Convert.ToString(RuntimeHelpers.GetObjectValue(HttpContext.Current.Session.Item("staffmcid"))).Length > 0))) Then
                        customer2.MasterCustomerId = Convert.ToString(RuntimeHelpers.GetObjectValue(HttpContext.Current.Session.Item("staffmcid")))
                        customer2.SubCustomerId = Convert.ToInt32(RuntimeHelpers.GetObjectValue(HttpContext.Current.Session.Item("staffscid")))
                        customer2.LabelName = Convert.ToString(RuntimeHelpers.GetObjectValue(HttpContext.Current.Session.Item("staffln")))
                        customer2.isLoggedin = True
                    End If
                Next
            End If
            Return customer2
        End Function

        Public Shared Function GetStaffCustomerLabelName(ByVal UserId As Integer, ByVal PortalId As Integer) As String
            Dim str2 As String = Nothing
            If (Not HttpContext.Current.Session Is Nothing) Then
                Dim rolesByUser As String() = New RoleController().GetRolesByUser(UserId, PortalId)
                If (If(((Not rolesByUser Is Nothing) AndAlso (rolesByUser.Length > 0)), 1, 0) = 0) Then
                    Return str2
                End If
                Dim str3 As String
                For Each str3 In rolesByUser
                    If ((str3 = "PersonifyStaff") AndAlso ((Not Convert.ToString(RuntimeHelpers.GetObjectValue(HttpContext.Current.Session.Item("staffmcid"))) Is Nothing) AndAlso (Convert.ToString(RuntimeHelpers.GetObjectValue(HttpContext.Current.Session.Item("staffmcid"))).Length > 0))) Then
                        str2 = Convert.ToString(RuntimeHelpers.GetObjectValue(HttpContext.Current.Session.Item("staffln")))
                    End If
                Next
            End If
            Return str2
        End Function

        Public Shared Function IsPersonifyUserLoggedIn(ByVal UserInfo As ProfilePropertyDefinitionCollection, ByRef MasterCustomerId As String, ByRef SubCustomerId As Integer) As Boolean
            If ((Not UserInfo.Item("MasterCustomerId") Is Nothing) AndAlso (Not UserInfo.Item("SubCustomerId") Is Nothing)) Then
                Dim propertyValue As String = UserInfo.Item("MasterCustomerId").PropertyValue
                If (propertyValue = String.Empty) Then
                    Return False
                End If
                MasterCustomerId = propertyValue
                SubCustomerId = Integer.Parse(UserInfo.Item("SubCustomerId").PropertyValue)
                Return True
            End If
            Return False
        End Function

        Public Shared Sub ParseCustomerId(ByVal customerId As String, ByRef MasterCustomerId As String, ByRef SubCustomerId As String)
            Dim strArray As String() = customerId.Split(New Char() { "|"c })
            If (strArray.Length > 1) Then
                MasterCustomerId = strArray((strArray.GetUpperBound(0) - 1))
                SubCustomerId = strArray(strArray.GetUpperBound(0))
            End If
        End Sub

        Public Shared Sub SaveStaffCustomer(ByVal lg As LoginCustomer)
            If (Not HttpContext.Current.Session Is Nothing) Then
                If (Not lg Is Nothing) Then
                    HttpContext.Current.Session.Item("staffmcid") = lg.MasterCustomerId
                    HttpContext.Current.Session.Item("staffscid") = lg.SubCustomerId
                    HttpContext.Current.Session.Item("staffln") = lg.LabelName
                Else
                    HttpContext.Current.Session.Item("staffmcid") = Nothing
                    HttpContext.Current.Session.Item("staffscid") = Nothing
                    HttpContext.Current.Session.Item("staffln") = Nothing
                End If
            End If
        End Sub

        Private Shared Sub SetAnonymousUserCookie(ByVal AnonUserId As String)
            If HttpContext.Current.Request.Browser.Cookies Then
                Dim cookie As New HttpCookie("AnonumousTimssCMSUser")
                cookie.Expires = DateTime.Now.AddDays(90)
                cookie.Value = AnonUserId
            End If
        End Sub

        Public Shared Function UserRole(ByVal UserInfo As UserInfo) As String
            Dim flag2 As Boolean = False
            If (((Not UserInfo.Profile.GetPropertyValue("MasterCustomerId") Is Nothing) AndAlso (UserInfo.Profile.GetPropertyValue("MasterCustomerId").Length > 0)) AndAlso ((Not UserInfo.Profile.GetPropertyValue("SubCustomerId") Is Nothing) AndAlso (UserInfo.Profile.GetPropertyValue("SubCustomerId").Length >= 0))) Then
                flag2 = True
            End If
            Dim str As String = "none"
            Dim flag As Boolean = False
            If UserInfo.IsSuperUser Then
                Return "host"
            End If
            Dim controller As New RoleController
            Dim str2 As String
            For Each str2 In controller.GetRolesByUser(UserInfo.UserID, UserInfo.PortalID)
                If (str2 = "Administrators") Then
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
                str = "user"
            End If
            Return str
        End Function

    End Class
End Namespace

