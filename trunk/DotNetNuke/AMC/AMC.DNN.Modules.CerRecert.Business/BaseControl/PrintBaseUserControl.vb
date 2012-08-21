Imports System.Web.UI
Imports AMC.DNN.Modules.CertRecert.Data.Enums
Imports AMC.DNN.Modules.CertRecert.Business.Entities
Imports System.Linq
Imports TIMSS.API.User.UserDefinedInfo

Namespace BaseControl
    Public Class PrintBaseUserControl
        Inherits BaseUserControl

        ''' <summary>
        ''' Gets the exam choice survey title.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetExamChoiceSurveyTitle() As String
            Return DataAccessConstants.CERTIFICATION_EXAM_CHOICE_SURVEY_TITLE.ToString()
        End Function

        ''' <summary>
        ''' Shows the error message.
        ''' </summary>
        ''' <param name="message">The message.</param>
        Public Overrides Sub ShowErrorMessage(ByVal message As String)
        End Sub

        ''' <summary>
        ''' Sets the current re cert option.
        ''' </summary>
        ''' <param name="questionId">The question id.</param>
        Public Overrides Sub SetCurrentReCertOption(ByVal questionId As String)
            Session("CurrentReCertOption") = questionId.ToString()
        End Sub

        ''' <summary>
        ''' Gets the current re cert option.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function GetCurrentReCertOption() As UserDefinedSurveyQuestion
            If CurrentFormInfo IsNot Nothing AndAlso CurrentFormInfo.Sections IsNot Nothing Then
                For Each section As SectionInfo In CurrentFormInfo.Sections
                    If section.SectionId = "RecertificationOptionUC" AndAlso section.IsEnabled Then
                        If Session("CurrentReCertOption") Is Nothing OrElse String.IsNullOrEmpty(Session("CurrentReCertOption").ToString()) Then
                            Return Nothing
                        Else
                            'Dim result As Enums.QuestionCode = CType([Enum].Parse(GetType(Enums.QuestionCode), hdCurrentReCertOptionCode.Value, True), Enums.QuestionCode)
                            'Return result
                            If section.Fields IsNot Nothing AndAlso section.Fields.Count > 0 Then
                                For Each fieldInfo As FieldInfo In section.Fields
                                    If fieldInfo.FieldId = "QuestionList" AndAlso fieldInfo.IsEnabled Then
                                        Dim currentOption = AMCCertRecertController.GetQuestionById(OrganizationId, OrganizationUnitId, Session("CurrentReCertOption").ToString())
                                        If currentOption.Enabled Then
                                            Return currentOption
                                        End If
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                    End If
                Next
            End If
            Session("CurrentReCertOption") = Nothing
            Return Nothing
        End Function

        ''' <summary>
        ''' Handles the Load event of the Page control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        End Sub

        ''' <summary>
        ''' Initializes the components.
        ''' </summary>
        Public Overridable Sub InitializeComponents()
            Dim printContainer = FindControl("printContainer")
            For Each sectionControl As Control In printContainer.Controls
                If sectionControl IsNot Nothing AndAlso TypeOf sectionControl Is SectionBaseUserControl Then
                    Dim tabControl As SectionBaseUserControl
                    tabControl = CType(sectionControl, SectionBaseUserControl)
                    tabControl.ModuleConfiguration = ModuleConfiguration
                    tabControl.LocalResourceFile = LocalResourceFile
                    tabControl.CertificationId = CertificationId
                    tabControl.CurrentCertificationCustomerCertification = CurrentCertificationCustomerCertification
                    tabControl.CurrentFormInfo = CurrentFormInfo
                    tabControl.ShowErrorMessage = AddressOf ShowErrorMessage
                    tabControl.GetCurrentReCertOptionAction = AddressOf GetCurrentReCertOption
                    tabControl.AMCCertRecertController = AMCCertRecertController
                    tabControl.ReferencAndVeryficationSurveyTitle = ReferencAndVeryficationSurveyTitle()
                    tabControl.PrintMode = True
                End If
            Next
        End Sub

        Public Sub ResequenceSections()
            Dim sections = New List(Of Control)
            Dim printContainer = FindControl("printContainer")
            If printContainer IsNot Nothing Then
                If CurrentFormInfo IsNot Nothing AndAlso CurrentFormInfo.Sections IsNot Nothing Then
                    Dim sectionsConfig = CurrentFormInfo.Sections.Where(Function(x) x.IsEnabled).OrderBy(Function(x) x.Sequence)
                    For Each section As SectionInfo In sectionsConfig
                        Dim sectionControl = printContainer.FindControl(String.Format("section{0}", section.SectionId))
                        If sectionControl IsNot Nothing Then
                            printContainer.Controls.Remove(sectionControl)
                            sections.Add(sectionControl)
                        End If
                    Next
                End If
                For Each control As Control In sections
                    printContainer.Controls.Add(control)
                Next
            End If

        End Sub
    End Class
End Namespace