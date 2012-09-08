<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginUC.ascx.cs" Inherits="Spring.Net.Sample.Web.UserControls.LoginUC" %>

<%--Remember to use "spring:Panel" to use DI--%>
<spring:Panel runat="server" ID="diContainer" SuppressDependencyInjection="true" RenderContainerTag="false">
    Username:
            <asp:TextBox runat="server" ID="txtUsername"></asp:TextBox><br />
    Password:
            <asp:TextBox runat="server" ID="txtPassword"></asp:TextBox><br />
    <asp:Button runat="server" ID="btnLogin" Text="Login" />
    <asp:Button runat="server" ID="btnCancel" Text="Cancel" /><br />
    <asp:Label runat="server" ID="lblMessage"></asp:Label>
</spring:Panel>
