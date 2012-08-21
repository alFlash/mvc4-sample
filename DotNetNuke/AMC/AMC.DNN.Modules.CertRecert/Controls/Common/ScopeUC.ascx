<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ScopeUC.ascx.vb" Inherits="AMC.DNN.Modules.CertRecert.Controls.Common.ScopeUC" %>
<script type="text/javascript">
    var ScopeUC_CPTCodeList = IsStringNullOrEmpty('<%= CPTCodeListJSon %>') ? null : eval('(' + '<%= CPTCodeListJSon %>' + ')');
    function showPracticePopup(isShow) {
        AmcCert.showPracticePopup("add-scope-popup", isShow);
        AmcCert.SetTitle("add-scope-popup", "Add Information ");
    }
    jQuery(document).ready(function () {
        var bindingList = [['lblScopeTypeValue', 'ddlScopetype'],
                           ['lblCPTCodeValue', 'ddlCPTType'],
                           ['lblCPTCodeValue', 'txtProCode'], 
                           ['lblNumberOfService', 'txtNumberOfService'],
                           ['lblAverageTime', 'txtAverageTime']
                           ];
        var practicePopup = new AMCTablePopUp(bindingList,
           "scope-uc",
            "tblscope",
            "add-scope-popup",
            "Add Information ",
            "Edit Item ",
            null);
    });

    var rqProCodeClientID = '<%= rqProCode.ClientID %>';


//    function checkRequireField(scopeTypeValue) {
//        var requireProCodeClientID = '<%= rqProCode.ClientID %>';
//        var myVal = document.getElementById(requireProCodeClientID);
//        if (scopeTypeValue = "PROCEDURES") {
//            ValidatorEnable(myVal, false);
//            jQuery(myVal).hide();
//        } else {
//            ValidatorEnable(myVal, true);
//            jQuery(myVal).hide();
//        }
//    }

</script>
<div id="scope-uc" class="amc-page">
    <asp:HiddenField runat="server" ID="hdCurrentSelectedCptCode"/>
    <asp:HiddenField runat="server" ID="hdCurrentSelectedScopeType"/>
    <asp:HiddenField runat="server" ID="hdIsIncomplete" />
    <asp:HiddenField runat="server" ID="hdIsValidateFailed" />
    <asp:HiddenField runat="server" ID="hdCurrentObjectUniqueId" />
    <div class="amc-title">
        <asp:Label runat="server" ID="lblscopeUC"></asp:Label>
    </div>
    <div id="FormFillingInstructions" runat="server" class="amc-instruction">
        <asp:Label runat="server" ID="lblFormFillingInstructions"></asp:Label>
    </div>
    <div class="amc-contents">
        <div>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtUniquePatientResponse"
                                    resourcekey="AnswerScope.Text" ValidationExpression="^\d{0,12}$"
                                    Display="Dynamic" ValidationGroup="AmcGeneralGroup"></asp:RegularExpressionValidator>
        </div>
        <br />
        <div runat="server" id="Question" Visible="False">
            <asp:HiddenField runat="server" ID="hdCurrentSurveyId" />
            <asp:HiddenField runat="server" ID="hdUniquePatientQuestionId" />
            <asp:HiddenField runat="server" ID="hdUniquePatientAnswerId" />
            <asp:HiddenField runat="server" ID="hdUniquePatientResponseId" />
            <asp:Label runat="server" ID="lblUniquePatientQuestion"></asp:Label>
            <asp:TextBox runat="server" CssClass="width250" ID="txtUniquePatientResponse"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator1" ControlToValidate="txtUniquePatientResponse"
                            EnableClientScript="True" Display="Dynamic" ValidationGroup="AmcGeneralGroup"></asp:RequiredFieldValidator>
        </div>
        <br />
        <div id="add-new-row" class="amc-add-instruction">
            <asp:Image CssClass="pointer fl" runat="server" ID="Image1" ImageUrl="../../Documentation/images/icons/addnew_11x11.gif">
            </asp:Image>
            <asp:HyperLink runat="server" ID="hlAddNew" resourcekey="AddInformation.Text" CssClass="fl padleft5" href="javascript:void(0);"></asp:HyperLink>
            <%--hlAddNew--%>
            <div class="cb">
            </div>
        </div>
        <div>
            <asp:Repeater runat="server" ID="rptscope" EnableViewState="True">
                <HeaderTemplate>
                    <table id="tblscope" class="amc-table">
                        <tr class="amc-table-header">
                            <td runat="server" visible='<%# GetFieldInfo("Scopetype").IsEnabled %>'>
                                <asp:Label runat="server" ID="lblScopetypeHeader" Text='<%# GetFieldInfo("Scopetype").FieldValue %>'></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("CPTCode").IsEnabled %>'>
                                <asp:Label runat="server" ID="lblCPTCodeHeader" Text='<%# GetFieldInfo("CPTCode").FieldValue %>'></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("NumberOfService").IsEnabled %>'>
                                <asp:Label runat="server" ID="lblNumberOfServiceHeader" Text='<%# GetFieldInfo("NumberOfService").FieldValue %>'></asp:Label>
                            </td>
                            <td runat="server" visible='<%# GetFieldInfo("AverageTime").IsEnabled %>'>
                                <asp:Label runat="server" ID="lblAverageTimeHeader" Text='<%# GetFieldInfo("AverageTime").FieldValue %>'></asp:Label>
                            </td>
                            <td class="action-column">
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="amc-table-content">
                        <td runat="server" visible='<%# GetFieldInfo("Scopetype").IsEnabled %>'>
                            <asp:Label runat="server" ID="lblScopetype"></asp:Label>
                            <asp:Label runat="server" ID="lblScopeTypeValue" CssClass="dn"></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("CPTCode").IsEnabled %>'>
                            <asp:Label runat="server" ID="lblCPTCode"></asp:Label>
                            <asp:Label runat="server" ID="lblCPTCodeValue" CssClass="dn"></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("NumberOfService").IsEnabled %>' class="text-right">
                            <asp:Label runat="server" ID="lblNumberOfService"></asp:Label>
                        </td>
                        <td runat="server" visible='<%# GetFieldInfo("AverageTime").IsEnabled %>' class="text-right">
                            <asp:Label runat="server" ID="lblAverageTime"></asp:Label>
                        </td>
                        <td class="action-column">
                            <asp:HiddenField runat="server" ID="hdObjectUniqueId" Value='<%# Eval("Guid") %>' />
                            <div class="edit-image fl" id="imgEdit">
                            </div>
                            <asp:ImageButton CssClass="pointer" runat="server" ID="imgItemDelete" ImageUrl="../../Documentation/images/icons/delete_icon1.gif"
                                CommandName="Delete" CommandArgument='<%# Eval("Guid") %>' CausesValidation="False"></asp:ImageButton>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
    <div id="add-scope-popup" class="amc-popup">
        <asp:Panel runat="server" ID="panelPopup" DefaultButton="btnSave">
        <div>
            <div class="amc-error-message">
                <asp:Label runat="server" ID="lblPopupMessage"></asp:Label>
                <div>
                    <asp:ValidationSummary runat="server" ID="vldsPopupRequiredGroup" ValidationGroup="PopupRequiredGroup" />
                </div>
            </div>
            <table class="width500">
                <tr id="Scopetype" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblScopetype"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlScopetype">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqScopetype" ControlToValidate="ddlScopetype"
                            EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="CPTCode" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblCPTCode"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlCPTType"></asp:DropDownList>
                        <%--<asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqCPTCode" ControlToValidate="ddlCPTType"
                            EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr id="ProCode" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblProCode"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtProCode"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqProCode" ControlToValidate="txtProCode"
                            EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="NumberOfService" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblNumberOfService"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtNumberOfService"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqNumberOfService"
                            ControlToValidate="txtNumberOfService" EnableClientScript="True" Display="Dynamic"
                            ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtNumberOfService"
                                resourcekey="NumberValues.Text" ValidationExpression="^\d{0,12}$" Display="Dynamic"
                                ValidationGroup="PopupRequiredGroup"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
                <tr id="AverageTime" runat="server">
                    <td>
                        <asp:Label runat="server" ID="lblAverageTime"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtAverageTime"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqAverageTime" ControlToValidate="txtAverageTime"
                            EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtAverageTime"
                                resourcekey="NumberValues.Text" ValidationExpression="^\d{0,12}$" Display="Dynamic"
                                ValidationGroup="PopupRequiredGroup"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
            </table>
            <div class="pad5">
                <asp:Button ID="btnSave" runat="server" Text="OK" ValidationGroup="PopupRequiredGroup" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
            </div>
        </div>
        </asp:Panel>
    </div>
</div>
<div class="page-break"></div>
