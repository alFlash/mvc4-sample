Imports TIMSS.API.User.ApplicationInfo
Imports TIMSS.API.User.UserDefinedInfo
Imports TIMSS.API.Core.Validation

Namespace Controller
    Partial Class AmcCertRecertController

#Region "CE Activities"

        Public Function GetCETypesByProgramType(ByVal programType As String) As ApplicationCodes
            Return _personifyDataProvider.GetCETypesByProgramType(programType)
        End Function

        Public Function GetExternalActivityList (ByVal type As String, ByVal masterCustomerId As String,
                                                 ByVal subCustomerId As Integer) As IUserDefinedCustomerCEActivities
            Dim listObject As IUserDefinedCustomerCEActivities = Nothing

            listObject = _personifyDataProvider.GetExternalActivityList (type, masterCustomerId, subCustomerId)

            Return listObject
        End Function

        Public Function GetCustomerExternalCEActivityByGuiId (ByVal guid As String, ByVal type As String,
                                                              ByVal masterCustomerId As String,
                                                              ByVal subCustomerId As Integer) _
            As IUserDefinedCustomerCEActivity
            Dim objectItem As IUserDefinedCustomerCEActivity

            objectItem = _personifyDataProvider.GetCustomerExternalCEActivityByGuiId (guid, type, masterCustomerId,
                                                                                      subCustomerId)

            Return objectItem
        End Function

        Public Function UpdateCustomerCEACtivity(
                                                  ByRef customerExternalCEActivitytItem As  _
                                                     IUserDefinedCustomerCEActivity) As IIssuesCollection
            Dim issuesObject As IIssuesCollection = Nothing

            issuesObject = _personifyDataProvider.UpdateCustomerExternalCEActivity(customerExternalCEActivitytItem)

            Return issuesObject
        End Function

        Public Function InsertCustomerExternalCEActivity(
                                                          ByRef customerExternalCEActivitytItem As  _
                                                             IUserDefinedCustomerCEActivity) As IIssuesCollection
            Dim issuesObject As IIssuesCollection = Nothing

            issuesObject = _personifyDataProvider.InsertCustomerExternalCEACtivity(customerExternalCEActivitytItem)

            Return issuesObject
        End Function

        Public Function CommitCustomerExternalCEActivity (ByVal type As String,
                                                          ByVal masterCustomerId As String,
                                                          ByVal subCustomerId As Integer) As IIssuesCollection
            Dim issuesObject As IIssuesCollection = Nothing

            issuesObject = _personifyDataProvider.CommitCustomerExternalCEActivity (type, masterCustomerId,
                                                                                    subCustomerId)

            Return issuesObject
        End Function
        Public Sub RefreshCustomerExternalActivity(ByVal type As String,
                                              ByVal masterCustomerId As String,
                                              ByVal subCustomerId As Integer)
            _personifyDataProvider.RefreshCustomerExternalActivity(type, masterCustomerId, subCustomerId)
        End Sub
        Public Function DeleteCustomerExternalCEActivity (
                                                          ByVal customerExternalCEActivityItem As _
                                                             IUserDefinedCustomerCEActivity) As IIssuesCollection
            Dim issuesObject As IIssuesCollection = Nothing
            If customerExternalCEActivityItem IsNot Nothing Then
                issuesObject = _personifyDataProvider.DeleteCustomerExternalCEActivity(customerExternalCEActivityItem)
            End If
            Return issuesObject
        End Function

        Public Function GetCETotal (ByVal masterCustomerId As String, ByVal subCustomerId As Integer,
                                    ByVal type As String, ByVal certificationId As Integer) As Decimal
            Dim totalCE As Decimal = 0

            totalCE = _personifyDataProvider.GetCETotal (masterCustomerId, subCustomerId, type, certificationId)

            Return totalCE
        End Function

#End Region
    End Class
End Namespace
