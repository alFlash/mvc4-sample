var QuestionList = new Object();
jQuery(document).ready(function () {
    jQuery.each(jQuery('#question-list-configuration #tbl-question-list tr'), function (idx, questionRow) {
        var questionItem = jQuery(questionRow).find('#question-item');
        QuestionList.AttachQuestionItemEvent(questionItem);
        jQuery.each(jQuery(questionItem).find('#tbl-answer-list tr'), function (adx, answerRow) {

            QuestionList.AttachAnswerItemEvent(answerRow);
        });
    });

    jQuery('#addnew-question').on('click', function (event) {
        event.preventDefault();
        jQuery('#question-list-configuration #[id*=hdCurrentQuestionItem]').val('');
        jQuery('#question-list-configuration #[id*=hdCurrentAnswerItem]').val('');
        jQuery('#add-question-item #[id*=rdbQuestionType]').show();
        AmcCert.ShowPopUp('add-question-item', true);
        jQuery('#add-question-item input:text, #add-question-item textarea').val('');
        AmcCert.SetTitle('add-question-item', 'Add Question');
    });


    jQuery('#add-question-item #[id*=btnCancel], #add-answer-item #[id*=btnCancel]').live('click', function (event) {
        event.preventDefault();
        jQuery('#question-list-configuration #[id*=hdCurrentQuestionItem]').val('');
        jQuery('#question-list-configuration #[id*=hdCurrentAnswerItem]').val('');
        jQuery('#add-question-item input:text, #add-question-item textarea').val('');
        jQuery('#add-answer-item input:text, #add-answer-item textarea').val('');
        AmcCert.ShowPopUp('add-question-item', false);
        AmcCert.ShowPopUp('add-answer-item', false);
    });

    jQuery('#question-list-configuration .add-image,#question-list-configuration  .edit-image,#question-list-configuration  .delete-image').hover(
        function () {
            jQuery(this).css({ 'opacity': '1', 'filter': 'alpha(opacity=100)' });
        }, function () {
            jQuery(this).css({ 'opacity': '0.4', 'filter': 'alpha(opacity=40)' });
        });
    jQuery('#question-list-configuration .delete-image').on('click', function () {
        if (!confirm(ConfirmDeleteQuestion)) {
            return false;
        }
        return true;
    });

//    jQuery('#[id*=txtAnswerStartDate_txtDatetime]').live('change', function () {
//        var content = jQuery('#[id*=txtAnswerStartDate_txtDatetime]').val();
//        var inputDate = new Date(Date.parse(content));
//        var endDate = new Date(inputDate.addMonths(parseInt(RecertificationCircleValidilityMonths)));
//        var applicationDeadline = new Date(inputDate.addMonths(-3));
//        jQuery('#[id*=txtAnswerEndDate_txtDatetime]').val(endDate.toString(DateTimeFormat));
//        jQuery('#[id*=txtAnswerApplicationDeadline_txtDatetime]').val(applicationDeadline.toString(DateTimeFormat));
//    });
});

QuestionList.AttachAnswerItemEvent = function (answerRow) {
    var answerItem = jQuery(answerRow).find('#answer-hover-item');
    var editImage = jQuery(answerItem).find('.edit-image');
    editImage.attr('questionId', jQuery(answerItem).find('#[id*=hdAnswerQuestionId]').val());
    editImage.attr('answerId', jQuery(answerItem).find('#[id*=hdAnswerId]').val());
    editImage.attr('answerTypeString', jQuery(answerItem).find('#[id*=hdAnswerType]').val());
    editImage.attr('answerQuestionTypeString', jQuery(answerItem).find('#[id*=hdAnswerQuestionType]').val());

    jQuery(editImage).live('click', function () {
        jQuery('#question-list-configuration #[id*=hdCurrentQuestionItem]').val(editImage.attr('questionId'));
        jQuery('#question-list-configuration #[id*=hdCurrentAnswerItem]').val(editImage.attr('answerId'));
        jQuery('#question-list-configuration #[id*=hdCurrentAnswerTypeString]').val(editImage.attr('answerTypeString'));
        if (editImage.attr('answerTypeString') == 'YES' && (currentSurvey == certification_ExamChoice_Survey_Title || currentSurvey == recertification_Option_Survey_Title)) {
            jQuery('#add-answer-item').find('#[id*=txtAnswerText]').hide();
            jQuery('#add-answer-item #[id*=txtAnswerStartDate_txtDatetime]').val(jQuery(answerItem).find('#[id*=lblExamStartDate]').text());
            jQuery('#add-answer-item #[id*=txtAnswerEndDate_txtDatetime]').val(jQuery(answerItem).find('#[id*=lblExamEndDate]').text());
            jQuery('#add-answer-item #[id*=txtAnswerApplicationDeadline_txtDatetime]').val(jQuery(answerItem).find('#[id*=lblExamApplicationDeadline]').text());
            jQuery('#add-answer-item #[id*=txtProductCode]').val(jQuery(answerItem).find('#[id*=lblProductCode]').text());
            jQuery('#add-answer-item #[id*=txtApplicationProductId]').val(jQuery(answerItem).find('#[id*=lblApplicationProductId]').text());
            jQuery('#add-answer-item').find('#tblAnswerText').show();
            jQuery('#add-answer-item').find('#[id*=btnSaveAnswer]').hide();
            jQuery('#add-answer-item').find('#[id*=btnSaveExamChoiceAnswer]').show();
        } else {
            jQuery('#add-answer-item').find('#[id*=txtAnswerText]').show();
            jQuery('#add-answer-item').find('#[id*=btnSaveAnswer]').show();
            jQuery('#add-answer-item').find('#[id*=btnSaveExamChoiceAnswer]').hide();
            jQuery('#add-answer-item #[id*=txtAnswerText]').val(jQuery(answerItem).find('#[id*=lblAnswerText]').html());
            jQuery('#add-answer-item').find('#tblAnswerText').hide();

        }
        AmcCert.ShowPopUp('add-answer-item', true);
        //TODO:

        AmcCert.SetTitle('add-answer-item', 'Change Answer Text');
    });


    jQuery(answerRow).hover(function () {
        jQuery(answerRow).find('.add-image, .edit-image, .delete-image').show();
    }, function () {
        jQuery(answerRow).find('.add-image, .edit-image, .delete-image').hide();
    });
};

QuestionList.AttachQuestionItemEvent = function (questionItem) {
    var questionType = jQuery(questionItem).find('#[id*=hdQuestionType]').val();
    jQuery(questionItem).find('#[id*=hdAnswerQuestionType]').val(questionType);

    var editImage = jQuery(questionItem).find('#question-hover-item .edit-image');
    editImage.attr('questionId', jQuery(questionItem).find('#question-hover-item #[id*=hdQuestionId]').val());
    jQuery(editImage).live('click', function () {
        jQuery('#add-question-item #[id*=rdbQuestionType]').hide();
        jQuery('#question-list-configuration #[id*=hdCurrentQuestionItem]').val(editImage.attr('questionId'));
        jQuery('#question-list-configuration #[id*=hdCurrentAnswerItem]').val('');
        AmcCert.ShowPopUp('add-question-item', true);
        jQuery('#add-question-item #[id*=txtQuestionText]').val(jQuery(questionItem).find('#question-hover-item #[id*=lblQuestionText]').html());
        AmcCert.SetTitle('add-question-item', 'Change Question Text');
    });

    var addAnswerImg = jQuery(questionItem).find('#question-hover-item .add-image');
    addAnswerImg.attr('questionId', jQuery(questionItem).find('#question-hover-item #[id*=hdQuestionId]').val());
    addAnswerImg.attr('questionType', jQuery(questionItem).find('#question-hover-item #[id*=hdQuestionType]').val());
    jQuery(addAnswerImg).live('click', function () {
        jQuery('#question-list-configuration #[id*=hdCurrentQuestionItem]').val(addAnswerImg.attr('questionId'));
        jQuery('#question-list-configuration #[id*=hdCurrentAnswerItem]').val('');

        if (addAnswerImg.attr('questionType') == 'MULTI' && (currentSurvey == certification_ExamChoice_Survey_Title || currentSurvey == recertification_Option_Survey_Title)) {
            jQuery('#add-answer-item').find('#[id*=txtAnswerText]').hide();
            jQuery('#add-answer-item').find('#tblAnswerText').show();
            jQuery('#add-answer-item').find('#[id*=btnSaveAnswer]').hide();
            jQuery('#add-answer-item').find('#[id*=btnSaveExamChoiceAnswer]').show();
            jQuery('#question-list-configuration #[id*=hdCurrentAnswerTypeString]').val('YES');
        } else {
            jQuery('#add-answer-item').find('#[id*=txtAnswerText]').show();
            jQuery('#add-answer-item').find('#tblAnswerText').hide();
            jQuery('#add-answer-item').find('#[id*=btnSaveAnswer]').show();
            jQuery('#add-answer-item').find('#[id*=btnSaveExamChoiceAnswer]').hide();
            jQuery('#question-list-configuration #[id*=hdCurrentAnswerTypeString]').val('');
        }

        AmcCert.ShowPopUp('add-answer-item', true);
        jQuery('#add-answer-item input:text, #add-answer-item textarea').val('');
        AmcCert.SetTitle('add-answer-item', 'Add Answer');
    });

    var expandImg = jQuery(questionItem).find('#question-hover-item #[id*=questionexpand]');
    expandImg.attr('questionId', jQuery(questionItem).find('#question-hover-item #[id*=hdQuestionId]').val());
    var currentOpenQuestion = jQuery('#question-list-configuration #[id*=hdCurrentOpenQuestion]');
    var openAttribStr = '[' + expandImg.attr('questionId') + ']';
    expandImg.live('click', function () {
        jQuery(questionItem).find('#[id*=answerlist]').slideToggle(300);

        if (expandImg.css('background-image').indexOf('plus.gif') != -1) {//Collapsed
            if (currentOpenQuestion.val().indexOf(openAttribStr) == -1) {
                currentOpenQuestion.val(currentOpenQuestion.val() + openAttribStr);
            }
        } else { //Expanded
            currentOpenQuestion.val(currentOpenQuestion.val().replace(openAttribStr, ''));
        }
        expandImg.css('background-image', expandImg.css('background-image').indexOf('plus.gif') != -1 ?
            expandImg.css('background-image').replace('plus.gif', 'minus.gif') : expandImg.css('background-image').replace('minus.gif', 'plus.gif'));
    });

    if (currentOpenQuestion.val().indexOf(openAttribStr) != -1) {
        jQuery(questionItem).find('#[id*=answerlist]').slideDown(300);
        expandImg.css('background-image', expandImg.css('background-image').replace('plus.gif', 'minus.gif'));
    }

    jQuery(questionItem).find('#question-hover-item').hover(function () {
        jQuery(this).find('.add-image, .edit-image, .delete-image').show();
    }, function () {
        jQuery(this).find('.add-image, .edit-image, .delete-image').hide();
    });
};