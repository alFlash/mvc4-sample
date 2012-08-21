<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ProfessionalPracticeQuestionnaire.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.Common.ProfessionalPracticeQuestionnaire" %>
<%@ Register TagPrefix="amc" TagName="DateTime" Src="../../Controls/Reusable/AMCDatetime.ascx" %>
<script type="text/javascript">
    ProPracQuestionnaire = new Object();
    ProPracQuestionnaire.divUploadFileAttachment = "divUploadFileAttachment";
    ProPracQuestionnaire.hlUploadFileAttachment = "hlUploadFileAttachment";
    ProPracQuestionnaire.imgDeleteAttachment = "imgDeleteAttachment";
    ProPracQuestionnaire.hfDeleteFile = "hfDeleteFile";
    ProPracQuestionnaire.CurrentRow = null;
    ProPracQuestionnaire.ShowHideDivUpload = function (questionItem, isShow) {
        var divUploadFileAttachment =
                        jQuery(questionItem).find('#' + ProPracQuestionnaire.divUploadFileAttachment);

        if (!isShow) {
            jQuery(divUploadFileAttachment).hide(300);
            //jQuery(questionItem).find('#[id*=' + StandardQuestionaireUC.hfDeleteFile + ']').val('YES');
        } else {
            jQuery(divUploadFileAttachment).show(300);
        }
    };
    ProPracQuestionnaire.ProcessDeleteFile = function (xmlhttp) {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            //delete file on server successfully
            if (xmlhttp.responseText.startsWith("1")) {
                jQuery(ProPracQuestionnaire.CurrentRow).find('#[id*=imgDeleteAttachment]').hide();
                var hlUploadFileAttachment = jQuery(ProPracQuestionnaire.CurrentRow)
                                            .find('#[id*=' + ProPracQuestionnaire.hlUploadFileAttachment + ']');
                hlUploadFileAttachment.attr('href', '');
                hlUploadFileAttachment.hide();
                hlUploadFileAttachment.val('');
                jQuery(ProPracQuestionnaire.CurrentRow).find('#[id*=' + ProPracQuestionnaire.hfDeleteFile + ']').val('NO');
            } else {
                ProPracQuestionnaire.ShowHideDivUpload(ProPracQuestionnaire.CurrentRow, true);
                jQuery(ProPracQuestionnaire.CurrentRow).find('#[id*=imgDeleteAttachment]').show();
                alert(DeleteFileError);
            }
        }
    };
    jQuery(document).ready(function() {
        jQuery.each(jQuery('#ulPracticeQuestionaire  li'), function (idx, lisender) {
            var hlUploadFileAttachment =
                jQuery(lisender).find('#[id*=' + ProPracQuestionnaire.hlUploadFileAttachment + ']');
            var hfDeleteFile =
                jQuery(lisender).find('#[id*=' + ProPracQuestionnaire.hfDeleteFile + ']');
            var imgDeleteFile = jQuery(lisender).find('#[id*=imgDeleteAttachment]');

            if (typeof hlUploadFileAttachment.attr('href') == 'undefined' ||
                hlUploadFileAttachment.attr('href').length < 1) {
                imgDeleteFile.hide();
                ProPracQuestionnaire.ShowHideDivUpload(lisender, false);
            } else {
                ProPracQuestionnaire.ShowHideDivUpload(lisender, true);
            }

            jQuery.each(jQuery(lisender).find('#[id*=' + ProPracQuestionnaire.imgDeleteAttachment + ']'), function (id, sender) {

                jQuery(sender).live('click', function (s) {
                    s.preventDefault();

                    var confirmDeleteFile = confirm(ConfirmDeleteFile);
                    if (confirmDeleteFile) {
                        var fileName = jQuery(sender).parents('#' + AMCTablePopUp.DivUploadFileAtachment).find('[id*=hlUploadFileAttachment]');
                        if (fileName.length > 0) {
                            jQuery(sender).hide();
                            ProPracQuestionnaire.CurrentRow = lisender;
                            deleteDocument(fileName.attr('href'), ProPracQuestionnaire.ProcessDeleteFile);
                        }
                    }

                });
            });

            jQuery.each(jQuery(lisender).find('#[id*=chkYesNo]:checkbox'), function (id, sender) {
                if (jQuery(sender).prop('checked') == true) {
                    jQuery(lisender).find('#[id*=pnlUploadFile]').slideDown();
                }
                jQuery(sender).live('click', function () {
                    if (jQuery(sender).prop('checked') == true) {
                        jQuery(lisender).find('#[id*=pnlUploadFile]').slideDown(300);
                        if (typeof hlUploadFileAttachment.attr('href') == 'undefined' ||
                            hlUploadFileAttachment.attr('href').length < 1 || hfDeleteFile.val() == "YES") {
                            ProPracQuestionnaire.ShowHideDivUpload(lisender, false);
                        } else {
                            ProPracQuestionnaire.ShowHideDivUpload(lisender, true);
                        }
                    } else {
                        jQuery(lisender).find('#[id*=pnlUploadFile]').slideUp(300);
                    }
                });
            });

        });
    });

</script>
<div class="amc-page" id="uc-questionaire">
    <asp:HiddenField runat="server" ID="hdIsIncomplete" />
    <asp:HiddenField runat="server" ID="hdIsValidateFailed" />
    <asp:HiddenField runat="server" ID="hdSurveyId" Value="0" />
    <div class="amc-title">
        <asp:Label runat="server" ID="lblProfessionalPracticeQuestionnaire"></asp:Label>
    </div>
    <div id="FormFillingInstructions" runat="server" class="amc-instruction">
        <asp:Label runat="server" ID="lblFormFillingInstructions" Text="Please check boxes below."></asp:Label>
    </div>
    <div class="amc-contents">
        <div id="QuestionList" runat="server">
            <asp:Repeater runat="server" ID="rptQuestionnaire" EnableViewState="True">
                <HeaderTemplate>
                    <ul id="ulPracticeQuestionaire" style="list-style: none; padding-left: 0px">
                </HeaderTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
                <ItemTemplate>
                    <li style="padding-left: 0px" runat="server" Visible='<%# Eval("Enabled") %>'>
                        <asp:HiddenField runat="server" ID="hdQuestionEnabled" Value='<%# Eval("Enabled") %>'/>
                        <asp:HiddenField runat="server" ID="hdQuestionId" Value='<%# Eval("QuestionId") %>' />
                        <asp:HiddenField runat="server" ID="hdAnswerYes" Value="0" />
                        <asp:HiddenField runat="server" ID="hdAnswerNo" Value="0" />
                        <asp:HiddenField runat="server" ID="hdAnswerRange" Value="0" />
                        <asp:HiddenField runat="server" ID="hdResponseId" Value="0" />
                        <asp:HiddenField runat="server" ID="hdQuestionType" Value='<%# Eval("QuestionTypeString")  %>' />
                        <asp:CheckBox runat="server" ID="chkYesNo" Text='<%# Eval("QuestionText") %>' />
                        <asp:Label runat="server" ID="lblQuestionText" Text='<%# Eval("QuestionText") %>'></asp:Label>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtResponse" Style="display: block"></asp:TextBox>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtResponse"
                                resourcekey="IsNumber.Text" ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,9})?$"
                                Display="Dynamic" ValidationGroup="AmcGeneralGroup"></asp:RegularExpressionValidator>
                        </div>
                        <asp:Panel runat="server" ID="pnlUploadFile" CssClass="dn">
                            <table style="margin-top: 10px; width: auto;">
                                <tr id="UploadFileAttachment" runat="server">
                                    <td>
                                        <asp:Label ID="lblUploadFileAttachment" runat="server" Text="Attach Document"></asp:Label>
                                    </td>
                                    <td style="padding-left: 20px">
                                        <asp:FileUpload ID="fuUploadFileAttachment" runat="server" />
                                        <asp:RegularExpressionValidator ID="rqPdfFileExtension" ControlToValidate="fuUploadFileAttachment"
                                            ValidationExpression="^.*\.(pdf|PDF)$" runat="server" ValidationGroup="AmcGeneralGroup"
                                            resourcekey="RequirePDFFile.Text" Display="Dynamic" />
                                        <div id="divUploadFileAttachment">
                                            <asp:HyperLink runat="server" ID="hlUploadFileAttachment" CssClass="fl" Target="_blank"></asp:HyperLink>
                                            <asp:ImageButton runat="server" ID="imgDeleteAttachment" CssClass="fl" ImageUrl="../../Documentation/images/icons/delete_icon1.gif" />
                                            <asp:HiddenField runat="server" ID="hfDeleteFile" Value="NO" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>
<div class="page-break">
</div>
