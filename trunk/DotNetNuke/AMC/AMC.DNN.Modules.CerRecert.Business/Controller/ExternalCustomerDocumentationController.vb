Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports AMC.DNN.Modules.CertRecert.Business.BusinessValidation
Imports TIMSS.API.User.UserDefinedInfo
Imports TIMSS.API.Core.Validation

Namespace Controller
    Partial Class AmcCertRecertController

#Region "Customer External Documents"
        Public Function CheckBusinessValidationForExternalDocumentation(ByRef externalDocumentation As IUserDefinedCustomerExternalDocumentation) As Boolean
            Dim returnValue As Boolean = True
            Select Case externalDocumentation.DocumentationTypeString
                Case DocumentationType.LICENSURE.ToString()
                    If externalDocumentation IsNot Nothing Then
                        Dim expirationDate =
                            ModuleConfigurationHelper.Instance.GetOtherModuleSettings(_modulePath).LicensureValidityDate
                        If expirationDate <> DateTime.MinValue Then
                            ExpirationDateIssue.Assert(externalDocumentation.CycleEndDate <
                                                       expirationDate,
                                                       externalDocumentation,
                                                       expirationDate)
                        End If
                        If externalDocumentation.ValidationIssues.Count > 0 Then
                            For Each issue As IIssue In externalDocumentation.ValidationIssues
                                If TypeOf issue Is UploadFileIssue Then
                                    returnValue = False
                                    Exit For
                                End If
                            Next
                        End If
                    End If
            End Select
            Return returnValue
        End Function
        Public Sub RefreshCustomerExternalDocuments(ByVal type As String,
                                                    ByVal masterCustomerId As String,
                                                    ByVal subCustomerId As Integer)
            _personifyDataProvider.RefreshCustomerExternalDocuments(type,
                                                                    masterCustomerId,
                                                                    subCustomerId)
        End Sub
        Public Function GetCustomerExternalDocuments(ByVal type As String,
                                                     ByVal masterCustomerId As String,
                                                     ByVal subCustomerId As Integer) _
            As IUserDefinedCustomerExternalDocumentations
            Dim iUserDefinedCustomerExternalDocumentations As IUserDefinedCustomerExternalDocumentations = Nothing

            iUserDefinedCustomerExternalDocumentations =
                _personifyDataProvider.GetCustomerExternalDocuments(type, masterCustomerId, subCustomerId)

            Return iUserDefinedCustomerExternalDocumentations
        End Function

        Public Function InsertCustomerExternalDocument(ByRef customerExternalDocumentItem As  _
                                                           IUserDefinedCustomerExternalDocumentation) _
            As IIssuesCollection
            Dim iissuesCollection As IIssuesCollection = Nothing
            If customerExternalDocumentItem Is Nothing Then
                Return iissuesCollection
            End If
            If CheckBusinessValidationForExternalDocumentation(customerExternalDocumentItem) Then
                iissuesCollection = _personifyDataProvider.InsertCustomerExternalDocument(customerExternalDocumentItem)
            Else
                iissuesCollection = customerExternalDocumentItem.ValidationIssues
            End If
            Return iissuesCollection
        End Function

        Public Function UpdateCustomerExternalDocument(ByVal customerExternalDocumentItem As  _
                                                           IUserDefinedCustomerExternalDocumentation) _
            As IIssuesCollection
            Dim iissuesCollection As IIssuesCollection = Nothing
            If customerExternalDocumentItem Is Nothing Then
                Return iissuesCollection
            End If
            If CheckBusinessValidationForExternalDocumentation(customerExternalDocumentItem) Then
                iissuesCollection = _personifyDataProvider.UpdateCustomerExternalDocument(customerExternalDocumentItem)
            Else
                iissuesCollection = customerExternalDocumentItem.ValidationIssues
            End If
            Return iissuesCollection
        End Function

        Public Function DeleteCustomerExternalDocument(ByVal customerExternalDocumentItem As  _
                                                           IUserDefinedCustomerExternalDocumentation) _
            As IIssuesCollection
            Dim iissuesCollection As IIssuesCollection = Nothing
            If customerExternalDocumentItem IsNot Nothing Then
                iissuesCollection =
                _personifyDataProvider.DeleteCustomerExternalDocument(customerExternalDocumentItem)
            End If
            Return iissuesCollection
        End Function

        Public Function GetCustomerExternalDocumentationByGUID(ByVal guid As String, ByVal type As String,
                                                                ByVal masterCustomerId As String,
                                                                ByVal subCustomerId As Integer) _
            As IUserDefinedCustomerExternalDocumentation
            Dim iUserDefinedCustomerExternalDocumentation As IUserDefinedCustomerExternalDocumentation = Nothing

            iUserDefinedCustomerExternalDocumentation =
                _personifyDataProvider.GetCustomerExternalDocumentationByGUID(guid,
                                                                               type,
                                                                               masterCustomerId,
                                                                               subCustomerId)

            Return iUserDefinedCustomerExternalDocumentation
        End Function

        Public Function CommitCustomerExternalDocuments(ByVal type As String, ByVal masterCustomerId As String,
                                                         ByVal subCustomerId As Integer) As IIssuesCollection
            Dim iissuesCollection As IIssuesCollection = Nothing
            
            iissuesCollection =
                _personifyDataProvider.CommitCustomerExternalDocuments(type, masterCustomerId, subCustomerId)

            Return iissuesCollection
        End Function

#End Region
    End Class
End Namespace
