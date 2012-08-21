Namespace Helpers
    Public Class Utils

        Public Shared Function ParseInteger(s As String, defaultValue As Integer) As Integer
            Dim result As Integer = defaultValue
            Integer.TryParse(s, result)
            Return result
        End Function
    End Class
End Namespace