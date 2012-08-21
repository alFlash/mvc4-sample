Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Controls.Reusable
Imports AMC.DNN.Modules.CertRecert.Business.Controller
Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports System.ComponentModel
Imports TIMSS.API.User.UserDefinedInfo
Imports TIMSS.API.Core.Validation

Namespace Controls.Common
    Public Class ProfessionalPracticeExperienceUC
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
                If Not Page.IsPostBack Then
                    cvComment.Visible = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                    ValidationRuleId.CERT_PRO_PRACTICE_EXP_TWO_MONTH_GAP.ToString(),
                                                    Server.MapPath(ParentModulePath))
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
            ''Commit data
            Try
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                Dim results = amcCertRecertController.CommitCustomerExternalCEActivity(ProgramType, MasterCustomerId, SubCustomerId)
                If results Is Nothing OrElse results.Count <= 0 Then
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                    Dim pracExperience = AMCCertRecertController.GetExternalActivityList(ProgramType, MasterCustomerId, SubCustomerId)
                    For Each pracItem As UserDefinedCustomerCEActivity In pracExperience
                        MoveFileFromTempToMainDirectory(DocumentationType.PRO_PRAC_EXPERIENCE.ToString(),
                                                        pracItem.Guid,
                                                        pracItem.CEActivityId.ToString(),
                                                        String.Empty)
                    Next
                End If
                BindingDataToList()
                If rptexperience.Items Is Nothing OrElse rptexperience.Items.Count <= 0 Then
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                End If
                Return results
            Catch ex As Exception
                ProcessException(ex)
            End Try
            Return Nothing
        End Function

        ''' <summary>
        ''' Bind CE Point of row into list
        ''' </summary>
        Protected Sub rptexperience_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptexperience.ItemCommand
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
            If activityExternals.Count > 0 AndAlso Not Page.IsPostBack Then
                hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED

            End If
            activityExternals.Sort("StartDate", ListSortDirection.Descending)
            rptexperience.DataSource = activityExternals
            rptexperience.DataBind()
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
                    If Me.hfDeleteFile.Value.Equals("YES") Then ''Delete file
                        DeleteAttachDocument(DocumentationType.PRO_PRAC_EXPERIENCE.ToString(),
                                             customerExternalCEActivityItem.Guid,
                                             customerExternalCEActivityItem.CEActivityId.ToString())
                    End If
                    If fuUploadFileAttachment.FileContent.Length > 0 Then ''upload file
                        Dim fileLocation As String = String.Empty
                        fileLocation = UploadTempFile(Me.fuUploadFileAttachment,
                                                      DocumentationType.PRO_PRAC_EXPERIENCE.ToString(),
                                                      customerExternalCEActivityItem.Guid)
                    End If
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
                    .StartDate = DateTime.MinValue
                    .EndDate = DateTime.MinValue
                    If CType(Me.txtStartDate, AMCDatetime).Text <> String.Empty Then
                        .StartDate = DateTime.Parse(CType(Me.txtStartDate, AMCDatetime).Text)
                    End If
                    If CType(Me.txtEndDate, AMCDatetime).Text <> String.Empty Then
                        .EndDate = DateTime.Parse(CType(Me.txtEndDate, AMCDatetime).Text)
                    End If
                    .Position = txtPosition.Text
                    .OrganizationProviding = txtInstitutionName.Text
                    .Comments = txtComment.Value
                    .ProgramTypeString = ProgramType
                End With
            End If
        End Sub

        Private Sub AddEventHandlers()
            AddHandler btnSave.Click, AddressOf btnSaveClick
        End Sub
#End Region

        Protected Sub rptexperience_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptexperience.ItemDataBound
            Try
                If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
                    Dim userdine As UserDefinedCustomerCEActivity = CType(e.Item.DataItem, UserDefinedCustomerCEActivity)
                    
                    If userdine IsNot Nothing Then
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
                    End If
                    ''upload file document
                    Dim linkLocation As String = String.Empty
                    Dim fileName = GetFileNameOfDocument(DocumentationType.PRO_PRAC_EXPERIENCE.ToString(),
                                                         userdine.Guid.ToString(),
                                                         userdine.CEActivityId.ToString(),
                                                         linkLocation)

                    If fileName <> String.Empty Then
                        Dim hlAttachedDocumentName =
                            CType(e.Item.FindControl("hlAttachedDocumentName"), HyperLink)
                        hlAttachedDocumentName.Text = fileName
                        hlAttachedDocumentName.NavigateUrl = linkLocation
                        Dim hdAttachedDocumentName =
                            CType(e.Item.FindControl("hdAttachedDocumentName"), HiddenField)
                        hdAttachedDocumentName.Value = fileName
                    End If
                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub
    End Class
End Namespace
