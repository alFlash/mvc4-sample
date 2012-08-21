Imports TIMSS.API.User.UserDefinedInfo
Imports TIMSS.API.Core.Validation

Namespace Controller
    Partial Class AmcCertRecertController
        Public Function GetCertificationCEWeights(ByVal organizationId As String, ByVal organizationUnitId As String) As UserDefinedCertificationCEWeights
            Return _personifyDataProvider.GetCertificationCEWeights(organizationId, organizationUnitId)
        End Function

        Public Function GetCertificationCEWeights(ByVal organizationId As String, ByVal organizationUnitId As String, ByVal programType As String) As UserDefinedCertificationCEWeights
            Return _personifyDataProvider.GetCertificationCEWeights(organizationId, organizationUnitId, programType)
        End Function

        Public Function AddCertificationCEWeightSetting(ByVal organizationId As String, ByVal organizationUnitId As String,
                                                        ByVal ceType As String, ByVal programType As String,
                                                        ByVal ceWeight As String, ByVal minCE As String, ByVal maxCE As String) As IIssuesCollection
            Return _personifyDataProvider.AddCertificationCEWeightSetting(organizationId, organizationUnitId, ceType, programType, ceWeight, minCE, maxCE)
        End Function

        Public Function UpdateCertificationCEWeightSetting(ByVal organizationId As String, ByVal organizationUnitId As String,
                                                           ByVal ceType As String, ByVal programType As String,
                                                           ByVal ceWeight As String, ByVal minCE As String, ByVal maxCE As String, ByVal uniqueId As String) As IIssuesCollection
            Return _personifyDataProvider.UpdateCertificationCEWeightSetting(organizationId, organizationUnitId, ceType, programType, ceWeight, minCE, maxCE, uniqueId)
        End Function

        Public Sub DeleteCEWeightSetting(ByVal organizationId As String, ByVal organizationUnitId As String, ByVal uniqueId As String)
            _personifyDataProvider.DeleteCEWeightSetting(organizationId, organizationUnitId, uniqueId)
        End Sub

        Public Function GetUserDefinedCertificationCEWeights(ByVal organizationId As String, ByVal organizationUnitId As String, ByVal ceType As String, ByVal programType As String) As IUserDefinedCertificationCEWeights
            Return _personifyDataProvider.GetUserDefinedCertificationCEWeights(organizationId, organizationUnitId, ceType, programType)
        End Function
    End Class
End Namespace