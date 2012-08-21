<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AMCDatetime.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.Reusable.AMCDatetime" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<div runat="server" id="divAmcDateTimeContainer">
    <table style="width: auto;" border="0px" cellpadding="0px" cellspacing="0px">
        <tr>
            <td>
                <asp:TextBox runat="server" CssClass="width250" ID="txtDatetime"></asp:TextBox>
            </td>
            <td>
                <asp:ImageButton runat="server" ID="imgDate" CssClass="img-datepicker" ImageUrl="../../Documentation/images/icons/datepicker.gif" />
                <cc1:CalendarExtender ID="calendarExtender" runat="server" TargetControlID="txtDatetime"
                    Format="MM/dd/yyyy" PopupButtonID="imgDate">
                </cc1:CalendarExtender>
            </td>
        </tr>
    </table>
    <div>
        <asp:CompareValidator ID="vldDateTime" runat="server" Type="Date" Operator="DataTypeCheck"
            Display="Dynamic" ControlToValidate="txtDatetime" CultureInvariantValues="True"
            ErrorMessage="Please enter a valid date.">
        </asp:CompareValidator>
    </div>
</div>
