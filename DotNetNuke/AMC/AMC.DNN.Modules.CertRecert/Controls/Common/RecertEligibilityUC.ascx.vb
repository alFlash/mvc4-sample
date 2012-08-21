Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Data.Enums
Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Business.BusinessValidation
Imports AMC.DNN.Modules.CertRecert.Business.IControls
Imports TIMSS.API.Core

Imports TIMSS.API.Core.Validation
Imports TIMSS.API.User.UserDefinedInfo

Namespace Controls.Common
    Public Class RecertEligibilityUC
        Inherits SectionBaseUserControl
        Implements IReload, ISave
#Region "Private member"
        Private ReadOnly Property EnableHiddenLicensureValidationRule() As Boolean
            Get
                Try
                    If CurrentCertificationCustomerCertification.CertificationTypeCodeString.Equals(
                                    CertificationTypeEnum.CERTIFICATION.ToString()) Then
                        Return False
                    Else
                        Return ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                            ValidationRuleId.RECERT_INVISIBLE_LICENSURE_IN_DECLARATION.ToString(),
                                                                            Server.MapPath(ParentModulePath))
                    End If
                Catch ex As Exception
                    Return False
                End Try
            End Get

        End Property

#End Region
#Region "Event Handler"
        Protected Sub Pre_Render(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
          
        End Sub
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            Try
                cpWithToday.ValueToCompare = DateTime.Now.ToString(CommonConstants.DATE_FORMAT)

            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Protected Sub RptLicensureItemCommand(ByVal source As Object,
                                               ByVal e As Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptLicensure.ItemCommand
            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
            Try
                If e.CommandName.Equals("Delete") Then
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                    Dim customerExternalDocumentItem As IUserDefinedCustomerExternalDocumentation
                    customerExternalDocumentItem =
                        AMCCertRecertController.GetCustomerExternalDocumentationByGUID(
                                                                e.CommandArgument.ToString(),
                                                                DocumentationType.LICENSURE.ToString(),
                                                                MasterCustomerId,
                                                                SubCustomerId)
                    AMCCertRecertController.DeleteCustomerExternalDocument(customerExternalDocumentItem)
                End If
                BindingDataToList()
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Protected Sub RptLicensureItemDataBound(ByVal sender As Object,
                                                 ByVal e As Web.UI.WebControls.RepeaterItemEventArgs) Handles rptLicensure.ItemDataBound
            Try
                If e.Item.ItemType = ListItemType.Item OrElse
                                e.Item.ItemType = ListItemType.AlternatingItem Then
                    Dim customerExternalDocument As UserDefinedCustomerExternalDocumentation =
                                            CType(e.Item.DataItem, 
                                                  UserDefinedCustomerExternalDocumentation)
                    If customerExternalDocument IsNot Nothing Then
                        Dim lblStateProvince = CType(e.Item.FindControl("lblStateOf"), Label)
                        lblStateProvince.Text =
                                    customerExternalDocument.IssuingBody.List(customerExternalDocument.IssuingBodyString).Description
                        ''set date

                        Dim originalIssueDate = CType(e.Item.FindControl("lblDateOfOriginalIssue"), Label)
                        If customerExternalDocument.InitialIssueDate <> DateTime.MinValue Then
                            originalIssueDate.Text = customerExternalDocument.InitialIssueDate.ToString(CommonConstants.DATE_FORMAT)
                        End If
                        Dim expirationDateLabel = CType(e.Item.FindControl("lblExpirationDate"), Label)
                        If customerExternalDocument.CycleEndDate <> DateTime.MinValue Then
                            expirationDateLabel.Text = customerExternalDocument.CycleEndDate.ToString(CommonConstants.DATE_FORMAT)
                        End If
                    End If

                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub
        Protected Sub RptAgreementItemDataBound(ByVal sender As Object, ByVal e As Web.UI.WebControls.RepeaterItemEventArgs) Handles rptAgreement.ItemDataBound
            Try
                If e.Item.ItemType = ListItemType.Item OrElse
                      e.Item.ItemType = ListItemType.AlternatingItem Then
                    Dim chkYesNo = CType(e.Item.FindControl("chkQuestion"), CheckBox)
                    Dim hdAnswerYes = CType(e.Item.FindControl("hdAnswerYes"), HiddenField)
                    Dim hdAnswerNo = CType(e.Item.FindControl("hdAnswerNo"), HiddenField)
                    Dim hdResponseId = CType(e.Item.FindControl("hdResponseId"), HiddenField)
                    Dim surveyQuestion = CType(e.Item.DataItem, UserDefinedSurveyQuestion)
                    Dim surveyResponse =
                        AMCCertRecertController.GetCustomerSurveyResponseByQuestionId(
                                                                    surveyQuestion.QuestionId,
                                                                    surveyQuestion.SurveyId,
                                                                    MasterCustomerId,
                                                                    SubCustomerId)

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
        Protected Sub RptQuestionnaireItemDataBound(ByVal sender As Object, ByVal e As Web.UI.WebControls.RepeaterItemEventArgs) Handles rptQuestionnaire.ItemDataBound
            Try
                If e.Item.ItemType = ListItemType.Item OrElse
                      e.Item.ItemType = ListItemType.AlternatingItem Then

                    Dim rdlAnswer = CType(e.Item.FindControl("rdlAnswer"), RadioButtonList)
                    Dim hdAnswerYes = CType(e.Item.FindControl("hdAnswerYes"), HiddenField)
                    Dim hdAnswerNo = CType(e.Item.FindControl("hdAnswerNo"), HiddenField)
                    Dim hdResponseId = CType(e.Item.FindControl("hdResponseId"), HiddenField)
                    Dim liQuestionItem = e.Item.FindControl("liQuestionItem")
                    Dim surveyQuestion = CType(e.Item.DataItem, UserDefinedSurveyQuestion)

                    Dim surveyResponse =
                        AMCCertRecertController.GetCustomerSurveyResponseByQuestionId(
                                                                    surveyQuestion.QuestionId,
                                                                    surveyQuestion.SurveyId,
                                                                    MasterCustomerId,
                                                                    SubCustomerId)

                    If (surveyQuestion.QuestionCode.Equals(Enums.QuestionCode.CERT_DECLARATION_AGREEMENT.ToString()) OrElse
                      surveyQuestion.QuestionCode.Equals(Enums.QuestionCode.RECERT_DECLARATION_AGREEMENT.ToString())) Then
                        liQuestionItem.Visible = False
                        Return
                    End If

                    rdlAnswer.DataTextField = "AnswerText"
                    rdlAnswer.DataValueField = "AnswerId"
                    rdlAnswer.DataSource = surveyQuestion.UserDefinedSurveyAnsweres
                    rdlAnswer.DataBind()

                    If surveyResponse IsNot Nothing Then
                        hdResponseId.Value = surveyResponse.ResponseId.ToString()
                        rdlAnswer.SelectedValue = surveyResponse.AnswerId.ToString()
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

                    ''logic for genarate control in case of recertification flow
                    If Not CurrentCertificationCustomerCertification.CertificationTypeCodeString.Equals(
                                                CertificationTypeEnum.CERTIFICATION.ToString()) Then
                        If Not String.IsNullOrEmpty(surveyQuestion.QuestionCode) Then
                            Dim currentSelectedOption = GetCurrentReCertOption()
                            If currentSelectedOption IsNot Nothing Then
                                If Not currentSelectedOption.Enabled OrElse Not currentSelectedOption.QuestionCode.Equals(surveyQuestion.QuestionCode) Then
                                    liQuestionItem.Visible = False
                                End If
                            Else
                                If surveyQuestion.QuestionCode.Equals(Enums.QuestionCode.RECERT_OPTION_RETAKE.ToString()) OrElse
                                    surveyQuestion.QuestionCode.Equals(Enums.QuestionCode.RECERT_OPTION_2.ToString()) OrElse
                                    surveyQuestion.QuestionCode.Equals(Enums.QuestionCode.RECERT_OPTION_3.ToString()) Then
                                    liQuestionItem.Visible = False
                                End If
                            End If
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
                results = CType(SaveCollectedDataFromUser(), IssuesCollection)
                If results Is Nothing OrElse results.Count <= 0 Then
                    BindingDataToControl()
                    results = CheckBusinessValidation()
                    If (results.Count < 1) Then
                        hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                        AMCCertRecertController.RefreshCustomerExternalDocuments(DocumentationType.LICENSURE.ToString(),
                                                                                MasterCustomerId,
                                                                                SubCustomerId)
                    Else
                        hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                    End If
                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
            Return results
        End Function

        Private Function CheckBusinessValidation() As IssuesCollection
            Dim iissuesColection As New IssuesCollection
            Dim fieldQuestionList = GetFieldInfo("QuestionList")
            If fieldQuestionList Is Nothing OrElse Not fieldQuestionList.IsEnabled Then
                Return iissuesColection
            End If
            Dim validationEnabled = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                    ValidationRuleId.DECLARATION_ALL_QUESTION_COMPLETED.ToString(),
                                                                    Server.MapPath(ParentModulePath))
            Dim licensureValidationEnabled = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                    ValidationRuleId.LICENSURE_AT_LEAST_ONE_ROW.ToString(),
                                                                    Server.MapPath(ParentModulePath))

            If validationEnabled Then
                For Each rptItem As RepeaterItem In rptQuestionnaire.Items
                    Dim hdQuestionEnabled = CType(rptItem.FindControl("hdQuestionEnabled"), HiddenField)
                    If hdQuestionEnabled IsNot Nothing AndAlso Not String.IsNullOrEmpty(hdQuestionEnabled.Value) AndAlso Convert.ToBoolean(hdQuestionEnabled.Value) Then
                        Dim rdlAnswer = CType(rptItem.FindControl("rdlAnswer"), RadioButtonList)
                        Dim hdResponseId = CType(rptItem.FindControl("hdResponseId"), HiddenField)
                        Dim hdQuestionCode = CType(rptItem.FindControl("hdQuestionCode"), HiddenField)
                        If hdResponseId.Value.Equals("0") Or String.IsNullOrEmpty(rdlAnswer.SelectedValue) Then
                            If hdQuestionCode.Value.Equals(Enums.QuestionCode.CERT_DECLARATION_AGREEMENT.ToString()) OrElse
                                hdQuestionCode.Value.Equals(Enums.QuestionCode.RECERT_DECLARATION_AGREEMENT) Then
                                Continue For
                            End If
                            If Not CurrentCertificationCustomerCertification.CertificationTypeCodeString.Equals(
                                                    CertificationTypeEnum.CERTIFICATION.ToString()) Then
                                Dim currentSelectedOption = GetCurrentReCertOption()
                                If currentSelectedOption IsNot Nothing Then
                                    If currentSelectedOption.QuestionCode.Equals(hdQuestionCode.Value) OrElse
                                        String.IsNullOrEmpty(hdQuestionCode.Value) Then
                                        iissuesColection.Add(New CheckBoxMustSelectedIssue(New BusinessObject(),
                                                                             Localization.GetString("CheckBoxMustSelectedIssue.Text",
                                                                                                    LocalResourceFile)))
                                    End If
                                End If
                            Else
                                iissuesColection.Add(New CheckBoxMustSelectedIssue(New BusinessObject(),
                                                                             Localization.GetString("CheckBoxMustSelectedIssue.Text",
                                                                                                    LocalResourceFile)))
                                Exit For
                            End If
                        End If
                    End If
                    
                Next
            End If

            If licensureValidationEnabled AndAlso Not EnableHiddenLicensureValidationRule Then
                If rptLicensure.Items Is Nothing OrElse rptLicensure.Items.Count < 1 Then
                    iissuesColection.Add(New AtLeastOneRecordIssue(New BusinessObject(),
                                                                   Localization.GetString("LicensureAtLeastOneRecord.Text",
                                                                                          LocalResourceFile)))
                End If
            End If

            For Each rptItem As RepeaterItem In rptAgreement.Items
                Dim hdQuestionEnabled = CType(rptItem.FindControl("hdQuestionEnabled"), HiddenField)
                If hdQuestionEnabled IsNot Nothing AndAlso Not String.IsNullOrEmpty(hdQuestionEnabled.Value) AndAlso Convert.ToBoolean(hdQuestionEnabled.Value) Then
                    Dim chkYesNo = CType(rptItem.FindControl("chkQuestion"), CheckBox)
                    If chkYesNo.Checked = False Then
                        iissuesColection.Add(New CheckBoxMustSelectedIssue(New BusinessObject(),
                                                                           Localization.GetString("CheckBoxAgreementMustCheck.Text",
                                                                                                  LocalResourceFile)))
                        Exit For
                    End If
                End If
            Next

            Return iissuesColection
        End Function

        Protected Sub BtnSaveClick(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
            Dim customerExternals As New UserDefinedCustomerExternalDocumentations
            Dim customerExternalDocumentItem As IUserDefinedCustomerExternalDocumentation
            Dim issueCollection As IIssuesCollection
            Try
                ''edit 
                If (hdCurrentObjectUniqueId.Value <> String.Empty) Then
                    customerExternalDocumentItem =
                        AMCCertRecertController.GetCustomerExternalDocumentationByGUID(
                                                                hdCurrentObjectUniqueId.Value,
                                                                DocumentationType.LICENSURE.ToString(),
                                                                MasterCustomerId,
                                                                SubCustomerId)
                    SetValueForCustomerExternalDocument(customerExternalDocumentItem)
                    issueCollection = AMCCertRecertController.UpdateCustomerExternalDocument(customerExternalDocumentItem)
                Else
                    ''Insert
                    customerExternalDocumentItem = customerExternals.CreateNew()
                    customerExternalDocumentItem.DocumentationTypeString =
                                            DocumentationType.LICENSURE.ToString()
                    customerExternalDocumentItem.IsNewObjectFlag = True
                    customerExternalDocumentItem.IssuingBody.FillList()
                    ''set properties
                    SetValueForCustomerExternalDocument(customerExternalDocumentItem)
                    ''call insert fucntion of amc controller
                    issueCollection = AMCCertRecertController.InsertCustomerExternalDocument(customerExternalDocumentItem)

                End If
                If issueCollection IsNot Nothing Then
                    If issueCollection.Count > 0 Then
                        ShowError(issueCollection, lblPopupMessage)
                        hdIsValidateFailed.Value = CommonConstants.TAB_INCOMPLETED
                    Else
                        hdIsValidateFailed.Value = CommonConstants.TAB_COMPLETED
                    End If
                Else
                    hdIsValidateFailed.Value = CommonConstants.TAB_COMPLETED
                End If
                BindingDataToList()
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub
#End Region

#Region "Private Method"
        Public Overrides Sub ValidateFormFillCompleted()
            If Page.IsPostBack Then
                BindingDataToControl()
            Else
                AMCCertRecertController.RefreshCustomerExternalDocuments(DocumentationType.LICENSURE.ToString(),
                                                                         MasterCustomerId,
                                                                         SubCustomerId)
                BindingDataToControl()
                Dim issues = CheckBusinessValidation()
                If issues.Count < 1 Then
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                End If
            End If
        End Sub

        Private Function SetPropertiesForCustomerResponse(ByVal rptItem As RepeaterItem,
                                                     ByRef customerResponse As IUserDefinedCustomerSurveyResponses) As Boolean

            Dim result As Boolean = True
            Dim surveyIdValue = Long.Parse(hdSurveyId.Value)
            Dim chkYesNo = CType(rptItem.FindControl("chkQuestion"), CheckBox)
            Dim hdAnswerYes = CType(rptItem.FindControl("hdAnswerYes"), HiddenField)
            Dim hdAnswerNo = CType(rptItem.FindControl("hdAnswerNo"), HiddenField)
            Dim hdQuestionId = CType(rptItem.FindControl("hdQuestionId"), HiddenField)
            Dim questionIdValue = Long.Parse(hdQuestionId.Value)
            Dim rdlAnswer = CType(rptItem.FindControl("rdlAnswer"), RadioButtonList)

            For Each issue As IIssue In customerResponse.ValidationIssuesForMe
                customerResponse.ValidationIssues.Remove(issue)
            Next

            With customerResponse
                .MasterCustomerId = MasterCustomerId
                .SubcustomerId = SubCustomerId
                .SurveyId = surveyIdValue
                .QuestionId = questionIdValue
            End With
            If chkYesNo Is Nothing Then
                Long.TryParse(rdlAnswer.SelectedValue, customerResponse.AnswerId)
            Else
                If chkYesNo.Checked Then
                    customerResponse.AnswerId = Long.Parse(hdAnswerYes.Value)
                Else
                    customerResponse.AnswerId = Long.Parse(hdAnswerNo.Value)
                End If
            End If
            CheckBoxMustSelectedIssue.Assert(customerResponse.AnswerId <> Long.Parse(hdAnswerYes.Value),
                                             customerResponse,
                                             Localization.GetString("CheckBoxMustSelectedIssue.Text", LocalResourceFile))
            If customerResponse.ValidationIssuesForMe.Count > 0 Then
                result = False
            End If
            Return result
        End Function

        Private Function SaveCustomerResponsees() As IIssuesCollection

            Dim issueColletion As New IssuesCollection
            Dim fieldQuestionList = GetFieldInfo("QuestionList")
            If fieldQuestionList IsNot Nothing AndAlso fieldQuestionList.IsEnabled Then
                Dim surveyIdValue = Long.Parse(hdSurveyId.Value)

                For Each rptItem As RepeaterItem In rptQuestionnaire.Items
                    Dim hdQuestionEnabled = CType(rptItem.FindControl("hdQuestionEnabled"), HiddenField)
                    If hdQuestionEnabled IsNot Nothing AndAlso Not String.IsNullOrEmpty(hdQuestionEnabled.Value) AndAlso Convert.ToBoolean(hdQuestionEnabled.Value) Then
                        Dim hdResponseId = CType(rptItem.FindControl("hdResponseId"), HiddenField)
                        Dim hdQuestionCode = CType(rptItem.FindControl("hdQuestionCode"), HiddenField)
                        Dim responseIdValue = Long.Parse(hdResponseId.Value)
                        Dim customerResponse As IUserDefinedCustomerSurveyResponses
                        Dim liQuestionItem = rptItem.FindControl("liQuestionItem")
                        If liQuestionItem.Visible = True Then
                            If Not CurrentCertificationCustomerCertification.CertificationTypeCodeString.Equals(
                                                       CertificationTypeEnum.CERTIFICATION.ToString()) Then
                                Dim currentSelectedOption = GetCurrentReCertOption()
                                If currentSelectedOption IsNot Nothing Then
                                    If Not currentSelectedOption.QuestionCode.Equals(hdQuestionCode.Value) AndAlso
                                        Not String.IsNullOrEmpty(hdQuestionCode.Value) Then
                                        Continue For
                                    End If
                                End If
                            End If

                            If hdQuestionCode.Value.Equals(Enums.QuestionCode.CERT_DECLARATION_AGREEMENT.ToString()) OrElse
                                hdQuestionCode.Value.Equals(Enums.QuestionCode.RECERT_DECLARATION_AGREEMENT.ToString()) Then
                                Continue For
                            End If

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
                                customerResponse.IsNewObjectFlag = True
                                If SetPropertiesForCustomerResponse(rptItem, customerResponse) Then
                                    AMCCertRecertController.InsertCustomerSurveyResponse(customerResponse)
                                End If
                            End If
                            For Each issue As IIssue In customerResponse.ValidationIssuesForMe
                                issueColletion.Add(issue)
                            Next
                        End If
                    End If
                Next
                If issueColletion.Count < 1 Then
                    issueColletion = CType(AMCCertRecertController.CommitCustomerSurveyResponsees(surveyIdValue, MasterCustomerId, SubCustomerId), 
                                           IssuesCollection)
                End If
            End If

            Return issueColletion
        End Function
        Private Function SaveAgreementResponse() As IIssuesCollection
            Dim issueColletion As New IssuesCollection
            Dim surveyIdValue = Long.Parse(hdSurveyId.Value)
            For Each rptItem As RepeaterItem In rptAgreement.Items
                Dim hdQuestionEnabled = CType(rptItem.FindControl("hdQuestionEnabled"), HiddenField)
                If hdQuestionEnabled IsNot Nothing AndAlso Not String.IsNullOrEmpty(hdQuestionEnabled.Value) AndAlso Convert.ToBoolean(hdQuestionEnabled.Value) Then
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
                        customerResponse.IsNewObjectFlag = True
                        If SetPropertiesForCustomerResponse(rptItem, customerResponse) Then
                            AMCCertRecertController.InsertCustomerSurveyResponse(customerResponse)
                        End If
                    End If
                    For Each issue As IIssue In customerResponse.ValidationIssuesForMe
                        issueColletion.Add(issue)
                    Next
                End If
            Next
            If issueColletion.Count < 1 Then
                issueColletion = CType(AMCCertRecertController.CommitCustomerSurveyResponsees(surveyIdValue, MasterCustomerId, SubCustomerId), 
                                       IssuesCollection)
            End If
            Return issueColletion
        End Function
        Private Function SaveCustomerExternalDocumentInfo() As IIssuesCollection
            Dim results As IIssuesCollection = Nothing
            Try
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                results =
                    AMCCertRecertController.CommitCustomerExternalDocuments(
                                                            DocumentationType.LICENSURE.ToString(),
                                                            MasterCustomerId,
                                                            SubCustomerId)
                If results Is Nothing OrElse results.Count <= 0 Then
                    MoveFileToApproriateDirectory(DocumentationType.LICENSURE.ToString())
                End If
                BindingDataToList()
            Catch ex As Exception
                ProcessException(ex)
            End Try
            Return results
        End Function

        Private Sub SetValueForCustomerExternalDocument(ByRef customerExternalDocumentation As IUserDefinedCustomerExternalDocumentation)
            With customerExternalDocumentation
                .RelatedMasterCustomerId = MasterCustomerId
                .RelatedSubcustomerId = SubCustomerId
                If .IssuingBody.List Is Nothing Then
                    .IssuingBody.FillList()
                End If
                .IssuingBodyString = ddlState.SelectedValue
                .IssuedNumber = txtLicenseNumber.Text()
                If txtExpirationDate.Text <> String.Empty Then
                    .CycleEndDate = DateTime.Parse(txtExpirationDate.Text)
                Else
                    .CycleEndDate = DateTime.MinValue
                End If
                If txtDateOfOriginalIssue.Text <> String.Empty Then
                    .InitialIssueDate =
                        DateTime.Parse(txtDateOfOriginalIssue.Text)
                Else
                    .InitialIssueDate = DateTime.MinValue
                End If
            End With
        End Sub

        Private Function SaveCollectedDataFromUser() As IIssuesCollection
            Dim iisuesCollection As New IssuesCollection
            Dim responseIIsuesCollection = SaveCustomerResponsees()
            If Not EnableHiddenLicensureValidationRule Then
                Dim customerExternalDocumentation = SaveCustomerExternalDocumentInfo()
                For Each issueItem As IIssue In customerExternalDocumentation
                    iisuesCollection.Add(issueItem)
                Next
            End If
            Dim fieldQuestionList = GetFieldInfo("QuestionList")
            If fieldQuestionList IsNot Nothing AndAlso fieldQuestionList.IsEnabled Then
                Dim agreeIssues = SaveAgreementResponse()
                For Each issueItem As IIssue In responseIIsuesCollection
                    iisuesCollection.Add(issueItem)
                Next


                For Each issueItem As IIssue In agreeIssues
                    iisuesCollection.Add(issueItem)
                Next
            End If
            Return iisuesCollection
        End Function

        Private Sub BindingDataToControl()
            Dim fieldQuestionList = GetFieldInfo("QuestionList")
            If fieldQuestionList IsNot Nothing AndAlso fieldQuestionList.IsEnabled Then
                BindingSurveyDataToControl()
            End If

            LicensureContainer.Visible = Not EnableHiddenLicensureValidationRule
            If Not Me.EnableHiddenLicensureValidationRule Then
                BindingDataToList()
            End If
            CommonHelper.BindIssuingBodyType(ddlState, DocumentationType.LICENSURE.ToString())
        End Sub

        Private Sub BindingDataToList()
            Dim customerExternals As IUserDefinedCustomerExternalDocumentations
            customerExternals =
                AMCCertRecertController.GetCustomerExternalDocuments(DocumentationType.LICENSURE.ToString(),
                                                                     MasterCustomerId,
                                                                     SubCustomerId)
            rptLicensure.DataSource = customerExternals
            rptLicensure.DataBind()
        End Sub

        Private Sub BindingSurveyDataToControl()
            Dim surveyQuestionaire As UserDefinedSurveyQuestions
            Dim survey As UserDefinedSurvey = Nothing
            Dim userDefinedSurvey As IUserDefinedSurvey
            If CurrentCertificationCustomerCertification IsNot Nothing AndAlso Not String.IsNullOrEmpty(CurrentCertificationCustomerCertification.CertificationTypeCodeString) Then
                If CurrentCertificationCustomerCertification.CertificationTypeCodeString.Equals(CertificationTypeEnum.CERTIFICATION.ToString()) Then
                    userDefinedSurvey = AMCCertRecertController.GetSurveyByTitle(DataAccessConstants.CERTIFICATION_DECLARATION_SURVEY_TITLE)
                    If userDefinedSurvey IsNot Nothing Then
                        survey = CType(userDefinedSurvey, UserDefinedSurvey)
                    End If
                Else
                    userDefinedSurvey = AMCCertRecertController.GetSurveyByTitle(DataAccessConstants.RECERTIFICATION_DECLARATION_SURVEY_TITLE)
                    If userDefinedSurvey IsNot Nothing Then
                        survey = CType(userDefinedSurvey, UserDefinedSurvey)
                    End If
                End If
            End If

            If survey IsNot Nothing Then
                hdSurveyId.Value = survey.SurveyId.ToString()
                surveyQuestionaire = CType(survey.UserDefinedSurveyQuestions, UserDefinedSurveyQuestions)
                rptQuestionnaire.DataSource = surveyQuestionaire
                rptQuestionnaire.DataBind()
                Dim agreementQuestions As New UserDefinedSurveyQuestions
                For Each question As UserDefinedSurveyQuestion In surveyQuestionaire
                    If question.Enabled AndAlso question.QuestionCode.Equals(Enums.QuestionCode.CERT_DECLARATION_AGREEMENT.ToString()) OrElse
                       question.QuestionCode.Equals(Enums.QuestionCode.RECERT_DECLARATION_AGREEMENT.ToString()) Then
                        agreementQuestions.Add(question)
                    End If
                Next
                rptAgreement.DataSource = agreementQuestions
                rptAgreement.DataBind()
            End If
        End Sub
#End Region
        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub

        Public ReadOnly Property SaveControls() As List(Of String) Implements IReload.SaveControls
            Get
                Dim result = New List(Of String)
                result.Add("RecertificationOptionUC")
                result.Add("Licensure")
                Return result
            End Get
        End Property

        Public Sub Reload(ByVal saveControl As String) Implements IReload.Reload
            If SaveControls.Contains(saveControl) Then
                BindingDataToList()
                Dim fieldQuestionList = GetFieldInfo("QuestionList")
                If fieldQuestionList IsNot Nothing AndAlso fieldQuestionList.IsEnabled Then
                    BindingSurveyDataToControl()
                End If

                Dim issues = CheckBusinessValidation()
                If issues Is Nothing OrElse issues.Count < 1 Then
                    Me.hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                Else
                    Me.hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                End If
            End If
        End Sub
    End Class
End Namespace