Namespace EnumItems
    Public Class CommonConstants

#Region "Query Strings"

        Public Const USER_CONTROL_PARAMETER = "uc"
        Public Const PRINT_PAGE_PARAMETER = "display"

        Public Const USER_CONTROL_FOLDER_PATH = "Controls"
        Public Const USER_CONTROL_DEFAULT_PATH = "InactiveStatus/CertificationTypeSelection"
        Public Const USER_AUTHENTICATION_CONTROL_DEFAULT = "InactiveStatus/CertificationTypeSelection"
        Public Const APPLY_INACTIVE_STATUS_CONTROL_PATH = "InactiveStatus/ApplyInactiveStatusUC"
        Public Const CHILD_FOLDER_CONTROL_PATH = "Controls/InactiveStatus"
        Public Const CERITIFICATE_CONTROL_PATH = "Certifications/CertUC"
        Public Const RECERTIFICATE_CONTROL_PATH = "Certifications/ReCertUC"
        Public Const CONFIGURATION_FORM_PATH = "FormConfigurations/ConfigureInactiveStatusUC"
        Public Const CONFIGURATION_QUESTIONLIST_PATH = "FormConfigurations/FieldQuestionListUC"
        Public Const CONFIGURATION_CETYPESETTINGS_PATH = "FormConfigurations/CETypeSettingsUC"
        Public Const CONFIGURATION_VALIDATION_RULE_SETTINGS_PATH = "FormConfigurations/ValidationRuleSettingsUC"
        Public Const FORM_CONFIGURATIONS_PATH = "FormConfigurations/FormConfigurationsUC"
        Public Const PROCESS_CONFIGURATION_FORM_PATH = "FormConfigurations/ProcessConfigurationsUC"

        Public Const PROCESS_PRINT_CERT_FORM_PATH = "print/printcert"
        Public Const PROCESS_PRINT_RECERT_FORM_PATH = "print/printrecert"
        Public Const MASTER_CUSTOMER_ID = "MasterCustomerId"
        Public Const SUB_CUSTOMER_ID = "SubCustomerId"
        Public Const CERTIFICATION_ID_QUERY_STRING = "CertId"
        'Public Const PAYMENT_ORDER_PATH = "Payments/PayOrdersUC"
        'Public Const ISNEWUSER_PARAMETER = "isNewUer"


        Public Shared AllowPages() As String = {
                                                    USER_CONTROL_DEFAULT_PATH, _
                                                    USER_AUTHENTICATION_CONTROL_DEFAULT, _
                                                    APPLY_INACTIVE_STATUS_CONTROL_PATH, _
                                                    CHILD_FOLDER_CONTROL_PATH, _
                                                    CERITIFICATE_CONTROL_PATH, _
                                                    RECERTIFICATE_CONTROL_PATH, _
                                                    CONFIGURATION_FORM_PATH, _
                                                    CONFIGURATION_QUESTIONLIST_PATH, _
                                                    PROCESS_CONFIGURATION_FORM_PATH, _
                                                    PROCESS_PRINT_CERT_FORM_PATH, _
                                                    PROCESS_PRINT_RECERT_FORM_PATH, _
                                                    CONFIGURATION_CETYPESETTINGS_PATH, _
                                                    CONFIGURATION_VALIDATION_RULE_SETTINGS_PATH, _
                                                    FORM_CONFIGURATIONS_PATH
                                                }


#End Region

#Region "Role Name Of Personify System"

        Public Const ASSOCIATE_ADMIN_ROLE_NAME = "personifyuser"
        'AccountTeam
        Public Const ACCOUNT_TEAM_ROLE_NAME = "personifyadmin"
        'AssocationTitle
        Public Const PERSONIFY_MEMBER_ROLE_NAME = "user"
        'MemberTitle

#End Region

#Region "Configurations"

        Public Const RESOURCE_KEY_SUFFIX = ".Text"
        Public Const CONFIGURATIONS_DIRECTORY = "Documentation\configurations\"
        Public Const FORM_CONFIGURATIONS_FILE_PATH = "Documentation\configurations\FormConfigurations.xml"
        Public Const NOTIFICATION_SETTINGS_FILE_PATH = "Documentation\configurations\NotificationSettings.xml"
        Public Const CERT_AUDIT_SETTINGS_FILE_PATH = "Documentation\configurations\CertAuditSettings.xml"
        Public Const RECERT_AUDIT_SETTINGS_FILE_PATH = "Documentation\configurations\RecertAuditSettings.xml"
        Public Const FORM_CONFIGURATION_FILE_PATH = "Documentation\configurations\FormInfo.xml"
        Public Const SECTION_CONFIGURATION_FILE_PATH = "Documentation\configurations\SectionInfo.xml"
        Public Const FIELD_CONFIGURATION_FILE_PATH = "Documentation\configurations\FieldInfo.xml"
        Public Const CERTIFICATION_CODE_FILE_PATH = "Documentation\configurations\CertificationCode.xml"
        Public Const OTHER_MODULE_SETTING_FILE_PATH = "Documentation\configurations\OtherModuleSettings.xml"
        Public Const GUIDELINE_SETTING_FILE_PATH = "Documentation\configurations\AMCDescriptions.xml"
        Public Const PROGRAMTYPE_RECERT_OPTION2_SETTING_FILE_PATH = "Documentation\configurations\ProgramTypeSettingsOpt2.xml"
        Public Const CONFIGURATION_VALIDATION_RULE_SETTINGS_FILE_PATH = "Documentation\configurations\ValidationRuleSettings.xml"
        Public Const FORM_CONFIGURATIONS_FILENAME = "FormConfigurations.xml"
        Public Const NOTIFICATION_SETTINGS_FILENAME = "NotificationSettings.xml"
        Public Const OTHER_MODULE_SETTINGS_FILENAME = "OtherModuleSettings.xml"
        Public Const CERT_AUDIT_SETTINGS_FILENAME = "CertAuditSettings.xml"
        Public Const RECERT_AUDIT_SETTINGS_FILENAME = "RecertAuditSettings.xml"
        Public Const PROGRAMTYPE_OPT2_SETTINGS_FILENAME = "ProgramTypeSettingsOpt2.xml"
        Public Const VALIDATION_RULE_SETTINGS_FILENAME = "ValidationRuleSettings.xml"
        Public Const CERTIFICATION_CODE_SETTINGS_FILENAME = "CertificationCode.xml"
#End Region

#Region "Configurations Text Type"
        Public Const CONFIGURATION_LABEL_TYPE = "Label"
        Public Const CONFIGURATION_HYPERLINK_TYPE = "HyperLink"
        Public Const CONFIGURATION_BUTTON_TYPE = "Button"
        Public Const CONFIGURATION_LINK_BUTTON_TYPE = "LinkButton"
        Public Const CONFIGURATION_LABEL_TYPE_PREFIX = "lbl"
        Public Const CONFIGURATION_HYPERLINK_TYPE_PREFIX = "hl"
        Public Const CONFIGURATION_BUTTON_TYPE_PREFIX = "btn"
        Public Const CONFIGURATION_LINK_BUTTON_TYPE_PREFIX = "lbt"
#End Region

#Region "Board Certification"
        Public Const OtherBoardValue = "OTHER"
        Public Const OtherSubBoardValue = "ABMSSUBSPEC"
#End Region

#Region "Version"
        Public Const CURRENT_VERSION = "07.03.2012"
#End Region

#Region "Tab control"
        Public Const TAB_COMPLETED = "0"
        Public Const TAB_INCOMPLETED = "1"
        Public Const DATE_FORMAT = "MM/dd/yyyy"
        Public Const CERT_SELECTED_AUDIT_PROCEDURE = "usp_NOTIFY_SUBSCRIPTION_RENEWAL"
        Public Const RECERT_SELECTED_AUDIT_PROCEDURE = "usr_usp_NOTIFY_RECert_audit_selected"
#End Region

#Region "Validation Expression"
        Public Const DOUBLE_QUOTE_CHAR = Chr(34)
        Public Const EMAIL_VALIDATION = "^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w]*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$"
#End Region

#Region "Default Value"
        Public Const ISSUE_NUMBER_DEFAULT = "0"
        Public Const CEHOURS_MIN = 0
#End Region
    End Class
End Namespace
