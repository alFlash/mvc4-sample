Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Business.Controller
Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports AMC.DNN.Modules.CertRecert.Data.Enums
Imports AMC.DNN.Modules.CertRecert.Business.Entities
Imports System.Web.Script.Serialization
Imports TIMSS.API.Core

Imports TIMSS.API.Core.Validation
Imports TIMSS.API.User.UserDefinedInfo

Namespace Controls.Common
    Public Class ScopeUC
        Inherits SectionBaseUserControl

#Region "property Member"
        Public Property CPTCodeList As List(Of CusCPTCodeList)
        Public ReadOnly Property CPTCodeListJSon As String
            Get
                If CPTCodeList IsNot Nothing AndAlso CPTCodeList.Count > 0 Then
                    CPTCodeList.Sort(Function(x, y) String.Compare(x.Description, y.Description))
                    Return (New JavaScriptSerializer()).Serialize(CPTCodeList)
                End If
                Return Nothing
            End Get
        End Property
#End Region

#Region "Event Handlers"
        ''' <summary>
        ''' Handles the Load event of the Page control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            Try
                Page.ClientScript.RegisterClientScriptInclude("configuration", String.Format("{0}/Documentation/scripts/amc_scopeuc.js?v={1}", ParentModulePath, CommonConstants.CURRENT_VERSION))
                AddEventHandlers()
                CPTCodeList = BindCPTCodeType()
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Protected Sub btnSaveClick(ByVal sender As Object, ByVal e As EventArgs)
            Try
                SavePracticeScope()
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Public Overrides Function Save() As IIssuesCollection
            ''Commit data
            Try
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                Dim results = AMCCertRecertController.CommitPracticeScope(MasterCustomerId, SubCustomerId)
                Dim fieldQuestion = GetFieldInfo("QuestionList")
                If fieldQuestion IsNot Nothing AndAlso fieldQuestion.IsEnabled Then
                    If IsQuestionExisted(DataAccessConstants.CERTIFICATION_PROFESSIONAL_PRACTICE_SCOPE.ToString(), Enums.QuestionCode.CERT_PRO_PRAC_SCOPE_UNIQUE_PATIENT.ToString()) Then
                        Dim updateResponseResults = AMCCertRecertController.UpdateCustomerSurveyResponse(hdCurrentSurveyId.Value, hdUniquePatientQuestionId.Value,
                                         hdUniquePatientAnswerId.Value, hdUniquePatientResponseId.Value,
                                         txtUniquePatientResponse.Text,
                                         MasterCustomerId, SubCustomerId, OrganizationId, OrganizationUnitId,
                                         CertificationId)
                        If updateResponseResults IsNot Nothing AndAlso updateResponseResults.Count > 0 Then
                            For Each updateResponseResult As IIssue In updateResponseResults
                                results.Add(updateResponseResult)
                            Next
                        End If
                    End If
                End If
                
                If results Is Nothing OrElse results.Count <= 0 Then
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                End If
                BindingDataToList()
                If fieldQuestion IsNot Nothing AndAlso fieldQuestion.IsEnabled Then
                    BindQuestion()
                End If
                If rptscope.Items Is Nothing OrElse rptscope.Items.Count <= 0 Then
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                End If
                Return results
            Catch ex As Exception
                ProcessException(ex)
            End Try
            Return Nothing
        End Function

        Protected Sub rptscope_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptscope.ItemCommand
            Try
                If e.CommandName.Equals("Delete") Then
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                    Dim practiceScopeItem As IUserDefinedCustomerPracticeScope
                    practiceScopeItem = amcCertRecertController.GetPracticeScopeByGuiID(e.CommandArgument.ToString(),
                                                                                                Me.MasterCustomerId, Me.SubCustomerId)
                    amcCertRecertController.DeletePracticeScope(practiceScopeItem)
                End If
                BindingDataToList()
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Protected Sub rptscope_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptscope.ItemDataBound
            Try
                If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
                    Dim userdine As UserDefinedCustomerPracticeScope = CType(e.Item.DataItem, UserDefinedCustomerPracticeScope)
                    If userdine IsNot Nothing Then
                        Dim lblCodeType As Label = CType(e.Item.FindControl("lblScopetype"), Label)
                        Dim lblCodeTypeValue As Label = CType(e.Item.FindControl("lblScopeTypeValue"), Label)
                        If lblCodeType IsNot Nothing AndAlso lblCodeTypeValue IsNot Nothing Then
                            lblCodeType.Text = userdine.ScopeType.List(userdine.ScopeTypeString).Description
                            lblCodeTypeValue.Text = userdine.ScopeType.List(userdine.ScopeTypeString).Code
                        End If
                        Dim lblCPTCode As Label = CType(e.Item.FindControl("lblCPTCode"), Label)
                        Dim lblCPTCodeValue As Label = CType(e.Item.FindControl("lblCPTCodeValue"), Label)
                        If userdine.ScopeType.List(userdine.ScopeTypeString).Code = "PROCEDURES" Then
                            lblCPTCode.Text = userdine.ScopeText
                            lblCPTCodeValue.Text = userdine.ScopeText
                        Else
                            lblCPTCode.Text = userdine.CptCode.List(userdine.CptCodeString).Description
                            lblCPTCodeValue.Text = userdine.CptCode.List(userdine.CptCodeString).Code
                        End If
                        '' bind number
                        Dim lblNumberOfService As Label = CType(e.Item.FindControl("lblNumberOfService"), Label)
                        If lblNumberOfService IsNot Nothing Then
                            If userdine.ScopeNbrServices <> 0 Then
                                lblNumberOfService.Text = userdine.ScopeNbrServices.ToString()
                            Else
                                lblNumberOfService.Text = String.Empty
                            End If
                        End If
                        Dim lblAverageTime As Label = CType(e.Item.FindControl("lblAverageTime"), Label)
                        If lblAverageTime IsNot Nothing Then
                            If userdine.ScopeAvgTimePerService <> 0 Then
                                lblAverageTime.Text = userdine.ScopeAvgTimePerService.ToString()
                            Else
                                lblAverageTime.Text = String.Empty
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

#End Region

#Region "Private Methods"

        Public Overrides Sub ValidateFormFillCompleted()
            If Not Page.IsPostBack Then
                BindScopetype(ddlScopetype)
            End If
            BindingDataToList()
            Dim fieldQuestion = GetFieldInfo("QuestionList")
            If fieldQuestion IsNot Nothing AndAlso fieldQuestion.IsEnabled Then
                BindQuestion()
            End If
        End Sub

        Private Sub BindQuestion()
            Dim survey = amcCertRecertController.GetSurveyByTitle(DataAccessConstants.CERTIFICATION_PROFESSIONAL_PRACTICE_SCOPE.ToString(), OrganizationId, OrganizationUnitId)
            If survey IsNot Nothing Then
                hdCurrentSurveyId.Value = survey.SurveyId.ToString()
                Dim questions = CType(survey, UserDefinedSurvey).UserDefinedSurveyQuestions
                If questions IsNot Nothing AndAlso questions.Count > 0 Then
                    For Each scopeQuestion As UserDefinedSurveyQuestion In questions
                        If scopeQuestion.Enabled AndAlso scopeQuestion.QuestionCode = Enums.QuestionCode.CERT_PRO_PRAC_SCOPE_UNIQUE_PATIENT.ToString() Then
                            hdUniquePatientQuestionId.Value = scopeQuestion.QuestionId.ToString()
                            lblUniquePatientQuestion.Text = scopeQuestion.QuestionText
                            Dim answerObj = CType(survey, UserDefinedSurvey).UserDefinedSurveyAnsweres.FindObject("QuestionId", scopeQuestion.QuestionId)
                            If answerObj IsNot Nothing Then
                                Dim answer = CType(answerObj, UserDefinedSurveyAnswers)
                                hdUniquePatientAnswerId.Value = answer.AnswerId.ToString()
                                Dim responses = CType(survey, UserDefinedSurvey).UserDefinedCustomerSurveyResponsees
                                If responses IsNot Nothing AndAlso responses.Count > 0 Then
                                    For Each amcresponse As UserDefinedCustomerSurveyResponses In responses
                                        If amcresponse.QuestionId = scopeQuestion.QuestionId AndAlso amcresponse.AnswerId = answer.AnswerId AndAlso amcresponse.MasterCustomerId = MasterCustomerId AndAlso amcresponse.SubcustomerId = SubCustomerId Then
                                            hdUniquePatientResponseId.Value = amcresponse.ResponseId.ToString()
                                            txtUniquePatientResponse.Text = amcresponse.Comments
                                            Exit For
                                        End If
                                    Next
                                End If
                            End If
                            Question.Visible = True
                            Exit For
                        End If
                    Next
                End If
            End If
            If IsQuestionExisted(DataAccessConstants.CERTIFICATION_PROFESSIONAL_PRACTICE_SCOPE.ToString(), Enums.QuestionCode.CERT_PRO_PRAC_SCOPE_UNIQUE_PATIENT.ToString()) AndAlso (survey IsNot Nothing AndAlso String.IsNullOrEmpty(hdUniquePatientResponseId.Value) OrElse String.IsNullOrEmpty(txtUniquePatientResponse.Text)) Then
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
            End If
        End Sub

        ''' <summary>
        ''' Binding Data to repeater
        ''' </summary>
        ''' <param></param>
        Private Sub BindingDataToList()
            Dim practiceScopeList As IUserDefinedCustomerPracticeScopes
            practiceScopeList = amcCertRecertController.GetPracticeScopeList(
                                                                    Me.MasterCustomerId,
                                                                    Me.SubCustomerId)
            If practiceScopeList.Count > 0 AndAlso Not Page.IsPostBack Then
                hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED

            End If
            Me.rptscope.DataSource = practiceScopeList
            Me.rptscope.DataBind()
        End Sub

        ''' <summary>
        ''' Insert or Edit Item to list Cache
        ''' </summary>
        ''' <param></param>
        Private Sub SavePracticeScope()
            Dim practiceScopeList As New UserDefinedCustomerPracticeScopes
            Dim practiceScopeItem As IUserDefinedCustomerPracticeScope
            Dim issueCollection As IIssuesCollection
            ''edit 
            If (Me.hdCurrentObjectUniqueId.Value <> String.Empty) Then
                practiceScopeItem = amcCertRecertController.GetPracticeScopeByGuiID(
                                                                            Me.hdCurrentObjectUniqueId.Value,
                                                                            Me.MasterCustomerId, Me.SubCustomerId)
                SetPropertiesForObject(practiceScopeItem)
                issueCollection = amcCertRecertController.UpdatePracticeScope(practiceScopeItem)
            Else
                ''Insert
                practiceScopeItem = practiceScopeList.CreateNew()
                SetPropertiesForObject(practiceScopeItem)
                practiceScopeItem.IsNewObjectFlag = True
                issueCollection = amcCertRecertController.InsertPracticeScope(practiceScopeItem)
            End If
            If issueCollection.Count > 0 Then
                ShowError(issueCollection, lblPopupMessage)
                hdIsValidateFailed.Value = CommonConstants.TAB_INCOMPLETED
            Else
                hdIsValidateFailed.Value = CommonConstants.TAB_COMPLETED
            End If
            BindingDataToList()
        End Sub

        ''' <summary>
        ''' Set property from GUI for Item Object
        ''' </summary>
        ''' <param name="ObjectItem"> Item Object need to set values </param>
        Private Sub SetPropertiesForObject(ByRef ObjectItem As IUserDefinedCustomerPracticeScope)
            If ObjectItem IsNot Nothing Then
                ObjectItem.ValidationIssuesForMe.RemoveAll()
                With ObjectItem
                    .MasterCustomerId = Me.MasterCustomerId
                    .SubcustomerId = Me.SubCustomerId
                    .ScopeTypeString = hdCurrentSelectedScopeType.Value
                    If ddlScopetype.SelectedValue = "PROCEDURES" Then
                        .ScopeText = txtProCode.Text
                    Else
                        .CptCodeString = hdCurrentSelectedCptCode.Value
                    End If
                    If CommonHelper.CheckIsNumber(txtNumberOfService.Text) Then
                        .ScopeNbrServices = Integer.Parse(txtNumberOfService.Text)
                    Else
                        .ScopeNbrServices = 0
                    End If
                    If CommonHelper.CheckIsNumber(txtAverageTime.Text) Then
                        .ScopeAvgTimePerService = Integer.Parse(txtAverageTime.Text)
                    Else
                        .ScopeAvgTimePerService = 0
                    End If
                End With
            End If
        End Sub

        Private Sub AddEventHandlers()
            AddHandler btnSave.Click, AddressOf btnSaveClick
        End Sub

        Private Sub BindScopetype(ByVal ddlName As DropDownList)
            Dim list As UserDefinedCustomerPracticeScopes = New UserDefinedCustomerPracticeScopes()
            Dim itemData As IUserDefinedCustomerPracticeScope
            itemData = list.CreateNew()
            ddlName.DataTextField = "Description"
            ddlName.DataValueField = "Code"
            ddlName.DataSource = itemData.ScopeType.List
            ddlName.DataBind()
        End Sub

        Public Shared Function BindCPTCodeType() As List(Of CusCPTCodeList)
            Dim cotCodeList = New List(Of CusCPTCodeList)
            Dim practiceScopes As UserDefinedCustomerPracticeScopes = New UserDefinedCustomerPracticeScopes()
            Dim practiceScope As IUserDefinedCustomerPracticeScope
            practiceScope = practiceScopes.CreateNew()
            For Each codeEntry As ICodeEntry In practiceScope.ScopeType.List
                practiceScope.ScopeTypeString = codeEntry.Code ''filter
                practiceScope.CptCode.FillList()
                If practiceScope.CptCode.List IsNot Nothing AndAlso practiceScope.CptCode.List.Count > 0 Then
                    Dim cusCPTCodeList = New CusCPTCodeList
                    cusCPTCodeList.Code = codeEntry.Code
                    cusCPTCodeList.Description = codeEntry.Description
                    cusCPTCodeList.CPTCodes = New List(Of CusCPTCode)
                    For Each cdpEntry As ICodeEntry In practiceScope.CptCode.List
                        Dim cptCodeEntry = New CusCPTCode
                        cptCodeEntry.Code = cdpEntry.Code
                        cptCodeEntry.Description = cdpEntry.Description
                        cusCPTCodeList.CPTCodes.Add(cptCodeEntry)
                    Next
                    cotCodeList.Add(cusCPTCodeList)
                End If
            Next
            Return cotCodeList
        End Function
#End Region

    End Class
End Namespace
