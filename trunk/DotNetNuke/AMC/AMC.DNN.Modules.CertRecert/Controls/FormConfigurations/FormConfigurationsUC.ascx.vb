Imports AMC.DNN.Modules.CertRecert.Business.Controller
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports AMC.DNN.Modules.CertRecert.Business.Entities
Imports System.Linq
Imports DotNetNuke.Services.Log.EventLog
Imports Personify.ApplicationManager
Imports System.Threading
Imports TIMSS.API.User.UserDefinedInfo
Imports DotNetNuke.UI.UserControls

Namespace Controls.FormConfigurations
    Public Class FormConfigurationsUC
        Inherits PersonifyDNNBaseForm

#Region "Properties"
        Public Property ParentModulePath() As String

        Public Property ShowErrorMessage As Action(Of String)

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
#End Region


#Region "Private Member"

        Private _amcCertRecertController As AmcCertRecertController
        Private ReadOnly _exceptionLogController As ExceptionLogController = New ExceptionLogController()
#End Region

#Region "Event Handlers"
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            _amcCertRecertController = New AmcCertRecertController(OrganizationId, OrganizationUnitId, 0, Server.MapPath(ParentModulePath), MasterCustomerId, SubCustomerId)
            Try
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
                'Dim objModules As New ModuleController
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
            Catch ex As Exception
                ProcessException(New Exception(Localization.GetString("ConcurrencyUsers.Text", LocalResourceFile)))
            End Try
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
        ''' BTNs the reset click.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Private Sub BtnResetClick(ByVal sender As Object, ByVal e As EventArgs)
            'Reset Survey
            Dim surveys = _amcCertRecertController.GetSurveys(OrganizationId, OrganizationUnitId, MasterCustomerId, False)
            _amcCertRecertController.DeleteAllSurveys(surveys)
        End Sub


        Private Sub BtnCloseClick(ByVal sender As Object, ByVal e As EventArgs)
            Response.Redirect(NavigateURL(TabId, "", String.Format("uc={0}", CommonConstants.USER_CONTROL_DEFAULT_PATH)))
        End Sub
#End Region

#Region "Private Methods"
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
        ''' Sections configuration item data bound.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterItemEventArgs" /> instance containing the event data.</param>
        Private Sub SectionConfigurationItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
            BuildSectionConfigurationItem(e)
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

        Private Sub CheckUserRole()
            If Not (UserInfo.IsSuperUser OrElse UserInfo.IsInRole("Administrators") OrElse UserInfo.IsInRole("CERTADMIN") OrElse UserInfo.IsInRole("Host")) Then
                Response.Redirect(NavigateURL(), True)
            End If
        End Sub

        Private Sub RegisterJavascript()
            Page.ClientScript.RegisterClientScriptInclude("configuration", String.Format("{0}/Documentation/scripts/configurations.js?v={1}", ParentModulePath, CommonConstants.CURRENT_VERSION))
        End Sub

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
            hlOtherSettings.NavigateUrl = NavigateURL(TabId, "", String.Format("uc={0}", CommonConstants.CONFIGURATION_FORM_PATH))
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
            AddHandler btnClose.Click, AddressOf BtnCloseClick
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

        ''' <summary>
        ''' Loads the module configurations.
        ''' </summary>
        Private Sub LoadModuleConfigurations()
            If Not Page.IsPostBack Then
                FixConfigurationsSequences()
            End If

            'Form Configurations
            LoadFormConfigurations()
        End Sub


        Private Sub LoadFormConfigurations()
            Dim configurations = ModuleConfigurationHelper.Instance.GetFormConfigurations(ModuleId, Server.MapPath(ParentModulePath), OrganizationId, OrganizationUnitId)
            rptFormComfiguration.DataSource = configurations
            rptFormComfiguration.DataBind()
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