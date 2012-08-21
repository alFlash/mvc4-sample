Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Business.Controller
Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports TIMSS.API.Core
Imports TIMSS.Enumerations

Imports TIMSS.API.CustomerInfo
Imports TIMSS.API.Core.Validation
Imports TIMSS.API.User.UserDefinedInfo

Namespace Controls.Common
    Public Class SupervisorUC
        Inherits SectionBaseUserControl

#Region "Private methods"
        ''' <summary>
        ''' set value properties for customer external contract object
        ''' </summary>
        ''' <param name="customerExternalContract"></param>
        ''' <remarks></remarks>
        Private Sub SetPropertiesForObject(ByRef customerExternalContract As UserDefinedCustomerExternalContact)
            If customerExternalContract Is Nothing Then
                Return
            End If
            With customerExternalContract
                .ContactTypeString = CustomerContactTypeEnum.SUPERVISOR.ToString()
                .RelatedMasterCustomerId = Me.MasterCustomerId
                .RelatedSubcustomerId = Me.SubCustomerId
                .FirstName = Server.HtmlDecode(Me.txtFirstName.Text).TrimStart().TrimEnd()
                .MiddleName = Server.HtmlDecode(Me.txtMiddleName.Text).TrimStart().TrimEnd()
                .LastName = Server.HtmlDecode(Me.txtLastName.Text).TrimStart().TrimEnd()
                .Employer = Me.txtInstitution.Text.TrimStart().TrimEnd()
                .JobTitle = Me.txtTitle.Text.TrimStart().TrimEnd()
                .PrefContactMethodString = Me.ddlPrefContactMethod.SelectedValue
                .Notify = Me.chkNotifySupervisor.Checked
                For Each codeItem As CodeEntry In .ContactClassType.List
                    If codeItem.Code.Equals(Me.rdlContactClass.SelectedValue) Then
                        .ContactClassType = codeItem.ToCodeObject()
                        Exit For
                    End If
                Next
            End With

            Dim customerExternalHomeAddress As UserDefinedCustomerExternalAddress
            Dim customerExternalWorkingAddress As UserDefinedCustomerExternalAddress
            Dim customerExternalHomeCommunication As UserDefinedCustomerExternalCommunications
            Dim customerExternalWorkingCommunication As UserDefinedCustomerExternalCommunications
            Dim isSetHomeAddress As Boolean = False
            Dim isSetOfficeAddress As Boolean = False
            Dim isSetHomePhoneCommunication As Boolean = False
            Dim isSetWorkPhoneCommunication As Boolean = False
            Dim isSetHomeEmailCommunication As Boolean = False
            Dim isSetWorkEmailCommunication As Boolean = False
            '' Set address
            For Each customerExternalAddressItem As IUserDefinedCustomerExternalAddress In customerExternalContract.UserDefinedCustomerExternalAddresses
                If (customerExternalAddressItem.AddressTypeString.Equals(AddressType.HOME.ToString()) AndAlso Not isSetHomeAddress) Then
                    customerExternalAddressItem.Address1 = Me.txtHomeAddress1.Text.TrimStart().TrimEnd()
                    customerExternalAddressItem.Address2 = Me.txtHomeAddress2.Text.TrimStart().TrimEnd()
                    customerExternalAddressItem.Address3 = Me.txtHomeAddress3.Text.TrimStart().TrimEnd()
                    customerExternalAddressItem.Address4 = Me.txtHomeAddress4.Text.TrimStart().TrimEnd()
                    customerExternalAddressItem.City = Me.txtHomeCity.Text.TrimStart().TrimEnd()
                    customerExternalAddressItem.State = Me.ddlHomeState.SelectedValue
                    customerExternalAddressItem.PostalCode = Me.txtHomeZip.Text.TrimStart().TrimEnd()
                    isSetHomeAddress = True
                End If

                If (customerExternalAddressItem.AddressTypeString.Equals(AddressType.OFFICE.ToString()) AndAlso Not isSetOfficeAddress) Then
                    customerExternalAddressItem.Address1 = Me.txtWorkAddress1.Text.TrimStart().TrimEnd()
                    customerExternalAddressItem.Address2 = Me.txtWorkAddress2.Text.TrimStart().TrimEnd()
                    customerExternalAddressItem.Address3 = Me.txtWorkAddress3.Text.TrimStart().TrimEnd()
                    customerExternalAddressItem.Address4 = Me.txtWorkAddress4.Text.TrimStart().TrimEnd()
                    customerExternalAddressItem.City = Me.txtCity.Text.TrimStart().TrimEnd()
                    customerExternalAddressItem.State = Me.ddlState.SelectedValue
                    customerExternalAddressItem.PostalCode = Me.txtZip.Text.TrimStart().TrimEnd()
                    isSetOfficeAddress = True
                End If
            Next

            ''set communication 
            For Each customerExternalCommunicationItem As IUserDefinedCustomerExternalCommunications In customerExternalContract.UserDefinedCustomerExternalCommunicationes

                If customerExternalCommunicationItem.CommunicationLocationString.Equals(CommunicationLocationEnum.HOME.ToString()) Then
                    If Not isSetHomePhoneCommunication AndAlso
                                    customerExternalCommunicationItem.CommunicationTypeString.Equals(CommunicationTypeEnum.PHONE.ToString()) Then
                        customerExternalCommunicationItem.FormattedPhoneAddress = txtHomeTelephone.Text.TrimStart().TrimEnd()
                        isSetHomePhoneCommunication = True
                    Else
                        If Not isSetHomeEmailCommunication AndAlso
                                    customerExternalCommunicationItem.CommunicationTypeString.Equals(CommunicationTypeEnum.EMAIL.ToString()) Then
                            customerExternalCommunicationItem.FormattedPhoneAddress = Me.txtHomeEmailAddress.Text.TrimStart().TrimEnd()
                            isSetHomeEmailCommunication = True
                        End If

                    End If
                End If

                If customerExternalCommunicationItem.CommunicationLocationString.Equals(CommunicationLocationEnum.OFFICE.ToString()) Then
                    If Not isSetWorkPhoneCommunication AndAlso
                            customerExternalCommunicationItem.CommunicationTypeString.Equals(CommunicationTypeEnum.PHONE.ToString()) Then
                        customerExternalCommunicationItem.FormattedPhoneAddress = Me.txtOfficeTelephone.Text.TrimStart().TrimEnd()
                        isSetWorkPhoneCommunication = True
                    Else
                        If Not isSetWorkEmailCommunication AndAlso
                            customerExternalCommunicationItem.CommunicationTypeString.Equals(CommunicationTypeEnum.EMAIL.ToString()) Then
                            customerExternalCommunicationItem.FormattedPhoneAddress = Me.txtEmailAddress.Text.TrimStart().TrimEnd()
                            isSetWorkEmailCommunication = True
                        End If
                    End If
                End If
            Next

            ''set address in case of not existing
            If Not isSetHomeAddress Then
                customerExternalHomeAddress = CType(customerExternalContract.UserDefinedCustomerExternalAddresses.CreateNew(), UserDefinedCustomerExternalAddress)
                customerExternalHomeAddress.IsNewObjectFlag = True
                customerExternalHomeAddress.AddressTypeString = AddressType.HOME.ToString()
                customerExternalHomeAddress.Address1 = Me.txtHomeAddress1.Text.TrimStart().TrimEnd()
                customerExternalHomeAddress.Address2 = Me.txtHomeAddress2.Text.TrimStart().TrimEnd()
                customerExternalHomeAddress.Address3 = Me.txtHomeAddress3.Text.TrimStart().TrimEnd()
                customerExternalHomeAddress.Address4 = Me.txtHomeAddress4.Text.TrimStart().TrimEnd()
                customerExternalContract.UserDefinedCustomerExternalAddresses.Add(customerExternalHomeAddress)
                customerExternalHomeAddress.City = Me.txtHomeCity.Text.TrimStart().TrimEnd()
                customerExternalHomeAddress.State = Me.ddlHomeState.SelectedValue
                customerExternalHomeAddress.PostalCode = Me.txtHomeZip.Text.TrimEnd().TrimStart()
            End If

            If Not isSetOfficeAddress Then
                customerExternalWorkingAddress = CType(customerExternalContract.UserDefinedCustomerExternalAddresses.CreateNew(), UserDefinedCustomerExternalAddress)
                customerExternalWorkingAddress.IsNewObjectFlag = True
                customerExternalWorkingAddress.AddressTypeString = AddressType.OFFICE.ToString()
                customerExternalWorkingAddress.Address1 = Me.txtWorkAddress1.Text.TrimStart().TrimEnd()
                customerExternalWorkingAddress.Address2 = Me.txtWorkAddress2.Text.TrimStart().TrimEnd()
                customerExternalWorkingAddress.Address3 = Me.txtWorkAddress3.Text.TrimStart().TrimEnd()
                customerExternalWorkingAddress.Address4 = Me.txtWorkAddress4.Text.TrimStart().TrimEnd()
                customerExternalContract.UserDefinedCustomerExternalAddresses.Add(customerExternalWorkingAddress)
                customerExternalWorkingAddress.City = Me.txtCity.Text.TrimStart().TrimEnd()
                customerExternalWorkingAddress.State = Me.ddlState.SelectedValue
                customerExternalWorkingAddress.PostalCode = Me.txtZip.Text.TrimEnd().TrimStart()
            End If

            ''set communication in case of not existing
            If Not isSetHomePhoneCommunication Then
                customerExternalHomeCommunication = CType(customerExternalContract.UserDefinedCustomerExternalCommunicationes.CreateNew(), UserDefinedCustomerExternalCommunications)
                customerExternalHomeCommunication.IsNewObjectFlag = True
                customerExternalHomeCommunication.CommunicationTypeString = CommunicationTypeEnum.PHONE.ToString()
                customerExternalHomeCommunication.CommunicationLocationString = CommunicationLocationEnum.HOME.ToString()
                customerExternalHomeCommunication.FormattedPhoneAddress = txtHomeTelephone.Text.TrimStart().TrimEnd()
                customerExternalContract.UserDefinedCustomerExternalCommunicationes.Add(customerExternalHomeCommunication)
            End If

            If Not isSetHomeEmailCommunication Then
                customerExternalHomeCommunication = CType(customerExternalContract.UserDefinedCustomerExternalCommunicationes.CreateNew(), UserDefinedCustomerExternalCommunications)
                customerExternalHomeCommunication.IsNewObjectFlag = True
                customerExternalHomeCommunication.CommunicationTypeString = CommunicationTypeEnum.EMAIL.ToString()
                customerExternalHomeCommunication.CommunicationLocationString = CommunicationLocationEnum.HOME.ToString()
                customerExternalHomeCommunication.FormattedPhoneAddress = Me.txtHomeEmailAddress.Text.TrimStart().TrimEnd()
                customerExternalContract.UserDefinedCustomerExternalCommunicationes.Add(customerExternalHomeCommunication)
            End If


            If Not isSetWorkPhoneCommunication Then
                customerExternalWorkingCommunication = CType(customerExternalContract.UserDefinedCustomerExternalCommunicationes.CreateNew(), UserDefinedCustomerExternalCommunications)
                customerExternalWorkingCommunication.IsNewObjectFlag = True
                customerExternalWorkingCommunication.CommunicationTypeString = CommunicationTypeEnum.PHONE.ToString()
                customerExternalWorkingCommunication.CommunicationLocationString = CommunicationLocationEnum.OFFICE.ToString()
                customerExternalWorkingCommunication.FormattedPhoneAddress = Me.txtOfficeTelephone.Text.TrimStart().TrimEnd()
                customerExternalContract.UserDefinedCustomerExternalCommunicationes.Add(customerExternalWorkingCommunication)
            End If

            If Not isSetWorkEmailCommunication Then
                customerExternalWorkingCommunication = CType(customerExternalContract.UserDefinedCustomerExternalCommunicationes.CreateNew(), UserDefinedCustomerExternalCommunications)
                customerExternalWorkingCommunication.IsNewObjectFlag = True
                customerExternalWorkingCommunication.CommunicationTypeString = CommunicationTypeEnum.EMAIL.ToString()
                customerExternalWorkingCommunication.CommunicationLocationString = CommunicationLocationEnum.OFFICE.ToString()
                customerExternalWorkingCommunication.FormattedPhoneAddress = Me.txtEmailAddress.Text.TrimStart().TrimEnd()
                customerExternalContract.UserDefinedCustomerExternalCommunicationes.Add(customerExternalWorkingCommunication)
            End If
        End Sub

        Public Overrides Sub ValidateFormFillCompleted()
            AMCCertRecertController.RefreshCustomerExternalContacts(CustomerContactTypeEnum.SUPERVISOR.ToString(),
                                                                    MasterCustomerId,
                                                                    SubCustomerId)
            BindingDataToControl()
        End Sub


#End Region

#Region "Event Handlers"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                Me.regexEmailValid.ValidationExpression = CommonConstants.EMAIL_VALIDATION
                Me.regexHomeEmailValid.ValidationExpression = CommonConstants.EMAIL_VALIDATION
                Me.maskedHomeTelephoneValidator.InvalidValueMessage = Localization.GetString("PhoneErrorFormat.Text", LocalResourceFile)
                Me.maskedOfficeTelephoneValidator.InvalidValueMessage = Localization.GetString("PhoneErrorFormat.Text", LocalResourceFile)
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Private Sub BindingDataToControl()
            Dim customerExternalContacts = amcCertRecertController.GetCustomerExternalContacts(CustomerContactTypeEnum.SUPERVISOR.ToString(),
                                                                                                    Me.MasterCustomerId,
                                                                                                    Me.SubCustomerId)

            ''Bind PrefContactMethod
            CommonHelper.BindPrefContactMethod(Me.ddlPrefContactMethod)
            ''Bind Contact Class Type
            CommonHelper.BindContactClassType(Me.rdlContactClass)
            CommonHelper.BindIssuingBodyType(Me.ddlState, DocumentationType.LICENSURE.ToString())
            CommonHelper.BindIssuingBodyType(Me.ddlHomeState, DocumentationType.LICENSURE.ToString())
            Me.rdlContactClass.Items(0).Selected = True
            If (customerExternalContacts.Count > 0) Then
                Dim customerExternalContact = CType(customerExternalContacts(0), UserDefinedCustomerExternalContact)
                Dim isFillHomeAddress As Boolean = False
                Dim isFillOfficeAddress As Boolean = False
                Dim isFillHomeCommunication As Boolean = False
                Dim isFillWorkCommunication As Boolean = False
                Dim isFillHomeEmailCommunication As Boolean = False
                Dim isFillWorkEmailCommunication As Boolean = False
                Me.hdSupervisorId.Value = customerExternalContact.Guid
                Me.txtFirstName.Text = customerExternalContact.FirstName
                Me.txtMiddleName.Text = customerExternalContact.MiddleName
                Me.txtLastName.Text = customerExternalContact.LastName
                Me.txtTitle.Text = customerExternalContact.JobTitle
                Me.txtInstitution.Text = customerExternalContact.Employer
                Me.chkNotifySupervisor.Checked = customerExternalContact.Notify
                If Not String.IsNullOrEmpty(customerExternalContact.ContactClassTypeString) Then
                    Me.rdlContactClass.SelectedValue = customerExternalContact.ContactClassTypeString
                Else
                    Me.rdlContactClass.SelectedValue = customerExternalContact.ContactClassType.List(0).Code
                End If
                If Not String.IsNullOrEmpty(customerExternalContact.PrefContactMethodString) Then
                    Me.ddlPrefContactMethod.SelectedValue = customerExternalContact.PrefContactMethodString
                End If
                For Each externalAddress As IUserDefinedCustomerExternalAddress In customerExternalContact.UserDefinedCustomerExternalAddresses
                    If externalAddress.AddressTypeString.Equals(AddressType.HOME.ToString()) AndAlso Not isFillHomeAddress Then
                        isFillHomeAddress = True
                        Me.txtHomeAddress1.Text = externalAddress.Address1
                        Me.txtHomeAddress2.Text = externalAddress.Address2
                        Me.txtHomeAddress3.Text = externalAddress.Address3
                        Me.txtHomeAddress4.Text = externalAddress.Address4
                        Me.ddlHomeState.SelectedValue = externalAddress.State
                        Me.txtHomeCity.Text = externalAddress.City
                        Me.txtHomeZip.Text = externalAddress.PostalCode
                    End If

                    If externalAddress.AddressTypeString.Equals(AddressType.OFFICE.ToString()) AndAlso Not isFillOfficeAddress Then
                        isFillOfficeAddress = True
                        Me.txtWorkAddress1.Text = externalAddress.Address1
                        Me.txtWorkAddress2.Text = externalAddress.Address2
                        Me.txtWorkAddress3.Text = externalAddress.Address3
                        Me.txtWorkAddress4.Text = externalAddress.Address4
                        Me.ddlState.SelectedValue = externalAddress.State
                        Me.txtCity.Text = externalAddress.City
                        Me.txtZip.Text = externalAddress.PostalCode
                    End If
                Next




                For Each customerExternalCommunication As IUserDefinedCustomerExternalCommunications In
                                                        customerExternalContact.UserDefinedCustomerExternalCommunicationes
                    ''set home communication
                    If customerExternalCommunication.CommunicationLocationString.Equals(CommunicationLocationEnum.HOME.ToString()) AndAlso
                        Not isFillHomeCommunication AndAlso
                        customerExternalCommunication.CommunicationTypeString.Equals(CommunicationTypeEnum.PHONE.ToString()) Then
                        isFillHomeCommunication = True
                        Me.txtHomeTelephone.Text = customerExternalCommunication.FormattedPhoneAddress
                    End If

                    If customerExternalCommunication.CommunicationLocationString.Equals(CommunicationLocationEnum.HOME.ToString()) AndAlso
                        Not isFillHomeEmailCommunication AndAlso
                        customerExternalCommunication.CommunicationTypeString.Equals(CommunicationTypeEnum.EMAIL.ToString()) Then
                        isFillHomeEmailCommunication = True
                        Me.txtHomeEmailAddress.Text = customerExternalCommunication.FormattedPhoneAddress
                    End If

                    If customerExternalCommunication.CommunicationLocationString.Equals(CommunicationLocationEnum.OFFICE.ToString()) AndAlso
                       Not isFillWorkCommunication AndAlso
                       customerExternalCommunication.CommunicationTypeString.Equals(CommunicationTypeEnum.PHONE.ToString()) Then
                        isFillWorkCommunication = True
                        Me.txtOfficeTelephone.Text = customerExternalCommunication.FormattedPhoneAddress
                    End If

                    If customerExternalCommunication.CommunicationLocationString.Equals(CommunicationLocationEnum.OFFICE.ToString()) AndAlso
                        Not isFillWorkEmailCommunication AndAlso
                        customerExternalCommunication.CommunicationTypeString.Equals(CommunicationTypeEnum.EMAIL.ToString()) Then
                        isFillWorkEmailCommunication = True
                        Me.txtEmailAddress.Text = customerExternalCommunication.FormattedPhoneAddress
                    End If

                Next

                If Not Page.IsPostBack Then
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                End If
            End If
        End Sub
#End Region

#Region "Save"
        Public Overrides Function Save() As IIssuesCollection
            Dim results As IIssuesCollection = Nothing
            Try
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                results = SaveSupervisor()
                If results Is Nothing OrElse results.Count <= 0 Then
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                    AMCCertRecertController.RefreshCustomerExternalContacts(CustomerContactTypeEnum.SUPERVISOR.ToString(),
                                                                            MasterCustomerId,
                                                                            SubCustomerId)
                    BindingDataToControl()
                End If
                ''End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
            Return results
        End Function

        Private Function SaveSupervisor() As IIssuesCollection
            Dim result As IIssuesCollection = Nothing
            Dim customerExternalContact As UserDefinedCustomerExternalContact

            If hdSupervisorId.Value <> "0" Then
                customerExternalContact =
                CType(AMCCertRecertController.GetCustomerExternalContactByGuid(hdSupervisorId.Value,
                                                                CustomerContactTypeEnum.SUPERVISOR.ToString(),
                                                                                Me.MasterCustomerId,
                                                                                Me.SubCustomerId), 
                                                                    UserDefinedCustomerExternalContact)
                If customerExternalContact IsNot Nothing Then
                    SetPropertiesForObject(customerExternalContact)
                    result = AMCCertRecertController.UpdateCustomerExternalContact(customerExternalContact)
                End If
            Else
                customerExternalContact =
                    CType(amcCertRecertController.GetCustomerExternalContacts(CustomerContactTypeEnum.SUPERVISOR.ToString(),
                                                                              Me.MasterCustomerId,
                                                                              Me.SubCustomerId).CreateNew(), UserDefinedCustomerExternalContact)
                SetPropertiesForObject(customerExternalContact)
                customerExternalContact.IsNewObjectFlag = True
                result = amcCertRecertController.InsertCustomerExternalContact(customerExternalContact)
            End If
            If result Is Nothing OrElse result.Count < 1 Then
                result = amcCertRecertController.CommitCustomerExternalContacts(
                                                           CustomerContactTypeEnum.SUPERVISOR.ToString(),
                                                           Me.MasterCustomerId,
                                                           Me.SubCustomerId)
            End If
            BindingDataToControl()
            Return result
        End Function
#End Region
    End Class
End Namespace