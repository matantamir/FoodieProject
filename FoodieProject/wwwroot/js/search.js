// ***************** convert userType number to string (in search table)  *********
function setType() {
    $('td[name="userType"]').each(function () {
        if (this.innerHTML == "0") {
            this.innerHTML = "Admin";
        }
        else { this.innerHTML = "Client"; }
    });

}
// ****************** Basic searches ***********************
$(function () {
    $('#SearchAjaxForm').on("submit", function (z) { return false});
    $('#SearchAjaxForm').on("input", function (e) {

        var query = $('#query').val();
        var currentPage = $(location)[0].href.split('/')[3]
        $.ajax({
            url: '/' + currentPage  + '/Search',
            data: { 'query': query }

        }).done(function (data) {
            $('tbody').html('');

            var template = $('#hidden-template').html();

            $.each(data, function (i, val) {
                var temp = template;

                $.each(val, function (key, value) {
                    temp = temp.replaceAll('{' + key + '}', value);
                });

                $('tbody').append(temp);
            });

            if (currentPage == "Users") {
                setType()
            }

            if (data.length == 0) {
                $('#hiddenError2').show();
            }
            else {
                $('#hiddenError2').hide();
            }
        });
    });
});

// ****************** Restaurants searches ***********************
$(function () {
    $('#SearchComplexAjaxForm').submit(function (e) {
        e.preventDefault();

        var qAddr = $('#qAddr').val();
        var qRest = $('#qRest').val(); 
        var qPrice = $('#qPrice').val();
        var qRate = $('#qRate').val();
        var allTags = $("[name=qTags]");
        var checkedTags = []
        for (i = 0; i < allTags.length; i++)
        {
            if (allTags[i].checked)
            {
                checkedTags.push(allTags[i].value);
            }
        }

        $.ajax({
            method: 'post',
            url: '/Restaurants/Search',
            data: {
                'qAddr': qAddr, 'qRest': qRest, 'qTags': checkedTags, 'qRate': qRate, 'qPrice': qPrice }
        }).done(function (data) {
            $('#toClean').html('');
            var template = $('#hidden-template').html();

            $.each(data, function (i, val) {
                var temp = template;

                $.each(val, function (key, value) {
                    temp = temp.replaceAll('{' + key + '}', value);
                });

                $('#toClean').append(temp);
                var rateSet = "Rate " + val['restId'];
                stars = document.getElementsByName(rateSet);
                var num = val['restRate'];
                stars[5 - num].checked = true;
            });

            if (data.length == 0) {
                $('#hiddenError2').show();
            }
            else {
                $('#hiddenError2').hide();
            }
        });
    });
});

$(function () {
    $('#qBasicRest').on("input",function (e) {
        
        var qRest = $('#qBasicRest').val();

        $.ajax({
            method: 'post',
            url: '/Restaurants/Search',
            data: { 'qRest': qRest }
        }).done(function (data) {

            $('#toClean').html('');
          
            var template = $('#hidden-template').html();

            $.each(data, function (i, val) {
                var temp = template;

                $.each(val, function (key, value) {
                    temp = temp.replaceAll('{' + key + '}', value);
                });

                $('#toClean').append(temp);
                var rateSet = "Rate " + val['restId'];
                stars = document.getElementsByName(rateSet);
                var num = val['restRate'];
                stars[5 - num].checked = true;
            });

            if (data.length == 0) {
                $('#hiddenError2').show();
            }
            else {
                $('#hiddenError2').hide();
            }
        });
    });
});

$(function () {
    $('#advancedSearch').click(function () {
        $('#qBasicRest').toggle()  
    });
});

// ****************** Dish searches ***********************
$(function () {
    $('#SearchComplexAjaxFormDish').submit(function (e) {
        e.preventDefault();

        var qDish = $('#qDish').val();
        var qDescription = $('#qDescription').val();
        var qDishRest = $('#qDishRest').val();
        var qMaxPrice = $('#qMaxPrice').val();

        $.ajax({
            url: '/Dishes/Search',
            data: {
                'qDish': qDish, 'qDescription': qDescription, 'qDishRest': qDishRest, 'qMaxPrice': qMaxPrice
            }
        }).done(function (data) {
             $('tbody').html('');

            var template = $('#hidden-template').html();

            $.each(data, function (i, val) {
                var temp = template;

                $.each(val, function (key, value) {
                    temp = temp.replaceAll('{' + key + '}', value);
                });

                $('tbody').append(temp);
            });

            if (data.length == 0) {
                $('#hiddenError2').show();
            }
            else {
                $('#hiddenError2').hide();
            }
        });
    });
});

$(function () {

    $('#qBasicDish').on("input", function (e) {

        var qDish = $('#qBasicDish').val();

        $.ajax({
            url: '/Dishes/Search',
            data: { 'qDish': qDish }
        }).done(function (data) {

            $('tbody').html('');

            var template = $('#hidden-template').html();

            $.each(data, function (i, val) {
                var temp = template;

                $.each(val, function (key, value) {
                    temp = temp.replaceAll('{' + key + '}', value);
                });

                $('tbody').append(temp);
            });

            if (data.length == 0) {
                $('#hiddenError2').show();
            }
            else {
                $('#hiddenError2').hide();
            }
        });
    });
});

$(function () {
    $('#advancedSearch').click(function () {
        $('#qBasicDish').toggle()
    });
});