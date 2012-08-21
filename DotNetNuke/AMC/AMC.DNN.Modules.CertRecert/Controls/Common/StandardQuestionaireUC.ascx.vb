Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Data.Enums
Imports AMC.DNN.Modules.CertRecert.Business.Controller
Imports AMC.DNN.Modules.CertRecert.Business.BusinessValidation

Imports TIMSS.API.Core.Validation
Imports TIMSS.API.User.UserDefinedInfo

Namespace Controls.Common
    Public Class StandardQuestionaireUC
        Inherits SectionBaseUserControl

#Region "Event Handle"
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            
        End Sub
        Protected Sub rptQuestionnaire_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptQuestionnaire.ItemDataBound
            Try
                If (e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item) Then
                    Dim hdAnswerYes = CType(e.Item.FindControl("hdAnswerYes"), HiddenField)
                    Dim hdAnswerNo = CType(e.Item.FindControl("hdAnswerNo"), HiddenField)
                    Dim hdResponseId = CType(e.Item.FindControl("hdResponseId"), HiddenField)
                    Dim rdlAnswer = CType(e.Item.FindControl("rdlAnswer"), RadioButtonList)
                    Dim surveyQuestion = CType(e.Item.DataItem, UserDefinedSurveyQuestion)
                    rdlAnswer.DataTextField = "AnswerText"
                    rdlAnswer.DataValueField = "AnswerId"
                    rdlAnswer.DataSource = surveyQuestion.UserDefinedSurveyAnsweres
                    rdlAnswer.DataBind()

                    For Each surveyAnswer As IUserDefinedSurveyAnswers In surveyQuestion.UserDefinedSurveyAnsweres
                        If surveyAnswer.AnswerText.Equals(AnswerTextDefault.YES.ToString()) Then
                            hdAnswerYes.Value = surveyAnswer.AnswerId.ToString()
                        Else
                            hdAnswerNo.Value = surveyAnswer.AnswerId.ToString()
                        End If
                    Next
                    Dim surveyResponse =
                        AMCCertRecertController.GetCustomerSurveyResponseByQuestionId(
                                                                    surveyQuestion.QuestionId,
                                                                    surveyQuestion.SurveyId,
                                                                    Me.MasterCustomerId,
                                                                    Me.SubCustomerId)

                    If surveyResponse IsNot Nothing Then
                        hdResponseId.Value = surveyResponse.ResponseId.ToString()
                        Dim surveyAnswer =
                            CType(surveyQuestion.UserDefinedSurveyAnsweres.FindObject("AnswerId", surveyResponse.AnswerId), UserDefinedSurveyAnswers)
                        rdlAnswer.SelectedValue = surveyAnswer.AnswerId.ToString()
                        If surveyAnswer.AnswerText.Equals(AnswerTextDefault.YES.ToString()) Then
                            Dim pnlDetail = CType(e.Item.FindControl("pnlDetail"), Panel)
                            pnlDetail.CssClass = String.Empty
                            Dim txtDetails = CType(e.Item.FindControl("txtDetails"), TextBox)
                            txtDetails.Text = surveyResponse.Comments
                        End If
                        Dim fileName As String = String.Empty
                        Dim linkLocation As String = String.Empty
                        fileName = GetFileNameOfDocument(DocumentationType.QUESTIONNAIRE.ToString(),
                                                         surveyResponse.Guid.ToString(),
                                                         surveyResponse.ResponseId.ToString(),
                                                         linkLocation)
                        If Not String.IsNullOrEmpty(fileName) Then
                            Dim hlUploadFileAttachment =
                                CType(e.Item.FindControl("hlUploadFileAttachment"), HyperLink)
                            hlUploadFileAttachment.Text = fileName
                            hlUploadFileAttachment.NavigateUrl = linkLocation
                        End If
                    End If
                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Public Overrides Function Save() As IIssuesCollection
            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
            Dim results As New IssuesCollection
            Try
                Dim fieldquestionList = GetFieldInfo("QuestionList")
                If fieldquestionList IsNot Nothing AndAlso fieldquestionList.IsEnabled Then
                    Dim customerResponsees = AMCCertRecertController.GetCustomerSurveyResponees(Long.Parse(hdSurveyId.Value),
                                                                                            MasterCustomerId,
                                                                                            Me.SubCustomerId)
                    For Each customerResponse As IUserDefinedCustomerSurveyResponses In customerResponsees
                        UploadFileIssue.Assert(False, customerResponse)
                    Next
                    results = CType(SaveCustomerResponsees(), IssuesCollection)
                    If results Is Nothing OrElse results.Count <= 0 Then
                        hdIsValidateFailed.Value = CommonConstants.TAB_COMPLETED
                        hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                        Dim surveyIdValue = Long.Parse(Me.hdSurveyId.Value)
                        AMCCertRecertController.RefreshCustomerSurveyResponsees(surveyIdValue, MasterCustomerId, SubCustomerId)
                    Else
                        hdIsValidateFailed.Value = CommonConstants.TAB_INCOMPLETED
                        ShowIssueMessages(results)
                    End If
                    BindingDataToList()
                Else
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                End If
                
            Catch ex As Exception
                ProcessException(ex)
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
            End Try
            Return results
        End Function

#End Region

#Region "Private member"
        Public Overrides Sub ValidateFormFillCompleted()
            Dim fieldquestionList = GetFieldInfo("QuestionList")
            If fieldquestionList IsNot Nothing AndAlso fieldquestionList.IsEnabled Then
                BindingDataToList()
                For Each rptItem As RepeaterItem In Me.rptQuestionnaire.Items
                    Dim hdQuestionEnabled = CType(rptItem.FindControl("hdQuestionEnabled"), HiddenField)
                    If hdQuestionEnabled IsNot Nothing AndAlso Convert.ToBoolean(hdQuestionEnabled.Value) Then
                        Dim rdlAnswer = CType(rptItem.FindControl("rdlAnswer"), RadioButtonList)
                        If String.IsNullOrEmpty(rdlAnswer.SelectedValue) Then
                            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                            Exit For
                        End If
                    End If
                Next
            Else
                hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
            End If

        End Sub

        Private Function SetPropertiesForCustomerResponse(ByVal rptItem As RepeaterItem,
                                                     ByRef customerResponse As IUserDefinedCustomerSurveyResponses) As Boolean
            Dim isIssue As Boolean = True

            Dim surveyIdValue = Long.Parse(hdSurveyId.Value)
            Dim hdAnswerYes = CType(rptItem.FindControl("hdAnswerYes"), HiddenField)
            Dim hdAnswerNo = CType(rptItem.FindControl("hdAnswerNo"), HiddenField)
            Dim txtDetails = CType(rptItem.FindControl("txtDetails"), TextBox)
            Dim hdQuestionId = CType(rptItem.FindControl("hdQuestionId"), HiddenField)
            Dim questionIdValue = Long.Parse(hdQuestionId.Value)
            Dim hdResponseId = CType(rptItem.FindControl("hdResponseId"), HiddenField)
            Dim responseIdValue = Long.Parse(hdResponseId.Value)
            Dim fuUploadFileAttachment = CType(rptItem.FindControl("fuUploadFileAttachment"), FileUpload)
            Dim rdlAnswer = CType(rptItem.FindControl("rdlAnswer"), RadioButtonList)
            If customerResponse.AnswerId = Long.Parse(hdAnswerYes.Value) AndAlso
               (rdlAnswer.SelectedValue.Equals(hdAnswerNo.Value)) AndAlso responseIdValue > 0 Then
               DeleteMainAttachDocument(DocumentationType.QUESTIONNAIRE.ToString(), customerResponse.ResponseId.ToString())
            End If

            For Each issue As IIssue In customerResponse.ValidationIssuesForMe
                customerResponse.ValidationIssues.Remove(issue)
            Next

            UploadFileIssue.Assert(False, customerResponse)
            With customerResponse
                .MasterCustomerId = MasterCustomerId
                .SubcustomerId = SubCustomerId
                .SurveyId = surveyIdValue
                .QuestionId = questionIdValue
                .Comments = txtDetails.Text
            End With
            If Not Long.TryParse(rdlAnswer.SelectedValue, customerResponse.AnswerId) Then
                CheckBoxMustSelectedIssue.Assert(True, customerResponse,
                                                 Localization.GetString("CheckBoxMustSelectedIssue.Text", LocalResourceFile))
            End If
            If Not customerResponse.AnswerId.ToString().Equals(hdAnswerYes.Value) Then
                customerResponse.Comments = String.Empty
            End If
            If fuUploadFileAttachment.FileContent.Length > 0 Then
                UploadFileIssue.Assert(UploadFileIssue.IsNotPdfFile(fuUploadFileAttachment.FileContent), customerResponse)
            End If
            Dim fileName As String = String.Empty
            Dim linkLocation As String = String.Empty
            fileName = GetFileNameOfDocument(DocumentationType.QUESTIONNAIRE.ToString(),
                                             customerResponse.Guid.ToString(),
                                             customerResponse.ResponseId.ToString(),
                                             linkLocation)
            If customerResponse.AnswerId.ToString().Equals(hdAnswerYes.Value) AndAlso
                String.IsNullOrEmpty(customerResponse.Comments) AndAlso fuUploadFileAttachment.FileContent.Length < 1 AndAlso String.IsNullOrEmpty(fileName) Then
                CheckBoxMustSelectedIssue.Assert(True, customerResponse,
                                                 Localization.GetString("MustFillInCommentOrUploadDocument.Text", LocalResourceFile))
            End If
            If customerResponse.ValidationIssuesForMe.Count > 0 Then
                isIssue = False
            End If
            Return isIssue
        End Function

        Private Function SaveCustomerResponsees() As IIssuesCollection
            Dim issueColletion As New IssuesCollection
            Dim surveyIdValue = Long.Parse(Me.hdSurveyId.Value)
            For Each rptItem As RepeaterItem In Me.rptQuestionnaire.Items
                Dim hdEnabled = CType(rptItem.FindControl("hdQuestionEnabled"), HiddenField)
                If hdEnabled IsNot Nothing AndAlso Not String.IsNullOrEmpty(hdEnabled.Value) AndAlso Convert.ToBoolean(hdEnabled.Value) Then
                    Dim hdResponseId = CType(rptItem.FindControl("hdResponseId"), HiddenField)
                    Dim responseIdValue = Long.Parse(hdResponseId.Value)
                    Dim customerResponse As IUserDefinedCustomerSurveyResponses
                    Dim hdAnswerYes = CType(rptItem.FindControl("hdAnswerYes"), HiddenField)
                    Dim hdAnswerNo = CType(rptItem.FindControl("hdAnswerNo"), HiddenField)
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
                    If customerResponse.ValidationIssuesForMe Is Nothing OrElse
                        customerResponse.ValidationIssuesForMe.Count < 1 Then
                        AMCCertRecertController.CommitCustomerSurveyResponsees(surveyIdValue,
                                                                                MasterCustomerId,
                                                                                SubCustomerId)
                        ''upload document
                        Dim hfDeleteFile = CType(rptItem.FindControl("hfDeleteFile"), HiddenField)
                        Dim fuUploadFileAttachment =
                                    CType(rptItem.FindControl("fuUploadFileAttachment"), FileUpload)
                        If hfDeleteFile.Value.Equals("YES") Then ''Delete file
                            DeleteAttachDocument(DocumentationType.QUESTIONNAIRE.ToString(),
                                                 customerResponse.Guid,
                                                 customerResponse.ResponseId.ToString())
                        End If

                        If customerResponse.AnswerId.ToString().ToLower() = hdAnswerYes.Value.ToLower() AndAlso fuUploadFileAttachment.FileContent.Length > 0 Then
                            Dim fileLocation As String = String.Empty
                            fileLocation = UploadTempFile(fuUploadFileAttachment,
                                           DocumentationType.QUESTIONNAIRE.ToString(),
                                           customerResponse.Guid)
                            If Not String.IsNullOrEmpty(fileLocation) Then
                                MoveFileFromTempToMainDirectory(DocumentationType.QUESTIONNAIRE.ToString(),
                                                                customerResponse.Guid,
                                                                customerResponse.ResponseId.ToString(),
                                                                String.Empty)
                            End If
                        End If
                    End If
                End If
            Next
            Dim customerResponsees = AMCCertRecertController.GetCustomerSurveyResponees(Long.Parse(hdSurveyId.Value),
                                                                                      Me.MasterCustomerId,
                                                                                      Me.SubCustomerId)
            For Each iissue As IIssue In customerResponsees.ValidationIssues
                issueColletion.Add(iissue)
            Next
            Return issueColletion
        End Function

        Private Function BindingDataToList() As Object
            Dim currentResponse As Object = Nothing
            Dim surveyQuestionaire As UserDefinedSurveyQuestions
            Dim survey As UserDefinedSurvey
            survey = CType(AMCCertRecertController.GetSurveyByTitle(DataAccessConstants.CERTIFICATION_STANDARD_QUESTIONAIRE_SURVEY_TITLE), UserDefinedSurvey)
            If survey IsNot Nothing Then
                Me.hdSurveyId.Value = survey.SurveyId.ToString()
                surveyQuestionaire =
                    CType(survey.UserDefinedSurveyQuestions, UserDefinedSurveyQuestions)
                For Each userDefinedSurveyQuestion As UserDefinedSurveyQuestion In surveyQuestionaire
                    If userDefinedSurveyQuestion.Enabled AndAlso userDefinedSurveyQuestion.UserDefinedCustomerSurveyResponsees.Count > 0 Then
                        currentResponse =
                            userDefinedSurveyQuestion.UserDefinedCustomerSurveyResponsees.FindObject("MasterCustomerId",
                                                                                                     MasterCustomerId)
                        If currentResponse IsNot Nothing AndAlso Not Page.IsPostBack Then
                            hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                        End If
                        Exit For
                    End If
                Next
                Me.rptQuestionnaire.DataSource = surveyQuestionaire
                Me.rptQuestionnaire.DataBind()
            End If
            Return currentResponse
        End Function
#End Region
    End Class
End Namespace