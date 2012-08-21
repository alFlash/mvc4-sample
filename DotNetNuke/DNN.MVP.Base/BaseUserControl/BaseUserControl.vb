Imports DotNetNuke.Entities.Modules.Actions
Imports DNN.MVP.Base.BasePresenter
Imports System.Web.UI
Imports DotNetNuke.Entities.Modules

Namespace BaseUserControl
    Public Class BaseUserControl(Of TPresenter As {Class, IBasePresenter})
        Inherits PortalModuleBase
        Implements IBaseUserControl, IActionable

#Region "Properties"
        Public Property Presenter() As TPresenter
        Public Overridable Property ParentControl As String
        Public Overridable Property RelatedControls As List(Of String)
#End Region

#Region "Public Methods"
        Public Overridable Sub AttachEventHandler()

        End Sub

        Public Function GetResource(ByVal className As String, ByVal resourceKey As String) As String
            Dim result = GetGlobalResourceObject(className, resourceKey)
            Return If(result IsNot Nothing, result.ToString(), String.Empty)
        End Function

        Public Function GetResource(ByVal resourceKey As String) As String
            Return GetResource(GetDefautResourceClassName(), resourceKey)
        End Function

        Public Overridable Function GetDefautResourceClassName() As String
            Return "Common"
        End Function
#End Region

#Region "IBaseUserControl"
        Public Overridable Sub ReLoad() Implements IBaseUserControl.ReLoad

        End Sub

        Public Sub CommitChanges() Implements IBaseUserControl.CommitChanges
            If (Not String.IsNullOrEmpty(ParentControl) AndAlso RelatedControls IsNot Nothing AndAlso RelatedControls.Count > 0) Then
                If (BaseControlCollection.Controls.ContainsKey(ParentControl)) Then
                    Dim parentCtr = BaseControlCollection.Controls(ParentControl)
                    If parentCtr IsNot Nothing Then
                        For Each controlKey As String In RelatedControls
                            parentCtr(controlKey).ReLoad()
                        Next
                    End If
                End If
            End If
        End Sub
#End Region

#Region "Event Handlers"
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Init
            Presenter = DirectCast(Activator.CreateInstance(GetType(TPresenter), Me), TPresenter)
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            Register(ParentControl, ID, Me)
            AttachEventHandler()
        End Sub
#End Region

#Region "IActionable"
        Public Overridable ReadOnly Property ModuleActions() As ModuleActionCollection Implements IActionable.ModuleActions
            Get
                Return Nothing
            End Get
        End Property
#End Region
        
    End Class
End Namespace

