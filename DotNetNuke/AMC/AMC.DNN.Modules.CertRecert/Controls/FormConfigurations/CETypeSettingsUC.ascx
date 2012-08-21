<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CETypeSettingsUC.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.FormConfigurations.CETypeSettingsUC" %>
<script>
    var CEType_Setting_CETypesList = IsStringNullOrEmpty('<%= CETypesListJson %>') ? null : eval('(' + '<%= CETypesListJson %>' + ')');
</script>
<div id="CE-type-settings-uc" class="amc-page">
<asp:HiddenField runat="server" ID="hdCurrentSelectedProgramTypeCode" />
    <asp:HiddenField runat="server" ID="hdCurrentSelectedCETypeCode" />
    <asp:HiddenField runat="server" ID="hdCurrentSectionPopupOpenningId" />
    <asp:HiddenField runat="server" ID="hdCurrentSectionPopupOpenningTitle" />
    <asp:HiddenField runat="server" ID="hdCurrentObjectUniqueId" />
    <%--Program Type - ReCert Option 2 Settings--%>
    <div class="amc-title">
        <asp:Label runat="server" ID="Label6" resourcekey="CertificationCEWeightSettings.Text"></asp:Label>
    </div>
    <div class="amc-instruction bg-darkgrey">
        <asp:Label runat="server" ID="Label1" resourcekey="ProgramTypeSettingsOpt2.Text"></asp:Label>
    </div>
    <div class="amc-guidelines">
        <asp:Label runat="server" ID="lblProgramTypeSettingsGuideline"></asp:Label>
    </div>
    <div class="amc-contents">
        <div>
            <asp:Repeater runat="server" ID="rptProgramTypesOption2">
                <HeaderTemplate>
                    <table class="amc-table">
                        <tr class="amc-table-header">
                            <td>
                                Program Type Name
                            </td>
                            <td>
                                Min CE Of ReCert Option 2
                            </td>
                            <td>
                                Max CE Of ReCert Option 2
                            </td>
                            <td>
                                Min CE Of ReCert Option 3
                            </td>
                            <td>
                                Max CE Of ReCert Option 3
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr runat="server" Visible='<%# Eval("Code") <> "COMMSERV" AndAlso Eval("Code") <> "COMMSERVPRES" AndAlso Eval("Code") <> "COMMSERVPUB" AndAlso Eval("Code") <> "COMMSERVR" AndAlso Eval("Code") <> "COMMSERVVS" AndAlso Eval("Code") <> "EDUCATION" AndAlso Eval("Code") <> "PRACTICEEXP" AndAlso Eval("Code") <> "COMMSERVVL"%>'>
                        <td>
                            <asp:Label runat="server" ID="lblProgramTypeCode" Text='<%# Eval("Code") %>' CssClass="dn"></asp:Label>
                            <asp:Label runat="server" ID="lblProgramTypeName" Text='<%# Eval("Description") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width150" ID="txtMinCEOpt2"></asp:TextBox>
                            <div>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtMinCEOpt2"
                                    resourcekey="InputPoint.Text" ValidationExpression="^\d{0,12}$"
                                    Display="Dynamic" ValidationGroup="SaveProgramTypeOpt2Group"></asp:RegularExpressionValidator>
                            </div>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width150" ID="txtMaxCEOpt2"></asp:TextBox>
                            <div>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtMaxCEOpt2"
                                    resourcekey="InputPoint.Text" ValidationExpression="^\d{0,12}$"
                                    Display="Dynamic" ValidationGroup="SaveProgramTypeOpt2Group"></asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <asp:CompareValidator runat="server" ID="compareValidator1" ControlToValidate="txtMaxCEOpt2"
                                    ControlToCompare="txtMinCEOpt2" Operator="GreaterThanEqual" Type="Double" resourcekey="MinCELessThanMaxCE.Text"
                                    ValidationGroup="SaveProgramTypeOpt2Group" Display="Dynamic"></asp:CompareValidator>
                            </div>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width150" ID="txtMinCEOpt3"></asp:TextBox>
                            <div>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtMinCEOpt3"
                                    resourcekey="InputPoint.Text" ValidationExpression="^\d{0,12}$"
                                    Display="Dynamic" ValidationGroup="SaveProgramTypeOpt2Group"></asp:RegularExpressionValidator>
                            </div>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width150" ID="txtMaxCEOpt3"></asp:TextBox>
                            <div>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtMaxCEOpt3"
                                    resourcekey="InputPoint.Text" ValidationExpression="^\d{0,12}$"
                                    Display="Dynamic" ValidationGroup="SaveProgramTypeOpt2Group"></asp:RegularExpressionValidator>
                            </div>
                            <div>
                                <asp:CompareValidator runat="server" ID="compareValidator2" ControlToValidate="txtMaxCEOpt3"
                                    ControlToCompare="txtMinCEOpt3" Operator="GreaterThanEqual" Type="Double" resourcekey="MinCELessThanMaxCE.Text"
                                    ValidationGroup="SaveProgramTypeOpt2Group" Display="Dynamic"></asp:CompareValidator>
                            </div>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <tr>
                        <td colspan="5" class="text-center">
                            <asp:Button runat="server" ID="btnSave" Text="Save" CommandName="SaveProgramTypeOpt2"
                                ValidationGroup="SaveProgramTypeOpt2Group" />
                        </td>
                    </tr>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
    <br />
    <%--CEType Settings--%>
    <div class="amc-instruction bg-darkgrey">
        <asp:Label runat="server" ID="lblCEWeightSettingsHeader" resourcekey="CEWeightSettings.Text"></asp:Label>
    </div>
    <div class="amc-guidelines">
        <asp:Label runat="server" ID="lblCETypeSettingsGuideline"></asp:Label>
    </div>
    <div class="amc-contents" id="CE-type-settings-container">
        <div>
            <div class="amc-error-message">
                <asp:Label runat="server" ID="lblCEWeightMessage"></asp:Label>
            </div>
            <div id="add-new-row" class="amc-add-instruction">
                <asp:Image CssClass="pointer fl" runat="server" ID="Image1" ImageUrl="../../Documentation/images/icons/addnew_11x11.gif">
                </asp:Image>
                <asp:HyperLink runat="server" ID="hlAddNew" resourcekey="AddInformation.Text" CssClass="fl padleft5" href="javascript:void(0);"></asp:HyperLink>
                <div class="cb">
                </div>
            </div>
            <asp:Repeater runat="server" ID="rptCEWeightSettings">
                <HeaderTemplate>
                    <table id="CE-type-settings-table" border="1px" class="amc-table">
                        <tr class="amc-table-header">
                            <td>
                                <asp:Label runat="server" ID="lblProgramTypeHeader" resourcekey="ProgramTypeHeader.Text"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblCETypeHeader" resourcekey="CETypeHeader.Text"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblWeightHeader" resourcekey="CEWeightHeader.Text"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label2" resourcekey="MinCE.Text"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label3" resourcekey="MaxCE.Text"></asp:Label>
                            </td>
                            <td class="action-column">
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="CE-item amc-table-content" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblProgramTypeString" Text='<%# Eval("ProgramTypeString") %>'
                                CssClass="dn"></asp:Label>
                            <asp:Label runat="server" ID="lblProgramTypeDesc"></asp:Label>
                        </td>
                        <td>
                            <asp:HiddenField runat="server" ID="hdCETypeId" Value='<%# Eval("CEWeightId") %>' />
                            <asp:HiddenField runat="server" ID="hdObjectUniqueId" Value='<%# Eval("Guid") %>' />
                            <asp:Label runat="server" ID="lblCETypeString" Text='<%# Eval("CETypeString") %>'
                                CssClass="dn"></asp:Label>
                            <asp:Label runat="server" ID="lblCETypeDesc"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblWeightText"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblMinCE"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblMaxCE"></asp:Label>
                        </td>
                        <td class="action-column">
                            <asp:Image CssClass="pointer" runat="server" ID="imgEdit" ImageUrl="../../Documentation/images/icons/EditIcon.gif">
                            </asp:Image>
                            <asp:ImageButton CssClass="pointer" runat="server" ID="imgItemDelete" ImageUrl="../../Documentation/images/icons/delete_icon1.gif"
                                CommandName="Delete" CommandArgument='<%# Eval("Guid") %>' ValidationGroup="CETypeListGroup">
                            </asp:ImageButton>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>
<div id="popup-addnew-cetype" class="amc-popup">
    <table>
        <tr>
            <td colspan="2">
                <div class="amc-error-message">
                    <asp:Label runat="server" ID="lblCEWeightPopupMessage"></asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="lblProgramTypeHeader" resourcekey="ProgramTypeHeader.Text"></asp:Label>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlProgramType">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="lblCETypeHeader" resourcekey="CETypeHeader.Text"></asp:Label>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlCEType">
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator1" ControlToValidate="ddlCEType"
                    CssClass="fl" Display="Dynamic" ValidationGroup="CETypeSettingValidationGroup"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="Label4" resourcekey="MinCE.Text"></asp:Label>
            </td>
            <td>
                <asp:TextBox runat="server" CssClass="width250" ID="txtMinCE"></asp:TextBox>
                <div>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtMinCE"
                        ErrorMessage="Input value should be less than 99999.99" ValidationExpression="^\d{0,5}(\.\d{0,2})?$"
                        Display="Dynamic" ValidationGroup="CETypeSettingValidationGroup"></asp:RegularExpressionValidator>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="Label5" resourcekey="MaxCE.Text"></asp:Label>
            </td>
            <td>
                <asp:TextBox runat="server" CssClass="width250" ID="txtMaxCE"></asp:TextBox>
                <div>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtMaxCE"
                        ErrorMessage="Input value should be less than 99999.99" ValidationExpression="^\d{0,5}(\.\d{0,2})?$"
                        Display="Dynamic" ValidationGroup="CETypeSettingValidationGroup"></asp:RegularExpressionValidator>
                </div>
                <div>
                    <asp:CompareValidator runat="server" ID="compareValidator1" ControlToValidate="txtMaxCE"
                        ControlToCompare="txtMinCE" Operator="GreaterThanEqual" Type="Double" resourcekey="MinCELessThanMaxCE.Text"
                        ValidationGroup="CETypeSettingValidationGroup" Display="Dynamic"></asp:CompareValidator>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="lblWeightHeader" resourcekey="CEWeightHeader.Text"></asp:Label>
            </td>
            <td>
                <asp:TextBox runat="server" CssClass="width250 fl" ID="txtCEWeight"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator17" ControlToValidate="txtCEWeight"
                    CssClass="fl" Display="Dynamic" ValidationGroup="CETypeSettingValidationGroup"></asp:RequiredFieldValidator>
                <div>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCEWeight"
                        ErrorMessage="Input value should be less than 99999.99" ValidationExpression="^\d{0,5}(\.\d{0,2})?$"
                        Display="Dynamic" ValidationGroup="CETypeSettingValidationGroup"></asp:RegularExpressionValidator>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="text-center">
                <asp:Button ID="btnSaveCEWeight" runat="server" Text="OK" ValidationGroup="CETypeSettingValidationGroup" />
                <asp:Button ID="btnCancelSaveCEWeight" runat="server" Text="Cancel" OnClientClick="AmcCert.ShowPopUp('popup-addnew-cetype', false);return false;" />
            </td>
        </tr>
    </table>
</div>
