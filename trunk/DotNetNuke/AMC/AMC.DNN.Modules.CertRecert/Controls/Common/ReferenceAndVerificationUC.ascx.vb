Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports AMC.DNN.Modules.CertRecert.Business.BusinessValidation
Imports TIMSS.API.Core
Imports TIMSS.API.User.UserDefinedInfo
Imports TIMSS.API.Core.Validation

Namespace Controls.Common
    Public Class ReferenceAndVerificationUC
        Inherits SectionBaseUserControl

        Private _stateCodeList As ICodeList
        ''display State Decsription on GridView
        Private ReadOnly Property StateCodeList() As ICodeList
            Get
                If _stateCodeList Is Nothing Then
                    Dim customerExternalDocumentation =
                   (New UserDefinedCustomerExternalDocumentations()).CreateNew()
                    customerExternalDocumentation.DocumentationTypeString = DocumentationType.LICENSURE.ToString()
                    customerExternalDocumentation.IssuingBody.FillList()
                    _stateCodeList = customerExternalDocumentation.IssuingBody.List
                End If
                Return _stateCodeList
            End Get
        End Property

        Private _allAlternativeItemCheck As Boolean = True

#Region "Event Handler"
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            Try
                AddHandler rptPrintReferences.ItemDataBound, AddressOf RptReferencesItemDataBound
                regexHomeEmail.ValidationExpression = CommonConstants.EMAIL_VALIDATION
                regexWorkEmailValid.ValidationExpression = CommonConstants.EMAIL_VALIDATION
                maskedHomePhoneValidator.InvalidValueMessage = Localization.GetString("PhoneErrorFormat.Text", LocalResourceFile)
                maskedWorkPhoneValidator.InvalidValueMessage = Localization.GetString("PhoneErrorFormat.Text", LocalResourceFile)
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Public Overrides Function Save() As IIssuesCollection
            Dim results As IssuesCollection = Nothing
            Try
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                results =
                    CType(AMCCertRecertController.CommitCustomerExternalContacts(
                                                            CustomerContactTypeEnum.REFERENCES.ToString(),
                                                            MasterCustomerId,
                                                            SubCustomerId), IssuesCollection)
                If results Is Nothing OrElse results.Count <= 0 Then
                    Dim references = AMCCertRecertController.GetCustomerExternalContacts(CustomerContactTypeEnum.REFERENCES.ToString(),
                                                                                           MasterCustomerId,
                                                                                           SubCustomerId)
                    For Each contactItem As UserDefinedCustomerExternalContact In references
                        MoveFileFromTempToMainDirectory(DocumentationType.REFERENCES.ToString(),
                                                        contactItem.Guid,
                                                        contactItem.ContactId.ToString(),
                                                        String.Empty)
                    Next
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                    AMCCertRecertController.RefreshCustomerExternalContacts(CustomerContactTypeEnum.REFERENCES.ToString(),
                                                                   MasterCustomerId,
                                                                   SubCustomerId)
                End If
                'Save Alternative Survey
                Dim issues = SaveAlternativeVerifications()
                BindingDataToList()
                results = CheckBusinessValidation()
                If issues IsNot Nothing AndAlso issues.Count > 0 Then
                    For Each issue As IIssue In issues
                        results.Add(issue)
                    Next
                End If
                Return results
            Catch ex As Exception
                ProcessException(ex)
            End Try
            Return results
        End Function

        'TODO
        Private Function SaveAlternativeVerifications() As IIssuesCollection
            Dim results As New IssuesCollection()
            Dim fieldQuestionList = GetFieldInfo("QuestionList")
            If fieldQuestionList IsNot Nothing AndAlso fieldQuestionList.IsEnabled Then
                If rptAlternativeVerification.Items IsNot Nothing AndAlso rptAlternativeVerification.Items.Count > 0 Then
                    For Each repeaterItem As RepeaterItem In rptAlternativeVerification.Items
                        Dim hdQuestionEnabled = CType(repeaterItem.FindControl("hdQuestionEnabled"), HiddenField)
                        If Not String.IsNullOrEmpty(hdQuestionEnabled.Value) AndAlso Convert.ToBoolean(hdQuestionEnabled.Value) Then
                            Dim chkAlternativeVerification = CType(repeaterItem.FindControl("chkAlternativeVerification"), CheckBox)
                            Dim hdQuestionId = CType(repeaterItem.FindControl("hdQuestionId"), HiddenField)
                            Dim hdAnswerYes = CType(repeaterItem.FindControl("hdAnswerYes"), HiddenField)
                            Dim hdAnswerNo = CType(repeaterItem.FindControl("hdAnswerNo"), HiddenField)
                            Dim answerId = If(chkAlternativeVerification.Checked, hdAnswerYes.Value, hdAnswerNo.Value)
                            Dim hdResponseId = CType(repeaterItem.FindControl("hdResponseId"), HiddenField)
                            Dim issues As IIssuesCollection
                            If String.IsNullOrEmpty(hdResponseId.Value) Then 'Add response
                                issues = AMCCertRecertController.AddResponse(OrganizationId, OrganizationUnitId, MasterCustomerId, SubCustomerId, CertificationId, Convert.ToInt32(hdAlternativeVerificationSurveyId.Value),
                                                                    Convert.ToInt32(hdQuestionId.Value), Convert.ToInt32(answerId), chkAlternativeVerification.Text)
                            Else 'Update response
                                issues = AMCCertRecertController.AddOrUpdateResponse(hdQuestionId.Value, answerId, chkAlternativeVerification.Text, ReferencAndVeryficationSurveyTitle)
                            End If
                            'TODO: upload file
                            Dim userdefinedResponses = AMCCertRecertController.GetResponses(Convert.ToInt32(hdQuestionId.Value), Convert.ToInt32(answerId))
                            If userdefinedResponses IsNot Nothing AndAlso userdefinedResponses.Count > 0 Then
                                Dim userDefinedReponse = userdefinedResponses(0)
                                Try
                                    Dim alternativeUpload = CType(repeaterItem.FindControl("fuUploadFileAttachment"), FileUpload)
                                    If alternativeUpload.FileContent.Length > 1 Then
                                        If UploadFileIssue.IsNotPdfFile(alternativeUpload.FileContent) Then
                                            results.Add(New UploadFileIssue(userDefinedReponse, Localization.GetString("IsNotPdfFile.Text", LocalResourceFile)))
                                        Else ' Is PDF
                                            Dim fileLocation = UploadTempFile(alternativeUpload,
                                                   DocumentationType.REFERENCE_VERFICATION_ALTERNATIVE.ToString(),
                                                   userDefinedReponse.Guid)
                                            If Not String.IsNullOrEmpty(fileLocation) Then
                                                MoveFileFromTempToMainDirectory(DocumentationType.REFERENCE_VERFICATION_ALTERNATIVE.ToString(),
                                                                                userDefinedReponse.Guid,
                                                                                userDefinedReponse.ResponseId.ToString(),
                                                                                String.Empty)
                                            End If
                                        End If
                                    End If
                                Catch ex As Exception
                                    results.Add(New UploadFileIssue(userDefinedReponse, ex.Message))
                                End Try
                            End If
                            If issues IsNot Nothing AndAlso issues.Count > 0 Then
                                For Each issue As IIssue In issues
                                    results.Add(issue)
                                Next
                            End If
                        End If
                    Next
                End If
            End If
           
            Return results
        End Function

        Protected Sub BtnSaveClick(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click

            Dim customerExternalContact As UserDefinedCustomerExternalContact
            Dim iIssuesCollection As IIssuesCollection
            Try
                If (hdCurrentObjectUniqueId.Value <> String.Empty) Then
                    ''Edit 
                    customerExternalContact = CType(AMCCertRecertController.GetCustomerExternalContactByGuid(
                                                        hdCurrentObjectUniqueId.Value,
                                                        CustomerContactTypeEnum.REFERENCES.ToString(),
                                                        MasterCustomerId, SubCustomerId), 
                                                                    UserDefinedCustomerExternalContact)
                    SetPropertiesForObject(customerExternalContact)
                    iIssuesCollection = AMCCertRecertController.UpdateCustomerExternalContact(customerExternalContact)
                Else
                    customerExternalContact =
                        CType(New UserDefinedCustomerExternalContacts().CreateNew(), UserDefinedCustomerExternalContact)
                    customerExternalContact.IsNewObjectFlag = True
                    SetPropertiesForObject(customerExternalContact)
                    iIssuesCollection =
                        AMCCertRecertController.InsertCustomerExternalContact(customerExternalContact)
                End If


                If (iIssuesCollection IsNot Nothing AndAlso iIssuesCollection.Count > 0) AndAlso
                    customerExternalContact IsNot Nothing Then
                    ShowError(iIssuesCollection, lblPopupMessage)
                    hdIsValidateFailed.Value = CommonConstants.TAB_INCOMPLETED
                Else
                    If hfDeleteFile.Value.Equals("YES") Then ''Delete file
                        DeleteAttachDocument(DocumentationType.REFERENCES.ToString(),
                                             customerExternalContact.Guid,
                                             customerExternalContact.ContactId.ToString())
                    End If
                    If fuUploadFileAttachment.FileContent.Length > 0 Then ''upload file
                        UploadTempFile(fuUploadFileAttachment,
                                       DocumentationType.REFERENCES.ToString(),
                                       customerExternalContact.Guid)
                    End If
                    hdIsValidateFailed.Value = CommonConstants.TAB_COMPLETED
                    BindingDataToList()
                    CheckBusinessValidation()
                End If

            Catch ex As Exception
                ProcessException(ex)
            End Try
            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
        End Sub

        Protected Sub RptReferencesItemDataBound(ByVal sender As Object, ByVal e As Web.UI.WebControls.RepeaterItemEventArgs) Handles rptReferences.ItemDataBound
            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
                Try

                    Dim indexItem = e.Item.FindControl("lblItemNo")
                    If indexItem IsNot Nothing Then
                        Dim indexLabel = CType(indexItem, Label)
                        indexLabel.Text = String.Format(indexLabel.Text, e.Item.ItemIndex + 1)
                    End If

                    Dim customerExternalContract = CType(e.Item.DataItem, UserDefinedCustomerExternalContact)
                    Dim currentRow = e.Item.FindControl("item_")
                    If currentRow IsNot Nothing AndAlso TypeOf currentRow Is HtmlTableRow Then
                        CType(currentRow, HtmlTableRow).Attributes.Add("guid", customerExternalContract.Guid)
                    End If
                    Dim isFillHomeAddress As Boolean = False
                    Dim isFillOfficeAddress As Boolean = False
                    Dim isFillHomeCommunication As Boolean = False
                    Dim isFillWorkCommunication As Boolean = False
                    Dim isFillHomeEmailCommunication As Boolean = False
                    Dim isFillWorkEmailCommunication As Boolean = False

                    Dim lblCity = CType(e.Item.FindControl("lblCity"), Label)
                    Dim lblState = CType(e.Item.FindControl("lblState"), Label)
                    Dim lblDescriptionState = CType(e.Item.FindControl("lblDescriptionState"), Label)
                    Dim lblHomeCity = CType(e.Item.FindControl("lblHomeCity"), Label)
                    Dim lblHomeState = CType(e.Item.FindControl("lblHomeState"), Label)
                    Dim lblDescriptionHomeState = CType(e.Item.FindControl("lblDescriptionHomeState"), Label)
                    Dim lblHomeZip = CType(e.Item.FindControl("lblHomeZip"), Label)
                    Dim lblWorkZip = CType(e.Item.FindControl("lblWorkZip"), Label)
                    Dim lblName = CType(e.Item.FindControl("lblName"), Label)
                    Dim lblMiddleName = CType(e.Item.FindControl("lblMiddleName"), Label)
                    Dim lblFirstName = CType(e.Item.FindControl("lblFirstName"), Label)
                    Dim lblDescContactClass = CType(e.Item.FindControl("lblDescContactClass"), Label)
                    Dim lblDescPrefContactMethod = CType(e.Item.FindControl("lblDescPrefContactMethod"), Label)

                    lblName.Text = customerExternalContract.LastName
                    lblFirstName.Text = customerExternalContract.FirstName
                    lblMiddleName.Text = customerExternalContract.MiddleName
                    lblDescContactClass.Text = customerExternalContract.ContactClassType.Description
                    lblDescPrefContactMethod.Text = customerExternalContract.PrefContactMethod.Description
                    Dim linkLocation As String = String.Empty
                    Dim fileName = GetFileNameOfDocument(DocumentationType.REFERENCES.ToString(),
                                                         customerExternalContract.Guid.ToString(),
                                                         customerExternalContract.ContactId.ToString(),
                                                         linkLocation)

                    If fileName <> String.Empty Then
                        Dim hlAttachedDocumentName =
                            CType(e.Item.FindControl("hlAttachedDocumentName"), HyperLink)
                        hlAttachedDocumentName.Text = fileName
                        hlAttachedDocumentName.NavigateUrl = linkLocation
                        Dim hdAttachedDocumentName =
                            CType(e.Item.FindControl("hdAttachedDocumentName"), HiddenField)
                        hdAttachedDocumentName.Value = fileName
                    End If

                    For Each customerExternalAddressItem As IUserDefinedCustomerExternalAddress In customerExternalContract.UserDefinedCustomerExternalAddresses
                        If (customerExternalAddressItem.AddressTypeString.Equals(AddressType.HOME.ToString()) AndAlso Not isFillHomeAddress) Then
                            Dim lblHomeAddress1 = CType(e.Item.FindControl("lblHomeAddress1"), Label)
                            lblHomeAddress1.Text = customerExternalAddressItem.Address1
                            Dim lblHomeAddress2 = CType(e.Item.FindControl("lblHomeAddress2"), Label)
                            lblHomeAddress2.Text = customerExternalAddressItem.Address2
                            Dim lblHomeAddress3 = CType(e.Item.FindControl("lblHomeAddress3"), Label)
                            lblHomeAddress3.Text = customerExternalAddressItem.Address3
                            Dim lblHomeAddress4 = CType(e.Item.FindControl("lblHomeAddress4"), Label)
                            lblHomeAddress4.Text = customerExternalAddressItem.Address4
                            lblHomeCity.Text = customerExternalAddressItem.City
                            lblDescriptionHomeState.Text = StateCodeList.Item(customerExternalAddressItem.State).Description
                            lblHomeState.Text = StateCodeList.Item(customerExternalAddressItem.State).Code
                            lblHomeZip.Text = customerExternalAddressItem.PostalCode
                            isFillHomeAddress = True
                        End If

                        If (customerExternalAddressItem.AddressTypeString.Equals(AddressType.OFFICE.ToString()) AndAlso Not isFillOfficeAddress) Then

                            Dim lblWorkAddress1 = CType(e.Item.FindControl("lblWorkAddress1"), Label)
                            lblWorkAddress1.Text = customerExternalAddressItem.Address1
                            Dim lblWorkAddress2 = CType(e.Item.FindControl("lblWorkAddress2"), Label)
                            lblWorkAddress2.Text = customerExternalAddressItem.Address2
                            Dim lblWorkAddress3 = CType(e.Item.FindControl("lblWorkAddress3"), Label)
                            lblWorkAddress3.Text = customerExternalAddressItem.Address3
                            Dim lblWorkAddress4 = CType(e.Item.FindControl("lblWorkAddress4"), Label)
                            lblWorkAddress4.Text = customerExternalAddressItem.Address4
                            lblCity.Text = customerExternalAddressItem.City
                            lblDescriptionState.Text = StateCodeList.Item(customerExternalAddressItem.State).Description
                            lblState.Text = StateCodeList.Item(customerExternalAddressItem.State).Code
                            lblWorkZip.Text = customerExternalAddressItem.PostalCode
                            isFillOfficeAddress = True
                        End If
                    Next

                    ''set communications
                    For Each customerExternalCommunicationItem As IUserDefinedCustomerExternalCommunications In customerExternalContract.UserDefinedCustomerExternalCommunicationes

                        If customerExternalCommunicationItem.CommunicationLocationString.Equals(CommunicationLocationEnum.HOME.ToString()) AndAlso Not isFillHomeCommunication AndAlso customerExternalCommunicationItem.CommunicationTypeString.Equals(CommunicationTypeEnum.PHONE.ToString()) Then
                            Dim lblHomeTelephone As Label
                            lblHomeTelephone = CType(e.Item.FindControl("lblHomeTelephone"), Label)
                            lblHomeTelephone.Text = customerExternalCommunicationItem.FormattedPhoneAddress
                            isFillHomeCommunication = True
                        End If

                        If customerExternalCommunicationItem.CommunicationLocationString.Equals(CommunicationLocationEnum.HOME.ToString()) AndAlso
                                Not isFillHomeEmailCommunication AndAlso
                                customerExternalCommunicationItem.CommunicationTypeString.Equals(CommunicationTypeEnum.EMAIL.ToString()) Then
                            Dim lblHomeEmail As Label
                            lblHomeEmail = CType(e.Item.FindControl("lblHomeEmail"), Label)
                            lblHomeEmail.Text = customerExternalCommunicationItem.FormattedPhoneAddress
                            isFillHomeEmailCommunication = True
                        End If

                        If customerExternalCommunicationItem.CommunicationLocationString.Equals(CommunicationLocationEnum.OFFICE.ToString()) AndAlso
                                Not isFillWorkCommunication AndAlso
                                customerExternalCommunicationItem.CommunicationTypeString.Equals(CommunicationTypeEnum.PHONE.ToString()) Then
                            Dim lblWorkTelephone As Label
                            lblWorkTelephone = CType(e.Item.FindControl("lblWorkTelephone"), Label)
                            lblWorkTelephone.Text = customerExternalCommunicationItem.FormattedPhoneAddress
                            isFillWorkCommunication = True
                        End If

                        If customerExternalCommunicationItem.CommunicationLocationString.Equals(CommunicationLocationEnum.OFFICE.ToString()) AndAlso
                                Not isFillWorkEmailCommunication AndAlso
                                customerExternalCommunicationItem.CommunicationTypeString.Equals(CommunicationTypeEnum.EMAIL.ToString()) Then
                            Dim lblWorkEmail As Label
                            lblWorkEmail = CType(e.Item.FindControl("lblWorkEmail"), Label)
                            lblWorkEmail.Text = customerExternalCommunicationItem.FormattedPhoneAddress
                            isFillWorkEmailCommunication = True
                        End If


                    Next
                Catch ex As Exception
                    ProcessException(ex)
                End Try
            End If
        End Sub

        Protected Sub rptReferences_ItemCommand(ByVal source As Object,
                                                ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptReferences.ItemCommand
            Try
                If e.CommandName.Equals("Delete") Then
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                    Dim customerExternalContact As IUserDefinedCustomerExternalContact = Nothing
                    customerExternalContact =
                            AMCCertRecertController.GetCustomerExternalContactByGuid(e.CommandArgument.ToString(),
                                                                                     CustomerContactTypeEnum.REFERENCES.ToString(),
                                                                                     MasterCustomerId,
                                                                                     SubCustomerId)
                    If customerExternalContact IsNot Nothing Then
                        AMCCertRecertController.DeleteCustomerExternalContact(customerExternalContact)
                    End If
                End If
                BindingDataToList()
                CheckBusinessValidation()
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub
#End Region

#Region "Private Method"
        Public Overrides Sub ValidateFormFillCompleted()
            AMCCertRecertController.RefreshCustomerExternalContacts(CustomerContactTypeEnum.REFERENCES.ToString(),
                                                                    MasterCustomerId,
                                                                    SubCustomerId)
            BindingDataToList()
            CheckBusinessValidation()
        End Sub

        Private Sub BindingDataToList()
            Try
                Dim questionList = GetFieldInfo("QuestionList")
                If questionList IsNot Nothing AndAlso questionList.IsEnabled Then
                    Dim survey As UserDefinedSurvey
                    survey = CType(AMCCertRecertController.GetSurveyByTitle(ReferencAndVeryficationSurveyTitle), UserDefinedSurvey)
                    If survey IsNot Nothing Then
                        hdAlternativeVerificationSurveyId.Value = survey.SurveyId.ToString()
                        Dim alternativeVerificationQuestions = survey.UserDefinedSurveyQuestions
                        AddHandler rptAlternativeVerification.ItemDataBound, AddressOf RptAlternativeVerificationItemDataBound
                        rptAlternativeVerification.DataSource = alternativeVerificationQuestions
                        rptAlternativeVerification.DataBind()
                    End If
                End If

                hdDeletedFileList.Value = String.Empty
                Dim customerContacts = AMCCertRecertController.GetCustomerExternalContacts(CustomerContactTypeEnum.REFERENCES.ToString(),
                                                                                           MasterCustomerId,
                                                                                           SubCustomerId)
                If customerContacts.Count > 0 AndAlso Not Page.IsPostBack Then
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                End If
                SetPrintMode(customerContacts)

                ''Bind PrefContactMethod
                CommonHelper.BindPrefContactMethod(ddlPrefContactMethod)
                CommonHelper.BindContactClassType(rdlContactClass)
                CommonHelper.BindIssuingBodyType(ddlHomeState, DocumentationType.LICENSURE.ToString())
                CommonHelper.BindIssuingBodyType(ddlWorkState, DocumentationType.LICENSURE.ToString())
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        ''' <summary>
        ''' alternative verification item data bound.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterItemEventArgs" /> instance containing the event data.</param>
        Private Sub RptAlternativeVerificationItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim question = CType(e.Item.DataItem, UserDefinedSurveyQuestion)
                If question IsNot Nothing Then
                    Dim currentRow = CType(e.Item.FindControl("item_"), HtmlGenericControl)
                    Dim deleteIcon = CType(e.Item.FindControl("imgDeleteAttachment"), ImageButton)
                    Dim fuUpload = CType(e.Item.FindControl("fuUploadFileAttachment"), FileUpload)
                    currentRow.Attributes.Add("guid", question.Guid)
                    deleteIcon.Attributes.Add("guid", question.Guid)
                    fuUpload.Attributes.Add("guid", question.Guid)
                    Dim hasdocument = False
                    Dim answers = AMCCertRecertController.GetAnswerByQuestionId(question.QuestionId)
                    If answers IsNot Nothing AndAlso answers.Count > 0 Then
                        Dim userdefinedResponses = AMCCertRecertController.GetResponseByQuestionId(question.QuestionId)
                        Dim userDefinedResponse As IUserDefinedCustomerSurveyResponses = Nothing
                        If userdefinedResponses IsNot Nothing AndAlso userdefinedResponses.Count > 0 Then
                            userDefinedResponse = userdefinedResponses(0)
                            Dim hdResponseId = CType(e.Item.FindControl("hdResponseId"), HiddenField)
                            hdResponseId.Value = userDefinedResponse.ResponseId.ToString()
                            'TODO: load document
                            Dim linkLocation As String = String.Empty
                            Dim fileName = GetFileNameOfDocument(DocumentationType.REFERENCE_VERFICATION_ALTERNATIVE.ToString(),
                                                         userDefinedResponse.Guid.ToString(),
                                                         userDefinedResponse.ResponseId.ToString(),
                                                         linkLocation)
                            If Not String.IsNullOrEmpty(fileName) Then
                                Dim hdUploadDocumentName = CType(e.Item.FindControl("hdUploadDocumentName"), HiddenField)
                                hdUploadDocumentName.Value = fileName
                                Dim hdUploaddocumentLink = CType(e.Item.FindControl("hdUploaddocumentLink"), HiddenField)
                                hdUploaddocumentLink.Value = linkLocation
                                hasdocument = True
                            End If
                        End If
                        Dim hdAnswerYes = CType(e.Item.FindControl("hdAnswerYes"), HiddenField)
                        Dim hdAnswerNo = CType(e.Item.FindControl("hdAnswerNo"), HiddenField)
                        For Each answer As UserDefinedSurveyAnswers In answers
                            If answer.AnswerText.ToUpper() = "YES" Then
                                hdAnswerYes.Value = answer.AnswerId.ToString()
                            ElseIf answer.AnswerText.ToUpper() = "NO" Then
                                hdAnswerNo.Value = answer.AnswerId.ToString()
                            End If
                            If userDefinedResponse IsNot Nothing AndAlso userDefinedResponse.AnswerId = answer.AnswerId Then
                                Dim chkAlternativeVerification = CType(e.Item.FindControl("chkAlternativeVerification"), CheckBox)
                                chkAlternativeVerification.Checked = (answer.AnswerText.ToUpper() = "YES" AndAlso hasdocument)
                                If Not chkAlternativeVerification.Checked Then
                                    _allAlternativeItemCheck = False
                                End If
                            End If
                        Next
                    End If
                End If
            End If
        End Sub

        Private Sub SetPrintMode(ByVal customerContacts As IUserDefinedCustomerExternalContacts)

            If PrintMode Then
                rptPrintReferences.Visible = True
                rptReferences.Visible = False
                AddHandler rptPrintReferences.ItemDataBound, AddressOf RptReferencesItemDataBound
                rptPrintReferences.DataSource = customerContacts
                rptPrintReferences.DataBind()
            Else
                rptPrintReferences.Visible = False
                rptReferences.Visible = True
                rptReferences.DataSource = customerContacts
                rptReferences.DataBind()
            End If
        End Sub

        Private Function SetPropertiesForObject(ByRef customerExternalContract As UserDefinedCustomerExternalContact) As Boolean
            Dim validResult As Boolean = True
            If customerExternalContract Is Nothing Then
                Return False
            End If
            For Each issue As IIssue In customerExternalContract.ValidationIssuesForMe
                customerExternalContract.ValidationIssues.Remove(issue)
            Next
            With customerExternalContract
                .ContactTypeString = CustomerContactTypeEnum.REFERENCES.ToString()
                .RelatedMasterCustomerId = MasterCustomerId
                .RelatedSubcustomerId = SubCustomerId
                ReplaceHTMLTag(txtFirstName)
                ReplaceHTMLTag(txtMiddleName)
                ReplaceHTMLTag(txtName)
                .FirstName = HttpUtility.HtmlDecode(txtFirstName.Text).TrimStart().TrimEnd()
                .MiddleName = HttpUtility.HtmlDecode(txtMiddleName.Text).TrimStart().TrimEnd()
                .LastName = HttpUtility.HtmlDecode(txtName.Text).TrimStart().TrimEnd()
                .Employer = txtEmployer.Text.TrimStart().TrimEnd()
                .Degree = txtDegree.Text.TrimStart().TrimEnd()
                .JobTitle = txtTitle.Text.TrimStart().TrimEnd()
                .PrefContactMethodString = ddlPrefContactMethod.SelectedValue

                For Each codeItem As CodeEntry In .ContactClassType.List
                    If codeItem.Code.Equals(rdlContactClass.SelectedValue) Then
                        .ContactClassType = codeItem.ToCodeObject()
                        Exit For
                    End If
                Next
            End With
            Dim isSetHomeAddress As Boolean = False
            Dim isSetOfficeAddress As Boolean = False
            Dim isSetHomePhoneCommunication As Boolean = False
            Dim isSetWorkPhoneCommunication As Boolean = False
            Dim isSetHomeEmailCommunication As Boolean = False
            Dim isSetWorkEmailCommunication As Boolean = False
            '' Set address
            isSetHomeAddress = SetAddress(customerExternalContract, isSetHomeAddress, isSetOfficeAddress)
            ''set communication 
            isSetHomePhoneCommunication = SetExternalCommunication(customerExternalContract, isSetHomePhoneCommunication, isSetHomeEmailCommunication, isSetWorkPhoneCommunication, isSetWorkEmailCommunication)
            SetExternalHomeAddress(customerExternalContract, isSetHomeAddress)
            SetExternalOfficeAddress(customerExternalContract, isSetOfficeAddress)
            ''set communication in case of not existing
            SetExternalHomePhoneCommunication(customerExternalContract, isSetHomePhoneCommunication)
            SetExternalHomeEmailCommunication(customerExternalContract, isSetHomeEmailCommunication)
            SetExternalWorkPhoneCommunication(customerExternalContract, isSetWorkPhoneCommunication)
            SetExternalWorkEmailCommunication(customerExternalContract, isSetWorkEmailCommunication)


            If customerExternalContract.ContactClassTypeString.Equals(CustomerContactClassEnum.PROFESSIONAL.ToString()) Then
                Dim customerContactList = AMCCertRecertController.GetCustomerExternalContacts(CustomerContactTypeEnum.REFERENCES.ToString(),
                                                                                         MasterCustomerId,
                                                                                         SubCustomerId)
                Dim professionContactItem = customerContactList.FindObject("ContactClassTypeString",
                                                                           CustomerContactClassEnum.PROFESSIONAL.ToString())
                If professionContactItem IsNot Nothing Then
                    If Not professionContactItem.Guid.Equals(customerExternalContract.Guid) Then
                        DuplicateItemIssues.Assert(True, customerExternalContract,
                                                   Localization.GetString("DuplicateProfessionalObject.Text", LocalResourceFile))

                    End If
                End If
            End If
            If customerExternalContract.ValidationIssuesForMe.Count > 0 Then
                validResult = False
            End If
            Return validResult
        End Function

        Private Sub SetExternalWorkEmailCommunication(ByVal customerExternalContract As UserDefinedCustomerExternalContact, ByVal isSetWorkEmailCommunication As Boolean)
            Dim customerExternalWorkingCommunication As UserDefinedCustomerExternalCommunications

            If Not isSetWorkEmailCommunication Then
                customerExternalWorkingCommunication = CType(customerExternalContract.UserDefinedCustomerExternalCommunicationes.CreateNew(), UserDefinedCustomerExternalCommunications)
                customerExternalWorkingCommunication.IsNewObjectFlag = True
                customerExternalWorkingCommunication.CommunicationTypeString = CommunicationTypeEnum.EMAIL.ToString()
                customerExternalWorkingCommunication.CommunicationLocationString = CommunicationLocationEnum.OFFICE.ToString()
                customerExternalWorkingCommunication.FormattedPhoneAddress = txtWorkEmail.Text.TrimStart().TrimEnd()
                customerExternalContract.UserDefinedCustomerExternalCommunicationes.Add(customerExternalWorkingCommunication)
            End If
        End Sub

        Private Sub SetExternalWorkPhoneCommunication(ByVal customerExternalContract As UserDefinedCustomerExternalContact, ByVal isSetWorkPhoneCommunication As Boolean)
            Dim customerExternalWorkingCommunication As UserDefinedCustomerExternalCommunications

            If Not isSetWorkPhoneCommunication Then
                customerExternalWorkingCommunication = CType(customerExternalContract.UserDefinedCustomerExternalCommunicationes.CreateNew(), UserDefinedCustomerExternalCommunications)
                customerExternalWorkingCommunication.IsNewObjectFlag = True
                customerExternalWorkingCommunication.CommunicationTypeString = CommunicationTypeEnum.PHONE.ToString()
                customerExternalWorkingCommunication.CommunicationLocationString = CommunicationLocationEnum.OFFICE.ToString()
                customerExternalWorkingCommunication.FormattedPhoneAddress = txtWorkTelephone.Text.TrimStart().TrimEnd()
                customerExternalContract.UserDefinedCustomerExternalCommunicationes.Add(customerExternalWorkingCommunication)
            End If
        End Sub

        Private Sub SetExternalHomeEmailCommunication(ByVal customerExternalContract As UserDefinedCustomerExternalContact, ByVal isSetHomeEmailCommunication As Boolean)
            Dim customerExternalHomeCommunication As UserDefinedCustomerExternalCommunications

            If Not isSetHomeEmailCommunication Then
                customerExternalHomeCommunication = CType(customerExternalContract.UserDefinedCustomerExternalCommunicationes.CreateNew(), UserDefinedCustomerExternalCommunications)
                customerExternalHomeCommunication.IsNewObjectFlag = True
                customerExternalHomeCommunication.CommunicationTypeString = CommunicationTypeEnum.EMAIL.ToString()
                customerExternalHomeCommunication.CommunicationLocationString = CommunicationLocationEnum.HOME.ToString()
                customerExternalHomeCommunication.FormattedPhoneAddress = txtHomeEmail.Text.TrimStart().TrimEnd()
                customerExternalContract.UserDefinedCustomerExternalCommunicationes.Add(customerExternalHomeCommunication)
            End If
        End Sub

        Private Sub SetExternalHomePhoneCommunication(ByVal customerExternalContract As UserDefinedCustomerExternalContact, ByVal isSetHomePhoneCommunication As Boolean)
            Dim customerExternalHomeCommunication As UserDefinedCustomerExternalCommunications

            If Not isSetHomePhoneCommunication Then
                customerExternalHomeCommunication = CType(customerExternalContract.UserDefinedCustomerExternalCommunicationes.CreateNew(), UserDefinedCustomerExternalCommunications)
                customerExternalHomeCommunication.IsNewObjectFlag = True
                customerExternalHomeCommunication.CommunicationTypeString = CommunicationTypeEnum.PHONE.ToString()
                customerExternalHomeCommunication.CommunicationLocationString = CommunicationLocationEnum.HOME.ToString()
                customerExternalHomeCommunication.FormattedPhoneAddress = txtHomeTelephone.Text.TrimStart().TrimEnd()
                customerExternalContract.UserDefinedCustomerExternalCommunicationes.Add(customerExternalHomeCommunication)
            End If
        End Sub

        Private Sub SetExternalOfficeAddress(ByVal customerExternalContract As UserDefinedCustomerExternalContact, ByVal isSetOfficeAddress As Boolean)
            Dim customerExternalWorkingAddress As UserDefinedCustomerExternalAddress

            If Not isSetOfficeAddress Then
                customerExternalWorkingAddress = CType(customerExternalContract.UserDefinedCustomerExternalAddresses.CreateNew(), UserDefinedCustomerExternalAddress)
                customerExternalWorkingAddress.IsNewObjectFlag = True
                customerExternalWorkingAddress.AddressTypeString = AddressType.OFFICE.ToString()
                customerExternalWorkingAddress.Address1 = txtWorkAddress1.Text.TrimStart().TrimEnd()
                customerExternalWorkingAddress.Address2 = txtWorkAddress2.Text.TrimStart().TrimEnd()
                customerExternalWorkingAddress.Address3 = txtWorkAddress3.Text.TrimStart().TrimEnd()
                customerExternalWorkingAddress.Address4 = txtWorkAddress4.Text.TrimStart().TrimEnd()
                customerExternalContract.UserDefinedCustomerExternalAddresses.Add(customerExternalWorkingAddress)
                customerExternalWorkingAddress.City = txtCity.Text.TrimStart().TrimEnd()
                customerExternalWorkingAddress.State = ddlWorkState.SelectedValue
                customerExternalWorkingAddress.PostalCode = txtWorkZip.Text.TrimStart().TrimEnd()
            End If
        End Sub

        Private Sub SetExternalHomeAddress(ByVal customerExternalContract As UserDefinedCustomerExternalContact, ByVal isSetHomeAddress As Boolean)
            Dim customerExternalHomeAddress As UserDefinedCustomerExternalAddress

            ''set address in case of not existing
            If Not isSetHomeAddress Then
                customerExternalHomeAddress = CType(customerExternalContract.UserDefinedCustomerExternalAddresses.CreateNew(), UserDefinedCustomerExternalAddress)
                customerExternalHomeAddress.IsNewObjectFlag = True
                customerExternalHomeAddress.AddressTypeString = AddressType.HOME.ToString()
                customerExternalHomeAddress.Address1 = txtHomeAddress1.Text.TrimStart().TrimEnd()
                customerExternalHomeAddress.Address2 = txtHomeAddress2.Text.TrimStart().TrimEnd()
                customerExternalHomeAddress.Address3 = txtHomeAddress3.Text.TrimStart().TrimEnd()
                customerExternalHomeAddress.Address4 = txtHomeAddress4.Text.TrimStart().TrimEnd()
                customerExternalContract.UserDefinedCustomerExternalAddresses.Add(customerExternalHomeAddress)
                customerExternalHomeAddress.City = txtCity.Text.TrimStart().TrimEnd()
                customerExternalHomeAddress.State = ddlHomeState.SelectedValue
                customerExternalHomeAddress.PostalCode = txtHomeZip.Text.TrimStart().TrimEnd()
            End If
        End Sub

        Private Function SetExternalCommunication(ByVal customerExternalContract As UserDefinedCustomerExternalContact, ByVal isSetHomePhoneCommunication As Boolean, ByRef isSetHomeEmailCommunication As Boolean, ByRef isSetWorkPhoneCommunication As Boolean, ByRef isSetWorkEmailCommunication As Boolean) As Boolean

            For Each customerExternalCommunicationItem As IUserDefinedCustomerExternalCommunications In customerExternalContract.UserDefinedCustomerExternalCommunicationes

                If customerExternalCommunicationItem.CommunicationLocationString.Equals(CommunicationLocationEnum.HOME.ToString()) Then
                    If Not isSetHomePhoneCommunication AndAlso
                       customerExternalCommunicationItem.CommunicationTypeString.Equals(CommunicationTypeEnum.PHONE.ToString()) Then
                        customerExternalCommunicationItem.FormattedPhoneAddress = txtHomeTelephone.Text.TrimStart().TrimEnd()
                        isSetHomePhoneCommunication = True
                    Else
                        If Not isSetHomeEmailCommunication AndAlso
                           customerExternalCommunicationItem.CommunicationTypeString.Equals(CommunicationTypeEnum.EMAIL.ToString()) Then
                            customerExternalCommunicationItem.FormattedPhoneAddress = txtHomeEmail.Text.TrimStart().TrimEnd()
                            isSetHomeEmailCommunication = True
                        End If

                    End If
                End If

                If customerExternalCommunicationItem.CommunicationLocationString.Equals(CommunicationLocationEnum.OFFICE.ToString()) Then
                    If Not isSetWorkPhoneCommunication AndAlso
                       customerExternalCommunicationItem.CommunicationTypeString.Equals(CommunicationTypeEnum.PHONE.ToString()) Then
                        customerExternalCommunicationItem.FormattedPhoneAddress = txtWorkTelephone.Text.TrimStart().TrimEnd()
                        isSetWorkPhoneCommunication = True
                    Else
                        If Not isSetWorkEmailCommunication AndAlso
                           customerExternalCommunicationItem.CommunicationTypeString.Equals(CommunicationTypeEnum.EMAIL.ToString()) Then
                            customerExternalCommunicationItem.FormattedPhoneAddress = txtWorkEmail.Text.TrimStart().TrimEnd()
                            isSetWorkEmailCommunication = True
                        End If
                    End If
                End If
            Next
            Return isSetHomePhoneCommunication
        End Function

        Private Function SetAddress(ByVal customerExternalContract As UserDefinedCustomerExternalContact, ByVal isSetHomeAddress As Boolean, ByRef isSetOfficeAddress As Boolean) As Boolean

            For Each customerExternalAddressItem As IUserDefinedCustomerExternalAddress In customerExternalContract.UserDefinedCustomerExternalAddresses
                If (customerExternalAddressItem.AddressTypeString.Equals(AddressType.HOME.ToString()) AndAlso Not isSetHomeAddress) Then
                    customerExternalAddressItem.Address1 = txtHomeAddress1.Text.TrimStart().TrimEnd()
                    customerExternalAddressItem.Address2 = txtHomeAddress2.Text.TrimStart().TrimEnd()
                    customerExternalAddressItem.Address3 = txtHomeAddress3.Text.TrimStart().TrimEnd()
                    customerExternalAddressItem.Address4 = txtHomeAddress4.Text.TrimStart().TrimEnd()
                    customerExternalAddressItem.City = txtHomeCity.Text.TrimStart().TrimEnd()
                    customerExternalAddressItem.State = ddlHomeState.SelectedValue
                    customerExternalAddressItem.PostalCode = txtHomeZip.Text.TrimStart().TrimEnd()
                    isSetHomeAddress = True
                End If

                If (customerExternalAddressItem.AddressTypeString.Equals(AddressType.OFFICE.ToString()) AndAlso Not isSetOfficeAddress) Then
                    customerExternalAddressItem.Address1 = txtWorkAddress1.Text.TrimStart().TrimEnd()
                    customerExternalAddressItem.Address2 = txtWorkAddress2.Text.TrimStart().TrimEnd()
                    customerExternalAddressItem.Address3 = txtWorkAddress3.Text.TrimStart().TrimEnd()
                    customerExternalAddressItem.Address4 = txtWorkAddress4.Text.TrimStart().TrimEnd()
                    customerExternalAddressItem.City = txtCity.Text.TrimStart().TrimEnd()
                    customerExternalAddressItem.State = ddlWorkState.SelectedValue
                    customerExternalAddressItem.PostalCode = txtWorkZip.Text.TrimStart().TrimEnd()
                    isSetOfficeAddress = True
                End If
            Next
            Return isSetHomeAddress
        End Function

        Private Sub ReplaceHTMLTag(ByRef textBox As TextBox)
            If textBox.Text.Equals("&nbsp") Then
                textBox.Text = textBox.Text.Replace("&nbsp", String.Empty)
            End If
        End Sub

        Private Function CheckBusinessValidation() As IssuesCollection
            Dim issueCollection As New IssuesCollection
            Dim allAlternativeVerificationChecked As Boolean = CheckAlternativeVerification()
            If Not allAlternativeVerificationChecked Then
                CheckNormalVerification(issueCollection)
            Else
                hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
            End If

            Return issueCollection
        End Function

        ''' <summary>
        ''' Checks the normal verification.
        ''' </summary>
        ''' <param name="issueCollection">The issue collection.</param>
        Private Sub CheckNormalVerification(ByVal issueCollection As IssuesCollection)

            Dim customerExternalContacts = AMCCertRecertController.GetCustomerExternalContacts(CustomerContactTypeEnum.REFERENCES.ToString(),
                                                                                               MasterCustomerId,
                                                                                               SubCustomerId)
            Dim crrnContactItem = customerExternalContacts.FindObject("ContactClassTypeString", CustomerContactClassEnum.CRRN.ToString())
            Dim superContactItem = customerExternalContacts.FindObject("ContactClassTypeString", CustomerContactClassEnum.SUPERVISOR.ToString())
            Dim professionalContactItem = customerExternalContacts.FindObject("ContactClassTypeString",
                                                                              CustomerContactClassEnum.PROFESSIONAL.ToString())

            If (crrnContactItem IsNot Nothing OrElse superContactItem IsNot Nothing) AndAlso professionalContactItem IsNot Nothing Then
                hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
            Else
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                issueCollection.Add(New AtLeastOneRecordIssue(New BusinessObject(), Localization.GetString("AtleastExternalContact.Text",
                                                                                                           LocalResourceFile)))
            End If

            For Each externalContact As IUserDefinedCustomerExternalContact In customerExternalContacts
                If externalContact.ContactClassTypeString.Equals(CustomerContactClassEnum.PROFESSIONAL.ToString()) AndAlso
                   Not externalContact.Guid.Equals(professionalContactItem.Guid) Then
                    issueCollection.Add(New AtLeastOneRecordIssue(New BusinessObject(),
                                                                  Localization.GetString("HaveTwoProfessionalReferencesError.Text",
                                                                                         LocalResourceFile)))
                End If
            Next
        End Sub

        ''' <summary>
        ''' Checks the alternative verification.
        ''' </summary>
        ''' <returns></returns>
        Private Function CheckAlternativeVerification() As Boolean
            Dim survey As UserDefinedSurvey

            Dim allAlternativeVerificationChecked As Boolean = True
            Dim questionList = GetFieldInfo("QuestionList")
            If questionList IsNot Nothing AndAlso questionList.IsEnabled Then
                survey = CType(AMCCertRecertController.GetSurveyByTitle(ReferencAndVeryficationSurveyTitle), UserDefinedSurvey)
                If survey IsNot Nothing Then
                    Dim alternativeVerificationQuestions = survey.UserDefinedSurveyQuestions
                    If alternativeVerificationQuestions IsNot Nothing AndAlso alternativeVerificationQuestions.Count > 0 Then
                        For Each alternativeVerificationQuestion As UserDefinedSurveyQuestion In alternativeVerificationQuestions
                            If alternativeVerificationQuestion.Enabled Then
                                Dim answers = AMCCertRecertController.GetAnswerByQuestionId(alternativeVerificationQuestion.QuestionId)
                                If answers IsNot Nothing AndAlso answers.Count > 0 Then
                                    For Each userDefinedSurveyAnswere As UserDefinedSurveyAnswers In answers
                                        If userDefinedSurveyAnswere.AnswerText.ToUpper() = "YES" Then
                                            Dim responses = AMCCertRecertController.GetResponses(alternativeVerificationQuestion.QuestionId, userDefinedSurveyAnswere.AnswerId)
                                            If responses IsNot Nothing AndAlso responses.Count > 0 Then
                                                Dim currentreponse = responses(0)
                                                Dim linkLocation As String = String.Empty
                                                Dim fileName = GetFileNameOfDocument(DocumentationType.REFERENCE_VERFICATION_ALTERNATIVE.ToString(),
                                                                                     currentreponse.Guid.ToString(),
                                                                                     currentreponse.ResponseId.ToString(),
                                                                                     linkLocation)
                                                If String.IsNullOrEmpty(fileName) Then
                                                    allAlternativeVerificationChecked = False
                                                End If
                                            Else
                                                'not checked => 
                                                allAlternativeVerificationChecked = False
                                            End If
                                            Exit For
                                        End If
                                    Next
                                    If Not allAlternativeVerificationChecked Then
                                        Exit For
                                    End If
                                End If
                            End If
                        Next
                    End If
                End If
            Else
                allAlternativeVerificationChecked = False
            End If
            Return allAlternativeVerificationChecked
        End Function

#End Region
    End Class
End Namespace