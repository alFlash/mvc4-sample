<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ProgramProjectActivities.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.Common.ProgramProjectActivities" %>
<%@ Register TagPrefix="amc" TagName="DateTime" Src="../Reusable/AMCDatetime.ascx" %>
<script type="text/javascript">
    jQuery(document).ready(function () {
        var bindingList = [["lblProgramTitle", "txtProgramTitle"],
                            ["lblCompletionDate", "txtProgramDate"],
                            ["lblOrganization", "txtOrganization"],
                            ["lblPoint", "txtApproved"],
                            ["lblCEValueType", "ddlCEType"]];

        var researchPopupControl = new AMCTablePopUp(bindingList, "uc_program_project_activities", "tbl-program",
            "popupAddProgramActivities", "Add Information ", "Edit Item", null);
    });
    
</script>
<div id="uc_program_project_activities" class="amc-page">
    <asp:HiddenField runat="server" ID="hdIsIncomplete" />
    <asp:HiddenField runat="server" ID="hdCurrentObjectUniqueId" />
    <asp:HiddenField runat="server" ID="hdIsValidateFailed" />
    <div class="amc-title">
        <asp:Label ID="lblProgramProjectActivities" runat="server"></asp:Label>
    </div>
    <div id="FormFillingInstructions" runat="server" class="amc-instruction">
        <asp:Label runat="server" ID="lblFormFillingInstructions"></asp:Label>
    </div>
    <div class="optiontitle">
        <asp:Label runat="server" ID="lblOptionTitle"></asp:Label>
    </div>
    <div class="amc-contents">
        <div id="add-new-row" class="amc-add-instruction">
            <asp:Image CssClass="pointer fl" runat="server" ID="Image1" ImageUrl="../../Documentation/images/icons/addnew_11x11.gif">
            </asp:Image>
            <asp:HyperLink runat="server" ID="hlAddNew" resourcekey="AddInformation.Text" CssClass="fl padleft5" href="javascript:void(0);"></asp:HyperLink>
            <div class="cb">
            </div>
        </div>
        <div>
            <asp:Repeater ID="rptProgramProjectActivities" runat="server" EnableViewState="True">
                <HeaderTemplate>
                    <table border="1px" class="amc-table" id="tbl-program">
                        <tr class="amc-table-header">
                            <td runat="server" visible='<%# GetFieldInfo("ProgramTitle").IsEnabled %>'>
                                <asp:Label ID="Label1" runat="server" Text='<%# GetFieldInfo("ProgramTitle").FieldValue %>'></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("ProgramDate").IsEnabled %>'>
                                <asp:Label ID="Label2" runat="server" Text='<%# GetFieldInfo("ProgramDate").FieldValue %>'></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("Organization").IsEnabled %>'>
                                <asp:Label ID="Label3" runat="server" Text='<%# GetFieldInfo("Organization").FieldValue %>'></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("Point").IsEnabled %>'>
                                <asp:Label ID="Label4" runat="server" Text='<%# GetFieldInfo("Point").FieldValue %>'></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("CEType").IsEnabled %>'>
                                <asp:Label ID="Label5" runat="server" Text='<%# GetFieldInfo("CEType").FieldValue %>'></asp:Label>
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
                        <td runat="server" visible='<%# GetFieldInfo("ProgramTitle").IsEnabled %>'>
                            <asp:Label ID="lblProgramTitle" runat="server" Text='<%# Eval("PublicationTitle") %>'></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("ProgramDate").IsEnabled %>'>
                            <asp:Label ID="lblCompletionDate" runat="server"></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("Organization").IsEnabled %>'>
                            <asp:Label ID="lblOrganization" runat="server" Text='<%# Eval("OrganizationProviding") %>'></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("Point").IsEnabled %>' class="text-right">
                            <asp:Label ID="lblPoint" runat="server" ></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("CEType").IsEnabled %>'>
                            <asp:Label ID="lblCEType" runat="server" Text=""></asp:Label>
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
                                CommandName="Delete" CommandArgument='<%# Eval("Guid") %>' CausesValidation="False">
                            </asp:ImageButton>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>
<!-- Popup -->
<div class="totalCE">
    Total Points:
    <asp:Label runat="server" ID="lblTotalCE"></asp:Label>
</div>
<div class="page-break">
</div>
<div id="popupAddProgramActivities" class="amc-popup">
    <!-- Content  -->
    <asp:Panel runat="server" ID="panelPopup" DefaultButton="btnSave">
        <div class="amc-error-message">
            <asp:Label runat="server" ID="lblPopupMessage"></asp:Label>
            <div>
                <asp:ValidationSummary runat="server" ID="vldsPopupRequiredGroup" ValidationGroup="PopupRequiredGroup" />
            </div>
        </div>
        <table>
            <tr id="ProgramTitle" runat="server">
                <td>
                    <asp:Label ID="lblProgramTitle" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtProgramTitle" runat="server" CssClass="width250"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqProgramTitle" ControlToValidate="txtProgramTitle"
                        EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="ProgramDate" runat="server">
                <td>
                    <asp:Label ID="lblProgramDate" runat="server"></asp:Label>
                </td>
                <td>
                    <amc:DateTime runat="server" ID="txtProgramDate" ValidationGroup="PopupRequiredGroup"
                        CssClass="fl"></amc:DateTime>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqProgramDate" ControlToValidate="txtProgramDate$txtDatetime"
                        CssClass="fl" EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                    <div class="cb">
                        <asp:CustomValidator runat="server" ID="cusValidOtherBoard" ControlToValidate="txtProgramDate$txtDatetime"
                            ClientValidationFunction="ValidateReCertCircle" Display="Dynamic" ValidationGroup="PopupRequiredGroup"
                            resourcekey="InvalidTimeFrame.Text" EnableClientScript="True" ValidateEmptyText="True"></asp:CustomValidator>
                    </div>
                </td>
            </tr>
            <tr id="Organization" runat="server">
                <td>
                    <asp:Label ID="lblOrganization" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtOrganization" runat="server" CssClass="width250"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqOrganization" ControlToValidate="txtOrganization"
                        EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="Point" runat="server">
                <td>
                    <asp:Label ID="lblPoint" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtApproved" runat="server" CssClass="width250"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqPoint" ControlToValidate="txtApproved"
                        EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                    <div>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtApproved"
                            resourcekey="NumberValues.Text" ValidationExpression="^[0-9]*(\.[0-9]{1,9})?$"
                            Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RegularExpressionValidator>
                    </div>
                </td>
            </tr>
            <tr id="CEType" runat="server">
                <td>
                    <asp:Label ID="lblCEType" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlCEType">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqCEType" ControlToValidate="ddlCEType"
                        EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnSave" runat="server" Text="OK" ValidationGroup="PopupRequiredGroup" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                    <br />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <!-- End Content  -->
</div>
