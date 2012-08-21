Imports AMC.DNN.Modules.CertRecert.Business.Entities
Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Data.Entities
Imports AMC.DNN.Modules.CertRecert.Business.Controller
Imports AMC.DNN.Modules.CertRecert.Controls.Reusable

Imports DotNetNuke.Services.Log.EventLog
Imports DotNetNuke.UI.UserControls
Imports System.Threading
Imports System.IO

Imports TIMSS.API.User.CertificationInfo
Imports TIMSS.API.NotificationServiceInfo

Imports Personify.ApplicationManager

Imports System.Linq
Imports TIMSS.API.User.UserDefinedInfo
Imports ICSharpCode.SharpZipLib.Zip

Namespace Controls.FormConfigurations
    Public Class ConfigureInactiveStatusUC
        Inherits PersonifyDNNBaseForm

#Region "Properties"

        Public Property CEWeightSettings As UserDefinedCertificationCEWeights
        Private ReadOnly _exceptionLogController As ExceptionLogController = New ExceptionLogController()
        Public Property ShowErrorMessage As Action(Of String)

        Public Property ParentModulePath() As String

        ''' <summary>
        ''' Gets or sets the pop up rich text editor mode.
        ''' </summary>
        ''' <value>
        ''' The pop up rich text editor mode.
        ''' </value>
        Public Property PopUpRichTextEditorMode() As String
            Get
                Return CType(teMessage, TextEditor).Mode
            End Get
            Set(ByVal value As String)
            End Set
        End Property

        Public Property IsShowReviewProcessPopup() As Boolean = False

        ''' <summary>
        ''' Gets or sets the notification settings.
        ''' </summary>
        ''' <value>
        ''' The notification settings.
        ''' </value>
        Public ReadOnly Property NotificationSettings() As List(Of NotificationInfo)
            Get
                Dim result = New List(Of NotificationInfo)
                For Each listItem As ListItem In cblNotificationSettings.Items
                    Dim notificationInfo = New NotificationInfo
                    notificationInfo.NotificationId = listItem.Value
                    notificationInfo.NotificationText = listItem.Text
                    notificationInfo.IsEnabled = listItem.Selected
                    result.Add(notificationInfo)
                Next
                Return result
            End Get
        End Property

        Public WriteOnly Property PersonifyNotificationServices() As INotificationserviceNotifications
            Set(ByVal value As INotificationserviceNotifications)
                cblNotificationSettings.DataValueField = "NotificationId"
                cblNotificationSettings.DataTextField = "NotificationName"
                cblNotificationSettings.DataSource = value
                cblNotificationSettings.DataBind()
                For Each listItem As ListItem In cblNotificationSettings.Items
                    For Each notificationInfo As INotificationserviceNotification In value
                        If notificationInfo.NotificationId = Convert.ToInt64(listItem.Value) Then
                            listItem.Selected = notificationInfo.ActiveFlag
                            Exit For
                        End If
                    Next
                Next
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the certification audit settings.
        ''' </summary>
        ''' <value>
        ''' The certification audit settings.
        ''' </value>
        Public Property CertificationAuditSettings() As CertificationAuditSetting
            Get
                Dim result = New CertificationAuditSetting
                result.StartDate = Convert.ToDateTime(txtCertAuditStartDate.Text)
                result.EndDate = Convert.ToDateTime(txtCertAuditEndDate.Text)
                result.LastAuditRun = Convert.ToDateTime(txtCertLastAuditRun.Text)
                result.SelectionRatio = txtCertSelectionRatio.Text
                Return result
            End Get
            Set(ByVal value As CertificationAuditSetting)
                txtCertAuditStartDate.Text = value.StartDate.ToString(CommonConstants.DATE_FORMAT)
                txtCertAuditEndDate.Text = value.EndDate.ToString(CommonConstants.DATE_FORMAT)
                txtCertLastAuditRun.Text = value.LastAuditRun.ToString(CommonConstants.DATE_FORMAT)
                txtCertSelectionRatio.Text = value.SelectionRatio.ToString()
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the re certification audit settings.
        ''' </summary>
        ''' <value>
        ''' The re certification audit settings.
        ''' </value>
        Public Property ReCertificationAuditSettings() As ReCertificationAuditSetting
            Get
                Dim result = New ReCertificationAuditSetting
                result.StartDate = Convert.ToDateTime(txtReCertAuditStartDate.Text)
                result.EndDate = Convert.ToDateTime(txtReCertAuditEndDate.Text)
                result.LastAuditRun = Convert.ToDateTime(txtReCertLastAuditRun.Text)
                result.SelectionRatio = txtReCertSelectionRatio.Text
                Return result
            End Get
            Set(ByVal value As ReCertificationAuditSetting)
                txtReCertAuditStartDate.Text = value.StartDate.ToString(CommonConstants.DATE_FORMAT)
                txtReCertAuditEndDate.Text = value.EndDate.ToString(CommonConstants.DATE_FORMAT)
                txtReCertLastAuditRun.Text = value.LastAuditRun.ToString(CommonConstants.DATE_FORMAT)
                txtReCertSelectionRatio.Text = value.SelectionRatio.ToString()
            End Set
        End Property

        Public Property OtherModuleSettings() As OtherModuleSettings
            Get
                Dim result = New OtherModuleSettings
                result.CertificationCode = txtCertificationCode.Text
                result.RecertificationCode = txtRecertificationCode.Text
                result.InactiveStatusProductCode = txtInactiveStatusProductCode.Text
                result.MembershipLink = txtMembershipLink.Text
                result.LicensureValidityDate = Convert.ToDateTime(txtLicensureValidityDate.Text)
                result.RecertProductId = Utils.ParseInteger(txtRecertProductId.Text, 0)
                result.InactiveStatusProductId = Utils.ParseInteger(txtInactiveStatusProductId.Text, 0)
                result.PersonifyShoppingCartWebService = txtPersonifyShoppingCartWebService.Text
                result.CheckoutURL = txtCheckoutURL.Text
                result.ReCertCMEQuestion1 = txtReCertCMEQuestion1.Text
                result.ReCertCMEQuestion2 = txtReCertCMEQuestion2.Text
                result.ReCertCMEQuestion3 = txtReCertCMEQuestion3.Text
                result.ReCertCMEQuestion4 = txtReCertCMEQuestion4.Text
                result.ProPracticeQuestionaireValidateMonth = txtProPracticeQuestionaireValidateMonth.Text
                result.ProfessionalPracticeQuestionaireStartYear = If(String.IsNullOrEmpty(txtProfessionalPracticeQuestionaireStartYear.Text), 0, Convert.ToInt64(txtProfessionalPracticeQuestionaireStartYear.Text))
                result.ProfessionalPracticeQuestionaireEndYear = If(String.IsNullOrEmpty(txtProfessionalPracticeQuestionaireEndYear.Text), 0, Convert.ToInt64(txtProfessionalPracticeQuestionaireEndYear.Text))
                result.CMEHourEarned = If(String.IsNullOrEmpty(txtCMEHoursEarned.Text), 0, Convert.ToInt64(txtCMEHoursEarned.Text))
                result.CMEStartYearRange = If(String.IsNullOrEmpty(txtCMEStartYear.Text), 0, Convert.ToInt64(txtCMEStartYear.Text))
                result.CMEEndYearRange = If(String.IsNullOrEmpty(txtCMEEndYear.Text), 0, Convert.ToInt64(txtCMEEndYear.Text))
                result.PresentationTotalHours = If(String.IsNullOrEmpty(txtPresentationTotalPoint.Text), 0, Convert.ToDecimal(txtPresentationTotalPoint.Text))
                result.EducationCourseTotalHours = If(String.IsNullOrEmpty(txtEducationCourseTotalPoint.Text), 0, Convert.ToDecimal(txtEducationCourseTotalPoint.Text))
                result.PublicationTotalHours = If(String.IsNullOrEmpty(txtPublicationTotalPoint.Text), 0, Convert.ToDecimal(txtPublicationTotalPoint.Text))
                result.CommunityServiceTotalHours = If(String.IsNullOrEmpty(txtCommunityServiceTotalPoint.Text), 0, Convert.ToDecimal(txtCommunityServiceTotalPoint.Text))
                result.ARNMaxSummaryPoint = If(String.IsNullOrEmpty(txtARNMaxSummaryPoints.Text), 0, Convert.ToInt32(txtARNMaxSummaryPoints.Text))
                result.ARNMaxSummaryPointOfContinuingEducation = If(String.IsNullOrEmpty(txtARNMaxSummaryPointOfContinuingEducation.Text), 0, Convert.ToDecimal(txtARNMaxSummaryPointOfContinuingEducation.Text))
                If String.IsNullOrEmpty(txtReCertificationCircle.Text) Then
                    result.ReCertificationCycle = Nothing
                Else
                    result.ReCertificationCycle = Convert.ToInt32(txtReCertificationCircle.Text)
                End If
                If String.IsNullOrEmpty(txtRecertificationPaymentMonths.Text) Then
                    result.RecertificationPaymentMonths = Nothing
                Else
                    result.RecertificationPaymentMonths = Convert.ToInt32(txtRecertificationPaymentMonths.Text)
                End If
                Return (result)
            End Get
            Set(ByVal value As OtherModuleSettings)
                txtCertificationCode.Text = If(value IsNot Nothing, value.CertificationCode, String.Empty)
                txtRecertificationCode.Text = If(value IsNot Nothing, value.RecertificationCode, String.Empty)
                txtInactiveStatusProductCode.Text = If(value IsNot Nothing, value.InactiveStatusProductCode, String.Empty)
                txtMembershipLink.Text = If(value IsNot Nothing, value.MembershipLink, String.Empty)
                txtLicensureValidityDate.Text = If(value IsNot Nothing, value.LicensureValidityDate.ToString(CommonConstants.DATE_FORMAT), String.Empty)
                txtRecertProductId.Text = If(value IsNot Nothing, value.RecertProductId.ToString(), "0")
                txtInactiveStatusProductId.Text = If(value IsNot Nothing, value.InactiveStatusProductId.ToString(), "0")
                txtPersonifyShoppingCartWebService.Text = If(value IsNot Nothing, value.PersonifyShoppingCartWebService, String.Empty)
                txtCheckoutURL.Text = If(value IsNot Nothing, value.CheckoutURL, String.Empty)
                txtReCertCMEQuestion1.Text = If(value IsNot Nothing, value.ReCertCMEQuestion1, "0")
                txtReCertCMEQuestion2.Text = If(value IsNot Nothing, value.ReCertCMEQuestion2, "0")
                txtReCertCMEQuestion3.Text = If(value IsNot Nothing, value.ReCertCMEQuestion3, "0")
                txtReCertCMEQuestion4.Text = If(value IsNot Nothing, value.ReCertCMEQuestion4, "0")
                txtProPracticeQuestionaireValidateMonth.Text = If(value IsNot Nothing, value.ProPracticeQuestionaireValidateMonth, "0")
                txtProfessionalPracticeQuestionaireStartYear.Text = If(value IsNot Nothing AndAlso value.ProfessionalPracticeQuestionaireStartYear IsNot Nothing, value.ProfessionalPracticeQuestionaireStartYear.ToString(), "0")
                txtProfessionalPracticeQuestionaireEndYear.Text = If(value IsNot Nothing AndAlso value.ProfessionalPracticeQuestionaireEndYear IsNot Nothing, value.ProfessionalPracticeQuestionaireEndYear.ToString(), "0")
                txtCMEHoursEarned.Text = If(value IsNot Nothing AndAlso value.CMEHourEarned IsNot Nothing, value.CMEHourEarned.ToString(), "0")
                txtCMEStartYear.Text = If(value IsNot Nothing AndAlso value.CMEStartYearRange IsNot Nothing, value.CMEStartYearRange.ToString(), "0")
                txtCMEEndYear.Text = If(value IsNot Nothing AndAlso value.CMEEndYearRange IsNot Nothing, value.CMEEndYearRange.ToString(), "0")
                txtPresentationTotalPoint.Text = If(value IsNot Nothing AndAlso value.PresentationTotalHours IsNot Nothing, value.PresentationTotalHours.ToString(), "0")
                txtEducationCourseTotalPoint.Text = If(value IsNot Nothing AndAlso value.EducationCourseTotalHours IsNot Nothing, value.EducationCourseTotalHours.ToString(), "0")
                txtPublicationTotalPoint.Text = If(value IsNot Nothing AndAlso value.PublicationTotalHours IsNot Nothing, value.PublicationTotalHours.ToString(), "0")
                txtCommunityServiceTotalPoint.Text = If(value IsNot Nothing AndAlso value.CommunityServiceTotalHours IsNot Nothing, value.CommunityServiceTotalHours.ToString(), "0")
                txtReCertificationCircle.Text = If(value IsNot Nothing AndAlso value.ReCertificationCycle IsNot Nothing, value.ReCertificationCycle.ToString(), String.Empty)
                txtRecertificationPaymentMonths.Text = If(value IsNot Nothing AndAlso value.RecertificationPaymentMonths IsNot Nothing, value.RecertificationPaymentMonths.ToString(), String.Empty)
                txtARNMaxSummaryPoints.Text = If(value IsNot Nothing AndAlso value.ARNMaxSummaryPoint IsNot Nothing, value.ARNMaxSummaryPoint.ToString(), String.Empty)
                txtARNMaxSummaryPointOfContinuingEducation.Text = If(value IsNot Nothing AndAlso value.ARNMaxSummaryPointOfContinuingEducation IsNot Nothing, value.ARNMaxSummaryPointOfContinuingEducation.ToString(), "0")
            End Set
        End Property

#End Region

#Region "Private Member"

        Private _amcCertRecertController As AmcCertRecertController

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
                CommonHelper.BindCertificationType(ddlProcessType)
                CheckUserRole()
                RegisterJavascript()
                InitializeComponents()
                AttatchEventHandlers()
                If Not Page.IsPostBack Then
                    If Not CheckForSurveyAndQuestions() Then
                        Dim worker As New Thread(AddressOf BuildSurveyAndQuestions)
                        worker.IsBackground = True
                        worker.Start()
                    End If
                    LoadModuleConfigurations()
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Private Sub BuildSurveyAndQuestions()
            Dim surveys = _amcCertRecertController.GetSurveys(OrganizationId, OrganizationUnitId, MasterCustomerId)
            _amcCertRecertController.BuildSurveysAndQuestions(surveys, OrganizationId, OrganizationUnitId, MasterCustomerId)
        End Sub

        Private Function CheckForSurveyAndQuestions() As Boolean
            Dim surveys = _amcCertRecertController.GetSurveys(OrganizationId, OrganizationUnitId, MasterCustomerId)
            Dim hasQuestions = False

            If surveys IsNot Nothing AndAlso surveys.Count > 0 Then
                For Each userDefinedSurvey As UserDefinedSurvey In surveys
                    If userDefinedSurvey.UserDefinedSurveyQuestions IsNot Nothing AndAlso userDefinedSurvey.UserDefinedSurveyQuestions.Count > 0 Then
                        hasQuestions = True
                        Exit For
                    End If
                Next
            End If
            Return hasQuestions
        End Function

        Private Sub CheckUserRole()
            If Not (UserInfo.IsSuperUser OrElse UserInfo.IsInRole("Administrators") OrElse UserInfo.IsInRole("CERTADMIN") OrElse UserInfo.IsInRole("Host")) Then
                Response.Redirect(NavigateURL(), True)
            End If
        End Sub

        Private Sub RegisterJavascript()
            Page.ClientScript.RegisterClientScriptInclude("configuration", String.Format("{0}/Documentation/scripts/configurations.js?v={1}", ParentModulePath, CommonConstants.CURRENT_VERSION))
        End Sub

        ''' <summary>
        ''' Handles the Event click of BtnOK
        ''' Change the name of form/section/field
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Private Sub BtnOKClick(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Dim newValue = GetConfigurationValue()

                If Not String.IsNullOrEmpty(hdConfigurationFormId.Value) AndAlso Not String.IsNullOrEmpty(newValue) Then
                    If Not String.IsNullOrEmpty(hdConfigurationSectionId.Value) Then
                        If Not String.IsNullOrEmpty(hdConfiguratonFieldId.Value) Then 'Edit FieldName
                            ModuleConfigurationHelper.Instance.UpdateFieldName(hdConfigurationFormId.Value, hdConfigurationSectionId.Value, hdConfiguratonFieldId.Value, OrganizationId, OrganizationUnitId, ModuleId, Server.MapPath(ParentModulePath), newValue)
                        Else 'Edit the Section Name
                            ModuleConfigurationHelper.Instance.UpdateSectionName(hdConfigurationFormId.Value, hdConfigurationSectionId.Value, OrganizationId, OrganizationUnitId, ModuleId, Server.MapPath(ParentModulePath), newValue)
                        End If
                    Else 'Edit the Form Name
                        ModuleConfigurationHelper.Instance.UpdateFormName(hdConfigurationFormId.Value, OrganizationId, OrganizationUnitId, ModuleId, Server.MapPath(ParentModulePath), newValue)
                    End If
                End If
                ResetPopupRelatedFields()
                LoadFormConfigurations()
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        ''' <summary>
        ''' BTNs the save click.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Private Sub BtnSaveClick(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Dim configurations = ModuleConfigurationHelper.Instance.GetFormConfigurations(ModuleId, Server.MapPath(ParentModulePath), OrganizationId, OrganizationUnitId)
                Dim hasChanged = False
                If rptFormComfiguration.Items IsNot Nothing Then
                    For Each formItem As RepeaterItem In rptFormComfiguration.Items
                        Dim formId = CType(formItem.FindControl("hdFormId"), HiddenField).Value
                        Dim isVisible = CType(formItem.FindControl("chkFormIsEnabled"), CheckBox).Checked
                        Dim oldVisible = Convert.ToBoolean(CType(formItem.FindControl("hdIsVisible"), HiddenField).Value)
                        Dim originalConfigurable = Convert.ToBoolean(CType(formItem.FindControl("hdIsFormConfigurable"), HiddenField).Value)
                        Dim newConfigurable = CType(formItem.FindControl("chkIsFormConfigurable"), CheckBox).Checked
                        If isVisible <> oldVisible OrElse originalConfigurable <> newConfigurable Then
                            For Each formInfo As FormInfo In configurations
                                If formInfo.FormId = formId Then
                                    formInfo.IsVisible = isVisible
                                    formInfo.IsConfigurable = newConfigurable
                                    hasChanged = True
                                    Exit For
                                End If
                            Next
                        End If

                        Dim sectionRepeater = CType(formItem.FindControl("rptSectionConfiguration"), Repeater)
                        If sectionRepeater IsNot Nothing AndAlso sectionRepeater.Items IsNot Nothing Then
                            UpdateSectionInfos(sectionRepeater.Items, formId, configurations, hasChanged)
                        End If
                    Next
                End If
                If hasChanged Then
                    ModuleConfigurationHelper.Instance.UpdateConfigurations(configurations, ModuleId, Server.MapPath(ParentModulePath))
                    LoadFormConfigurations()
                End If

                ModuleConfigurationHelper.Instance.SaveSettings(Server.MapPath(ParentModulePath), CommonConstants.OTHER_MODULE_SETTING_FILE_PATH, OtherModuleSettings)
                Dim results = _amcCertRecertController.UpdateNotifications(OrganizationId, OrganizationUnitId, NotificationSettings)
                If results Is Nothing OrElse results.Count <= 0 Then
                    PersonifyNotificationServices = _amcCertRecertController.GetNotifications(OrganizationId, OrganizationUnitId)
                End If
            Catch ex As Exception
                ProcessException(New Exception(Localization.GetString("ConcurrencyUsers.Text", LocalResourceFile)))
            End Try
        End Sub

        ''' <summary>
        ''' Updates the section infos.
        ''' </summary>
        ''' <param name="repeaterItemCollection">The repeater item collection.</param>
        ''' <param name="formId">The form id.</param>
        ''' <param name="formInfos">The form infos.</param>
        Private Shared Sub UpdateSectionInfos(ByVal repeaterItemCollection As IEnumerable, ByVal formId As String, ByRef formInfos As List(Of FormInfo), ByRef hasChanged As Boolean)
            For Each sectionItem As RepeaterItem In repeaterItemCollection
                Dim sectionId = CType(sectionItem.FindControl("hdSectionId"), HiddenField).Value
                Dim oldSectionEnabled = Convert.ToBoolean(CType(sectionItem.FindControl("hdSectionIsEnabled"), HiddenField).Value)
                Dim newSectionEnabled = CType(sectionItem.FindControl("chkSectionIsEnabled"), CheckBox).Checked
                Dim oldSequence = Convert.ToInt32(CType(sectionItem.FindControl("hdOriginalSectionSequence"), HiddenField).Value)
                Dim newSequence = Convert.ToInt32(CType(sectionItem.FindControl("hdSectionCurrentSequence"), HiddenField).Value)
                Dim originalConfigurable = Convert.ToBoolean(CType(sectionItem.FindControl("hdIsSectionConfigurable"), HiddenField).Value)
                Dim newConfigurable = CType(sectionItem.FindControl("chkIsSectionConfigurable"), CheckBox).Checked
                If oldSectionEnabled <> newSectionEnabled OrElse oldSequence <> newSequence OrElse newConfigurable <> originalConfigurable Then
                    UpdateSectionInfo(newSectionEnabled, newSequence, newConfigurable, formInfos, formId, sectionId)
                    hasChanged = True
                End If

                Dim fieldRepeater = CType(sectionItem.FindControl("rptFieldConfiguration"), Repeater)
                If fieldRepeater IsNot Nothing AndAlso fieldRepeater.Items IsNot Nothing Then
                    UpdateFieldInfos(fieldRepeater.Items, formId, sectionId, formInfos, hasChanged)
                End If
            Next
        End Sub

        ''' <summary>
        ''' Updates the section info.
        ''' </summary>
        ''' <param name="newSectionEnabled">if set to <c>true</c> [new section enabled].</param>
        ''' <param name="formInfos">The form infos.</param>
        ''' <param name="formId">The form id.</param>
        ''' <param name="sectionId">The section id.</param>
        Private Shared Sub UpdateSectionInfo(ByVal newSectionEnabled As Boolean, ByVal newSequence As Integer, ByVal isConfigurable As Boolean, ByRef formInfos As List(Of FormInfo), ByVal formId As String, ByVal sectionId As String)
            For Each formInfo As FormInfo In formInfos
                If formInfo.FormId = formId AndAlso formInfo.Sections IsNot Nothing Then
                    For Each sectionInfo As SectionInfo In formInfo.Sections
                        If sectionInfo.SectionId = sectionId Then
                            sectionInfo.IsEnabled = newSectionEnabled
                            sectionInfo.Sequence = newSequence
                            sectionInfo.IsConfigurable = isConfigurable
                            Exit For
                        End If
                    Next
                    Exit For
                End If
            Next
        End Sub

        ''' <summary>
        ''' Updates the field infos.
        ''' </summary>
        ''' <param name="repeaterItemCollection">The repeater item collection.</param>
        ''' <param name="formId">The form id.</param>
        ''' <param name="sectionId">The section id.</param>
        ''' <param name="formInfos">The form infos.</param>
        Private Shared Sub UpdateFieldInfos(ByVal repeaterItemCollection As IEnumerable, ByVal formId As String,
                                            ByVal sectionId As String, ByRef formInfos As List(Of FormInfo), ByRef hasChanged As Boolean)
            For Each fieldItem As RepeaterItem In repeaterItemCollection
                Dim fieldId = CType(fieldItem.FindControl("hdFieldId"), HiddenField).Value
                Dim oldFieldEnabled = Convert.ToBoolean(CType(fieldItem.FindControl("hdFieldIsEnabled"), HiddenField).Value)
                Dim newFieldEnabled = CType(fieldItem.FindControl("chkFieldIsEnable"), CheckBox).Checked
                Dim oldFieldRequired = Convert.ToBoolean(CType(fieldItem.FindControl("hdFieldIsRequired"), HiddenField).Value)
                Dim newFieldRequired = CType(fieldItem.FindControl("chkFieldIsRequired"), CheckBox).Checked
                Dim originalConfigurable = Convert.ToBoolean(CType(fieldItem.FindControl("hdIsFieldConfigurable"), HiddenField).Value)
                Dim newConfigurable = CType(fieldItem.FindControl("chkIsFieldConfigurable"), CheckBox).Checked

                Dim newFormular = String.Empty
                Dim originalFormular = String.Empty
                Dim isCalculable = Convert.ToBoolean(CType(fieldItem.FindControl("hdIsCalculate"), HiddenField).Value)
                If (isCalculable) Then
                    newFormular = CType(fieldItem.FindControl("txtCalculateFormular"), TextBox).Text
                    originalFormular = CType(fieldItem.FindControl("hdCalculateFormular"), HiddenField).Value
                    If String.IsNullOrEmpty(newFormular) Then
                        newFormular = "0.00"
                    End If
                End If
                If oldFieldEnabled <> newFieldEnabled OrElse oldFieldRequired <> newFieldRequired OrElse originalConfigurable <> newConfigurable OrElse newFormular <> originalFormular Then
                    UpdateFieldInfo(newFieldEnabled, newFieldRequired, newConfigurable, newFormular, formInfos, formId, sectionId, fieldId)
                    Dim sessionKey = String.Format("_SS_AMC_Config_Form{0}_Section{1}_Field{2}", formId, sectionId, fieldId)
                    HttpContext.Current.Session(sessionKey) = Nothing
                    hasChanged = True
                End If
            Next
        End Sub

        ''' <summary>
        ''' Updates the field info.
        ''' </summary>
        ''' <param name="newFieldEnabled">if set to <c>true</c> [new field enabled].</param>
        ''' <param name="newFieldRequired">if set to <c>true</c> [new field required].</param>
        ''' <param name="formInfos">The form infos.</param>
        ''' <param name="formId">The form id.</param>
        ''' <param name="sectionId">The section id.</param>
        ''' <param name="fieldId">The field id.</param>
        Private Shared Sub UpdateFieldInfo(ByVal newFieldEnabled As Boolean, ByVal newFieldRequired As Boolean, ByVal isConfigurable As Boolean, ByVal newFormular As String, ByRef formInfos As List(Of FormInfo), ByVal formId As String, ByVal sectionId As String, ByVal fieldId As String)
            For Each formInfo As FormInfo In formInfos
                If formInfo.FormId = formId AndAlso formInfo.Sections IsNot Nothing Then
                    For Each sectionInfo As SectionInfo In formInfo.Sections
                        If sectionInfo.SectionId = sectionId AndAlso sectionInfo.Fields IsNot Nothing Then
                            For Each fieldInfo As FieldInfo In sectionInfo.Fields
                                If fieldInfo.FieldId = fieldId Then
                                    fieldInfo.IsEnabled = newFieldEnabled
                                    fieldInfo.IsRequired = newFieldRequired
                                    fieldInfo.IsConfigurable = isConfigurable
                                    If Not String.IsNullOrEmpty(newFormular) Then
                                        fieldInfo.CalculateFormula = Convert.ToDecimal(newFormular)
                                    End If
                                End If
                            Next
                            Exit For
                        End If
                    Next
                    Exit For
                End If
            Next
        End Sub

        ''' <summary>
        ''' BTNs the reset click.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Private Sub BtnResetClick(ByVal sender As Object, ByVal e As EventArgs)
            'Reset Survey
            Dim surveys = _amcCertRecertController.GetSurveys(OrganizationId, OrganizationUnitId, MasterCustomerId, False)
            _amcCertRecertController.DeleteAllSurveys(surveys)
        End Sub

        ''' <summary>
        ''' BTNs the build question click.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Private Sub BtnBuildQuestionClick(ByVal sender As Object, ByVal e As EventArgs)
            Dim surveys = _amcCertRecertController.GetSurveys(OrganizationId, OrganizationUnitId, MasterCustomerId)
            _amcCertRecertController.BuildSurveysAndQuestions(surveys, OrganizationId, OrganizationUnitId, MasterCustomerId)
        End Sub

        ''' <summary>
        ''' Forms configuration item data bound.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterItemEventArgs" /> instance containing the event data.</param>
        Private Sub FormConfigurationItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
            BuildFormConfigurationItem(e)
        End Sub

        ''' <summary>
        ''' Sections configuration item data bound.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterItemEventArgs" /> instance containing the event data.</param>
        Private Sub SectionConfigurationItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
            BuildSectionConfigurationItem(e)
        End Sub

        ''' <summary>
        ''' Saves the re certificate audit settings.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Private Sub SaveReCertificateAuditSettings(ByVal sender As Object, ByVal e As EventArgs)
            ModuleConfigurationHelper.Instance.SaveSettings(Server.MapPath(ParentModulePath), CommonConstants.RECERT_AUDIT_SETTINGS_FILE_PATH, ReCertificationAuditSettings)
            UpdateNotificationEventByStoreProcedureName(chkReCertEnableNotifyApplicant.Checked, CertificationTypeEnum.RECERTIFICATION.ToString())
        End Sub

        ''' <summary>
        ''' Saves the certificate audit settings.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Private Sub SaveCertificateAuditSettings(ByVal sender As Object, ByVal e As EventArgs)
            ModuleConfigurationHelper.Instance.SaveSettings(Server.MapPath(ParentModulePath), CommonConstants.CERT_AUDIT_SETTINGS_FILE_PATH, CertificationAuditSettings)
            UpdateNotificationEventByStoreProcedureName(chkCertEnableNotifyApplicant.Checked, CertificationTypeEnum.CERTIFICATION.ToString())
        End Sub

        Protected Sub BtnCertRunClick(ByVal sender As Object, ByVal e As EventArgs) Handles btnCertRun.Click
            Try
                StartRunningAudit(txtCertAuditStartDate.Text, txtCertAuditEndDate.Text, txtCertSelectionRatio.Text, chkCertEnableNotifyApplicant.Checked, CertificationTypeEnum.CERTIFICATION, OtherModuleSettings.CertificationCode)
                SaveAuditProcessLastRunningDate(AuditProcessRunningType.CertificationAudit)

            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Protected Sub BtnCertRunLastAuditsClick(ByVal sender As Object, ByVal e As EventArgs) Handles btnCertRunLastAudits.Click
            Try
                InitAuditRunningDate(AuditProcessRunningType.LastCertificationAudit)

                StartRunningAudit(txtCertAuditStartDate.Text, txtCertAuditEndDate.Text, txtCertSelectionRatio.Text, chkCertEnableNotifyApplicant.Checked, CertificationTypeEnum.CERTIFICATION, OtherModuleSettings.CertificationCode)
                SaveAuditProcessLastRunningDate(AuditProcessRunningType.CertificationAudit)
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Protected Sub BtnReCertRunClick(ByVal sender As Object, ByVal e As EventArgs) Handles btnReCertRun.Click
            Try
                StartRunningAudit(txtReCertAuditStartDate.Text, txtReCertAuditEndDate.Text, txtReCertSelectionRatio.Text, chkReCertEnableNotifyApplicant.Checked, CertificationTypeEnum.RECERTIFICATION, OtherModuleSettings.RecertificationCode)

                SaveAuditProcessLastRunningDate(AuditProcessRunningType.RecertificationAudit)
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Protected Sub BtnReCertRunLastAuditClick(ByVal sender As Object, ByVal e As EventArgs) Handles btnReCertRunLastAudit.Click
            Try
                InitAuditRunningDate(AuditProcessRunningType.LastRecertificationAudit)

                StartRunningAudit(txtReCertAuditStartDate.Text, txtReCertAuditEndDate.Text, txtReCertSelectionRatio.Text, chkReCertEnableNotifyApplicant.Checked, CertificationTypeEnum.RECERTIFICATION, OtherModuleSettings.RecertificationCode)

                SaveAuditProcessLastRunningDate(AuditProcessRunningType.RecertificationAudit)
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Protected Sub GrvSelectedApplicationForAuditRowDataBound(ByVal sender As Object, ByVal e As Web.UI.WebControls.GridViewRowEventArgs) Handles grvSelectedApplicationForAudit.RowDataBound
            Try
                Dim lblCusName = e.Row.FindControl("lblCustomerName")
                If (lblCusName IsNot Nothing) Then
                    Dim customerCertification As CertificationCustomerCertification = CType(e.Row.DataItem, CertificationCustomerCertification)
                    If (customerCertification IsNot Nothing) Then
                        CType(lblCusName, Label).Text = customerCertification.CustomerInfo.FirstName + "&nbsp;" + customerCertification.CustomerInfo.LastName
                    End If
                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

#End Region

#Region "Private Methods"

        ''' <summary>
        ''' Gets the configuration value.
        ''' </summary>
        ''' <returns></returns>
        Private Function GetConfigurationValue() As String
            Dim newValue As String
            If Convert.ToBoolean(hdCurrentItemIsRichTextMode.Value) Then
                newValue = CType(teMessage, TextEditor).Text
            Else
                newValue = txtNewValue.Text
            End If

            If Not String.IsNullOrEmpty(newValue) Then
                newValue = newValue.Replace("<p>", String.Empty)
                newValue = newValue.Replace("</p>", String.Empty)
            End If
            Return newValue
        End Function

        ''' <summary>
        ''' Resets the popup related fields.
        ''' </summary>
        Private Sub ResetPopupRelatedFields()
            hdConfigurationFormId.Value = String.Empty
            hdConfigurationSectionId.Value = String.Empty
            hdConfiguratonFieldId.Value = String.Empty
            hdOpeningPopUp.Value = String.Empty
            hdCurrentPopUpTitle.Value = String.Empty
            txtNewValue.Text = String.Empty
            CType(teMessage, TextEditor).Text = String.Empty
        End Sub

        ''' <summary>
        ''' Builds the section configuration item.
        ''' </summary>
        ''' <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterItemEventArgs" /> instance containing the event data.</param>
        Private Sub BuildSectionConfigurationItem(ByVal e As RepeaterItemEventArgs)
            Dim sectionConfiguration = CType(e.Item.DataItem, SectionInfo)
            Dim childControl = e.Item.FindControl("rptFieldConfiguration")
            Dim btnEditSectionName = e.Item.FindControl("imgEditSection")
            If btnEditSectionName IsNot Nothing AndAlso TypeOf btnEditSectionName Is Web.UI.HtmlControls.HtmlGenericControl Then
                CType(btnEditSectionName, Web.UI.HtmlControls.HtmlGenericControl).Attributes.Add("FormId", sectionConfiguration.FormId)
                CType(btnEditSectionName, Web.UI.HtmlControls.HtmlGenericControl).Attributes.Add("SectionId", sectionConfiguration.SectionId)
                CType(btnEditSectionName, Web.UI.HtmlControls.HtmlGenericControl).Attributes.Add("IsRichText", sectionConfiguration.IsRichText.ToString())
            End If
            If childControl IsNot Nothing Then
                Dim fieldRepeater = CType(childControl, Repeater)
                AddHandler fieldRepeater.ItemDataBound, AddressOf FieldRepeaterItemDatabound
                fieldRepeater.DataSource = sectionConfiguration.Fields
                fieldRepeater.DataBind()
            End If
        End Sub

        ''' <summary>
        ''' Fields the repeater item databound.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterItemEventArgs" /> instance containing the event data.</param>
        Private Sub FieldRepeaterItemDatabound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
            Dim fieldConfiguration = CType(e.Item.DataItem, FieldInfo)
            Dim btnEditFieldName = e.Item.FindControl("imgEditField")
            Dim hlNavigateToQuestList = e.Item.FindControl("hlFieldNameItem")
            Dim hoverDiv = e.Item.FindControl("divFieldHover")
            If hlNavigateToQuestList IsNot Nothing AndAlso TypeOf hlNavigateToQuestList Is HyperLink Then
                CType(hlNavigateToQuestList, HyperLink).NavigateUrl = NavigateToQuestionConfigurationPage(fieldConfiguration.FormId, fieldConfiguration.SectionId, fieldConfiguration.FieldId)
                If Not fieldConfiguration.IsQuestion AndAlso CType(hlNavigateToQuestList, HyperLink).CssClass.IndexOf("disabled") = -1 Then
                    CType(hlNavigateToQuestList, HyperLink).CssClass += " disabled"
                End If
            End If
            If btnEditFieldName IsNot Nothing AndAlso TypeOf btnEditFieldName Is Web.UI.HtmlControls.HtmlGenericControl Then
                CType(btnEditFieldName, Web.UI.HtmlControls.HtmlGenericControl).Attributes.Add("FormId", fieldConfiguration.FormId)
                CType(btnEditFieldName, Web.UI.HtmlControls.HtmlGenericControl).Attributes.Add("SectionId", fieldConfiguration.SectionId)
                CType(btnEditFieldName, Web.UI.HtmlControls.HtmlGenericControl).Attributes.Add("FieldId", fieldConfiguration.FieldId)
                CType(btnEditFieldName, Web.UI.HtmlControls.HtmlGenericControl).Attributes.Add("IsRichText", fieldConfiguration.IsRichText.ToString())
            End If
            Dim tdFieldEnabledCheckbox = e.Item.FindControl("tdFieldEnabledCheckbox")
            Dim tdIsFieldRequiredCheckbox = e.Item.FindControl("tdIsFieldRequiredCheckbox")
            Dim tdIsFieldConfigurableCheckBox = e.Item.FindControl("tdIsFieldConfigurableCheckBox")
            If hoverDiv IsNot Nothing AndAlso TypeOf hoverDiv Is Web.UI.HtmlControls.HtmlGenericControl AndAlso fieldConfiguration.IsInstruction Then
                CType(hoverDiv, Web.UI.HtmlControls.HtmlGenericControl).Style.Add("background-color", "darkgrey")
                CType(tdFieldEnabledCheckbox, Web.UI.HtmlControls.HtmlGenericControl).Style.Add("background-color", "darkgrey")
                CType(tdIsFieldRequiredCheckbox, Web.UI.HtmlControls.HtmlGenericControl).Style.Add("background-color", "darkgrey")
                CType(tdIsFieldConfigurableCheckBox, Web.UI.HtmlControls.HtmlGenericControl).Style.Add("background-color", "darkgrey")
            End If
        End Sub

        ''' <summary>
        ''' Builds the form configuration item.
        ''' </summary>
        ''' <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterItemEventArgs" /> instance containing the event data.</param>
        Private Sub BuildFormConfigurationItem(ByVal e As RepeaterItemEventArgs)
            Dim formConfiguration = CType(e.Item.DataItem, FormInfo)
            Dim childControl = e.Item.FindControl("rptSectionConfiguration")
            Dim btnEditForm = e.Item.FindControl("imgEditForm")
            If btnEditForm IsNot Nothing AndAlso TypeOf btnEditForm Is Web.UI.HtmlControls.HtmlGenericControl Then
                CType(btnEditForm, Web.UI.HtmlControls.HtmlGenericControl).Attributes.Add("FormId", formConfiguration.FormId)
                CType(btnEditForm, Web.UI.HtmlControls.HtmlGenericControl).Attributes.Add("IsRichText", formConfiguration.IsRichText.ToString())
            End If
            If childControl IsNot Nothing Then
                Dim sectionRepeater = CType(childControl, Repeater)
                AddHandler sectionRepeater.ItemDataBound, AddressOf SectionConfigurationItemDataBound
                sectionRepeater.DataSource = formConfiguration.Sections.OrderBy(Function(x) x.Sequence)
                sectionRepeater.DataBind()
            End If
        End Sub

        ''' <summary>
        ''' Loads the module configurations.
        ''' </summary>
        Private Sub LoadModuleConfigurations()
            If Not Page.IsPostBack Then
                FixConfigurationsSequences()
            End If

            'Form Configurations
            LoadFormConfigurations()

            'Notifications
            LoadNotificationServices()

            'Certification Settings
            LoadAuditSettings()

            'Certification Code
            OtherModuleSettings = ModuleConfigurationHelper.Instance.GetOtherModuleSettings(Server.MapPath(ParentModulePath))
        End Sub

        Private Sub FixConfigurationsSequences()
            Dim configurations = ModuleConfigurationHelper.Instance.GetFormConfigurations(ModuleId, Server.MapPath(ParentModulePath), OrganizationId, OrganizationUnitId)
            Dim wrongSequences = False
            If configurations IsNot Nothing AndAlso configurations.Count > 0 Then
                For Each configuration As FormInfo In configurations
                    If configuration.Sections IsNot Nothing AndAlso configuration.Sections.Count > 1 Then
                        configuration.Sections = configuration.Sections.OrderBy(Function(x) x.Sequence).ToList()

                        For i As Integer = 0 To configuration.Sections.Count - 1
                            Dim currentSection = configuration.Sections(i)
                            If currentSection.Sequence <> i Then
                                currentSection.Sequence = i
                                wrongSequences = True
                            End If
                        Next
                    End If
                Next
            End If
            If wrongSequences Then
                ModuleConfigurationHelper.Instance.UpdateConfigurations(configurations, ModuleId, Server.MapPath(ParentModulePath))
            End If
        End Sub


        Private Sub LoadAuditSettings()
            CertificationAuditSettings = ModuleConfigurationHelper.Instance.GetCertificationSettings(Server.MapPath(ParentModulePath))
            ReCertificationAuditSettings = ModuleConfigurationHelper.Instance.GetRecertificationSettings(Server.MapPath(ParentModulePath))
        End Sub

        Private Sub LoadNotificationServices()
            PersonifyNotificationServices = _amcCertRecertController.GetNotifications(OrganizationId, OrganizationUnitId)
            BindNotificationEventForAuditProcess()
        End Sub

        Private Sub LoadFormConfigurations()
            Dim configurations = ModuleConfigurationHelper.Instance.GetFormConfigurations(ModuleId, Server.MapPath(ParentModulePath), OrganizationId, OrganizationUnitId)
            rptFormComfiguration.DataSource = configurations
            rptFormComfiguration.DataBind()
        End Sub

        ''' <summary>
        ''' Initializes the components.
        ''' </summary>
        Private Sub InitializeComponents()
            If Request.QueryString("buildquestions") = "y" Then
                btnBuildQuestion.Visible = True
            Else
                btnBuildQuestion.Visible = False
            End If

            CType(teMessage, TextEditor).DefaultMode = "RICH"
            CType(teMessage, TextEditor).Mode = "RICH"
            CType(teMessage, TextEditor).ChooseMode = False
            hlGoToQuestionList.NavigateUrl = NavigateToQuestionConfigurationPage(String.Empty, String.Empty, String.Empty)
            hlCETypeSettings.NavigateUrl = NavigateURL(TabId, "", String.Format("uc={0}", CommonConstants.CONFIGURATION_CETYPESETTINGS_PATH))
            hlValidationRuleSettings.NavigateUrl = NavigateURL(TabId, "", String.Format("uc={0}", CommonConstants.CONFIGURATION_VALIDATION_RULE_SETTINGS_PATH))
        End Sub

        ''' <summary>
        ''' Attatches the event handlers.
        ''' </summary>
        Private Sub AttatchEventHandlers()
            AddHandler rptFormComfiguration.ItemDataBound, AddressOf FormConfigurationItemDataBound
            AddHandler btnReset.Click, AddressOf BtnResetClick
            AddHandler btnSave.Click, AddressOf BtnSaveClick
            AddHandler btnOK.Click, AddressOf BtnOKClick
            AddHandler btnBuildQuestion.Click, AddressOf BtnBuildQuestionClick
            AddHandler btnCertSaveSettings.Click, AddressOf SaveCertificateAuditSettings
            AddHandler btnReCertSaveSettings.Click, AddressOf SaveReCertificateAuditSettings
            AddHandler btnClose.Click, AddressOf BtnCloseClick
            AddHandler btnExport.Click, AddressOf BtnExportClick
            AddHandler btnImport.Click, AddressOf BtnImportClick
            AddHandler cusvldImport.ServerValidate, AddressOf CusVldImportValidate
        End Sub

        Private Sub CusVldImportValidate(ByVal source As Object, ByVal args As ServerValidateEventArgs)
            args.IsValid = False
            Try
                If fuImport.PostedFile IsNot Nothing AndAlso fuImport.PostedFile.InputStream.Length >= 5 Then
                    Dim buf As Byte() = New Byte(4) {}
                    fuImport.PostedFile.InputStream.Read(buf, 0, 4)
                    If buf(0) = &H50 AndAlso buf(1) = &H4B AndAlso buf(2) = &H3 AndAlso buf(3) = &H4 Then
                        args.IsValid = True
                        fuImport.PostedFile.InputStream.Position = 0
                    Else
                        fuImport.PostedFile.InputStream.Close()
                    End If

                End If
            Catch ex As Exception
                Throw
            End Try
        End Sub

        Protected Sub LbtnRunReviewClick(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnRunReview.Click
            Try
                Dim certificationApplicationsForReview As CertificationCustomerCertifications
                If ddlProcessType.SelectedValue.Equals(CertificationTypeEnum.CERTIFICATION.ToString()) Then
                    certificationApplicationsForReview = _amcCertRecertController.GetCertificationApplicationsForReview(DateTime.Parse(txtReviewStartDate.Text), DateTime.Parse(txtReviewEndDate.Text), ddlProcessType.SelectedValue, OtherModuleSettings.CertificationCode)
                Else
                    certificationApplicationsForReview = _amcCertRecertController.GetCertificationApplicationsForReview(DateTime.Parse(txtReviewStartDate.Text), DateTime.Parse(txtReviewEndDate.Text), ddlProcessType.SelectedValue, OtherModuleSettings.RecertificationCode)
                End If
                rptReviewProcess.DataSource = certificationApplicationsForReview
                rptReviewProcess.DataBind()
                IsShowReviewProcessPopup = True
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Protected Sub RptReviewProcessItemDataBound(ByVal sender As Object, ByVal e As Web.UI.WebControls.RepeaterItemEventArgs) Handles rptReviewProcess.ItemDataBound
            Try
                If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
                    Dim certificationApplication = CType(e.Item.DataItem, CertificationCustomerCertification)
                    Dim lblCustomerName = CType(e.Item.FindControl("lblCustomerName"), Label)
                    Dim lblApplicationDate = CType(e.Item.FindControl("lblApplicationDate"), Label)
                    Dim hlPrintApplication = CType(e.Item.FindControl("hlPrintApplication"), HyperLink)
                    Dim printApplicationLink As String

                    lblCustomerName.Text = certificationApplication.CustomerInfo.LabelName
                    lblApplicationDate.Text = certificationApplication.ApplicationDate.ToString(CommonConstants.DATE_FORMAT)

                    If certificationApplication.CertificationTypeCodeString = CertificationTypeEnum.CERTIFICATION.ToString() Then
                        printApplicationLink = String.Format("{0}={1}", CommonConstants.USER_CONTROL_PARAMETER, Server.UrlEncode(CommonConstants.PROCESS_PRINT_CERT_FORM_PATH))
                    Else
                        printApplicationLink = String.Format("{0}={1}", CommonConstants.USER_CONTROL_PARAMETER, Server.UrlEncode(CommonConstants.PROCESS_PRINT_RECERT_FORM_PATH))
                    End If
                    printApplicationLink = DotNetNuke.Common.Globals.NavigateURL(TabId, "", {printApplicationLink, String.Format("{0}={1}", CommonConstants.MASTER_CUSTOMER_ID, certificationApplication.MasterCustomerId), String.Format("{0}={1}", CommonConstants.SUB_CUSTOMER_ID, certificationApplication.SubCustomerId), String.Format("{0}={1}", CommonConstants.CERTIFICATION_ID_QUERY_STRING, certificationApplication.CertificationId)})
                    hlPrintApplication.Attributes.Remove("onclick")
                    hlPrintApplication.Attributes.Add("onclick", String.Format("OpenPrintPage('{0}')", printApplicationLink))
                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Private Sub BtnCloseClick(ByVal sender As Object, ByVal e As EventArgs)
            Response.Redirect(NavigateURL(TabId, "", String.Format("uc={0}", CommonConstants.USER_CONTROL_DEFAULT_PATH)))
        End Sub

        Private Sub SaveAuditProcessLastRunningDate(ByVal auditProcessRunningType As AuditProcessRunningType)
            If (auditProcessRunningType = auditProcessRunningType.CertificationAudit) Then
                txtCertLastAuditRun.Text = DateTime.Now.ToString(CommonConstants.DATE_FORMAT)
                ModuleConfigurationHelper.Instance.SaveSettings(Server.MapPath(ParentModulePath), CommonConstants.CERT_AUDIT_SETTINGS_FILE_PATH, CertificationAuditSettings)
            ElseIf (auditProcessRunningType = auditProcessRunningType.RecertificationAudit) Then
                txtReCertLastAuditRun.Text = DateTime.Now.ToString(CommonConstants.DATE_FORMAT)
                ModuleConfigurationHelper.Instance.SaveSettings(Server.MapPath(ParentModulePath), CommonConstants.RECERT_AUDIT_SETTINGS_FILE_PATH, ReCertificationAuditSettings)
            End If
        End Sub

        Private Sub InitAuditRunningDate(ByVal auditProcessRunningType As AuditProcessRunningType)
            If (auditProcessRunningType = auditProcessRunningType.LastCertificationAudit And Not String.IsNullOrEmpty(txtCertLastAuditRun.Text)) Then
                Dim certStartDate As AMCDatetime = txtCertAuditStartDate
                Dim certEndDate As AMCDatetime = txtCertAuditEndDate
                certStartDate.Text = txtCertLastAuditRun.Text
                certEndDate.Text = DateTime.Now.ToString(CommonConstants.DATE_FORMAT)
            End If

            If (auditProcessRunningType = auditProcessRunningType.LastRecertificationAudit And Not String.IsNullOrEmpty(txtReCertLastAuditRun.Text)) Then
                Dim reCertStartDate As AMCDatetime = txtReCertAuditStartDate
                Dim reCertEndDate As AMCDatetime = txtReCertAuditEndDate
                reCertStartDate.Text = txtReCertLastAuditRun.Text
                reCertEndDate.Text = DateTime.Now.ToString(CommonConstants.DATE_FORMAT)
            End If
        End Sub

        Private Sub BuildAuditPopupTitle(ByVal totalApp As Int32, ByVal selectedApp As Int32)
            lblSelectedApplicationForAuditPopupTitle.Text = DotNetNuke.Services.Localization.Localization.GetString("SelectedApplicationForAuditPopupTitle.Text", LocalResourceFile)
            If (Not String.IsNullOrEmpty(lblSelectedApplicationForAuditPopupTitle.Text)) Then
                If (lblSelectedApplicationForAuditPopupTitle.Text.Contains("{totalApplication}")) Then
                    lblSelectedApplicationForAuditPopupTitle.Text = lblSelectedApplicationForAuditPopupTitle.Text.Replace("{totalApplication}", totalApp.ToString())
                End If
                If (lblSelectedApplicationForAuditPopupTitle.Text.Contains("{selectedApplication}")) Then
                    lblSelectedApplicationForAuditPopupTitle.Text = lblSelectedApplicationForAuditPopupTitle.Text.Replace("{selectedApplication}", selectedApp.ToString())
                End If
            End If
        End Sub

        Private Sub StartRunningAudit(ByVal startDate As String, ByVal endDate As String, ByVal ratio As String, ByVal notifyApplicant As Boolean, ByVal certificationType As CertificationTypeEnum, ByVal certRecertCode As String)
            Dim totalApplication As Int32 = 0
            Dim selectedApplicationList As List(Of CertificationCustomerCertification) = _amcCertRecertController.SeclectApplicationRandomlyForAuditing(DateTime.Parse(startDate), DateTime.Parse(endDate), ratio, notifyApplicant, certificationType.ToString(), certRecertCode, totalApplication)
            If (selectedApplicationList IsNot Nothing And selectedApplicationList.Count > 0) Then
                grvSelectedApplicationForAudit.DataSource = selectedApplicationList
                grvSelectedApplicationForAudit.DataBind()
            Else
                grvSelectedApplicationForAudit.DataSource = Nothing
                grvSelectedApplicationForAudit.DataBind()
            End If
            BuildAuditPopupTitle(totalApplication, selectedApplicationList.Count)

            UpdateNotificationEventByStoreProcedureName(notifyApplicant, certificationType.ToString())

            ScriptManager.RegisterArrayDeclaration(Page, "runCertAudit", String.Concat("'", "1", "'"))
        End Sub

        Private Sub UpdateNotificationEventByStoreProcedureName(ByVal notifyApplicant As Boolean, ByVal typeCode As String)
            ' Update notificaiton event for Certification Audit Section
            If (typeCode = CertificationTypeEnum.CERTIFICATION.ToString()) Then
                Dim notification As INotificationserviceNotifications = _amcCertRecertController.GetNotificationByStoreProcedureName(OrganizationId, OrganizationUnitId, CommonConstants.CERT_SELECTED_AUDIT_PROCEDURE)
                If (notification IsNot Nothing And notification.Count = 1) Then
                    Dim notificationInfos As New List(Of NotificationInfo)()
                    Dim notificationInfo As New NotificationInfo()
                    notificationInfo.IsEnabled = notifyApplicant
                    notificationInfo.NotificationId = notification(0).NotificationId.ToString()
                    notificationInfo.NotificationText = notification(0).NotificationName
                    notificationInfos.Add(notificationInfo)
                    _amcCertRecertController.UpdateNotificationByStorecedureName(OrganizationId, OrganizationUnitId, notificationInfos, CommonConstants.CERT_SELECTED_AUDIT_PROCEDURE)
                End If
            ElseIf (typeCode = CertificationTypeEnum.RECERTIFICATION.ToString()) Then
                Dim notification As INotificationserviceNotifications = _amcCertRecertController.GetNotificationByStoreProcedureName(OrganizationId, OrganizationUnitId, CommonConstants.RECERT_SELECTED_AUDIT_PROCEDURE)
                If (notification IsNot Nothing And notification.Count = 1) Then
                    Dim notificationInfos As New List(Of NotificationInfo)()
                    Dim notificationInfo As New NotificationInfo()
                    notificationInfo.IsEnabled = notifyApplicant
                    notificationInfo.NotificationId = notification(0).NotificationId.ToString()
                    notificationInfo.NotificationText = notification(0).NotificationName
                    notificationInfos.Add(notificationInfo)
                    _amcCertRecertController.UpdateNotificationByStorecedureName(OrganizationId, OrganizationUnitId, notificationInfos, CommonConstants.RECERT_SELECTED_AUDIT_PROCEDURE)
                End If
            End If
        End Sub

        Private Sub BindNotificationEventForAuditProcess()
            Dim notification As INotificationserviceNotifications
            notification = _amcCertRecertController.GetNotificationByStoreProcedureName(OrganizationId, OrganizationUnitId, CommonConstants.CERT_SELECTED_AUDIT_PROCEDURE)
            If (notification IsNot Nothing And notification.Count = 1) Then
                chkCertEnableNotifyApplicant.Checked = notification(0).ActiveFlag
            End If
            notification = _amcCertRecertController.GetNotificationByStoreProcedureName(OrganizationId, OrganizationUnitId, CommonConstants.RECERT_SELECTED_AUDIT_PROCEDURE)
            If (notification IsNot Nothing And notification.Count = 1) Then
                chkReCertEnableNotifyApplicant.Checked = notification(0).ActiveFlag
            End If
        End Sub

        ''' <summary>
        ''' Process unhanlde exception on page by logging error and showing a message
        ''' </summary>
        ''' <param name="unhandleException"></param>
        ''' <remarks></remarks>
        Private Sub ProcessException(ByVal unhandleException As Exception)
            _exceptionLogController.AddLog(unhandleException)
            If Not ShowErrorMessage Is Nothing Then
                ShowErrorMessage.Invoke(unhandleException.Message)
            End If
        End Sub

        ''' <summary>
        ''' BTNs the import click.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Private Sub BtnImportClick(ByVal sender As Object, ByVal e As EventArgs)
            _amcCertRecertController = New AmcCertRecertController(OrganizationId, OrganizationUnitId, 0, Server.MapPath(ParentModulePath), MasterCustomerId, SubCustomerId)
            If Page.IsValid Then
                Dim zipInStream As ZipInputStream = Nothing
                Dim fstream As FileStream = Nothing
                Dim streamReader As StreamReader = Nothing
                Dim memoryStream As MemoryStream = Nothing
                Try
                    Dim configurationDirectory = Path.Combine(Server.MapPath(ParentModulePath), CommonConstants.CONFIGURATIONS_DIRECTORY)
                    If fuImport.PostedFile IsNot Nothing AndAlso fuImport.PostedFile.InputStream.Length > 0 Then
                        zipInStream = New ZipInputStream(fuImport.PostedFile.InputStream)
                        Dim entry = zipInStream.GetNextEntry()
                        While entry IsNot Nothing
                            Dim fileName = Path.GetFileName(entry.Name)
                            memoryStream = New MemoryStream()
                            memoryStream.Position = 0
                            Dim buffer = New Byte(4096) {}
                            Dim count = zipInStream.Read(buffer, 0, buffer.Length)
                            While count > 0
                                memoryStream.Write(buffer, 0, count)
                                count = zipInStream.Read(buffer, 0, buffer.Length)
                            End While
                            memoryStream.Position = 0
                            streamReader = New StreamReader(memoryStream)
                            streamReader.BaseStream.Position = 0
                            Dim str = streamReader.ReadToEnd()
                            MergeConfigurations(fileName, str, configurationDirectory)
                            entry = zipInStream.GetNextEntry()
                        End While
                    End If
                Catch ex As Exception
                    ProcessException(ex)
                Finally
                    If zipInStream IsNot Nothing Then
                        zipInStream.Close()
                    End If
                    If fstream IsNot Nothing Then
                        fstream.Close()
                    End If
                    If fuImport.PostedFile IsNot Nothing Then
                        fuImport.PostedFile.InputStream.Close()
                    End If
                    If streamReader IsNot Nothing Then
                        streamReader.Close()
                    End If
                    If memoryStream IsNot Nothing Then
                        memoryStream.Close()
                    End If
                End Try

            End If
            '' to be added by DuyTruong : refresh page
            Dim navigateUrlString = String.Empty
            navigateUrlString = NavigateURL(TabId, "", String.Format("uc={0}", CommonConstants.CONFIGURATION_FORM_PATH))
            Response.Redirect(navigateUrlString, True)
        End Sub

        Private Sub MergeConfigurations(ByVal fileName As String, ByVal str As String, ByVal configurationDirectory As String)

            Select Case fileName
                Case CommonConstants.FORM_CONFIGURATIONS_FILENAME
                    ModuleConfigurationHelper.Instance.MergeConfigurations(Of List(Of FormInfo))(str, Path.Combine(configurationDirectory, fileName))
                    Exit Select
                Case CommonConstants.CERTIFICATION_CODE_SETTINGS_FILENAME
                    ModuleConfigurationHelper.Instance.MergeSingleConfigurations(Of CertificationCode)(str, Path.Combine(configurationDirectory, fileName))
                    Exit Select
                Case CommonConstants.CERT_AUDIT_SETTINGS_FILENAME
                    ModuleConfigurationHelper.Instance.MergeSingleConfigurations(Of CertificationAuditSetting)(str, Path.Combine(configurationDirectory, fileName))
                    Exit Select
                Case CommonConstants.OTHER_MODULE_SETTINGS_FILENAME
                    ModuleConfigurationHelper.Instance.MergeSingleConfigurations(Of OtherModuleSettings)(str, Path.Combine(configurationDirectory, fileName))
                    Exit Select
                Case CommonConstants.PROGRAMTYPE_OPT2_SETTINGS_FILENAME
                    ModuleConfigurationHelper.Instance.MergeFile(Of List(Of ProgramTypeSettings))(Str, Path.Combine(configurationDirectory, fileName))
                    Exit Select
                Case CommonConstants.RECERT_AUDIT_SETTINGS_FILENAME
                    ModuleConfigurationHelper.Instance.MergeSingleConfigurations(Of ReCertificationAuditSetting)(str, Path.Combine(configurationDirectory, fileName))
                    Exit Select
                Case CommonConstants.VALIDATION_RULE_SETTINGS_FILENAME
                    ModuleConfigurationHelper.Instance.MergeFile(Of List(Of ValidationRuleSetting))(Str, Path.Combine(configurationDirectory, fileName))
                    Exit Select
            End Select
        End Sub

        '
        ''' <summary>
        ''' BTNs the export click.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Private Sub BtnExportClick(ByVal sender As Object, ByVal e As EventArgs)
            AddFilesToZip("AMC_Configurations",
                          Path.Combine(Server.MapPath(ParentModulePath), CommonConstants.FORM_CONFIGURATIONS_FILE_PATH),
                          Path.Combine(Server.MapPath(ParentModulePath), CommonConstants.CERTIFICATION_CODE_FILE_PATH),
                          Path.Combine(Server.MapPath(ParentModulePath), CommonConstants.CERT_AUDIT_SETTINGS_FILE_PATH),
                          Path.Combine(Server.MapPath(ParentModulePath), CommonConstants.NOTIFICATION_SETTINGS_FILE_PATH),
                          Path.Combine(Server.MapPath(ParentModulePath), CommonConstants.OTHER_MODULE_SETTING_FILE_PATH),
                          Path.Combine(Server.MapPath(ParentModulePath), CommonConstants.PROGRAMTYPE_RECERT_OPTION2_SETTING_FILE_PATH),
                          Path.Combine(Server.MapPath(ParentModulePath), CommonConstants.RECERT_AUDIT_SETTINGS_FILE_PATH),
                          Path.Combine(Server.MapPath(ParentModulePath), CommonConstants.CONFIGURATION_VALIDATION_RULE_SETTINGS_FILE_PATH))
        End Sub

        ''' <summary>
        ''' Adds the files to zip.
        ''' </summary>
        ''' <param name="output">The output.</param>
        ''' <param name="inputs">The inputs.</param>
        Private Sub AddFilesToZip(ByVal output As String, ByVal ParamArray inputs() As String)
            Using mStream = New MemoryStream
                Using zipOutput = New ZipOutputStream(mStream)
                    zipOutput.SetLevel(9)
                    If inputs IsNot Nothing AndAlso inputs.Length > 0 Then
                        Dim buffer = New Byte(4096) {}
                        For Each fileName As String In inputs
                            Using fStream = File.OpenRead(fileName) 'New FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read)

                                Dim entry = New ZipEntry(Path.GetFileName(fileName))
                                entry.Size = fStream.Length
                                zipOutput.PutNextEntry(entry)

                                Dim count = fStream.Read(buffer, 0, buffer.Length)
                                While count > 0
                                    zipOutput.Write(buffer, 0, count)
                                    count = fStream.Read(buffer, 0, buffer.Length)
                                End While
                                fStream.Close()
                            End Using
                        Next
                    End If
                    zipOutput.Finish()
                    mStream.WriteTo(Response.OutputStream)
                    Response.BufferOutput = False
                    Dim archiveName As String = [String].Format("{0}-{1}.zip", output, DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"))
                    Response.ContentType = "application/zip"
                    Response.AddHeader("Content-Type", "application/zip")
                    Response.AddHeader("content-disposition", "inline;filename=""" & archiveName & """")
                    Response.End()
                    zipOutput.Close()
                End Using
            End Using
        End Sub

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Navigates to question configuration page.
        ''' </summary>
        ''' <param name="formId">The form id.</param>
        ''' <param name="sectionId">The section id.</param>
        ''' <param name="fieldId">The field id.</param>
        ''' <returns></returns>
        Public Function NavigateToQuestionConfigurationPage(ByVal formId As String, ByVal sectionId As String, ByVal fieldId As String) As String
            If String.IsNullOrEmpty(formId) OrElse String.IsNullOrEmpty(sectionId) OrElse String.IsNullOrEmpty(fieldId) Then
                Return NavigateURL(TabId, "", String.Format("uc={0}", CommonConstants.CONFIGURATION_QUESTIONLIST_PATH))
            Else
                Return NavigateURL(TabId, "", String.Format("{0}={1}", "formId", formId), String.Format("{0}={1}", "sectionId", sectionId), String.Format("{0}={1}", "fieldId", fieldId), String.Format("uc={0}", CommonConstants.CONFIGURATION_QUESTIONLIST_PATH))
            End If
        End Function

#End Region

    End Class
End Namespace