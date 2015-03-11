$(function () {
    var converter = new Showdown.converter({ extensions: ['prettify'] });

    $("#wmd-preview").html(converter.makeHtml($('textarea#wmd-input')[0].value));

    $('textarea#wmd-input').bind('input propertychange', function () {
        $("#wmd-preview").html(converter.makeHtml(this.value));
    });
});
