<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="OrganizationInvolvement.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.Common.OrganizationInvolvement" %>
<%@ Register TagPrefix="amc" TagName="DateTime" Src="../Reusable/AMCDatetime.ascx" %>
<script type="text/javascript">
    function showPracticePopup(isShow) {
        AmcCert.showPracticePopup("add-involvement", isShow);
        AmcCert.SetTitle("add-involvement", "Add Information ");
    }

    jQuery(document).ready(function () {
        var bindingList = [['lblTitle', 'txtTitle'],
                            ['lblFullName', 'txtFullName'],
                            ['lblPoint', 'txtNumberOfHours'],
                            ['lblRole', 'txtRole'],
                            ['lblDate', 'txtDate'],
                            ['lblCEValueType', 'ddlCEType']
                            ];

        var practicePopup = new AMCTablePopUp(bindingList,
            "involvement-uc",
            "tblinvolvement",
            "add-involvement",
            "Add Information ",
            "Edit Item",
            null);
    });
    
</script>
<div id="involvement-uc" class="amc-page">
    <asp:HiddenField runat="server" ID="hdIsIncomplete" />
    <asp:HiddenField runat="server" ID="hdIsValidateFailed" />
    <asp:HiddenField runat="server" ID="hdCurrentObjectUniqueId" />
    <div class="amc-title">
        <asp:Label runat="server" ID="lblOrganizationInvolvement"></asp:Label>
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
        <asp:Repeater runat="server" ID="rptInvolvement" EnableViewState="True">
            <HeaderTemplate>
                <table id="tblinvolvement" border="1px" class="amc-table">
                    <tr class="amc-table-header">
                        <td visible='<%# GetFieldInfo("Title").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblTitleInvolvement" Text='<%# GetFieldInfo("Title").FieldValue %>'></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("FullName").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblFullNameInvolvement" Text='<%# GetFieldInfo("FullName").FieldValue %>'></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("Point").IsEnabled %>' runat="server" >
                            <asp:Label runat="server" ID="lblNumberOfHoursInvolvement" Text='<%# GetFieldInfo("Point").FieldValue %>'></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("Role").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblRole" Text='<%# GetFieldInfo("Role").FieldValue %>'></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("Date").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblDateInvolvement" Text='<%# GetFieldInfo("Date").FieldValue %>'></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("TypeOfCE").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblTypeOfCEInvolvement" Text='<%# GetFieldInfo("TypeOfCE").FieldValue %>'></asp:Label>
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
                    <td visible='<%# GetFieldInfo("Title").IsEnabled %>' runat="server">
                        <asp:Label runat="server" ID="lblTitle" Text='<%# Eval("ProgramTitle") %>'></asp:Label>
                    </td>
                    <td visible='<%# GetFieldInfo("FullName").IsEnabled %>' runat="server">
                        <asp:Label runat="server" ID="lblFullName" Text='<%# Eval("OrganizationProviding") %>'></asp:Label>
                    </td>
                    <td visible='<%# GetFieldInfo("Point").IsEnabled %>' runat="server" class="text-right">
                        <asp:Label runat="server" ID="lblPoint" ></asp:Label>
                        <asp:Label runat="server" ID="lblPointValue" Text="" CssClass="dn"></asp:Label>
                    </td>
                    <td visible='<%# GetFieldInfo("Role").IsEnabled %>' runat="server">
                        <asp:Label runat="server" ID="lblRole" Text='<%# Eval("Role") %>'></asp:Label>
                    </td>
                    <td visible='<%# GetFieldInfo("Date").IsEnabled %>' runat="server">
                        <asp:Label runat="server" ID="lblDate"></asp:Label>
                    </td>
                    <td visible='<%# GetFieldInfo("TypeOfCE").IsEnabled %>' runat="server">
                        <asp:Label runat="server" ID="lblTypeOfCE" Text=""></asp:Label>
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
    <div class="totalCE">
        Total Points:
        <asp:Label runat="server" ID="lblTotalCE"></asp:Label>
    </div>
    <div id="add-involvement" class="amc-popup">
        <asp:Panel runat="server" ID="panelPopup" DefaultButton="btnSave">
            <div class="amc-error-message">
                <asp:Label runat="server" ID="lblPopupMessage"></asp:Label>
                <div>
                    <asp:ValidationSummary runat="server" ID="vldsPopupRequiredGroup" ValidationGroup="PopupRequiredGroup" />
                </div>
            </div>
            <table class="width500">
                <tr id="Title" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblTitle"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtTitle"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqTitle" ControlToValidate="txtTitle"
                            EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="FullName" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblFullName"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtFullName"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqFullName" ControlToValidate="txtFullName"
                            EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="Point" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblPoint"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtNumberOfHours"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqPoint" ControlToValidate="txtNumberOfHours"
                            EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtNumberOfHours"
                                resourcekey="NumberValues.Text" ValidationExpression="^[0-9]{1,3}(\.[0-9]{1,9})?$"
                                Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
                <tr id="Role" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblRole"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtRole"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqRole" ControlToValidate="txtRole"
                            EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
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
                <tr id="TypeOfCE" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblTypeOfCE"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlCEType">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqTypeOfCE" ControlToValidate="ddlCEType"
                            EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
            <div class="pad5">
                <asp:Button ID="btnSave" runat="server" Text="OK" ValidationGroup="PopupRequiredGroup" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
            </div>
        </asp:Panel>
    </div>
</div>
<div class="page-break">
</div>
