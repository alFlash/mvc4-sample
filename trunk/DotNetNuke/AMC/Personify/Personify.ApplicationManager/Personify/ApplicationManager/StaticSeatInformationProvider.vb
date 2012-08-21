Imports System
Imports TIMSS.Client.Implementation.Security.Authentication
Imports TIMSS.Security.Network

Namespace Personify.ApplicationManager
    Public Class StaticSeatInformationProvider
        Implements ISeatInformationProvider
        ' Methods
        Public Sub New(ByVal seatInformation As SeatInformation, ByVal password As String)
            Me._seatInformation = seatInformation
            Me._password = password
        End Sub

        Public Function GetPassword() As String Implements ISeatInformationProvider.GetPassword
            Return Me._password
        End Function

        Public Function GetSeatInformation() As SeatInformation Implements ISeatInformationProvider.GetSeatInformation
            Return Me._seatInformation
        End Function


        ' Fields
        Private _password As String
        Private _seatInformation As SeatInformation
    End Class
End Namespace

