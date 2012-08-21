Namespace IControls
    Public Interface IReload
        ReadOnly Property SaveControls() As List(Of String)
        Sub Reload(ByVal saveControl As String)
    End Interface
End Namespace