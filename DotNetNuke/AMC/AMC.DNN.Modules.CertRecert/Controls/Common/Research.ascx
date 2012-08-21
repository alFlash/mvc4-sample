<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Research.ascx.vb" Inherits="AMC.DNN.Modules.CertRecert.Controls.Common.Research" %>
<%@ Register TagPrefix="amc" TagName="DateTime" Src="../Reusable/AMCDatetime.ascx" %>
<script type="text/javascript">
    jQuery(document).ready(function () {
        var researchPopupBindingList = [["lblResearchTitle", "txtResearch"],
                                        ["lblCompletionDate", "txtCompletionDate"],
                                        ["lblOrganization", "txtOrganization"],
                                        ["lblPoint", "txtApprovedCE"],
                                        ["lblCEValueType", "ddlCEType"]
                                        ];

        var researchPopupControl = new AMCTablePopUp(researchPopupBindingList, "uc_research", "tbl_research",
            "popupAddResearch", "Add Information ", "Edit Item", null);
    });
    
</script>
<div id="uc_research" class="amc-page">
    <asp:HiddenField runat="server" ID="hdIsIncomplete" />
    <asp:HiddenField runat="server" ID="hdIsValidateFailed" />
    <asp:HiddenField runat="server" ID="hdCurrentObjectUniqueId" />
    <div class="amc-title">
        <asp:Label ID="lblResearch" runat="server"></asp:Label>
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
            <asp:Repeater ID="rptResearch" runat="server" EnableViewState="True">
                <HeaderTemplate>
                    <table border="1px" class="amc-table" id="tbl_research">
                        <tr class="amc-table-header">
                            <td visible='<%# GetFieldInfo("ResearchTitle").IsEnabled %>' runat="server">
                                <asp:Label ID="Label1" runat="server" Text='<%# GetFieldInfo("ResearchTitle").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("CompletionDate").IsEnabled %>' runat="server">
                                <asp:Label ID="Label2" runat="server" Text='<%# GetFieldInfo("CompletionDate").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("Organization").IsEnabled %>' runat="server">
                                <asp:Label ID="Label3" runat="server" Text='<%# GetFieldInfo("Organization").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("Point").IsEnabled %>' runat="server" >
                                <asp:Label ID="Label4" runat="server" Text='<%# GetFieldInfo("Point").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("CEType").IsEnabled %>' runat="server">
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
                    <tr class="amc-table-content">
                        <td visible='<%# GetFieldInfo("ResearchTitle").IsEnabled %>' runat="server">
                            <asp:Label ID="lblResearchTitle" runat="server" Text='<%# Eval("ProgramTitle") %>'></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("CompletionDate").IsEnabled %>' runat="server">
                            <asp:Label ID="lblCompletionDate" runat="server"></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("Organization").IsEnabled %>' runat="server">
                            <asp:Label ID="lblOrganization" runat="server" Text='<%# Eval("OrganizationProviding") %>'></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("Point").IsEnabled %>' runat="server" class="text-right">
                            <asp:Label ID="lblPoint" runat="server" ></asp:Label>
                            <asp:Label runat="server" ID="lblPointValue" Text="" CssClass="dn"></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("CEType").IsEnabled %>' runat="server">
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
<div id="popupAddResearch" class="amc-popup">
    <!-- Content  -->
    <asp:Panel runat="server" ID="panelPopup" DefaultButton="btnSave">
        <div class="amc-error-message">
            <asp:Label runat="server" ID="lblPopupMessage"></asp:Label>
            <div>
                <asp:ValidationSummary runat="server" ID="vldsPopupRequiredGroup" ValidationGroup="PopupRequiredGroup" />
            </div>
        </div>
        <table>
            <tr id="ResearchTitle" runat="server">
                <td>
                    <asp:Label ID="lblResearchTitle" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtResearch" runat="server" CssClass="width250"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqResearchTitle"
                        ControlToValidate="txtResearch" EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="CompletionDate" runat="server">
                <td>
                    <asp:Label ID="lblCompletionDate" runat="server"></asp:Label>
                </td>
                <td>
                    <amc:DateTime runat="server" ID="txtCompletionDate" ValidationGroup="PopupRequiredGroup"
                        CssClass="fl"></amc:DateTime>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqCompletionDate"
                        ControlToValidate="txtCompletionDate$txtDatetime" EnableClientScript="True" Display="Dynamic"
                        ValidationGroup="PopupRequiredGroup" CssClass="fl"></asp:RequiredFieldValidator>
                    <div class="cb">
                        <asp:CustomValidator runat="server" ID="cusValidOtherBoard" ControlToValidate="txtCompletionDate$txtDatetime"
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
                    <asp:TextBox ID="txtApprovedCE" runat="server" CssClass="width250"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqPoint" ControlToValidate="txtApprovedCE"
                        EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                    <div>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtApprovedCE"
                            resourcekey="NumberValues.Text" ValidationExpression="^[0-9]{1,3}(\.[0-9]{1,9})?$"
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
