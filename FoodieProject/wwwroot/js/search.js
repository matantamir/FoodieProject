// ****************** Basic searches ***********************
$(function () {
    $('#SearchAjaxForm').on("input",function (e) {
        e.preventDefault();

        var query = $('#query').val();


        //$('tbody').load('/Addresses/Search?query=' + query);

        $.ajax({
            //method: 'post'
            url: '/' + $(location)[0].href.split('/')[3] + '/Search',
            data: { 'query': query }

        }).done(function (data) {
            /* $('tbody').html('');
             for (var i = 0; i < data.length; i++) {
                 //var template = '<tr><td>' + data[i].City + '</td><td>' + data[i].Street + '</td><td>' + data[i].Number + '</td><td>' + data[i].Restaurant.Name + '</td></tr>';
                 var template = '<tr><td>' + data[i].city + '</td><td>' + data[i].street + '</td><td>' + data[i].number + '</td></tr>';
                 $('tbody').append(template);
             }*/

            $('tbody').html('');

            var template = $('#hidden-template').html();

            $.each(data, function (i, val) {
                var temp = template;

                $.each(val, function (key, value) {
                    temp = temp.replaceAll('{' + key + '}', value);
                });

                $('tbody').append(temp);
            });


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
        //var qRate = $('[name=qRate]');
        //var fRate = 0;
        //for (var i = 0; i < qRate.length; i++)
        //{
        //    if (qRate[i].checked)
        //    {
        //        fRate = qRate[i].value;
        //    }
        //}
        var allTags = $("[name=qTags]");
        var checkedTags = []
        for (i = 0; i < allTags.length; i++)
        {
            if (allTags[i].checked)
            {
                checkedTags.push(allTags[i].value);
            }
        }


        //$('tbody').load('/Addresses/Search?query=' + query);

        $.ajax({
            method: 'post',
            url: '/Restaurants/Search',
            data: {
                'qAddr': qAddr, 'qRest': qRest, 'qTags': checkedTags, 'qRate': qRate, 'qPrice': qPrice }
        }).done(function (data) {
            /* $('tbody').html('');
             for (var i = 0; i < data.length; i++) {
                 //var template = '<tr><td>' + data[i].City + '</td><td>' + data[i].Street + '</td><td>' + data[i].Number + '</td><td>' + data[i].Restaurant.Name + '</td></tr>';
                 var template = '<tr><td>' + data[i].city + '</td><td>' + data[i].street + '</td><td>' + data[i].number + '</td></tr>';
                 $('tbody').append(template);
             }*/

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
        e.preventDefault();
        
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
            method: 'post',
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
        e.preventDefault();

        var qDish = $('#qBasicDish').val();

        $.ajax({
            method: 'post',
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