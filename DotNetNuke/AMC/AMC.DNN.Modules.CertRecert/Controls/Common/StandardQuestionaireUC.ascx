<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="StandardQuestionaireUC.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.Common.StandardQuestionaireUC" %>
<%@ Register TagPrefix="amc" TagName="DateTime" Src="../../Controls/Reusable/AMCDatetime.ascx" %>
<script type="text/javascript">
    StandardQuestionaireUC = new Object();
    StandardQuestionaireUC.divUploadFileAttachment = "divUploadFileAttachment";
    StandardQuestionaireUC.hlUploadFileAttachment = "hlUploadFileAttachment";
    StandardQuestionaireUC.imgDeleteAttachment = "imgDeleteAttachment";
    StandardQuestionaireUC.hfDeleteFile = "hfDeleteFile";
    StandardQuestionaireUC.CurrentRow = null;
    StandardQuestionaireUC.ShowHideDivUpload = function (questionItem, isShow) {
        var divUploadFileAttachment =
                        jQuery(questionItem).find('#' + StandardQuestionaireUC.divUploadFileAttachment);

        if (!isShow) {
            jQuery(divUploadFileAttachment).hide(300);
            //jQuery(questionItem).find('#[id*=' + StandardQuestionaireUC.hfDeleteFile + ']').val('YES');
        } else {
           jQuery(divUploadFileAttachment).show(300);
        }
    };
    StandardQuestionaireUC.ProcessDeleteFile = function (xmlhttp) {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            //delete file on server successfully
            if (xmlhttp.responseText.startsWith("1")) {
                StandardQuestionaireUC.ShowHideDivUpload(StandardQuestionaireUC.CurrentRow, false);
                jQuery(StandardQuestionaireUC.CurrentRow).find('#[id*=imgDeleteAttachment]').hide();
                var hlUploadFileAttachment = jQuery(StandardQuestionaireUC.CurrentRow)
                                            .find('#[id*=' + StandardQuestionaireUC.hlUploadFileAttachment + ']');
                hlUploadFileAttachment.attr('href', '');
                hlUploadFileAttachment.hide();
                hlUploadFileAttachment.val('');
                jQuery(StandardQuestionaireUC.CurrentRow).find('#[id*=' + StandardQuestionaireUC.hfDeleteFile + ']').val('NO');
            } else {
                StandardQuestionaireUC.ShowHideDivUpload(StandardQuestionaireUC.CurrentRow, true);
                jQuery(StandardQuestionaireUC.CurrentRow).find('#[id*=imgDeleteAttachment]').show();
                alert(DeleteFileError);
            }
        }
    };


    jQuery(document).ready(function () {

        jQuery.each(jQuery('#ulStandardQuestionaire li'), function (idx, lisender) {

            var hlUploadFileAttachment =
                jQuery(lisender).find('#[id*=' + StandardQuestionaireUC.hlUploadFileAttachment + ']');
            var hfDeleteFile =
                jQuery(lisender).find('#[id*=' + StandardQuestionaireUC.hfDeleteFile + ']');
            var imgDeleteFile = jQuery(lisender).find('#[id*=imgDeleteAttachment]');

            if (typeof hlUploadFileAttachment.attr('href') == 'undefined' ||
                hlUploadFileAttachment.attr('href').length < 1) {
                imgDeleteFile.hide();
                StandardQuestionaireUC.ShowHideDivUpload(lisender, false);
            } else {
                StandardQuestionaireUC.ShowHideDivUpload(lisender, true);
            }

            jQuery.each(jQuery(lisender).find('#[id*=' + StandardQuestionaireUC.imgDeleteAttachment + ']'), function (id, sender) {

                jQuery(sender).live('click', function (s) {
                    s.preventDefault();

                    var confirmDeleteFile = confirm(ConfirmDeleteFile);
                    if (confirmDeleteFile) {
                        var fileName = jQuery(sender).parent().find('[id*=hlUploadFileAttachment]');

                        if (fileName.length > 0) {
                            jQuery(sender).hide();
                            StandardQuestionaireUC.CurrentRow = lisender;
                            deleteDocument(fileName.attr('href'), StandardQuestionaireUC.ProcessDeleteFile);
                        }
                    }
                });
            });
            jQuery.each(jQuery(lisender).find('#[id*=rdlAnswer]:radio'), function (id, sender) {
                var hdAnswerYes = jQuery(lisender).find('#[id*=hdAnswerYes]:hidden');
                if (jQuery(sender).prop('checked') == true && jQuery(sender).val() == hdAnswerYes.val()) {
                    jQuery(lisender).find('#[id*=pnlDetail]').slideDown();
                }
                else { // add by DuyTruong 
                       // jQuery(lisender).find('#[id*=pnlDetail]').hide();
                }

                jQuery(sender).live('click', function () {
                    if (jQuery(sender).val() == hdAnswerYes.val()) {
                        jQuery(lisender).find('#[id*=pnlDetail]').slideDown(300);
                        if (typeof hlUploadFileAttachment.attr('href') == 'undefined' ||
                            hlUploadFileAttachment.attr('href').length < 1 || hfDeleteFile.val() == "YES") {
                            StandardQuestionaireUC.ShowHideDivUpload(lisender, false);
                        } else {
                            StandardQuestionaireUC.ShowHideDivUpload(lisender, true);
                        }
                    } else {
                        jQuery(lisender).find('#[id*=pnlDetail]').slideUp(300);
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
        <asp:Label runat="server" ID="lblStandardQuestionaireUC"></asp:Label>
    </div>
    <div id="FormFillingInstructions" runat="server" class="amc-instruction">
        <asp:Label runat="server" ID="lblFormFillingInstructions" Text="Please check boxes below. If “yes,” please give full details in field provided or upload documentation."></asp:Label>
    </div>
    <div class="amc-contents">
        <div id="QuestionList" runat="server">
            <asp:Repeater runat="server" ID="rptQuestionnaire" EnableViewState="True">
                <HeaderTemplate>
                    <ul id="ulStandardQuestionaire" style="list-style: none; padding-left: 0px">
                </HeaderTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
                <ItemTemplate>
                    <li style="padding-left: 0px" id="StandardQuestionaireItem" runat="server" Visible='<%# Eval("Enabled") %>'>
                        <div>
                            <asp:HiddenField runat="server" ID="hdQuestionEnabled" Value='<%# Eval("Enabled") %>'/>
                            <asp:HiddenField runat="server" ID="hdQuestionId" Value='<%# Eval("QuestionId") %>' />
                            <asp:HiddenField runat="server" ID="hdAnswerYes" Value="0" />
                            <asp:HiddenField runat="server" ID="hdAnswerNo" Value="0" />
                            <asp:HiddenField runat="server" ID="hdResponseId" Value="0" />
                            <asp:Label runat="server" ID="lblQuestion" Text='<%# Eval("QuestionText") %>'></asp:Label>
                        </div>
                        <div style="width: 150px">
                            <asp:RadioButtonList runat="server" ID="rdlAnswer" RepeatDirection="Horizontal">
                            </asp:RadioButtonList>
                        </div>
                        <asp:Panel runat="server" ID="pnlDetail" CssClass="dn">
                            <table style="margin-top: 10px; width: auto;">
                                <tr id="Details" runat="server" visible='<%# GetFieldInfo("Details").IsEnabled %>'>
                                    <td>
                                        <asp:Label runat="server" Text="Details" ID="lblDetail"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" CssClass="width250 fl" TextMode="MultiLine" ID="txtDetails"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="UploadFileAttachment" runat="server" visible='<%# GetFieldInfo("UploadFileAttachment").IsEnabled %>'>
                                    <td>
                                        <asp:Label ID="lblUploadFileAttachment" runat="server" Text="Attach Document"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:FileUpload ID="fuUploadFileAttachment" runat="server" />
                                        <asp:RegularExpressionValidator ID="rqPdfFileExtension" ControlToValidate="fuUploadFileAttachment"
                                            ValidationExpression="^.*\.(pdf|PDF)$" runat="server" ValidationGroup="PopupRequiredGroup"
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
<div class="page-break"></div>
