Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Data.Enums
Imports AMC.DNN.Modules.CertRecert.Business.Controller

Imports TIMSS.API.Core.Validation
Imports TIMSS.API.User.UserDefinedInfo

Namespace Controls.Common
    Public Class ProfessionalPracticeSetting
        Inherits SectionBaseUserControl

#Region "Private Member"

#End Region

#Region "Properties"

#End Region

#Region "Event Handle"
        Protected Sub rptQuestionnaire_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptQuestionnaire.ItemDataBound
            Try
                If e.Item.ItemType = ListItemType.Item OrElse
                      e.Item.ItemType = ListItemType.AlternatingItem Then
                    Dim chkYesNo = CType(e.Item.FindControl("chkYesNo"), CheckBox)
                    Dim hdAnswerYes = CType(e.Item.FindControl("hdAnswerYes"), HiddenField)
                    Dim hdAnswerNo = CType(e.Item.FindControl("hdAnswerNo"), HiddenField)
                    Dim hdResponseId = CType(e.Item.FindControl("hdResponseId"), HiddenField)
                    Dim surveyQuestion = CType(e.Item.DataItem, UserDefinedSurveyQuestion)
                    Dim surveyResponse =
                        amcCertRecertController.GetCustomerSurveyResponseByQuestionId(
                                                                    surveyQuestion.QuestionId,
                                                                    surveyQuestion.SurveyId,
                                                                    Me.MasterCustomerId,
                                                                    Me.SubCustomerId)

                    If surveyResponse IsNot Nothing Then
                        hdResponseId.Value = surveyResponse.ResponseId.ToString()
                        Dim surveyAnswer =
                            CType(surveyQuestion.UserDefinedSurveyAnsweres.FindObject("AnswerId", surveyResponse.AnswerId), UserDefinedSurveyAnswers)
                        If surveyAnswer.AnswerText.Equals(AnswerTextDefault.YES.ToString()) Then
                            chkYesNo.Checked = True
                        End If
                    End If

                    For Each surveyAnswer As IUserDefinedSurveyAnswers In surveyQuestion.UserDefinedSurveyAnsweres
                        If surveyAnswer.QuestionId = surveyQuestion.QuestionId Then
                            If surveyAnswer.AnswerText.Equals(AnswerTextDefault.YES.ToString()) Then
                                hdAnswerYes.Value = surveyAnswer.AnswerId.ToString()
                            Else
                                hdAnswerNo.Value = surveyAnswer.AnswerId.ToString()
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Public Overrides Function Save() As IIssuesCollection
            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
            Dim results As New IssuesCollection
            Try
                Dim fieldQuestionList = GetFieldInfo("QuestionList")
                If fieldQuestionList IsNot Nothing AndAlso fieldQuestionList.IsEnabled Then
                    results = CType(SaveCustomerResponsees(), IssuesCollection)

                    If results Is Nothing OrElse results.Count <= 0 Then
                        Dim surveyIdValue = Long.Parse(Me.hdSurveyId.Value)
                        AMCCertRecertController.RefreshCustomerSurveyResponsees(surveyIdValue, MasterCustomerId, SubCustomerId)
                        hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                    End If
                    Dim questionResponse = BindingDataToList()
                    If questionResponse Is Nothing Then
                        hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                    End If
                Else
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                End If
                
            Catch ex As Exception
                ProcessException(ex)
            End Try
            Return results
        End Function

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        End Sub
#End Region

#Region "Private member"
        Public Overrides Sub ValidateFormFillCompleted()
            Try
                Dim fieldQuestionList = GetFieldInfo("QuestionList")
                If fieldQuestionList IsNot Nothing AndAlso fieldQuestionList.IsEnabled Then
                    BindingDataToList()
                Else
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Private Sub SetPropertiesForCustomerResponse(ByVal rptItem As RepeaterItem,
                                                     ByRef customerResponse As IUserDefinedCustomerSurveyResponses)

            Dim surveyIdValue = Long.Parse(Me.hdSurveyId.Value)
            Dim chkYesNo = CType(rptItem.FindControl("chkYesNo"), CheckBox)
            Dim hdAnswerYes = CType(rptItem.FindControl("hdAnswerYes"), HiddenField)
            Dim hdAnswerNo = CType(rptItem.FindControl("hdAnswerNo"), HiddenField)
            Dim hdResponseId = CType(rptItem.FindControl("hdResponseId"), HiddenField)

            Dim hdQuestionId = CType(rptItem.FindControl("hdQuestionId"), HiddenField)
            Dim responseIdValue = Long.Parse(hdResponseId.Value)
            Dim questionIdValue = Long.Parse(hdQuestionId.Value)

            With customerResponse
                .MasterCustomerId = MasterCustomerId
                .SubcustomerId = SubCustomerId
                .SurveyId = surveyIdValue
                .QuestionId = questionIdValue
                If chkYesNo.Checked Then
                    .AnswerId = Long.Parse(hdAnswerYes.Value)
                Else
                    .AnswerId = Long.Parse(hdAnswerNo.Value)
                End If
            End With

        End Sub

        Private Function SaveCustomerResponsees() As IIssuesCollection
            Dim issueColletion As IIssuesCollection = Nothing
            Dim surveyIdValue = Long.Parse(Me.hdSurveyId.Value)

            For Each rptItem As RepeaterItem In Me.rptQuestionnaire.Items
                Dim hdEnabled = CType(rptItem.FindControl("hdQuestionEnabled"), HiddenField)
                If hdEnabled IsNot Nothing AndAlso Not String.IsNullOrEmpty(hdEnabled.Value) AndAlso Convert.ToBoolean(hdEnabled.Value) Then
                    Dim hdResponseId = CType(rptItem.FindControl("hdResponseId"), HiddenField)
                    Dim responseIdValue = Long.Parse(hdResponseId.Value)
                    Dim customerResponse As IUserDefinedCustomerSurveyResponses
                    If Long.Parse(hdResponseId.Value) > 0 Then ''has existing response
                        customerResponse =
                            AMCCertRecertController.GetCustomerSurveyResponseByReponseId(
                                                                            responseIdValue,
                                                                            surveyIdValue,
                                                                            MasterCustomerId,
                                                                            SubCustomerId)
                        SetPropertiesForCustomerResponse(rptItem, customerResponse)
                        issueColletion = AMCCertRecertController.UpdateCustomerSurveyResponse(customerResponse)
                    Else
                        customerResponse = (New UserDefinedCustomerSurveyResponsees()).CreateNew()
                        SetPropertiesForCustomerResponse(rptItem, customerResponse)
                        issueColletion = AMCCertRecertController.InsertCustomerSurveyResponse(customerResponse)
                    End If
                End If
            Next
            issueColletion = amcCertRecertController.CommitCustomerSurveyResponsees(surveyIdValue,
                                                                                    MasterCustomerId,
                                                                                    SubCustomerId)
           Return issueColletion
        End Function

        Private Function BindingDataToList() As Object

            Dim currentResponse As Object = Nothing
            Dim surveyQuestionaire As UserDefinedSurveyQuestions
            Dim survey As UserDefinedSurvey = Nothing

            If Me.CurrentCertificationCustomerCertification IsNot Nothing Then
                If Me.CurrentCertificationCustomerCertification.CertificationTypeCodeString.Equals(CertificationTypeEnum.CERTIFICATION.ToString()) Then
                    survey = CType(AMCCertRecertController.GetSurveyByTitle(
                                               DataAccessConstants.CERT_PROFESSIONAL_PRATICE_SETTING), 
                                               UserDefinedSurvey)
                Else
                    survey = CType(AMCCertRecertController.GetSurveyByTitle(
                                               DataAccessConstants.RECERT_PROFESSIONAL_PRATICE_SETTING), 
                                               UserDefinedSurvey)
                End If
            End If

           
            If survey IsNot Nothing Then
                Me.hdSurveyId.Value = survey.SurveyId.ToString()
                surveyQuestionaire =
                    CType(survey.UserDefinedSurveyQuestions, UserDefinedSurveyQuestions)

            End If

            If surveyQuestionaire IsNot Nothing Then
                If surveyQuestionaire.Count > 0 Then
                    For Each userDefinedSurveyQuestion As UserDefinedSurveyQuestion In surveyQuestionaire
                        If userDefinedSurveyQuestion.UserDefinedCustomerSurveyResponsees.Count > 0 Then
                            currentResponse = userDefinedSurveyQuestion.UserDefinedCustomerSurveyResponsees.FindObject("MasterCustomerId", MasterCustomerId)
                            If currentResponse IsNot Nothing AndAlso Not Page.IsPostBack Then
                                hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                            End If
                            Exit For
                        End If
                    Next
                    rptQuestionnaire.DataSource = surveyQuestionaire
                    rptQuestionnaire.DataBind()
                End If

            End If

            Return currentResponse
        End Function
#End Region

    End Class
End Namespace