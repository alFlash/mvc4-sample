<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="FieldQuestionListUC.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.FormConfigurations.FieldQuestionListUC" %>
<%@ Import Namespace="AMC.DNN.Modules.CertRecert.Data.Enums" %>
<%@ Register TagPrefix="amc" TagName="DateTime" Src="../../Controls/Reusable/AMCDatetime.ascx" %>
<script>
    var currentSurvey = '<%= CurrentSurvey.Title %>';
    var certification_ExamChoice_Survey_Title = '<% = DataAccessConstants.CERTIFICATION_EXAM_CHOICE_SURVEY_TITLE %>';
    var recertification_Option_Survey_Title = '<% = DataAccessConstants.RECERTIFICATION_OPTION_SERVEY_TITLE %>';
    var RecertificationCircleValidilityMonths = '<%= RecertificationCircleValidilityMonths %>';
    var DateTimeFormat = '<%= DateTimeFormat %>';
</script>
<div id="question-list-configuration" class="amc-page">
    <asp:HiddenField runat="server" ID="hdCurrentOpenQuestion" />
    <asp:HiddenField runat="server" ID="hdCurrentQuestionItem" />
    <asp:HiddenField runat="server" ID="hdCurrentAnswerItem" />
    <asp:HiddenField runat="server" ID="hdCurrentAnswerTypeString" />
    <div class="amc-title">
        <asp:Label runat="server" ID="lblTitle"></asp:Label>
    </div>
    <div class="amc-instruction bg-darkgrey">
        <asp:Label runat="server" ID="lblQuestionListSettingsTitle" resourcekey="QuestionListSettings.Text"></asp:Label>
    </div>
    <div class="amc-contents">
        <div class="padleft20">
            <asp:Label runat="server" ID="lblMessage" ForeColor="Red"></asp:Label>
        </div>
        <%  If GetFieldConfigInfo().CanAddQuestion Then%>
        <div id="addnew-question" class="amc-add-instruction padleft20">
            <asp:Image CssClass="pointer fl" runat="server" ID="Image1" ImageUrl="../../Documentation/images/icons/addnew_11x11.gif">
            </asp:Image>
            <asp:HyperLink runat="server" ID="hlAddNew" resourcekey="AddInformation.Text" CssClass="fl padleft5"
                href="javascript:void(0);"></asp:HyperLink>
            <div class="cb">
            </div>
        </div>
        <%End If%>
        <div class="padleft20">
            <div id="question-list">
                <asp:Repeater runat="server" ID="rptQuestionList">
                    <HeaderTemplate>
                        <table id="tbl-question-list">
                            <tr class="tr-question-header">
                                <td>
                                    <div class="fl width400">
                                        <asp:Label runat="server" ID="lblQuestTextHeader" resourcekey="QuestionText.Text"></asp:Label>
                                    </div>
                                    <div class="fr width100 text-center">
                                        <asp:Label runat="server" ID="lblIsEnable" Text="IsEnable"></asp:Label>
                                    </div>
                                    <div class="fr width100 text-center">
                                        <asp:Label runat="server" ID="lblAction" resourcekey="Actions.Text"></asp:Label>
                                    </div>
                                    <div class="fr width100 text-center">
                                        <asp:Label runat="server" ID="lblAddAnswer" resourcekey="AddAnswer.Text"></asp:Label>
                                    </div>
                                    <div class="cb">
                                    </div>
                                </td>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <div id="question-item">
                                    <div class="hover-item" style="width: 100%; padding-bottom: 5px; padding-top: 5px;
                                        line-height: 22px;" id="question-hover-item">
                                        <asp:HiddenField runat="server" ID="hdQuestionId" Value='<%# Eval("QuestionId") %>' />
                                        <asp:HiddenField runat="server" ID="hdQuestionType" Value='<%# Eval("QuestionTypeString") %>' />
                                        <div id="questionexpand" class="expand-image fl" runat="server">
                                        </div>
                                        <div class="fl width400 padleft5">
                                            <asp:Label runat="server" ID="lblQuestionRelationshipIndicators" Font-Bold="True"
                                                Font-Italic="True"></asp:Label>
                                            <asp:Label runat="server" ID="lblQuestionText" Text='<%# Eval("QuestionText") %>'></asp:Label>
                                        </div>
                                        <div class="fr width100 text-center">
                                            <asp:CheckBox runat="server" ID="chkIsEnabled" Checked='<%# Eval("Enabled") %>' />
                                        </div>
                                        <div class="fr width200">
                                            <div class="width100 fl text-center">
                                                <div class="add-image margin-auto dn opacity40" runat="server" visible='<%# Eval("QuestionTypeString") = Enums.QuestionType.MULTI.ToString() %>'>
                                                </div>
                                            </div>
                                            <div class="fr width100">
                                                <div class="width50px margin-auto">
                                                    <div class="fr">
                                                        <%--<%  If GetFieldConfigInfo().CanAddQuestion Then%>--%>
                                                        <asp:ImageButton runat="server" CssClass="delete-image dn opacity40" ID="imgDeleteQuestion"
                                                            Visible='<%# Eval("CanDelete") %>' CommandName="DeleteQuestionItem" CausesValidation="False"
                                                            ImageUrl="../../Documentation/images/icons/delete_icon1.gif" />
                                                        <%--<%End If%>--%>
                                                    </div>
                                                    <div class="edit-image configuration-edit-image">
                                                    </div>
                                                    <div class="cb">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="cb">
                                            </div>
                                        </div>
                                        <div class="cb">
                                        </div>
                                    </div>
                                    <div runat="server" id="answerlist" class="dn" visible='<%# Eval("QuestionTypeString") <> Enums.QuestionType.RANGE.ToString() %>'>
                                        <asp:Repeater runat="server" ID="rptAnswerList">
                                            <HeaderTemplate>
                                                <table id="tbl-answer-list">
                                                    <tr>
                                                        <td class="width40px">
                                                        </td>
                                                        <td class="tr-question-header">
                                                            <div class="fl width400">
                                                                <asp:Label runat="server" ID="lblAnswerTextHeader" resourcekey="AnswerText.Text"></asp:Label>
                                                            </div>
                                                            <div class="fr width100 text-center">
                                                                <asp:Label runat="server" ID="Label1" resourcekey="Actions.Text"></asp:Label>
                                                            </div>
                                                            <div class="cb">
                                                            </div>
                                                        </td>
                                                    </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td class="width40px">
                                                    </td>
                                                    <td>
                                                        <div class="hover-item" style="padding-top: 5px; padding-bottom: 5px; line-height: 22px;"
                                                            id="answer-hover-item">
                                                            <asp:HiddenField runat="server" ID="hdAnswerQuestionId" Value='<%# Eval("QuestionId") %>' />
                                                            <asp:HiddenField runat="server" ID="hdAnswerId" Value='<%# Eval("AnswerId") %>' />
                                                            <asp:HiddenField runat="server" ID="hdAnswerQuestionType" />
                                                            <asp:HiddenField runat="server" ID="hdAnswerType" Value='<%# Eval("AnswerTypeString") %>' />
                                                            <div class="fl width600">
                                                                <table runat="server" id="tblAnswerText" visible='<%# (CurrentSurvey.Title = DataAccessConstants.CERTIFICATION_EXAM_CHOICE_SURVEY_TITLE OrElse CurrentSurvey.Title = DataAccessConstants.RECERTIFICATION_OPTION_SERVEY_TITLE) AndAlso Eval("AnswerTypeString") = "YES" %>'>
                                                                    <tr style="background-color: cornflowerblue; font-weight: bold;">
                                                                        <td class="width200">
                                                                            <asp:Label runat="server" ID="lblStartDate" resourcekey="StartDate.Text"></asp:Label>
                                                                        </td>
                                                                        <td class="width200">
                                                                            <asp:Label runat="server" ID="Label2" resourcekey="EndDate.Text"></asp:Label>
                                                                        </td>
                                                                        <td class="width200">
                                                                            <asp:Label runat="server" ID="Label3" resourcekey="ApplicationDeadline.Text"></asp:Label>
                                                                        </td>
                                                                        <td class="width200">
                                                                            <asp:Label runat="server" ID="Label4" resourcekey="ExamProductId.Text"></asp:Label>
                                                                        </td>
                                                                        <td class="width200">
                                                                            <asp:Label runat="server" ID="Label5" resourcekey="ApplicationProductId.Text"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblExamStartDate"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblExamEndDate"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblExamApplicationDeadline"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblProductCode"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblApplicationProductId"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <asp:Label runat="server" ID="lblAnswerText" Text='<%# Eval("AnswerText") %>' Visible='<%# Not ((CurrentSurvey.Title = DataAccessConstants.CERTIFICATION_EXAM_CHOICE_SURVEY_TITLE OrElse CurrentSurvey.Title = DataAccessConstants.RECERTIFICATION_OPTION_SERVEY_TITLE) AndAlso Eval("AnswerTypeString") = "YES") %>'></asp:Label>
                                                            </div>
                                                            <div class="fr width100">
                                                                <div class="width50px margin-auto">
                                                                    <div class="fr" runat="server" visible='<%# Eval("AnswerTypeString") = Enums.AnswerType.YES.ToString() %>'>
                                                                        <asp:ImageButton runat="server" CssClass="delete-image dn" ID="imgDeleteAnswer" CommandName="DeleteAnswerItem"
                                                                            ImageUrl="../../Documentation/images/icons/delete_icon1.gif" CausesValidation="False" />
                                                                    </div>
                                                                    <div class="edit-image configuration-edit-image" runat="server" visible='<%# Eval("AnswerTypeString") = Enums.AnswerType.YES.ToString() %>'>
                                                                    </div>
                                                                    <div class="cb">
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="cb">
                                                            </div>
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
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
            <div class="text-center">
                <asp:Button runat="server" ID="btnSave" resourcekey="Save.Text" />
                <asp:Button runat="server" ID="btnBack" resourcekey="Back.Text" />
            </div>
        </div>
        <div class="amc-popup" id="add-question-item">
            <table class="width500">
                <%--<asp:ListItem Value="RANGE" Text="Range Answer"></asp:ListItem>--%>
                <%--<asp:ListItem Value="YESNO" Text="Yes/No Answer" Selected="True"></asp:ListItem>--%>
                <%  If GetFieldConfigInfo().QuestionType.IndexOf(Enums.QuestionType.MULTI.ToString()) <> -1 OrElse GetFieldConfigInfo().QuestionType.IndexOf(Enums.QuestionType.RANGE.ToString()) <> -1 Then%>
                <tr>
                    <td>
                        <div class="text-center">
                            <asp:RadioButtonList runat="server" ID="rdbQuestionType" RepeatDirection="Horizontal"
                                CssClass="margin-auto">
                            </asp:RadioButtonList>
                        </div>
                    </td>
                </tr>
                <%End If%>
                <tr>
                    <td colspan="2">
                        <div>
                            <asp:RequiredFieldValidator runat="server" ID="rqQuestionText" ControlToValidate="txtQuestionText"
                                EnableClientScript="True" ErrorMessage="Please enter value." ValidationGroup="AddQuestionGroup"
                                Display="Dynamic" CssClass="padbot10"></asp:RequiredFieldValidator>
                        </div>
                        <asp:TextBox runat="server" ID="txtQuestionText" Width="550px" Height="350px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="text-center">
                        <asp:Button runat="server" ID="btnOK" resourcekey="OK.Text" ValidationGroup="AddQuestionGroup" />
                        <asp:Button runat="server" ID="btnCancel" resourcekey="Cancel.Text" CausesValidation="False" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="amc-popup" id="add-answer-item">
            <table class="width500">
                <tr>
                    <td colspan="2">
                        <div>
                            <div>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="txtAnswerText"
                                    EnableClientScript="True" ErrorMessage="Please enter value." ValidationGroup="AddAnswerGroup"
                                    Display="Dynamic" CssClass="padbot10"></asp:RequiredFieldValidator>
                            </div>
                            <asp:TextBox runat="server" ID="txtAnswerText" Width="550px" Height="350px" TextMode="MultiLine"></asp:TextBox>
                        </div>
                        <table id="tblAnswerText">
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblStartDate" resourcekey="StartDate.Text"></asp:Label>
                                </td>
                                <td>
                                    <amc:DateTime runat="server" ID="txtAnswerStartDate" ValidationGroup="SaveAnswerPopUp"
                                        CssClass="fl"></amc:DateTime>
                                    <asp:RequiredFieldValidator runat="server" ID="rqStarDate" ControlToValidate="txtAnswerStartDate$txtDatetime"
                                        CssClass="fl" ErrorMessage="*" Display="Dynamic" ValidationGroup="SaveAnswerPopUp"></asp:RequiredFieldValidator>
                                    <%--<div>
                                        <asp:CompareValidator ID="rqAnswerStartDate" runat="server" Type="Date" Operator="DataTypeCheck" ValidationGroup="SaveAnswerPopUp"
                                            Display="Dynamic" ControlToValidate="txtAnswerStartDate$txtDatetime" ErrorMessage="Please enter a valid date.">
                                        </asp:CompareValidator>
                                    </div>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="Label2" resourcekey="EndDate.Text"></asp:Label>
                                </td>
                                <td>
                                    <amc:DateTime runat="server" ID="txtAnswerEndDate" ValidationGroup="SaveAnswerPopUp"
                                        CssClass="fl"></amc:DateTime>
                                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator1"
                                        ControlToValidate="txtAnswerEndDate$txtDatetime" CssClass="fl" Display="Dynamic"
                                        ValidationGroup="SaveAnswerPopUp"></asp:RequiredFieldValidator>
                                    <div class="cb">
                                        <asp:CompareValidator ID="Compare1" ControlToValidate="txtAnswerEndDate$txtDatetime"
                                            ValidationGroup="SaveAnswerPopUp" ControlToCompare="txtAnswerStartDate$txtDatetime"
                                            resourcekey="StartDateLessEndDate.Text" Type="Date" runat="server" Display="Dynamic"
                                            Operator="GreaterThanEqual" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="Label3" resourcekey="ApplicationDeadline.Text"></asp:Label>
                                </td>
                                <td>
                                    <amc:DateTime runat="server" ID="txtAnswerApplicationDeadline" ValidationGroup="SaveAnswerPopUp"
                                        CssClass="fl"></amc:DateTime>
                                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator2"
                                        ControlToValidate="txtAnswerApplicationDeadline$txtDatetime" Display="Dynamic"
                                        ValidationGroup="SaveAnswerPopUp" CssClass="fl"></asp:RequiredFieldValidator>
                                    <div class="cb">
                                        <asp:CompareValidator ID="CompareValidator1" ControlToValidate="txtAnswerApplicationDeadline$txtDatetime"
                                            ValidationGroup="SaveAnswerPopUp" ControlToCompare="txtAnswerStartDate$txtDatetime"
                                            resourcekey="ApplicationDeadlineGreaterStartDate.Text" Type="Date" runat="server" Display="Dynamic"
                                            Operator="GreaterThanEqual" />
                                    </div>
                                    <%--<div class="cb">
                                        <asp:CompareValidator ID="CompareValidator2" ControlToValidate="txtAnswerApplicationDeadline$txtDatetime"
                                            ValidationGroup="SaveAnswerPopUp" ControlToCompare="txtAnswerEndDate$txtDatetime"
                                            resourcekey="ApplicationDeadlineGreaterEndDate.Text" Type="Date" runat="server" Display="Dynamic"
                                            Operator="GreaterThanEqual" />
                                    </div>--%>
                                    <%--<div>
                                        <asp:CompareValidator ID="CompareValidator2" runat="server" Type="Date" Operator="DataTypeCheck"
                                            Display="Dynamic" ControlToValidate="txtAnswerApplicationDeadline$txtDatetime"
                                            ErrorMessage="Please enter a valid date." ValidationGroup="SaveAnswerPopUp">
                                        </asp:CompareValidator>
                                    </div>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="Label4" resourcekey="ExamProductId.Text"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" CssClass="width250" ID="txtProductCode"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator3"
                                        ControlToValidate="txtProductCode" Display="Dynamic" ValidationGroup="SaveAnswerPopUp"></asp:RequiredFieldValidator>
                                    <div>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtProductCode"
                                            resourcekey="ValuesLess999999999999.Text" ValidationExpression="^\d{0,12}$" Display="Dynamic"
                                            ValidationGroup="SaveAnswerPopUp"></asp:RegularExpressionValidator>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="Label5" resourcekey="ApplicationProductId.Text"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" CssClass="width250" ID="txtApplicationProductId"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator4"
                                        ControlToValidate="txtApplicationProductId" Display="Dynamic" ValidationGroup="SaveAnswerPopUp"></asp:RequiredFieldValidator>
                                    <div>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtApplicationProductId"
                                            resourcekey="ValuesLess999999999999.Text" ValidationExpression="^\d{0,12}$" Display="Dynamic"
                                            ValidationGroup="SaveAnswerPopUp"></asp:RegularExpressionValidator>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="text-center">
                        <asp:Button runat="server" ID="btnSaveAnswer" resourcekey="OK.Text" ValidationGroup="AddAnswerGroup" />
                        <asp:Button runat="server" ID="btnSaveExamChoiceAnswer" resourcekey="OK.Text" ValidationGroup="SaveAnswerPopUp" />
                        <asp:Button runat="server" ID="btnCancelAnswer" resourcekey="Cancel.Text" CausesValidation="False" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
