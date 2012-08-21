Imports AMC.DNN.Modules.CertRecert.Data.Exception
Imports TIMSS
Imports TIMSS.Enumerations
Imports TIMSS.API.CertificationInfo
Imports TIMSS.API.User.CertificationInfo
Imports TIMSS.API.Core.Validation
Imports TIMSS.API.User.UserDefinedInfo

Partial Class PersonifyDataProvider
    Inherits DataProvider

#Region "Certification Application Exam Period"

    Public Function GetCertAppExamPeriodByQuestionId(ByVal questionId As Long) As IUserDefinedCertificationApplicationExamPeriod
        Dim certAppExamPeriodReturn As IUserDefinedCertificationApplicationExamPeriod = Nothing
        Try
            Dim certAppExamPeriods = CType(TIMSS.Global.GetCollection(Me._organizationId,
                                                                Me._organizationUnitId,
                                                                NamespaceEnum.UserDefinedInfo,
                                                        "UserDefinedCertificationApplicationExamPeriods"), 
                                                    UserDefinedCertificationApplicationExamPeriods)
            With certAppExamPeriods.Filter
                .Add("QuestionId", questionId.ToString())
            End With
            certAppExamPeriods.Fill()
            If certAppExamPeriods IsNot Nothing AndAlso certAppExamPeriods.Count > 0 Then
                certAppExamPeriodReturn = certAppExamPeriods(0)
            End If
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return certAppExamPeriodReturn
    End Function

#End Region



End Class
