Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports AMC.DNN.Modules.CertRecert.Business.BusinessValidation
Imports AMC.DNN.Modules.CertRecert.Business.IControls

Imports TIMSS.API.User.UserDefinedInfo
Imports TIMSS.API.Core.Validation

Namespace Controls.Common
    ''' <summary>
    ''' Prepresent GUI for Licensure tab 
    ''' </summary>
    Public Class Licensure
        Inherits SectionBaseUserControl
        Implements ISave, IReload

#Region "Private Member"

#End Region

#Region "Event Handler"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            Try
                cpWithToday.ValueToCompare = DateTime.Now.ToString(CommonConstants.DATE_FORMAT)
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Protected Sub BtnOKClick(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click

            Dim customerExternals As New UserDefinedCustomerExternalDocumentations
            Dim customerExternalDocumentItem As IUserDefinedCustomerExternalDocumentation
            Dim issueCollection As IIssuesCollection
            Try
                If (hdCurrentObjectUniqueId.Value <> String.Empty) Then
                    customerExternalDocumentItem =
                        AMCCertRecertController.GetCustomerExternalDocumentationByGUID(hdCurrentObjectUniqueId.Value,
                                                                                       DocumentationType.LICENSURE.ToString(), MasterCustomerId, SubCustomerId)
                    If SetPropertiesForObject(customerExternalDocumentItem) Then
                        issueCollection = AMCCertRecertController.UpdateCustomerExternalDocument(customerExternalDocumentItem)
                    Else
                        issueCollection = customerExternalDocumentItem.ValidationIssues
                    End If

                Else
                    ''Insert
                    customerExternalDocumentItem = customerExternals.CreateNew()
                    customerExternalDocumentItem.DocumentationTypeString =
                                            DocumentationType.LICENSURE.ToString()
                    customerExternalDocumentItem.IsNewObjectFlag = True
                    customerExternalDocumentItem.IssuingBody.FillList()
                    ''set properties
                    If SetPropertiesForObject(customerExternalDocumentItem) Then
                        issueCollection = AMCCertRecertController.InsertCustomerExternalDocument(customerExternalDocumentItem)
                    Else
                        issueCollection = customerExternalDocumentItem.ValidationIssues
                    End If
                End If

                If (issueCollection Is Nothing OrElse issueCollection.Count < 1) AndAlso
                    customerExternalDocumentItem IsNot Nothing Then
                    If hfDeleteFile.Value.Equals("YES") Then ''Delete file
                        DeleteAttachDocument(DocumentationType.LICENSURE.ToString(),
                                             customerExternalDocumentItem.Guid,
                                             customerExternalDocumentItem.DocumentationId.ToString())
                    End If
                    If fuUploadFileAttachment.FileContent.Length > 0 Then ''upload file
                        Dim fileLocation As String
                        fileLocation = UploadTempFile(fuUploadFileAttachment,
                                                      customerExternalDocumentItem.DocumentationTypeString,
                                                      customerExternalDocumentItem.Guid)
                        If fileLocation <> String.Empty Then
                            customerExternalDocumentItem.DocumentLocation = fileLocation
                            customerExternalDocumentItem.DocumentTitle = fuUploadFileAttachment.FileName
                            issueCollection =
                                    CType(AMCCertRecertController.UpdateCustomerExternalDocument(customerExternalDocumentItem), IssuesCollection)
                        End If
                    End If
                End If

                If issueCollection.Count > 0 Then
                    ShowError(issueCollection, lblPopupMessage)
                    hdIsValidateFailed.Value = CommonConstants.TAB_INCOMPLETED
                Else
                    hdIsValidateFailed.Value = CommonConstants.TAB_COMPLETED
                End If
                BindingDataToList(False)
            Catch ex As Exception
                ProcessException(ex)
            End Try
            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
        End Sub

        Protected Sub RptLicensureItemDataBound(ByVal sender As Object, ByVal e As Web.UI.WebControls.RepeaterItemEventArgs) Handles rptLicensure.ItemDataBound
            Try
                If e.Item.ItemType = ListItemType.Item OrElse
                                e.Item.ItemType = ListItemType.AlternatingItem Then
                    Dim fileName As String
                    Dim linkLocation As String = String.Empty
                    Dim customerExternalDocument As UserDefinedCustomerExternalDocumentation =
                                            CType(e.Item.DataItem, UserDefinedCustomerExternalDocumentation)

                    If customerExternalDocument IsNot Nothing Then

                        'Build unique row id
                        Dim currentRow = e.Item.FindControl("item_")
                        If currentRow IsNot Nothing AndAlso TypeOf currentRow Is HtmlTableRow Then
                            CType(currentRow, HtmlTableRow).Attributes.Add("guid", customerExternalDocument.Guid)
                        End If

                        fileName = GetFileNameOfDocument(customerExternalDocument.DocumentationTypeString,
                                                         customerExternalDocument.Guid.ToString(),
                                                         customerExternalDocument.DocumentationId.ToString(),
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

                        Dim stateProvinceLabel = CType(e.Item.FindControl("lblStateProvince"), Label)
                        stateProvinceLabel.Text =
                            customerExternalDocument.IssuingBody.List(
                                customerExternalDocument.IssuingBodyString).Description


                        ''set date
                        Dim originalIssueDateLabel = CType(e.Item.FindControl("lblDateOfOriginalIssue"), Label)
                        If customerExternalDocument.InitialIssueDate <> DateTime.MinValue Then
                            originalIssueDateLabel.Text = customerExternalDocument.InitialIssueDate.ToString(CommonConstants.DATE_FORMAT)
                        End If
                        Dim expirationDateLabel = CType(e.Item.FindControl("lblExpirationDate"), Label)
                        If customerExternalDocument.CycleEndDate <> DateTime.MinValue Then
                            expirationDateLabel.Text = customerExternalDocument.CycleEndDate.ToString(CommonConstants.DATE_FORMAT)
                        End If
                    End If
                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Protected Sub RptLicensureItemCommand(ByVal source As Object, ByVal e As Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptLicensure.ItemCommand
            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
            Try
                If e.CommandName.Equals("Delete") Then
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                    Dim customerExternalDocumentItem As IUserDefinedCustomerExternalDocumentation
                    customerExternalDocumentItem =
                        AMCCertRecertController.GetCustomerExternalDocumentationByGUID(
                                                                e.CommandArgument.ToString(),
                                                                DocumentationType.LICENSURE.ToString(), MasterCustomerId, SubCustomerId)
                    AMCCertRecertController.DeleteCustomerExternalDocument(customerExternalDocumentItem)
                End If
                BindingDataToList(False)
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Public Overrides Function Save() As IIssuesCollection
            Dim results As IIssuesCollection = Nothing
            Try
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                Dim customerExternalDocuments = AMCCertRecertController.GetCustomerExternalDocuments(
                                                            DocumentationType.LICENSURE.ToString(),
                                                            MasterCustomerId,
                                                            SubCustomerId)
                If customerExternalDocuments IsNot Nothing Then
                    For Each customerExternalDocument As IUserDefinedCustomerExternalDocumentation In customerExternalDocuments
                        UploadFileIssue.Assert(False, customerExternalDocument)
                    Next
                End If

                results =
                    AMCCertRecertController.CommitCustomerExternalDocuments(
                                                            DocumentationType.LICENSURE.ToString(),
                                                            MasterCustomerId,
                                                            SubCustomerId)

                If results Is Nothing OrElse results.Count <= 0 Then
                    MoveFileToApproriateDirectory(DocumentationType.LICENSURE.ToString())
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                    AMCCertRecertController.RefreshCustomerExternalDocuments(DocumentationType.LICENSURE.ToString(),
                                                                             MasterCustomerId,
                                                                             SubCustomerId)
                End If

                BindingDataToList(True)
                If rptLicensure.Items Is Nothing OrElse rptLicensure.Items.Count <= 0 Then
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
            Return results
        End Function
#End Region

#Region "Private Method"

        ''' <summary>
        ''' set value for hdIsIncomplete , check vulues has incorrect or not when page loads
        ''' </summary>
        ''' <param></param>
        Public Overrides Sub ValidateFormFillCompleted()
            If Not Page.IsPostBack Then
                BindingDataToList(True)
                CommonHelper.BindIssuingBodyType(ddlState, DocumentationType.LICENSURE.ToString())
            Else
                BindingDataToList(False)
            End If
        End Sub

        Private Sub BindingDataToList(ByVal checkTabCompleted As Boolean)
            Dim customerExternals As IUserDefinedCustomerExternalDocumentations
            customerExternals =
                AMCCertRecertController.GetCustomerExternalDocuments(
                                                        DocumentationType.LICENSURE.ToString(), MasterCustomerId, SubCustomerId)
            If customerExternals IsNot Nothing AndAlso checkTabCompleted Then
                If customerExternals.Count > 0 Then
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                Else
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                End If
            End If
            rptLicensure.DataSource = customerExternals
            rptLicensure.DataBind()
        End Sub

        Private Function SetPropertiesForObject(ByRef customerExternalDocumentItem As IUserDefinedCustomerExternalDocumentation) As Boolean
            If customerExternalDocumentItem Is Nothing Then
                Return False
            End If
            Dim isIssue As Boolean = True
            UploadFileIssue.Assert(False, customerExternalDocumentItem)
            With customerExternalDocumentItem
                .RelatedMasterCustomerId = MasterCustomerId
                .RelatedSubcustomerId = SubCustomerId
                .IssuingBodyString = ddlState.SelectedValue
                .IssuedNumber = txtLicenseNumber.Text()
                If txtExpirationDate.Text <> String.Empty Then
                    .CycleEndDate = DateTime.Parse(txtExpirationDate.Text)
                Else
                    .CycleEndDate = DateTime.MinValue
                End If
                If txtDateOfOriginalIssue.Text <> String.Empty Then
                    .InitialIssueDate =
                        DateTime.Parse(txtDateOfOriginalIssue.Text)
                Else
                    .InitialIssueDate = DateTime.MinValue
                End If
            End With
            If fuUploadFileAttachment.FileContent.Length > 1 Then
                UploadFileIssue.Assert(UploadFileIssue.IsNotPdfFile(fuUploadFileAttachment.FileContent),
                                       customerExternalDocumentItem)
            End If
            For Each issue As IIssue In customerExternalDocumentItem.ValidationIssues
                If TypeOf issue Is UploadFileIssue Then
                    isIssue = False
                    Exit For
                End If
            Next
            Return isIssue
        End Function

#End Region

        Public ReadOnly Property SaveControls() As List(Of String) Implements IReload.SaveControls
            Get
                Dim result = New List(Of String)
                result.Add("RecertEligibilityUC")
                Return result
            End Get
        End Property

        Public Sub Reload(ByVal saveControl As String) Implements IReload.Reload
            If SaveControls.Contains(saveControl) Then
                BindingDataToList(False)
            End If
        End Sub
    End Class

End Namespace

