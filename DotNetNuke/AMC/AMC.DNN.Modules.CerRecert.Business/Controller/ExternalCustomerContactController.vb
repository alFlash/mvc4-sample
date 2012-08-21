Imports TIMSS.API.User.UserDefinedInfo
Imports TIMSS.API.Core.Validation

Namespace Controller
    Partial Class AmcCertRecertController

#Region "Customer External Contacts"

        Public Function GetCustomerExternalContacts (ByVal type As String, ByVal masterCustomerId As String,
                                                     ByVal subCustomerId As Integer) _
            As IUserDefinedCustomerExternalContacts
            Dim customerExternalContractsReturn As IUserDefinedCustomerExternalContacts = Nothing
            customerExternalContractsReturn = _personifyDataProvider.GetCustomerExternalContacts (type,
                                                                                                  masterCustomerId,
                                                                                                  subCustomerId)
            Return customerExternalContractsReturn
        End Function
        Public Sub RefreshCustomerExternalContacts(ByVal type As String,
                                                    ByVal masterCustomerId As String,
                                                    ByVal subCustomerId As Integer)
            _personifyDataProvider.RefreshCustomerExternalContacts(type, masterCustomerId, subCustomerId)
        End Sub

        Public Function DeleteCustomerExternalContact (
                                                       ByVal iCustomerExternelContactInput As _
                                                          IUserDefinedCustomerExternalContact) As IIssuesCollection
            Dim iissuesCollection As IIssuesCollection = Nothing
            If iCustomerExternelContactInput IsNot Nothing Then
                iissuesCollection =
                _personifyDataProvider.DeleteCustomerExternalContact(iCustomerExternelContactInput)
            End If
            Return iissuesCollection
        End Function

        Public Function GetCustomerExternalContactByGuid (ByVal guid As String, ByVal type As String,
                                                          ByVal masterCustomerId As String,
                                                          ByVal subCustomerId As Integer) _
            As IUserDefinedCustomerExternalContact
            Dim iCustomerExternalContractData As IUserDefinedCustomerExternalContact = Nothing

            iCustomerExternalContractData = _personifyDataProvider.GetCustomerExternalContactByGuid (guid,
                                                                                                     type,
                                                                                                     masterCustomerId,
                                                                                                     subCustomerId)
            Return iCustomerExternalContractData
        End Function

        Public Function InsertCustomerExternalContact(
                                                       ByRef iCustomerExternalContactInput As  _
                                                          UserDefinedCustomerExternalContact) As IIssuesCollection
            Dim iIssuesCollection As IIssuesCollection = Nothing

            iIssuesCollection = _personifyDataProvider.InsertCustomerExternalContact(iCustomerExternalContactInput)

            Return iIssuesCollection
        End Function

        Public Function UpdateCustomerExternalContact (
                                                       ByVal iCustomerExternelContactInput As _
                                                          IUserDefinedCustomerExternalContact) As IIssuesCollection
            Dim iIssuesCollection As IIssuesCollection = Nothing

            iIssuesCollection = _personifyDataProvider.UpdateCustomerExternalContact (iCustomerExternelContactInput)

            Return iIssuesCollection
        End Function

        Public Function CommitCustomerExternalContacts (ByVal type As String,
                                                        ByVal masterCustomerId As String,
                                                        ByVal subCustomerId As Integer) As IIssuesCollection
            Dim iIssuesCollection As IIssuesCollection = Nothing

            iIssuesCollection = _personifyDataProvider.CommitCustomerExternalContacts (type,
                                                                                       masterCustomerId,
                                                                                       subCustomerId)
            Return iIssuesCollection
        End Function

#End Region
    End Class
End Namespace
