Imports System

Namespace Exception
    Public Class DataAccessException
        Inherits System.Exception

        Public Sub New()

        End Sub

        Public Sub New(ByVal err As String, ByVal exception As System.Exception)
            MyBase.New(err, exception)
        End Sub
    End Class
End Namespace