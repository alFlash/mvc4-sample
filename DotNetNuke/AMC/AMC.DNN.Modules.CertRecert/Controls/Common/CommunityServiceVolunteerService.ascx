<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CommunityServiceVolunteerService.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.Common.CommunityServiceVolunteerService" %>
<%@ Register TagPrefix="amc" TagName="DateTime" Src="../Reusable/AMCDatetime.ascx" %>
<script type="text/javascript">
    function showPracticePopup(isShow) {
        AmcCert.showPracticePopup("add-communityVolunteerService", isShow);
        AmcCert.SetTitle("add-communityVolunteerService", "Add Information ");
    }

    jQuery(document).ready(function () {
        var bindingList = [['lblNameOfOrganization', 'txtNameOfOrganization'],
                           ['lblTypeOfActivity', 'txtTypeOfActivity'],
                           ['lblRole', 'txtRole'],
                           ['lblDate', 'txtDate'],
                           ['lblPoint', 'txtPoint']];

        var practicePopup = new AMCTablePopUp(bindingList,
            "communityVolunteerService-uc",
            "tblCommunityVolunteerService",
            "add-communityVolunteerService",
            "Add Information ",
            "Edit Item",
            null);
    });
</script>
<div id="communityVolunteerService-uc" class="amc-page">
    <asp:HiddenField runat="server" ID="hdIsIncomplete" />
    <asp:HiddenField runat="server" ID="hdIsValidateFailed" />
    <asp:HiddenField runat="server" ID="hdCurrentObjectUniqueId" />
    <div class="amc-title">
        <asp:Label runat="server" ID="lblCommunityServiceVolunteerService"></asp:Label>
    </div>
    <div id="FormFillingInstructions" runat="server" class="amc-instruction">
        <asp:Label runat="server" ID="lblFormFillingInstructions"></asp:Label>
    </div>
    <div class="amc-contents">
        <div id="add-new-row" class="amc-add-instruction">
            <asp:Image CssClass="pointer fl" runat="server" ID="imgAdd" ImageUrl="../../Documentation/images/icons/addnew_11x11.gif">
            </asp:Image>
            <asp:HyperLink runat="server" ID="hlAddNew"  resourcekey="AddInformation.Text" 
                CssClass="fl padleft5" href="javascript:void(0);"></asp:HyperLink>
            <div class="cb">
            </div>
        </div>
        <div>
            <asp:Repeater runat="server" ID="rptCommunityVolunteerService" EnableViewState="True">
                <HeaderTemplate>
                    <table id="tblCommunityVolunteerService" border="1px" class="amc-table">
                        <tr class="amc-table-header">
                            <td visible='<%# GetFieldInfo("NameOfOrganization").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblNameOfOrganizationHeader" Text='<%# GetFieldInfo("NameOfOrganization").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("TypeOfActivity").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblTypeOfActivityHeader" Text='<%# GetFieldInfo("TypeOfActivity").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("Role").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblRoleHeader" Text='<%# GetFieldInfo("Role").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("TermOfOffice").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblDate" Text='<%# GetFieldInfo("TermOfOffice").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("Point").IsEnabled %>' runat="server" >
                                <asp:Label runat="server" ID="lblPointHeader" Text='<%# GetFieldInfo("Point").FieldValue %>'></asp:Label>
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
                        <td visible='<%# GetFieldInfo("NameOfOrganization").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblNameOfOrganization" Text='<%# Eval("OrganizationProviding") %>'></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("TypeOfActivity").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblTypeOfActivity" Text='<%# Eval("ProgramTitle") %>'></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("Role").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblRole" Text='<%# Eval("Role") %>'></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("TermOfOffice").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblDate"></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("Point").IsEnabled %>' runat="server" class="text-right">
                            <asp:Label runat="server" ID="lblPoint" Text=""></asp:Label>
                            <asp:Label runat="server" ID="lblPointValue" Text="" CssClass="dn"></asp:Label>
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
                    <%--                <tr>
                    <td class="width150">
                        <asp:Label runat="server" ID="lblDegreeHeader" resourcekey="OtherEducation.Text"></asp:Label>
                    </td>
                    <td colspan="4" class="text-center">
                        <asp:Image CssClass="pointer" runat="server" ID="imgAdd" ImageUrl="../../Documentation/images/icons/add_icon.gif">
                        </asp:Image>
                    </td>
                </tr>--%>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
    <div class="totalCE">
        Total Points:
        <asp:Label runat="server" ID="lblTotalCE"></asp:Label>
    </div>
    <div id="add-communityVolunteerService" class="amc-popup">
        <asp:HiddenField runat="server" ID="hdnARN"/>
        <asp:HiddenField runat="server" ID="hdnPointValuesDefault"/>
        <asp:Panel runat="server" ID="panelPopup" DefaultButton="btnSave">
            <div class="amc-error-message">
                <asp:Label runat="server" ID="lblPopupMessage"></asp:Label>
                <div>
                    <asp:ValidationSummary runat="server" ID="vldsPopupRequiredGroup" ValidationGroup="PopupRequiredGroup" />
                </div>
            </div>
            <table class="width500">
                <tr id="NameOfOrganization" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblNameOfOrganization"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtNameOfOrganization"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqNameOfOrganization"
                            ControlToValidate="txtNameOfOrganization" EnableClientScript="True" Display="Dynamic"
                            ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="TypeOfActivity" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblTypeOfActivity"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtTypeOfActivity"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqTypeOfActivity"
                            ControlToValidate="txtTypeOfActivity" EnableClientScript="True" Display="Dynamic"
                            ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
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
                <tr id="TermOfOffice" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblTermOfOffice"></asp:Label>
                    </td>
                    <td>
                        <amc:DateTime runat="server" ID="txtDate" ValidationGroup="PopupRequiredGroup" CssClass="fl">
                        </amc:DateTime>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqTermOfOffice" ControlToValidate="txtDate$txtDatetime"
                            CssClass="fl" EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
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
                        <asp:TextBox runat="server" CssClass="width250" ID="txtPoint"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqPoint" ControlToValidate="txtPoint"
                            EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtPoint"
                                resourcekey="NumberValues.Text" ValidationExpression="^[0-9]{1,3}(\.[0-9]{1,9})?$"
                                Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RegularExpressionValidator>
                        </div>
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
