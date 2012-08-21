<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ExamChoiceUC.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.Common.ExamChoiceUC" %>
<div id="amc-exam-choice" class="amc-page">
<asp:HiddenField runat="server" ID="hdIsIncomplete" />
<asp:HiddenField runat="server" ID="hdIsValidateFailed" />
<asp:HiddenField runat="server" ID="hdSurveyId"  Value="0" />
<asp:HiddenField runat="server" ID="hdExamChoiceQuestionId" Value="0"/>
<asp:HiddenField runat="server" ID="hdExamChoiceResponseId" Value="0"/>
<asp:HiddenField runat="server" ID="hdExamChoiceResponseAnswerId" Value="0"/>
    <div class="amc-title">
            <asp:Label runat="server" ID="lblExamChoiceUC"></asp:Label>
    </div>
    <div id="FormFillingInstructions" runat="server" class="amc-instruction">
        <asp:Label runat="server" ID="lblFormFillingInstructions"></asp:Label>
    </div>
    <div class="amc-contents">
        <asp:Repeater runat="server" ID="rptExamChoice">
            <HeaderTemplate>
                <table id="tblExamChoice" class="amc-table">
                    <tr class="amc-table-header">
                        <td>
                            <asp:Label runat="server" ID="lblExamAdministrationHeader" resourcekey="ExamAdministration.Text"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblApplicationDeadlineHeader" resourcekey="ApplicationDeadline.Text"></asp:Label>
                        </td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="amc-table-content" id="trExamChoiceItem" runat="server">
                    <td>
                        <asp:RadioButton runat="server" ID="rdbExamAdministration" />
                        <asp:Label runat="server" ID="lblExamAdministration" ></asp:Label>
                        <asp:HiddenField runat="server" ID="hdAnswerId" Value='<%# Eval("AnswerId") %>'/>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblExamDate"></asp:Label>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</div>
<div class="page-break"></div>
