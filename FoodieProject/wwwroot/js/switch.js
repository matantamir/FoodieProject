// ****************** Average price switch ***********************
$(function () {
    $('#modeSwitch').click(function (e) {
        e.preventDefault();
        if ($("table").is(":hidden")) {
            $("table").show();
            $("#Area").hide()
            $("#modeIndicator").html("Table");
        }
        else {
            $("table").hide();
            $("#Area").show();
            $("#modeIndicator").html("Graph");
        }

    });
});



