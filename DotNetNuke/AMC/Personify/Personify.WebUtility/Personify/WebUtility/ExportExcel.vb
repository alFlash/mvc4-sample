Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Collections
Imports System.Data
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Threading
Imports System.Web
Imports System.Xml
Imports System.Xml.XPath
Imports System.Xml.Xsl

Namespace Personify.WebUtility
    Public Class ExportExcel
        ' Methods
        Private Sub CreateStylesheet(ByVal writer As XmlTextWriter, ByVal sHeaders As String(), ByVal sFileds As String(), ByVal FormatType As ExportType)
            Try 
                Dim ns As String = "http://www.w3.org/1999/XSL/Transform"
                writer.Formatting = Formatting.Indented
                writer.WriteStartDocument
                writer.WriteStartElement("xsl", "stylesheet", ns)
                writer.WriteAttributeString("version", "1.0")
                writer.WriteStartElement("xsl:output")
                writer.WriteAttributeString("method", "text")
                writer.WriteAttributeString("version", "4.0")
                writer.WriteEndElement
                writer.WriteStartElement("xsl:template")
                writer.WriteAttributeString("match", "/")
                Dim num3 As Integer = (sHeaders.Length - 1)
                Dim i As Integer = 0
                Do While (i <= num3)
                    writer.WriteString("""")
                    writer.WriteStartElement("xsl:value-of")
                    writer.WriteAttributeString("select", ("'" & sHeaders(i) & "'"))
                    writer.WriteEndElement
                    writer.WriteString("""")
                    If (i <> (sFileds.Length - 1)) Then
                        writer.WriteString(Conversions.ToString(Interaction.IIf((FormatType = ExportType.CSV), ",", ChrW(9))))
                    End If
                    i += 1
                Loop
                writer.WriteStartElement("xsl:for-each")
                writer.WriteAttributeString("select", "Export/Values")
                writer.WriteString(ChrW(13) & ChrW(10))
                Dim num4 As Integer = (sFileds.Length - 1)
                Dim j As Integer = 0
                Do While (j <= num4)
                    writer.WriteString("""")
                    writer.WriteStartElement("xsl:value-of")
                    writer.WriteAttributeString("select", sFileds(j))
                    writer.WriteEndElement
                    writer.WriteString("""")
                    If (j <> (sFileds.Length - 1)) Then
                        writer.WriteString(Conversions.ToString(Interaction.IIf((FormatType = ExportType.CSV), ",", ChrW(9))))
                    End If
                    j += 1
                Loop
                writer.WriteEndElement
                writer.WriteEndElement
                writer.WriteEndElement
                writer.WriteEndDocument
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                Throw exception
            End Try
        End Sub

        Public Sub ExportData(ByVal datasource As DataTable, ByVal FormatType As ExportType, ByVal FileName As String)
            Try 
                If (datasource.Rows.Count = 0) Then
                    Throw New Exception("There is no data to export.")
                End If
                Dim dsExport As New DataSet("Export")
                Dim table As DataTable = datasource.Copy
                table.TableName = "Values"
                dsExport.Tables.Add(table)
                Dim sHeaders As String() = New String(((table.Columns.Count - 1) + 1)  - 1) {}
                Dim sFileds As String() = New String(((table.Columns.Count - 1) + 1)  - 1) {}
                Dim num2 As Integer = (table.Columns.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num2)
                    sHeaders(i) = table.Columns.Item(i).ColumnName
                    sFileds(i) = Me.ReplaceSpclChars(table.Columns.Item(i).ColumnName)
                    i += 1
                Loop
                Me.ExportXSLT(dsExport, sHeaders, sFileds, FormatType, FileName)
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                Throw exception
            End Try
        End Sub

        Public Sub ExportData(ByVal datasource As DataTable, ByVal ColumnList As Integer(), ByVal FormatType As ExportType, ByVal FileName As String)
            Try 
                If (datasource.Rows.Count = 0) Then
                    Throw New Exception("There is no data to export")
                End If
                Dim dsExport As New DataSet("Export")
                Dim table As DataTable = datasource.Copy
                table.TableName = "Values"
                dsExport.Tables.Add(table)
                If (ColumnList.Length > table.Columns.Count) Then
                    Throw New Exception("ExportColumn List should not exceed Total Columns")
                End If
                Dim sHeaders As String() = New String(((ColumnList.Length - 1) + 1)  - 1) {}
                Dim sFileds As String() = New String(((ColumnList.Length - 1) + 1)  - 1) {}
                Dim num2 As Integer = (ColumnList.Length - 1)
                Dim i As Integer = 0
                Do While (i <= num2)
                    If ((ColumnList(i) < 0) OrElse (ColumnList(i) >= table.Columns.Count)) Then
                        Throw New Exception("ExportColumn Number should not exceed Total Columns Range")
                    End If
                    sHeaders(i) = table.Columns.Item(ColumnList(i)).ColumnName
                    sFileds(i) = Me.ReplaceSpclChars(table.Columns.Item(ColumnList(i)).ColumnName)
                    i += 1
                Loop
                Me.ExportXSLT(dsExport, sHeaders, sFileds, FormatType, FileName)
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                Throw exception
            End Try
        End Sub

        Public Sub ExportData(ByVal datasource As DataTable, ByVal ColumnList As Integer(), ByVal Headers As String(), ByVal FormatType As ExportType, ByVal FileName As String)
            Try 
                If (datasource.Rows.Count = 0) Then
                    Throw New Exception("There is no data to export")
                End If
                Dim dsExport As New DataSet("Export")
                Dim table As DataTable = datasource.Copy
                table.TableName = "Values"
                dsExport.Tables.Add(table)
                If (ColumnList.Length <> Headers.Length) Then
                    Throw New Exception("ExportColumn List and Headers List should be of same length")
                End If
                If ((ColumnList.Length > table.Columns.Count) OrElse (Headers.Length > table.Columns.Count)) Then
                    Throw New Exception("ExportColumn List should not exceed Total Columns")
                End If
                Dim sFileds As String() = New String(((ColumnList.Length - 1) + 1)  - 1) {}
                Dim num2 As Integer = (ColumnList.Length - 1)
                Dim i As Integer = 0
                Do While (i <= num2)
                    If ((ColumnList(i) < 0) OrElse (ColumnList(i) >= table.Columns.Count)) Then
                        Throw New Exception("ExportColumn Number should not exceed Total Columns Range")
                    End If
                    sFileds(i) = Me.ReplaceSpclChars(table.Columns.Item(ColumnList(i)).ColumnName)
                    i += 1
                Loop
                Me.ExportXSLT(dsExport, Headers, sFileds, FormatType, FileName)
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                Throw exception
            End Try
        End Sub

        Private Sub ExportXSLT(ByVal dsExport As DataSet, ByVal sHeaders As String(), ByVal sFileds As String(), ByVal FormatType As ExportType, ByVal FileName As String)
            Try 
                If (dsExport.Tables.Count > 0) Then
                    Dim enumerator As IEnumerator
                    Try 
                        enumerator = dsExport.Tables.Item(0).Rows.GetEnumerator
                        Do While enumerator.MoveNext
                            Dim current As DataRow = DirectCast(enumerator.Current, DataRow)
                            Dim num2 As Integer = (sFileds.Length - 1)
                            Dim i As Integer = 0
                            Do While (i <= num2)
                                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(current.Item(sFileds(i)))) Then
                                    current.Item(sFileds(i)) = Me.ReplaceInvalidChars(Conversions.ToString(current.Item(sFileds(i))))
                                End If
                                i += 1
                            Loop
                        Loop
                    Finally
                        If TypeOf enumerator Is IDisposable Then
                            TryCast(enumerator,IDisposable).Dispose
                        End If
                    End Try
                End If
                Me.response.Clear
                Me.response.Buffer = True
                If (FormatType = ExportType.CSV) Then
                    Me.response.ContentType = "text/csv"
                    Me.response.AppendHeader("content-disposition", ("attachment; filename=" & FileName))
                ElseIf (FormatType = ExportType.Excel) Then
                    Me.response.ContentType = "application/vnd.ms-excel"
                    Me.response.AppendHeader("content-disposition", ("attachment; filename=" & FileName))
                End If
                Dim w As New MemoryStream
                Dim writer2 As New XmlTextWriter(w, Encoding.UTF8)
                Me.CreateStylesheet(writer2, sHeaders, sFileds, FormatType)
                writer2.Flush
                w.Seek(0, SeekOrigin.Begin)
                Dim document As New XmlDataDocument(dsExport)
                Dim transform As New XslTransform
                transform.Load(New XmlTextReader(w), Nothing, Nothing)
                Dim writer As New StringWriter
                transform.Transform(DirectCast(document, IXPathNavigable), Nothing, DirectCast(writer, TextWriter), Nothing)
                Me.response.Write(writer.ToString)
                writer.Close
                writer2.Close
                w.Close
                Me.response.End
            Catch exception1 As ThreadAbortException
                ProjectData.SetProjectError(exception1)
                Dim exception As ThreadAbortException = exception1
                Dim message As String = exception.Message
                ProjectData.ClearProjectError
            Catch exception3 As Exception
                ProjectData.SetProjectError(exception3)
                Dim exception2 As Exception = exception3
                Throw exception2
            End Try
        End Sub

        Private Function ReplaceInvalidChars(ByVal fieldName As String) As String
            fieldName = fieldName.Replace("""", """""")
            Return fieldName
        End Function

        Private Function ReplaceSpclChars(ByVal fieldName As String) As String
            fieldName = fieldName.Replace(" ", "_x0020_")
            fieldName = fieldName.Replace("%", "_x0025_")
            fieldName = fieldName.Replace("#", "_x0023_")
            fieldName = fieldName.Replace("&", "_x0026_")
            fieldName = fieldName.Replace("/", "_x002F_")
            fieldName = fieldName.Replace("""", """""")
            Return fieldName
        End Function


        ' Fields
        Private response As HttpResponse = HttpContext.Current.Response

        ' Nested Types
        Public Enum ExportType
            ' Fields
            CSV = 2
            Excel = 1
        End Enum
    End Class
End Namespace

