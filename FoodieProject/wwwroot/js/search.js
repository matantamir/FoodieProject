$(function () {
    $('#SearchAjaxForm').submit(function (e) {
        e.preventDefault();

        var query = $('#query').val();


        //$('tbody').load('/Addresses/Search?query=' + query);

       $.ajax({
            //method: 'post'
           url: '/' + $(location)[0].href.split('/')[3] +'/Search',
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

$(function () {
    $('#SearchComplexAjaxForm').submit(function (e) {
        e.preventDefault();

        var qAddr = $('#qAddr').val();
        var qRest = $('#qRest').val();
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
                'qAddr': qAddr, 'qRest': qRest, 'qTags': checkedTags }
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
            });


        });

    });
});



