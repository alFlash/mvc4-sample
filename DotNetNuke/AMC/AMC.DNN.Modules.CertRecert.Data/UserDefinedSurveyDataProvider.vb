Imports AMC.DNN.Modules.CertRecert.Data.Enums
Imports TIMSS.API.Core
Imports TIMSS
Imports TIMSS.Enumerations
Imports TIMSS.API.Core.Validation
Imports TIMSS.API.User.UserDefinedInfo
Imports AMC.DNN.Modules.CertRecert.Data.Exception

Partial Class PersonifyDataProvider
    Inherits DataProvider

#Region "Survey"

    ''' <summary>
    ''' Creates the survey by title.
    ''' </summary>
    ''' <param name="surveyTitle">The survey title.</param>
    ''' <returns></returns>
    Public Function CreateSurveyByTitle(ByVal surveyTitle As String, ByVal orgId As String, ByVal orgUnitId As String,
                                         ByVal masterCustomerId As String) As IIssuesCollection
        Dim surveys = GetSurveys(orgId, orgUnitId, masterCustomerId)
        Dim item = surveys.CreateNew()
        With item
            .Title = surveyTitle
            .IsNewObjectFlag = True
        End With
        surveys.Add(item)
        surveys.Validate()
        surveys.Save()
        Return surveys.ValidationIssues
    End Function

    ''' <summary>
    ''' Builds the surveys and questions.
    ''' </summary>
    ''' <param name="userDefinedSurveys">The user defined surveys.</param>
    Public Sub BuildSurveysAndQuestions(ByVal userDefinedSurveys As IUserDefinedSurveys, ByVal orgId As String,
                                         ByVal orgUnitId As String, ByVal masterCustomerId As String)
        '|ReCertification Demographics
        Const surveyTitleList =
                  "ReCertification Option Notification Survey|Certification Demographics|Certification Exam Choice Survey|Certification Standard Questionaire Survey|ReCertification Option Survey|Qualifying Event Survey|Certification Declaration Survey|Recertification Declaration Survey|Certification Professional Practice Questionnaire|Certification Professional Practice Setting|Category Certified CME|Professional Practice Scope Survey Title|Recertification Category Certified CME|Recertification Professional Practice Setting|Cert Reference And Vertification - Alternative Verification Survey|ReCert Reference And Vertification - Alternative Verification Survey"
        Dim surveyTitleArr = surveyTitleList.Split(CType("|", Char))
        For Each title As String In surveyTitleArr
            Dim survey = userDefinedSurveys.FindObject("Title", title)
            If survey Is Nothing Then
                CreateSurveyByTitle(title, orgId, orgUnitId, masterCustomerId)
                survey = userDefinedSurveys.FindObject("Title", title)
            End If
            CreateQuestions(userDefinedSurveys, CType(survey, UserDefinedSurvey), title)
        Next
    End Sub

    ''' <summary>
    ''' Creates the questions.
    ''' </summary>
    ''' <param name="userDefinedSurveys">The user defined surveys.</param>
    ''' <param name="userDefinedSurvey">The user defined survey.</param>
    ''' <param name="title">The title.</param>
    Private Sub CreateQuestions(ByRef userDefinedSurveys As IUserDefinedSurveys,
                                 ByRef userDefinedSurvey As UserDefinedSurvey, ByVal title As String)
        Select Case title
            Case DataAccessConstants.CERT_REFERENCE_VERIFICATION_SURVEY_TITLE
                CreateCertReferenceVerificationSurveyTitle(userDefinedSurveys, userDefinedSurvey)
                Exit Select
            Case DataAccessConstants.RECERT_REFERENCE_VERIFICATION_SURVEY_TITLE
                CreateReCertReferenceVerificationSurveyTitle(userDefinedSurveys, userDefinedSurvey)
                Exit Select
            Case DataAccessConstants.RECERTIFICATION_DECLARATION_SURVEY_TITLE
                CreateReCertificationDeclarationSurveyQuestions(userDefinedSurveys, userDefinedSurvey)
                Exit Select
            Case DataAccessConstants.CERT_PROFESSIONAL_PRACTICE_QUESTIONNAIRE
                CreateCertificationProfessionalPracticeQuestionaireSurveyQuestions(userDefinedSurveys, userDefinedSurvey)
                Exit Select
            Case DataAccessConstants.CERT_PROFESSIONAL_PRATICE_SETTING
                CreateCertificationProfessionalPracticeSettingSurveyQuestions(userDefinedSurveys, userDefinedSurvey)
                Exit Select
            Case DataAccessConstants.RECERT_PROFESSIONAL_PRATICE_SETTING
                CreateRecertificationProfessionalPracticeSettingSurveyQuestions(userDefinedSurveys, userDefinedSurvey)
                Exit Select
            Case DataAccessConstants.CATEGORY_CERTIFIED_CME
                CreateCategoryCertifiedCMESurveyQuestions(userDefinedSurveys, userDefinedSurvey)
                Exit Select
            Case DataAccessConstants.CERT_DEMOGRAPHICS_SURVEY_TITLE
                CreateCertificationDemoGraphicsSurveyQuestions(userDefinedSurveys, userDefinedSurvey)
                Exit Select
            Case DataAccessConstants.RECERT_DEMOGRAPHICS_SURVEY_TITLE
                CreateCertificationDemoGraphicsSurveyQuestions(userDefinedSurveys, userDefinedSurvey)
                Exit Select
            Case DataAccessConstants.CERTIFICATION_DECLARATION_SURVEY_TITLE
                CreateCertificationDeclarationSurveyQuestions(userDefinedSurveys, userDefinedSurvey)
                Exit Select
            Case DataAccessConstants.CERTIFICATION_EXAM_CHOICE_SURVEY_TITLE
                CreateCertificationExamChoiceSurveyQuestions(userDefinedSurveys, userDefinedSurvey)
                Exit Select
            Case DataAccessConstants.CERTIFICATION_STANDARD_QUESTIONAIRE_SURVEY_TITLE,
                DataAccessConstants.RECERTIFICATION_STANDARD_QUESTIONAIRE_SURVEY_TITLE
                CreateCertificationStandardQuestionaireSurveyQuestions(userDefinedSurveys, userDefinedSurvey)
                Exit Select
            Case DataAccessConstants.QUALIFYING_EVENT_SURVEY_TITLE
                CreateQualifyingEventSurveyQuestions(userDefinedSurveys, userDefinedSurvey)
                Exit Select
            Case DataAccessConstants.RECERTIFICATION_OPTION_SERVEY_TITLE
                CreateReCertificationOptionSurveyQuestions(userDefinedSurveys, userDefinedSurvey)
                Exit Select
            Case DataAccessConstants.CERTIFICATION_PROFESSIONAL_PRACTICE_SCOPE
                CreateCertificationProfessionalPracticeScopeSurveyQuestions(userDefinedSurveys, userDefinedSurvey)
            Case DataAccessConstants.RECERTIFICATION_CATEGORY_CERTIFIED_CME
                CreateRecertificationCategoryCertifiedCME(userDefinedSurveys, userDefinedSurvey)
            Case DataAccessConstants.RECERTIFICATION_OPTION_NOTIFICATION_SURVEY_TITLE
                CreateRecertificationOptionNotification(userDefinedSurveys, userDefinedSurvey)
        End Select
    End Sub

    Private Sub CreateCertReferenceVerificationSurveyTitle(ByVal userDefinedSurveys As IUserDefinedSurveys, ByVal userDefinedSurvey As UserDefinedSurvey)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "I am providing alternative verification of rehabilitation nursing experience", True, String.Empty, False)
    End Sub

    Private Sub CreateReCertReferenceVerificationSurveyTitle(ByVal userDefinedSurveys As IUserDefinedSurveys, ByVal userDefinedSurvey As UserDefinedSurvey)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "I am providing alternative verification of rehabilitation nursing experience", True, String.Empty, False)
    End Sub

    Private Sub CreateRecertificationOptionNotification(ByVal userDefinedSurveys As IUserDefinedSurveys, ByVal userDefinedSurvey As UserDefinedSurvey)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "If I pass the exam, I DO NOT wish my name to be included on the list at www.cnrn.org", True, Enums.Enums.QuestionCode.RECERT_OPTION_IF_PASS_EXAM.ToString(), True)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "When I am due for recertification, I DO NOT wish my name to be included on a list on www.cnrn.org", True, Enums.Enums.QuestionCode.RECERT_OPTION_INCLUDE_MYNAME_ONLIST.ToString(), True)
    End Sub

    Private Sub CreateRecertificationCategoryCertifiedCME(ByVal userDefinedSurveys As IUserDefinedSurveys,
                                                          ByVal userDefinedSurvey As UserDefinedSurvey)
        AddRangeQuestion(userDefinedSurveys, userDefinedSurvey, "a. Please specify the number of certified CME hours earned during the 10-year period leading up to the exam", True, Enums.Enums.QuestionCode.RECERT_CME_HOURS_TEN_YEARS.ToString(), True)
        AddRangeQuestion(userDefinedSurveys, userDefinedSurvey, "b. Please specify the number of these certified CME hours that included training in Algiatry", True, Enums.Enums.QuestionCode.RECERT_CME_HOURS_TRAINING.ToString(), True)
        AddRangeQuestion(userDefinedSurveys, userDefinedSurvey, "c. Please specify the number of certified CME hours earned during the 3 years prior to recertification", True, Enums.Enums.QuestionCode.RECERT_CME_HOURS_THREE_YEARS.ToString(), True)
        AddRangeQuestion(userDefinedSurveys, userDefinedSurvey, "d. Please specify the number of certified Category I CME hours that included training in Algiatry", True, Enums.Enums.QuestionCode.RECERT_CATEGORY_CME_HOURS_TRAINING.ToString(), True)
    End Sub

    Private Sub CreateCertificationProfessionalPracticeScopeSurveyQuestions(ByVal userDefinedSurveys As IUserDefinedSurveys, ByVal userDefinedSurvey As UserDefinedSurvey)
        AddRangeQuestion(userDefinedSurveys, userDefinedSurvey, "In a typical 1-month period, on average, what is the total number of unique patients to whom you have personally provided clinical services in Pain Medicine?", True, Enums.Enums.QuestionCode.CERT_PRO_PRAC_SCOPE_UNIQUE_PATIENT.ToString(), True)
    End Sub

    Private Sub CreateReCertificationOptionSurveyQuestions(ByVal userDefinedSurveys As IUserDefinedSurveys, ByVal userDefinedSurvey As UserDefinedSurvey)
        Dim questionId = AddMultiQuestion(userDefinedSurveys, userDefinedSurvey, "4,160 neuroscience nursing practice hours in the past 5 years and retaking the certification exam", True, Enums.Enums.QuestionCode.RECERT_OPTION_RETAKE.ToString(), False)
        AddExamPeriodAnswer(userDefinedSurveys, userDefinedSurvey, questionId, "Exam Period Answer 1")
        AddExamPeriodAnswer(userDefinedSurveys, userDefinedSurvey, questionId, "Exam Period Answer 2")
        AddExamPeriodAnswer(userDefinedSurveys, userDefinedSurvey, questionId, "Exam Period Answer 3")
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "4,160 neuroscience nursing practice hours in the past 5 years and 75 continuing education hours", True, Enums.Enums.QuestionCode.RECERT_OPTION_2.ToString(), False)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "2,500 neuroscience nursing practice hours in the past 5 years and 100 continuing education hours", True, Enums.Enums.QuestionCode.RECERT_OPTION_3.ToString(), False)
        'AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "If I pass the exam, I DO NOT wish my name to be included on the list at www.cnrn.org", True, Enums.Enums.QuestionCode.RECERT_OPTION_IF_PASS_EXAM.ToString(), True)
        'AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "When I am due for recertification, I DO NOT wish my name to be included on a list on www.cnrn.org", True, Enums.Enums.QuestionCode.RECERT_OPTION_INCLUDE_MYNAME_ONLIST.ToString(), True)
    End Sub

    Private Sub CreateQualifyingEventSurveyQuestions(ByVal userDefinedSurveys As IUserDefinedSurveys, ByVal userDefinedSurvey As UserDefinedSurvey)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "Birth or adoption of a child", True, Enums.Enums.QuestionCode.QUAL_EVENT_ADOPTION_OF_CHILD.ToString(), True)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "Leaving paid employment to care for child or other dependent", True, Enums.Enums.QuestionCode.QUAL_EVENT_LEAVING_PAID_EMPLOYMENT.ToString(), True)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "Active military duty", True, Enums.Enums.QuestionCode.QUAL_EVENT_MILITARY_DUTY.ToString(), True)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "Divorce", True, Enums.Enums.QuestionCode.QUAL_EVENT_DIVORCE.ToString(), True)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "Serious illness of self or family members", True, Enums.Enums.QuestionCode.QUAL_EVENT_ILLNESS_OF_FAMILY.ToString(), True)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "Death of a family member", True, Enums.Enums.QuestionCode.QUAL_EVENT_DEATH_OF_FAMILY_MEMBER.ToString(), True)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "Loss of primary housing due to natural disaster", True, Enums.Enums.QuestionCode.QUAL_EVENT_PRIMARY_HOUSING.ToString(), True)
    End Sub

    Private Sub CreateCertificationStandardQuestionaireSurveyQuestions(ByVal userDefinedSurveys As IUserDefinedSurveys, ByVal userDefinedSurvey As UserDefinedSurvey)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "Has your license to practice medicine in any jurisdiction ever been limited, suspended, revoked, denied, restricted, voluntarily surrendered or allowed to lapse/not renewed in lieu of disciplinary action, or have proceedings toward any of those ends ever been instituted?", True, Enums.Enums.QuestionCode.CERT_STANDARD_QUESTIONAIRE_LICENSED.ToString(), True)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "Have your clinical privileges at any hospital, healthcare facility or system ever been limited, suspended, revoked, denied, restricted, voluntarily surrendered or allowed to lapse/not renewed in lieu of disciplinary action, or have proceedings toward any of those ends ever been instituted or recommended by a medical staff committee, administrative office or governing body?", True, Enums.Enums.QuestionCode.CERT_STANDARD_QUESTIONAIRE_SYSTEM.ToString(), True)
    End Sub

    Private Sub CreateCertificationExamChoiceSurveyQuestions(ByVal userDefinedSurveys As IUserDefinedSurveys, ByVal userDefinedSurvey As UserDefinedSurvey)
        Dim questionId = AddMultiQuestion(userDefinedSurveys, userDefinedSurvey, "Certification Exam Choice", True, Enums.Enums.QuestionCode.CERT_EXAM_CHOICE.ToString(), False)
        AddExamPeriodAnswer(userDefinedSurveys, userDefinedSurvey, questionId, String.Format("Start Date: {0}, End Date: {1}, Exam Date (Application Dealine Date): {2}", New DateTime(2012, 5, 1), New DateTime(2012, 6, 30), New DateTime(2012, 7, 16)))
        AddExamPeriodAnswer(userDefinedSurveys, userDefinedSurvey, questionId, String.Format("Start Date: {0}, End Date: {1}, Exam Date (Application Dealine Date): {2}", New DateTime(2012, 5, 1), New DateTime(2012, 6, 30), New DateTime(2012, 7, 16)))
        AddExamPeriodAnswer(userDefinedSurveys, userDefinedSurvey, questionId, String.Format("Start Date: {0}, End Date: {1}, Exam Date (Application Dealine Date): {2}", New DateTime(2012, 5, 1), New DateTime(2012, 6, 30), New DateTime(2012, 7, 16)))
    End Sub

    Private Sub CreateCertificationDeclarationSurveyQuestions(ByVal userDefinedSurveys As IUserDefinedSurveys, ByVal userDefinedSurvey As UserDefinedSurvey)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "I attest that during the past five (5) years I have been actively and directly involved in the care of neuroscience patients", True, Enums.Enums.QuestionCode.CERT_DECLARATION_INVOLVED_IN_CARE.ToString(), True)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "I attest that I have been an active registered nurse (RN)", True, Enums.Enums.QuestionCode.CERT_DECLARATION_ACTIVE_REGISTERED.ToString(), True)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "I agree that the Board of Directors of association shall be the sole judge of my qualifications to receive and retain a certificate issued by association, the timeliness and completeness of my application, and my eligibility to have my name included in any list or directory in which the names of Diplomats of association are published", True, Enums.Enums.QuestionCode.CERT_DECLARATION_AGREEMENT.ToString(), True)
    End Sub

    Private Sub CreateCertificationDemoGraphicsSurveyQuestions(ByVal userDefinedSurveys As IUserDefinedSurveys, ByVal userDefinedSurvey As UserDefinedSurvey)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "Do you require a special test arrangement?", True, Enums.Enums.QuestionCode.CERT_DEMOGRAPHICS_REQUIRE_SPECIAL_TEST.ToString(), True)
        AddRangeQuestion(userDefinedSurveys, userDefinedSurvey, "Messages to be left with", True, Enums.Enums.QuestionCode.CERT_DEMOGRAPHICS_MESSAGE_LEFT_WITH.ToString(), True)
        Dim questionId As Int64 = AddMultiQuestion(userDefinedSurveys, userDefinedSurvey, "How did you hear about the Exam?", True, Enums.Enums.QuestionCode.CERT_DEMOGRAPHICS_HOW_YOU_HEAR_THE_EXAM.ToString(), True)
        AddMultiAnswer(userDefinedSurveys, CType(userDefinedSurvey, IUserDefinedSurvey), questionId, "Friend")
        AddMultiAnswer(userDefinedSurveys, CType(userDefinedSurvey, IUserDefinedSurvey), questionId, "Internet")
        AddMultiAnswer(userDefinedSurveys, CType(userDefinedSurvey, IUserDefinedSurvey), questionId, "Newspaper")
    End Sub

    Private Sub CreateCategoryCertifiedCMESurveyQuestions(ByVal userDefinedSurveys As IUserDefinedSurveys, ByVal userDefinedSurvey As UserDefinedSurvey)
        AddRangeQuestion(userDefinedSurveys, userDefinedSurvey, "Please specify the number of Category I certified CME hours earned", True, Enums.Enums.QuestionCode.CATEGORY_CERTIFIED_CME_HOURS_EARNED.ToString(), True)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "I am counting an ACGME-accredited pain management/medicine fellowship or subspecialty training program of 12 months or longer as all 50 hours of certified CME", True, Enums.Enums.QuestionCode.CATEGORY_CERTIFIED_CME_FELLOWSHIP.ToString(), True)
    End Sub

    Private Sub CreateRecertificationProfessionalPracticeSettingSurveyQuestions(ByVal userDefinedSurveys As IUserDefinedSurveys, ByVal userDefinedSurvey As UserDefinedSurvey)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "Medical School", True, Enums.Enums.QuestionCode.RECERT_PRO_PRAC_SETTING_MEDICAL_SCHOOL.ToString(), True)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "Private Practice, solo", True, Enums.Enums.QuestionCode.RECERT_PRO_PRAC_SETTING_PRIVATE_SOLO.ToString(), True)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "Private Practice, group", True, Enums.Enums.QuestionCode.RECERT_PRO_PRAC_SETTING_PRIVATE_GROUP.ToString(), True)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "Hospital-based", True, Enums.Enums.QuestionCode.RECERT_PRO_PRAC_SETTING_HOSPITAL_BASED.ToString(), True)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "Outpatient-based", True, Enums.Enums.QuestionCode.RECERT_PRO_PRAC_SETTING_OUT_PATIENT_BASED.ToString(), True)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "Military", True, Enums.Enums.QuestionCode.RECERT_PRO_PRAC_SETTING_MILITARY.ToString(), True)
    End Sub
    Private Sub CreateCertificationProfessionalPracticeSettingSurveyQuestions(ByVal userDefinedSurveys As IUserDefinedSurveys, ByVal userDefinedSurvey As UserDefinedSurvey)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "Medical School", True, Enums.Enums.QuestionCode.CERT_PRO_PRAC_SETTING_MEDICAL_SCHOOL.ToString(), True)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "Private Practice, solo", True, Enums.Enums.QuestionCode.CERT_PRO_PRAC_SETTING_PRIVATE_SOLO.ToString(), True)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "Private Practice, group", True, Enums.Enums.QuestionCode.CERT_PRO_PRAC_SETTING_PRIVATE_GROUP.ToString(), True)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "Hospital-based", True, Enums.Enums.QuestionCode.CERT_PRO_PRAC_SETTING_HOSPITAL_BASED.ToString(), True)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "Outpatient-based", True, Enums.Enums.QuestionCode.CERT_PRO_PRAC_SETTING_OUT_PATIENT_BASED.ToString(), True)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "Military", True, Enums.Enums.QuestionCode.CERT_PRO_PRAC_SETTING_MILITARY.ToString(), True)
    End Sub

    Private Sub CreateCertificationProfessionalPracticeQuestionaireSurveyQuestions(ByVal userDefinedSurveys As IUserDefinedSurveys, ByVal userDefinedSurvey As UserDefinedSurvey)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "Following completion of your primary residency training programs (eg, psychiatry, neurosurgery), if you have successfully completed an ACGME-accredited fellowship or subspecialty training program in pain management/medicine that lasted 12 months or longer, you may count that experience as up to 12 months of Clinical Practice of Pain Medicine for the purposes of this requirement. I am counting an ACGME-accredited pain management/medicine fellowship or subspecialty training program of 12 months as Clinical Practice of Pain Medicine", True, Enums.Enums.QuestionCode.CERT_PRO_PRAC_QUESTIONAIRE_FELLOWSHIP.ToString(), True)
        AddRangeQuestion(userDefinedSurveys, userDefinedSurvey, "Indicate below the number of months you were engaged in the Clinical Practice of Pain Medicine in the 24 month period ending April _, 201_", True, Enums.Enums.QuestionCode.CERT_PRO_PRAC_QUESTIONAIRE_MONTH.ToString(), True)
        AddRangeQuestion(userDefinedSurveys, userDefinedSurvey, "Total number of years in the Clinical Practice of Pain Medicine after completion of primary residency training program", True, Enums.Enums.QuestionCode.CERT_PRO_PRAC_QUESTIONAIRE_YEARS.ToString(), True)
        AddRangeQuestion(userDefinedSurveys, userDefinedSurvey, "On average, how many hours per week are you currently engaged in the Clinical Practice of Pain Medicine? Hours per week (average over the last six months)", True, Enums.Enums.QuestionCode.CERT_PRO_PRAC_QUESTIONAIRE_HOURS.ToString(), True)
    End Sub

    Private Sub CreateReCertificationDeclarationSurveyQuestions(ByRef userDefinedSurveys As IUserDefinedSurveys,
                             ByRef userDefinedSurvey As UserDefinedSurvey)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "I attest that I have been an active registered nurse (RN), directly involved in the care of neuroscience patients or in management, education, or research directly related to neuroscience nursing for at least the equivalent of 2 years’ full time work with at least 4,160 hours, during the past 5 years and I will retake the certification exam.", True, Enums.Enums.QuestionCode.RECERT_OPTION_RETAKE.ToString(), True)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "I attest that I have been an active registered nurse (RN), directly involved in the care of neuroscience patients or in management, education, or research directly related to neuroscience nursing for at least the equivalent of 2 years’ full time work with at least 4,160 hours, and earned a minimum of 75 CE hours during the past 5 years.", True, Enums.Enums.QuestionCode.RECERT_OPTION_2.ToString(), True)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "I attest that I have been an active registered nurse (RN), directly involved in the care of neuroscience patients or in management, education, or research directly related to neuroscience nursing for at least the equivalent of 2 years’ full time work with at least 2,500 hours, approximately 14.5 month’s full time work equivalent  during the past 5 years and earned a minimum of 100 CE hours.", True, Enums.Enums.QuestionCode.RECERT_OPTION_3.ToString(), True)
        AddYesNoQuestion(userDefinedSurveys, userDefinedSurvey, "I agree that the Board of Directors of association shall be the sole judge of my qualifications to receive and retain a certificate issued by association, the timeliness and completeness of my application, and my eligibility to have my name included in any list or directory in which the names of Diplomats of association are published. I hereby indemnify and hold harmless association, and its officers, directors, appointees, examiners, agents, and employees, from any demand or action based on any decision or conduct relating to my application, to the evaluation and scoring of my examination, to my certification status with association, and to the issuance or revocation of certification., True", True, Enums.Enums.QuestionCode.RECERT_DECLARATION_AGREEMENT.ToString(), True)
    End Sub

    Public Function AddRangeQuestion(ByVal userDefinedSurveys As IUserDefinedSurveys, ByVal userDefinedSurvey As UserDefinedSurvey,
                                     ByVal questionText As String, ByVal isEnbaled As Boolean, Optional ByVal questionCode As String = "", Optional ByVal candelete As Boolean = True) As IIssuesCollection
        Dim question = userDefinedSurvey.UserDefinedSurveyQuestions.CreateNew()
        'TODO: IsEnabled
        With question
            .SurveyId = userDefinedSurvey.SurveyId
            .QuestionText = questionText
            .QuestionTypeString = "RANGE"
            .CanDelete = candelete
            .Enabled = isEnbaled
            .QuestionCode = questionCode
            .IsNewObjectFlag = True
        End With
        userDefinedSurvey.UserDefinedSurveyQuestions.Add(question)
        userDefinedSurveys.Save()

        Dim yesAnswer = CType(question, UserDefinedSurveyQuestion).UserDefinedSurveyAnsweres.CreateNew()
        With yesAnswer
            .SurveyId = question.SurveyId
            .QuestionId = question.QuestionId
            .AnswerText = "RANGE"
            .AnswerTypeString = "NO"
            .IsNewObjectFlag = True
        End With
        CType(question, UserDefinedSurveyQuestion).UserDefinedSurveyAnsweres.Add(yesAnswer)
        userDefinedSurveys.Save()
        Return userDefinedSurveys.ValidationIssues
    End Function

    Private Function AddMultiQuestion(ByVal userDefinedSurveys As IUserDefinedSurveys, ByVal userDefinedSurvey As UserDefinedSurvey,
                                      ByVal questionText As String, ByVal isEnabled As Boolean, Optional ByVal questionCode As String = "", Optional ByVal candelete As Boolean = True) As Long
        Dim question = userDefinedSurvey.UserDefinedSurveyQuestions.CreateNew()
        With question
            .SurveyId = userDefinedSurvey.SurveyId
            .QuestionText = questionText
            .QuestionType = .QuestionType.List("MULTI").ToCodeObject()
            .CanDelete = candelete
            .QuestionCode = questionCode
            .Enabled = isEnabled
            .IsNewObjectFlag = True
        End With
        userDefinedSurvey.UserDefinedSurveyQuestions.Add(question)
        userDefinedSurveys.Save()
        Return question.QuestionId
    End Function

    Public Function AddYesNoQuestion(ByVal userDefinedSurveys As IUserDefinedSurveys, ByVal userDefinedSurvey As UserDefinedSurvey,
                                     ByVal questionText As String, ByVal isEnabled As Boolean, Optional ByVal questionCode As String = "", Optional ByVal candelete As Boolean = True) As IIssuesCollection
        Dim question = userDefinedSurvey.UserDefinedSurveyQuestions.CreateNew()
        'TODO: IsEnabled
        With question
            .SurveyId = userDefinedSurvey.SurveyId
            .QuestionText = questionText
            .QuestionType = .QuestionType.List("YESNO").ToCodeObject()
            .CanDelete = candelete
            .QuestionCode = questionCode
            .Enabled = isEnabled
            .IsNewObjectFlag = True
        End With
        userDefinedSurvey.UserDefinedSurveyQuestions.Add(question)
        userDefinedSurveys.Save()
        AddYesNoAnswer(userDefinedSurveys, question)
        Return userDefinedSurveys.ValidationIssues
    End Function

    Private Sub AddExamPeriodAnswer(ByVal userDefinedSurveys As IUserDefinedSurveys, ByVal survey As UserDefinedSurvey, ByVal questionId As Long, ByVal answerText As String)
        survey.UserDefinedSurveyQuestions.Fill()
        Dim userDefinedQuestion = survey.UserDefinedSurveyQuestions.FindObject("QuestionId", questionId)
        If userDefinedQuestion IsNot Nothing Then
            Dim question = CType(userDefinedQuestion, UserDefinedSurveyQuestion)
            question.UserDefinedSurveyAnsweres.Fill()
            Dim answer = question.UserDefinedSurveyAnsweres.CreateNew()
            With answer
                .SurveyId = survey.SurveyId
                .QuestionId = question.QuestionId
                .AnswerText = answerText
                .AnswerTypeString = "YES"
                .IsNewObjectFlag = True
            End With
            question.UserDefinedSurveyAnsweres.Add(answer)
            userDefinedSurveys.Validate()
            userDefinedSurveys.Save()
            Dim answerId = answer.AnswerId
            Dim examPeriods = TIMSS.Global.GetCollection(_organizationId, _organizationUnitId, NamespaceEnum.UserDefinedInfo, "UserDefinedCertificationApplicationExamPeriods")
            With examPeriods.Filter
                .Add("QuestionId", answerId.ToString())
            End With
            examPeriods.Fill()
            Dim examPeriod = CType(examPeriods, IUserDefinedCertificationApplicationExamPeriods).CreateNew()
            With examPeriod
                .QuestionId = answerId
                .StartDate = New DateTime(2012, 5, 1)
                .EndDate = New DateTime(2012, 6, 30)
                .ExamDate = New DateTime(2012, 7, 16)
                .ExamProductId = 675
                .IsNewObjectFlag = True
            End With
            examPeriods.Add(examPeriod)
            examPeriods.Save()
        End If
    End Sub
#End Region


#Region "Customer Survey"

    Public Function GetSurveyQuestionByQuestionId(ByVal surveyId As Long, ByVal questionId As Long) _
        As UserDefinedSurveyQuestion
        Dim surveyQuestions As IUserDefinedSurveyQuestions
        Dim surveyQuestionReturn As UserDefinedSurveyQuestion = Nothing
        Try
            surveyQuestions = CType(GetSurveyQuestionsBySurveyId(surveyId), IUserDefinedSurveyQuestions)
            surveyQuestionReturn = CType(surveyQuestions.FindObject("QuestionId", questionId.ToString()), 
                                          UserDefinedSurveyQuestion)
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return surveyQuestionReturn
    End Function



    Public Function GetSurveyQuestionsBySurveyId(ByVal surveyId As Long) As UserDefinedSurveyQuestions
        Dim surveyQuestions As UserDefinedSurveyQuestions = Nothing
        Try
            surveyQuestions = CType(GetObjectFromCache(Me._organizationId,
                                                       Me._organizationUnitId,
                                                       surveyId.ToString(),
                                                       Me._certificationId.ToString(), "0", 0), 
                                     UserDefinedSurveyQuestions)
            If surveyQuestions Is Nothing Then
                surveyQuestions = CType([Global].GetCollection(Me._organizationId,
                                                                 Me._organizationUnitId,
                                                                 NamespaceEnum.UserDefinedInfo,
                                                                 "UserDefinedSurveyQuestions"), 
                                         UserDefinedSurveyQuestions)
                With surveyQuestions.Filter
                    .Add("SurveyId", surveyId.ToString())
                End With
                surveyQuestions.Fill()
                StoreCacheObject(Me._organizationId,
                                 Me._organizationUnitId,
                                 surveyId.ToString(),
                                 Me._certificationId.ToString(), "0", 0, surveyQuestions)
            End If
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return surveyQuestions
    End Function

    Public Function GetCustomerSurveyResponees(ByVal surveyId As Long, ByVal masterCustomerId As String,
                                                ByVal subcustomerId As Integer) As UserDefinedCustomerSurveyResponsees
        Dim surveyResponsees As UserDefinedCustomerSurveyResponsees = Nothing
        Try
            surveyResponsees = CType(GetObjectFromCache(_organizationId, _organizationUnitId, CustomerObjectEnum.CUSTOMER_SURVEY.ToString() + surveyId.ToString(),
                                                        _certificationId.ToString(), masterCustomerId, subcustomerId), 
                                      UserDefinedCustomerSurveyResponsees)
            If surveyResponsees Is Nothing Then
                surveyResponsees = CType([Global].GetCollection(_organizationId, _organizationUnitId,
                                                                NamespaceEnum.UserDefinedInfo,
                                                                "UserDefinedCustomerSurveyResponsees"), 
                                          UserDefinedCustomerSurveyResponsees)
                With surveyResponsees.Filter
                    .Add("CertificationId", QueryOperatorEnum.Equals, _certificationId.ToString())
                    .Add("SurveyId", QueryOperatorEnum.Equals, surveyId.ToString())
                    .Add("MasterCustomerId", QueryOperatorEnum.Equals, masterCustomerId)
                    .Add("SubcustomerId", QueryOperatorEnum.Equals, subcustomerId.ToString())
                End With
                surveyResponsees.Fill()
                StoreCacheObject(_organizationId, _organizationUnitId, CustomerObjectEnum.CUSTOMER_SURVEY.ToString() + surveyId.ToString(), _certificationId.ToString(),
                                  masterCustomerId,
                                  subcustomerId,
                                  surveyResponsees)
            End If
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return surveyResponsees
    End Function

    Public Function InsertCustomerSurveyResponse(
                                                  ByRef customerSurveyResponseInput As  _
                                                     IUserDefinedCustomerSurveyResponses) As IIssuesCollection
        Dim surveyResponees As UserDefinedCustomerSurveyResponsees = Nothing
        Dim issueCollection As IIssuesCollection = Nothing
        Try
            surveyResponees = GetCustomerSurveyResponees(customerSurveyResponseInput.SurveyId,
                                                         customerSurveyResponseInput.MasterCustomerId,
                                                         customerSurveyResponseInput.SubcustomerId)
            Dim surveyRespones = CType(surveyResponees.CreateNew(), UserDefinedCustomerSurveyResponses)
            With surveyRespones
                .IsNewObjectFlag = True
                .MasterCustomerId = customerSurveyResponseInput.MasterCustomerId
                .SubcustomerId = customerSurveyResponseInput.SubcustomerId
                .CertificationId = Me._certificationId
                .SurveyId = customerSurveyResponseInput.SurveyId
                .AnswerId = customerSurveyResponseInput.AnswerId
                .QuestionId = customerSurveyResponseInput.QuestionId
                .Comments = customerSurveyResponseInput.Comments
            End With
            surveyResponees.Add(surveyRespones)
            issueCollection = surveyResponees.ValidationIssues
            customerSurveyResponseInput = surveyRespones
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try

        Return issueCollection
    End Function

    Public Function GetCustomerSurveyResponseByGuid(ByVal guid As String, ByVal surveyId As Long,
                                                     ByVal masterCustomerId As String, ByVal subCustomerId As Integer) _
        As UserDefinedCustomerSurveyResponses
        Dim surveyResponees As UserDefinedCustomerSurveyResponsees = Nothing
        Dim surveyResponesReturn As UserDefinedCustomerSurveyResponses = Nothing
        Try
            surveyResponees = GetCustomerSurveyResponees(surveyId,
                                                          masterCustomerId,
                                                          subCustomerId)

            surveyResponesReturn = CType(CType(surveyResponees, IBusinessObjectCollection).FindObject("Guid", guid), UserDefinedCustomerSurveyResponses)
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return surveyResponesReturn
    End Function

    Public Function GetCustomerSurveyResponseByQuestionId(ByVal questionId As Long, ByVal surveyId As Long,
                                                           ByVal masterCustomerId As String,
                                                           ByVal subCustomerId As Integer) _
        As UserDefinedCustomerSurveyResponses
        Dim surveyResponees As UserDefinedCustomerSurveyResponsees = Nothing
        Dim surveyResponesReturn As UserDefinedCustomerSurveyResponses = Nothing
        Try
            surveyResponees = GetCustomerSurveyResponees(surveyId,
                                                          masterCustomerId,
                                                          subCustomerId)

            For Each surveyResponseItem As UserDefinedCustomerSurveyResponses In surveyResponees
                If surveyResponseItem.SurveyId = surveyId AndAlso
                            surveyResponseItem.QuestionId = questionId Then
                    surveyResponesReturn = surveyResponseItem
                    Exit For
                End If
            Next
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return surveyResponesReturn
    End Function

    Public Function GetCustomerSurveyResponseByResponseId(ByVal responseId As Long,
                                                           ByVal surveyId As Long,
                                                           ByVal masterCustomerId As String,
                                                           ByVal subCustomerId As Integer) _
        As UserDefinedCustomerSurveyResponses

        Dim surveyResponees As UserDefinedCustomerSurveyResponsees = Nothing
        Dim surveyResponesReturn As UserDefinedCustomerSurveyResponses = Nothing
        Try
            surveyResponees = GetCustomerSurveyResponees(surveyId,
                                                          masterCustomerId,
                                                          subCustomerId)
            For Each surveyResponsesItem As UserDefinedCustomerSurveyResponses In surveyResponees
                If surveyResponsesItem.ResponseId = responseId Then
                    surveyResponesReturn = surveyResponsesItem
                    Exit For
                End If
            Next
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return surveyResponesReturn
    End Function


    Public Function DeleteCustomerSurveyResponse(
                                                  ByVal customerSurveyResponseInput As  _
                                                     IUserDefinedCustomerSurveyResponses) As IIssuesCollection
        Dim surveyResponees As UserDefinedCustomerSurveyResponsees = Nothing
        Dim surveyResponesData As IUserDefinedCustomerSurveyResponses = Nothing
        Try
            surveyResponees = GetCustomerSurveyResponees(customerSurveyResponseInput.SurveyId,
                                                          customerSurveyResponseInput.MasterCustomerId,
                                                          customerSurveyResponseInput.SubcustomerId)
            surveyResponesData = CType(CType(surveyResponees, IBusinessObjectCollection).FindObject("Guid",
                                                                                                       customerSurveyResponseInput _
                                                                                                          .Guid), 
                                        UserDefinedCustomerSurveyResponses)
            surveyResponees.Remove(surveyResponesData)

        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return surveyResponees.ValidationIssues
    End Function

    Public Function CommitCustomerSurveyResponsees(ByVal surveyId As Long,
                                                   ByVal masterCustomerId As String,
                                                   ByVal subcustomerId As Integer) As IIssuesCollection
        Dim surveyResponees As UserDefinedCustomerSurveyResponsees = Nothing
        Dim issueCollection As IIssuesCollection = Nothing
        Try
            surveyResponees = GetCustomerSurveyResponees(surveyId,
                                                         masterCustomerId,
                                                         subcustomerId)

            For i As Integer = surveyResponees.Count - 1 To 0 Step -1
                Dim question = GetQuestionById(_organizationId, _organizationUnitId, surveyResponees(i).QuestionId.ToString())
                If question Is Nothing Then
                    surveyResponees.RemoveAt(i)
                End If
            Next
            surveyResponees.Save()
            issueCollection = surveyResponees.ValidationIssues

        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return issueCollection
    End Function

    Public Sub RefreshCustomerSurveyResponsees(ByVal surveyId As Long,
                                               ByVal masterCustomerId As String,
                                               ByVal subcustomerId As Integer)

        RemoveCacheObject(Me._organizationId,
                          Me._organizationUnitId,
                          Enums.CustomerObjectEnum.CUSTOMER_SURVEY.ToString() + surveyId.ToString(),
                          Me._certificationId.ToString(),
                          masterCustomerId,
                          subcustomerId)
    End Sub

    Public Function UpdateCustomerSurveyResponse(
                                                  ByVal customerSurveyResponseInput As  _
                                                     IUserDefinedCustomerSurveyResponses) As IssuesCollection
        Dim issuesCollection As IssuesCollection = Nothing
        Dim surveyResponees As IUserDefinedCustomerSurveyResponsees = Nothing
        Dim surveyResponesData As IUserDefinedCustomerSurveyResponses = Nothing
        Try
            surveyResponees = GetCustomerSurveyResponees(customerSurveyResponseInput.SurveyId,
                                                          customerSurveyResponseInput.MasterCustomerId,
                                                          customerSurveyResponseInput.SubcustomerId)
            'surveyResponesData =
            '    CType(
            '        CType(surveyResponees, IBusinessObjectCollection).FindObject("ResponseId",
            '                                                    customerSurveyResponseInput.ResponseId), 
            '                            UserDefinedCustomerSurveyResponses)
            'surveyResponees.Remove(surveyResponesData)
            issuesCollection = CType(surveyResponees.ValidationIssues, IssuesCollection)

        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return issuesCollection
    End Function

#End Region

#Region "Survey"

    ''' <summary>
    ''' Gets the surveys.
    ''' </summary>
    ''' <param name="orgId">The org id.</param>
    ''' <param name="orgUnitId">The org unit id.</param>
    ''' <param name="masterCustomerId">The master customer id.</param>
    ''' <returns></returns>
    Public Function GetSurveys(ByVal orgId As String, ByVal orgUnitId As String, ByVal masterCustomerId As String, Optional ByVal withCache As Boolean = True) _
        As IUserDefinedSurveys
        Dim surveys = GetObjectFromCache(String.Format("{0}-{1}-{2}",
                                                         DataAccessConstants.SURVEY_COLLECTION_CACHE_KEY, orgId,
                                                         orgUnitId))
        Dim results As IUserDefinedSurveys
        If surveys Is Nothing OrElse Not withCache Then
            results = CType([Global].GetCollection(orgId, orgUnitId, NamespaceEnum.UserDefinedInfo,
                                                     "UserDefinedSurveys"), 
                             IUserDefinedSurveys)
            results.Fill()
            StoreCacheObject(String.Format("{0}-{1}-{2}",
                             DataAccessConstants.SURVEY_COLLECTION_CACHE_KEY, orgId, orgUnitId), results)
        Else
            results = CType(surveys, IUserDefinedSurveys)
        End If
        Return results
    End Function

#End Region


#Region "Question List"

    ''' <summary>
    ''' Adds the question.
    ''' </summary>
    ''' <param name="surveys">The surveys.</param>
    ''' <param name="userDefinedSurvey">The user defined survey.</param>
    ''' <param name="questionText">The question text.</param>
    ''' <param name="questionType">Type of the question.</param>
    ''' <returns></returns>
    Public Function AddQuestion(ByRef surveys As IUserDefinedSurveys, ByRef userDefinedSurvey As IUserDefinedSurvey,
                                 ByVal questionText As String, ByVal questionType As String, ByVal isEnabled As Boolean, Optional ByVal canDelete As Boolean = True) _
        As IIssuesCollection
        'TODO: isEnabled
        Dim survey = CType(userDefinedSurvey, UserDefinedSurvey)
        survey.UserDefinedSurveyQuestions.Fill()
        Dim question = survey.UserDefinedSurveyQuestions.CreateNew()
        With question
            .SurveyId = survey.SurveyId
            .QuestionText = questionText
            .QuestionType = .QuestionType.List(questionType).ToCodeObject()
            .IsNewObjectFlag = True
            .Enabled = isEnabled
            .CanDelete = canDelete
        End With
        survey.UserDefinedSurveyQuestions.Add(question)
        surveys.Validate()
        surveys.Save()
        Return surveys.ValidationIssues
    End Function

#End Region

#Region "Answers"

    ''' <summary>
    ''' Adds the yes no answer.
    ''' </summary>
    ''' <param name="userDefinedSurveyQuestion">The user defined survey question.</param>
    ''' <returns></returns>
    Public Function AddYesNoAnswer(ByRef surveys As IUserDefinedSurveys,
                                    ByRef userDefinedSurveyQuestion As IUserDefinedSurveyQuestion) As IIssuesCollection
        Dim question = CType(userDefinedSurveyQuestion, UserDefinedSurveyQuestion)
        question.UserDefinedSurveyAnsweres.Fill()
        Dim yesAnswer = question.UserDefinedSurveyAnsweres.CreateNew()
        With yesAnswer
            .SurveyId = question.SurveyId
            .QuestionId = question.QuestionId
            .AnswerText = "YES"
            .AnswerTypeString = "NO"
            .IsNewObjectFlag = True
        End With
        question.UserDefinedSurveyAnsweres.Add(yesAnswer)

        Dim noAnswer = question.UserDefinedSurveyAnsweres.CreateNew()
        With noAnswer
            .SurveyId = question.SurveyId
            .QuestionId = question.QuestionId
            .AnswerText = "NO"
            .AnswerTypeString = "NO"
            .IsNewObjectFlag = True
        End With
        question.UserDefinedSurveyAnsweres.Add(noAnswer)

        surveys.Validate()
        surveys.Save()
        Return surveys.ValidationIssues
    End Function

    ''' <summary>
    ''' Adds the multi answer.
    ''' </summary>
    ''' <param name="surveys">The surveys.</param>
    ''' <param name="userDefinedSurvey">The user defined survey.</param>
    ''' <param name="questionId">The question id.</param>
    ''' <param name="answerText">The answer text.</param>
    ''' <returns></returns>
    Public Function AddMultiAnswer(ByRef surveys As IUserDefinedSurveys, ByRef userDefinedSurvey As IUserDefinedSurvey,
                                    ByVal questionId As Long, ByVal answerText As String) _
        As IIssuesCollection
        Dim survey = CType(userDefinedSurvey, UserDefinedSurvey)
        survey.UserDefinedSurveyQuestions.Fill()
        Dim userDefinedQuestion = survey.UserDefinedSurveyQuestions.FindObject("QuestionId", questionId)
        If userDefinedQuestion IsNot Nothing Then
            Dim question = CType(userDefinedQuestion, UserDefinedSurveyQuestion)
            question.UserDefinedSurveyAnsweres.Fill()
            Dim answer = question.UserDefinedSurveyAnsweres.CreateNew()
            With answer
                .SurveyId = survey.SurveyId
                .QuestionId = question.QuestionId
                .AnswerText = answerText
                .AnswerTypeString = "YES"
                .IsNewObjectFlag = True
            End With
            question.UserDefinedSurveyAnsweres.Add(answer)
            surveys.Validate()
            surveys.Save()
            Return surveys.ValidationIssues
        End If
        Return Nothing
    End Function

#End Region


#Region "Question List"

    ''' <summary>
    ''' Updates the question.
    ''' </summary>
    ''' <param name="userDefinedSurvey">The user defined survey.</param>
    ''' <param name="userDefinedSurveyQuestion">The user defined survey question.</param>
    ''' <returns></returns>
    Public Function UpdateQuestion(ByRef surveys As IUserDefinedSurveys, ByVal userDefinedSurvey As IUserDefinedSurvey,
                                    ByVal userDefinedSurveyQuestion As IUserDefinedSurveyQuestion) As IIssuesCollection
        If userDefinedSurveyQuestion IsNot Nothing Then
            Dim survey = CType(userDefinedSurvey, UserDefinedSurvey)
            survey.UserDefinedSurveyQuestions.Fill()
            For Each question As IUserDefinedSurveyQuestion In survey.UserDefinedSurveyQuestions
                If question.QuestionId = userDefinedSurveyQuestion.QuestionId Then
                    question.QuestionText = userDefinedSurveyQuestion.QuestionText
                    Exit For
                End If
            Next
            surveys.Validate()
            surveys.Save()
            Return survey.UserDefinedSurveyQuestions.ValidationIssues
        End If
        Return Nothing
    End Function

    Public Function UpdateQuestion(ByRef surveys As IUserDefinedSurveys, ByVal userDefinedSurvey As IUserDefinedSurvey,
                                    ByVal questionId As String, ByVal isEnabled As Boolean) As IIssuesCollection
        If Not String.IsNullOrEmpty(questionId) Then
            Dim survey = CType(userDefinedSurvey, UserDefinedSurvey)
            survey.UserDefinedSurveyQuestions.Fill()
            For Each question As IUserDefinedSurveyQuestion In survey.UserDefinedSurveyQuestions
                If question.QuestionId = Convert.ToInt32(questionId) Then
                    'TODO:
                    question.Enabled = isEnabled
                    Exit For
                End If
            Next
            surveys.Validate()
            surveys.Save()
            Return survey.UserDefinedSurveyQuestions.ValidationIssues
        End If
        Return Nothing
    End Function

#End Region

#Region "Answers"

    ''' <summary>
    ''' Updates the multi answer text.
    ''' </summary>
    ''' <param name="userDefinedSurvey">The user defined survey.</param>
    ''' <param name="questionId">The question id.</param>
    ''' <param name="answerId">The answer id.</param>
    ''' <param name="answerText">The answer text.</param>
    ''' <returns></returns>
    Public Function UpdateMultiAnswerText(ByRef surveys As IUserDefinedSurveys,
                                           ByRef userDefinedSurvey As IUserDefinedSurvey, ByVal questionId As Long,
                                           ByVal answerId As Long, ByVal answerText As String) As IIssuesCollection
        Dim survey = CType(userDefinedSurvey, UserDefinedSurvey)
        survey.UserDefinedSurveyQuestions.Fill()
        Dim userDefinedQuestion = survey.UserDefinedSurveyQuestions.FindObject("QuestionId", questionId)
        If userDefinedQuestion IsNot Nothing Then
            Dim question = CType(userDefinedQuestion, UserDefinedSurveyQuestion)
            question.UserDefinedSurveyAnsweres.Fill()
            Dim userDefinedAnswer = question.UserDefinedSurveyAnsweres.FindObject("AnswerId", answerId)
            If userDefinedAnswer IsNot Nothing Then
                Dim answer = CType(userDefinedAnswer, UserDefinedSurveyAnswers)
                With answer
                    .AnswerText = answerText
                End With
                surveys.Validate()
                surveys.Save()
                Return question.UserDefinedSurveyAnsweres.ValidationIssues
            End If
        End If
        Return Nothing
    End Function

#End Region


#Region "Delete"

    ''' <summary>
    ''' Deletes the answer.
    ''' </summary>
    ''' <param name="surveys">The surveys.</param>
    ''' <param name="userDefinedSurvey">The user defined survey.</param>
    ''' <param name="questionId">The question id.</param>
    ''' <param name="answerId">The answer id.</param>
    ''' <returns></returns>
    Public Function DeleteAnswer(ByRef surveys As IUserDefinedSurveys, ByRef userDefinedSurvey As IUserDefinedSurvey,
                                  ByVal questionId As Long, ByVal answerId As Long) As IIssuesCollection
        Dim survey = CType(userDefinedSurvey, UserDefinedSurvey)
        survey.UserDefinedSurveyQuestions.Fill()
        Dim userDefinedQuestion = survey.UserDefinedSurveyQuestions.FindObject("QuestionId", questionId)
        If userDefinedQuestion IsNot Nothing Then
            Dim question = CType(userDefinedQuestion, UserDefinedSurveyQuestion)
            question.UserDefinedSurveyAnsweres.Fill()
            Dim userDefinedAnswer = question.UserDefinedSurveyAnsweres.FindObject("AnswerId", answerId)
            If userDefinedAnswer IsNot Nothing Then
                Dim answer = CType(userDefinedAnswer, UserDefinedSurveyAnswers)
                question.UserDefinedSurveyAnsweres.Remove(answer)
                surveys.Validate()
                surveys.Save()
                Return surveys.ValidationIssues
            End If
        End If
        Return Nothing
    End Function


#Region "Question List"

    ''' <summary>
    ''' Deletes the question.
    ''' </summary>
    ''' <param name="userDefinedSurvey">The user defined survey.</param>
    ''' <param name="questionId">The question id.</param>
    ''' <returns></returns>
    Public Function DeleteQuestion(ByRef surveys As IUserDefinedSurveys,
                                    ByVal userDefinedSurvey As IUserDefinedSurvey, ByVal questionId As Long) _
        As IIssuesCollection
        If userDefinedSurvey IsNot Nothing Then
            CType(userDefinedSurvey, UserDefinedSurvey).UserDefinedSurveyQuestions.Fill()
            Dim questions = CType(userDefinedSurvey, UserDefinedSurvey).UserDefinedSurveyQuestions
            Dim question = questions.FindObject("QuestionId", questionId)
            If question IsNot Nothing Then
                questions.Remove(CType(question, IUserDefinedSurveyQuestion))
                surveys.Validate()
                surveys.Save()
                Return questions.ValidationIssues
            End If
        End If
        Return Nothing
    End Function

#End Region

#Region "Survey"

    ''' <summary>
    ''' Deletes all surveys.
    ''' </summary>
    ''' <param name="userDefinedSurveys">The user defined surveys.</param>
    Public Sub DeleteAllSurveys(ByRef userDefinedSurveys As IUserDefinedSurveys)
        Dim surveys = CType(userDefinedSurveys, UserDefinedSurveys)
        For Each userDefinedSurvey As UserDefinedSurvey In surveys
            userDefinedSurvey.UserDefinedCustomerSurveyResponsees.RemoveAll()
            userDefinedSurvey.UserDefinedSurveyAnsweres.RemoveAll()
            userDefinedSurvey.UserDefinedSurveyQuestions.RemoveAll()
        Next
        surveys.RemoveAll()
        userDefinedSurveys.Save()
    End Sub

#End Region

#End Region


    ''' <summary>
    ''' Gets the surveys by id.
    ''' </summary>
    ''' <param name="surveyId">The survey id.</param>
    ''' <param name="orgId">The org id.</param>
    ''' <param name="orgUnitId">The org unit id.</param>
    ''' <returns></returns>
    Public Function GetSurveysById(ByVal surveyId As Long,
                                   ByVal orgId As String,
                                   ByVal orgUnitId As String) _
        As IUserDefinedSurveys
        Dim results As IUserDefinedSurveys
        results = CType([Global].GetCollection(orgId, orgUnitId, NamespaceEnum.UserDefinedInfo, "UserDefinedSurveys"), 
                         IUserDefinedSurveys)
        With results.Filter
            .Add("OrganizationId", orgId)
            .Add("OrganizationUnitId", orgUnitId)
            .Add("SurveyId", surveyId.ToString())
        End With
        results.Fill()
        Return results
    End Function

    ''' <summary>
    ''' Gets the survey by id.
    ''' </summary>
    ''' <param name="surveyId">The survey id.</param>
    ''' <param name="orgId">The org id.</param>
    ''' <param name="orgUnitId">The org unit id.</param>
    ''' <returns></returns>
    Public Function GetSurveyById(ByVal surveyId As Long, ByVal orgId As String, ByVal orgUnitId As String) _
        As IUserDefinedSurvey
        Dim results As IUserDefinedSurveys
        results = CType([Global].GetCollection(orgId, orgUnitId, NamespaceEnum.UserDefinedInfo, "UserDefinedSurveys"), 
                         IUserDefinedSurveys)
        With results.Filter
            .Add("OrganizationId", orgId)
            .Add("OrganizationUnitId", orgUnitId)
            .Add("SurveyId", surveyId.ToString())
        End With
        results.Fill()
        Return If(results IsNot Nothing AndAlso results.Count > 0, results(0), Nothing)
    End Function

    ''' <summary>
    ''' Gets the survey by title.
    ''' </summary>
    ''' <param name="surveyTitle">The survey title.</param>
    ''' <returns></returns>
    Public Function GetSurveyByTitle(ByVal surveyTitle As String) As IUserDefinedSurvey
        Dim surveys As UserDefinedSurveys
        Dim surveyResult As IUserDefinedSurvey = Nothing
        Try
            surveys = CType([Global].GetCollection(_organizationId, _organizationUnitId,
                                               NamespaceEnum.UserDefinedInfo,
                                               "UserDefinedSurveys"), 
                                        UserDefinedSurveys)
            With surveys.Filter
                .Add("Title", surveyTitle)
            End With
            surveys.Fill()
            If surveys IsNot Nothing AndAlso surveys.Count > 0 Then
                surveyResult = surveys(0)
            End If
        Catch ex As DataAccessException
            DataAccessExceptionHandler.HandleException(ex)
        End Try

        Return surveyResult
    End Function

    ''' <summary>
    ''' Gets the surveys by title.
    ''' </summary>
    ''' <param name="surveyTitle">The survey title.</param>
    ''' <param name="orgId">The org id.</param>
    ''' <param name="orgUnitId">The org unit id.</param>
    ''' <returns></returns>
    Public Shared Function GetSurveysByTitle(ByVal surveyTitle As String, ByVal orgId As String,
                                             ByVal orgUnitId As String) As IUserDefinedSurveys
        Dim results As IUserDefinedSurveys
        results = CType([Global].GetCollection(orgId, orgUnitId, NamespaceEnum.UserDefinedInfo, "UserDefinedSurveys"), 
                         IUserDefinedSurveys)
        With results.Filter
            .Add("OrganizationId", orgId)
            .Add("OrganizationUnitId", orgUnitId)
            .Add("Title", surveyTitle)
        End With
        results.Fill()
        ''Return If(results IsNot Nothing AndAlso results.Count > 0, results(0), Nothing)
        Return results
    End Function

    ''' <summary>
    ''' Gets the customer survey responses.
    ''' </summary>
    ''' <param name="orgId">The org id.</param>
    ''' <param name="orgUnitId">The org unit id.</param>
    ''' <param name="masterCustomerId">The master customer id.</param>
    ''' <param name="subCustomerId">The sub customer id.</param>
    ''' <param name="certId">The cert id.</param>
    ''' <param name="surveyId">The survey id.</param>
    ''' <param name="questionId">The question id.</param>
    ''' <param name="answerId">The answer id.</param>
    ''' <returns></returns>
    Public Function GetCustomerSurveyResponses(ByVal orgId As String, ByVal orgUnitId As String,
                                                ByVal masterCustomerId As String,
                                                ByVal subCustomerId As Integer,
                                                ByVal certId As Integer, ByVal surveyId As Long,
                                                ByVal questionId As Long, ByVal answerId As Long) _
        As UserDefinedCustomerSurveyResponsees
        Return Nothing
    End Function

    ''' <summary>
    ''' Adds the response.
    ''' </summary>
    ''' <param name="orgId">The org id.</param>
    ''' <param name="orgUnitId">The org unit id.</param>
    ''' <param name="masterCustomerId">The master customer id.</param>
    ''' <param name="subCustomerId">The sub customer id.</param>
    ''' <param name="certId">The cert id.</param>
    ''' <param name="surveyId">The survey id.</param>
    ''' <param name="questionId">The question id.</param>
    ''' <param name="answerId">The answer id.</param>
    ''' <returns></returns>
    Public Function AddResponse(ByVal orgId As String, ByVal orgUnitId As String, ByVal masterCustomerId As String,
                                 ByVal subCustomerId As Integer,
                                 ByVal certId As Integer, ByVal surveyId As Long, ByVal questionId As Long,
                                 ByVal answerId As Long, ByVal questionText As String) As IIssuesCollection

        Dim resultIssues As IssuesCollection = New IssuesCollection()
        _organizationId = orgId
        _organizationUnitId = orgUnitId
        _certificationId = certId
        Dim userDefinedResponses = GetCustomerSurveyResponees(surveyId, masterCustomerId, subCustomerId)
        userDefinedResponses.ValidationIssues.RemoveAll()
        Dim response = userDefinedResponses.CreateNew()
        With response
            .MasterCustomerId = masterCustomerId
            .SubcustomerId = subCustomerId
            .CertificationId = certId
            .SurveyId = surveyId
            .QuestionId = questionId
            .AnswerId = answerId
            .Comments = questionText
            .IsNewObjectFlag = True
        End With
        userDefinedResponses.Add(response)
        'End If
        userDefinedResponses.Save()
        '' change by DuyTruong: When choose answer that have been deleted by admin. User can choose another answer and didn't refresh pages
        If userDefinedResponses.ValidationIssues.Count > 0 Then
            For i As Integer = 0 To userDefinedResponses.ValidationIssues.Count - 1
                Dim iissue As IIssue = userDefinedResponses.ValidationIssues.Item(i)
                resultIssues.Add(iissue)
                userDefinedResponses.ValidationIssues.Remove(iissue)
            Next
            userDefinedResponses.Remove(response)
            userDefinedResponses.Save()
        End If
        Return resultIssues
    End Function

    ''' <summary>
    ''' Deletes the responses.
    ''' </summary>
    ''' <param name="orgId">The org id.</param>
    ''' <param name="orgUnitId">The org unit id.</param>
    ''' <param name="masterCustomerId">The master customer id.</param>
    ''' <param name="subCustomerId">The sub customer id.</param>
    ''' <param name="certificationId">The certification id.</param>
    ''' <param name="surveyTitle">The survey title.</param>
    Public Sub DeleteResponses(ByVal orgId As String, ByVal orgUnitId As String, ByVal masterCustomerId As String,
                                ByVal subCustomerId As Integer, ByVal certificationId As Integer,
                                ByVal surveyTitle As String)
        Dim userDefinedSurveys = GetSurveysByTitle(surveyTitle, orgId, orgUnitId)
        If userDefinedSurveys IsNot Nothing AndAlso userDefinedSurveys.Count > 0 Then
            For Each userDefinedSurvey As IUserDefinedSurvey In userDefinedSurveys
                Dim responses = CType(userDefinedSurvey, UserDefinedSurvey).UserDefinedCustomerSurveyResponsees
                For i As Integer = responses.Count - 1 To 0 Step -1
                    Dim currentResponse = responses(i)
                    If currentResponse.MasterCustomerId = masterCustomerId AndAlso currentResponse.SubcustomerId = subCustomerId Then
                        responses.Remove(currentResponse)
                    End If
                Next
            Next
            userDefinedSurveys.Save()
        End If
    End Sub

    ''' <summary>
    ''' Adds the multi answer.
    ''' </summary>
    ''' <param name="orgId">The org id.</param>
    ''' <param name="orgUnitId">The org unit id.</param>
    ''' <param name="userDefinedSurveys">The user defined surveys.</param>
    ''' <param name="userDefinedSurvey">The user defined survey.</param>
    ''' <param name="questionId">The question id.</param>
    ''' <param name="startDate">The start date.</param>
    ''' <param name="endDate">The end date.</param>
    ''' <param name="applicationDeadline">The application deadline.</param>
    ''' <param name="productCode">The product code.</param>
    ''' <param name="applicationProductId">The application product id.</param>
    ''' <returns></returns>
    Public Function AddMultiAnswer(ByVal orgId As String, ByVal orgUnitId As String, ByVal userDefinedSurveys As IUserDefinedSurveys,
                                   ByVal userDefinedSurvey As IUserDefinedSurvey, ByVal questionId As Long, ByVal startDate As Date,
                                   ByVal endDate As Date, ByVal applicationDeadline As Date, ByVal productCode As String,
                                   ByVal applicationProductId As String) As IIssuesCollection
        Dim results As IIssuesCollection
        results = New IssuesCollection()
        Dim survey = CType(userDefinedSurvey, UserDefinedSurvey)
        survey.UserDefinedSurveyQuestions.Fill()
        Dim userDefinedQuestion = survey.UserDefinedSurveyQuestions.FindObject("QuestionId", questionId)
        If userDefinedQuestion IsNot Nothing Then
            Dim question = CType(userDefinedQuestion, UserDefinedSurveyQuestion)
            question.UserDefinedSurveyAnsweres.Fill()
            Dim answer = question.UserDefinedSurveyAnsweres.CreateNew()
            With answer
                .SurveyId = survey.SurveyId
                .QuestionId = question.QuestionId
                .AnswerText = String.Format("Start Date: {0}, End Date: {1}, Exam Date(Application Deadline Date): {2}", startDate, endDate, applicationDeadline)
                .AnswerTypeString = "YES"
                .IsNewObjectFlag = True
            End With
            question.UserDefinedSurveyAnsweres.Add(answer)
            userDefinedSurveys.Validate()
            results = userDefinedSurveys.ValidationIssues
            If results.Count > 0 Then
                question.UserDefinedSurveyAnsweres.Remove(answer)
                Return results
            Else
                userDefinedSurveys.Save()
            End If

            If userDefinedSurveys.ValidationIssues Is Nothing OrElse userDefinedSurveys.ValidationIssues.Count <= 0 Then
                Dim answerId = answer.AnswerId
                Dim examPeriods = TIMSS.Global.GetCollection(orgId, orgUnitId, NamespaceEnum.UserDefinedInfo, "UserDefinedCertificationApplicationExamPeriods")
                With examPeriods.Filter
                    .Add("QuestionId", answerId.ToString())
                End With
                examPeriods.Fill()
                Dim examPeriod = CType(examPeriods, IUserDefinedCertificationApplicationExamPeriods).CreateNew()
                With examPeriod
                    .QuestionId = answerId
                    .StartDate = startDate
                    .EndDate = endDate
                    .ExamDate = applicationDeadline
                    .ExamProductId = If(Not String.IsNullOrEmpty(productCode), Convert.ToInt64(productCode), 0)
                    .ApplicationProductId = If(Not String.IsNullOrEmpty(applicationProductId), Convert.ToInt64(applicationProductId), 0)
                    .IsNewObjectFlag = True
                End With
                examPeriods.Add(examPeriod)
                examPeriods.Validate()
                If examPeriods.ValidationIssues.Count > 0 Then
                    For Each issue As IIssue In examPeriods.ValidationIssues
                        results.Add(issue)
                    Next
                    examPeriods.Remove(examPeriod)
                    question.UserDefinedSurveyAnsweres.Remove(answer)
                End If
                examPeriods.Save()
                userDefinedSurveys.Save()
            End If
        End If
        Return results
    End Function

    ''' <summary>
    ''' Updates the multi answer.
    ''' </summary>
    ''' <param name="orgId">The org id.</param>
    ''' <param name="orgUnitId">The org unit id.</param>
    ''' <param name="userDefinedSurveys">The user defined surveys.</param>
    ''' <param name="userDefinedSurvey">The user defined survey.</param>
    ''' <param name="questionId">The question id.</param>
    ''' <param name="answerId">The answer id.</param>
    ''' <param name="startDate">The start date.</param>
    ''' <param name="endDate">The end date.</param>
    ''' <param name="applicationDeadline">The application deadline.</param>
    ''' <param name="productCode">The product code.</param>
    ''' <param name="applicationProductId">The application product id.</param>
    ''' <returns></returns>
    Public Function UpdateMultiAnswer(ByVal orgId As String, ByVal orgUnitId As String, ByVal userDefinedSurveys As IUserDefinedSurveys,
                                      ByVal userDefinedSurvey As IUserDefinedSurvey, ByVal questionId As Long, ByVal answerId As Long,
                                      ByVal startDate As Date, ByVal endDate As Date, ByVal applicationDeadline As Date, ByVal productCode As String,
                                      ByVal applicationProductId As String) As IIssuesCollection
        Dim answer = CType(userDefinedSurvey, UserDefinedSurvey).UserDefinedSurveyAnsweres.FindObject("AnswerId", answerId)
        If answer IsNot Nothing Then
            CType(answer, UserDefinedSurveyAnswers).AnswerText = String.Format("Start Date: {0}, End Date: {1}, Exam Date (Application Dealine Date): {2}", startDate, endDate, applicationDeadline)
            userDefinedSurveys.Save()
        End If
        If userDefinedSurveys.ValidationIssues Is Nothing OrElse userDefinedSurveys.ValidationIssues.Count <= 0 Then
            Dim examPeriods = TIMSS.Global.GetCollection(orgId, orgUnitId, NamespaceEnum.UserDefinedInfo, "UserDefinedCertificationApplicationExamPeriods")
            With examPeriods.Filter
                .Add("QuestionId", answerId.ToString())
            End With
            examPeriods.Fill()
            If examPeriods.Count > 0 Then
                Dim originalExamPeriod = CType(examPeriods(0), UserDefinedCertificationApplicationExamPeriod).ExamProductId
                Dim originalApplicationProductId = CType(examPeriods(0), UserDefinedCertificationApplicationExamPeriod).ApplicationProductId
                With CType(examPeriods(0), UserDefinedCertificationApplicationExamPeriod)
                    .StartDate = startDate
                    .EndDate = endDate
                    .ExamDate = applicationDeadline
                    .ExamProductId = If(Not String.IsNullOrEmpty(productCode), Convert.ToInt64(productCode), 0)
                    .ApplicationProductId = If(Not String.IsNullOrEmpty(applicationProductId), Convert.ToInt64(applicationProductId), 0)
                End With
                examPeriods.Validate()
                If examPeriods.ValidationIssues.Count > 0 Then
                    With CType(examPeriods(0), UserDefinedCertificationApplicationExamPeriod)
                        .StartDate = startDate
                        .EndDate = endDate
                        .ExamDate = applicationDeadline
                        .ExamProductId = originalExamPeriod
                        .ApplicationProductId = originalApplicationProductId
                    End With
                Else
                    examPeriods.Save()
                End If
                Return examPeriods.ValidationIssues
            End If
        Else
            Return userDefinedSurveys.ValidationIssues
        End If
        Return Nothing
    End Function

    Public Function GetCustomerSurveyResponsesByAnswerId(ByVal orgId As String, ByVal orgUnitId As String,
                                                         ByVal userDefinedSurvey As IUserDefinedSurvey,
                                                         ByVal questionId As Long,
                                                         ByVal answerId As Long) As IUserDefinedCustomerSurveyResponsees
        Try
            Dim responses = TIMSS.Global.GetCollection(orgId, orgUnitId, NamespaceEnum.UserDefinedInfo, "UserDefinedCustomerSurveyResponsees")
            With responses.Filter
                '.Add("OrganizationId", orgId)
                '.Add("OrganizationUnitId", orgUnitId)
                .Add("QuestionId", questionId.ToString())
                .Add("AnswerId", answerId.ToString())
                .Add("SurveyId", userDefinedSurvey.SurveyId.ToString())
            End With
            responses.Fill()
            Return CType(responses, IUserDefinedCustomerSurveyResponsees)
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return Nothing
    End Function

    Public Function GetCustomerSurveyResponses(ByVal orgId As String, ByVal orgUnitId As String, ByVal userDefinedSurvey As IUserDefinedSurvey, ByVal questionId As Long) As IUserDefinedCustomerSurveyResponsees
        Try
            Dim responses = TIMSS.Global.GetCollection(orgId, orgUnitId, NamespaceEnum.UserDefinedInfo, "UserDefinedCustomerSurveyResponsees")
            With responses.Filter
                '.Add("OrganizationId", orgId)
                '.Add("OrganizationUnitId", orgUnitId)
                .Add("QuestionId", questionId.ToString())
                .Add("SurveyId", userDefinedSurvey.SurveyId.ToString())
            End With
            responses.Fill()
            Return CType(responses, IUserDefinedCustomerSurveyResponsees)
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return Nothing
    End Function

    Public Function GetQuestionById(ByVal organizationId As String, ByVal organizationUnitId As String, ByVal questionId As String) As UserDefinedSurveyQuestion
        Try
            Dim questions = TIMSS.Global.GetCollection(organizationId, organizationUnitId, NamespaceEnum.UserDefinedInfo, "UserDefinedSurveyQuestions")
            With questions.Filter
                .Add("QuestionId", questionId.ToString())
            End With
            questions.Fill()
            Dim userDefinedQuestions = CType(questions, UserDefinedSurveyQuestions)
            If userDefinedQuestions.Count > 0 Then
                Return CType(userDefinedQuestions(0), UserDefinedSurveyQuestion)
            End If
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return Nothing
    End Function

    Public Function UpdateCustomerSurveyResponse(ByVal surveyId As String, ByVal questionId As String, ByVal answerId As String,
                                                 ByVal responseId As String, ByVal comments As String,
                                                 ByVal masterCustomerId As String, ByVal subCustomerId As Integer, ByVal organizationId As String,
                                                 ByVal organizationUnitId As String, ByVal certificationId As Integer
                                                 ) As IIssuesCollection

        Dim responses = TIMSS.Global.GetCollection(organizationId, organizationUnitId, NamespaceEnum.UserDefinedInfo, "UserDefinedCustomerSurveyResponsees")
        With responses.Filter
            .Add("MasterCustomerId", masterCustomerId)
            .Add("SubCustomerId", subCustomerId.ToString())
            .Add("SurveyId", surveyId)
            .Add("QuestionId", questionId)
            .Add("AnswerId", answerId)
        End With
        responses.Fill()
        If String.IsNullOrEmpty(responseId) Then
            CreateNewResponse(responses, questionId, answerId, masterCustomerId, subCustomerId, certificationId, surveyId, comments)
        Else
            Dim currentResponse = responses.FindObject("ResponseId", Convert.ToInt64(responseId))
            If currentResponse IsNot Nothing Then
                CType(currentResponse, UserDefinedCustomerSurveyResponses).Comments = comments
            Else
                CreateNewResponse(responses, questionId, answerId, masterCustomerId, subCustomerId, certificationId, surveyId, comments)
            End If
        End If
        responses.Save()
        Return responses.ValidationIssues
    End Function

    Private Sub CreateNewResponse(ByRef responses As IBusinessObjectCollection, ByVal questionId As String, ByVal answerId As String, ByVal masterCustomerId As String, ByVal subCustomerId As Integer, ByVal certificationId As Integer, ByVal surveyId As String, ByVal comments As String)

        Dim newResponse = CType(responses, UserDefinedCustomerSurveyResponsees).CreateNew()
        With newResponse
            .IsNewObjectFlag = True
            .QuestionId = Convert.ToInt64(questionId)
            .AnswerId = Convert.ToInt64(answerId)
            .CertificationId = certificationId
            .MasterCustomerId = masterCustomerId
            .SubcustomerId = subCustomerId
            .SurveyId = Convert.ToInt64(surveyId)
            .Comments = comments
        End With
        responses.Add(newResponse)
    End Sub

    Public Function GetResponseByQuestionId(ByVal questionId As Long) As UserDefinedCustomerSurveyResponsees
        Dim responses = CType(TIMSS.Global.GetCollection(_organizationId, _organizationUnitId, NamespaceEnum.UserDefinedInfo, "UserDefinedCustomerSurveyResponsees"), UserDefinedCustomerSurveyResponsees)
        With responses.Filter
            .Add("MasterCustomerId", _masterCustomerId)
            .Add("SubCustomerId", _subCustomerId.ToString())
            .Add("QuestionId", questionId.ToString())
        End With
        responses.Fill()
        Return responses
    End Function

    Public Function GetAnswerByQuestionId(ByVal questionId As Long) As IUserDefinedSurveyAnsweres
        Dim answers = CType(TIMSS.Global.GetCollection(_organizationId, _organizationUnitId, NamespaceEnum.UserDefinedInfo, "UserDefinedSurveyAnsweres"), IUserDefinedSurveyAnsweres)
        With answers.Filter
            .Add("QuestionId", questionId.ToString())
        End With
        answers.Fill()
        Return answers
    End Function

    Public Function AddOrUpdateRecertOptionResponse(ByVal questionId As String, ByVal answerId As Long, ByVal comments As String, ByVal surveyTitle As String) As IIssuesCollection
        Dim surveys = GetSurveysByTitle(surveyTitle, _organizationId, _organizationUnitId)
        Dim updated = False
        If surveys IsNot Nothing AndAlso surveys.Count > 0 Then
            Dim survey = CType(surveys(0), UserDefinedSurvey)
            Dim responses = survey.UserDefinedCustomerSurveyResponsees
            Dim questions = survey.UserDefinedSurveyQuestions
            Dim answers = survey.UserDefinedSurveyAnsweres
            Dim currentQuestion As UserDefinedSurveyQuestion = Nothing
            Dim currentAnswer As UserDefinedSurveyAnswers = Nothing

            For Each userDefinedSurveyQuestion As UserDefinedSurveyQuestion In questions
                If userDefinedSurveyQuestion.QuestionId.ToString() = questionId Then
                    currentQuestion = userDefinedSurveyQuestion
                    Exit For
                End If
            Next

            For Each userDefinedSurveyAnswers As UserDefinedSurveyAnswers In answers
                If userDefinedSurveyAnswers.AnswerId = answerId Then
                    currentAnswer = userDefinedSurveyAnswers
                    Exit For
                End If
            Next

            If currentQuestion IsNot Nothing AndAlso currentAnswer IsNot Nothing Then
                If responses IsNot Nothing AndAlso responses.Count > 0 Then 'Update Responses
                    For Each userDefinedCustomerSurveyResponsee As UserDefinedCustomerSurveyResponses In responses
                        If userDefinedCustomerSurveyResponsee.MasterCustomerId = _masterCustomerId AndAlso userDefinedCustomerSurveyResponsee.SubcustomerId = _subCustomerId Then
                            With userDefinedCustomerSurveyResponsee
                                .QuestionId = Convert.ToInt32(questionId)
                                .AnswerId = Convert.ToInt32(answerId)
                                .Comments = comments
                            End With
                            updated = True
                        End If
                    Next
                End If
                If Not updated Then 'Add Reponse
                    'responses = New UserDefinedCustomerSurveyResponsees()
                    Dim response = responses.CreateNew()
                    With response
                        .QuestionId = Convert.ToInt32(questionId)
                        .AnswerId = Convert.ToInt32(answerId)
                        .Comments = comments
                        .IsNewObjectFlag = True
                        .CertificationId = _certificationId
                        .MasterCustomerId = _masterCustomerId
                        .SubcustomerId = _subCustomerId
                        .SurveyId = survey.SurveyId
                    End With
                    responses.Add(response)
                End If
                surveys.Save()
            Else
                Dim errorResult As New DatabaseErrorIssue("System cannot save information. The question or answer has been deleted.")
                surveys.ValidationIssues.Add(errorResult)
            End If
        End If

        Return surveys.ValidationIssues
    End Function

    Public Function AddOrUpdateResponse(ByVal questionId As String, ByVal answerId As String, ByVal comments As String, ByVal surveyTitle As String) As IIssuesCollection
        Dim surveys = GetSurveysByTitle(surveyTitle, _organizationId, _organizationUnitId)
        If surveys IsNot Nothing AndAlso surveys.Count > 0 Then
            Dim survey = CType(surveys(0), UserDefinedSurvey)
            Dim responses = survey.UserDefinedCustomerSurveyResponsees
            Dim questions = survey.UserDefinedSurveyQuestions
            Dim answers = survey.UserDefinedSurveyAnsweres
            Dim currentQuestion As UserDefinedSurveyQuestion = Nothing
            Dim currentAnswer As UserDefinedSurveyAnswers = Nothing

            For Each userDefinedSurveyQuestion As UserDefinedSurveyQuestion In questions
                If userDefinedSurveyQuestion.QuestionId.ToString() = questionId Then
                    currentQuestion = userDefinedSurveyQuestion
                    Exit For
                End If
            Next

            For Each userDefinedSurveyAnswers As UserDefinedSurveyAnswers In answers
                If userDefinedSurveyAnswers.AnswerId.ToString() = answerId Then
                    currentAnswer = userDefinedSurveyAnswers
                    Exit For
                End If
            Next
            If currentQuestion IsNot Nothing AndAlso currentAnswer IsNot Nothing Then
                Dim updated = False
                'Dim resp = responses.FindObject("QuestionId", questionId)
                If responses IsNot Nothing AndAlso responses.Count > 0 Then 'Update Responses
                    For Each userDefinedCustomerSurveyResponsee As UserDefinedCustomerSurveyResponses In responses
                        If userDefinedCustomerSurveyResponsee.QuestionId.ToString() = questionId AndAlso userDefinedCustomerSurveyResponsee.MasterCustomerId = _masterCustomerId AndAlso userDefinedCustomerSurveyResponsee.SubcustomerId = _subCustomerId Then
                            With userDefinedCustomerSurveyResponsee
                                .QuestionId = Convert.ToInt32(questionId)
                                .AnswerId = Convert.ToInt32(answerId)
                                .Comments = comments
                            End With
                            updated = True
                        End If
                    Next
                End If
                If Not updated Then 'Add Responses
                    'survey.UserDefinedCustomerSurveyResponsees = New UserDefinedCustomerSurveyResponsees()
                    Dim response = responses.CreateNew()
                    With response
                        .QuestionId = Convert.ToInt32(questionId)
                        .AnswerId = Convert.ToInt32(answerId)
                        .Comments = comments
                        .IsNewObjectFlag = True
                        .CertificationId = _certificationId
                        .MasterCustomerId = _masterCustomerId
                        .SubcustomerId = _subCustomerId
                        .SurveyId = survey.SurveyId
                    End With
                    responses.Add(response)
                End If
                surveys.Save()
            Else
                Dim errorResult As New DatabaseErrorIssue("System cannot save information. The question or answer has been deleted.")
                surveys.ValidationIssues.Add(errorResult)
            End If
        End If

        Return surveys.ValidationIssues
    End Function

    Public Function GetResponses(ByVal questionId As Long, ByVal answerId As Long) As IUserDefinedCustomerSurveyResponsees
        Dim responses = CType(TIMSS.Global.GetCollection(_organizationId, _organizationUnitId, NamespaceEnum.UserDefinedInfo, "UserDefinedCustomerSurveyResponsees"), UserDefinedCustomerSurveyResponsees)
        With responses.Filter
            .Add("MasterCustomerId", _masterCustomerId)
            .Add("SubCustomerId", _subCustomerId.ToString())
            .Add("QuestionId", questionId.ToString())
            .Add("AnswerId", answerId.ToString())
        End With
        responses.Fill()
        Return responses
    End Function
End Class
