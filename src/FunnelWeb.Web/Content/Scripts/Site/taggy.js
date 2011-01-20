/// <reference path="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.4.4-vsdoc.js" />
$(function () {

    var tagCache = {};

    $("#tagInput").autocomplete({

        minLength: 0,
        source: function (request, response) {
            if (tagCache.hasOwnProperty(request.term)) {
                response(tagCache[request.term]);
            } else {
                $.getJSON('/tag/' + request.term, function (data) {
                    tagCache[request.term] = $.map(data, function (item) {
                        return { label: item.Name, value: item.Id, id: item.Id };
                    });
                    console.log(response);
                    response(tagCache[request.term]);
                });
            }
        },

        focus: function (event, ui) {
            $("#tagInput").val(ui.item.name);
            return false;
        },

        select: function (event, ui) {
            $('.newTagInputWrapper').before(
                    '<li class="newTagItem" id="container-' + ui.item.value + '"><a><em class="tagName">' + ui.item.label + '</em><span class="rm" id="' + ui.item.value + '"></span></a></li>'
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