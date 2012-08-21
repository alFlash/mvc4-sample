

Namespace Enums
    Public Enum CustomerObjectEnum
        CONTRACT
        CUSEDUCATION
        CUSTOMER_SURVEY
        PRACTICESCOPE
    End Enum

    Public Class Enums
        Public Enum QuestionType
            YESNO
            MULTI
            RANGE
        End Enum

        Public Enum AnswerType
            YES
            NO
        End Enum

        Enum CardType
            MasterCard = 1
            Visa = 2
            Discover = 3
            AmericanExpress = 4
        End Enum

        Enum InactiveFlowAprovedStatus
            AwaitingAssociationStaffSubmit = 1
            AwaitingAccountTeamAprovedOrRejected = 2
            AccountTeamAproved = 3
            AccountTeamRejected = 4
        End Enum

        Enum CertType
            InactiveStatus = 1
            Certification = 2
            ReCertification = 3
        End Enum

        Enum UserType
            ExistUser = 1
            NewUser = 2
        End Enum

        Enum QuestionCode
            'recert option
            RECERT_OPTION_RETAKE
            RECERT_OPTION_2
            RECERT_OPTION_3
            RECERT_OPTION_IF_PASS_EXAM
            RECERT_OPTION_INCLUDE_MYNAME_ONLIST
            NONE

            'qualifying event
            QUAL_EVENT_ADOPTION_OF_CHILD
            QUAL_EVENT_LEAVING_PAID_EMPLOYMENT
            QUAL_EVENT_MILITARY_DUTY
            QUAL_EVENT_DIVORCE
            QUAL_EVENT_ILLNESS_OF_FAMILY
            QUAL_EVENT_DEATH_OF_FAMILY_MEMBER
            QUAL_EVENT_PRIMARY_HOUSING

            'cert/recert standard questionaire
            CERT_STANDARD_QUESTIONAIRE_LICENSED
            CERT_STANDARD_QUESTIONAIRE_SYSTEM

            'exam choice
            CERT_EXAM_CHOICE

            'CERT DECLARATION
            CERT_DECLARATION_INVOLVED_IN_CARE
            CERT_DECLARATION_ACTIVE_REGISTERED
            CERT_DECLARATION_AGREEMENT
            'DEMOGRAPHICS
            CERT_DEMOGRAPHICS_REQUIRE_SPECIAL_TEST
            CERT_DEMOGRAPHICS_MESSAGE_LEFT_WITH
            CERT_DEMOGRAPHICS_HOW_YOU_HEAR_THE_EXAM

            'CATEGORY CERTIFIED CME
            CATEGORY_CERTIFIED_CME_HOURS_EARNED
            CATEGORY_CERTIFIED_CME_FELLOWSHIP

            'PROFESSIONAL PRACTICE SETTING
            CERT_PRO_PRAC_SETTING_MEDICAL_SCHOOL
            CERT_PRO_PRAC_SETTING_PRIVATE_SOLO
            CERT_PRO_PRAC_SETTING_PRIVATE_GROUP
            CERT_PRO_PRAC_SETTING_HOSPITAL_BASED
            CERT_PRO_PRAC_SETTING_OUT_PATIENT_BASED
            CERT_PRO_PRAC_SETTING_MILITARY

            'PROFESSIONAL PRACTICE QUESTIONAIRE
            CERT_PRO_PRAC_QUESTIONAIRE_FELLOWSHIP
            CERT_PRO_PRAC_QUESTIONAIRE_MONTH
            CERT_PRO_PRAC_QUESTIONAIRE_YEARS
            CERT_PRO_PRAC_QUESTIONAIRE_HOURS

            'RECERT DECLARATION
            RECERT_DECLARATION_UNDERSTANDING
            RECERT_DECLARATION_AGREEMENT

            'PROFESSIONAL PRACTICE SCOPE
            CERT_PRO_PRAC_SCOPE_UNIQUE_PATIENT

            'RECERTIFICATION CATEGORY CERTIFIED CME
            RECERT_CME_HOURS_TEN_YEARS
            RECERT_CME_HOURS_TRAINING
            RECERT_CME_HOURS_THREE_YEARS
            RECERT_CATEGORY_CME_HOURS_TRAINING
            ''Recertification professional practice setting
            RECERT_PRO_PRAC_SETTING_MEDICAL_SCHOOL
            RECERT_PRO_PRAC_SETTING_PRIVATE_SOLO
            RECERT_PRO_PRAC_SETTING_PRIVATE_GROUP
            RECERT_PRO_PRAC_SETTING_HOSPITAL_BASED
            RECERT_PRO_PRAC_SETTING_OUT_PATIENT_BASED
            RECERT_PRO_PRAC_SETTING_MILITARY
        End Enum
    End Class
End Namespace