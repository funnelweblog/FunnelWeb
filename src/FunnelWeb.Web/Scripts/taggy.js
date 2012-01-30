/// <reference path="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.4.4-vsdoc.js" />
$(function () {

    var tagCache = {};

    $("#tagInput").autocomplete({

        minLength: 0,
        source: function (request, response) {
            if (tagCache.hasOwnProperty(request.term)) {
                response(tagCache[request.term]);
            } else {
                $.getJSON(getApplicationPath() + 'tag/' + request.term, function (data) {
                    var termExists = false;
                    for (var i = 0, l = data.length; i < l; i++) {
                        if (data[i].Name === request.term) {
                            termExists = true;
                            break;
                        }
                    }

                    if (!termExists) data.push({ Name: request.term, Id: request.term });

                    tagCache[request.term] = $.map(data, function (item) {
                        return { label: item.Name, value: item.Id, id: item.Id };
                    });
                    if (window.console) {
                        console.log(response);
                    }

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
                    '<li class="newTagItem" id="container-' + ui.item.value + '"><a><em class="tagName">' + ui.item.label + '</em><span class="rm" id="' + ui.item.id + '"></span></a></li>'
                );

            var h = $('#tags input[type="hidden"]');
            h.val(h.val() + ',' + ui.item.id);
        },

        close: function (event, ui) {
            $("#tagInput").val("");
        },

        width: 200
    });

    $('span.rm').live('click', function () {
        var h = $('#tags input[type="hidden"]');
        h.val(h.val().replace(this.id, ''));
        $('#container-' + this.id).remove();
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

function getApplicationPath()
{
    return window.location.pathname.split('admin/wikiadmin')[0];
}