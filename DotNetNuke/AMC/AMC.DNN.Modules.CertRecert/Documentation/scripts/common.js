/* File Created: March 21, 2012 */
var AmcCert = new Object();

jQuery(document).ready(function () {
    //    remove 2 br tag
    jQuery(jQuery("#Content").find('br')[0]).remove();
    jQuery('#amc-settings').next().remove();
    AmcCert.TrackForChanges('certification-uc');
    AmcCert.TrackForChanges('recertification-uc');
    AmcCert.ApplyCSSForStepMenu();

    //re-check business error message
    if (jQuery('#[id*=hdCurrentSectionPopupOpenningId]').val() != '' && jQuery('#[id*=hdCurrentSectionPopupOpenningId]').val() != null
        && jQuery('#[id*=hdIsValidateFailed]').val() == '1') {
        AmcCert.ShowPopUp(jQuery('#[id*=hdCurrentSectionPopupOpenningId]').val(), true);
        AmcCert.SetTitle(jQuery('#[id*=hdCurrentSectionPopupOpenningId]').val(), jQuery('#[id*=hdCurrentSectionPopupOpenningTitle]').val());

        if (typeof attachHandleForDdlBoardCertification !== 'undefined' && attachHandleForDdlBoardCertification !== null && typeof attachHandleForCheckBoxNoneRecertDate !== 'undefined' && attachHandleForCheckBoxNoneRecertDate !== null) {
            attachHandleForDdlBoardCertification();
            attachHandleForCheckBoxNoneRecertDate();
        }
        
    }

    ValidatorValidate = ValidatorValidate_Override; //Validate IMMEDIATELY
    ValidatorCommonOnSubmit = ValidatorCommonOnSubmit_Override; //Validate only on SUBMIT ACTION
    ValidationSummaryOnSubmit = ValidationSummaryOnSubmit_Override; //Validation SUMMARY

    if (typeof PaymentProcessed !== 'undefined' && !IsStringNullOrEmpty(PaymentProcessed) && PaymentProcessed.toLowerCase() == 'true') {
        AmcCert.TriggerReadonlyMode();
    }
});

AmcCert.TriggerReadonlyMode = function () {
    jQuery('.amc-disabled-container').show();
    jQuery('.amc-disabled-container').css('height', jQuery('.amc-disabled-container').parent().height() + 'px');
    var certReCertModule = jQuery('#recertification-uc, #certification-uc, #apply-inactive-status-uc');
    certReCertModule.find('.amc-main-content').find('input:file, input:checkbox, input:text, input:submit, input:button, input:radio, select, option, textarea, img, input:image').prop('disabled', true);
    jQuery('.amc-popup').find('input:file, input:checkbox, input:text, input:submit, input:button, input:radio, select, option, textarea, img, input:image').prop('disabled', true);
    certReCertModule.find('.amc-main-footer').find('#[id*=btnBack], #[id*=btnSave], #[id*=btbSave], #[id*=btnNext], #[id*=btnSubmit]').prop('disabled', true);
};

window.onbeforeunload = function () {
    jQuery('#amc-certification-modules').find('input:file, input:checkbox, input:text, input:submit, input:button, input:radio, select, option, textarea, img, input:image').prop('disabled', true);
    jQuery('.amc-popup').find('input:file, input:checkbox, input:text, input:submit, input:button, input:radio, select, option, textarea, img, input:image').prop('disabled', true);
    return;
};

function IsStringNullOrEmpty(input) {
    if (typeof input === 'undefined' || input === null)
        return true;
    var regx = /^\s*$/; //create RegExp object for re-use
    return regx.test(input);
}


/*ASP.NET Default function*/
function ValidatorValidate_Override(val, validationGroup, event) {
    //Default of ASP.NET
    val.isvalid = true;
    if ((typeof (val.enabled) == "undefined" || val.enabled != false) && IsValidationGroupMatch(val, validationGroup)) {
        if (typeof (val.evaluationfunction) == "function") {
            val.isvalid = val.evaluationfunction(val);
            if (!val.isvalid && Page_InvalidControlToBeFocused == null &&
                            typeof (val.focusOnError) == "string" && val.focusOnError == "t") {
                ValidatorSetFocus(val, event);
            }
        }
    }
    ValidatorUpdateDisplay(val);

    /* Override - Highlight Control... */
    //    if (val && val.controltovalidate != null && val.controltovalidate != '') {
    //        if (val.isvalid)
    //            HightLightControlToValidate(jQuery('#' + val.controltovalidate), false /*val.isvalid => not hightlight*/);
    //        else
    //            HightLightControlToValidate(jQuery('#' + val.controltovalidate), true);
    //    }
}

/*ASP.NET Default function*/
function ValidatorCommonOnSubmit_Override() {
    Page_InvalidControlToBeFocused = null;
    var result = !Page_BlockSubmit;
    if ((typeof (window.event) != "undefined") && (window.event != null)) {
        window.event.returnValue = result;
    }
    Page_BlockSubmit = false;

    /* Override - Highlight Control... */
    //    if (Page_Validators && Page_Validators.length > 0) {
    //        for (var i = 0; i < Page_Validators.length; i++) {
    //            if (!Page_Validators[i].isvalid) {
    //                HightLightControlToValidate(jQuery('#' + Page_Validators[i].controltovalidate), true);
    //            }
    //        }
    //    }
    /* Override */

    return result;
}

function HightLightControlToValidate(jControl /*jQuery Object*/, isHightLight) {
    if (isHightLight) {
        if (typeof jControl.attr('class') == 'undefined' || jControl.attr('class').indexOf('amc-validate-error') == -1) {
            jControl.addClass('amc-validate-error');
        }
        //jControl.css('border', '1px solid red');
    } else {
        jControl.removeClass('amc-validate-error');
        //jControl.css('border', '1px solid gray');
    }
}

/*ASP.NET Default function*/
function ValidationSummaryOnSubmit_Override(validationGroup) {
    if (typeof (Page_ValidationSummaries) == "undefined")
        return;
    var summary, sums, s;
    for (sums = 0; sums < Page_ValidationSummaries.length; sums++) {
        summary = Page_ValidationSummaries[sums];
        summary.style.display = "none";
        if (!Page_IsValid && IsValidationGroupMatch(summary, validationGroup)) {
            var i;
            if (summary.showsummary != "False") {
                summary.style.display = "";
                /* Add custom function here */
                /*
                /* Add custom function here */

                if (typeof (summary.displaymode) != "string") {
                    summary.displaymode = "BulletList";
                }
                switch (summary.displaymode) {
                    case "List":
                        headerSep = "<br>";
                        first = "";
                        pre = "";
                        post = "<br>";
                        end = "";
                        break;
                    case "BulletList":
                    default:
                        headerSep = "";
                        first = "<ul>";
                        pre = "<li>";
                        post = "</li>";
                        end = "</ul>";
                        break;
                    case "SingleParagraph":
                        headerSep = " ";
                        first = "";
                        pre = "";
                        post = " ";
                        end = "<br>";
                        break;
                }
                s = "";
                if (typeof (summary.headertext) == "string") {
                    s += summary.headertext + headerSep;
                }
                s += first;
                for (i = 0; i < Page_Validators.length; i++) {
                    if (!Page_Validators[i].isvalid && (typeof (Page_Validators[i].errormessage) == "string")) {
                        //s += pre + Page_Validators[i].errormessage + post;
                        if (Page_Validators[i].errormessage == '*') {
                            if (s.indexOf(FieldIsRequiredString) == -1) {
                                s += FieldIsRequiredString + '<br/>';
                            }
                        }
                    }
                }
                if (s != first) {
                    s += end;
                    summary.innerHTML = s;
                } else {
                    summary.innerHTML = "";
                }
                window.scrollTo(0, 0);
            }
            if (summary.showmessagebox == "True") {
                s = "";
                if (typeof (summary.headertext) == "string") {
                    s += summary.headertext + "\r\n";
                }
                var lastValIndex = Page_Validators.length - 1;
                for (i = 0; i <= lastValIndex; i++) {
                    if (!Page_Validators[i].isvalid && typeof (Page_Validators[i].errormessage) == "string") {
                        switch (summary.displaymode) {
                            case "List":
                                s += Page_Validators[i].errormessage;
                                if (i < lastValIndex) {
                                    s += "\r\n";
                                }
                                break;
                            case "BulletList":
                            default:
                                s += "- " + Page_Validators[i].errormessage;
                                if (i < lastValIndex) {
                                    s += "\r\n";
                                }
                                break;
                            case "SingleParagraph":
                                s += Page_Validators[i].errormessage + " ";
                                break;
                        }
                    }
                }
                alert(s);
            }
        }
    }
}

AmcCert.ShowLoadingPopUp = function () {
    var loadingPanel = jQuery('form #loadingContainer');
    if (loadingPanel.length <= 0) {
        var loadingContainer = '<div id="loadingContainer" class="amc-loading-panel">' +
        '<div id="loadingContainerInner" style="margin:auto; width:30px; height:30px; top:50%; left:50%;position:absolute; background:url(\'' + ParentModulePath + '/Documentation/images/icons/loading.gif\') no-repeat"></div>' +
            '<div class="cover"></div>' +
        '</div>';
        jQuery('form').append(loadingContainer);
        AmcCert.AttachWindowScrollEvent(jQuery('#loadingContainer'), jQuery('#loadingContainer #loadingContainerInner'));
    } else {
        AmcCert.AttachWindowScrollEvent(loadingPanel, loadingPanel.find('#loadingContainerInner'));
    }
};

AmcCert.AttachWindowScrollEvent = function (loadingPanel /*jQuery Object*/, innerContainer) {
    jQuery(loadingPanel).find('.cover').css({ 'width': jQuery(window).width(), 'height': jQuery(window).height(), 'top': jQuery(window).scrollTop(), 'left': jQuery(window).scrollLeft() });
    loadingPanel.show();
    //var innerContainer = jQuery('#loadingContainer #loadingContainerInner');
    var contentTop = (jQuery(window).height() - innerContainer.outerHeight()) / 2 + jQuery(window).scrollTop();
    var contentLeft = (jQuery(window).width() - innerContainer.outerWidth()) / 2 + jQuery(window).scrollLeft();
    innerContainer.css({ 'top': contentTop, 'left': contentLeft, 'margin-top': '0px', 'margin-left': '0px' });

    jQuery(window).scroll(function () {
        loadingPanel.stop().css({ 'top': jQuery(window).scrollTop(), 'left': jQuery(window).scrollLeft() });
        loadingPanel.find('.cover').css({ 'top': jQuery(window).scrollTop(), 'left': jQuery(window).scrollLeft() });
        var scontentTop = (jQuery(window).height() - innerContainer.outerHeight()) / 2 + jQuery(window).scrollTop();
        var scontentLeft = (jQuery(window).width() - innerContainer.outerWidth()) / 2 + jQuery(window).scrollLeft();
        innerContainer.css({ 'top': scontentTop, 'left': scontentLeft, 'margin-top': '0px', 'margin-left': '0px' });
    });

    AmcCert.SetTabIndexScope(innerContainer);

};


AmcCert.SetTabIndexScope = function (scope /*jQuery Object*/) {
    //TabIndex - Loop
    var allInputs = scope.find('input:text, input:submit, input:button, input:radio, input:image, input:checkbox, img, textarea, select');
    for (var j = 0; j < allInputs.length; j++) {
        var currentItem = allInputs[j];
        if (j == 0) {
            jQuery(currentItem).focus();
        }
        jQuery(currentItem).attr('tabindex', j);
        jQuery(currentItem).keydown(function (args) {
            if (args.keyCode === 9) {
                args.preventDefault();
                if (parseInt(jQuery(this).attr('tabindex')) == allInputs.length - 1) {
                    jQuery(allInputs[0]).focus();
                } else {
                    jQuery(allInputs[parseInt(jQuery(this).attr('tabindex')) + 1]).focus();
                }
            }
        });
    }
};

AmcCert.HideLoadingPopUp = function () {
    jQuery('#loadingContainer').hide();
};

AmcCert.ApplyCSSForStepMenu = function () {
    var interval1 = setInterval(function () {
        if (typeof CertificationStepMenuClientId !== 'undefined') {
            AmcCert.ApplyIncompleteCSSForStepMenu(CertificationStepMenuClientId);
            clearInterval(interval1);
        } else if (typeof ReCertificationStepMenuClientId !== 'undefined') {
            AmcCert.ApplyIncompleteCSSForStepMenu(ReCertificationStepMenuClientId);
            clearInterval(interval1);
        }
    }, 1000);
};

AmcCert.ApplyIncompleteCSSForStepMenu = function (menuId) {
    var CertificationMenu = $find(menuId);
    if (CertificationMenu) {
        if (StepCompletedList != null && StepCompletedList != '') {
            var stepList = StepCompletedList.split(';');
            for (var j = 0; j < stepList.length; j++) {
                var optionList = stepList[j].split('|');
                var currentTabIndex = optionList[0];
                var isIncomplete = optionList[1];
                var incompleteTab = CertificationMenu.get_tabs().getTab(currentTabIndex).get_element();
                if (CertificationMenu.get_selectedTab().get_index() != currentTabIndex && CertificationMenu.get_tabs().getTab(currentTabIndex).get_enabled()) {
                    if (isIncomplete == '0') { //completed
                        jQuery(incompleteTab).find('a.rtsLink').addClass('finished-task');
                        jQuery(incompleteTab).find('a.rtsLink').removeClass('unfinished-task');
                        jQuery(incompleteTab).find('a.rtsLink').attr('title', 'Tab section completed');
                    } else if (isIncomplete == '1') { //incomplete
                        jQuery(incompleteTab).find('a.rtsLink').addClass('unfinished-task');
                        jQuery(incompleteTab).find('a.rtsLink').removeClass('finished-task');
                        jQuery(incompleteTab).find('a.rtsLink').attr('title', 'Tab section not completed');
                    }
                }
            }
        }
    }
};

AmcCert.TrackForChanges = function (usercontrolId) {
    var userControl = jQuery('#' + usercontrolId);
    var controls = userControl.find('input:text, input:radio, textarea, select, input:checkbox, input:file');
    jQuery.each(controls, function (idx, sender) {
        jQuery(sender).live('change', function () {
            userControl.find('#[id*=hdIsIncomplete]').val('1');
        });
    });
};

AmcCert.ClearPopUpContent = function (popupId, contentToClearId /* must be #[id*=xxxx]*/) {
    if (popupId.indexOf('#') == -1) popupId = '#' + popupId;
    jQuery(popupId + ' #[id*=vldsPopupRequiredGroup]').html('');
    if (contentToClearId == null || contentToClearId == '') {
        jQuery(popupId + ' input:text, ' + popupId + ' textarea').val('');
        jQuery.each(jQuery(popupId + ' select'), function (idx, selectObj) {
            var options = jQuery(selectObj).find('option');
            if (options.length > 0) {
                jQuery(selectObj).val(jQuery(options[0]).val());
            }
        });
    } else {
        jQuery(popupId + ' ' + contentToClearId).val('');
    }
};

AmcCert.SetTitle = function (popupId, title) {
    if (popupId.indexOf('#') == -1) {
        popupId = '#' + popupId;
    }
    jQuery(popupId).find('.amc-popup-con .amc-popup-title .amc-popup-title-text').text(title);
};

AmcCert.ShowPopUp = function (containerId, isShown) {
    if (containerId.indexOf('#') == -1) {
        containerId = '#' + containerId;
    }
    var contentId = containerId + " .amc-popup-con";
    if (isShown) {

        var children = jQuery(containerId).contents();
        if (jQuery(containerId).find('.amc-popup-con').length <= 0) {
            children.remove();
            jQuery(containerId).append('<div class="amc-popup-con"></div>');
            //Add title div
            jQuery(containerId).find('.amc-popup-con').append('<div class="amc-popup-title">' +
                '<div class="amc-popup-title-text">Title</div>' +
                '<div class="amc-popup-close-icon"></div>' +
                '<div class="cb"></div>' +
                '</div>');
            //add contents div
            jQuery(containerId).find('.amc-popup-con').append('<div class="amc-popup-contents"></div>');
            //add contents
            jQuery(containerId).find('.amc-popup-contents').append(children);
        }

        if (jQuery(containerId).find('.cover').length <= 0) {
            jQuery(containerId).append('<div class="cover"></div>');
        }
        var container = jQuery(containerId);
        container.remove();
        jQuery('form').append(container);
        container.find('.cover').css({ 'width': jQuery(window).width(), 'height': jQuery(window).height(), 'top': jQuery(window).scrollTop(), 'left': jQuery(window).scrollLeft() });
        if (typeof window.Page_Validators != 'undefined' && window.Page_Validators) {
            for (var i = 0; i < window.Page_Validators.length; i++) {
                jQuery(window.Page_Validators[i]).css({ 'display': 'none' });
            }
        }
        jQuery(containerId + ' #[id*=vldsPopupRequiredGroup]').html('');
        container.show();

        jQuery(contentId).find('.amc-popup-close-icon').live('click', function (args) {
            jQuery('#[id*=hdOpeningPopUp]').val('');
            jQuery('#[id*=hdCurrentPopUpTitle]').val('');
            jQuery('#[id*=hdCurrentSectionPopupOpenningId]').val('');
            container.fadeOut(300);
        });

        AmcCert.AttachWindowScrollEvent(container, jQuery(contentId));
    } else {
        jQuery(containerId).fadeOut(300);
    }
};
AmcCert.NewGuid = function () {
    var chars = '0123456789abcdef'.split('');

    var uuid = [], rnd = Math.random, r;
    uuid[8] = uuid[13] = uuid[18] = uuid[23] = '-';
    uuid[14] = '4'; // version 4

    for (var i = 0; i < 36; i++) {
        if (!uuid[i]) {
            r = 0 | rnd() * 16;

            uuid[i] = chars[(i == 19) ? (r & 0x3) | 0x8 : r & 0xf];
        }
    }

    return uuid.join('');
};

function ValidateReCertCircle(sender, args) {
    var inputDate = new Date(Date.parse(args.Value));
    if (typeof RecertificationCircleJson === 'undefined' || RecertificationCircleJson === null) {
        args.IsValid = true;
    } else {
        var startDate = new Date(parseInt(RecertificationCircleJson.StartDate.replace(/\/Date\(([-\d]+).*$/, "$1")));
        var expirationDate = new Date(parseInt(RecertificationCircleJson.ExpirationDate.replace(/\/Date\(([-\d]+).*$/, "$1")));
        sender.errormessage = sender.errormessage.replace('{0}', startDate.toString(DateTimeFormat));
        sender.errormessage = sender.errormessage.replace('{1}', expirationDate.toString(DateTimeFormat));
        jQuery('#' + sender.id).text(sender.errormessage);
        args.IsValid = (inputDate.clearTime() <= expirationDate.clearTime() && inputDate.clearTime() >= startDate.clearTime());
    }
}

//#region Validation function for upload control
function ValidateUploadFile(sender, args) {
    args.IsValid = false;
    var fuUploadFileAttachmentControl = jQuery(sender).parents('[id*=UploadFileAttachment]').find('[id*=fuUploadFileAttachment]');
    if (fuUploadFileAttachmentControl.val() != 'undefined' && fuUploadFileAttachmentControl.val() != '') {
        args.IsValid = true;
    } else {
        var hfDeleteFile = jQuery(sender).parents('[id*=UploadFileAttachment]').find('[id*=hfDeleteFile]');
        var hlUploadFileAttachmentCtl = jQuery(sender).parents('[id*=UploadFileAttachment]').find('[id*=hlUploadFileAttachment]');
        if (hfDeleteFile.val() != 'undefined') {
            if (hlUploadFileAttachmentCtl.val() == 'undefined' || hlUploadFileAttachmentCtl.val() == '') {
                args.IsValid = false;
            } else {
                args.IsValid = true;
            }
        } else {
            if (hlUploadFileAttachmentCtl.val() != 'undefined' && hlUploadFileAttachmentCtl.val() != '') {
                args.IsValid = true;
            }
        }
    }
}

function ValidateImportFile(sender, args) {
    args.IsValid = false;
    var fuImportFile = jQuery(sender).parent().find('[id*=fuImport]');
    if (fuImportFile.val() != 'undefined'  && fuImportFile.val() != '' && fuImportFile.val() != null) {
        args.IsValid = true;
    } else {
        jQuery('#' + sender.id).text('*');
    }
}
//#endregion 'Validation function for upload control'
CommonHelper = new Object();
CommonHelper.PhoneEmpty = '___.___.____';
CommonHelper.ClearPhoneEmpty = function (parent, textbox) {
    var txtControl = jQuery('#' + parent + ' #[id*=' + textbox + ']');
    if (txtControl.val() == CommonHelper.PhoneEmpty) {
        txtControl.val('');
    }
    jQuery('#' + parent).delegate('#[id*=' + textbox + ']', 'blur', function (ev) {
        ev.stopPropagation();
        if (txtControl.val() == CommonHelper.PhoneEmpty) {
            txtControl.val('');
        }
    });
};

//#region Trackchanges

//#endregion