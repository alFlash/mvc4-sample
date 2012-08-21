<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ProfessionalPracticeSetting.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.Common.ProfessionalPracticeSetting" %>
<%@ Register TagPrefix="amc" TagName="DateTime" Src="../../Controls/Reusable/AMCDatetime.ascx" %>
<div class="amc-page" id="uc-practice-setting">
<asp:HiddenField runat="server" ID="hdIsIncomplete" />
<asp:HiddenField runat="server" ID="hdIsValidateFailed" />
<asp:HiddenField runat="server" ID="hdSurveyId" Value="0"/>
    <div class="amc-title">
        <asp:Label runat="server" ID="lblProfessionalPracticeSetting"></asp:Label>
    </div>
    <div id="FormFillingInstructions" runat="server" class="amc-instruction">
        <asp:Label runat="server" ID="lblFormFillingInstructions" Text="Please check boxes below."></asp:Label>
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
                    <li style="padding-left: 0px" runat="server" Visible='<%# Eval("Enabled") %>'>
                        <div>
                             <asp:HiddenField runat="server" ID="hdQuestionEnabled" Value='<%# Eval("Enabled") %>'/>
                            <asp:HiddenField runat="server" ID="hdQuestionId" Value='<%# Eval("QuestionId") %>' />
                            <asp:HiddenField runat="server" ID="hdAnswerYes" Value="0"/>
                            <asp:HiddenField runat="server" ID="hdAnswerNo" Value="0" />
                            <asp:HiddenField runat="server" ID="hdResponseId" Value="0"/>
                            <asp:CheckBox runat="server" ID="chkYesNo" Text='<%# Eval("QuestionText") %>' />
                        </div>
                        
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>
<div class="page-break"></div>
