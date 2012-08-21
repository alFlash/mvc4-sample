Imports System.Xml.Serialization

Namespace Entities
    Public Class ValidationRuleSetting
        <XmlAttribute()> _
        Public Property Id() As String
        <XmlText()> _
        Public Property Description() As String
        <XmlAttribute()> _
        Public Property IsEnabled() As Boolean
        '<XmlAttribute()> _
        'Public Property IsConfigurable() As Boolean
        Public Const KEY = "Id"
    End Class
End Namespace