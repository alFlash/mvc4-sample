Imports System
Imports System.Data
Imports TIMSS
Imports TIMSS.DataAccess

Namespace Personify.ApplicationManager
    Public Class IdentityManagement
        ' Methods
        Public Shared Function ExecuteQuery(ByVal strSQL As String) As DataSet
            Dim request As New SimpleRequest("IMQuery", strSQL.ToString, Nothing)
            Return TIMSS.Global.App.GetData(request).Unpack
        End Function

    End Class
End Namespace

