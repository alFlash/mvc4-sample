<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EducationUC.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.Common.EducationUC" %>
<%@ Register TagPrefix="amc" TagName="DateTime" Src="../Reusable/AMCDatetime.ascx" %>
<script type="text/javascript">

    function showPracticePopup(isShow) {
        AmcCert.showPracticePopup("add-education-popup", isShow);
        AmcCert.SetTitle("add-education-popup", "Add Information ");
    }

    jQuery(document).ready(function () {
        var bindingList = [['lblInstitudeName', 'txtInstitudeName'],
                           ['lblProgTypevalues', 'ddlProgType'],
                           ['lblDate', 'txtDate'],
                           ['lblEndDate', 'txtEndDate'],
                           ['lblDegreeValue', 'ddlDegree'],
                           ['hlAttachedDocumentName', 'fuUploadFileAttachment'],
                           ['hlAttachedDocumentName', 'hlUploadFileAttachment'],
                           ['lblComment', 'txtComment']];
        var practicePopup = new AMCTablePopUp(bindingList,
           "education-uc",
            "tblEducation",
            "add-education-popup",
            "Add Information ",
            "Edit Item",
            null);
    });


    function EducationUc_ValidateComment(sender, args) {
        args.IsValid = true;
        var startDateTextBox = jQuery('#add-education-popup #[id*=txtDate] #[id*=txtDatetime]');
        var startDate = new Date(Date.parse(startDateTextBox.val()));
        var expRows = jQuery('#tblEducation tr.amc-table-content');
        var commentTestBox = jQuery('#add-education-popup #[id*=txtComment]');
        for (var i = 0; i < expRows.length; i++) {
            var currentRow = expRows[i];
            var endDateLable = jQuery(currentRow).find('#[id*=lblEndDate]');
            var endDate = new Date(Date.parse(endDateLable.text()));
            var uniqueId = jQuery(currentRow).find('#[id*=hdObjectUniqueId]').val();
            var currentUniqueId = jQuery('#[id*=hdCurrentObjectUniqueId]').val();
            if (uniqueId != currentUniqueId) {
                if ((startDate.getYear() > (endDate.getYear() + 1)) || (startDate.getYear() < (endDate.getYear() - 1))
                    || ((startDate.getYear() == (endDate.getYear() - 1)) && (((endDate.getMonth() + (11 - startDate.getMonth())) > 2) || (((endDate.getMonth() + (11 - startDate.getMonth())) == 2) && (endDate.getDate() > startDate.getDate()))))
                    || ((startDate.getYear() == (endDate.getYear() + 1)) && (((startDate.getMonth() + (11 - endDate.getMonth())) > 2) || (((startDate.getMonth() + (11 - endDate.getMonth())) == 2) && (startDate.getDate() > endDate.getDate()))))
                    || (startDate.getYear() == endDate.getYear() && (startDate.getMonth() - endDate.getMonth() > 2 || endDate.getMonth() - startDate.getMonth() > 2 || (startDate.getMonth() - endDate.getMonth() == 2 && (startDate.getDate() > endDate.getDate()))))) {
                    if (IsStringNullOrEmpty(commentTestBox.val())) {
                        args.IsValid = false;
                        break;
                    }
                }
            }
        }
    }
</script>
<div id="education-uc" class="amc-page">
    <asp:HiddenField runat="server" ID="hdIsIncomplete" />
    <asp:HiddenField runat="server" ID="hdIsValidateFailed" />
    <asp:HiddenField runat="server" ID="hdCurrentObjectUniqueId" />
    <div class="optiontitle">
        <asp:Label runat="server" ID="lblErrorMessage" ForeColor="red"></asp:Label>
    </div>
    <div class="amc-title">
        <asp:Label runat="server" ID="lblEducationUC"></asp:Label>
    </div>
    <div id="FormFillingInstructions" runat="server" class="amc-instruction">
        <asp:Label runat="server" ID="lblFormFillingInstructions"></asp:Label>
    </div>
    <div class="amc-contents">
        <div id="add-new-row" class="amc-add-instruction">
            <asp:Image CssClass="pointer fl" runat="server" ID="imgAdd" ImageUrl="../../Documentation/images/icons/addnew_11x11.gif">
            </asp:Image>
            <asp:HyperLink runat="server" ID="hlAddNew" resourcekey="AddInformation.Text" CssClass="fl padleft5" href="javascript:void(0);"></asp:HyperLink>
            <%--hlAddNew--%>
            <div class="cb">
            </div>
        </div>
        <div>
            <asp:Repeater runat="server" ID="rptEducation" EnableViewState="True">
                <HeaderTemplate>
                    <table id="tblEducation" class="amc-table">
                        <tr class="amc-table-header">
                            <td runat="server" visible='<%# GetFieldInfo("InstitudeName").IsEnabled %>'>
                                <asp:Label runat="server" ID="lblInstitudeNameHeader" Text='<%# GetFieldInfo("InstitudeName").FieldValue %>'></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("ProgType").IsEnabled %>'>
                                <asp:Label runat="server" ID="lblProgTypeHeader" Text='<%# GetFieldInfo("ProgType").FieldValue %>'></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("Date").IsEnabled %>'>
                                <asp:Label runat="server" ID="lblDateHeader" Text='<%# GetFieldInfo("Date").FieldValue %>'></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("EndDate").IsEnabled %>'>
                                <asp:Label runat="server" ID="lblEndDateHeader" Text='<%# GetFieldInfo("EndDate").FieldValue %>'></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("Degree").IsEnabled %>'>
                                <asp:Label runat="server" ID="lblDegreeHeader" Text='<%# GetFieldInfo("Degree").FieldValue %>'></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("UploadFileAttachment").IsEnabled %>'>
                                <asp:Label runat="server" ID="lblAttachedDocumentHeader" resourcekey="AttachDocumentHeader.Text"></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("Comment").IsEnabled %>'>
                                <asp:Label runat="server" ID="lblCommentHeader" Text='<%# GetFieldInfo("Comment").FieldValue %>'></asp:Label>
                            </td>
                            <td class="action-column">
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="amc-table-content">
                        <td runat="server" visible='<%# GetFieldInfo("InstitudeName").IsEnabled %>'>
                            <asp:Label runat="server" ID="lblInstitudeName" Text='<%# Eval("InstitutionName") %>'></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("ProgType").IsEnabled %>'>
                            <asp:Label runat="server" ID="lblProgType"></asp:Label>
                            <asp:Label runat="server" ID="lblProgTypevalues" CssClass="dn"></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("Date").IsEnabled %>'>
                            <asp:Label runat="server" ID="lblDate"></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("EndDate").IsEnabled %>'>
                            <asp:Label runat="server" ID="lblEndDate"></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("Degree").IsEnabled %>'>
                            <asp:Label runat="server" ID="lblDegree" Text=""></asp:Label>
                            <asp:Label ID="lblDegreeValue" runat="server" Text="" CssClass="dn"></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("UploadFileAttachment").IsEnabled %>'>
                            <asp:HiddenField runat="server" ID="hdAttachedDocumentName" />
                            <div class="add-image dn margin-auto" id="imgAddAttachment" runat="server">
                            </div>
                            <asp:HyperLink runat="server" ID="hlAttachedDocumentName" Target="_blank"></asp:HyperLink>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("Comment").IsEnabled %>'>
                            <asp:Label runat="server" ID="lblComment" Text='<%# Eval("Comments") %>'></asp:Label>
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
    <div id="add-education-popup" class="amc-popup">
        <asp:Panel runat="server" ID="panelPopup" DefaultButton="btnSave">
        <div>
            <div class="amc-error-message">
                <asp:Label runat="server" ID="lblPopupMessage"></asp:Label>
                <div>
                    <asp:ValidationSummary runat="server" ID="vldsPopupRequiredGroup" ValidationGroup="PopupRequiredGroup" />
                </div>
            </div>
            <table class="width500">
                <tr id="InstitudeName" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblInstitudeName"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtInstitudeName"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqInstitudeName"
                            ControlToValidate="txtInstitudeName" EnableClientScript="True" Display="Dynamic"
                            ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="ProgType" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblProgType"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlProgType">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqProgType" ControlToValidate="ddlProgType"
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
                            CssClass="fl" EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="EndDate" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblEndDate"></asp:Label>
                    </td>
                    <td>
                        <amc:DateTime runat="server" ID="txtEndDate" ValidationGroup="PopupRequiredGroup" CssClass="fl">
                        </amc:DateTime>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqEndDate" ControlToValidate="txtEndDate$txtDatetime"
                            EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup" CssClass="fl"></asp:RequiredFieldValidator>
                        <div class="cb">
                            <asp:CompareValidator ID="Compare1" ControlToValidate="txtEndDate$txtDatetime" ControlToCompare="txtDate$txtDatetime"
                                resourcekey="CompareDateCommon.Text" Type="Date" runat="server"
                                Display="Dynamic" Operator="GreaterThanEqual" ValidationGroup="PopupRequiredGroup" />
                        </div>
                    </td>
                </tr>
                <tr id="Degree" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblDegree"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlDegree">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqDegree" ControlToValidate="ddlDegree"
                            EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
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
                            ControlToValidate="fuUploadFileAttachment" ValidateEmptyText="True" ErrorMessage="*"></asp:CustomValidator>
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
                <tr id="Comment" runat="server">
                    <td>
                        <asp:Label ID="lblComment" runat="server"></asp:Label>
                    </td>
                    <td>
                        <div>
                            <asp:CustomValidator runat="server" ID="cvComment" EnableClientScript="True" Visible="True"
                                Display="Dynamic" ValidationGroup="PopupRequiredGroup" ClientValidationFunction="EducationUc_ValidateComment"
                                ControlToValidate="txtComment" ValidateEmptyText="True" resourcekey="CommentEducation.Text"></asp:CustomValidator>
                        </div>
                        <textarea id="txtComment" runat="server"></textarea>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqComment" ControlToValidate="txtComment"
                            EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
            <div class="pad5">
                <asp:Button ID="btnSave" runat="server" Text="OK" ValidationGroup="PopupRequiredGroup" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
            </div>
        </div>
        </asp:Panel>
    </div>
</div>
<div class="page-break"></div>
