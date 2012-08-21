Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports TIMSS.API.Core
Imports TIMSS.API.Core.Validation

Namespace BusinessValidation
    Public Class ExpirationDateIssue
        Inherits IssueBase
        Private Const ERROR_MESSAGE = "Expiration date must be greater than or equal to {0}"

        Public Sub New(ByVal BusinessObject As IBusinessObject, ByVal expirationDate As Date)
            MyBase.New(BusinessObject, "", IssueSeverityEnum.Error, Nothing, False, "")
            Me.Message = String.Format(ERROR_MESSAGE, expirationDate.ToString(CommonConstants.DATE_FORMAT))
        End Sub

        Public Shared Sub Assert(ByVal Expression As Boolean, ByVal BusinessObject As IBusinessObject, ByVal expirationDate As Date)
            If Expression Then
                BusinessObject.ValidationIssues.Add(New ExpirationDateIssue(BusinessObject,
                                                                            expirationDate))
            Else
                BusinessObject.ValidationIssues.Remove(ExpirationDateIssue.GetKey(BusinessObject))
            End If
        End Sub

        Public Shared Function GetKey(ByVal BusinessObject As IBusinessObject) As String
            Return String.Format("{0}:{1}:", BusinessObject.Guid, GetType(ExpirationDateIssue).FullName)
        End Function
    End Class
End Namespace