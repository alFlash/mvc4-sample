Imports AMC.DNN.Modules.CertRecert.Business.Entities
Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports AMC.DNN.Modules.CertRecert.Business.Controller
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports TIMSS.API.Generated.CertificationInfo

Imports TIMSS.API.CertificationInfo

Imports Personify.ApplicationManager

Namespace Controls.InactiveStatus

    Public Class CertificationTypeSelection
        Inherits PersonifyDNNBaseForm

#Region "private Members"
        Dim _certificationCode As CertificationCode = Nothing
        Dim _isLogin As Boolean
        Dim _isUserHaveCert As Boolean
        Public Property FormConfigurations() As List(Of FormInfo)
        Public Property ParentModulePath() As String
        Private Property AmcCertRecertController As AmcCertRecertController
#End Region

#Region "Public Memebers"
        Private Property _certificationRecord() As ICertificationCustomerCertification = Nothing
        Public ReadOnly Property CertificationRecord() As ICertificationCustomerCertification
            Get
                If _certificationRecord Is Nothing AndAlso _certificationCode IsNot Nothing Then
                    AmcCertRecertController.GetApplicantStatus(_certificationCode,
                                                                   MasterCustomerId,
                                                                   SubCustomerId,
                                                                   _certificationRecord)
                End If
                Return _certificationRecord
            End Get
        End Property

        Private Property _pendingCertificationRecord() As ICertificationCustomerCertification = Nothing
        Public ReadOnly Property PendingCertificationRecord() As ICertificationCustomerCertification
            Get
                If _pendingCertificationRecord Is Nothing Then
                    _pendingCertificationRecord = AmcCertRecertController.GetPendingCertification(CertificationRecord.OrigCertificationId, MasterCustomerId, SubCustomerId)
                End If
                Return _pendingCertificationRecord
            End Get
        End Property

        Public ReadOnly Property IsValidRecert As Boolean
            Get
                If UserInfo.IsSuperUser Then
                    Return False
                ElseIf PendingCertificationRecord Is Nothing Then
                    Return False
                ElseIf PendingCertificationRecord IsNot Nothing Then
                    Return PendingCertificationRecord.ProcessExpirationDate.Date > DateTime.Now.Date
                Else
                    Return True
                End If
            End Get
        End Property
#End Region

#Region "Event hadlers"
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                InitializeConfigurations() 'dont put this function in "Not IsPostBack" scope
                AmcCertRecertController = New AmcCertRecertController(OrganizationId, OrganizationUnitId, 0, Server.MapPath(ParentModulePath), MasterCustomerId, SubCustomerId)
                If CheckDataIsAvailable() Then
                    Dim otherModuleSetting As New OtherModuleSettings
                    otherModuleSetting = ModuleConfigurationHelper.Instance.GetOtherModuleSettings(Server.MapPath(ParentModulePath))
                    _certificationCode = New CertificationCode()
                    _certificationCode.CertificationCode = otherModuleSetting.CertificationCode
                    _certificationCode.ReCertificationCode = otherModuleSetting.RecertificationCode
                    If Not IsPostBack Then
                        _isLogin = IsPersonifyWebUserLoggedIn OrElse UserInfo.IsSuperUser

                        If _isLogin = True Then
                            BuildAllLink()
                        Else ' not login
                            Dim navigateUrlString As String = String.Empty
                            navigateUrlString = NavigateURL(Me.TabId, "", {String.Format("{0}={1}", CommonConstants.USER_CONTROL_PARAMETER, "Certifications/CertUC")})
                            navigateUrlString = navigateUrlString.Replace("http://", String.Empty)
                            Dim urlEncode As String = HttpUtility.UrlEncode(navigateUrlString)
                            navigateUrlString = NavigateURL(PortalSettings.LoginTabId, "", "returnurl=" & urlEncode)
                            Response.Redirect(navigateUrlString, True)
                            hlAdminControlPanel.Visible = False
                        End If
                    End If
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        ''' <summary>
        ''' Initializes the configurations.
        ''' </summary>
        Private Sub InitializeConfigurations()
            FormConfigurations = ModuleConfigurationHelper.Instance.GetFormConfigurations(ModuleId, Server.MapPath(ParentModulePath), OrganizationId, OrganizationUnitId)
            For Each formConfiguration As FormInfo In FormConfigurations
                If formConfiguration.FormId = "CertUC" Then


                    hlrCert.Text = formConfiguration.FormValue 'Localization.GetString("ApplyforCertification.Text", LocalResourceFile)
                    rCert.Visible = formConfiguration.IsVisible AndAlso Not UserInfo.IsSuperUser
                ElseIf formConfiguration.FormId = "ReCertUC" Then
                    hlrReCert.Text = formConfiguration.FormValue 'Localization.GetString("ApplyforReCertification.Text", LocalResourceFile) '"Apply for Re-Certification"
                    rReCert.Visible = formConfiguration.IsVisible AndAlso Not UserInfo.IsSuperUser
                ElseIf formConfiguration.FormId = "ApplyInactiveStatusUC" Then
                    hlrInActive.Text = formConfiguration.FormValue 'Localization.GetString("ApplyforInactiveStatus.Text", LocalResourceFile) '"Apply for Inactive Status"
                    rInActive.Visible = formConfiguration.IsVisible AndAlso Not UserInfo.IsSuperUser
                End If
            Next
            hlAdminControlPanel.Text = Localization.GetString("AdminControlPanel.Text", LocalResourceFile)
            hlAdminControlPanel.NavigateUrl = NavigateURL(TabId, "", String.Format("uc={0}", CommonConstants.CONFIGURATION_FORM_PATH))
            If (UserInfo.IsSuperUser OrElse UserInfo.IsInRole("Administrators") OrElse UserInfo.IsInRole("CERTADMIN") OrElse UserInfo.IsInRole("Host")) Then
                hlAdminControlPanel.Visible = True
            Else
                hlAdminControlPanel.Visible = False
            End If
        End Sub

#End Region

#Region "Private Methods"
        Private Sub VisibleInvisibleLink(ByVal showInactiveStatusLink As Boolean,
                                         ByVal showCertLink As Boolean,
                                         ByVal showCertLable As Boolean,
                                         ByVal showRecertLabel As Boolean,
                                         ByVal showRecertLink As Boolean)
            hlrInActive.Visible = showInactiveStatusLink AndAlso Not UserInfo.IsSuperUser
            hlrCert.Visible = showCertLink AndAlso Not UserInfo.IsSuperUser
            lblCertIsProcess.Visible = showCertLable

            'todo: check for PENDING recert record

            hlrReCert.Visible = showRecertLink AndAlso Not UserInfo.IsSuperUser AndAlso IsValidRecert
            lblRecertIsProcess.Visible = showRecertLabel
        End Sub

        Public Sub BuildAllLink()

            Dim certicationCustomerCertication As ICertificationCustomerCertification = Nothing
            Dim applicantStatus =
                amcCertRecertController.GetApplicantStatus(_certificationCode, MasterCustomerId, SubCustomerId,
                                                            certicationCustomerCertication)
            Select Case applicantStatus
                Case ApplicantStatusEnum.CreateNewCertfication, ApplicantStatusEnum.EditCertification
                    VisibleInvisibleLink(False, True, False, False, False)
                Case ApplicantStatusEnum.AllowApplyForRecertification
                    VisibleInvisibleLink(True, False, False, False, True)
                Case ApplicantStatusEnum.CertificationIsBeingProcess
                    VisibleInvisibleLink(False, True, False, False, False)
                Case ApplicantStatusEnum.RecertificationIsBeingProcess
                    VisibleInvisibleLink(True, False, False, True, False)
            End Select
            hlrInActive.NavigateUrl = NavigateURL(TabId, "", String.Format("{0}={1}", CommonConstants.USER_CONTROL_PARAMETER, CommonConstants.APPLY_INACTIVE_STATUS_CONTROL_PATH))
            hlrCert.NavigateUrl = NavigateURL(TabId, "", String.Format("{0}={1}", CommonConstants.USER_CONTROL_PARAMETER, CommonConstants.CERITIFICATE_CONTROL_PATH))
            hlrReCert.NavigateUrl = NavigateURL(TabId, "", String.Format("{0}={1}", CommonConstants.USER_CONTROL_PARAMETER, CommonConstants.RECERTIFICATE_CONTROL_PATH))
        End Sub

        Private Function CheckDataIsAvailable() As Boolean
            Dim result = AmcCertRecertController.CheckDataIsAvailable(Me.MasterCustomerId)
            If result Then
                Dim otherModuleSetting As New OtherModuleSettings
                otherModuleSetting = ModuleConfigurationHelper.Instance.GetOtherModuleSettings(Server.MapPath(ParentModulePath))
                If otherModuleSetting Is Nothing OrElse
                    String.IsNullOrEmpty(otherModuleSetting.CertificationCode) OrElse
                    String.IsNullOrEmpty(otherModuleSetting.RecertificationCode) Then
                    result = False
                End If
            End If
            If Not result Then
                VisibleInvisibleLink(False, False, False, False, False)
                Me.certificationTypeSelectionError.Visible = True
            End If
            Return result
        End Function
#End Region
    End Class
End Namespace