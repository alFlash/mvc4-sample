<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginUC.ascx.cs" Inherits="MVP.Sample.Web.UserControls.LoginUC" %>
<div id="login">
    <table style="border: 0px; border-spacing: 0px;">
        <tr>
            <td colspan="2">
                <asp:Label runat="server" ID="lblMessage"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 200px;">
                Username: 
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtUsername"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 200px;">
                Password:
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtPassword" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button runat="server" ID="btnLogin" Text="Login"/>
                <asp:Button runat="server" ID="btnCancel" Text="Cancel"/>
            </td>
        </tr>
    </table>
</div>