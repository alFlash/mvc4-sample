Imports AMC.DNN.Modules.CertRecert.Data.Enums
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Business.Controller
Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Business.Entities
Imports AMC.DNN.Modules.CertRecert.Business.Helpers

Imports System.Threading

Imports TIMSS.API.User.UserDefinedInfo
Imports TIMSS.API.Core.Validation

Namespace Controls.InactiveStatus
    Public Class ApplyInactiveStatusUC
        Inherits BaseUserControl

#Region "Protected Mebmer"
        Public Property CertificationId() As Integer
#End Region

#Region "Event handler"
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

            Try
                ApplyConfigurations(CurrentFormInfo, Me)
                Dim quesitonVisible = CheckForQuestionList()
                If Not Page.IsPostBack Then
                    lblNote.Text = String.Empty
                    If quesitonVisible Then
                        BindingDataToList()
                    End If
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Protected Sub Page_PreRender(ByVal s As Object, ByVal e As EventArgs) Handles Me.PreRender
            Try
                PaymentProcessed = False
                Dim productIds = GetProductIds()
                If productIds IsNot Nothing AndAlso productIds.Length > 0 AndAlso
                   AMCCertRecertController.WasPaymentProcessed(OrganizationId, OrganizationUnitId, MasterCustomerId, SubCustomerId, productIds(0)) Then
                    PaymentProcessed = True
                    ShowErrorMessage("Payment Already Processed.")
                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Public Overrides Function GetProductIds(Optional ByVal currentRecertOption As UserDefinedSurveyQuestion = Nothing) As Integer()
            Dim otherSettings = ModuleConfigurationHelper.Instance.GetOtherModuleSettings(Server.MapPath(ParentModulePath))
            Dim ret = New Integer() {CInt(otherSettings.InactiveStatusProductId)}
            Return ret
        End Function

        ''' <summary>
        ''' Checks for question list.
        ''' </summary>
        Private Function CheckForQuestionList() As Boolean
            For Each sectionInfo As SectionInfo In CurrentFormInfo.Sections
                If sectionInfo.SectionId = Me.GetType().BaseType.Name Then
                    For Each fieldInfo As FieldInfo In sectionInfo.Fields
                        If fieldInfo.IsQuestion AndAlso Not fieldInfo.IsEnabled Then
                            rptQualifyingEvent.Visible = False
                            Return False
                        End If
                    Next
                    Exit For
                End If
            Next
            Return True
        End Function

        ''' <summary>
        ''' Applies the configurations.
        ''' </summary>
        ''' <param name="formInfo">The form info.</param>
        ''' <param name="control">The control.</param>
        Private Shared Sub ApplyConfigurations(ByVal formInfo As FormInfo, ByVal control As Control)
            If formInfo IsNot Nothing Then
                Dim headerControl = control.FindControl(String.Format("lbl{0}", formInfo.FormId))
                If headerControl IsNot Nothing AndAlso TypeOf headerControl Is Label Then
                    CType(headerControl, Label).Text = formInfo.FormValue
                End If
                For Each sectionInfo As SectionInfo In formInfo.Sections
                    If sectionInfo.Fields IsNot Nothing Then
                        For Each fieldInfo As FieldInfo In sectionInfo.Fields
                            ModuleConfigurationHelper.Instance.ApplyFieldConfiguration(control.FindControl(fieldInfo.FieldId), fieldInfo)
                        Next
                    End If
                Next
            End If
        End Sub

        ''' <summary>
        ''' Shows the error message.
        ''' </summary>
        ''' <param name="message">The message.</param>
        Public Overrides Sub ShowErrorMessage(ByVal message As String)
            lblNote.Text = message
        End Sub

        Public Overrides Sub SetCurrentReCertOption(ByVal questionId As String)
            'Throw New NotImplementedException()
        End Sub

        Public Overrides Function GetCurrentReCertOption() As UserDefinedSurveyQuestion
            Return Nothing
        End Function

        ''' <summary>
        ''' Handles the ItemDataBound event of the rptQualifyingEvent control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="System.Web.UI.WebControls.RepeaterItemEventArgs" /> instance containing the event data.</param>
        Protected Sub rptQualifyingEvent_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptQualifyingEvent.ItemDataBound
            Try
                If e.Item.ItemType = ListItemType.Item OrElse
                      e.Item.ItemType = ListItemType.AlternatingItem Then
                    Dim chkYesNo = CType(e.Item.FindControl("chkYesNo"), CheckBox)
                    Dim hdAnswerYes = CType(e.Item.FindControl("hdAnswerYes"), HiddenField)
                    Dim hdAnswerNo = CType(e.Item.FindControl("hdAnswerNo"), HiddenField)
                    Dim hdResponseId = CType(e.Item.FindControl("hdResponseId"), HiddenField)
                    Dim surveyQuestion = CType(e.Item.DataItem, UserDefinedSurveyQuestion)
                    Dim surveyResponse =
                        amcCertRecertController.GetCustomerSurveyResponseByQuestionId(
                                                                    surveyQuestion.QuestionId,
                                                                    surveyQuestion.SurveyId,
                                                                    Me.MasterCustomerId,
                                                                    Me.SubCustomerId)

                    If surveyResponse IsNot Nothing Then
                        hdResponseId.Value = surveyResponse.ResponseId.ToString()
                        Dim surveyAnswer =
                            CType(surveyQuestion.UserDefinedSurveyAnsweres.FindObject("AnswerId", surveyResponse.AnswerId), UserDefinedSurveyAnswers)
                        If surveyAnswer.AnswerText.Equals("YES") Then
                            chkYesNo.Checked = True
                        End If
                    End If

                    For Each surveyAnswer As IUserDefinedSurveyAnswers In surveyQuestion.UserDefinedSurveyAnsweres
                        If surveyAnswer.QuestionId = surveyQuestion.QuestionId Then
                            If surveyAnswer.AnswerText.Equals("YES") Then
                                hdAnswerYes.Value = surveyAnswer.AnswerId.ToString()
                            Else
                                hdAnswerNo.Value = surveyAnswer.AnswerId.ToString()
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmit.Click
            Dim results As New IssuesCollection
            Try
                results = CType(SaveCustomerResponsees(), IssuesCollection)
                If results Is Nothing OrElse results.Count <= 0 Then '' check survey complete, navigate to payment form 
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                    PaymentProcess(GetProductIds())
                Else '' have error => show message
                    lblNote.Text = " Process is not complete, Please try again "
                End If
            Catch ex1 As ThreadAbortException
                'nothing here 
                ''http://www.dotnetnuke.com/Community/Community-Exchange/Question/142/How-to-solve-Thread-was-being-aborted-error.aspx
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
            Response.Redirect(NavigateURL(TabId, "", String.Format("{0}={1}", CommonConstants.USER_CONTROL_PARAMETER, "InactiveStatus/CertificationTypeSelection")))
        End Sub

        Public Overrides Function GetExamChoiceSurveyTitle() As String
            Return String.Empty
        End Function
#End Region

#Region "Private member"
        Private Function SaveCustomerResponsees() As IIssuesCollection
            Dim issueColletion As IIssuesCollection = Nothing
            Dim surveyIdValue = Long.Parse(Me.hdSurveyId.Value)
            For Each rptItem As RepeaterItem In Me.rptQualifyingEvent.Items
                Dim hdEnabled = CType(rptItem.FindControl("hdQuestionEnabled"), HiddenField)
                If hdEnabled IsNot Nothing AndAlso Not String.IsNullOrEmpty(hdEnabled.Value) AndAlso Convert.ToBoolean(hdEnabled.Value) Then
                    Dim chkYesNo = CType(rptItem.FindControl("chkYesNo"), CheckBox)
                    Dim hdAnswerYes = CType(rptItem.FindControl("hdAnswerYes"), HiddenField)
                    Dim hdAnswerNo = CType(rptItem.FindControl("hdAnswerNo"), HiddenField)
                    Dim hdResponseId = CType(rptItem.FindControl("hdResponseId"), HiddenField)

                    Dim hdQuestionId = CType(rptItem.FindControl("hdQuestionId"), HiddenField)
                    Dim responseIdValue = Long.Parse(hdResponseId.Value)
                    Dim questionIdValue = Long.Parse(hdQuestionId.Value)


                    Dim customerResponse As IUserDefinedCustomerSurveyResponses
                    If Long.Parse(hdResponseId.Value) > 0 Then ''has existing response
                        customerResponse =
                            AMCCertRecertController.GetCustomerSurveyResponseByReponseId(
                                                                            responseIdValue,
                                                                            surveyIdValue,
                                                                            MasterCustomerId,
                                                                            SubCustomerId)
                        With customerResponse
                            .MasterCustomerId = MasterCustomerId
                            .SubcustomerId = SubCustomerId
                            .SurveyId = surveyIdValue
                            .QuestionId = questionIdValue
                            If chkYesNo.Checked Then
                                .AnswerId = Long.Parse(hdAnswerYes.Value)
                            Else
                                .AnswerId = Long.Parse(hdAnswerNo.Value)
                            End If
                        End With
                        issueColletion = AMCCertRecertController.UpdateCustomerSurveyResponse(customerResponse)
                    Else
                        customerResponse = (New UserDefinedCustomerSurveyResponsees()).CreateNew()
                        With customerResponse
                            .IsNewObjectFlag = True
                            .MasterCustomerId = MasterCustomerId
                            .SubcustomerId = SubCustomerId
                            .SurveyId = surveyIdValue
                            .QuestionId = questionIdValue
                            If chkYesNo.Checked Then
                                .AnswerId = Long.Parse(hdAnswerYes.Value)
                            Else
                                .AnswerId = Long.Parse(hdAnswerNo.Value)
                            End If
                        End With
                        issueColletion = AMCCertRecertController.InsertCustomerSurveyResponse(customerResponse)
                    End If
                End If
            Next
            issueColletion = AMCCertRecertController.CommitCustomerSurveyResponsees(surveyIdValue,
                                                                                    MasterCustomerId,
                                                                                    SubCustomerId)
            If issueColletion Is Nothing OrElse issueColletion.Count < 1 Then
                BindingDataToList()
            End If
            Return issueColletion
        End Function

        Private Sub BindingDataToList()
            Dim surveyQuestionaire As UserDefinedSurveyQuestions
            Dim survey As UserDefinedSurvey = Nothing
            survey = CType(AMCCertRecertController.GetSurveyByTitle(DataAccessConstants.QUALIFYING_EVENT_SURVEY_TITLE), UserDefinedSurvey)
            If survey IsNot Nothing Then
                Me.hdSurveyId.Value = survey.SurveyId.ToString()
                surveyQuestionaire =
                    CType(survey.UserDefinedSurveyQuestions, UserDefinedSurveyQuestions)
            End If
            Me.rptQualifyingEvent.DataSource = surveyQuestionaire
            Me.rptQualifyingEvent.DataBind()
        End Sub
#End Region

    End Class
End Namespace