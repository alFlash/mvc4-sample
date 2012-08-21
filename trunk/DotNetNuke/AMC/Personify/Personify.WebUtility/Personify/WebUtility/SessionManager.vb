Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Web
Imports System.Enum

Namespace Personify.WebUtility
    Public Class SessionManager
        ' Methods
        Public Shared Sub AddSessionObject(ByVal PortalId As Integer, ByVal Key As SessionKeys, ByVal ObjectToAdd As Object, ByVal Optional AppendToKey As String = "")
            Try 
                Dim name As String = ("Personify_" & PortalId.ToString & "_" & Key.ToString)
                If (AppendToKey <> "") Then
                    name = (name & "_" & AppendToKey)
                End If
                HttpContext.Current.Session.Add(name, RuntimeHelpers.GetObjectValue(ObjectToAdd))
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                ProjectData.ClearProjectError
            End Try
        End Sub

        Public Shared Sub ClearAllSessionObjects(ByVal PortalId As Integer)
            Try 
                Dim str2 As String
                For Each str2 In System.Enum.GetNames(GetType(SessionKeys))
                    Dim name As String = ("Personify_" & PortalId.ToString & "_" & str2.ToString)
                    HttpContext.Current.Session.Remove(name)
                Next
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                ProjectData.ClearProjectError
            End Try
        End Sub

        Public Shared Sub ClearSessionObject(ByVal PortalId As Integer, ByVal Key As SessionKeys, ByVal Optional AppendToKey As String = "")
            Dim name As String = ("Personify_" & PortalId.ToString & "_" & Key.ToString)
            If (AppendToKey <> "") Then
                name = (name & "_" & AppendToKey)
            End If
            Try 
                HttpContext.Current.Session.Remove(name)
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                ProjectData.ClearProjectError
            End Try
        End Sub

        Public Shared Function GetSessionObject(ByVal PortalId As Integer, ByVal Key As SessionKeys, ByVal Optional AppendToKey As String = "") As Object
            Try 
                Dim str As String = ("Personify_" & PortalId.ToString & "_" & Key.ToString)
                If (AppendToKey <> "") Then
                    str = (str & "_" & AppendToKey)
                End If
                Return HttpContext.Current.Session.Item(str)
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                ProjectData.ClearProjectError
            End Try
            Return Nothing
        End Function


        ' Nested Types
        Public Enum SessionKeys
            ' Fields
            PersonifyAddCustomerCommunicationGUIDKeys = &H11
            PersonifyAddCustomerGUIDKeys = 15
            PersonifyAddSegmentGUIDKeys = 14
            PersonifyCurrentAffiliateInfo = 7
            PersonifyCurrentSegmentInfo = 6
            PersonifyGroupActionInfo = 9
            PersonifyGroupPurchaseInfo = 10
            PersonifyMembershipRenewalsViewList = 11
            PersonifyOrder = 1
            PersonifyOrdersViewList = 13
            PersonifyPayOrderIds = 12
            PersonifySaveAddressGUIDKeys = 2
            PersonifySaveCommitteeMemberGUIDKeys = 4
            PersonifySaveCommunicationGUIDKeys = 3
            PersonifySaveEmployeeRelationshipGUIDKeys = 5
            PersonifyWebOrderMaster = &H10
        End Enum
    End Class
End Namespace

