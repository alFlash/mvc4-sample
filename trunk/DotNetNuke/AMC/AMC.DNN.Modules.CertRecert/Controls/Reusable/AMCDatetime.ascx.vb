Namespace Controls.Reusable
    Public Class AMCDatetime
        Inherits Web.UI.UserControl

#Region "Property"
        Public Property CssClass() As String
            Get
                Return divAmcDateTimeContainer.Attributes("class")
            End Get
            Set(ByVal value As String)
                If Not String.IsNullOrEmpty(value) Then
                    divAmcDateTimeContainer.Attributes("class") = value
                End If
            End Set
        End Property

        Public Property ValidationGroup As String
            Get
                Return vldDateTime.ValidationGroup
            End Get
            Set(ByVal value As String)
                vldDateTime.ValidationGroup = value
            End Set
        End Property

        Public Property Text() As String
            Get
                Return txtDatetime.Text
            End Get
            Set(ByVal value As String)
                txtDatetime.Text = value
            End Set
        End Property

        Public Property Enabled() As Boolean
            Get
                Return txtDatetime.Enabled
            End Get
            Set(ByVal value As Boolean)
                txtDatetime.Enabled = value
                imgDate.Visible = value
            End Set
        End Property
#End Region

#Region "Event Handle"
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        End Sub
#End Region

    End Class
End Namespace