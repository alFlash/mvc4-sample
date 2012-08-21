<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ReferenceAndVerificationUC.ascx.vb"
    Inherits="AMC.DNN.Modules.CertRecert.Controls.Common.ReferenceAndVerificationUC" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<script type="text/javascript">
    jQuery(document).ready(function () {

        var referencePopupBindingList = [
                            ['lblFirstName', 'txtFirstName'],
                            ['lblMiddleName', 'txtMiddleName'],
                           ['lblName', 'txtName'],
                           ['lblDegree', 'txtDegree'],
                           ['lblEmployer', 'txtEmployer'],
                           ['lblWorkAddress1', 'txtWorkAddress1'],
                            ['lblWorkAddress2', 'txtWorkAddress2'],
                            ['lblWorkAddress3', 'txtWorkAddress3'],
                            ['lblWorkAddress4', 'txtWorkAddress4'],
                           ['lblWorkTelephone', 'txtWorkTelephone'],
                           ['lblWorkEmail', 'txtWorkEmail'],
                           ['lblHomeAddress1', 'txtHomeAddress1'],
                            ['lblHomeAddress2', 'txtHomeAddress2'],
                            ['lblHomeAddress3', 'txtHomeAddress3'],
                            ['lblHomeAddress4', 'txtHomeAddress4'],
                           ['lblHomeTelephone', 'txtHomeTelephone'],
                           ['lblHomeEmail', 'txtHomeEmail'],
                           ['lblCity', 'txtCity'],
                           ['lblState', 'ddlWorkState'],
                           ['lblTitle', 'txtTitle'],
                           ['lblWorkZip', 'txtWorkZip'],
                           ['lblHomeCity', 'txtHomeCity'],
                           ['lblHomeState', 'ddlHomeState'],
                           ['lblHomeZip', 'txtHomeZip'],
                           ['lblPrefContactMethod', 'ddlPrefContactMethod'],
                           ['hlAttachedDocumentName', 'fuUploadFileAttachment'],
                           ['hlAttachedDocumentName', 'hlUploadFileAttachment']];
        var referencePopupInstance = new AMCTablePopUp(referencePopupBindingList, "uc_reference", "tblReference",
            "popup_add_reference", "Add Information ", "Edit Item", null);
        if (jQuery('#[id*=hdCurrentSectionPopupOpenningId]').val() != '' && jQuery('#[id*=hdCurrentSectionPopupOpenningId]').val() != null
        && jQuery('#[id*=hdIsValidateFailed]').val() == '1') {
            if (!IsStringNullOrEmpty(jQuery('#[id*=hdCurrentObjectUniqueId]').val())) {
                var currentRow = jQuery('[guid=' + jQuery('#[id*=hdCurrentObjectUniqueId]').val() + ']');
                if (currentRow.length > 0) {
                    AMCTablePopUp.FillData(currentRow, jQuery('#[id=' + jQuery('#[id*=hdCurrentSectionPopupOpenningId]').val() + ']'), [['hlAttachedDocumentName', 'fuUploadFileAttachment'],
                           ['hlAttachedDocumentName', 'hlUploadFileAttachment']]);
                    //                    var contactClassTypeValue = jQuery(currentRow).find('[id*=lblContactClass]').text();
                    //                    var rdlContactClass = jQuery('#popup_add_reference [id*=rdlContactClass_]');
                    //                    jQuery(rdlContactClass[0]).prop('checked', true);
                    //                    for (var i = 0; i < rdlContactClass.length; i++) {
                    //                        if (jQuery(rdlContactClass[i]).val() == contactClassTypeValue) {
                    //                            jQuery(rdlContactClass[i]).prop('checked', true);
                    //                            break;
                    //                        }
                    //                    }
                }
            }
        }
        var inteval = setInterval(function () {
            CommonHelper.ClearPhoneEmpty('popup_add_reference', 'txtWorkTelephone');
            CommonHelper.ClearPhoneEmpty('popup_add_reference', 'txtHomeTelephone');
            clearInterval(inteval);
        }, 500);
        //set value for radio buttons 
        jQuery('#uc_reference').delegate('[id*=imgAddAttachment]', 'click', function (ev) {
            bindValueToContactClass(ev);
        });

        //set value for radio buttons 
        jQuery('#uc_reference').delegate('[id*=imgEdit]', 'click', function (ev) {
            bindValueToContactClass(ev);
        });

        function bindValueToContactClass(ev) {
            ev.stopPropagation();
            CommonHelper.ClearPhoneEmpty('popup_add_reference', 'txtWorkTelephone');
            CommonHelper.ClearPhoneEmpty('popup_add_reference', 'txtHomeTelephone');
            var contactClassTypeValue = jQuery(AMCTablePopUp.CurrentRow).find('[id*=lblContactClass]').text();
            var rdlContactClass = jQuery('#popup_add_reference [id*=rdlContactClass_]');
            jQuery(rdlContactClass[0]).prop('checked', true);
            for (var i = 0; i < rdlContactClass.length; i++) {
                if (jQuery(rdlContactClass[i]).val() == contactClassTypeValue) {
                    jQuery(rdlContactClass[i]).prop('checked', true);
                    break;
                }
            }
        }
        //set initial value for contact class radio buttons
        jQuery('#uc_reference').delegate('#add-new-row', 'click', function (ev) {
            ev.stopPropagation();
            CommonHelper.ClearPhoneEmpty('popup_add_reference', 'txtWorkTelephone');
            CommonHelper.ClearPhoneEmpty('popup_add_reference', 'txtHomeTelephone');
            jQuery.each(jQuery('#popup_add_reference [id*=rdlContactClass_]'), function (idx, elem) {
                if (idx == 0) {
                    jQuery(elem).prop('checked', true);
                } else {
                    jQuery(elem).prop('checked', false);
                }
            });
        });

        var altertiveReferences = jQuery('#alternative-verification li[id*=item_]');
        for (var i = 0; i < altertiveReferences.length; i++) {
            var currentAlternativeReference = jQuery(altertiveReferences[i]);
            var hddocumentName = currentAlternativeReference.find('#[id*=hdUploadDocumentName]');
            var hdDocumentLink = currentAlternativeReference.find('#[id*=hdUploaddocumentLink]');
            if (!IsStringNullOrEmpty(hddocumentName.val()) && !IsStringNullOrEmpty(hdDocumentLink.val())) {
                var uploadContainer = currentAlternativeReference.find('#divUploadFileAttachment');
                uploadContainer.show();
                var hlUploadFileAttachment = currentAlternativeReference.find('#[id*=hlUploadFileAttachment]');
                hlUploadFileAttachment.text(hddocumentName.val());
                hlUploadFileAttachment.attr('href', hdDocumentLink.val());
            }
        }

        jQuery.each(jQuery('#alternative-verification li[id*=item_] #[id*=imgDeleteAttachment]'), function (idx, deleteIcon) {
            jQuery(deleteIcon).click(function () {
                if (confirm(ConfirmDeleteFile)) {
                    var guid = jQuery(deleteIcon).attr('guid');
                    var currentAlternativeRow = jQuery('#alternative-verification li[guid=' + guid + ']');
                    var currenthdDocumentLink = currentAlternativeRow.find('#[id*=hdUploaddocumentLink]');
                    deleteDocument(currenthdDocumentLink.val(), function (xmlhttp) {
                        reference_verification_processdeletedocument(xmlhttp, deleteIcon);
                    });
                }
                return false;
            });
        });
    });


    function reference_verification_processdeletedocument(xmlhttp, deleteIcon) {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            //delete file on server successfully
            if (xmlhttp.responseText.startsWith("1")) {
                var guid = jQuery(deleteIcon).attr('guid');
                var currentRow = jQuery('#alternative-verification li[guid=' + guid + ']');
                var chkAlternativeVerification = jQuery(currentRow).find('#[id*=chkAlternativeVerification]');
                chkAlternativeVerification.prop('checked', false);
                var hddocumentName = currentRow.find('#[id*=hdUploadDocumentName]');
                hddocumentName.val('');
                var hdDocumentLink = currentRow.find('#[id*=hdUploaddocumentLink]');
                hdDocumentLink.val('');
                var uploadContainer = currentRow.find('#divUploadFileAttachment');
                uploadContainer.hide();
                var hlUploadFileAttachment = currentRow.find('#[id*=hlUploadFileAttachment]');
                hlUploadFileAttachment.text('');
                hlUploadFileAttachment.attr('href', '#');
                jQuery('#[id*=hdIsIncomplete]').val('1');
            } else {
                alert(DeleteFileError);
            }
        }
    }

    function Reference_ValidateAlternativeVerification(sender, args) {
        args.IsValid = false;
        var control = jQuery('#' + sender.controltovalidate);
        var guid = control.attr('guid');
        var currentRow = jQuery('#alternative-verification li[guid=' + guid + ']');
        var chkAlternativeVerification = jQuery(currentRow).find('#[id*=chkAlternativeVerification]');
        var hdUploaddocumentLink = jQuery(currentRow).find('#[id*=hdUploaddocumentLink]');
        if (chkAlternativeVerification.prop('checked') == true && (!IsStringNullOrEmpty(control.val()) || !IsStringNullOrEmpty(hdUploaddocumentLink.val()))) {
            args.IsValid = true;
        }else if (chkAlternativeVerification.prop('checked') == false) {
            args.IsValid = true;
        }
    }
</script>
<div id="uc_reference" class="amc-page">
    <div>
        <asp:HiddenField runat="server" ID="hdDeletedFileList" />
        <asp:HiddenField runat="server" ID="hdIsIncomplete" />
        <asp:HiddenField runat="server" ID="hdIsValidateFailed" />
        <asp:HiddenField runat="server" ID="hdCurrentObjectUniqueId" />
    </div>
    <div class="amc-title">
        <asp:Label runat="server" ID="lblReferenceAndVerificationUC"></asp:Label>
    </div>
    <div id="FormFillingInstructions" runat="server" class="amc-instruction">
        <asp:Label runat="server" ID="lblFormFillingInstructions"></asp:Label>
    </div>
    <div class="amc-contents">
        <div id="add-new-row" class="amc-add-instruction">
            <asp:Image CssClass="pointer fl" runat="server" ID="Image1" ImageUrl="../../Documentation/images/icons/addnew_11x11.gif">
            </asp:Image>
            <asp:HyperLink runat="server" ID="hlAddNew" resourcekey="AddInformation.Text" CssClass="fl padleft5"
                href="javascript:void(0);"></asp:HyperLink>
            <div class="cb">
            </div>
        </div>
        <div class="width900">
            <%--27--%>
            <asp:Repeater runat="server" ID="rptPrintReferences" Visible="False">
                <HeaderTemplate>
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="reference-verification-print-item">
                        <div class="reference-verification-print-number">
                            <asp:Label runat="server" ID="lblItemNo" Text="Reference #{0}: " Font-Bold="True"></asp:Label>
                        </div>
                        <div class="width400 fl">
                            <div visible='<%# GetFieldInfo("FirstName").IsEnabled %>' runat="server" class="reference-verification-print-row">
                                <div class="width200 fl">
                                    <asp:Label runat="server" ID="lblFirstNameHeader" Text='<%# GetFieldInfo("FirstName").FieldValue + ":" %>'></asp:Label>
                                </div>
                                <div class="width200 fr">
                                    <asp:Label runat="server" ID="lblFirstName"></asp:Label>
                                </div>
                                <div class="cb">
                                </div>
                            </div>
                            <div visible='<%# GetFieldInfo("MiddleName").IsEnabled %>' runat="server">
                                <div class="width200 fl">
                                    <asp:Label runat="server" ID="lblMiddleNameHeader" Text='<%# GetFieldInfo("MiddleName").FieldValue + ":"  %>'></asp:Label>
                                </div>
                                <div class="width200 fr">
                                    <asp:Label runat="server" ID="lblMiddleName"></asp:Label>
                                </div>
                                <div class="cb">
                                </div>
                            </div>
                            <div visible='<%# GetFieldInfo("Name").IsEnabled %>' runat="server">
                                <div class="width200 fl">
                                    <asp:Label runat="server" ID="lblLastNameHeader" Text='<%# GetFieldInfo("Name").FieldValue + ":"  %>'></asp:Label>
                                </div>
                                <div class="width200 fr">
                                    <asp:Label runat="server" ID="lblName"></asp:Label>
                                </div>
                                <div class="cb">
                                </div>
                            </div>
                            <div visible='<%# GetFieldInfo("Title").IsEnabled %>' runat="server">
                                <div class="width200 fl">
                                    <asp:Label runat="server" ID="Label10" Text='<%# GetFieldInfo("Title").FieldValue + ":"  %>'></asp:Label>
                                </div>
                                <div class="width200 fr">
                                    <asp:Label runat="server" ID="lblTitle" Text='<%# Eval("JobTitle") %> '></asp:Label>
                                </div>
                                <div class="cb">
                                </div>
                            </div>
                            <div visible='<%# GetFieldInfo("Degree").IsEnabled %>' runat="server">
                                <div class="width200 fl">
                                    <asp:Label runat="server" ID="Label15" Text='<%# GetFieldInfo("Degree").FieldValue + ":"  %>'></asp:Label>
                                </div>
                                <div class="width200 fr">
                                    <asp:Label runat="server" ID="lblDegree" Text='<%# Eval("Degree") %>'></asp:Label>
                                </div>
                                <div class="cb">
                                </div>
                            </div>
                            <div visible='<%# GetFieldInfo("Employer").IsEnabled %>' runat="server">
                                <div class="width200 fl">
                                    <asp:Label runat="server" ID="Label16" Text='<%# GetFieldInfo("Employer").FieldValue + ":"  %>'></asp:Label>
                                </div>
                                <div class="width200 fr">
                                    <asp:Label runat="server" ID="lblEmployer" Text='<%# Eval("Employer") %>'></asp:Label>
                                </div>
                                <div class="cb">
                                </div>
                            </div>
                            <div visible='<%# GetFieldInfo("PrefContactMethod").IsEnabled %>' runat="server">
                                <div class="width200 fl">
                                    <asp:Label runat="server" ID="lblPrefContactMethod" Text='<%# GetFieldInfo("PrefContactMethod").FieldValue + ":"  %>'>
                                    </asp:Label>
                                </div>
                                <div class="width200 fr">
                                    <asp:Label runat="server" ID="lblDescPrefContactMethod"></asp:Label>
                                    <asp:Label runat="server" ID="Label21" Text='<%# Eval("PrefContactMethodString") %>'
                                        CssClass="dn"></asp:Label>
                                </div>
                                <div class="cb">
                                </div>
                            </div>
                            <div visible='<%# GetFieldInfo("ContactClass").IsEnabled %>' runat="server">
                                <div class="width200 fl">
                                    <asp:Label runat="server" ID="lblContactClass" Text='<%# GetFieldInfo("ContactClass").FieldValue + ":"  %>'></asp:Label>
                                </div>
                                <div class="width200 fr">
                                    <asp:Label runat="server" ID="lblDescContactClass"></asp:Label>
                                    <asp:Label runat="server" ID="Label22" Text='<%# Eval("ContactClassTypeString") %>'
                                        CssClass="dn"></asp:Label>
                                </div>
                                <div class="cb">
                                </div>
                            </div>
                            <div visible='<%# GetFieldInfo("WorkAddress1").IsEnabled %>' runat="server">
                                <div class="width200 fl">
                                    <asp:Label runat="server" ID="Label17" Text='<%# GetFieldInfo("WorkAddress1").FieldValue + ":"  %>'></asp:Label>
                                </div>
                                <div class="width200 fr">
                                    <asp:Label runat="server" ID="lblWorkAddress1"></asp:Label>
                                </div>
                                <div class="cb">
                                </div>
                            </div>
                            <div visible='<%# GetFieldInfo("WorkAddress2").IsEnabled %>' runat="server">
                                <div class="width200 fl">
                                    <asp:Label runat="server" ID="Label5" Text='<%# GetFieldInfo("WorkAddress2").FieldValue + ":"  %>'></asp:Label>
                                </div>
                                <div class="width200 fr">
                                    <asp:Label runat="server" ID="lblWorkAddress2"></asp:Label>
                                </div>
                                <div class="cb">
                                </div>
                            </div>
                            <div visible='<%# GetFieldInfo("WorkAddress3").IsEnabled %>' runat="server">
                                <div class="width200 fl">
                                    <asp:Label runat="server" ID="Label6" Text='<%# GetFieldInfo("WorkAddress3").FieldValue + ":"  %>'></asp:Label>
                                </div>
                                <div class="width200 fr">
                                    <asp:Label runat="server" ID="lblWorkAddress3"></asp:Label>
                                </div>
                                <div class="cb">
                                </div>
                            </div>
                            <div visible='<%# GetFieldInfo("WorkAddress4").IsEnabled %>' runat="server">
                                <div class="width200 fl">
                                    <asp:Label runat="server" ID="Label4" Text='<%# GetFieldInfo("WorkAddress4").FieldValue + ":"  %>'></asp:Label>
                                </div>
                                <div class="width200 fr">
                                    <asp:Label runat="server" ID="lblWorkAddress4"></asp:Label>
                                </div>
                                <div class="cb">
                                </div>
                            </div>
                            <div visible='<%# GetFieldInfo("City").IsEnabled %>' runat="server">
                                <div class="width200 fl">
                                    <asp:Label runat="server" ID="Label11" Text='<%# GetFieldInfo("City").FieldValue + ":"  %>'></asp:Label>
                                </div>
                                <div class="width200 fr">
                                    <asp:Label runat="server" ID="lblCity"></asp:Label>
                                </div>
                                <div class="cb">
                                </div>
                            </div>
                        </div>
                        <div class="width400 fr">
                            <div id="Div1" visible='<%# GetFieldInfo("State").IsEnabled %>' runat="server">
                                <div class="width200 fl">
                                    <asp:Label runat="server" ID="Label12" Text='<%# GetFieldInfo("State").FieldValue + ":"  %>'></asp:Label>
                                </div>
                                <div class="width200 fr">
                                    <asp:Label runat="server" ID="lblState" CssClass="dn"></asp:Label>
                                    <asp:Label runat="server" ID="lblDescriptionState"></asp:Label>
                                </div>
                                <div class="cb">
                                </div>
                            </div>
                            <div id="Div2" visible='<%# GetFieldInfo("WorkZip").IsEnabled %>' runat="server">
                                <div class="width200 fl">
                                    <asp:Label runat="server" ID="Label13" Text='<%# GetFieldInfo("WorkZip").FieldValue + ":"  %>'></asp:Label>
                                </div>
                                <div class="width200 fr">
                                    <asp:Label runat="server" ID="lblWorkZip"></asp:Label>
                                </div>
                                <div class="cb">
                                </div>
                            </div>
                            <div id="Div3" visible='<%# GetFieldInfo("WorkTelephone").IsEnabled %>' runat="server">
                                <div class="width200 fl">
                                    <asp:Label runat="server" ID="Label18" Text='<%# GetFieldInfo("WorkTelephone").FieldValue + ":"  %>'></asp:Label>
                                </div>
                                <div class="width200 fr">
                                    <asp:Label runat="server" ID="lblWorkTelephone"></asp:Label>
                                </div>
                                <div class="cb">
                                </div>
                            </div>
                            <div id="Div4" visible='<%# GetFieldInfo("WorkEmail").IsEnabled %>' runat="server">
                                <div class="width200 fl">
                                    <asp:Label runat="server" ID="Label19" Text='<%# GetFieldInfo("WorkEmail").FieldValue + ":"  %>'></asp:Label>
                                </div>
                                <div class="width200 fr">
                                    <asp:Label runat="server" ID="lblWorkEmail"></asp:Label>
                                </div>
                                <div class="cb">
                                </div>
                            </div>
                            <div id="Div5" visible='<%# GetFieldInfo("HomeAddress1").IsEnabled %>' runat="server">
                                <div class="width200 fl">
                                    <asp:Label runat="server" ID="Label20" Text='<%# GetFieldInfo("HomeAddress1").FieldValue + ":"  %>'></asp:Label>
                                </div>
                                <div class="width200 fr">
                                    <asp:Label runat="server" ID="lblHomeAddress1"></asp:Label>
                                </div>
                                <div class="cb">
                                </div>
                            </div>
                            <div id="Div6" visible='<%# GetFieldInfo("HomeAddress2").IsEnabled %>' runat="server">
                                <div class="width200 fl">
                                    <asp:Label runat="server" ID="Label2" Text='<%# GetFieldInfo("HomeAddress2").FieldValue + ":"  %>'></asp:Label>
                                </div>
                                <div class="width200 fr">
                                    <asp:Label runat="server" ID="lblHomeAddress2"></asp:Label>
                                </div>
                                <div class="cb">
                                </div>
                            </div>
                            <div id="Div7" visible='<%# GetFieldInfo("HomeAddress3").IsEnabled %>' runat="server">
                                <div class="width200 fl">
                                    <asp:Label runat="server" ID="Label3" Text='<%# GetFieldInfo("HomeAddress3").FieldValue + ":"  %>'></asp:Label>
                                </div>
                                <div class="width200 fr">
                                    <asp:Label runat="server" ID="lblHomeAddress3"></asp:Label>
                                </div>
                                <div class="cb">
                                </div>
                            </div>
                            <div id="Div8" visible='<%# GetFieldInfo("HomeAddress4").IsEnabled %>' runat="server">
                                <div class="width200 fl">
                                    <asp:Label runat="server" ID="Label9" Text='<%# GetFieldInfo("HomeAddress4").FieldValue + ":"  %>'></asp:Label>
                                </div>
                                <div class="width200 fr">
                                    <asp:Label runat="server" ID="lblHomeAddress4"></asp:Label>
                                </div>
                                <div class="cb">
                                </div>
                            </div>
                            <div id="Div9" visible='<%# GetFieldInfo("HomeCity").IsEnabled %>' runat="server">
                                <div class="width200 fl">
                                    <asp:Label runat="server" ID="lblHomeCityHeader" Text='<%# GetFieldInfo("HomeCity").FieldValue + ":"  %>'></asp:Label>
                                </div>
                                <div class="width200 fr">
                                    <asp:Label runat="server" ID="lblHomeCity"></asp:Label>
                                </div>
                                <div class="cb">
                                </div>
                            </div>
                            <div id="Div10" visible='<%# GetFieldInfo("HomeState").IsEnabled %>' runat="server">
                                <div class="width200 fl">
                                    <asp:Label runat="server" ID="lblHomeStateHeader" Text='<%# GetFieldInfo("HomeState").FieldValue + ":"  %>'></asp:Label>
                                </div>
                                <div class="width200 fr">
                                    <asp:Label runat="server" ID="lblHomeState" CssClass="dn"></asp:Label>
                                    <asp:Label runat="server" ID="lblDescriptionHomeState"></asp:Label>
                                </div>
                                <div class="cb">
                                </div>
                            </div>
                            <div id="Div11" visible='<%# GetFieldInfo("HomeZip").IsEnabled %>' runat="server">
                                <div class="width200 fl">
                                    <asp:Label runat="server" ID="lblHomeZipHeader" Text='<%# GetFieldInfo("HomeZip").FieldValue + ":"  %>'></asp:Label>
                                </div>
                                <div class="width200 fr">
                                    <asp:Label runat="server" ID="lblHomeZip"></asp:Label>
                                </div>
                                <div class="cb">
                                </div>
                            </div>
                            <div id="Div12" visible='<%# GetFieldInfo("HomeTelephone").IsEnabled %>' runat="server">
                                <div class="width200 fl">
                                    <asp:Label runat="server" ID="Label7" Text='<%# GetFieldInfo("HomeTelephone").FieldValue + ":"  %>'></asp:Label>
                                </div>
                                <div class="width200 fr">
                                    <asp:Label runat="server" ID="lblHomeTelephone"></asp:Label>
                                </div>
                                <div class="cb">
                                </div>
                            </div>
                            <div id="Div13" visible='<%# GetFieldInfo("HomeEmail").IsEnabled %>' runat="server">
                                <div class="width200 fl">
                                    <asp:Label runat="server" ID="Label8" Text='<%# GetFieldInfo("HomeEmail").FieldValue + ":"  %>'></asp:Label>
                                </div>
                                <div class="width200 fr">
                                    <asp:Label runat="server" ID="lblHomeEmail"></asp:Label>
                                </div>
                                <div class="cb">
                                </div>
                            </div>
                            <div id="Div14" runat="server" visible='<%# GetFieldInfo("UploadFileAttachment").IsEnabled %>'>
                                <div class="width200 fl">
                                    <asp:Label runat="server" ID="Label1" Text='<%# GetFieldInfo("UploadFileAttachment").FieldValue + ":"  %>'></asp:Label>
                                </div>
                                <div class="width200 fr">
                                    <asp:HiddenField runat="server" ID="hdAttachedDocumentName" />
                                    <div class="add-image dn margin-auto" id="imgAddAttachment" runat="server">
                                    </div>
                                    <asp:HyperLink runat="server" ID="hlAttachedDocumentName" Target="_blank"></asp:HyperLink>
                                </div>
                                <div class="cb">
                                </div>
                            </div>
                        </div>
                        <div class="cb">
                        </div>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <div style="width: 910px; overflow-x: auto;">
            <asp:Repeater runat="server" ID="rptReferences" EnableViewState="True">
                <HeaderTemplate>
                    <table id="tblReference" border="1px" class="amc-table">
                        <tr class="amc-table-header">
                            <td id="Td1" visible='<%# GetFieldInfo("FirstName").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblFirstNameHeader" Text='<%# GetFieldInfo("FirstName").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td2" visible='<%# GetFieldInfo("MiddleName").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblMiddleNameHeader" Text='<%# GetFieldInfo("MiddleName").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td3" visible='<%# GetFieldInfo("Name").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblName" Text='<%# GetFieldInfo("Name").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td35" visible='<%# GetFieldInfo("Title").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="Label10" Text='<%# GetFieldInfo("Title").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td4" visible='<%# GetFieldInfo("Degree").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="Label15" Text='<%# GetFieldInfo("Degree").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td5" visible='<%# GetFieldInfo("Employer").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="Label16" Text='<%# GetFieldInfo("Employer").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td36" visible='<%# GetFieldInfo("PrefContactMethod").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblPrefContactMethod" Text='<%# GetFieldInfo("PrefContactMethod").FieldValue %>'>
                                </asp:Label>
                            </td>
                            <td id="Td37" visible='<%# GetFieldInfo("ContactClass").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblContactClass" Text='<%# GetFieldInfo("ContactClass").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td6" visible='<%# GetFieldInfo("WorkAddress1").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="Label17" Text='<%# GetFieldInfo("WorkAddress1").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td32" visible='<%# GetFieldInfo("WorkAddress2").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="Label5" Text='<%# GetFieldInfo("WorkAddress2").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td33" visible='<%# GetFieldInfo("WorkAddress3").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="Label6" Text='<%# GetFieldInfo("WorkAddress3").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td44" visible='<%# GetFieldInfo("WorkAddress4").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="Label4" Text='<%# GetFieldInfo("WorkAddress4").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td41" visible='<%# GetFieldInfo("City").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="Label11" Text='<%# GetFieldInfo("City").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td42" visible='<%# GetFieldInfo("State").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="Label12" Text='<%# GetFieldInfo("State").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td43" visible='<%# GetFieldInfo("WorkZip").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="Label13" Text='<%# GetFieldInfo("WorkZip").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td7" visible='<%# GetFieldInfo("WorkTelephone").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="Label18" Text='<%# GetFieldInfo("WorkTelephone").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td8" visible='<%# GetFieldInfo("WorkEmail").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="Label19" Text='<%# GetFieldInfo("WorkEmail").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td9" visible='<%# GetFieldInfo("HomeAddress1").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="Label20" Text='<%# GetFieldInfo("HomeAddress1").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td29" visible='<%# GetFieldInfo("HomeAddress2").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="Label2" Text='<%# GetFieldInfo("HomeAddress2").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td30" visible='<%# GetFieldInfo("HomeAddress3").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="Label3" Text='<%# GetFieldInfo("HomeAddress3").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td20" visible='<%# GetFieldInfo("HomeAddress4").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="Label9" Text='<%# GetFieldInfo("HomeAddress4").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td38" visible='<%# GetFieldInfo("HomeCity").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblHomeCity" Text='<%# GetFieldInfo("HomeCity").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td39" visible='<%# GetFieldInfo("HomeState").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblHomeState" Text='<%# GetFieldInfo("HomeState").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td40" visible='<%# GetFieldInfo("HomeZip").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="lblHomeZip" Text='<%# GetFieldInfo("HomeZip").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td10" visible='<%# GetFieldInfo("HomeTelephone").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="Label7" Text='<%# GetFieldInfo("HomeTelephone").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td11" visible='<%# GetFieldInfo("HomeEmail").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="Label8" Text='<%# GetFieldInfo("HomeEmail").FieldValue %>'></asp:Label>
                            </td>
                            <td id="Td12" visible='<%# GetFieldInfo("UploadFileAttachment").IsEnabled %>' runat="server">
                                <asp:Label runat="server" ID="Label1" Text='<%# GetFieldInfo("UploadFileAttachment").FieldValue %>'></asp:Label>
                            </td>
                            <td class="action-column" style="color: #808080">
                                Action
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="amc-table-content" runat="server" id='item_'>
                        <td id="Td13" visible='<%# GetFieldInfo("FirstName").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblFirstName"></asp:Label>
                        </td>
                        <td id="Td14" visible='<%# GetFieldInfo("MiddleName").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblMiddleName"></asp:Label>
                        </td>
                        <td id="Td15" visible='<%# GetFieldInfo("Name").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblName"></asp:Label>
                        </td>
                        <td id="Td3" visible='<%# GetFieldInfo("Title").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblTitle" Text='<%# Eval("JobTitle") %> '></asp:Label>
                        </td>
                        <td id="Td16" visible='<%# GetFieldInfo("Degree").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblDegree" Text='<%# Eval("Degree") %>'></asp:Label>
                        </td>
                        <td id="Td17" visible='<%# GetFieldInfo("Employer").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblEmployer" Text='<%# Eval("Employer") %>'></asp:Label>
                        </td>
                        <td id="Td4" visible='<%# GetFieldInfo("PrefContactMethod").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblDescPrefContactMethod"></asp:Label>
                            <asp:Label runat="server" ID="lblPrefContactMethod" Text='<%# Eval("PrefContactMethodString") %>'
                                CssClass="dn"></asp:Label>
                        </td>
                        <td id="Td9" visible='<%# GetFieldInfo("ContactClass").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblDescContactClass"></asp:Label>
                            <asp:Label runat="server" ID="lblContactClass" Text='<%# Eval("ContactClassTypeString") %>'
                                CssClass="dn"></asp:Label>
                        </td>
                        <td id="Td18" visible='<%# GetFieldInfo("WorkAddress1").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblWorkAddress1"></asp:Label>
                        </td>
                        <td id="Td12" visible='<%# GetFieldInfo("WorkAddress2").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblWorkAddress2"></asp:Label>
                        </td>
                        <td id="Td19" visible='<%# GetFieldInfo("WorkAddress3").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblWorkAddress3"></asp:Label>
                        </td>
                        <td id="Td26" visible='<%# GetFieldInfo("WorkAddress4").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblWorkAddress4"></asp:Label>
                        </td>
                        <td id="Td1" visible='<%# GetFieldInfo("City").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblCity"></asp:Label>
                        </td>
                        <td id="Td2" visible='<%# GetFieldInfo("State").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblState" CssClass="dn"></asp:Label>
                            <asp:Label runat="server" ID="lblDescriptionState"></asp:Label>
                        </td>
                        <td id="Td8" visible='<%# GetFieldInfo("WorkZip").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblWorkZip"></asp:Label>
                        </td>
                        <td id="Td21" visible='<%# GetFieldInfo("WorkTelephone").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblWorkTelephone"></asp:Label>
                        </td>
                        <td id="Td22" visible='<%# GetFieldInfo("WorkEmail").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblWorkEmail"></asp:Label>
                        </td>
                        <td id="Td23" visible='<%# GetFieldInfo("HomeAddress1").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblHomeAddress1"></asp:Label>
                        </td>
                        <td id="Td24" visible='<%# GetFieldInfo("HomeAddress2").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblHomeAddress2"></asp:Label>
                        </td>
                        <td id="Td25" visible='<%# GetFieldInfo("HomeAddress3").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblHomeAddress3"></asp:Label>
                        </td>
                        <td id="Td31" visible='<%# GetFieldInfo("HomeAddress4").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblHomeAddress4"></asp:Label>
                        </td>
                        <td id="Td5" visible='<%# GetFieldInfo("HomeCity").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblHomeCity"></asp:Label>
                        </td>
                        <td id="Td6" visible='<%# GetFieldInfo("HomeState").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblHomeState" CssClass="dn"></asp:Label>
                            <asp:Label runat="server" ID="lblDescriptionHomeState"></asp:Label>
                        </td>
                        <td id="Td7" visible='<%# GetFieldInfo("HomeZip").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblHomeZip"></asp:Label>
                        </td>
                        <td id="Td27" visible='<%# GetFieldInfo("HomeTelephone").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblHomeTelephone"></asp:Label>
                        </td>
                        <td id="Td28" visible='<%# GetFieldInfo("HomeEmail").IsEnabled %>' runat="server">
                            <asp:Label runat="server" ID="lblHomeEmail"></asp:Label>
                        </td>
                        <td id="Td10" runat="server" visible='<%# GetFieldInfo("UploadFileAttachment").IsEnabled %>'>
                            <asp:HiddenField runat="server" ID="hdAttachedDocumentName" />
                            <div class="add-image dn margin-auto" id="imgAddAttachment" runat="server">
                            </div>
                            <asp:HyperLink runat="server" ID="hlAttachedDocumentName" Target="_blank"></asp:HyperLink>
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
        <div class="pad5">
            <asp:HiddenField runat="server" ID="hdAlternativeVerificationSurveyId" />
            <asp:Repeater runat="server" ID="rptAlternativeVerification">
                <HeaderTemplate>
                    <ul id="alternative-verification" style="list-style: none; padding-left: 0px">
                </HeaderTemplate>
                <ItemTemplate>
                    <li runat="server" id="item_" visible='<%# Eval("Enabled") %>'>
                        <asp:CheckBox runat="server" ID="chkAlternativeVerification" Text='<%# Eval("QuestionText") %>'>
                        </asp:CheckBox>
                        <div class="pad10">
                            <div class="fl">
                                <asp:HiddenField runat="server" ID="hdQuestionEnabled" Value='<%# Eval("Enabled") %>' />
                                <asp:HiddenField runat="server" ID="hdQuestionId" Value='<%# Eval("QuestionId") %>' />
                                <asp:HiddenField runat="server" ID="hdAnswerYes" />
                                <asp:HiddenField runat="server" ID="hdAnswerNo" />
                                <asp:HiddenField runat="server" ID="hdResponseId" />
                                <asp:Label ID="lblUploadFileAttachment" runat="server" Text="Attach Document: "></asp:Label>
                            </div>
                            <div class="fl">
                                <asp:FileUpload ID="fuUploadFileAttachment" runat="server" />
                                <asp:RegularExpressionValidator ID="rqPdfFileExtension" ControlToValidate="fuUploadFileAttachment"
                                    ValidationExpression="^.*\.(pdf|PDF)$" runat="server" ValidationGroup="AmcGeneralGroup"
                                    resourcekey="RequirePDFFile.Text" Display="Dynamic" />
                                <asp:CustomValidator runat="server" ID="cusValidOtherBoard" ControlToValidate="fuUploadFileAttachment"
                                    ClientValidationFunction="Reference_ValidateAlternativeVerification" Display="Dynamic"
                                    ValidationGroup="AmcGeneralGroup" resourcekey="PleaseAttachDocument.Text" EnableClientScript="True"
                                    ValidateEmptyText="True"></asp:CustomValidator>
                                <div id="divUploadFileAttachment" class="dn">
                                    <asp:HiddenField runat="server" ID="hdUploadDocumentName" />
                                    <asp:HiddenField runat="server" ID="hdUploaddocumentLink" />
                                    <asp:HyperLink runat="server" ID="hlUploadFileAttachment" CssClass="fl" Target="_blank"></asp:HyperLink>
                                    <asp:ImageButton runat="server" ID="imgDeleteAttachment" CssClass="fl" ImageUrl="../../Documentation/images/icons/delete_icon1.gif" />
                                </div>
                            </div>
                            <div class="cb">
                            </div>
                        </div>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
    <div id="popup_add_reference" class="amc-popup">
        <asp:Panel runat="server" ID="panelPopup" DefaultButton="btnSave">
            <div style="overflow-y: scroll; height: 400px; float: left; width: 600px;">
                <div class="amc-error-message">
                    <asp:Label runat="server" ID="lblPopupMessage"></asp:Label>
                    <div>
                        <asp:ValidationSummary runat="server" ID="vldsPopupRequiredGroup" ValidationGroup="PopupRequiredGroup" />
                    </div>
                </div>
                <table>
                    <tr id="FirstName" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblFirstName"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtFirstName" CssClass="alpha width250"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqFirstName" ControlToValidate="txtFirstName"
                                EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="MiddleName" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblMiddleName"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMiddleName" CssClass="alpha width250"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqMiddleName" ControlToValidate="txtMiddleName"
                                EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="Name" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblName"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtName" CssClass="alpha width250"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqName" ControlToValidate="txtName"
                                EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="Title" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblTitle"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width250" ID="txtTitle"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqTitle" ControlToValidate="txtTitle"
                                EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="Degree" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblDegree"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width250" ID="txtDegree"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqDegree" ControlToValidate="txtDegree"
                                EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="Employer" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblEmployer"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width250" ID="txtEmployer"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqEmployer" ControlToValidate="txtEmployer"
                                EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="PrefContactMethod" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblPrefContactMethod"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlPrefContactMethod">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqPrefContactMethod"
                                ControlToValidate="ddlPrefContactMethod" EnableClientScript="True" Display="Dynamic"
                                ValidationGroup="PopupRequiredGroup">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="ContactClass" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblContactClass"></asp:Label>
                        </td>
                        <td>
                            <asp:RadioButtonList runat="server" ID="rdlContactClass" RepeatDirection="Vertical">
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr id="WorkAddress1" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblWorkAddress1"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width250" ID="txtWorkAddress1"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqWorkAddress1" ControlToValidate="txtWorkAddress1"
                                EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="WorkAddress2" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblWorkAddress2"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width250" ID="txtWorkAddress2"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqWorkAddress2" ControlToValidate="txtWorkAddress2"
                                EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="WorkAddress3" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblWorkAddress3"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width250" ID="txtWorkAddress3"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqWorkAddress3" ControlToValidate="txtWorkAddress3"
                                EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="WorkAddress4" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblWorkAddress4"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width250" ID="txtWorkAddress4"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqWorkAddress4" ControlToValidate="txtWorkAddress4"
                                EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="City" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblCity"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width250" ID="txtCity"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqCity" ControlToValidate="txtCity"
                                EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="State" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblState"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlWorkState">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqState" ControlToValidate="ddlWorkState"
                                EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="WorkZip" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblWorkZip"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width250" ID="txtWorkZip"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqWorkZip" ControlToValidate="txtWorkZip"
                                EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="WorkTelephone" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblWorkTelephone"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width250" ID="txtWorkTelephone"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqWorkTelephone"
                                ControlToValidate="txtWorkTelephone" EnableClientScript="True" Display="Dynamic"
                                ValidationGroup="PopupRequiredGroup">
                            </asp:RequiredFieldValidator>
                            <cc1:MaskedEditExtender runat="server" TargetControlID="txtWorkTelephone" ID="maskedWorkPhone"
                                Mask="999.999.9999" ClearMaskOnLostFocus="False" />
                            <div>
                                <cc1:MaskedEditValidator runat="server" ID="maskedWorkPhoneValidator" ControlToValidate="txtWorkTelephone"
                                    ControlExtender="maskedWorkPhone" ValidationExpression="^\d\d\d.\d\d\d.\d\d\d\d$|(___-___-____)"
                                    ValidationGroup="PopupRequiredGroup" IsValidEmpty="True" Display="Dynamic" />
                            </div>
                        </td>
                    </tr>
                    <tr id="WorkEmail" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblWorkEmail"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width250" ID="txtWorkEmail"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqWorkEmail" ControlToValidate="txtWorkEmail"
                                EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                            <div>
                                <asp:RegularExpressionValidator runat="server" ID="regexWorkEmailValid" ControlToValidate="txtWorkEmail"
                                    EnableClientScript="True" Display="Dynamic" resourcekey="EmailNotValid.Text"
                                    ValidationGroup="PopupRequiredGroup">
                                </asp:RegularExpressionValidator>
                            </div>
                        </td>
                    </tr>
                    <tr id="HomeAddress1" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblHomeAddress1"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width250" ID="txtHomeAddress1"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqHomeAddress1" ControlToValidate="txtHomeAddress1"
                                EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="HomeAddress2" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblHomeAddress2"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width250" ID="txtHomeAddress2"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqHomeAddress2" ControlToValidate="txtHomeAddress2"
                                EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="HomeAddress3" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblHomeAddress3"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width250" ID="txtHomeAddress3"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqHomeAddress3" ControlToValidate="txtHomeAddress3"
                                EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="HomeAddress4" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblHomeAddress4"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width250" ID="txtHomeAddress4"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqHomeAddress4" ControlToValidate="txtHomeAddress4"
                                ValidationGroup="PopupRequiredGroup" EnableClientScript="True" Display="Dynamic"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="HomeCity" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblHomeCity"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width250" ID="txtHomeCity"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqHomeCity" ControlToValidate="txtHomeCity"
                                EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="HomeState" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblHomeState"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlHomeState">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqHomeState" ControlToValidate="ddlHomeState"
                                EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="HomeZip" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblHomeZip"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width250" ID="txtHomeZip"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqHomeZip" ControlToValidate="txtHomeZip"
                                EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="HomeTelephone" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblHomeTelephone"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width250" ID="txtHomeTelephone"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqHomeTelephone"
                                ControlToValidate="txtHomeTelephone" EnableClientScript="True" Display="Dynamic"
                                ValidationGroup="PopupRequiredGroup">
                            </asp:RequiredFieldValidator>
                            <cc1:MaskedEditExtender runat="server" TargetControlID="txtHomeTelephone" ID="maskedHomePhone"
                                Mask="999.999.9999" ClearMaskOnLostFocus="False" />
                            <div>
                                <cc1:MaskedEditValidator runat="server" ID="maskedHomePhoneValidator" ControlToValidate="txtHomeTelephone"
                                    ControlExtender="maskedHomePhone" ValidationExpression="^\d\d\d.\d\d\d.\d\d\d\d$|(___-___-____)"
                                    ValidationGroup="PopupRequiredGroup" IsValidEmpty="True" Display="Dynamic" />
                            </div>
                        </td>
                    </tr>
                    <tr id="HomeEmail" runat="server">
                        <td>
                            <asp:Label runat="server" ID="lblHomeEmail"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="width250" ID="txtHomeEmail"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqHomeEmail" ControlToValidate="txtHomeEmail"
                                EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup">
                            </asp:RequiredFieldValidator>
                            <div>
                                <asp:RegularExpressionValidator runat="server" ID="regexHomeEmail" ControlToValidate="txtHomeEmail"
                                    EnableClientScript="True" Display="Dynamic" resourcekey="EmailNotValid.Text"
                                    ValidationGroup="PopupRequiredGroup">
                                </asp:RegularExpressionValidator>
                            </div>
                        </td>
                    </tr>
                    <tr id="UploadFileAttachment" runat="server">
                        <td>
                            <asp:Label ID="lblUploadFileAttachment" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:FileUpload ID="fuUploadFileAttachment" runat="server" CssClass="width250 fl" />
                            <%-- <asp:RequiredFieldValidator runat="server" ErrorMessage="*" ID="rqUploadFileAttachment" ControlToValidate="fuUploadFileAttachment"
                    EnableClientScript="True" Display="Dynamic" ValidationGroup="PopupRequiredGroup"></asp:RequiredFieldValidator>--%>
                            <asp:CustomValidator runat="server" ID="rqUploadFileAttachment" EnableClientScript="True"
                                Display="Dynamic" ValidationGroup="PopupRequiredGroup" CssClass="fl" ClientValidationFunction="ValidateUploadFile"
                                ControlToValidate="fuUploadFileAttachment" ValidateEmptyText="True" ErrorMessage="*">
                            </asp:CustomValidator>
                            <div class="cb">
                            </div>
                            <div>
                                <asp:RegularExpressionValidator ID="rqPdfFileExtension" ControlToValidate="fuUploadFileAttachment"
                                    ValidationExpression="^.*\.(pdf|PDF)$" runat="server" ValidationGroup="PopupRequiredGroup"
                                    resourcekey="RequirePDFFile.Text" Display="Dynamic" />
                            </div>
                            <br />
                            <div id="divUploadFileAttachment">
                                <asp:HyperLink runat="server" ID="hlUploadFileAttachment" CssClass="fl" Text="sample.pdf"
                                    Target="_blank"></asp:HyperLink>
                                <asp:ImageButton runat="server" ID="imgDeleteAttachment" CssClass="fl" ImageUrl="../../Documentation/images/icons/delete_icon1.gif" />
                                <asp:HiddenField runat="server" ID="hfDeleteFile" Value="NO" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="cb text-center">
                <asp:Button ID="btnSave" runat="server" Text="OK" ValidationGroup="PopupRequiredGroup" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
            </div>
        </asp:Panel>
    </div>
</div>
<div class="page-break">
</div>
