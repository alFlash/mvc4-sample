Imports System.Xml.Serialization

Namespace Entities
    Public Class ProgramTypeSettings
        <XmlAttribute()> _
        Public Property ProgramTypeCode() As String
        <XmlAttribute()> _
        Public Property MinCEOpt2() As String
        <XmlAttribute()> _
        Public Property MaxCEOpt2() As String
        <XmlAttribute()> _
        Public Property MinCEOpt3() As String
        <XmlAttribute()> _
        Public Property MaxCEOpt3() As String
        Public Const KEY = "ProgramTypeCode"
    End Class
End Namespace