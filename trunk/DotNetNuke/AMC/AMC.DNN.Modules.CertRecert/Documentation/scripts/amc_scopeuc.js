var ScopeUc = new Object();

jQuery(document).ready(function() {
    jQuery.each(jQuery("#scope-uc #tblscope tr"), function (idx, cesettingsRow) {
        jQuery(cesettingsRow).delegate(jQuery(cesettingsRow).find('#[id*=imgEdit]'), 'click', function (args) {
            args.stopPropagation();
            var scopeType = jQuery(cesettingsRow).find('#[id*=lblScopeTypeValue]').text();
            var selectedCptCode = jQuery(cesettingsRow).find('#[id*=lblCPTCodeValue]').text();
            ScopeUc.ScopeTypeDropDownClick(scopeType, selectedCptCode);
            ScopeUc.AttachScopeTypeDropdownClickEvent();
            ScopeUc.AttachCptCodeDropdownClickEvent();
        });
    });
    jQuery('#scope-uc').delegate(jQuery('#scope-uc').find('#add-new-row'), 'click', function (args) {
        args.stopPropagation();
        var scopeType = jQuery('#add-scope-popup #[id*=ddlScopetype]').val();
        ScopeUc.ScopeTypeDropDownClick(scopeType, null);
        ScopeUc.AttachScopeTypeDropdownClickEvent();
        ScopeUc.AttachCptCodeDropdownClickEvent();
    });
});

ScopeUc.AttachCptCodeDropdownClickEvent = function() {
    jQuery('#add-scope-popup #[id*=ddlCPTType]').click(function () {
        jQuery('#[id*=hdCurrentSelectedCptCode]').val(jQuery('#add-scope-popup #[id*=ddlCPTType]').val());
    });
};

ScopeUc.AttachScopeTypeDropdownClickEvent = function() {
    jQuery('#add-scope-popup #[id*=ddlScopetype]').click(function () {
        var scopeType = jQuery('#add-scope-popup #[id*=ddlScopetype]').val();
        ScopeUc.ScopeTypeDropDownClick(scopeType, null);
    });
};

ScopeUc.ScopeTypeDropDownClick = function (scopeType, cptCode) {
    jQuery('#[id*=hdCurrentSelectedScopeType]').val(scopeType);
    var proCode = jQuery('#add-scope-popup #[id*=ProCode]');
    var cPTCode = jQuery('#add-scope-popup #[id*=CPTCode]');
    var myVal = document.getElementById(rqProCodeClientID);
    if (scopeType == "PROCEDURES") {
        proCode.show();
        cPTCode.hide();
        if (myVal != null) {
            ValidatorEnable(myVal, true);
            jQuery(myVal).hide();
        }
    } else {
        proCode.hide();
        cPTCode.show();
        if (myVal != null) {
            ValidatorEnable(myVal, false);
            jQuery(myVal).hide();
        }
        if (ScopeUC_CPTCodeList != null) {
            for (var i = 0; i < ScopeUC_CPTCodeList.length; i++) {
                var currentCptCodeList = ScopeUC_CPTCodeList[i];
                if (currentCptCodeList.Code == scopeType) {
                    //TODO: build the list
                    jQuery('#add-scope-popup #[id*=ddlCPTType]').empty();
                    for (var j = 0; j < currentCptCodeList.CPTCodes.length; j++) {
                        var currentCptCode = currentCptCodeList.CPTCodes[j];
                        var item = '<option value="' + currentCptCode.Code + '">' + currentCptCode.Description + '</option>';
                        jQuery('#add-scope-popup #[id*=ddlCPTType]').append(item);
                    }
                    if (cptCode != '' && cptCode != null) {
                        jQuery('#add-scope-popup #[id*=ddlCPTType]').val(cptCode);
                        jQuery('#[id*=hdCurrentSelectedCptCode]').val(cptCode);
                    } else if (jQuery('#add-scope-popup #[id*=ddlCPTType]').length > 0) {
                        jQuery('#[id*=hdCurrentSelectedCptCode]').val(jQuery('#add-scope-popup #[id*=ddlCPTType]')[0].value);
                    }
                    break;
                }
            }
        }
    }
};