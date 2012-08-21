Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports TIMSS
Imports TIMSS.API.Core
Imports TIMSS.API.WebInfo
Imports TIMSS.Enumerations

Namespace Personify.ApplicationManager
    Public Class AffiliateManagement
        Inherits BaseHelperClass
        ' Methods
        Public Sub New(ByVal OrgId As String, ByVal OrgUnitId As String)
            MyBase.New(OrgId, OrgUnitId)
        End Sub

        Public Sub New(ByVal OrgId As String, ByVal OrgUnitId As String, ByVal EnableOnDemandDataLoad As Boolean)
            MyBase.New(OrgId, OrgUnitId, EnableOnDemandDataLoad)
        End Sub

        Public Overridable Function GetCommitteeSegmentAffiliateList(ByVal PortalId As Integer, ByVal SegmentControllerMasterCustomerId As String, ByVal SegmentControllerSubCustomerId As Integer, ByVal SegmentRule As String, ByVal SegmentQualifier1 As String, ByVal SegmentQualifier2 As String) As IWebSegCommitteeMembersInfoViewList
            Return Me.GetCommitteeSegmentAffiliateList(PortalId, SegmentControllerMasterCustomerId, SegmentControllerSubCustomerId, SegmentRule, SegmentQualifier1, SegmentQualifier2, Nothing, DateTime.MinValue, Nothing, Nothing)
        End Function

        Public Overridable Function GetCommitteeSegmentAffiliateList(ByVal PortalId As Integer, ByVal SegmentControllerMasterCustomerId As String, ByVal SegmentControllerSubCustomerId As Integer, ByVal SegmentRule As String, ByVal SegmentQualifier1 As String, ByVal SegmentQualifier2 As String, ByVal CustomerIds As String) As IWebSegCommitteeMembersInfoViewList
            Return Me.GetCommitteeSegmentAffiliateList(PortalId, SegmentControllerMasterCustomerId, SegmentControllerSubCustomerId, SegmentRule, SegmentQualifier1, SegmentQualifier2, Nothing, DateTime.MinValue, Nothing, CustomerIds)
        End Function

        Public Overridable Function GetCommitteeSegmentAffiliateList(ByVal PortalId As Integer, ByVal SegmentControllerMasterCustomerId As String, ByVal SegmentControllerSubCustomerId As Integer, ByVal SegmentRule As String, ByVal SegmentQualifier1 As String, ByVal SegmentQualifier2 As String, ByVal LabelName As String, ByVal EndDate As DateTime, ByVal PositionCode As String) As IWebSegCommitteeMembersInfoViewList
            Return Me.GetCommitteeSegmentAffiliateList(PortalId, SegmentControllerMasterCustomerId, SegmentControllerSubCustomerId, SegmentRule, SegmentQualifier1, SegmentQualifier2, LabelName, EndDate, PositionCode, Nothing)
        End Function

        Public Overridable Function GetCommitteeSegmentAffiliateList(ByVal PortalId As Integer, ByVal SegmentControllerMasterCustomerId As String, ByVal SegmentControllerSubCustomerId As Integer, ByVal SegmentRule As String, ByVal SegmentQualifier1 As String, ByVal SegmentQualifier2 As String, ByVal LabelName As String, ByVal EndDate As DateTime, ByVal PositionCode As String, ByVal CustomerIds As String) As IWebSegCommitteeMembersInfoViewList
            Dim list2 As IWebSegCommitteeMembersInfoViewList = DirectCast(TIMSS.Global.GetCollection(Me.OrganizationId, Me.OrganizationUnitId, NamespaceEnum.WebInfo, "WebSegCommitteeMembersInfoViewList"), IWebSegCommitteeMembersInfoViewList)
            list2.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
            Dim filter As IFilter = list2.Filter
            filter.Add("CommitteeMasterCustomer", QueryOperatorEnum.Equals, SegmentControllerMasterCustomerId)
            filter.Add("CommitteeSubCustomer", QueryOperatorEnum.Equals, Conversions.ToString(SegmentControllerSubCustomerId))
            filter.Add("SegmentRuleCode", QueryOperatorEnum.Equals, SegmentRule)
            filter.Add("SegmentQualifier1", QueryOperatorEnum.Equals, SegmentQualifier1)
            filter.Add("SegmentQualifier2", QueryOperatorEnum.Equals, SegmentQualifier2)
            If (Not LabelName Is Nothing) Then
                filter.Add("LABELNAME", QueryOperatorEnum.StartsWith, LabelName)
            End If
            If (DateTime.Compare(EndDate, DateTime.MinValue) <> 0) Then
                filter.Add("ENDDATE", QueryOperatorEnum.LessThanOrEqual, Conversions.ToString(EndDate))
            End If
            If (Not PositionCode Is Nothing) Then
                filter.Add("POSITIONCODE", QueryOperatorEnum.StartsWith, PositionCode)
            End If
            If (Not CustomerIds Is Nothing) Then
                If (CustomerIds.Split(New Char() { ","c }).Length = 1) Then
                    CustomerIds = ("'" & CustomerIds & "'")
                End If
                filter.Add("CustomerId", QueryOperatorEnum.IsIn, CustomerIds)
            End If
            filter = Nothing
            list2.Fill(True)
            Return list2
        End Function

        Public Overridable Function GetEmployeeSegmentAffiliateList(ByVal PortalId As Integer, ByVal SegmentControllerMasterCustomerId As String, ByVal SegmentControllerSubCustomerId As Integer, ByVal SegmentRule As String, ByVal SegmentQualifier1 As String, ByVal SegmentQualifier2 As String) As IWebSegEmploymentEmployeesInfoViewList
            Return Me.GetEmployeeSegmentAffiliateList(PortalId, SegmentControllerMasterCustomerId, SegmentControllerSubCustomerId, SegmentRule, SegmentQualifier1, SegmentQualifier2, Nothing, DateTime.MinValue, Nothing, Nothing)
        End Function

        Public Overridable Function GetEmployeeSegmentAffiliateList(ByVal PortalId As Integer, ByVal SegmentControllerMasterCustomerId As String, ByVal SegmentControllerSubCustomerId As Integer, ByVal SegmentRule As String, ByVal SegmentQualifier1 As String, ByVal SegmentQualifier2 As String, ByVal CustomerIds As String) As IWebSegEmploymentEmployeesInfoViewList
            Return Me.GetEmployeeSegmentAffiliateList(PortalId, SegmentControllerMasterCustomerId, SegmentControllerSubCustomerId, SegmentRule, SegmentQualifier1, SegmentQualifier2, Nothing, DateTime.MinValue, Nothing, CustomerIds)
        End Function

        Public Overridable Function GetEmployeeSegmentAffiliateList(ByVal PortalId As Integer, ByVal SegmentControllerMasterCustomerId As String, ByVal SegmentControllerSubCustomerId As Integer, ByVal SegmentRule As String, ByVal SegmentQualifier1 As String, ByVal SegmentQualifier2 As String, ByVal LabelName As String, ByVal BeginDate As DateTime, ByVal RelationshipCode As String) As IWebSegEmploymentEmployeesInfoViewList
            Return Me.GetEmployeeSegmentAffiliateList(PortalId, SegmentControllerMasterCustomerId, SegmentControllerSubCustomerId, SegmentRule, SegmentQualifier1, SegmentQualifier2, LabelName, BeginDate, RelationshipCode, Nothing)
        End Function

        Public Overridable Function GetEmployeeSegmentAffiliateList(ByVal PortalId As Integer, ByVal SegmentControllerMasterCustomerId As String, ByVal SegmentControllerSubCustomerId As Integer, ByVal SegmentRule As String, ByVal SegmentQualifier1 As String, ByVal SegmentQualifier2 As String, ByVal LabelName As String, ByVal BeginDate As DateTime, ByVal RelationshipCode As String, ByVal CustomerIds As String) As IWebSegEmploymentEmployeesInfoViewList
            Dim list2 As IWebSegEmploymentEmployeesInfoViewList = DirectCast(TIMSS.Global.GetCollection(Me.OrganizationId, Me.OrganizationUnitId, NamespaceEnum.WebInfo, "WebSegEmploymentEmployeesInfoViewList"), IWebSegEmploymentEmployeesInfoViewList)
            list2.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
            list2.Filter.Add("RelatedMasterCustomerId", QueryOperatorEnum.Equals, SegmentControllerMasterCustomerId)
            list2.Filter.Add("RelatedSubCustomerId", QueryOperatorEnum.Equals, Conversions.ToString(SegmentControllerSubCustomerId))
            list2.Filter.Add("SegmentRuleCode", QueryOperatorEnum.Equals, SegmentRule)
            list2.Filter.Add("SegmentQualifier1", QueryOperatorEnum.Equals, SegmentQualifier1)
            list2.Filter.Add("SegmentQualifier2", QueryOperatorEnum.Equals, SegmentQualifier2)
            If (Not LabelName Is Nothing) Then
                list2.Filter.Add("LABELNAME", QueryOperatorEnum.StartsWith, LabelName)
            End If
            If (DateTime.Compare(BeginDate, DateTime.MinValue) <> 0) Then
                list2.Filter.Add("BEGINDATE", QueryOperatorEnum.GreaterThan, Conversions.ToString(BeginDate))
            End If
            If (Not RelationshipCode Is Nothing) Then
                list2.Filter.Add("RELATIONSHIPCODE", QueryOperatorEnum.StartsWith, RelationshipCode)
            End If
            If (Not CustomerIds Is Nothing) Then
                list2.Filter.Add("CustomerId", QueryOperatorEnum.IsIn, CustomerIds)
            End If
            list2.Fill(True)
            Return list2
        End Function

        Public Overridable Function GetGeographySegmentAffiliateList(ByVal PortalId As Integer, ByVal SegmentControllerMasterCustomerId As String, ByVal SegmentControllerSubCustomerId As String, ByVal SegmentRule As String, ByVal SegmentQualifier1 As String, ByVal SegmentQualifier2 As String) As IWebSegProductMembershipInfoViewList
            Return Me.GetGeographySegmentAffiliateList(PortalId, SegmentControllerMasterCustomerId, SegmentControllerSubCustomerId, SegmentRule, SegmentQualifier1, SegmentQualifier2, Nothing, Nothing, Nothing, Nothing, Nothing)
        End Function

        Public Overridable Function GetGeographySegmentAffiliateList(ByVal PortalId As Integer, ByVal SegmentControllerMasterCustomerId As String, ByVal SegmentControllerSubCustomerId As String, ByVal SegmentRule As String, ByVal SegmentQualifier1 As String, ByVal SegmentQualifier2 As String, ByVal CustomerIds As String) As IWebSegProductMembershipInfoViewList
            Return Me.GetGeographySegmentAffiliateList(PortalId, SegmentControllerMasterCustomerId, SegmentControllerSubCustomerId, SegmentRule, SegmentQualifier1, SegmentQualifier2, Nothing, Nothing, Nothing, Nothing, CustomerIds)
        End Function

        Public Overridable Function GetGeographySegmentAffiliateList(ByVal PortalId As Integer, ByVal SegmentControllerMasterCustomerId As String, ByVal SegmentControllerSubCustomerId As String, ByVal SegmentRule As String, ByVal SegmentQualifier1 As String, ByVal SegmentQualifier2 As String, ByVal LabelName As String, ByVal City As String, ByVal State As String, ByVal CountryCode As String) As IWebSegProductMembershipInfoViewList
            Return Me.GetGeographySegmentAffiliateList(PortalId, SegmentControllerMasterCustomerId, SegmentControllerSubCustomerId, SegmentRule, SegmentQualifier1, SegmentQualifier2, LabelName, City, State, CountryCode, Nothing)
        End Function

        Public Overridable Function GetGeographySegmentAffiliateList(ByVal PortalId As Integer, ByVal SegmentControllerMasterCustomerId As String, ByVal SegmentControllerSubCustomerId As String, ByVal SegmentRule As String, ByVal SegmentQualifier1 As String, ByVal SegmentQualifier2 As String, ByVal LabelName As String, ByVal City As String, ByVal State As String, ByVal CountryCode As String, ByVal CustomerIds As String) As IWebSegProductMembershipInfoViewList
            Dim list2 As IWebSegProductMembershipInfoViewList = DirectCast(TIMSS.Global.GetCollection(Me.OrganizationId, Me.OrganizationUnitId, NamespaceEnum.WebInfo, "WebSegProductMembershipInfoViewList"), IWebSegProductMembershipInfoViewList)
            list2.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
            list2.Filter.Add("SegmentRuleCode", QueryOperatorEnum.Equals, SegmentRule)
            list2.Filter.Add("SegmentQualifier1", QueryOperatorEnum.Equals, SegmentQualifier1)
            list2.Filter.Add("SegmentQualifier2", QueryOperatorEnum.Equals, SegmentQualifier2)
            If (Not LabelName Is Nothing) Then
                list2.Filter.Add("LABELNAME", QueryOperatorEnum.StartsWith, LabelName)
            End If
            If (Not City Is Nothing) Then
                list2.Filter.Add("CITY", QueryOperatorEnum.Equals, City)
            End If
            If (Not State Is Nothing) Then
                list2.Filter.Add("STATE", QueryOperatorEnum.Equals, State)
            End If
            If (Not CountryCode Is Nothing) Then
                list2.Filter.Add("COUNTRYCODE", QueryOperatorEnum.Equals, CountryCode)
            End If
            If (Not CustomerIds Is Nothing) Then
                list2.Filter.Add("CustomerId", QueryOperatorEnum.IsIn, CustomerIds)
            End If
            list2.Fill(True)
            Return list2
        End Function

        Public Overridable Function GetMembershipProductSegmentAffiliateList(ByVal PortalId As Integer, ByVal SegmentControllerMasterCustomerId As String, ByVal SegmentControllerSubCustomerId As String, ByVal SegmentRule As String, ByVal SegmentQualifier1 As String, ByVal SegmentQualifier2 As String) As IWebSegProductMembershipInfoViewList
            Return Me.GetMembershipProductSegmentAffiliateList(PortalId, SegmentControllerMasterCustomerId, SegmentControllerSubCustomerId, SegmentRule, SegmentQualifier1, SegmentQualifier2, Nothing, DateTime.MinValue, DateTime.MinValue, Nothing)
        End Function

        Public Overridable Function GetMembershipProductSegmentAffiliateList(ByVal PortalId As Integer, ByVal SegmentControllerMasterCustomerId As String, ByVal SegmentControllerSubCustomerId As String, ByVal SegmentRule As String, ByVal SegmentQualifier1 As String, ByVal SegmentQualifier2 As String, ByVal CustomerIds As String) As IWebSegProductMembershipInfoViewList
            Return Me.GetMembershipProductSegmentAffiliateList(PortalId, SegmentControllerMasterCustomerId, SegmentControllerSubCustomerId, SegmentRule, SegmentQualifier1, SegmentQualifier2, Nothing, DateTime.MinValue, DateTime.MinValue, CustomerIds)
        End Function

        Public Overridable Function GetMembershipProductSegmentAffiliateList(ByVal PortalId As Integer, ByVal SegmentControllerMasterCustomerId As String, ByVal SegmentControllerSubCustomerId As String, ByVal SegmentRule As String, ByVal SegmentQualifier1 As String, ByVal SegmentQualifier2 As String, ByVal SearchName As String, ByVal InitialBeginDate As DateTime, ByVal GraceDate As DateTime) As IWebSegProductMembershipInfoViewList
            Return Me.GetMembershipProductSegmentAffiliateList(PortalId, SegmentControllerMasterCustomerId, SegmentControllerSubCustomerId, SegmentRule, SegmentQualifier1, SegmentQualifier2, SearchName, InitialBeginDate, GraceDate, Nothing)
        End Function

        Public Overridable Function GetMembershipProductSegmentAffiliateList(ByVal PortalId As Integer, ByVal SegmentControllerMasterCustomerId As String, ByVal SegmentControllerSubCustomerId As String, ByVal SegmentRule As String, ByVal SegmentQualifier1 As String, ByVal SegmentQualifier2 As String, ByVal SearchName As String, ByVal InitialBeginDate As DateTime, ByVal GraceDate As DateTime, ByVal CustomerIds As String) As IWebSegProductMembershipInfoViewList
            Dim list2 As IWebSegProductMembershipInfoViewList = DirectCast(TIMSS.Global.GetCollection(Me.OrganizationId, Me.OrganizationUnitId, NamespaceEnum.WebInfo, "WebSegProductMembershipInfoViewList"), IWebSegProductMembershipInfoViewList)
            list2.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
            list2.Filter.Add("SegmentRuleCode", QueryOperatorEnum.Equals, SegmentRule)
            list2.Filter.Add("SegmentQualifier1", QueryOperatorEnum.Equals, SegmentQualifier1)
            list2.Filter.Add("SegmentQualifier2", QueryOperatorEnum.Equals, SegmentQualifier2)
            If (Not SearchName Is Nothing) Then
                list2.Filter.Add("SEARCHNAME", QueryOperatorEnum.StartsWith, SearchName)
            End If
            If (DateTime.Compare(InitialBeginDate, DateTime.MinValue) <> 0) Then
                list2.Filter.Add("INITIALBEGINDATE", QueryOperatorEnum.Equals, Conversions.ToString(InitialBeginDate))
            End If
            If (DateTime.Compare(GraceDate, DateTime.MinValue) <> 0) Then
                list2.Filter.Add("GRACEDATE", QueryOperatorEnum.Equals, Conversions.ToString(GraceDate))
            End If
            If (Not CustomerIds Is Nothing) Then
                list2.Filter.Add("CustomerId", QueryOperatorEnum.IsIn, CustomerIds)
            End If
            list2.Fill(True)
            Return list2
        End Function

        Public Overridable Function GetMiscellaneousSegmentAffiliateList(ByVal PortalId As Integer, ByVal SegmentControllerMasterCustomerId As String, ByVal SegmentControllerSubCustomerId As String, ByVal SegmentRule As String, ByVal SegmentQualifier1 As String, ByVal SegmentQualifier2 As String) As IWebSegProductOthersInfoViewList
            Return Me.GetMiscellaneousSegmentAffiliateList(PortalId, SegmentControllerMasterCustomerId, SegmentControllerSubCustomerId, SegmentRule, SegmentQualifier1, SegmentQualifier2, Nothing, Nothing, DateTime.MinValue, Nothing)
        End Function

        Public Overridable Function GetMiscellaneousSegmentAffiliateList(ByVal PortalId As Integer, ByVal SegmentControllerMasterCustomerId As String, ByVal SegmentControllerSubCustomerId As String, ByVal SegmentRule As String, ByVal SegmentQualifier1 As String, ByVal SegmentQualifier2 As String, ByVal CustomerIds As String) As IWebSegProductOthersInfoViewList
            Return Me.GetMiscellaneousSegmentAffiliateList(PortalId, SegmentControllerMasterCustomerId, SegmentControllerSubCustomerId, SegmentRule, SegmentQualifier1, SegmentQualifier2, Nothing, Nothing, DateTime.MinValue, CustomerIds)
        End Function

        Public Overridable Function GetMiscellaneousSegmentAffiliateList(ByVal PortalId As Integer, ByVal SegmentControllerMasterCustomerId As String, ByVal SegmentControllerSubCustomerId As String, ByVal SegmentRule As String, ByVal SegmentQualifier1 As String, ByVal SegmentQualifier2 As String, ByVal LabelName As String, ByVal CompanyName As String, ByVal OrderDate As DateTime) As IWebSegProductOthersInfoViewList
            Return Me.GetMiscellaneousSegmentAffiliateList(PortalId, SegmentControllerMasterCustomerId, SegmentControllerSubCustomerId, SegmentRule, SegmentQualifier1, SegmentQualifier2, LabelName, CompanyName, OrderDate, Nothing)
        End Function

        Public Overridable Function GetMiscellaneousSegmentAffiliateList(ByVal PortalId As Integer, ByVal SegmentControllerMasterCustomerId As String, ByVal SegmentControllerSubCustomerId As String, ByVal SegmentRule As String, ByVal SegmentQualifier1 As String, ByVal SegmentQualifier2 As String, ByVal LabelName As String, ByVal CompanyName As String, ByVal OrderDate As DateTime, ByVal CustomerIds As String) As IWebSegProductOthersInfoViewList
            Dim list2 As IWebSegProductOthersInfoViewList = DirectCast(TIMSS.Global.GetCollection(Me.OrganizationId, Me.OrganizationUnitId, NamespaceEnum.WebInfo, "WebSegProductOthersInfoViewList"), IWebSegProductOthersInfoViewList)
            list2.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
            list2.Filter.Add("SegmentRuleCode", QueryOperatorEnum.Equals, SegmentRule)
            list2.Filter.Add("SegmentQualifier1", QueryOperatorEnum.Equals, SegmentQualifier1)
            list2.Filter.Add("SegmentQualifier2", QueryOperatorEnum.Equals, SegmentQualifier2)
            If (Not LabelName Is Nothing) Then
                list2.Filter.Add("LABELNAME", QueryOperatorEnum.StartsWith, LabelName)
            End If
            If (Not CompanyName Is Nothing) Then
                list2.Filter.Add("COMPANYNAME", QueryOperatorEnum.StartsWith, CompanyName)
            End If
            If (DateTime.Compare(OrderDate, DateTime.MinValue) <> 0) Then
                list2.Filter.Add("ORDERDATE", QueryOperatorEnum.Equals, Conversions.ToString(OrderDate))
            End If
            If (Not CustomerIds Is Nothing) Then
                list2.Filter.Add("CustomerId", QueryOperatorEnum.IsIn, CustomerIds)
            End If
            list2.Fill(True)
            Return list2
        End Function

        Public Overridable Function GetSegmentList(ByVal PortalId As Integer, ByVal SegmentControllerMasterCustomerId As String, ByVal SegmentControllerSubCustomerId As Integer, ByVal FilterByCanPlaceOrderFlag As Boolean) As IWebCustomerSegmentControlViewList
            Dim list2 As IWebCustomerSegmentControlViewList = DirectCast(TIMSS.Global.GetCollection(Me.OrganizationId, Me.OrganizationUnitId, NamespaceEnum.WebInfo, "WebCustomerSegmentControlViewList"), IWebCustomerSegmentControlViewList)
            list2.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
            Dim list3 As IWebCustomerSegmentControlViewList = list2
            list3.Filter.Add("MasterCustomerId", QueryOperatorEnum.Equals, SegmentControllerMasterCustomerId)
            list3.Filter.Add("SubCustomerId", QueryOperatorEnum.Equals, Conversions.ToString(SegmentControllerSubCustomerId))
            If FilterByCanPlaceOrderFlag Then
                list3.Filter.Add("CanPlaceOrderFlag", QueryOperatorEnum.Equals, "Y")
            End If
            list3.Fill(True)
            list3 = Nothing
            Return list2
        End Function

        Public Overridable Function IsGroupPurchaseEnabledForSegmentController(ByVal PortalId As Integer, ByVal SegmentControllerMasterCustomerId As String, ByVal SegmentControllerSubCustomerId As Integer) As Boolean
            Dim flag As Boolean
            Dim list As IWebCustomerSegmentControlViewList
            Dim key As String = String.Concat(New Object() { "IsControllerAllowedGroupPurchase", PortalId, SegmentControllerMasterCustomerId, SegmentControllerSubCustomerId })
            If (PersonifyDataCache.Fetch(key) Is Nothing) Then
                list = DirectCast(TIMSS.Global.GetCollection(Me.OrganizationId, Me.OrganizationUnitId, NamespaceEnum.WebInfo, "WebCustomerSegmentControlViewList"), IWebCustomerSegmentControlViewList)
                list.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
                Dim list2 As IWebCustomerSegmentControlViewList = list
                list2.Filter.Add("MasterCustomerId", QueryOperatorEnum.Equals, SegmentControllerMasterCustomerId)
                list2.Filter.Add("SubCustomerId", QueryOperatorEnum.Equals, Conversions.ToString(SegmentControllerSubCustomerId))
                list2.Fill(True)
                list2 = Nothing
                PersonifyDataCache.Store(key, list)
            End If
            list = DirectCast(PersonifyDataCache.Fetch(key), IWebCustomerSegmentControlViewList)
            If (list.Count > 0) Then
                Dim num2 As Integer = (list.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num2)
                    Dim view As IWebCustomerSegmentControlView = list.Item(i)
                    Dim view2 As IWebCustomerSegmentControlView = view
                    If view2.CanPlaceOrderFlag Then
                        Return True
                    End If
                    view2 = Nothing
                    i += 1
                Loop
            End If
            Return flag
        End Function

    End Class
End Namespace

