Namespace Entities
    Public Class SectionInfo
        Public Property FormId() As String
        Public Property SectionId() As String
        Public Property SectionName() As String
        'Text display in Administrator Control Panel
        Public Property SectionValue() As String
        'Text display in Each form
        Public Property SectionProgramType() As String
        Public Property IsEnabled() As Boolean
        Public Property IsReadOnly() As Boolean
        Public Property Fields() As List(Of FieldInfo)
        Public Property Sequence() As Integer
        Public Property IsRichText() As Boolean
        Public Property IsConfigurable() As Boolean
        Public Const KEY = "SectionId"
    End Class
End Namespace