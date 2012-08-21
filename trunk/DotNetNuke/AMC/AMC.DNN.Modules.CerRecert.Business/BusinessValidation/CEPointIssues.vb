Imports TIMSS.API.Core
Imports TIMSS.API.Core.Validation

Namespace BusinessValidation
    Public Class CEPointIssues
        Inherits IssueBase
        Public Sub New(ByVal businessObject As IBusinessObject, ByVal errorMessage As String)
            MyBase.New(businessObject, "", IssueSeverityEnum.Error, Nothing, False, "")
            Message = errorMessage
        End Sub

        Public Shared Sub Assert(ByVal expression As Boolean, ByVal businessObject As IBusinessObject, ByVal errorMessage As String)
            If expression Then
                businessObject.ValidationIssues.Add(New CEPointIssues(businessObject,
                                                                            errorMessage))
            Else
                businessObject.ValidationIssues.Remove(GetKey(businessObject))
            End If
        End Sub

        Public Shared Function GetKey(ByVal businessObject As IBusinessObject) As String
            Return String.Format("{0}:{1}:", businessObject.Guid, GetType(CEPointIssues).FullName)
        End Function
    End Class
End Namespace