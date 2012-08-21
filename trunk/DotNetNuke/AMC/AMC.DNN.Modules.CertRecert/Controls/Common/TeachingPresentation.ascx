<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TeachingPresentation.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.Common.TeachingPresentation" %>
<%@ Register TagPrefix="amc" TagName="DateTime" Src="../../Controls/Reusable/AMCDatetime.ascx" %>
<script type="text/javascript">
    function showPracticePopup(isShow) {
        AmcCert.showPracticePopup("add-communityPresentation", isShow);
        AmcCert.SetTitle("add-communityPresentation", "Add Information ");
    }

    jQuery(document).ready(function () {
        var bindingList = [['lblNameOfProgram', 'txtNameOfProgram'],
                           ['lblTitlePresentation', 'txtTitlePresentation'],
                           ['lbldate', 'txtDate'],
                           ['lblpoint', 'txtPoint'],
                            ['lblAudience', 'txtAudience'],
                            ['lblCEValueType', 'ddlCEType']];

        var practicePopup = new AMCTablePopUp(bindingList,
            "communityPresentation-uc",
            "tblCommunityPresentation",
            "add-communityPresentation",
            "Add Information ",
            "Edit Item",
            null);
    });
</script>
<div id="communityPresentation-uc" class="amc-page">
    <asp:HiddenField runat="server" ID="hdIsIncomplete" />
    <asp:HiddenField runat="server" ID="hdIsValidateFailed" />
    <asp:HiddenField runat="server" ID="hdCurrentObjectUniqueId" />
    <div class="amc-title">
        <asp:Label runat="server" ID="lblTeachingPresentation"></asp:Label>
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
            <asp:HyperLink runat="server" ID="hlAddNew"  resourcekey="AddInformation.Text" 
                CssClass="fl padleft5" href="javascript:void(0);"></asp:HyperLink>
            <div class="cb">
            </div>
        </div>
        <div>
            <asp:Repeater runat="server" ID="rptCommunityPresentation" EnableViewState="True">
                <HeaderTemplate>
                    <table id="tblCommunityPresentation" border="1px" class="amc-table">
                        <tr class="amc-table-header">
                            <td visible='<%# GetFieldInfo("NameOfProgram").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblNameOfProgramHeader" Text='<%# GetFieldInfo("NameOfProgram").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("TitlePresentation").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblTitlePresentationHeader" Text='<%# GetFieldInfo("TitlePresentation").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("Date").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lbldateHeader" Text='<%# GetFieldInfo("Date").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("Point").IsEnabled %>' runat="server" >
                                <asp:Label runat="server" ID="lblpointHeader" Text='<%# GetFieldInfo("Point").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("Audience").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblAudienceHeader" Text='<%# GetFieldInfo("Audience").FieldValue %>'></asp:Label>
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
                        <td visible='<%# GetFieldInfo("NameOfProgram").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblNameOfProgram" Text='<%# Eval("OrganizationProviding") %>'></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("TitlePresentation").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblTitlePresentation" Text='<%# Eval("ProgramTitle") %>'></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("Date").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lbldate"></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("Point").IsEnabled %>' runat="server" class="text-right">
                            <asp:Label runat="server" ID="lblpoint" ></asp:Label>
                            <asp:Label runat="server" ID="lblPointValue" Text="" CssClass="dn"></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("Audience").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblAudience" Text='<%# Eval("Audience") %>'></asp:Label>
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
    <div id="add-communityPresentation" class="amc-popup">
        <asp:Panel runat="server" ID="panelPopup" DefaultButton="btnSave">
            <div class="amc-error-message">
                <asp:Label runat="server" ID="lblPopupMessage"></asp:Label>
                <div>
                    <asp:ValidationSummary runat="server" ID="vldsPopupRequiredGroup" ValidationGroup="PopupRequiredGroup" />
                </div>
            </div>
            <table class="width500">
                <tr id="NameOfProgram" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblNameOfProgram"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtNameOfProgram"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqNameOfProgram"
                            ControlToValidate="txtNameOfProgram" EnableClientScript="True" ValidationGroup="PopupRequiredGroup"
                            Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="TitlePresentation" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblTitlePresentation"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtTitlePresentation"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqTitlePresentation"
                            ControlToValidate="txtTitlePresentation" EnableClientScript="True" ValidationGroup="PopupRequiredGroup"
                            Display="Dynamic"></asp:RequiredFieldValidator>
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
                            EnableClientScript="True" ValidationGroup="PopupRequiredGroup" Display="Dynamic"
                            CssClass="fl"></asp:RequiredFieldValidator>
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
                            EnableClientScript="True" ValidationGroup="PopupRequiredGroup" Display="Dynamic"></asp:RequiredFieldValidator>
                        <div class="cb">
                        </div>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtPoint"
                                resourcekey="NumberValues.Text" ValidationExpression="^[0-9]{1,3}(\.[0-9]{1,9})?$"
                                Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
                <tr id="Audience" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblAudience"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtAudience"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqAudience" ControlToValidate="txtAudience"
                            EnableClientScript="True" ValidationGroup="PopupRequiredGroup" Display="Dynamic"></asp:RequiredFieldValidator>
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
