Imports AMC.DNN.Modules.CertRecert.Business.Controller

Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Text.RegularExpressions
Imports TIMSS.API.User.CertificationInfo

Imports TIMSS.API.User.UserDefinedInfo

Namespace Helpers
    Public Class CommonHelper

#Region "Singleton"

        ''' <summary>
        ''' Gets the instance.
        ''' </summary>
        ''' 
        Public Shared ReadOnly Instance As New CommonHelper

        Private Sub New()

        End Sub

#End Region

#Region "Private Member"

#End Region

#Region "Public Methods"


        ''' <summary>
        ''' Chops the unused decimal.
        ''' </summary>
        ''' <param name="input">The input.</param>
        ''' <returns></returns>
        Public Shared Function ChopUnusedDecimal(ByVal input As String) As String
            Dim result = "0"
            Dim resultDecimal As Decimal = 0
            If Not String.IsNullOrEmpty(input) Then
                If Decimal.TryParse(input, resultDecimal) Then
                    resultDecimal = Math.Round(resultDecimal, 2, MidpointRounding.AwayFromZero)
                    Dim regex = New Regex("(\.[1-9]*)0+$")
                    result = regex.Replace(resultDecimal.ToString(), "$1") '"(\.[1-9]*)0+$" => "(\.[1-9]*)"
                    regex = New Regex("\.$")
                    result = regex.Replace(result, "")
                End If
            End If
            Return result
        End Function

        ''' <summary>
        ''' Writes the XML.
        ''' </summary>
        ''' <param name="filepath">The filepath.</param>
        ''' <param name="val">The val.</param>
        Public Sub WriteXML(ByVal filepath As String, ByVal val As String)
            Dim file As FileStream = Nothing
            Dim writer As StreamWriter = Nothing
            Try
                file = New FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read)
                file.SetLength(0)
                file.Flush()
                writer = New StreamWriter(file)
                writer.Write(val)
                writer.Flush()
            Catch ex As Exception
                Throw
            Finally
                If writer IsNot Nothing Then
                    writer.Close()
                End If
                If file IsNot Nothing Then
                    file.Close()
                End If
            End Try
            'File.WriteAllText(filepath, val)
        End Sub

        Public Shared Sub BindCEType(ByVal ddlName As DropDownList, ByVal programType As String, ByVal amcController As AmcCertRecertController)
            Dim ceTypeList = amcController.GetCETypesByProgramType(programType)
            ddlName.DataTextField = "Description"
            ddlName.DataValueField = "Code"
            ddlName.DataSource = ceTypeList
            ddlName.DataBind()
        End Sub

        ''' <summary>
        ''' Get CEWeight of CETypeCode when selected value on dropdown CEType
        ''' </summary>
        ''' <param name="cETypeCode"> CETypeCode of Item is selected on dropdown </param>
        Public Shared Function GetCEWeight(ByVal cETypeCode As String,
                                           ByVal cEWeightList As UserDefinedCertificationCEWeights,
                                           ByVal isHaveCEType As Boolean, ByVal defaultCEWeight As String) As String
            Dim weight As String = "1"
            If isHaveCEType = True Then '' have CEType => get weight 
                If cEWeightList IsNot Nothing Then
                    If cEWeightList.Count > 0 Then
                        For Each ceWeightObject As UserDefinedCertificationCEWeight In cEWeightList
                            If ceWeightObject.CEType.Code = cETypeCode Then
                                weight = ceWeightObject.Weight.ToString()
                                Exit For
                            End If
                        Next
                    End If
                End If
            Else
                weight = defaultCEWeight
            End If
            '' if don't have CEType , set weight = 1 (default value)
            Return weight
        End Function

        Public Shared Sub BindIssuingBodyType(ByVal ddlIssuingBodyType As DropDownList,
                                               ByVal documentationTypeString As String)
            Dim customerExternalDocumentation =
                    (New UserDefinedCustomerExternalDocumentations()).CreateNew()
            customerExternalDocumentation.DocumentationTypeString = documentationTypeString
            customerExternalDocumentation.IssuingBody.FillList()
            ddlIssuingBodyType.DataTextField = "Description"
            ddlIssuingBodyType.DataValueField = "Code"
            ddlIssuingBodyType.DataSource = customerExternalDocumentation.IssuingBody.List
            ddlIssuingBodyType.DataBind()
        End Sub
        
        Public Shared Sub BindPrefContactMethod(ByVal ddlName As DropDownList)
            Dim userDefinedCustomerExternalContactList As UserDefinedCustomerExternalContacts =
                    New UserDefinedCustomerExternalContacts()
            Dim userDefinedCustomerExternalContact As IUserDefinedCustomerExternalContact
            userDefinedCustomerExternalContact = userDefinedCustomerExternalContactList.CreateNew()
            ddlName.DataTextField = "Description"
            ddlName.DataValueField = "Code"
            ddlName.DataSource = userDefinedCustomerExternalContact.PrefContactMethod.List
            ddlName.DataBind()
        End Sub

        Public Shared Sub BindContactClassType(ByVal contactClassControl As ListControl)
            Dim userDefinedCustomerExternalContactList As UserDefinedCustomerExternalContacts =
                    New UserDefinedCustomerExternalContacts()
            Dim userDefinedCustomerExternalContact As IUserDefinedCustomerExternalContact
            userDefinedCustomerExternalContact = userDefinedCustomerExternalContactList.CreateNew()
            contactClassControl.DataTextField = "Description"
            contactClassControl.DataValueField = "Code"
            contactClassControl.DataSource = userDefinedCustomerExternalContact.ContactClassType.List
            contactClassControl.DataBind()
        End Sub

        Public Shared Sub BindCertificationType(ByVal certificationTypeCtrl As ListControl)
            Dim customerCertification =
                (New CertificationCustomerCertifications).CreateNew()
            certificationTypeCtrl.DataTextField = "Description"
            certificationTypeCtrl.DataValueField = "Code"
            certificationTypeCtrl.DataSource = customerCertification.CertificationTypeCode.List
            certificationTypeCtrl.DataBind()
        End Sub

        Public Shared Function CheckIsNumber(ByVal strInput As String) As Boolean
            Dim Num As Double
            Dim isNum As Boolean = Double.TryParse(strInput, Num)
            If isNum Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function CalculatorTotalCE(ByVal organizationId As String, ByVal organizationUnitId As String,
                                               ByVal masterCustomerId As String, ByVal subCustomerId As Integer,
                                               ByVal programTypeCEActivity As String, ByVal certificationId As Integer,
                                               ByVal factorIndex As String) As Decimal
            Dim amcCertRecertController As AmcCertRecertController = New AmcCertRecertController(organizationId,
                                                                                                        organizationUnitId,
                                                                                                        certificationId, String.Empty, masterCustomerId, subCustomerId)
            Dim total As Decimal = 0
            If CheckIsNumber(factorIndex) Then
                total =
                    amcCertRecertController.GetCETotal(masterCustomerId, subCustomerId, programTypeCEActivity,
                                                        certificationId) * Decimal.Parse(factorIndex)
                total = Math.Round(total, 2, MidpointRounding.AwayFromZero)
            End If
            Return total
        End Function

        Public Shared Function CalculatorCEItem(ByVal CEHours As String, ByVal factorIndex As String) As Decimal
            If CheckIsNumber(CEHours) AndAlso CheckIsNumber(factorIndex) Then
                Return Math.Round((Decimal.Parse(CEHours) * Decimal.Parse(factorIndex)), 2, MidpointRounding.AwayFromZero)
            End If
            Return 0
        End Function

#End Region
    End Class
End Namespace