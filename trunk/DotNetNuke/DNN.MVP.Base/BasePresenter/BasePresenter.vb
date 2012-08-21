Imports DNN.MVP.Base.Common
Imports DNN.MVP.Base.BaseView

Namespace BasePresenter
    Public MustInherit Class BasePresenter(Of TView As IBaseView, TRepository As Class)
        Implements IBasePresenter

#Region "View"
        Protected View As TView
#End Region

#Region "Repository"
        Protected Overridable ReadOnly Property Repository() As TRepository
            Get
                Return Nothing
            End Get
        End Property
#End Region

#Region "Constructors"
        Protected Sub New(ByVal tView As TView)
            View = tView
        End Sub
#End Region

#Region "Actions"
        Public MustOverride Sub ViewAction()
        Public MustOverride Sub UpdateAction()
        Public MustOverride Sub DeleteAction()
        Public MustOverride Sub InsertAction()
        Public MustOverride Sub InitializeAction()
        Public MustOverride Sub SearchAction()

        Public Overridable Sub DoAction() Implements IBasePresenter.DoAction
            Select Case View.PageMode
                Case PageMode.NONE
                    InitializeAction()
                    Exit Select
                Case PageMode.VIEW
                    ViewAction()
                    Exit Select
                Case PageMode.INSERT
                    InsertAction()
                    Exit Select
                Case PageMode.UPDATE
                    UpdateAction()
                    Exit Select
                Case PageMode.DELETE
                    DeleteAction()
                    Exit Select
                Case PageMode.SEARCH
                    SearchAction()
                    Exit Select
            End Select
        End Sub
#End Region

#Region "Actions"
        Public MustOverride Sub Validate() Implements IBasePresenter.Validate
#End Region

    End Class
End Namespace

