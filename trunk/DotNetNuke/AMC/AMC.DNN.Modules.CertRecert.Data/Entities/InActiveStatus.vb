Namespace Entities
    <Serializable()>
    Public Class InActiveStatus
        ' Inherits Request

#Region "ApplyInactiveStatus"

        Private _applicantFirstName As String
        Private _applicantMiddleName As String
        Private _applicantLastName As String
        Private _applicantHomeAddress As String
        Private _applicantCity As String
        Private _applicantState As String
        Private _applicantZip As String
        Private _applicantHomePhone As String
        Private _applicantWorkPhone As String
        Private _applicantEmail As String
        Private _applicantLicense As String
        Private _applicantCertification As String

        Private _applicantBirthOfChid As Boolean
        Private _applicantDependent As Boolean
        Private _applicantMilitaryDuty As Boolean
        Private _applicantDivorce As Boolean
        Private _applicantIllness As Boolean
        Private _applicantDeathFamilyMember As Boolean
        Private _applicantLossHousing As Boolean

        Private _applicantCard As Enums.Enums.CardType
        Private _applicantAccountNumber As String
        Private _applicantExpirationDate As DateTime
        Private _applicantCSV As String
        Private _applicantCardHolder As String

#End Region

#Region "ApplyInactiveStatus"

        Public Property ApplicantFirstName() As String
            Get
                Return _applicantFirstName
            End Get
            Set (ByVal value As String)
                _applicantFirstName = value
            End Set
        End Property

        Public Property ApplicantMiddleName() As String
            Get
                Return _applicantMiddleName
            End Get
            Set (ByVal value As String)
                _applicantMiddleName = value
            End Set
        End Property

        Public Property ApplicantLastName() As String
            Get
                Return _applicantLastName
            End Get
            Set (ByVal value As String)
                _applicantLastName = value
            End Set
        End Property

        Public Property ApplicantHomeAddress() As String
            Get
                Return _applicantHomeAddress
            End Get
            Set (ByVal value As String)
                _applicantHomeAddress = value
            End Set
        End Property

        Public Property ApplicantCity() As String
            Get
                Return _applicantCity
            End Get
            Set (ByVal value As String)
                _applicantCity = value
            End Set
        End Property

        Public Property ApplicantState() As String
            Get
                Return _applicantState
            End Get
            Set (ByVal value As String)
                _applicantState = value
            End Set
        End Property

        Public Property ApplicantZip() As String
            Get
                Return _applicantZip
            End Get
            Set (ByVal value As String)
                _applicantZip = value
            End Set
        End Property

        Public Property ApplicantHomePhone() As String
            Get
                Return _applicantHomePhone
            End Get
            Set (ByVal value As String)
                _applicantHomePhone = value
            End Set
        End Property

        Public Property ApplicantWorkPhone() As String
            Get
                Return _applicantWorkPhone
            End Get
            Set (ByVal value As String)
                _applicantWorkPhone = value
            End Set
        End Property

        Public Property ApplicantEmail() As String
            Get
                Return _applicantEmail
            End Get
            Set (ByVal value As String)
                _applicantEmail = value
            End Set
        End Property

        Public Property ApplicantLicense() As String
            Get
                Return _applicantLicense
            End Get
            Set (ByVal value As String)
                _applicantLicense = value
            End Set
        End Property

        Public Property ApplicantCertification() As String
            Get
                Return _applicantCertification
            End Get
            Set (ByVal value As String)
                _applicantCertification = value
            End Set
        End Property

        Public Property ApplicantBirthOfChid() As Boolean
            Get
                Return _applicantBirthOfChid
            End Get
            Set (ByVal value As Boolean)
                _applicantBirthOfChid = value
            End Set
        End Property

        Public Property ApplicantDependent() As Boolean
            Get
                Return _applicantDependent
            End Get
            Set (ByVal value As Boolean)
                _applicantDependent = value
            End Set
        End Property

        Public Property ApplicantMilitaryDuty() As Boolean
            Get
                Return _applicantMilitaryDuty
            End Get
            Set (ByVal value As Boolean)
                _applicantMilitaryDuty = value
            End Set
        End Property

        Public Property ApplicantDivorce() As Boolean
            Get
                Return _applicantDivorce
            End Get
            Set (ByVal value As Boolean)
                _applicantDivorce = value
            End Set
        End Property

        Public Property ApplicantIllness() As Boolean
            Get
                Return _applicantIllness
            End Get
            Set (ByVal value As Boolean)
                _applicantIllness = value
            End Set
        End Property

        Public Property ApplicantDeathFamilyMember() As Boolean
            Get
                Return _applicantDeathFamilyMember
            End Get
            Set (ByVal value As Boolean)
                _applicantDeathFamilyMember = value
            End Set
        End Property

        Public Property ApplicantLossHousing() As Boolean
            Get
                Return _applicantLossHousing
            End Get
            Set (ByVal value As Boolean)
                _applicantLossHousing = value
            End Set
        End Property

        Public Property ApplicantCard() As Enums.Enums.CardType
            Get
                Return _applicantCard
            End Get
            Set (ByVal value As Enums.Enums.CardType)
                _applicantCard = value
            End Set
        End Property

        Public Property ApplicantAccountNumber() As String
            Get
                Return _applicantAccountNumber
            End Get
            Set (ByVal value As String)
                _applicantAccountNumber = value
            End Set
        End Property

        Public Property ApplicantExpirationDate() As DateTime
            Get
                Return _applicantExpirationDate
            End Get
            Set (ByVal value As DateTime)
                _applicantExpirationDate = value
            End Set
        End Property

        Public Property ApplicantCSV() As String
            Get
                Return _applicantCSV
            End Get
            Set (ByVal value As String)
                _applicantCSV = value
            End Set
        End Property

        Public Property ApplicantCardHolder() As String
            Get
                Return _applicantCardHolder
            End Get
            Set (ByVal value As String)
                _applicantCardHolder = value
            End Set
        End Property


#End Region
    End Class
End Namespace