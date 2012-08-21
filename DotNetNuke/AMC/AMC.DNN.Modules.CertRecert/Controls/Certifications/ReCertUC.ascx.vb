Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Business.Controller
Imports AMC.DNN.Modules.CertRecert.Business.Entities

Imports System.Threading
Imports System.Linq
Imports AMC.DNN.Modules.CertRecert.Data.Enums
Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports AMC.DNN.Modules.CertRecert.Controls.Common
Imports AMC.DNN.Modules.CertRecert.Business.IControls
Imports System.Web.Script.Serialization
Imports DotNetNuke.Services.Log.EventLog

Imports Telerik.Web.UI
Imports TIMSS.API.CertificationInfo
Imports TIMSS.API.Core.Validation
Imports TIMSS.API.User.UserDefinedInfo

Namespace Controls.Certifications
    ''' <summary>
    ''' Prepresent GUI for Re-Cert application 
    ''' </summary>
    Public Class ReCertUC
        Inherits BaseUserControl

#Region "Properties"
        Public ReadOnly Property DateTimeFormat As String
            Get
                Return CommonConstants.DATE_FORMAT
            End Get
        End Property

        Public ReadOnly Property RecertOpt0BypassPayment() As Boolean
            Get
                Return False
                'Return ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(ValidationRuleId.RECERT_OPTION0_BYPASS_PAYMENT.ToString(), Server.MapPath(ParentModulePath))
            End Get
        End Property

        Public Shared currentFormInfoShare As FormInfo
        ''' <summary>
        ''' Gets print URL for Re-Cert application
        ''' </summary>
        Public ReadOnly Property PrintURL() As String
            Get
                Dim url As String = DotNetNuke.Common.Globals.NavigateURL()
                url = String.Format("{0}?{1}={2}", url, CommonConstants.USER_CONTROL_PARAMETER, Server.UrlEncode(CommonConstants.PROCESS_PRINT_RECERT_FORM_PATH))
                Return url
            End Get
        End Property

        Public ReadOnly Property RecertificationCircleJson() As String
            Get
                If RecertificationCircle IsNot Nothing Then
                    Return (New JavaScriptSerializer()).Serialize(RecertificationCircle)
                End If
                Return String.Empty
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
                btnPrint.Enabled = False
                btnSubmit.Enabled = False
                If UserInfo.IsSuperUser Then
                    Response.Redirect(NavigateURL(TabId, "", String.Format("uc={0}", CommonConstants.USER_CONTROL_DEFAULT_PATH)), True)
                    Return
                End If
                If Not Page.IsPostBack Then
                    CheckCurrentReCertOption()
                End If

                CheckForReCertification()
                AMCCertRecertController = New AmcCertRecertController(OrganizationId, OrganizationUnitId, CertificationId, Server.MapPath(ParentModulePath), MasterCustomerId, SubCustomerId)
                InitializeComponents()
                AddEventHandlers()
                If Not Page.IsPostBack AndAlso mvCertification.Views IsNot Nothing AndAlso mvCertification.Views.Count > 0 Then
                    LoadMenuStep()
                    CheckForIncompleteTabs()
                End If

            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Private Sub CheckCurrentReCertOption()
            If CurrentFormInfo IsNot Nothing AndAlso CurrentFormInfo.Sections IsNot Nothing Then
                For Each section As SectionInfo In CurrentFormInfo.Sections
                    If section.SectionId = "RecertificationOptionUC" AndAlso section.IsEnabled Then
                        Dim survey = AMCCertRecertController.GetSurveyByTitle(DataAccessConstants.RECERTIFICATION_OPTION_SERVEY_TITLE,
                                                                                      OrganizationId, OrganizationUnitId)
                        If survey IsNot Nothing Then
                            Dim questions = CType(survey, UserDefinedSurvey).UserDefinedSurveyQuestions
                            Dim responses As IUserDefinedCustomerSurveyResponsees = CType(survey, UserDefinedSurvey).UserDefinedCustomerSurveyResponsees
                            If responses IsNot Nothing AndAlso responses.Count > 0 AndAlso questions IsNot Nothing AndAlso questions.Count > 0 Then
                                For Each userdefinedSurveyResponse As UserDefinedCustomerSurveyResponses In responses
                                    If userdefinedSurveyResponse.MasterCustomerId = MasterCustomerId Then
                                        For Each userDefinedSurveyQuestion As UserDefinedSurveyQuestion In questions
                                            If userdefinedSurveyResponse.QuestionId = userDefinedSurveyQuestion.QuestionId _
                                                AndAlso userDefinedSurveyQuestion.QuestionCode <> Enums.QuestionCode.RECERT_OPTION_IF_PASS_EXAM.ToString() _
                                                AndAlso userDefinedSurveyQuestion.QuestionCode <> Enums.QuestionCode.RECERT_OPTION_INCLUDE_MYNAME_ONLIST.ToString() Then

                                                SetCurrentReCertOption(userDefinedSurveyQuestion.QuestionId.ToString())
                                                Exit For
                                            End If
                                        Next
                                    End If
                                Next
                            End If
                        End If
                        Exit For
                    End If
                Next
            End If
        End Sub

        Protected Sub Page_PreRender(ByVal s As Object, ByVal e As EventArgs) Handles Me.PreRender
            Try
                If Not Page.IsPostBack AndAlso mvCertification.Views IsNot Nothing AndAlso mvCertification.Views.Count > 0 Then
                    StepMenuTabClick(Nothing, Nothing)
                End If
                PaymentProcessed = False
                Dim currentRecertOption = GetCurrentReCertOption()
                'If hdAllTabCompleted.Value = CommonConstants.TAB_COMPLETED Then
                Dim productIds = If(currentRecertOption IsNot Nothing AndAlso currentRecertOption.Enabled, GetProductIds(currentRecertOption), Nothing)
                If productIds IsNot Nothing AndAlso productIds.Count > 0 AndAlso
                   AMCCertRecertController.WasPaymentProcessed(OrganizationId, OrganizationUnitId, MasterCustomerId, SubCustomerId, productIds(0)) Then
                    PaymentProcessed = True
                    ShowErrorMessage("Payment Already Processed.")
                End If
                'End If

            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Steps the menu tab click.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="Telerik.Web.UI.RadTabStripEventArgs" /> instance containing the event data.</param>
        Private Sub StepMenuTabClick(ByVal sender As Object, ByVal e As RadTabStripEventArgs)
            btnPrint.Enabled = False
            btnSubmit.Enabled = False
            Dim firstEnabledTab As Integer = GetFirstEnabledTab()
            Dim lastEnabledTab As Integer = GetLastEnabledTab()

            Dim activeView As View
            Dim currentTab As RadTab
            If e IsNot Nothing Then
                activeView = GetActiveView(e.Tab.Value)
                currentTab = e.Tab
            Else 'Load the first Enabled Tab
                If firstEnabledTab = 0 Then
                    activeView = GetActiveView(rtsStepMenu.SelectedTab.Value)
                    currentTab = rtsStepMenu.SelectedTab
                Else
                    activeView = GetActiveView(rtsStepMenu.Tabs(firstEnabledTab).Value)
                    currentTab = rtsStepMenu.Tabs(firstEnabledTab)
                End If
            End If

            If activeView IsNot Nothing Then
                mvCertification.SetActiveView(activeView)
            End If
            rtsStepMenu.SelectedIndex = currentTab.Index

            btnBack.Visible = rtsStepMenu.SelectedIndex > firstEnabledTab
            btnNext.Visible = rtsStepMenu.SelectedIndex < lastEnabledTab
            'If Page.IsPostBack Then
            CheckForIncompleteTabs()
            'End If
        End Sub

        Private Function GetLastEnabledTab() As Integer

            Dim lastEnabledTab = 0
            For i As Integer = rtsStepMenu.Tabs.Count - 1 To 0 Step -1
                Dim lastTab = rtsStepMenu.Tabs(i)
                If lastTab.Enabled Then
                    lastEnabledTab = lastTab.Index
                    Exit For
                End If
            Next
            Return lastEnabledTab
        End Function

        Private Function GetFirstEnabledTab() As Integer

            Dim firstEnabledTab = 0
            For i As Integer = 0 To rtsStepMenu.Tabs.Count - 1
                Dim lastTab = rtsStepMenu.Tabs(i)
                If lastTab.Enabled Then
                    firstEnabledTab = lastTab.Index
                    Exit For
                End If
            Next
            Return firstEnabledTab
        End Function

        ''' <summary>
        ''' BTNs the next click.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Private Sub BtnNextClick(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Dim results As IIssuesCollection
                results = Nothing
                Dim view = mvCertification.GetActiveView()
                For Each control As Control In view.Controls
                    If TypeOf control Is SectionBaseUserControl Then
                        results = CType(control, SectionBaseUserControl).Save()
                        If TypeOf control Is ISave Then 'reload if required
                            Reload(mvCertification, control.GetType().BaseType.Name)
                        End If
                        Exit For
                    End If
                Next
                Dim currentReCertOption = GetCurrentReCertOption()
                If currentReCertOption IsNot Nothing AndAlso currentReCertOption.Enabled AndAlso currentReCertOption.QuestionCode = Enums.QuestionCode.RECERT_OPTION_RETAKE.ToString() Then
                    For Each radTab As RadTab In rtsStepMenu.Tabs
                        If (RecertOpt0BypassPayment AndAlso radTab.Value = "RecertificationOptionUC") OrElse (Not RecertOpt0BypassPayment AndAlso (radTab.Value = "RecertificationOptionUC" OrElse radTab.Value = "SupervisorUC" OrElse radTab.Value = "RecertEligibilityUC" OrElse radTab.Value = "RegistrationUC")) Then
                            radTab.Enabled = True
                        Else
                            radTab.Enabled = False
                        End If
                    Next
                Else
                    For Each radTab As RadTab In rtsStepMenu.Tabs
                        radTab.Enabled = True
                    Next
                End If
                If results Is Nothing OrElse results.Count <= 0 Then
                    Dim selectedTab As RadTabStripEventArgs
                    hdCurrentSectionPopupOpenningId.Value = String.Empty
                    For i As Integer = 0 To rtsStepMenu.Tabs.Count - 1
                        Dim radTab = rtsStepMenu.Tabs(i)
                        If i > rtsStepMenu.SelectedIndex AndAlso radTab.Enabled Then
                            selectedTab = New RadTabStripEventArgs(radTab)
                            'rtsStepMenu.SelectedIndex = radTab.Index
                            Exit For
                        End If
                    Next
                    StepMenuTabClick(Nothing, selectedTab)
                Else
                    ShowError(results, lblMessage)
                End If
                CheckForIncompleteTabs()
            Catch ex As Exception
                Me.ProcessException(ex)
            End Try
        End Sub

        ''' <summary>
        ''' BTNs the back click.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Private Sub BtnBackClick(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Dim selectedTab As RadTabStripEventArgs
                For i As Integer = rtsStepMenu.Tabs.Count - 1 To 0 Step -1
                    Dim radTab = rtsStepMenu.Tabs(i)
                    If i < rtsStepMenu.SelectedIndex AndAlso radTab.Enabled Then
                        'rtsStepMenu.SelectedIndex = radTab.Index
                        selectedTab = New RadTabStripEventArgs(radTab)
                        Exit For
                    End If
                Next
                StepMenuTabClick(Nothing, selectedTab)
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Gets the active view.
        ''' </summary>
        ''' <param name="viewId">The view id.</param>
        ''' <returns></returns>
        Private Function GetActiveView(ByVal viewId As String) As View
            For Each view As View In mvCertification.Views
                If view.ID = viewId Then
                    Return view
                End If
            Next
            Return Nothing
        End Function


        ''' <summary>
        ''' BTNs the cancel click.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Private Sub BtnCancelClick(ByVal sender As Object, ByVal e As EventArgs)
            Response.Redirect(NavigateURL(TabId, "", String.Format("uc={0}", CommonConstants.USER_CONTROL_DEFAULT_PATH)))
        End Sub

        ''' <summary>
        ''' BTNs the submit click.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Private Sub BtnPaymentSubmitClick(ByVal sender As Object, ByVal e As EventArgs)
            Try
                'PaymentProcess()
                Dim origCertification As ICertificationCustomerCertification = AMCCertRecertController.GetOrigCertification(CurrentCertificationCustomerCertification.OrigCertificationId, MasterCustomerId, SubCustomerId)
                Dim otherModuleSettings = ModuleConfigurationHelper.Instance.GetOtherModuleSettings(Server.MapPath(ParentModulePath))
                If origCertification IsNot Nothing AndAlso otherModuleSettings IsNot Nothing Then
                    Dim paymentMonths = If(otherModuleSettings.RecertificationPaymentMonths.HasValue, otherModuleSettings.RecertificationPaymentMonths.Value, 12)
                    Dim validPaymentDate = New DateTime(origCertification.CertificationExpirationDate.Year, origCertification.CertificationExpirationDate.Month, origCertification.CertificationExpirationDate.Day)
                    validPaymentDate = validPaymentDate.AddMonths(-paymentMonths)
                    Dim now = New DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
                    If origCertification.CertificationExpirationDate.Year > 1 AndAlso origCertification.CertificationExpirationDate < now.AddMonths(paymentMonths) Then
                        PaymentProcess(GetProductIds(GetCurrentReCertOption()))
                    ElseIf origCertification.CertificationExpirationDate.Year <= 1 Then
                        ShowErrorMessage("The expiration date of your certification/recertification is invalid.")
                    Else
                        ShowErrorMessage(String.Format("You could only process payment from {0} to {1}", validPaymentDate.ToString("MM/dd/yyyy"), origCertification.CertificationExpirationDate.ToString("MM/dd/yyyy")))
                    End If
                Else
                    ShowErrorMessage("Can not find related certification.")
                End If
                CheckForIncompleteTabs()
            Catch ex1 As ThreadAbortException
                'nothing here
            Catch ex As System.Exception
                ProcessPageLoadException(ex)
            End Try

        End Sub

        Private Sub BtnPrintClick(ByVal sender As Object, ByVal e As EventArgs)

        End Sub


        ''' <summary>
        ''' BTNs the save click.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Private Sub BtnSaveClick(ByVal sender As Object, ByVal e As EventArgs)
            ' Response.Redirect(NavigateURL(TabId, "", String.Format("uc={0}", CommonConstants.PAYMENT_ORDER_PATH)))
        End Sub

        Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
            Try
                Dim results As IIssuesCollection
                Dim view = mvCertification.GetActiveView()
                For Each control As Control In view.Controls
                    If TypeOf control Is SectionBaseUserControl Then
                        results = CType(control, SectionBaseUserControl).Save()
                        If results IsNot Nothing AndAlso results.Count > 0 Then
                            ShowError(results, lblMessage)
                        Else
                            If TypeOf control Is ISave Then 'reload if required
                                Reload(mvCertification, control.GetType().BaseType.Name)
                            End If
                        End If
                        CheckForIncompleteTabs()
                        Return
                    End If
                Next
            Catch ex As Exception
                Me.ProcessException(ex)
            End Try
        End Sub

#End Region

#Region "Private Methods"

        Public Overrides Function ReferencAndVeryficationSurveyTitle() As String
            Return DataAccessConstants.RECERT_REFERENCE_VERIFICATION_SURVEY_TITLE
        End Function

        ''' <summary>
        ''' Checks for re certification.
        ''' </summary>
        Private Sub CheckForReCertification()
            ''Set hard code for certification id
            Dim applicantStatus As ApplicantStatusEnum
            If String.IsNullOrEmpty(Request.QueryString(CommonConstants.CERTIFICATION_ID_QUERY_STRING)) Then
                If Not UserInfo.IsSuperUser AndAlso IsPersonifyWebUserLoggedIn Then
                    Dim certicationCustomerCertication As ICertificationCustomerCertification = Nothing
                    applicantStatus =
                        AMCCertRecertController.GetApplicantStatus(AmcCertificationCode,
                                                                   MasterCustomerId,
                                                                   SubCustomerId,
                                                                   certicationCustomerCertication)
                    If applicantStatus = ApplicantStatusEnum.AllowApplyForRecertification AndAlso
                                                            certicationCustomerCertication IsNot Nothing Then
                        CurrentCertificationCustomerCertification = certicationCustomerCertication
                        CertificationId = CurrentCertificationCustomerCertification.CertificationId
                    Else
                        NavigateToDefaultControl()
                    End If
                End If
                If Me.CurrentCertificationCustomerCertification Is Nothing Then
                    NavigateToDefaultControl()
                    _logController.AddLog(New LogInfo("No recertification info"))
                    _logController.AddLog(New LogInfo(applicantStatus.ToString()))
                End If
            End If
        End Sub

        ''' <summary>
        ''' Loads the menu step.
        ''' </summary>
        Private Sub LoadMenuStep()
            rtsStepMenu.DataTextField = "SectionValue"
            rtsStepMenu.DataValueField = "SectionId"
            ''rtsStepMenu.DataSource = CurrentFormInfo.Sections.Where(Function(x) x.IsEnabled).OrderBy(Function(x) x.Sequence)
            rtsStepMenu.DataSource = CurrentFormInfo.Sections.Where(Function(x) x.IsEnabled).OrderBy(Function(x) x.Sequence)
            rtsStepMenu.DataBind()
        End Sub

        ''' <summary>
        ''' Initializes the components.
        ''' </summary>
        Private Sub InitializeComponents()
            lblMessage.Text = String.Empty
            StepCompletedList = String.Empty
            currentFormInfoShare = CurrentFormInfo
            RecertificationCircle = ModuleConfigurationHelper.Instance.GetRecertificationCyle(Server.MapPath(ParentModulePath), AMCCertRecertController, CurrentCertificationCustomerCertification)
            For Each view As View In mvCertification.Views
                Dim sectionControl = view.FindControl(String.Format("section{0}", view.ID))
                If sectionControl IsNot Nothing AndAlso TypeOf sectionControl Is SectionBaseUserControl Then
                    Dim tabControl As SectionBaseUserControl
                    tabControl = CType(sectionControl, SectionBaseUserControl)

                    tabControl.ModuleConfiguration = ModuleConfiguration
                    tabControl.LocalResourceFile = LocalResourceFile
                    tabControl.CertificationId = CertificationId
                    tabControl.CurrentCertificationCustomerCertification = Me.CurrentCertificationCustomerCertification
                    'tabControl.ParentModulePath = ParentModulePath
                    tabControl.CurrentFormInfo = CurrentFormInfo
                    CheckForIncompleteTab(sectionControl)
                    tabControl.ShowErrorMessage = AddressOf ShowErrorMessage
                    tabControl.CurrentReCertOptionAction = AddressOf SetCurrentReCertOption
                    tabControl.GetCurrentReCertOptionAction = AddressOf GetCurrentReCertOption
                    tabControl.AMCCertRecertController = AMCCertRecertController
                    tabControl.RecertificationCircle = RecertificationCircle
                    tabControl.ReferencAndVeryficationSurveyTitle = ReferencAndVeryficationSurveyTitle()
                    Dim summaryControl = TryCast(tabControl, Summary)
                    If (summaryControl IsNot Nothing) Then
                        summaryControl.ParentSaveButton = btnSave
                        summaryControl.ParentSaveButtonClick = AddressOf btnSave_Click
                    End If
                End If
            Next
        End Sub

        ''' <summary>
        ''' Checks for incomplete tabs.
        ''' </summary>
        Private Sub CheckForIncompleteTabs()
            Dim result = True
            Dim currentReCertOption = GetCurrentReCertOption()

            For Each view As View In mvCertification.Views
                Dim sectionControl = view.FindControl(String.Format("section{0}", view.ID))

                If sectionControl IsNot Nothing AndAlso TypeOf sectionControl Is SectionBaseUserControl Then
                    If TypeOf sectionControl Is Summary Then
                        CType(sectionControl, Summary).Save()
                    End If
                    Dim tempResult = True
                    If currentReCertOption Is Nothing OrElse Not currentReCertOption.Enabled OrElse _
                        (currentReCertOption IsNot Nothing AndAlso ((currentReCertOption.QuestionCode = Enums.QuestionCode.RECERT_OPTION_RETAKE.ToString() _
                                                                     AndAlso ((RecertOpt0BypassPayment AndAlso view.ID = "RecertificationOptionUC") _
                                                                              OrElse (Not RecertOpt0BypassPayment _
                                                                                      AndAlso (view.ID = "RecertificationOptionUC" OrElse view.ID = "SupervisorUC" _
                                                                                        OrElse view.ID = "RecertEligibilityUC" OrElse view.ID = "RegistrationUC")))) _
                        OrElse currentReCertOption.QuestionCode <> Enums.QuestionCode.RECERT_OPTION_RETAKE.ToString())) Then
                        tempResult = CheckForIncompleteTab(sectionControl)
                        Dim disabledTab = rtsStepMenu.FindTabByValue(view.ID)
                        If disabledTab IsNot Nothing Then
                            disabledTab.Enabled = True
                        End If
                    Else
                        Dim disabledTab = rtsStepMenu.FindTabByValue(view.ID)
                        If disabledTab IsNot Nothing Then
                            disabledTab.Enabled = False
                        End If
                    End If
                    If Not tempResult Then
                        result = False
                    End If
                End If
            Next
            If result Then
                btnPrint.Enabled = True
                'TODO: CHECK FOR VALID DATE OF CERT/RECERT
                btnSubmit.Enabled = True
                hdAllTabCompleted.Value = CommonConstants.TAB_COMPLETED
            Else
                hdAllTabCompleted.Value = CommonConstants.TAB_INCOMPLETED
            End If
        End Sub

        ''' <summary>
        ''' Checks for incomplete tab.
        ''' </summary>
        ''' <param name="sectionControl">The section control.</param>
        Private Function CheckForIncompleteTab(ByVal sectionControl As Control) As Boolean
            Dim result = False
            Dim isIncompletehd = sectionControl.FindControl("hdIsIncomplete")
            If isIncompletehd IsNot Nothing AndAlso TypeOf isIncompletehd Is HiddenField Then
                Dim currentTab = rtsStepMenu.FindTabByValue(sectionControl.ID.Replace("section", String.Empty))
                Dim incompleteMode = CType(isIncompletehd, HiddenField).Value
                If currentTab IsNot Nothing AndAlso Not String.IsNullOrEmpty(incompleteMode) Then
                    If String.IsNullOrEmpty(StepCompletedList) Then
                        StepCompletedList = String.Format("{0}|{1}", currentTab.Index, incompleteMode)
                    Else
                        StepCompletedList = String.Format("{0};{1}|{2}", StepCompletedList, currentTab.Index, incompleteMode)
                    End If
                End If
                If incompleteMode = CommonConstants.TAB_COMPLETED Then
                    result = True
                End If
            End If
            Return result
        End Function

        Public Overrides Function GetExamChoiceSurveyTitle() As String
            Return DataAccessConstants.RECERTIFICATION_OPTION_SERVEY_TITLE
        End Function

        Public Overrides Sub ShowErrorMessage(ByVal message As String)
            Me.lblMessage.Text = message
        End Sub

        Public Overrides Sub SetCurrentReCertOption(ByVal questionId As String)
            'hdCurrentReCertOptionCode.Value = questionId.ToString()
            Session("CurrentReCertOption") = questionId.ToString()
        End Sub

        Public Overrides Function GetCurrentReCertOption() As UserDefinedSurveyQuestion
            If CurrentFormInfo IsNot Nothing AndAlso CurrentFormInfo.Sections IsNot Nothing Then
                For Each section As SectionInfo In CurrentFormInfo.Sections
                    If section.SectionId = "RecertificationOptionUC" AndAlso section.IsEnabled Then
                        If Session("CurrentReCertOption") Is Nothing OrElse String.IsNullOrEmpty(Session("CurrentReCertOption").ToString()) Then
                            Return Nothing
                        Else
                            'Dim result As Enums.QuestionCode = CType([Enum].Parse(GetType(Enums.QuestionCode), hdCurrentReCertOptionCode.Value, True), Enums.QuestionCode)
                            'Return result
                            If section.Fields IsNot Nothing AndAlso section.Fields.Count > 0 Then
                                For Each fieldInfo As FieldInfo In section.Fields
                                    If fieldInfo.FieldId = "QuestionList" AndAlso fieldInfo.IsEnabled Then
                                        Dim currentOption = AMCCertRecertController.GetQuestionById(OrganizationId, OrganizationUnitId, Session("CurrentReCertOption").ToString())
                                        If currentOption.Enabled Then
                                            Return currentOption
                                        End If
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                    End If
                Next
            End If
            Session("CurrentReCertOption") = Nothing
            Return Nothing
        End Function

        ''' <summary>
        ''' Adds the event handlers.
        ''' </summary>
        Private Sub AddEventHandlers()
            AddHandler btnPrint.Click, AddressOf BtnPrintClick
            AddHandler btnSubmit.Click, AddressOf BtnPaymentSubmitClick
            AddHandler btnCancel.Click, AddressOf BtnCancelClick
            AddHandler btnBack.Click, AddressOf BtnBackClick
            AddHandler btnNext.Click, AddressOf BtnNextClick
            AddHandler btnSave.Click, AddressOf BtnSaveClick
            AddHandler rtsStepMenu.TabClick, AddressOf StepMenuTabClick
        End Sub

#End Region
    End Class
End Namespace