<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ValidationRuleSettingsUC.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.FormConfigurations.ValidationRuleSettingsUC" %>
<div id="validation-rule-settings-uc" class="amc-page">
    <div class="amc-title">
        <asp:Label runat="server" ID="lblValidationRuleInstruction" resourcekey="ValidationRuleSettings.Text"></asp:Label>
    </div>
    <div class="amc-contents">
        <asp:Repeater runat="server" ID="rptValidationRules">
            <HeaderTemplate>
                <table class="amc-table">
                    <tr class="amc-table-header">
                        <td>
                            <asp:Label ID="Label1" runat="server" resourcekey="Description.Text"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" resourcekey="IsEnabled.Text"></asp:Label>
                        </td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:HiddenField runat="server" ID="hdValidationRuleId" Value='<%# Eval("Id") %>' />
                        <asp:Label runat="server" ID="lblValidationRuleDescription" Text='<%# Eval("Description") %>'></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkValidationRuleIsEnabled" Checked='<%# Eval("IsEnabled") %>' />
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                <tr>
                    <td colspan="2" class="text-center">
                        <asp:Button runat="server" ID="btnSaveValidationRules" resourcekey="Save.Text" CommandName="SaveValidationRules" />
                    </td>
                </tr>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</div>
