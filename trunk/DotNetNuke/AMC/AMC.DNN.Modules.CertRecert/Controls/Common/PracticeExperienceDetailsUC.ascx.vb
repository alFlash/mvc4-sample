Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports AMC.DNN.Modules.CertRecert.Controls.Reusable
Imports AMC.DNN.Modules.CertRecert.Business.Controller

Imports TIMSS.API.User.UserDefinedInfo
Imports TIMSS.API.Core.Validation

Namespace Controls.Common
    Public Class PracticeExperienceDetailsUC
        Inherits SectionBaseUserControl

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
                ProgramType = ActivityProgramType.PRACTICEEXP.ToString()
                cEWeightList = amcCertRecertController.GetCertificationCEWeights(OrganizationId, OrganizationUnitId, ProgramType)
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
                Dim results =
                    amcCertRecertController.CommitCustomerExternalCEActivity(
                                                    ProgramType,
                                                    MasterCustomerId,
                                                    SubCustomerId)
                If results Is Nothing OrElse results.Count <= 0 Then
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                End If
                BindingDataToList()

                If rptPracticeExprience.Items Is Nothing OrElse rptPracticeExprience.Items.Count <= 0 Then
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                End If
                Return results
            Catch ex As Exception
                ProcessException(ex)
            End Try
            Return Nothing
        End Function

        Protected Sub rptPracticeExprience_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptPracticeExprience.ItemCommand
            Try
                If e.CommandName.Equals("Delete") Then
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                    Dim customerCEActivityItem As IUserDefinedCustomerCEActivity
                    customerCEActivityItem = amcCertRecertController.GetCustomerExternalCEActivityByGuiId(e.CommandArgument.ToString(), ProgramType, Me.MasterCustomerId, Me.SubCustomerId)
                    amcCertRecertController.DeleteCustomerExternalCEActivity(customerCEActivityItem)
                End If
                BindingDataToList()
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Protected Sub rptPracticeExprience_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptPracticeExprience.ItemDataBound
            Try
                If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
                    Dim userdine As UserDefinedCustomerCEActivity = CType(e.Item.DataItem, UserDefinedCustomerCEActivity)
                    If userdine IsNot Nothing Then
                        '' bind CE Type
                        Dim point As String
                        Dim lblCE As Label = CType(e.Item.FindControl("lblSumaryPoint"), Label)
                        If lblCE IsNot Nothing Then
                            '' calculator point
                           FactorIndex = CommonHelper.GetCEWeight(userdine.CEType.List(userdine.CETypeString).Code, CEWeightList, False,
                                                                       If(GetFieldInfo("Point") IsNot Nothing AndAlso GetFieldInfo("Point").CalculateFormula > 0,
                                                                          GetFieldInfo("Point").CalculateFormula.ToString(), "1"))
                            point = CommonHelper.CalculatorCEItem(userdine.CEHours.ToString(), FactorIndex).ToString()
                            lblCE.Text = CommonHelper.ChopUnusedDecimal(point)
                            totalPoint += Decimal.Parse(point) '' not null, min value is 0
                        End If
                        '' Bind date
                        Dim lblStartDate As Label = CType(e.Item.FindControl("lblStartDate"), Label)
                        If lblStartDate IsNot Nothing Then
                            If userdine.StartDate <> DateTime.MinValue Then
                                lblStartDate.Text = userdine.StartDate.ToString(CommonConstants.DATE_FORMAT)
                            End If
                        End If
                        Dim lblEndDate As Label = CType(e.Item.FindControl("lblEndDate"), Label)
                        If lblEndDate IsNot Nothing Then
                            If userdine.EndDate <> DateTime.MinValue Then
                                lblEndDate.Text = userdine.EndDate.ToString(CommonConstants.DATE_FORMAT)
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
            ProgramType = ActivityProgramType.PRACTICEEXP.ToString()
            BindingDataToList()
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
                    If Not Page.IsPostBack Then
                        hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                    End If
                End If
            End If
            rptPracticeExprience.DataSource = activityExternals
            rptPracticeExprience.DataBind()
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
                    issueCollection = amcCertRecertController.InsertCustomerExternalCEActivity(customerExternalCEActivityItem)
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
                    .Position = Me.txtPosition.Text
                    .StartDate = DateTime.MinValue
                    .EndDate = DateTime.MinValue
                    If CType(Me.txtStartDate, AMCDatetime).Text <> String.Empty Then
                        .StartDate = DateTime.Parse(CType(Me.txtStartDate, AMCDatetime).Text)
                    End If
                    If CType(Me.txtEndDate, AMCDatetime).Text <> String.Empty Then
                        .EndDate = DateTime.Parse(CType(Me.txtEndDate, AMCDatetime).Text)
                    End If
                    .OrganizationProviding = Me.txtOrganization.Text
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

    End Class
End Namespace