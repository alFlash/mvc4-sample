

Namespace Entities
    Public NotInheritable Class ThreadResults
        Private Shared ReadOnly Results As New Hashtable()

        ''' <summary>
        ''' Gets the specified item id.
        ''' </summary>
        ''' <param name="itemId">The item id.</param>
        ''' <returns></returns>
        Public Shared Function [Get] (ByVal itemId As String) As Object
            Return Results (itemId)
        End Function

        ''' <summary>
        ''' Adds the specified item id.
        ''' </summary>
        ''' <param name="itemId">The item id.</param>
        ''' <param name="result">The result.</param>
        Public Shared Sub Add (ByVal itemId As String, ByVal result As Object)
            Results (itemId) = result
        End Sub

        ''' <summary>
        ''' Removes the specified item id.
        ''' </summary>
        ''' <param name="itemId">The item id.</param>
        Public Shared Sub Remove (ByVal itemId As String)
            Results.Remove (itemId)
        End Sub

        ''' <summary>
        ''' Determines whether [contains] [the specified item id].
        ''' </summary>
        ''' <param name="itemId">The item id.</param>
        ''' <returns>
        '''   <c>true</c> if [contains] [the specified item id]; otherwise, <c>false</c>.
        ''' </returns>
        Public Shared Function Contains (ByVal itemId As String) As Boolean
            Return Results.Contains (itemId)
        End Function
    End Class
End Namespace