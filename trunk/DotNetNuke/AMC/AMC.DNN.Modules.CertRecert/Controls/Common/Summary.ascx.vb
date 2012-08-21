Imports AMC.DNN.Modules.CertRecert.Business.BaseControl
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems
Imports AMC.DNN.Modules.CertRecert.Controls.Certifications
Imports AMC.DNN.Modules.CertRecert.Business.Entities
Imports AMC.DNN.Modules.CertRecert.Business.Controller
Imports AMC.DNN.Modules.CertRecert.Business.Helpers
Imports TIMSS.API.Core.Validation
Imports System.Linq
Imports AMC.DNN.Modules.CertRecert.Data.Enums
Imports AMC.DNN.Modules.CertRecert.Business.BusinessValidation
Imports TIMSS.API.Core

Namespace Controls.Common
    Public Class Summary
        Inherits SectionBaseUserControl

#Region "public Member"
        Public totalCEMin As Decimal = 0
        Public Property ParentSaveButtonClick() As Action(Of Object, EventArgs)
        Public Property ParentSaveButton As Button
#End Region

#Region "Event Handlers"
        ''' <summary>
        ''' Handles the Load event of the Page control.
        ''' </summary>
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            Try
                If CheckConflictValidation(RuleID, ConflictMessage) = True AndAlso Not String.IsNullOrEmpty(ConflictMessage) Then
                    Dim issuesCollection As New IssuesCollection
                    issuesCollection.Add(New CESummaryIssue(New BusinessObject(), ConflictMessage))
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                Else
                    If RuleID = ValidationRuleId.RECERT_SUMMARY_COMMON_CALCULATOR.ToString() Then '' load summary form for ABNN
                        BindingDataToList(TotalCEOneForm)
                        BuildOptionTitle()
                    ElseIf RuleID = ValidationRuleId.RECERT_SUMMARY_ARN.ToString() Then '' load summary form for ARN
                        BindingDataToList_ARN(TotalPointARN)
                        BuildTitleTotalCEPoint_ARN()
                    End If
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Private Sub PreRenderPages(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
            If ParentSaveButtonClick IsNot Nothing Then
                ParentSaveButtonClick.Invoke(ParentSaveButton, e)
            End If
        End Sub

#End Region

#Region "ABNN validation"
        Public Overrides Sub ValidateFormFillCompleted()

        End Sub

        Private Sub BindingDataToList(ByRef totalCE As Decimal)
            Dim objectItem As SummaryObject
            Dim objectList As List(Of SummaryObject) = New List(Of SummaryObject)

            Dim arrProgramTypeEnum As New ArrayList
            For Each programtype As String In [Enum].GetNames(GetType(ActivityProgramType))
                If Not String.IsNullOrEmpty(programtype) Then
                    arrProgramTypeEnum.Add(programtype)
                End If
            Next
            Dim arr As List(Of SectionInfo) = If(CurrentFormInfo IsNot Nothing AndAlso CurrentFormInfo.Sections IsNot Nothing, CurrentFormInfo.Sections, New List(Of SectionInfo)())
            For Each sectionInfo As SectionInfo In arr
                If CheckExistItem(sectionInfo.SectionProgramType, arrProgramTypeEnum) Then   '' only list programType in Enum
                    If sectionInfo.IsEnabled = True AndAlso Not String.IsNullOrEmpty(sectionInfo.SectionProgramType) Then  ''Tab must be Enable 
                        objectItem = New SummaryObject()
                        objectItem.CECategoryName = GetCategoryByNameByProgramTypeCode(sectionInfo.SectionProgramType)
                        If objectItem.CECategoryName <> String.Empty Then '' have form must list out 
                            ''set form name
                            If sectionInfo.SectionProgramType = ActivityProgramType.EDUCATION.ToString() Then
                                objectItem.CECategoryName = String.Format("{0} {1}", objectItem.CECategoryName, GetContinuingEducationTitle())
                            End If
                            '' set CEpoint
                            Dim totalCEString = GetCookie(sectionInfo.SectionProgramType + MasterCustomerId)
                            totalCE += Decimal.Parse(totalCEString)
                            objectItem.TotalCEString = CommonHelper.ChopUnusedDecimal(totalCEString)
                            objectList.Add(objectItem)

                        End If
                    End If
                End If
            Next
            objectList.Sort(Function(x, y) String.Compare(x.CECategoryName, y.CECategoryName))
            Me.rptSummary.DataSource = objectList
            Me.rptSummary.DataBind()
            lblTotalCE.Text = CommonHelper.ChopUnusedDecimal(totalCE.ToString())
        End Sub

        Private Function GetCategoryByNameByProgramTypeCode(ByVal programTypeCode As String) As String
            Dim typeName As String = String.Empty
            Dim arr As List(Of SectionInfo) = If(CurrentFormInfo IsNot Nothing AndAlso CurrentFormInfo.Sections IsNot Nothing, CurrentFormInfo.Sections, New List(Of SectionInfo)())
            For Each sectionInfo As SectionInfo In arr
                If sectionInfo.IsEnabled = True Then
                    If sectionInfo.SectionProgramType = programTypeCode Then
                        typeName = sectionInfo.SectionValue
                        Exit For
                    End If
                End If
            Next
            Return typeName
        End Function

        Private Function CheckExistItem(ByVal item As String, ByVal arrItem As ArrayList) As Boolean
            For i As Integer = 0 To arrItem.Count - 1
                If arrItem.Item(i).ToString() = item Then
                    Return True
                End If
            Next
            Return False
        End Function

        Public Function GetFieldInfoTemp(ByVal fieldId As String,
                                         ByVal sectionInfo As SectionInfo) As Business.Entities.FieldInfo
            If sectionInfo IsNot Nothing And sectionInfo.Fields IsNot Nothing Then
                For Each fieldInfo As Business.Entities.FieldInfo In From fieldInfo1 In sectionInfo.Fields Where fieldInfo1.FieldId = fieldId
                    Return fieldInfo
                Next
            End If
            Return New Business.Entities.FieldInfo()
        End Function

        Public Overrides Function Save() As IIssuesCollection
            Dim issuesCollection As New IssuesCollection
            Dim errorMessages = String.Empty
            If CheckConflictValidation(RuleID, ConflictMessage) = True AndAlso Not String.IsNullOrEmpty(ConflictMessage) Then
                issuesCollection.Add(New CESummaryIssue(New BusinessObject(), ConflictMessage))
                hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
            End If
            If RuleID = ValidationRuleId.RECERT_SUMMARY_COMMON_CALCULATOR.ToString() Then '' load summary form for ABNN
                If Not CheckBusinessValidation(TotalCEOneForm, errorMessages) Then
                    issuesCollection.Add(New CESummaryIssue(New BusinessObject(), String.Format("{0}, you have {1}", errorMessages, lblTotalCE.Text)))
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                Else
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                End If
            ElseIf RuleID = ValidationRuleId.RECERT_SUMMARY_ARN.ToString() Then '' load summary form for ARN
                If Not CheckBusinessValidation_ARN(TotalPointARN, errorMessages) Then
                    issuesCollection.Add(New CESummaryIssue(New BusinessObject(), errorMessages))
                    hdIsIncomplete.Value = CommonConstants.TAB_INCOMPLETED
                Else
                    hdIsIncomplete.Value = CommonConstants.TAB_COMPLETED
                End If
            End If
            Return issuesCollection
        End Function

        Private Function CheckBusinessValidation(ByVal totalCE As Decimal, ByRef errorMessages As String) As Boolean
            Dim checkValidation = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                    ValidationRuleId.RECERT_SUMMARY_COMMON_CALCULATOR.ToString(),
                                                                    Server.MapPath(ParentModulePath))
            If checkValidation = False Then '' not require validation
                Return True
            End If
            Dim totalCEMin As Decimal = 0
            Dim totalCEMax As Decimal = 0
            ProgramType = ActivityProgramType.SUMMARY.ToString()
            Dim programTypeSetting = ModuleConfigurationHelper.Instance.GetProgramTypeSetting(ProgramType, Server.MapPath(ParentModulePath))
            If programTypeSetting IsNot Nothing Then
                Dim currentRecertOption = GetCurrentReCertOption()
                If currentRecertOption IsNot Nothing Then
                    If currentRecertOption.QuestionCode = Enums.QuestionCode.RECERT_OPTION_2.ToString() Then
                        totalCEMin = Decimal.Parse(programTypeSetting.MinCEOpt2.ToString())
                        totalCEMax = Decimal.Parse(programTypeSetting.MaxCEOpt2.ToString())
                        errorMessages = DotNetNuke.Services.Localization.Localization.GetString("ErrorMessagesSummaryOption2.Text", Me.LocalResourceFile)
                        errorMessages = String.Format(errorMessages, totalCEMin, totalCEMax)
                    ElseIf currentRecertOption.QuestionCode = Enums.QuestionCode.RECERT_OPTION_3.ToString() Then
                        totalCEMin = Decimal.Parse(programTypeSetting.MinCEOpt3.ToString())
                        totalCEMax = Decimal.Parse(programTypeSetting.MaxCEOpt3.ToString())
                        errorMessages = DotNetNuke.Services.Localization.Localization.GetString("ErrorMessagesSummaryOption3.Text", Me.LocalResourceFile)
                        errorMessages = String.Format(errorMessages, totalCEMin, totalCEMax)
                    End If
                    If totalCE < totalCEMin OrElse totalCE > totalCEMax Then
                        Return False
                    End If
                End If
            End If
            Return True
        End Function

        Private Sub BuildOptionTitle()
           Dim errorMessages As String = String.Empty
            CheckBusinessValidation(0, errorMessages)
            lblOptionTitle.Text = errorMessages
        End Sub

        Private Function GetContinuingEducationTitle() As String
            Dim message = String.Empty
            Dim totalCEMinOption2 As Decimal = 0
            Dim totalCEMinOption3 As Decimal = 0
            ProgramType = ActivityProgramType.CONTEDUCATION.ToString()
            Dim programTypeSetting = ModuleConfigurationHelper.Instance.GetProgramTypeSetting(ProgramType, Server.MapPath(ParentModulePath))
            If programTypeSetting IsNot Nothing Then
                Dim currentRecertOption = GetCurrentReCertOption()
                If currentRecertOption IsNot Nothing Then
                    If currentRecertOption.QuestionCode = Enums.QuestionCode.RECERT_OPTION_2.ToString() Then
                        totalCEMinOption2 = Decimal.Parse(programTypeSetting.MinCEOpt2.ToString())
                        message = DotNetNuke.Services.Localization.Localization.GetString("ContinuingEducationSummaryTitle.Text", Me.LocalResourceFile)
                        message = String.Format(message, totalCEMinOption2, "2")
                    ElseIf currentRecertOption.QuestionCode = Enums.QuestionCode.RECERT_OPTION_3.ToString() Then
                        totalCEMinOption3 = Decimal.Parse(programTypeSetting.MinCEOpt3.ToString())
                        message = DotNetNuke.Services.Localization.Localization.GetString("ContinuingEducationSummaryTitle.Text", Me.LocalResourceFile)
                        message = String.Format(message, totalCEMinOption3, "3")
                    End If
                End If
            End If
            Return message
        End Function

#End Region

#Region "ARN_Validation"

        Private Sub BindingDataToList_ARN(ByRef TotalPoint As Decimal)
            TotalPoint = 0
            Dim objectList As List(Of SummaryObject) = New List(Of SummaryObject)

            Dim arrProgramTypeEnum As New ArrayList
            For Each programtype As String In [Enum].GetNames(GetType(ActivityProgramType))
                If Not String.IsNullOrEmpty(programtype) Then
                    arrProgramTypeEnum.Add(programtype)
                End If
            Next

            Dim arr As List(Of SectionInfo) = If(CurrentFormInfo IsNot Nothing AndAlso CurrentFormInfo.Sections IsNot Nothing, CurrentFormInfo.Sections, New List(Of SectionInfo)())
            Dim cePointofForm As Decimal = 0
            Dim arrCommunityService As ArrayList = New ArrayList()
            For Each sectionInfo As SectionInfo In arr
                If CheckExistItem(sectionInfo.SectionProgramType, arrProgramTypeEnum) Then   '' only list programType in Enum
                    If sectionInfo.IsEnabled = True AndAlso Not String.IsNullOrEmpty(sectionInfo.SectionProgramType) Then  ''Tab must be Enable 
                        Select Case sectionInfo.SectionProgramType
                            Case ActivityProgramType.EDUCATION.ToString()
                                If CreateSummaryObject_ContinuingEducation(sectionInfo.SectionProgramType, 3, cePointofForm) IsNot Nothing Then
                                    objectList.Add(CreateSummaryObject_ContinuingEducation(sectionInfo.SectionProgramType, 3, cePointofForm))
                                    TotalPoint += cePointofForm
                                End If
                                Exit Select
                            Case ActivityProgramType.TEACHINGPRESN.ToString()
                                If CreateSummaryObject_TeachingPresentation(sectionInfo.SectionProgramType, cePointofForm) IsNot Nothing Then
                                    objectList.Add(CreateSummaryObject_TeachingPresentation(sectionInfo.SectionProgramType, cePointofForm))
                                    TotalPoint += cePointofForm
                                End If
                                Exit Select
                            Case ActivityProgramType.CONTEDUCATION.ToString()
                                If CreateSummaryObject_EducationCourse(sectionInfo.SectionProgramType, cePointofForm) IsNot Nothing Then
                                    objectList.Add(CreateSummaryObject_EducationCourse(sectionInfo.SectionProgramType, cePointofForm))
                                    TotalPoint += cePointofForm
                                End If
                                Exit Select
                            Case ActivityProgramType.PUBLICATION.ToString()
                                If CreateSummaryObject_Publication(sectionInfo.SectionProgramType, cePointofForm) IsNot Nothing Then
                                    objectList.Add(CreateSummaryObject_Publication(sectionInfo.SectionProgramType, cePointofForm))
                                    TotalPoint += cePointofForm
                                End If
                                Exit Select
                            Case ActivityProgramType.COMMSERVR.ToString()
                                arrCommunityService.Add(sectionInfo.SectionProgramType)
                                Exit Select
                            Case ActivityProgramType.COMMSERVVL.ToString()
                                arrCommunityService.Add(sectionInfo.SectionProgramType)
                                Exit Select
                            Case ActivityProgramType.COMMSERVVS.ToString()
                                arrCommunityService.Add(sectionInfo.SectionProgramType)
                                Exit Select
                            Case ActivityProgramType.COMMSERVPRES.ToString()
                                arrCommunityService.Add(sectionInfo.SectionProgramType)
                                Exit Select
                            Case ActivityProgramType.COMMSERVPUB.ToString()
                                arrCommunityService.Add(sectionInfo.SectionProgramType)
                                Exit Select
                            Case Else
                                If CreateSummaryObjectItem(sectionInfo.SectionProgramType, cePointofForm) IsNot Nothing Then
                                    objectList.Add(CreateSummaryObjectItem(sectionInfo.SectionProgramType, cePointofForm))
                                    TotalPoint += cePointofForm
                                End If
                                Exit Select
                        End Select
                    End If
                End If
            Next
            '' create communityService ( sum 5 forms)
            If arrCommunityService IsNot Nothing Then
                If arrCommunityService.Count > 0 Then
                    If CreateSummaryObject_CommunityService(arrCommunityService, cePointofForm) IsNot Nothing Then
                        objectList.Add(CreateSummaryObject_CommunityService(arrCommunityService, cePointofForm))
                        TotalPoint += cePointofForm
                        SetCookie("TotalCEpointCummunityService", cePointofForm.ToString()) '' set data to be check validation 
                    End If
                End If
            End If
            objectList.Sort(Function(x, y) String.Compare(x.CECategoryName, y.CECategoryName))
            Me.rptSummary.DataSource = objectList
            Me.rptSummary.DataBind()
            lblTotalCE.Text = CommonHelper.ChopUnusedDecimal(TotalPoint.ToString())
        End Sub

        Private Function CreateSummaryObjectItem(ByVal programType As String, ByRef cePointofForm As Decimal) As SummaryObject
            Dim objectItem As SummaryObject = New SummaryObject()
            cePointofForm = 0
            objectItem.CECategoryName = GetCategoryByNameByProgramTypeCode(programType)
            If objectItem.CECategoryName <> String.Empty Then '' have form must list out 
                '' set CEpoint
                objectItem.TotalCEString = CommonHelper.ChopUnusedDecimal(GetCookie(programType + MasterCustomerId))
                If CommonHelper.CheckIsNumber(objectItem.TotalCEString) Then
                    cePointofForm = Decimal.Parse(objectItem.TotalCEString)
                End If
            End If
            Return objectItem
        End Function

        Private Function CreateSummaryObject_ContinuingEducation(ByVal programType As String, ByVal columIndex As Integer,
                                                                 ByRef cePointofForm As Decimal) As SummaryObject

            Dim maxCEPoint As Decimal = 0
            cePointofForm = 0
            Dim otherModuleSettings = ModuleConfigurationHelper.Instance.GetOtherModuleSettings(Server.MapPath(ParentModulePath))
            If otherModuleSettings IsNot Nothing Then
                maxCEPoint = If(otherModuleSettings.ARNMaxSummaryPointOfContinuingEducation.HasValue, otherModuleSettings.ARNMaxSummaryPointOfContinuingEducation.Value, 0)
            End If
            cePointofForm = Decimal.Parse(GetCookie(programType + MasterCustomerId))
            Dim validationContinuingEducationARN = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                               ValidationRuleId.RECERT_CON_EDU_ARN.ToString(),
                                                               Server.MapPath(ParentModulePath))
            If validationContinuingEducationARN AndAlso cePointofForm > maxCEPoint Then
                cePointofForm = maxCEPoint
            End If

            Dim objectItem As SummaryObject = New SummaryObject()

            Dim formName = GetCategoryByNameByProgramTypeCode(programType)
            If formName <> String.Empty Then '' have form must list out 
                If columIndex = 3 Then '' total CE points
                    objectItem.CECategoryName = String.Format("{0} - {1}", formName, DotNetNuke.Services.Localization.Localization.GetString(
                                                                "SummaryContinuingEducationCol3.Text", Me.LocalResourceFile))
                    objectItem.TotalCEString = CommonHelper.ChopUnusedDecimal(cePointofForm.ToString())
                End If
            End If
            Return objectItem
        End Function

        Private Function CreateSummaryObject_TeachingPresentation(ByVal programType As String, ByRef cePointofForm As Decimal) As SummaryObject
            Dim objectItem As SummaryObject = New SummaryObject()
            cePointofForm = 0
            Dim formName = GetCategoryByNameByProgramTypeCode(programType)
            If formName <> String.Empty Then '' have form must list out 
                objectItem.CECategoryName = formName
                Dim validationARN_Greater = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                 ValidationRuleId.RECERT_TEACH_PRESENTATION_GREATER_ARN.ToString(),
                                                                 Server.MapPath(ParentModulePath))
                Dim validationARN_Equal = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                ValidationRuleId.RECERT_TEACH_PRESENTATION_EQUAL_ARN.ToString(),
                                                                Server.MapPath(ParentModulePath))
                Dim currentPoint As Decimal = 0
                Dim maxCEHours As Decimal = 0
                currentPoint = Decimal.Parse(GetCookie(programType + MasterCustomerId))
                Dim otherModuleSettings = ModuleConfigurationHelper.Instance.GetOtherModuleSettings(Server.MapPath(ParentModulePath))
                If otherModuleSettings IsNot Nothing Then
                    maxCEHours = If(otherModuleSettings.PresentationTotalHours.HasValue, otherModuleSettings.PresentationTotalHours.Value, 0)
                End If
                If (validationARN_Equal OrElse validationARN_Greater) AndAlso currentPoint > maxCEHours Then
                    objectItem.TotalCEString = CommonHelper.ChopUnusedDecimal(maxCEHours.ToString())
                Else
                    objectItem.TotalCEString = CommonHelper.ChopUnusedDecimal(currentPoint.ToString())
                End If
                If CommonHelper.CheckIsNumber(objectItem.TotalCEString) Then
                    cePointofForm = Decimal.Parse(objectItem.TotalCEString)
                End If
            End If
            Return objectItem
        End Function

        Private Function CreateSummaryObject_EducationCourse(ByVal programType As String, ByRef cePointofForm As Decimal) As SummaryObject
            Dim objectItem As SummaryObject = New SummaryObject()
            cePointofForm = 0
            Dim formName = GetCategoryByNameByProgramTypeCode(programType)
            If formName <> String.Empty Then '' have form must list out 
                objectItem.CECategoryName = formName
                Dim validationARN_Greater = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                 ValidationRuleId.RECERT_EDU_COURSE_GREATER_ARN.ToString(),
                                                                 Server.MapPath(ParentModulePath))
                Dim validationARN_Equal = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                 ValidationRuleId.RECERT_EDU_COURSE_EQUAL_ARN.ToString(),
                                                                 Server.MapPath(ParentModulePath))
                Dim currentPoint As Decimal = 0
                Dim maxCEHours As Decimal = 0
                currentPoint = Decimal.Parse(GetCookie(programType + MasterCustomerId))
                Dim otherModuleSettings = ModuleConfigurationHelper.Instance.GetOtherModuleSettings(Server.MapPath(ParentModulePath))
                If otherModuleSettings IsNot Nothing Then
                    maxCEHours = If(otherModuleSettings.EducationCourseTotalHours.HasValue, otherModuleSettings.EducationCourseTotalHours.Value, 0)
                End If
                If (validationARN_Equal OrElse validationARN_Greater) AndAlso currentPoint > maxCEHours Then
                    objectItem.TotalCEString = CommonHelper.ChopUnusedDecimal(maxCEHours.ToString())
                Else
                    objectItem.TotalCEString = CommonHelper.ChopUnusedDecimal(currentPoint.ToString())
                End If
                If CommonHelper.CheckIsNumber(objectItem.TotalCEString) Then
                    cePointofForm = Decimal.Parse(objectItem.TotalCEString)
                End If
            End If
            Return objectItem
        End Function

        Private Function CreateSummaryObject_Publication(ByVal programType As String, ByRef cePointofForm As Decimal) As SummaryObject
            Dim objectItem As SummaryObject = New SummaryObject()
            cePointofForm = 0
            Dim formName = GetCategoryByNameByProgramTypeCode(programType)
            If formName <> String.Empty Then '' have form must list out 
                objectItem.CECategoryName = formName
                Dim validationARN_Greater = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                 ValidationRuleId.RECERT_PUBLICATION_GREATER_ARN.ToString(),
                                                                 Server.MapPath(ParentModulePath))
                Dim validationARN_Equal = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                 ValidationRuleId.RECERT_PUBLICATION_EQUAL_ARN.ToString(),
                                                                 Server.MapPath(ParentModulePath))
                Dim currentPoint As Decimal = 0
                Dim maxCEHours As Decimal = 0
                currentPoint = Decimal.Parse(GetCookie(programType + MasterCustomerId))
                Dim otherModuleSettings = ModuleConfigurationHelper.Instance.GetOtherModuleSettings(Server.MapPath(ParentModulePath))
                If otherModuleSettings IsNot Nothing Then
                    maxCEHours = If(otherModuleSettings.PublicationTotalHours.HasValue, otherModuleSettings.PublicationTotalHours.Value, 0)
                End If
                If (validationARN_Equal OrElse validationARN_Greater) AndAlso currentPoint > maxCEHours Then
                    objectItem.TotalCEString = CommonHelper.ChopUnusedDecimal(maxCEHours.ToString())
                Else
                    objectItem.TotalCEString = CommonHelper.ChopUnusedDecimal(currentPoint.ToString())
                End If
                If CommonHelper.CheckIsNumber(objectItem.TotalCEString) Then
                    cePointofForm = Decimal.Parse(objectItem.TotalCEString)
                End If
            End If
            Return objectItem
        End Function

        Private Function SumCommunityService(ByVal arrProgramType As ArrayList) As Decimal
            Dim CEPoint As Decimal = 0
            For i As Integer = 0 To arrProgramType.Count - 1
                CEPoint += Decimal.Parse(GetCookie(arrProgramType(i).ToString() + MasterCustomerId))
            Next
            Return CEPoint
        End Function

        Private Function CreateSummaryObject_CommunityService(ByVal arrProgramType As ArrayList, ByRef cePointofForm As Decimal) As SummaryObject
            Dim objectItem As SummaryObject = New SummaryObject()
            cePointofForm = 0
            If arrProgramType IsNot Nothing Then '' have form must list out 
                If arrProgramType.Count > 0 Then
                    objectItem.CECategoryName = DotNetNuke.Services.Localization.Localization.GetString("CommunityServiceName.Text", Me.LocalResourceFile)
                    Dim validationARN_Greater = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                     ValidationRuleId.RECERT_COM_SERVICE_ARN.ToString(),
                                                                     Server.MapPath(ParentModulePath))
                    Dim currentPoint As Decimal = SumCommunityService(arrProgramType)
                    Dim maxCEHours As Decimal = 0
                    Dim otherModuleSettings = ModuleConfigurationHelper.Instance.GetOtherModuleSettings(Server.MapPath(ParentModulePath))
                    If otherModuleSettings IsNot Nothing Then
                        maxCEHours = If(otherModuleSettings.CommunityServiceTotalHours.HasValue, otherModuleSettings.CommunityServiceTotalHours.Value, 0)
                    End If
                    If validationARN_Greater = True AndAlso currentPoint > maxCEHours Then
                        objectItem.TotalCEString = CommonHelper.ChopUnusedDecimal(maxCEHours.ToString())
                    Else
                        objectItem.TotalCEString = CommonHelper.ChopUnusedDecimal(currentPoint.ToString())
                    End If
                    If CommonHelper.CheckIsNumber(objectItem.TotalCEString) Then
                        cePointofForm = Decimal.Parse(objectItem.TotalCEString)
                    End If
                End If
            End If
            Return objectItem
        End Function

        Private Sub BuildTitleTotalCEPoint_ARN()
            lblOptionTitle.Text = DotNetNuke.Services.Localization.Localization.GetString("SummaryTitleARN.Text", Me.LocalResourceFile)
        End Sub



#End Region

#Region "Validation"

        ''' <summary>
        ''' Check validation is conflicted
        ''' </summary>
        ''' <param name="ruleID"> get ruleID and check validation follow this ID </param>
        ''' <param name="conflictMessage"> if have conflict , message will be raised </param>
        ''' <returns> boolean value : have or none conflict  </returns>
        ''' <comment>base on conflictMessage, to distinguish conflict or not validation</comment>
        Public Function CheckConflictValidation(ByRef ruleID As String,
                                                ByRef conflictMessage As String) As Boolean
            Dim validationABNN = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                 ValidationRuleId.RECERT_SUMMARY_COMMON_CALCULATOR.ToString(),
                                                                 Server.MapPath(ParentModulePath))
            Dim validationARN = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                 ValidationRuleId.RECERT_SUMMARY_ARN.ToString(),
                                                                 Server.MapPath(ParentModulePath))

            conflictMessage = String.Empty
            ruleID = String.Empty
            If validationABNN = True AndAlso validationARN = True Then
                conflictMessage = DotNetNuke.Services.Localization.Localization.GetString("ConflictMessage.Text", Me.LocalResourceFile)
                Return True
            ElseIf validationABNN = True Then
                ruleID = ValidationRuleId.RECERT_SUMMARY_COMMON_CALCULATOR.ToString()
                Return False
            ElseIf validationARN = True Then
                ruleID = ValidationRuleId.RECERT_SUMMARY_ARN.ToString()
                Return False
            End If
            Return True '' don't have validation because conflictMessage is empty
        End Function

        Private Function CheckBusinessValidation_ARN(ByVal totalPoint As Decimal, ByRef errorMessages As String) As Boolean '' total CE point of 5 form must be equal to or greater than maximum CE

            Dim validationARN_Greater = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                ValidationRuleId.RECERT_COM_SERVICE_ARN.ToString(),
                                                                Server.MapPath(ParentModulePath))
            Dim validationSummaryARN_Equal = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                               ValidationRuleId.RECERT_SUMMARY_EQUAL_ARN.ToString(),
                                                               Server.MapPath(ParentModulePath))
            Dim validationSummaryARN_Greater = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                 ValidationRuleId.RECERT_SUMMARY_GREATER_ARN.ToString(),
                                                                 Server.MapPath(ParentModulePath))
            Dim validationContinuingEducationARN = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                ValidationRuleId.RECERT_CON_EDU_ARN.ToString(),
                                                                Server.MapPath(ParentModulePath))
            Dim validationResentationARN_Equal = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                 ValidationRuleId.RECERT_TEACH_PRESENTATION_EQUAL_ARN.ToString(),
                                                                 Server.MapPath(ParentModulePath))
            Dim validationResentationARN_Greater = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                 ValidationRuleId.RECERT_TEACH_PRESENTATION_GREATER_ARN.ToString(),
                                                                 Server.MapPath(ParentModulePath))
            Dim validationEducationCourseARN_Equal = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                ValidationRuleId.RECERT_EDU_COURSE_EQUAL_ARN.ToString(),
                                                                Server.MapPath(ParentModulePath))
            Dim validationEducationCourseARN_Greater = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                 ValidationRuleId.RECERT_EDU_COURSE_GREATER_ARN.ToString(),
                                                                 Server.MapPath(ParentModulePath))
            Dim validationPublicationARN_Equal = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                ValidationRuleId.RECERT_PUBLICATION_EQUAL_ARN.ToString(),
                                                                Server.MapPath(ParentModulePath))
            Dim validationPublicationARN_Greater = ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                                 ValidationRuleId.RECERT_PUBLICATION_GREATER_ARN.ToString(),
                                                                 Server.MapPath(ParentModulePath))

            errorMessages = String.Empty
            Dim currentPointFormE As Decimal = 0
            Dim maxPointFormE As Decimal = 0
            Dim maxTotalpoint As Decimal = 0
            Dim flag As Boolean = True
            currentPointFormE = Decimal.Parse(GetCookie("TotalCEpointCummunityService"))
            Dim otherModuleSettings = ModuleConfigurationHelper.Instance.GetOtherModuleSettings(Server.MapPath(ParentModulePath))
            If otherModuleSettings IsNot Nothing Then
                maxPointFormE = If(otherModuleSettings.CommunityServiceTotalHours.HasValue, otherModuleSettings.CommunityServiceTotalHours.Value, 0)
                maxTotalpoint = If(otherModuleSettings.ARNMaxSummaryPoint.HasValue, otherModuleSettings.ARNMaxSummaryPoint.Value, 0)
            End If

            '' check carryover point of form : E
            If Not ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(ValidationRuleId.COMMUNITY_SERVICE_VOLUNTEER_SERVICE_OPTIONAL.ToString(), Server.MapPath(ParentModulePath)) OrElse
               Not ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(ValidationRuleId.COMMUNITY_SERVICE_LEADERSHIP_OPTIONAL.ToString(), Server.MapPath(ParentModulePath)) OrElse
               Not ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(ValidationRuleId.COMMUNITY_SERVICE_PRESENTATION_OPTIONAL.ToString(), Server.MapPath(ParentModulePath)) OrElse
               Not ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(ValidationRuleId.COMMUNITY_SERVICE_PUBLICATION_OPTIONAL.ToString(), Server.MapPath(ParentModulePath)) OrElse
               Not ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(ValidationRuleId.COMMUNITY_SERVICE_REVIEW_OPTIONAL.ToString(), Server.MapPath(ParentModulePath)) Then '' if one of 5 forms uncheck optional rule => will check carry over rule on form
                If validationARN_Greater = True AndAlso currentPointFormE < maxPointFormE Then    '' 
                    errorMessages = String.Format(DotNetNuke.Services.Localization.Localization.GetString(
                                                  "ValidationErrorMessage_CommunityService_ARN.Text", Me.LocalResourceFile), maxPointFormE, currentPointFormE)
                    flag = False
                End If
            End If

            '' check carryover point of form : A,B,C,D
            Dim maxCEPointCarryOver As Decimal = 0
            Dim subMessageCarry = String.Empty
            If otherModuleSettings IsNot Nothing Then
                Dim arrProgramTypeEnum As New ArrayList
                For Each programtype As String In [Enum].GetNames(GetType(ActivityProgramType))
                    If Not String.IsNullOrEmpty(programtype) Then
                        arrProgramTypeEnum.Add(programtype)
                    End If
                Next
                Dim arr As List(Of SectionInfo) = If(CurrentFormInfo IsNot Nothing AndAlso CurrentFormInfo.Sections IsNot Nothing, CurrentFormInfo.Sections, New List(Of SectionInfo)())
                For Each sectionInfo As SectionInfo In arr
                    If CheckExistItem(sectionInfo.SectionProgramType, arrProgramTypeEnum) Then   '' only list programType in Enum
                        If sectionInfo.IsEnabled = True AndAlso Not String.IsNullOrEmpty(sectionInfo.SectionProgramType) Then  ''Tab must be Enable 
                            Select Case sectionInfo.SectionProgramType
                                Case ActivityProgramType.EDUCATION.ToString()
                                    If validationContinuingEducationARN Then
                                        maxCEPointCarryOver = If(otherModuleSettings.ARNMaxSummaryPointOfContinuingEducation.HasValue, otherModuleSettings.ARNMaxSummaryPointOfContinuingEducation.Value, 0)
                                        subMessageCarry = BuildErrorMessageForContiningEducation(sectionInfo.SectionProgramType, sectionInfo.SectionValue, maxCEPointCarryOver)
                                        If Not String.IsNullOrEmpty(subMessageCarry) Then
                                            errorMessages += subMessageCarry
                                            flag = False
                                        End If
                                    End If
                                    Exit Select
                                Case ActivityProgramType.TEACHINGPRESN.ToString()
                                    If Not ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(ValidationRuleId.PRESENTATIONS_OPTIONAL.ToString(), Server.MapPath(ParentModulePath)) Then '' not check optional rule => check rele on form
                                        If validationResentationARN_Equal OrElse validationResentationARN_Greater Then
                                            maxCEPointCarryOver = If(otherModuleSettings.PresentationTotalHours.HasValue, otherModuleSettings.PresentationTotalHours.Value, 0)
                                            subMessageCarry = BuildErrorMessageValidationRuleCarryOverPoint(sectionInfo.SectionProgramType, maxCEPointCarryOver, sectionInfo.SectionValue)
                                            If Not String.IsNullOrEmpty(subMessageCarry) Then
                                                errorMessages += subMessageCarry
                                                flag = False
                                            End If
                                        End If
                                    End If
                                    Exit Select
                                Case ActivityProgramType.CONTEDUCATION.ToString()
                                    If Not ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(ValidationRuleId.ACADEMIC_COURSEWORK_OPTIONAL.ToString(), Server.MapPath(ParentModulePath)) Then '' not check optional rule => check rele on form
                                        If validationEducationCourseARN_Equal OrElse validationEducationCourseARN_Greater Then
                                            maxCEPointCarryOver = If(otherModuleSettings.EducationCourseTotalHours.HasValue, otherModuleSettings.EducationCourseTotalHours.Value, 0)
                                            subMessageCarry = BuildErrorMessageValidationRuleCarryOverPoint(sectionInfo.SectionProgramType, maxCEPointCarryOver, sectionInfo.SectionValue)
                                            If Not String.IsNullOrEmpty(subMessageCarry) Then
                                                errorMessages += subMessageCarry
                                                flag = False
                                            End If
                                        End If
                                    End If
                                    Exit Select
                                Case ActivityProgramType.PUBLICATION.ToString()
                                    If Not ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(ValidationRuleId.PROFESSIONAL_PUBLICATIONS_OPTIONAL.ToString(), Server.MapPath(ParentModulePath)) Then '' not check optional rule => check rele on form
                                        If validationPublicationARN_Equal OrElse validationPublicationARN_Greater Then
                                            maxCEPointCarryOver = If(otherModuleSettings.PublicationTotalHours.HasValue, otherModuleSettings.PublicationTotalHours.Value, 0)
                                            subMessageCarry = BuildErrorMessageValidationRuleCarryOverPoint(sectionInfo.SectionProgramType, maxCEPointCarryOver, sectionInfo.SectionValue)
                                            If Not String.IsNullOrEmpty(subMessageCarry) Then
                                                errorMessages += subMessageCarry
                                                flag = False
                                            End If
                                        End If
                                    End If
                                    Exit Select
                            End Select
                        End If
                    End If
                Next

            End If

            '' check max values of Summary form
            If validationSummaryARN_Greater = True Then    '' 
                If totalPoint < maxTotalpoint Then
                    errorMessages += String.Format("<br/>" + DotNetNuke.Services.Localization.Localization.GetString(
                                              "ValidationErrorMessageARN_greater.Text", Me.LocalResourceFile), CommonHelper.ChopUnusedDecimal(maxTotalpoint.ToString()))
                    flag = False
                End If
            Else
                If validationSummaryARN_Equal = True Then    '' 
                    If totalPoint <> maxTotalpoint Then
                        errorMessages += String.Format("<br/>" + DotNetNuke.Services.Localization.Localization.GetString(
                                                 "ValidationErrorMessageARN_equal.Text", Me.LocalResourceFile), CommonHelper.ChopUnusedDecimal(maxTotalpoint.ToString()))
                        flag = False
                    End If
                End If
            End If
            Return flag
        End Function

        Private Function BuildErrorMessageValidationRuleCarryOverPoint(ByVal programType As String, ByVal maxCEHours As Decimal,
                                                                       ByVal formName As String) As String
            Dim currentPoint As Decimal = 0
            Dim message = String.Empty
            currentPoint = Decimal.Parse(GetCookie(programType + MasterCustomerId))
            If currentPoint < maxCEHours Then
                message = String.Format("<br/>" + DotNetNuke.Services.Localization.Localization.GetString(
                                                 "ErrorMessageCarryOverPointOnSummary.Text", Me.LocalResourceFile),
                                                    formName, CommonHelper.ChopUnusedDecimal(maxCEHours.ToString()), CommonHelper.ChopUnusedDecimal(currentPoint.ToString()))
            End If
            Return message
        End Function

        Private Function BuildErrorMessageForContiningEducation(ByVal programType As String, ByVal formName As String,
                                                                ByVal maxCEHours As Decimal) As String
            Dim pointofForm As Decimal = -1
            Dim pointApprove As Decimal = -1
            Dim subMessages = String.Empty
            If Decimal.Parse(GetCookie(programType + MasterCustomerId)) <> 0 Then
                pointofForm = Decimal.Parse(GetCookie(programType + MasterCustomerId))
            End If
            If pointofForm > maxCEHours Then
                pointofForm = maxCEHours
            End If
            If Decimal.Parse(GetCookie("approvedHour")) <> 0 Then
                pointApprove = Decimal.Parse(GetCookie("approvedHour"))
            End If
            Dim leftParam = Math.Ceiling((pointofForm * 2) / 3)
            If ModuleConfigurationHelper.Instance.IsValidationRuleEnabled(
                                                        ValidationRuleId.CONTINUING_EDUCATION_OPTIONAL.ToString(),
                                                        Server.MapPath(ParentModulePath)) Then 'Bypass

                If (pointApprove > -1 OrElse pointofForm > 0) AndAlso pointApprove < leftParam Then 'has invalid data
                    subMessages = "<br/>" +
                          String.Format(
                              Localization.GetString("MessagesContinuingEducationARNWithForm.Text",
                                                     LocalResourceFile), formName)
                End If

            Else 'Not Bypass
                'no data or (has invalid data)
                If (pointApprove = -1 AndAlso pointofForm = -1) OrElse ((pointApprove > -1 OrElse pointofForm > 0) AndAlso pointApprove < leftParam) Then
                    subMessages = "<br/>" +
                                  String.Format(
                                      Localization.GetString("MessagesContinuingEducationARNWithForm.Text",
                                                             LocalResourceFile), formName)
                End If
            End If
            Return subMessages
        End Function

#End Region

    End Class
End Namespace

Public Class SummaryObject
    Public Property TotalCEString() As String
    Public Property CECategoryName() As String
End Class
