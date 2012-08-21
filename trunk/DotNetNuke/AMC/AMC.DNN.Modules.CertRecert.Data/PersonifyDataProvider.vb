Imports System.Web
Imports AMC.DNN.Modules.CertRecert.Data.Entities
Imports AMC.DNN.Modules.CertRecert.Data.Enums
Imports Personify.ApplicationManager
Imports AMC.DNN.Modules.CertRecert.Data.Exception
Imports TIMSS.API.User.UserDefinedInfo

Public Class PersonifyDataProvider
    Inherits DataProvider


#Region "Private Member"

    Private _organizationId As String
    Private _organizationUnitId As String
    Private _certificationId As Integer
    Private _modulePath As String
    Private _masterCustomerId As String
    Private _subCustomerId As Integer
#End Region

#Region "Constructors"
    Public Sub New(ByVal organizationId As String, ByVal organizationUnitId As String, ByVal certificationId As Integer, ByVal modulePath As String, ByVal masterCustomerId As String, ByVal subCustomerId As Integer)
        _organizationId = organizationId
        _organizationUnitId = organizationUnitId
        _certificationId = certificationId
        _modulePath = modulePath
        _masterCustomerId = masterCustomerId
        _subCustomerId = subCustomerId
    End Sub
#End Region

#Region "Cache"

    ''cache interval: sliding caching specific the data will be cache in hours
    Private Const CacheInterval As Integer = 3

    ''' <summary>
    ''' Creates the cache key.
    ''' </summary>
    ''' <param name="organizationId">The organization id.</param>
    ''' <param name="organizationUnitId">The organization unit id.</param>
    ''' <param name="type">The type.</param>
    ''' <param name="masterCustomerId">The master customer id.</param>
    ''' <param name="subCustomerId">The sub customer id.</param>
    ''' <returns></returns>
    Private Shared Function CreateCacheKey(ByVal organizationId As String,
                                           ByVal organizationUnitId As String,
                                           ByVal type As String,
                                           ByVal certificationIdString As String,
                                           ByVal masterCustomerId As String,
                                           ByVal subCustomerId As Integer) As String
        Dim cacheKeyResult As String
        cacheKeyResult = String.Format("AMC_{0}_{1}_{2}_{3}_{4}_{5}",
                                       organizationId,
                                       organizationUnitId,
                                       type,
                                       masterCustomerId,
                                       subCustomerId,
                                       certificationIdString)
        Return cacheKeyResult
    End Function

    ''' <summary>
    ''' Gets the object from cache.
    ''' </summary>
    ''' <param name="key">The key.</param>
    ''' <returns></returns>

    Private Shared Function GetObjectFromCache(ByVal key As String) As Object
        Dim objectResult As Object = Nothing
        Try
            objectResult = PersonifyDataCache.Fetch(key)
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return objectResult
    End Function

    ''' <summary>
    ''' Gets the object from cache.
    ''' </summary>
    ''' <param name="organizationId">The organization id.</param>
    ''' <param name="organizationUnitId">The organization unit id.</param>
    ''' <param name="type">The type.</param>
    ''' <param name="masterCustomerId">The master customer id.</param>
    ''' <param name="subCustomerId">The sub customer id.</param>
    ''' <returns></returns>
    Private Shared Function GetObjectFromCache(ByVal organizationId As String,
                                               ByVal organizationUnitId As String,
                                               ByVal type As String,
                                               ByVal certificationIdString As String,
                                               ByVal masterCustomerId As String,
                                               ByVal subCustomerId As Integer) As Object
        Dim cacheObjectResult As Object = Nothing
        Try
            Dim cacheKey = CreateCacheKey(organizationId,
                                      organizationUnitId,
                                      type,
                                      certificationIdString,
                                      masterCustomerId,
                                      subCustomerId)

            cacheObjectResult = PersonifyDataCache.Fetch(cacheKey)
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return cacheObjectResult
    End Function

    ''' <summary>
    ''' Stores the cache object.
    ''' </summary>
    ''' <param name="key">The key.</param>
    ''' <param name="cacheObject">The cache object.</param>
    Private Shared Sub StoreCacheObject(ByVal key As String,
                                        ByVal cacheObject As Object)
        Try
            PersonifyDataCache.Store(key, cacheObject, Nothing, Caching.Cache.NoAbsoluteExpiration, New TimeSpan(CacheInterval, 0, 0))
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try

    End Sub

    ''' <summary>
    ''' Stores the cache object.
    ''' </summary>
    ''' <param name="organizationId">The organization id.</param>
    ''' <param name="organizationUnitId">The organization unit id.</param>
    ''' <param name="type">The type.</param>
    ''' <param name="masterCustomerId">The master customer id.</param>
    ''' <param name="subCustomerId">The sub customer id.</param>
    ''' <param name="cacheObject">The cache object.</param>
    Private Shared Sub StoreCacheObject(ByVal organizationId As String,
                                        ByVal organizationUnitId As String,
                                        ByVal type As String,
                                        ByVal certificationIdString As String,
                                        ByVal masterCustomerId As String,
                                        ByVal subCustomerId As Integer,
                                        ByVal cacheObject As Object)
        Try
            Dim cacheKey = CreateCacheKey(organizationId,
                                      organizationUnitId,
                                      type,
                                      certificationIdString,
                                      masterCustomerId,
                                      subCustomerId)
            PersonifyDataCache.Store(cacheKey, cacheObject, Nothing, Caching.Cache.NoAbsoluteExpiration, New TimeSpan(CacheInterval, 0, 0))
       Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Removes the cache object.
    ''' </summary>
    ''' <param name="organizationId">The organization id.</param>
    ''' <param name="organizationUnitId">The organization unit id.</param>
    ''' <param name="type">The type.</param>
    ''' <param name="masterCustomerId">The master customer id.</param>
    ''' <param name="subCustomerId">The sub customer id.</param>
    Private Shared Sub RemoveCacheObject(ByVal organizationId As String,
                                         ByVal organizationUnitId As String,
                                         ByVal type As String,
                                         ByVal certificationIdString As String,
                                         ByVal masterCustomerId As String,
                                         ByVal subCustomerId As Integer)
        Try
            Dim cacheKey = CreateCacheKey(organizationId,
                                      organizationUnitId,
                                      type,
                                      certificationIdString,
                                      masterCustomerId,
                                      subCustomerId)
            PersonifyDataCache.Remove(cacheKey)
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
    End Sub

#End Region

#Region "Inactive Status"

    ''' <summary>
    ''' Saves the in active status info.
    ''' </summary>
    ''' <param name="currentHttpContext">The current HTTP context.</param>
    ''' <param name="inActiveStatusInfo">The in active status info.</param>
    Public Shared Sub SaveInActiveStatusInfo(ByVal currentHttpContext As HttpContext,
                                              ByVal inActiveStatusInfo As InActiveStatus)
        If currentHttpContext IsNot Nothing Then
            If currentHttpContext.Session("InActiveStatusList") IsNot Nothing Then
                Dim inActiveStatusInfoList As List(Of InActiveStatus) =
                        CType(currentHttpContext.Session("InActiveStatusList"), List(Of InActiveStatus))
                inActiveStatusInfoList.Add(inActiveStatusInfo)
                currentHttpContext.Session("InActiveStatusList") = inActiveStatusInfoList
            Else
                Dim inActiveStatusInfoList As List(Of InActiveStatus) = New List(Of InActiveStatus)()
                inActiveStatusInfoList.Add(inActiveStatusInfo)
                currentHttpContext.Session("InActiveStatusList") = inActiveStatusInfoList
            End If
        End If
    End Sub

#End Region

#Region "Check data is available"
    Public Function CheckDataIsAvailable(ByVal masterCustomer As String) As Boolean
        Dim result As Boolean = True
        Try
            Dim surveys = GetSurveys(_organizationId, _organizationUnitId, masterCustomer)
            If surveys Is Nothing OrElse surveys.Count < 1 Then
                Return False
            End If
            For Each surveyTitle As String In DataAccessConstants.AmcSurveys
                If surveys.FindObject("Title", surveyTitle) Is Nothing Then
                    result = False
                    Exit For
                End If
            Next
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return result
    End Function
#End Region

End Class
