Imports System

Namespace Personify.ApplicationManager.PersonifyDataObjects
    <Serializable> _
    Public Class SiteSettings
        ' Methods
        Public Sub New(ByVal pPortalId As Integer)
            Me.PortalId = pPortalId
        End Sub


        ' Properties
        Public Property AdminEmailAddress As String
            Get
                Return Me._AdminEmailAddress
            End Get
            Set(ByVal value As String)
                Me._AdminEmailAddress = value
            End Set
        End Property

        Public Property AffilatePortalTabId As Integer
            Get
                Return Me._AffilatePortalTabId
            End Get
            Set(ByVal value As Integer)
                Me._AffilatePortalTabId = value
            End Set
        End Property

        Public Property ImageURL As String
            Get
                Return Me._ImageURL
            End Get
            Set(ByVal value As String)
                Me._ImageURL = value
            End Set
        End Property

        Public Property MyPortalTabId As Integer
            Get
                Return Me._MyPortalTabId
            End Get
            Set(ByVal value As Integer)
                Me._MyPortalTabId = value
            End Set
        End Property

        Public Property OrgId As String
            Get
                Return Me._OrgId
            End Get
            Set(ByVal value As String)
                Me._OrgId = value
            End Set
        End Property

        Public Property OrgUnitId As String
            Get
                Return Me._OrgUnitId
            End Get
            Set(ByVal value As String)
                Me._OrgUnitId = value
            End Set
        End Property

        Public Property PasswordRegularExpression As String
            Get
                Return Me._PasswordRegularExpression
            End Get
            Set(ByVal value As String)
                Me._PasswordRegularExpression = value
            End Set
        End Property

        Public Property PortalCurrency As String
            Get
                Return Me._PortalCurrency
            End Get
            Set(ByVal value As String)
                Me._PortalCurrency = value
            End Set
        End Property

        Public Property PortalId As Integer
            Get
                Return Me._PortalId
            End Get
            Set(ByVal value As Integer)
                Me._PortalId = value
            End Set
        End Property

        Public Property ProductImageURL As String
            Get
                Return Me._ProductImageURL
            End Get
            Set(ByVal value As String)
                Me._ProductImageURL = value
            End Set
        End Property

        Public Property UserData As String
            Get
                Return Me._UserData
            End Get
            Set(ByVal value As String)
                Me._UserData = value
            End Set
        End Property


        ' Fields
        Private _AdminEmailAddress As String
        Private _AffilatePortalTabId As Integer
        Private _ImageURL As String
        Private _MyPortalTabId As Integer
        Private _OrgId As String
        Private _OrgUnitId As String
        Private _PasswordRegularExpression As String
        Private _PortalCurrency As String
        Private _PortalId As Integer
        Private _ProductImageURL As String
        Private _UserData As String
    End Class
End Namespace

