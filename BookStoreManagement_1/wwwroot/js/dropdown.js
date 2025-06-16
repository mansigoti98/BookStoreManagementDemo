$(document).ready(function () {
    // Load countries
    $.getJSON('/Admin/GetCountries')
        .done(function (countries) {
            $('#CountryId').empty().append('<option value="">-- Select Country --</option>');
            $.each(countries, function (i, country) {
                $('#CountryId').append($('<option>', {
                    value: country.countryId,
                    text: country.name
                }));
            });
        })
        .fail(function () {
            alert('Unable to load countries.');
        });

    // Load states on country change
    $('#CountryId').change(function () {
        var countryId = $(this).val();
        $('#StateId').empty().append('<option value="">-- Select State --</option>').prop('disabled', true);
        $('#CityId').empty().append('<option value="">-- Select City --</option>').prop('disabled', true);

        if (countryId) {
            $.getJSON('/Admin/GetStates?countryId=' + countryId)
                .done(function (states) {
                    $('#StateId').prop('disabled', false);
                    $.each(states, function (i, state) {
                        $('#StateId').append($('<option>', {
                            value: state.stateId,
                            text: state.name
                        }));
                    });
                })
                .fail(function () {
                    alert('Unable to load states.');
                });
        }
    });

    // Load cities on state change
    $('#StateId').change(function () {
        var stateId = $(this).val();
        $('#CityId').empty().append('<option value="">-- Select City --</option>').prop('disabled', true);

        if (stateId) {
            $.getJSON('/Admin/GetCities?stateId=' + stateId)
                .done(function (cities) {
                    $('#CityId').prop('disabled', false);
                    $.each(cities, function (i, city) {
                        $('#CityId').append($('<option>', {
                            value: city.cityId,
                            text: city.name
                        }));
                    });
                })
                .fail(function () {
                    alert('Unable to load cities.');
                });
        }
    });
});
