Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Controls.Reusable
Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports AMC.DNN.Modules.CertRecert.Business.BusinessValidation
Imports AMC.DNN.Modules.CertRecert.Business.Controller

Imports TIMSS.API.User.UserDefinedInfo
Imports TIMSS.API.Core.Validation

Namespace Controls.Common
    Public Class ControlSubstanceAuthorizationUC
        Inherits SectionBaseUserControl

#Region "Private Member"
        Private issuesCollection As IIssuesCollection
#End Region

#Region "Event Handle"
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                Me.cpWithToday.ValueToCompare = DateTime.Now.ToString(CommonConstants.DATE_FORMAT)
            Catch ex As System.Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Public Overrides Function Save() As IIssuesCollection
            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
            Try
                Dim customerExternalDocuments = amcCertRecertController.GetCustomerExternalDocuments(DocumentationType.CSAI.ToString(),
                                                                                                     MasterCustomerId,
                                                                                                     SubCustomerId)
                If customerExternalDocuments IsNot Nothing Then
                    For Each customerExternalDocument As IUserDefinedCustomerExternalDocumentation In customerExternalDocuments
                        UploadFileIssue.Assert(False, customerExternalDocument)
                    Next
                End If

                issuesCollection = amcCertRecertController.CommitCustomerExternalDocuments(DocumentationType.CSAI.ToString(),
                                                                                           MasterCustomerId,
                                                                                           SubCustomerId)

                If issuesCollection Is Nothing OrElse issuesCollection.Count <= 0 Then
                    MoveFileToApproriateDirectory(DocumentationType.CSAI.ToString())
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                    AMCCertRecertController.RefreshCustomerExternalDocuments(DocumentationType.CSAI.ToString(),
                                                                             MasterCustomerId,
                                                                             SubCustomerId)
                End If
                BindingDataToList()
                If rptControlSubstance.Items Is Nothing OrElse rptControlSubstance.Items.Count <= 0 Then
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try

            Return issuesCollection
        End Function

        Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
            Dim customerExternals As New UserDefinedCustomerExternalDocumentations
            Dim customerExternalDocumentItem As IUserDefinedCustomerExternalDocumentation
            Try
                ''edit 
                If (Me.hdCurrentObjectUniqueId.Value <> String.Empty) Then
                    customerExternalDocumentItem =
                                AMCCertRecertController.GetCustomerExternalDocumentationByGUID(Me.hdCurrentObjectUniqueId.Value,
                                                                                               DocumentationType.CSAI.ToString(),
                                                                                               Me.MasterCustomerId,
                                                                                               Me.SubCustomerId)
                    If customerExternalDocumentItem IsNot Nothing Then
                        If SetPropertiesForObject(customerExternalDocumentItem) Then
                            issuesCollection = AMCCertRecertController.UpdateCustomerExternalDocument(customerExternalDocumentItem)
                        Else
                            issuesCollection = customerExternalDocumentItem.ValidationIssues
                        End If
                    End If
                Else
                    ''Insert
                    customerExternalDocumentItem = customerExternals.CreateNew()
                    customerExternalDocumentItem.DocumentationTypeString =
                                       DocumentationType.CSAI.ToString()
                    customerExternalDocumentItem.IsNewObjectFlag = True
                    customerExternalDocumentItem.IssuingBody.FillList()
                    If SetPropertiesForObject(customerExternalDocumentItem) Then
                        issuesCollection = amcCertRecertController.InsertCustomerExternalDocument(customerExternalDocumentItem)
                    Else
                        issuesCollection = customerExternalDocumentItem.ValidationIssues
                    End If

                End If

                If (issuesCollection Is Nothing Or issuesCollection.Count < 1) AndAlso
                    customerExternalDocumentItem IsNot Nothing Then
                    If Me.hfDeleteFile.Value.Equals("YES") Then ''Delete file
                        DeleteAttachDocument(DocumentationType.CSAI.ToString(),
                                                customerExternalDocumentItem.Guid,
                                                customerExternalDocumentItem.DocumentationId.ToString())
                    End If
                    If fuUploadFileAttachment.FileContent.Length > 0 Then ''upload file
                        Dim fileLocation As String = String.Empty
                        fileLocation = UploadTempFile(Me.fuUploadFileAttachment,
                                        customerExternalDocumentItem.DocumentationTypeString,
                                        customerExternalDocumentItem.Guid)
                        If fileLocation <> String.Empty Then
                            customerExternalDocumentItem.DocumentLocation = fileLocation
                            customerExternalDocumentItem.DocumentTitle = fuUploadFileAttachment.FileName
                            issuesCollection = AMCCertRecertController.UpdateCustomerExternalDocument(customerExternalDocumentItem)
                        End If
                    End If
                End If

                If issuesCollection IsNot Nothing AndAlso issuesCollection.Count > 0 Then
                    ShowError(issuesCollection, lblPopupMessage)
                    hdIsValidateFailed.Value = CommonConstants.TAB_INCOMPLETED
                Else
                    hdIsValidateFailed.Value = CommonConstants.TAB_COMPLETED
                End If
                BindingDataToList()
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED

            Catch ex As Exception
                ProcessException(ex)
            End Try
            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
        End Sub

        Protected Sub rptControlSubstance_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptControlSubstance.ItemCommand
            Try
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                If e.CommandName.Equals("Delete") Then
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                    Dim customerExternalDocumentItem As IUserDefinedCustomerExternalDocumentation
                    customerExternalDocumentItem = amcCertRecertController.GetCustomerExternalDocumentationByGUID(e.CommandArgument.ToString(), DocumentationType.CSAI.ToString(), Me.MasterCustomerId, Me.SubCustomerId)
                    issuesCollection = amcCertRecertController.DeleteCustomerExternalDocument(customerExternalDocumentItem)
                End If
                BindingDataToList()
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Protected Sub rptControlSubstance_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptControlSubstance.ItemDataBound
            Try
                If e.Item.ItemType = ListItemType.Item OrElse
               e.Item.ItemType = ListItemType.AlternatingItem Then
                    Dim lblStateProvince = CType(e.Item.FindControl("lblState"), Label)
                    Dim customerExternalDocument As UserDefinedCustomerExternalDocumentation =
                                        CType(e.Item.DataItem, UserDefinedCustomerExternalDocumentation)
                    lblStateProvince.Text =
                        customerExternalDocument.IssuingBody.List(
                            customerExternalDocument.IssuingBodyString).Description

                    Dim fileName As String = String.Empty
                    Dim linkLocation As String = String.Empty

                    If customerExternalDocument IsNot Nothing Then
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

                        ''set date

                        Dim lblDateOfOriginalIssue = CType(e.Item.FindControl("lblIssueDate"), Label)
                        If customerExternalDocument.InitialIssueDate <> DateTime.MinValue Then
                            lblDateOfOriginalIssue.Text = customerExternalDocument.InitialIssueDate.ToString(CommonConstants.DATE_FORMAT)
                        End If
                        Dim lblExpirationDate = CType(e.Item.FindControl("lblExpirDate"), Label)
                        If customerExternalDocument.CycleEndDate <> DateTime.MinValue Then
                            lblExpirationDate.Text = customerExternalDocument.CycleEndDate.ToString(CommonConstants.DATE_FORMAT)
                        End If

                    End If
                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub
#End Region

#Region "Private Method"

        ''' <summary>
        ''' set value for hdIsIncomplete , check vulues has incorrect or not when page loads
        ''' </summary>
        ''' <param></param>
        Public Overrides Sub ValidateFormFillCompleted()
            Try
                AMCCertRecertController.RefreshCustomerExternalDocuments(DocumentationType.CSAI.ToString(),
                                                                         MasterCustomerId,
                                                                         SubCustomerId)
                BindingDataToList()
                If Not Page.IsPostBack Then
                    CommonHelper.BindIssuingBodyType(Me.ddlState,
                                                 DocumentationType.CSAI.ToString())
                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Private Sub BindingDataToList()
            Dim customerExternals As IUserDefinedCustomerExternalDocumentations
            Try
                customerExternals = amcCertRecertController.GetCustomerExternalDocuments(DocumentationType.CSAI.ToString(), Me.MasterCustomerId, Me.SubCustomerId)
                If customerExternals IsNot Nothing Then
                    If customerExternals.Count > 0 AndAlso Not Page.IsPostBack Then
                        hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED

                    End If
                End If
                Me.rptControlSubstance.DataSource = customerExternals
                Me.rptControlSubstance.DataBind()
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Private Function SetPropertiesForObject(ByRef customerExternalDocumentItem As IUserDefinedCustomerExternalDocumentation) As Boolean
            If customerExternalDocumentItem Is Nothing Then
                Return False
            End If
            Dim isIssue As Boolean = True
            UploadFileIssue.Assert(False, customerExternalDocumentItem)
            With customerExternalDocumentItem
                .RelatedMasterCustomerId = Me.MasterCustomerId
                .RelatedSubcustomerId = Me.SubCustomerId
                .IssuingBodyString = ddlState.SelectedValue
                .IssuedNumber = Me.txtAuthorNumber.Text
                If CType(Me.txtExpirationDate, AMCDatetime).Text <> String.Empty Then
                    .CycleEndDate = DateTime.Parse(CType(Me.txtExpirationDate, AMCDatetime).Text)
                Else
                    .CycleEndDate = DateTime.MinValue

                End If
                If CType(Me.txtDateOfOriginalIssue, AMCDatetime).Text <> String.Empty Then
                    .InitialIssueDate = DateTime.Parse(CType(Me.txtDateOfOriginalIssue, AMCDatetime).Text)
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

    End Class
End Namespace