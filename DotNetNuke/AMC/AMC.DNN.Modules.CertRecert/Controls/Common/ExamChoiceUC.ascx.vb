Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Data.Enums
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Business.Controller
Imports AMC.DNN.Modules.CertRecert.Business.BusinessValidation
Imports TIMSS.API.Core

Imports TIMSS.API.Core.Validation
Imports TIMSS.API.User.UserDefinedInfo

Namespace Controls.Common
    ''' <summary>
    ''' Prepresent GUI for ExamChoiceUC tab 
    ''' </summary>
    Public Class ExamChoiceUC
        Inherits SectionBaseUserControl

#Region "Event Handlers"
        ''' <summary>
        ''' Handles the Load event of the Page control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            Try
                Page.ClientScript.RegisterClientScriptInclude("examchoiceUc", String.Format("{0}/Documentation/scripts/examchoice.js?v={1}", ParentModulePath, CommonConstants.CURRENT_VERSION))
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Protected Sub rptExamChoice_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptExamChoice.ItemDataBound
            Try
                If e.Item.ItemType = ListItemType.AlternatingItem OrElse
                    e.Item.ItemType = ListItemType.Item Then
                    Dim surveyAnswer = CType(e.Item.DataItem, IUserDefinedSurveyAnswers)
                    Dim certAppExamPeriod =
                        AMCCertRecertController.GetCertAppExamPeriodByQuestionId(surveyAnswer.AnswerId)
                    Dim trExamChoiceItem = e.Item.FindControl("trExamChoiceItem")
                    If certAppExamPeriod IsNot Nothing Then
                        Dim lblExamAdministration =
                            CType(e.Item.FindControl("lblExamAdministration"), Label)
                        Dim lblExamDate = CType(e.Item.FindControl("lblExamDate"), Label)
                        Dim rdbExamAdministration =
                            CType(e.Item.FindControl("rdbExamAdministration"), RadioButton)

                        lblExamAdministration.Text =
                            String.Format("{0} - {1}",
                                        certAppExamPeriod.StartDate.ToString(CommonConstants.DATE_FORMAT),
                                        certAppExamPeriod.EndDate.ToString(CommonConstants.DATE_FORMAT))
                        lblExamDate.Text = certAppExamPeriod.ExamDate.ToString(CommonConstants.DATE_FORMAT)
                        If certAppExamPeriod.QuestionId =
                                            Long.Parse(Me.hdExamChoiceResponseAnswerId.Value) Then
                            rdbExamAdministration.Checked = True
                        Else
                            rdbExamAdministration.Checked = False
                        End If
                        If certAppExamPeriod.ExamDate < DateTime.Today Then
                            trExamChoiceItem.Visible = False
                        End If
                    Else
                        trExamChoiceItem.Visible = False
                    End If
                End If
            Catch ex As Exception
                ProcessException(ex)
            End Try
        End Sub

        Public Overrides Function Save() As IIssuesCollection
            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
            Dim results As New IssuesCollection
            Try
                Dim fieldQuestionList = GetFieldInfo("QuestionList")
                If fieldQuestionList IsNot Nothing AndAlso fieldQuestionList.IsEnabled Then
                    If rptExamChoice.Items IsNot Nothing AndAlso rptExamChoice.Items.Count > 0 Then
                        results = CType(SaveCustomerResponse(), IssuesCollection)
                        If results Is Nothing OrElse results.Count <= 0 Then
                            hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                            Dim surveyIdValue = Long.Parse(Me.hdSurveyId.Value)
                            AMCCertRecertController.RefreshCustomerSurveyResponsees(surveyIdValue, MasterCustomerId, SubCustomerId)
                        End If
                        Dim examchoiceReponses = BindingDataToExamChoiceRepeater()
                        If examchoiceReponses Is Nothing Then
                            hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                        End If
                    End If
                Else
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                End If
                
            Catch ex As Exception
                ProcessException(ex)
            End Try
            Return results
        End Function

        Private Function SaveCustomerResponse() As IIssuesCollection
            Dim issuesCollection As IssuesCollection = New IssuesCollection()
            Dim objCusSurveyReponse As IUserDefinedCustomerSurveyResponses
            Dim examChoiceSurveyId = Long.Parse(hdSurveyId.Value)
            Dim cusResponseId = Long.Parse(hdExamChoiceResponseId.Value)
            Dim cusQuestionId = Long.Parse(hdExamChoiceQuestionId.Value)
            Dim cusSurveyAnswerResponseId As Long = 0

            If rptExamChoice.Items IsNot Nothing AndAlso rptExamChoice.Items.Count > 0 Then
                For Each rptItem As RepeaterItem In rptExamChoice.Items
                    Dim rdbExamAdministration =
                                CType(rptItem.FindControl("rdbExamAdministration"), RadioButton)
                    Dim hdAnswerId =
                                CType(rptItem.FindControl("hdAnswerId"), HiddenField)
                    If rdbExamAdministration.Checked Then
                        cusSurveyAnswerResponseId = Long.Parse(hdAnswerId.Value)
                        Exit For
                    End If
                Next
            End If

            If cusSurveyAnswerResponseId > 0 Then
                Dim examChoiceQuestion = AMCCertRecertController.GetQuestionById(Me.OrganizationId,
                                                                                 Me.OrganizationUnitId,
                                                                                 cusQuestionId.ToString())
                If examChoiceQuestion IsNot Nothing Then
                    Dim examChoiceAnswer As UserDefinedSurveyAnswers = Nothing
                    For Each answerItem As UserDefinedSurveyAnswers In examChoiceQuestion.UserDefinedSurveyAnsweres
                        If answerItem.AnswerId.ToString().Equals(cusSurveyAnswerResponseId.ToString()) Then
                            examChoiceAnswer = answerItem
                            Exit For
                        End If
                    Next
                    If examChoiceAnswer Is Nothing Then
                        issuesCollection.Add(New AtLeastOneRecordIssue(New BusinessObject(),
                                                                       Localization.GetString("AnswerContraintError.Text", Me.LocalResourceFile)))
                        BindingDataToExamChoiceRepeater()
                        Return issuesCollection
                    End If
                End If
                
                If cusResponseId > 0 Then
                    objCusSurveyReponse =
                        AMCCertRecertController.GetCustomerSurveyResponseByReponseId(cusResponseId,
                                                                                     examChoiceSurveyId,
                                                                                     Me.MasterCustomerId,
                                                                                     Me.SubCustomerId)
                    objCusSurveyReponse.AnswerId = cusSurveyAnswerResponseId
                    AMCCertRecertController.UpdateCustomerSurveyResponse(objCusSurveyReponse)
                Else
                    objCusSurveyReponse = (New UserDefinedCustomerSurveyResponsees()).CreateNew()
                    With objCusSurveyReponse
                        .IsNewObjectFlag = True
                        .SubcustomerId = Me.SubCustomerId
                        .MasterCustomerId = Me.MasterCustomerId
                        .SurveyId = examChoiceSurveyId
                        .QuestionId = cusQuestionId
                        .AnswerId = cusSurveyAnswerResponseId
                    End With
                    AMCCertRecertController.InsertCustomerSurveyResponse(objCusSurveyReponse)
                End If
                If objCusSurveyReponse.ValidationIssuesForMe IsNot Nothing Then
                    For Each issue As IIssue In objCusSurveyReponse.ValidationIssuesForMe
                        issuesCollection.Add(issue)
                    Next
                End If
                If issuesCollection.Count < 1 Then
                    Dim issuesCommit = AMCCertRecertController.CommitCustomerSurveyResponsees(examChoiceSurveyId,
                                                                           Me.MasterCustomerId,
                                                                           Me.SubCustomerId)
                    If issuesCommit IsNot Nothing Then
                        For Each issue As IIssue In issuesCommit
                            issuesCollection.Add(issue)
                        Next
                    End If
                End If
            End If
            Return issuesCollection
        End Function
#End Region

#Region "Private Method"
        Public Overrides Sub ValidateFormFillCompleted()
            Dim fieldQuestionList = GetFieldInfo("QuestionList")
            If fieldQuestionList IsNot Nothing AndAlso fieldQuestionList.IsEnabled Then
                BindingDataToExamChoiceRepeater()
            Else
                hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
            End If
        End Sub

        Private Function BindingDataToExamChoiceRepeater() As Object
            Dim currentResponse As Object = Nothing
            Dim examChoiceSurvey =
                        CType(AMCCertRecertController.GetSurveyByTitle(
                                            DataAccessConstants.CERTIFICATION_EXAM_CHOICE_SURVEY_TITLE), 
                                            UserDefinedSurvey)
            Dim examChoiceQuestions =
                        CType(examChoiceSurvey.UserDefinedSurveyQuestions, UserDefinedSurveyQuestions)

            If examChoiceQuestions IsNot Nothing AndAlso examChoiceQuestions.Count > 0 Then

                Dim examChoiceQuestion = examChoiceQuestions(0)
                If examChoiceQuestion.Enabled Then
                    Dim objExamChoiceResponse =
                   AMCCertRecertController.GetCustomerSurveyResponseByQuestionId(
                                                                       examChoiceQuestion.QuestionId,
                                                                       examChoiceQuestion.SurveyId,
                                                                       Me.MasterCustomerId,
                                                                       Me.SubCustomerId)
                    hdSurveyId.Value = examChoiceQuestion.SurveyId.ToString()
                    hdExamChoiceQuestionId.Value = examChoiceQuestion.QuestionId.ToString()
                    If objExamChoiceResponse IsNot Nothing Then
                        hdExamChoiceResponseId.Value = objExamChoiceResponse.ResponseId.ToString()
                        hdExamChoiceResponseAnswerId.Value = objExamChoiceResponse.AnswerId.ToString()
                        hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                        currentResponse = objExamChoiceResponse
                    End If
                    rptExamChoice.DataSource =
                        CType(examChoiceQuestion, UserDefinedSurveyQuestion).UserDefinedSurveyAnsweres
                    rptExamChoice.DataBind()
                End If
            End If
            Return currentResponse
        End Function
#End Region

    End Class
End Namespace