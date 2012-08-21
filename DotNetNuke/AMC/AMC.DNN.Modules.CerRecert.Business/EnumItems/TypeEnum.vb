

Namespace EnumItems
    Public Enum ValidationRuleId
        RECERTOPTIONRULE1
        RECERTOPTIONRULE2
        RECERTOPTIONRULE3
        RECERT_CATEGORY_CERTIFIED_CME
        DECLARATION_ALL_QUESTION_COMPLETED
        LICENSURE_AT_LEAST_ONE_ROW
        CERT_CATEGORY_CME_HOURS
        CERT_PRO_PRACTICE_EXP_TWO_MONTH_GAP
        CERT_EDUCATION_TWO_MONTH_GAP
        CERT_PRO_QUESTIONNAIRE_VALID_MONTH
        CERT_EDU_ROW_REQUIRED
        RECERT_CON_EDU_COMMON_CALCULATOR
        RECERT_SUMMARY_COMMON_CALCULATOR
        RECERT_OPTION0_BYPASS_PAYMENT
        RECERT_CON_EDU_ARN
        RECERT_EDU_COURSE_EQUAL_ARN
        RECERT_EDU_COURSE_GREATER_ARN
        RECERT_TEACH_PRESENTATION_EQUAL_ARN
        RECERT_TEACH_PRESENTATION_GREATER_ARN
        RECERT_PUBLICATION_EQUAL_ARN
        RECERT_PUBLICATION_GREATER_ARN
        RECERT_COM_SERVICE_ARN
        RECERT_SET_DEFAULT_FORM_E_ARN
        RECERT_SUMMARY_EQUAL_ARN
        RECERT_SUMMARY_GREATER_ARN
        RECERT_SUMMARY_ARN
        RECERT_PUBLICATION_MIM_MAX_ABNN
        RECERT_ORGAN_INVOL_MIM_MAX_ABNN
        DEMOGRAPHIC_READONLY
        RECERT_PRO_PROJ_ACTIVITY_MIM_MAX_ABNN
        RECERT_RESEARCH_MIM_MAX_ABNN
        RECERT_TEACH_PRESENTATION_MIM_MAX_ABNN
        CERT_INVISIBLE_LICENSURE_IN_DECLARATION
        RECERT_INVISIBLE_LICENSURE_IN_DECLARATION
        COMMUNITY_SERVICE_PUBLICATION_OPTIONAL
        COMMUNITY_SERVICE_PRESENTATION_OPTIONAL
        COMMUNITY_SERVICE_REVIEW_OPTIONAL
        COMMUNITY_SERVICE_LEADERSHIP_OPTIONAL
        COMMUNITY_SERVICE_VOLUNTEER_SERVICE_OPTIONAL
        CONTINUING_EDUCATION_OPTIONAL
        PROFESSIONAL_PUBLICATIONS_OPTIONAL
        PRESENTATIONS_OPTIONAL
        ACADEMIC_COURSEWORK_OPTIONAL
    End Enum
    Public Enum QuestionTypeString
        YESNO
        RANGE
        MULTI
    End Enum
    Public Enum AnswerTextDefault
        YES
        NO
        RANGE
    End Enum

    Public Enum ActivityProgramType
        COMMSERVR
        COMMSERVVL
        COMMSERVVS
        CONTEDUCATION
        EDUCATION
        ORGINVOLVE
        PRACTICEEXP
        PROGPROJECT
        PUBLICATION
        RESEARCH
        TEACHINGPRESN
        COMMSERVPRES
        COMMSERVPUB
        SUMMARY
    End Enum

    Public Enum CustomerEducationEnum
        CUSEDUCATION
    End Enum

    Public Enum CEType
        [CLASS]
        [OTHER]
    End Enum

    Public Enum DocumentationType
        LICENSURE
        BRDCRT
        CSAI
        TREATMENT
        QUESTIONNAIRE
        OTHER
        EDUCATION_UC
        REFERENCES
        PRO_PRAC_QUESTIONNAIRE
        PRO_PRAC_EXPERIENCE
        REFERENCE_VERFICATION_ALTERNATIVE
    End Enum

    Public Enum BodyType
        BOARD
        OTHER
        STATE
    End Enum

    Public Enum AddressType
        HOME
        OFFICE
    End Enum

    Public Enum PrefContactMethodEnum
        HOME
        OFFICE
    End Enum

    Public Enum CommunicationTypeEnum
        EMAIL
        FAX
        PHONE
    End Enum

    Public Enum CommunicationLocationEnum
        HOME
        OFFICE
    End Enum

    Public Enum CertificationTypeEnum
        CERTIFICATION
        RECERTIFICATION
    End Enum

    Public Enum CertificationStatusCodeEnum
        PENDING
        NOT_APPROVED
        UNDER_REVIEW
        COMPLETED
    End Enum

    Public Enum ApplicantStatusEnum
        CreateNewCertfication
        EditCertification
        AllowApplyForRecertification
        CertificationIsBeingProcess
        RecertificationIsBeingProcess
    End Enum

    Public Enum CustomerContactTypeEnum
        REFERENCES
        SUPERVISOR
    End Enum
    Public Enum CustomerContactClassEnum
        CRRN
        PROFESSIONAL
        SUPERVISOR
    End Enum
    Public Enum YesNoAnwserType
        YES
        NO
    End Enum

    Public Enum AuditProcessRunningType
        CertificationAudit
        LastCertificationAudit
        RecertificationAudit
        LastRecertificationAudit
    End Enum

    Public Enum InstitutionName
        Undergraduate
        Masters
        Residency
        Fellowship
    End Enum

End Namespace