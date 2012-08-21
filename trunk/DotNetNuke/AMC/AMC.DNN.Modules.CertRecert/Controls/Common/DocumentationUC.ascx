<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="DocumentationUC.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.Common.DocumentationUC" %>
<script type="text/javascript">

    function showPracticePopup(isShow) {
        AmcCert.showPracticePopup("add-documentation-popup", isShow);
        AmcCert.SetTitle("add-documentation-popup", "Add Information ");
    }
    jQuery(document).ready(function () {
        var bindingList = [['lblIssuingBodyString', 'ddlExperienceType'],
                           ['lblExperienceDetail', 'txtExperienceDetail'],
                           ['hlAttachedDocumentName', 'fuUploadFileAttachment'],
                           ['hlAttachedDocumentName', 'hlUploadFileAttachment']
                           ];
        var practicePopup = new AMCTablePopUp(bindingList,
           "documentation-uc",
            "tbldocumentation",
            "add-documentation-popup",
            "Add Information ",
            "Edit Item ",
            null);
    });
</script>
<div id="documentation-uc" class="amc-page">
    <asp:HiddenField runat="server" ID="hdIsIncomplete" />
    <asp:HiddenField runat="server" ID="hdIsValidateFailed" />
    <asp:HiddenField runat="server" ID="hdCurrentObjectUniqueId" />
    <div class="amc-title">
        <asp:Label runat="server" ID="lbldocumentationUC"></asp:Label>
    </div>
    <div id="FormFillingInstructions" runat="server" class="amc-instruction">
        <asp:Label runat="server" ID="lblFormFillingInstructions"></asp:Label>
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
            <asp:Repeater runat="server" ID="rptdocumentation" EnableViewState="True">
                <HeaderTemplate>
                    <table id="tbldocumentation" class="amc-table">
                        <tr class="amc-table-header">
                            <td runat="server" visible='<%# GetFieldInfo("ExperienceType").IsEnabled %>'>
                                <asp:Label runat="server" ID="lblExperienceTypeHeader" Text='<%# GetFieldInfo("ExperienceType").FieldValue %>'></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("ExperienceDetail").IsEnabled %>'>
                                <asp:Label runat="server" ID="lblExperienceDetailHeader" Text='<%# GetFieldInfo("ExperienceDetail").FieldValue %>'></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("UploadFileAttachment").IsEnabled %>'>
                                <asp:Label runat="server" ID="lblAttachedDocumentHeader" resourcekey="AttachDocumentHeader.Text"></asp:Label>
                            </td>
                            <td class="action-column">
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="amc-table-content">
                        <td runat="server" visible='<%# GetFieldInfo("ExperienceType").IsEnabled %>'>
                            <asp:Label runat="server" ID="lblExperienceTypeString"></asp:Label>
                            <asp:Label ID="lblIssuingBodyString" runat="server" CssClass="dn" Text='<%# Eval("IssuingBodyString") %>'></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("ExperienceDetail").IsEnabled %>'>
                            <asp:Label runat="server" ID="lblExperienceDetail" Text='<%# Eval("IssuingBodyText") %>'></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("UploadFileAttachment").IsEnabled %>'>
                            <asp:HiddenField runat="server" ID="hdAttachedDocumentName" />
                            <div class="add-image dn margin-auto" id="imgAddAttachment" runat="server">
                            </div>
                            <asp:HyperLink runat="server" ID="hlAttachedDocumentName" Target="_blank"></asp:HyperLink>
                        </td>
                        <td class="action-column">
                            <asp:HiddenField runat="server" ID="hdObjectUniqueId" Value='<%# Eval("Guid") %>' />
                            <div class="edit-image fl" id="imgEdit">
                            </div>
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
    <div id="add-documentation-popup" class="amc-popup">
        <div>
            <div class="amc-error-message">
                <asp:Label runat="server" ID="lblPopupMessage"></asp:Label>
                <div>
                <asp:ValidationSummary runat="server" ID="vldsPopupRequiredGroup" ValidationGroup="PopupRequiredGroup"/>
            </div>
            </div>
            <table class="width500">
                <tr id="ExperienceType" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblExperienceType"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlExperienceType">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqExperienceType" ControlToValidate="ddlExperienceType"
                            EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="ExperienceDetail" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblExperienceDetail"></asp:Label>
                    </td>
                    <td>
                        <textarea id="txtExperienceDetail" runat="server" maxlength="2000"></textarea>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqExperienceDetail" ControlToValidate="txtExperienceDetail"
                            EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="UploadFileAttachment" runat="server">
                    <td>
                        <asp:Label ID="lblUploadFileAttachment" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:FileUpload ID="fuUploadFileAttachment" runat="server" />
                        <%--<asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator1" ControlToValidate="fuUploadFileAttachment"
                            EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>--%>
                        <asp:CustomValidator runat="server" ID="rqUploadFileAttachment" EnableClientScript="True"
                            Display="Dynamic" ValidationGroup="PopupRequiredGroup" ClientValidationFunction="ValidateUploadFile"
                            ControlToValidate="fuUploadFileAttachment" ValidateEmptyText="True" ErrorMessage="*"></asp:CustomValidator>
                        <br />
                        <asp:RegularExpressionValidator ID="rqPdfFileExtension" ControlToValidate="fuUploadFileAttachment"
                            ValidationExpression="^.*\.(pdf|PDF)$" runat="server" ValidationGroup="PopupRequiredGroup"
                            resourcekey="RequirePDFFile.Text" Display="Dynamic" />
                        <div id="divUploadFileAttachment">
                            <asp:HyperLink runat="server" ID="hlUploadFileAttachment" CssClass="fl" Text="sample.pdf"
                                Target="_blank"></asp:HyperLink>
                            <asp:ImageButton runat="server" ID="imgDeleteAttachment" CssClass="fl" 
                            ImageUrl="../../Documentation/images/icons/delete_icon1.gif" />
                            <asp:HiddenField runat="server" ID="hfDeleteFile" Value="NO" />
                        </div>
                    </td>
                </tr>
            </table>
            <div class="pad5">
                <asp:Button ID="btnSave" runat="server" Text="OK" ValidationGroup="PopupRequiredGroup" />
                <asp:Button runat="server" ID="btnCancel" Text="Cancel" OnClientClick="showPracticePopup(false); return false;" />
            </div>
        </div>
    </div>
</div>
<div class="page-break"></div>
