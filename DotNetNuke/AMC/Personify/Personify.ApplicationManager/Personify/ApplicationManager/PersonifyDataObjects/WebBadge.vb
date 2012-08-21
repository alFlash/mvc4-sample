Imports System
Imports TIMSS.API.OrderInfo

Namespace Personify.ApplicationManager.PersonifyDataObjects
    Public Class WebBadge
        ' Fields
        Public Badges As IOrderDetailBadges
        Public CycleBeginDate As DateTime
        Public CycleEndDate As DateTime
        Public MasterProductFlag As Boolean
        Public MaxBadges As Integer
        Public MeetingLineNumber As Integer
        Public OrderLineNumber As Integer
        Public ProductName As String
        Public ProductType As String
        Public TotalBadgesForMeeting As Integer
        Public TotalPaidBadgesForMeeting As Integer
    End Class
End Namespace

