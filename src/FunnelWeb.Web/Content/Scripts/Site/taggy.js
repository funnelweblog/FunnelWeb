/// <reference path="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.4.4-vsdoc.js" />

(function () {

    jQuery.fn.taggy = function (options) {
        var that = this;
        var defaults = {
            url: '/tag/'
        };

        var tagCache = {};

        var opts = jQuery.extend(true, options, defaults);

        that.keyup(function (e) {
            switch (e.which) {
                case 188:
                    //new tag
                    break;
                default:
                    if (tagCache.hasOwnProperty(that.val())) {
                        console.log(tagCache[that.val()]);
                    } else {
                        jQuery.getJSON(opts.url + that.val(), null, function (result) {
                            console.log(result);
                        });
                    }
                    break;
            }
        });
    };
})();