<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SupervisorUC.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.Common.SupervisorUC" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<script type="text/javascript">

    jQuery(document).ready(function () {
        var inteval = setInterval(function () {
            CommonHelper.ClearPhoneEmpty('supervisor-uc', 'txtOfficeTelephone');
            CommonHelper.ClearPhoneEmpty('supervisor-uc', 'txtHomeTelephone');
            clearInterval(inteval);
        }, 500);
    });

    function ValidateEmptyCheckbox(oSrouce, args) {
        var myCheckBox = document.getElementById('<%= chkNotifySupervisor.ClientID %>');
        if (!myCheckBox.checked) {
            args.IsValid = false;
        }
        else {
            args.IsValid = true;
        }
    }

</script>
<div class="amc-page" id="supervisor-uc">
    <asp:HiddenField runat="server" ID="hdIsIncomplete" />
    <asp:HiddenField runat="server" ID="hdIsValidateFailed" />
    <asp:HiddenField runat="server" ID="hdSupervisorId" Value="0" />
    <div class="amc-title">
        <asp:Label runat="server" ID="lblSupervisorUC"></asp:Label>
    </div>
    <div id="FormFillingInstructions" runat="server" class="amc-instruction">
        <asp:Label runat="server" ID="lblFormFillingInstructions"></asp:Label>
    </div>
    <div class="amc-contents">
        <table id="tblSupervisor" runat="server" class="amc-info-table">
            <tr id="NotifySupervisor" runat="server">
                <td colspan="2">
                    <asp:CheckBox runat="server" ID="chkNotifySupervisor" />
                    <asp:Label runat="server" ID="lblNotifySupervisor"></asp:Label>
                    <asp:CustomValidator runat="server" ID="rqNotifySupervisor" ClientValidationFunction="ValidateEmptyCheckbox"
                        Display="Dynamic" ValidationGroup="AmcGeneralGroup" ErrorMessage="*" EnableClientScript="True"
                        ValidateEmptyText="True" CssClass="fl"></asp:CustomValidator>
                </td>
            </tr>
            <tr id="ContactClass" runat="server">
                <td class="width250">
                    <asp:Label runat="server" ID="lblContactClass"></asp:Label>
                </td>
                <td>
                    <asp:RadioButtonList runat="server" ID="rdlContactClass" RepeatDirection="Vertical"
                        Style="width: auto">
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr id="FirstName" runat="server">
                <td class="width250">
                    <asp:Label runat="server" ID="lblFirstName"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="width250 fl" ID="txtFirstName" MaxLength="40"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqFirstName" ControlToValidate="txtFirstName"
                        ValidationGroup="AmcGeneralGroup" EnableClientScript="True" Display="Dynamic"
                        CssClass="fl"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="MiddleName" runat="server">
                <td class="width250">
                    <asp:Label runat="server" ID="lblMiddleName"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="width250 fl" ID="txtMiddleName" MaxLength="40"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqMiddleName" ControlToValidate="txtMiddleName"
                        ValidationGroup="AmcGeneralGroup" EnableClientScript="True" Display="Dynamic"
                        CssClass="fl"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="LastName" runat="server">
                <td class="width250">
                    <asp:Label runat="server" ID="lblLastName"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="width250 fl" ID="txtLastName" MaxLength="60"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqLastName" ControlToValidate="txtLastName"
                        ValidationGroup="AmcGeneralGroup" EnableClientScript="True" Display="Dynamic"
                        CssClass="fl"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="Title" runat="server">
                <td class="width250">
                    <asp:Label runat="server" ID="lblTitle"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="width250 fl" ID="txtTitle" MaxLength="80"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqTitle" ControlToValidate="txtTitle"
                        ValidationGroup="AmcGeneralGroup" EnableClientScript="True" Display="Dynamic"
                        CssClass="fl"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="Institution" runat="server">
                <td class="width250">
                    <asp:Label runat="server" ID="lblInstitution"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="width250 fl" ID="txtInstitution" MaxLength="20"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqInstitution" ControlToValidate="txtInstitution"
                        ValidationGroup="AmcGeneralGroup" EnableClientScript="True" Display="Dynamic"
                        CssClass="fl"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="WorkAddress1" runat="server">
                <td class="width250">
                    <asp:Label runat="server" ID="lblWorkAddress1"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="width250 fl" ID="txtWorkAddress1" MaxLength="80"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqWorkAddress1" ControlToValidate="txtWorkAddress1"
                        ValidationGroup="AmcGeneralGroup" EnableClientScript="True" Display="Dynamic"
                        CssClass="fl"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="WorkAddress2" runat="server">
                <td class="width250">
                    <asp:Label runat="server" ID="lblWorkAddress2"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="width250 fl" ID="txtWorkAddress2" MaxLength="80"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqWorkAddress2" ControlToValidate="txtWorkAddress2"
                        ValidationGroup="AmcGeneralGroup" EnableClientScript="True" Display="Dynamic"
                        CssClass="fl"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="WorkAddress3" runat="server">
                <td class="width250">
                    <asp:Label runat="server" ID="lblWorkAddress3"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="width250 fl" ID="txtWorkAddress3" MaxLength="80"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqWorkAddress3" ControlToValidate="txtWorkAddress3"
                        ValidationGroup="AmcGeneralGroup" EnableClientScript="True" Display="Dynamic"
                        CssClass="fl"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="WorkAddress4" runat="server">
                <td class="width250">
                    <asp:Label runat="server" ID="lblWorkAddress4"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="width250 fl" ID="txtWorkAddress4" MaxLength="80"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqWorkAddress4" ControlToValidate="txtWorkAddress4"
                        ValidationGroup="AmcGeneralGroup" EnableClientScript="True" Display="Dynamic"
                        CssClass="fl"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="City" runat="server">
                <td class="width250">
                    <asp:Label runat="server" ID="lblCity"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="width250 fl" ID="txtCity" MaxLength="40"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqCity" ControlToValidate="txtCity"
                        ValidationGroup="AmcGeneralGroup" EnableClientScript="True" Display="Dynamic"
                        CssClass="fl"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="State" runat="server">
                <td class="width250">
                    <asp:Label runat="server" ID="lblState"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlState" CssClass="fl">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" CssClass="fl" ErrorMessage="*" ID="rqState"
                        ControlToValidate="ddlState" ValidationGroup="AmcGeneralGroup" EnableClientScript="True"
                        Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="Zip" runat="server">
                <td class="width250">
                    <asp:Label runat="server" ID="lblZip"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="width250 fl" ID="txtZip" MaxLength="15"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqZip" ControlToValidate="txtZip"
                        ValidationGroup="AmcGeneralGroup" EnableClientScript="True" Display="Dynamic"
                        CssClass="fl"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="OfficeTelephone" runat="server">
                <td class="width250">
                    <asp:Label runat="server" ID="lblOfficeTelephone"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="width250 fl" ID="txtOfficeTelephone" MaxLength="20"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqOfficeTelephone"
                        ControlToValidate="txtOfficeTelephone" ValidationGroup="AmcGeneralGroup" EnableClientScript="True"
                        Display="Dynamic" CssClass="fl"></asp:RequiredFieldValidator>
                    <cc1:MaskedEditExtender runat="server" TargetControlID="txtOfficeTelephone" ID="maskedOfficeTelephone"
                        Mask="999.999.9999" ClearMaskOnLostFocus="False" ClearTextOnInvalid="True" />
                    <div class="cb">
                        <cc1:MaskedEditValidator runat="server" ID="maskedOfficeTelephoneValidator" ControlToValidate="txtOfficeTelephone"
                            ValidationGroup="AmcGeneralGroup" ControlExtender="maskedOfficeTelephone" ValidationExpression="^\d\d\d.\d\d\d.\d\d\d\d$"
                            IsValidEmpty="True" />
                    </div>
                </td>
            </tr>
            <tr id="EmailAddress" runat="server">
                <td class="width250">
                    <asp:Label runat="server" ID="lblEmailAddress"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="width250 fl" ID="txtEmailAddress" MaxLength="100"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqEmailAddress" ControlToValidate="txtEmailAddress"
                        ValidationGroup="AmcGeneralGroup" EnableClientScript="True" Display="Dynamic"
                        CssClass="fl"></asp:RequiredFieldValidator>
                    <div class="cb">
                        <asp:RegularExpressionValidator runat="server" ID="regexEmailValid" ControlToValidate="txtEmailAddress"
                            ValidationGroup="AmcGeneralGroup" EnableClientScript="True" Display="Dynamic"
                            resourcekey="EmailNotValid.Text">
                        </asp:RegularExpressionValidator>
                    </div>
                </td>
            </tr>
            <tr id="HomeAddress1" runat="server">
                <td class="width250">
                    <asp:Label runat="server" ID="lblHomeAddress1"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="width250 fl" ID="txtHomeAddress1" MaxLength="80"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqHomeAddress1" ControlToValidate="txtHomeAddress1"
                        ValidationGroup="AmcGeneralGroup" EnableClientScript="True" Display="Dynamic"
                        CssClass="fl"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="HomeAddress2" runat="server">
                <td class="width250">
                    <asp:Label runat="server" ID="lblHomeAddress2"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="width250 fl" ID="txtHomeAddress2" MaxLength="80"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqHomeAddress2" ControlToValidate="txtHomeAddress2"
                        ValidationGroup="AmcGeneralGroup" EnableClientScript="True" Display="Dynamic"
                        CssClass="fl"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="HomeAddress3" runat="server">
                <td class="width250">
                    <asp:Label runat="server" ID="lblHomeAddress3"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="width250 fl" ID="txtHomeAddress3" MaxLength="80"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqHomeAddress3" ControlToValidate="txtHomeAddress3"
                        ValidationGroup="AmcGeneralGroup" EnableClientScript="True" Display="Dynamic"
                        CssClass="fl"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="HomeAddress4" runat="server">
                <td class="width250">
                    <asp:Label runat="server" ID="lblHomeAddress4"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="width250 fl" ID="txtHomeAddress4" MaxLength="80"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqHomeAddress4" ControlToValidate="txtHomeAddress4"
                        ValidationGroup="AmcGeneralGroup" EnableClientScript="True" Display="Dynamic"
                        CssClass="fl"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="HomeCity" runat="server">
                <td class="width250">
                    <asp:Label runat="server" ID="lblHomeCity"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="width250 fl" ID="txtHomeCity" MaxLength="40"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqHomeCity" ControlToValidate="txtHomeCity"
                        ValidationGroup="AmcGeneralGroup" EnableClientScript="True" Display="Dynamic"
                        CssClass="fl"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="HomeState" runat="server">
                <td class="width250">
                    <asp:Label runat="server" ID="lblHomeState"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlHomeState" CssClass="fl">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqHomeState" ControlToValidate="ddlHomeState"
                        ValidationGroup="AmcGeneralGroup" EnableClientScript="True" Display="Dynamic"
                        CssClass="fl"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="HomeZip" runat="server">
                <td class="width250">
                    <asp:Label runat="server" ID="lblHomeZip"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="width250 fl" ID="txtHomeZip" MaxLength="15"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqHomeZip" ControlToValidate="txtHomeZip"
                        ValidationGroup="AmcGeneralGroup" EnableClientScript="True" Display="Dynamic"
                        CssClass="fl"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="HomeTelephone" runat="server">
                <td class="width250">
                    <asp:Label runat="server" ID="lblHomeTelephone"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="width250 fl" ID="txtHomeTelephone" MaxLength="20"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqHomeTelephone"
                        ControlToValidate="txtHomeTelephone" ValidationGroup="AmcGeneralGroup" EnableClientScript="True"
                        Display="Dynamic" CssClass="fl"></asp:RequiredFieldValidator>
                    <div class="cb">
                        <cc1:MaskedEditExtender runat="server" TargetControlID="txtHomeTelephone" ID="maskedHomeTelephone"
                            Mask="999.999.9999" ClearMaskOnLostFocus="False" />
                        <cc1:MaskedEditValidator runat="server" ID="maskedHomeTelephoneValidator" ControlToValidate="txtHomeTelephone"
                            ValidationGroup="AmcGeneralGroup" ControlExtender="maskedHomeTelephone" ValidationExpression="^\d\d\d.\d\d\d.\d\d\d\d$"
                            IsValidEmpty="True" />
                    </div>
                </td>
            </tr>
            <tr id="HomeEmailAddress" runat="server">
                <td class="width250">
                    <asp:Label runat="server" ID="lblHomeEmailAddress"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" CssClass="width250 fl" ID="txtHomeEmailAddress" MaxLength="100"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqHomeEmailAddress"
                        ControlToValidate="txtHomeEmailAddress" ValidationGroup="AmcGeneralGroup" EnableClientScript="True"
                        Display="Dynamic" CssClass="fl"></asp:RequiredFieldValidator>
                    <div class="cb">
                        <asp:RegularExpressionValidator runat="server" ID="regexHomeEmailValid" ControlToValidate="txtHomeEmailAddress"
                            ValidationGroup="AmcGeneralGroup" EnableClientScript="True" Display="Dynamic"
                            resourcekey="EmailNotValid.Text">
                        </asp:RegularExpressionValidator>
                    </div>
                </td>
            </tr>
            <tr id="PrefContactMethod" runat="server">
                <td class="width250">
                    <asp:Label runat="server" ID="lblPrefContactMethod"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlPrefContactMethod" CssClass="fl">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqPrefContactMethod"
                        ControlToValidate="ddlPrefContactMethod" EnableClientScript="True" Display="Dynamic"
                        ValidationGroup="AmcGeneralGroup" CssClass="fl">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
    </div>
</div>
<div class="page-break">
</div>
