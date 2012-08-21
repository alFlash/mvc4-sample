var ConfigurationForm = new Object();
ConfigurationForm.IsRichText = null;
jQuery(document).ready(function () {
    //alert(CurrentRichTextMode); //BASIC - RICH

    jQuery('#inactive-status-configuration #[id*=btnReset]').click(function () {
        if (!confirm(ConfirmResetData)) {
            return false;
        }
        return true;
    });

    jQuery('#form-configuration-popup #[id*=btnCancel]').live('click', function (args) {
        args.preventDefault();
        jQuery('#form-configuration-popup input:text, #form-configuration-popup textarea').val('');
        ConfigurationForm.HidePopUp();
    });
    jQuery('#form-configuration-popup #[id*=btnOK]').on('click', function (args) {
        ConfigurationForm.HidePopUp();
    });

    var allFormRows = jQuery('#inactive-status-configuration #tbl-form-configurations tr.form-item');

    var i = 0;
    var interval = setInterval(function () {
        if (i == 0) {
            AmcCert.ShowLoadingPopUp();
        }
        if (i < allFormRows.length) {
            var formRow = allFormRows[i];
            ConfigurationForm.AddFormEventHandlers(jQuery(formRow));
            i++;
        } else {
            clearInterval(interval);
            AmcCert.HideLoadingPopUp();
            //Enabled - Disabled all
            var enabledallchks = jQuery('input[id*=chkEnabledAllList]');
            jQuery.each(enabledallchks, function (idx, enalbedCheckbox) {
                jQuery(enalbedCheckbox).click(function () {
                    for (var j = 0; j < enabledallchks.length; j++) {
                        if (j != idx) {
                            jQuery(enabledallchks[j]).prop('checked', false);
                        }
                    }
                    if (jQuery(enalbedCheckbox).prop('checked')) {
                        ConfigurationForm.EnableAll(jQuery(enalbedCheckbox).parent().find('label[for*=chkEnabledAllList]').text() == 'Enable All');
                    }
                });
            });
            //ConfigurationForm.CheckEnabledAll();
        }
    }, 1000
    );
    ConfigurationForm.SetHoverAnimation();
    //ConfigurationForm.TrimText();
    ConfigurationForm.CheckIfPopUpIsOpened();

    jQuery('#inactive-status-configuration .configuration-footer #[id*=btnSave]').on('click', function () {
        if (typeof Page_IsValid !== 'undefined' && Page_IsValid == false) {
            if (Page_Validators != null && Page_Validators.length > 0) {
                for (var i = 0; i < Page_Validators.length; i++) {
                    if (!Page_Validators[i].isvalid) {
                        var validatedControl = document.getElementById(Page_Validators[i].controltovalidate);
                        var sectionDivs = jQuery('#[id*=divSectionConfiguration]');
                        var isFound = false;
                        for (var j = 0; j < sectionDivs.length; j++) {
                            var fieldDivs = jQuery(sectionDivs[j]).find('#[id*=divFieldConfiguration]');
                            for (var k = 0; k < fieldDivs.length; k++) {
                                var calculatorControls = jQuery(fieldDivs[k]).find('#[id*=txtCalculateFormular]');
                                for (var l = 0; l < calculatorControls.length; l++) {
                                    if (jQuery(calculatorControls[l]).attr('id') == jQuery(validatedControl).attr('id')) {
                                        var currentOpenFormObject = jQuery('#inactive-status-configuration #[id*=hdCurrentOpenFormId]');
                                        var formPrev = jQuery(sectionDivs[j]).prev();
                                        var formEditImage = formPrev.find('#[id*=imgEditForm]');
                                        var attribString1 = '[' + formEditImage.attr('FormId') + '***' + formEditImage.attr('SectionId') + ']';
                                        if (currentOpenFormObject.val().indexOf(attribString1) == -1)
                                            currentOpenFormObject.val(currentOpenFormObject.val() + attribString1);
                                        var formHover = formPrev.find('.hover-item #form-expand');
                                        formHover.css('background-image', formHover.css('background-image').replace('plus.gif', 'minus.gif'));
                                        jQuery(sectionDivs[j]).slideDown();
                                        var sectionPrev = jQuery(fieldDivs[k]).prev();
                                        var sectionEditImage = sectionPrev.find('#[id*=imgEditSection]');
                                        var currentOpenSectionObject = jQuery('#inactive-status-configuration #[id*=hdCurrentOpenSectionId]');
                                        var attribString = '[' + sectionEditImage.attr('FormId') + '***' + sectionEditImage.attr('SectionId') + ']';
                                        if (currentOpenSectionObject.val().indexOf(attribString) == -1)
                                            currentOpenSectionObject.val(currentOpenSectionObject.val() + attribString);
                                        var sectionHover = sectionPrev.find('.hover-item #section-expand');
                                        sectionHover.css('background-image', sectionHover.css('background-image').replace('plus.gif', 'minus.gif'));
                                        jQuery(fieldDivs[k]).slideDown();
                                        jQuery(validatedControl).parent().slideDown();
                                        jQuery(window).scrollTop(jQuery(validatedControl).position().top - 20);
                                        isFound = true;
                                        break;
                                    }
                                    if (isFound)
                                        break;
                                }
                                if (isFound)
                                    break;
                            }
                        }
                        break;
                    }
                }
            }
        }
    });

//    jQuery('#' + txtImportConfigurationsClientID).click(function () {
//        jQuery('#' + fuImportConfigurationsClientID).click();
//    });
//    jQuery('#' + fuImportConfigurationsClientID).change(function () {
//        jQuery('#' + txtImportConfigurationsClientID).val(jQuery('#' + fuImportConfigurationsClientID).val());
//    });
});

ConfigurationForm.CheckEnabledAll = function () {
    AmcCert.ShowLoadingPopUp();
    var formEnabledCheckboxes = jQuery('#tbl-form-configurations #[id*=chkFormIsEnabled]').length;
    var checkedForms = jQuery('#tbl-form-configurations #[id*=chkFormIsEnabled]:checked').length;
    var sectionEnabledCheckboxes = jQuery('#tbl-form-configurations #[id*=chkSectionIsEnabled]').length;
    var checkedSectins = jQuery('#tbl-form-configurations #[id*=chkSectionIsEnabled]:checked').length;
    var fieldEnabledCheckboxes = jQuery('#tbl-form-configurations #[id*=chkFieldIsEnable]').length;
    var checkedEnabledFields = jQuery('#tbl-form-configurations #[id*=chkFieldIsEnable]:checked').length;
    var fieldRequiredCheckboxes = jQuery('#tbl-form-configurations #[id*=chkFieldIsRequired]').length;
    var checkedRequiredFields = jQuery('#tbl-form-configurations #[id*=chkFieldIsRequired]:checked').length;
    if (formEnabledCheckboxes == checkedForms && sectionEnabledCheckboxes == checkedSectins && fieldEnabledCheckboxes == checkedEnabledFields && fieldRequiredCheckboxes == checkedRequiredFields ) {
        jQuery('label[for*=chkEnabledAllList]:contains("Enable All")').parent().find('input[id*=chkEnabledAllList]').prop('checked', true);
        jQuery('label[for*=chkEnabledAllList]:contains("Disable All")').parent().find('input[id*=chkEnabledAllList]').prop('checked', false);
    } else if (checkedForms == 0 && checkedSectins == 0 && checkedEnabledFields == 0 && checkedRequiredFields == 0) {
        jQuery('label[for*=chkEnabledAllList]:contains("Enable All")').parent().find('input[id*=chkEnabledAllList]').prop('checked', false);
        jQuery('label[for*=chkEnabledAllList]:contains("Disable All")').parent().find('input[id*=chkEnabledAllList]').prop('checked', true);
    } else {
        jQuery('label[for*=chkEnabledAllList]:contains("Enable All")').parent().find('input[id*=chkEnabledAllList]').prop('checked', false);
        jQuery('label[for*=chkEnabledAllList]:contains("Disable All")').parent().find('input[id*=chkEnabledAllList]').prop('checked', false);
    }
    AmcCert.HideLoadingPopUp();
};
ConfigurationForm.EnableAll = function (isEnabled) {
    var formEnabledCheckboxes = null;
    var sectionEnabledCheckboxes = null;
    var fieldEnabledCheckboxes = null;
    var fieldRequiredCheckboxes = null;
    AmcCert.ShowLoadingPopUp();
    var count = 0;
    var interval = setInterval(function () {
        if (count == 0) {
            formEnabledCheckboxes = jQuery('#tbl-form-configurations #[id*=chkFormIsEnabled]');
            for (var i = 0; i < formEnabledCheckboxes.length; i++) {
                var currentFormEnabledChk = jQuery(formEnabledCheckboxes[i]);
                currentFormEnabledChk.prop('checked', isEnabled);
                currentFormEnabledChk.trigger('click');
                currentFormEnabledChk.prop('checked', isEnabled);
            }
        } else if (count == 1) {
            sectionEnabledCheckboxes = jQuery('#tbl-form-configurations #[id*=chkSectionIsEnabled]');
            var sectionCount = 0;

            var sectionInterval = setInterval(function () {
                AmcCert.ShowLoadingPopUp();
                var currentSection = jQuery(sectionEnabledCheckboxes[sectionCount]);
                currentSection.prop('checked', isEnabled);
                currentSection.trigger('click');
                currentSection.prop('checked', isEnabled);
                if (sectionCount >= sectionEnabledCheckboxes.length) {
                    AmcCert.HideLoadingPopUp();
                    clearInterval(sectionInterval);
                }
                sectionCount++;
            }, 20);
        } else if (count == 2) {
            fieldEnabledCheckboxes = jQuery('#tbl-form-configurations #[id*=chkFieldIsEnable]');
            var fieldCount1 = 0;

            var fieldInterval1 = setInterval(function () {
                AmcCert.ShowLoadingPopUp();
                var currentField1 = jQuery(fieldEnabledCheckboxes[fieldCount1]);
                currentField1.prop('checked', isEnabled);
                currentField1.trigger('click');
                currentField1.prop('checked', isEnabled);
                if (fieldCount1 >= fieldEnabledCheckboxes.length) {
                    AmcCert.HideLoadingPopUp();
                    clearInterval(fieldInterval1);
                }
                fieldCount1++;
            }, 20);
        } else if (count == 3) {
            fieldRequiredCheckboxes = jQuery('#tbl-form-configurations #[id*=chkFieldIsRequired]');
            var fieldCount2 = 0;

            var fieldInterval2 = setInterval(function () {
                AmcCert.ShowLoadingPopUp();
                var currentField2 = jQuery(fieldRequiredCheckboxes[fieldCount2]);
                currentField2.prop('checked', isEnabled);
                currentField2.trigger('click');
                currentField2.prop('checked', isEnabled);
                if (fieldCount2 >= fieldRequiredCheckboxes.length) {
                    AmcCert.HideLoadingPopUp();
                    clearInterval(fieldInterval2);
                }
                fieldCount2++;
            }, 20);
        } else if (count >= 4) {
            if (!isEnabled) {
                jQuery('#tbl-form-configurations #[id*=divFieldConfiguration]').hide();
            }
            AmcCert.HideLoadingPopUp();
            clearInterval(interval);
        }
        count++;
    }, 100);
};

ConfigurationForm.OnBeforeUnload = function () {
    if (!jQuery('#form-configuration-popup').is(':visible')) {
        jQuery('#[id*=hdOpeningPopUp]').val('');
        jQuery('#[id*=hdCurrentPopUpTitle]').val('');
    }
    return;
};
window.onbeforeunload = ConfigurationForm.OnBeforeUnload;

ConfigurationForm.SetHoverAnimation = function () {
    jQuery.each(jQuery('.hover-item'), function (idx, sender) {
        jQuery(sender).hover(function () {
            jQuery(sender).find('.edit-image:first').show();
            jQuery(sender).find('.resequence:first').show();
            var isInstructionstr = jQuery(sender).find('#[id*=hdIsInstruction]').val();
            if (isInstructionstr) {
                var isInstruction = isInstructionstr.toLowerCase() == 'true';
                if (!isInstruction) {
                    jQuery(sender).css({ 'background-color': '#CCCCCC' });
                }
            }
        }, function () {
            jQuery(sender).find('.edit-image:first').hide();
            jQuery(sender).find('.resequence:first').hide();
            var isInstructionstr = jQuery(sender).find('#[id*=hdIsInstruction]').val();
            if (isInstructionstr) {
                var isInstruction = isInstructionstr.toLowerCase() == 'true';
                if (!isInstruction) {
                    jQuery(sender).css({ 'background-color': 'transparent' });
                }
            }
        });
    });
    jQuery.each(jQuery('.edit-image, .resequence'), function (idx, sender) {
        jQuery(sender).hover(function () {
            jQuery(sender).css({ 'opacity': '1', 'filter': 'alpha(opacity=100)' });
        }, function () {
            jQuery(sender).css({ 'opacity': '0.6', 'filter': 'alpha(opacity=60)' });
        });
    });
};

ConfigurationForm.CheckIfPopUpIsOpened = function () {
    if (jQuery('#[id*=hdOpeningPopUp]').val() == 'True') {
        if (jQuery('#inactive-status-configuration #[id*=hdCurrentItemIsRichTextMode]').val().toLowerCase() == 'true') {
            jQuery('#form-configuration-popup #configure-richtext').show();
            jQuery('#form-configuration-popup #configure-basictext').hide();
        } else {
            jQuery('#form-configuration-popup #configure-richtext').hide();
            jQuery('#form-configuration-popup #configure-basictext').show();
        }
        AmcCert.ShowPopUp('form-configuration-popup', true);
        AmcCert.SetTitle('form-configuration-popup', jQuery('#[id*=hdCurrentPopUpTitle]').val());
    } else {
        ConfigurationForm.HidePopUp();
    }
};

ConfigurationForm.TrimText = function () {
    jQuery.each(jQuery('.configuration-item-value'), function (idx, sender) {
        var textDiv = jQuery(sender).find('.configuration-item-text');
        if (textDiv.width() > 200) {
            textDiv.css({ 'width': '200px' });
            jQuery(sender).find('.more-text').show();
        }
    });
};

ConfigurationForm.HidePopUp = function () {
    AmcCert.ShowPopUp('form-configuration-popup', false);
    jQuery('#[id*=hdOpeningPopUp]').val('');
    jQuery('#[id*=hdCurrentPopUpTitle]').val('');
};

ConfigurationForm.ShowPopup = function (title, content) {
    jQuery('#form-configuration-popup input:text, #form-configuration-popup textarea').val('');
    jQuery('#form-configuration-popup #[id*=txtNewValue]').val('');

    if (jQuery('#inactive-status-configuration #[id*=hdCurrentItemIsRichTextMode]').val().toLowerCase() == 'true') {
        jQuery('#form-configuration-popup #configure-richtext').show();
        jQuery('#form-configuration-popup #configure-basictext').hide();
    } else {
        jQuery('#form-configuration-popup #configure-richtext').hide();
        jQuery('#form-configuration-popup #configure-basictext').show();
    }

    AmcCert.ShowPopUp('form-configuration-popup', true);
    AmcCert.SetTitle('form-configuration-popup', title);
    jQuery('#[id*=hdOpeningPopUp]').val('True');
    jQuery('#[id*=hdCurrentPopUpTitle]').val(title);
    if (jQuery('#inactive-status-configuration #[id*=hdCurrentItemIsRichTextMode]').val().toLowerCase() == 'true') {
        if (CurrentRichTextMode == 'RICH') {
            var interval = setInterval(function () {
                var iframebody = jQuery('#form-configuration-popup iframe#[id*=teMessage]').contents().find('iframe').contents().find('body');
                if (iframebody && iframebody.length > 0) {
                    iframebody.html(content);
                    jQuery('#form-configuration-popup #[id*=txtNewValue]').val(content);
                    jQuery('a#[id*=teMessage_cerefresh]').parent().parent().remove();
                    jQuery('#[id*=teMessage_pnlOption]').hide();

                    iframebody.keypress(function () {
                        jQuery('#form-configuration-popup #[id*=txtNewValue]').val(iframebody.html());
                    });

                    clearInterval(interval);
                }
            }, 500);
        } else {
            jQuery('#form-configuration-popup textarea#[id*=teMessage]').val(content);
            jQuery('#form-configuration-popup #[id*=txtNewValue]').val(content);
            jQuery('#form-configuration-popup textarea#[id*=teMessage]').keypress(function () {
                jQuery('#form-configuration-popup #[id*=txtNewValue]').val(jQuery('#form-configuration-popup textarea#[id*=teMessage]').val());
            });
        }
    } else {
        jQuery('#form-configuration-popup #[id*=txtNewValue]').val(content);
    }
};


ConfigurationForm.AttachEditImageEventClick = function (editImage, parentRow, popupTitle, formHiddentId, formId,
        sectionHiddenId, sectionId, currentObjectValue) {
    editImage.off('click');
    editImage.on('click', function () {
        if (parentRow.attr('enabled') == 1) {
            jQuery('#inactive-status-configuration #[id*=' + formHiddentId + ']').val(editImage.attr(formId));
            if (sectionId != null && sectionId != '') jQuery('#inactive-status-configuration #[id*=' + sectionHiddenId + ']').val(editImage.attr(sectionId));
            else jQuery('#inactive-status-configuration #[id*=' + sectionHiddenId + ']').val('');
            jQuery('#inactive-status-configuration #[id*=hdConfiguratonFieldId]').val('');
            jQuery('#inactive-status-configuration #[id*=hdCurrentItemIsRichTextMode]').val(editImage.attr('IsRichText'));
            ConfigurationForm.ShowPopup(popupTitle, currentObjectValue.val());
        }
    });
};

ConfigurationForm.AttachEnabledCheckboxEventClick = function (enableCheckbox, expandImage, content, parentRow,editImage, hdCurrentOpenId) {
    var currentOpenObject = jQuery('#inactive-status-configuration #[id*=' + hdCurrentOpenId + ']');
    var attribString = '[' + editImage.attr('FormId') + '***' + editImage.attr('SectionId') + ']';
    enableCheckbox.off('click');
    enableCheckbox.on('click', function () {
        if (enableCheckbox.prop("checked") == false) {
            content.slideUp(300);
            expandImage.css('background-image', expandImage.css('background-image').replace('minus.gif', 'plus.gif'));
            parentRow.attr('enabled', 0);
            parentRow.find('td:first').css({ 'opacity': '0.6', 'filter': 'alpha(opacity=60)' });
            jQuery('label[for*=chkEnabledAllList]:contains("Enable All")').parent().find('input[id*=chkEnabledAllList]').prop('checked', false);
            currentOpenObject.val(currentOpenObject.val().replace(attribString, ''));
        } else {
            parentRow.attr('enabled', 1);
            parentRow.find('td:first').css({ 'opacity': '1', 'filter': 'alpha(opacity=100)' });
            jQuery('label[for*=chkEnabledAllList]:contains("Disable All")').parent().find('input[id*=chkEnabledAllList]').prop('checked', false);
            if (currentOpenObject.val().indexOf(attribString) == -1)
                currentOpenObject.val(currentOpenObject.val() + attribString);
        }
        //ConfigurationForm.CheckEnabledAll();
    });

    if (enableCheckbox.prop("checked") == false) {
        parentRow.attr('enabled', 0);
        parentRow.find('td:first').css({ 'opacity': '0.6', 'filter': 'alpha(opacity=60)' });
        jQuery('label[for*=chkEnabledAllList]:contains("Enable All")').parent().find('input[id*=chkEnabledAllList]').prop('checked', false);
    } else {
        parentRow.attr('enabled', 1);
        jQuery('label[for*=chkEnabledAllList]:contains("Disable All")').parent().find('input[id*=chkEnabledAllList]').prop('checked', false);
    }
};

ConfigurationForm.AttachExpandImageEventClick = function (expandImage, editImage, formRow, sectionRow, hdCurrentOpenId, attribId, content) {
    var currentOpenObject = jQuery('#inactive-status-configuration #[id*=' + hdCurrentOpenId + ']');
    var attribString = '[' + editImage.attr('FormId') + '***' + editImage.attr('SectionId') + ']';
    var parentRow = sectionRow != null ? sectionRow : formRow;
    expandImage.off('click');
    expandImage.on('click', function () {
        if (parentRow.attr('enabled') == 1) {
            parentRow.find('td:first').css({ 'opacity': '1', 'filter': 'alpha(opacity=100)' });
            expandImage.css('background-image', expandImage.css('background-image').indexOf('plus.gif') != -1 ?
            expandImage.css('background-image').replace('plus.gif', 'minus.gif') : expandImage.css('background-image').replace('minus.gif', 'plus.gif'));
            content.slideToggle(300, function () {
                if (expandImage.attr('IsExpanded') != 'true') {
                    if (editImage.attr('SectionId') == '' || editImage.attr('SectionId') == null) {
                        var allSectonRows = jQuery(parentRow).find('#tbl-section-configuration tr.section-item');
                        var i = 0;
                        var sectionInterval = setInterval(function () {
                            if (i < allSectonRows.length) {
                                AmcCert.ShowLoadingPopUp();
                                var sRow = allSectonRows[i];
                                ConfigurationForm.AddSectionEventHandlers(jQuery(sRow), jQuery(parentRow));
                                i++;
                            } else {
                                AmcCert.HideLoadingPopUp();
                                clearInterval(sectionInterval);
                            }
                        }, 500);
                    } else {
                        var allFildRows = jQuery(parentRow).find('#tbl-field-configurations tr.field-item');
                        for (var j = 0; j < allFildRows.length; j++) {
                            var fRow = allFildRows[j];
                            ConfigurationForm.AddFieldEventHandlers(jQuery(fRow), jQuery(sectionRow), jQuery(formRow));
                        }
                    }
                    expandImage.attr('IsExpanded', 'true');
                }
            });
            //ConfigurationForm.TrimText();
            if (expandImage.css('background-image').indexOf('plus.gif') == -1) { //expanded
                if (currentOpenObject.val().indexOf(attribString) == -1)
                    currentOpenObject.val(currentOpenObject.val() + attribString);
            } else { //collapsed
                currentOpenObject.val(currentOpenObject.val().replace(attribString, ''));
            }
        }
    });

    if (jQuery('#inactive-status-configuration #[id*=' + hdCurrentOpenId + ']').val().indexOf(attribString) != -1) {
        content.slideDown(300, function () {
            expandImage.css('background-image', expandImage.css('background-image').replace('plus.gif', 'minus.gif'));
            var allSectionRows = jQuery(parentRow).find('#tbl-section-configuration tr.section-item');
            if (allSectionRows.length > 0) { //section
                var i = 0;
                var sectionInteval = setInterval(function () {
                    if (i < allSectionRows.length) {
                        AmcCert.ShowLoadingPopUp();
                        var sRow = allSectionRows[i];
                        ConfigurationForm.AddSectionEventHandlers(jQuery(sRow), jQuery(parentRow));
                        i++;
                    } else {
                        AmcCert.HideLoadingPopUp();
                        clearInterval(sectionInteval);
                    }
                }, 500);
            } else {//field
                var allFieldRows = jQuery(parentRow).find('#tbl-field-configurations tr.field-item');
                for (var j = 0; j < allFieldRows.length; j++) {
                    var fRow = allFieldRows[j];
                    ConfigurationForm.AddFieldEventHandlers(jQuery(fRow), jQuery(sectionRow), jQuery(formRow));
                }
            }
        });
    }
};

ConfigurationForm.AttachEventHandlers = function (formRow, sectionRow, contentId, editImageId, expandImageId, enabledCheckboxId, popupTitle, formConfigHiddenId, formAttrib,
    sectionConfigHiddenId, sectionAttrib, currenOpenHiddenId, currentOpenAttribId, currentValueId, hdCurrentSequenceId, btnMoveUpId, btnMoveDownId, itemClassName) {
    contentId = '#[id*=' + contentId.replace('#', '') + ']';
    editImageId = '#[id*=' + editImageId.replace('#', '') + ']';
    expandImageId = '#[id*=' + expandImageId.replace('#', '') + ']';
    enabledCheckboxId = '#[id*=' + enabledCheckboxId.replace('#', '') + ']';
    if (btnMoveUpId != null && btnMoveUpId != '') btnMoveUpId = '#[id*=' + btnMoveUpId.replace('#', '') + ']';
    if (btnMoveDownId != null && btnMoveDownId != '') btnMoveDownId = '#[id*=' + btnMoveDownId.replace('#', '') + ']';
    var cObjValue = '#[id*=' + currentValueId.replace('#', '') + ']';
    var parentRow = sectionRow != null ? sectionRow : formRow;
    var content = jQuery(parentRow).find(contentId);
    var editImage = jQuery(parentRow).find(editImageId);
    var expandImage = parentRow.find(expandImageId);
    var enableCheckbox = jQuery(parentRow).find(enabledCheckboxId);
    var btnMoveUp = btnMoveUpId != null && btnMoveUpId != '' ? jQuery(parentRow).find(btnMoveUpId) : null;
    var btnMoveDown = btnMoveDownId != null && btnMoveDownId != '' ? jQuery(parentRow).find(btnMoveDownId) : null;
    var i = 0;
    var interval = setInterval(function () {
        if (i < 4) {
            if (i == 0) {
                ConfigurationForm.AttachEditImageEventClick(editImage, parentRow, popupTitle, formConfigHiddenId,
                    formAttrib, sectionConfigHiddenId, sectionAttrib, parentRow.find(cObjValue));
            } else if (i == 1) {
                ConfigurationForm.AttachEnabledCheckboxEventClick(enableCheckbox, expandImage, content, parentRow, editImage, currenOpenHiddenId);
            } else if (i == 2) {
                ConfigurationForm.AttachExpandImageEventClick(expandImage, editImage, formRow, sectionRow, currenOpenHiddenId, currentOpenAttribId, content);
            } else if (i == 3) {
                ConfigurationForm.AttachSequenceButtonEventClick(parentRow, btnMoveUp, btnMoveDown, hdCurrentSequenceId, itemClassName); //Resequence
            }
            i++;
        } else {
            clearInterval(interval);
        }
    }, 300);
};

ConfigurationForm.AttachSequenceButtonEventClick = function (parentRow, btnMoveUp, btnMoveDown, hdCurrentSequenceId, itemClassName) {
    if (btnMoveUp != null) {
        btnMoveUp.on('click', function (args) {
            ConfigurationForm.SwapRow(parentRow.prev(), parentRow, hdCurrentSequenceId);
        });
    }
    if (btnMoveDown != null) {
        btnMoveDown.on('click', function (args) {
            ConfigurationForm.SwapRow(parentRow, parentRow.next(), hdCurrentSequenceId);
        });
    }
};

ConfigurationForm.Exchange = function (oRowI, oRowJ, oTable) {
    if (oRowI.rowIndex == oRowJ.rowIndex + 1) {
        oTable.insertBefore(oRowI, oRowJ);
    } else if (oRowJ.rowIndex == oRowI.rowIndex + 1) {
        oTable.insertBefore(oRowJ, oRowI);
    } else {
        var tmpNode = oTable.replaceChild(oRowI, oRowJ);
        if (typeof (oRowI) != "undefined") {
            oTable.insertBefore(tmpNode, oRowI);
        } else {
            oTable.appendChild(tmpNode);
        }
    }
};

ConfigurationForm.SwapRow = function (prev, next, hdCurrentSequenceId) {
    var previousSequence = prev.find('#[id*=' + hdCurrentSequenceId + ']').val();
    var currentSequence = next.find('#[id*=' + hdCurrentSequenceId + ']').val();
    prev.before(next);
    prev.find('#[id*=' + hdCurrentSequenceId + ']').val(currentSequence);
    next.find('#[id*=' + hdCurrentSequenceId + ']').val(previousSequence);
};

ConfigurationForm.AddFormEventHandlers = function (formrow) {
    ConfigurationForm.AttachEventHandlers(formrow, null, 'divSectionConfiguration', 'imgEditForm', 'form-expand', 'chkFormIsEnabled', 'Edit Form Value',
        'hdConfigurationFormId', 'FormId', null, null, 'hdCurrentOpenFormId', 'FormId', 'hdFormValue',
        '', '', '', '');

    var chkFormConfigurableCheckbox = jQuery(formrow).find('#[id*=chkIsFormConfigurable]');
    chkFormConfigurableCheckbox.on('click', function () {
        jQuery(formrow).find('#[id*=chkIsSectionConfigurable], #[id*=chkIsFieldConfigurable]').prop('checked', jQuery(this).prop('checked'));
    });
};

ConfigurationForm.AddSectionEventHandlers = function (sectionrow, formRow) {
    ConfigurationForm.AttachEventHandlers(formRow, sectionrow, 'divFieldConfiguration', 'imgEditSection', 'section-expand', 'chkSectionIsEnabled', 'Edit Section Value',
        'hdConfigurationFormId', 'FormId', 'hdConfigurationSectionId', 'SectionId', 'hdCurrentOpenSectionId', 'SectionId', 'hdSectionValue',
        'hdSectionCurrentSequence', 'section-moveup', 'section-movedown', 'section-item'); //Resequence

    var chkFormConfigurableCheckbox = jQuery(formRow).find('#[id*=chkIsFormConfigurable]');
    jQuery(sectionrow).find('#[id*=chkIsSectionConfigurable]').on('click', function () {
        if (jQuery(formRow).find('#[id*=chkIsSectionConfigurable]:checked').length > 0) {
            chkFormConfigurableCheckbox.prop('checked', true);
        }
        jQuery(sectionrow).find('#[id*=chkIsFieldConfigurable]').prop('checked', jQuery(this).prop('checked'));
    });

};

ConfigurationForm.AddFieldEventHandlers = function (fieldRow, sectionRow, formRow) {
    var hdIsInstruction = jQuery(fieldRow).find('#[id*=hdIsInstruction]');
    if (hdIsInstruction.val() && hdIsInstruction.val().toLowerCase() == 'true') {
        var headerRow = fieldRow.parent().find('tr').eq(0);
        headerRow.before(fieldRow);
        fieldRow.find('.configuration-item-text').css('width', '500px');
    }
    var editImage = jQuery(fieldRow).find('#[id*=imgEditField]');
    var fieldValue = fieldRow.find('#[id*=hdFieldValueItem]');
    editImage.off('click');
    editImage.on('click', function () {
        jQuery('#inactive-status-configuration #[id*=hdConfigurationFormId]').val(editImage.attr('FormId'));
        jQuery('#inactive-status-configuration #[id*=hdConfigurationSectionId]').val(editImage.attr('SectionId'));
        jQuery('#inactive-status-configuration #[id*=hdConfiguratonFieldId]').val(editImage.attr('FieldId'));
        jQuery('#inactive-status-configuration #[id*=hdCurrentItemIsRichTextMode]').val(editImage.attr('IsRichText'));
        ConfigurationForm.ShowPopup('Edit Field Value', fieldValue.val());
    });

    var isCalculate = jQuery(fieldRow).find('#[id*=hdIsCalculate]').val().toLowerCase() == 'true';
    if (isCalculate) {
        var formularContainer = jQuery(fieldRow).find('.configuration-item-value');
        var formularTextContainer = formularContainer.find('.configuration-item-text');

        if (formularTextContainer.attr('class').indexOf('calculator-field') == -1) {
            formularTextContainer.addClass('calculator-field');
        }
        formularTextContainer.hover(function () {
            formularTextContainer.css({ 'text-decoration': 'underline' });
        }, function () {
            formularTextContainer.css({ 'text-decoration': 'none' });
        });
        formularTextContainer.on('click', function () {
            fieldRow.find('#[id*=fieldcalculator]').slideToggle(300);
        });
    }

    var chkFormConfigurableCheckbox = jQuery(formRow).find('#[id*=chkIsFormConfigurable]');
    var chkSectionConfigurableCheckbox = jQuery(sectionRow).find('#[id*=chkIsSectionConfigurable]');
    jQuery(fieldRow).find('#[id*=chkIsFieldConfigurable]').on('click', function () {
        if (jQuery(sectionRow).find('#[id*=chkIsFieldConfigurable]:checked').length > 0) {
            chkSectionConfigurableCheckbox.prop('checked', true);
        }
        if (jQuery(formRow).find('#[id*=chkIsSectionConfigurable]:checked').length > 0) {
            chkFormConfigurableCheckbox.prop('checked', true);
        }
    });

    var isFieldConfigurable = jQuery(fieldRow).find('#[id*=hdIsFieldConfigurable]').val();
    var chkIsEnabledCheckbox = jQuery(fieldRow).find('#[id*=chkFieldIsEnable]');
    if (chkIsEnabledCheckbox.prop('checked') == false || (isFieldConfigurable.toLowerCase() == 'false' && IsSuperUser.toLowerCase() == 'false')) {
        jQuery(fieldRow).find('#[id*=chkFieldIsRequired]').prop('checked', false);
        jQuery(fieldRow).find('#[id*=chkFieldIsRequired]').prop('disabled', true);
    } else {
        jQuery(fieldRow).find('#[id*=chkFieldIsRequired]').prop('disabled', false);
    }
    chkIsEnabledCheckbox.on('click', function () {
        if (chkIsEnabledCheckbox.prop('checked') == false || (isFieldConfigurable.toLowerCase() == 'false' && IsSuperUser.toLowerCase() == 'false')) {
            jQuery(fieldRow).find('#[id*=chkFieldIsRequired]').prop('checked', false);
            jQuery(fieldRow).find('#[id*=chkFieldIsRequired]').prop('disabled', true);
        } else {
            jQuery(fieldRow).find('#[id*=chkFieldIsRequired]').prop('disabled', false);
        }
        //ConfigurationForm.CheckEnabledAll();
        if (chkIsEnabledCheckbox.prop('checked') == false) {
            jQuery('label[for*=chkEnabledAllList]:contains("Enable All")').parent().find('input[id*=chkEnabledAllList]').prop('checked', false);
        } else {
            jQuery('label[for*=chkEnabledAllList]:contains("Disable All")').parent().find('input[id*=chkEnabledAllList]').prop('checked', false);
        }
    });

    var chkIsRequire = jQuery(fieldRow).find('#[id*=chkFieldIsRequired]');
    chkIsRequire.on('click', function() {
        //ConfigurationForm.CheckEnabledAll();
        if (chkIsRequire.prop('checked') == false) {
            jQuery('label[for*=chkEnabledAllList]:contains("Enable All")').parent().find('input[id*=chkEnabledAllList]').prop('checked', false);
        } else {
            jQuery('label[for*=chkEnabledAllList]:contains("Disable All")').parent().find('input[id*=chkEnabledAllList]').prop('checked', false);
        }
    });

};


