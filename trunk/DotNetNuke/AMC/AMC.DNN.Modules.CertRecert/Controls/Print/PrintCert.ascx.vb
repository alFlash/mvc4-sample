Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Business.Controller
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems

Imports TIMSS.API.CertificationInfo

Namespace Controls.Print
    Public Class PrintCert
        Inherits PrintBaseUserControl

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            CheckForCertifications()
            AMCCertRecertController = New AmcCertRecertController(OrganizationId, OrganizationUnitId, CertificationId, Server.MapPath(ParentModulePath), MasterCustomerId, SubCustomerId)
            InitializeComponents()
        End Sub

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
            Try
                ResequenceSections()
                If Page.Validators IsNot Nothing Then
                    For i As Integer = Page.Validators.Count - 1 To 0 Step -1
                        Dim validator = CType(Page.Validators(i), Control)
                        validator.Parent.Controls.Remove(validator)
                    Next
                End If

            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Public Overrides Function GetFormId() As String
            Return "CertUC"
        End Function

        ''' <summary>
        ''' Checks for certifications.
        ''' </summary>
        Private Sub CheckForCertifications()
            ''Check applicant status, create a certification record if needed
            Try
                If String.IsNullOrEmpty(Request.QueryString(CommonConstants.CERTIFICATION_ID_QUERY_STRING)) Then
                    If (Not UserInfo.IsSuperUser AndAlso IsPersonifyWebUserLoggedIn) OrElse
                    (UserInfo.IsSuperUser AndAlso Not String.IsNullOrEmpty(Me.MasterCustomerId)) Then
                        Dim certicationCustomerCertication As ICertificationCustomerCertification = Nothing
                        Dim applicantStatus =
                            AMCCertRecertController.GetApplicantStatus(AmcCertificationCode,
                                                                       MasterCustomerId,
                                                                       SubCustomerId,
                                                                       certicationCustomerCertication)
                        If applicantStatus = ApplicantStatusEnum.CreateNewCertfication AndAlso
                            certicationCustomerCertication Is Nothing Then
                            ''Create a new Certification for current user
                            Dim newCertification =
                                (AMCCertRecertController.GetCertificationCustomerCertifications(
                                 AmcCertificationCode.CertificationCode,
                                 CertificationTypeEnum.CERTIFICATION.ToString(),
                                 MasterCustomerId,
                                 SubCustomerId)).CreateNew()
                            With newCertification
                                .IsNewObjectFlag = True
                                .MasterCustomerId = MasterCustomerId
                                .SubCustomerId = SubCustomerId
                                .CertificationCode = .CertificationCodeList(AmcCertificationCode.CertificationCode).ToCodeObject()
                            End With
                            Dim result =
                                AMCCertRecertController.InsertCertificationCustomerCertification(newCertification)
                            If (result.Count < 1) Then
                                CurrentCertificationCustomerCertification = newCertification
                                CertificationId = CurrentCertificationCustomerCertification.CertificationId
                            Else
                                NavigateToDefaultControl()
                            End If
                        Else
                            If applicantStatus = ApplicantStatusEnum.EditCertification AndAlso
                                certicationCustomerCertication IsNot Nothing Then
                                ''set certification id
                                CurrentCertificationCustomerCertification = certicationCustomerCertication
                                CertificationId = CurrentCertificationCustomerCertification.CertificationId
                            Else
                                ''don't allow user continue on this
                                NavigateToDefaultControl()
                            End If
                        End If
                    End If
                End If

                
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

    End Class
End Namespace