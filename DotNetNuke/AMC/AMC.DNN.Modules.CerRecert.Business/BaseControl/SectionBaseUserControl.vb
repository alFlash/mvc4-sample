Imports AMC.DNN.Modules.CertRecert.Business.Controller
Imports AMC.DNN.Modules.CertRecert.Business.Entities
Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems

Imports System.Linq
Imports System.IO

Imports DotNetNuke.Services.Log.EventLog
Imports DotNetNuke.Services.Exceptions

Imports TIMSS.API.User.UserDefinedInfo
Imports TIMSS.API.Core.Validation
Imports TIMSS.API.CertificationInfo

Imports Microsoft.VisualBasic.FileIO
Imports Personify.ApplicationManager

Imports System.Web.UI.WebControls
Imports System.Web.UI

Namespace BaseControl
    Public MustInherit Class SectionBaseUserControl
        Inherits PersonifyDNNBaseForm

#Region "Protected Mebmer"
        Public Property PrintMode As Boolean

        ''' <summary>
        ''' 
        ''' </summary>
        Public AMCCertRecertController As AmcCertRecertController
        ''' <summary>
        ''' 
        ''' </summary>
        Private ReadOnly _exceptionLogController As ExceptionLogController = New ExceptionLogController()
#End Region

#Region "Properties"
        ''' <summary>
        ''' Gets or sets the parent module path.
        ''' </summary>
        ''' <value>
        ''' The parent module path.
        ''' </value>
        Public Property ParentModulePath() As String
        ''' <summary>
        ''' Gets or sets the current section info.
        ''' </summary>
        ''' <value>
        ''' The current section info.
        ''' </value>
        Public Property CurrentSectionInfo As SectionInfo
        ''' <summary>
        ''' Gets or sets the current form info.
        ''' </summary>
        ''' <value>
        ''' The current form info.
        ''' </value>
        Public Property CurrentFormInfo() As FormInfo
        ''' <summary>
        ''' Gets or sets the certification id.
        ''' </summary>
        ''' <value>
        ''' The certification id.
        ''' </value>
        Public Property CertificationId() As Integer
        ''' <summary>
        ''' Gets or sets the current certification customer certification.
        ''' </summary>
        ''' <value>
        ''' The current certification customer certification.
        ''' </value>
        Public Property CurrentCertificationCustomerCertification As ICertificationCustomerCertification
        ''' <summary>
        ''' Gets or sets the form id.
        ''' </summary>
        ''' <value>
        ''' The form id.
        ''' </value>
        Public Property FormId() As String
        ''' <summary>
        ''' Gets or sets the section id.
        ''' </summary>
        ''' <value>
        ''' The section id.
        ''' </value>
        Public Property SectionId() As String
        ''' <summary>
        ''' Gets or sets the show error message.
        ''' </summary>
        ''' <value>
        ''' The show error message.
        ''' </value>
        Public Property ShowErrorMessage As Action(Of String)
        ''' <summary>
        ''' Gets or sets the current re cert option action.
        ''' </summary>
        ''' <value>
        ''' The current re cert option action.
        ''' </value>
        Public Property CurrentReCertOptionAction As Action(Of String)
        ''' <summary>
        ''' Gets or sets the get current re cert option action.
        ''' </summary>
        ''' <value>
        ''' The get current re cert option action.
        ''' </value>
        Public Property GetCurrentReCertOptionAction As Func(Of UserDefinedSurveyQuestion)
        ''' <summary>
        ''' Gets or sets the index of the factor.
        ''' </summary>
        ''' <value>
        ''' The index of the factor.
        ''' </value>
        Public Property FactorIndex As String
        ''' <summary>
        ''' Gets or sets the type of the program.
        ''' </summary>
        ''' <value>
        ''' The type of the program.
        ''' </value>
        Public Property ProgramType As String
        ''' <summary>
        ''' Gets or sets the CE weight list.
        ''' </summary>
        ''' <value>
        ''' The CE weight list.
        ''' </value>
        Public Property CEWeightList As UserDefinedCertificationCEWeights
        ''' <summary>
        ''' Gets or sets the total point.
        ''' </summary>
        ''' <value>
        ''' The total point.
        ''' </value>
        Public Property TotalPoint As Decimal
        ''' <summary>
        ''' Gets or sets the total CE one form.
        ''' </summary>
        ''' <value>
        ''' The total CE one form.
        ''' </value>
        Public Property TotalCEOneForm As Decimal
        ''' <summary>
        ''' Gets or sets the payment process action.
        ''' </summary>
        ''' <value>
        ''' The payment process action.
        ''' </value>
        ''' 
        Public Property TotalPointARN As Decimal
        ''' <summary>
        ''' Gets or sets the payment process action.
        ''' </summary>
        ''' <value>
        ''' The payment process action.
        ''' </value>

        Public Property PaymentProcessAction As Action(Of Integer())
        ''' <summary>
        ''' Gets or sets the get product ids action.
        ''' </summary>
        ''' <value>
        ''' The get product ids action.
        ''' </value>
        Public Property GetProductIdsAction As Func(Of UserDefinedSurveyQuestion, Integer())
        ''' <summary>
        ''' Gets or sets the rule ID.
        ''' </summary>
        ''' <value>
        ''' The rule ID.
        ''' </value>
        Public Property RuleID As String
        ''' <summary>
        ''' Gets or sets the conflict message.
        ''' </summary>
        ''' <value>
        ''' The conflict message.
        ''' </value>
        Public Property ConflictMessage As String
        ''' <summary>
        ''' Gets or sets the certification code.
        ''' </summary>
        ''' <value>
        ''' The certification code.
        ''' </value>
        Public Property CertificationCode() As CertificationCode
        ''' <summary>
        ''' Gets or sets the max CE hours ARN.
        ''' </summary>
        ''' <value>
        ''' The max CE hours ARN.
        ''' </value>
        Public Property MaxCEHoursARN As Decimal
        ''' <summary>
        ''' Gets or sets the recertification circle.
        ''' </summary>
        ''' <value>
        ''' The recertification circle.
        ''' </value>
        Public Property RecertificationCircle As RecertificationCircleValidation
        ''' <summary>
        ''' Gets the master customer id.
        ''' </summary>
        ''' 
        Public Property CETypeNameList As String

        Public Overridable ReadOnly Property MasterCustomerId As String
            Get
                Dim masterCustomerQuery As String = Request.QueryString(CommonConstants.MASTER_CUSTOMER_ID)
                If Not String.IsNullOrEmpty(masterCustomerQuery) Then
                    Return masterCustomerQuery
                End If
                Return MyBase.MasterCustomerId
            End Get
        End Property
        ''' <summary>
        ''' Gets the sub customer id.
        ''' </summary>
        ''' 
        Public Overridable ReadOnly Property SubCustomerId As Integer
            Get
                Dim subCustomerIdQuery As String = Request.QueryString(CommonConstants.SUB_CUSTOMER_ID)
                If Not String.IsNullOrEmpty(subCustomerIdQuery) Then
                    Dim cusSubCustomerId As Integer
                    If Integer.TryParse(subCustomerIdQuery, cusSubCustomerId) Then
                        Return cusSubCustomerId
                    End If
                End If
                Return MyBase.SubCustomerId
            End Get
        End Property

        Public Property ReferencAndVeryficationSurveyTitle() As String

#End Region

#Region "Event handler"

        ''' <summary>
        ''' Handles the Load event of the Page control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            Try
                If (CurrentSectionInfo IsNot Nothing) Then
                    ModuleConfigurationHelper.Instance.ApplySectionConfigurations(CurrentSectionInfo, Me)

                    If Not Page.IsPostBack AndAlso CurrentSectionInfo.IsEnabled Then
                        Dim hiddenIsInCompleted = FindControl("hdIsIncomplete")
                        If hiddenIsInCompleted IsNot Nothing AndAlso TypeOf hiddenIsInCompleted Is HiddenField Then
                            CType(hiddenIsInCompleted, HiddenField).Value = CommonConstants.TAB_INCOMPLETED
                        End If

                        'Validate form already filled all data
                        ValidateFormFillCompleted()
                    End If
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
            Try
                If PrintMode Then
                    If Page.Validators IsNot Nothing Then
                        For i As Integer = Page.Validators.Count - 1 To 0 Step -1
                            Dim validator = CType(Page.Validators(i), Control)
                            validator.Parent.Controls.Remove(validator)
                        Next
                    End If
                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub
#End Region

#Region "Public Methods"
        ''' <summary>
        ''' Determines whether [is question existed] [the specified survey title].
        ''' </summary>
        ''' <param name="surveyTitle">The survey title.</param>
        ''' <param name="questionCode">The question code.</param>
        ''' <returns>
        '''   <c>true</c> if [is question existed] [the specified survey title]; otherwise, <c>false</c>.
        ''' </returns>
        Public Function IsQuestionExisted(ByVal surveyTitle As String, ByVal questionCode As String) As Boolean
            Dim result = False
            Dim survey = CType(AMCCertRecertController.GetSurveyByTitle(surveyTitle), UserDefinedSurvey)
            If survey IsNot Nothing Then
                Dim question = survey.UserDefinedSurveyQuestions.FindObject("QuestionCode", questionCode)
                If question IsNot Nothing AndAlso CType(question, IUserDefinedSurveyQuestion).Enabled Then
                    result = True
                End If
            End If
            Return result
        End Function


        ''' <summary>
        ''' Gets the product ids.
        ''' </summary>
        ''' <param name="currentRecertOption">The current recert option.</param>
        ''' <returns></returns>
        Public Function GetProductIds(Optional ByVal currentRecertOption As UserDefinedSurveyQuestion = Nothing) As Integer()
            If GetProductIdsAction IsNot Nothing Then
                Return GetProductIdsAction.Invoke(currentRecertOption)
            End If
            Return Nothing
        End Function

        ''' <summary>
        ''' Process the Payment.
        ''' </summary>
        ''' <param name="productIds">The product ids.</param>
        Public Sub PaymentProcess(ByVal productIds As Integer())
            If PaymentProcessAction IsNot Nothing Then
                PaymentProcessAction.Invoke(productIds)
            End If
        End Sub

        ''' <summary>
        ''' Gets the field id prefix.
        ''' </summary>
        ''' <param name="fieldType">Type of the field.</param>
        ''' <returns></returns>
        Public Function GetFieldIdPrefix(ByVal fieldType As String) As String
            Select Case fieldType
                Case CommonConstants.CONFIGURATION_LABEL_TYPE
                    Return CommonConstants.CONFIGURATION_LABEL_TYPE_PREFIX
                Case CommonConstants.CONFIGURATION_HYPERLINK_TYPE
                    Return CommonConstants.CONFIGURATION_HYPERLINK_TYPE_PREFIX
                Case CommonConstants.CONFIGURATION_BUTTON_TYPE
                    Return CommonConstants.CONFIGURATION_BUTTON_TYPE_PREFIX
                Case CommonConstants.CONFIGURATION_LINK_BUTTON_TYPE
                    Return CommonConstants.CONFIGURATION_LINK_BUTTON_TYPE_PREFIX
                Case Else
                    Return CommonConstants.CONFIGURATION_LABEL_TYPE_PREFIX
            End Select
        End Function

        ''' <summary>
        ''' Gets the field info.
        ''' </summary>
        ''' <param name="fieldId">The field id.</param>
        ''' <returns></returns>
        Public Function GetFieldInfo(ByVal fieldId As String) As FieldInfo
            If CurrentSectionInfo IsNot Nothing And CurrentSectionInfo.Fields IsNot Nothing Then
                For Each fieldInfo As FieldInfo In _
                    From fieldInfo1 In CurrentSectionInfo.Fields Where fieldInfo1.FieldId = fieldId
                    Return fieldInfo
                Next
            End If
            Return New FieldInfo()
        End Function

        ''' <summary>
        ''' Gets the temp directory.
        ''' </summary>
        ''' <param name="documentType">Type of the document.</param>
        ''' <param name="guid">The GUID.</param>
        ''' <param name="isCreate">if set to <c>true</c> [is create].</param>
        ''' <returns></returns>
        Public Function GetTempDirectory(ByVal documentType As String, ByVal guid As String, ByVal isCreate As Boolean) _
            As String
            Dim directoryResult As String
            Dim uniqueSession As String
            If Session.IsNewSession = True Then
                Session("UniqueSession") = System.Guid.NewGuid().ToString()
            End If
            uniqueSession = CType(Session("UniqueSession"), String)
            directoryResult = String.Format("{0}Cert_Recert/Files/{1}_{2}/{3}/{4}/GUID/{5}",
                                                PortalSettings.HomeDirectory, MasterCustomerId, SubCustomerId,
                                                documentType, uniqueSession, guid)
            Dim directoryInfo As New DirectoryInfo(Server.MapPath(directoryResult))
            directoryResult = directoryInfo.FullName
            If Not isCreate Then
                Return directoryResult
            End If

            If Not directoryInfo.Exists Then
                directoryInfo.Create()
                directoryInfo = New DirectoryInfo(directoryResult)
                If Not directoryInfo.Exists Then
                    directoryResult = String.Empty
                Else
                    directoryResult = directoryInfo.FullName
                End If
            End If
            Return directoryResult
        End Function

        ''' <summary>
        ''' Gets the directory of documentation.
        ''' </summary>
        ''' <param name="documentType">Type of the document.</param>
        ''' <param name="objectId">The object id.</param>
        ''' <param name="isCreate">if set to <c>true</c> [is create].</param>
        ''' <returns></returns>
        Public Function GetDirectoryOfDocumentation(ByVal documentType As String,
                                                     ByVal objectId As String,
                                                     ByVal isCreate As Boolean) As String
            Dim directoryResult As String
            directoryResult = String.Format("{0}Cert_Recert/Files/{1}_{2}/{3}/{4}",
                                                PortalSettings.HomeDirectory, MasterCustomerId, SubCustomerId,
                                                documentType, objectId)
            Dim directoryInfo As New DirectoryInfo(Server.MapPath(directoryResult))
            directoryResult = directoryInfo.FullName
            If Not isCreate Then
                Return directoryResult
            End If

            If Not directoryInfo.Exists Then
                directoryInfo.Create()
                directoryInfo = New DirectoryInfo(directoryResult)
                If Not directoryInfo.Exists Then
                    directoryResult = String.Empty
                Else
                    directoryResult = directoryInfo.FullName
                End If
            End If
            Return directoryResult
        End Function

        ''' <summary>
        ''' Uploads the temp file.
        ''' </summary>
        ''' <param name="fileUpload">The file upload.</param>
        ''' <param name="documentType">Type of the document.</param>
        ''' <param name="guid">The GUID.</param>
        ''' <returns></returns>
        Public Function UploadTempFile(ByVal fileUpload As FileUpload,
                                        ByVal documentType As String,
                                        ByVal guid As String) As String
            Dim result = String.Empty
            If fileUpload Is Nothing OrElse fileUpload.FileContent.Length < 1 Then
                Return result
            End If
            Dim directoryResult = GetTempDirectory(documentType, guid, True)
            Dim files = Directory.GetFiles(directoryResult)
            For Each file As String In files
                FileSystem.DeleteFile(file)
            Next
            If directoryResult <> String.Empty Then
                directoryResult = String.Format("{0}/{1}",
                                                    directoryResult,
                                                    fileUpload.FileName)
                fileUpload.SaveAs(directoryResult)
            End If
            result = directoryResult
            Return result
        End Function

        ''' <summary>
        ''' Gets the file name of document.
        ''' </summary>
        ''' <param name="type">The type.</param>
        ''' <param name="guid">The GUID.</param>
        ''' <param name="objectId">The object id.</param>
        ''' <param name="documentUrl">The document URL.</param>
        ''' <returns></returns>
        Public Function GetFileNameOfDocument(ByVal type As String,
                                               ByVal guid As String,
                                               ByVal objectId As String,
                                               ByRef documentUrl As String) As String
            Dim fileName As String = String.Empty
            Dim directoryPath As String
            Dim applicationPath As String = Server.MapPath("")

            directoryPath = GetTempDirectory(type, guid, False)
            If Not Directory.Exists(directoryPath) Then
                directoryPath = GetDirectoryOfDocumentation(type, objectId, False)
            End If
            If Directory.Exists(directoryPath) Then
                Dim directoryInfo As New DirectoryInfo(directoryPath)
                Dim fileInfos = directoryInfo.GetFiles()
                If fileInfos.Count > 0 Then
                    'fileName = fileInfos (0).Name.Replace ("AMC_", String.Empty)
                    fileName = fileInfos(0).Name
                    documentUrl = Page.ResolveClientUrl(fileInfos(0).FullName)
                    documentUrl = documentUrl.Substring(applicationPath.Length).Replace("\\", "/").Insert(0, "~/")
                End If
            End If
            Return fileName

        End Function

        ''' <summary>
        ''' Deletes the attach document.
        ''' </summary>
        ''' <param name="documentType">Type of the document.</param>
        ''' <param name="guid">The GUID.</param>
        ''' <param name="objectId">The object id.</param>
        ''' <returns></returns>
        Public Function DeleteAttachDocument(ByVal documentType As String,
                                             ByVal guid As String,
                                             ByVal objectId As String) As Boolean
            Const result As Boolean = True
            Dim directoryResult = GetTempDirectory(documentType, guid, True)
            Dim files = Directory.GetFiles(directoryResult)
            For Each file As String In files
                FileSystem.DeleteFile(file)
            Next
            Return result
        End Function

        Public Function DeleteMainAttachDocument(ByVal documentType As String, ByVal objectId As String) As Boolean
            Const result As Boolean = True
            Dim directoryResult = GetDirectoryOfDocumentation(documentType, objectId, False)
            Dim files = Directory.GetFiles(directoryResult)
            For Each file As String In files
                FileSystem.DeleteFile(file)
            Next
            Return result
        End Function

        ''' <summary>
        ''' Moves the file from temp to main directory.
        ''' </summary>
        ''' <param name="documentType">Type of the document.</param>
        ''' <param name="guid">The GUID.</param>
        ''' <param name="objectId">The object id.</param>
        ''' <param name="documentUrl">The document URL.</param>
        ''' <returns></returns>
        Public Function MoveFileFromTempToMainDirectory(ByVal documentType As String,
                                               ByVal guid As String,
                                               ByVal objectId As String,
                                               ByRef documentUrl As String) As String
            Dim fileNameResult As String = String.Empty
            Dim applicationPath As String = Server.MapPath("")
            Dim guidDirectory = GetTempDirectory(documentType,
                                                 guid,
                                                 False)
            If Directory.Exists(guidDirectory) Then
                Dim directoryInfo As New DirectoryInfo(guidDirectory)
                Dim objectIdDirectory = GetDirectoryOfDocumentation(documentType,
                                                                    objectId,
                                                                    True)
                '' delete existing documentation
                If Directory.Exists(objectIdDirectory) Then
                    Dim objectIdFileInfos = Directory.GetFiles(objectIdDirectory)
                    If objectIdFileInfos IsNot Nothing AndAlso objectIdFileInfos.Count > 0 Then
                        For Each file As String In objectIdFileInfos
                            FileSystem.DeleteFile(file)
                        Next
                    End If
                End If
                Dim fileInfos = directoryInfo.GetFiles()
                If fileInfos IsNot Nothing AndAlso fileInfos.Count > 0 Then
                    objectIdDirectory = String.Format("{0}/{1}",
                                                      objectIdDirectory,
                                                      fileInfos(0).Name)
                    fileInfos(0).MoveTo(objectIdDirectory)

                    fileNameResult = fileInfos(0).Name
                    documentUrl = Page.ResolveClientUrl(fileInfos(0).FullName)
                    documentUrl = documentUrl.Substring(applicationPath.Length).Replace("\\", "/").Insert(0, "~/")
                End If
                FileSystem.DeleteDirectory(guidDirectory, DeleteDirectoryOption.DeleteAllContents)
            End If
            Return fileNameResult
        End Function

        ''' <summary>
        ''' Moves the file to approriate directory.
        ''' </summary>
        ''' <param name="documentType">Type of the document.</param>
        Protected Overridable Sub MoveFileToApproriateDirectory(ByVal documentType As String)
            Dim customerExternals As IUserDefinedCustomerExternalDocumentations
            customerExternals = AMCCertRecertController.GetCustomerExternalDocuments(documentType,
                                                                                    MasterCustomerId,
                                                                                    SubCustomerId)
            If customerExternals IsNot Nothing Then
                Dim fileName As String
                Dim fileLocation As String = String.Empty
                For Each customerExternalDocumentationItem As IUserDefinedCustomerExternalDocumentation In customerExternals

                    fileName = MoveFileFromTempToMainDirectory(documentType,
                                                    customerExternalDocumentationItem.Guid,
                                        customerExternalDocumentationItem.DocumentationId.ToString(),
                                        fileLocation)
                    If Not fileName.Equals(String.Empty) Then
                        customerExternalDocumentationItem.DocumentTitle = fileName
                        customerExternalDocumentationItem.DocumentLocation = fileLocation
                        AMCCertRecertController.UpdateCustomerExternalDocument(customerExternalDocumentationItem)
                    End If
                Next
                AMCCertRecertController.CommitCustomerExternalDocuments(documentType, MasterCustomerId, SubCustomerId)
            End If
        End Sub

        ''' <summary>
        ''' This function will how error on popup form
        ''' </summary>
        ''' <param name="issuesCollection"></param>
        ''' <param name="message"></param>
        ''' <remarks></remarks>
        Public Sub ShowError(ByVal issuesCollection As IIssuesCollection, ByVal message As Label)
            message.Text = String.Empty
            For Each collection As IIssue In issuesCollection
                message.Text += collection.Message + "<br/>"
            Next
        End Sub

        ''' <summary>
        ''' Process unhanlde exception on page by logging error and showing a message
        ''' </summary>
        ''' <param name="unhandleException"></param>
        ''' <remarks></remarks>
        Public Sub ProcessException(ByVal unhandleException As Exception)
            _exceptionLogController.AddLog(unhandleException)
            If Not ShowErrorMessage Is Nothing Then
                ShowErrorMessage.Invoke(unhandleException.Message)
            End If
        End Sub

        ''' <summary>
        ''' Sets the current re cert option.
        ''' </summary>
        ''' <param name="questionId">The question id.</param>
        Public Sub SetCurrentReCertOption(ByVal questionId As String)
            If CurrentReCertOptionAction IsNot Nothing Then
                CurrentReCertOptionAction.Invoke(questionId)
            End If
        End Sub

        ''' <summary>
        ''' Gets the current re cert option.
        ''' </summary>
        ''' <returns></returns>
        Public Function GetCurrentReCertOption() As UserDefinedSurveyQuestion
            If GetCurrentReCertOptionAction IsNot Nothing Then
                Return GetCurrentReCertOptionAction.Invoke()
            End If
            Return Nothing
        End Function

        ''' <summary>
        ''' Shows the issue messages.
        ''' </summary>
        ''' <param name="issueCollection">The issue collection.</param>
        Public Sub ShowIssueMessages(ByVal issueCollection As IIssuesCollection)
            Dim errorMessage As String
            If issueCollection IsNot Nothing Then
                For Each issue As IIssue In issueCollection
                    errorMessage += issue.Message + "<br/>"
                Next
                If Not ShowErrorMessage Is Nothing Then
                    ShowErrorMessage.Invoke(errorMessage)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Shows the error.
        ''' </summary>
        ''' <param name="message">The message.</param>
        Public Sub ShowError(ByVal message As String)
            If Not ShowErrorMessage Is Nothing Then
                ShowErrorMessage.Invoke(message)
            End If
        End Sub

        ''' <summary>
        ''' Processes the popup exception.
        ''' </summary>
        ''' <param name="unhandleException">The unhandle exception.</param>
        ''' <param name="message">The message.</param>
        Public Sub ProcessPopupException(ByVal unhandleException As Exception, ByVal message As Label)
            _exceptionLogController.AddLog(unhandleException)
            'will implement show error message here
            message.Text = unhandleException.Message
        End Sub

        ''' <summary>
        ''' Saves this instance.
        ''' </summary>
        ''' <returns></returns>
        Public MustOverride Function Save() As IIssuesCollection
        ''' <summary>
        ''' Validates the form fill completed.
        ''' </summary>
        Public MustOverride Sub ValidateFormFillCompleted()

        Public Function GetCookie(ByVal key As String) As String
            Return If(Request.Cookies(key) IsNot Nothing AndAlso Not String.IsNullOrEmpty(Request.Cookies(key).Value), Request.Cookies(key).Value, "0")
        End Function

        Public Sub SetCookie(ByVal key As String, ByVal value As String)
            Response.Cookies(key).Expires = DateTime.Now.AddDays(1)
            Response.Cookies(key).Value = value
        End Sub

#End Region
    End Class
End Namespace