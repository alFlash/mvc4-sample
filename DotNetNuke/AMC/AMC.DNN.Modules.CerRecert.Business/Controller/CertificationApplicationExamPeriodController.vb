Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Business.Entities
Imports TIMSS.API.OrderInfo
Imports TIMSS.Enumerations
Imports TIMSS
Imports TIMSS.API.User.OrderInfo
Imports TIMSS.API.CustomerInfo
Imports TIMSS.API.CertificationInfo
Imports TIMSS.API.User.CertificationInfo
Imports TIMSS.API.Core.Validation

Namespace Controller
    Partial Class AmcCertRecertController
        Public Function SeclectApplicationRandomlyForAuditing(ByVal startDate As DateTime, ByVal endDate As DateTime,
                                                               ByVal selectionRatio As String,
                                                               ByVal notifyApplicant As Boolean,
                                                               ByVal typeCode As String,
                                                               ByVal certificationCode As String) _
            As List(Of CertificationCustomerCertification)
            Dim certificationCustomerResult As CertificationCustomerCertifications =
                    _personifyDataProvider.GetCertificationApplication(startDate, endDate, selectionRatio,
                                                                        notifyApplicant, typeCode, certificationCode)
            Dim selectedApplicationList As List(Of CertificationCustomerCertification) =
                    New List(Of CertificationCustomerCertification)()
            If certificationCustomerResult IsNot Nothing Then
                Dim totalApplication = certificationCustomerResult.Count
                Dim sRatio As Integer = CType(selectionRatio, Int32)
                Dim randoms As New List(Of Integer)()
                Dim numberOfSelectedRecord As Int32
                If (totalApplication > 0 And sRatio > 0) Then
                    numberOfSelectedRecord = CType(Math.Ceiling((totalApplication * sRatio) / 100), Int32)
                    While randoms.Count <> numberOfSelectedRecord
                        'generate new random between one and total list count
                        Dim randomInt As Integer = New Random().[Next](totalApplication)
                        ' store this in dictionary to ensure uniqueness
                        If Not randoms.Contains(randomInt) Then
                            randoms.Add(randomInt)
                            Dim customerCertification As CertificationCustomerCertification =
                                    CType(certificationCustomerResult(randomInt), 
                                            CertificationCustomerCertification)
                            customerCertification.UserDefinedAudit = False
                            selectedApplicationList.Add(customerCertification)
                        End If
                    End While
                    'Commit the cert/recert records which are selected for auditting
                    Dim issuesCollection As IIssuesCollection =
                            CommitCertificationCustomerCertifications(certificationCustomerResult)
                    'Update audit notification event
                    If (notifyApplicant) Then
                        'Call method to update notification event.
                    End If
                End If
            End If
            Return selectedApplicationList
        End Function

#Region "Certification Customer Certification"

        Public Function GetCertificationCustomerCertifications(ByVal certificationCode As String,
                                                                ByVal certificationTypeCode As String,
                                                                ByVal masterCustomerId As String,
                                                                ByVal subCustomerId As Integer) _
            As CertificationCustomerCertifications
            Dim certificationCustomerCertifications As CertificationCustomerCertifications = Nothing
            certificationCustomerCertifications =
                _personifyDataProvider.GetCertificationCustomerCertifications(certificationCode,
                                                                                certificationTypeCode,
                                                                                masterCustomerId,
                                                                                subCustomerId)
            Return certificationCustomerCertifications
        End Function


        Public Function GetApplicantStatus(ByVal certificationCode As CertificationCode, ByVal masterCustomerId As String,
                                            ByVal subCustomerId As Integer,
                                            ByRef certificationCustomerCertificationOut As  _
                                               ICertificationCustomerCertification) As ApplicantStatusEnum
            Dim applicantStatusReturn As ApplicantStatusEnum = ApplicantStatusEnum.CreateNewCertfication
            Dim certificationCustomerCertifications As CertificationCustomerCertifications = Nothing
            Dim certificationCustomerCertification As CertificationCustomerCertification = Nothing
            certificationCustomerCertifications =
                _personifyDataProvider.GetCertificationCustomerCertifications(certificationCode.ReCertificationCode,
                                                CertificationTypeEnum.RECERTIFICATION.ToString(),
                                                                                masterCustomerId,
                                                                                subCustomerId)

            If certificationCustomerCertifications.Count > 0 Then

                certificationCustomerCertification =
                    CType(certificationCustomerCertifications(0), CertificationCustomerCertification)
                If certificationCustomerCertification.CertificationStatusCodeString =
                    CertificationStatusCodeEnum.PENDING.ToString() Then
                    applicantStatusReturn = ApplicantStatusEnum.AllowApplyForRecertification
                    certificationCustomerCertificationOut = certificationCustomerCertifications(0)
                Else
                    applicantStatusReturn = ApplicantStatusEnum.RecertificationIsBeingProcess
                End If
                ''Store current recertifcation
            Else

                certificationCustomerCertifications =
                    _personifyDataProvider.GetCertificationCustomerCertifications(certificationCode.CertificationCode,
                                                                                    CertificationTypeEnum.
                                                                                        CERTIFICATION.ToString(),
                                                                                    masterCustomerId,
                                                                                    subCustomerId)
                If certificationCustomerCertifications.Count < 1 Then
                    applicantStatusReturn = ApplicantStatusEnum.CreateNewCertfication
                Else
                    Dim iCertificationCustomerCertifications = CType(certificationCustomerCertifications, 
                                                                        ICertificationCustomerCertifications)
                    certificationCustomerCertification =
                        CType(iCertificationCustomerCertifications.FindObject("CertificationStatusCodeString",
                                                                                CertificationStatusCodeEnum.
                                                                                    COMPLETED.ToString()), 
                                CertificationCustomerCertification)
                    If certificationCustomerCertification IsNot Nothing Then
                        applicantStatusReturn = ApplicantStatusEnum.AllowApplyForRecertification
                    Else
                        ''find certification for editing
                        For Each customerCertification As CertificationCustomerCertification In _
                            certificationCustomerCertifications
                            If _
                                (customerCertification.CertificationStatusCodeString.Equals(
                                    CertificationStatusCodeEnum.PENDING.ToString()) OrElse
                                    (customerCertification.CertificationStatusCodeString.Equals(
                                        CertificationStatusCodeEnum.UNDER_REVIEW.ToString()) AndAlso
                                    customerCertification.UserDefinedAudit = True)) Then
                                applicantStatusReturn = ApplicantStatusEnum.EditCertification
                                certificationCustomerCertificationOut = customerCertification
                                Exit For
                            Else
                                If _
                                    customerCertification.CertificationStatusCodeString.Equals(
                                        CertificationStatusCodeEnum.UNDER_REVIEW.ToString()) AndAlso
                                    customerCertification.UserDefinedAudit = True Then
                                    applicantStatusReturn = ApplicantStatusEnum.CertificationIsBeingProcess
                                    Exit For
                                End If
                            End If
                        Next
                    End If
                End If
            End If
            Return applicantStatusReturn
        End Function

        Public Function InsertCertificationCustomerCertification(
                                                                  ByRef certificationCustomerCertification As  _
                                                                     ICertificationCustomerCertification) _
            As IIssuesCollection
            Dim iissuesCollection As IIssuesCollection = Nothing

            iissuesCollection =
                _personifyDataProvider.InsertCertificationCustomerCertification(
                    certificationCustomerCertification)

            Return iissuesCollection
        End Function

#End Region

#Region "Payment"
        Public Function WasPaymentProcessed(ByVal orgId As String, ByVal orgUnitId As String, _
                                            ByVal masterCustomerId As String, ByVal subCustomerId As Integer,
                                            ByVal productId As Integer) As Boolean
            Dim customer As ICustomer = GetCustomerInfoItem(orgId, orgUnitId, masterCustomerId, subCustomerId)
            For Each orderHistory As IOrderHistoryView In customer.RecentOrders
                If orderHistory.ProductId = productId Then

                    'Get OrderDetail
                    Dim orderDetails = _personifyDataProvider.GetOrderDetail(orgId, orgUnitId, orderHistory.OrderNumber, productId)

                    If orderDetails IsNot Nothing Then
                        For Each orderDetail As IOrderDetail In orderDetails
                            'Get OrderMaster for this order
                            Dim orders As IOrderMasters = _personifyDataProvider.GetOrderMaster(orgId, orgUnitId, orderDetail.OrderNumber)
                            If orders IsNot Nothing Then
                                For Each orderMaster As IOrderMaster In orders
                                    If orderMaster.OrderBalance = 0 Then
                                        Return True
                                    End If
                                Next
                            End If
                        Next
                    End If
                End If
            Next

            Return False
        End Function


#End Region

    End Class
End Namespace
