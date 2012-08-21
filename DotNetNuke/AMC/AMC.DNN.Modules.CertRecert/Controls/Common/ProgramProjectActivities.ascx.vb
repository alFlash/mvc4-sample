Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Controls.Reusable
Imports AMC.DNN.Modules.CertRecert.Business.BusinessValidation
Imports TIMSS.API.Core
Imports TIMSS.API.User.UserDefinedInfo
Imports TIMSS.API.Core.Validation
Imports AMC.DNN.Modules.CertRecert.Data.Enums

Namespace Controls.Common
    Public Class ProgramProjectActivities
        Inherits SectionBaseUserControl

#Region "public Member"

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
                ProgramType = ActivityProgramType.PROGPROJECT.ToString()
                CEWeightList = AMCCertRecertController.GetCertificationCEWeights(OrganizationId, OrganizationUnitId, ProgramType)
                If CheckConflictValidation(RuleID, ConflictMessage) = True AndAlso Not String.IsNullOrEmpty(ConflictMessage) Then
                    Dim issuesCollection As New IssuesCollection
                    issuesCollection.Add(New CESummaryIssue(New BusinessObject(), ConflictMessage))
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                End If
                BuildTitle() ''check change option
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Protected Sub btnSaveClick(ByVal sender As Object, ByVal e As EventArgs)
            Try
                SaveCEActivity()
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Protected Sub rptProgramProjectActivities_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptProgramProjectActivities.ItemCommand
            Try
                If e.CommandName.Equals("Delete") Then
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                    Dim customerCEActivityItem As IUserDefinedCustomerCEActivity
                    customerCEActivityItem = AMCCertRecertController.GetCustomerExternalCEActivityByGuiId(e.CommandArgument.ToString(), ProgramType, Me.MasterCustomerId, Me.SubCustomerId)
                    AMCCertRecertController.DeleteCustomerExternalCEActivity(customerCEActivityItem)
                End If
                BindingDataToList()
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Public Overrides Function Save() As IIssuesCollection
            ''Commit data
            Try
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                Dim results = AMCCertRecertController.CommitCustomerExternalCEActivity(ProgramType, MasterCustomerId, SubCustomerId)
                BindingDataToList()
                '' check validation
                Dim iissueResult As New IssuesCollection
                Dim totalCEMin As Decimal = 0
                Dim errorMessages = String.Empty
                If Not String.IsNullOrEmpty(ConflictMessage) Then '' validation rule is conflicted
                    iissueResult.Add(New CheckBoxMustSelectedIssue(New BusinessObject(), ConflictMessage))
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                    Return iissueResult
                End If
                If RuleID = ValidationRuleId.RECERT_PRO_PROJ_ACTIVITY_MIM_MAX_ABNN.ToString() Then
                    If CheckBusinessValidationCommitDataABNNMinMaxCEType_AllList(errorMessages, totalCEMin) Then
                        iissueResult.Add(New CheckBoxMustSelectedIssue(New BusinessObject(), errorMessages))
                        hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                        Return iissueResult
                    End If
                End If
                '' End check validation
                If results Is Nothing OrElse results.Count <= 0 Then
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                End If
                If rptProgramProjectActivities.Items Is Nothing OrElse rptProgramProjectActivities.Items.Count <= 0 Then
                    '' if check rule and set ceMin = 0 => pass basic rule
                    If RuleID = ValidationRuleId.RECERT_PRO_PROJ_ACTIVITY_MIM_MAX_ABNN.ToString() AndAlso totalCEMin = 0 Then
                        hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                    Else
                        hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                    End If
                End If
                Return results
            Catch ex As Exception
                ProcessException(ex)
            End Try
            Return Nothing
        End Function

        Protected Sub rptProgramProjectActivities_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptProgramProjectActivities.ItemDataBound
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

                            ''get CEtype Name : validation rule
                            If String.IsNullOrEmpty(CETypeNameList) OrElse CETypeNameList.IndexOf(userdine.CETypeString) = -1 Then
                                CETypeNameList += userdine.CETypeString + ","
                            End If

                            '' get FactorIndex base on Association
                            FactorIndex = "1"
                            If RuleID = ValidationRuleId.RECERT_PRO_PROJ_ACTIVITY_MIM_MAX_ABNN.ToString() Then
                                FactorIndex = CommonHelper.GetCEWeight(userdine.CEType.List(userdine.CETypeString).Code, CEWeightList, True, "1")
                            ElseIf String.IsNullOrEmpty(RuleID) Then
                                FactorIndex = CommonHelper.GetCEWeight(userdine.CEType.List(userdine.CETypeString).Code, CEWeightList, False,
                                                                       If(GetFieldInfo("Point") IsNot Nothing AndAlso GetFieldInfo("Point").CalculateFormula > 0,
                                                                          GetFieldInfo("Point").CalculateFormula.ToString(), "1"))
                            End If

                            '' calculator point
                            point = CommonHelper.CalculatorCEItem(userdine.CEHours.ToString(), FactorIndex).ToString()
                            lblCE.Text = CommonHelper.ChopUnusedDecimal(point)
                            TotalPoint += Decimal.Parse(point) '' not null, min value is 0
                        End If
                        ''bind date
                        Dim lbldate As Label = CType(e.Item.FindControl("lblCompletionDate"), Label)
                        If lbldate IsNot Nothing Then
                            If userdine.ProgramDate <> DateTime.MinValue Then
                                lbldate.Text = userdine.ProgramDate.ToString(CommonConstants.DATE_FORMAT)
                            End If
                        End If
                        '' bind CEHours
                        Dim lblpoint As Label = CType(e.Item.FindControl("lblPoint"), Label)
                        If lblpoint IsNot Nothing Then
                            lblpoint.Text = CommonHelper.ChopUnusedDecimal(userdine.CEHours.ToString())
                        End If
                    End If
                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub
#End Region

#Region "Private Methods"
        Public Overrides Sub ValidateFormFillCompleted()
            Try
                ProgramType = ActivityProgramType.PROGPROJECT.ToString()
                CEWeightList = AMCCertRecertController.GetCertificationCEWeights(OrganizationId, OrganizationUnitId, ProgramType)
                If CheckConflictValidation(RuleID, ConflictMessage) = True AndAlso Not String.IsNullOrEmpty(ConflictMessage) Then
                    Dim issuesCollection As New IssuesCollection
                    issuesCollection.Add(New CESummaryIssue(New BusinessObject(), ConflictMessage))
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                End If
                CommonHelper.BindCEType(ddlCEType, ProgramType, AMCCertRecertController)
                BindingDataToList()
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        ''' <summary>
        ''' Binding Data to repeater
        ''' </summary>
        ''' <param></param>
        Private Sub BindingDataToList()
            Try
                Dim activityExternals As IUserDefinedCustomerCEActivities
                activityExternals = AMCCertRecertController.GetExternalActivityList(ProgramType, Me.MasterCustomerId, Me.SubCustomerId)
                If activityExternals.Count > 0 Then
                    If Not Page.IsPostBack Then
                        hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                    End If
                End If
                Me.rptProgramProjectActivities.DataSource = activityExternals
                Me.rptProgramProjectActivities.DataBind()
                lblTotalCE.Text = CommonHelper.ChopUnusedDecimal(TotalPoint.ToString())
                SetCookie(ProgramType + MasterCustomerId, TotalPoint.ToString())
                BuildTitle()
            Catch ex As Exception
                ProcessException(ex)
            End Try

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
                    customerExternalCEActivityItem = AMCCertRecertController.GetCustomerExternalCEActivityByGuiId(Me.hdCurrentObjectUniqueId.Value, ProgramType, Me.MasterCustomerId, Me.SubCustomerId)
                    If customerExternalCEActivityItem IsNot Nothing Then
                        SetPropertiesForObject(customerExternalCEActivityItem)
                        issueCollection = AMCCertRecertController.UpdateCustomerCEACtivity(customerExternalCEActivityItem)
                    End If
                Else
                    ''Insert
                    customerExternalCEActivityItem = customerExternalList.CreateNew()
                    ''set properties
                    SetPropertiesForObject(customerExternalCEActivityItem)
                    customerExternalCEActivityItem.IsNewObjectFlag = True
                    ''call insert fucntion of amc controller
                    issueCollection = AMCCertRecertController.InsertCustomerExternalCEActivity(customerExternalCEActivityItem)
                End If
                If issueCollection.Count > 0 Then
                    ShowError(issueCollection, lblPopupMessage)
                    hdIsValidateFailed.Value = CommonConstants.TAB_INCOMPLETED
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
                    .PublicationTitle = Me.txtProgramTitle.Text
                    .OrganizationProviding = Me.txtOrganization.Text
                    If CType(Me.txtProgramDate, AMCDatetime).Text <> String.Empty Then
                        .ProgramDate = DateTime.Parse(CType(Me.txtProgramDate, AMCDatetime).Text)
                    End If
                    If Me.txtApproved.Text <> String.Empty AndAlso CommonHelper.CheckIsNumber(Me.txtApproved.Text) Then
                        .CEHours = Decimal.Parse(Me.txtApproved.Text)
                    Else
                        .CEHours = CommonConstants.CEHOURS_MIN
                    End If
                End With
            End If
        End Sub

        Private Sub AddEventHandlers()
            AddHandler btnSave.Click, AddressOf btnSaveClick
        End Sub
#End Region

#Region "validation"
        Public Function CheckConflictValidation(ByRef ruleID As String,
                                              ByRef conflictMessage As String) As Boolean

            Dim validationABNN_MinMaxItem = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                ValidationRuleId.RECERT_PRO_PROJ_ACTIVITY_MIM_MAX_ABNN.ToString(),
                                                                Server.MapPath(ParentModulePath))
            conflictMessage = String.Empty
            ruleID = String.Empty
            If validationABNN_MinMaxItem = True Then
                ruleID = ValidationRuleId.RECERT_PRO_PROJ_ACTIVITY_MIM_MAX_ABNN.ToString()
                Return False
            End If
            Return True   '' don't have validation rule
        End Function

        ''' <summary>
        ''' Check validation for ABNN . All CEType in GridVIew
        ''' </summary>
        ''' <param name="errorMessages"> message will be raised if to violate validation  </param>
        ''' <param name="maxHours,totalHours"> hours will be compared </param>
        ''' <returns> have or not violate validation </returns>
        Private Function CheckBusinessValidationCommitDataABNNMinMaxCEType_AllList(ByRef errorMessages As String, ByRef totalCEMin As Decimal) As Boolean
            Dim flag = False '' pass rule
            '' check validation for each item
            If Not String.IsNullOrEmpty(CETypeNameList) Then
                Dim arrCETypeName As String() = CETypeNameList.TrimEnd(","c).Split(","c)
                If arrCETypeName.Length > 0 Then
                    For i As Integer = 0 To arrCETypeName.Length - 1
                        Dim subMessage = String.Empty
                        If CheckBusinessValidationCommitDataABNNMinMaxCEType(arrCETypeName(i), subMessage) Then
                            errorMessages = errorMessages + String.Format("{0} </br>", subMessage)
                            flag = True
                        End If
                    Next
                End If
            End If
            '' check total CE point must be between min and max CE point
            Dim programTypeSetting = ModuleConfigurationHelper.Instance.GetProgramTypeSetting(ProgramType, Server.MapPath(ParentModulePath))
            Dim totalCEMax As Decimal = 0
            Dim totalCE As Decimal = 0
            totalCEMin = 0
            totalCE = Decimal.Parse(GetCookie(ProgramType + MasterCustomerId))
            Dim subMessageMinMax = String.Empty
            If programTypeSetting IsNot Nothing Then
                Dim currentRecertOption = GetCurrentReCertOption()
                If currentRecertOption IsNot Nothing Then
                    If currentRecertOption.QuestionCode = Enums.QuestionCode.RECERT_OPTION_2.ToString() Then
                        totalCEMin = Decimal.Parse(programTypeSetting.MinCEOpt2.ToString())
                        totalCEMax = Decimal.Parse(programTypeSetting.MaxCEOpt2.ToString())
                        subMessageMinMax = DotNetNuke.Services.Localization.Localization.GetString("MessagesContinuingEducationOption2.Text", Me.LocalResourceFile)
                    ElseIf currentRecertOption.QuestionCode = Enums.QuestionCode.RECERT_OPTION_3.ToString() Then
                        totalCEMin = Decimal.Parse(programTypeSetting.MinCEOpt3.ToString())
                        totalCEMax = Decimal.Parse(programTypeSetting.MaxCEOpt3.ToString())
                        subMessageMinMax = DotNetNuke.Services.Localization.Localization.GetString("MessagesContinuingEducationOption3.Text", Me.LocalResourceFile)
                    End If
                    If totalCE < totalCEMin OrElse totalCE > totalCEMax Then
                        errorMessages += "</br>" + String.Format(subMessageMinMax, totalCEMin, totalCEMax)
                        Return True
                    End If
                End If
            End If
            Return flag
        End Function

        ''' <summary>
        ''' Check validation for ABNN . One CEType
        ''' </summary>
        ''' <param name="errorMessages"> message will be raised if to violate validation  </param>
        ''' <param name="maxHours,totalHours"> hours will be compared </param>
        ''' <returns> have or not violate validation </returns>
        Private Function CheckBusinessValidationCommitDataABNNMinMaxCEType(ByVal cETypeCode As String,
                                                                           ByRef errorMessages As String) As Boolean
            Dim weight As Decimal = 1
            Dim minPoint As Decimal = 0
            Dim maxPoint As Decimal = 0
            Dim hours As Decimal = 0
            Dim points As Decimal = 0
            Dim ceTypeName = String.Empty
            GetMinMaxofCEType(cETypeCode, weight, minPoint, maxPoint, ceTypeName)
            hours = SumTotalHoursOfCEType(cETypeCode)
            points = hours * weight
            If minPoint = 0 AndAlso maxPoint = 0 Then '' if not config incontrol panal => pass
                Return False
            End If
            If points < minPoint OrElse points > maxPoint Then
                errorMessages = String.Format(DotNetNuke.Services.Localization.Localization.GetString(
                                            "ValidationErrorMessageABNN_MinMaxPointsCEType.Text", Me.LocalResourceFile),
                                               ceTypeName, CommonHelper.ChopUnusedDecimal(minPoint.ToString()),
                                               CommonHelper.ChopUnusedDecimal(maxPoint.ToString()),
                                               CommonHelper.ChopUnusedDecimal(points.ToString()))
                Return True
            End If
            Return False
        End Function

        Private Sub GetMinMaxofCEType(ByVal cETypeCode As String,
                                      ByRef weight As Decimal,
                                      ByRef minPoint As Decimal,
                                      ByRef maxpoint As Decimal,
                                      ByRef ceTypeName As String)
            If CEWeightList IsNot Nothing Then
                If CEWeightList.Count > 0 Then
                    For Each ceWeightObject As UserDefinedCertificationCEWeight In CEWeightList
                        If ceWeightObject.CEType.Code = cETypeCode Then
                            weight = ceWeightObject.Weight
                            minPoint = ceWeightObject.MinCE
                            maxpoint = ceWeightObject.MaxCE
                            ceTypeName = ceWeightObject.CEType.Description
                            Exit For
                        End If
                    Next
                End If
            End If
        End Sub

        ''' <summary>
        ''' sum CE hours of List Object in GridVIew
        ''' </summary>
        ''' <param name="ceType"> CEType want to sum CE hours  </param>
        ''' <returns> hours </returns>
        Private Function SumTotalHoursOfCEType(ByVal ceType As String) As Decimal
            Dim activityExternals As IUserDefinedCustomerCEActivities
            activityExternals = AMCCertRecertController.GetExternalActivityList(ProgramType, Me.MasterCustomerId, Me.SubCustomerId)
            Dim hours As Decimal = 0
            For Each objectItem As IUserDefinedCustomerCEActivity In activityExternals
                If objectItem.CETypeString = ceType Then
                    If CommonHelper.CheckIsNumber(objectItem.CEHours.ToString()) Then
                        hours += Decimal.Parse(objectItem.CEHours.ToString())
                    End If
                End If
            Next
            Return hours
        End Function

        Private Sub BuildTitle()
            lblOptionTitle.Text = String.Empty
            CheckConflictValidation(RuleID, ConflictMessage)
            If RuleID = ValidationRuleId.RECERT_PRO_PROJ_ACTIVITY_MIM_MAX_ABNN.ToString() Then
                BuildTitle_ABNN_MinMaxPointsCEType()
            End If
        End Sub

        Public Sub BuildTitle_ABNN_MinMaxPointsCEType()
            lblOptionTitle.Text = String.Empty
            hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
            If Not String.IsNullOrEmpty(CETypeNameList) Then
                Dim arrCETypeName As String() = CETypeNameList.TrimEnd(","c).Split(","c)
                If arrCETypeName.Length > 0 Then
                    For i As Integer = 0 To arrCETypeName.Length - 1
                        If CheckBusinessValidationCommitDataABNNMinMaxCEType(arrCETypeName(i), String.Empty) Then
                            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                            Exit For
                        End If
                    Next
                End If
            End If
            '' check total CE point must be between min and max CE point
            Dim programTypeSetting = ModuleConfigurationHelper.Instance.GetProgramTypeSetting(ProgramType, Server.MapPath(ParentModulePath))
            Dim programTypeSettingSummary = ModuleConfigurationHelper.Instance.GetProgramTypeSetting(ActivityProgramType.SUMMARY.ToString(),
                                                                                                    Server.MapPath(ParentModulePath))
            Dim totalCEMin As Decimal = 0
            Dim totalCEMax As Decimal = 0
            Dim totalCEMinSummary As Decimal = 0
            Dim totalCE As Decimal = 0
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
                        hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                    End If
                    messagesTitle = String.Format(messagesTitle, totalCEMinSummary, totalCEMin, totalCEMax)
                End If
            End If
            lblOptionTitle.Text = messagesTitle
        End Sub
#End Region

    End Class
End Namespace