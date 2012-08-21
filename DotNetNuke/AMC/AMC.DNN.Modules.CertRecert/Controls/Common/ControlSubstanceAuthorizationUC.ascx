<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ControlSubstanceAuthorizationUC.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.Common.ControlSubstanceAuthorizationUC" %>
<%@ Register TagPrefix="amc" TagName="DateTime" Src="../Reusable/AMCDatetime.ascx" %>
<!-- control javascript -->
<script type="text/javascript">


    jQuery(document).ready(function () {
        var bindingList = [['lblStateType', 'ddlState'],
                           ['lblAuthorNumber', 'txtAuthorNumber'],
                           ['lblExpirDate', 'txtExpirationDate'],
                           ['lblIssueDate', 'txtDateOfOriginalIssue'],
                           ['hlAttachedDocumentName', 'fuUploadFileAttachment'],
                           ['hlAttachedDocumentName', 'hlUploadFileAttachment']];

        var practicePopup = new AMCTablePopUp(bindingList,
            "uc-DAE",
            "substance-authorization-table",
            "popupAddNewDAE",
            "Add Information ",
            "Edit Item",
            null);
    });
</script>
<div id="uc-DAE" class="amc-page">
    <asp:HiddenField runat="server" ID="hdIsIncomplete" />
    <asp:HiddenField runat="server" ID="hdIsValidateFailed" />
    <asp:HiddenField runat="server" ID="hdCurrentObjectUniqueId" />
    <div class="amc-title">
        <asp:Label ID="lblControlSubstanceAuthorizationUC" runat="server"></asp:Label>
    </div>
    <div id="FormFillingInstructions" runat="server" class="amc-instruction">
        <asp:Label runat="server" ID="lblFormFillingInstructions"></asp:Label>
    </div>
    <div class="amc-contents">
        <div id="add-new-row" class="amc-add-instruction">
            <asp:Image CssClass="pointer fl" runat="server" ID="imgAdd" ImageUrl="../../Documentation/images/icons/addnew_11x11.gif">
            </asp:Image>
            <asp:HyperLink runat="server" ID="hlAddNew" resourcekey="AddInformation.Text" CssClass="fl padleft5"
                href="javascript:void(0);"></asp:HyperLink>
            <div class="cb">
            </div>
        </div>
        <asp:Repeater ID="rptControlSubstance" runat="server" EnableViewState="True">
            <HeaderTemplate>
                <table id="substance-authorization-table" border="1px" class="amc-table">
                    <tr class="amc-table-header">
                        <td runat="server" visible='<%# GetFieldInfo("State").IsEnabled %>'>
                            <asp:Label ID="lblStateHeader" runat="server" Text='<%# GetFieldInfo("State").FieldValue %>'></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("AuthorNumber").IsEnabled %>'>
                            <asp:Label ID="lblAuthorNumberHeader" runat="server" Text='<%# GetFieldInfo("AuthorNumber").FieldValue %>'></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("ExpirationDate").IsEnabled %>'>
                            <asp:Label ID="lblExpirDateHeader" runat="server" Text='<%# GetFieldInfo("ExpirationDate").FieldValue %>'></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("DateOfOriginalIssue").IsEnabled %>'>
                            <asp:Label ID="lblIssueDateHeader" runat="server" Text='<%# GetFieldInfo("DateOfOriginalIssue").FieldValue %>'></asp:Label>
                        </td>
                        <td class="add-column" runat="server" visible='<%# GetFieldInfo("UploadFileAttachment").IsEnabled %>'>
                            <asp:Label ID="lblAttachHeader" runat="server" Text='<%# GetFieldInfo("UploadFileAttachment").FieldValue %>'></asp:Label>
                        </td>
                        <td class="action-column">
                        </td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td runat="server" visible='<%# GetFieldInfo("State").IsEnabled %>'>
                        <asp:Label ID="lblState" runat="server"></asp:Label>
                        <asp:Label ID="lblStateType" runat="server" CssClass="dn" Text='<%# Eval("IssuingBodyString") %>'></asp:Label>
                    </td>
                    <td runat="server" visible='<%# GetFieldInfo("AuthorNumber").IsEnabled %>'>
                        <asp:Label ID="lblAuthorNumber" runat="server" Text='<%# Eval("IssuedNumber") %>'></asp:Label>
                    </td>
                    <td runat="server" visible='<%# GetFieldInfo("ExpirationDate").IsEnabled %>'>
                        <asp:Label ID="lblExpirDate" runat="server"></asp:Label>
                    </td>
                    <td runat="server" visible='<%# GetFieldInfo("DateOfOriginalIssue").IsEnabled %>'>
                        <asp:Label ID="lblIssueDate" runat="server"></asp:Label>
                    </td>
                    <td class="text-center" runat="server" visible='<%# GetFieldInfo("UploadFileAttachment").IsEnabled %>'>
                        <asp:HiddenField runat="server" ID="hdAttachedDocumentName" />
                        <div class="add-image dn margin-auto" id="imgAddAttachment" runat="server">
                        </div>
                        <asp:HyperLink runat="server" ID="hlAttachedDocumentName" Target="_blank"></asp:HyperLink>
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
<!-- Popup -->
<div id="popupAddNewDAE" class="amc-popup">
    <asp:Panel runat="server" ID="panelPopup" DefaultButton="btnSave">
        <div>
            <div class="amc-error-message">
                <asp:Label runat="server" ID="lblPopupMessage"></asp:Label>
                <div>
                    <asp:ValidationSummary runat="server" ID="vldsPopupRequiredGroup" ValidationGroup="PopupRequiredGroup" />
                </div>
            </div>
            <table>
                <tr id="State" runat="server">
                    <td>
                        <asp:Label ID="lblState" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlState">
                        </asp:DropDownList>
                        <%-- <asp:TextBox ID="txtState" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqState" ControlToValidate="txtState"
                        EnableClientScript="True"EnableClientScript="True"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr id="AuthorNumber" runat="server">
                    <td>
                        <asp:Label ID="lblAuthorNumber" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAuthorNumber" runat="server" CssClass="width250"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqAuthorNumber" ControlToValidate="txtAuthorNumber"
                            EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="ExpirationDate" runat="server">
                    <td>
                        <asp:Label ID="lblExpirationDate" runat="server"></asp:Label>
                    </td>
                    <td>
                        <amc:DateTime runat="server" ID="txtExpirationDate" ValidationGroup="PopupRequiredGroup"
                            CssClass="fl"></amc:DateTime>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqExpirationDate"
                            ControlToValidate="txtExpirationDate$txtDatetime" CssClass="fl" EnableClientScript="True"
                            Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                        <div class="cb">
                        </div>
                        <div>
                            <asp:CompareValidator ID="Compare1" ControlToValidate="txtExpirationDate$txtDatetime"
                                ControlToCompare="txtDateOfOriginalIssue$txtDatetime" resourcekey="CompareDateControlSubs.Text"
                                Type="Date" runat="server" Display="Dynamic" Operator="GreaterThanEqual" ValidationGroup="PopupRequiredGroup" />
                        </div>
                    </td>
                </tr>
                <tr id="DateOfOriginalIssue" runat="server">
                    <td>
                        <asp:Label ID="lblDateOfOriginalIssue" runat="server"></asp:Label>
                    </td>
                    <td>
                        <amc:DateTime runat="server" ID="txtDateOfOriginalIssue" ValidationGroup="PopupRequiredGroup"
                            CssClass="fl"></amc:DateTime>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqDateOfOriginalIssue"
                            ControlToValidate="txtDateOfOriginalIssue$txtDatetime" CssClass="fl" EnableClientScript="True"
                            Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                        <div class="cb">
                            <asp:CompareValidator runat="server" ID="cpWithToday" ValidationGroup="PopupRequiredGroup"
                                Type="Date" ControlToValidate="txtDateOfOriginalIssue$txtDatetime" Display="Dynamic"
                                Operator="LessThanEqual" resourcekey="cpWithToday.Text">
                            </asp:CompareValidator>
                        </div>
                    </td>
                </tr>
                <tr id="UploadFileAttachment" runat="server">
                    <td>
                        <asp:Label ID="lblUploadFileAttachment" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:FileUpload ID="fuUploadFileAttachment" runat="server" CssClass="fl" />
                        <asp:CustomValidator runat="server" ID="rqUploadFileAttachment" EnableClientScript="True"
                            Display="Dynamic" ValidationGroup="PopupRequiredGroup" ClientValidationFunction="ValidateUploadFile"
                            ControlToValidate="fuUploadFileAttachment" ValidateEmptyText="True" ErrorMessage="*">
                        </asp:CustomValidator>
                        <br />
                        <asp:RegularExpressionValidator ID="rqPdfFileExtension" ControlToValidate="fuUploadFileAttachment"
                            ValidationExpression="^.*\.(pdf|PDF)$" runat="server" ValidationGroup="PopupRequiredGroup"
                            resourcekey="RequirePDFFile.Text" Display="Dynamic" />
                        <div id="divUploadFileAttachment">
                            <asp:HyperLink runat="server" ID="hlUploadFileAttachment" CssClass="fl" Text="sample.pdf"
                                Target="_blank"></asp:HyperLink>
                            <asp:ImageButton runat="server" ID="imgDeleteAttachment" CssClass="fl" ImageUrl="../../Documentation/images/icons/delete_icon1.gif" />
                            <asp:HiddenField runat="server" ID="hfDeleteFile" Value="NO" />
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
        </div>
    </asp:Panel>
</div>
<div class="page-break">
</div>
