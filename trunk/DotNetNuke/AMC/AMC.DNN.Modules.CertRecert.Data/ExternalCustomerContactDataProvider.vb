Imports AMC.DNN.Modules.CertRecert.Data.Enums
Imports AMC.DNN.Modules.CertRecert.Data.Exception
Imports DotNetNuke.Services.Localization
Imports TIMSS.API.Core
Imports TIMSS
Imports TIMSS.Enumerations
Imports TIMSS.API.User.UserDefinedInfo
Imports TIMSS.API.Core.Validation
Imports System.Web

Partial Class PersonifyDataProvider
    Inherits DataProvider

#Region "Customer external contacts"

    Public Function GetCustomerExternalContacts (ByVal type As String, ByVal masterCustomerId As String,
                                                 ByVal subCustomerId As Integer) As IUserDefinedCustomerExternalContacts
        Dim customerExternalContacts As IUserDefinedCustomerExternalContacts = Nothing
        Try
            customerExternalContacts = CType(GetObjectFromCache(_organizationId,
                                                                _organizationUnitId,
                String.Format("{0}_{1}", CustomerObjectEnum.CONTRACT.ToString(), type),
                                                                Me._certificationId.ToString(),
                                                                masterCustomerId,
                                                                subCustomerId), 
                                              IUserDefinedCustomerExternalContacts)
            If customerExternalContacts Is Nothing Then
                customerExternalContacts = CType([Global].GetCollection(_organizationId, _organizationUnitId,
                                                                          NamespaceEnum.UserDefinedInfo,
                                                                          "UserDefinedCustomerExternalContacts"), 
                                                  IUserDefinedCustomerExternalContacts)
                With customerExternalContacts.Filter
                    .Add("CertificationId", QueryOperatorEnum.Equals, Me._certificationId.ToString())
                    .Add("ContactType", QueryOperatorEnum.Equals, type)
                    .Add("RelatedMasterCustomerId", QueryOperatorEnum.Equals, masterCustomerId)
                    .Add("RelatedSubCustomerId", QueryOperatorEnum.Equals, subCustomerId.ToString())
                End With
                customerExternalContacts.Fill()

                StoreCacheObject(_organizationId,
                                  _organizationUnitId,
                                  String.Format("{0}_{1}", CustomerObjectEnum.CONTRACT.ToString(), type),
                                  Me._certificationId.ToString(),
                                  masterCustomerId,
                                  subCustomerId,
                                  customerExternalContacts)
            End If

        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return customerExternalContacts
    End Function

    Public Function GetCustomerExternalContactByGuid(ByVal guid As String, ByVal type As String,
                                                      ByVal masterCustomerId As String, ByVal subCustomerId As Integer) _
        As IUserDefinedCustomerExternalContact
        Dim customerExternalContact As IUserDefinedCustomerExternalContact = Nothing
        Dim customerExternalContacts As IUserDefinedCustomerExternalContacts
        Try
            customerExternalContacts = GetCustomerExternalContacts(type, masterCustomerId, subCustomerId)
            customerExternalContact = CType(customerExternalContacts.FindObject("Guid", guid), 
                                             IUserDefinedCustomerExternalContact)
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return customerExternalContact
    End Function
    
    Public Function InsertCustomerExternalContact(ByRef iCustomerExternalContactInput As  _
                                                      UserDefinedCustomerExternalContact) As IIssuesCollection
        Dim customerExternalContactItem As UserDefinedCustomerExternalContact
        Dim iCustomerExternalContacts As IUserDefinedCustomerExternalContacts = Nothing
        Dim issuesCollection As New IssuesCollection
        Try
            iCustomerExternalContactInput.Validate()
            If iCustomerExternalContactInput.ValidationIssuesForMe.Count > 0 Then
                Return iCustomerExternalContactInput.ValidationIssuesForMe
            End If
            iCustomerExternalContacts = GetCustomerExternalContacts(iCustomerExternalContactInput.ContactTypeString,
                                                                     iCustomerExternalContactInput.
                                                                        RelatedMasterCustomerId,
                                                                     iCustomerExternalContactInput.RelatedSubcustomerId)
            customerExternalContactItem = CType(iCustomerExternalContacts.CreateNew(), 
                                                      UserDefinedCustomerExternalContact)
            With customerExternalContactItem
                .CertificationId = Me._certificationId
                .IsNewObjectFlag = True
                .ContactTypeString = iCustomerExternalContactInput.ContactTypeString
                .LastName = iCustomerExternalContactInput.LastName
                .RelatedMasterCustomerId = iCustomerExternalContactInput.RelatedMasterCustomerId
                .RelatedSubcustomerId = iCustomerExternalContactInput.RelatedSubcustomerId
                .PrefContactMethodString = iCustomerExternalContactInput.PrefContactMethodString
                .ContactClassTypeString = iCustomerExternalContactInput.ContactClassTypeString
                .Employer = iCustomerExternalContactInput.Employer
                .Degree = iCustomerExternalContactInput.Degree
                .MiddleName = iCustomerExternalContactInput.MiddleName
                .LabelName = iCustomerExternalContactInput.LabelName
                .NamePrefix = iCustomerExternalContactInput.NamePrefix
                .NameSuffix = iCustomerExternalContactInput.NameSuffix
                .FirstName = iCustomerExternalContactInput.FirstName
                .JobTitle = iCustomerExternalContactInput.JobTitle
                .Notify = iCustomerExternalContactInput.Notify
            End With

            For Each customerAddressItem As IUserDefinedCustomerExternalAddress In _
                iCustomerExternalContactInput.UserDefinedCustomerExternalAddresses
                If (customerAddressItem IsNot Nothing) Then
                    Dim customerAddressItemData =
                            customerExternalContactItem.UserDefinedCustomerExternalAddresses.CreateNew()
                    With customerAddressItemData
                        .IsNewObjectFlag = True
                        .Address1 = customerAddressItem.Address1
                        .Address2 = customerAddressItem.Address2
                        .Address3 = customerAddressItem.Address3
                        .Address4 = customerAddressItem.Address4
                        .City = customerAddressItem.City
                        .PostalCode = customerAddressItem.PostalCode
                        .State = customerAddressItem.State
                        .AddressTypeString = customerAddressItem.AddressTypeString()
                    End With
                    customerExternalContactItem.UserDefinedCustomerExternalAddresses.Add(customerAddressItemData)
                End If
            Next

            For Each customerCommunicationItem As IUserDefinedCustomerExternalCommunications In _
                iCustomerExternalContactInput.UserDefinedCustomerExternalCommunicationes
                If (customerCommunicationItem IsNot Nothing) Then
                    Dim customerExternalCommunicationItemData =
                            customerExternalContactItem.UserDefinedCustomerExternalCommunicationes.CreateNew()
                    With customerExternalCommunicationItemData
                        .IsNewObjectFlag = True
                        .CommunicationLocationString = customerCommunicationItem.CommunicationLocationString
                        .CommunicationTypeString = customerCommunicationItem.CommunicationTypeString
                        .PhoneNumber = customerCommunicationItem.PhoneNumber
                        .FormattedPhoneAddress = customerCommunicationItem.FormattedPhoneAddress
                    End With
                    customerExternalContactItem.UserDefinedCustomerExternalCommunicationes.Add(
                        customerExternalCommunicationItemData)
                End If
            Next

            iCustomerExternalContacts.Add(customerExternalContactItem)
            customerExternalContactItem.Validate()
            For Each issue As IIssue In customerExternalContactItem.ValidationIssuesForMe
                issuesCollection.Add(issue)
            Next
            If issuesCollection.Count > 0 Then
                iCustomerExternalContacts.Remove(customerExternalContactItem)
            Else
                iCustomerExternalContactInput = customerExternalContactItem
            End If
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return issuesCollection
    End Function

    Public Function UpdateCustomerExternalContact(ByVal iCustomerExternelContactInput As  _
                                                      IUserDefinedCustomerExternalContact) As IIssuesCollection
        Dim issueCollections As New IssuesCollection
        Try
            If iCustomerExternelContactInput IsNot Nothing Then
                iCustomerExternelContactInput.Validate()
                If iCustomerExternelContactInput.ValidationIssuesForMe.Count > 0 Then
                    Return iCustomerExternelContactInput.ValidationIssuesForMe
                End If
            End If
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return issueCollections
    End Function

    Public Function DeleteCustomerExternalContact(
                                                   ByVal iCustomerExternelContactInput As  _
                                                      IUserDefinedCustomerExternalContact) As IIssuesCollection
        Dim customerExternalContractData As UserDefinedCustomerExternalContact = Nothing
        Dim customerExternalContracts As IUserDefinedCustomerExternalContacts = Nothing
        Try
            customerExternalContracts =
                GetCustomerExternalContacts(iCustomerExternelContactInput.ContactTypeString,
                                             iCustomerExternelContactInput.RelatedMasterCustomerId,
                                             iCustomerExternelContactInput.RelatedSubcustomerId)
            customerExternalContractData =
                CType(customerExternalContracts.FindObject("Guid",
                                                             iCustomerExternelContactInput.Guid), 
                       UserDefinedCustomerExternalContact)
            customerExternalContracts.Remove(customerExternalContractData)
            customerExternalContracts.Validate()
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return customerExternalContracts.ValidationIssues
    End Function

    Public Function CommitCustomerExternalContacts(ByVal type As String,
                                                    ByVal masterCustomerId As String,
                                                    ByVal subCustomerId As Integer) As IIssuesCollection
        Dim customerExternalContacts As UserDefinedCustomerExternalContacts = Nothing
        Dim issuesCollection As IIssuesCollection
        Try
            customerExternalContacts = CType(GetCustomerExternalContacts(type,
                                                                         masterCustomerId,
                                                                         subCustomerId), 
                                              UserDefinedCustomerExternalContacts)
            customerExternalContacts.Save()
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return customerExternalContacts.ValidationIssues
    End Function
    Public Sub RefreshCustomerExternalContacts(ByVal type As String,
                                                    ByVal masterCustomerId As String,
                                                    ByVal subCustomerId As Integer)
        RemoveCacheObject(Me._organizationId, Me._organizationUnitId,
                          String.Format("{0}_{1}", CustomerObjectEnum.CONTRACT.ToString(), type),
                          Me._certificationId.ToString(), masterCustomerId, subCustomerId)
    End Sub
#End Region
End Class
