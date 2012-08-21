Namespace Entities
    Public Class OtherModuleSettings
        Public Property CertificationCode() As String
        Public Property RecertificationCode() As String
        Public Property InactiveStatusProductCode() As String
        Public Property MembershipLink() As String
        Public Property LicensureValidityDate() As DateTime
        Public Property RecertProductId() As Integer
        Public Property InactiveStatusProductId() As Integer
        Public Property PersonifyShoppingCartWebService() As String
        Public Property CheckoutURL() As String
        Public Property ReCertCMEQuestion1() As String '300
        Public Property ReCertCMEQuestion2() As String '150
        Public Property ReCertCMEQuestion3() As String '100
        Public Property ReCertCMEQuestion4() As String '50
        Public Property ProPracticeQuestionaireValidateMonth As String
        Public Property ProfessionalPracticeQuestionaireStartYear As Int64?
        Public Property ProfessionalPracticeQuestionaireEndYear As Int64?
        Public Property CMEHourEarned() As Int64?
        Public Property CMEStartYearRange() As Int64?
        Public Property CMEEndYearRange() As Int64?
        Public Property PresentationTotalHours() As Decimal?
        Public Property EducationCourseTotalHours() As Decimal?
        Public Property PublicationTotalHours() As Decimal?
        Public Property CommunityServiceTotalHours() As Decimal?
        Public Property ARNMaxSummaryPointOfContinuingEducation() As Decimal?
        Public Property ReCertificationCycle() As Integer?
        Public Property RecertificationPaymentMonths() As Integer?
        Public Property ARNMaxSummaryPoint() As Integer?
    End Class
End Namespace