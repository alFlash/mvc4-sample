Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Business.BusinessValidation
Imports AMC.DNN.Modules.CertRecert.Business.Controller
Imports TIMSS.API.Core.Validation
Imports TIMSS.API.User.UserDefinedInfo

Namespace Controls.Common
    ''' <summary>
    ''' Prepresent GUI for DocumentationUC tab 
    ''' </summary>
    Public Class DocumentationUC
        Inherits SectionBaseUserControl

#Region "Event Handler"
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                If Not Page.IsPostBack Then
                    BindingDataToList()
                    CommonHelper.BindIssuingBodyType(Me.ddlExperienceType,
                                                     DocumentationType.TREATMENT.ToString())
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Public Overrides Function Save() As IIssuesCollection
            Dim results As IIssuesCollection = Nothing
            Try
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED

                Dim customerExternalDocuments = amcCertRecertController.GetCustomerExternalDocuments(
                                                           DocumentationType.TREATMENT.ToString(),
                                                           MasterCustomerId,
                                                           SubCustomerId)
                If customerExternalDocuments IsNot Nothing Then
                    For Each customerExternalDocument As IUserDefinedCustomerExternalDocumentation In customerExternalDocuments
                        UploadFileIssue.Assert(False, customerExternalDocument)
                    Next
                End If
                results = amcCertRecertController.CommitCustomerExternalDocuments(
                                                            DocumentationType.TREATMENT.ToString(),
                                                            MasterCustomerId,
                                                            SubCustomerId)
                If results Is Nothing OrElse results.Count <= 0 Then
                    MoveFileToApproriateDirectory(DocumentationType.TREATMENT.ToString())
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                    AMCCertRecertController.RefreshCustomerExternalDocuments(DocumentationType.TREATMENT.ToString(),
                                                                             MasterCustomerId,
                                                                             SubCustomerId)
                End If
                BindingDataToList()

                If rptdocumentation.Items Is Nothing OrElse rptdocumentation.Items.Count <= 0 Then
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
            Return results
        End Function

        Protected Sub rptdocumentation_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptdocumentation.ItemCommand
            Try
                If e.CommandName.Equals("Delete") Then
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                    Dim customerExternalDocumentItem As IUserDefinedCustomerExternalDocumentation
                    customerExternalDocumentItem =
                        amcCertRecertController.GetCustomerExternalDocumentationByGUID(
                                                                e.CommandArgument.ToString(),
                                                                DocumentationType.TREATMENT.ToString(),
                                                                Me.MasterCustomerId,
                                                                Me.SubCustomerId)
                    amcCertRecertController.DeleteCustomerExternalDocument(customerExternalDocumentItem)
                End If
                BindingDataToList()
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Protected Sub rptdocumentation_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptdocumentation.ItemDataBound
            Try
                If e.Item.ItemType = ListItemType.Item OrElse
                                e.Item.ItemType = ListItemType.AlternatingItem Then
                    Dim fileName As String = String.Empty
                    Dim linkLocation As String = String.Empty
                    Dim customerExternalDocument As UserDefinedCustomerExternalDocumentation =
                                            CType(e.Item.DataItem, UserDefinedCustomerExternalDocumentation)

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

                        Dim lblExperienceTypeString =
                                    CType(e.Item.FindControl("lblExperienceTypeString"), Label)
                        lblExperienceTypeString.Text =
                            customerExternalDocument.IssuingBody.List(
                                customerExternalDocument.IssuingBodyString).Description
                    End If
                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click

            Dim customerExternals As New UserDefinedCustomerExternalDocumentations
            Dim customerExternalDocumentItem As IUserDefinedCustomerExternalDocumentation
            Dim issueCollection As IIssuesCollection
            Try
                ''edit 
                If (Me.hdCurrentObjectUniqueId.Value <> String.Empty) Then
                    customerExternalDocumentItem =
                        amcCertRecertController.GetCustomerExternalDocumentationByGUID(
                                                                Me.hdCurrentObjectUniqueId.Value,
                                                                DocumentationType.TREATMENT.ToString(),
                                                                Me.MasterCustomerId,
                                                                Me.SubCustomerId)
                    If SetValuesForObjectCusDocumentation(customerExternalDocumentItem) Then
                        issueCollection = amcCertRecertController.UpdateCustomerExternalDocument(customerExternalDocumentItem)
                    Else
                        issueCollection = customerExternalDocumentItem.ValidationIssues
                    End If

                Else
                    ''Insert
                    customerExternalDocumentItem = customerExternals.CreateNew()
                    customerExternalDocumentItem.DocumentationTypeString =
                                            DocumentationType.TREATMENT.ToString()
                    customerExternalDocumentItem.IsNewObjectFlag = True
                    customerExternalDocumentItem.IssuingBody.FillList()
                    If SetValuesForObjectCusDocumentation(customerExternalDocumentItem) Then
                        issueCollection = amcCertRecertController.InsertCustomerExternalDocument(customerExternalDocumentItem)
                    Else
                        issueCollection = customerExternalDocumentItem.ValidationIssues
                    End If
                End If

                If issueCollection Is Nothing OrElse issueCollection.Count < 1 Then
                    If Me.hfDeleteFile.Value.Equals("YES") Then ''Delete file
                        DeleteAttachDocument(DocumentationType.TREATMENT.ToString(),
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
                            issueCollection = amcCertRecertController.UpdateCustomerExternalDocument(customerExternalDocumentItem)
                        End If
                    End If
                End If
                If issueCollection IsNot Nothing AndAlso issueCollection.Count > 0 Then
                    ShowError(issueCollection, lblPopupMessage)
                    hdIsValidateFailed.Value = CommonConstants.TAB_INCOMPLETED
                Else
                    hdIsValidateFailed.Value = CommonConstants.TAB_COMPLETED
                    BindingDataToList()
                End If

            Catch ex As Exception
                ProcessException(ex)
            End Try
            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
        End Sub
#End Region

#Region "Private Method"
        ''' <summary>
        ''' set value for hdIsIncomplete , check vulues has incorrect or not when page loads
        ''' </summary>
        ''' <param></param>
        Public Overrides Sub ValidateFormFillCompleted()
            Try
                AMCCertRecertController.RefreshCustomerExternalDocuments(DocumentationType.TREATMENT.ToString(),
                                                                         MasterCustomerId,
                                                                         SubCustomerId)
                BindingDataToList()
                If Not Page.IsPostBack Then
                    CommonHelper.BindIssuingBodyType(Me.ddlExperienceType,
                                                 DocumentationType.TREATMENT.ToString())
                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Private Sub BindingDataToList()
            Dim customerExternals As IUserDefinedCustomerExternalDocumentations
            customerExternals =
             amcCertRecertController.GetCustomerExternalDocuments(
                                                        DocumentationType.TREATMENT.ToString(),
                                                        Me.MasterCustomerId,
                                                        Me.SubCustomerId)
            If customerExternals IsNot Nothing Then
                If customerExternals.Count > 0 AndAlso Not Page.IsPostBack Then
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                End If
            End If
            Me.rptdocumentation.DataSource = customerExternals
            Me.rptdocumentation.DataBind()
        End Sub

        Private Function SetValuesForObjectCusDocumentation(ByRef customerExternalDocumentItem As IUserDefinedCustomerExternalDocumentation) As Boolean
            If customerExternalDocumentItem Is Nothing Then
                Return False
            End If


            Dim isIssue As Boolean = True
            UploadFileIssue.Assert(False, customerExternalDocumentItem)
            With customerExternalDocumentItem
                .RelatedMasterCustomerId = Me.MasterCustomerId
                .RelatedSubcustomerId = Me.SubCustomerId
                .IssuingBodyString = Me.ddlExperienceType.SelectedValue
                .IssuingBodyText = Me.txtExperienceDetail.InnerText
                .IssuedNumber = CommonConstants.ISSUE_NUMBER_DEFAULT.ToString()
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
