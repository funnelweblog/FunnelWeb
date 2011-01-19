/// <reference path="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.4.4-vsdoc.js" />

(function () {

    jQuery.fn.taggy = function (options) {
        var that = this;
        var defaults = {
            url: '/tag/'
        };

        var tagCache = {};

        var opts = jQuery.extend(true, options, defaults);

        var load = function () {
            if (tagCache.hasOwnProperty(that.val())) {

            } else {
                jQuery.getJSON(opts.url + that.val(), null, function (result) {
                    tagCache[that.val()] = result;
                });
            }
        };

        var displayData = function () {
            console.log(tagCache[that.val()]);
        };

        that.keyup(function (e) {
            switch (e.which) {
                case 188:
                    //new tag
                    break;
                case 8:
                    if (that.val() !== '') {
                        load();
                    }
                    break;
                default:
                    load();
                    break;
            }
        });
    };
})();

jQuery('.taggy').taggy();