Imports System.Xml.Serialization

Namespace Entities
    Public Class AMCDescription
        <XmlAttribute()> _
        Public Property ID() As String
        <XmlText()> _
        Public Property Text() As String
    End Class
End Namespace