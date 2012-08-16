var JsCommon = new Object();
/*
jQuery BlockUI: $.blockUI({ message: '<h1><img src="/images/busy.gif" class="padr10" /> Please wait...</h1>' });
jQuery Dialog: 
$('.<your-selector>').dialog({
autoOpen: false;
modal: true,
title: "<your pop-up title>",
open: function(event, ui) { ... },
beforeClose: function(event, ui) { ... },
close: function(event, ui) { ... }
});
jQuery Dialog event binding:
$( ".selector" ).bind( "dialogopen", function(event, ui) {...});
$( ".selector" ).bind( "dialogbeforeclose", function(event, ui) {...});
$( ".selector" ).bind( "dialogclose", function(event, ui) {...});
jQuery Dialog properties get/set:
//getter
var title = $( ".selector" ).dialog( "option", "title" );
//setter
$( ".selector" ).dialog( "option", "title", "Dialog Title" );
jQuery Dialog Methods:
.dialog( "close" )
.dialog( "isOpen" )
.dialog( "open" )

*/
$(document).ready(function () {
    JsCommon.InitializeJQueryPopup();

    JsCommon.RemoveDuplicate(".validation-summary-errors ul li");

    $("form").submit(function () {
        if (!$(this).valid()) {
            //form is not valid         
            JsCommon.RemoveDuplicate(".validation-summary-errors ul li");
        }
    });

    //    $.get("/Home/GetBaseEntities", function (data) {
    //        //JSON.parse(data);
    //        $('.jquery-popup').dialog("option", "title", "Get BaseEntities");
    //        $('.jquery-popup').dialog("open");
    //    });
    //TODO: KendoUI!!!
});

$(document).ready(function () {

});

JsCommon.RemoveDuplicate = function (selector) {
    var map = {};
    $(selector).each(function () {
        var value = $(this).text();
        if (map[value] == null) {
            map[value] = true;
        } else {
            $(this).remove();
        }
    });
};

JsCommon.InitializeJQueryPopup = function () {
    $('.jquery-popup').dialog({
        autoOpen: false,
        modal: true,
        draggable: false,
        resizable: false,
        height: 500,
        width: 600
    });
};

JsCommon.ConfirmDeleteConfiguration = function () {
    //return confirm(Resx_ConfirmDeleteConfiguration);
};

