Imports System

Namespace Personify.ApplicationManager
    Public Class CustomerHelper
        Inherits BaseHelperClass
        ' Methods
        Public Sub New(ByVal OrgId As String, ByVal OrgUnitId As String)
            MyBase.New(OrgId, OrgUnitId)
        End Sub

        Public Sub New(ByVal OrgId As String, ByVal OrgUnitId As String, ByVal EnableOnDemandDataLoad As Boolean)
            MyBase.New(OrgId, OrgUnitId, EnableOnDemandDataLoad)
        End Sub

    End Class
End Namespace

