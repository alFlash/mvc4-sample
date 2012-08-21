Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports AMC.DNN.Modules.CertRecert.Controls.Reusable
Imports AMC.DNN.Modules.CertRecert.Business.Controller
Imports TIMSS.API.User.UserDefinedInfo
Imports TIMSS.API.Core.Validation

Namespace Controls.Common
    ''' <summary>
    ''' Prepresent GUI for CommunityServiceReview tab 
    ''' </summary>
    Public Class CommunityServiceReview
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
                ProgramType = ActivityProgramType.COMMSERVR.ToString()
                CEWeightList = AMCCertRecertController.GetCertificationCEWeights(OrganizationId, OrganizationUnitId, ProgramType)
                '' set default value for point (ARN)
                Dim setdefaultPointARNFormE = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                               ValidationRuleId.RECERT_SET_DEFAULT_FORM_E_ARN.ToString(),
                                                               Server.MapPath(ParentModulePath))
                If setdefaultPointARNFormE = True Then '' if checking rule in control panel
                    Dim hdnARN As HiddenField = CType(FindControl("hdnARN"), HiddenField)
                    Dim hdnPointValuesDefault As HiddenField = CType(FindControl("hdnPointValuesDefault"), HiddenField)
                    If hdnARN IsNot Nothing AndAlso hdnPointValuesDefault IsNot Nothing AndAlso GetFieldInfo("Point") IsNot Nothing Then
                        hdnARN.Value = "true"
                        hdnPointValuesDefault.Value = CommonHelper.ChopUnusedDecimal(GetFieldInfo("Point").CalculateFormula.ToString())
                    End If
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Protected Sub rptCommunityReview_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptCommunityReview.ItemCommand
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

        Protected Sub rptCommunityReview_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptCommunityReview.ItemDataBound
            Try
                If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
                    Dim userdine As UserDefinedCustomerCEActivity = CType(e.Item.DataItem, UserDefinedCustomerCEActivity)
                    If userdine IsNot Nothing Then
                        Dim point As String
                        Dim lblCE As Label = CType(e.Item.FindControl("lblSumaryPoint"), Label)
                        If lblCE IsNot Nothing Then
                            '' calculator point
                            FactorIndex = CommonHelper.GetCEWeight(userdine.CEType.List(userdine.CETypeString).Code, CEWeightList, False,
                                                                       If(GetFieldInfo("Point") IsNot Nothing AndAlso GetFieldInfo("Point").CalculateFormula > 0,
                                                                          GetFieldInfo("Point").CalculateFormula.ToString(), "1"))
                            Dim setdefaultPointARNFormE = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                              ValidationRuleId.RECERT_SET_DEFAULT_FORM_E_ARN.ToString(),
                                                              Server.MapPath(ParentModulePath))
                            If setdefaultPointARNFormE = True Then
                                point = FactorIndex '' automatic set point value is configed in control panel
                            Else
                                point = CommonHelper.CalculatorCEItem(userdine.CEHours.ToString(), FactorIndex).ToString()
                            End If
                            lblCE.Text = CommonHelper.ChopUnusedDecimal(point)
                            TotalPoint += Decimal.Parse(point) '' not null, min value is 0
                        End If
                        ''bind date
                        Dim lbldate As Label = CType(e.Item.FindControl("lblTermOfOfficeStartDate"), Label)
                        If lbldate IsNot Nothing Then
                            If userdine.StartDate <> DateTime.MinValue Then
                                lbldate.Text = userdine.StartDate.ToString(CommonConstants.DATE_FORMAT)
                            End If
                        End If
                        Dim lblEnddate As Label = CType(e.Item.FindControl("lblTermOfOfficeEnDate"), Label)
                        If lblEnddate IsNot Nothing Then
                            If userdine.EndDate <> DateTime.MinValue Then
                                lblEnddate.Text = userdine.EndDate.ToString(CommonConstants.DATE_FORMAT)
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

        Protected Sub btnSaveClick(ByVal sender As Object, ByVal e As EventArgs)
            Try
                SaveCEActivity()
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Saves form data
        ''' </summary>
        Public Overrides Function Save() As IIssuesCollection
            ''Commit data
            Try
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                Dim results = amcCertRecertController.CommitCustomerExternalCEActivity(ProgramType, MasterCustomerId, SubCustomerId)
                If results Is Nothing OrElse results.Count <= 0 Then
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                End If
                BindingDataToList()
                Return results
            Catch ex As Exception
                ProcessException(ex)
            End Try
            Return Nothing
        End Function
#End Region

#Region "Private Methods"
        ''' <summary>
        ''' set value for hdIsIncomplete , check vulues has incorrect or not when page loads
        ''' </summary>
        ''' <param></param>
        Public Overrides Sub ValidateFormFillCompleted()
            Try
                ProgramType = ActivityProgramType.COMMSERVR.ToString()
                BindingDataToList()
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Binding Data to repeater
        ''' </summary>
        ''' <param></param>
        Private Sub BindingDataToList()
            Dim activityExternals As IUserDefinedCustomerCEActivities
            activityExternals = amcCertRecertController.GetExternalActivityList(ProgramType, Me.MasterCustomerId, Me.SubCustomerId)
            If activityExternals IsNot Nothing Then
                If activityExternals.Count > 0 Then
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                Else
                    If ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                         ValidationRuleId.COMMUNITY_SERVICE_REVIEW_OPTIONAL.ToString(),
                                                         Server.MapPath(ParentModulePath)) Then
                        hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                    Else
                        hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                    End If
                End If
            End If
            Me.rptCommunityReview.DataSource = activityExternals
            Me.rptCommunityReview.DataBind()
            lblTotalCE.Text = CommonHelper.ChopUnusedDecimal(TotalPoint.ToString())
            SetCookie(ProgramType + MasterCustomerId, TotalPoint.ToString())
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
                    customerExternalCEActivityItem = amcCertRecertController.GetCustomerExternalCEActivityByGuiId(Me.hdCurrentObjectUniqueId.Value, ProgramType, Me.MasterCustomerId, Me.SubCustomerId)
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
                If issueCollection IsNot Nothing AndAlso issueCollection.Count > 0 Then
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
                    .MasterCustomerId = MasterCustomerId
                    .SubcustomerId = SubCustomerId
                    .ProgramTypeString = ProgramType
                    .OrganizationProviding = txtNameOfOrganization.Text
                    .Role = txtRole.Text
                    .StartDate = DateTime.MinValue
                    If CType(txtDate, AMCDatetime).Text <> String.Empty Then
                        .StartDate = DateTime.Parse(CType(txtDate, AMCDatetime).Text)
                    End If
                    .EndDate = DateTime.MinValue
                    If CType(txtEndDate, AMCDatetime).Text <> String.Empty Then
                        .EndDate = DateTime.Parse(CType(txtEndDate, AMCDatetime).Text)
                    End If
                    If txtPoint.Text <> String.Empty AndAlso CommonHelper.CheckIsNumber(txtPoint.Text) Then
                        .CEHours = Decimal.Parse(txtPoint.Text)
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
    End Class
End Namespace