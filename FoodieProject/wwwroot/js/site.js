// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

window.addEventListener('DOMContentLoaded', event => {

    // Navbar shrink function
    var navbarShrink = function () {
        const navbarCollapsible = document.body.querySelector('#mainNav');
        if (!navbarCollapsible) {
            return;
        }
        if (window.scrollY === 0) {
            navbarCollapsible.classList.remove('navbar-shrink')
        } else {
            navbarCollapsible.classList.add('navbar-shrink')
        }

    };

    // Shrink the navbar 
    navbarShrink();

    // Shrink the navbar when page is scrolled
    document.addEventListener('scroll', navbarShrink);

    // Activate Bootstrap scrollspy on the main nav element
    const mainNav = document.body.querySelector('#mainNav');
    if (mainNav) {
        new bootstrap.ScrollSpy(document.body, {
            target: '#mainNav',
            offset: 74,
        });
    };

    // Collapse responsive navbar when toggler is visible
    const navbarToggler = document.body.querySelector('.navbar-toggler');
    const responsiveNavItems = [].slice.call(
        document.querySelectorAll('#navbarResponsive .nav-link')
    );
    responsiveNavItems.map(function (responsiveNavItem) {
        responsiveNavItem.addEventListener('click', () => {
            if (window.getComputedStyle(navbarToggler).display !== 'none') {
                navbarToggler.click();
            }
        });
    });

});


// ---------------------------------- Tweets -------------------------------------
function embedTweet(tweets)
{
    for (var i = 0; i < tweets.data.length; i++) {
        var template = $('#hidden-template').html();
        template = template.replaceAll("{id}", i);
        $('tbody').append(template);
    }

    for (var i = 0; i < tweets.data.length; i++) {
        twttr.widgets.createTweet(tweets.data[i].id, document.getElementById('tweet' + i), { theme: 'light' });
    }
}

// ---------------------------------- Google data ----------------------------------
function initializeCreate() {
    const map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: 32.082796, lng: 34.793785 },
        zoom: 15,
    });
    const input = document.getElementById("Name");
    const options = {
        componentRestrictions: { country: "il" }, // Restrict search to israel only.
        fields: ["place_id", "formatted_phone_number", "geometry", "name", "address_component", "price_level", "rating", "opening_hours"], // the place search will return this fields as the .getDetails function api.
        origin: map.getCenter(),
        strictBounds: false, // Does not strict the bounds of the search, false is by default but mentioned to make sure.
        types: ["establishment"], // Restricts search to only businesses.
    };
    const autocomplete = new google.maps.places.Autocomplete(input, options);
    autocomplete.addListener("place_changed", () => {
        const place = autocomplete.getPlace();
        if (!place.geometry || !place.geometry.location) {
            window.alert(place.name + " not found as a restaurant in google, manually fill is needed.");
            return;
        }

        // Starts filling the inputs using place details from google.
        if (place.formatted_phone_number) {
            document.getElementById("Phone").value = place.formatted_phone_number;
        }
        if (place.address_components[2]) {
            document.getElementById("Address_City").value = place.address_components[2].long_name;
        }
        if (place.address_components[1].types.includes("route") || place.address_components[1].types.include("street_address")) {
            document.getElementById("Address_Street").value = place.address_components[1].long_name;
        }
        if (place.address_components[0].types.includes("street_number")) {
            document.getElementById("Address_Number").value = place.address_components[0].long_name;
        }

        // Goes over the dollars and marks them while ignoring price_level 4.
        for (let i = 1; i <= place.price_level; i++) {
            if (i == 4) {
                continue;
            }
            document.getElementById("dollar" + i).checked = true;
        }
        // Goes over the stars till reaching the restaurant's rating (rounded) by google and marks the stars.
        for (let i = 1; i <= Math.round(place.rating); i++) {

            document.getElementById("star" + i).checked = true;
        }
        document.getElementById("Name").value = place.name;
        document.getElementById("Address_PlaceId").value = place.place_id;
        var openPeriods = 'No working hours';
        if (!!place.opening_hours.weekday_text) {
            openPeriods = place.opening_hours.weekday_text[6] + "\n";
            for (var i = 0; i < place.opening_hours.weekday_text.length - 1; i++) {
                openPeriods += place.opening_hours.weekday_text[i] + "\n";
            }

        }
        document.getElementById("About").value = openPeriods;
    });
}

// ---------------------------------- About rest ----------------------------------
function aboutToggle() {
    $("#restOpenStatus, #arrow").click(function () {
        $("#abouttext").slideToggle();
        if ($("#arrow").attr('class') == 'fas fa-chevron-right') {
            $("#arrow").attr('class', 'fas fa-chevron-down');
        }
        else {
            $("#arrow").attr('class', 'fas fa-chevron-right');
        }
    })
}

// ---------------------------------- Dish album ----------------------------------
function createAlbumContainer(dishCount) {

    for (var i = 1; i <= dishCount; i++) {
        $('#CSSgal_container').append('<s id="s' + i + '"></s>')
    }
}

// ---------------------------------- Restaurants map/cards switch ---------------------
$(function () {
    $('#restSwitchButton').on("click", function (e) {
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

// ---------------------------------- AveragePrice + Addresses graph/table switch -------------------
$(function () {
    $('#priceSwitchButton').click(function (e) {
        e.preventDefault();
        if ($("table").is(":hidden")) {
            $("table").show();
            $("#Area").hide()
            $("#modeIndicator").html("Table");

            if ($('#query').length) {
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


//  ------------------------------ Statistics Graphs ---------------------------------------------------
function responsivefy(svg) {
    // get container + svg aspect ratio
    var container = d3.select(svg.node().parentNode),
        width = parseInt(svg.style("width")),
        height = parseInt(svg.style("height")),
        aspect = width / height;

    // add viewBox and preserveAspectRatio properties,
    // and call resize so that svg resizes on inital page load
    svg.attr("viewBox", "0 0 " + width + " " + height)
        .attr("preserveAspectRatio", "xMinYMid")
        .call(resize);

    // to register multiple listeners for same event type,
    // you need to add namespace, i.e., 'click.foo'
    // necessary if you call invoke this function for multiple svgs
    // api docs: https://github.com/mbostock/d3/wiki/Selections#on
    d3.select(window).on("resize." + container.attr("id"), resize);

    // get width of container and resize svg to fit it
    function resize() {
        var targetWidth = parseInt(container.style("width"));
        if (targetWidth < 700) {
            svg.attr("width", targetWidth);
            svg.attr("height", Math.round(targetWidth / aspect));
        }
    }

}
// Parse the Data

function build(data, width, height, max, svg) {

    // X axis
    var x = d3.scaleBand()
        .range([0, width])
        .domain(data.map(function (d) { return d.name; }))
        .padding(0.2);
    svg.append("g")
        .attr("transform", "translate(0," + height + ")")
        .call(d3.axisBottom(x))
        .selectAll("text")
        .attr("transform", "translate(-10,0)rotate(-45)")
        .style("text-anchor", "end");

    // Add Y axis
    var y = d3.scaleLinear()
        .domain([0, max * 1.2])
        .range([height, 0]);
    svg.append("g")
        .call(d3.axisLeft(y));

    // Bars
    svg.selectAll("mybar")
        .data(data)
        .enter()
        .append("rect")
        .attr("x", function (d) { return x(d.name); })
        .attr("y", function (d) { return y(d.value); })
        .attr("width", x.bandwidth())
        .attr("height", function (d) { return height - y(d.value); })
        .attr("fill", "#F5F3F0")

}

function createGraphInput(tableName, nameIndex, valueIndex)
{
    var elements = [];
    var table = document.getElementById(tableName);

    for (var i = 1; i < table.rows.length; i++) {
        var currCity = table.rows[i].cells[nameIndex].innerHTML;
        var flag = false;
        var pos = -1;


        // Check if the city is already exists in the element object
        for (var j = 0; j < elements.length; j++) {
            if (elements[j].name == currCity && flag == false) {
                pos = j;
                flag = true;
            }
        }

        if (pos != -1) {
            elements[pos].value++
        }
        else {
            elements.push({ "name": table.rows[i].cells[nameIndex].innerHTML, "value": valueIndex == null ? 1 : table.rows[i].cells[valueIndex].innerHTML });
        }
    }

    // Get maximum and minumum value
    var min = 999
    var max = 0
    for (var i = 0; i < elements.length; i++) {
        if (parseInt(elements[i].value) > parseInt(max)) { max = parseInt(elements[i].value); }
        if (parseInt(elements[i].value) < parseInt(min)) { min = parseInt(elements[i].value); }
    }

    // set the dimensions and margins of the graph
    var margin = { top: 30, right: 30, bottom: 70, left: 60 },
        width = 600 - margin.left - margin.right,
        height = 500 - margin.top - margin.bottom;


    // append the svg object to the body of the page
    var svg = d3.select("#Area")
        .append("svg")
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
        .call(responsivefy)
        .append("g")
        .attr("transform",
            "translate(" + margin.left + "," + margin.top + ")");

    build(elements,width,height,max,svg);
}

// ---------------------------------- Password eye ----------------------------------
$('#togglePassword').click(function (e) {
    // Password toggle
    const password = document.querySelector('#id_password');
    // toggle the type attribute
    const type = password.getAttribute('type') === 'password' ? 'text' : 'password';
    password.setAttribute('type', type);
    // toggle the eye slash icon
    this.classList.toggle('fa-eye-slash');
});