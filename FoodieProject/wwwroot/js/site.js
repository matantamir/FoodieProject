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


$('#togglePassword').click(function (e) {
    // Password toggle
    const password = document.querySelector('#id_password');
    // toggle the type attribute
    const type = password.getAttribute('type') === 'password' ? 'text' : 'password';
    password.setAttribute('type', type);
    // toggle the eye slash icon
    this.classList.toggle('fa-eye-slash');
});

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
        for (let i = 1; i < Math.round(place.rating); i++) {

            document.getElementById("star" + i).checked = true;
        }
        document.getElementById("Name").value = place.name;
        document.getElementById("Address_PlaceId").value = place.place_id;
        var openPeriods = 'No working hours';
        if (!!place.opening_hours.weekday_text) {
            openPeriods = "Opening hours: \n";
            openPeriods = place.opening_hours.weekday_text[6] + "\n";
            for (var i = 0; i < place.opening_hours.weekday_text.length - 1; i++) {
                openPeriods += place.opening_hours.weekday_text[i] + "\n";
            }

        }
        document.getElementById("About").value = openPeriods;
    });
}

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