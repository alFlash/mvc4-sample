OrganizationInvolvement = new Object();
OrganizationInvolvement.CurrentAction = null; //0: Add, 1: edit
OrganizationInvolvement.CurrentRow = null;
OrganizationInvolvement.BindingList = [['lblDateInvolvementContent', 'txtDate'], 
    ['lblTypeOfCEInvolvementContent', 'txtTypeOfCE'],
    ['lblTitleInvolvementContent', 'txtTitle'], 
    ['lblFullNameInvolvementContent', 'txtFullName'],
    ['lblNumberOfHoursInvolvementContent', 'txtNumberOfHours'],
    ['lblRoleContent', 'txtrRole']];
jQuery(document).ready(function () {
    OrganizationInvolvement.InitializeComponents('involvement-uc',
        'addnew-involvement', 
        'tblOrganiza',
        'add-involvement',
        "Add New Involvement",
        'Edit Involvement',
        'btnAdd',
        'btnCancel',
        'imgAddAttachment',
        'imgEdit',
        'imgDelete', 
        'template-row');
});

OrganizationInvolvement.InitializeComponents = function (userControlId, addNewRowId, parentTableId, popupId, addNewTitle, editTitle,
        btnAddRowId, btnCancelId, btnAddAttachementId, btnEditRowId, btnDeleteRowId, templateRowId) {
    var involmentuc = jQuery('#' + userControlId);
    var addNewInvolvement = involmentuc.find('#[id*='+ addNewRowId + ']');
    var tableOrganization = involmentuc.find('#' + parentTableId);
    var involvementRows = tableOrganization.find('tr');
    var addInvolvementPopup = jQuery('#' + popupId);
    OrganizationInvolvement.AttachAddNewRowButtonClickEvent(addNewInvolvement, popupId, addNewTitle);
    OrganizationInvolvement.AttachRowEvent(involvementRows, addInvolvementPopup, btnAddAttachementId, btnEditRowId, btnDeleteRowId, popupId, editTitle);
    OrganizationInvolvement.AttachPopUpSubmitButtonClickEvent(btnAddRowId, templateRowId, popupId, addInvolvementPopup,
        involvementRows, tableOrganization, OrganizationInvolvement.BindingList, btnEditRowId, btnDeleteRowId, editTitle);
    OrganizationInvolvement.AttachPopUpCancelButtonClickEvent(btnCancelId, popupId, addInvolvementPopup);
};

OrganizationInvolvement.AttachPopUpCancelButtonClickEvent = function (btnCancelId, popupId, popup) {
    popup.find('#[id*=' + btnCancelId + ']').on('click', function (args) {
        args.preventDefault();
        AmcCert.ShowPopUp(popupId, false);
    });
};

OrganizationInvolvement.AttachPopUpSubmitButtonClickEvent = function (btnAddId, templateRowId, popupId, popup,
    rows, parentTable, bindingList, btnEditRowId, btnDeleteRowId, popupTitle) {
    var btnSubmit = popup.find('#[id*=' + btnAddId + ']');
    btnSubmit.off('click');
    btnSubmit.live('click', function (args) {
        args.preventDefault();
        if (OrganizationInvolvement.CurrentAction == 0) { //add
            var row = OrganizationInvolvement.GetTemplate(templateRowId, rows);
            OrganizationInvolvement.UpdateValue(row, popup, bindingList);
            //TODO: ATTACH ADD DOCUMENT EVENT
            //OrganizationInvolvement.CurrentRow.remove();
            parentTable.append(row);
            OrganizationInvolvement.AttachDeleteRowButtonClickEvent(row.find('#[id*=' + btnDeleteRowId + ']'), row);
            OrganizationInvolvement.AttachEditRowButtonClickEvent(row.find('#[id*=' + btnEditRowId + ']'), row, popup, popupId, popupTitle, bindingList);
        } else if (OrganizationInvolvement.CurrentAction == 1) { //edit
            if (OrganizationInvolvement.CurrentRow != null) {
                OrganizationInvolvement.UpdateValue(OrganizationInvolvement.CurrentRow, popup, bindingList);
            }
        }
        AmcCert.ShowPopUp(popupId, false);
    });
};

OrganizationInvolvement.GetTemplate = function (templateId, rows) {
    return jQuery('#' + templateId).length > 0 ? jQuery('#' + templateId).clone(true) : jQuery(rows[rows.length - 1]).clone(true);
};

OrganizationInvolvement.AttachRowEvent = function (rows, popup, imgAddId, imgEditId, imgDeleteId, popupId, popupTitle) {
    jQuery.each(rows, function (edx, row) {
        //var imgAdd = jQuery(row).find('#[id*=' + imgAddId + ']');
        var imgEdit = jQuery(row).find('#[id*=' + imgEditId + ']');
        var imgDelete = jQuery(row).find('#[id*=' + imgDeleteId + ']');
        OrganizationInvolvement.AttachEditRowButtonClickEvent(imgEdit, row, popup, popupId,
            popupTitle, OrganizationInvolvement.BindingList);
        OrganizationInvolvement.AttachDeleteRowButtonClickEvent(imgDelete, row);
        //TODO: ADD IMGADD IMAGE EVENT
    });
};

OrganizationInvolvement.AttachDeleteRowButtonClickEvent = function (imgDelete, row) {
    imgDelete.off('click');
    imgDelete.on('click',function (args) {
        args.preventDefault();
        jQuery(row).remove();
    });
};

OrganizationInvolvement.AttachEditRowButtonClickEvent = function (imgEdit, row, popup, popupId, popupTitle, bindingList) {
    imgEdit.off('click');
    imgEdit.on('click', function (args) {
        args.preventDefault();
        OrganizationInvolvement.CurrentAction = 1;
        OrganizationInvolvement.CurrentRow = row;
        OrganizationInvolvement.FillData(jQuery(row), popup, bindingList);
        AmcCert.ShowPopUp(popupId, true);
        AmcCert.SetTitle(popupId, popupTitle);
    });
};

OrganizationInvolvement.AttachAddNewRowButtonClickEvent = function (button, popupId, popupTitle) {
    button.off('click');
    button.on('click',function (evt) {
        evt.preventDefault();
        OrganizationInvolvement.CurrentAction = 0;
        AmcCert.ClearPopUpContent(popupId, null);
        AmcCert.ShowPopUp(popupId, true);
        AmcCert.SetTitle(popupId, popupTitle);
    });
};

OrganizationInvolvement.UpdateValue = function (currentRow, popup, bindingList) {
    for (var i = 0; i <= bindingList.length - 1; i++) { //each Label
        jQuery(currentRow).find('#[id*=' + bindingList[i][0] + ']').
            text(jQuery(popup).find('#[id*=' + bindingList[i][1] + ']').val());
    }
};

OrganizationInvolvement.FillData = function (currentRow, popup, bindingList) {
    for (var i = 0; i <= bindingList.length - 1; i++) { //each Label
        jQuery(popup).find('#[id*=' + bindingList[i][1] + ']').
            val(jQuery(currentRow).find('#[id*=' + bindingList[i][0] + ']').text());
    }
};
