Imports AMC.DNN.Modules.CertRecert.Data.Exception
Imports AMC.DNN.Modules.CertRecert.Data.Enums
Imports TIMSS
Imports TIMSS.Enumerations
Imports TIMSS.API.User.UserDefinedInfo
Imports TIMSS.API.Core.Validation

Partial Class PersonifyDataProvider
    Inherits DataProvider

#Region "External Customer Documentation"

    Public Function GetCustomerExternalDocuments (ByVal documentationType As String, ByVal masterCustomerId As String,
                                                  ByVal subCustomerId As Integer) _
        As IUserDefinedCustomerExternalDocumentations
        Dim documentCustomerExternals As IUserDefinedCustomerExternalDocumentations

        Try
            documentCustomerExternals = CType(GetObjectFromCache(_organizationId, _organizationUnitId, documentationType,
                                                                   Me._certificationId.ToString(), masterCustomerId,
                                                                   subCustomerId), 
                                               IUserDefinedCustomerExternalDocumentations)

            If documentCustomerExternals Is Nothing Then
                documentCustomerExternals = CType([Global].GetCollection(_organizationId, _organizationUnitId,
                                                                           NamespaceEnum.UserDefinedInfo,
                                                                           "UserDefinedCustomerExternalDocumentations"), 
                                                   IUserDefinedCustomerExternalDocumentations)

                With documentCustomerExternals.Filter
                    .Add("CertificationId", QueryOperatorEnum.Equals, Me._certificationId.ToString())
                    .Add("DocumentationType", QueryOperatorEnum.Equals, documentationType)
                    .Add("RelatedMasterCustomerId", QueryOperatorEnum.Equals, masterCustomerId)
                    .Add("RelatedSubCustomerId", QueryOperatorEnum.Equals, subCustomerId.ToString())
                End With
                documentCustomerExternals.Fill()
                StoreCacheObject(_organizationId, _organizationUnitId, documentationType, Me._certificationId.ToString(),
                                  masterCustomerId, subCustomerId, documentCustomerExternals)
            End If

         Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try

        Return documentCustomerExternals
    End Function

    Public Function InsertCustomerExternalDocument(ByRef customerExternalDocumentItem As  _
                                                       IUserDefinedCustomerExternalDocumentation) As IIssuesCollection

        Dim customerExternalDocuments As IUserDefinedCustomerExternalDocumentations = Nothing
        Dim customerExternalDocumentItemData As IUserDefinedCustomerExternalDocumentation
        Dim issueCollection As New IssuesCollection
        Try
            customerExternalDocumentItem.Validate()
            If customerExternalDocumentItem.ValidationIssuesForMe.Count > 0 Then
                Return customerExternalDocumentItem.ValidationIssuesForMe
            End If
            '' Get list cache object
            customerExternalDocuments =
                GetCustomerExternalDocuments(customerExternalDocumentItem.DocumentationTypeString,
                                             customerExternalDocumentItem.RelatedMasterCustomerId,
                                             customerExternalDocumentItem.RelatedSubcustomerId)
            customerExternalDocumentItemData = customerExternalDocuments.CreateNew()
            ''set properties for object data from business object
            ''SynchronizeObject(customerExternalDocumentItem, customerExternalDocumentItemData)
            With customerExternalDocumentItemData
                .IsNewObjectFlag = True
                .CertificationId = Me._certificationId
                .DocumentationType = customerExternalDocumentItem.DocumentationType
                .RelatedMasterCustomerId = customerExternalDocumentItem.RelatedMasterCustomerId
                .RelatedSubcustomerId = customerExternalDocumentItem.RelatedSubcustomerId
                If .IssuingBody.List Is Nothing Then
                    .IssuingBody.FillList()
                End If
                .IssuedNumber = customerExternalDocumentItem.IssuedNumber
                .IssuingBody = customerExternalDocumentItem.IssuingBody
                .IssuingBodyText = customerExternalDocumentItem.IssuingBodyText
                .InitialIssueDate = customerExternalDocumentItem.InitialIssueDate
                .ChangedBy = customerExternalDocumentItem.ChangedBy
                .ChangedOn = customerExternalDocumentItem.ChangedOn
                .Comments = customerExternalDocumentItem.Comments
                .CycleBeginDate = customerExternalDocumentItem.CycleBeginDate
                .CycleEndDate = customerExternalDocumentItem.CycleEndDate
                .ConcurrencyId = customerExternalDocumentItem.ConcurrencyId
                .DocumentTitle = customerExternalDocumentItem.DocumentTitle
                .DocumentLocation = customerExternalDocumentItem.DocumentLocation
            End With

            ''Add object
            customerExternalDocuments.Add(customerExternalDocumentItemData)
            ''validate object
            customerExternalDocuments.Validate()

            If customerExternalDocumentItemData.ValidationIssuesForMe IsNot Nothing Then
                For Each issue As IIssue In customerExternalDocumentItemData.ValidationIssuesForMe
                    issueCollection.Add(issue)
                Next
            End If
            If issueCollection.Count > 0 Then
                customerExternalDocuments.Remove(customerExternalDocumentItemData)
            Else
                customerExternalDocumentItem = customerExternalDocumentItemData
            End If
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        ''return issues
        Return issueCollection
    End Function


    Public Function GetCustomerExternalDocumentationByGUID(ByVal guid As String, ByVal type As String,
                                                            ByVal masterCustomerId As String,
                                                            ByVal subCustomerId As Integer) _
        As IUserDefinedCustomerExternalDocumentation

        Dim customerExternalDocumentReturn As IUserDefinedCustomerExternalDocumentation = Nothing
        Dim customerExternalDocumentations As IUserDefinedCustomerExternalDocumentations

        Try
            customerExternalDocumentations =
                            GetCustomerExternalDocuments(type, masterCustomerId, subCustomerId)
            customerExternalDocumentReturn =
                CType(customerExternalDocumentations.FindObject("Guid", guid), 
                                                    IUserDefinedCustomerExternalDocumentation)
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return customerExternalDocumentReturn
    End Function

    Public Function UpdateCustomerExternalDocument(ByVal customerExternalDocumentItem As  _
                                                       IUserDefinedCustomerExternalDocumentation) As IIssuesCollection
        ''To Do:

        Dim customerExternalDocuments As IUserDefinedCustomerExternalDocumentations = Nothing
        Dim customerExternalDocumentItemData As IUserDefinedCustomerExternalDocumentation
        Try
            If customerExternalDocumentItem Is Nothing Then
                Return New IssuesCollection()
            End If
            customerExternalDocumentItem.Validate()
            If customerExternalDocumentItem.ValidationIssuesForMe.Count > 0 Then
                customerExternalDocumentItem.SourceRow.CancelEdit()
                Return customerExternalDocumentItem.ValidationIssuesForMe
            End If
            customerExternalDocuments =
                GetCustomerExternalDocuments(customerExternalDocumentItem.DocumentationTypeString,
                                              customerExternalDocumentItem.RelatedMasterCustomerId,
                                              customerExternalDocumentItem.RelatedSubcustomerId)
            customerExternalDocumentItemData =
                GetCustomerExternalDocumentationByGUID(
                                                customerExternalDocumentItem.Guid.ToString(),
                                                customerExternalDocumentItem.DocumentationTypeString,
                                                customerExternalDocumentItem.RelatedMasterCustomerId,
                                                customerExternalDocumentItem.RelatedSubcustomerId)

            customerExternalDocuments.Validate()
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return customerExternalDocumentItem.ValidationIssuesForMe
    End Function

    Public Function DeleteCustomerExternalDocument(
                                                    ByVal customerExternalDocumentItem As  _
                                                       IUserDefinedCustomerExternalDocumentation) As IIssuesCollection
        Dim customerExternalDocuments As IUserDefinedCustomerExternalDocumentations = Nothing
        Dim customerExternalDocumentItemData As IUserDefinedCustomerExternalDocumentation
        Dim issueCollectionReturn As New IssuesCollection
        Try
            If customerExternalDocumentItem Is Nothing Then
                Return issueCollectionReturn
            End If
            customerExternalDocuments =
                GetCustomerExternalDocuments(customerExternalDocumentItem.DocumentationTypeString,
                                              customerExternalDocumentItem.RelatedMasterCustomerId,
                                              customerExternalDocumentItem.RelatedSubcustomerId)
            customerExternalDocumentItemData =
                GetCustomerExternalDocumentationByGUID(
                                    customerExternalDocumentItem.Guid.ToString(),
                                    customerExternalDocumentItem.DocumentationTypeString,
                                    customerExternalDocumentItem.RelatedMasterCustomerId,
                                    customerExternalDocumentItem.RelatedSubcustomerId)
            If customerExternalDocumentItemData IsNot Nothing Then
                customerExternalDocuments.Remove(customerExternalDocumentItemData)
                customerExternalDocuments.Validate()
                issueCollectionReturn = CType(customerExternalDocuments.ValidationIssues, IssuesCollection)
            End If

        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return issueCollectionReturn
    End Function

    Public Function CommitCustomerExternalDocuments(ByVal type As String, ByVal masterCustomerId As String,
                                                     ByVal subCustomerId As Integer) As IIssuesCollection
        Dim customerExternalDocumentations As IUserDefinedCustomerExternalDocumentations = Nothing
        Try
            customerExternalDocumentations = GetCustomerExternalDocuments(type, masterCustomerId, subCustomerId)
            customerExternalDocumentations.Save()

        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return customerExternalDocumentations.ValidationIssues
    End Function
    Public Sub RefreshCustomerExternalDocuments(ByVal type As String,
                                                    ByVal masterCustomerId As String,
                                                    ByVal subCustomerId As Integer)
        RemoveCacheObject(_organizationId,
                          _organizationUnitId,
                          type,
                          Me._certificationId.ToString(),
                          masterCustomerId,
                          subCustomerId)
    End Sub
#End Region

End Class
