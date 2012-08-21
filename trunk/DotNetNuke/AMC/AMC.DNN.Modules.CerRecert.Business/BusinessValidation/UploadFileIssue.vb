Imports System.IO
Imports TIMSS.API.Core
Imports TIMSS.API.Core.Validation

Namespace BusinessValidation
    Public Class UploadFileIssue
        Inherits IssueBase
        Private Const ERROR_MESSAGE = "Please attach only PDF file"

        Public Sub New(ByVal BusinessObject As IBusinessObject)
            MyBase.New(BusinessObject, "", IssueSeverityEnum.Error, Nothing, False, ERROR_MESSAGE)

        End Sub

        Public Sub New(ByVal businessObject As IBusinessObject, ByVal errorMessage As String)
            MyBase.New(businessObject, "", IssueSeverityEnum.Error, Nothing, False, errorMessage)
        End Sub

        Public Shared Sub Assert(ByVal Expression As Boolean, ByVal BusinessObject As IBusinessObject)
            If Expression Then
                BusinessObject.ValidationIssues.Add(New UploadFileIssue(BusinessObject))
            Else
                BusinessObject.ValidationIssues.Remove(UploadFileIssue.GetKey(BusinessObject))
            End If
        End Sub

        Public Shared Function GetKey(ByVal BusinessObject As IBusinessObject) As String
            Return String.Format("{0}:{1}:", BusinessObject.Guid, GetType(UploadFileIssue).FullName)
        End Function

        Public Shared Function IsNotPdfFile(ByVal stream As Stream) As Boolean
            Dim result = True
            Try
                If stream Is Nothing OrElse stream.Length < 5 Then
                    Return False
                End If
                Dim buf As Byte() = New Byte(4) {}
                stream.Read(buf, 0, 4)
                Dim header As String = String.Empty
                For i As Integer = 0 To 3
                    header = String.Format("{0}{1}", header, ChrW(buf(i)).ToString())
                Next
                If (header.Equals("%PDF")) Then
                    result = False
                End If
            Catch ex As Exception
                Throw
            End Try
            Return result
        End Function

    End Class
End Namespace