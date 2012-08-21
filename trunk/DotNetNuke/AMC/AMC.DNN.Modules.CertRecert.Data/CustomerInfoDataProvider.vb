Imports AMC.DNN.Modules.CertRecert.Data.Exception
Imports TIMSS.API.OrderInfo
Imports TIMSS.API.Generated.CustomerInfo
Imports TIMSS
Imports TIMSS.Enumerations
Imports TIMSS.API.CustomerInfo
Imports TIMSS.API.Core.Validation

Partial Class PersonifyDataProvider
    Inherits DataProvider


#Region "CustomerInfo"

    ''' <summary>
    ''' Gets the customer info.
    ''' </summary>
    ''' <param name="orgId">The org id.</param>
    ''' <param name="orgUnitId">The org unit id.</param>
    ''' <param name="masterCustomerId">The master customer id.</param>
    ''' <param name="subCustomerId">The sub customer id.</param>
    ''' <returns></returns>

    Public Function GetCustomerInfoItem(ByVal orgId As String, ByVal orgUnitId As String, ByVal masterCustomerId As String, ByVal subCustomerId As Integer) As ICustomer
        Try
            Dim customers = CType([Global].GetCollection(orgId, orgUnitId, NamespaceEnum.CustomerInfo, "Customers"), 
                              ICustomers)
            customers.Filter.Add("MasterCustomerId", masterCustomerId)
            customers.Filter.Add("SubCustomerId", subCustomerId.ToString())
            customers.Filter.Add("OrganizationId", orgId)
            customers.Filter.Add("OrganizationUnitId", orgUnitId)
            customers.Fill()

            Return If(customers IsNot Nothing AndAlso customers.Count > 0, customers(0), Nothing)

        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return Nothing
    End Function

    Public Function GetCustomerList(ByVal orgId As String, ByVal orgUnitId As String, ByVal masterCustomerId As String,
                                     ByVal subCustomerId As Integer) As ICustomers
        Dim customers = CType([Global].GetCollection(orgId, orgUnitId, NamespaceEnum.CustomerInfo, "Customers"), 
                               ICustomers)
        customers.Filter.Add("MasterCustomerId", masterCustomerId)
        customers.Filter.Add("SubCustomerId", subCustomerId.ToString())
        customers.Filter.Add("OrganizationId", orgId)
        customers.Filter.Add("OrganizationUnitId", orgUnitId)
        customers.Fill()
        Return customers
    End Function

    Public Function UpdateCustomerInfo(ByVal orgId As String,
                                       ByVal orgUnitId As String,
                                       ByVal masterCustomerId As String,
                                       ByVal subCustomerId As Integer,
                                       ByVal degree As String,
                                       ByVal telephone As String,
                                       ByVal commTypeCodeString As String,
                                       ByVal commLocationCodeString As String,
                                       ByVal dateBirth As DateTime,
                                       ByVal messageLeft As String,
                                       ByVal receiveMeterial As Integer,
                                       ByVal WorkPhone As String,
                                       ByVal commLocationCodeStringWork As String) As IIssuesCollection
        Dim customerList = GetCustomerList(orgId, orgUnitId, masterCustomerId, subCustomerId)
        If customerList IsNot Nothing AndAlso customerList.Count >= 1 Then
            Dim customerInfo = customerList(0)
            customerInfo.BirthDate = dateBirth 'TODO: 
            customerInfo.NameCredentials = degree
            CType(customerInfo, Customer).AddressDetails(0).PrioritySeq = receiveMeterial  '' only have 1 values => alway set values for the first Item of customerAddressDetails object (index : 0)
            ''set phone in communication object
            '' If Not String.IsNullOrEmpty(telephone) Then
            'Dim isHaveHomePhone = False '' don't have
            'Dim isHaveWorkPhone = False '' don't have
            'For Each customercommunication As CustomerCommunication In customerInfo.Communications
            '    If customercommunication.CommLocationCodeString = commLocationCodeString _
            '                        AndAlso customercommunication.CommTypeCodeString = commTypeCodeString Then  '' HOME
            '        customercommunication.FormattedPhoneAddress = If(String.IsNullOrEmpty(telephone), "", telephone) 'TODO:
            '        If customercommunication.SearchPhoneAddress = String.Empty Then
            '            customercommunication.SearchPhoneAddress = If(String.IsNullOrEmpty(telephone), "", telephone) 'TODO:
            '        End If
            '        isHaveHomePhone = True
            '    End If
            '    If customercommunication.CommLocationCodeString = commLocationCodeStringWork _
            '                        AndAlso customercommunication.CommTypeCodeString = commTypeCodeString Then   '' OFFICE
            '        customercommunication.FormattedPhoneAddress = If(String.IsNullOrEmpty(telephone), "", WorkPhone) 'TODO:
            '        If customercommunication.SearchPhoneAddress = String.Empty Then
            '            customercommunication.SearchPhoneAddress = If(String.IsNullOrEmpty(telephone), "", WorkPhone) 'TODO:
            '        End If
            '        isHaveWorkPhone = True
            '    End If
            'Next
            'If Not isHaveHomePhone Then  '' add Home phone
            '    Dim customerHomePhoneItemData = customerInfo.Communications.AddNew()
            '    With customerHomePhoneItemData
            '        .MasterCustomerId = masterCustomerId
            '        .SubCustomerId = subCustomerId
            '        .PrimaryFlag = False
            '        .IsNewObjectFlag = True
            '        .CommLocationCodeString = commLocationCodeString
            '        .CommTypeCode = .CommTypeCode.List(commTypeCodeString).ToCodeObject()
            '        .CommTypeCodeString = commTypeCodeString
            '        .FormattedPhoneAddress = If(String.IsNullOrEmpty(telephone), "", telephone)
            '        If .SearchPhoneAddress = String.Empty Then
            '            .SearchPhoneAddress = If(String.IsNullOrEmpty(telephone), "", telephone)
            '        End If
            '    End With
            '    '' customerInfo.Communications.Add(customerHomePhoneItemData)
            'End If
            'If Not isHaveWorkPhone Then   '' add work Phone
            '    Dim customerWorkPhoneItemData = customerInfo.Communications.AddNew()
            '    With customerWorkPhoneItemData
            '        .MasterCustomerId = masterCustomerId
            '        .SubCustomerId = subCustomerId
            '        .PrimaryFlag = False
            '        .IsNewObjectFlag = True
            '        .CommLocationCodeString = commLocationCodeStringWork
            '        .CommTypeCode = .CommTypeCode.List(commTypeCodeString).ToCodeObject()
            '        .CommTypeCodeString = commTypeCodeString
            '        .FormattedPhoneAddress = If(String.IsNullOrEmpty(telephone), "", WorkPhone)
            '        If .SearchPhoneAddress = String.Empty Then
            '            .SearchPhoneAddress = If(String.IsNullOrEmpty(telephone), "", WorkPhone)
            '        End If
            '    End With
            '    '' customerInfo.Communications.Add(customerHomePhoneItemData)
            'End If
            ''End If
            customerList.Validate()
            customerList.Save()
        End If
        Return customerList.ValidationIssues
    End Function

    Public Function SubmitCustomerInfo(ByVal orgId As String, ByVal orgUnitId As String,
                                       ByVal masterCustomerId As String, ByVal subCustomerId As Integer) As IIssuesCollection
        Dim customerList As ICustomers = Nothing
        Try
            customerList = GetCustomerList(orgId, orgUnitId, masterCustomerId, subCustomerId)
            customerList.Save()
        Catch ex As System.Exception
            ''log
            Throw
        End Try
        Return customerList.ValidationIssues
    End Function



#End Region

#Region "Payment"
    Public Function GetOrderMaster(ByVal orgId As String, ByVal orgUnitId As String, _
                              ByVal orderNumber As String) As IOrderMasters
        Dim orders = CType([Global].GetCollection(orgId, orgUnitId, NamespaceEnum.OrderInfo, "OrderMasters"), 
                             IOrderMasters)
        orders.Filter.Add("OrderNumber", orderNumber)
        orders.Fill()
        Return orders
    End Function

    Public Function GetOrderDetail(ByVal orgId As String, ByVal orgUnitId As String, ByVal orderNumber As String,
                                   ByVal productId As Integer) As IOrderDetails
        Dim orders = CType([Global].GetCollection(orgId, orgUnitId, NamespaceEnum.OrderInfo, "OrderDetails"), 
                             IOrderDetails)
        orders.Filter.Add("OrderNumber", orderNumber)
        orders.Filter.Add("ProductId", productId.ToString())
        orders.Fill()
        Return orders
    End Function
#End Region

End Class
