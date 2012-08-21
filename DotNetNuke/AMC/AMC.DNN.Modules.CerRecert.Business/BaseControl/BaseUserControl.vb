Imports AMC.DNN.Modules.CertRecert.Business.Entities
Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Business.PersonifyShoppingCart
Imports AMC.DNN.Modules.CertRecert.Data
Imports AMC.DNN.Modules.CertRecert.Business.Controller
Imports AMC.DNN.Modules.CertRecert.Business.IControls

Imports DotNetNuke.Services.Log.EventLog
Imports DotNetNuke.Common

Imports System.Linq
Imports System.ServiceModel
Imports System.Threading
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web
Imports AMC.DNN.Modules.CertRecert.Data.Enums
Imports Personify.ApplicationManager
Imports TIMSS.Security.Network
Imports TIMSS.Client.Implementation.Security.Authentication
Imports TIMSS.API.CertificationInfo
Imports TIMSS.API.Core.Validation
Imports TIMSS.API.User.UserDefinedInfo

Namespace BaseControl
    Public MustInherit Class BaseUserControl
        Inherits PersonifyDNNBaseForm

        Protected AMCCertRecertController As AmcCertRecertController
        Private ReadOnly _exceptionLogController As ExceptionLogController = New ExceptionLogController()
        Private _currentCertification As ICertificationCustomerCertification = Nothing
        Private _currentCertificationId As Integer
        Protected ReadOnly _logController As LogController = New LogController()
#Region "Properties"
        ''' <summary>
        ''' Gets or sets a value indicating whether [payment processed].
        ''' </summary>
        ''' <value>
        '''   <c>true</c> if [payment processed]; otherwise, <c>false</c>.
        ''' </value>
        Public Property PaymentProcessed() As Boolean
        ''' <summary>
        ''' Gets or sets the parent module path.
        ''' </summary>
        ''' <value>
        ''' The parent module path.
        ''' </value>
        Public Property ParentModulePath() As String
        ''' <summary>
        ''' Gets or sets the current form info.
        ''' </summary>
        ''' <value>
        ''' The current form info.
        ''' </value>
        Public Property CurrentFormInfo() As FormInfo
        ''' <summary>
        ''' Stores forms has completed data and ids
        ''' </summary>
        Public Property RecertificationCircle As RecertificationCircleValidation
        ''' <summary>
        ''' Gets or sets the amc certification code.
        ''' </summary>
        ''' <value>
        ''' The amc certification code.
        ''' </value>
        Public Property AmcCertificationCode As CertificationCode
        ''' <summary>
        ''' Gets or sets the step completed list.
        ''' </summary>
        ''' <value>
        ''' The step completed list.
        ''' </value>
        Public Property StepCompletedList() As String
        ''' <summary>
        ''' Gets the master customer id.
        ''' </summary>
        ''' 
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
        ''' <summary>
        ''' Gets or sets the certification id.
        ''' </summary>
        ''' <value>
        ''' The certification id.
        ''' </value>
        Public Property CertificationId() As Integer
            Set(ByVal value As Integer)
                _currentCertificationId = value
            End Set
            Get
                Return _currentCertificationId
            End Get
        End Property
        ''' <summary>
        ''' Gets or sets the current certification customer certification.
        ''' </summary>
        ''' <value>
        ''' The current certification customer certification.
        ''' </value>
        Public Property CurrentCertificationCustomerCertification() As ICertificationCustomerCertification
            Set(ByVal value As ICertificationCustomerCertification)
                _currentCertification = value
            End Set
            Get
                If Not String.IsNullOrEmpty(Request.QueryString(CommonConstants.CERTIFICATION_ID_QUERY_STRING)) Then
                    Dim certificationIdQuery As Integer
                    If AMCCertRecertController IsNot Nothing Then
                        If Integer.TryParse(Request.QueryString(CommonConstants.CERTIFICATION_ID_QUERY_STRING), certificationIdQuery) Then
                            _currentCertification = AMCCertRecertController.GetCertificationCustomerCertificationByCertId(certificationIdQuery)
                            CertificationId = _currentCertification.CertificationId
                        End If
                    End If
                End If
                Return _currentCertification
            End Get
        End Property

#End Region

#Region "Public Methods"

        Public Overridable Function ReferencAndVeryficationSurveyTitle() As String
            Return DataAccessConstants.CERT_REFERENCE_VERIFICATION_SURVEY_TITLE
        End Function

        ''' <summary>
        ''' Reloads the specified mv certification.
        ''' </summary>
        ''' <param name="mvCertification">The mv certification.</param>
        ''' <param name="saveControlId">The save control id.</param>
        Public Sub Reload(ByVal mvCertification As MultiView, ByVal saveControlId As String)
            For Each view As View In mvCertification.Views
                Dim sectionControl = view.FindControl(String.Format("section{0}", view.ID))
                Dim control = TryCast(sectionControl, IReload)
                If (control IsNot Nothing) Then
                    control.Reload(saveControlId)
                End If
            Next
        End Sub

        ''' <summary>
        ''' Gets the section info.
        ''' </summary>
        ''' <param name="sectionId">The section id.</param>
        ''' <returns></returns>
        Public Function GetSectionInfo(ByVal sectionId As String) As SectionInfo
            If CurrentFormInfo IsNot Nothing AndAlso CurrentFormInfo.Sections IsNot Nothing Then
                For Each sectionInfo As SectionInfo In _
                    From sectionInfo1 In CurrentFormInfo.Sections Where sectionInfo1.SectionId = sectionId
                    Return sectionInfo
                Next
            End If
            Return New SectionInfo()
        End Function

        ''' <summary>
        ''' Gets the field info.
        ''' </summary>
        ''' <param name="sectionId">The section id.</param>
        ''' <param name="fieldId">The field id.</param>
        ''' <returns></returns>
        Public Function GetFieldInfo(ByVal sectionId As String, ByVal fieldId As String) As FieldInfo
            Dim sectionInfo = GetSectionInfo(sectionId)
            If sectionInfo IsNot Nothing AndAlso sectionInfo.Fields IsNot Nothing Then
                For Each fieldInfo As FieldInfo In _
                    From fieldInfo1 In sectionInfo.Fields Where fieldInfo1.FieldId = fieldId
                    Return fieldInfo
                Next
            End If
            Return New FieldInfo()
        End Function

        ''' <summary>
        ''' Traverses the specified control collection.
        ''' </summary>
        ''' <param name="controlCollection">The control collection.</param>
        Public Sub Traverse(ByVal controlCollection As ControlCollection)
            For i As Integer = controlCollection.Count - 1 To 0 Step -1
                Dim control = controlCollection(i)
                If TypeOf control Is SectionBaseUserControl Then

                    Dim sectionControl = CType(control, SectionBaseUserControl)
                    'Get product IDs from BaseControls
                    sectionControl.GetProductIdsAction = AddressOf GetProductIds
                    sectionControl.PaymentProcessAction = AddressOf PaymentProcess
                    sectionControl.CertificationCode = AmcCertificationCode
                    sectionControl.AMCCertRecertController = AMCCertRecertController
                    sectionControl.PrintMode = False
                    Dim currentSection = GetSectionInfo(sectionControl.GetType().BaseType.Name)
                    If currentSection.IsEnabled Then
                        sectionControl.ParentModulePath = ParentModulePath
                        sectionControl.CurrentSectionInfo = currentSection
                    Else
                        sectionControl.Parent.Controls.Remove(sectionControl)
                    End If
                Else
                    If control.Controls IsNot Nothing AndAlso control.Controls.Count > 0 Then
                        Traverse(control.Controls)
                    End If
                End If
            Next
        End Sub

        ''' <summary>
        ''' Gets the form id.
        ''' </summary>
        ''' <returns></returns>
        Public Overridable Function GetFormId() As String
            Return Me.GetType().BaseType.Name
        End Function

#End Region

#Region "Event Handlers"

        ''' <summary>
        ''' Handles the Load event of the Page control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            CurrentFormInfo = ModuleConfigurationHelper.Instance.GetFormConfiguration(ModuleId,
                                                                                       Server.MapPath(ParentModulePath),
                                                                                       OrganizationId,
                                                                                       OrganizationUnitId,
                                                                                       GetFormId())
            If Not CurrentFormInfo.IsVisible Then
                Response.Redirect(NavigateURL(), True)
            End If
            AMCCertRecertController = New AmcCertRecertController(OrganizationId, OrganizationUnitId, CertificationId, Server.MapPath(ParentModulePath), MasterCustomerId, SubCustomerId)
            Dim otherModuleSetting = ModuleConfigurationHelper.Instance.GetOtherModuleSettings(Server.MapPath(ParentModulePath))
            AmcCertificationCode = New CertificationCode()
            AmcCertificationCode.CertificationCode = otherModuleSetting.CertificationCode
            AmcCertificationCode.ReCertificationCode = otherModuleSetting.RecertificationCode
            Traverse(Controls)
        End Sub
#End Region

#Region "Protected Method"

        ''' <summary>
        ''' Navigates to default control.
        ''' </summary>
        Protected Sub NavigateToDefaultControl()
            Dim navigateUrlString As String
            navigateUrlString = NavigateURL(TabId, "",
                                             {String.Format("{0}={1}",
                                                CommonConstants.USER_CONTROL_PARAMETER,
                                                CommonConstants.USER_AUTHENTICATION_CONTROL_DEFAULT)})
            Response.Redirect(navigateUrlString, True)
        End Sub

        ''' <summary>
        ''' Shows the error.
        ''' </summary>
        ''' <param name="issuesCollection">The issues collection.</param>
        ''' <param name="message">The message.</param>
        Public Sub ShowError(ByVal issuesCollection As IIssuesCollection, ByVal message As Label)
            message.Text = String.Empty
            For Each collection As IIssue In issuesCollection
                message.Text += collection.Message + "<br/>"
            Next
            issuesCollection.RemoveAll()
        End Sub

        ''' <summary>
        ''' Process payment by connect to shooping cart service, add a product on it and redirect to create order page
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub PaymentProcess(ByVal productIds As Integer())
            Dim personifyShoppingCart As PersonifyShoppingCartSoapClient = Nothing
            Dim resultMessage As PersonifyShoppingCart.Result_Message = Nothing

            Try
                Dim configFilePath = Server.MapPath(ParentModulePath)
                Dim otherModuleSetting = ModuleConfigurationHelper.Instance.GetOtherModuleSettings(configFilePath)

                'get persionify webservice account from configuration
                Dim seatInformationProvider As ISeatInformationProvider = PersonifySiteSettings.GetSeatInformation()
                Dim seatInformation As SeatInformation = seatInformationProvider.GetSeatInformation()

                Dim userName = seatInformation.Login '"harveynash"
                Dim password = seatInformationProvider.GetPassword() '"h@rv3yN@sh"


                Dim url As String = CorrectConfigurationUrl(otherModuleSetting.PersonifyShoppingCartWebService) '"http://localhost/arntest/personifyshoppingcart.asmx"
                url = ReplaceHttpsByHttp(url)

                'connect to webservice
                Dim binding = New BasicHttpBinding()
                binding.SendTimeout = New TimeSpan(0, 0, 10, 10)
                Dim enpoint = New EndpointAddress(url)
                personifyShoppingCart = New PersonifyShoppingCartSoapClient(binding, enpoint)

                resultMessage = personifyShoppingCart.Connect(userName, password, OrganizationId, OrganizationUnitId)

                If productIds IsNot Nothing Then
                    If resultMessage.Success AndAlso productIds.Count > 0 Then

                        Dim isSuccess As Boolean = True
                        Dim errorMessage As String = String.Empty

                        For Each productId As Integer In productIds
                            Dim result As Result
                            If isSuccess AndAlso productId > 0 Then
                                result = AddMainProductToCart(personifyShoppingCart, resultMessage.Token, productId)
                                If result.ItemId <= 0 Then
                                    isSuccess = False
                                    errorMessage = result.Message
                                End If
                            End If
                        Next

                        If isSuccess Then
                            'get check out url from configuration file
                            Dim checkOutUrl As String = CorrectConfigurationUrl(otherModuleSetting.CheckoutURL) '"http://localhost/arntest/ShoppingCart/OrderCreate/tabid/135/Default.aspx"
                            Response.Redirect(checkOutUrl)
                        Else
                            ShowErrorMessage(errorMessage)
                        End If

                    Else
                        ShowErrorMessage(resultMessage.Message)

                    End If
                Else
                    ShowErrorMessage("There is no product for processing payment.")
                End If

            Catch ex1 As ThreadAbortException
                'nothing here 
                'http://www.dotnetnuke.com/Community/Community-Exchange/Question/142/How-to-solve-Thread-was-being-aborted-error.aspx
            Catch ex As System.Exception
                _exceptionLogController.AddLog(ex)
                ShowErrorMessage(ex.Message)
            Finally
                If (resultMessage IsNot Nothing) AndAlso (personifyShoppingCart IsNot Nothing) Then
                    If resultMessage.Success Then
                        personifyShoppingCart.Disconnect(resultMessage.Token)
                    End If
                End If
            End Try
        End Sub

        ''' <summary>
        ''' Adds the main product to cart.
        ''' </summary>
        ''' <param name="personifyShoppingCart">The personify shopping cart.</param>
        ''' <param name="token">The token.</param>
        ''' <param name="productId">The product id.</param>
        ''' <returns></returns>
        Private Function AddMainProductToCart(ByVal personifyShoppingCart As PersonifyShoppingCartSoapClient,
                                              ByVal token As String,
                                              ByVal productId As Integer
                                              ) As Result

            Dim result As Result

            'get rate structure. This needs to be determined based on Personify Logic for the Logged in Customer
            Dim rateStructure As String
            Dim rateCode As String

            Dim productRateCodeRateStructure As PersonifyDataProvider.ProductRateCodeRateStructure

            productRateCodeRateStructure = AMCCertRecertController.GetProductReateCodeReateStructure(productId.ToString())

            If Not productRateCodeRateStructure Is Nothing Then
                rateStructure = productRateCodeRateStructure.ProductRateStructure
                rateCode = productRateCodeRateStructure.ProductRateCode
            Else
                Throw New System.Exception(String.Format("Could not found product id {0} ", productId))
            End If

            'Remove old item if exist
            personifyShoppingCart.DeleteProductFromCart(token, MasterCustomerId, SubCustomerId, productId, False)

            result = personifyShoppingCart.AddMainProductToCart(token, _
                                            MasterCustomerId, _
                                            0, _
                                            productId, _
                                            1, _
                                            MasterCustomerId, _
                                            0, _
                                            rateStructure, _
                                            rateCode, _
                                            Nothing, Nothing, Nothing, Nothing, Nothing)

            If result.ItemId <= 0 Then 'call again to make sure it can add
                result = personifyShoppingCart.AddMainProductToCart(token, _
                                                            MasterCustomerId, _
                                                            0, _
                                                            productId, _
                                                            1, _
                                                            MasterCustomerId, _
                                                            0, _
                                                            rateStructure, _
                                                            rateCode, _
                                                            Nothing, Nothing, Nothing, Nothing, Nothing)
            End If

            Return result

        End Function

        ''' <summary>
        ''' Replaces the HTTPS by HTTP.
        ''' </summary>
        ''' <param name="url">The URL.</param>
        ''' <returns></returns>
        Private Shared Function ReplaceHttpsByHttp(ByVal url As String) As String
            Dim ret As String = url
            Dim temp As String = url.ToUpper()

            If temp.StartsWith("HTTPS") Then
                ret = url.Replace("https://", "http://")
            End If

            'remove port if any
            Dim ctx = HttpContext.Current
            Dim urlWithPort = String.Format("://{0}{1}/",
                                            ctx.Request.Url.Host,
                                            If(ctx.Request.Url.Port = 80, String.Empty, ":" + ctx.Request.Url.Port.ToString())
                                            )
            Dim urlWithoutPort = String.Format("://{0}{1}/",
                                           ctx.Request.Url.Host,
                                           String.Empty
                                           )
            ret = ret.Replace(urlWithPort, urlWithoutPort)

            Return ret
        End Function

        ''' <summary>
        ''' Corrects the configuration URL.
        ''' </summary>
        ''' <param name="url">The URL.</param>
        ''' <returns></returns>
        Private Shared Function CorrectConfigurationUrl(ByVal url As String) As String
            Dim url1 = url.ToUpper().Trim()

            If url1.StartsWith("HTTP://") Or url1.StartsWith("HTTPS://") Then
                Return url
            Else

                url1 = url

                If Not url1.StartsWith("/") Then
                    url1 = "/" + url1
                End If

                'Getting the current context of HTTP request
                Dim ctx = HttpContext.Current

                'Checking the current context content
                If ctx IsNot Nothing Then
                    'Formatting the fully qualified website url/name
                    url1 = String.Format("{0}://{1}{2}{3}",
                                            ctx.Request.Url.Scheme,
                                            ctx.Request.Url.Host,
                                            If(ctx.Request.Url.Port = 80, String.Empty, ":" + ctx.Request.Url.Port.ToString()),
                                            url1)
                End If
                Return url1
            End If
        End Function

        ''' <summary>
        ''' Process unhanlde exception on page by logging error and showing a message
        ''' </summary>
        ''' <param name="unhandleException"></param>
        ''' <remarks></remarks>
        Public Sub ProcessException(ByVal unhandleException As System.Exception)
            _exceptionLogController.AddLog(unhandleException)
            ShowErrorMessage(unhandleException.Message)
        End Sub

        ''' <summary>
        ''' Gets the product ids.
        ''' </summary>
        ''' <param name="currentRecertOption">The current recert option.</param>
        ''' <returns></returns>
        Public Overridable Function GetProductIds(Optional ByVal currentRecertOption As UserDefinedSurveyQuestion = Nothing) As Integer()
            Dim otherSettings = ModuleConfigurationHelper.Instance.GetOtherModuleSettings(Server.MapPath(ParentModulePath))
            Dim ret = New Integer() {CInt(otherSettings.RecertProductId)}
            If currentRecertOption IsNot Nothing Then
                Dim examChoice = AMCCertRecertController.GetUserDefinedCertificationApplicationExamPeriod(
                                                                            GetExamChoiceSurveyTitle(), MasterCustomerId, SubCustomerId)

                If currentRecertOption.QuestionCode = Enums.QuestionCode.RECERT_OPTION_RETAKE.ToString() Then
                    If examChoice IsNot Nothing Then
                        If examChoice.ExamProductId <= 0 And examChoice.ApplicationProductId <= 0 Then
                            ShowErrorMessage("ExamProductId and ApplicationProductId could not be null")
                        Else
                            ret = New Integer() {CInt(examChoice.ExamProductId), CInt(examChoice.ApplicationProductId)}
                        End If
                    End If
                End If
            End If
            Return ret
        End Function

        ''' <summary>
        ''' Gets the exam choice survey title.
        ''' </summary>
        ''' <returns></returns>
        Public MustOverride Function GetExamChoiceSurveyTitle() As String
        ''' <summary>
        ''' Shows the error message.
        ''' </summary>
        ''' <param name="message">The message.</param>
        Public MustOverride Sub ShowErrorMessage(ByVal message As String)
        ''' <summary>
        ''' Sets the current re cert option.
        ''' </summary>
        ''' <param name="questionId">The question id.</param>
        Public MustOverride Sub SetCurrentReCertOption(ByVal questionId As String)
        ''' <summary>
        ''' Gets the current re cert option.
        ''' </summary>
        ''' <returns></returns>
        Public MustOverride Function GetCurrentReCertOption() As UserDefinedSurveyQuestion
#End Region

    End Class
End Namespace