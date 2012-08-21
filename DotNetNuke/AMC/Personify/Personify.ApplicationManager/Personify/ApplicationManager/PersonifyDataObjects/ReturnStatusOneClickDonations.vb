Imports System
Imports TIMSS.API.Core.Validation
Imports TIMSS.API.CustomerInfo
Imports TIMSS.API.OrderInfo

Namespace Personify.ApplicationManager.PersonifyDataObjects
    Public Class ReturnStatusOneClickDonations
        ' Fields
        Public DonationSuccessFull As Boolean
        Public oCustomers As ICustomers
        Public oFARIssues As IssuesCollection
        Public oOrderMasters As IOrderMasters
    End Class
End Namespace

