Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Data.Enums
Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports TIMSS.API.Generated.CustomerInfo
Imports TIMSS.API.User.CertificationInfo
Imports TIMSS.API.CustomerInfo
Imports TIMSS.API.Core.Validation
Imports TIMSS.API.User.UserDefinedInfo

Namespace Controls.Common
    Public Class RegistrationUC
        Inherits SectionBaseUserControl

#Region "Properties"

#End Region

#Region "Private Methods"
        Public Overrides Sub ValidateFormFillCompleted()
            If Not Page.IsPostBack Then
                BindDegreeToListBox()
            End If
        End Sub
#End Region

#Region "Event Handlers"
        ''' <summary>
        ''' Handles the Load event of the Page control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            Try
                txtCurrentDate.Text = DateTime.Today.ToString(CommonConstants.DATE_FORMAT)
                LoadData()
                Dim validationReadOnly = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                               ValidationRuleId.DEMOGRAPHIC_READONLY.ToString(),
                                                               Server.MapPath(ParentModulePath))
                If validationReadOnly = True Then
                    DisableControl(True)
                Else
                    DisableControl(False)
                End If

            Catch ex As Exception
                ProcessException(ex)
            End Try

        End Sub
#End Region

#Region "Private function"

        ''' <summary>
        ''' Gets the home address.
        ''' </summary>
        ''' <param name="customerInfo">The customer information.</param>
        ''' <param name="locationType">Adress location Type : Home or office </param>
        ''' <returns></returns>
        Private Function GetAddress(ByVal customerInfo As ICustomer, ByVal locationType As String) As ICustomerAddress
            If customerInfo IsNot Nothing Then
                If customerInfo.Addresses IsNot Nothing Then
                    If customerInfo.Addresses.Count > 0 Then
                        For Each customerAddress As ICustomerAddress In customerInfo.Addresses
                            If customerAddress.AddressStatusCodeString = locationType Then
                                Return customerAddress
                            End If
                        Next
                    End If
                End If
            End If
            Return Nothing
        End Function

        Public Overrides Function Save() As IIssuesCollection
            Dim results As New IssuesCollection
            Try
                Dim validationReadOnly = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                               ValidationRuleId.DEMOGRAPHIC_READONLY.ToString(),
                                                               Server.MapPath(ParentModulePath))
                If validationReadOnly = True Then  '' readonly , not edit
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                    Return results
                End If

                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                Dim birthDate As DateTime
                If txtBirthDate.Text <> String.Empty Then
                    birthDate = DateTime.Parse(txtBirthDate.Text)
                End If
                results =
                    CType(AMCCertRecertController.UpdateCustomerInfo(OrganizationId,
                                                                     OrganizationUnitId,
                                                                     MasterCustomerId,
                                                                     SubCustomerId,
                                                                     GetSelectedDegrees(),
                                                                     lblHomeTelephoneValue.Text,
                                                                CommunicationTypeEnum.PHONE.ToString(),
                                                                CommunicationLocationEnum.HOME.ToString(),
                                                                     birthDate,
                                                                     txtMessagesLeft.Text,
                                                                     0, lblWorkTelephone.Text,
                                                                     CommunicationLocationEnum.OFFICE.ToString()), IssuesCollection) 'Integer.Parse(ddlReceiveMaterials.SelectedValue)
                UpdateNameOnCertification(txtNameAppearOnCertificate.Text)
                Dim fieldQuestionList = GetFieldInfo("QuestionList")
                If fieldQuestionList IsNot Nothing AndAlso fieldQuestionList.IsEnabled Then
                    Dim saveCustomerResponseResult = SaveCustomerSurveyResponse()
                    For Each issue As IIssue In saveCustomerResponseResult
                        If results Is Nothing Then
                            results = New IssuesCollection()
                        End If
                        results.Add(issue)
                    Next
                End If

                If results Is Nothing OrElse results.Count <= 0 Then
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                End If
                LoadData()
            Catch ex As Exception
                Dim issue = New DatabaseErrorIssue(ex.Message)
                If results Is Nothing Then
                    results = New IssuesCollection()
                End If
                results.Add(issue)
                Return results
            End Try
            Return results
        End Function

        Private Function GetSelectedDegrees() As String
            Dim result = String.Empty
            For Each item As ListItem In lstbDegree.Items
                If item.Selected = True Then
                    result += String.Format("{0},", item.Value)
                End If
            Next
            If Not String.IsNullOrEmpty(result) Then
                result = result.TrimEnd(CType(",", Char))
            End If
            Return result
        End Function

        Private Function SaveCustomerSurveyResponse() As IIssuesCollection
            ''save response for question require special test
            Dim currentSurveyId = Long.Parse(hdSurveyId.Value)
            Dim iissuesCollectionResult As New IssuesCollection

            If IsQuestionExisted(DataAccessConstants.CERT_DEMOGRAPHICS_SURVEY_TITLE, Enums.QuestionCode.CERT_DEMOGRAPHICS_REQUIRE_SPECIAL_TEST.ToString()) Then
                Dim requiresIssueCollection = SaveRequireTestSurvey(currentSurveyId)
                For Each iisue As IIssue In requiresIssueCollection
                    iissuesCollectionResult.Add(iisue)
                Next
            End If

            If IsQuestionExisted(DataAccessConstants.CERT_DEMOGRAPHICS_SURVEY_TITLE, Enums.QuestionCode.CERT_DEMOGRAPHICS_MESSAGE_LEFT_WITH.ToString()) Then
                Dim messageLeftIssuesCollection = SaveMessageLeftSurvey(currentSurveyId)
                For Each issue As IIssue In messageLeftIssuesCollection
                    iissuesCollectionResult.Add(issue)
                Next
            End If

            If IsQuestionExisted(DataAccessConstants.CERT_DEMOGRAPHICS_SURVEY_TITLE, Enums.QuestionCode.CERT_DEMOGRAPHICS_HOW_YOU_HEAR_THE_EXAM.ToString()) Then
                Dim knownFromIssuesCollection = SaveKnownFromSurvey(currentSurveyId)
                For Each issue As IIssue In knownFromIssuesCollection
                    iissuesCollectionResult.Add(issue)
                Next
            End If
            AMCCertRecertController.CommitCustomerSurveyResponsees(currentSurveyId, MasterCustomerId, SubCustomerId)
            ''save response for question message left for
            Return iissuesCollectionResult
        End Function

        Private Function SaveRequireTestSurvey(ByVal surveyId As Long) As IIssuesCollection
            Dim requiresIssueCollection As IIssuesCollection
            Dim requireSpecialTestResponse As IUserDefinedCustomerSurveyResponses
            If Long.Parse(hdRequireSpecialTestResponse.Value) > 0 Then
                requireSpecialTestResponse =
                    AMCCertRecertController.GetCustomerSurveyResponseByReponseId(
                                                            Long.Parse(hdRequireSpecialTestResponse.Value),
                                                            surveyId,
                                                            MasterCustomerId,
                                                            SubCustomerId)
                If Integer.Parse(rblRequireSpecialTest.SelectedValue) =
                                                CType(YesNoAnwserType.YES, Integer) Then
                    requireSpecialTestResponse.AnswerId = Long.Parse(hdAnswerYes.Value)
                    requireSpecialTestResponse.Comments = txtComment.Text
                Else
                    requireSpecialTestResponse.AnswerId = Long.Parse(hdAnswerNo.Value)
                    requireSpecialTestResponse.Comments = String.Empty
                End If
                requiresIssueCollection =
                    AMCCertRecertController.UpdateCustomerSurveyResponse(requireSpecialTestResponse)
            Else
                requireSpecialTestResponse = (New UserDefinedCustomerSurveyResponsees()).CreateNew()
                With requireSpecialTestResponse
                    .IsNewObjectFlag = True
                    .MasterCustomerId = MasterCustomerId
                    .SubcustomerId = SubCustomerId
                    .QuestionId = Long.Parse(hdRequireSpecialTestQuestionId.Value)
                    .SurveyId = surveyId
                End With
                If Integer.Parse(rblRequireSpecialTest.SelectedValue) =
                                                CType(YesNoAnwserType.YES, Integer) Then
                    requireSpecialTestResponse.AnswerId = Long.Parse(hdAnswerYes.Value)
                    requireSpecialTestResponse.Comments = txtComment.Text
                Else
                    requireSpecialTestResponse.AnswerId = Long.Parse(hdAnswerNo.Value)
                    requireSpecialTestResponse.Comments = String.Empty
                End If
                requiresIssueCollection =
                   AMCCertRecertController.InsertCustomerSurveyResponse(requireSpecialTestResponse)
            End If
            Return requiresIssueCollection
        End Function

        Private Function SaveMessageLeftSurvey(ByVal surveyId As Long) As IIssuesCollection
            Dim messageLeftIssueCollection As IIssuesCollection = Nothing
            Dim messageLeftReponse As IUserDefinedCustomerSurveyResponses
            Dim messageLeftResponseId = Long.Parse(Me.hdMessageLeftResponse.Value)
            If messageLeftResponseId > 0 Then
                messageLeftReponse =
                    AMCCertRecertController.GetCustomerSurveyResponseByReponseId(messageLeftResponseId,
                                                                                 surveyId,
                                                                                 Me.MasterCustomerId,
                                                                                 Me.SubCustomerId)
                messageLeftReponse.AnswerId = Long.Parse(hdMessageLeftAnswer.Value)
                messageLeftReponse.Comments = txtMessagesLeft.Text
                messageLeftIssueCollection =
                    AMCCertRecertController.UpdateCustomerSurveyResponse(messageLeftReponse)
            Else
                messageLeftReponse = (New UserDefinedCustomerSurveyResponsees()).CreateNew()
                With messageLeftReponse
                    .IsNewObjectFlag = True
                    .SurveyId = surveyId
                    .QuestionId = Long.Parse(Me.hdMessageLeftQuestionId.Value)
                    .MasterCustomerId = MasterCustomerId
                    .SubcustomerId = SubCustomerId
                    .AnswerId = Long.Parse(hdMessageLeftAnswer.Value)
                    .Comments = txtMessagesLeft.Text
                End With
                messageLeftIssueCollection =
                    AMCCertRecertController.InsertCustomerSurveyResponse(messageLeftReponse)
            End If
            Return messageLeftIssueCollection
        End Function

        Private Function SaveKnownFromSurvey(ByVal surveyId As Long) As IIssuesCollection
            Dim knownFromIssuesCollection As IIssuesCollection = Nothing
            Dim knownFromResponseObject As IUserDefinedCustomerSurveyResponses
            Dim knownFromResponseId = Long.Parse(Me.hdKnownFromResponseID.Value)
            If knownFromResponseId > 0 Then
                knownFromResponseObject =
                    AMCCertRecertController.GetCustomerSurveyResponseByReponseId(knownFromResponseId,
                                                                                 surveyId,
                                                                                 Me.MasterCustomerId,
                                                                                 Me.SubCustomerId)
                knownFromResponseObject.AnswerId =
                    Long.Parse(ddlRequireSpecialTestDesc.SelectedValue)

                knownFromIssuesCollection =
                    AMCCertRecertController.UpdateCustomerSurveyResponse(knownFromResponseObject)
            Else
                knownFromResponseObject = (New UserDefinedCustomerSurveyResponsees()).CreateNew()
                With knownFromResponseObject
                    .IsNewObjectFlag = True
                    .MasterCustomerId = Me.MasterCustomerId
                    .SubcustomerId = Me.SubCustomerId
                    .SurveyId = surveyId
                    .QuestionId = Long.Parse(Me.hdKnownFromQuestionId.Value)
                    .AnswerId = Long.Parse(ddlRequireSpecialTestDesc.SelectedValue)
                End With
                knownFromIssuesCollection =
                    AMCCertRecertController.InsertCustomerSurveyResponse(knownFromResponseObject)
            End If
            Return knownFromIssuesCollection
        End Function

        Private Sub BindDegreeToListBox()
            lstbDegree.Items.Clear()
            With lstbDegree
                'sets the Available Credentials  listbox
                .DataSource = GetApplicationCodes("CUS", "CREDENTIALS", True)
                .DataTextField = "DESCRIPTION"
                .DataValueField = "CODE"
                .DataBind()
            End With
        End Sub

        Private Sub LoadData()
            Dim customerHomeAddress As ICustomerAddress
            Dim customerWorkAddress As ICustomerAddress
            Dim customerInfo = AMCCertRecertController.GetCustomerInfoItem(OrganizationId, OrganizationUnitId, MasterCustomerId, SubCustomerId)
            If customerInfo IsNot Nothing Then
                customerHomeAddress = GetAddress(customerInfo, AddressType.HOME.ToString())
                customerWorkAddress = GetAddress(customerInfo, AddressType.OFFICE.ToString())
                lblFirstNameValue.Text = customerInfo.FirstName
                lblMiddleNameValue.Text = customerInfo.MiddleName
                lblLastNameValue.Text = customerInfo.LastName
                lblEmailAddressValue.Text = customerInfo.PrimaryEmailAddress
                lblFormerNameValue.Text = customerInfo.Aliases(0).SearchName.TrimEnd(","c)
                If CurrentCertificationCustomerCertification.CertificationTypeCodeString.Equals(
                                                   CertificationTypeEnum.CERTIFICATION.ToString()) Then  '' Cert => don't have CertificationID
                    lblCertificationNumberValue.Text = String.Empty
                Else '' Recert
                    lblCertificationNumberValue.Text = CurrentCertificationCustomerCertification.OrigCertificationId.ToString()
                End If
                If customerInfo.BirthDate = DateTime.MinValue Then
                    txtBirthDate.Text = String.Empty
                    txtDateOfBirthValues.Text = String.Empty
                Else
                    txtBirthDate.Text = customerInfo.BirthDate.ToString(CommonConstants.DATE_FORMAT)
                    txtDateOfBirthValues.Text = customerInfo.BirthDate.ToString(CommonConstants.DATE_FORMAT)
                End If

                Dim fieldQuestionList = GetFieldInfo("QuestionList")
                If fieldQuestionList IsNot Nothing AndAlso fieldQuestionList.IsEnabled Then
                    LoadSurveyData()
                End If

                LoadCustomerDegree(customerInfo)
                LoadCustomerHomeAddress(customerInfo, customerHomeAddress)
                LoadCustomerWorkAddress(customerInfo, customerWorkAddress)
                LoadCustomerPhone(customerInfo)
                LoadFaxNumber(customerInfo)
                LoaNameOnCertification()
                If Not Page.IsPostBack Then
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                End If
            End If

        End Sub

        Private Sub LoadCustomerPhone(ByVal customerInfo As ICustomer)
            For Each customercommunication As CustomerCommunication In customerInfo.Communications
                If customercommunication.CommLocationCodeString = CommunicationLocationEnum.HOME.ToString() _
                                    AndAlso customercommunication.CommTypeCodeString = CommunicationTypeEnum.PHONE.ToString() Then
                    txtHomeTelephone.Text = customercommunication.FormattedPhoneAddress
                    lblHomeTelephoneValue.Text = txtHomeTelephone.Text
                    Exit For
                End If
            Next
            '' load work telephone
            For Each customercommunication As CustomerCommunication In customerInfo.Communications
                If customercommunication.CommLocationCodeString = CommunicationLocationEnum.OFFICE.ToString() _
                                    AndAlso customercommunication.CommTypeCodeString = CommunicationTypeEnum.PHONE.ToString() Then
                    txtWorkTelephone.Text = customercommunication.FormattedPhoneAddress
                    lblWorkTelephoneValue.Text = txtWorkTelephone.Text '' useing phone format
                    Exit For
                End If
            Next
        End Sub

        Private Sub LoadCustomerHomeAddress(ByVal customerInfo As ICustomer, ByVal customerHomeAddress As ICustomerAddress)
            If customerHomeAddress IsNot Nothing Then
                lblHomeAddressValue.Text = customerHomeAddress.Address1
                lblHomeAddress2Value.Text = customerHomeAddress.Address2
                lblHomeAddress3values.Text = customerHomeAddress.Address3
                lblHomeAddress4Value.Text = customerHomeAddress.Address4
                lblCityValue.Text = customerHomeAddress.City
                lblStateValue.Text = customerHomeAddress.State
                lblZipValue.Text = customerHomeAddress.PostalCode
            ElseIf customerInfo.PrimaryAddress IsNot Nothing Then
                If customerInfo.PrimaryAddress.AddressTypeCodeString = CommunicationLocationEnum.HOME.ToString() Then
                    lblHomeAddressValue.Text = customerInfo.PrimaryAddress.Address.Address1
                    lblHomeAddress2Value.Text = customerInfo.PrimaryAddress.Address.Address2
                    lblHomeAddress3values.Text = customerInfo.PrimaryAddress.Address.Address3
                    lblHomeAddress4Value.Text = customerInfo.PrimaryAddress.Address.Address4
                    lblCityValue.Text = customerInfo.PrimaryAddress.Address.City
                    lblStateValue.Text = customerInfo.PrimaryAddress.Address.State
                    lblZipValue.Text = customerInfo.PrimaryAddress.Address.PostalCode
                End If
            End If
        End Sub

        Private Sub LoadFaxNumber(ByVal customerInfo As ICustomer)
            If customerInfo IsNot Nothing Then
                If customerInfo.PrimaryFax IsNot Nothing Then
                    If customerInfo.PrimaryFaxLocationCodeString = CommunicationLocationEnum.HOME.ToString() Then
                        lblFaxNumberHomeValue.Text = customerInfo.PrimaryFax
                    ElseIf customerInfo.PrimaryFaxLocationCodeString = CommunicationLocationEnum.OFFICE.ToString() Then
                        lblFaxNumberOfficeValue.Text = customerInfo.PrimaryFax
                    End If
                End If
            End If
        End Sub

        Private Sub LoadCustomerWorkAddress(ByVal customerInfo As ICustomer, ByVal customerHomeAddress As ICustomerAddress)
            If customerHomeAddress IsNot Nothing Then
                lblWorkAddressOneValue.Text = customerHomeAddress.Address1
                lblWorkAddressTwoValue.Text = customerHomeAddress.Address2
                lblWorkAddressThreeValue.Text = customerHomeAddress.Address3
                lblWorkCityValue.Text = customerHomeAddress.City
                lblWorkStateValue.Text = customerHomeAddress.State
                lblWorkZipValue.Text = customerHomeAddress.PostalCode
            ElseIf customerInfo.PrimaryAddress IsNot Nothing Then
                If customerInfo.PrimaryAddress.AddressTypeCodeString = CommunicationLocationEnum.OFFICE.ToString() Then
                    lblWorkAddressOneValue.Text = customerInfo.PrimaryAddress.Address.Address1
                    lblWorkAddressTwoValue.Text = customerInfo.PrimaryAddress.Address.Address2
                    lblWorkAddressThreeValue.Text = customerInfo.PrimaryAddress.Address.Address3
                    lblWorkCityValue.Text = customerInfo.PrimaryAddress.Address.City
                    lblWorkStateValue.Text = customerInfo.PrimaryAddress.Address.State
                    lblWorkZipValue.Text = customerInfo.PrimaryAddress.Address.PostalCode
                End If
            End If
        End Sub

        Private Sub LoadCustomerDegree(ByVal customerInfo As ICustomer)
            If Not String.IsNullOrEmpty(customerInfo.NameCredentials) Then
                lblDegreeValue.Text = customerInfo.NameCredentials
                Dim arrNameCredenials As String() = customerInfo.NameCredentials.ToString().Split(","c)
                If arrNameCredenials IsNot Nothing AndAlso arrNameCredenials.Length > 0 Then
                    For Each listItem As ListItem In lstbDegree.Items
                        If CheckExistItem(listItem.Value, arrNameCredenials) Then
                            listItem.Selected = True
                        End If
                    Next
                End If
            End If
        End Sub

        Private Function CheckExistItem(ByVal item As String, ByVal arrItems As String()) As Boolean
            For i As Integer = 0 To arrItems.Length - 1
                If arrItems(i) = item Then
                    Return True
                End If
            Next
            Return False
        End Function


        Private Sub LoaNameOnCertification()
            Dim certification As New CertificationCustomerCertification
            certification = CType(AMCCertRecertController.GetCertificationCustomerCertificationByCertId(
                    Me.CurrentCertificationCustomerCertification.CertificationId), CertificationCustomerCertification)
            If certification IsNot Nothing Then
                txtNameAppearOnCertificate.Text = certification.UserDefinedNameOnCert
            End If
        End Sub

        Private Sub UpdateNameOnCertification(ByVal name As String)
            Dim certification As New CertificationCustomerCertification
            certification = CType(AMCCertRecertController.GetCertificationCustomerCertificationByCertId(
                    Me.CurrentCertificationCustomerCertification.CertificationId), CertificationCustomerCertification)
            If certification IsNot Nothing Then
                certification.UserDefinedNameOnCert = name
                AMCCertRecertController.UpdateCertification(certification)
            End If
        End Sub

        Private Sub LoadSurveyData()
            Dim surveyQuestionaire As UserDefinedSurveyQuestions
            Dim survey As UserDefinedSurvey
            survey =
                CType(AMCCertRecertController.GetSurveyByTitle(DataAccessConstants.CERT_DEMOGRAPHICS_SURVEY_TITLE), 
                                                    UserDefinedSurvey)
            If survey IsNot Nothing Then
                hdSurveyId.Value = survey.SurveyId.ToString()
                surveyQuestionaire =
                    CType(survey.UserDefinedSurveyQuestions, UserDefinedSurveyQuestions)
            End If
            Dim surveyResponse As IUserDefinedCustomerSurveyResponses
            For Each surveyQuestion As UserDefinedSurveyQuestion In surveyQuestionaire
                surveyResponse = AMCCertRecertController.GetCustomerSurveyResponseByQuestionId(surveyQuestion.QuestionId,
                                                                    surveyQuestion.SurveyId, MasterCustomerId, SubCustomerId)
                'TODO: CHECK IF surveyQuestion.IsEnabled
                If surveyQuestion.Enabled Then
                    Select Case surveyQuestion.QuestionCode
                        Case Enums.QuestionCode.CERT_DEMOGRAPHICS_REQUIRE_SPECIAL_TEST.ToString()
                            lblRequireSpecialTest.Text = surveyQuestion.QuestionText
                            hdRequireSpecialTestQuestionId.Value = surveyQuestion.QuestionId.ToString()
                            rblRequireSpecialTest.SelectedValue = CType(YesNoAnwserType.NO, Integer).ToString()
                            For Each surveyAnswer As IUserDefinedSurveyAnswers In surveyQuestion.UserDefinedSurveyAnsweres
                                If surveyAnswer.QuestionId = surveyQuestion.QuestionId Then
                                    If surveyAnswer.AnswerText.Equals(AnswerTextDefault.YES.ToString()) Then
                                        hdAnswerYes.Value = surveyAnswer.AnswerId.ToString()
                                    Else
                                        hdAnswerNo.Value = surveyAnswer.AnswerId.ToString()
                                    End If
                                    If surveyResponse IsNot Nothing AndAlso surveyResponse.AnswerId = surveyAnswer.AnswerId AndAlso surveyResponse.QuestionId = surveyQuestion.QuestionId Then
                                        hdRequireSpecialTestResponse.Value = surveyResponse.ResponseId.ToString()
                                        rblRequireSpecialTest.SelectedValue = If(surveyAnswer.AnswerText.ToUpper() = "YES", CType(YesNoAnwserType.YES, Integer).ToString(), CType(YesNoAnwserType.NO, Integer).ToString())
                                        txtComment.Text = surveyResponse.Comments
                                    End If
                                End If
                            Next
                            RequireSpecialTest.Visible = True
                        Case Enums.QuestionCode.CERT_DEMOGRAPHICS_MESSAGE_LEFT_WITH.ToString()
                            hdMessageLeftQuestionId.Value = surveyQuestion.QuestionId.ToString()
                            lblMessagesLeft.Text = surveyQuestion.QuestionText
                            If surveyResponse IsNot Nothing Then
                                hdMessageLeftResponse.Value = surveyResponse.ResponseId.ToString()
                                txtMessagesLeft.Text = surveyResponse.Comments
                            End If
                            For Each surveyAnswer As IUserDefinedSurveyAnswers In surveyQuestion.UserDefinedSurveyAnsweres
                                hdMessageLeftAnswer.Value = surveyAnswer.AnswerId.ToString()
                            Next
                            MessagesLeft.Visible = True
                        Case Enums.QuestionCode.CERT_DEMOGRAPHICS_HOW_YOU_HEAR_THE_EXAM.ToString()
                            lblRequireSpecialTestDesc.Text = surveyQuestion.QuestionText
                            hdKnownFromQuestionId.Value = surveyQuestion.QuestionId.ToString()
                            ddlRequireSpecialTestDesc.DataValueField = "AnswerId"
                            ddlRequireSpecialTestDesc.DataTextField = "AnswerText"
                            ddlRequireSpecialTestDesc.DataSource = surveyQuestion.UserDefinedSurveyAnsweres
                            ddlRequireSpecialTestDesc.DataBind()
                            If surveyResponse IsNot Nothing Then
                                hdKnownFromResponseID.Value = surveyResponse.ResponseId.ToString()
                                ddlRequireSpecialTestDesc.SelectedValue = surveyResponse.AnswerId.ToString()
                            End If
                            RequireSpecialTestDesc.Visible = True
                    End Select
                End If
            Next
        End Sub

        Private Sub DisableControl(ByVal flag As Boolean)
            If flag = True Then '' recert (read only)
                txtDateOfBirthValues.Visible = True
                txtBirthDate.Visible = False
                lblDegreeValue.Visible = True
                lstbDegree.Visible = False
                rblRequireSpecialTest.Enabled = False
                ddlRequireSpecialTestDesc.Enabled = False
                txtMessagesLeft.Enabled = False
                txtComment.Enabled = False
                txtNameAppearOnCertificate.Enabled = False
            Else  '' Cert
                txtDateOfBirthValues.Visible = False
                txtBirthDate.Visible = True
                lblDegreeValue.Visible = False
                lstbDegree.Visible = True
                rblRequireSpecialTest.Enabled = True
                ddlRequireSpecialTestDesc.Enabled = True
                txtMessagesLeft.Enabled = True
                txtComment.Enabled = True
                txtNameAppearOnCertificate.Enabled = True
            End If
        End Sub
       
#End Region
    End Class
End Namespace