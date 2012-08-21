<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PracticeExperienceDetailsUC.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.Common.PracticeExperienceDetailsUC" %>
<%@ Register TagPrefix="dnn" TagName="sectionhead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="amc" TagName="DateTime" Src="../Reusable/AMCDatetime.ascx" %>
<script type="text/javascript">

    jQuery(document).ready(function () {
        var bindingList = [['lblPosition', 'txtPosition'],
                           ['lblStartDate', 'txtStartDate'],
                           ['lblEndDate', 'txtEndDate'],
                           ['lblOrganization', 'txtOrganization'],
                           ['lblPoint', 'txtApprovedCE']];

        var practicePopup = new AMCTablePopUp(bindingList, "uc_practice_experience", "tbl-practice-experience",
            "popupAddPracticeExperience", "Add Information ", "Edit Item", null);
    });
    
</script>
<div id="uc_practice_experience" class="amc-page">
    <asp:HiddenField runat="server" ID="hdIsIncomplete" />
    <asp:HiddenField runat="server" ID="hdIsValidateFailed" />
    <asp:HiddenField runat="server" ID="hdCurrentObjectUniqueId" />
    <div class="amc-title">
        <asp:Label runat="server" ID="lblPracticeExperienceDetailsUC"></asp:Label>
    </div>
    <div id="FormFillingInstructions" runat="server" class="amc-instruction">
        <asp:Label runat="server" ID="lblFormFillingInstructions"></asp:Label>
    </div>
    <div class="amc-contents">
        <div id="add-new-row" class="amc-add-instruction">
            <asp:Image CssClass="pointer fl" runat="server" ID="Image1" ImageUrl="../../Documentation/images/icons/addnew_11x11.gif">
            </asp:Image>
            <asp:HyperLink runat="server" ID="hlAddNew" resourcekey="AddInformation.Text" CssClass="fl padleft5"
                href="javascript:void(0);"></asp:HyperLink>
            <div class="cb">
            </div>
        </div>
        <div>
            <asp:Repeater ID="rptPracticeExprience" runat="server" EnableViewState="True">
                <HeaderTemplate>
                    <table border="1px" class="amc-table" id="tbl-practice-experience">
                        <tr class="amc-table-header">
                            <td runat="server" visible='<%# GetFieldInfo("Position").IsEnabled %>'>
                                <asp:Label ID="Label1" runat="server" Text='<%# GetFieldInfo("Position").FieldValue %>'></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("StartDate").IsEnabled %>'>
                                <asp:Label ID="Label2" runat="server" Text='<%# GetFieldInfo("StartDate").FieldValue %>'></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("EndDate").IsEnabled %>'>
                                <asp:Label ID="Label3" runat="server" Text='<%# GetFieldInfo("EndDate").FieldValue %>'></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("Organization").IsEnabled %>'>
                                <asp:Label ID="Label4" runat="server" Text='<%# GetFieldInfo("Organization").FieldValue %>'></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("Point").IsEnabled %>'>
                                <asp:Label ID="Label5" runat="server" Text='<%# GetFieldInfo("Point").FieldValue %>'></asp:Label>
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
                        <td runat="server" visible='<%# GetFieldInfo("Position").IsEnabled %>'>
                            <asp:Label ID="lblPosition" runat="server" Text='<%# Eval("Position") %>'></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("StartDate").IsEnabled %>'>
                            <asp:Label ID="lblStartDate" runat="server"></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("EndDate").IsEnabled %>'>
                            <asp:Label ID="lblEndDate" runat="server"></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("Organization").IsEnabled %>'>
                            <asp:Label ID="lblOrganization" runat="server" Text='<%# Eval("OrganizationProviding") %>'></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("Point").IsEnabled %>' class="text-right">
                            <asp:Label ID="lblPoint" runat="server"></asp:Label>
                            <asp:Label runat="server" ID="lblPointValue" Text="" CssClass="dn"></asp:Label>
                        </td>
                        <td class="text-right">
                            <asp:Label runat="server" ID="lblSumaryPoint" Text=""></asp:Label>
                        </td>
                        <td class="action-column">
                            <asp:HiddenField runat="server" ID="hdObjectUniqueId" Value='<%# Eval("Guid") %>' />
                            <asp:Image runat="server" ID="imgEdit" ImageUrl="../../Documentation/images/icons/EditIcon.gif">
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
<div class="totalCE">
    Total Points:
    <asp:Label runat="server" ID="lblTotalCE"></asp:Label>
</div>
<div id="popupAddPracticeExperience" class="amc-popup">
    <!-- Content  -->
    <asp:Panel runat="server" ID="panelPopup" DefaultButton="btnSave">
        <div class="amc-error-message">
            <asp:Label runat="server" ID="lblPopupMessage"></asp:Label>
            <div>
                <asp:ValidationSummary runat="server" ID="vldsPopupRequiredGroup" ValidationGroup="PopupRequiredGroup" />
            </div>
        </div>
        <table>
            <tr id="Position" runat="server">
                <td>
                    <asp:Label ID="lblPosition" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPosition" runat="server" CssClass="width250"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqPosition" ControlToValidate="txtPosition"
                        EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="StartDate" runat="server">
                <td>
                    <asp:Label ID="lblStartDate" runat="server"></asp:Label>
                </td>
                <td>
                    <amc:DateTime runat="server" ID="txtStartDate" ValidationGroup="PopupRequiredGroup"
                        CssClass="fl"></amc:DateTime>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqStartDate" ControlToValidate="txtStartDate$txtDatetime"
                        EnableClientScript="True" CssClass="fl" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                    <div class="cb">
                        <asp:CustomValidator runat="server" ID="CustomValidator1" ControlToValidate="txtStartDate$txtDatetime"
                            ClientValidationFunction="ValidateReCertCircle" Display="Dynamic" ValidationGroup="PopupRequiredGroup"
                            resourcekey="InvalidTimeFrame.Text" EnableClientScript="True" ValidateEmptyText="True"></asp:CustomValidator>
                    </div>
                </td>
            </tr>
            <tr id="EndDate" runat="server">
                <td>
                    <asp:Label ID="lblEndDate" runat="server"></asp:Label>
                </td>
                <td>
                    <amc:DateTime runat="server" ID="txtEndDate" ValidationGroup="PopupRequiredGroup"
                        CssClass="fl"></amc:DateTime>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqEndDate" ControlToValidate="txtEndDate$txtDatetime"
                        EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"
                        CssClass="fl"></asp:RequiredFieldValidator>
                    <div class="cb">
                        <asp:CompareValidator ID="Compare1" ControlToValidate="txtEndDate$txtDatetime" ControlToCompare="txtStartDate$txtDatetime"
                            resourcekey="CompareDateCommon.Text" Type="Date" runat="server"
                            Display="Dynamic" Operator="GreaterThanEqual" ValidationGroup="PopupRequiredGroup" />
                    </div>
                    <div class="cb">
                        <asp:CustomValidator runat="server" ID="cusValidOtherBoard" ControlToValidate="txtEndDate$txtDatetime"
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
<div class="page-break">
</div>
