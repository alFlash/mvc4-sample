Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Controls.Reusable
Imports AMC.DNN.Modules.CertRecert.Business.BusinessValidation
Imports AMC.DNN.Modules.CertRecert.Business.IControls
Imports TIMSS.API.CustomerInfo
Imports TIMSS.API.Generated.CustomerInfo
Imports TIMSS.API.Core
Imports TIMSS.API.User.UserDefinedInfo
Imports TIMSS.API.Core.Validation

Namespace Controls.Common
    ''' <summary>
    ''' Prepresent GUI for EducationCourseUC tab 
    ''' </summary>
    Public Class EducationCourseUC
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
                ProgramType = ActivityProgramType.CONTEDUCATION.ToString()
                cEWeightList = AMCCertRecertController.GetCertificationCEWeights(OrganizationId, OrganizationUnitId, ProgramType)
                maxCEHoursARN = 0
                Dim otherModuleSettings = ModuleConfigurationHelper.Instance.GetOtherModuleSettings(Server.MapPath(ParentModulePath))
                If otherModuleSettings IsNot Nothing Then
                    maxCEHoursARN = If(otherModuleSettings.EducationCourseTotalHours.HasValue, otherModuleSettings.EducationCourseTotalHours.Value, 0)
                End If
                If CheckConflictValidation(RuleID, ConflictMessage) = True AndAlso Not String.IsNullOrEmpty(ConflictMessage) Then
                    Dim issuesCollection As New IssuesCollection
                    issuesCollection.Add(New CESummaryIssue(New BusinessObject(), ConflictMessage))
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                End If
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
            'Commit data
            Try
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                Dim results = AMCCertRecertController.CommitCustomerExternalCEActivity(ProgramType, MasterCustomerId, SubCustomerId)
                If results Is Nothing OrElse results.Count <= 0 Then
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                End If
                BindingDataToList()
                '' optinal rule will be overwrite another rule
                If ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(ValidationRuleId.ACADEMIC_COURSEWORK_OPTIONAL.ToString(), Server.MapPath(ParentModulePath)) Then
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                Else
                    '' check validation
                    Dim iissueResult As New IssuesCollection
                    Dim errorMessages = String.Empty
                    Dim totalCEHours As Decimal = 0
                    totalCEHours = Decimal.Parse(GetCookie(ProgramType + MasterCustomerId))
                    If Not String.IsNullOrEmpty(ConflictMessage) Then '' validation rule is conflicted
                        iissueResult.Add(New CheckBoxMustSelectedIssue(New BusinessObject(), ConflictMessage))
                        hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                        Return iissueResult
                    End If
                    If RuleID = ValidationRuleId.RECERT_EDU_COURSE_EQUAL_ARN.ToString() Then  '' 
                        If CheckBusinessValidationCommitData_ARN_Equal(errorMessages, MaxCEHoursARN, totalCEHours) Then
                            iissueResult.Add(New CheckBoxMustSelectedIssue(New BusinessObject(), errorMessages))
                            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                            Return iissueResult
                        End If
                    ElseIf RuleID = ValidationRuleId.RECERT_EDU_COURSE_GREATER_ARN.ToString() Then  ''
                        If CheckBusinessValidationCommitData_ARN_AllowGreater(errorMessages, MaxCEHoursARN, totalCEHours) Then
                            iissueResult.Add(New CheckBoxMustSelectedIssue(New BusinessObject(), errorMessages))
                            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                            Return iissueResult
                        End If
                    End If
                    '' End check validation
                    If rpteducationCourse.Items Is Nothing OrElse rpteducationCourse.Items.Count <= 0 Then
                        hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                    End If
                End If
                Return results
            Catch ex As Exception
                ProcessException(ex)
            End Try
            Return Nothing
        End Function

        Protected Sub rpteducationCourse_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rpteducationCourse.ItemCommand
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

        Protected Sub rpteducationCourse_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rpteducationCourse.ItemDataBound
            Try
                If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
                    Dim userdine As UserDefinedCustomerCEActivity = CType(e.Item.DataItem, UserDefinedCustomerCEActivity)
                    If userdine IsNot Nothing Then
                        '' calculator point
                        Dim point As String
                        Dim lblCE As Label = CType(e.Item.FindControl("lblSumaryPoint"), Label)
                        If lblCE IsNot Nothing Then
                            FactorIndex = CommonHelper.GetCEWeight(userdine.CEType.List(userdine.CETypeString).Code, CEWeightList, False, If(GetFieldInfo("Point") IsNot Nothing AndAlso GetFieldInfo("Point").CalculateFormula > 0, GetFieldInfo("Point").CalculateFormula.ToString(), "1"))
                            point = CommonHelper.CalculatorCEItem(userdine.CEHours.ToString(), FactorIndex).ToString()
                            lblCE.Text = CommonHelper.ChopUnusedDecimal(point)
                            TotalPoint += Decimal.Parse(point) '' not null, min value is 0
                        End If
                        ''bind date
                        Dim lblStartDate As Label = CType(e.Item.FindControl("lblStartDate"), Label)
                        If lblStartDate IsNot Nothing Then
                            If userdine.StartDate <> DateTime.MinValue Then
                                lblStartDate.Text = userdine.StartDate.ToString(CommonConstants.DATE_FORMAT)
                            End If
                        End If
                        Dim lbldate As Label = CType(e.Item.FindControl("lblCompletionDate"), Label)
                        If lbldate IsNot Nothing Then
                            If userdine.EndDate <> DateTime.MinValue Then
                                lbldate.Text = userdine.EndDate.ToString(CommonConstants.DATE_FORMAT)
                            End If
                        End If
                        '' bind CEHours
                        Dim lblpoint As Label = CType(e.Item.FindControl("lblPoint"), Label)
                        If lblpoint IsNot Nothing Then
                            lblpoint.Text = CommonHelper.ChopUnusedDecimal(userdine.CEHours.ToString())
                        End If
                        '' bind Degree
                        Dim lblDegree As Label = CType(e.Item.FindControl("lblDegree"), Label)
                        Dim hdnDegreeValue As Label = CType(e.Item.FindControl("lblDegreeValue"), Label)
                        If lblDegree IsNot Nothing AndAlso hdnDegreeValue IsNot Nothing Then
                            Dim arrDegree As String() = userdine.Degree.List(userdine.DegreeString).Code.ToString().Split(","c)
                            If arrDegree IsNot Nothing AndAlso arrDegree.Length > 0 Then
                                Dim degreeName = String.Empty
                                Dim degreValue = String.Empty
                                For i As Integer = 0 To arrDegree.Length - 1
                                    degreeName += userdine.Degree.List(arrDegree(i)).Description + ","
                                    degreValue += arrDegree(i) + ","
                                Next
                                lblDegree.Text = degreeName.TrimEnd(","c)
                                hdnDegreeValue.Text = degreValue.TrimEnd(","c)
                            End If
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
        ''' <param></param>
        Public Overrides Sub ValidateFormFillCompleted()
            Try
                ProgramType = ActivityProgramType.CONTEDUCATION.ToString()
                If CheckConflictValidation(RuleID, ConflictMessage) = True AndAlso Not String.IsNullOrEmpty(ConflictMessage) Then
                    Dim issuesCollection As New IssuesCollection
                    issuesCollection.Add(New CESummaryIssue(New BusinessObject(), ConflictMessage))
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                End If
                maxCEHoursARN = 0
                Dim otherModuleSettings = ModuleConfigurationHelper.Instance.GetOtherModuleSettings(Server.MapPath(ParentModulePath))
                If otherModuleSettings IsNot Nothing Then
                    maxCEHoursARN = If(otherModuleSettings.EducationCourseTotalHours.HasValue, otherModuleSettings.EducationCourseTotalHours.Value, 0)
                End If
                BindDegreeToDropDownList()
                BindingDataToList()
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Private Sub BindDegreeToDropDownList()
            Dim list As UserDefinedCustomerCEActivities = New UserDefinedCustomerCEActivities()
            Dim itemData As IUserDefinedCustomerCEActivity
            itemData = list.CreateNew()
            lstbDegree.Items.Clear()
            With lstbDegree
                .DataSource = itemData.Degree.List
                .DataTextField = "Description"
                .DataValueField = "Code"
                .DataBind()
            End With

        End Sub

        ''' <summary>
        ''' Binding Data to repeater
        ''' </summary>
        ''' <param></param>
        Private Sub BindingDataToList()
            Dim activityExternals As IUserDefinedCustomerCEActivities
            activityExternals = amcCertRecertController.GetExternalActivityList(ProgramType, Me.MasterCustomerId, Me.SubCustomerId)
            If activityExternals IsNot Nothing Then
                If Not Page.IsPostBack Then
                    If activityExternals.Count > 0 Then
                        hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                    Else
                        If ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(ValidationRuleId.ACADEMIC_COURSEWORK_OPTIONAL.ToString(), Server.MapPath(ParentModulePath)) Then
                            hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                        Else
                            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                        End If
                    End If
                End If
            End If
            Me.rpteducationCourse.DataSource = activityExternals
            Me.rpteducationCourse.DataBind()
            lblTotalCE.Text = CommonHelper.ChopUnusedDecimal(TotalPoint.ToString())
            SetCookie(ProgramType + MasterCustomerId, TotalPoint.ToString())
            BuildTitle()
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
                    SetPropertiesForObject(customerExternalCEActivityItem)
                    customerExternalCEActivityItem.IsNewObjectFlag = True
                    issueCollection = amcCertRecertController.InsertCustomerExternalCEActivity(customerExternalCEActivityItem)
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
                    .ProgramTitle = txtCourseTitle.Text
                    .StartDate = DateTime.MinValue
                    If CType(Me.txtStartDate, AMCDatetime).Text <> String.Empty Then
                        .StartDate = DateTime.Parse(CType(Me.txtStartDate, AMCDatetime).Text)
                    End If
                    .EndDate = DateTime.MinValue
                    If CType(Me.txtCompletionDate, AMCDatetime).Text <> String.Empty Then
                        .EndDate = DateTime.Parse(CType(Me.txtCompletionDate, AMCDatetime).Text)
                    End If
                    .DegreeString = GetSelectedDegrees()
                    .OrganizationProviding = Me.txtOrganizationProvidingCourse.Text
                    .OrganizationApproving = Me.txtOrganizationApprovingTheCourse.Text
                    If Me.txtApprovedCE.Text <> String.Empty AndAlso CommonHelper.CheckIsNumber(Me.txtApprovedCE.Text) Then
                        .CEHours = Decimal.Parse(Me.txtApprovedCE.Text)
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
                                                                 ValidationRuleId.RECERT_EDU_COURSE_EQUAL_ARN.ToString(),
                                                                 Server.MapPath(ParentModulePath))
            Dim validationARN_Greater = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                 ValidationRuleId.RECERT_EDU_COURSE_GREATER_ARN.ToString(),
                                                                 Server.MapPath(ParentModulePath))
            conflictMessage = String.Empty
            ruleID = String.Empty
            If validationARN_Equal = False AndAlso validationARN_Greater = False Then '' non validation : return true + don't have conflictMessage
                Return True
            ElseIf validationARN_Greater = True Then
                ruleID = ValidationRuleId.RECERT_EDU_COURSE_GREATER_ARN.ToString()
                Return False
            ElseIf validationARN_Equal = True Then
                ruleID = ValidationRuleId.RECERT_EDU_COURSE_EQUAL_ARN.ToString()
                Return False
            End If
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

        Private Sub BuildTitle()
            lblOptionTitle.Text = String.Empty
            CheckConflictValidation(RuleID, ConflictMessage)
            If RuleID = ValidationRuleId.RECERT_EDU_COURSE_GREATER_ARN.ToString() Then
                BuildTitle_ARN_GreaterMaxHours()
            ElseIf RuleID = ValidationRuleId.RECERT_EDU_COURSE_EQUAL_ARN.ToString() Then
                BuildTitle_ARN_EqualMaxHours()
            End If
        End Sub

        Public Sub BuildTitle_ARN_GreaterMaxHours()
            lblOptionTitle.Text = String.Empty
            Dim errorMessages = String.Empty
            Dim totalCEHours As Decimal = 0
            totalCEHours = Decimal.Parse(GetCookie(ProgramType + MasterCustomerId))
            '' optinal rule will be overwrite another rule
            If ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(ValidationRuleId.ACADEMIC_COURSEWORK_OPTIONAL.ToString(), Server.MapPath(ParentModulePath)) Then
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
            If ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(ValidationRuleId.ACADEMIC_COURSEWORK_OPTIONAL.ToString(), Server.MapPath(ParentModulePath)) Then
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
            End If
        End Sub
#End Region

    End Class
End Namespace