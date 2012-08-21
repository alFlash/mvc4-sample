Imports TIMSS.API.Core.Validation
Imports TIMSS.API.User.UserDefinedInfo

Namespace Controller
    Partial Class AmcCertRecertController

#Region "Customer Education"

        Public Function GetPracticeScopeList(ByVal masterCustomerId As String,
                                          ByVal subCustomerId As Integer) As IUserDefinedCustomerPracticeScopes
            Dim listObject As IUserDefinedCustomerPracticeScopes = Nothing
            listObject = _personifyDataProvider.GetPracticeScopeList(masterCustomerId, subCustomerId)
            Return listObject
        End Function

        Public Function GetPracticeScopeByGuiID(ByVal guid As String,
                                                     ByVal masterCustomerId As String,
                                                     ByVal subCustomerId As Integer) As IUserDefinedCustomerPracticeScope
            Dim objectItem As IUserDefinedCustomerPracticeScope
            objectItem = _personifyDataProvider.GetPracticeScopeByGuiID(guid, masterCustomerId, subCustomerId)
            Return objectItem
        End Function

        Public Function UpdatePracticeScope(ByVal Item As IUserDefinedCustomerPracticeScope) _
            As IIssuesCollection
            Dim issuesObject As IIssuesCollection = Nothing
            issuesObject = _personifyDataProvider.UpdatePracticeScope(Item)
            Return issuesObject
        End Function

        Public Function InsertPracticeScope(ByVal ObjectItem As IUserDefinedCustomerPracticeScope) _
            As IIssuesCollection
            Dim issuesObject As IIssuesCollection = Nothing
            issuesObject = _personifyDataProvider.InsertPracticeScope(ObjectItem)
            Return issuesObject
        End Function

        Public Function CommitPracticeScope(ByVal masterCustomerId As String,
                                                 ByVal subCustomerId As Integer) As IIssuesCollection
            Dim issuesObject As IIssuesCollection = Nothing
            issuesObject = _personifyDataProvider.CommitPracticeScope(masterCustomerId, subCustomerId)
            Return issuesObject
        End Function

        Public Function DeletePracticeScope(ByVal ObjectItem As IUserDefinedCustomerPracticeScope) _
            As IIssuesCollection
            Dim issuesObject As IIssuesCollection = Nothing
            If ObjectItem IsNot Nothing Then
                issuesObject = _personifyDataProvider.DeletePracticeScope(ObjectItem)
            End If
            Return issuesObject
        End Function

#End Region
    End Class
End Namespace
