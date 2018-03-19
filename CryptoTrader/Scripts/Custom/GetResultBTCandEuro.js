$("#BTC").keyup(function () {
    $.ajax({
        url: "Trade/GetEuro",
        type: 'POST',
        data: {
            BTCAmount: $("#BTC").val().replace(".", ",")
        },
        success: function (data) {
            $("#Euro").val(data);
        }
    });
});

$("#Euro").keyup(function () {
    $.ajax({
        url: "Trade/GetBTC",
        type: 'POST',
        data: {
            EuroAmount: $("#Euro").val().replace(".", ",")
        },
        success: function (data) {
            $("#BTC").val(data);
        }
    });
});
