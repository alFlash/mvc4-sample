Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports AMC.DNN.Modules.CertRecert.Business.Entities
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Data.Enums
Imports AMC.DNN.Modules.CertRecert.Business.Controller

Imports Personify.ApplicationManager

Imports TIMSS.API.User.UserDefinedInfo
Imports TIMSS.API.Core.Validation

Namespace Controls.FormConfigurations
    Public Class FieldQuestionListUC
        Inherits PersonifyDNNBaseForm

#Region "Properties"
        Private _amcCertRecertController As AmcCertRecertController

        Public Property ParentModulePath() As String

        Public ReadOnly Property CurrentSurvey() As UserDefinedSurvey
            Get
                Dim surveys = _amcCertRecertController.GetSurveys(OrganizationId, OrganizationUnitId, MasterCustomerId)
                Dim fieldInfo = GetFieldConfigInfo()
                Dim survey = surveys.FindObject("Title", fieldInfo.SurveyTitle)
                If survey IsNot Nothing Then
                    Return CType(survey, UserDefinedSurvey)
                End If
                Return Nothing
            End Get
        End Property

        Public ReadOnly Property RecertificationCircleValidilityMonths As Integer
            Get
                Dim otherModuleSettings = ModuleConfigurationHelper.Instance.GetOtherModuleSettings(Server.MapPath(ParentModulePath))
                If otherModuleSettings IsNot Nothing AndAlso otherModuleSettings.ReCertificationCycle.HasValue Then
                    Return otherModuleSettings.ReCertificationCycle.Value
                End If
                Return 60 '
            End Get
        End Property

        Public ReadOnly Property DateTimeFormat As String
            Get
                Return CommonConstants.DATE_FORMAT
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the current question.
        ''' </summary>
        ''' <value>
        ''' The current question.
        ''' </value>
        Public ReadOnly Property CurrentQuestion() As IUserDefinedSurveyQuestion
            Get
                Dim surveys = _amcCertRecertController.GetSurveys(OrganizationId, OrganizationUnitId, MasterCustomerId)
                Dim fieldInfo = GetFieldConfigInfo()
                Dim survey = surveys.FindObject("Title", fieldInfo.SurveyTitle)
                Dim result As IUserDefinedSurveyQuestion
                If survey IsNot Nothing Then
                    result = CType(survey, UserDefinedSurvey).UserDefinedSurveyQuestions.CreateNew()
                    With result
                        .QuestionId = Convert.ToInt64(hdCurrentQuestionItem.Value)
                        .QuestionText = txtQuestionText.Text
                        .QuestionTypeString = GetQuestionType(GetFieldConfigInfo().QuestionType)
                    End With
                    Return result
                End If
                Return Nothing
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
            _amcCertRecertController = New AmcCertRecertController(OrganizationId, OrganizationUnitId, 0, Server.MapPath(ParentModulePath), MasterCustomerId, SubCustomerId)
            Try
                CheckUserRole()
                RegisterJavascript()
                AttachEventHandlers()
                lblMessage.Text = String.Empty
                If Not Page.IsPostBack Then
                    Dim fieldConfigInfo = GetFieldConfigInfo()
                    If fieldConfigInfo IsNot Nothing Then
                        If fieldConfigInfo.QuestionType.IndexOf(Enums.QuestionType.YESNO.ToString()) <> -1 Then
                            rdbQuestionType.Items.Add(New ListItem("Yes/No Answer", Enums.QuestionType.YESNO.ToString()))
                        End If
                        If fieldConfigInfo.QuestionType.IndexOf(Enums.QuestionType.MULTI.ToString()) <> -1 Then
                            rdbQuestionType.Items.Add(New ListItem("Multiple Answer", Enums.QuestionType.MULTI.ToString()))
                        End If
                        If fieldConfigInfo.QuestionType.IndexOf(Enums.QuestionType.RANGE.ToString()) <> -1 Then
                            rdbQuestionType.Items.Add(New ListItem("Range Answer", Enums.QuestionType.RANGE.ToString()))
                        End If
                        If rdbQuestionType.Items IsNot Nothing AndAlso rdbQuestionType.Items.Count > 0 Then
                            rdbQuestionType.Items(0).Selected = True
                        End If
                    End If
                End If

                LoadModuleConfigurations()
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Private Sub CheckUserRole()
            'OrElse UserInfo.IsInRole("PersonifyStaff")
            If Not (UserInfo.IsSuperUser OrElse UserInfo.IsInRole("Administrators") OrElse UserInfo.IsInRole("CERTADMIN") OrElse UserInfo.IsInRole("Host")) Then
                Response.Redirect(NavigateURL(), True)
            End If
        End Sub

        ''' <summary>
        ''' Called when [question list item command].
        ''' </summary>
        ''' <param name="source">The source.</param>
        ''' <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterCommandEventArgs" /> instance containing the event data.</param>
        Private Sub OnQuestionListItemCommand(ByVal source As Object, ByVal e As RepeaterCommandEventArgs)
            If e.CommandName = "DeleteQuestionItem" Then
                Dim surveys = _amcCertRecertController.GetSurveys(OrganizationId, OrganizationUnitId, MasterCustomerId)
                Dim fieldInfo = GetFieldConfigInfo()
                Dim survey = surveys.FindObject("Title", fieldInfo.SurveyTitle)
                Dim hdQuestionId = e.Item.FindControl("hdQuestionId")
                If hdQuestionId IsNot Nothing AndAlso survey IsNot Nothing Then
                    Dim responses = _amcCertRecertController.GetCustomerSurveyResponses(OrganizationId, OrganizationUnitId, CType(survey, IUserDefinedSurvey), Convert.ToInt64(CType(hdQuestionId, HiddenField).Value))
                    If responses Is Nothing OrElse responses.Count <= 0 Then
                        Dim issues = _amcCertRecertController.DeleteQuestion(surveys, CType(survey, IUserDefinedSurvey), Convert.ToInt64(CType(hdQuestionId, HiddenField).Value))
                        ReloadModuleConfigurations(issues)
                    Else
                        ShowErrorMessage(Localization.GetString("QuestionInUseError.Text", LocalResourceFile))
                    End If

                End If
            End If
        End Sub

        ''' <summary>
        ''' Called when [answer list item command].
        ''' </summary>
        ''' <param name="source">The source.</param>
        ''' <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterCommandEventArgs" /> instance containing the event data.</param>
        Private Sub OnAnswerListItemCommand(ByVal source As Object, ByVal e As RepeaterCommandEventArgs)
            If e.CommandName = "DeleteAnswerItem" Then
                Dim surveys = _amcCertRecertController.GetSurveys(OrganizationId, OrganizationUnitId, MasterCustomerId)
                Dim fieldInfo = GetFieldConfigInfo()
                Dim survey = surveys.FindObject("Title", fieldInfo.SurveyTitle)
                Dim hdQuestionId = e.Item.FindControl("hdAnswerQuestionId")
                Dim hdAnswerId = e.Item.FindControl("hdAnswerId")
                If hdQuestionId IsNot Nothing AndAlso survey IsNot Nothing AndAlso hdAnswerId IsNot Nothing Then
                    Dim responses = _amcCertRecertController.GetCustomerSurveyResponsesByAnswerId(OrganizationId, OrganizationUnitId, CType(survey, IUserDefinedSurvey), Convert.ToInt64(CType(hdQuestionId, HiddenField).Value), Convert.ToInt64(CType(hdAnswerId, HiddenField).Value))
                    If responses Is Nothing OrElse responses.Count <= 0 Then
                        Dim issues As IIssuesCollection = _amcCertRecertController.DeleteAnswer(surveys, CType(survey, IUserDefinedSurvey),
                                                              Convert.ToInt64(CType(hdQuestionId, HiddenField).Value),
                                                              Convert.ToInt64(CType(hdAnswerId, HiddenField).Value))
                        ReloadModuleConfigurations(issues)
                    Else
                        ShowErrorMessage(Localization.GetString("AnswerInUseError.Text", LocalResourceFile))
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Called when [question list item data bound].
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterItemEventArgs" /> instance containing the event data.</param>
        Private Sub OnQuestionListItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
            Dim question = CType(e.Item.DataItem, UserDefinedSurveyQuestion)
            Dim fieldInfo = GetFieldConfigInfo()
            SetQuestionsRelationshipIndicator(e, question, fieldInfo)
            BindAnswersToQuestion(e, question)
        End Sub

        Private Sub BindAnswersToQuestion(ByVal e As RepeaterItemEventArgs, ByVal question As UserDefinedSurveyQuestion)
            Dim answerControl = e.Item.FindControl("rptAnswerList")
            If answerControl IsNot Nothing Then
                Dim answerRepater = CType(answerControl, Repeater)
                AddHandler answerRepater.ItemDataBound, AddressOf OnAnswerListItemDataBound
                AddHandler answerRepater.ItemCommand, AddressOf OnAnswerListItemCommand
                answerRepater.DataSource = question.UserDefinedSurveyAnsweres
                answerRepater.DataBind()
            End If
        End Sub

        Private Sub SetQuestionsRelationshipIndicator(ByVal e As RepeaterItemEventArgs, ByVal question As UserDefinedSurveyQuestion, ByVal fieldInfo As FieldInfo)
            If question IsNot Nothing AndAlso Not String.IsNullOrEmpty(question.QuestionCode) Then
                Select Case question.QuestionCode
                    Case Enums.QuestionCode.RECERT_OPTION_RETAKE.ToString()
                        SetQuestionsRelationshipIndicator(e, fieldInfo, "Option1Indicator")
                        Exit Select
                    Case Enums.QuestionCode.RECERT_OPTION_2.ToString()
                        SetQuestionsRelationshipIndicator(e, fieldInfo, "Option2Indicator")
                        Exit Select
                    Case Enums.QuestionCode.RECERT_OPTION_3.ToString()
                        SetQuestionsRelationshipIndicator(e, fieldInfo, "Option3Indicator")
                        Exit Select
                End Select
            End If
        End Sub

        Private Sub SetQuestionsRelationshipIndicator(ByVal args As RepeaterItemEventArgs, ByVal fieldInfo As FieldInfo, ByVal resourcekey As String)
            Dim lblQuestionRelationshipIndicators = CType(args.Item.FindControl("lblQuestionRelationshipIndicators"), Label)
            If fieldInfo.SurveyTitle = DataAccessConstants.RECERTIFICATION_OPTION_SERVEY_TITLE Then
                lblQuestionRelationshipIndicators.Text = Localization.GetString(String.Format("{0}{1}.Text", resourcekey, "RecertOptionSurvey"), LocalResourceFile) + "<br/>"
            ElseIf fieldInfo.SurveyTitle = DataAccessConstants.RECERTIFICATION_DECLARATION_SURVEY_TITLE Then
                lblQuestionRelationshipIndicators.Text = Localization.GetString(String.Format("{0}{1}.Text", resourcekey, "RecertDeclarationSurvey"), LocalResourceFile) + "<br/>"
            End If
        End Sub

        Private Sub OnAnswerListItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
            Dim answerItem = CType(e.Item.DataItem, UserDefinedSurveyAnswers)
            If (CurrentSurvey.Title = DataAccessConstants.CERTIFICATION_EXAM_CHOICE_SURVEY_TITLE OrElse CurrentSurvey.Title = DataAccessConstants.RECERTIFICATION_OPTION_SERVEY_TITLE) _
                 AndAlso answerItem IsNot Nothing AndAlso answerItem.AnswerTypeString = "YES" Then
                Dim lblExamStartDate = e.Item.FindControl("lblExamStartDate")
                Dim lblExamEndDate = e.Item.FindControl("lblExamEndDate")
                Dim lblExamApplicationDeadline = e.Item.FindControl("lblExamApplicationDeadline")
                Dim lblProductCode = e.Item.FindControl("lblProductCode")
                Dim lblApplicationProductId = e.Item.FindControl("lblApplicationProductId")
                If lblExamStartDate IsNot Nothing Then
                    Dim answerId = CType(e.Item.FindControl("hdAnswerId"), HiddenField).Value
                    Dim controler = New AmcCertRecertController(OrganizationId, OrganizationUnitId, 0, Server.MapPath(ParentModulePath), MasterCustomerId, SubCustomerId)
                    Dim examPeriod = controler.GetCertAppExamPeriodByQuestionId(Convert.ToInt64(answerId))
                    If (examPeriod IsNot Nothing) Then
                        CType(lblExamStartDate, Label).Text = examPeriod.StartDate.ToString(CommonConstants.DATE_FORMAT)
                        CType(lblExamEndDate, Label).Text = examPeriod.EndDate.ToString(CommonConstants.DATE_FORMAT)
                        CType(lblExamApplicationDeadline, Label).Text = examPeriod.ExamDate.ToString(CommonConstants.DATE_FORMAT)
                        CType(lblProductCode, Label).Text = examPeriod.ExamProductId.ToString()
                        CType(lblApplicationProductId, Label).Text = examPeriod.ApplicationProductId.ToString()
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Buttons the save answer click.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Private Sub ButtonSaveAnswerClick(ByVal sender As Object, ByVal e As EventArgs)
            Dim answerText = txtAnswerText.Text
            Dim startDate = If(Not String.IsNullOrEmpty(txtAnswerStartDate.Text), Convert.ToDateTime(txtAnswerStartDate.Text), Nothing)
            Dim endDate = If(Not String.IsNullOrEmpty(txtAnswerEndDate.Text), Convert.ToDateTime(txtAnswerEndDate.Text), Nothing)
            Dim applicationDeadline = If(Not String.IsNullOrEmpty(txtAnswerApplicationDeadline.Text), Convert.ToDateTime(txtAnswerApplicationDeadline.Text), Nothing)
            Dim productCode = txtProductCode.Text
            Dim applicationProductId = txtApplicationProductId.Text

            Dim fieldInfo As FieldInfo = GetFieldConfigInfo()
            Dim issues As IIssuesCollection
            If fieldInfo IsNot Nothing Then
                Dim surveys = _amcCertRecertController.GetSurveys(OrganizationId, OrganizationUnitId, MasterCustomerId)
                For Each survey As IUserDefinedSurvey In surveys
                    If survey.Title = fieldInfo.SurveyTitle AndAlso Not String.IsNullOrEmpty(hdCurrentQuestionItem.Value) Then
                        Dim questionId = Convert.ToInt64(hdCurrentQuestionItem.Value)
                        Try
                            If String.IsNullOrEmpty(hdCurrentAnswerItem.Value) Then 'Add
                                If (CurrentSurvey.Title = DataAccessConstants.CERTIFICATION_EXAM_CHOICE_SURVEY_TITLE OrElse CurrentSurvey.Title = DataAccessConstants.RECERTIFICATION_OPTION_SERVEY_TITLE) _
                                    AndAlso hdCurrentAnswerTypeString.Value = "YES" Then
                                    issues = _amcCertRecertController.AddMultiAnswer(OrganizationId, OrganizationUnitId, surveys, survey, questionId, startDate, endDate, applicationDeadline, productCode, applicationProductId)
                                ElseIf Not String.IsNullOrEmpty(answerText) Then
                                    issues = _amcCertRecertController.AddMultiAnswer(surveys, survey, questionId, answerText, Enums.QuestionType.MULTI.ToString())
                                End If
                            Else 'Edit
                                If (CurrentSurvey.Title = DataAccessConstants.CERTIFICATION_EXAM_CHOICE_SURVEY_TITLE OrElse CurrentSurvey.Title = DataAccessConstants.RECERTIFICATION_OPTION_SERVEY_TITLE) _
                                    AndAlso hdCurrentAnswerTypeString.Value = "YES" Then
                                    issues = _amcCertRecertController.UpdateMultiAnswer(OrganizationId, OrganizationUnitId, surveys, survey, questionId, Convert.ToInt64(hdCurrentAnswerItem.Value), startDate, endDate, applicationDeadline, productCode, applicationProductId)
                                ElseIf Not String.IsNullOrEmpty(answerText) Then
                                    issues = _amcCertRecertController.UpdateMultiAnswerText(surveys, survey, questionId, Convert.ToInt64(hdCurrentAnswerItem.Value), answerText)
                                End If

                            End If
                        Catch ex As Exception
                            lblMessage.Text = ex.Message
                        End Try
                    End If
                Next
            End If
            ReloadModuleConfigurations(issues)
        End Sub

        ''' <summary>
        ''' Handles the button OK event click - Save the new question item/Update existing question item
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Private Sub ButtonOKClick(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Dim questionText = txtQuestionText.Text
                Dim issues As IIssuesCollection = Nothing
                Dim fieldInfo As FieldInfo = GetFieldConfigInfo()
                If fieldInfo IsNot Nothing AndAlso Not String.IsNullOrEmpty(questionText) Then
                    Dim surveys = _amcCertRecertController.GetSurveys(OrganizationId, OrganizationUnitId, MasterCustomerId)
                    For Each survey As IUserDefinedSurvey In surveys
                        If survey.Title = fieldInfo.SurveyTitle Then
                            If String.IsNullOrEmpty(hdCurrentQuestionItem.Value) AndAlso fieldInfo.CanAddQuestion Then 'Add new
                                Dim questionType = GetQuestionType(fieldInfo.QuestionType)
                                If questionType = Enums.QuestionType.YESNO.ToString() Then
                                    issues = _amcCertRecertController.AddYesNoQuestion(surveys, survey, questionText, True) 'IsEnabled
                                ElseIf questionType = Enums.QuestionType.RANGE.ToString() Then
                                    issues = _amcCertRecertController.AddRangeQuestion(surveys, survey, questionText, True) 'IsEnabled
                                ElseIf questionType = Enums.QuestionType.MULTI.ToString() Then
                                    issues = _amcCertRecertController.AddQuestion(surveys, survey, questionText, questionType, True) 'IsEnabled
                                End If
                            Else 'Update
                                issues = _amcCertRecertController.UpdateQuestion(surveys, survey, CurrentQuestion)
                            End If
                            Exit For
                        End If
                    Next
                End If
                ReloadModuleConfigurations(issues)
            Catch ex As Exception
                ProcessPageLoadException(ex)
            End Try
        End Sub


        ''' <summary>
        ''' Buttons the save click.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Private Sub ButtonSaveClick(ByVal sender As Object, ByVal e As EventArgs)
            Try
                'todo: save isEnabled
                Dim issues = New IssuesCollection()
                Dim surveys = _amcCertRecertController.GetSurveys(OrganizationId, OrganizationUnitId, MasterCustomerId)
                Dim fieldInfo As FieldInfo = GetFieldConfigInfo()
                If fieldInfo IsNot Nothing Then
                    For Each survey As IUserDefinedSurvey In surveys
                        If survey.Title = fieldInfo.SurveyTitle Then
                            If rptQuestionList.Items IsNot Nothing AndAlso rptQuestionList.Items.Count > 0 Then
                                For Each repeaterItem As RepeaterItem In rptQuestionList.Items
                                    Dim hdQuestionId = CType(repeaterItem.FindControl("hdQuestionId"), HiddenField)
                                    Dim chkIsEnabled = CType(repeaterItem.FindControl("chkIsEnabled"), CheckBox)
                                    Dim issue = _amcCertRecertController.UpdateQuestion(surveys, survey, hdQuestionId.Value, chkIsEnabled.Checked)
                                    If issue IsNot Nothing AndAlso issue.Count > 0 Then
                                        For Each issue1 As IIssue In issue
                                            issues.Add(issue1)
                                        Next
                                    End If
                                Next
                            End If
                            Exit For
                        End If
                    Next
                End If
                surveys.Save()
                ReloadModuleConfigurations(issues)
            Catch ex As Exception
                ProcessPageLoadException(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Buttons the back click.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Private Sub ButtonBackClick(ByVal sender As Object, ByVal e As EventArgs)
            Response.Redirect(NavigateURL(TabId, "", String.Format("uc={0}", CommonConstants.CONFIGURATION_FORM_PATH)))
        End Sub
#End Region

#Region "Private Methods"
        ''' <summary>
        ''' Gets the type of the question.
        ''' </summary>
        ''' <param name="questionType">The survey title.</param>
        ''' <returns></returns>
        Private Function GetQuestionType(ByVal questionType As String) As String
            Dim result = Enums.QuestionType.YESNO.ToString()
            If (questionType.IndexOf(Enums.QuestionType.MULTI.ToString()) <> -1 OrElse questionType.IndexOf(Enums.QuestionType.RANGE.ToString()) <> -1) AndAlso rdbQuestionType IsNot Nothing Then
                result = rdbQuestionType.SelectedValue
            End If
            Return result
        End Function

        ''' <summary>
        ''' Registers the javascript.
        ''' </summary>
        Private Sub RegisterJavascript()
            Page.ClientScript.RegisterClientScriptInclude("fieldconfig", String.Format("{0}/Documentation/scripts/QuestionListConfigurations.js?v={1}", ParentModulePath, CommonConstants.CURRENT_VERSION))
        End Sub

        ''' <summary>
        ''' Attaches the event handlers.
        ''' </summary>
        Private Sub AttachEventHandlers()
            AddHandler rptQuestionList.ItemDataBound, AddressOf OnQuestionListItemDataBound
            AddHandler rptQuestionList.ItemCommand, AddressOf OnQuestionListItemCommand
            AddHandler btnOK.Click, AddressOf ButtonOKClick
            AddHandler btnSave.Click, AddressOf ButtonSaveClick
            AddHandler btnBack.Click, AddressOf ButtonBackClick
            AddHandler btnSaveAnswer.Click, AddressOf ButtonSaveAnswerClick
            AddHandler btnSaveExamChoiceAnswer.Click, AddressOf ButtonSaveAnswerClick
        End Sub

        ''' <summary>
        ''' Loads the module configurations.
        ''' </summary>
        Private Sub LoadModuleConfigurations()
            Dim fieldInfo As FieldInfo = GetFieldConfigInfo()
            lblTitle.Text = Localization.GetString("QuestionListConfigurationTitle.Text", LocalResourceFile)
            If fieldInfo IsNot Nothing AndAlso fieldInfo.IsEnabled AndAlso Not String.IsNullOrEmpty(fieldInfo.SurveyTitle) Then
                Dim surveys As IUserDefinedSurveys = _amcCertRecertController.GetSurveys(OrganizationId, OrganizationUnitId, MasterCustomerId)
                Dim surveyObj As Object
                surveyObj = surveys.FindObject("Title", fieldInfo.SurveyTitle)
                If surveyObj IsNot Nothing Then
                    ''set survey title for form title
                    lblTitle.Text = fieldInfo.SurveyTitle
                    Dim survey = CType(surveyObj, UserDefinedSurvey)
                    rptQuestionList.DataSource = survey.UserDefinedSurveyQuestions
                    rptQuestionList.DataBind()
                End If
                'End If
            Else
                Response.Redirect(NavigateURL(), True)
            End If
        End Sub

        ''' <summary>
        ''' Gets the field config info.
        ''' </summary>
        ''' <returns></returns>
        Public Function GetFieldConfigInfo() As FieldInfo
            Dim fieldInfo As FieldInfo
            Dim formId = Request.QueryString("formId")
            Dim sectionId = Request.QueryString("sectionId")
            Dim fieldId = Request.QueryString("fieldId")
            Dim sessionKey = String.Format("_SS_AMC_Config_Form{0}_Section{1}_Field{2}", formId, sectionId, fieldId)
            If HttpContext.Current.Session(sessionKey) IsNot Nothing Then
                fieldInfo = CType(HttpContext.Current.Session(sessionKey), FieldInfo)
            Else
                Dim configurations = ModuleConfigurationHelper.Instance.GetFormConfigurations(ModuleId,
                                                                                              Server.MapPath(ParentModulePath),
                                                                                              OrganizationId, OrganizationUnitId)
                fieldInfo = GetFieldInfoFromConfigs(configurations, formId, sectionId, fieldId)
                HttpContext.Current.Session(sessionKey) = fieldInfo
            End If
            Return fieldInfo
        End Function

        ''' <summary>
        ''' Initializes the survey.
        ''' </summary>
        ''' <param name="surveys">The surveys.</param>
        ''' <param name="surveyTitle">The survey title.</param>
        ''' <returns></returns>
        Public Function InitializeSurvey(ByRef surveys As IUserDefinedSurveys, ByVal surveyTitle As String) As IIssuesCollection
            If surveys Is Nothing OrElse surveys.Count <= 0 Then
                Return _amcCertRecertController.CreateSurveyByTitle(surveyTitle, OrganizationId, OrganizationUnitId, MasterCustomerId)
            Else
                Dim surveyObj = surveys.FindObject("Title", surveyTitle)
                If surveyObj Is Nothing Then
                    Return _amcCertRecertController.CreateSurveyByTitle(surveyTitle, OrganizationId, OrganizationUnitId, MasterCustomerId)
                End If
            End If
            Return Nothing
        End Function

        ''' <summary>
        ''' Gets the field config info.
        ''' </summary>
        ''' <param name="formInfos">The form infos.</param>
        ''' <param name="formId">The form id.</param>
        ''' <param name="sectionId">The section id.</param>
        ''' <param name="fieldId">The field id.</param>
        ''' <returns></returns>
        Private Shared Function GetFieldInfoFromConfigs(ByVal formInfos As IEnumerable(Of FormInfo), ByVal formId As String, ByVal sectionId As String, ByVal fieldId As String) As FieldInfo
            If formInfos IsNot Nothing Then
                For Each formInfo As FormInfo In formInfos
                    If formId = formInfo.FormId AndAlso formInfo.Sections IsNot Nothing Then
                        For Each sectionInfo As SectionInfo In formInfo.Sections
                            If sectionInfo.SectionId = sectionId AndAlso sectionInfo.Fields IsNot Nothing Then
                                For Each fieldInfo As FieldInfo In sectionInfo.Fields
                                    If fieldInfo.FieldId = fieldId Then
                                        Return fieldInfo
                                    End If
                                Next
                                Exit For
                            End If
                        Next
                        Exit For
                    End If
                Next
            End If
            Return Nothing
        End Function

        ''' <summary>
        ''' Reloads the module configurations.
        ''' </summary>
        ''' <param name="issuesCollection">The issues collection.</param>
        Private Sub ReloadModuleConfigurations(ByVal issuesCollection As IIssuesCollection)
            lblMessage.Text = String.Empty
            If issuesCollection Is Nothing OrElse issuesCollection.Count <= 0 Then
                LoadModuleConfigurations()
            Else
                For Each issue As IIssue In issuesCollection
                    lblMessage.Text += issue.Message + "<br/>"
                Next
                issuesCollection.RemoveAll()
            End If
        End Sub

        ''' <summary>
        ''' Shows the error message.
        ''' </summary>
        ''' <param name="message">The message.</param>
        Private Sub ShowErrorMessage(ByVal message As String)
            lblMessage.Text = message
        End Sub
#End Region

    End Class
End Namespace