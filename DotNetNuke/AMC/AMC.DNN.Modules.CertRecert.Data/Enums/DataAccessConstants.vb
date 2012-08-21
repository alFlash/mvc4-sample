Namespace Enums
    Public Class DataAccessConstants

#Region "Survey Title"
        Public Shared AmcSurveys() As String = {CERTIFICATION_EXAM_CHOICE_SURVEY_TITLE,
                                            CERTIFICATION_STANDARD_QUESTIONAIRE_SURVEY_TITLE,
                                            RECERTIFICATION_OPTION_SERVEY_TITLE,
                                            RECERTIFICATION_STANDARD_QUESTIONAIRE_SURVEY_TITLE,
                                            QUALIFYING_EVENT_SURVEY_TITLE,
                                            RECERTIFICATION_DECLARATION_SURVEY_TITLE,
                                            CERTIFICATION_DECLARATION_SURVEY_TITLE,
                                            CERT_DEMOGRAPHICS_SURVEY_TITLE,
                                            CERT_PROFESSIONAL_PRACTICE_QUESTIONNAIRE,
                                            CERT_PROFESSIONAL_PRATICE_SETTING,
                                            CATEGORY_CERTIFIED_CME,
                                            RECERT_PROFESSIONAL_PRATICE_SETTING}
        Public Const RECERT_PROFESSIONAL_PRATICE_SETTING = "Recertification Professional Practice Setting"
        Public Const SURVEY_COLLECTION_CACHE_KEY = "_CC_AMC_Survey"
        Public Const CERTIFICATION_EXAM_CHOICE_SURVEY_TITLE = "Certification Exam Choice Survey"
        Public Const RECERTIFICATION_OPTION_NOTIFICATION_SURVEY_TITLE = "ReCertification Option Notification Survey"
        Public Const CERTIFICATION_STANDARD_QUESTIONAIRE_SURVEY_TITLE = "Certification Standard Questionaire Survey"
        Public Const RECERTIFICATION_OPTION_SERVEY_TITLE = "ReCertification Option Survey"
        Public Const RECERTIFICATION_STANDARD_QUESTIONAIRE_SURVEY_TITLE = "Certification Standard Questionaire Survey"
        'maybe different from Certification
        Public Const QUALIFYING_EVENT_SURVEY_TITLE = "Qualifying Event Survey"
        Public Const CERTIFICATION_DECLARATION_SURVEY_TITLE = "Certification Declaration Survey"
        Public Const RECERTIFICATION_DECLARATION_SURVEY_TITLE = "Recertification Declaration Survey"
        Public Const CERT_REFERENCE_VERIFICATION_SURVEY_TITLE = "Cert Reference And Vertification - Alternative Verification Survey"
        Public Const RECERT_REFERENCE_VERIFICATION_SURVEY_TITLE = "ReCert Reference And Vertification - Alternative Verification Survey"
        Public Const CERT_DEMOGRAPHICS_SURVEY_TITLE = "Certification Demographics"
        Public Const RECERT_DEMOGRAPHICS_SURVEY_TITLE = "ReCertification Demographics"
        Public Const CERT_PROFESSIONAL_PRACTICE_QUESTIONNAIRE = "Certification Professional Practice Questionnaire"
        Public Const CERT_PROFESSIONAL_PRATICE_SETTING = "Certification Professional Practice Setting"
        Public Const CATEGORY_CERTIFIED_CME = "Category Certified CME"
        Public Const CERTIFICATION_PROFESSIONAL_PRACTICE_SCOPE = "Professional Practice Scope Survey Title"
        Public Const RECERTIFICATION_CATEGORY_CERTIFIED_CME = "Recertification Category Certified CME"
#End Region
#Region "Default Data For AMC Constraint"
        Public Const ISSUE_NUMBER_DEFAULT = "0"
#End Region

    End Class
End Namespace