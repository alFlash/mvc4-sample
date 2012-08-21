<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="RecertEligibilityUC.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.Common.RecertEligibilityUC" %>
<%@ Register TagPrefix="amc" TagName="DateTime" Src="../Reusable/AMCDatetime.ascx" %>
<script type="text/javascript">

    jQuery(document).ready(function () {
        //add new licensure
        var bindingList = [['lblStateType', 'ddlState'],
                           ['lblLicenseNumber', 'txtLicenseNumber'],
                           ['lblExpirationDate', 'txtExpirationDate'],
                           ['lblDateOfOriginalIssue', 'txtDateOfOriginalIssue']];

        var practicePopup = new AMCTablePopUp(bindingList,
            "uc_eligibility",
            "tbl-licensure",
            "popupAddLicensure",
            "Add Information ",
            "Edit Item",
            null);
    });
    
</script>
<div id="uc_eligibility" class="amc-page">
    <asp:HiddenField runat="server" ID="hdIsIncomplete" />
    <asp:HiddenField runat="server" ID="hdIsValidateFailed" />
    <asp:HiddenField runat="server" ID="hdSurveyId" Value="0" />
    <asp:HiddenField runat="server" ID="hdCurrentObjectUniqueId" />
    <div class="amc-title">
        <asp:Label runat="server" ID="lblRecertEligibilityUC"></asp:Label>
    </div>
    <div id="FormFillingInstructions" runat="server" class="amc-instruction">
        <asp:Label runat="server" ID="lblFormFillingInstructions"></asp:Label>
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
                    <li style="padding-left: 0px" id="liQuestionItem" runat="server" visible='<%# Eval("Enabled") %>'>
                        <div>
                            <asp:HiddenField runat="server" ID="hdQuestionEnabled" Value='<%# Eval("Enabled") %>' />
                            <asp:HiddenField runat="server" ID="hdQuestionId" Value='<%# Eval("QuestionId") %>' />
                            <asp:HiddenField runat="server" ID="hdAnswerYes" Value="0" />
                            <asp:HiddenField runat="server" ID="hdAnswerNo" Value="0" />
                            <asp:HiddenField runat="server" ID="hdResponseId" Value="0" />
                            <asp:HiddenField runat="server" ID="hdQuestionCode" Value='<%# Eval("QuestionCode")  %>' />
                            <asp:Label runat="server" ID="lblQuestion" Text='<%# Eval("QuestionText") %>'></asp:Label>
                        </div>
                        <div style="width: 150px">
                            <asp:RadioButtonList runat="server" ID="rdlAnswer" RepeatDirection="Horizontal">
                            </asp:RadioButtonList>
                        </div>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
<%--            <div id="divSelectedOption" runat="server" style="padding: 0 20px 20px;" visible="False">
                <asp:Label runat="server" ID="lblSelectedOption"></asp:Label>
            </div>--%>
        </div>
        <div id="LicensureContainer" runat="server">
            <div id="add-new-row" class="amc-add-instruction">
                <asp:Image CssClass="pointer fl" runat="server" ID="Image1" ImageUrl="../../Documentation/images/icons/addnew_11x11.gif">
                </asp:Image>
                <asp:HyperLink runat="server" ID="hlAddNew" resourcekey="AddInformation.Text" CssClass="fl padleft5"
                    href="javascript:void(0);"></asp:HyperLink>
                <div class="cb">
                </div>
            </div>
            <asp:Repeater ID="rptLicensure" runat="server" EnableViewState="True">
                <HeaderTemplate>
                    <table id="tbl-licensure" border="1px" class="amc-table">
                        <tr class="amc-table-header">
                            <td id="Td1" runat="server" visible='<%# GetFieldInfo("StateOf").IsEnabled %>'>
                                <asp:Label ID="Label1" runat="server" Text='<%# GetFieldInfo("StateOf").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td2" runat="server" visible='<%# GetFieldInfo("LicenseNumber").IsEnabled %>'>
                                <asp:Label ID="Label2" runat="server" Text='<%# GetFieldInfo("LicenseNumber").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td3" runat="server" visible='<%# GetFieldInfo("ExpirationDate").IsEnabled %>'>
                                <asp:Label ID="Label3" runat="server" Text='<%# GetFieldInfo("ExpirationDate").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td5" runat="server" visible='<%# GetFieldInfo("DateOfOriginalIssue").IsEnabled %>'>
                                <asp:Label ID="Label4" runat="server" Text='<%# GetFieldInfo("DateOfOriginalIssue").FieldValue %>'></asp:Label>
                            </td>
                            <td class="action-column">
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="amc-table-content" id="item-content">
                        <td id="Td6" runat="server" visible='<%# GetFieldInfo("StateOf").IsEnabled %>'>
                            <asp:Label ID="lblStateOf" runat="server"></asp:Label>
                            <asp:Label ID="lblStateType" runat="server" CssClass="dn" Text='<%# Eval("IssuingBodyString") %>'></asp:Label>
                        </td>
                        <td id="Td7" runat="server" visible='<%# GetFieldInfo("LicenseNumber").IsEnabled %>'>
                            <asp:Label ID="lblLicenseNumber" runat="server" Text='<%# Eval("IssuedNumber") %>'></asp:Label>
                        </td>
                        <td id="Td8" runat="server" visible='<%# GetFieldInfo("ExpirationDate").IsEnabled %>'>
                            <asp:Label ID="lblExpirationDate" runat="server"></asp:Label>
                        </td>
                        <td id="Td4" runat="server" visible='<%# GetFieldInfo("DateOfOriginalIssue").IsEnabled %>'>
                            <asp:Label ID="lblDateOfOriginalIssue" runat="server"></asp:Label>
                        </td>
                        <td class="action-column">
                            <asp:HiddenField runat="server" ID="hdObjectUniqueId" Value='<%# Eval("Guid") %>' />
                            <asp:Image CssClass="pointer" runat="server" ID="imgEdit" ImageUrl="../../Documentation/images/icons/EditIcon.gif">
                            </asp:Image>
                            <asp:ImageButton CssClass="pointer" runat="server" ID="imgItemDelete" ImageUrl="../../Documentation/images/icons/delete_icon1.gif"
                                CommandName="Delete" CommandArgument='<%# Eval("Guid") %>' CausesValidation="False">
                            </asp:ImageButton>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <div id="Declaration" style="margin-top: 5px" runat="server">
            <asp:Repeater runat="server" ID="rptAgreement" EnableViewState="True">
                <HeaderTemplate>
                    <ul id="ulAgreement" style="list-style: none; padding-left: 0px">
                </HeaderTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
                <ItemTemplate>
                    <li style="padding-left: 0px" id="liQuestionItem" runat="server" visible='<%# Eval("Enabled") %>'>
                        <div>
                            <asp:HiddenField runat="server" ID="hdQuestionEnabled" Value='<%# Eval("Enabled") %>' />
                            <asp:HiddenField runat="server" ID="hdQuestionId" Value='<%# Eval("QuestionId") %>' />
                            <asp:HiddenField runat="server" ID="hdAnswerYes" Value="0" />
                            <asp:HiddenField runat="server" ID="hdAnswerNo" Value="0" />
                            <asp:HiddenField runat="server" ID="hdResponseId" Value="0" />
                            <asp:HiddenField runat="server" ID="hdQuestionCode" Value='<%# Eval("QuestionCode")  %>' />
                            <asp:CheckBox runat="server" ID="chkQuestion" Text='<%# Eval("QuestionText") %>'>
                            </asp:CheckBox>
                        </div>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>
<div class="page-break">
</div>
<!-- Popup -->
<div id="popupAddLicensure" class="amc-popup">
    <asp:Panel runat="server" ID="panelPopup" DefaultButton="btnSave">
        <div class="amc-error-message">
            <asp:Label runat="server" ID="lblPopupMessage"></asp:Label>
            <div>
                <asp:ValidationSummary runat="server" ID="vldsPopupRequiredGroup" ValidationGroup="PopupRequiredGroup" />
            </div>
        </div>
        <table>
            <tr id="StateOf" runat="server">
                <td>
                    <asp:Label ID="lblStateOf" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlState">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="LicenseNumber" runat="server">
                <td>
                    <asp:Label ID="lblLicenseNumber" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtLicenseNumber" runat="server" CssClass="width250"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqLicenseNumber"
                        ControlToValidate="txtLicenseNumber" EnableClientScript="True" ValidationGroup="PopupRequiredGroup"
                        Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="ExpirationDate" runat="server" validationgroup="PopupRequiredGroup">
                <td>
                    <asp:Label ID="lblExpirationDate" runat="server"></asp:Label>
                </td>
                <td>
                    <amc:DateTime runat="server" ID="txtExpirationDate" ValidationGroup="PopupRequiredGroup"
                        CssClass="fl"></amc:DateTime>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqExpirationDate"
                        ControlToValidate="txtExpirationDate$txtDatetime" EnableClientScript="True" ValidationGroup="PopupRequiredGroup"
                        CssClass="fl" Display="Dynamic"></asp:RequiredFieldValidator>
                    <div class="cb">
                    </div>
                    <div>
                        <asp:CompareValidator ID="Compare1" ControlToValidate="txtExpirationDate$txtDatetime"
                            ControlToCompare="txtDateOfOriginalIssue$txtDatetime" resourcekey="CompareDateControlSubs.Text"
                            Type="Date" runat="server" Display="Dynamic" Operator="GreaterThanEqual" ValidationGroup="PopupRequiredGroup" />
                    </div>
                </td>
            </tr>
            <tr id="DateOfOriginalIssue" runat="server">
                <td>
                    <asp:Label ID="lblDateOfOriginalIssue" runat="server"></asp:Label>
                </td>
                <td>
                    <amc:DateTime runat="server" ID="txtDateOfOriginalIssue" ValidationGroup="PopupRequiredGroup"
                        CssClass="fl"></amc:DateTime>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqDateOfOriginalIssue"
                        ControlToValidate="txtDateOfOriginalIssue$txtDatetime" EnableClientScript="True"
                        ValidationGroup="PopupRequiredGroup" CssClass="fl" Display="Dynamic"></asp:RequiredFieldValidator>
                    <div class="cb">
                    </div>
                    <div>
                        <asp:CompareValidator runat="server" ID="cpWithToday" ValidationGroup="PopupRequiredGroup"
                            Type="Date" ControlToValidate="txtDateOfOriginalIssue$txtDatetime" Display="Dynamic"
                            Operator="LessThanEqual" resourcekey="cpWithToday.Text">
                        </asp:CompareValidator>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnSave" runat="server" Text="OK" ValidationGroup="PopupRequiredGroup" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClientClick="AmcCert.ShowPopUp('popupAddLicensure', false);return false;" />
                    <br />
                </td>
            </tr>
        </table>
    </asp:Panel>
</div>
