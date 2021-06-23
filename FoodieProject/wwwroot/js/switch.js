// ****************** Restaurants switch ***********************
$(function () {
    $('#restSwitchButton').click(function (e) {
        e.preventDefault();
        if ($("#cardDiv").is(":hidden")) {
            $("#cardDiv").show();
            $("#map").hide()
            $("#modeIndicator").html("Cards");
        }
        else {
            $("#cardDiv").hide();
            $("#map").show();
            $("#modeIndicator").html("Map");
        }

    });
});

// ****************** AveragePrice + Addresses switch ***********************
$(function () {
    $('#priceSwitchButton').click(function (e) {
        e.preventDefault();
        if ($("table").is(":hidden")) {
            $("table").show();
            $("#Area").hide()
            $("#modeIndicator").html("Table");

            if ($('#query').length) {
               // $('#query').val('');
                $('#query').submit();
                $('#query').show();
                $('#icon').show();

            }
        }
        else {
            $("table").hide();
            $("#Area").show();
            $("#modeIndicator").html("Graph");

            if ($('#query').length) {
                $('#query').hide();
                $('#icon').hide();
            }
        }

    });
});




