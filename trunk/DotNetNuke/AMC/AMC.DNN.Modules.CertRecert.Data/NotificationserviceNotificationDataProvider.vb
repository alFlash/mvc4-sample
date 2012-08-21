Imports AMC.DNN.Modules.CertRecert.Data.Entities
Imports TIMSS.API.Generated.NotificationServiceInfo
Imports TIMSS
Imports TIMSS.Enumerations
Imports TIMSS.API.NotificationServiceInfo
Imports TIMSS.API.Core.Validation

Partial Class PersonifyDataProvider
    Inherits DataProvider
#Region "Private Const"
    Private Const CERT_SELECTED_AUDIT_PROCEDURE = "usp_NOTIFY_SUBSCRIPTION_RENEWAL"
    Private Const RECERT_SELECTED_AUDIT_PROCEDURE = "usr_usp_NOTIFY_RECert_audit_selected"
#End Region
    ''' <summary>
    ''' Gets the notifications.
    ''' </summary>
    ''' <param name="orgId">The org id.</param>
    ''' <param name="orgUnitId">The org unit id.</param>
    ''' <returns></returns>
    Public Function GetNotifications(ByVal orgId As String, ByVal orgUnitId As String) _
        As INotificationserviceNotifications
        Dim notifications = CType([Global].GetCollection(orgId, orgUnitId, NamespaceEnum.NotificationServiceInfo,
                                                           "NotificationserviceNotifications"), 
                                   INotificationserviceNotifications)
        With notifications.Filter
            .Add("OrganizationId", orgId)
            .Add("OrganizationUnitId", orgUnitId)
            .Add("Subsystem", "CRT")
            .Add("SpName", QueryOperatorEnum.NotEqualTo, CERT_SELECTED_AUDIT_PROCEDURE)
            .Add("SpName", QueryOperatorEnum.NotEqualTo, RECERT_SELECTED_AUDIT_PROCEDURE)
        End With
        notifications.Fill()
        Return notifications
    End Function


    Public Function UpdateNotifications(ByVal orgId As String, ByVal orgUnitId As String,
                                         ByVal notificationSettings As List(Of NotificationInfo)) As IIssuesCollection
        Dim notifications = CType([Global].GetCollection(orgId, orgUnitId, NamespaceEnum.NotificationServiceInfo,
                                                           "NotificationserviceNotifications"), 
                                   INotificationserviceNotifications)
        With notifications.Filter
            .Add("OrganizationId", orgId)
            .Add("OrganizationUnitId", orgUnitId)
            .Add("Subsystem", "CRT")

        End With
        notifications.Fill()
        For Each notificationSetting As NotificationInfo In notificationSettings
            Dim notificationObject = notifications.FindObject("NotificationId",
                                                               Convert.ToInt64(notificationSetting.NotificationId))
            If notificationObject IsNot Nothing Then
                CType(notificationObject, NotificationserviceNotification).ActiveFlag = notificationSetting.IsEnabled
            End If
        Next
        notifications.Validate()
        notifications.Save()
        Return notifications.ValidationIssues
    End Function

    Public Function UpdateNotificationByStorecedureName(ByVal orgId As String, ByVal orgUnitId As String,
                                         ByVal notificationSettings As List(Of NotificationInfo), ByVal procedureName As String) As IIssuesCollection
        Dim notifications = CType([Global].GetCollection(orgId, orgUnitId, NamespaceEnum.NotificationServiceInfo,
                                                           "NotificationserviceNotifications"), 
                                   INotificationserviceNotifications)
        With notifications.Filter
            .Add("OrganizationId", orgId)
            .Add("OrganizationUnitId", orgUnitId)
            .Add("Subsystem", "CRT")
            .Add("SpName", procedureName)
        End With
        notifications.Fill()
        For Each notificationSetting As NotificationInfo In notificationSettings
            Dim notificationObject = notifications.FindObject("NotificationId",
                                                               Convert.ToInt64(notificationSetting.NotificationId))
            If notificationObject IsNot Nothing Then
                CType(notificationObject, NotificationserviceNotification).ActiveFlag = notificationSetting.IsEnabled
            End If
        Next
        notifications.Validate()
        notifications.Save()
        Return notifications.ValidationIssues
    End Function

    Public Function GetNotificationByStoreProcedureName(ByVal orgId As String, ByVal orgUnitId As String, ByVal procedureName As String) _
    As INotificationserviceNotifications
        Dim notifications = CType([Global].GetCollection(orgId, orgUnitId, NamespaceEnum.NotificationServiceInfo,
                                                           "NotificationserviceNotifications"), 
                                   INotificationserviceNotifications)
        With notifications.Filter
            .Add("OrganizationId", orgId)
            .Add("OrganizationUnitId", orgUnitId)
            .Add("Subsystem", "CRT")
            .Add("SpName", procedureName)
        End With
        notifications.Fill()
        Return notifications
    End Function


End Class
