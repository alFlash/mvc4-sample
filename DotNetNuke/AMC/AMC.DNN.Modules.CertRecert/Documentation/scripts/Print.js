jQuery(document).ready(function () {

    if (!(typeof isPrint === 'undefined')) {
        jQuery('#tbldnn_dnnSOLPARTMENU_ctldnnSOLPARTMENUMenuBar').remove();
        jQuery('#registration-uc #[id*=txtCurrentDate]').remove();
        jQuery('#registration-uc #[id*=txtHomeTelephone]').remove();
        jQuery('#registration-uc #[id*=txtWorkTelephone]').remove();
        //        if (typeof IsByPassPaymentProcess !== 'undefined' && IsByPassPaymentProcess !== null && IsByPassPaymentProcess.toLowerCase() == 'true') {
        //            jQuery('#supervisor-uc').remove();
        //            jQuery('#uc_eligibility').remove();
        //            jQuery('#registration-uc').remove();
        //        }
        //call renderLayoutForReferenceScreen
        //renderLayoutForReferenceScreen();
        //replace multiple choice
        jQuery('select').each(function () {
            var selectedVal = "";
            jQuery(this).children('option:selected').each(function () {
                if (selectedVal != "") {
                    selectedVal += ", ";
                }
                selectedVal += jQuery(this).text();
            });

            jQuery(this).replaceWith('<span>' + selectedVal + '</span>'); //p
        });

        //replace input tags
        jQuery('input').each(function () {
            if (jQuery(this).attr("type") == "text") {
                if (jQuery(this).attr("class") != "dn") {
                    jQuery(this).replaceWith('<span>' + this.value + '</span>'); //p
                }
                else {
                    jQuery(this).remove();
                }
            }
        });

        //replace textarea tags
        jQuery('textarea').each(function () {
            jQuery(this).replaceWith('<span>' + this.value + '</span>');//p
        });

        //replace upload tags
        jQuery('input').each(function () {
            switch (jQuery(this).attr("type")) {
                case "file":
                    jQuery(this).replaceWith('');
                    break;
                case "radio":
                    jQuery(this).attr('disabled', true);
                    break;
                case "checkbox":
                    jQuery(this).attr('disabled', true);
                    break;
            }
        });

        //remove edit & delete image
        jQuery('.edit-image, .delete-image, .add-image, #[id*=imgItemDelete], #[id*=imgEdit], #[id*=imgDeleteAttachment]').remove();

        //remove DNN un-use sections
        jQuery('#[id*=zoom_header], #[id*=zoom_mainmenu], #[id*=zoom_footer]').remove();

        //replace date time image
        jQuery('.img-datepicker').remove();

        //replace Add New
        jQuery('.amc-add-instruction').remove();

        //replace Add New
        jQuery('.amc-popup').remove();

        //replace Add New
        jQuery('#add-new-row').each(function () {
            jQuery(this).remove();
        });

        //remove control panel
        jQuery('table.ControlPanel').remove();

        //remove br tag
        jQuery('br').remove();
    }
});
function renderLayoutForReferenceScreen() {
    var tbl = document.getElementById('tblReference');
    if (tbl) {
        for (var i = 0; i < tbl.rows.length; i++) {
            if (i > 0) {
                var div = document.createElement('DIV');

                div.id = 'printing_div' + i;
                tbl.parentNode.appendChild(div);
                div.innerHTML = div.innerHTML + '<br><div><b>Reference# ' + i + '</b></div>';
                for (var j = 0; j < tbl.rows[i].cells.length; j++) {
                    var sp = tbl.rows[0].cells[j].getElementsByTagName('span')[0];
                    var sp2 = tbl.rows[i].cells[j].getElementsByTagName('span')[0];
                    if (sp && sp2) {
                        if (j != tbl.rows[i].cells.length - 1) {
                            div.innerHTML = div.innerHTML + "<table><tbody><tr><td style = 'width: 300px'>" + tbl.rows[0].cells[j].getElementsByTagName('span')[0].innerHTML + "</td>" + "<td>" + tbl.rows[i].cells[j].getElementsByTagName('span')[0].innerHTML + "</td></tr></tbody></table>";
                        }
                    }
                    else {
                        if (j != tbl.rows[i].cells.length - 1) {
                            div.innerHTML = div.innerHTML + "<table><tbody><tr><td style = 'width: 300px'>" + tbl.rows[0].cells[j].innerHTML + "</td>" + "<td>" + tbl.rows[i].cells[j].innerHTML + "</td></tr></tbody></table>";
                        }
                    }
                }
            }
        }
        tbl.parentNode.removeChild(tbl);
    }
}