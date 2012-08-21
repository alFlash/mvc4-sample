var RecertOption = new Object();
jQuery(document).ready(function () {
    var previousQuestionId = jQuery('#uc_recerfication_option #[id*=hdPreviousQuestionId]').val();
    var previousAnswerId = jQuery('#uc_recerfication_option #[id*=hdPreviousAnswerId]').val();
    var questionItemRows = jQuery('#uc_recerfication_option #tbl-recert-option-question tr');
    var answerItemRows = jQuery('#popup_exam_choice #tbl-recert-option-answer tr');
    RecertOption.AttacheAnswerItemRowEvent(answerItemRows, previousAnswerId, previousQuestionId);
    jQuery.each(questionItemRows, function (idx, questionItemRow) {
        var questionItem = jQuery(questionItemRow).find('#[id*=rdbRecertOptionQuestionItem]');
        var questionType = jQuery(questionItemRow).find('#[id*=hdQuestionType]').val();
        var questionId = jQuery(questionItemRow).find('#[id*=hdQuestionId]').val();
//        if (questionId == previousQuestionId) {
//            questionItem.prop('checked', true);
//        } else {
//            questionItem.prop('checked', false);
//        }
        jQuery(questionItem).on('click', function () {
            jQuery('#uc_recerfication_option #[id*=rdbRecertOptionQuestionItem]:checked').prop('checked', false);
            jQuery(questionItem).prop('checked', true);
            jQuery('#uc_recerfication_option #[id*=hdPreviousQuestionId]').val(questionId);
            //QuestionType = 'MULTI': show popup

            if (questionType == RECERT_OPTION_MULTI_QUESTIONTYPE) {
                //Filter Answer base on QuestionId

                RecertOption.AttacheAnswerItemRowEvent(answerItemRows, jQuery('#uc_recerfication_option #[id*=hdPreviousAnswerId]').val(), questionId);
                AmcCert.ShowPopUp('popup_exam_choice', true);
                AmcCert.SetTitle('popup_exam_choice', RECERT_OPTION_EXAM_CHOICE_POPUP_TITLE);

                var answerItems = jQuery('#popup_exam_choice #tbl-recert-option-answer tr #[id*=rdbRecertOptionAnswerItem]');
                if (jQuery('#popup_exam_choice #tbl-recert-option-answer tr #[id*=rdbRecertOptionAnswerItem]:checked').length <= 0) {
                    jQuery('#popup_exam_choice #tbl-recert-option-answer tr #[id*=rdbRecertOptionAnswerItem]:visible:first').prop('checked', true);
                }
                jQuery.each(answerItems, function (adx, answerItem) {
                    jQuery(answerItem).on('click', function () {
                        jQuery('#popup_exam_choice #tbl-recert-option-answer #[id*=rdbRecertOptionAnswerItem]:checked').prop('checked', false);
                        jQuery(answerItem).prop('checked', true);
                        jQuery('#uc_recerfication_option #[id*=hdPreviousAnswerId]').val(jQuery(answerItem).parent().find('#[id*=hdAnswerId]').val());
                    });
                });

                jQuery('#popup_exam_choice #[id*=btnExamChoiceOK]').on('click', function (args) {
                    args.preventDefault();
                    AmcCert.ShowPopUp('popup_exam_choice', false);
                });
            }
        });
    });

    //
    jQuery('#recertification-uc .amc-main-footer #[id*=btnSave]').click(function () {
        jQuery('#recertification-uc #[id*=hdCurrentReCertOptionCode]').val(jQuery('#uc_recerfication_option #tbl-recert-option-question #[id*=rdbRecertOptionQuestionItem]:checked').parent().find('#[id*=hdQuestionId]').val());
    });
    jQuery('#recertification-uc .amc-main-footer #[id*=btnNext]').click(function () {
        jQuery('#recertification-uc #[id*=hdCurrentReCertOptionCode]').val(jQuery('#uc_recerfication_option #tbl-recert-option-question #[id*=rdbRecertOptionQuestionItem]:checked').parent().find('#[id*=hdQuestionId]').val());
    });
});

RecertOption.AttacheAnswerItemRowEvent = function (rows, previousAnswerId, questionId) {
    jQuery.each(rows, function (ardx, answerItemRow) {
        var hiddenAnswerQuestionId = jQuery(answerItemRow).find('#[id*=hdQuestionId]');
        if (hiddenAnswerQuestionId.length > 0) {
            var answerQuestionId = hiddenAnswerQuestionId.val();
            if (answerQuestionId == questionId) {
                jQuery(answerItemRow).show();
            } else {
                jQuery(answerItemRow).hide();
            }
        }

//        var answerId = jQuery(answerItemRow).find('#[id*=hdAnswerId]').val();
//        if (answerId == previousAnswerId) {
//            jQuery(answerItemRow).find('#[id*=rdbRecertOptionAnswerItem]').prop('checked', true);
//        } else {
//            jQuery(answerItemRow).find('#[id*=rdbRecertOptionAnswerItem]').prop('checked', false);
//        }
    });
 }