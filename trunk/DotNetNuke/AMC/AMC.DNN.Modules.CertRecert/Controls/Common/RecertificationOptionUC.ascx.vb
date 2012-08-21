
Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Data.Enums
Imports AMC.DNN.Modules.CertRecert.Business.Controller
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Business.IControls
Imports AMC.DNN.Modules.CertRecert.Business.BusinessValidation
Imports TIMSS.API.Core

Imports TIMSS.API.Core.Validation
Imports TIMSS.API.User.UserDefinedInfo

Namespace Controls.Common
    Public Class RecertificationOptionUC
        Inherits SectionBaseUserControl
        Implements ISave

#Region "Properties"
        Public ReadOnly Property RecertOpt0BypassPayment() As Boolean
            Get
                Return False
            End Get
        End Property
#End Region

#Region "Event Handlers"
        ''' <summary>
        ''' Handles the Load event of the Page control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            Try
                RegisterJavascript()
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        ''' <summary>
        ''' Called when [re cert option answer item data bound].
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterItemEventArgs" /> instance containing the event data.</param>
        Private Sub OnReCertOptionAnswerItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
            Try
                If e.Item.DataItem IsNot Nothing Then
                    Dim answer = CType(e.Item.DataItem, UserDefinedSurveyAnswers)
                    Dim answerRow = CType(e.Item.FindControl("answerItem"), HtmlTableRow)
                    Dim examPeriod = AMCCertRecertController.GetCertAppExamPeriodByQuestionId(answer.AnswerId)
                    Dim questionId = answer.QuestionId
                    Dim answerId = answer.AnswerId
                    Dim responses As IUserDefinedCustomerSurveyResponsees = AMCCertRecertController.GetResponses(questionId, answerId)
                    If e.Item.FindControl("rdbRecertOptionAnswerItem") IsNot Nothing Then
                        Dim rdbResponse = CType(e.Item.FindControl("rdbRecertOptionAnswerItem"), RadioButton)
                        rdbResponse.Checked = responses IsNot Nothing AndAlso responses.Count > 0
                    End If
                    If examPeriod IsNot Nothing Then
                        Dim startDateRadio = CType(e.Item.FindControl("rdbRecertOptionAnswerItem"), RadioButton)
                        Dim endDateLabel = CType(e.Item.FindControl("lblReCertOptionAnswerEndDate"), Label)
                        Dim deadlineLabel = CType(e.Item.FindControl("lblReCertOptionAnswerDeadline"), Label)
                        startDateRadio.Text = examPeriod.StartDate.ToString(CommonConstants.DATE_FORMAT)
                        endDateLabel.Text = examPeriod.EndDate.ToString(CommonConstants.DATE_FORMAT)
                        deadlineLabel.Text = examPeriod.ExamDate.ToString(CommonConstants.DATE_FORMAT)
                        If examPeriod.ExamDate < DateTime.Today Then
                            answerRow.Visible = False
                        End If
                    Else
                        answerRow.Visible = False
                    End If
                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

#End Region

#Region "Private Methods"
        ''' <summary>
        ''' Validates the form fill completed.
        ''' </summary>
        Public Overrides Sub ValidateFormFillCompleted()
            Dim fieldQuestionList = GetFieldInfo("QuestionList")
            If fieldQuestionList IsNot Nothing AndAlso fieldQuestionList.IsEnabled Then
                AddHandler rptRecertOptionNotificationQuestions.ItemDataBound, AddressOf OnRecertOptionNotificationQuestionItemDatabound
                AddHandler rptRecertOptionQuestion.ItemDataBound, AddressOf OnRecertOptionQuestionItemDataBound
                AddHandler rptRecertOptionAnswer.ItemDataBound, AddressOf OnReCertOptionAnswerItemDataBound

                Dim survey = Business.Controller.AmcCertRecertController.GetSurveyByTitle(DataAccessConstants.RECERTIFICATION_OPTION_SERVEY_TITLE,
                                                                                          OrganizationId, OrganizationUnitId)
                Dim notificationSurvey = AMCCertRecertController.GetSurveyByTitle(DataAccessConstants.RECERTIFICATION_OPTION_NOTIFICATION_SURVEY_TITLE, OrganizationId, OrganizationUnitId)
                If notificationSurvey IsNot Nothing Then
                    rptRecertOptionNotificationQuestions.DataSource = CType(notificationSurvey, UserDefinedSurvey).UserDefinedSurveyQuestions
                    rptRecertOptionNotificationQuestions.DataBind()
                End If
                If survey IsNot Nothing Then
                    Dim questions = CType(survey, UserDefinedSurvey).UserDefinedSurveyQuestions
                    Dim answers = CType(survey, UserDefinedSurvey).UserDefinedSurveyAnsweres
                    Dim hasResponses = False
                    If Not Page.IsPostBack Then
                        Dim responses As IUserDefinedCustomerSurveyResponsees = CType(survey, UserDefinedSurvey).UserDefinedCustomerSurveyResponsees
                        If responses IsNot Nothing AndAlso responses.Count > 0 Then
                            For Each currentResponse As UserDefinedCustomerSurveyResponses In responses
                                If currentResponse.MasterCustomerId = MasterCustomerId AndAlso currentResponse.SubcustomerId = SubCustomerId Then
                                    Dim currentQuestion = questions.FindObject("QuestionId", currentResponse.QuestionId)
                                    If currentQuestion IsNot Nothing AndAlso CType(currentQuestion, UserDefinedSurveyQuestion).Enabled Then
                                        SetCurrentReCertOption(responses(0).QuestionId.ToString())
                                        hasResponses = True
                                    End If
                                    Exit For
                                End If
                            Next
                        End If
                        If hasResponses Then
                            hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                        Else
                            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                        End If
                    End If
                    rptRecertOptionQuestion.DataSource = questions
                    rptRecertOptionQuestion.DataBind()

                    rptRecertOptionAnswer.DataSource = answers
                    rptRecertOptionAnswer.DataBind()
                End If
            Else
                hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
            End If

        End Sub

        ''' <summary>
        ''' Called when [recert option notification question item databound].
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterItemEventArgs" /> instance containing the event data.</param>
        Private Sub OnRecertOptionNotificationQuestionItemDatabound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
            If e.Item IsNot Nothing AndAlso e.Item.DataItem IsNot Nothing Then
                Dim data = CType(e.Item.DataItem, UserDefinedSurveyQuestion)
                If data IsNot Nothing Then
                    Dim userdefinedAnswers As IUserDefinedSurveyAnsweres = AMCCertRecertController.GetAnswerByQuestionId(data.QuestionId)
                    If userdefinedAnswers IsNot Nothing Then
                        Dim userdifnedResponses = AMCCertRecertController.GetResponseByQuestionId(data.QuestionId)
                        Dim chkReponse = CType(e.Item.FindControl("chkRecertOptionNotificationResponse"), CheckBox)
                        Dim userDefinedResponse = If(userdifnedResponses.Count > 0, userdifnedResponses(0), Nothing)
                        For Each answer As UserDefinedSurveyAnswers In userdefinedAnswers
                            If Not String.IsNullOrEmpty(answer.AnswerText) Then
                                If answer.AnswerText.ToUpper() = "YES" Then
                                    Dim hdYesAnswerId = CType(e.Item.FindControl("hdYesAnswerId"), HiddenField)
                                    hdYesAnswerId.Value = answer.AnswerId.ToString()
                                Else
                                    Dim hdNoAnswerId = CType(e.Item.FindControl("hdNoAnswerId"), HiddenField)
                                    hdNoAnswerId.Value = answer.AnswerId.ToString()
                                End If
                                If userDefinedResponse Is Nothing Then
                                    chkReponse.Checked = False
                                ElseIf answer.AnswerId = userDefinedResponse.AnswerId Then
                                    chkReponse.Checked = answer.AnswerText.ToUpper() = "YES"
                                End If
                            End If
                        Next
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Called when [recert option question item data bound].
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterItemEventArgs" /> instance containing the event data.</param>
        Private Sub OnRecertOptionQuestionItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
            If e.Item IsNot Nothing AndAlso e.Item.DataItem IsNot Nothing Then
                Dim data = CType(e.Item.DataItem, UserDefinedSurveyQuestion)
                If data IsNot Nothing AndAlso data.Enabled Then
                    Dim userdefinedresponse As UserDefinedCustomerSurveyResponsees = AMCCertRecertController.GetResponseByQuestionId(data.QuestionId)
                    Dim rdbResponse = CType(e.Item.FindControl("rdbRecertOptionQuestionItem"), RadioButton)
                    If rdbResponse IsNot Nothing Then
                        rdbResponse.Checked = userdefinedresponse IsNot Nothing AndAlso userdefinedresponse.Count > 0
                    End If

                    Dim userdefinedAnswers As IUserDefinedSurveyAnsweres = AMCCertRecertController.GetAnswerByQuestionId(data.QuestionId)
                    If userdefinedAnswers IsNot Nothing Then
                        For Each answer As UserDefinedSurveyAnswers In userdefinedAnswers
                            If Not String.IsNullOrEmpty(answer.AnswerText) Then
                                If answer.AnswerText.ToUpper() = "YES" Then
                                    Dim hdYesAnswerId = CType(e.Item.FindControl("hdYesAnswerId"), HiddenField)
                                    hdYesAnswerId.Value = answer.AnswerId.ToString()
                                Else
                                    Dim hdNoAnswerId = CType(e.Item.FindControl("hdNoAnswerId"), HiddenField)
                                    hdNoAnswerId.Value = answer.AnswerId.ToString()
                                End If
                            End If
                        Next
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Registers the javascript.
        ''' </summary>
        Private Sub RegisterJavascript()
            Page.ClientScript.RegisterClientScriptInclude("amc-recert-option", String.Format("{0}/Documentation/scripts/RecertOption.js?v={1}", ParentModulePath, CommonConstants.CURRENT_VERSION))
        End Sub
#End Region

#Region "Public Methods"
        ''' <summary>
        ''' Gets the localized string.
        ''' </summary>
        ''' <param name="key">The key.</param>
        ''' <returns></returns>
        Public Function GetLocalizedString(ByVal key As String) As String
            Return Localization.GetString(key, LocalResourceFile)
        End Function
#End Region

#Region "Section Base"
        ''' <summary>
        ''' Saves this instance.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function Save() As IIssuesCollection
            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
            Try
                Dim fieldQuestionList = GetFieldInfo("QuestionList")
                Dim results As IIssuesCollection = Nothing
                If fieldQuestionList IsNot Nothing AndAlso fieldQuestionList.IsEnabled Then
                    results = AddResponse()
                    If results IsNot Nothing Then
                        If results.Count <= 0 Then
                            hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                        End If
                    End If
                End If
                ValidateFormFillCompleted() 're-bind data
                Return results
            Catch ex As Exception
                ProcessException(ex)
            End Try
            Return Nothing
        End Function

        ''' <summary>
        ''' Adds the response.
        ''' </summary>
        ''' <returns></returns>
        Private Function AddResponse() As IIssuesCollection
            Dim results As IIssuesCollection = New IssuesCollection()
            Dim answerId As Long
            Dim isMultiAnswer = False
            For Each questionItem As RepeaterItem In rptRecertOptionQuestion.Items
                Dim hdQuestionEnabled = CType(questionItem.FindControl("hdQuestionEnabled"), HiddenField)
                If hdQuestionEnabled IsNot Nothing AndAlso Not String.IsNullOrEmpty(hdQuestionEnabled.Value) AndAlso Convert.ToBoolean(hdQuestionEnabled.Value) Then
                    Dim isChecked = CType(questionItem.FindControl("rdbRecertOptionQuestionItem"), RadioButton).Checked
                    If isChecked Then
                        Dim questionId = CType(questionItem.FindControl("hdQuestionId"), HiddenField).Value
                        Dim questionType = CType(questionItem.FindControl("hdQuestionType"), HiddenField).Value
                        answerId = GetAnswerId(questionItem, questionType, answerId, isMultiAnswer, questionId)
                        If answerId > 0 Then
                            AddOrUpdateRecertOptionResponse(answerId, questionId, results)
                            AddOrUpdateNotificationResponse(isMultiAnswer, results)
                        Else
                            results.Add(New AtLeastOneRecordIssue(New BusinessObject(), "No exam window exists."))
                        End If
                        Exit For
                    End If
                End If
            Next
            Return results
        End Function

        ''' <summary>
        ''' Adds the or update notification response.
        ''' </summary>
        ''' <param name="isMultiAnswer">if set to <c>true</c> [is multi answer].</param>
        ''' <param name="results">The results.</param>
        Private Sub AddOrUpdateNotificationResponse(ByVal isMultiAnswer As Boolean, ByVal results As IIssuesCollection)

            If isMultiAnswer AndAlso rptRecertOptionNotificationQuestions.Items IsNot Nothing AndAlso rptRecertOptionNotificationQuestions.Items.Count > 0 Then
                If results Is Nothing OrElse results.Count <= 0 Then
                    For Each notificationQuestionItem As RepeaterItem In rptRecertOptionNotificationQuestions.Items
                        Dim hdNotificationQuestionEnabled = CType(notificationQuestionItem.FindControl("hdQuestionEnabled"), HiddenField)
                        If hdNotificationQuestionEnabled IsNot Nothing AndAlso Not String.IsNullOrEmpty(hdNotificationQuestionEnabled.Value) AndAlso Convert.ToBoolean(hdNotificationQuestionEnabled.Value) Then
                            Dim isNotificationChecked = CType(notificationQuestionItem.FindControl("chkRecertOptionNotificationResponse"), CheckBox).Checked
                            Dim yesAnswerId = CType(notificationQuestionItem.FindControl("hdYesAnswerId"), HiddenField).Value
                            Dim noAnswerId = CType(notificationQuestionItem.FindControl("hdNoAnswerId"), HiddenField).Value
                            Dim notificationAnswerId = If(isNotificationChecked, yesAnswerId, noAnswerId)
                            Dim notificationQuestionId = CType(notificationQuestionItem.FindControl("hdQuestionId"), HiddenField).Value
                            Dim responseResults As IIssuesCollection = AMCCertRecertController.AddOrUpdateResponse(notificationQuestionId, notificationAnswerId, "", DataAccessConstants.RECERTIFICATION_OPTION_NOTIFICATION_SURVEY_TITLE)
                            If responseResults IsNot Nothing AndAlso responseResults.Count > 0 Then
                                For Each responseIssue As IIssue In responseResults
                                    results.Add(responseIssue)
                                Next
                            End If
                        End If
                    Next
                End If
            End If
        End Sub

        ''' <summary>
        ''' Adds the or update recert option response.
        ''' </summary>
        ''' <param name="answerId">The answer id.</param>
        ''' <param name="questionId">The question id.</param>
        ''' <param name="results">The results.</param>
        Private Sub AddOrUpdateRecertOptionResponse(ByVal answerId As Long, ByVal questionId As String, ByVal results As IIssuesCollection)

            If answerId > 0 Then
                Dim responseResults As IIssuesCollection = AMCCertRecertController.AddOrUpdateRecertOptionResponse(questionId, answerId, "", DataAccessConstants.RECERTIFICATION_OPTION_SERVEY_TITLE)
                If responseResults IsNot Nothing AndAlso responseResults.Count > 0 Then
                    For Each responseIssue As IIssue In responseResults
                        results.Add(responseIssue)
                    Next
                Else
                    SetCurrentReCertOption(questionId)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Gets the answer id.
        ''' </summary>
        ''' <param name="questionItem">The question item.</param>
        ''' <param name="questionType">Type of the question.</param>
        ''' <param name="answerId">The answer id.</param>
        ''' <param name="isMultiAnswer">if set to <c>true</c> [is multi answer].</param>
        ''' <returns></returns>
        Private Function GetAnswerId(ByVal questionItem As RepeaterItem, ByVal questionType As String, ByVal answerId As Long, ByRef isMultiAnswer As Boolean, ByVal questionId As String) As Long
            If questionType.ToUpper() = "MULTI" Then
                For Each answerItem As RepeaterItem In rptRecertOptionAnswer.Items
                    Dim isAnswerChecked = CType(answerItem.FindControl("rdbRecertOptionAnswerItem"), CheckBox).Checked
                    Dim hdQuestionId = CType(answerItem.FindControl("hdQuestionId"), HiddenField).Value
                    If isAnswerChecked AndAlso hdQuestionId = questionId Then
                        answerId = Convert.ToInt32(CType(answerItem.FindControl("hdAnswerId"), HiddenField).Value)
                        Exit For
                    End If
                Next
                isMultiAnswer = True
            ElseIf questionType.ToUpper() = "YESNO" Then
                answerId = Convert.ToInt32(CType(questionItem.FindControl("hdYesAnswerId"), HiddenField).Value)
            End If
            Return answerId
        End Function

#End Region
    End Class
End Namespace