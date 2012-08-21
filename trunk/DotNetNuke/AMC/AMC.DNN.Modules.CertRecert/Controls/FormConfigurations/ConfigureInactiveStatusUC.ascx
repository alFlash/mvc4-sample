<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ConfigureInactiveStatusUC.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.FormConfigurations.ConfigureInactiveStatusUC"
    EnableViewState="true" %>
<%@ Register TagPrefix="amc" TagName="DateTime" Src="../Reusable/AMCDatetime.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/texteditor.ascx" %>
<style type="text/css">
    .style1
    {
        width: 100%;
    }
</style>
<style type="text/css">
    .AuditGridHeader
    {
        background-color: gray;
        height: 25px;
        text-align: center;
    }
</style>
<script>
    var CurrentRichTextMode = '<%= PopUpRichTextEditorMode %>';
    var IsSuperUser = '<%= UserInfo.IsSuperUser %>';
    var IsShowReviewProcessPopup = '<%= IsShowReviewProcessPopup %>';
    jQuery(document).ready(function () {
        if (typeof runCertAudit != 'undefined' && runCertAudit.length == 1) {
            AmcCert.ShowPopUp('select-application-for-audit', true);
            AmcCert.SetTitle('select-application-for-audit', 'Applications Are Subjected to Audit');
        } else if (jQuery("#inactive-status-configuration #[id*=hdCurrentSectionPopupOpenningId]").val() != ''
            && jQuery("#inactive-status-configuration #[id*=hdCurrentSectionPopupOpenningId]").val() != null) {
            AmcCert.ShowPopUp(jQuery("#inactive-status-configuration #[id*=hdCurrentSectionPopupOpenningId]").val(), true);
            AmcCert.SetTitle(jQuery("#inactive-status-configuration #[id*=hdCurrentSectionPopupOpenningId]").val(), jQuery("#inactive-status-configuration #[id*=hdCurrentSectionPopupOpenningTitle]").val());
        }

        if (IsShowReviewProcessPopup == 'True') {
            AmcCert.ShowPopUp('select-review-process', true);
            AmcCert.SetTitle('select-review-process', 'Review Board');
        }

    });
    function OpenPrintPage(url) {
        var newwindow = window.open(url, 'Certification', 'toolbar=yes,scrollbars=yes,menubar=yes,location=no,resizable=1');
        if (window.focus) { newwindow.focus(); }
        return false;
    }

    var fuImportConfigurationsClientID = '<%= fuImport.ClientID %>';
    //    var txtImportConfigurationsClientID = '< % = txtImport.ClientID %>';
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
                                                                        <div class="fr width120 text-center" runat="server" visible='<%# UserInfo.IsSuperUser AndAlso Not Eval("IsInstruction") %>'>
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
                                                                                resourcekey="PointConfig.Text" MaximumValue="100" MinimumValue="1"
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
    <br />
    <div class="amc-instruction bg-darkgrey">
        <asp:Label runat="server" ID="lblNotificationSettings" resourcekey="NotificationSettings.Text"></asp:Label>
    </div>
    <div class="amc-contents">
        <div id="notification-settings">
            <div class="padleft20">
                <asp:CheckBoxList runat="server" ID="cblNotificationSettings">
                </asp:CheckBoxList>
            </div>
        </div>
    </div>
    <br />
    <div class="amc-instruction bg-darkgrey">
        <asp:Label runat="server" ID="lblCertAuditSettingsTitle" resourcekey="CertAuditSettingsTitle.Text"></asp:Label>
    </div>
    <div class="amc-contents">
        <div id="certAudit-settings">
            <div class="padleft20">
                <table>
                    <tr>
                        <td width="350px">
                            <asp:Label runat="server" ID="lblCertAuditSettingsStartDate" resourcekey="AuditSettingsStartDate.Text"></asp:Label>
                        </td>
                        <td>
                            <amc:DateTime runat="server" ID="txtCertAuditStartDate" ValidationGroup="CertAuditGroup"
                                CssClass="fl"></amc:DateTime>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqCertAuditStartDate"
                                ControlToValidate="txtCertAuditStartDate$txtDatetime" CssClass="fl" Display="Dynamic"
                                ValidationGroup="CertAuditGroup"></asp:RequiredFieldValidator>
                            <%--<div>
                                <asp:CompareValidator ID="dateValidator" runat="server" Type="Date" Operator="DataTypeCheck" ValidationGroup="CertAuditGroup"
                                    Display="Dynamic" ControlToValidate="txtCertAuditStartDate$txtDatetime" ErrorMessage="Please enter a valid date.">
                                </asp:CompareValidator>
                            </div>--%>
                        </td>
                    </tr>
                    <tr>
                        <td width="350px">
                            <asp:Label runat="server" ID="lblCertLastAuditRun" resourcekey="CertLastAuditRun.Text"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width250" ID="txtCertLastAuditRun" Enabled="False"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator1"
                                ControlToValidate="txtCertLastAuditRun" Display="Dynamic" ValidationGroup="CertAuditGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td width="350px">
                            <asp:Label runat="server" ID="lblCertAuditSettingsEndDate" resourcekey="AuditSettingsEndDate.Text"></asp:Label>
                        </td>
                        <td>
                            <amc:DateTime runat="server" ID="txtCertAuditEndDate" ValidationGroup="CertAuditGroup"
                                CssClass="fl"></amc:DateTime>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator2"
                                ControlToValidate="txtCertAuditEndDate$txtDatetime" CssClass="fl" Display="Dynamic"
                                ValidationGroup="CertAuditGroup"></asp:RequiredFieldValidator>
                            <%--<div>
                                <asp:CompareValidator ID="rqCertAuditEndDate" runat="server" Type="Date" Operator="DataTypeCheck" ValidationGroup="CertAuditGroup"
                                    Display="Dynamic" ControlToValidate="txtCertAuditEndDate$txtDatetime" ErrorMessage="Please enter a valid date.">
                                </asp:CompareValidator>
                            </div>--%>
                            <div>
                                <asp:CompareValidator ID="Compare1" ControlToValidate="txtCertAuditEndDate$txtDatetime"
                                    ControlToCompare="txtCertAuditStartDate$txtDatetime" resourcekey="StartDateLessThanEndDate.Text"
                                    Type="Date" runat="server" Display="Dynamic" Operator="GreaterThanEqual" ValidationGroup="CertAuditGroup" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td width="350px">
                            <asp:Label runat="server" ID="lblCertSelectionRatio" resourcekey="AuditSelectionRatio.Text"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width250" ID="txtCertSelectionRatio"></asp:TextBox>
                            %
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator3"
                                ControlToValidate="txtCertSelectionRatio" Display="Dynamic" ValidationGroup="CertAuditGroup"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtCertSelectionRatio"
                                resourcekey="Number0to100.Text" MaximumValue="100" MinimumValue="0"
                                Display="Dynamic" Type="Double" ValidationGroup="CertAuditGroup"></asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td width="350px">
                            <asp:Label runat="server" ID="lblCertEnableNotifyApplicant" resourcekey="AuditSettingsDonotNotifyApplicant.Text"></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkCertEnableNotifyApplicant"></asp:CheckBox>
                        </td>
                    </tr>
                </table>
                <br />
                <table>
                    <tr>
                        <td width="350px">
                            <asp:LinkButton runat="server" ID="btnCertSaveSettings" resourcekey="AuditSettingsSaveSettings.Text"
                                ValidationGroup="CertAuditGroup"></asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton runat="server" ID="btnCertRun" resourcekey="AuditSettingsRunSettings.Text"
                                ValidationGroup="CertAuditGroup"></asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton runat="server" ID="btnCertRunLastAudits" resourcekey="RunSinceLastAudit.Text"
                                ValidationGroup="CertAuditGroup"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <br />
    <div class="amc-instruction bg-darkgrey">
        <asp:Label runat="server" ID="lblReCertAuditSettingsTitle" resourcekey="ReCertAuditSettingsTitle.Text"></asp:Label>
    </div>
    <div class="amc-contents">
        <div id="RecertAudit-settings">
            <div class="padleft20">
                <table>
                    <tr>
                        <td width="350px">
                            <asp:Label runat="server" ID="lblReCertAuditSettingsStartDate" resourcekey="AuditSettingsStartDate.Text"></asp:Label>
                        </td>
                        <td>
                            <amc:DateTime runat="server" ID="txtReCertAuditStartDate" ValidationGroup="ReCertAuditGroup"
                                CssClass="fl"></amc:DateTime>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator4"
                                ControlToValidate="txtReCertAuditStartDate$txtDatetime" CssClass="fl" Display="Dynamic"
                                ValidationGroup="ReCertAuditGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td width="350px">
                            <asp:Label runat="server" ID="Label1" resourcekey="ReCertLastAuditRun.Text"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width250" ID="txtReCertLastAuditRun" Enabled="False"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator5"
                                ControlToValidate="txtReCertLastAuditRun" Display="Dynamic" ValidationGroup="ReCertAuditGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td width="350px">
                            <asp:Label runat="server" ID="lblReCertAuditSettingsEndDate" resourcekey="AuditSettingsEndDate.Text"></asp:Label>
                        </td>
                        <td>
                            <amc:DateTime runat="server" ID="txtReCertAuditEndDate" ValidationGroup="ReCertAuditGroup"
                                CssClass="fl"></amc:DateTime>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator6"
                                ControlToValidate="txtReCertAuditEndDate$txtDatetime" CssClass="fl" Display="Dynamic"
                                ValidationGroup="ReCertAuditGroup"></asp:RequiredFieldValidator>
                            <div>
                                <asp:CompareValidator ID="CompareValidator2" ControlToValidate="txtReCertAuditEndDate$txtDatetime"
                                    ValidationGroup="ReCertAuditGroup" ControlToCompare="txtReCertAuditStartDate$txtDatetime"
                                    resourcekey="StartDateLessThanEndDate.Text" Type="Date" runat="server"
                                    Display="Dynamic" Operator="GreaterThanEqual" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td width="350px">
                            <asp:Label runat="server" ID="lblReCertSelectionRatio" resourcekey="AuditSelectionRatio.Text"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width250" ID="txtReCertSelectionRatio"></asp:TextBox>%
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator7"
                                ControlToValidate="txtReCertSelectionRatio" Display="Dynamic" ValidationGroup="ReCertAuditGroup"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtReCertSelectionRatio"
                                resourcekey="Number0to100.Text" MaximumValue="100" MinimumValue="0"
                                Display="Dynamic" Type="Double" ValidationGroup="ReCertAuditGroup"></asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td width="350px">
                            <asp:Label runat="server" ID="lblReCertEnableNotifyApplicant" resourcekey="AuditSettingsDonotNotifyApplicant.Text"></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkReCertEnableNotifyApplicant"></asp:CheckBox>
                        </td>
                    </tr>
                </table>
                <br />
                <table>
                    <tr>
                        <td width="350px">
                            <asp:LinkButton runat="server" ID="btnReCertSaveSettings" resourcekey="AuditSettingsSaveSettings.Text"
                                ValidationGroup="ReCertAuditGroup"></asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton runat="server" ID="btnReCertRun" resourcekey="AuditSettingsRunSettings.Text"
                                ValidationGroup="ReCertAuditGroup"></asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton runat="server" ID="btnReCertRunLastAudit" resourcekey="RunSinceLastAudit.Text"
                                ValidationGroup="ReCertAuditGroup"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <br />
    <!--- Review Board Region -->
    <div class="amc-instruction bg-darkgrey">
        <asp:Label runat="server" ID="lblReviewBoard" Text="Review Board" resourcekey="ReviewBoard.Text"></asp:Label>
    </div>
    <div class="amc-contents">
        <div id="Div5">
            <div class="padleft20">
                <table>
                    <tr>
                        <td width="350px">
                            <asp:Label runat="server" ID="lblStartDate" resourcekey="AuditSettingsStartDate.Text"></asp:Label>
                        </td>
                        <td>
                            <amc:DateTime runat="server" ID="txtReviewStartDate" ValidationGroup="ReviewValidationGroup"
                                CssClass="fl"></amc:DateTime>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqAuditSettingsStartDate"
                                ControlToValidate="txtReviewStartDate$txtDatetime" CssClass="fl" Display="Dynamic"
                                ValidationGroup="ReviewValidationGroup">
                            </asp:RequiredFieldValidator>
                            <%--<div>
                                <asp:CompareValidator ID="dateValidator" runat="server" Type="Date" Operator="DataTypeCheck" ValidationGroup="CertAuditGroup"
                                    Display="Dynamic" ControlToValidate="txtCertAuditStartDate$txtDatetime" ErrorMessage="Please enter a valid date.">
                                </asp:CompareValidator>
                            </div>--%>
                        </td>
                    </tr>
                    <tr>
                        <td width="350px">
                            <asp:Label runat="server" ID="lblReviewEndDate" Text="End Date" resourcekey="ReviewEndDate.Text"></asp:Label>
                        </td>
                        <td>
                            <amc:DateTime runat="server" ID="txtReviewEndDate" ValidationGroup="ReviewValidationGroup"
                                CssClass="fl"></amc:DateTime>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator30"
                                ControlToValidate="txtReviewEndDate$txtDatetime" CssClass="fl" Display="Dynamic"
                                ValidationGroup="ReviewValidationGroup"></asp:RequiredFieldValidator>
                            <div>
                                <asp:CompareValidator ID="CompareValidator1" ControlToValidate="txtReviewEndDate$txtDatetime"
                                    ControlToCompare="txtReviewStartDate$txtDatetime" resourcekey="StartDateLessThanEndDate.Text"
                                    Type="Date" runat="server" Display="Dynamic" Operator="GreaterThanEqual" ValidationGroup="ReviewValidationGroup" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblProcessType" Text="Process" resourcekey="ReviewProcessType.Text"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlProcessType">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:LinkButton runat="server" ID="lbtnRunReview" Text="Run" resourcekey="RunReview.Text"
                                ValidationGroup="ReviewValidationGroup"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <br />
    <div class="amc-instruction bg-darkgrey">
        <asp:Label runat="server" ID="lblCertificaionCodeSettingsHeader" resourcekey="CertificationCodeSettings.Text"></asp:Label>
    </div>
    <div class="amc-contents">
        <div class="padleft20">
            <table>
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="lblCertificationCodeSettings" resourcekey="CertificationCode.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtCertificationCode"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator8"
                            ControlToValidate="txtCertificationCode" EnableClientScript="True" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="lblRecertificationCodeSettings" resourcekey="RecertificationCode.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtRecertificationCode"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator9"
                            ControlToValidate="txtRecertificationCode" EnableClientScript="True" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="Label7" resourcekey="InactiveStatusProductCode.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtInactiveStatusProductCode"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator10"
                            ControlToValidate="txtInactiveStatusProductCode" EnableClientScript="True" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
            <table style="padding-top: 5px">
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="lblMembershipLink" resourcekey="MembershipLink.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtMembershipLink"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator11"
                            ControlToValidate="txtMembershipLink" EnableClientScript="True" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
            <table style="padding-top: 5px">
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="lblLicensureValidityDate" resourcekey="LicensureValidityDate.Text"></asp:Label>
                    </td>
                    <td>
                        <amc:DateTime runat="server" ID="txtLicensureValidityDate" CssClass="fl"></amc:DateTime>
                        <%--                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator12" ControlToValidate="txtLicensureValidityDate$txtDatetime"
                            EnableClientScript="True" CssClass="fl" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
            </table>
            <!-- Shopping cart -->
            <table style="padding-top: 5px">
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="Label3" resourcekey="ProductId.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtRecertProductId"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator13"
                            ControlToValidate="txtRecertProductId" EnableClientScript="True" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
            <table style="padding-top: 5px">
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="Label6" resourcekey="InactiveStatusProductId.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtInactiveStatusProductId"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator16"
                            ControlToValidate="txtInactiveStatusProductId" EnableClientScript="True" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
            <table style="padding-top: 5px">
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="Label4" resourcekey="PersonifyShoppingCartWebService.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtPersonifyShoppingCartWebService"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator14"
                            ControlToValidate="txtPersonifyShoppingCartWebService" EnableClientScript="True"
                            Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
            <table style="padding-top: 5px">
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="Label5" resourcekey="CheckoutURL.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtCheckoutURL"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator15"
                            ControlToValidate="txtCheckoutURL" EnableClientScript="True" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
            <table style="padding-top: 5px">
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="lblReCertCMEQuestion1" resourcekey="ReCertCMEQuestion1.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtReCertCMEQuestion1"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator17"
                            ControlToValidate="txtReCertCMEQuestion1" EnableClientScript="True" Display="Dynamic"></asp:RequiredFieldValidator>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtReCertCMEQuestion1"
                                resourcekey="Inputthan999999999999.Text" ValidationExpression="^\d{0,12}$"
                                EnableClientScript="True" Display="Dynamic"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
            </table>
            <table style="padding-top: 5px">
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="Label9" resourcekey="ReCertCMEQuestion2.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtReCertCMEQuestion2"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator19"
                            ControlToValidate="txtReCertCMEQuestion2" EnableClientScript="True" Display="Dynamic"></asp:RequiredFieldValidator>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtReCertCMEQuestion2"
                                resourcekey="Inputthan999999999999.Text" ValidationExpression="^\d{0,12}$"
                                EnableClientScript="True" Display="Dynamic"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
            </table>
            <table style="padding-top: 5px">
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="Label10" resourcekey="ReCertCMEQuestion3.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtReCertCMEQuestion3"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator20"
                            ControlToValidate="txtReCertCMEQuestion3" EnableClientScript="True" Display="Dynamic"></asp:RequiredFieldValidator>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtReCertCMEQuestion3"
                                resourcekey="Inputthan999999999999.Text" ValidationExpression="^\d{0,12}$"
                                EnableClientScript="True" Display="Dynamic"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
            </table>
            <table style="padding-top: 5px">
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="Label11" resourcekey="ReCertCMEQuestion4.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtReCertCMEQuestion4"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator21"
                            ControlToValidate="txtReCertCMEQuestion4" EnableClientScript="True" Display="Dynamic"></asp:RequiredFieldValidator>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtReCertCMEQuestion4"
                               resourcekey="Inputthan999999999999.Text" ValidationExpression="^\d{0,12}$"
                                EnableClientScript="True" Display="Dynamic"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
            </table>
            <table style="padding-top: 5px">
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="Label13" resourcekey="CMEHoursEarned.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtCMEHoursEarned"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator23"
                            ControlToValidate="txtCMEHoursEarned" EnableClientScript="True" Display="Dynamic"></asp:RequiredFieldValidator>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtCMEHoursEarned"
                               resourcekey="Inputthan999999999999.Text" ValidationExpression="^\d{0,12}$"
                                EnableClientScript="True" Display="Dynamic"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
            </table>
            <table style="padding-top: 5px">
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="Label14" resourcekey="CMEStartYear.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtCMEStartYear"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator24"
                            ControlToValidate="txtCMEStartYear" EnableClientScript="True" Display="Dynamic"></asp:RequiredFieldValidator>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txtCMEStartYear"
                                resourcekey="Inputthan999999999999.Text" ValidationExpression="^\d{0,12}$"
                                EnableClientScript="True" Display="Dynamic"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
            </table>
            <table style="padding-top: 5px;">
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="Label15" resourcekey="CMEEndYear.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtCMEEndYear"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator25"
                            ControlToValidate="txtCMEEndYear" EnableClientScript="True" Display="Dynamic"></asp:RequiredFieldValidator>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txtCMEEndYear"
                                resourcekey="Inputthan999999999999.Text" ValidationExpression="^\d{0,12}$"
                                EnableClientScript="True" Display="Dynamic"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
            </table>
            <table style="padding-top: 5px">
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="Label12" resourcekey="ProPracticeQuestionaireValidateMonth.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtProPracticeQuestionaireValidateMonth"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator22"
                            ControlToValidate="txtProPracticeQuestionaireValidateMonth" EnableClientScript="True"
                            Display="Dynamic"></asp:RequiredFieldValidator>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtProPracticeQuestionaireValidateMonth"
                                resourcekey="Inputthan999999999999.Text" ValidationExpression="^\d{0,12}$"
                                EnableClientScript="True" Display="Dynamic"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
            </table>
            <table style="padding-top: 5px;">
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="Label17" resourcekey="ProfessionalPracticeQuestionaireStartYear.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtProfessionalPracticeQuestionaireStartYear"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator27"
                            ControlToValidate="txtProfessionalPracticeQuestionaireStartYear" EnableClientScript="True"
                            Display="Dynamic"></asp:RequiredFieldValidator>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server"
                                ControlToValidate="txtProfessionalPracticeQuestionaireStartYear" resourcekey="Inputthan999999999999.Text"
                                ValidationExpression="^\d{0,12}$" EnableClientScript="True" Display="Dynamic"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
            </table>
            <table style="padding-top: 5px;">
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="Label18" resourcekey="ProfessionalPracticeQuestionaireEndYear.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtProfessionalPracticeQuestionaireEndYear"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator28"
                            ControlToValidate="txtProfessionalPracticeQuestionaireEndYear" EnableClientScript="True"
                            Display="Dynamic"></asp:RequiredFieldValidator>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server"
                                ControlToValidate="txtProfessionalPracticeQuestionaireEndYear" resourcekey="Inputthan999999999999.Text"
                                ValidationExpression="^\d{0,12}$" EnableClientScript="True" Display="Dynamic"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
            </table>
            <table style="padding-top: 5px;">
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="Label26" resourcekey="ARNMaxSummaryPointOfContinuingEducation.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtARNMaxSummaryPointOfContinuingEducation"></asp:TextBox>
                         <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator26"
                            ControlToValidate="txtARNMaxSummaryPointOfContinuingEducation" EnableClientScript="True"
                            Display="Dynamic"></asp:RequiredFieldValidator>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator18" runat="server"
                                ControlToValidate="txtARNMaxSummaryPointOfContinuingEducation" resourcekey="InputThan99999.Text"
                                ValidationExpression="^\d{0,5}(\.\d{0,2})?$" EnableClientScript="True" Display="Dynamic"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
            </table>
            <table style="padding-top: 5px;">
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="Label19" resourcekey="PresentationTotalPoint.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtPresentationTotalPoint"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator12"
                            ControlToValidate="txtPresentationTotalPoint" EnableClientScript="True" Display="Dynamic"></asp:RequiredFieldValidator>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server"
                                ControlToValidate="txtPresentationTotalPoint" resourcekey="InputThan99999.Text"
                                ValidationExpression="^\d{0,5}(\.\d{0,2})?$" EnableClientScript="True" Display="Dynamic"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
            </table>
            <table style="padding-top: 5px;">
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="Label20" resourcekey="EducationCourseTotalPoint.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtEducationCourseTotalPoint"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator29"
                            ControlToValidate="txtEducationCourseTotalPoint" EnableClientScript="True" Display="Dynamic"></asp:RequiredFieldValidator>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server"
                                ControlToValidate="txtEducationCourseTotalPoint" resourcekey="InputThan99999.Text"
                                ValidationExpression="^\d{0,5}(\.\d{0,2})?$" EnableClientScript="True" Display="Dynamic"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
            </table>
            <table style="padding-top: 5px;">
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="Label21" resourcekey="PublicationTotalPoint.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtPublicationTotalPoint"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator31"
                            ControlToValidate="txtPublicationTotalPoint" EnableClientScript="True" Display="Dynamic"></asp:RequiredFieldValidator>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator14" runat="server"
                                ControlToValidate="txtPublicationTotalPoint" resourcekey="InputThan99999.Text"
                                ValidationExpression="^\d{0,5}(\.\d{0,2})?$" EnableClientScript="True" Display="Dynamic"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
            </table>
            <table style="padding-top: 5px;">
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="Label22" resourcekey="CommunityServiceTotalPoint.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtCommunityServiceTotalPoint"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator32"
                            ControlToValidate="txtCommunityServiceTotalPoint" EnableClientScript="True" Display="Dynamic"></asp:RequiredFieldValidator>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator15" runat="server"
                                ControlToValidate="txtCommunityServiceTotalPoint" resourcekey="InputThan99999.Text"
                                ValidationExpression="^\d{0,5}(\.\d{0,2})?$" EnableClientScript="True" Display="Dynamic"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
            </table>
            
            <table style="padding-top: 5px;">
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="Label25" resourcekey="ARNMaxSummaryPoints.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtARNMaxSummaryPoints"></asp:TextBox>
                          <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="RequiredFieldValidator33"
                            ControlToValidate="txtARNMaxSummaryPoints" EnableClientScript="True" Display="Dynamic"></asp:RequiredFieldValidator>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator17" runat="server"
                                ControlToValidate="txtARNMaxSummaryPoints" resourcekey="InputThan9999.Text"
                                ValidationExpression="^\d{0,5}?$" EnableClientScript="True" Display="Dynamic"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
            </table>
            <table style="padding-top: 5px;">
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="Label23" resourcekey="RecertificationPaymentMonths.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtRecertificationPaymentMonths"></asp:TextBox>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator16" runat="server"
                                ControlToValidate="txtRecertificationPaymentMonths" resourcekey="InputThan9999.Text"
                                ValidationExpression="^\d{0,5}?$" EnableClientScript="True" Display="Dynamic"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
            </table>
            <table style="padding-top: 5px;">
                <tr>
                    <td style="width: 350px;">
                        <asp:Label runat="server" ID="Label16" resourcekey="ReCertificationCircle.Text"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" CssClass="width250" ID="txtReCertificationCircle"></asp:TextBox>
                        <div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ControlToValidate="txtReCertificationCircle"
                                resourcekey="InputThan9999.Text" ValidationExpression="^\d{0,5}$"
                                EnableClientScript="True" Display="Dynamic"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
            </table>
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
        </div>
    </div>
    <br />
    <div class="amc-instruction bg-darkgrey">
        <asp:Label runat="server" ID="Label24" resourcekey="ImportExport.Text"></asp:Label>
    </div>
    <div class="amc-contents">
        <div class="padleft20" style="padding-bottom: 10px;">
            <ul>
                <li>
                    <asp:Label runat="server" ID="lblImportExportInstruction1" resourcekey="ImportExportInstruction1.Text"></asp:Label></li>
                <li>
                    <asp:Label runat="server" ID="lblImportExportInstruction2" resourcekey="ImportExportInstruction2.Text"></asp:Label></li>
                <li>
                    <asp:Label runat="server" ID="lblImportExportInstruction3" resourcekey="ImportExportInstruction3.Text"></asp:Label></li>
            </ul>
        </div>
        <div class="padleft20">
            <asp:Label runat="server" ID="lblExport" resourcekey="ExportConfigurations.Text"></asp:Label>:
            <asp:Button runat="server" ID="btnExport" resourcekey="Export.Text" CausesValidation="False" />
            <br />
            <asp:Label runat="server" ID="lblImport" resourcekey="ImportConfigurations.Text"></asp:Label>:
            <asp:FileUpload runat="server" ID="fuImport" />
            <asp:Button runat="server" ID="btnImport" resourcekey="Import.Text" ValidationGroup="ImportConfigurations" />
            <asp:RegularExpressionValidator ID="rqPdfFileExtension" ControlToValidate="fuImport"
                ValidationExpression="^.*\.(zip|ZIP)$" runat="server" ValidationGroup="ImportConfigurations"
                resourcekey="RequireZipFile.Text" Display="Dynamic" />
            <asp:CustomValidator runat="server" ID="cusvldImport" resourcekey="RequireZipFile.Text"
                ControlToValidate="fuImport" ValidateEmptyText="True" Display="Dynamic" ValidationGroup="ImportConfigurations"
                ClientValidationFunction="ValidateImportFile" EnableClientScript="True"></asp:CustomValidator>
        </div>
    </div>
    <br />
    <div class="configuration-footer text-center">
        <asp:Button runat="server" ID="btnSave" resourcekey="Save.Text" />
        <asp:Button runat="server" ID="btnReset" resourcekey="Reset.Text" CausesValidation="False" />
        <asp:Button runat="server" ID="btnBuildQuestion" Text="Build Questions" Visible="False"
            CausesValidation="False" />
        <asp:Button ID="btnClose" runat="server" Text="Close" CausesValidation="False" />
    </div>
</div>
<div id="form-configuration-popup" class="amc-popup">
    <table>
        <tr>
            <td>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator18" ControlToValidate="txtNewValue"
                    CssClass="fl" ErrorMessage="Input value is required." Display="Dynamic" ValidationGroup="NewFormSectionTextPopUp"></asp:RequiredFieldValidator>
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
<div id="select-application-for-audit" class="amc-popup">
    <div style="width: 600px; overflow: auto; height: 400px;">
        <asp:Label runat="server" ID="lblSelectedApplicationForAuditPopupTitle"></asp:Label>
        <asp:GridView ID="grvSelectedApplicationForAudit" runat="server" AutoGenerateColumns="False"
            Width="600px">
            <HeaderStyle CssClass="AuditGridHeader" VerticalAlign="Middle" ForeColor="white">
            </HeaderStyle>
            <Columns>
                <asp:TemplateField HeaderText="Certification ID">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblCertificationId" Text='<%# Eval("CertificationId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--                <asp:TemplateField HeaderText="Certification Number">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblCertificationNumber" Text='<%# Eval("CertificationNumber") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Certification Expiration Date">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblCertificationExpiredDate" Text='<%# Eval("CertificationExpirationDate") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Customer Name">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblCustomerName"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</div>
<div id="select-review-process" class="amc-popup">
    <div style="width: 600px; overflow: auto; height: 400px;">
        <asp:Repeater runat="server" ID="rptReviewProcess" EnableViewState="True">
            <HeaderTemplate>
                <table border="1px" class="amc-table" id="tbl-board" style="width: 100%">
                    <tr class="AuditGridHeader">
                        <td>
                            <asp:Label runat="server" ID="lblCustomerName" Text="Customer Name" resourcekey="CustomerName.Text"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblApplicationDate" Text="Application Date" resourcekey="ApplicationDate.Text"></asp:Label>
                        </td>
                        <td style="width: 120px;">
                        </td>
                    </tr>
            </HeaderTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblCustomerName"></asp:Label>
                    </td>
                    <td class="text-right">
                        <asp:Label runat="server" ID="lblApplicationDate"></asp:Label>
                    </td>
                    <td class="text-center">
                        <asp:HyperLink ID="hlPrintApplication" NavigateUrl="javascript:void(0)" runat="server"
                            Text="View Application" />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>
