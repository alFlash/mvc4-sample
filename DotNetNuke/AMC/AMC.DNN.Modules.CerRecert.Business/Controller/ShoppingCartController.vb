Imports AMC.DNN.Modules.CertRecert.Data
Imports AMC.DNN.Modules.CertRecert.Data.Enums
Imports TIMSS.API.User.UserDefinedInfo

Namespace Controller
    Partial Class AmcCertRecertController
        Public Function GetProductReateCodeReateStructure (productId As String) _
            As PersonifyDataProvider.ProductRateCodeRateStructure
            Return _personifyDataProvider.GetProductReateCodeReateStructure (productId)
        End Function

        ''' <summary>
        ''' Get User Defined Certification Application Exam Period
        ''' </summary>
        ''' <param name="masterCustomerId"></param>
        ''' <param name="subCustomerId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetUserDefinedCertificationApplicationExamPeriod(surveyTitle As String,
                                                                         masterCustomerId As String,
                                                                         subCustomerId As Integer) _
            As IUserDefinedCertificationApplicationExamPeriod

            'get survey defination
            Dim examChoiceSurvey = CType(_personifyDataProvider.GetSurveyByTitle(surveyTitle), UserDefinedSurvey)
            Dim examChoiceQuestions = CType(examChoiceSurvey.UserDefinedSurveyQuestions, UserDefinedSurveyQuestions)
            Dim surveyResponse As UserDefinedCustomerSurveyResponses = Nothing
            If examChoiceQuestions IsNot Nothing AndAlso examChoiceQuestions.Count > 0 Then
                'looking for respone
                Dim tempSurveyResponse As UserDefinedCustomerSurveyResponses
                For Each question As UserDefinedSurveyQuestion In examChoiceQuestions
                    tempSurveyResponse = _personifyDataProvider.GetCustomerSurveyResponseByQuestionId(question.QuestionId,
                                                                                      question.SurveyId,
                                                                                      masterCustomerId,
                                                                                      subCustomerId)

                    If Not tempSurveyResponse Is Nothing Then
                        If tempSurveyResponse.ResponseId > 0 AndAlso tempSurveyResponse.AnswerId > 0 Then
                            surveyResponse = tempSurveyResponse
                            Exit For
                        End If
                    End If
                Next
            End If
            Dim ret As IUserDefinedCertificationApplicationExamPeriod = Nothing
            'get Exam Period
            If surveyResponse IsNot Nothing Then
                ret = _personifyDataProvider.GetCertAppExamPeriodByQuestionId(surveyResponse.AnswerId)
            End If
            Return ret
        End Function
    End Class
End Namespace
