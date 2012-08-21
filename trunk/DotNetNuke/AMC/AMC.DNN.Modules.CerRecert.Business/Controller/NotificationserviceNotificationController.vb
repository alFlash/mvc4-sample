Imports AMC.DNN.Modules.CertRecert.Data.Entities
Imports TIMSS.API.NotificationServiceInfo
Imports TIMSS.API.Core.Validation

Namespace Controller
    Partial Class AmcCertRecertController

#Region "Notification Services"

        ''' <summary>
        ''' Gets the notifications.
        ''' </summary>
        ''' <param name="organizationId">The organization id.</param>
        ''' <param name="organizationUnitId">The organization unit id.</param>
        ''' <returns></returns>
        Public Function GetNotifications (ByVal organizationId As String, ByVal organizationUnitId As String) _
            As INotificationserviceNotifications
            Return _personifyDataProvider.GetNotifications (organizationId, organizationUnitId)
        End Function

        Public Function UpdateNotifications (ByVal organizationId As String, ByVal organizationUnitId As String,
                                             ByVal notificationSettings As List(Of NotificationInfo)) _
            As IIssuesCollection
            Return _
                _personifyDataProvider.UpdateNotifications (organizationId, organizationUnitId,
                                                            notificationSettings)
        End Function

        Public Function GetNotificationByStoreProcedureName(ByVal organizationId As String, ByVal organizationUnitId As String, ByVal procedureName As String) _
            As INotificationserviceNotifications
            Return _personifyDataProvider.GetNotificationByStoreProcedureName(organizationId, organizationUnitId, procedureName)
        End Function

        Public Function UpdateNotificationByStorecedureName(ByVal organizationId As String, ByVal organizationUnitId As String,
                                             ByVal notificationSettings As List(Of NotificationInfo), ByVal procedureName As String) _
            As IIssuesCollection
            Return _
                _personifyDataProvider.UpdateNotificationByStorecedureName(organizationId, organizationUnitId,
                                                            notificationSettings, procedureName)
        End Function

#End Region
    End Class
End Namespace
