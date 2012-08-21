Imports DotNetNuke.UI.Utilities
Imports System
Imports System.Collections
Imports System.Runtime.CompilerServices
Imports System.Web.Caching
Imports System.Web

Namespace Personify.WebUtility
    <Serializable> _
    Public Class DataCache
        ' Methods
        Public Shared Sub ClearCache()
            Dim list As ArrayList = DirectCast(DataCache.Fetch(DataCache._CacheKeyList), ArrayList)
            If (Not list Is Nothing) Then
                Dim num2 As Integer = (list.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num2)
                    DataCache.Remove(list.Item(i).ToString)
                    i += 1
                Loop
                DataCache.Remove(DataCache._CacheKeyList)
            End If
        End Sub

        Public Shared Function Fetch(ByVal Key As String) As Object
            Return DotNetNuke.UI.Utilities.DataCache.GetCache(Key)
        End Function

        Private Shared Sub MaintainCacheKeyList(ByVal Key As String)
            Dim objObject As ArrayList = DirectCast(DataCache.Fetch(DataCache._CacheKeyList), ArrayList)
            If (objObject Is Nothing) Then
                objObject = New ArrayList
            End If
            objObject.Add(Key)
            DotNetNuke.UI.Utilities.DataCache.SetCache(DataCache._CacheKeyList, objObject)
        End Sub

        Public Shared Sub Remove(ByVal Key As String)
            DotNetNuke.UI.Utilities.DataCache.RemoveCache(Key)
        End Sub

        Public Shared Function Store(ByVal Key As String, ByVal CacheObject As Object) As Boolean
            Dim flag As Boolean
            DotNetNuke.UI.Utilities.DataCache.SetCache(Key, RuntimeHelpers.GetObjectValue(CacheObject))
            DataCache.MaintainCacheKeyList(Key)
            Return flag
        End Function

        Public Shared Function Store(ByVal Key As String, ByVal CacheObject As Object, ByVal AbsoluteExpiration As DateTime) As Boolean
            Dim flag As Boolean
            DotNetNuke.UI.Utilities.DataCache.SetCache(Key, RuntimeHelpers.GetObjectValue(CacheObject), AbsoluteExpiration)
            DataCache.MaintainCacheKeyList(Key)
            Return flag
        End Function

        Public Shared Function Store(ByVal Key As String, ByVal CacheObject As Object, ByVal SlidingExpiration As Integer) As Boolean
            Dim flag As Boolean
            DotNetNuke.UI.Utilities.DataCache.SetCache(Key, RuntimeHelpers.GetObjectValue(CacheObject), SlidingExpiration)
            DataCache.MaintainCacheKeyList(Key)
            Return flag
        End Function

        Public Shared Function Store(ByVal Key As String, ByVal CacheObject As Object, ByVal ObjDependancy As CacheDependency) As Boolean
            Dim flag As Boolean
            DotNetNuke.UI.Utilities.DataCache.SetCache(Key, RuntimeHelpers.GetObjectValue(CacheObject), ObjDependancy)
            DataCache.MaintainCacheKeyList(Key)
            Return flag
        End Function

        Public Shared Function Store(ByVal Key As String, ByVal CacheObject As Object, ByVal ObjDependancy As Object, ByVal AbsoluteExpiration As DateTime, ByVal SlidingExpiration As TimeSpan) As Boolean
            Dim flag As Boolean
            DotNetNuke.UI.Utilities.DataCache.SetCache(Key, RuntimeHelpers.GetObjectValue(CacheObject), DirectCast(ObjDependancy, CacheDependency), AbsoluteExpiration, SlidingExpiration)
            DataCache.MaintainCacheKeyList(Key)
            Return flag
        End Function

       




        ' Fields
        Private Shared _CacheKeyList As String = "CacheKeyList"
    End Class
End Namespace

