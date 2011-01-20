$(function () {

    var skillsDb = [
            { value: "Push", name: "Push" },
            { value: "Hit", name: "Hit" },
            { value: "Trap", name: "Trap" },
            { value: "Flick", name: "Flick" }
        ];

    $("#tagInput").autocomplete({

        minLength: 0,
        source: skillsDb,

        focus: function (event, ui) {
            $("#tagInput").val(ui.item.name);
            return false;
        },

        select: function (event, ui) {
            $('.newTagInputWrapper').before(
                    '<li class="newTagItem" id="container_' + ui.item.value + '"><a><em class="tagName">' + ui.item.name + '</em><span class="rm" id="' + ui.item.value + '"></span></a></li>'
                );
        },

        close: function (event, ui) {
            $("#tagInput").val("");
        },

        width: 200
    });


    $('#tags').click(function (event) {
        if ($(event.target).is('span')) {
            $('#container_' + event.target.id).remove();
        }


    });

    $('#tags').click(function (event) {
        $("#tagInput").focus();
    });
});