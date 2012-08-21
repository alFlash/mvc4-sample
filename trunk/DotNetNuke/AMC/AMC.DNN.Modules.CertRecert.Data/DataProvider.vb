Imports TIMSS.API.Core

''' -----------------------------------------------------------------------------
''' <summary>
''' An abstract class for the data access layer
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' </history>
''' -----------------------------------------------------------------------------
    Public MustInherit Class DataProvider

#Region "Shared/Static Methods"


    ''' <summary>
    ''' Gets the object id.
    ''' </summary>
    ''' <param name="collections">The collections.</param>
    ''' <param name="propertyName">Name of the property.</param>
    ''' <returns></returns>
    Public Function GetObjectId (ByVal collections As IBusinessObjectCollection, ByVal propertyName As String) As Long
        Dim result As Long = 0
        For Each businessObject As IBusinessObject In collections
            Dim businessObjectType = businessObject.GetType()
            If businessObjectType IsNot Nothing Then
                Dim businessObjectProperty = businessObjectType.GetProperty (propertyName)
                If businessObjectProperty IsNot Nothing Then
                    Dim propertyValue = businessObjectProperty.GetValue (Nothing, Nothing)
                    If propertyValue IsNot Nothing And TypeOf propertyValue Is Long Then
                        Dim objectUniqueId = CType (propertyValue, Long)
                        If objectUniqueId >= result Then
                            result = objectUniqueId + 1
                        End If
                    End If
                End If
            End If
        Next
        Return result
    End Function

#End Region

#Region "Abstract methods"

    ''' <summary>
    ''' Synchronizes the object.
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="fromObj">From obj.</param>
    ''' <param name="toObj">To obj.</param>
    Public Shared Sub SynchronizeObject(Of T)(ByVal fromObj As T, ByRef toObj As T,
                                                ByVal ParamArray propertyNames() As String)
        For Each propertyName As String In propertyNames
            Try
                toObj.GetType().GetProperty(propertyName).SetValue(toObj,
                                                                     fromObj.GetType().GetProperty(propertyName).
                                                                        GetValue(fromObj, Nothing),
                                                                     Nothing)
            Catch ex As System.Exception
                Continue For
            End Try
        Next
    End Sub

   



#End Region
End Class