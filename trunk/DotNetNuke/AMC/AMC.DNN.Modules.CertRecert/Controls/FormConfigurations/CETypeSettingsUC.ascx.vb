Imports AMC.DNN.Modules.CertRecert.Business.Controller
Imports AMC.DNN.Modules.CertRecert.Business.BusinessValidation
Imports System.Web.Script.Serialization
Imports System.ComponentModel
Imports TIMSS.API.User.ApplicationInfo
Imports TIMSS.API.Core
Imports DotNetNuke.Services.Log.EventLog
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Business.Entities
Imports AMC.DNN.Modules.CertRecert.Business.Helpers

Imports System.Linq

Imports Personify.ApplicationManager

Imports TIMSS.API.Core.Validation
Imports TIMSS.API.User.UserDefinedInfo

Namespace Controls.FormConfigurations
    Public Class CETypeSettingsUC
        Inherits PersonifyDNNBaseForm

#Region "Private Members"
        Private _amcCertRecertController As AmcCertRecertController
#End Region

#Region "Public Properties"
        Public Property CEWeightSettings As UserDefinedCertificationCEWeights
        Private ReadOnly _exceptionLogController As ExceptionLogController = New ExceptionLogController()
        Public Property ShowErrorMessage As Action(Of String)
        Public Property CETypesList As List(Of CusCETypeCodeList)

        Public ReadOnly Property CETypesListJson As String
            Get
                CETypesList.Sort(Function(x, y) String.Compare(x.Description, y.Description))
                Return (New JavaScriptSerializer()).Serialize(CETypesList)
            End Get
        End Property
        Public Property ParentModulePath() As String

        Public Property XmlProgramTypeSettingsOpt2() As List(Of ProgramTypeSettings)

        Public ReadOnly Property ProgramTypeSettingOpt2() As List(Of ProgramTypeSettings)
            Get
                Dim results As New List(Of ProgramTypeSettings)
                If rptProgramTypesOption2.Items IsNot Nothing AndAlso rptProgramTypesOption2.Items.Count > 0 Then
                    For Each item As RepeaterItem In rptProgramTypesOption2.Items
                        If item.ItemType = ListItemType.Item OrElse item.ItemType = ListItemType.AlternatingItem Then
                            Dim programTypeCode = CType(item.FindControl("lblProgramTypeCode"), Label).Text
                            Dim txtMinCEOpt2 = CType(item.FindControl("txtMinCEOpt2"), TextBox).Text
                            Dim txtMaxCEOpt2 = CType(item.FindControl("txtMaxCEOpt2"), TextBox).Text
                            Dim txtMinCEOpt3 = CType(item.FindControl("txtMinCEOpt3"), TextBox).Text
                            Dim txtMaxCEOpt3 = CType(item.FindControl("txtMaxCEOpt3"), TextBox).Text
                            Dim programType As New ProgramTypeSettings
                            programType.ProgramTypeCode = programTypeCode
                            programType.MinCEOpt2 = txtMinCEOpt2
                            programType.MaxCEOpt2 = txtMaxCEOpt2
                            programType.MinCEOpt3 = txtMinCEOpt3
                            programType.MaxCEOpt3 = txtMaxCEOpt3
                            results.Add(programType)
                        End If
                    Next
                End If
                Return results
            End Get
        End Property
#End Region

#Region "Private Methods"

        Private Sub LoadCEWeightSettings()
            CEWeightSettings.Sort("ProgramType", ListSortDirection.Ascending)
            rptCEWeightSettings.DataSource = CEWeightSettings
            rptCEWeightSettings.DataBind()
        End Sub

        Private Sub LoadProgramTypeList()
            Dim newCEWeight = CEWeightSettings.CreateNew()
            rptProgramTypesOption2.DataSource = newCEWeight.ProgramType.List
            rptProgramTypesOption2.DataBind()
        End Sub

        Private Sub CheckUserRole()
            If Not (UserInfo.IsSuperUser OrElse UserInfo.IsInRole("Administrators") OrElse UserInfo.IsInRole("CERTADMIN") OrElse UserInfo.IsInRole("Host")) Then
                Response.Redirect(NavigateURL(), True)
            End If
        End Sub

        Private Sub RegisterJavascript()
            Page.ClientScript.RegisterClientScriptInclude("configuration", String.Format("{0}/Documentation/scripts/cetypesettings.js?v={1}", ParentModulePath, CommonConstants.CURRENT_VERSION))
        End Sub

        Private Sub InitializeComponents()
            lblCEWeightMessage.Text = String.Empty
            lblCEWeightPopupMessage.Text = String.Empty

            XmlProgramTypeSettingsOpt2 = ModuleConfigurationHelper.Instance.GetSettings(Of List(Of ProgramTypeSettings))(Server.MapPath(ParentModulePath), CommonConstants.PROGRAMTYPE_RECERT_OPTION2_SETTING_FILE_PATH)
        End Sub

        Private Sub AttatchEventHandlers()
            AddHandler btnSaveCEWeight.Click, AddressOf BtnSaveCEWeightClick
            AddHandler rptCEWeightSettings.ItemDataBound, AddressOf CEWeightSettingsItemDataBound
            AddHandler rptCEWeightSettings.ItemCommand, AddressOf CEWeightSettingsItemCommand

            AddHandler rptProgramTypesOption2.ItemDataBound, AddressOf ProgramTypesOptionItemDataBound
            AddHandler rptProgramTypesOption2.ItemCommand, AddressOf ProgramTypesOptionItemCommand
        End Sub

        ''' <summary>
        ''' Process unhanlde exception on page by logging error and showing a message
        ''' </summary>
        ''' <param name="unhandleException"></param>
        ''' <remarks></remarks>
        Private Sub ProcessException(ByVal unhandleException As System.Exception)
            _exceptionLogController.AddLog(unhandleException)
            If Not ShowErrorMessage Is Nothing Then
                ShowErrorMessage.Invoke(unhandleException.Message)
            End If
        End Sub
#End Region

#Region "Event handers"
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            _amcCertRecertController = New AmcCertRecertController(OrganizationId, OrganizationUnitId, 0, Server.MapPath(ParentModulePath), MasterCustomerId, SubCustomerId)
            CheckUserRole()
            RegisterJavascript()
            InitializeComponents()
            AttatchEventHandlers()
            CEWeightSettings = _amcCertRecertController.GetCertificationCEWeights(OrganizationId, OrganizationUnitId)
            CETypesList = New List(Of CusCETypeCodeList)
            Dim newCEWeight = CEWeightSettings.CreateNew()
            For Each codeEntry As ICodeEntry In newCEWeight.ProgramType.List
                If codeEntry.Code <> "CONTEDUCATION" AndAlso codeEntry.Code <> "EDUCATION" Then
                    Dim ceTypeList = _amcCertRecertController.GetCETypesByProgramType(codeEntry.Code)
                    Dim cusCETypeList As New List(Of CusCETypeCode)
                    For Each ceTypeCode As ApplicationCode In ceTypeList
                        Dim ceType = New CusCETypeCode
                        ceType.Code = ceTypeCode.Code
                        ceType.Description = ceTypeCode.Description
                        cusCETypeList.Add(ceType)
                    Next
                    If cusCETypeList IsNot Nothing AndAlso cusCETypeList.Count > 0 Then

                        Dim cusCEtypeCodeList = New CusCETypeCodeList
                        cusCEtypeCodeList.Code = codeEntry.Code
                        cusCEtypeCodeList.Description = codeEntry.Description
                        cusCEtypeCodeList.CETypes = cusCETypeList
                        CETypesList.Add(cusCEtypeCodeList)
                    End If
                End If
            Next

            If Not Page.IsPostBack Then
                LoadCEWeightSettings()
                LoadProgramTypeList()

                ddlProgramType.DataTextField = "Description"
                ddlProgramType.DataValueField = "Code"
                ddlProgramType.DataSource = CETypesList
                ddlProgramType.DataBind()

                Dim guidelines = ModuleConfigurationHelper.Instance.GetGuidelineSettings(Server.MapPath(ParentModulePath))
                If guidelines IsNot Nothing Then
                    For Each guideline As AMCDescription In guidelines
                        If guideline.ID = "ProgramTypeSettings" Then
                            lblProgramTypeSettingsGuideline.Text = guideline.Text
                        ElseIf guideline.ID = "CETypeSettings" Then
                            lblCETypeSettingsGuideline.Text = guideline.Text
                        End If
                    Next
                End If
            End If
        End Sub

        ''' <summary>
        ''' BTNs the save CE weight click.
        ''' </summary>
        ''' <param name="sender">The sender.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Private Sub BtnSaveCEWeightClick(ByVal sender As Object, ByVal e As EventArgs)
            Dim issues As IIssuesCollection = Nothing
            Try
                If Not String.IsNullOrEmpty(hdCurrentSelectedCETypeCode.Value) Then
                    If String.IsNullOrEmpty(hdCurrentObjectUniqueId.Value) Then 'Add CE Weight
                        Dim ceWeight = _amcCertRecertController.GetUserDefinedCertificationCEWeights(OrganizationId,
                                                                                                     OrganizationUnitId,
                                                                                                     hdCurrentSelectedCETypeCode.Value,
                                                                                                     hdCurrentSelectedProgramTypeCode.Value)
                        If ceWeight Is Nothing OrElse ceWeight.Count <= 0 Then
                            issues = _amcCertRecertController.AddCertificationCEWeightSetting(OrganizationId, OrganizationUnitId, hdCurrentSelectedCETypeCode.Value,
                                                             ddlProgramType.SelectedValue, txtCEWeight.Text, txtMinCE.Text, txtMaxCE.Text)
                        Else
                            DuplicateItemIssues.Assert(True, ceWeight(0), Localization.GetString("DuplicateItemError.Text", LocalResourceFile))
                            issues = ceWeight.ValidationIssues
                        End If

                    Else 'Edit
                        issues = _amcCertRecertController.UpdateCertificationCEWeightSetting(OrganizationId, OrganizationUnitId, hdCurrentSelectedCETypeCode.Value,
                                                                                 hdCurrentSelectedProgramTypeCode.Value, txtCEWeight.Text, txtMinCE.Text, txtMaxCE.Text, hdCurrentObjectUniqueId.Value)
                    End If
                End If

                If issues IsNot Nothing AndAlso issues.Count > 0 Then
                    For Each issue As IIssue In issues
                        lblCEWeightPopupMessage.Text += issue.Message + "<br/>"
                    Next
                    issues.RemoveAll()
                Else
                    hdCurrentSectionPopupOpenningId.Value = String.Empty
                    hdCurrentSectionPopupOpenningTitle.Value = String.Empty
                    hdCurrentObjectUniqueId.Value = String.Empty
                End If

                LoadCEWeightSettings()
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Private Sub CEWeightSettingsItemCommand(ByVal source As Object, ByVal e As RepeaterCommandEventArgs)
            If e.CommandName = "Delete" Then
                Try
                    Dim hdObjectUniqueId = CType(e.Item.FindControl("hdObjectUniqueId"), HiddenField)
                    If Not String.IsNullOrEmpty(hdObjectUniqueId.Value) Then
                        _amcCertRecertController.DeleteCEWeightSetting(OrganizationId, OrganizationUnitId, hdObjectUniqueId.Value)
                        LoadCEWeightSettings()
                    End If
                Catch ex As Exception
                    ProcessException(ex)
                End Try
            End If
        End Sub

        Private Sub CEWeightSettingsItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim dataItem = CType(e.Item.DataItem, UserDefinedCertificationCEWeight)
                Dim lblCETypeDesc = CType(e.Item.FindControl("lblCETypeDesc"), Label)
                Dim lblProgramTypeDesc = CType(e.Item.FindControl("lblProgramTypeDesc"), Label)
                Dim lblCETypeString = CType(e.Item.FindControl("lblCETypeString"), Label)
                Dim lblProgramTypeString = CType(e.Item.FindControl("lblProgramTypeString"), Label)
                Dim lblWeightText = CType(e.Item.FindControl("lblWeightText"), Label)
                lblWeightText.Text = dataItem.Weight.ToString("F")
                Dim lblMinCE = CType(e.Item.FindControl("lblMinCE"), Label)
                lblMinCE.Text = dataItem.MinCE.ToString("F")
                Dim lblMaxCE = CType(e.Item.FindControl("lblMaxCE"), Label)
                lblMaxCE.Text = dataItem.MaxCE.ToString("F")
                Dim newCEWeight = CEWeightSettings.CreateNew()
                lblCETypeDesc.Text = newCEWeight.CEType.List(lblCETypeString.Text).Description
                lblProgramTypeDesc.Text = newCEWeight.ProgramType.List(lblProgramTypeString.Text).Description
            End If
        End Sub

        Private Sub ProgramTypesOptionItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim programTypeCode = CType(e.Item.FindControl("lblProgramTypeCode"), Label).Text
                Dim programType = XmlProgramTypeSettingsOpt2.FirstOrDefault(Function(x) InlineAssignHelper(x.ProgramTypeCode, programTypeCode))
                If programType IsNot Nothing Then
                    CType(e.Item.FindControl("txtMinCEOpt2"), TextBox).Text = programType.MinCEOpt2
                    CType(e.Item.FindControl("txtMaxCEOpt2"), TextBox).Text = programType.MaxCEOpt2
                    CType(e.Item.FindControl("txtMinCEOpt3"), TextBox).Text = programType.MinCEOpt3
                    CType(e.Item.FindControl("txtMaxCEOpt3"), TextBox).Text = programType.MaxCEOpt3
                Else
                    CType(e.Item.FindControl("txtMinCEOpt2"), TextBox).Text = "0"
                    CType(e.Item.FindControl("txtMaxCEOpt2"), TextBox).Text = "0"
                    CType(e.Item.FindControl("txtMinCEOpt3"), TextBox).Text = "0"
                    CType(e.Item.FindControl("txtMaxCEOpt3"), TextBox).Text = "0"
                End If
            End If
        End Sub

        Private Function InlineAssignHelper(ByVal queryCode As String, ByVal returnCode As String) As Boolean
            If queryCode = returnCode Then
                Return True
            End If
            Return False
        End Function

        Private Sub ProgramTypesOptionItemCommand(ByVal source As Object, ByVal e As RepeaterCommandEventArgs)
            If e.CommandName = "SaveProgramTypeOpt2" Then
                ModuleConfigurationHelper.Instance.SaveSettings(Server.MapPath(ParentModulePath), CommonConstants.PROGRAMTYPE_RECERT_OPTION2_SETTING_FILE_PATH, ProgramTypeSettingOpt2)
            End If
        End Sub
#End Region

    End Class
End Namespace