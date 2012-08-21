Imports DotNetNuke.Services.Log.EventLog

Namespace Exception
    Public Class DataAccessExceptionHandler

        Public Shared Sub HandleException(ex As System.Exception)
            'log exception here
            Dim exceptionLogController As ExceptionLogController = New ExceptionLogController()
            exceptionLogController.AddLog(ex)

            'and throw new data access exception
            'wrap exception to new data access exception
            Dim dataException = New DataAccessException("An Error occurs when access to Persionify", ex)
            Throw dataException

        End Sub
    End Class
End Namespace