
Imports AMC.DNN.Modules.CertRecert.Data.Enums
Imports AMC.DNN.Modules.CertRecert.Data.Exception
Imports TIMSS.API.CustomerInfo
Imports TIMSS.API.User.CustomerInfo
Imports TIMSS
Imports TIMSS.Enumerations
Imports TIMSS.API.User.UserDefinedInfo
Imports TIMSS.API.Core.Validation


Partial Class PersonifyDataProvider
    Inherits DataProvider

    Public Function GetPracticeScopeList(ByVal masterCustomerId As String,
                                     ByVal subCustomerId As Integer) As IUserDefinedCustomerPracticeScopes
        Dim practiceScopeList As IUserDefinedCustomerPracticeScopes
        Try
            practiceScopeList = CType(GetObjectFromCache(_organizationId,
                                                       _organizationUnitId,
                                                       CustomerObjectEnum.PRACTICESCOPE.ToString(),
                                                       Me._certificationId.ToString(),
                                                       masterCustomerId,
                                                       subCustomerId), 
                                   IUserDefinedCustomerPracticeScopes)
            If practiceScopeList Is Nothing Then
                practiceScopeList = CType([Global].GetCollection(_organizationId, _organizationUnitId,
                                                               NamespaceEnum.UserDefinedInfo, "UserDefinedCustomerPracticeScopes"), 
                                       IUserDefinedCustomerPracticeScopes)
                With practiceScopeList.Filter
                    .Add("MasterCustomerId", QueryOperatorEnum.Equals, masterCustomerId)
                    .Add("SubcustomerId", QueryOperatorEnum.Equals, subCustomerId.ToString())
                End With
                practiceScopeList.Fill()
                StoreCacheObject(_organizationId,
                                 _organizationUnitId,
                                  CustomerObjectEnum.PRACTICESCOPE.ToString(),
                                  Me._certificationId.ToString(),
                                  masterCustomerId,
                                  subCustomerId,
                                  practiceScopeList)
            End If
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return practiceScopeList
    End Function

    Public Function GetPracticeScopeByGuiID(ByVal guid As String,
                                                ByVal masterCustomerId As String,
                                                ByVal subCustomerId As Integer) As IUserDefinedCustomerPracticeScope
        Dim practiceScopeList As IUserDefinedCustomerPracticeScopes
        Try
            practiceScopeList = GetPracticeScopeList(masterCustomerId, subCustomerId)
            For Each practiceScope As UserDefinedCustomerPracticeScope In practiceScopeList
                If practiceScope.Guid = guid Then
                    Return practiceScope
                End If
            Next
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return Nothing
    End Function

    Public Function UpdatePracticeScope(ByVal practiceScopeItem As IUserDefinedCustomerPracticeScope) As IIssuesCollection
        ''To Do:
        Dim practiceScopeList As IUserDefinedCustomerPracticeScopes
        Dim practiceScopeItemData As IUserDefinedCustomerPracticeScope
        Try
            practiceScopeList = GetPracticeScopeList(practiceScopeItem.MasterCustomerId,
                                                      practiceScopeItem.SubcustomerId)
            practiceScopeItemData = GetPracticeScopeByGuiID(practiceScopeItem.Guid.ToString(),
                                                                     practiceScopeItem.MasterCustomerId,
                                                                     practiceScopeItem.SubcustomerId)
            If practiceScopeItemData IsNot Nothing Then
                SynchronizeObject(practiceScopeItem, practiceScopeItemData)
                practiceScopeList.Validate()
            End If
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return practiceScopeList.ValidationIssues
    End Function


    Public Function InsertPracticeScope(ByVal practiceScope As IUserDefinedCustomerPracticeScope) As IIssuesCollection
        Dim practiceScopeList As IUserDefinedCustomerPracticeScopes
        Dim practiceScopeItemData As IUserDefinedCustomerPracticeScope
        Dim issueCollection As New IssuesCollection
        Try
            '' Get list cache object
            practiceScopeList = GetPracticeScopeList(practiceScope.MasterCustomerId,
                                                      practiceScope.SubcustomerId)
            practiceScopeItemData = practiceScopeList.CreateNew()
            ''set properties for object data from business object
            With practiceScopeItemData
                .CertificationId = Me._certificationId
                .Comments = practiceScope.Comments
                .Guid = practiceScope.Guid
                .ScopeAvgTimePerService = practiceScope.ScopeAvgTimePerService
                .ScopeNbrServices = practiceScope.ScopeNbrServices
                .ScopeText = practiceScope.ScopeText
                .ScopeType = practiceScope.ScopeType
                .ScopeTypeString = practiceScope.ScopeTypeString
                .CptCode = practiceScope.CptCode
                .CptCodeString = practiceScope.CptCodeString
                .IsNewObjectFlag = True
                .MasterCustomerId = practiceScope.MasterCustomerId
                .SubcustomerId = practiceScope.SubcustomerId
            End With
            practiceScopeList.Add(practiceScopeItemData)
            practiceScopeList.Validate()
            '' have issue => remove item out of cache
            For Each issue As IIssue In practiceScopeList.ValidationIssues
                issueCollection.Add(issue)
            Next
            If issueCollection.Count >= 1 Then
                practiceScopeList.Remove(practiceScopeItemData)
            End If
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return issueCollection
    End Function

    Public Function CommitPracticeScope(ByVal masterCustomerId As String,
                                            ByVal subCustomerId As Integer) As IIssuesCollection
        Dim practiceScopeList As IUserDefinedCustomerPracticeScopes
        Try
            practiceScopeList = GetPracticeScopeList(masterCustomerId, subCustomerId)
            practiceScopeList.Save()
            If (practiceScopeList.ValidationIssues.Count < 1) Then
                RemoveCacheObject(_organizationId, _organizationUnitId,
                                   CustomerObjectEnum.PRACTICESCOPE.ToString(),
                                   Me._certificationId.ToString(),
                                   masterCustomerId, subCustomerId)
            End If
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return practiceScopeList.ValidationIssues
    End Function

    Public Function DeletePracticeScope(ByVal practiceScopeItem As IUserDefinedCustomerPracticeScope) As IIssuesCollection
        Dim practiceScopeList As IUserDefinedCustomerPracticeScopes
        Dim practiceScopeItemData As IUserDefinedCustomerPracticeScope
        Try
            practiceScopeList = GetPracticeScopeList(practiceScopeItem.MasterCustomerId,
                                                      practiceScopeItem.SubcustomerId)
            practiceScopeItemData = CType(
                practiceScopeList.FindObject("Guid",practiceScopeItem.Guid.ToString()), IUserDefinedCustomerPracticeScope)

            For Each practiceScope As IUserDefinedCustomerPracticeScope In practiceScopeList
                If practiceScope.Guid = practiceScopeItem.Guid.ToString() Then
                    practiceScopeItemData = practiceScope
                End If
            Next
            If practiceScopeItemData IsNot Nothing Then
                practiceScopeList.Remove(practiceScopeItemData)
            End If
            practiceScopeList.Validate()

        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return practiceScopeList.ValidationIssues
    End Function

End Class
