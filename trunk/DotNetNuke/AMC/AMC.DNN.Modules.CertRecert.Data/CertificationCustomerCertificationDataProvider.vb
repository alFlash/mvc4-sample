Imports AMC.DNN.Modules.CertRecert.Data.Exception
Imports System.ComponentModel
Imports TIMSS
Imports TIMSS.Enumerations
Imports TIMSS.API.CertificationInfo
Imports TIMSS.API.User.CertificationInfo
Imports TIMSS.API.Core.Validation

Partial Class PersonifyDataProvider
    Inherits DataProvider

#Region "Certification Customer Certification"

    Public Function GetCertificationCustomerCertifications (ByVal certificationCode As String,
                                                            ByVal certificationTypeCode As String,
                                                            ByVal masterCustomerId As String,
                                                            ByVal subCustomerId As Integer) _
        As CertificationCustomerCertifications
        Dim certificationCustomerCertifications As CertificationCustomerCertifications = Nothing
        Try
            certificationCustomerCertifications =
                CType([Global].GetCollection(Me._organizationId,
                                               Me._organizationUnitId,
                                               NamespaceEnum.CertificationInfo,
                                               "CertificationCustomerCertifications"), 
                       CertificationCustomerCertifications)
            With certificationCustomerCertifications.Filter
                If Not certificationCode.Equals(String.Empty) Then
                    .Add("CertificationCode", certificationCode)
                End If
                If Not certificationTypeCode.Equals(String.Empty) Then
                    .Add("CertificationTypeCode", certificationTypeCode)
                End If
                .Add("MasterCustomerId", masterCustomerId)
                .Add("SubCustomerId", subCustomerId.ToString())
            End With
            certificationCustomerCertifications.Fill()


        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        certificationCustomerCertifications.Sort("RegistrationDate", ListSortDirection.Descending)
        Return certificationCustomerCertifications

    End Function

    Public Function InsertCertificationCustomerCertification(
                                                    ByRef certificationCustomerCertificationInput As  _
                                                                 ICertificationCustomerCertification) _
        As IIssuesCollection
        Dim certificationCustomerCertifications As CertificationCustomerCertifications = Nothing
        Try
            certificationCustomerCertifications =
                GetCertificationCustomerCertifications(certificationCustomerCertificationInput.CertificationCodeString,
                                                        certificationCustomerCertificationInput.
                                                        CertificationTypeCodeString,
                                                        certificationCustomerCertificationInput.MasterCustomerId,
                                                        certificationCustomerCertificationInput.SubCustomerId)
            Dim certificationCustomer = certificationCustomerCertifications.CreateNew()
            With certificationCustomer
                .IsNewObjectFlag = True
                .MasterCustomerId = certificationCustomerCertificationInput.MasterCustomerId
                .SubCustomerId = certificationCustomerCertificationInput.SubCustomerId
                .CertificationCodeString = certificationCustomerCertificationInput.CertificationCodeString
            End With
            certificationCustomerCertifications.Add(certificationCustomer)
            certificationCustomerCertifications.Save()
            If certificationCustomerCertifications.ValidationIssues.Count < 1 Then
                certificationCustomerCertificationInput = certificationCustomer
            End If
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return certificationCustomerCertifications.ValidationIssues
    End Function

#End Region


#Region "Audit Process"

    Public Function GetCertificationApplication(ByVal startDate As DateTime, ByVal endDate As DateTime,
                                                 ByVal selectionRatio As String, ByVal notifyApplicant As Boolean,
                                                 ByVal typeCode As String, ByVal certificationCode As String) _
        As CertificationCustomerCertifications
        Dim certificationCustomerResult As CertificationCustomerCertifications = Nothing
        Try
            certificationCustomerResult = CType([Global].GetCollection(Me._organizationId, Me._organizationUnitId,
                                                                         NamespaceEnum.CertificationInfo,
                                                                         "CertificationCustomerCertifications"), 
                                                 CertificationCustomerCertifications)
            With certificationCustomerResult.Filter
                .Add("ApplicationDate", QueryOperatorEnum.GreaterThanOrEqual, startDate.ToString()) ' A configurable parameter in Control Panel
                .Add("ApplicationDate", QueryOperatorEnum.LessThanOrEqual, endDate.ToString()) ' A configurable parameter in Control Panel

                .Add("UserDefinedAudit", QueryOperatorEnum.NotEqualTo, "Y") ' Only cert/recert records which are not being audited?
                .Add("CertificationStatusCode", QueryOperatorEnum.Equals, "PENDING") ' Only cert/recert records in PENDING status can be applicable for Audit?

                .Add("CertificationCode", QueryOperatorEnum.Equals, certificationCode) ' A configurable parameter in Control Panel (ex: CRRN)
                .Add("CertificationTypeCode", QueryOperatorEnum.Equals, typeCode) ' Values can be either CERTIFICATION or RECERTIFICATION
            End With

            certificationCustomerResult.Fill()
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return certificationCustomerResult
    End Function

    Public Function GetCertificationApplicationsForReview(ByVal startDate As DateTime, ByVal endDate As DateTime,
                                                ByVal typeCode As String, ByVal certificationCode As String) _
       As CertificationCustomerCertifications
        Dim certificationCustomerResult As CertificationCustomerCertifications = Nothing
        Try
            certificationCustomerResult = CType([Global].GetCollection(Me._organizationId, Me._organizationUnitId,
                                                                         NamespaceEnum.CertificationInfo,
                                                                         "CertificationCustomerCertifications"), 
                                                 CertificationCustomerCertifications)
            With certificationCustomerResult.Filter
                .Add("ApplicationDate", QueryOperatorEnum.GreaterThanOrEqual, startDate.ToString()) ' A configurable parameter in Control Panel
                .Add("ApplicationDate", QueryOperatorEnum.LessThanOrEqual, endDate.ToString()) ' A configurable parameter in Control Panel
                .Add("CertificationCode", QueryOperatorEnum.Equals, certificationCode) ' A configurable parameter in Control Panel (ex: CRRN)
                .Add("CertificationTypeCode", QueryOperatorEnum.Equals, TypeCode) ' Values can be either CERTIFICATION or RECERTIFICATION
            End With
            certificationCustomerResult.Fill()
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return certificationCustomerResult
    End Function

    Public Function CommitCertificationCustomerCertifications(
                                                               ByVal iCertificationCustomerCertifications As  _
                                                                  ICertificationCustomerCertifications) _
        As IIssuesCollection
        Try
            iCertificationCustomerCertifications.Save()
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return iCertificationCustomerCertifications.ValidationIssues
    End Function


#End Region

    Public Function GetOrigCertification(ByVal origCertificationId As Integer, ByVal masterCustomerId As String, ByVal subCustomerId As Integer) As ICertificationCustomerCertification
        Dim results = TIMSS.Global.GetCollection(_organizationId, _organizationUnitId, NamespaceEnum.CertificationInfo, "CertificationCustomerCertifications")
        With results.Filter
            .Add("CertificationId", origCertificationId.ToString())
            .Add("MasterCustomerId", masterCustomerId)
            .Add("SubCustomerId", subCustomerId.ToString())
        End With
        results.Fill()
        Return If(results IsNot Nothing AndAlso results.Count > 0, CType(results(0), ICertificationCustomerCertification), Nothing)
    End Function
    Public Function GetCertificationCustomerCertificationByCertId(ByVal certificationId As Integer) As ICertificationCustomerCertification
        Dim results = TIMSS.Global.GetCollection(_organizationId, _organizationUnitId, NamespaceEnum.CertificationInfo, "CertificationCustomerCertifications")
        With results.Filter
            .Add("CertificationId", certificationId.ToString())
        End With
        results.Fill()
        Return If(results IsNot Nothing AndAlso results.Count > 0, CType(results(0), ICertificationCustomerCertification), Nothing)
    End Function

    Public Function UpdateCertification(ByVal practiceScopeItem As ICertificationCustomerCertification) As IIssuesCollection
        ''To Do:
        Dim iissueResult As New IssuesCollection
        Dim results = TIMSS.Global.GetCollection(_organizationId, _organizationUnitId,
                                                 NamespaceEnum.CertificationInfo, "CertificationCustomerCertifications")
        With results.Filter
            .Add("CertificationId", practiceScopeItem.CertificationId.ToString())
        End With
        results.Fill()
        If results IsNot Nothing Then
            If results.Count > 0 Then
                CType(results(0), CertificationCustomerCertification).UserDefinedNameOnCert =
                    CType(practiceScopeItem, CertificationCustomerCertification).UserDefinedNameOnCert
                results.Save()
            End If
        End If
        Return iissueResult
    End Function
End Class
