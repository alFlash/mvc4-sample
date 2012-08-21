Imports TIMSS.API.Core
Imports TIMSS.Enumerations
Imports TIMSS
Imports TIMSS.API.NotificationServiceInfo
Imports TIMSS.API.User.UserDefinedInfo
Imports TIMSS.API.Core.Validation
Imports System.Xml.Linq
Imports System.Linq

Partial Class PersonifyDataProvider
    Inherits DataProvider

    Public Function GetCertificationCEWeights(ByVal organizationId As String, ByVal organizationUnitId As String) As UserDefinedCertificationCEWeights
        Dim results = GetObjectFromCache(String.Format("_CC_CETypeSettings_{0}_{1}", organizationId, organizationUnitId))
        If results Is Nothing Then
            Dim certificationCEWeights = CType([Global].GetCollection(organizationId, organizationUnitId, NamespaceEnum.UserDefinedInfo,
                                                           "UserDefinedCertificationCEWeights"), 
                                   IUserDefinedCertificationCEWeights)
            With certificationCEWeights.Filter
                .Add("OrganizationId", organizationId)
                .Add("OrganizationUnitId", organizationId)
            End With
            certificationCEWeights.Fill()
            Dim ceweights = CType(certificationCEWeights, UserDefinedCertificationCEWeights)
            StoreCacheObject(String.Format("_CC_CETypeSettings_{0}_{1}", organizationId, organizationUnitId), ceweights)
            Return ceweights
        Else
            Return CType(results, UserDefinedCertificationCEWeights)
        End If
    End Function

    Public Function GetCertificationCEWeights(ByVal organizationId As String, ByVal organizationUnitId As String, ByVal programType As String) As UserDefinedCertificationCEWeights
        Dim results = GetCertificationCEWeights(organizationId, organizationUnitId)
        Dim ceweights = New UserDefinedCertificationCEWeights()

        If results IsNot Nothing AndAlso results.Count > 0 Then
            For Each ceweight As UserDefinedCertificationCEWeight In results
                If ceweight.ProgramTypeString = programType Then
                    ceweights.Add(ceweight)
                End If
            Next
            Return ceweights
        End If
        Return Nothing
    End Function

    Public Function AddCertificationCEWeightSetting(ByVal organizationId As String, ByVal organizationUnitId As String,
                                                    ByVal ceType As String, ByVal programType As String,
                                                    ByVal ceWeight As String, ByVal minCE As String, ByVal maxCE As String) As IIssuesCollection
        Dim weight = Math.Round(Convert.ToDecimal(ceWeight), 2, MidpointRounding.AwayFromZero)
        Dim ceWeights = GetCertificationCEWeights(organizationId, organizationUnitId)
        Dim newCeWeight = ceWeights.CreateNew()
        With newCeWeight
            .OrganizationId = organizationId
            .OrganizationUnitId = organizationUnitId
            .CEType = .CEType.List(ceType).ToCodeObject()
            .ProgramType = .ProgramType.List(programType).ToCodeObject()
            .Weight = weight
            .MinCE = If(String.IsNullOrEmpty(minCE), 0, Convert.ToDecimal(minCE))
            .MaxCE = If(String.IsNullOrEmpty(minCE), 0, Convert.ToDecimal(maxCE))
            .IsNewObjectFlag = True
        End With
        ceWeights.Add(newCeWeight)
        ceWeights.Save()
        If ceWeights.ValidationIssues.Count > 0 Then
            ceWeights.Remove(newCeWeight)
        End If
        Return ceWeights.ValidationIssues
    End Function

    Public Function UpdateCertificationCEWeightSetting(ByVal organizationId As String, ByVal organizationUnitId As String,
                                                       ByVal ceType As String, ByVal programType As String,
                                                       ByVal ceWeightValue As String, ByVal minCE As String, ByVal maxCE As String, ByVal uniqueId As String) As IIssuesCollection
        Dim weight = Math.Round(Convert.ToDecimal(ceWeightValue), 2, MidpointRounding.AwayFromZero)
        Dim ceWeights = GetCertificationCEWeights(organizationId, organizationUnitId)
        Dim ceWeight = CType(ceWeights, IUserDefinedCertificationCEWeights).FindObject("Guid", uniqueId)
        If ceWeight IsNot Nothing Then
            Dim newCeWeight = CType(ceWeight, UserDefinedCertificationCEWeight)
            With newCeWeight
                .OrganizationId = organizationId
                .OrganizationUnitId = organizationUnitId
                .CEType = .CEType.List(ceType).ToCodeObject()
                .ProgramType = .ProgramType.List(programType).ToCodeObject()
                .MinCE = If(String.IsNullOrEmpty(minCE), 0, Convert.ToDecimal(minCE))
                .MaxCE = If(String.IsNullOrEmpty(minCE), 0, Convert.ToDecimal(maxCE))
                .Weight = weight
            End With
            ceWeights.Save()
            If ceWeights.ValidationIssues.Count > 0 Then
                StoreCacheObject(String.Format("_CC_CETypeSettings_{0}_{1}", organizationId, organizationUnitId), Nothing)
            End If
            Return ceWeights.ValidationIssues
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Deletes the CE weight setting.
    ''' </summary>
    ''' <param name="organizationId">The organization id.</param>
    ''' <param name="organizationUnitId">The organization unit id.</param>
    ''' <param name="uniqueId">The unique id.</param>
    Public Sub DeleteCEWeightSetting(ByVal organizationId As String, ByVal organizationUnitId As String, ByVal uniqueId As String)
        Dim ceWeights = GetCertificationCEWeights(organizationId, organizationUnitId)
        Dim ceWeight = CType(ceWeights, IUserDefinedCertificationCEWeights).FindObject("Guid", uniqueId)
        If ceWeight IsNot Nothing Then
            Dim newCeWeight = CType(ceWeight, UserDefinedCertificationCEWeight)
            ceWeights.Remove(newCeWeight)
            ceWeights.Save()
        End If
    End Sub

    Public Function GetUserDefinedCertificationCEWeights(ByVal organizationId As String, ByVal organizationUnitId As String, ByVal ceType As String, ByVal programType As String) As IUserDefinedCertificationCEWeights
        Dim certificationCEWeights = CType([Global].GetCollection(organizationId, organizationUnitId, NamespaceEnum.UserDefinedInfo,
                                                       "UserDefinedCertificationCEWeights"), 
                               IUserDefinedCertificationCEWeights)
        With certificationCEWeights.Filter
            .Add("OrganizationId", organizationId)
            .Add("OrganizationUnitId", organizationId)
            .Add("ProgramType", programType)
            .Add("CEType", ceType)
        End With
        certificationCEWeights.Fill()
        Dim ceweights = CType(certificationCEWeights, UserDefinedCertificationCEWeights)
        Return ceweights
    End Function
End Class
