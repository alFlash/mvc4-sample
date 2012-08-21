var ExamChoiceUC = new Object();
jQuery(document).ready(function () {
    var allExamAdministrationChk = jQuery('#amc-exam-choice #tblExamChoice #[id*=rdbExamAdministration]');
    jQuery.each(allExamAdministrationChk, function (idx, sender) {
        jQuery(sender).live('click', function () {
            jQuery('#amc-exam-choice #tblExamChoice #[id*=rdbExamAdministration]:checked').prop('checked', false);
            jQuery(sender).prop('checked', true);
        });
    });
}); 