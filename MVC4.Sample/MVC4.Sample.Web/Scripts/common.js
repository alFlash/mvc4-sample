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
    $.get("/Home/GetBaseEntities", function (data) {
        //JSON.parse(data);
        $('.jquery-popup').dialog("option", "title", "Get BaseEntities");
        $('.jquery-popup').dialog("open");
    });
    //TODO: KendoUI!!!
});

JsCommon.InitializeJQueryPopup = function() {
    $('.jquery-popup').dialog({
        autoOpen: false,
        modal: true,
        draggable: false,
        resizable: false,
        height: 500,
        width: 600
    });
};

JsCommon.ConfirmDeleteConfiguration = function() {
    //return confirm(Resx_ConfirmDeleteConfiguration);
};

