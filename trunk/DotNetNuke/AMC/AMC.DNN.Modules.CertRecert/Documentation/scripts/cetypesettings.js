var CETypeSettings = new Object();
jQuery(document).ready(function () {
    var bindingList = [['lblCETypeString', 'ddlCEType'],
                           ['lblProgramTypeString', 'ddlProgramType'],
                           ['lblPublicationTypeString', 'ddlPublicationType'],
                            ['lblMinCE', 'txtMinCE'],
                            ['lblMaxCE', 'txtMaxCE'],
                           ['lblWeightText', 'txtCEWeight']];

    var cetypesettingPopup = new AMCTablePopUp(bindingList,
            "CE-type-settings-container",
            "CE-type-settings-table",
            "popup-addnew-cetype",
            "Add Information ",
            "Edit Item",
            null);

    jQuery.each(jQuery("#CE-type-settings-uc #CE-type-settings-table tr"), function (idx, cesettingsRow) {
        jQuery(cesettingsRow).delegate(jQuery(cesettingsRow).find('#[id*=imgEdit]'), 'click', function (args) {
            args.stopPropagation();
            jQuery('#popup-addnew-cetype #[id*=ddlCEType]').prop('disabled', true);
            jQuery('#popup-addnew-cetype #[id*=ddlProgramType]').prop('disabled', true);
            jQuery('#popup-addnew-cetype #[id*=lblCEWeightPopupMessage]').text('');
            var programTypeCode = jQuery(cesettingsRow).find('#[id*=lblProgramTypeString]').text();
            var selectedCETypeCode = jQuery(cesettingsRow).find('#[id*=CETypeString]').text();
            CETypeSettings.ProgramTypeDropDownClick(programTypeCode, selectedCETypeCode);
            CETypeSettings.AttachProgramTypeDropdownClickEvent();
            CETypeSettings.AttachCETypeDropdownClickEvent();
        });
    });

    jQuery('#CE-type-settings-uc #CE-type-settings-container').delegate(jQuery('#CE-type-settings-uc #CE-type-settings-container').find('#add-new-row'), 'click', function (args) {
        args.stopPropagation();
        jQuery('#popup-addnew-cetype #[id*=ddlCEType]').prop('disabled', false);
        jQuery('#popup-addnew-cetype #[id*=ddlProgramType]').prop('disabled', false);
        jQuery('#popup-addnew-cetype #[id*=lblCEWeightPopupMessage]').text('');

        //todo: 
        var programTypeCode = jQuery('#popup-addnew-cetype #[id*=ddlProgramType]').val();
        CETypeSettings.ProgramTypeDropDownClick(programTypeCode, null);
        CETypeSettings.AttachProgramTypeDropdownClickEvent();
        CETypeSettings.AttachCETypeDropdownClickEvent();
    });

    //re-check business error message
    if (jQuery('#[id*=hdCurrentSectionPopupOpenningId]').val() != '' && jQuery('#[id*=hdCurrentSectionPopupOpenningId]').val() != null) {
        AmcCert.ShowPopUp(jQuery('#[id*=hdCurrentSectionPopupOpenningId]').val(), true);
        AmcCert.SetTitle(jQuery('#[id*=hdCurrentSectionPopupOpenningId]').val(), jQuery('#[id*=hdCurrentSectionPopupOpenningTitle]').val());

        var programTypeCode = jQuery('#popup-addnew-cetype #[id*=ddlProgramType]').val();
        var ceTypeCode = jQuery('#[id*=hdCurrentSelectedCETypeCode]').val();
        CETypeSettings.ProgramTypeDropDownClick(programTypeCode, ceTypeCode);
        CETypeSettings.AttachProgramTypeDropdownClickEvent();
        CETypeSettings.AttachCETypeDropdownClickEvent();
    }

});

CETypeSettings.AttachCETypeDropdownClickEvent = function () {
    jQuery('#popup-addnew-cetype #[id*=ddlCEType]').click(function () {
        jQuery('#[id*=hdCurrentSelectedCETypeCode]').val(jQuery('#popup-addnew-cetype #[id*=ddlCEType]').val());
    });
};

CETypeSettings.AttachProgramTypeDropdownClickEvent = function () {
    jQuery('#popup-addnew-cetype #[id*=ddlProgramType]').click(function () {
        var programTypeCode = jQuery('#popup-addnew-cetype #[id*=ddlProgramType]').val();
        CETypeSettings.ProgramTypeDropDownClick(programTypeCode, null);
    });
};

CETypeSettings.ProgramTypeDropDownClick = function (programTypeCode, ceTypeCode) {
    jQuery('#[id*=hdCurrentSelectedProgramTypeCode]').val(programTypeCode);
    if (CEType_Setting_CETypesList != null) {
        for (var i = 0; i < CEType_Setting_CETypesList.length; i++) {
            var currentCETypeList = CEType_Setting_CETypesList[i];
            if (currentCETypeList.Code == programTypeCode) {
                //TODO: build the list
                jQuery('#popup-addnew-cetype #[id*=ddlCEType]').empty();
                for (var j = 0; j < currentCETypeList.CETypes.length; j++) {
                    var currentCEType = currentCETypeList.CETypes[j];
                    var item = '<option value="' + currentCEType.Code + '">' + currentCEType.Description + '</option>';
                    jQuery('#popup-addnew-cetype #[id*=ddlCEType]').append(item);
                }
                if (ceTypeCode != '' && ceTypeCode != null) {
                    jQuery('#popup-addnew-cetype #[id*=ddlCEType]').val(ceTypeCode);
                    jQuery('#[id*=hdCurrentSelectedCETypeCode]').val(ceTypeCode);
                } else if (jQuery('#popup-addnew-cetype #[id*=ddlCEType]').length > 0) {
                    jQuery('#[id*=hdCurrentSelectedCETypeCode]').val(jQuery('#popup-addnew-cetype #[id*=ddlCEType]')[0].value);
                }
                break;
            }
        }
    }
};
