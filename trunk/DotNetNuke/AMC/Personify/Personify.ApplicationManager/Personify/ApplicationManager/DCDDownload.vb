Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Security.Principal
Imports System.Web
Imports TIMSS.Server.BusinessMessages.FileUploadDownload
Imports TIMSS.ThirdPartyInterfaces

Namespace Personify.ApplicationManager
    Public Class DCDDownload
        ' Methods
        <DllImport("Kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
        Public Shared Function CloseHandle(ByVal handle As IntPtr) As Long
        End Function

        Public Function DownloadFile(ByRef errorMessage As String) As Boolean
            Dim flag As Boolean
            If Me.impersonateValidUser(Me.UserName, Me.Domain, Me.Password) Then
                Dim buffer As Byte() = New Byte(&H2711  - 1) {}
                Try 
                    Dim stream As Stream = New MemoryStream(FileOperation.DownLoadFile(UploadResourceType.DCDFiles, Me.FileName, Conversions.ToString(Me.ECDProductId)))
                    Dim current As HttpContext = HttpContext.Current
                    Me._UserIPAddress = current.Request.UserHostAddress
                    current.Response.Clear
                    current.Response.ClearHeaders
                    current.Response.ClearContent
                    current.Response.Buffer = True
                    current.Response.ContentType = "application/octet-stream"
                    current.Response.AddHeader("Content-Disposition", ("attachment; filename=" & Me.FileName))
                    Dim length As Long = stream.Length
                    Do While (length > 0)
                        If current.Response.IsClientConnected Then
                            Dim count As Integer = stream.Read(buffer, 0, &H2710)
                            current.Response.OutputStream.Write(buffer, 0, count)
                            current.Response.Flush
                            buffer = New Byte(&H2711  - 1) {}
                            length = (length - count)
                        Else
                            length = -1
                        End If
                    Loop
                    If (length <> -1) Then
                        current.Response.Flush
                        Return True
                    End If
                    flag = False
                Catch exception1 As Exception
                    ProjectData.SetProjectError(exception1)
                    Dim exception As Exception = exception1
                    errorMessage = "File may be expired or does not exist - please contact customer service"
                    flag = False
                    ProjectData.ClearProjectError
                End Try
            End If
            Return flag
        End Function

        <DllImport("advapi32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
        Public Shared Function DuplicateToken(ByVal ExistingTokenHandle As IntPtr, ByVal ImpersonationLevel As Integer, ByRef DuplicateTokenHandle As IntPtr) As Integer
        End Function

        Private Function impersonateValidUser(ByVal userName As String, ByVal domain As String, ByVal password As String) As Boolean
            If Me.ImpersonateUser Then
                Dim zero As IntPtr = IntPtr.Zero
                Dim duplicateTokenHandle As IntPtr = IntPtr.Zero
                Dim flag As Boolean = False
                If (((DCDDownload.RevertToSelf > 0) AndAlso (DCDDownload.LogonUserA(userName, domain, password, Me.LOGON32_LOGON_INTERACTIVE, Me.LOGON32_PROVIDER_DEFAULT, zero) <> 0)) AndAlso (DCDDownload.DuplicateToken(zero, 2, duplicateTokenHandle) <> 0)) Then
                    Me.impersonationContext = New WindowsIdentity(duplicateTokenHandle).Impersonate
                    If (Not Me.impersonationContext Is Nothing) Then
                        flag = True
                    End If
                End If
                If Not duplicateTokenHandle.Equals(IntPtr.Zero) Then
                    DCDDownload.CloseHandle(duplicateTokenHandle)
                End If
                If Not zero.Equals(IntPtr.Zero) Then
                    DCDDownload.CloseHandle(zero)
                End If
                Return flag
            End If
            Return True
        End Function

        <DllImport("advapi32.dll", CharSet:=CharSet.Ansi, SetLastError:=True, ExactSpelling:=True)> _
        Public Shared Function LogonUserA(<MarshalAs(UnmanagedType.VBByRefStr)> ByRef lpxzUsername As String, <MarshalAs(UnmanagedType.VBByRefStr)> ByRef lpszDomain As String, <MarshalAs(UnmanagedType.VBByRefStr)> ByRef lpszpassword As String, ByVal dwLogonType As Integer, ByVal dwLogonProvider As Integer, ByRef phToken As IntPtr) As Integer
        End Function

        <DllImport("advapi32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
        Public Shared Function RevertToSelf() As Long
        End Function

        Private Sub undoImpersonation()
            If Me.ImpersonateUser Then
                Me.impersonationContext.Undo
            End If
        End Sub


        ' Properties
        Public Property Domain As String
            Get
                Return Me._Domain
            End Get
            Set(ByVal Value As String)
                Me._Domain = Value
            End Set
        End Property

        Public Property ECDProductId As Long
            Get
                Return Me._ECDProductId
            End Get
            Set(ByVal Value As Long)
                Me._ECDProductId = Value
            End Set
        End Property

        Public Property FileId As Integer
            Get
                Return Me._FileId
            End Get
            Set(ByVal Value As Integer)
                Me._FileId = Value
            End Set
        End Property

        Public Property FileName As String
            Get
                Return Me._FileName
            End Get
            Set(ByVal Value As String)
                Me._FileName = Value
            End Set
        End Property

        Public ReadOnly Property FileSize As Decimal
            Get
                Return Me._FileSize
            End Get
        End Property

        Public Property ImpersonateUser As Boolean
            Get
                Return Me._ImpersonateUser
            End Get
            Set(ByVal Value As Boolean)
                Me._ImpersonateUser = Value
            End Set
        End Property

        Public Property Password As String
            Get
                Return Me._Password
            End Get
            Set(ByVal Value As String)
                Me._Password = Value
            End Set
        End Property

        Public Property Path As String
            Get
                Return Me._Path
            End Get
            Set(ByVal Value As String)
                Me._Path = Value
            End Set
        End Property

        Public ReadOnly Property UserIPAddress As String
            Get
                Return Me._UserIPAddress
            End Get
        End Property

        Public Property UserName As String
            Get
                Return Me._UserName
            End Get
            Set(ByVal Value As String)
                Me._UserName = Value
            End Set
        End Property


        ' Fields
        Private _Domain As String
        Private _ECDProductId As Long
        Private _FileId As Integer
        Private _FileName As String
        Private _FileSize As Decimal
        Private _ImpersonateUser As Boolean
        Private _Password As String
        Private _Path As String
        Private _UserIPAddress As String
        Private _UserName As String
        Private impersonationContext As WindowsImpersonationContext
        Private LOGON32_LOGON_INTERACTIVE As Integer = 2
        Private LOGON32_PROVIDER_DEFAULT As Integer = 0
    End Class
End Namespace

