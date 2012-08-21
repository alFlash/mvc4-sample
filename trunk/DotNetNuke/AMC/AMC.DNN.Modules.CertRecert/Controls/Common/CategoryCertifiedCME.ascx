<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CategoryCertifiedCME.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.Common.CategoryCertifiedCME" %>
<%@ Register TagPrefix="amc" TagName="DateTime" Src="../../Controls/Reusable/AMCDatetime.ascx" %>
<script type="text/javascript">
//    jQuery(document).ready(function () {
//        var hdQuestionCodeCtrl = jQuery('#ulCategoryCertifiedCME #ques-li-item #[id*=hdQuestionCode]');
//        jQuery.each(jQuery('#ulCategoryCertifiedCME #ques-li-item #[id*=hdQuestionCode]'), function (idx, item) {
//            if (item.val() == 'CATEGORY_CERTIFIED_CME_FELLOWSHIP') {
//                var chkYesNoCtrl = hdQuestionCodeCtrl.parents('#ulCategoryCertifiedCME').find('#[id*=chkYesNo]');
//                if (chkYesNoCtrl.length > 0) {
//                    jQuery('#ulCategoryCertifiedCME').delegate('#[id*=chkYesNo]', 'click', function() {
//                        if(chkYesNoCtrl.prop('checked') == true) {
//                            var hdQuestionCodeCMEEarnsCtrl = jQuery('#ulCategoryCertifiedCME #ques-li-item #[id*=hdQuestionCode]');
//                            
//                        }
//                    });
//                }

//            }
//        });

//    });
</script>
<div class="amc-page" id="uc-category-certificated-cme">
<asp:HiddenField runat="server" ID="hdIsIncomplete" />
<asp:HiddenField runat="server" ID="hdIsValidateFailed" />
<asp:HiddenField runat="server" ID="hdSurveyId" Value="0"/>
    <div class="amc-title">
        <asp:Label runat="server" ID="lblCategoryCertifiedCME"></asp:Label>
    </div>
    <div id="FormFillingInstructions" runat="server" class="amc-instruction">
        <asp:Label runat="server" ID="lblFormFillingInstructions" Text="Please check boxes below."></asp:Label>
    </div>
    <div class="amc-contents">
        <div id="QuestionList" runat="server">
            <asp:Repeater runat="server" ID="rptQuestionnaire" EnableViewState="True">
                <HeaderTemplate>
                    <ul id="ulCategoryCertifiedCME" style="list-style-image: none; padding-left: 0px">
                </HeaderTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
                <ItemTemplate>
                    <li id="quesliitem" style="padding-left: 0px" runat="server" Visible='<%# Eval("Enabled") %>'>
                            <asp:HiddenField runat="server" ID="hdQuestionEnabled" Value='<%# Eval("Enabled") %>'/>
                            <asp:HiddenField runat="server" ID="hdQuestionId" Value='<%# Eval("QuestionId") %>' />
                            <asp:HiddenField runat="server" ID="hdAnswerYes" Value="0"/>
                            <asp:HiddenField runat="server" ID="hdAnswerNo" Value="0" />
                            <asp:HiddenField runat="server" ID="hdAnswerRange" Value="0"/>
                            <asp:HiddenField runat="server" ID="hdResponseId" Value="0"/> 
                            <asp:HiddenField runat="server" ID="hdQuestionCode" Value='<%# Eval("QuestionCode") %>'/>
                            <asp:HiddenField runat="server" ID="hdQuestionType" Value='<%# Eval("QuestionTypeString")  %>'/>
                            <asp:CheckBox runat="server" ID="chkYesNo" Text='<%# Eval("QuestionText") %>' />    
                            <asp:Label runat="server" ID="lblQuestionText" Text='<%# Eval("QuestionText") %>'></asp:Label>
                            <asp:TextBox runat="server" CssClass="width250" ID="txtResponse" style="display: block" MaxLength="9"></asp:TextBox>
                             <div>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtResponse"
                                    resourcekey="IsNumber.Text" ValidationExpression="^[0-9]{1,9}(\.[0-9]{1,9})?$"
                                    Display="Dynamic" ValidationGroup="AmcGeneralGroup"></asp:RegularExpressionValidator>
                            </div>
                            <%--<div>
                                <asp:CompareValidator runat="server" ID="cpCMEHours" ControlToValidate="txtResponse" Visible="False" 
                                ValidationGroup="AmcGeneralGroup" EnableClientScript="True" Display="Dynamic" Type="Integer" 
                                Operator="GreaterThanEqual"></asp:CompareValidator>
                            </div>--%>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>
<div class="page-break"></div>
