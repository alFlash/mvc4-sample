Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Controls.Reusable
Imports AMC.DNN.Modules.CertRecert.Business.IControls
Imports AMC.DNN.Modules.CertRecert.Business.BusinessValidation
Imports AMC.DNN.Modules.CertRecert.Data.Enums
Imports TIMSS.API.Core
Imports TIMSS.API.User.UserDefinedInfo
Imports TIMSS.API.Core.Validation


Namespace Controls.Common
    Public Class Publication
        Inherits SectionBaseUserControl
        Implements IReload

#Region "Private Member"

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
                ProgramType = ActivityProgramType.PUBLICATION.ToString()
                cEWeightList = AMCCertRecertController.GetCertificationCEWeights(OrganizationId, OrganizationUnitId, ProgramType)
                maxCEHoursARN = 0
                Dim otherModuleSettings = ModuleConfigurationHelper.Instance.GetOtherModuleSettings(Server.MapPath(ParentModulePath))
                If otherModuleSettings IsNot Nothing Then
                    maxCEHoursARN = If(otherModuleSettings.PublicationTotalHours.HasValue, otherModuleSettings.PublicationTotalHours.Value, 0)
                End If
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

        Public Overrides Function Save() As IIssuesCollection
            ''Commit data
            Try
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                Dim results = AMCCertRecertController.CommitCustomerExternalCEActivity(ProgramType, MasterCustomerId, SubCustomerId)
                If results Is Nothing OrElse results.Count <= 0 Then '' not issues when save data
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                End If
                BindingDataToList()
                '' optinal rule will be overwrite another rule
                If ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(ValidationRuleId.PROFESSIONAL_PUBLICATIONS_OPTIONAL.ToString(), Server.MapPath(ParentModulePath)) Then
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                Else
                    '' check validation
                    Dim iissueResult As New IssuesCollection
                    Dim totalCEMin As Decimal = 0
                    Dim errorMessages = String.Empty
                    Dim totalCEHours As Decimal = 0
                    totalCEHours = Decimal.Parse(GetCookie(ProgramType + MasterCustomerId))
                    If Not String.IsNullOrEmpty(ConflictMessage) Then '' validation rule is conflicted
                        iissueResult.Add(New CheckBoxMustSelectedIssue(New BusinessObject(), ConflictMessage))
                        hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                        Return iissueResult
                    End If
                    If RuleID = ValidationRuleId.RECERT_PUBLICATION_EQUAL_ARN.ToString() Then  '' 
                        If CheckBusinessValidationCommitData_ARN_Equal(errorMessages, MaxCEHoursARN, totalCEHours) Then
                            iissueResult.Add(New CheckBoxMustSelectedIssue(New BusinessObject(), errorMessages))
                            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                            Return iissueResult
                        End If
                    ElseIf RuleID = ValidationRuleId.RECERT_PUBLICATION_GREATER_ARN.ToString() Then  ''
                        If CheckBusinessValidationCommitData_ARN_AllowGreater(errorMessages, MaxCEHoursARN, totalCEHours) Then
                            iissueResult.Add(New CheckBoxMustSelectedIssue(New BusinessObject(), errorMessages))
                            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                            Return iissueResult
                        End If
                    ElseIf RuleID = ValidationRuleId.RECERT_PUBLICATION_MIM_MAX_ABNN.ToString() Then
                        If CheckBusinessValidationCommitDataABNNMinMaxCEType_AllList(errorMessages, totalCEMin) Then
                            iissueResult.Add(New CheckBoxMustSelectedIssue(New BusinessObject(), errorMessages))
                            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                            Return iissueResult
                        End If
                    End If
                    '' End check validation
                    If rptCommunityPublication.Items Is Nothing OrElse rptCommunityPublication.Items.Count <= 0 Then
                        '' if check rule and set ceMin = 0 => pass basic rule
                        If RuleID = ValidationRuleId.RECERT_PUBLICATION_MIM_MAX_ABNN.ToString() AndAlso totalCEMin = 0 Then
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

        Protected Sub rptCommunityPublication_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptCommunityPublication.ItemCommand
            Try
                If e.CommandName.Equals("Delete") Then
                    Dim customerCEActivityItem As IUserDefinedCustomerCEActivity
                    customerCEActivityItem = amcCertRecertController.GetCustomerExternalCEActivityByGuiId(e.CommandArgument.ToString(), ProgramType, Me.MasterCustomerId, Me.SubCustomerId)
                    amcCertRecertController.DeleteCustomerExternalCEActivity(customerCEActivityItem)
                End If
                BindingDataToList()
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Protected Sub rptCommunityPublication_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptCommunityPublication.ItemDataBound
            Try
                If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
                    Dim userdine As UserDefinedCustomerCEActivity = CType(e.Item.DataItem, UserDefinedCustomerCEActivity)
                    If userdine IsNot Nothing Then
                        '' bind CE Type
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
                            ''FactorIndex = "1"
                            If RuleID = ValidationRuleId.RECERT_PUBLICATION_EQUAL_ARN.ToString() OrElse
                                        RuleID = ValidationRuleId.RECERT_PUBLICATION_GREATER_ARN.ToString() Then   '' base on control panel 
                                FactorIndex = CommonHelper.GetCEWeight(userdine.CEType.List(userdine.CETypeString).Code, CEWeightList, False,
                                                                        If(GetFieldInfo("Point") IsNot Nothing AndAlso GetFieldInfo("Point").CalculateFormula > 0,
                                                                           GetFieldInfo("Point").CalculateFormula.ToString(), "1"))
                                '' calculator point
                                point = FactorIndex  '' Set point by value is configed in control Panel
                            ElseIf RuleID = ValidationRuleId.RECERT_PUBLICATION_MIM_MAX_ABNN.ToString() Then
                                FactorIndex = CommonHelper.GetCEWeight(userdine.CEType.List(userdine.CETypeString).Code, CEWeightList, True, "1")
                                '' calculator point
                                point = CommonHelper.CalculatorCEItem(userdine.CEHours.ToString(), FactorIndex).ToString()
                            End If
                            If point Is Nothing Then
                                point = CommonHelper.CalculatorCEItem(userdine.CEHours.ToString(), "1").ToString()
                            End If
                            lblCE.Text = CommonHelper.ChopUnusedDecimal(point)
                            totalPoint += Decimal.Parse(point) '' not null, min value is 0
                        End If
                        '' bind date
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
                    End If
                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

#End Region

#Region "Private Methods"
        ''' <summary>
        ''' set value for hdIsIncomplete , check vulues has incorrect or not when page loads
        ''' </summary>
        Public Overrides Sub ValidateFormFillCompleted()
            Try
                ProgramType = ActivityProgramType.PUBLICATION.ToString()
                CEWeightList = AMCCertRecertController.GetCertificationCEWeights(OrganizationId, OrganizationUnitId, ProgramType)
                If CheckConflictValidation(RuleID, ConflictMessage) = True AndAlso Not String.IsNullOrEmpty(ConflictMessage) Then
                    Dim issuesCollection As New IssuesCollection
                    issuesCollection.Add(New CESummaryIssue(New BusinessObject(), ConflictMessage))
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                End If
                MaxCEHoursARN = 0
                Dim otherModuleSettings = ModuleConfigurationHelper.Instance.GetOtherModuleSettings(Server.MapPath(ParentModulePath))
                If otherModuleSettings IsNot Nothing Then
                    MaxCEHoursARN = If(otherModuleSettings.PublicationTotalHours.HasValue, otherModuleSettings.PublicationTotalHours.Value, 0)
                End If
                BindingDataToList()
                CommonHelper.BindCEType(ddlCEType, ProgramType, AMCCertRecertController)
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Binding Data to repeater
        ''' </summary>
        ''' <param></param>
        Private Sub BindingDataToList()
            Try
                Dim activityExternals As IUserDefinedCustomerCEActivities
                CETypeNameList = String.Empty
                activityExternals = AMCCertRecertController.GetExternalActivityList(ProgramType, Me.MasterCustomerId, Me.SubCustomerId)
                If activityExternals IsNot Nothing Then
                    If Not Page.IsPostBack Then
                        If activityExternals.Count > 0 Then
                            hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                        Else
                            If ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(ValidationRuleId.PROFESSIONAL_PUBLICATIONS_OPTIONAL.ToString(), Server.MapPath(ParentModulePath)) Then
                                hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                            Else
                                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                            End If
                        End If
                    End If
                End If
                Me.rptCommunityPublication.DataSource = activityExternals
                Me.rptCommunityPublication.DataBind()
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
                If issueCollection IsNot Nothing Then
                    If issueCollection.Count > 0 Then
                        ShowError(issueCollection, lblPopupMessage)
                        hdIsValidateFailed.Value = CommonConstants.TAB_INCOMPLETED
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
                    .PublicationTitle = Me.txtTitlePublication.Text
                    .ProgramTitle = Me.txtTitleManuscript.Text
                    .ArticlePage = Me.txtPages.Text
                    .OrganizationProviding = Me.txtNameOfPublisher.Text
                    .ProgramDate = DateTime.MinValue
                    If CType(Me.txtDate, AMCDatetime).Text <> String.Empty Then
                        .ProgramDate = DateTime.Parse(CType(Me.txtDate, AMCDatetime).Text)
                    End If
                    If Me.txtPoint.Text <> String.Empty AndAlso CommonHelper.CheckIsNumber(Me.txtPoint.Text) Then
                        .CEHours = Decimal.Parse(Me.txtPoint.Text)
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
        ''' <summary>
        ''' Check validation is conflicted
        ''' </summary>
        ''' <param name="ruleID"> get ruleID and check validation follow this ID </param>
        ''' <param name="conflictMessage"> if have conflict , message will be raised </param>
        ''' <returns> boolean value : have or none conflict  </returns>
        ''' <comment>base on conflictMessage, to distinguish conflict or not validation</comment>
        Public Function CheckConflictValidation(ByRef ruleID As String,
                                               ByRef conflictMessage As String) As Boolean
            Dim validationARN_Equal = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                 ValidationRuleId.RECERT_PUBLICATION_EQUAL_ARN.ToString(),
                                                                 Server.MapPath(ParentModulePath))
            Dim validationARN_Greater = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                 ValidationRuleId.RECERT_PUBLICATION_GREATER_ARN.ToString(),
                                                                 Server.MapPath(ParentModulePath))
            Dim validationABNN_MinMaxItem = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                ValidationRuleId.RECERT_PUBLICATION_MIM_MAX_ABNN.ToString(),
                                                                Server.MapPath(ParentModulePath))
            conflictMessage = String.Empty
            ruleID = String.Empty
            If validationABNN_MinMaxItem = True AndAlso validationARN_Equal = True OrElse
                validationABNN_MinMaxItem = True AndAlso validationARN_Greater = True OrElse
                validationABNN_MinMaxItem = True AndAlso validationARN_Equal = True AndAlso validationARN_Greater = True Then
                conflictMessage = DotNetNuke.Services.Localization.Localization.GetString("ConflictMessage.Text", Me.LocalResourceFile)
                Return True
            ElseIf validationABNN_MinMaxItem = True Then
                ruleID = ValidationRuleId.RECERT_PUBLICATION_MIM_MAX_ABNN.ToString()
                Return False
            ElseIf validationARN_Greater = True Then
                ruleID = ValidationRuleId.RECERT_PUBLICATION_GREATER_ARN.ToString()
                Return False
            ElseIf validationARN_Equal = True Then
                ruleID = ValidationRuleId.RECERT_PUBLICATION_EQUAL_ARN.ToString()
                Return False
            End If
            Return True   '' don't have validation rule
        End Function

        ''' <summary>
        ''' Check validation for ARN
        ''' </summary>
        ''' <param name="errorMessages"> message will be raised if to violate validation  </param>
        ''' <param name="maxHours,totalHours"> hours will be compared </param>
        ''' <returns> have or not violate validation </returns>
        Private Function CheckBusinessValidationCommitData_ARN_Equal(ByRef errorMessages As String,
                                                               ByVal maxHours As Decimal,
                                                               ByVal totalHours As Decimal) As Boolean
            If maxHours <> totalHours Then
                errorMessages = String.Format(DotNetNuke.Services.Localization.Localization.GetString(
                                              "ValidationErrorMessageARN_equal.Text", Me.LocalResourceFile), CommonHelper.ChopUnusedDecimal(maxHours.ToString()))
                Return True
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Check validation for ARN
        ''' </summary>
        ''' <param name="errorMessages"> message will be raised if to violate validation  </param>
        ''' <param name="maxHours,totalHours"> hours will be compared </param>
        ''' <returns> have or not violate validation </returns>
        Private Function CheckBusinessValidationCommitData_ARN_AllowGreater(ByRef errorMessages As String,
                                                               ByVal maxHours As Decimal,
                                                               ByVal totalHours As Decimal) As Boolean
            If totalHours < maxHours Then
                errorMessages = String.Format(DotNetNuke.Services.Localization.Localization.GetString(
                                              "ValidationErrorMessageARN_greater.Text", Me.LocalResourceFile), CommonHelper.ChopUnusedDecimal(maxHours.ToString()))
                Return True
            Else
                Return False
            End If
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
                            errorMessages += String.Format("{0} </br>", subMessage)
                            flag = True
                        End If
                    Next
                End If
            End If
            '' check total CE point must be between min and max CE point
            Dim programTypeSetting = ModuleConfigurationHelper.Instance.GetProgramTypeSetting(ProgramType, Server.MapPath(ParentModulePath))
            totalCEMin = 0
            Dim totalCEMax As Decimal = 0
            Dim totalCE As Decimal = 0
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
            Dim cETypeName = String.Empty
            GetMinMaxofCEType(cETypeCode, weight, minPoint, maxPoint, cETypeName)
            hours = SumTotalHoursOfCEType(cETypeCode)
            points = hours * weight
            If minPoint = 0 AndAlso maxPoint = 0 Then '' if not config incontrol panal => pass
                Return False
            End If
            If points < minPoint OrElse points > maxPoint Then
                errorMessages = String.Format(DotNetNuke.Services.Localization.Localization.GetString(
                                            "ValidationErrorMessageABNN_MinMaxPointsCEType.Text", Me.LocalResourceFile),
                                               cETypeName, CommonHelper.ChopUnusedDecimal(minPoint.ToString()),
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
            Dim hdnARN As HiddenField = CType(FindControl("hdnARN"), HiddenField)
            Dim hdnPointValuesDefault As HiddenField = CType(FindControl("hdnPointValuesDefault"), HiddenField)
            CheckConflictValidation(RuleID, ConflictMessage)
            If RuleID = ValidationRuleId.RECERT_PUBLICATION_GREATER_ARN.ToString() Then
                BuildTitle_ARN_GreaterMaxHours()
                '' set default value for point (ARN)
                If hdnARN IsNot Nothing AndAlso hdnPointValuesDefault IsNot Nothing AndAlso GetFieldInfo("Point") IsNot Nothing Then
                    hdnARN.Value = "true"
                    hdnPointValuesDefault.Value = CommonHelper.ChopUnusedDecimal(GetFieldInfo("Point").CalculateFormula.ToString())
                End If
            ElseIf RuleID = ValidationRuleId.RECERT_PUBLICATION_EQUAL_ARN.ToString() Then
                BuildTitle_ARN_EqualMaxHours()
                '' set default value for point (ARN)
                If hdnARN IsNot Nothing AndAlso hdnPointValuesDefault IsNot Nothing AndAlso GetFieldInfo("Point") IsNot Nothing Then
                    hdnARN.Value = "true"
                    hdnPointValuesDefault.Value = CommonHelper.ChopUnusedDecimal(GetFieldInfo("Point").CalculateFormula.ToString())
                End If
            ElseIf RuleID = ValidationRuleId.RECERT_PUBLICATION_MIM_MAX_ABNN.ToString() Then
                BuildTitle_ABNN_MinMaxPointsCEType()
            End If
        End Sub

        Public Sub BuildTitle_ARN_GreaterMaxHours()
            lblOptionTitle.Text = String.Empty
            Dim errorMessages = String.Empty
            Dim totalCEHours As Decimal = 0
            totalCEHours = Decimal.Parse(GetCookie(ProgramType + MasterCustomerId))
            '' optinal rule will be overwrite another rule
            If ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(ValidationRuleId.PROFESSIONAL_PUBLICATIONS_OPTIONAL.ToString(), Server.MapPath(ParentModulePath)) Then
                hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
            Else
                lblOptionTitle.Text = String.Format(DotNetNuke.Services.Localization.Localization.GetString(
                                              "MessagesARNAllowGreaterMaxHours.Text", Me.LocalResourceFile), MaxCEHoursARN)
                If CheckBusinessValidationCommitData_ARN_AllowGreater(errorMessages, MaxCEHoursARN, totalCEHours) Then
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                Else
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                End If
            End If
        End Sub

        Public Sub BuildTitle_ARN_EqualMaxHours()
            lblOptionTitle.Text = String.Empty
            Dim errorMessages = String.Empty
            Dim totalCEHours As Decimal = 0
            totalCEHours = Decimal.Parse(GetCookie(ProgramType + MasterCustomerId))
            '' optinal rule will be overwrite another rule
            If ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(ValidationRuleId.PROFESSIONAL_PUBLICATIONS_OPTIONAL.ToString(), Server.MapPath(ParentModulePath)) Then
                hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
            Else
                lblOptionTitle.Text = String.Format(DotNetNuke.Services.Localization.Localization.GetString(
                                             "MessagesARNMustEqualMaxHours.Text", Me.LocalResourceFile), MaxCEHoursARN)
                If CheckBusinessValidationCommitData_ARN_Equal(errorMessages, MaxCEHoursARN, totalCEHours) Then
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                Else
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                End If
            End If
        End Sub

        Public Sub BuildTitle_ABNN_MinMaxPointsCEType()
            lblOptionTitle.Text = String.Empty
            hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
            '' optinal rule will be overwrite another rule
            If ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(ValidationRuleId.PROFESSIONAL_PUBLICATIONS_OPTIONAL.ToString(), Server.MapPath(ParentModulePath)) Then
                hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
            Else '' check for each item
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
                        '' optinal rule will be overwrite another rule
                        If ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(ValidationRuleId.PROFESSIONAL_PUBLICATIONS_OPTIONAL.ToString(), Server.MapPath(ParentModulePath)) Then
                            hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                        Else
                            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                        End If
                    End If
                        messagesTitle = String.Format(messagesTitle, totalCEMinSummary, totalCEMin, totalCEMax)
                    End If
            End If
            If Not ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(ValidationRuleId.PROFESSIONAL_PUBLICATIONS_OPTIONAL.ToString(), Server.MapPath(ParentModulePath)) Then
                lblOptionTitle.Text = messagesTitle
            End If
        End Sub

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
                BuildTitle()
                'Dim issues = Save()
                'If issues IsNot Nothing AndAlso issues.Count > 0 Then
                '    ShowIssueMessages(issues)
                'End If
            End If
        End Sub
#End Region

    End Class
End Namespace
