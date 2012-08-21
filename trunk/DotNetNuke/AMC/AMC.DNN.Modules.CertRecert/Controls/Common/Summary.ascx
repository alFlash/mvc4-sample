<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Summary.ascx.vb" Inherits="AMC.DNN.Modules.CertRecert.Controls.Common.Summary"
    EnableViewState="true" %>
<div id="summary-uc" class="amc-page">
    <asp:HiddenField runat="server" ID="hdIsIncomplete" />
    <asp:HiddenField runat="server" ID="hdIsValidateFailed" />
    <div class="amc-title">
        <asp:Label runat="server" ID="lblSummary"></asp:Label>
    </div>
    <div id="FormFillingInstructions" runat="server" class="amc-instruction">
        <asp:Label runat="server" ID="lblFormFillingInstructions"></asp:Label>
    </div>
    <div class="amc-contents">
        <asp:Repeater runat="server" ID="rptSummary">
            <HeaderTemplate>
                <table id="tblSummary" border="1px" class="amc-table">
                    <tr class="amc-table-header">
                        <td visible='<%# GetFieldInfo("Categories").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblCategory" Text='<%# GetFieldInfo("Categories").FieldValue %>'></asp:Label>
                        </td>
                        <td visible='<%# GetFieldInfo("TotalCE").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblCEEarned" Text='<%# GetFieldInfo("TotalCE").FieldValue %>'></asp:Label>
                        </td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="amc-table-content">
                    <td visible='<%# GetFieldInfo("Categories").IsEnabled %>' runat="server">
                        <asp:Label runat="server" ID="lblCategoryName" Text='<%# Eval("CECategoryName") %>'></asp:Label>
                    </td>
                    <td visible='<%# GetFieldInfo("TotalCE").IsEnabled %>' runat="server" style="text-align: right">
                        <asp:Label runat="server" ID="lblTotalCE" Text='<%# Eval("TotalCEString") %>'></asp:Label>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</div>
<div class="messagetotalCE">
    <asp:Label runat="server" ID="lblOptionTitle"></asp:Label>
</div>
<div class="totalCE">
    Total Points:
    <asp:Label runat="server" ID="lblTotalCE"></asp:Label>
</div>
<div class="page-break">
</div>
