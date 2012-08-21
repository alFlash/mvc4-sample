Imports DNN.MVP.Base.BaseView
Imports DNN.MVP.Base.Common
Imports DNN.MVP.Base.BaseUserControl
Imports DNN.MVP.Sample.ModuleName1.IViews
Imports DNN.MVP.Sample.ModuleName1.Presenters

Namespace UserControls
    Public Class DependencyUC1
        Inherits BaseUserControl(Of DependencyUC1Presenter)
        Implements IDependencyUC1View

        Public Overrides Sub ReLoad()
            MyBase.ReLoad()
            If (String.IsNullOrEmpty(lblContent.Text)) Then
                lblContent.Text = "Control Updated 1"
            Else
                'Convert.ToInt32(strArray(strArray.Length - 1)
                Dim strArray = lblContent.Text.Split(CType(" ", Char))
                Dim count = Convert.ToInt32(strArray(strArray.Length - 1))
                Dim result = String.Empty
                For i As Integer = 0 To strArray.Length - 2
                    result += (strArray(i) + " ")
                Next
                result += (count + 1).ToString()
                lblContent.Text = result
            End If
        End Sub

        Public Overrides Sub AttachEventHandler()
            AddHandler btnUpdate.Click, AddressOf BtnUpdateClick
        End Sub

        Private Sub BtnUpdateClick(ByVal sender As Object, ByVal e As EventArgs)
            CommitChanges()
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Presenter.DoAction()
        End Sub

        Public Property PageMode() As PageMode Implements IBaseView.PageMode

        Public Property PageStatus() As PageStatus Implements IBaseView.PageStatus

        Public Property ErrorMessages() As List(Of String) Implements IBaseView.ErrorMessages

        Public Sub AddErrorMessage(ByVal errorMessage As String) Implements IBaseView.AddErrorMessage
            'Throw New NotImplementedException()
        End Sub

        Public Sub ShowErrorMessage() Implements IBaseView.ShowErrorMessage
            'Throw New NotImplementedException()
        End Sub
    End Class
End Namespace