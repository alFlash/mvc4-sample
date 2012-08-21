<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ProfessionalPracticeExperienceUC.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.Common.ProfessionalPracticeExperienceUC" %>
<%@ Register TagPrefix="amc" TagName="DateTime" Src="../Reusable/AMCDatetime.ascx" %>
<script type="text/javascript">

    function showPracticePopup(isShow) {
        AmcCert.showPracticePopup("add-experience-popup", isShow);
        AmcCert.SetTitle("add-experience-popup", "Add Information ");
    }
    jQuery(document).ready(function () {
        var bindingList = [['lblInstitutionName', 'txtInstitutionName'],
                           ['lblPosition', 'txtPosition'],
                           ['lblStartDate', 'txtStartDate'],
                           ['lblEndDate', 'txtEndDate'],
                           ['lblComment', 'txtComment'],
                           ['hlAttachedDocumentName', 'fuUploadFileAttachment'],
                           ['hlAttachedDocumentName', 'hlUploadFileAttachment']
                           ];
        var practicePopup = new AMCTablePopUp(bindingList,
           "experience-uc",
            "tblexperience",
            "add-experience-popup",
            "Add Information ",
            "Edit Item ",
            null);
    });

    function ProfessionalPracticeExp_ValidateComment(sender, args) {
        args.IsValid = true;
        var startDateTextBox = jQuery('#add-experience-popup #[id*=txtStartDate] #[id*=txtDatetime]');
        var startDate = new Date(Date.parse(startDateTextBox.val()));
        var expRows = jQuery('#tblexperience tr.amc-table-content');
        var commentTestBox = jQuery('#add-experience-popup #[id*=txtComment]');
        for (var i = 0; i < expRows.length; i++) {
            var currentRow = expRows[i];
            var endDateLable = jQuery(currentRow).find('#[id*=lblEndDate]');
            var endDate = new Date(Date.parse(endDateLable.text()));
            var uniqueId = jQuery(currentRow).find('#[id*=hdObjectUniqueId]').val();
            var currentUniqueId = jQuery('#[id*=hdCurrentObjectUniqueId]').val();
            var hlUploadFileAttachment = jQuery('#add-experience-popup #[id*=hlUploadFileAttachment]');
            var fuUploadFileAttachment = jQuery('#add-experience-popup #[id*=fuUploadFileAttachment]');
            if (uniqueId != currentUniqueId) {
                if ((startDate.getYear() > (endDate.getYear() + 1)) || (startDate.getYear() < (endDate.getYear() - 1))
                    || ((startDate.getYear() == (endDate.getYear() - 1)) && (((endDate.getMonth() + (11 - startDate.getMonth())) > 2) || (((endDate.getMonth() + (11 - startDate.getMonth())) == 2) && (endDate.getDate() > startDate.getDate()))))
                    || ((startDate.getYear() == (endDate.getYear() + 1)) && (((startDate.getMonth() + (11 - endDate.getMonth())) > 2) || (((startDate.getMonth() + (11 - endDate.getMonth())) == 2) && (startDate.getDate() > endDate.getDate()))))
                    || (startDate.getYear() == endDate.getYear() && (startDate.getMonth() - endDate.getMonth() > 2 || endDate.getMonth() - startDate.getMonth() > 2 || (startDate.getMonth() - endDate.getMonth() == 2 && (startDate.getDate() > endDate.getDate()))))) {
                    if (IsStringNullOrEmpty(commentTestBox.val()) ||
                           (IsStringNullOrEmpty(hlUploadFileAttachment.text()) && IsStringNullOrEmpty(fuUploadFileAttachment.val()))) {
                        args.IsValid = false;
                        break;
                    }
                }
            }
        }
    }
</script>
<div id="experience-uc" class="amc-page">
    <asp:HiddenField runat="server" ID="hdIsIncomplete" />
    <asp:HiddenField runat="server" ID="hdIsValidateFailed" />
    <asp:HiddenField runat="server" ID="hdCurrentObjectUniqueId" />
    <div class="amc-title">
        <asp:Label runat="server" ID="lblProfessionalPracticeExperienceUC"></asp:Label>
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
            <%--hlAddNew--%>
            <div class="cb">
            </div>
        </div>
        <div>
            <asp:Repeater runat="server" ID="rptexperience" EnableViewState="True">
                <HeaderTemplate>
                    <table id="tblexperience" class="amc-table">
                        <tr class="amc-table-header">
                            <td runat="server" visible='<%# GetFieldInfo("InstitutionName").IsEnabled %>'>
                                <asp:Label runat="server" ID="lblInstitutionNameHeader" Text='<%# GetFieldInfo("InstitutionName").FieldValue %>'></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("Position").IsEnabled %>'>
                                <asp:Label runat="server" ID="lblPositionHeader" Text='<%# GetFieldInfo("Position").FieldValue %>'></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("StartDate").IsEnabled %>'>
                                <asp:Label runat="server" ID="lblStartDateHeader" Text='<%# GetFieldInfo("StartDate").FieldValue %>'></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("EndDate").IsEnabled %>'>
                                <asp:Label runat="server" ID="lblEndDateHeader" Text='<%# GetFieldInfo("EndDate").FieldValue %>'></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("Comment").IsEnabled %>'>
                                <asp:Label runat="server" ID="lblCommentHeader" Text='<%# GetFieldInfo("Comment").FieldValue %>'></asp:Label>
                            </td>
                            <td visible='<%# GetFieldInfo("UploadFileAttachment").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="Label1" Text='<%# GetFieldInfo("UploadFileAttachment").FieldValue %>'></asp:Label>
                            </td>
                            <td class="action-column">
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="amc-table-content">
                        <td runat="server" visible='<%# GetFieldInfo("InstitutionName").IsEnabled %>'>
                            <asp:Label runat="server" ID="lblInstitutionName" Text='<%# Eval("OrganizationProviding") %>'></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("Position").IsEnabled %>'>
                            <asp:Label runat="server" ID="lblPosition" Text='<%# Eval("Position") %>'></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("StartDate").IsEnabled %>'>
                            <asp:Label runat="server" ID="lblStartDate"></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("EndDate").IsEnabled %>'>
                            <asp:Label runat="server" ID="lblEndDate"></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("Comment").IsEnabled %>'>
                            <asp:Label runat="server" ID="lblComment" Text='<%# Eval("Comments") %>'></asp:Label>
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
    <div id="add-experience-popup" class="amc-popup">
        <asp:Panel runat="server" ID="panelPopup" DefaultButton="btnSave">
            <div>
                <div class="amc-error-message">
                    <asp:Label runat="server" ID="lblPopupMessage"></asp:Label>
                    <div>
                        <asp:ValidationSummary runat="server" ID="vldsPopupRequiredGroup" ValidationGroup="PopupRequiredGroup" />
                    </div>
                </div>
                <table class="width500">
                    <tr id="InstitutionName" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblInstitutionName"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width250" ID="txtInstitutionName"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqInstitutionName"
                                ControlToValidate="txtInstitutionName" EnableClientScript="True" Display="Dynamic"
                                ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="Position" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblPosition"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width250" ID="txtPosition"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqPosition" ControlToValidate="txtPosition"
                                EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="StartDate" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblStartDate"></asp:Label>
                        </td>
                        <td>
                            <amc:DateTime runat="server" ID="txtStartDate" ValidationGroup="PopupRequiredGroup"
                                CssClass="fl"></amc:DateTime>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqStartDate" ControlToValidate="txtStartDate$txtDatetime"
                                CssClass="fl" EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="EndDate" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblEndDate"></asp:Label>
                        </td>
                        <td>
                            <amc:DateTime runat="server" ID="txtEndDate" ValidationGroup="PopupRequiredGroup"
                                CssClass="fl"></amc:DateTime>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqEndDate" ControlToValidate="txtEndDate$txtDatetime"
                                CssClass="fl" EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                            <div class="cb">
                                <asp:CompareValidator ID="Compare1" ControlToValidate="txtEndDate$txtDatetime" ControlToCompare="txtStartDate$txtDatetime"
                                     Type="Date" runat="server" resourcekey="CompareDateCommon.Text"
                                    Display="Dynamic" Operator="GreaterThanEqual" ValidationGroup="PopupRequiredGroup" />
                            </div>
                            <div class="cb">
                                <asp:CustomValidator runat="server" ID="cusValidOtherBoard" ControlToValidate="txtEndDate$txtDatetime"
                                    ClientValidationFunction="ValidateReCertCircle" Display="Dynamic" ValidationGroup="PopupRequiredGroup"
                                    resourcekey="InvalidTimeFrame.Text" EnableClientScript="True" ValidateEmptyText="True"></asp:CustomValidator>
                            </div>
                        </td>
                    </tr>
                    <tr id="Comment" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblComment"></asp:Label>
                        </td>
                        <td>
                            <div>
                                <asp:CustomValidator runat="server" ID="cvComment" EnableClientScript="True" Visible="True"
                                    Display="Dynamic" ValidationGroup="PopupRequiredGroup" ClientValidationFunction="ProfessionalPracticeExp_ValidateComment"
                                    ControlToValidate="txtComment" ValidateEmptyText="True" resourcekey="CommentProExperience.Text"></asp:CustomValidator>
                            </div>
                            <textarea id="txtComment" runat="server" rows="3"></textarea>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqComment" ControlToValidate="txtComment"
                                EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="UploadFileAttachment" runat="server">
                        <td>
                            <asp:Label ID="lblUploadFileAttachment" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:FileUpload ID="fuUploadFileAttachment" runat="server" CssClass="fl" />
                            <%-- <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqUploadFileAttachment" ControlToValidate="fuUploadFileAttachment"
                    EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>--%>
                            <asp:CustomValidator runat="server" ID="rqUploadFileAttachment" EnableClientScript="True"
                                Display="Dynamic" ValidationGroup="PopupRequiredGroup" CssClass="fl" ClientValidationFunction="ValidateUploadFile"
                                ControlToValidate="fuUploadFileAttachment" ValidateEmptyText="True" ErrorMessage="*">
                            </asp:CustomValidator>
                            <div class="cb">
                            </div>
                            <div>
                                <asp:RegularExpressionValidator ID="rqPdfFileExtension" ControlToValidate="fuUploadFileAttachment"
                                    ValidationExpression="^.*\.(pdf|PDF)$" runat="server" ValidationGroup="PopupRequiredGroup"
                                    resourcekey="RequirePDFFile.Text" Display="Dynamic" />
                            </div>
                            <br />
                            <div id="divUploadFileAttachment">
                                <asp:HyperLink runat="server" ID="hlUploadFileAttachment" CssClass="fl" Text=""
                                    Target="_blank"></asp:HyperLink>
                                <asp:ImageButton runat="server" ID="imgDeleteAttachment" CssClass="fl" ImageUrl="../../Documentation/images/icons/delete_icon1.gif" />
                                <asp:HiddenField runat="server" ID="hfDeleteFile" Value="NO" />
                            </div>
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
<div class="page-break">
</div>
