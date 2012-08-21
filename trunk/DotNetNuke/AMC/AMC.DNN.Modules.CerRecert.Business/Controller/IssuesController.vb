Imports TIMSS.API.CertificationInfo
Imports TIMSS.API.CustomerInfo
Imports TIMSS.API.Core.Validation
Imports TIMSS.API.User.UserDefinedInfo

Namespace Controller
    Partial Class AmcCertRecertController
        Public Function UpdateCustomerInfo(ByVal orgId As String, ByVal orgUnitId As String,
                                            ByVal masterCustomerId As String, ByVal subCustomerId As Integer,
                                            ByVal degree As String,
                                            ByVal telephone As String,
                                            ByVal commTypeCodeString As String,
                                            ByVal commLocationCodeString As String,
                                            ByVal dateBirth As DateTime,
                                            ByVal messageLeft As String,
                                            ByVal receiveMeterial As Integer,
                                            ByVal WorkPhone As String,
                                            ByVal commLocationCodeStringWork As String) As IIssuesCollection
            Dim issuesObject As IIssuesCollection = Nothing

            issuesObject = _personifyDataProvider.UpdateCustomerInfo(orgId, orgUnitId, masterCustomerId,
                                                                      subCustomerId,
                                                                      degree, telephone, commTypeCodeString,
                                                                      commLocationCodeString,
                                                                      dateBirth, messageLeft,
                                                                      receiveMeterial, WorkPhone, commLocationCodeStringWork)

            Return issuesObject
        End Function

        Public Function SubmitCustomerInfo(ByVal orgId As String, ByVal orgUnitId As String,
                                            ByVal masterCustomerId As String, ByVal subCustomerId As Integer) _
            As IIssuesCollection
            Dim issuesObject As IIssuesCollection = Nothing

            issuesObject = _personifyDataProvider.SubmitCustomerInfo(orgId, orgUnitId, masterCustomerId,
                                                                      subCustomerId)

            Return issuesObject
        End Function

        Public Function CommitCustomerInfo(ByVal customerInfo As ICustomer, ByVal supervisor As ICustomer,
                                            ByVal newGuid As String,
                                            ByVal relatedMasterCustomerId As String,
                                            ByVal relatedSubCustomerId As Integer) As IIssuesCollection
            If String.IsNullOrEmpty(newGuid) Then 'Insert
                Return _
                    _personifyDataProvider.InsertSupervisor(customerInfo, supervisor, relatedMasterCustomerId,
                                                             relatedSubCustomerId)
            Else 'Update
                Return _personifyDataProvider.UpdateSupervisor(customerInfo, supervisor)
            End If
        End Function

        Public Function CommitCertificationCustomerCertifications(
                                                                   ByVal iCertificationCustomerCertifications As  _
                                                                      ICertificationCustomerCertifications) _
            As IIssuesCollection
            Dim issueCollection As IIssuesCollection = Nothing

            issueCollection =
                _personifyDataProvider.CommitCertificationCustomerCertifications(
                    iCertificationCustomerCertifications)

            Return issueCollection
        End Function

        Public Function CreateSurveyByTitle(ByVal surveyTitle As String, ByVal orgId As String,
                                             ByVal orgUnitId As String, ByVal masterCustomerId As String) _
            As IIssuesCollection
            Return _personifyDataProvider.CreateSurveyByTitle(surveyTitle, orgId, orgUnitId, masterCustomerId)
        End Function


        Public Function UpdateQuestion(ByRef surveys As IUserDefinedSurveys,
                                        ByRef userDefinedSurvey As IUserDefinedSurvey,
                                        ByVal userDefinedSurveyQuestion As IUserDefinedSurveyQuestion) _
            As IIssuesCollection
            Return _personifyDataProvider.UpdateQuestion(surveys, userDefinedSurvey, userDefinedSurveyQuestion)
        End Function

        Public Function UpdateQuestion(ByRef surveys As IUserDefinedSurveys,
                                        ByRef userDefinedSurvey As IUserDefinedSurvey,
                                        ByVal questionId As String, ByVal isEnabled As Boolean) _
            As IIssuesCollection
            Return _personifyDataProvider.UpdateQuestion(surveys, userDefinedSurvey, questionId, isEnabled)
        End Function

        Public Function DeleteQuestion(ByRef surveys As IUserDefinedSurveys,
                                        ByRef userDefinedSurvey As IUserDefinedSurvey, ByVal questionId As Long) _
            As IIssuesCollection
            Return _personifyDataProvider.DeleteQuestion(surveys, userDefinedSurvey, questionId)
        End Function


        Public Function DeleteAnswer(ByRef surveys As IUserDefinedSurveys,
                                      ByRef userDefinedSurvey As IUserDefinedSurvey, ByVal questionId As Long,
                                      ByVal answerId As Long) As IIssuesCollection
            Return _personifyDataProvider.DeleteAnswer(surveys, userDefinedSurvey, questionId, answerId)
        End Function
    End Class
End Namespace