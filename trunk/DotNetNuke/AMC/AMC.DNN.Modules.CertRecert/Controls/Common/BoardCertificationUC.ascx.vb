Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports AMC.DNN.Modules.CertRecert.Controls.Reusable
Imports AMC.DNN.Modules.CertRecert.Business.BusinessValidation
Imports AMC.DNN.Modules.CertRecert.Business.Controller

Imports TIMSS.API.User.UserDefinedInfo
Imports TIMSS.API.Core.Validation

Namespace Controls.Common
    ''' <summary>
    ''' Prepresent GUI for BoardCertification tab 
    ''' </summary>
    Public Class BoardCertificationUC
        Inherits SectionBaseUserControl

#Region "Private Member"

#End Region

#Region "Event Handle"
        ''' <summary>
        ''' override save method of SectionBaseUserControl control for committing data
        ''' </summary>
        ''' <remarks></remarks>
        Public Overrides Function Save() As IIssuesCollection
            Try
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                Dim customerExternalDocuments = amcCertRecertController.GetCustomerExternalDocuments(
                                                               DocumentationType.BRDCRT.ToString(),
                                                               MasterCustomerId,
                                                               SubCustomerId)
                If customerExternalDocuments IsNot Nothing Then
                    For Each customerExternalDocument As IUserDefinedCustomerExternalDocumentation In customerExternalDocuments
                        UploadFileIssue.Assert(False, customerExternalDocument)
                    Next
                End If

                Dim results = amcCertRecertController.CommitCustomerExternalDocuments(DocumentationType.BRDCRT.ToString(),
                                                                                      MasterCustomerId,
                                                                                      SubCustomerId)


                If results Is Nothing OrElse results.Count <= 0 Then
                    MoveFileToApproriateDirectory(DocumentationType.BRDCRT.ToString())
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                    AMCCertRecertController.RefreshCustomerExternalDocuments(DocumentationType.BRDCRT.ToString(),
                                                                             MasterCustomerId,
                                                                             SubCustomerId)
                End If
                BindingDataToList()
                If customerExternalDocuments.Count <= 0 Then
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                End If
                Return results
            Catch ex As Exception
                ProcessException(ex)
            End Try
            Return Nothing
        End Function

        Protected Sub BtnSaveClick(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
            Try
                Dim customerExternals As New UserDefinedCustomerExternalDocumentations
                Dim customerExternalDocumentItem As IUserDefinedCustomerExternalDocumentation
                Dim issueCollection As IIssuesCollection
                'Edit 
                If (Me.hdCurrentObjectUniqueId.Value <> String.Empty) Then
                    customerExternalDocumentItem =
                        amcCertRecertController.GetCustomerExternalDocumentationByGUID(
                                            Me.hdCurrentObjectUniqueId.Value,
                                            DocumentationType.BRDCRT.ToString(),
                                            Me.MasterCustomerId,
                                            Me.SubCustomerId)
                    If SetPropertiesForObject(customerExternalDocumentItem) Then
                        issueCollection = amcCertRecertController.UpdateCustomerExternalDocument(customerExternalDocumentItem)
                    Else
                        issueCollection = customerExternalDocumentItem.ValidationIssues
                    End If
                Else
                    'Insert
                    customerExternalDocumentItem = customerExternals.CreateNew()
                    customerExternalDocumentItem.DocumentationTypeString =
                                                DocumentationType.BRDCRT.ToString()
                    customerExternalDocumentItem.IssuingBody.FillList()
                    customerExternalDocumentItem.IsNewObjectFlag = True
                    ''call insert fucntion of amc controller
                    If SetPropertiesForObject(customerExternalDocumentItem) Then
                        issueCollection = amcCertRecertController.InsertCustomerExternalDocument(customerExternalDocumentItem)
                    Else
                        issueCollection = customerExternalDocumentItem.ValidationIssues
                    End If
                End If

                If (issueCollection Is Nothing OrElse issueCollection.Count < 1) AndAlso
                    customerExternalDocumentItem IsNot Nothing Then
                    If Me.hfDeleteFile.Value.Equals("YES") Then ''Delete file
                        DeleteAttachDocument(DocumentationType.BRDCRT.ToString(),
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
                            issueCollection = AMCCertRecertController.UpdateCustomerExternalDocument(customerExternalDocumentItem)
                        End If
                    End If
                End If

                If issueCollection IsNot Nothing AndAlso issueCollection.Count > 0 Then
                    ShowError(issueCollection, lblPopupMessage)
                    hdIsValidateFailed.Value = CommonConstants.TAB_INCOMPLETED
                Else
                    hdIsValidateFailed.Value = CommonConstants.TAB_COMPLETED
                End If
                BindingDataToList()
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
            Catch ex As System.Exception
                ProcessException(ex)
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
            End Try
        End Sub

        Protected Sub rptSubCertificate_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptSubCertificate.ItemCommand
            Try
                If e.CommandName.Equals("Delete") Then
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                    Dim customerExternalDocumentItem As IUserDefinedCustomerExternalDocumentation
                    customerExternalDocumentItem = amcCertRecertController.GetCustomerExternalDocumentationByGUID(e.CommandArgument.ToString(), DocumentationType.BRDCRT.ToString(), Me.MasterCustomerId, Me.SubCustomerId)
                    amcCertRecertController.DeleteCustomerExternalDocument(customerExternalDocumentItem)
                End If
                BindingDataToList()
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Protected Sub rptSubCertificate_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptSubCertificate.ItemDataBound
            Try
                If e.Item.ItemType = ListItemType.AlternatingItem OrElse
                             e.Item.ItemType = ListItemType.Item Then
                    Dim customerExternalDocument As UserDefinedCustomerExternalDocumentation =
                                         CType(e.Item.DataItem, UserDefinedCustomerExternalDocumentation)
                    Dim lblBoard = CType(e.Item.FindControl("lblBoard"), Label)
                    Dim lblBoardCertification = CType(e.Item.FindControl("lblBoardCertification"), Label)
                    If String.IsNullOrEmpty(customerExternalDocument.IssuingBodyText) Then
                        lblBoard.Text =
                        customerExternalDocument.IssuingBody.List(
                            customerExternalDocument.IssuingBodyString).Description
                    Else
                        lblBoard.Text = String.Format("{0} - {1}",
                                                customerExternalDocument.IssuingBody.List(customerExternalDocument.IssuingBodyString).Description,
                                                    customerExternalDocument.IssuingBodyText)
                        lblBoardCertification.Text = customerExternalDocument.IssuingBodyText
                    End If

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
                    End If
                    Dim hdNoneRecertDate = CType(e.Item.FindControl("hdNoneRecertDate"), HiddenField)
                    Dim lblCertificateDate = CType(e.Item.FindControl("lblCertificateDate"), Label)
                    Dim lblRecertificateDate = CType(e.Item.FindControl("lblRecertificateDate"), Label)
                    If customerExternalDocument.CycleBeginDate <> DateTime.MinValue Then
                        lblCertificateDate.Text = customerExternalDocument.CycleBeginDate.ToString(CommonConstants.DATE_FORMAT)
                    End If
                    If customerExternalDocument.CycleEndDate.Date <> DateTime.MaxValue.Date Then
                        hdNoneRecertDate.Value = Boolean.FalseString
                        If customerExternalDocument.CycleEndDate <> DateTime.MinValue Then
                            lblRecertificateDate.Text = customerExternalDocument.CycleEndDate.ToString(CommonConstants.DATE_FORMAT)
                        End If
                    Else
                        hdNoneRecertDate.Value = Boolean.TrueString
                        lblRecertificateDate.Text = Localization.GetString("NoneRecertDateValue.Text", LocalResourceFile)
                    End If
                    

                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub
#End Region

#Region "Private Method"
        ''' <summary>
        ''' Validates usercontrol has completed data fill
        ''' </summary>
        Public Overrides Sub ValidateFormFillCompleted()
            AMCCertRecertController.RefreshCustomerExternalDocuments(DocumentationType.BRDCRT.ToString(),
                                                                     MasterCustomerId,
                                                                     SubCustomerId)
            BindingDataToList()
            If Not Page.IsPostBack Then
                CommonHelper.BindIssuingBodyType(Me.ddlBoardCertificate,
                             DocumentationType.BRDCRT.ToString())
            End If
        End Sub

        Private Sub BindingDataToList()
            Dim customerExternals As IUserDefinedCustomerExternalDocumentations
            customerExternals = amcCertRecertController.GetCustomerExternalDocuments(DocumentationType.BRDCRT.ToString(),
                                                                                     Me.MasterCustomerId,
                                                                                     Me.SubCustomerId)
            If customerExternals.Count > 0 AndAlso Not Page.IsPostBack Then
                hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
            End If
            rptSubCertificate.DataSource = customerExternals
            rptSubCertificate.DataBind()
        End Sub

        Private Function SetPropertiesForObject(ByRef customerExternalDocumentItem As IUserDefinedCustomerExternalDocumentation) As Boolean
            If customerExternalDocumentItem Is Nothing Then
                Return False
            End If
            Dim isIssue As Boolean = True
            Try
                UploadFileIssue.Assert(False, customerExternalDocumentItem)
                With customerExternalDocumentItem
                    .RelatedMasterCustomerId = Me.MasterCustomerId
                    .RelatedSubcustomerId = Me.SubCustomerId
                    .IssuingBodyString = ddlBoardCertificate.SelectedValue
                    .IssuedNumber = Me.txtCertificateNumber.Text
                    If .IssuingBodyString.Equals(CommonConstants.OtherBoardValue.ToString()) OrElse
                       .IssuingBodyString.Equals(CommonConstants.OtherSubBoardValue.ToString()) Then
                        .IssuingBodyText = Me.txtBoardCertification.Text
                    Else
                        .IssuingBodyText = String.Empty
                    End If

                    If CType(Me.txtCertificateDate, AMCDatetime).Text <> String.Empty Then
                        .CycleBeginDate = DateTime.Parse(CType(Me.txtCertificateDate, AMCDatetime).Text)
                    Else
                        .CycleBeginDate = DateTime.MinValue
                    End If
                    If Not chkNoneRecertDate.Checked Then
                        If CType(Me.txtRecertificateDate, AMCDatetime).Text <> String.Empty Then
                            .CycleEndDate = DateTime.Parse(CType(Me.txtRecertificateDate, AMCDatetime).Text)
                        Else
                            .CycleEndDate = DateTime.MinValue
                        End If
                    Else
                        .CycleEndDate = DateTime.MaxValue
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
            Catch ex As Exception
                Me.ProcessException(ex)
            End Try
            Return isIssue
        End Function
#End Region

        Private Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        End Sub
    End Class
End Namespace