Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Collections
Imports TIMSS
Imports TIMSS.API.Core
Imports TIMSS.API.CustomerInfo
Imports TIMSS.API.WebInfo
Imports TIMSS.Enumerations

Namespace Personify.ApplicationManager
    Public Class SortByLocation
        Inherits BaseHelperClass
        Implements IComparer
        ' Methods
        Public Sub New(ByVal OrgId As String, ByVal OrgUnitId As String)
            MyBase.New(OrgId, OrgUnitId)
        End Sub

        Public Sub New(ByVal OrgId As String, ByVal OrgUnitId As String, ByVal EnableOnDemandDataLoad As Boolean)
            MyBase.New(OrgId, OrgUnitId, EnableOnDemandDataLoad)
        End Sub

        Public Overridable Function [Compare](ByVal product1 As Object, ByVal product2 As Object) As Integer Implements IComparer.Compare
            Dim num As Integer
            Dim view As ITmarWebProductView = DirectCast(product1, ITmarWebProductView)
            Dim view2 As ITmarWebProductView = DirectCast(product2, ITmarWebProductView)
            Dim primaryAddress As ICustomerAddressViewList = Me.GetPrimaryAddress(view.FacilityMasterCustomerId, Conversions.ToString(view.FacilitySubCustomerId))
            Dim list2 As ICustomerAddressViewList = Me.GetPrimaryAddress(view2.FacilityMasterCustomerId, Conversions.ToString(view2.FacilitySubCustomerId))
            If ((Not primaryAddress.Item(0) Is Nothing) And (Not list2.Item(0) Is Nothing)) Then
                If (String.Compare(primaryAddress.Item(0).City, list2.Item(0).City) > 0) Then
                    Return -1
                End If
                Return String.Compare(primaryAddress.Item(0).State, list2.Item(0).State)
            End If
            If (primaryAddress.Item(0) Is Nothing) Then
                Return 0
            End If
            If (list2.Item(0) Is Nothing) Then
                num = -1
            End If
            Return num
        End Function

        Private Function GetPrimaryAddress(ByVal pMasterCustomerId As String, ByVal pSubCustomerId As String) As ICustomerAddressViewList
            Dim list2 As ICustomerAddressViewList = DirectCast(TIMSS.Global.GetCollection(Me.OrganizationId, Me.OrganizationUnitId, NamespaceEnum.CustomerInfo, "CustomerAddressViewList"), ICustomerAddressViewList)
            list2.EnableIncrementalDataLoad = Me.EnableOnDemandDataLoad
            Dim filter As IFilter = list2.Filter
            filter.Add("MasterCustomerId", QueryOperatorEnum.Equals, pMasterCustomerId)
            filter.Add("SubCustomerId", QueryOperatorEnum.Equals, pSubCustomerId)
            filter.Add("PrioritySeq", Conversions.ToString(0))
            filter = Nothing
            list2.Fill
            Return list2
        End Function


        ' Fields
        Private PortalId As Integer
    End Class
End Namespace

