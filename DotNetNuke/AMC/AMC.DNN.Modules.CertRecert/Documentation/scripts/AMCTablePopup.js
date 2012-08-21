/* AMC - Field Id conventions:
- AddNewRowId (div): add-new-row
- Button Submit (in popup): btnOK
- Button Cancel (in Popup): btnCancel
- Image "Add Attachement": imgAddAttachment
- Image "Edit": imgEdit
- Image "Delete: imgDelete
- Hyperlink Document: hlAttachedDocument
*/
function AMCTablePopUp(bindingList, containerId, contentTableId,
popupId, addNewTitle, editTitle, templateRowId) {
    this.BindingList = bindingList;
    this.ContainerId = containerId;
    this.ParentTableId = contentTableId;
    this.PopupId = popupId;
    this.AddNewTitle = addNewTitle;
    this.EditTitle = editTitle;
    this.TemplateRowId = templateRowId;
    this.AddNewRowId = "add-new-row";
    this.ButtonSubmitId = "btnOK";
    this.ButtonCancelId = "btnCancel";
    this.ImageEdit = "imgEdit";
    this.ImageDelete = "imgItemDelete";
    this.AttachedDocumentHiddenId = "hdAttachedDocumentId";
    this.InitializeComponents();
}

AMCTablePopUp.CurrentObjectUniqueId = "hdCurrentObjectUniqueId";
AMCTablePopUp.AttachedDocumentNameHiddenId = 'hdAttachedDocumentName';
AMCTablePopUp.HyperlinkDocument = "hlAttachedDocumentName";
AMCTablePopUp.ImageAddAttachment = "imgAddAttachment";
AMCTablePopUp.CurrentAction = null; //0: Add, 1: edit
AMCTablePopUp.CurrentRow = null;
AMCTablePopUp.DivUploadFileAtachment = "divUploadFileAttachment";
AMCTablePopUp.CurrentDivUploadFile = null;

AMCTablePopUp.prototype.InitializeComponents = function () {

    var involmentuc = jQuery('#' + this.ContainerId);
    var addNewInvolvement = involmentuc.find('#[id*=' + this.AddNewRowId + ']');
    var tableOrganization = involmentuc.find('#' + this.ParentTableId);
    var involvementRows = tableOrganization.find('tr');
    var addInvolvementPopup = jQuery('#' + this.PopupId);
    var divUpload = jQuery(addInvolvementPopup).find('#' + AMCTablePopUp.DivUploadFileAtachment);
    if (divUpload != 'undefined') {
        divUpload.hide();
    }

    AMCTablePopUp.AttachAddNewRowButtonClickEvent(addNewInvolvement, this.PopupId, this.AddNewTitle);
    AMCTablePopUp.AttachRowEvent(involvementRows, addInvolvementPopup,
        AMCTablePopUp.ImageAddAttachment, AMCTablePopUp.HyperlinkDocument, AMCTablePopUp.AttachedDocumentNameHiddenId, //For Add New Attachement
        this.ImageEdit, this.ImageDelete, this.PopupId, this.EditTitle, this.BindingList);
    AMCTablePopUp.AttachPopUpSubmitButtonClickEvent(this.ButtonSubmitId, this.TemplateRowId, this.PopupId, addInvolvementPopup, involvementRows, tableOrganization, this.BindingList,
        AMCTablePopUp.ImageAddAttachment, AMCTablePopUp.AttachedDocumentNameHiddenId, AMCTablePopUp.HyperlinkDocument, this.ImageEdit, this.ImageDelete, this.EditTitle);
    AMCTablePopUp.AttachPopUpCancelButtonClickEvent(this.ButtonCancelId, this.PopupId, addInvolvementPopup);
    AMCTablePopUp.AttachDeleteDocumentEvent(addInvolvementPopup);

};

AMCTablePopUp.AttachPopUpCancelButtonClickEvent = function (btnCancelId, popupId, popup) {
    popup.find('#[id*=' + btnCancelId + ']').live('click', function (args) {
        args.preventDefault();

        jQuery('#[id*=hdCurrentSectionPopupOpenningId]').val('');
        jQuery('#[id*=lblPopupMessage]').text('');
        AmcCert.ShowPopUp(popupId, false);
        return false;
    });
};

AMCTablePopUp.ProcessDeleteFile = function (xmlhttp) {
    if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
        //delete file on server successfully
        if (xmlhttp.responseText.startsWith("1")) {
            if (typeof AMCTablePopUp.CurrentRow === 'undefined' || AMCTablePopUp.CurrentRow === null) {
                if (!IsStringNullOrEmpty(jQuery('[id*=hdCurrentObjectUniqueId]').val())) {
                    AMCTablePopUp.CurrentRow = jQuery('[guid=' + jQuery('[id*=hdCurrentObjectUniqueId]').val() + ']');
                }
            }
            if (AMCTablePopUp.CurrentRow !== null && jQuery(AMCTablePopUp.CurrentRow).length > 0) {
                var hlAttachedDocumentName = jQuery(AMCTablePopUp.CurrentRow).find('[id*=hlAttachedDocumentName]');
                if (IsStringNullOrEmpty(jQuery('#[id*=hdDeletedFileList]').val()) || (!IsStringNullOrEmpty(jQuery(AMCTablePopUp.CurrentRow).attr('guid')) && jQuery('#[id*=hdDeletedFileList]').val().indexOf(jQuery(AMCTablePopUp.CurrentRow).attr('guid')) == -1)) {
                    jQuery('#[id*=hdDeletedFileList]').val(jQuery('#[id*=hdDeletedFileList]').val() + '***-***' + jQuery(AMCTablePopUp.CurrentRow).attr('guid'));
                }
                hlAttachedDocumentName.hide();
                hlAttachedDocumentName.text('');
                hlAttachedDocumentName.attr('href', '');
                var imgAddAttachment = jQuery(AMCTablePopUp.CurrentRow).find('[id*=imgAddAttachment]');
                imgAddAttachment.show();
                var hdAttachedDocumentName = jQuery(AMCTablePopUp.CurrentRow).find('[id*=hdAttachedDocumentName]');
                hdAttachedDocumentName.val('');
            }
            jQuery(AMCTablePopUp.CurrentDivUploadFile).hide(200);
            jQuery(AMCTablePopUp.CurrentDivUploadFile).find('[id*=hlUploadFileAttachment]').text('');
            jQuery(AMCTablePopUp.CurrentDivUploadFile).find('[id*=hlUploadFileAttachment]').val('');
            jQuery(AMCTablePopUp.CurrentDivUploadFile).find('[id*=hlUploadFileAttachment]').attr('href', '');

        } else {
            jQuery('#[id*=imgDeleteAttachment]').show();
            alert(DeleteFileError);
        }
    }
};
//Author: Hai Nguyen
//Handle delete attachment click in popup
AMCTablePopUp.AttachDeleteDocumentEvent = function (popup) {
    popup.find('#[id*=imgDeleteAttachment]').live('click', function (args) {
        args.preventDefault();
        var divUploadOnPopup = jQuery(popup).find('#' + AMCTablePopUp.DivUploadFileAtachment);
        var hfDeleteFile = jQuery(popup).find('#[id*=hfDeleteFile]');
        var confirmDeleteFile = confirm(ConfirmDeleteFile);
        if (confirmDeleteFile) {
            var fileName = jQuery('#[id*=imgDeleteAttachment]').parents('#' + AMCTablePopUp.DivUploadFileAtachment).find('[id*=hlUploadFileAttachment]');
            if (fileName.length > 0) {
                jQuery('#[id*=imgDeleteAttachment]').hide();
                AMCTablePopUp.CurrentDivUploadFile = divUploadOnPopup;
                deleteDocument(fileName.attr('href'), AMCTablePopUp.ProcessDeleteFile);
            }
        }
        return false;
    });
};


AMCTablePopUp.AttachDeleteRowButtonClickEvent = function (imgDelete, row) {
    imgDelete.off('click');
    imgDelete.on('click', function (args) {
        if (!confirm(ConfirmDeleteRowText)) {
            args.preventDefault();
        } else {
            jQuery('#[id*=hdCurrentSectionPopupOpenningId]').val('');
            jQuery('#[id*=lblPopupMessage]').text('');
        }
    });
};

AMCTablePopUp.AttachPopUpSubmitButtonClickEvent = function (btnAddId, templateRowId, popupId, popup,
    rows, parentTable, bindingList, imgAddAttachmentId, attachedDocumentNameHiddenId, hlattachedDocId, btnEditRowId, btnDeleteRowId, popupTitle) {
    var btnSubmit = popup.find('#[id*=' + btnAddId + ']');
    btnSubmit.off('click');
    btnSubmit.live('click', function (args) {
        jQuery('#[id*=hdCurrentSectionPopupOpenningId]').val(popupId);
        jQuery('#[id*=hdCurrentSectionPopupOpenningTitle]').val(popupTitle);
        args.preventDefault();
        if (AMCTablePopUp.CurrentAction == 0) { //add
            var row = AMCTablePopUp.GetTemplate(templateRowId, rows);
            AMCTablePopUp.UpdateValue(row, popup, bindingList);
            //TODO: ATTACH ADD DOCUMENT EVENT
            parentTable.append(row);
            AMCTablePopUp.AttachDeleteRowButtonClickEvent(row.find('#[id*=' + btnDeleteRowId + ']'), row);
            AMCTablePopUp.AttachEditRowButtonClickEvent(row.find('#[id*=' + btnEditRowId + ']'), row, popup, popupId, popupTitle, bindingList);
            AMCTablePopUp.AttachAddDocumentButtonClickEvent(imgAddAttachmentId, hlattachedDocId, attachedDocumentNameHiddenId, row, popup, popupId, popupTitle, bindingList);
        } else if (AMCTablePopUp.CurrentAction == 1) { //edit
            if (AMCTablePopUp.CurrentRow != null) {
                AMCTablePopUp.UpdateValue(AMCTablePopUp.CurrentRow, popup, bindingList);
                var imgAddAttachement = jQuery(AMCTablePopUp.CurrentRow).find('#[id*=' + imgAddAttachmentId + ']');
                var hdattachedDocument = jQuery(AMCTablePopUp.CurrentRow).find('#[id*=' + attachedDocumentNameHiddenId + ']');
                var attachedDocument = jQuery(AMCTablePopUp.CurrentRow).find('#[id*=' + hlattachedDocId + ']');
                if (hdattachedDocument.val() != null && hdattachedDocument.val() != '') {
                    imgAddAttachement.hide();
                    attachedDocument.show();
                } else {
                    imgAddAttachement.show();
                    attachedDocument.hide();
                }
            }
        }
        AmcCert.ShowPopUp(popupId, false);
    });
};

AMCTablePopUp.GetTemplate = function (templateId, rows) {
    return jQuery('#' + templateId).length > 0 ? jQuery('#' + templateId).clone(true) : jQuery(rows[rows.length - 1]).clone(true);
};

AMCTablePopUp.AttachRowEvent = function (rows, popup,
        imageAddAttachmentId, hyperlinkDocumentId, hiddenDocumentId, imgEditId, imgDeleteId, popupId, popupTitle, bindingList) {
    var i = 0;
    var deletedFiles = jQuery('#[id*=hdDeletedFileList]').val();
    if (!IsStringNullOrEmpty(deletedFiles)) {
        var arrDeletedFiles = deletedFiles.split('***-***');
        if (arrDeletedFiles != null && arrDeletedFiles.length > 0) {
            for (var j = 0; j < arrDeletedFiles.length; j++) {
                var deletedFileRow = jQuery('#[guid= ' + arrDeletedFiles[j] + ']');
                deletedFileRow.find('#[id*=' + hyperlinkDocumentId + ']').text('');
                deletedFileRow.find('#[id*=' + hyperlinkDocumentId + ']').attr('href', '');
                deletedFileRow.find('#[id*=' + hiddenDocumentId + ']').val('');
            }
        }
    }
    var interval = setInterval(function () {
        if (i < rows.length) {
            var row = rows[i];


            var imgEdit = jQuery(row).find('#[id*=' + imgEditId + ']');
            var imgDelete = jQuery(row).find('#[id*=' + imgDeleteId + ']');
            AMCTablePopUp.AttachEditRowButtonClickEvent(imgEdit, row, popup, popupId,
                        popupTitle, bindingList);
            AMCTablePopUp.AttachDeleteRowButtonClickEvent(imgDelete, row);
            AMCTablePopUp.AttachAddDocumentButtonClickEvent(imageAddAttachmentId, hyperlinkDocumentId, hiddenDocumentId, row, popup, popupId,
                        popupTitle, bindingList);
            i++;
        } else {
            clearInterval(interval);
        }
    }, 300);
};

AMCTablePopUp.AttachAddDocumentButtonClickEvent = function (imgAddAttachmentId, hyperlinkDocumentId, hiddenDocumentId, row, popup, popupId, popupTitle, bindingList) {
    var imgAddAttachement = jQuery(row).find('#[id*=' + imgAddAttachmentId + ']');
    var hdattachedDocument = jQuery(row).find('#[id*=' + hiddenDocumentId + ']');
    var attachedDocument = jQuery(row).find('#[id*=' + hyperlinkDocumentId + ']');
    if (hdattachedDocument.val() != null && hdattachedDocument.val() != '') {
        imgAddAttachement.hide();
        attachedDocument.show();
    } else {
        attachedDocument.val('');
        imgAddAttachement.show();
        attachedDocument.hide();
    }
    imgAddAttachement.off('click');
    imgAddAttachement.on('click', function (args) {
        jQuery('#[id*=hdCurrentSectionPopupOpenningId]').val(popupId);
        jQuery('#[id*=hdCurrentSectionPopupOpenningTitle]').val(popupTitle);
        jQuery(row).find('#[id*=hlAttachedDocumentName]').val('');
        jQuery('#[id*=lblPopupMessage]').text('');
        args.preventDefault();
        jQuery('#[id*=lblPopupMessage]').text('');
        AMCTablePopUp.CurrentAction = 1;
        AMCTablePopUp.CurrentRow = row;
        AMCTablePopUp.FillData(jQuery(row), popup, bindingList);
        jQuery('#[id*=hdCurrentObjectUniqueId]').val(jQuery(row).find('#[id*=hdObjectUniqueId]').val());
        AmcCert.ShowPopUp(popupId, true);
        AmcCert.SetTitle(popupId, popupTitle);
    });
};


AMCTablePopUp.AttachEditRowButtonClickEvent = function (imgEdit, row, popup, popupId, popupTitle, bindingList) {
    imgEdit.off('click');
    imgEdit.on('click', function (args) {
        args.preventDefault();
        jQuery('#[id*=hdCurrentSectionPopupOpenningId]').val(popupId);
        jQuery('#[id*=hdCurrentSectionPopupOpenningTitle]').val(popupTitle);
        jQuery('#[id*=lblPopupMessage]').text('');
        AMCTablePopUp.CurrentAction = 1;
        AMCTablePopUp.CurrentRow = row;
        AMCTablePopUp.FillData(jQuery(row), popup, bindingList);
        jQuery('#[id*=hdCurrentObjectUniqueId]').val(jQuery(row).find('#[id*=hdObjectUniqueId]').val());

        var hfDeleteFile = jQuery(popup).find('#[id*=hfDeleteFile]');
        if (hfDeleteFile != 'undefined') {
            hfDeleteFile.val('NO');
        }

        //**********************/
        //author: Hai Nguyen
        //specific for board certification********************************/
        var lblBoardCertification = jQuery(row).find('#[id*=lblBoardCertification]');
        var txtBoardCertification = jQuery(popup).find('#[id*=txtBoardCertification]');
        if (lblBoardCertification != 'undefined' &&
                jQuery(lblBoardCertification).text() != 'undefined' &&
                jQuery(lblBoardCertification).text().length > 0) {
            jQuery(txtBoardCertification).val(jQuery(lblBoardCertification).text());
            jQuery(txtBoardCertification).parent("div").show();
        } else {
            if (txtBoardCertification != 'undefined') {
                jQuery(txtBoardCertification).parent("div").hide();
            }
        }
        //*************************************************************************/
        AmcCert.ShowPopUp(popupId, true);
        AmcCert.SetTitle(popupId, popupTitle);

        //author: Duy Truong
        /////// Set default values for Point (config in control panel) ARN only
        var hdnARN = jQuery('#' + popupId).find('#[id*=hdnARN]');
        var hdnPointValuesDefault = jQuery('#' + popupId).find('#[id*=hdnPointValuesDefault]');
        var txtPoint = jQuery('#' + popupId).find('#[id*=txtPoint]');
        if (hdnARN != 'undefined' && hdnPointValuesDefault != 'undefined' && txtPoint != 'undefined') {
            if (hdnARN.val() == 'true') {
                txtPoint.prop('readonly', true);
                txtPoint.on('keydown', function (e) {
                    if (e.keyCode == 8) {
                        e.preventDefault();
                        return false;
                    }
                    return true;
                });
                txtPoint.val(hdnPointValuesDefault.val());
            } else {
                txtPoint.prop('readonly', false);
                txtPoint.off('keydown');
            }
        }
    });
};

AMCTablePopUp.AttachAddNewRowButtonClickEvent = function (button, popupId, popupTitle) {
    button.off('click');
    button.on('click', function (evt) {
        evt.preventDefault();
        AMCTablePopUp.CurrentAction = 0;
        AmcCert.ClearPopUpContent(popupId, null);
        jQuery('#[id*=hdCurrentObjectUniqueId]').val('');
        jQuery('#[id*=hdCurrentSectionPopupOpenningId]').val(popupId);
        jQuery('#[id*=hdCurrentSectionPopupOpenningTitle]').val(popupTitle);
        jQuery('#[id*=lblPopupMessage]').text('');

        var popup = jQuery('#' + popupId);
        //hide div upload controls
        var divUploadOnPopup = jQuery(popup).find('#' + AMCTablePopUp.DivUploadFileAtachment);
        divUploadOnPopup.hide();
        AmcCert.ShowPopUp(popupId, true);
        AmcCert.SetTitle(popupId, popupTitle);
        //**********specific for board certification
        var txtBoardCertification = jQuery('#' + popupId).find('#[id*=txtBoardCertification]');
        if (txtBoardCertification != 'undefined') {
            txtBoardCertification.parent("div").hide();
            txtBoardCertification.val('');
        }
        //author: Duy Truong
        /////// Set default values for Point (config in control panel) ARN only
        var hdnARN = jQuery('#' + popupId).find('#[id*=hdnARN]');
        var hdnPointValuesDefault = jQuery('#' + popupId).find('#[id*=hdnPointValuesDefault]');
        var txtPoint = jQuery('#' + popupId).find('#[id*=txtPoint]');
        if (hdnARN != 'undefined' && hdnPointValuesDefault != 'undefined' && txtPoint != 'undefined') {
            if (hdnARN.val() == 'true') {
                txtPoint.prop('readonly', true);
                txtPoint.val(hdnPointValuesDefault.val());
                txtPoint.on('keydown', function (e) {
                    if (e.keyCode == 8) {
                        e.preventDefault();
                        return false;
                    }
                    return true;
                });
            } else {
                txtPoint.prop('readonly', false);
                txtPoint.off('keydown');
            }
        }

        //*************************
    });
};

AMCTablePopUp.UpdateValue = function (currentRow, popup, bindingList) {
    for (var i = 0; i <= bindingList.length - 1; i++) { //each Label
        var fromObject = jQuery(currentRow).find('#[id*=' + bindingList[i][0] + ']');
        var toObject = jQuery(popup).find('#[id*=' + bindingList[i][1] + ']');
        if (toObject == null || toObject.length <= 0) {
            toObject = toObject.find('input:text');
        }
        if (bindingList[i][0].indexOf('hdAttachedDocumentName') != -1) {
            fromObject.val(toObject.val());
        } else {
            fromObject.text(toObject.val());
        }
    }
};

AMCTablePopUp.FillData = function (currentRow, popup, bindingList) {
    for (var i = 0; i <= bindingList.length - 1; i++) { //each Label
        var fromObject = jQuery(currentRow).find('#[id*=' + bindingList[i][0] + ']');
        var toObject = jQuery(popup).find('#[id*=' + bindingList[i][1] + ']');
        if (toObject == null || toObject.length <= 0) {
            toObject = toObject.find('input:text');
        }
        if (bindingList[i][1].indexOf('fuUploadFileAttachment') == -1) {
            if (bindingList[i][0].indexOf('hdAttachedDocumentName') != -1) {
                toObject.val(fromObject.val());
            } else {
                toObject.val(fromObject.text());
            }
        }
        //Hai Nguyen
        //check if document exist, hide upload document div on popup
        if (bindingList[i][0].indexOf('hlAttachedDocumentName') != -1 &&
                bindingList[i][1].indexOf('fuUploadFileAttachment') == -1) {
            var divUploadOnPopup = jQuery(popup).find('#' + AMCTablePopUp.DivUploadFileAtachment);
            if (fromObject.text().length < 1) {
                divUploadOnPopup.hide();
            } else {
                divUploadOnPopup.show();
                var imgDeleteAttachmentIcon = jQuery(popup).find('#[id*=imgDeleteAttachmentIcon]');
                imgDeleteAttachmentIcon.show();
                toObject.text(fromObject.text());
                toObject.attr('href', fromObject.attr('href'));
            }
        }
    }
};
