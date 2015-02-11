$(function () {
    function enablePrettyPrinting() {
        $("pre").each(
        function () {
            $(this).attr("class", "prettyprint");
        });
        prettyPrint();
    }

    function enablePrettyDates() {
        $("span.date").each(function () {
            var utc = $(this).html();
            var utcDate = new Date(Date.parse(utc));

            var utcDateValue = utcDate.getTime();
            var localTimeOffset = utcDate.getTimezoneOffset() * 60000;

            var localDate = new Date(utcDateValue - (localTimeOffset));
            $(this).attr("title", Date.Format(localDate, "dd MMM, yyyy hh:nn"));

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
        $("input[type=text]:not(#q)").click(function () {
            changes++;
        });

        $("textarea").click(function () {
            changes++;
        });

        $("input[type=submit]").click(function () {
            changes = 0;
        });

        window.onbeforeunload = function () {
            if (changes > 0) {
                return "Navigating away will lose the unsaved changes you have made.";
            }
        };
    }

    $.expr[':'].external = function (obj) {
        return !(obj.href == "")
            && !obj.href.match(/^mailto\:/)
            && !(obj.hostname == location.hostname);
    };

    function enableNewWindowLinks() {
// ReSharper disable once CssNotResolved
        $(".content a:external")
        .not(".comment-author a")
        .addClass("new-window");

        $('a.new-window').click(function () {
            window.open(this.href);
            return false;
        });
    }
    
    function enableMarkdown() {
        if ($("#wmd-input").size() > 0) {
            var converter1 = Markdown.getSanitizingConverter();

            converter1.hooks.chain("preBlockGamut", function (text, rbg) {
                return text.replace(/^ {0,3}""" *\n((?:.*?\n)+?) {0,3}""" *$/gm, function (whole, inner) {
                    return "<blockquote>" + rbg(inner) + "</blockquote>\n";
                });
            });

            var editor1 = new Markdown.Editor(converter1);
            editor1.run();
        }
    }

    enablePrettyPrinting();
    enablePrettyDates();
    enableNewWindowLinks();
    enableChangeDetection();
    enableMarkdown();    
});