String.prototype.nl2br = function () {
    var text = escape(this);
    if (text.indexOf('%0D%0A') > -1) {
        re_nlchar = /%0D%0A/g;
    } else if (text.indexOf('%0A') > -1) {
        re_nlchar = /%0A/g;
    } else if (text.indexOf('%0D') > -1) {
        re_nlchar = /%0D/g;
    }
    else return unescape(text);
    return unescape(text.replace(re_nlchar, '<br />'));
};

String.prototype.trim = function () {
    return this.replace(/^\s+|\s+$/, '');
};

String.prototype.chop = function (limit) {
    return (this.length > limit) ? (this.substring(0, limit) + "...") : this;
};

String.prototype.escapeAngles = function () {
    return this.replace(/>/g, '&gt;').replace(/</g, '&lt;');
};

String.prototype.coalesce = function (defaultValue) {
    return this == "" ? defaultValue : this;
};

function refreshGravatar() {
    var value = $("#email").attr("value");
    if (value != null && value.length > 0) {
        value = value.toLowerCase().trim();
        var hash = hex_md5(value);
        var source = "http://www.gravatar.com/avatar/" + hash + ".jpg?size=50";
        $("#gravatarpreview").attr("src", source).show();
    } else {
        $("#gravatarpreview").hide();
    }
}

function enableGravatarLookup() {
    $("#email").blur(
        function () {
            refreshGravatar();
        });
}

function scrollToValidationFailure() {
    if ($(".validation-failure:first").size() > 0) {
        $.scrollTo(".validation-failure:first");
    }
}

function enablePrettyPrinting() {
    $("pre").each(
        function () {
            $(this).attr("class", "prettyprint");
        });
    prettyPrint();
}

function enableValidation() {
    $("form").each(
        function () {
            $(this).validate();
        });
}

function enablePrettyDates() {
    $("span.date").each(function () {
        var utc = $(this).html();
        var utcDate = new Date(Date.parse(utc));

        var utcDateValue = utcDate.getTime();
        var localTimeOffset = utcDate.getTimezoneOffset() * 60000;

        var localDate = new Date(utcDateValue - (localTimeOffset));

        var pretty = prettyDate(localDate);
        if (pretty == undefined) {
            var local = Date.Format(localDate, "dd MMM, yyyy");
            localTimeOffset / 360000;
            $(this).html(local);
        } else {
            $(this).html(pretty);
        }

    });
}

var changes = 0;
function enableChangeDetection() {
    $(".promptBeforeUnload input").click(function () {
        changes++;
    });

    $(".promptBeforeUnload textarea").click(function () {
        changes++;
    });

    $("input[type=submit]").click(function () {
        changes = 0;
    });

    window.onbeforeunload = function () {
        if (changes > 0) {
            return "Navigating away will lose the unsaved changes you have made.";
        }
    }
}

$.expr[':'].external = function (obj) {
    return !obj.href.match(/^mailto\:/)
            && !(obj.hostname == location.hostname)
};

function enableNewWindowLinks() {
    $(".content a:external")
        .not(".comment-author a")
        .addClass("new-window");

    $('a.new-window').click(function () {
        window.open(this.href);
        return false;
    });
}

function enableTextLengthCounters() {
    $("input.restricted-length")
        .each(function () {
            $(this).after("<span class='counter'>" + ($(this).attr("maxlength") - $(this).attr("value").length) + "</span>")
        })
        .keyup(
            function () {
                var counter = $(this).next(".counter");
                var remainder = $(this).attr("maxlength") - $(this).attr("value").length;
                if (remainder <= 5) counter.addClass("close");
                else counter.removeClass("close");
                counter.text(remainder);
            });
}

function hideFlashes() {
    $(".flash").delay(3000).fadeOut('slow');
}

$(function () {
    scrollToValidationFailure();
    enableGravatarLookup();
    refreshGravatar();
    enablePrettyPrinting();
    enableValidation();
    enablePrettyDates();
    enableNewWindowLinks();

    if ($("#wmd-input").size() > 0) {
        initializeWmd();
    }
    enableChangeDetection();
    enableTextLengthCounters();
});

