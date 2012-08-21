<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CommunityServicePubication.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.Common.CommunityServicePubication" %>
<%@ Register TagPrefix="amc" TagName="DateTime" Src="../Reusable/AMCDatetime.ascx" %>
<script type="text/javascript">

    function showPracticePopup(isShow) {
        AmcCert.showPracticePopup("add-communityPublication", isShow);
        AmcCert.SetTitle("add-communityPublication", "Add Information ");
    }

    jQuery(document).ready(function () {
        var bindingList = [['lblNameOfPublisher', 'txtNameOfPublisher'],
                           ['lblTitlePublication', 'txtTitlePublication'],
                           ['lbldate', 'txtDate'],
                           ['lblPages', 'txtPages'],
                           ['lblTitleManuscript', 'txtTitleManuscript'],
                           ['lblpoint', 'txtPoint']
                           ];

        var practicePopup = new AMCTablePopUp(bindingList,
            "communityPublication-uc",
            "tblCommunityPublication",
            "add-communityPublication",
            "Add Information ",
            "Edit Item",
            null);
    });
</script>
<div id="communityPublication-uc" class="amc-page">
    <asp:HiddenField runat="server" ID="hdIsIncomplete" />
    <asp:HiddenField runat="server" ID="hdIsValidateFailed" />
    <asp:HiddenField runat="server" ID="hdCurrentObjectUniqueId" />
    <div class="amc-title">
        <asp:Label runat="server" ID="lblCommunityServicePubication"></asp:Label>
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
            <asp:Repeater runat="server" ID="rptCommunityPublication" EnableViewState="True">
                <HeaderTemplate>
                    <table id="tblCommunityPublication" border="1px" class="amc-table">
                        <tr class="amc-table-header">
                            <td visible='<%# GetFieldInfo("NameOfPublisher").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblNameOfPublisherHeader" Text='<%# GetFieldInfo("NameOfPublisher").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("TitlePublication").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblTitlePublicationHeader" Text='<%# GetFieldInfo("TitlePublication").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("Date").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lbldateHeader" Text='<%# GetFieldInfo("Date").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("Pages").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblPagesHeader" Text='<%# GetFieldInfo("Pages").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("TitleManuscript").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblTitleManuscriptHeader" Text='<%# GetFieldInfo("TitleManuscript").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("Point").IsEnabled %>' runat="server" >
                                <asp:Label runat="server" ID="lblpointHeader" Text='<%# GetFieldInfo("Point").FieldValue %>'></asp:Label>
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
                        <td visible='<%# GetFieldInfo("NameOfPublisher").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblNameOfPublisher" Text='<%# Eval("OrganizationProviding") %>'></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("TitlePublication").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblTitlePublication" Text='<%# Eval("PublicationTitle") %>'></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("Date").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lbldate"></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("Pages").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblPages" Text='<%# Eval("ArticlePage") %>'></asp:Label>
                        </td>
                        <td id="Td1" visible='<%# GetFieldInfo("TitleManuscript").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblTitleManuscript" Text='<%# Eval("ProgramTitle") %>'></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("Point").IsEnabled %>' runat="server" class="text-right">
                            <asp:Label runat="server" ID="lblpoint" Text=""></asp:Label>
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
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
    <div class="totalCE">
        Total Points:
        <asp:Label runat="server" ID="lblTotalCE"></asp:Label>
    </div>
    <div id="add-communityPublication" class="amc-popup">
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
                <tr id="NameOfPublisher" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblNameOfPublisher"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtNameOfPublisher"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqNameOfPublisher"
                            ControlToValidate="txtNameOfPublisher" EnableClientScript="True" ValidationGroup="PopupRequiredGroup"
                            Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="TitlePublication" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblTitlePublication"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtTitlePublication"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqTitlePublication"
                            ControlToValidate="txtTitlePublication" EnableClientScript="True" ValidationGroup="PopupRequiredGroup"
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
                            CssClass="fl" EnableClientScript="True" ValidationGroup="PopupRequiredGroup"
                            Display="Dynamic"></asp:RequiredFieldValidator>
                        <div class="cb">
                            <asp:CustomValidator runat="server" ID="cusValidOtherBoard" ControlToValidate="txtDate$txtDatetime"
                                ClientValidationFunction="ValidateReCertCircle" Display="Dynamic" ValidationGroup="PopupRequiredGroup"
                                resourcekey="InvalidTimeFrame.Text" EnableClientScript="True" ValidateEmptyText="True"></asp:CustomValidator>
                        </div>
                    </td>
                </tr>
                <tr id="Pages" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblPages"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtPages"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqPages" ControlToValidate="txtPages"
                            EnableClientScript="True" ValidationGroup="PopupRequiredGroup" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="TitleManuscript" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblTitleManuscript"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtTitleManuscript"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqTitleManuscript"
                            ControlToValidate="txtTitleManuscript" EnableClientScript="True" ValidationGroup="PopupRequiredGroup"
                            Display="Dynamic"></asp:RequiredFieldValidator>
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
