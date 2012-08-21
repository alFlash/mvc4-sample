<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ContinuingEducation.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.Common.ContinuingEducation" %>
<%@ Register TagPrefix="amc" TagName="DateTime" Src="../Reusable/AMCDatetime.ascx" %>
<script type="text/javascript">
    function showPracticePopup(isShow) {
        AmcCert.showPracticePopup("add-continuingEducation", isShow);
        AmcCert.SetTitle("add-continuingEducation", "Add Information ");
    }

    jQuery(document).ready(function () {
        var bindingList = [['lblProgramTittle', 'txtProgramTittle'],
                            ['lblNameOfOrganizationProviding', 'txtNameOfOrganizationProviding'],
                            ['lblNameOfOrganizationApproved', 'txtNameOfOrganizationApproved'],
                           ['lblApprovodContacHours', 'txtApprovodContacHours'],
                           ['lbldate', 'txtDate'],
                           ['lblpoint', 'txtPoint'],
                           ['lblCEValueType', 'ddlCEType']];

        var practicePopup = new AMCTablePopUp(bindingList,
            "continuingEducation-uc",
            "tblcontinuingEducation",
            "add-continuingEducation",
            "Add Information ",
            "Edit Item",
            null);
    });
</script>
<div id="continuingEducation-uc" class="amc-page">
    <asp:HiddenField runat="server" ID="hdIsIncomplete" />
    <asp:HiddenField runat="server" ID="hdIsValidateFailed" />
    <asp:HiddenField runat="server" ID="hdCurrentObjectUniqueId" />
    <div class="optiontitle">
        <asp:Label runat="server" ID="lblErrorMessageChangeOption" ForeColor="red"></asp:Label>
    </div>
    <div class="amc-title">
        <asp:Label runat="server" ID="lblContinuingEducation"></asp:Label>
    </div>
    <div id="FormFillingInstructions" runat="server" class="amc-instruction">
        <asp:Label runat="server" ID="lblFormFillingInstructions"></asp:Label>
    </div>
    <div class="optiontitle">
        <asp:Label runat="server" ID="lblOptionTitle"></asp:Label>
    </div>
    <div class="amc-contents">
        <div id="add-new-row" class="amc-add-instruction">
            <asp:Image CssClass="pointer fl" runat="server" ID="imgAdd" ImageUrl="../../Documentation/images/icons/addnew_11x11.gif">
            </asp:Image>
            <asp:HyperLink runat="server" ID="hlAddNew" CssClass="fl padleft5" href="javascript:void(0);" resourcekey="AddInformation.Text" ></asp:HyperLink>
            <div class="cb">
            </div>
        </div>
        <div>
            <asp:Repeater runat="server" ID="rptcontinuingEducation" EnableViewState="True">
                <HeaderTemplate>
                    <table id="tblcontinuingEducation" border="1px" class="amc-table">
                        <tr class="amc-table-header">
                            <td visible='<%# GetFieldInfo("ProgramTittle").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblProgramTittleHeader" Text='<%# GetFieldInfo("ProgramTittle").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("NameOfOrganizationProviding").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblNameOfOrganizationProvidingHeader" Text='<%# GetFieldInfo("NameOfOrganizationProviding").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("NameOfOrganizationApproved").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblNameOfOrganizationApprovedHeader" Text='<%# GetFieldInfo("NameOfOrganizationApproved").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("ApprovodContacHours").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblApprovodContacHoursHeader" Text='<%# GetFieldInfo("ApprovodContacHours").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("Date").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lbldateHeader" Text='<%# GetFieldInfo("Date").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("Point").IsEnabled %>' runat="server" >
                                <asp:Label runat="server" ID="lblpointHeader" Text='<%# GetFieldInfo("Point").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("CEType").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblCETypeHeader" Text='<%# GetFieldInfo("CEType").FieldValue %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblSumaryPointHeader" Text="Points"></asp:Label>
                            </td>
                            <td class="action-column">
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="amc-table-content" id="item-content">
                        <td visible='<%# GetFieldInfo("ProgramTittle").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblProgramTittle" Text='<%# Eval("ProgramTitle") %>'></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("NameOfOrganizationProviding").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblNameOfOrganizationProviding" Text='<%# Eval("OrganizationProviding") %>'></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("NameOfOrganizationApproved").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblNameOfOrganizationApproved" Text='<%# Eval("OrganizationApproving") %>'></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("ApprovodContacHours").IsEnabled %>' runat="server" class="text-right">
                            <asp:Label runat="server" ID="lblApprovodContacHours" ></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("Date").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lbldate"></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("Point").IsEnabled %>' runat="server" class="text-right">
                            <asp:Label runat="server" ID="lblpoint" Text=""></asp:Label>
                            <asp:Label runat="server" ID="lblPointValue" Text="" CssClass="dn"></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("CEType").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblCEType"></asp:Label>
                            <asp:Label ID="lblCEValueType" runat="server" Text="" CssClass="dn"></asp:Label>
                        </td>
                        <td class="text-right">
                             <asp:Label runat="server" ID="lblSumaryPoint" Text=""></asp:Label>
                        </td>
                        <td class="action-column">
                            <asp:HiddenField runat="server" ID="hdObjectUniqueId" Value='<%# Eval("Guid") %>' />
                            <asp:Image CssClass="pointer" runat="server" ID="imgEdit" ImageUrl="../../Documentation/images/icons/EditIcon.gif">
                            </asp:Image>
                            <asp:ImageButton CssClass="pointer" runat="server" ID="imgItemDelete" ImageUrl="../../Documentation/images/icons/delete_icon1.gif"
                                CommandName="Delete" CommandArgument='<%# Eval("Guid") %>' CausesValidation="False"></asp:ImageButton>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
    <div class="totalCE">
        Total Points:
        <asp:Label runat="server" ID="lblTotalCE"></asp:Label>
    </div>
    <div id="add-continuingEducation" class="amc-popup">
        <asp:Panel runat="server" ID="panelPopup" DefaultButton="btnSave">
            <div class="amc-error-message">
                <asp:Label runat="server" ID="lblPopupMessage"></asp:Label>
                <div>
                    <asp:ValidationSummary runat="server" ID="vldsPopupRequiredGroup" ValidationGroup="PopupRequiredGroup" />
                </div>
            </div>
            <table class="width500">
                <tr id="ProgramTittle" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblProgramTittle"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250 fl" ID="txtProgramTittle"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqProgramTittle"
                            ControlToValidate="txtProgramTittle" EnableClientScript="True" Display="Dynamic"
                            ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="NameOfOrganizationProviding" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblNameOfOrganizationProviding"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250 fl" ID="txtNameOfOrganizationProviding"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqNameOfOrganizationProviding"
                            ControlToValidate="txtNameOfOrganizationProviding" EnableClientScript="True" Display="Dynamic"
                            ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="NameOfOrganizationApproved" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblNameOfOrganizationApproved"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250 fl" ID="txtNameOfOrganizationApproved"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqNameOfOrganizationApproved"
                            ControlToValidate="txtNameOfOrganizationApproved" EnableClientScript="True" Display="Dynamic"
                            ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="ApprovodContacHours" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblApprovodContacHours"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250 fl" ID="txtApprovodContacHours"></asp:TextBox>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtApprovodContacHours"
                                resourcekey="NumberValuesConEducation.Text" ValidationExpression="^[0-9]{1,3}(\.[0-9]{1,9})?$"
                                Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RegularExpressionValidator>
                        </div>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqApprovodContacHours"
                            ControlToValidate="txtApprovodContacHours" EnableClientScript="True" Display="Dynamic"
                            ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="Date" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblDate"></asp:Label>
                    </td>
                    <td>
                        <amc:DateTime runat="server" ID="txtDate" ValidationGroup="PopupRequiredGroup" CssClass="fl">
                        </amc:DateTime>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqDate" ControlToValidate="txtDate$txtDatetime"
                            EnableClientScript="True" CssClass="fl" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                        <div class="cb">
                            <asp:CustomValidator runat="server" ID="cusValidOtherBoard" ControlToValidate="txtDate$txtDatetime"
                                ClientValidationFunction="ValidateReCertCircle" Display="Dynamic" ValidationGroup="PopupRequiredGroup"
                                resourcekey="InvalidTimeFrame.Text" EnableClientScript="True" ValidateEmptyText="True"></asp:CustomValidator>
                        </div>
                    </td>
                </tr>
                <tr id="Point" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblPoint"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250 fl" ID="txtPoint"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqPoint" ControlToValidate="txtPoint"
                            EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtPoint"
                                resourcekey="NumberValues.Text" ValidationExpression="^[0-9]{1,3}(\.[0-9]{1,9})?$"
                                Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
                <tr id="CEType" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblCEType"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlCEType">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqCEType" ControlToValidate="ddlCEType"
                            EnableClientScript="True" ValidationGroup="PopupRequiredGroup" CssClass="fl"
                            Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
            <div class="pad5">
                <asp:Button ID="btnSave" runat="server" Text="OK" ValidationGroup="PopupRequiredGroup" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                <br />
            </div>
        </asp:Panel>
    </div>
</div>
<div class="page-break">
</div>
