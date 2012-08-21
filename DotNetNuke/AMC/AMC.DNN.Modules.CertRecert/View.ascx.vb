'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2005
' by Perpetual Motion Interactive Systems Inc. ( http://www.perpetualmotion.ca )
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
'
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Entities.Modules

Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Business.Entities
Imports System.Globalization

Imports Personify.ApplicationManager

Imports System.Web.Services

''' -----------------------------------------------------------------------------
''' <summary>
''' The ViewAmcCertRecertGui class displays the content
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' </history>
''' -----------------------------------------------------------------------------
Partial Class ViewAmcCertRecertGui
    Inherits PersonifyDNNBaseForm

#Region "property"
    Private Shared _cePointCollection As New Dictionary(Of String, String)
    Public Shared Property CEPointCollection() As Dictionary(Of String, String)
        Get
            Return _cePointCollection
        End Get
        Set(ByVal value As Dictionary(Of String, String))
            _cePointCollection = value
        End Set
    End Property

    Public ReadOnly Property FieldIsRequiredString() As String
        Get
            Return Localization.GetString("FieldIsRequired.Text", LocalResourceFile)
        End Get

    End Property
    Public ReadOnly Property InputValueNotCorrect() As String
        Get
            Return Localization.GetString("InputValueNotCorrect.Text", LocalResourceFile)
        End Get
    End Property

    Public Function GetResourceText(ByVal resourceKey As String) As String
        Return Localization.GetString(resourceKey, LocalResourceFile)
    End Function
#End Region

#Region "Event Handlers"

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As EventArgs) Handles MyBase.Init
        Dim docPath As String = Request.QueryString("DocPath")
        If (String.IsNullOrEmpty(docPath)) Then
            If AJAX.IsInstalled Then
                AJAX.RegisterScriptManager() 'RadTabStrip
            End If
            If ProcessQueryString() Then
                Return
            End If
            RegisterJavascript()
            RegisterCss()
            LoadModuleControl()
        End If
    End Sub

    ''' <summary>
    ''' Handles the Load event of the Page control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As EventArgs) Handles MyBase.Load
        Try
            Dim docPath As String = Request.QueryString("DocPath")

            If Not (String.IsNullOrEmpty(docPath)) Then
                docPath = HttpUtility.HtmlDecode(docPath)
                docPath = HttpUtility.UrlDecode(docPath)
                DeleteDocumemtByDocumentPath(Server.MapPath(docPath))
            End If
        Catch exc As Exception        'Module failed to load
            ProcessModuleLoadException(Me, exc)
        End Try
    End Sub

#End Region

#Region "Private Methods"

    ''' <summary>
    ''' Registers the javascript.
    ''' </summary>
    Private Sub RegisterJavascript()
        Page.ClientScript.RegisterClientScriptInclude("$", String.Format("{0}/Documentation/scripts/jquery-1.7.2.js?v={1}", ModulePath, CommonConstants.CURRENT_VERSION))
        Page.ClientScript.RegisterClientScriptInclude("dateformat", String.Format("{0}/Documentation/scripts/DateFormat.js?v={1}", ModulePath, CommonConstants.CURRENT_VERSION))
        Page.ClientScript.RegisterClientScriptInclude("amc1", String.Format("{0}/Documentation/scripts/common.js?v={1}", ModulePath, CommonConstants.CURRENT_VERSION))
        Page.ClientScript.RegisterClientScriptInclude("amc-table-popup", String.Format("{0}/Documentation/scripts/AMCTablePopup.js?v={1}", ModulePath, CommonConstants.CURRENT_VERSION))
        Page.ClientScript.RegisterClientScriptInclude("print", String.Format("{0}/Documentation/scripts/Print.js?v={1}", ModulePath, CommonConstants.CURRENT_VERSION))
        Page.ClientScript.RegisterArrayDeclaration("certModuleCurrentPageURL", String.Concat("'", Page.Request.Url.PathAndQuery, "'"))
        Page.ClientScript.RegisterClientScriptInclude("datejs", String.Format("{0}/Documentation/scripts/date.js?v={1}", ModulePath, CommonConstants.CURRENT_VERSION))
    End Sub

    '''<summary>
    '''Register css to page
    ''' </summary>
    Public Sub RegisterCss()
        Dim linkUrl As String = String.Format("{0}/Documentation/styles/Print.css?v={1}", ModulePath, CommonConstants.CURRENT_VERSION)
        Dim link As String = [String].Format("<link rel=""stylesheet"" type=""text/css"" href=""{0}"" media=""print"" />", linkUrl)
        Dim linkLiteral As LiteralControl = New LiteralControl()
        linkLiteral.Text = link
        Me.Page.Header.Controls.Add(linkLiteral)
    End Sub


    ''' <summary>
    ''' Processes the query string.
    ''' If the Jquery Ajax call, then return true and write result & exit
    ''' If there is not Jquery Ajax call, then continue with the page life cycle
    ''' </summary>
    ''' <returns></returns>
    Public Function ProcessQueryString() As Boolean
        'TODO: ADD MORE FUNCTION HERE
        Return GetCurrentModuleSettingProcessPercent("GetPercents")
        'EX: Return GetCurrentModuleSettingProcessPercent(queryString) || GetCertification(queryString)... 
    End Function

    ''' <summary>
    ''' Gets the current module setting process percent.
    ''' </summary>
    ''' <param name="queryString">The query string.</param>
    ''' <returns></returns>
    Public Function GetCurrentModuleSettingProcessPercent(ByVal queryString As String) As Boolean
        If Not String.IsNullOrEmpty(Request.QueryString(queryString)) Then
            Response.Clear()
            Response.Write(GetModuleSettingsProcessPercent(OrganizationId, OrganizationUnitId, ModuleId))
            Response.Flush()
            Response.End()
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Loads the module control.
    ''' </summary>
    Private Sub LoadModuleControl()
        'Initialize child control
        Dim controlResourceFile = String.Empty
        Dim requestedControlPath = GetRequestedControlPath(controlResourceFile)
        InitializeChildControls(requestedControlPath, controlResourceFile)
    End Sub

    ''' <summary>
    ''' Initializes the child controls.
    ''' </summary>
    ''' <param name="requestedControlPath">The requested control path.</param>
    ''' <param name="controlResourceFile">The control resource file.</param>
    Private Sub InitializeChildControls(ByVal requestedControlPath As String, ByVal controlResourceFile As String)
        'Load the appropriate controls base on QueryString
        Dim neededLoadControl = CType(Page.LoadControl(requestedControlPath), PortalModuleBase)
        phAMCMain.Controls.Add(neededLoadControl)
        neededLoadControl.LocalResourceFile = Localization.GetResourceFile(Me, String.Format("{0}.ascx.resx", controlResourceFile))
        neededLoadControl.ModuleConfiguration = ModuleConfiguration
        BuildExtensionProperties(neededLoadControl)
    End Sub

    ''' <summary>
    ''' Builds the extension properties.
    ''' </summary>
    ''' <param name="neededLoadControl">The needed load control.</param>
    Private Sub BuildExtensionProperties(ByVal neededLoadControl As PortalModuleBase)
        Dim moduleType = neededLoadControl.GetType()
        If moduleType IsNot Nothing Then
            Dim modulePathProperty = moduleType.GetProperty("ParentModulePath")
            If modulePathProperty IsNot Nothing Then
                modulePathProperty.SetValue(neededLoadControl, ModulePath, Nothing)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Gets the requested control path.
    ''' </summary>
    ''' <param name="controlResourceFile">The control resource file.</param>
    ''' <returns></returns>
    Private Function GetRequestedControlPath(ByRef controlResourceFile As String) As String
        Dim queryString = Request.QueryString(CommonConstants.USER_CONTROL_PARAMETER)

        If Not IsAllowedPage(queryString) Then
            queryString = String.Empty 'default is home page
        End If

        Dim requestedControlPath As String

        If Not String.IsNullOrEmpty(queryString) Then
            requestedControlPath = String.Format("{0}/{1}/{2}.ascx", ModulePath, CommonConstants.USER_CONTROL_FOLDER_PATH, queryString)
            controlResourceFile = queryString.Split(CType("/", Char))(queryString.Split(CType("/", Char)).Length - 1)
        ElseIf IsPersonifyWebUserLoggedIn = True Then
            requestedControlPath = String.Format("{0}/{1}/{2}.ascx", ModulePath, CommonConstants.USER_CONTROL_FOLDER_PATH, CommonConstants.USER_AUTHENTICATION_CONTROL_DEFAULT)
            controlResourceFile = CommonConstants.USER_AUTHENTICATION_CONTROL_DEFAULT.Split(CType("/", Char))(CommonConstants.USER_AUTHENTICATION_CONTROL_DEFAULT.Split(CType("/", Char)).Length - 1)
        Else
            requestedControlPath = String.Format("{0}/{1}/{2}.ascx", ModulePath, CommonConstants.USER_CONTROL_FOLDER_PATH, CommonConstants.USER_CONTROL_DEFAULT_PATH)
            controlResourceFile = CommonConstants.USER_CONTROL_DEFAULT_PATH.Split(CType("/", Char))(CommonConstants.USER_CONTROL_DEFAULT_PATH.Split(CType("/", Char)).Length - 1)
        End If
        Return requestedControlPath
    End Function

    ''' <summary>
    ''' delete file on server by docpath value
    ''' </summary>
    ''' <param name="docPath"></param>
    ''' <remarks></remarks>
    Private Sub DeleteDocumemtByDocumentPath(ByVal docPath As String)
        Try
            If (IO.File.Exists(docPath)) Then
                IO.File.Delete(docPath)
                Response.Clear()
                Response.Write("1")
                Response.End()
            Else
                Response.Clear()
                Response.Write("0")
                Response.End()
            End If

        Catch ex As Exception
            'Delete fail. Set hasDeleted flag to false.
        End Try
    End Sub
    Private Function IsAllowedPage(ByVal pageName As String) As Boolean
        Dim decodedPageName = Server.UrlDecode(pageName)
        For Each s As String In CommonConstants.AllowPages
            If String.Compare(s, decodedPageName, True) = 0 Then
                Return True
            End If
        Next
        Return False
    End Function
#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Gets the current percent.
    ''' </summary>
    ''' <param name="orgId">The org id.</param>
    ''' <param name="orgUnitId">The org unit id.</param>
    ''' <param name="nmoduleId">The nmodule id.</param>
    ''' <returns></returns>
    <WebMethod()> _
    Public Shared Function GetModuleSettingsProcessPercent(ByVal orgId As String, ByVal orgUnitId As String, ByVal nmoduleId As Integer) As String
        Dim key = String.Format("{0}-{1}-{2}", orgId, orgUnitId, nmoduleId)
        If ThreadResults.Contains(key) Then
            Dim result = CType(ThreadResults.Get(key), String)
            If result = "100%" Then
                ThreadResults.Remove(key)
            End If
            Return result
        End If
        Return "100%"
    End Function
#End Region
End Class