Imports DNN.MVP.Base.Common

Namespace BaseView
    Public Interface IBaseView
        Property PageMode As PageMode
        Property PageStatus As PageStatus
        Property ErrorMessages As List(Of String)
        Sub AddErrorMessage(ByVal errorMessage As String)
        Sub ShowErrorMessage()
    End Interface
End Namespace