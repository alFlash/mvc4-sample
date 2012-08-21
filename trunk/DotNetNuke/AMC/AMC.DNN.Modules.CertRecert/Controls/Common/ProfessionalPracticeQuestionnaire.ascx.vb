Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Data.Enums
Imports AMC.DNN.Modules.CertRecert.Business.Controller
Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports AMC.DNN.Modules.CertRecert.Business.Entities
Imports AMC.DNN.Modules.CertRecert.Business.BusinessValidation
Imports TIMSS.API.Core

Imports TIMSS.API.Core.Validation
Imports TIMSS.API.User.UserDefinedInfo

Namespace Controls.Common
    Public Class ProfessionalPracticeQuestionnaire
        Inherits SectionBaseUserControl

#Region "Private Member"

#End Region

#Region "Properties"

#End Region

#Region "Event Handle"
        Public Overrides Function Save() As IIssuesCollection
            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
            Dim results As New IssuesCollection
            Try
                Dim fieldQuestionList = GetFieldInfo("QuestionList")
                If fieldQuestionList IsNot Nothing AndAlso fieldQuestionList.IsEnabled Then
                    results = CType(SaveCustomerResponsees(), IssuesCollection)
                    If results Is Nothing OrElse results.Count <= 0 Then
                        results = CheckBusinessValidation()
                        If results.Count < 1 Then
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
        Protected Sub rptQuestionnaire_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptQuestionnaire.ItemDataBound
            Try
                If (e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item) Then

                    Dim chkYesNo = CType(e.Item.FindControl("chkYesNo"), CheckBox)
                    Dim hdAnswerYes = CType(e.Item.FindControl("hdAnswerYes"), HiddenField)
                    Dim hdAnswerNo = CType(e.Item.FindControl("hdAnswerNo"), HiddenField)
                    Dim hdResponseId = CType(e.Item.FindControl("hdResponseId"), HiddenField)
                    Dim hdAnswerRange = CType(e.Item.FindControl("hdAnswerRange"), HiddenField)
                    Dim lblQuestionText = CType(e.Item.FindControl("lblQuestionText"), Label)
                    Dim txtResponse = CType(e.Item.FindControl("txtResponse"), TextBox)
                    Dim pnlUploadFile = CType(e.Item.FindControl("pnlUploadFile"), Panel)
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
                        If surveyQuestion.QuestionTypeString.Equals(QuestionTypeString.YESNO.ToString()) Then
                            If surveyAnswer.AnswerText.Equals(AnswerTextDefault.YES.ToString()) Then
                                chkYesNo.Checked = True
                            End If
                        Else
                            txtResponse.Text = surveyResponse.Comments
                        End If

                        Dim fileName As String = String.Empty
                        Dim linkLocation As String = String.Empty
                        fileName = GetFileNameOfDocument(DocumentationType.PRO_PRAC_QUESTIONNAIRE.ToString(),
                                                         surveyResponse.Guid.ToString(),
                                                         surveyResponse.ResponseId.ToString(),
                                                         linkLocation)
                        If fileName <> String.Empty Then
                            Dim hlUploadFileAttachment =
                                CType(e.Item.FindControl("hlUploadFileAttachment"), HyperLink)
                            hlUploadFileAttachment.Text = fileName
                            hlUploadFileAttachment.NavigateUrl = linkLocation
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
                    ''only show upload control when qustion code value is Enums.QuestionCode.CERT_PRO_PRAC_QUESTIONAIRE_FELLOWSHIP
                    If surveyQuestion.QuestionCode.Equals(Enums.QuestionCode.CERT_PRO_PRAC_QUESTIONAIRE_FELLOWSHIP.ToString()) Then
                        pnlUploadFile.Visible = True
                    Else
                        pnlUploadFile.Visible = False
                    End If
                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub
#End Region

#Region "Private member"
        Private Function CheckBusinessValidation() As IssuesCollection
            Dim issueCollection As New IssuesCollection
            Try
                Dim validationEnabled = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                    ValidationRuleId.CERT_PRO_QUESTIONNAIRE_VALID_MONTH.ToString(),
                                                                    Server.MapPath(ParentModulePath))
                If validationEnabled Then
                    Dim survey =
                        CType(amcCertRecertController.GetSurveyByTitle(DataAccessConstants.CERT_PROFESSIONAL_PRACTICE_QUESTIONNAIRE), 
                                                                        UserDefinedSurvey)
                    If survey IsNot Nothing Then
                        For Each question As IUserDefinedSurveyQuestion In survey.UserDefinedSurveyQuestions
                            If question.QuestionCode.Equals(Enums.QuestionCode.CERT_PRO_PRAC_QUESTIONAIRE_MONTH.ToString()) AndAlso
                                Convert.ToBoolean(question.Enabled) Then
                                Dim customerResponse = AMCCertRecertController.GetCustomerSurveyResponseByQuestionId(question.QuestionId,
                                                                                                                     survey.SurveyId,
                                                                                                                     MasterCustomerId,
                                                                                                                     SubCustomerId)
                                Dim monthsInConfigString = ModuleConfigurationHelper.Instance.GetSettings(Of OtherModuleSettings)(
                                                            Server.MapPath(ParentModulePath),
                                                            CommonConstants.OTHER_MODULE_SETTING_FILE_PATH).ProPracticeQuestionaireValidateMonth
                                If String.IsNullOrEmpty(monthsInConfigString) Then
                                    Return issueCollection
                                End If
                                Dim monthsInConfig = CType(monthsInConfigString, Long)
                                Dim cmeHoursErrorMessage = Localization.GetString("ProPracQuestionnaireValidMonth.Text",
                                                                                   LocalResourceFile)
                                If Not String.IsNullOrEmpty(cmeHoursErrorMessage) Then
                                    cmeHoursErrorMessage = String.Format(cmeHoursErrorMessage, monthsInConfig)
                                End If

                                If customerResponse IsNot Nothing Then
                                    Dim customerHours As Long = 0
                                    If Not Long.TryParse(customerResponse.Comments, customerHours) Then
                                        customerHours = 0
                                    End If
                                    If customerHours < monthsInConfig Then
                                        issueCollection.Add(New EarnedCMEHoursIssue(New BusinessObject(), cmeHoursErrorMessage))
                                    End If
                                Else
                                    issueCollection.Add(New EarnedCMEHoursIssue(New BusinessObject(), cmeHoursErrorMessage))
                                End If
                                Exit For
                            End If
                        Next
                    End If
                End If
            Catch ex As Exception
                ''log exception
                ProcessException(ex)
            End Try
            Return issueCollection
        End Function
        Public Overrides Sub ValidateFormFillCompleted()
            Try
                Dim fieldQuestionList = GetFieldInfo("QuestionList")
                If fieldQuestionList IsNot Nothing AndAlso fieldQuestionList.IsEnabled Then
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
                
            Catch ex As Exception
                ProcessPageLoadException(ex)
            End Try
        End Sub

        Private Function SetPropertiesForCustomerResponse(ByVal rptItem As RepeaterItem,
                                                     ByRef customerResponse As IUserDefinedCustomerSurveyResponses) As Boolean
            Dim isIssue As Boolean = True
            Dim surveyIdValue = Long.Parse(Me.hdSurveyId.Value)
            Dim chkYesNo = CType(rptItem.FindControl("chkYesNo"), CheckBox)
            Dim hdAnswerYes = CType(rptItem.FindControl("hdAnswerYes"), HiddenField)
            Dim hdAnswerNo = CType(rptItem.FindControl("hdAnswerNo"), HiddenField)
            Dim hdQuestionId = CType(rptItem.FindControl("hdQuestionId"), HiddenField)
            Dim hdAnswerRange = CType(rptItem.FindControl("hdAnswerRange"), HiddenField)
            Dim txtResponse = CType(rptItem.FindControl("txtResponse"), TextBox)
            Dim questionIdValue = Long.Parse(hdQuestionId.Value)
            Dim hdResponseId = CType(rptItem.FindControl("hdResponseId"), HiddenField)
            Dim responseIdValue = Long.Parse(hdResponseId.Value)
            Dim fuUploadFileAttachment = CType(rptItem.FindControl("fuUploadFileAttachment"), FileUpload)
            If customerResponse.AnswerId = Long.Parse(hdAnswerYes.Value) AndAlso
              (chkYesNo.Checked = False) AndAlso responseIdValue > 0 Then
                ''delete file
                DeleteAttachDocument(DocumentationType.QUESTIONNAIRE.ToString(),
                                             customerResponse.Guid,
                                             customerResponse.ResponseId.ToString())
            End If
            customerResponse.ValidationIssuesForMe.RemoveAll()
            UploadFileIssue.Assert(False, customerResponse)
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
            If fuUploadFileAttachment.FileContent.Length > 0 Then
                UploadFileIssue.Assert(UploadFileIssue.IsNotPdfFile(fuUploadFileAttachment.FileContent), customerResponse)
            End If
            If customerResponse.ValidationIssuesForMe.Count > 0 Then
                isIssue = False
            End If
            Return isIssue
        End Function

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
                        If SetPropertiesForCustomerResponse(rptItem, customerResponse) Then
                            AMCCertRecertController.UpdateCustomerSurveyResponse(customerResponse)
                        End If

                    Else
                        customerResponse = (New UserDefinedCustomerSurveyResponsees()).CreateNew()
                        If SetPropertiesForCustomerResponse(rptItem, customerResponse) Then
                            AMCCertRecertController.InsertCustomerSurveyResponse(customerResponse)
                        End If
                    End If
                    For Each iissue As IIssue In customerResponse.ValidationIssuesForMe
                        issueColletion.Add(iissue)
                    Next
                    ''upload document
                    If customerResponse.ValidationIssuesForMe Is Nothing OrElse
                       customerResponse.ValidationIssuesForMe.Count < 1 Then
                        Dim hfDeleteFile = CType(rptItem.FindControl("hfDeleteFile"), HiddenField)
                        Dim fuUploadFileAttachment =
                                    CType(rptItem.FindControl("fuUploadFileAttachment"), FileUpload)
                        If hfDeleteFile.Value.Equals("YES") Then ''Delete file
                            DeleteAttachDocument(DocumentationType.PRO_PRAC_QUESTIONNAIRE.ToString(),
                                                 customerResponse.Guid,
                                                 customerResponse.ResponseId.ToString())
                        End If

                        If fuUploadFileAttachment.FileContent.Length > 0 Then ''upload file
                            Dim fileLocation As String = String.Empty
                            fileLocation = UploadTempFile(fuUploadFileAttachment,
                                           DocumentationType.PRO_PRAC_QUESTIONNAIRE.ToString(),
                                           customerResponse.Guid)
                            If Not String.IsNullOrEmpty(fileLocation) Then
                                MoveFileFromTempToMainDirectory(DocumentationType.PRO_PRAC_QUESTIONNAIRE.ToString(),
                                                                customerResponse.Guid,
                                                                customerResponse.ResponseId.ToString(),
                                                                String.Empty)
                            End If
                        End If
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
            Dim survey As UserDefinedSurvey
            survey = CType(amcCertRecertController.GetSurveyByTitle(DataAccessConstants.CERT_PROFESSIONAL_PRACTICE_QUESTIONNAIRE), UserDefinedSurvey)
            If survey IsNot Nothing Then
                hdSurveyId.Value = survey.SurveyId.ToString()
                surveyQuestionaire =
                    CType(survey.UserDefinedSurveyQuestions, UserDefinedSurveyQuestions)
            End If
            If surveyQuestionaire IsNot Nothing Then
                rptQuestionnaire.DataSource = surveyQuestionaire
                rptQuestionnaire.DataBind()
            End If
            
            Return currentResponse
        End Function

        
#End Region
    End Class
End Namespace