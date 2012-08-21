Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Controls.Reusable
Imports AMC.DNN.Modules.CertRecert.Business.Controller
Imports AMC.DNN.Modules.CertRecert.Business.BusinessValidation
Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports System.ComponentModel
Imports TIMSS.API.Core
Imports TIMSS.API.Generated.CustomerInfo
Imports TIMSS.API.CustomerInfo
Imports TIMSS.API.Core.Validation
Imports System.Linq

Namespace Controls.Common
    ''' <summary>
    ''' Prepresent GUI for EducationUC tab 
    ''' </summary>
    Public Class EducationUC
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
                Page.ClientScript.RegisterClientScriptInclude("educationUC", String.Format("{0}/Documentation/scripts/education.js?v={1}", ModulePath, CommonConstants.CURRENT_VERSION))
                AddEventHandlers()
                If Not Page.IsPostBack Then
                    cvComment.Visible = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                    ValidationRuleId.CERT_EDUCATION_TWO_MONTH_GAP.ToString(),
                                                                    Server.MapPath(ParentModulePath))
                End If

            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Protected Sub btnSaveClick(ByVal sender As Object, ByVal e As EventArgs)
            Try
                SaveEducation()
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
                Dim customerEducations = AMCCertRecertController.GetEducationList(MasterCustomerId,
                                                                                  SubCustomerId)
                If CheckExistFourRows(customerEducations) = False Then
                    iissueResult.Add(New CheckBoxMustSelectedIssue(New BusinessObject(),
                                                                   DotNetNuke.Services.Localization.Localization.GetString("ErrorMessageEducation.Text",
                                                                                                                           Me.LocalResourceFile)))
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                    Return iissueResult
                End If
                If customerEducations IsNot Nothing Then
                    For Each customerEducation As ICustomerEducation In customerEducations
                        UploadFileIssue.Assert(False, customerEducation)
                    Next
                End If
                Dim results = AMCCertRecertController.CommitCustomerEducation(MasterCustomerId, SubCustomerId)
                If results Is Nothing OrElse results.Count <= 0 Then
                    MoveEducationFileToApproriateDirectory()
                    AMCCertRecertController.RefreshCustomerEducation(MasterCustomerId, SubCustomerId)
                End If
                If rptEducation.Items Is Nothing OrElse rptEducation.Items.Count <= 0 Then
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                End If
                BindingDataToList()
                Return results
            Catch ex As Exception
                ProcessException(ex)
            End Try
            Return Nothing
        End Function

        Protected Sub MoveEducationFileToApproriateDirectory()
            Dim customerExternals As ICustomerEducationList
            customerExternals =
            AMCCertRecertController.GetEducationList(Me.MasterCustomerId,
                                                    Me.SubCustomerId)
            If customerExternals IsNot Nothing Then
                For Each customerExternalDocumentationItem As ICustomerEducation In customerExternals
                    MoveFileFromTempToMainDirectory(DocumentationType.EDUCATION_UC.ToString(),
                                                    customerExternalDocumentationItem.Guid,
                                    customerExternalDocumentationItem.CustomerEducationId.ToString(),
                                    String.Empty)
                Next
            End If
        End Sub

        Protected Sub rptEducation_ItemCommand(ByVal source As Object,
                                               ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs
                                               ) Handles rptEducation.ItemCommand
            Try
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                If e.CommandName.Equals("Delete") Then
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                    Dim customerEducationItem As ICustomerEducation
                    customerEducationItem = AMCCertRecertController.GetCustomerEducationByGuiId(e.CommandArgument.ToString(),
                                                                                                Me.MasterCustomerId, Me.SubCustomerId)
                    If customerEducationItem IsNot Nothing Then
                        AMCCertRecertController.DeleteCustomerEducation(customerEducationItem)
                    End If
                End If
                BindingDataToList()
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Protected Sub rptEducation_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptEducation.ItemDataBound
            Try
                Dim fileName As String = String.Empty
                Dim linkLocation As String = String.Empty
                Dim customerExternalDocument As ICustomerEducation =
                                        CType(e.Item.DataItem, ICustomerEducation)

                If customerExternalDocument IsNot Nothing Then
                    fileName = GetFileNameOfDocument(DocumentationType.EDUCATION_UC.ToString(),
                                                        customerExternalDocument.Guid.ToString(),
                                                        customerExternalDocument.CustomerEducationId.ToString(),
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

                Dim userdine As ICustomerEducation = CType(e.Item.DataItem, ICustomerEducation)
                If userdine IsNot Nothing Then
                    '' bind Degree
                    Dim lblDegree As Label = CType(e.Item.FindControl("lblDegree"), Label)
                    Dim hdnDegreeValue As Label = CType(e.Item.FindControl("lblDegreeValue"), Label)
                    If lblDegree IsNot Nothing AndAlso hdnDegreeValue IsNot Nothing Then
                        For i As Integer = 0 To userdine.ProgDegreeCode.List.Count - 1
                            If userdine.ProgDegreeCode.List(i).Code = userdine.ProgDegreeCode.Code Then
                                lblDegree.Text = userdine.ProgDegreeCode.List(i).Description
                                hdnDegreeValue.Text = userdine.ProgDegreeCode.List(i).Code
                                Exit For
                            End If
                        Next
                    End If
                    '' Bind ProgramType
                    Dim lblProgType As Label = CType(e.Item.FindControl("lblProgType"), Label)
                    Dim lblProramTypevalues As Label = CType(e.Item.FindControl("lblProgTypevalues"), Label)
                    If lblProgType IsNot Nothing AndAlso lblProramTypevalues IsNot Nothing Then
                        For i As Integer = 0 To userdine.ProgTypeCode.List.Count - 1
                            If userdine.ProgTypeCode.List(i).Code = userdine.ProgTypeCode.Code Then
                                lblProgType.Text = userdine.ProgTypeCode.List(i).Description
                                lblProramTypevalues.Text = userdine.ProgTypeCode.List(i).Code
                                Exit For
                            End If
                        Next
                    End If
                    '' bind Date
                    Dim lblDate As Label = CType(e.Item.FindControl("lblDate"), Label)
                    Dim lblEndDate As Label = CType(e.Item.FindControl("lblEndDate"), Label)
                    If lblDate IsNot Nothing Then
                        If userdine.BeginDate <> DateTime.MinValue Then
                            lblDate.Text = userdine.BeginDate.ToString(CommonConstants.DATE_FORMAT)
                        End If
                    End If
                    If lblEndDate IsNot Nothing Then
                        If userdine.EndDate <> DateTime.MinValue Then
                            lblEndDate.Text = userdine.EndDate.ToString(CommonConstants.DATE_FORMAT)
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
            If Not Page.IsPostBack Then
                BindDegree(ddlDegree)
                BindProgramType(ddlProgType)
            End If
            BindingDataToList()
        End Sub

        ''' <summary>
        ''' Binding Data to repeater
        ''' </summary>
        ''' <param></param>
        Private Sub BindingDataToList()
            Dim customerEducationList As ICustomerEducationList
            customerEducationList = AMCCertRecertController.GetEducationList(
                                                                    Me.MasterCustomerId,
                                                                    Me.SubCustomerId)
            If customerEducationList IsNot Nothing Then
                If customerEducationList.Count > 0 Then ''AndAlso Not Page.IsPostBack
                    If CheckExistFourRows(customerEducationList) Then
                        hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                    End If
                End If
            End If
            customerEducationList.Sort("BeginDate", ListSortDirection.Descending)
            Me.rptEducation.DataSource = customerEducationList
            Me.rptEducation.DataBind()
        End Sub

        Private Function CheckExistFourRows(ByVal customerEducationList As ICustomerEducationList) As Boolean
            Dim checkFourRows = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                    ValidationRuleId.CERT_EDU_ROW_REQUIRED.ToString(),
                                                                    Server.MapPath(ParentModulePath))
            If checkFourRows = False Then '' rule isn't require
                Return True
            End If
            If customerEducationList Is Nothing Then
                Return False
            Else
                If customerEducationList.Count < 4 Then
                    Return False
                Else
                    Dim arrProgramType() As String = {"FELLOWSHIP", "MASTER", "RESIDENCY", "UNDERGRADUATE"}
                    Dim index As Integer = 0
                    For Each objectItem As CustomerEducation In customerEducationList
                        If CheckExistItem(objectItem.ProgTypeCodeString, arrProgramType) Then
                            index += 1
                        End If
                        If index = 4 Then
                            Return True
                        End If
                    Next
                End If
            End If
            Return False
        End Function

        Private Function CheckExistItem(ByVal strItem As String, ByVal strArray As String()) As Boolean
            For i As Integer = 0 To strArray.Length - 1
                If strArray(i) = strItem Then
                    strArray(i) = strItem + "check"
                    Return True
                End If
            Next
            Return False
        End Function


        ''' <summary>
        ''' Insert or Edit Item to list Cache
        ''' </summary>
        ''' <param></param>
        Private Sub SaveEducation()
            Dim customerEducationList As New CustomerEducationList
            Dim customerEducationItem As ICustomerEducation
            Dim issueCollection As New IssuesCollection
            Dim educationIssue As IIssuesCollection
            ''edit 
            If (Me.hdCurrentObjectUniqueId.Value <> String.Empty) Then
                customerEducationItem = AMCCertRecertController.GetCustomerEducationByGuiId(
                                                                            Me.hdCurrentObjectUniqueId.Value,
                                                                            Me.MasterCustomerId,
                                                                            Me.SubCustomerId)
                If customerEducationItem IsNot Nothing Then
                    If SetPropertiesForObject(customerEducationItem) Then
                        educationIssue = AMCCertRecertController.UpdateCustomerEducation(customerEducationItem)
                    Else
                        educationIssue = customerEducationItem.ValidationIssuesForMe
                    End If
                End If
            Else
                ''Insert
                customerEducationItem = customerEducationList.CreateNew()
                customerEducationItem.IsNewObjectFlag = True
                If SetPropertiesForObject(customerEducationItem) Then
                    educationIssue = AMCCertRecertController.InsertCustomerEducation(customerEducationItem)
                Else
                    educationIssue = customerEducationItem.ValidationIssuesForMe
                End If
            End If
            If educationIssue IsNot Nothing Then
                For Each issue As IIssue In educationIssue 
                    issueCollection.Add(issue)
                Next
            End If
            If (issueCollection Is Nothing OrElse issueCollection.Count < 1) AndAlso
                customerEducationItem IsNot Nothing Then
                If Me.hfDeleteFile.Value.Equals("YES") Then ''Delete file
                    DeleteAttachDocument(DocumentationType.EDUCATION_UC.ToString(),
                                            customerEducationItem.Guid,
                                            customerEducationItem.CustomerEducationId.ToString())
                End If
                If fuUploadFileAttachment.FileContent.Length > 0 Then ''upload file
                    Dim fileLocation As String = String.Empty
                    fileLocation = UploadTempFile(Me.fuUploadFileAttachment,
                                    DocumentationType.EDUCATION_UC.ToString(),
                                    customerEducationItem.Guid)
                End If
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
        End Sub

        ''' <summary>
        ''' Set property from GUI for Item Object
        ''' </summary>
        ''' <param name="customerEducationItem"> Item Object need to set values </param>
        Private Function SetPropertiesForObject(ByRef customerEducationItem As ICustomerEducation) As Boolean
            Dim isIssue As Boolean = True
            If customerEducationItem IsNot Nothing Then
                customerEducationItem.ValidationIssuesForMe.RemoveAll()
                UploadFileIssue.Assert(False, customerEducationItem)
                With customerEducationItem
                    .MasterCustomerId = Me.MasterCustomerId
                    .SubCustomerId = Me.SubCustomerId
                    .InstitutionName = txtInstitudeName.Text
                    .ProgTypeCodeString = ddlProgType.SelectedValue
                    If CType(Me.txtDate, AMCDatetime).Text <> String.Empty Then
                        .BeginDate = DateTime.Parse(CType(Me.txtDate, AMCDatetime).Text)
                    End If
                    If CType(Me.txtEndDate, AMCDatetime).Text <> String.Empty Then
                        .EndDate = DateTime.Parse(CType(Me.txtEndDate, AMCDatetime).Text)
                    End If
                    .Comments = Me.txtComment.Value
                    .ProgDegreeCodeString = ddlDegree.SelectedValue
                End With
                UploadFileIssue.Assert(UploadFileIssue.IsNotPdfFile(fuUploadFileAttachment.FileContent),
                                          customerEducationItem)
                For Each issue As IIssue In customerEducationItem.ValidationIssues
                    If TypeOf issue Is UploadFileIssue Then
                        isIssue = False
                        Exit For
                    End If
                Next
            End If
            Return isIssue
        End Function

        Private Sub AddEventHandlers()
            AddHandler btnSave.Click, AddressOf btnSaveClick
        End Sub

        Private Sub BindDegree(ByVal ddlName As DropDownList)
            Dim list As CustomerEducationList = New CustomerEducationList()
            Dim itemData As ICustomerEducation
            itemData = list.CreateNew()
            ddlName.DataTextField = "Description"
            ddlName.DataValueField = "Code"
            ddlName.DataSource = itemData.ProgDegreeCode.List
            ddlName.DataBind()
        End Sub

        Private Sub BindProgramType(ByVal ddlName As DropDownList)
            Dim listTemp As CustomerEducationList = New CustomerEducationList()
            Dim itemData As ICustomerEducation
            itemData = listTemp.CreateNew()
            ddlName.DataTextField = "Description"
            ddlName.DataValueField = "Code"
            ddlName.DataSource = itemData.ProgTypeCode.List
            ddlName.DataBind()
        End Sub

#End Region

    End Class
End Namespace