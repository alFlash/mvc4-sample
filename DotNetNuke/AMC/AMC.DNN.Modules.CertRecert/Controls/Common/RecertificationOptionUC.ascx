<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="RecertificationOptionUC.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.Common.RecertificationOptionUC" %>
<%@ Import Namespace="AMC.DNN.Modules.CertRecert.Data.Enums" %>
<script>
    var RECERT_OPTION_YESNO_QUESTIONTYPE = '<%= AMC.DNN.Modules.CertRecert.Data.Enums.Enums.QuestionType.YESNO.ToString() %>';
    var RECERT_OPTION_MULTI_QUESTIONTYPE = '<%= AMC.DNN.Modules.CertRecert.Data.Enums.Enums.QuestionType.MULTI.ToString() %>';
    var RECERT_OPTION_EXAM_CHOICE_POPUP_TITLE = '<%= GetLocalizedString("ExamChoice.Text") %>';

    function ReCert_Option_ValidateResponse(sender, args) {
        args.IsValid = true;
        var questions = jQuery('#uc_recerfication_option #tbl-recert-option-question #[id*=rdbRecertOptionQuestionItem]');
        var checkedQuestion = jQuery('#uc_recerfication_option #tbl-recert-option-question #[id*=rdbRecertOptionQuestionItem]:checked');
        if (questions.length > 0 && checkedQuestion.length <= 0) {
            args.IsValid = false;
        }
    }
</script>
<div class="amc-page" id="uc_recerfication_option">
    <asp:HiddenField runat="server" ID="hdPreviousQuestionId" />
    <asp:HiddenField runat="server" ID="hdPreviousAnswerId" />
    <asp:HiddenField runat="server" ID="hdIsIncomplete" />
    <asp:HiddenField runat="server" ID="hdIsValidateFailed" />
    <asp:HiddenField runat="server" ID="hdCurrentObjectUniqueId" />
    <div class="amc-title">
        <asp:Label ID="lblRecertificationOptionUC" runat="server"></asp:Label></div>
    <div id="FormFillingInstructions" runat="server" class="amc-instruction">
        <asp:Label runat="server" ID="lblFormFillingInstructions"></asp:Label>
    </div>
    <div class="amc-contents">
        <div class="amc-error-message">
            <asp:CustomValidator runat="server" ID="rqReCertOptionQuestion" EnableClientScript="True"
                resourcekey="ChooseOptionMessage.Text" Display="Dynamic" ValidationGroup="AmcGeneralGroup"
                ClientValidationFunction="ReCert_Option_ValidateResponse"></asp:CustomValidator>
        </div>
        <div id="recert-option-question">
            <asp:Repeater runat="server" ID="rptRecertOptionQuestion" EnableViewState="True">
                <HeaderTemplate>
                    <table id="tbl-recert-option-question">
                </HeaderTemplate>
                <ItemTemplate>
                    <tr runat="server" visible='<%# Eval("Enabled") AndAlso Eval("QuestionCode").ToString() <> Enums.QuestionCode.RECERT_OPTION_IF_PASS_EXAM.ToString() AndAlso Eval("QuestionCode").ToString() <> Enums.QuestionCode.RECERT_OPTION_INCLUDE_MYNAME_ONLIST.ToString() %>'>
                        <td>
                            <div id="recert-option-question-item">
                                <asp:HiddenField runat="server" ID="hdQuestionEnabled" Value='<%# Eval("Enabled") %>'/>
                                <asp:HiddenField runat="server" ID="hdSurveyId" Value='<%# Eval("SurveyId") %>' />
                                <asp:HiddenField runat="server" ID="hdQuestionId" Value='<%# Eval("QuestionId") %>' />
                                <asp:HiddenField runat="server" ID="hdQuestionType" Value='<%# Eval("QuestionTypeString") %>' />
                                <asp:HiddenField runat="server" ID="hdQuestionCode" Value='<%# Eval("QuestionCode") %>' />
                                <asp:HiddenField runat="server" ID="hdYesAnswerId"/>
                                <asp:HiddenField runat="server" ID="hdNoAnswerId"/>
                                <asp:RadioButton runat="server" ID="rdbRecertOptionQuestionItem" Text='<%# Eval("QuestionText") %>' />
                            </div>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>
<div class="page-break">
</div>
<!-- Popup-->
<div id="popup_exam_choice" class="amc-popup">
    <div id="amc-exam-choice">
        <div id="ExamChoiceInstructions" runat="server">
            <asp:Label runat="server" ID="lblExamChoiceInstructions"></asp:Label>
        </div>
        <div id="recert-option-answer">
            <div class="amc-error-message">
                <asp:Label runat="server" ID="lblPopupMessage"></asp:Label>
            </div>
            <asp:Repeater runat="server" ID="rptRecertOptionAnswer" EnableViewState="True">
                <HeaderTemplate>
                    <table id="tbl-recert-option-answer">
                        <tr class="tr-header bg-darkgrey">
                            <td runat="server" visible='<%# GetFieldInfo("StartDate").IsEnabled %>' class="width150">
                                <asp:Label runat="server" ID="lblStartDate" Text='<%# GetFieldInfo("StartDate").FieldValue %>'></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("EndDate").IsEnabled %>' class="width150">
                                <asp:Label runat="server" ID="lblEndDate" Text='<%# GetFieldInfo("EndDate").FieldValue %>'></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("ApplicationDeadline").IsEnabled %>'
                                class="width150">
                                <asp:Label runat="server" ID="lblApplicationDeadline" Text='<%# GetFieldInfo("ApplicationDeadline").FieldValue %>'></asp:Label>
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr id="answerItem" runat="server" visible='<%# Eval("AnswerTypeString") = AMC.DNN.Modules.CertRecert.Data.Enums.Enums.AnswerType.YES.ToString() %>'>
                        <td runat="server" visible='<%# GetFieldInfo("StartDate").IsEnabled %>'>
                            <div id="recert-option-answer-item">
                                <asp:HiddenField runat="server" ID="hdQuestionId" Value='<%# Eval("QuestionId") %>' />
                                <asp:HiddenField runat="server" ID="hdAnswerId" Value='<%# Eval("AnswerId") %>' />
                                <asp:HiddenField runat="server" ID="hdAnswerType" Value='<%# Eval("AnswerTypeString") %>' />
                                <asp:HiddenField runat="server" ID="hdAnswerText" Value='<%# Eval("AnswerText") %>' />
                                <asp:RadioButton runat="server" ID="rdbRecertOptionAnswerItem" />
                            </div>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("EndDate").IsEnabled %>'>
                            <asp:Label runat="server" ID="lblReCertOptionAnswerEndDate"></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("ApplicationDeadline").IsEnabled %>'>
                            <asp:Label runat="server" ID="lblReCertOptionAnswerDeadline"></asp:Label>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <asp:Repeater runat="server" ID="rptRecertOptionNotificationQuestions">
                <HeaderTemplate>
                    <table id="tbl-recertoption-notification-questions">
                </HeaderTemplate>
                <ItemTemplate>
                    <tr runat="server" Visible='<%# Eval("Enabled") %>'>
                        <td>
                            <asp:HiddenField runat="server" ID="hdQuestionEnabled" Value='<%# Eval("Enabled") %>'/>
                            <asp:HiddenField runat="server" ID="hdQuestionId" Value='<%# Eval("QuestionId") %>' />
                            <asp:HiddenField runat="server" ID="hdYesAnswerId"/>
                            <asp:HiddenField runat="server" ID="hdNoAnswerId"/>
                            <asp:CheckBox runat="server" ID="chkRecertOptionNotificationResponse" Text='<%# Eval("QuestionText") %>'/>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <div class="text-center">
            <asp:Button runat="server" ID="btnExamChoiceOK" Text="OK" />
        </div>
    </div>
</div>
