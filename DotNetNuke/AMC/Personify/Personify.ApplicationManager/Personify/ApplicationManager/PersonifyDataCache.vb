Imports DotNetNuke.UI.Utilities
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Runtime.CompilerServices
Imports System.Web.Caching

Namespace Personify.ApplicationManager
    <Serializable> _
    Public Class PersonifyDataCache
        ' Methods
        Shared Sub New()
            If (ConfigurationManager.AppSettings.Item("ProductCatalogCacheExpiration") Is Nothing) Then
                PersonifyDataCache._CacheExpirationInterval = &H1C20
            Else
                PersonifyDataCache._CacheExpirationInterval = Conversions.ToInteger(ConfigurationManager.AppSettings.Item("CacheExpirationInterval"))
            End If
        End Sub

        Public Shared Sub ClearCache()
            Dim list As ArrayList = DirectCast(PersonifyDataCache.Fetch(PersonifyDataCache._CacheKeyList), ArrayList)
            If (Not list Is Nothing) Then
                Dim num2 As Integer = (list.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num2)
                    PersonifyDataCache.Remove(list.Item(i).ToString)
                    i += 1
                Loop
                PersonifyDataCache.Remove(PersonifyDataCache._CacheKeyList)
            End If
        End Sub

        Public Shared Function Fetch(ByVal Key As String) As Object
            Return DataCache.GetCache(Key)
        End Function

        Private Shared Sub MaintainCacheKeyList(ByVal Key As String)
            Dim objObject As ArrayList = DirectCast(PersonifyDataCache.Fetch(PersonifyDataCache._CacheKeyList), ArrayList)
            If (objObject Is Nothing) Then
                objObject = New ArrayList
            End If
            objObject.Add(Key)
            DataCache.SetCache(PersonifyDataCache._CacheKeyList, objObject)
        End Sub

        Public Shared Sub Remove(ByVal Key As String)
            DataCache.RemoveCache(Key)
        End Sub

        Public Shared Function Store(ByVal Key As String, ByVal CacheObject As Object) As Boolean
            Dim flag As Boolean
            DataCache.SetCache(Key, RuntimeHelpers.GetObjectValue(CacheObject))
            PersonifyDataCache.MaintainCacheKeyList(Key)
            Return flag
        End Function

        Public Shared Function Store(ByVal Key As String, ByVal CacheObject As Object, ByVal AbsoluteExpiration As DateTime) As Boolean
            Dim flag As Boolean
            DataCache.SetCache(Key, RuntimeHelpers.GetObjectValue(CacheObject), AbsoluteExpiration)
            PersonifyDataCache.MaintainCacheKeyList(Key)
            Return flag
        End Function

        Public Shared Function Store(ByVal Key As String, ByVal CacheObject As Object, ByVal SlidingExpiration As Integer) As Boolean
            Dim flag As Boolean
            DataCache.SetCache(Key, RuntimeHelpers.GetObjectValue(CacheObject), SlidingExpiration)
            PersonifyDataCache.MaintainCacheKeyList(Key)
            Return flag
        End Function

        Public Shared Function Store(ByVal Key As String, ByVal CacheObject As Object, ByVal ObjDependancy As CacheDependency) As Boolean
            Dim flag As Boolean
            DataCache.SetCache(Key, RuntimeHelpers.GetObjectValue(CacheObject), ObjDependancy)
            PersonifyDataCache.MaintainCacheKeyList(Key)
            Return flag
        End Function

        Public Shared Function Store(ByVal Key As String, ByVal CacheObject As Object, ByVal ObjDependancy As Object, ByVal AbsoluteExpiration As DateTime, ByVal SlidingExpiration As TimeSpan) As Boolean
            Dim flag As Boolean
            DataCache.SetCache(Key, RuntimeHelpers.GetObjectValue(CacheObject), DirectCast(ObjDependancy, CacheDependency), AbsoluteExpiration, SlidingExpiration)
            PersonifyDataCache.MaintainCacheKeyList(Key)
            Return flag
        End Function


        ' Properties
        Public Shared ReadOnly Property CacheExpirationInterval As Integer
            Get
                Return PersonifyDataCache._CacheExpirationInterval
            End Get
        End Property


        ' Fields
        Private Shared _CacheExpirationInterval As Integer
        Private Shared _CacheKeyList As String = "CacheKeyList"
    End Class
End Namespace

