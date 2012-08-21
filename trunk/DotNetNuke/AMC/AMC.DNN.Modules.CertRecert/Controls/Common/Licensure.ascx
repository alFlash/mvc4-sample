<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Licensure.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.Common.Licensure" EnableViewState="true" %>
<%@ Register TagPrefix="amc" TagName="DateTime" Src="../Reusable/AMCDatetime.ascx" %>
<%--<script type="text/vbscript">
    Sub ValidateUploadFile(sender, args) {
        args.IsValid = false;
        if (args.Value != "") {
            args.IsValid = false;

        }        
    }
</script>--%>
<script type="text/javascript">

    jQuery(document).ready(function () {
        //add new licensure
        var bindingList = [['lblStateType', 'ddlState'],
                           ['lblLicenseNumber', 'txtLicenseNumber'],
                           ['lblExpirationDate', 'txtExpirationDate'],
                           ['lblDateOfOriginalIssue', 'txtDateOfOriginalIssue'],
                           ['hlAttachedDocumentName', 'fuUploadFileAttachment'],
                           ['hlAttachedDocumentName', 'hlUploadFileAttachment']];

        var practicePopup = new AMCTablePopUp(bindingList,
            "uc-licensure",
            "tbl-licensure",
            "popupAddLicensure",
            "Add Information ",
            "Edit Item",
            null);

        if (jQuery('#[id*=hdCurrentSectionPopupOpenningId]').val() != '' && jQuery('#[id*=hdCurrentSectionPopupOpenningId]').val() != null
        && jQuery('#[id*=hdIsValidateFailed]').val() == '1') {
            if (!IsStringNullOrEmpty(jQuery('#[id*=hdCurrentObjectUniqueId]').val())) {
                var currentRow = jQuery('[guid=' + jQuery('#[id*=hdCurrentObjectUniqueId]').val() + ']');
                if (currentRow.length > 0) {
                    AMCTablePopUp.FillData(currentRow, jQuery('#[id=' + jQuery('#[id*=hdCurrentSectionPopupOpenningId]').val() + ']'), [['hlAttachedDocumentName', 'fuUploadFileAttachment'],
                           ['hlAttachedDocumentName', 'hlUploadFileAttachment']]);
                }
            }
        }
    });
    
</script>
<div id="uc-licensure" class="amc-page">
    <asp:HiddenField runat="server" ID="hdIsIncomplete" />
    <asp:HiddenField runat="server" ID="hdIsValidateFailed" />
    <asp:HiddenField runat="server" ID="hdCurrentObjectUniqueId" />
    <div class="amc-title">
        <asp:Label ID="lblLicensure" runat="server"></asp:Label>
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
        <asp:Repeater ID="rptLicensure" runat="server" EnableViewState="True">
            <HeaderTemplate>
                <table id="tbl-licensure" border="1px" class="amc-table">
                    <tr class="amc-table-header">
                        <td runat="server" visible='<%# GetFieldInfo("StateProvince").IsEnabled %>'>
                            <asp:Label ID="Label1" runat="server" Text='<%# GetFieldInfo("StateProvince").FieldValue %>'></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("LicenseNumber").IsEnabled %>'>
                            <asp:Label ID="Label2" runat="server" Text='<%# GetFieldInfo("LicenseNumber").FieldValue %>'></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("ExpirationDate").IsEnabled %>'>
                            <asp:Label ID="Label3" runat="server" Text='<%# GetFieldInfo("ExpirationDate").FieldValue %>'></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("DateOfOriginalIssue").IsEnabled %>'>
                            <asp:Label ID="Label4" runat="server" Text='<%# GetFieldInfo("DateOfOriginalIssue").FieldValue %>'></asp:Label>
                        </td>
                        <td class="add-column" runat="server" visible='<%# GetFieldInfo("UploadFileAttachment").IsEnabled %>'>
                            <asp:Label ID="Label5" runat="server" Text='<%# GetFieldInfo("UploadFileAttachment").FieldValue %>'></asp:Label>
                        </td>
                        <td class="action-column">
                        </td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="amc-table-content" id="item_" runat="server">
                    <td runat="server" visible='<%# GetFieldInfo("StateProvince").IsEnabled %>'>
                        <asp:Label ID="lblStateProvince" runat="server"></asp:Label>
                        <asp:Label ID="lblStateType" runat="server" CssClass="dn" Text='<%# Eval("IssuingBodyString") %>'></asp:Label>
                    </td>
                    <td runat="server" visible='<%# GetFieldInfo("LicenseNumber").IsEnabled %>'>
                        <asp:Label ID="lblLicenseNumber" runat="server" Text='<%# Eval("IssuedNumber") %>'></asp:Label>
                    </td>
                    <td runat="server" visible='<%# GetFieldInfo("ExpirationDate").IsEnabled %>'>
                        <asp:Label ID="lblExpirationDate" runat="server"></asp:Label>
                    </td>
                    <td runat="server" visible='<%# GetFieldInfo("DateOfOriginalIssue").IsEnabled %>'>
                        <asp:Label ID="lblDateOfOriginalIssue" runat="server"></asp:Label>
                    </td>
                    <td runat="server" visible='<%# GetFieldInfo("UploadFileAttachment").IsEnabled %>'>
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
<div id="popupAddLicensure" class="amc-popup">
    <asp:Panel runat="server" ID="panelPopup" DefaultButton="btnSave">
        <div class="amc-error-message">
            <asp:Label runat="server" ID="lblPopupMessage"></asp:Label>
            <div>
                <asp:ValidationSummary runat="server" ID="vldsPopupRequiredGroup" ValidationGroup="PopupRequiredGroup" />
            </div>
        </div>
        <table>
            <tr id="StateProvince" runat="server">
                <td>
                    <asp:Label ID="lblStateProvince" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlState">
                    </asp:DropDownList>
                    <%--<asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqStateProvince" ControlToValidate="ddlState" EnableClientScript="True"
               ></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr id="LicenseNumber" runat="server">
                <td>
                    <asp:Label ID="lblLicenseNumber" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtLicenseNumber" runat="server" CssClass="width250"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqLicenseNumber"
                        ControlToValidate="txtLicenseNumber" EnableClientScript="True" Display="Dynamic"
                        ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="ExpirationDate" runat="server" validationgroup="PopupRequiredGroup">
                <td>
                    <asp:Label ID="lblExpirationDate" runat="server"></asp:Label>
                </td>
                <td>
                    <amc:DateTime runat="server" ID="txtExpirationDate" ValidationGroup="PopupRequiredGroup"
                        CssClass="fl"></amc:DateTime>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqExpirationDate"
                        ControlToValidate="txtExpirationDate$txtDatetime" CssClass="fl" EnableClientScript="True"
                        Display="Dynamic" ValidationGroup="PopupRequiredGroup">
                    </asp:RequiredFieldValidator>
                    <div class="cb">
                    </div>
                    <div>
                        <asp:CompareValidator ID="Compare1" ControlToValidate="txtExpirationDate$txtDatetime"
                            ControlToCompare="txtDateOfOriginalIssue$txtDatetime" resourcekey="CompareDateLicensure.Text"
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
                        Display="Dynamic" ValidationGroup="PopupRequiredGroup">
                    </asp:RequiredFieldValidator>
                    <div class="cb">
                    </div>
                    <div>
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
                    <asp:FileUpload ID="fuUploadFileAttachment" runat="server"  CssClass="fl"/>
                    <%-- <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqUploadFileAttachment" ControlToValidate="fuUploadFileAttachment"
                    EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>--%>
                    <asp:CustomValidator runat="server" ID="rqUploadFileAttachment" EnableClientScript="True"
                        Display="Dynamic" ValidationGroup="PopupRequiredGroup" CssClass="fl" ClientValidationFunction="ValidateUploadFile"
                        ControlToValidate="fuUploadFileAttachment" ValidateEmptyText="True" ErrorMessage="*">
                    </asp:CustomValidator>
                    <div class="cb">
                        <asp:RegularExpressionValidator ID="rqPdfFileExtension" ControlToValidate="fuUploadFileAttachment"
                            ValidationExpression="^.*\.(pdf|PDF)$" runat="server" ValidationGroup="PopupRequiredGroup"
                            resourcekey="RequirePDFFile.Text" Display="Dynamic" />
                    </div>
                    <div id="divUploadFileAttachment" class="cb">
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
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClientClick="AmcCert.ShowPopUp('popupAddLicensure', false);return false;" />
                    <br />
                </td>
            </tr>
        </table>
    </asp:Panel>
</div>
<div class="page-break">
</div>
