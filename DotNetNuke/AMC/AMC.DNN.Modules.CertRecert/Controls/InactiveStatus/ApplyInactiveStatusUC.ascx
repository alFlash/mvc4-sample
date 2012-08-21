<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ApplyInactiveStatusUC.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.InactiveStatus.ApplyInactiveStatusUC" %>
<%@ Register TagPrefix="amc" TagName="DateTime" Src="../../Controls/Reusable/AMCDatetime.ascx" %>
<script>
    var PaymentProcessed = '<%= PaymentProcessed %>';
</script>
<div class="amc-page" id="apply-inactive-status-uc">
    <div class="amc-title">
        <asp:Label ID="lblApplyInactiveStatusUC" runat="server"></asp:Label>
    </div>
    <div id="FormFillingInstructions" runat="server" class="amc-instruction">
        <asp:Label runat="server" ID="lblFormFillingInstructions"></asp:Label>
    </div>
    <asp:HiddenField runat="server" ID="hdIsIncomplete" />
    <asp:HiddenField runat="server" ID="hdIsValidateFailed" />
    <asp:HiddenField runat="server" ID="hdSurveyId" Value="0" />
    <div class="amc-contents">
        <div class="amc-main-content">
            <asp:Label runat="server" ID="lblNote" ForeColor="Red" Font-Bold="True"></asp:Label>
            <div id="QuestionList" runat="server">
                <asp:Repeater runat="server" ID="rptQualifyingEvent">
                    <HeaderTemplate>
                        <ul id="ulSQualifyingEvent" style="list-style: none; padding-left: 0px">
                    </HeaderTemplate>
                    <FooterTemplate>
                        </ul>
                    </FooterTemplate>
                    <ItemTemplate>
                        <li style="padding-left: 0px"  runat="server" Visible='<%# Eval("Enabled") %>'>
                            <div>
                                <asp:HiddenField runat="server" ID="hdQuestionEnabled" Value='<%# Eval("Enabled") %>'/>
                                <asp:HiddenField runat="server" ID="hdQuestionId" Value='<%# Eval("QuestionId") %>' />
                                <asp:HiddenField runat="server" ID="hdAnswerYes" Value="0" />
                                <asp:HiddenField runat="server" ID="hdAnswerNo" Value="0" />
                                <asp:HiddenField runat="server" ID="hdResponseId" Value="0" />
                                <asp:CheckBox runat="server" ID="chkYesNo" Text='<%# Eval("QuestionText") %>' />
                            </div>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
    <div style="padding-bottom: 20px;" class="amc-main-footer">
        <asp:Button ID="btnSubmit" runat="server" resourcekey="btnSubmitApp.Text" />
        <asp:Button ID="btnCancel" runat="server" resourcekey="btnCancelApp.Text" />
    </div>
</div>
