<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="RegistrationUC.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.Common.RegistrationUC" %>
<%@ Register TagPrefix="amc" TagName="DateTime" Src="../Reusable/AMCDatetime.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=1.0.20229.20821, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<script type="text/javascript">
    jQuery(document).ready(function () {
        var allExamAdministrationChk = jQuery('#[id*=rblRequireSpecialTest]');
        var txtComment = jQuery('#[id*=requirespecialtestcomments]');
        // check when loading
        if (allExamAdministrationChk.find('input:checked').val() == '0') {
            txtComment.slideDown(300);
        } else if (allExamAdministrationChk.find('input:checked').val() == '1') {
            txtComment.slideUp(300);
        }
        allExamAdministrationChk.on('click', function () {
            if (allExamAdministrationChk.find('input:checked').val() == '0') {
                txtComment.slideDown(300);
            } else if (allExamAdministrationChk.find('input:checked').val() == '1') {
                txtComment.slideUp(300);
            }
        });
        CommonHelper.ClearPhoneEmpty('registration-uc', 'txtHomeTelephone');
    });
    function RegistrationUC_ValidateSpecialTestComments(sender, args) {
        args.IsValid = true;
        var allExamAdministrationChk = jQuery('#[id*=rblRequireSpecialTest]');
        var txtComment = jQuery('#[id*=txtComment]');
        if (allExamAdministrationChk.find('input:checked').val() == '0' && IsStringNullOrEmpty(txtComment.val())) {
            args.IsValid = false;
        }
    }
</script>
<div id="registration-uc" class="amc-page">
    <asp:HiddenField runat="server" ID="hdIsIncomplete" />
    <asp:HiddenField runat="server" ID="hdIsValidateFailed" />
    <asp:HiddenField runat="server" ID="hdSurveyId" Value="0" />
    <div class="amc-title">
        <asp:Label runat="server" ID="lblRegistrationUC"></asp:Label>
    </div>
    <div id="FormFillingInstructions" runat="server" class="amc-instruction">
        <asp:Label runat="server" ID="lblFormFillingInstructions"></asp:Label>
    </div>
    <div class="amc-contents">
        <table id="tblRegistration" runat="server" class="amc-info-table">
            <tr id="FirstName" runat="server">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblFirstName"></asp:Label>:
                    </div>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblFirstNameValue"></asp:Label>
                    <%--<asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqFirstName" ControlToValidate="txtFirstName" EnableClientScript="True"
                Display="Dynamic"></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr id="MiddleName" runat="server">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblMiddleName"></asp:Label>:
                    </div>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblMiddleNameValue"></asp:Label>
                    <%--<asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqMiddleName" ControlToValidate="txtMiddleName" EnableClientScript="True"
                Display="Dynamic"></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr id="LastName" runat="server">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblLastName"></asp:Label>:
                    </div>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblLastNameValue"></asp:Label>
                    <%-- <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqLastName" ControlToValidate="txtLastName" EnableClientScript="True"
                Display="Dynamic"></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr id="HomeAddress" runat="server">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblHomeAddress"></asp:Label>:
                    </div>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblHomeAddressValue"></asp:Label>
                    <%-- <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqHomeAddress" ControlToValidate="txtHomeAddress" EnableClientScript="True"
                Display="Dynamic"></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr id="HomeAddress2" runat="server">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblHomeAddress2"></asp:Label>:
                    </div>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblHomeAddress2Value"></asp:Label>
                    <%--<asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqHomeAddress2" ControlToValidate="txtHomeAddress2" EnableClientScript="True"
               Display="Dynamic"></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr id="HomeAddress3" runat="server">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblHomeAddress3"></asp:Label>:
                    </div>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblHomeAddress3values"></asp:Label>
                    <%--<asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqHomeAddress3" ControlToValidate="txtHomeAddress3" EnableClientScript="True"
                Display="Dynamic"></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr id="HomeAddress4" runat="server">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblHomeAddress4"></asp:Label>:
                    </div>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblHomeAddress4Value"></asp:Label>
                    <%--<asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqHomeAddress4" ControlToValidate="txtHomeAddress4" EnableClientScript="True"
               Display="Dynamic" ></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr id="City" runat="server">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblCity"></asp:Label>:
                    </div>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblCityValue"></asp:Label>
                    <%-- <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqCity" ControlToValidate="txtCity" EnableClientScript="True"
              Display="Dynamic" ></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr id="State" runat="server">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblState"></asp:Label>:
                    </div>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblStateValue"></asp:Label>
                    <%--<asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqState" ControlToValidate="txtState" EnableClientScript="True"
              Display="Dynamic" ></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr id="Zip" runat="server">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblZip"></asp:Label>:
                    </div>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblZipValue"></asp:Label>
                    <%--  <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqZip" ControlToValidate="txtZip" EnableClientScript="True"
              Display="Dynamic" ></asp:RequiredFieldValidator>--%>
                </td>
            </tr>   
            <tr id="FaxNumberHome" runat="server">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblFaxNumberHome"></asp:Label>:
                    </div>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblFaxNumberHomeValue"></asp:Label>
                </td>
            </tr>        
            <tr id="FaxNumberOffice" runat="server">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblFaxNumberOffice"></asp:Label>:
                    </div>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblFaxNumberOfficeValue"></asp:Label>
                </td>
            </tr>
            <tr id="WorkAddressOne" runat="server">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblWorkAddressOne"></asp:Label>:
                    </div>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblWorkAddressOneValue"></asp:Label>
                </td>
            </tr>
            <tr id="WorkAddressTwo" runat="server">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblWorkAddressTwo"></asp:Label>:
                    </div>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblWorkAddressTwoValue"></asp:Label>
                </td>
            </tr>
            <tr id="WorkAddressThree" runat="server">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblWorkAddressThree"></asp:Label>:
                    </div>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblWorkAddressThreeValue"></asp:Label>
                </td>
            </tr>
            <tr id="WorkCity" runat="server">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblWorkCity"></asp:Label>:
                    </div>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblWorkCityValue"></asp:Label>
                </td>
            </tr>
            <tr id="WorkState" runat="server">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblWorkState"></asp:Label>:
                    </div>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblWorkStateValue"></asp:Label>
                </td>
            </tr>
            <tr id="WorkZip" runat="server">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblWorkZip"></asp:Label>:
                    </div>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblWorkZipValue"></asp:Label>
                </td>
            </tr>
            <tr id="EmailAddress" runat="server">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblEmailAddress"></asp:Label>:
                    </div>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblEmailAddressValue"></asp:Label>
                    <%--    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqEmailAddress" ControlToValidate="txtEmailAddress" EnableClientScript="True"
               Display="Dynamic" ></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr id="FormerName" runat="server">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblFormerName"></asp:Label>:
                    </div>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblFormerNameValue"></asp:Label>
                    <%--   <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqFormerName" ControlToValidate="txtFormerName" EnableClientScript="True"
              Display="Dynamic" ></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
             <tr id="HomeTelephone" runat="server">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblHomeTelephone"></asp:Label>:
                    </div>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblHomeTelephoneValue" ></asp:Label>
                    <asp:TextBox runat="server" CssClass="width250 fl dn" ID="txtHomeTelephone"></asp:TextBox>
                    <%--asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqHomeTelephone"
                        ControlToValidate="txtHomeTelephone" ValidationGroup="AmcGeneralGroup" EnableClientScript="True"
                        Display="Dynamic" CssClass="fl"></asp:RequiredFieldValidator--%>
                    <cc1:MaskedEditExtender runat="server" TargetControlID="txtHomeTelephone" ID="maskedHomeTelephone"
                        Mask="999.999.9999" ClearMaskOnLostFocus="False" />
                  <%--<div class="cb">
                        <cc1:MaskedEditValidator runat="server" ID="maskedHomePhoneValidator" ControlToValidate="txtHomeTelephone"
                            ControlExtender="maskedHomeTelephone" IsValidEmpty="True" ValidationExpression="^\d\d\d.\d\d\d.\d\d\d\d$|(___-___-____)" />
                    </div>--%>
                </td>
            </tr>
            <tr id="WorkTelephone" runat="server">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblWorkTelephone"></asp:Label>:
                    </div>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblWorkTelephoneValue"></asp:Label>
                    <asp:TextBox runat="server" ID="txtWorkTelephone" CssClass="width250 dn"></asp:TextBox>
                    <cc1:MaskedEditExtender runat="server" TargetControlID="txtWorkTelephone" ID="maskedWorkTelephone"
                        Mask="999.999.9999" ClearMaskOnLostFocus="False" />
                    <%--<div class="cb">
                        <cc1:MaskedEditValidator runat="server" ID="MaskedEditValidator1" ControlToValidate="txtWorkTelephone"
                            ControlExtender="maskedWorkTelephone" IsValidEmpty="True" ValidationExpression="^\d\d\d.\d\d\d.\d\d\d\d$|(___-___-____)" />
                    </div>--%>
                </td>
            </tr>
            <tr id="Degree" runat="server">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblDegree"></asp:Label>:
                    </div>
                </td>
                <td>
                    <asp:ListBox runat="server" ID="lstbDegree" Rows="7" SelectionMode="Multiple" CssClass="fl">
                    </asp:ListBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqDegree" ControlToValidate="lstbDegree"
                        ValidationGroup="AmcGeneralGroup" EnableClientScript="True" Display="Dynamic"
                        CssClass="fl"></asp:RequiredFieldValidator>
                        <asp:Label runat="server" ID="lblDegreeValue"></asp:Label>
                </td>
            </tr>
             <tr id="NameAppearOnCertificate" runat="server">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblNameAppearOnCertificate"></asp:Label>:
                    </div>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="width250 fl" ID="txtNameAppearOnCertificate"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqNameAppearOnCertificate" ControlToValidate="txtNameAppearOnCertificate" 
                     ValidationGroup="AmcGeneralGroup" EnableClientScript="True" Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="DateOfBirth" runat="server">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblDateOfBirth"></asp:Label>:
                    </div>
                </td>
                <td>
                    <amc:DateTime runat="server" ID="txtBirthDate" CssClass="fl"></amc:DateTime>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqDateOfBirth" ControlToValidate="txtBirthDate$txtDatetime"
                        EnableClientScript="True" ValidationGroup="AmcGeneralGroup" Display="Dynamic"
                        CssClass="fl"></asp:RequiredFieldValidator>
                    <div class="cb">
                        <asp:TextBox runat="server" CssClass="width250" ID="txtDateOfBirthValues" Enabled="False"></asp:TextBox>
                        <asp:TextBox runat="server" ID="txtCurrentDate" CssClass="dn width250"></asp:TextBox>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" resourcekey="CompareDateDemoGraphic.Text"
                            Operator="LessThanEqual" ControlToValidate="txtBirthDate$txtDatetime" Type="Date"
                            Display="Dynamic" ControlToCompare="txtCurrentDate" />
                    </div>
                </td>
            </tr>
            <tr id="RequireSpecialTest" runat="server" Visible="False">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblRequireSpecialTest"></asp:Label>
                    </div>
                </td>
                <td>
                    <asp:RadioButtonList runat="server" ID="rblRequireSpecialTest" RepeatDirection="Horizontal"
                        Width="150px">
                        <asp:ListItem Value="0" resourcekey="Yes.Text"></asp:ListItem>
                        <asp:ListItem Value="1" resourcekey="No.Text" Selected="True"></asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:HiddenField runat="server" ID="hdRequireSpecialTestResponse" Value="0" />
                    <asp:HiddenField runat="server" ID="hdRequireSpecialTestQuestionId" />
                    <asp:HiddenField runat="server" ID="hdAnswerYes" Value="0" />
                    <asp:HiddenField runat="server" ID="hdAnswerNo" Value="0" />
                </td>
            </tr>
            <tr id="requirespecialtestcomments" class="dn">
                <td>
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblrequirespecialtestcomments"></asp:Label>
                    </div>
                </td>
                <td>
                    <div class="cb">
                        <asp:CustomValidator runat="server" ID="rqSpecialTestComments" EnableClientScript="True"
                            Display="Dynamic" ValidationGroup="AmcGeneralGroup" ClientValidationFunction="RegistrationUC_ValidateSpecialTestComments"
                            ControlToValidate="txtComment" ValidateEmptyText="True" resourcekey="CommentEducation.Text"></asp:CustomValidator>
                    </div>
                    <div class="cb">
                        <asp:TextBox runat="server" CssClass="width250" ID="txtComment" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr id="RequireSpecialTestDesc" runat="server" Visible="False">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblRequireSpecialTestDesc"></asp:Label>
                    </div>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlRequireSpecialTestDesc" CssClass="fl">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqRequireSpecialTestDesc"
                        ControlToValidate="ddlRequireSpecialTestDesc" ValidationGroup="AmcGeneralGroup"
                        EnableClientScript="True" Display="Dynamic" CssClass="fl"></asp:RequiredFieldValidator>
                    <asp:HiddenField runat="server" ID="hdKnownFromResponseID" Value="0" />
                    <asp:HiddenField runat="server" ID="hdKnownFromQuestionId" Value="0" />
                </td>
            </tr>
            <tr id="MessagesLeft" runat="server" Visible="False">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblMessagesLeft"></asp:Label>:
                    </div>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="width250" ID="txtMessagesLeft"></asp:TextBox>
                    <asp:HiddenField runat="server" ID="hdMessageLeftQuestionId" Value="0" />
                    <asp:HiddenField runat="server" ID="hdMessageLeftResponse" Value="0" />
                    <asp:HiddenField runat="server" ID="hdMessageLeftAnswer" Value="0" />
                </td>
            </tr>
             <tr id="CertificationNumber" runat="server">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblCertificationNumber"></asp:Label>:
                    </div>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblCertificationNumberValue"></asp:Label>
                </td>
            </tr>
            <tr id="ReceiveMaterials" runat="server" class="dn">
                <td class="width250">
                    <div style="white-space: nowrap;">
                        <asp:Label runat="server" ID="lblReceiveMaterials"></asp:Label>:
                    </div>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlReceiveMaterials">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
</div>
<div class="page-break">
</div>
