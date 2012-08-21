<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="FormConfigurationsUC.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.FormConfigurations.FormConfigurationsUC" %>
<%@ Register TagPrefix="dnn" TagName="texteditor" Src="~/controls/texteditor.ascx" %>
<script>
    var CurrentRichTextMode = '<%= PopUpRichTextEditorMode %>';
    var IsSuperUser = '<%= UserInfo.IsSuperUser %>';
</script>
<div id="inactive-status-configuration" class="amc-page">
    <asp:HiddenField runat="server" ID="hdCurrentSectionPopupOpenningId" />
    <asp:HiddenField runat="server" ID="hdCurrentSectionPopupOpenningTitle" />
    <asp:HiddenField runat="server" ID="hdCurrentObjectUniqueId" />
    <asp:HiddenField runat="server" ID="hdCurrentOpenFormId" />
    <asp:HiddenField runat="server" ID="hdCurrentOpenSectionId" />
    <asp:HiddenField runat="server" ID="hdConfigurationFormId" />
    <asp:HiddenField runat="server" ID="hdConfigurationSectionId" />
    <asp:HiddenField runat="server" ID="hdConfiguratonFieldId" />
    <asp:HiddenField runat="server" ID="hdOpeningPopUp" />
    <asp:HiddenField runat="server" ID="hdCurrentPopUpTitle" />
    <asp:HiddenField runat="server" ID="hdCurrentItemIsRichTextMode" />
    <div class="amc-title">
        <asp:Label runat="server" ID="lblTitle" resourcekey="FormConfigurationTitle.Text"></asp:Label>
    </div>
    <div class="amc-instruction bg-darkgrey">
        <asp:Label runat="server" ID="lblFormConfigurationTitle" resourcekey="FormConfiguration.Text"
            CssClass="fl position-relative line-height-25"></asp:Label>
        <asp:CheckBoxList runat="server" ID="chkEnabledAllList" CssClass="form-config-enabled-all"
            RepeatDirection="Horizontal" Width="210px">
            <asp:ListItem Value="EnableAll" Text="Enable All"></asp:ListItem>
            <asp:ListItem Value="DisableAll" Text="Disable All"></asp:ListItem>
        </asp:CheckBoxList>
        <asp:HyperLink runat="server" ID="hlGoToQuestionList" resourcekey="GoToQuestionList.Text"
            CssClass="fr dn"></asp:HyperLink>
        <div class="cb">
        </div>
    </div>
    <div class="amc-contents">
        <div>
            <asp:Repeater runat="server" ID="rptFormComfiguration">
                <HeaderTemplate>
                    <table id="tbl-form-configurations" cellpadding="0px" cellspacing="0px" border="0px">
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="form-item">
                        <td class="width20px">
                        </td>
                        <td>
                            <div>
                                <div class="hover-item" style="width: 100%;">
                                    <asp:HiddenField runat="server" ID="hdFormValue" Value='<%# Eval("FormValue") %>' />
                                    <asp:HiddenField runat="server" ID="hdFormId" Value='<%# Eval("FormId") %>' />
                                    <asp:HiddenField runat="server" ID="hdIsVisible" Value='<%# Eval("IsVisible") %>' />
                                    <asp:HiddenField runat="server" ID="hdIsFormRichText" Value='<%# Eval("IsRichText") %>' />
                                    <asp:HiddenField runat="server" ID="hdIsFormConfigurable" Value='<%# Eval("IsConfigurable") %>' />
                                    <div id="form-expand" class="expand-image fl">
                                    </div>
                                    <div class="configuration-item-value">
                                        <div class="configuration-item-text" runat="server" id="formItemText">
                                            <asp:Label runat="server" ID="lblFormValue" Text='<%# Eval("FormValue") %>'></asp:Label>
                                        </div>
                                        <div class="more-text">
                                            ...
                                        </div>
                                    </div>
                                    <table id="form-action" class="fr" style="width: auto;" border="0" cellpadding="0"
                                        cellspacing="0">
                                        <tr>
                                            <td>
                                                <div class="edit-image configuration-edit-image" id="imgEditForm" runat="server"
                                                    visible='<%# Eval("IsConfigurable") OrElse UserInfo.IsSuperUser %>'>
                                                </div>
                                            </td>
                                            <td>
                                                <div id="Div2" class="fr marginleft5" runat="server">
                                                    <asp:CheckBox runat="server" ID="chkFormIsEnabled" Checked='<%# Eval("IsVisible") %>'
                                                        Enabled='<%# Eval("IsConfigurable") OrElse UserInfo.IsSuperUser %>' resourcekey="IsEnable.Text" />
                                                </div>
                                            </td>
                                            <td>
                                                <div id="Div1" class="fr marginleft5" runat="server" visible='<%# UserInfo.IsSuperUser %>'>
                                                    <asp:CheckBox runat="server" ID="chkIsFormConfigurable" Checked='<%# Eval("IsConfigurable") %>'
                                                        Visible='<%# UserInfo.IsSuperUser %>' resourcekey="IsConfigurable.Text" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="cb">
                                    </div>
                                </div>
                            </div>
                            <div id="divSectionConfiguration" runat="server" class="dn" visible='<%# Eval("IsConfigurable") OrElse UserInfo.IsSuperUser %>'>
                                <%--Visible='<%# Eval("IsConfigurable") OrElse UserInfo.IsSuperUser %>'--%>
                                <asp:Repeater runat="server" ID="rptSectionConfiguration">
                                    <HeaderTemplate>
                                        <table id="tbl-section-configuration" cellpadding="0px" cellspacing="0px" border="0px">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class="section-item">
                                            <td class="width40px">
                                            </td>
                                            <td>
                                                <div>
                                                    <div class="hover-item" style="width: 100%;">
                                                        <asp:HiddenField runat="server" ID="hdSectionValue" Value='<%# Eval("SectionValue") %>' />
                                                        <asp:HiddenField runat="server" ID="hdSectionId" Value='<%# Eval("SectionId") %>' />
                                                        <asp:HiddenField runat="server" ID="hdSectionIsEnabled" Value='<%# Eval("IsEnabled") %>' />
                                                        <asp:HiddenField runat="server" ID="hdOriginalSectionSequence" Value='<%# Eval("Sequence") %>' />
                                                        <asp:HiddenField runat="server" ID="hdSectionCurrentSequence" Value='<%# Eval("Sequence") %>' />
                                                        <asp:HiddenField runat="server" ID="hdIsSectionRichText" Value='<%# Eval("IsRichText") %>' />
                                                        <asp:HiddenField runat="server" ID="hdIsSectionConfigurable" Value='<%# Eval("IsConfigurable") %>' />
                                                        <div id="section-expand" class="expand-image fl">
                                                        </div>
                                                        <div class="configuration-item-value">
                                                            <div class="configuration-item-text" runat="server" id="sectionItemText">
                                                                <asp:Label runat="server" ID="lblSectionValue" Text='<%# Eval("SectionValue") %>'></asp:Label>
                                                            </div>
                                                            <div class="more-text">
                                                                ...
                                                            </div>
                                                        </div>
                                                        <table id="section-action" class="fr" style="width: auto;" border="0" cellpadding="0"
                                                            cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <div class="edit-image configuration-edit-image" id="imgEditSection" runat="server"
                                                                        visible='<%# Eval("IsConfigurable") OrElse UserInfo.IsSuperUser %>'>
                                                                    </div>
                                                                </td>
                                                                <td>
                                                                    <div id="Div4" style="width: 30px;" class="resequence fr dn opacity40" runat="server"
                                                                        visible='<%# Eval("IsConfigurable") OrElse UserInfo.IsSuperUser %>'>
                                                                        <div class="down-image" id="section-movedown">
                                                                        </div>
                                                                        <div class="up-image" id="section-moveup">
                                                                        </div>
                                                                        <div class="cb">
                                                                        </div>
                                                                    </div>
                                                                </td>
                                                                <td>
                                                                    <div class="fr marginleft5">
                                                                        <asp:CheckBox runat="server" ID="chkSectionIsEnabled" Checked='<%# Eval("IsEnabled") %>'
                                                                            Enabled='<%# Eval("IsConfigurable") OrElse UserInfo.IsSuperUser %>' resourcekey="IsEnable.Text" />
                                                                    </div>
                                                                </td>
                                                                <td>
                                                                    <div id="Div3" class="fr marginleft5" runat="server" visible='<%# UserInfo.IsSuperUser %>'>
                                                                        <asp:CheckBox runat="server" ID="chkIsSectionConfigurable" Checked='<%# Eval("IsConfigurable") %>'
                                                                            Visible='<%# UserInfo.IsSuperUser %>' resourcekey="IsConfigurable.Text" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <div class="cb">
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="divFieldConfiguration" runat="server" class="dn" visible='<%# Eval("IsConfigurable") OrElse UserInfo.IsSuperUser %>'>
                                                    <%--Visible='<%# Eval("IsConfigurable") OrElse UserInfo.IsSuperUser %>'--%>
                                                    <asp:Repeater runat="server" ID="rptFieldConfiguration">
                                                        <HeaderTemplate>
                                                            <table id="tbl-field-configurations" cellpadding="0px" cellspacing="0px" border="0px">
                                                                <tr class="tr-header">
                                                                    <td class="width80px">
                                                                    </td>
                                                                    <td>
                                                                        <div class="fl width200">
                                                                            <asp:Label runat="server" ID="lblFieldName" resourcekey="FieldName.Text"></asp:Label>
                                                                        </div>
                                                                        <div id="Div1" class="fr width120 text-center" runat="server" visible='<%# UserInfo.IsSuperUser AndAlso Not Eval("IsInstruction") %>'>
                                                                            <asp:Label runat="server" ID="Label2" resourcekey="IsConfigurable.Text"></asp:Label>
                                                                        </div>
                                                                        <div class="fr width100 text-center">
                                                                            <asp:Label runat="server" ID="lblIsRequired" resourcekey="IsRequired.Text"></asp:Label>
                                                                        </div>
                                                                        <div class="fr width100 text-center">
                                                                            <asp:Label runat="server" ID="lblIsEnable" resourcekey="IsEnable.Text"></asp:Label>
                                                                        </div>
                                                                        <div class="cb">
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr class="field-item">
                                                                <td class="width80px">
                                                                </td>
                                                                <td id="td-field-hover">
                                                                    <div class="hover-item" style="width: 100%;" id="divFieldHover" runat="server">
                                                                        <asp:HiddenField runat="server" ID="hdFieldValueItem" Value='<%# Eval("FieldValue") %>' />
                                                                        <asp:HiddenField runat="server" ID="hdFieldIsEnabled" Value='<%# Eval("IsEnabled") %>' />
                                                                        <asp:HiddenField runat="server" ID="hdFieldIsRequired" Value='<%# Eval("IsRequired") %>' />
                                                                        <asp:HiddenField runat="server" ID="hdFieldId" Value='<%# Eval("FieldId") %>' />
                                                                        <asp:HiddenField runat="server" ID="hdIsFieldRichText" Value='<%# Eval("IsRichText") %>' />
                                                                        <asp:HiddenField runat="server" ID="hdValidateControls" />
                                                                        <asp:HiddenField runat="server" ID="hdIsFieldConfigurable" Value='<%# Eval("IsConfigurable") %>' />
                                                                        <asp:HiddenField runat="server" ID="hdIsInstruction" Value='<%# Eval("IsInstruction") %>' />
                                                                        <asp:HiddenField runat="server" ID="hdIsCalculate" Value='<%# Eval("IsCalculate") %>' />
                                                                        <asp:HiddenField runat="server" ID="hdCalculateFormular" Value='<%# Eval("CalculateFormula") %>' />
                                                                        <div class="configuration-item-value">
                                                                            <div class="configuration-item-text" runat="server" id="fieldItemText">
                                                                                <asp:Label runat="server" ID="lblFieldNameItem" Text='<%# Eval("FieldValue") %>'
                                                                                    Visible='<%# Not Eval("IsQuestion") %>'></asp:Label>
                                                                                <asp:HyperLink runat="server" ID="hlFieldNameItem" Text='<%# Eval("FieldValue") %>'
                                                                                    Visible='<%# Eval("IsQuestion") %>'></asp:HyperLink>
                                                                            </div>
                                                                            <div class="more-text">
                                                                                ...
                                                                            </div>
                                                                        </div>
                                                                        <table class="fr" style="width: auto;" border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <div class="edit-image configuration-edit-image" id="imgEditField" runat="server"
                                                                                        visible='<%# Eval("IsConfigurable") OrElse UserInfo.IsSuperUser %>'>
                                                                                    </div>
                                                                                </td>
                                                                                <td>
                                                                                    <div style="height: 20px;" class="width100 text-center valign-top" id="tdFieldEnabledCheckbox"
                                                                                        runat="server" visible='<%# Not Eval("IsInstruction") %>'>
                                                                                        <asp:CheckBox runat="server" ID="chkFieldIsEnable" Checked='<%# Eval("IsEnabled") %>'
                                                                                            Enabled='<%# Eval("IsConfigurable") OrElse UserInfo.IsSuperUser %>' Visible='<%# Not Eval("IsInstruction") %>' />
                                                                                    </div>
                                                                                </td>
                                                                                <td>
                                                                                    <div style="height: 20px;" class="width100 text-center valign-top" id="tdIsFieldRequiredCheckbox"
                                                                                        runat="server" visible='<%# Not Eval("IsInstruction") %>'>
                                                                                        <asp:CheckBox runat="server" ID="chkFieldIsRequired" Checked='<%# Eval("IsRequired") %>'
                                                                                            Enabled='<%# Eval("IsConfigurable") OrElse UserInfo.IsSuperUser %>' Visible='<%# Not Eval("IsInstruction") AndAlso Not Eval("IsQuestion") AndAlso Not Eval("IsReadOnly")%>' />
                                                                                    </div>
                                                                                </td>
                                                                                <td>
                                                                                    <div style="height: 20px;" class="width120 text-center valign-top" id="tdIsFieldConfigurableCheckBox"
                                                                                        runat="server" visible='<%# UserInfo.IsSuperUser AndAlso Not Eval("IsInstruction")%>'>
                                                                                        <asp:CheckBox runat="server" ID="chkIsFieldConfigurable" Checked='<%# Eval("IsConfigurable") %>'
                                                                                            Enabled='<%# UserInfo.IsSuperUser %>' Visible='<%# Not Eval("IsInstruction") %>' />
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <div class="cb">
                                                                        </div>
                                                                        <div id="fieldcalculator" runat="server" visible='<%# Eval("IsCalculate") %>' class="dn padleft20 padbot5">
                                                                            <asp:Label runat="server" ID="lblFormularText" Text="CE Hours Multiplying Rate: "></asp:Label><asp:TextBox
                                                                                runat="server" ID="txtCalculateFormular" Text='<%# Eval("CalculateFormula") %>'></asp:TextBox>
                                                                            <br />
                                                                            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtCalculateFormular"
                                                                                ErrorMessage="Input value should be between 1 and 100." MaximumValue="100" MinimumValue="1"
                                                                                Display="Dynamic" Type="Double"></asp:RangeValidator>
                                                                        </div>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </table>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
    <div class="amc-instruction bg-darkgrey">
        <asp:Label runat="server" ID="lblOtherSettingsHeader" resourcekey="OtherSettings.Text"></asp:Label>
    </div>
    <div class="amc-contents">
        <div class="padleft20">
            <table style="padding-top: 5px;">
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="lblCETypeSettings" resourcekey="CETypeSettings.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:HyperLink runat="server" ID="hlCETypeSettings" resourcekey="GoToCETypeSettings.Text"></asp:HyperLink>
                    </td>
                </tr>
            </table>
            <table style="padding-top: 5px;">
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="Label8" resourcekey="ValidationRuleSettings.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:HyperLink runat="server" ID="hlValidationRuleSettings" resourcekey="GoToValidationRuleSettings.Text"></asp:HyperLink>
                    </td>
                </tr>
            </table>
            <table style="padding-top: 5px;">
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="Label1" resourcekey="OtherSettings.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:HyperLink runat="server" ID="hlOtherSettings" resourcekey="GoToOtherSettings.Text"></asp:HyperLink>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="configuration-footer text-center">
        <asp:Button runat="server" ID="btnSave" resourcekey="Save.Text" />
        <asp:Button runat="server" ID="btnReset" resourcekey="Reset.Text" CausesValidation="False" />
        <asp:Button runat="server" ID="btnBuildQuestion" Text="Build Questions" Visible="False"
            CausesValidation="False" />
        <asp:Button ID="btnClose" runat="server" Text="Close" CausesValidation="False" />
    </div>
    <div id="form-configuration-popup" class="amc-popup">
        <table>
            <tr>
                <td>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator18" ControlToValidate="txtNewValue"
                        CssClass="fl" resourcekey="InputValueRequired.Text" Display="Dynamic" ValidationGroup="NewFormSectionTextPopUp"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="configure-richtext" class="dn">
                <td>
                    <dnn:texteditor id="teMessage" runat="server" width="550" textrendermode="Raw" htmlencode="False"
                        defaultmode="Rich" height="350" choosemode="True" chooserender="False" />
                </td>
            </tr>
            <tr id="configure-basictext">
                <td>
                    <asp:TextBox runat="server" ID="txtNewValue" TextMode="MultiLine" Width="300px" Height="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button runat="server" ID="btnOK" resourcekey="OK.Text" ValidationGroup="NewFormSectionTextPopUp" />
                    <asp:Button runat="server" ID="btnCancel" resourcekey="Cancel.Text" />
                </td>
            </tr>
        </table>
    </div>
</div>
