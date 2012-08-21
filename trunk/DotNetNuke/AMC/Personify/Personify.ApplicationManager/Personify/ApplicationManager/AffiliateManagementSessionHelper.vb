Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Runtime.InteropServices

Namespace Personify.ApplicationManager
    <Serializable> _
    Public Class AffiliateManagementSessionHelper
        ' Methods
        Public Shared Sub CreateAffiliateGroupActionInfo(ByVal PortalId As Integer, ByVal ActionType As enmGroupAction, ByVal arrGroupActionInfo As GroupActionAffiliateInfo())
            Dim infoArray As GroupActionAffiliateInfo() = arrGroupActionInfo
            If (infoArray.Length > 0) Then
                Dim objectToAdd As New GroupActionInfo
                objectToAdd.ActionType = ActionType
                objectToAdd.AffiliateInfo = arrGroupActionInfo
                SessionManager.AddSessionObject(PortalId, PersonifyEnumerations.SessionKeys.PersonifyGroupActionInfo, objectToAdd, "")
            End If
            infoArray = Nothing
        End Sub

        Public Shared Function GetAffiliateGroupActionInfo(ByVal PortalId As Integer) As GroupActionAffiliateInfo()
            Dim info2 As GroupActionInfo = DirectCast(SessionManager.GetSessionObject(PortalId, PersonifyEnumerations.SessionKeys.PersonifyGroupActionInfo, ""), GroupActionInfo)
            Return info2.AffiliateInfo
        End Function

        Public Shared Function GetAffiliateGroupActionType(ByVal PortalId As Integer) As enmGroupAction
            Dim info2 As GroupActionInfo = DirectCast(SessionManager.GetSessionObject(PortalId, PersonifyEnumerations.SessionKeys.PersonifyGroupActionInfo, ""), GroupActionInfo)
            Return info2.ActionType
        End Function

        Public Shared Function GetCurrentAffiliateCustomerId(ByVal PortalId As Integer) As CustomerId
            Dim affiliateCustomerId As CustomerId
            Try 
                Dim info As AffiliateInfo = DirectCast(SessionManager.GetSessionObject(PortalId, PersonifyEnumerations.SessionKeys.PersonifyCurrentAffiliateInfo, ""), AffiliateInfo)
                affiliateCustomerId = info.AffiliateCustomerId
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                Throw exception
            End Try
            Return affiliateCustomerId
        End Function

        Public Shared Function GetCurrentAffiliateInfo(ByVal PortalId As Integer) As AffiliateInfo
            Dim info As AffiliateInfo
            Try 
                Dim info2 As AffiliateInfo = DirectCast(SessionManager.GetSessionObject(PortalId, PersonifyEnumerations.SessionKeys.PersonifyCurrentAffiliateInfo, ""), AffiliateInfo)
                info = info2
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                Throw exception
            End Try
            Return info
        End Function

        Public Shared Function GetCurrentSegmentGroupPurchaseActionURL(ByVal PortalId As Integer) As Integer
            Dim affiliateListGroupPurchaseTabId As Integer
            Try 
                Dim currentSegmentInfo As SegmentInfo
                If AffiliateManagementSessionHelper.IsManagingSegment(PortalId) Then
                    currentSegmentInfo = AffiliateManagementSessionHelper.GetCurrentSegmentInfo(PortalId)
                Else
                    Return 0
                End If
                affiliateListGroupPurchaseTabId = currentSegmentInfo.AffiliateListGroupPurchaseTabId
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                Throw exception
            End Try
            Return affiliateListGroupPurchaseTabId
        End Function

        Public Shared Sub GetCurrentSegmentIdentifier(ByVal PortalId As Integer, ByRef SegmentType As String, ByRef SegmentQualifier1 As String, ByRef SegmentQualifier2 As String)
            Try 
                If AffiliateManagementSessionHelper.IsManagingSegment(PortalId) Then
                    Dim currentSegmentInfo As SegmentInfo = AffiliateManagementSessionHelper.GetCurrentSegmentInfo(PortalId)
                    SegmentType = currentSegmentInfo.SegmentType
                    SegmentQualifier1 = currentSegmentInfo.SegmentQualifier1
                    SegmentQualifier2 = currentSegmentInfo.SegmentQualifier2
                End If
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                Throw exception
            End Try
        End Sub

        Public Shared Function GetCurrentSegmentInfo(ByVal PortalId As Integer) As SegmentInfo
            Return DirectCast(SessionManager.GetSessionObject(PortalId, PersonifyEnumerations.SessionKeys.PersonifyCurrentSegmentInfo, ""), SegmentInfo)
        End Function

        Public Shared Function GetGroupPurchaseCartGUID(ByVal PortalId As Integer) As String
            Return AffiliateManagementSessionHelper.GetGroupPurchaseInfo(PortalId).CartGUID
        End Function

        Public Shared Function GetGroupPurchaseInfo(ByVal PortalId As Integer) As GroupActionInfo
            Dim info3 As GroupActionInfo
            Dim info2 As GroupActionInfo = DirectCast(SessionManager.GetSessionObject(PortalId, PersonifyEnumerations.SessionKeys.PersonifyGroupPurchaseInfo, ""), GroupActionInfo)
            If (info2.CartGUID <> String.Empty) Then
                Return info2
            End If
            Return info3
        End Function

        Public Shared Function GetMemberInfoForCurrentSegment(ByVal PortalId As Integer) As CustomerId
            Dim id As CustomerId
            Try 
                Dim id2 As CustomerId
                If AffiliateManagementSessionHelper.IsManagingSegment(PortalId) Then
                    Return AffiliateManagementSessionHelper.GetCurrentSegmentInfo(PortalId).SegmentMember
                End If
                id = id2
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                Throw exception
            End Try
            Return id
        End Function

        Public Shared Function IsManagingAffiliate(ByVal PortalId As Integer) As Boolean
            Dim flag As Boolean
            Try
                Dim obj As Object = SessionManager.GetSessionObject(PortalId, PersonifyEnumerations.SessionKeys.PersonifyCurrentAffiliateInfo, "")

                If Not obj Is Nothing Then
                    Dim info As AffiliateInfo = DirectCast(obj, AffiliateInfo)
                    If (Not info.AffiliateCustomerId.MasterCustomerId Is Nothing) Then
                        Return True
                    End If
                End If
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                Throw exception
            End Try
            Return flag
        End Function

        Public Shared Function IsManagingSegment(ByVal PortalId As Integer) As Boolean
            Dim flag As Boolean
            Try 
                Dim currentSegmentInfo As SegmentInfo = AffiliateManagementSessionHelper.GetCurrentSegmentInfo(PortalId)
                If ((currentSegmentInfo.SegmentType Is Nothing) OrElse (currentSegmentInfo.SegmentType = String.Empty)) Then
                    Return False
                End If
                flag = True
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                Throw exception
            End Try
            Return flag
        End Function

        Public Shared Sub ManageAffiliate(ByVal PortalId As Integer, ByVal AffiliateMasterCustomerId As String, ByVal AffiliateSubCustomerId As Integer, ByVal AffiliateLabelName As String, ByVal EditProfileFlag As Boolean)
            Try 
                If AffiliateManagementSessionHelper.IsManagingSegment(PortalId) Then
                    Dim objectToAdd As New AffiliateInfo
                    objectToAdd.AffiliateCustomerId.MasterCustomerId = AffiliateMasterCustomerId
                    objectToAdd.AffiliateCustomerId.SubCustomerId = AffiliateSubCustomerId
                    objectToAdd.AffiliateLabelName = AffiliateLabelName
                    objectToAdd.EditProfileFlag = EditProfileFlag
                    SessionManager.AddSessionObject(PortalId, PersonifyEnumerations.SessionKeys.PersonifyCurrentAffiliateInfo, objectToAdd, "")
                End If
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                Throw exception
            End Try
        End Sub

        Public Shared Sub ManageSegmentOwner(ByVal PortalId As Integer)
            Try 
                If AffiliateManagementSessionHelper.IsManagingSegment(PortalId) Then
                    Dim currentSegmentInfo As SegmentInfo = AffiliateManagementSessionHelper.GetCurrentSegmentInfo(PortalId)
                    Dim objectToAdd As New AffiliateInfo
                    objectToAdd.AffiliateCustomerId.MasterCustomerId = currentSegmentInfo.SegmentOwner.MasterCustomerId
                    objectToAdd.AffiliateCustomerId.SubCustomerId = currentSegmentInfo.SegmentOwner.SubCustomerId
                    objectToAdd.AffiliateLabelName = currentSegmentInfo.SegmentDescr
                    objectToAdd.EditProfileFlag = True
                    SessionManager.AddSessionObject(PortalId, PersonifyEnumerations.SessionKeys.PersonifyCurrentAffiliateInfo, objectToAdd, "")
                End If
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                Throw exception
            End Try
        End Sub

        Public Shared Sub RemoveAffiliateGroupActionInfo(ByVal PortalId As Integer)
            SessionManager.ClearSessionObject(PortalId, PersonifyEnumerations.SessionKeys.PersonifyGroupActionInfo, "")
        End Sub

        Private Shared Sub RemoveCurrentAffiliate(ByVal PortalId As Integer)
            SessionManager.ClearSessionObject(PortalId, PersonifyEnumerations.SessionKeys.PersonifyCurrentAffiliateInfo, "")
        End Sub

        Public Shared Sub RemoveCurrentSegmentInfo(ByVal PortalId As Integer)
            SessionManager.ClearSessionObject(PortalId, PersonifyEnumerations.SessionKeys.PersonifyCurrentSegmentInfo, "")
        End Sub

        Public Shared Sub RemoveGroupPurchaseInfo(ByVal PortalId As Integer)
            SessionManager.ClearSessionObject(PortalId, PersonifyEnumerations.SessionKeys.PersonifyGroupPurchaseInfo, "")
        End Sub

        Public Shared Sub SetCurrentSegmentGroupPurchaseActionURL(ByVal PortalId As Integer, ByVal GroupPurchaseActionURL As Integer)
            Try 
                If AffiliateManagementSessionHelper.IsManagingSegment(PortalId) Then
                    Dim currentSegmentInfo As SegmentInfo = AffiliateManagementSessionHelper.GetCurrentSegmentInfo(PortalId)
                    currentSegmentInfo.AffiliateListGroupPurchaseTabId = GroupPurchaseActionURL
                    SessionManager.AddSessionObject(PortalId, PersonifyEnumerations.SessionKeys.PersonifyCurrentSegmentInfo, currentSegmentInfo, "")
                End If
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                Throw exception
            End Try
        End Sub

        Public Shared Sub SetCurrentSegmentInfo(ByVal PortalId As Integer, ByVal SegmentType As String, ByVal SegmentQualifier1 As String, ByVal SegmentQualifier2 As String, ByVal Subsystem As String, ByVal strucSegmentOwner As CustomerId, ByVal SegmentDescr As String, ByVal ReadOnlyFlag As Boolean, ByVal CanAddMemberFlag As Boolean, ByVal CanRemoveMemberFlag As Boolean, ByVal CanPlaceOrderFlag As Boolean, ByVal CanEditSegmentOwnerFlag As Boolean, ByVal SegmentListTabId As Integer, ByVal AffiliateListTabId As Integer, ByVal Optional AffiliateListGroupPurchaseTabId As Integer = 0)
            Try 
                Dim currentSegmentInfo As SegmentInfo
                If AffiliateManagementSessionHelper.IsManagingSegment(PortalId) Then
                    currentSegmentInfo = AffiliateManagementSessionHelper.GetCurrentSegmentInfo(PortalId)
                Else
                    currentSegmentInfo = New SegmentInfo
                End If
                currentSegmentInfo.SegmentType = SegmentType
                currentSegmentInfo.SegmentQualifier1 = SegmentQualifier1
                currentSegmentInfo.SegmentQualifier2 = SegmentQualifier2
                currentSegmentInfo.Subsystem = Subsystem
                currentSegmentInfo.SegmentOwner = strucSegmentOwner
                currentSegmentInfo.SegmentDescr = SegmentDescr
                currentSegmentInfo.ReadOnlyFlag = ReadOnlyFlag
                currentSegmentInfo.CanAddMemberFlag = CanAddMemberFlag
                currentSegmentInfo.CanRemoveMemberFlag = CanRemoveMemberFlag
                currentSegmentInfo.CanPlaceOrderFlag = CanPlaceOrderFlag
                currentSegmentInfo.CanEditSegmentOwnerFlag = CanEditSegmentOwnerFlag
                currentSegmentInfo.SegmentListTabId = SegmentListTabId
                currentSegmentInfo.AffiliateListTabId = AffiliateListTabId
                currentSegmentInfo.AffiliateListGroupPurchaseTabId = AffiliateListGroupPurchaseTabId
                SessionManager.AddSessionObject(PortalId, PersonifyEnumerations.SessionKeys.PersonifyCurrentSegmentInfo, currentSegmentInfo, "")
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                Throw exception
            End Try
        End Sub

        Public Shared Sub SetGroupPurchaseInfo(ByVal PortalId As Integer, ByVal CartGUID As String)
            Dim objectToAdd As New GroupActionInfo
            objectToAdd.ActionType = enmGroupAction.PURCHASE
            objectToAdd.CartGUID = CartGUID
            SessionManager.AddSessionObject(PortalId, PersonifyEnumerations.SessionKeys.PersonifyGroupPurchaseInfo, objectToAdd, "")
        End Sub

        Public Shared Sub SetMemberInfoForCurrentSegment(ByVal PortalId As Integer, ByVal MemberMasterCustomerId As String, ByVal MemberSubCustomerId As Integer)
            Try 
                If AffiliateManagementSessionHelper.IsManagingSegment(PortalId) Then
                    Dim currentSegmentInfo As SegmentInfo = AffiliateManagementSessionHelper.GetCurrentSegmentInfo(PortalId)
                    currentSegmentInfo.SegmentMember.MasterCustomerId = MemberMasterCustomerId
                    currentSegmentInfo.SegmentMember.SubCustomerId = MemberSubCustomerId
                    SessionManager.AddSessionObject(PortalId, PersonifyEnumerations.SessionKeys.PersonifyCurrentSegmentInfo, currentSegmentInfo, "")
                End If
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                Throw exception
            End Try
        End Sub

        Public Shared Sub StopManagingAffiliate(ByVal PortalId As Integer)
            AffiliateManagementSessionHelper.RemoveCurrentAffiliate(PortalId)
        End Sub


        ' Fields
        Private Const C_SEGMENT_COMMITTEE As String = "COMMITTEE"
        Private Const C_SEGMENT_EMPLOYEE As String = "EMPLOYEE"
        Private Const C_SEGMENT_GEOGRAPHY As String = "GEOGRAPHY"
        Private Const C_SEGMENT_PRODUCT As String = "PRODUCT"

        ' Nested Types
        <StructLayout(LayoutKind.Sequential)> _
        Public Structure AffiliateInfo
            Public AffiliateCustomerId As CustomerId
            Public AffiliateLabelName As String
            Public EditProfileFlag As Boolean
        End Structure

        <StructLayout(LayoutKind.Sequential)> _
        Public Structure CustomerId
            Public MasterCustomerId As String
            Public SubCustomerId As Integer
        End Structure

        Public Enum enmGroupAction
            ' Fields
            PRINTROSTER = 2
            PURCHASE = 3
            SENDEMAIL = 1
        End Enum

        Public Enum enmGroupPurchaseAction
            ' Fields
            CONFIRMPURCHASE = 2
            NONE = 0
            SELECTMEMBERS = 1
        End Enum

        <StructLayout(LayoutKind.Sequential)> _
        Public Structure GroupActionAffiliateInfo
            Public CustomerIdKey As String
            Public AffiliateCustomerId As CustomerId
            Public AffiliateLabelName As String
            Public EmailAddress As String
        End Structure

        <StructLayout(LayoutKind.Sequential)> _
        Public Structure GroupActionInfo
            Public ActionType As enmGroupAction
            Public CartGUID As String
            Public AffiliateInfo As GroupActionAffiliateInfo()
        End Structure

        <StructLayout(LayoutKind.Sequential)> _
        Public Structure SegmentInfo
            Public SegmentType As String
            Public SegmentDescr As String
            Public Subsystem As String
            Public SegmentQualifier1 As String
            Public SegmentQualifier2 As String
            Public SegmentOwner As CustomerId
            Public SegmentMember As CustomerId
            Public ReadOnlyFlag As Boolean
            Public CanAddMemberFlag As Boolean
            Public CanRemoveMemberFlag As Boolean
            Public CanPlaceOrderFlag As Boolean
            Public CanEditSegmentOwnerFlag As Boolean
            Public SegmentListTabId As Integer
            Public AffiliateListTabId As Integer
            Public AffiliateListGroupPurchaseTabId As Integer
            Public ReferrerURL As String()
        End Structure
    End Class
End Namespace

