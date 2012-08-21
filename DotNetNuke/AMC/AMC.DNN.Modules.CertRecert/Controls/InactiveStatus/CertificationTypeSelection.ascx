<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CertificationTypeSelection.ascx.vb" Inherits="AMC.DNN.Modules.CertRecert.Controls.InactiveStatus.CertificationTypeSelection" %>

<div class="amc-error-message" id="certificationTypeSelectionError" runat="server" Visible="False">
    <asp:Label runat="server" ID="lblErrorMessage" resourcekey="CertificationCodeMissingError.Text"></asp:Label>
</div>
<div id="certificationTypeSelectionContainer" runat="server">
 <div>
        <table>
            <tr id="rInActive" runat="server">
                <td>
                    <asp:HyperLink ID="hlrInActive" runat="server"></asp:HyperLink>
                </td>
            </tr>
            <tr id="rReCert" runat="server">
                <td>
                    <asp:HyperLink ID="hlrReCert" runat="server"></asp:HyperLink>
                    <asp:Label runat="server" ID="lblRecertIsProcess" Text="Your recertification application is being processed"></asp:Label>
                </td>
            </tr>
            <tr id="rCert" runat="server">
                <td>
                    <asp:HyperLink ID="hlrCert" runat="server"></asp:HyperLink>
                     <asp:Label runat="server" ID="lblCertIsProcess" Text="Your certification application is being processed"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HyperLink ID="hlAdminControlPanel" runat="server"></asp:HyperLink>
                </td>
            </tr>
        </table>
 </div>
 <div class="btsubmit" style="display: none">
        <asp:Button ID="btnOK" runat="server" Text="Button" resourcekey="btnOK.Text"  />
 </div>
 </div>