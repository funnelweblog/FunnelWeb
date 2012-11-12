$(function () {
    
    var preview = $("#wmd-preview");
    
    if (preview.length) {

        function enablePrettyPrinting() {

            var doPrettyPrint = false;
            
            preview.children("pre").each(function () {
                $(this).attr("class", "prettyprint");
                doPrettyPrint = true;
            });
            
            if (doPrettyPrint) prettyPrint();
        }

        $('#apply-pretty-print').click(enablePrettyPrinting);
    }
});
