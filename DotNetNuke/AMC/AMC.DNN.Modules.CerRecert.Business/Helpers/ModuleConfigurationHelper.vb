Imports AMC.DNN.Modules.CertRecert.Business.Entities
Imports AMC.DNN.Modules.CertRecert.Data.Entities
Imports AMC.DNN.Modules.CertRecert.Business.EnumItems

Imports System.Xml
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.IO
Imports System.Linq
Imports System.Xml.Serialization
Imports System.Text
Imports AMC.DNN.Modules.CertRecert.Business.Controller
Imports System.Reflection
Imports TIMSS.Enumerations
Imports TIMSS.API.CertificationInfo
Imports TIMSS.API.User.CertificationInfo

Imports DotNetNuke.Entities.Modules

Namespace Helpers
    Public Class ModuleConfigurationHelper

#Region "Singleton"

        ''' <summary>
        ''' Gets the instance.
        ''' </summary>
        ''' 
        Public Shared ReadOnly Instance As New ModuleConfigurationHelper

        Private Sub New()

        End Sub

#End Region

#Region "Properties"

        ''' <summary>
        ''' Gets or sets the module settings.
        ''' </summary>
        ''' <value>
        ''' The module settings.
        ''' </value>
        Public Shared Property ModuleSettings() As Hashtable
            Get
                Return _moduleSettings
            End Get
            Set(ByVal value As Hashtable)
                _moduleSettings = value
            End Set
        End Property

        Private Shared _moduleSettings As Hashtable

#End Region

#Region "Reset Settings"

        ''' <summary>
        ''' Resets the specified module id.
        ''' </summary>
        ''' <param name="moduleId">The module id.</param>
        Public Sub Reset(ByVal moduleId As Integer)
            Dim objModules As New ModuleController
            objModules.DeleteModuleSettings(moduleId)
        End Sub

#End Region

#Region "Update Module Settings"

        ''' <summary>
        ''' Updates the name of the field.
        ''' </summary>
        ''' <param name="formId">The form id.</param>
        ''' <param name="sectionId">The section id.</param>
        ''' <param name="fieldId">The field id.</param>
        ''' <param name="organizationId">The organization id.</param>
        ''' <param name="organizationUnitId">The organization unit id.</param>
        ''' <param name="moduleId">The module id.</param>
        ''' <param name="modulePath">The module path.</param>
        ''' <param name="newName">The new name.</param>
        Public Function UpdateFieldName(ByVal formId As String, ByVal sectionId As String, ByVal fieldId As String,
                                         ByVal organizationId As String, ByVal organizationUnitId As String,
                                         ByVal moduleId As Integer,
                                         ByVal modulePath As String, ByVal newName As String) As Boolean
            Dim xPath = String.Format("//FormId[text()='{0}']/..//SectionId[text()='{1}']/..//FieldId[text()='{2}']/..",
                                       formId, sectionId, fieldId)
            Dim result = UpdateXMLConfigurationNode(
                Path.Combine(modulePath, CommonConstants.FORM_CONFIGURATIONS_FILE_PATH), xPath, "FieldValue", newName)
            Return result
        End Function


        ''' <summary>
        ''' Updates the name of the section.
        ''' </summary>
        ''' <param name="formId">The form id.</param>
        ''' <param name="sectionId">The section id.</param>
        ''' <param name="organizationId">The organization id.</param>
        ''' <param name="organizationUnitId">The organization unit id.</param>
        ''' <param name="moduleId">The module id.</param>
        ''' <param name="modulePath">The module path.</param>
        ''' <param name="newName">The new name.</param>
        Public Function UpdateSectionName(ByVal formId As String, ByVal sectionId As String,
                                           ByVal organizationId As String,
                                           ByVal organizationUnitId As String, ByVal moduleId As Integer,
                                           ByVal modulePath As String, ByVal newName As String) As Boolean
            Dim xPath = String.Format("//FormId[text()='{0}']/..//SectionId[text()='{1}']/..",
                                       formId, sectionId)
            Dim result = UpdateXMLConfigurationNode(
                Path.Combine(modulePath, CommonConstants.FORM_CONFIGURATIONS_FILE_PATH), xPath, "SectionValue", newName)
            Return result
        End Function

        ''' <summary>
        ''' Updates the name of the form.
        ''' </summary>
        ''' <param name="formId">The form id.</param>
        ''' <param name="organizationId">The organization id.</param>
        ''' <param name="organizationUnitId">The organization unit id.</param>
        ''' <param name="moduleId">The module id.</param>
        ''' <param name="modulePath">The module path.</param>
        ''' <param name="newName">The new name.</param>
        Public Function UpdateFormName(ByVal formId As String, ByVal organizationId As String,
                                        ByVal organizationUnitId As String,
                                        ByVal moduleId As Integer, ByVal modulePath As String, ByVal newName As String) _
            As Boolean
            Dim xPath = String.Format("//FormId[text()='{0}']/..",
                                       formId)
            Dim result = UpdateXMLConfigurationNode(
                Path.Combine(modulePath, CommonConstants.FORM_CONFIGURATIONS_FILE_PATH), xPath, "FormValue", newName)
            Return result
        End Function

        ''' <summary>
        ''' Updates the XML configuration.
        ''' </summary>
        ''' <param name="xmlFilePath">The XML file path.</param>
        ''' <param name="xPath">The x path.</param>
        ''' <param name="nodeName">Name of the node.</param>
        ''' <param name="newValue">The new value.</param>
        Public Function UpdateXMLConfigurationNode(ByVal xmlFilePath As String, ByVal xPath As String,
                                                    ByVal nodeName As String, ByVal newValue As Object) As Boolean
            Dim xmlDocument = New XmlDocument()
            xmlDocument.Load(xmlFilePath)
            Dim xnList = xmlDocument.SelectSingleNode(xPath)
            If xnList IsNot Nothing Then
                Dim valueNode = xnList.SelectSingleNode(nodeName)
                If valueNode IsNot Nothing Then
                    valueNode.InnerText = newValue.ToString()
                    xmlDocument.Save(xmlFilePath)
                    Return True
                End If
            End If
            Return False
        End Function

        ''' <summary>
        ''' Updates the configurations.
        ''' </summary>
        ''' <param name="formInfos">The form infos.</param>
        ''' <param name="moduleId">The module id.</param>
        ''' <param name="modulePath">The module path.</param>
        Public Sub UpdateConfigurations(ByVal formInfos As List(Of FormInfo), ByVal moduleId As Integer,
                                         ByVal modulePath As String)
            Dim path = IO.Path.Combine(modulePath, CommonConstants.FORM_CONFIGURATIONS_FILE_PATH)
            CommonHelper.Instance.WriteXML(path, SerializeObject(Of List(Of FormInfo))(formInfos))
        End Sub

#End Region

#Region "Gets Form Configurations"

        ''' <summary>
        ''' Gets the form configurations.
        ''' </summary>
        ''' <param name="moduleId">The module id.</param>
        ''' <param name="modulePath">The module path.</param>
        ''' <param name="orgId">The org id.</param>
        ''' <param name="orgUnitId">The org unit id.</param>
        ''' <returns></returns>
        Public Function GetFormConfigurations(ByVal moduleId As Integer, ByVal modulePath As String,
                                               ByVal orgId As String, ByVal orgUnitId As String) As List(Of FormInfo)
            Dim strReader = File.ReadAllText(Path.Combine(modulePath, CommonConstants.FORM_CONFIGURATIONS_FILE_PATH))
            If Not String.IsNullOrEmpty(strReader) Then
                Dim formInfos = DeserializeXML(Of List(Of FormInfo))(strReader)
                Return formInfos
            End If
            Return Nothing
        End Function

        ''' <summary>
        ''' Gets the form configurations.
        ''' </summary>
        ''' <param name="moduleId">The module id.</param>
        ''' <param name="modulePath">The module path.</param>
        ''' <param name="orgId">The org id.</param>
        ''' <param name="orgUnitId">The org unit id.</param>
        ''' <param name="formID">The form ID.</param>
        ''' <returns></returns>
        Public Function GetFormConfiguration(ByVal moduleId As Integer, ByVal modulePath As String,
                                              ByVal orgId As String, ByVal orgUnitId As String, ByVal formID As String) _
            As FormInfo
            Dim formInfos = GetFormConfigurations(moduleId, modulePath, orgId, orgUnitId)
            Return _
                If _
                    (formInfos IsNot Nothing,
                     formInfos.FirstOrDefault(Function(x) InlineAssignHelper(x.FormId, formID)), Nothing)
        End Function

        ''' <summary>
        ''' Inlines the assign helper.
        ''' </summary>
        ''' <returns></returns>
        Private Shared Function InlineAssignHelper(ByVal senderId As String, ByVal requestId As String) As Boolean
            Return senderId = requestId
        End Function

#End Region

#Region "Question Item"

        ''' <summary>
        ''' Deletes the question item.
        ''' </summary>
        ''' <param name="formId">The form id.</param>
        ''' <param name="sectionId">The section id.</param>
        ''' <param name="fieldId">The field id.</param>
        ''' <param name="questionId">The question id.</param>
        ''' <param name="modulePath">The module path.</param>
        ''' <param name="moduleId">The module id.</param>
        ''' <param name="organizationId">The organization id.</param>
        ''' <param name="organizationUnitId">The organization unit id.</param>
        Public Sub DeleteQuestionItem(ByVal formId As String, ByVal sectionId As String, ByVal fieldId As String,
                                       ByVal questionId As String, ByVal modulePath As String, ByVal moduleId As Integer,
                                       ByVal organizationId As String, ByVal organizationUnitId As String)
            Dim xPath =
                    String.Format(
                        "//FormId[text()='{0}']/..//SectionId[text()='{1}']/..//FieldId[text()='{2}']/..//QuestionId[text()='{3}']/..",
                        formId, sectionId, fieldId, questionId)
            Dim xmlFilePath = Path.Combine(modulePath, CommonConstants.FORM_CONFIGURATIONS_FILE_PATH)
            Dim xmlDocument = New XmlDocument()
            xmlDocument.Load(xmlFilePath)
            Dim xnList = xmlDocument.SelectSingleNode(xPath)
            If xnList IsNot Nothing Then
                xnList.ParentNode.RemoveChild(xnList)
                xmlDocument.Save(xmlFilePath)
            End If
        End Sub

        ''' <summary>
        ''' Updates the question item.
        ''' </summary>
        ''' <param name="formId">The form id.</param>
        ''' <param name="sectionId">The section id.</param>
        ''' <param name="fieldId">The field id.</param>
        ''' <param name="questionId">The question id.</param>
        ''' <param name="modulePath">The module path.</param>
        ''' <param name="organizationId">The organization id.</param>
        ''' <param name="organizationUnitId">The organization unit id.</param>
        ''' <param name="nodeValue">The node value.</param>
        ''' <param name="isVisible">if set to <c>true</c> [is visible].</param>
        ''' <returns></returns>
        Public Function UpdateQuestionItem(ByVal formId As String, ByVal sectionId As String, ByVal fieldId As String,
                                            ByVal questionId As String, ByVal modulePath As String,
                                            ByVal organizationId As String,
                                            ByVal organizationUnitId As String, ByVal nodeValue As String,
                                            ByVal isVisible As Object) As Boolean
            Dim xPath =
                    String.Format(
                        "//FormId[text()='{0}']/..//SectionId[text()='{1}']/..//FieldId[text()='{2}']/..//QuestionId[text()='{3}']/..",
                        formId, sectionId, fieldId, questionId)
            Return _
                UpdateXMLConfigurationNode(Path.Combine(modulePath, CommonConstants.FORM_CONFIGURATIONS_FILE_PATH),
                                            xPath, nodeValue, isVisible)
        End Function

        ''' <summary>
        ''' Adds the question item.
        ''' </summary>
        ''' <param name="formId">The form id.</param>
        ''' <param name="sectionId">The section id.</param>
        ''' <param name="fieldId">The field id.</param>
        ''' <param name="modulePath">The module path.</param>
        ''' <param name="organizationId">The organization id.</param>
        ''' <param name="organizationUnitId">The organization unit id.</param>
        ''' <returns></returns>
        Public Function AddQuestionItem(ByVal formId As String, ByVal sectionId As String, ByVal fieldId As String,
                                         ByVal modulePath As String, ByVal organizationId As String,
                                         ByVal organizationUnitId As String, ByVal newId As String,
                                         Optional ByVal newText As String = "") As Boolean
            Dim xmlFilePath = Path.Combine(modulePath, CommonConstants.FORM_CONFIGURATIONS_FILE_PATH)
            Dim xmlDocument = New XmlDocument()
            xmlDocument.Load(xmlFilePath)
            Dim node = GetQuestionNode(formId, sectionId, fieldId, newId, newText)
            Dim xPath =
                    String.Format(
                        "//FormId[text()='{0}']/..//SectionId[text()='{1}']/..//FieldId[text()='{2}']/../QuestionList",
                        formId, sectionId, fieldId)
            Dim xnList = xmlDocument.SelectSingleNode(xPath)
            Dim importNode = xmlDocument.ImportNode(node, True)

            If xnList IsNot Nothing Then
                xnList.AppendChild(importNode)
                xmlDocument.Save(xmlFilePath)
                Return True
            Else
                xPath = String.Format("//FormId[text()='{0}']/..//SectionId[text()='{1}']/..//FieldId[text()='{2}']/..",
                                       formId, sectionId, fieldId)
                xnList = xmlDocument.SelectSingleNode(xPath)
                If xnList IsNot Nothing Then
                    Dim xmlNode = xmlDocument.CreateNode(XmlNodeType.Element, "QuestionList", String.Empty)
                    xmlNode.AppendChild(importNode)
                    xnList.AppendChild(xmlNode)
                    xmlDocument.Save(xmlFilePath)
                    Return True
                End If
            End If
            Return False
        End Function

        ''' <summary>
        ''' Gets the question node.
        ''' </summary>
        ''' <param name="formId">The form id.</param>
        ''' <param name="sectionId">The section id.</param>
        ''' <param name="fieldId">The field id.</param>
        ''' <param name="questionId">The question id.</param>
        ''' <returns></returns>
        Private Shared Function GetQuestionNode(ByVal formId As String, ByVal sectionId As String,
                                                 ByVal fieldId As String,
                                                 ByVal questionId As String, Optional ByVal newText As String = "") _
            As XmlNode
            Dim questionList = New List(Of QuestionInfo)
            Dim questionInfo = New QuestionInfo()
            questionInfo.FormId = formId
            questionInfo.SectionId = sectionId
            questionInfo.FieldId = fieldId
            questionInfo.QuestionId = questionId
            questionInfo.IsVisible = True
            questionInfo.QuestionText = newText
            questionList.Add(questionInfo)
            Dim strQuestionInfo = SerializeObject(Of List(Of QuestionInfo))(questionList)
            Dim nodeDocument = New XmlDocument()
            nodeDocument.LoadXml(strQuestionInfo)
            Return nodeDocument.SelectSingleNode("//QuestionInfo")
        End Function

#End Region

#Region "GUI"

        ''' <summary>
        ''' Applies the configurations.
        ''' </summary>
        ''' <param name="moduleId">The module id.</param>
        ''' <param name="formID">The form ID.</param>
        ''' <param name="container">The container.</param>
        Public Sub ApplyConfigurations(ByVal moduleId As Integer, ByVal modulePath As String, ByVal orgId As String,
                                        ByVal orgUnitId As String, ByVal formID As String,
                                        ByVal container As PlaceHolder)
            Dim configurations = GetFormConfiguration(moduleId, modulePath, orgId, orgUnitId, formID)
            If configurations IsNot Nothing AndAlso configurations.Sections IsNot Nothing Then
                For Each section As SectionInfo In configurations.Sections
                    Dim sectionControl = If(container.Controls IsNot Nothing AndAlso container.Controls.Count > 0,
                                            container.Controls(0).FindControl(section.SectionId), Nothing)
                    If sectionControl IsNot Nothing Then
                        ApplySectionConfigurations(section, sectionControl)
                    Else 'Form only have fields but not section
                        Dim headerControl = If(container.Controls IsNot Nothing AndAlso container.Controls.Count > 0,
                                               container.Controls(0).FindControl(String.Format("lbl{0}",
                                                                                                  configurations.FormId)),
                                               Nothing)
                        If headerControl IsNot Nothing AndAlso TypeOf headerControl Is Label Then
                            CType(headerControl, Label).Text = configurations.FormValue
                        End If
                        sectionControl = If(container.Controls IsNot Nothing AndAlso container.Controls.Count > 0,
                                            container.Controls(0),
                                            Nothing)
                        If _
                            section IsNot Nothing AndAlso section.Fields IsNot Nothing AndAlso
                            sectionControl IsNot Nothing Then
                            For Each field As Entities.FieldInfo In section.Fields
                                ApplyFieldConfiguration(sectionControl.FindControl(field.FieldId), field)
                            Next
                        End If
                    End If

                Next
            End If
        End Sub

        ''' <summary>
        ''' Applies the section configurations.
        ''' </summary>
        ''' <param name="section">The section.</param>
        ''' <param name="sectionControl">The section control.</param>
        Public Sub ApplySectionConfigurations(ByVal section As SectionInfo, ByVal sectionControl As Control)
            If Not String.IsNullOrEmpty(section.SectionId) Then
                If section.IsEnabled Then
                    '    sectionControl.Parent.Controls.Remove(sectionControl)
                    'Else
                    Dim headerControl = GetControl(sectionControl, section, String.Format("lbl{0}", section.SectionId))
                    'sectionControl.FindControl(String.Format("lbl{0}", section.SectionId))
                    If headerControl IsNot Nothing AndAlso TypeOf headerControl Is Label Then
                        CType(headerControl, Label).Text = section.SectionValue
                    End If
                    If section IsNot Nothing AndAlso section.Fields IsNot Nothing Then
                        For Each field As Entities.FieldInfo In section.Fields
                            ApplyFieldConfiguration(GetControl(sectionControl, section, field.FieldId), field)
                        Next
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Applies the field configuration.
        ''' </summary>
        ''' <param name="fieldControl">The field control.</param>
        ''' <param name="field">The field.</param>
        Public Sub ApplyFieldConfiguration(ByVal fieldControl As Control, ByVal field As Entities.FieldInfo)
            If fieldControl IsNot Nothing Then
                If Not field.IsEnabled Then
                    fieldControl.Visible = field.IsEnabled
                Else
                    Dim prefix = GetFieldIdPrefix(field.FieldTextType)
                    Dim fieldTextControl = fieldControl.FindControl(String.Format("{0}{1}", prefix, field.FieldId))
                    If fieldTextControl IsNot Nothing Then
                        Dim fieldType = fieldTextControl.GetType()
                        If fieldType IsNot Nothing Then
                            Dim textProperty = fieldType.GetProperty("Text")
                            If textProperty IsNot Nothing Then
                                textProperty.SetValue(fieldTextControl, field.FieldValue, Nothing)
                            End If
                            If field.IsRequired AndAlso Not field.IsReadOnly AndAlso fieldTextControl IsNot Nothing AndAlso TypeOf fieldTextControl Is Label Then
                                CType(fieldTextControl, Label).Text += "<span style=""color: red; padding-left:5px;"">(*)</span>"
                            End If
                        End If
                    End If
                    Dim validator = fieldControl.FindControl(String.Format("rq{0}", field.FieldId))
                    If validator IsNot Nothing AndAlso (TypeOf validator Is RequiredFieldValidator OrElse TypeOf validator Is CustomValidator) Then
                        If Not field.IsRequired Then
                            validator.Parent.Controls.Remove(validator)
                        End If
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Gets the field id prefix.
        ''' </summary>
        ''' <param name="fieldType">Type of the field.</param>
        ''' <returns></returns>
        Public Function GetFieldIdPrefix(ByVal fieldType As String) As String
            Select Case fieldType
                Case CommonConstants.CONFIGURATION_LABEL_TYPE
                    Return CommonConstants.CONFIGURATION_LABEL_TYPE_PREFIX
                Case CommonConstants.CONFIGURATION_HYPERLINK_TYPE
                    Return CommonConstants.CONFIGURATION_HYPERLINK_TYPE_PREFIX
                Case CommonConstants.CONFIGURATION_BUTTON_TYPE
                    Return CommonConstants.CONFIGURATION_BUTTON_TYPE_PREFIX
                Case CommonConstants.CONFIGURATION_LINK_BUTTON_TYPE
                    Return CommonConstants.CONFIGURATION_LINK_BUTTON_TYPE_PREFIX
                Case Else
                    Return CommonConstants.CONFIGURATION_LABEL_TYPE_PREFIX
            End Select
        End Function

        ''' <summary>
        ''' Gets the control.
        ''' </summary>
        ''' <param name="sectionControl">The section control.</param>
        ''' <param name="sectionInfo">The section info.</param>
        ''' <param name="controlId">The control id.</param>
        ''' <returns></returns>
        Private Shared Function GetControl(ByVal sectionControl As Control, ByVal sectionInfo As SectionInfo,
                                            ByVal controlId As String) As Control
            Dim result = sectionControl.FindControl(controlId)
            If result Is Nothing Then
                Dim section = sectionControl.FindControl(String.Format("section{0}", sectionInfo.SectionId))
                If section IsNot Nothing Then
                    result = section.FindControl(controlId)
                End If
            End If
            Return result
        End Function

#End Region

#Region "Serialization"

        ''' <summary>
        ''' Serializes the object.
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="input">The input object.</param>
        ''' <returns></returns>
        Public Shared Function SerializeObject(Of T)(ByVal input As T) As String
            Try
                Dim confString As String
                Using stream = New MemoryStream()
                    Dim serializer = New XmlSerializer(GetType(T))
                    serializer.Serialize(stream, input)
                    stream.Seek(0, SeekOrigin.Begin)
                    Using reader = New StreamReader(stream)
                        confString = reader.ReadToEnd()
                    End Using
                End Using
                Return confString
            Catch ex As Exception
                Return String.Empty
            End Try
        End Function

        ''' <summary>
        ''' Deserializes the XML.
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="xml">The XML.</param>
        ''' <returns></returns>
        Public Shared Function DeserializeXML(Of T)(ByVal xml As String) As T
            Try
                Dim serializer = New XmlSerializer(GetType(T))
                Dim result As T = Nothing
                Using stream = New MemoryStream(xml.Length)
                    If stream IsNot Nothing Then
                        Dim bytes = Encoding.UTF8.GetBytes(xml)
                        stream.Write(bytes, 0, bytes.Length)
                        stream.Seek(0, SeekOrigin.Begin)
                        result = DirectCast(serializer.Deserialize(stream), T)
                    End If
                End Using
                Return result
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

#End Region

#Region "Notification Settings"

        ''' <summary>
        ''' Gets the notification settings.
        ''' </summary>
        ''' <param name="modulePath">The module path.</param>
        ''' <returns></returns>
        Public Function GetNotificationSettings(ByVal modulePath As String) As List(Of NotificationInfo)
            Return _
                GetSettings(Of List(Of NotificationInfo))(modulePath, CommonConstants.NOTIFICATION_SETTINGS_FILE_PATH)
        End Function

#End Region

#Region "Certification Settings"

        ''' <summary>
        ''' Gets the notification settings.
        ''' </summary>
        ''' <param name="modulePath">The module path.</param>
        ''' <returns></returns>
        Public Function GetCertificationSettings(ByVal modulePath As String) As CertificationAuditSetting
            Return _
                GetSettings(Of CertificationAuditSetting)(modulePath, CommonConstants.CERT_AUDIT_SETTINGS_FILE_PATH)
        End Function

#End Region

#Region "Recertification Settings"

        Public Function GetRecertificationSettings(ByVal modulePath As String) As ReCertificationAuditSetting
            Return _
                GetSettings(Of ReCertificationAuditSetting)(modulePath,
                                                              CommonConstants.RECERT_AUDIT_SETTINGS_FILE_PATH)
        End Function

#End Region

#Region "Other Module Settings"

        Public Function GetGuidelineSettings(ByVal modulePath As String) As List(Of AMCDescription)
            Return GetSettings(Of List(Of AMCDescription))(modulePath, CommonConstants.GUIDELINE_SETTING_FILE_PATH)
        End Function

        ''' <summary>
        ''' Gets the certification code.
        ''' </summary>
        ''' <param name="modulePath">The module path.</param>
        ''' <returns></returns>
        Public Function GetOtherModuleSettings(ByVal modulePath As String) As OtherModuleSettings
            Return GetSettings(Of OtherModuleSettings)(modulePath, CommonConstants.OTHER_MODULE_SETTING_FILE_PATH)
        End Function

        Public Function GetRecertificationCyle(ByVal modulePath As String, ByVal amcCertRecertController As AmcCertRecertController,
                                               ByVal cusCertification As ICertificationCustomerCertification) As RecertificationCircleValidation
            Dim othermoduleSettings = GetOtherModuleSettings(modulePath)
            Dim origCert = amcCertRecertController.GetCertificationCustomerCertificationByCertId(cusCertification.OrigCertificationId)
            Dim result = New RecertificationCircleValidation()
            'Get Previous Cert/Recert Record
            If origCert IsNot Nothing Then
                result.ExpirationDate = origCert.CertificationExpirationDate
                If othermoduleSettings IsNot Nothing AndAlso othermoduleSettings.ReCertificationCycle.HasValue Then
                    result.ValidilityMonths = othermoduleSettings.ReCertificationCycle.Value
                    Return result
                Else
                    'Get Certifications from CRT_CERTIFICATION
                    Dim certifications = TIMSS.Global.GetCollection(amcCertRecertController.OrganizationId, amcCertRecertController.OrganizationUnitId, NamespaceEnum.CertificationInfo, "Certifications")
                    With certifications.Filter
                        .Add("CertificationCode", cusCertification.CertificationCodeString)
                        .Add("CertificationTypeCode", cusCertification.CertificationTypeCodeString)
                    End With
                    certifications.Fill()
                    If certifications IsNot Nothing AndAlso certifications.Count > 0 Then
                        result.ValidilityMonths = CType(certifications(0), Certification).ExpirationOptionMonths
                        Return result
                    End If
                End If
            End If
            Return Nothing
        End Function

#End Region

#Region "Public Methods"

        Public Function GetSettings(Of T)(ByVal modulePath As String, ByVal filePath As String) As T
            Dim strReader = File.ReadAllText(Path.Combine(modulePath, filePath))
            If Not String.IsNullOrEmpty(strReader) Then
                Dim settings = DeserializeXML(Of T)(strReader)
                Return settings
            End If
            Return Nothing
        End Function

        ''' <summary>
        ''' Saves the settings.
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="modulePath">The module path.</param>
        ''' <param name="filePath">The file path.</param>
        ''' <param name="input">The input.</param>
        ''' <returns></returns>
        Public Function SaveSettings(Of T)(ByVal modulePath As String, ByVal filePath As String, ByVal input As T) _
            As Boolean
            Try
                CommonHelper.Instance.WriteXML(Path.Combine(modulePath, filePath), SerializeObject(Of T)(input))
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

#End Region

#Region "Validation Rules"
        ''' <summary>
        ''' Determines whether [is validation rule enabled] [the specified validation rule id].
        ''' </summary>
        ''' <param name="validationRuleId">The validation rule id.</param>
        ''' <param name="modulePath">The module path.</param>
        ''' <returns>
        '''   <c>true</c> if [is validation rule enabled] [the specified validation rule id]; otherwise, <c>false</c>.
        ''' </returns>
        Public Function IsValidationRuleEnabled(ByVal validationRuleId As String, ByVal modulePath As String) As Boolean
            Dim validationRules = GetSettings(Of List(Of ValidationRuleSetting))(modulePath, CommonConstants.CONFIGURATION_VALIDATION_RULE_SETTINGS_FILE_PATH)
            If validationRules IsNot Nothing AndAlso validationRules.Count > 0 Then
                Dim validationRule = validationRules.FirstOrDefault(Function(x) x.Id = validationRuleId)
                If validationRule IsNot Nothing Then
                    Return validationRule.IsEnabled
                End If
            End If
            Return False
        End Function
#End Region

#Region "Program Type Settings"
        ''' <summary>
        ''' Gets the program type setting.
        ''' </summary>
        ''' <param name="programTypeCode">The program type code.</param>
        ''' <param name="modulePath">The module path.</param>
        ''' <returns></returns>
        Public Function GetProgramTypeSetting(ByVal programTypeCode As String, ByVal modulePath As String) As ProgramTypeSettings
            Dim programTypeSettings = GetSettings(Of List(Of ProgramTypeSettings))(modulePath, CommonConstants.PROGRAMTYPE_RECERT_OPTION2_SETTING_FILE_PATH)
            If programTypeSettings IsNot Nothing AndAlso programTypeSettings.Count > 0 Then
                Dim programType = programTypeSettings.FirstOrDefault(Function(x) x.ProgramTypeCode = programTypeCode)
                If programType IsNot Nothing Then
                    Return programType
                End If
            End If
            Return Nothing
        End Function
#End Region

        ''' <summary>
        ''' Merges the file.
        ''' </summary>
        ''' <param name="input">The input.</param>
        ''' <param name="filePath">The file path of the current configuration file.</param>
        ''' <returns></returns>
        Public Function MergeFile(Of T)(ByVal input As String, ByVal filePath As String) As AMCStatus
            Dim result As New AMCStatus
            result.Result = True
            Try
                Dim inputObj = DeserializeXML(Of T)(input)
                Dim outputDocument = New XmlDocument()
                outputDocument.Load(filePath)
                If inputObj IsNot Nothing Then
                    If GetType(T).IsGenericType AndAlso (TypeOf (inputObj) Is IEnumerable) Then
                        Dim listObj = CType(inputObj, IEnumerable)
                        For Each item As Object In listObj
                            outputDocument = MergeObject(outputDocument, item)
                        Next
                        outputDocument.Save(filePath)
                    End If
                End If
            Catch ex As Exception
                result.Result = False
                result.Message = ex.Message
            End Try
            Return result
        End Function

        Private Function MergeObject(ByRef outputDocument As XmlDocument, ByVal item As Object) As XmlDocument

            Dim itemType = item.GetType()
            Dim key = itemType.GetField("KEY")
            If key IsNot Nothing Then
                Dim properties = itemType.GetProperties()
                Dim keyVal = key.GetValue(item)
                If keyVal IsNot Nothing AndAlso Not String.IsNullOrEmpty(keyVal.ToString()) Then
                    Dim idProperty = itemType.GetProperty(keyVal.ToString())
                    For Each propertyInfo As PropertyInfo In properties
                        MergeProperties(outputDocument, item, keyVal, idProperty, propertyInfo)
                    Next
                End If
            End If
            Return outputDocument
        End Function

        Private Sub MergeProperties(ByRef outputDocument As XmlDocument, ByVal item As Object, ByVal keyVal As Object, ByVal idProperty As PropertyInfo, ByVal propertyInfo As PropertyInfo)

            Dim xPath = String.Format("//{0}[@{1}='{2}']/@{3}", item.GetType().Name, keyVal, idProperty.GetValue(item, Nothing), propertyInfo.Name)
            Dim currentNode = outputDocument.SelectSingleNode(xPath)
            If currentNode IsNot Nothing Then
                SetValueForAttribute(item, propertyInfo, currentNode)
            Else
                'text()
                xPath = String.Format("//{0}[@{1}='{2}']", item.GetType().Name, keyVal, idProperty.GetValue(item, Nothing))
                currentNode = outputDocument.SelectSingleNode(xPath)
                If currentNode IsNot Nothing Then
                    SetValueForAttribute(item, propertyInfo, currentNode)
                End If
            End If
        End Sub

        Private Sub SetValueForAttribute(ByVal item As Object, ByVal propertyInfo As PropertyInfo, ByRef currentNode As XmlNode)
            Dim propertyValue = propertyInfo.GetValue(item, Nothing)
            If propertyValue IsNot Nothing AndAlso Not String.IsNullOrEmpty(propertyValue.ToString()) Then
                If currentNode.Attributes IsNot Nothing AndAlso currentNode.Attributes.Count > 0 AndAlso currentNode.Attributes("xsi:nil") IsNot Nothing Then
                    currentNode.Attributes.Remove(currentNode.Attributes("xsi:nil"))
                End If

                If TypeOf propertyValue Is Boolean Then
                    currentNode.InnerText = propertyValue.ToString().ToLower()
                ElseIf TypeOf propertyValue Is DateTime Then
                    currentNode.InnerText = XmlConvert.ToString(CType(propertyValue, DateTime), XmlDateTimeSerializationMode.Local)
                Else
                    currentNode.InnerText = propertyValue.ToString()
                End If
            Else
                currentNode.InnerText = String.Empty
            End If
        End Sub

        Public Function MergeConfigurations(Of T)(ByVal configs As String, ByVal filePath As String) As AMCStatus
            Dim result As New AMCStatus
            result.Result = True
            Try
                Dim inputObj = DeserializeXML(Of T)(configs)
                Dim outputDocument = New XmlDocument()
                outputDocument.Load(filePath)
                If inputObj IsNot Nothing Then
                    If GetType(T).IsGenericType AndAlso (TypeOf (inputObj) Is IEnumerable) Then
                        Dim listObj = CType(inputObj, IEnumerable)
                        For Each item As Object In listObj
                            outputDocument = MergeConfigurations(outputDocument, item)
                        Next
                        outputDocument.Save(filePath)
                    End If
                End If
            Catch ex As Exception
                result.Result = False
                result.Message = ex.Message
            End Try
            Return result
        End Function

        Private Function MergeConfigurations(ByRef outputDocument As XmlDocument, ByVal item As Object) As XmlDocument
            Dim itemType = item.GetType()
            Dim key = itemType.GetField("KEY")
            If key IsNot Nothing Then
                Dim keyVal = key.GetValue(item)
                If keyVal IsNot Nothing AndAlso Not String.IsNullOrEmpty(keyVal.ToString()) Then
                    Dim idProperty = itemType.GetProperty(keyVal.ToString())
                    Dim properties = itemType.GetProperties()
                    For Each propertyInfo As PropertyInfo In properties
                        If propertyInfo.PropertyType.IsGenericType Then
                            Dim propertyValue = propertyInfo.GetValue(item, Nothing)
                            Dim listObj = CType(propertyValue, IEnumerable)
                            Dim xPath = String.Format("//{0}/{1}[text()='{2}']/../{3}", item.GetType().Name, keyVal, idProperty.GetValue(item, Nothing), propertyInfo.Name)
                            For Each subItem As Object In listObj
                                MergeSubConfigurations(outputDocument, xPath, subItem)
                            Next
                        Else
                            Dim xPath = String.Format("//{0}/{1}[text()='{2}']/../{3}", item.GetType().Name, keyVal, idProperty.GetValue(item, Nothing), propertyInfo.Name)
                            Dim currentNode = outputDocument.SelectSingleNode(xPath)
                            If currentNode IsNot Nothing Then
                                SetValueForAttribute(item, propertyInfo, currentNode)
                            End If
                        End If
                    Next
                End If
            End If
            Return outputDocument
        End Function

        Private Sub MergeSubConfigurations(ByVal outputDocument As XmlDocument, ByVal xPath As String, ByVal item As Object)
            Dim itemType = item.GetType()
            Dim key = itemType.GetField("KEY")
            If key IsNot Nothing Then
                Dim keyVal = key.GetValue(item)
                If keyVal IsNot Nothing AndAlso Not String.IsNullOrEmpty(keyVal.ToString()) Then
                    Dim idProperty = itemType.GetProperty(keyVal.ToString())
                    Dim properties = itemType.GetProperties()
                    For Each propertyInfo As PropertyInfo In properties
                        If propertyInfo.PropertyType.IsGenericType Then
                            Dim propertyValue = propertyInfo.GetValue(item, Nothing)
                            Dim listObj = CType(propertyValue, IEnumerable)
                            Dim subXPath = String.Format("{0}/{1}/{2}[text()='{3}']/../{4}", xPath, itemType.Name, keyVal, idProperty.GetValue(item, Nothing), propertyInfo.Name)
                            For Each subItem As Object In listObj
                                MergeSubConfigurations(outputDocument, subXPath, subItem) 'recursive
                            Next
                        Else
                            Dim subXPath = String.Format("{0}/{1}/{2}[text()='{3}']/../{4}", xPath, itemType.Name, keyVal, idProperty.GetValue(item, Nothing), propertyInfo.Name)
                            Dim currentNode = outputDocument.SelectSingleNode(subXPath)
                            If currentNode IsNot Nothing Then
                                SetValueForAttribute(item, propertyInfo, currentNode)
                            End If
                        End If
                    Next
                End If
            End If

        End Sub

        Public Sub MergeSingleConfigurations(Of T)(ByVal input As String, ByVal filePath As String)
            Dim inputObj = DeserializeXML(Of T)(input)
            Dim properties = inputObj.GetType().GetProperties()

            Dim inputDoc = New XmlDocument()
            inputDoc.LoadXml(input)

            Dim outputDoc = New XmlDocument()
            outputDoc.Load(filePath)
            If properties IsNot Nothing Then
                For Each propertyInfo As PropertyInfo In properties
                    Dim xPath = String.Format("//{0}", propertyInfo.Name)
                    Dim inputNode = inputDoc.SelectSingleNode(xPath)
                    If inputNode IsNot Nothing Then
                        Dim outputNode = outputDoc.SelectSingleNode(xPath)
                        'outputNode.Value = propertyInfo.GetValue(inputObj, Nothing)
                        If outputNode IsNot Nothing Then
                            SetValueForAttribute(inputObj, propertyInfo, outputNode)
                        End If
                    End If
                Next
            End If
            outputDoc.Save(filePath)
        End Sub
    End Class
End Namespace