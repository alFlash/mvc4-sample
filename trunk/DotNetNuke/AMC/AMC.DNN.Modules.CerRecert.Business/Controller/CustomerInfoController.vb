Imports TIMSS.API.CustomerInfo

Namespace Controller
    Partial Class AmcCertRecertController

#Region "customer"

        Public Function GetCustomerList(ByVal orgId As String, ByVal orgUnitId As String,
                                         ByVal masterCustomerId As String,
                                         ByVal subCustomerId As Integer) As ICustomers
            Dim listObject As ICustomers = Nothing

            listObject = _personifyDataProvider.GetCustomerList(orgId, orgUnitId, masterCustomerId, subCustomerId)

            Return listObject
        End Function

        Public Function GetCustomerInfoItem(ByVal orgId As String, ByVal orgUnitId As String,
                                             ByVal masterCustomerId As String,
                                             ByVal subCustomerId As Integer) As ICustomer
            Dim objectItem As ICustomer

            objectItem = _personifyDataProvider.GetCustomerInfoItem(orgId, orgUnitId, masterCustomerId,
                                                                     subCustomerId)

            Return objectItem
        End Function


#End Region

    End Class
End Namespace

