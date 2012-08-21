Imports AMC.DNN.Modules.CertRecert.Data
Imports TIMSS.API.User.UserDefinedInfo
Imports TIMSS.API.Core.Validation

Namespace Controller
    Partial Class AmcCertRecertController

#Region "Customer Survey"

        Public Function GetSurveyByTitle(ByVal surveyTitle As String) As IUserDefinedSurvey
            Dim surveyReturn As IUserDefinedSurvey = Nothing

            surveyReturn = _personifyDataProvider.GetSurveyByTitle(surveyTitle)
            Return surveyReturn
        End Function

        Public Function GetSurveyQuestionByQuestionId(ByVal surveyId As Long, ByVal questionId As Long) _
            As UserDefinedSurveyQuestion
            Return _personifyDataProvider.GetSurveyQuestionByQuestionId(surveyId, questionId)
        End Function

        Public Function GetQuestionsBySurveyId(ByVal surveyId As Long) As UserDefinedSurveyQuestions
            Return _personifyDataProvider.GetSurveyQuestionsBySurveyId(surveyId)
        End Function

        Public Function GetCustomerSurveyResponees(ByVal surveyId As Long,
                                                    ByVal masterCustomerId As String,
                                                    ByVal subcustomerId As Integer) _
            As UserDefinedCustomerSurveyResponsees
            Return _personifyDataProvider.GetCustomerSurveyResponees(surveyId,
                                                                      masterCustomerId,
                                                                      subcustomerId)
        End Function

        Public Function InsertCustomerSurveyResponse(ByRef customerSurveyResponseInput As  _
                                                         IUserDefinedCustomerSurveyResponses) As IIssuesCollection
            Return _personifyDataProvider.InsertCustomerSurveyResponse(customerSurveyResponseInput)
        End Function

        Public Function UpdateCustomerSurveyResponse(ByRef customerSurveyResponseInput As  _
                                                         IUserDefinedCustomerSurveyResponses) As IssuesCollection
            Dim issuesCollection As IssuesCollection = Nothing

            issuesCollection =
                _personifyDataProvider.UpdateCustomerSurveyResponse(customerSurveyResponseInput)

            Return issuesCollection
        End Function

        Public Function GetCustomerSurveyResponse(ByVal guid As String, ByVal surveyId As Long,
                                                   ByVal masterCustomerId As String, ByVal subCustomerId As Integer) _
            As UserDefinedCustomerSurveyResponses
            Return _personifyDataProvider.GetCustomerSurveyResponseByGuid(guid,
                                                                           surveyId,
                                                                           masterCustomerId,
                                                                           subCustomerId)
        End Function

        Public Function GetCustomerSurveyResponseByQuestionId(ByVal questionId As Long, ByVal surveyId As Long,
                                                               ByVal masterCustomerId As String,
                                                               ByVal subCustomerId As Integer) _
            As UserDefinedCustomerSurveyResponses
            Return _
                _personifyDataProvider.GetCustomerSurveyResponseByQuestionId(questionId,
                                                                             surveyId,
                                                                             masterCustomerId,
                                                                             subCustomerId)
        End Function

        Public Function GetCustomerSurveyResponseByReponseId(ByVal responseId As Long,
                                                              ByVal surveyId As Long,
                                                              ByVal masterCustomerId As String,
                                                              ByVal subCustomerId As Integer) _
            As UserDefinedCustomerSurveyResponses
            Return _
                _personifyDataProvider.GetCustomerSurveyResponseByResponseId(responseId, surveyId, masterCustomerId,
                                                                              subCustomerId)
        End Function


        Public Function DeleteCustomerSurveyResponse(ByVal customerSurveyResponseInput As  _
                                                         IUserDefinedCustomerSurveyResponses) As IIssuesCollection
            If customerSurveyResponseInput IsNot Nothing Then
                Return _personifyDataProvider.DeleteCustomerSurveyResponse(customerSurveyResponseInput)
            End If
            Return Nothing
        End Function

        Public Function CommitCustomerSurveyResponsees(ByVal surveyId As Long, ByVal masterCustomerId As String,
                                                        ByVal subcustomerId As Integer) As IIssuesCollection
            Return _personifyDataProvider.CommitCustomerSurveyResponsees(surveyId,
                                                                          masterCustomerId,
                                                                          subcustomerId)
        End Function
        Public Sub RefreshCustomerSurveyResponsees(ByVal surveyId As Long,
                                               ByVal masterCustomerId As String,
                                               ByVal subcustomerId As Integer)
            _personifyDataProvider.RefreshCustomerSurveyResponsees(surveyId,
                                                                   masterCustomerId,
                                                                   subcustomerId)
        End Sub
#Region "Survey"

#Region "Get"

        Public Function GetSurveys(ByVal orgId As String, ByVal orgUnitId As String, ByVal masterCustomer As String, Optional ByVal withCache As Boolean = True) _
            As IUserDefinedSurveys
            Return _personifyDataProvider.GetSurveys(orgId, orgUnitId, masterCustomer, withCache)
        End Function

#End Region

#Region "Insert"

        ''' <summary>
        ''' Builds the surveys and questions.
        ''' </summary>
        ''' <param name="userDefinedSurveys">The user defined surveys.</param>
        Public Sub BuildSurveysAndQuestions(ByRef userDefinedSurveys As IUserDefinedSurveys, ByVal orgId As String,
                                             ByVal orgUnitId As String, ByVal masterCustomerId As String)
            _personifyDataProvider.BuildSurveysAndQuestions(userDefinedSurveys, orgId, orgUnitId,
                                                             masterCustomerId)
        End Sub

#End Region

#Region "Delete"

        ''' <summary>
        ''' Deletes all surveys.
        ''' </summary>
        ''' <param name="userDefinedSurveys">The user defined surveys.</param>
        Public Sub DeleteAllSurveys(ByRef userDefinedSurveys As IUserDefinedSurveys)
            _personifyDataProvider.DeleteAllSurveys(userDefinedSurveys)
        End Sub

#End Region

#End Region

#End Region
        
        Public Function AddQuestion(ByRef surveys As IUserDefinedSurveys, ByRef userDefinedSurvey As IUserDefinedSurvey,
                                     ByVal questionText As String, ByVal questionType As String, ByVal isEnabled As Boolean) _
            As IIssuesCollection
            Return _personifyDataProvider.AddQuestion(surveys, userDefinedSurvey, questionText, questionType, isEnabled)
        End Function

        Public Function AddYesNoAnswer(ByRef surveys As IUserDefinedSurveys,
                                        ByRef userDefinedSurveyQuestion As IUserDefinedSurveyQuestion,
                                        ByVal questionType As String) As IIssuesCollection
            Return _personifyDataProvider.AddYesNoAnswer(surveys, userDefinedSurveyQuestion)
        End Function

        Public Function AddMultiAnswer(ByRef surveys As IUserDefinedSurveys,
                                        ByRef userDefinedSurvey As IUserDefinedSurvey, ByVal questionId As Long,
                                        ByVal answerText As String, ByVal questionType As String) As IIssuesCollection
            Return _
                _personifyDataProvider.AddMultiAnswer(surveys, userDefinedSurvey, questionId, answerText)
        End Function

        Public Function UpdateMultiAnswerText(ByRef surveys As IUserDefinedSurveys,
                                               ByRef userDefinedSurvey As IUserDefinedSurvey, ByVal questionId As Long,
                                               ByVal answerId As Long, ByVal answerText As String) As IIssuesCollection
            Return _
                _personifyDataProvider.UpdateMultiAnswerText(surveys, userDefinedSurvey, questionId, answerId,
                                                              answerText)
        End Function

        Public Shared Function GetSurveyByTitle(ByVal surveyTitle As String, ByVal organizationId As String,
                                                 ByVal organizationUnitId As String) As IUserDefinedSurvey
            Dim surveys = PersonifyDataProvider.GetSurveysByTitle(surveyTitle, organizationId, organizationUnitId)
            If surveys IsNot Nothing Then
                Return surveys(0)
            End If
            Return Nothing
        End Function

        Public Function AddResponse(ByVal organizationId As String, ByVal organizationUnitId As String,
                                     ByVal masterCustomerId As String,
                                     ByVal subCustomerId As Integer, ByVal certificationId As Integer,
                                     ByVal surveyId As Long,
                                     ByVal questionId As Long, ByVal answerId As Long, ByVal questionText As String) As IIssuesCollection
            Return _
                _personifyDataProvider.AddResponse(organizationId, organizationUnitId, masterCustomerId,
                                                    subCustomerId,
                                                    certificationId, surveyId, questionId, answerId, questionText)
        End Function

        Public Sub DeleteResponses(ByVal organizationId As String, ByVal organizationUnitId As String,
                                    ByVal masterCustomerId As String, ByVal subCustomerId As Integer,
                                    ByVal certificationId As Integer, ByVal surveyTitle As String)

            _personifyDataProvider.DeleteResponses(organizationId, organizationUnitId, masterCustomerId,
                                                    subCustomerId, certificationId, surveyTitle)
        End Sub

        Public Function AddMultiAnswer(ByVal organizationId As String, ByVal organizationUnitId As String, ByVal userDefinedSurveys As IUserDefinedSurveys,
                                       ByVal userDefinedSurvey As IUserDefinedSurvey, ByVal questionId As Long, ByVal startDate As Date,
                                       ByVal endDate As Date, ByVal applicationDeadline As Date, ByVal productCode As String,
                                       ByVal applicationProductId As String) As IIssuesCollection
            Return _
                _personifyDataProvider.AddMultiAnswer(organizationId, organizationUnitId, userDefinedSurveys, userDefinedSurvey, questionId,
                                                      startDate, endDate, applicationDeadline, productCode, applicationProductId)
        End Function

        Public Function UpdateMultiAnswer(ByVal organizationId As String, ByVal organizationUnitId As String, ByVal userDefinedSurveys As IUserDefinedSurveys,
                                          ByVal userDefinedSurvey As IUserDefinedSurvey, ByVal questionId As Long, ByVal answerId As Long, ByVal startDate As Date,
                                          ByVal endDate As Date, ByVal applicationDeadline As Date, ByVal productCode As String,
                                          ByVal applicationProductId As String) As IIssuesCollection
            Return _
               _personifyDataProvider.UpdateMultiAnswer(organizationId, organizationUnitId, userDefinedSurveys, userDefinedSurvey, questionId, answerId,
                                                             startDate, endDate, applicationDeadline, productCode, applicationProductId)
        End Function

        Public Function AddYesNoQuestion(ByVal userDefinedSurveys As IUserDefinedSurveys, ByVal userDefinedSurvey As IUserDefinedSurvey, ByVal questionText As String, ByVal isEnabled As Boolean) As IIssuesCollection
            Return _personifyDataProvider.AddYesNoQuestion(userDefinedSurveys, CType(userDefinedSurvey, UserDefinedSurvey), questionText, isEnabled)
        End Function

        Public Function AddRangeQuestion(ByVal userDefinedSurveys As IUserDefinedSurveys, ByVal userDefinedSurvey As IUserDefinedSurvey, ByVal questionText As String, ByVal isEnabled As Boolean) As IIssuesCollection
            Return _personifyDataProvider.AddRangeQuestion(userDefinedSurveys, CType(userDefinedSurvey, UserDefinedSurvey), questionText, isEnabled)
        End Function

        Public Function GetCustomerSurveyResponses(ByVal orgId As String, ByVal orgUnitId As String, ByRef userDefinedSurvey As IUserDefinedSurvey, ByVal questionId As Long) As IUserDefinedCustomerSurveyResponsees
            Return _personifyDataProvider.GetCustomerSurveyResponses(orgId, orgUnitId, userDefinedSurvey, questionId)
        End Function

        Public Function GetCustomerSurveyResponsesByAnswerId(ByVal orgId As String, ByVal orgUnitId As String, ByRef userDefinedSurvey As IUserDefinedSurvey, ByVal questionId As Long, ByVal answerId As Long) As IUserDefinedCustomerSurveyResponsees
            Return _personifyDataProvider.GetCustomerSurveyResponsesByAnswerId(orgId, orgUnitId, userDefinedSurvey, questionId, answerId)
        End Function

        Public Function GetQuestionById(ByVal organizationId As String, ByVal organizationUnitId As String, ByVal questionId As String) As UserDefinedSurveyQuestion
            Return _personifyDataProvider.GetQuestionById(organizationId, organizationUnitId, questionId)
        End Function

        Public Function UpdateCustomerSurveyResponse(ByVal surveyId As String, ByVal questionId As String, ByVal answerId As String,
                                                     ByVal responseId As String, ByVal comments As String, ByVal masterCustomerId As String,
                                                     ByVal subCustomerId As Integer, ByVal organizationId As String,
                                                     ByVal organizationUnitId As String, ByVal certificationId As Integer) As IIssuesCollection
            Return _personifyDataProvider.UpdateCustomerSurveyResponse(surveyId, questionId, answerId, responseId, comments,
                                                                       masterCustomerId, subCustomerId, organizationId,
                                                                       organizationUnitId, certificationId)
        End Function

        Public Function GetResponseByQuestionId(ByVal questionId As Long) As UserDefinedCustomerSurveyResponsees
            Return _personifyDataProvider.GetResponseByQuestionId(questionId)
        End Function

        Public Function GetAnswerByQuestionId(ByVal questionId As Long) As IUserDefinedSurveyAnsweres
            Return _personifyDataProvider.GetAnswerByQuestionId(questionId)
        End Function

        Public Function AddOrUpdateRecertOptionResponse(ByVal questionId As String, ByVal answerId As Long, ByVal comments As String, ByVal surveyTitle As String) As IIssuesCollection
            Return _personifyDataProvider.AddOrUpdateRecertOptionResponse(questionId, answerId, comments, surveyTitle)
        End Function

        Public Function AddOrUpdateResponse(ByVal questionId As String, ByVal answerId As String, ByVal comments As String, ByVal surveyTitle As String) As IIssuesCollection
            Return _personifyDataProvider.AddOrUpdateResponse(questionId, answerId, comments, surveyTitle)
        End Function

        Public Function GetResponses(ByVal questionId As Long, ByVal answerId As Long) As IUserDefinedCustomerSurveyResponsees
            Return _personifyDataProvider.GetResponses(questionId, answerId)
        End Function
    End Class
End Namespace
