Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Business.Controller

Imports System.Linq
Imports System.Threading
Imports AMC.DNN.Modules.CertRecert.Data.Enums
Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports DotNetNuke.Services.Log.EventLog
Imports AMC.DNN.Modules.CertRecert.Business.IControls

Imports TIMSS.API.CertificationInfo
Imports Telerik.Web.UI
Imports TIMSS.API.Core.Validation
Imports TIMSS.API.User.UserDefinedInfo

Namespace Controls.Certifications
    ''' <summary>
    ''' Prepresent GUI for Cert application 
    ''' </summary>
    Public Class CertUC
        Inherits BaseUserControl

#Region "Properties"

        ''' <summary>
        ''' Gets print URL for Cert application
        ''' </summary>
        Public ReadOnly Property PrintURL() As String
            Get
                Dim url As String = DotNetNuke.Common.Globals.NavigateURL()
                url = String.Format("{0}?{1}={2}", url, CommonConstants.USER_CONTROL_PARAMETER, Server.UrlEncode(CommonConstants.PROCESS_PRINT_CERT_FORM_PATH))
                Return url
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
                CheckForCertifications()
                AMCCertRecertController = New AmcCertRecertController(OrganizationId, OrganizationUnitId, CertificationId, Server.MapPath(ParentModulePath), MasterCustomerId, SubCustomerId)
                InitializeComponents()
                AddEventHandlers()
                CheckForUserRoles()
                If Not Page.IsPostBack AndAlso mvCertification.Views IsNot Nothing AndAlso mvCertification.Views.Count > 0 Then
                    LoadMenuStep()
                    StepMenuTabClick(Nothing, Nothing)
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Protected Sub Page_PreRender(ByVal s As Object, ByVal e As EventArgs) Handles Me.PreRender
            Try
                'If Not Page.IsPostBack Then
                CheckForIncompleteTabs()
                'End If

                PaymentProcessed = False
                'If hdAllTabCompleted.Value = CommonConstants.TAB_COMPLETED Then
                Dim productIds = GetProductIds()
                If productIds IsNot Nothing AndAlso productIds.Count > 0 AndAlso
                   AMCCertRecertController.WasPaymentProcessed(OrganizationId, OrganizationUnitId, MasterCustomerId, SubCustomerId, productIds(0)) Then
                    PaymentProcessed = True
                    ShowErrorMessage("Payment Already Processed.")
                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

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
                If results Is Nothing OrElse results.Count <= 0 Then
                    hdCurrentSectionPopupOpenningId.Value = String.Empty
                    rtsStepMenu.SelectedIndex += 1
                    StepMenuTabClick(Nothing, Nothing)
                Else
                    ShowError(results, lblMessage)
                End If
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
                rtsStepMenu.SelectedIndex -= 1
                StepMenuTabClick(Nothing, Nothing)
            Catch ex As Exception
                Me.ProcessException(ex)
            End Try
        End Sub

        ''' <summary>
        ''' BTNs the cancel click.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Private Sub BtnCancelClick(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Response.Redirect(NavigateURL(TabId, "", String.Format("uc={0}", CommonConstants.USER_CONTROL_DEFAULT_PATH)))
            Catch ex1 As ThreadAbortException
            Catch ex As Exception
                Me.ProcessException(ex)
            End Try
        End Sub

        ''' <summary>
        ''' BTNs the submit click.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Private Sub BtnSubmitClick(ByVal sender As Object, ByVal e As EventArgs)
            Try
                PaymentProcess(GetProductIds())
            Catch ex1 As ThreadAbortException
                'nothing here
            Catch ex As System.Exception
                Me.ProcessException(ex)
            End Try
        End Sub

        ''' <summary>
        ''' BTNs the Save click.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Private Sub BtnSaveClick(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Dim results As IIssuesCollection
                Dim view = mvCertification.GetActiveView()
                For Each control As Control In view.Controls
                    If TypeOf control Is SectionBaseUserControl Then
                        results = CType(control, SectionBaseUserControl).Save()
                        If TypeOf control Is ISave Then 'reload if required
                            Reload(mvCertification, control.GetType().BaseType.Name)
                        End If
                        If results IsNot Nothing AndAlso results.Count > 0 Then
                            ShowError(results, lblMessage)
                        End If
                        Return
                    End If
                Next
            Catch ex As Exception
                Me.ProcessException(ex)
            End Try
        End Sub

#End Region

#Region "Implement MustInherit"

        Public Overrides Function GetExamChoiceSurveyTitle() As String
            Return DataAccessConstants.CERTIFICATION_EXAM_CHOICE_SURVEY_TITLE
        End Function

        ''' <summary>
        ''' Show error message on error region of page
        ''' </summary>
        Public Overrides Sub ShowErrorMessage(ByVal message As String)
            Me.lblMessage.Text = message
        End Sub

        Public Overrides Sub SetCurrentReCertOption(ByVal questionId As String)
            'Throw New NotImplementedException()
        End Sub

        Public Overrides Function GetCurrentReCertOption() As UserDefinedSurveyQuestion
            Return Nothing
        End Function

#End Region

#Region "Private Methods"

        Public Overrides Function GetProductIds(Optional ByVal currentRecertOption As UserDefinedSurveyQuestion = Nothing) As Integer()
            Dim examChoice = AMCCertRecertController.GetUserDefinedCertificationApplicationExamPeriod(
                                                                                        GetExamChoiceSurveyTitle(),
                                                                                        Me.MasterCustomerId,
                                                                                        Me.SubCustomerId)
            Dim ret As Integer() = Nothing
            'If examChoice Is Nothing Then
            '    ShowErrorMessage("There is no examination was choiced")
            If examChoice IsNot Nothing Then
                If examChoice.ExamProductId <= 0 And examChoice.ApplicationProductId <= 0 Then
                    ShowErrorMessage("ExamProductId and ApplicationProductId could not be null")
                Else
                    ret = New Integer() {CInt(examChoice.ExamProductId), CInt(examChoice.ApplicationProductId)}
                End If
            End If
            Return ret
        End Function

        ''' <summary>
        ''' Checks for user roles.
        ''' </summary>
        Private Sub CheckForUserRoles()
            If Not Page.IsPostBack Then
                If UserInfo.IsSuperUser Then
                    Response.Redirect(NavigateURL(TabId, "", String.Format("uc={0}", CommonConstants.USER_CONTROL_DEFAULT_PATH)), True)
                ElseIf IsPersonifyWebUserLoggedIn = True Then
                    mvCertification.ActiveViewIndex = 0
                Else
                    Dim navigateUrlString As String
                    navigateUrlString = NavigateURL(TabId, String.Empty, {
                                                                   String.Format("{0}={1}", _
                                                                    CommonConstants.USER_CONTROL_PARAMETER, _
                                                                    CommonConstants.USER_CONTROL_DEFAULT_PATH)
                                                               })
                    navigateUrlString = navigateUrlString.Replace("http://", String.Empty)
                    Dim urlEncode As String = HttpUtility.UrlEncode(navigateUrlString)
                    navigateUrlString = NavigateURL(PortalSettings.LoginTabId, String.Empty, "returnurl=" & urlEncode)
                    Response.Redirect(navigateUrlString, True)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Checks for certifications.
        ''' </summary>
        Private Sub CheckForCertifications()
            ''Check applicant status, create a certification record if needed
            Dim certicationCustomerCertication As ICertificationCustomerCertification = Nothing
            Dim applicantStatus As ApplicantStatusEnum
            If String.IsNullOrEmpty(Request.QueryString(CommonConstants.CERTIFICATION_ID_QUERY_STRING)) Then
                If Not UserInfo.IsSuperUser AndAlso IsPersonifyWebUserLoggedIn Then
                    applicantStatus = AMCCertRecertController.GetApplicantStatus(AmcCertificationCode, MasterCustomerId, SubCustomerId, certicationCustomerCertication)
                    If applicantStatus = ApplicantStatusEnum.CreateNewCertfication AndAlso certicationCustomerCertication Is Nothing Then
                        ''Create a new Certification for current user
                        Dim newCertification = (AMCCertRecertController.GetCertificationCustomerCertifications(
                                                                                AmcCertificationCode.CertificationCode,
                                                                                CertificationTypeEnum.CERTIFICATION.ToString(),
                                                                                MasterCustomerId,
                                                                                SubCustomerId)).CreateNew()
                        With newCertification
                            .IsNewObjectFlag = True
                            .MasterCustomerId = MasterCustomerId
                            .SubCustomerId = SubCustomerId
                            .CertificationCode = .CertificationCodeList(AmcCertificationCode.CertificationCode).ToCodeObject()
                        End With
                        Dim result =
                            AMCCertRecertController.InsertCertificationCustomerCertification(newCertification)
                        If (result.Count < 1) Then
                            CurrentCertificationCustomerCertification = newCertification
                            CertificationId = CurrentCertificationCustomerCertification.CertificationId
                        Else
                            NavigateToDefaultControl()
                        End If
                    Else
                        If applicantStatus = ApplicantStatusEnum.EditCertification AndAlso certicationCustomerCertication IsNot Nothing Then
                            ''set certification id
                            CurrentCertificationCustomerCertification = certicationCustomerCertication
                            CertificationId = CurrentCertificationCustomerCertification.CertificationId
                        Else
                            ''don't allow user continue on this
                            NavigateToDefaultControl()
                        End If
                    End If
                End If
                If Me.CurrentCertificationCustomerCertification Is Nothing Then
                    NavigateToDefaultControl()
                    _logController.AddLog(New LogInfo("No Certification info"))
                    _logController.AddLog(New LogInfo(applicantStatus.ToString()))
                End If
            End If

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
        ''' Loads the menu step.
        ''' </summary>
        Private Sub LoadMenuStep()
            rtsStepMenu.DataTextField = "SectionValue"
            rtsStepMenu.DataValueField = "SectionId"
            rtsStepMenu.DataSource = CurrentFormInfo.Sections.Where(Function(x) x.IsEnabled).OrderBy(Function(x) x.Sequence)
            rtsStepMenu.DataBind()
        End Sub

        ''' <summary>
        ''' Initializes the components.
        ''' </summary>
        Private Sub InitializeComponents()
            lblMessage.Text = String.Empty
            StepCompletedList = String.Empty
            For Each view As View In mvCertification.Views
                Dim sectionControl = view.FindControl(String.Format("section{0}", view.ID))
                If sectionControl IsNot Nothing AndAlso TypeOf sectionControl Is SectionBaseUserControl Then
                    Dim tabControl As SectionBaseUserControl = CType(sectionControl, SectionBaseUserControl)
                    tabControl.ModuleConfiguration = ModuleConfiguration
                    tabControl.LocalResourceFile = LocalResourceFile
                    tabControl.CertificationId = CertificationId
                    tabControl.CurrentCertificationCustomerCertification = Me.CurrentCertificationCustomerCertification
                    tabControl.CurrentFormInfo = CurrentFormInfo
                    tabControl.ShowErrorMessage = AddressOf Me.ShowErrorMessage
                    tabControl.AMCCertRecertController = AMCCertRecertController
                    tabControl.ReferencAndVeryficationSurveyTitle = ReferencAndVeryficationSurveyTitle()
                    CheckForIncompleteTab(sectionControl)
                End If
            Next
        End Sub

        ''' <summary>
        ''' Checks for incomplete tabs.
        ''' </summary>
        Private Sub CheckForIncompleteTabs()
            Dim result = True
            For Each view As View In mvCertification.Views
                Dim sectionControl = view.FindControl(String.Format("section{0}", view.ID))
                If sectionControl IsNot Nothing AndAlso TypeOf sectionControl Is SectionBaseUserControl Then
                    Dim tempResult = CheckForIncompleteTab(sectionControl)
                    If Not tempResult Then
                        result = False
                    End If
                End If
            Next
            If result Then
                btnPrint.Enabled = True
                btnSubmit.Enabled = True
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

        ''' <summary>
        ''' Adds the event handlers.
        ''' </summary>
        Private Sub AddEventHandlers()
            AddHandler btnSubmit.Click, AddressOf BtnSubmitClick
            AddHandler btnCancel.Click, AddressOf BtnCancelClick
            AddHandler btnBack.Click, AddressOf BtnBackClick
            AddHandler btnNext.Click, AddressOf BtnNextClick
            AddHandler btbSave.Click, AddressOf BtnSaveClick
            AddHandler rtsStepMenu.TabClick, AddressOf StepMenuTabClick
        End Sub

        ''' <summary>
        ''' Steps the menu tab click.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="Telerik.Web.UI.RadTabStripEventArgs" /> instance containing the event data.</param>
        Private Sub StepMenuTabClick(ByVal sender As Object, ByVal e As RadTabStripEventArgs)
            Try
                btnPrint.Enabled = False
                btnSubmit.Enabled = False

                Dim activeView As View
                Dim currentTab As RadTab
                If e IsNot Nothing Then
                    activeView = GetActiveView(e.Tab.Value)
                    currentTab = e.Tab
                Else
                    activeView = GetActiveView(rtsStepMenu.SelectedTab.Value)
                    currentTab = rtsStepMenu.SelectedTab
                End If

                If activeView IsNot Nothing Then
                    mvCertification.SetActiveView(activeView)
                    'If Page.IsPostBack Then
                    '    'Re-bind data for current ActiveView
                    '    Dim activeSection = activeView.FindControl(String.Format("section{0}", activeView.ID))
                    '    If activeSection IsNot Nothing AndAlso TypeOf activeSection Is SectionBaseUserControl Then
                    '        CType(activeSection, SectionBaseUserControl).ValidateFormFillCompleted()
                    '    End If
                    'End If
                End If
                rtsStepMenu.SelectedIndex = currentTab.Index
                btnBack.Visible = rtsStepMenu.SelectedIndex > 0
                btnNext.Visible = rtsStepMenu.SelectedIndex < rtsStepMenu.Tabs.Count - 1
                CheckForIncompleteTabs()

            Catch ex As Exception
                Me.ProcessException(ex)
            End Try
        End Sub

#End Region
    End Class
End Namespace