Imports AMC.DNN.Modules.CertRecert.Business.Controller
Imports DotNetNuke.Services.Log.EventLog
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Business.Entities
Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports Personify.ApplicationManager

Namespace Controls.FormConfigurations
    Public Class ValidationRuleSettingsUC
        Inherits PersonifyDNNBaseForm

#Region "Properties"
        Public AMCCertRecertController As AmcCertRecertController
        Public ReadOnly ExceptionLogController As ExceptionLogController = New ExceptionLogController()
        Public Property ShowErrorMessage As Action(Of String)
        Public Property ParentModulePath() As String

        Public ReadOnly Property ValidationRules As List(Of ValidationRuleSetting)
            Get
                Dim results As New List(Of ValidationRuleSetting)
                If rptValidationRules.Items IsNot Nothing AndAlso rptValidationRules.Items.Count > 0 Then
                    For Each repeaterItem As RepeaterItem In rptValidationRules.Items
                        If repeaterItem.ItemType = ListItemType.Item OrElse repeaterItem.ItemType = ListItemType.AlternatingItem Then
                            Dim validationRule = New ValidationRuleSetting
                            validationRule.Id = CType(repeaterItem.FindControl("hdValidationRuleId"), HiddenField).Value
                            validationRule.Description = CType(repeaterItem.FindControl("lblValidationRuleDescription"), Label).Text
                            validationRule.IsEnabled = CType(repeaterItem.FindControl("chkValidationRuleIsEnabled"), CheckBox).Checked
                            results.Add(validationRule)
                        End If
                    Next
                End If
                Return results
            End Get
        End Property

#End Region

#Region "Private Methods"
        Private Sub CheckUserRole()
            If Not (UserInfo.IsSuperUser OrElse UserInfo.IsInRole("Administrators") OrElse UserInfo.IsInRole("CERTADMIN") OrElse UserInfo.IsInRole("Host")) Then
                Response.Redirect(NavigateURL(), True)
                ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(ValidationRuleId.RECERTOPTIONRULE1.ToString(), Server.MapPath(ParentModulePath))
            End If
        End Sub

        Private Sub RegisterJavascript()
            Page.ClientScript.RegisterClientScriptInclude("configuration", String.Format("{0}/Documentation/scripts/validationrulesettings.js?v={1}", ParentModulePath, CommonConstants.CURRENT_VERSION))
        End Sub

        Private Sub InitializeComponents()

        End Sub

        Private Sub AttatchEventHandlers()
            AddHandler rptValidationRules.ItemCommand, AddressOf RptValidationRulesItemCommand

        End Sub

        Private Sub RptValidationRulesItemCommand(ByVal source As Object, ByVal e As RepeaterCommandEventArgs)
            If e.CommandName = "SaveValidationRules" Then
                ModuleConfigurationHelper.Instance.SaveSettings(Server.MapPath(ParentModulePath), CommonConstants.CONFIGURATION_VALIDATION_RULE_SETTINGS_FILE_PATH, ValidationRules)
            End If
        End Sub

        Private Sub LoadValidationRules()
            rptValidationRules.DataSource = ModuleConfigurationHelper.Instance.GetSettings(Of List(Of ValidationRuleSetting))(Server.MapPath(ParentModulePath), CommonConstants.CONFIGURATION_VALIDATION_RULE_SETTINGS_FILE_PATH)
            rptValidationRules.DataBind()
        End Sub
#End Region

#Region "Event Handlers"
        ''' <summary>
        ''' Handles the Load event of the Page control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            AMCCertRecertController = New AmcCertRecertController(OrganizationId, OrganizationUnitId, 0, Server.MapPath(ParentModulePath), MasterCustomerId, SubCustomerId)
            Try
                CheckUserRole()
                RegisterJavascript()
                InitializeComponents()
                AttatchEventHandlers()
                If Not Page.IsPostBack Then
                    LoadValidationRules()
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
#End Region

#Region "Public Methods"

#End Region

    End Class
End Namespace