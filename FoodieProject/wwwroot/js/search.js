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

