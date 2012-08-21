Imports AMC.DNN.Modules.CertRecert.Data
Imports TIMSS.API.User.UserDefinedInfo

Namespace Controller
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The Controller class for AmcCertRecertGui
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class AmcCertRecertController

#Region "Properties"
        Public ReadOnly Property OrganizationId As String
            Get
                Return _organizationId
            End Get
        End Property
        Public ReadOnly Property OrganizationUnitId As String
            Get
                Return _organizationUnitId
            End Get
        End Property
        Public ReadOnly Property MasterCustomerId As String
            Get
                Return _masterCustomerId
            End Get
        End Property
        Public ReadOnly Property SubCustomerId As Integer
            Get
                Return _subCustomerId
            End Get
        End Property
#End Region

#Region "Private Member"

        Private ReadOnly _personifyDataProvider As PersonifyDataProvider
        Private _organizationId As String
        Private _organizationUnitId As String
        Private _certificationId As Integer
        Private _modulePath As String
        Private _masterCustomerId As String
        Private _subCustomerId As Integer

#End Region

        Public Sub New(ByVal organizationId As String,
                       ByVal organizationUnitId As String,
                       ByVal certificationId As Integer,
                       ByVal modulePath As String,
                       ByVal masterCustomerId As String,
                       ByVal subCustomerId As Integer)
            _organizationId = organizationId
            _organizationUnitId = organizationUnitId
            _certificationId = certificationId
            _modulePath = modulePath
            _masterCustomerId = masterCustomerId
            _subCustomerId = subCustomerId
            _personifyDataProvider = New PersonifyDataProvider(_organizationId, _organizationUnitId, _certificationId, _modulePath, _masterCustomerId, _subCustomerId)
        End Sub

#Region "Check amc data available"
        Public Function CheckDataIsAvailable(ByVal masterCustomer As String) As Boolean
            Return _personifyDataProvider.CheckDataIsAvailable(masterCustomer)
        End Function
#End Region

    End Class
End Namespace
