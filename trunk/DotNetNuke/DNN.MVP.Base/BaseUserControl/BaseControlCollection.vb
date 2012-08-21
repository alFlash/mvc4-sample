Imports System.Web

Namespace BaseUserControl
    Public Module BaseControlCollection
        Public ReadOnly Property Controls As Dictionary(Of String, Dictionary(Of String, IBaseUserControl))
            Get
                If (HttpContext.Current.Session("SS_BaseControlCollection_Key") Is Nothing) Then
                    HttpContext.Current.Session("SS_BaseControlCollection_Key") = New Dictionary(Of String, Dictionary(Of String, IBaseUserControl))()
                End If
                Return CType(HttpContext.Current.Session("SS_BaseControlCollection_Key"), Dictionary(Of String, Dictionary(Of String, IBaseUserControl)))
            End Get
        End Property

        Public Sub Register(ByVal parentKey As String, ByVal key As String, ByVal control As IBaseUserControl)
            If Not String.IsNullOrEmpty(parentKey) Then
                SyncLock (Controls)
                    If Controls.ContainsKey(parentKey) Then
                        If (Controls(parentKey) Is Nothing) Then
                            Controls(parentKey) = New Dictionary(Of String, IBaseUserControl)
                        End If
                        If (Controls(parentKey).ContainsKey(key)) Then
                            Controls(parentKey)(key) = control
                        Else
                            Controls(parentKey).Add(key, control)
                        End If
                    Else
                        Controls.Add(parentKey, New Dictionary(Of String, IBaseUserControl)())
                        Controls(parentKey).Add(key, control)
                    End If
                End SyncLock
            End If
        End Sub
    End Module
End Namespace