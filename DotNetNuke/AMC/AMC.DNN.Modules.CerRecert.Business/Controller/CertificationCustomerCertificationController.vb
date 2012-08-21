Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Data
Imports AMC.DNN.Modules.CertRecert.Business.Entities
Imports System.ComponentModel
Imports TIMSS.API.CertificationInfo
Imports TIMSS.API.User.CertificationInfo
Imports TIMSS.API.Core.Validation
Imports TIMSS.API.User.UserDefinedInfo

Namespace Controller
    Partial Class AmcCertRecertController
        Public Function GetCertificationApplicationsForReview(ByVal startDate As DateTime, ByVal endDate As DateTime,
                                               ByVal typeCode As String, ByVal certificationCode As String) _
        As CertificationCustomerCertifications
            Return _personifyDataProvider.GetCertificationApplicationsForReview(startDate, endDate, typeCode, certificationCode)
        End Function
        Public Function GetCertAppExamPeriodByQuestionId(ByVal questionId As Long) As IUserDefinedCertificationApplicationExamPeriod
            Return _personifyDataProvider.GetCertAppExamPeriodByQuestionId(questionId)
        End Function
        Public Function SeclectApplicationRandomlyForAuditing(ByVal startDate As DateTime, ByVal endDate As DateTime,
                                                               ByVal selectionRatio As String,
                                                               ByVal notifyApplicant As Boolean,
                                                               ByVal typeCode As String,
                                                               ByVal certificationCode As String, ByRef totalApplication As Int32) _
            As List(Of CertificationCustomerCertification)
            Dim certificationCustomerResult As CertificationCustomerCertifications =
                    _personifyDataProvider.GetCertificationApplication(startDate, endDate, selectionRatio,
                                                                        notifyApplicant, typeCode, certificationCode)
            Dim selectedApplicationList As List(Of CertificationCustomerCertification) =
                    New List(Of CertificationCustomerCertification)()
            If certificationCustomerResult IsNot Nothing Then
                totalApplication = certificationCustomerResult.Count
                Dim sRatio As Integer = CType(selectionRatio, Int32)
                Dim randoms As New List(Of Integer)()
                Dim numberOfSelectedRecord As Int32
                If (totalApplication > 0 And sRatio > 0) Then
                    numberOfSelectedRecord = CType(Math.Ceiling((totalApplication * sRatio) / 100), Int32)
                    While randoms.Count <> numberOfSelectedRecord
                        'generate new random between one and total list count
                        Dim randomInt As Integer = New Random().[Next](totalApplication)
                        ' store this in a generic list to ensure uniqueness
                        If Not randoms.Contains(randomInt) Then
                            randoms.Add(randomInt)
                            Dim customerCertification As CertificationCustomerCertification =
                                    CType(certificationCustomerResult(randomInt), 
                                            CertificationCustomerCertification)
                            customerCertification.UserDefinedAudit = True
                            selectedApplicationList.Add(customerCertification)
                        End If
                    End While
                    'Commit the cert/recert records which are selected for auditting
                    Dim issuesCollection As IIssuesCollection = CommitCertificationCustomerCertifications(certificationCustomerResult)
                End If
            End If
            Return selectedApplicationList
        End Function

        Public Function GetAllCertifications(ByVal certificationCode As CertificationCode, ByVal masterCustomerId As String, ByVal subCustomerId As Integer) As CertificationCustomerCertifications
            Dim results = _personifyDataProvider.GetCertificationCustomerCertifications(
                                                    certificationCode.ReCertificationCode,
                                                    CertificationTypeEnum.RECERTIFICATION.ToString(),
                                                    masterCustomerId,
                                                    subCustomerId)
            If results Is Nothing OrElse results.Count <= 0 Then
                results = _personifyDataProvider.GetCertificationCustomerCertifications(
                                                    certificationCode.CertificationCode,
                                                    CertificationTypeEnum.CERTIFICATION.ToString(),
                                                    masterCustomerId,
                                                    subCustomerId)
            End If
            results.Sort("RegistrationDate", ListSortDirection.Descending)
            Return results
        End Function

        Public Function GetPendingCertification(ByVal orgCertId As Integer, ByVal masterId As String, ByVal subId As Integer) As ICertificationCustomerCertification
            Dim orgCert = _personifyDataProvider.GetOrigCertification(orgCertId, masterId, subId)
            While orgCert.CertificationExpirationDate.Year > 1 AndAlso orgCert.CertificationStatusCodeString <> "PENDING" AndAlso orgCertId <> orgCert.OrigCertificationId
                orgCert = _personifyDataProvider.GetOrigCertification(orgCert.OrigCertificationId, masterId, subId)
            End While
            Return If(orgCert.CertificationExpirationDate.Year > 1, orgCert, Nothing)
        End Function

        Public Function GetOrigCertification(ByVal origCertificationId As Integer, ByVal masterId As String, ByVal subId As Integer) As ICertificationCustomerCertification
            Return _personifyDataProvider.GetOrigCertification(origCertificationId, masterId, subId)
        End Function
        Public Function GetCertificationCustomerCertificationByCertId(ByVal certificationId As Integer) As ICertificationCustomerCertification
            Return _personifyDataProvider.GetCertificationCustomerCertificationByCertId(certificationId)
        End Function
        Public Function UpdateCertification(ByVal practiceScopeItem As ICertificationCustomerCertification) As IIssuesCollection
            ''To Do:
            Return _personifyDataProvider.UpdateCertification(practiceScopeItem)
        End Function

    End Class
End Namespace
