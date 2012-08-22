Imports DNN.MVP.Base.BasePresenter
Imports DNN.MVP.Sample.ModuleName1.IViews

Namespace Presenters
    Public Class DependencyUC1Presenter
        Inherits BasePresenter(Of IDependencyUC1View, Object)

        Public Sub New(view As IDependencyUC1View)
            MyBase.New(view)
        End Sub

        Protected Overrides Sub ViewAction()
        End Sub

        Protected Overrides Sub UpdateAction()
        End Sub

        Protected Overrides Sub DeleteAction()
        End Sub

        Protected Overrides Sub InsertAction()
        End Sub

        Protected Overrides Sub InitializeAction()
        End Sub

        Protected Overrides Sub SearchAction()
        End Sub

        Public Overrides Sub Validate()
        End Sub
    End Class
End Namespace