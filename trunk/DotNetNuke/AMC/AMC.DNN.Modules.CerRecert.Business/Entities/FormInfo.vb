Namespace Entities
    Public Class FormInfo
        ''' <summary>
        ''' Gets or sets the form id.
        ''' </summary>
        ''' <value>
        ''' The form id.
        ''' </value>
        Public Property FormId() As String

        ''' <summary>
        ''' Gets or sets the name of the form.
        ''' </summary>
        ''' <value>
        ''' The name of the form.
        ''' </value>
        Public Property FormName() As String

        ''' <summary>
        ''' Gets or sets the form value.
        ''' </summary>
        ''' <value>
        ''' The form value.
        ''' </value>
        Public Property FormValue() As String

        ''' <summary>
        ''' Gets or sets the sections.
        ''' </summary>
        ''' <value>
        ''' The sections.
        ''' </value>
        Public Property Sections() As List(Of SectionInfo)

        ''' <summary>
        ''' Gets or sets a value indicating whether this instance is visible.
        ''' </summary>
        ''' <value>
        ''' 
        ''' <c>true</c> if this instance is visible; otherwise, <c>false</c>.
        ''' 
        ''' </value>
        Public Property IsVisible() As Boolean

        Public Property Sequence() As Integer
        Public Property IsRichText() As Boolean
        Public Property IsConfigurable() As Boolean
        Public Const KEY = "FormId"
    End Class
End Namespace