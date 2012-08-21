Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Controls.Reusable
Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports AMC.DNN.Modules.CertRecert.Business.BusinessValidation
Imports AMC.DNN.Modules.CertRecert.Data.Enums
Imports AMC.DNN.Modules.CertRecert.Business.IControls
Imports TIMSS.API.Core
Imports TIMSS.API.User.UserDefinedInfo
Imports TIMSS.API.Core.Validation

Namespace Controls.Common
    Public Class ContinuingEducation
        Inherits SectionBaseUserControl
        Implements IReload

#Region "Private Member"
        Public Property approvedHour As Decimal

#End Region

#Region "Event Handlers"
        ''' <summary>
        ''' Handles the Load event of the Page control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            Try
                AddEventHandlers()
                ProgramType = ActivityProgramType.EDUCATION.ToString()
                CEWeightList = AMCCertRecertController.GetCertificationCEWeights(OrganizationId, OrganizationUnitId, ProgramType)
                If IsPostBack Then
                    Dim otherModuleSettings = ModuleConfigurationHelper.Instance.GetOtherModuleSettings(Server.MapPath(ParentModulePath))
                    If otherModuleSettings IsNot Nothing Then
                        MaxCEHoursARN = If(otherModuleSettings.ARNMaxSummaryPointOfContinuingEducation.HasValue, otherModuleSettings.ARNMaxSummaryPointOfContinuingEducation.Value, 0)
                    End If
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Protected Sub Pre_Render(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender

        End Sub

        Protected Sub BtnSaveClick(ByVal sender As Object, ByVal e As EventArgs)
            Try
                SaveCEActivity()
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Public Overrides Function Save() As IIssuesCollection
            ''Commit data
            Try
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                Dim iissueResult As New IssuesCollection
                Dim errorMessages = String.Empty
                Dim totalCE As Decimal = 0
                Dim totalCEMin As Decimal = 0
                Dim results = AMCCertRecertController.CommitCustomerExternalCEActivity(ProgramType, MasterCustomerId, SubCustomerId)
                If results Is Nothing OrElse results.Count <= 0 Then '' not issues when save data
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                End If
                BindingDataToList()
                totalCE = Decimal.Parse(GetCookie(ProgramType + MasterCustomerId))
                '' check validation
                If Not String.IsNullOrEmpty(ConflictMessage) Then '' validation rule is conflicted
                    iissueResult.Add(New CheckBoxMustSelectedIssue(New BusinessObject(), ConflictMessage))
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                    Return iissueResult
                End If
                '' optinal rule will be overwrite another rule BUT Except for the form “Continuing Education”
                If ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(ValidationRuleId.CONTINUING_EDUCATION_OPTIONAL.ToString(), Server.MapPath(ParentModulePath)) Then
                    If rptcontinuingEducation.Items.Count > 0 Then  '' althought check option rule but must be chek 2/3 rule because have data on Grid
                        If RuleID = ValidationRuleId.RECERT_CON_EDU_ARN.ToString() Then  ''ARN
                            If CheckBusinessValidationCommitData_ARN(errorMessages, totalCE,
                                                                     If(GetCookie("approvedHour") <> "0",
                                                                        Decimal.Parse(GetCookie("approvedHour")), approvedHour)) Then
                                iissueResult.Add(New CheckBoxMustSelectedIssue(New BusinessObject(), errorMessages))
                                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                                Return iissueResult
                            End If
                        End If
                    Else
                        hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                    End If
                Else
                    '' check validation
                    If RuleID = ValidationRuleId.RECERT_CON_EDU_COMMON_CALCULATOR.ToString() Then  '' ABNN
                        If CheckBusinessValidationCommitData_ABNN(errorMessages, totalCE, totalCEMin) Then
                            iissueResult.Add(New CheckBoxMustSelectedIssue(New BusinessObject(), errorMessages))
                            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                            Return iissueResult
                        End If
                    ElseIf RuleID = ValidationRuleId.RECERT_CON_EDU_ARN.ToString() Then  ''ARN
                        If CheckBusinessValidationCommitData_ARN(errorMessages, totalCE,
                                                                 If(GetCookie("approvedHour") <> "0",
                                                                    Decimal.Parse(GetCookie("approvedHour")), approvedHour)) Then
                            iissueResult.Add(New CheckBoxMustSelectedIssue(New BusinessObject(), errorMessages))
                            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                            Return iissueResult
                        End If
                    End If
                    ''end check validation
                    If rptcontinuingEducation.Items Is Nothing OrElse rptcontinuingEducation.Items.Count <= 0 Then
                        '' if check rule and set ceMin = 0 => pass basic rule
                        If RuleID = ValidationRuleId.RECERT_CON_EDU_COMMON_CALCULATOR.ToString() AndAlso totalCEMin = 0 Then
                            hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                        Else
                            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                        End If
                    End If
                End If
                Return results
            Catch ex As Exception
                ProcessException(ex)
            End Try
            Return Nothing
        End Function

        Protected Sub RptcontinuingEducationItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptcontinuingEducation.ItemDataBound
            Try
                If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
                    Dim userdine As UserDefinedCustomerCEActivity = CType(e.Item.DataItem, UserDefinedCustomerCEActivity)
                    If userdine IsNot Nothing Then
                        Dim point As String
                        Dim lblCEType As Label = CType(e.Item.FindControl("lblCEType"), Label)
                        Dim hdnCETypeValue As Label = CType(e.Item.FindControl("lblCEValueType"), Label)
                        Dim lblCE As Label = CType(e.Item.FindControl("lblSumaryPoint"), Label)
                        If lblCEType IsNot Nothing AndAlso hdnCETypeValue IsNot Nothing AndAlso lblCE IsNot Nothing Then
                            lblCEType.Text = userdine.CEType.List(userdine.CETypeString).Description
                            hdnCETypeValue.Text = userdine.CEType.List(userdine.CETypeString).Code
                            '' get FactorIndex base on Association
                            FactorIndex = "1"
                            If RuleID = ValidationRuleId.RECERT_CON_EDU_COMMON_CALCULATOR.ToString() Then '' base on weight of ceType
                                FactorIndex = CommonHelper.GetCEWeight(userdine.CEType.List(userdine.CETypeString).Code, CEWeightList, False,
                                                                       If(GetFieldInfo("Point") IsNot Nothing AndAlso GetFieldInfo("Point").CalculateFormula > 0,
                                                                          GetFieldInfo("Point").CalculateFormula.ToString(), "1"))
                            ElseIf RuleID = ValidationRuleId.RECERT_CON_EDU_ARN.ToString() Then  '' base on control panel
                                FactorIndex = CommonHelper.GetCEWeight(userdine.CEType.List(userdine.CETypeString).Code, CEWeightList, False,
                                                                       If(GetFieldInfo("Point") IsNot Nothing AndAlso GetFieldInfo("Point").CalculateFormula > 0,
                                                                          GetFieldInfo("Point").CalculateFormula.ToString(), "1"))
                            End If
                            '' calculator point
                            point = CommonHelper.CalculatorCEItem(userdine.CEHours.ToString(), FactorIndex).ToString()
                            lblCE.Text = CommonHelper.ChopUnusedDecimal(point)
                            TotalPoint += Decimal.Parse(point) '' not null, min value is 0
                            approvedHour += userdine.CEHoursAwarded
                        End If
                        ''bind date
                        Dim lbldate As Label = CType(e.Item.FindControl("lbldate"), Label)
                        If lbldate IsNot Nothing Then
                            If userdine.ProgramDate <> DateTime.MinValue Then
                                lbldate.Text = userdine.ProgramDate.ToString(CommonConstants.DATE_FORMAT)
                            End If
                        End If
                        '' bind CEHours
                        Dim lblpoint As Label = CType(e.Item.FindControl("lblpoint"), Label)
                        If lblpoint IsNot Nothing Then
                            lblpoint.Text = CommonHelper.ChopUnusedDecimal(userdine.CEHours.ToString())
                        End If
                        Dim lblApprovodContacHours As Label = CType(e.Item.FindControl("lblApprovodContacHours"), Label)
                        If lblApprovodContacHours IsNot Nothing Then
                            lblApprovodContacHours.Text = CommonHelper.ChopUnusedDecimal(userdine.CEHoursAwarded.ToString())
                        End If
                    End If
                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub
        ''' <summary>
        ''' Bind CE Point of row into list
        ''' </summary>
        Protected Sub RptcontinuingEducationItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptcontinuingEducation.ItemCommand
            Try
                If e.CommandName.Equals("Delete") Then

                    Dim customerCEActivityItem As IUserDefinedCustomerCEActivity
                    customerCEActivityItem = AMCCertRecertController.GetCustomerExternalCEActivityByGuiId(e.CommandArgument.ToString(), ProgramType, Me.MasterCustomerId, Me.SubCustomerId)
                    AMCCertRecertController.DeleteCustomerExternalCEActivity(customerCEActivityItem)
                End If
                BindingDataToList()
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

#End Region

#Region "Private Methods"
        ''' <summary>
        ''' set value for hdIsIncomplete , check vulues has incorrect or not when page loads
        ''' </summary>
        ''' <param></param>
        Public Overrides Sub ValidateFormFillCompleted()
            ProgramType = ActivityProgramType.EDUCATION.ToString()
            CEWeightList = AMCCertRecertController.GetCertificationCEWeights(OrganizationId, OrganizationUnitId, ProgramType)
            If CheckConflictValidation(RuleID, ConflictMessage) = True AndAlso Not String.IsNullOrEmpty(ConflictMessage) Then
                Dim issuesCollection As New IssuesCollection
                issuesCollection.Add(New CESummaryIssue(New BusinessObject(), ConflictMessage))
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
            End If
            Dim otherModuleSettings = ModuleConfigurationHelper.Instance.GetOtherModuleSettings(Server.MapPath(ParentModulePath))
            If otherModuleSettings IsNot Nothing Then
                MaxCEHoursARN = If(otherModuleSettings.ARNMaxSummaryPointOfContinuingEducation.HasValue, otherModuleSettings.ARNMaxSummaryPointOfContinuingEducation.Value, 0)
            End If
            BindingDataToList()
            CommonHelper.BindCEType(ddlCEType, ProgramType, AMCCertRecertController)
        End Sub

        ''' <summary>
        ''' Binding Data to repeater
        ''' </summary>
        ''' <param></param>
        Private Sub BindingDataToList()
            Dim tabcompleted = True
            Dim activityExternals As IUserDefinedCustomerCEActivities
            activityExternals = AMCCertRecertController.GetExternalActivityList(ProgramType, Me.MasterCustomerId, Me.SubCustomerId)
            If activityExternals IsNot Nothing Then
                If Not Page.IsPostBack Then
                    If activityExternals.Count > 0 Then
                        tabcompleted = True
                    Else '< 0
                        If ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(ValidationRuleId.CONTINUING_EDUCATION_OPTIONAL.ToString(), Server.MapPath(ParentModulePath)) Then
                            tabcompleted = True
                        Else
                            tabcompleted = False
                        End If
                    End If
                End If
            End If
            Me.rptcontinuingEducation.DataSource = activityExternals
            Me.rptcontinuingEducation.DataBind()
            lblTotalCE.Text = CommonHelper.ChopUnusedDecimal(TotalPoint.ToString())
            TotalCEOneForm = TotalPoint
            SetCookie("approvedHour", approvedHour.ToString())
            SetCookie(ProgramType + MasterCustomerId, TotalPoint.ToString())
            Dim result = BuildTitle()
            If result AndAlso tabcompleted Then
                hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
            Else
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
            End If
        End Sub

        ''' <summary>
        ''' Insert or Edit Item to list Cache
        ''' </summary>
        ''' <param></param>
        Private Sub SaveCEActivity()
            Dim customerExternalList As New UserDefinedCustomerCEActivities
            Dim customerExternalCEActivityItem As IUserDefinedCustomerCEActivity
            Dim issueCollection As IIssuesCollection
            Try
                ''edit
                If (Me.hdCurrentObjectUniqueId.Value <> String.Empty) Then
                    customerExternalCEActivityItem = AMCCertRecertController.GetCustomerExternalCEActivityByGuiId(Me.hdCurrentObjectUniqueId.Value,
                                                                                                                  ProgramType, Me.MasterCustomerId,
                                                                                                                  Me.SubCustomerId)
                    If customerExternalCEActivityItem IsNot Nothing Then
                        SetPropertiesForObject(customerExternalCEActivityItem)
                        issueCollection = AMCCertRecertController.UpdateCustomerCEACtivity(customerExternalCEActivityItem)
                    End If
                Else
                    ''Insert
                    customerExternalCEActivityItem = customerExternalList.CreateNew()
                    SetPropertiesForObject(customerExternalCEActivityItem)
                    customerExternalCEActivityItem.IsNewObjectFlag = True
                    issueCollection = AMCCertRecertController.InsertCustomerExternalCEActivity(customerExternalCEActivityItem)
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

        ''' <summary>
        ''' Set property from GUI for Item Object
        ''' </summary>
        ''' <param name="objectItem"> Item Object need to set values </param>
        Private Sub SetPropertiesForObject(ByRef objectItem As IUserDefinedCustomerCEActivity)
            If objectItem IsNot Nothing Then
                objectItem.ValidationIssuesForMe.RemoveAll()
                With objectItem
                    .MasterCustomerId = Me.MasterCustomerId
                    .SubcustomerId = Me.SubCustomerId
                    .ProgramTypeString = ProgramType
                    If .CEType Is Nothing OrElse .CEType.List.Count < 1 Then
                        .CEType.FillList()
                    End If
                    .CETypeString = ddlCEType.SelectedValue
                    .ProgramTitle = txtProgramTittle.Text
                    .OrganizationApproving = txtNameOfOrganizationApproved.Text
                    .OrganizationProviding = txtNameOfOrganizationProviding.Text
                    .ProgramDate = DateTime.MinValue
                    If CType(Me.txtDate, AMCDatetime).Text <> String.Empty Then
                        .ProgramDate = DateTime.Parse(CType(Me.txtDate, AMCDatetime).Text)
                    End If
                    If Me.txtPoint.Text <> String.Empty AndAlso CommonHelper.CheckIsNumber(Me.txtPoint.Text) Then
                        .CEHours = Decimal.Parse(Me.txtPoint.Text)
                    Else
                        .CEHours = CommonConstants.CEHOURS_MIN
                    End If
                    If Me.txtApprovodContacHours.Text <> String.Empty AndAlso CommonHelper.CheckIsNumber(Me.txtApprovodContacHours.Text) Then
                        .CEHoursAwarded = Decimal.Parse(Me.txtApprovodContacHours.Text)
                    Else
                        .CEHoursAwarded = CommonConstants.CEHOURS_MIN
                    End If
                End With
            End If
        End Sub

        Private Sub AddEventHandlers()
            AddHandler btnSave.Click, AddressOf BtnSaveClick
        End Sub

        Private Function CalculatorCurrentCEPoint(ByVal cetypeCode As String,
                                            ByVal cEWeightList As UserDefinedCertificationCEWeights,
                                          ByVal CEhours As String) As Decimal
            FactorIndex = CommonHelper.GetCEWeight(cetypeCode, cEWeightList, True, If(GetFieldInfo("Point") IsNot Nothing AndAlso GetFieldInfo("Point").CalculateFormula > 0, GetFieldInfo("Point").CalculateFormula.ToString(), "1"))
            Return CommonHelper.CalculatorCEItem(CEhours, FactorIndex)
        End Function

        Private Function BuildOptionTitle_ABNN() As Boolean
            Dim result = True
            Dim programTypeSetting = ModuleConfigurationHelper.Instance.GetProgramTypeSetting(ActivityProgramType.CONTEDUCATION.ToString(), Server.MapPath(ParentModulePath))
            Dim programTypeSettingSummary = ModuleConfigurationHelper.Instance.GetProgramTypeSetting(ActivityProgramType.SUMMARY.ToString(),
                                                                                                    Server.MapPath(ParentModulePath))
            Dim totalCEMin As Decimal = 0
            Dim totalCEMax As Decimal = 0
            Dim totalCEMinSummary As Decimal = 0
            Dim totalCE As Decimal = 0
            hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
            totalCE = Decimal.Parse(GetCookie(ProgramType + MasterCustomerId))
            Dim messagesTitle = String.Empty
            If programTypeSetting IsNot Nothing Then
                Dim currentRecertOption = GetCurrentReCertOption()
                If currentRecertOption IsNot Nothing AndAlso programTypeSettingSummary IsNot Nothing Then
                    If currentRecertOption.QuestionCode = Enums.QuestionCode.RECERT_OPTION_2.ToString() Then
                        totalCEMin = Decimal.Parse(programTypeSetting.MinCEOpt2.ToString())
                        totalCEMax = Decimal.Parse(programTypeSetting.MaxCEOpt2.ToString())
                        totalCEMinSummary = Decimal.Parse(programTypeSettingSummary.MinCEOpt2.ToString())
                        messagesTitle = DotNetNuke.Services.Localization.Localization.GetString("ErrorMessagesContinuingEducationOption2.Text", Me.LocalResourceFile)
                    ElseIf currentRecertOption.QuestionCode = Enums.QuestionCode.RECERT_OPTION_3.ToString() Then
                        totalCEMin = Decimal.Parse(programTypeSetting.MinCEOpt3.ToString())
                        totalCEMax = Decimal.Parse(programTypeSetting.MaxCEOpt3.ToString())
                        totalCEMinSummary = Decimal.Parse(programTypeSettingSummary.MinCEOpt3.ToString())
                        messagesTitle = DotNetNuke.Services.Localization.Localization.GetString("ErrorMessagesContinuingEducationOption3.Text", Me.LocalResourceFile)
                    End If
                    '' control if change option => compare point again
                    If totalCE < totalCEMin OrElse totalCE > totalCEMax Then
                        '' optinal rule will be overwrite another rule
                        If ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(ValidationRuleId.CONTINUING_EDUCATION_OPTIONAL.ToString(), Server.MapPath(ParentModulePath)) Then
                           result = True
                        Else
                           result = False
                        End If
                    Else
                        lblErrorMessageChangeOption.Text = String.Empty
                    End If
                    messagesTitle = String.Format(messagesTitle, totalCEMinSummary, totalCEMin, totalCEMax)
                End If
            End If
            lblOptionTitle.Text = messagesTitle
            Return result
        End Function

        Private Function BuildTitle_ARN() As Boolean
            Dim result = True
            lblOptionTitle.Text = String.Empty
            Dim errorMessages = String.Empty
            Dim totalCE As Decimal = 0
            totalCE = Decimal.Parse(GetCookie(ProgramType + MasterCustomerId))
            '' optinal rule will be overwrite another rule
            If ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(ValidationRuleId.CONTINUING_EDUCATION_OPTIONAL.ToString(),
                                                                          Server.MapPath(ParentModulePath)) AndAlso totalCE = 0 Then
                result = True
            Else
                lblOptionTitle.Text = DotNetNuke.Services.Localization.Localization.GetString("MessagesContinuingEducationARN.Text", Me.LocalResourceFile)
                If CheckBusinessValidationCommitData_ARN(errorMessages, totalCE,
                                                             If(GetCookie("approvedHour") <> "0",
                                                                Decimal.Parse(GetCookie("approvedHour")), approvedHour)) Then
                    result = False
                Else
                    result = True
                End If
            End If
            Return result
        End Function

        Private Function BuildTitle() As Boolean
            CheckConflictValidation(RuleID, ConflictMessage)
            Dim ruleResult = True
            If RuleID = ValidationRuleId.RECERT_CON_EDU_COMMON_CALCULATOR.ToString() Then
                ruleResult = BuildOptionTitle_ABNN()
            ElseIf RuleID = ValidationRuleId.RECERT_CON_EDU_ARN.ToString() Then
                ruleResult = BuildTitle_ARN()
            End If
            Return ruleResult
        End Function

        ''' <summary>
        ''' Check validation for ABNN
        ''' </summary>
        ''' <param name="errorMessages"> message will be raised if to violate validation  </param>
        ''' <param name="totalCE"> hours will be compared </param>
        ''' <returns> have or not violate validation </returns>
        Private Function CheckBusinessValidationCommitData_ABNN(ByRef errorMessages As String, ByVal totalCE As Decimal,
                                                                ByRef totalCEMin As Decimal) As Boolean
            Dim programTypeSetting = ModuleConfigurationHelper.Instance.GetProgramTypeSetting(ActivityProgramType.CONTEDUCATION.ToString(),
                                                                                              Server.MapPath(ParentModulePath))
            Dim programTypeSettingSummary = ModuleConfigurationHelper.Instance.GetProgramTypeSetting(ActivityProgramType.SUMMARY.ToString(),
                                                                                                     Server.MapPath(ParentModulePath))
            totalCEMin = 0
            Dim totalCEMax As Decimal = 0
            If programTypeSetting IsNot Nothing AndAlso programTypeSettingSummary IsNot Nothing Then
                Dim currentRecertOption = GetCurrentReCertOption()
                If currentRecertOption IsNot Nothing Then
                    If currentRecertOption.QuestionCode = Enums.QuestionCode.RECERT_OPTION_2.ToString() Then
                        totalCEMin = Decimal.Parse(programTypeSetting.MinCEOpt2.ToString())
                        totalCEMax = Decimal.Parse(programTypeSetting.MaxCEOpt2.ToString())
                        errorMessages = DotNetNuke.Services.Localization.Localization.GetString("MessagesContinuingEducationOption2.Text", Me.LocalResourceFile)
                    ElseIf currentRecertOption.QuestionCode = Enums.QuestionCode.RECERT_OPTION_3.ToString() Then
                        totalCEMin = Decimal.Parse(programTypeSetting.MinCEOpt3.ToString())
                        totalCEMax = Decimal.Parse(programTypeSetting.MaxCEOpt3.ToString())
                        errorMessages = DotNetNuke.Services.Localization.Localization.GetString("MessagesContinuingEducationOption3.Text", Me.LocalResourceFile)
                    End If
                    If totalCE < totalCEMin OrElse totalCE > totalCEMax Then
                        errorMessages = String.Format(errorMessages, totalCEMin, totalCEMax)
                        Return True
                    End If
                End If
            End If
            Return False
        End Function

        ''' <summary>
        ''' Check validation for ARN
        ''' </summary>
        ''' <param name="errorMessages"> message will be raised if to violate validation  </param>
        ''' <param name="contactHours,approvedHours"> hours will be compared </param>
        ''' <returns> have or not violate validation </returns>
        Private Function CheckBusinessValidationCommitData_ARN(ByRef errorMessages As String,
                                                               ByVal contactHours As Decimal,
                                                               ByVal approvedHours As Decimal) As Boolean
            ''
            Dim result As Boolean
            If MaxCEHoursARN < contactHours Then '' if form's hours (contactHours) > hours is configured in control panel (MaxCEHoursARN) => choose MaxCEHoursARN
                contactHours = MaxCEHoursARN
            End If
            Dim leftParam = Math.Ceiling((contactHours * 2) / 3)
            If approvedHours <> 0 AndAlso contactHours <> 0 AndAlso approvedHours >= leftParam Then
                result = False '' pass validation
            ElseIf contactHours = 0 Then
                result = False
            Else
                errorMessages = String.Format(DotNetNuke.Services.Localization.Localization.GetString(
                                              "ErrorMessagesContinuingEducationARN.Text", LocalResourceFile), CommonHelper.ChopUnusedDecimal(contactHours.ToString()))
                result = True
            End If
            Return result
        End Function

        ''' <summary>
        ''' Check validation is conflicted
        ''' </summary>
        ''' <param name="ruleID"> get ruleID and check validation follow this ID </param>
        ''' <param name="conflictMessage"> if have conflict , message will be raised </param>
        ''' <returns> boolean value : have or none conflict  </returns>
        ''' <comment>base on conflictMessage, to distinguish conflict or not validation</comment>
        Public Function CheckConflictValidation(ByRef ruleID As String,
                                                ByRef conflictMessage As String) As Boolean
            Dim validationABNN = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                 ValidationRuleId.RECERT_CON_EDU_COMMON_CALCULATOR.ToString(),
                                                                 Server.MapPath(ParentModulePath))
            Dim validationARN = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                 ValidationRuleId.RECERT_CON_EDU_ARN.ToString(),
                                                                 Server.MapPath(ParentModulePath))
            conflictMessage = String.Empty
            ruleID = String.Empty
            If validationABNN = True AndAlso validationARN = True Then
                conflictMessage = DotNetNuke.Services.Localization.Localization.GetString("ConflictMessage.Text", Me.LocalResourceFile)
                Return True
            ElseIf validationABNN = True Then
                ruleID = ValidationRuleId.RECERT_CON_EDU_COMMON_CALCULATOR.ToString()
                Return False
            ElseIf validationARN = True Then
                ruleID = ValidationRuleId.RECERT_CON_EDU_ARN.ToString()
                Return False
            End If
            Return True '' don't have validation because conflictMessage is empty
        End Function


#End Region

#Region "IReload"

        Public ReadOnly Property SaveControls() As List(Of String) Implements IReload.SaveControls
            Get
                Dim result = New List(Of String)
                result.Add("RecertificationOptionUC")
                Return result
            End Get
        End Property

        ''' <summary>
        ''' Reloads this instance.
        ''' </summary>
        Public Sub Reload(ByVal saveControlId As String) Implements IReload.Reload
            If SaveControls.Contains(saveControlId) Then
                If Not BuildTitle() Then
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                Else
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                End If
            End If
        End Sub
#End Region

    End Class
End Namespace
