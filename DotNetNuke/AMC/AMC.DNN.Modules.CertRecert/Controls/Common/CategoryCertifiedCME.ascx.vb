Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Data.Enums
Imports AMC.DNN.Modules.CertRecert.Business.Controller
Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports AMC.DNN.Modules.CertRecert.Business.Entities
Imports AMC.DNN.Modules.CertRecert.Business.BusinessValidation
Imports System.Web.UI.MobileControls
Imports TIMSS.API.Core

Imports TIMSS.API.Core.Validation
Imports TIMSS.API.User.UserDefinedInfo

Namespace Controls.Common
    ''' <summary>
    ''' Prepresent GUI for CategoryCertifiedCME tab 
    ''' </summary>
    Public Class CategoryCertifiedCME
        Inherits SectionBaseUserControl


#Region "Event Handle"
        ''' <summary>
        ''' Saves data on CategoryCertifiedCME tab
        ''' </summary>
        Public Overrides Function Save() As IIssuesCollection
            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
            Dim results As New IssuesCollection
            ''set nothing for results
            Try
                Dim fieldQuestionList = GetFieldInfo("QuestionList")
                If fieldQuestionList IsNot Nothing AndAlso fieldQuestionList.IsEnabled Then
                    results = CType(SaveCustomerResponsees(), IssuesCollection)
                    If results Is Nothing OrElse results.Count <= 0 Then
                        results = CheckBusinessValidation()
                        If results.Count < 1 Then
                            Dim surveyIdValue = Long.Parse(Me.hdSurveyId.Value)
                            AMCCertRecertController.RefreshCustomerSurveyResponsees(surveyIdValue, MasterCustomerId, SubCustomerId)
                            hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                        End If
                    End If
                    BindingDataToList()
                Else
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                End If
                
            Catch ex As Exception
                ProcessException(ex)
            End Try
            Return results
        End Function


#End Region

#Region "Private Methods"
        Private Function CheckBusinessValidation() As IssuesCollection
            Dim issueCollection As New IssuesCollection
            Try
                Dim otherModuleSetting As OtherModuleSettings = ModuleConfigurationHelper.Instance.GetSettings(Of OtherModuleSettings)(
                                                                            Server.MapPath(ParentModulePath),
                                                                            CommonConstants.OTHER_MODULE_SETTING_FILE_PATH)
                If Me.CurrentCertificationCustomerCertification.CertificationTypeCodeString.Equals(
                    CertificationTypeEnum.CERTIFICATION.ToString()) Then
                    Dim validationEnabled = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                    ValidationRuleId.CERT_CATEGORY_CME_HOURS.ToString(),
                                                                    Server.MapPath(ParentModulePath))
                    If validationEnabled Then
                        Dim survey = CType(AMCCertRecertController.GetSurveyByTitle(DataAccessConstants.CATEGORY_CERTIFIED_CME), UserDefinedSurvey)
                        If survey IsNot Nothing Then
                            Dim isCheckCMEHours As Boolean = True
                            Dim followQuestion = CType(survey.UserDefinedSurveyQuestions.FindObject("QuestionCode",
                                                            Enums.QuestionCode.CATEGORY_CERTIFIED_CME_FELLOWSHIP.ToString()), 
                                                        UserDefinedSurveyQuestion)
                            Dim followResponse As UserDefinedCustomerSurveyResponses = Nothing
                            If followQuestion IsNot Nothing Then
                                followResponse = AMCCertRecertController.GetCustomerSurveyResponseByQuestionId(followQuestion.QuestionId,
                                                                                                               survey.SurveyId,
                                                                                                               MasterCustomerId,
                                                                                                               SubCustomerId)
                            End If
                            If followResponse IsNot Nothing Then
                                Dim answerYes = CType(followQuestion.UserDefinedSurveyAnsweres.FindObject("AnswerText",
                                                                                                          AnswerTextDefault.YES.ToString()), 
                                                                                                      UserDefinedSurveyAnswers)
                                If answerYes IsNot Nothing Then
                                    If answerYes.AnswerId = followResponse.AnswerId Then
                                        isCheckCMEHours = False
                                    End If
                                End If
                            End If
                            If isCheckCMEHours Then
                                For Each question As IUserDefinedSurveyQuestion In survey.UserDefinedSurveyQuestions
                                    If Convert.ToBoolean(question.Enabled) Then
                                        If question.QuestionCode.Equals(Enums.QuestionCode.CATEGORY_CERTIFIED_CME_HOURS_EARNED.ToString()) Then
                                            Dim customerResponse = AMCCertRecertController.GetCustomerSurveyResponseByQuestionId(question.QuestionId,
                                                                                                                                 survey.SurveyId,
                                                                                                                                 MasterCustomerId,
                                                                                                                                 SubCustomerId)

                                            Dim cmeHoursInConfig = ModuleConfigurationHelper.Instance.GetSettings(Of OtherModuleSettings)(
                                                                                        Server.MapPath(ParentModulePath),
                                                                                        CommonConstants.OTHER_MODULE_SETTING_FILE_PATH).CMEHourEarned
                                            If cmeHoursInConfig Is Nothing Then
                                                cmeHoursInConfig = 0
                                            End If
                                            Dim cmeHoursErrorMessage = Localization.GetString("EanredCmeHoursError.Text",
                                                                                               LocalResourceFile)
                                            If Not String.IsNullOrEmpty(cmeHoursErrorMessage) Then
                                                cmeHoursErrorMessage = String.Format(cmeHoursErrorMessage, cmeHoursInConfig)
                                            End If

                                            If customerResponse IsNot Nothing Then
                                                Dim customerHours As Long = 0
                                                If Not Long.TryParse(customerResponse.Comments, customerHours) Then
                                                    customerHours = 0
                                                End If
                                                If customerHours < cmeHoursInConfig Then
                                                    issueCollection.Add(New EarnedCMEHoursIssue(New BusinessObject(), cmeHoursErrorMessage))
                                                End If
                                            Else
                                                issueCollection.Add(New EarnedCMEHoursIssue(New BusinessObject(), cmeHoursErrorMessage))
                                            End If
                                            Exit For
                                        End If
                                    End If
                                Next
                            End If

                        End If
                    End If
                Else
                    Dim recertIssueColection = CheckBusinessValidationOnRecertFlow(otherModuleSetting)
                    For Each issue As IIssue In recertIssueColection
                        issueCollection.Add(issue)
                    Next
                End If
            Catch ex As Exception
                ''log exception
                ProcessException(ex)
            End Try
            Return issueCollection
        End Function

        Private Function CheckBusinessValidationOnRecertFlow(ByVal otherModuleSetting As OtherModuleSettings) As IIssuesCollection
            Dim issueColection As New IssuesCollection
            Dim recertCategoryCMERuleEnabled As Boolean = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                    ValidationRuleId.RECERT_CATEGORY_CERTIFIED_CME.ToString(),
                                                                    Server.MapPath(ParentModulePath))

            If recertCategoryCMERuleEnabled = True Then
                Dim survey = CType(AMCCertRecertController.GetSurveyByTitle(DataAccessConstants.RECERTIFICATION_CATEGORY_CERTIFIED_CME), 
                                   UserDefinedSurvey)
                If survey IsNot Nothing Then
                    survey.UserDefinedSurveyQuestions.Sort("QuestionCode")
                    For Each question As UserDefinedSurveyQuestion In survey.UserDefinedSurveyQuestions
                        If Convert.ToBoolean(question.Enabled) Then  '' questtion is enable must be Check validation
                            Dim valueTocompare As Integer = 0
                            Dim userResponse As Integer = 0
                            Dim inputErrorMessage As String = String.Empty
                            Dim customerResponse = AMCCertRecertController.GetCustomerSurveyResponseByQuestionId(question.QuestionId,
                                                                                                                 survey.SurveyId,
                                                                                                                 MasterCustomerId,
                                                                                                                 SubCustomerId)
                            If customerResponse Is Nothing OrElse Not Integer.TryParse(customerResponse.Comments, userResponse) Then
                                userResponse = 0
                            End If
                            If otherModuleSetting Is Nothing Then
                                Return issueColection
                            End If
                            Select Case question.QuestionCode
                                Case Enums.QuestionCode.RECERT_CME_HOURS_TEN_YEARS.ToString()
                                    If Not Integer.TryParse(otherModuleSetting.ReCertCMEQuestion1, valueTocompare) _
                                        Then
                                        valueTocompare = 0
                                    End If
                                    If (userResponse < valueTocompare) Then
                                        inputErrorMessage = Localization.GetString("ReCertCMEQuestion1Error.Text",
                                                                                       LocalResourceFile)
                                        inputErrorMessage = String.Format(inputErrorMessage, valueTocompare)
                                    End If

                                Case Enums.QuestionCode.RECERT_CME_HOURS_TRAINING.ToString()
                                    If Not Integer.TryParse(otherModuleSetting.ReCertCMEQuestion2, valueTocompare) _
                                        Then
                                        valueTocompare = 0
                                    End If
                                    If (userResponse < valueTocompare) Then
                                        inputErrorMessage = Localization.GetString("ReCertCMEQuestion2Error.Text",
                                                                                       LocalResourceFile)
                                        inputErrorMessage = String.Format(inputErrorMessage, valueTocompare)
                                    End If
                                Case Enums.QuestionCode.RECERT_CME_HOURS_THREE_YEARS.ToString()
                                    If Not Integer.TryParse(otherModuleSetting.ReCertCMEQuestion3, valueTocompare) _
                                        Then
                                        valueTocompare = 0
                                    End If
                                    If (userResponse < valueTocompare) Then
                                        inputErrorMessage = Localization.GetString("ReCertCMEQuestion3Error.Text",
                                                                                       LocalResourceFile)
                                        inputErrorMessage = String.Format(inputErrorMessage, valueTocompare)

                                    End If
                                Case Enums.QuestionCode.RECERT_CATEGORY_CME_HOURS_TRAINING.ToString()
                                    If Not Integer.TryParse(otherModuleSetting.ReCertCMEQuestion4, valueTocompare) _
                                        Then
                                        valueTocompare = 0
                                    End If
                                    If (userResponse < valueTocompare) Then
                                        inputErrorMessage = Localization.GetString("ReCertCMEQuestion4Error.Text",
                                                                                       LocalResourceFile)
                                        inputErrorMessage = String.Format(inputErrorMessage, valueTocompare)
                                    End If
                            End Select
                            If Not String.IsNullOrEmpty(inputErrorMessage) Then
                                issueColection.Add(New EarnedCMEHoursIssue(customerResponse, inputErrorMessage))
                            End If
                        End If
                    Next
                End If
            End If
            Return issueColection
        End Function

        ''' <summary>
        ''' binding data and validate on the form
        ''' </summary>
        ''' <remarks></remarks>
        Public Overrides Sub ValidateFormFillCompleted()
            Dim fieldQuestionList = GetFieldInfo("QuestionList")
            If fieldQuestionList IsNot Nothing AndAlso fieldQuestionList.IsEnabled Then
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                BindingDataToList()
                Dim issueResult = CheckBusinessValidation()
                If issueResult.Count < 1 Then
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                Else
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                End If
            Else
                hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
            End If
        End Sub

        Private Sub SetPropertiesForCustomerResponse(ByVal rptItem As RepeaterItem,
                                                     ByRef customerResponse As IUserDefinedCustomerSurveyResponses)

            Dim surveyIdValue = Long.Parse(Me.hdSurveyId.Value)
            Dim chkYesNo = CType(rptItem.FindControl("chkYesNo"), CheckBox)
            Dim hdAnswerYes = CType(rptItem.FindControl("hdAnswerYes"), HiddenField)
            Dim hdAnswerNo = CType(rptItem.FindControl("hdAnswerNo"), HiddenField)
            Dim hdQuestionId = CType(rptItem.FindControl("hdQuestionId"), HiddenField)
            Dim hdAnswerRange = CType(rptItem.FindControl("hdAnswerRange"), HiddenField)
            Dim txtResponse = CType(rptItem.FindControl("txtResponse"), System.Web.UI.WebControls.TextBox)
            Dim questionIdValue = Long.Parse(hdQuestionId.Value)
            With customerResponse
                .MasterCustomerId = MasterCustomerId
                .SubcustomerId = SubCustomerId
                .SurveyId = surveyIdValue
                .QuestionId = questionIdValue
                If chkYesNo IsNot Nothing AndAlso chkYesNo.Visible = True Then
                    If chkYesNo.Checked Then
                        .AnswerId = Long.Parse(hdAnswerYes.Value)
                    Else
                        .AnswerId = Long.Parse(hdAnswerNo.Value)
                    End If
                End If

                If hdAnswerRange IsNot Nothing AndAlso Long.Parse(hdAnswerRange.Value) > 0 Then
                    .AnswerId = Long.Parse(hdAnswerRange.Value)
                    .Comments = txtResponse.Text
                End If
            End With

        End Sub

        Private Function SaveCustomerResponsees() As IIssuesCollection
            Dim issueColletion As IIssuesCollection = Nothing
            Try
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
                issueColletion = AMCCertRecertController.CommitCustomerSurveyResponsees(surveyIdValue,
                                                                                        MasterCustomerId,
                                                                                        SubCustomerId)
            Catch ex As Exception
                ProcessException(ex)
            End Try
            Return issueColletion
        End Function

        Private Function BindingDataToList() As Object
            Dim currentResponse As New Object
            Dim surveyQuestionaire As UserDefinedSurveyQuestions
            Dim survey As UserDefinedSurvey = Nothing
            If Me.CurrentCertificationCustomerCertification IsNot Nothing Then
                If Me.CurrentCertificationCustomerCertification.CertificationTypeCodeString.Equals(CertificationTypeEnum.CERTIFICATION.ToString()) Then
                    survey = CType(AMCCertRecertController.GetSurveyByTitle(DataAccessConstants.CATEGORY_CERTIFIED_CME), UserDefinedSurvey)
                Else
                    survey = CType(AMCCertRecertController.GetSurveyByTitle(DataAccessConstants.RECERTIFICATION_CATEGORY_CERTIFIED_CME), UserDefinedSurvey)
                End If

                If survey IsNot Nothing Then
                    Me.hdSurveyId.Value = survey.SurveyId.ToString()
                    surveyQuestionaire = CType(survey.UserDefinedSurveyQuestions, UserDefinedSurveyQuestions)
                    rptQuestionnaire.DataSource = surveyQuestionaire
                    rptQuestionnaire.DataBind()
                End If
            End If
            Return currentResponse
        End Function

        Protected Sub rptQuestionnaire_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptQuestionnaire.ItemDataBound
            Try
                If (e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item) Then


                    Dim chkYesNo = CType(e.Item.FindControl("chkYesNo"), CheckBox)
                    Dim hdAnswerYes = CType(e.Item.FindControl("hdAnswerYes"), HiddenField)
                    Dim hdAnswerNo = CType(e.Item.FindControl("hdAnswerNo"), HiddenField)
                    Dim hdResponseId = CType(e.Item.FindControl("hdResponseId"), HiddenField)
                    Dim hdAnswerRange = CType(e.Item.FindControl("hdAnswerRange"), HiddenField)
                    Dim lblQuestionText = CType(e.Item.FindControl("lblQuestionText"), System.Web.UI.WebControls.Label)
                    Dim txtResponse = CType(e.Item.FindControl("txtResponse"), System.Web.UI.WebControls.TextBox)
                    Dim hdQuestionCode = CType(e.Item.FindControl("hdQuestionCode"), HiddenField)
                    Dim surveyQuestion = CType(e.Item.DataItem, UserDefinedSurveyQuestion)
                    Dim surveyResponse =
                            AMCCertRecertController.GetCustomerSurveyResponseByQuestionId(
                                                                        surveyQuestion.QuestionId,
                                                                        surveyQuestion.SurveyId,
                                                                        Me.MasterCustomerId,
                                                                        Me.SubCustomerId)
                    If surveyResponse IsNot Nothing Then
                        hdResponseId.Value = surveyResponse.ResponseId.ToString()
                        Dim surveyAnswer =
                            CType(surveyQuestion.UserDefinedSurveyAnsweres.FindObject("AnswerId",
                                                                                      surveyResponse.AnswerId), 
                                                                                        UserDefinedSurveyAnswers)
                        If surveyQuestion.QuestionTypeString.Equals(QuestionTypeString.YESNO.ToString()) Then
                            If surveyAnswer.AnswerText.Equals(AnswerTextDefault.YES.ToString()) Then
                                chkYesNo.Checked = True
                            End If
                        Else
                            txtResponse.Text = surveyResponse.Comments
                        End If
                    End If

                    If surveyQuestion.QuestionTypeString.Equals(QuestionTypeString.YESNO.ToString()) Then
                        lblQuestionText.Visible = False
                        txtResponse.Visible = False
                        chkYesNo.Visible = True
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

                    If surveyQuestion.QuestionTypeString.Equals(QuestionTypeString.RANGE.ToString()) Then
                        lblQuestionText.Visible = True
                        txtResponse.Visible = True
                        chkYesNo.Visible = False
                        For Each surveyAnswer As IUserDefinedSurveyAnswers In surveyQuestion.UserDefinedSurveyAnsweres
                            If surveyAnswer.QuestionId = surveyQuestion.QuestionId Then
                                hdAnswerRange.Value = surveyAnswer.AnswerId.ToString()
                            End If
                        Next
                    End If
                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub
#End Region

    End Class

End Namespace