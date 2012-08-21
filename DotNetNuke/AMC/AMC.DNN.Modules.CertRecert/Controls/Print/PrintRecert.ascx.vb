Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Business.Controller
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Data.Enums
Imports AMC.DNN.Modules.CertRecert.Business.Helpers

Imports TIMSS.API.CertificationInfo
Imports TIMSS.API.User.UserDefinedInfo

Namespace Controls.Print
    Public Class PrintRecert
        Inherits PrintBaseUserControl

        Public Overrides Function ReferencAndVeryficationSurveyTitle() As String
            Return DataAccessConstants.RECERT_REFERENCE_VERIFICATION_SURVEY_TITLE
        End Function

        Public ReadOnly Property RecertOpt0BypassPayment() As Boolean
            Get
                Return False
                'Return ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(ValidationRuleId.RECERT_OPTION0_BYPASS_PAYMENT.ToString(), Server.MapPath(ParentModulePath))
            End Get
        End Property

        Public ReadOnly Property IsByPassPaymentProcess As Boolean
            Get
                Dim currentRecertOption = GetCurrentReCertOption()
                If currentRecertOption IsNot Nothing AndAlso currentRecertOption.QuestionCode = Enums.QuestionCode.RECERT_OPTION_RETAKE.ToString() AndAlso RecertOpt0BypassPayment Then
                    Return True
                End If
                Return False
            End Get
        End Property

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                CheckForReCertification()
                AMCCertRecertController = New AmcCertRecertController(OrganizationId, OrganizationUnitId, CertificationId, Server.MapPath(ParentModulePath), MasterCustomerId, SubCustomerId)
                UpdateCurrentReCertOption()
                InitializeComponents()
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Public Overrides Sub InitializeComponents()
            Dim printRecertContainer = FindControl("printContainer")
            For Each sectionControl As Control In printRecertContainer.Controls
                If sectionControl IsNot Nothing AndAlso TypeOf sectionControl Is SectionBaseUserControl Then
                    Dim tabControl As SectionBaseUserControl
                    tabControl = CType(sectionControl, SectionBaseUserControl)
                    tabControl.ModuleConfiguration = ModuleConfiguration
                    tabControl.LocalResourceFile = LocalResourceFile
                    tabControl.CertificationId = CertificationId
                    tabControl.CurrentCertificationCustomerCertification = CurrentCertificationCustomerCertification
                    tabControl.CurrentFormInfo = CurrentFormInfo
                    tabControl.ShowErrorMessage = AddressOf ShowErrorMessage
                    tabControl.GetCurrentReCertOptionAction = AddressOf GetCurrentReCertOption
                    tabControl.AMCCertRecertController = AMCCertRecertController
                    tabControl.ReferencAndVeryficationSurveyTitle = ReferencAndVeryficationSurveyTitle()
                    tabControl.PrintMode = True
                End If
            Next
        End Sub

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
            Try
                UpdateTabControlStatus()
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
            Return "ReCertUC"
        End Function

        ''' <summary>
        ''' Checks for re certification.
        ''' </summary>
        Private Sub CheckForReCertification()
            ''Set hard code for certification id
            If String.IsNullOrEmpty(Request.QueryString(CommonConstants.CERTIFICATION_ID_QUERY_STRING)) Then
                If Not UserInfo.IsSuperUser AndAlso IsPersonifyWebUserLoggedIn OrElse
                    (UserInfo.IsSuperUser AndAlso Not String.IsNullOrEmpty(Me.MasterCustomerId)) Then
                    Dim certicationCustomerCertication As ICertificationCustomerCertification = Nothing
                    Dim applicantStatus =
                        AMCCertRecertController.GetApplicantStatus(AmcCertificationCode,
                                                                   MasterCustomerId,
                                                                   SubCustomerId,
                                                                   certicationCustomerCertication)
                    If applicantStatus = ApplicantStatusEnum.AllowApplyForRecertification AndAlso
                                                            certicationCustomerCertication IsNot Nothing Then
                        CurrentCertificationCustomerCertification = certicationCustomerCertication
                        CertificationId = CurrentCertificationCustomerCertification.CertificationId
                    Else
                        NavigateToDefaultControl()
                    End If
                End If
            End If

        End Sub

        Private Sub UpdateTabControlStatus()
            Dim currentReCertOption = GetCurrentReCertOption()
            If currentReCertOption IsNot Nothing AndAlso currentReCertOption.QuestionCode = Enums.QuestionCode.RECERT_OPTION_RETAKE.ToString() Then
                sectionReferenceAndVerificationUC.Visible = False
                sectionLicensure.Visible = False
                sectionControlSubstanceAuthorizationUC.Visible = False
                sectionBoardCertificationUC.Visible = False
                sectionStandardQuestionaireUC.Visible = False
                sectionEducationCourseUC.Visible = False
                sectionCategoryCertifiedCME.Visible = False
                sectionCommunityServicePresentation.Visible = False
                sectionCommunityServicePubication.Visible = False
                sectionCommunityServiceReview.Visible = False
                sectionCommunityServiceVolunteerLeaderShip.Visible = False
                sectionCommunityServiceVolunteerService.Visible = False
                sectionPracticeExperienceDetailsUC.Visible = False
                sectionProgramProjectActivities.Visible = False
                sectionResearch.Visible = False
                sectionOrganizationInvolvement.Visible = False
                sectionContinuingEducation.Visible = False
                sectionSummary.Visible = False
                sectionPublication.Visible = False
                sectionTeachingPresentation.Visible = False
                sectionSupervisorUC.Visible = Not RecertOpt0BypassPayment
                sectionRecertEligibilityUC.Visible = Not RecertOpt0BypassPayment
                sectionRegistrationUC.Visible = Not RecertOpt0BypassPayment
            End If
        End Sub

        Private Sub UpdateCurrentReCertOption()

            Dim survey = Business.Controller.AmcCertRecertController.GetSurveyByTitle(DataAccessConstants.RECERTIFICATION_OPTION_SERVEY_TITLE,
                                                                                      OrganizationId, OrganizationUnitId)
            Dim questions = CType(survey, UserDefinedSurvey).UserDefinedSurveyQuestions
            Dim responses As IUserDefinedCustomerSurveyResponsees = CType(survey, UserDefinedSurvey).UserDefinedCustomerSurveyResponsees
            If responses IsNot Nothing AndAlso responses.Count > 0 Then
                For Each userdefinedSurveyResponse As UserDefinedCustomerSurveyResponses In responses
                    If userdefinedSurveyResponse.MasterCustomerId = MasterCustomerId Then
                        For Each userDefinedSurveyQuestion As UserDefinedSurveyQuestion In questions
                            If userdefinedSurveyResponse.QuestionId = userDefinedSurveyQuestion.QuestionId _
                                AndAlso userDefinedSurveyQuestion.QuestionCode <> Enums.QuestionCode.RECERT_OPTION_IF_PASS_EXAM.ToString() _
                                AndAlso userDefinedSurveyQuestion.QuestionCode <> Enums.QuestionCode.RECERT_OPTION_INCLUDE_MYNAME_ONLIST.ToString() Then

                                SetCurrentReCertOption(userDefinedSurveyQuestion.QuestionId.ToString())
                                Exit For
                            End If
                        Next
                    End If
                Next
            End If
        End Sub

    End Class
End Namespace